using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LDM.Module.Controllers.ICM
{
    public partial class POViewController : ViewController, IXafCallbackHandler
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        ShowNavigationItemController ShowNavigationController;
        ModificationsController mdcSave;
        ModificationsController mdcSaveClose;
        ICMinfo objInfo = new ICMinfo();
        string objclose;
        Poquerypanelinfo poinfo = new Poquerypanelinfo();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        int intSelectedItemsCount = 0;
        PurchaseOrderInfo POInfo = new PurchaseOrderInfo();
        ICallbackManagerHolder sheet;
        #endregion

        #region Constructor
        public POViewController()
        {
            InitializeComponent();
            TargetViewId = "Requisition_ListView_PurchaseOrder_Lookup;" + "Purchaseorder_ListView_Approve;" + "Vendors_LookupListView_PO;" +
                "Purchaseorder_DetailView_Approve;" + "Purchaseorder_ListView;" + "Purchaseorder_DetailView;" + "Purchaseorder_Item_ListView;" +
                "Requisition_ListView_Purchaseorder_Vendor;" + "Requisition_ListView_Purchaseorder_Mainview;" + "POquerypanel_DetailView;" +
                "Requisition_ListView_Purchaseorder_ViewMode;" + "Purchaseorder_Item_ListView";
            POTracking.TargetViewId = "Requisition_ListView_Purchaseorder_Mainview;"; //+ "Requisition_ListView_Purchaseorder_ViewMode;";
            //POTracking.Category = "RecordEdit";
            //POTracking.Model.Index = 5 ;
            POrollback.TargetViewId = "Purchaseorder_DetailView;";
            POFilter.TargetViewId = "Requisition_ListView_Purchaseorder_ViewMode";
            //POFilter.Category = "RecordEdit";
            //POFilter.Model.Index =8;

            POReport.TargetViewId = "Requisition_ListView_Purchaseorder_ViewMode;" + "Purchaseorder_DetailView;";
        }
        #endregion

        #region Default Events
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                mdcSave = Frame.GetController<ModificationsController>();
                mdcSave.SaveAction.Executing += SaveAction_Executing;
                mdcSave.SaveAndCloseAction.Executing += SaveAction_Executing;

                //mdcSaveClose = Frame.GetController<ModificationsController>();
                //mdcSaveClose.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing += UnlinkAction_Executing;
                Frame.GetController<LinkUnlinkController>().LinkAction.Executing += LinkAction_Executing;
                Frame.GetController<WebModificationsController>().EditAction.Executing += EditAction_Executing;
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                //RefreshController refresh = Frame.GetController<RefreshController>();
                //BoolList boolList = refresh.Active;
                //refresh.RefreshAction.Execute += RefreshAction_Execute;
                ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;

                if (View.Id == "Purchaseorder_DetailView")
                {
                    SimpleAction objDelete = Frame.GetController<DeleteObjectsViewController>().DeleteAction;
                    objDelete.Caption = "Rollback";
                    objDelete.ImageName = "Action_Cancel";
                    objDelete.ToolTip = "Rollback";
                    objDelete.ConfirmationMessage = string.Empty;
                    objDelete.Active["ShowAction"] = false;
                    Purchaseorder objPurchaseOrder = (Purchaseorder)View.CurrentObject;
                    if (objPurchaseOrder != null && objPurchaseOrder.Item != null && objPurchaseOrder.Item.Count > 0)
                    {
                        POInfo.SelectedPOItem = new List<Requisition>();
                        foreach (Requisition option in objPurchaseOrder.Item)
                        {
                            if (!POInfo.SelectedPOItem.Contains(option))
                            {
                                POInfo.SelectedPOItem.Add(option);
                            }
                        }
                    }
                    if (!View.ObjectSpace.IsNewObject(objPurchaseOrder) && objPurchaseOrder.Item != null && objPurchaseOrder.Item.Count > 0)
                    {
                        POInfo.LinkedPOItems = objPurchaseOrder.Item.ToList();
                    }
                    else
                    {
                        POInfo.LinkedPOItems = new List<Requisition>();
                    }
                    POInfo.UnLinkedPOItems = new List<Requisition>();
                    POInfo.IsItemSelected = false;
                }

                if (View != null && View.CurrentObject != null && View.Id == "POquerypanel_DetailView")
                {
                    POquerypanel RPanel = (POquerypanel)View.CurrentObject;
                    poinfo.rgMode = RPanel.Mode.ToString();
                }
                if (View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
                {
                    Frame.GetController<POViewController>().Actions["POrollback"].Active.SetItemValue("", true);

                    if (POFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        POFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        POFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        POFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        POFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        POFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        POFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        POFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    POFilter.SelectedItemChanged += POFilter_SelectedItemChanged;
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (POFilter.SelectedItem == null)
                    {
                        //DateTime srDateFilter = DateTime.MinValue;
                        if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                        {
                            //srDateFilter = DateTime.Today.AddMonths(-1);
                            POFilter.SelectedItem = POFilter.Items[0];
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                        {
                            //srDateFilter = DateTime.Today.AddMonths(-3);
                            POFilter.SelectedItem = POFilter.Items[1];
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                        {
                            //srDateFilter = DateTime.Today.AddMonths(-6);
                            POFilter.SelectedItem = POFilter.Items[2];
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                        {
                            //srDateFilter = DateTime.Today.AddYears(-1);
                            POFilter.SelectedItem = POFilter.Items[3];
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                        {
                            //srDateFilter = DateTime.Today.AddYears(-2);
                            POFilter.SelectedItem = POFilter.Items[4];
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                        {
                            //srDateFilter = DateTime.Today.AddYears(-5);
                            POFilter.SelectedItem = POFilter.Items[5];
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5");
                        }
                        else
                        {
                            //srDateFilter = DateTime.MinValue;
                            POFilter.SelectedItem = POFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[POID.POID] IS NOT NULL");
                        }                        
                    }
                    //POFilter.SelectedIndex = 0;
                    //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                }
                //if (View.Id == "Requisition_ListView_Purchaseorder_Vendor")
                //{
                //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                //    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                //    {
                //        lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                //        lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                //        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                //        List<object> groups = new List<object>();
                //        foreach (ViewRecord rec in lstview)
                //            groups.Add(rec["Toid"]);
                //        ((ListView)View).CollectionSource.Criteria["Distinct1"] = new InOperator("Oid", groups);
                //    }
                //}

                if (View.Id == "Purchaseorder_Item_ListView")
                {
                    if (ObjectSpace.IsNewObject(Application.MainWindow.View.CurrentObject))
                    {
                        Frame.GetController<LinkUnlinkController>().Actions["Unlink"].Active.SetItemValue("HideUnlink", false);
                        Frame.GetController<LinkUnlinkController>().Actions["Link"].Active.SetItemValue("HideLink", false);
                    }

                }
                if (base.View != null && base.View.Id == "Requisition_ListView_Purchaseorder_Mainview")
                {
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                    {
                        if (poinfo.poquery == null || poinfo.poquery.Length == 0)
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                            lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                        }
                        else
                        {
                            lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter8"] = CriteriaOperator.Parse(poinfo.poquery);
                            //lstview.Criteria = CriteriaOperator.Parse(poinfo.poquery);
                        }

                    }


                    //ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //ASPxGridView gv = gridListEditor.Grid;
                    //gridListEditor.Grid.Load += Grid_Load;
                    //gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e) {
                    //        window.setTimeout(function() { s.Refresh(); }, 20); }";
                }
                if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                {
                    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                    if (popupWindow != null)
                    {
                        //popupWindow.RefreshParentWindowOnCloseButtonClick = true;
                        DialogController dc = popupWindow.GetController<DialogController>();
                        if (dc != null)
                        {
                            dc.Accepting += Dc_Accepting;
                            dc.AcceptAction.Executed += AcceptAction_Executed;
                            dc.SaveOnAccept = false;
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

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                Purchaseorder objPO = (Purchaseorder)Application.MainWindow.View.CurrentObject;
                ViewItem viItem = ((DetailView)Application.MainWindow.View).FindItem("Item");

                foreach (Requisition objRequisition in objPO.Item.ToList())
                {
                    if (POInfo.LinkedPOItems.FirstOrDefault(i => i.Oid == objRequisition.Oid) == null)
                    {
                        objPO.Item.Remove(objRequisition);
                        if (viItem != null && viItem.View != null)
                        {
                            ((ListPropertyEditor)viItem).ListView.CollectionSource.Remove(objRequisition);
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

        private void LinkAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                Purchaseorder objPO = (Purchaseorder)Application.MainWindow.View.CurrentObject;
                ViewItem viItem = ((DetailView)Application.MainWindow.View).FindItem("Item");

                //foreach (Requisition objRequisition in objPO.Item.ToList())
                //{
                //    if (POInfo.LinkedPOItems.FirstOrDefault(i => i.Oid == objRequisition.Oid) == null)
                //    {
                //        objPO.Item.Remove(objRequisition);
                //        if (viItem != null && viItem.View != null)
                //        {
                //            ((ListView)viItem.View).CollectionSource.Remove(objRequisition);
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

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                //if (View != null && View.Id == "Purchaseorder_DetailView")
                //{
                //    ListPropertyEditor dvSample = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
                //    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Requisition));
                //    cs.Criteria["Filtervendor"] = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                //    foreach (Requisition reqobj in cs.List)
                //    {
                //        ((ListView)dvSample.ListView).CollectionSource.Add(reqobj);
                //    }
                //}

                //POrollback.Active["ShowAction"] = false;
                //if (base.View != null && base.View.Id == "Requisition_ListView_Purchaseorder_Mainview")
                //{
                //using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                //{
                //    if (poinfo.poquery == null || poinfo.poquery.Length == 0)
                //    {
                //        lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                //        lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                //        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                //        List<object> groups = new List<object>();
                //        foreach (ViewRecord rec in lstview)
                //            groups.Add(rec["Toid"]);
                //        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                //    }
                //    else
                //    {
                //        lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                //        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                //        List<object> groups = new List<object>();
                //        foreach (ViewRecord rec in lstview)
                //            groups.Add(rec["Toid"]);
                //        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                //        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter8"] = CriteriaOperator.Parse(poinfo.poquery);
                //        //lstview.Criteria = CriteriaOperator.Parse(poinfo.poquery);
                //    }

                //}

                //ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //ASPxGridView gv = gridListEditor.Grid;
                //gridListEditor.Grid.Load += Grid_Load;
                //gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e) {
                //        window.setTimeout(function() { s.Refresh(); }, 20); }";
                //}
                if (View != null && View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
                {
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[POID.POID] IS NOT NULL and POID.GCRecord is null");
                        lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                        lstview.Properties.Add(new ViewProperty("POID", SortDirection.Ascending, "POID", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                    }
                    //if (POReport.Items.Count == 1)
                    //{
                    //    var item1 = new ChoiceActionItem();
                    //    var item2 = new ChoiceActionItem();
                    //    POReport.Items.Add(new ChoiceActionItem("PurchaseOrder", item1));
                    //}
                    //    var item5 = new ChoiceActionItem();
                    //    var item6 = new ChoiceActionItem();
                    //    var item7 = new ChoiceActionItem();
                    //    POFilter.Items.Add(new ChoiceActionItem("1M", item1));
                    //    POFilter.Items.Add(new ChoiceActionItem("3M", item2));
                    //    POFilter.Items.Add(new ChoiceActionItem("6M", item3));
                    //    POFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                    //    POFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                    //    POFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                    //    POFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    //    POFilter.SelectedIndex = 1;
                    //}
                    //POFilter.SelectedItemChanged += POFilter_SelectedItemChanged;
                    //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                    //if (POReport.Items.Count == 1)
                    //{
                    //    var item1 = new ChoiceActionItem();
                    //    var item2 = new ChoiceActionItem();
                    //    POReport.Items.Add(new ChoiceActionItem("PurchaseOrder", item1));
                    //}
                    //POFilter.SelectedIndex = 0;
                    //DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    //if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                    //{
                    //    POFilter.SelectedIndex = 0;
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                    //}
                    //else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                    //{
                    //    POFilter.SelectedIndex = 1;
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                    //}
                    //else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                    //{
                    //    POFilter.SelectedIndex = 2;
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6");
                    //}
                    //else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                    //{
                    //    POFilter.SelectedIndex = 3;
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1");
                    //}
                    //else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                    //{
                    //    POFilter.SelectedIndex = 4;
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2");
                    //}
                    //else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                    //{
                    //    POFilter.SelectedIndex = 5;
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5");
                    //}
                    //else if (setting.InventoryWorkFlow == EnumDateFilter.All)
                    //{
                    //    POFilter.SelectedIndex = 6;
                    //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[POID.POID] IS NOT NULL");
                    //}
                    //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");

                }
                if (View != null && View.Id == "Purchaseorder_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridview = gridListEditor.Grid;
                    if (gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    }
                }
                if (View != null && View.Id == "Vendors_LookupListView_PO")
                {
                    if (poinfo.rgMode == ENMode.View.ToString())
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Purchaseorder)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("Not IsNullOrEmpty([POID])");
                            lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor.Vendor.Vendor", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["TVendor"]);
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Vendor", groups);
                        }
                    }
                    else
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Requisition)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                            lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor.Vendor", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["TVendor"]);
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Vendor", groups);
                        }
                    }
                }
                //if (View != null && View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
                //{
                //    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    ASPxGridView gv = gridListEditor.Grid;
                //   // gridListEditor.Grid.Load += Grid_Load;
                //    gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e) {
                //            window.setTimeout(function() { s.Refresh(); }, 20); }";
                //}
                if (View.Id != null && View.Id == "Purchaseorder_DetailView" && View.CurrentObject != null)
                {
                    Purchaseorder obj = (Purchaseorder)View.CurrentObject;
                    if (obj.POID != null)
                    {
                        if (obj.Vendor != null)
                        {
                            obj.VendorName = obj.Vendor.Vendor.Vendor;
                            obj.Account = obj.Vendor.Vendor.Account;
                            obj.Phone = obj.Vendor.Vendor.Phone;
                            obj.Email = obj.Vendor.Vendor.Email;
                            obj.Contact = obj.Vendor.Vendor.Contact;
                            obj.Address = obj.Vendor.Vendor.Address1;
                        }
                        obj.NonPersisitantTotalPrice = Convert.ToDouble(obj.TotalPrice);
                    }
                    if (((DetailView)View).ViewEditMode is ViewEditMode.View)
                    {
                        //POrollback.Active["ShowAction"] = true;
                        if (View.ObjectSpace.IsNewObject(obj) || obj.Item == null || obj.Item.Count == 0)
                        {
                            POReport.Active["ShowAction"] = false;
                        }
                        else
                        {
                            POReport.Active["ShowAction"] = true;
                        }
                    }
                    else
                    {
                        //POrollback.Active.RemoveItem("ShowAction");
                        //POrollback.Active["ShowAction"] = false;
                        if (View.ObjectSpace.IsNewObject(obj) || obj.Item == null || obj.Item.Count == 0)
                        {
                            POReport.Active["ShowAction"] = false;
                        }
                        else
                        {
                            POReport.Active["ShowAction"] = true;
                        }
                    }

                    //ListPropertyEditor lst = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
                    //ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)lst.ListView).Editor;
                    //var selection = gridlst.GetSelectedObjects();
                    //foreach (Requisition reobj in obj.Item.ToList())
                    //{
                    //    if (!selection.Contains(reobj))
                    //    {
                    //        ((ListView)lst.ListView).CollectionSource.Remove(reobj);
                    //    }
                    //    else if ((selection.Contains(reobj)))
                    //    {
                    //        reobj.POID = obj;
                    //        reobj.Status = Requisition.TaskStatus.PendingReceived;

                    //    }
                    //}
                    Application.MainWindow.View.Refresh();

                    sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    sheet.CallbackManager.RegisterHandler("openspreadsheet", this);
                }
                if (View.Id != null && View.Id == "Requisition_ListView_PurchaseOrder_Lookup")
                {
                    //ListPropertyEditor lst = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
                    //ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)lst.ListView).Editor;
                    //var selection = gridlst.GetSelectedObjects();
                    //foreach (Requisition reobj in obj.Item.ToList())
                    //{
                    //    if (!selection.Contains(reobj))
                    //    {
                    //        ((ListView)lst.ListView).CollectionSource.Remove(reobj);
                    //    }
                    //    else if ((selection.Contains(reobj)))
                    //    {
                    //        reobj.POID = obj;
                    //        reobj.Status = Requisition.TaskStatus.PendingReceived;

                    //    }
                    //}

                    Purchaseorder objPurchase = (Purchaseorder)Application.MainWindow.View.CurrentObject;

                    if (objPurchase != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Vendor] = ? and [NonPersistentStatus]= 'PendingPurchasing'", objPurchase.Vendor.Vendor);
                        if (objPurchase.Item != null && objPurchase.Item.Count > 0)
                        {
                            //string strCriteria = "Not [Item] In(" + string.Format("'{0}'", string.Join("','", objPurchase.Item.Select(i => i.Item.Oid.ToString().Replace("'", "''")))) + ")";
                            string strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", objPurchase.Item.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")";
                            ((ListView)View).CollectionSource.Criteria["Filter2"] = CriteriaOperator.Parse(strCriteria);
                        }
                    }
                }
                if (View.Id == "Purchaseorder_Item_ListView")
                {
                    ICallbackManagerHolder handlerid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    handlerid.CallbackManager.RegisterHandler("PoCheckHandler", this);
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                        editor.Grid.Load += Grid_Load;
                        editor.Grid.CustomSummaryCalculate += Grid_CustomSummaryCalculate;
                        editor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                          if (e.visibleIndex != '-1')
                          {
                            s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {  
                             if (s.IsRowSelectedOnPage(e.visibleIndex)) { 
                                RaiseXafCallback(globalCallbackControl, 'PoCheckHandler', 'Selected|' + Oidvalue , '', false);    
                             }
                            else{
                                RaiseXafCallback(globalCallbackControl, 'PoCheckHandler', 'UNSelected|' + Oidvalue, '', false);    
                             }
                            }); 
                          }
                          else if(e.visibleIndex == '-1' &&  s.GetSelectedRowCount() == s.cpVisibleRowCount)
                          { 
                            RaiseXafCallback(globalCallbackControl, 'PoCheckHandler', 'SelectAll', '', false);     
                          }
                          else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                          {
                            RaiseXafCallback(globalCallbackControl, 'PoCheckHandler', 'UNSelectAll', '', false);                        
                          }                      
                        }";
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (View.Id == "Purchaseorder_Item_ListView" && e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    List<Requisition> lstRequisition = ((ListView)View).SelectedObjects.Cast<Requisition>().ToList();
                    if (lstRequisition != null && lstRequisition.Count > 0)
                    {
                        e.TotalValue = "Sum =$" + lstRequisition.Sum(i => i.ExpPrice);
                    }
                    else
                    {
                        e.TotalValue = "Sum =$0.00";
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

        //private void Grid_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ASPxGridView gridView = sender as ASPxGridView;
        //        var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
        //        selectionBoxColumn.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (base.View != null && base.View.Id == "Requisition_ListView_Purchaseorder_Mainview")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    double Total = 0;
                    Requisition reqobj = (Requisition)e.InnerArgs.CurrentObject;
                    Purchaseorder objToShow = (Purchaseorder)objspace.CreateObject(typeof(Purchaseorder));
                    objToShow.Vendor = objspace.GetObject<Requisition>(reqobj);
                    objInfo.Vendor = reqobj.Vendor.Vendor;
                    objToShow.PurchaseBy = objspace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                    objToShow.VendorName = reqobj.Vendor.Vendor;
                    objToShow.Account = reqobj.Vendor.Account;
                    objToShow.Phone = reqobj.Vendor.Phone;
                    objToShow.Address = reqobj.Vendor.Address1;
                    objToShow.Contact = reqobj.Vendor.Contact;
                    objToShow.Email = reqobj.Vendor.Email;
                    CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
                    cs.Criteria["Filtervendor"] = CriteriaOperator.Parse("[Status] = 'PendingOrdering' And [Vendor] =?", objToShow.Vendor.Vendor);
                    foreach (Requisition reqobjvendor in cs.List)
                    {
                        //Total = Convert.ToDouble(Total) + Convert.ToDouble(reqobjvendor.ExpPrice);
                        objToShow.Item.Add(reqobjvendor);
                    }
                    // objToShow.TotalPrice = Total.ToString();


                    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDetailView(objspace, objToShow, true);
                    Frame.GetController<POViewController>().Actions["POrollback"].Active.SetItemValue("", false);
                    e.Handled = true;
                }
                else if (View != null && View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace(typeof(Purchaseorder));
                    Purchaseorder reqobj = objspace.FindObject<Purchaseorder>(CriteriaOperator.Parse("POID =?", ((Requisition)e.InnerArgs.CurrentObject).POID.POID));
                    if (reqobj != null)
                    {
                        if (reqobj.Vendor != null && reqobj.Vendor.Vendor != null)
                        {
                            objInfo.Vendor = reqobj.Vendor.Vendor.Vendor;
                            //reqobj.PurchaseBy = objspace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                            reqobj.VendorName = objInfo.Vendor;
                            reqobj.Account = reqobj.Vendor.Vendor.Account;
                            reqobj.Phone = reqobj.Vendor.Vendor.Phone;
                            reqobj.Email = reqobj.Vendor.Vendor.Email;
                            reqobj.Contact = reqobj.Vendor.Vendor.Contact;
                            reqobj.Address = reqobj.Vendor.Vendor.Address1;
                        }
                        e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDetailView(objspace, reqobj, true);
                        Frame.GetController<POViewController>().Actions["POrollback"].Active.SetItemValue("", true);
                        e.Handled = true;
                    }
                }
                //else if (base.View != null && base.View.Id == "Purchaseorder_ListView")
                //{
                //    IObjectSpace objspace = Application.CreateObjectSpace(typeof(Purchaseorder));
                //    Purchaseorder reqobj = objspace.GetObject<Purchaseorder>((Purchaseorder)e.InnerArgs.CurrentObject);
                //    //Purchaseorder objToShow = (Purchaseorder)objspace.CreateObject(typeof(Purchaseorder));
                //    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDetailView(objspace, reqobj, true);
                //    e.Handled = true;
                //}
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
            try
            {
                SimpleAction objDelete = Frame.GetController<DeleteObjectsViewController>().DeleteAction;
                if (objDelete != null)
                {
                    objDelete.Caption = "Delete";
                    objDelete.ImageName = "Action_Delete";
                    objDelete.ToolTip = "Delete";
                    objDelete.ConfirmationMessage = @"You are about to delete the selected record(s). Do you want to proceed?";
                }
                mdcSave = Frame.GetController<ModificationsController>();
                mdcSave.SaveAction.Executing -= SaveAction_Executing;
                //mdcSaveClose = Frame.GetController<ModificationsController>();
                //mdcSaveClose.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                Frame.GetController<LinkUnlinkController>().UnlinkAction.Executing -= UnlinkAction_Executing;
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                Frame.GetController<DeleteObjectsViewController>().DeleteAction.Caption = "Delete";
                Frame.GetController<WebModificationsController>().EditAction.Executing -= EditAction_Executing;
                if (View.Id == "Purchaseorder_DetailView")
                {
                    if (POrollback.Active.Contains("ShowAction"))
                    {
                        POrollback.Active.RemoveItem("ShowAction");
                    }
                    if (POReport.Active.Contains("ShowAction"))
                    {
                        POReport.Active.RemoveItem("ShowAction");
                    }
                    POInfo.LinkedPOItems = new List<Requisition>();
                    POInfo.UnLinkedPOItems = new List<Requisition>();
                    if (objDelete.Active.Contains("ShowAction"))
                    {
                        objDelete.Active.RemoveItem("ShowAction");
                    }
                }
                if (View.Id == "Purchaseorder_Item_ListView")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.CustomJSProperties -= Grid_CustomJSProperties;
                        editor.Grid.Load -= Grid_Load;
                        editor.Grid.CustomSummaryCalculate -= Grid_CustomSummaryCalculate;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                //if (View != null && View.Id == "Purchaseorder_DetailView" && View.CurrentObject == e.Object && e.PropertyName == "Vendor")
                //{
                //    if (View.ObjectTypeInfo.Type == typeof(Purchaseorder))
                //    {
                //        Purchaseorder objPurchaseorder = (Purchaseorder)e.Object;
                //        if (objPurchaseorder.Vendor != null)
                //        {
                //            if (e.OldValue != null && objPurchaseorder.Vendor != e.OldValue)
                //            {
                //                if (objPurchaseorder.Item.Count > 0)
                //                {
                //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "unlinkitem"), InformationType.Warning, 1500, InformationPosition.Top);
                //                    //objPurchaseorder.Vendor.Vendor = e.OldValue.ToString();
                //                    return;
                //                }
                //            }

                //            objInfo.Vendor = objPurchaseorder.Vendor.Vendor.Vendor;
                //            objPurchaseorder.VendorName = objPurchaseorder.Vendor.Vendor.Vendor;
                //            objPurchaseorder.Account = objPurchaseorder.Vendor.Vendor.Account;
                //            objPurchaseorder.Phone = objPurchaseorder.Vendor.Vendor.Phone;
                //        }
                //        else
                //        {
                //            objInfo.Vendor = string.Empty;
                //            objPurchaseorder.VendorName = string.Empty;
                //            objPurchaseorder.Account = string.Empty;
                //            objPurchaseorder.Phone = string.Empty;
                //        }

                //    }
                //}
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Vendor")
                {
                    if (View.ObjectTypeInfo.Type == typeof(POquerypanel))
                    {
                        POquerypanel RPanel = (POquerypanel)e.Object;
                        if (RPanel.Vendor != null)
                        {
                            if (poinfo.poquery != string.Empty)
                            {
                                poinfo.poquery = poinfo.poquery + "AND [Vendor.Vendor] == '" + RPanel.Vendor.Vendor + "'";
                            }
                            else
                            {
                                poinfo.poquery = "[Vendor.Vendor] == '" + RPanel.Vendor.Vendor + "'";
                            }
                        }
                    }
                }
                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Mode")
                {
                    if (View.ObjectTypeInfo.Type == typeof(POquerypanel))
                    {
                        POquerypanel Rpanel = (POquerypanel)e.Object;
                        if (Rpanel != null)
                        {
                            poinfo.rgMode = Rpanel.Mode.ToString();
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

        private void UnlinkAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (base.View != null && base.View.Id == "Purchaseorder_Item_ListView")
                {
                    foreach (Requisition reobj in View.SelectedObjects)
                    {
                        if (reobj.Status == Requisition.TaskStatus.Received || reobj.Status == Requisition.TaskStatus.PartiallyReceived)
                        {
                            Application.ShowViewStrategy.ShowMessage("Item already received, Cannot be unlinked", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                        else
                        {
                            reobj.POID = null;
                            reobj.Status = Requisition.TaskStatus.PendingOrdering;
                            //if (POInfo.SelectedPOItem.Contains(reobj))
                            //{
                            //    POInfo.SelectedPOItem.Remove(reobj);
                            //}
                            if (POInfo.SelectedPOItem.FirstOrDefault(i => i.Oid == reobj.Oid) != null)
                            {
                                POInfo.SelectedPOItem.Remove(POInfo.SelectedPOItem.FirstOrDefault(i => i.Oid == reobj.Oid));
                            }
                            if (POInfo.LinkedPOItems.FirstOrDefault(i => i.Oid == reobj.Oid) != null)
                            {
                                POInfo.LinkedPOItems.Remove(POInfo.LinkedPOItems.FirstOrDefault(i => i.Oid == reobj.Oid));
                            }
                            if (POInfo.UnLinkedPOItems.FirstOrDefault(i => i.Oid == reobj.Oid) == null)
                            {
                                POInfo.UnLinkedPOItems.Add(reobj);
                            }
                        }
                        POInfo.IsItemSelected = false;
                        // POInfo.IsItemSelected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Save()
        {
            try
            {
                if (base.View != null && base.View.Id == "Purchaseorder_DetailView")
                {
                    string POID = string.Empty;
                    Purchaseorder obj = (Purchaseorder)View.CurrentObject;
                    if (obj.POID == null)
                    {
                        IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Purchaseorder));
                        CriteriaOperator sam = CriteriaOperator.Parse("Max(SUBSTRING(POID, 2))");
                        string temprc = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Purchaseorder), sam, null)) + 1).ToString();
                        var curdate = DateTime.Now.ToString("yyMMdd");
                        if (temprc != "1")
                        {
                            var predate = temprc.Substring(0, 6);
                            if (predate == curdate)
                            {
                                temprc = "PO" + temprc;
                            }
                            else
                            {
                                temprc = "PO" + curdate + "01";
                            }
                        }
                        else
                        {
                            temprc = "PO" + curdate + "01";
                        }
                        POID = temprc;
                        obj.POID = temprc;

                        ListPropertyEditor lst = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
                        ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)lst.ListView).Editor;
                        List<Requisition> selection = gridlst.GetSelectedObjects().Cast<Requisition>().ToList();
                        foreach (Requisition reobj in obj.Item.ToList())
                        {
                            if (!selection.Contains(reobj))
                            {
                                ((ListView)lst.ListView).CollectionSource.Remove(reobj);
                            }
                            else if ((selection.Contains(reobj)))
                            {
                                reobj.POID = obj;
                                reobj.Status = Requisition.TaskStatus.PendingReceived;

                            }
                        }


                        ObjectSpace.CommitChanges();
                        //string strTempPath = Path.GetTempPath();
                        //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\PO\")) == false)
                        //{
                        //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\PO\"));
                        //}
                        //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\PO\" + timeStamp + ".pdf");
                        //XtraReport xtraReport = new XtraReport();

                        //objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        //SetConnectionString();

                        //DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                        ////DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                        //ObjReportingInfo.strPOID = "'" + POID + "'";
                        ////xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("ICMPurchaseOrderReport", ObjReportingInfo, false);
                        //xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("ICMPurchaseOrderReport", ObjReportingInfo, false);
                        ////DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                        //xtraReport.ExportToPdf(strExportedPath);
                        //string[] path = strExportedPath.Split('\\');
                        //int arrcount = path.Count();
                        //int sc = arrcount - 3;
                        //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                        ////WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        ////if (obj.TotalPrice != null && obj.NonPersisitantTotalPrice != 0 && obj.TotalPriceWithTax != 0)
                        ////{
                        ////    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        ////    //e.Cancel = true;


                        ////}
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        ////print report directly. combine this code - MKS
                        //string Print = ConfigurationManager.AppSettings["AllowPrint"];
                        //if (Print == "True")
                        //{
                        //    xtraReport.ShowPrintMarginsWarning = false;
                        //    ReportPrintTool printTool = new ReportPrintTool(xtraReport);
                        //    //printTool.AutoShowParametersPanel = false;
                        //    if (printTool.PrinterSettings.IsDefaultPrinter)
                        //    {
                        //        try
                        //        {
                        //            printTool.Print();
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        ListPropertyEditor lst = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
                        ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)lst.ListView).Editor;
                        var selection = gridlst.GetSelectedObjects();
                        List<Requisition> lstGridObjects = ((ListView)lst.ListView).CollectionSource.List.Cast<Requisition>().ToList();
                        foreach (Requisition reobj in obj.Item.ToList())
                        {
                            if ((selection.Contains(reobj)))
                            {
                                if (reobj.Status == Requisition.TaskStatus.PendingOrdering)
                                {
                                    reobj.POID = obj;
                                    reobj.Status = Requisition.TaskStatus.PendingReceived;
                                }
                                //if (lstGridObjects != null && lstGridObjects.Count > 0)
                                //{
                                //    if (lstGridObjects.FirstOrDefault(i => i.Oid == reobj.Oid) == null)
                                //    {
                                //        ((ListView)lst.ListView).CollectionSource.Add(reobj);
                                //    }
                                //}
                                //else
                                //{
                                //    ((ListView)lst.ListView).CollectionSource.Add(reobj);
                                //}
                            }
                            else
                            {
                                ((ListView)lst.ListView).CollectionSource.Remove(reobj);
                                reobj.POID = null;
                                reobj.Status = Requisition.TaskStatus.PendingOrdering;
                            }
                            //else
                            //{
                            //        reobj.POID = null;
                            //        reobj.Status = Requisition.TaskStatus.PendingOrdering;
                            //}
                        }
                        //lst.ListView.ObjectSpace.CommitChanges();
                        ObjectSpace.CommitChanges();

                        List<Guid> lstSelectedItemOid = selection.Cast<Requisition>().Select(i => i.Oid).Distinct().ToList();
                        List<Guid> lstCurItemOid = obj.Item.Select(i => i.Oid).Distinct().ToList();

                        IObjectSpace os = Application.CreateObjectSpace();
                        Purchaseorder objPO = os.GetObjectByKey<Purchaseorder>(obj.Oid);
                        if (objPO != null)
                        {
                            IEnumerable<Guid> lstMissingItemOid = lstSelectedItemOid.Except(lstCurItemOid);
                            foreach (Guid oid in lstMissingItemOid)
                            {
                                if (objPO.Item.FirstOrDefault(i => i.Oid == oid) == null)
                                {
                                    objPO.Item.Add(os.GetObjectByKey<Requisition>(oid));
                                }
                            }
                            if (obj.Item.Count != objPO.Item.Count)
                            {
                                foreach (Requisition req in obj.Item)
                                {
                                    if ((selection.Contains(req)) && objPO.Item.FirstOrDefault(i => i.Oid == req.Oid) == null)
                                    {
                                        objPO.Item.Add(os.GetObject<Requisition>(req));
                                    }
                                }
                            }
                        }

                        os.CommitChanges();
                        os.Dispose();

                        //foreach (Requisition reobj in obj.Item.ToList())
                        //{
                        //    if ((selection.Contains(reobj)))
                        //    {
                        //        if (reobj.Status == Requisition.TaskStatus.PendingOrdering)
                        //        {
                        //            reobj.POID = obj;
                        //            reobj.Status = Requisition.TaskStatus.PendingReceived;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        ((ListView)lst.ListView).CollectionSource.Remove(reobj);
                        //        reobj.POID = null;
                        //        reobj.Status = Requisition.TaskStatus.PendingOrdering;
                        //    }
                        //}
                    }

                    if (POInfo.SelectedPOItem != null)
                    {
                        POInfo.SelectedPOItem = obj.Item.ToList<Requisition>();
                        POInfo.IsItemSelected = false;
                    }

                    //Navigate to List View
                    //IObjectSpace objspace = Application.CreateObjectSpace();
                    //CollectionSource collection = new CollectionSource(objspace, typeof(Requisition));
                    //Frame.SetView(Application.CreateListView("Requisition_ListView_Purchaseorder_Mainview", collection, true));

                    //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    //{
                    //    if (parent.Id == "InventoryManagement")
                    //    {
                    //        foreach (ChoiceActionItem child in parent.Items)
                    //        {
                    //            if (child.Id == "Operations")
                    //            {
                    //                foreach (ChoiceActionItem subchild in child.Items)
                    //                {
                    //                    if (subchild.Id == "OrderingItems")
                    //                    {
                    //                        int intOrderValue = 0;
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingOrdering'"));
                    //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        CriteriaOperator criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                    //                        IList<Requisition> req = objectSpace.GetObjects<Requisition>(criteria);
                    //                        string[] strVendor = new string[req.Count];
                    //                        foreach (Requisition item in req)
                    //                        {
                    //                            if (!strVendor.Contains(item.Vendor.Vendor))
                    //                            {
                    //                                strVendor[intOrderValue] = item.Vendor.Vendor;
                    //                                intOrderValue = intOrderValue + 1;
                    //                            }
                    //                        }


                    //                        if (intOrderValue > 0)
                    //                        {
                    //                            subchild.Caption = cap[0] + " (" + intOrderValue + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            subchild.Caption = cap[0];
                    //                        }
                    //                    }
                    //                    else if (subchild.Id == "OrderingApproval")
                    //                    {
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        var count = objectSpace.GetObjectsCount(typeof(Purchaseorder), CriteriaOperator.Parse("IsNullOrEmpty([ApprovedBy]) And [ApprovedDate] Is Null"));
                    //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        if (count > 0)
                    //                        {
                    //                            subchild.Caption = cap[0] + " (" + count + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            subchild.Caption = cap[0];
                    //                        }
                    //                    }
                    //                    else if (subchild.Id == "Receive Order")
                    //                    {
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        //CollectionSource cs = new CollectionSource(objectSpace, typeof(Requisition));
                    //                        //cs.Criteria["FilterPOID"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                    //                        //List<string> listpoid = new List<string>();
                    //                        //foreach (Requisition reqobjvendor in cs.List)
                    //                        //{
                    //                        //    if (!listpoid.Contains(reqobjvendor.POID.POID))
                    //                        //    {
                    //                        //        listpoid.Add(reqobjvendor.POID.POID);
                    //                        //    }
                    //                        //}
                    //                        //var count = listpoid.Count;
                    //                        var count = 0;
                    //                        using (XPView lstview = new XPView(((XPObjectSpace)objectSpace).Session, typeof(Requisition)))
                    //                        {
                    //                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                    //                            lstview.Properties.Add(new ViewProperty("TPOID", SortDirection.Ascending, "POID", true, true));
                    //                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //                            if (lstview != null && lstview.Count > 0)
                    //                            {
                    //                                count = lstview.Count;
                    //                            }
                    //                        }

                    //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        if (count > 0)
                    //                        {
                    //                            subchild.Caption = cap[0] + " (" + count + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            subchild.Caption = cap[0];
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                SimpleAction obj = (sender) as SimpleAction;
                objclose = obj.Id;
                if (base.View != null && base.View.Id == "Purchaseorder_DetailView")
                {
                    ListPropertyEditor lst = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
                    ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)lst.ListView).Editor;
                    Purchaseorder orderItem = (Purchaseorder)View.CurrentObject;
                    Regex numericRegex = new Regex(@"^\d+$");
                    if (orderItem != null)
                    {

                        if (orderItem.Shipping == null || orderItem.ShipVia == null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ShipViaShipOptionNotEmpty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }


                        //IObjectSpace os = Application.CreateObjectSpace();
                        //Purchaseorder valPo = os.GetObjectByKey<Purchaseorder>(orderItem.Oid);
                        //IEnumerable<Requisition> addedItems = orderItem.Item.Except(valPo.Item);
                        //IEnumerable<Requisition> removedItems = valPo.Item.Except(orderItem.Item);
                        //if (addedItems.Count() > 0 || removedItems.Count() > 0)
                        //{
                        //    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectitems"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        //    ICallbackManagerHolder handlerid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        //    handlerid.CallbackManager.RegisterHandler("PoCheckHandler", this);
                        //    string msg = "Do you want to add or remove the item from existing PO?";
                        //    WebWindow.CurrentRequestWindow.RegisterClientScript("AddingItemPO", string.Format(CultureInfo.InvariantCulture, @"var CanAddNewItem = confirm('" + msg + "'); {0}", handlerid.CallbackManager.GetScript("PoCheckHandler", "CanAddNewItem")));
                        //}
                        if (orderItem.Item.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages", "CannotSaveOrderAddItems"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        if (gridlst.GetSelectedObjects().Count == 0)
                        {
                            //if (ObjectSpace.IsNewObject(View.CurrentObject))
                            //{
                            //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectitems"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            //}
                            //else
                            //{
                            //    IObjectSpace os = Application.CreateObjectSpace();
                            //    //Purchaseorder obj = (Purchaseorder)View.CurrentObject;
                            //    Purchaseorder valPo = os.GetObjectByKey<Purchaseorder>(orderItem.Oid);

                            //    IEnumerable<Requisition> addedItems = orderItem.Item.Except(valPo.Item);
                            //    IEnumerable<Requisition> removedItems = valPo.Item.Except(orderItem.Item);
                            //    if (addedItems.Count() > 0 || removedItems.Count() > 0)
                            //    {
                            //        double TotalPrice = 0;
                            //        foreach (Requisition s in orderItem.Item)
                            //        {
                            //            TotalPrice = TotalPrice + Convert.ToDouble(s.ExpPrice);
                            //        }
                            //        orderItem.TotalPrice = TotalPrice.ToString();
                            //        orderItem.TotalPriceWithTax = TotalPrice + Convert.ToDouble(orderItem.ShippingCharge) + Convert.ToDouble(Convert.ToDouble(orderItem.Tax) / 100);
                            //        ObjectSpace.CommitChanges();
                            //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectitems"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            //    }
                            //}
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectitems"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                        if((orderItem.ShippingCharge!=null&&!numericRegex.IsMatch(orderItem.ShippingCharge))|| (orderItem.Tax != null && !numericRegex.IsMatch(orderItem.Tax)))
                        {
                            if (orderItem.ShippingCharge != null && !numericRegex.IsMatch(orderItem.ShippingCharge))
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\ICM", "OnlynumericInShippingCharge"), InformationType.Warning, timer.Seconds, InformationPosition.Top); 
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\ICM", "OnlynumericInTax"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            }
                            e.Cancel = true;
                        }
                        else
                        {
                            //Purchaseorder orderItem = (Purchaseorder)View.CurrentObject;
                            ListPropertyEditor lstitem = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
                            ASPxGridListEditor gridlstitem = (ASPxGridListEditor)((ListView)lst.ListView).Editor;
                            IList ss = gridlstitem.GetSelectedObjects();
                            double TotalPrice = 0;
                            foreach (Requisition s in ss)
                            {
                                TotalPrice = Math.Round(TotalPrice + Convert.ToDouble(s.ExpPrice), 2, MidpointRounding.ToEven);
                            }
                            orderItem.TotalPrice = TotalPrice.ToString();
                            orderItem.NonPersisitantTotalPrice = TotalPrice;
                            orderItem.TotalPriceWithTax = TotalPrice + Convert.ToDouble(orderItem.ShippingCharge) + Convert.ToDouble(Convert.ToDouble(orderItem.Tax) / 100);
                            Save();
                            e.Cancel = true;
                            WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Do you want to preview the purchase order report ?'); {0}", sheet.CallbackManager.GetScript("openspreadsheet", "openconfirm")));
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

        //private void SaveAndCloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    try
        //    {
        //        if (base.View != null && base.View.Id == "Purchaseorder_DetailView")
        //        {
        //            ListPropertyEditor lst = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
        //            ASPxGridListEditor gridlst = (ASPxGridListEditor)((ListView)lst.ListView).Editor;
        //            Purchaseorder orderItem = (Purchaseorder)View.CurrentObject;
        //            if (orderItem != null)
        //            {
        //                if (orderItem.Shipping == null || orderItem.ShipVia == null)
        //                {
        //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ShipViaShipOptionNotEmpty"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
        //                    e.Cancel = true;
        //                    return;
        //                }
        //                if (orderItem.Item.Count == 0)
        //                {
        //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages", "CannotSaveOrderAddItems"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
        //                    e.Cancel = true;
        //                    return;
        //                }
        //                if (gridlst.GetSelectedObjects().Count == 0)
        //                {
        //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectitems"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
        //                    e.Cancel = true;
        //                }
        //                else
        //                {
        //                    ListPropertyEditor lstitem = ((DetailView)View).FindItem("Item") as ListPropertyEditor;
        //                    ASPxGridListEditor gridlstitem = (ASPxGridListEditor)((ListView)lst.ListView).Editor;
        //                    IList ss = gridlstitem.GetSelectedObjects();
        //                    double TotalPrice = 0;
        //                    foreach (Requisition s in ss)
        //                    {
        //                        TotalPrice = Math.Round(TotalPrice + Convert.ToDouble(s.ExpPrice), 2, MidpointRounding.ToEven);
        //                    }
        //                    orderItem.TotalPrice = TotalPrice.ToString();
        //                    orderItem.NonPersisitantTotalPrice = TotalPrice;
        //                    orderItem.TotalPriceWithTax = TotalPrice + Convert.ToDouble(orderItem.ShippingCharge) + Convert.ToDouble(Convert.ToDouble(orderItem.Tax) / 100);
        //                    Save();
        //                    WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Do you want to preview the Purchase Order Report ?'); {0}", sheet.CallbackManager.GetScript("openspreadsheet", "openconfirm")));
        //                }
        //                }
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                IObjectSpace os = this.ObjectSpace;
                Session CS = ((XPObjectSpace)(os)).Session;
                var selected = gridListEditor.GetSelectedObjects();
                if (View.Id == "Purchaseorder_ListView_Approve")
                {
                    foreach (Purchaseorder obj in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(obj))
                        {
                            obj.ApprovedDate = DateTime.Now;
                            obj.ApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            obj.ApprovedDate = null;
                            obj.ApprovedBy = null;
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
        private void POApprove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count >= 1)
                {
                    if (base.View != null && base.View.Id == "Purchaseorder_ListView_Approve" || base.View.Id == "Purchaseorder_DetailView_Approve")
                    {
                        IObjectSpace os = this.ObjectSpace;
                        Session CS = ((XPObjectSpace)(os)).Session;
                        foreach (Purchaseorder obj in View.SelectedObjects)
                        {
                            obj.ApprovedDate = DateTime.Now;
                            obj.ApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                            foreach (Requisition reobj in obj.Item)
                            {
                                reobj.Status = Requisition.TaskStatus.PendingReceived;
                            }
                            ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                            View.Refresh();
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
                                                if (subchild.Id == "OrderingItems")
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
                                                }
                                                else if (subchild.Id == "OrderingApproval")
                                                {
                                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                    var count = objectSpace.GetObjectsCount(typeof(Purchaseorder), CriteriaOperator.Parse("IsNullOrEmpty([ApprovedBy]) And [ApprovedDate] Is Null"));
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
                                                else if (subchild.Id == "Receive Order")
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
                                            }
                                        }
                                    }
                                }
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "approvesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "dataapprove"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void POFilter_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        ((ListView)View).CollectionSource.Criteria.Clear();
        //        int RowCount = 0;
        //        if (View != null && View.Id == "Requisition_ListView_Purchaseorder_Mainview")
        //        {
        //            if (poinfo.poquery == string.Empty)
        //            {
        //                if (poinfo.rgMode == ENMode.View.ToString())
        //                {
        //                    IObjectSpace objspace = Application.CreateObjectSpace();
        //                    CollectionSource cs = new CollectionSource(objspace, typeof(Purchaseorder));
        //                    Frame.SetView(Application.CreateListView("Purchaseorder_ListView", cs, true));
        //                }
        //                else
        //                {
        //                    ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
        //                }
        //            }
        //            else if (poinfo.poquery != string.Empty)
        //            {
        //                if (poinfo.rgMode == ENMode.View.ToString())
        //                {
        //                    poinfo.poquery = poinfo.poquery + "And Not IsNullOrEmpty([POID])";
        //                    IObjectSpace objspace = Application.CreateObjectSpace();
        //                    CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
        //                    cs.Criteria["Filter"] = CriteriaOperator.Parse(poinfo.poquery);
        //                    Frame.SetView(Application.CreateListView("Requisition_ListView_Purchaseorder_ViewMode", cs, true));
        //                    //poinfo.poquery = poinfo.poquery.Replace("[Vendor.Vendor]", "[Vendor.Vendor.Vendor]");
        //                    //IObjectSpace objspace = Application.CreateObjectSpace();
        //                    //CollectionSource cs = new CollectionSource(objspace, typeof(Purchaseorder));
        //                    //cs.Criteria["Filter"] = CriteriaOperator.Parse(poinfo.poquery);
        //                    //Frame.SetView(Application.CreateListView("Purchaseorder_ListView", cs, true));
        //                }
        //                else
        //                {
        //                    //AND [Status] = 'PendingOrdering'
        //                    poinfo.poquery = poinfo.poquery + " And [NumItems] <> 0";
        //                    ((ListView)View).CollectionSource.Criteria["filter2"] = CriteriaOperator.Parse(poinfo.poquery);
        //                }
        //                RowCount = ((ListView)View).CollectionSource.GetCount();
        //                if (RowCount == 0)
        //                {
        //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalid"), InformationType.Info, 1500, InformationPosition.Top);
        //                }
        //            }
        //        }
        //        else if (View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
        //        {
        //            if (poinfo.poquery == string.Empty)
        //            {
        //                if (poinfo.rgMode == ENMode.View.ToString())
        //                {
        //                    ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse("Not IsNullOrEmpty([POID])");
        //                }
        //                else
        //                {
        //                    IObjectSpace objspace = Application.CreateObjectSpace();
        //                    CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
        //                    Frame.SetView(Application.CreateListView("Requisition_ListView_Purchaseorder_Mainview", cs, true));
        //                }
        //            }
        //            else if (poinfo.poquery != string.Empty)
        //            {
        //                if (poinfo.rgMode == ENMode.View.ToString())
        //                {
        //                    poinfo.poquery = poinfo.poquery + "And Not IsNullOrEmpty([POID])";
        //                    ((ListView)View).CollectionSource.Criteria["filter2"] = CriteriaOperator.Parse(poinfo.poquery);
        //                }
        //                else
        //                {
        //                    IObjectSpace objspace = Application.CreateObjectSpace();
        //                    CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
        //                    poinfo.poquery = poinfo.poquery + "And [NumItems] <> 0";
        //                    cs.Criteria["Filter"] = CriteriaOperator.Parse(poinfo.poquery);
        //                    Frame.SetView(Application.CreateListView("Requisition_ListView_Purchaseorder_Mainview", cs, true));
        //                }
        //                RowCount = ((ListView)View).CollectionSource.GetCount();
        //                if (RowCount == 0)
        //                {
        //                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalid"), InformationType.Info, 1500, InformationPosition.Top);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        //private void POFilter_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        //{
        //    try
        //    {
        //        poinfo.poquery = string.Empty;
        //        IObjectSpace objspace = Application.CreateObjectSpace();
        //        object objToShow = objspace.CreateObject(typeof(POquerypanel));
        //        if (objToShow != null)
        //        {
        //            DetailView CreateDetailView = Application.CreateDetailView(objspace, objToShow);
        //            CreateDetailView.ViewEditMode = ViewEditMode.Edit;
        //            e.View = CreateDetailView;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id); Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

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
        private void POrollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(ICMRollBack));
                    ICMRollBack obj = os.CreateObject<ICMRollBack>();
                    DetailView createdView = Application.CreateDetailView(os, "PORollBack_DetailView", true, obj);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += PORollBack_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        private void PORollBack_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                ICMRollBack objReason = (ICMRollBack)e.AcceptActionArgs.CurrentObject;
                if (!string.IsNullOrEmpty(objReason.Reason))
                {
                    if (View.ObjectTypeInfo.Type == typeof(Requisition))
                    {
                        foreach (Requisition reobj in View.SelectedObjects)
                        {
                            if (reobj.Status == Requisition.TaskStatus.Received || reobj.Status == Requisition.TaskStatus.PartiallyReceived)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreceivefail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        foreach (Requisition reobj in View.SelectedObjects)
                        {
                            Purchaseorder objPo = ObjectSpace.FindObject<Purchaseorder>(CriteriaOperator.Parse("POID=?", reobj.POID.POID));
                            if (objPo != null)
                            {
                                ObjectSpace.Delete(objPo);
                            }
                            reobj.Status = Requisition.TaskStatus.PendingOrdering;
                            reobj.POID = null;
                            reobj.RollbackReason = objReason.Reason;
                        }
                        ObjectSpace.CommitChanges();
                    }
                    else if (View.ObjectTypeInfo.Type == typeof(Purchaseorder))
                    {
                        Purchaseorder objPurchaseOrder = (Purchaseorder)View.CurrentObject;
                        if (objPurchaseOrder != null)
                        {
                            List<Requisition> lstRequisition = ObjectSpace.GetObjects<Requisition>(CriteriaOperator.Parse("POID.POID=?", objPurchaseOrder.POID)).ToList<Requisition>();
                            if (lstRequisition != null)
                            {

                                foreach (Requisition reobj in lstRequisition)
                                {
                                    if (reobj.Status == Requisition.TaskStatus.Received || reobj.Status == Requisition.TaskStatus.PartiallyReceived)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreceivefail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                }
                                foreach (Requisition reobj in lstRequisition)
                                {
                                    reobj.Status = Requisition.TaskStatus.PendingOrdering;
                                    reobj.POID = null;
                                    reobj.RollbackReason = objReason.Reason;
                                }
                            }
                            Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active["ShowAction"] = true;
                            Frame.GetController<DeleteObjectsViewController>().DeleteAction.DoExecute();
                            //ObjectSpace.Delete(objPurchaseOrder);
                            ObjectSpace.CommitChanges();
                        }

                    }
                    //IObjectSpace objspace = Application.CreateObjectSpace();
                    //CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
                    //cs.Criteria["Filter"] = CriteriaOperator.Parse(poinfo.poquery);
                    //Frame.SetView(Application.CreateListView("Requisition_ListView_Purchaseorder_ViewMode", cs, true));
                    //ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    //foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    //{
                    //    if (parent.Id == "InventoryManagement")
                    //    {
                    //        foreach (ChoiceActionItem child in parent.Items)
                    //        {
                    //            if (child.Id == "Operations")
                    //            {
                    //                foreach (ChoiceActionItem subchild in child.Items)
                    //                {
                    //                    if (subchild.Id == "OrderingItems")
                    //                    {
                    //                        int intOrderValue = 0;
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        var count = objectSpace.GetObjectsCount(typeof(Requisition), CriteriaOperator.Parse("[Status] = 'PendingOrdering'"));
                    //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        CriteriaOperator criteria = CriteriaOperator.Parse("[Status] = 'PendingOrdering'");
                    //                        IList<Requisition> req = objectSpace.GetObjects<Requisition>(criteria);
                    //                        string[] strVendor = new string[req.Count];
                    //                        foreach (Requisition item in req)
                    //                        {
                    //                            if (!strVendor.Contains(item.Vendor.Vendor))
                    //                            {
                    //                                strVendor[intOrderValue] = item.Vendor.Vendor;
                    //                                intOrderValue = intOrderValue + 1;
                    //                            }
                    //                        }


                    //                        if (intOrderValue > 0)
                    //                        {
                    //                            subchild.Caption = cap[0] + " (" + intOrderValue + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            subchild.Caption = cap[0];
                    //                        }
                    //                    }
                    //                    else if (subchild.Id == "OrderingApproval")
                    //                    {
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        var count = objectSpace.GetObjectsCount(typeof(Purchaseorder), CriteriaOperator.Parse("IsNullOrEmpty([ApprovedBy]) And [ApprovedDate] Is Null"));
                    //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        if (count > 0)
                    //                        {
                    //                            subchild.Caption = cap[0] + " (" + count + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            subchild.Caption = cap[0];
                    //                        }
                    //                    }
                    //                    else if (subchild.Id == "Receive Order")
                    //                    {
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        //CollectionSource cspoid = new CollectionSource(objectSpace, typeof(Requisition));
                    //                        //cspoid.Criteria["FilterPOID"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                    //                        //List<string> listpoid = new List<string>();
                    //                        //foreach (Requisition reqobjvendor in cspoid.List)
                    //                        //{
                    //                        //    if (!listpoid.Contains(reqobjvendor.POID.POID))
                    //                        //    {
                    //                        //        listpoid.Add(reqobjvendor.POID.POID);
                    //                        //    }
                    //                        //}
                    //                        //var count = listpoid.Count;
                    //                        var count = 0;
                    //                        using (XPView lstview = new XPView(((XPObjectSpace)objectSpace).Session, typeof(Requisition)))
                    //                        {
                    //                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived'");
                    //                            lstview.Properties.Add(new ViewProperty("TPOID", SortDirection.Ascending, "POID", true, true));
                    //                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //                            if (lstview != null && lstview.Count > 0)
                    //                            {
                    //                                count = lstview.Count;
                    //                            }
                    //                        }

                    //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        if (count > 0)
                    //                        {
                    //                            subchild.Caption = cap[0] + " (" + count + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            subchild.Caption = cap[0];
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        private void POTracking_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //poinfo.poquery = poinfo.poquery + "And Not IsNullOrEmpty([POID])";
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Requisition));
                cs.Criteria["Filter"] = CriteriaOperator.Parse("[POID.POID] IS NOT NULL and [POID.PurchaseDate] BETWEEN('" + DateTime.Now.AddMonths(-3) + "', '" + DateTime.Now + "')");
                //cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] = 'PendingReceived' Or[Status] = 'PartiallyReceived' or [Status] = 'Received'");
                Frame.SetView(Application.CreateListView("Requisition_ListView_Purchaseorder_ViewMode", cs, true));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void POFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
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
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[POID.POID] IS NOT NULL");
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void POFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && POFilter != null && POFilter.SelectedItem != null)
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
                    {
                        if (strSelectedItem == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 3");
                        }
                        else if (strSelectedItem == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(RequestedDate, Now()) <= 6");
                        }
                        else if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 2");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(RequestedDate, Now()) <= 5");
                        }
                        else if (strSelectedItem == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[POID.POID] IS NOT NULL and POID.GCRecord is null");
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
        private void POReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        string strPOID = string.Empty;
                        foreach (Requisition reqId in View.SelectedObjects)
                        {
                            if (strPOID == string.Empty)
                            {
                                strPOID = "'" + reqId.POID.POID + "'";
                            }
                            else
                            {
                                strPOID = strPOID + ",'" + reqId.POID.POID + "'";
                            }
                        }

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
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
                if (View != null && View.Id == "Purchaseorder_DetailView")
                {

                    string strPOID = string.Empty;
                    Purchaseorder objPurchaseOrder = (Purchaseorder)View.CurrentObject;
                    if (objPurchaseOrder != null && string.IsNullOrEmpty(objPurchaseOrder.POID) == false)
                    {
                        strPOID = "'" + objPurchaseOrder.POID + "'";
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
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("ICMPurchaseOrderReport", ObjReportingInfo, false);
                        //xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("PurchaseOrder_ICM2023", ObjReportingInfo, false);
                        //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                        xtraReport.ExportToPdf(strExportedPath);
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 3;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                    }
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
                if (((DetailView)Application.MainWindow.View).ViewEditMode is ViewEditMode.Edit)
                {
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        string[] arrParams = parameter.Split('|');
                        if (arrParams[0] == "UNSelectAll" || arrParams[0] == "SelectAll" || arrParams[0] == "Selected" || arrParams[0] == "UNSelected")
                        {
                            if (Frame is NestedFrame)
                            {
                                NestedFrame nestedFrame = (NestedFrame)Frame;
                                if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                                {
                                    CompositeView cv = nestedFrame.ViewItem.View;
                                    if (cv != null && cv.SelectedObjects != null)
                                    {
                                        Purchaseorder objPO = (Purchaseorder)cv.CurrentObject;
                                        if (arrParams[0] == "UNSelectAll")
                                        {
                                            objPO.NonPersisitantTotalPrice = 0;
                                            objPO.TotalPrice = objPO.NonPersisitantTotalPrice.ToString();
                                            objPO.NumItems = 0;
                                            intSelectedItemsCount = 0;
                                        }
                                        else if (arrParams[0] == "SelectAll")
                                        {

                                            objPO.NonPersisitantTotalPrice = Math.Round(objPO.Item.Sum(i => i.ExpPrice), 2, MidpointRounding.ToEven);
                                            objPO.TotalPrice = objPO.NonPersisitantTotalPrice.ToString();
                                            objPO.NumItems = objPO.Item.Count;
                                            intSelectedItemsCount = objPO.NumItems;
                                        }
                                        else if (arrParams[0] == "UNSelected")
                                        {
                                            double curTotalPrice = Convert.ToDouble(objPO.TotalPrice);
                                            Requisition objRequisition = ObjectSpace.FindObject<Requisition>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                                            if (objRequisition != null)
                                            {
                                                curTotalPrice = Math.Round(curTotalPrice - objRequisition.ExpPrice, 2, MidpointRounding.ToEven);
                                                objPO.NonPersisitantTotalPrice = curTotalPrice;
                                                objPO.TotalPrice = objPO.NonPersisitantTotalPrice.ToString();
                                                intSelectedItemsCount = intSelectedItemsCount - 1;
                                                objPO.NumItems = intSelectedItemsCount;
                                            }
                                        }
                                        else if (arrParams[0] == "Selected")
                                        {
                                            double curTotalPrice = Convert.ToDouble(objPO.TotalPrice);
                                            Requisition objRequisition = ObjectSpace.FindObject<Requisition>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                                            if (objRequisition != null)
                                            {
                                                curTotalPrice = Math.Round(curTotalPrice + objRequisition.ExpPrice, 2, MidpointRounding.ToEven);
                                                objPO.NonPersisitantTotalPrice = curTotalPrice;
                                                objPO.TotalPrice = objPO.NonPersisitantTotalPrice.ToString();
                                                intSelectedItemsCount = intSelectedItemsCount + 1;
                                                objPO.NumItems = intSelectedItemsCount;
                                            }

                                        }
                                        cv.Refresh();
                                    }
                                }
                            }
                        }
                    }
                }
                if (bool.TryParse(parameter, out bool showreport))
                {
                    if (showreport)
                    {
                        string strPOID = string.Empty;
                        Purchaseorder objPurchaseOrder = (Purchaseorder)View.CurrentObject;
                        if (objPurchaseOrder != null && string.IsNullOrEmpty(objPurchaseOrder.POID) == false)
                        {
                            strPOID = "'" + objPurchaseOrder.POID + "'";
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
                            xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("ICMPurchaseOrderReport", ObjReportingInfo, false);
                            //xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("PurchaseOrder_ICM2023", ObjReportingInfo, false);
                            //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 3;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                            if (objclose == "SaveAndClose")
                            {
                                View.Close();
                                objclose = null;
                            }
                        }
                        Application.ShowViewStrategy.ShowMessage("Purchase Order '" + objPurchaseOrder.POID + "' created successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Purchaseorder objPurchaseOrder = (Purchaseorder)View.CurrentObject;
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource collection = new CollectionSource(objspace, typeof(Requisition));
                        Frame.SetView(Application.CreateListView("Requisition_ListView_Purchaseorder_Mainview", collection, true));
                        if (objPurchaseOrder!=null)
                        {
                            Application.ShowViewStrategy.ShowMessage("Purchase Order '" + objPurchaseOrder.POID + "' created successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top); 
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

        private void SaveAndCloseAction_Executing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

        }

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Purchaseorder_Item_ListView" && POInfo.IsItemSelected == false)
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    if (POInfo.SelectedPOItem != null && POInfo.SelectedPOItem.Count > 0)
                    {
                        foreach (var obj in POInfo.SelectedPOItem)
                        {
                            gridView.Selection.SelectRowByKey(obj.Oid);
                        }
                    }
                    POInfo.IsItemSelected = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {

        }

        private void EditAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                POInfo.IsItemSelected = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (base.View != null && base.View.Id == "Requisition_ListView_PurchaseOrder_Lookup")
                {
                    if (POInfo.SelectedPOItem == null)
                    {
                        POInfo.SelectedPOItem = new List<Requisition>();
                    }
                    if (POInfo.LinkedPOItems == null)
                    {
                        POInfo.LinkedPOItems = new List<Requisition>();
                    }
                    if (POInfo.UnLinkedPOItems == null)
                    {
                        POInfo.UnLinkedPOItems = new List<Requisition>();
                    }
                    //IObjectSpace os = Application.CreateObjectSpace();
                    //List<Guid> lstExistingItems = new List<Guid>();
                    //Purchaseorder objPO = (Purchaseorder)Application.MainWindow.View.CurrentObject;
                    //if (objPO != null)
                    //{
                    //    Purchaseorder obj = os.GetObjectByKey<Purchaseorder>(objPO.Oid);
                    //    if (obj != null && obj.Item.Count > 0)
                    //    {
                    //        lstExistingItems = obj.Item.Select(i => i.Oid).Distinct().ToList();
                    //    }
                    //}
                    ASPxGridView gridView = sender as ASPxGridView;
                    foreach (Requisition reobj in e.AcceptActionArgs.SelectedObjects.Cast<Requisition>().ToList())
                    {
                        if (POInfo.SelectedPOItem.FirstOrDefault(i => i.Oid == reobj.Oid) == null)
                        {
                            POInfo.SelectedPOItem.Add(reobj);
                        }
                        if (POInfo.LinkedPOItems.FirstOrDefault(i => i.Oid == reobj.Oid) == null)
                        {
                            POInfo.LinkedPOItems.Add(reobj);
                        }
                        if (POInfo.UnLinkedPOItems.FirstOrDefault(i => i.Oid == reobj.Oid) != null)
                        {
                            POInfo.UnLinkedPOItems.Remove(reobj);
                        }
                        if (Application.MainWindow.View.ObjectSpace.ModifiedObjects.Count > 0)
                        {
                            //Requisition  objExistingItem = Application.MainWindow.View.ObjectSpace.ModifiedObjects.Cast<Requisition>().FirstOrDefault(i => i.Oid == reobj.Oid);
                            //List<Requisition> lstModifiedItems = new List<Requisition>();
                            foreach (object obj in Application.MainWindow.View.ObjectSpace.ModifiedObjects)
                            {
                                ObjectSpace.ModifiedObjects.Remove(obj);
                                //if (obj.GetType() == typeof(Requisition))
                                //{
                                //    lstModifiedItems.Add(obj as Requisition);
                                //}
                            }
                            //Requisition objExistingItem = lstModifiedItems.FirstOrDefault(i => i.Oid == reobj.Oid);
                            //if (objExistingItem != null)
                            //{
                            //    ObjectSpace.ModifiedObjects.Remove(objExistingItem);
                            //}
                        }
                    }
                    POInfo.IsItemSelected = false;

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
