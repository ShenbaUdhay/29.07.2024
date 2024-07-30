using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.SamplesSite;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Labmaster.Module.Controllers.SamplingManagement
{
    public partial class FieldDataEntryViewController : ViewController, IXafCallbackHandler
    {
        TaskManagementInfo TMInfo = new TaskManagementInfo();
        MessageTimer timer = new MessageTimer();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        SamplingFieldConfigurationInfo SFCInfo = new SamplingFieldConfigurationInfo();
        curlanguage objLanguage = new curlanguage();
        AuditInfo objAuditInfo = new AuditInfo();

        public FieldDataEntryViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleLogIn_ListView_FieldDataEntry_Station;"
                + "SampleLogIn_ListView_FieldDataEntry_Sampling;"
                + "SampleParameter_ListView_FieldDataEntry;"
                + "SampleLogIn_DetailView_FieldDataEntry;"
                + "SampleLogIn_ListView_CopyTo_FieldDataEntry;"
                + "Samplecheckin_ListView_FieldDataEntry;"
                + "Samplecheckin_DetailView_FieldDataEntry;"
                + "SampleLogIn_ListView_FieldDataEntry_History;"
                + "Samplecheckin_DetailView_FieldDataEntry_History;";
            FieldDataEntrySave.TargetViewId = "Samplecheckin_DetailView_FieldDataEntry;" + "Samplecheckin_DetailView_FieldDataEntry_History;";
            FieldDataEntryComplete.TargetViewId = "Samplecheckin_DetailView_FieldDataEntry;";
            FieldDataEntryEdit.TargetViewId = "SampleLogIn_ListView_FieldDataEntry_Sampling;";
            FieldDataEntryCopyPrevious.TargetViewId = FieldDataEntryCopyTo.TargetViewId = FieldDataEntryCopyToAll.TargetViewId = "SampleLogIn_DetailView_FieldDataEntry";
            SimpleAction taskfde = new SimpleAction(this, "taskfde", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit)
            {
                Caption = "History",
            };
            taskfde.TargetViewId = "Samplecheckin_ListView_FieldDataEntry";
            taskfde.Execute += Taskfde_Execute;
            taskfde.ImageName = "Action_Search";
            STFilter.TargetViewId = "SampleLogIn_ListView_FieldDataEntry_History";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                FieldDataEntryComplete.Active.SetItemValue("valComplete", false);
                FieldDataEntryEdit.Active.SetItemValue("valEdit", false);
                FieldDataEntrySave.Active.SetItemValue("valSave", false);
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                {
                    FieldDataEntryComplete.Active.SetItemValue("valComplete", true);
                    FieldDataEntryEdit.Active.SetItemValue("valEdit", true);
                    FieldDataEntrySave.Active.SetItemValue("valSave", true);
                }
                else
                {
                    if (objnavigationRefresh.ClickedNavigationItem == "FieldDataEntry")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "FieldDataEntry" && i.Write == true) != null)
                            {
                                FieldDataEntryComplete.Active.SetItemValue("valComplete", true);
                                FieldDataEntryEdit.Active.SetItemValue("valEdit", true);
                                FieldDataEntrySave.Active.SetItemValue("valSave", true);
                            }
                        }
                    }
                }

                if (View.Id == "Samplecheckin_ListView_FieldDataEntry" || View.Id == "SampleLogIn_ListView_FieldDataEntry_History")
                {

                    if (STFilter != null && STFilter.SelectedItem == null && View.Id == "SampleLogIn_ListView_FieldDataEntry_History")
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
                        listProcessController.CustomProcessSelectedItem += ProcessListViewRowController_CustomProcessSelectedItem;
                    }

                    List<object> OidTask = new List<object>();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    {
                        if (Application.MainWindow.View.Id.Contains("History"))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Samplelogin.JobID.IsSampling] = True And [Samplelogin.SamplingStatus] <> 'PendingCompletion'");
                            lstview.Properties.Add(new ViewProperty("Status", SortDirection.Ascending, "Samplelogin.SamplingStatus", true, true));
                            lstview.Properties.Add(new ViewProperty("JobId", SortDirection.Ascending, "Samplelogin.JobID.Oid", true, true));
                            lstview.Properties.Add(new ViewProperty("group", SortDirection.Ascending, "Max(Samplelogin.Oid)", false, true));
                        }
                        else
                        {
                            StatusDefinition objStatus = View.ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 29"));
                            if (objStatus != null)
                            {
                                lstview.Criteria = CriteriaOperator.Parse("[Samplelogin.JobID.Index.Oid] = ? And [Samplelogin.JobID.IsSampling] = True And [Samplelogin.SamplingStatus] = 'PendingCompletion'", objStatus.Oid);
                            }
                            lstview.Properties.Add(new ViewProperty("group", SortDirection.Ascending, "Samplelogin.JobID.Oid", true, true));
                        }
                        foreach (ViewRecord Vrec in lstview)
                            OidTask.Add(Vrec["group"]);
                    }
                    ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);
                }
                else if (View.Id == "Samplecheckin_DetailView_FieldDataEntry" || View.Id == "Samplecheckin_DetailView_FieldDataEntry_History")
                {
                    Samplecheckin samplecheckin = (Samplecheckin)View.CurrentObject;
                    if (samplecheckin != null)
                    {
                        objAuditInfo.currentViewOid = samplecheckin.Oid;
                    }
                    RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
                    DashboardViewItem viStation = ((DetailView)View).FindItem("StationInformation") as DashboardViewItem;
                    if (viStation != null)
                    {
                        viStation.ControlCreated += ViStation_ControlCreated;
                    }
                    SFCInfo.lstMandatoryColumn = ObjectSpace.GetObjects<VisualMatrix>(CriteriaOperator.Parse("[SamplingFieldConfiguration][[IsMandatory] = True]"));
                    ((WebLayoutManager)((DetailView)View).LayoutManager).PageControlCreated += FieldDataEntryViewController_PageControlCreated;
                }
                else if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling")
                {
                    if (TMInfo.TaskOid != null)
                    {
                        IList<SampleParameter> lstss;
                        if (Application.MainWindow.View.Id.Contains("History"))
                        {
                            FieldDataEntryEdit.Active.SetItemValue("valEdit", false);
                            lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? AND [Samplelogin.SamplingStatus] = ?", TMInfo.TaskOid , TMInfo.SamplingStatus));
                        }
                        else
                        {
                            FieldDataEntryEdit.Active.SetItemValue("valEdit", true);
                            lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? AND [Samplelogin.SamplingStatus] = 'PendingCompletion'", TMInfo.TaskOid));
                        }
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] In (" + string.Format("'{0}'", string.Join("','", lstss.Select(i => i.Samplelogin.Oid.ToString().Replace("'", "''")))) + ")");
                    }
                }
                else if (View.Id == "SampleParameter_ListView_FieldDataEntry")
                {
                    if (TMInfo.TaskOid != null)
                    {
                        if (Application.MainWindow.View.Id.Contains("History"))
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("Samplelogin.JobID.Oid=? AND [Samplelogin.SamplingStatus] = ? And [Testparameter.TestMethod.IsFieldTest] = True And [Samplelogin.Collector] Is Not Null And [Samplelogin.CollectDate] Is Not Null", TMInfo.TaskOid, TMInfo.SamplingStatus);
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("Samplelogin.JobID.Oid=? AND [Samplelogin.SamplingStatus] = 'PendingCompletion' And [Testparameter.TestMethod.IsFieldTest] = True And [Samplelogin.Collector] Is Not Null And [Samplelogin.CollectDate] Is Not Null", TMInfo.TaskOid);
                        }
                    }
                }
                else if (View.Id == "SampleLogIn_ListView_CopyTo_FieldDataEntry")
                {
                    if (TMInfo.TaskOid != null && TMInfo.strSamplingSampleID != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[JobID.Oid]='" + TMInfo.TaskOid + "'And [Oid]<>?", new Guid(TMInfo.strSamplingSampleID));
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
                if (View is ListView)
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
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("JobID.CreatedDate >= ?", srDateFilter);
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
        private void RuleSet_CustomNeedToValidateRule(object sender, CustomNeedToValidateRuleEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_DetailView_FieldDataEntry" || View.Id == "Samplecheckin_DetailView_FieldDataEntry_History")
                {
                    e.NeedToValidateRule = false;
                    e.Handled = !e.NeedToValidateRule;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ViStation_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem viStation = ((DetailView)View).FindItem("StationInformation") as DashboardViewItem;
                if (viStation != null && viStation.InnerView != null)
                {
                    IList<SampleParameter> lstss;
                    if (Application.MainWindow.View.Id.Contains("History"))
                    {
                        lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? AND [Samplelogin.SamplingStatus] = ?", TMInfo.TaskOid, TMInfo.SamplingStatus));
                    }
                    else
                    {
                        lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? AND [Samplelogin.SamplingStatus] = 'PendingCompletion'", TMInfo.TaskOid));
                    }
                    List<SampleLogIn> distinctStation = lstss.Select(a => a.Samplelogin).GroupBy(p => new { p.Matrix, p.StationLocation }).Select(g => g.First()).ToList();
                    List<Guid> objOid = distinctStation.Select(i => i.Oid).ToList();
                    if (objOid.Count > 0)
                    {
                        ((ListView)viStation.InnerView).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", objOid);
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
                if (View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling")
                        {
                            if (Application.MainWindow.View.Id.Contains("History"))
                            {
                                gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) { e.cancel = true; }";
                            }
                            else
                            {
                                gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;

                            }
                        }
                        else if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Station")
                        {
                            gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                            gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                            ICallbackManagerHolder Station = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                            Station.CallbackManager.RegisterHandler("SampleSourcePopup", this);
                        }
                        else if (View.Id == "SampleParameter_ListView_FieldDataEntry")
                        {
                            if (Application.MainWindow.View.Id.Contains("History"))
                            {
                                gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) { e.cancel = true; }";
                            }
                            else
                            {
                                gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                            }
                        }

                        if (View != null && View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling" || View.Id == "SampleParameter_ListView_FieldDataEntry" || View.Id == "SampleLogIn_ListView_FieldDataEntry_Station")
                        {
                            gridListEditor.Grid.Load += Grid_Load;
                            gridListEditor.Grid.BatchUpdate += Grid_BatchUpdate;
                            if (Application.MainWindow.View.Id.Contains("History") && View.Id != "SampleLogIn_ListView_FieldDataEntry_Station")
                            {
                                gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            }
                            if (TMInfo.EditColumnName == null)
                            {
                                TMInfo.EditColumnName = new List<string>();
                            }
                            foreach (ColumnWrapper wrapper in gridListEditor.Columns)
                            {
                                IModelColumn columnModel = ((ListView)View).Model.Columns[wrapper.PropertyName];
                                if (columnModel != null && columnModel.AllowEdit == true && !TMInfo.EditColumnName.Contains(columnModel.Id + ".Oid") && columnModel.PropertyEditorType == typeof(ASPxLookupPropertyEditor))
                                {
                                    TMInfo.EditColumnName.Add(columnModel.Id + ".Oid");
                                }
                                else if (columnModel != null && columnModel.AllowEdit == true && !TMInfo.EditColumnName.Contains(columnModel.Id) && columnModel.PropertyEditorType != typeof(ASPxLookupPropertyEditor))
                                {
                                    TMInfo.EditColumnName.Add(columnModel.Id);
                                }
                            }

                            if (TMInfo.EditColumnName.Count > 0)
                            {
                                gridListEditor.Grid.JSProperties["cpeditcolumnname"] = TMInfo.EditColumnName;
                            }
                            gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {   
                                sessionStorage.setItem('FieldDataEntryFocusedColumn', null);  
                                if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                                {  
                                    e.cancel = true;
                                }                  
                                else
                                {
                                    if(s.cpeditcolumnname.includes(e.cellInfo.column.fieldName))
                                    {
                                        sessionStorage.setItem('FieldDataEntryFocusedColumn', e.cellInfo.column.fieldName); 
                                    }
                                    else
                                    {
                                        e.cancel=true;
                                    }
                                }                                         
                            }";
                            gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('FieldDataEntryFocusedColumn');    
                                    if(FocusedColumn.includes('.'))
                                    {   
                                        var oid = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                        var text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;   
                                        if (e.item.name =='CopyToAllCell')
                                        {
                                            for(var i = 0; i < s.cpVisibleRowCount; i++)
                                            { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                {          
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                                }
                                            }
                                        }        
                                    }
                                    else 
                                    {      
                                        var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                        if (e.item.name =='CopyToAllCell')
                                        {
                                            for(var i = 0; i < s.cpVisibleRowCount; i++)
                                            {                                                 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                {
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                                }
                                            }
                                        }    
                                    }
                                }
                                e.processOnServer = false;
                            }";
                            Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                        }
                    }
                }
                else
                {
                    if (View.Id == "SampleLogIn_DetailView_FieldDataEntry" && View.CurrentObject != null)
                    {
                        SampleLogIn objSampling = (SampleLogIn)View.CurrentObject;
                        if (objSampling.SampleID != null)
                        {
                            TMInfo.strSamplingSampleID = objSampling.Oid.ToString();
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

        private void FieldDataEntryViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            if (e.Model.Id == "Tabs")
            {
                e.PageControl.Callback += PageControl_Callback;
                e.PageControl.Init += PageControl_Init;
            }
        }

        private void PageControl_Init(object sender, EventArgs e)
        {
            ASPxPageControl pageControl = (ASPxPageControl)sender;
            ClientSideEventsHelper.AssignClientHandlerSafe(pageControl, "ActiveTabChanged", "function(s, e) { if(s.GetActiveTabIndex() == 2) { s.PerformCallback('TabChanged'); } }", "MonitoringRecords");
        }

        private void PageControl_Callback(object sender, CallbackEventArgsBase e)
        {
            if (e.Parameter == "TabChanged")
            {
                TabPage activePage = ((ASPxPageControl)sender).ActiveTabPage;
                if (activePage.Text == "Monitoring Records")
                {
                    DashboardViewItem viMonitoringRecords = (DashboardViewItem)((DetailView)View).FindItem("MonitoringRecords");
                    if (viMonitoringRecords != null && viMonitoringRecords.InnerView != null)
                    {
                        viMonitoringRecords.InnerView.ObjectSpace.Refresh();
                        viMonitoringRecords.InnerView.RefreshDataSource();
                    }
                }
            }
        }

        private void Grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                foreach (var args in e.UpdateValues)
                {
                    UpdateItem(args.Keys, args.NewValues);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected void UpdateItem(OrderedDictionary keys, OrderedDictionary newValues)
        {
            try
            {
                if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Station")
                {
                    DetailView dv = (DetailView)Application.MainWindow.View;
                    if (dv != null)
                    {
                        DashboardViewItem viSamplingInformation = (DashboardViewItem)dv.FindItem("SamplingInformation");
                        if (viSamplingInformation != null && viSamplingInformation.InnerView == null)
                        {
                            viSamplingInformation.CreateControl();
                            viSamplingInformation.InnerView.CreateControls();
                        }
                        if (viSamplingInformation != null && viSamplingInformation.InnerView != null)
                        {
                            foreach (SampleLogIn logIn in ((ListView)viSamplingInformation.InnerView).CollectionSource.List.Cast<SampleLogIn>().Where(a => a.StationLocation.Oid.ToString() == newValues["StationLocation.Oid"].ToString() && a.Oid.ToString() != Convert.ToString(keys["Oid"])).ToList())
                            {
                                logIn.Humidity = newValues["Humidity"]?.ToString();
                                logIn.Temp = newValues["Temp"]?.ToString();
                            }
                            viSamplingInformation.InnerView.ObjectSpace.CommitChanges();
                            viSamplingInformation.InnerView.Refresh();
                            viSamplingInformation.InnerView.RefreshDataSource();
                        }
                    }
                    if (Application.MainWindow.View.Id.Contains("History"))
                    {
                        IList<AuditData> Objects = View.ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                        if (Objects.Count() > 0)
                        {
                            Frame.GetController<AuditlogViewController>().getcomments(View.ObjectSpace, Objects.First(), ViewEditMode.Edit);
                        }
                    }
                }
                else if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling")
                {
                    if (newValues["SampleStatus.Oid"] != null)
                    {
                        SampleStatus status = View.ObjectSpace.GetObjectByKey<SampleStatus>(new Guid(newValues["SampleStatus.Oid"].ToString()));
                        if (status != null)
                        {
                            if (status.Samplinghold)
                            {
                                SampleBottleAllocation logIn = View.ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=?", new Guid(Convert.ToString(keys["Oid"]))));
                                if (logIn != null)
                                {
                                    logIn.SampleStatus = status;
                                    View.ObjectSpace.CommitChanges();
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

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (e.DataColumn.FieldName == "StationLocation.Oid")
                {
                    e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'SampleSourcePopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                }
                else
                {
                    return;
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
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                gridListEditor.Grid.JSProperties["cpVisibleRowCount"] = gridListEditor.Grid.VisibleRowCount;
                if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling")
                {
                    if (SFCInfo.lstSamplingColumn == null)
                    {
                        Samplecheckin objTasks = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", TMInfo.TaskOid));
                        if (gridListEditor != null && objTasks != null)
                        {
                            SFCInfo.lstSamplingColumn = new List<SamplingFieldConfiguration>();
                            if (!string.IsNullOrEmpty(objTasks.SampleMatries))
                            {
                                List<string> lstSMOid = objTasks.SampleMatries.Split(';').ToList();
                                foreach (string strOid in lstSMOid)
                                {
                                    VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));
                                    if (objVM != null && objVM.SamplingFieldConfiguration.Count > 0)
                                    {
                                        IList<SamplingFieldConfiguration> objFiledSample = objVM.SamplingFieldConfiguration.Where(i => i.FieldClass == FieldClass.Sampling).ToList();
                                        if (objFiledSample != null && objFiledSample.Count > 0)
                                        {
                                            foreach (SamplingFieldConfiguration objField in objFiledSample)
                                            {
                                                if (SFCInfo.lstSamplingColumn.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                                {
                                                    SFCInfo.lstSamplingColumn.Add(objField);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    customisefields(gridListEditor, SFCInfo.lstSamplingColumn);
                }
                else if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Station")
                {
                    if (SFCInfo.lstStationColumn == null)
                    {
                        Samplecheckin objTasks = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", TMInfo.TaskOid));
                        if (gridListEditor != null && objTasks != null)
                        {
                            SFCInfo.lstStationColumn = new List<SamplingFieldConfiguration>();
                            if (!string.IsNullOrEmpty(objTasks.SampleMatries))
                            {
                                List<string> lstSMOid = objTasks.SampleMatries.Split(';').ToList();
                                foreach (string strOid in lstSMOid)
                                {
                                    VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));

                                    if (objVM != null && objVM.SamplingFieldConfiguration.Count > 0)
                                    {
                                        IList<SamplingFieldConfiguration> objFiledStation = objVM.SamplingFieldConfiguration.Where(i => i.FieldClass == FieldClass.Station).ToList();
                                        if (objFiledStation != null && objFiledStation.Count > 0)
                                        {
                                            foreach (SamplingFieldConfiguration objField in objFiledStation)
                                            {
                                                if (SFCInfo.lstStationColumn.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                                {
                                                    SFCInfo.lstStationColumn.Add(objField);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    customisefields(gridListEditor, SFCInfo.lstStationColumn);
                }
                else if (View.Id == "SampleParameter_ListView_FieldDataEntry")
                {
                    if (SFCInfo.lstTestColumn == null)
                    {
                        Samplecheckin objTasks = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", TMInfo.TaskOid));
                        if (gridListEditor != null && objTasks != null)
                        {
                            SFCInfo.lstTestColumn = new List<SamplingFieldConfiguration>();
                            if (!string.IsNullOrEmpty(objTasks.SampleMatries))
                            {
                                List<string> lstSMOid = objTasks.SampleMatries.Split(';').ToList();
                                foreach (string strOid in lstSMOid)
                                {
                                    VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strOid.Trim()));
                                    IList<SamplingFieldConfiguration> objFiledTest = objVM.SamplingFieldConfiguration.Where(i => i.FieldClass == FieldClass.Test).ToList();
                                    if (objVM != null && objFiledTest.Count > 0)
                                    {
                                        foreach (SamplingFieldConfiguration objField in objFiledTest)
                                        {
                                            if (SFCInfo.lstTestColumn.FirstOrDefault(i => i.Oid == objField.Oid) == null)
                                            {
                                                SFCInfo.lstTestColumn.Add(objField);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    customisefields(gridListEditor, SFCInfo.lstTestColumn);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        void customisefields(ASPxGridListEditor gridListEditor, List<SamplingFieldConfiguration> lstFields)
        {
            try
            {
                gridListEditor.Grid.ClearSort();
                foreach (GridViewColumn column in gridListEditor.Grid.Columns.Cast<GridViewColumn>().ToList())
                {
                    if (column.Name == "SelectionCommandColumn")
                    {
                        gridListEditor.Grid.VisibleColumns[column.Name].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    else
                    {
                        IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                        if (columnInfo != null)
                        {
                            IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                            if (lstFields != null)
                            {
                                if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Station" && columnInfo.Model.Id == "SampleID")
                                {
                                    gridListEditor.Grid.SortBy(column, 0);
                                    column.Visible = false;
                                }
                                else
                                {
                                    SamplingFieldConfiguration curField = lstFields.FirstOrDefault(i => i.FieldID.Trim().ToLower() == columnInfo.Model.Id.Trim().ToLower());
                                    if (curField != null)
                                    {
                                        column.Visible = true;
                                        if (!string.IsNullOrEmpty(curField.FieldCustomCaption))
                                        {
                                            column.Caption = curField.FieldCustomCaption;
                                        }
                                        else
                                        {
                                            column.Caption = curField.FieldCaption;
                                        }
                                        if (curField.SortOrder > 0)
                                        {
                                            column.SetColVisibleIndex(curField.SortOrder + 1);
                                            if (curField.Freeze)
                                            {
                                                gridListEditor.Grid.SortBy(column, curField.SortOrder);
                                            }
                                        }
                                        if (curField.Freeze && gridListEditor.Grid.Columns[columnInfo.Model.Id] != null)
                                        {
                                            gridListEditor.Grid.Columns[columnInfo.Model.Id].FixedStyle = GridViewColumnFixedStyle.Left;
                                        }
                                        if (curField.Width > 0)
                                        {
                                            column.Width = curField.Width;
                                        }
                                        modelColumn.AllowEdit = !curField.IsReadonly;
                                    }
                                    else
                                    {
                                        modelColumn.Remove();
                                        gridListEditor.Grid.Columns.Remove(column);
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

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            try
            {
                if (View.Id == "Samplecheckin_ListView_FieldDataEntry" || View.Id == "SampleLogIn_ListView_FieldDataEntry_History")
                {
                    ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    if (listProcessController != null)
                    {
                        listProcessController.CustomProcessSelectedItem -= ProcessListViewRowController_CustomProcessSelectedItem;
                    }
                }
                else if (View.Id == "Samplecheckin_DetailView_FieldDataEntry" || View.Id == "Samplecheckin_DetailView_FieldDataEntry_History")
                {
                    RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
                    DashboardViewItem viStation = ((DetailView)View).FindItem("StationInformation") as DashboardViewItem;
                    if (viStation != null)
                    {
                        viStation.ControlCreated -= ViStation_ControlCreated;
                    }
                    ((WebLayoutManager)((DetailView)View).LayoutManager).PageControlCreated -= FieldDataEntryViewController_PageControlCreated;
                }
                else if (View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling" || View.Id == "SampleParameter_ListView_FieldDataEntry" || View.Id == "SampleLogIn_ListView_FieldDataEntry_Station")
                {
                    if (TMInfo.EditColumnName.Count > 0)
                    {
                        TMInfo.EditColumnName.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ProcessListViewRowController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (View.Id == "SampleLogIn_ListView_FieldDataEntry_History")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    SampleLogIn objToShow = e.InnerArgs.CurrentObject as SampleLogIn;
                    if (objToShow != null)
                    {
                        Samplecheckin objERG = objspace.GetObject<Samplecheckin>(objToShow.JobID);
                        TMInfo.TaskOid = objToShow.JobID.Oid;
                        TMInfo.ClientName = objToShow.JobID.ClientName.CustomerName;
                        TMInfo.SamplingStatus = objToShow.SamplingStatus.ToString();
                        e.Handled = true;
                        DetailView createDetailView = Application.CreateDetailView(objspace, "Samplecheckin_DetailView_FieldDataEntry_History", true, objERG);
                        Frame.SetView(createDetailView);
                    }
                                       
                }
                else
                {
                if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject.GetType() == typeof(Samplecheckin))
                {
                    Samplecheckin obj = (Samplecheckin)e.InnerArgs.CurrentObject;
                    if (obj != null)
                    {
                        TMInfo.TaskOid = obj.Oid;
                        TMInfo.ClientName = obj.ClientName.CustomerName;
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

        private void FieldDataEntrySave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_DetailView_FieldDataEntry" || View.Id == "Samplecheckin_DetailView_FieldDataEntry_History")
                {
                    DetailView dv = (DetailView)Application.MainWindow.View;
                    if (dv != null)
                    {
                        DashboardViewItem viSamplingInformation = (DashboardViewItem)((DetailView)View).FindItem("SamplingInformation");
                        DashboardViewItem viStationInformation = (DashboardViewItem)((DetailView)View).FindItem("StationInformation");
                        DashboardViewItem viMonitoringRecords = (DashboardViewItem)((DetailView)View).FindItem("MonitoringRecords");
                        if (viStationInformation != null && viStationInformation.InnerView != null)
                        {
                            ((ASPxGridListEditor)((ListView)viStationInformation.InnerView).Editor).Grid.UpdateEdit();
                            ((ListView)viStationInformation.InnerView).ObjectSpace.CommitChanges();
                        }
                        if (!dv.Id.Contains("History"))
                        {
                            if (viSamplingInformation != null && viSamplingInformation.InnerView != null)
                            {
                                ((ASPxGridListEditor)((ListView)viSamplingInformation.InnerView).Editor).Grid.UpdateEdit();
                                ((ListView)viSamplingInformation.InnerView).ObjectSpace.CommitChanges();
                                viSamplingInformation.InnerView.Refresh();
                                viSamplingInformation.InnerView.RefreshDataSource();
                            }
                            if (viMonitoringRecords != null && viMonitoringRecords.InnerView != null)
                            {
                                ((ASPxGridListEditor)((ListView)viMonitoringRecords.InnerView).Editor).Grid.UpdateEdit();
                                ((ListView)viMonitoringRecords.InnerView).ObjectSpace.CommitChanges();
                                viMonitoringRecords.InnerView.Refresh();
                                viMonitoringRecords.InnerView.RefreshDataSource();
                            }
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    if (objLanguage.strcurlanguage != "En")
                    {
                        e.Items.Add("复制到所有单元格", "CopyToAllCell");
                    }
                    else
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    }
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FieldDataEntryComplete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_DetailView_FieldDataEntry")
                {
                    DashboardViewItem tasksSamplingInfo = ((DetailView)View).FindItem("SamplingInformation") as DashboardViewItem;
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView == null)
                    {
                        tasksSamplingInfo.CreateControl();
                        tasksSamplingInfo.InnerView.CreateControls();
                    }
                    if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView != null & ((ListView)tasksSamplingInfo.InnerView).SelectedObjects.Count > 0)
                    {
                        bool IsError = false;
                        DashboardViewItem tasksMonitoringInfo = ((DetailView)View).FindItem("MonitoringRecords") as DashboardViewItem;
                        DashboardViewItem stationInfo = ((DetailView)View).FindItem("StationInformation") as DashboardViewItem;
                        if (tasksMonitoringInfo != null && tasksMonitoringInfo.InnerView == null)
                        {
                            tasksMonitoringInfo.CreateControl();
                            tasksMonitoringInfo.InnerView.CreateControls();
                        }
                        if (stationInfo != null && stationInfo.InnerView != null)
                        {
                            ((ASPxGridListEditor)((ListView)stationInfo.InnerView).Editor).Grid.UpdateEdit();
                            ((ListView)stationInfo.InnerView).ObjectSpace.CommitChanges();
                        }
                        if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView != null)
                        {
                            ((ASPxGridListEditor)((ListView)tasksSamplingInfo.InnerView).Editor).Grid.UpdateEdit();
                            ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.CommitChanges();
                            tasksSamplingInfo.InnerView.Refresh();
                            tasksSamplingInfo.InnerView.RefreshDataSource();
                        }
                        if (tasksMonitoringInfo != null && tasksMonitoringInfo.InnerView != null)
                        {
                            ((ASPxGridListEditor)((ListView)tasksMonitoringInfo.InnerView).Editor).Grid.UpdateEdit();
                            ((ListView)tasksMonitoringInfo.InnerView).ObjectSpace.CommitChanges();
                            tasksMonitoringInfo.InnerView.Refresh();
                            tasksMonitoringInfo.InnerView.RefreshDataSource();
                        }
                        if (tasksSamplingInfo != null && tasksSamplingInfo.InnerView != null)
                        {
                            ITypeInfo objectTypeInfo = XafTypesInfo.Instance.FindTypeInfo(typeof(SampleLogIn).FullName);
                            foreach (SampleLogIn objSampling in ((ListView)tasksSamplingInfo.InnerView).SelectedObjects.Cast<SampleLogIn>().ToList())
                            {
                                if (objSampling.SampleStatus == null || !objSampling.SampleStatus.Samplinghold)
                                {
                                    VisualMatrix objMatrix = SFCInfo.lstMandatoryColumn.FirstOrDefault(i => i.Oid == objSampling.VisualMatrix.Oid);
                                    if (objMatrix != null && IsError == false)
                                    {
                                        foreach (SamplingFieldConfiguration objConfig in objMatrix.SamplingFieldConfiguration.Where(i => i.IsMandatory == true && i.FieldClass == FieldClass.Sampling))
                                        {
                                            if (objectTypeInfo != null && objectTypeInfo.FindMember(objConfig.FieldID) != null)
                                            {
                                                var value = objSampling.GetMemberValue(objConfig.FieldID);
                                                if (value == null)
                                                {
                                                    if (objLanguage.strcurlanguage == "En")
                                                    {
                                                        if (!string.IsNullOrEmpty(objConfig.FieldCustomCaption))
                                                        {
                                                            Application.ShowViewStrategy.ShowMessage(("Enter the Value " + objConfig.FieldCustomCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                            IsError = true;
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            Application.ShowViewStrategy.ShowMessage(("Enter the Value " + objConfig.FieldCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                            IsError = true;
                                                            return;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(objConfig.FieldCustomCaption))
                                                        {
                                                            Application.ShowViewStrategy.ShowMessage(("输入值" + objConfig.FieldCustomCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                            IsError = true;
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            Application.ShowViewStrategy.ShowMessage(("输入值" + objConfig.FieldCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                            IsError = true;
                                                            return;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (stationInfo != null && stationInfo.InnerView != null && IsError == false)
                                        {
                                            foreach (SampleLogIn objSamplingStation in ((ListView)stationInfo.InnerView).CollectionSource.List.Cast<SampleLogIn>().Where(a => a.StationLocation.Oid == objSampling.StationLocation.Oid).ToList())
                                            {
                                                foreach (SamplingFieldConfiguration objConfig in objMatrix.SamplingFieldConfiguration.Where(i => i.IsMandatory && i.FieldClass == FieldClass.Station))
                                                {
                                                    if (objectTypeInfo != null && objectTypeInfo.FindMember(objConfig.FieldID) != null)
                                                    {
                                                        var value = objSamplingStation.GetMemberValue(objConfig.FieldID);
                                                        if (value == null)
                                                        {
                                                            if (objLanguage.strcurlanguage == "En")
                                                            {
                                                                if (!string.IsNullOrEmpty(objConfig.FieldCustomCaption))
                                                                {
                                                                    Application.ShowViewStrategy.ShowMessage(("Enter the Value " + objConfig.FieldCustomCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                                    IsError = true;
                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    Application.ShowViewStrategy.ShowMessage(("Enter the Value " + objConfig.FieldCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                                    IsError = true;
                                                                    return;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!string.IsNullOrEmpty(objConfig.FieldCustomCaption))
                                                                {
                                                                    Application.ShowViewStrategy.ShowMessage(("输入值" + objConfig.FieldCustomCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                                    IsError = true;
                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    Application.ShowViewStrategy.ShowMessage(("输入值" + objConfig.FieldCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                                    IsError = true;
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (tasksMonitoringInfo != null && tasksMonitoringInfo.InnerView != null && IsError == false)
                                        {
                                            ITypeInfo objectTypeInfo1 = XafTypesInfo.Instance.FindTypeInfo(typeof(SampleParameter).FullName);
                                            foreach (SampleParameter objSamplingMonitoring in ((ListView)tasksMonitoringInfo.InnerView).CollectionSource.List.Cast<SampleParameter>().Where(a => a.Samplelogin.Oid == objSampling.Oid).ToList())
                                            {
                                                foreach (SamplingFieldConfiguration objConfig in objMatrix.SamplingFieldConfiguration.Where(i => i.IsMandatory && i.FieldClass == FieldClass.Test))
                                                {
                                                    if (objectTypeInfo1 != null && objectTypeInfo1.FindMember(objConfig.FieldID) != null)
                                                    {
                                                        var value = objSamplingMonitoring.GetMemberValue(objConfig.FieldID);
                                                        if (value == null)
                                                        {
                                                            if (objLanguage.strcurlanguage == "En")
                                                            {
                                                                if (!string.IsNullOrEmpty(objConfig.FieldCustomCaption))
                                                                {
                                                                    Application.ShowViewStrategy.ShowMessage(("Enter the Value " + objConfig.FieldCustomCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                                    IsError = true;
                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    Application.ShowViewStrategy.ShowMessage(("Enter the Value " + objConfig.FieldCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                                    IsError = true;
                                                                    return;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!string.IsNullOrEmpty(objConfig.FieldCustomCaption))
                                                                {
                                                                    Application.ShowViewStrategy.ShowMessage(("输入值" + objConfig.FieldCustomCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                                    IsError = true;
                                                                    return;
                                                                }
                                                                else
                                                                {
                                                                    Application.ShowViewStrategy.ShowMessage(("输入值" + objConfig.FieldCaption), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                                    IsError = true;
                                                                    return;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (IsError == false)
                        {
                            SamplingStatus status = SamplingStatus.PendingValidation;
                            IList<DefaultSetting> FDdefsetting = View.ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Sampling Management' And Not IsNullOrEmpty([NavigationItemName])"));
                            if (FDdefsetting != null && FDdefsetting.Count > 0)
                            {
                                DefaultSetting FD1defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "FieldDataReview1").FirstOrDefault();
                                DefaultSetting FD2defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "FieldDataReview2").FirstOrDefault();
                                if (FD1defsetting != null && FD2defsetting != null)
                                {
                                    if (FD1defsetting.Select == false && FD2defsetting.Select == true)
                                    {
                                        status = SamplingStatus.PendingApproval;
                                    }
                                }
                            }

                            Employee emp = tasksSamplingInfo.InnerView.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            if (emp != null)
                            {
                                foreach (SampleLogIn objSampling in ((ListView)tasksSamplingInfo.InnerView).SelectedObjects.Cast<SampleLogIn>().ToList())
                                {
                                    objSampling.EnteredBy = emp;
                                    objSampling.LastUpdatedBy = emp.FullName;
                                    objSampling.EnteredDate = DateTime.Now;
                                    objSampling.LastUpdatedDate = DateTime.Now.ToString();
                                    objSampling.CollectTime = objSampling.CollectDate.TimeOfDay;
                                    if (objSampling.SampleStatus == null || !objSampling.SampleStatus.Samplinghold)
                                    {
                                        objSampling.MonitoredBy = emp;
                                        objSampling.MonitoredDate = DateTime.Now;
                                    }
                                    objSampling.SamplingStatus = status;
                                }
                            }
                            ((ListView)tasksSamplingInfo.InnerView).ObjectSpace.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CompleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            e.ShowViewParameters.CreatedView = Application.CreateListView("Samplecheckin_ListView_FieldDataEntry", new CollectionSource(Application.CreateObjectSpace(), typeof(Samplecheckin)), true);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select the SampleID to complete", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FieldDataEntryEdit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                SampleLogIn objView = (SampleLogIn)os.GetObject<SampleLogIn>((SampleLogIn)View.CurrentObject);
                if (objView != null)
                {
                    DetailView dv = Application.CreateDetailView(os, "SampleLogIn_DetailView_FieldDataEntry", true, objView);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.Accepting += Popup_Accepted;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Popup_Accepted(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                SampleLogIn objSampling = (SampleLogIn)e.AcceptActionArgs.CurrentObject;
                if (objSampling != null)
                {
                    bool CollectDate = false;
                    bool Collector = false;
                    if (objSampling.CollectDate != null && objSampling.CollectDate != DateTime.MinValue)
                    {
                        CollectDate = true;
                    }
                    if (objSampling.Collector != null)
                    {
                        Collector = true;
                    }
                    if (CollectDate && Collector)
                    {
                        ObjectSpace.CommitChanges();
                        View.ObjectSpace.Refresh();
                    }
                    else if (CollectDate == false)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Enter the CollecteddDate", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (Collector == false)
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Enter the CollectedBy", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void FieldDataEntryCopyToAll_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SampleLogIn objectSampling = (SampleLogIn)View.CurrentObject;
                IList<SampleLogIn> lstSampling = ObjectSpace.GetObjects<SampleLogIn>(CriteriaOperator.Parse("[JobID.Oid]=?", objectSampling.JobID.Oid));
                if (lstSampling != null && lstSampling.Count > 0)
                {
                    foreach (SampleLogIn objSampling in lstSampling)
                    {
                        objSampling.WindDirection = objectSampling.WindDirection;
                        objSampling.Barp = objectSampling.Barp;
                        objSampling.Temp = objectSampling.Temp;
                        objSampling.Humidity = objectSampling.Humidity;
                        objSampling.WeatherCondition = objectSampling.WeatherCondition;
                        objSampling.Transparencyk = objectSampling.Transparencyk;
                        objSampling.Transparencyk1 = objectSampling.Transparencyk1;
                        objSampling.Transparencyk2 = objectSampling.Transparencyk2;
                        objSampling.Depth = objectSampling.Depth;
                        objSampling.RiverWidth = objectSampling.RiverWidth;
                        objSampling.FlowRate = objectSampling.FlowRate;
                        objSampling.CollectDate = objectSampling.CollectDate;
                        objSampling.Collector = objectSampling.Collector;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void FieldDataEntryCopyPrevious_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SampleLogIn objSampling = (SampleLogIn)View.CurrentObject;
                int sampleNo = objSampling.SampleNo;
                sampleNo = sampleNo - 1;
                SampleLogIn objectSampling = ObjectSpace.FindObject<SampleLogIn>(CriteriaOperator.Parse("[JobID.Oid]='" + objSampling.JobID.Oid + "'And [SampleNo]='" + sampleNo.ToString() + "'"));
                if (objSampling != null && objectSampling != null)
                {
                    objSampling.WindDirection = objectSampling.WindDirection;
                    objSampling.Barp = objectSampling.Barp;
                    objSampling.Temp = objectSampling.Temp;
                    objSampling.Humidity = objectSampling.Humidity;
                    objSampling.WeatherCondition = objectSampling.WeatherCondition;
                    objSampling.Transparencyk = objectSampling.Transparencyk;
                    objSampling.Transparencyk1 = objectSampling.Transparencyk1;
                    objSampling.Transparencyk2 = objectSampling.Transparencyk2;
                    objSampling.Depth = objectSampling.Depth;
                    objSampling.RiverWidth = objectSampling.RiverWidth;
                    objSampling.FlowRate = objectSampling.FlowRate;
                    objSampling.CollectDate = objectSampling.CollectDate;
                    objSampling.Collector = objectSampling.Collector;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FieldDataEntryCopyTo_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(os, typeof(SampleLogIn));
                ListView lv = Application.CreateListView("SampleLogIn_ListView_CopyTo_FieldDataEntry", cs, true);
                ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.Accepting += CopyTo_Accepting;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void CopyTo_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    if (TMInfo.strSamplingSampleID != null && TMInfo.TaskOid != null)
                    {
                        SampleLogIn objectSampling = ObjectSpace.FindObject<SampleLogIn>(CriteriaOperator.Parse("[Oid]=?", new Guid(TMInfo.strSamplingSampleID)), true);
                        if (objectSampling != null)
                        {
                            foreach (SampleLogIn sampling in e.AcceptActionArgs.SelectedObjects)
                            {
                                SampleLogIn objSampling = ObjectSpace.GetObject<SampleLogIn>(sampling);
                                if (objSampling != null)
                                {
                                    objSampling.WindDirection = objectSampling.WindDirection;
                                    objSampling.Barp = objectSampling.Barp;
                                    objSampling.Temp = objectSampling.Temp;
                                    objSampling.Humidity = objectSampling.Humidity;
                                    objSampling.WeatherCondition = objectSampling.WeatherCondition;
                                    objSampling.Transparencyk = objectSampling.Transparencyk;
                                    objSampling.Transparencyk1 = objectSampling.Transparencyk1;
                                    objSampling.Transparencyk2 = objectSampling.Transparencyk2;
                                    objSampling.Depth = objectSampling.Depth;
                                    objSampling.RiverWidth = objectSampling.RiverWidth;
                                    objSampling.FlowRate = objectSampling.FlowRate;
                                    objSampling.CollectDate = objectSampling.CollectDate;
                                    objSampling.Collector = objectSampling.Collector;
                                }
                            }
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Taskfde_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_ListView_FieldDataEntry")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objspace, typeof(SampleLogIn));
                    ListView createListview = Application.CreateListView("SampleLogIn_ListView_FieldDataEntry_History", cs, true);
                    Frame.SetView(createListview);
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
                if (!string.IsNullOrEmpty(parameter))
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    string[] param = parameter.Split('|');
                    if (param[0] == "StationLocation.Oid")
                    {
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            SampleLogIn objSample = os.GetObjectByKey<SampleLogIn>(currentOid);
                            if (objSample != null && !string.IsNullOrEmpty(objSample.AlternativeStationOid))
                            {
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                CollectionSource cs = new CollectionSource(os, typeof(SampleSites));
                                List<string> stations = objSample.AlternativeStationOid.Split(new[] { "; " }, StringSplitOptions.None).ToList();
                                stations.Add(objSample.StationLocation.Oid.ToString());
                                cs.Criteria["Filter1"] = CriteriaOperator.Parse("[Oid] In (" + string.Format("'{0}'", string.Join("','", stations.Select(i => i.Replace("'", "''")))) + ")");
                                showViewParameters.CreatedView = Application.CreateListView("SampleSites_LookupListView_Sampling_StationLocation", cs, false);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.AcceptAction.Execute += AcceptAction_Execute;
                                dc.Accepting += Dc_Accepting;
                                dc.SaveOnAccept = false;
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
                if (e.AcceptActionArgs.SelectedObjects.Count != 1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["rowid"] != null)
                {
                    SampleLogIn objsampling = ((ListView)View).CollectionSource.List.Cast<SampleLogIn>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                    if (objsampling != null)
                    {
                        DashboardViewItem viSamplingInformation = (DashboardViewItem)((DetailView)Application.MainWindow.View).FindItem("SamplingInformation");
                        if (viSamplingInformation != null && viSamplingInformation.InnerView != null)
                        {
                            SampleSites objSite = e.SelectedObjects.Cast<SampleSites>().FirstOrDefault();
                            if (objSite != null)
                            {
                                foreach (SampleLogIn samples in ((ListView)viSamplingInformation.InnerView).CollectionSource.List.Cast<SampleLogIn>().Where(a => a.StationLocation.Oid == objsampling.StationLocation.Oid && a.Oid != objsampling.Oid).ToList())
                                {
                                    samples.StationLocation = ((ListView)viSamplingInformation.InnerView).ObjectSpace.GetObjectByKey<SampleSites>(objSite.Oid);
                                    samples.PWSID = objSite.PWSID;
                                    samples.KeyMap = objSite.KeyMap;
                                    samples.Address = objSite.Address;
                                    samples.SamplePointID = objSite.SamplePointID;
                                    samples.SamplePointType = objSite.SamplePointType;
                                    if (objSite.SystemType != null)
                                    {
                                        samples.SystemType = ((ListView)viSamplingInformation.InnerView).ObjectSpace.GetObjectByKey<SystemTypes>(objSite.SystemType.Oid);
                                    }
                                    samples.PWSSystemName = objSite.PWSSystemName;
                                    samples.RejectionCriteria = objSite.RejectionCriteria;
                                    if (objSite.WaterType != null)
                                    {
                                        samples.WaterType = ((ListView)viSamplingInformation.InnerView).ObjectSpace.GetObjectByKey<WaterTypes>(objSite.WaterType.Oid);
                                    }
                                    samples.ParentSampleID = objSite.ParentSampleID;
                                    samples.ParentSampleDate = objSite.ParentSampleDate;
                                }
                                SampleLogIn sample = ((ListView)View).CollectionSource.List.Cast<SampleLogIn>().Where(a => a.StationLocation.Oid == objsampling.StationLocation.Oid && a.Oid == objsampling.Oid).First();
                                if (sample != null)
                                {
                                    sample.StationLocation = ((ListView)View).ObjectSpace.GetObjectByKey<SampleSites>(objSite.Oid);
                                    sample.PWSID = objSite.PWSID;
                                    sample.KeyMap = objSite.KeyMap;
                                    sample.Address = objSite.Address;
                                    sample.SamplePointID = objSite.SamplePointID;
                                    sample.SamplePointType = objSite.SamplePointType;
                                    if (objSite.SystemType != null)
                                    {
                                        sample.SystemType = ((ListView)View).ObjectSpace.GetObjectByKey<SystemTypes>(objSite.SystemType.Oid);
                                    }
                                    sample.PWSSystemName = objSite.PWSSystemName;
                                    sample.RejectionCriteria = objSite.RejectionCriteria;
                                    if (objSite.WaterType != null)
                                    {
                                        sample.WaterType = ((ListView)View).ObjectSpace.GetObjectByKey<WaterTypes>(objSite.WaterType.Oid);
                                    }
                                    sample.ParentSampleID = objSite.ParentSampleID;
                                    sample.ParentSampleDate = objSite.ParentSampleDate;
                                }
                                ((ListView)viSamplingInformation.InnerView).Refresh();
                                ((ListView)viSamplingInformation.InnerView).RefreshDataSource();
                                ((ListView)View).Refresh();
                                ((ListView)View).RefreshDataSource();
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
    }
}
