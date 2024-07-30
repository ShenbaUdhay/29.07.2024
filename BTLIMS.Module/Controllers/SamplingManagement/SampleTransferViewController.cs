using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using View = DevExpress.ExpressApp.View;

namespace LDM.Module.Controllers.SamplingManagement
{
    public partial class SampleTransferViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        AuditInfo objAuditInfo = new AuditInfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        ModificationsController modificationsController;
        AppearanceController appearanceController;
        public SampleTransferViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleBottleAllocation_ListView_SampleTransferMain;"
                + "SampleBottleAllocation_ListView_SampleTransferMain_History;"
                + "SampleBottleAllocation_DetailView_SampleTransfer;"
                + "SampleBottleAllocation_ListView_SampleTransfer;"
                + "SampleBottleAllocation_ListView_SampleTransfer_Selected;";
            Submit.TargetViewId = "SampleBottleAllocation_DetailView_SampleTransfer";
            Rollback.TargetViewId = "SampleBottleAllocation_DetailView_SampleTransfer";
            Submit.Execute += Submit_Execute;
            Rollback.Execute += Rollback_Execute;
            SimpleAction STHistory = new SimpleAction(this, "STHistory", DevExpress.Persistent.Base.PredefinedCategory.View)
            {
                Caption = "History",
            };
            STHistory.TargetViewId = "SampleBottleAllocation_ListView_SampleTransferMain";
            STHistory.Execute += STHistory_Execute;
            STHistory.ImageName = "Action_Search";
            STDelete.TargetViewId = "SampleBottleAllocation_DetailView_SampleTransfer";
            STDelete.Execute += STDelete_Execute;
            STDelete.ImageName = "Action_Delete";
            STFilter.TargetViewId = "SampleBottleAllocation_ListView_SampleTransferMain";
        }

        private void STHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objspace = Application.CreateObjectSpace();
            CollectionSource cs = new CollectionSource(objspace, typeof(SampleBottleAllocation));
            ListView createListview = Application.CreateListView("SampleBottleAllocation_ListView_SampleTransferMain_History", cs, true);
            Frame.SetView(createListview);
        }

        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "SampleBottleAllocation_ListView_SampleTransferMain" || View.Id == "SampleBottleAllocation_ListView_SampleTransferMain_History")
                {
                    if (STFilter != null && STFilter.SelectedItem == null && View.Id == "SampleBottleAllocation_ListView_SampleTransferMain")
                    {
                        DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                        if (STFilter.SelectedItem == null)
                        {
                            if (setting.SampleTransfer == EnumDateFilter.OneMonth)
                            {
                                STFilter.SelectedItem = STFilter.Items[0];
                            }
                            else if (setting.SampleTransfer == EnumDateFilter.ThreeMonth)
                            {
                                STFilter.SelectedItem = STFilter.Items[1];
                            }
                            else if (setting.SampleTransfer == EnumDateFilter.SixMonth)
                            {
                                STFilter.SelectedItem = STFilter.Items[2];
                            }
                            else if (setting.SampleTransfer == EnumDateFilter.OneYear)
                            {
                                STFilter.SelectedItem = STFilter.Items[3];
                            }
                            else if (setting.SampleTransfer == EnumDateFilter.TwoYear)
                            {
                                STFilter.SelectedItem = STFilter.Items[4];
                            }
                            else if (setting.SampleTransfer == EnumDateFilter.FiveYear)
                            {
                                STFilter.SelectedItem = STFilter.Items[5];
                            }
                            else
                            {
                                STFilter.SelectedItem = STFilter.Items[6];
                            }
                        }
                        STFilter.SelectedItemChanged += STFilter_SelectedItemChanged;
                    }

                    ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listProcessController != null)
                    {
                        listProcessController.CustomProcessSelectedItem += ListProcessController_CustomProcessSelectedItem;
                    }

                    List<object> OidTask = new List<object>();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleBottleAllocation)))
                    {
                        if (View.Id == "SampleBottleAllocation_ListView_SampleTransferMain")
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[SampleRegistration.JobID.IsSampling] = True And ([SampleTransferStatus] is Null Or [SampleTransferStatus] <> 'Submitted')");
                        }
                        else
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[SampleRegistration.JobID.IsSampling] = True And [SampleTransferStatus] = 'Submitted'");
                        }
                        lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "SampleRegistration.JobID.Oid", true, true));
                        lstview.Properties.Add(new ViewProperty("BroughtBy", SortDirection.Ascending, "BroughtBy.Oid", true, true));
                        lstview.Properties.Add(new ViewProperty("Status", SortDirection.Ascending, "SampleTransferStatus", true, true));
                        lstview.Properties.Add(new ViewProperty("TopOid", SortDirection.Ascending, "Max(Oid)", false, true));
                        foreach (ViewRecord Vrec in lstview)
                            OidTask.Add(Vrec["TopOid"]);
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);
                }
                else if (View.Id == "SampleBottleAllocation_ListView_SampleTransfer")
                {
                    SampleBottleAllocation samplebottle = (SampleBottleAllocation)Application.MainWindow.View.CurrentObject;
                    if (samplebottle != null)
                    {
                        objAuditInfo.currentViewOid = samplebottle.SampleRegistration.JobID.Oid;
                        List<object> OidTask = new List<object>();
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleBottleAllocation)))
                        {
                            if (samplebottle.SampleTransferStatus.ToString() == "PendingTransfer")
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[SampleRegistration.JobID.Oid]=? And ([SampleTransferStatus] is Null Or [SampleTransferStatus]= 'PendingTransfer')", samplebottle.SampleRegistration.JobID.Oid);
                            }
                            else
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[SampleRegistration.JobID.Oid]=? And [SampleTransferStatus]=? And [BroughtBy.Oid]=?", samplebottle.SampleRegistration.JobID.Oid, samplebottle.SampleTransferStatus, samplebottle.BroughtBy.Oid);
                            }
                            lstview.Properties.Add(new ViewProperty("SampleID", SortDirection.Ascending, "SampleRegistration.Oid", true, true));
                            lstview.Properties.Add(new ViewProperty("BottleID", SortDirection.Ascending, "BottleID", true, true));
                            lstview.Properties.Add(new ViewProperty("TopOid", SortDirection.Ascending, "Max(Oid)", false, true));
                            foreach (ViewRecord Vrec in lstview)
                                OidTask.Add(Vrec["TopOid"]);
                        }
                        ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);
                    }
                }
                else if (View.Id == "SampleBottleAllocation_ListView_SampleTransfer_Selected")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                }
                else if (View.Id == "SampleBottleAllocation_DetailView_SampleTransfer")
                {
                    Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                    Rollback.Active.SetItemValue("valRollback", false);
                    Submit.Active.SetItemValue("valSubmit", false);
                    STDelete.Active.SetItemValue("valSTDelete", false);
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        Rollback.Active.SetItemValue("valRollback", true);
                        Submit.Active.SetItemValue("valSubmit", true);
                        STDelete.Active.SetItemValue("valSTDelete", true);
                    }
                    else
                    {
                        if (objnavigationRefresh.ClickedNavigationItem == "SampleTransfer")
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SampleTransfer" && i.Write == true) != null)
                                {
                                    Rollback.Active.SetItemValue("valRollback", true);
                                    Submit.Active.SetItemValue("valSubmit", true);
                                    STDelete.Active.SetItemValue("valSTDelete", true);
                                }
                            }
                        }
                    }
                    SampleBottleAllocation samplebottle = (SampleBottleAllocation)View.CurrentObject;
                    if (samplebottle.SampleTransferStatus.ToString() == "Submitted")
                    {
                        Rollback.Active.SetItemValue("enb", true);
                        Submit.Active.SetItemValue("enb", false);
                        Frame.GetController<WebModificationsController>().EditAction.Active.SetItemValue("enb", false);
                    }
                    else
                    {
                        Submit.Active.SetItemValue("enb", true);
                        Rollback.Active.SetItemValue("enb", false);
                    }
                    if (samplebottle.SampleTransferStatus.ToString() == "PendingTransfer")
                    {
                        STDelete.Active.SetItemValue("enb", false);
                    }
                    else
                    {
                        STDelete.Active.SetItemValue("enb", true);
                    }
                    modificationsController = Frame.GetController<ModificationsController>();
                    if (modificationsController != null)
                    {
                        modificationsController.SaveAction.Executing += SaveAction_Executing;
                    }
                    appearanceController = Frame.GetController<AppearanceController>();
                    if (appearanceController != null)
                    {
                        appearanceController.CustomApplyAppearance += AppearanceController_CustomApplyAppearance;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AppearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {
                    SampleBottleAllocation sample = (SampleBottleAllocation)View.CurrentObject;
                    if (e.ItemName == "SelectedSampleGrid")
                    {
                        if (sample.SampleTransferStatus == SamplingTransferStatus.PendingTransfer)
                        {
                            e.AppearanceObject.Visibility = ViewItemVisibility.Show;
                        }
                        else
                        {
                            e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
                        }
                    }
                    else if (e.ItemName == "ScanBottle")
                    {
                        if (sample.SampleTransferStatus == SamplingTransferStatus.PendingTransfer)
                        {
                            e.AppearanceObject.Visibility = ViewItemVisibility.Show;
                        }
                        else
                        {
                            e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
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

        private void STFilter_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime srDateFilter = DateTime.MinValue;
                if (STFilter != null && STFilter.SelectedItem != null)
                {
                    if (STFilter.SelectedItem.Id == "1M")
                    {
                        srDateFilter = DateTime.Today.AddMonths(-1);
                    }
                    else if (STFilter.SelectedItem.Id == "3M")
                    {
                        srDateFilter = DateTime.Today.AddMonths(-3);
                    }
                    else if (STFilter.SelectedItem.Id == "6M")
                    {
                        srDateFilter = DateTime.Today.AddMonths(-6);
                    }
                    else if (STFilter.SelectedItem.Id == "1Y")
                    {
                        srDateFilter = DateTime.Today.AddYears(-1);
                    }
                    else if (STFilter.SelectedItem.Id == "2Y")
                    {
                        srDateFilter = DateTime.Today.AddYears(-2);
                    }
                    else if (STFilter.SelectedItem.Id == "5Y")
                    {
                        srDateFilter = DateTime.Today.AddYears(-5);
                    }
                }
                if (srDateFilter != DateTime.MinValue)
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("SampleRegistration.JobID.CreatedDate >= ?", srDateFilter);
                }
                else
                {
                    ((ListView)View).CollectionSource.Criteria.Remove("Filter");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ListProcessController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject.GetType() == typeof(SampleBottleAllocation))
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    SampleBottleAllocation bottle = os.GetObjectByKey<SampleBottleAllocation>(((SampleBottleAllocation)e.InnerArgs.CurrentObject).Oid);
                    if (bottle != null)
                    {
                        if (bottle.BroughtBy == null)
                        {
                            bottle.CoolerId = "1";
                            bottle.ReceivedDate = DateTime.Now;
                            bottle.ReceivedBy = os.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        DetailView CreateDetailView = Application.CreateDetailView(os, "SampleBottleAllocation_DetailView_SampleTransfer", true, bottle);
                        CreateDetailView.ViewEditMode = View.Caption.Contains("History") ? ViewEditMode.View : ViewEditMode.Edit;
                        CreateDetailView.Caption = View.Caption.Contains("History") ? "Sample Transfer History" : "Sample Transfer";
                        Frame.SetView(CreateDetailView);
                        e.Handled = true;
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
                if (View.Id == "SampleBottleAllocation_ListView_SampleTransfer")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        SampleBottleAllocation sample = (SampleBottleAllocation)Application.MainWindow.View.CurrentObject;
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("Remark", this);
                        editor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        editor.Grid.HtmlRowPrepared += Grid_HtmlRowPrepared;
                        if (sample.SampleTransferStatus.ToString() != "Submitted")
                        {
                            editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            if (sample.SampleTransferStatus.ToString() == "PendingTransfer")
                            {
                                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                                editor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                                editor.Grid.JSProperties["cpusername"] = currentUser.DisplayName;
                                editor.Grid.JSProperties["cpcurdatetime"] = DateTime.Now;
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) == null)
                                {
                                    GridViewColumn column = editor.Grid.Columns.Cast<GridViewColumn>().Where(a => a.Name == "SelectionCommandColumn").FirstOrDefault();
                                    if (column != null)
                                    {
                                        column.Visible = false;
                                    }
                                }
                                editor.Grid.ClientSideEvents.SelectionChanged = @"function(s, e) 
                                {           
                                    //var today = new Date();   
                                    var today = s.cpcurdatetime;
                                    var scan = sessionStorage.getItem('Scantriggered');
                                    if (e.visibleIndex == -1 && s.GetSelectedRowCount() > 0 && scan != null && scan == 'true')
                                    {                                        
                                        for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                        {
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {  
                                                s.batchEditApi.SetCellValue(i, 'ScanDateTime', today);
                                                s.batchEditApi.SetCellValue(i, 'ReceivedDate', today);
                                                s.batchEditApi.SetCellValue(i, 'ReceivedBy.Oid', s.cpuserid, s.cpusername, false);
                                            }
                                        }
                                        sessionStorage.setItem('Scantriggered', false);  
                                    }                                    
                                    else if (e.visibleIndex == -1 && s.GetSelectedRowCount() == 0) 
                                    {
                                        for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                        {
                                            s.batchEditApi.SetCellValue(i, 'ScanDateTime', null);
                                            s.batchEditApi.SetCellValue(i, 'ReceivedDate', null);                  
                                            s.batchEditApi.SetCellValue(i, 'ReceivedBy.Oid', null);                                      
                                        }
                                    }                           
                                    else if (e.visibleIndex == -1 && s.GetSelectedRowCount() == s.GetVisibleRowsOnPage())
                                    {                                        
                                        for (var i = 0 ; i <= s.GetVisibleRowsOnPage() - 1; i++)
                                        {
                                            s.batchEditApi.SetCellValue(i, 'ReceivedDate', today);
                                            s.batchEditApi.SetCellValue(i, 'ReceivedBy.Oid', s.cpuserid, s.cpusername, false);
                                        }
                                    }                                    
                                    else
                                    {
                                        if (s.IsRowSelectedOnPage(e.visibleIndex)) 
                                        {                                                                               
                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedDate', today);
                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy.Oid', s.cpuserid, s.cpusername, false);
                                        } 
                                        else 
                                        {
                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'ScanDateTime', null);
                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedDate', null);                   
                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'ReceivedBy.Oid', null);                   
                                        }                             
                                    }
                                    RaiseXafCallback(globalCallbackControl, 'Remark', 'Selection', '', false);
                                }";
                            }
                            else
                            {
                                editor.CanSelectRows = false;
                            }
                        }
                        else
                        {
                            editor.AllowEdit = false;
                            editor.CanSelectRows = false;
                        }
                    }
                }
                else if (View.Id == "SampleBottleAllocation_ListView_SampleTransfer_Selected")
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) { e.cancel = true; }";
                        editor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    }
                }
                else if (View.Id == "SampleBottleAllocation_DetailView_SampleTransfer")
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                if (item.Id == "ScanBottle")
                                {
                                    ASPxTextBox textBox = (ASPxTextBox)propertyEditor.Editor;
                                    if (textBox != null)
                                    {
                                        ICallbackManagerHolder holder = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                                        holder.CallbackManager.RegisterHandler("ScanBottle", this);
                                        textBox.ClientSideEvents.ValueChanged = @"function(s,e) { 
                                            if(s.GetText().length > 0)
                                            {
                                            sessionStorage.setItem('Scantriggered', true);  
                                            RaiseXafCallback(globalCallbackControl, 'ScanBottle', 'ScanBottle|' + s.GetText(), '', false); 
                                            }
                                        }";
                                    }
                                }
                            }
                            else if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                            else if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                            else if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
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
                            else if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                {
                                    propertyEditor.FindEdit.Editor.BackColor = Color.LightYellow;
                                }
                                else if (propertyEditor.DropDownEdit != null)
                                {
                                    propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                            if (propertyEditor != null && propertyEditor.DropDownEdit != null && propertyEditor.DropDownEdit.DropDown != null)
                            {
                                propertyEditor.DropDownEdit.DropDown.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                            else if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                        {
                            ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                            else if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
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

        private void Grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                ASPxGridView grid = sender as ASPxGridView;
                if (e.RowType == GridViewRowType.Data)
                {
                    SampleStatus status = (SampleStatus)grid.GetRowValues(e.VisibleIndex, "SampleStatus");
                    if (status != null)
                    {
                        if (status.Samplestatus == "Uncollected")
                        {
                            e.Row.BackColor = Color.Orange;
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

        private void Grid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (e.DataColumn.FieldName == "IsRemark")
                {
                    e.Cell.Attributes.Add("onclick", "RaiseXafCallback(globalCallbackControl, 'Remark'," + e.VisibleIndex + " , '', false);");
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
                if (View.Id == "SampleBottleAllocation_ListView_SampleTransferMain" || View.Id == "SampleBottleAllocation_ListView_SampleTransferMain_History")
                {
                    ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listProcessController != null)
                    {
                        listProcessController.CustomProcessSelectedItem -= ListProcessController_CustomProcessSelectedItem;
                    }
                }
                else if (View.Id == "SampleBottleAllocation_DetailView_SampleTransfer")
                {
                    modificationsController = Frame.GetController<ModificationsController>();
                    if (modificationsController != null)
                    {
                        modificationsController.SaveAction.Executing -= SaveAction_Executing;
                    }
                    if (appearanceController != null)
                    {
                        appearanceController.CustomApplyAppearance -= AppearanceController_CustomApplyAppearance;
                    }
                }
                if (View.Id == "SampleBottleAllocation_ListView_SampleTransferMain")
                {
                    STFilter.SelectedItemChanged -= STFilter_SelectedItemChanged;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void cleargrid(View view)
        {
            foreach (SampleBottleAllocation samples in ((ListView)view).CollectionSource.List.Cast<SampleBottleAllocation>().ToList())
            {
                ((ListView)view).CollectionSource.Remove(samples);
            }
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    if (parameter == "Selection")
                    {
                        SampleBottleAllocation sample = (SampleBottleAllocation)Application.MainWindow.View.CurrentObject;
                        if (sample != null)
                        {
                            sample.STNoSamples = View.SelectedObjects.Cast<SampleBottleAllocation>().Select(a => a.SampleRegistration.Oid).Distinct().Count();
                            sample.STNoContainers = View.SelectedObjects.Count;
                            DashboardViewItem viewItem = ((DetailView)Application.MainWindow.View).FindItem("SampleGrid") as DashboardViewItem;
                            DashboardViewItem selviewItem = ((DetailView)Application.MainWindow.View).FindItem("SelectedSampleGrid") as DashboardViewItem;
                            if (viewItem != null && viewItem.InnerView != null && selviewItem != null && selviewItem.InnerView != null)
                            {
                                cleargrid(selviewItem.InnerView);
                                foreach (SampleBottleAllocation samples in ((ListView)viewItem.InnerView).SelectedObjects)
                                {
                                    if (string.IsNullOrEmpty(samples.CoolerId))
                                    {
                                        samples.CoolerId = sample.CoolerId;
                                        samples.CoolerTemp = sample.CoolerTemp;
                                    }
                                    ((ListView)selviewItem.InnerView).CollectionSource.Add(selviewItem.InnerView.ObjectSpace.GetObjectByKey<SampleBottleAllocation>(samples.Oid));
                                }
                                ((ListView)selviewItem.InnerView).Refresh();
                            }
                        }
                    }
                    else if (parameter.Contains("ScanBottle"))
                    {
                        SampleBottleAllocation sample = (SampleBottleAllocation)Application.MainWindow.View.CurrentObject;
                        if (sample != null)
                        {
                            DashboardViewItem viewItem = ((DetailView)Application.MainWindow.View).FindItem("SampleGrid") as DashboardViewItem;
                            if (viewItem != null && viewItem.InnerView != null)
                            {
                                parameter = parameter.Split('|')[1];
                                ASPxGridListEditor gridView = (ASPxGridListEditor)((ListView)viewItem.InnerView).Editor;
                                if (gridView != null && gridView.Grid != null && !string.IsNullOrEmpty(parameter))
                                {
                                    string sampleid = parameter.Substring(0, parameter.Length - 2);
                                    string Bottleid = parameter.Substring(parameter.Length - 1);
                                    SampleBottleAllocation sampleallocation = ((ListView)viewItem.InnerView).CollectionSource.List.Cast<SampleBottleAllocation>().Where(a => a.SampleRegistration.SampleID == sampleid && a.BottleID == Bottleid).FirstOrDefault();
                                    if (sampleallocation != null)
                                    {
                                        gridView.Grid.Selection.SelectRowByKey(sampleallocation.Oid.ToString());
                                        sample.NPScanBottle = null;
                                    }
                                    else
                                    {
                                        WebWindow.CurrentRequestWindow.RegisterClientScript("sam", "sessionStorage.setItem('Scantriggered', false);");
                                        Application.ShowViewStrategy.ShowMessage("Entered sample label does not exist.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            SampleBottleAllocation obj = View.ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid]=?", gridListEditor.Grid.GetRowValues(int.Parse(parameter), "Oid")), true);
                            if (obj != null)
                            {
                                DetailView dv = Application.CreateDetailView(View.ObjectSpace, "SampleBottleAllocation_DetailView_SampleTransfer_Remark", false, obj);
                                dv.ViewEditMode = obj.SampleTransferStatus == SamplingTransferStatus.Submitted ? ViewEditMode.View : ViewEditMode.Edit;
                                dv.Caption = "Enter Remark";
                                ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.CloseOnCurrentObjectProcessing = false;
                                if (obj.SampleTransferStatus == SamplingTransferStatus.Submitted)
                                {
                                    dc.CancelAction.Active.SetItemValue("enb", false);
                                    dc.AcceptAction.Active.SetItemValue("enb", false);
                                }
                                else
                                {
                                    dc.CancelAction.Active.SetItemValue("enb", true);
                                    dc.AcceptAction.Active.SetItemValue("enb", true);
                                    dc.Accepting += Dc_Accepting;
                                }
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                SampleBottleAllocation sample = (SampleBottleAllocation)e.AcceptActionArgs.CurrentObject;
                if (sample != null)
                {
                    SampleBottleAllocation allocation = ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().Where(a => a.Oid == sample.Oid).First();
                    if (allocation != null)
                    {
                        allocation.Remark = sample.Remark;
                        ((ListView)View).Refresh();
                    }
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
                e.Cancel = true;
                SampleBottleAllocation samplebottle = (SampleBottleAllocation)View.CurrentObject;
                if (samplebottle != null && samplebottle.BroughtBy != null)
                {
                    DashboardViewItem viewItem = ((DetailView)View).FindItem("SampleGrid") as DashboardViewItem;
                    if (viewItem != null && viewItem.InnerView != null)
                    {
                        if (samplebottle.SampleTransferStatus.ToString() == "PendingTransfer")
                        {
                            if (viewItem.InnerView.SelectedObjects.Count > 0)
                            {
                                ((ASPxGridListEditor)((ListView)viewItem.InnerView).Editor).Grid.UpdateEdit();
                                foreach (SampleBottleAllocation sample in viewItem.InnerView.SelectedObjects)
                                {
                                    savedata(viewItem.InnerView, sample, samplebottle, "save");
                                }
                                if (viewItem.InnerView.SelectedObjects.Cast<SampleBottleAllocation>().Where(a => a.Oid == samplebottle.Oid).Count() == 0)
                                {
                                    samplebottle.ReceivedBy = null;
                                    samplebottle.ReceivedDate = null;
                                    samplebottle.BroughtBy = null;
                                    samplebottle.Comment = null;
                                    samplebottle.CoolerId = null;
                                    samplebottle.STNoContainers = 0;
                                    samplebottle.STNoSamples = 0;
                                    samplebottle.STNoCoolers = 0;
                                    samplebottle.SampleAppearance = null;
                                    samplebottle.TransportCondition = null;
                                    View.ObjectSpace.CommitChanges();
                                }
                                viewItem.InnerView.ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                Frame.SetView(Application.CreateListView("SampleBottleAllocation_ListView_SampleTransferMain", new CollectionSource(Application.CreateObjectSpace(), typeof(SampleBottleAllocation)), true));
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Select the samples.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            ((ASPxGridListEditor)((ListView)viewItem.InnerView).Editor).Grid.UpdateEdit();
                            foreach (SampleBottleAllocation sample in ((ListView)viewItem.InnerView).CollectionSource.List)
                            {
                                savedata(viewItem.InnerView, sample, samplebottle, "");
                            }
                            viewItem.InnerView.ObjectSpace.CommitChanges();
                            View.ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select the brought by.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void savedata(View view, SampleBottleAllocation sample, SampleBottleAllocation samplebottle, string type)
        {
            if (type == "submit")
            {
                if (sample.SampleRegistration.ReceivedDate == null)
                {
                    sample.SampleRegistration.ReceivedDate = (DateTime)samplebottle.ReceivedDate;
                }
                if (sample.SampleRegistration.JobID.RecievedDate == DateTime.MinValue)
                {
                    sample.SampleRegistration.JobID.RecievedDate = (DateTime)samplebottle.ReceivedDate;
                }
                if (sample.SampleRegistration.JobID.RecievedBy == null)
                {
                    sample.SampleRegistration.JobID.RecievedBy = view.ObjectSpace.GetObjectByKey<Employee>(samplebottle.ReceivedBy.Oid);
                }
            }
            sample.BroughtBy = view.ObjectSpace.GetObjectByKey<BroughtBy>(samplebottle.BroughtBy.Oid);
            sample.Comment = samplebottle.Comment;
            sample.STNoContainers = samplebottle.STNoContainers;
            sample.STNoSamples = samplebottle.STNoSamples;
            sample.STNoCoolers = samplebottle.STNoCoolers;
            sample.SampleAppearance = samplebottle.SampleAppearance;
            sample.TransportCondition = samplebottle.TransportCondition;
            if (type == "save")
            {
                sample.SampleTransferStatus = SamplingTransferStatus.PendingSubmission;
            }
            else if (type == "submit")
            {
                sample.SampleTransferStatus = SamplingTransferStatus.Submitted;
                if (sample.SampleStatus == null || (sample.SampleStatus != null && !sample.SampleStatus.Samplinghold))
                {
                    foreach (SampleParameter parameter in view.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.Oid]=?", sample.SampleRegistration.Oid, sample.TestMethod.Oid)))
                    {
                        parameter.IsTransferred = true;
                    }
                }
            }
            foreach (SampleBottleAllocation TestBottles in view.ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [BottleID]=? And [Oid]<>?", sample.SampleRegistration.Oid, sample.BottleID, sample.Oid)))
            {
                TestBottles.BroughtBy = view.ObjectSpace.GetObjectByKey<BroughtBy>(samplebottle.BroughtBy.Oid);
                TestBottles.Comment = samplebottle.Comment;
                TestBottles.STNoContainers = samplebottle.STNoContainers;
                TestBottles.STNoSamples = samplebottle.STNoSamples;
                TestBottles.STNoCoolers = samplebottle.STNoCoolers;
                TestBottles.SampleAppearance = samplebottle.SampleAppearance;
                TestBottles.TransportCondition = samplebottle.TransportCondition;
                TestBottles.Remark = sample.Remark;
                TestBottles.ReceivedBy = sample.ReceivedBy;
                TestBottles.ReceivedDate = sample.ReceivedDate;
                TestBottles.SampleStatus = sample.SampleStatus;
                TestBottles.ScanDateTime = sample.ScanDateTime;
                if (type == "save")
                {
                    TestBottles.SampleTransferStatus = SamplingTransferStatus.PendingSubmission;
                }
                else if (type == "submit")
                {
                    TestBottles.SampleTransferStatus = SamplingTransferStatus.Submitted;
                    if (sample.SampleStatus == null || (sample.SampleStatus != null && !sample.SampleStatus.Samplinghold))
                    {
                        foreach (SampleParameter parameter in view.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.Oid]=?", TestBottles.SampleRegistration.Oid, TestBottles.TestMethod.Oid)))
                        {
                            parameter.IsTransferred = true;
                        }
                    }
                }
            }
        }

        private void Submit_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                SampleBottleAllocation samplebottle = (SampleBottleAllocation)View.CurrentObject;
                if (samplebottle != null && samplebottle.BroughtBy != null)
                {
                    DashboardViewItem viewItem = ((DetailView)View).FindItem("SampleGrid") as DashboardViewItem;
                    if (viewItem != null && viewItem.InnerView != null)
                    {
                        if (samplebottle.SampleTransferStatus == SamplingTransferStatus.PendingTransfer)
                        {
                            if (viewItem.InnerView.SelectedObjects.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage("Select the samples.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }
                        }
                        SampleSourceSetup setup = View.ObjectSpace.FindObject<SampleSourceSetup>(CriteriaOperator.Parse(""));
                        if (setup != null)
                        {
                            ((ASPxGridListEditor)((ListView)viewItem.InnerView).Editor).Grid.UpdateEdit();
                            if (setup.SampleTransfer == SampleSourceMode.Yes)
                            {
                                DetailView dv = Application.CreateDetailView(View.ObjectSpace, "SampleBottleAllocation_DetailView_STPassword", false, samplebottle);
                                dv.ViewEditMode = ViewEditMode.Edit;
                                ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.CloseOnCurrentObjectProcessing = false;
                                dc.CancelAction.Active.SetItemValue("enb", false);
                                dc.Accepting += AcceptAction_Executing;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                            else
                            {
                                if (samplebottle.SampleTransferStatus == SamplingTransferStatus.PendingTransfer)
                                {
                                    foreach (SampleBottleAllocation sample in ((ListView)viewItem.InnerView).SelectedObjects)
                                    {
                                        savedata(viewItem.InnerView, sample, samplebottle, "submit");
                                    }
                                    if (viewItem.InnerView.SelectedObjects.Cast<SampleBottleAllocation>().Where(a => a.Oid == samplebottle.Oid).Count() == 0)
                                    {
                                        samplebottle.ReceivedBy = null;
                                        samplebottle.ReceivedDate = null;
                                        samplebottle.BroughtBy = null;
                                        samplebottle.Comment = null;
                                        samplebottle.CoolerId = null;
                                        samplebottle.STNoContainers = 0;
                                        samplebottle.STNoSamples = 0;
                                        samplebottle.STNoCoolers = 0;
                                        samplebottle.SampleAppearance = null;
                                        samplebottle.TransportCondition = null;
                                        View.ObjectSpace.CommitChanges();
                                    }
                                }
                                else
                                {
                                    foreach (SampleBottleAllocation sample in ((ListView)viewItem.InnerView).CollectionSource.List)
                                    {
                                        savedata(viewItem.InnerView, sample, samplebottle, "submit");
                                    }
                                    View.ObjectSpace.CommitChanges();
                                }
                                checkstatus(samplebottle.SampleRegistration.JobID, viewItem.InnerView.ObjectSpace);
                                viewItem.InnerView.ObjectSpace.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                e.ShowViewParameters.CreatedView = Application.CreateListView("SampleBottleAllocation_ListView_SampleTransferMain", new CollectionSource(Application.CreateObjectSpace(), typeof(SampleBottleAllocation)), true);
                            }
                        }
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select the brought by.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SampleBottleAllocation samplebottle = (SampleBottleAllocation)View.CurrentObject;
            if (samplebottle != null)
            {
                if (string.IsNullOrEmpty(samplebottle.STPassword))
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage("Enter the password", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    PermissionPolicyUser policyUser = View.ObjectSpace.FindObject<PermissionPolicyUser>(CriteriaOperator.Parse("UserName=?", samplebottle.ReceivedBy.UserName));
                    if (policyUser != null)
                    {
                        if (policyUser.ComparePassword(samplebottle.STPassword))
                        {
                            DashboardViewItem viewItem = ((DetailView)View).FindItem("SampleGrid") as DashboardViewItem;
                            if (viewItem != null && viewItem.InnerView != null)
                            {
                                if (samplebottle.SampleTransferStatus == SamplingTransferStatus.PendingTransfer)
                                {
                                    foreach (SampleBottleAllocation sample in ((ListView)viewItem.InnerView).SelectedObjects)
                                    {
                                        savedata(viewItem.InnerView, sample, samplebottle, "submit");
                                    }
                                    if (viewItem.InnerView.SelectedObjects.Cast<SampleBottleAllocation>().Where(a => a.Oid == samplebottle.Oid).Count() == 0)
                                    {
                                        samplebottle.ReceivedBy = null;
                                        samplebottle.ReceivedDate = null;
                                        samplebottle.BroughtBy = null;
                                        samplebottle.Comment = null;
                                        samplebottle.CoolerId = null;
                                        samplebottle.STNoContainers = 0;
                                        samplebottle.STNoSamples = 0;
                                        samplebottle.STNoCoolers = 0;
                                        samplebottle.SampleAppearance = null;
                                        samplebottle.TransportCondition = null;
                                        View.ObjectSpace.CommitChanges();
                                    }
                                }
                                else
                                {
                                    foreach (SampleBottleAllocation sample in ((ListView)viewItem.InnerView).CollectionSource.List)
                                    {
                                        savedata(viewItem.InnerView, sample, samplebottle, "submit");
                                    }
                                    View.ObjectSpace.CommitChanges();
                                }
                                checkstatus(samplebottle.SampleRegistration.JobID, viewItem.InnerView.ObjectSpace);
                                viewItem.InnerView.ObjectSpace.CommitChanges();
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            Frame.SetView(Application.CreateListView("SampleBottleAllocation_ListView_SampleTransferMain", new CollectionSource(Application.CreateObjectSpace(), typeof(SampleBottleAllocation)), true));
                        }
                        else
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Password is incorrect", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
            }
        }

        private void checkstatus(Samplecheckin objSamplecheckin, IObjectSpace os)
        {
            bool fieldtest = false;
            bool normaltest = false;
            objSamplecheckin = os.GetObjectByKey<Samplecheckin>(objSamplecheckin.Oid);
            IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objSamplelogin = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? And [Testparameter.TestMethod.IsFieldTest] = True", objSamplecheckin.Oid)).Cast<SampleParameter>().Select(a => a.Samplelogin).Distinct().ToList();
            if (objSamplelogin != null)
            {
                if (objSamplelogin.Count == objSamplelogin.Where(i => i.SamplingStatus == SamplingStatus.Completed).Count())
                {
                    fieldtest = true;
                }
            }
            IList<SampleBottleAllocation> samples = os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.JobID.Oid]=?", objSamplecheckin.Oid), true);
            if (samples != null)
            {
                if (samples.Count == samples.Where(i => i.SampleTransferStatus == SamplingTransferStatus.Submitted).Count())
                {
                    normaltest = true;
                }
            }
            if (fieldtest && normaltest)
            {
                StatusDefinition objStatus = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 10"));
                if (objStatus != null)
                {
                    objSamplecheckin.Index = objStatus;
                }
            }
        }

        private void rollbackstatus(Samplecheckin objSamplecheckin, IObjectSpace os)
        {
            objSamplecheckin = os.GetObjectByKey<Samplecheckin>(objSamplecheckin.Oid);
            StatusDefinition objStatus = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 29"));
            if (objStatus != null)
            {
                objSamplecheckin.Index = objStatus;
            }
        }

        private void Rollback_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                SampleBottleAllocation samplebottle = (SampleBottleAllocation)View.CurrentObject;
                if (samplebottle != null)
                {
                    if (samplebottle.SampleRegistration.JobID.Index.UqIndexID == 29 || samplebottle.SampleRegistration.JobID.Index.UqIndexID == 10)
                    {
                        DashboardViewItem viewItem = ((DetailView)View).FindItem("SampleGrid") as DashboardViewItem;
                        if (viewItem != null && viewItem.InnerView != null)
                        {
                            foreach (SampleBottleAllocation sample in ((ListView)viewItem.InnerView).CollectionSource.List)
                            {
                                Frame.GetController<AuditlogViewController>().insertauditdata(View.ObjectSpace, sample.SampleRegistration.JobID.Oid, OperationType.Rollback, "Sample Transfer", sample.SampleRegistration.SampleID + " - " + sample.BottleID, "Bottles", "Submitted", "PendingSubmission", "");
                                sample.SampleTransferStatus = SamplingTransferStatus.PendingSubmission;
                                foreach (SampleParameter parameter in viewItem.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.Oid]=?", sample.SampleRegistration.Oid, sample.TestMethod.Oid)))
                                {
                                    parameter.IsTransferred = false;
                                }
                                foreach (SampleBottleAllocation TestBottles in viewItem.InnerView.ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [BottleID]=? And [Oid]<>?", sample.SampleRegistration.Oid, sample.BottleID, sample.Oid)))
                                {
                                    TestBottles.SampleTransferStatus = SamplingTransferStatus.PendingSubmission;
                                    foreach (SampleParameter parameter in viewItem.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.Oid]=?", TestBottles.SampleRegistration.Oid, TestBottles.TestMethod.Oid)))
                                    {
                                        parameter.IsTransferred = false;
                                    }
                                }
                                if (!viewItem.InnerView.ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [ReceivedBy] is not null", sample.SampleRegistration.Oid), true).Any())
                                {
                                    sample.SampleRegistration.ReceivedDate = null;
                                }
                            }
                            rollbackstatus(samplebottle.SampleRegistration.JobID, viewItem.InnerView.ObjectSpace);
                            if (!viewItem.InnerView.ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.JobID.Oid]=? And [ReceivedBy] is not null", samplebottle.SampleRegistration.JobID.Oid), true).Any())
                            {
                                Samplecheckin samplecheckin = viewItem.InnerView.ObjectSpace.GetObjectByKey<Samplecheckin>(samplebottle.SampleRegistration.JobID.Oid);
                                if (samplecheckin != null)
                                {
                                    samplecheckin.RecievedDate = DateTime.MinValue;
                                    samplecheckin.RecievedBy = null;
                                }
                            }
                            viewItem.InnerView.ObjectSpace.CommitChanges();
                            IList<AuditData> Objects = View.ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                            if (Objects.Count() > 0)
                            {
                                Frame.GetController<AuditlogViewController>().commentdialog(sender, Objects.First());
                            }
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Rollback cannot be performed", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void STDelete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SampleBottleAllocation samplebottle = (SampleBottleAllocation)View.CurrentObject;
                if (samplebottle != null)
                {
                    if (samplebottle.SampleRegistration.JobID.Index.UqIndexID == 29 || samplebottle.SampleRegistration.JobID.Index.UqIndexID == 10)
                    {
                        DashboardViewItem viewItem = ((DetailView)View).FindItem("SampleGrid") as DashboardViewItem;
                        if (viewItem != null && viewItem.InnerView != null)
                        {
                            foreach (SampleBottleAllocation sample in ((ListView)viewItem.InnerView).CollectionSource.List)
                            {
                                Frame.GetController<AuditlogViewController>().insertauditdata(View.ObjectSpace, sample.SampleRegistration.JobID.Oid, OperationType.Deleted, "Sample Transfer", sample.SampleRegistration.SampleID + " - " + sample.BottleID, "Bottles", sample.SampleTransferStatus.ToString(), "PendingTransfer", "");
                                sample.BroughtBy = null;
                                sample.Comment = null;
                                sample.STNoContainers = 0;
                                sample.STNoSamples = 0;
                                sample.STNoCoolers = 0;
                                sample.CoolerId = null;
                                sample.SampleAppearance = null;
                                sample.TransportCondition = null;
                                sample.SampleStatus = null;
                                sample.ReceivedDate = null;
                                sample.ReceivedBy = null;
                                sample.Remark = null;
                                sample.ScanDateTime = null;
                                sample.SampleTransferStatus = SamplingTransferStatus.PendingTransfer;
                                foreach (SampleParameter parameter in viewItem.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.Oid]=?", sample.SampleRegistration.Oid, sample.TestMethod.Oid)))
                                {
                                    parameter.IsTransferred = false;
                                }
                                foreach (SampleBottleAllocation TestBottles in viewItem.InnerView.ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [BottleID]=? And [Oid]<>?", sample.SampleRegistration.Oid, sample.BottleID, sample.Oid)))
                                {
                                    TestBottles.BroughtBy = null;
                                    TestBottles.Comment = null;
                                    TestBottles.STNoContainers = 0;
                                    TestBottles.STNoSamples = 0;
                                    TestBottles.STNoCoolers = 0;
                                    TestBottles.CoolerId = null;
                                    TestBottles.SampleAppearance = null;
                                    TestBottles.TransportCondition = null;
                                    TestBottles.SampleStatus = null;
                                    TestBottles.ReceivedDate = null;
                                    TestBottles.ReceivedBy = null;
                                    TestBottles.Remark = null;
                                    TestBottles.ScanDateTime = null;
                                    TestBottles.SampleTransferStatus = SamplingTransferStatus.PendingTransfer;
                                    foreach (SampleParameter parameter in viewItem.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=? And [Testparameter.TestMethod.Oid]=?", TestBottles.SampleRegistration.Oid, TestBottles.TestMethod.Oid)))
                                    {
                                        parameter.IsTransferred = false;
                                    }
                                }
                                if (!viewItem.InnerView.ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [ReceivedBy] is not null", sample.SampleRegistration.Oid), true).Any())
                                {
                                    sample.SampleRegistration.ReceivedDate = null;
                                }
                            }
                            rollbackstatus(samplebottle.SampleRegistration.JobID, viewItem.InnerView.ObjectSpace);
                            if (!viewItem.InnerView.ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.JobID.Oid]=? And [ReceivedBy] is not null", samplebottle.SampleRegistration.JobID.Oid), true).Any())
                            {
                                Samplecheckin samplecheckin = viewItem.InnerView.ObjectSpace.GetObjectByKey<Samplecheckin>(samplebottle.SampleRegistration.JobID.Oid);
                                if (samplecheckin != null)
                                {
                                    samplecheckin.RecievedDate = DateTime.MinValue;
                                    samplecheckin.RecievedBy = null;
                                }
                            }
                            viewItem.InnerView.ObjectSpace.CommitChanges();
                            IList<AuditData> Objects = View.ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                            if (Objects.Count() > 0)
                            {
                                Frame.GetController<AuditlogViewController>().commentdialog(sender, Objects.First());
                            }
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Cannot delete this transfer", InformationType.Error, timer.Seconds, InformationPosition.Top);
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
