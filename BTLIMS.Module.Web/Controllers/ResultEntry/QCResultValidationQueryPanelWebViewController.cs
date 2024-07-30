using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace LDM.Module.Web.Controllers.ResultEntry
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class QCResultValidationQueryPanelWebViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        bool QueryPanel = false;
        ShowNavigationItemController ShowNavigationController;
        QCResultValidationQueryPanelInfo objQPInfo = new QCResultValidationQueryPanelInfo();
        ASPxPageControl pageControl = null;
        XafCallbackManager callbackManager;
        public QCResultValidationQueryPanelWebViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "Result_Validation;" + "SpreadSheetEntry_ListView_Copy_Sample;" + "SpreadSheetEntry_ListView_Copy_QC;" + "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel;" +
                "TestMethod_LookupListView_Copy_QCResultValidationQueryPanel;" + "SpreadSheetEntry_ListView_Copy_Sample_ResultApproval;" + "SpreadSheetEntry_ListView_Copy_QC_ResultApproval;"
                + "Result_Approval;" + "Result_View;" + "SpreadSheetEntry_ListView_Copy_Sample_ResultView;" + "SpreadSheetEntry_ListView_Copy_QC_ResultView;" + "Result_View;" + "QCResultValidationQueryPanel_DetailView;"
                + "SampleLogIn_LookupListView_SampleId;" + "SpreadSheetEntry_ListView_QCBatchQueryPanel;" + "QCResultValidationQueryPanel_DetailView_ResultValidation;"
                + "QCResultValidationQueryPanel_DetailView_ResultApproval;" + "ResultValidationQueryPanel_DetailView_ResultValidation;" + "ResultValidationQueryPanel_DetailView_ResultApproval;"
                + "Samplecheckin_ListView_ResultValidation;" + "Samplecheckin_ListView_ResultApproval;";
            //+ "SpreadSheetEntry_LookupListView_SampleID;"
            QCResultValidation.TargetViewId = "Result_Validation;" + "SpreadSheet;";
            //QCResultValidationQueryPanel.TargetViewId = "Result_View;"; + "Result_Validation;" + "Result_Approval;";
            QCResultApproval.TargetViewId = "Result_Approval";
            OpenDataReviewJobId.TargetViewId = "QCResultValidationQueryPanel_DetailView_ResultValidation;" + "QCResultValidationQueryPanel_DetailView_ResultApproval;";
            Level1ResultViewHistory.TargetViewId = "ResultValidationQueryPanel_DetailView_ResultValidation;" + "Samplecheckin_ListView_ResultValidation;";
            Level2ResultViewHistory.TargetViewId = "ResultValidationQueryPanel_DetailView_ResultApproval;" + "Samplecheckin_ListView_ResultApproval;";

        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                if (View.Id == "QCResultValidationQueryPanel_DetailView" || View.Id == "QCResultValidationQueryPanel_DetailView_ResultValidation"
                    || View.Id == "QCResultValidationQueryPanel_DetailView_ResultApproval")
                {
                    objQPInfo.lstJobID = new List<string>();
                    objQPInfo.rgFilterByMonthDate = DateTime.Now;
                    objQPInfo.rgFilterByMonthDate = objQPInfo.rgFilterByMonthDate.AddMonths(-1);
                    if (View.Id == "QCResultValidationQueryPanel_DetailView_ResultValidation" || View.Id == "QCResultValidationQueryPanel_DetailView_ResultApproval"
                         || View.Id == "ResultValidationQueryPanel_DetailView_ResultApproval_View"
                    || View.Id == "ResultValidationQueryPanel_DetailView_ResultValidation_View")
                    {
                        DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                        if (viQC != null)
                        {
                            viQC.ControlCreated += ViQC_ControlCreated;
                        }
                        DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                        if (viJob != null)
                        {
                            viJob.ControlCreated += ViJob_ControlCreated;
                        }
                        DashboardViewItem viABID = (DashboardViewItem)((DetailView)View).FindItem("viewItemABID");
                        if (viABID != null)
                        {
                            viABID.ControlCreated += ViABID_ControlCreated;
                        }
                    }
                }
                else if (View.Id == "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel")
                {
                    View.SelectionChanged += View_SelectionChanged;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ViJob_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DashboardViewItem vi = (DashboardViewItem)sender;
                    if (vi != null && vi.Control != null)
                    {
                        if (objQPInfo.SelectMode == QueryMode.Job)
                        {
                            ((Control)vi.Control).Visible = true;
                        }
                        //else if (objQPInfo.SelectMode == QueryMode.QC)
                        //{
                        //    ((Control)vi.Control).Visible = false;
                        //}
                        else if (objQPInfo.SelectMode == QueryMode.ABID)
                        {
                            ((Control)vi.Control).Visible = false;
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

        private void ViQC_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DashboardViewItem vi = (DashboardViewItem)sender;
                    if (vi != null && vi.Control != null)
                    {
                        if (objQPInfo.IsQueryPanelOpened == true || objQPInfo.SelectMode == QueryMode.Job)
                        {
                            objQPInfo.IsQueryPanelOpened = false;
                            ((Control)vi.Control).Visible = false;
                        }
                        //else if (objQPInfo.SelectMode == QueryMode.QC)
                        //{
                        //    ((Control)vi.Control).Visible = true;
                        //}
                        else if (objQPInfo.SelectMode == QueryMode.ABID)
                        {
                            ((Control)vi.Control).Visible = false;
                        }
                    }
                    //View.Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ViABID_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DashboardViewItem vi = (DashboardViewItem)sender;
                    if (vi != null && vi.Control != null)
                    {
                        if (objQPInfo.SelectMode == QueryMode.Job)
                        {
                            ((Control)vi.Control).Visible = false;
                        }
                        //else if (objQPInfo.SelectMode == QueryMode.QC)
                        //{
                        //    ((Control)vi.Control).Visible = false;
                        //}
                        else if (objQPInfo.SelectMode == QueryMode.ABID)
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
        private void View_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel")
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
                if (View != null && View.CurrentObject == e.Object && e.PropertyName == "FilterDataByMonth")
                {
                    objQPInfo.rgFilterByMonthDate = DateTime.Now;
                    if (View.ObjectTypeInfo.Type == typeof(QCResultValidationQueryPanel))
                    {
                        QCResultValidationQueryPanel REQPanel = (QCResultValidationQueryPanel)e.Object;
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
                            objQPInfo.rgFilterByMonthDate = DateTime.MinValue;
                        }
                    }
                }
                else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "SelectionMode")
                {
                    QCResultValidationQueryPanel queryPanel = (QCResultValidationQueryPanel)e.Object;
                    if (queryPanel != null)
                    {
                        DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                        DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                        DashboardViewItem viABID = (DashboardViewItem)((DetailView)View).FindItem("viewItemABID");
                        if (queryPanel.SelectionMode == QueryMode.Job)
                        {
                            objQPInfo.SelectMode = QueryMode.Job;
                            if (viJob != null)
                            {
                                ((Control)viJob.Control).Visible = true;
                            }
                            if (viQC != null)
                            {
                                ((Control)viQC.Control).Visible = false;
                            }
                        }
                        //else if (queryPanel.SelectionMode == QueryMode.QC)
                        //{
                        //    objQPInfo.SelectMode = QueryMode.QC;
                        //    if (viJob != null)
                        //    {
                        //        ((Control)viJob.Control).Visible = false;
                        //    }
                        //    if (viQC != null)
                        //    {
                        //        ((Control)viQC.Control).Visible = true;
                        //    }
                        //}
                        else if (queryPanel.SelectionMode == QueryMode.ABID)
                        {
                            objQPInfo.SelectMode = QueryMode.ABID;
                            if (viJob != null)
                            {
                                ((Control)viJob.Control).Visible = false;
                            }
                            if (viQC != null)
                            {
                                ((Control)viQC.Control).Visible = false;
                            }
                            if (viABID != null)
                            {
                                ((Control)viABID.Control).Visible = true;
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

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            try
            {
                if (View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (View.Id == "SpreadSheetEntry_ListView_Copy_Sample" || View.Id == "SpreadSheetEntry_ListView_Copy_QC"
                       || View.Id == "SpreadSheetEntry_ListView_Copy_Sample_ResultApproval" || View.Id == "SpreadSheetEntry_ListView_Copy_QC_ResultApproval")
                    {
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                            gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                            gridListEditor.Grid.Load += gridView_Load;
                            gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        }
                    }
                    else if (View.Id == "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel"
                        || View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultValidation"
                        || View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultApproval")
                    {
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            ASPxGridView gridView = gridListEditor.Grid;
                            if (gridView != null)
                            {
                                //gridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared;
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

        private void GridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel"
                        || View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultValidation"
                        || View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultApproval")
                {
                    callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("CustomProcessCurrentObjectController", this);
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
            try
            {
                if (View.Id == "SpreadSheetEntry_ListView_Copy_Sample" || View.Id == "SpreadSheetEntry_ListView_Copy_QC"
                   || View.Id == "SpreadSheetEntry_ListView_Copy_Sample_ResultApproval" || View.Id == "SpreadSheetEntry_ListView_Copy_QC_ResultApproval")
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    if (gridView != null)
                    {
                        gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                        gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                        if (View.Id == "SpreadSheetEntry_ListView_Copy_Sample" || View.Id == "SpreadSheetEntry_ListView_Copy_Sample_ResultApproval")
                        {
                            gridView.VisibleColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;
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

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ASPxGridView gv = gridListEditor.Grid;
                IObjectSpace os = this.ObjectSpace;
                Session CS = ((XPObjectSpace)(os)).Session;
                var selected = gridListEditor.GetSelectedObjects();// View.SelectedObjects;
                if (View.Id == "SpreadSheetEntry_ListView_Copy_Sample")
                {
                    foreach (SpreadSheetEntry objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            objSampleResult.ValidatedDate = DateTime.Now;
                            objSampleResult.ValidatedBy = CS.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            if (objSampleResult.uqSampleParameterID != null)
                            {
                                objSampleResult.uqSampleParameterID.ValidatedDate = DateTime.Now;
                                objSampleResult.uqSampleParameterID.ValidatedBy = CS.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            }
                        }
                        else
                        {
                            objSampleResult.ValidatedDate = null;
                            objSampleResult.ValidatedBy = null;
                            if (objSampleResult.uqSampleParameterID != null)
                            {
                                objSampleResult.uqSampleParameterID.ValidatedDate = null;
                                objSampleResult.uqSampleParameterID.ValidatedBy = null;
                            }
                        }
                    }
                    View.Refresh();
                }
                else if (View.Id == "SpreadSheetEntry_ListView_Copy_QC")
                {
                    foreach (SpreadSheetEntry objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            objSampleResult.ValidatedDate = DateTime.Now;
                            objSampleResult.ValidatedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objSampleResult.ValidatedDate = null;
                            objSampleResult.ValidatedBy = null;
                        }
                    }
                    View.Refresh();
                }
                if (View.Id == "SpreadSheetEntry_ListView_Copy_Sample_ResultApproval")
                {
                    foreach (SpreadSheetEntry objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            objSampleResult.ApprovedDate = DateTime.Now;
                            objSampleResult.ApprovedBy = CS.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            if (objSampleResult.uqSampleParameterID != null)
                            {
                                objSampleResult.uqSampleParameterID.ApprovedDate = DateTime.Now;
                                objSampleResult.uqSampleParameterID.ApprovedBy = CS.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            }
                        }
                        else
                        {
                            objSampleResult.ApprovedDate = null;
                            objSampleResult.ApprovedBy = null;
                            if (objSampleResult.uqSampleParameterID != null)
                            {
                                objSampleResult.uqSampleParameterID.ApprovedDate = null;
                                objSampleResult.uqSampleParameterID.ApprovedBy = null;
                            }
                        }
                    }
                    View.Refresh();
                }
                else if (View.Id == "SpreadSheetEntry_ListView_Copy_QC_ResultApproval")
                {
                    foreach (SpreadSheetEntry objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            objSampleResult.ApprovedDate = DateTime.Now;
                            objSampleResult.ApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objSampleResult.ApprovedDate = null;
                            objSampleResult.ApprovedBy = null;
                        }
                    }
                    View.Refresh();
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            //if(View is DashboardView)
            //((WebLayoutManager)((DashboardView)View).LayoutManager).PageControlCreated -= OnPageControlCreated;
        }
        //private void QCResultValidationQueryPanel_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        //{
        //    try
        //    {
        //        objQPInfo.CurrentViewID = View.Id;
        //        IObjectSpace objspace = Application.CreateObjectSpace();
        //        object objToShow = objspace.CreateObject(typeof(QCResultValidationQueryPanel));
        //        if (objToShow != null)
        //        {
        //            string strDetailViewID = string.Empty;
        //            if (View.Id == "Result_Validation")
        //            {
        //                strDetailViewID = "QCResultValidationQueryPanel_DetailView_ResultValidation";
        //            }
        //            else if (View.Id == "Result_Approval")
        //            {
        //                strDetailViewID = "QCResultValidationQueryPanel_DetailView_ResultApproval";
        //            }
        //            DetailView CreateDetailView = null;
        //            if (string.IsNullOrEmpty(strDetailViewID))
        //            {
        //                CreateDetailView = Application.CreateDetailView(objspace, objToShow);
        //            }
        //            else
        //            {
        //                CreateDetailView = Application.CreateDetailView(objspace, strDetailViewID, false, objToShow);
        //            }
        //            CreateDetailView.ViewEditMode = ViewEditMode.Edit;
        //            //e.Size = new Size(500, 500);
        //            e.DialogController.SaveOnAccept = true;
        //            e.DialogController.Accepting += DialogController_Accepting;
        //            e.DialogController.Cancelling += DialogController_Cancelling;
        //            e.View = CreateDetailView;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}

        private void DialogController_Cancelling(object sender, EventArgs e)
        {
            try
            {
                objQPInfo.SampleResultValidationQueryFilter = string.Empty;
                objQPInfo.lstJobID = new List<string>();
                objQPInfo.lstSampleID = new List<string>();
                objQPInfo.lstSampleOid = new List<Guid>();
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
                string Filter = string.Empty;
                QueryPanel = true;
                objQPInfo.SampleResultValidationQueryFilter = string.Empty;
                string strCriteria = string.Empty;

                if (objQPInfo.lstJobID != null && objQPInfo.lstJobID.Count > 0)
                {
                    List<object> ABID = new List<object>();
                    using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SpreadSheetEntry)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[uqSampleJobID.JobID] in ('" + string.Join("','", objQPInfo.lstJobID) + "')");
                        lstview.Properties.Add(new ViewProperty("TuqAnalyticalBatchID", SortDirection.Ascending, "uqAnalyticalBatchID.AnalyticalBatchID", true, true));
                        foreach (ViewRecord rec in lstview)
                            ABID.Add(rec["TuqAnalyticalBatchID"]);
                    }
                    Filter = string.Join(",", ABID);
                }
                if (!string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                {
                    objQPInfo.SampleResultValidationQueryFilter = objQPInfo.SampleResultValidationQueryFilter + "AND [uqAnalyticalBatchID.AnalyticalBatchID] in('" + string.Join("','", Filter) + "')";
                    if (objQPInfo.rgFilterByMonthDate.Date != DateTime.Now.Date)
                    {
                        //objQPInfo.SampleResultValidationQueryFilter = objQPInfo.SampleResultValidationQueryFilter + "AND [uqSampleJobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate.Date + "', '" + DateTime.Now.Date + "')";
                    }
                }
                else
                {
                    objQPInfo.SampleResultValidationQueryFilter = "[uqAnalyticalBatchID.AnalyticalBatchID] in('" + string.Join("','", Filter) + "')";
                    if (objQPInfo.rgFilterByMonthDate.Date != DateTime.Now.Date)
                    {
                        //objQPInfo.SampleResultValidationQueryFilter = objQPInfo.SampleResultValidationQueryFilter + "AND [uqSampleJobID.RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate.Date + "', '" + DateTime.Now.Date + "')";
                    }
                }


            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void QCResultValidationQueryPanelWebViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SpreadSheetEntry_ListView_Copy_Sample" || View.Id == "Sample") //"SampleParameter_ListView_Copy_ResultValidation_Result_DV")
                {
                    if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status]='PendingValidation' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]=True or [UQTESTPARAMETERID.InternalStandard]=True)");
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status]='PendingValidation' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]=1 or [UQTESTPARAMETERID.InternalStandard]=1)");// AND [Status]='PendingEntry'"); 
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status]='Pending Validation' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null)");                      
                    }
                    else
                    {
                        //CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [Status]='PendingValidation' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]=1 or [UQTESTPARAMETERID.InternalStandard]=1)");
                        CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [Status]='PendingValidation' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]=True or [UQTESTPARAMETERID.InternalStandard]=True)");
                        //CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [Status]='Pending Validation' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null)");
                        //((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["filter"] = criteria;
                    }
                }
                else if (View != null && View.Id == "SpreadSheetEntry_ListView_Copy_QC" || View.Id == "QC")
                {
                    if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter) && QueryPanel == false)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[RunNo] =1 AND [SystemID] <> 'SAMPLE' AND [Status]='PendingValidation'");
                    }
                    else if (!string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [RunNo] =1 AND [SystemID] <> 'SAMPLE' AND [Status]='PendingValidation'");
                        ((ListView)View).CollectionSource.Criteria["filter"] = criteria;
                    }
                }
                if (View != null && View.Id == "SpreadSheetEntry_ListView_Copy_Sample_ResultApproval") //"SampleParameter_ListView_Copy_ResultValidation_Result_DV")
                {
                    if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status]='PendingApproval' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]='1' or [UQTESTPARAMETERID.InternalStandard]='1')");
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Status]='Pending Approval' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null)");                   
                    }
                    else if (!string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [Status]='PendingApproval' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]='1' or [UQTESTPARAMETERID.InternalStandard]='1')");
                        //CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [Status]='Pending Approval' AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null)");
                        ((ListView)View).CollectionSource.Criteria["filter"] = criteria;
                    }
                }
                else if (View != null && View.Id == "SpreadSheetEntry_ListView_Copy_QC_ResultApproval")
                {
                    if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter) && QueryPanel == false)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[RunNo] =1 AND [SystemID] <> 'SAMPLE' AND [Status]='PendingApproval'");
                    }
                    else if (!string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [RunNo] =1 AND [SystemID] <> 'SAMPLE' AND [Status]='PendingApproval'");
                        ((ListView)View).CollectionSource.Criteria["filter"] = criteria;
                    }
                }
                if (View != null && View.Id == "SpreadSheetEntry_ListView_Copy_Sample_ResultView") //"SampleParameter_ListView_Copy_ResultValidation_Result_DV")
                {
                    if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]='1' or [UQTESTPARAMETERID.InternalStandard]='1')");// AND [Status]='PendingEntry'"); 
                        //((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null)");                                            
                    }
                    else if (!string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null Or [UQTESTPARAMETERID.Surroagate]='1' or [UQTESTPARAMETERID.InternalStandard]='1')");
                        //CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [RunNo] =1 AND [SystemID] = 'SAMPLE' AND (uqSampleParameterID is not null)");
                        ((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["filter"] = criteria;
                        if (objQPInfo.lstSampleID.Count > 0)
                        {
                            ((ListView)View).CollectionSource.Criteria["filter1"] = new InOperator("SampleID", objQPInfo.lstSampleID);
                        }
                    }
                }
                else if (View != null && View.Id == "SpreadSheetEntry_ListView_Copy_QC_ResultView")
                {
                    if (string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter) && QueryPanel == false)
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[RunNo] =1 AND [SystemID] <> 'SAMPLE'");
                    }
                    else if (!string.IsNullOrEmpty(objQPInfo.SampleResultValidationQueryFilter))
                    {
                        CriteriaOperator criteria = CriteriaOperator.Parse("" + objQPInfo.SampleResultValidationQueryFilter + "AND [RunNo] =1 AND [SystemID] <> 'SAMPLE'");
                        ((ListView)View).CollectionSource.Criteria.Clear();
                        ((ListView)View).CollectionSource.Criteria["filter"] = criteria;
                    }
                }

                ApplyCriteriaForQueryPanel();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);

            }

        }
        private void ApplyCriteriaForQueryPanel()
        {
            try
            {
                if (View != null && View.Id == "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel")
                {
                    if (objQPInfo.CurrentViewID == "Result_Validation")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingValidation'");// AND [ABID] IS NOT NULL");
                            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                            List<object> jobid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                jobid.Add(rec["JobID"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("JobID", jobid);
                        }
                    }
                    if (objQPInfo.CurrentViewID == "Result_Approval")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingApproval' AND [ABID] IS NOT NULL");
                            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                            List<object> jobid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                jobid.Add(rec["JobID"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("JobID", jobid);
                        }
                    }
                    if (objQPInfo.CurrentViewID == "Result_View")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ABID] IS NOT NULL");
                            lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                            List<object> jobid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                jobid.Add(rec["JobID"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("JobID", jobid);
                            if (objQPInfo.rgFilterByMonthDate != DateTime.MinValue)
                                ((ListView)View).CollectionSource.Criteria["Distinct1"] = CriteriaOperator.Parse("[RecievedDate] BETWEEN('" + objQPInfo.rgFilterByMonthDate + "', '" + DateTime.Now + "')");
                        }
                    }
                }
                else if (View != null && View.Id == "TestMethod_LookupListView_Copy_QCResultValidationQueryPanel")
                {
                    if (objQPInfo.CurrentViewID == "Result_Validation")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingValidation' AND [ABID] IS NOT NULL");
                            lstview.Properties.Add(new ViewProperty("TestName", SortDirection.Ascending, "Testparameter.TestMethod.TestName", true, true));
                            List<object> lstTestName = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstTestName.Add(rec["TestName"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("TestName", lstTestName);
                        }
                    }
                    if (objQPInfo.CurrentViewID == "Result_Approval")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingApproval' AND [ABID] IS NOT NULL");
                            lstview.Properties.Add(new ViewProperty("TestName", SortDirection.Ascending, "Testparameter.TestMethod.TestName", true, true));
                            List<object> lstTestName = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstTestName.Add(rec["TestName"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("TestName", lstTestName);
                        }
                    }
                    if (objQPInfo.CurrentViewID == "Result_View")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[ABID] IS NOT NULL");
                            lstview.Properties.Add(new ViewProperty("TestName", SortDirection.Ascending, "Testparameter.TestMethod.TestName", true, true));
                            List<object> lstTestName = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                lstTestName.Add(rec["TestName"]);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("TestName", lstTestName);
                        }
                    }
                }
                else if (View != null && View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel")
                {
                    if (objQPInfo.CurrentViewID == "Result_Validation")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SpreadSheetEntry)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingValidation'");// AND [ABID] IS NOT NULL");
                            lstview.Properties.Add(new ViewProperty("QCBatchID", SortDirection.Ascending, "uqSampleParameterID.QCBatchID.qcseqdetail.QCBatchID", true, true));
                            lstview.Properties.Add(new ViewProperty("fOid", SortDirection.Ascending, "MAX(uqSampleParameterID.Oid)", false, true));
                            //lstview.Properties.Add(new ViewProperty("QCBatchID", SortDirection.Ascending, "uqQCBatchID.QCBatchID", true, true));
                            List<object> Oid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                Oid.Add(rec["fOid"]);
                            //((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("uqQCBatchID.QCBatchID", qcbatchid);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("uqSampleParameterID.Oid", Oid);
                        }
                    }
                    if (objQPInfo.CurrentViewID == "Result_Approval")
                    {
                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SpreadSheetEntry)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[Status]='PendingApproval'");// AND [ABID] IS NOT NULL");
                            lstview.Properties.Add(new ViewProperty("QCBatchID", SortDirection.Ascending, "uqSampleParameterID.QCBatchID.qcseqdetail.QCBatchID", true, true));
                            lstview.Properties.Add(new ViewProperty("fOid", SortDirection.Ascending, "MAX(uqSampleParameterID.Oid)", false, true));
                            //lstview.Properties.Add(new ViewProperty("QCBatchID", SortDirection.Ascending, "uqQCBatchID.QCBatchID", true, true));
                            List<object> Oid = new List<object>();
                            foreach (ViewRecord rec in lstview)
                                Oid.Add(rec["fOid"]);
                            //((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("uqQCBatchID.QCBatchID", qcbatchid);
                            ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("uqSampleParameterID.Oid", Oid);
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

        private void QCResultValidation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Result_Validation")
                {
                    DashboardViewItem dvSample = ((DashboardView)View).FindItem("Sample") as DashboardViewItem;
                    DashboardViewItem dvQC = ((DashboardView)View).FindItem("QC") as DashboardViewItem;
                    if (dvSample.InnerView != null)
                    {
                        if (dvSample.InnerView.SelectedObjects.Count > 0)
                        {
                            IObjectSpace os = dvSample.InnerView.ObjectSpace;
                            DefaultSetting defaultSetting = os.FindObject<DefaultSetting>(CriteriaOperator.Parse(""));
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                            Session CS = ((XPObjectSpace)(os)).Session;
                            ((ASPxGridListEditor)((ListView)dvSample.InnerView).Editor).Grid.UpdateEdit();
                            foreach (SpreadSheetEntry objSampleResult in dvSample.InnerView.SelectedObjects)
                            {
                                if (defaultSetting.REApprove == EnumRELevelSetup.No)
                                {
                                    objSampleResult.Status = Samplestatus.Approved; //"Approved";
                                    if (objSampleResult.uqSampleParameterID != null)
                                    {
                                        objSampleResult.uqSampleParameterID.Status = Samplestatus.PendingReporting; //"Pending Reporting";
                                        objSampleResult.uqSampleParameterID.OSSync = true; //"Pending Reporting";
                                    }
                                }
                                else
                                {
                                    objSampleResult.Status = Samplestatus.PendingApproval;//"Pending Approval";
                                    if (objSampleResult.uqSampleParameterID != null)
                                    {
                                        objSampleResult.uqSampleParameterID.Status = Samplestatus.PendingApproval; //"Pending Approval";
                                        objSampleResult.uqSampleParameterID.OSSync = true; //"Pending Reporting";
                                    }
                                }
                                //os.CommitChanges();
                            }
                            os.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultvalidate"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            os.Refresh();
                        }
                    }
                    if (dvQC.InnerView != null)
                    {
                        if (dvQC.InnerView.SelectedObjects.Count > 0)
                        {
                            IObjectSpace os = dvQC.InnerView.ObjectSpace;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                            Session CS = ((XPObjectSpace)(os)).Session;
                            ((ASPxGridListEditor)((ListView)dvQC.InnerView).Editor).Grid.UpdateEdit();
                            foreach (SpreadSheetEntry objSampleResult in dvQC.InnerView.SelectedObjects)
                            {
                                objSampleResult.Status = Samplestatus.PendingApproval;//"Pending Approval";
                            }
                            os.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultvalidate"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            os.Refresh();
                        }
                    }
                    ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    {
                        if (parent.Id == "Data Review")
                        {
                            foreach (ChoiceActionItem child in parent.Items)
                            {
                                if (child.Id == "Result Validation")
                                {
                                    int count = 0;
                                    IObjectSpace objSpace = Application.CreateObjectSpace();
                                    using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                    {
                                        lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingValidation' And[SignOff] = True And([QCBatchID] Is Null Or[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] = 'Sample')");
                                        lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                        List<object> jobid = new List<object>();
                                        if (lstview != null)
                                        {
                                            foreach (ViewRecord rec in lstview)
                                                jobid.Add(rec["Toid"]);
                                        }

                                        count = jobid.Count;
                                    }
                                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    if (count > 0)
                                    {
                                        child.Caption = cap[0] + " (" + count + ")";
                                    }
                                    else
                                    {
                                        child.Caption = cap[0];
                                    }
                                }
                                else if (child.Id == "Result Approval")
                                {
                                    int count = 0;
                                    IObjectSpace objSpace = Application.CreateObjectSpace();
                                    using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                    {
                                        lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingApproval' And[SignOff] = True And([QCBatchID] Is Null Or[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] = 'Sample')");
                                        lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                        lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                        List<object> jobid = new List<object>();
                                        if (lstview != null)
                                        {
                                            foreach (ViewRecord rec in lstview)
                                                jobid.Add(rec["Toid"]);
                                        }

                                        count = jobid.Count;
                                    }
                                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                    if (count > 0)
                                    {
                                        child.Caption = cap[0] + " (" + count + ")";
                                    }
                                    else
                                    {
                                        child.Caption = cap[0];
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

        private void ResultApproval_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "Result_Approval")
                {
                    DashboardViewItem dvSample = ((DashboardView)View).FindItem("Sample") as DashboardViewItem;
                    DashboardViewItem dvQC = ((DashboardView)View).FindItem("QC") as DashboardViewItem;
                    if (dvSample.InnerView != null)
                    {
                        if (dvSample.InnerView.SelectedObjects.Count > 0)
                        {
                            IObjectSpace os = dvSample.InnerView.ObjectSpace;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                            Session CS = ((XPObjectSpace)(os)).Session;
                            ((ASPxGridListEditor)((ListView)dvSample.InnerView).Editor).Grid.UpdateEdit();
                            foreach (SpreadSheetEntry objSampleResult in dvSample.InnerView.SelectedObjects)
                            {
                                objSampleResult.Status = Samplestatus.Approved;//"Approved";
                                if (objSampleResult.uqSampleParameterID != null)
                                {
                                    objSampleResult.uqSampleParameterID.Status = Samplestatus.PendingReporting;//"Pending Reporting";
                                    objSampleResult.uqSampleParameterID.OSSync = true;
                                }
                            }
                            os.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultapprove"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            os.Refresh();
                        }
                    }
                    if (dvQC.InnerView != null)
                    {
                        if (dvQC.InnerView.SelectedObjects.Count > 0)
                        {
                            IObjectSpace os = dvQC.InnerView.ObjectSpace;
                            IObjectSpace objSpace = Application.CreateObjectSpace();
                            Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                            Session CS = ((XPObjectSpace)(os)).Session;
                            ((ASPxGridListEditor)((ListView)dvQC.InnerView).Editor).Grid.UpdateEdit();
                            foreach (SpreadSheetEntry objSampleResult in dvQC.InnerView.SelectedObjects)
                            {
                                objSampleResult.Status = Samplestatus.Approved;//"Approved";
                            }
                            os.CommitChanges();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultapprove"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            os.Refresh();
                        }
                    }
                }
                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                {
                    if (parent.Id == "Data Review")
                    {
                        foreach (ChoiceActionItem child in parent.Items)
                        {
                            if (child.Id == "Result Validation")
                            {
                                int count = 0;
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingValidation' And[SignOff] = True And([QCBatchID] Is Null Or[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] = 'Sample')");
                                    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                    List<object> jobid = new List<object>();
                                    if (lstview != null)
                                    {
                                        foreach (ViewRecord rec in lstview)
                                            jobid.Add(rec["Toid"]);
                                    }

                                    count = jobid.Count;
                                }
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (count > 0)
                                {
                                    child.Caption = cap[0] + " (" + count + ")";
                                }
                                else
                                {
                                    child.Caption = cap[0];
                                }
                            }
                            else if (child.Id == "Result Approval")
                            {
                                int count = 0;
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                using (XPView lstview = new XPView(((XPObjectSpace)objSpace).Session, typeof(SampleParameter)))
                                {
                                    lstview.Criteria = CriteriaOperator.Parse("[Status] = 'PendingApproval' And[SignOff] = True And([QCBatchID] Is Null Or[QCBatchID] Is Not Null And[QCBatchID.QCType] Is Not Null And[QCBatchID.QCType.QCTypeName] = 'Sample')");
                                    lstview.Properties.Add(new ViewProperty("JobID", SortDirection.Ascending, "Samplelogin.JobID.JobID", true, true));
                                    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                    List<object> jobid = new List<object>();
                                    if (lstview != null)
                                    {
                                        foreach (ViewRecord rec in lstview)
                                            jobid.Add(rec["Toid"]);
                                    }

                                    count = jobid.Count;
                                }
                                var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                                if (count > 0)
                                {
                                    child.Caption = cap[0] + " (" + count + ")";
                                }
                                else
                                {
                                    child.Caption = cap[0];
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

        public void ProcessAction(string parameter)
        {
            try
            {
                if (View.Id == "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel" ||
                    View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultValidation" ||
                    View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultApproval")
                {
                    OpenDataReviewJobId.DoExecute();
                }
                //if (View.Id == "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel")
                //{
                //    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                //    if (editor != null && editor.Grid != null)
                //    {
                //        object currentOid = editor.Grid.GetRowValues(int.Parse(parameter), "Oid");
                //        Samplecheckin checkin = View.ObjectSpace.GetObjectByKey<Samplecheckin>(currentOid);
                //        if (checkin != null)
                //        {

                //        }
                //    }
                //}
                //else if( View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultValidation"
                //        || View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultApproval")
                //{
                //    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                //    if (editor != null && editor.Grid != null)
                //    {
                //        object currentOid = editor.Grid.GetRowValues(int.Parse(parameter), "Oid");
                //        SpreadSheetEntry entry = View.ObjectSpace.GetObjectByKey<SpreadSheetEntry>(currentOid);
                //        if (entry != null)
                //        {

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

        private void OpenDataReviewJobId_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ListView joblistview = null;
                ListView qcTypelistview = null;
                if (View is ListView && View.Model.GetType() == typeof(Samplecheckin))
                {
                    joblistview = (ListView)View;
                }
                if (View is ListView && View.Model.GetType() == typeof(SpreadSheetEntry))
                {
                    qcTypelistview = (ListView)View;
                }
                else if (View is DetailView)
                {
                    if (View.Id == "QCResultValidationQueryPanel_DetailView_ResultValidation" || View.Id == "QCResultValidationQueryPanel_DetailView_ResultApproval")
                    {
                        DashboardViewItem viJob = (DashboardViewItem)((DetailView)View).FindItem("viewItemJobID");
                        if (viJob != null && viJob.InnerView != null && viJob.InnerView is ListView)
                        {
                            joblistview = (ListView)viJob.InnerView;
                        }
                        DashboardViewItem viQC = (DashboardViewItem)((DetailView)View).FindItem("viewItemQCBatchID");
                        if (viQC != null && viQC.InnerView != null && viQC.InnerView is ListView)
                        {
                            qcTypelistview = (ListView)viQC.InnerView;
                        }
                    }
                }

                if (joblistview != null || qcTypelistview != null)
                {
                    if (joblistview.SelectedObjects.Count > 0 || qcTypelistview.SelectedObjects.Count > 0)
                    {
                        if (joblistview != null)
                        {
                            objQPInfo.lstJobID = new List<string>();
                            foreach (Samplecheckin objsample in joblistview.SelectedObjects)
                            {
                                objQPInfo.lstJobID.Add(objsample.JobID);
                            }
                        }

                        if (qcTypelistview != null)
                        {
                            objQPInfo.lstQCBatchID = new List<string>();
                            foreach (SpreadSheetEntry objsample in qcTypelistview.SelectedObjects)
                            {
                                objQPInfo.lstQCBatchID.Add(objsample.uqAnalyticalBatchID.AnalyticalBatchID);
                            }
                        }

                        QueryPanel = true;
                        objQPInfo.QCResultValidationQueryFilter = "[uqQCBatchID.QCBatchID] IN (" + "'" + string.Join("','", objQPInfo.lstQCBatchID) + "'" + ")";
                        objQPInfo.SampleResultValidationQueryFilter = "[uqSampleJobID.JobID] IN (" + string.Join(",", objQPInfo.lstJobID) + ")";
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                        if (View.Id == "QCResultValidationQueryPanel_DetailView_ResultValidation")
                        {
                            DashboardView dv = Application.CreateDashboardView(Application.CreateObjectSpace(), "Result_Validation", false);
                            Frame.SetView(dv);
                        }
                        else if (View.Id == "QCResultValidationQueryPanel_DetailView_ResultApproval")
                        {
                            DashboardView dv = Application.CreateDashboardView(Application.CreateObjectSpace(), "Result_Approval", false);
                            Frame.SetView(dv);
                        }
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

        private void Level1ResultViewHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(Samplecheckin));
                CollectionSource cs = new CollectionSource(os, typeof(Samplecheckin));
                ListView lv = Application.CreateListView("Samplecheckin_ListView_ResultValidation_View", cs, true);
                Frame.SetView(lv);
                //QCResultValidationQueryPanel obj = os.CreateObject<QCResultValidationQueryPanel>();
                //DetailView Dvresultview = Application.CreateDetailView(os, "ResultValidationQueryPanel_DetailView_ResultValidation_View", true, obj);
                //Dvresultview.ViewEditMode = ViewEditMode.Edit;
                //Frame.SetView(Dvresultview);
                //obj.SelectionMode = objQPInfo.SelectMode;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Level2ResultViewHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                IObjectSpace os = Application.CreateObjectSpace(typeof(QCResultValidationQueryPanel));
                CollectionSource cs = new CollectionSource(os, typeof(Samplecheckin));
                ListView lv = Application.CreateListView("Samplecheckin_ListView_ResultApproval_View", cs, true);
                Frame.SetView(lv);
                //Dvresultview.ViewEditMode = ViewEditMode.Edit;
                //Frame.SetView(Dvresultview);
                //obj.SelectionMode = objQPInfo.SelectMode;

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}

