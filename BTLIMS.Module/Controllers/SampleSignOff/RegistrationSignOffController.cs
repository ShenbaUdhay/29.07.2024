using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.PLM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace LDM.Module.Controllers.SampleSignOff
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class RegistrationSignOffController : ViewController
    {
        viewInfo strviewid = new viewInfo();
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        NavigationInfo objNavInfo = new NavigationInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        Guid SampleCheckInOid = Guid.Empty;
        public RegistrationSignOffController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "Samplecheckin_ListView_Copy_RegistrationSigningOff;" + "Samplecheckin_DetailView_Copy_RegistrationSigningOff;" +
                "SampleParameter_ListView_Copy_RegistrationSignOffSamples;" + "Samplecheckin_DetailView_Copy_RegistrationSigningOff_History;" +
                "SampleRegistration;" + "Samplecheckin_ListView_Copy_RegistrationSigningOff_History;" + "SampleBottleAllocation_ListView_SignOff;"
                + "SampleBottleAllocation_ListView_SignedOff;" + "Samplecheckin_DetailView_Rollback;" + "RegistrationSignOff_ListView;" + "RegistrationSignOff_ListView_SignedOff;";
            SignOffAction.TargetViewId = "Samplecheckin_DetailView_Copy_RegistrationSigningOff;";
            SignedOffHistoryAction.TargetViewId = "Samplecheckin_ListView_Copy_RegistrationSigningOff;";
            SignedOffSamplesAction.TargetViewId = "Samplecheckin_DetailView_Copy_RegistrationSigningOff_History;";
            RegistrationSignOffDateFilterAction.TargetViewId = "Samplecheckin_ListView_Copy_RegistrationSigningOff_History;";
            RegistrationSignOffRollback.TargetViewId = "SampleBottleAllocation_ListView_SignedOff;" + "RegistrationSignOff_ListView_SignedOff;";
            SubmittedJobIDRollback.TargetViewId = "Samplecheckin_ListView_Copy_RegistrationSigningOff;" + "Samplecheckin_DetailView_Copy_RegistrationSigningOff;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View != null && View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History")
                {
                    strviewid.strtempviewid = View.Id.ToString();
                    if (View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History")
                    {
                        RegistrationSignOffDateFilterAction.SelectedIndex = 0;
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("DateDiffMonth(RecievedDate , Now()) <= 3 And [RecievedDate] Is Not Null");
                    }
                }
                // Perform various tasks depending on the target View.
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View != null && (View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_DetailView_Copy_RegistrationSigningOff"))
                {
                    objPermissionInfo.RegistrationSignOffIsWrite = false;
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    if (user.Roles != null && user.Roles.Count > 0)
                    {
                        if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.RegistrationSignOffIsWrite = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in user.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == objnavigationRefresh.ClickedNavigationItem && i.Write == true) != null)
                                {
                                    objPermissionInfo.RegistrationSignOffIsWrite = true;
                                    break;
                                }
                            }
                        }
                    }
                    SignOffAction.Active["ShowSignOff"] = objPermissionInfo.RegistrationSignOffIsWrite;
                    SubmittedJobIDRollback.Active["ShowJobIDRollBack"] = objPermissionInfo.RegistrationSignOffIsWrite;
                    RegistrationSignOffRollback.Active["ShowsignoffRollback"] = objPermissionInfo.RegistrationSignOffIsWrite;
                }
                if (View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                    IObjectSpace os = Application.CreateObjectSpace();
                    Session currentSession = ((XPObjectSpace)os).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<Guid> lstBottles = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.JobID != null && (i.SignOffBy == null || i.SignOffDate == null) && (i.SampleRegistration.JobID.Status == SampleRegistrationSignoffStatus.PartiallySignedOff ||
                       i.SampleRegistration.JobID.Status == SampleRegistrationSignoffStatus.PendingSigningOff)).Select(i => i.SampleRegistration.JobID.Oid).Distinct().ToList();
                    ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstBottles);
                    //os.Dispose();
                }
                else if (View.Id == "SampleRegistration")
                {
                    if (objnavigationRefresh.ClickedNavigationItem != "RegistrationSigningOff")
                    {
                        objPermissionInfo.RegistrationSignOffIsWrite = false;
                    }
                    if (!objNavInfo.SelectedNavigationCaption.Contains("("))
                    {
                        View.Caption = objNavInfo.SelectedNavigationCaption;
                    }
                    else
                    {
                        View.Caption = objNavInfo.SelectedNavigationCaption.Substring(0, (objNavInfo.SelectedNavigationCaption.IndexOf("(") - 1));
                    }
                    SignOffAction.Active["ShowSignOff"] = objPermissionInfo.RegistrationSignOffIsWrite;
                    RegistrationSignOffRollback.Active["SignOffRollBack"] = objPermissionInfo.RegistrationSignOffIsWrite;
                }
                else if (View.Id == "RegistrationSignOff_ListView")
                {
                    Samplecheckin objSamplecheckin = (Samplecheckin)Application.MainWindow.View.CurrentObject;
                    if (objSamplecheckin != null)
                    {
                        SelectedData result = ((XPObjectSpace)ObjectSpace).Session.ExecuteSproc("GetRegistrationSignOffSamples_Sp", new SprocParameter("@JobIDOid", objSamplecheckin.Oid));
                        foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                        {
                            RegistrationSignOff objNewSignOff = View.ObjectSpace.CreateObject<RegistrationSignOff>();
                            objNewSignOff.JobID = row.Values[0] != null ? (string)row.Values[0] : "";
                            objNewSignOff.SampleID = row.Values[1] != null ? (string)row.Values[1] : "";
                            objNewSignOff.SampleName = row.Values[3] != null ? (string)row.Values[3] : "";
                            objNewSignOff.TestName = row.Values[6] != null ? (string)row.Values[6] : "";
                            objNewSignOff.BottleID = row.Values[2] != null ? (string)row.Values[2] : "";
                            objNewSignOff.ReceivedBy = row.Values[4] != null ? (string)row.Values[4] : "";
                            objNewSignOff.ReceivedDate = row.Values[5] != null ? (DateTime)row.Values[5] : DateTime.MinValue;
                            objNewSignOff.SampleLogin = row.Values[8] != null ? View.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(new Guid(row.Values[8].ToString())) : null;
                            objNewSignOff.TestMethod = row.Values[7] != null ? (string)row.Values[7] : "";
                            ((ListView)View).CollectionSource.Add(objNewSignOff);
                        }
                    }
                }
                else if (View.Id == "RegistrationSignOff_ListView_SignedOff")
                {
                    Samplecheckin objSamplecheckin = (Samplecheckin)Application.MainWindow.View.CurrentObject;
                    if (objSamplecheckin != null)
                    {
                        SelectedData result = ((XPObjectSpace)ObjectSpace).Session.ExecuteSproc("GetRegistrationSignedOffSamples_Sp", new SprocParameter("@JobIDOid", objSamplecheckin.Oid));
                        foreach (SelectStatementResultRow row in result.ResultSet[0].Rows)
                        {
                            RegistrationSignOff objNewSignOff = View.ObjectSpace.CreateObject<RegistrationSignOff>();
                            objNewSignOff.JobID = row.Values[0] != null ? (string)row.Values[0] : "";
                            objNewSignOff.SampleID = row.Values[1] != null ? (string)row.Values[1] : "";
                            objNewSignOff.SampleName = row.Values[3] != null ? (string)row.Values[3] : "";
                            objNewSignOff.TestName = row.Values[6] != null ? (string)row.Values[6] : "";
                            objNewSignOff.BottleID = row.Values[2] != null ? (string)row.Values[2] : "";
                            objNewSignOff.ReceivedBy = row.Values[4] != null ? (string)row.Values[4] : "";
                            objNewSignOff.ReceivedDate = row.Values[5] != null ? (DateTime)row.Values[5] : DateTime.MinValue;
                            objNewSignOff.SampleLogin = row.Values[8] != null ? View.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(new Guid(row.Values[8].ToString())) : null;
                            objNewSignOff.TestMethod = row.Values[7] != null ? (string)row.Values[7] : "";
                            objNewSignOff.SignOffBy = row.Values[9] != null ? (string)row.Values[9] : "";
                            objNewSignOff.SignOffDate = row.Values[10] != null ? (DateTime)row.Values[10] : DateTime.MinValue;
                            ((ListView)View).CollectionSource.Add(objNewSignOff);
                        }
                    }
                }
                if (View is DetailView && View.ObjectTypeInfo.Type == typeof(Samplecheckin))
                {
                    Samplecheckin obj = (Samplecheckin)View.CurrentObject;
                    if (obj != null)
                    {
                        SRInfo.strJobID = obj.JobID;
                    }
                }
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
                if (e.PopupFrame.View != null && (e.PopupFrame.View.Id == "SampleBottleAllocation_ListView_SignOff" || e.PopupFrame.View.Id == "SampleBottleAllocation_ListView_SignedOff" || e.PopupFrame.View.Id == "RegistrationSignOff_ListView"
                    || e.PopupFrame.View.Id == "RegistrationSignOff_ListView_SignedOff"))
                {
                    string strheight = System.Web.HttpContext.Current.Request.Cookies.Get("height").Value;
                    //string strscreenheight = System.Web.HttpContext.Current.Request.Cookies.Get("screenheight").Value;
                    e.Width = new System.Web.UI.WebControls.Unit(1280);
                    e.Height = new System.Web.UI.WebControls.Unit(Convert.ToInt32(strheight));
                    //if (((ListView)e.PopupFrame.View).CollectionSource.GetCount() <= 13)
                    //{
                    //    e.Height = new System.Web.UI.WebControls.Unit();
                    //}
                    //else
                    //{
                    //    e.Height = new System.Web.UI.WebControls.Unit(780);
                    //}
                    e.Handled = true;
                }
                if (View.Id == "Samplecheckin_DetailView_Rollback")
                {
                    e.Height = 300;
                    e.Width = 800;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                Samplecheckin selObj = (Samplecheckin)e.InnerArgs.CurrentObject;
                if (selObj != null)
                {
                    SRInfo.strJobID = selObj.JobID;
                    IObjectSpace os = Application.CreateObjectSpace();
                    if (View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff")
                    {
                        Samplecheckin SCObj = os.GetObjectByKey<Samplecheckin>(selObj.Oid);
                        DetailView detailview = Application.CreateDetailView(os, "Samplecheckin_DetailView_Copy_RegistrationSigningOff", true, SCObj);
                        if (objPermissionInfo.RegistrationSignOffIsWrite)
                        {
                            detailview.ViewEditMode = ViewEditMode.Edit;
                        }
                        else
                        {
                            detailview.ViewEditMode = ViewEditMode.View;
                        }
                        Frame.SetView(detailview);
                        e.Handled = true;
                    }
                    else if (View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History")
                    {
                        Samplecheckin objJobID = (Samplecheckin)View.CurrentObject;
                        if (objJobID != null)
                        {

                            //IObjectSpace objectSpace = Application.CreateObjectSpace();
                            //IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstsmpllog = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", objJobID.JobID));
                            //CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleBottleAllocation));
                            //IList<Guid> objSLOid = lstsmpllog.Select(i => i.Oid).ToList();
                            //cs.Criteria["filter"] = new InOperator("SampleRegistration", objSLOid);
                            //ListView listview = Application.CreateListView("SampleBottleAllocation_ListView_SignedOff", cs, true);
                            //Frame.SetView(listview);
                            //e.Handled = true;
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

        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (View is DetailView)
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item is ASPxStringPropertyEditor)
                        {
                            ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }

                        }

                        else if (item is ASPxIntPropertyEditor)
                        {
                            ASPxIntPropertyEditor editor = (ASPxIntPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDoublePropertyEditor)
                        {
                            ASPxDoublePropertyEditor editor = (ASPxDoublePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDecimalPropertyEditor)
                        {
                            ASPxDecimalPropertyEditor editor = (ASPxDecimalPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }

                        }

                        else if (item is ASPxDateTimePropertyEditor)
                        {
                            ASPxDateTimePropertyEditor editor = (ASPxDateTimePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxLookupPropertyEditor)
                        {
                            ASPxLookupPropertyEditor editor = (ASPxLookupPropertyEditor)item;
                            if (editor != null && editor.DropDownEdit != null && editor.DropDownEdit.DropDown != null)
                            {
                                editor.DropDownEdit.DropDown.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxBooleanPropertyEditor)
                        {
                            ASPxBooleanPropertyEditor editor = (ASPxBooleanPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxEnumPropertyEditor)
                        {
                            ASPxEnumPropertyEditor editor = (ASPxEnumPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxGridLookupPropertyEditor)
                        {
                            ASPxGridLookupPropertyEditor editor = (ASPxGridLookupPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                    }
                }

                // Access and customize the target View control.
                if (View.Id == "SampleParameter_ListView_Copy_RegistrationSignOffSamples")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.SelectionChanged += Grid_SelectionChanged;
                        editor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                        editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        if (editor.Grid.Columns["DataReviewBatchDetailsAction"] != null)
                        {
                            editor.Grid.Columns["DataReviewBatchDetailsAction"].Caption = "Details";
                        }
                        if (((ListView)View).CollectionSource.GetCount() > 13)
                        {
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                            editor.Grid.Settings.VerticalScrollableHeight = 500;
                        }
                    }
                }
                else if (View.Id == "SampleBottleAllocation_ListView_SignOff")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.SelectionChanged += Grid_SelectionChanged;
                        editor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    }
                }
                else if (View.Id == "RegistrationSignOff_ListView")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.SelectionChanged += Grid_SelectionChanged;
                        editor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    }
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
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                var selected = gridListEditor.GetSelectedObjects();
                Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                Employee objEmp = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                if (View.Id == "RegistrationSignOff_ListView")
                {
                    foreach (RegistrationSignOff objbottle in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objbottle))
                        {
                            objbottle.SignOffDate = DateTime.Now;
                            objbottle.SignOffBy = user.DisplayName;

                        }
                        else
                        {
                            objbottle.SignOffDate = null;
                            objbottle.SignOffBy = null;
                        }
                    }
                }
                else
                {
                    foreach (SampleBottleAllocation objbottle in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objbottle))
                        {
                            objbottle.SignOffDate = DateTime.Now;
                            objbottle.SignOffBy = objEmp;

                        }
                        else
                        {
                            objbottle.SignOffDate = null;
                            objbottle.SignOffBy = null;
                        }
                    }
                }
                View.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnDeactivated()
        {
            try
            {
                // Unsubscribe from previously subscribed events and release other references and resources.
                base.OnDeactivated();
                if (View != null && View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History")
                {
                    strviewid.strtempviewid = string.Empty;
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SignOffAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_DetailView_Copy_RegistrationSigningOff" && View.CurrentObject != null)
                {
                    Samplecheckin objJobID = (Samplecheckin)View.CurrentObject;
                    if (objJobID != null)
                    {
                        bool isBottleAllocation = false;
                        IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstsmpllog = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", objJobID.JobID));
                        foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn objsmplelogin in lstsmpllog.ToList())
                        {
                            IList<SampleBottleAllocation> lstsmplalloc = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid] = ?", objsmplelogin.Oid));
                            if (lstsmplalloc != null && lstsmplalloc.Count > 0)
                            {
                                if (lstsmplalloc.FirstOrDefault(i => string.IsNullOrEmpty(i.BottleID)) == null)
                                {
                                    isBottleAllocation = true;
                                }
                                else
                                {
                                    isBottleAllocation = false;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assignBottleAllocation_Details"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                            else
                            {
                                isBottleAllocation = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assignBottleAllocation"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        if (isBottleAllocation)
                        {
                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleBottleAllocation));
                            IList<Guid> objSLOid = lstsmpllog.Select(i => i.Oid).ToList();
                            cs.Criteria["filter"] = new InOperator("SampleRegistration", objSLOid);
                            ListView listview = Application.CreateListView("SampleBottleAllocation_ListView_SignOff", cs, true);
                            ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.Accepting += Dc_Accepting;
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                            //IObjectSpace os = Application.CreateObjectSpace(typeof(RegistrationSignOff));
                            //CollectionSource cs = new CollectionSource(os, typeof(RegistrationSignOff));
                            //ListView listview = Application.CreateListView("RegistrationSignOff_ListView", cs, true);
                            //ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                            //showViewParameters.Context = TemplateContext.PopupWindow;
                            //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            //DialogController dc = Application.CreateController<DialogController>();
                            //dc.SaveOnAccept = false;
                            //dc.Accepting += Dc_Accepting;
                            //dc.CloseOnCurrentObjectProcessing = false;
                            //showViewParameters.Controllers.Add(dc);
                            //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                }
                else if (View.Id == "SampleRegistration")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    Samplecheckin objJobID = objectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", SRInfo.strJobID));
                    //if (objJobID != null)
                    //{
                    //    IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstsmpllog = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", objJobID.JobID));
                    //    foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn objsmplelogin in lstsmpllog.ToList())
                    //    {
                    //        IList<SampleBottleAllocation> lstsmplalloc = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid] = ?", objsmplelogin.Oid));
                    //        if (lstsmplalloc != null && lstsmplalloc.Count > 0)
                    //        {
                    //            foreach (SampleBottleAllocation objsmplalloc in lstsmplalloc.ToList())
                    //            {
                    //                if (!string.IsNullOrEmpty(objsmplalloc.BottleID) && !string.IsNullOrEmpty(objsmplalloc.SharedTests) && objsmplalloc.Qty > 0)
                    //                {
                    //                    CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleBottleAllocation));
                    //                    cs.Criteria["filter"] = CriteriaOperator.Parse("[SampleRegistration.JobID.Oid] = ?", objJobID.Oid);
                    //                    ListView listview = Application.CreateListView("SampleBottleAllocation_ListView_SignOff", cs, true);
                    //                    ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                    //                    showViewParameters.Context = TemplateContext.PopupWindow;
                    //                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //                    DialogController dc = Application.CreateController<DialogController>();
                    //                    dc.SaveOnAccept = false;
                    //                    dc.Accepting += Dc_Accepting;
                    //                    dc.AcceptAction.Executed += AcceptAction_Executed;
                    //                    dc.CloseOnCurrentObjectProcessing = false;
                    //                    showViewParameters.Controllers.Add(dc);
                    //                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //                }
                    //                else
                    //                {
                    //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assignBottleAllocation_Details"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //                    return;
                    //                    break;
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assignBottleAllocation"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //        }
                    //    }
                    //}
                    if (objJobID != null)
                    {
                        bool isBottleAllocation = false;
                        IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstsmpllog = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", objJobID.JobID));
                        foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn objsmplelogin in lstsmpllog.ToList())
                        {
                            IList<SampleBottleAllocation> lstsmplalloc = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid] = ?", objsmplelogin.Oid));
                            if (lstsmplalloc != null && lstsmplalloc.Count > 0)
                            {

                                if (lstsmplalloc.FirstOrDefault(i => string.IsNullOrEmpty(i.BottleID)) == null)
                                {
                                    isBottleAllocation = true;
                                }
                                else
                                {
                                    isBottleAllocation = false;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assignBottleAllocation_Details"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                            else
                            {
                                isBottleAllocation = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assignBottleAllocation"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        if (isBottleAllocation)
                        {
                            //IObjectSpace objectSpace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleBottleAllocation));
                            IList<Guid> objSLOid = lstsmpllog.Select(i => i.Oid).ToList();
                            cs.Criteria["filter"] = new InOperator("SampleRegistration", objSLOid);
                            //cs.Criteria["filter"] = CriteriaOperator.Parse("[SampleRegistration.JobID.Oid] = ?", objJobID.Oid);
                            ListView listview = Application.CreateListView("SampleBottleAllocation_ListView_SignOff", cs, true);
                            ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.Accepting += Dc_Accepting;
                            //dc.AcceptAction.Executed += AcceptAction_Executed;
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        //private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        //{
        //    try
        //    {
        //        IObjectSpace os = Application.CreateObjectSpace();
        //        Samplecheckin objCheckin = os.GetObject<Samplecheckin>(Application.MainWindow.View.CurrentObject as Samplecheckin);
        //        if (objCheckin != null)
        //        {
        //            IList<Modules.BusinessObjects.SampleManagement.SampleParameter> lstSamples = os.GetObjects<Modules.BusinessObjects.SampleManagement.SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And ([SubOut] Is Null Or [SubOut] = False)", objCheckin.Oid));
        //            if (lstSamples != null && lstSamples.Count > 0)
        //            {
        //                if (lstSamples.FirstOrDefault(i => i.SignOff == false) == null)
        //                {
        //                    objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.Signedoff;
        //                    os.CommitChanges();
        //                    os.Dispose();
        //                }
        //                else
        //                if (lstSamples.Where(i => i.SignOff).Count() == 0)
        //                {
        //                    objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSigningOff;
        //                    os.CommitChanges();
        //                    os.Dispose();
        //                }
        //                else
        //                if (lstSamples.Where(i => i.SignOff).Count() < lstSamples.Count)
        //                {
        //                    objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PartiallySignedOff;
        //                    os.CommitChanges();
        //                    os.Dispose();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs a)
        {
            try
            {
                var dc = (DialogController)sender;
                dc.Window.View.ObjectSpace.CommitChanges();
                if (a.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    bool CanCommit = false;
                    Session currentSession = ((XPObjectSpace)ObjectSpace).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    foreach (SampleBottleAllocation objBottle in a.AcceptActionArgs.SelectedObjects.Cast<SampleBottleAllocation>().ToList())
                    {
                        XPClassInfo sampleParameterinfo;
                        sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                        IList<SampleParameter> lstSampleParam = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.Oid]=? and [Testparameter.TestMethod.Oid]=?", objBottle.SampleRegistration.Oid, objBottle.TestMethod.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                        if (lstSampleParam.Count > 0)
                        {
                            lstSampleParam.ToList().ForEach(i => { i.SignOff = true; });
                            CanCommit = true;
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assigntesttosample"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    //foreach (RegistrationSignOff objBottle in a.AcceptActionArgs.SelectedObjects.Cast<RegistrationSignOff>().ToList())
                    //{
                    //    XPClassInfo sampleParameterinfo;
                    //    sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                    //    IList<SampleParameter> lstSampleParam = uow.GetObjects(sampleParameterinfo, new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Samplelogin.Oid]=?", objBottle.SampleLogin.Oid), new InOperator("Testparameter.TestMethod.Oid", objBottle.TestMethod.Split(';').ToList().Select(i => new Guid(i = i.Trim())).ToList())), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                    //    //IList<SampleParameter> lstSampleParam = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.Oid]=? and [Testparameter.TestMethod.Oid]=?", objBottle.SampleLogin.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                    //    if (lstSampleParam.Count > 0)
                    //    {
                    //        lstSampleParam.ToList().ForEach(i => { i.SignOff = true; });
                    //        XPClassInfo BottleAllocationinfo = uow.GetClassInfo(typeof(SampleBottleAllocation));
                    //        IList<SampleBottleAllocation> lstSampleBottle = uow.GetObjects(BottleAllocationinfo, new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[SampleRegistration.Oid]=?", objBottle.SampleLogin.Oid), new InOperator("TestMethod.Oid", objBottle.TestMethod.Split(';').ToList().Select(i =>new Guid(i = i.Trim())).ToList())), new SortingCollection(), 0, 0, false, true).Cast<SampleBottleAllocation>().ToList();
                    //        if (lstSampleBottle.Count > 0)
                    //        {
                    //            lstSampleBottle.ToList().ForEach(i => { i.SignOffBy = uow.GetObjectByKey<Employee>(user.Oid); i.SignOffDate = objBottle.SignOffDate; });
                    //        }
                    //        CanCommit = true;
                    //    }
                    //    else
                    //    {
                    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "assigntesttosample"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    //        return;
                    //    }
                    //}
                    if (CanCommit)
                    {
                        uow.CommitChanges();
                        Samplecheckin objCheckin = null;
                        if (Application.MainWindow.View is DetailView)
                        {
                            Samplecheckin objCurrent = (Samplecheckin)Application.MainWindow.View.CurrentObject;
                            objCheckin = uow.GetObjectByKey<Samplecheckin>(objCurrent.Oid);
                        }
                        else if (Application.MainWindow.View is DashboardView)
                        {
                            objCheckin = uow.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", SRInfo.strJobID));
                        }
                        if (objCheckin != null)
                        {
                            XPClassInfo sampleParameterinfo;
                            sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                            IList<SampleParameter> lstSamples = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ?  and [Samplelogin.GCRecord] is NULL", objCheckin.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                            if (lstSamples != null && lstSamples.Count > 0)
                            {
                                if (lstSamples.FirstOrDefault(i => i.SignOff == false) == null)
                                {
                                    objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.Signedoff;
                                    objCheckin.DateTimeSignedOff = DateTime.Now;
                                    objCheckin.Save();
                                    uow.CommitChanges();
                                }
                                else
                                if (lstSamples.Where(i => i.SignOff).Count() < lstSamples.Count)
                                {
                                    objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PartiallySignedOff;
                                    objCheckin.DateTimeSignedOff = DateTime.Now;
                                    objCheckin.Save();
                                    uow.CommitChanges();
                                }
                                else
                                if (lstSamples.Where(i => !i.SignOff).Count() == lstSamples.Count)
                                {
                                    objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSigningOff;
                                    objCheckin.DateTimeSignedOff = DateTime.Now;
                                    objCheckin.Save();
                                    uow.CommitChanges();
                                }
                            }
                            if (Application.MainWindow.View is DetailView)
                            {
                                Application.MainWindow.View.ObjectSpace.Refresh();
                            }
                            else if (Application.MainWindow.View is DashboardView)
                            {
                                DashboardViewItem SCRegDetailView = ((DashboardView)Application.MainWindow.View).FindItem("SampleCheckin") as DashboardViewItem;
                                if (SCRegDetailView != null && SCRegDetailView.InnerView != null)
                                {
                                    SCRegDetailView.InnerView.ObjectSpace.Refresh();
                                }
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "SamplesSignedOffSuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    //os.Dispose();
                    //PendingSigningOffJobIDCount();
                    Samplecheckin objSC = Application.MainWindow.View.CurrentObject as Samplecheckin;
                    if (objSC != null && objSC.ProjectCategory != null && (objSC.ProjectCategory.CategoryName == "PT" || objSC.ProjectCategory.CategoryName == "DOC" || objSC.ProjectCategory.CategoryName == "MDL"))
                    {
                        Samplecheckin lstobj = uow.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]= ?", objSC.Oid));
                        PTStudyLog Objstudylog = uow.FindObject<PTStudyLog>(CriteriaOperator.Parse("[SampleCheckinJobID.JobID]= ?", objSC.JobID));
                        if (Objstudylog == null)
                        {
                            PTStudyLog objPT = new PTStudyLog(uow);
                            objPT.JobID = lstobj.JobID;
                            objPT.DatePTSampleReceived = lstobj.RecievedDate;
                            objPT.SampleCheckinJobID = lstobj;
                            objPT.Category = lstobj.ProjectCategory.CategoryName;
                            objPT.Save();
                            uow.CommitChanges();
                            foreach (SampleBottleAllocation objBottle in a.AcceptActionArgs.SelectedObjects.Cast<SampleBottleAllocation>().ToList())
                            {
                                SampleBottleAllocation obj = uow.GetObjectByKey<SampleBottleAllocation>(objBottle.Oid);
                                if (obj != null)
                                {
                                    XPClassInfo sampleParameterinfo;
                                    sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                                    IList<SampleParameter> lstSampleParam = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.Oid]=? and [Testparameter.TestMethod.Oid]=?", obj.SampleRegistration.Oid, obj.TestMethod.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                                    foreach (SampleParameter objParam in lstSampleParam)
                                    {
                                        PTStudyLogResults objPTRes = new PTStudyLogResults(uow);
                                        SampleParameter objParameter = uow.GetObjectByKey<SampleParameter>(objParam.Oid);
                                        objPTRes.PTStudyLog = objPT;
                                        objPTRes.SampleID = objParameter;
                                        objPTRes.Save();
                                    }
                                }

                            }
                            uow.CommitChanges();
                        }
                        else
                        {
                            Objstudylog.JobID = lstobj.JobID;
                            Objstudylog.DatePTSampleReceived = lstobj.RecievedDate;
                            Objstudylog.SampleCheckinJobID = lstobj;
                            Objstudylog.Category = lstobj.ProjectCategory.CategoryName;
                            uow.CommitChanges();
                            foreach (SampleBottleAllocation objBottle in a.AcceptActionArgs.SelectedObjects.Cast<SampleBottleAllocation>().ToList())
                            {
                                SampleBottleAllocation obj = uow.GetObjectByKey<SampleBottleAllocation>(objBottle.Oid);
                                if (obj != null)
                                {
                                    XPClassInfo sampleParameterinfo;
                                    sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                                    IList<SampleParameter> lstSampleParam = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.Oid]=? and [Testparameter.TestMethod.Oid]=?", obj.SampleRegistration.Oid, obj.TestMethod.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                                    foreach (SampleParameter objParam in lstSampleParam)
                                    {
                                        PTStudyLogResults objPTRes = new PTStudyLogResults(uow);
                                        SampleParameter objParameter = uow.GetObjectByKey<SampleParameter>(objParam.Oid);
                                        objPTRes.PTStudyLog = Objstudylog;
                                        objPTRes.SampleID = objParameter;
                                        objPTRes.Save();
                                    }
                                }

                            }
                            uow.CommitChanges();
                        }
                    }
                }
                else
                {
                    a.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //public void PendingSigningOffJobIDCount()
        //{
        //    try
        //    {
        //        ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
        //        if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
        //        {
        //            ChoiceActionItem parent = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "SampleManagement");
        //            if (parent != null)
        //            {
        //                ChoiceActionItem child = parent.Items.FirstOrDefault(i => i.Id == "RegistrationSigningOff");
        //                if (child != null && child.Active==true)
        //                {
        //                    int count = 0;
        //                    IObjectSpace objSpace = Application.CreateObjectSpace();
        //                    //IList<Samplecheckin> lstPendingRegistrationSignOff = objSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse("[Status] = 'PendingSigningOff' Or [Status] = 'PartiallySignedOff'"));

        //                    using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.SampleParameter)))
        //                    {
        //                        lstview.Criteria = CriteriaOperator.Parse("([Samplelogin.JobID.Status] = 'PendingSigningOff' Or [Samplelogin.JobID.Status] = 'PartiallySignedOff') And Not IsNullOrEmpty([Samplelogin.JobID.JobID]) And ([SubOut] Is Null Or [SubOut] = False) and [Samplelogin.GCRecord] is NULL and [Samplelogin.JobID.GCRecord] is NULL");
        //                        lstview.Properties.Add(new ViewProperty("JobID", DevExpress.Xpo.SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
        //                        lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
        //                        List<object> jobid = new List<object>();
        //                        if (lstview != null)
        //                        {
        //                            //foreach (ViewRecord rec in lstview)
        //                            //    jobid.Add(rec["Toid"]);
        //                            foreach (ViewRecord rec in lstview)
        //                            {
        //                                SampleParameter objsample = objSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[Oid]= ?", new Guid(rec["Toid"].ToString())));
        //                                if (objsample != null && objsample.Samplelogin.Oid != null)
        //                                {
        //                                    IList<SampleBottleAllocation> ObjbottleAllocation = objSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.JobID.Oid] = ? And [SignOffDate] Is Null And [SignOffBy] Is Null", objsample.Samplelogin.JobID.Oid));
        //                                    foreach (SampleBottleAllocation objSamplebottle in ObjbottleAllocation)
        //                                    {
        //                                        if (objSamplebottle != null && objSamplebottle.SharedTests != null)
        //                                        {
        //                                            if (!jobid.Contains(rec["Toid"]))
        //                                            {
        //                                                jobid.Add(rec["Toid"]);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        count = jobid.Count;
        //                    }
        //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                    if (count > 0)
        //                    {
        //                        child.Caption = cap[0] + " (" + count + ")";
        //                    }
        //                    else
        //                    {
        //                        child.Caption = cap[0];
        //                    }
        //                }
        //            }
        //            ChoiceActionItem dataentryNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "DataEntry");
        //            if (dataentryNode != null)
        //            {
        //                ChoiceActionItem child = dataentryNode.Items.FirstOrDefault(i => i.Id == "AnalysisQueue" || i.Id == "AnalysisQueue ");
        //                if (child != null && child.Active==true)
        //                {
        //                    int count = 0;
        //                    IObjectSpace objSpace = Application.CreateObjectSpace();
        //                    IList<SampleParameter> lstTests = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SignOff] = True  And [Samplelogin.IsNotTransferred] = false And ([SubOut] Is Null Or [SubOut] = False) And [UQABID] Is Null And [QCBatchID] Is Null And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [SamplePrepBatchID] Is Not Null) Or [Testparameter.TestMethod.PrepMethods][].Count() = 0)"));
        //                    if (lstTests != null && lstTests.Count > 0)
        //                    {
        //                        //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
        //                        count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
        //                    }
        //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                    if (count > 0)
        //                    {
        //                        child.Caption = cap[0] + " (" + count + ")";
        //                    }
        //                    else
        //                    {
        //                        child.Caption = cap[0];
        //                    }
        //                }
        //            }
        //            ChoiceActionItem parentSamplePreparationRootNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "SamplePreparationRootNode");
        //            if (parentSamplePreparationRootNode != null)
        //            {
        //                ChoiceActionItem child = parentSamplePreparationRootNode.Items.FirstOrDefault(i => i.Id == "SamplePreparation");
        //                if (child != null && child.Active==true)
        //                {
        //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
        //                    int objperpCount = objectSpace.GetObjects<TestMethod>().ToList().Where(i => i.NoOfPrepSamples > 0).Count();
        //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
        //                    if (objperpCount > 0)
        //                    {
        //                        child.Caption = cap[0] + " (" + objperpCount + ")";
        //                    }
        //                    else
        //                    {
        //                        child.Caption = cap[0];
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void SignedOffHistoryAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff")//Samplecheckin_ListView_Copy_RegistrationSigningOff_History
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(Modules.BusinessObjects.SampleManagement.Samplecheckin));
                    ListView listview = Application.CreateListView("Samplecheckin_ListView_Copy_RegistrationSigningOff_History", cs, true);
                    Frame.SetView(listview);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SignedOffSamplesAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_DetailView_Copy_RegistrationSigningOff_History" && View.CurrentObject != null)
                {
                    Samplecheckin objJobID = (Samplecheckin)View.CurrentObject;
                    if (objJobID != null)
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstsmpllog = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", objJobID.JobID));
                        CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleBottleAllocation));
                        IList<Guid> objSLOid = lstsmpllog.Select(i => i.Oid).ToList();
                        cs.Criteria["filter"] = new InOperator("SampleRegistration", objSLOid);
                        //cs.Criteria["filter"] = CriteriaOperator.Parse("[SampleRegistration] = ?", objJobID.Oid);
                        ListView listview = Application.CreateListView("SampleBottleAllocation_ListView_SignedOff", cs, true);
                        ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                        //IObjectSpace os = Application.CreateObjectSpace(typeof(RegistrationSignOff));
                        //CollectionSource cs = new CollectionSource(os, typeof(RegistrationSignOff));
                        //ListView listview = Application.CreateListView("RegistrationSignOff_ListView_SignedOff", cs, true);
                        //ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                        //showViewParameters.Context = TemplateContext.PopupWindow;
                        //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        //DialogController dc = Application.CreateController<DialogController>();
                        //dc.AcceptAction.Active.SetItemValue("disable", false);
                        //dc.CancelAction.Active.SetItemValue("disable", false);
                        //dc.CloseOnCurrentObjectProcessing = false;
                        //dc.CloseOnCurrentObjectProcessing = false;
                        //showViewParameters.Controllers.Add(dc);
                        //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                    }
                }
                else if (View.Id == "SampleRegistration")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    Samplecheckin objJobID = objectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", SRInfo.strJobID));
                    if (objJobID != null)
                    {
                        ////CollectionSource cs = new CollectionSource(objectSpace, typeof(Modules.BusinessObjects.SampleManagement.SampleParameter));
                        //IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstsmpllog = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] = ?", objJobID.JobID));
                        //CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleBottleAllocation));
                        //IList<Guid> objSLOid = lstsmpllog.Select(i => i.Oid).ToList();
                        //cs.Criteria["filter"] = new InOperator("SampleRegistration", objSLOid);
                        ////cs.Criteria["filter"] = CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ?", objJobID.Oid);
                        //ListView listview = Application.CreateListView("SampleBottleAllocation_ListView_SignedOff", cs, true);
                        //ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                        //showViewParameters.Context = TemplateContext.PopupWindow;
                        //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        //DialogController dc = Application.CreateController<DialogController>();
                        //dc.AcceptAction.Active.SetItemValue("disable", false);
                        //dc.CancelAction.Active.SetItemValue("disable", false);
                        //dc.CloseOnCurrentObjectProcessing = false;
                        //showViewParameters.Controllers.Add(dc);
                        //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));

                        IObjectSpace os = Application.CreateObjectSpace(typeof(RegistrationSignOff));
                        CollectionSource cs = new CollectionSource(os, typeof(RegistrationSignOff));
                        ListView listview = Application.CreateListView("RegistrationSignOff_ListView_SignedOff", cs, true);
                        ShowViewParameters showViewParameters = new ShowViewParameters(listview);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("disable", false);
                        dc.CancelAction.Active.SetItemValue("disable", false);
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_ViewClosed(object sender, EventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                Samplecheckin objCheckin = null;
                if (Application.MainWindow.View is DetailView)
                {
                    objCheckin = os.GetObject<Samplecheckin>(Application.MainWindow.View.CurrentObject as Samplecheckin);
                }
                else if (Application.MainWindow.View is DashboardView)
                {
                    objCheckin = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", SRInfo.strJobID));
                }
                if (objCheckin != null)
                {
                    IList<Modules.BusinessObjects.SampleManagement.SampleParameter> lstSamples = os.GetObjects<Modules.BusinessObjects.SampleManagement.SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ?  and [Samplelogin.GCRecord] is NULL", objCheckin.Oid));
                    if (lstSamples != null && lstSamples.Count > 0)
                    {
                        if (lstSamples.FirstOrDefault(i => i.SignOff == false) == null)
                        {
                            objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.Signedoff;
                            os.CommitChanges();
                        }
                        else
                        if (lstSamples.Where(i => i.SignOff).Count() < lstSamples.Count)
                        {
                            objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PartiallySignedOff;
                            os.CommitChanges();
                        }
                        else
                        if (lstSamples.Where(i => !i.SignOff).Count() == lstSamples.Count)
                        {
                            objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSigningOff;
                            os.CommitChanges();
                        }
                    }
                    if (Application.MainWindow.View is DetailView)
                    {
                        Application.MainWindow.View.ObjectSpace.Refresh();
                    }
                    else if (Application.MainWindow.View is DashboardView)
                    {
                        DashboardViewItem SCRegDetailView = ((DashboardView)Application.MainWindow.View).FindItem("SampleCheckin") as DashboardViewItem;
                        if (SCRegDetailView != null && SCRegDetailView.InnerView != null)
                        {
                            SCRegDetailView.InnerView.ObjectSpace.Refresh();
                        }
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "SamplesSignedOffSuccessfully"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                //PendingSigningOffJobIDCount();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RegistrationSignOffDateFilterAction_SelectedItemChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History")
                {
                    DateTime srDateFilter = DateTime.MinValue;
                    if (RegistrationSignOffDateFilterAction != null && RegistrationSignOffDateFilterAction.SelectedItem != null)
                    {
                        if (RegistrationSignOffDateFilterAction.SelectedItem.Id == "3M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-3);
                        }
                        else if (RegistrationSignOffDateFilterAction.SelectedItem.Id == "6M")
                        {
                            srDateFilter = DateTime.Today.AddMonths(-6);
                        }
                        else if (RegistrationSignOffDateFilterAction.SelectedItem.Id == "1Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-1);
                        }
                        else if (RegistrationSignOffDateFilterAction.SelectedItem.Id == "2Y")
                        {
                            srDateFilter = DateTime.Today.AddYears(-2);
                        }
                    }
                    if (srDateFilter != DateTime.MinValue)
                    {
                        //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[RecievedDate] BETWEEN('" + srDateFilter + "', '" + DateTime.Now + "')");
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", srDateFilter, DateTime.Now);

                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria.Remove("Filter");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Rollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    bool isUsedSamples = false;
                    foreach (SampleBottleAllocation objBottle in View.SelectedObjects)
                    {
                        SampleBottleAllocation obj = View.ObjectSpace.GetObject<SampleBottleAllocation>(objBottle);
                        if (obj != null)
                        {
                            IList<SampleParameter> objSamples = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? and [Samplelogin.Oid] = ?", obj.TestMethod.Oid, obj.SampleRegistration.Oid));
                            if (objSamples.FirstOrDefault(i => i.QCBatchID != null || i.ABID != null) != null)
                            {
                                isUsedSamples = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotrollback"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else if (objSamples.FirstOrDefault(i => i.PrepBatchID != null || i.PrepMethodCount > 0) != null)
                            {
                                isUsedSamples = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "alreadyusedpreparation"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                            else if (objSamples.FirstOrDefault(i => i.ResultNumeric != null || i.ABID != null) != null)
                            {
                                isUsedSamples = true;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotrollback"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                    //foreach (RegistrationSignOff obj in View.SelectedObjects)
                    //{
                    //    //SampleBottleAllocation obj = View.ObjectSpace.GetObject<SampleBottleAllocation>(objBottle);
                    //    if (obj != null)
                    //    {
                    //        IList<SampleParameter> objSamples = View.ObjectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And,CriteriaOperator.Parse("[Samplelogin.Oid] = ?",obj.SampleLogin.Oid, new InOperator("TestMethod.Oid", obj.TestMethod.Split(';').ToList().Select(i => new Guid(i = i.Trim())).ToList()))));
                    //        if (objSamples.FirstOrDefault(i => i.QCBatchID != null || i.ABID != null) != null)
                    //        {
                    //            isUsedSamples = true;
                    //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotrollback"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //            return;
                    //        }
                    //        else if (objSamples.FirstOrDefault(i => i.PrepBatchID != null || i.PrepMethodCount > 0) != null)
                    //        {
                    //            isUsedSamples = true;
                    //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "alreadyusedpreparation"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //            return;
                    //        }
                    //        else if (objSamples.FirstOrDefault(i => i.ResultNumeric != null || i.ABID != null) != null)
                    //        {
                    //            isUsedSamples = true;
                    //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotrollback"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //            return;
                    //        }
                    //    }
                    //}
                    if (!isUsedSamples)
                    {
                        IObjectSpace os = Application.CreateObjectSpace(typeof(SampleBottleAllocation));
                        SampleBottleAllocation obj = os.CreateObject<SampleBottleAllocation>();
                        DetailView createdView = Application.CreateDetailView(os, "SampleBottleAllocation_DetailView_RollBack", true, obj);
                        createdView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += RollBackReason_Accepting;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RollBackReason_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                SampleBottleAllocation objCancel = (SampleBottleAllocation)e.AcceptActionArgs.CurrentObject;
                if (!string.IsNullOrEmpty(objCancel.RollbackReason) && !string.IsNullOrWhiteSpace(objCancel.RollbackReason))
                {
                    DevExpress.ExpressApp.Web.PopupWindow nestedFrame = (DevExpress.ExpressApp.Web.PopupWindow)Frame;
                    foreach (SampleBottleAllocation objBottle in nestedFrame.View.SelectedObjects.Cast<SampleBottleAllocation>().ToList())
                    {
                        SampleBottleAllocation obj = uow.GetObjectByKey<SampleBottleAllocation>(objBottle.Oid);
                        bool CanCommit = false;
                        if (obj != null)
                        {
                            XPClassInfo sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                            IList<SampleParameter> objSamples = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? and [Samplelogin.Oid] = ?", obj.TestMethod.Oid, obj.SampleRegistration.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                            if (objSamples.Count > 0)
                            {
                                foreach (SampleParameter objParam in objSamples)
                                {
                                    objParam.SignOff = false;
                                    CanCommit = true;
                                }
                                //uow.CommitChanges();
                            }
                            Samplecheckin objSC = Application.MainWindow.View.CurrentObject as Samplecheckin;
                            if (CanCommit)
                            {
                                obj.RollbackReason = objCancel.RollbackReason;
                                obj.RollbackedBy = uow.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                obj.RollbackedDate = DateTime.Now;
                                obj.SignOffBy = null;
                                obj.SignOffDate = null;
                            }
                            //uow.CommitChanges();
                        }
                    }
                    //foreach (RegistrationSignOff obj in nestedFrame.View.SelectedObjects.Cast<RegistrationSignOff>().ToList())
                    //{
                    //    //SampleBottleAllocation obj = uow.GetObjectByKey<SampleBottleAllocation>(objBottle.Oid);
                    //    bool CanCommit = false;
                    //    if (obj != null)
                    //    {
                    //        XPClassInfo sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                    //        IList<SampleParameter> objSamples = uow.GetObjects(sampleParameterinfo, new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Samplelogin.Oid]=?", obj.SampleLogin.Oid), new InOperator("Testparameter.TestMethod.Oid", obj.TestMethod.Split(';').ToList().Select(i => new Guid(i = i.Trim())).ToList())), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                    //        if (objSamples.Count > 0)
                    //        {
                    //            foreach (SampleParameter objParam in objSamples)
                    //            {
                    //                objParam.SignOff = false;
                    //                CanCommit = true;
                    //            }
                    //            uow.CommitChanges();
                    //        }
                    //        Samplecheckin objSC = Application.MainWindow.View.CurrentObject as Samplecheckin;
                    //        if (CanCommit)
                    //        {
                    //            XPClassInfo BottleAllocationinfo = uow.GetClassInfo(typeof(SampleBottleAllocation));
                    //            IList<SampleBottleAllocation> lstSampleBottle = uow.GetObjects(BottleAllocationinfo, new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[SampleRegistration.Oid]=?", obj.SampleLogin.Oid), new InOperator("TestMethod.Oid", obj.TestMethod.Split(';').ToList().Select(i => new Guid(i = i.Trim())).ToList())), new SortingCollection(), 0, 0, false, true).Cast<SampleBottleAllocation>().ToList();
                    //            if (lstSampleBottle.Count > 0)
                    //            {
                    //                lstSampleBottle.ToList().ForEach(i => { i.RollbackReason= objCancel.RollbackReason; i.RollbackedBy = uow.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);i.RollbackedDate = DateTime.Now; i.SignOffBy = null;i.SignOffDate = null; });
                    //            }
                    //            //obj.RollbackReason = objCancel.RollbackReason;
                    //            //obj.RollbackedBy = uow.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    //            //obj.RollbackedDate = DateTime.Now;
                    //            //obj.SignOffBy = null;
                    //            //obj.SignOffDate = null;
                    //        }
                    //        uow.CommitChanges();
                    //    }
                    //}
                    uow.CommitChanges();
                    Samplecheckin objCheckin = null;
                    if (SRInfo.strJobID != null)
                    {
                        objCheckin = uow.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", SRInfo.strJobID));
                    }
                    if (objCheckin != null)
                    {
                        XPClassInfo sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                        IList<SampleParameter> lstSamples = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? and [Samplelogin.GCRecord] is NULL", objCheckin.Oid), new SortingCollection(), 0, 0, false, true).Cast<SampleParameter>().ToList();
                        if (lstSamples != null && lstSamples.Count > 0)
                        {
                            if (lstSamples.Where(i => !i.SignOff).Count() == lstSamples.Count)
                            {
                                objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PendingSigningOff;
                                //uow.CommitChanges();
                            }
                            else
                            {
                                objCheckin.Status = Modules.BusinessObjects.Hr.SampleRegistrationSignoffStatus.PartiallySignedOff;
                                //uow.CommitChanges();
                            }
                        }
                    }
                    uow.CommitChanges();
                    View.ObjectSpace.Refresh();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void RollbackJobID_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(Samplecheckin));
                    Samplecheckin obj = os.CreateObject<Samplecheckin>();
                    SRInfo.bolNewJobID = false;
                    DetailView createdView = Application.CreateDetailView(os, "Samplecheckin_DetailView_Rollback", true, obj);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += JobIDRollBackReason_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void JobIDRollBackReason_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Samplecheckin objSamplecheckin = (Samplecheckin)e.AcceptActionArgs.CurrentObject;
                if (objSamplecheckin != null && !string.IsNullOrEmpty(objSamplecheckin.RollbackReason) && !string.IsNullOrWhiteSpace(objSamplecheckin.RollbackReason))
                {
                    SRInfo.bolNewJobID = false;
                    foreach (Samplecheckin objSampleCheck in View.SelectedObjects)
                    {
                        objSampleCheck.RollbackReason = objSamplecheckin.RollbackReason;
                        objSampleCheck.RollbackedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        objSampleCheck.RollbackedDate = DateTime.Now;
                        objSampleCheck.Status = objSamplecheckin.Status = SampleRegistrationSignoffStatus.PendingSubmit;
                    }
                    View.ObjectSpace.CommitChanges();
                    if (View is ListView)
                    {
                        Session currentSession = ((XPObjectSpace)View.ObjectSpace).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        List<Guid> lstBottles = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.JobID != null && (i.SignOffBy == null || i.SignOffDate == null) && (i.SampleRegistration.JobID.Status == SampleRegistrationSignoffStatus.PartiallySignedOff ||
                           i.SampleRegistration.JobID.Status == SampleRegistrationSignoffStatus.PendingSigningOff)).Select(i => i.SampleRegistration.JobID.Oid).Distinct().ToList();
                        ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstBottles);
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    if (View is DetailView)
                    {
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(Samplecheckin));
                        ListView createListview = Application.CreateListView("Samplecheckin_ListView_Copy_RegistrationSigningOff", cs, true);
                        Frame.SetView(createListview);
                    }
                    //Frame.GetController<SampleRegistrationViewController>().ResetNavigationCount();
                    //PendingSigningOffJobIDCount();
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
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
