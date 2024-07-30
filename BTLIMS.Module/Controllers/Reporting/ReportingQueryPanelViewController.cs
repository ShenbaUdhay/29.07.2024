using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTLIMS.Module.Controllers.Reporting
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ReportingQueryPanelViewController : ViewController
    {
        GridListEditor gridListEditor;
        MessageTimer timer = new MessageTimer();
        ReportingQueryPanelInfo objRQPInfo = new ReportingQueryPanelInfo();
        object rgFilterValue;
        #region Constructor
        public ReportingQueryPanelViewController()
        {
            InitializeComponent();
            TargetViewId = "Reporting_ListView;" + "ReportingQueryPanel_DetailView;" + "ReportingQueryPanel_DetailView_Validation;" +
                "Reporting_ListView_Copy_ReportApproval;" + "Reporting_ListView_Copy_ReportView;" + "SampleParameter_ListView_ReportingValidation_QueryPanel;" +
                "SampleParameter_ListView_ReportingApproval_QueryPanel;" + "ReportingQueryPanel_DetailView_ResultView;" +
                "SampleParameter_ListView_ReportingView_QueryPanel;" + "Customer_LookupListView;" +
                "Project_LookupListView;" + "Reporting_LookupListView;" + "SampleParameter_ListView_Copy_Reporting_MainView;" +
                "Reporting_ListView_ValidationQueryPanel;" + "Reporting_ListView_ApprovalQueryPanel;" +
                "Reporting_ListView_ReportViewQueryPanel;" + "Samplecheckin_ListView_Copy_Reporting;";
            // ReportingQueryPanelPopupWindow.TargetViewId = "Reporting_ListView;" + "Reporting_ListView_Copy_ReportApproval;";
            objRQPInfo.ReportingQueryFilter = string.Empty;
            //OpenReportJobId.TargetViewId = "ReportingQueryPanel_DetailView;" + "ReportingQueryPanel_DetailView_Validation;" +
            //    "ReportingQueryPanel_DetailView_ResultView;";
            reportingDateFilterAction.TargetViewId = "ReportingQueryPanel_DetailView;" + "ReportingQueryPanel_DetailView_Validation;"
                + "ReportingQueryPanel_DetailView_ResultView;" + "SampleParameter_ListView_Copy_Reporting_MainView;" +
                "Reporting_ListView_ValidationQueryPanel;" + "Reporting_ListView_ApprovalQueryPanel;" +
                "Reporting_ListView_ReportViewQueryPanel;" + "Samplecheckin_ListView_Copy_Reporting;";
            reportingJobIDFilterAction.TargetViewId = "SampleParameter_ListView_Copy_Reporting_MainView;" + "Samplecheckin_ListView_Copy_Reporting;";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ObjectSpace.ObjectChanged += new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);
                if (View.Id == "SampleParameter_ListView_ReportingValidation_QueryPanel" || View.Id == "SampleParameter_ListView_ReportingApproval_QueryPanel"
                    || View.Id == "SampleParameter_ListView_ReportingView_QueryPanel"
                    )
                {
                    ((ListView)View).SelectionChanged += ReportingQueryPanelViewController_SelectionChanged;
                    //using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    //{
                    //    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                    //    lstview.Properties.Add(new ViewProperty("TOID", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //    List<object> jobid = new List<object>();
                    //    foreach (ViewRecord rec in lstview)
                    //        jobid.Add(rec["TOID"]);
                    //    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", jobid);
                    //}
                    Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<SampleParameter> lstSamples = uow.Query<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).ToList();
                    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", lstSamples.Select(i => i.Samplelogin.JobID.Oid).Distinct());
                }
                else if (View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                    || View.Id == "Reporting_ListView_ReportViewQueryPanel")
                {
                    ((ListView)View).SelectionChanged += ReportingQueryPanelViewController_SelectionChanged;
                }

                if (View.Id == "ReportingQueryPanel_DetailView" || View.Id == "ReportingQueryPanel_DetailView_Validation"
                    || View.Id == "ReportingQueryPanel_DetailView_ResultView" || View.Id == "SampleParameter_ListView_Copy_Reporting_MainView"
                    || View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                    || View.Id == "Reporting_ListView_ReportViewQueryPanel" || View.Id == "Samplecheckin_ListView_Copy_Reporting")
                {
                    reportingDateFilterAction.SelectedItemChanged += ReportingDateFilterAction_SelectedItemChanged;
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (reportingDateFilterAction.SelectedItem == null)
                    {
                        if (setting.ReportingWorkFlow == EnumDateFilter.OneMonth)
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                            reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[0];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.ThreeMonth)
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                            reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.SixMonth)
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                            reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[2];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.OneYear)
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                            reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[3];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.TwoYear)
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-2);
                            reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[4];
                        }
                        else if (setting.ReportingWorkFlow == EnumDateFilter.FiveYear)
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-5);
                            reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[5];
                        }
                        else
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                            reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[6];
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    //        objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    //        reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[0];
                    if (View.Id == "SampleParameter_ListView_Copy_Reporting_MainView" || View.Id == "Samplecheckin_ListView_Copy_Reporting")
                    {
                        reportingJobIDFilterAction.SelectedItemChanged += ReportingJobIDFilterAction_SelectedItemChanged;
                        if (reportingJobIDFilterAction.SelectedItem == null)
                        {
                            reportingJobIDFilterAction.SelectedItem = reportingJobIDFilterAction.Items[1];
                        }
                        ////Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                        ////UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        ////List<SampleParameter> lstSamples = null;
                        ////lstSamples = uow.Query<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Status == Samplestatus.PendingReporting).ToList();
                        ////lstSamples = lstSamples.GroupBy(i => i.Samplelogin.JobID.JobID).Select(grp => grp.FirstOrDefault()).ToList();
                        ////((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", lstSamples.Select(i => i.Oid));
                    }
                }
                objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                objRQPInfo.rgFilterByJobID = "PendingJobID";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportingJobIDFilterAction_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SampleParameter_ListView_Copy_Reporting_MainView" || View.Id == "Samplecheckin_ListView_Copy_Reporting")
                {
                    Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<SampleParameter> lstSamples = null;
                    if (reportingJobIDFilterAction.SelectedItem.Id == "AllJobID")
                    {
                        objRQPInfo.rgFilterByJobID = "AllJobID";
                        lstSamples = uow.Query<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && (i.Status == Samplestatus.PendingReporting || i.Status == Samplestatus.Reported)).ToList();
                        lstSamples = lstSamples.GroupBy(i => i.Samplelogin.JobID.JobID).Select(grp => grp.FirstOrDefault()).ToList();
                    }
                    else if (reportingJobIDFilterAction.SelectedItem.Id == "PendingJobID")
                    {
                        objRQPInfo.rgFilterByJobID = "PendingJobID";
                        lstSamples = uow.Query<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Status == Samplestatus.PendingReporting).ToList();
                        lstSamples = lstSamples.GroupBy(i => i.Samplelogin.JobID.JobID).Select(grp => grp.FirstOrDefault()).ToList();
                    }
                    ////((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", lstSamples.Select(i=>i.Oid));
                    if (objRQPInfo.rgFilterByMonthDate == DateTime.MinValue)
                    {
                        SelectedData sproc = currentSession.ExecuteSproc("GetCustomReportingData", new OperandValue("DateFilterRemove"), new OperandValue(objRQPInfo.rgFilterByMonthDate), new OperandValue(objRQPInfo.rgFilterByJobID));
                        List<Guid> lstsmploid = new List<Guid>();
                        foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                        {
                            if (!lstsmploid.Contains(new Guid(row.Values[0].ToString())))
                            {
                                lstsmploid.Add(new Guid(row.Values[0].ToString()));
                            }
                        }
                        if (lstsmploid.Count > 0)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                        }
                    }
                    else
                    {
                        SelectedData sproc = currentSession.ExecuteSproc("GetCustomReportingData", new OperandValue("DateFilter"), new OperandValue(objRQPInfo.rgFilterByMonthDate), new OperandValue(objRQPInfo.rgFilterByJobID));
                        List<Guid> lstsmploid = new List<Guid>();
                        foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                        {
                            if (!lstsmploid.Contains(new Guid(row.Values[0].ToString())))
                            {
                                lstsmploid.Add(new Guid(row.Values[0].ToString()));
                            }
                        }
                        if (lstsmploid.Count > 0)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                        }
                        else
                        {
                            ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                        }
                    }

                    //using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    //{
                    //    if (reportingJobIDFilterAction.SelectedItem.Id == "AllJobID")
                    //    {
                    //        objRQPInfo.rgFilterByJobID = "AllJobID";
                    //        lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingReporting' or [Status]='Reported'");
                    //    }
                    //    else if (reportingJobIDFilterAction.SelectedItem.Id == "PendingJobID")
                    //    {
                    //        objRQPInfo.rgFilterByJobID = "PendingJobID";
                    //        lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingReporting'");
                    //    }
                    //    //lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                    //    //lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    //    //List<object> jobid = new List<object>();
                    //    //foreach (ViewRecord rec in lstview)
                    //    //    jobid.Add(rec["Toid"]);
                    //    //((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", jobid);
                    //    Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    //    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    //    List<SampleParameter> lstSamples = uow.Query<SampleParameter>().Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).ToList();
                    //    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", lstSamples.Select(i => i.Samplelogin.JobID.Oid).Distinct());
                    //}
                    //((DevExpress.ExpressApp.ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportingDateFilterAction_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && reportingDateFilterAction != null && reportingDateFilterAction.SelectedItem != null &&
                        (View.Id == "ReportingQueryPanel_DetailView" || View.Id == "ReportingQueryPanel_DetailView_Validation"
                         || View.Id == "ReportingQueryPanel_DetailView_ResultView" || View.Id == "SampleParameter_ListView_Copy_Reporting_MainView"
                         || View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                         || View.Id == "Reporting_ListView_ReportViewQueryPanel" || View.Id == "Samplecheckin_ListView_Copy_Reporting"))
                {
                    if (reportingDateFilterAction.SelectedItem.Id == "1M")
                    {
                        objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    }
                    else if (reportingDateFilterAction.SelectedItem.Id == "3M")
                    {
                        objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                    }
                    else if (reportingDateFilterAction.SelectedItem.Id == "6M")
                    {
                        objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                    }
                    else if (reportingDateFilterAction.SelectedItem.Id == "1Y")
                    {
                        objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                    }
                    else if (reportingDateFilterAction.SelectedItem.Id == "2Y")
                    {
                        objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-2);
                    }
                    else if (reportingDateFilterAction.SelectedItem.Id == "5Y")
                    {
                        objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-5);
                    }
                    else
                    {
                        objRQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                    }

                    if (objRQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                    {
                        if (View is ListView && (View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                     || View.Id == "Reporting_ListView_ReportViewQueryPanel"))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[ReportedDate] BETWEEN('" + objRQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')");
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_Reporting_MainView" || View.Id == "Samplecheckin_ListView_Copy_Reporting")
                        {
                            //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] >= ? and [Samplelogin.JobID.RecievedDate] < ?", objQPInfo.rgFilterByMonthDate, DateTime.Now);
                            //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[Samplelogin.JobID.RecievedDate] BETWEEN('" + objRQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')");
                            //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[RecievedDate] >= ? and [RecievedDate] < ?", objRQPInfo.rgFilterByMonthDate, DateTime.Now);
                            Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                            SelectedData sproc = currentSession.ExecuteSproc("GetCustomReportingData", new OperandValue("DateFilter"), new OperandValue(objRQPInfo.rgFilterByMonthDate), new OperandValue(objRQPInfo.rgFilterByJobID));
                            List<Guid> lstsmploid = new List<Guid>();
                            foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                            {
                                if (!lstsmploid.Contains(new Guid(row.Values[0].ToString())))
                                {
                                    lstsmploid.Add(new Guid(row.Values[0].ToString()));
                                }
                            }
                            if (lstsmploid.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                            }
                        }
                    }
                    else
                    {
                        if (View.Id == "Samplecheckin_ListView_Copy_Reporting")
                        {
                            Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                            SelectedData sproc = currentSession.ExecuteSproc("GetCustomReportingData", new OperandValue("DateFilterRemove"), new OperandValue(objRQPInfo.rgFilterByMonthDate), new OperandValue(objRQPInfo.rgFilterByJobID));
                            List<Guid> lstsmploid = new List<Guid>();
                            foreach (SelectStatementResultRow row in sproc.ResultSet[0].Rows)
                            {
                                if (!lstsmploid.Contains(new Guid(row.Values[0].ToString())))
                                {
                                    lstsmploid.Add(new Guid(row.Values[0].ToString()));
                                }
                            }
                            if (lstsmploid.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsmploid);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is NULL");
                            }
                        }
                        else
                        {
                            if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey("dateFilter"))
                            {
                                ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                            }
                        }
                    }
                    //((DevExpress.ExpressApp.ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReportingQueryPanelViewController_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_ReportingValidation_QueryPanel" || View.Id == "SampleParameter_ListView_ReportingApproval_QueryPanel"
                    || View.Id == "SampleParameter_ListView_ReportingView_QueryPanel")
                {
                    if (objRQPInfo.lstJobID == null)
                    {
                        objRQPInfo.lstJobID = new List<string>();
                    }
                    else
                    {
                        objRQPInfo.lstJobID.Clear();
                    }

                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (SampleParameter obj in View.SelectedObjects)
                        {
                            if (objRQPInfo.lstJobID != null && !objRQPInfo.lstJobID.Contains(obj.Samplelogin.JobID.JobID))
                            {
                                objRQPInfo.lstJobID.Add(obj.Samplelogin.JobID.JobID);
                            }
                        }
                    }
                }
                else if (View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                    || View.Id == "Reporting_ListView_ReportViewQueryPanel")
                {
                    if (objRQPInfo.lstJobID == null)
                    {
                        objRQPInfo.lstJobID = new List<string>();
                    }
                    else
                    {
                        objRQPInfo.lstJobID.Clear();
                    }

                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (Modules.BusinessObjects.SampleManagement.Reporting obj in View.SelectedObjects)
                        {
                            if (objRQPInfo.lstJobID != null && !objRQPInfo.lstJobID.Contains(obj.JobID.JobID))
                            {
                                objRQPInfo.lstJobID.Add(obj.JobID.JobID);
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
                ObjectSpace.ObjectChanged -= new EventHandler<ObjectChangedEventArgs>(objectSpace_ObjectChanged);

                if (View.Id == "ReportingQueryPanel_DetailView" || View.Id == "ReportingQueryPanel_DetailView_Validation"
                    || View.Id == "ReportingQueryPanel_DetailView_ResultView" || View.Id == "SampleParameter_ListView_Copy_Reporting_MainView"
                    || View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                    || View.Id == "Reporting_ListView_ReportViewQueryPanel" || View.Id == "Samplecheckin_ListView_Copy_Reporting")
                {
                    reportingDateFilterAction.SelectedItemChanged -= ReportingDateFilterAction_SelectedItemChanged;
                }
                if (View.Id == "SampleParameter_ListView_Copy_Reporting_MainView" || View.Id == "Samplecheckin_ListView_Copy_Reporting")
                {
                    reportingJobIDFilterAction.SelectedItemChanged -= ReportingJobIDFilterAction_SelectedItemChanged;
                }
                if (View.Id == "SampleParameter_ListView_ReportingValidation_QueryPanel" || View.Id == "SampleParameter_ListView_ReportingApproval_QueryPanel"
                    || View.Id == "SampleParameter_ListView_ReportingView_QueryPanel" || View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                    || View.Id == "Reporting_ListView_ReportViewQueryPanel" || View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                        || View.Id == "Reporting_ListView_ReportViewQueryPanel")

                {
                    ((ListView)View).SelectionChanged -= ReportingQueryPanelViewController_SelectionChanged;
                }
                //objRQPInfo.CurrentViewID = string.Empty;
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
                if (View != null && (View.Id == "ReportingQueryPanel_DetailView" || View.Id == "ReportingQueryPanel_DetailView_Validation"
                    || View.Id == "ReportingQueryPanel_DetailView_ResultView"))
                {
                    objRQPInfo.CurrentViewID = View.Id;
                    //objRQPInfo.rgFilterByMonthDate = DateTime.Now;
                    //PropertyEditor rgEditor = (PropertyEditor)((DetailView)View).FindItem("FilterDataByMonth");
                    //if(rgEditor !=null)
                    //{
                    //    rgFilterValue = rgEditor.PropertyValue;
                    //    if (rgFilterValue.Equals(FilterByMonthEN._1M))
                    //    {
                    //        objRQPInfo.rgFilterByMonthDate = objRQPInfo.rgFilterByMonthDate.AddMonths(-1);
                    //    }
                    //    if (rgFilterValue.Equals(FilterByMonthEN._3M))
                    //    {
                    //        objRQPInfo.rgFilterByMonthDate = objRQPInfo.rgFilterByMonthDate.AddMonths(-3);
                    //    }
                    //    else if (rgFilterValue.Equals(FilterByMonthEN._6M))
                    //    {
                    //        objRQPInfo.rgFilterByMonthDate = objRQPInfo.rgFilterByMonthDate.AddMonths(-6);
                    //    }
                    //    else if (rgFilterValue.Equals(FilterByMonthEN._1Y))
                    //    {
                    //        objRQPInfo.rgFilterByMonthDate = objRQPInfo.rgFilterByMonthDate.AddYears(-1);
                    //    }
                    //}                  
                }
                else if (View.Id == "SampleParameter_ListView_ReportingApproval_QueryPanel" || View.Id == "SampleParameter_ListView_ReportingValidation_QueryPanel"
                    || View.Id == "SampleParameter_ListView_ReportingView_QueryPanel" || View.Id == "Reporting_ListView_ValidationQueryPanel"
                    || View.Id == "Reporting_ListView_ApprovalQueryPanel" || View.Id == "Reporting_ListView_ReportViewQueryPanel")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load += Grid_Load;
                    }
                    ListViewProcessCurrentObjectController lstCurrObj = Frame.GetController<ListViewProcessCurrentObjectController>();
                    lstCurrObj.CustomProcessSelectedItem += LstCurrObj_CustomProcessSelectedItem;
                    if (View.Id == "Reporting_ListView_ValidationQueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel"
                        || View.Id == "Reporting_ListView_ReportViewQueryPanel")
                    {
                        ((ListView)View).SelectionChanged += ReportingQueryPanelViewController_SelectionChanged;
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            if (View.Id == "Reporting_ListView_ValidationQueryPanel")
                            {
                                if (!string.IsNullOrEmpty(objRQPInfo.ReportingQueryFilter))
                                {
                                    lstview.Criteria = CriteriaOperator.Parse(objRQPInfo.ReportingQueryFilter + " AND [ReportValidatedDate] IS NULL AND[ReportValidatedBy] IS NULL");
                                }
                                else
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[ReportValidatedDate] IS NULL AND[ReportValidatedBy] IS NULL");
                                }
                            }
                            else
                            if (View.Id == "Reporting_ListView_ApprovalQueryPanel")
                            {
                                if (!string.IsNullOrEmpty(objRQPInfo.ReportingQueryFilter))
                                {
                                    lstview.Criteria = CriteriaOperator.Parse(objRQPInfo.ReportingQueryFilter + " AND [ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                                }
                                else
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                                }

                            }
                            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "JobID.JobID", true, true));
                            lstview.Properties.Add(new ViewProperty("TOID", SortDirection.Ascending, "MAX(Oid)", false, true));
                            List<object> jobid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                jobid.Add(rec["TOID"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", jobid);
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
                if (View.Id == "SampleParameter_ListView_ReportingApproval_QueryPanel" || View.Id == "SampleParameter_ListView_ReportingValidation_QueryPanel"
                    || View.Id == "SampleParameter_ListView_ReportingView_QueryPanel" || View.Id == "Reporting_ListView_ValidationQueryPanel"
                    || View.Id == "Reporting_ListView_ApprovalQueryPanel" || View.Id == "Reporting_ListView_ReportViewQueryPanel")
                {
                    if (!string.IsNullOrEmpty(objRQPInfo.ReportingQueryFilter))
                    {
                        objRQPInfo.ReportingQueryFilter = objRQPInfo.ReportingQueryFilter + "AND [JobID.JobID] IN ('" + string.Join(",", objRQPInfo.lstJobID) + "')";
                    }
                    else
                    {
                        objRQPInfo.ReportingQueryFilter = "[JobID.JobID] IN ('" + string.Join(",", objRQPInfo.lstJobID) + "')";
                    }

                    if (objRQPInfo.rgFilterByMonthDate.Year != 001)
                    {
                        objRQPInfo.ReportingQueryFilter = objRQPInfo.ReportingQueryFilter + "AND [ReportedDate] BETWEEN('" + objRQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')";
                    }

                    CollectionSource cs = new CollectionSource(Application.CreateObjectSpace(), typeof(Modules.BusinessObjects.SampleManagement.Reporting));
                    cs.Criteria["filter"] = CriteriaOperator.Parse(objRQPInfo.ReportingQueryFilter);

                    if (View.Id == "SampleParameter_ListView_ReportingApproval_QueryPanel" || View.Id == "Reporting_ListView_ApprovalQueryPanel")
                    {
                        ListView lstView = Application.CreateListView("Reporting_ListView_Copy_ReportApproval", cs, false);
                        //Frame.SetView(lstView);
                        e.InnerArgs.ShowViewParameters.CreatedView = lstView;
                    }
                    else
                    if (View.Id == "SampleParameter_ListView_ReportingValidation_QueryPanel" || View.Id == "Reporting_ListView_ValidationQueryPanel")
                    {
                        ListView lstView = Application.CreateListView("Reporting_ListView", cs, false);
                        //Frame.SetView(lstView);
                        e.InnerArgs.ShowViewParameters.CreatedView = lstView;
                    }
                    else
                    if (View.Id == "SampleParameter_ListView_ReportingView_QueryPanel" || View.Id == "Reporting_ListView_ReportViewQueryPanel")
                    {
                        ListView lstView = Application.CreateListView("Reporting_ListView_Copy_ReportView", cs, false);
                        //Frame.SetView(lstView);
                        e.InnerArgs.ShowViewParameters.CreatedView = lstView;
                    }
                    e.Handled = true;
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
                if (View.Id == "SampleParameter_ListView_ReportingApproval_QueryPanel" || View.Id == "SampleParameter_ListView_ReportingValidation_QueryPanel"
                    || View.Id == "SampleParameter_ListView_ReportingView_QueryPanel" || View.Id == "Reporting_ListView_ValidationQueryPanel"
                    || View.Id == "Reporting_ListView_ApprovalQueryPanel" || View.Id == "Reporting_ListView_ReportViewQueryPanel")
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    if (gridView != null)
                    {
                        GridViewCommandColumn selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.Visible = false;
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

        #region PopupEvents

        #endregion

        #region Events
        private void objectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "FilterDataByMonth")
                {
                    if (View.ObjectTypeInfo.Type == typeof(ReportingQueryPanel))
                    {
                        ReportingQueryPanel REQPanel = (ReportingQueryPanel)e.Object;
                        if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._1M))
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-1);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._3M))
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-3);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._6M))
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddMonths(-6);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN._1Y))
                        {
                            objRQPInfo.rgFilterByMonthDate = DateTime.Today.AddYears(-1);
                        }
                        else if (REQPanel.FilterDataByMonth.Equals(FilterByMonthEN.All))
                        {
                            objRQPInfo.rgFilterByMonthDate = new DateTime();
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
        private void ReportingQueryPanelViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Reporting_ListView")//Report Validation View
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportStatus] <> ##Enum#Modules.BusinessObjects.Hr.ReportStatus,Rollbacked# AND [ReportValidatedDate] IS NULL AND [ReportValidatedBy] IS NULL");
                    //if (!string.IsNullOrEmpty(objRQPInfo.ReportingQueryFilter))
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(objRQPInfo.ReportingQueryFilter + " AND [ReportValidatedDate] IS NULL AND[ReportValidatedBy] IS NULL");
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[ReportValidatedDate] IS NULL AND[ReportValidatedBy] IS NULL");
                    //}
                }
                else if (View != null && View.Id == "Reporting_ListView_Copy_ReportApproval")
                {
                    //if (!string.IsNullOrEmpty(objRQPInfo.ReportingQueryFilter))
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse(objRQPInfo.ReportingQueryFilter + " AND [ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                    //}
                    //else
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse("[ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                    //}
                    ((ListView)View).CollectionSource.Criteria["filter1"] = CriteriaOperator.Parse("[ReportStatus] <> ##Enum#Modules.BusinessObjects.Hr.ReportStatus,Rollbacked# AND [ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                }
                ApplyCriteriaForReportingQueryPanel();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Method
        private void ApplyCriteriaForReportingQueryPanel()
        {
            try
            {
                if (View != null && View.Id == "SampleParameter_ListView_ReportingValidation_QueryPanel" || View.Id == "SampleParameter_ListView_ReportingApproval_QueryPanel"
                    || View.Id == "SampleParameter_ListView_ReportingView_QueryPanel")
                {
                    if (objRQPInfo.CurrentViewID == "Reporting_ListView")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ReportedDate] IS NOT NULL AND [ReportValidatedDate] IS NULL AND [ReportValidatedBy] IS NULL");
                            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "JobID.JobID", true, true));
                            List<object> jobid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                jobid.Add(rec["JobID"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("JobID", jobid);
                            ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[ReportedDate] BETWEEN('" + objRQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')");

                        }
                    }
                    else if (objRQPInfo.CurrentViewID == "Reporting_ListView_Copy_ReportApproval")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ReportedDate] IS NOT NULL AND [ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "JobID.JobID", true, true));
                            List<object> jobid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                jobid.Add(rec["JobID"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("JobID", jobid);
                        }
                    }
                }
                else if (View != null && View.Id == "Project_LookupListView")
                {
                    if (objRQPInfo.CurrentViewID == "Reporting_ListView")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ReportedDate] IS NOT NULL AND [ReportValidatedDate] IS NULL AND [ReportValidatedBy] IS NULL");
                            lstview.Properties.Add(new ViewProperty("ProjectId", SortDirection.Ascending, "JobID.ProjectID.ProjectId", true, true));
                            List<object> lstProjectId = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstProjectId.Add(rec["ProjectId"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("ProjectId", lstProjectId);
                        }
                    }
                    else if (objRQPInfo.CurrentViewID == "Reporting_ListView_Copy_ReportApproval")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ReportedDate] IS NOT NULL AND [ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                            lstview.Properties.Add(new ViewProperty("ProjectId", SortDirection.Ascending, "JobID.ProjectID.ProjectId", true, true));
                            List<object> lstProjectId = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstProjectId.Add(rec["ProjectId"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("ProjectId", lstProjectId);
                        }
                    }
                }
                else if (View != null && View.Id == "Customer_LookupListView")
                {
                    if (objRQPInfo.CurrentViewID == "Reporting_ListView")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ReportedDate] IS NOT NULL AND [ReportValidatedDate] IS NULL AND [ReportValidatedBy] IS NULL");
                            lstview.Properties.Add(new ViewProperty("CustomerName", SortDirection.Ascending, "JobID.ClientName.CustomerName", true, true));
                            List<object> lstCustomerName = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstCustomerName.Add(rec["CustomerName"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("CustomerName", lstCustomerName);
                        }
                    }
                    else if (objRQPInfo.CurrentViewID == "Reporting_ListView_Copy_ReportApproval")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ReportedDate] IS NOT NULL AND [ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                            lstview.Properties.Add(new ViewProperty("CustomerName", SortDirection.Ascending, "JobID.ClientName.CustomerName", true, true));
                            List<object> lstCustomerName = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstCustomerName.Add(rec["CustomerName"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("CustomerName", lstCustomerName);
                        }
                    }
                }
                else if (View != null && View.Id == "Reporting_LookupListView")
                {
                    if (objRQPInfo.CurrentViewID == "Reporting_ListView")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ReportedDate] IS NOT NULL AND [ReportValidatedDate] IS NULL AND [ReportValidatedBy] IS NULL");
                            lstview.Properties.Add(new ViewProperty("ReportID", SortDirection.Ascending, "ReportID", true, true));
                            List<object> lstReportID = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstReportID.Add(rec["ReportID"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("ReportID", lstReportID);
                        }
                    }
                    else if (objRQPInfo.CurrentViewID == "Reporting_ListView_Copy_ReportApproval")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(Modules.BusinessObjects.SampleManagement.Reporting)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ReportedDate] IS NOT NULL AND [ReportValidatedDate] IS NOT NULL AND [ReportValidatedBy] IS NOT NULL AND [ReportApprovedDate] IS NULL AND [ReportApprovedBy] IS NULL");
                            lstview.Properties.Add(new ViewProperty("ReportID", SortDirection.Ascending, "ReportID", true, true));
                            List<object> lstReportID = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstReportID.Add(rec["ReportID"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("ReportID", lstReportID);
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

    }
}
