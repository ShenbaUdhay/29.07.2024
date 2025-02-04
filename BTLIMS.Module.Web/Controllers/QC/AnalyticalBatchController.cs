﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Web;
using DevExpress.Web.ASPxRichEdit;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LDM.Module.Web.Controllers.QC
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AnalyticalBatchController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        AnalyticalBatchInfo analyticalbatchinfo = new AnalyticalBatchInfo();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        CaseNarativeInfo CNInfo = new CaseNarativeInfo();       

        public AnalyticalBatchController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView;" + "SpreadSheetEntry_AnalyticalBatch_DetailView_Copy;"
                + "QCType_ListView_analyticalbatchsequence;" + "QCBatchSequence_ListView_Copy;" + "SpreadSheetEntry_AnalyticalBatch_DetailView_History;"
                + "SpreadSheetEntry_AnalyticalBatch_ListView_History;" + "Notes_ListView_CaseNarrative_Copy_QCBatch;";
            ABload.TargetViewId = "AnalyticalBatchsequence;";
            ABprevious.TargetViewId = "AnalyticalBatchsequence;";
            ABreset.TargetViewId = "AnalyticalBatchsequence;";
            ABsort.TargetViewId = "AnalyticalBatchsequence;";
            OpenSDMS.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_DetailView_Copy;";
            OpenSDMS.TargetObjectsCriteria = " [TemplateID] > 0";
            ABHistorys.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView;";
            ABHistoryDateFilter.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_History;";
            Comment.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView_History;"+ "SpreadSheetEntry_AnalyticalBatch_ListView;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ABload.Enabled.SetItemValue("key", false);
                ABprevious.Enabled.SetItemValue("key", false);
                ABreset.Enabled.SetItemValue("key", false);
                ABsort.Enabled.SetItemValue("key", false);
                // Perform various tasks depending on the target View.
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_DetailView_Copy" || View.Id == "SpreadSheetEntry_AnalyticalBatch_DetailView_History")
                {
                    ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Comments") as ASPxRichTextPropertyEditor;
                    if (RichText != null)
                    {
                        RichText.ControlCreated += RichText_ControlCreated;
                    }
                    analyticalbatchinfo.QCTypeIsLoaded = false;
                    analyticalbatchinfo.SeqIsLoaded = false;
                    DashboardViewItem dvQCType = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                    if (dvQCType != null)
                    {
                        dvQCType.ControlCreated += DvQCType_ControlCreated;
                    }
                    DashboardViewItem dvQCList = ((DetailView)View).FindItem("qclist") as DashboardViewItem;
                    if (dvQCList != null)
                    {
                        dvQCList.ControlCreated += DvQCList_ControlCreated;
                    }
                    SpreadSheetEntry_AnalyticalBatch objAB = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                    ListPropertyEditor dvInstrument = ((DetailView)View).FindItem("Instruments") as ListPropertyEditor;
                    ListPropertyEditor dvReagents = ((DetailView)View).FindItem("Reagents") as ListPropertyEditor;
                    if (objAB != null && objAB.Status == 4)
                    {
                        if (RichText != null)
                        {
                            RichText.AllowEdit["Read-Only"] = false;
                        }
                        if (dvInstrument != null)
                        {
                            dvInstrument.AllowEdit["Read-Only"] = false;
                        }
                        if (dvReagents != null)
                        {
                            dvReagents.AllowEdit["Read-Only"] = false;
                        }
                    }
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_History")
                {
                    //ABHistoryDateFilter.SelectedIndex = 1;
                    //((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate,Now())<=3 And [CreatedDate] Is Not Null");
                    //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [NonStatus] <> ? And  [NonStatus] <> ?", AnalyticalBatchStatus.PendingCompletion,AnalyticalBatchStatus.PendingResultEntry);
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (ABHistoryDateFilter.SelectedItem == null)
                    {
                        if (setting.AnalysisEntryModel == EnumDateFilter.OneMonth)
                        {
                            ABHistoryDateFilter.SelectedItem = ABHistoryDateFilter.Items[0];
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate,Now())<=1 And [CreatedDate] Is Not Null");
                            //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);

                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.ThreeMonth)
                        {
                            ABHistoryDateFilter.SelectedItem = ABHistoryDateFilter.Items[1];
                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate,Now())<=3 And [CreatedDate] Is Not Null");
                   // ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.SixMonth)
                        {
                            ABHistoryDateFilter.SelectedItem = ABHistoryDateFilter.Items[2];
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate,Now())<=6 And [CreatedDate] Is Not Null");
                            //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);

                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.OneYear)
                        {
                            ABHistoryDateFilter.SelectedItem = ABHistoryDateFilter.Items[3];
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate,Now())<=1 And [CreatedDate] Is Not Null");
                           // ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);

                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.TwoYear)
                        {
                            ABHistoryDateFilter.SelectedItem = ABHistoryDateFilter.Items[4];
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate,Now())<=2 And [CreatedDate] Is Not Null");
                            //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);

                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.FiveYear)
                        {
                            ABHistoryDateFilter.SelectedItem = ABHistoryDateFilter.Items[5];
                            ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate,Now())<=5 And [CreatedDate] Is Not Null");
                            //((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);

                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.All)
                        {
                            ABHistoryDateFilter.SelectedItem = ABHistoryDateFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                          //  ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleParameterStatus.TestHold] = False And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);

                        }
                    }                   

                    bool Administrator = false;
                    List<Guid> lstTestMethodOid = new List<Guid>();
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        Administrator = true;
                    }
                    else
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        //if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                        //{
                        //    lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));
                        //}
                        //else
                        {
                            lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] =True", currentUser.Oid));
                        }
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTestMethodOid.Contains(testMethod.Oid))
                                    {
                                        lstTestMethodOid.Add(testMethod.Oid);
                                    }
                                }
                            }
                        }
                    }
                    if (!Administrator)
                    {
                        ((ListView)View).CollectionSource.Criteria["ADCFilter"] = new InOperator("Test.Oid", lstTestMethodOid);
                    }
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                {
                    bool Administrator = false;
                    List<Guid> lstTestMethodOid = new List<Guid>();
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        Administrator = true;
                    }
                    else
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        //if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                        //{
                        //    lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));
                        //}
                        //else
                        {
                            lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] =True", currentUser.Oid));
                        }
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (!lstTestMethodOid.Contains(testMethod.Oid))
                                    {
                                        lstTestMethodOid.Add(testMethod.Oid);
                                    }
                                }
                            }
                        }
                    }
                    if (!Administrator)
                    {
                        ((ListView)View).CollectionSource.Criteria["ADCFilter"] = new InOperator("Test.Oid", lstTestMethodOid);
                    }
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
                else if (View.Id == "Notes_ListView_CaseNarrative_Copy_QCBatch")
                {
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void EditAction_Executing(object sender, CancelEventArgs e)
        {
            if (View.Id == "Notes_ListView_CaseNarrative_Copy_QCBatch")
            {
                Notes note = (Notes)View.CurrentObject;
                if (CNInfo.SPJobId != null)
                {
                    note.SourceID = CNInfo.SPJobId;
                    ObjectSpace.CommitChanges();
                }
                
            }
        }

        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
            {
                foreach (SpreadSheetEntry_AnalyticalBatch sseab in View.SelectedObjects)
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        IList<SampleParameter> lstsmplpara = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[ABID] = ?", sseab.AnalyticalBatchID));
                        foreach (SampleParameter objsp in lstsmplpara.ToList())
                        {
                            if (objsp.QCBatchID != null && objsp.QCBatchID.IsDilution == true)
                            {
                                ObjectSpace.Delete(objsp);
                            }
                            QCBatchSequence objqbs = ObjectSpace.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[Oid] = ?", objsp.QCBatchID));
                            {
                                ObjectSpace.Delete(objqbs);
                            }
                        }
                        Samplecheckin checkin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", sseab.Jobid));
                        IList<Notes> notes = ObjectSpace.GetObjects<Notes>(CriteriaOperator.Parse("[Samplecheckin.Oid] =? AND [NoteSource] <> 'Sample Registration' AND [NoteSource] <> 'Sample Prepration' ", checkin.Oid));
                        ObjectSpace.Delete(notes);
                    }
                }
            }
        }
        private void RichText_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxRichEdit RichEdit = ((ASPxRichTextPropertyEditor)sender).ASPxRichEditControl;
                if (RichEdit != null && RichEdit.RibbonTabs.Count > 0)
                {
                    //RichEdit.RibbonTabs.FindByName("File").Visible = false;
                    RichEdit.RibbonTabs[0].Visible = false;
                    RichEdit.RibbonTabs[2].Visible = false;
                    RichEdit.RibbonTabs[3].Visible = false;
                    RichEdit.RibbonTabs[4].Visible = false;
                    RichEdit.RibbonTabs[5].Visible = false;
                    RichEdit.RibbonTabs[6].Visible = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void DvQCList_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (!analyticalbatchinfo.SeqIsLoaded)
                {
                    DashboardViewItem dvQCList = ((DetailView)View).FindItem("qclist") as DashboardViewItem;
                    SpreadSheetEntry_AnalyticalBatch objAB = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                    if (dvQCList != null && objAB != null)
                    {
                        ((ListView)dvQCList.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[qcseqdetail] is null or [qcseqdetail]=?", objAB.Oid);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DvQCType_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (!analyticalbatchinfo.QCTypeIsLoaded)
                {
                    DashboardViewItem dvQCType = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                    SpreadSheetEntry_AnalyticalBatch objAB = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                    if (dvQCType != null && objAB != null && objAB.Test != null)
                    {
                        List<string> lstqctype = new List<string>();
                        foreach (Testparameter TP in objAB.Test.TestParameter)
                        {
                            if (TP.QCType != null && !lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample" && objAB.Test.QCTypes.Contains(TP.QCType))
                            //if (!lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample")
                            {
                                lstqctype.Add(TP.QCType.QCTypeName);
                            }
                        }
                        ((ListView)dvQCType.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCTypeName] In(" + string.Format("'{0}'", string.Join("','", lstqctype)) + ")");
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
            if (View.Id == "QCType_ListView_analyticalbatchsequence" || View.Id == "QCBatchSequence_ListView_Copy")
            {
                ASPxGridListEditor gridlistEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridlistEditor != null)
                {
                    gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridlistEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                    if (View.Id == "QCBatchSequence_ListView_Copy")
                    {
                        gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        if (gridlistEditor.Grid.Columns["StrSampleID"] != null)
                        {
                            gridlistEditor.Grid.Columns["StrSampleID"].Width = 130;
                        }
                        if (gridlistEditor.Grid.Columns["SysSampleCode"] != null)
                        {
                            gridlistEditor.Grid.Columns["SysSampleCode"].Width = 130;
                        }
                        if (gridlistEditor.Grid.Columns["DilutionCount"] != null)
                        {
                            gridlistEditor.Grid.Columns["DilutionCount"].Width = 40;
                        }
                    }
                    else if (View.Id == "QCType_ListView_analyticalbatchsequence")
                    {
                        gridlistEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');
                        if(nav != null && sep != null) {
                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                           s.SetWidth((totusablescr / 100) * 13); 
                        }
                        else {
                            s.SetWidth(180); 
                        }                        
                    }";
                    }
                    gridlistEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridlistEditor.Grid.Settings.VerticalScrollableHeight = 300;
                }
            }
        }
        protected override void OnDeactivated()
        {
            try
            {
                // Unsubscribe from previously subscribed events and release other references and resources.
                base.OnDeactivated();
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_DetailView_Copy" || View.Id == "SpreadSheetEntry_AnalyticalBatch_DetailView_History")
                {
                    DashboardViewItem dvQCType = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                    if (dvQCType != null)
                    {
                        dvQCType.ControlCreated -= DvQCType_ControlCreated;
                    }
                    DashboardViewItem dvQCList = ((DetailView)View).FindItem("qclist") as DashboardViewItem;
                    if (dvQCList != null)
                    {
                        dvQCList.ControlCreated -= DvQCList_ControlCreated;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void OpenSDMS_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)((DetailView)View).CurrentObject;
                qcbatchinfo.canfilter = true;
                qcbatchinfo.strTest = qC.Test.TestName;
                if (qC != null)
                {
                    qcbatchinfo.QCBatchOid = qC.Oid;
                    qcbatchinfo.strqcid = qcbatchinfo.strqcbatchid = qC.AnalyticalBatchID;
                    if (qC.Test != null)
                    {
                        qcbatchinfo.OidTestMethod = qC.Test.Oid;
                    }
                }
                qcbatchinfo.strAB = qC.AnalyticalBatchID;
                qcbatchinfo.qcstatus = qC.Status;
                if (qC.Test.TestName.StartsWith("Mold"))
                {
                    qcbatchinfo.IsMoldTest = true;
                }
                else
                {
                    qcbatchinfo.IsMoldTest = false;
                }
                Frame.SetView(Application.CreateDashboardView((NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet)), "SDMS", true));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ABHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(os, typeof(SpreadSheetEntry_AnalyticalBatch));
                Frame.SetView(Application.CreateListView("SpreadSheetEntry_AnalyticalBatch_ListView_History", cs, true));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ABHistoryDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedChoiceActionItem.Id == "1M")
                {
                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffDay(CreatedDate,Now())<=30 And [CreatedDate] Is Not Null And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);
                }
                else if (e.SelectedChoiceActionItem.Id == "3M")
                {
                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate,Now())<=3 And [CreatedDate] Is Not Null And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);
                }
                else if (e.SelectedChoiceActionItem.Id == "6M")
                {
                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffMonth(CreatedDate,Now())<=6 And [CreatedDate] Is Not Null And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);
                }
                else if (e.SelectedChoiceActionItem.Id == "1Y")
                {
                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate,Now())<=1 And [CreatedDate] Is Not Null And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);
                }
                else if (e.SelectedChoiceActionItem.Id == "2Y")
                {
                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate,Now())<=2 And [CreatedDate] Is Not Null And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);
                }
                else if (e.SelectedChoiceActionItem.Id == "5Y")
                {
                    ((ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("DateDiffYear(CreatedDate,Now())<=5 And [CreatedDate] Is Not Null And [SampleParameterStatus.Status] <> ? ", Samplestatus.PendingEntry);
                }
                else if (e.SelectedChoiceActionItem.Id == "All")
                {
                    ((ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                }
                bool Administrator = false;
                List<Guid> lstTestMethodOid = new List<Guid>();
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                {
                    Administrator = true;
                }
                else
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                    //if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                    //{
                    //    lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));
                    //}
                    //else
                    {
                        lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] =True", currentUser.Oid));
                    }
                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                    {
                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                        {
                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                            {
                                if (!lstTestMethodOid.Contains(testMethod.Oid))
                                {
                                    lstTestMethodOid.Add(testMethod.Oid);
                                }
                            }
                        }
                    }
                }
                if (!Administrator)
                {
                    ((ListView)View).CollectionSource.Criteria["ADCFilter"] = new InOperator("Test.Oid", lstTestMethodOid);
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
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_History")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            string jobid = null;
                            foreach (SpreadSheetEntry_AnalyticalBatch ab in View.SelectedObjects)
                            {
                                Samplecheckin checkin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", ab.Jobid));
                                CNInfo.SCoidValue = checkin.Oid;
                                if (ab.AnalyticalBatchID != null)
                                {
                                    CNInfo.SPJobId = ab.AnalyticalBatchID;
                                }
                            }
                            IObjectSpace os = Application.CreateObjectSpace(typeof(Notes));
                            Notes objcrtdummy = os.CreateObject<Notes>();
                            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Notes));
                            cs.Criteria["filter"] = CriteriaOperator.Parse("[Samplecheckin.Oid] = ? AND [NoteSource] <> 'Sample Registration' AND [NoteSource] <> 'SDMS' AND [NoteSource] <> 'Result Entry' AND [NoteSource] <> 'Sample Prepration' AND [NoteSource] <> 'Custom Reporting'", CNInfo.SCoidValue);
                            ListView lvparameter = Application.CreateListView("Notes_ListView_CaseNarrative_Copy_QCBatch", cs, false);
                            lvparameter.Caption = "Comment";
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
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
