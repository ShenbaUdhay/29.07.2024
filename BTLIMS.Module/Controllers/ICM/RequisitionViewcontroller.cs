using ALPACpre.Module.BusinessObjects;
using BTLIMS.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static ICM.Module.BusinessObjects.Requisition;

namespace LDM.Module.Controllers.ICM
{
    public partial class RequisitionViewcontroller : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        Guid objguid = new Guid();
        List<string> objdelivery = new List<string>();
        //List<Guid> objguid = new List<Guid>();
        ShowNavigationItemController ShowNavigationController;
        requisitionquerypanelinfo objreq = new requisitionquerypanelinfo();
        Receivequerypanelinfo recobj = new Receivequerypanelinfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        ICMinfo Filter = new ICMinfo();
        string AdminFilter;
        ICMRequisition IcmInfo = new ICMRequisition();
        bool IsReqReviewClicked;

        public RequisitionViewcontroller()
        {
            InitializeComponent();
            TargetViewId = "Requisition_ListView_Cancelled;" + "Requisition_ListView;"
                + "Items_LookupListView;"
                + "Requisitionquerypanel_DetailView;"
                + "Requisition_ListView_Specification;"
                + "Requisition_ListView_RequestedBy;"
                + "Requisition_ListView_Item;"
                + "Requisition_ListView_Category;"
                + "Requisition_ListView_Review;"
                + "Requisition_ListView_Approve;"
                + "Requisition_ListView_Receive;"
                + "Requisition_ListView_Receive_MainReceive;"
                + "Requisition_ListViewEntermode;"
                + "Requisition_ListView_DirectReceiveEntermode;"
                + "Vendors_ListView_Retire;"
                + "Distribution_ListView_ReceiveView;"
                + "Distribution_ListView_ReceiveViewDirect;"
                + "Requisition_ListView_ViewMode;"
                + "Items_LookupListView_ReviewCombo;";
            objreq.RequisitionFilter = string.Empty;
            Filter.ApproveFilter = string.Empty;
            AdminFilter = string.Empty;
            Receive.TargetViewId = "Requisition_ListView_Receive;" + "Requisition_ListView_DirectReceiveEntermode;";
            RequisitionQuerPanel.TargetViewId = "Requisition_ListView";
            RequistionNew.TargetViewId = "Requisition_ListView;" + "Distribution_ListView_ReceiveViewDirect;";
            RequisitionSave.TargetViewId = "Requisition_ListViewEntermode";
            //requisitionquerypanelinfo.strFilter = string.Empty;
            RequisitionAddItem.TargetViewId = "Requisition_ListViewEntermode;" + "Requisition_ListView_DirectReceiveEntermode;";
            Requisitiondelete.TargetViewId = "Requisition_ListViewEntermode;" + "Requisition_ListView_DirectReceiveEntermode;";
            Receive.Executing += Receive_Executing;
            Requisitionview.TargetViewId = "Requisition_ListView_Review;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //Requisitionview.Model.Index = 7;
            RequisitionDateFilter.TargetViewId = "Requisition_ListView_ViewMode;";
            //RequisitionDateFilter.Category = "RecordEdit";
            //RequisitionDateFilter.Model.Index = 8;
            //Review.Category = "RecordEdit";
            //Review.Model.Index = 5;

            //RequisitionRollback.TargetViewId = "Requisition_ListView_ViewMode;";
            //  CancelledItemRollback.TargetViewId = "Requisition_ListView_Cancelled;";
            //requisitionquerypanelinfo.strFilter = string.Empty;


            SimpleAction HistoryReqView = new SimpleAction(this, "HistoryReqView", PredefinedCategory.View)
            {
                Caption = "History",
                ImageName = "Action_Search",
            };
            //HistoryReqView.Category = "RecordEdit";
            //HistoryReqView.Model.Index = 6;
            HistoryReqView.TargetViewId = "Requisition_ListViewEntermode;";
            //QuoteView.TargetObjectsCriteria = "Status='" + ContractStatus.Saved + "'";
            HistoryReqView.Execute += QHistoryReqView_Execute;
            HistoryReqView.Category = "Edit";
            HistoryReqView.Id = "HistoryReqView";
        }

