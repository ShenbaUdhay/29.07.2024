using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;

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
    public partial class DisposalViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        ICMinfo objIcmInfo = new ICMinfo();
        ShowNavigationItemController ShowNavigationController;
        disposalquerypanelinfo objdis = new disposalquerypanelinfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        #endregion

        #region DefaultEvents
        public DisposalViewController()
        {
            InitializeComponent();
            TargetViewId = "Distribution_ListView_Disposal;" + "DisposalQuerypanel_DetailView;" + "Distribution_ListView_Disposal_Copy;";
            Disposal.TargetViewId = "Distribution_ListView_Disposal;";
            //Disposal.Category = "RecordEdit";
            //Disposal.Model.Index = 4;
            DisposalReturn.TargetViewId = "Distribution_ListView_Disposal;";
            //DisposalReturn.Category = "RecordEdit";
            //DisposalReturn.Model.Index = 7;
            DisposalQueryPanel.TargetViewId = "Distribution_ListView_Disposal";
            DisposalDateFilter.TargetViewId = "Distribution_ListView_Disposal";
            //DisposalDateFilter.Category = "RecordEdit";
            //DisposalDateFilter.Model.Index = 8;
            txtBarcodeActionDisposal.TargetViewId = "Distribution_ListView_Disposal";
            txtBarcodeActionDisposal.CustomizeControl += TxtBarcodeActionDisposal_CustomizeControl;

            //DisposalView.Category = "RecordEdit";
            //DisposalView.Model.Index = 5;
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Frame.GetController<DisposalViewController>().Actions["txtBarcodeActionDisposal"].Active.SetItemValue("", false);
                Frame.GetController<DisposalViewController>().Actions["DisposeQuerypanel"].Active.SetItemValue("", false);
                //Frame.GetController<DisposalViewController>().Actions["Dispose"].Active.SetItemValue("", true);
                Frame.GetController<DisposalViewController>().Actions["DisposalView"].Active.SetItemValue("", true);
                Frame.GetController<DisposalViewController>().Actions["DisposalDateFilter"].Active.SetItemValue("", false);
                txtBarcodeActionDisposal.ValueChanged += TxtBarcodeActionDisposal_ValueChanged;
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.DisposalIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.DisposalIsWrite = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "DisposalItems" && i.Write == true) != null)
                                {
                                    objPermissionInfo.DisposalIsWrite = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                //if (View.Id == "Distribution_ListView_Disposal")
                //{
                //    Disposal.Active.SetItemValue("Reportpreview.SaveReport", objPermissionInfo.CustomReportingIsWrite);
                //    DisposalReturn.Active.SetItemValue("Reportpreview.Comment", objPermissionInfo.CustomReportingIsWrite);
                //}

                Frame.GetController<DisposalViewController>().Actions["Dispose"].Active.SetItemValue("ShowDispose", objPermissionInfo.DisposalIsWrite);
                Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.SetItemValue("ShowDisposeReturn", objPermissionInfo.DisposalIsWrite);
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
                //        if (navPermissionDetails != null && View.Id == "Distribution_ListView_Disposal")
                //        {
                //            if (navPermissionDetails.Write == true)
                //            {
                //                Disposal.Active.SetItemValue("DisposalViewController.Disposal", true);
                //            }
                //            else if (navPermissionDetails.Write == false)
                //            {
                //                Disposal.Active.SetItemValue("DisposalViewController.Disposal", false);
                //            }
                //        }
                //    }
                //}
                if (View != null && View.CurrentObject != null && View.Id == "DisposalQuerypanel_DetailView")
                {
                    DisposalQuerypanel QPanel = (DisposalQuerypanel)View.CurrentObject;
                    objdis.rgMode = QPanel.Mode.ToString();
                }
                if (View.Id == "Distribution_ListView_Disposal")
                {

                    if (DisposalDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        DisposalDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        DisposalDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        DisposalDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        DisposalDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        DisposalDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        DisposalDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        DisposalDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //DisposalDateFilter.SelectedIndex = 1;
                    objdis.rgMode = ENMode.Enter.ToString();
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (DisposalDateFilter.SelectedItem == null)
                    {
                        if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                        {                           
                            DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[0];
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 1");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                        {
                            DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[1];
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 3");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                        {
                            DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[2];
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 6");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                        {
                            DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[3];
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 1");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                        {
                            DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[4];
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 2");
                        }
                        else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                        {
                            DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[5];
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 5");
                        }
                        else
                        {
                            DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedBy] Is Not Null And [DisposedDate] Is Not Null And [ExpiryDate] Is Not Null");
                        }                       
                    }
                    DisposalDateFilter.SelectedItemChanged += DisposalDateFilter_SelectedItemChanged;
                    if (objdis.rgMode == ENMode.Enter.ToString())
                    {
                        Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.SetItemValue("ShowDisposeReturn", false);
                    }


                    // ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 3");
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
                if (base.View != null && base.View.Id == "Distribution_ListView_Disposal" && objdis.rgMode != ENMode.View.ToString())
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 3 /*OR [Status] == 1*/) And [ExpiryDate] < ?", DateTime.Today);
                    //if (objdis.DisposalFilter == string.Empty && objIcmInfo.ObjectsToShow.Count == 0)
                    //{
                    //    if (objdis.rgMode == ENMode.Enter.ToString())
                    //    {
                    //        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 3 OR [Status] == 1) And [ExpiryDate] < ?", DateTime.Today);
                    //        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now);
                    //    }


                    //    //if(objdis.rgMode == ENMode.Enter.ToString() || objdis.rgMode == string.Empty)
                    //    //{
                    //    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now);
                    //    //}


                    //    //else
                    //    //{
                    //    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Emptygrid'");
                    //    //}


                    //    //Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.SetItemValue("", false);
                    //    //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReceiveID]==NULL");
                    //    //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Emptygrid'");
                    //}
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    if (gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                        gridListEditor.Grid.JSProperties["cpusername"] = user.DisplayName;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {
                    if((s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingConsume' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingDispose') && s.batchEditApi.GetCellValue(e.visibleIndex, 'ExpiryDate') < today){
                    if(e.focusedColumn.fieldName == 'DisposedDate' || e.focusedColumn.fieldName == 'DisposedBy.Oid'){
                    e.cancel = false;                  
                    }
                    else{
                    e.cancel = true;                  
                    }}
                    else{
                    e.cancel = true;                  
                    }
                    }";
                        gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) {               
                    if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) {
                    for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++){
                    if(s.batchEditApi.HasChanges(i)){                    
                    s.batchEditApi.SetCellValue(i, 'DisposedDate', null);
                    s.batchEditApi.SetCellValue(i, 'DisposedBy', null);
                    }}}
                    else if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0)){                   
                    var today = new Date();                                        
                    for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++){
                    if(s.batchEditApi.GetCellValue(i, 'DisposedDate') == null && s.batchEditApi.GetCellValue(i, 'DisposedBy') == null){
                    s.batchEditApi.SetCellValue(i, 'DisposedDate', today);          
                    s.batchEditApi.SetCellValue(i, 'DisposedBy', s.cpuserid, s.cpusername, false);                    
                    }}}
                    else{                    
                    if (s.IsRowSelectedOnPage(e.visibleIndex)) {                                       
                    var today = new Date();   
                    if(s.batchEditApi.GetCellValue(e.visibleIndex, 'DisposedDate') == null && s.batchEditApi.GetCellValue(e.visibleIndex, 'DisposedBy') == null){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DisposedDate', today);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DisposedBy', s.cpuserid, s.cpusername, false);
                    }}
                    else{
                    if(s.batchEditApi.HasChanges(e.visibleIndex)){
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DisposedDate', null);
                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DisposedBy', null);
                    }}}}";
                        //if((s.batchEditApi.GetCellValue(i, 'Status') == 'PendingConsume' || s.batchEditApi.GetCellValue(i, 'Status') == 'PendingDispose') && s.batchEditApi.GetCellValue(i, 'ExpiryDate') <= today){
                        //if(s.batchEditApi.GetCellValue(i, 'DisposedDate') == null && s.batchEditApi.GetCellValue(i, 'DisposedBy') == null){}
                    }
                }
                else if (View != null && View.Id == "Distribution_ListView_Disposal_Copy")
                {
                    if (objdis.rgMode == ENMode.View.ToString())
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedBy] Is Not Null And [DisposedDate] Is Not Null And [ExpiryDate] Is Not Null");
                    }
                    else
                    {
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedBy] Is Null And [DisposedDate] Is Null And [ExpiryDate] <= ?", DateTime.Now);
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 'PendingDispose' /*OR [Status] == 'PendingConsume*/') And [ExpiryDate] < ?", DateTime.Today);
                        //'Status') == 'PendingConsume' || s.batchEditApi.GetCellValue(e.visibleIndex, 'Status') == 'PendingDispose')
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
            txtBarcodeActionDisposal.ValueChanged -= TxtBarcodeActionDisposal_ValueChanged;
            ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
            if (Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.Contains("ShowDisposeReturn"))
            {
                Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.RemoveItem("ShowDisposeReturn");
                Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.SetItemValue("ShowDisposeReturn", false);
            }
            // ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("DateDiffMonth(DistributionDate, Now()) <= 3");


        }

        #endregion

        #region Events
        private void TxtBarcodeActionDisposal_CustomizeControl(object sender, CustomizeControlEventArgs e)
        {
            //ParametrizedActionMenuActionItem actionItem = e.Control as ParametrizedActionMenuActionItem;
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
        private void TxtBarcodeActionDisposal_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcodeActionDisposal.Value != null && txtBarcodeActionDisposal.Value.ToString() != string.Empty)
                {
                    string TextValue = txtBarcodeActionDisposal.Value.ToString();
                    Distribution objConsumption = ObjectSpace.FindObject<Distribution>(CriteriaOperator.Parse("[LT]='" + TextValue + "' AND [ExpiryDate] < ?", DateTime.Today));
                    if (objConsumption != null)
                    {
                        if (View is ListView)
                        {
                            if (!objIcmInfo.ObjectsToShow.Contains(new Guid(objConsumption.Oid.ToString())))
                                objIcmInfo.ObjectsToShow.Add(new Guid(objConsumption.Oid.ToString()));
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
                if (base.View != null && base.View.Id == "DisposalQuerypanel_DetailView")
                {
                    if (View != null && View.CurrentObject == e.Object && e.PropertyName == "LT")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(DisposalQuerypanel))
                        {
                            DisposalQuerypanel CPanel = (DisposalQuerypanel)e.Object;
                            if (CPanel.LT != null)
                            {
                                if (CPanel.Mode.Equals(ENMode.View))
                                {
                                    objdis.DisposalFilter = "[LT] == '" + CPanel.LT.LT + "'";
                                }
                                else
                                {
                                    objdis.DisposalFilter = "[LT] == '" + CPanel.LT.LT + "'";
                                }
                            }
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Mode")
                    {
                        if (View.ObjectTypeInfo.Type == typeof(DisposalQuerypanel))
                        {
                            DisposalQuerypanel CPanel = (DisposalQuerypanel)e.Object;
                            if (CPanel != null)
                            {
                                objdis.rgMode = CPanel.Mode.ToString();
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
        private void Disposal_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count >= 1)
                {
                    if (base.View != null && base.View.Id == "Distribution_ListView_Disposal")
                    {
                        foreach (Distribution obj in View.SelectedObjects)
                        {
                            if (obj.Status == Distribution.LTStatus.Disposed)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalid"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                        View.ObjectSpace.Refresh();
                        View.Refresh();
                        foreach (Distribution obj in View.SelectedObjects)
                        {
                            IObjectSpace objspace = Application.CreateObjectSpace();
                            obj.Item.StockQty = obj.Item.StockQty - 1;
                            obj.Status = Distribution.LTStatus.Disposed;

                            DisposalHistory objdisp = objspace.CreateObject<DisposalHistory>();
                            objdisp.Distribution = objspace.GetObjectByKey<Distribution>(obj.Oid);
                            if (obj.DisposedBy!=null)
                            {
                            objdisp.DisposedBy = objspace.GetObjectByKey<Employee>(obj.DisposedBy.Oid);
                            }
                            objdisp.DisposedDate = obj.DisposedDate;

                            IList<ICMAlert> alertlist = objspace.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + obj.LT));
                            if (alertlist != null)
                            {
                                foreach (ICMAlert item in alertlist)
                                {
                                    item.AlarmTime = null;
                                    item.RemindIn = null;
                                }
                            }

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
                        ObjectSpace.CommitChanges();
                        NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                        module.ShowNotificationsWindow = false;
                        module.NotificationsService.Refresh();
                        if (objIcmInfo.ObjectsToShow != null && objIcmInfo.ObjectsToShow.Count == 0)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 3 /*OR [Status] == 1*/) And [ExpiryDate] < ?", DateTime.Today);
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now);
                            // ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Emptygrid'");
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReceiveID]==NULL");
                        }
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("xml", "sessionStorage.clear();");
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
                                    else if (child.Id == "Alert")
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
                                            else if (subchild.Id == "Expiration Alert")
                                            {
                                                DateTime TodayDate = DateTime.Now;
                                                TodayDate = TodayDate.AddDays(7);
                                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                //var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", TodayDate));
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
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "disposesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        View.RefreshDataSource();
                    }
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
        }
        private void DisposalReturn_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    foreach (Distribution obj in View.SelectedObjects)
                    {
                        if (obj.Status == Distribution.LTStatus.PendingConsume || obj.Status == Distribution.LTStatus.PendingDispose)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Invalid"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    if (objIcmInfo.RollBackReason != null)
                    {
                        if (base.View != null && base.View.Id == "Distribution_ListView_Disposal")
                        {
                            foreach (Distribution obj in View.SelectedObjects)
                            {
                                obj.DisposedBy = null;
                                obj.DisposedDate = null;
                                obj.Item.StockQty = obj.Item.StockQty + 1;
                                obj.Status = Distribution.LTStatus.PendingDispose;

                                IObjectSpace os = Application.CreateObjectSpace();
                                DisposalHistory objdishis = os.CreateObject<DisposalHistory>();
                                objdishis.Distribution = os.GetObjectByKey<Distribution>(obj.Oid);
                                objdishis.DisposedBy = null;
                                objdishis.DisposedDate = null;
                                objdishis.ReturnReason = objIcmInfo.RollBackReason;
                                objdishis.ReturnBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId); ;
                                objdishis.ReturnDate = DateTime.Now;

                                if (obj.ExpiryDate > DateTime.Now)
                                {
                                    obj.Status = Distribution.LTStatus.PendingConsume;
                                    obj.ConsumptionBy = null;
                                    obj.ConsumptionDate = null;
                                    obj.EnteredBy = null;
                                    obj.EnteredDate = null;

                                    ConsumptionHistory objconhis = os.CreateObject<ConsumptionHistory>();
                                    objconhis.Distribution = os.GetObjectByKey<Distribution>(obj.Oid);
                                    objconhis.Consumed = false;
                                    objconhis.ConsumptionBy = null;
                                    objconhis.ConsumptionDate = null;
                                    objconhis.EnteredBy = null;
                                    objconhis.EnteredDate = null;
                                    objconhis.ReturnReason = objIcmInfo.RollBackReason;
                                    objconhis.ReturnBy = ((XPObjectSpace)(os)).Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    objconhis.ReturnDate = DateTime.Now;
                                }
                                os.CommitChanges();
                                IObjectSpace space = Application.CreateObjectSpace(typeof(ICMAlert));
                                if (obj.ExpiryDate <= DateTime.Now.AddDays(7) && obj.ExpiryDate != null && obj.ExpiryDate != DateTime.MinValue.Date)
                                {
                                    ICMAlert objdisp = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Expiry Alert - " + obj.LT));
                                    if (objdisp == null)
                                    {
                                        ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                        obj1.Subject = "Expiry Alert - " + obj.LT;
                                        obj1.StartDate = DateTime.Now;
                                        obj1.DueDate = DateTime.Now.AddDays(7);
                                        obj1.RemindIn = TimeSpan.FromMinutes(5);
                                        obj1.Description = "Nice";
                                    }
                                }
                                else
                                {
                                    IList<ICMAlert> alertlist = space.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Expiry Alert - " + obj.LT));
                                    if (alertlist != null)
                                    {
                                        foreach (ICMAlert item in alertlist)
                                        {
                                            item.AlarmTime = null;
                                            item.RemindIn = null;
                                        }
                                    }
                                }
                                if (obj.Item.StockQty <= obj.Item.AlertQty)
                                {
                                    ICMAlert objdisp1 = space.FindObject<ICMAlert>(CriteriaOperator.Parse("[Subject] = ? AND [AlarmTime] Is Not Null And [RemindIn] Is Not Null", "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")"));
                                    if (objdisp1 == null)
                                    {
                                        ICMAlert obj1 = space.CreateObject<ICMAlert>();
                                        obj1.Subject = "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")";
                                        obj1.StartDate = DateTime.Now;
                                        obj1.DueDate = DateTime.Now.AddDays(7);
                                        obj1.RemindIn = TimeSpan.FromMinutes(5);
                                        obj1.Description = "Nice";
                                    }
                                }
                                else
                                {
                                    IList<ICMAlert> alertlist = space.GetObjects<ICMAlert>(CriteriaOperator.Parse("[Subject] = ?", "Low Stock - " + obj.Item.items + "(" + obj.Item.ItemCode + ")"));
                                    if (alertlist != null)
                                    {
                                        foreach (ICMAlert item in alertlist)
                                        {
                                            item.AlarmTime = null;
                                            item.RemindIn = null;
                                        }
                                    }
                                }
                                space.CommitChanges();
                            }
                            ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "retunsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            View.ObjectSpace.Refresh();
                            View.Refresh();
                            NotificationsModule module = this.Application.Modules.FindModule<NotificationsModule>();
                            module.ShowNotificationsWindow = false;
                            module.NotificationsService.Refresh();
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now);
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Emptygrid'");
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReceiveID]==NULL");
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
                                                    var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 3 OR [Status] = 1) And [ExpiryDate] < ?", DateTime.Today));
                                                    //var count = objectSpace.GetObjectsCount(typeof(Distribution), CriteriaOperator.Parse("([Status] = 'PendingDispose' OR [Status] = 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now));
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
                                                //if (subchild.Id == "Stock Watch")
                                                //{
                                                //    IObjectSpace objectSpace = Application.CreateObjectSpace();
                                                //    var count = objectSpace.GetObjectsCount(typeof(Items), CriteriaOperator.Parse("[StockQty] > 0"));
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
                                                else if (subchild.Id == "Expiration Alert")
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
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "returnreason"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
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
        private void DisposalQueryPanel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                object objToShow = objspace.CreateObject(typeof(DisposalQuerypanel));
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
        private void DisposalQueryPanel_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (objIcmInfo.ObjectsToShow.Count > 0)
                {
                    objIcmInfo.ObjectsToShow.Clear();
                }
                ((ListView)View).CollectionSource.Criteria.Clear();
                int RowCount = 0;
                if (View != null && View.Id == "Distribution_ListView_Disposal")
                {
                    if (objdis.DisposalFilter == string.Empty)
                    {
                        //if(objdis.rgMode == ENMode.Enter.ToString())
                        //{
                        //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("([Status] == 'PendingDispose' OR [Status] == 'PendingConsume') And [ExpiryDate] <= ?", DateTime.Now);
                        //}
                        //else
                        //{
                        //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedBy] Is Not Null And [DisposedDate] Is Not Null And [ExpiryDate] Is Not Null");
                        //}
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 'Emptygrid'");
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status] = 6");
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReceiveID]==NULL");
                    }
                    else if (objdis.DisposalFilter != string.Empty)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse(objdis.DisposalFilter);
                        RowCount = ((ListView)View).CollectionSource.GetCount();
                        //if (objdis.rgMode == ENMode.View.ToString())
                        //{
                        //    Frame.GetController<DisposalViewController>().Actions["Dispose"].Active.SetItemValue("", false);
                        //    Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.SetItemValue("", true);
                        //}
                        //else
                        //{
                        //    Frame.GetController<DisposalViewController>().Actions["Dispose"].Active.SetItemValue("", true);
                        //    Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.SetItemValue("", false);
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

        private void DisposalReturn_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
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
                    CreateDetailView.Caption = "Dispose Return";
                    e.View = CreateDetailView;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        #endregion

        private void DisposalView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Frame.GetController<DisposalViewController>().Actions["Dispose"].Active.SetItemValue("ShowDispose", false);
                Frame.GetController<DisposalViewController>().Actions["DisposalView"].Active.SetItemValue("", false);
                Frame.GetController<DisposalViewController>().Actions["DisposeReturn"].Active.SetItemValue("ShowDisposeReturn", objPermissionInfo.DisposalIsWrite);
                Frame.GetController<DisposalViewController>().Actions["DisposalDateFilter"].Active.SetItemValue("", true);
                objdis.rgMode = ENMode.View.ToString();
                DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));

                if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                {
                    DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[0];
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 1");
                }
                else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                {
                    DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[1];
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 3");
                }
                else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                {
                    DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[2];
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 6");
                }
                else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                {
                    DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[3];
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 1");
                }
                else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                {
                    DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[4];
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 2");
                }
                else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                {
                    DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[5];
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 5");
                }
                else
                {
                    DisposalDateFilter.SelectedItem = DisposalDateFilter.Items[6];
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And [DisposedDate] Is Not Null And [ExpiryDate] Is Not Null");
                }
                //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 3");
                //DisposalDateFilter.SelectedIndex = 1;
                //DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                //if (setting.InventoryWorkFlow == EnumDateFilter.OneMonth)
                //{
                //    DisposalDateFilter.SelectedIndex = 0;
                //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 1");
                //}
                //else if (setting.InventoryWorkFlow == EnumDateFilter.ThreeMonth)
                //{
                //    DisposalDateFilter.SelectedIndex = 1;
                //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 3");
                //}
                //else if (setting.InventoryWorkFlow == EnumDateFilter.SixMonth)
                //{
                //    DisposalDateFilter.SelectedIndex = 2;
                //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 6");
                //}
                //else if (setting.InventoryWorkFlow == EnumDateFilter.OneYear)
                //{
                //    DisposalDateFilter.SelectedIndex = 3;
                //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 1");
                //}
                //else if (setting.InventoryWorkFlow == EnumDateFilter.TwoYear)
                //{
                //    DisposalDateFilter.SelectedIndex = 4;
                //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 2");
                //}
                //else if (setting.InventoryWorkFlow == EnumDateFilter.FiveYear)
                //{
                //    DisposalDateFilter.SelectedIndex = 5;
                //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 5");
                //}
                //else if (setting.InventoryWorkFlow == EnumDateFilter.All)
                //{
                //    DisposalDateFilter.SelectedIndex = 6;
                //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedBy] Is Not Null And [DisposedDate] Is Not Null And [ExpiryDate] Is Not Null");
                //}
                //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 3");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DisposalDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            //if (e.SelectedChoiceActionItem.Id == "1M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 1");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "3M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 3");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "6M")
            //{
            //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 6");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "1Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 1");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "2Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 2");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "5Y")
            //{
            //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 5");
            //}
            //else if (e.SelectedChoiceActionItem.Id == "ALL")
            //{
            //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedBy] Is Not Null And [DisposedDate] Is Not Null And [ExpiryDate] Is Not Null");
            //}
        }

        private void DisposalDateFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && DisposalDateFilter != null && DisposalDateFilter.SelectedItem != null)
                {
                    string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                    if (View.Id == "Distribution_ListView_Disposal" && objdis.rgMode == ENMode.View.ToString())
                    {
                        if (strSelectedItem == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 3");
                        }
                        else if (strSelectedItem == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffMonth(DisposedDate, Now()) <= 6");
                        }
                        else if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 1");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 2");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And DateDiffYear(DisposedDate, Now()) <= 5");
                        }
                        else if (strSelectedItem == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[DisposedDate] Is Not Null And [DisposedDate] Is Not Null And [ExpiryDate] Is Not Null");
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
