using ALPACpre.Module.BusinessObjects.ICM;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
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
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.ICM
{
    public partial class DistributionViewController : ViewController, IXafCallbackHandler
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        ShowNavigationItemController ShowNavigationController;
        Distributionquerypanelinfo objdis = new Distributionquerypanelinfo();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        ICMinfo ICMInfo = new ICMinfo();
        ICallbackManagerHolder sheet;
        PermissionInfo objPermissionInfo = new PermissionInfo();
        //List<Guid> ObjectsToShow = new List<Guid>();
        string strLT = string.Empty;
        #endregion

        #region Constructor
        public DistributionViewController()
        {
            InitializeComponent();
            TargetViewId = "Distribution_DetailView_Rollback;" + "Distribution_ListView_Receiveid;" + "Distribution_ListView_Vendor;" + "Distributionquerypanel_DetailView;" + "Distribution_ListView;"
                + "Distribution_ListView_Item;" + "Distribution_ListView_MainDistribute;" + "Distribution_ListView_Viewmode;" + "Distribution_ListView_LTsearch;" + "Distribution_ListView_Viewmode_Copy;" + "Distribution_ListView_Fractional_Consumption;" + "Distribution_itemDepletionsCollection_ListView;"
                 + "Distribution_DetailView;" + "Distribution_ListView_ItemStockInventory;" + "Distribution_ListView_Fractional_Consumption_History;";
            DistributionQueryPanel.TargetViewId = "Distribution_ListView;" + "Distribution_ListView_MainDistribute;";
            Distribute.TargetViewId = "Distribution_ListView;" + "Distribution_ListView_Viewmode;";
            LTPreviewReport.TargetViewId = "Distribution_ListView_Viewmode;" + "Distribution_ListView_Viewmode_Copy;" + "Distribution_ListView_ItemStockInventory;";
            LTBarcodeReport = new SingleChoiceAction(this, "LTBarcodeReport", "View");
            LTBarcodeReport.TargetViewId = "Distribution_ListView_Viewmode;" + "Distribution_ListView_Viewmode_Copy;";
            LTBarcodeReport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            var item = new ChoiceActionItem();
            LTBarcodeReport.Items.Add(new ChoiceActionItem("", item));
            DistributeRollback.TargetViewId = "Distribution_ListView_Viewmode";
            NewAction.TargetViewId = "Distribution_itemDepletionsCollection_ListView";
            //DistributeRollback.Category = "RecordEdit";
            //DistributeRollback.Model.Index = 5;
            Distributionview.TargetViewId = "Distribution_ListView_MainDistribute";
            DistributionDateFilter.TargetViewId = "Distribution_ListView_Viewmode;";
            //DistributionDateFilter.Category = "RecordEdit";
            //DistributionDateFilter.Model.Index = 8;
            //DistributionSave.TargetViewId = "Distribution_ListView_Viewmode;";
            //DistributionSave.Category = "RecordEdit";
            //DistributionSave.Model.Index = 0;
            txtBarcodeActionDistribution.TargetViewId = "Distribution_ListView_Viewmode;" + "Distribution_ListView_LTsearch;" + "Distribution_ListView_Viewmode_Copy;";
            FractionalHistory.TargetViewId = "Distribution_ListView_Fractional_Consumption;";
            //objdis.DistributionFilter = string.Empty;
            txtBarcodeActionDistribution.CustomizeControl += TxtBarcodeActionDistribution_CustomizeControl;
        }
        #endregion

        #region Default Methods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {

                if (View.Id == "Distribution_ListView_Fractional_Consumption")
                {
                    FilterController filterController = Frame.GetController<FilterController>();
                    filterController.FullTextFilterAction.Executing += FullTextFilterAction_Executing;
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                }
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {
                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.DistributionIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.DistributionIsWrite = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "DistributionItem" && i.Write == true) != null)
                                {
                                    objPermissionInfo.DistributionIsWrite = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                Frame.GetController<DistributionViewController>().Actions["Distributionquerypanel"].Active.SetItemValue("", false);
                txtBarcodeActionDistribution.ValueChanged += TxtBarcodeActionDistribution_ValueChanged;
                txtBarcodeActionDistribution.Active["ShowScan"] = false;
                if (View != null && View.Id == "Distribution_ListView_Viewmode")
                {
                    ObjectSpace.Committed += ObjectSpace_Committed;
                    LTBarcodeReport.Active["Lable Type"] = false;
                    Distribute.Active["ShowDistribution"] = false;
                    DistributeRollback.Active["ShowRollback"] = objPermissionInfo.DistributionIsWrite;
                    if (LTBarcodeReport.Items.Count == 1)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        LTBarcodeReport.Items.Add(new ChoiceActionItem("LTBarCode", item1));
                        LTBarcodeReport.Items.Add(new ChoiceActionItem("LTBarCodeBig", item2));
                    }
                    if (DistributionDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        DistributionDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        DistributionDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        DistributionDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        DistributionDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        DistributionDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        DistributionDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        DistributionDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //DistributionDateFilter.SelectedIndex = 1;
                    DistributionDateFilter.SelectedItemChanged += DistributionDateFilter_SelectedItemChanged;
                    //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 3");
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (DistributionDateFilter.SelectedItem == null)
                    { 
                        if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                    {
                        DistributionDateFilter.SelectedIndex = 0;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 1");
                    }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                    {
                    DistributionDateFilter.SelectedIndex = 1;
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 3");
                    }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                    {
                        DistributionDateFilter.SelectedIndex = 2;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 6");
                    }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                    {
                        DistributionDateFilter.SelectedIndex = 3;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 1");
                    }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                    {
                        DistributionDateFilter.SelectedIndex = 4;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 2");
                    }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                    {
                        DistributionDateFilter.SelectedIndex = 5;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 5");
                    }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.All)
                    {
                        DistributionDateFilter.SelectedIndex = 6;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] <> 0");
                    }
                    //DistributionDateFilter.SelectedIndex = 0;
                    // ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 3");
                    }
                }
                else if (View.Id == "Distribution_ListView_MainDistribute")
                {
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                    {
                        //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                        lstview.Criteria = CriteriaOperator.Parse("[Status] = 0");
                        lstview.Properties.Add(new ViewProperty("TReceiveID", SortDirection.Ascending, "ReceiveID", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                    }
                    //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                else if (View.Id == "Distribution_ListView_Fractional_Consumption_History")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                else if (View.Id == "Distribution_ListView")
                {
                    Distribute.Active["ShowDistribution"] = objPermissionInfo.DistributionIsWrite;
                }
                else if (View.Id == "Distribution_ListView_Viewmode_Copy")
                {
                    if (LTBarcodeReport.Items.Count == 1)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        LTBarcodeReport.Items.Add(new ChoiceActionItem("LTBarCode", item1));
                        LTBarcodeReport.Items.Add(new ChoiceActionItem("LTBarCodeBig", item2));
                    }
                }

                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);


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
                //        if (navPermissionDetails != null && View.Id == "Distribution_ListView")
                //        {
                //            if (navPermissionDetails.Write == true)
                //            {
                //                Distribute.Active.SetItemValue("DistributionViewController.Distribute", true);
                //            }
                //            else if (navPermissionDetails.Write == false)
                //            {
                //                Distribute.Active.SetItemValue("DistributionViewController.Distribute", false);
                //            }
                //        }
                //    }
                //}
                if (View != null && View.CurrentObject != null && View.Id == "Distributionquerypanel_DetailView")
                {
                    Distributionquerypanel QPanel = (Distributionquerypanel)View.CurrentObject;
                    objdis.rgMode = QPanel.Mode.ToString();
                }
                if (View != null && View.Id == "Distribution_ListView_Fractional_Consumption")
                {
                    ListViewProcessCurrentObjectController listViewProcessCurrent = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listViewProcessCurrent != null)
                    {
                        listViewProcessCurrent.CustomProcessSelectedItem += ListViewProcessCurrent_CustomProcessSelectedItem;
                    }
                }
                if (View.Id == "Distribution_DetailView" || View.Id == "Distribution_itemDepletionsCollection_ListView")
                {
                    ModificationsController objmodifycontroller = Frame.GetController<ModificationsController>();
                    objmodifycontroller.SaveAction.Executing += SaveAction_Executing;
                    objmodifycontroller.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing; ;
                }
                if (View.Id == "Distribution_itemDepletionsCollection_ListView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    DeleteObjectsViewController objDeleteController = Frame.GetController<DeleteObjectsViewController>();
                    if (objDeleteController != null && objDeleteController.DeleteAction != null)
                    {
                        objDeleteController.DeleteAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (Application.MainWindow.View.Id == "Distribution_DetailView")
                {
                    Distribution objDistribution = (Distribution)Application.MainWindow.View.CurrentObject;
                    ListPropertyEditor listPropertyEditor = ((DetailView)View).FindItem("itemDepletionsCollection") as ListPropertyEditor;
                    if (listPropertyEditor != null && listPropertyEditor.ListView != null)
                    {
                        //if (listPropertyEditor.ListView.SelectedObjects.Count > 0)
                        {
                            if (listPropertyEditor.ListView.CollectionSource.List.Cast<ItemDepletion>().ToList().FirstOrDefault(i => i.AmountRemain == 0) != null)
                            {
                                if (objDistribution.Item != null && objDistribution.Item.StockQty > 0)
                                {
                                    objDistribution.Item.StockQty -= 1;
                                }
                                objDistribution.IsDeplete = true;
                                if (objDistribution.ConsumptionDate == DateTime.MinValue)
                                {
                                    objDistribution.ConsumptionDate = DateTime.Now;
                                }
                                if (objDistribution.ConsumptionBy == null)
                                {
                                    objDistribution.ConsumptionBy = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                }
                                objDistribution.Status = Distribution.LTStatus.Consumed;
                                //Application.ShowViewStrategy.ShowMessage("StockAmount is empty to check the stockamount", InformationType.Error, 3000, InformationPosition.Top);
                                //e.Cancel = true;
                                //Application.MainWindow.View.Refresh();
                            }
                            else if (listPropertyEditor.ListView.CollectionSource.List.Cast<ItemDepletion>().ToList().FirstOrDefault(i => i.AmountRemain != 0) != null)
                            {
                                listPropertyEditor.ListView.ObjectSpace.CommitChanges();
                            }
                        }
                        //else
                        //{
                        //    Application.ShowViewStrategy.ShowMessage(" Please select atleast one checkbox to save", InformationType.Error, 3000, InformationPosition.Top);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FullTextFilterAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id == "Distribution_ListView_Fractional_Consumption")
                {
                    if (((ListView)View).CollectionSource.Criteria.ContainsKey("Filter"))
                    {
                        ((ListView)View).CollectionSource.Criteria.Remove("Filter");
                    }
                    //((ListView)View).CollectionSource.Criteria["FranctionalFilter"] = CriteriaOperator.Parse("[Item.IsFractional]=? like '%'");
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
                if (Application.MainWindow.View.Id == "Distribution_DetailView")
                {
                    Distribution objDistribution = (Distribution)Application.MainWindow.View.CurrentObject;
                    ListPropertyEditor listPropertyEditor = ((DetailView)View).FindItem("itemDepletionsCollection") as ListPropertyEditor;
                    if (listPropertyEditor != null && listPropertyEditor.ListView != null)
                    {
                        //if (listPropertyEditor.ListView.SelectedObjects.Count > 0)
                        {
                            if (listPropertyEditor.ListView.CollectionSource.List.Cast<ItemDepletion>().ToList().FirstOrDefault(i => i.AmountRemain == 0) != null)
                            {
                                if (objDistribution.Item != null && objDistribution.Item.StockQty > 0)
                                {
                                    objDistribution.Item.StockQty -= 1;
                                }
                                objDistribution.IsDeplete = true;
                                if (objDistribution.ConsumptionDate == DateTime.MinValue)
                                {
                                    objDistribution.ConsumptionDate = DateTime.Now;
                                }
                                if (objDistribution.ConsumptionBy == null)
                                {
                                    objDistribution.ConsumptionBy = Application.MainWindow.View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                }
                                objDistribution.Status = Distribution.LTStatus.Consumed;
                                //Application.ShowViewStrategy.ShowMessage("StockAmount is empty to check the stockamount", InformationType.Error, 3000, InformationPosition.Top);
                                //e.Cancel = true;
                                //Application.MainWindow.View.Refresh();
                            }
                            else if (listPropertyEditor.ListView.CollectionSource.List.Cast<ItemDepletion>().ToList().FirstOrDefault(i => i.AmountRemain != 0) != null)
                            {
                                listPropertyEditor.ListView.ObjectSpace.CommitChanges();
                            }
                        }
                        //else
                        //{
                        //    Application.ShowViewStrategy.ShowMessage(" Please select atleast one checkbox to save", InformationType.Error, 3000, InformationPosition.Top);
                        //}
                    }
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
                if (Application.MainWindow.View.Id == "Distribution_DetailView")
                {
                    if (e.PropertyName == "AmountTaken" && e.NewValue != e.OldValue)
                    {
                        Distribution objDistribution = (Distribution)Application.MainWindow.View.CurrentObject;
                        ItemDepletion itemDepletion = (ItemDepletion)e.Object;
                        if (itemDepletion != null)
                        {
                            int intAmountRemain = 0;

                            if (itemDepletion.Distribution != null && itemDepletion.Distribution.itemDepletionsCollection.Count == 1 && !ObjectSpace.IsNewObject(itemDepletion))
                            {
                                intAmountRemain = itemDepletion.StockAmount - itemDepletion.AmountTaken;
                            }
                            else
                            if (objDistribution.itemDepletionsCollection.Where(i => i.Oid != itemDepletion.Oid).Count() > 0)
                            {
                                intAmountRemain = objDistribution.itemDepletionsCollection.Where(i => i.Oid != itemDepletion.Oid).Min(i => i.AmountRemain);
                            }
                            else
                            if (objDistribution.Item != null && !string.IsNullOrEmpty(objDistribution.Item.Amount))
                            {
                                intAmountRemain = Convert.ToInt32(objDistribution.Item.Amount);
                            }

                            if (itemDepletion.AmountTaken <= 0)
                            {
                                Application.ShowViewStrategy.ShowMessage("Please enter valid amount to consume.", InformationType.Error, 3000, InformationPosition.Top);
                                return;
                                //itemDepletion.AmountTaken = Convert.ToInt32(e.OldValue);

                            }
                            else if (itemDepletion.StockAmount > 0 && itemDepletion.AmountTaken > intAmountRemain)
                            {
                                Application.ShowViewStrategy.ShowMessage("Please enter the amount to be taken within the remaining amount.", InformationType.Error, 3000, InformationPosition.Top);
                                itemDepletion.AmountTaken = Convert.ToInt32(e.OldValue);
                                return;
                                //itemDepletion.AmountTaken = Convert.ToInt32(e.OldValue);
                            }
                            if (itemDepletion.Distribution != null && itemDepletion.Distribution.Item != null)
                            {
                                if (itemDepletion.Distribution.itemDepletionsCollection.Count == 1)
                                {
                                    itemDepletion.AmountRemain = itemDepletion.StockAmount - itemDepletion.AmountTaken;
                                }
                                else
                                {
                                    int amt = itemDepletion.Distribution.itemDepletionsCollection.Where(i => i.Oid != itemDepletion.Oid).Min(i => i.AmountRemain);
                                    itemDepletion.AmountRemain = amt - itemDepletion.AmountTaken;
                                }
                            }
                        }

                    }
                    if (e.PropertyName == "AmountRemain")
                    {
                        ItemDepletion itemDepletion = (ItemDepletion)e.Object;
                        if (itemDepletion.AmountRemain == 0)
                        {
                            Distribution objDistribution = (Distribution)Application.MainWindow.View.CurrentObject;
                            objDistribution.IsDeplete = true;
                            //IObjectSpace space = Application.CreateObjectSpace(typeof(Distribution));
                            //Distribution objD = space.GetObject<Distribution>(itemDepletion.Distribution);
                            //objD.IsDeplete = true;
                            //space.CommitChanges();
                            //space.Refresh();
                        }
                        else
                        if (itemDepletion.AmountRemain > 0)
                        {
                            Distribution objDistribution = (Distribution)Application.MainWindow.View.CurrentObject;
                            objDistribution.IsDeplete = false;
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

        private void NewAction_Executing(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Distribution_itemDepletionsCollection_ListView")
                {

                    IObjectSpace objectSpace = ((ListView)View).CollectionSource.ObjectSpace;
                    //IObjectSpace objectSpace = Application.MainWindow.View.ObjectSpace;

                    if (Application.MainWindow.View.Id == "Distribution_DetailView")
                    {
                        Distribution objdistribution = Application.MainWindow.View.CurrentObject as Distribution;
                        objdistribution = objectSpace.GetObject(objdistribution);
                        int amt = 0;
                        int sort = objdistribution.itemDepletionsCollection.Count;
                        if (sort > 0)
                        {
                            amt = objdistribution.itemDepletionsCollection.Min(i => i.AmountRemain);
                        }
                        else
                        {
                            amt = Convert.ToInt32(objdistribution.Item.Amount);
                        }
                        if (amt > 0)
                        {
                            ItemDepletion objdepletion = objectSpace.CreateObject<ItemDepletion>();
                            //objdistribution.itemDepletionsCollection.Add(objdepletion);

                            objdepletion.Distribution = objdistribution;
                            objdepletion.StockAmount = amt;
                            objdepletion.AmountTaken = 1;
                            objdepletion.Sort = sort + 1;
                            if (objdistribution.Item != null)
                            {
                                objdepletion.AmountUnits = objdistribution.Item.AmountUnit;
                                objdepletion.AmountRemain = amt - objdepletion.AmountTaken;
                            }
                        //Frame.Application.MainWindow.View.Refresh();
                        //((ListView)View).CollectionSource.ObjectSpace.Refresh();

                        ((ListView)View).CollectionSource.Add(objdepletion);
                            ((ListView)View).Refresh();
                            // ((ListView)View).ObjectSpace.CommitChanges();

                            //Application.MainWindow.View.ObjectSpace.Refresh();
                            //Frame.Application.MainWindow.View.Refresh(); 
                        }
                        else
                        {
                            IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                            Distribution objDistribution = Application.MainWindow.View.CurrentObject as Distribution;
                            //objDistribution.IsDeplete = true;
                            objDistribution.DateDepleted = DateTime.Now;
                            objDistribution.DepletedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            os.CommitChanges();
                            os.Refresh();
                            Application.ShowViewStrategy.ShowMessage("Depleted item cannot be consumed.", InformationType.Info, timer.Seconds, InformationPosition.Top);
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


        private void ListViewProcessCurrent_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {

                if (View != null && View.Id == "Distribution_ListView_Fractional_Consumption")
                {
                    ObjectSpace.CommitChanges();
                    IObjectSpace iobjectspace = Application.CreateObjectSpace();
                    //Distribution distribution = iobjectspace.FindObject<Distribution>(CriteriaOperator.Parse("[Item.StockQty] = ?", ICMInfo.StockQty));
                    Distribution objdi = e.InnerArgs.CurrentObject as Distribution;
                    Distribution objdis = iobjectspace.GetObject<Distribution>(objdi);
                    DetailView detail = Application.CreateDetailView(iobjectspace, objdis);
                    detail.ViewEditMode = ViewEditMode.Edit;
                    Frame.SetView(detail);
                    e.Handled = true;
                }

                if (View != null && View.Id == "Distribution_itemDepletionsCollection_ListView")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    ItemDepletion objitem = e.InnerArgs.CurrentObject as ItemDepletion;
                    ItemDepletion itemDepletion = objspace.GetObject<ItemDepletion>(objitem);
                    DetailView detial = Application.CreateDetailView(objspace, itemDepletion);
                    detial.ViewEditMode = ViewEditMode.Edit;
                    Frame.SetView(detial);
                    e.Handled = true;
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
                base.OnDeactivated();
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                strLT = string.Empty;
                ObjectSpace.Committed -= ObjectSpace_Committed;
                if (View.Id == "Distribution_ListView_Fractional_Consumption")
                {
                    FilterController filterController = Frame.GetController<FilterController>();
                    filterController.FullTextFilterAction.Executing += FullTextFilterAction_Executing;

                }
                if (View.Id == "Distribution_DetailView" || View.Id == "Distribution_itemDepletionsCollection_ListView")
                {
                    ModificationsController objmodifycontroller = Frame.GetController<ModificationsController>();
                    objmodifycontroller.SaveAction.Executing -= SaveAction_Executing;
                }
                if (View.Id == "Distribution_itemDepletionsCollection_ListView")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                if (View != null && View.Id == "Distribution_ListView_Item_Depletion")
                {
                    ListViewProcessCurrentObjectController listViewProcessCurrent = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listViewProcessCurrent != null)
                    {
                        listViewProcessCurrent.CustomProcessSelectedItem -= ListViewProcessCurrent_CustomProcessSelectedItem;
                        DeleteObjectsViewController objDeleteController = Frame.GetController<DeleteObjectsViewController>();
                        if (objDeleteController != null && objDeleteController.DeleteAction != null)
                        {
                            objDeleteController.DeleteAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
                            objDeleteController.DeleteAction.Category = "Edit";
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
            base.OnViewControlsCreated();
            try
            {
                if (base.View != null && base.View.Id == "Distribution_ListView")
                {
                    //if ((objdis.DistributionFilter == string.Empty || objdis.DistributionFilter == "Nothing") && ICMInfo.ObjectsToShow.Count == 0)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                    //    //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReceiveID] IS NULL");
                    //}
                    ICallbackManagerHolder handlerid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    handlerid.CallbackManager.RegisterHandler("distributeHandler", this);
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                    if (user != null)
                    {
                        gridListEditor.Grid.JSProperties["cpusername"] = user.DisplayName;
                    }
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.RowValidating += Grid_RowValidating;
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {                                                                               
                    if(e.focusedColumn.fieldName == 'ExpiryDate' || e.focusedColumn.fieldName == 'Comment' || e.focusedColumn.fieldName == 'DistributionDate' || e.focusedColumn.fieldName == 'GivenBy.Oid' || e.focusedColumn.fieldName == 'givento.Oid' || e.focusedColumn.fieldName == 'Storage.Oid' || e.focusedColumn.fieldName == 'VendorLT'){
                    e.cancel = false;                  
                    }
                    else{
                    e.cancel = true;                  
                    }                                         
                    }";//RaiseXafCallback(globalCallbackControl, 'salesHandler', 'Qty'+'|'+oid+'|'+val, '', false);
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) 
                    {
                        if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) 
                        {
                            for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                            {
                                s.batchEditApi.SetCellValue(i, 'DistributionDate', null);
                                s.batchEditApi.SetCellValue(i, 'GivenBy', null);
                                s.batchEditApi.SetCellValue(i, 'DistributedBy', null);
                                s.batchEditApi.SetCellValue(i, 'givento', null);
                                s.batchEditApi.SetCellValue(i, 'Storage', null);
                                s.batchEditApi.SetCellValue(i, 'VendorLT', null);
                                s.batchEditApi.SetCellValue(i, 'ExpiryDate', null);
                                s.batchEditApi.SetCellValue(i, 'Comment', null);
                            }
                            RaiseXafCallback(globalCallbackControl, 'distributeHandler', 'UNSelectAll', '', false);
                        }
                        else if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0))
                        {                       
                            var today = new Date();  
                            for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                            {                                        
                                s.batchEditApi.SetCellValue(i, 'DistributionDate', today);
                                s.batchEditApi.SetCellValue(i, 'DistributedBy',s.cpuserid, s.cpusername, false);
                                s.batchEditApi.SetCellValue(i, 'GivenBy',s.cpuserid, s.cpusername, false);
                            }
                            RaiseXafCallback(globalCallbackControl, 'distributeHandler', 'SelectAll', '', false);
                        }
                        else
                        {                   
                            //if (s.IsRowSelectedOnPage(e.visibleIndex)) 
                            //{                      
                            //    var today = new Date();                    
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributionDate', today);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributedBy',s.cpuserid, s.cpusername, false);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'GivenBy',s.cpuserid, s.cpusername, false);
                            //    RaiseXafCallback(globalCallbackControl, 'distributeHandler', 'Selected|' + Oidvalue , '', false);
                            //}
                            //else
                            //{
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributionDate', null);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributedBy', null);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'GivenBy', null);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'VendorLT', null);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpiryDate', null);
                            //    s.batchEditApi.SetCellValue(e.visibleIndex, 'Comment', null);
                            //    RaiseXafCallback(globalCallbackControl, 'distributeHandler', 'UNSelected|' + Oidvalue, '', false);
                            //}
                            s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) 
                            {  
                                if (s.IsRowSelectedOnPage(e.visibleIndex)) 
                                {
                                    var today = new Date();                    
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributionDate', today);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributedBy',s.cpuserid, s.cpusername, false);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'GivenBy',s.cpuserid, s.cpusername, false);
                                    RaiseXafCallback(globalCallbackControl, 'distributeHandler', 'Selected|' + Oidvalue , '', false);    
                                }
                                else
                                {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributionDate', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributedBy', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'GivenBy', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'VendorLT', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpiryDate', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Comment', null);
                                    RaiseXafCallback(globalCallbackControl, 'distributeHandler', 'UNSelected|' + Oidvalue, '', false);    
                                }
                            });
                        }
                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                    window.setTimeout(function() {   
                    var Storage = s.batchEditApi.GetCellValue(e.visibleIndex, 'Storage');
                    var givento = s.batchEditApi.GetCellValue(e.visibleIndex, 'givento');                    
                    if(sessionStorage.getItem('selecttype' + e.visibleIndex) == null){                    
                    if(Storage != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'Storage');                                                  
                    }
                    else if(givento != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'givento');                                                  
                    }
                    } else{
                    var select = sessionStorage.getItem('selecttype' + e.visibleIndex);
                    if(select == 'givento' && Storage != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'Storage');                                                  
                    }
                    else if(select == 'Storage' && givento != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'givento');                                                  
                    } 
                    }                                     
                    }, 20);}";
                    sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    sheet.CallbackManager.RegisterHandler("openspreadsheet", this);
                }
                else if (View != null && View.Id == "Distribution_ListView_Viewmode_Copy") // || View.Id == "Distribution_ListView_Viewmode" )
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {                              
                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'Consumed' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'Disposed'){
                    var Rowdataexp = s.batchEditApi.GetCellValue(e.visibleIndex, 'ExpiryDate');
                    if (sessionStorage.getItem('Rowexp' + e.visibleIndex) == null){  
                    sessionStorage.setItem('Rowexp' + e.visibleIndex, Rowdataexp);       
                    }     
                    if(e.focusedColumn.fieldName != 'ExpiryDate' || e.focusedColumn.fieldName != 'Comment')
                    {
                    e.cancel = true;                  
                    }}
                    else{
                    var Rowdata = s.batchEditApi.GetCellValue(e.visibleIndex, 'DistributionDate') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'GivenBy') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'givento') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'Storage') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'VendorLT') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'ExpiryDate') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'DistributedBy');
                    if (sessionStorage.getItem('Row' + e.visibleIndex) == null){  
                    sessionStorage.setItem('Row' + e.visibleIndex, Rowdata);       
                    }                     
                    if(e.focusedColumn.fieldName == 'ExpiryDate' || e.focusedColumn.fieldName == 'Comment' || e.focusedColumn.fieldName == 'DistributionDate' || e.focusedColumn.fieldName == 'GivenBy.Oid' || e.focusedColumn.fieldName == 'givento.Oid' || e.focusedColumn.fieldName == 'Storage.Oid' || e.focusedColumn.fieldName == 'VendorLT'){
                    e.cancel = false;                  
                    }
                    else{
                    e.cancel = true;                  
                    }}                  
                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {                    
                    s.SelectRowOnPage(e.visibleIndex);                     
                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') != 'Consumed'){
                    window.setTimeout(function() {                      
                    var Storage = s.batchEditApi.GetCellValue(e.visibleIndex, 'Storage');
                    var givento = s.batchEditApi.GetCellValue(e.visibleIndex, 'givento');                    
                    if(sessionStorage.getItem('selecttype' + e.visibleIndex) == null){                    
                    if(Storage != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'Storage');                                                  
                    }
                    else if(givento != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'givento');                                                  
                    }
                    } else {
                    var select = sessionStorage.getItem('selecttype' + e.visibleIndex);
                    if(select == 'givento' && Storage != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'Storage');                                                  
                    }
                    else if(select == 'Storage' && givento != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'givento');                                                  
                    } 
                    }                                     
                    }, 20);}}";
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) {                 
                    if (!s.IsRowSelectedOnPage(e.visibleIndex)) {
                    if(sessionStorage.getItem('Row' + e.visibleIndex) != null){
                    var Rowdata = sessionStorage.getItem('Row' + e.visibleIndex);
                    if(Rowdata.includes('#')){
                    var Rowdataspl = Rowdata.split('#');
                    var ddate = new Date(Rowdataspl[0]);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributionDate', ddate);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'GivenBy', Rowdataspl[1]);
                    if(Rowdataspl[2] != 'null'){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', Rowdataspl[2]);
                    }
                    else{
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    }
                    if(Rowdataspl[3] != 'null'){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', Rowdataspl[3]);
                    }
                    else{
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    }
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'VendorLT', Rowdataspl[4]);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributedBy', Rowdataspl[6]);
                    var edate = new Date(Rowdataspl[5]);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpiryDate', edate);
                    }
                    else{
                    var edate = new Date(Rowdata);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpiryDate', edate);
                    }
                    }}
                    }";
                }
                else if (View != null && View.Id == "Distribution_ListView_Receiveid")
                {
                    if (objdis.rgMode == ENMode.View.ToString())
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status] <> 0");
                            //lstview.Criteria = CriteriaOperator.Parse("[Status] <> 'PendingDistribute'");
                            lstview.Properties.Add(new ViewProperty("TReceiveID", SortDirection.Ascending, "ReceiveID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                        }
                    }
                    else
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                        {
                            //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 0");
                            lstview.Properties.Add(new ViewProperty("TReceiveID", SortDirection.Ascending, "ReceiveID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                        }
                    }

                }
                else if (base.View != null && base.View.Id == "Distribution_ListView_Vendor")
                {
                    if (objdis.rgMode == ENMode.View.ToString())
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                        {
                            //lstview.Criteria = CriteriaOperator.Parse("[Status] <> 'PendingDistribute'");
                            lstview.Criteria = CriteriaOperator.Parse("[Status] <> 0");
                            lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                        }
                    }
                    else
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                        {
                            //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 0");
                            lstview.Properties.Add(new ViewProperty("TVendor", SortDirection.Ascending, "Vendor", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                        }
                    }
                }
                else if (base.View != null && base.View.Id == "Distribution_ListView_Item")
                {
                    if (objdis.rgMode == ENMode.View.ToString())
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status] <> 0");
                            lstview.Properties.Add(new ViewProperty("TItem", SortDirection.Ascending, "Item", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                        }
                    }
                    else
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 0");
                            //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                            lstview.Properties.Add(new ViewProperty("TItem", SortDirection.Ascending, "Item", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                        }
                    }
                }
                else if (base.View != null && base.View.Id == "Distribution_ListView_MainDistribute" && objdis.DistributionFilter == string.Empty)
                {
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[Status] = 0");
                        //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                        lstview.Properties.Add(new ViewProperty("TReceiveID", SortDirection.Ascending, "ReceiveID", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                    }
                    //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                    //ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //ASPxGridView gv = gridListEditor.Grid;
                    //gridListEditor.Grid.Load += Grid_Load;
                    //gridListEditor.Grid.ClientSideEvents.Init = @"function(s, e) {
                    //        window.setTimeout(function() { s.Refresh(); }, 20); }";
                }
                else if (View != null && View.Id == "Distribution_ListView_Viewmode") // || View.Id == "Distribution_ListView_Viewmode" )
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {                              
                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'Consumed' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'Disposed'){
                    var Rowdataexp = s.batchEditApi.GetCellValue(e.visibleIndex, 'ExpiryDate');
                    if (sessionStorage.getItem('Rowexp' + e.visibleIndex) == null){  
                    sessionStorage.setItem('Rowexp' + e.visibleIndex, Rowdataexp);       
                    }                        
                    if(e.focusedColumn.fieldName != 'ExpiryDate' || e.focusedColumn.fieldName != 'Comment')
                    {
                    e.cancel = true;                  
                    }}
                    else{
                    var Rowdata = s.batchEditApi.GetCellValue(e.visibleIndex, 'DistributionDate') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'GivenBy') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'givento') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'Storage') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'VendorLT') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'ExpiryDate') + '#' + s.batchEditApi.GetCellValue(e.visibleIndex, 'DistributedBy');
                    if (sessionStorage.getItem('Row' + e.visibleIndex) == null){  
                    sessionStorage.setItem('Row' + e.visibleIndex, Rowdata);       
                    }                     
                    if(e.focusedColumn.fieldName == 'ExpiryDate' || e.focusedColumn.fieldName == 'Comment' || e.focusedColumn.fieldName == 'DistributionDate' || e.focusedColumn.fieldName == 'GivenBy.Oid' || e.focusedColumn.fieldName == 'givento.Oid' || e.focusedColumn.fieldName == 'Storage.Oid' || e.focusedColumn.fieldName == 'VendorLT'){
                    e.cancel = false;                  
                    }
                    else{
                    e.cancel = true;                  
                    }}                  
                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {                    
                    s.SelectRowOnPage(e.visibleIndex);                     
                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') != 'Consumed'){
                    window.setTimeout(function() {                      
                    var Storage = s.batchEditApi.GetCellValue(e.visibleIndex, 'Storage');
                    var givento = s.batchEditApi.GetCellValue(e.visibleIndex, 'givento');                    
                    if(sessionStorage.getItem('selecttype' + e.visibleIndex) == null){                    
                    if(Storage != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'Storage');                                                  
                    }
                    else if(givento != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'givento');                                                  
                    }
                    } else {
                    var select = sessionStorage.getItem('selecttype' + e.visibleIndex);
                    if(select == 'givento' && Storage != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'Storage');                                                  
                    }
                    else if(select == 'Storage' && givento != null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    sessionStorage.setItem('selecttype' + e.visibleIndex, 'givento');                                                  
                    } 
                    }                                     
                    }, 20);}}";
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) {                 
                    if (!s.IsRowSelectedOnPage(e.visibleIndex)) {
                    if(sessionStorage.getItem('Row' + e.visibleIndex) != null){
                    var Rowdata = sessionStorage.getItem('Row' + e.visibleIndex);
                    if(Rowdata.includes('#')){
                    var Rowdataspl = Rowdata.split('#');
                    var ddate = new Date(Rowdataspl[0]);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributionDate', ddate);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'GivenBy', Rowdataspl[1]);
                    if(Rowdataspl[2] != 'null'){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', Rowdataspl[2]);
                    }
                    else{
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'givento', null);
                    }
                    if(Rowdataspl[3] != 'null'){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', Rowdataspl[3]);
                    }
                    else{
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Storage', null);
                    }
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'VendorLT', Rowdataspl[4]);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DistributedBy', Rowdataspl[6]);
                    var edate = new Date(Rowdataspl[5]);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpiryDate', edate);
                    }
                    else{
                    var edate = new Date(Rowdata);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ExpiryDate', edate);
                    }
                    }}
                    }";
                }

                if (Application.MainWindow.View.Id == "Distribution_DetailView")
                {
                    if (View != null && View.Id == "Distribution_itemDepletionsCollection_ListView")
                    {
                        ASPxGridListEditor objAspxgridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                        ICallbackManagerHolder callbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        callbackManager.CallbackManager.RegisterHandler("DepeltionHandler", this);
                        if (objAspxgridlisteditor != null)
                        {
                            ASPxGridView objGridView = objAspxgridlisteditor.Grid;
                            //objGridView.Columns["AmountTaken"].Visible=true;
                            //objGridView.Columns["AmountTaken"].VisibleIndex = 6;


                            objGridView.ClientSideEvents.BatchEditEndEditing = @"function(s,e){
                    var StockAmt = s.GetColumnByField('StockAmount');
                    var AmtTaken = s.GetColumnByField('AmountTaken');
                    var stockamtvalue = s.batchEditApi.GetCellValue(e.visibleIndex, '_StockAmount');
                    var AmtTakensvalue = s.batchEditApi.GetCellValue(e.visibleIndex, '_AmountTaken');

               
                       stockamtvalue = e.rowValues[StockAmt.index].value;
                       AmtTakensvalue = e.rowValues[AmtTaken.index].value;
                        
                  
                    var newTotalColumnValue = stockamtvalue - AmtTakensvalue ;
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'AmountRemain', newTotalColumnValue);
                    RaiseXafCallback(globalCallbackControl, 'DepeltionHandler',newTotalColumnValue, '', false)
                    ////RaiseXafCallback(globalCallbackControl, 'DepeltionHandler',AmtTakensvalue,'',false)
                    //e.cancel=false;
                }";
                            objGridView.HtmlCommandCellPrepared += Gv_HtmlCommandCellPrepared;
                            //objGridView.HtmlDataCellPrepared += Gv_HtmlDataCellPrepared;


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

        private void Gv_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
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

        private void Gv_HtmlCommandCellPrepared(object sender, ASPxGridViewTableCommandCellEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (e.CommandCellType == GridViewTableCommandCellType.Data)
                {
                    if (e.CommandColumn.Name == "InlineEditCommandColumn" || e.CommandColumn.Name == "Delete")
                    {
                        ItemDepletion objLastDepletion = ((ListView)View).CollectionSource.List.Cast<ItemDepletion>().ToList().OrderByDescending(i => i.CreatedDate).FirstOrDefault();
                        Guid oid = new Guid(gridView.GetRowValuesByKeyValue(e.KeyValue, "Oid").ToString());
                        if (objLastDepletion.Oid != oid)
                        {
                            ////e.Cell.Controls[0].Visible = false;
                            ((System.Web.UI.WebControls.WebControl)e.Cell.Controls[0]).Enabled = false;
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

        private void Grid_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            try
            {
                //if (e.NewValues["VendorLT"] == null && e.NewValues["ExpiryDate"] == null)
                //{
                //    e.RowError = "VendorLT and ExpiryDate Should not be empty";
                //}
                //else if (e.NewValues["VendorLT"] == null)
                //{
                //    e.RowError = "VendorLT Should not be empty";
                //}
                //else if (e.NewValues["ExpiryDate"] == null)
                //{
                //    e.RowError = "ExpiryDate Should not be empty";
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

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

        #region Events
        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (View.Id == "Distribution_ListView_MainDistribute")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    Distribution obj = (Distribution)e.InnerArgs.CurrentObject;
                    CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                    //cs.Criteria["Filter"] = CriteriaOperator.Parse("[ReceiveID] = ? And [Status] = 'PendingDistribute'", obj.ReceiveID);
                    cs.Criteria["Filter"] = CriteriaOperator.Parse("[ReceiveID] = ? And [Status] = 0", obj.ReceiveID);
                    objdis.rgMode = ENMode.Enter.ToString();
                    objdis.DistributionFilter = "Nothing";
                    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateListView("Distribution_ListView", cs, false);
                    e.Handled = true;
                }
                else if (View.Id == "Distribution_ListView_Fractional_Consumption_History")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    Distribution obj = (Distribution)e.InnerArgs.CurrentObject;
                    Distribution objDis = objspace.GetObject<Distribution>(obj);
                    DetailView CreatedView = Application.CreateDetailView(objspace, "Distribution_DetailView_History", false, objDis);
                    CreatedView.Caption = "Fractional Consumption - History";
                    CreatedView.ViewEditMode = ViewEditMode.Edit;
                    Frame.SetView(CreatedView);
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void TxtBarcodeActionDistribution_CustomizeControl(object sender, CustomizeControlEventArgs e)
        {
            //ParametrizedActionMenuActi  onItem actionItem = e.Control as ParametrizedActionMenuActionItem;
            //if (actionItem != null)
            //{
            //    ASPxButtonEdit Editor = actionItem.Control.Editor as ASPxButtonEdit;
            //    if (Editor != null)
            //    {
            //        Editor.Buttons[0].Visible = false;
            //        Editor.Buttons[1].Visible = false;
            //    }
            //}
        }
        private void TxtBarcodeActionDistribution_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcodeActionDistribution.Value != null && txtBarcodeActionDistribution.Value.ToString() != string.Empty)
                {
                    string TextValue = txtBarcodeActionDistribution.Value.ToString();
                    Distribution objDistribution = ObjectSpace.FindObject<Distribution>(CriteriaOperator.Parse("[LT]= ?", TextValue));

                    if (objDistribution != null)
                    {
                        if (objDistribution.DistributionDate != null && objDistribution.ConsumptionDate == null)
                        {
                            if (!ICMInfo.ObjectsToShow.Contains(new Guid(objDistribution.Oid.ToString())))
                                ICMInfo.ObjectsToShow.Add(new Guid(objDistribution.Oid.ToString()));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "LTNot"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        if (View is ListView)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", ICMInfo.ObjectsToShow);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "LTNot"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", ICMInfo.ObjectsToShow);
                    }
                }
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
                if (base.View != null && base.View.Id == "Distributionquerypanel_DetailView")
                {
                    if (View != null && View.CurrentObject == e.Object && e.PropertyName == "RCID")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Distributionquerypanel))
                        {
                            Distributionquerypanel DPanel = (Distributionquerypanel)e.Object;
                            if (DPanel.RCID != null)
                            {
                                if (DPanel.Mode.Equals(ENMode.View))
                                {
                                    if (objdis.DistributionFilter != string.Empty)
                                    {
                                        objdis.DistributionFilter = objdis.DistributionFilter + "AND [ReceiveID] == '" + DPanel.RCID.ReceiveID + "'";
                                    }
                                    else
                                    {
                                        objdis.DistributionFilter = "[ReceiveID] == '" + DPanel.RCID.ReceiveID + "'";
                                    }
                                }
                                else
                                {
                                    if (objdis.DistributionFilter != string.Empty)
                                    {
                                        objdis.DistributionFilter = objdis.DistributionFilter + "AND [ReceiveID] == '" + DPanel.RCID.ReceiveID + "'";
                                    }
                                    else
                                    {
                                        objdis.DistributionFilter = "[ReceiveID] == '" + DPanel.RCID.ReceiveID + "'";
                                    }
                                }
                                //objdis.sesitem = DPanel.RCID.Item;

                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Vendor")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Distributionquerypanel))
                        {
                            Distributionquerypanel DPanel = (Distributionquerypanel)e.Object;
                            if (DPanel.Vendor != null)
                            {
                                if (DPanel.Mode.Equals(ENMode.View))
                                {
                                    if (objdis.DistributionFilter != string.Empty)
                                    {
                                        objdis.DistributionFilter = objdis.DistributionFilter + "AND [Vendor.Vendor] == '" + DPanel.Vendor.Vendor.Vendor + "'";
                                    }
                                    else
                                    {
                                        objdis.DistributionFilter = "[Vendor.Vendor] == '" + DPanel.Vendor.Vendor.Vendor + "'";
                                    }
                                }
                                else
                                {
                                    if (objdis.DistributionFilter != string.Empty)
                                    {
                                        objdis.DistributionFilter = objdis.DistributionFilter + "AND [Vendor.Vendor] == '" + DPanel.Vendor.Vendor.Vendor + "'";
                                    }
                                    else
                                    {
                                        objdis.DistributionFilter = "[Vendor.Vendor] == '" + DPanel.Vendor.Vendor.Vendor + "'";
                                    }
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Item")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Distributionquerypanel))
                        {
                            Distributionquerypanel DPanel = (Distributionquerypanel)e.Object;
                            if (DPanel.Item != null)
                            {
                                if (DPanel.Mode.Equals(ENMode.View))
                                {
                                    if (objdis.DistributionFilter != string.Empty)
                                    {
                                        objdis.DistributionFilter = objdis.DistributionFilter + "AND [Item.items] == '" + DPanel.Item.Item.items + "'";
                                    }
                                    else
                                    {
                                        objdis.DistributionFilter = "[Item.items] == '" + DPanel.Item.Item.items + "'";
                                    }
                                }
                                else
                                {
                                    if (objdis.DistributionFilter != string.Empty)
                                    {
                                        objdis.DistributionFilter = objdis.DistributionFilter + "AND [Item.items] == '" + DPanel.Item.Item.items + "'";
                                    }
                                    else
                                    {
                                        objdis.DistributionFilter = "[Item.items] == '" + DPanel.Item.Item.items + "'";
                                    }
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Mode")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(Distributionquerypanel))
                        {
                            Distributionquerypanel DPanel = (Distributionquerypanel)e.Object;
                            if (DPanel != null)
                            {
                                objdis.rgMode = DPanel.Mode.ToString();
                            }
                            //if (DPanel.Mode.Equals(ENMode.View))
                            //{
                            //    objdis.rgMode = DPanel.Mode.ToString();// ENMode.View.ToString();
                            //}
                            //else 
                            //{
                            //    objdis.rgMode = ENMode.Enter.ToString();
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
        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                //if (View.Id == "Distribution_ListView_MainDistribute")
                //{
                //    ASPxGridView gridView = sender as ASPxGridView;
                //    var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                //    selectionBoxColumn.Visible = false;
                //}
                //else
                //{
                //    ASPxGridView gridView = sender as ASPxGridView;
                //    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                //    var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                //    selectionBoxColumn.SelectCheckBoxPosition = GridSelectCheckBoxPosition.Left;
                //    selectionBoxColumn.FixedStyle = GridViewColumnFixedStyle.Left;
                //    selectionBoxColumn.VisibleIndex = 0;
                //    //selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;                   
                //    if (objdis.rgMode == ENMode.View.ToString())
                //    {
                //    gridView.Columns["LT"].Visible = true;
                //    gridView.Columns["ESID"].Visible = true;
                //    gridView.Columns["LT"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["LT"].VisibleIndex = 1;
                //    gridView.Columns["Vendor"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["Vendor"].VisibleIndex = 2;
                //    gridView.Columns["Item"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["Item"].VisibleIndex = 3;
                //    gridView.Columns["ReceiveID"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["ReceiveID"].VisibleIndex = 4;
                //    gridView.Columns["OrderQty"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    gridView.Columns["OrderQty"].VisibleIndex = 5;
                //}
                //else if (objdis.rgMode == ENMode.Enter.ToString())
                //{
                //    //gridView.Columns["LT"].Visible = false;
                //    //gridView.Columns["ESID"].Visible = false;
                //    //gridView.Columns["Vendor"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    //gridView.Columns["Vendor"].VisibleIndex = 1;
                //    //gridView.Columns["Item"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    //gridView.Columns["Item"].VisibleIndex = 2;
                //    //gridView.Columns["ReceiveID"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    //gridView.Columns["ReceiveID"].VisibleIndex = 3;
                //    //gridView.Columns["OrderQty"].FixedStyle = GridViewColumnFixedStyle.Left;
                //    //gridView.Columns["OrderQty"].VisibleIndex = 4;
                //}
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DistributionQueryPanel_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (ICMInfo.ObjectsToShow.Count > 0)
                {
                    ICMInfo.ObjectsToShow.Clear();
                }
                ((ListView)View).CollectionSource.Criteria.Clear();
                objdis.QueryFilter = true;
                int RowCount = 0;
                if (View != null && View.Id == "Distribution_ListView" || View.Id == "Distribution_ListView_MainDistribute")
                {
                    if (objdis.DistributionFilter == string.Empty)
                    {
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Emptygrid'");
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 6");
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReceiveID]==NULL");
                    }
                    else if (objdis.DistributionFilter != string.Empty)
                    {
                        if (objdis.rgMode == "View")
                        {
                            objdis.DistributionFilter = objdis.DistributionFilter + "AND [Status] <> 0";
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                            cs.Criteria["Filter"] = CriteriaOperator.Parse(objdis.DistributionFilter);
                            Frame.SetView(Application.CreateListView("Distribution_ListView", cs, false));
                        }
                        else
                        {
                            if (View.Id == "Distribution_ListView_MainDistribute")
                            {
                                using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Distribution)))
                                {
                                    objdis.DistributionFilter = objdis.DistributionFilter + "AND [Status] = 0";
                                    lstview.Criteria = CriteriaOperator.Parse(objdis.DistributionFilter);
                                    lstview.Properties.Add(new ViewProperty("TReceiveID", SortDirection.Ascending, "ReceiveID", true, true));
                                    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                    List<object> groups = new List<object>();
                                    foreach (ViewRecord rec in lstview)
                                        groups.Add(rec["Toid"]);
                                    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                                }
                            }
                            else
                            {
                                objdis.DistributionFilter = objdis.DistributionFilter + "AND [Status] = 0";
                                ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse(objdis.DistributionFilter);
                            }
                            WebWindow.CurrentRequestWindow.RegisterClientScript("user", "sessionStorage.setItem('curuser', '" + SecuritySystem.CurrentUserId + "');");
                            WebWindow.CurrentRequestWindow.RegisterClientScript("user1", "sessionStorage.setItem('curuser1', '" + SecuritySystem.CurrentUserName + "');");
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
        private void DistributionQueryPanel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(Distributionquerypanel));
                if (objToShow != null)
                {
                    objdis.DistributionFilter = string.Empty;
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
        private void Distribute_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count > 0)
                {
                    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                    List<Distribution> lstDistributions = View.SelectedObjects.Cast<Distribution>().ToList();
                    if (lstDistributions.FirstOrDefault(i => i.Item.IsVendorLT && string.IsNullOrEmpty(i.VendorLT)) != null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "emptyvendorlt"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    else if (lstDistributions.FirstOrDefault(i => i.Storage == null && i.givento == null) != null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectgivento"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    else if (lstDistributions.FirstOrDefault(i => i.ExpiryDate == null) != null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "expirydate"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        return;
                    }

                    //foreach (Distribution obj in View.SelectedObjects)
                    //{
                    //    if (obj.Item.IsVendorLT == true)
                    //    {
                    //        if (obj.VendorLT == null)
                    //        {
                    //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "emptyvendorlt"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //            return;
                    //        }
                    //    }
                    //    if (obj.Storage == null && obj.Givento == null)
                    //    {
                    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectgivento"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //        return;
                    //    }
                    //    if(obj.ExpiryDate == null)
                    //    {
                    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "expirydate"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    //        return;
                    //    }
                    //}
                    List<Tuple<string, string, string, bool>> lstLt = new List<Tuple<string, string, string, bool>>();
                    IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Distribution));
                    string strLTNo = string.Empty;
                    string strLTNowithVendorLT = string.Empty;
                    foreach (Distribution obj in lstDistributions)
                    {
                        //if (objdis.rgMode == ENMode.Enter.ToString())
                        if (string.IsNullOrEmpty(obj.LT))
                        {
                            Tuple<string, string, string, bool> tupDistribution;
                            string templt = string.Empty;
                            if (obj.Item != null)
                            {
                                string curdate = DateTime.Now.ToString("yy");
                                if ((obj.Item.IsVendorLT && lstLt.FirstOrDefault(i => i.Item1 == obj.Item.ItemCode && i.Item4 == obj.Item.IsVendorLT && i.Item3 == obj.VendorLT) == null) ||
                                    (!obj.Item.IsVendorLT && lstLt.FirstOrDefault(i => i.Item1 == obj.Item.ItemCode && i.Item4 == false) == null))
                                {
                                    bool HasSequence = false;
                                    if ((obj.Item.IsVendorLT && lstDistributions.FirstOrDefault(i => i.Item.ItemCode == obj.Item.ItemCode && i.Item.IsVendorLT == true && i.VendorLT == obj.VendorLT && i.Oid != obj.Oid) != null) ||
                                        (!obj.Item.IsVendorLT && lstDistributions.FirstOrDefault(i => i.Item.ItemCode == obj.Item.ItemCode && i.Item.IsVendorLT == false && i.Oid != obj.Oid) != null))
                                    {
                                        HasSequence = true;
                                    }
                                    CriteriaOperator LTExpressionWithoutSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                                    CriteriaOperator LTCriteriaWithoutSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Not Contains([LT], '_')", "LT" + curdate);

                                    CriteriaOperator LTExpressionWithSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2, 6))");
                                    CriteriaOperator LTCriteriaWithSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Contains([LT], '_')", "LT" + curdate);

                                    int templtwithoutsequence = Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Distribution), LTExpressionWithoutSequence, LTCriteriaWithoutSequence)) + 1;
                                    int templtwithsequence = Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Distribution), LTExpressionWithSequence, LTCriteriaWithSequence)) + 1;
                                    if (templtwithoutsequence == 1 && templtwithsequence == 1)
                                    {
                                        if (!obj.Item.IsVendorLT && string.IsNullOrEmpty(strLTNo))
                                        {
                                            strLTNo = "LT" + curdate + "0001";
                                        }
                                        else
                                        if (!string.IsNullOrEmpty(obj.VendorLT) && string.IsNullOrEmpty(strLTNowithVendorLT))
                                        {
                                            strLTNowithVendorLT = "LT" + curdate + "0001";
                                        }
                                    }
                                    else if (templtwithoutsequence > templtwithsequence)
                                    {
                                        if (string.IsNullOrEmpty(obj.VendorLT))
                                        {
                                            strLTNo = "LT" + templtwithoutsequence;
                                        }
                                        else
                                        if (!string.IsNullOrEmpty(obj.VendorLT))
                                        {
                                            strLTNowithVendorLT = "LT" + templtwithoutsequence;
                                        }
                                    }
                                    else if (templtwithoutsequence < templtwithsequence)
                                    {

                                        if (string.IsNullOrEmpty(obj.VendorLT))
                                        {
                                            strLTNo = "LT" + templtwithsequence;
                                        }
                                        else
                                        if (!string.IsNullOrEmpty(obj.VendorLT)/* && string.IsNullOrEmpty(strLTNowithVendorLT)*/)
                                        {
                                            strLTNowithVendorLT = "LT" + templtwithsequence;
                                        }
                                    }
                                    else if (templtwithoutsequence == templtwithsequence)
                                    {

                                        if (string.IsNullOrEmpty(obj.VendorLT))
                                        {
                                            strLTNo = "LT" + templtwithsequence;
                                        }
                                        else
                                        if (!string.IsNullOrEmpty(obj.VendorLT)/* && string.IsNullOrEmpty(strLTNowithVendorLT)*/)
                                        {
                                            strLTNowithVendorLT = "LT" + templtwithsequence;
                                        }
                                    }

                                    if (!HasSequence)
                                    {
                                        if (string.IsNullOrEmpty(obj.VendorLT))
                                        {
                                            templt = strLTNo;
                                        }
                                        else
                                        {
                                            templt = strLTNowithVendorLT;
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(obj.VendorLT))
                                        {
                                            templt = strLTNo + "_01";
                                        }
                                        else
                                        {
                                            templt = strLTNowithVendorLT + "_01";
                                        }
                                    }
                                    tupDistribution = new Tuple<string, string, string, bool>(obj.Item.ItemCode, templt, obj.VendorLT, obj.Item.IsVendorLT);
                                    lstLt.Add(tupDistribution);
                                }
                                else
                                {
                                    int index = -1;
                                    if (!obj.Item.IsVendorLT)
                                    {
                                        index = lstLt.FindIndex(i => i.Item1 == obj.Item.ItemCode && i.Item4 == false);
                                    }
                                    else
                                    {
                                        index = lstLt.FindIndex(i => i.Item1 == obj.Item.ItemCode && i.Item3 == obj.VendorLT);
                                    }
                                    if (index >= 0)
                                    {
                                        string[] strPrevlt = lstLt[index].Item2.Split('_');
                                        if ((Convert.ToInt32(strPrevlt[1]) + 1) < 10)
                                        {
                                            templt = strPrevlt[0] + "_0" + (Convert.ToInt32(strPrevlt[1]) + 1);
                                        }
                                        else
                                        {
                                            templt = strPrevlt[0] + "_" + (Convert.ToInt32(strPrevlt[1]) + 1);
                                        }
                                        lstLt[index] = new Tuple<string, string, string, bool>(obj.Item.ItemCode, templt, obj.VendorLT, obj.Item.IsVendorLT);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(templt))
                            {

                                obj.LT = templt;
                                objectSpace1.CommitChanges();
                            }
                        }
                        if (!string.IsNullOrEmpty(obj.LT))
                        {
                            if (obj.givento != null)
                            {
                                if (obj.Item.StockQty > 0)
                                {
                                    obj.Item.StockQty = obj.Item.StockQty - 1;
                                }
                                obj.Status = Distribution.LTStatus.Consumed;
                                obj.ConsumptionBy = obj.givento;
                                obj.ConsumptionDate = DateTime.Now;
                                obj.EnteredBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                obj.EnteredDate = DateTime.Now;
                                obj.DistributedBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                obj.DistributionDate = DateTime.Now;
                                obj.GivenBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                ConsumptionHistory newobj = objspace.CreateObject<ConsumptionHistory>();
                                newobj.Distribution = objspace.GetObjectByKey<Distribution>(obj.Oid);
                                newobj.ConsumptionBy = objspace.GetObjectByKey<Employee>(obj.givento.Oid);
                                newobj.ConsumptionDate = obj.ConsumptionDate;
                                newobj.EnteredBy = objspace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                newobj.EnteredDate = obj.EnteredDate;
                                newobj.Consumed = true;
                                if (obj.Item.StockQty <= obj.Item.AlertQty)
                                {
                                    ICMAlert objdisp1 = objspace.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")"));
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
                                objspace.CommitChanges();
                            }
                            else
                            {
                                //obj.Item.StockQty += 1;
                                obj.DistributedBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                obj.DistributionDate = DateTime.Now;
                                obj.Status = Distribution.LTStatus.PendingConsume;
                            }
                            if (obj.ExpiryDate <= DateTime.Now.AddDays(7) && obj.ExpiryDate != null && obj.LT != null && obj.Status == Distribution.LTStatus.PendingConsume && obj.ExpiryDate != DateTime.MinValue.Date)
                            {
                                IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                ICMAlert objdisp = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + obj.LT));
                                if (objdisp == null)
                                {
                                    ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                    obj1.Subject = "Expiry Alert - " + obj.LT;
                                    obj1.StartDate = DateTime.Now;
                                    obj1.DueDate = DateTime.Now.AddDays(7);
                                    obj1.RemindIn = TimeSpan.FromMinutes(5);
                                    obj1.Description = "Nice";
                                    space.CommitChanges();
                                }
                                if (obj.ExpiryDate < DateTime.Today)
                                {
                                    obj.Status = Distribution.LTStatus.PendingDispose;
                                }
                            }
                            else
                            {
                                IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                IList<ICMAlert> alertlist = space.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Expiry Alert - " + obj.LT));
                                if (alertlist != null)
                                {
                                    foreach (ICMAlert item in alertlist)
                                    {
                                        item.AlarmTime = null;
                                        item.RemindIn = null;
                                    }
                                    space.CommitChanges();
                                }
                            }

                            ObjectSpace.CommitChanges();
                        }
                    }
                    foreach (Distribution obj in View.SelectedObjects)
                    {
                        if (obj.LT == null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "pendingdistribute"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            return;
                        }

                        if (strLT == string.Empty)
                        {
                            strLT = "'" + obj.Oid.ToString() + "'";
                        }
                        else
                        {
                            strLT = strLT + ",'" + obj.Oid.ToString() + "'";
                        }
                    }

                    #region Insert LT into ReagentVendorCertificate
                    if (View != null && View.Id == "Distribution_ListView" && e.SelectedObjects.Count > 0)
                    {
                        IObjectSpace objectspace = Application.CreateObjectSpace(typeof(VendorReagentCertificate));
                        foreach (Distribution obj in e.SelectedObjects)
                        {
                            if (obj.VendorLT != null)
                            {
                                Distribution DistObj = objectspace.GetObject<Distribution>(obj);
                                VendorReagentCertificate VRC = objectspace.FindObject<VendorReagentCertificate>(new BinaryOperator("VendorLT", obj.VendorLT));
                                if (VRC == null)
                                {
                                    VendorReagentCertificate VRCObject = (VendorReagentCertificate)objectspace.CreateObject(typeof(VendorReagentCertificate));
                                    //VRCObject.LT = DistObj.LT;
                                    VRCObject.VendorLT = DistObj.VendorLT;
                                    VRCObject.Vendor = DistObj.Vendor;
                                    VRCObject.Item = DistObj.Item;
                                    VRCObject.Catelog = DistObj.Item.VendorCatName;
                                    VRCObject.Requestor = DistObj.RequestedBy;
                                    Employee emp = (Employee)DistObj.RequestedBy;
                                    if (emp != null && emp.Department != null)
                                    {
                                        VRCObject.Department = emp.Department.Name;
                                    }
                                    //VRCObject.RQID = DistObj.RQID.RQID;
                                    VRCObject.POID = DistObj.POID;
                                    VRCObject.ReceiveID = DistObj.ReceiveID;
                                    DistObj.VendorReagentCertificate = objectspace.GetObject(VRCObject);
                                }
                                else
                                {
                                    DistObj.VendorReagentCertificate = objectspace.GetObject(VRC);
                                }
                            }
                        }
                        objectspace.CommitChanges();
                    }
                    #endregion
                    ObjReportingInfo.strLTOid = strLT;

                    WebWindow.CurrentRequestWindow.RegisterClientScript("Openspreadsheet", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('Do you want to preview the barcode labels ?'); {0}", sheet.CallbackManager.GetScript("openspreadsheet", "openconfirm")));


                    ////print report directly. combine this code - MKS
                    //string Print = ConfigurationManager.AppSettings["AllowPrint"];
                    //if (Print == "True")
                    //{
                    //    xtraReport.ShowPrintMarginsWarning = false;
                    //    ReportPrintTool printTool = new ReportPrintTool(xtraReport);
                    //    //printTool.AutoShowParametersPanel = false;
                    //    if (printTool.PrinterSettings.IsDefaultPrinter)
                    //    {
                    //        printTool.Print();
                    //    }
                    //}

                    ObjectSpace.Refresh();
                    View.Refresh();
                    NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                    module.ShowNotificationsWindow = false;
                    module.NotificationsService.Refresh();
                    // ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[LT] = " + strLT  );
                    strLT = string.Empty;
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
                                        else if (subchild.Id == "DisposalItems")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 3 OR [Status] = 1) And [ExpiryDate] < ?", DateTime.Today));
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
                                        else if (subchild.Id == "VendorReagentCertificate")
                                        {
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(VendorReagentCertificate), CriteriaOperator.Parse("[LoadedDate] IS NULL AND [LoadedBy] IS NULL"));
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
                                else if (child.Id == "Alert")
                                {
                                    foreach (ChoiceActionItem subchild in child.Items)
                                    {
                                        if (subchild.Id == "Expiration Alert")
                                        {
                                            DateTime TodayDate = DateTime.Now;
                                            TodayDate = TodayDate.AddDays(7);
                                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] == 3 OR [Status] == 1) And [ExpiryDate] <= ?", TodayDate));
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
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "distributesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                //if (View != null && View.SelectedObjects.Count > 0)
                //{
                //    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                //    foreach (Distribution obj in View.SelectedObjects)
                //    {
                //        if (obj.Item.IsVendorLT == true)
                //        {
                //            if (obj.VendorLT == null)
                //            {
                //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "emptyvendorlt"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //                return;
                //            }
                //        }
                //        if (obj.Storage == null && obj.givento == null)
                //        {
                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectgivento"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //            return;
                //        }
                //        if(obj.ExpiryDate == null)
                //        {
                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "expirydate"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //            return;
                //        }
                //    }
                //    foreach (Distribution obj in View.SelectedObjects)
                //    {
                //        //if (objdis.rgMode == ENMode.Enter.ToString())
                //        if (obj.LT == null)
                //        {
                //            string templt = string.Empty;
                //            string curdate = DateTime.Now.ToString("yy");
                //            IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(Distribution));
                //            CriteriaOperator LTExpressionWithoutSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                //            CriteriaOperator LTCriteriaWithoutSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Not Contains([LT], '_')", "LT" + curdate);

                //            CriteriaOperator LTExpressionWithSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2, 6))");
                //            CriteriaOperator LTCriteriaWithSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Contains([LT], '_')", "LT" + curdate);

                //            int templtwithoutsequence = Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Distribution), LTExpressionWithoutSequence, LTCriteriaWithoutSequence)) + 1;
                //            int templtwithsequence = Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Distribution), LTExpressionWithSequence, LTCriteriaWithSequence)) + 1;
                //            if (templtwithoutsequence == 1 && templtwithsequence == 1)
                //            {
                //                templt = "LT" + curdate + "0001";
                //            }
                //            else if (templtwithoutsequence > templtwithsequence)
                //            {
                //                templt = "LT" + templtwithoutsequence;

                //            }
                //            else if (templtwithoutsequence < templtwithsequence)
                //            {
                //                templt = "LT" + templtwithsequence;

                //            }
                //            //CriteriaOperator LTCriteria = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                //            //string templt = (Convert.ToInt32(((XPObjectSpace)objectSpace1).Session.Evaluate(typeof(Distribution), LTCriteria, null)) + 1).ToString();
                //            //var curdate = DateTime.Now.ToString("yy");
                //            //if (templt != "1")
                //            //{
                //            //    var predate = templt.Substring(0, 2);
                //            //    if (predate == curdate)
                //            //    {
                //            //        templt = "LT" + templt;
                //            //    }
                //            //    else
                //            //    {
                //            //        templt = "LT" + curdate + "0001";
                //            //    }
                //            //}
                //            //else
                //            //{
                //            //    templt = "LT" + curdate + "0001";
                //            //}
                //            obj.LT = templt;
                //        }
                //        if (obj.givento != null)
                //        {
                //            obj.Item.StockQty = obj.Item.StockQty - 1;
                //            obj.Status = Distribution.LTStatus.Consumed;
                //            obj.ConsumptionBy = obj.givento;
                //            obj.ConsumptionDate = DateTime.Now;
                //            obj.EnteredBy = ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                //            obj.EnteredDate = DateTime.Now;
                //            IObjectSpace objspace = Application.CreateObjectSpace();
                //            ConsumptionHistory newobj = objspace.CreateObject<ConsumptionHistory>();
                //            newobj.Distribution = objspace.GetObjectByKey<Distribution>(obj.Oid);
                //            newobj.ConsumptionBy = objspace.GetObjectByKey<Employee>(obj.givento.Oid);
                //            newobj.ConsumptionDate = obj.ConsumptionDate;
                //            newobj.EnteredBy = objspace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                //            newobj.EnteredDate = obj.EnteredDate;
                //            newobj.Consumed = true;
                //            if (obj.Item.StockQty <= obj.Item.AlertQty)
                //            {
                //                ICMAlert objdisp1 = objspace.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")"));
                //                if (objdisp1 == null)
                //                {
                //                    ICMAlert obj1 = objspace.CreateObject<ICMAlert>();
                //                    obj1.Subject = "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")";
                //                    obj1.StartDate = DateTime.Now;
                //                    obj1.DueDate = DateTime.Now.AddDays(7);
                //                    obj1.RemindIn = TimeSpan.FromMinutes(5);
                //                    obj1.Description = "Nice";
                //                }
                //            }
                //            objspace.CommitChanges();
                //        }
                //        else
                //        {
                //            obj.Status = Distribution.LTStatus.PendingConsume;
                //        }
                //        if (obj.ExpiryDate <= DateTime.Now.AddDays(7) && obj.ExpiryDate != null && obj.LT != null && obj.Status == Distribution.LTStatus.PendingConsume && obj.ExpiryDate != DateTime.MinValue.Date)
                //        {
                //            IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                //            ICMAlert objdisp = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + obj.LT));
                //            if (objdisp == null)
                //            {
                //                ICMAlert obj1 = space.CreateObject<ICMAlert>();
                //                obj1.Subject = "Expiry Alert - " + obj.LT;
                //                obj1.StartDate = DateTime.Now;
                //                obj1.DueDate = DateTime.Now.AddDays(7);
                //                obj1.RemindIn = TimeSpan.FromMinutes(5);
                //                obj1.Description = "Nice";
                //                space.CommitChanges();
                //            }
                //            if (obj.ExpiryDate <= DateTime.Now)
                //            {
                //                obj.Status = Distribution.LTStatus.PendingDispose;
                //            }
                //        }
                //        else
                //        {
                //            IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                //            IList<ICMAlert> alertlist = space.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Expiry Alert - " + obj.LT));
                //            if (alertlist != null)
                //            {
                //                foreach (ICMAlert item in alertlist)
                //                {
                //                    item.AlarmTime = null;
                //                    item.RemindIn = null;
                //                }
                //                space.CommitChanges();
                //            }
                //        }
                //        ObjectSpace.CommitChanges();
                //    }
                //    foreach (Distribution obj in View.SelectedObjects)
                //    {
                //        if (obj.LT == null)
                //        {
                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "pendingdistribute"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //            return;
                //        }

                //        if (strLT == string.Empty)
                //        {
                //            strLT = "'" + obj.LT.ToString() + "'";
                //        }
                //        else
                //        {
                //            strLT = strLT + ",'" + obj.LT.ToString() + "'";
                //        }
                //    }

                //    #region Insert LT into ReagentVendorCertificate
                //    if (View != null && View.Id == "Distribution_ListView" && e.SelectedObjects.Count > 0)
                //    {
                //        IObjectSpace objectspace = Application.CreateObjectSpace(typeof(VendorReagentCertificate));
                //        foreach (Distribution obj in e.SelectedObjects)
                //        {
                //            if (obj.VendorLT != null)
                //            {
                //                Distribution DistObj = objectspace.GetObject<Distribution>(obj);
                //                VendorReagentCertificate VRC = objectspace.FindObject<VendorReagentCertificate>(new BinaryOperator("VendorLT", obj.VendorLT));
                //                if (VRC == null)
                //                {
                //                    VendorReagentCertificate VRCObject = (VendorReagentCertificate)objectspace.CreateObject(typeof(VendorReagentCertificate));
                //                    //VRCObject.LT = DistObj.LT;
                //                    VRCObject.VendorLT = DistObj.VendorLT;
                //                    VRCObject.Vendor = DistObj.Vendor;
                //                    VRCObject.Item = DistObj.Item;
                //                    VRCObject.Catelog = DistObj.Item.VendorCatName;
                //                    VRCObject.Requestor = DistObj.RequestedBy;
                //                    Employee emp = (Employee)DistObj.RequestedBy;
                //                    if (emp != null && emp.Department != null)
                //                    {
                //                        VRCObject.Department = emp.Department.Name;
                //                    }
                //                    //VRCObject.RQID = DistObj.RQID.RQID;
                //                    VRCObject.POID = DistObj.POID;
                //                    VRCObject.ReceiveID = DistObj.ReceiveID;
                //                    DistObj.VendorReagentCertificate = objectspace.GetObject(VRCObject);                                  
                //                }
                //                else
                //                {
                //                    DistObj.VendorReagentCertificate = objectspace.GetObject(VRC);
                //                }
                //            }
                //        }
                //        objectspace.CommitChanges();
                //    }
                //    #endregion


                //    string strTempPath = Path.GetTempPath();
                //    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\")) == false)
                //    {
                //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\"));
                //    }
                //    string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\" + timeStamp + ".pdf");
                //    XtraReport xtraReport = new XtraReport();

                //    objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //    SetConnectionString();

                //    DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                //    //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                //    ObjReportingInfo.strLT = strLT;
                //    xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("LTBarcode", ObjReportingInfo, false);                   
                //    //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                //    xtraReport.ExportToPdf(strExportedPath);
                //    string[] path = strExportedPath.Split('\\');
                //    int arrcount = path.Count();
                //    int sc = arrcount - 3;
                //    string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));                   
                //    WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                //    ////print report directly. combine this code - MKS
                //    //string Print = ConfigurationManager.AppSettings["AllowPrint"];
                //    //if (Print == "True")
                //    //{
                //    //    xtraReport.ShowPrintMarginsWarning = false;
                //    //    ReportPrintTool printTool = new ReportPrintTool(xtraReport);
                //    //    //printTool.AutoShowParametersPanel = false;
                //    //    if (printTool.PrinterSettings.IsDefaultPrinter)
                //    //    {
                //    //        printTool.Print();
                //    //    }
                //    //}

                //    ObjectSpace.Refresh();
                //    View.Refresh();
                //    NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                //    module.ShowNotificationsWindow = false;
                //    module.NotificationsService.Refresh();                   
                //   // ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[LT] = " + strLT  );
                //    strLT = string.Empty;                  
                //    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                //    {
                //        if (parent.Id == "ICM")
                //        {
                //            foreach (ChoiceActionItem child in parent.Items)
                //            {
                //                if (child.Id == "Operations")
                //                {
                //                    foreach (ChoiceActionItem subchild in child.Items)
                //                    {
                //                        if (subchild.Id == "DistributionItem")
                //                        {
                //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                //                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[LT] Is Null"));
                //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                            int intReceive = 0;
                //                            CriteriaOperator criteria = CriteriaOperator.Parse("[LT] Is Null");
                //                            IList<Distribution> req = ObjectSpace.GetObjects<Distribution>(criteria);
                //                            string[] ReceiveID = new string[req.Count];
                //                            foreach (Distribution item in req)
                //                            {
                //                                if (!ReceiveID.Contains(item.ReceiveID))
                //                                {
                //                                    ReceiveID[intReceive] = item.ReceiveID;
                //                                    intReceive = intReceive + 1;
                //                                }
                //                            }
                //                            if (count > 0)
                //                            {
                //                                subchild.Caption = cap[0] + " (" + intReceive + ")";
                //                            }
                //                            else
                //                            {
                //                                subchild.Caption = cap[0];
                //                            }
                //                        }
                //                        else if (subchild.Id == "DisposalItems")
                //                        {
                //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                //                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 3 OR [Status] = 1) And [ExpiryDate] <= ?", DateTime.Now));
                //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                            if (count > 0)
                //                            {
                //                                subchild.Caption = cap[0] + " (" + count + ")";
                //                            }
                //                            else
                //                            {
                //                                subchild.Caption = cap[0];
                //                            }
                //                        }
                //                        else if (subchild.Id == "VendorReagentCertificate")
                //                        {
                //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                //                            var count = objectSpace.GetObjectsCount(typeof(VendorReagentCertificate), CriteriaOperator.Parse("[LoadedDate] IS NULL AND [LoadedBy] IS NULL"));
                //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                            if (count > 0)
                //                            {
                //                                subchild.Caption = cap[0] + " (" + count + ")";
                //                            }
                //                            else
                //                            {
                //                                subchild.Caption = cap[0];
                //                            }
                //                        }                                      
                //                    }
                //                }
                //                else if (child.Id == "Alert")
                //                {
                //                    foreach (ChoiceActionItem subchild in child.Items)
                //                    {
                //                        if (subchild.Id == "Expiration Alert")
                //                        {
                //                            DateTime TodayDate = DateTime.Now;
                //                            TodayDate = TodayDate.AddDays(7);
                //                            IObjectSpace objectSpace = Application.CreateObjectSpace();
                //                            var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] == 3 OR [Status] == 1) And [ExpiryDate] <= ?", TodayDate));
                //                            var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //                            if (count > 0)
                //                            {
                //                                subchild.Caption = cap[0] + " (" + count + ")";
                //                            }
                //                            else
                //                            {
                //                                subchild.Caption = cap[0];
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "distributesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //}
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "dataselect"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void LTPreviewReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Distribution_ListView_Viewmode" || View.Id == "Distribution_ListView_Viewmode_Copy" || View.Id == "Distribution_ListView_ItemStockInventory")
                {
                    //if (LTBarcodeReport.SelectedItem != null && LTBarcodeReport.SelectedItem.ToString() != string.Empty)
                    if (View.SelectedObjects.Count > 0)
                    {
                        strLT = string.Empty;
                        foreach (Distribution obj in View.SelectedObjects.Cast<Distribution>().Where(i => !string.IsNullOrEmpty(i.LT)))
                        {
                            if (obj.LT == null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "pendingdistribute"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                return;
                            }

                            if (strLT == string.Empty)
                            {
                                strLT = "'" + obj.Oid.ToString() + "'";
                            }
                            else
                            {
                                strLT = strLT + ",'" + obj.Oid.ToString() + "'";
                            }
                        }

                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\"));
                        }
                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\" + timeStamp + ".pdf");
                        XtraReport xtraReport = new XtraReport();

                        objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();

                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                        //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                        ObjReportingInfo.strLTOid = strLT;
                        //xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(LTBarcodeReport.SelectedItem.ToString(), ObjReportingInfo, false);
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("LTBarcode", ObjReportingInfo, false);
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
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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

        //private void Distributionsave_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (View != null && View.SelectedObjects.Count > 0)
        //        {
        //            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
        //            List<Distribution> lstDistributions = View.SelectedObjects.Cast<Distribution>().ToList();
        //            ((ListView)View).ObjectSpace.CommitChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Distribution_ListView_Viewmode")
                {
                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DistributeRollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count > 0)
                {
                    foreach (Distribution obj in View.SelectedObjects)
                    {
                        if (obj.Status == Distribution.LTStatus.Consumed || obj.Status == Distribution.LTStatus.Disposed)
                        {
                            //Application.ShowViewStrategy.ShowMessage("Item Already Consumed", InformationType.Error, 1500, InformationPosition.Top);
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackfail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }



                        ////else
                        ////{
                        ////    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        ////}

                        //else if (obj.Status == Distribution.LTStatus.Disposed)
                        //{
                        //    Application.ShowViewStrategy.ShowMessage("Item Already Disposed", InformationType.Error, 1500, InformationPosition.Top);
                        //    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectgivento"), InformationType.Warning, 1500, InformationPosition.Top);
                        //    return;
                        //}
                    }
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    object objToShow = objspace.CreateObject(typeof(Distribution));
                    //ICMInfo.RollBackReason = false;
                    DetailView createdView = Application.CreateDetailView(objspace, "Distribution_DetailView_Rollback", true, objToShow);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += RollBackReason_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
                    //                    if (subchild.Id == "DistributionItem")
                    //                    {
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[LT] Is Null"));
                    //                        var cap = subchild.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        int intReceive = 0;
                    //                        //CriteriaOperator criteria = CriteriaOperator.Parse("[LT] Is Null");
                    //                        //IList<Distribution> req = ObjectSpace.GetObjects<Distribution>(criteria);
                    //                        //string[] ReceiveID = new string[req.Count];
                    //                        //foreach (Distribution item in req)
                    //                        //{
                    //                        //    if (!ReceiveID.Contains(item.ReceiveID))
                    //                        //    {
                    //                        //        ReceiveID[intReceive] = item.ReceiveID;
                    //                        //        intReceive = intReceive + 1;
                    //                        //    }
                    //                        //}

                    //                        using (XPView lstview = new XPView(((XPObjectSpace)objectSpace).Session, typeof(Distribution)))
                    //                        {
                    //                            lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingDistribute'");
                    //                            lstview.Properties.Add(new ViewProperty("TReceiveID", SortDirection.Ascending, "ReceiveID", true, true));
                    //                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //                            if (lstview != null && lstview.Count > 0)
                    //                            {
                    //                                intReceive = lstview.Count;
                    //                            }
                    //                        }


                    //                        if (count > 0)
                    //                        {
                    //                            subchild.Caption = cap[0] + " (" + intReceive + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            subchild.Caption = cap[0];
                    //                        }
                    //                    }
                    //                    else if (subchild.Id == "DisposalItems")
                    //                    {
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 3 OR [Status] = 1) And [ExpiryDate] < ?", DateTime.Today));
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
                    //                    else if (subchild.Id == "VendorReagentCertificate")
                    //                    {
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        var count = objectSpace.GetObjectsCount(typeof(VendorReagentCertificate), CriteriaOperator.Parse("[LoadedDate] IS NULL AND [LoadedBy] IS NULL"));
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
                    //            else if (child.Id == "Alert")
                    //            {
                    //                foreach (ChoiceActionItem subchild in child.Items)
                    //                {
                    //                    if (subchild.Id == "Expiration Alert")
                    //                    {
                    //                        DateTime TodayDate = DateTime.Now;
                    //                        TodayDate = TodayDate.AddDays(7);
                    //                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                        var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] == 3 OR [Status] == 1) And [ExpiryDate] <= ?", TodayDate));
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

        //private void DistributeRollback_Executed(object sender, ActionBaseEventArgs e)
        //{
        //    //try
        //    //{

        //    //    //if (View.SelectedObjects.Count > 0)
        //    //    //{
        //    //    //    IObjectSpace os = Application.CreateObjectSpace(typeof(Distribution));
        //    //    //    Distribution obj = os.CreateObject<Distribution>();
        //    //    //    //ICMInfo.RollBackReason = false;
        //    //    //    DetailView createdView = Application.CreateDetailView(os, "Distribution_DetailView_Rollback", true, obj);
        //    //    //    createdView.ViewEditMode = ViewEditMode.Edit;
        //    //    //    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
        //    //    //    showViewParameters.Context = TemplateContext.NestedFrame;
        //    //    //    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
        //    //    //    DialogController dc = Application.CreateController<DialogController>();
        //    //    //    dc.SaveOnAccept = false;
        //    //    //    dc.Accepting += RollBackReason_Accepting;
        //    //    //    dc.CloseOnCurrentObjectProcessing = false;
        //    //    //    showViewParameters.Controllers.Add(dc);
        //    //    //    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
        //    //    //}
        //    //    //if (obj.Status == Distribution.LTStatus.Consumed || obj.Status == Distribution.LTStatus.Disposed)
        //    //    //{
        //    //    //    //Application.ShowViewStrategy.ShowMessage("Item Already Consumed", InformationType.Error, 1500, InformationPosition.Top);
        //    //    //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackfail"), InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    //    //    return;


        //    //    //else
        //    //    //{
        //    //    //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "rollbackreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
        //    //    //}
        //    //    else
        //    //    {
        //    //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
        //    //    }

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //    //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    //}
        //}


        private void RollBackReason_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Distribution objSamplecheckin = (Distribution)e.AcceptActionArgs.CurrentObject;
                if (objSamplecheckin != null && !string.IsNullOrEmpty(objSamplecheckin.RollbackReason) && !string.IsNullOrWhiteSpace(objSamplecheckin.RollbackReason))
                {
                    //SRInfo.bolNewJobID = false;
                    foreach (Distribution objSampleCheck in View.SelectedObjects.Cast<Distribution>().ToList())
                    {
                        objSampleCheck.RollbackReason = objSamplecheckin.RollbackReason;
                        objSampleCheck.Status = Distribution.LTStatus.PendingDistribute;
                    }
                    View.ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    foreach (Distribution obj in View.SelectedObjects)
                    {
                        if (obj.Status == Distribution.LTStatus.PendingConsume || obj.Status == Distribution.LTStatus.PendingDispose)
                        {
                            if (obj.ESID == null)
                            {
                                IObjectSpace space = Application.CreateObjectSpace();
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
                                newobj.itemremaining = obj.itemremaining;
                                newobj.itemreceived = obj.itemreceived;
                                if (obj.ReceivedBy != null)
                                {
                                    newobj.RequestedBy = space.GetObjectByKey<Employee>(obj.RequestedBy.Oid);
                                }
                                newobj.RequestedDate = obj.RequestedDate;
                                if (obj.RQID != null)
                                {
                                    newobj.RQID = obj.RQID.RQID;
                                }
                                if (obj.POID != null)
                                {
                                    newobj.POID = obj.POID.POID;
                                }
                                newobj.ReceiveID = obj.ReceiveID;
                                newobj.ReceiveCount = obj.ReceiveCount;
                                newobj.ReceiveDate = obj.ReceiveDate;
                                newobj.EnteredBy = ((XPObjectSpace)(space)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                newobj.EnteredDate = DateTime.Now;
                                newobj.DistributionDate = obj.DistributionDate;
                                if (obj.GivenBy != null)
                                {
                                    newobj.GivenBy = space.GetObjectByKey<Employee>(obj.GivenBy.Oid);
                                }
                                if (obj.givento != null)
                                {
                                    newobj.givento = space.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(obj.givento.Oid);
                                }
                                else
                                {
                                    newobj.Storage = space.GetObjectByKey<ICMStorage>(obj.Storage.Oid);
                                }
                                newobj.VendorLT = obj.VendorLT;
                                newobj.ExpiryDate = obj.ExpiryDate;
                                newobj.LT = obj.LT;
                                IList<ICMAlert> alertlist = space.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Expiry Alert - " + obj.LT));
                                if (alertlist != null)
                                {
                                    foreach (ICMAlert item in alertlist)
                                    {
                                        item.AlarmTime = null;
                                        item.RemindIn = null;
                                    }
                                }
                                space.CommitChanges();
                                obj.DistributionDate = null;
                                obj.DistributedBy = null;
                                obj.GivenBy = null;
                                obj.givento = null;
                                obj.Storage = null;
                                obj.VendorLT = null;
                                obj.ExpiryDate = null;
                                obj.LT = null;
                                obj.Status = Distribution.LTStatus.PendingDistribute;
                                ObjectSpace.CommitChanges();
                            }
                            else
                            {
                                IObjectSpace space = Application.CreateObjectSpace();
                                ExistingStock objes = space.FindObject<ExistingStock>(CriteriaOperator.Parse("[ESID] = ? ", obj.ESID));
                                if (objes.Qty > 1)
                                {
                                    objes.Qty -= 1;
                                    objes.Itemname.StockQty -= 1;
                                }
                                else if (objes.Qty == 1)
                                {
                                    objes.Itemname.StockQty -= 1;
                                    space.Delete(objes);
                                }
                                ObjectSpace.Delete(obj);
                                space.CommitChanges();
                                ObjectSpace.CommitChanges();
                            }
                        }
                    }
                    View.Refresh();
                    ObjectSpace.Refresh();
                    NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                    module.ShowNotificationsWindow = false;
                    module.NotificationsService.Refresh();
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


        private void DistributionDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {

            //if (e.SelectedChoiceActionItem.Id == "1M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 1");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "3M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 3");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "6M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 6");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "1Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 1");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "2Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 2");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "5Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 5");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "ALL")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] <> 0");
            //}
        }

        private void DistributionDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && DistributionDateFilter != null && DistributionDateFilter.SelectedItem != null)
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (View.Id == "Distribution_ListView_Viewmode")
                    {
                        if (strSelectedItem == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 3");
                        }
                        else if (strSelectedItem == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 6");
                        }
                        else if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 2");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(DistributionDate, Now()) <= 5");
                        }
                        else if (strSelectedItem == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] <> 0");
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

        private void Distributionview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                cs.Criteria["Filter"] = CriteriaOperator.Parse("[Status] <> 'PendingDistribute'");
                Frame.SetView(Application.CreateListView("Distribution_ListView_Viewmode", cs, false));
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
                if (bool.TryParse(parameter, out bool showreport))
                {
                    if (showreport)
                    {
                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\"));
                        }
                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\" + timeStamp + ".pdf");
                        XtraReport xtraReport = new XtraReport();

                        objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();

                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                        //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("LTBarcode", ObjReportingInfo, false);
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
                        IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Distribution));
                        CollectionSource cs = new CollectionSource(objectSpace, typeof(Distribution));
                        Frame.SetView(Application.CreateListView("Distribution_ListView_MainDistribute", cs, true));
                    }
                }
                else
                {
                    string[] arrParams = parameter.Split('|');
                    if (arrParams[0] == "UNSelectAll" || arrParams[0] == "SelectAll" || arrParams[0] == "Selected" || arrParams[0] == "UNSelected")
                    {
                        if (arrParams[0] == "UNSelectAll")
                        {
                            List<Distribution> lstDitributions = ((ListView)View).CollectionSource.List.Cast<Distribution>().ToList();
                            foreach (Distribution objDistribution in lstDitributions)
                            {
                                objDistribution.LT = string.Empty;
                            }
                            ((ListView)View).Refresh();
                        }
                        else if (arrParams[0] == "SelectAll")
                        {
                            List<Distribution> lstDistributions = ((ListView)View).SelectedObjects.Cast<Distribution>().ToList();
                            List<Tuple<Guid, string, string>> lstLt = new List<Tuple<Guid, string, string>>();
                            string strLTNo = string.Empty;
                            foreach (Distribution obj in lstDistributions.OrderBy(i => i.Item.items).ThenBy(i => i.itemreceivedsort).ToList())
                            {
                                Tuple<Guid, string, string> tupDistribution;
                                bool HasSequence = false;
                                Distribution objDistributionSequence = lstDistributions.FirstOrDefault(i => i.Oid != obj.Oid && i.Item.Oid == obj.Item.Oid && i.VendorLT == obj.VendorLT);
                                if (objDistributionSequence != null)
                                {
                                    HasSequence = true;
                                    Distribution objDistributionChangeLT = lstDistributions.FirstOrDefault(i => i.Oid != obj.Oid && i.Item.Oid == obj.Item.Oid && i.VendorLT == obj.VendorLT && !string.IsNullOrEmpty(i.LT) && !i.LT.Contains("_"));
                                    if (objDistributionChangeLT != null)
                                    {
                                        objDistributionChangeLT.LT += "_01";
                                        tupDistribution = new Tuple<Guid, string, string>(objDistributionChangeLT.Item.Oid, objDistributionChangeLT.LT, objDistributionChangeLT.VendorLT);
                                        lstLt.Add(tupDistribution);
                                    }
                                }
                                string templt = string.Empty;
                                if (obj.Item != null)
                                {
                                    if (lstLt.Count == 0)
                                    {
                                        string curdate = DateTime.Now.ToString("yy");
                                        //CriteriaOperator LTExpressionWithoutSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                                        //CriteriaOperator LTCriteriaWithoutSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Not Contains([LT], '_')", "LT" + curdate);

                                        //CriteriaOperator LTExpressionWithSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2, 6))");
                                        //CriteriaOperator LTCriteriaWithSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Contains([LT], '_')", "LT" + curdate);

                                        CriteriaOperator LTExpressionWithoutSequence = CriteriaOperator.Parse("Max(LT)");
                                        CriteriaOperator LTCriteriaWithoutSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Not Contains([LT], '_')", curdate);

                                        CriteriaOperator LTExpressionWithSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 0, 6))");
                                        CriteriaOperator LTCriteriaWithSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Contains([LT], '_')", curdate);

                                        int templtwithoutsequence = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Distribution), LTExpressionWithoutSequence, LTCriteriaWithoutSequence)) + 1;
                                        int templtwithsequence = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Distribution), LTExpressionWithSequence, LTCriteriaWithSequence)) + 1;

                                        if (templtwithoutsequence == 1 && templtwithsequence == 1)
                                        {
                                            //strLTNo = "LT" + curdate + "0001";
                                            strLTNo = curdate + "0001";
                                        }
                                        else
                                        {
                                            int templtno = (templtwithoutsequence > templtwithsequence) ? templtwithoutsequence : templtwithsequence;
                                            //strLTNo = "LT" + templtno;
                                            strLTNo = templtno.ToString();
                                        }
                                        if (HasSequence)
                                        {
                                            strLTNo += "_01";
                                        }
                                    }
                                    else
                                    if (!HasSequence)
                                    {
                                        int id = Convert.ToInt32(DateTime.Now.ToString("yy") + "0001");
                                        int id2 = Convert.ToInt32(DateTime.Now.ToString("yy") + "0001");

                                        //if (lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")) != null && lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")).Count() > 0)
                                        //{
                                        //    id = lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")).Max(i => Convert.ToInt32(i.Item2.Substring(2, i.Item2.Length - 2))) + 1;
                                        //}
                                        //if (lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")) != null && lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")).Count() > 0)
                                        //{
                                        //    id2 = lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")).Max(i => Convert.ToInt32(i.Item2.Split('_')[0].Substring(2, i.Item2.Split('_')[0].Length - 2))) + 1;
                                        //}

                                        if (lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")) != null && lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")).Count() > 0)
                                        {
                                            id = lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")).Max(i => Convert.ToInt32(i.Item2)) + 1;
                                        }
                                        if (lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")) != null && lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")).Count() > 0)
                                        {
                                            id2 = lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")).Max(i => Convert.ToInt32(i.Item2.Split('_')[0])) + 1;
                                        }

                                        if (id == id2)
                                        {
                                            //strLTNo = "LT" + id;
                                            strLTNo = id.ToString();
                                        }
                                        else
                                        {
                                            int templtno = (id > id2) ? id : id2;
                                            //strLTNo = "LT" + templtno;
                                            strLTNo = templtno.ToString();
                                        }
                                    }
                                    else
                                    {
                                        int curltno = Convert.ToInt32(DateTime.Now.ToString("yy") + "0001");
                                        int seqno = 1;
                                        if (lstLt.Where(i => i.Item1 == obj.Item.Oid && i.Item3 == obj.VendorLT) != null && lstLt.Where(i => i.Item1 == obj.Item.Oid && i.Item3 == obj.VendorLT).Count() > 0)
                                        {
                                            string strlt = lstLt.FirstOrDefault(i => i.Item1 == obj.Item.Oid && i.Item3 == obj.VendorLT).Item2;
                                            //curltno = Convert.ToInt32(strlt.Split('_')[0].Substring(2, strlt.Split('_')[0].Length - 2));
                                            //seqno = Convert.ToInt32(lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.StartsWith("LT" + curltno)).ToList().Max(i => Convert.ToInt32(i.Item2.Split('_')[1]))) + 1;

                                            curltno = Convert.ToInt32(strlt.Split('_')[0]);
                                            seqno = Convert.ToInt32(lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.StartsWith(curltno.ToString())).ToList().Max(i => Convert.ToInt32(i.Item2.Split('_')[1]))) + 1;
                                        }
                                        else
                                        {
                                            int id = Convert.ToInt32(DateTime.Now.ToString("yy") + "0001");
                                            int id2 = Convert.ToInt32(DateTime.Now.ToString("yy") + "0001");

                                            if (lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")) != null && lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")).Count() > 0)
                                            {
                                                id = lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && !i.Item2.Contains("_")).Max(i => Convert.ToInt32(i.Item2.ToString())) + 1;
                                            }
                                            if (lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")) != null && lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")).Count() > 0)
                                            {
                                                id2 = lstLt.Where(i => !string.IsNullOrEmpty(i.Item2) && i.Item2.Contains("_")).Max(i => Convert.ToInt32(i.Item2.Split('_')[0])) + 1;
                                            }

                                            if (id == id2)
                                            {
                                                curltno = id;
                                            }
                                            else
                                            {
                                                int templtno = (id > id2) ? id : id2;
                                                curltno = templtno;
                                            }
                                        }
                                        if (seqno.ToString().Length == 1)
                                        {
                                            //strLTNo = "LT" + curltno + "_0" + seqno;
                                            strLTNo = curltno + "_0" + seqno;
                                        }
                                        else
                                        {
                                            //strLTNo = "LT" + curltno + "_" + seqno;
                                            strLTNo = curltno + "_" + seqno;
                                        }
                                    }

                                    obj.LT = strLTNo;
                                    tupDistribution = new Tuple<Guid, string, string>(obj.Item.Oid, obj.LT, obj.VendorLT);
                                    lstLt.Add(tupDistribution);
                                }
                            }
                            ((ListView)View).Refresh();
                        }
                        else if (arrParams[0] == "Selected")
                        {
                            List<Distribution> lstDistributions = ((ListView)View).SelectedObjects.Cast<Distribution>().ToList();
                            List<Tuple<string, string, string, bool>> lstLt = new List<Tuple<string, string, string, bool>>();
                            string strLTNo = string.Empty;
                            foreach (Distribution obj in lstDistributions.OrderBy(i => i.Item.items).ThenBy(i => i.LT).ToList())
                            {
                                bool HasSequence = false;
                                Distribution objDistributionSequence = lstDistributions.FirstOrDefault(i => i.Oid != obj.Oid && i.Item.Oid == obj.Item.Oid && i.VendorLT == obj.VendorLT);
                                if (objDistributionSequence != null)
                                {
                                    HasSequence = true;
                                    Distribution objDistributionChangeLT = lstDistributions.FirstOrDefault(i => i.Oid != obj.Oid && i.Item.Oid == obj.Item.Oid && i.VendorLT == obj.VendorLT && !string.IsNullOrEmpty(i.LT) && !i.LT.Contains("_"));
                                    if (objDistributionChangeLT != null)
                                    {
                                        objDistributionChangeLT.LT += "_01";
                                        Tuple<string, string, string, bool> tupDistribution = new Tuple<string, string, string, bool>(objDistributionChangeLT.Item.ItemCode, objDistributionChangeLT.LT, objDistributionChangeLT.VendorLT, objDistributionChangeLT.Item.IsVendorLT);
                                        lstLt.Add(tupDistribution);
                                    }
                                }
                                if (string.IsNullOrEmpty(obj.LT))
                                {
                                    Tuple<string, string, string, bool> tupDistribution;
                                    string templt = string.Empty;
                                    if (obj.Item != null)
                                    {
                                        if (lstDistributions.Count == 1)
                                        {
                                            string curdate = DateTime.Now.ToString("yy");
                                            //CriteriaOperator LTExpressionWithoutSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2))");
                                            //CriteriaOperator LTCriteriaWithoutSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Not Contains([LT], '_')", "LT" + curdate);

                                            //CriteriaOperator LTExpressionWithSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 2, 6))");
                                            //CriteriaOperator LTCriteriaWithSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Contains([LT], '_')", "LT" + curdate);

                                            CriteriaOperator LTExpressionWithoutSequence = CriteriaOperator.Parse("Max(LT)");
                                            CriteriaOperator LTCriteriaWithoutSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Not Contains([LT], '_')", curdate);

                                            CriteriaOperator LTExpressionWithSequence = CriteriaOperator.Parse("Max(SUBSTRING(LT, 0, 6))");
                                            CriteriaOperator LTCriteriaWithSequence = CriteriaOperator.Parse("StartsWith([LT], ?) And Contains([LT], '_')", curdate);

                                            int templtwithoutsequence = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Distribution), LTExpressionWithoutSequence, LTCriteriaWithoutSequence)) + 1;
                                            int templtwithsequence = Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Distribution), LTExpressionWithSequence, LTCriteriaWithSequence)) + 1;

                                            if (templtwithoutsequence == 1 && templtwithsequence == 1)
                                            {
                                                //strLTNo = "LT" + curdate + "0001";
                                                strLTNo = curdate + "0001";
                                            }
                                            else
                                            {
                                                int templtno = (templtwithoutsequence > templtwithsequence) ? templtwithoutsequence : templtwithsequence;
                                                //strLTNo = "LT" + templtno;
                                                strLTNo = templtno.ToString();
                                            }
                                        }
                                        else
                                        if (!HasSequence)
                                        {
                                            int id = Convert.ToInt32(DateTime.Now.ToString("yy") + "0001");
                                            int id2 = Convert.ToInt32(DateTime.Now.ToString("yy") + "0001");
                                            if (lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && !i.LT.Contains("_")) != null && lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && !i.LT.Contains("_")).Count() > 0)
                                            {
                                                //id = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && !i.LT.Contains("_")).Max(i => Convert.ToInt32(i.LT.Substring(2, i.LT.Length - 2))) + 1;
                                                id = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && !i.LT.Contains("_")).Max(i => Convert.ToInt32(i.LT.ToString())) + 1;
                                            }
                                            if (lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_")) != null && lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_")).Count() > 0)
                                            {
                                                //id2 = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_")).Max(i => Convert.ToInt32(i.LT.Split('_')[0].Substring(2, i.LT.Split('_')[0].Length - 2))) + 1;
                                                id2 = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_")).Max(i => Convert.ToInt32(i.LT.Split('_')[0])) + 1;
                                            }

                                            if (id == id2)
                                            {
                                                //strLTNo = "LT" + id;
                                                strLTNo = id.ToString();
                                            }
                                            else
                                            {
                                                int templtno = (id > id2) ? id : id2;
                                                //strLTNo = "LT" + templtno;
                                                strLTNo = templtno.ToString();
                                            }
                                        }
                                        else
                                        {
                                            int curltno = Convert.ToInt32(DateTime.Now.ToString("yy") + "0001");
                                            int seqno = 1;
                                            if (lstDistributions.Where(i => i.Oid != obj.Oid && i.Item.Oid == obj.Item.Oid && i.VendorLT == obj.VendorLT && !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_")) != null && lstDistributions.Where(i => i.Oid != obj.Oid && i.Item.ItemCode == obj.Item.ItemCode && i.Item.IsVendorLT == obj.Item.IsVendorLT && i.VendorLT == obj.VendorLT && !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_")).Count() > 0)
                                            {
                                                //curltno = lstDistributions.Where(i => i.Oid != obj.Oid && i.Item.Oid == obj.Item.Oid && i.VendorLT == obj.VendorLT && !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_")).Max(i => Convert.ToInt32(i.LT.Split('_')[0].Substring(2, i.LT.Split('_')[0].Length - 2)));
                                                curltno = lstDistributions.Where(i => i.Oid != obj.Oid && i.Item.Oid == obj.Item.Oid && i.VendorLT == obj.VendorLT && !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_")).Max(i => Convert.ToInt32(i.LT.Split('_')[0]));
                                            }
                                            //if (lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.StartsWith("LT" + curltno)) != null && lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.StartsWith("LT" + curltno)).Count() > 0)
                                            //{
                                            //    seqno = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.StartsWith("LT" + curltno)).ToList().Max(i => Convert.ToInt32(i.LT.Split('_')[1])) + 1;
                                            //}
                                            if (lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.StartsWith(curltno.ToString())) != null && lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.StartsWith(curltno.ToString())).Count() > 0)
                                            {
                                                seqno = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.StartsWith(curltno.ToString())).ToList().Max(i => Convert.ToInt32(i.LT.Split('_')[1])) + 1;
                                            }
                                            if (seqno.ToString().Length == 1)
                                            {
                                                //strLTNo = "LT" + curltno + "_0" + seqno;
                                                strLTNo = curltno + "_0" + seqno;
                                            }
                                            else
                                            {
                                                //strLTNo = "LT" + curltno + "_" + seqno;
                                                strLTNo = curltno + "_" + seqno;
                                            }
                                        }

                                        obj.LT = strLTNo;
                                        tupDistribution = new Tuple<string, string, string, bool>(obj.Item.ItemCode, obj.LT, obj.VendorLT, obj.Item.IsVendorLT);
                                        lstLt.Add(tupDistribution);
                                    }
                                }
                            }
                            ((ListView)View).Refresh();
                        }
                        else if (arrParams[0] == "UNSelected" && arrParams.Count() > 1 && !string.IsNullOrEmpty(arrParams[1]))
                        {
                            Distribution objDistribution = ObjectSpace.FindObject<Distribution>(CriteriaOperator.Parse("[Oid]=?", new Guid(arrParams[1])), true);
                            if (objDistribution != null)
                            {
                                string strRemovedLT = objDistribution.LT;
                                string strLTNO = string.Empty;
                                objDistribution.LT = string.Empty;
                                if (((ListView)View).SelectedObjects.Count > 0)
                                {
                                    List<Distribution> lstDistributions = ((ListView)View).SelectedObjects.Cast<Distribution>().ToList();
                                    bool HasSequence = false;
                                    Distribution objDistributionSequence = lstDistributions.FirstOrDefault(i => i.Oid != objDistribution.Oid && i.Item.Oid == objDistribution.Item.Oid && i.VendorLT == objDistribution.VendorLT);
                                    if (objDistributionSequence != null)
                                    {
                                        HasSequence = true;
                                    }
                                    if (!string.IsNullOrEmpty(strRemovedLT))
                                    {
                                        if (strRemovedLT.Contains('_'))
                                        {
                                            strLTNO = strRemovedLT.Split('_')[0];
                                        }
                                        else
                                        {
                                            strLTNO = strRemovedLT;
                                        }
                                        if (HasSequence)
                                        {
                                            int removedSeqno = 1;
                                            int seqno = 1;
                                            if (strRemovedLT.Split('_').Count() > 1)
                                            {
                                                removedSeqno = Convert.ToInt32(strRemovedLT.Split('_')[1]);
                                                seqno = Convert.ToInt32(strRemovedLT.Split('_')[1]);
                                            }
                                            if (lstDistributions.Where(i => i.Oid != objDistribution.Oid && i.Item.Oid == objDistribution.Item.Oid && i.VendorLT == objDistribution.VendorLT).Count() > 1)
                                            {
                                                foreach (Distribution obj in lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.StartsWith(strLTNO) && i.LT.Split('_').Count() > 1 && Convert.ToInt32(i.LT.Split('_')[1]) > removedSeqno).ToList())
                                                {
                                                    if (seqno.ToString().Length == 1)
                                                    {
                                                        obj.LT = strLTNO + "_0" + seqno;
                                                    }
                                                    else
                                                    {
                                                        obj.LT = strLTNO + "_" + seqno;
                                                    }
                                                    seqno++;
                                                }
                                            }
                                            else
                                            {
                                                objDistributionSequence.LT = strLTNO;
                                            }
                                        }

                                        if (!strRemovedLT.Contains('_'))
                                        {
                                            //IEnumerable<Distribution> lstIndividDesciption = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && !i.LT.Contains("_") && Convert.ToInt32(i.LT.Substring(2)) > Convert.ToInt32(strLTNO.Substring(2)));
                                            IEnumerable<Distribution> lstIndividDesciption = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && !i.LT.Contains("_") && Convert.ToInt32(i.LT) > Convert.ToInt32(strLTNO));
                                            if (lstIndividDesciption != null && lstIndividDesciption.Count() > 0)
                                            {
                                                foreach (Distribution obj in lstIndividDesciption.ToList())
                                                {
                                                    //obj.LT = "LT" + (Convert.ToInt32(obj.LT.Substring(2)) - 1);
                                                    obj.LT = (Convert.ToInt32(obj.LT) - 1).ToString();
                                                }
                                            }

                                            //List<Distribution> lstSeqDesciption = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_") && Convert.ToInt32(i.LT.Substring(2, i.LT.Split('_')[0].Length - 2)) > Convert.ToInt32(strLTNO.Substring(2))).ToList();
                                            List<Distribution> lstSeqDesciption = lstDistributions.Where(i => !string.IsNullOrEmpty(i.LT) && i.LT.Contains("_") && Convert.ToInt32(i.LT.Split('_')[0]) > Convert.ToInt32(strLTNO)).ToList();
                                            if (lstSeqDesciption != null && lstSeqDesciption.Count > 0)
                                            {
                                                foreach (Distribution obj in lstSeqDesciption)
                                                {
                                                    string[] strTemp = obj.LT.Split('_');
                                                    //obj.LT = "LT" + (Convert.ToInt32(strTemp[0].Substring(2)) - 1) + "_" + strTemp[1];
                                                    obj.LT = (Convert.ToInt32(strTemp[0]) - 1) + "_" + strTemp[1];
                                                }
                                            }
                                        }
                                    }
                                }
                                ((ListView)View).Refresh();
                            }
                        }
                    }
                    if (Application.MainWindow.View.Id == "Distribution_DetailView")
                    {
                        if (!string.IsNullOrEmpty(parameter))
                        {
                            if (parameter == "0")
                            {
                                Distribution objD = Application.MainWindow.View.CurrentObject as Distribution;
                                objD.IsDeplete = true;
                                ObjectSpace.CommitChanges();
                            }
                            //if (View.Id == "Distribution_itemDepletionsCollection_ListView")
                            //{
                            //    if (parameter != null)
                            //    {
                            //        ItemDepletion depletion = View.CurrentObject as ItemDepletion;
                            //        if (depletion != null)
                            //        {
                            //            if (depletion.AmountTaken > depletion.StockAmount)
                            //            {
                            //                Application.ShowViewStrategy.ShowMessage("Please Enter the lesser amount", InformationType.Error, 3000, InformationPosition.Top);
                            //                ObjectSpace.Refresh();
                            //                return;
                            //            }
                            //            else if (depletion.AmountTaken == 0)
                            //            {
                            //                Application.ShowViewStrategy.ShowMessage("Amounttaken doesnt allow to enter zero value", InformationType.Error, 3000, InformationPosition.Top);
                            //                ObjectSpace.Refresh();
                            //                return;

                            //            }

                            //        }
                            //    } 
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


        private void FractionalHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                ListView createview = Application.CreateListView("Distribution_ListView_Fractional_Consumption_History", cs, false);
                createview.Caption = "Fractional Consumption - History";
                Frame.SetView(createview);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