        private void QHistoryReqView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(Requisition));
                CollectionSource cs = new CollectionSource(os, typeof(Requisition));
                //cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] <> 'PendingReview' And [Status] <> 'PendingOrdering'");
                Frame.SetView(Application.CreateListView("Requisition_ListView", cs, true));
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
                if (View.Id == "Requisition_ListView" || (View.Id == "Requisition_ListView_Review" && IsReqReviewClicked == false))
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id == "Requisition_ListView" || View.Id == "Requisition_ListView_Review")
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(Requisition));
                    if (objguid != Guid.Empty)
                    {
                        Requisition req = os.FindObject<Requisition>(CriteriaOperator.Parse("[Item.Oid] = ?", objguid));
                        if (req != null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Ordersameitem"), InformationType.Error, timer.Seconds, InformationPosition.Top);

                            e.Cancel = true;
                            ObjectSpace.Refresh();
                            //Exception ex = new Exception("You have requested the same items which are pending for ordering");
                            //throw ex;
                        }
                    }
                    //ObjectSpace.Refresh();
                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);                    
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                IsReqReviewClicked = false;
                if (View.Id == "Requisition_ListView" || View.Id == "Requisition_ListView_Review")
                {
                    ObjectSpace.Committing += ObjectSpace_Committing;
                    ObjectSpace.Committed += ObjectSpace_Committed;
                    RequistionNew.Active["ShowReqNew"] = false;
                    //ObjectSpace.Committed += ObjectSpace_Committed;
                    //ObjectSpace.CustomCommitChanges += ObjectSpace_CustomCommitChanges;
                    //ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    //if (modificationController != null)
                    //{
                    //    modificationController.SaveAction.Executed += SaveAction_Executed;
                    //}
                }
                if (RequisitionDateFilter != null && RequisitionDateFilter.Items.Count > 0)
                {
                    //RequisitionDateFilter.SelectedIndex = 1;
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    RequisitionDateFilter.SelectedItem = null;
                    if (RequisitionDateFilter.SelectedItem == null)
                    {                        
                        //DateTime srDateFilter = DateTime.MinValue;
                        if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                        {
                            //srDateFilter = DateTime.Today.AddMonths(-1);
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                            RequisitionDateFilter.SelectedItem = RequisitionDateFilter.Items[0];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                        {
                            //srDateFilter = DateTime.Today.AddMonths(-3);
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                            RequisitionDateFilter.SelectedItem = RequisitionDateFilter.Items[1];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                        {
                            //srDateFilter = DateTime.Today.AddMonths(-6);
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6");
                            RequisitionDateFilter.SelectedItem = RequisitionDateFilter.Items[2];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                        {
                            //srDateFilter = DateTime.Today.AddYears(-1);
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1");
                            RequisitionDateFilter.SelectedItem = RequisitionDateFilter.Items[3];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                        {
                            //srDateFilter = DateTime.Today.AddYears(-2);
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2");
                            RequisitionDateFilter.SelectedItem = RequisitionDateFilter.Items[4];
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                        {
                            //srDateFilter = DateTime.Today.AddYears(-5);
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5");
                            RequisitionDateFilter.SelectedItem = RequisitionDateFilter.Items[5];
                        }
                        else
                        {
                            //srDateFilter = DateTime.MinValue;
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] = 2");
                            RequisitionDateFilter.SelectedItem = RequisitionDateFilter.Items[6];
                        }
                    }
                    RequisitionDateFilter.SelectedItemChanged += RequisitionDateFilter_SelectedItemChanged;
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                Frame.GetController<RequisitionViewcontroller>().Actions["RequisitionQueryPanel"].Active.SetItemValue("", false);
                //Permisson code
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.RequisitionIsCreate = false;
                        objPermissionInfo.RequisitionIsDelete = false;
                        objPermissionInfo.ReceiveOrderDirectIsWrite = false;
                        objPermissionInfo.ReceiveOrderDirectIsCreate = false;
                        objPermissionInfo.ReceiveOrderDirectIsDelete = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.RequisitionIsWrite = true;
                            objPermissionInfo.RequisitionIsCreate = true;
                            objPermissionInfo.RequisitionIsDelete = true;
                            objPermissionInfo.ReceiveOrderDirectIsWrite = true;
                            objPermissionInfo.ReceiveOrderDirectIsCreate = true;
                            objPermissionInfo.ReceiveOrderDirectIsDelete = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Requisition" && i.Create == true) != null)
                                {
                                    objPermissionInfo.RequisitionIsCreate = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Requisition" && i.Write == true) != null)
                                {
                                    objPermissionInfo.RequisitionIsWrite = true;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Requisition" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.RequisitionIsDelete = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReceiveOrderDirect" && i.Create == true) != null)
                                {
                                    objPermissionInfo.ReceiveOrderDirectIsCreate = true;
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReceiveOrderDirect" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ReceiveOrderDirectIsWrite = true;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ReceiveOrderDirect" && i.Delete == true) != null)
                                {
                                    objPermissionInfo.ReceiveOrderDirectIsDelete = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                if (View.Id == "Requisition_ListView")
                {
                    RequistionNew.Active["ShowRequisitionNew"] = objPermissionInfo.RequisitionIsCreate;
                }
                else
                if (View.Id == "Requisition_ListViewEntermode")
                {
                    RequisitionAddItem.Active["ShowAddItem"] = objPermissionInfo.RequisitionIsCreate;
                    RequisitionSave.Active["ShowRequisitionSave"] = objPermissionInfo.RequisitionIsWrite;
                    Requisitiondelete.Active["ShowRequisitionDelete"] = objPermissionInfo.RequisitionIsDelete;
                    if (objreq.Items == null)
                    {
                        objreq.Items = new List<string>();
                    }
                    if (objreq.fromStockAlert && objreq.SelectedItem != null)
                    {
                        Requisition objReq = ObjectSpace.CreateObject<Requisition>();
                        objReq.Item = ObjectSpace.GetObject<Items>(objreq.SelectedItem);
                        if (objreq.SelectedItem.Vendor != null)
                        {
                            objReq.Vendor = ObjectSpace.GetObjectByKey<Vendors>(objreq.SelectedItem.Vendor.Oid);
                        }
                        if (objreq.SelectedItem.Manufacturer != null)
                        {
                            objReq.Manufacturer = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.ICM.Manufacturer>(objreq.SelectedItem.Manufacturer.Oid);
                        }
                        objReq.UnitPrice = objreq.SelectedItem.UnitPrice;
                        if (objreq.SelectedItem.VendorCatName != null)
                        {
                            objReq.Catalog = objreq.SelectedItem.VendorCatName;
                        }
                        IList<Employee> obj1 = ObjectSpace.GetObjects<Employee>(CriteriaOperator.Parse("Oid = ?", objReq.RequestedBy.Oid));
                        foreach (var rec in obj1)
                        {
                            if (rec.Department != null)
                            {
                                objReq.department = rec.Department;
                            }
                        }
                        objReq.ExpPrice = Math.Round(objReq.OrderQty * objreq.SelectedItem.UnitPrice, 2, MidpointRounding.ToEven);
                        ((ListView)View).CollectionSource.Add(objReq);
                        objreq.fromStockAlert = false;
                        objreq.SelectedItem = null;
                    }
                }
                else
                if (View.Id == "Distribution_ListView_ReceiveViewDirect")
                {
                    RequistionNew.Active["ShowRequisitionNew"] = objPermissionInfo.ReceiveOrderDirectIsCreate;
                }
                else
                if (View.Id == "Requisition_ListView_Review")
                {
                    Review.Active["ShowReview"] = objPermissionInfo.RequisitionReviewIsWrite;
                }
                //Employee currentUser = SecuritySystem.CurrentUser as Employee;
                //if (currentUser != null && View != null && View.Id != null)
                //{
                //    IObjectSpace objSpace = Application.CreateObjectSpace();
                //    CriteriaOperator criteria = CriteriaOperator.Parse("[User]='" + currentUser.Oid + "'");
                //    UserNavigationPermission userpermission = objSpace.FindObject<UserNavigationPermission>(criteria);
                //    CriteriaOperator navcriteria = CriteriaOperator.Parse("[NavigationView]='" + View.Id + "'");
                //    Modules.BusinessObjects.Setting.NavigationItem navigation = objSpace.FindObject<Modules.BusinessObjects.Setting.NavigationItem>(navcriteria);
                //    if (navigation != null && userpermission != null)
                //    {
                //        CriteriaOperator navpermissioncriteria = CriteriaOperator.Parse("[NavigationItem]='" + navigation.Oid + "' and [UserNavigationPermission]='" + userpermission.Oid + "'");
                //        UserNavigationPermissionDetails navPermissionDetails = objSpace.FindObject<UserNavigationPermissionDetails>(navpermissioncriteria);
                //        if (navPermissionDetails != null && View.Id == "Requisition_ListView")
                //        {
                //            if (navPermissionDetails.Create == true)
                //            {
                //                RequistionNew.Active.SetItemValue("RequisitionViewcontroller.RequistionNew", true);
                //            }
                //            else if (navPermissionDetails.Create == false)
                //            {
                //                RequistionNew.Active.SetItemValue("RequisitionViewcontroller.RequistionNew", false);
                //            }
                //        }
                //    }
                //}
                if (base.View != null && base.View.Id == "Requisition_ListView_Specification")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                    {
                        lstview.Properties.Add(new ViewProperty("TSpecification", SortDirection.Ascending, "Item.Specification", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    }
                }
                else if (base.View != null && base.View.Id == "Requisition_ListView_Category")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                    {
                        lstview.Properties.Add(new ViewProperty("TCategory", SortDirection.Ascending, "Item.Category.category", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    }
                }
                else if (base.View != null && base.View.Id == "Requisition_ListView_Item")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                    {
                        lstview.Properties.Add(new ViewProperty("TItem", SortDirection.Ascending, "Item", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    }
                }
                else if (base.View != null && base.View.Id == "Requisition_ListView_RequestedBy")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                    {
                        lstview.Properties.Add(new ViewProperty("TEmployee", SortDirection.Ascending, "RequestedBy", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    }
                }
                else if (View.Id == "Requisition_ListView_Receive_MainReceive")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                else if (base.View != null && base.View.Id == "Items_LookupListView")
                {
                    if (objreq.Items != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new NotOperator(new InOperator("items", objreq.Items));
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("RetireDate IS NULL Or  RetireDate >= ?", DateTime.Now);
                    }
                    //gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                }
                else if (View != null && View.Id == "Vendors_ListView_Retire")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("RetiredDate IS NULL OR RetiredDate >= ?", DateTime.Now);
                }
                else if (View.Id == "Items_LookupListView_ReviewCombo")
                {
                    if (IcmInfo.lstItemsOid.Count > 0)
                    {
                        ((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["filter1"] = new NotOperator(new InOperator("Oid", IcmInfo.lstItemsOid));
                    }
                }
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
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
                if (e.PopupFrame.View.Id == "Items_LookupListView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1200);
                    e.Height = new System.Web.UI.WebControls.Unit(670);
                }
                e.Handled = true;
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
                if (View.Id == "Requisition_ListViewEntermode")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    if (((ListView)View).CollectionSource.List.Count > 0)
                    {
                        RequisitionAddItem.Caption = "Add Items";
                        RequisitionAddItem.ImageName = "Add_16x16";
                        RequisitionAddItem.ToolTip = "Add Items";
                    }
                    else
                    {
                        RequisitionAddItem.Caption = "New";
                        RequisitionAddItem.ImageName = "Action_New";
                        RequisitionAddItem.ToolTip = "New";
                    }
                    List<Requisition> reqcount = ((ListView)View).CollectionSource.List.Cast<Requisition>().ToList();
                    if (reqcount.Count >= 1)
                    {
                        RequisitionSave.Enabled["ShowSaveAction"] = true;
                    }
                    else
                    {
                        RequisitionSave.Enabled["ShowSaveAction"] = false;
                    }
                }
                if (View != null && View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridview = gridListEditor.Grid;
                    if (gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.Load += Grid_Load;
                        gridListEditor.Grid.KeyFieldName = "Oid";
                        if (base.View != null && base.View.Id == "Requisition_ListView_DirectReceiveEntermode")
                        {
                            Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                            gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                            if (user != null)
                            {
                                gridListEditor.Grid.JSProperties["cpusername"] = user.DisplayName;
                            }
                            gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = false;
                            gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) {
                            if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) 
                            {
                                for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                {
                                    s.batchEditApi.SetCellValue(i, 'ReceiveDate', null);
                                    s.batchEditApi.SetCellValue(i, 'ReceivedBy', null);
                                }
                            }
                            else if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0)) 
                            {                       
                                var today = new Date();  
                                for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                {                                        
                                    s.batchEditApi.SetCellValue(i, 'ReceiveDate', today);
                                    s.batchEditApi.SetCellValue(i, 'ReceivedBy', s.cpuserid, s.cpusername, false);
                                }
                            }
                            else
                            {                   
                                if (s.IsRowSelectedOnPage(e.visibleIndex)) 
                                {                      
                                    var today = new Date();                    
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceiveDate', today);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy', s.cpuserid, s.cpusername, false);
                                }
                                else
                                {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceiveDate', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy', null);
                                }
                            }}";
                        }
                        else if (base.View != null && base.View.Id == "Requisition_ListView_Receive")
                        {
                            Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                            gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                            if (user != null)
                            {
                                gridListEditor.Grid.JSProperties["cpusername"] = user.DisplayName;
                            }
                            gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = false;
                            gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) {
                            window.setTimeout(function() {  
                            if(e.visibleIndex != -1) 
                            {
                                var itemrem = s.batchEditApi.GetCellValue(e.visibleIndex, 'itemremaining');
                                var itemrecd = s.batchEditApi.GetCellValue(e.visibleIndex, 'itemreceived');
                                var itemrecv = s.batchEditApi.GetCellValue(e.visibleIndex, 'itemreceiving');
                                var odrqty = s.batchEditApi.GetCellValue(e.visibleIndex, 'TotalItems'); 
                                if (sessionStorage.getItem('itemrem' + e.visibleIndex) == null && sessionStorage.getItem('itemrecd' + e.visibleIndex) == null)
                                {                                                           
                                    var temprecd = itemrecd.substring(0, itemrecd.indexOf('of') - 1);
                                    sessionStorage.setItem('itemrem' + e.visibleIndex, itemrem);                      
                                    sessionStorage.setItem('itemrecd' + e.visibleIndex, temprecd);
                                    sessionStorage.setItem('itemrecdfull' + e.visibleIndex, itemrecd);                                           
                                }
                                var selectsesitemrem = sessionStorage.getItem('itemrem' + e.visibleIndex);
                                var selectsesitemrecd = sessionStorage.getItem('itemrecd' + e.visibleIndex);
                                var selectsesitemrecdfull = sessionStorage.getItem('itemrecdfull' + e.visibleIndex);   
                                var sesselecttype =   sessionStorage.getItem('selecttype' + e.visibleIndex);                          
                                var today = new Date();                                     
                                if (s.IsRowSelectedOnPage(e.visibleIndex)) 
                                {  
                                    if(sesselecttype != 'column')
                                    {                                  
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceiving', selectsesitemrem);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'itemremaining', 0);
                                        var selectitemrecvdtotal = odrqty + ' of ' + odrqty;
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceived', selectitemrecvdtotal);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceiveDate', today);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy', s.cpuserid, s.cpusername, false);
                                        sessionStorage.setItem('selecttype' + e.visibleIndex, 'row');                                                  
                                    }
                                }
                                else
                                {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceiving', 0);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemremaining', selectsesitemrem);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceived', selectsesitemrecdfull);  
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceiveDate', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy', null);
                                    sessionStorage.setItem('selecttype' + e.visibleIndex, '');                                                  
                                }
                            }
                            else if(e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0))
                            {
                                for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                {
                                    var selectsesitemrem = sessionStorage.getItem('itemrem' + i);
                                    var selectsesitemrecd = sessionStorage.getItem('itemrecd' + i);
                                    var selectsesitemrecdfull = sessionStorage.getItem('itemrecdfull' + i); 
                                    if(selectsesitemrem != null)
                                    {
                                        s.batchEditApi.SetCellValue(i, 'ReceiveDate', null);
                                        s.batchEditApi.SetCellValue(i, 'ReceivedBy', null);
                                        s.batchEditApi.SetCellValue(i, 'itemreceiving', 0);
                                        s.batchEditApi.SetCellValue(i, 'itemremaining', selectsesitemrem);
                                        s.batchEditApi.SetCellValue(i, 'itemreceived', selectsesitemrecdfull);  
                                    }
                                }   
                            }
                            else if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0))
                            {
                                for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                {
                                    var itemrem = s.batchEditApi.GetCellValue(i, 'itemremaining');
                                    var itemrecd = s.batchEditApi.GetCellValue(i, 'itemreceived');
                                    var itemrecv = s.batchEditApi.GetCellValue(i, 'itemreceiving');
                                    var odrqty = s.batchEditApi.GetCellValue(i, 'TotalItems'); 
                                    if (sessionStorage.getItem('itemrem' + i) == null && sessionStorage.getItem('itemrecd' + i) == null)
                                    {                                                           
                                        var temprecd = itemrecd.substring(0, itemrecd.indexOf('of') - 1);
                                        sessionStorage.setItem('itemrem' + i, itemrem);                      
                                        sessionStorage.setItem('itemrecd' + i, temprecd);
                                        sessionStorage.setItem('itemrecdfull' + i, itemrecd);                                           
                                    }
                                    var selectsesitemrem = sessionStorage.getItem('itemrem' + i);
                                    var selectsesitemrecd = sessionStorage.getItem('itemrecd' + i);
                                    var selectsesitemrecdfull = sessionStorage.getItem('itemrecdfull' + i);
                                    var today = new Date();
                                    s.batchEditApi.SetCellValue(i, 'itemreceiving', selectsesitemrem);
                                    s.batchEditApi.SetCellValue(i, 'itemremaining', 0);
                                    var selectitemrecvdtotal = odrqty + ' of ' + odrqty;
                                    s.batchEditApi.SetCellValue(i, 'itemreceived', selectitemrecvdtotal);
                                    s.batchEditApi.SetCellValue(i, 'ReceiveDate', today);
                                    s.batchEditApi.SetCellValue(i, 'ReceivedBy', s.cpuserid, s.cpusername, false);
                                    sessionStorage.setItem('selecttype' + i, 'row');                                         
                                }
                            }
                            }, 20); }";

                            gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() {                     
                            var itemrem = s.batchEditApi.GetCellValue(e.visibleIndex, 'itemremaining');
                            var itemrecd = s.batchEditApi.GetCellValue(e.visibleIndex, 'itemreceived');
                            var itemrecv = s.batchEditApi.GetCellValue(e.visibleIndex, 'itemreceiving');
                            var odrqty = s.batchEditApi.GetCellValue(e.visibleIndex, 'TotalItems');                   
                            if (sessionStorage.getItem('itemrem' + e.visibleIndex) == null && sessionStorage.getItem('itemrecd' + e.visibleIndex) == null)
                            {                                        
                                var temprecd = itemrecd.substring(0, itemrecd.indexOf('of') - 1);
                                sessionStorage.setItem('itemrem' + e.visibleIndex, itemrem);                      
                                sessionStorage.setItem('itemrecd' + e.visibleIndex, temprecd);
                                sessionStorage.setItem('itemrecdfull' + e.visibleIndex, itemrecd);                      
                            }
                            var sesitemrem = sessionStorage.getItem('itemrem' + e.visibleIndex);
                            var sesitemrecd = sessionStorage.getItem('itemrecd' + e.visibleIndex);
                            var sesitemrecdfull = sessionStorage.getItem('itemrecdfull' + e.visibleIndex);                                                   
                            var today = new Date();    
                            if(itemrecv >= 1 && itemrem >= 0)
                            {
                                if(itemrecv <= odrqty && itemrecv <= sesitemrem)
                                {
                                    var itemrecvcal = itemrecv + parseInt(sesitemrecd,10);
                                    if(itemrecvcal <= odrqty) 
                                    {
                                        var itemrecvdtot = itemrecvcal + ' of ' + odrqty;
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceived', itemrecvdtot);
                                    }
                                    var sam = sesitemrem - itemrecv;
                                    if(sam >= 0 && sam <= sesitemrem)
                                    {
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'itemremaining', sam);
                                    }
                                    else
                                    {
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'itemremaining', 0);
                                    }
                                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'column');  
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceiveDate', today);
                                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ReceivedBy') == null)
                                    {
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy', s.cpuserid, s.cpusername, false);
                                    }
                                    s.SelectRowOnPage(e.visibleIndex);
                                }
                                else
                                {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceiving', sesitemrem);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemremaining', 0);
                                    var itemrecvdtotal = odrqty + ' of ' + odrqty;
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceived', itemrecvdtotal);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceiveDate', today);
                                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'ReceivedBy') == null)
                                    {
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy', s.cpuserid, s.cpusername, false);
                                    }
                                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'column');                                                  
                                    s.SelectRowOnPage(e.visibleIndex);
                                }
                            }
                            else
                            {
                                if(itemrecv <= 0)
                                {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceiving', 0);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemremaining', sesitemrem);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'itemreceived', sesitemrecdfull);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceiveDate', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy', null);
                                    sessionStorage.setItem('selecttype' + e.visibleIndex, '');                                                  
                                    s.UnselectRowOnPage(e.visibleIndex);
                                }
                            }                                
                            }, 20); }";
                        }
                        else if (base.View != null && base.View.Id == "Requisition_ListViewEntermode")
                        {
                            string js = @"  
                            if (confirm('Do you want to save the Item?')) {{
                            if (window.ASPxClientGridView) {{
                            var allGirds = ASPx.GetControlCollection().GetControlsByType(ASPxClientGridView);
                            }}
                            var errstat = 0;
                            for (var a in allGirds) {{                  
                            for (var i = 0; i <= allGirds[a].GetVisibleRowsOnPage() - 1; i++) {{
                                allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', null);
                            if (allGirds[a].batchEditApi.GetCellValue(i, 'DeliveryPriority.Oid') == null && allGirds[a].batchEditApi.GetCellValue(i, 'OrderQty') == 0 && allGirds[a].batchEditApi.GetCellValue(i, 'department.Oid') == null) {{
                                allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', 'DeliveryPriority should not be Empty,Department should not be Empty and orderqty is zero');                         
                                errstat = 1;
                            }}
                            else if (allGirds[a].batchEditApi.GetCellValue(i, 'DeliveryPriority.Oid') == null && allGirds[a].batchEditApi.GetCellValue(i, 'OrderQty') <= 0) {{
                                allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', 'DeliveryPriority is Empty and orderqty is not allow negative value');                         
                                errstat = 1;
                            }}
                            else if (allGirds[a].batchEditApi.GetCellValue(i, 'department.Oid') == null && allGirds[a].batchEditApi.GetCellValue(i, 'DeliveryPriority.Oid') == null) {{
                                allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', 'Department should not be Empty and DeliveryPriority should not be Empty');                         
                                errstat = 1;
                            }}
                            else if (allGirds[a].batchEditApi.GetCellValue(i, 'department.Oid') == null) {{
                                allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', 'Department should not be Empty');                         
                                errstat = 1;
                            }}
                            else if (allGirds[a].batchEditApi.GetCellValue(i, 'DeliveryPriority.Oid') == null) {{
                                allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', 'DeliveryPriority should not be Empty');                         
                                errstat = 1;
                            }}
                            else if (allGirds[a].batchEditApi.GetCellValue(i, 'OrderQty') < 0) {{
                                allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', 'orderqty is not allow negative value');                         
                                errstat = 1;
                            }}
                            else if (allGirds[a].batchEditApi.GetCellValue(i, 'OrderQty') == 0) {{
                                allGirds[a].batchEditApi.SetCellValue(i, 'Errorlog', 'orderqty is zero');                         
                                errstat = 1;
                            }}
                            }}                   
                            }}
                            {0}
                            }}
                            ";

                            ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                            holder.CallbackManager.RegisterHandler("sam", this);
                            RequisitionSave.SetClientScript(string.Format(CultureInfo.InvariantCulture, js, holder.CallbackManager.GetScript("sam", "errstat")), false);
                            gridListEditor.Grid.Load += Grid_Load;
                            gridListEditor.Grid.ClientSideEvents.CustomButtonClick = @"function(s ,e){ 
                                s.batchEditApi.ResetChanges(e.visibleIndex);     
                            }";



                            gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e){                            
                            for (var i = 0; i <= s.GetVisibleRowsOnPage() - 1; i++) {

                            if (s.batchEditApi.GetCellValue(i, 'Errorlog') != null) {
                                  var error = s.batchEditApi.GetCellValue(i, 'Errorlog');
                                  console.log(error);
                                 if(error!=null)
                                   {
                                      let text=error.split('and')
                                       console.log(text);
                                     text.forEach(myFunction);
                                      function myFunction(item, index) {
                                         console.log(item);
                                        if(item.trim()=='DeliveryPriority should not be Empty')
                                           {
                                               s.GetRow(i).cells[10].style.backgroundColor='OrangeRed';
                                           }
                                         if(item.trim()=='Department should not be Empty')
                                           {
                                               s.GetRow(i).cells[11].style.backgroundColor='OrangeRed';
                                           }
                                      }

                                    }
                                
                                //s.GetRow(i).style.backgroundColor='OrangeRed'; 
                                  //var errorlogCell = s.GetRow(i).cells[10]; 
                                  //if (errorlogCell.innerText == null || errorlogCell.innerText.length == 0) {
                                  //    errorlogCell.style.backgroundColor='OrangeRed';
                                  //    console.log('length')
                                  //}
                                  //else {
                                  //  errorlogCell.style.backgroundColor = '' errorlogCell.innerText.length == 0);
                                  //  console.log('empty');
                                  //}
                                  //var value= errorlogCell.innerText;
                                  //console.log(value);
                                  //var errorlogCell1 = s.GetRow(i).cells[11]; 
                                  //if (errorlogCell1.innerText != null) {
                                  //    errorlogCell1.style.backgroundColor='OrangeRed';
                                  //}
                                  ////s.GetRow(i).cells[9].style.backgroundColor='OrangeRed';
                                  ////s.GetRow(i).cells[10].style.backgroundColor='OrangeRed';
                                setTimeout(function () {
                                var width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
                                window.scrollTo(width, 0);
                            }, 2);
                            }                           
                            }                          
                            }";


                            //                gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {

                            //                 var FrontColIndex = s.GetColumnByField('DeliveryPriority').index;
                            //                 var frontValue = parseFloat(e.rowValues[FrontColIndex].value);
                            //                 if(frontValue == null){
                            //$('#Grid_DXDataRow' + e.visibleIndex + 'td[fieldName=TotalQty]').addClass('OrangeRed');

                            //                 }



                            //                 }";



                            //    gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e){                            
                            //    for (var i = 0; i <= s.GetVisibleRowsOnPage() - 1; i++) {

                            //    if (s.batchEditApi.GetCellValue(i, 'Errorlog') != null) {

                            //     if (e.rowType ==='data') {

                            //     if (e.cellInfo.dataField === 'DeliveryPriority')
                            //    {
                            //        e.cellElement.style.color = 'OrangeRed';
                            //    }
                            //}

                            //        //s.GetRow(i).style.backgroundColor='OrangeRed'; 
                            //        setTimeout(function () {
                            //        var width = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
                            //        window.scrollTo(width, 0);
                            //    }, 2);
                            //    }


                            //    }                          
                            //    }";

                            gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {  
                            if(e.focusedColumn.fieldName == 'OrderQty' || e.focusedColumn.fieldName == 'Comment' || e.focusedColumn.fieldName == 'UnitPrice' || e.focusedColumn.fieldName == 'ShippingOption.Oid' || e.focusedColumn.fieldName == 'DeliveryPriority.Oid' || e.focusedColumn.fieldName == 'itemreceiving' || e.focusedColumn.fieldName == 'POID' || e.focusedColumn.fieldName == 'department.Oid')
                            {
                            e.cancel = false;
                            sessionStorage.setItem('RequisitionUnitPrice', null);
                            s.GetRowValues(e.visibleIndex,'UnitPrice', function getUnitPrice(value)
                                                                                            {
                                                                                                sessionStorage.setItem('RequisitionUnitPrice', value);
                                                                                            });                
                            }    
                            else{
                            e.cancel = true;
                            }
                            }";
                            gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() {   
                            var ordqty = s.batchEditApi.GetCellValue(e.visibleIndex, 'OrderQty');
                            //var unprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'UnitPrice');                            
                            var unprice = sessionStorage.getItem('RequisitionUnitPrice');
                            //alert(unprice);
                            if(ordqty != null && unprice != null)
                            {
                             if(ordqty <= 0 )
                             {
                              alert('OrderQty must be greater than zero');
                              s.batchEditApi.SetCellValue(e.visibleIndex, 'OrderQty', 1); 
                             }
                             else
                             {
                                var tempprice = ordqty * unprice; 
                                tempprice = Math.round(tempprice * 100) / 100; 
                                s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpPrice', tempprice);
                                //s.SelectRowOnPage(e.visibleIndex);
                             }

                            }
                            }, 20); }";
                        }
                        else if (base.View != null && base.View.Id == "Requisition_ListView_Review")
                        {

                            gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                        window.setTimeout(function() {   
                        var ordqty = s.batchEditApi.GetCellValue(e.visibleIndex, 'OrderQty');
                        var unprice = s.batchEditApi.GetCellValue(e.visibleIndex, 'UnitPrice'); 
                        //var Itmuni = s.batchEditApi.GetCellValue(e.visibleIndex, 'Item.ItemUnit');
                        //var lablt = s.batchEditApi.GetCellValue(e.visibleIndex, 'Item.IsLabLT');   
                        if(ordqty != null && unprice != null){
                            var tempprice = ordqty * unprice;
                            tempprice = Math.round(tempprice * 100) / 100; 
                            s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpPrice', tempprice);   
                            //var temptotal = ordqty * Itmuni;
                            //s.batchEditApi.SetCellValue(e.visibleIndex, 'TotalItems', temptotal);
                            //if(lablt == true){
                            //var temptotal = ordqty * Itmuni;
                            //s.batchEditApi.SetCellValue(e.visibleIndex, 'TotalItems', temptotal);
                            //} else {
                            //s.batchEditApi.SetCellValue(e.visibleIndex, 'TotalItems', ordqty);
                            //}                            
                          }
                         }, 20); }";
                        }
                        else if (base.View != null && base.View.Id == "Requisition_ListView_Approve")
                        {
                            gridListEditor.Grid.ClientSideEvents.Init = "function(s, e) { s.timerHandle = -1; }";

                            gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e){ 
                             clearTimeout(s.timerHandle);
                             if(e.focusedColumn.fieldName != 'Cancel')
                             {
                             e.cancel = true;                  
                             }    
                             }";

                            gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            s.timerHandle = setTimeout(function() {  
                                 if (s.batchEditApi.HasChanges()) {  
                                   s.UpdateEdit();  
                                 } 
                               }, 20);}";
                        }
                        //else if (base.View != null && base.View.Id == "Requisition_ListView_Receive_MainReceive")
                        //{
                        //    gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e) {
                        //    window.setTimeout(function() { s.Refresh(); }, 20); }";
                        //}

                    }
                }

                if (View.Id == "Requisition_ListView_Approve" || View.Id == "Requisition_ListView_Review")
                {
                    FuncCurrentUserIsAdministrative obj = new FuncCurrentUserIsAdministrative();
                    object val = obj.Evaluate();
                    if ((string)val == "0")
                    {
                        object CurrentUser = SecuritySystem.CurrentUserId;
                        IObjectSpace objectspace = Application.CreateObjectSpace(typeof(WorkflowConfig));
                        CriteriaOperator criteria = CriteriaOperator.Parse("GCRecord IS Null");
                        IList<WorkflowConfig> ICMWorkFlow = objectspace.GetObjects<WorkflowConfig>(criteria);
                        foreach (WorkflowConfig item in ICMWorkFlow)
                        {
                            if (item.Level == 1)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {

                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level1Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level1Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 2)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level2Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level2Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 3)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level3Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level3Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 4)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level4Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level2Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 5)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status=" + TaskStatus.Level5Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level5Pending + "'";
                                        }
                                    }
                                }
                            }
                            if (item.Level == 6)
                            {
                                foreach (CustomSystemUser userid in item.User)
                                {
                                    if (userid.Oid == (Guid)CurrentUser && item.ActivationOn == true)
                                    {
                                        if (Filter.ApproveFilter == string.Empty)
                                        {
                                            Filter.ApproveFilter = "Status='" + TaskStatus.Level6Pending + "'";
                                        }
                                        else
                                        {
                                            Filter.ApproveFilter = Filter.ApproveFilter + "|| Status='" + TaskStatus.Level6Pending + "'";
                                        }
                                    }
                                }
                            }

                        }
                    }
                    else if ((string)val == "1")
                    {
                        AdminFilter = "Status='" + TaskStatus.Level1Pending + "'|| Status='" + TaskStatus.Level2Pending + "'|| Status='" + TaskStatus.Level3Pending + "'||Status='" + TaskStatus.Level4Pending + "'||Status='" +
                            TaskStatus.Level5Pending + "'|| Status='" + TaskStatus.Level6Pending + "'";
                    }
                }

                if (View.Id == "Requisition_ListView_Review")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.UseFixedTableLayout = true;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    gridListEditor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    IList<Requisition> lstRequisitions = ObjectSpace.GetObjects<Requisition>(CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                    if (lstRequisitions != null && lstRequisitions.Count > 0)
                    {
                        if (IcmInfo.lstItemsOid == null)
                        {
                            IcmInfo.lstItemsOid = new List<Guid>();
                        }
                        if (IcmInfo.lstItemsOid.Count > 0)
                        {
                            IcmInfo.lstItemsOid.Clear();
                        }
                        IcmInfo.lstItemsOid = lstRequisitions.Where(i => i.Item != null).Select(i => i.Item.Oid).ToList();
                    }
                    if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Requisition))
                    {
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Status] = 'PendingReview'");
                    }
                }
                else if (View.Id == "Requisition_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.UseFixedTableLayout = true;
                    //gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    gridListEditor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    if (!((Employee)SecuritySystem.CurrentUser).IsManager)
                    {
                        if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Requisition))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[RequestedBy] = ?", SecuritySystem.CurrentUserId);
                        }
                    }
                    ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("[Status] <> 'Cancelled'");
                }
                else if (View.Id == "Requisition_ListView_Approve")
                {
                    if (Filter.ApproveFilter != string.Empty)
                    {
                        if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Requisition))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(Filter.ApproveFilter);
                        }
                    }
                    else
                    {
                        if (View is DevExpress.ExpressApp.ListView && View.ObjectTypeInfo.Type == typeof(Requisition))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(AdminFilter);
                        }
                    }
                }
                //else if (View.Id == "Requisition_ListView_Entermode")
                //{
                //    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    ASPxGridView gv = gridListEditor.Grid;
                //    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                //    gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    if (gridListEditor != null && gridListEditor.Grid != null)
                //    {
                //        gridListEditor.Grid.Load += Grid_Load;
                //    }                   
                //}
                //else if (View.Id == "Requisition_ListView_Receive_MainReceive")
                //{
                //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                //    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                //    {
                //        lstview.Properties.Add(new ViewProperty("TSpecification", SortDirection.Ascending, "Item.Specification", true, true));
                //        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                //        List<object> groups = new List<object>();
                //        foreach (ViewRecord rec in lstview)
                //            groups.Add(rec["Toid"]);
                //        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        void IXafCallbackHandler.ProcessAction(string parameter)
        {
            try
            {
                if (parameter == "0")
                {
                    //IList<Requisition> reqlist = ObjectSpace.GetObjects<Requisition>(CriteriaOperator.Parse("IsNullOrEmpty([RQID]) and IsNullOrEmpty([ReceiveID])"), true);
                    List<Requisition> reqlist = ((ListView)View).CollectionSource.List.Cast<Requisition>().ToList();
                    if (reqlist.Count >= 1)
                    {
                        int existstatus = 0;
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        ASPxGridView gv = gridListEditor.Grid;
                        gv.KeyFieldName = "Oid";
                        List<Guid> lstNewReqOids = reqlist.Select(i => i.Oid).ToList();
                        foreach (Requisition objreqsave in reqlist)
                        {
                            IObjectSpace space = Application.CreateObjectSpace(typeof(Requisition));
                            Requisition objchkreq = space.FindObject<Requisition>(CriteriaOperator.Parse("([Status] = 'PendingReview' Or [Status] = 'PendingApproval' Or [Status] = 'PendingOrdering' Or [Status] = 'Level1Pending' Or [Status] = 'Level2Pending' Or [Status] = 'Level3Pending' Or [Status] = 'Level4Pending' Or [Status] = 'Level5Pending' Or [Status] = 'Level6Pending') AND [Item.Oid] = ? AND [RequestedBy.Oid] = ?", objreqsave.Item.Oid, objreqsave.RequestedBy.Oid));
                            //GroupOperator criteria = new GroupOperator(GroupOperatorType.And,CriteriaOperator.Parse("([Status] = 'PendingReview' Or [Status] = 'PendingApproval' Or [Status] = 'PendingOrdering' Or [Status] = 'Level1Pending' Or [Status] = 'Level2Pending' Or [Status] = 'Level3Pending' Or [Status] = 'Level4Pending' Or [Status] = 'Level5Pending' Or [Status] = 'Level6Pending') AND [Item.Oid] = ? AND [RequestedBy.Oid] = ?", objreqsave.Item.Oid, objreqsave.RequestedBy.Oid),
                            //    new NotOperator(new InOperator("Oid", lstNewReqOids)));
                            //Requisition objchkreq = space.FindObject<Requisition>(criteria);
                            if (objchkreq != null)
                            {
                                existstatus = 1;
                                gv.Selection.SelectRowByKey(objreqsave.Oid);
                                gv.Styles.SelectedRow.BackColor = System.Drawing.Color.OrangeRed;
                            }
                        }
                        if (existstatus == 1)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Ordersameitem"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        gv.UpdateEdit();
                        foreach (Requisition objreqsave in reqlist)
                        {
                            IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Distribution));
                            CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(RQID, 2))");
                            string temprc = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
                            var curdate = DateTime.Now.ToString("yyMMdd");
                            if (temprc != "1")
                            {
                                var predate = temprc.Substring(0, 6);
                                if (predate == curdate)
                                {
                                    temprc = "RQ" + temprc;
                                }
                                else
                                {
                                    temprc = "RQ" + curdate + "01";
                                }
                            }
                            else
                            {
                                temprc = "RQ" + curdate + "01";
                            }
                            objreqsave.RQID = temprc;
                            objreqsave.TotalItems = objreqsave.OrderQty * objreqsave.Item.ItemUnit;
                            //if (objreqsave.Item.IsLabLT == true)
                            //{
                            //    objreqsave.TotalItems = objreqsave.OrderQty * objreqsave.Item.ItemUnit;
                            //}
                            //else
                            //{
                            //    objreqsave.TotalItems = objreqsave.OrderQty;
                            //}
                            ObjectSpace.CommitChanges();
                            ((ListView)View).CollectionSource.Remove(objreqsave);
                            //((ListView)View).CollectionSource.Remove(objreqsave);
                            //objreq.Items.Remove(objreqsave.Item.items);
                        }
                        objreq.Items.Clear();
                        //IObjectSpace objspace = Application.CreateObjectSpace();
                        //CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
                        //ListView CreatedView = Application.CreateListView("Requisition_ListView", cs, true);
                        //Frame.SetView(CreatedView);
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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
                                            if (subchild.Id == "Review")
                                            {
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                                                var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                if (count > 0)
                                                {
                                                    subchild.Caption = cap[0] + " (" + count + ")";
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
                    }
                }
                else
                {
                    //WebWindow.CurrentRequestWindow.RegisterClientScript("sam", "alert(Grid.GetHorizontalScrollPosition())");
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "shippingnull"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
                if (View.Id == "Requisition_ListView_Receive_MainReceive")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    Requisition obj = (Requisition)e.InnerArgs.CurrentObject;
                    CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("([Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived') AND [POID] = ?", obj.POID.Oid);
                    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateListView("Requisition_ListView_Receive", cs, false);
                    e.Handled = true;
                    recobj.receivequery = "Nothing";
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
            try
            {
                //ObjectSpace.CustomCommitChanges -= ObjectSpace_CustomCommitChanges;
                ObjectSpace.Committing -= ObjectSpace_Committing;
                ObjectSpace.Committed -= ObjectSpace_Committed;
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                base.OnDeactivated();
                if (View.Id == "Requisition_ListView_Approve")
                {
                    CriteriaOperator criteria = CriteriaOperator.Parse("[Status] <> 'Cancelled' And [Cancel] = True");
                    IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                    foreach (Requisition item in req)
                    {
                        item.Cancel = false;
                        item.ApprovedBy = null;
                        item.ApprovedDate = null;
                    }
                    ObjectSpace.CommitChanges();
                }
                if (View.Id == "Requisition_ListViewEntermode" || View.Id == "Requisition_ListView_DirectReceiveEntermode")
                {
                    IList<Requisition> objRequisition = ObjectSpace.GetObjects<Requisition>(CriteriaOperator.Parse("IsNullOrEmpty([RQID]) And IsNullOrEmpty([ReceiveID]) And [RequestedBy] = ?", SecuritySystem.CurrentUserId));
                    if (objRequisition != null)
                    {
                        foreach (Requisition obj in objRequisition.ToList())
                        {
                            ObjectSpace.Delete(obj);
                            ObjectSpace.CommitChanges();
                        }
                    }
                }

                if ((base.View.Id == "Requisition_ListView" || View.Id == "Requisition_ListView_Review") && Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.Contains("DisableUnsavedChangesNotificationController"))
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
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
                IObjectSpace os = this.ObjectSpace;
                Session CS = ((XPObjectSpace)(os)).Session;
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                var selected = gridListEditor.GetSelectedObjects();
                if (View.Id == "Requisition_ListView_Review")
                {
                    foreach (Requisition obj in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(obj))
                        {
                            obj.ReviewedDate = DateTime.Now;
                            obj.ReviewedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                            //if (obj.Vendor == null || obj.OrderQty < 1)
                            //{
                            //    Application.ShowViewStrategy.ShowMessage("Vendor And Order Quantity must not be empty", InformationType.Warning, 1500, InformationPosition.Top);
                            //}
                        }
                        else
                        {
                            obj.ReviewedDate = null;
                            obj.ReviewedBy = null;
                        }
                    }
                }
                else if (View.Id == "Requisition_ListView_Approve")
                {
                    foreach (Requisition obj in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(obj))
                        {
                            obj.ApprovedDate = DateTime.Now;
                            obj.ApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            obj.ApprovedBy = null;
                            obj.ApprovedDate = null;
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
        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                //if (View.Id == "Requisition_ListView_Entermode") {
                //    WebWindow.CurrentRequestWindow.RegisterClientScript("scroll", " ");
                //}
                //if (View.Id == "Requisition_ListView_Receive_MainReceive")
                //{
                //    ASPxGridView gridView = sender as ASPxGridView;
                //    var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                //    selectionBoxColumn.Visible = false;
                //}
                //else if (View.Id == "Requisition_ListView_Entermode")
                //{
                //    ASPxGridView gridView = sender as ASPxGridView;
                //    gridView.Selection.SelectAll();
                //}
                //else if(View.Id == "Requisition_ListView_Receive")
                //{
                //    ASPxGridView gridView = sender as ASPxGridView;
                //    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                //    var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                //    selectionBoxColumn.SelectCheckBoxPosition = GridSelectCheckBoxPosition.Left;
                //    selectionBoxColumn.FixedStyle = GridViewColumnFixedStyle.Left;
                //    selectionBoxColumn.VisibleIndex = 0;
                //    selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                //    //gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["Vendor"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["Vendor"].VisibleIndex = 1;
                //    gridView.Columns["POID"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["POID"].VisibleIndex = 2;
                //    gridView.Columns["Item"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["Item"].VisibleIndex = 3;
                //    gridView.Columns["Item.ItemCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["Item.ItemCode"].VisibleIndex = 4;
                //    gridView.Columns["OrderQty"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["OrderQty"].VisibleIndex = 5;
                //    gridView.Columns["TotalItems"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["TotalItems"].VisibleIndex = 6;
                //    gridView.Columns["itemreceiving"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["itemreceiving"].VisibleIndex = 7;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (base.View != null && base.View.Id == "Requisitionquerypanel_DetailView")
                {
                    if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Specification")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisitionquerypanel))
                        {
                            Requisitionquerypanel RQPanel = (Requisitionquerypanel)e.Object;
                            if (RQPanel.Specification != null)
                            {
                                if (objreq.RequisitionFilter != string.Empty)
                                {
                                    objreq.RequisitionFilter = objreq.RequisitionFilter + "AND [Item.Specification] == '" + RQPanel.Specification.Item.Specification + "'";
                                }
                                else
                                {
                                    objreq.RequisitionFilter = "[Item.Specification] == '" + RQPanel.Specification.Item.Specification + "'";
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "ItemName")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisitionquerypanel))
                        {
                            Requisitionquerypanel RQpanel = (Requisitionquerypanel)e.Object;
                            if (RQpanel.ItemName != null)
                            {
                                if (objreq.RequisitionFilter != string.Empty)
                                {
                                    objreq.RequisitionFilter = objreq.RequisitionFilter + "AND [Item.items] == '" + RQpanel.ItemName.Item.items + "'";
                                }
                                else
                                {
                                    objreq.RequisitionFilter = "[Item.items] == '" + RQpanel.ItemName.Item.items + "'";
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "category")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisitionquerypanel))
                        {
                            Requisitionquerypanel RQpanel = (Requisitionquerypanel)e.Object;
                            if (RQpanel.category != null)
                            {
                                if (objreq.RequisitionFilter != string.Empty)
                                {
                                    objreq.RequisitionFilter = objreq.RequisitionFilter + "AND [Item.Category] == '" + RQpanel.category.Item.Category.Oid + "'";
                                }
                                else
                                {
                                    objreq.RequisitionFilter = "[Item.Category] == '" + RQpanel.category.Item.Category.Oid + "'";
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Requisitionnumber")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisitionquerypanel))
                        {
                            Requisitionquerypanel RQpanel = (Requisitionquerypanel)e.Object;
                            if (RQpanel.Requisitionnumber != null)
                            {
                                if (objreq.RequisitionFilter != string.Empty)
                                {
                                    objreq.RequisitionFilter = objreq.RequisitionFilter + "AND[RQID] == '" + RQpanel.Requisitionnumber.RQID + "'";
                                }
                                else
                                {
                                    objreq.RequisitionFilter = "[RQID] == '" + RQpanel.Requisitionnumber.RQID + "'";
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Requestedby")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisitionquerypanel))
                        {
                            Requisitionquerypanel RQpanel = (Requisitionquerypanel)e.Object;
                            if (RQpanel.Requestedby != null)
                            {
                                if (objreq.RequisitionFilter != string.Empty)
                                {
                                    objreq.RequisitionFilter = objreq.RequisitionFilter + "AND[RequestedBy] == '" + RQpanel.Requestedby.RequestedBy.Oid + "'";
                                }
                                else
                                {
                                    objreq.RequisitionFilter = "[RequestedBy] == '" + RQpanel.Requestedby.RequestedBy.Oid + "'";
                                }
                            }
                        }
                    }
                    else if (e.PropertyName == "RequestedDateFrom" || e.PropertyName == "RequestedDateTo")
                    {
                        Requisitionquerypanel RQpanel = (Requisitionquerypanel)e.Object;
                        if (RQpanel.RequestedDateFrom != null && RQpanel.RequestedDateTo != null)
                        {
                            if (objreq.RequisitionFilter != string.Empty)
                            {
                                if (objreq.RequisitionFilter.Contains("[RequestedDate]") == false)
                                {
                                    objreq.RequisitionFilter = objreq.RequisitionFilter + "AND [RequestedDate] BETWEEN ('" + RQpanel.RequestedDateFrom.Value + "','" + RQpanel.RequestedDateTo.Value.AddHours(23.99) + "')";
                                }
                            }
                            else
                            {
                                objreq.RequisitionFilter = "[RequestedDate] BETWEEN ('" + RQpanel.RequestedDateFrom.Value + "','" + RQpanel.RequestedDateTo.Value.AddHours(23.99) + "')";
                            }
                        }
                    }
                    else if (e.PropertyName == "ResetAll")
                    {
                        Requisitionquerypanel RQpanel = (Requisitionquerypanel)e.Object;
                        if (RQpanel.ResetAll == true)
                        {
                            objreq.RequisitionFilter = "Resetall";
                        }
                    }
                }

                if (base.View != null && (base.View.Id == "Requisition_ListView" || View.Id == "Requisition_ListView_Review"))
                {
                    string status = string.Empty;
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    if (View != null && e.PropertyName == "Item")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisition))
                        {
                            Requisition objReq = (Requisition)e.Object;
                            if (objReq.Item != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace(typeof(Requisition));
                                //Requisition objreq = ObjectSpace.FindObject<Requisition>(CriteriaOperator.Parse("[Item.items] = ? And [Item.Specification] = ? And [Item.Vendor.Vendor] = ? ", Req.Item.items, Req.Item.Specification, Req.Item.Vendor.Vendor));
                                Requisition checkreq = os.FindObject<Requisition>(CriteriaOperator.Parse("[Item.Oid] = ? And [Oid] <> ? And ([Status] = 'PendingReview' Or [Status] = 'PendingOrdering')", objReq.Item.Oid, objReq.Oid)); // [Item.] = ? And [Status] = 'PendingReview' Or [Status] = 'PendingOrdering'", objreq.Item.items ));//[Item.items] = ? And [Item.Specification] = ? And [Item.Vendor.Vendor] = ? ", objreq.Item.items, objreq.Item.Specification,objreq.Item.Vendor.Vendor));
                                if (checkreq != null)
                                {
                                    objguid = checkreq.Item.Oid;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Ordersameitem"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                                else
                                {
                                    objguid = Guid.Empty;
                                }
                                //Items objitem = ObjectSpace.FindObject<Items>(CriteriaOperator.Parse("[items] = ? And [Vendor.Vendor] = ? And [Specification] = ?",objreq.Item.items,objreq.Item.Vendor.Vendor,objreq.Item.Specification)); 
                                //if (objitem != null)
                                //{
                                //    Application.ShowViewStrategy.ShowMessage("This Item already added", InformationType.Error, 3000 ,InformationPosition.Top);
                                //    return;
                                //    View.Close();
                                //}

                                //if (objreq.RequisitionFilter != string.Empty)
                                //{
                                //    objreq.RequisitionFilter = objreq.RequisitionFilter + "AND [Item.items] == '" + RQpanel.ItemName.Item.items + "'";
                                //}
                                //else
                                //{
                                //    objreq.RequisitionFilter = "[Item.items] == '" + RQpanel.ItemName.Item.items + "'";
                                //}
                            }
                            else if (objReq.Item == null)
                            {
                                objReq.Item = (Items)e.OldValue;
                                Application.ShowViewStrategy.ShowMessage("Item must not be empty.", InformationType.Error, 3000, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                    else if (View != null && e.PropertyName == "OrderQty")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisition))
                        {
                            Requisition Req = (Requisition)e.Object;
                            if (Req.OrderQty <= 0)
                            {
                                Req.OrderQty = 1;
                                Application.ShowViewStrategy.ShowMessage("Order Qty not allowed negative value", InformationType.Error, 3000, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                    else if (View != null && e.PropertyName == "DeliveryPriority")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisition))
                        {
                            Requisition Req = (Requisition)e.Object;
                            if (Req.DeliveryPriority == null)
                            {
                                Req.DeliveryPriority = (DeliveryPriority)e.OldValue;
                                Application.ShowViewStrategy.ShowMessage("DeliveryPriority must not be empty.", InformationType.Error, 3000, InformationPosition.Top);
                                return;
                            }
                        }
                    }
                }

                if (base.View != null && base.View.Id == "Requisition_ListViewEntermode")
                {
                    if (View != null && (e.PropertyName == "OrderQty" || e.PropertyName == "DeliveryPriority" || e.PropertyName == "department"))
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Requisition))
                        {
                            Requisition Req = (Requisition)e.Object;
                            if (e.PropertyName == "OrderQty")
                            {
                                if (Req.OrderQty <= 0)
                                {
                                    Req.OrderQty = 1;
                                    Application.ShowViewStrategy.ShowMessage("Order Qty  not allowed negative value", InformationType.Error, 3000, InformationPosition.Top);
                                    return;
                                }
                            }
                            if (e.PropertyName == "DeliveryPriority")
                            {
                                if (Req.DeliveryPriority == null)
                                {
                                    Req.DeliveryPriority = (DeliveryPriority)e.OldValue;
                                    Application.ShowViewStrategy.ShowMessage("DeliveryPriority must not be empty.", InformationType.Error, 3000, InformationPosition.Top);
                                    return;
                                }
                            }
                            if (e.PropertyName == "department")
                            {
                                if (Req.department == null)
                                {
                                    Req.department = (Department)e.OldValue;
                                    Application.ShowViewStrategy.ShowMessage("Department must not be empty.", InformationType.Error, 3000, InformationPosition.Top);
                                    return;
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
        private void RequisitionQuerPanel_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                int RowCount = 0;
                if (View != null && View.Id == "Requisition_ListView")
                {
                    if (objreq.RequisitionFilter == string.Empty)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[RQID]==NULL");
                    }
                    else if (objreq.RequisitionFilter != string.Empty)
                    {
                        if (objreq.RequisitionFilter == "Resetall")
                        {
                            ((ListView)View).CollectionSource.Criteria.Clear();
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse(objreq.RequisitionFilter);
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
        private void RequisitionQuerPanel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(Requisitionquerypanel));
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
        private void Review_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count >= 1)
                {
                    if (base.View != null && base.View.Id == "Requisition_ListView_Review")
                    {
                        IsReqReviewClicked = true;
                        string strBatchid = string.Empty;
                        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                        int reviewstatussucess = 0;
                        int reviewstatuserror = 0;
                        string msg = string.Empty;
                        foreach (Requisition obj in View.SelectedObjects)
                        {
                            if (obj.Vendor != null && obj.OrderQty > 0 && obj.UnitPrice > 0)
                            {
                                obj.itemremaining = obj.OrderQty * obj.Item.ItemUnit;
                                obj.itemreceived = "0 of " + (obj.OrderQty * obj.Item.ItemUnit).ToString();
                                obj.TotalItems = obj.OrderQty * obj.Item.ItemUnit;
                                //if (obj.Item.IsLabLT == true)
                                //{
                                //    obj.itemremaining = obj.OrderQty * obj.Item.ItemUnit;
                                //    obj.itemreceived = "0 of " + (obj.OrderQty * obj.Item.ItemUnit).ToString();
                                //}
                                //else
                                //{
                                //    obj.itemremaining = obj.OrderQty;
                                //    obj.itemreceived = "0 of " + obj.OrderQty.ToString();
                                //}
                                reviewstatussucess = 1;
                                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Requisition));
                                CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(BatchID, 2))");
                                string temprc = (Convert.ToInt32(((XPObjectSpace)objectSpace).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
                                var curdate = DateTime.Now.ToString("yyMMdd");
                                if (temprc != "1")
                                {
                                    var predate = temprc.Substring(0, 6);
                                    if (predate == curdate)
                                    {
                                        temprc = "RR" + temprc;
                                    }
                                    else
                                    {
                                        temprc = "RR" + curdate + "01";
                                    }
                                }
                                else
                                {
                                    temprc = "RR" + curdate + "01";
                                }
                                obj.BatchID = temprc;
                                strBatchid = temprc;
                                CriteriaOperator criteriaLevel = CriteriaOperator.Parse("GCRecord IS Null and ActivationOn = True");
                                List<SortProperty> sorting = new List<SortProperty>();
                                IObjectSpace objectspace = Application.CreateObjectSpace(typeof(DefaultSetting));
                                DefaultSetting objSetting = objectspace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID = 'RequisitionApproval'"));
                                if (objSetting != null && objSetting.Select == true)
                                {
                                    obj.Level = 1;
                                    obj.Status = TaskStatus.Level1Pending;
                                }
                                else
                                {
                                    obj.Status = TaskStatus.PendingOrdering;
                                }
                            }
                            //IObjectSpace objectspace = Application.CreateObjectSpace(typeof(WorkflowConfig));
                            //    sorting.Add(new SortProperty("Level", DevExpress.Xpo.DB.SortingDirection.Ascending));
                            //    CollectionSource objcollectionLevel = new CollectionSource(objectspace, typeof(WorkflowConfig));
                            //    objcollectionLevel.Criteria["flowcriteria"] = criteriaLevel;
                            //    objcollectionLevel.Sorting = sorting;
                            //    if (objcollectionLevel.List.Count > 0)
                            //    {
                            //        foreach (WorkflowConfig ICMWorkFlow in objcollectionLevel.List)
                            //        {
                            //            if (ICMWorkFlow.Level == 1)
                            //            {
                            //                obj.Status = TaskStatus.Level1Pending;
                            //            }
                            //            else if (ICMWorkFlow.Level == 2)
                            //            {
                            //                obj.Status = TaskStatus.Level2Pending;
                            //            }
                            //            else if (ICMWorkFlow.Level == 3)
                            //            {
                            //                obj.Status = TaskStatus.Level3Pending;
                            //            }
                            //            else if (ICMWorkFlow.Level == 4)
                            //            {
                            //                obj.Status = TaskStatus.Level4Pending;
                            //            }
                            //            else if (ICMWorkFlow.Level == 5)
                            //            {
                            //                obj.Status = TaskStatus.Level5Pending;
                            //            }
                            //            else if (ICMWorkFlow.Level == 6)
                            //            {
                            //                obj.Status = TaskStatus.Level6Pending;
                            //            }
                            //            else
                            //            {
                            //                obj.Status = TaskStatus.PendingOrdering;
                            //            }
                            //            obj.Level = ICMWorkFlow.Level;
                            //            break;
                            //        }
                            //    }
                            //    else
                            //    {
                            //        obj.Status = TaskStatus.PendingOrdering;
                            //    }
                            //}
                            //IObjectSpace objectspace = Application.CreateObjectSpace(typeof(WorkflowConfig));

                            //CriteriaOperator criterialevel1 = CriteriaOperator.Parse("[GCRecord] IS Null and [ActivationOn] =1");

                            //WorkflowConfig ICMWorkFlow = (WorkflowConfig)objectspace.GetObjects(typeof(WorkflowConfig), criterialevel1);
                            //if (ICMWorkFlow != null)
                            //{
                            //    if (ICMWorkFlow.Level == 1)
                            //    {
                            //        obj.Status = TaskStatus.Level1Pending;
                            //    }
                            //    if (ICMWorkFlow.Level == 2)
                            //    {
                            //        obj.Status = TaskStatus.Level2Pending;
                            //    }
                            //    else if (ICMWorkFlow.Level == 3)
                            //    {
                            //        obj.Status = TaskStatus.Level3Pending;
                            //    }
                            //    else if (ICMWorkFlow.Level == 4)
                            //    {
                            //        obj.Status = TaskStatus.Level4Pending;
                            //    }
                            //    else if (ICMWorkFlow.Level == 5)
                            //    {
                            //        obj.Status = TaskStatus.Level5Pending;
                            //    }
                            //    else if (ICMWorkFlow.Level == 6)
                            //    {
                            //        obj.Status = TaskStatus.Level6Pending;
                            //    }
                            //    else
                            //    {
                            //        obj.Status = TaskStatus.PendingOrdering;
                            //    }
                            //    obj.Level = ICMWorkFlow.Level;
                            //}
                            else
                            {
                                obj.ReviewedBy = null;
                                obj.ReviewedDate = null;
                                if (obj.Vendor == null)
                                {
                                    msg = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Vendor");
                                }
                                else if (obj.OrderQty <= 0)
                                {
                                    msg = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "orderquantity");
                                }
                                else if (obj.UnitPrice <= 0)
                                {
                                    // msg = CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "UnitPrice");
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "UnitPrice"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                reviewstatuserror = 1;
                            }
                        }
                        ObjectSpace.CommitChanges();
                        if (reviewstatussucess == 1 && reviewstatuserror == 1)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "partialreview"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        else if (reviewstatuserror == 1)
                        {
                            Application.ShowViewStrategy.ShowMessage(msg + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "notempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        else if (reviewstatussucess == 1)
                        {
                            Application.ShowViewStrategy.ShowMessage(strBatchid + " " + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ReviewSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        View.ObjectSpace.Refresh();
                        View.Refresh();
                        ((ASPxGridListEditor)((ListView)View).Editor).Grid.Selection.UnselectAll();
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
                                            if (subchild.Id == "Review")
                                            {
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReview'"));
                                                var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                if (count > 0)
                                                {
                                                    subchild.Caption = cap[0] + " (" + count + ")";
                                                }
                                                else
                                                {
                                                    subchild.Caption = cap[0];
                                                }
                                            }
                                            else if (subchild.Id == "RequisitionApproval")
                                            {
                                                if (Filter.ApproveFilter != string.Empty)
                                                {

                                                    int intValue = 0;
                                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                    CriteriaOperator criteria = CriteriaOperator.Parse(Filter.ApproveFilter);
                                                    IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                                                    string[] batch = new string[req.Count];
                                                    foreach (Requisition item in req)
                                                    {
                                                        if (!batch.Contains(item.BatchID))
                                                        {
                                                            batch[intValue] = item.BatchID;
                                                            intValue = intValue + 1;
                                                        }
                                                    }
                                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                    if (intValue > 0)
                                                    {
                                                        subchild.Caption = cap[0] + " (" + intValue + ")";
                                                    }
                                                    else
                                                    {
                                                        subchild.Caption = cap[0];
                                                    }
                                                }
                                                else
                                                {
                                                    int intValue = 0;
                                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                    CriteriaOperator criteria = CriteriaOperator.Parse(AdminFilter);
                                                    IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                                                    string[] batch = new string[req.Count];
                                                    foreach (Requisition item in req)
                                                    {
                                                        if (!batch.Contains(item.BatchID))
                                                        {
                                                            batch[intValue] = item.BatchID;
                                                            intValue = intValue + 1;
                                                        }
                                                    }
                                                    var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                    if (intValue > 0)
                                                    {
                                                        subchild.Caption = cap[0] + " (" + intValue + ")";
                                                    }
                                                    else
                                                    {
                                                        subchild.Caption = cap[0];
                                                    }
                                                }
                                            }
                                            else if (subchild.Id == "OrderingItems")
                                            {
                                                int intOrderValue = 0;
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingOrdering'"));
                                                var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                CriteriaOperator criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                                                IList<Requisition> req = objectSpace.GetObjects<Requisition>(criteria);
                                                string[] strVendor = new string[req.Count];
                                                foreach (Requisition item in req)
                                                {
                                                    if (!strVendor.Contains(item.Vendor.Vendor))
                                                    {
                                                        strVendor[intOrderValue] = item.Vendor.Vendor;
                                                        intOrderValue = intOrderValue + 1;
                                                    }
                                                }


                                                if (intOrderValue > 0)
                                                {
                                                    subchild.Caption = cap[0] + " (" + intOrderValue + ")";
                                                }
                                                else
                                                {
                                                    subchild.Caption = cap[0];
                                                }
                                                //Frame.GetController<POQuerypanel>().Actions["rcount"].Caption = "Pending - " + count;
                                                //Frame.GetController<POQuerypanel>().Actions["rcount"].Enabled.SetItemValue("", false);
                                            }
                                            //else if (subchild.Id == "Receive Order")
                                            //{
                                            //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            //    var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'"));
                                            //    subchild.Caption = "Receiving Items " + "(" + count + ")";
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectdatareview"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Review_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                IsReqReviewClicked = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Approve_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count >= 1)
                {
                    if (base.View != null && base.View.Id == "Requisition_ListView_Approve")
                    {
                        IObjectSpace objectspace = Application.CreateObjectSpace(typeof(DefaultSetting));
                        foreach (Requisition obj in View.SelectedObjects)
                        {
                            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                            IObjectSpace os = this.ObjectSpace;
                            Session CS = ((XPObjectSpace)(os)).Session;
                            if (obj.Cancel == true)
                            {
                                obj.ApprovedDate = null;
                                obj.ApprovedBy = null;
                                obj.CanceledDate = DateTime.Now;
                                obj.CanceledBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                IObjectSpace objSpace1 = Application.CreateObjectSpace();
                                LevelApprovalHistory newApprovalHistory1 = ObjectSpace.CreateObject<LevelApprovalHistory>();
                                newApprovalHistory1.BatchID = obj.BatchID;
                                newApprovalHistory1.ApprovedDate = null;
                                newApprovalHistory1.ApprovedBy = null;
                                newApprovalHistory1.Level = obj.Level;
                                newApprovalHistory1.CanceledBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                newApprovalHistory1.CanceledDate = DateTime.Now;
                                obj.Status = TaskStatus.Cancelled;
                            }
                            else
                            {
                                obj.ApprovedDate = DateTime.Now;
                                obj.ApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                LevelApprovalHistory newApprovalHistory = ObjectSpace.CreateObject<LevelApprovalHistory>();
                                newApprovalHistory.BatchID = obj.BatchID;
                                newApprovalHistory.ApprovedDate = obj.ApprovedDate;
                                newApprovalHistory.ApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                newApprovalHistory.Level = obj.Level;
                                CriteriaOperator criterialevel1 = CriteriaOperator.Parse("GCRecord IS Null and Level=" + obj.Level);
                                //WorkflowConfig ICMWorkFlow = (WorkflowConfig)objectspace.FindObject(typeof(WorkflowConfig), criterialevel1);
                                // DefaultSetting ICMWorkFlow = (DefaultSetting)objectspace.FindObject(typeof(DefaultSetting), criterialevel1);
                                //DefaultSetting ICMWorkFlow = (DefaultSetting)objectspace.FindObject<DefaultSetting>(CriteriaOperator.Parse(""));
                                IObjectSpace objectsetting = Application.CreateObjectSpace(typeof(DefaultSetting));
                                DefaultSetting ICMWorkFlow = objectsetting.FindObject<DefaultSetting>(CriteriaOperator.Parse("ModuleName = 'Inventory Management'"));
                                if (ICMWorkFlow != null)
                                {
                                    if (ICMWorkFlow.NoofLevels > obj.Level)
                                    {
                                        if (obj.Level == 1)
                                        {
                                            obj.Status = TaskStatus.Level2Pending;
                                        }
                                        else if (obj.Level == 2)
                                        {
                                            obj.Status = TaskStatus.Level3Pending;
                                        }
                                        else if (obj.Level == 3)
                                        {
                                            obj.Status = TaskStatus.Level4Pending;
                                        }
                                        else if (obj.Level == 4)
                                        {
                                            obj.Status = TaskStatus.Level5Pending;
                                        }
                                        else if (obj.Level == 5)
                                        {
                                            obj.Status = TaskStatus.Level6Pending;
                                        }
                                        else if (obj.Level == 6)
                                        {
                                            obj.Status = TaskStatus.Level7Pending;
                                        }
                                        else if (obj.Level == 7)
                                        {
                                            obj.Status = TaskStatus.Level8Pending;
                                        }
                                        else if (obj.Level == 8)
                                        {
                                            obj.Status = TaskStatus.Level9Pending;
                                        }
                                        else if (obj.Level == 9)
                                        {
                                            obj.Status = TaskStatus.Level10Pending;
                                        }

                                        obj.Level++;
                                    }
                                    else
                                    {
                                        obj.Status = TaskStatus.PendingOrdering;
                                    }
                                }
                                //if (ICMWorkFlow != null)
                                //{
                                //    obj.Level = ICMWorkFlow.NoofLevels;
                                //    if (ICMWorkFlow.NoofLevels == 2)
                                //    {
                                //        obj.Status = TaskStatus.Level2Pending;
                                //    }
                                //    else if (ICMWorkFlow.NoofLevels == 3)
                                //    {
                                //        obj.Status = TaskStatus.Level3Pending;
                                //    }
                                //    else if (ICMWorkFlow.NoofLevels == 4)
                                //    {
                                //        obj.Status = TaskStatus.Level4Pending;
                                //    }
                                //    else if (ICMWorkFlow.NoofLevels == 5)
                                //    {
                                //        obj.Status = TaskStatus.Level5Pending;
                                //    }
                                //    else if (ICMWorkFlow.NoofLevels == 6)
                                //    {
                                //        obj.Status = TaskStatus.Level6Pending;
                                //    }
                                //    else if (ICMWorkFlow.NoofLevels == 7)
                                //    {
                                //        obj.Status = TaskStatus.Level7Pending;
                                //    }
                                //    else if (ICMWorkFlow.NoofLevels == 8)
                                //    {
                                //        obj.Status = TaskStatus.Level8Pending;
                                //    }
                                //    else if (ICMWorkFlow.NoofLevels == 9)
                                //    {
                                //        obj.Status = TaskStatus.Level9Pending;
                                //    }
                                //    else if (ICMWorkFlow.NoofLevels == 10)
                                //    {
                                //        obj.Status = TaskStatus.Level10Pending;
                                //    }
                                //    else
                                //    {
                                //        obj.Status = TaskStatus.PendingOrdering;
                                //    }
                                //}
                                //if (ICMWorkFlow != null)
                                //{
                                //    obj.Level = ICMWorkFlow.NextLevel;
                                //    if (ICMWorkFlow.NextLevel == 2)
                                //    {
                                //        obj.Status = TaskStatus.Level2Pending;
                                //    }
                                //    else if (ICMWorkFlow.NextLevel == 3)
                                //    {
                                //        obj.Status = TaskStatus.Level3Pending;
                                //    }
                                //    else if (ICMWorkFlow.NextLevel == 4)
                                //    {
                                //        obj.Status = TaskStatus.Level4Pending;
                                //    }
                                //    else if (ICMWorkFlow.NextLevel == 5)
                                //    {
                                //        obj.Status = TaskStatus.Level5Pending;
                                //    }
                                //    else if (ICMWorkFlow.NextLevel == 6)
                                //    {
                                //        obj.Status = TaskStatus.Level6Pending;
                                //    }
                                //    else
                                //    {
                                //        obj.Status = TaskStatus.PendingOrdering;
                                //    }
                                //}
                            }
                            ObjectSpace.CommitChanges();
                        }
                    }
                    View.ObjectSpace.Refresh();
                    View.Refresh();
                    //((ASPxGridListEditor)((ListView)View).Editor).Grid.Selection.UnselectAll();
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
                                        if (subchild.Id == "RequisitionApproval")
                                        {
                                            if (Filter.ApproveFilter != string.Empty)
                                            {

                                                int intValue = 0;
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                CriteriaOperator criteria = CriteriaOperator.Parse(Filter.ApproveFilter);
                                                IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                                                string[] batch = new string[req.Count];
                                                foreach (Requisition item in req)
                                                {
                                                    if (!batch.Contains(item.BatchID))
                                                    {
                                                        batch[intValue] = item.BatchID;
                                                        intValue = intValue + 1;
                                                    }
                                                }
                                                var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                if (intValue > 0)
                                                {
                                                    subchild.Caption = cap[0] + " (" + intValue + ")";
                                                }
                                                else
                                                {
                                                    subchild.Caption = cap[0];
                                                }
                                            }
                                            else
                                            {
                                                int intValue = 0;
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                CriteriaOperator criteria = CriteriaOperator.Parse(AdminFilter);
                                                IList<Requisition> req = ObjectSpace.GetObjects<Requisition>(criteria);
                                                string[] batch = new string[req.Count];
                                                foreach (Requisition item in req)
                                                {
                                                    if (!batch.Contains(item.BatchID))
                                                    {
                                                        batch[intValue] = item.BatchID;
                                                        intValue = intValue + 1;
                                                    }
                                                }
                                                var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                                if (intValue > 0)
                                                {
                                                    subchild.Caption = cap[0] + " (" + intValue + ")";
                                                }
                                                else
                                                {
                                                    subchild.Caption = cap[0];
                                                }
                                            }
                                        }
                                        else if (subchild.Id == "OrderingItems")
                                        {
                                            int intOrderValue = 0;
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingOrdering'"));
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            CriteriaOperator criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                                            IList<Requisition> req = objectSpace.GetObjects<Requisition>(criteria);
                                            string[] strVendor = new string[req.Count];
                                            foreach (Requisition item in req)
                                            {
                                                if (!strVendor.Contains(item.Vendor.Vendor))
                                                {
                                                    strVendor[intOrderValue] = item.Vendor.Vendor;
                                                    intOrderValue = intOrderValue + 1;
                                                }
                                            }


                                            if (intOrderValue > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + intOrderValue + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                            //Frame.GetController<POQuerypanel>().Actions["rcount"].Caption = "Pending - " + count;
                                            //Frame.GetController<POQuerypanel>().Actions["rcount"].Enabled.SetItemValue("", false);
                                        }
                                        //else if (subchild.Id == "Receive Order")
                                        //{
                                        //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                        //    var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'"));
                                        //    subchild.Caption = "Receiving Items " + "(" + count + ")";
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "approvesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectdataapprove"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Receive_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Guid POID = new Guid();
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                if (View != null && View.SelectedObjects.Count > 0)
                {
                    foreach (Requisition requisitionobj in View.SelectedObjects)
                    {
                        if (requisitionobj.itemreceiving == 0 || requisitionobj.itemreceiving < 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "itemreceiving"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        if (requisitionobj.ReceiveDate == null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "enterreceivdate"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        if (requisitionobj.ReceivedBy == null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "enterreceiveby"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }

                    IObjectSpace Space = Application.CreateObjectSpace(typeof(Requisition));
                    CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(ReceiveID, 2))");
                    string temprc = (Convert.ToInt32(((XPObjectSpace)Space).Session.Evaluate(typeof(Requisition), sam, null)) + 1).ToString();
                    var curdate = DateTime.Now.ToString("yyMMdd");
                    if (temprc != "1")
                    {
                        var predate = temprc.Substring(0, 6);
                        if (predate == curdate)
                        {
                            temprc = "RC" + temprc;
                        }
                        else
                        {
                            temprc = "RC" + curdate + "01";
                        }
                    }
                    else
                    {
                        temprc = "RC" + curdate + "01";
                    }

                    List<string> NumItemcount = new List<string>();
                    foreach (Requisition requisitionobj in View.SelectedObjects)
                    {
                        if (NumItemcount != null && !NumItemcount.Contains(requisitionobj.Item.items))
                        {
                            NumItemcount.Add(requisitionobj.Item.items);
                        }
                    }

                    foreach (Requisition obj in View.SelectedObjects)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        for (int i = 1; i <= obj.itemreceiving; i++)
                        {
                            Distribution newobj = os.CreateObject<Distribution>();
                            if (obj.Vendor.Vendor != null)
                            {
                                newobj.Vendor = os.GetObjectByKey<Vendors>(obj.Vendor.Oid);
                            }
                            if (obj.Item.items != null)
                            {
                                newobj.Item = os.GetObjectByKey<Items>(obj.Item.Oid);
                            }
                            if (obj.Manufacturer != null)
                            {
                                newobj.Manufacturer = os.GetObjectByKey<Modules.BusinessObjects.ICM.Manufacturer>(obj.Manufacturer.Oid);
                            }
                            if (obj.OrderQty.ToString() != null)
                            {
                                newobj.OrderQty = obj.OrderQty;
                            }
                            if (obj.TotalItems.ToString() != null)
                            {
                                newobj.TotalItems = obj.TotalItems;
                            }
                            newobj.itemreceiving = obj.itemreceiving;
                            if (View.Id == "Requisition_ListView_Receive")
                            {
                                newobj.itemremaining = (obj.itemremaining + obj.itemreceiving) - i;
                                newobj.itemreceived = (obj.TotalItems - newobj.itemremaining) + " of " + obj.TotalItems;
                                newobj.RQID = os.GetObjectByKey<Requisition>(obj.Oid);
                                newobj.POID = os.GetObjectByKey<Purchaseorder>(obj.POID.Oid);
                            }
                            newobj.RequisitionID = os.GetObjectByKey<Requisition>(obj.Oid);
                            newobj.RequestedBy = os.GetObjectByKey<Employee>(obj.RequestedBy.Oid);
                            newobj.RequestedDate = obj.RequestedDate;
                            newobj.ReceiveID = temprc;
                            newobj.ReceiveCount = temprc + "-" + i;
                            newobj.ReceivedBy = os.GetObjectByKey<Employee>(obj.ReceivedBy.Oid);
                            newobj.ReceiveDate = obj.ReceiveDate;
                            newobj.NumItemCode = NumItemcount.Count;
                            newobj.Status = Distribution.LTStatus.PendingDistribute;
                            obj.ReceiveID = temprc;
                        }
                        os.CommitChanges();

                        IObjectSpace itemobjspace = Application.CreateObjectSpace();
                        //Items itemobj = itemobjspace.FindObject<Items>(CriteriaOperator.Parse("[ItemCode] = ?", obj.Item.ItemCode));
                        Items itemobj = itemobjspace.GetObjectByKey<Items>(obj.Item.Oid);
                        if (itemobj != null)
                        {
                            itemobj.StockQty += obj.itemreceiving;
                            if (itemobj.StockQty <= itemobj.AlertQty)
                            {
                                ICMAlert objdisp1 = itemobjspace.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")"));
                                if (objdisp1 == null)
                                {
                                    ICMAlert obj1 = itemobjspace.CreateObject<ICMAlert>();
                                    obj1.Subject = "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")";
                                    obj1.StartDate = DateTime.Now;
                                    obj1.DueDate = DateTime.Now.AddDays(7);
                                    obj1.RemindIn = TimeSpan.FromMinutes(5);
                                    obj1.Description = "Nice";
                                }
                            }
                            else
                            {
                                IList<ICMAlert> alertlist = itemobjspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")"));
                                if (alertlist != null)
                                {
                                    foreach (ICMAlert item in alertlist)
                                    {
                                        item.AlarmTime = null;
                                        item.RemindIn = null;
                                    }
                                }
                            }
                            itemobjspace.CommitChanges();
                        }
                        if (obj.itemremaining > 0)
                        {
                            obj.Status = Requisition.TaskStatus.PartiallyReceived;
                            POID = obj.POID.Oid;
                        }
                        else
                        {
                            obj.Status = Requisition.TaskStatus.Received;
                        }
                    }
                    ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();
                    //((ListView)View).CollectionSource.Criteria.Clear();
                    if (POID != new Guid())
                    {
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("([Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived') And [POID] ='" + POID + "'");
                    }
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
                                        if (subchild.Id == "Receive Order")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            //CollectionSource cs = new CollectionSource(objectSpace, typeof(Requisition));
                                            //cs.Criteria["FilterPOID"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                                            //List<string> listpoid = new List<string>();
                                            //foreach (Requisition reqobjvendor in cs.List)
                                            //{
                                            //    if (!listpoid.Contains(reqobjvendor.POID.POID))
                                            //    {
                                            //        listpoid.Add(reqobjvendor.POID.POID);
                                            //    }
                                            //}
                                            //var count = listpoid.Count;
                                            var count = 0;
                                            using (XPView lstview = new XPView(((XPObjectSpace)objectSpace).Session, typeof(Requisition)))
                                            {
                                                lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                                                lstview.Properties.Add(new ViewProperty("TPOID", SortDirection.Ascending, "POID", true, true));
                                                lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                                if (lstview != null && lstview.Count > 0)
                                                {
                                                    count = lstview.Count;
                                                }
                                            }
                                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                            if (count > 0)
                                            {
                                                subchild.Caption = cap[0] + " (" + count + ")";
                                            }
                                            else
                                            {
                                                subchild.Caption = cap[0];
                                            }
                                        }
                                        else if (subchild.Id == "DistributionItem")
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
                    WebWindow.CurrentRequestWindow.RegisterClientScript("xml", "sessionStorage.clear();");
                    //((ASPxGridListEditor)((ListView)View).Editor).Grid.Selection.UnselectAll();
                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "receivesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    Application.ShowViewStrategy.ShowMessage(temprc + " received successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "dataselect"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            //ObjectSpace.Refresh();
        }

        private void RequistionNew_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (e.PopupWindowViewSelectedObjects.Count >= 1)
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
                    ListView CreatedView;
                    if (View.Id == "Requisition_ListView")
                    {
                        CreatedView = Application.CreateListView("Requisition_ListViewEntermode", cs, true);
                    }
                    else
                    {
                        CreatedView = Application.CreateListView("Requisition_ListView_DirectReceiveEntermode", cs, true);
                    }
                    Frame.SetView(CreatedView);
                    List<Guid> lstRequisition = new List<Guid>();
                    //cs.Criteria["filter"] = CriteriaOperator.Parse("IsNullOrEmpty([RQID]) and IsNullOrEmpty([ReceiveID])");
                    cs.Criteria["filter"] = new InOperator("Oid", lstRequisition);
                    if (objreq.Items == null)
                    {
                        objreq.Items = new List<string>();
                    }
                    foreach (Items obj in e.PopupWindowViewSelectedObjects)
                    {
                        objreq.Items.Add(obj.items);
                        Requisition objnewreq = ((ListView)View).ObjectSpace.CreateObject<Requisition>();
                        objnewreq.Item = ObjectSpace.GetObject<Items>(obj);
                        if (obj.Vendor != null)
                        {
                            objnewreq.Vendor = ObjectSpace.GetObjectByKey<Vendors>(obj.Vendor.Oid);
                        }
                        if (obj.Manufacturer != null)
                        {
                            objnewreq.Manufacturer = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.ICM.Manufacturer>(obj.Manufacturer.Oid);
                        }
                        objnewreq.UnitPrice = obj.UnitPrice;
                        if (obj.VendorCatName != null)
                        {
                            objnewreq.Catalog = obj.VendorCatName;
                        }
                        IList<Employee> obj1 = ObjectSpace.GetObjects<Employee>(CriteriaOperator.Parse("Oid = ?", objnewreq.RequestedBy.Oid));
                        foreach (var rec in obj1)
                        {
                            if (rec.Department != null)
                            {
                                objnewreq.department = rec.Department;
                            }
                        }
                        objnewreq.ExpPrice = Math.Round(objnewreq.OrderQty * obj.UnitPrice, 2, MidpointRounding.ToEven);
                        ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(objnewreq));
                    }
                }
                else
                {
                    e.CanCloseWindow = false;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "dataselect"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RequistionNew_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Items));
                ListView CreateListView = Application.CreateListView("Items_LookupListView", cs, false);
                e.View = CreateListView;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void RequisitionAddItem_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (e.PopupWindowViewSelectedObjects.Count >= 1)
                {
                    foreach (Items obj in e.PopupWindowViewSelectedObjects)
                    {
                        objreq.Items.Add(obj.items);
                        Requisition objnewreq = ObjectSpace.CreateObject<Requisition>();
                        objnewreq.Item = ObjectSpace.GetObject<Items>(obj);
                        if (obj.Vendor != null)
                        {
                            objnewreq.Vendor = ObjectSpace.GetObjectByKey<Vendors>(obj.Vendor.Oid);
                        }
                        if (obj.Manufacturer != null)
                        {
                            objnewreq.Manufacturer = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.ICM.Manufacturer>(obj.Manufacturer.Oid);
                        }
                        objnewreq.UnitPrice = obj.UnitPrice;
                        if (obj.VendorCatName != null)
                        {
                            objnewreq.Catalog = obj.VendorCatName;
                        }
                        IList<Employee> obj1 = ObjectSpace.GetObjects<Employee>(CriteriaOperator.Parse("Oid = ?", objnewreq.RequestedBy.Oid));
                        foreach (var rec in obj1)
                        {
                            if (rec.Department != null)
                            {
                                objnewreq.department = rec.Department;
                            }
                        }
                        //objnewreq.ExpPrice = string.Format("{0:0.00}", objnewreq.OrderQty * obj.UnitPrice);
                        objnewreq.ExpPrice = Math.Round(objnewreq.OrderQty * obj.UnitPrice, 2, MidpointRounding.ToEven);
                        ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(objnewreq));
                    }
                }
                else
                {
                    e.CanCloseWindow = false;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "dataselect"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RequisitionAddItem_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Items));
                ListView CreateListView = Application.CreateListView("Items_LookupListView", cs, false);
                e.View = CreateListView;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Requisitiondelete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Requisition item = (Requisition)e.CurrentObject;
                if (item.RQID == null)
                {
                    ((ListView)View).CollectionSource.Remove(item);
                    ObjectSpace.RemoveFromModifiedObjects(item);
                    objreq.Items.Remove(item.Item.items);
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Requisitionview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
                DefaultSetting objdefset = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'RequisitionApproval'"));
                if (objdefset != null && objdefset.Select == true)
                {
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] = '8' Or [Status] = 'PendingApproval' Or [Status] = 'Level2Pending' Or [Status] = 'Level3Pending' Or [Status] = 'Level4Pending' Or [Status] = 'Level5Pending' Or [Status] = 'Level6Pending'");
                }
                else
                {
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] >= 'PendingOrdering'");
                }
                Frame.SetView(Application.CreateListView("Requisition_ListView_ViewMode", cs, false));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RequisitionDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedChoiceActionItem.Id == "1M")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                }
                else if (e.SelectedChoiceActionItem.Id == "3M")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                }
                else if (e.SelectedChoiceActionItem.Id == "6M")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6");
                }
                else if (e.SelectedChoiceActionItem.Id == "1Y")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1");
                }
                else if (e.SelectedChoiceActionItem.Id == "2Y")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2");
                }
                else if (e.SelectedChoiceActionItem.Id == "5Y")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5");
                }
                else if (e.SelectedChoiceActionItem.Id == "ALL")
                {
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] <> 0");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RequisitionDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && RequisitionDateFilter != null && RequisitionDateFilter.SelectedItem != null)
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (View.Id == "Requisition_ListView_ViewMode")
                    {
                        if (strSelectedItem == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReviewedDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReviewedDate, Now()) <= 3");
                        }
                        else if (strSelectedItem == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ReviewedDate, Now()) <= 6");
                        }
                        else if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReviewedDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReviewedDate, Now()) <= 2");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ReviewedDate, Now()) <= 5");
                        }
                        else if (strSelectedItem == "All")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] = 2");
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
    }
}
