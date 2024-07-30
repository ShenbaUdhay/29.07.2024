using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using DevExpress.Web.Data;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DefaultSettingViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        DefaultSettingInfo objDefaultInfo = new DefaultSettingInfo();
        public DefaultSettingViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "DefaultSetting_ListView_2;" + "DefaultSetting_ListView_2_DefaultSetting2;";
        }
        protected override void OnActivated()
        {

            base.OnActivated();
            try
            {
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                ObjectSpace.Committed += ObjectSpace_Committed;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
                if (showNavigationItemController != null)
                {
                    showNavigationItemController.RecreateNavigationItems();
                }
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
                if (base.View != null && base.View.Id == "DefaultSetting_ListView_2_DefaultSetting2")
                {
                    if (View != null && /*View.CurrentObject == e.Object &&*/ e.PropertyName == "Select")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(DefaultSetting))
                        {
                            DefaultSetting objcrtdefsettings = (DefaultSetting)e.Object;
                            if (objcrtdefsettings.Select != null && objcrtdefsettings != null)
                            {
                                IList<DefaultSetting> lstdefsetting = ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = ?", objcrtdefsettings.NavigationItemNameID));
                                if (lstdefsetting.Count > 0)
                                {
                                    foreach (DefaultSetting objdefset in lstdefsetting.ToList())
                                    {
                                        objdefset.Select = objcrtdefsettings.Select;
                                    }
                                    //ObjectSpace.CommitChanges();
                                }
                            }
                            if (objcrtdefsettings.Select == true && objcrtdefsettings != null && objcrtdefsettings.ModuleName == "Data Review" && objcrtdefsettings.NavigationItemNameID == "Result Validation")
                            {
                                objcrtdefsettings.REValidate = EnumRELevelSetup.Yes;
                                //objcrtdefsettings.REApprove = EnumRELevelSetup.Yes;
                            }
                            else if (objcrtdefsettings.Select == false && objcrtdefsettings != null && objcrtdefsettings.ModuleName == "Data Review" && objcrtdefsettings.NavigationItemNameID == "Result Validation")
                            {
                                //objcrtdefsettings.REApprove = EnumRELevelSetup.No;
                                objcrtdefsettings.REValidate = EnumRELevelSetup.No;
                            }
                            if (objcrtdefsettings.Select == true && objcrtdefsettings != null && objcrtdefsettings.ModuleName == "Data Review" && objcrtdefsettings.NavigationItemNameID == "Result Approval")
                            {
                                objcrtdefsettings.REApprove = EnumRELevelSetup.Yes;
                                //objcrtdefsettings.REValidate = EnumRELevelSetup.Yes;
                            }
                            else if (objcrtdefsettings.Select == false && objcrtdefsettings != null && objcrtdefsettings.ModuleName == "Data Review" && objcrtdefsettings.NavigationItemNameID == "Result Approval")
                            {
                                objcrtdefsettings.REApprove = EnumRELevelSetup.No;
                                //objcrtdefsettings.REValidate = EnumRELevelSetup.Yes;
                            }
                            if (objcrtdefsettings.Select == true && objcrtdefsettings != null && objcrtdefsettings.ModuleName == "Data Review" && objcrtdefsettings.NavigationItemNameID == "RawDataLevel2BatchReview ")
                            {
                                DefaultSetting objdefset = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
                                // objdefset.Verify = EnumRELevelSetup.Yes;
                                objdefset.Review = EnumRELevelSetup.Yes;
                            }
                            if (objcrtdefsettings.Select == false && objcrtdefsettings != null && objcrtdefsettings.ModuleName == "Data Review" && objcrtdefsettings.NavigationItemNameID == "RawDataLevel2BatchReview ")
                            {
                                DefaultSetting objdefset = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
                                //objdefset.Verify = EnumRELevelSetup.Yes;
                                objdefset.Review = EnumRELevelSetup.No;
                            }
                            else if (objcrtdefsettings.Select == true && objcrtdefsettings != null && objcrtdefsettings.ModuleName == "Data Review" && objcrtdefsettings.NavigationItemNameID == "RawDataLevel3BatchReview ")
                            {
                                DefaultSetting objdefset = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
                                objdefset.Verify = EnumRELevelSetup.Yes;
                                //objdefset.Review = EnumRELevelSetup.No;
                            }
                            else if (objcrtdefsettings.Select == false && objcrtdefsettings != null && objcrtdefsettings.ModuleName == "Data Review" && objcrtdefsettings.NavigationItemNameID == "RawDataLevel3BatchReview ")
                            {
                                DefaultSetting objdefset = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
                                objdefset.Verify = EnumRELevelSetup.No;
                                //objdefset.Review = EnumRELevelSetup.No;
                            }
                        }
                        //ObjectSpace.CommitChanges();
                    }

                    if (View != null && /*View.CurrentObject == e.Object &&*/ e.PropertyName == "SortIndex")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(DefaultSetting))
                        {
                            DefaultSetting objcrtdefsettings = (DefaultSetting)e.Object;
                            if (objcrtdefsettings.SortIndex < 0)
                            {
                                Application.ShowViewStrategy.ShowMessage("Not allowed negative value", InformationType.Error, 3000, InformationPosition.Top);
                                objcrtdefsettings.SortIndex = 0;
                            }
                        }
                    }
                }
                if (base.View != null && base.View.Id == "DefaultSetting_ListView_2" || base.View.Id == "DefaultSetting_ListView_2_DefaultSetting2")
                {
                    if (View != null && /*View.CurrentObject == e.Object &&*/ e.PropertyName == "Select")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(DefaultSetting))
                        {
                            DefaultSetting objcrtdefsettings = (DefaultSetting)e.Object;
                            if (objcrtdefsettings.Select != null)
                            {
                                IList<DefaultSetting> lstdefsetting = ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = ?", objcrtdefsettings.NavigationItemNameID));
                                if (lstdefsetting.Count > 0)
                                {
                                    foreach (DefaultSetting objdefset in lstdefsetting.ToList())
                                    {
                                        if (objdefset.NavigationItemNameID == "Settings" || objdefset.NavigationItemNameID == "Utility" || objdefset.NavigationItemNameID == "SystemManagement"
                                            || objdefset.NavigationItemNameID == "Maintenance" || objdefset.NavigationItemNameID == "Customer" || objdefset.NavigationItemNameID == "System"
                                            || objdefset.NavigationItemNameID == "Assets_Copy" || objdefset.NavigationItemNameID == "Customer_ListView" || objdefset.NavigationItemNameID == "Contact_ListView"
                                            || objdefset.NavigationItemNameID == "Project" || objdefset.NavigationItemNameID == "Labware_ListView" || objdefset.NavigationItemNameID == "Role"
                                            || objdefset.NavigationItemNameID == "Method" || objdefset.NavigationItemNameID == "TestParameterDefaultSetup" || objdefset.NavigationItemNameID == "Subout Contract Lab"
                                            || objdefset.NavigationItemNameID == "MethodCategory" || objdefset.NavigationItemNameID == "Parameter" || objdefset.NavigationItemNameID == "Test Parameter"
                                            || objdefset.NavigationItemNameID == "State" || objdefset.NavigationItemNameID == "City" || objdefset.NavigationItemNameID == "Country"
                                            || objdefset.NavigationItemNameID == "Matrix" || objdefset.NavigationItemNameID == "VisualMatrix" || objdefset.NavigationItemNameID == "QCType"
                                            || objdefset.NavigationItemNameID == "SampleCategory" || objdefset.NavigationItemNameID == "DeliveryPriority" || objdefset.NavigationItemNameID == "Items"
                                            || objdefset.NavigationItemNameID == "Vendor" || objdefset.NavigationItemNameID == "InstrumentSoftware" || objdefset.NavigationItemNameID == "CheckPointsSetup" || objdefset.NavigationItemNameID == "Vendor")
                                        {
                                            objdefset.Select = true;

                                        }
                                        else
                                        {
                                            if (!objdefset.Select)
                                            {
                                                //objdefset.Select = true;
                                                objdefset.Select = objcrtdefsettings.Select;
                                                //return;
                                            }
                                        }
                                    }
                                    //ObjectSpace.CommitChanges();
                                }
                            }
                        }
                    }
                    if (View != null && /*View.CurrentObject == e.Object &&*/ e.PropertyName == "SortIndex")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(DefaultSetting))
                        {
                            DefaultSetting objcrtdefsettings = (DefaultSetting)e.Object;
                            if (objcrtdefsettings.SortIndex < 0)
                            {
                                Application.ShowViewStrategy.ShowMessage("Not allowed negative value", InformationType.Error, 3000, InformationPosition.Top);
                                objcrtdefsettings.SortIndex = 0;
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

        private void ObjectSpace_Committing(object sender, CancelEventArgs e)
        {
            try
            {
                DefaultSetting objdefsetting = (DefaultSetting)View.CurrentObject;
                if (objdefsetting != null && objdefsetting.NavigationItemNameID != null)
                {
                    IList<DefaultSetting> lstdefsetting = ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = ?", objdefsetting.NavigationItemNameID));
                    if (lstdefsetting.Count > 0)
                    {
                        foreach (DefaultSetting objdefset in lstdefsetting.ToList())
                        {
                            objdefset.Select = objdefsetting.Select;
                        }
                        ObjectSpace.CommitChanges();
                    }
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
                if (View.Id == "DefaultSetting_ListView_2")
                {
                    View.Caption = "Address";
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    DefaultSetting obj = (DefaultSetting)e.InnerArgs.CurrentObject;
                    CollectionSource cs = new CollectionSource(objspace, typeof(DefaultSetting));
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[ModuleName] = ?", obj.ModuleName);
                    //objdis.rgMode = ENMode.Enter.ToString();
                    //objdis.DistributionFilter = "Nothing";
                    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateListView("DefaultSetting_ListView_2_DefaultSetting2", cs, true);
                    e.Handled = true;
                    objDefaultInfo.NavigationName = obj.ModuleName;
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
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            try
            {
                if (base.View != null && base.View.Id == "DefaultSetting_ListView_2")
                {

                    //using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(DefaultSetting)))
                    //{
                    //    //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                    //    lstview.Properties.Add(new ViewProperty("TModuleName", SortDirection.Ascending, "ModuleName", true, true));
                    //    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //    List<object> groups = new List<object>();
                    //    foreach (ViewRecord rec in lstview)
                    //        groups.Add(rec["Toid"]);
                    //    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                    //}

                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null)
                    {
                        ASPxGridView gridView = editor.Grid;
                        gridView.RowUpdating += GridView_RowUpdating;
                        gridView.CancelRowEditing += gv_CancelRowEditing;
                    }
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                if (View.Id == "DefaultSetting_ListView_2_DefaultSetting2")
                {
                    View.Caption = objDefaultInfo.NavigationName;
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null)
                    {
                        ASPxGridView gridView = editor.Grid;
                        gridView.RowUpdating += gv_RowUpdating_NavigationItem;
                        gridView.CancelRowEditing += gv_CancelRowEditing_NavigationItem;

                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void gv_CancelRowEditing_NavigationItem(object sender, ASPxStartRowEditingEventArgs e)
        {
            try
            {
                //if (e.EditingKeyValue != null)
                //{
                //    IObjectSpace objSpace = Application.CreateObjectSpace();
                //    Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                //    DefaultSetting obj = currentSession.GetObjectByKey<DefaultSetting>(e.EditingKeyValue);
                //    if (obj != null && !string.IsNullOrEmpty(obj.NavigationCaption))
                //    {

                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void gv_RowUpdating_NavigationItem(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                //ASPxGridView gridView = (ASPxGridView)sender;
                if (View != null && View.Id == "DefaultSetting_ListView_2_DefaultSetting2")
                {
                    if (e.Keys[0] != null)
                    {
                        DefaultSetting obj = View.ObjectSpace.GetObjectByKey<DefaultSetting>(e.Keys[0]);
                        if (obj != null && !string.IsNullOrEmpty(obj.NavigationCaption))
                        {
                            //Regex rgx = new Regex("^[a-zA-Z0-9 ]*$");
                            //bool containsSpecialCharacter = rgx.IsMatch(obj.NavigationCaption);
                            //if (!containsSpecialCharacter)
                            //{
                            //    e.Cancel = true;
                            //    Application.ShowViewStrategy.ShowMessage("CustomCaption should not have special characters. ", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            //}
                            //else
                            //{
                            e.Cancel = false;
                            //}
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

        private void gv_CancelRowEditing(object sender, ASPxStartRowEditingEventArgs e)
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

        private void GridView_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                //ASPxGridView gridView = (ASPxGridView)sender;
                if (View != null && View.Id == "DefaultSetting_ListView_2")
                {
                    if (e.Keys[0] != null)
                    {
                        DefaultSetting obj = View.ObjectSpace.GetObjectByKey<DefaultSetting>(e.Keys[0]);
                        if (obj != null && !string.IsNullOrEmpty(obj.NavigationCaption))
                        {
                            Regex rgx = new Regex("^[a-zA-Z0-9 ]*$");
                            bool containsSpecialCharacter = rgx.IsMatch(obj.NavigationCaption);
                            if (!containsSpecialCharacter)
                            {
                                e.Cancel = true;
                                Application.ShowViewStrategy.ShowMessage("CustomCaption should not have special characters. ", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                e.Cancel = false;
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
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                ObjectSpace.Committed -= ObjectSpace_Committed;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
    }
}
