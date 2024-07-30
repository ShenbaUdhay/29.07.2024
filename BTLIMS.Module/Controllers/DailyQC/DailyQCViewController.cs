using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LDM.Module.Controllers.DailyQC
{
    public partial class DailyQCViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        DailyQCinfo qCinfo = new DailyQCinfo();
        private SingleChoiceAction cmbReportName;
        private string JobID;
        private string DLQCID;
        private string dcUqOid;
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        private Stream newms;
        curlanguage objLanguage = new curlanguage();
        string strLWID = string.Empty;

        public DailyQCViewController()
        {
            InitializeComponent();
            TargetViewId = "DailyQC_ListView;" + "DailyQC_DetailView;" + "DailyQCSettings_ListView_dailyqc;" + "DailyQCChart_DetailView;" + "DailyQC_ListView_Data;" + "DailyQC_ListView_Chart;" + "Labware_ListView_DailyQC;"+ "Labware_ListView;";
            QCChart.TargetViewId = "DailyQC_ListView";
            ChartRetrieve.TargetViewId = "DailyQCChart_DetailView";
            ChartReport.TargetViewId = "DailyQCChart_DetailView";
          
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "DailyQC_ListView" || View.Id == "DailyQC_DetailView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                }
                if (View.Id == "DailyQC_ListView")
                {
                    View.Caption = qCinfo.Dailyqc.Test.TestName;
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Test.Oid]=? and [Method.Oid]=? and [InstrumentID.Oid]=?", qCinfo.Dailyqc.Test.Oid, qCinfo.Dailyqc.Method.Oid, qCinfo.Dailyqc.InstrumentID.Oid);
                }
                else if (View.Id == "DailyQC_DetailView")
                {
                    View.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "DailyQCSettings_ListView_dailyqc")
                {
                    Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem += Dailyqc_CustomProcessSelectedItem;
                }
                else if (View.Id == "Labware_ListView_DailyQC")
                {
                    DailyQCSettings qCSettings = (DailyQCSettings)Application.MainWindow.View.CurrentObject;
                    if (qCSettings != null && qCSettings.Test != null && qCSettings.Test.Labwares != null && qCSettings.Test.Labwares.Count > 0)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] in (" + string.Format("'{0}'", string.Join("','", qCSettings.Test.Labwares.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")");
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria.Clear();
                    }
                }
                else if (View.Id == "DailyQC_ListView_Data" || View.Id == "DailyQC_ListView_Chart")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] in (" + string.Format("'{0}'", string.Join("','", qCinfo.sellist.Select(i => i.Oid.ToString().Replace("'", "''")))) + ")");
                }
                else if (View.Id == "DailyQCChart_DetailView")
                {
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated += DailyQCViewController_ItemCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DailyQCViewController_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            if (e.TemplateContainer != null && e.TemplateContainer is LayoutItemTemplateContainer && e.ModelLayoutElement.Id != "ReferenceValue")
            {
                if (e.TemplateContainer.CaptionControl != null)
                {
                    CustomizeCaptionControl(e.TemplateContainer.CaptionControl);
                }
                else
                {
                    e.TemplateContainer.Load += TemplateContainer_Load;
                }
            }
        }

        private void TemplateContainer_Load(object sender, EventArgs e)
        {
            try
            {
                if (sender != null && sender is LayoutItemTemplateContainerBase)
                {
                    LayoutItemTemplateContainerBase templateControler = (LayoutItemTemplateContainerBase)sender;
                    if (templateControler != null)
                    {
                        templateControler.Load -= TemplateContainer_Load;
                        if (templateControler != null && templateControler.CaptionControl != null)
                        {
                            CustomizeCaptionControl(templateControler.CaptionControl);
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

        private void CustomizeCaptionControl(WebControl captionControl)
        {
            try
            {
                if (captionControl != null)
                {
                    captionControl.Width = new System.Web.UI.WebControls.Unit(50);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void Dailyqc_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.Setting.DailyQCSettings qc = (Modules.BusinessObjects.Setting.DailyQCSettings)e.InnerArgs.CurrentObject;
                if (qc != null)
                {
                    qCinfo.Dailyqc = qc;
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(Modules.BusinessObjects.Setting.DailyQC));
                    Frame.SetView(Application.CreateListView("DailyQC_ListView", cs, true));
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Modules.BusinessObjects.Setting.DailyQC objdailyqc = (Modules.BusinessObjects.Setting.DailyQC)View.CurrentObject;
                if (objdailyqc != null)
                {
                    if (objdailyqc.Analyst.Oid.ToString() != SecuritySystem.CurrentUserId.ToString())
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Cannot be edited", InformationType.Error, timer.Seconds, InformationPosition.Top);
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
                if (View.Id == "DailyQC_DetailView")
                {
                    ViewItem item = ((DetailView)View).FindItem("Status");
                    if (item != null && e.PropertyName == "Reading")
                    {
                        Modules.BusinessObjects.Setting.DailyQC objdailyqc = (Modules.BusinessObjects.Setting.DailyQC)e.Object;
                        if (objdailyqc != null)
                        {
                            ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.Font.Bold = true;
                                if (objdailyqc.Reading >= objdailyqc.LCL && objdailyqc.Reading <= objdailyqc.UCL)
                                {
                                    objdailyqc.Status = "Pass";
                                    editor.Editor.ForeColor = Color.Green;
                                }
                                else if (objdailyqc.Reading < objdailyqc.LCL || objdailyqc.Reading > objdailyqc.UCL)
                                {
                                    objdailyqc.Status = "Fail";
                                    editor.Editor.ForeColor = Color.Red;
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

        private void NewObjectAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                if (qCinfo.Dailyqc != null)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Modules.BusinessObjects.Setting.DailyQC objdailyqc = os.CreateObject<Modules.BusinessObjects.Setting.DailyQC>();
                    objdailyqc.Date = DateTime.Now;
                    objdailyqc.Analyst = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    objdailyqc.InstrumentID = os.GetObjectByKey<Labware>(qCinfo.Dailyqc.InstrumentID.Oid);
                    objdailyqc.LCL = qCinfo.Dailyqc.LCL;
                    objdailyqc.UCL = qCinfo.Dailyqc.UCL;
                    objdailyqc.Units = os.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(qCinfo.Dailyqc.Units.Oid);
                    objdailyqc.Test = os.GetObjectByKey<TestMethod>(qCinfo.Dailyqc.Test.Oid);
                    objdailyqc.Method = os.GetObjectByKey<TestMethod>(qCinfo.Dailyqc.Method.Oid);
                    objdailyqc.ReferenceValue = qCinfo.Dailyqc.StandardValue;
                    DetailView dvdailyqc = Application.CreateDetailView(os, "DailyQC_DetailView", true, objdailyqc);
                    dvdailyqc.ViewEditMode = ViewEditMode.Edit;
                    dvdailyqc.Caption = qCinfo.Dailyqc.Test.TestName;
                    Frame.SetView(dvdailyqc);
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
                if (View.Id == "DailyQCChart_DetailView")
                {
                    if (qCinfo.sellist.Count > 0)
                    {
                        DailyQCChart objDQC = (DailyQCChart)View.CurrentObject;
                        if (objDQC != null)
                        {
                            qCinfo.sellist = new List<Modules.BusinessObjects.Setting.DailyQC>();
                            DashboardViewItem dqcdata = ((DetailView)View).FindItem("DailyQCData") as DashboardViewItem;
                            DashboardViewItem dqcchart = ((DetailView)View).FindItem("DailyQCChart") as DashboardViewItem;
                            if (dqcdata != null && dqcdata.InnerView != null && dqcchart != null && dqcchart.InnerView != null)
                            {
                                foreach (Modules.BusinessObjects.Setting.DailyQC calset in ((ListView)dqcdata.InnerView).CollectionSource.List)
                                {
                                    calset.Mean = Convert.ToDouble(objDQC.Mean);
                                }
                                foreach (Modules.BusinessObjects.Setting.DailyQC calset in ((ListView)dqcchart.InnerView).CollectionSource.List)
                                {
                                    calset.Mean = Convert.ToDouble(objDQC.Mean);
                                }
                            }
                        }
                    }
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                ASPxTextBox textBox = (ASPxTextBox)propertyEditor.Editor;
                                if (textBox != null)
                                {
                                    textBox.ReadOnly = true;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                    }
                }
                else if (View.Id == "DailyQC_DetailView")
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    if (propertyEditor.Id == "Status")
                                    {
                                        propertyEditor.Editor.Font.Bold = true;
                                        if (propertyEditor.PropertyValue != null)
                                        {
                                            if (propertyEditor.PropertyValue.ToString() == "Pass")
                                            {
                                                propertyEditor.Editor.ForeColor = Color.Green;
                                            }
                                            else if (propertyEditor.PropertyValue.ToString() == "Fail")
                                            {
                                                propertyEditor.Editor.ForeColor = Color.Red;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        propertyEditor.Editor.ForeColor = Color.Black;
                                    }
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.DropDownEdit != null)
                                {
                                    if (propertyEditor.AllowEdit)
                                    {
                                        propertyEditor.DropDownEdit.DropDown.BackColor = Color.LightYellow;
                                    }
                                    else
                                    {
                                        propertyEditor.DropDownEdit.DropDown.ForeColor = Color.Black;
                                    }
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDoublePropertyEditor))
                        {
                            ASPxDoublePropertyEditor propertyEditor = item as ASPxDoublePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(FileDataPropertyEditor))
                        {
                            FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                if (propertyEditor.AllowEdit)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                                else
                                {
                                    propertyEditor.Editor.ForeColor = Color.Black;
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "DailyQC_ListView_Data")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) { s.RowClick.ClearHandlers(); }";
                }
                else if (View.Id == "Labware_ListView_DailyQC")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 400;
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
                if (View.Id == "DailyQC_ListView" || View.Id == "DailyQC_DetailView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                }
                if (View.Id == "DailyQC_DetailView")
                {
                    View.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "DailyQCSettings_ListView_dailyqc")
                {
                    Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem -= Dailyqc_CustomProcessSelectedItem;
                }
                else if (View.Id == "DailyQCChart_DetailView")
                {
                    ((WebLayoutManager)((DetailView)View).LayoutManager).ItemCreated -= DailyQCViewController_ItemCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QCChart_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                qCinfo.sellist = new List<Modules.BusinessObjects.Setting.DailyQC>();
                foreach (Modules.BusinessObjects.Setting.DailyQC qC in View.SelectedObjects)
                {
                    qCinfo.sellist.Add(qC);
                }
                Tuple<double, double> tuple = StandardDeviation(qCinfo.sellist.Select(a => a.Reading));
                double mean = Math.Round(tuple.Item1, 3);
                double stdv = Math.Round(tuple.Item2, 3);
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                Modules.BusinessObjects.Setting.DailyQCChart obj = objectSpace.CreateObject<Modules.BusinessObjects.Setting.DailyQCChart>();
                obj.Mean = mean.ToString();
                obj.STD = stdv.ToString();
                obj.UCL = qCinfo.sellist[0].UCL.ToString();
                obj.LCL = qCinfo.sellist[0].LCL.ToString();
                obj.ReferenceValue = qCinfo.sellist[0].ReferenceValue.ToString();
                obj.Count = qCinfo.sellist.Count.ToString();
                DetailView createdView = Application.CreateDetailView(objectSpace, "DailyQCChart_DetailView", true, obj);
                createdView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.AcceptAction.Active.SetItemValue("AcceptAction", false);
                dc.CancelAction.Active.SetItemValue("CancelAction", false);
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private Tuple<double, double> StandardDeviation(IEnumerable<double> values)
        {
            double avg = values.Average();
            return Tuple.Create(avg, Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2))));
        }

        private void ChartRetrieve_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "DailyQCChart_DetailView")
                {
                    DailyQCChart objDQC = (DailyQCChart)View.CurrentObject;
                    if (objDQC != null)
                    {
                        if (objDQC.From != DateTime.MinValue && objDQC.To != DateTime.MinValue)
                        {
                            qCinfo.sellist = new List<Modules.BusinessObjects.Setting.DailyQC>();
                            DashboardViewItem dqcdata = ((DetailView)View).FindItem("DailyQCData") as DashboardViewItem;
                            DashboardViewItem dqcchart = ((DetailView)View).FindItem("DailyQCChart") as DashboardViewItem;
                            if (dqcdata != null && dqcdata.InnerView != null && dqcchart != null && dqcchart.InnerView != null && qCinfo.Dailyqc != null)
                            {
                                ((ListView)dqcdata.InnerView).CollectionSource.Criteria["Filter"] =
                                    CriteriaOperator.Parse("GETDATE([Date]) BETWEEN('" + objDQC.From + "', '" + objDQC.To + "') and [Test.Oid]=? and [Method.Oid]=? and [InstrumentID.Oid]=?", qCinfo.Dailyqc.Test.Oid, qCinfo.Dailyqc.Method.Oid, qCinfo.Dailyqc.InstrumentID.Oid);
                                ((ListView)dqcchart.InnerView).CollectionSource.Criteria["Filter"] =
                                    CriteriaOperator.Parse("GETDATE([Date]) BETWEEN('" + objDQC.From + "', '" + objDQC.To + "') and [Test.Oid]=? and [Method.Oid]=? and [InstrumentID.Oid]=?", qCinfo.Dailyqc.Test.Oid, qCinfo.Dailyqc.Method.Oid, qCinfo.Dailyqc.InstrumentID.Oid);
                                if (((ListView)dqcdata.InnerView).CollectionSource.GetCount() > 0)
                                {
                                    Tuple<double, double> tuple = StandardDeviation(((ListView)dqcdata.InnerView).CollectionSource.List.Cast<Modules.BusinessObjects.Setting.DailyQC>().Select(a => a.Reading));
                                    double mean = Math.Round(tuple.Item1, 3);
                                    double stdv = Math.Round(tuple.Item2, 3);
                                    objDQC.Mean = mean.ToString();
                                    objDQC.STD = stdv.ToString();
                                    objDQC.Count = ((ListView)dqcdata.InnerView).CollectionSource.GetCount().ToString();
                                    foreach (Modules.BusinessObjects.Setting.DailyQC calset in ((ListView)dqcdata.InnerView).CollectionSource.List)
                                    {
                                        calset.Mean = mean;
                                    }
                                    foreach (Modules.BusinessObjects.Setting.DailyQC calset in ((ListView)dqcchart.InnerView).CollectionSource.List)
                                    {
                                        calset.Mean = mean;
                                    }
                                }
                                else
                                {
                                    objDQC.Mean = "0";
                                    objDQC.STD = "0";
                                    objDQC.Count = "0";
                                }
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("'From','To' cannot be empty!", InformationType.Error, timer.Seconds, InformationPosition.Top);
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
        private void ChartReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DLQCID = string.Empty;
                foreach (Modules.BusinessObjects.Setting.DailyQC obj in Application.MainWindow.View.SelectedObjects)
                {
                    if (string.IsNullOrEmpty(DLQCID))
                    {
                        DLQCID = "'" + obj.DLQCID + "'";
                    }
                    else
                    {
                        DLQCID = DLQCID + ",'" + obj.DLQCID + "'";
                    }
                }

                XtraReport xtraReport = new XtraReport();
                ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SetConnectionString();
                DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);


                ObjReportingInfo.strDLQCID = DLQCID;
                string strTempPath = Path.GetTempPath();
                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\Preview\DailyQc\")) == false)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\Preview\DailyQc\"));
                }
                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\Preview\DailyQc\" + timeStamp + ".pdf");

                DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);
                //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                ObjReportingInfo.strDLQCID = DLQCID;
                xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("DailyQCReport", ObjReportingInfo, false);
                //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                xtraReport.ExportToPdf(strExportedPath);
                string[] path = strExportedPath.Split('\\');
                int arrcount = path.Count();
                int sc = arrcount - 3;
                string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));


                //using (MemoryStream ms = new MemoryStream())
                //{
                //    xtraReport.ExportToPdf(ms);
                //    using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                //    {
                //        source.LoadDocument(ms);
                //        foreach (DevExpress.Pdf.PdfPage page in source.Document.Pages)
                //        {
                //            var curpageval = listPage[source.Document.Pages.IndexOf(page)];
                //            if (curpageval.Length > 0)
                //            {
                //                using (DevExpress.Pdf.PdfGraphics graphics = source.CreateGraphics())
                //                {
                //                    DevExpress.Pdf.PdfRectangle rectangle = page.MediaBox;
                //                    RectangleF r = new RectangleF((float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
                //                    SolidBrush black = (SolidBrush)Brushes.Black;
                //                    using (Font font = new Font("Microsoft Yahei", 11, FontStyle.Regular))
                //                    {
                //                        string text;
                //                        if (objLanguage.strcurlanguage == "En")
                //                        {
                //                            text = "Total " + pagenumber + " of " + curpageval + " page";
                //                        }
                //                        else
                //                        {
                //                            text = "共 " + pagenumber + " 页 第 " + curpageval + " 页";
                //                        }
                //                        graphics.DrawString(text, font, black, r.Width + 48, 170);
                //                    }
                //                    graphics.AddToPageForeground(page);
                //                }
                //            }
                //        }
                //        //source.SaveDocument(newms);
                //    }
                //}
                //tempxtraReport.ShowPreview();
                //tempxtraReport.ExportToPdf();
                //tempxtraReport.CreateDocument();
                //xtraReport.Pages.AddRange(tempxtraReport.Pages);
                //xtraReport.ShowPreview();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SetConnectionString()
        {
            try
            {
                AppSettingsReader config = new AppSettingsReader();
                string serverType, server, database, user, password;
                string[] connectionstring = ObjReportDesignerInfo.WebConfigConn.Split(';');
                ObjReportDesignerInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
