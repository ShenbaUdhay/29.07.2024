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
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.ICM
{
    public partial class ConsumptionViewController : ViewController
    {
        #region Declartion

        MessageTimer timer = new MessageTimer();
        ICMinfo objIcmInfo = new ICMinfo();
        ShowNavigationItemController ShowNavigationController;
        consumptionquerypanelinfo objcons = new consumptionquerypanelinfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        #endregion

        #region Constructor
        public ConsumptionViewController()
        {
            InitializeComponent();
            TargetViewId = "Distribution_ListView_Consumption;" + "ConsumptionQuerypanel_DetailView;" + "Distribution_ListView_Consumption_Copy;" + "Distribution_LookupListView;" + "Distribution_ListView_ConsumptionViewmode;";
            Consume.TargetViewId = "Distribution_ListView_Consumption;";
            ConsumeReturn.TargetViewId = "Distribution_ListView_ConsumptionViewmode";
            ConsumeQueryPanel.TargetViewId = "Distribution_ListView_Consumption";
            txtBarcodeActionConsumption.TargetViewId = "Distribution_ListView_Consumption";
            txtBarcodeActionConsumption.CustomizeControl += TextBoxAction_CustomizeControl;
            PendingConsume.TargetViewId = "Distribution_ListView_Consumption";
            Consumeview.TargetViewId = "Distribution_ListView_Consumption";
            ConsumeDateFilter.TargetViewId = "Distribution_ListView_ConsumptionViewmode";
        }
        #endregion

        #region DefaultEvents
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Frame.GetController<ConsumptionViewController>().Actions["ConsumeQueryPanel"].Active.SetItemValue("", false);
                Frame.GetController<ConsumptionViewController>().Actions["PendingConsume"].Active.SetItemValue("", false);
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                txtBarcodeActionConsumption.ValueChanged += TextBoxAction_ValueChanged;
                //Permisson code
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.ConsumptionIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.ConsumptionIsWrite = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ConsumptionItems" && i.Write == true) != null)
                                {
                                    objPermissionInfo.ConsumptionIsWrite = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                Consume.Active.SetItemValue("ShowConsume", objPermissionInfo.ConsumptionIsWrite);
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
                //        if (navPermissionDetails != null && View.Id == "Distribution_ListView_Consumption")
                //        {
                //            if (navPermissionDetails.Write == true)
                //            {
                //                Consume.Active.SetItemValue("ConsumptionViewController.Consume", true);
                //            }
                //            else if (navPermissionDetails.Write == false)
                //            {
                //                Consume.Active.SetItemValue("ConsumptionViewController.Consume", false);
                //            }
                //        }
                //    }
                //}
                if (View.Id == "Distribution_ListView_ConsumptionViewmode")
                {
                    ConsumeReturn.Active.SetItemValue("ShowConsumeReturn", objPermissionInfo.ConsumptionIsWrite);
                    if (ConsumeDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        ConsumeDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        ConsumeDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        ConsumeDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        ConsumeDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        ConsumeDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        ConsumeDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        ConsumeDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //ConsumeDateFilter.SelectedIndex = 1;
                    ConsumeDateFilter.SelectedItemChanged += ConsumeDateFilter_SelectedItemChanged;
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                    {
                        ConsumeDateFilter.SelectedIndex = 0;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 1 And [ConsumptionDate] Is Not Null");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                    {
                    ConsumeDateFilter.SelectedIndex = 1;
                    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 3 And [ConsumptionDate] Is Not Null");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                    {
                        ConsumeDateFilter.SelectedIndex = 2;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 6 And [ConsumptionDate] Is Not Null");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                    {
                        ConsumeDateFilter.SelectedIndex = 3;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 1 And [ConsumptionDate] Is Not Null");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                    {
                        ConsumeDateFilter.SelectedIndex = 4;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 2 And [ConsumptionDate] Is Not Null");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                    {
                        ConsumeDateFilter.SelectedIndex = 5;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 5 And [ConsumptionDate] Is Not Null");
                    }
                    else if (setting.InventoryWorkFlow == EnumDateFilter.All)
                    {
                        ConsumeDateFilter.SelectedIndex = 6;
                        ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[Status] <> 0");
                    }
                    //((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 3 And [ConsumptionDate] Is Not Null");
                }
                if (View != null && View.CurrentObject != null && View.Id == "ConsumptionQuerypanel_DetailView")
                {
                    ConsumptionQuerypanel QPanel = (ConsumptionQuerypanel)View.CurrentObject;
                    objcons.rgMode = QPanel.Mode.ToString();
                }
                if (View.Id == "Distribution_ListView_Consumption")
                {
                    ((ListView)View).CollectionSource.Criteria["FilterCriteria"] = CriteriaOperator.Parse("1=2");
                    Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>().FullTextFilterAction.Executing += FullTextFilterAction_Executing;
                }
                if (objIcmInfo.ObjectsToShow == null)
                {
                    objIcmInfo.ObjectsToShow = new List<Guid>();
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
                if (((ListView)View).CollectionSource.Criteria.ContainsKey("FilterCriteria"))
                {
                    ((ListView)View).CollectionSource.Criteria.Remove("FilterCriteria");
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
                if (base.View != null && base.View.Id == "Distribution_ListView_Consumption")
                {
                    if (string.IsNullOrEmpty(objcons.ConsumptionFilter) && (objIcmInfo.ObjectsToShow == null || objIcmInfo.ObjectsToShow.Count == 0))
                    {
                        if (string.IsNullOrEmpty(objcons.rgMode))
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'PendingConsume'");
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Emptygrid'");
                        }
                        //Frame.GetController<ConsumptionViewController>().Actions["ConsumeReturn"].Active.SetItemValue("", false);
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[LT] Is Null");                       
                    }
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                    if (user!=null)
                    {
                        gridListEditor.Grid.JSProperties["cpusername"] = user.DisplayName; 
                    }
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridview = gridListEditor.Grid;
                    if (gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {
                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingConsume'){
                    if(e.focusedColumn.fieldName == 'ConsumptionDate'|| e.focusedColumn.fieldName == 'ConsumptionBy.Oid'){
                    e.cancel = false;                  
                    }
                    else{
                    e.cancel = true;                  
                    }}
                    else{
                    e.cancel = true;                  
                    }
                    }";
                        gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) 
                        {//console.log('selectedrowcount:'+s.GetSelectedRowCount());
                            //if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) 
                            if (e.visibleIndex == -1 && s.GetSelectedRowCount() == 0) 
                            {//console.log('visiblerowsonpage'+s.GetVisibleRowsOnPage()+'unselectall');
                                for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                {
                                    if(s.batchEditApi.GetCellValue(i, 'Status') == 'PendingConsume')
                                    {//console.log(i+s.batchEditApi.GetCellValue(i, 'Status'));
                                        s.batchEditApi.SetCellValue(i, 'ConsumptionDate', null);                  
                                        s.batchEditApi.SetCellValue(i, 'ConsumptionBy', null);                                      
                                    }
                                }
                            }
                            //else if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0))
                            else if(e.visibleIndex == -1 && s.GetSelectedRowCount() > 0)
                            {//console.log('visiblerowsonpage'+s.GetVisibleRowsOnPage()+'selectall');                   
                                var today = new Date();                                        
                                for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                {
                                    if(s.batchEditApi.GetCellValue(i, 'Status') == 'PendingConsume')
                                    {//console.log(i+s.batchEditApi.GetCellValue(i, 'Status')+'-'+today+'-'+s.cpuserid);
                                        s.batchEditApi.SetCellValue(i, 'ConsumptionDate', today);
                                        s.batchEditApi.SetCellValue(i, 'ConsumptionBy', s.cpuserid, s.cpusername, false);
                                    }
                                }
                            }
                            else
                            {
                                if(s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingConsume')
                                {
                                    if (s.IsRowSelectedOnPage(e.visibleIndex)) 
                                    {                   
                                        var today = new Date();                                        
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ConsumptionDate', today);
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ConsumptionBy', s.cpuserid, s.cpusername, false);
                                    } 
                                    else 
                                    {
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ConsumptionDate', null);                   
                                        s.batchEditApi.SetCellValue(e.visibleIndex, 'ConsumptionBy', null);                   
                                    }
                                }
                            }
                        }";

                        //gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) 
                        //{ 
                        //    if (e.visibleIndex != '-1')
                        //    {
                        //        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) 
                        //        {      
                        //            if (s.IsRowSelectedOnPage(e.visibleIndex))
                        //            {                                    
                        //                if(s.batchEditApi.GetCellValue(i, 'Status') == 'PendingConsume')
                        //                {
                        //                    var today = new Date();                                        
                        //                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ConsumptionDate', today);
                        //                    s.batchEditApi.SetCellValue(e.visibleIndex, 'ConsumptionBy', s.cpuserid, s.cpusername, false);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                s.batchEditApi.SetCellValue(e.visibleIndex, 'ConsumptionDate', null);
                        //                s.batchEditApi.SetCellValue(e.visibleIndex, 'ConsumptionBy', null);
                        //            }
                        //        }); 
                        //    }
                        //    else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                        //    {
                        //        for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                        //        {
                        //            if(s.batchEditApi.GetCellValue(i, 'Status') == 'PendingConsume')
                        //            {
                        //                var today = new Date();                                        
                        //                s.batchEditApi.SetCellValue(i, 'ConsumptionDate', today);
                        //                s.batchEditApi.SetCellValue(i, 'ConsumptionBy', s.cpuserid, s.cpusername, false);
                        //            }
                        //        }
                        //    }
                        //    else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                        //    {
                        //        for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                        //        {
                        //            if(s.batchEditApi.GetCellValue(i, 'Status') == 'PendingConsume')
                        //            {
                        //                s.batchEditApi.SetCellValue(i, 'ConsumptionDate', null);
                        //                s.batchEditApi.SetCellValue(i, 'ConsumptionBy', null);
                        //            }
                        //        }
                        //    }                      
                        //}";

                    }
                }
                else if (View != null && View.Id == "Distribution_ListView_Consumption_Copy")
                {
                    if (objcons.rgMode == ENMode.View.ToString())
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ConsumptionBy] Is Not Null And [ConsumptionDate] Is Not Null And [LT] Is Not Null");
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ConsumptionBy] Is Null And [ConsumptionDate] Is Null And [LT] Is Not Null  And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now);
                    }
                }
                else if (View != null && View.Id == "Distribution_LookupListView")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ConsumptionBy] Is Null And [ConsumptionDate] Is Null And [LT] Is Not Null  And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now);
                    ((ListView)View).CollectionSource.Criteria["filter1"] = new NotOperator(new InOperator("LT", objcons.Items));
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

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            try
            {
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                txtBarcodeActionConsumption.ValueChanged -= TextBoxAction_ValueChanged;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void TextBoxAction_CustomizeControl(object sender, CustomizeControlEventArgs e)
        {
            //ParametrizedActionMenuActionItem actionItem = e.Control as ParametrizedActionMenuActionItem;
            //if (actionItem != null)
            //{
            //    ASPxButtonEdit Editor = actionItem.Control.Editor as ASPxButtonEdit;
            //    if (Editor != null)
            //    {
            //        Editor.Buttons[0].Visible=false;
            //        Editor.Buttons[1].Visible = false;
            //    }
            //}

            //throw new NotImplementedException();
        }
        private void TextBoxAction_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcodeActionConsumption.Value != null && txtBarcodeActionConsumption.Value.ToString() != string.Empty)
                {
                    string TextValue = txtBarcodeActionConsumption.Value.ToString();
                    Distribution objDistribution = ObjectSpace.FindObject<Distribution>(CriteriaOperator.Parse("[LT]= ?", TextValue));

                    if (objDistribution != null)
                    {
                        if (View is ListView)
                        {
                            if (((ListView)View).CollectionSource.Criteria.ContainsKey("FilterCriteria"))
                            {
                                ((ListView)View).CollectionSource.Criteria.Remove("FilterCriteria");
                            }
                            if (!objIcmInfo.ObjectsToShow.Contains(new Guid(objDistribution.Oid.ToString())))
                            {
                                objIcmInfo.ObjectsToShow.Clear();
                                objIcmInfo.ObjectsToShow.Add(new Guid(objDistribution.Oid.ToString()));
                            }
                            ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", objIcmInfo.ObjectsToShow);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "LTNot"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", objIcmInfo.ObjectsToShow);
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
                if (base.View != null && base.View.Id == "ConsumptionQuerypanel_DetailView")
                {
                    if (View != null && View.CurrentObject == e.Object && e.PropertyName == "LT")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(ConsumptionQuerypanel))
                        {
                            ConsumptionQuerypanel CPanel = (ConsumptionQuerypanel)e.Object;
                            if (CPanel.LT != null)
                            {
                                if (CPanel.Mode.Equals(ENMode.View))
                                {
                                    objcons.ConsumptionFilter = "[LT] == '" + CPanel.LT.LT + "'";
                                }
                                else
                                {
                                    objcons.ConsumptionFilter = "[LT] == '" + CPanel.LT.LT + "'";
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Mode")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(ConsumptionQuerypanel))
                        {
                            ConsumptionQuerypanel CPanel = (ConsumptionQuerypanel)e.Object;
                            if (CPanel != null)
                            {
                                objcons.rgMode = CPanel.Mode.ToString();
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
        private void Consume_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                List<string> list = new List<string>();
                if (View.SelectedObjects.Count >= 1)
                {
                    if (base.View != null && base.View.Id == "Distribution_ListView_Consumption")
                    {
                        foreach (Distribution obj in View.SelectedObjects)
                        {
                            if (obj.Status == Distribution.LTStatus.Consumed)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalid"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                        foreach (Distribution obj in View.SelectedObjects)
                        {
                            if (obj.ConsumptionBy == null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectConsumedBy"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        foreach (Distribution obj in View.SelectedObjects)
                        {
                            if (obj.Item.StockQty > 0)
                            {
                                obj.Item.StockQty = obj.Item.StockQty - 1;
                            }
                            obj.Status = Distribution.LTStatus.Consumed;
                            obj.EnteredBy = ((XPObjectSpace)(ObjectSpace)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                            obj.EnteredDate = DateTime.Now;
                            ObjectSpace.CommitChanges();
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            ConsumptionHistory newobj = objspace.CreateObject<ConsumptionHistory>();
                            newobj.Distribution = objspace.GetObjectByKey<Distribution>(obj.Oid);
                            newobj.ConsumptionBy = objspace.GetObjectByKey<Employee>(obj.ConsumptionBy.Oid);
                            newobj.ConsumptionDate = obj.ConsumptionDate;
                            newobj.EnteredBy = objspace.GetObjectByKey<Employee>(obj.EnteredBy.Oid);
                            newobj.EnteredDate = obj.EnteredDate;
                            newobj.Consumed = true;
                            IObjectSpace alertobjspace = Application.CreateObjectSpace();
                            if (obj.Item.StockQty <= obj.Item.AlertQty)
                            {
                                ICMAlert objdisp1 = alertobjspace.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")"));
                                if (objdisp1 == null)
                                {
                                    ICMAlert obj1 = alertobjspace.CreateObject<ICMAlert>();
                                    obj1.Subject = "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")";
                                    obj1.StartDate = DateTime.Now;
                                    obj1.DueDate = DateTime.Now.AddDays(7);
                                    obj1.RemindIn = TimeSpan.FromMinutes(5);
                                    obj1.Description = "Nice";
                                    alertobjspace.CommitChanges();
                                }
                            }
                            IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + obj.LT));
                            if (alertlist != null)
                            {
                                foreach (ICMAlert item in alertlist)
                                {
                                    item.AlarmTime = null;
                                    item.RemindIn = null;
                                }
                            }
                            objspace.CommitChanges();
                        }
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'PendingConsume'");
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 1");
                        View.ObjectSpace.Refresh();
                        View.Refresh();
                        NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                        module.ShowNotificationsWindow = false;
                        module.NotificationsService.Refresh();
                        ((ASPxGridListEditor)((ListView)View).Editor).Grid.Selection.UnselectAll();
                        WebWindow.CurrentRequestWindow.RegisterClientScript("xml", "sessionStorage.clear();");
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
                                            if (subchild.Id == "ConsumptionItems")
                                            {
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[ConsumptionBy] Is Null And [ConsumptionDate] Is Null And [Status] = 'PendingConsume' And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now));
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
                                            if (subchild.Id == "DisposalItems")
                                            {
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                //var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 'PendingDispose' OR [Status] = 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now));
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
                                        }
                                    }

                                    if (child.Id == "Alert")
                                    {
                                        foreach (ChoiceActionItem subchild in child.Items)
                                        {
                                            if (subchild.Id == "Stock Alert")
                                            {
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                var count = objectSpace.GetObjectsCount(typeof(Items), CriteriaOperator.Parse("[StockQty] <= [AlertQty]"));
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
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ConsumeSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
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
        private void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (objIcmInfo.RollBackReason == null)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "returnreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ConsumeQueryPanel_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (objIcmInfo.ObjectsToShow.Count > 0)
                {
                    objIcmInfo.ObjectsToShow.Clear();
                }
                ((ListView)View).CollectionSource.Criteria.Clear();
                int RowCount = 0;
                if (View != null && View.Id == "Distribution_ListView_Consumption")
                {
                    if (objcons.ConsumptionFilter == string.Empty)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReceiveID] == NULL");
                    }
                    else if (objcons.ConsumptionFilter != string.Empty)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse(objcons.ConsumptionFilter);
                        RowCount = ((ListView)View).CollectionSource.GetCount();
                        //if (objcons.rgMode == ENMode.View.ToString())
                        //{
                        //    Frame.GetController<ConsumptionViewController>().Actions["Consume"].Active.SetItemValue("", false);
                        //    Frame.GetController<ConsumptionViewController>().Actions["ConsumeReturn"].Active.SetItemValue("", true);
                        //}
                        //else
                        //{
                        //    Frame.GetController<ConsumptionViewController>().Actions["Consume"].Active.SetItemValue("", true);
                        //    Frame.GetController<ConsumptionViewController>().Actions["ConsumeReturn"].Active.SetItemValue("", false);
                        //}
                        if (RowCount == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalid"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
        private void ConsumeQueryPanel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(ConsumptionQuerypanel));
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
        private void ConsumeReturn_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
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
                    CreateDetailView.Caption = "Consume Return ";
                    e.View = CreateDetailView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ConsumeReturn_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count > 0)
                {
                    foreach (Distribution obj in View.SelectedObjects)
                    {
                        if (obj.Status == Distribution.LTStatus.PendingConsume)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "consumestatuserror"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        if (obj.ExpiryDate != null && obj.ExpiryDate != DateTime.MinValue && obj.ExpiryDate.Value.Date <= DateTime.Today)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ExpDateerror"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    List<string> list = new List<string>();
                    if (objIcmInfo.RollBackReason != null)
                    {
                        if (base.View != null && base.View.Id == "Distribution_ListView_ConsumptionViewmode")
                        {
                            foreach (Distribution obj in ((ListView)View).SelectedObjects)
                            {
                                obj.Status = Distribution.LTStatus.PendingConsume;
                                IObjectSpace os = Application.CreateObjectSpace();
                                ConsumptionHistory newobj = os.CreateObject<ConsumptionHistory>();
                                newobj.Distribution = os.GetObjectByKey<Distribution>(obj.Oid);
                                newobj.Consumed = false;
                                newobj.ConsumptionBy = null;
                                newobj.ConsumptionDate = null;
                                newobj.EnteredBy = null;
                                newobj.EnteredDate = null;
                                newobj.ReturnReason = objIcmInfo.RollBackReason;
                                newobj.ReturnBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                newobj.ReturnDate = DateTime.Now;
                                os.CommitChanges();
                                obj.ConsumptionBy = null;
                                obj.ConsumptionDate = null;
                                obj.EnteredBy = null;
                                obj.EnteredDate = null;
                                obj.Item.StockQty = obj.Item.StockQty + 1;
                                IObjectSpace objspace = Application.CreateObjectSpace();
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
                                else
                                {
                                    IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")"));
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
                                if (obj.ExpiryDate <= DateTime.Now.AddDays(7) && obj.ExpiryDate != null && obj.LT != null && obj.ExpiryDate != DateTime.MinValue.Date)
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
                                    if (obj.ExpiryDate <= DateTime.Now)
                                    {
                                        obj.Status = Distribution.LTStatus.PendingDispose;
                                    }
                                }
                            }
                            ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                            View.Refresh();
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
                                                //if (subchild.Id == "ConsumptionItems")
                                                //{
                                                //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                //    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("[ConsumptionBy] Is Null And [ConsumptionDate] Is Null And [Status] = 'PendingConsume' And ([ExpiryDate] Is Null or [ExpiryDate] > ?)", DateTime.Now));
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
                                                if (subchild.Id == "DisposalItems")
                                                {
                                                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                    //var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 'PendingDispose' OR [Status] = 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now));
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
                                            }
                                        }
                                    }
                                }
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "retunsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "returnreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
        #endregion
        private void PendingConsume_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                ((ListView)View).CollectionSource.Criteria.Clear();
                foreach (Distribution objrec in e.PopupWindowViewSelectedObjects)
                    objcons.Items.Add(objrec.LT);
                ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("LT", objcons.Items);
                objcons.rgMode = "Nothing";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PendingConsume_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                using (XPView view = new XPView())
                {
                    view.Properties.Add(new ViewProperty("ItemName", SortDirection.Ascending, "Item", true, true));
                    view.Properties.Add(new ViewProperty("VendorName", SortDirection.Ascending, "Vendor", true, true));
                    view.Properties.Add(new ViewProperty("TopOid", SortDirection.Ascending, "Max(Oid)", false, true));
                    List<object> groups = new List<object>();
                    foreach (ViewRecord rec in view)
                        groups.Add(rec["TopOid"]);
                    cs.Criteria["Distinct"] = new InOperator("Oid", groups);
                }
                ListView CreateListView = Application.CreateListView("Distribution_LookupListView", cs, false);
                CreateListView.Caption = "Consumption";
                e.View = CreateListView;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Consumeview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objspace, typeof(Distribution));
                //cs.Criteria["Filter"] = CriteriaOperator.Parse("DateDiffMonth(ReceiveDate, Now()) <= 3");
                Frame.SetView(Application.CreateListView("Distribution_ListView_ConsumptionViewmode", cs, false));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ConsumeDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //if (e.SelectedChoiceActionItem.Id == "1M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 1 And [ConsumptionDate] Is Not Null");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "3M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 3 And [ConsumptionDate] Is Not Null");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "6M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 6 And [ConsumptionDate] Is Not Null");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "1Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 1 And [ConsumptionDate] Is Not Null");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "2Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 2 And [ConsumptionDate] Is Not Null");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "5Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 5 And [ConsumptionDate] Is Not Null");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "ALL")
            //{         
            //    ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[ConsumptionDate] Is Not Null");
            //}
        }

        private void ConsumeDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && ConsumeDateFilter != null && ConsumeDateFilter.SelectedItem != null)
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (View.Id == "Distribution_ListView_ConsumptionViewmode")
                    {
                        if (strSelectedItem == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 1 And [ConsumptionDate] Is Not Null");
                        }
                        else if (strSelectedItem == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 3 And [ConsumptionDate] Is Not Null");
                        }
                        else if (strSelectedItem == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(ConsumptionDate, Now()) <= 6 And [ConsumptionDate] Is Not Null");
                        }
                        else if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 1 And [ConsumptionDate] Is Not Null");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 2 And [ConsumptionDate] Is Not Null");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffYear(ConsumptionDate, Now()) <= 5 And [ConsumptionDate] Is Not Null");
                        }
                        else if (strSelectedItem == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[ConsumptionDate] Is Not Null");
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
