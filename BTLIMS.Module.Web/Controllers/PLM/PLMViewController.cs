using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace LDM.Module.Controllers.PLM
{
    public partial class PLMViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        AnaliytialBatchinfo objABinfo = new AnaliytialBatchinfo();
        PLMInfo plmInfo = new PLMInfo();
        public PLMViewController()
        {
            InitializeComponent();
            TargetViewId = "PLM;"
                + "PLMStereoscopicObservation_ListView;"
                + "NPPLMStereoscopicObservation_DetailView;"
                + "PLMExam_ListView;"
                + "NPPLMExam_DetailView;"
                + "Non_AsbestosFibers_LookupListView;"
                + "OtherComponents_LookupListView;"
                + "Non_FibrousComponents_LookupListView;"
                + "AsbestosFibers_LookupListView;"
                + "PLMCopyToCombo_DetailView;";
            PLMRemoveLayer.TargetViewId = "PLMStereoscopicObservation_ListView;";
            PLMAddLayer.TargetViewId = "PLMStereoscopicObservation_ListView;";
            PLMAddType.TargetViewId = "PLMExam_ListView;";
            PLMRemoveType.TargetViewId = "PLMExam_ListView;";
            PLMSave.TargetViewId = "PLM;";
            PLMComplete.TargetViewId = "PLM;";
            CarryOverResults.TargetViewId = "PLM;";
            FullScreen.TargetViewId = "PLM;";
            PLMDelete.TargetViewId = "PLM;";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "PLM")
                {
                    FullScreen.SetClientScript("plmuirefresh();", false);
                    if (qcbatchinfo.QCBatchOid != null && qcbatchinfo.qcstatus < 2)
                    {
                        enbactions(true);
                    }
                    else if (qcbatchinfo.QCBatchOid == null)
                    {
                        enbactions(true);
                        PLMDelete.Active.SetItemValue("enb", false);
                    }
                    else
                    {
                        enbactions(false);
                    }
                    Application.DetailViewCreating += Application_DetailViewCreating;
                    ((WebLayoutManager)((DashboardView)View).LayoutManager).PageControlCreated += PLMViewController_PageControlCreated;
                    ((WebLayoutManager)((DashboardView)View).LayoutManager).PageControlCreated += TabViewController_PageControlCreated;
                }
                else if (View.Id == "PLMStereoscopicObservation_ListView")
                {
                    if (qcbatchinfo.qcstatus < 2)
                    {
                        PLMAddLayer.Enabled.SetItemValue("enb", true);
                        PLMRemoveLayer.Enabled.SetItemValue("enb", true);
                    }
                    else
                    {
                        PLMAddLayer.Enabled.SetItemValue("enb", false);
                        PLMRemoveLayer.Enabled.SetItemValue("enb", false);
                    }
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
                    if (qcbatchinfo.QCBatchOid != null)
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Qcbatchsequence.qcseqdetail]=?", qcbatchinfo.QCBatchOid);
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is Null");
                        if (objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstQCBatchSequence.Count > 0)
                        {
                            convertqcos(View.ObjectSpace);
                        }
                    }
                }
                else if (View.Id == "AsbestosFibers_LookupListView" || View.Id == "Non_AsbestosFibers_LookupListView" || View.Id == "OtherComponents_LookupListView" || View.Id == "Non_FibrousComponents_LookupListView")
                {
                    View.ControlsCreated += View_ControlsCreated;
                }
                else if (View.Id == "PLMExam_ListView")
                {
                    if (qcbatchinfo.qcstatus < 2)
                    {
                        PLMAddType.Enabled.SetItemValue("enb", true);
                        PLMRemoveType.Enabled.SetItemValue("enb", true);
                    }
                    else
                    {
                        PLMAddType.Enabled.SetItemValue("enb", false);
                        PLMRemoveType.Enabled.SetItemValue("enb", false);
                    }
                    DashboardViewItem lvSeqList = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("SequenceList") as DashboardViewItem;
                    if (lvSeqList != null && lvSeqList.InnerView != null)
                    {
                        PLMStereoscopicObservation objQcSeqList = (PLMStereoscopicObservation)lvSeqList.InnerView.CurrentObject;
                        if (objQcSeqList != null)
                        {
                            ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", objQcSeqList.Oid);
                        }
                    }
                    else
                    {
                        ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is Null");
                    }
                }
                else if (View.Id == "NPPLMStereoscopicObservation_DetailView" || View.Id == "NPPLMExam_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active["DisableUnsavedChangesNotificationController"] = true;
                    if (View.Id == "NPPLMExam_DetailView")
                    {
                        View.ControlsCreated += View_ControlsCreated_NPPLMExam;
                    }
                }
                else if (View.Id == "PLMCopyToCombo_DetailView")
                {
                    ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void enbactions(bool val)
        {
            PLMDelete.Active.SetItemValue("enb", val);
            PLMSave.Active.SetItemValue("enb", val);
            PLMComplete.Active.SetItemValue("enb", val);
            CarryOverResults.Active.SetItemValue("enb", val);
        }

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            e.PopupControl.CustomizePopupControl += PopupControl_CustomizePopupControl;
        }

        private void PopupControl_CustomizePopupControl(object sender, CustomizePopupControlEventArgs e)
        {
            XafPopupWindowControl popupControl = (XafPopupWindowControl)sender;
            popupControl.CustomizePopupControl -= PopupControl_CustomizePopupControl;
            string script = CallbackManager.GetScript("Copypopup", string.Empty);
            ClientSideEventsHelper.AssignClientHandlerSafe(e.PopupControl, "Closing", $"function(s, e) {{ if(e.closeReason==='CloseButton') {{ {script} }} }}", "MyWindowController");
        }

        protected XafCallbackManager CallbackManager
        {
            get { return ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager; }
        }

        private void RuleSet_CustomNeedToValidateRule(object sender, DevExpress.Persistent.Validation.CustomNeedToValidateRuleEventArgs e)
        {
            try
            {
                if (e.Rule.Id == "ABNPJobid")
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

        private void View_ControlsCreated_NPPLMExam(object sender, EventArgs e)
        {
            try
            {
                View.ControlsCreated -= View_ControlsCreated_NPPLMExam;
                DashboardViewItem lvPlmExam = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamList") as DashboardViewItem;
                if (lvPlmExam != null && lvPlmExam.InnerView != null)
                {
                    if (((ListView)lvPlmExam.InnerView).CollectionSource.GetCount() == 0)
                    {
                        disenbcontrols(false, View);
                    }
                    else
                    {
                        disenbcontrols(true, View);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PLMViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            try
            {
                if (e.Model.Id == "Item2")
                {
                    e.PageControl.Init += PageControl_Init;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TabViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            try
            {
                ((WebLayoutManager)((DashboardView)View).LayoutManager).PageControlCreated -= TabViewController_PageControlCreated;
                if (e.Model.Id == "Item2")
                {
                    e.PageControl.ClientSideEvents.Init = "function(s,e) { s.SetActiveTabIndex(0); }";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PageControl_Init(object sender, EventArgs e)
        {
            try
            {
                ASPxPageControl pageControl = (ASPxPageControl)sender;
                ClientSideEventsHelper.AssignClientHandlerSafe(pageControl, "Init", "plmtab", "LayoutTabContainer");
                ClientSideEventsHelper.AssignClientHandlerSafe(pageControl, "ActiveTabChanged", "plmtab", "LayoutTabContainer");
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
                if (View.Id == "NPPLMStereoscopicObservation_DetailView")
                {
                    if (e.PropertyName == "PositiveStop" && e.NewValue != e.OldValue)
                    {
                        DashboardViewItem dvPlmView = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamDetail") as DashboardViewItem;
                        NPPLMStereoscopicObservation objSampleChanged = (NPPLMStereoscopicObservation)e.Object;
                        if (objSampleChanged != null && objSampleChanged.PositiveStop == true)
                        {
                            disenbcontrols(false, View);
                            if (dvPlmView != null && dvPlmView.InnerView != null)
                            {
                                disenbcontrols(false, dvPlmView.InnerView);
                                ClearPLMEx((NPPLMExam)dvPlmView.InnerView.CurrentObject);
                                DashboardViewItem lvPlmExam = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamList") as DashboardViewItem;
                                if (lvPlmExam != null && lvPlmExam.InnerView != null)
                                {
                                    foreach (PLMExam pLMExam in ((ListView)lvPlmExam.InnerView).CollectionSource.List.Cast<PLMExam>().ToList())
                                    {
                                        ((ListView)lvPlmExam.InnerView).CollectionSource.Remove(pLMExam);
                                        ((ListView)lvPlmExam.InnerView).ObjectSpace.Delete(pLMExam);
                                    }
                                }
                            }
                            ClearStereo(objSampleChanged);
                        }
                        else
                        {
                            disenbcontrols(true, View);
                            if (dvPlmView != null && dvPlmView.InnerView != null)
                            {
                                disenbcontrols(true, dvPlmView.InnerView);
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
            try
            {
                if (View.Id == "PLMStereoscopicObservation_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.UseFixedTableLayout = true;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    gridListEditor.Grid.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.Disabled;
                    CallbackManager.RegisterHandler("PLMStereo", this);
                    foreach (WebColumnBase column in gridListEditor.Grid.Columns)
                    {
                        IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                        if (columnInfo != null)
                        {
                            IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                            column.Width = System.Web.UI.WebControls.Unit.Pixel(modelColumn.Width);
                        }
                    }
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    { 
                        s.SetWidth(354); 
                        s.RowClick.ClearHandlers();
                        s.SetHeight(screen.height * 2/3.5);
                        if(s.GetVisibleRowsOnPage() > 0 && s.GetSelectedRowCount() == 0)
                        {
                            s.SelectRowOnPage(0);               
                        }
                    }";
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) 
                    { 
                        if(e.visibleIndex != -1)
                        {
                            s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                if (s.IsRowSelectedOnPage(e.visibleIndex)) { 
                                    RaiseXafCallback(globalCallbackControl, 'PLMStereo', Oidvalue , '', false);     
                                }
                            });                                                    
                        }   
                    }";
                }
                else if (View.Id == "PLMExam_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.SettingsLoadingPanel.Mode = GridViewLoadingPanelMode.Disabled;
                    CallbackManager.RegisterHandler("FiberTypePopup", this);
                    if (qcbatchinfo.qcstatus < 2)
                    {
                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                        {         
                            s.SetHeight(140);
                            s.GetEditor('Result').KeyPress.AddHandler(OnResultNumericChanged);
                            if(s.GetVisibleRowsOnPage() > 0 && s.GetSelectedRowCount() == 0)
                            {
                                s.SelectRowOnPage(0);               
                            }
                        }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s,e)
                        {
                            window.setTimeout(function() {       
                                for(col in s.BatchData[e.visibleIndex]) {
                                    s.batchEditApi.ResetChanges(e.visibleIndex); 
                                    RaiseXafCallback(globalCallbackControl, 'FiberTypePopup', 'Entered|'+e.key+'|'+col+'|'+s.BatchData[e.visibleIndex][col], '', false);      
                                }                                                            
                            }, 10); 
                        }";
                    }
                    else
                    {
                        gridListEditor.AllowEdit = false;
                        gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                        {         
                            s.SetHeight(140);
                            s.RowClick.ClearHandlers();
                            if(s.GetVisibleRowsOnPage() > 0 && s.GetSelectedRowCount() == 0)
                            {
                                s.SelectRowOnPage(0);               
                            }
                        }";
                    }
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) 
                    { 
                        if (e.visibleIndex != '-1')
                        {
                            if(s.cpRefresh == null)
                            {
                                s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                                    if (s.IsRowSelectedOnPage(e.visibleIndex)) { 
                                        RaiseXafCallback(globalCallbackControl, 'FiberTypePopup', 'PLMExamRow|' + Oidvalue , '', false);    
                                    }
                                });
                            }
                            s.cpRefresh = null;
                        }
                    }";
                }
                else if (View.Id == "AsbestosFibers_LookupListView" || View.Id == "Non_AsbestosFibers_LookupListView" || View.Id == "OtherComponents_LookupListView" || View.Id == "Non_FibrousComponents_LookupListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    { 
                        s.RowClick.ClearHandlers();
                    }";
                }
                else
                {
                    CallbackManager.RegisterHandler("Copypopup", this);
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
                if (View.Id == "PLMStereoscopicObservation_ListView")
                {
                    ASPxGridView grid = sender as ASPxGridView;
                    if (grid != null)
                    {
                        e.Cell.BackColor = grid.GetRowValues(e.VisibleIndex, "Qcbatchsequence.Status").ToString() == "Entered" ? Color.LightGreen : Color.Empty;
                    }
                }
                else if (View.Id == "PLMExam_ListView")
                {
                    if (e.DataColumn.FieldName != "Name") return;
                    e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'FiberTypePopup', 'PLMExamCell|{0}' , '', false)", e.VisibleIndex));
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
                if (View.Id == "PLM")
                {
                    Application.DetailViewCreating -= Application_DetailViewCreating;
                    ((WebLayoutManager)((DashboardView)View).LayoutManager).PageControlCreated -= PLMViewController_PageControlCreated;
                    qcbatchinfo.QCBatchOid = null;
                    plmInfo.Materials = null;
                    plmInfo.lstFiberTypesValues = null;
                    qcbatchinfo.strqcid = qcbatchinfo.strqcbatchid = string.Empty;
                    qcbatchinfo.strTest = string.Empty;
                    qcbatchinfo.OidTestMethod = null;
                    qcbatchinfo.strAB = string.Empty;
                    qcbatchinfo.qcstatus = 0;
                }
                else if (View.Id == "AsbestosFibers_LookupListView" || View.Id == "Non_AsbestosFibers_LookupListView" || View.Id == "OtherComponents_LookupListView" || View.Id == "Non_FibrousComponents_LookupListView")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                }
                else if (View.Id == "PLMStereoscopicObservation_ListView")
                {
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
                }
                else if (View.Id == "NPPLMStereoscopicObservation_DetailView" || View.Id == "NPPLMExam_DetailView")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Application_DetailViewCreating(object sender, DetailViewCreatingEventArgs e)
        {
            try
            {
                if (e.ViewID == "NPPLMStereoscopicObservation_DetailView" && e.Obj == null)
                {
                    IObjectSpace os = View.ObjectSpace;
                    plmInfo.LastPLMSte = null;
                    e.View = Application.CreateDetailView(os, "NPPLMStereoscopicObservation_DetailView", false, os.CreateObject<NPPLMStereoscopicObservation>());
                    e.View.ViewEditMode = ViewEditMode.Edit;
                }
                else if (e.ViewID == "NPPLMExam_DetailView" && e.Obj == null)
                {
                    IObjectSpace os = View.ObjectSpace;
                    ResultEntryDefaultSettings settings = os.GetObjects<ResultEntryDefaultSettings>(CriteriaOperator.Parse("")).FirstOrDefault();
                    if (settings != null && settings.PLMDefaultValue == ResultEntryDefaultSettings.YesNoFilter.Yes)
                    {
                        plmInfo.lstFiberTypesValues = os.GetObjects<FiberTypesValues>(CriteriaOperator.Parse(""));
                    }
                    plmInfo.LastPLMEx = null;
                    e.View = Application.CreateDetailView(os, "NPPLMExam_DetailView", false, os.CreateObject<NPPLMExam>());
                    e.View.ViewEditMode = ViewEditMode.Edit;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PLMAddLayer_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                PLMStereoscopicObservation objqcseq = (PLMStereoscopicObservation)View.CurrentObject;
                if (objqcseq != null)
                {
                    var strSampleId = objqcseq.Qcbatchsequence.StrSampleL.Split(new string[] { "." }, StringSplitOptions.None);
                    IList<PLMStereoscopicObservation> objqcbatch = ((ListView)View).CollectionSource.List.Cast<PLMStereoscopicObservation>().Where(a => a.Qcbatchsequence.StrSampleL.StartsWith(strSampleId[0])).OrderBy(a => a.Qcbatchsequence.StrSampleL).ToList();
                    if (objqcbatch.Count > 0)
                    {
                        const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        string valueeql = "";
                        for (int i = objqcbatch.Count; i <= objqcbatch.Count; i++)
                        {
                            QCBatchSequence qCBatch = View.ObjectSpace.CreateObject<QCBatchSequence>();
                            qCBatch.QCType = objqcseq.Qcbatchsequence.QCType;
                            qCBatch.batchno = objqcseq.Qcbatchsequence.batchno;
                            qCBatch.Runno = objqcseq.Qcbatchsequence.Runno;
                            qCBatch.SampleID = objqcseq.Qcbatchsequence.SampleID;
                            qCBatch.Sort = objqcseq.Qcbatchsequence.Sort;
                            qCBatch.StrSampleID = objqcseq.Qcbatchsequence.StrSampleID;
                            qCBatch.SYSSamplecode = objqcseq.Qcbatchsequence.SYSSamplecode;
                            qCBatch.qcseqdetail = objqcseq.Qcbatchsequence.qcseqdetail;
                            valueeql = "";
                            if (i >= letterseql.Length)
                                valueeql += letterseql[i / letterseql.Length - 1];

                            valueeql += letterseql[i % letterseql.Length];
                            qCBatch.StrSampleL = qCBatch.SampleID.SampleID + "." + valueeql;
                            PLMStereoscopicObservation PLMS = View.ObjectSpace.CreateObject<PLMStereoscopicObservation>();
                            PLMS.Qcbatchsequence = qCBatch;
                            ((ListView)View).CollectionSource.Add(PLMS);
                        }
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

        private void PLMRemoveLayer_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                PLMStereoscopicObservation objqcseq = (PLMStereoscopicObservation)View.CurrentObject;
                if (objqcseq != null)
                {
                    if (objqcseq.Qcbatchsequence.StrSampleL.EndsWith("A"))
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "RemoveSampleLayer"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    PLMExam PLM = View.ObjectSpace.FindObject<PLMExam>(CriteriaOperator.Parse("PLMStereoscopicObservation=?", objqcseq.Oid), true);
                    if (PLM != null)
                    {
                        View.ObjectSpace.Delete(PLM);
                    }
                    View.ObjectSpace.Delete(objqcseq.Qcbatchsequence);
                    var strSampleId = objqcseq.Qcbatchsequence.StrSampleL.Split(new string[] { "." }, StringSplitOptions.None);
                    int sort = 0;
                    foreach (PLMStereoscopicObservation objOrder in ((ListView)View).CollectionSource.List.Cast<PLMStereoscopicObservation>().Where(a => a.Qcbatchsequence.StrSampleL.StartsWith(strSampleId[0]) && a.Oid != objqcseq.Oid).OrderBy(a => a.Qcbatchsequence.StrSampleL))
                    {
                        const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        string valueeql = "";
                        for (int i = sort; i <= sort; i++)
                        {
                            valueeql = "";
                            if (i >= letterseql.Length)
                                valueeql += letterseql[i / letterseql.Length - 1];
                            valueeql += letterseql[i % letterseql.Length];
                            objOrder.Qcbatchsequence.StrSampleL = objOrder.Qcbatchsequence.SampleID.SampleID + "." + valueeql;
                        }
                        sort++;
                    }
                    ((ListView)View).CollectionSource.Remove(objqcseq);
                    View.ObjectSpace.Delete(objqcseq);
                    ((ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PLMAddType_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem Seqlist = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("SequenceList") as DashboardViewItem;
                DashboardViewItem dvPlmExam = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamDetail") as DashboardViewItem;
                DashboardViewItem dvPSO = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMstereoscopicDetail") as DashboardViewItem;
                if (Seqlist != null && Seqlist.InnerView != null && dvPSO != null && dvPSO.InnerView != null)
                {
                    PLMStereoscopicObservation curSeqList = (PLMStereoscopicObservation)Seqlist.InnerView.CurrentObject;
                    if (curSeqList != null && !((NPPLMStereoscopicObservation)dvPSO.InnerView.CurrentObject).PositiveStop)
                    {
                        PLMExam newPlmExam = View.ObjectSpace.CreateObject<PLMExam>();
                        newPlmExam.PLMStereoscopicObservation = curSeqList.Oid;
                        newPlmExam.Sort = ((ListView)View).CollectionSource.GetCount() + 1;
                        ((ListView)View).CollectionSource.Add(newPlmExam);
                        if (dvPlmExam != null && dvPlmExam.InnerView != null && ((ListView)View).CollectionSource.GetCount() == 1)
                        {
                            disenbcontrols(true, (DetailView)dvPlmExam.InnerView);
                        }
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

        private void PLMRemoveType_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem dvPlmExam = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamDetail") as DashboardViewItem;
                PLMExam cuPlmList = (PLMExam)View.CurrentObject;
                if (cuPlmList != null)
                {
                    ((ListView)View).CollectionSource.Remove(cuPlmList);
                    View.ObjectSpace.Delete(cuPlmList);
                    int i = 1;
                    foreach (PLMExam obj in ((ListView)View).CollectionSource.List.Cast<PLMExam>().OrderBy(a => a.Sort))
                    {
                        obj.Sort = i;
                        i++;
                    }
                    if (dvPlmExam != null && dvPlmExam.InnerView != null && ((ListView)View).CollectionSource.GetCount() == 0)
                    {
                        disenbcontrols(false, (DetailView)dvPlmExam.InnerView);
                        ClearPLMEx((NPPLMExam)dvPlmExam.InnerView.CurrentObject);
                    }
                    ((ListView)View).Refresh();
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
                    string[] param = parameter.Split('|');
                    if (View.Id == "PLMStereoscopicObservation_ListView")
                    {
                        DashboardViewItem dvPSO = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMstereoscopicDetail") as DashboardViewItem;
                        if (dvPSO != null && dvPSO.InnerView != null)
                        {
                            PLMStereoscopicObservation curpLM = ((ListView)View).CollectionSource.List.Cast<PLMStereoscopicObservation>().Where(a => a.Oid == new Guid(param[0])).FirstOrDefault();
                            if (curpLM != null)
                            {
                                if (plmInfo.LastPLMSte != null && qcbatchinfo.qcstatus < 2)
                                {
                                    PLMStereoscopicObservation pLM = ((ListView)View).CollectionSource.List.Cast<PLMStereoscopicObservation>().Where(a => a.Oid == plmInfo.LastPLMSte.Oid).FirstOrDefault();
                                    if (pLM != null)
                                    {
                                        ChangeStereo(pLM, (NPPLMStereoscopicObservation)dvPSO.InnerView.CurrentObject);
                                    }
                                }
                                else if (qcbatchinfo.qcstatus > 1)
                                {
                                    disenbcontrols(false, (DetailView)dvPSO.InnerView, true);
                                }
                                ChangeStereo((NPPLMStereoscopicObservation)dvPSO.InnerView.CurrentObject, curpLM);
                                ((ListView)View).Refresh();
                                DashboardViewItem lvPlmExam = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamList") as DashboardViewItem;
                                if (lvPlmExam != null && lvPlmExam.InnerView != null)
                                {
                                    DashboardViewItem dvPLE = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamDetail") as DashboardViewItem;
                                    if (dvPLE != null && dvPLE.InnerView != null)
                                    {
                                        if (plmInfo.LastPLMEx != null && qcbatchinfo.qcstatus < 2)
                                        {
                                            PLMExam pLM = ((ListView)lvPlmExam.InnerView).CollectionSource.List.Cast<PLMExam>().Where(a => a.Oid == plmInfo.LastPLMEx.Oid).FirstOrDefault();
                                            if (pLM != null)
                                            {
                                                ChangePLMEx(pLM, (NPPLMExam)dvPLE.InnerView.CurrentObject);
                                                plmInfo.LastPLMEx = null;
                                            }
                                        }
                                        foreach (PLMExam pLMExam in ((ListView)lvPlmExam.InnerView).CollectionSource.List.Cast<PLMExam>().ToList())
                                        {
                                            ((ListView)lvPlmExam.InnerView).CollectionSource.Remove(pLMExam);
                                        }
                                        foreach (PLMExam pLM in lvPlmExam.InnerView.ObjectSpace.GetObjects<PLMExam>(CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", curpLM.Oid), true))
                                        {
                                            ((ListView)lvPlmExam.InnerView).CollectionSource.Add(pLM);
                                        }
                                        if (((ListView)lvPlmExam.InnerView).CollectionSource.GetCount() == 0)
                                        {
                                            disenbcontrols(false, (DetailView)dvPLE.InnerView);
                                            ClearPLMEx((NPPLMExam)dvPLE.InnerView.CurrentObject);
                                        }
                                        else
                                        {
                                            if (qcbatchinfo.qcstatus > 1)
                                            {
                                                disenbcontrols(false, (DetailView)dvPLE.InnerView, true);
                                            }
                                            else
                                            {
                                                disenbcontrols(true, (DetailView)dvPLE.InnerView);
                                            }
                                            PLMExam curpLMEx = ((ListView)lvPlmExam.InnerView).CollectionSource.List.Cast<PLMExam>().Where(a => a.Oid == ((PLMExam)((ListView)lvPlmExam.InnerView).CollectionSource.List[0]).Oid).FirstOrDefault();
                                            if (curpLMEx != null)
                                            {
                                                ChangePLMEx((NPPLMExam)dvPLE.InnerView.CurrentObject, curpLMEx);
                                            }
                                        }
                                        ((ASPxGridListEditor)((ListView)lvPlmExam.InnerView).Editor).Grid.JSProperties["cpRefresh"] = false;
                                        ((ListView)lvPlmExam.InnerView).Refresh();
                                    }
                                }
                            }
                        }
                    }
                    else if (View.Id == "PLMExam_ListView")
                    {
                        if (param[0] == "PLMExamRow")
                        {
                            DashboardViewItem dvPLE = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamDetail") as DashboardViewItem;
                            if (dvPLE != null && dvPLE.InnerView != null)
                            {
                                PLMExam curpLM = ((ListView)View).CollectionSource.List.Cast<PLMExam>().Where(a => a.Oid == new Guid(param[1])).FirstOrDefault();
                                if (curpLM != null)
                                {
                                    if (plmInfo.LastPLMEx != null && qcbatchinfo.qcstatus < 2)
                                    {
                                        PLMExam pLM = ((ListView)View).CollectionSource.List.Cast<PLMExam>().Where(a => a.Oid == plmInfo.LastPLMEx.Oid).FirstOrDefault();
                                        if (pLM != null)
                                        {
                                            ChangePLMEx(pLM, (NPPLMExam)dvPLE.InnerView.CurrentObject);
                                        }
                                    }
                                    else if (qcbatchinfo.qcstatus > 1)
                                    {
                                        disenbcontrols(false, (DetailView)dvPLE.InnerView, true);
                                    }
                                    ChangePLMEx((NPPLMExam)dvPLE.InnerView.CurrentObject, curpLM);
                                }
                            }
                        }
                        else if (param[0] == "PLMExamCell")
                        {
                            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (gridListEditor != null)
                            {
                                HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                                if (HttpContext.Current.Session["rowid"] != null)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    PLMExam objPlmExam = ((ListView)View).CollectionSource.List.Cast<PLMExam>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).FirstOrDefault();
                                    if (objPlmExam != null && objPlmExam.FiberType != null)
                                    {
                                        HttpContext.Current.Session["FiberTypeName"] = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Name");
                                        if (objPlmExam.FiberType.FiberType == "Asbestos Fibers")
                                        {
                                            CollectionSource cs = new CollectionSource(os, typeof(AsbestosFibers));
                                            ListView lv = Application.CreateListView("AsbestosFibers_LookupListView", cs, false);
                                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                                            showViewParameters.Context = TemplateContext.PopupWindow;
                                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                            showViewParameters.CreatedView.Caption = objPlmExam.FiberType.FiberType;
                                            DialogController dc = Application.CreateController<DialogController>();
                                            dc.SaveOnAccept = false;
                                            dc.CloseOnCurrentObjectProcessing = false;
                                            dc.Accepting += Dc_Accepting;
                                            dc.AcceptAction.Execute += AcceptAction_Execute;
                                            showViewParameters.Controllers.Add(dc);
                                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                        }
                                        else if (objPlmExam.FiberType.FiberType == "Non-AsbestosFibers")
                                        {
                                            CollectionSource cs = new CollectionSource(os, typeof(Non_AsbestosFibers));
                                            ListView lv = Application.CreateListView("Non_AsbestosFibers_LookupListView", cs, false);
                                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                                            showViewParameters.Context = TemplateContext.PopupWindow;
                                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                            showViewParameters.CreatedView.Caption = objPlmExam.FiberType.FiberType;
                                            DialogController dc = Application.CreateController<DialogController>();
                                            dc.SaveOnAccept = false;
                                            dc.CloseOnCurrentObjectProcessing = false;
                                            dc.Accepting += Dc_Accepting;
                                            dc.AcceptAction.Execute += AcceptAction_Execute;
                                            showViewParameters.Controllers.Add(dc);
                                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                        }
                                        else if (objPlmExam.FiberType.FiberType == "Other Components")
                                        {
                                            CollectionSource cs = new CollectionSource(os, typeof(OtherComponents));
                                            ListView lv = Application.CreateListView("OtherComponents_LookupListView", cs, false);
                                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                                            showViewParameters.Context = TemplateContext.PopupWindow;
                                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                            showViewParameters.CreatedView.Caption = objPlmExam.FiberType.FiberType;
                                            DialogController dc = Application.CreateController<DialogController>();
                                            dc.SaveOnAccept = false;
                                            dc.CloseOnCurrentObjectProcessing = false;
                                            dc.Accepting += Dc_Accepting;
                                            dc.AcceptAction.Execute += AcceptAction_Execute;
                                            showViewParameters.Controllers.Add(dc);
                                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                        }
                                        else if (objPlmExam.FiberType.FiberType == "Non Fiberous Components")
                                        {
                                            CollectionSource cs = new CollectionSource(os, typeof(Non_FibrousComponents));
                                            ListView lv = Application.CreateListView("Non_FibrousComponents_LookupListView", cs, false);
                                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                                            showViewParameters.Context = TemplateContext.PopupWindow;
                                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                            showViewParameters.CreatedView.Caption = objPlmExam.FiberType.FiberType;
                                            DialogController dc = Application.CreateController<DialogController>();
                                            dc.SaveOnAccept = false;
                                            dc.CloseOnCurrentObjectProcessing = false;
                                            dc.Accepting += Dc_Accepting;
                                            dc.AcceptAction.Execute += AcceptAction_Execute;
                                            showViewParameters.Controllers.Add(dc);
                                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                        }
                                    }
                                }
                            }
                        }
                        else if (param[0] == "Entered")
                        {
                            PLMExam currrow = ((ListView)View).CollectionSource.List.Cast<PLMExam>().Where(a => a.Oid == new Guid(param[1])).FirstOrDefault();
                            if (currrow != null)
                            {
                                if (param[2] == "Result")
                                {
                                    if (!string.IsNullOrEmpty(param[3].Trim()) && param[3] != "null")
                                    {
                                        int val = int.Parse(param[3]);
                                        if (val <= 0)
                                        {
                                            currrow.Range = "<1%";
                                        }
                                        else if (val <= 10)
                                        {
                                            currrow.Range = "1-10%";
                                        }
                                        else if (val <= 50)
                                        {
                                            currrow.Range = "11-50%";
                                        }
                                        else if (val <= 90)
                                        {
                                            currrow.Range = "51-90%";
                                        }
                                        else if (val > 90)
                                        {
                                            currrow.Range = ">90%";
                                        }
                                        currrow.Result = param[3];
                                    }
                                    else
                                    {
                                        currrow.Result = null;
                                        currrow.Range = null;
                                    }
                                }
                                else if (param[2] == "FiberType.Oid")
                                {
                                    DashboardViewItem dvPLE = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamDetail") as DashboardViewItem;
                                    if (dvPLE != null && dvPLE.InnerView != null)
                                    {
                                        if (param[3] != "null")
                                        {
                                            FiberTypes Objrefrence = View.ObjectSpace.FindObject<FiberTypes>(CriteriaOperator.Parse("[Oid]=?", new Guid(param[3])));
                                            if (Objrefrence != null)
                                            {
                                                currrow.FiberType = Objrefrence;
                                            }
                                        }
                                        else
                                        {
                                            currrow.FiberType = null;
                                        }
                                        currrow.Name = null;
                                        currrow.Result = null;
                                        currrow.Range = null;
                                        ClearPLMEx((NPPLMExam)dvPLE.InnerView.CurrentObject);
                                    }
                                }
                                ((ListView)View).Refresh();
                            }
                        }
                    }
                }
                else
                {
                    DashboardViewItem lvQcSeq = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("SequenceList") as DashboardViewItem;
                    if (lvQcSeq != null && lvQcSeq.InnerView != null)
                    {
                        DashboardViewItem dvPSO = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMstereoscopicDetail") as DashboardViewItem;
                        if (dvPSO != null && dvPSO.InnerView != null)
                        {
                            PLMStereoscopicObservation curpLM = (PLMStereoscopicObservation)((ListView)lvQcSeq.InnerView).CurrentObject;
                            if (curpLM != null)
                            {
                                ChangeStereo((NPPLMStereoscopicObservation)dvPSO.InnerView.CurrentObject, curpLM);
                                ((ListView)lvQcSeq.InnerView).Refresh();
                                DashboardViewItem lvPlmExam = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamList") as DashboardViewItem;
                                if (lvPlmExam != null && lvPlmExam.InnerView != null)
                                {
                                    DashboardViewItem dvPLE = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamDetail") as DashboardViewItem;
                                    if (dvPLE != null && dvPLE.InnerView != null)
                                    {
                                        foreach (PLMExam pLMExam in ((ListView)lvPlmExam.InnerView).CollectionSource.List.Cast<PLMExam>().ToList())
                                        {
                                            ((ListView)lvPlmExam.InnerView).CollectionSource.Remove(pLMExam);
                                        }
                                        foreach (PLMExam pLM in lvPlmExam.InnerView.ObjectSpace.GetObjects<PLMExam>(CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", curpLM.Oid), true))
                                        {
                                            ((ListView)lvPlmExam.InnerView).CollectionSource.Add(pLM);
                                        }
                                        if (((ListView)lvPlmExam.InnerView).CollectionSource.GetCount() == 0)
                                        {
                                            disenbcontrols(false, (DetailView)dvPLE.InnerView);
                                            ClearPLMEx((NPPLMExam)dvPLE.InnerView.CurrentObject);
                                        }
                                        else
                                        {
                                            disenbcontrols(true, (DetailView)dvPLE.InnerView);
                                            PLMExam curpLMEx = ((ListView)lvPlmExam.InnerView).CollectionSource.List.Cast<PLMExam>().Where(a => a.Oid == ((PLMExam)((ListView)lvPlmExam.InnerView).CollectionSource.List[0]).Oid).FirstOrDefault();
                                            if (curpLMEx != null)
                                            {
                                                ChangePLMEx((NPPLMExam)dvPLE.InnerView.CurrentObject, curpLMEx);
                                            }
                                        }
                                    }
                                    ((ASPxGridListEditor)((ListView)lvPlmExam.InnerView).Editor).Grid.JSProperties["cpRefresh"] = false;
                                    ((ListView)lvPlmExam.InnerView).Refresh();
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dc = (DialogController)sender;
                    if (dc != null)
                    {
                        if (dc.Window.View.SelectedObjects.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
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

        private void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SimpleAction dc = (SimpleAction)sender;
                if (dc != null && e.SelectedObjects.Count == 1)
                {
                    if (HttpContext.Current.Session["rowid"] != null)
                    {
                        PLMExam objAT = ((ListView)View).CollectionSource.List.Cast<PLMExam>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                        if (objAT != null)
                        {
                            DashboardViewItem dvPLE = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMExamDetail") as DashboardViewItem;
                            if (dvPLE != null && dvPLE.InnerView != null)
                            {
                                if (dc.SelectionContext.Name == "Asbestos Fibers")
                                {
                                    objAT.Name = ((AsbestosFibers)e.SelectedObjects[0]).AsbestosFibersName;
                                }
                                else if (dc.SelectionContext.Name == "Non-AsbestosFibers")
                                {
                                    objAT.Name = ((Non_AsbestosFibers)e.SelectedObjects[0]).NonAsbestosFiberName;
                                }
                                else if (dc.SelectionContext.Name == "Other Components")
                                {
                                    objAT.Name = ((OtherComponents)e.SelectedObjects[0]).OtherComponentName;
                                }
                                else if (dc.SelectionContext.Name == "Non Fiberous Components")
                                {
                                    objAT.Name = ((Non_FibrousComponents)e.SelectedObjects[0]).NonFibrousComponenetName;
                                }

                                if (!string.IsNullOrEmpty(objAT.Name) && plmInfo.lstFiberTypesValues != null)
                                {
                                    ClearPLMEx((NPPLMExam)dvPLE.InnerView.CurrentObject);
                                    FiberTypesValues values = plmInfo.lstFiberTypesValues.Where(a => a.FiberType == objAT.Name).FirstOrDefault();
                                    if (values != null)
                                    {
                                        LoadPLMEDVal((NPPLMExam)dvPLE.InnerView.CurrentObject, values);
                                    }
                                }
                                ((ListView)View).Refresh();
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

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                View.ControlsCreated -= View_ControlsCreated;
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    if (HttpContext.Current.Session["FiberTypeName"] != null)
                    {
                        if (View.Id == "AsbestosFibers_LookupListView")
                        {
                            AsbestosFibers AsbestosFibers = View.ObjectSpace.FindObject<AsbestosFibers>(CriteriaOperator.Parse("[AsbestosFibersName]=?", HttpContext.Current.Session["FiberTypeName"].ToString()));
                            if (AsbestosFibers != null)
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(AsbestosFibers.Oid);
                            }
                        }
                        else if (View.Id == "Non_AsbestosFibers_LookupListView")
                        {
                            Non_AsbestosFibers non_AsbestosFibers = View.ObjectSpace.FindObject<Non_AsbestosFibers>(CriteriaOperator.Parse("[NonAsbestosFiberName]=?", HttpContext.Current.Session["FiberTypeName"].ToString()));
                            if (non_AsbestosFibers != null)
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(non_AsbestosFibers.Oid);
                            }
                        }
                        else if (View.Id == "OtherComponents_LookupListView")
                        {
                            OtherComponents otherComponents = View.ObjectSpace.FindObject<OtherComponents>(CriteriaOperator.Parse("[OtherComponentName]=?", HttpContext.Current.Session["FiberTypeName"].ToString()));
                            if (otherComponents != null)
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(otherComponents.Oid);
                            }
                        }
                        else if (View.Id == "Non_FibrousComponents_LookupListView")
                        {
                            Non_FibrousComponents non_FibrousComponents = View.ObjectSpace.FindObject<Non_FibrousComponents>(CriteriaOperator.Parse("[NonFibrousComponenetName]=?", HttpContext.Current.Session["FiberTypeName"].ToString()));
                            if (non_FibrousComponents != null)
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(non_FibrousComponents.Oid);
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

        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string result = savecurdata();
                if (result == string.Empty)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else if (result != "Error")
                {
                    Application.ShowViewStrategy.ShowMessage("A QCBatch ID " + result + " has been created sucessfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CompleteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                savecurdata();
                DashboardViewItem lvQcSeq = ((DashboardView)View).FindItem("SequenceList") as DashboardViewItem;
                if (lvQcSeq != null && lvQcSeq.InnerView != null)
                {
                    PLMStereoscopicObservation observation = (PLMStereoscopicObservation)lvQcSeq.InnerView.CurrentObject;
                    if (observation != null && observation.Qcbatchsequence.qcseqdetail.Status == 1)
                    {
                        observation.Qcbatchsequence.qcseqdetail.Status = 2;
                        DefaultSetting defaultSetting = ((ListView)lvQcSeq.InnerView).ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'Spreadsheet'"));
                        if (defaultSetting != null)
                        {
                            Employee curusr = lvQcSeq.InnerView.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            if (curusr != null)
                            {
                                foreach (SampleParameter sample in ((ListView)lvQcSeq.InnerView).ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail]=?", observation.Qcbatchsequence.qcseqdetail.Oid)))
                                {
                                    sample.EnteredDate = DateTime.Now;
                                    sample.EnteredBy = curusr;
                                    sample.Status = Samplestatus.PendingReview;
                                    sample.OSSync = true;
                                    if (defaultSetting.Review == EnumRELevelSetup.No && defaultSetting.Verify == EnumRELevelSetup.No)
                                    {
                                        observation.Qcbatchsequence.qcseqdetail.Status = 4;
                                        if (defaultSetting.REValidate == EnumRELevelSetup.Yes)
                                        {
                                            sample.Status = Samplestatus.PendingValidation;
                                        }
                                        else if (defaultSetting.REValidate == EnumRELevelSetup.No && defaultSetting.REApprove == EnumRELevelSetup.Yes)
                                        {
                                            sample.Status = Samplestatus.PendingApproval;
                                            sample.ValidatedDate = DateTime.Now;
                                            sample.ValidatedBy = curusr;
                                        }
                                        else if (defaultSetting.REValidate == EnumRELevelSetup.No && defaultSetting.REApprove == EnumRELevelSetup.No)
                                        {
                                            sample.Status = Samplestatus.PendingReporting;
                                            sample.ValidatedDate = DateTime.Now;
                                            sample.ValidatedBy = curusr;
                                            sample.AnalyzedDate = DateTime.Now;
                                            sample.AnalyzedBy = curusr;
                                        }
                                    }
                                    else if (defaultSetting.Review == EnumRELevelSetup.No && defaultSetting.Verify == EnumRELevelSetup.Yes)
                                    {
                                        observation.Qcbatchsequence.qcseqdetail.Status = 3;
                                        sample.Status = Samplestatus.PendingVerify;
                                    }
                                }
                                if (observation.Qcbatchsequence.qcseqdetail.Status == 3)
                                {
                                    observation.Qcbatchsequence.qcseqdetail.ReviewedDate = DateTime.Now;
                                    observation.Qcbatchsequence.qcseqdetail.ReviewedBy = curusr;
                                }
                                else if (observation.Qcbatchsequence.qcseqdetail.Status == 4)
                                {
                                    observation.Qcbatchsequence.qcseqdetail.ReviewedDate = DateTime.Now;
                                    observation.Qcbatchsequence.qcseqdetail.ReviewedBy = curusr;
                                    observation.Qcbatchsequence.qcseqdetail.VerifiedDate = DateTime.Now;
                                    observation.Qcbatchsequence.qcseqdetail.VerifiedBy = curusr;
                                }
                            }
                        }
                        StatusDefinition objStatus = ObjectSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 17"));
                        if (objStatus != null)
                        {
                            string[] ids = observation.Qcbatchsequence.qcseqdetail.Jobid.Split(';');
                            foreach (string obj in ids)
                            {
                                Samplecheckin objSamplecheckin = ((ListView)lvQcSeq.InnerView).ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", obj));
                                if (objSamplecheckin != null)
                                {
                                    IList<SampleParameter> lstSamples = ObjectSpace.GetObjects<SampleParameter>().Where(j => j.Samplelogin != null && j.Samplelogin.JobID != null && j.Samplelogin.JobID.JobID == objSamplecheckin.JobID).ToList();
                                    if (lstSamples.Where(j => j.Status == Samplestatus.PendingEntry).Count() == 0)
                                    {
                                        objSamplecheckin.Index = objStatus;
                                    }
                                }
                            }
                        }
                        ((ListView)lvQcSeq.InnerView).ObjectSpace.CommitChanges();
                        enbactions(false);
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "CompleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Cannot be Completed.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void movecurdata()
        {
            try
            {
                DashboardViewItem lvQcSeq = ((DashboardView)View).FindItem("SequenceList") as DashboardViewItem;
                if (lvQcSeq != null && lvQcSeq.InnerView != null)
                {
                    DashboardViewItem dvPSO = ((DashboardView)View).FindItem("PLMstereoscopicDetail") as DashboardViewItem;
                    if (dvPSO != null && dvPSO.InnerView != null)
                    {
                        PLMStereoscopicObservation curpLM = (PLMStereoscopicObservation)((ListView)lvQcSeq.InnerView).CurrentObject;
                        if (curpLM != null)
                        {
                            ChangeStereo(curpLM, (NPPLMStereoscopicObservation)dvPSO.InnerView.CurrentObject);
                            DashboardViewItem lvPlmExam = ((DashboardView)View).FindItem("PLMExamList") as DashboardViewItem;
                            if (lvPlmExam != null && lvPlmExam.InnerView != null)
                            {
                                DashboardViewItem dvPLE = ((DashboardView)View).FindItem("PLMExamDetail") as DashboardViewItem;
                                if (dvPLE != null && dvPLE.InnerView != null)
                                {
                                    PLMExam pLM = (PLMExam)((ListView)lvPlmExam.InnerView).CurrentObject;
                                    if (pLM != null)
                                    {
                                        ChangePLMEx(pLM, (NPPLMExam)dvPLE.InnerView.CurrentObject);
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

        private string savecurdata()
        {
            try
            {
                string tempab = string.Empty;
                DashboardViewItem lvQcSeq = ((DashboardView)View).FindItem("SequenceList") as DashboardViewItem;
                if (lvQcSeq != null && lvQcSeq.InnerView != null)
                {
                    DashboardViewItem dvPSO = ((DashboardView)View).FindItem("PLMstereoscopicDetail") as DashboardViewItem;
                    if (dvPSO != null && dvPSO.InnerView != null)
                    {
                        PLMStereoscopicObservation curpLM = (PLMStereoscopicObservation)((ListView)lvQcSeq.InnerView).CurrentObject;
                        if (curpLM != null)
                        {
                            ChangeStereo(curpLM, (NPPLMStereoscopicObservation)dvPSO.InnerView.CurrentObject);
                            if (curpLM.Qcbatchsequence != null && curpLM.Qcbatchsequence.qcseqdetail != null && string.IsNullOrEmpty(curpLM.Qcbatchsequence.qcseqdetail.AnalyticalBatchID))
                            {
                                string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                var curdate = DateTime.Now.ToString("yyMMdd");
                                IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = lvQcSeq.InnerView.ObjectSpace.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 6)=?", curdate));
                                if (spreadSheets.Count > 0)
                                {
                                    spreadSheets = spreadSheets.OrderBy(a => a.AnalyticalBatchID).ToList();
                                    tempab = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                                }
                                else
                                {
                                    tempab = "QB" + curdate + "01" + userid;
                                }
                                curpLM.Qcbatchsequence.qcseqdetail.AnalyticalBatchID = tempab;
                                qcbatchinfo.QCBatchOid = curpLM.Qcbatchsequence.qcseqdetail.Oid;
                                PLMDelete.Active.SetItemValue("enb", true);
                            }
                            if (((ListView)lvQcSeq.InnerView).CollectionSource.List.Cast<PLMStereoscopicObservation>().Where(a => a.Qcbatchsequence.Status == PLMDataEnterStatus.PendingResultEntry).Count() == 0)
                            {
                                curpLM.Qcbatchsequence.qcseqdetail.Status = 1;
                            }
                            else
                            {
                                curpLM.Qcbatchsequence.qcseqdetail.Status = 0;
                            }
                            ((ListView)lvQcSeq.InnerView).ObjectSpace.CommitChanges();
                            ((ListView)lvQcSeq.InnerView).Refresh();
                            DashboardViewItem lvPlmExam = ((DashboardView)View).FindItem("PLMExamList") as DashboardViewItem;
                            if (lvPlmExam != null && lvPlmExam.InnerView != null)
                            {
                                DashboardViewItem dvPLE = ((DashboardView)View).FindItem("PLMExamDetail") as DashboardViewItem;
                                if (dvPLE != null && dvPLE.InnerView != null)
                                {
                                    PLMExam pLM = (PLMExam)((ListView)lvPlmExam.InnerView).CurrentObject;
                                    if (pLM != null)
                                    {
                                        ChangePLMEx(pLM, (NPPLMExam)dvPLE.InnerView.CurrentObject);
                                    }
                                }
                                ((ListView)lvPlmExam.InnerView).ObjectSpace.CommitChanges();
                            }
                        }
                    }
                }
                return tempab;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return "Error";
            }
        }

        private void CopyPreviuous_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem lvQcSeq = ((DashboardView)Application.MainWindow.View).FindItem("SequenceList") as DashboardViewItem;
                if (lvQcSeq != null && lvQcSeq.InnerView != null)
                {
                    PLMStereoscopicObservation objCurrentQcSeq = (PLMStereoscopicObservation)((ListView)lvQcSeq.InnerView).CurrentObject;
                    if (objCurrentQcSeq != null)
                    {
                        int curseq = ((ListView)lvQcSeq.InnerView).CollectionSource.List.Cast<PLMStereoscopicObservation>().OrderBy(a => a.Qcbatchsequence.StrSampleL).ToList().IndexOf(objCurrentQcSeq);
                        if (curseq > 0)
                        {
                            PLMStereoscopicObservation objLastQcSeq = ((ListView)lvQcSeq.InnerView).CollectionSource.List.Cast<PLMStereoscopicObservation>().OrderBy(a => a.Qcbatchsequence.StrSampleL).ToList().ElementAt(curseq - 1);
                            if (objLastQcSeq != null)
                            {
                                ChangeStereo(objCurrentQcSeq, objLastQcSeq);
                            }
                            DashboardViewItem lvPlmListView = ((DashboardView)Application.MainWindow.View).FindItem("PLMExamList") as DashboardViewItem;
                            if (lvPlmListView != null && lvPlmListView.InnerView != null)
                            {
                                foreach (PLMExam pLMExam in ((ListView)lvPlmListView.InnerView).CollectionSource.List.Cast<PLMExam>().ToList())
                                {
                                    ((ListView)lvPlmListView.InnerView).CollectionSource.Remove(pLMExam);
                                    ((ListView)lvPlmListView.InnerView).ObjectSpace.Delete(pLMExam);
                                }
                                foreach (PLMExam pLM in lvPlmListView.InnerView.ObjectSpace.GetObjects<PLMExam>(CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", objLastQcSeq.Oid), true))
                                {
                                    PLMExam pLMExam = lvPlmListView.InnerView.ObjectSpace.CreateObject<PLMExam>();
                                    pLMExam.PLMStereoscopicObservation = objCurrentQcSeq.Oid;
                                    ChangePLMEx(pLMExam, pLM);
                                }
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "copysuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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

        private void CopyToAll_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem lvQcSeq = ((DashboardView)Application.MainWindow.View).FindItem("SequenceList") as DashboardViewItem;
                DashboardViewItem lvPlmListView = ((DashboardView)Application.MainWindow.View).FindItem("PLMExamList") as DashboardViewItem;
                if (lvQcSeq != null && lvQcSeq.InnerView != null && lvPlmListView != null && lvPlmListView.InnerView != null)
                {
                    PLMStereoscopicObservation objCurrentQcSeq = (PLMStereoscopicObservation)((ListView)lvQcSeq.InnerView).CurrentObject;
                    if (objCurrentQcSeq != null)
                    {
                        foreach (PLMStereoscopicObservation objQcSeq in ((ListView)lvQcSeq.InnerView).CollectionSource.List.Cast<PLMStereoscopicObservation>().Where(a => a.Oid != objCurrentQcSeq.Oid))
                        {
                            foreach (PLMExam pLM in lvPlmListView.InnerView.ObjectSpace.GetObjects<PLMExam>(CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", objQcSeq.Oid), true).ToList())
                            {
                                ((ListView)lvPlmListView.InnerView).ObjectSpace.Delete(pLM);
                            }
                            ChangeStereo(objQcSeq, objCurrentQcSeq);
                            foreach (PLMExam pLM in lvPlmListView.InnerView.ObjectSpace.GetObjects<PLMExam>(CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", objCurrentQcSeq.Oid), true))
                            {
                                PLMExam pLMExam = lvPlmListView.InnerView.ObjectSpace.CreateObject<PLMExam>();
                                pLMExam.PLMStereoscopicObservation = objQcSeq.Oid;
                                ChangePLMEx(pLMExam, pLM);
                            }
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "copysuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Copy_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                PLMCopyToCombo combo = (PLMCopyToCombo)View.CurrentObject;
                if (combo != null && !string.IsNullOrEmpty(combo.SourceSample) && !string.IsNullOrEmpty(combo.CopyTo))
                {
                    DashboardViewItem lvQcSeq = ((DashboardView)Application.MainWindow.View).FindItem("SequenceList") as DashboardViewItem;
                    DashboardViewItem lvPlmListView = ((DashboardView)Application.MainWindow.View).FindItem("PLMExamList") as DashboardViewItem;
                    if (lvQcSeq != null && lvQcSeq.InnerView != null && lvPlmListView != null && lvPlmListView.InnerView != null)
                    {
                        PLMStereoscopicObservation objCurrentQcSeq = ((ListView)lvQcSeq.InnerView).CollectionSource.List.Cast<PLMStereoscopicObservation>().Where(a => a.Qcbatchsequence.StrSampleL == combo.SourceSample).FirstOrDefault();
                        if (objCurrentQcSeq != null)
                        {
                            foreach (PLMStereoscopicObservation objQcSeq in ((ListView)lvQcSeq.InnerView).CollectionSource.List.Cast<PLMStereoscopicObservation>().Where(a => combo.CopyTo.Split(new string[] { "; " }, StringSplitOptions.None).Contains(a.Oid.ToString())))
                            {
                                foreach (PLMExam pLM in lvPlmListView.InnerView.ObjectSpace.GetObjects<PLMExam>(CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", objQcSeq.Oid), true).ToList())
                                {
                                    ((ListView)lvPlmListView.InnerView).ObjectSpace.Delete(pLM);
                                }
                                ChangeStereo(objQcSeq, objCurrentQcSeq);
                                foreach (PLMExam pLM in lvPlmListView.InnerView.ObjectSpace.GetObjects<PLMExam>(CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", objCurrentQcSeq.Oid), true))
                                {
                                    PLMExam pLMExam = lvPlmListView.InnerView.ObjectSpace.CreateObject<PLMExam>();
                                    pLMExam.PLMStereoscopicObservation = objQcSeq.Oid;
                                    ChangePLMEx(pLMExam, pLM);
                                }
                            }
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "copysuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            ClearPLM.DoExecute();
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

        private void ClearPLM_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                PLMCopyToCombo combo = (PLMCopyToCombo)View.CurrentObject;
                if (combo != null)
                {
                    combo.SourceSample = null;
                    combo.CopyTo = null;
                    ASPxCheckedLookupStringPropertyEditor CopyTo = ((DetailView)View).FindItem("CopyTo") as ASPxCheckedLookupStringPropertyEditor;
                    if (CopyTo != null)
                    {
                        ASPxGridLookup lookup = (ASPxGridLookup)CopyTo.Editor;
                        if (lookup != null)
                        {
                            lookup.Value = null;
                        }
                    }
                }
                View.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CarryOverResults_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                movecurdata();
                DashboardViewItem lvSeqList = ((DashboardView)View).FindItem("SequenceList") as DashboardViewItem;
                if (lvSeqList != null && lvSeqList.InnerView != null)
                {
                    plmInfo.lstPLMSte = new Dictionary<object, string>();
                    foreach (PLMStereoscopicObservation pLM in ((ListView)lvSeqList.InnerView).CollectionSource.List)
                    {
                        plmInfo.lstPLMSte.Add(pLM.Oid, pLM.Qcbatchsequence.StrSampleL);
                    }
                }
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                PLMCopyToCombo objView = objectSpace.CreateObject<PLMCopyToCombo>();
                DetailView dv = Application.CreateDetailView(objectSpace, "PLMCopyToCombo_DetailView", true, objView);
                dv.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters();
                showViewParameters.CreatedView = dv;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.CancelAction.Active.SetItemValue("Cancel", false);
                dc.AcceptAction.Active.SetItemValue("Ok", false);
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void disenbcontrols(bool enbstat, DevExpress.ExpressApp.View view, bool all = false)
        {
            try
            {
                foreach (ViewItem item in ((DetailView)view).Items)
                {
                    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                    {
                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(EnumRadioButtonPropertyEditor))
                    {
                        EnumRadioButtonPropertyEditor propertyEditor = item as EnumRadioButtonPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                    {
                        ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxBooleanPropertyEditor))
                    {
                        ASPxBooleanPropertyEditor propertyEditor = item as ASPxBooleanPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null && (all || all == false && propertyEditor.Id != "PositiveStop"))
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                    {
                        ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null && all)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
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

        private void PLMDelete_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (qcbatchinfo.QCBatchOid != null)
                {
                    SpreadSheetEntry_AnalyticalBatch batch = View.ObjectSpace.GetObjectByKey<SpreadSheetEntry_AnalyticalBatch>(qcbatchinfo.QCBatchOid);
                    if (batch != null)
                    {
                        if (batch.NonStatus != AnalyticalBatchStatus.PendingResultEntry || batch.NonStatus != AnalyticalBatchStatus.PendingCompletion)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "QCnotdelete"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }

                        IList<QCBatchSequence> objqCBatch = View.ObjectSpace.GetObjects<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail]=?", batch.Oid));
                        View.ObjectSpace.Delete(objqCBatch);

                        IList<SampleParameter> objsampleParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail]=?", batch.Oid));
                        foreach (SampleParameter sample in objsampleParameters.ToList())
                        {
                            if (sample.QCBatchID != null && sample.QCBatchID.QCType != null && sample.QCBatchID.QCType.QCTypeName != "Sample")
                            {
                                View.ObjectSpace.Delete(sample);
                            }
                            else
                            {
                                sample.UQABID = null;
                                sample.QCBatchID = null;
                                sample.QCSort = 0;
                            }
                        }

                        IList<PLMStereoscopicObservation> objPLMStereos = ObjectSpace.GetObjects<PLMStereoscopicObservation>(CriteriaOperator.Parse("[Qcbatchsequence.qcseqdetail]=?", batch.Oid));
                        foreach (PLMStereoscopicObservation pLM in objPLMStereos.Cast<PLMStereoscopicObservation>().ToList())
                        {
                            IList<PLMExam> objPLMs = ObjectSpace.GetObjects<PLMExam>(CriteriaOperator.Parse("[PLMStereoscopicObservation]=?", pLM.Oid));
                            View.ObjectSpace.Delete(objPLMs);
                            View.ObjectSpace.Delete(pLM);
                        }

                        View.ObjectSpace.Delete(batch);
                        View.ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        #region supporting functions

        private void convertqcos(IObjectSpace os)
        {
            try
            {
                SpreadSheetEntry_AnalyticalBatch batch = os.CreateObject<SpreadSheetEntry_AnalyticalBatch>();
                batch.Humidity = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Humidity;
                batch.Roomtemp = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Roomtemp;
                batch.Instrument = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument;
                batch.Jobid = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Jobid;
                batch.Matrix = os.FindObject<Matrix>(CriteriaOperator.Parse("[Oid]=?", objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Matrix.Oid));
                batch.Method = os.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Method.Oid));
                batch.Test = batch.Method;
                batch.Comments = objABinfo.Comments;
                batch.Status = 1;
                string[] ids = batch.Instrument.Split(';');
                foreach (string id in ids)
                {
                    QCBatchInstrument qcinstrument = os.CreateObject<QCBatchInstrument>();
                    qcinstrument.QCBatchID = batch;
                    qcinstrument.LabwareID = os.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", new Guid(id.Replace(" ", ""))));
                }

                int sort = 0;
                foreach (QCBatchSequence curseq in objABinfo.lstQCBatchSequence.OrderBy(i => i.Sort).ToList())
                {
                    QCBatchSequence sequence = os.CreateObject<QCBatchSequence>();
                    sequence.SYSSamplecode = curseq.SYSSamplecode;
                    sequence.QCType = os.FindObject<QCType>(CriteriaOperator.Parse("[Oid]=?", curseq.QCType.Oid));
                    sequence.Runno = curseq.Runno;
                    sequence.SampleAmount = curseq.SampleAmount;
                    sequence.Sort = curseq.Sort;
                    sequence.StrSampleID = curseq.StrSampleID;
                    sequence.StrSampleL = curseq.StrSampleL;
                    sequence.SystemID = curseq.SystemID;
                    sequence.batchno = curseq.batchno;
                    sequence.LayerCount = curseq.LayerCount;
                    sequence.Status = PLMDataEnterStatus.PendingResultEntry;
                    if (curseq.SampleID != null)
                    {
                        sequence.SampleID = os.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[Oid]=?", curseq.SampleID.Oid));
                    }
                    sequence.qcseqdetail = batch;
                    sequence.IsReport = true;

                    if (sequence.QCType != null && sequence.QCType.QCTypeName != "Sample")
                    {
                        IList<Testparameter> testparams = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.Oid]=? and [TestMethod.Oid]=?", sequence.QCType.Oid, batch.Test.Oid));
                        foreach (Testparameter testparam in testparams.OrderBy(a => a.Parameter.ParameterName).ToList())
                        {
                            SampleParameter newsample = os.CreateObject<SampleParameter>();
                            newsample.QCBatchID = sequence;
                            newsample.Testparameter = testparam;
                            newsample.QCSort = sort;
                            newsample.SignOff = true;
                            newsample.UQABID = batch;
                            sort++;
                        }
                    }
                    else
                    {
                        IList<SampleParameter> sampleparams = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? And [SignOff] = True", sequence.SampleID.SampleID, batch.Test.Oid));
                        foreach (SampleParameter sampleparam in sampleparams.OrderBy(a => a.Testparameter.Sort).ThenBy(a => a.Testparameter.Parameter.ParameterName).ToList())
                        {
                            sampleparam.QCBatchID = sequence;
                            sampleparam.QCSort = sort;
                            sampleparam.UQABID = batch;
                            sort++;
                        }
                    }

                    PLMStereoscopicObservation pLM = View.ObjectSpace.CreateObject<PLMStereoscopicObservation>();
                    pLM.Qcbatchsequence = sequence;
                    ((ListView)View).CollectionSource.Add(pLM);
                }
                objABinfo.lstQCBatchSequence = new List<QCBatchSequence>();
                objABinfo.lstSpreadSheetEntry_AnalyticalBatch = null;
                objABinfo.Comments = null;
                ((ListView)View).Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void LoadPLMEDVal(NPPLMExam val, FiberTypesValues newval)
        {
            try
            {
                if (!string.IsNullOrEmpty(newval.Isotropic))
                {
                    if (newval.Isotropic.ToLower().Trim() == "yes")
                    {
                        val.Type_Isotropic = true;
                    }
                }

                if (!string.IsNullOrEmpty(newval.Pleochroism))
                {
                    val.Color = newval.Pleochroism;
                }

                if (!string.IsNullOrEmpty(newval.Morphology))
                {
                    string[] Morphology = newval.Morphology.Split('/');
                    if (Morphology.Length > 0)
                    {
                        if (Morphology.Contains("Bundles"))
                        {
                            val.Morphology_Bundles = true;
                        }
                        if (Morphology.Contains("Curved"))
                        {
                            val.Morphology_Curved = true;
                        }
                        if (Morphology.Contains("Ribbon"))
                        {
                            val.Morphology_Ribbon = true;
                        }
                        if (Morphology.Contains("Splayed"))
                        {
                            val.Morphology_Splayed = true;
                        }
                        if (Morphology.Contains("Straight"))
                        {
                            val.Morphology_Straight = true;
                        }
                        if (Morphology.Contains("Wavey"))
                        {
                            val.Morphology_Wavey = true;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(newval.Extinction))
                {
                    string[] Extinction = newval.Extinction.Split('/');
                    if (Extinction.Length > 0)
                    {
                        if (Extinction.Contains("Oblique"))
                        {
                            val.Extinction_Oblique = true;
                        }
                        if (Extinction.Contains("Parallel"))
                        {
                            val.Extinction_Parallel = true;
                        }
                        if (Extinction.Contains("Undulose"))
                        {
                            val.Extinction_Undulose = true;
                        }
                        if (Extinction.Contains("Wavey"))
                        {
                            val.Extinction_Wavey = true;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(newval.Elognation))
                {
                    if (newval.Elognation.ToLower().Trim() == "positive")
                    {
                        val.Elonggation_Positive = true;
                    }
                    else if (newval.Elognation.ToLower().Trim() == "negative")
                    {
                        val.Elonggation_Negative = true;
                    }
                    else if (newval.Elognation.ToLower().Trim() == "both")
                    {
                        val.Elonggation_Both = true;
                    }
                }

                if (!string.IsNullOrEmpty(newval.RIGamma))
                {
                    val.Gamma = newval.RIGamma;
                }
                if (!string.IsNullOrEmpty(newval.RIAlpha))
                {
                    val.Alpha = newval.RIAlpha;
                }
                if (!string.IsNullOrEmpty(newval.GammaPara))
                {
                    val.LambdaPara = newval.GammaPara;
                }
                if (!string.IsNullOrEmpty(newval.GammaPrep))
                {
                    val.LambdaPerb = newval.GammaPrep;
                }
                if (!string.IsNullOrEmpty(newval.RI))
                {
                    val.RIOil = newval.RI;
                }

                if (!string.IsNullOrEmpty(newval.Parallel))
                {
                    val.Parallel = newval.Parallel;
                }
                if (!string.IsNullOrEmpty(newval.Perpenducular))
                {
                    val.Perpendicular = newval.Perpenducular;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChangePLMEx(NPPLMExam NPval, PLMExam val)
        {
            try
            {
                NPval.Type_Anisotropic = val.Type_Anisotropic;
                NPval.Type_Isotropic = val.Type_Isotropic;

                NPval.Pleochroism_Yes = val.Pleochroism_Yes;
                NPval.Pleochroism_No = val.Pleochroism_No;
                NPval.Color = val.Color;

                NPval.Morphology_Bundles = val.Morphology_Bundles;
                NPval.Morphology_Curved = val.Morphology_Curved;
                NPval.Morphology_Ribbon = val.Morphology_Ribbon;
                NPval.Morphology_Splayed = val.Morphology_Splayed;
                NPval.Morphology_Straight = val.Morphology_Straight;
                NPval.Morphology_Wavey = val.Morphology_Wavey;

                NPval.Extinction_Oblique = val.Extinction_Oblique;
                NPval.Extinction_Parallel = val.Extinction_Parallel;
                NPval.Extinction_Undulose = val.Extinction_Undulose;
                NPval.Extinction_Wavey = val.Extinction_Wavey;

                NPval.Elonggation_Negative = val.Elonggation_Negative;
                NPval.Elonggation_Both = val.Elonggation_Both;
                NPval.Elonggation_Positive = val.Elonggation_Positive;

                NPval.Gamma = val.Gamma;
                NPval.Alpha = val.Alpha;
                NPval.LambdaPara = val.LambdaPara;
                NPval.LambdaPerb = val.LambdaPerb;
                NPval.RIOil = val.RIOil;

                NPval.Parallel = val.Parallel;
                NPval.Perpendicular = val.Perpendicular;

                plmInfo.LastPLMEx = val;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChangePLMEx(PLMExam val, NPPLMExam NPval)
        {
            try
            {
                val.Type_Anisotropic = NPval.Type_Anisotropic;
                val.Type_Isotropic = NPval.Type_Isotropic;

                val.Pleochroism_Yes = NPval.Pleochroism_Yes;
                val.Pleochroism_No = NPval.Pleochroism_No;
                val.Color = NPval.Color;

                val.Morphology_Bundles = NPval.Morphology_Bundles;
                val.Morphology_Curved = NPval.Morphology_Curved;
                val.Morphology_Ribbon = NPval.Morphology_Ribbon;
                val.Morphology_Splayed = NPval.Morphology_Splayed;
                val.Morphology_Straight = NPval.Morphology_Straight;
                val.Morphology_Wavey = NPval.Morphology_Wavey;

                val.Extinction_Oblique = NPval.Extinction_Oblique;
                val.Extinction_Parallel = NPval.Extinction_Parallel;
                val.Extinction_Undulose = NPval.Extinction_Undulose;
                val.Extinction_Wavey = NPval.Extinction_Wavey;

                val.Elonggation_Negative = NPval.Elonggation_Negative;
                val.Elonggation_Both = NPval.Elonggation_Both;
                val.Elonggation_Positive = NPval.Elonggation_Positive;

                val.Gamma = NPval.Gamma;
                val.Alpha = NPval.Alpha;
                val.LambdaPara = NPval.LambdaPara;
                val.LambdaPerb = NPval.LambdaPerb;
                val.RIOil = NPval.RIOil;

                val.Parallel = NPval.Parallel;
                val.Perpendicular = NPval.Perpendicular;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChangePLMEx(PLMExam val, PLMExam newval)
        {
            try
            {
                val.FiberType = newval.FiberType;
                val.Name = newval.Name;
                val.Result = newval.Result;
                val.Range = newval.Range;

                val.Type_Anisotropic = newval.Type_Anisotropic;
                val.Type_Isotropic = newval.Type_Isotropic;

                val.Pleochroism_Yes = newval.Pleochroism_Yes;
                val.Pleochroism_No = newval.Pleochroism_No;
                val.Color = newval.Color;

                val.Morphology_Bundles = newval.Morphology_Bundles;
                val.Morphology_Curved = newval.Morphology_Curved;
                val.Morphology_Ribbon = newval.Morphology_Ribbon;
                val.Morphology_Splayed = newval.Morphology_Splayed;
                val.Morphology_Straight = newval.Morphology_Straight;
                val.Morphology_Wavey = newval.Morphology_Wavey;

                val.Extinction_Oblique = newval.Extinction_Oblique;
                val.Extinction_Parallel = newval.Extinction_Parallel;
                val.Extinction_Undulose = newval.Extinction_Undulose;
                val.Extinction_Wavey = newval.Extinction_Wavey;

                val.Elonggation_Negative = newval.Elonggation_Negative;
                val.Elonggation_Both = newval.Elonggation_Both;
                val.Elonggation_Positive = newval.Elonggation_Positive;

                val.Gamma = newval.Gamma;
                val.Alpha = newval.Alpha;
                val.LambdaPara = newval.LambdaPara;
                val.LambdaPerb = newval.LambdaPerb;
                val.RIOil = newval.RIOil;

                val.Parallel = newval.Parallel;
                val.Perpendicular = newval.Perpendicular;

                val.Sort = newval.Sort;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ClearPLMEx(NPPLMExam obj)
        {
            try
            {
                obj.Type_Anisotropic = false;
                obj.Type_Isotropic = false;

                obj.Pleochroism_Yes = false;
                obj.Pleochroism_No = false;
                obj.Color = null;

                obj.Morphology_Bundles = false;
                obj.Morphology_Curved = false;
                obj.Morphology_Ribbon = false;
                obj.Morphology_Splayed = false;
                obj.Morphology_Straight = false;
                obj.Morphology_Wavey = false;

                obj.Extinction_Oblique = false;
                obj.Extinction_Parallel = false;
                obj.Extinction_Undulose = false;
                obj.Extinction_Wavey = false;

                obj.Elonggation_Negative = false;
                obj.Elonggation_Both = false;
                obj.Elonggation_Positive = false;

                obj.Gamma = null;
                obj.Alpha = null;
                obj.LambdaPara = null;
                obj.LambdaPerb = null;
                obj.RIOil = null;

                obj.Parallel = null;
                obj.Perpendicular = null;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void ChangeStereo(NPPLMStereoscopicObservation NPval, PLMStereoscopicObservation val)
        {
            try
            {
                DashboardViewItem dvPSO = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("PLMstereoscopicDetail") as DashboardViewItem;
                if (dvPSO != null && dvPSO.InnerView != null)
                {
                    ASPxCheckedLookupStringPropertyEditor Mat = ((DetailView)dvPSO.InnerView).FindItem("Material") as ASPxCheckedLookupStringPropertyEditor;
                    if (Mat != null)
                    {
                        ASPxGridLookup lookup = (ASPxGridLookup)Mat.Editor;
                        if (lookup != null)
                        {
                            lookup.Value = null;
                        }
                    }
                }

                NPval.Material = val.Material;
                NPval.PositiveStop = val.PositiveStop;

                NPval.Fibrous = val.Fibrous;
                NPval.NonFibrous = val.NonFibrous;

                NPval.Homogeneous = val.Homogeneous;
                NPval.NonHomogenerous = val.NonHomogenerous;
                NPval.Layered = val.Layered;
                NPval.OtherHomogenity = val.OtherHomogenity;
                NPval.HomogenityText = val.HomogenityText;

                NPval.Color_Beige = val.Color_Beige;
                NPval.Color_Black = val.Color_Black;
                NPval.Color_Blue = val.Color_Blue;
                NPval.Color_Brown = val.Color_Brown;
                NPval.Color_Clear = val.Color_Clear;
                NPval.Color_Gray = val.Color_Gray;
                NPval.Color_Green = val.Color_Green;
                NPval.Color_Orange = val.Color_Orange;
                NPval.Color_Pink = val.Color_Pink;
                NPval.Color_Red = val.Color_Red;
                NPval.Color_Silver = val.Color_Silver;
                NPval.Color_Tan = val.Color_Tan;
                NPval.Color_Violet = val.Color_Violet;
                NPval.Color_White = val.Color_White;
                NPval.Color_Yellow = val.Color_Yellow;
                NPval.Color_Other = val.Color_Other;
                NPval.Color_Text = val.Color_Text;

                NPval.FibersPresent = val.FibersPresent;

                if (val.Friability == null)
                {
                    NPval.Friability = Friability.Dummy;
                }
                else
                {
                    NPval.Friability = val.Friability;
                }

                NPval.SampleTreatment_AcidTreated = val.SampleTreatment_AcidTreated;
                NPval.SampleTreatment_Ashed = val.SampleTreatment_Ashed;
                NPval.SampleTreatment_HCL = val.SampleTreatment_HCL;
                NPval.SampleTreatment_Ground = val.SampleTreatment_Ground;
                NPval.SampleTreatment_Matrix = val.SampleTreatment_Matrix;
                NPval.SampleTreatment_NonFriable = val.SampleTreatment_NonFriable;
                NPval.SampleTreatment_SolventTreated = val.SampleTreatment_SolventTreated;
                NPval.SampleTreatment_Teased = val.SampleTreatment_Teased;
                NPval.SampleTreatment_Other = val.SampleTreatment_Other;

                NPval.FiberObservation = val.FiberObservation;
                NPval.NoAsbestosDetected = val.NoAsbestosDetected;

                plmInfo.LastPLMSte = val;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChangeStereo(PLMStereoscopicObservation val, NPPLMStereoscopicObservation NPval)
        {
            try
            {
                val.Material = NPval.Material;
                val.PositiveStop = NPval.PositiveStop;

                val.Fibrous = NPval.Fibrous;
                val.NonFibrous = NPval.NonFibrous;

                val.Homogeneous = NPval.Homogeneous;
                val.NonHomogenerous = NPval.NonHomogenerous;
                val.Layered = NPval.Layered;
                val.OtherHomogenity = NPval.OtherHomogenity;
                val.HomogenityText = NPval.HomogenityText;

                val.Color_Beige = NPval.Color_Beige;
                val.Color_Black = NPval.Color_Black;
                val.Color_Blue = NPval.Color_Blue;
                val.Color_Brown = NPval.Color_Brown;
                val.Color_Clear = NPval.Color_Clear;
                val.Color_Gray = NPval.Color_Gray;
                val.Color_Green = NPval.Color_Green;
                val.Color_Orange = NPval.Color_Orange;
                val.Color_Pink = NPval.Color_Pink;
                val.Color_Red = NPval.Color_Red;
                val.Color_Silver = NPval.Color_Silver;
                val.Color_Tan = NPval.Color_Tan;
                val.Color_Violet = NPval.Color_Violet;
                val.Color_White = NPval.Color_White;
                val.Color_Yellow = NPval.Color_Yellow;
                val.Color_Other = NPval.Color_Other;
                val.Color_Text = NPval.Color_Text;

                val.FibersPresent = NPval.FibersPresent;

                val.Friability = NPval.Friability;

                val.SampleTreatment_AcidTreated = NPval.SampleTreatment_AcidTreated;
                val.SampleTreatment_Ashed = NPval.SampleTreatment_Ashed;
                val.SampleTreatment_HCL = NPval.SampleTreatment_HCL;
                val.SampleTreatment_Ground = NPval.SampleTreatment_Ground;
                val.SampleTreatment_Matrix = NPval.SampleTreatment_Matrix;
                val.SampleTreatment_NonFriable = NPval.SampleTreatment_NonFriable;
                val.SampleTreatment_SolventTreated = NPval.SampleTreatment_SolventTreated;
                val.SampleTreatment_Teased = NPval.SampleTreatment_Teased;
                val.SampleTreatment_Other = NPval.SampleTreatment_Other;

                val.FiberObservation = NPval.FiberObservation;
                val.NoAsbestosDetected = NPval.NoAsbestosDetected;

                if (!string.IsNullOrEmpty(val.Material))
                {
                    val.Qcbatchsequence.Status = PLMDataEnterStatus.Entered;
                }
                else
                {
                    val.Qcbatchsequence.Status = PLMDataEnterStatus.PendingResultEntry;
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ChangeStereo(PLMStereoscopicObservation val, PLMStereoscopicObservation newval)
        {
            try
            {
                val.Material = newval.Material;
                val.PositiveStop = newval.PositiveStop;

                val.Fibrous = newval.Fibrous;
                val.NonFibrous = newval.NonFibrous;

                val.Homogeneous = newval.Homogeneous;
                val.NonHomogenerous = newval.NonHomogenerous;
                val.Layered = newval.Layered;
                val.OtherHomogenity = newval.OtherHomogenity;
                val.HomogenityText = newval.HomogenityText;

                val.Color_Beige = newval.Color_Beige;
                val.Color_Black = newval.Color_Black;
                val.Color_Blue = newval.Color_Blue;
                val.Color_Brown = newval.Color_Brown;
                val.Color_Clear = newval.Color_Clear;
                val.Color_Gray = newval.Color_Gray;
                val.Color_Green = newval.Color_Green;
                val.Color_Orange = newval.Color_Orange;
                val.Color_Pink = newval.Color_Pink;
                val.Color_Red = newval.Color_Red;
                val.Color_Silver = newval.Color_Silver;
                val.Color_Tan = newval.Color_Tan;
                val.Color_Violet = newval.Color_Violet;
                val.Color_White = newval.Color_White;
                val.Color_Yellow = newval.Color_Yellow;
                val.Color_Other = newval.Color_Other;
                val.Color_Text = newval.Color_Text;

                val.FibersPresent = newval.FibersPresent;

                val.Friability = newval.Friability;

                val.SampleTreatment_AcidTreated = newval.SampleTreatment_AcidTreated;
                val.SampleTreatment_Ashed = newval.SampleTreatment_Ashed;
                val.SampleTreatment_HCL = newval.SampleTreatment_HCL;
                val.SampleTreatment_Ground = newval.SampleTreatment_Ground;
                val.SampleTreatment_Matrix = newval.SampleTreatment_Matrix;
                val.SampleTreatment_NonFriable = newval.SampleTreatment_NonFriable;
                val.SampleTreatment_SolventTreated = newval.SampleTreatment_SolventTreated;
                val.SampleTreatment_Teased = newval.SampleTreatment_Teased;
                val.SampleTreatment_Other = newval.SampleTreatment_Other;

                val.FiberObservation = newval.FiberObservation;
                val.NoAsbestosDetected = newval.NoAsbestosDetected;

                if (!string.IsNullOrEmpty(val.Material))
                {
                    val.Qcbatchsequence.Status = PLMDataEnterStatus.Entered;
                }
                else
                {
                    val.Qcbatchsequence.Status = PLMDataEnterStatus.PendingResultEntry;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ClearStereo(NPPLMStereoscopicObservation obj)
        {
            try
            {
                //obj.Material = null;
                //obj.PositiveStop = false;

                obj.Fibrous = false;
                obj.NonFibrous = false;

                obj.Homogeneous = false;
                obj.NonHomogenerous = false;
                obj.Layered = false;
                obj.OtherHomogenity = false;
                obj.HomogenityText = null;

                obj.Color_Beige = false;
                obj.Color_Black = false;
                obj.Color_Blue = false;
                obj.Color_Brown = false;
                obj.Color_Clear = false;
                obj.Color_Gray = false;
                obj.Color_Green = false;
                obj.Color_Orange = false;
                obj.Color_Pink = false;
                obj.Color_Red = false;
                obj.Color_Silver = false;
                obj.Color_Tan = false;
                obj.Color_Violet = false;
                obj.Color_White = false;
                obj.Color_Yellow = false;
                obj.Color_Other = false;
                obj.Color_Text = null;

                obj.FibersPresent = false;

                obj.Friability = Friability.Dummy;

                obj.SampleTreatment_AcidTreated = false;
                obj.SampleTreatment_Ashed = false;
                obj.SampleTreatment_HCL = false;
                obj.SampleTreatment_Ground = false;
                obj.SampleTreatment_Matrix = false;
                obj.SampleTreatment_NonFriable = false;
                obj.SampleTreatment_SolventTreated = false;
                obj.SampleTreatment_Teased = false;
                obj.SampleTreatment_Other = false;

                obj.FiberObservation = null;
                obj.NoAsbestosDetected = false;
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
