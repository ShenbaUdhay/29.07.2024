using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using LDM.Module.Web.Editors.QueryPanel;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace BTLIMS.Module.Controllers.Reporting
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class QueryPanelViewController : ViewController
    {
        viewInfo strviewid = new viewInfo();
        AnalysisDeptUser analysisDeptUser = new AnalysisDeptUser();
        MessageTimer timer = new MessageTimer();
        ResultEntrySelectionInfo resultentryinfo = new ResultEntrySelectionInfo();
        IObjectSpace ResultEntryObjectSpace;
        bool IsNew = false;
        //GridControl     gridlisteditor;
        #region Declaration
        public bool QueryPanel = false;
        ASPxGridListEditor gridlisteditor;
        ResultEntryQueryPanelInfo objQPInfo = new ResultEntryQueryPanelInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        CaseNarativeInfo CNInfo = new CaseNarativeInfo();
        //public DateTime rgFilterByMonthDate = DateTime.Now;
        object rgFilterValue;
        SingleChoiceAction ResultEntrySelection;
        #endregion

        #region Constructor
        public QueryPanelViewController()
        {
            InitializeComponent();
            RegisterActions(components);
            TargetViewId = "SampleParameter_ListView_Copy_Reporting;" + "ResultEntryQueryPanel_DetailView;" + "Project_LookupListView;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;" + "SampleParameter_ListView_Copy_ResultView_SingleChoice;" +
                "Reporting_ListView;" + "SampleParameter_ListView_Copy_CustomReporting;" + "SampleParameter_ListView_Copy_ResultValidation;" +
                "SampleParameter_ListView_Copy_ResultApproval;" + "SampleParameter_ListView_Copy_ResultView;" + "SampleParameter_LookupListView;" +
                "SampleParameter_LookupListView_Copy_JobID;" + "Samplecheckin_LookupListView_Copy_ResultEntryQueryPanel;" +
           "Customer_LookupListView;" + "Matrix_LookupListView;" + "Test_LookupListView;" + "Method_LookupListView;" +
           "SampleParameter_ListView_Copy_ResultEntry;" + "TestMethod_LookupListView_Copy_ResultEntryQueryPanel;" +
           "Result_DV;" + "SampleParameter_ListView_Copy_ResultEntry_Main;" + "SampleParameter_ListView_Copy_ResultView_Main;" +
           "ResultEntryQueryPanel_DetailView;" + "ResultEntryQueryPanel_DetailView_Copy;" + "SampleParameter_ListView_Copy_QCResultEntry;" +
           "QCBatch_ListView_ResultEntry;" + "ResultViewQueryPanel_DetailView;" + "QCBatch_ListView_ResultView;" + "SampleParameter_ListView_Copy_QCResultView;"
           + "SampleParameter_ListView_ResultView_ABID;" + "Samplecheckin_ListView_ResultEntry;" + "Samplecheckin_ListView_ResultView;" + "ResultEntry_Enter;";
            SampleSelectionMode.TargetViewId = "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;" + "SampleParameter_ListView_Copy_ResultView_SingleChoice;";
            Comment.TargetViewId = "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;" + "SampleParameter_ListView_Copy_ResultView_SingleChoice;";
            Loaddefault.TargetViewId = "SampleParameter_ListView_Copy_ResultEntry;";
            ResultViewHistory.TargetViewId = "ResultEntryQueryPanel_DetailView_Copy;";
            resultEntryDateFilterAction.TargetViewId = "ResultEntryQueryPanel_DetailView_Copy;" + "ResultViewQueryPanel_DetailView;";// "SampleParameter_ListView_Copy_ResultEntry";
            RetriveResultEntry.TargetViewId = "ResultEntryQueryPanel_DetailView_Copy;" + "ResultViewQueryPanel_DetailView;";

            ResultEntrySelection = new SingleChoiceAction(this, "ResultEntrySelection", DevExpress.Persistent.Base.PredefinedCategory.View)
            {
                Caption = "Selection"
            };
            ResultEntrySelection.TargetViewId = "ResultEntryQueryPanel_DetailView_Copy;" + "ResultViewQueryPanel_DetailView;";
            ResultEntrySelection.ConfirmationMessage = null;
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem8 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem9 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            ResultEntrySelection.Items.Add(choiceActionItem8);
            ResultEntrySelection.Items.Add(choiceActionItem9);
            ResultEntrySelection.ToolTip = null;
            choiceActionItem8.Caption = "JOBID";
            choiceActionItem8.Id = "ResultEntryJOBID";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem8.ToolTip = null;
            choiceActionItem9.Caption = "QCBatchID";
            choiceActionItem9.Id = "ResultEntryQCBatchID";
            choiceActionItem9.ImageName = null;
            choiceActionItem9.Shortcut = null;
            choiceActionItem9.ToolTip = null;
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View != null && (View.Id == "SampleParameter_ListView_Copy_CustomReporting" || View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                    || View.Id == "SampleParameter_ListView_Copy_QCResultView" || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "Result_DV"))
                {
                    View.ControlsCreated += QueryPanelViewController_ViewControlsCreated;
                }
                if ((ObjectSpace is NonPersistentObjectSpace) && (View.CurrentObject == null))
                {
                    View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
                    ((DetailView)View).ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                }
                CriteriaOperator criteria = null;
                if (View.Id == "SampleParameter_ListView_Copy_Reporting")
                {
                    criteria = CriteriaOperator.Parse("[Samplelogin.GCRecord] IS NULL");
                }
                else if (View != null && (View.Id == "Samplecheckin_LookupListView_Copy_ResultEntryQueryPanel" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" || View.Id == "QCBatch_ListView_ResultEntry" || View.Id == "QCBatch_ListView_ResultView" || View.Id == "Samplecheckin_ListView_ResultEntry" || View.Id == "Samplecheckin_ListView_ResultView"))
                {
                    if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" || View.Id == "Samplecheckin_ListView_ResultEntry" || View.Id == "Samplecheckin_ListView_ResultView")
                    {
                        objQPInfo.ResultEntryQueryFilter = string.Empty;
                        objQPInfo.CurrentViewID = View.Id;
                    }
                    if (View != null && (View.Id == "QCBatch_ListView_ResultEntry" || View.Id == "QCBatch_ListView_ResultView"))
                    {
                        objQPInfo.QCResultEntryQueryFilter = string.Empty;
                    }
                }
                //Application.DashboardViewCreating += Application_DashboardViewCreating;
                //Application.DashboardViewCreated += Application_DashboardViewCreated;
                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice")
                {
                    if (SampleSelectionMode.SelectedItem == null)
                    {
                        SampleSelectionMode.SelectedItem = SampleSelectionMode.Items[0];
                    }
                    SampleSelectionMode.SelectedItemChanged += SampleSelectionMode_SelectedItemChanged;
                    SampleSelectionMode.Executed += SampleSelectionMode_Executed;
                    ((ListView)View).Editor.ControlsCreated += new EventHandler(Editor_ControlsCreated);
                    ((ListView)View).CollectionSource.List.Cast<SampleParameter>().ToList().Where(i=>i.Testparameter!=null && !string.IsNullOrEmpty(i.Testparameter.DefaultResult) && string.IsNullOrEmpty(i.Result)).ToList().ForEach(i => { i.Result = i.Testparameter.DefaultResult;});
                    IList<SampleParameter> lstQcSamples = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("" + objQPInfo.QCResultEntryQueryFilter + " AND [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample' And [Status] ='" + Samplestatus.PendingEntry + "' And [GCRecord] IS NULL And ([SubOut] Is Null Or [SubOut] = False)"));
                    lstQcSamples.ToList().Where(i => i.Testparameter != null && !string.IsNullOrEmpty(i.Testparameter.DefaultResult) && string.IsNullOrEmpty(i.Result)).ToList().ForEach(i => { i.Result = i.Testparameter.DefaultResult; });
                    if (View.ObjectSpace.ModifiedObjects.Count>0)
                    {
                        View.ObjectSpace.CommitChanges();
                    }
                    IsNew = true;
                }
                if (View != null && (View.Id == "ResultEntryQueryPanel_DetailView_Copy" || View.Id == "ResultEntry_Enter" || View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "ResultViewQueryPanel_DetailView"))
                {
                    if (ResultEntrySelection.SelectedItem == null)
                    {
                        ResultEntrySelection.SelectedItem = ResultEntrySelection.Items[0];
                    }
                    ResultEntrySelection.SelectedItemChanged += ResultEntrySelection_SelectedItemChanged;
                    if (View.Id == "ResultEntryQueryPanel_DetailView_Copy" && View.CurrentObject == null)
                    {
                        View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
                        ((DetailView)View).ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;

                        ////IObjectSpace os = Application.CreateObjectSpace();
                        ////ResultEntryQueryPanel resultEntryQueryPanel = os.CreateObject<ResultEntryQueryPanel>();
                        ////////Frame.GetController<DetailViewController>().Active["AssignNewObject"] = false;
                        ////View.CurrentObject = View.ObjectSpace.GetObject(resultEntryQueryPanel);
                        ////////Frame.GetController<DetailViewController>().Active["AssignNewObject"] = true;
                        ////////DetailView view = Application.CreateDetailView(os, "ResultEntryQueryPanel_DetailView_Copy", true, resultEntryQueryPanel);
                        ////////view.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                        ////////Frame.SetView(view);
                    }
                    else if (View.Id == "ResultViewQueryPanel_DetailView" && View.CurrentObject == null)
                    {
                        objQPInfo.IsQueryPanelOpened = true;
                        IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ResultEntryQueryPanel));
                        ResultEntryQueryPanel newQuery = objectSpace.CreateObject<ResultEntryQueryPanel>();
                        //newQuery.FilterDataByMonth = FilterByMonthEN._1M;
                        //newQuery.SelectionMode = QueryMode.Job;
                        //objQPInfo.SelectMode = QueryMode.Job;
                        DetailView detailView = Application.CreateDetailView(objectSpace, "ResultViewQueryPanel_DetailView", false, newQuery);
                        detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                        Frame.SetView(detailView);
                        //e.Handled = true;
                    }
                    //if (resultEntryDateFilterAction.SelectedItem == null)
                    //{
                    //    resultEntryDateFilterAction.SelectedItem = resultEntryDateFilterAction.Items[0];
                    //}
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (resultEntryDateFilterAction.SelectedItem == null)
                    {
                        if (setting.AnalysisEntryModel == EnumDateFilter.OneMonth)
                        {
                        resultEntryDateFilterAction.SelectedItem = resultEntryDateFilterAction.Items[0];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.ThreeMonth)
                        {
                            resultEntryDateFilterAction.SelectedItem = resultEntryDateFilterAction.Items[1];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.SixMonth)
                        {
                            resultEntryDateFilterAction.SelectedItem = resultEntryDateFilterAction.Items[2];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.OneYear)
                        {
                            resultEntryDateFilterAction.SelectedItem = resultEntryDateFilterAction.Items[3];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.TwoYear)
                        {
                            resultEntryDateFilterAction.SelectedItem = resultEntryDateFilterAction.Items[4];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-2);
                    }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.FiveYear)
                        {
                            resultEntryDateFilterAction.SelectedItem = resultEntryDateFilterAction.Items[5];
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-5);
                        }
                        else
                        {
                            resultEntryDateFilterAction.SelectedItem = resultEntryDateFilterAction.Items[6];
                            objQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    //objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    //objQPInfo.SelectMode = Modules.BusinessObjects.SampleManagement.QueryMode.Job;
                    resultEntryDateFilterAction.SelectedItemChanged += ResultEntryDateFilterAction_SelectedItemChanged;

                }
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                if (View.CurrentObject != null && View.ObjectTypeInfo.Type == typeof(ResultEntryQueryPanel))
                {

                    objQPInfo.ResultEntryQueryFilter = string.Empty;
                    objQPInfo.lstJobID = new List<string>();
                    objQPInfo.QCResultEntryQueryFilter = string.Empty;
                    objQPInfo.lstQCBatchID = new List<string>();

                    QueryPanel = true;
                    objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    ResultEntryQueryPanel REQPanel = (ResultEntryQueryPanel)View.CurrentObject;
                    if (View is DetailView)
                    {
                        PropertyEditor rgEditor = (PropertyEditor)((DetailView)View).FindItem("FilterDataByMonth");
                        if (rgEditor != null)
                        {
                            rgEditor.ControlCreated += RgEditor_ControlCreated;
                        }
                        ////DashboardViewItem viAB = (DashboardViewItem)((DetailView)View).FindItem("viewItemABID");
                        ////if (viAB != null)
                        ////{
                        ////    viAB.ControlCreated += ViAB_ControlCreated;
                        ////}
                        ////DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                        ////if (viQC != null)
                        ////{
                        ////    viQC.ControlCreated += ViQC_ControlCreated;
                        ////}
                        ////DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                        ////if (viJob != null)
                        ////{
                        ////    viJob.ControlCreated += ViJob_ControlCreated;
                        ////}
                    }
                }
                else if (View.Id == "Samplecheckin_ListView_ResultEntry" || View.Id == "Samplecheckin_ListView_ResultView")
                {
                    strviewid.strtempresultentryviewid = View.Id.ToString();
                    //ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    //lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;
                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)this.ObjectSpace).Session.DataLayer);
                    XPClassInfo sampleParameterinfo = uow.GetClassInfo(typeof(SampleParameter));
                    DefaultSetting objDefault = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True"));
                    DefaultSetting objDefaultmodule = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparationRootNode'AND [Select]=True"));
                    if (View.Id == "Samplecheckin_ListView_ResultEntry")
                    {
                        if (objDefault != null && objDefaultmodule != null)
                        {
                            IList<SampleParameter> objSampleParameters = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.JobID.Status]  >= 2 And [Status] = 'PendingEntry' And [SignOff] = True And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [IsPrepMethodComplete]  = True) OR [Testparameter.TestMethod.PrepMethods][].Count() == 0 And ([IsPrepMethodComplete]  = False Or [IsPrepMethodComplete] Is Null ))"), null, int.MaxValue, false, true).Cast<SampleParameter>().ToList().Where(p => p.Samplelogin != null && p.Samplelogin.JobID != null && p.TestHold==false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).ToList();
                            ((ListView)View).CollectionSource.Criteria["Filter2"] = new InOperator("Oid", objSampleParameters.Select(i => i.Samplelogin.JobID.Oid));
                        }
                        else
                        {
                            IList<SampleParameter> objSampleParameters = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.JobID.Status]  >= 2 And [Status] = 'PendingEntry' And [SignOff] = True"), null, int.MaxValue, false, true).Cast<SampleParameter>().ToList().Where(p => p.Samplelogin != null && p.Samplelogin.JobID != null && p.TestHold==false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).ToList();
                            ((ListView)View).CollectionSource.Criteria["Filter2"] = new InOperator("Oid", objSampleParameters.Select(i => i.Samplelogin.JobID.Oid));
                        }

                    }
                    else
                    {
                        IList<SampleParameter> objSampleParameters = uow.GetObjects(sampleParameterinfo, CriteriaOperator.Parse("[Samplelogin.JobID.Status] >=3 And [SignOff] = True"), null, int.MaxValue, false, true).Cast<SampleParameter>().ToList().Where(p => p.Samplelogin != null && p.Samplelogin.JobID != null && p.TestHold==false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).ToList();
                        ((ListView)View).CollectionSource.Criteria["Filter2"] = new InOperator("Oid", objSampleParameters.Select(i => i.Samplelogin.JobID.Oid));
                    }
                }
                if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" /*|| View.Id == "QCBatch_ListView_ResultEntry")*/))
                {
                    ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;
                }
                else if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultView_Main" /*|| View.Id == "QCBatch_ListView_ResultView")*/))
                {
                    ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;
                }
                else if (View != null && View.Id == "SampleParameter_ListView_ResultView_ABID")
                {
                    ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Editor_ControlsCreated(object sender, EventArgs e)
        {
            ASPxGridView gridView = ((ListEditor)sender).Control as ASPxGridView;
            gridView.DataBound += new EventHandler(gridView_DataBound);
            gridView.Load += new EventHandler(gridView_Loads);
        }
        private void gridView_Loads(object sender, EventArgs e)
        {
            UpdateColumnVisible((ASPxGridView)sender);
        }
        private void gridView_DataBound(object sender, EventArgs e)
        {
            UpdateColumnVisible((ASPxGridView)sender);
        }
        private void UpdateColumnVisible(ASPxGridView gridView)
        {
            if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice")
            {
                if (SampleSelectionMode != null && SampleSelectionMode.SelectedItem != null)
                {
                    if (SampleSelectionMode.SelectedItem.Id == "Sample")
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            foreach (WebColumnBase column in gridListEditor.Grid.Columns)
                            {
                                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                if (columnInfo != null)
                                {
                                    IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                                    if (modelColumn.Id == "DateReceived" || modelColumn.Id == "SpikeAmount" || modelColumn.Id == "SpikeAmountUnit" || modelColumn.Id == "REC" || modelColumn.Id == "RPD")
                                    {
                                        column.Visible = false;
                                    }
                                    else if (modelColumn.Id == "ReceivedDates" /*|| modelColumn.Id == "QcBatchID"*/ || modelColumn.Id == "QCType")
                                    {
                                        column.Visible = false;
                                    }
                                }
                            }
                            gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                            if (gridView.Columns["QcBatchID"] != null)
                            {
                                gridView.Columns["QcBatchID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.Columns["QCType"] != null)
                            {
                                gridView.Columns["QCType"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.Columns["SysSampleCode"] != null)
                            {
                                gridView.Columns["SysSampleCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                    }
                    else
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            foreach (WebColumnBase column in gridListEditor.Grid.Columns)
                            {
                                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                if (columnInfo != null)
                                {
                                    IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                                    if (modelColumn.Id == "SCDateReceived" || modelColumn.Id == "SpikeAmount" || modelColumn.Id == "SpikeAmountUnit" || modelColumn.Id == "REC" || modelColumn.Id == "RPD"
                                        || modelColumn.Id == "QcBatchID" || modelColumn.Id == "QCType")
                                    {
                                        column.Visible = true;
                                    }
                                    else if (modelColumn.Id == "ReceivedDates" || modelColumn.Id == "ClientSampleID")
                                    {
                                        column.Visible = false;
                                    }
                                }
                            }
                            gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                            if (gridView.Columns["QcBatchID"] != null)
                            {
                                gridView.Columns["QcBatchID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.Columns["QCType"] != null)
                            {
                                gridView.Columns["QCType"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.Columns["SysSampleCode"] != null)
                            {
                                gridView.Columns["SysSampleCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                    }
                }
            }
        }
        private void ResultEntryDetailsView_Closed(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                {
                    NavigationRefresh objnavigationRefresh = new NavigationRefresh();
                    if (!string.IsNullOrEmpty(objnavigationRefresh.ClickedNavigationItem) && objnavigationRefresh.ClickedNavigationItem == "Result Entry")
                    {
                        ResultEntryQueryPanelInfo resultEntryQueryPanelInfo = new ResultEntryQueryPanelInfo();
                        objQPInfo.IsQueryPanelOpened = true;
                        IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ResultEntryQueryPanel));
                        ResultEntryQueryPanel newQuery = objectSpace.CreateObject<ResultEntryQueryPanel>();
                        resultEntryQueryPanelInfo.ResultEntryCurrentobject = newQuery;
                        DetailView detailView = Application.CreateDetailView(objectSpace, "ResultEntryQueryPanel_DetailView_Copy", false, newQuery);
                        detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                        Frame.SetView(detailView);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ResultEntrySelection_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "ResultEntryQueryPanel_DetailView_Copy" || View.Id == "ResultViewQueryPanel_DetailView"))
                {
                    DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                    DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                    DashboardViewItem viAB = (DashboardViewItem)((DetailView)View).FindItem("viewItemABID");
                    if (ResultEntrySelection.SelectedItem.Id == "ResultEntryJOBID")
                    {
                        objQPInfo.ResultEntryCurrentselectionid = "ResultEntryJOBID";
                        if (viJob != null)
                        {
                            ((Control)viJob.Control).Visible = true;
                        }
                        if (viQC != null)
                        {
                            ((Control)viQC.Control).Visible = false;
                        }
                        if (viAB != null)
                        {
                            ((Control)viAB.Control).Visible = false;
                        }
                    }
                    else
                    {
                        objQPInfo.ResultEntryCurrentselectionid = "ResultEntryQCBatchID";
                        if (viJob != null)
                        {
                            ((Control)viJob.Control).Visible = false;
                        }
                        if (viQC != null)
                        {
                            ((Control)viQC.Control).Visible = true;
                        }
                        if (viAB != null)
                        {
                            ((Control)viAB.Control).Visible = true;
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
        private void SampleSelectionMode_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                {
                    resultentryinfo.IsResultEntrySelectionChanged = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SampleSelectionMode_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")//if (View != null && (View.Id == "ResultEntry_Enter" || View.Id == "ResultEntry_View"))
                {
                    if (!IsNew)
                    {
                        resultentryinfo.IsResultEntrySelectionChanged = true;
                    }
                    IsNew = false;
                    strviewid.strtempresultentryviewid = View.Id;
                    ListView lstsample = ((ListView)View);
                    if (SampleSelectionMode != null && SampleSelectionMode.SelectedItem != null)
                    {
                        if (SampleSelectionMode.SelectedItem.Id == "Sample")
                        {
                            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (gridListEditor != null && gridListEditor.Grid != null)
                            {
                                ASPxGridView gridView = gridListEditor.Grid;
                                //foreach (GridViewColumn column in gridView.Columns.Cast<GridViewColumn>().Where(i => i.VisibleIndex >= 0).ToList())
                                //{
                                //    if (column.Name == "DateReceived" || column.Caption == "SpikeAmount" || column.Caption == "SpikeAmountUnit" || column.Caption == "REC" || column.Caption == "RPD")
                                //    {
                                //        column.Visible = false;
                                //    }
                                //}
                                //foreach (WebColumnBase column in gridView.Columns)
                                //{
                                //    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                //    if (columnInfo != null)
                                //    {
                                //        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                                //        if(modelColumn.Id == "DateReceived" || modelColumn.Id == "SpikeAmount" || modelColumn.Id == "SpikeAmountUnit" || modelColumn.Id == "REC" || modelColumn.Id == "RPD")
                                //        {
                                //            column.Visible = false;
                                //        }
                                //        else if(modelColumn.Id == "ReceivedDates" || modelColumn.Id == "QcBatchID" || modelColumn.Id == "QCType")
                                //        {
                                //            column.Visible = false;
                                //        }
                                //    }
                                //}
                                gridListEditor.Grid.ClearSort();
                                if (gridListEditor.Grid.Columns["SysSampleCode"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["SysSampleCode"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.TestMethod.TestName"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.TestMethod.TestName"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.TestMethod.MethodName.MethodNumber"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.TestMethod.MethodName.MethodNumber"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.Sort"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.Sort"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.Parameter.ParameterName"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.Parameter.ParameterName"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["RecLCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RecLCLimit"].Visible = false;
                                }
                                if (gridListEditor.Grid.Columns["RecHCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RecHCLimit"].Visible = false;
                                }
                                if (gridListEditor.Grid.Columns["RPDLCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RPDLCLimit"].Visible = false;
                                }
                                if (gridListEditor.Grid.Columns["RPDHCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RPDHCLimit"].Visible = false;
                                }
                            }
                            if (!string.IsNullOrEmpty(objQPInfo.ResultEntryADCFilter))
                            {
                                lstsample.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND " + objQPInfo.ResultEntryADCFilter + " And [Testparameter.QCType.QCTypeName] = 'Sample' And [Status] ='" + Samplestatus.PendingEntry + "' And [GCRecord] IS NULL And ([SubOut] Is Null Or [SubOut] = False) And ([TestHold] = False Or [TestHold] Is null)");/*And [SubOut] Is Null Or [SubOut] = False*/
                            }
                            else
                            {
                                lstsample.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Testparameter.QCType.QCTypeName] = 'Sample' And [Status] ='" + Samplestatus.PendingEntry + "' And [GCRecord] IS NULL And ([SubOut] Is Null Or [SubOut] = False)And ([TestHold] = False Or [TestHold] Is null)");/*And [SubOut] Is Null Or [SubOut] = False*/
                            }
                            if (resultentryinfo.lstresultentry != null && resultentryinfo.lstresultentry.Count > 0)
                            {
                                resultentryinfo.IsResultEntrySelectionChanged = true;
                                //resultentryinfo.lsttempresultentry = new List<SampleParameter>();
                                //resultentryinfo.lsttempresultentry = resultentryinfo.lstresultentry;
                                ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                                foreach (SampleParameter objsmplpara in lstsample.CollectionSource.List)
                                {
                                    resultentryinfo.IsResultEntrySelectionChanged = true;
                                    if (resultentryinfo.lstresultentry.Contains(objsmplpara))
                                    {
                                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                                        {
                                            gridlisteditor.Grid.Selection.SelectRowByKey(objsmplpara.Oid);
                                        }
                                    }
                                }
                                //resultentryinfo.lstresultentry = resultentryinfo.lsttempresultentry;
                                //resultentryinfo.IsResultEntrySelectionChanged = false;
                            }
                            gridListEditor.IsFooterVisible = true;
                            ASPxSummaryItem customCount = new ASPxSummaryItem();
                            customCount.FieldName = "SysSampleCode";
                            customCount.ShowInColumn = "SysSampleCode";
                            customCount.SummaryType = SummaryItemType.Count;
                            gridListEditor.Grid.TotalSummary.Clear();
                            gridListEditor.Grid.TotalSummary.Add(customCount);
                        }
                        else
                        {
                            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (gridListEditor != null && gridListEditor.Grid != null)
                            {
                                ASPxGridView gridView = gridListEditor.Grid;
                                //foreach (GridViewColumn column in gridView.Columns.Cast<GridViewColumn>().Where(i => i.VisibleIndex >= 0).ToList())
                                //{
                                //    if (column.Name == "DateReceived" || column.Caption == "SpikeAmount" || column.Caption == "SpikeAmountUnit" || column.Caption == "REC" || column.Caption == "RPD")
                                //    {
                                //        column.Visible = true;
                                //    }
                                //    if(column.Name == "ReceivedDate")
                                //    {
                                //        column.Visible = false;
                                //    }
                                //}
                                //foreach (WebColumnBase column in gridView.Columns)
                                //{
                                //    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                //    if (columnInfo != null)
                                //    {
                                //        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                                //        if (modelColumn.Id == "SCDateReceived" || modelColumn.Id == "SpikeAmount" || modelColumn.Id == "SpikeAmountUnit" || modelColumn.Id == "REC" || modelColumn.Id == "RPD"
                                //            || modelColumn.Id == "QcBatchID" || modelColumn.Id == "QCType")
                                //        {
                                //            column.Visible = true;
                                //        }
                                //        else if(modelColumn.Id == "ReceivedDates" || modelColumn.Id== "ClientSampleID")
                                //        {
                                //            column.Visible = false;
                                //        }
                                //    }
                                //}
                                gridListEditor.Grid.ClearSort();
                                if (gridListEditor.Grid.Columns["QcBatchID"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["QcBatchID"], ColumnSortOrder.Ascending);
                                    gridListEditor.Grid.Columns["QcBatchID"].Width = 120;
                                }
                                if (gridListEditor.Grid.Columns["QCType"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["QCType"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["SysSampleCode"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["SysSampleCode"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.TestMethod.TestName"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.TestMethod.TestName"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.TestMethod.MethodName.MethodNumber"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.TestMethod.MethodName.MethodNumber"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.Sort"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.Sort"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.Parameter.ParameterName"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.Parameter.ParameterName"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["RecLCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RecLCLimit"].Visible = true;
                                }
                                if (gridListEditor.Grid.Columns["RecHCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RecHCLimit"].Visible = true;
                                }
                                if (gridListEditor.Grid.Columns["RPDLCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RPDLCLimit"].Visible = true;
                                }
                                if (gridListEditor.Grid.Columns["RPDHCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RPDHCLimit"].Visible = true;
                                }
                                //gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            }
                            if (!string.IsNullOrEmpty(objQPInfo.ResultEntryADCFilter))
                            {
                                lstsample.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.QCResultEntryQueryFilter + " AND " + objQPInfo.ResultEntryADCFilter + " AND [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample' And [Status] ='" + Samplestatus.PendingEntry + "' And [GCRecord] IS NULL And ([SubOut] Is Null Or [SubOut] = False)");
                            }
                            else
                            {
                                objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.qcseqdetail.Jobid] Like '%" + objQPInfo.objJobID + "%' ";
                                lstsample.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.QCResultEntryQueryFilter + " AND [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample' And [Status] ='" + Samplestatus.PendingEntry + "' And [GCRecord] IS NULL And ([SubOut] Is Null Or [SubOut] = False)");
                            }
                            if (resultentryinfo.lstresultentry != null && resultentryinfo.lstresultentry.Count > 0)
                            {
                                resultentryinfo.IsResultEntrySelectionChanged = true;
                                //resultentryinfo.lsttempresultentry = new List<SampleParameter>();
                                //resultentryinfo.lsttempresultentry = resultentryinfo.lstresultentry;
                                ASPxGridListEditor gridlisteditor = ((ListView)View).Editor as ASPxGridListEditor;
                                foreach (SampleParameter objsmplpara in lstsample.CollectionSource.List)
                                {
                                    resultentryinfo.IsResultEntrySelectionChanged = true;
                                    if (resultentryinfo.lstresultentry.Contains(objsmplpara))
                                    {
                                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                                        {
                                            gridlisteditor.Grid.Selection.SelectRowByKey(objsmplpara.Oid);
                                        }
                                    }
                                }
                                //resultentryinfo.lstresultentry = resultentryinfo.lsttempresultentry;
                                //resultentryinfo.IsResultEntrySelectionChanged = false;
                            }
                            gridListEditor.IsFooterVisible = true;
                            ASPxSummaryItem customCount = new ASPxSummaryItem();
                            customCount.FieldName = "QcBatchID";
                            customCount.ShowInColumn = "QcBatchID";
                            customCount.SummaryType = SummaryItemType.Count;
                            gridListEditor.Grid.TotalSummary.Clear();
                            gridListEditor.Grid.TotalSummary.Add(customCount);
                        }
                    }
                }
                else if (View != null && View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice")
                {
                    strviewid.strtempresultentryviewid = View.Id;
                    ListView lstsample = ((ListView)View);
                    if (SampleSelectionMode != null && SampleSelectionMode.SelectedItem != null)
                    {
                        if (SampleSelectionMode.SelectedItem.Id == "Sample")
                        {
                            lstsample.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Testparameter.QCType.QCTypeName] = 'Sample' And [GCRecord] IS NULL"); //And [Status] <>'" + Samplestatus.PendingEntry + "'
                            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (gridListEditor != null && gridListEditor.Grid != null)
                            {
                                ASPxGridView gridView = gridListEditor.Grid;
                                //foreach (GridViewColumn column in gridView.Columns.Cast<GridViewColumn>().Where(i => i.VisibleIndex >= 0).ToList())
                                //{
                                //    if (column.Name == "DateReceived" || column.Caption == "SpikeAmount" || column.Caption == "SpikeAmountUnit" || column.Caption == "REC" || column.Caption == "RPD")
                                //    {
                                //        column.Visible = false;
                                //    }
                                //}
                                //foreach (WebColumnBase column in gridView.Columns)
                                //{
                                //    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                //    if (columnInfo != null)
                                //    {
                                //        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                                //        if (modelColumn.Id == "DateReceived" || modelColumn.Id == "SpikeAmount" || modelColumn.Id == "SpikeAmountUnit" || modelColumn.Id == "Rec" || modelColumn.Id == "RPD")
                                //        {
                                //            column.Visible = false;
                                //        }
                                //        else if (modelColumn.Id == "ReceivedDates" || modelColumn.Id == "QcBatchID" || modelColumn.Id == "QCType")
                                //        {
                                //            column.Visible = false;
                                //        }
                                //    }
                                //}
                                gridListEditor.Grid.ClearSort();
                                if (gridListEditor.Grid.Columns["SysSampleCode"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["SysSampleCode"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.TestMethod.TestName"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.TestMethod.TestName"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.TestMethod.MethodName.MethodNumber"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.TestMethod.MethodName.MethodNumber"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.Sort"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.Sort"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.Parameter.ParameterName"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.Parameter.ParameterName"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["RecLCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RecLCLimit"].Visible = false;
                                }
                                if (gridListEditor.Grid.Columns["RecHCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RecHCLimit"].Visible = false;
                                }
                                if (gridListEditor.Grid.Columns["RPDLCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RPDLCLimit"].Visible = false;
                                }
                                if (gridListEditor.Grid.Columns["RPDHCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RPDHCLimit"].Visible = false;
                                }
                                gridListEditor.IsFooterVisible = true;
                                ASPxSummaryItem customCount = new ASPxSummaryItem();
                                customCount.FieldName = "SysSampleCode";
                                customCount.ShowInColumn = "SysSampleCode";
                                customCount.SummaryType = SummaryItemType.Count;
                                gridListEditor.Grid.TotalSummary.Clear();
                                gridListEditor.Grid.TotalSummary.Add(customCount);
                            }
                        }
                        else
                        {
                            lstsample.CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.QCResultEntryQueryFilter + " AND [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample' And [GCRecord] IS NULL"); //And [Status] <>'" + Samplestatus.PendingEntry + "'
                            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (gridListEditor != null && gridListEditor.Grid != null)
                            {
                                ASPxGridView gridView = gridListEditor.Grid;
                                //foreach (GridViewColumn column in gridView.Columns.Cast<GridViewColumn>().Where(i => i.VisibleIndex >= 0).ToList())
                                //{
                                //    if (column.Name == "DateReceived" || column.Caption == "SpikeAmount" || column.Caption == "SpikeAmountUnit" || column.Caption == "REC" || column.Caption == "RPD")
                                //    {
                                //        column.Visible = false;
                                //    }
                                //}
                                //foreach (WebColumnBase column in gridView.Columns)
                                //{
                                //    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                                //    if (columnInfo != null)
                                //    {
                                //        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                                //        if (modelColumn.Id == "DateReceived" || modelColumn.Id == "SpikeAmount" || modelColumn.Id == "SpikeAmountUnit" || modelColumn.Id == "Rec" || modelColumn.Id == "RPD"
                                //           || modelColumn.Id == "QcBatchID" || modelColumn.Id == "QCType")
                                //        {
                                //            column.Visible = true;
                                //        }
                                //        else if (modelColumn.Id == "ReceivedDates")
                                //        {
                                //            column.Visible = false;
                                //        }
                                //    }
                                //}
                                gridListEditor.Grid.ClearSort();
                                if (gridListEditor.Grid.Columns["QcBatchID"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["QcBatchID"], ColumnSortOrder.Ascending);
                                    gridListEditor.Grid.Columns["QcBatchID"].Width = 120;
                                }
                                if (gridListEditor.Grid.Columns["QCType"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["QCType"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["SysSampleCode"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["SysSampleCode"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.TestMethod.TestName"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.TestMethod.TestName"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.TestMethod.MethodName.MethodNumber"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.TestMethod.MethodName.MethodNumber"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.Sort"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.Sort"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["Testparameter.Parameter.ParameterName"] != null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Testparameter.Parameter.ParameterName"], ColumnSortOrder.Ascending);
                                }
                                if (gridListEditor.Grid.Columns["RecLCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RecLCLimit"].Visible = true;
                                }
                                if (gridListEditor.Grid.Columns["RecHCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RecHCLimit"].Visible = true;
                                }
                                if (gridListEditor.Grid.Columns["RPDLCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RPDLCLimit"].Visible = true;
                                }
                                if (gridListEditor.Grid.Columns["RPDHCLimit"] != null)
                                {
                                    gridListEditor.Grid.Columns["RPDHCLimit"].Visible = true;
                                }
                                gridListEditor.IsFooterVisible = true;
                                ASPxSummaryItem customCount = new ASPxSummaryItem();
                                customCount.FieldName = "QcBatchID";
                                customCount.ShowInColumn = "QcBatchID";
                                customCount.SummaryType = SummaryItemType.Count;
                                gridListEditor.Grid.TotalSummary.Clear();
                                gridListEditor.Grid.TotalSummary.Add(customCount);
                                //gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
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

        ////private void ViAB_ControlCreated(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        if (sender != null)
        ////        {
        ////            DashboardViewItem vi = (DashboardViewItem)sender;
        ////            if (vi != null && vi.Control != null)
        ////            {
        ////                if (objQPInfo.SelectMode == QueryMode.ABID)
        ////                {
        ////                    ((Control)vi.Control).Visible = true;
        ////                }
        ////                else if (objQPInfo.SelectMode == QueryMode.Job /*|| objQPInfo.SelectMode == QueryMode.QC*/)
        ////                {
        ////                    ((Control)vi.Control).Visible = false;
        ////                }
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        ////        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        ////    }
        ////}

        private void Application_DashboardViewCreating(object sender, DashboardViewCreatingEventArgs e)
        {
            try
            {
                if (Frame != null && Frame is WebWindow)
                {
                    SRInfo.ResultEntryFrame = Frame;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Application_DashboardViewCreated(object sender, DashboardViewCreatedEventArgs e)
        {
            try
            {
                if (Frame != null && Frame is WebWindow)
                {
                    SRInfo.ResultEntryFrame = Frame;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        ////private void ViJob_ControlCreated(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        if (sender != null)
        ////        {
        ////            DashboardViewItem vi = (DashboardViewItem)sender;
        ////            if (vi != null && vi.Control != null)
        ////            {
        ////                if (objQPInfo.SelectMode == QueryMode.Job)
        ////                {
        ////                    ((Control)vi.Control).Visible = true;
        ////                }
        ////                else if (/*objQPInfo.SelectMode == QueryMode.QC ||*/ objQPInfo.SelectMode == QueryMode.ABID)
        ////                {
        ////                    ((Control)vi.Control).Visible = false;
        ////                }
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        ////        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        ////    }
        ////}
        private void ResultEntryDateFilterAction_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "ResultEntryQueryPanel_DetailView_Copy" || View.Id == "ResultViewQueryPanel_DetailView"))
                {
                    if (resultEntryDateFilterAction != null && resultEntryDateFilterAction.SelectedItem != null)
                    {
                        if (resultEntryDateFilterAction.SelectedItem.Id == "1M")
                        {
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                        }
                        else if (resultEntryDateFilterAction.SelectedItem.Id == "3M")
                        {
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                        }
                        else if (resultEntryDateFilterAction.SelectedItem.Id == "6M")
                        {
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                        }
                        else if (resultEntryDateFilterAction.SelectedItem.Id == "1Y")
                        {
                            objQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                        }
                        else
                        {
                            objQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                        }
                    }
                    DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                    DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                    if (viJob != null && viJob.InnerView != null && viQC != null && viQC.InnerView != null)
                    {
                        if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                        {
                            if (View.Id == "ResultEntryQueryPanel_DetailView_Copy")
                            {
                                bool isAdministrator = false;
                                List<string> lstTests = new List<string>();
                                string strTestMethodsPermissionCriteria = string.Empty;
                                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                                if (currentUser != null && currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    isAdministrator = true;
                                }
                                List<Guid> lstsmploid = new List<Guid>();
                                List<Guid> lstQCBatchoid = new List<Guid>();
                                ////((ListView)viJob.InnerView).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("RecievedDate>=? and RecievedDate<?", objQPInfo.rgFilterByMonthDate, DateTime.Now); 
                                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                                SelectedData JOBIDsproc = currentSession.ExecuteSproc("GetResultEntryData", new OperandValue(currentUser.DisplayName), new OperandValue(isAdministrator), new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                foreach (SelectStatementResultRow row in JOBIDsproc.ResultSet[0].Rows)
                                {
                                    if (row.Values[1] != null && !lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                    {
                                        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                    }
                                }
                                if (lstsmploid.Count > 0)
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                                }
                                else
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                                }
                                SelectedData QCIDsproc = currentSession.ExecuteSproc("GetQCBatchResultEntryData", new OperandValue(currentUser.DisplayName), new OperandValue(isAdministrator), new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                foreach (SelectStatementResultRow row in QCIDsproc.ResultSet[0].Rows)
                                {
                                    if (row.Values[1] != null && !lstQCBatchoid.Contains(new Guid(row.Values[1].ToString())))
                                    {
                                        lstQCBatchoid.Add(new Guid(row.Values[1].ToString()));
                                    }
                                }
                                if (lstQCBatchoid.Count > 0)
                                {
                                    ((ListView)viQC.InnerView).CollectionSource.Criteria["filter"] = new InOperator("UQABID.Oid", lstQCBatchoid);
                                }
                                else
                                {
                                    ((ListView)viQC.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                                }
                            }
                            else if (View.Id == "ResultViewQueryPanel_DetailView")
                            {
                                List<Guid> lstsmploid = new List<Guid>();
                                List<Guid> lstQCBatchoid = new List<Guid>();
                                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                                SelectedData JOBIDsproc = currentSession.ExecuteSproc("GetResultViewData", new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                //foreach (SelectStatementResultRow row in JOBIDsproc.ResultSet[0].Rows)
                                //{
                                //    if (row.Values[1] != null && !lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                //    {
                                //        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                //    }
                                //}
                                lstsmploid = JOBIDsproc.ResultSet[0].Rows.Where(i => i.Values[1] != null).Select(i => new Guid(i.Values[1].ToString())).Distinct().ToList();
                                if (lstsmploid.Count > 0)
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                                }
                                else
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                                }
                                SelectedData QCIDsproc = currentSession.ExecuteSproc("GetQCBatchResultViewData", new OperandValue("DateFilter"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                //foreach (SelectStatementResultRow row in QCIDsproc.ResultSet[0].Rows)
                                //{
                                //    if (row.Values[1] != null && !lstQCBatchoid.Contains(new Guid(row.Values[1].ToString())))
                                //    {
                                //        lstQCBatchoid.Add(new Guid(row.Values[1].ToString()));
                                //    }
                                //}
                                lstQCBatchoid = QCIDsproc.ResultSet[0].Rows.Where(i => i.Values[1] != null).Select(i => new Guid(i.Values[1].ToString())).Distinct().ToList();
                                if (lstQCBatchoid.Count > 0)
                                {
                                    ((ListView)viQC.InnerView).CollectionSource.Criteria["filter"] = new InOperator("UQABID.Oid", lstQCBatchoid);
                                }
                                else
                                {
                                    ((ListView)viQC.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                                }
                            }
                            ////else
                            ////{
                            ////    ////((ListView)viJob.InnerView).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("Samplelogin.JobID.RecievedDate>=? and Samplelogin.JobID.RecievedDate<?", objQPInfo.rgFilterByMonthDate, DateTime.Now);

                            ////}
                        }
                        else
                        {
                            bool isAdministrator = false;
                            if (View.Id == "ResultEntryQueryPanel_DetailView_Copy")
                            {
                                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    isAdministrator = true;
                                }
                                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                                SelectedData sproc = currentSession.ExecuteSproc("GetResultEntryData", new OperandValue(currentUser.DisplayName), new OperandValue(isAdministrator), new OperandValue("ALL"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                List<Guid> lstsmploid = new List<Guid>();
                                List<Guid> lstQCBatchoid = new List<Guid>();
                                foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                                {
                                    if (row.Values[1] != null && !lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                    {
                                        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                    }
                                }
                                if (lstsmploid.Count > 0)
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                                }
                                else
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                                }
                                SelectedData QCIDsproc = currentSession.ExecuteSproc("GetQCBatchResultEntryData", new OperandValue(currentUser.UserName), new OperandValue(isAdministrator), new OperandValue("ALL"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                foreach (SelectStatementResultRow row in QCIDsproc.ResultSet[0].Rows)
                                {
                                    if (row.Values[1] != null && !lstQCBatchoid.Contains(new Guid(row.Values[1].ToString())))
                                    {
                                        lstQCBatchoid.Add(new Guid(row.Values[1].ToString()));
                                    }
                                }
                                if (lstQCBatchoid.Count > 0)
                                {
                                    ((ListView)viQC.InnerView).CollectionSource.Criteria["filter"] = new InOperator("UQABID.Oid", lstQCBatchoid);
                                }
                                else
                                {
                                    ((ListView)viQC.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                                }
                            }
                            else if (View.Id == "ResultViewQueryPanel_DetailView")
                            {
                                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                                SelectedData sproc = currentSession.ExecuteSproc("GetResultViewData", new OperandValue("ALL"), new OperandValue(objQPInfo.rgFilterByMonthDate));
                                List<Guid> lstsmploid = new List<Guid>();
                                List<Guid> lstQCBatchoid = new List<Guid>();
                                foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                                {
                                    if (row.Values[1] != null && !lstsmploid.Contains(new Guid(row.Values[1].ToString())))
                                    {
                                        lstsmploid.Add(new Guid(row.Values[1].ToString()));
                                    }
                                    if (row.Values[2] != null && !lstQCBatchoid.Contains(new Guid(row.Values[2].ToString())))
                                    {
                                        lstQCBatchoid.Add(new Guid(row.Values[2].ToString()));
                                    }
                                }
                                if (lstsmploid.Count > 0)
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                                }
                                else
                                {
                                    ((ListView)viJob.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                                }
                                if (lstQCBatchoid.Count > 0)
                                {
                                    ((ListView)viQC.InnerView).CollectionSource.Criteria["filter"] = new InOperator("UQABID.Oid", lstQCBatchoid);
                                }
                                else
                                {
                                    ((ListView)viQC.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                                }
                            }
                            //if (((DevExpress.ExpressApp.ListView)viJob.InnerView).CollectionSource.Criteria.ContainsKey("dateFilter"))
                            //{
                            //    //((DevExpress.ExpressApp.ListView)viJob.InnerView).CollectionSource.Criteria["dateFilter"].IsNull();
                            //    ((DevExpress.ExpressApp.ListView)viJob.InnerView).CollectionSource.Criteria.Remove("dateFilter");
                            //}
                        }
                        //((DevExpress.ExpressApp.ListView)viJob.InnerView).ObjectSpace.Refresh();
                        ((DevExpress.ExpressApp.ListView)viJob.InnerView).CollectionSource.Reload();
                        ((DevExpress.ExpressApp.ListView)viQC.InnerView).CollectionSource.Reload();
                    }
                    ////if (viQC != null && viQC.InnerView != null)
                    ////{
                    ////    if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                    ////    {
                    ////        //((DevExpress.ExpressApp.ListView)viQC.InnerView).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')");
                    ////        ((DevExpress.ExpressApp.ListView)viQC.InnerView).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] >= ? and [Samplelogin.JobID.RecievedDate] < ?", objQPInfo.rgFilterByMonthDate, DateTime.Now);
                    ////    }
                    ////    else
                    ////    {
                    ////        if (((DevExpress.ExpressApp.ListView)viQC.InnerView).CollectionSource.Criteria.ContainsKey("dateFilter"))
                    ////        {
                    ////            //((DevExpress.ExpressApp.ListView)viQC.InnerView).CollectionSource.Criteria["dateFilter"].IsNull();
                    ////            ((DevExpress.ExpressApp.ListView)viQC.InnerView).CollectionSource.Criteria.Remove("dateFilter");
                    ////        }
                    ////    }
                    ////    //((DevExpress.ExpressApp.ListView)viJob.InnerView).ObjectSpace.Refresh();
                    ////    ((DevExpress.ExpressApp.ListView)viJob.InnerView).CollectionSource.Reload();
                    ////}
                    ////if (viQC != null && viQC.InnerView != null)
                    ////{
                    ////    ((DevExpress.ExpressApp.ListView)viQC.InnerView).Refresh();
                    ////}
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RgEditor_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    if (sender is RadioButtonListEnumPropertyEditor)
                    {
                        RadioButtonListEnumPropertyEditor vi = (RadioButtonListEnumPropertyEditor)sender;
                        if (vi != null && vi.Control != null)
                        {
                            ((Control)vi.Control).Visible = true;
                        }
                    }
                    else if (sender is EnumRadioButtonListPropertyEditor)
                    {
                        EnumRadioButtonListPropertyEditor vi = (EnumRadioButtonListPropertyEditor)sender;
                        if (vi != null && vi.Control != null)
                        {
                            ((Control)vi.Control).Visible = true;
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

        ////private void ViQC_ControlCreated(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        if (sender != null)
        ////        {
        ////            DashboardViewItem vi = (DashboardViewItem)sender;
        ////            if (vi != null && vi.Control != null)
        ////            {
        ////                if (objQPInfo.IsQueryPanelOpened == true || objQPInfo.SelectMode == QueryMode.Job || objQPInfo.SelectMode == QueryMode.ABID)
        ////                {
        ////                    objQPInfo.IsQueryPanelOpened = false;
        ////                    ((Control)vi.Control).Visible = false;
        ////                }
        ////                //else if (objQPInfo.SelectMode == QueryMode.QC)
        ////                //{
        ////                //    ((Control)vi.Control).Visible = true;
        ////                //}
        ////            }
        ////            //View.Refresh();
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        ////        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        ////    }
        ////}

        private void View_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Samplecheckin_LookupListView_Copy_ResultEntryQueryPanel")
                {
                    if (objQPInfo.lstJobID == null)
                    {
                        objQPInfo.lstJobID = new List<string>();
                    }
                    else
                    {
                        objQPInfo.lstJobID.Clear();
                    }

                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (Samplecheckin obj in View.SelectedObjects)
                        {
                            if (objQPInfo.lstJobID != null && !objQPInfo.lstJobID.Contains(obj.JobID))
                            {
                                objQPInfo.lstJobID.Add(obj.JobID);
                            }
                        }
                    }
                }
                else if (View != null && (View.Id == "QCBatch_ListView_ResultEntry" || View.Id == "QCBatch_ListView_ResultView"))
                {
                    if (objQPInfo.lstQCBatchID == null)
                    {
                        objQPInfo.lstQCBatchID = new List<string>();
                    }
                    else
                    {
                        objQPInfo.lstQCBatchID.Clear();
                    }

                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (SpreadSheetEntry_AnalyticalBatch obj in View.SelectedObjects)
                        {
                            if (objQPInfo.lstQCBatchID != null && !objQPInfo.lstQCBatchID.Contains(obj.AnalyticalBatchID))
                            {
                                objQPInfo.lstQCBatchID.Add(obj.AnalyticalBatchID);
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

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" || View.Id == "SampleParameter_ListView_Copy_ResultView_Main" || View.Id == "QCBatch_ListView_ResultEntry" || View.Id == "QCBatch_ListView_ResultView"
                    || View.Id == "Samplecheckin_ListView_ResultEntry" || View.Id == "Samplecheckin_ListView_ResultView")
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    if (gridView != null)
                    {
                        GridViewCommandColumn selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
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

                if (View is ListView)
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                else if (View is DetailView)
                {
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" || View.Id == "QCBatch_ListView_ResultEntry"))
                {
                    objQPInfo.REViewMode = true;
                    bool DisplayRows = false;
                    bool IsAdmin = false;
                    List<Guid> lstTestMethodOids = new List<Guid>();
                    IList<TestMethod> lstAllTestMethods = ObjectSpace.GetObjects<TestMethod>();
                    Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        DisplayRows = true;
                        IsAdmin = true;
                    }
                    else
                    {
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            DisplayRows = true;
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTestMethodOids.Contains(testMethod.Oid))
                                    {
                                        lstTestMethodOids.Add(testMethod.Oid);
                                    }
                                }
                            }
                        }
                    }
                    //ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    //lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;

                    if (DisplayRows)
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main")
                            {
                                if (IsAdmin)
                                {
                                    ///lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingEntry' And ([SubOut] Is Null Or [SubOut] = False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)");
                                    lstview.Criteria = new GroupOperator(GroupOperatorType.And,
                                        CriteriaOperator.Parse("[Status] = 'PendingEntry' And [SignOff] = True  And [IsTransferred] = true And [GCRecord] IS NULL")//AND [Samplelogin] IS NOT NULL AND [ABID] IS NULL
                                        , new NotOperator(new InOperator("Oid", lstAllTestMethods.Where(i => i.IsSDMSTest).Select(i => i.Oid))));
                                }
                                else
                                {
                                    //lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingEntry' AND ([SubOut] is null Or [SubOut]=False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)");

                                    lstview.Criteria = new GroupOperator(GroupOperatorType.And,
                                        CriteriaOperator.Parse("[Status]='PendingEntry' AND [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND [ABID] IS NULL And ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)"),
                                        CriteriaOperator.Parse("[Status]='PendingEntry' And [SignOff] = True AND [IsTransferred] = true And [GCRecord] IS NULL"),
                                    new InOperator("Testparameter.TestMethod.Oid", lstTestMethodOids),
                                    new NotOperator(new InOperator("Oid", lstAllTestMethods.Where(i => i.IsSDMSTest).Select(i => i.Oid))));
                                }
                                lstview.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                List<object> groups = new List<object>();
                                foreach (ViewRecord rec in lstview)
                                    groups.Add(rec["Toid"]);
                                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                            }
                            else
                            {
                                if (IsAdmin)
                                {
                                    //lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingEntry' And ([SubOut] Is Null Or [SubOut] = False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND [ABID] IS NULL And ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)");
                                    lstview.Criteria = new GroupOperator(GroupOperatorType.And,
                                        CriteriaOperator.Parse("[Status] = 'PendingEntry' And [SignOff] = True  And [IsTransferred] = true And [GCRecord] IS NULL And [UQABID] IS NOT NULL")//AND [Samplelogin] IS NOT NULL AND [ABID] IS NULL
                                        , new NotOperator(new InOperator("Oid", lstAllTestMethods.Where(i => i.IsSDMSTest).Select(i => i.Oid))));
                                }
                                else
                                {
                                    lstview.Criteria = new GroupOperator(GroupOperatorType.And,
                                        //CriteriaOperator.Parse("[Status]='PendingEntry' AND ([SubOut] is null Or [SubOut]=False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND [ABID] IS NULL And ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)"),
                                        CriteriaOperator.Parse("[Status]='PendingEntry' And [SignOff] = True AND [IsTransferred] = true And [GCRecord] IS NULL And [UQABID] IS NOT NULL"),
                                    new InOperator("Testparameter.TestMethod.Oid", lstTestMethodOids),
                                    new NotOperator(new InOperator("Oid", lstAllTestMethods.Where(i => i.IsSDMSTest).Select(i => i.Oid))));
                                }
                                lstview.Properties.Add(new ViewProperty("TAnalyticalBatchID", SortDirection.Ascending, "UQABID.AnalyticalBatchID", true, true));
                                lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                List<object> groups = new List<object>();
                                List<string> ABID = new List<string>();
                                foreach (ViewRecord rec in lstview)
                                    if (!ABID.Contains(rec["TAnalyticalBatchID"].ToString()) && !string.IsNullOrEmpty(rec["TAnalyticalBatchID"].ToString()))
                                    {
                                        groups.Add(rec["Toid"]);
                                        ABID.Add(rec["TAnalyticalBatchID"].ToString());
                                    }

                                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                            }
                        }
                        //if(View.Id== "Samplecheckin_ListView_ResultEntry")
                        //{
                        //    if (IsAdmin)
                        //    {
                        //        ///lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingEntry' And ([SubOut] Is Null Or [SubOut] = False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)");
                        //        ((ListView)View).CollectionSource.Criteria = new GroupOperator(GroupOperatorType.And,
                        //            CriteriaOperator.Parse("[Status] = 'PendingEntry' And [SignOff] = True  And [Samplelogin.IsNotTransferred] = false And ([SubOut] Is Null Or [SubOut] = False Or ([SubOut] = True And [IsExportedSuboutResult] = True)) And [GCRecord] IS NULL")//AND [Samplelogin] IS NOT NULL AND [ABID] IS NULL
                        //            , new NotOperator(new InOperator("Oid", lstAllTestMethods.Where(i => i.IsSDMSTest).Select(i => i.Oid))));
                        //    }
                        //    else
                        //    {
                        //        //lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingEntry' AND ([SubOut] is null Or [SubOut]=False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)");

                        //        lstview.Criteria = new GroupOperator(GroupOperatorType.And,
                        //            CriteriaOperator.Parse("[Status]='PendingEntry' AND ([SubOut] is null Or [SubOut]=False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND [ABID] IS NULL And ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)"),
                        //            CriteriaOperator.Parse("[Status]='PendingEntry' And [SignOff] = True AND [Samplelogin.IsNotTransferred] = false And ([SubOut] is null Or [SubOut]=False Or ([SubOut] = True And [IsExportedSuboutResult] = True)) And [GCRecord] IS NULL"),
                        //        new InOperator("Testparameter.TestMethod.Oid", lstTestMethodOids),
                        //        new NotOperator(new InOperator("Oid", lstAllTestMethods.Where(i => i.IsSDMSTest).Select(i => i.Oid))));
                        //    }
                        //    lstview.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                        //    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        //    List<object> groups = new List<object>();
                        //    foreach (ViewRecord rec in lstview)
                        //        groups.Add(rec["Toid"]);
                        //    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                        //}
                        if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] >= ? and [Samplelogin.JobID.RecievedDate] < ?", objQPInfo.rgFilterByMonthDate, DateTime.Now);
                        }
                        else
                        {
                            if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey("dateFilter"))
                            {
                                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                            }
                        }
                    }
                    else
                    {
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] is null");
                    }

                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.Load += Grid_Load;
                        //editor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    }
                }
                else if (View.Id == "Samplecheckin_ListView_ResultEntry" || View.Id == "Samplecheckin_ListView_ResultView")
                {

                    //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[GCRecord] IS NULL");
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.Load += Grid_Load;
                        //editor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    }
                    //objQPInfo.REViewMode = true;
                    //bool DisplayRows = false;
                    //bool IsAdmin = false;
                    //List<Guid> lstTestMethodOids = new List<Guid>();
                    //IList<TestMethod> lstAllTestMethods = ObjectSpace.GetObjects<TestMethod>();
                    //Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                    //if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    //{
                    //    DisplayRows = true;
                    //    IsAdmin = true;
                    //}
                    //else
                    //{
                    //    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));
                    //    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                    //    {
                    //        DisplayRows = true;
                    //        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                    //        {
                    //            foreach (TestMethod testMethod in departmentChain.TestMethods)
                    //            {
                    //                if (!lstTestMethodOids.Contains(testMethod.Oid))
                    //                {
                    //                    lstTestMethodOids.Add(testMethod.Oid);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //if (DisplayRows)
                    //{
                    //    XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>();
                    //    IList<SampleParameter> lstSample = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse(""));
                    //    if (IsAdmin)
                    //    {
                    //        List<SampleParameter> lstSamples = lstSample.Where(i => i.Samplelogin != null && i.Testparameter != null && i.Testparameter.TestMethod != null && (i.Testparameter.TestMethod.IsSDMSTest == false || i.Testparameter.TestMethod.IsSDMSTest == null) && (i.SubOut == false || i.SubOut == null || (i.SubOut == true && i.IsExportedSuboutResult == true)) && i.Status == Samplestatus.PendingEntry
                    //        && i.SignOff == true && i.Samplelogin.IsNotTransferred == false).ToList();
                    //        /////lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingEntry' And ([SubOut] Is Null Or [SubOut] = False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)");
                    //        //lstSamples.Criteria = new GroupOperator(GroupOperatorType.And,
                    //        //    CriteriaOperator.Parse("[Status] = 'PendingEntry' And [SignOff] = True  And [Samplelogin.IsNotTransferred] = false And ([SubOut] Is Null Or [SubOut] = False Or ([SubOut] = True And [IsExportedSuboutResult] = True)) And [GCRecord] IS NULL")//AND [Samplelogin] IS NOT NULL AND [ABID] IS NULL
                    //        //    , new NotOperator(new InOperator("Oid", lstAllTestMethods.Where(i => i.IsSDMSTest).Select(i => i.Oid))));
                    //    }
                    //    else
                    //    {
                    //        //lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingEntry' AND ([SubOut] is null Or [SubOut]=False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)");

                    //        lstview.Criteria = new GroupOperator(GroupOperatorType.And,
                    //            CriteriaOperator.Parse("[Status]='PendingEntry' AND ([SubOut] is null Or [SubOut]=False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL AND [ABID] IS NULL And ([Testparameter.TestMethod.IsSDMSTest] = False Or [Testparameter.TestMethod.IsSDMSTest] Is Null)"),
                    //            CriteriaOperator.Parse("[Status]='PendingEntry' And [SignOff] = True AND [Samplelogin.IsNotTransferred] = false And ([SubOut] is null Or [SubOut]=False Or ([SubOut] = True And [IsExportedSuboutResult] = True)) And [GCRecord] IS NULL"),
                    //        new InOperator("Testparameter.TestMethod.Oid", lstTestMethodOids),
                    //        new NotOperator(new InOperator("Oid", lstAllTestMethods.Where(i => i.IsSDMSTest).Select(i => i.Oid))));
                    //    }
                    //    lstview.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                    //    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //    List<object> groups = new List<object>();
                    //    foreach (ViewRecord rec in lstview)
                    //        groups.Add(rec["Toid"]);
                    //    ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                    //}
                }
                if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultView_Main" || View.Id == "QCBatch_ListView_ResultView"))
                {
                    if (View.Id == "SampleParameter_ListView_Copy_ResultView_Main")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            //lstview.Criteria = CriteriaOperator.Parse("([SubOut] Is Null Or [SubOut] = False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL");
                            lstview.Criteria = CriteriaOperator.Parse("[SignOff] = True And [IsTransferred] = true And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL and ([ABID] is Null or ([ABID] is not null))");//and [Status] <>'" + Samplestatus.PendingEntry + "' And [Status] <>'" + Samplestatus.PendingEntry + "' and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'
                            lstview.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                groups.Add(rec["Toid"]);
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                        }
                    }
                    else
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            //lstview.Criteria = CriteriaOperator.Parse("([SubOut] Is Null Or [SubOut] = False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL");
                            lstview.Criteria = CriteriaOperator.Parse("([SignOff] = True And [IsTransferred] = true And [GCRecord] IS NULL and  ([UQABID] is Null or ([UQABID] is not null)))"); //and [Status] <>'" + Samplestatus.PendingEntry + "' And [Status] <>'" + Samplestatus.PendingEntry + "' [Samplelogin] IS NOT NULL and  and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'
                            lstview.Properties.Add(new ViewProperty("TAnalyticalBatchID", SortDirection.Ascending, "UQABID.AnalyticalBatchID", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> groups = new List<object>();
                            List<string> ABID = new List<string>();
                            foreach (ViewRecord rec in lstview)
                                if (rec["TAnalyticalBatchID"] != null && !ABID.Contains(rec["TAnalyticalBatchID"].ToString()) && !string.IsNullOrEmpty(rec["TAnalyticalBatchID"].ToString()))
                                {
                                    groups.Add(rec["Toid"]);
                                    ABID.Add(rec["TAnalyticalBatchID"].ToString());
                                }
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                        }
                    }

                    if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                    {
                        //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')");
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] >= ? and [Samplelogin.JobID.RecievedDate] < ?", objQPInfo.rgFilterByMonthDate, DateTime.Now);
                    }
                    else
                    {
                        if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey("dateFilter"))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                        }
                    }

                    //ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    //lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;

                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.Load += Grid_Load;
                        //editor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    }
                }
                if (View != null && View.Id == "SampleParameter_ListView_ResultView_ABID")
                {
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    {
                        //lstview.Criteria = CriteriaOperator.Parse("([SubOut] Is Null Or [SubOut] = False) And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL");
                        lstview.Criteria = CriteriaOperator.Parse("( [SignOff] = True And [IsTransferred] = true And [GCRecord] IS NULL And [Samplelogin] IS NOT NULL and [Status] <>'" + Samplestatus.PendingEntry + "' and [ABID] is not null");
                        lstview.Properties.Add(new ViewProperty("TABID", SortDirection.Ascending, "UQABID.AnalyticalBatchID", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", groups);
                    }

                    if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                    {
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')");
                    }
                    else
                    {
                        if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey("dateFilter"))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                        }
                    }

                    //ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    //lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;

                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null)
                    {
                        editor.Grid.Load += Grid_Load;
                        editor.Grid.SettingsBehavior.AllowSelectByRowClick = true;
                    }
                }
                else if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultValidation"))
                {
                    objQPInfo.CurrentViewID = "SampleParameter_ListView_Copy_ResultValidation";
                }
                else if (View != null && (View.Id == "SampleParameter_ListView_Copy_CustomReporting"))
                {
                    objQPInfo.CurrentViewID = "SampleParameter_ListView_Copy_CustomReporting";
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    //gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load += Grid_Load;
                    }
                }
                else if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultApproval"))
                {
                    objQPInfo.CurrentViewID = "SampleParameter_ListView_Copy_ResultApproval";
                }
                else if (View != null && (View.Id == "ResultEntryQueryPanel_DetailView" || View.Id == "ResultEntryQueryPanel_DetailView_Copy" || View.Id == "ResultViewQueryPanel_DetailView"))
                {
                    if (View.CurrentObject != null && ResultEntrySelection.SelectedItem != null) //&& objQPInfo.FilterOpened == true)
                    {
                        if (!string.IsNullOrEmpty(objQPInfo.ResultEntryCurrentselectionid))
                        {
                            DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                            DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                            DashboardViewItem viAB = (DashboardViewItem)((DetailView)View).FindItem("viewItemABID");
                            if (ResultEntrySelection.SelectedItem.Id == "ResultEntryJOBID")
                            {
                                objQPInfo.ResultEntryCurrentselectionid = "ResultEntryJOBID";
                                if (viJob != null)
                                {
                                    ((Control)viJob.Control).Visible = true;
                                }
                                if (viQC != null)
                                {
                                    ((Control)viQC.Control).Visible = false;
                                }
                                if (viAB != null)
                                {
                                    ((Control)viAB.Control).Visible = false;
                                }
                            }
                            else
                            {
                                objQPInfo.ResultEntryCurrentselectionid = "ResultEntryQCBatchID";
                                if (viJob != null)
                                {
                                    ((Control)viJob.Control).Visible = false;
                                }
                                if (viQC != null)
                                {
                                    ((Control)viQC.Control).Visible = true;
                                }
                                if (viAB != null)
                                {
                                    ((Control)viAB.Control).Visible = true;
                                }
                            }
                        }
                        ////ResultEntryQueryPanel reQueryPanel = (ResultEntryQueryPanel)View.CurrentObject;
                        ////if (reQueryPanel != null)
                        ////{
                        ////    DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                        ////    DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                        ////    if (reQueryPanel.SelectionMode == QueryMode.Job)
                        ////    {
                        ////        if (viJob != null)
                        ////        {
                        ////            ((Control)viJob.Control).Visible = true;
                        ////            viJob.Refresh();
                        ////        }
                        ////        if (viQC != null)
                        ////        {
                        ////            ((Control)viQC.Control).Visible = false;
                        ////            viQC.Refresh();
                        ////        }
                        ////    }
                        ////    else
                        ////    {
                        ////        if (viJob != null)
                        ////        {
                        ////            ((Control)viJob.Control).Visible = false;
                        ////            viJob.Refresh();
                        ////        }
                        ////        if (viQC != null)
                        ////        {
                        ////            ((Control)viQC.Control).Visible = true;
                        ////            viQC.Refresh();
                        ////        }
                        ////    }
                        ////}
                    }
                }

                else if (View != null && (View.Id == "SampleParameter_ListView_Copy_QCResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultView"))
                {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null && ((ListView)View).CollectionSource != null && ((ListView)View).CollectionSource.GetCount() > 0)
                    {
                        editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        editor.Grid.Settings.VerticalScrollableHeight = 300;
                        editor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                        editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        if (View.Id == "SampleParameter_ListView_Copy_QCResultEntry")
                        {
                            ((DevExpress.ExpressApp.Web.SystemModule.IModelListViewWeb)Application.FindModelView("SampleParameter_ListView_Copy_QCResultEntry")).PageSize = ((ListView)View).CollectionSource.GetCount();
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

        private void LstCurrObj_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" || View.Id == "SampleParameter_ListView_Copy_ResultView_Main"))
                {
                    objQPInfo.lstJobID = new List<string>();
                    objQPInfo.lstQCJobID = new List<string>();
                    objQPInfo.objJobID = null;
                    //objQPInfo.SelectMode = QueryMode.Job;
                    foreach (SampleParameter objsample in View.SelectedObjects)
                    {
                        objQPInfo.lstJobID.Add(objsample.Samplelogin.JobID.JobID);
                        objQPInfo.objJobID = objsample.Samplelogin.JobID.JobID;
                    }
                    QueryPanel = true;

                    //objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ")";
                    ////objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True and [Samplelogin.IsNotTransferred] = false And ([ABID] is Null or ([ABID] is not null and [Status] ='" + Samplestatus.PendingEntry + "'))";//and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'
                    //objQPInfo.QCResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ")";
                    //objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.SampleID.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True";
                    objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.JOBID] Like '%" + objQPInfo.objJobID + "%'";
                    if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" || View.Id == "Samplecheckin_ListView_ResultEntry")
                    {
                        e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Enter", true);
                        objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True and [IsTransferred] = true And ([ABID] is Null or ([ABID] is not null and [Status] ='" + Samplestatus.PendingEntry + "'))";//and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'

                    }
                    else
                    {
                        e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "Result_View", false);
                        objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True and [IsTransferred] = true And ([ABID] is Null or ([ABID] is not null and [Status] <>'" + Samplestatus.PendingEntry + "'))";//and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'

                    }
                    e.Handled = true;
                }
                else if (View != null && (View.Id == "QCBatch_ListView_ResultEntry" || View.Id == "QCBatch_ListView_ResultView"))
                {
                    objQPInfo.lstQCBatchID = new List<string>();
                    objQPInfo.objJobID = null;
                    //objQPInfo.SelectMode = QueryMode.ABID;
                    foreach (SampleParameter objsample in View.SelectedObjects)
                    {
                        objQPInfo.lstQCBatchID.Add(objsample.QCBatchID.qcseqdetail.AnalyticalBatchID);
                        objQPInfo.objJobID = objsample.Samplelogin.JobID.JobID;
                    }
                    QueryPanel = true;

                    objQPInfo.ResultEntryQueryFilter = "[QCBatchID.qcseqdetail.AnalyticalBatchID] IN (" + "'" + string.Join("','", objQPInfo.lstQCBatchID) + "'" + ") And [SignOff] = True";
                    objQPInfo.QCResultEntryQueryFilter = objQPInfo.ResultEntryQueryFilter;

                    if (View.Id == "QCBatch_ListView_ResultEntry")
                    {
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                        CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                        bool isAdministrator = false;
                        List<Guid> lstTests = new List<Guid>();
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            isAdministrator = true;
                        }
                        else
                        {
                            IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));

                            if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                            {
                                foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                {
                                    foreach (TestMethod testMethod in departmentChain.TestMethods)
                                    {
                                        if (!lstTests.Contains(testMethod.Oid))
                                        {
                                            lstTests.Add(testMethod.Oid);
                                        }
                                    }
                                }
                            }
                            //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                        }
                        if (lstTests.Count > 0)
                        {
                            objQPInfo.ResultEntryADCFilter = "[Testparameter.TestMethod.Oid] IN(" + "'" + string.Join("', '", lstTests) + "'" + ")";
                        }
                        else
                        {
                            objQPInfo.ResultEntryADCFilter = string.Empty;
                        }
                        cs.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]=0 AND [GCRecord] IS NULL");
                        ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultEntry_SingleChoice", cs, true);
                        e.InnerArgs.ShowViewParameters.CreatedView = listview;
                        //e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Enter", true);
                    }
                    else
                    {
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                        cs.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [GCRecord] IS NULL");
                        ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultView_SingleChoice", cs, true);
                        e.InnerArgs.ShowViewParameters.CreatedView = listview;
                        //e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "Result_View", false);
                    }
                    e.Handled = true;

                }
                else if (View.Id == "SampleParameter_ListView_ResultView_ABID")
                {
                    if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject is SampleParameter)
                    {
                        SampleParameter obj = (SampleParameter)e.InnerArgs.CurrentObject;
                        if (obj != null)
                        {
                            objQPInfo.lstABID = new List<string>();
                            //objQPInfo.SelectMode = QueryMode.ABID;
                            foreach (SampleParameter objsample in View.SelectedObjects)
                            {
                                objQPInfo.lstABID.Add(objsample.UQABID.AnalyticalBatchID);
                            }
                            QueryPanel = true;

                            objQPInfo.ResultEntryQueryFilter = "[UQABID.AnalyticalBatchID] IN (" + "'" + string.Join("','", objQPInfo.lstABID) + "'" + ") And [SignOff] = True And [UQABID.GCRecord] IS NULL";
                            objQPInfo.QCResultEntryQueryFilter = objQPInfo.ResultEntryQueryFilter;
                            e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "Result_View", false);
                            e.Handled = true;
                        }
                    }
                }
                else if (View.Id == "Samplecheckin_ListView_ResultEntry")
                {
                    objQPInfo.lstJobID = new List<string>();
                    objQPInfo.lstQCJobID = new List<string>();
                    objQPInfo.objJobID = null;
                    //objQPInfo.SelectMode = QueryMode.Job;
                    foreach (Modules.BusinessObjects.SampleManagement.Samplecheckin objsample in View.SelectedObjects)
                    {
                        objQPInfo.lstJobID.Add(objsample.JobID);
                        objQPInfo.objJobID = objsample.JobID;
                    }
                    QueryPanel = true;
                    objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.JOBID] Like '%" + objQPInfo.objJobID + "%' And [QCBatchID.SampleID.JobID.JobID] IN(" + "'" + string.Join("', '", objQPInfo.lstJobID) + "'" + ")";
                    objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN(" + "'" + string.Join("', '", objQPInfo.lstJobID) + "'" + ") And[SignOff] = True and [IsTransferred] = true And([ABID] is Null or ([ABID] is not null and [Status] = '" + Samplestatus.PendingEntry + "'))";
                    CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                    bool isAdministrator = false;
                    List<Guid> lstTests = new List<Guid>();
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        isAdministrator = true;
                    }
                    else
                    {
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));

                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTests.Contains(testMethod.Oid))
                                    {
                                        lstTests.Add(testMethod.Oid);
                                    }
                                }
                            }
                        }
                        //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                    }
                    if (lstTests.Count > 0)
                    {
                        objQPInfo.ResultEntryADCFilter = "[Testparameter.TestMethod.Oid] IN(" + "'" + string.Join("', '", lstTests) + "'" + ")";
                    }
                    else
                    {
                        objQPInfo.ResultEntryADCFilter = string.Empty;
                    }
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]=0 And [GCRecord] IS NULL");
                    ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultEntry_SingleChoice", cs, true);
                    e.InnerArgs.ShowViewParameters.CreatedView = listview;
                    ////objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.JOBID] Like '%" + objQPInfo.objJobID + "%'";
                    ////objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True and [Samplelogin.IsNotTransferred] = false And ([ABID] is Null or ([ABID] is not null and [Status] ='" + Samplestatus.PendingEntry + "'))";//and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'
                    ////e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Enter", true);
                    e.Handled = true;
                }
                else if (View.Id == "Samplecheckin_ListView_ResultView")
                {
                    objQPInfo.lstJobID = new List<string>();
                    objQPInfo.lstQCJobID = new List<string>();
                    objQPInfo.objJobID = null;
                    //objQPInfo.SelectMode = QueryMode.Job;
                    foreach (Modules.BusinessObjects.SampleManagement.Samplecheckin objsample in View.SelectedObjects)
                    {
                        objQPInfo.lstJobID.Add(objsample.JobID);
                        objQPInfo.objJobID = objsample.JobID;
                    }
                    QueryPanel = true;
                    ////objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.JOBID] Like '%" + objQPInfo.objJobID + "%'";
                    ////if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" || View.Id == "Samplecheckin_ListView_ResultEntry")
                    ////{
                    ////    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Enter", true);
                    ////    objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True and [Samplelogin.IsNotTransferred] = false And ([ABID] is Null or ([ABID] is not null and [Status] ='" + Samplestatus.PendingEntry + "'))";//and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'

                    ////}
                    ////else
                    ////{
                    ////    e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "Result_View", false);
                    ////    objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True and [Samplelogin.IsNotTransferred] = false And ([ABID] is Null or ([ABID] is not null and [Status] <>'" + Samplestatus.PendingEntry + "'))";//and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'
                    ////}
                    ////e.Handled = true;
                    objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True and [IsTransferred] = true And ([UQABID] is Null or ([UQABID] is not null ))";//and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'
                    objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.JOBID] Like '%" + objQPInfo.objJobID + "%' And [QCBatchID.SampleID.JobID.JobID] IN(" + "'" + string.Join("', '", objQPInfo.lstJobID) + "'" + ")";
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [GCRecord] IS NULL");
                    ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultView_SingleChoice", cs, true);
                    e.InnerArgs.ShowViewParameters.CreatedView = listview;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void gridView_Load(object sender, EventArgs e)
        {
            ASPxGridView gridView = sender as ASPxGridView;
            gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
            gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice")
            {
                SampleSelectionMode.SelectedItemChanged -= SampleSelectionMode_SelectedItemChanged;
                CNInfo.REQCBatchId = null;
                CNInfo.RESampleMatries = null;
            }
            if (View != null && (View.Id == "SampleParameter_ListView_Copy_CustomReporting" || View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                    || View.Id == "SampleParameter_ListView_Copy_QCResultView" || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "Result_DV"))
            {
                View.ControlsCreated -= QueryPanelViewController_ViewControlsCreated;
            }
            //if (View.Id == "Samplecheckin_ListView_ResultEntry")
            //{
            //    ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
            //    lstCurrObj.CustomProcessSelectedItem -= LstCurrObj_CustomProcessSelectedItem;
            //}
            if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" /*|| View.Id == "QCBatch_ListView_ResultEntry"*/))
            {
                ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                lstCurrObj.CustomProcessSelectedItem -= LstCurrObj_CustomProcessSelectedItem;
            }
            else if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultView_Main" /*|| View.Id == "QCBatch_ListView_ResultView"*/))
            {
                ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                lstCurrObj.CustomProcessSelectedItem -= LstCurrObj_CustomProcessSelectedItem;
            }
            else if (View != null && View.Id == "SampleParameter_ListView_ResultView_ABID")
            {
                ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                lstCurrObj.CustomProcessSelectedItem -= LstCurrObj_CustomProcessSelectedItem;
            }
            if (View.Id == "ResultEntryQueryPanel_DetailView_Copy" || View.Id == "ResultViewQueryPanel_DetailView")
            {
                View.ObjectSpace.Dispose();
                ObjectSpace.Dispose();
            }
        }
        #endregion 

        #region Events
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "FilterDataByMonth")
                {
                    if (View.ObjectTypeInfo.Type == typeof(ResultEntryQueryPanel))
                    {
                        ResultEntryQueryPanel REQPanel = (ResultEntryQueryPanel)e.Object;
                        if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._1M))
                        {
                            objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddMonths(-1);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._3M))
                        {
                            objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddMonths(-3);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._6M))
                        {
                            objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddMonths(-6);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._1Y))
                        {
                            objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddYears(-1);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN.All))
                        {
                            DateTime dt = new DateTime();
                            objQPInfo.rgFilterByMonthDate = dt;
                        }
                    }
                }
                ////else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "SelectionMode" && View.ObjectTypeInfo.Type == typeof(ResultEntryQueryPanel))
                ////{
                ////    ResultEntryQueryPanel reQueryPanel = (ResultEntryQueryPanel)e.Object;
                ////    if (reQueryPanel != null)
                ////    {
                ////        DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                ////        DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                ////        DashboardViewItem viAB = (DashboardViewItem)((DetailView)View).FindItem("viewItemABID");

                ////        if (reQueryPanel.SelectionMode == QueryMode.Job)
                ////        {
                ////            objQPInfo.SelectMode = QueryMode.Job;
                ////            if (viJob != null)
                ////            {
                ////                ((Control)viJob.Control).Visible = true;
                ////            }
                ////            if (viQC != null)
                ////            {
                ////                ((Control)viQC.Control).Visible = false;
                ////            }
                ////            if (viAB != null)
                ////            {
                ////                ((Control)viAB.Control).Visible = false;
                ////            }
                ////        }
                ////        //else if (reQueryPanel.SelectionMode == QueryMode.QC)
                ////        //{
                ////        //    objQPInfo.SelectMode = QueryMode.QC;
                ////        //    if (viJob != null)
                ////        //    {
                ////        //        ((Control)viJob.Control).Visible = false;
                ////        //    }
                ////        //    if (viQC != null)
                ////        //    {
                ////        //        ((Control)viQC.Control).Visible = true;
                ////        //    }
                ////        //    if (viAB != null)
                ////        //    {
                ////        //        ((Control)viAB.Control).Visible = false;
                ////        //    }
                ////        //}
                ////        else if (reQueryPanel.SelectionMode == QueryMode.ABID)
                ////        {
                ////            objQPInfo.SelectMode = QueryMode.ABID;
                ////            if (viJob != null)
                ////            {
                ////                ((Control)viJob.Control).Visible = false;
                ////            }
                ////            if (viQC != null)
                ////            {
                ////                ((Control)viQC.Control).Visible = true;
                ////            }
                ////            if (viAB != null)
                ////            {
                ////                ((Control)viAB.Control).Visible = true;
                ////            }
                ////        }
                ////    }
                ////}

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void QueryPanelViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                int RowCount = 0;
                if ((View is ListView) && (View.ObjectTypeInfo.Type == typeof(Project)))
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[ProjectId] IS NOT NULL");
                }
                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                {
                    if (objQPInfo.ResultEntryQueryFilter != string.Empty && QueryPanel == true)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]=0 AND [GCRecord] IS NULL");
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Samplelogin.JobID.JobID]==NULL");
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status]='PendingEntry' AND [SubOut]=True");                    
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status]=0 and [Samplelogin.JobID.RecievedDate]  BETWEEN('" + DateTime.Now.AddMonths(-3) + "', '" + DateTime.Now + "') AND [SubOut]=True");
                    }
                }
                if (View != null && View.Id == "SampleParameter_ListView_Copy_CustomReporting")
                {
                    if (string.IsNullOrEmpty(objQPInfo.ResultEntryQueryFilter))
                    {
                        ((ListView)View).CollectionSource.Criteria["reporting"] = CriteriaOperator.Parse("[ValidatedDate] IS NOT NULL AND [ValidatedBy] IS NOT NULL And [SignOff] = True");
                    }
                    else if (!string.IsNullOrEmpty(objQPInfo.ResultEntryQueryFilter))
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status] !=0 And [SignOff] = True");
                    }
                }
                else if (View != null && View.Id == "SampleParameter_ListView_Copy_ResultEntry")
                {
                    if (objQPInfo.FromDashboard)
                    {
                        return;//no need to apply the filter criteria because the filter criteria is applied from the ShowDetailViewFromDashboardIView itself.
                    }
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView cv = nestedFrame.ViewItem.View;
                        if (cv != null && cv.Id == "ResultEntry_Enter")
                        {
                            QueryPanel = true;
                        }
                    }
                    bool isAdministrator = false;
                    List<string> lstTests = new List<string>();
                    string strTestMethodsPermissionCriteria = string.Empty;

                    CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        isAdministrator = true;
                    }
                    else
                    {
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTests.Contains(testMethod.TestName))
                                    {
                                        lstTests.Add(testMethod.TestName);
                                    }
                                }
                            }
                        }
                        //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                    }

                    if (!string.IsNullOrEmpty(objQPInfo.ResultEntryQueryFilter) && QueryPanel == true)
                    {
                        //Or([SubOut] = True AND )
                        if (isAdministrator)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]='PendingEntry' AND [GCRecord] IS NULL");
                        }
                        else
                        {
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]='PendingEntry' AND ([SubOut] is null Or [SubOut]=False)" + strTestMethodsPermissionCriteria);
                            ((ListView)View).CollectionSource.Criteria["filter"] = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]='PendingEntry'"),
                                new InOperator("Testparameter.TestMethod.TestName", lstTests));
                        }
                    }
                    else
                    {
                        if (objQPInfo.Fromloaddefault && !!string.IsNullOrEmpty(objQPInfo.tempResultEntryQueryFilter))
                        {
                            objQPInfo.Fromloaddefault = false;
                            objQPInfo.ResultEntryQueryFilter = objQPInfo.tempResultEntryQueryFilter;
                            if (isAdministrator)
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]='PendingEntry' AND [GCRecord] IS NULL");
                            }
                            else
                            {
                                //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]='PendingEntry' AND ([SubOut] is null Or [SubOut]=False)" + strTestMethodsPermissionCriteria);
                                ((ListView)View).CollectionSource.Criteria["filter"] = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]='PendingEntry'"),
                                new InOperator("Testparameter.TestMethod.TestName", lstTests));
                            }
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Samplelogin.JobID.JobID]==NULL");
                        }
                    }
                }
                else if (View != null && (View.Id == "SampleParameter_ListView_Copy_QCResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultView"))
                {
                    if (!string.IsNullOrEmpty(objQPInfo.QCResultEntryQueryFilter))
                    {
                        bool isAdministrator = false;
                        List<string> lstTests = new List<string>();
                        //string strTestMethodsPermissionCriteria = string.Empty;

                        CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            isAdministrator = true;
                        }
                        else
                        {
                            IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));
                            if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                            {
                                foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                {
                                    foreach (TestMethod testMethod in departmentChain.TestMethods)
                                    {
                                        if (!lstTests.Contains(testMethod.TestName))
                                        {
                                            lstTests.Add(testMethod.TestName);
                                        }
                                    }
                                }
                            }
                            //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                        }

                        if (isAdministrator || View.Id == "SampleParameter_ListView_Copy_QCResultView")
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(objQPInfo.QCResultEntryQueryFilter + " AND [UQABID.GCRecord] IS NULL AND [GCRecord] IS NULL");
                        }
                        else
                        {
                            //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(objQPInfo.QCResultEntryQueryFilter + " AND ([SubOut] is null Or [SubOut]=False)" + strTestMethodsPermissionCriteria);
                            ((ListView)View).CollectionSource.Criteria["filter"] = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse(objQPInfo.QCResultEntryQueryFilter + " AND [UQABID.GCRecord] IS NULL  AND [Status]='PendingEntry'"),
                                new InOperator("Testparameter.TestMethod.TestName", lstTests));
                        }
                    }
                }
                else if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultView"))
                {
                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView cv = nestedFrame.ViewItem.View;
                        if (cv != null && cv.Id == "Result_View")
                        {
                            QueryPanel = true;
                        }
                    }
                    bool isAdministrator = false;
                    List<string> lstTests = new List<string>();
                    //string strTestMethodsPermissionCriteria = string.Empty;

                    CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        isAdministrator = true;
                    }
                    else
                    {
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTests.Contains(testMethod.TestName))
                                    {
                                        lstTests.Add(testMethod.TestName);
                                    }
                                }
                            }
                        }
                        //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                    }

                    if (!string.IsNullOrEmpty(objQPInfo.ResultEntryQueryFilter) && QueryPanel == true)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(objQPInfo.ResultEntryQueryFilter + " AND [GCRecord] IS NULL");
                        //if (isAdministrator)
                        //{
                        //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(objQPInfo.ResultEntryQueryFilter + " AND ([SubOut] is null Or [SubOut]=False) And [GCRecord] IS NULL"); 
                        //}
                        //else
                        //{
                        //    //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(objQPInfo.ResultEntryQueryFilter + " AND ([SubOut] is null Or [SubOut]=False)" + strTestMethodsPermissionCriteria);
                        //    ((ListView)View).CollectionSource.Criteria["filter"] = new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse(objQPInfo.ResultEntryQueryFilter + " AND ([SubOut] is null Or [SubOut]=False)"),
                        //        new InOperator("Testparameter.TestMethod.TestName", lstTests));
                        //}
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Samplelogin.JobID.JobID]==NULL");
                    }

                }
                else if (View != null && (View.Id == "Result_DV"))
                {
                    DashboardViewItem dv = ((DashboardView)View).FindItem("Sample") as DashboardViewItem;
                    if (dv.InnerView != null)
                    {
                        if (!string.IsNullOrEmpty(objQPInfo.ResultEntryQueryFilter) && QueryPanel == true)
                        {
                            CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + "");
                            ((ListView)dv.InnerView).CollectionSource.Criteria["filter"] = criteria;
                        }
                    }

                }
                //ApplyCriteriaForQueryPanel();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Functions
        private void SetEnableDisableControls(DetailView dtView)
        {
            try
            {
                if (dtView != null)
                {
                    foreach (PropertyEditor editor in ((DetailView)dtView).GetItems<PropertyEditor>())
                    {
                        if (View != null && View.Id == "Reporting_ListView")
                        {
                            if (editor.Id == "JobID")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "ClientName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "ProjectID")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "ProjectName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "ReportID")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "MatrixName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "TestName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "MethodName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "AnalyzedDateFrom")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "AnalyzedDateTo")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "ReceivedDateFrom")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "ReceivedDateTo")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }

                        }
                        else if (View != null && View.Id == "SampleParameter_ListView_Copy_CustomReporting")
                        {
                            if (editor.Id == "JobID")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "MatrixName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "TestName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "MethodName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "ClientName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "ProjectID")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "ProjectName")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", true);
                            }
                            else if (editor.Id == "AnalyzedDateFrom")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "AnalyzedDateTo")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "ReceivedDateFrom")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "ReceivedDateTo")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
                            }
                            else if (editor.Id == "ReportID")
                            {
                                editor.AllowEdit.SetItemValue("AllowEdit", false);
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

        #endregion

        private void Loaddefault_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //objQPInfo.tempResultEntryQueryFilter = objQPInfo.ResultEntryQueryFilter;
                //IObjectSpace os = Application.CreateObjectSpace();
                //Defaultsettingload objdefault = os.CreateObject<Defaultsettingload>();
                //foreach (XPMemberInfo obj in objdefault.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name != "oid").ToList())
                //{
                //    obj.SetValue(objdefault, true);
                //}
                //DetailView dv = Application.CreateDetailView(os, objdefault);
                //dv.ViewEditMode = ViewEditMode.Edit;
                //ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                //showViewParameters.Context = TemplateContext.PopupWindow;
                //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                //showViewParameters.CreatedView.Caption = "Load Default";
                //DialogController dc = Application.CreateController<DialogController>();
                //dc.SaveOnAccept = false;
                //dc.Accepting += Dc_Accepting;
                //showViewParameters.Controllers.Add(dc);
                //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
                //Dictionary<string, string> diffname = new Dictionary<string, string>();
                //diffname.Add("Result", "DefaultResult");
                //diffname.Add("Units", "DefaultUnits");
                ////diffname.Add("SurrogateSpikeAmount", "SurrogateAmount");
                //Defaultsettingload objdefault = (Defaultsettingload)e.AcceptActionArgs.CurrentObject;
                //foreach (SampleParameter sample in ((ListView)View).CollectionSource.List.Cast<SampleParameter>().OrderBy(a => a.QCSort).ToList())
                //{
                //    foreach (XPMemberInfo obj in objdefault.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name != "oid").ToList())
                //    {
                //        if (Convert.ToBoolean(obj.GetValue(objdefault)) == true)
                //        {
                //            var sproperty = sample.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == obj.Name).ToList();
                //            if (sproperty.Count == 1 && sample.Testparameter != null)
                //            {
                //                List<XPMemberInfo> tmproperty = new List<XPMemberInfo>();
                //                if (diffname.Count > 0 && diffname.ContainsKey(sproperty[0].Name))
                //                {
                //                    tmproperty = sample.Testparameter.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == diffname[sproperty[0].Name]).ToList();
                //                }
                //                else
                //                {
                //                    tmproperty = sample.Testparameter.ClassInfo.PersistentProperties.Cast<XPMemberInfo>().Where(a => a.Name == sproperty[0].Name).ToList();
                //                }
                //                if (tmproperty.Count == 1)
                //                {
                //                    sproperty[0].SetValue(sample, tmproperty[0].GetValue(sample.Testparameter));
                //                }
                //            }
                //        }
                //    }
                //}
                //objQPInfo.Fromloaddefault = true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ResultViewHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Employee currentUser = SecuritySystem.CurrentUser as Employee;
                if (currentUser != null && View != null && View.Id != null)
                {
                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            IObjectSpace os = Application.CreateObjectSpace(typeof(ResultEntryQueryPanel));
                            ResultEntryQueryPanel obj = os.CreateObject<ResultEntryQueryPanel>();
                            DetailView Dvresultview = Application.CreateDetailView(os, "ResultViewQueryPanel_DetailView", true, obj);
                            Dvresultview.ViewEditMode = ViewEditMode.Edit;
                            Frame.SetView(Dvresultview);
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "ResultView") != null)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace(typeof(ResultEntryQueryPanel));
                                    ResultEntryQueryPanel obj = os.CreateObject<ResultEntryQueryPanel>();
                                    DetailView Dvresultview = Application.CreateDetailView(os, "ResultViewQueryPanel_DetailView", true, obj);
                                    Dvresultview.ViewEditMode = ViewEditMode.Edit;
                                    Frame.SetView(Dvresultview);
                                    break;
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Permissiondenied"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
        private void RetriveResultEntry_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                String strResultEntrySelection = string.Empty;
                strResultEntrySelection = ResultEntrySelection.SelectedItem.Id.ToString();
                //ResultEntryQueryPanel objCurrent = (ResultEntryQueryPanel)View.CurrentObject;
                if (strResultEntrySelection == "ResultEntryJOBID")
                {
                    DashboardViewItem lvSamples = ((DetailView)View).FindItem("viewItemJobID") as DashboardViewItem;
                    if (lvSamples != null && lvSamples.InnerView != null)
                    {
                        if (lvSamples.InnerView.SelectedObjects.Count == 1)
                        {
                            objQPInfo.lstJobID = new List<string>();
                            objQPInfo.lstQCJobID = new List<string>();
                            objQPInfo.objJobID = null;
                            //objQPInfo.SelectMode = QueryMode.Job;
                            foreach (Modules.BusinessObjects.SampleManagement.Samplecheckin objsample in lvSamples.InnerView.SelectedObjects)
                            {
                                objQPInfo.lstJobID.Add(objsample.JobID);
                                objQPInfo.objJobID = objsample.JobID;
                                CNInfo.SCoidValue = objsample.Oid;
                                if (!string.IsNullOrEmpty(objsample.SampleMatries))
                                {
                                    StringBuilder sb = new StringBuilder();
                                    foreach (string strMatrix in objsample.SampleMatries.Split(';'))
                                    {
                                        VisualMatrix objSM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                        if (sb.Length > 0)
                                        {
                                            sb.Append(";"); // Add semicolon before appending the next name
                                        }
                                        sb.Append(objSM.VisualMatrixName);
                                    }
                                    CNInfo.RESampleMatries = sb.ToString();
                                }

                            }
                            QueryPanel = true;
                            DefaultSetting objDefault = View.ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True"));
                            DefaultSetting objDefaultmodule = View.ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparationRootNode'AND [Select]=True"));
                            if (lvSamples.InnerView.Id == "Samplecheckin_ListView_ResultEntry")
                            {
                                objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.qcseqdetail.Jobid] Like '%" + objQPInfo.objJobID + "%' And [QCBatchID.SampleID.JobID.JobID] IN(" + "'" + string.Join("', '", objQPInfo.lstJobID) + "'" + ")";
                                if (objDefault != null && objDefaultmodule != null)
                                {
                                    objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN(" + "'" + string.Join("', '", objQPInfo.lstJobID) + "'" + ") And[SignOff] = True and [IsTransferred] = true And ([ABID] is Null or ([ABID] is not null and [Status] = '" + Samplestatus.PendingEntry + "')) And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [IsPrepMethodComplete]  = True) OR [Testparameter.TestMethod.PrepMethods][].Count() == 0 And ([IsPrepMethodComplete]  = False Or [IsPrepMethodComplete] Is Null) ) And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False)";
                                }
                                else
                                {
                                    objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN(" + "'" + string.Join("', '", objQPInfo.lstJobID) + "'" + ") And[SignOff] = True and [IsTransferred] = true And [Testparameter.TestMethod.IsFieldTest] <> True And([ABID] is Null or ([ABID] is not null and [Status] = '" + Samplestatus.PendingEntry + "'))";
                                }
                                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                                bool isAdministrator = false;
                                List<Guid> lstTests = new List<Guid>();
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    isAdministrator = true;
                                }
                                else
                                {
                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));
                                    lstTests = lstAnalysisDepartChain.SelectMany(x => x.TestMethods).Select(x => x.Oid).Distinct().ToList();
                                    //if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                    //{
                                    //    foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                    //    {
                                    //        foreach (TestMethod testMethod in departmentChain.TestMethods)
                                    //        {
                                    //            if (!lstTests.Contains(testMethod.Oid))
                                    //            {
                                    //                lstTests.Add(testMethod.Oid);
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                if (lstTests.Count > 0)
                                {
                                    analysisDeptUser.lstAnalysisEmp = new List<Employee>();
                                    foreach (Guid objtm in lstTests.ToList())
                                    {
                                        TestMethod testMethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", objtm));
                                        if (testMethod != null && testMethod.ResultEntryUsers != null)
                                        {
                                            string[] stremparr = testMethod.ResultEntryUsers.Split(',');
                                            foreach (string strempname in stremparr.ToList())
                                            {
                                                Employee employee = ObjectSpace.FindObject<Employee>(CriteriaOperator.Parse("[DisplayName] = ?", strempname));
                                                if (employee != null)
                                                {
                                                    if (!analysisDeptUser.lstAnalysisEmp.Contains(employee))
                                                    {
                                                        analysisDeptUser.lstAnalysisEmp.Add(employee);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    objQPInfo.ResultEntryADCFilter = "[Testparameter.TestMethod.Oid] IN(" + "'" + string.Join("', '", lstTests) + "'" + ")";
                                }
                                else
                                {
                                    objQPInfo.ResultEntryADCFilter = string.Empty;
                                }
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                                cs.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]=0 And [GCRecord] IS NULL And ([TestHold] = False Or [TestHold] Is null) ");
                                ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultEntry_SingleChoice", cs, true);
                                Frame.SetView(listview);
                            }
                            else
                            {
                                objQPInfo.ResultEntryQueryFilter = "[Samplelogin.JobID.JobID] IN (" + "'" + string.Join("','", objQPInfo.lstJobID) + "'" + ") And [SignOff] = True and [IsTransferred] = true And ([UQABID] is Null or ([UQABID] is not null ))";//and [Status] <>'" + Samplestatus.PendingReview + "' and [Status] <>'" + Samplestatus.PendingVerify + "'
                                                                                                                                                                                                                                                                 // objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.qcseqdetail.Jobid] Like '%" + objQPInfo.objJobID + "%' And [QCBatchID.SampleID.JobID.JobID] IN(" + "'" + string.Join("', '", objQPInfo.lstJobID) + "'" + ")";
                                objQPInfo.QCResultEntryQueryFilter = "[QCBatchID.qcseqdetail.Jobid] Like '%" + objQPInfo.objJobID + "%'";
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                                cs.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [GCRecord] IS NULL");
                                ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultView_SingleChoice", cs, true);
                                Frame.SetView(listview);
                            }

                        }
                        else
                        {
                            if (lvSamples.InnerView.SelectedObjects.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        IList<SampleParameter> objSampleLogin = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " And [GCRecord] IS NULL And ([TestHold] = False Or [TestHold] Is null) "));
                        if (objSampleLogin.Count > 0)
                        {
                            SampleParameter firstSampleParameter = objSampleLogin.FirstOrDefault(i => i.UQABID != null);
                            if (firstSampleParameter != null && firstSampleParameter.UQABID != null)
                            {
                                CNInfo.REQCBatchId = firstSampleParameter.UQABID.AnalyticalBatchID;
                            }
                            else
                            {
                                SampleParameter firstSampleParameter1 = objSampleLogin.FirstOrDefault(i => i.UQABID == null);

                                CNInfo.REQCBatchId = firstSampleParameter1.Samplelogin.JobID.JobID;
                            }
                        }
                    }

                }
                else if (strResultEntrySelection == "ResultEntryQCBatchID") //(objCurrent != null && objCurrent.SelectionMode == QueryMode.ABID)
                {
                    DashboardViewItem lvQcSamples = ((DetailView)View).FindItem("viewItemQCBatchID") as DashboardViewItem;
                    if (lvQcSamples != null && lvQcSamples.InnerView != null)
                    {
                        if (lvQcSamples.InnerView.SelectedObjects.Count == 1)
                        {
                            objQPInfo.lstQCBatchID = new List<string>();
                            objQPInfo.objJobID = null;
                            //objQPInfo.SelectMode = QueryMode.ABID;
                            foreach (SampleParameter objsample in lvQcSamples.InnerView.SelectedObjects)
                            {
                                objQPInfo.lstQCBatchID.Add(objsample.QCBatchID.qcseqdetail.AnalyticalBatchID);
                                objQPInfo.objJobID = objsample.Samplelogin.JobID.JobID;
                                CNInfo.SCoidValue = objsample.Oid;
                                if (!string.IsNullOrEmpty(objsample.Samplelogin.JobID.SampleMatries))
                                {
                                    StringBuilder sb = new StringBuilder();
                                    foreach (string strMatrix in objsample.Samplelogin.JobID.SampleMatries.Split(';'))
                                    {
                                        VisualMatrix objSM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                        if (sb.Length > 0)
                                        {
                                            sb.Append(";"); // Add semicolon before appending the next name
                                        }
                                        sb.Append(objSM.VisualMatrixName);
                                    }
                                    CNInfo.RESampleMatries = sb.ToString();
                                }
                            }
                            QueryPanel = true;
                            objQPInfo.ResultEntryQueryFilter = "[QCBatchID.qcseqdetail.AnalyticalBatchID] IN (" + "'" + string.Join("','", objQPInfo.lstQCBatchID) + "'" + ") And [SignOff] = True";
                            objQPInfo.QCResultEntryQueryFilter = objQPInfo.ResultEntryQueryFilter;

                            if (lvQcSamples.InnerView.Id == "QCBatch_ListView_ResultEntry")
                            {
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                                CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                                bool isAdministrator = false;
                                List<Guid> lstTests = new List<Guid>();
                                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    isAdministrator = true;
                                }
                                else
                                {
                                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));
                                    lstTests = lstAnalysisDepartChain.SelectMany(x => x.TestMethods).Select(x => x.Oid).Distinct().ToList();
                                    //if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                                    //{
                                    //    foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                    //    {
                                    //        foreach (TestMethod testMethod in departmentChain.TestMethods)
                                    //        {
                                    //            if (!lstTests.Contains(testMethod.Oid))
                                    //            {
                                    //                lstTests.Add(testMethod.Oid);
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //strTestMethodsPermissionCriteria = "And [Testparameter.TestMethod.Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestMethodOids.Select(i => i.ToString().Replace("'", "''")))) + ") And [GCRecord] IS NULL";
                                }
                                if (lstTests.Count > 0)
                                {
                                    objQPInfo.ResultEntryADCFilter = "[Testparameter.TestMethod.Oid] IN(" + "'" + string.Join("', '", lstTests) + "'" + ")";
                                }
                                else
                                {
                                    objQPInfo.ResultEntryADCFilter = string.Empty;
                                }
                                cs.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [Status]=0 AND [GCRecord] IS NULL");
                                ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultEntry_SingleChoice", cs, true);
                                Frame.SetView(listview);
                            }
                            else
                            {
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                                cs.Criteria["filter"] = CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " AND [GCRecord] IS NULL");
                                ListView listview = Application.CreateListView("SampleParameter_ListView_Copy_ResultView_SingleChoice", cs, true);
                                Frame.SetView(listview);
                            }
                        }
                        else
                        {
                            if (lvQcSamples.InnerView.SelectedObjects.Count == 0)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                    IList<SampleParameter> objSampleLogin = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("" + objQPInfo.ResultEntryQueryFilter + " And [GCRecord] IS NULL And ([TestHold] = False Or [TestHold] Is null) "));
                    if (objSampleLogin.Count > 0)
                    {
                        SampleParameter firstSampleParameter = objSampleLogin.FirstOrDefault(i => i.UQABID != null);
                        if (firstSampleParameter != null && firstSampleParameter.UQABID != null)
                        {
                            CNInfo.REQCBatchId = firstSampleParameter.UQABID.AnalyticalBatchID;
                        }
                        else
                        {
                            SampleParameter firstSampleParameter1 = objSampleLogin.FirstOrDefault(i => i.UQABID == null);
                            CNInfo.REQCBatchId = firstSampleParameter1.Samplelogin.JobID.JobID;
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
        private void Comment_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    Notes objToShow = objspace.CreateObject<Notes>();
                    if (objToShow != null)
                    {
                        DetailView dvInput = Application.CreateDetailView(objspace, "Notes_DetailView", true, objToShow);
                        dvInput.Caption = "Comment";
                        dvInput.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(dvInput);
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.CreatedView = dvInput;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = true;
                        dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dc_Accepting1;
                        dc.ViewClosed += Dc_ViewClosed;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
                else if (View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice")
                {
                    IObjectSpace os = Application.CreateObjectSpace(typeof(Notes));
                    //Notes objcrtdummy = os.CreateObject<Notes>();
                    CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Notes));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("[Samplecheckin.Oid] = ? AND [NoteSource] <> 'Sample Registration' AND [NoteSource] <> 'QC Batch' AND [NoteSource] <> 'Sample Prepration' AND [NoteSource] <> 'Custom Reporting'", CNInfo.SCoidValue);
                    ListView lvparameter = Application.CreateListView("Notes_ListView_CaseNarrative_ResultEntry", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    showViewParameters.CreatedView = lvparameter;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    //dc.Accepting += CaseDC_Accepting;
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

        private void Dc_ViewClosed(object sender, EventArgs e)
        {
            try
            {
                if (View != null && !string.IsNullOrEmpty(View.Id))
                {
                    strviewid.strtempviewid = View.Id.ToString();
                    strviewid.strsampleviewtype = View.ObjectTypeInfo.Name.ToString();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting1(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DialogController DC = sender as DialogController;
                Notes param = (Notes)e.AcceptActionArgs.CurrentObject;
                if (param.Text != null && param.Title != null)
                {
                    param.Samplecheckin = DC.Window.View.ObjectSpace.GetObjectByKey<Samplecheckin>(CNInfo.SCoidValue);
                    DC.Window.View.ObjectSpace.CommitChanges();
                }

            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

    }
}
