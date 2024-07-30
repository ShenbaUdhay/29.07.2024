using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
//using Modules.BusinessObjects.ContractManagement;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.SamplesSite;
//using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class COCSettingsController : ViewController, IXafCallbackHandler
    {
        FileDataPropertyEditor FilePropertyEditor;
        MessageTimer timer = new MessageTimer();
        COCSettingsInfo COCInfo = new COCSettingsInfo();
        COCSettingsSampleInfo objCOCSampleinfo = new COCSettingsSampleInfo();
        DeleteObjectsViewController DeleteController;
        bool samplingfirstdefault = false;
        PermissionInfo objPermissionInfo = new PermissionInfo();
        private StaticText staticText;
        public COCSettingsController()
        {
            InitializeComponent();
            TargetViewId = "COCSettings_DetailView;" + "COCSettings_DetailView_Copy;" + "COCSettings;" + "COCSettingsSamples_ListView;" + "COCSettings_ListView;" +
                "COCTestparameter_LookupListView_Copy_SampleLogin;" + "COCTestparameter_LookupListView_Copy_SampleLogin_Copy;" +
                "COCTestparameter_LookupListView_Copy_SampleLogin_Copy_Parameter;" + "COCSettingsTest_ListView;" + "COCSettingsSamples_ListView_COCBottle;";
            Sample.TargetViewId = "COCSettings_DetailView;";
            COCSaveAs.TargetViewId = "COCSettings_DetailView;" + "COCSettings_ListView;";
            Test.TargetViewId = "COCSettingsSamples_ListView;";
            COCTestGroup.TargetViewId = "COCSettingsSamples_ListView;";
            AddSample.TargetViewId = "COCSettingsSamples_ListView;";
            CopySamples.TargetViewId = "COCSettingsSamples_ListView;";
            CopyTest.TargetViewId = "COCSettingsSamples_ListView;";
            SaveCOCSettings.TargetViewId = "COCSettings;";
        }//
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            try
            {
                objPermissionInfo.COCSettingsIsWrite = false;
                objPermissionInfo.COCSettingsIsDelete = false;
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                if (currentUser != null && View != null && View.Id != null && currentUser.Roles != null && currentUser.Roles.Count > 0)
                {
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        objPermissionInfo.COCSettingsIsWrite = true;
                        objPermissionInfo.COCSettingsIsDelete = true;
                    }
                    else
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "COCSettings" && i.Write == true) != null)
                            {
                                objPermissionInfo.COCSettingsIsWrite = true;
                                //return;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "COCSettings" && i.Delete == true) != null)
                            {
                                objPermissionInfo.COCSettingsIsDelete = true;
                                //return;
                            }
                        }
                    }
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View != null && (View.Id == "COCSettings_DetailView" || View.Id == "COCSettings_DetailView_Copy"))
                {
                    FilePropertyEditor = ((DetailView)View).FindItem("FileUpload") as FileDataPropertyEditor;
                    if (FilePropertyEditor != null)
                        FilePropertyEditor.ControlCreated += FilePropertyEditor_ControlCreated;
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "COCSettingsSamples_ListView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    DeleteController = Frame.GetController<DeleteObjectsViewController>();
                    if (DeleteController != null)
                    {
                        DeleteController.DeleteAction.Executing += DeleteAction_Executing;
                        DeleteController.DeleteAction.Executed += DeleteAction_Executed;
                        DeleteController.DeleteAction.Execute += DeleteAction_Execute;
                    }
                    if (COCTestGroup != null)
                    {
                        COCTestGroup.Active.SetItemValue("HideCOCTestGroup", false);
                    }
                }
                else
                if (View != null && View.Id == "COCSettings")
                {
                    DashboardViewItem COCDetailView = ((DashboardView)View).FindItem("COCSettings") as DashboardViewItem;
                    DashboardViewItem SampleListView = ((DashboardView)View).FindItem("Samples") as DashboardViewItem;
                    DashboardViewItem TestRegListView = ((DashboardView)View).FindItem("COCSettingsTest") as DashboardViewItem;

                    if (COCDetailView != null)
                    {
                        COCDetailView.ControlCreated += COCDetailView_ControlCreated;
                    }
                    if (SampleListView != null)
                    {
                        SampleListView.ControlCreated += SampleListView_ControlCreated;
                    }
                    if (TestRegListView != null)
                    {
                        TestRegListView.ControlCreated += TestRegListView_ControlCreated;
                    }
                }
                if (View != null && View.Id == "COCSettingsSamples_ListView_COCBottle")
                {
                    samplingfirstdefault = true;
                }
                //if (View.Id == "COCTest")
                //{
                //    staticText = (StaticText)((DashboardView)this.View).FindItem("SampleMatrix");
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null & (View.Id == "COCSettings_DetailView") || View.Id == "COCSettings_DetailView_Copy")
                {
                    if (e.PropertyName == "Client")
                    {
                        COCSettings objCOC = e.Object as COCSettings;
                        if (objCOC.ClientName != null)
                        {
                            IList<Project> lstProject = ObjectSpace.GetObjects<Project>(CriteriaOperator.Parse("[customername.Oid] = ?", objCOC.ClientName.Oid));
                            ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null)
                            {
                                if (lstProject.Count > 0)
                                {
                                    propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstProject.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")");
                                }
                                else
                                {
                                    propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                                }
                                propertyEditor.RefreshDataSource();
                            }
                        }
                    }
                }
                else if (View.Id == "COCSettingsSamples_ListView")
                {
                    if (e.PropertyName == "SiteName")
                    {
                        COCSettingsSamples objCOC = e.Object as COCSettingsSamples;
                        if (objCOC != null && objCOC.StationLocation != null)
                        {
                            if (objCOC.StationLocation.Qty > 0)
                            {
                                objCOC.Qty = objCOC.StationLocation.Qty;
                            }
                            objCOC.Collector = objCOC.StationLocation.Collector;
                            objCOC.CollectorPhone = objCOC.StationLocation.CollectorPhone;
                            objCOC.SampleDescription = objCOC.StationLocation.Description;
                            objCOC.SamplingLocation = objCOC.StationLocation.SamplingAddress;
                            objCOC.Blended = objCOC.StationLocation.Blended;
                            objCOC.SamplingLocation = objCOC.StationLocation.SamplingAddress;
                            if (objCOC.StationLocation.PWSSystemName != null)
                            {
                                objCOC.PWSSystemName = objCOC.StationLocation.PWSSystemName;
                            }
                            if (objCOC.StationLocation != null && objCOC.StationLocation.SystemType != null)
                            {
                                objCOC.SystemType = objCOC.StationLocation.SystemType;
                            }
                            objCOC.PWSID = objCOC.StationLocation.PWSID;
                            objCOC.KeyMap = objCOC.StationLocation.KeyMap;
                            objCOC.ServiceArea = objCOC.StationLocation.ServiceArea;
                            objCOC.IsActive = objCOC.StationLocation.IsActive;
                            objCOC.City = objCOC.StationLocation.City;
                            objCOC.State = objCOC.StationLocation.State;
                            objCOC.ZipCode = objCOC.StationLocation.ZipCode;
                            objCOC.FacilityID = objCOC.StationLocation.FacilityID;
                            objCOC.FacilityName = objCOC.StationLocation.FacilityName;
                            objCOC.FacilityType = objCOC.StationLocation.FacilityType;
                            objCOC.SamplePointType = objCOC.StationLocation.SamplePointType;
                            objCOC.WaterType = objCOC.StationLocation.WaterType;
                            objCOC.Longitude = objCOC.StationLocation.Longitude;
                            objCOC.Latitude = objCOC.StationLocation.Latitude;
                            objCOC.MonitoryingRequirement = objCOC.StationLocation.MonitoryingRequirement;
                            objCOC.ParentSampleID = objCOC.StationLocation.ParentSampleID;
                            objCOC.ParentSampleDate = objCOC.StationLocation.ParentSampleDate;
                            objCOC.RepeatLocation = objCOC.StationLocation.RepeatLocation;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                NestedFrame nestedFrame = (NestedFrame)Frame;
                if (nestedFrame != null)
                {
                    CompositeView view = nestedFrame.ViewItem.View;
                    foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                    {
                        if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                        {
                            //frameContainer.Frame.View.ObjectSpace.Refresh();
                            if (frameContainer.Frame.View is DetailView)
                            {
                                frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                            }
                            else
                            {
                                (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                            }
                            frameContainer.Frame.View.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                //if (View != null && View.Id == "COCSettingsSamples_ListView" && View.SelectedObjects.Count > 0)
                //{
                //    IObjectSpace os = Application.CreateObjectSpace();
                //    foreach (COCSettingsSamples sample in View.SelectedObjects)
                //    {
                //        IList<COCSettingsTest> lstTests = os.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid]=?", sample.Oid));
                //        foreach (COCSettingsTest test in lstTests.ToList())
                //        {
                //            os.Delete(test);
                //        }
                //    }
                //    os.CommitChanges();
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                //if (e.PopupFrame.View.Id == "COCTest")
                //{
                //    e.Width = new System.Web.UI.WebControls.Unit(1400);
                //    e.Height = new System.Web.UI.WebControls.Unit(648);
                //    e.Handled = true;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TestRegListView_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardView dv = (DashboardView)Application.MainWindow.View;
                DashboardViewItem SamplesListView = (DashboardViewItem)dv.FindItem("COCSettingsTest");
                if (SamplesListView != null && SamplesListView.InnerView != null && COCInfo.TestAssigned == false && COCInfo.COCOid != null)
                {
                    COCInfo.TestAssigned = true;
                    ((ListView)SamplesListView.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Samples.COC.Oid] = ?", COCInfo.COCOid);

                    ((ListView)SamplesListView.InnerView).ObjectSpace.Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SampleListView_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardView dv = (DashboardView)Application.MainWindow.View;
                DashboardViewItem SamplesListView = (DashboardViewItem)dv.FindItem("Samples");
                if (SamplesListView != null && SamplesListView.InnerView != null && COCInfo.SampleAssigned == false && COCInfo.COCOid != null)//Samples.COC.Oid
                {
                    ((ListView)SamplesListView.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[COC.Oid] = ?", COCInfo.COCOid);
                    //((ListView)SamplesListView.InnerView).ObjectSpace.Refresh();
                    COCInfo.SampleAssigned = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void COCDetailView_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardView dv = (DashboardView)Application.MainWindow.View;
                DashboardViewItem COCDetailView = (DashboardViewItem)dv.FindItem("COCSettings");
                if (COCDetailView != null && COCDetailView.InnerView != null && COCInfo.COCAssigned == false && COCInfo.COCOid != null)
                {
                    ((DetailView)COCDetailView.InnerView).CurrentObject = ((DetailView)COCDetailView.InnerView).ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Setting.COCSettings>(COCInfo.COCOid);
                    //((DetailView)COCDetailView.InnerView).ViewEditMode = ViewEditMode.Edit;
                    if (objPermissionInfo.COCSettingsIsWrite == true && objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit)
                    {
                        ((DetailView)COCDetailView.InnerView).ViewEditMode = ViewEditMode.Edit;
                    }
                    else
                    {
                        ((DetailView)COCDetailView.InnerView).ViewEditMode = ViewEditMode.View;
                    }
                    ((DetailView)COCDetailView.InnerView).ObjectSpace.Refresh();
                    COCInfo.COCAssigned = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FilePropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                FileDataEdit FileControl = ((FileDataPropertyEditor)sender).Editor;
                if (FileControl != null)
                    FileControl.UploadControlCreated += control_UploadControlCreated;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void control_UploadControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxUploadControl FileUploadControl = ((FileDataEdit)sender).UploadControl;
                string[] arrExtensions = ConfigurationManager.AppSettings["AllowedExtensions"].Split(',').Select(p => p.Trim()).ToArray();
                //FileUploadControl.ValidationSettings.AllowedFileExtensions = new String[] { ".jpg", ".png", ".pdf", ".xls", ".xlt" };
                FileUploadControl.ValidationSettings.AllowedFileExtensions = arrExtensions;
                FileUploadControl.ValidationSettings.NotAllowedFileExtensionErrorText = "Unsupported File format.";
                FileUploadControl.ValidationSettings.MaxFileSize = 20000000;
                FileUploadControl.ValidationSettings.MaxFileSizeErrorText = "File size shouldn't be greater than 20MB.";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            try
            {
                if (View.Id == "COCSettings_ListView")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor.Grid.Columns["InlineEditCommandColumn"] != null)
                    {
                        editor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                    }
                }
                if (View != null && (View.Id == "COCSettings_DetailView" || View.Id == "COCSettings_DetailView_Copy"))
                {
                    COCSettings objCOC = View.CurrentObject as COCSettings;
                    if (objCOC != null && objCOC.ClientName != null)
                    {
                        IList<Project> lstProject = ObjectSpace.GetObjects<Project>(CriteriaOperator.Parse("[customername.Oid] = ?", objCOC.ClientName.Oid));
                        ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Control == null)
                        {
                            propertyEditor.CreateControl();
                        }
                        if (propertyEditor != null && propertyEditor.CollectionSource != null)
                        {
                            if (lstProject.Count > 0)
                            {
                                propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstProject.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")");
                            }
                            else
                            {
                                propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                            }
                            propertyEditor.RefreshDataSource();
                        }
                    }
                    else
                    {
                        ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Control == null)
                        {
                            propertyEditor.CreateControl();
                        }
                        if (propertyEditor != null && propertyEditor.CollectionSource != null)
                        {
                            propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                            propertyEditor.RefreshDataSource();
                        }
                    }
                    if (View.Id == "COCSettings_DetailView")
                    {
                        objPermissionInfo.COCSettingsViewEditMode = ((DetailView)View).ViewEditMode;
                        //COCSettings objCOC = View.CurrentObject as COCSettings;
                        //if (objCOC.Client != null)
                        //{
                        //    IList<Project> lstProject = ObjectSpace.GetObjects<Project>(CriteriaOperator.Parse("[customername.Oid] = ?", objCOC.Client.Oid));
                        //    ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                        //    if (propertyEditor != null)
                        //    {
                        //        if (lstProject.Count > 0)
                        //        {
                        //            propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstProject.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")");
                        //            View.Refresh();
                        //        }
                        //        else
                        //        {
                        //            propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                        //            View.Refresh();
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                        //    if (propertyEditor != null)
                        //    {
                        //        propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("1=2");
                        //    }
                        //}
                    }

                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                System.Globalization.CultureInfo objServerCulture = System.Globalization.CultureInfo.CurrentCulture;
                                if (objServerCulture != null)
                                {
                                    propertyEditor.DisplayFormat = objServerCulture.DateTimeFormat.ShortDatePattern;
                                    propertyEditor.EditMask = objServerCulture.DateTimeFormat.ShortDatePattern;
                                }
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(FileDataPropertyEditor))
                        {
                            FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                        {
                            ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                    }
                }
                else if (View.Id == "COCSettingsTest_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.Clear();
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["ShowCOCSettingsTestDelete"] = objPermissionInfo.COCSettingsIsDelete;
                }
                else if (View.Id == "COCSettingsSamples_ListView")
                {
                    ICallbackManagerHolder sampleid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    sampleid.CallbackManager.RegisterHandler("id", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Load += Grid_Load1;
                    gridListEditor.Grid.JSProperties["cpsuboutremove"] = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Suboutremove");
                    gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    gridListEditor.Grid.HtmlCommandCellPrepared += Grid_HtmlCommandCellPrepared;
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active.Clear();
                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["ShowCOCSettingsSamplesDelete"] = objPermissionInfo.COCSettingsIsDelete;

                    if (objPermissionInfo.COCSettingsIsWrite == false || objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.View)
                    {
                        AddSample.Active["ShowAddSample"] = false;
                        CopySamples.Active["ShowCopySample"] = false;
                        CopyTest.Active["ShowCopyTest"] = false;
                        Test.Active["ShowTest"] = false;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e){
                e.cancel = true;
                }";
                    }
                    else if (objPermissionInfo.COCSettingsIsWrite == true && objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit)
                    {
                        AddSample.Active["ShowAddSample"] = true;
                        CopySamples.Active["ShowCopySample"] = true;
                        CopyTest.Active["ShowCopyTest"] = true;
                        Test.Active["ShowTest"] = true;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e){
                if(e.focusedColumn.fieldName == 'VisualMatrix.Oid' && s.batchEditApi.GetCellValue(e.visibleIndex, 'VisualMatrix') != null){
                s.GetRowValues(e.visibleIndex, 'Test;Oid', OnGetRowValues);
                }
                }";
                    }

                    //gridListEditor.Grid.ClientSideEvents.CustomButtonClick = @"function(s,e)
                    //{     
                    //    if(e.buttonID == 'COCTest')
                    //    {alert(e.buttonID);
                    //        if(s.batchEditApi.GetCellValue(e.visibleIndex, 'VisualMatrix') != null)
                    //        {
                    //            if (s.batchEditApi.HasChanges())
                    //            {
                    //                s.UpdateEdit(); 
                    //            }
                    //            var value = s.batchEditApi.GetCellValue(e.visibleIndex, 'SampleNo') + '|' + s.batchEditApi.GetCellValue(e.visibleIndex, 'VisualMatrix') + '|' + s.GetRowKey(e.visibleIndex);
                    //            RaiseXafCallback(globalCallbackControl, 'id', value, '', false);                 
                    //        }
                    //        else
                    //        {
                    //            RaiseXafCallback(globalCallbackControl, 'id', 'error', '', false);                  
                    //        }
                    //    }
                    //}";

                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s,e){
                    s.UpdateEdit();
                }";

                    string strActionScript = @"
                                                            window.setTimeout(function () {
                                                            Grid.UpdateEdit();
                                                            //Grid.Refresh();
                                                            }, 10);
                                                        ";

                    AddSample.SetClientScript(strActionScript);
                    CopySamples.SetClientScript(strActionScript);
                    CopyTest.SetClientScript(strActionScript);
                    //Test.SetClientScript(strActionScript);

                }
                //else if (View.Id == "COCTest" && COCInfo.IsTestcanFilter)
                //{
                //    COCInfo.IsTestcanFilter = false;
                //    List<object> groups = new List<object>();
                //    DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
                //    DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
                //    DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
                //    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Testparameter)))
                //    {
                //        string criteria = string.Empty;
                //        if (COCInfo.lstdupfilterstr != null && COCInfo.lstdupfilterstr.Count > 0)
                //        {
                //            foreach (string test in COCInfo.lstdupfilterstr)
                //            {
                //                var testsplit = test.Split('|');
                //                IList<Testparameter> testparameters = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("([TestMethod.TestName] ='" + testsplit[0] + "' and [TestMethod.MethodName.MethodNumber] ='" + testsplit[1] + "' and [TestMethod.MatrixName.MatrixName] ='" + testsplit[2] + "' and [Component.Components] ='" + testsplit[3] + "')"));
                //                if (criteria == string.Empty)
                //                {
                //                    criteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                //                }
                //                else
                //                {
                //                    criteria = criteria + "and Not [Oid] In(" + string.Format("'{0}'", string.Join("','", testparameters.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                //                }
                //            }
                //            lstview.Criteria = CriteriaOperator.Parse(criteria);
                //        }
                //        lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.TestName", true, true));
                //        lstview.Properties.Add(new ViewProperty("TMethodName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MethodName.MethodNumber", true, true));
                //        //lstview.Properties.Add(new ViewProperty("TMethodName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MethodName.MethodName", true, true));
                //        lstview.Properties.Add(new ViewProperty("TMatrixName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MatrixName.MatrixName", true, true));
                //        lstview.Properties.Add(new ViewProperty("TComponentName", DevExpress.Xpo.SortDirection.Ascending, "Component.Components", true, true));
                //        lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                //        foreach (ViewRecord rec in lstview)
                //            groups.Add(rec["Toid"]);
                //        if (COCInfo.lstTestParameter != null && COCInfo.lstTestParameter.Count > 0)
                //        {
                //            if (COCInfo.lstdupfilterguid != null && COCInfo.lstdupfilterguid.Count > 0)
                //            {
                //                foreach (Guid guid in COCInfo.lstdupfilterguid)
                //                {
                //                    groups.Add(guid);
                //                }
                //            }
                //            ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                //            ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                //            ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                //            ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                //        }
                //        else
                //        {
                //            ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = new InOperator("Oid", groups);
                //            ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                //        }

                //        ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                //    }
                //    if (COCInfo.strvisualmatrix != null && staticText != null)
                //    {
                //        staticText.Text = COCInfo.strvisualmatrix;
                //    }
                //}
                else if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Testparameter) && View.Id == "COCTestparameter_LookupListView_Copy_SampleLogin")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    if (string.IsNullOrEmpty(COCInfo.strViewID))
                    {
                        // Modules.BusinessObjects.Setting.COCSettingsSamples sample = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Setting.COCSettingsSamples>(COCInfo.SampleOid);
                        // if (sample != null && sample.VisualMatrix != null && sample.VisualMatrix.MatrixName != null)
                        // {
                        //     //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("'" + sample.VisualMatrix.MatrixName.MatrixName + "'==[TestMethod.MatrixName.MatrixName] AND [TestMethod.GCRecord] IS NULL AND (([TestMethod.RetireDate] IS NULL OR [TestMethod.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')) AND " +
                        //     //               " ([TestMethod.MethodName.RetireDate] IS NULL OR [TestMethod.MethodName.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "') AND ([Parameter.RetireDate] IS NULL OR [Parameter.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" + "AND([RetireDate] IS NULL OR[RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" +
                        //     //                "AND ([InternalStandard] == False or [InternalStandard] IS NULL ) AND ([Surroagate] == False or [Surroagate] IS NULL)");
                        //     ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName]='" + sample.VisualMatrix.MatrixName.MatrixName + "' AND [TestMethod.GCRecord] IS NULL AND (([TestMethod.RetireDate] IS NULL OR [TestMethod.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')) AND " +
                        //" ([TestMethod.MethodName.RetireDate] IS NULL OR [TestMethod.MethodName.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "') AND ([Parameter.RetireDate] IS NULL OR [Parameter.RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" + "AND([RetireDate] IS NULL OR[RetireDate] > '" + DateTime.Now.Date.ToString("MM/dd/yyyy") + "')" +
                        // "AND ([InternalStandard] == False or [InternalStandard] IS NULL ) AND ([Surroagate] == False or [Surroagate] IS NULL)");
                        // }
                        // else
                        // {
                        //     ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is NULL");
                        // }
                    }
                    else
                    {
                        //List<Guid> lstParams = new List<Guid>();
                        //Guid TestGroupName;
                        //Guid SLOid = new Guid();
                        //if (View != null)
                        //{
                        //    COCSettingsSamples settingsSamples = ObjectSpace.GetObjectByKey<COCSettingsSamples>(COCInfo.SampleOid);
                        //    if (settingsSamples != null)
                        //    {
                        //        foreach (GroupTest obj in COCInfo.lstBottleID)
                        //        {
                        //            if (obj != null)
                        //            {
                        //                TestGroupName = obj.Oid;
                        //                CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + settingsSamples.Oid + "'");
                        //                COCSettingsSamples objSL = ObjectSpace.FindObject<COCSettingsSamples>(criteria);
                        //                CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]='" + TestGroupName + "'");
                        //                IList<GroupTest> objTP = ObjectSpace.GetObjects<GroupTest>(criteria1);
                        //                SLOid = objSL.Oid;
                        //                if (objTP != null)
                        //                {
                        //                    foreach (GroupTest tp in objTP)
                        //                    {
                        //                        foreach (TestMethod testmethod in tp.TestMethods)
                        //                        {
                        //                            if ((testmethod.RetireDate == DateTime.MinValue || testmethod.RetireDate > DateTime.Now) && (testmethod.MethodName.RetireDate
                        //                                == DateTime.MinValue || testmethod.MethodName.RetireDate > DateTime.Now))
                        //                            {
                        //                                foreach (Testparameter testparam in testmethod.TestParameter)
                        //                                {
                        //                                    if (objSL != null && !objSL.Testparameters.Contains(testparam))
                        //                                    {
                        //                                        if (testparam.TestMethod.MatrixName.MatrixName == settingsSamples.VisualMatrix.MatrixName.MatrixName)
                        //                                        {
                        //                                            lstParams.Add(testparam.Oid);
                        //                                            //testparam.TestGroup = tp.TestGroupName;
                        //                                            //objSL.Testparameters.Add(testparam);
                        //                                        }
                        //                                    }
                        //                                }
                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                        //if (lstParams != null && lstParams.Count > 0)
                        //{
                        //    string strCriteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", lstParams.Select(i => i.ToString().Replace("'", "''")))) + ")";

                        //    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(strCriteria); 
                        //}
                        //else
                        //{
                        //    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is NULL");
                        //}

                        //if (COCInfo.lstBottleID != null && COCInfo.lstBottleID.Count > 0)
                        //{
                        //    IList<GroupTest> lstGT = ObjectSpace.GetObjects<GroupTest>(CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstBottleID.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")"));
                        //    if (lstGT != null && lstGT.Count > 0)
                        //    {
                        //        //IList<TestMethod> lstTM = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstGT.Select(i => i.TestMethods.ToString().Replace("'", "''")))) + ")"));
                        //        string strCriteria = string.Empty;
                        //        foreach (GroupTest tp in lstGT)
                        //        {
                        //            foreach (TestMethod testmethod in tp.TestMethods)
                        //            {
                        //                if ((testmethod.RetireDate == DateTime.MinValue || testmethod.RetireDate > DateTime.Now) && (testmethod.MethodName.RetireDate
                        //                    == DateTime.MinValue || testmethod.MethodName.RetireDate > DateTime.Now))
                        //                {
                        //                    if (string.IsNullOrEmpty(strCriteria))
                        //                    {
                        //                        strCriteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", testmethod.TestParameter.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                        //                    }
                        //                    else
                        //                    {
                        //                        strCriteria = strCriteria + "and [Oid] In(" + string.Format("'{0}'", string.Join("','", testmethod.TestParameter.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                        //                    }
                        //                }
                        //            }
                        //        }

                        //        if (!string.IsNullOrEmpty(strCriteria))
                        //        {
                        //            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(strCriteria);
                        //        }
                        //        else
                        //        {
                        //            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is NULL");
                        //        }
                        //    }
                        //}
                    }
                }
                else if (View.Id == "COCTestparameter_LookupListView_Copy_SampleLogin_Copy")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                    gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    ICallbackManagerHolder seltest = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    string script = seltest.CallbackManager.GetScript();
                    script = string.Format(CultureInfo.InvariantCulture, @"
                        function(s, e) {{ 
                            var xafCallback = function() {{
                            s.EndCallback.RemoveHandler(xafCallback);
                            {0}
                            }};
                            s.EndCallback.AddHandler(xafCallback);
                        }}
                    ", script);
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = script;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e)
                    { 
                    s.SetWidth(400); 
                    s.RowClick.ClearHandlers();
                    }";
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                }
                else if (View.Id == "COCTestparameter_LookupListView_Copy_SampleLogin_Copy_Parameter")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("COCTest", this);
                    gridListEditor.Grid.SettingsPager.AlwaysShowPager = true;
                    gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e){ 
                    s.SetWidth(400); 
                    s.RowClick.ClearHandlers();
                    }";
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e){
                      if(e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                        if (s.IsRowSelectedOnPage(e.visibleIndex)) {   
                            var value = 'Testselection|Selected|' + Oidvalue;
                            RaiseXafCallback(globalCallbackControl, 'COCTest', value, '', false);    
                        }else{
                            var value = 'Testselection|UNSelected|' + Oidvalue;
                            RaiseXafCallback(globalCallbackControl, 'COCTest', value, '', false);    
                        }
                      });
                     }
                     else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                     {        
                        RaiseXafCallback(globalCallbackControl, 'COCTest', 'Testselection|Selectall', '', false);                        
                     }   
                     else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                     {
                        RaiseXafCallback(globalCallbackControl, 'COCTest', 'Testselection|UNSelectall', '', false);                        
                     }  
                    }";
                }
                else if (View.Id == "COCSettings")
                {
                    if (objPermissionInfo.COCSettingsIsWrite == false || objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.View)
                    {
                        SaveCOCSettings.Active["ShowCOCSetting"] = false;
                    }
                    else if (objPermissionInfo.COCSettingsIsWrite == false && objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.Edit)
                    {
                        SaveCOCSettings.Active["ShowCOCSetting"] = true;
                    }
                }
                else if (View != null && View.Id == "COCSettingsSamples_ListView_COCBottle")
                {
                    ASPxGridListEditor gridlist = ((ListView)View).Editor as ASPxGridListEditor;
                    if (samplingfirstdefault == true)
                    {
                        //COCSettingsSamples objsmpl = ((ListView)View).CollectionSource.List.Cast<COCSettingsSamples>().FirstOrDefault();
                        //if (objsmpl != null)
                        //{
                        //    COCInfo.SamplingGuid = objsmpl.Oid;
                        //    DashboardViewItem DVBotallocation = ((DashboardView)Application.MainWindow.View).FindItem("BottleAllocation") as DashboardViewItem;
                        //    if (DVBotallocation != null && DVBotallocation.InnerView != null)
                        //    {
                        //        ((ListView)DVBotallocation.InnerView).CollectionSource.Criteria.Clear();
                        //        ((ListView)DVBotallocation.InnerView).CollectionSource.Criteria["criteria"] = new InOperator("COCSettings.Oid", objsmpl.Oid);
                        //    }
                        //}
                        //samplingfirstdefault = false;
                    }
                    if (gridlist != null && gridlist.Grid != null)
                    {
                        gridlist.Grid.Load += Grid_Load;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                if (View.Id == "COCSettingsSamples_ListView" && (objPermissionInfo.COCSettingsIsWrite == false || objPermissionInfo.COCSettingsViewEditMode == ViewEditMode.View))
                {
                    if (e.CommandCellType == GridViewTableCommandCellType.Data && e.CommandColumn.Name == "COCTest")
                    {
                        ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_Load1(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "COCSettingsSamples_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    //var os = Application.CreateObjectSpace();
                    //Session currentSession = ((XPObjectSpace)(os)).Session;
                    //SelectedData sproc = currentSession.ExecuteSproc("getCurrentLanguage", "");
                    //var CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                    curlanguage strCurrentLanguage = new curlanguage();
                    if (strCurrentLanguage.strcurlanguage == "En")
                    {
                        if (gridView.Columns["COCTest"] != null)
                        {
                            gridView.Columns["COCTest"].Caption = "Test";
                            gridView.Columns["COCTest"].Width = 65;
                        }
                        if (gridView.Columns["COCTestGroup"] != null)
                        {
                            gridView.Columns["COCTestGroup"].Caption = "TestGroup";
                            gridView.Columns["COCTestGroup"].Width = 65;
                        }
                        gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                        //gridView.Columns[30].Caption = "TestGroup";
                        //gridView.Columns[30].Width = 85;
                        //gridView.Columns[31].Caption = "Test";
                        //gridView.Columns[31].Width = 65;
                        //gridView.Columns[32].Caption = "BottleId";
                        //gridView.Columns[32].Width = 65;

                    }
                    else
                    {
                        if (gridView.Columns["COCTest"] != null)
                        {
                            gridView.Columns["COCTest"].Caption = "检测项目";
                            gridView.Columns["COCTest"].Width = 65;
                        }
                        if (gridView.Columns["COCTestGroup"] != null)
                        {
                            gridView.Columns["COCTestGroup"].Caption = "监测项目组合";
                            gridView.Columns["COCTestGroup"].Width = 65;
                        }
                        //gridView.Columns[30].Caption = "监测项目组合";
                        //gridView.Columns[30].Width = 85;
                        //gridView.Columns[31].Caption = "检测项目";
                        //gridView.Columns[31].Width = 65;
                        //gridView.Columns[32].Caption = "样瓶编号";
                        //gridView.Columns[32].Width = 65;
                    }
                    CriteriaOperator cs = CriteriaOperator.Parse("Oid=?", COCInfo.COCOid);
                    COCSettings objTasks = ObjectSpace.FindObject<COCSettings>(cs);
                    if (gridListEditor != null && objTasks != null)
                    {
                        List<SampleMatrixSetupFields> lstFields = new List<SampleMatrixSetupFields>();
                        if (!string.IsNullOrEmpty(objTasks.SampleMatries))
                        {
                            List<string> lstSMOid = objTasks.SampleMatries.Split(';').ToList();
                            //lstFields = objsamplecheckin.SampleMatries.SetupFields.ToList();
                            foreach (string strOid in lstSMOid)
                            {
                                VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));
                                if (objVM != null && objVM.SetupFields.Count > 0)
                                {
                                    foreach (SampleMatrixSetupFields objField in objVM.SetupFields)
                                    {
                                        if (lstFields.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                        {
                                            lstFields.Add(objField);
                                        }
                                    }
                                }
                            }
                        }

                        //VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(objTasks.);
                        //if (objVM != null && objVM.SetupFields.Count > 0)
                        //{
                        //    foreach (SampleMatrixSetupFields objField in objVM.SetupFields)
                        //    {
                        //        if (lstFields.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                        //        {
                        //            lstFields.Add(objField);
                        //        }
                        //    }
                        //}

                        foreach (WebColumnBase column in gridView.Columns)
                        {
                            if (column.Name == "SelectionCommandColumn" || column.Name == "COCTest")
                            {
                                gridView.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (column.Caption == "SamplingLocation")
                            {
                                gridView.VisibleColumns["SamplingLocation"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            else
                            {
                                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                if (columnInfo != null)
                                {
                                    SampleMatrixSetupFields curField = lstFields.FirstOrDefault(i => i.FieldID == columnInfo.Model.Id);
                                    if (curField != null)
                                    {
                                        column.Visible = true;
                                        if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                                        {

                                            column.Caption = curField.FieldCustomCaption;
                                        }
                                        else
                                        {
                                            column.Caption = curField.FieldCaption.Replace(" ", "");
                                        }
                                        if (curField.SortOrder > 0)
                                        {
                                            column.VisibleIndex = curField.SortOrder + 3;
                                        }
                                        if (curField.Freeze)
                                        {
                                            gridView.VisibleColumns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                                        }
                                        if (curField.Width > 0)
                                        {
                                            column.Width = curField.Width;
                                        }
                                    }
                                    else
                                    {
                                        if (columnInfo.Model.Id != "SampleID" && columnInfo.Model.Id != "SampleName" && columnInfo.Model.Id != "VisualMatrix" && columnInfo.Model.Id != "ClientSampleID"
                                             && columnInfo.Model.Id != "SamplingLocation" && columnInfo.Model.Id != "CollectDate" && columnInfo.Model.Id != "CollectTimeDisplay" && columnInfo.Model.Id != "SiteName")
                                        {
                                            column.Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                e.Properties["cpVisibleRowCount"] = gridView.VisibleRowCount;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (COCInfo.lstTestParameter != null && COCInfo.lstTestParameter.Count > 0 && COCInfo.strSelectionMode == "Selected")
                {
                    foreach (Guid obj in COCInfo.lstTestParameter)
                    {
                        gridView.Selection.SelectRowByKey(obj);
                    }
                    COCInfo.strSelectionMode = string.Empty;
                }
                if (View != null && View.Id == "COCSettingsSamples_ListView_COCBottle")
                {
                    if (((ListView)View).CollectionSource.List.Count == 1)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            gridView.Selection.SelectRow(i);
                        }
                    }
                    else if (((ListView)View).CollectionSource.List.Count > 1)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            if (samplingfirstdefault == true)
                            {
                                i = 1;
                                break;
                                gridView.Selection.SelectRow(i);
                                samplingfirstdefault = false;
                            }
                            else if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "SampleID").ToString()))
                            {
                                //string strbottleid = gridView.GetRowValues(i, "SampleID").ToString();
                                //COCSettingsSamples objsmpling = ObjectSpace.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SamplingGuid));
                                //if (objsmpling != null && objsmpling.SampleID == strbottleid)
                                //{
                                //    gridView.Selection.SelectRow(i);
                                //}
                            }
                        }
                    }
                    gridView.Selection.SelectRowByKey(COCInfo.SamplingGuid);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "COCTestparameter_LookupListView_Copy_SampleLogin_Copy")
                {
                    Testparameter testparameter = (Testparameter)View.CurrentObject;
                    DashboardViewItem TestViewSubChild = ((NestedFrame)Frame).ViewItem.View.FindItem("TestViewSubChild") as DashboardViewItem;
                    if (testparameter != null && TestViewSubChild != null && COCInfo.UseSelchanged)
                    {
                        if (testparameter.IsGroup != true)
                        {
                            //((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.TestName] = ? And [TestMethod.MethodName.MethodName] = ? And [TestMethod.MatrixName.MatrixName] = ? And [QCType.QCTypeName] = 'Sample'", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodName, testparameter.TestMethod.MatrixName.MatrixName);
                            ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [TestMethod.MatrixName.MatrixName] = ? And [QCType.QCTypeName] = 'Sample'And [Component.Components]=?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components);
                            COCInfo.strSelectionMode = "Selected";
                        }
                    }
                    else
                    {
                        COCInfo.UseSelchanged = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
                {
                    if ((View.Id == "COCSettingsSamples_ListView" || View.Id == "COCSettingsTest_ListView") && objPermissionInfo.COCSettingsIsWrite == false && objPermissionInfo.COCSettingsIsDelete == false)
                    {
                        e.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                if (View != null && (View.Id == "COCSettings_DetailView" || View.Id == "COCSettings_DetailView_Copy"))
                {
                    FilePropertyEditor = ((DetailView)View).FindItem("FileUpload") as FileDataPropertyEditor;
                    if (FilePropertyEditor != null)
                        FilePropertyEditor.ControlCreated -= FilePropertyEditor_ControlCreated;
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                else if (View != null && View.Id == "COCSettings")
                {
                    DashboardViewItem COCDetailView = ((DashboardView)View).FindItem("COCSettings") as DashboardViewItem;
                    DashboardViewItem SampleListView = ((DashboardView)View).FindItem("Samples") as DashboardViewItem;
                    DashboardViewItem TestRegListView = ((DashboardView)View).FindItem("COCSettingsTest") as DashboardViewItem;

                    if (COCDetailView != null)
                    {
                        COCDetailView.ControlCreated -= COCDetailView_ControlCreated;
                    }
                    if (SampleListView != null)
                    {
                        SampleListView.ControlCreated -= SampleListView_ControlCreated;
                    }
                    if (TestRegListView != null)
                    {
                        TestRegListView.ControlCreated -= TestRegListView_ControlCreated;
                    }
                }
                else if (View != null && View.Id == "COCSettingsSamples_ListView")
                {
                    DeleteController = Frame.GetController<DeleteObjectsViewController>();
                    if (DeleteController != null)
                    {
                        DeleteController.DeleteAction.Executing -= DeleteAction_Executing;
                        DeleteController.DeleteAction.Executed -= DeleteAction_Executed;
                        DeleteController.DeleteAction.Execute -= DeleteAction_Execute;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Sample_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "COCSettings_DetailView")
                {
                    COCInfo.COCAssigned = false;
                    COCInfo.SampleAssigned = false;
                    COCInfo.TestAssigned = false;
                    ObjectSpace.CommitChanges();
                    Modules.BusinessObjects.Setting.COCSettings objCOC = (Modules.BusinessObjects.Setting.COCSettings)e.CurrentObject;
                    if (objCOC != null)
                    {
                        //COCInfo.strcocID = objCOC.COC_ID;
                        //COCInfo.COCOid = objCOC.Oid;
                        //IList<Modules.BusinessObjects.Setting.COCSettingsSamples> lstSamples = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.COCSettingsSamples>(CriteriaOperator.Parse("[COC.Oid] = ?", objCOC.Oid));
                        //if (lstSamples != null && lstSamples.Count == 0)
                        //{
                        //    Modules.BusinessObjects.Setting.COCSettingsSamples newSample = ObjectSpace.CreateObject<Modules.BusinessObjects.Setting.COCSettingsSamples>();
                        //    newSample.SampleNo = "01";
                        //    newSample.ClientSampleID = "";
                        //    newSample.COC = objCOC;
                        //    newSample.VisualMatrix = objCOC.SampleMatrix;
                        //    ObjectSpace.CommitChanges();
                        //}
                        //Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "COCSettings", false));
                        //View.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Test_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //if (View != null && View.Id == "COCSettingsSamples_ListView")
                //{
                //    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                //    COCSettingsSamples currentSamples = ObjectSpace.GetObjectByKey<COCSettingsSamples>(((COCSettingsSamples)e.CurrentObject).Oid);
                //    if (currentSamples != null && currentSamples.VisualMatrix != null)
                //    //if (COCInfo.strSampleID== "error")
                //    {
                //        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                //        COCInfo.strvisualmatrix = currentSamples.VisualMatrix.VisualMatrixName;
                //        WebWindow.CurrentRequestWindow.RegisterClientScript("console", "console.clear();");
                //        Modules.BusinessObjects.Setting.COCSettingsSamples sample = (Modules.BusinessObjects.Setting.COCSettingsSamples)e.CurrentObject;
                //        DashboardView dashboard = Application.CreateDashboardView(ObjectSpace, "COCTest", false);
                //        ShowViewParameters showViewParameters = new ShowViewParameters(dashboard);
                //        showViewParameters.Context = TemplateContext.NestedFrame;
                //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                //        showViewParameters.CreatedView.Closed += CreatedView_Closed;
                //        DialogController dc = Application.CreateController<DialogController>();
                //        dc.SaveOnAccept = false;
                //        dc.AcceptAction.Active.SetItemValue("disable", false);
                //        dc.CancelAction.Active.SetItemValue("disable", false);
                //        dc.CloseOnCurrentObjectProcessing = false;
                //        //dc.Accepting += Dc_Accepting;
                //        showViewParameters.Controllers.Add(dc);
                //        CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse(""));
                //        if (currentLanguage.Chinese == true)
                //        {
                //            showViewParameters.CreatedView.Caption = "选择检测项目 - " + sample.SampleNo;
                //        }
                //        else
                //        {
                //            showViewParameters.CreatedView.Caption = "Test Assignment - " + sample.SampleNo;
                //        }

                //        COCInfo.SampleOid = sample.Oid;
                //        COCInfo.lstTestParameter = new List<Guid>();
                //        COCInfo.lstSavedTestParameter = new List<Guid>();
                //        COCInfo.lstdupfilterguid = new List<Guid>();
                //        COCInfo.lstdupfilterstr = new List<string>();
                //        COCInfo.strViewID = string.Empty;

                //        IList<Modules.BusinessObjects.Setting.COCSettingsTest> objsample = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.COCSettingsTest>(CriteriaOperator.Parse("Samples.Oid=?", sample.Oid));
                //        if (objsample != null && objsample.Count > 0)
                //        {
                //            foreach (Modules.BusinessObjects.Setting.COCSettingsTest objTest in objsample.ToList())
                //            {
                //                if (objTest.IsGroup != true)
                //                {
                //                    if (!COCInfo.lstTestParameter.Contains(objTest.Testparameter.Oid))
                //                    {
                //                        if (objTest.Testparameter.TestMethod != null && objTest.Testparameter.TestMethod.MethodName != null && objTest.Testparameter.TestMethod.MatrixName != null && objTest.Testparameter.Component != null)
                //                        {
                //                            if (!COCInfo.lstdupfilterstr.Contains(objTest.Testparameter.TestMethod.TestName + "|" + objTest.Testparameter.TestMethod.MethodName.MethodNumber + "|" + objTest.Testparameter.TestMethod.MatrixName.MatrixName + "|" + objTest.Testparameter.Component.Components))
                //                            {
                //                                COCInfo.lstdupfilterstr.Add(objTest.Testparameter.TestMethod.TestName + "|" + objTest.Testparameter.TestMethod.MethodName.MethodNumber + "|" + objTest.Testparameter.TestMethod.MatrixName.MatrixName + "|" + objTest.Testparameter.Component.Components);
                //                                COCInfo.lstdupfilterguid.Add(objTest.Testparameter.Oid);
                //                            }
                //                        }
                //                        //if (!COCInfo.lstdupfilterstr.Contains(objTest.Testparameter.TestMethod.TestName + "|" + objTest.Testparameter.TestMethod.MethodName.MethodName + "|" + objTest.Testparameter.TestMethod.MatrixName.MatrixName))
                //                        //{
                //                        //    COCInfo.lstdupfilterstr.Add(objTest.Testparameter.TestMethod.TestName + "|" + objTest.Testparameter.TestMethod.MethodName.MethodName + "|" + objTest.Testparameter.TestMethod.MatrixName.MatrixName);
                //                        //    COCInfo.lstdupfilterguid.Add(objTest.Testparameter.Oid);
                //                        //}
                //                        COCInfo.lstSavedTestParameter.Add(objTest.Testparameter.Oid);
                //                        COCInfo.lstTestParameter.Add(objTest.Testparameter.Oid);
                //                    }
                //                }
                //                else if (objTest.IsGroup == true)
                //                {
                //                    GroupTestMethod objgtm = ObjectSpace.FindObject<GroupTestMethod>(CriteriaOperator.Parse("[Oid] =?", objTest.GroupTest.Oid));
                //                    if (objgtm != null && objgtm.TestMethod != null && objgtm.TestMethod.Oid != null)
                //                    {
                //                        IList<Testparameter> testparameters = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", objgtm.TestMethod.Oid));
                //                        foreach (Testparameter objtp in testparameters.ToList())
                //                        {
                //                            if (!COCInfo.lstdupfilterguid.Contains(objtp.Oid))
                //                            {
                //                                COCInfo.lstdupfilterguid.Add(objtp.Oid);
                //                            }
                //                            if (!COCInfo.lstSavedTestParameter.Contains(objtp.Oid))
                //                            {
                //                                COCInfo.lstSavedTestParameter.Add(objtp.Oid);
                //                            }
                //                            if (!COCInfo.lstTestParameter.Contains(objtp.Oid))
                //                            {
                //                                COCInfo.lstTestParameter.Add(objtp.Oid);
                //                            }
                //                        }
                //                    }
                //                }

                //            }
                //        }
                //        COCInfo.IsTestcanFilter = true;
                //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectmatrix"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CreatedView_Closed(object sender, EventArgs e)
        {
            try
            {
                NestedFrame nestedFrame = (NestedFrame)Frame;
                if (nestedFrame != null)
                {
                    CompositeView view = nestedFrame.ViewItem.View;
                    foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                    {
                        if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                        {
                            //frameContainer.Frame.View.ObjectSpace.Refresh();
                            if (frameContainer.Frame.View is DetailView)
                            {
                                frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                            }
                            else
                            {
                                (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                            }
                            frameContainer.Frame.View.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AddSample_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "COCSettingsSamples_ListView" && COCInfo != null && COCInfo.COCOid != null)
                {
                    Modules.BusinessObjects.Setting.COCSettings cocSettings = View.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Setting.COCSettings>(COCInfo.COCOid);
                    //if (cocSettings != null)
                    //{
                    //    Modules.BusinessObjects.Setting.COCSettingsSamples cocSample = View.ObjectSpace.CreateObject<Modules.BusinessObjects.Setting.COCSettingsSamples>();
                    //    cocSample.COC = cocSettings;
                    //    SelectedData sproc = ((XPObjectSpace)View.ObjectSpace).Session.ExecuteSproc("GetCOCSampleNo", new OperandValue(cocSettings.COC_ID));
                    //    if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                    //    {
                    //        string strID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                    //        if (strID.Length == 1)
                    //        {
                    //            strID = 0 + strID;
                    //        }
                    //        cocSample.SampleNo = strID;
                    //    }
                    //    //CriteriaOperator criteria = CriteriaOperator.Parse("[COC.Oid] = ?", COCInfo.COCOid);
                    //    //CriteriaOperator expression = CriteriaOperator.Parse("Max(SampleNo)");
                    //    //string strID = (Convert.ToInt32(((XPObjectSpace)View.ObjectSpace).Session.Evaluate(typeof(Modules.BusinessObjects.Setting.COCSettingsSamples), expression, criteria)) + 1).ToString();
                    //    //cocSample.SampleNo = strID;
                    //    View.ObjectSpace.CommitChanges();
                    //    ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(cocSample));
                    //    View.Refresh();
                    //}
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (parameter != "error")
                {
                    string[] values = parameter.Split('|');
                    if (values[0] == "Testselection")
                    {
                        if (values[1] == "Selected")
                        {
                            Guid curguid = new Guid(values[2]);
                            COCInfo.strSelectionMode = values[1];
                            if (!COCInfo.lstTestParameter.Contains(curguid))
                            {
                                COCInfo.lstTestParameter.Add(curguid);
                            }

                            if (COCInfo.lstRemovedTestParameter == null)
                            {
                                COCInfo.lstRemovedTestParameter = new List<Guid>();
                            }
                            if (COCInfo.lstRemovedTestParameter.Contains(curguid))
                            {
                                COCInfo.lstRemovedTestParameter.Remove(curguid);
                            }
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            Testparameter testparameter = ObjectSpace.GetObjectByKey<Testparameter>(curguid);
                            DashboardViewItem TestViewMain = ((DashboardView)view).FindItem("TestViewMain") as DashboardViewItem;
                            DashboardViewItem TestViewSub = ((DashboardView)view).FindItem("TestViewSub") as DashboardViewItem;
                            DashboardViewItem TestViewSubChild = ((DashboardView)view).FindItem("TestViewSubChild") as DashboardViewItem;
                            bool Oidchange = true;
                            Guid curusedguid = new Guid();
                            foreach (Testparameter objtestparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List)
                            {
                                if (objtestparameter.Oid == testparameter.Oid)
                                {
                                    Oidchange = false;
                                }
                                if (Oidchange && objtestparameter.TestMethod.TestName == testparameter.TestMethod.TestName && objtestparameter.TestMethod.MethodName.MethodNumber == testparameter.TestMethod.MethodName.MethodNumber)
                                {
                                    curusedguid = objtestparameter.Oid;
                                }
                            }
                            if (Oidchange && TestViewSubChild != null && TestViewSubChild.InnerView.SelectedObjects.Count == 1)
                            {
                                Testparameter addnewtestparameter = (Testparameter)TestViewSubChild.InnerView.SelectedObjects[0];
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] =
                                CriteriaOperator.Parse(((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"].ToString().Replace(curusedguid.ToString(), addnewtestparameter.Oid.ToString()));
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ASPxGridListEditor gridListEditor = ((ListView)TestViewSub.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    COCInfo.UseSelchanged = false;
                                    gridListEditor.Grid.Selection.SelectRowByKey(addnewtestparameter.Oid);
                                }
                            }
                        }
                        else if (values[1] == "UNSelected")
                        {
                            Guid curguid = new Guid(values[2]);
                            COCInfo.strSelectionMode = values[1];
                            if (COCInfo.lstTestParameter.Contains(curguid))
                            {
                                COCInfo.lstTestParameter.Remove(curguid);
                            }
                            if (COCInfo.lstRemovedTestParameter == null)
                            {
                                COCInfo.lstRemovedTestParameter = new List<Guid>();
                            }
                            if (!COCInfo.lstRemovedTestParameter.Contains(curguid))
                            {
                                COCInfo.lstRemovedTestParameter.Add(curguid);
                            }
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            Testparameter testparameter = ObjectSpace.GetObjectByKey<Testparameter>(curguid);
                            DashboardViewItem TestViewMain = ((DashboardView)view).FindItem("TestViewMain") as DashboardViewItem;
                            DashboardViewItem TestViewSub = ((DashboardView)view).FindItem("TestViewSub") as DashboardViewItem;
                            DashboardViewItem TestViewSubChild = ((DashboardView)view).FindItem("TestViewSubChild") as DashboardViewItem;
                            Testparameter addnewtestparameter = null;
                            foreach (Testparameter objtestparameter in ((ListView)TestViewSub.InnerView).CollectionSource.List)
                            {
                                if (testparameter != null && objtestparameter.Oid == testparameter.Oid)
                                {
                                    if (TestViewSubChild != null && TestViewSubChild.InnerView.SelectedObjects.Count > 0)
                                    {
                                        addnewtestparameter = (Testparameter)TestViewSubChild.InnerView.SelectedObjects[0];
                                    }
                                }
                            }
                            if (addnewtestparameter != null)
                            {
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"] = ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["dupfilter"] =
                                CriteriaOperator.Parse(((ListView)TestViewMain.InnerView).CollectionSource.Criteria["dupfilter"].ToString().Replace(curguid.ToString(), addnewtestparameter.Oid.ToString()));
                                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
                                ASPxGridListEditor gridListEditor = ((ListView)TestViewSub.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    COCInfo.UseSelchanged = false;
                                    gridListEditor.Grid.Selection.SelectRowByKey(addnewtestparameter.Oid);
                                }
                            }
                        }
                        else if (values[1] == "Selectall")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                                {
                                    Guid curguid = new Guid(editor.Grid.GetRowValues(i, "Oid").ToString());
                                    COCInfo.strSelectionMode = "Selected";
                                    if (!COCInfo.lstTestParameter.Contains(curguid))
                                    {
                                        COCInfo.lstTestParameter.Add(curguid);
                                    }

                                    if (COCInfo.lstRemovedTestParameter == null)
                                    {
                                        COCInfo.lstRemovedTestParameter = new List<Guid>();
                                    }
                                    if (COCInfo.lstRemovedTestParameter.Contains(curguid))
                                    {
                                        COCInfo.lstRemovedTestParameter.Remove(curguid);
                                    }
                                }
                            }
                        }
                        else if (values[1] == "UNSelectall")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                for (int i = 0; i < editor.Grid.VisibleRowCount; i++)
                                {
                                    Guid curguid = new Guid(editor.Grid.GetRowValues(i, "Oid").ToString());
                                    COCInfo.strSelectionMode = "UNSelected";
                                    if (COCInfo.lstTestParameter.Contains(curguid))// && !COCInfo.lstSavedTestParameter.Contains(curguid))
                                    {
                                        COCInfo.lstTestParameter.Remove(curguid);
                                    }
                                    if (COCInfo.lstRemovedTestParameter == null)
                                    {
                                        COCInfo.lstRemovedTestParameter = new List<Guid>();
                                    }
                                    if (!COCInfo.lstRemovedTestParameter.Contains(curguid))
                                    {
                                        COCInfo.lstRemovedTestParameter.Add(curguid);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    COCInfo.strSampleID = "error";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void TestSelectionAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
        //        DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
        //        DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
        //        if (TestViewMain != null && ((ListView)TestViewMain.InnerView).SelectedObjects.Count > 0)
        //        {
        //            foreach (Testparameter testparameter in ((ListView)TestViewMain.InnerView).SelectedObjects)
        //            {
        //                if (testparameter.IsGroup != true)
        //                {
        //                    //IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodName]=? and [TestMethod.MatrixName.MatrixName] = ?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodName, testparameter.TestMethod.MatrixName.MatrixName));
        //                    IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and Component.Components=?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
        //                    foreach (Testparameter test in listseltest)
        //                    {
        //                        if (!COCInfo.lstTestParameter.Contains(test.Oid))
        //                        {
        //                            COCInfo.lstTestParameter.Add(test.Oid);
        //                        }
        //                    }
        //                }
        //                else if (testparameter.IsGroup == true)
        //                {
        //                    IList<Testparameter> listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MatrixName.MatrixName] = ?", testparameter.TestMethod.TestName, testparameter.TestMethod.MatrixName.MatrixName));
        //                    foreach (Testparameter test in listseltest)
        //                    {
        //                        if (!COCInfo.lstTestParameter.Contains(test.Oid))
        //                        {
        //                            COCInfo.lstTestParameter.Add(test.Oid);
        //                        }
        //                    }
        //                }

        //            }
        //            if (TestViewSub != null && COCInfo.lstTestParameter != null && COCInfo.lstTestParameter.Count > 0)
        //            {
        //                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
        //                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
        //                ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
        //            }
        //            List<object> groups = new List<object>();
        //            using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Testparameter)))
        //            {
        //                string criteria = string.Empty;
        //                if (TestViewSub != null && COCInfo.lstTestParameter != null && COCInfo.lstTestParameter.Count > 0)
        //                {
        //                    criteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")";
        //                }
        //                lstview.Criteria = CriteriaOperator.Parse(criteria);
        //                lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.TestName", true, true));
        //                lstview.Properties.Add(new ViewProperty("TMethodName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MethodName.MethodNumber", true, true));
        //                lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
        //                foreach (ViewRecord rec in lstview)
        //                    groups.Add(rec["Toid"]);
        //                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", groups.Select(i => i.ToString().Replace("'", "''")))) + ")");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void TestSelectionRemove_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
        //        DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
        //        DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
        //        if (TestViewMain != null && TestViewSub != null && ((ListView)TestViewSub.InnerView).SelectedObjects.Count > 0)
        //        {
        //            if (COCInfo.lstTestParameter == null)
        //            {
        //                COCInfo.lstTestParameter = new List<Guid>();
        //            }
        //            if (COCInfo.lstRemovedTestParameter == null)
        //            {
        //                COCInfo.lstRemovedTestParameter = new List<Guid>();
        //            }
        //            foreach (Testparameter testparameter in ((ListView)TestViewSub.InnerView).SelectedObjects)
        //            {
        //                IList<Testparameter> listseltest = new List<Testparameter>();
        //                if (testparameter.IsGroup != true)
        //                {
        //                    //listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodName]=? and [TestMethod.MatrixName.MatrixName] = ?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodName, testparameter.TestMethod.MatrixName.MatrixName));
        //                    listseltest = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.TestName]=? and [TestMethod.MethodName.MethodNumber]=? and [TestMethod.MatrixName.MatrixName] = ? and [Component.Components]=?", testparameter.TestMethod.TestName, testparameter.TestMethod.MethodName.MethodNumber, testparameter.TestMethod.MatrixName.MatrixName, testparameter.Component.Components));
        //                }
        //                else
        //                {
        //                    if (COCInfo.lstTestParameter.Contains(testparameter.Oid))
        //                    {
        //                        COCInfo.lstTestParameter.Remove(testparameter.Oid);
        //                    }
        //                    IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", testparameter.TestMethod.Oid));
        //                    foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
        //                    {
        //                        IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
        //                        foreach (Testparameter paramgtm in lsttestpara.ToList())
        //                        {
        //                            listseltest.Add(paramgtm);
        //                        }
        //                    }
        //                }
        //                foreach (Testparameter test in listseltest)
        //                {
        //                    if (COCInfo.lstTestParameter.Contains(test.Oid))
        //                    {
        //                        COCInfo.lstTestParameter.Remove(test.Oid);
        //                    }
        //                    if (!COCInfo.lstRemovedTestParameter.Contains(test.Oid))
        //                    {
        //                        COCInfo.lstRemovedTestParameter.Add(test.Oid);
        //                    }
        //                }
        //            }
        //            if (COCInfo.lstTestParameter.Count != 0 && TestViewSubChild != null)
        //            {
        //                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("Not [Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
        //                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")");
        //                ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");

        //                List<object> groups = new List<object>();
        //                using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Testparameter)))
        //                {
        //                    string criteria = string.Empty;
        //                    if (TestViewSub != null && COCInfo.lstTestParameter != null && COCInfo.lstTestParameter.Count > 0)
        //                    {
        //                        criteria = "[Oid] In(" + string.Format("'{0}'", string.Join("','", COCInfo.lstTestParameter.Select(i => i.ToString().Replace("'", "''")))) + ")";
        //                    }
        //                    lstview.Criteria = CriteriaOperator.Parse(criteria);
        //                    lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.TestName", true, true));
        //                    lstview.Properties.Add(new ViewProperty("TMethodName", DevExpress.Xpo.SortDirection.Ascending, "TestMethod.MethodName.MethodNumber", true, true));
        //                    lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
        //                    foreach (ViewRecord rec in lstview)
        //                        groups.Add(rec["Toid"]);
        //                    ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", groups.Select(i => i.ToString().Replace("'", "''")))) + ")");
        //                }
        //            }
        //            else
        //            {
        //                ((ListView)TestViewMain.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("");
        //                ((ListView)TestViewSub.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
        //                ((ListView)TestViewSubChild.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void TestSelectionSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        DashboardViewItem TestViewMain = ((DashboardView)View).FindItem("TestViewMain") as DashboardViewItem;
        //        DashboardViewItem TestViewSub = ((DashboardView)View).FindItem("TestViewSub") as DashboardViewItem;
        //        DashboardViewItem TestViewSubChild = ((DashboardView)View).FindItem("TestViewSubChild") as DashboardViewItem;
        //        //if ((COCInfo.lstTestParameter != null && COCInfo.lstTestParameter.Count > 0) || (COCInfo.lstRemovedTestParameter != null && COCInfo.lstRemovedTestParameter.Count > 0))
        //        //{
        //        //    Modules.BusinessObjects.Setting.COCSettingsSamples sampleLog = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.COCSettingsSamples>(CriteriaOperator.Parse("[Oid] = ?", COCInfo.SampleOid));
        //        //    if (COCInfo.lstRemovedTestParameter != null && COCInfo.lstRemovedTestParameter.Count > 0)
        //        //    {
        //        //        IObjectSpace os = Application.CreateObjectSpace();
        //        //        foreach (Guid objTestParameter in COCInfo.lstRemovedTestParameter)
        //        //        {
        //        //            Testparameter param = os.GetObjectByKey<Testparameter>(objTestParameter);
        //        //            COCSettingsTest objsmpltest = os.FindObject<COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid]=? and [Testparameter.Oid]=?", sampleLog.Oid, param.Oid));
        //        //            if (objsmpltest != null)
        //        //            {
        //        //                os.Delete(objsmpltest);
        //        //                os.CommitChanges();
        //        //            }
        //        //        }
        //        //        COCInfo.lstRemovedTestParameter.Clear();
        //        //    }
        //        //    if (COCInfo.lstTestParameter != null && COCInfo.lstTestParameter.Count > 0)
        //        //    {
        //        //        Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
        //        //        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
        //        //        foreach (Guid objtestparameter in COCInfo.lstTestParameter)
        //        //        {
        //        //            Testparameter testparameter = ObjectSpace.GetObjectByKey<Testparameter>(objtestparameter);
        //        //            if (testparameter.IsGroup != true)
        //        //            {
        //        //                if (testparameter != null && testparameter.QCType != null && testparameter.QCType.QCTypeName == "Sample")
        //        //                {
        //        //                    Modules.BusinessObjects.Setting.COCSettingsTest sample = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid]=? and [Testparameter.Oid]=?", sampleLog.Oid, objtestparameter));
        //        //                    if (sample == null)
        //        //                    {
        //        //                        Modules.BusinessObjects.Setting.COCSettingsTest newsample = ObjectSpace.CreateObject<Modules.BusinessObjects.Setting.COCSettingsTest>();
        //        //                        newsample.Samples = sampleLog;
        //        //                        newsample.Testparameter = testparameter;
        //        //                        //sampleLog.Test = true; 
        //        //                    }
        //        //                }
        //        //            }
        //        //            else if (testparameter.IsGroup == true)
        //        //            {
        //        //                IList<GroupTestMethod> lstgrouptestmed = ObjectSpace.GetObjects<GroupTestMethod>(CriteriaOperator.Parse("[TestMethod.Oid] = ?", testparameter.TestMethod.Oid));
        //        //                foreach (GroupTestMethod objgtm in lstgrouptestmed.ToList())
        //        //                {
        //        //                    IList<Testparameter> lsttestpara = ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.Oid] = ? And [QCType.QCTypeName] = 'Sample' And [Component.Components] = 'Default'", objgtm.TestParameter.TestMethod.Oid));
        //        //                    foreach (Testparameter param1 in lsttestpara.ToList())
        //        //                    {
        //        //                        if (testparameter != null && param1 != null && param1.QCType != null && param1.QCType.QCTypeName == "Sample")
        //        //                        {
        //        //                            Modules.BusinessObjects.Setting.COCSettingsTest sample = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid]=? and [Testparameter.Oid]=?", sampleLog.Oid, param1.Oid));
        //        //                            if (sample == null)
        //        //                            {
        //        //                                Modules.BusinessObjects.Setting.COCSettingsTest newsample = ObjectSpace.CreateObject<Modules.BusinessObjects.Setting.COCSettingsTest>();
        //        //                                newsample.Samples = sampleLog;
        //        //                                newsample.Testparameter = param1;
        //        //                                newsample.GroupTest = ObjectSpace.GetObjectByKey<GroupTestMethod>(objgtm.Oid);
        //        //                                newsample.IsGroup = true;
        //        //                                ObjectSpace.CommitChanges();
        //        //                            }
        //        //                        }
        //        //                    }
        //        //                }
        //        //            }
        //        //        }
        //        //        TestViewMain.InnerView.ObjectSpace.CommitChanges();
        //        //        TestViewSub.InnerView.ObjectSpace.CommitChanges();
        //        //        ObjectSpace.CommitChanges();
        //        //        AssignBottleAllocationToSamples(uow, COCInfo.strcocID, COCInfo.SampleOid);
        //        //    }
        //        //    (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void AssignBottleAllocationToSamples(UnitOfWork uow, string strcocID, Guid sampleOid)
        {
            //try
            //{
            //    COCSettingsSamples objSample = uow.GetObjectByKey<COCSettingsSamples>(sampleOid);
            //    SampleBottleAllocation objSampleBottle = uow.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("COCSettings=?", sampleOid));
            //    if (objSample != null && objSampleBottle == null)
            //    {
            //        XPClassInfo sampleParameterinfo;
            //        sampleParameterinfo = uow.GetClassInfo(typeof(COCSettingsTest));
            //        IList<COCSettingsTest> objSampleParameters = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samples.Oid]=?", objSample.Oid), null, int.MaxValue, false, true).Cast<COCSettingsTest>().ToList();
            //        if (objSampleParameters.Count > 0)
            //        {
            //            string testName = string.Empty;
            //            List<string> lstTest = objSampleParameters.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.TestName != null).Select(i => i.Testparameter.TestMethod.TestName).Distinct().ToList();
            //            if (lstTest.Count > 0)
            //            {
            //                foreach (string objString in lstTest)
            //                {
            //                    if (string.IsNullOrEmpty(testName))
            //                    {
            //                        testName = objString;
            //                    }
            //                    else
            //                    {
            //                        testName = testName + ";" + objString;
            //                    }
            //                }
            //                SampleBottleAllocation objBottleAllocation = uow.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings]=?", objSample.Oid));
            //                if (objBottleAllocation == null)
            //                {
            //                    SampleBottleAllocation objNewBottle = new SampleBottleAllocation(uow);
            //                    objNewBottle.COCSettings = objSample;
            //                    objNewBottle.BottleSet = 1;
            //                    objNewBottle.BottleID = "A";
            //                    objNewBottle.Qty = 1;
            //                    objNewBottle.SharedTests = testName;
            //                    List<Guid> lstContainer = new List<Guid>();
            //                    List<Guid> lstPreservative = new List<Guid>();
            //                    List<Guid> lstTestOid = objSampleParameters.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).ToList();
            //                    foreach (Guid objTestMeOid in lstTestOid)
            //                    {
            //                        TestMethod objTM = uow.FindObject<TestMethod>(CriteriaOperator.Parse("Oid=?", objTestMeOid));
            //                        if (objTM != null)
            //                        {
            //                            IList<Guid> containerNames = objTM.TestGuides.Where(i => i.Container != null).Select(i => i.Container.Oid).ToList();
            //                            if (containerNames != null && containerNames.Count > 0)
            //                            {
            //                                foreach (Guid objContainer in containerNames)
            //                                {
            //                                    if (!lstContainer.Contains(objContainer))
            //                                    {
            //                                        lstContainer.Add(objContainer);
            //                                    }
            //                                }
            //                            }
            //                            IList<Guid> Preservative = objTM.TestGuides.Where(i => i.Preservative != null).Select(i => i.Preservative.Oid).ToList();
            //                            if (Preservative != null && Preservative.Count > 0)
            //                            {
            //                                foreach (Guid objPreservative in Preservative)
            //                                {
            //                                    if (!lstPreservative.Contains(objPreservative))
            //                                    {
            //                                        lstPreservative.Add(objPreservative);
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                    if (lstContainer.Count == 1)
            //                    {
            //                        Modules.BusinessObjects.Setting.Container objContainer = uow.FindObject<Modules.BusinessObjects.Setting.Container>(CriteriaOperator.Parse("Oid=?", lstContainer[0]));
            //                        if (objContainer != null)
            //                        {
            //                            objNewBottle.Containers = objContainer;
            //                        }
            //                    }
            //                    if (lstPreservative.Count == 1)
            //                    {
            //                        Preservative objpreservative = uow.FindObject<Preservative>(CriteriaOperator.Parse("Oid=?", lstPreservative[0]));
            //                        if (objpreservative != null)
            //                        {
            //                            objNewBottle.Preservative = objpreservative;
            //                        }
            //                    }
            //                    uow.CommitChanges();
            //                }
            //            }
            //        }
            //    }
            //    else if (objSampleBottle != null)
            //    {
            //        XPClassInfo BottleAllocationInfo;
            //        BottleAllocationInfo = uow.GetClassInfo(typeof(SampleBottleAllocation));
            //        IList<SampleBottleAllocation> objBottleAllocation = uow.GetObjects(BottleAllocationInfo, CriteriaOperator.Parse("[COCSettings]=?", sampleOid), null, int.MaxValue, false, true).Cast<SampleBottleAllocation>().ToList();
            //        if (objBottleAllocation.Count == 1)
            //        {
            //            XPClassInfo sampleParameterinfo;
            //            sampleParameterinfo = uow.GetClassInfo(typeof(COCSettingsTest));
            //            IList<COCSettingsTest> objSampleParameters = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samples]=?", objSample.Oid), null, int.MaxValue, false, true).Cast<COCSettingsTest>().ToList();
            //            if (objSampleParameters.Count > 0)
            //            {
            //                string testName = string.Empty;
            //                List<string> lstTest = objSampleParameters.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.TestName != null).Select(i => i.Testparameter.TestMethod.TestName).Distinct().ToList();
            //                if (lstTest.Count > 0)
            //                {
            //                    foreach (string objString in lstTest)
            //                    {
            //                        if (string.IsNullOrEmpty(testName))
            //                        {
            //                            testName = objString;
            //                        }
            //                        else
            //                        {
            //                            testName = testName + "; " + objString;
            //                        }
            //                    }

            //                    SampleBottleAllocation objBottle = uow.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid]=?", objBottleAllocation[0]));
            //                    if (objBottle != null)
            //                    {
            //                        objBottle.SharedTests = testName;
            //                        uow.CommitChanges();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }

        private void CopySamples_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(Modules.BusinessObjects.SampleManagement.SL_CopyNoOfSamples));
                if (objToShow != null)
                {
                    DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
                    CreateDetailView.ViewEditMode = ViewEditMode.Edit;
                    e.View = CreateDetailView;
                }
                if (View.SelectedObjects.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                else if (View.SelectedObjects.Count > 1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopySamples_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                //if (e.CurrentObject != null && e.CurrentObject.GetType() == typeof(COCSettingsSamples))
                //{
                //    COCSettingsSamples currentSample = (COCSettingsSamples)e.CurrentObject;
                //    Modules.BusinessObjects.SampleManagement.SL_CopyNoOfSamples objSamplesCount = (Modules.BusinessObjects.SampleManagement.SL_CopyNoOfSamples)e.PopupWindowViewCurrentObject;
                //    if (currentSample != null && objSamplesCount != null)
                //    {
                //        for (int i = 1; i <= objSamplesCount.NoOfSamples; i++)
                //        {
                //            IObjectSpace os = Application.CreateObjectSpace();
                //            currentSample = os.GetObject<COCSettingsSamples>((COCSettingsSamples)e.CurrentObject);
                //            Modules.BusinessObjects.Setting.COCSettings cocSettings = os.GetObjectByKey<Modules.BusinessObjects.Setting.COCSettings>(COCInfo.COCOid);
                //            if (cocSettings != null)
                //            {
                //                string strID = string.Empty;
                //                SelectedData sproc = ((XPObjectSpace)os).Session.ExecuteSproc("GetCOCSampleNo", new OperandValue(cocSettings.COC_ID));
                //                if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                //                {
                //                    strID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                //                    if (strID.Length == 1)
                //                    {
                //                        strID = 0 + strID;
                //                    }
                //                }
                //                COCSettingsSamples cocSample = os.FindObject<COCSettingsSamples>(CriteriaOperator.Parse("[SampleNo] = ? AND [COC.Oid] = ?", strID, cocSettings.Oid));
                //                if (cocSample == null && !string.IsNullOrEmpty(strID))
                //                {
                //                    cocSample = os.CreateObject<COCSettingsSamples>();
                //                    cocSample.COC = cocSettings;
                //                    cocSample.SampleNo = strID;
                //                    cocSample.ClientSampleID = currentSample.ClientSampleID;

                //                    if (currentSample.VisualMatrix != null)
                //                    {
                //                        cocSample.VisualMatrix = currentSample.VisualMatrix;
                //                    }

                //                    if (currentSample.Testparameters != null && currentSample.Testparameters.Count > 0)
                //                    {
                //                        foreach (Testparameter testparameter in currentSample.Testparameters)
                //                        {
                //                            cocSample.Testparameters.Add(os.GetObject<Testparameter>(testparameter));
                //                        }
                //                    }
                //                    if (currentSample.AssignedBy != null)
                //                    {
                //                        cocSample.AssignedBy = currentSample.AssignedBy;

                //                    }
                //                    cocSample.AssignTo = currentSample.AssignTo;
                //                    cocSample.CollectDate = currentSample.CollectDate;
                //                    cocSample.CollectTime = currentSample.CollectTime;

                //                    if (currentSample.Collector != null)
                //                    {
                //                        cocSample.Collector = currentSample.Collector;
                //                    }
                //                    cocSample.Comment = currentSample.Comment;


                //                    cocSample.EquipmentName = currentSample.EquipmentName;

                //                    if (currentSample.Preservetives != null)
                //                    {
                //                        cocSample.Preservetives = currentSample.Preservetives;
                //                    }
                //                    if (currentSample.QCSource != null)
                //                    {
                //                        cocSample.QCSource = currentSample.QCSource;
                //                    }
                //                    if (currentSample.QCType != null)
                //                    {
                //                        cocSample.QCType = currentSample.QCType;
                //                    }
                //                    cocSample.Qty = currentSample.Qty;
                //                    cocSample.SampleName = currentSample.SampleName;
                //                    if (currentSample.SampleType != null)
                //                    {
                //                        cocSample.SampleType = currentSample.SampleType;
                //                    }
                //                    if (currentSample.Storage != null)
                //                    {
                //                        cocSample.Storage = currentSample.Storage;
                //                    }
                //                    cocSample.CollectDate = currentSample.CollectDate;
                //                    cocSample.CollectTime = currentSample.CollectTime;
                //                    //cocSample.FlowRate = currentSample.FlowRate;
                //                    cocSample.TimeStart = currentSample.TimeStart;
                //                    cocSample.TimeEnd = currentSample.TimeEnd;
                //                    //cocSample.Time = currentSample.Time;
                //                    //cocSample.Volume = currentSample.Volume;
                //                    //cocSample.Address = currentSample.Address;
                //                    //cocSample.AreaOrPerson = currentSample.AreaOrPerson;
                //                    cocSample.BalanceID = currentSample.BalanceID;
                //                    cocSample.AssignTo = currentSample.AssignTo;
                //                    //cocSample.Barp = currentSample.Barp;
                //                    cocSample.BatchID = currentSample.BatchID;
                //                    cocSample.BatchSize = currentSample.BatchSize;
                //                    cocSample.BatchSize_pc = currentSample.BatchSize_pc;
                //                    cocSample.BatchSize_Units = currentSample.BatchSize_Units;
                //                    cocSample.Blended = currentSample.Blended;
                //                    cocSample.BottleQty = currentSample.BottleQty;
                //                    //cocSample.BuriedDepthOfGroundWater = currentSample.BuriedDepthOfGroundWater;
                //                    //cocSample.ChlorineFree = currentSample.ChlorineFree;
                //                    //cocSample.ChlorineTotal = currentSample.ChlorineTotal;
                //                    cocSample.City = currentSample.City;
                //                    cocSample.CollectorPhone = currentSample.CollectorPhone;
                //                    //cocSample.CollectTimeDisplay = currentSample.CollectTimeDisplay;
                //                    //cocSample.CompositeQty = currentSample.CompositeQty;
                //                    //cocSample.DateEndExpected = currentSample.DateEndExpected;
                //                    //cocSample.DateStartExpected = currentSample.DateStartExpected;
                //                    //cocSample.Depth = currentSample.Depth;
                //                    //cocSample.Description = currentSample.Description;
                //                    //cocSample.DischargeFlow = currentSample.DischargeFlow;
                //                    //cocSample.DischargePipeHeight = currentSample.DischargePipeHeight;
                //                    //cocSample.DO = currentSample.DO;
                //                    //cocSample.DueDate = currentSample.DueDate;
                //                    //cocSample.Emission = currentSample.Emission;
                //                    //cocSample.EndOfRoad = currentSample.EndOfRoad;
                //                    //cocSample.EquipmentModel = currentSample.EquipmentModel;
                //                    cocSample.EquipmentName = currentSample.EquipmentName;
                //                    cocSample.FacilityID = currentSample.FacilityID;
                //                    cocSample.FacilityName = currentSample.FacilityName;
                //                    cocSample.FacilityType = currentSample.FacilityType;
                //                    cocSample.FinalForm = currentSample.FinalForm;
                //                    cocSample.FinalPackaging = currentSample.FinalPackaging;
                //                    //cocSample.FlowRate = currentSample.FlowRate;
                //                    //cocSample.FlowRateCubicMeterPerHour = currentSample.FlowRateCubicMeterPerHour;
                //                    //cocSample.FlowRateLiterPerMin = currentSample.FlowRateLiterPerMin;
                //                    //cocSample.FlowVelocity = currentSample.FlowVelocity;
                //                    //cocSample.ForeignMaterial = currentSample.ForeignMaterial;
                //                    //cocSample.Frequency = currentSample.Frequency;
                //                    //cocSample.GISStatus = currentSample.GISStatus;
                //                    //cocSample.GravelContent = currentSample.GravelContent;
                //                    cocSample.GrossWeight = currentSample.GrossWeight;
                //                    //cocSample.GroupSample = currentSample.GroupSample;
                //                    //cocSample.Hold = currentSample.Hold;
                //                    //cocSample.Humidity = currentSample.Humidity;
                //                    //cocSample.IceCycle = currentSample.IceCycle;
                //                    cocSample.Increments = currentSample.Increments;
                //                    cocSample.Interval = currentSample.Interval;
                //                    cocSample.IsActive = currentSample.IsActive;
                //                    cocSample.IsNotTransferred = currentSample.IsNotTransferred;
                //                    cocSample.ItemName = currentSample.ItemName;
                //                    cocSample.KeyMap = currentSample.KeyMap;
                //                    cocSample.LicenseNumber = currentSample.LicenseNumber;
                //                    cocSample.ManifestNo = currentSample.ManifestNo;
                //                    cocSample.MonitoryingRequirement = currentSample.MonitoryingRequirement;
                //                    cocSample.NoOfCollectionsEachTime = currentSample.NoOfCollectionsEachTime;
                //                    cocSample.NoOfPoints = currentSample.NoOfPoints;
                //                    cocSample.Notes = currentSample.Notes;
                //                    cocSample.OriginatingEntiry = currentSample.OriginatingEntiry;
                //                    cocSample.OriginatingLicenseNumber = currentSample.OriginatingLicenseNumber;
                //                    cocSample.PackageNumber = currentSample.PackageNumber;
                //                    cocSample.ParentSampleDate = currentSample.ParentSampleDate;
                //                    cocSample.ParentSampleID = currentSample.ParentSampleID;
                //                    cocSample.PiecesPerUnit = currentSample.PiecesPerUnit;
                //                    cocSample.Preservetives = currentSample.Preservetives;
                //                    //cocSample.ProjectName = currentSample.ProjectName;
                //                    //cocSample.PurifierSampleID = currentSample.PurifierSampleID;
                //                    cocSample.PWSID = currentSample.PWSID;
                //                    cocSample.PWSSystemName = currentSample.PWSSystemName;
                //                    //cocSample.RegionNameOfSection = currentSample.RegionNameOfSection;
                //                    //cocSample.RejectionCriteria = currentSample.RejectionCriteria;
                //                    cocSample.RepeatLocation = currentSample.RepeatLocation;
                //                    cocSample.RetainedWeight = currentSample.RetainedWeight;
                //                    //cocSample.RiverWidth = currentSample.RiverWidth;
                //                    cocSample.RushSample = currentSample.RushSample;
                //                    //cocSample.SampleAmount = currentSample.SampleAmount;
                //                    cocSample.SampleCondition = currentSample.SampleCondition;
                //                    cocSample.SampleDescription = currentSample.SampleDescription;
                //                    cocSample.SampleImage = currentSample.SampleImage;
                //                    cocSample.SampleName = currentSample.SampleName;
                //                    //cocSample.SamplePointID = currentSample.SamplePointID;
                //                    cocSample.SamplePointType = currentSample.SamplePointType;
                //                    //cocSample.SampleSource = currentSample.SampleSource;
                //                    cocSample.SampleTag = currentSample.SampleTag;
                //                    cocSample.SampleWeight = currentSample.SampleWeight;
                //                    //cocSample.SamplingAddress = currentSample.SamplingAddress;
                //                    cocSample.SamplingEquipment = currentSample.SamplingEquipment;
                //                    cocSample.SamplingLocation = currentSample.SamplingLocation;
                //                    cocSample.SamplingProcedure = currentSample.SamplingProcedure;
                //                    //cocSample.SequenceTestSampleID = currentSample.SequenceTestSampleID;
                //                    //cocSample.SequenceTestSortNo = currentSample.SequenceTestSortNo;
                //                    cocSample.ServiceArea = currentSample.ServiceArea;
                //                    cocSample.SiteCode = currentSample.SiteCode;
                //                    cocSample.SiteDescription = currentSample.SiteDescription;
                //                    //cocSample.SiteID = currentSample.SiteID;
                //                    cocSample.SiteNameArchived = currentSample.SiteNameArchived;
                //                    //cocSample.SiteUserDefinedColumn1 = currentSample.SiteUserDefinedColumn1;
                //                    //cocSample.SiteUserDefinedColumn2 = currentSample.SiteUserDefinedColumn2;
                //                    //cocSample.SiteUserDefinedColumn3 = currentSample.SiteUserDefinedColumn3;
                //                    cocSample.SubOut = currentSample.SubOut;
                //                    cocSample.SystemType = currentSample.SystemType;
                //                    cocSample.TargetMGTHC_CBD_mg_pc = currentSample.TargetMGTHC_CBD_mg_pc;
                //                    cocSample.TargetMGTHC_CBD_mg_unit = currentSample.TargetMGTHC_CBD_mg_unit;
                //                    cocSample.TargetPotency = currentSample.TargetPotency;
                //                    cocSample.TargetUnitWeight_g_pc = currentSample.TargetUnitWeight_g_pc;
                //                    cocSample.TargetUnitWeight_g_unit = currentSample.TargetUnitWeight_g_unit;
                //                    cocSample.TargetWeight = currentSample.TargetWeight;
                //                    //cocSample.Time = currentSample.Time;
                //                    cocSample.TimeEnd = currentSample.TimeEnd;
                //                    cocSample.TimeStart = currentSample.TimeStart;
                //                    cocSample.TotalSamples = currentSample.TotalSamples;
                //                    cocSample.TotalTimes = currentSample.TotalTimes;
                //                    cocSample.TtimeUnit = currentSample.TtimeUnit;
                //                    cocSample.WaterType = currentSample.WaterType;
                //                    cocSample.ZipCode = currentSample.ZipCode;

                //                    if (currentSample.VisualMatrix != null)
                //                    {
                //                        cocSample.VisualMatrix = currentSample.VisualMatrix;
                //                    }

                //                    foreach (var objLineA in currentSample.Testparameters)
                //                    {
                //                        cocSample.Testparameters.Add(objLineA);
                //                    }
                //                    for (int Count = 0; Count < cocSample.Tests.Count; Count++)
                //                    {
                //                        cocSample.Tests[Count].TestGroup = cocSample.Tests[Count].TestGroup;
                //                    }
                //                    foreach (var objSampleparameter in currentSample.Tests.Where(a => a.IsGroup == true && a.GroupTest != null).ToList())
                //                    {

                //                        COCSettingsTest sample = cocSample.Tests.FirstOrDefault<COCSettingsTest>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                //                        if (objSampleparameter.GroupTest != null && sample != null)
                //                        {
                //                            sample.IsGroup = true;
                //                            sample.GroupTest = os.GetObjectByKey<GroupTestMethod>(objSampleparameter.GroupTest.Oid);
                //                            //sample.TestGroup = uow.GetObjectByKey<GroupTest>(objSampleparameter.TestGroup.Oid);
                //                        }
                //                    }
                //                    os.CommitChanges();

                //                    //if (currentSample.Tests != null && currentSample.Tests.Count > 0)
                //                    //{
                //                    //    foreach (Modules.BusinessObjects.Setting.COCSettingsTest objTest in currentSample.Tests)
                //                    //    {
                //                    //        Modules.BusinessObjects.Setting.COCSettingsTest newsample = os.CreateObject<Modules.BusinessObjects.Setting.COCSettingsTest>();
                //                    //        newsample.Samples = os.GetObject<COCSettingsSamples>(cocSample);
                //                    //        newsample.Testparameter = os.GetObject<Testparameter>(objTest.Testparameter);
                //                    //    }
                //                    //}

                //                    //os.CommitChanges(); 
                //                }
                //            }
                //        }
                //    }
                //    NestedFrame nestedFrame = (NestedFrame)Frame;
                //    CompositeView view = nestedFrame.ViewItem.View;
                //    foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                //    {
                //        if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                //        {
                //            //frameContainer.Frame.View.ObjectSpace.Refresh();
                //            if (frameContainer.Frame.View is DetailView)
                //            {
                //                frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                //            }
                //            else
                //            {
                //                (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                //            }
                //            frameContainer.Frame.View.Refresh();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyTest_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                //if (COCInfo != null && COCInfo.SampleOid != null)
                //{
                //    IObjectSpace os = Application.CreateObjectSpace();
                //    COCSettingsSamples currentSample = os.GetObjectByKey<COCSettingsSamples>(COCInfo.SampleOid);

                //    if (currentSample != null)
                //    {
                //        foreach (Modules.BusinessObjects.Setting.COCSettingsTest objTest in currentSample.Tests)
                //        {
                //            foreach (Modules.BusinessObjects.Setting.COCSettingsSamples objSample in e.PopupWindowView.SelectedObjects)
                //            {
                //                if (objTest.Testparameter != null)
                //                {
                //                    Modules.BusinessObjects.Setting.COCSettingsTest valsample = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.COCSettingsTest>(CriteriaOperator.Parse("[Samples.Oid]=? and [Testparameter.Oid]=?", objSample.Oid, objTest.Testparameter.Oid));
                //                    if (valsample == null)
                //                    {
                //                        Modules.BusinessObjects.Setting.COCSettingsTest newsample = os.CreateObject<Modules.BusinessObjects.Setting.COCSettingsTest>();
                //                        newsample.Samples = os.GetObject<COCSettingsSamples>(objSample);
                //                        newsample.Testparameter = os.GetObject<Testparameter>(objTest.Testparameter);
                //                    }
                //                }
                //            }
                //        }
                //        os.CommitChanges();

                //        NestedFrame nestedFrame = (NestedFrame)Frame;
                //        CompositeView view = nestedFrame.ViewItem.View;
                //        foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                //        {
                //            if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                //            {
                //                //frameContainer.Frame.View.ObjectSpace.Refresh();
                //                if (frameContainer.Frame.View is DetailView)
                //                {
                //                    frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                //                }
                //                else
                //                {
                //                    (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                //                }
                //                frameContainer.Frame.View.Refresh();
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyTest_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            //try
            //{
            //    if (View.CurrentObject != null)
            //    {
            //        COCSettingsSamples currentSample = (COCSettingsSamples)View.CurrentObject;
            //        COCInfo.SampleOid = currentSample.Oid;
            //        IObjectSpace objspace = Application.CreateObjectSpace();
            //        CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.Setting.COCSettingsSamples));
            //        cs.Criteria.Clear();
            //        cs.Criteria["filter"] = CriteriaOperator.Parse("[COC.Oid] = ? and Oid <> ? and [VisualMatrix.MatrixName.MatrixName] = ?", currentSample.COC.Oid, currentSample.Oid, currentSample.VisualMatrix.MatrixName.MatrixName);
            //        ListView CreateListView = Application.CreateListView("COCSettingsSamples_ListView_Copy", cs, false);
            //        e.Size = new Size(750, 500);
            //        e.View = CreateListView;
            //    }
            //    else
            //    {
            //        IObjectSpace objspace = Application.CreateObjectSpace();
            //        CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.Setting.COCSettingsSamples));
            //        cs.Criteria.Clear();
            //        cs.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
            //        ListView CreateListView = Application.CreateListView("COCSettingsSamples_ListView_Copy", cs, false);
            //        e.Size = new Size(750, 500);
            //        e.View = CreateListView;
            //        if (View.SelectedObjects != null && View.SelectedObjects.Count > 1)
            //        {
            //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
            //        }
            //        else
            //        {
            //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }

        private void SaveCOCSettings_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "COCSettings")
                {
                    DashboardViewItem dvCOC = (DashboardViewItem)((DashboardView)View).FindItem("COCSettings");
                    DashboardViewItem dvSamples = (DashboardViewItem)((DashboardView)View).FindItem("Samples");

                    if (true)
                    {
                        if (dvCOC.InnerView == null)
                        {
                            dvCOC.CreateControl();
                        }
                        if (dvCOC != null && dvCOC.InnerView != null)
                        {
                            DetailView dv = (DetailView)dvCOC.InnerView;
                            if (dv != null)
                            {
                                dv.ObjectSpace.CommitChanges();
                                using (IObjectSpace os = Application.CreateObjectSpace())
                                {
                                    Session currentSession = ((XPObjectSpace)(os)).Session;
                                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                    //IList<COCSettings> lstCS = uow.Query<COCSettings>().Where(i => i.EndOn >= i.NextUpdateDate && !string.IsNullOrEmpty(i.RecurrenceInfoXml) && i.NextUpdateDate <= DateTime.Today.AddDays(i.BeforEventDays)).ToList();
                                    COCSettings lstCS = dvCOC.InnerView.CurrentObject as COCSettings;
                                    COCSettings objCocSetting = uow.FindObject<COCSettings>(CriteriaOperator.Parse("[COC_ID]=? AND [EndOn] >= [NextUpdateDate] And Not IsNullOrEmpty([RecurrenceInfoXml])", lstCS.COC_ID));

                                    //if (objCocSetting != null)
                                    //{
                                    //    DateTime Now = new DateTime();
                                    //    Now = DateTime.Today.AddDays(objCocSetting.BeforEventDays);
                                    //    if (objCocSetting.NextUpdateDate <= Now)
                                    //    {
                                    //        RecurrenceInfo info = new RecurrenceInfo();
                                    //        info.FromXml(objCocSetting.RecurrenceInfoXml);
                                    //        if (info.Type == RecurrenceType.Daily)
                                    //        {
                                    //            if (info.WeekDays == DevExpress.XtraScheduler.WeekDays.WorkDays)
                                    //            {
                                    //                if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Saturday)
                                    //                {
                                    //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(2);
                                    //                    uow.CommitChanges();
                                    //                    NextRecurrence(objCocSetting);
                                    //                }
                                    //                else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Friday)
                                    //                {
                                    //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(3);
                                    //                    uow.CommitChanges();
                                    //                    NextRecurrence(objCocSetting);
                                    //                }
                                    //                else
                                    //                {
                                    //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                    uow.CommitChanges();
                                    //                    NextRecurrence(objCocSetting);
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(info.Periodicity);
                                    //                uow.CommitChanges();
                                    //                NextRecurrence(objCocSetting);
                                    //            }
                                    //        }
                                    //        else if (info.Type == RecurrenceType.Weekly)
                                    //        {
                                    //            //objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //            bool Sunday = false;
                                    //            bool Monday = false;
                                    //            bool Tuesday = false;
                                    //            bool Wednesday = false;
                                    //            bool Thursday = false;
                                    //            bool Friday = false;
                                    //            bool Saturday = false;
                                    //            string LastDayofWeek = null;
                                    //            string[] Days = info.WeekDays.ToString().Split(',');
                                    //            foreach (string D in Days)
                                    //            {
                                    //                string Day = D.Replace(" ", "");
                                    //                if (Day == "Sunday")
                                    //                {
                                    //                    Sunday = true;
                                    //                    if (Days[Days.Count() - 1] == D)
                                    //                    {
                                    //                        LastDayofWeek = "Sunday";
                                    //                    }
                                    //                }
                                    //                if (Day == "Monday")
                                    //                {
                                    //                    Monday = true;
                                    //                    if (Days[Days.Count() - 1] == D)
                                    //                    {
                                    //                        LastDayofWeek = "Monday";
                                    //                    }
                                    //                }
                                    //                if (Day == "Tuesday")
                                    //                {
                                    //                    Tuesday = true;
                                    //                    if (Days[Days.Count() - 1] == D)
                                    //                    {
                                    //                        LastDayofWeek = "Tuesday";
                                    //                    }
                                    //                }
                                    //                if (Day == "Wednesday")
                                    //                {
                                    //                    Wednesday = true;
                                    //                    if (Days[Days.Count() - 1] == D)
                                    //                    {
                                    //                        LastDayofWeek = "Wednesday";
                                    //                    }
                                    //                }
                                    //                if (Day == "Thursday")
                                    //                {
                                    //                    Thursday = true;
                                    //                    if (Days[Days.Count() - 1] == D)
                                    //                    {
                                    //                        LastDayofWeek = "Thursday";
                                    //                    }
                                    //                }
                                    //                if (Day == "Friday")
                                    //                {
                                    //                    Friday = true;
                                    //                    if (Days[Days.Count() - 1] == D)
                                    //                    {
                                    //                        LastDayofWeek = "Friday";
                                    //                    }
                                    //                }
                                    //                if (Day == "Saturday")
                                    //                {
                                    //                    Saturday = true;
                                    //                    if (Days[Days.Count() - 1] == D)
                                    //                    {
                                    //                        LastDayofWeek = "Saturday";
                                    //                    }
                                    //                }
                                    //            }
                                    //            do
                                    //            {
                                    //                for (int a = 1; a <= 7; a++)
                                    //                {
                                    //                    if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Monday && Monday == true)
                                    //                    {
                                    //                        NextRecurrence(objCocSetting);
                                    //                        if (LastDayofWeek == "Monday")
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(((info.Periodicity - 1) * 7) + 1);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        }
                                    //                        uow.CommitChanges();
                                    //                        break;
                                    //                    }
                                    //                    else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Tuesday && Tuesday == true)
                                    //                    {
                                    //                        NextRecurrence(objCocSetting);
                                    //                        if (LastDayofWeek == "Tuesday")
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(((info.Periodicity - 1) * 7) + 1);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        }
                                    //                        uow.CommitChanges();
                                    //                        break;

                                    //                    }
                                    //                    else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Wednesday)
                                    //                    {
                                    //                        NextRecurrence(objCocSetting);
                                    //                        if (LastDayofWeek == "Wednesday")
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(((info.Periodicity - 1) * 7) + 1);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        }
                                    //                        uow.CommitChanges();
                                    //                        break;
                                    //                    }
                                    //                    else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Thursday && Thursday == true)
                                    //                    {
                                    //                        NextRecurrence(objCocSetting);
                                    //                        if (LastDayofWeek == "Thursday")
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(((info.Periodicity - 1) * 7) + 1);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        }
                                    //                        uow.CommitChanges();
                                    //                        break;
                                    //                    }
                                    //                    else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Friday && Friday == true)
                                    //                    {
                                    //                        NextRecurrence(objCocSetting);
                                    //                        if (LastDayofWeek == "Friday")
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(((info.Periodicity - 1) * 7) + 1);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        }
                                    //                        uow.CommitChanges();
                                    //                        break;
                                    //                    }
                                    //                    else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Saturday && Saturday == true)
                                    //                    {
                                    //                        NextRecurrence(objCocSetting);
                                    //                        if (LastDayofWeek == "Saturday")
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(((info.Periodicity - 1) * 7) + 1);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        }
                                    //                        uow.CommitChanges();
                                    //                        break;
                                    //                    }
                                    //                    else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Sunday && Sunday == true)
                                    //                    {
                                    //                        NextRecurrence(objCocSetting);
                                    //                        if (LastDayofWeek == "Sunday")
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(((info.Periodicity - 1) * 7) + 1);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        }
                                    //                        uow.CommitChanges();
                                    //                        break;
                                    //                    }
                                    //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                }
                                    //            }
                                    //            while (objCocSetting.NextUpdateDate <= objCocSetting.EndOn && objCocSetting.NextUpdateDate <= DateTime.Now);
                                    //        }
                                    //        else if (info.Type == RecurrenceType.Monthly)
                                    //        {

                                    //            if (info.WeekOfMonth == WeekOfMonth.None)
                                    //            {
                                    //                if (objCocSetting.NextUpdateDate == objCocSetting.StartOn)
                                    //                {
                                    //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, objCocSetting.StartOn.Month, info.DayNumber);
                                    //                    uow.CommitChanges();
                                    //                }
                                    //                if (objCocSetting.NextUpdateDate <= DateTime.Today.AddDays(objCocSetting.BeforEventDays))
                                    //                {
                                    //                    if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths((info.Periodicity) * 2);
                                    //                        uow.CommitChanges();
                                    //                        //NextRecurrence(objCocSetting);
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
                                    //                        uow.CommitChanges();
                                    //                        NextRecurrence(objCocSetting);
                                    //                    }
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                if (objCocSetting.LastUpdateDate == DateTime.MinValue)
                                    //                {
                                    //                    if (info.WeekOfMonth == WeekOfMonth.First)
                                    //                    {
                                    //                        if (objCocSetting.NextUpdateDate == objCocSetting.StartOn)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, objCocSetting.StartOn.Month, 01);
                                    //                        }
                                    //                        if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(7);
                                    //                            if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                            {
                                    //                                objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month + 1, 01);
                                    //                            }
                                    //                            else
                                    //                            {
                                    //                                int day = Convert.ToInt32(objCocSetting.NextUpdateDate.Day - objCocSetting.StartOn.Day);
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(-day);
                                    //                            }
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);

                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Second)
                                    //                    {
                                    //                        if (objCocSetting.NextUpdateDate == objCocSetting.StartOn)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, objCocSetting.StartOn.Month, 08);
                                    //                        }
                                    //                        if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(7);
                                    //                            if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                            {
                                    //                                objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month + 1, 08);
                                    //                            }
                                    //                            else
                                    //                            {
                                    //                                int day = Convert.ToInt32(objCocSetting.NextUpdateDate.Day - objCocSetting.StartOn.Day);
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(-day);
                                    //                            }
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Third)
                                    //                    {
                                    //                        if (objCocSetting.NextUpdateDate == objCocSetting.StartOn)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, objCocSetting.StartOn.Month, 15);
                                    //                        }
                                    //                        if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(7);
                                    //                            if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                            {
                                    //                                objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month + 1, 15);
                                    //                            }
                                    //                            else
                                    //                            {
                                    //                                int day = Convert.ToInt32(objCocSetting.NextUpdateDate.Day - objCocSetting.StartOn.Day);
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(-day);
                                    //                            }
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                                    //                    {
                                    //                        if (objCocSetting.NextUpdateDate == objCocSetting.StartOn)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, objCocSetting.StartOn.Month, 22);
                                    //                        }
                                    //                        if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(7);
                                    //                            if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                            {
                                    //                                objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month + 1, 22);
                                    //                            }
                                    //                            else
                                    //                            {
                                    //                                int day = Convert.ToInt32(objCocSetting.NextUpdateDate.Day - objCocSetting.StartOn.Day);
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(-day);
                                    //                            }
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Last)
                                    //                    {
                                    //                        DateTime DT = new DateTime();
                                    //                        DT = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
                                    //                        objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        if (objCocSetting.NextUpdateDate.Month == objCocSetting.StartOn.Month)
                                    //                        {
                                    //                            if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                            {
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(7);
                                    //                                if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                                {
                                    //                                    DT = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month + 1, 28);
                                    //                                    if (objCocSetting.NextUpdateDate.Month != DT.Month)
                                    //                                    {
                                    //                                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month + 1, 22);
                                    //                                    }
                                    //                                    else
                                    //                                    {
                                    //                                        objCocSetting.NextUpdateDate = DT;
                                    //                                    }
                                    //                                }
                                    //                            }
                                    //                            else
                                    //                            {
                                    //                                int day = Convert.ToInt32(objCocSetting.NextUpdateDate.Day - objCocSetting.StartOn.Day);
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(-day);
                                    //                            }
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            DT = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, objCocSetting.StartOn.Month, 22);
                                    //                            if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                            {
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(7);
                                    //                                if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                                {
                                    //                                    objCocSetting.NextUpdateDate = DT.AddMonths(info.Periodicity);
                                    //                                }
                                    //                            }
                                    //                            else
                                    //                            {
                                    //                                int day = Convert.ToInt32(objCocSetting.NextUpdateDate.Day - objCocSetting.StartOn.Day);
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(-day);
                                    //                            }
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                }
                                    //                if (objCocSetting.NextUpdateDate <= objCocSetting.EndOn && objCocSetting.NextUpdateDate <= DateTime.Now)
                                    //                {
                                    //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
                                    //                    if (info.WeekOfMonth == WeekOfMonth.First)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Second)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Third)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Last)
                                    //                    {
                                    //                        DateTime LastUpdateDate = new DateTime();
                                    //                        LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
                                    //                        LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = LastUpdateDate;
                                    //                        }
                                    //                    }
                                    //                    MonthlyYearly(info, objCocSetting, uow);
                                    //                    NextRecurrence(objCocSetting);
                                    //                }
                                    //            }
                                    //        }
                                    //        else if (info.Type == RecurrenceType.Yearly)
                                    //        {
                                    //            if (info.WeekOfMonth == WeekOfMonth.None)
                                    //            {
                                    //                if (objCocSetting.LastUpdateDate == DateTime.MinValue)
                                    //                {
                                    //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, info.Month, info.DayNumber);
                                    //                    if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                        uow.CommitChanges();
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        uow.CommitChanges();
                                    //                    }
                                    //                    objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
                                    //                }
                                    //                else
                                    //                {
                                    //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                    uow.CommitChanges();
                                    //                    NextRecurrence(objCocSetting);
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                if (objCocSetting.LastUpdateDate == DateTime.MinValue)
                                    //                {
                                    //                    if (info.WeekOfMonth == WeekOfMonth.First)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, info.Month, 01);
                                    //                        if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Second)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, info.Month, 08);
                                    //                        if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Third)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, info.Month, 15);
                                    //                        if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, info.Month, 22);
                                    //                        if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Last)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, info.Month, 28);
                                    //                        objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        if (objCocSetting.NextUpdateDate.Month == info.Month)
                                    //                        {
                                    //                            if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                            {
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                            }
                                    //                        }
                                    //                        else
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = new DateTime(objCocSetting.StartOn.Year, info.Month, 22);
                                    //                            if (objCocSetting.StartOn >= objCocSetting.NextUpdateDate)
                                    //                            {
                                    //                                objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                            }
                                    //                        }
                                    //                        MonthlyYearly(info, objCocSetting, uow);
                                    //                    }
                                    //                }
                                    //                if (objCocSetting.NextUpdateDate <= objCocSetting.EndOn && objCocSetting.NextUpdateDate <= DateTime.Now)
                                    //                {
                                    //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
                                    //                    if (info.WeekOfMonth == WeekOfMonth.First)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Second)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Third)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Fourth)
                                    //                    {
                                    //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
                                    //                    }
                                    //                    else if (info.WeekOfMonth == WeekOfMonth.Last)
                                    //                    {
                                    //                        DateTime DT = new DateTime();
                                    //                        DT = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
                                    //                        objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
                                    //                        if (objCocSetting.NextUpdateDate.Month != DT.Month)
                                    //                        {
                                    //                            objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
                                    //                        }
                                    //                    }
                                    //                    MonthlyYearly(info, objCocSetting, uow);
                                    //                    NextRecurrence(objCocSetting);
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                        }

                        if (dvSamples != null && dvSamples.InnerView != null)
                        {
                            ((ASPxGridListEditor)((ListView)dvSamples.InnerView).Editor).Grid.UpdateEdit();
                        }

                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void NextRecurrence(COCSettings objCocSetting)
        {
            //IObjectSpace os = Application.CreateObjectSpace();
            //Session currentSession = ((XPObjectSpace)(os)).Session;
            //UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
            //Tasks objCurrentTasks = new Tasks(uow);
            //Samplecheckin objNewSampleCheckIn = new Samplecheckin(uow);
            //if (objCocSetting.Client != null)
            //{
            //    objNewSampleCheckIn.ClientName = objCurrentTasks.Client = uow.GetObjectByKey<Customer>(objCocSetting.Client.Oid);
            //}
            //if (objCocSetting.ProjectID != null)
            //{
            //    objNewSampleCheckIn.ProjectID = objCurrentTasks.ProjectID = uow.GetObjectByKey<Project>(objCocSetting.ProjectID.Oid);
            //}

            //objCurrentTasks.ProjectName = objCocSetting.ProjectName;

            //objNewSampleCheckIn.ProjectLocation = objCurrentTasks.ProjectLocation = objCocSetting.ProjectLocation;

            //objNewSampleCheckIn.ClientAddress = objCurrentTasks.ClientAddress = objCocSetting.Address;
            ////objNewTask.Status = objTask.Status;
            //objCurrentTasks.Status = TaskManagementStatus.PendingSubmit;
            //objNewSampleCheckIn.Status = SampleRegistrationSignoffStatus.PendingSubmit;
            //objNewSampleCheckIn.SampleMatries = objCurrentTasks.SampleMatrix = objCocSetting.SampleMatrix.Oid.ToString();
            ////objCurrentTasks.TAT = objCocSetting.SampleMatrix.Oid.ToString();
            ////objCurrentTasks.WhenAct = objCocSetting.SampleMatrix.Oid.ToString();
            //uow.CommitChanges();
            //XPClassInfo sampleloginfo;
            //sampleloginfo = uow.GetClassInfo(typeof(COCSettingsSamples));
            //IList<COCSettingsSamples> objCocSettingSamples = uow.GetObjects(sampleloginfo, CriteriaOperator.Parse("[COC.Oid]=?", objCocSetting.Oid), null, int.MaxValue, false, true).Cast<COCSettingsSamples>().ToList();
            ////IList<Modules.BusinessObjects.Setting.COCSettingsSamples> lstSamples = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.COCSettingsSamples>(CriteriaOperator.Parse("[COC.Oid] = ?", objCOC.Oid));
            //if (objCocSettingSamples != null && objCocSettingSamples.Count > 0)
            //{
            //    //foreach (Sampling obj in objSampling)
            //    foreach (COCSettingsSamples obj in objCocSettingSamples)
            //    {
            //        SelectedData sproc = currentSession.ExecuteSproc("GetSamplingSampleID", new OperandValue(objCurrentTasks.Oid.ToString()));
            //        Sampling objNewSampling = new Sampling(uow);
            //        Modules.BusinessObjects.SampleManagement.SampleLogIn objNewSample = new Modules.BusinessObjects.SampleManagement.SampleLogIn(uow);
            //        if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
            //        {
            //            objNewSample.SampleNo = objNewSampling.SampleNo = (int)sproc.ResultSet[1].Rows[0].Values[0];
            //            objNewSample.SortOrder = objNewSampling.SortOrder = objNewSampling.SampleNo;
            //        }
            //        if (obj.SiteName != null)
            //        {
            //            objNewSample.StationLocation = objNewSampling.StationLocation = obj.SiteName.StationLocation;
            //        }
            //        if (obj.VisualMatrix != null)
            //        {
            //            objNewSample.VisualMatrix = objNewSampling.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(obj.VisualMatrix.Oid);
            //        }
            //        if (objCurrentTasks != null)
            //        {
            //            objNewSampling.Tasks = uow.GetObjectByKey<Tasks>(objCurrentTasks.Oid);
            //        }
            //        if (objNewSampleCheckIn != null)
            //        {
            //            objNewSample.JobID = uow.GetObjectByKey<Samplecheckin>(objNewSampleCheckIn.Oid);
            //        }
            //        if (obj.Testparameters != null && obj.Testparameters.Count > 0)
            //        {
            //            foreach (Testparameter testparameter in obj.Testparameters)
            //            {
            //                objNewSampling.Testparameters.Add(uow.GetObjectByKey<Testparameter>(testparameter.Oid));
            //                objNewSample.Testparameters.Add(uow.GetObjectByKey<Testparameter>(testparameter.Oid));
            //            }
            //        }
            //        if (obj.AssignedBy != null)
            //        {
            //            objNewSample.AssignedBy = objNewSampling.AssignedBy = uow.GetObjectByKey<Employee>(obj.AssignedBy.Oid);

            //        }
            //        objNewSample.AssignTo = objNewSampling.AssignTo = obj.AssignTo;
            //        objNewSample.CollectDate = objNewSampling.CollectDate = obj.CollectDate;
            //        objNewSample.CollectTime = objNewSampling.CollectTime = obj.CollectTime;

            //        if (obj.Collector != null)
            //        {
            //            objNewSample.Collector = objNewSampling.Collector = uow.GetObjectByKey<Collector>(obj.Collector.Oid);
            //        }
            //        objNewSample.Comment = objNewSampling.Comment = obj.Comment;


            //        objNewSample.EquipmentName = objNewSampling.EquipmentName = obj.EquipmentName;

            //        if (obj.Preservetives != null)
            //        {
            //            objNewSample.Preservetives = objNewSampling.Preservetives = obj.Preservetives;
            //        }
            //        if (obj.QCSource != null)
            //        {
            //            objNewSampling.QCSource = uow.GetObjectByKey<QCType>(obj.QCSource.Oid);
            //        }
            //        if (obj.QCType != null)
            //        {
            //            objNewSample.QCType = objNewSampling.QCType = uow.GetObjectByKey<QCType>(obj.QCType.Oid);
            //        }
            //        objNewSampling.Qty = obj.Qty;
            //        objNewSample.Qty = (uint)obj.Qty;

            //        objNewSample.SampleName = objNewSampling.SampleName = obj.SampleName;
            //        if (obj.SampleType != null)
            //        {
            //            objNewSample.SampleType = objNewSampling.SampleType = uow.GetObjectByKey<SampleType>(obj.SampleType.Oid);
            //        }
            //        if (obj.Storage != null)
            //        {
            //            objNewSample.Storage = objNewSampling.Storage = uow.GetObjectByKey<Storage>(obj.Storage.Oid);
            //        }
            //        objNewSample.SubOut = objNewSampling.SubOut = obj.SubOut;
            //        objNewSampling.Timemin = obj.Timemin;
            //        objNewSample.TimeEnd = objNewSampling.TimeEnd = obj.TimeEnd;
            //        objNewSample.TimeStart = objNewSampling.TimeStart = obj.TimeStart;
            //        objNewSample.SiteName = objNewSampling.SiteName = obj.SiteName;
            //        objNewSample.SiteDescription = objNewSampling.SiteDescription = obj.SiteDescription;
            //        objNewSample.PWSID = objNewSampling.PWSID = obj.PWSID;
            //        objNewSample.PWSSystemName = objNewSampling.PWSSystemName = obj.PWSSystemName;
            //        objNewSample.KeyMap = objNewSampling.KeyMap = obj.KeyMap;
            //        objNewSample.ServiceArea = objNewSampling.ServiceArea = obj.ServiceArea;
            //        objNewSample.SiteNameArchived = objNewSampling.SiteNameArchived = obj.SiteNameArchived;
            //        objNewSample.IsActive = objNewSampling.IsActive = obj.IsActive;
            //        objNewSample.City = objNewSampling.City = obj.City;
            //        objNewSample.ZipCode = objNewSampling.ZipCode = obj.ZipCode;
            //        objNewSample.FacilityID = objNewSampling.FacilityID = obj.FacilityID;
            //        objNewSample.FacilityName = objNewSampling.FacilityName = obj.FacilityName;
            //        objNewSample.FacilityType = objNewSampling.FacilityType = obj.FacilityType;
            //        objNewSample.SamplePointType = objNewSampling.SamplePointType = obj.SamplePointType;
            //        objNewSample.WaterType = objNewSampling.WaterType = obj.WaterType;
            //        objNewSample.Longitude = objNewSampling.Longitude = obj.Longitude;
            //        objNewSample.Latitude = objNewSampling.Latitude = obj.Latitude;
            //        objNewSample.MonitoryingRequirement = objNewSampling.MonitoryingRequirement = obj.MonitoryingRequirement;
            //        objNewSample.ParentSampleID = objNewSampling.ParentSampleID = obj.ParentSampleID;
            //        objNewSample.ParentSampleDate = objNewSampling.ParentSampleDate = obj.ParentSampleDate;
            //        objNewSample.RepeatLocation = objNewSampling.RepeatLocation = obj.RepeatLocation;

            //        if (obj.VisualMatrix != null)
            //        {
            //            objNewSample.VisualMatrix = objNewSampling.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(obj.VisualMatrix.Oid);
            //        }
            //        SamplingStation objStation = uow.FindObject<SamplingStation>(CriteriaOperator.Parse("[Sampling.Oid] = ?", objNewSampling.Oid));
            //        if (objStation == null)
            //        {
            //            objStation = new SamplingStation(uow);
            //            objStation.Sampling = objNewSampling;
            //            objStation.SampleLocation = objNewSampling.StationLocation;
            //            objStation.SiteName = objNewSampling.StationLocation;
            //            objStation.Matrix = objNewSampling.VisualMatrix;
            //        }
            //        IList<SampleBottleAllocation> lstsmplbtl = os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[COCSettings.Oid] = ?", obj.Oid));
            //        if (lstsmplbtl != null && lstsmplbtl.Count > 0)
            //        {
            //            foreach (SampleBottleAllocation objsmpl in lstsmplbtl.ToList())
            //            {
            //                SampleBottleAllocation newsmplbtl = new SampleBottleAllocation(uow);
            //                newsmplbtl.BottleSet = objsmpl.BottleSet;
            //                newsmplbtl.SharedTests = objsmpl.SharedTests;
            //                newsmplbtl.SharedTestsGuid = objsmpl.SharedTestsGuid;
            //                newsmplbtl.Qty = objsmpl.Qty;
            //                newsmplbtl.BottleID = objsmpl.BottleID;
            //                if (objsmpl.Containers != null)
            //                {
            //                    newsmplbtl.Containers = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Container>(objsmpl.Containers.Oid);
            //                }
            //                if (objsmpl.Preservative != null)
            //                {
            //                    newsmplbtl.Preservative = uow.GetObjectByKey<Preservative>(objsmpl.Preservative.Oid);
            //                }
            //                newsmplbtl.TaskRegistration = objNewSampling;
            //                newsmplbtl.SampleRegistration = objNewSample;
            //            }
            //        }
            //        uow.CommitChanges();
            //    }
            //}
        }
        //public void MonthlyYearly(RecurrenceInfo info, COCSettings objCocSetting, UnitOfWork uow)
        //{
        //    DateTime DT = new DateTime();

        //    for (int a = 0; a <= 7; a++)
        //    {
        //        if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Monday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Monday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }
        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Tuesday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Tuesday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }

        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Wednesday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Wednesday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }

        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Thursday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Thursday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }

        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Friday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Friday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }

        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Saturday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Saturday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }

        //        }
        //        else if (objCocSetting.NextUpdateDate.DayOfWeek == DayOfWeek.Sunday && info.WeekDays == DevExpress.XtraScheduler.WeekDays.Sunday)
        //        {
        //            if (objCocSetting.LastUpdateDate == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                uow.CommitChanges();
        //                break;
        //            }
        //            else if (DT == DateTime.MinValue)
        //            {
        //                objCocSetting.LastUpdateDate = objCocSetting.NextUpdateDate;
        //                if (info.Type == RecurrenceType.Monthly)
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddMonths(info.Periodicity);
        //                }
        //                else
        //                {
        //                    objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddYears(info.Periodicity);
        //                }
        //                if (info.WeekOfMonth == WeekOfMonth.First)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 01);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Second)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 08);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Third)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 15);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Fourth)
        //                {
        //                    objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                }
        //                else if (info.WeekOfMonth == WeekOfMonth.Last)
        //                {
        //                    DateTime LastUpdateDate = new DateTime();
        //                    LastUpdateDate = objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 28);
        //                    LastUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //                    if (objCocSetting.NextUpdateDate.Month != LastUpdateDate.Month)
        //                    {
        //                        objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                    }
        //                    else
        //                    {
        //                        objCocSetting.NextUpdateDate = LastUpdateDate;
        //                    }
        //                }
        //                DT = objCocSetting.NextUpdateDate;
        //            }
        //            else
        //            {
        //                uow.CommitChanges();
        //            }

        //        }
        //        if (info.WeekOfMonth == WeekOfMonth.Last)
        //        {
        //            DateTime DT1 = new DateTime();
        //            DT1 = objCocSetting.NextUpdateDate;
        //            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //            if (objCocSetting.NextUpdateDate.Month != DT1.Month)
        //            {
        //                objCocSetting.NextUpdateDate = new DateTime(objCocSetting.NextUpdateDate.Year, objCocSetting.NextUpdateDate.Month, 22);
        //                a = 0;
        //            }
        //        }
        //        else
        //        {
        //            objCocSetting.NextUpdateDate = objCocSetting.NextUpdateDate.AddDays(1);
        //        }
        //    }
        //}
        private void COCTestGroup_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //try
            //{
            //    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
            //    COCSettingsSamples currentSamples = ObjectSpace.GetObjectByKey<COCSettingsSamples>(((COCSettingsSamples)e.CurrentObject).Oid);
            //    if (currentSamples != null && currentSamples.VisualMatrix != null)
            //    //if (SRInfo.strSampleID != "error")
            //    {
            //        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
            //        IObjectSpace objspace = Application.CreateObjectSpace();
            //        CollectionSource cs = new CollectionSource(objspace, typeof(GroupTest));
            //        cs.Criteria.Clear();
            //        List<object> GroupTestName = new List<object>();
            //        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(GroupTestMethod)))
            //        {
            //            lstview.Criteria = CriteriaOperator.Parse("[TestMethods.MatrixName.MatrixName]='" + currentSamples.VisualMatrix.MatrixName.MatrixName + "'");
            //            lstview.Properties.Add(new ViewProperty("GroupTestName", DevExpress.Xpo.SortDirection.Ascending, "GroupTests.TestGroupName", true, true));
            //            foreach (ViewRecord Vrec in lstview)
            //                GroupTestName.Add(Vrec["GroupTestName"]);
            //        }
            //        cs.Criteria["filter"] = new InOperator("TestGroupName", GroupTestName);
            //        ListView CreateListView = Application.CreateListView("GroupTest_ListView_Copy_Samplelogin", cs, false);
            //        //}
            //        ShowViewParameters showViewParameters = new ShowViewParameters(CreateListView);
            //        showViewParameters.Context = TemplateContext.PopupWindow;
            //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            //        DialogController dc = Application.CreateController<DialogController>();
            //        dc.SaveOnAccept = false;
            //        dc.Accepting += Dc_Accepting1;
            //        //dc.CanCloseWindow = AssignTestGroup(showViewParameters);  
            //        showViewParameters.Controllers.Add(dc);

            //        COCInfo.SampleOid = currentSamples.Oid;
            //        COCInfo.lstTestParameter = new List<Guid>();
            //        COCInfo.lstSavedTestParameter = new List<Guid>();
            //        COCInfo.lstdupfilterguid = new List<Guid>();
            //        COCInfo.lstdupfilterstr = new List<string>();
            //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            //    }
            //    else
            //    {
            //        //Application.ShowViewStrategy.ShowMessage("Select the VisualMatrix", InformationType.Error, timer.Seconds, InformationPosition.Top);
            //        Application.ShowViewStrategy
            //            .ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectmatrix"), InformationType.Error, timer.Seconds, InformationPosition.Top);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>()
            //        .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }

        private void Dc_Accepting1(object sender, DialogControllerAcceptingEventArgs e)
        {
            //try
            //{
            //    //if (e.AcceptActionArgs.SelectedObjects.Count > 0)
            //    //{
            //    //    COCInfo.strViewID = "COCTestGroup";
            //    //    var lstGroupTest = e.AcceptActionArgs.SelectedObjects;
            //    //    if (lstGroupTest != null && lstGroupTest.Count > 0)
            //    //    {
            //    //        COCInfo.lstBottleID = lstGroupTest.Cast<GroupTest>().ToList();
            //    //    }

            //    //    COCSettingsSamples currentSamples = ObjectSpace.GetObjectByKey<COCSettingsSamples>(COCInfo.SampleOid);
            //    //    if (currentSamples != null)
            //    //    {
            //    //        DialogController dc = Application.CreateController<DialogController>();
            //    //        dc.SaveOnAccept = false;
            //    //        dc.AcceptAction.Active.SetItemValue("disable", false);
            //    //        dc.CancelAction.Active.SetItemValue("disable", false);
            //    //        dc.CloseOnCurrentObjectProcessing = false;
            //    //        dc.Accepting += TestGroup_Accepting;
            //    //        e.ShowViewParameters.Controllers.Add(dc);
            //    //        e.ShowViewParameters.CreatedView = Application.CreateDashboardView(ObjectSpace, "COCTest", false);
            //    //        e.ShowViewParameters.CreatedView.Caption = "TestGroup Assignment - " + currentSamples.SampleNo;
            //    //    }
            //    //}

            //    Guid TestGroupName;
            //    Guid SLOid = new Guid();
            //    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
            //    if (View != null)
            //    {
            //        COCSettingsSamples settingsSamples = (COCSettingsSamples)View.CurrentObject;
            //        if (settingsSamples != null)
            //        {
            //            foreach (GroupTest obj in e.AcceptActionArgs.SelectedObjects)
            //            {
            //                if (obj != null)
            //                {
            //                    TestGroupName = obj.Oid;
            //                    CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + settingsSamples.Oid + "'");
            //                    COCSettingsSamples objSL = ObjectSpace.FindObject<COCSettingsSamples>(criteria);
            //                    CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]='" + TestGroupName + "'");
            //                    IList<GroupTest> objTP = ObjectSpace.GetObjects<GroupTest>(criteria1);
            //                    SLOid = objSL.Oid;
            //                    if (objTP != null)
            //                    {
            //                        foreach (GroupTest tp in objTP)
            //                        {
            //                            foreach (TestMethod testmethod in tp.TestMethods)
            //                            {
            //                                if ((testmethod.RetireDate == DateTime.MinValue || testmethod.RetireDate > DateTime.Now) && (testmethod.MethodName.RetireDate
            //                                    == DateTime.MinValue || testmethod.MethodName.RetireDate > DateTime.Now))
            //                                {
            //                                    foreach (Testparameter testparam in testmethod.TestParameter)
            //                                    {
            //                                        if (objSL != null && !objSL.Testparameters.Contains(testparam))
            //                                        {
            //                                            if (testparam.TestMethod.MatrixName.MatrixName == settingsSamples.VisualMatrix.MatrixName.MatrixName)
            //                                            {
            //                                                testparam.TestGroup = tp.TestGroupName;
            //                                                objSL.Testparameters.Add(testparam);
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            ObjectSpace.CommitChanges();
            //        }
            //    }
            //    NestedFrame nestedFrame = (NestedFrame)Frame;
            //    CompositeView view = nestedFrame.ViewItem.View;
            //    foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
            //    {
            //        if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
            //        {
            //            //frameContainer.Frame.View.ObjectSpace.Refresh();
            //            if (frameContainer.Frame.View is DetailView)
            //            {
            //                frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
            //            }
            //            else
            //            {
            //                (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
            //            }
            //            frameContainer.Frame.View.Refresh();
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>()
            //        .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
        }

        private void TestGroup_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                //Guid TestGroupName;
                //Guid SLOid = new Guid();
                //Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                //if (View != null)
                //{
                //    COCSettingsSamples settingsSamples = (COCSettingsSamples)View.CurrentObject;
                //    if (settingsSamples != null)
                //    {
                //        foreach (GroupTest obj in e.AcceptActionArgs.SelectedObjects)
                //        {
                //            if (obj != null)
                //            {
                //                TestGroupName = obj.Oid;
                //                CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + settingsSamples.Oid + "'");
                //                COCSettingsSamples objSL = ObjectSpace.FindObject<COCSettingsSamples>(criteria);
                //                CriteriaOperator criteria1 = CriteriaOperator.Parse("[Oid]='" + TestGroupName + "'");
                //                IList<GroupTest> objTP = ObjectSpace.GetObjects<GroupTest>(criteria1);
                //                SLOid = objSL.Oid;
                //                if (objTP != null)
                //                {
                //                    foreach (GroupTest tp in objTP)
                //                    {
                //                        foreach (TestMethod testmethod in tp.TestMethods)
                //                        {
                //                            if ((testmethod.RetireDate == DateTime.MinValue || testmethod.RetireDate > DateTime.Now) && (testmethod.MethodName.RetireDate
                //                                == DateTime.MinValue || testmethod.MethodName.RetireDate > DateTime.Now))
                //                            {
                //                                foreach (Testparameter testparam in testmethod.TestParameter)
                //                                {
                //                                    if (objSL != null && !objSL.Testparameters.Contains(testparam))
                //                                    {
                //                                        if (testparam.TestMethod.MatrixName.MatrixName == settingsSamples.VisualMatrix.MatrixName.MatrixName)
                //                                        {
                //                                            testparam.TestGroup = tp.TestGroupName;
                //                                            objSL.Testparameters.Add(testparam);
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        ObjectSpace.CommitChanges();
                //    }
                //}
                //NestedFrame nestedFrame = (NestedFrame)Frame;
                //CompositeView view = nestedFrame.ViewItem.View;
                //foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                //{
                //    if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                //    {
                //        frameContainer.Frame.View.ObjectSpace.Refresh();
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                    .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void COCSaveAs_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                bool DBAccess = false;
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                int SampleNo = 0;
                if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
                {

                    foreach (COCSettings objCurCOC in View.SelectedObjects)
                    {
                        COCSettings objCOC = uow.GetObjectByKey<COCSettings>(objCurCOC.Oid);
                        if (objCOC != null)
                        {
                            COCSettings newObj = new COCSettings(uow);
                            newObj.COCName = objCOC.COCName;
                            newObj.SampleMatries = objCOC.SampleMatries;
                            newObj.BalanceID = objCOC.BalanceID;
                            newObj.BatchID = objCOC.BatchID;
                            newObj.Comment = objCOC.Comment;
                            newObj.IsAlpacCOCid = objCOC.IsAlpacCOCid;
                            newObj.NoOfSamples = objCOC.NoOfSamples;
                            newObj.NPTest = objCOC.NPTest;
                            newObj.NumberOfSample = objCOC.NumberOfSample;
                            newObj.PackageNo = objCOC.PackageNo;
                            newObj.SampleCategory = objCOC.SampleCategory;
                            newObj.ReportTemplate = objCOC.ReportTemplate;
                            newObj.COCTemplate = objCOC.COCTemplate;
                            newObj.EDDTemplate = objCOC.EDDTemplate;
                            newObj.Test = objCOC.NPTest;
                            newObj.TestName = objCOC.NPTest;
                            if (objCOC.ClientName != null)
                            {
                                newObj.ClientName = uow.GetObjectByKey<Customer>(objCOC.ClientName.Oid);
                                newObj.ClientAddress = objCOC.ClientAddress;
                                newObj.ClientAddress2 = objCOC.ClientAddress2;
                                newObj.ClientContact = objCOC.ClientContact;
                                newObj.ClientPhone = objCOC.ClientPhone;
                            }
                            if (objCOC.ProjectID != null)
                            {
                                newObj.ProjectID = uow.GetObjectByKey<Project>(objCOC.ProjectID.Oid);
                                newObj.ProjectCity = objCOC.ProjectCity;
                                newObj.ProjectOverview = objCOC.ProjectOverview;
                                newObj.ProjectSource = objCOC.ProjectSource;
                                if (objCOC.ProjectCategory != null)
                                {
                                    newObj.ProjectCategory = uow.GetObjectByKey<ProjectCategory>(objCOC.ProjectCategory.Oid);
                                }
                            }
                            if (objCOC.QuoteID != null)
                            {
                                newObj.QuoteID = uow.GetObjectByKey<CRMQuotes>(objCOC.QuoteID.Oid);

                            }
                            if (objCOC.Attachment != null && objCOC.Attachment.Count > 0)
                            {
                                List<Attachment> lstAttachment = View.ObjectSpace.GetObjects<Attachment>(CriteriaOperator.Parse("[COCSettings] = ?", objCOC.Oid)).ToList();
                                foreach (Attachment objattachment in lstAttachment.ToList())
                                {
                                    if (lstAttachment != null)
                                    {
                                        //Attachment objNewAttachment = os.CreateObject<Attachment>();
                                        Attachment objNewAttachment = new Attachment(uow);
                                        Attachment oldAttachment = uow.GetObjectByKey<Attachment>(objattachment.Oid);
                                        if (oldAttachment != null)
                                        {
                                            objNewAttachment.Name = oldAttachment.Name;
                                            objNewAttachment.Category = oldAttachment.Category;
                                            objNewAttachment.Date = oldAttachment.Date;
                                            if (oldAttachment.Operator != null)
                                            {
                                                objNewAttachment.Operator = uow.GetObjectByKey<Employee>(oldAttachment.Operator.Oid);
                                            }
                                            objNewAttachment.Comment = oldAttachment.Comment;
                                            objNewAttachment.Samplecheckin = uow.GetObjectByKey<Samplecheckin>(newObj.Oid);
                                            objNewAttachment.Attachments = oldAttachment.Attachments;
                                            newObj.Attachment.Add(View.ObjectSpace.GetObject(objNewAttachment));
                                        }
                                    }
                                }
                            }
                            if (objCOC.Note != null && objCOC.Note.Count > 0)
                            {
                                List<Notes> lstNotes = View.ObjectSpace.GetObjects<Notes>(CriteriaOperator.Parse("[COCSettings] = ?", objCOC.Oid)).ToList();
                                foreach (Notes objNotes in lstNotes.ToList())
                                {
                                    Notes oldNotes = uow.GetObjectByKey<Notes>(objNotes.Oid);
                                    if (oldNotes != null)
                                    {
                                        Notes objNewNotes = new Notes(uow);
                                        objNewNotes.Title = oldNotes.Title;
                                        objNewNotes.Attachment = oldNotes.Attachment;
                                        objNewNotes.Text = oldNotes.Text;
                                        if (oldNotes.Author != null)
                                        {
                                            objNewNotes.Author = uow.GetObjectByKey<Employee>(oldNotes.Author.Oid);
                                        }
                                        objNewNotes.Date = oldNotes.Date;
                                        objNewNotes.Samplecheckin = uow.GetObjectByKey<Samplecheckin>(newObj.Oid);
                                        objNewNotes.FollowUpDate = oldNotes.FollowUpDate;
                                        newObj.Note.Add(View.ObjectSpace.GetObject(objNewNotes));
                                    }
                                }
                            }
                            uow.CommitChanges();
                            List<COCSettingsSamples> lstcocSample = View.ObjectSpace.GetObjects<COCSettingsSamples>(CriteriaOperator.Parse("[COCID] = ?", objCOC.Oid)).ToList();
                            if (lstcocSample.Count > 0)
                            {
                                foreach (COCSettingsSamples cocSS in lstcocSample.OrderBy(i => i.SampleNo).ToList())
                                {
                                    COCSettingsSamples objSLNew = new COCSettingsSamples(uow);
                                    objSLNew.COCID = uow.GetObjectByKey<COCSettings>(newObj.Oid);
                                    if (objSLNew != null)
                                    {
                                        if (DBAccess == false)
                                        {
                                            SelectedData sproc = currentSession.ExecuteSproc("GetSampleID", new OperandValue(newObj.COC_ID.ToString()));
                                            if (sproc.ResultSet[1].Rows[0].Values[0] != null)
                                            {
                                                objCOCSampleinfo.SampleID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                                SampleNo = Convert.ToInt32(objCOCSampleinfo.SampleID);
                                                DBAccess = true;
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                        objSLNew.SampleNo = SampleNo;
                                        objSLNew.ClientSampleID = cocSS.ClientSampleID;
                                        objSLNew.Test = true;
                                        if (cocSS.VisualMatrix != null)
                                        {
                                            objSLNew.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(cocSS.VisualMatrix.Oid);
                                        }
                                        if (cocSS.SampleType != null)
                                        {
                                            objSLNew.SampleType = uow.GetObjectByKey<SampleType>(cocSS.SampleType.Oid);
                                        }
                                        objSLNew.Qty = cocSS.Qty;
                                        if (cocSS.Storage != null)
                                        {
                                            objSLNew.Storage = uow.GetObjectByKey<Storage>(cocSS.Storage.Oid);
                                        }
                                        objSLNew.Preservetives = cocSS.Preservetives;
                                        objSLNew.SamplingLocation = cocSS.SamplingLocation;
                                        if (cocSS.QCType != null)
                                        {
                                            objSLNew.QCType = uow.GetObjectByKey<QCType>(cocSS.QCType.Oid);
                                        }
                                        if (cocSS.QCSource != null)
                                        {
                                            objSLNew.QCSource = uow.GetObjectByKey<COCSettingsSamples>(cocSS.QCSource.Oid);
                                        }
                                        if (cocSS.Client != null)
                                        {
                                            objSLNew.Client = uow.GetObjectByKey<Customer>(cocSS.Client.Oid);
                                        }
                                        if (cocSS.Department != null)
                                        {
                                            objSLNew.Department = uow.GetObjectByKey<Department>(cocSS.Department.Oid);
                                        }
                                        if (cocSS.ProjectID != null)
                                        {
                                            objSLNew.ProjectID = uow.GetObjectByKey<Project>(cocSS.ProjectID.Oid);
                                        }
                                        if (cocSS.PreserveCondition != null)
                                        {
                                            objSLNew.PreserveCondition = uow.GetObjectByKey<PreserveCondition>(cocSS.PreserveCondition.Oid);
                                        }
                                        if (cocSS.StorageID != null)
                                        {
                                            objSLNew.StorageID = uow.GetObjectByKey<Storage>(cocSS.StorageID.Oid);
                                        }
                                        objSLNew.FlowRate = cocSS.FlowRate;
                                        objSLNew.TimeStart = cocSS.TimeStart;
                                        objSLNew.TimeEnd = cocSS.TimeEnd;
                                        objSLNew.Time = cocSS.Time;
                                        objSLNew.Volume = cocSS.Volume;
                                        objSLNew.Address = cocSS.Address;
                                        objSLNew.AreaOrPerson = cocSS.AreaOrPerson;
                                        if (cocSS.BalanceID != null)
                                        {
                                            objSLNew.BalanceID = uow.GetObjectByKey<Modules.BusinessObjects.Assets.Labware>(cocSS.BalanceID.Oid);
                                        }
                                        objSLNew.AssignTo = cocSS.AssignTo;
                                        objSLNew.Barp = cocSS.Barp;
                                        objSLNew.BatchID = cocSS.BatchID;
                                        objSLNew.BatchSize = cocSS.BatchSize;
                                        objSLNew.BatchSize_pc = cocSS.BatchSize_pc;
                                        objSLNew.BatchSize_Units = cocSS.BatchSize_Units;
                                        objSLNew.Blended = cocSS.Blended;
                                        objSLNew.BottleQty = cocSS.BottleQty;
                                        objSLNew.BuriedDepthOfGroundWater = cocSS.BuriedDepthOfGroundWater;
                                        objSLNew.ChlorineFree = cocSS.ChlorineFree;
                                        objSLNew.ChlorineTotal = cocSS.ChlorineTotal;
                                        objSLNew.City = cocSS.City;
                                        objSLNew.CompositeQty = cocSS.CompositeQty;
                                        objSLNew.DateEndExpected = cocSS.DateEndExpected;
                                        objSLNew.DateStartExpected = cocSS.DateStartExpected;
                                        objSLNew.ClientSampleID = cocSS.ClientSampleID;
                                        objSLNew.Comment = cocSS.Comment;
                                        objSLNew.Containers = cocSS.Containers;
                                        objSLNew.Depth = cocSS.Depth;
                                        objSLNew.Description = cocSS.Description;
                                        objSLNew.DischargeFlow = cocSS.DischargeFlow;
                                        objSLNew.DischargePipeHeight = cocSS.DischargePipeHeight;
                                        objSLNew.DO = cocSS.DO;
                                        objSLNew.Emission = cocSS.Emission;
                                        objSLNew.EndOfRoad = cocSS.EndOfRoad;
                                        objSLNew.EquipmentModel = cocSS.EquipmentModel;
                                        objSLNew.EquipmentName = cocSS.EquipmentName;
                                        objSLNew.FacilityID = cocSS.FacilityID;
                                        objSLNew.FacilityName = cocSS.FacilityName;
                                        objSLNew.FacilityType = cocSS.FacilityType;
                                        objSLNew.FinalForm = cocSS.FinalForm;
                                        objSLNew.FinalPackaging = cocSS.FinalPackaging;
                                        objSLNew.FlowRate = cocSS.FlowRate;
                                        objSLNew.FlowRateCubicMeterPerHour = cocSS.FlowRateCubicMeterPerHour;
                                        objSLNew.FlowRateLiterPerMin = cocSS.FlowRateLiterPerMin;
                                        objSLNew.FlowVelocity = cocSS.FlowVelocity;
                                        objSLNew.ForeignMaterial = cocSS.ForeignMaterial;
                                        objSLNew.Frequency = cocSS.Frequency;
                                        objSLNew.GISStatus = cocSS.GISStatus;
                                        objSLNew.GravelContent = cocSS.GravelContent;
                                        objSLNew.GrossWeight = cocSS.GrossWeight;
                                        objSLNew.GroupSample = cocSS.GroupSample;
                                        objSLNew.Hold = cocSS.Hold;
                                        objSLNew.Humidity = cocSS.Humidity;
                                        objSLNew.IceCycle = cocSS.IceCycle;
                                        objSLNew.Increments = cocSS.Increments;
                                        objSLNew.Interval = cocSS.Interval;
                                        objSLNew.IsActive = cocSS.IsActive;
                                        //objSLNew.IsNotTransferred = cocSS.IsNotTransferred;
                                        objSLNew.ItemName = cocSS.ItemName;
                                        objSLNew.KeyMap = cocSS.KeyMap;
                                        objSLNew.LicenseNumber = cocSS.LicenseNumber;
                                        objSLNew.ManifestNo = cocSS.ManifestNo;
                                        objSLNew.MonitoryingRequirement = cocSS.MonitoryingRequirement;
                                        objSLNew.NoOfCollectionsEachTime = cocSS.NoOfCollectionsEachTime;
                                        objSLNew.NoOfPoints = cocSS.NoOfPoints;
                                        objSLNew.Notes = cocSS.Notes;
                                        objSLNew.OriginatingEntiry = cocSS.OriginatingEntiry;
                                        objSLNew.OriginatingLicenseNumber = cocSS.OriginatingLicenseNumber;
                                        objSLNew.PackageNumber = cocSS.PackageNumber;
                                        objSLNew.ParentSampleDate = cocSS.ParentSampleDate;
                                        objSLNew.ParentSampleID = cocSS.ParentSampleID;
                                        objSLNew.PiecesPerUnit = cocSS.PiecesPerUnit;
                                        objSLNew.Preservetives = cocSS.Preservetives;
                                        objSLNew.ProjectName = cocSS.ProjectName;
                                        objSLNew.PurifierSampleID = cocSS.PurifierSampleID;
                                        objSLNew.PWSID = cocSS.PWSID;
                                        if (cocSS.PWSSystemName != null)
                                        {
                                            objSLNew.PWSSystemName = uow.GetObjectByKey<PWSSystem>(cocSS.PWSSystemName.Oid);
                                        }
                                        objSLNew.RegionNameOfSection = cocSS.RegionNameOfSection;
                                        objSLNew.RejectionCriteria = cocSS.RejectionCriteria;
                                        objSLNew.RepeatLocation = cocSS.RepeatLocation;
                                        objSLNew.RetainedWeight = cocSS.RetainedWeight;
                                        objSLNew.RiverWidth = cocSS.RiverWidth;
                                        objSLNew.RushSample = cocSS.RushSample;
                                        objSLNew.SampleAmount = cocSS.SampleAmount;
                                        objSLNew.SampleCondition = cocSS.SampleCondition;
                                        objSLNew.SampleDescription = cocSS.SampleDescription;
                                        objSLNew.SampleImage = cocSS.SampleImage;
                                        objSLNew.SampleName = cocSS.SampleName;
                                        objSLNew.SamplePointID = cocSS.SamplePointID;
                                        objSLNew.SamplePointType = cocSS.SamplePointType;
                                        objSLNew.SampleSource = cocSS.SampleSource;
                                        objSLNew.SampleTag = cocSS.SampleTag;
                                        objSLNew.SampleWeight = cocSS.SampleWeight;
                                        objSLNew.SamplingAddress = cocSS.SamplingAddress;
                                        objSLNew.SamplingEquipment = cocSS.SamplingEquipment;
                                        objSLNew.SamplingLocation = cocSS.SamplingLocation;
                                        objSLNew.SamplingProcedure = cocSS.SamplingProcedure;
                                        objSLNew.SequenceTestSampleID = cocSS.SequenceTestSampleID;
                                        objSLNew.SequenceTestSortNo = cocSS.SequenceTestSortNo;
                                        objSLNew.ServiceArea = cocSS.ServiceArea;
                                        objSLNew.SiteCode = cocSS.SiteCode;
                                        objSLNew.SiteDescription = cocSS.SiteDescription;
                                        objSLNew.SiteID = cocSS.SiteID;
                                        objSLNew.SiteNameArchived = cocSS.SiteNameArchived;
                                        objSLNew.SiteUserDefinedColumn1 = cocSS.SiteUserDefinedColumn1;
                                        objSLNew.SiteUserDefinedColumn2 = cocSS.SiteUserDefinedColumn2;
                                        objSLNew.SiteUserDefinedColumn3 = cocSS.SiteUserDefinedColumn3;
                                        objSLNew.SubOut = cocSS.SubOut;
                                        if (cocSS.SystemType != null)
                                        {
                                            objSLNew.SystemType = uow.GetObjectByKey<SystemTypes>(cocSS.SystemType.Oid);
                                        }
                                        objSLNew.TargetMGTHC_CBD_mg_pc = cocSS.TargetMGTHC_CBD_mg_pc;
                                        objSLNew.TargetMGTHC_CBD_mg_unit = cocSS.TargetMGTHC_CBD_mg_unit;
                                        objSLNew.TargetPotency = cocSS.TargetPotency;
                                        objSLNew.TargetUnitWeight_g_pc = cocSS.TargetUnitWeight_g_pc;
                                        objSLNew.TargetUnitWeight_g_unit = cocSS.TargetUnitWeight_g_unit;
                                        objSLNew.TargetWeight = cocSS.TargetWeight;
                                        objSLNew.Time = cocSS.Time;
                                        objSLNew.TimeEnd = cocSS.TimeEnd;
                                        objSLNew.TimeStart = cocSS.TimeStart;
                                        objSLNew.TotalSamples = cocSS.TotalSamples;
                                        objSLNew.TotalTimes = cocSS.TotalTimes;
                                        if (cocSS.TtimeUnit != null)
                                        {
                                            objSLNew.TtimeUnit = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(cocSS.TtimeUnit.Oid);
                                        }
                                        if (cocSS.WaterType != null)
                                        {
                                            objSLNew.WaterType = uow.GetObjectByKey<WaterTypes>(cocSS.WaterType.Oid);
                                        }
                                        objSLNew.ZipCode = cocSS.ZipCode;
                                        //if (cocSS.ModifiedBy != null)
                                        //{
                                        //    objSLNew.ModifiedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.CustomSystemUser>(cocSS.ModifiedBy.Oid);
                                        //}
                                        //objSLNew.ModifiedDate = cocSS.ModifiedDate;
                                        objSLNew.Comment = cocSS.Comment;
                                        objSLNew.Latitude = cocSS.Latitude;
                                        objSLNew.Longitude = cocSS.Longitude;
                                        List<COCSettingsTest> lstcocTest = View.ObjectSpace.GetObjects<COCSettingsTest>(CriteriaOperator.Parse("[COCSettingsSamples] = ?", cocSS.Oid)).ToList();
                                        foreach (COCSettingsTest cocT in lstcocTest.ToList())
                                        {
                                            //SampleParameter objSP = os.CreateObject<SampleParameter>();
                                            COCSettingsTest objSP = new COCSettingsTest(uow);
                                            if (objSP != null)
                                            {
                                                if (cocT.Testparameter != null)
                                                {
                                                    objSP.Testparameter = uow.GetObjectByKey<Testparameter>(cocT.Testparameter.Oid);
                                                }
                                                if (cocT.COCSettingsSamples != null)
                                                {
                                                    objSP.COCSettingsSamples = objSLNew;
                                                }
                                            }
                                            objSP.Save();
                                        }
                                        objSLNew.Save();
                                        SampleNo++;
                                        List<COCSettingsBottleAllocation> lstcocBottle = View.ObjectSpace.GetObjects<COCSettingsBottleAllocation>(CriteriaOperator.Parse("[COCSettingsRegistration] = ?", cocSS.Oid)).ToList();
                                        foreach (COCSettingsBottleAllocation cocBA in lstcocBottle.ToList())
                                        {
                                            //SampleBottleAllocation smplnew = os.CreateObject<SampleBottleAllocation>();
                                            COCSettingsBottleAllocation smplnew = new COCSettingsBottleAllocation(uow);
                                            smplnew.COCSettingsRegistration = objSLNew;
                                            smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(cocBA.TestMethod.Oid);
                                            smplnew.BottleID = cocBA.BottleID;
                                            if (cocBA.Containers != null)
                                            {
                                                smplnew.Containers = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Container>(cocBA.Containers.Oid);
                                            }
                                            if (cocBA.Preservative != null)
                                            {
                                                smplnew.Preservative = uow.GetObjectByKey<Preservative>(cocBA.Preservative.Oid);
                                            }
                                            if (cocBA.StorageID != null)
                                            {
                                                smplnew.StorageID = uow.GetObjectByKey<Storage>(cocBA.StorageID.Oid);
                                            }
                                            if (cocBA.StorageCondition != null)
                                            {
                                                smplnew.StorageCondition = uow.GetObjectByKey<PreserveCondition>(cocBA.StorageCondition.Oid);
                                            }
                                            smplnew.Save();
                                        }
                                    }
                                    uow.CommitChanges();
                                }

                            }

                            //newObj.Active = objCOC.Active;
                            //newObj.ClientAddress = objCOC.ClientAddress;
                            //newObj.ClientAddress2 = objCOC.ClientAddress2;
                            //newObj.ClientName = objCOC.ClientName;
                            //newObj.COCName = objCOC.COCName;
                            //newObj.SamplingAddress = objCOC.SamplingAddress;
                            //newObj.ProjectID = objCOC.ProjectID;
                            //newObj.ProjectName = objCOC.ProjectName;
                            //newObj.ProjectLocation = objCOC.ProjectLocation;
                            //newObj.ProjectCategory = objCOC.ProjectCategory;
                            //newObj.ClientContact = objCOC.ClientContact;
                            //newObj.ClientPhone = objCOC.ClientPhone;
                            //newObj.SiteMapID = objCOC.SiteMapID;
                            //newObj.SampleMatries = objCOC.SampleMatries;
                            //newObj.ReportTemplate = objCOC.ReportTemplate;
                            //newObj.Retire = objCOC.Retire;
                            //newObj.Comment = objCOC.Comment;
                            //newObj.RecurrenceInfoXml = objCOC.RecurrenceInfoXml;
                            //newObj.StartOn = objCOC.StartOn;
                            //newObj.EndOn = objCOC.EndOn;
                            //newObj.BalanceID = objCOC.BalanceID;
                            //newObj.RetireDate = objCOC.RetireDate;
                            //newObj.SampleCategory = objCOC.SampleCategory;
                            //newObj.NoOfSamples = objCOC.NoOfSamples;
                            objCOC.Save();
                            uow.CommitChanges();
                            /*Attachment*/
                            //XPClassInfo attachmentInfo;
                            //attachmentInfo = uow1.GetClassInfo(typeof(Modules.BusinessObjects.SampleManagement.Attachment));
                            //IList<Modules.BusinessObjects.SampleManagement.Attachment> objAttachment = uow1.GetObjects(attachmentInfo, CriteriaOperator.Parse("[COCSettings]=?", objCOC.Oid), null, int.MaxValue, false, true).Cast<Modules.BusinessObjects.SampleManagement.Attachment>().ToList();
                            //if (objAttachment != null && objAttachment.Count > 0)
                            //{
                            //    foreach (Modules.BusinessObjects.SampleManagement.Attachment objAttch in objAttachment)
                            //    {
                            //        Modules.BusinessObjects.SampleManagement.Attachment objNewAttachment = new Modules.BusinessObjects.SampleManagement.Attachment(uow1);
                            //        objNewAttachment.Name = objAttch.Name;
                            //        objNewAttachment.Attachments = objAttch.Attachments;
                            //        objNewAttachment.Date = objAttch.Date;
                            //        objNewAttachment.Operator = objAttch.Operator;
                            //        objNewAttachment.CreatedDate = DateTime.Now;
                            //        objNewAttachment.CreatedBy = objAttch.CreatedBy;
                            //        if (newObj != null)
                            //        {
                            //            objNewAttachment.COCSettings = uow1.GetObjectByKey<COCSettings>(newObj.Oid);
                            //        }
                            //    }
                            //}

                            ///*Notes*/
                            //XPClassInfo NotesInfo;
                            //NotesInfo = uow1.GetClassInfo(typeof(Notes));
                            //IList<Notes> objnotes = uow1.GetObjects(NotesInfo, CriteriaOperator.Parse("[COCSettings]=?", objCOC.Oid), null, int.MaxValue, false, true).Cast<Notes>().ToList();
                            //if (objnotes != null && objnotes.Count > 0)
                            //{
                            //    foreach (Notes objN in objnotes)
                            //    {
                            //        Notes objNewNotes = new Notes(uow1);
                            //        if (objN.Author != null)
                            //        {
                            //            objNewNotes.Author = uow1.GetObjectByKey<Employee>(objN.Author.Oid);
                            //        }
                            //        objNewNotes.Date = DateTime.Now;
                            //        objNewNotes.COCSettings = newObj;
                            //        objNewNotes.Title = objN.Title;

                            //        if (objN.Attachment != null)
                            //        {
                            //            objNewNotes.Attachment = objN.Attachment;
                            //        }
                            //        objNewNotes.Text = objN.Text;
                            //    }
                            //}
                            ///*COCSettingsSamples*/
                            //XPClassInfo COCSettingsSamplesInfo;

                            //using (IObjectSpace os = Application.CreateObjectSpace())
                            //{
                            //    Session currentSession = ((XPObjectSpace)(os)).Session;
                            //    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                            //    COCSettings objCocSetting = uow.FindObject<COCSettings>(CriteriaOperator.Parse("[COC_ID]=? AND [EndOn] >= [NextUpdateDate] And Not IsNullOrEmpty([RecurrenceInfoXml])", newObj.COC_ID));

                            //    if (objCocSetting != null)
                            //    {
                            //        DateTime Now = new DateTime();
                            //        Now = DateTime.Today.AddDays(objCocSetting.BeforEventDays);
                            //    }
                            //}
                            if (View.Id == "COCSettings_DetailView")
                            {
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objspace, typeof(COCSettings));
                                //cs.Criteria["filter2"] = CriteriaOperator.Parse("");
                                ListView createListview = Application.CreateListView("COCSettings_ListView", cs, true);
                                Frame.SetView(createListview);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.ObjectSpace.Refresh();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

    }
}
