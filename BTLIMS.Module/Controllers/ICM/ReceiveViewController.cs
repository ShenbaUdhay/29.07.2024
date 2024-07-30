using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.ICM
{
    public partial class ReceiveViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        ICMinfo objIcmInfo = new ICMinfo();
        Receivequerypanelinfo recobj = new Receivequerypanelinfo();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        ShowNavigationItemController ShowNavigationController;
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        #endregion

        #region Constructor
        public ReceiveViewController()
        {
            InitializeComponent();
            //Distribution_ListView_ReceiveView
            TargetViewId = "Requisition_ListView_Receive;"
                + "Requisition_ListView_Receive_Vendor;"
                + "Requisition_ListView_Receive_POID;"
                + "Receivequerypanel_DetailView;"
                + "Requisition_LookupListView_POID;"
                + "Requisition_ListView_Receive_MainReceive;"
                + "Distribution_ListView_ReceiveView;"
                + "Distribution_ListView_ReceiveViewDirect;";
            ReceiveQuerPanel.TargetViewId = "Requisition_ListView_Receive;" + "Requisition_ListView_Receive_MainReceive;";
            ReceiveModify.TargetViewId = "Distribution_ListView_ReceiveView";
            ReceiveDateFilter.TargetViewId = "Distribution_ListView_ReceiveView;" + "Distribution_ListView_ReceiveViewDirect;";
            Receiveview.TargetViewId = "Requisition_ListView_Receive_MainReceive";
            RollbackReceive.TargetViewId = "Distribution_ListView_ReceiveView";
            POReportPreview.TargetViewId = "Requisition_ListView_Receive_MainReceive";
        }
        #endregion

        #region Default Events
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                Frame.GetController<ReceiveViewController>().Actions["Receivequerypanel"].Active.SetItemValue("", false);
                Frame.GetController<ReceiveViewController>().Actions["ReceiveModify"].Active.SetItemValue("", false);
                //RollbackReceive.Active.SetItemValue("RequisitionViewcontroller.RollbackReceive", false);
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.ReceivingItemsIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.ReceivingItemsIsWrite = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Receive Order" && i.Create == true) != null)
                                {
                                    objPermissionInfo.ReceivingItemsIsWrite = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                if (View.Id == "Distribution_ListView_ReceiveView" || View.Id == "Distribution_ListView_ReceiveViewDirect")
                {
                    RollbackReceive.Active["ShowReceive"] = objPermissionInfo.ReceivingItemsIsWrite;
                    if (ReceiveDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        ReceiveDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        ReceiveDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        ReceiveDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        ReceiveDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        ReceiveDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        ReceiveDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        ReceiveDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //ReceiveDateFilter.SelectedIndex = 1;
                    ReceiveDateFilter.SelectedItemChanged += ReceiveDateFilter_SelectedItemChanged;
                    if (ReceiveDateFilter.SelectedItem == null)
                    {
                        //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 3");
                        DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                        if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 1");
                            ReceiveDateFilter.SelectedIndex = 0;
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                        {
                    ReceiveDateFilter.SelectedIndex = 1;
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 3");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                        {
                            ReceiveDateFilter.SelectedIndex = 2;
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 6");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                        {
                            ReceiveDateFilter.SelectedIndex = 3;
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 1");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                        {
                            ReceiveDateFilter.SelectedIndex = 4;
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 2");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                        {
                            ReceiveDateFilter.SelectedIndex = 5;
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 5");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.All)
                        {
                            ReceiveDateFilter.SelectedIndex = 6;
                            ((ListView)View).CollectionSource.Criteria.Clear();
                        }
                    //    ReceiveDateFilter.SelectedIndex = 0;
                    }
                }
                if (base.View != null && base.View.Id == "Requisition_ListView_Receive_Vendor")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                    {
                        //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                        lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = new InOperator("Oid", groups);
                    }
                }
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
                if (View != null && View.CurrentObject != null && View.Id == "Receivequerypanel_DetailView")
                {
                    Receivequerypanel RPanel = (Receivequerypanel)View.CurrentObject;
                    recobj.rgMode = RPanel.Mode.ToString();
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
            try
            {
                if (View.Id == "Distribution_ListView_ReceiveView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    if (gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {                                                 
                    if(e.focusedColumn.fieldName != 'ReceiveDate'){
                    e.cancel = true;                  
                    }
                    else{
                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') != 'PendingDistribute'){
                    e.cancel = true; 
                    }
                    }
                    }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {                                       
                    window.setTimeout(function() {   
                    if(s.batchEditApi.HasChanges(e.visibleIndex)){
                    s.SelectRowOnPage(e.visibleIndex);
                    }
                    }, 20);}";
                    }
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[RQID] Is Not Null");
                }
                else if (View.Id == "Distribution_ListView_ReceiveViewDirect")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("IsNullOrEmpty([RQID])");
                }
                if (View != null && (View.Id == "Requisition_ListView_Receive_MainReceive") && (string.IsNullOrEmpty(recobj.receivequery) || recobj.receivequery == "Nothing"))
                {
                    if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Requisition))
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                            lstview.Properties.Add(new ViewProperty("TPOID", SortDirection.Ascending, "POID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = new InOperator("Oid", groups);
                        }
                        //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                    }
                }
                if (base.View != null && base.View.Id == "Requisition_LookupListView_POID")
                {
                    if (recobj.rgMode == ENMode.View.ToString())
                    {
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                        {
                            //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'Received' Or [Status] = 'PartiallyReceived' And [Vendor.Vendor] = ? ", recobj.vendorid);
                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'Received' Or [Status] = 'PartiallyReceived'");
                            lstview.Properties.Add(new ViewProperty("TPOID", SortDirection.Ascending, "POID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct2"] = new InOperator("Oid", groups);
                        }
                    }
                    else
                    {
                        if (Application.MainWindow.View.Id == "Purchaseorder_DetailView")
                        {
                            Purchaseorder objPOOrder = (Purchaseorder)Application.MainWindow.View.CurrentObject;
                            if (objPOOrder != null && objPOOrder.Vendor != null && objPOOrder.Vendor.Vendor != null)
                            {
                                recobj.vendorid = objPOOrder.Vendor.Vendor.Vendor;
                            }
                        }
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived' AND [Vendor.Vendor] = ? ", recobj.vendorid);
                            lstview.Properties.Add(new ViewProperty("TPOID", SortDirection.Ascending, "POID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct3"] = new InOperator("Oid", groups);
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
            base.OnDeactivated();
            ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(ObjectSpace_ObjectChanged);
        }


        #endregion

        #region Events
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Vendor")
                {
                    if (View.ObjectTypeInfo.Type == typeof(Receivequerypanel))
                    {
                        Receivequerypanel RPanel = (Receivequerypanel)e.Object;
                        if (RPanel.Vendor != null)
                        {
                            string path;
                            if (RPanel.Vendor.Item.Vendor != null)
                            {
                                path = RPanel.Vendor.Item.Vendor.Vendor;
                            }
                            else
                            {
                                path = RPanel.Vendor.Vendor.Vendor;
                            }

                            if (recobj.receivequery != string.Empty)
                            {
                                recobj.receivequery = recobj.receivequery + "AND [Vendor.Vendor] == '" + path + "'";
                            }
                            else
                            {
                                recobj.vendorid = path;
                                recobj.receivequery = "[Vendor.Vendor] == '" + path + "'";
                            }
                        }
                    }
                }
                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "POID")
                {
                    if (View.ObjectTypeInfo.Type == typeof(Receivequerypanel))
                    {
                        Receivequerypanel Rpanel = (Receivequerypanel)e.Object;
                        if (Rpanel.POID != null)
                        {
                            if (recobj.receivequery != string.Empty)
                            {
                                recobj.receivequery = recobj.receivequery + "AND [POID] == '" + Rpanel.POID.POID + "'";
                            }
                            else
                            {
                                recobj.receivequery = "[POID] == '" + Rpanel.POID.POID + "'";
                            }
                        }
                    }
                }
                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Mode")
                {
                    if (View.ObjectTypeInfo.Type == typeof(Receivequerypanel))
                    {
                        Receivequerypanel Rpanel = (Receivequerypanel)e.Object;
                        if (Rpanel != null)
                        {
                            recobj.rgMode = Rpanel.Mode.ToString();
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
        private void ReceiveQuerPanel_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                WebWindow.CurrentRequestWindow.RegisterClientScript("xml", "sessionStorage.clear();");
                ((ListView)View).CollectionSource.Criteria.Clear();
                int RowCount = 0;
                if (View != null && View.Id == "Requisition_ListView_Receive" || View.Id == "Requisition_ListView_Receive_MainReceive")
                {
                    if (recobj.receivequery == string.Empty)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse("[POID]==NULL");
                    }
                    else if (recobj.receivequery != string.Empty)
                    {
                        if (recobj.rgMode == ENMode.View.ToString())
                        {
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                            cs.Criteria["Filter"] = CriteriaOperator.Parse(recobj.receivequery);
                            //e.ShowViewParameters.CreatedView = Application.CreateListView("Distribution_ListView_ReceiveView", cs, false);
                            Frame.SetView(Application.CreateListView("Distribution_ListView_ReceiveView", cs, false));
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter2"] = CriteriaOperator.Parse(recobj.receivequery);
                            RowCount = ((ListView)View).CollectionSource.GetCount();
                            if (RowCount == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalid"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
        private void ReceiveQuerPanel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                recobj.receivequery = string.Empty;
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(Receivequerypanel));
                if (objToShow != null)
                {
                    DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
                    CreateDetailView.ViewEditMode = ViewEditMode.Edit;
                    e.View = CreateDetailView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ReceiveModify_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                foreach (Distribution obj in View.SelectedObjects)
                {
                    if (obj.Status != Distribution.LTStatus.PendingDistribute)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AlreadyDistributed"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                }
            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Receiveview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                //cs.Criteria["Filter"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 3");
                Frame.SetView(Application.CreateListView("Distribution_ListView_ReceiveView", cs, false));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        private void ReceiveDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //if (e.SelectedChoiceActionItem.Id == "1M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 1");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "3M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 3");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "6M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 6");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "1Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 1");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "2Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 2");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "5Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 5");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "ALL")
            //{
            //    ((ListView)View).CollectionSource.Criteria.Clear();
            //}
        }

        private void ReceiveDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && ReceiveDateFilter != null && ReceiveDateFilter.SelectedItem != null)
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (View.Id == "Distribution_ListView_ReceiveView" || View.Id == "Distribution_ListView_ReceiveViewDirect")
                    {
                        if (strSelectedItem == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 3");
                        }
                        else if (strSelectedItem == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 6");
                        }
                        else if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 2");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReceiveDate, Now()) <= 5");
                        }
                        else if (strSelectedItem == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria.Clear();
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

        private void RollbackReceive_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count > 0)
                {
                    foreach (Distribution obj in View.SelectedObjects)
                    {
                        if (obj.Status != Distribution.LTStatus.PendingDistribute)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "AlreadyDistributed"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    foreach (Distribution obj in View.SelectedObjects)
                    {
                        if (obj.Status == Distribution.LTStatus.PendingDistribute)
                        {
                            IObjectSpace space = Application.CreateObjectSpace();
                            Requisition objdisp = space.FindObject<Requisition>(CriteriaOperator.Parse("[RQID] = ? ", obj.RQID.RQID));
                            if (objdisp != null)
                            {
                                objdisp.itemremaining += 1;
                                objdisp.Item.StockQty = objdisp.Item.StockQty - 1;
                                objdisp.itemreceived = (obj.TotalItems - objdisp.itemremaining) + " of " + obj.TotalItems;
                                if (objdisp.itemremaining == objdisp.TotalItems)
                                {
                                    objdisp.Status = Requisition.TaskStatus.PendingReceived;
                                }
                                else if (objdisp.itemremaining < objdisp.TotalItems)
                                {
                                    objdisp.Status = Requisition.TaskStatus.PartiallyReceived;
                                }
                                ReceiveDistributionHistory newobj = space.CreateObject<ReceiveDistributionHistory>();
                                if (obj.Vendor.Vendor != null)
                                {
                                    newobj.Vendor = space.GetObjectByKey<Vendors>(obj.Vendor.Oid);
                                }
                                if (obj.Item.items != null)
                                {
                                    newobj.Item = space.GetObjectByKey<Items>(obj.Item.Oid);
                                }
                                if (obj.Manufacturer != null)
                                {
                                    newobj.Manufacturer = space.GetObjectByKey<Modules.BusinessObjects.ICM.Manufacturer>(obj.Manufacturer.Oid);
                                }
                                if (obj.OrderQty.ToString() != null)
                                {
                                    newobj.OrderQty = obj.OrderQty;
                                }
                                if (obj.TotalItems.ToString() != null)
                                {
                                    newobj.TotalItems = obj.TotalItems;
                                }
                                newobj.ReturnReason = objIcmInfo.RollBackReason;
                                newobj.itemremaining = obj.itemremaining;
                                newobj.itemreceived = obj.itemreceived;
                                newobj.RequestedBy = space.GetObjectByKey<Employee>(obj.RequestedBy.Oid);
                                newobj.RequestedDate = obj.RequestedDate;
                                newobj.RQID = obj.RQID.RQID;
                                newobj.POID = obj.POID.POID;
                                newobj.ReceiveID = obj.ReceiveID;
                                newobj.ReceiveCount = obj.ReceiveCount;
                                newobj.ReceiveDate = obj.ReceiveDate;
                                newobj.EnteredBy = ((XPObjectSpace)(space)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                newobj.EnteredDate = DateTime.Now;
                                space.CommitChanges();
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                if (objdisp.Item.StockQty <= objdisp.Item.AlertQty)
                                {
                                    ICMAlert objdisp1 = objspace.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + objdisp.Item.items + "(" + objdisp.Item.ItemCode + ")"));
                                    if (objdisp1 == null)
                                    {
                                        ICMAlert obj1 = objspace.CreateObject<ICMAlert>();
                                        obj1.Subject = "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")";
                                        obj1.StartDate = DateTime.Now;
                                        obj1.DueDate = DateTime.Now.AddDays(7);
                                        obj1.RemindIn = TimeSpan.FromMinutes(5);
                                        obj1.Description = "Nice";
                                    }
                                }
                                else
                                {
                                    IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + objdisp.Item.items + "(" + objdisp.Item.ItemCode + ")"));
                                    if (alertlist != null)
                                    {
                                        foreach (ICMAlert item in alertlist)
                                        {
                                            item.AlarmTime = null;
                                            item.RemindIn = null;
                                        }
                                    }
                                }
                                objspace.CommitChanges();
                                ObjectSpace.Delete(obj);
                            }
                        }
                    }
                    ObjectSpace.CommitChanges();
                    NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                    module.ShowNotificationsWindow = false;
                    module.NotificationsService.Refresh();
                    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    {
                        if (parent.Id == "InventoryManagement")
                        {
                            foreach (ChoiceActionItem child in parent.Items)
                            {
                                if (child.Id == "Operations")
                                {
                                    foreach (ChoiceActionItem subchild in child.Items)
                                    {
                                        //if (subchild.Id == "Receive Order")
                                        //{

                                        //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        //    CollectionSource cs = new CollectionSource(objectSpace, typeof(Requisition));
                                        //    cs.Criteria["FilterPOID"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                                        //    List<string> listpoid = new List<string>();
                                        //    foreach (Requisition reqobjvendor in cs.List)
                                        //    {
                                        //        if (!listpoid.Contains(reqobjvendor.POID.POID))
                                        //        {
                                        //            listpoid.Add(reqobjvendor.POID.POID);
                                        //        }
                                        //    }
                                        //    var count = listpoid.Count;
                                        //    //var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'"));
                                        //    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                        //    if (count > 0)
                                        //    {
                                        //        subchild.Caption = cap[0] + " (" + count + ")";
                                        //    }
                                        //    else
                                        //    {
                                        //        subchild.Caption = cap[0];
                                        //    }
                                        //}
                                        if (subchild.Id == "DistributionItem")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[LT] Is Null"));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            int intReceive = 0;
                                            //CriteriaOperator criteria = CriteriaOperator.Parse("[LT] Is Null");
                                            //IList<Distribution> req = ObjectSpace.GetObjects<Distribution>(criteria);
                                            //string[] ReceiveID = new string[req.Count];
                                            //foreach (Distribution item in req)
                                            //{
                                            //    if (!ReceiveID.Contains(item.ReceiveID))
                                            //    {
                                            //        ReceiveID[intReceive] = item.ReceiveID;
                                            //        intReceive = intReceive + 1;
                                            //    }
                                            //}
                                            using (XPView lstview = new XPView(((XPObjectSpace)objectSpace).Session, typeof(Distribution)))
                                            {
                                                lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                                                lstview.Properties.Add(new ViewProperty("TReceiveID", SortDirection.Ascending, "ReceiveID", true, true));
                                                lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                                if (lstview != null && lstview.Count > 0)
                                                {
                                                    intReceive = lstview.Count;
                                                }
                                            }

                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + intReceive + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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

        private void RollbackReceive_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(ICMRollBack));
                if (objToShow != null)
                {
                    DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
                    CreateDetailView.ViewEditMode = ViewEditMode.Edit;
                    e.DialogController.SaveOnAccept = true;
                    e.DialogController.Accepting += DialogController_Accepting;
                    CreateDetailView.Caption = "RollBack";
                    e.View = CreateDetailView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (objIcmInfo.RollBackReason == null)
                {
                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "returnreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    Application.ShowViewStrategy.ShowMessage("Please enter the rollback reason.", InformationType.Info, timer.Seconds, InformationPosition.Top);

                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        #region Function
        private void SetConnectionString()
        {
            try
            {
                string[] connectionstring = objDRDCInfo.WebConfigConn.Split(';');
                objDRDCInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion   

        private void POReportPreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Requisition obj = (Requisition)e.CurrentObject;
                string strPOID = "'" + obj.POID.POID + "'";
                string strTempPath = Path.GetTempPath();
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\PO\")) == false)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\PO\"));
                }
                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\PO\" + timeStamp + ".pdf");
                XtraReport xtraReport = new XtraReport();

                objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SetConnectionString();

                DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                ObjReportingInfo.strPOID = strPOID;
                //xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("ICMPurchaseOrderReport", ObjReportingInfo, false);
                xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("ICMPurchaseOrderReport", ObjReportingInfo, false);
                //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                xtraReport.ExportToPdf(strExportedPath);
                string[] path = strExportedPath.Split('\\');
                int arrcount = path.Count();
                int sc = arrcount - 3;
                string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
