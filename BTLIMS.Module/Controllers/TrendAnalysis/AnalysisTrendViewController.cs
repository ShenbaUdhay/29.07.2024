using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Chart.Web;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.TrendAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using View = DevExpress.ExpressApp.View;
using ViewType = DevExpress.XtraCharts.ViewType;

namespace LDM.Module.Controllers.AnalysisTrend
{
    public partial class AnalysisTrendViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        TrendInfo info = new TrendInfo();
        public AnalysisTrendViewController()
        {
            InitializeComponent();
            TargetViewId = "TrendAnalysis_DetailView;" + "TrendAnalysis_ListView_Chart;" + "SampleParameter_ListView_Copy_TrendAnalysis;" + "TrendAnalysis_ListView_DateVisualization;"
                + "TrendAnalysis_DetailView_Charts;";
            ListTrend.TargetViewId = "TrendAnalysis_ListView_DateVisualization";
            RefreshScale.TargetViewId = "TrendAnalysis_DetailView_Charts;";
            AutoScaleChart.TargetViewId = "TrendAnalysis_DetailView_Charts;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "SampleParameter_ListView_Copy_TrendAnalysis")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                }
                else if (View.Id == "TrendAnalysis_ListView_Chart")
                {
                    View.ControlsCreated += View_ControlsCreated;
                    Frame.GetController<FilterController>().FullTextFilterAction.Active.SetItemValue("act", false);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                View view = Application.MainWindow.View;
                if (view.Id == "TrendAnalysis_DetailView")
                {
                    DashboardViewItem viRawData = ((DetailView)view).FindItem("RawData") as DashboardViewItem;
                    DashboardViewItem viDateVisualization = ((DetailView)view).FindItem("DateVisualization") as DashboardViewItem;
                   
                        var chartListEditor = ((ListView)View).Editor as ASPxChartListEditor;
                    if (chartListEditor != null&& info.lstSelectedObject!=null && info.lstSelectedObject.Count>0)
                    {
                        WebChartControl chartControl = chartListEditor.ChartControl;
                        if (chartControl != null)
                        {
                            if (info.actionname == "ListTrend")
                            {
                                TrendAnalysis analysis = (TrendAnalysis)viDateVisualization.InnerView.CurrentObject;
                                if (analysis != null)
                                {
                                    Series series = new Series(analysis.DVParameter, ViewType.Line);
                                    //var groupedsamples = viRawData.InnerView.SelectedObjects.Cast<SampleParameter>().Where(i => i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == analysis.DVParameter && i.AnalyzedDate != DateTime.MinValue).OrderBy(a => a.AnalyzedDate).GroupBy(a => a.AnalyzedDate).ToList();
                                    var groupedsamples = info.lstSelectedObject.Where(i => i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == analysis.DVParameter && i.AnalyzedDate != DateTime.MinValue).OrderBy(a => a.AnalyzedDate).GroupBy(a => a.AnalyzedDate).ToList();
                                    foreach (var group in groupedsamples)
                                    {
                                        series.Points.AddRange(new SeriesPoint(group.Key.Value, group.Cast<SampleParameter>().Where(a => a.ResultNumeric != null).Average(a => Convert.ToDouble(a.ResultNumeric))));
                                    }
                                    chartControl.Series.Add(series);
                                }
                            }
                            else
                            {
                                foreach (TrendAnalysis param in viDateVisualization.InnerView.SelectedObjects)
                                {
                                    Series series = new Series(param.DVParameter, ViewType.Line);
                                    //var groupedsamples = ((ListView)viRawData.InnerView).CollectionSource.List.Cast<SampleParameter>().Where(i => i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == param.DVParameter).OrderBy(a => a.AnalyzedDate).GroupBy(a => a.AnalyzedDate).ToList();
                                    var groupedsamples = info.lstSelectedObject.Where(i => i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == param.DVParameter).OrderBy(a => a.AnalyzedDate).GroupBy(a => a.AnalyzedDate).ToList();
                                    foreach (var group in groupedsamples)
                                    {
                                        series.Points.AddRange(new SeriesPoint(group.Key.Value, group.Cast<SampleParameter>().Where(a => a.ResultNumeric != null).Average(a => Convert.ToDouble(a.ResultNumeric))));
                                    }
                                    chartControl.Series.Add(series);
                                }
                            }
                        }
                        if (Frame is NestedFrame && info.Refresh)
                        {
                            view = ((NestedFrame)Frame).ViewItem.View;
                            if (view.Id == "TrendAnalysis_DetailView_Charts")
                            {
                                TrendAnalysis objTA = view.CurrentObject as TrendAnalysis;

                                if (chartControl != null && chartControl.Diagram != null)
                                {
                                    XYDiagram diagram = chartControl.Diagram as XYDiagram;
                                    if (diagram != null && objTA.ScaleSize != null)
                                    {
                                        diagram.AxisY.NumericScaleOptions.AutoGrid = false;
                                        diagram.AxisY.NumericScaleOptions.GridSpacing = objTA.ScaleSize.GetValueOrDefault();
                                    }
                                    if (diagram != null && objTA.MinimumScale != null && objTA.MaximumScale != null)
                                    {
                                        if (objTA.MinimumScale <= objTA.MaximumScale)
                                        {
                                            diagram.AxisY.WholeRange.Auto = false;
                                            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
                                            // Set limits for an Y-axis's whole range.
                                            diagram.AxisY.WholeRange.MinValue = objTA.MinimumScale;
                                            diagram.AxisY.WholeRange.MaxValue = objTA.MaximumScale;
                                            // Alternatively, you can use the SetMinMaxValues method to specify range limits.
                                            diagram.AxisY.WholeRange.SetMinMaxValues(objTA.MinimumScale, objTA.MaximumScale);

                                            // Set limits for an Y-axis's visual range.
                                            diagram.AxisY.VisualRange.MinValue = objTA.MinimumScale;
                                            diagram.AxisY.VisualRange.MaxValue = objTA.MaximumScale;
                                            // Alternatively, you can use the SetMinMaxValues method to specify range limits.
                                            diagram.AxisY.VisualRange.SetMinMaxValues(objTA.MinimumScale, objTA.MaximumScale);
                                        }
                                    }
                                    info.Refresh = false;
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

        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View.Id == "TrendAnalysis_DetailView")
                {
                    ViewItem viewItem = ((DetailView)View).FindItem("Parameter");
                    var propertyEditor = viewItem as ASPxCheckedLookupStringPropertyEditor;
                    if (propertyEditor != null && propertyEditor.ViewEditMode == ViewEditMode.Edit)
                    {
                        ASPxGridLookup lookup = (ASPxGridLookup)propertyEditor.Editor;
                        lookup.GridViewProperties.Settings.ShowFilterRow = true;
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
            try
            {
                base.OnDeactivated();
                if (View.Id == "TrendAnalysis_ListView_Chart")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                    Frame.GetController<FilterController>().FullTextFilterAction.Active.SetItemValue("act", true);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TARetrieve_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TrendAnalysis_DetailView")
                {
                    TrendAnalysis objTA = (TrendAnalysis)View.CurrentObject;
                    if (objTA != null)
                    {
                        DashboardViewItem viRawData = ((DetailView)View).FindItem("RawData") as DashboardViewItem;
                        DashboardViewItem viDateVisualization = ((DetailView)View).FindItem("DateVisualization") as DashboardViewItem;
                        string strCriteria = string.Empty;
                        if (viRawData != null && viRawData.InnerView != null && viDateVisualization != null && viDateVisualization.InnerView != null)
                        {
                            foreach (TrendAnalysis TA in ((ListView)viDateVisualization.InnerView).CollectionSource.List.Cast<TrendAnalysis>().ToList())
                            {
                                ((ListView)viDateVisualization.InnerView).CollectionSource.Remove(TA);
                            }
                            ((ListView)viDateVisualization.InnerView).Refresh();
                            if (objTA.From != DateTime.MinValue && objTA.To != DateTime.MinValue)
                            {
                                //strCriteria = "[Samplelogin.CreatedDate] BETWEEN('" + Convert.ToDateTime( objTA.From) + "', '" + Convert.ToDateTime(  objTA.To )+ "')";
                                //strCriteria = "[Samplelogin.CreatedDate] BETWEEN('" + Convert.ToDateTime(objTA.From) + "', '" + Convert.ToDateTime(objTA.To) + "')";
                                //strCriteria =  String.Format("[Samplelogin.CreatedDate] >( '" + objTA.From + "' and [Samplelogin.CreatedDate] < '" + objTA.To + "')");
                                //TimeSpan newTime = new TimeSpan(23, 59, 0);
                                //objTA.To = objTA.To + newTime;
                                strCriteria = "[Samplelogin.JobID.RecievedDate] BETWEEN('" + objTA.From + "', '" + objTA.To + "')";
                            }
                            if (objTA.Client != null)
                            {
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    strCriteria = strCriteria + "And" + string.Format("[Samplelogin.JobID.ClientName.Oid] = '{0}'", objTA.Client.Oid);
                                }
                                else
                                {
                                    strCriteria = string.Format("[Samplelogin.JobID.ClientName.Oid] = '{0}'", objTA.Client.Oid);
                                }
                            }
                            if (objTA.Project != null)
                            {
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    strCriteria = strCriteria + "And" + string.Format("[Samplelogin.JobID.ProjectID.Oid] = '{0}'", objTA.Project.Oid);
                                }
                                else
                                {
                                    strCriteria = string.Format("[Samplelogin.JobID.ProjectID.Oid] = '{0}'", objTA.Project.Oid);
                                }
                            }
                            if (objTA.GeographicSelector != null)
                            {
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    strCriteria = strCriteria + "And" + string.Format("[Samplelogin.SamplingLocation] = '{0}'", objTA.GeographicSelector.SamplingLocation);
                                }
                                else
                                {
                                    strCriteria = string.Format("[Samplelogin.SamplingLocation] = '{0}'", objTA.GeographicSelector.SamplingLocation);
                                }
                            }
                            if (objTA.Parameter != null && !string.IsNullOrEmpty(objTA.Parameter))
                            {
                                List<string> lstSMOid = objTA.Parameter.Split(';').ToList();
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    strCriteria = strCriteria + "And [Testparameter.Parameter.Oid] in (" + string.Format("'{0}'", string.Join("','", lstSMOid.Select(i => i.ToString().Trim().Replace("'", "''")))) + ")";
                                }
                                else
                                {
                                    strCriteria = "[Testparameter.Parameter.Oid] in (" + string.Format("'{0}'", string.Join("','", lstSMOid.Select(i => i.ToString().Trim().Replace("'", "''")))) + ")";
                                }
                            }
                            if (!string.IsNullOrEmpty(strCriteria))
                            {
                                strCriteria = strCriteria + "And [SignOff] = True and [IsTransferred] = true And [Status] <>'" + Samplestatus.PendingEntry + "'";
                                ((ListView)viRawData.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(strCriteria);
                            }
                            else
                            {
                                ((ListView)viRawData.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ChooseQuery"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            ((ListView)viRawData.InnerView).CollectionSource.ObjectSpace.Refresh();
                        }
                        info.lstSelectedObject = new List<SampleParameter>();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void TACalculate_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TrendAnalysis_DetailView")
                {
                    DashboardViewItem viRawData = ((DetailView)View).FindItem("RawData") as DashboardViewItem;
                    DashboardViewItem viDateVisualization = ((DetailView)View).FindItem("DateVisualization") as DashboardViewItem;
                    if (viRawData.InnerView.SelectedObjects.Count > 0)
                    {
                        foreach (var obj in ((ListView)viDateVisualization.InnerView).CollectionSource.List.Cast<TrendAnalysis>().ToList())
                        {
                            ((ListView)viDateVisualization.InnerView).CollectionSource.Remove(obj);
                        }
                        IList<string> lststr = viRawData.InnerView.SelectedObjects.Cast<SampleParameter>().Where(i => i.Testparameter.Parameter != null).Select(i => i.Testparameter.Parameter.ParameterName).Distinct().ToList();
                        foreach (string strparameter in lststr)
                        {
                            IList<SampleParameter> lstSP = viRawData.InnerView.SelectedObjects.Cast<SampleParameter>().Where(i => i.Testparameter.Parameter != null && i.Testparameter.Parameter.ParameterName == strparameter && !string.IsNullOrEmpty(i.ResultNumeric)).ToList();
                            if (lstSP != null && lstSP.Count>0)
                            { 
                            TrendAnalysis newTA = ((ListView)viDateVisualization.InnerView).CollectionSource.ObjectSpace.CreateObject<TrendAnalysis>();
                            newTA.DVParameter = strparameter;
                            newTA.Units = lstSP.Where(i => i.Units != null).Select(i => i.Units.UnitName).FirstOrDefault();
                            newTA.Minimum = lstSP.Where(a => a.ResultNumeric != null).Select(i => i.ResultNumeric).Min();
                            newTA.Maximum = lstSP.Where(a => a.ResultNumeric != null).Select(i => i.ResultNumeric).Max();
                            newTA.PointCount = lstSP.Count.ToString();
                            Tuple<double, double> tuple = StandardDeviation(lstSP.Select(i => i.DVResult).Where(d => d.HasValue).Select(d => d.Value));
                            newTA.Average = Math.Round(tuple.Item1, 5).ToString();
                            newTA.STDV = Math.Round(tuple.Item2, 5).ToString();
                            ((ListView)viDateVisualization.InnerView).CollectionSource.Add(newTA);
                            }
                        }
                        ((ListView)viDateVisualization.InnerView).Refresh();
                        info.lstSelectedObject = viRawData.InnerView.SelectedObjects.Cast<SampleParameter>().ToList();
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select atleast one row.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void TATrend_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                View view;
                if (string.IsNullOrEmpty(info.actionname))
                {
                    info.actionname = "Trend";
                }
                if (View.Id == "TrendAnalysis_ListView_DateVisualization")
                {
                    view = ((NestedFrame)Frame).ViewItem.View;
                }
                else
                {
                    view = View;
                }
                DashboardViewItem viData = ((DetailView)view).FindItem("DateVisualization") as DashboardViewItem;
                if (viData != null && viData.InnerView != null && ((ListView)viData.InnerView).CollectionSource.List.Count > 0)
                {
                    if (viData.InnerView.SelectedObjects.Count > 0)
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        TrendAnalysis objTA = os.CreateObject<TrendAnalysis>();
                        //CollectionSource cs = new CollectionSource(os, typeof(TrendAnalysis));
                        //ListView createdView = Application.CreateListView("TrendAnalysis_ListView_Chart", cs, false);
                        DetailView createdView = Application.CreateDetailView(os, "TrendAnalysis_DetailView_Charts", true, objTA);
                        createdView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        showViewParameters.Context = TemplateContext.NestedFrame;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.AcceptAction.Active.SetItemValue("AcceptAction", false);
                        dc.CancelAction.Active.SetItemValue("CancelAction", false);
                        dc.ViewClosed += Dc_ViewClosed;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        //IObjectSpace os = Application.CreateObjectSpace();
                        //CollectionSource cs = new CollectionSource(os, typeof(TrendAnalysis));
                        //ListView createdView = Application.CreateListView("TrendAnalysis_ListView_Chart", cs, false);
                        //ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        //showViewParameters.Context = TemplateContext.NestedFrame;
                        //showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        //DialogController dc = Application.CreateController<DialogController>();
                        //dc.AcceptAction.Active.SetItemValue("AcceptAction", false);
                        //dc.CancelAction.Active.SetItemValue("CancelAction", false);
                        //dc.ViewClosed += Dc_ViewClosed;
                        //showViewParameters.Controllers.Add(dc);
                        //Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select atleast one row.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Resultstatistics"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
            info.actionname = string.Empty;
        }

        private Tuple<double, double> StandardDeviation(IEnumerable<double> enumerable)
        {
            double avg = enumerable.Average();
            return Tuple.Create(avg, Math.Sqrt(enumerable.Average(v => Math.Pow(v - avg, 2))));
        }

        private void ListTrend_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            info.actionname = "ListTrend";
            TATrend.DoExecute();
        }
        private void RefreshScale_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "TrendAnalysis_DetailView_Charts")
                {
                    TrendAnalysis objTA = e.CurrentObject as TrendAnalysis;
                    if (objTA.MinimumScale != null && objTA.MaximumScale != null)
                    {
                        if (objTA.MinimumScale <= objTA.MaximumScale)
                        {
                            info.Refresh = true;
                        }
                        else
                        {
                            info.Refresh = false;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Maxgreater"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        info.Refresh = false;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Maxnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void AutoScaleChart_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                info.Refresh = false;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
