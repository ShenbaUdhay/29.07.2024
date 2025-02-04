﻿using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Office.Web;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Web.ASPxRichEdit;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using LDM.Module.Controllers.ICM;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.SDMS;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.SDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;

namespace LDM.Module.Web.Controllers.QC
{
    public partial class QCBatchController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        ICallbackManagerHolder sheet;
        ResourceManager rm;
        string CurrentLanguage = string.Empty;
        bool CanOpenQCBatch = false;
        QCBatchViewMode IsQCview = new QCBatchViewMode();
        ResultEntryQueryPanelInfo objQPInfo = new ResultEntryQueryPanelInfo();
        DefaultSetting objDefault;
        // bool boolComment;
        Qcbatchinfo objQcInfo = new Qcbatchinfo();
        AnaliytialBatchinfo objABinfo = new AnaliytialBatchinfo();
        viewInfo strviewid = new viewInfo();
        CaseNarativeInfo CNInfo = new CaseNarativeInfo();

        public QCBatchController()
        {
            InitializeComponent();
            TargetViewId = "QCBatchsequence;" + "QCType_ListView_qcbatchsequence;" + "QCBatchSequence_ListView;" + "QCBatch_ListView;" + "SpreadSheetEntry_AnalyticalBatch_DetailView;" + "TestMethod_ListView_AnalysisQueue;" + "QCBatch_ListView_Copy;"
                + "NonPersistentQcComment_DetailView;" + "SpreadSheetEntry_AnalyticalBatch_Reagents_ListView;" + "SpreadSheetEntry_AnalyticalBatch_DetailView_QCBatchID;"
                + "SpreadSheetEntry_AnalyticalBatch_Instruments_ListView;" + "SpreadSheetEntry_AnalyticalBatch_ListView;" + "SpreadSheetEntry_AnalyticalBatch_ListView_History";
            QCadd.TargetViewId = "QCType_ListView_qcbatchsequence";
            QCremove.TargetViewId = "QCBatchSequence_ListView";
            qcbatchDateFilterActions.TargetViewId = "QCBatch_ListView;" + "SpreadSheetEntry_AnalyticalBatch_ListView;";
            OpenResultEntry.TargetViewId = "QCBatchsequence;";
            ReagentLink.TargetViewId = ReagentUnLink.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_Reagents_ListView;";
            InstrumentLink.TargetViewId = InstrumentUnLink.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_Instruments_ListView;";
            Comment.TargetViewId = "QCBatchsequence;";
            OpenSDMSQcbatch.TargetViewId = "SpreadSheetEntry_AnalyticalBatch_ListView;" + "QCBatchsequence;" + "SpreadSheetEntry_AnalyticalBatch_ListView_History;";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                QCreset.Active.SetItemValue("btnreset", true);
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null)
                {
                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        CanOpenQCBatch = false;
                        InstrumentLink.Active.SetItemValue("InstrumentLink", false);
                        InstrumentUnLink.Active.SetItemValue("InstrumentUnLink", false);
                        ReagentLink.Active.SetItemValue("ReagentLink", false);
                        ReagentUnLink.Active.SetItemValue("ReagentUnLink", false);
                        if (objnavigationRefresh.ClickedNavigationItem == "AnalysisQueue ")
                        {
                            OpenSDMSQcbatch.Active.SetItemValue("openSDMS", false);
                        }
                        else
                        {
                            OpenSDMSQcbatch.Active.SetItemValue("openSDMS", true);
                            OpenSDMSQcbatch.Enabled.SetItemValue("openSDMS", false);
                        }
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            CanOpenQCBatch = true;
                            InstrumentLink.Active.SetItemValue("InstrumentLink", true);
                            InstrumentUnLink.Active.SetItemValue("InstrumentUnLink", true);
                            ReagentLink.Active.SetItemValue("ReagentLink", true);
                            ReagentUnLink.Active.SetItemValue("ReagentUnLink", true);
                            if (objnavigationRefresh.ClickedNavigationItem == "AnalysisQueue ")
                            {
                                OpenSDMSQcbatch.Active.SetItemValue("openSDMS", false);
                            }
                            else
                            {
                                OpenSDMSQcbatch.Enabled.SetItemValue("openSDMS", true);
                            }
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "QCbatch") != null)
                                {
                                    CanOpenQCBatch = true;
                                    InstrumentLink.Active.SetItemValue("InstrumentLink", true);
                                    InstrumentUnLink.Active.SetItemValue("InstrumentUnLink", true);
                                    ReagentLink.Active.SetItemValue("ReagentLink", true);
                                    ReagentUnLink.Active.SetItemValue("ReagentUnLink", true);
                                    //return;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Result Entry") == null)
                                {
                                    OpenResultEntry.Active["ShowOpenResultEntry"] = false;
                                }
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Spreadsheet") != null)
                                {
                                    OpenSDMSQcbatch.Enabled.SetItemValue("openSDMS", true);
                                }
                            }
                        }
                    }
                }

                DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
                if (ObjectSpace is XPObjectSpace)
                {
                    SelectedData sproc = ((XPObjectSpace)(ObjectSpace)).Session.ExecuteSproc("getCurrentLanguage", "");
                    CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                }
                rm = new ResourceManager("Resources.SDMS", Assembly.Load("App_GlobalResources"));
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_History")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                if (View.Id == "QCBatch_ListView" || View.Id == "QCBatch_ListView_Copy" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                {
                    strviewid.strtempresultentryviewid = View.Id;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    qcbatchDateFilterActions.SelectedItemChanged += QcbatchDateFilterAction_SelectedItemChanged;
                    qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    if (qcbatchDateFilterActions.SelectedItem == null)
                    {
                        if (setting.AnalysisEntryModel == EnumDateFilter.OneMonth)
                        {
                            qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-1);
                            qcbatchDateFilterActions.SelectedItem = qcbatchDateFilterActions.Items[0];
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.ThreeMonth)
                        {
                            qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-3);
                            qcbatchDateFilterActions.SelectedItem = qcbatchDateFilterActions.Items[1];
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.SixMonth)
                        {
                            qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-6);
                            qcbatchDateFilterActions.SelectedItem = qcbatchDateFilterActions.Items[2];
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.OneYear)
                        {
                            qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddYears(-1);
                            qcbatchDateFilterActions.SelectedItem = qcbatchDateFilterActions.Items[3];
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.TwoYear)
                        {
                            qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddYears(-2);
                            qcbatchDateFilterActions.SelectedItem = qcbatchDateFilterActions.Items[4];
                        }
                        else if (setting.AnalysisEntryModel == EnumDateFilter.FiveYear)
                        {
                            qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddYears(-5);
                            qcbatchDateFilterActions.SelectedItem = qcbatchDateFilterActions.Items[5];
                        }
                        else
                        {
                            qcbatchinfo.qcFilterByMonthDate = DateTime.MinValue;
                            qcbatchDateFilterActions.SelectedItem = qcbatchDateFilterActions.Items[6];
                        }
                    }
                    if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                    {
                        ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                        tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[SampleParameterStatus.Status] = ? And ([SampleParameterStatus.TestHold] <> true or [SampleParameterStatus.TestHold] is Null)", Samplestatus.PendingEntry);
                    }
                }
                else if (View.Id == "QCBatchsequence")
                {
                    WebWindow.CurrentRequestWindow.RegisterClientScript("alrt", "document.getElementById('separatorButton').setAttribute('onclick', 'qcuirefresh();')");
                    Application.DetailViewCreating += Application_DetailViewCreating;
                    //Frame.GetController<RefreshController>().RefreshAction.Executing += RefreshAction_Executing;
                    QCprevious.Enabled.SetItemValue("key", false);
                    Comment.Enabled.SetItemValue("HideComment", false);

                    if (CurrentLanguage == "En")
                    {
                        QCsort.Caption = "Sort";
                    }
                    else
                    {
                        QCsort.Caption = "序号";
                    }
                    DashboardViewItem QCDetailViewComment = ((DashboardView)View).FindItem("Others") as DashboardViewItem;
                    DashboardViewItem QCDetailView = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                    DashboardViewItem QCReagent = ((DashboardView)View).FindItem("Reagent") as DashboardViewItem;
                    DashboardViewItem qclist = ((DashboardView)View).FindItem("qclist") as DashboardViewItem;
                    if (QCDetailViewComment != null)
                    {
                        QCDetailViewComment.ControlCreated += QCDetailViewComment_ControlCreated;
                    }
                }
                else if (View.Id == "QCBatchSequence_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                }
                else if (View.Id == "QCType_ListView_qcbatchsequence")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                }
                else if (View.Id == "TestMethod_ListView_AnalysisQueue")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                    bool Administrator = false;
                    List<Guid> lstTestMethodOid = new List<Guid>();
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        Administrator = true;
                    }
                    else
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview")
                        {
                            lstAnalysisDepartChain = os.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));
                        }
                        else
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
                        ((ListView)View).CollectionSource.Criteria["ADCFilter"] = new InOperator("Oid", lstTestMethodOid);
                    }
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_DetailView")
                {
                    qcbatchinfo.AnalyticalQCBatchOid = null;
                    if (View.CurrentObject != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch samplingAllocation = View.CurrentObject as SpreadSheetEntry_AnalyticalBatch;
                        if (samplingAllocation != null && samplingAllocation.Test != null && samplingAllocation.Method.Labwares != null && samplingAllocation.Method.Labwares.Any(i => !string.IsNullOrEmpty(i.LabwareName)))
                        {
                            if (samplingAllocation.Method.Labwares.Count == 1)
                            {
                                samplingAllocation.strInstrument = samplingAllocation.Test.Labwares.First().AssignedName.ToString();
                                samplingAllocation.Instrument = samplingAllocation.Test.Labwares.First().Oid.ToString();
                            }
                            
                        }
                    }
                    

                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_Reagents_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    DashboardViewItem QCDetailView = null;
                    if (Application.MainWindow.View is DashboardView)
                    {
                        QCDetailView = ((DashboardView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    else if (Application.MainWindow.View is DetailView)
                    {
                        QCDetailView = ((DetailView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    if (QCDetailView != null && QCDetailView.InnerView != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch objQc = (SpreadSheetEntry_AnalyticalBatch)QCDetailView.InnerView.CurrentObject;
                        if (objQc != null)
                        {
                            if (objQc.Reagents.Count > 0)
                            {
                                IList<Guid> objReagent = objQc.Reagents.Select(i => i.Oid).ToList();
                                ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", objReagent);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                            }
                        }
                    }
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_Instruments_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    DashboardViewItem QCDetailView = null;
                    if (Application.MainWindow.View is DashboardView)
                    {
                        QCDetailView = ((DashboardView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    else if (Application.MainWindow.View is DetailView)
                    {
                        QCDetailView = ((DetailView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    if (QCDetailView != null && QCDetailView.InnerView != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch objQc = (SpreadSheetEntry_AnalyticalBatch)QCDetailView.InnerView.CurrentObject;
                        if (objQc != null)
                        {
                            if (objQc.Instruments.Count > 0)
                            {
                                IList<Guid> objInstrument = objQc.Instruments.Select(i => i.Oid).ToList();
                                ((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", objInstrument);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("Oid is null");
                            }
                        }
                    }
                }
                else if (View.Id == "NonPersistentQcComment_DetailView")
                {
                    ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Comments") as ASPxRichTextPropertyEditor;
                    if (RichText != null)
                    {
                        RichText.ControlCreated += RichText_ControlCreated;
                    }
                    View.CurrentObject = View.ObjectTypeInfo.CreateInstance();
                    ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
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
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_DetailView" && e.Object != null)
                {
                    if (e.PropertyName == "Method" && e.Object.GetType() == typeof(SpreadSheetEntry_AnalyticalBatch))
                    {
                        SpreadSheetEntry_AnalyticalBatch objqcbatch = (SpreadSheetEntry_AnalyticalBatch)e.Object;
                        if (objqcbatch != null && objqcbatch.Matrix != null && objqcbatch.Test != null && objqcbatch.Method != null)
                        {
                            string js = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');                      
                        if(nav != null && sep != null) 
                        {
                            var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                            s.SetWidth(totusablescr /3);
                        }
                        else 
                        {
                            s.SetWidth(145); 
                        }                      
                    }";

                            if (Application.MainWindow.View.Id == "SDMS")
                            {
                                js = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');                      
                        if(nav != null && sep != null) 
                        {
                            var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                            s.SetWidth(totusablescr /2);
                        }
                        else 
                        {
                            s.SetWidth(220); 
                        }                      
                    }";
                            }
                            if (Frame is NestedFrame)
                            {
                                ActionContainerViewItem qcaction2 = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qcaction2") as ActionContainerViewItem;
                                bool enbstat;

                                //if (qcaction2 != null && qcaction2.Actions.Count > 0 && (qcaction2.Actions[1].Caption == "Ok" || qcaction2.Actions[1].Caption == "确定"))                       
                                if (qcbatchinfo.strqcid != null || (qcaction2 != null && qcaction2.Actions.Count > 0 && (qcaction2.Actions[1].Caption == "Ok" || qcaction2.Actions[1].Caption == "确定")))
                                {
                                    if (qcbatchinfo.strqcid != null && qcaction2 != null && qcaction2.Actions.Count > 0 && (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "确定"))
                                    {
                                        enbstat = true;
                                    }
                                    else
                                    {
                                        enbstat = false;
                                    }
                                }
                                else
                                {
                                    enbstat = true;
                                }

                                ((SpreadSheetEntry_AnalyticalBatch)View.CurrentObject).ISShown = enbstat;
                                foreach (ViewItem item in ((DetailView)View).Items)
                                {
                                    if (item.GetType() == typeof(ASPxStringPropertyEditor))
                                    {
                                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                            if (propertyEditor.Editor.GetType() != typeof(DevExpress.Web.ASPxMemo))
                                            {
                                                ASPxTextBox textBox = (ASPxTextBox)propertyEditor.Editor;
                                                if (textBox != null)
                                                {
                                                    textBox.ClientSideEvents.Init = js;
                                                    textBox.ClientInstanceName = propertyEditor.Id;
                                                }
                                            }
                                            else
                                            {
                                                DevExpress.Web.ASPxMemo memoBox = (DevExpress.Web.ASPxMemo)propertyEditor.Editor;
                                                if (memoBox != null)
                                                {
                                                    memoBox.ClientSideEvents.Init = js;
                                                    memoBox.ClientInstanceName = propertyEditor.Id;
                                                }
                                            }
                                            if (propertyEditor.AllowEdit)
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                            }
                                        }
                                    }
                                    else if (item.GetType() == typeof(AspxGridLookupCustomEditor))
                                    {
                                        AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null && item.Id== "NPJobid")
                                        {
                                            //if (qcbatchinfo.strqcid == null)
                                            //{
                                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                            //}
                                            //else
                                            //{
                                            //    propertyEditor.AllowEdit.SetItemValue("stat", true);
                                            //}
                                            ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                            SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                                            if (qC != null)
                                            {
                                                if (gridLookup != null)
                                                {
                                                    if (Application.MainWindow.View.Id == "SDMS")
                                                    {
                                                        gridLookup.JSProperties["cpJobID"] = qC.Jobid;
                                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                                    }

                                                    else
                                                    {
                                                        gridLookup.JSProperties["cpJobID"] = qC.Jobid;
                                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 3);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                                    }

                                                    gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                                    gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                                    gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                                    gridLookup.ValueChanged += GridLookup_ValueChanged;
                                                    gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                                    gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                                    gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "JobID", Caption = rm.GetString("JobID_" + CurrentLanguage) });
                                                    gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Sx" });
                                                    gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TAT" });
                                                    gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DueDate", Caption = rm.GetString("DueDate_" + CurrentLanguage) });
                                                    gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DateReceived", Caption = rm.GetString("DateReceived_" + CurrentLanguage) });
                                                    gridLookup.GridView.Columns["JobID"].Width = 100;
                                                    gridLookup.GridView.Columns["Sx"].Width = 70;
                                                    gridLookup.GridView.Columns["TAT"].Width = 70;
                                                    gridLookup.GridView.Columns["DueDate"].Width = 100;
                                                    gridLookup.GridView.Columns["DateReceived"].Width = 100;
                                                    gridLookup.GridView.KeyFieldName = "JobID";
                                                    gridLookup.TextFormatString = "{0}";
                                                    //gridLookup.GridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared
                                                    gridLookup.GridView.HtmlRowPrepared += Grid_HtmlRowPrepared;
                                                    DataTable table = new DataTable();
                                                    table.Columns.Add("JobID");
                                                    table.Columns.Add("Sx");
                                                    table.Columns.Add("TAT");
                                                    table.Columns.Add("DueDate");
                                                    table.Columns.Add("DateReceived");


                                                    //if (qC.Test != null && qC.Method != null && (qC.QCBatchID == "Auto Generate" || qC.QCBatchID == "自动生成"))
                                                    if (qC.Test != null && qC.Method != null) // && (qC.QCBatchID == "Auto Generate" || qC.QCBatchID == "自动生成")
                                                    {
                                                        IList<SampleParameter> samples = new List<SampleParameter>();
                                                        int testprepmethodd = 0;
                                                        TestMethod objtestmethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", qC.Test.Oid));
                                                        if (objtestmethod != null && objtestmethod.PrepMethods.Count > 0)
                                                        {
                                                            testprepmethodd = objtestmethod.PrepMethods.Count;
                                                        }
                                                        objDefault = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True"));
                                                        if (objnavigationRefresh.ClickedNavigationItem == "QCbatches")
                                                        {
                                                            if (objDefault != null && testprepmethodd > 0)
                                                            {
                                                                samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [IsPrepMethodComplete]  = True And [SignOff] = True AND [IsTransferred] = true And (([UQABID] Is Null And [QCBatchID] Is Null) || ([QCBatchID.qcseqdetail.AnalyticalBatchID] = ?))", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.AnalyticalBatchID));
                                                            }
                                                            else
                                                            {
                                                                samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [SignOff] = True And [IsTransferred] = true And (([UQABID] Is Null And [QCBatchID] Is Null) || ([QCBatchID.qcseqdetail.AnalyticalBatchID] = ?))", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.AnalyticalBatchID));
                                                            }

                                                        }
                                                        else
                                                        {
                                                            if (objDefault != null && testprepmethodd > 0)
                                                            {
                                                                samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.GCRecord] Is Null And [SignOff] = True And [IsTransferred] = true And [UQABID] Is Null And [QCBatchID] Is Null AND [IsPrepMethodComplete]  = True", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber));
                                                            }
                                                            else
                                                            {
                                                                samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.GCRecord] Is Null And [SignOff] = True And [IsTransferred] = true And [UQABID] Is Null And [QCBatchID] Is Null", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber));
                                                            }
                                                        }
                                                        if (samples != null && samples.Count > 0)
                                                        {
                                                            if (objnavigationRefresh.ClickedNavigationItem == "QCbatches")
                                                            {
                                                                foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.Samplelogin.JobID != null && a.SubOut != true /*&& a.SamplePrepBatchID != null */&& !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                                {
                                                                    if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                                    {
                                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                                    }
                                                                    else if (objsample.Samplelogin.JobID.TAT != null)
                                                                    {
                                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null });
                                                                    }
                                                                    else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                                    {
                                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                                    }
                                                                    else
                                                                    {
                                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null });
                                                                    }
                                                                }
                                                                DataView dv = new DataView(table);
                                                                dv.Sort = "DueDate Asc";
                                                                table = dv.ToTable();
                                                            }
                                                            else
                                                            {
                                                                foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.SignOff == true && a.Samplelogin.JobID != null && a.SubOut != true && !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                                {
                                                                    if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                                    {
                                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                                    }
                                                                    else if (objsample.Samplelogin.JobID.TAT != null)
                                                                    {
                                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null });
                                                                    }
                                                                    else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                                    {
                                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                                    }
                                                                    else
                                                                    {
                                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null });
                                                                    }
                                                                }
                                                                DataView dv = new DataView(table);
                                                                dv.Sort = "DueDate Asc";
                                                                table = dv.ToTable();
                                                            }
                                                        }
                                                        else if (samples == null && qC.Jobid != null)
                                                        {
                                                            string[] ids = qC.Jobid.Split(';');
                                                            foreach (string id in ids)
                                                            {
                                                                Samplecheckin sample = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Jobid]=?", id));
                                                                if (sample != null && !string.IsNullOrEmpty(sample.JobID))
                                                                {
                                                                    table.Rows.Add(new object[] { sample.JobID, 0 });
                                                                }
                                                            }
                                                        }
                                                    }
                                                    gridLookup.GridView.DataSource = table;
                                                    gridLookup.GridView.DataBind();
                                                }
                                            }
                                        }
                                        else if (propertyEditor != null && propertyEditor.Editor != null&&item.Id== "NPInstrument")
                                        {
                                            propertyEditor.AllowEdit.SetItemValue("NPInstrument", enbstat);
                                            ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                            SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                                            if (qC != null)
                                            {
                                                if (gridLookup != null)
                                                {
                                                    if (Application.MainWindow.View.Id == "SDMS")
                                                    {
                                                        gridLookup.JSProperties["cpNPInstrument"] = qC.strInstrument;
                                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpNPInstrument);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                                    }

                                                    else
                                                    {
                                                        gridLookup.JSProperties["cpNPInstrument"] = qC.strInstrument;
                                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpNPInstrument);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 3);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                                    }
                                                    gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                                    gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                                    gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                                    gridLookup.ValueChanged += GridLookup_ValueChanged;
                                                    gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                                    gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                                    gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Oid" });
                                                    gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Instrument" });
                                                    gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "InstrumentID" });
                                                    gridLookup.GridView.Columns["Instrument"].Width = 150;
                                                    gridLookup.GridView.Columns["InstrumentID"].Width = 150;
                                                    gridLookup.GridView.KeyFieldName = "Oid";
                                                    gridLookup.TextFormatString = "{2}";
                                                    gridLookup.GridView.Columns["Oid"].Visible = false;
                                                    DataTable table = new DataTable();
                                                    table.Columns.Add("Oid");
                                                    table.Columns.Add("Instrument");
                                                    table.Columns.Add("InstrumentID");
                                                    if (qC.Test != null && qC.Method != null)
                                                    {
                                                        foreach (Labware objlab in qC.Method.Labwares.OrderBy(a => a.AssignedName).Distinct().ToList())
                                                        {
                                                            table.Rows.Add(new object[] {objlab.Oid, objlab.LabwareName, objlab.AssignedName });
                                                        }
                                                        DataView dv = new DataView(table);
                                                        dv.Sort = "Instrument Asc";
                                                        table = dv.ToTable();
                                                    }
                                                    gridLookup.GridView.DataSource = table;
                                                    gridLookup.GridView.DataBind();
                                                }
                                            }
                                        }
                                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.BackColor = Color.LightYellow;
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                                    {
                                        ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                                        if (item.Id != "Instrument")
                                        {
                                            if (propertyEditor != null && propertyEditor.Editor != null)
                                            {
                                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                                if (gridLookup != null)
                                                {
                                                    gridLookup.ClientSideEvents.Init = js;
                                                    gridLookup.ClientInstanceName = propertyEditor.Id;
                                                }
                                            }
                                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                            }
                                        }
                                        else
                                        {
                                            if (propertyEditor != null && propertyEditor.Editor != null)
                                            {
                                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                                if (gridLookup != null)
                                                {
                                                    gridLookup.ClientSideEvents.Init = js;
                                                    gridLookup.ClientInstanceName = propertyEditor.Id;
                                                }
                                            }
                                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                            {
                                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                            }
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                                    {
                                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                            propertyEditor.Editor.ClientSideEvents.Init = js;
                                            propertyEditor.Editor.ClientInstanceName = propertyEditor.Id;
                                        }
                                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.BackColor = Color.LightYellow;
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                                    {
                                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                            propertyEditor.Editor.ClientSideEvents.Init = js;
                                            propertyEditor.Editor.ClientInstanceName = propertyEditor.Id;
                                        }
                                    }
                                    else if (item.GetType() == typeof(FileDataPropertyEditor))
                                    {
                                        FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                                    {
                                        ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                            propertyEditor.Editor.ClientSideEvents.Init = js;
                                            propertyEditor.Editor.ClientInstanceName = propertyEditor.Id;
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                                    {
                                        ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            if (qcbatchinfo.strqcid != null && qcaction2 != null && qcaction2.Actions.Count > 0 && (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "确定"))
                                            {
                                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                                            }
                                            else
                                            {
                                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                            }
                                            //if (qcbatchinfo.strqcid == null)
                                            //{
                                            //    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                            //}
                                            //else
                                            //{
                                            //    propertyEditor.AllowEdit.SetItemValue("stat", false);
                                            //}
                                            if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                            {
                                                propertyEditor.FindEdit.Editor.ClientSideEvents.Init = js;
                                                propertyEditor.FindEdit.Editor.ClientInstanceName = propertyEditor.Id;
                                            }
                                            else if (propertyEditor.DropDownEdit != null)
                                            {
                                                propertyEditor.DropDownEdit.DropDown.ClientSideEvents.Init = js;
                                                propertyEditor.DropDownEdit.DropDown.ClientInstanceName = propertyEditor.Id;
                                            }
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
                                        }
                                    }
                                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                                    {
                                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                        if (propertyEditor != null && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                            propertyEditor.Editor.ClientSideEvents.Init = js;
                                            propertyEditor.Editor.ClientInstanceName = propertyEditor.Id;
                                        }
                                        if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                        {
                                            propertyEditor.Editor.BackColor = Color.LightYellow;
                                        }
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

        private void RichText_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxRichEdit RichEdit = ((ASPxRichTextPropertyEditor)sender).ASPxRichEditControl;
                if (RichEdit != null)
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

        private void QCReagent_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem QCReagent = ((DashboardView)View).FindItem("Reagent") as DashboardViewItem;
                if (QCReagent != null && QCReagent.InnerView != null)
                {
                    Application.DetailViewCreating += Application_DetailViewCreating;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QCDetailViewComment_ControlCreated(object sender, EventArgs e)
        {
            try
            {
                if (objQcInfo.boolComment == false && qcbatchinfo.strqcid != null)
                {
                    DashboardViewItem QCDetailView = null;
                    DashboardViewItem QCDetailViewComment = null;
                    if (View is DashboardView)
                    {
                        QCDetailView = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                        QCDetailViewComment = ((DashboardView)View).FindItem("Others") as DashboardViewItem;
                    }
                    else if (View is DetailView)
                    {
                        QCDetailView = ((DetailView)View).FindItem("qcdetail") as DashboardViewItem;
                        QCDetailViewComment = ((DetailView)View).FindItem("Others") as DashboardViewItem;
                    }
                    if (((DetailView)QCDetailViewComment.InnerView).CurrentObject != null)
                    {
                        if (((DetailView)QCDetailView.InnerView).CurrentObject != null)
                        {
                            SpreadSheetEntry_AnalyticalBatch Qcbatch = (SpreadSheetEntry_AnalyticalBatch)(((DetailView)QCDetailView.InnerView).CurrentObject);
                            if (Qcbatch != null)
                            {
                                if (!((DetailView)QCDetailView.InnerView).ObjectSpace.IsNewObject(Qcbatch))
                                {
                                    NonPersistentQcComment objComment = (NonPersistentQcComment)QCDetailViewComment.InnerView.CurrentObject;
                                    if (objComment != null)
                                    {
                                        objComment.Comments = Qcbatch.Comments;
                                        objQcInfo.boolComment = true;
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

        private void RuleSet_CustomNeedToValidateRule(object sender, DevExpress.Persistent.Validation.CustomNeedToValidateRuleEventArgs e)
        {
            try
            {
                if (e.Rule.Id == "NPJobid")
                {
                    e.NeedToValidateRule = false;
                    e.Handled = !e.NeedToValidateRule;
                }
                else if (e.Rule.Id == "NPInstrument")
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

        private void QcbatchDateFilterAction_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && qcbatchDateFilterActions != null && qcbatchDateFilterActions.SelectedItem != null && (View.Id == "QCBatch_ListView" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView"))
                {
                    if (qcbatchDateFilterActions.SelectedItem.Id == "1M")
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-1);
                    }
                    else if (qcbatchDateFilterActions.SelectedItem.Id == "3M")
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-3);
                    }
                    else if (qcbatchDateFilterActions.SelectedItem.Id == "6M")
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddMonths(-6);
                    }
                    else if (qcbatchDateFilterActions.SelectedItem.Id == "1Y")
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.Today.AddYears(-1);
                    }
                    else
                    {
                        qcbatchinfo.qcFilterByMonthDate = DateTime.MinValue;
                    }

                    if (qcbatchinfo.qcFilterByMonthDate != DateTime.MinValue)
                    {
                        //((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[CreatedDate] BETWEEN('" + qcbatchinfo.qcFilterByMonthDate + "', '" + DateTime.Now + "')");
                        ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria["dateFilter"] = CriteriaOperator.Parse("[CreatedDate] >= ? And [CreatedDate] < ?", qcbatchinfo.qcFilterByMonthDate, DateTime.Now);
                    }
                    else
                    {
                        if (((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.ContainsKey("dateFilter"))
                        {
                            ((DevExpress.ExpressApp.ListView)View).CollectionSource.Criteria.Remove("dateFilter");
                        }
                    }
                    ((DevExpress.ExpressApp.ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RefreshAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                if (QCreset.Enabled)
                {
                    QCreset.DoExecute();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                foreach (SpreadSheetEntry_AnalyticalBatch batch in View.SelectedObjects)
                {
                    if (batch.Test != null && batch.Test.IsPLMTest)
                    {
                        if (batch.NonStatus == AnalyticalBatchStatus.PendingValidation || batch.NonStatus == AnalyticalBatchStatus.Validated)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "QCnotdelete"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                    else
                    {
                        List<SampleParameter> lst = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("UQABID=?", batch.Oid)).ToList();
                        if (batch.NonStatus != AnalyticalBatchStatus.PendingResultEntry || batch.SampleParameterStatus.Status != Samplestatus.PendingEntry)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "QCnotdelete"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        else if (lst.FirstOrDefault(i => !string.IsNullOrEmpty(i.ResultNumeric)) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "alreadyenteredresult"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                    }
                }
                foreach (SpreadSheetEntry_AnalyticalBatch batch in View.SelectedObjects.Cast<SpreadSheetEntry_AnalyticalBatch>().ToList())
                {
                    if (batch.Test != null && batch.Test.IsPLMTest)
                    {
                        IList<QCBatchSequence> objqCBatch = ObjectSpace.GetObjects<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail]=?", batch.Oid));
                        ObjectSpace.Delete(objqCBatch);

                        IList<SampleParameter> objsampleParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail]=?", batch.Oid));
                        foreach (SampleParameter sample in objsampleParameters.ToList())
                        {
                            if (sample.QCBatchID != null && sample.QCBatchID.QCType != null && sample.QCBatchID.QCType.QCTypeName != "Sample")
                            {
                                ObjectSpace.Delete(sample);
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
                            ObjectSpace.Delete(objPLMs);
                            ObjectSpace.Delete(pLM);
                        }

                        ObjectSpace.Delete(batch);
                    }
                    else
                    {
                        if (batch.NonStatus == AnalyticalBatchStatus.PendingResultEntry || batch.SampleParameterStatus.Status == Samplestatus.PendingEntry)
                        {
                            IList<QCBatchSequence> objqCBatch = ObjectSpace.GetObjects<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail]=?", batch.Oid));
                            ObjectSpace.Delete(objqCBatch);
                            IList<SampleParameter> objsampleParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail]=?", batch.Oid));
                            foreach (SampleParameter sample in objsampleParameters.ToList())
                            {
                                if (sample.QCBatchID != null && sample.QCBatchID.QCType != null && sample.QCBatchID.QCType.QCTypeName != "Sample")
                                {
                                    ObjectSpace.Delete(sample);
                                }
                                else
                                {
                                    sample.UQABID = null;
                                    sample.QCBatchID = null;
                                    sample.QCSort = 0;
                                }
                            }
                            ObjectSpace.Delete(batch);
                        }
                    }
                }
                ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DeleteSuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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
                if (e.ViewID == "SpreadSheetEntry_AnalyticalBatch_DetailView")
                {
                    Application.DetailViewCreating -= Application_DetailViewCreating;
                    IObjectSpace os = Application.CreateObjectSpace();
                    qcbatchinfo.canfilter = true;
                    if (qcbatchinfo.strqcid == null && qcbatchinfo.strqcbatchid == null)
                    {
                        SpreadSheetEntry_AnalyticalBatch qC = os.CreateObject<SpreadSheetEntry_AnalyticalBatch>();
                        //if (CurrentLanguage == "En")
                        //{
                        //    qC.QCBatchID = "Auto Generate";
                        //}
                        //else
                        //{
                        //    qC.QCBatchID = "自动生成";
                        //}
                        qC.AnalyticalBatchID = string.Empty;
                        qC.CreatedDate = DateTime.Now;
                        qC.CreatedBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        qC.Noruns = 1;
                        if (qcbatchinfo.OidTestMethod != null)
                        {
                            TestMethod test = os.GetObjectByKey<TestMethod>(qcbatchinfo.OidTestMethod);
                            if (test != null)
                            {
                                qC.Matrix = test.MatrixName;
                                qC.Test = test;
                                if (Frame.Context != TemplateContext.PopupWindow)
                                {
                                    qcbatchinfo.OidTestMethod = null;
                                }
                            }
                        }
                        e.View = Application.CreateDetailView(os, qC);
                        e.View.ViewEditMode = ViewEditMode.Edit;
                    }
                    else
                    {
                        if (qcbatchinfo.strqcid == null && qcbatchinfo.strqcbatchid != null)
                        {
                            qcbatchinfo.strqcid = qcbatchinfo.strqcbatchid;
                        }
                        SpreadSheetEntry_AnalyticalBatch qC = os.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[AnalyticalBatchID]=?", qcbatchinfo.strqcid));
                        if (qC != null)
                        {
                            e.View = Application.CreateDetailView(os, qC);
                            e.View.ViewEditMode = ViewEditMode.Edit;
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

        private void EditAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id != "SpreadSheetEntry_AnalyticalBatch_Reagents_ListView" && View.Id != "SpreadSheetEntry_AnalyticalBatch_Instruments_ListView")
                {
                    e.Cancel = true;
                    qcbatchinfo.isview = false;
                    SpreadSheetEntry_AnalyticalBatch batch = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                    qcbatchinfo.strqcbatchid = null;
                    if (batch != null)
                    {
                        qcbatchinfo.strqcid = batch.AnalyticalBatchID;
                    }
                    foreach (QCBatchSequence sequence in batch.qcdetail)
                    {
                        if (sequence.SampleID.SampleParameter.Where(a => a.Testparameter.TestMethod == batch.Test && a.Status != Samplestatus.PendingEntry).ToList().Count > 0)
                        {
                            qcbatchinfo.isview = true;
                            break;
                        }
                    }
                    Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "QCBatchsequence", true));
                }
                else
                {
                    if (View.Id == "SpreadSheetEntry_AnalyticalBatch_Reagents_ListView")
                    {
                        e.Cancel = true;
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        Reagent objReagent = objectSpace.GetObject((Reagent)View.CurrentObject);
                        DetailView dv = Application.CreateDetailView(objectSpace, "Reagent_DetailView_Copy", true, objReagent);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters();
                        showViewParameters.CreatedView = dv;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        //dc.SaveOnAccept = false;
                        //dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dc_Accepting;
                        //dc.AcceptAction.Executed += AcceptAction_Executed;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_Instruments_ListView")
                    {
                        e.Cancel = true;
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        Labware objInstrument = objectSpace.GetObject((Labware)View.CurrentObject);
                        DetailView dv = Application.CreateDetailView(objectSpace, "Labware_DetailView_Copy", true, objInstrument);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters();
                        showViewParameters.CreatedView = dv;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        //dc.SaveOnAccept = false;
                        //dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dc_Accepting;
                        //dc.AcceptAction.Executed += AcceptAction_Executed;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                {
                    e.Cancel = true;
                    IObjectSpace os = Application.CreateObjectSpace();
                    SpreadSheetEntry_AnalyticalBatch analyBatch = os.CreateObject<SpreadSheetEntry_AnalyticalBatch>();
                    DetailView dvanalyBatch = Application.CreateDetailView(os, "SpreadSheetEntry_AnalyticalBatch_DetailView_QCBatchID", false, analyBatch);
                    dvanalyBatch.ViewEditMode = ViewEditMode.Edit;
                    Frame.SetView(dvanalyBatch);
                }
                if (View.Id != "SpreadSheetEntry_AnalyticalBatch_Reagents_ListView" && View.Id != "SpreadSheetEntry_AnalyticalBatch_Instruments_ListView")
                {
                    e.Cancel = true;
                    qcbatchinfo.strqcid = null;
                    qcbatchinfo.OidTestMethod = null;
                    qcbatchinfo.strqcbatchid = null;
                    Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "QCBatchsequence", true));
                    OpenSDMSQcbatch.Enabled.SetItemValue("openSDMS", false);
                }
                else
                {
                    if (View.Id == "SpreadSheetEntry_AnalyticalBatch_Reagents_ListView")
                    {
                        e.Cancel = true;
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        Reagent obj = objectSpace.CreateObject<Reagent>();
                        DetailView dv = Application.CreateDetailView(objectSpace, "Reagent_DetailView_Copy", true, obj);
                        ShowViewParameters showViewParameters = new ShowViewParameters();
                        showViewParameters.CreatedView = dv;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        //dc.SaveOnAccept = false;
                        //dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dc_Accepting;
                        dc.AcceptAction.Executed += AcceptAction_Executed;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                    else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_Instruments_ListView")
                    {
                        e.Cancel = true;
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        Labware obj = objectSpace.CreateObject<Labware>();
                        DetailView dv = Application.CreateDetailView(objectSpace, "Labware_DetailView_Copy", true, obj);
                        ShowViewParameters showViewParameters = new ShowViewParameters();
                        showViewParameters.CreatedView = dv;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        //dc.SaveOnAccept = false;
                        //dc.CloseOnCurrentObjectProcessing = false;
                        dc.Accepting += Dc_InstrumentAccepting;
                        //dc.AcceptAction.Executed += InstrmentAcceptAction_Executed;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_InstrumentAccepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                DialogController objDialog = (DialogController)sender as DialogController;
                if (objDialog != null && objDialog.Frame.View is DetailView)
                {
                    DetailView view = (DetailView)objDialog.Frame.View;
                    Labware ObjLabware = ((Labware)e.AcceptActionArgs.CurrentObject);
                    if (ObjLabware != null)
                    {
                        DashboardViewItem QCInstrument = null;
                        DashboardViewItem QCDetailView = null;
                        if (Application.MainWindow.View is DashboardView)
                        {
                            QCInstrument = ((DashboardView)Application.MainWindow.View).FindItem("Instrument") as DashboardViewItem;
                            QCDetailView = ((DashboardView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                        }
                        else
                        if (Application.MainWindow.View is DetailView)
                        {
                            QCInstrument = ((DetailView)Application.MainWindow.View).FindItem("Instrument") as DashboardViewItem;
                            QCDetailView = ((DetailView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                        }
                        view.ObjectSpace.CommitChanges();
                        if (QCDetailView != null && QCDetailView.InnerView != null)
                        {
                            SpreadSheetEntry_AnalyticalBatch objQc = ((SpreadSheetEntry_AnalyticalBatch)QCDetailView.InnerView.CurrentObject);
                            if (objQc != null && !objQc.Instruments.Contains(ObjLabware))
                            {
                                objQc.Instruments.Add(QCDetailView.InnerView.ObjectSpace.GetObject(ObjLabware));
                                ((ListView)QCInstrument.InnerView).CollectionSource.Add(QCInstrument.InnerView.ObjectSpace.GetObject(ObjLabware));
                                QCInstrument.InnerView.Refresh();
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

        private void AcceptAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                DashboardViewItem QCReagent = ((DashboardView)Application.MainWindow.View).FindItem("Reagent") as DashboardViewItem;
                if (QCReagent != null && QCReagent.InnerView != null)
                {
                    QCReagent.InnerView.Refresh();
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
                DialogController objDialog = (DialogController)sender as DialogController;
                if (objDialog != null && objDialog.Frame.View is DetailView)
                {
                    DetailView view = (DetailView)objDialog.Frame.View;
                    Reagent ObjReagent = ((Reagent)e.AcceptActionArgs.CurrentObject);
                    if (ObjReagent != null)
                    {
                        DashboardViewItem QCReagent = null;
                        DashboardViewItem QCDetailView = null;
                        if (Application.MainWindow.View is DashboardView)
                        {
                            QCReagent = ((DashboardView)Application.MainWindow.View).FindItem("Reagent") as DashboardViewItem;
                            QCDetailView = ((DashboardView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                        }
                        else
                        if (Application.MainWindow.View is DetailView)
                        {
                            QCReagent = ((DetailView)Application.MainWindow.View).FindItem("Reagent") as DashboardViewItem;
                            QCDetailView = ((DetailView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                        }
                        view.ObjectSpace.CommitChanges();
                        if (QCDetailView != null && QCDetailView.InnerView != null)
                        {
                            SpreadSheetEntry_AnalyticalBatch objQc = ((SpreadSheetEntry_AnalyticalBatch)QCDetailView.InnerView.CurrentObject);
                            if (objQc != null && !objQc.Reagents.Contains(ObjReagent))
                            {
                                objQc.Reagents.Add(QCDetailView.InnerView.ObjectSpace.GetObject(ObjReagent));
                                ((ListView)QCReagent.InnerView).CollectionSource.Add(QCReagent.InnerView.ObjectSpace.GetObject(ObjReagent));
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

                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                if (View.Id == "QCBatchSequence_ListView")
                {
                    ASPxGridListEditor GridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (GridListEditor != null)
                    {
                        GridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                        GridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        GridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        //GridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        //{
                        //    var fieldName = e.cellInfo.column.fieldName;
                        //    sessionStorage.setItem('ItemFocusedColumn', fieldName);
                        //}";


                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (View is DetailView)
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item is ASPxStringPropertyEditor)
                        {
                            ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }

                        }

                        else if (item is ASPxIntPropertyEditor)
                        {
                            ASPxIntPropertyEditor editor = (ASPxIntPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDoublePropertyEditor)
                        {
                            ASPxDoublePropertyEditor editor = (ASPxDoublePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }

                        else if (item is ASPxDecimalPropertyEditor)
                        {
                            ASPxDecimalPropertyEditor editor = (ASPxDecimalPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }

                        }

                        else if (item is ASPxDateTimePropertyEditor)
                        {
                            ASPxDateTimePropertyEditor editor = (ASPxDateTimePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxLookupPropertyEditor)
                        {
                            ASPxLookupPropertyEditor editor = (ASPxLookupPropertyEditor)item;
                            if (editor != null && editor.DropDownEdit != null && editor.DropDownEdit.DropDown != null)
                            {
                                editor.DropDownEdit.DropDown.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxBooleanPropertyEditor)
                        {
                            ASPxBooleanPropertyEditor editor = (ASPxBooleanPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxEnumPropertyEditor)
                        {
                            ASPxEnumPropertyEditor editor = (ASPxEnumPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item is ASPxGridLookupPropertyEditor)
                        {
                            ASPxGridLookupPropertyEditor editor = (ASPxGridLookupPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                            }
                        }
                    }
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                if (View.Id == "QCType_ListView_qcbatchsequence")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                    gridListEditor.Grid.SettingsPager.PageSizeItemSettings.Visible = false;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.ClientInstanceName = "QCType";
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
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
                else if (View.Id == "QCBatchSequence_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (Application.MainWindow.View.Id == "QCBatchsequence")
                    {
                        ActionContainerViewItem qcaction2 = ((DashboardView)Application.MainWindow.View).FindItem("qcaction2") as ActionContainerViewItem;
                        if (qcaction2 != null && (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "序号"))
                        {
                            if (gridListEditor.Grid != null && gridListEditor != null)
                            {
                                gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = false;
                            }
                        }
                        else if (qcaction2 != null && (qcaction2.Actions[1].Caption == "Ok" || qcaction2.Actions[1].Caption == "确定"))
                        {
                            if (gridListEditor.Grid != null && gridListEditor != null)
                            {
                                gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = false;// true;
                            }
                        }
                    }
                    else
                    {
                        //if (Frame is NestedFrame)
                        //{
                        //    NestedFrame nestedFrame = (NestedFrame)Frame;
                        //    if (nestedFrame != null && nestedFrame.ViewItem.View != null)
                        //    {
                        //        CompositeView cv = nestedFrame.ViewItem.View;
                        //    }
                        //}
                        CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                        ActionContainerViewItem qcaction2 = ((DashboardView)cv).FindItem("qcaction2") as ActionContainerViewItem;
                        if (qcaction2 != null && (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "序号"))
                        {
                            if (gridListEditor.Grid != null && gridListEditor != null)
                            {
                                gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = false;
                            }
                        }
                        else if (qcaction2 != null && (qcaction2.Actions[1].Caption == "Ok" || qcaction2.Actions[1].Caption == "确定"))
                        {
                            if (gridListEditor.Grid != null && gridListEditor != null)
                            {
                                gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = false;// true;
                            }
                        }
                    }
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("QC", this);
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.Columns["SampleName"].Width = 150;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.ClientInstanceName = "QCBatchSequence";
                    gridListEditor.Grid.JSProperties["cpViewID"] = View.Id;
                    //                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e) 
                    //                    { 
                    //                        var nav = document.getElementById('LPcell');
                    //                        var sep = document.getElementById('separatorCell');
                    //                        if(nav != null && sep != null) {
                    //                           var totusablescr = screen.width - (sep.offsetWidth + nav.offsetWidth);
                    //alert(totusablescr);
                    //                           s.SetWidth((totusablescr / 100) * 80);         
                    //                        }
                    //                        else {
                    //                            s.SetWidth(800); 
                    //                        }                  
                    //                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                    { 
                        if(e.focusedColumn.fieldName != 'Dilution' && e.focusedColumn.fieldName != 'DilutionCount' && e.focusedColumn.fieldName != 'LayerCount' && e.focusedColumn.fieldName != 'SampleAmount' && e.focusedColumn.fieldName != 'FinalVolume')
                        {
                            e.cancel = true;
                        }        
                    }";

                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {   
                                //sessionStorage.setItem('FocusedColumn', null); 
                                // var fieldName = e.cellInfo.column.fieldName;                       
                                //    sessionStorage.setItem('FocusedColumn', fieldName);      
                                if(sessionStorage.getItem('CurrFocusedColumn') == null)
                                {
                                    sessionStorage.setItem('PrevFocusedColumn', e.cellInfo.column.fieldName);
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }
                                else
                                {
                                    var precolumn = sessionStorage.getItem('CurrFocusedColumn');
                                    sessionStorage.setItem('PrevFocusedColumn', precolumn);                           
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }  
                            }";
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                        { 
                            //if (s.IsRowSelectedOnPage(e.elementIndex))  
                            //{ 
                                var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');  
                                var oid;
                                var text;
                                if(FocusedColumn.includes('.'))
                                {                                       
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText; 
                                    if (e.item.name =='CopyToAllCell' )
                                    {
                                       if (FocusedColumn=='SampleAmount' || FocusedColumn=='FinalVolume')
	                                   {
		                                 for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            //if (s.IsRowSelectedOnPage(i)) 
                                            //{
                                               s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);   
                                            //}
                                         } 
	                                  }
                                     }        
                                 }    
                                   else
                                 {                                                             
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn); 
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        if (FocusedColumn=='FinalVolume' || FocusedColumn=='SampleAmount' )
                                        {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            //if (s.IsRowSelectedOnPage(i)) 
                                            //{
                                                s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                            //}
                                        }
                                      }
                                    }                            
                                 }
                             //}
                             e.processOnServer = false;
                        }";
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared1;
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                    window.setTimeout(function() {   
                    var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');  
                    var Dilutioncount = s.batchEditApi.GetCellValue(e.visibleIndex, 'DilutionCount');
                    var Dilution = s.batchEditApi.GetCellValue(e.visibleIndex, 'Dilution');
                    var SysSampleCode = s.batchEditApi.GetCellValue(e.visibleIndex, 'SYSSamplecode');
                    var SampleAmount = s.batchEditApi.GetCellValue(e.visibleIndex, 'SampleAmount');
                    var FinalVolume = s.batchEditApi.GetCellValue(e.visibleIndex, 'FinalVolume');
                    if (e.visibleIndex != '-1' && (FocusedColumn == 'Dilutioncount' || FocusedColumn == 'Dilution')  )
                    {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) 
                        {
                            //if (s.IsRowSelectedOnPage(e.visibleIndex))
                            
                                if(Dilutioncount >= 0)
                                {
                                      var dilutioncount = sessionStorage.getItem('Dilutioncount');      
                                      if(s.batchEditApi.HasChanges(e.visibleIndex, dilutioncount))
                                      {
                                         RaiseXafCallback(globalCallbackControl,'QC' , 'DilutionCnt|'+ Oidvalue+'|'+Dilutioncount, '', false);
                                         s.batchEditApi.SetCellValue(e.visibleIndex, 'Dilutioncount', dilutioncount);
                                      }
                                      else if(Dilutioncount == 0 || Dilutioncount == 1)
                                      {
                                            RaiseXafCallback(globalCallbackControl,'QC' , 'DilutionCnt|'+ Oidvalue+'|'+Dilutioncount, '', false);
                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Dilutioncount', dilutioncount);
                                      }
                            
                    //                     RaiseXafCallback(globalCallbackControl,'QC' , 'DilutionCnt|'+ Oidvalue+'|'+Dilutioncount, '', false);
                                }
                                ////else if(Dilutioncount < 1)
                                ////{
                                ////    var dilutioncnt = 1;
                                ////    s.batchEditApi.SetCellValue(e.visibleIndex, 'Dilutioncount', dilutioncnt);
                                ////    var dilcnt = sessionStorage.getItem('Dilutioncount'); 
                                ////    alert(dilcnt);
                                ////}
                                if(Dilution != null)
                                {
                                    RaiseXafCallback(globalCallbackControl,'QC' , 'StringDilution|'+ Oidvalue+'|'+Dilution, '', false);
                                }
                                if(SampleAmount != null)
                                {
                                    RaiseXafCallback(globalCallbackControl,'QC' , 'StringSampleAmount|'+ Oidvalue+'|'+SampleAmount, '', false);
                                }
                                if(FinalVolume != null)
                                {
                                    RaiseXafCallback(globalCallbackControl,'QC' , 'StringFinalVolume|'+ Oidvalue+'|'+FinalVolume, '', false);
                                }
                            
                        });
                    }                                  
                    }, 20);}";

                    gridListEditor.Grid.ClientSideEvents.BatchEditConfirmShowing = @"function(s,e) 
                    { 
                        e.cancel = true;
                    }";
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e)
                    {
                        var canRefresh = true;
                        var column = s.GetColumnById('QCremove');     
        
                        if (column != null && column.visible == true)
                        {
                            canRefresh = false;
                        }


                        if (canRefresh == true)
	                    {
		                  if (e.visibleIndex != '-1')
                          {
                            s.batchEditApi.ResetChanges(e.visibleIndex);
                            s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {
                             if (s.IsRowSelectedOnPage(e.visibleIndex)) {
                                RaiseXafCallback(globalCallbackControl, 'QC', 'Selected|' + Oidvalue , '', false);
                             }else{
                                RaiseXafCallback(globalCallbackControl, 'QC', 'UNSelected|' + Oidvalue, '', false);
                             }
                            });
                          }
                          else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                          {
                            RaiseXafCallback(globalCallbackControl, 'QC', 'Selectall', '', false);
                          }
                          else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                          {
                            RaiseXafCallback(globalCallbackControl, 'QC', 'UNSelectall', '', false);
                          }
                          else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.pageRowCount)
                          {
                            RaiseXafCallback(globalCallbackControl, 'QC', 'Selectall', '', false);
                          }
                        }
                    }";
                    if (qcbatchinfo.IsPLMTest)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                    window.setTimeout(function() {   
                    var layercount = s.batchEditApi.GetCellValue(e.visibleIndex, 'LayerCount');
                    var SysSampleCode = s.batchEditApi.GetCellValue(e.visibleIndex, 'SYSSamplecode');
                    if(layercount!=null && layercount==0)
                     {
                         var Count=1
                         s.batchEditApi.SetCellValue(e.visibleIndex, 'LayerCount', Count);
                     }
                    if (e.visibleIndex != '-1')
                    {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) 
                        {
                            {
                                if(layercount > 1)
                                {
                                      var Lycount = sessionStorage.getItem('layercount');      
                                      if(s.batchEditApi.HasChanges(e.visibleIndex, Lycount))
                                      {
                                         
                                         RaiseXafCallback(globalCallbackControl,'QC' , 'lyCnt|'+ Oidvalue+'|'+layercount, '', false);
                                         s.batchEditApi.SetCellValue(e.visibleIndex, 'LayerCount', layercount);
                                      }
                                }
                            }
                        });
                       
                    }                                  
                    }, 20);}";
                    }

                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_DetailView")
                {
                    string js = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');                      
                        if(nav != null && sep != null) 
                        {
                            var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                            s.SetWidth(totusablescr /3);
                        }
                        else 
                        {
                            s.SetWidth(220); 
                        }                      
                    }";

                    if (Application.MainWindow.View.Id == "SDMS")
                    {
                        js = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');                      
                        if(nav != null && sep != null) 
                        {
                            var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                            s.SetWidth(totusablescr /3);
                        }
                        else 
                        {
                            s.SetWidth(220); 
                        }                      
                    }";
                    }

                    if (Frame is NestedFrame)
                    {
                        ActionContainerViewItem qcaction2 = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qcaction2") as ActionContainerViewItem;
                        bool enbstat;

                        //if (qcaction2 != null && qcaction2.Actions.Count > 0 && (qcaction2.Actions[1].Caption == "Ok" || qcaction2.Actions[1].Caption == "确定"))                       
                        if (qcbatchinfo.strqcid != null || (qcaction2 != null && qcaction2.Actions.Count > 0 && (qcaction2.Actions[1].Caption == "Ok" || qcaction2.Actions[1].Caption == "确定")))
                        {
                            if (qcbatchinfo.strqcid != null && qcaction2 != null && qcaction2.Actions.Count > 0 && (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "确定"))
                            {
                                enbstat = true;
                            }
                            else
                            {
                                enbstat = false;
                            }
                        }
                        else
                        {
                            enbstat = true;
                        }
                        ActionContainerViewItem Enblqcaction0 = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qcaction0") as ActionContainerViewItem;
                        ActionContainerViewItem Enblqcaction2 = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qcaction2") as ActionContainerViewItem;
                        ////if (Enblqcaction0 != null && Enblqcaction0.Actions.Count > 0)
                        ////{
                        ////    Enblqcaction0.Actions[0].Enabled.SetItemValue("key", enbstat);
                        ////    Enblqcaction0.Actions[1].Enabled.SetItemValue("key", enbstat);
                        ////}
                        ////if (Enblqcaction2 != null && Enblqcaction2.Actions.Count >0)
                        ////{
                        ////    Enblqcaction2.Actions[0].Enabled.SetItemValue("key", enbstat);
                        ////    Enblqcaction2.Actions[1].Enabled.SetItemValue("key", enbstat);
                        ////}
                        ((SpreadSheetEntry_AnalyticalBatch)View.CurrentObject).ISShown = enbstat;
                        foreach (ViewItem item in ((DetailView)View).Items)
                        {
                            if (item.GetType() == typeof(ASPxStringPropertyEditor))
                            {
                                ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    if (propertyEditor.Editor.GetType() != typeof(DevExpress.Web.ASPxMemo))
                                    {
                                        ASPxTextBox textBox = (ASPxTextBox)propertyEditor.Editor;
                                        if (textBox != null)
                                        {
                                            textBox.ClientSideEvents.Init = js;
                                            textBox.ClientInstanceName = propertyEditor.Id;
                                        }
                                    }
                                    else
                                    {
                                        DevExpress.Web.ASPxMemo memoBox = (DevExpress.Web.ASPxMemo)propertyEditor.Editor;
                                        if (memoBox != null)
                                        {
                                            memoBox.ClientSideEvents.Init = js;
                                            memoBox.ClientInstanceName = propertyEditor.Id;
                                        }
                                    }
                                    if (propertyEditor.AllowEdit)
                                    {
                                        propertyEditor.Editor.BackColor = Color.LightYellow;
                                    }
                                }
                            }
                            else if (item.GetType() == typeof(AspxGridLookupCustomEditor))
                            {
                                AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null && item.Id == "NPJobid")
                                {
                                    //if (qcbatchinfo.strqcid == null)
                                    //{
                                    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    //}
                                    //else
                                    //{
                                    //    propertyEditor.AllowEdit.SetItemValue("stat", true);
                                    //}
                                    ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                    SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                                    if (qC != null)
                                    {
                                        if (gridLookup != null)
                                        {
                                            if (Application.MainWindow.View.Id == "SDMS")
                                            {
                                                gridLookup.JSProperties["cpJobID"] = qC.Jobid;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                            }

                                            else
                                            {
                                                gridLookup.JSProperties["cpJobID"] = qC.Jobid;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 3);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                            }

                                            gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                            gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                            gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                            gridLookup.ValueChanged += GridLookup_ValueChanged;
                                            gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                            gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "JobID", Caption = rm.GetString("JobID_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Sx" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TAT" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DueDate", Caption = rm.GetString("DueDate_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DateReceived", Caption = rm.GetString("DateReceived_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns["JobID"].Width = 100;
                                            gridLookup.GridView.Columns["Sx"].Width = 70;
                                            gridLookup.GridView.Columns["TAT"].Width = 70;
                                            gridLookup.GridView.Columns["DueDate"].Width = 100;
                                            gridLookup.GridView.Columns["DateReceived"].Width = 100;
                                            gridLookup.GridView.KeyFieldName = "JobID";
                                            gridLookup.TextFormatString = "{0}";
                                            //gridLookup.GridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared
                                            gridLookup.GridView.HtmlRowPrepared += Grid_HtmlRowPrepared;
                                            DataTable table = new DataTable();
                                            table.Columns.Add("JobID");
                                            table.Columns.Add("Sx");
                                            table.Columns.Add("TAT");
                                            table.Columns.Add("DueDate");
                                            table.Columns.Add("DateReceived");


                                            //if (qC.Test != null && qC.Method != null && (qC.QCBatchID == "Auto Generate" || qC.QCBatchID == "自动生成"))
                                            if (qC.Test != null && qC.Method != null) // && (qC.QCBatchID == "Auto Generate" || qC.QCBatchID == "自动生成")
                                            {
                                                IList<SampleParameter> samples = new List<SampleParameter>();
                                                int testprepmethodd = 0;
                                                //TestMethod objtestmethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", qC.Test.Oid));
                                                TestMethod objtestmethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ? And [MethodName.MethodNumber] = ? And[MatrixName.MatrixName] = ?", qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.Matrix.MatrixName));
                                                if (objtestmethod != null && objtestmethod.PrepMethods.Count > 0)
                                                {
                                                    testprepmethodd = objtestmethod.PrepMethods.Count;
                                                }
                                                //objDefault = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True"));
                                                if (objnavigationRefresh.ClickedNavigationItem == "QCbatch")
                                                {
                                                    if (/*objDefault != null &&*/ testprepmethodd > 0)
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? /*And [IsPrepMethodComplete]  = True*/ And [SignOff] = True AND [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False) And (([UQABID] Is Null And [QCBatchID] Is Null) || ([QCBatchID.qcseqdetail.AnalyticalBatchID] = ?)) And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False)", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.AnalyticalBatchID));
                                                    }
                                                    else
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [SignOff] = True And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False) And (([UQABID] Is Null And [QCBatchID] Is Null) || ([QCBatchID.qcseqdetail.AnalyticalBatchID] = ?)) And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False)", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.AnalyticalBatchID));
                                                    }

                                                }
                                                else
                                                {
                                                    if (/*objDefault != null &&*/ testprepmethodd > 0)
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.GCRecord] Is Null And [SignOff] = True And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False)  And [UQABID] Is Null And [QCBatchID] Is Null /*AND [IsPrepMethodComplete]  = True*/ And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False )", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber));
                                                    }
                                                    else
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.GCRecord] Is Null And [SignOff] = True And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False)  And [UQABID] Is Null And [QCBatchID] Is Null And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False )", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber));
                                                    }
                                                }
                                                if (samples != null && samples.Count > 0)
                                                {
                                                    if (objnavigationRefresh.ClickedNavigationItem == "QCbatches")
                                                    {
                                                        foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.Samplelogin.JobID != null && a.SubOut != true /*&& a.SamplePrepBatchID != null */&& !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                        {
                                                            if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.TAT != null)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null });
                                                            }
                                                        }
                                                        DataView dv = new DataView(table);
                                                        dv.Sort = "DueDate Asc";
                                                        table = dv.ToTable();
                                                    }
                                                    else
                                                    {
                                                        foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.SignOff == true && a.Samplelogin.JobID != null && a.SubOut != true && !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                        {
                                                            if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.TAT != null)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null });
                                                            }
                                                            //if (!objsample.Samplelogin.JobID.IsSampling)
                                                            //{
                                                            //    table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.TestHold == false && a.Samplelogin.JobID != null && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            //}
                                                            //else
                                                            //{

                                                            //}
                                                        }
                                                        DataView dv = new DataView(table);
                                                        dv.Sort = "DueDate Asc";
                                                        table = dv.ToTable();
                                                    }
                                                }
                                                else if (samples == null && qC.Jobid != null)
                                                {
                                                    string[] ids = qC.Jobid.Split(';');
                                                    foreach (string id in ids)
                                                    {
                                                        Samplecheckin sample = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Jobid]=?", id));
                                                        if (sample != null && !string.IsNullOrEmpty(sample.JobID))
                                                        {
                                                            table.Rows.Add(new object[] { sample.JobID, 0 });
                                                        }
                                                    }
                                                }
                                            }
                                            gridLookup.GridView.DataSource = table;
                                            gridLookup.GridView.DataBind();
                                        }
                                    }
                                }
                                else if (propertyEditor != null && propertyEditor.Editor != null && item.Id == "NPInstrument")
                                {
                                    propertyEditor.AllowEdit.SetItemValue("NPInstrument", enbstat);
                                    ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                    SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                                    if (qC != null)
                                    {
                                        if (gridLookup != null)
                                        {
                                            if (Application.MainWindow.View.Id == "SDMS")
                                            {
                                                gridLookup.JSProperties["cpNPInstrument"] = qC.strInstrument;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpNPInstrument);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                            }

                                            else
                                            {
                                                gridLookup.JSProperties["cpNPInstrument"] = qC.strInstrument;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpNPInstrument);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 3);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                            }
                                            gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                            gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                            gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                            gridLookup.ValueChanged += GridLookup_ValueChanged;
                                            gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                            gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Oid" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Instrument" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "InstrumentID" });
                                            gridLookup.GridView.Columns["Instrument"].Width = 150;
                                            gridLookup.GridView.Columns["InstrumentID"].Width = 150;
                                            gridLookup.GridView.KeyFieldName = "Oid";
                                            gridLookup.TextFormatString = "{2}";
                                            gridLookup.GridView.Columns["Oid"].Visible = false;
                                            DataTable table = new DataTable();
                                            table.Columns.Add("Oid");
                                            table.Columns.Add("Instrument");
                                            table.Columns.Add("InstrumentID");
                                            if (qC.Test != null && qC.Method != null)
                                            {
                                                foreach (Labware objlab in qC.Method.Labwares.OrderBy(a => a.AssignedName).Distinct().ToList())
                                                {
                                                    table.Rows.Add(new object[] { objlab.Oid, objlab.LabwareName, objlab.AssignedName });
                                                }
                                                DataView dv = new DataView(table);
                                                dv.Sort = "Instrument Asc";
                                                table = dv.ToTable();
                                            }
                                            gridLookup.GridView.DataSource = table;
                                            gridLookup.GridView.DataBind();
                                        }
                                    }
                                }
                                if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                            else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                            {
                                ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                                if (item.Id != "Instrument")
                                {
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                        ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                        if (gridLookup != null)
                                        {
                                            gridLookup.ClientSideEvents.Init = js;
                                            gridLookup.ClientInstanceName = propertyEditor.Id;
                                        }
                                    }
                                    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.BackColor = Color.LightYellow;
                                    }
                                }
                                else
                                {
                                    if (propertyEditor != null && propertyEditor.Editor != null)
                                    {
                                        if (propertyEditor.Id != "Instrument")
                                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                        ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                        if (gridLookup != null)
                                        {
                                            gridLookup.ClientSideEvents.Init = js;
                                            gridLookup.ClientInstanceName = propertyEditor.Id;
                                        }
                                    }
                                    if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                    {
                                        propertyEditor.Editor.BackColor = Color.LightYellow;
                                    }
                                }
                            }
                            else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                            {
                                ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    propertyEditor.Editor.ClientSideEvents.Init = js;
                                    propertyEditor.Editor.ClientInstanceName = propertyEditor.Id;
                                }
                                if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                            else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                            {
                                ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    propertyEditor.Editor.ClientSideEvents.Init = js;
                                    propertyEditor.Editor.ClientInstanceName = propertyEditor.Id;
                                }
                            }
                            else if (item.GetType() == typeof(FileDataPropertyEditor))
                            {
                                FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                }
                            }
                            else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                            {
                                ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    propertyEditor.Editor.ClientSideEvents.Init = js;
                                    propertyEditor.Editor.ClientInstanceName = propertyEditor.Id;
                                }
                            }
                            else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                            {
                                ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    if (qcbatchinfo.strqcid != null && qcaction2 != null && qcaction2.Actions.Count > 0 && (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "确定"))
                                    {
                                        propertyEditor.AllowEdit.SetItemValue("stat", false);
                                    }
                                    else
                                    {
                                        propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    }
                                    //if (qcbatchinfo.strqcid == null)
                                    //{
                                    //    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    //}
                                    //else
                                    //{
                                    //    propertyEditor.AllowEdit.SetItemValue("stat", false);
                                    //}
                                    if (propertyEditor.FindEdit != null && propertyEditor.FindEdit.Visible)
                                    {
                                        propertyEditor.FindEdit.Editor.ClientSideEvents.Init = js;
                                        propertyEditor.FindEdit.Editor.ClientInstanceName = propertyEditor.Id;
                                    }
                                    else if (propertyEditor.DropDownEdit != null)
                                    {
                                        propertyEditor.DropDownEdit.DropDown.ClientSideEvents.Init = js;
                                        propertyEditor.DropDownEdit.DropDown.ClientInstanceName = propertyEditor.Id;
                                    }
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
                                }
                            }
                            else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                            {
                                ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                    propertyEditor.Editor.ClientSideEvents.Init = js;
                                    propertyEditor.Editor.ClientInstanceName = propertyEditor.Id;
                                }
                                if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                        }
                    }

                    SpreadSheetEntry_AnalyticalBatch objQcbatch = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                    if (objQcbatch != null && objQcbatch.Test != null && objQcbatch.Test.IsPLMTest) //&& objQcbatch.Test.TestName.StartsWith("PLM")
                    {
                        qcbatchinfo.IsPLMTest = true;

                    }
                    else
                    {
                        qcbatchinfo.IsPLMTest = false;
                    }
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_DetailView_QCBatchID")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    sheet.CallbackManager.RegisterHandler("openspreadsheet", this);
                    //DashboardViewItem qcdetail = ((DetailView)View).FindItem("qcdetail") as DashboardViewItem;
                    DashboardViewItem qctype = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                    DashboardViewItem qclist = ((DetailView)View).FindItem("qclist") as DashboardViewItem;
                    //DashboardViewItem QCDetailViewComment = ((DashboardView)View).FindItem("Others") as DashboardViewItem;
                    //if (qcdetail != null && qcdetail.InnerView != null)
                    {
                        foreach (ViewItem item in ((DetailView)View).Items)
                        {
                            if (item.GetType() == typeof(AspxGridLookupCustomEditor) && item.Id == "NPJobid")
                            {
                                AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                    SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                                    if (qC != null)
                                    {
                                        if (gridLookup != null)
                                        {
                                            if (Application.MainWindow.View.Id == "SDMS")
                                            {
                                                gridLookup.JSProperties["cpJobID"] = qC.Jobid;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(270); 
                                            }                      
                                            }";
                                            }

                                            else
                                            {
                                                gridLookup.JSProperties["cpJobID"] = qC.Jobid;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(145); 
                                            }                      
                                            }";
                                            }

                                            gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                            gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                            gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                            gridLookup.ValueChanged += GridLookup_ValueChanged;
                                            gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                            gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "JobID", Caption = rm.GetString("JobID_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Sx" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TAT" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DueDate", Caption = rm.GetString("DueDate_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DateReceived", Caption = rm.GetString("DateReceived_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns["JobID"].Width = 100;
                                            gridLookup.GridView.Columns["Sx"].Width = 70;
                                            gridLookup.GridView.Columns["TAT"].Width = 70;
                                            gridLookup.GridView.Columns["DueDate"].Width = 100;
                                            gridLookup.GridView.Columns["DateReceived"].Width = 100;
                                            gridLookup.GridView.KeyFieldName = "JobID";
                                            gridLookup.TextFormatString = "{0}";
                                            //gridLookup.GridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared
                                            gridLookup.GridView.HtmlRowPrepared += Grid_HtmlRowPrepared;
                                            DataTable table = new DataTable();
                                            table.Columns.Add("JobID");
                                            table.Columns.Add("Sx");
                                            table.Columns.Add("TAT");
                                            table.Columns.Add("DueDate");
                                            table.Columns.Add("DateReceived");


                                            //if (qC.Test != null && qC.Method != null && (qC.QCBatchID == "Auto Generate" || qC.QCBatchID == "自动生成"))
                                            if (qC.Test != null && qC.Method != null) // && (qC.QCBatchID == "Auto Generate" || qC.QCBatchID == "自动生成")
                                            {
                                                IList<SampleParameter> samples = new List<SampleParameter>();
                                                int testprepmethodd = 0;
                                                //TestMethod objtestmethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", qC.Test.Oid));
                                                TestMethod objtestmethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ? And [MethodName.MethodNumber] = ? And[MatrixName.MatrixName] = ?", qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.Matrix.MatrixName));
                                                if (objtestmethod != null && objtestmethod.PrepMethods.Count > 0)
                                                {
                                                    testprepmethodd = objtestmethod.PrepMethods.Count;
                                                }
                                                objDefault = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True"));
                                                if (objnavigationRefresh.ClickedNavigationItem == "QCbatch")
                                                {
                                                    if (objDefault != null && testprepmethodd > 0)
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [IsPrepMethodComplete]  = True And [SignOff] = True AND [IsTransferred] = true And (([UQABID] Is Null And [QCBatchID] Is Null) || ([QCBatchID.qcseqdetail.AnalyticalBatchID] = ?))", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.AnalyticalBatchID));
                                                    }
                                                    else
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [SignOff] = True And [IsTransferred] = true And (([UQABID] Is Null And [QCBatchID] Is Null) || ([QCBatchID.qcseqdetail.AnalyticalBatchID] = ?))", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.AnalyticalBatchID));
                                                    }

                                                }
                                                else
                                                {
                                                    if (objDefault != null && testprepmethodd > 0)
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.GCRecord] Is Null And [SignOff] = True And [IsTransferred] = true And [UQABID] Is Null And [QCBatchID] Is Null AND [IsPrepMethodComplete]  = True", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber));
                                                    }
                                                    else
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.GCRecord] Is Null And [SignOff] = True And [IsTransferred] = true And [UQABID] Is Null And [QCBatchID] Is Null", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber));
                                                    }
                                                }
                                                if (samples != null && samples.Count > 0)
                                                {
                                                    if (objnavigationRefresh.ClickedNavigationItem == "QCbatch")
                                                    {
                                                        foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.Samplelogin.JobID != null /*&& a.SamplePrepBatchID != null */&& !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                        {
                                                            if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.TAT != null)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null });
                                                            }
                                                        }
                                                        DataView dv = new DataView(table);
                                                        dv.Sort = "DueDate Asc";
                                                        table = dv.ToTable();
                                                    }
                                                    else
                                                    {
                                                        foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.SignOff == true && a.Samplelogin.JobID != null && !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                        {
                                                            if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.TAT != null)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null });
                                                            }
                                                            //if (objsample.Samplelogin.JobID.TAT != null)
                                                            //{
                                                            //    table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            //}
                                                            //else
                                                            //{
                                                            //    table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            //}
                                                        }
                                                        DataView dv = new DataView(table);
                                                        dv.Sort = "DueDate Asc";
                                                        table = dv.ToTable();
                                                    }
                                                }
                                                else if (samples == null && qC.Jobid != null)
                                                {
                                                    string[] ids = qC.Jobid.Split(';');
                                                    foreach (string id in ids)
                                                    {
                                                        Samplecheckin sample = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Jobid]=?", id));
                                                        if (sample != null && !string.IsNullOrEmpty(sample.JobID))
                                                        {
                                                            table.Rows.Add(new object[] { sample.JobID, 0 });
                                                        }
                                                    }
                                                }
                                            }
                                            gridLookup.GridView.DataSource = table;
                                            gridLookup.GridView.DataBind();
                                        }
                                    }
                                }
                                else if (propertyEditor != null && propertyEditor.Editor != null && item.Id == "NPInstrument")
                                {
                                    ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                    SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                                    if (qC != null)
                                    {
                                        if (gridLookup != null)
                                        {
                                            if (Application.MainWindow.View.Id == "SDMS")
                                            {
                                                gridLookup.JSProperties["cpNPInstrument"] = qC.strInstrument;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpNPInstrument);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                            }

                                            else
                                            {
                                                gridLookup.JSProperties["cpNPInstrument"] = qC.strInstrument;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpNPInstrument);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 3);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                            }
                                            gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                            gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                            gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                            gridLookup.ValueChanged += GridLookup_ValueChanged;
                                            gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                            gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Oid" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Instrument" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "InstrumentID" });
                                            gridLookup.GridView.Columns["Instrument"].Width = 150;
                                            gridLookup.GridView.Columns["InstrumentID"].Width = 150;
                                            gridLookup.GridView.KeyFieldName = "Oid";
                                            gridLookup.TextFormatString = "{2}";
                                            gridLookup.GridView.Columns["Oid"].Visible = false;
                                            DataTable table = new DataTable();
                                            table.Columns.Add("Oid");
                                            table.Columns.Add("Instrument");
                                            table.Columns.Add("InstrumentID");
                                            if (qC.Test != null && qC.Method != null)
                                            {
                                                foreach (Labware objlab in qC.Method.Labwares.OrderBy(a => a.AssignedName).Distinct().ToList())
                                                {
                                                    table.Rows.Add(new object[] { objlab.Oid, objlab.LabwareName, objlab.AssignedName });
                                                }
                                                DataView dv = new DataView(table);
                                                dv.Sort = "Instrument Asc";
                                                table = dv.ToTable();
                                            }
                                            gridLookup.GridView.DataSource = table;
                                            gridLookup.GridView.DataBind();
                                        }
                                    }
                                }
                                if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                        }
                        SpreadSheetEntry_AnalyticalBatch qCBatch = (SpreadSheetEntry_AnalyticalBatch)((DetailView)View).CurrentObject;
                        if (qclist != null && qCBatch != null && qctype != null)
                        {
                            if (qctype.InnerView == null)
                            {
                                qctype.CreateControl();
                            }
                            if (qclist.InnerView == null)
                            {
                                qclist.CreateControl();
                            }
                            if (qcbatchinfo.canfilter)
                            {
                                if (!string.IsNullOrEmpty(qCBatch.AnalyticalBatchID))
                                {
                                    List<string> lstqctype = new List<string>();
                                    foreach (Testparameter TP in qCBatch.Test.TestParameter)
                                    {
                                        if (TP.QCType != null && !lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample" && qCBatch.Test != null && qCBatch.Test.QCTypes.Contains(TP.QCType))
                                        //if (!lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample")
                                        {
                                            lstqctype.Add(TP.QCType.QCTypeName);
                                        }
                                    }
                                    ((ListView)qctype.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCTypeName] In(" + string.Format("'{0}'", string.Join("','", lstqctype)) + ")");
                                }
                                else
                                {
                                    ((ListView)qctype.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                                }
                                //((ListView)qclist.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[qcseqdetail] is null or [qcseqdetail]=?", qCBatch.Oid);
                                ((ListView)qclist.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[qcseqdetail]=?", qCBatch.Oid);
                            }
                        }
                    }
                }
                else if (View.Id == "QCBatchsequence")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    sheet = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    sheet.CallbackManager.RegisterHandler("openspreadsheet", this);
                    DashboardViewItem qcdetail = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                    DashboardViewItem qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                    DashboardViewItem qclist = ((DashboardView)View).FindItem("qclist") as DashboardViewItem;
                    DashboardViewItem QCDetailViewComment = ((DashboardView)View).FindItem("Others") as DashboardViewItem;
                    if (qcdetail != null && qcdetail.InnerView != null)
                    {
                        foreach (ViewItem item in ((DetailView)qcdetail.InnerView).Items)
                        {
                            if (item.GetType() == typeof(AspxGridLookupCustomEditor) && item.Id == "NPJobid")
                            {
                                AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                                if (propertyEditor != null && propertyEditor.Editor != null)
                                {
                                    ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                    SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                                    if (qC != null)
                                    {
                                        if (gridLookup != null)
                                        {
                                            if (Application.MainWindow.View.Id == "SDMS")
                                            {
                                                gridLookup.JSProperties["cpJobID"] = qC.Jobid;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(270); 
                                            }                      
                                            }";
                                            }

                                            else
                                            {
                                                gridLookup.JSProperties["cpJobID"] = qC.Jobid;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(145); 
                                            }                      
                                            }";
                                            }

                                            gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                            gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                            gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                            gridLookup.ValueChanged += GridLookup_ValueChanged;
                                            gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                            gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "JobID", Caption = rm.GetString("JobID_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Sx" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TAT" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DueDate", Caption = rm.GetString("DueDate_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DateReceived", Caption = rm.GetString("DateReceived_" + CurrentLanguage) });
                                            gridLookup.GridView.Columns["JobID"].Width = 100;
                                            gridLookup.GridView.Columns["Sx"].Width = 70;
                                            gridLookup.GridView.Columns["TAT"].Width = 70;
                                            gridLookup.GridView.Columns["DueDate"].Width = 100;
                                            gridLookup.GridView.Columns["DateReceived"].Width = 100;
                                            gridLookup.GridView.KeyFieldName = "JobID";
                                            gridLookup.TextFormatString = "{0}";
                                            //gridLookup.GridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared
                                            gridLookup.GridView.HtmlRowPrepared += Grid_HtmlRowPrepared;
                                            DataTable table = new DataTable();
                                            table.Columns.Add("JobID");
                                            table.Columns.Add("Sx");
                                            table.Columns.Add("TAT");
                                            table.Columns.Add("DueDate");
                                            table.Columns.Add("DateReceived");


                                            //if (qC.Test != null && qC.Method != null && (qC.QCBatchID == "Auto Generate" || qC.QCBatchID == "自动生成"))
                                            if (qC.Test != null && qC.Method != null) // && (qC.QCBatchID == "Auto Generate" || qC.QCBatchID == "自动生成")
                                            {
                                                IList<SampleParameter> samples = new List<SampleParameter>();
                                                int testprepmethodd = 0;
                                                //TestMethod objtestmethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] = ?", qC.Test.Oid));
                                                TestMethod objtestmethod = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] = ? And [MethodName.MethodNumber] = ? And[MatrixName.MatrixName] = ?", qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.Matrix.MatrixName));
                                                if (objtestmethod != null && objtestmethod.PrepMethods.Count > 0)
                                                {
                                                    testprepmethodd = objtestmethod.PrepMethods.Count;
                                                }
                                                objDefault = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True"));
                                                if (objnavigationRefresh.ClickedNavigationItem == "QCbatches")
                                                {
                                                    if (objDefault != null && testprepmethodd > 0)
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [IsPrepMethodComplete]  = True And [SignOff] = True AND [IsTransferred] = true And (([UQABID] Is Null And [QCBatchID] Is Null) || ([QCBatchID.qcseqdetail.AnalyticalBatchID] = ?))", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.AnalyticalBatchID));
                                                    }
                                                    else
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [SignOff] = True And [IsTransferred] = true And (([UQABID] Is Null And [QCBatchID] Is Null) || ([QCBatchID.qcseqdetail.AnalyticalBatchID] = ?))", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber, qC.AnalyticalBatchID));
                                                    }

                                                }
                                                else
                                                {
                                                    if (objDefault != null && testprepmethodd > 0)
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.GCRecord] Is Null And [SignOff] = True And [IsTransferred] = true And [UQABID] Is Null And [QCBatchID] Is Null AND [IsPrepMethodComplete]  = True", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber));
                                                    }
                                                    else
                                                    {
                                                        samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.GCRecord] Is Null And [SignOff] = True And [IsTransferred] = true And [UQABID] Is Null And [QCBatchID] Is Null", qC.Matrix.MatrixName, qC.Test.TestName, qC.Method.MethodName.MethodNumber));
                                                    }
                                                }
                                                if (samples != null && samples.Count > 0)
                                                {
                                                    if (objnavigationRefresh.ClickedNavigationItem == "QCbatches")
                                                    {
                                                        foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.Samplelogin.JobID != null /*&& a.SamplePrepBatchID != null */&& !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                        {
                                                            if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.TAT != null)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null });
                                                            }
                                                        }
                                                        DataView dv = new DataView(table);
                                                        dv.Sort = "DueDate Asc";
                                                        table = dv.ToTable();
                                                    }
                                                    else
                                                    {
                                                        foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.SignOff == true && a.Samplelogin.JobID != null && !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                        {
                                                            if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.TAT != null)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null });
                                                            }
                                                            else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy") });
                                                            }
                                                            else
                                                            {
                                                                table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null });
                                                            }
                                                        }
                                                        DataView dv = new DataView(table);
                                                        dv.Sort = "DueDate Asc";
                                                        table = dv.ToTable();
                                                    }
                                                }
                                                else if (samples == null && qC.Jobid != null)
                                                {
                                                    string[] ids = qC.Jobid.Split(';');
                                                    foreach (string id in ids)
                                                    {
                                                        Samplecheckin sample = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Jobid]=?", id));
                                                        if (sample != null && !string.IsNullOrEmpty(sample.JobID))
                                                        {
                                                            table.Rows.Add(new object[] { sample.JobID, 0 });
                                                        }
                                                    }
                                                }
                                            }
                                            gridLookup.GridView.DataSource = table;
                                            gridLookup.GridView.DataBind();
                                        }
                                    }
                                }
                                else if (propertyEditor != null && propertyEditor.Editor != null && item.Id == "NPInstrument")
                                {
                                    ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                    SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                                    if (qC != null)
                                    {
                                        if (gridLookup != null)
                                        {
                                            if (Application.MainWindow.View.Id == "SDMS")
                                            {
                                                gridLookup.JSProperties["cpNPInstrument"] = qC.strInstrument;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpNPInstrument);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 2);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                            }

                                            else
                                            {
                                                gridLookup.JSProperties["cpNPInstrument"] = qC.strInstrument;
                                                gridLookup.ClientInstanceName = propertyEditor.Id;
                                                gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpNPInstrument);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                                var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                                s.SetWidth(totusablescr / 3);
                                            }
                                            else 
                                            {
                                                s.SetWidth(220); 
                                            }                      
                                            }";
                                            }
                                            gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                            gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                            gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                            gridLookup.ValueChanged += GridLookup_ValueChanged;
                                            gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                            gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Oid" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Instrument" });
                                            gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "InstrumentID" });
                                            gridLookup.GridView.Columns["Instrument"].Width = 150;
                                            gridLookup.GridView.Columns["InstrumentID"].Width = 150;
                                            gridLookup.GridView.KeyFieldName = "Oid";
                                            gridLookup.TextFormatString = "{2}";
                                            gridLookup.GridView.Columns["Oid"].Visible = false;
                                            DataTable table = new DataTable();
                                            table.Columns.Add("Oid");
                                            table.Columns.Add("Instrument");
                                            table.Columns.Add("InstrumentID");
                                            if (qC.Test != null && qC.Method != null)
                                            {
                                                foreach (Labware objlab in qC.Method.Labwares.OrderBy(a => a.AssignedName).Distinct().ToList())
                                                {
                                                    table.Rows.Add(new object[] { objlab.Oid, objlab.LabwareName, objlab.AssignedName });
                                                }
                                                DataView dv = new DataView(table);
                                                dv.Sort = "Instrument Asc";
                                                table = dv.ToTable();
                                            }
                                            gridLookup.GridView.DataSource = table;
                                            gridLookup.GridView.DataBind();
                                        }
                                    }
                                }
                                if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                                {
                                    propertyEditor.Editor.BackColor = Color.LightYellow;
                                }
                            }
                        }
                        SpreadSheetEntry_AnalyticalBatch qCBatch = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                        if (qclist != null && qCBatch != null && qctype != null)
                        {
                            if (qctype.InnerView == null)
                            {
                                qctype.CreateControl();
                            }
                            if (qclist.InnerView == null)
                            {
                                qclist.CreateControl();
                            }
                            if (qcbatchinfo.canfilter)
                            {
                                if (!string.IsNullOrEmpty(qCBatch.AnalyticalBatchID))
                                {
                                    List<string> lstqctype = new List<string>();
                                    foreach (Testparameter TP in qCBatch.Test.TestParameter)
                                    {
                                        if (TP.QCType != null && !lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample" && qCBatch.Test != null && qCBatch.Test.QCTypes.Contains(TP.QCType))
                                        //if (!lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample")
                                        {
                                            lstqctype.Add(TP.QCType.QCTypeName);
                                        }
                                    }
                                    ((ListView)qctype.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCTypeName] In(" + string.Format("'{0}'", string.Join("','", lstqctype)) + ")");
                                }
                                else
                                {
                                    ((ListView)qctype.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                                }
                                //((ListView)qclist.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[qcseqdetail] is null or [qcseqdetail]=?", qCBatch.Oid);
                                ((ListView)qclist.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[qcseqdetail]=?", qCBatch.Oid);
                            }
                        }
                    }
                    qcload.SetClientScript("sessionStorage.setItem('AllowSelectionByDataCell', false);");
                    QCsort.SetClientScript("sessionStorage.setItem('AllowSelectionByDataCell', true);");
                    if (objnavigationRefresh.ClickedNavigationItem == "QCbatch" || objnavigationRefresh.ClickedNavigationItem == "AnalysisQueue" || objnavigationRefresh.ClickedNavigationItem == "AnalysisQueue " || objnavigationRefresh.ClickedNavigationItem == "Spreadsheet")
                    {
                        if (CurrentLanguage == "En")
                        {
                            //View.Caption = "Analytical Batch";
                            View.Caption = "QC Batch";
                        }
                        else
                        {
                            View.Caption = "分析批次";
                        }

                        OpenResultEntry.Active["ShowOpenResultEntry"] = false;
                    }
                    //else if (objnavigationRefresh.ClickedNavigationItem == "Asbestos_PLM")
                    //{
                    //    View.Caption = "Asbestos_PLM";
                    //    OpenResultEntry.Active["ShowOpenResultEntry"] = false;
                    //}
                }
                else if (View.Id == "QCBatch_ListView" || View.Id == "QCBatch_ListView_Copy")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid.Columns["InlineEditCommandColumn"] != null)
                    {
                        gridListEditor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                    }

                    if (objnavigationRefresh.ClickedNavigationItem == "QCbatch" || objnavigationRefresh.ClickedNavigationItem == "Spreadsheet")
                    {
                        if (CurrentLanguage == "En")
                        {
                            View.Caption = "Analytical Batches";
                        }
                        else
                        {
                            View.Caption = "分析批次";
                        }

                    }
                }
                if (View.Id == "TestMethod_ListView_AnalysisQueue")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                        {
                            column.CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
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

        private void Grid_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {

                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    e.Items.Remove(e.Items.FindByText("Edit"));
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";//"navigation_home_16x16";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_HtmlDataCellPrepared1(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (View.Id == "QCBatchSequence_ListView")
            {
                if (e.DataColumn.FieldName != "Dilution" && e.DataColumn.FieldName != "DilutionCount" && e.DataColumn.FieldName != "LayerCount" && e.DataColumn.FieldName != "SampleAmount" && e.DataColumn.FieldName != "FinalVolume")
                {
                    e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'QC', 'Rowselect|'+{0}, '', false)", e.VisibleIndex));
                }
            }
        }

        private void Editor_ControlsCreated(object sender, EventArgs e)
        {

        }

        private void Grid_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {

                ASPxGridView grid = sender as ASPxGridView;
                if (e.RowType == GridViewRowType.Data)
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    string strDueDate = grid.GetRowValues(e.VisibleIndex, "DueDate").ToString();
                    if (!string.IsNullOrEmpty(strDueDate))
                    {
                        DateTime DTduedate = DateTime.ParseExact(strDueDate, "MM/dd/yy", provider);
                        ////string[] strDueDatearr = DTduedate.ToString().Substring(0, 10).Split('/');
                        ////string strdd = strDueDatearr[0] + "/" + strDueDatearr[1] + "/" + strDueDatearr[2];
                        ////DateTime duedate = DateTime.ParseExact(strdd, "dd/MM/yyyy", provider);
                        var intCountDays = ((DateTime)DTduedate).Subtract(DateTime.Today).Days;
                        if (intCountDays == 0 || intCountDays < 0)
                        {
                            e.Row.BackColor = System.Drawing.Color.IndianRed;
                            e.Row.ForeColor = System.Drawing.Color.White;
                        }
                        else if (intCountDays == 1)
                        {
                            e.Row.BackColor = System.Drawing.Color.Yellow;
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

            //ASPxGridView grid = sender as ASPxGridView;
            //if (e.RowType == GridViewRowType.Data)
            //{
            //    string status = grid.GetRowValues(e.VisibleIndex, "Status").ToString();
            //    if (!string.IsNullOrEmpty(status))
            //    {
            //        if (status == "Done")
            //        {
            //            e.Row.BackColor = System.Drawing.Color.LightGreen;
            //        }
            //        else if (status == "Partial")
            //        {
            //            e.Row.BackColor = System.Drawing.Color.Orange;
            //        }
            //        else if (status == "Pending")
            //        {
            //            e.Row.BackColor = System.Drawing.Color.Red;
            //            e.Row.ForeColor = System.Drawing.Color.White;
            //        }
            //    }
            //}
            //DateTime status = grid.GetRowValues(e.VisibleIndex, "DueDate").ToString();
            //DateTime strDueDate = e.GetValue(Convert.ToString(DateTime.Now.ToString("DueDate")));
            //var TAT = ((DateTime)objTask.DueDate).Subtract(DateTime.Today).Days;
            //DateTime Date = DateTime.Now; //Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy"));
            ////string crtdate = DateTime.Now.ToString("MM/dd/yyyy");
            //if (strDueDate >= Date)
            //{
            //    e.Cell.BackColor = System.Drawing.Color.Red;
            //}
            //else if(strDueDate <= DateTime.Today.AddDays(1))
            //{
            //    e.Cell.BackColor = System.Drawing.Color.Yellow;
            //}


        }

        private void GridLookup_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridLookup grid = sender as ASPxGridLookup;
                if (grid != null && grid.GridView != null)
                {
                    if (grid.GridView.KeyFieldName == "JobID")
                    {
                        ((SpreadSheetEntry_AnalyticalBatch)View.CurrentObject).Jobid = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("JobID"));
                    }
                    else
                    {
                        ((SpreadSheetEntry_AnalyticalBatch)View.CurrentObject).Instrument = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("Oid"));
                        ((SpreadSheetEntry_AnalyticalBatch)View.CurrentObject).strInstrument = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("InstrumentID"));
                    }
                }
                //((SpreadSheetEntry_AnalyticalBatch)View.CurrentObject).NPJobid = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("JobID"));
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
                NestedFrame nestedFrame = (NestedFrame)Frame;
                CompositeView view = nestedFrame.ViewItem.View;
                ActionContainerViewItem qcaction2 = ((DashboardView)view).FindItem("qcaction2") as ActionContainerViewItem;
                if (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "序号")
                {
                    //e.Cell.Attributes.Add("onclick", "event.stopPropagation();");
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
                if (parameter != string.Empty)
                {
                    if (bool.TryParse(parameter, out bool opensheet))
                    {
                        if (opensheet)
                        {
                            qcbatchinfo.canfilter = true;
                            //// Frame.SetView(Application.CreateDashboardView((NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet)), "SDMS", true));
                        }
                        else
                        {
                            if (CanOpenQCBatch)
                            {
                                Employee objEmployee = (Employee)SecuritySystem.CurrentUser;
                                if (objEmployee != null && objEmployee.Roles.Count > 0 && objEmployee.Roles.FirstOrDefault(i => i.IsAdministrative == true) == null && objEmployee.RolePermissions.Count > 0)
                                {
                                    bool IsDelete = false;
                                    bool IsCreate = false;
                                    bool IsWrite = false;

                                    foreach (RoleNavigationPermission obj in objEmployee.RolePermissions)
                                    {
                                        if (obj != null && obj.RoleNavigationPermissionDetails.Count > 0)
                                        {
                                            if (obj.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationId == "QCbatch" && i.Create == true) != null)
                                            {
                                                IsCreate = true;
                                            }
                                            if (obj.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationId == "QCbatch" && i.Write == true) != null)
                                            {
                                                IsWrite = true;
                                            }
                                            if (obj.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationId == "QCbatch" && i.Delete == true) != null)
                                            {
                                                IsDelete = true;
                                            }
                                            foreach (RoleNavigationPermissionDetails permission in obj.RoleNavigationPermissionDetails.Where(i => i.NavigationItem != null && i.NavigationItem.NavigationId == "QCbatch"))
                                            {
                                                if (!string.IsNullOrEmpty(permission.NavigationItem.NavigationModelClass))
                                                {
                                                    Frame.GetController<NavigationRefreshController>().AssignObjectPermission(objEmployee.Roles, IsCreate, IsWrite, IsDelete, permission.NavigationItem.NavigationModelClass, false);
                                                }
                                                if (permission.NavigationItem.LinkedClasses.Count > 0)
                                                {
                                                    foreach (LinkedClasses objLinkedClass in permission.NavigationItem.LinkedClasses)
                                                    {
                                                        if (!string.IsNullOrEmpty(objLinkedClass.ClassName))
                                                        {
                                                            Frame.GetController<NavigationRefreshController>().AssignObjectPermission(objEmployee.Roles, IsCreate, IsWrite, IsDelete, objLinkedClass.ClassName, true);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                Frame.SetView(Application.CreateListView(typeof(SpreadSheetEntry_AnalyticalBatch), true));
                            }
                            else
                            {
                                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(TestMethod));
                                CollectionSource cs = new CollectionSource(objectSpace, typeof(TestMethod));
                                Frame.SetView(Application.CreateListView("TestMethod_ListView_AnalysisQueue", cs, true));
                            }
                        }
                    }
                    else
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        DashboardViewItem qclist = ((DashboardView)view).FindItem("qclist") as DashboardViewItem;
                        DashboardViewItem qcdetail = ((DashboardView)view).FindItem("qcdetail") as DashboardViewItem;
                        SpreadSheetEntry_AnalyticalBatch objBatch = (SpreadSheetEntry_AnalyticalBatch)qcdetail.InnerView.CurrentObject;
                        if (qclist != null && qclist.InnerView != null)
                        {
                            ActionContainerViewItem qcaction2 = ((DashboardView)view).FindItem("qcaction2") as ActionContainerViewItem;
                            if (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "序号")
                            {
                                ASPxGridListEditor gridListEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    if (parameter == "Selectall")
                                    {
                                        string[] param1 = parameter.Split('|');
                                        List<SampleParameter> lstSamples = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.Oid", ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Select(i => i.SampleID).Distinct())).ToList();
                                        if (lstSamples.FirstOrDefault(j => j.Samplelogin != null && j.Testparameter != null && j.Testparameter.TestMethod != null && j.Testparameter.TestMethod.TestName == objBatch.Test.TestName && j.Testparameter.TestMethod.MethodName.MethodNumber == objBatch.Method.MethodName.MethodNumber && j.Testparameter.TestMethod.MatrixName.MatrixName == objBatch.Matrix.MatrixName && j.Testparameter.TestMethod.PrepMethods.Count > 0 && j.IsPrepMethodComplete == false) != null)
                                        {
                                            foreach (var qCseq in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>())
                                            {
                                                var matchingSampleParameter = lstSamples.FirstOrDefault(j => j.Samplelogin != null && j.Samplelogin.Oid == qCseq.SampleID.Oid && j.IsPrepMethodComplete == false && j.Testparameter.TestMethod.PrepMethods != null && j.Testparameter.TestMethod.PrepMethods.Count > 0);
                                                if (matchingSampleParameter != null)
                                                {
                                                    // Unselect each QCBatchSequence row in the grid
                                                    gridListEditor.Grid.Selection.UnselectRowByKey(qCseq.Oid);
                                                }
                                            }
                                            Application.ShowViewStrategy.ShowMessage("Sample preparation process is required.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        }
                                        //List<SampleParameter> lstSamples = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.Oid", ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Select(i => i.SampleID).Distinct())).ToList();
                                        //if (lstSamples.FirstOrDefault(j => j.Samplelogin != null && j.Testparameter != null && j.Testparameter.TestMethod.PrepMethods != null && j.Testparameter.TestMethod.PrepMethods.Count > 0 && j.IsPrepMethodComplete == false) != null)
                                        //{
                                        //    foreach (var qCseq in lstSamples)
                                        //    {
                                        //        //gridListEditor.Grid.Selection.UnselectRowByKey(qCseq.Samplelogin.Oid);
                                        //        gridListEditor.Grid.Selection.SelectAll();

                                        //        Application.ShowViewStrategy.ShowMessage("Sample preparation process is required.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        //    }
                                        //}


                                        if (gridListEditor.Grid.VisibleRowCount == gridListEditor.Grid.Selection.Count)
                                        {
                                            int maxsort = 1;
                                            foreach (QCBatchSequence sequences in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(i => i.StrSampleID).ToList())
                                            {
                                                sequences.Sort = maxsort;
                                                sequences.batchno = maxsort;
                                                maxsort++;
                                                if (qcbatchinfo.lststrseqdilutioncount != null && qcbatchinfo.lststrseqdilutioncount.Count > 0)
                                                {
                                                    foreach (string lststrqcseq in qcbatchinfo.lststrseqdilutioncount.ToList())
                                                    {
                                                        string[] strqcseq = lststrqcseq.Split('|');
                                                        if (strqcseq[0].Contains(sequences.Oid.ToString()))
                                                        {
                                                            if (strqcseq[1] != "null")
                                                            {
                                                                sequences.DilutionCount = Convert.ToUInt32(strqcseq[1].ToString());
                                                                break;
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (parameter == "UNSelectall")
                                    {
                                        foreach (QCBatchSequence sequences in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().ToList())
                                        {
                                            sequences.Sort = 0;
                                            sequences.batchno = 0;
                                            if (qcbatchinfo.lststrseqdilutioncount != null && qcbatchinfo.lststrseqdilutioncount.Count > 0)
                                            {
                                                foreach (string lststrqcseq in qcbatchinfo.lststrseqdilutioncount.ToList())
                                                {
                                                    string[] strqcseq = lststrqcseq.Split('|');
                                                    if (strqcseq[0].Contains(sequences.Oid.ToString()))
                                                    {
                                                        if (strqcseq[1] != "null")
                                                        {
                                                            sequences.DilutionCount = Convert.ToUInt32(strqcseq[1].ToString());
                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string[] splparm = parameter.Split('|');
                                        if (!string.IsNullOrEmpty(splparm[1]))
                                        {
                                            if (splparm[0] == "Selected")
                                            {
                                                QCBatchSequence objqCseq = qclist.InnerView.ObjectSpace.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                                if (objqCseq != null && objqCseq.SampleID != null)
                                                {
                                                    var matchingSampleParameter = qclist.InnerView.ObjectSpace.FirstOrDefault<SampleParameter>(j => j.Samplelogin != null && j.Samplelogin.Oid == objqCseq.SampleID.Oid && j.Testparameter != null && j.Testparameter.TestMethod.TestName == objBatch.Test.TestName && j.Testparameter.TestMethod.MethodName.MethodNumber == objBatch.Method.MethodName.MethodNumber && j.Testparameter.TestMethod.MatrixName.MatrixName == objBatch.Matrix.MatrixName && j.Testparameter.TestMethod != null && j.Testparameter.TestMethod.PrepMethods.Count > 0 && j.IsPrepMethodComplete == false);
                                                    if (matchingSampleParameter != null)
                                                    {
                                                        gridListEditor.Grid.Selection.UnselectRowByKey(objqCseq.Oid);
                                                        Application.ShowViewStrategy.ShowMessage("Sample preparation process is required.", InformationType.Error, timer.Seconds, InformationPosition.Top);

                                                        return;
                                                    }
                                                }
                                                int maxsort = 0;
                                                for (int i = 0; i <= gridListEditor.Grid.VisibleRowCount; i++)
                                                {
                                                    int cursort = Convert.ToInt32(gridListEditor.Grid.GetRowValues(i, "Sort"));
                                                    if (maxsort <= cursort)
                                                    {
                                                        maxsort = cursort + 1;
                                                    }
                                                }
                                                QCBatchSequence qCseq = qclist.InnerView.ObjectSpace.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                                if (qCseq != null && qCseq.Sort == 0)
                                                {
                                                    qCseq.Sort = maxsort;
                                                    qCseq.batchno = maxsort;
                                                    if (qcbatchinfo.lststrseqdilutioncount != null && qcbatchinfo.lststrseqdilutioncount.Count > 0)
                                                    {
                                                        foreach (string lststrqcseq in qcbatchinfo.lststrseqdilutioncount.ToList())
                                                        {
                                                            string[] strqcseq = lststrqcseq.Split('|');
                                                            if (strqcseq[0].Contains(splparm[1]))
                                                            {
                                                                if (strqcseq[1].ToString() != null)
                                                                {
                                                                    qCseq.DilutionCount = Convert.ToUInt32(strqcseq[1].ToString());
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (qcbatchinfo != null && qcbatchinfo.lststrseqstringSampleAmount != null)
                                                    {
                                                        foreach (string lststrSmpleAmt in qcbatchinfo.lststrseqstringSampleAmount.ToList())
                                                        {
                                                            string[] strqcseq = lststrSmpleAmt.Split('|');
                                                            if (strqcseq[0].Contains(splparm[1]))
                                                            {
                                                                qCseq.SampleAmount = strqcseq[1].ToString();
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (qcbatchinfo != null && qcbatchinfo.lststrseqstringFinalVolume != null)
                                                    {
                                                        foreach (string lststrFinalVol in qcbatchinfo.lststrseqstringFinalVolume.ToList())
                                                        {
                                                            string[] strqcseq = lststrFinalVol.Split('|');
                                                            if (strqcseq[0].Contains(splparm[1]))
                                                            {
                                                                qCseq.FinalVolume = strqcseq[1].ToString();
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (maxsort == 0)
                                                {
                                                    int j = 0;
                                                }
                                            }
                                            else if (splparm[0] == "UNSelected")
                                            {
                                                QCBatchSequence qCseq = qclist.InnerView.ObjectSpace.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                                if (qCseq != null)
                                                {
                                                    foreach (QCBatchSequence sequences in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.Sort > qCseq.Sort).OrderBy(a => a.SampleID.SampleID).ToList())
                                                    {
                                                        sequences.Sort -= 1;
                                                        sequences.batchno -= 1;
                                                    }

                                                    qCseq.Sort = 0;
                                                    qCseq.batchno = 0;
                                                }
                                                if (qcbatchinfo.lststrseqdilutioncount != null && qcbatchinfo.lststrseqdilutioncount.Count > 0)
                                                {
                                                    foreach (string lststrqcseq in qcbatchinfo.lststrseqdilutioncount.ToList())
                                                    {
                                                        string[] strqcseq = lststrqcseq.Split('|');
                                                        if (strqcseq[0].Contains(splparm[1]))
                                                        {
                                                            qCseq.DilutionCount = Convert.ToUInt32(strqcseq[1].ToString());
                                                            break;
                                                        }
                                                    }
                                                }
                                                if (qcbatchinfo != null && qcbatchinfo.lststrseqstringSampleAmount != null)
                                                {
                                                    foreach (string lststrSmpleAmt in qcbatchinfo.lststrseqstringSampleAmount.ToList())
                                                    {
                                                        string[] strqcseq = lststrSmpleAmt.Split('|');
                                                        if (strqcseq[0].Contains(splparm[1]))
                                                        {
                                                            qCseq.SampleAmount = strqcseq[1].ToString();
                                                            break;
                                                        }
                                                    }
                                                }
                                                if (qcbatchinfo != null && qcbatchinfo.lststrseqstringFinalVolume != null)
                                                {
                                                    foreach (string lststrFinalVol in qcbatchinfo.lststrseqstringFinalVolume.ToList())
                                                    {
                                                        string[] strqcseq = lststrFinalVol.Split('|');
                                                        if (strqcseq[0].Contains(splparm[1]))
                                                        {
                                                            qCseq.FinalVolume = strqcseq[1].ToString();
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            //else if (splparm[0] == "SelectedRow")
                                            //{
                                            //    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                                            //    if (editor != null && editor.Grid != null)
                                            //    {
                                            //        int index = int.Parse(splparm[1]);
                                            //        editor.Grid.Selection.SelectRow(index);
                                            //        object currentOid = editor.Grid.GetRowValues(index, "Oid");
                                            //    }
                                            //} 
                                        }
                                    }
                                }
                                ((ListView)qclist.InnerView).Refresh();
                            }
                        }
                    }
                }
                string[] param = parameter.Split('|');
                if (param[0] == "DilutionCnt")
                {
                    if (qcbatchinfo.lststrseqdilutioncount == null)
                    {
                        qcbatchinfo.lststrseqdilutioncount = new List<string>();
                    }
                    if (qcbatchinfo.lststrseqdilutioncount.Count == 0)
                    {
                        qcbatchinfo.lststrseqdilutioncount.Add(param[1].ToString() + "|" + param[2].ToString());
                    }
                    else
                    {
                        foreach (string objstrdcnt in qcbatchinfo.lststrseqdilutioncount)
                        {
                            string[] strcnt = objstrdcnt.Split('|');
                            if (strcnt[0].Contains(param[1]))
                            {
                                qcbatchinfo.lststrseqdilutioncount.Remove(objstrdcnt);
                                qcbatchinfo.lststrseqdilutioncount.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                            else
                            {
                                qcbatchinfo.lststrseqdilutioncount.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                        }
                    }

                }
                else if (param[0] == "StringDilution")
                {
                    if (qcbatchinfo.lststrseqstringdilution == null)
                    {
                        qcbatchinfo.lststrseqstringdilution = new List<string>();
                    }
                    if (qcbatchinfo.lststrseqstringdilution.Count == 0)
                    {
                        qcbatchinfo.lststrseqstringdilution.Add(param[1].ToString() + "|" + param[2].ToString());
                    }
                    else
                    {
                        foreach (string objstrdcnt in qcbatchinfo.lststrseqstringdilution)
                        {
                            string[] strcnt = objstrdcnt.Split('|');
                            if (strcnt[0].Contains(param[1]))
                            {
                                qcbatchinfo.lststrseqstringdilution.Remove(objstrdcnt);
                                qcbatchinfo.lststrseqstringdilution.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                            else
                            {
                                qcbatchinfo.lststrseqstringdilution.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                        }
                    }
                }
                else if (param[0] == "StringSampleAmount")
                {
                    if (qcbatchinfo.lststrseqstringSampleAmount == null)
                    {
                        qcbatchinfo.lststrseqstringSampleAmount = new List<string>();
                    }
                    if (qcbatchinfo.lststrseqstringSampleAmount.Count == 0)
                    {
                        qcbatchinfo.lststrseqstringSampleAmount.Add(param[1].ToString() + "|" + param[2].ToString());
                    }
                    else
                    {
                        foreach (string objstrdcnt in qcbatchinfo.lststrseqstringSampleAmount)
                        {
                            string[] strcnt = objstrdcnt.Split('|');
                            if (strcnt[0].Contains(param[1]))
                            {
                                qcbatchinfo.lststrseqstringSampleAmount.Remove(objstrdcnt);
                                qcbatchinfo.lststrseqstringSampleAmount.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                            else
                            {
                                qcbatchinfo.lststrseqstringSampleAmount.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                        }
                    }
                }
                else if (param[0] == "StringFinalVolume")
                {
                    if (qcbatchinfo.lststrseqstringFinalVolume == null)
                    {
                        qcbatchinfo.lststrseqstringFinalVolume = new List<string>();
                    }
                    if (qcbatchinfo.lststrseqstringFinalVolume.Count == 0)
                    {
                        qcbatchinfo.lststrseqstringFinalVolume.Add(param[1].ToString() + "|" + param[2].ToString());
                    }
                    else
                    {
                        foreach (string objstrdcnt in qcbatchinfo.lststrseqstringFinalVolume)
                        {
                            string[] strcnt = objstrdcnt.Split('|');
                            if (strcnt[0].Contains(param[1]))
                            {
                                qcbatchinfo.lststrseqstringFinalVolume.Remove(objstrdcnt);
                                qcbatchinfo.lststrseqstringFinalVolume.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                            else
                            {
                                qcbatchinfo.lststrseqstringFinalVolume.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                        }
                    }
                }
                else if (param[0] == "Rowselect")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid.Columns["Sort"].Visible == false)
                    {
                        object currentOid = gridListEditor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                        Guid currentguid = new Guid(currentOid.ToString());
                        //QCBatchSequence curSampleAutomation = View.ObjectSpace.GetObjectByKey<QCBatchSequence>(currentOid);
                        for (int i = 0; i <= gridListEditor.Grid.VisibleRowCount; i++)
                        {
                            //int cursort1 = Convert.ToInt32(gridListEditor.Grid.GetRowValues(i, "Sort"));
                            object currentOid1 = gridListEditor.Grid.GetRowValues(i, "Oid");
                            if (currentOid1 != null)
                            {
                                Guid selectguid = new Guid(currentOid1.ToString());
                                if (currentguid == selectguid)
                                {
                                    gridListEditor.Grid.Selection.SelectRow(i);
                                }
                            }
                        }
                    }
                }
                if (param[0] == "lyCnt")
                {
                    if (qcbatchinfo.lststrseqlayercount == null)
                    {
                        qcbatchinfo.lststrseqlayercount = new List<string>();
                    }
                    if (qcbatchinfo.lststrseqlayercount.Count == 0)
                    {
                        qcbatchinfo.lststrseqlayercount.Add(param[1].ToString() + "|" + param[2].ToString());
                    }
                    else
                    {
                        foreach (string objstrdcnt in qcbatchinfo.lststrseqlayercount)
                        {
                            string[] strcnt = objstrdcnt.Split('|');
                            if (strcnt[0].Contains(param[1]))
                            {
                                qcbatchinfo.lststrseqlayercount.Remove(objstrdcnt);
                                qcbatchinfo.lststrseqlayercount.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
                            }
                            else
                            {
                                qcbatchinfo.lststrseqlayercount.Add(param[1].ToString() + "|" + param[2].ToString());
                                break;
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
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "QCType_ListView_qcbatchsequence")
                {
                    GridViewColumn QCadd = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "QCadd").ToList()[0];
                    if (QCadd != null)
                    {
                        QCadd.Width = 40;
                        if (qcbatchinfo.canfilter)
                        {
                            QCadd.Visible = true;
                        }
                        if (qcbatchinfo.isview)
                        {
                            QCadd.Visible = false;
                        }
                        if (IsQCview.IsViewMode)
                        {
                            QCadd.Visible = false;
                        }
                    }
                }
                else
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    ActionContainerViewItem qcaction0 = ((DashboardView)view).FindItem("qcaction0") as ActionContainerViewItem;
                    ActionContainerViewItem qcaction2 = ((DashboardView)view).FindItem("qcaction2") as ActionContainerViewItem;
                    string strwidth = System.Web.HttpContext.Current.Request.Cookies.Get("width").Value;
                    //string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    //if (View.Id != "QCBatchSequence_ListView" || (View.Id == "QCBatchSequence_ListView" && Convert.ToInt32(strscreenwidth) < 1500))
                    //{
                    //    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    //}
                    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    gridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridView.Settings.VerticalScrollableHeight = 300;
                    gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    //if (gridView.VisibleRowCount > 10)
                    //{  
                    //    gridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    //    gridView.Settings.VerticalScrollableHeight = 250;
                    //}
                    //else
                    //{
                    //    gridView.Settings.VerticalScrollableHeight = 302;
                    //}
                    foreach (GridViewColumn column in gridView.Columns)
                    {
                        if (column.Name == "InlineEditCommandColumn")
                        {
                            column.Visible = false;
                        }
                        else if (column.Name == "SelectionCommandColumn")
                        {
                            if (qcbatchinfo.canfilter)
                            {
                                if (qcbatchinfo.strqcid != null)
                                {
                                    column.Visible = false;
                                    column.FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                else
                                {
                                    column.Visible = true;
                                    column.FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                            }
                            else
                            {
                                column.FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (column.Name == "QCremove")
                        {
                            column.Width = 40;
                            if (qcbatchinfo.canfilter)
                            {
                                column.Visible = true;
                                column.FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            else
                            {
                                column.FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (qcbatchinfo.isview)
                            {
                                column.Visible = false;
                            }
                            if (IsQCview.IsViewMode)
                            {
                                column.Visible = false;
                            }
                        }
                    }
                    if (qcbatchinfo.IsPLMTest)
                    {
                        if (gridView.Columns["StrSampleL"] != null)
                        {
                            gridView.Columns["StrSampleL"].Visible = true;
                            gridView.Columns["StrSampleL"].Width = 170;
                        }
                        if (gridView.Columns["LayerCount"] != null)
                        {
                            gridView.Columns["LayerCount"].Visible = true;
                        }
                        if (gridView.Columns["Dilution"] != null)
                        {
                            gridView.Columns["Dilution"].Visible = false;
                        }
                        if (gridView.Columns["DilutionCount"] != null)
                        {
                            gridView.Columns["DilutionCount"].Visible = false;
                        }
                        if (gridView.Columns["SampleAmount"] != null)
                        {
                            gridView.Columns["SampleAmount"].Visible = false;
                        }
                        if (gridView.Columns["FinalVol(ml)"] != null)
                        {
                            gridView.Columns["FinalVol(ml)"].Visible = false;
                        }
                    }
                    else
                    {
                        if (gridView.Columns["StrSampleL"] != null)
                        {
                            gridView.Columns["StrSampleL"].Visible = false;
                        }
                        if (gridView.Columns["LayerCount"] != null)
                        {
                            gridView.Columns["LayerCount"].Visible = false;
                        }
                        if (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "序号")
                        {
                            if (gridView.Columns["DilutionCount"] != null)
                            {
                                gridView.Columns["DilutionCount"].Visible = true;
                                gridView.Columns["DilutionCount"].Width = 100;
                                gridView.Columns["DilutionCount"].SetColVisibleIndex(4);
                            }
                            if (gridView.Columns["SampleAmount"] != null)
                            {
                                gridView.Columns["SampleAmount"].Visible = true;
                                gridView.Columns["SampleAmount"].Width = 120;
                                gridView.Columns["SampleAmount"].SetColVisibleIndex(5);
                            }
                            if (gridView.Columns["FinalVol(ml)"] != null)
                            {
                                gridView.Columns["FinalVol(ml)"].Visible = true;
                                gridView.Columns["FinalVol(ml)"].Width = 120;
                                gridView.Columns["FinalVol(ml)"].SetColVisibleIndex(6);
                            }
                        }
                        else
                        {
                            if (gridView.Columns["DilutionCount"] != null)
                            {
                                gridView.Columns["DilutionCount"].Visible = false;
                            }
                        }

                    }

                    if (qcbatchinfo.canfilter)
                    {
                        qcbatchinfo.canfilter = false;
                        if (qcbatchinfo.strqcid != null)
                        {
                            gridView.ClearSort();
                            gridView.SortBy(gridView.Columns["Sort"], ColumnSortOrder.Ascending);
                            qcaction0.Actions[0].Enabled.SetItemValue("key", true);
                            qcaction0.Actions[1].Enabled.SetItemValue("key", true);
                            qcaction2.Actions[0].Enabled.SetItemValue("key", true);
                            qcaction2.Actions[1].Enabled.SetItemValue("key", true);
                            gridView.Columns["Sort"].Visible = false;
                            gridView.Columns["DilutionCount"].Visible = false;
                            if (!qcbatchinfo.IsPLMTest)
                            {
                                gridView.Columns["Dilution"].Visible = true;
                                gridView.Columns["Dilution"].Width = 70;
                                gridView.Columns["Dilution"].SetColVisibleIndex(5);
                            }
                            if (CurrentLanguage == "En")
                            {
                                qcaction2.Actions[1].Caption = "Ok";
                            }
                            else
                            {
                                qcaction2.Actions[1].Caption = "确定";
                            }
                        }
                        else
                        {
                            gridView.ClearSort();
                            qcaction0.Actions[0].Enabled.SetItemValue("key", true);
                            qcaction0.Actions[1].Enabled.SetItemValue("key", true);
                            qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                            qcaction2.Actions[1].Enabled.SetItemValue("key", true);
                            gridView.Columns["Sort"].Visible = true;
                            gridView.Columns["Sort"].Width = 50;
                            if (qcbatchinfo.IsPLMTest)
                            {
                                gridView.Columns["DilutionCount"].Visible = false;
                            }
                            else
                            {
                                gridView.Columns["DilutionCount"].Visible = true;
                                //gridView.Columns["DilutionCount"].Width = 100;
                                gridView.Columns["DilutionCount"].SetColVisibleIndex(4);
                            }
                            gridView.Columns["Dilution"].Visible = false;
                            gridView.Columns["Sort"].VisibleIndex = 4;
                            gridView.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        //gridView.ClearSort();
                        //gridView.SortBy(gridView.Columns["Sort"], ColumnSortOrder.Ascending);
                        //qcaction0.Actions[0].Enabled.SetItemValue("key", true);
                        //qcaction0.Actions[1].Enabled.SetItemValue("key", true);
                        //qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                        //qcaction2.Actions[1].Enabled.SetItemValue("key", true);
                        //gridView.Columns["Sort"].Visible = true;
                        //gridView.Columns["Sort"].VisibleIndex = 4;
                        //gridView.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    else
                    {
                        if (gridView.Columns["Sort"] != null)
                        {
                            //gridView.Columns["Sort"].Width = 50;
                            gridView.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                    }
                    if (qcbatchinfo.isview)
                    {
                        gridView.ClearSort();
                        qcaction0.Actions[0].Enabled.SetItemValue("key", false);
                        qcaction0.Actions[1].Enabled.SetItemValue("key", false);
                        qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                        qcaction2.Actions[1].Enabled.SetItemValue("key", false);
                    }
                    //gridView.Columns["SampleID"].Width = 130;
                    //gridView.Columns["SysSampleCode"].Width = 200;
                    if (gridView.Columns["QCType.QCTypeName"] != null)
                    {
                        //gridView.Columns["QCType.QCTypeName"].Width = 100;
                        gridView.Columns["QCType.QCTypeName"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.Columns["SysSampleCode"] != null)
                    {
                        gridView.Columns["SysSampleCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                        gridView.Columns["SysSampleCode"].Width = 150;
                    }
                    //if (gridView.Columns["SampleName"] != null)
                    //{
                    //    gridView.Columns["SampleName"].Width = 100;
                    //}
                    if (gridView.Columns["ProjectID"] != null)
                    {
                        gridView.Columns["ProjectID"].Width = 250;
                    }
                    //if (gridView.Columns["DateCollected"] != null)
                    //{
                    //    gridView.Columns["DateCollected"].Width = 100;
                    //}
                    if (gridView.Columns["DateReceived"] != null)
                    {
                        gridView.Columns["DateReceived"].Width = 150;
                    }
                    if (gridView.Columns["Status"] != null)
                    {
                        gridView.Columns["Status"].Width = 200;
                    }
                    if (gridView.Columns["SampleID"] != null)
                    {
                        //gridView.Columns["SampleID"].Width = 130;
                        gridView.Columns["SampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.Columns["JobID"] != null)
                    {
                        gridView.Columns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    //if (gridView.Columns["SampleID.JobID.ClientName.CustomerName"] != null)
                    //{
                    //    gridView.Columns["SampleID.JobID.ClientName.CustomerName"].Width = 200;
                    //}

                    if (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "序号")
                    {
                        gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                    }
                    else
                    {
                        List<string> lststr = new List<string>();
                        List<string> strdilution = new List<string>();
                        List<string> strdilutionno = new List<string>();
                        DashboardViewItem qclist = ((DashboardView)Application.MainWindow.View).FindItem("qclist") as DashboardViewItem;
                        if (qclist != null && qclist.InnerView != null)
                        {
                            for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                            {
                                if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "Oid").ToString()))
                                {
                                    string qcseqoid = gridView.GetRowValues(i, "Oid").ToString();
                                    string dcount = gridView.GetRowValues(i, "DilutionCount").ToString();
                                    QCBatchSequence sequence1 = qclist.InnerView.ObjectSpace.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[Oid] = ?", new Guid(qcseqoid)));
                                    if (sequence1 != null)
                                    {
                                        strdilution.Add(sequence1.Dilution);
                                        strdilutionno.Add(sequence1.DilutionCount.ToString());
                                    }
                                    if (!string.IsNullOrEmpty(qcseqoid) && !string.IsNullOrEmpty(dcount))
                                    {
                                        string lststrqcseq = qcseqoid + "| " + dcount;
                                        lststr.Add(lststrqcseq);
                                    }
                                }
                            }
                        }
                    }
                    if (IsQCview.IsViewMode == true)
                    {
                        //disenbcontrols(false, false, qcdetail.InnerView);
                        if (qcaction0 != null && qcaction2 != null && qcaction0.Actions.Count > 0 && qcaction2.Actions.Count > 0)
                        {
                            qcaction0.Actions[0].Enabled.SetItemValue("key", false);
                            qcaction0.Actions[1].Enabled.SetItemValue("key", false);
                            qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                            qcaction2.Actions[1].Enabled.SetItemValue("key", false);
                        }
                    }
                }
                ////if(View.Id == "QCBatchSequence_ListView" && qcbatchinfo.IsSortActionEnable == true)
                ////{
                ////    foreach(QCBatchSequence qcbatchseq in ((ListView)View).CollectionSource.List)
                ////    {
                ////        qcbatchseq.Dilution = "1";
                ////    }
                ////}
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

                Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;

                //if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                //{
                //    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                //}
                if (View.Id == "QCBatch_ListView" || View.Id == "QCBatch_ListView_Copy" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                {

                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;

                    qcbatchDateFilterActions.SelectedItemChanged -= QcbatchDateFilterAction_SelectedItemChanged;
                    if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                    {
                        ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                        tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;

                    }
                }
                if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_History")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                }
                ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                if (qcbatchinfo.lststrseqdilutioncount != null)
                {
                    qcbatchinfo.lststrseqdilutioncount.Clear();
                }
                DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
                if (View.Id == "QCBatch_ListView" || View.Id == "QCBatch_ListView_Copy")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;

                    if (View.Id == "QCBatch_ListView_Copy")
                    {
                        ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                        tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                    }
                    qcbatchinfo.strqcbatchid = null;
                }
                else if (View.Id == "QCBatchsequence")
                {
                    IsQCview.IsViewMode = false;
                    qcbatchinfo.IsSortActionEnable = false;
                    qcbatchinfo.IsResetActionEnable = false;
                    if (WebWindow.CurrentRequestWindow != null)
                    {
                        WebWindow.CurrentRequestWindow.RegisterClientScript("alrt", "document.getElementById('separatorButton').setAttribute('onclick', 'NavSplit();')");
                    }
                    Frame.GetController<RefreshController>().RefreshAction.Executing -= RefreshAction_Executing;
                    if (OpenResultEntry.Active.Contains("ShowOpenResultEntry"))
                    {
                        OpenResultEntry.Active.RemoveItem("ShowOpenResultEntry");
                    }
                    CNInfo.SCoidValue = Guid.Empty;
                    CNInfo.QCJobId = null;
                    CNInfo.QCSampleMatries = null;
                }
                else if (View.Id == "QCBatchSequence_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                else if (View.Id == "QCType_ListView_qcbatchsequence")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                else if (View.Id == "TestMethod_ListView_AnalysisQueue")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                }
                else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_Reagents_ListView" || View.Id == "SpreadSheetEntry_AnalyticalBatch_Instruments_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QCreset_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem qcdetail = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                SpreadSheetEntry_AnalyticalBatch batch = (SpreadSheetEntry_AnalyticalBatch)qcdetail.InnerView.CurrentObject;
                if (qcdetail.InnerView.ObjectSpace.IsNewObject(batch))
                {
                    qcbatchinfo.IsResetActionEnable = true;
                    batch.AnalyticalBatchID = string.Empty;
                    batch.Humidity = null;
                    batch.Instrument = null;
                    ASPxCheckedLookupStringPropertyEditor InstrumentpropertyEditor = ((DetailView)qcdetail.InnerView).FindItem("Instrument") as ASPxCheckedLookupStringPropertyEditor;
                    if (InstrumentpropertyEditor != null && InstrumentpropertyEditor.Editor != null)
                    {
                        ASPxGridLookup spinEdit = (ASPxGridLookup)InstrumentpropertyEditor.Editor;
                        spinEdit.GridView.Selection.UnselectAll();
                        InstrumentpropertyEditor.Refresh();
                    }
                    batch.Jobid = null;
                    batch.Matrix = null;
                    batch.Method = null;
                    batch.Noruns = 1;
                    batch.Roomtemp = null;
                    batch.Test = null;
                    batch.ISShown = true;
                    disenbcontrols(true, true, qcdetail.InnerView);
                    ASPxCheckedLookupStringPropertyEditor TestpropertyEditor = ((DetailView)qcdetail.InnerView).FindItem("Test") as ASPxCheckedLookupStringPropertyEditor;
                    if (TestpropertyEditor != null && TestpropertyEditor.Editor != null)
                    {
                        ASPxGridLookup spinEdit = (ASPxGridLookup)TestpropertyEditor.Editor;
                        spinEdit.GridView.Selection.UnselectAll();
                        TestpropertyEditor.Refresh();
                    }
                    ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                    qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                    if (CurrentLanguage == "En")
                    {
                        qcaction2.Actions[1].Caption = "Sort";
                    }
                    else
                    {
                        qcaction2.Actions[1].Caption = "序号";
                    }
                    DashboardViewItem qclist = ((DashboardView)View).FindItem("qclist") as DashboardViewItem;
                    if (qclist != null && qclist.InnerView != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.ClearSort();
                            gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["StrSampleID"], ColumnSortOrder.Ascending);
                            foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                            {
                                if (column.Name == "SelectionCommandColumn")
                                {
                                    column.Visible = true;
                                }
                                else if (column.Name == "QCremove")
                                {
                                    column.Visible = false;
                                }
                            }
                            gridListEditor.Grid.Columns["Sort"].Visible = true;
                            gridListEditor.Grid.Columns["SampleName"].Width = 150;
                            gridListEditor.Grid.Columns["SYSSamplecode"].Width = 200;
                            gridListEditor.Grid.Columns["DilutionCount"].Visible = true;
                            gridListEditor.Grid.Columns["DilutionCount"].Width = 100;
                            gridListEditor.Grid.Columns["DilutionCount"].SetColVisibleIndex(4);
                            gridListEditor.Grid.Columns["Dilution"].Visible = false;
                            gridListEditor.Grid.Columns["Dilution"].SetColVisibleIndex(5);
                            gridListEditor.Grid.Columns["Sort"].Width = 50;
                            gridListEditor.Grid.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().ToList())
                        {
                            qclist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequence);
                            ((ListView)qclist.InnerView).CollectionSource.Remove(sequence);
                        }
                        ((ListView)qclist.InnerView).Refresh();
                    }
                    DashboardViewItem qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                    if (qctype != null && qctype.InnerView != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.ClearSort();
                            foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                            {
                                if (column.Name == "QCadd")
                                {
                                    column.Visible = false;
                                }
                            }
                        }
                        ((ListView)qctype.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                        ((ListView)qctype.InnerView).Refresh();
                    }
                }
                else
                {
                    qcdetail.InnerView.ObjectSpace.Refresh();
                    ((SpreadSheetEntry_AnalyticalBatch)qcdetail.InnerView.CurrentObject).ISShown = false;
                    qcdetail.InnerView.Refresh();
                    disenbcontrols(false, false, qcdetail.InnerView);
                    ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                    qcaction2.Actions[0].Enabled.SetItemValue("key", true);
                    if (CurrentLanguage == "En")
                    {
                        qcaction2.Actions[1].Caption = "Ok";
                    }
                    else
                    {
                        qcaction2.Actions[1].Caption = "序号";
                    }
                    DashboardViewItem qclist = ((DashboardView)View).FindItem("qclist") as DashboardViewItem;
                    if (qclist != null && qclist.InnerView != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.ClearSort();
                            gridListEditor.Grid.Selection.UnselectAll();
                            gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Sort"], ColumnSortOrder.Ascending);
                            foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                            {
                                if (column.Name == "SelectionCommandColumn")
                                {
                                    //column.Visible = false;
                                }
                                else if (column.Name == "QCremove")
                                {
                                    column.Visible = true;
                                }
                            }
                            gridListEditor.Grid.Columns["Sort"].Visible = false;
                            gridListEditor.Grid.Columns["SampleName"].Width = 150;
                            gridListEditor.Grid.Columns["SYSSamplecode"].Width = 200;
                            gridListEditor.Grid.Columns["DilutionCount"].Visible = false;
                            gridListEditor.Grid.Columns["Dilution"].Visible = true;
                            gridListEditor.Grid.Columns["Dilution"].Width = 70;
                            gridListEditor.Grid.Columns["Dilution"].SetColVisibleIndex(5);
                            gridListEditor.Grid.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        qclist.InnerView.ObjectSpace.Refresh();
                    }
                    DashboardViewItem qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                    if (qctype != null && qctype.InnerView != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.ClearSort();
                            foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                            {
                                if (column.Name == "QCadd")
                                {
                                    column.Visible = true;
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

        private void qcload_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                DashboardViewItem qclist = ((DashboardView)View).FindItem("qclist") as DashboardViewItem;
                DashboardViewItem qcdetail = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                SpreadSheetEntry_AnalyticalBatch curqC = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;

                if (curqC.Jobid != null && !string.IsNullOrEmpty(curqC.Jobid) && curqC.Matrix != null && curqC.Test != null && curqC.Method != null /*&& !string.IsNullOrEmpty(curqC.Instrument)*/)
                {
                    Comment.Enabled.SetItemValue("HideComment", true);
                    if (qclist != null && qclist.InnerView != null && qctype != null && qctype.InnerView != null && qcdetail != null && qcdetail.InnerView != null && qcaction2 != null)
                    {
                        if (((ListView)qclist.InnerView).CollectionSource.GetCount() > 0 && (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "序号"))
                        {
                            foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().ToList())
                            {
                                if (qclist.InnerView.ObjectSpace.IsNewObject(sequence))
                                {
                                    qclist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequence);
                                    ((ListView)qclist.InnerView).CollectionSource.Remove(sequence);
                                }
                            }
                            ((ListView)qclist.InnerView).Refresh();
                        }
                        else if (((ListView)qclist.InnerView).CollectionSource.GetCount() > 0 && (qcaction2.Actions[1].Caption == "Ok" || qcaction2.Actions[1].Caption == "确定"))
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "samplesorterror"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        SpreadSheetEntry_AnalyticalBatch batch = (SpreadSheetEntry_AnalyticalBatch)qcdetail.InnerView.CurrentObject;
                        if (batch != null && !string.IsNullOrEmpty(batch.Jobid))
                        {
                            List<string> lstqctype = new List<string>();
                            foreach (Testparameter TP in batch.Test.TestParameter)
                            {
                                if (TP.QCType != null && !lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample" && batch.Test.QCTypes.Contains(TP.QCType))
                                {
                                    lstqctype.Add(TP.QCType.QCTypeName);
                                }
                            }
                            ((ListView)qctype.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[QCTypeName] In(" + string.Format("'{0}'", string.Join("','", lstqctype)) + ")");
                            QCType sampleqCType = qclist.InnerView.ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                            string[] ids = batch.Jobid.Split(';');
                            foreach (string id in ids)
                            {
                                Samplecheckin objsamplecheckin = qclist.InnerView.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", id));
                                if (objsamplecheckin != null)
                                {
                                    CNInfo.QCJobId = objsamplecheckin.JobID;
                                    CNInfo.SCoidValue = objsamplecheckin.Oid;
                                    if (!string.IsNullOrEmpty(objsamplecheckin.SampleMatries))
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        foreach (string strMatrix in objsamplecheckin.SampleMatries.Split(';'))
                                        {
                                            VisualMatrix objSM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(strMatrix));
                                            if (sb.Length > 0)
                                            {
                                                sb.Append(";"); // Add semicolon before appending the next name
                                            }
                                            sb.Append(objSM.VisualMatrixName);
                                        }
                                        CNInfo.QCSampleMatries = sb.ToString();
                                    }
                                    IList<SampleParameter> objsp = null;
                                    DefaultSetting objDefault = this.ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparation'AND [Select]=True"));
                                    DefaultSetting objDefaultmodule = this.ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID]='SamplePreparationRootNode'AND [Select]=True"));
                                    if (objDefault != null && objDefaultmodule != null)
                                    {
                                        objsp = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.JobID] = ? and [TestHold] =false and [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [SignOff] = True And [IsTransferred] = true And [QCBatchID] Is Null And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 /*And [IsPrepMethodComplete]  = True*/) OR [Testparameter.TestMethod.PrepMethods][].Count() == 0 And ([IsPrepMethodComplete]  = False Or [IsPrepMethodComplete] Is Null) And ([SubOut] Is Null Or [SubOut] = False) )", objsamplecheckin.JobID, batch.Matrix.MatrixName, batch.Test.TestName, batch.Method.MethodName.MethodNumber));
                                    }
                                    else
                                    {
                                        objsp = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.JobID] = ? and [TestHold] =false and [Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [SignOff] = True  And [IsTransferred] = true And [QCBatchID] Is Null And ([SubOut] Is Null Or [SubOut] = False)", objsamplecheckin.JobID, batch.Matrix.MatrixName, batch.Test.TestName, batch.Method.MethodName.MethodNumber));
                                    }
                                    //IList<SampleParameter> objsp = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [QCBatchID] Is Null", objsamplecheckin.Oid, batch.Test.Oid));
                                    IList<SampleLogIn> objdistsl = new List<SampleLogIn>();
                                    foreach (SampleParameter sample in objsp)
                                    {
                                        if (!objdistsl.Contains(sample.Samplelogin))
                                        {
                                            objdistsl.Add(sample.Samplelogin);
                                        }
                                    }
                                    foreach (SampleLogIn sampleLog in objdistsl.OrderBy(a => int.Parse(a.SampleID.Split('-')[1])).ToList())
                                    {
                                        for (int i = 1; i <= batch.Noruns; i++)
                                        {
                                            QCBatchSequence qCBatch = qclist.InnerView.ObjectSpace.CreateObject<QCBatchSequence>();
                                            qCBatch.QCType = sampleqCType;
                                            qCBatch.SampleID = sampleLog;
                                            qCBatch.StrSampleID = sampleLog.SampleID;
                                            qCBatch.SYSSamplecode = sampleLog.SampleID;
                                            qCBatch.Runno = i;
                                            qCBatch.LayerCount = 1;
                                            IList<SamplePrepBatch> objlist = ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("[Jobid]=?", sampleLog.JobID.JobID));
                                            bool tier2Found = false;
                                            foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 2))
                                            {
                                                SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SampleID.Oid] = ? AND [QCType] Is Not Null AND [QCType.QCTypeName] = 'Sample' And [SamplePrepBatchDetail.Oid] = ?", sampleLog.Oid, obj.Oid));

                                                if (prepSequence != null)
                                                {
                                                    qCBatch.SampleAmount = prepSequence.SampleAmount;
                                                    qCBatch.FinalVolume = prepSequence.FinalVolume;
                                                    tier2Found = true;
                                                    break;
                                                }
                                            }
                                            if (!tier2Found)
                                            {
                                                foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 1))
                                                {
                                                    SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SampleID.Oid] = ? AND [QCType] Is Not Null AND [QCType.QCTypeName] = 'Sample' And [SamplePrepBatchDetail.Oid] = ?", sampleLog.Oid, obj.Oid));
                                                    if (prepSequence != null)
                                                    {
                                                        qCBatch.SampleAmount = prepSequence.SampleAmount;
                                                        qCBatch.FinalVolume = prepSequence.FinalVolume;
                                                        tier2Found = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (qcbatchinfo.IsPLMTest)
                                            {
                                                qCBatch.StrSampleL = sampleLog.SampleID + ".A";
                                            }
                                            ((ListView)qclist.InnerView).CollectionSource.Add(qCBatch);
                                        }
                                    }
                                }
                            }
                            List<SampleParameter> lstSamples = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(new InOperator("Samplelogin.Oid", ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Select(i => i.SampleID).Distinct())).ToList();
                            if (lstSamples.FirstOrDefault(j => j.Samplelogin != null && j.Testparameter != null && j.Testparameter.TestMethod != null && j.Testparameter.TestMethod.TestName == curqC.Test.TestName && j.Testparameter.TestMethod.MethodName.MethodNumber == curqC.Method.MethodName.MethodNumber && j.Testparameter.TestMethod.MatrixName.MatrixName == curqC.Matrix.MatrixName && j.Testparameter.TestMethod.PrepMethods.Count > 0 && j.IsPrepMethodComplete == false) != null)
                            {
                                foreach (var qCseq in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>())
                                {
                                    var matchingSampleParameter = lstSamples.FirstOrDefault(j => j.Samplelogin != null && j.Samplelogin.Oid == qCseq.SampleID.Oid && j.IsPrepMethodComplete == false && j.Testparameter.TestMethod.PrepMethods != null && j.Testparameter.TestMethod.PrepMethods.Count > 0);
                                    if (matchingSampleParameter != null)
                                    {
                                        // Unselect each QCBatchSequence row in the grid
                                        qCseq.Status = PLMDataEnterStatus.PendingSamplePrep;
                                    }
                                }
                            }
                            ((ListView)qclist.InnerView).Refresh();
                            ASPxGridListEditor gridListEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                            ASPxGridView gridView = gridListEditor.Grid;
                            if (gridListEditor != null && gridListEditor.Grid != null)
                            {
                                gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                                gridListEditor.Grid.Settings.VerticalScrollableHeight = 300;
                                //gridListEditor.Grid.Columns["SampleName"].Width = 150;
                                //gridListEditor.Grid.Columns["SYSSamplecode"].Width = 150;
                                //gridListEditor.Grid.Columns["Status"].Width = System.Web.UI.WebControls.Unit.Percentage(100);
                                gridListEditor.Grid.ClearSort();
                                if (qcbatchinfo.strqcid == null)
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["StrSampleID"], ColumnSortOrder.Ascending);
                                }
                                else
                                {
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Sort"], ColumnSortOrder.Ascending);
                                }
                                for (int i = 0; i <= gridListEditor.Grid.VisibleRowCount; i++)
                                {
                                    int cursort = Convert.ToInt32(gridListEditor.Grid.GetRowValues(i, "Sort"));
                                    if (cursort != 0)
                                    {
                                        gridListEditor.Grid.Selection.SelectRow(i);
                                    }
                                }
                            }
                        }
                        //if (qcbatchinfo.IsPLMTest)
                        //{
                        //    ((ListView)qclist.InnerView).AllowEdit["CanSampleBatchEdit"] = true; 
                        //}
                        //else
                        //{
                        //    ((ListView)qclist.InnerView).AllowEdit["CanSampleBatchEdit"] = false;
                        //}
                    }
                }

                else
                {
                    if (curqC.Matrix == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "matrixnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (curqC.Test == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectTest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (curqC.Method == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Methodnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (string.IsNullOrEmpty(curqC.Jobid))
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "jobidnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }

                    //else if (string.IsNullOrEmpty(curqC.Instrument))
                    //{
                    //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectinstrument"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    //}
                    //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectJobId"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }

                ASPxGridListEditor qclistEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                if (qclistEditor != null && qclistEditor.Grid != null)
                {
                    foreach (GridViewColumn column in qclistEditor.Grid.Columns)
                    {
                        if (column.Name == "QCremove")
                        {
                            column.Visible = false;
                        }
                    }
                }
                ASPxGridListEditor qctypeEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                if (qctypeEditor != null && qctypeEditor.Grid != null)
                {
                    foreach (GridViewColumn column in qctypeEditor.Grid.Columns)
                    {
                        if (column.Name == "QCadd")
                        {
                            column.Visible = false;
                        }
                    }
                }
                if (qcbatchinfo.lststrseqlayercount != null)
                {
                    qcbatchinfo.lststrseqlayercount.Clear();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QCsort_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                DashboardViewItem qclist = ((DashboardView)View).FindItem("qclist") as DashboardViewItem;
                DashboardViewItem qcdetail = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                DashboardViewItem QCDetailViewComment = ((DashboardView)View).FindItem("Others") as DashboardViewItem;
                ActionContainerViewItem btnReset = ((DashboardView)View).FindItem("qcaction0") as ActionContainerViewItem;
                if (e.Action.Caption == "Sort" || e.Action.Caption == "序号")
                {
                    if (qclist != null && qcdetail != null && qctype != null && qclist.InnerView != null && qcdetail.InnerView != null && ((ListView)qclist.InnerView).CollectionSource.GetCount() > 0)
                    {
                        if (((ListView)qclist.InnerView).SelectedObjects.Count > 0)
                        {
                            SpreadSheetEntry_AnalyticalBatch batch = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                            batch.ISShown = false;
                            disenbcontrols(false, false, qcdetail.InnerView);
                            //foreach (QCBatchSequence sequences in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.Sort == 0).ToList())
                            foreach (QCBatchSequence sequences in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.Sort >= 0).ToList())
                            {
                                if (sequences.Sort == 0)
                                {
                                    qclist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequences);
                                    ((ListView)qclist.InnerView).CollectionSource.Remove(sequences);
                                    ((ListView)qclist.InnerView).Refresh();
                                }
                                else if (string.IsNullOrEmpty(sequences.Dilution) || string.IsNullOrWhiteSpace(sequences.Dilution))
                                {
                                    sequences.Dilution = sequences.DilutionCount.ToString();
                                }
                            }
                            qcbatchinfo.IsSortActionEnable = true;
                            //AddSequence
                            AddSequence(qclist, batch);
                            if (qcbatchinfo.lststrseqdilutioncount != null && qcbatchinfo.lststrseqdilutioncount.Count > 0)
                            {
                                foreach (string strseq in qcbatchinfo.lststrseqdilutioncount.ToList())
                                {
                                    int dcount = 0;
                                    string[] strarr = strseq.Split('|');
                                    if (strarr[1] != "null")
                                    {
                                        dcount = Convert.ToInt32(strarr[1]);
                                    }
                                    QCBatchSequence objqcseq = qclist.InnerView.ObjectSpace.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strarr[0].ToString())));
                                    if (objqcseq != null && objqcseq.Sort > 0)
                                    {
                                        int sortcnt = objqcseq.Sort;
                                        int rcnt = 1;
                                        for (int x = 0; x < dcount - 1; x++)
                                        {
                                            ASPxGridListEditor qclistgrid = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                                            if (qclistgrid != null && qclistgrid.Grid != null)
                                            {
                                                QCBatchSequence qCBatch = qclist.InnerView.ObjectSpace.CreateObject<QCBatchSequence>();
                                                qCBatch.QCType = qclist.InnerView.ObjectSpace.GetObjectByKey<QCType>(objqcseq.QCType.Oid);
                                                qCBatch.batchno = objqcseq.batchno;
                                                qCBatch.Runno = objqcseq.Runno;
                                                qCBatch.SampleID = objqcseq.SampleID;
                                                qCBatch.Sort = sortcnt;
                                                qCBatch.StrSampleID = objqcseq.StrSampleID;
                                                qCBatch.SYSSamplecode = objqcseq.SYSSamplecode + "R" + rcnt.ToString();
                                                qCBatch.IsDilution = true;
                                                ((ListView)qclist.InnerView).CollectionSource.Add(qCBatch);
                                                rcnt++;
                                            }
                                        }
                                    }
                                }
                                ((ListView)qclist.InnerView).Refresh();
                            }
                            ASPxGridListEditor gridListEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                            if (gridListEditor != null && gridListEditor.Grid != null)
                            {
                                gridListEditor.Grid.ClearSort();
                                gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Sort"], ColumnSortOrder.Ascending);
                                foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                                {
                                    if (column.Name == "SelectionCommandColumn")
                                    {
                                        column.Visible = true;
                                    }
                                    else if (column.Name == "QCremove")
                                    {
                                        column.Visible = true;
                                    }
                                }
                                gridListEditor.Grid.Columns["Sort"].Visible = false;
                                gridListEditor.Grid.Columns["SampleName"].Width = 150;
                                gridListEditor.Grid.Columns["SYSSamplecode"].Width = 200;
                                gridListEditor.Grid.Columns["DilutionCount"].Visible = false;
                                if (!qcbatchinfo.IsPLMTest)
                                {
                                    gridListEditor.Grid.Columns["Dilution"].Visible = true;
                                    gridListEditor.Grid.Columns["Dilution"].Width = 70;
                                    gridListEditor.Grid.Columns["Dilution"].SetColVisibleIndex(5);
                                }
                                gridListEditor.Grid.Selection.UnselectAll();
                                gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = false;// true;
                            }
                            ASPxGridListEditor qctypegridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                            if (qctypegridListEditor != null && qctypegridListEditor.Grid != null)
                            {
                                GridViewColumn QCadd = qctypegridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "QCadd").ToList()[0];
                                if (QCadd != null)
                                {
                                    QCadd.Visible = true;
                                    QCadd.VisibleIndex = 2;
                                }
                            }
                            if (qcbatchinfo.lststrseqlayercount != null && qcbatchinfo.lststrseqlayercount.Count > 0)
                            {
                                foreach (string strseq in qcbatchinfo.lststrseqlayercount.ToList())
                                {
                                    string[] strarr = strseq.Split('|');
                                    int dcount = Convert.ToInt32(strarr[1]);
                                    QCBatchSequence objqcseq = qclist.InnerView.ObjectSpace.FindObject<QCBatchSequence>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strarr[0].ToString())));
                                    if (objqcseq != null)
                                    {
                                        int rcnt = 1;
                                        {
                                            ASPxGridListEditor qclistgrid = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                                            if (qclistgrid != null && qclistgrid.Grid != null)
                                            {
                                                const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                                string valueeql = "";
                                                for (int i = 1; i <= dcount - 1; i++)
                                                {
                                                    QCBatchSequence qCBatch = qclist.InnerView.ObjectSpace.CreateObject<QCBatchSequence>();
                                                    qCBatch.QCType = qclist.InnerView.ObjectSpace.GetObjectByKey<QCType>(objqcseq.QCType.Oid);
                                                    qCBatch.batchno = objqcseq.batchno;
                                                    qCBatch.Runno = objqcseq.Runno;
                                                    qCBatch.SampleID = objqcseq.SampleID;
                                                    qCBatch.Sort = objqcseq.Sort;
                                                    qCBatch.StrSampleID = objqcseq.StrSampleID;
                                                    qCBatch.LayerCount = objqcseq.LayerCount;
                                                    qCBatch.SYSSamplecode = objqcseq.SYSSamplecode;
                                                    /////qCBatch.qcseqdetail = objqcseq.qcseqdetail;
                                                    valueeql = "";
                                                    if (i >= letterseql.Length)
                                                        valueeql += letterseql[i / letterseql.Length - 1];

                                                    valueeql += letterseql[i % letterseql.Length];
                                                    qCBatch.StrSampleL = qCBatch.SampleID.SampleID + "." + valueeql;
                                                    qCBatch.LayerCount = Convert.ToUInt32(strarr[1]);
                                                    ((ListView)qclist.InnerView).CollectionSource.Add(qCBatch);
                                                }
                                            }
                                        }
                                    }
                                }
                              ((ListView)qclist.InnerView).Refresh();
                            }
                            ((ListView)qclist.InnerView).Refresh();
                            ((ListView)qctype.InnerView).Refresh();

                            ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                            qcaction2.Actions[0].Enabled.SetItemValue("key", true);
                            if (CurrentLanguage == "En")
                            {
                                qcaction2.Actions[1].Caption = "Ok";
                                QCreset.Active.SetItemValue("btnreset", false);
                            }
                            else
                            {
                                qcaction2.Actions[1].Caption = "确定";
                            }
                            //foreach (QCBatchSequence qcbatchseq in ((ListView)qclist.InnerView).CollectionSource.List)
                            //{
                            //    qcbatchseq.Dilution = "1";
                            //}
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Selectsample"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }

                }
                else if (e.Action.Caption == "Ok" || e.Action.Caption == "确定")
                {
                    SpreadSheetEntry_AnalyticalBatch curqCbatch = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                    if (curqCbatch != null && curqCbatch.Test != null && curqCbatch.Matrix != null && curqCbatch.Method != null)
                    {
                        TestMethod testMethod = qcdetail.InnerView.ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [MethodName.MethodNumber] = ?", curqCbatch.Matrix.MatrixName, curqCbatch.Test.TestName, curqCbatch.Method.MethodName.MethodNumber));
                        if (testMethod != null)
                        {
                            curqCbatch.Matrix = testMethod.MatrixName;
                            curqCbatch.Test = testMethod;
                            curqCbatch.Method = testMethod;
                        }
                        SpreadSheetBuilder_TestParameter lnksdmstest = ObjectSpace.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID] = ?", curqCbatch.Test.Oid));
                        if (lnksdmstest != null || qcbatchinfo.IsPLMTest)
                        {
                            if (qclist != null && qcdetail != null && qclist.InnerView != null && qcdetail.InnerView != null)
                            {
                                QCreset.Active.SetItemValue("btnreset", true);
                                SpreadSheetEntry_AnalyticalBatch curqC = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                                if (curqC != null && !string.IsNullOrEmpty(curqC.Instrument))
                                {
                                    qcbatchinfo.strTest = curqC.Test.TestName;
                                    qcbatchinfo.OidTestMethod = curqC.Test.Oid;
                                    qcbatchinfo.strTestMethodMatrixName = curqC.Matrix.MatrixName;
                                    qcbatchinfo.strTestMethodTestName = curqC.Test.TestName;
                                    qcbatchinfo.strTestMethodMethodNumber = curqC.Method.MethodName.MethodNumber;
                                    qcbatchinfo.strTestMethodMethodNumberOid = curqC.Method.MethodName.Oid;
                                    qcbatchinfo.qcstatus = 0;
                                    //string tempqc = string.Empty;

                                    //if (qcdetail.InnerView.ObjectSpace.IsNewObject(curqC))
                                    //{
                                    curqC.NPJobid = curqC.Jobid;
                                    curqC.NPInstrument = curqC.strInstrument;
                                    ////if (objnavigationRefresh.ClickedNavigationItem == "QCbatch")
                                    ////{
                                    //    //string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                    //    //string userid = ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                    //    string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                    //    var curdate = DateTime.Now.ToString("yyMMdd");
                                    //    IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = qcdetail.InnerView.ObjectSpace.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 6)=? AND StartsWith([AnalyticalBatchID], 'QB')", curdate));
                                    //    if (spreadSheets.Count > 0)
                                    //    {
                                    //        spreadSheets = spreadSheets.OrderBy(a => a.AnalyticalBatchID).ToList();
                                    //        tempqc = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                                    //        //tempab = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                                    //        //tempqc = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].QCBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                                    //    }
                                    //    else
                                    //    {
                                    //        tempqc = "QB" + curdate + "01" + userid;
                                    //        //tempqc = "QB" + curdate + userid + "01";
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                    //    var curdate = DateTime.Now.ToString("yyMMdd");
                                    //    IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = qcdetail.InnerView.ObjectSpace.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 6)=? AND StartsWith([AnalyticalBatchID], 'QB')", curdate));
                                    //    if (spreadSheets.Count > 0)
                                    //    {
                                    //        spreadSheets = spreadSheets.OrderBy(a => a.AnalyticalBatchID).ToList();
                                    //        //tempqc = "AB" + curdate + userid + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].QCBatchID.Substring(11, 2)) + 1).ToString("00");
                                    //        tempqc = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;

                                    //    }
                                    //    else
                                    //    {
                                    //        //tempqc = "AB" + curdate + userid + "01";
                                    //        tempqc = "QB" + curdate + "01" + userid;
                                    //    }
                                    //}
                                    //    curqC.AnalyticalBatchID = qcbatchinfo.strqcid = tempqc;
                                    //}

                                    //if (QCDetailViewComment != null && QCDetailViewComment.InnerView != null)
                                    //{
                                    //    NonPersistentQcComment objComment = (NonPersistentQcComment)QCDetailViewComment.InnerView.CurrentObject;
                                    //    if (objComment != null)
                                    //    {
                                    //        curqC.Comments = objComment.Comments;
                                    //    }
                                    //}

                                    //if (!qcdetail.InnerView.ObjectSpace.IsNewObject(curqC))
                                    //{
                                    //    IList<QCBatchInstrument> instruments = qcdetail.InnerView.ObjectSpace.GetObjects<QCBatchInstrument>(CriteriaOperator.Parse("[QCBatchID.AnalyticalBatchID]=?", curqC.AnalyticalBatchID));
                                    //    foreach (QCBatchInstrument ins in instruments.ToList())
                                    //    {
                                    //        qcdetail.InnerView.ObjectSpace.Delete(ins);
                                    //    }
                                    //}

                                    //string[] ids = curqC.Instrument.Split(';');
                                    //foreach (string id in ids)
                                    //{
                                    //    QCBatchInstrument qcinstrument = qcdetail.InnerView.ObjectSpace.CreateObject<QCBatchInstrument>();
                                    //    qcinstrument.QCBatchID = curqC;
                                    //    qcinstrument.LabwareID = qcdetail.InnerView.ObjectSpace.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", new Guid(id.Replace(" ", ""))));
                                    //}
                                    //qcdetail.InnerView.ObjectSpace.CommitChanges();
                                    //qcbatchinfo.AnalyticalQCBatchOid = curqC.Oid;
                                    //qcbatchinfo.QCBatchOid = curqC.Oid;

                                    //if (!qclist.InnerView.ObjectSpace.IsNewObject(curqC))
                                    //{
                                    //    IList<SampleParameter> samples = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Oid]=?", curqC.Oid));
                                    //    foreach (SampleParameter sample in samples.ToList())
                                    //    {
                                    //        sample.UQABID = null;
                                    //        sample.QCBatchID = null;
                                    //        sample.QCSort = 0;
                                    //    }
                                    //}

                                    //IList<QCBatchSequence> seq = qclist.InnerView.ObjectSpace.GetObjects<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail.Oid]=?", qcbatchinfo.QCBatchOid));
                                    //foreach (QCBatchSequence qCBatch in seq.ToList())
                                    //{
                                    //    if (((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(x => x.Oid == qCBatch.Oid).ToList().Count == 0)
                                    //    {
                                    //        qclist.InnerView.ObjectSpace.Delete(qCBatch);
                                    //    }
                                    //}

                                    //SpreadSheetEntry_AnalyticalBatch qC = qclist.InnerView.ObjectSpace.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[Oid]=?", curqC.Oid), true);
                                    //if (qC != null)
                                    //{
                                    //    List<string> strdilution = new List<string>();
                                    //    List<string> strdilutionno = new List<string>();
                                    //    Guid qcseqoid = Guid.Empty;
                                    //    int sort = 0;
                                    //    foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(i => i.Sort).ToList())
                                    //    {
                                    //        sequence.qcseqdetail = qC;
                                    //        sequence.JOBID = qC.Jobid;
                                    //        if (sequence.IsDilution == true)
                                    //        {
                                    //            string[] strsamplid = sequence.SYSSamplecode.Split('R');
                                    //            //if (strsamplid != null && strsamplid.Length ==1)
                                    //            {
                                    //                IList<SampleParameter> lstsampleparams = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? and [Testparameter.QCType.Oid] =? And [SignOff] = True", strsamplid[0], qC.Test.Oid, sequence.QCType.Oid)); //sequence.SampleID.SampleID//and [Testparameter.TestMethod.Oid] =? , qC.Test.Oid
                                    //                if (lstsampleparams != null && lstsampleparams.Count > 0)
                                    //                {
                                    //                    foreach (SampleParameter sampleparams in lstsampleparams.ToList())
                                    //                    {
                                    //                        SampleParameter newsample = qclist.InnerView.ObjectSpace.CreateObject<SampleParameter>();
                                    //                        newsample.QCBatchID = sequence;
                                    //                        newsample.QCSort = sort;
                                    //                        newsample.SignOff = true;
                                    //                        newsample.UQABID = qclist.InnerView.ObjectSpace.GetObject(qC);
                                    //                        newsample.Samplelogin = qclist.InnerView.ObjectSpace.GetObject(sampleparams.Samplelogin);
                                    //                        newsample.Testparameter = qclist.InnerView.ObjectSpace.GetObject(sampleparams.Testparameter);
                                    //                        //newsample.SamplePrepBatchID = qclist.InnerView.ObjectSpace.GetObject(sampleparams.SamplePrepBatchID);
                                    //                        if (qcbatchinfo.lststrseqstringdilution != null && qcbatchinfo.lststrseqstringdilution.Count > 0)
                                    //                        {
                                    //                            foreach (string objstrdil in qcbatchinfo.lststrseqstringdilution.ToList())
                                    //                            {
                                    //                                string[] strdil = objstrdil.Split('|');
                                    //                                if (strdil[0].Contains(sequence.Oid.ToString()))
                                    //                                {
                                    //                                    newsample.Dilution = strdil[1];
                                    //                                }
                                    //                            }
                                    //                        }
                                    //                        sort++;
                                    //                    }
                                    //                }
                                    //            }

                                    //            ////SampleParameter sampleparams = qclist.InnerView.ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? And [SignOff] = True", strsamplid[0].ToString(), qC.Test.Oid)); //sequence.SampleID.SampleID
                                    //            ////IList<SampleParameter> lstsampleparams = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? And [SignOff] = True", strsamplid[0].ToString())); //sequence.SampleID.SampleID//and [Testparameter.TestMethod.Oid] =? , qC.Test.Oid

                                    //}
                                    //        else
                                    //        if (sequence.QCType != null && sequence.QCType.QCTypeName != "Sample")
                                    //        {
                                    //            IList<Testparameter> testparams = qclist.InnerView.ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.Oid]=? and [TestMethod.Oid]=?", sequence.QCType.Oid, qC.Test.Oid));
                                    //            //foreach (Testparameter testparam in testparams.OrderBy(a => a.Sort).ThenBy(i => i.Parameter.ParameterName).ToList())
                                    //            //{
                                    //            //    SampleParameter newsample = qclist.InnerView.ObjectSpace.CreateObject<SampleParameter>();
                                    //            //    newsample.QCBatchID = sequence;
                                    //            //    newsample.Testparameter = testparam;
                                    //            //    newsample.QCSort = sort;
                                    //            //    newsample.SignOff = true;
                                    //            //    //newsample.SignOffBy = qclist.InnerView.ObjectSpace.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                                    //            //    //newsample.SignOffDate = DateTime.Now;
                                    //            //    sort++;
                                    //            //}
                                    //            foreach (Testparameter testparam in testparams.OrderBy(a => a.Parameter.ParameterName).ToList())
                                    //            {
                                    //                SampleParameter newsample = qclist.InnerView.ObjectSpace.CreateObject<SampleParameter>();
                                    //                newsample.QCBatchID = sequence;
                                    //                newsample.Testparameter = testparam;
                                    //                newsample.QCSort = sort;
                                    //                newsample.UQABID = qclist.InnerView.ObjectSpace.GetObject(qC);
                                    //                newsample.SignOff = true;
                                    //                sort++;
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            IList<SampleParameter> sampleparams = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? and [Testparameter.QCType.Oid] =? And [SignOff] = True", sequence.SampleID.SampleID, qC.Test.Oid, sequence.QCType.Oid));
                                    //            //foreach (SampleParameter sampleparam in sampleparams.OrderBy(a => a.Testparameter.Sort).ThenBy(a => a.Testparameter.Parameter.ParameterName).ToList())
                                    //            //{
                                    //            //    sampleparam.QCBatchID = sequence;
                                    //            //    sampleparam.QCSort = sort;
                                    //            //    sort++;
                                    //            //}
                                    //            foreach (SampleParameter sampleparam in sampleparams.OrderBy(a => a.Testparameter.Parameter.ParameterName).ToList())
                                    //            {
                                    //                if (qcbatchinfo.lststrseqstringdilution != null && qcbatchinfo.lststrseqstringdilution.Count > 0)
                                    //                {
                                    //                    foreach (string objstrdil in qcbatchinfo.lststrseqstringdilution.ToList())
                                    //                    {
                                    //                        string[] strdil = objstrdil.Split('|');
                                    //                        if (strdil[0].Contains(sequence.Oid.ToString()))
                                    //                        {
                                    //                            sampleparam.Dilution = strdil[1];
                                    //                        }
                                    //                    }
                                    //                }
                                    //                sampleparam.QCBatchID = sequence;
                                    //                sampleparam.UQABID = qclist.InnerView.ObjectSpace.GetObject(qC);
                                    //                sampleparam.QCSort = sort;
                                    //                sort++;
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //qclist.InnerView.ObjectSpace.CommitChanges();

                                    //ASPxGridListEditor gridListEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                                    //if (gridListEditor != null && gridListEditor.Grid != null)
                                    //{
                                    //    gridListEditor.Grid.Columns["Sort"].Visible = true;
                                    //    gridListEditor.Grid.Columns["Sort"].Width = 50;
                                    //    gridListEditor.Grid.Columns["SampleName"].Width = 125;
                                    //    gridListEditor.Grid.Columns["SYSSamplecode"].Width = 200;
                                    //    if (!qcbatchinfo.IsPLMTest)
                                    //    {
                                    //        gridListEditor.Grid.Columns["DilutionCount"].Visible = true;
                                    //        gridListEditor.Grid.Columns["DilutionCount"].Width = 100;
                                    //        gridListEditor.Grid.Columns["DilutionCount"].SetColVisibleIndex(4);
                                    //    }
                                    //    gridListEditor.Grid.Columns["Dilution"].Visible = false;
                                    //    foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                                    //    {
                                    //        if (column.Name == "SelectionCommandColumn")
                                    //        {
                                    //            //column.Visible = false;
                                    //        }
                                    //        else if (column.Name == "QCremove")
                                    //        {
                                    //            column.Visible = false;
                                    //        }
                                    //    }
                                    //}
                                    //if (Frame.Context == TemplateContext.PopupWindow)
                                    //{
                                    //    qcbatchinfo.canfilter = true;
                                    //    (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
                                    //}
                                    //else
                                    //{
                                    //    string msg;
                                    //    if (CurrentLanguage == "En")
                                    //    {
                                    //        msg = "A QCBatch ID has been created. Do you want to open the SDMS raw data entry form?";
                                    //    }
                                    //    else
                                    //    {
                                    //        msg = "质控批次号已创建。是否要打开原始数据输入表单";
                                    //    }

                                    //    if (View.Id == "QCBatchsequence" && objnavigationRefresh.ClickedNavigationItem == "QCbatch")
                                    //    {
                                    //        Application.ShowViewStrategy.ShowMessage("A QCBatch ID " + tempqc + " has been created sucessfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    //        ASPxGridListEditor qctypegridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                                    //        if (qctypegridListEditor != null && qctypegridListEditor.Grid != null)
                                    //        {
                                    //            GridViewColumn QCadd = qctypegridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "QCadd").ToList()[0];
                                    //            if (QCadd != null)
                                    //            {
                                    //                QCadd.Visible = false;
                                    //            }
                                    //        }
                                    //        disenbcontrols(false, false, qcdetail.InnerView);
                                    //        ActionContainerViewItem qcaction0 = ((DashboardView)View).FindItem("qcaction0") as ActionContainerViewItem;
                                    //        ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                                    //        if (qcaction0 != null)
                                    //        {
                                    //            qcaction0.Actions[0].Enabled.SetItemValue("key", false);
                                    //            qcaction0.Actions[1].Enabled.SetItemValue("key", false);
                                    //        }
                                    //        if (qcaction2 != null)
                                    //        {
                                    //            qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                                    //            qcaction2.Actions[1].Enabled.SetItemValue("key", false);
                                    //        }
                                    //        qcbatchinfo.canfilter = true;
                                    //    }
                                    //    else
                                    //    {
                                    //New Comments
                                    //Application.ShowViewStrategy.ShowMessage("A QCBatch ID " + tempqc + " has been created sucessfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);

                                    //    ASPxGridListEditor qctypegridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                                    //        if (qctypegridListEditor != null && qctypegridListEditor.Grid != null)
                                    //        {
                                    //            GridViewColumn QCadd = qctypegridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "QCadd").ToList()[0];
                                    //            if (QCadd != null)
                                    //            {
                                    //                QCadd.Visible = false;
                                    //            }
                                    //        }
                                    //        disenbcontrols(false, false, qcdetail.InnerView);
                                    //        ActionContainerViewItem qcaction0 = ((DashboardView)View).FindItem("qcaction0") as ActionContainerViewItem;
                                    //        ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                                    //        if (qcaction0 != null)
                                    //        {
                                    //            qcaction0.Actions[0].Enabled.SetItemValue("key", false);
                                    //            qcaction0.Actions[1].Enabled.SetItemValue("key", false);
                                    //        }
                                    //        if (qcaction2 != null)
                                    //        {
                                    //            qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                                    //            qcaction2.Actions[1].Enabled.SetItemValue("key", false);
                                    //        }
                                    //        qcbatchinfo.canfilter = true;
                                    //    }
                                    //}
                                    objABinfo.lstSpreadSheetEntry_AnalyticalBatch = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                                    objABinfo.lstQCBatchSequence = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().ToList();
                                    if (qcbatchinfo != null && (qcbatchinfo.lststrseqstringdilution != null || qcbatchinfo.lststrseqstringSampleAmount != null || qcbatchinfo.lststrseqstringFinalVolume != null))
                                    {
                                        foreach (QCBatchSequence sequence in objABinfo.lstQCBatchSequence.ToList())
                                        {
                                            foreach (string objstrdil in qcbatchinfo.lststrseqstringdilution.ToList())
                                            {
                                                string[] strdil = objstrdil.Split('|');
                                                if (strdil[0].Contains(sequence.Oid.ToString()))
                                                {
                                                    sequence.Dilution = strdil[1];
                                                }
                                            }
                                            foreach (string objstrSA in qcbatchinfo.lststrseqstringSampleAmount.ToList())
                                            {
                                                string[] strSA = objstrSA.Split('|');
                                                if (strSA[0].Contains(sequence.Oid.ToString()))
                                                {
                                                    sequence.SampleAmount = strSA[1];
                                                }
                                            }
                                            foreach (string objstrFV in qcbatchinfo.lststrseqstringFinalVolume.ToList())
                                            {
                                                string[] strFV = objstrFV.Split('|');
                                                if (strFV[0].Contains(sequence.Oid.ToString()))
                                                {
                                                    sequence.FinalVolume = strFV[1];
                                                }
                                            }
                                        }
                                    }

                                    BindQCBatchSequenceToDataTable((ListView)qclist.InnerView);
                                    qcbatchinfo.canfilter = true;
                                    //Frame.GetController<SamplePreparationController>().ResetNavigationCount();
                                    if (Frame.Context == TemplateContext.PopupWindow)
                                    {
                                        (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
                                        qcbatchinfo.canfilter = true;
                                    }
                                    else
                                    {
                                        if (!qcbatchinfo.IsPLMTest)
                                        {
                                            Frame.SetView(Application.CreateDashboardView((NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet)), "SDMS", true));
                                        }
                                        else
                                        {
                                            Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "PLM", true));
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectinstrument"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }

                            }
                        }
                        else
                        {
                            SpreadSheetEntry_AnalyticalBatch curqC = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                            if (curqC != null && !string.IsNullOrEmpty(curqC.Instrument))
                            {
                                qcbatchinfo.strTest = curqC.Test.TestName;
                                qcbatchinfo.OidTestMethod = curqC.Test.Oid;
                                qcbatchinfo.strTestMethodMatrixName = curqC.Matrix.MatrixName;
                                qcbatchinfo.strTestMethodTestName = curqC.Test.TestName;
                                qcbatchinfo.strTestMethodMethodNumber = curqC.Method.MethodName.MethodNumber;
                                qcbatchinfo.strTestMethodMethodNumberOid = curqC.Method.MethodName.Oid;
                                qcbatchinfo.qcstatus = 0;
                                string tempqc = string.Empty;
                                curqC.NPJobid = curqC.Jobid;
                                curqC.NPInstrument = curqC.strInstrument;

                                if (qcdetail.InnerView.ObjectSpace.IsNewObject(curqC))
                                {
                                    curqC.NPJobid = curqC.Jobid;
                                    curqC.NPInstrument = curqC.strInstrument;
                                    if (objnavigationRefresh.ClickedNavigationItem == "QCbatch")
                                    {
                                        //string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                        //string userid = ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                        string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                        var curdate = DateTime.Now.ToString("yyMMdd");
                                        IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = qcdetail.InnerView.ObjectSpace.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 6)=? AND StartsWith([AnalyticalBatchID], 'QB')", curdate));
                                        if (spreadSheets.Count > 0)
                                        {
                                            spreadSheets = spreadSheets.OrderBy(a => a.AnalyticalBatchID).ToList();
                                            tempqc = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                                            //tempab = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                                            //tempqc = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].QCBatchID.Substring(8, 2)) + 1).ToString("00") + userid;
                                        }
                                        else
                                        {
                                            tempqc = "QB" + curdate + "01" + userid;
                                            //tempqc = "QB" + curdate + userid + "01";
                                        }
                                    }
                                    else
                                    {
                                        string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                        var curdate = DateTime.Now.ToString("yyMMdd");
                                        IList<SpreadSheetEntry_AnalyticalBatch> spreadSheets = qcdetail.InnerView.ObjectSpace.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("SUBSTRING([AnalyticalBatchID], 2, 6)=? AND StartsWith([AnalyticalBatchID], 'QB')", curdate));
                                        if (spreadSheets.Count > 0)
                                        {
                                            spreadSheets = spreadSheets.OrderBy(a => a.AnalyticalBatchID).ToList();
                                            //tempqc = "AB" + curdate + userid + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].QCBatchID.Substring(11, 2)) + 1).ToString("00");
                                            tempqc = "QB" + curdate + (Convert.ToInt32(spreadSheets[spreadSheets.Count - 1].AnalyticalBatchID.Substring(8, 2)) + 1).ToString("00") + userid;

                                        }
                                        else
                                        {
                                            //tempqc = "AB" + curdate + userid + "01";
                                            tempqc = "QB" + curdate + "01" + userid;
                                        }
                                    }
                                    curqC.AnalyticalBatchID = qcbatchinfo.strqcid = tempqc;
                                    OpenSDMSQcbatch.Enabled.SetItemValue("openSDMS", true);
                                }

                                if (QCDetailViewComment != null && QCDetailViewComment.InnerView != null)
                                {
                                    NonPersistentQcComment objComment = (NonPersistentQcComment)QCDetailViewComment.InnerView.CurrentObject;
                                    if (objComment != null)
                                    {
                                        curqC.Comments = objComment.Comments;
                                    }
                                }

                                if (!qcdetail.InnerView.ObjectSpace.IsNewObject(curqC))
                                {
                                    IList<QCBatchInstrument> instruments = qcdetail.InnerView.ObjectSpace.GetObjects<QCBatchInstrument>(CriteriaOperator.Parse("[QCBatchID.AnalyticalBatchID]=?", curqC.AnalyticalBatchID));
                                    foreach (QCBatchInstrument ins in instruments.ToList())
                                    {
                                        qcdetail.InnerView.ObjectSpace.Delete(ins);
                                    }
                                }

                                string[] ids = curqC.Instrument.Split(';');
                                foreach (string id in ids)
                                {
                                    QCBatchInstrument qcinstrument = qcdetail.InnerView.ObjectSpace.CreateObject<QCBatchInstrument>();
                                    qcinstrument.QCBatchID = curqC;
                                    qcinstrument.LabwareID = qcdetail.InnerView.ObjectSpace.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", new Guid(id.Replace(" ", ""))));
                                }
                                qcdetail.InnerView.ObjectSpace.CommitChanges();
                                qcbatchinfo.AnalyticalQCBatchOid = curqC.Oid;
                                qcbatchinfo.QCBatchOid = curqC.Oid;

                                if (!qclist.InnerView.ObjectSpace.IsNewObject(curqC))
                                {
                                    IList<SampleParameter> samples = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[QCBatchID.qcseqdetail.Oid]=? And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False)", curqC.Oid));
                                    foreach (SampleParameter sample in samples.ToList())
                                    {
                                        sample.UQABID = null;
                                        sample.QCBatchID = null;
                                        sample.QCSort = 0;
                                    }
                                }

                                IList<QCBatchSequence> seq = qclist.InnerView.ObjectSpace.GetObjects<QCBatchSequence>(CriteriaOperator.Parse("[qcseqdetail.Oid]=?", qcbatchinfo.QCBatchOid));
                                foreach (QCBatchSequence qCBatch in seq.ToList())
                                {
                                    if (((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(x => x.Oid == qCBatch.Oid).ToList().Count == 0)
                                    {
                                        qclist.InnerView.ObjectSpace.Delete(qCBatch);
                                    }
                                }

                                SpreadSheetEntry_AnalyticalBatch qC = qclist.InnerView.ObjectSpace.FindObject<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[Oid]=?", curqC.Oid), true);
                                if (qC != null)
                                {
                                    List<string> strdilution = new List<string>();
                                    List<string> strdilutionno = new List<string>();
                                    Guid qcseqoid = Guid.Empty;
                                    int sort = 0;
                                    foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(i => i.Sort).ToList())
                                    {
                                        sequence.qcseqdetail = qC;
                                        sequence.JOBID = qC.Jobid;
                                        if (sequence.IsDilution == true)
                                        {
                                            string[] strsamplid = sequence.SYSSamplecode.Split('R');
                                            //if (strsamplid != null && strsamplid.Length ==1)
                                            {
                                                IList<SampleParameter> lstsampleparams = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? and [Testparameter.QCType.Oid] =? And [SignOff] = True And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False)", strsamplid[0], qC.Test.Oid, sequence.QCType.Oid)); //sequence.SampleID.SampleID//and [Testparameter.TestMethod.Oid] =? , qC.Test.Oid
                                                if (lstsampleparams != null && lstsampleparams.Count > 0)
                                                {
                                                    foreach (SampleParameter sampleparams in lstsampleparams.ToList())
                                                    {
                                                        SampleParameter newsample = qclist.InnerView.ObjectSpace.CreateObject<SampleParameter>();
                                                        newsample.QCBatchID = sequence;
                                                        newsample.QCSort = sort;
                                                        newsample.SignOff = true;
                                                        newsample.UQABID = qclist.InnerView.ObjectSpace.GetObject(qC);
                                                        newsample.Samplelogin = qclist.InnerView.ObjectSpace.GetObject(sampleparams.Samplelogin);
                                                        newsample.Testparameter = qclist.InnerView.ObjectSpace.GetObject(sampleparams.Testparameter);
                                                        //newsample.SamplePrepBatchID = qclist.InnerView.ObjectSpace.GetObject(sampleparams.SamplePrepBatchID);
                                                        if (qcbatchinfo.lststrseqstringdilution != null && qcbatchinfo.lststrseqstringdilution.Count > 0)
                                                        {
                                                            foreach (string objstrdil in qcbatchinfo.lststrseqstringdilution.ToList())
                                                            {
                                                                string[] strdil = objstrdil.Split('|');
                                                                if (strdil[0].Contains(sequence.Oid.ToString()))
                                                                {
                                                                    newsample.Dilution = strdil[1];
                                                                }
                                                            }
                                                        }
                                                        sort++;
                                                    }
                                                }
                                            }

                                            ////SampleParameter sampleparams = qclist.InnerView.ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? And [SignOff] = True", strsamplid[0].ToString(), qC.Test.Oid)); //sequence.SampleID.SampleID
                                            ////IList<SampleParameter> lstsampleparams = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? And [SignOff] = True", strsamplid[0].ToString())); //sequence.SampleID.SampleID//and [Testparameter.TestMethod.Oid] =? , qC.Test.Oid

                                        }
                                        else
                                        if (sequence.QCType != null && sequence.QCType.QCTypeName != "Sample")
                                        {
                                            IList<Testparameter> testparams = qclist.InnerView.ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.Oid]=? and [TestMethod.Oid]=?", sequence.QCType.Oid, qC.Test.Oid));
                                            //foreach (Testparameter testparam in testparams.OrderBy(a => a.Sort).ThenBy(i => i.Parameter.ParameterName).ToList())
                                            //{
                                            //    SampleParameter newsample = qclist.InnerView.ObjectSpace.CreateObject<SampleParameter>();
                                            //    newsample.QCBatchID = sequence;
                                            //    newsample.Testparameter = testparam;
                                            //    newsample.QCSort = sort;
                                            //    newsample.SignOff = true;
                                            //    //newsample.SignOffBy = qclist.InnerView.ObjectSpace.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                                            //    //newsample.SignOffDate = DateTime.Now;
                                            //    sort++;
                                            //}
                                            foreach (Testparameter testparam in testparams.OrderBy(a => a.Parameter.ParameterName).ToList())
                                            {
                                                SampleParameter newsample = qclist.InnerView.ObjectSpace.CreateObject<SampleParameter>();
                                                newsample.QCBatchID = sequence;
                                                newsample.Testparameter = testparam;
                                                newsample.QCSort = sort;
                                                newsample.UQABID = qclist.InnerView.ObjectSpace.GetObject(qC);
                                                newsample.SignOff = true;
                                                sort++;
                                            }
                                        }
                                        else
                                        {
                                            IList<SampleParameter> sampleparams = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.SampleID]=? and [Testparameter.TestMethod.Oid] =? and [Testparameter.QCType.Oid] =? And [SignOff] = True", sequence.SampleID.SampleID, qC.Test.Oid, sequence.QCType.Oid));
                                            //foreach (SampleParameter sampleparam in sampleparams.OrderBy(a => a.Testparameter.Sort).ThenBy(a => a.Testparameter.Parameter.ParameterName).ToList())
                                            //{
                                            //    sampleparam.QCBatchID = sequence;
                                            //    sampleparam.QCSort = sort;
                                            //    sort++;
                                            //}
                                            foreach (SampleParameter sampleparam in sampleparams.OrderBy(a => a.Testparameter.Parameter.ParameterName).ToList())
                                            {
                                                if (qcbatchinfo.lststrseqstringdilution != null && qcbatchinfo.lststrseqstringdilution.Count > 0)
                                                {
                                                    foreach (string objstrdil in qcbatchinfo.lststrseqstringdilution.ToList())
                                                    {
                                                        string[] strdil = objstrdil.Split('|');
                                                        if (strdil[0].Contains(sequence.Oid.ToString()))
                                                        {
                                                            sampleparam.Dilution = strdil[1];
                                                        }
                                                    }
                                                }
                                                sampleparam.QCBatchID = sequence;
                                                sampleparam.UQABID = qclist.InnerView.ObjectSpace.GetObject(qC);
                                                sampleparam.QCSort = sort;
                                                sort++;
                                            }
                                        }
                                        if (sequence != null)
                                        {
                                            if (qcbatchinfo != null && qcbatchinfo.lststrseqstringSampleAmount != null)
                                            {
                                                foreach (string objstrSA in qcbatchinfo.lststrseqstringSampleAmount.ToList())
                                                {
                                                    string[] strSA = objstrSA.Split('|');
                                                    if (strSA[0].Contains(sequence.Oid.ToString()))
                                                    {
                                                        sequence.SampleAmount = strSA[1];
                                                    }
                                                }
                                            }
                                            if (qcbatchinfo != null && qcbatchinfo.lststrseqstringFinalVolume != null)
                                            {
                                                foreach (string objstrFV in qcbatchinfo.lststrseqstringFinalVolume.ToList())
                                                {
                                                    string[] strFV = objstrFV.Split('|');
                                                    if (strFV[0].Contains(sequence.Oid.ToString()))
                                                    {
                                                        sequence.FinalVolume = strFV[1];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                qclist.InnerView.ObjectSpace.CommitChanges();
                                ((ASPxGridListEditor)((ListView)qclist.InnerView).Editor).Grid.UpdateEdit();
                                //qclist.InnerView.Refresh();
                                //View.Refresh();


                                ASPxGridListEditor gridListEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null && gridListEditor.Grid != null)
                                {
                                    gridListEditor.Grid.Columns["Sort"].Visible = true;
                                    gridListEditor.Grid.Columns["Sort"].Width = 50;
                                    gridListEditor.Grid.Columns["SampleName"].Width = 125;
                                    gridListEditor.Grid.Columns["SYSSamplecode"].Width = 200;
                                    if (!qcbatchinfo.IsPLMTest)
                                    {
                                        gridListEditor.Grid.Columns["DilutionCount"].Visible = true;
                                        gridListEditor.Grid.Columns["DilutionCount"].Width = 100;
                                        gridListEditor.Grid.Columns["DilutionCount"].SetColVisibleIndex(4);
                                    }
                                    gridListEditor.Grid.Columns["Dilution"].Visible = false;
                                    foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                                    {
                                        if (column.Name == "SelectionCommandColumn")
                                        {
                                            //column.Visible = false;
                                        }
                                        else if (column.Name == "QCremove")
                                        {
                                            column.Visible = false;
                                        }
                                    }
                                }
                                qcbatchinfo.strTest = null;
                                qcbatchinfo.OidTestMethod = null;
                                if (Frame.Context == TemplateContext.PopupWindow)
                                {
                                    Application.ShowViewStrategy.ShowMessage("A QCBatch ID " + tempqc + " has been created sucessfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    qcbatchinfo.canfilter = true;
                                    (Frame as DevExpress.ExpressApp.Web.PopupWindow).Close(true);
                                }
                                else
                                {
                                    string msg;
                                    if (CurrentLanguage == "En")
                                    {
                                        msg = "A QCBatch ID has been created. Do you want to open the SDMS raw data entry form?";
                                    }
                                    else
                                    {
                                        msg = "质控批次号已创建。是否要打开原始数据输入表单";
                                    }

                                    if (View.Id == "QCBatchsequence" && objnavigationRefresh.ClickedNavigationItem == "QCbatch")
                                    {
                                        Application.ShowViewStrategy.ShowMessage("A QCBatch ID " + tempqc + " has been created sucessfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                                        ASPxGridListEditor qctypegridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                                        if (qctypegridListEditor != null && qctypegridListEditor.Grid != null)
                                        {
                                            GridViewColumn QCadd = qctypegridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "QCadd").ToList()[0];
                                            if (QCadd != null)
                                            {
                                                QCadd.Visible = false;
                                            }
                                        }
                                        disenbcontrols(false, false, qcdetail.InnerView);
                                        ActionContainerViewItem qcaction0 = ((DashboardView)View).FindItem("qcaction0") as ActionContainerViewItem;
                                        ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                                        if (qcaction0 != null)
                                        {
                                            qcaction0.Actions[0].Enabled.SetItemValue("key", false);
                                            qcaction0.Actions[1].Enabled.SetItemValue("key", false);
                                        }
                                        if (qcaction2 != null)
                                        {
                                            qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                                            qcaction2.Actions[1].Enabled.SetItemValue("key", false);
                                        }
                                        qcbatchinfo.canfilter = true;
                                        Frame.SetView(Application.CreateListView(typeof(SpreadSheetEntry_AnalyticalBatch), true));
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage("A QCBatch ID " + tempqc + " has been created sucessfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                                        ASPxGridListEditor qctypegridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                                        if (qctypegridListEditor != null && qctypegridListEditor.Grid != null)
                                        {
                                            GridViewColumn QCadd = qctypegridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "QCadd").ToList()[0];
                                            if (QCadd != null)
                                            {
                                                QCadd.Visible = false;
                                            }
                                        }
                                        disenbcontrols(false, false, qcdetail.InnerView);
                                        ActionContainerViewItem qcaction0 = ((DashboardView)View).FindItem("qcaction0") as ActionContainerViewItem;
                                        ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                                        if (qcaction0 != null)
                                        {
                                            qcaction0.Actions[0].Enabled.SetItemValue("key", false);
                                            qcaction0.Actions[1].Enabled.SetItemValue("key", false);
                                        }
                                        if (qcaction2 != null)
                                        {
                                            qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                                            qcaction2.Actions[1].Enabled.SetItemValue("key", false);
                                        }
                                        qcbatchinfo.canfilter = true;
                                        objQPInfo.lstQCBatchID = new List<string>();
                                        objQPInfo.objJobID = null;
                                        objQPInfo.lstQCBatchID.Add(curqC.AnalyticalBatchID);
                                        objQPInfo.objJobID = curqC.Jobid;
                                        objQPInfo.ResultEntryQueryFilter = "[QCBatchID.qcseqdetail.AnalyticalBatchID] IN (" + "'" + string.Join("','", objQPInfo.lstQCBatchID) + "'" + ") And [SignOff] = True";
                                        objQPInfo.QCResultEntryQueryFilter = objQPInfo.ResultEntryQueryFilter;
                                        IObjectSpace objspace = Application.CreateObjectSpace();
                                        CollectionSource cs = new CollectionSource(objspace, typeof(SampleParameter));
                                        CustomSystemUser currentUser = (CustomSystemUser)SecuritySystem.CurrentUser;
                                        List<Guid> lstTests = new List<Guid>();
                                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) == null)
                                        {
                                            IList<AnalysisDepartmentChain> lstAnalysisDepartChain = ObjectSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] = True", currentUser.Oid));
                                            lstTests = lstAnalysisDepartChain.SelectMany(x => x.TestMethods).Select(x => x.Oid).Distinct().ToList();
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
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectinstrument"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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

        private void AddSequence(DashboardViewItem qclist, SpreadSheetEntry_AnalyticalBatch batch)
        {
            try
            {
                IList<SpreadSheetBuilder_InitialQCTestRun> initialQCTestRuns = new List<SpreadSheetBuilder_InitialQCTestRun>();
                IList<SpreadSheetBuilder_SampleQCTestRun> sampleQCTestRuns = new List<SpreadSheetBuilder_SampleQCTestRun>();
                IList<SpreadSheetBuilder_ClosingQCTestRun> closingQCTestRuns = new List<SpreadSheetBuilder_ClosingQCTestRun>();
                SpreadSheetBuilder_SequencePattern sequencePattern = ((ListView)qclist.InnerView).ObjectSpace.FindObject<SpreadSheetBuilder_SequencePattern>(CriteriaOperator.Parse("[uqTestMethodID] =? AND [uqMatrixID]=?", batch.Test.Oid, batch.Matrix.Oid));
                if (sequencePattern != null)
                {
                    initialQCTestRuns = ((ListView)qclist.InnerView).ObjectSpace.GetObjects<SpreadSheetBuilder_InitialQCTestRun>(CriteriaOperator.Parse("[fuqSequencePatternID] =?", sequencePattern.uqSequencePatternID));
                    sampleQCTestRuns = ((ListView)qclist.InnerView).ObjectSpace.GetObjects<SpreadSheetBuilder_SampleQCTestRun>(CriteriaOperator.Parse("[fuqSequencePatternID] =?", sequencePattern.uqSequencePatternID));
                    if (sequencePattern.IsClosingQC)
                    {
                        closingQCTestRuns = ((ListView)qclist.InnerView).ObjectSpace.GetObjects<SpreadSheetBuilder_ClosingQCTestRun>(CriteriaOperator.Parse("[fuqSequencePatternID] =?", sequencePattern.uqSequencePatternID));
                    }

                    //InitialQC
                    int index = 0;
                    if (initialQCTestRuns != null && initialQCTestRuns.Count > 0)
                    {
                        foreach (SpreadSheetBuilder_InitialQCTestRun testRun in initialQCTestRuns.OrderBy(a => a.Order).ToList())
                        {
                            if (testRun.uqQCTypeID != null)
                            {
                                QCType qcType = ((ListView)qclist.InnerView).ObjectSpace.GetObjectByKey<QCType>(testRun.uqQCTypeID.Oid);
                                int intRunNo = 1;
                                if (qcType != null)
                                {
                                    if (((ListView)qclist.InnerView).CollectionSource != null && ((ListView)qclist.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        List<QCBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                        if (lstTestRuns != null && lstTestRuns.Count > 0)
                                        {
                                            intRunNo = lstTestRuns.Count + 1;
                                        }
                                    }
                                    QCBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<QCBatchSequence>();
                                    sample.QCType = qcType;
                                    sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                    sample.SystemID = qcType.QCTypeName + intRunNo;
                                    sample.Sort = index;
                                    sample.Runno = intRunNo;
                                    sample.Dilution = "1";
                                    IList<SamplePrepBatch> objlist = ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("[Jobid]=?", batch.Jobid));
                                    if (objlist != null)
                                    {
                                        bool tier2Found1 = false;
                                        foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 2))
                                        {
                                            SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                            if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                            {
                                                sample.SampleAmount = prepSequence.SampleAmount;
                                                sample.FinalVolume = prepSequence.FinalVolume;
                                                tier2Found1 = true;
                                                break;
                                            }
                                        }
                                        if (!tier2Found1)
                                        {
                                            foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 1))
                                            {
                                                SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                {
                                                    sample.SampleAmount = prepSequence.SampleAmount;
                                                    sample.FinalVolume = prepSequence.FinalVolume;
                                                }
                                            }
                                        }
                                    }

                                    int tempindex = index;
                                    if (qcType.QCSource != null && qcType.QCSource.QC_Source == "LCS")
                                    {
                                        Int32 iLCSCount = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(x => x.QCType.QCTypeName == "LCS").OrderBy(a => a.Sort).ToList().Count;
                                        if (iLCSCount > 0)
                                        {
                                            QCBatchSequence qblist = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(x => x.QCType.QCTypeName == "LCS").OrderBy(a => a.Sort).ToList()[iLCSCount - 1];
                                            sample.StrSampleID = qblist.SYSSamplecode;
                                        }
                                    }
                                    foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(a => a.Sort).ToList())
                                    {
                                        if (tempindex == sequence.Sort)
                                        {
                                            sequence.Sort += 1;
                                            tempindex = sequence.Sort;
                                            //if (string.IsNullOrEmpty(sample.Dilution) || string.IsNullOrWhiteSpace(sample.Dilution))
                                            //{
                                            //    sample.Dilution = "1";
                                            //}
                                        }
                                    }
                                    ((ListView)qclist.InnerView).CollectionSource.Add(sample);
                                    index++;
                                }
                            }
                        }
                    }

                    //SampleQC
                    int batchno = 1;
                    int intEntryCount = 0;
                    int intIterations = 0;
                    string strLastSample = string.Empty;

                    List<QCBatchSequence> lstSamples = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(x => x.SampleID != null).OrderBy(a => a.Sort).ToList();
                    QCBatchSequence objLastSample = lstSamples.OrderByDescending(i => i.Sort).FirstOrDefault();
                    QCBatchSequence objFirstSample = lstSamples.OrderBy(i => i.Sort).FirstOrDefault();

                    //Sample QC 1st iteration NonSample QCTypeTestRuns
                    if (sampleQCTestRuns != null && sampleQCTestRuns.Count > 0)
                    {
                        //List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && (x.uqQCTypeID.QCRootRole != QCRoleCN.平行 && x.uqQCTypeID.QCRootRole != QCRoleCN.加标)).OrderBy(a => a.Order).ToList();
                        //List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && (x.uqQCTypeID.QCSource == null || (x.uqQCTypeID.QCSource != null && x.uqQCTypeID.QCSource.QC_Source != "Sample")) && (x.uqQCTypeID.QCRootRole.IsDuplicate == false && x.uqQCTypeID.QCRootRole.IsSpike == false)).OrderBy(a => a.Order).ToList();
                        //List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && (x.uqQCTypeID.QCSource == null || (x.uqQCTypeID.QCSource != null && x.uqQCTypeID.QCSource.QC_Source != "Sample"))).OrderBy(a => a.Order).ToList();
                        List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.HasSampleAsQCSource == false).OrderBy(a => a.Order).ToList();
                        if (lstTest != null && lstTest.Count > 0)
                        {
                            foreach (SpreadSheetBuilder_SampleQCTestRun testRun in lstTest)
                            {
                                int intRunNo = 1;
                                QCType qcType = ((ListView)qclist.InnerView).ObjectSpace.GetObjectByKey<QCType>(testRun.uqQCTypeID.Oid);
                                if (qcType != null)
                                {
                                    if (((ListView)qclist.InnerView).CollectionSource != null && ((ListView)qclist.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        List<QCBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                        if (lstTestRuns != null && lstTestRuns.Count > 0)
                                        {
                                            intRunNo = lstTestRuns.Count + 1;
                                        }
                                    }
                                    QCBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<QCBatchSequence>();
                                    sample.QCType = qcType;
                                    sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                    sample.SystemID = qcType.QCTypeName + intRunNo;
                                    sample.Sort = index;
                                    //sample.Runno = intRunNo;
                                    sample.Runno = batchno;
                                    sample.Dilution = "1";
                                    IList<SamplePrepBatch> objlist = ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("[Jobid]=?", batch.Jobid));
                                    if (objlist != null)
                                    {
                                        bool tier2Found2 = false;
                                        foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 2))
                                        {
                                            SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                            if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                            {
                                                sample.SampleAmount = prepSequence.SampleAmount;
                                                sample.FinalVolume = prepSequence.FinalVolume;
                                                tier2Found2 = true;
                                                break;
                                            }
                                        }
                                        if (!tier2Found2)
                                        {
                                            foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 1))
                                            {
                                                SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                {
                                                    sample.SampleAmount = prepSequence.SampleAmount;
                                                    sample.FinalVolume = prepSequence.FinalVolume;
                                                }
                                            }
                                        }
                                    }
                                    int tempindex = index;
                                    foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(a => a.Sort).ToList())
                                    {
                                        if (tempindex == sequence.Sort)
                                        {
                                            sequence.Sort += 1;
                                            tempindex = sequence.Sort;
                                            //if (string.IsNullOrEmpty(sample.Dilution) || string.IsNullOrWhiteSpace(sample.Dilution))
                                            //{
                                            //    sample.Dilution = "1";
                                            //}
                                        }
                                    }
                                    ((ListView)qclist.InnerView).CollectionSource.Add(sample);
                                    index++;
                                }
                            }
                        }
                    }

                    foreach (QCBatchSequence qCBatch in lstSamples)
                    {
                        intIterations++;
                        intEntryCount++;
                        //if (sequencePattern.NumberOfSamplesBetweenQCTest == intIterations) seq and postion same
                        //{
                        //    batchno += 1;
                        //    intIterations = 0;
                        //}

                        if (intIterations == 0 && objFirstSample != null && objFirstSample.Oid != qCBatch.Oid)
                        {
                            //List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && (x.uqQCTypeID.QCSource == null || (x.uqQCTypeID.QCSource != null && x.uqQCTypeID.QCSource.QC_Source != "Sample")) && (x.uqQCTypeID.QCRootRole.IsDuplicate == false && x.uqQCTypeID.QCRootRole.IsSpike == false)).OrderBy(a => a.Order).ToList();
                            List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.HasSampleAsQCSource == false).OrderBy(a => a.Order).ToList();
                            if (lstTest != null && lstTest.Count > 0)
                            {
                                index = qCBatch.Sort + 1;
                                foreach (SpreadSheetBuilder_SampleQCTestRun testRun in lstTest)
                                {
                                    int intRunNo = 1;
                                    QCType qcType = ((ListView)qclist.InnerView).ObjectSpace.GetObjectByKey<QCType>(testRun.uqQCTypeID.Oid);
                                    if (qcType != null)
                                    {
                                        if (((ListView)qclist.InnerView).CollectionSource != null && ((ListView)qclist.InnerView).CollectionSource.GetCount() > 0)
                                        {
                                            List<QCBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                            if (lstTestRuns != null && lstTestRuns.Count > 0)
                                            {
                                                intRunNo = lstTestRuns.Count + 1;
                                            }
                                        }
                                        QCBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<QCBatchSequence>();
                                        sample.QCType = qcType;
                                        sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                        sample.Sort = index;
                                        //sample.Runno = intRunNo;
                                        sample.Runno = batchno;
                                        sample.Dilution = "1";
                                        int tempindex = index;
                                        IList<SamplePrepBatch> objlist = ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("[Jobid]=?", batch.Jobid));
                                        if (objlist != null)
                                        {
                                            bool tier2Found3 = false;
                                            foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 2))
                                            {
                                                SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                {
                                                    sample.SampleAmount = prepSequence.SampleAmount;
                                                    sample.FinalVolume = prepSequence.FinalVolume;
                                                    tier2Found3 = true;
                                                    break;
                                                }
                                            }
                                            if (!tier2Found3)
                                            {
                                                foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 1))
                                                {
                                                    SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                    if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                    {
                                                        sample.SampleAmount = prepSequence.SampleAmount;
                                                        sample.FinalVolume = prepSequence.FinalVolume;
                                                    }
                                                }
                                            }
                                        }
                                        foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(a => a.Sort).ToList())
                                        {
                                            if (tempindex == sequence.Sort)
                                            {
                                                sequence.Sort += 1;
                                                tempindex = sequence.Sort;
                                                //if (string.IsNullOrEmpty(sample.Dilution) || string.IsNullOrWhiteSpace(sample.Dilution))
                                                //{
                                                //    sample.Dilution = "1";
                                                //}
                                            }
                                        }
                                        ((ListView)qclist.InnerView).CollectionSource.Add(sample);
                                        index++;
                                    }
                                }
                            }
                        }

                        if (sequencePattern.DupSamplesAfterNoOfSamples == intIterations)
                        {
                            if (sampleQCTestRuns != null && sampleQCTestRuns.Count > 0)
                            {
                                //List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && x.uqQCTypeID.QCRootRole == QCRoleCN.平行).OrderBy(a => a.Order).ToList();
                                List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && x.HasSampleAsQCSource == true && x.uqQCTypeID.QCRootRole.IsDuplicate == true).OrderBy(a => a.Order).ToList();
                                if (lstTest != null && lstTest.Count > 0)
                                {
                                    index = qCBatch.Sort + 1;
                                    foreach (SpreadSheetBuilder_SampleQCTestRun testRun in lstTest.OrderBy(i => i.Order))
                                    {
                                        QCType qcType = ((ListView)qclist.InnerView).ObjectSpace.GetObjectByKey<QCType>(testRun.uqQCTypeID.Oid);
                                        if (qcType != null)
                                        {
                                            //if (sequencePattern.DupSamplesAfterNoOfSamples == sequencePattern.SpikeSamplesAfterNoOfSamples)
                                            //{
                                            //    index = qCBatch.Sort + testRun.Order;
                                            //}
                                            QCBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<QCBatchSequence>();
                                            sample.QCType = qcType;
                                            sample.batchno = qCBatch.batchno;
                                            sample.SampleID = qCBatch.SampleID;
                                            sample.Dilution = "1";
                                            if (qcType.QCSource != null)
                                            {
                                                if (qcType.QCSource.QC_Source == "Sample")
                                                {
                                                    sample.StrSampleID = qCBatch.SampleID.SampleID;
                                                    sample.SystemID = qcType.QCTypeName + batchno;
                                                    sample.SYSSamplecode = qCBatch.SampleID.SampleID + qcType.QCTypeName + batchno;
                                                }
                                                else if (qcType.QCSource.QC_Source == "MS")
                                                {
                                                    sample.StrSampleID = qcType.QCSource.QC_Source + batchno;
                                                    sample.SystemID = qcType.QCTypeName + batchno;
                                                    sample.SYSSamplecode = qCBatch.SampleID.SampleID + qcType.QCTypeName + batchno;
                                                }
                                                else
                                                {
                                                    sample.StrSampleID = qcType.QCSource.QC_Source;
                                                    sample.SystemID = qcType.QCTypeName + batchno;
                                                    sample.SYSSamplecode = qcType.QCTypeName + batchno;
                                                }
                                            }
                                            else
                                            {
                                                sample.SystemID = qcType.QCTypeName + batchno;
                                                sample.SYSSamplecode = qcType.QCTypeName + batchno;
                                            }
                                            IList<SamplePrepBatch> objlist = ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("[Jobid]=?", batch.Jobid));
                                            if (objlist != null)
                                            {
                                                bool tier2Found4 = false;
                                                foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 2))
                                                {
                                                    SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                    if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                    {
                                                        sample.SampleAmount = prepSequence.SampleAmount;
                                                        sample.FinalVolume = prepSequence.FinalVolume;
                                                        tier2Found4 = true;
                                                        break;
                                                    }
                                                }
                                                if (!tier2Found4)
                                                {
                                                    foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 1))
                                                    {
                                                        SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                        if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                        {
                                                            sample.SampleAmount = prepSequence.SampleAmount;
                                                            sample.FinalVolume = prepSequence.FinalVolume;
                                                        }
                                                    }
                                                }
                                            }
                                            sample.Sort = index;
                                            sample.Runno = batchno;
                                            int tempindex = index;
                                            foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(a => a.Sort).ToList())
                                            {
                                                if (tempindex == sequence.Sort)
                                                {
                                                    sequence.Sort += 1;
                                                    tempindex = sequence.Sort;
                                                    //if (string.IsNullOrEmpty(sample.Dilution) || string.IsNullOrWhiteSpace(sample.Dilution))
                                                    //{
                                                    //    sample.Dilution = "1";
                                                    //}
                                                }
                                            }
                                            ((ListView)qclist.InnerView).CollectionSource.Add(sample);
                                            index++;
                                        }
                                    }
                                }
                            }
                        }

                        if (sequencePattern.SpikeSamplesAfterNoOfSamples == intIterations)
                        {
                            if (sampleQCTestRuns != null && sampleQCTestRuns.Count > 0)
                            {
                                //List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && x.uqQCTypeID.QCRootRole == QCRoleCN.加标).OrderBy(a => a.Order).ToList();
                                List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && x.HasSampleAsQCSource == true && x.uqQCTypeID.QCRootRole.IsSpike == true).OrderBy(a => a.Order).ToList();
                                if (lstTest != null && lstTest.Count > 0)
                                {
                                    index = qCBatch.Sort + 1;
                                    foreach (SpreadSheetBuilder_SampleQCTestRun testRun in lstTest)
                                    {
                                        QCType qcType = ((ListView)qclist.InnerView).ObjectSpace.GetObjectByKey<QCType>(testRun.uqQCTypeID.Oid);
                                        if (qcType != null)
                                        {
                                            if (sequencePattern.DupSamplesAfterNoOfSamples == sequencePattern.SpikeSamplesAfterNoOfSamples)
                                            {
                                                index = qCBatch.Sort + testRun.Order;
                                            }
                                            QCBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<QCBatchSequence>();
                                            sample.QCType = qcType;
                                            sample.batchno = qCBatch.batchno;
                                            sample.SampleID = qCBatch.SampleID;
                                            sample.Dilution = "1";
                                            if (qcType.QCSource != null)
                                            {
                                                if (qcType.QCSource.QC_Source == "Sample")
                                                {
                                                    sample.StrSampleID = qCBatch.SampleID.SampleID;
                                                    sample.SystemID = qcType.QCTypeName + batchno;
                                                    sample.SYSSamplecode = qCBatch.SampleID.SampleID + qcType.QCTypeName + batchno;
                                                }
                                                else if (qcType.QCSource.QC_Source == "MS")
                                                {
                                                    sample.StrSampleID = qcType.QCSource.QC_Source + batchno;
                                                    sample.SystemID = qcType.QCTypeName + batchno;
                                                    sample.SYSSamplecode = qCBatch.SampleID.SampleID + qcType.QCTypeName + batchno;
                                                }
                                                else
                                                {
                                                    sample.StrSampleID = qcType.QCSource.QC_Source;
                                                    sample.SystemID = qcType.QCTypeName + batchno;
                                                    sample.SYSSamplecode = qcType.QCTypeName + batchno;
                                                }
                                            }
                                            else
                                            {
                                                sample.SystemID = qcType.QCTypeName + batchno;
                                                sample.SYSSamplecode = qcType.QCTypeName + batchno;
                                            }
                                            IList<SamplePrepBatch> objlist = ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("[Jobid]=?", batch.Jobid));
                                            if (objlist != null)
                                            {
                                                bool tier2Found5 = false;
                                                foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 2))
                                                {
                                                    SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                    if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                    {
                                                        sample.SampleAmount = prepSequence.SampleAmount;
                                                        sample.FinalVolume = prepSequence.FinalVolume;
                                                        tier2Found5 = true;
                                                        break;
                                                    }
                                                }
                                                if (!tier2Found5)
                                                {
                                                    foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 1))
                                                    {
                                                        SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                        if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                        {
                                                            sample.SampleAmount = prepSequence.SampleAmount;
                                                            sample.FinalVolume = prepSequence.FinalVolume;
                                                        }
                                                    }
                                                }
                                            }
                                            sample.Sort = index;
                                            sample.Runno = batchno;
                                            int tempindex = index;
                                            foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(a => a.Sort).ToList())
                                            {
                                                if (tempindex == sequence.Sort)
                                                {
                                                    sequence.Sort += 1;
                                                    tempindex = sequence.Sort;
                                                    //if (string.IsNullOrEmpty(sample.Dilution) || string.IsNullOrWhiteSpace(sample.Dilution))
                                                    //{
                                                    //    sample.Dilution = "1";
                                                    //}
                                                }
                                            }
                                            ((ListView)qclist.InnerView).CollectionSource.Add(sample);
                                            index++;
                                        }
                                    }
                                }
                            }
                        }

                        if (intIterations == 0 || (objLastSample != null && qCBatch.Oid == objLastSample.Oid))
                        //if (intIterations == 0)
                        {
                            if (sampleQCTestRuns != null && sampleQCTestRuns.Count > 0)
                            {
                                //List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && (x.uqQCTypeID.QCRootRole != QCRoleCN.平行 && x.uqQCTypeID.QCRootRole != QCRoleCN.加标)).OrderBy(a => a.Order).ToList();
                                //List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && x.uqQCTypeID.QCSource != null && x.uqQCTypeID.QCSource.QC_Source == "Sample" && (x.uqQCTypeID.QCRootRole.IsDuplicate == false && x.uqQCTypeID.QCRootRole.IsSpike == false)).OrderBy(a => a.Order).ToList();
                                List<SpreadSheetBuilder_SampleQCTestRun> lstTest = sampleQCTestRuns.Where(x => x.uqQCTypeID != null && x.HasSampleAsQCSource == true && (x.uqQCTypeID.QCRootRole.IsDuplicate == false && x.uqQCTypeID.QCRootRole.IsSpike == false)).OrderBy(a => a.Order).ToList();
                                if (lstTest != null && lstTest.Count > 0)
                                {
                                    index = qCBatch.Sort + 1;
                                    foreach (SpreadSheetBuilder_SampleQCTestRun testRun in lstTest)
                                    {
                                        int intRunNo = 1;
                                        QCType qcType = ((ListView)qclist.InnerView).ObjectSpace.GetObjectByKey<QCType>(testRun.uqQCTypeID.Oid);
                                        if (qcType != null)
                                        {
                                            if (((ListView)qclist.InnerView).CollectionSource != null && ((ListView)qclist.InnerView).CollectionSource.GetCount() > 0)
                                            {
                                                List<QCBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                                if (lstTestRuns != null && lstTestRuns.Count > 0)
                                                {
                                                    intRunNo = lstTestRuns.Count + 1;
                                                }
                                            }
                                            QCBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<QCBatchSequence>();
                                            sample.QCType = qcType;
                                            sample.SampleID = qCBatch.SampleID;
                                            sample.Dilution = "1";

                                            //sample.batchno = qCBatch.batchno;
                                            //if (qcType.QCSource != null)
                                            //{
                                            //    if (qcType.QCSource.QCTypeName == "Sample")
                                            //    {
                                            //        sample.StrSampleID = qCBatch.SampleID.SampleID;
                                            //        sample.SYSSamplecode = qCBatch.SampleID.SampleID + qcType.QCTypeName + batchno;
                                            //    }
                                            //    else if (qcType.QCSource.QCTypeName == "MS")
                                            //    {
                                            //        sample.StrSampleID = qcType.QCSource.QCTypeName;
                                            //        sample.SYSSamplecode = qCBatch.SampleID.SampleID + qcType.QCTypeName + batchno;
                                            //    }
                                            //    else
                                            //    {
                                            //        sample.StrSampleID = qcType.QCSource.QCTypeName;
                                            //        sample.SYSSamplecode = qcType.QCTypeName + batchno;
                                            //    }
                                            //}
                                            if (qcType.QCSource != null && qcType.QCSource.QC_Source == "Sample")
                                            {
                                                sample.SampleID = qclist.InnerView.ObjectSpace.GetObjectByKey<SampleLogIn>(qCBatch.SampleID.Oid);
                                                sample.SYSSamplecode = qCBatch.SampleID.SampleID + qcType.QCTypeName + batchno;
                                                //string MSsampleid = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == sample.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.StrSampleID).FirstOrDefault();
                                                //string MSSYSSamplecode = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == sample.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample" && a.Oid == qbslist.Oid).Select(i => i.SYSSamplecode).FirstOrDefault();
                                                sample.StrSampleID = sample.SampleID.SampleID;//MSSYSSamplecode.Replace(MSsampleid, "");
                                                IList<SamplePrepBatch> objlist = ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("[Jobid]=?", batch.Jobid));
                                                if (objlist != null)
                                                {
                                                    bool tier2Found6 = false;
                                                    foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 2))
                                                    {
                                                        SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                        if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                        {
                                                            sample.SampleAmount = prepSequence.SampleAmount;
                                                            sample.FinalVolume = prepSequence.FinalVolume;
                                                            tier2Found6 = true;
                                                            break;
                                                        }
                                                    }
                                                    if (!tier2Found6)
                                                    {
                                                        foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 1))
                                                        {
                                                            SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                            if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                            {
                                                                sample.SampleAmount = prepSequence.SampleAmount;
                                                                sample.FinalVolume = prepSequence.FinalVolume;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                            }


                                            sample.Sort = index;
                                            sample.Runno = batchno;
                                            int tempindex = index;
                                            foreach (QCBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(a => a.Sort).ToList())
                                            {
                                                if (tempindex == sequence.Sort)
                                                {
                                                    sequence.Sort += 1;
                                                    tempindex = sequence.Sort;
                                                    //if (string.IsNullOrEmpty(sample.Dilution) || string.IsNullOrWhiteSpace(sample.Dilution))
                                                    //{
                                                    //    sample.Dilution = "1";
                                                    //}
                                                }
                                            }
                                            ((ListView)qclist.InnerView).CollectionSource.Add(sample);
                                            index++;
                                        }
                                    }
                                }
                            }
                        }
                        /* reset cycle */
                        if (sequencePattern.NumberOfSamplesBetweenQCTest == intIterations)
                        {
                            batchno += 1;
                            intIterations = 0;
                        }
                    }

                    index = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Max(i => i.Sort) + 1;
                    //ClosingQC
                    if (sequencePattern.IsClosingQC == true && closingQCTestRuns != null && closingQCTestRuns.Count > 0)
                    {
                        foreach (SpreadSheetBuilder_ClosingQCTestRun testRun in closingQCTestRuns.OrderBy(a => a.Order).ToList())
                        {
                            int intRunNo = 1;
                            if (testRun.uqQCTypeID != null)
                            {
                                QCType qcType = ((ListView)qclist.InnerView).ObjectSpace.GetObjectByKey<QCType>(testRun.uqQCTypeID.Oid);
                                if (qcType != null)
                                {
                                    if (((ListView)qclist.InnerView).CollectionSource != null && ((ListView)qclist.InnerView).CollectionSource.GetCount() > 0)
                                    {
                                        List<QCBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                        if (lstTestRuns != null && lstTestRuns.Count > 0)
                                        {
                                            intRunNo = lstTestRuns.Count + 1;
                                        }
                                    }
                                    QCBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<QCBatchSequence>();
                                    sample.QCType = qcType;
                                    sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                    sample.SystemID = qcType.QCTypeName + intRunNo;
                                    sample.Sort = index;
                                    //sample.Runno = intRunNo;
                                    sample.Runno = batchno;
                                    sample.Dilution = "1";
                                    IList<SamplePrepBatch> objlist = ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("[Jobid]=?", batch.Jobid));
                                    if (objlist != null)
                                    {
                                        bool tier2Found7 = false;
                                        foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 2))
                                        {
                                            SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                            if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                            {
                                                sample.SampleAmount = prepSequence.SampleAmount;
                                                sample.FinalVolume = prepSequence.FinalVolume;
                                                tier2Found7 = true;
                                                break;
                                            }
                                        }
                                        if (!tier2Found7)
                                        {
                                            foreach (SamplePrepBatch obj in objlist.Where(j => j.Tier == 1))
                                            {
                                                SamplePrepBatchSequence prepSequence = ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ? AND [QCType.Oid] = ?  AND [SYSSamplecode] = ?", obj.Oid, qcType.Oid, sample.SYSSamplecode));
                                                if (prepSequence != null && prepSequence.SYSSamplecode == sample.SYSSamplecode)
                                                {
                                                    sample.SampleAmount = prepSequence.SampleAmount;
                                                    sample.FinalVolume = prepSequence.FinalVolume;
                                                }
                                            }
                                        }
                                    }
                                    ((ListView)qclist.InnerView).CollectionSource.Add(sample);
                                    if (qcType.QCSource != null && qcType.QCSource.QC_Source == "LCS")
                                    {
                                        Int32 iLCSCount = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(x => x.QCType.QCTypeName == "LCS").OrderBy(a => a.Sort).ToList().Count;
                                        if (iLCSCount > 0)
                                        {
                                            QCBatchSequence qblist = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(x => x.QCType.QCTypeName == "LCS").OrderBy(a => a.Sort).ToList()[iLCSCount - 1];
                                            sample.StrSampleID = qblist.SYSSamplecode;
                                        }
                                    }
                                    index++;
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

        private void BindQCBatchSequenceToDataTable(ListView qcbatchSeqInnerView)
        {
            try
            {
                DataTable dtsht1 = new DataTable { TableName = "RawDataTableDataSource" };
                dtsht1.Columns.Add("Sort");
                dtsht1.Columns.Add("UQSAMPLEPARAMETERID", typeof(Guid));
                dtsht1.Columns.Add("SAMPLEID");
                dtsht1.Columns.Add("SystemID");
                dtsht1.Columns.Add("SYSSamplecode");
                dtsht1.Columns.Add("PARAMETER");
                dtsht1.Columns.Add("UQTESTPARAMETERID", typeof(Guid));
                dtsht1.Columns.Add("RptLimit");
                dtsht1.Columns.Add("DefaultResult");
                dtsht1.Columns.Add("MDL");
                dtsht1.Columns.Add("ParameterSort");
                dtsht1.Columns.Add("Units");
                dtsht1.Columns.Add("Unit");
                dtsht1.Columns.Add("TEST");
                dtsht1.Columns.Add("METHOD");
                dtsht1.Columns.Add("METHODNAME");
                dtsht1.Columns.Add("MATRIX");
                dtsht1.Columns.Add("ClientSampleID");
                dtsht1.Columns.Add("COLLECTIONDATE", typeof(DateTime));
                dtsht1.Columns.Add("JOBID");
                dtsht1.Columns.Add("SAMPLENO");
                dtsht1.Columns.Add("UQSAMPLEID", typeof(Guid));
                dtsht1.Columns.Add("FLOWRATE");
                dtsht1.Columns.Add("Vol(Liters)");
                dtsht1.Columns.Add("Volume");
                dtsht1.Columns.Add("UQSAMPLEJOBID");
                dtsht1.Columns.Add("SAMPLENAME");
                dtsht1.Columns.Add("RECEIVEDDATE", typeof(DateTime));
                dtsht1.Columns.Add("ReportID");
                dtsht1.Columns.Add("ModelNumber");
                dtsht1.Columns.Add("REPORTINGMATRIX");
                dtsht1.Columns.Add("PROJECTID");
                dtsht1.Columns.Add("PROJECTNAME");
                dtsht1.Columns.Add("CLIENTNAME");
                dtsht1.Columns.Add("QCType");
                dtsht1.Columns.Add("RunType");
                dtsht1.Columns.Add("uqQCTypeID", typeof(Guid));
                dtsht1.Columns.Add("ENTEREDBY");
                dtsht1.Columns.Add("ENTEREDDATE", typeof(DateTime));
                dtsht1.Columns.Add("ANALYZEDBY");
                dtsht1.Columns.Add("ANALYZEDDATE", typeof(DateTime));
                dtsht1.Columns.Add("uqQCBatchID", typeof(Guid));
                dtsht1.Columns.Add("Humidity");
                dtsht1.Columns.Add("Temperature");
                dtsht1.Columns.Add("QCBatchID");
                dtsht1.Columns.Add("LabwareName1");
                dtsht1.Columns.Add("LabwareName2");
                dtsht1.Columns.Add("LabwareName3");
                dtsht1.Columns.Add("LabwareName4");
                dtsht1.Columns.Add("LabwareName5");
                dtsht1.Columns.Add("AssignedName1");
                dtsht1.Columns.Add("AssignedName2");
                dtsht1.Columns.Add("AssignedName3");
                dtsht1.Columns.Add("AssignedName4");
                dtsht1.Columns.Add("AssignedName5");
                dtsht1.Columns.Add("FileNumber1");
                dtsht1.Columns.Add("FileNumber2");
                dtsht1.Columns.Add("FileNumber3");
                dtsht1.Columns.Add("FileNumber4");
                dtsht1.Columns.Add("FileNumber5");
                dtsht1.Columns.Add("Specification1");
                dtsht1.Columns.Add("Specification2");
                dtsht1.Columns.Add("Specification3");
                dtsht1.Columns.Add("Specification4");
                dtsht1.Columns.Add("Specification5");
                dtsht1.Columns.Add("ExpirationDate1", typeof(DateTime));
                dtsht1.Columns.Add("ExpirationDate2", typeof(DateTime));
                dtsht1.Columns.Add("ExpirationDate3", typeof(DateTime));
                dtsht1.Columns.Add("ExpirationDate4", typeof(DateTime));
                dtsht1.Columns.Add("ExpirationDate5", typeof(DateTime));
                dtsht1.Columns.Add("SurrogateSort");
                dtsht1.Columns.Add("uqTestSurrogateId");
                dtsht1.Columns.Add("RunNo");
                dtsht1.Columns.Add("Instrument");
                dtsht1.Columns.Add("LabwareName");
                dtsht1.Columns.Add("SerialNumber");
                dtsht1.Columns.Add("SpikeAmount");
                dtsht1.Columns.Add("RecLCLimit");
                dtsht1.Columns.Add("RPDLCLimit");
                dtsht1.Columns.Add("%RECCTRLLIMIT");
                dtsht1.Columns.Add("%RDCTRLLIMIT");
                dtsht1.Columns.Add("[%RECULIMIT]");
                dtsht1.Columns.Add("[%RPDULIMIT]");
                dtsht1.Columns.Add("[%RECLLIMIT]");
                dtsht1.Columns.Add("[%RPDLLIMIT]");
                dtsht1.Columns.Add("DilutionCount");
                dtsht1.Columns.Add("Dilution");
                dtsht1.Columns.Add("ISReport");
                dtsht1.Columns.Add("SampleType");
                dtsht1.Columns.Add("Description");
                dtsht1.Columns.Add("SampleLocation");
                dtsht1.Columns.Add("SampleLayerID");
                dtsht1.Columns.Add("SampledBy");
                dtsht1.Columns.Add("ProjectLocation");
                dtsht1.Columns.Add("StartFlow");
                dtsht1.Columns.Add("EndFlow");
                dtsht1.Columns.Add("StartTime");
                dtsht1.Columns.Add("EndTime");
                dtsht1.Columns.Add("Time");
                dtsht1.Columns.Add("T(Min)");
                dtsht1.Columns.Add("Length");
                dtsht1.Columns.Add("Width");
                dtsht1.Columns.Add("AREA");
                dtsht1.Columns.Add("Material");
                dtsht1.Columns.Add("NAPositiveStop");
                dtsht1.Columns.Add("Friable");
                dtsht1.Columns.Add("Texture");
                dtsht1.Columns.Add("VisualGross");
                dtsht1.Columns.Add("Color");
                dtsht1.Columns.Add("NonAsbestosValue1");
                dtsht1.Columns.Add("NonAsbestosType1");
                dtsht1.Columns.Add("NonAsbestosValue2");
                dtsht1.Columns.Add("NonAsbestosType2");
                dtsht1.Columns.Add("NonAsbestosValue3");
                dtsht1.Columns.Add("NonAsbestosType3");
                dtsht1.Columns.Add("NonAsbestosValue4");
                dtsht1.Columns.Add("NonAsbestosType4");
                dtsht1.Columns.Add("PointCount");
                dtsht1.Columns.Add("AsbestosType1");
                dtsht1.Columns.Add("AsbestosValue1");
                dtsht1.Columns.Add("AsbestosType2");
                dtsht1.Columns.Add("AsbestosValue2");
                dtsht1.Columns.Add("AsbestosType3");
                dtsht1.Columns.Add("AsbestosValue3");
                dtsht1.Columns.Add("NonFibrousValue1");
                dtsht1.Columns.Add("NonFibrousType1");

                XPClassInfo sampleParameterinfo;
                IObjectSpace os = Application.CreateObjectSpace();
                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                sampleParameterinfo = uow.GetClassInfo(typeof(SpreadSheetEntry));
                string tableName = sampleParameterinfo.TableName;
                foreach (XPMemberInfo info in sampleParameterinfo.PersistentProperties)
                {
                    if (!dtsht1.Columns.Contains(info.MappingField))
                    {
                        dtsht1.Columns.Add(info.MappingField);
                    }
                }
                #region DBDeclaration
                bool DilIsReport = true;
                int strQcSort = 0;
                int strSampleNo = 0;
                decimal intFlowRate = 0;
                int intRunno = 0;
                int intuqTestSurrogateId = -1;
                int intSurrogateSort = 1;
                int strSort = 0;
                int intdilutioncnt = 0;
                int totaltime = 0;
                string strlength = string.Empty;
                string strwidth = string.Empty;
                string strarea = string.Empty;
                Nullable<DateTime> starttime = null;
                Nullable<DateTime> endtime = null;
                string strdilution = string.Empty;
                string strSampleID = string.Empty;
                string strSystemID = string.Empty;
                string strSYSSamplecode = string.Empty;
                string strParameter = string.Empty;
                string strRptLimit = string.Empty;
                string strDefaultResult = string.Empty;
                string strMDL = string.Empty;
                string strUnits = string.Empty;
                string strSLUnits = string.Empty;
                string strTestName = string.Empty;
                string strMethodNumber = string.Empty;
                string strMethodName = string.Empty;
                string strMatrixName = string.Empty;
                string strClientSampleID = string.Empty;
                string strJobID = string.Empty;
                //string strSCJobID = string.Empty;
                string strSampleName = string.Empty;
                string strReportID = string.Empty;
                string strModelNumber = string.Empty;
                string strVolume = string.Empty;
                string strQCTypeName = string.Empty;
                string strQCTypeSource = string.Empty;
                string strUserName = string.Empty;
                string strHumidity = string.Empty;
                string strRoomtemp = string.Empty;
                string strQCBatchID = string.Empty;
                string strClientName = string.Empty;
                string strProjectId = string.Empty;
                string strProjectName = string.Empty;
                string strVisualMatrixName = string.Empty;
                string strAnalyedby = string.Empty;
                string strAssignedName1 = string.Empty;
                string strLabwareName1 = string.Empty;
                string strFileNumber1 = string.Empty;
                string strSpecification1 = string.Empty;
                string strAssignedName2 = string.Empty;
                string strLabwareName2 = string.Empty;
                string strFileNumber2 = string.Empty;
                string strSpecification2 = string.Empty;
                string strAssignedName3 = string.Empty;
                string strLabwareName3 = string.Empty;
                string strFileNumber3 = string.Empty;
                string strSpecification3 = string.Empty;
                string strAssignedName4 = string.Empty;
                string strLabwareName4 = string.Empty;
                string strFileNumber4 = string.Empty;
                string strSpecification4 = string.Empty;
                string strAssignedName5 = string.Empty;
                string strLabwareName5 = string.Empty;
                string strFileNumber5 = string.Empty;
                string strSpecification5 = string.Empty;
                string strLabwareName = string.Empty;
                string strSerialNumber = string.Empty;
                string strAssignedName = string.Empty;
                string strSerialNumber1 = string.Empty;
                string strSerialNumber2 = string.Empty;
                string strSerialNumber3 = string.Empty;
                string strSerialNumber4 = string.Empty;
                string strSerialNumber5 = string.Empty;
                string strRecLCLimit = string.Empty;
                string strRECCTRLLIMIT = string.Empty;
                string strRDCTRLLIMIT = string.Empty;
                string strRECULIMIT = string.Empty;
                string strRPDULIMIT = string.Empty;
                string strRECLLIMIT = string.Empty;
                string strRPDLLIMIT = string.Empty;
                string strSampleType = string.Empty;
                string strDescription = string.Empty;
                string strSampleLocation = string.Empty;
                string strSampleLayerID = string.Empty;
                string strMaterial = string.Empty;
                string strNAPositiveStop = string.Empty;
                string strFriable = string.Empty;
                string strTexture = string.Empty;
                string strVisualGross = string.Empty;
                string strColor = string.Empty;
                string strNonAsbestosValue1 = string.Empty;
                string strNonAsbestosType1 = string.Empty;
                string strNonAsbestosValue2 = string.Empty;
                string strNonAsbestosType2 = string.Empty;
                string strNonAsbestosValue3 = string.Empty;
                string strNonAsbestosType3 = string.Empty;
                string strNonAsbestosValue4 = string.Empty;
                string strNonAsbestosType4 = string.Empty;
                string strPointCount = string.Empty;
                string strAsbestosType1 = string.Empty;
                string strAsbestosValue1 = string.Empty;
                string strAsbestosType2 = string.Empty;
                string strAsbestosValue2 = string.Empty;
                string strAsbestosType3 = string.Empty;
                string strAsbestosValue3 = string.Empty;
                string strNonFibrousValue1 = string.Empty;
                string strNonFibrousType1 = string.Empty;
                string strSampledBy = string.Empty;
                string strProjectLocation = string.Empty;
                string strStartFlow = string.Empty;
                string strEndFlow = string.Empty;
                string strTime = string.Empty;
                Nullable<double> strSpikeAmount = null;
                string strRPDLCLimit = string.Empty;
                Guid strUqTestparameterID = new Guid();
                Guid strUqQcTypeID = new Guid();
                Guid objuqQCBatchID = new Guid();
                Guid objUQSAMPLEID = new Guid();
                Guid objSCJobID = new Guid();
                Guid objUqSampleparameterID = new Guid();
                DateTime objCollected = new DateTime();
                DateTime objAnalyedDate = new DateTime();
                DateTime RecievedDate = new DateTime();
                DateTime objExpirationDate1 = new DateTime();
                DateTime objExpirationDate2 = new DateTime();
                DateTime objExpirationDate3 = new DateTime();
                DateTime objExpirationDate4 = new DateTime();
                DateTime objExpirationDate5 = new DateTime();
                Nullable<DateTime> dStartTime = new Nullable<DateTime>();
                Nullable<DateTime> dEndTime = new Nullable<DateTime>();
                #endregion
                int qcsort = 0;
                foreach (QCBatchSequence objQCBatchSequence in objABinfo.lstQCBatchSequence.OrderBy(a => a.Sort).ToList())
                {
                    intFlowRate = 0;
                    totaltime = 0;
                    strlength = string.Empty;
                    strwidth = string.Empty;
                    strarea = string.Empty;
                    strVolume = string.Empty;
                    starttime = null;
                    endtime = null;
                    strdilution = string.Empty;
                    strSampleID = string.Empty;
                    strSystemID = string.Empty;
                    strSYSSamplecode = string.Empty;
                    strParameter = string.Empty;
                    strRptLimit = string.Empty;
                    strDefaultResult = string.Empty;
                    strMDL = string.Empty;
                    strUnits = string.Empty;
                    strSLUnits = string.Empty;
                    strTestName = string.Empty;
                    strMethodNumber = string.Empty;
                    strMethodName = string.Empty;
                    strMatrixName = string.Empty;
                    strClientSampleID = string.Empty;
                    strJobID = string.Empty;
                    //string strSCJobID = string.Empty;
                    strSampleName = string.Empty;
                    strReportID = string.Empty;
                    strModelNumber = string.Empty;
                    strVolume = string.Empty;
                    strQCTypeSource = string.Empty;
                    strQCTypeName = string.Empty;
                    strUserName = string.Empty;
                    strHumidity = string.Empty;
                    strRoomtemp = string.Empty;
                    strQCBatchID = string.Empty;
                    strClientName = string.Empty;
                    strProjectId = string.Empty;
                    strProjectName = string.Empty;
                    strVisualMatrixName = string.Empty;
                    strAnalyedby = string.Empty;
                    strAssignedName1 = string.Empty;
                    strLabwareName1 = string.Empty;
                    strFileNumber1 = string.Empty;
                    strSpecification1 = string.Empty;
                    strAssignedName2 = string.Empty;
                    strLabwareName2 = string.Empty;
                    strFileNumber2 = string.Empty;
                    strSpecification2 = string.Empty;
                    strAssignedName3 = string.Empty;
                    strLabwareName3 = string.Empty;
                    strFileNumber3 = string.Empty;
                    strSpecification3 = string.Empty;
                    strAssignedName4 = string.Empty;
                    strLabwareName4 = string.Empty;
                    strFileNumber4 = string.Empty;
                    strSpecification4 = string.Empty;
                    strAssignedName5 = string.Empty;
                    strLabwareName5 = string.Empty;
                    strFileNumber5 = string.Empty;
                    strSpecification5 = string.Empty;
                    strLabwareName = string.Empty;
                    strSerialNumber = string.Empty;
                    strAssignedName = string.Empty;
                    strSerialNumber1 = string.Empty;
                    strSerialNumber2 = string.Empty;
                    strSerialNumber3 = string.Empty;
                    strSerialNumber4 = string.Empty;
                    strSerialNumber5 = string.Empty;
                    strRecLCLimit = string.Empty;
                    strRECCTRLLIMIT = string.Empty;
                    strRDCTRLLIMIT = string.Empty;
                    strRECULIMIT = string.Empty;
                    strRPDULIMIT = string.Empty;
                    strRECLLIMIT = string.Empty;
                    strRPDLLIMIT = string.Empty;
                    strSpikeAmount = null;
                    strSampleType = string.Empty;
                    strDescription = string.Empty;
                    strSampleLocation = string.Empty;
                    strSampledBy = string.Empty;
                    strProjectLocation = string.Empty;
                    strStartFlow = string.Empty;
                    strEndFlow = string.Empty;
                    strTime = string.Empty;
                    strMaterial = string.Empty;
                    strNAPositiveStop = string.Empty;
                    strFriable = string.Empty;
                    strTexture = string.Empty;
                    strVisualGross = string.Empty;
                    strColor = string.Empty;
                    strNonAsbestosValue1 = string.Empty;
                    strNonAsbestosType1 = string.Empty;
                    strNonAsbestosValue2 = string.Empty;
                    strNonAsbestosType2 = string.Empty;
                    strNonAsbestosValue3 = string.Empty;
                    strNonAsbestosType3 = string.Empty;
                    strNonAsbestosValue4 = string.Empty;
                    strNonAsbestosType4 = string.Empty;
                    strPointCount = string.Empty;
                    strAsbestosType1 = string.Empty;
                    strAsbestosValue1 = string.Empty;
                    strAsbestosType2 = string.Empty;
                    strAsbestosValue2 = string.Empty;
                    strAsbestosType3 = string.Empty;
                    strAsbestosValue3 = string.Empty;
                    strNonFibrousValue1 = string.Empty;
                    strNonFibrousType1 = string.Empty;


                    strSampleLayerID = string.Empty;
                    strRPDLCLimit = string.Empty;
                    strUqTestparameterID = new Guid();
                    strUqQcTypeID = new Guid();
                    objuqQCBatchID = new Guid();
                    objUQSAMPLEID = new Guid();
                    objSCJobID = new Guid();
                    if (objQCBatchSequence.QCType != null && objQCBatchSequence.QCType.QCTypeName != "Sample")
                    {
                        IList<Testparameter> testparams = qcbatchSeqInnerView.ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.Oid]=? and [TestMethod.Oid]=?", objQCBatchSequence.QCType.Oid, objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Test.Oid));
                        foreach (Testparameter objTP in testparams.ToList())
                        {
                            //newsample.QCBatchID = sequence;
                            ////                newsample.Testparameter = testparam;
                            ////                newsample.QCSort = sort;
                            ////                newsample.SignOff = true;
                            ////                sort++;
                            #region fields
                            if (objTP != null)
                            {
                                strQcSort = qcsort;
                                qcsort++;
                            }
                            //if (objTP != null)
                            //{
                            //    objUqSampleparameterID = objSP.Oid;
                            //}
                            DilIsReport = true;
                            if (objQCBatchSequence != null)
                            {
                                intdilutioncnt = 1;
                            }
                            if (objQCBatchSequence != null)
                            {
                                strdilution = objQCBatchSequence.Dilution;
                            }
                            if (objQCBatchSequence != null)
                            {
                                strSampleID = objQCBatchSequence.StrSampleID;
                            }
                            if (objQCBatchSequence != null)
                            {
                                if (objQCBatchSequence.SampleID != null)
                                {
                                    strDescription = objQCBatchSequence.SampleID.Description;
                                    strTime = objQCBatchSequence.SampleID.Time;
                                    if (objQCBatchSequence.SampleID.SampleType != null)
                                        strSampleType = objQCBatchSequence.SampleID.SampleType.SampleTypeName;
                                }
                            }
                            if (objQCBatchSequence != null && objQCBatchSequence.SampleID != null)
                            {
                                if (objQCBatchSequence.SampleID.SamplingLocation != null)
                                    strSampleLocation = objQCBatchSequence.SampleID.SamplingLocation;
                            }
                            if (objQCBatchSequence != null)
                            {
                                if (objQCBatchSequence.StrSampleL != null)
                                    strSampleLayerID = objQCBatchSequence.StrSampleL;
                            }
                            if (objQCBatchSequence != null)
                            {
                                strSystemID = objQCBatchSequence.SystemID;
                            }
                            if (objQCBatchSequence != null)
                            {
                                strSYSSamplecode = objQCBatchSequence.SYSSamplecode;
                            }
                            if (objTP.Parameter != null)
                            {
                                strParameter = objTP.Parameter.ParameterName;
                            }
                            if (objTP.Oid != null)
                            {
                                strUqTestparameterID = objTP.Oid;
                            }
                            if (objTP != null)
                            {
                                strRptLimit = objTP.RptLimit;
                            }
                            if (objTP != null)
                            {
                                strDefaultResult = objTP.DefaultResult;
                            }
                            if (objTP != null)
                            {
                                strMDL = objTP.MDL;
                            }
                            if (objTP != null)
                            {
                                strSort = objTP.Sort;
                            }
                            if (objTP.DefaultUnits != null)
                            {
                                strUnits = objTP.DefaultUnits.UnitName;
                            }
                            if (objTP != null && objTP.TestMethod != null)
                            {
                                strTestName = objTP.TestMethod.TestName;
                            }
                            if (objTP.TestMethod != null && objTP.TestMethod.MethodName != null)
                            {
                                strMethodNumber = objTP.TestMethod.MethodName.MethodNumber;
                            }
                            if (objTP.TestMethod != null && objTP.TestMethod.MethodName != null)
                            {
                                strMethodName = objTP.TestMethod.MethodName.MethodName;
                            }
                            if (objTP.TestMethod != null && objTP.TestMethod.MatrixName != null)
                            {
                                strMatrixName = objTP.TestMethod.MatrixName.MatrixName;
                            }
                            //if (objSP.Samplelogin != null)
                            //{
                            //    strClientSampleID = objSP.Samplelogin.ClientSampleID;
                            //}
                            //if (objSP.Samplelogin != null)
                            //{
                            //    objCollected = objSP.Samplelogin.CollectDate;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            //{
                            //    strJobID = objSP.Samplelogin.JobID.JobID;
                            //}
                            //if (objSP.Samplelogin != null)
                            //{
                            //    strSampleNo = objSP.Samplelogin.SampleNo;
                            //}
                            //if (objSP.Samplelogin != null)
                            //{
                            //    objUQSAMPLEID = objSP.Samplelogin.Oid;
                            //}
                            //if (objSP.Samplelogin != null)
                            //{
                            //    intFlowRate = objSP.Samplelogin.FlowRate;
                            //}
                            //if (objSP.Samplelogin != null)
                            //{
                            //    strVolume = objSP.Samplelogin.Volume;
                            //}
                            //if (objSP.Samplelogin != null)
                            //{
                            //    starttime = objSP.Samplelogin.TimeStart;
                            //}
                            //if (objSP.Samplelogin != null)
                            //{
                            //    endtime = objSP.Samplelogin.TimeEnd;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            //{
                            //    objSCJobID = objSP.Samplelogin.Oid;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            //{
                            //    strSampleName = objSP.Samplelogin.JobID.SampleName;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            //{
                            //    RecievedDate = objSP.Samplelogin.JobID.RecievedDate;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            //{
                            //    strReportID = objSP.Samplelogin.JobID.ReportID;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            //{
                            //    strModelNumber = objSP.Samplelogin.JobID.ModelNumber;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.VisualMatrix != null)
                            //{
                            //    strVisualMatrixName = objSP.Samplelogin.VisualMatrix.VisualMatrixName;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null && objSP.Samplelogin.JobID.ProjectID != null)
                            //{
                            //    strProjectId = objSP.Samplelogin.JobID.ProjectID.ProjectId;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null && objSP.Samplelogin.JobID.ProjectID != null)
                            //{
                            //    strProjectName = objSP.Samplelogin.JobID.ProjectID.ProjectName;
                            //}
                            //if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null && objSP.Samplelogin.JobID.ClientName != null)
                            //{
                            //    strClientName = objSP.Samplelogin.JobID.ClientName.CustomerName;
                            //}
                            if (objQCBatchSequence != null && objQCBatchSequence.QCType != null)
                            {
                                strQCTypeName = objQCBatchSequence.QCType.QCTypeName;
                            }
                            //if (objQCBatchSequence != null && objQCBatchSequence.QCType != null)
                            //{
                            //    strQCTypeSource = objQCBatchSequence.QCType.QCSource.;
                            //}
                            if (objQCBatchSequence != null && objQCBatchSequence.QCType != null)
                            {
                                strUqQcTypeID = objQCBatchSequence.QCType.Oid;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                strAnalyedby = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.CreatedBy.DisplayName;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                objAnalyedDate = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.CreatedDate;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                objuqQCBatchID = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Oid;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                strHumidity = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Humidity;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                strRoomtemp = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Roomtemp;
                            }
                            //if (objQCBatchSequence != null && objQCBatchSequence.qcseqdetail != null)
                            //{
                            //    strQCBatchID = objQCBatchSequence.qcseqdetail.QCBatchID;
                            //}
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                            {
                                //strQCBatchID = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument;
                                string[] paramsplit = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument.Split(';');
                                foreach (string arrInstrument in paramsplit)
                                {
                                    Guid objInstrument = new Guid(arrInstrument);
                                    Labware objlabware = ObjectSpace.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", objInstrument));
                                    if (paramsplit[0] == arrInstrument)
                                    {
                                        strLabwareName1 = objlabware.LabwareName;
                                        strAssignedName1 = objlabware.AssignedName;
                                        strFileNumber1 = objlabware.FileNumber;
                                        strSpecification1 = objlabware.Specification;
                                        objExpirationDate1 = objlabware.ExpirationDate;
                                        strSerialNumber1 = objlabware.SerialNumber;
                                    }
                                    else if (paramsplit[1] == arrInstrument)
                                    {
                                        strLabwareName2 = objlabware.LabwareName;
                                        strAssignedName2 = objlabware.AssignedName;
                                        strFileNumber2 = objlabware.FileNumber;
                                        strSpecification2 = objlabware.Specification;
                                        objExpirationDate2 = objlabware.ExpirationDate;
                                        strSerialNumber2 = objlabware.SerialNumber;
                                    }
                                    else if (paramsplit[2] == arrInstrument)
                                    {
                                        strLabwareName1 = objlabware.LabwareName;
                                        strAssignedName3 = objlabware.AssignedName;
                                        strFileNumber3 = objlabware.FileNumber;
                                        strSpecification3 = objlabware.Specification;
                                        objExpirationDate3 = objlabware.ExpirationDate;
                                        strSerialNumber3 = objlabware.SerialNumber;
                                    }
                                    else if (paramsplit[3] == arrInstrument)
                                    {
                                        strLabwareName4 = objlabware.LabwareName;
                                        strAssignedName4 = objlabware.AssignedName;
                                        strFileNumber4 = objlabware.FileNumber;
                                        strSpecification4 = objlabware.Specification;
                                        objExpirationDate4 = objlabware.ExpirationDate;
                                        strSerialNumber4 = objlabware.SerialNumber;
                                    }
                                    else if (paramsplit[4] == arrInstrument)
                                    {
                                        strLabwareName5 = objlabware.LabwareName;
                                        strAssignedName5 = objlabware.AssignedName;
                                        strFileNumber5 = objlabware.FileNumber;
                                        strSpecification5 = objlabware.Specification;
                                        objExpirationDate5 = objlabware.ExpirationDate;
                                        strSerialNumber5 = objlabware.SerialNumber;
                                    }
                                }
                            }
                            if (objQCBatchSequence != null)
                            {
                                intRunno = objQCBatchSequence.Runno;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                            {
                                strLabwareName = strLabwareName1 + (string.IsNullOrEmpty(strLabwareName2) ? "" : ";" + strLabwareName2 + (string.IsNullOrEmpty(strLabwareName3) ? "" : ";" + strLabwareName3 + (string.IsNullOrEmpty(strLabwareName4) ? "" : ";" + strLabwareName4 + (string.IsNullOrEmpty(strLabwareName5) ? "" : ";" + strLabwareName5))));
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                            {
                                strAssignedName = strAssignedName1 + (string.IsNullOrEmpty(strAssignedName2) ? "" : ";" + strAssignedName2 + (string.IsNullOrEmpty(strAssignedName3) ? "" : ";" + strAssignedName3 + (string.IsNullOrEmpty(strLabwareName4) ? "" : ";" + strAssignedName4 + (string.IsNullOrEmpty(strAssignedName5) ? "" : ";" + strAssignedName5))));
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                            {
                                strSerialNumber = strSerialNumber1 + (string.IsNullOrEmpty(strSerialNumber2) ? "" : ";" + strSerialNumber2 + (string.IsNullOrEmpty(strSerialNumber3) ? "" : ";" + strSerialNumber3 + (string.IsNullOrEmpty(strSerialNumber4) ? "" : ";" + strSerialNumber4 + (string.IsNullOrEmpty(strSerialNumber5) ? "" : ";" + strSerialNumber5))));
                            }
                            if (objTP != null)
                            {
                                if (objTP.SpikeAmount == 0)
                                {
                                    strSpikeAmount = null;
                                }
                                else
                                {
                                    strSpikeAmount = objTP.SpikeAmount;
                                }
                            }
                            if (objTP != null)
                            {
                                strRecLCLimit = objTP.RecLCLimit;
                            }
                            if (objTP != null)
                            {
                                strRPDLCLimit = objTP.RPDLCLimit;
                            }
                            if (objTP != null && objTP.RecLCLimit != null)
                            {
                                strRECCTRLLIMIT = objTP.RecLCLimit + "-" + objTP.RecHCLimit;
                            }
                            if (objTP != null && objTP.RPDLCLimit != null)
                            {
                                strRDCTRLLIMIT = objTP.RPDLCLimit + "-" + objTP.RPDHCLimit;
                            }
                            if (objTP != null)
                            {
                                strRECULIMIT = objTP.RecHCLimit;
                            }
                            if (objTP != null)
                            {
                                strRPDULIMIT = objTP.RPDHCLimit;
                            }
                            if (objTP != null)
                            {
                                strRECLLIMIT = objTP.RecLCLimit;
                            }
                            if (objTP != null)
                            {
                                strRPDLLIMIT = objTP.RPDLCLimit;
                            }
                            #endregion
                            dtsht1.Rows.Add(strQcSort, objUqSampleparameterID, strSampleID, strSystemID, strSYSSamplecode, strParameter, strUqTestparameterID, strRptLimit, strDefaultResult,
                                strMDL, strSort, strUnits, strSLUnits, strTestName, strMethodNumber, strMethodName, strMatrixName, strClientSampleID, objCollected, strJobID, strSampleNo, objUQSAMPLEID,
                                intFlowRate, strVolume, strVolume, objSCJobID, strSampleName, RecievedDate, strReportID, strModelNumber, strVisualMatrixName, strProjectId, strProjectName, strClientName,
                                strQCTypeName, strQCTypeName, strUqQcTypeID, strAnalyedby, objAnalyedDate, strAnalyedby, objAnalyedDate, objuqQCBatchID, strHumidity, strRoomtemp, strQCBatchID, strLabwareName1,
                                strLabwareName2, strLabwareName3, strLabwareName4, strLabwareName5, strAssignedName1, strAssignedName2, strAssignedName3, strAssignedName4, strAssignedName5, strFileNumber1,
                                strFileNumber2, strFileNumber3, strFileNumber4, strFileNumber5, strSpecification1, strSpecification2, strSpecification3, strSpecification4, strSpecification5, objExpirationDate1,
                                objExpirationDate2, objExpirationDate3, objExpirationDate4, objExpirationDate5, intuqTestSurrogateId, intSurrogateSort, intRunno, strAssignedName, strLabwareName, strSerialNumber,
                                strSpikeAmount, strRecLCLimit, strRPDLCLimit, strRECCTRLLIMIT, strRDCTRLLIMIT, strRECULIMIT, strRPDULIMIT, strRECLLIMIT, strRPDLLIMIT, intdilutioncnt, strdilution, DilIsReport, strSampleType, strDescription, strSampleLocation, strSampleLayerID, strSampledBy, strProjectLocation, strStartFlow, strEndFlow, starttime, endtime, strTime, totaltime, strlength, strwidth, strarea);
                            strRECCTRLLIMIT = "";
                            strRDCTRLLIMIT = "";
                            strUnits = string.Empty;
                            strSLUnits = string.Empty;
                        }
                    }
                    else
                    {
                        if (objQCBatchSequence != null && !string.IsNullOrEmpty(objQCBatchSequence.SYSSamplecode))
                        {
                            int intdilcnt = 1;
                            string[] strarrsyssmpl = objQCBatchSequence.SYSSamplecode.Split('R');
                            if (strarrsyssmpl.Length > 1)
                            {
                                DilIsReport = false;
                                string strdilcnt = strarrsyssmpl[1].Replace("R", "");
                                strdilcnt = strdilcnt.Trim();
                                intdilcnt = Convert.ToInt32(strdilcnt) + 1;
                            }
                            else
                            {
                                DilIsReport = true;
                            }
                            intdilutioncnt = intdilcnt;
                            strdilution = objQCBatchSequence.Dilution;
                        }
                        IList<SampleParameter> samples = qcbatchSeqInnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin] = ? AND [Testparameter.TestMethod.Oid] = ? And ([Testparameter.TestMethod.IsFieldTest] Is Null Or [Testparameter.TestMethod.IsFieldTest] = False)", objQCBatchSequence.SampleID, objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Test.Oid));
                        foreach (SampleParameter objSP in samples)
                        {
                            #region fields

                            if (objSP != null)
                            {
                                strQcSort = qcsort;
                                qcsort++;
                            }
                            if (objSP != null)
                            {
                                objUqSampleparameterID = objSP.Oid;
                            }
                            if (objQCBatchSequence != null)
                            {
                                strSampleID = objQCBatchSequence.StrSampleID;
                            }
                            if (objQCBatchSequence != null)
                            {
                                if (objQCBatchSequence.SampleID != null)
                                {
                                    strDescription = objQCBatchSequence.SampleID.Description;
                                    strTime = objQCBatchSequence.SampleID.Time;
                                    if (objQCBatchSequence.SampleID.SamplingLocation != null)
                                    {
                                        strSampleLocation = objQCBatchSequence.SampleID.SamplingLocation;
                                    }
                                    if (objQCBatchSequence.SampleID.SampleType != null)
                                    {
                                        strSampleType = objQCBatchSequence.SampleID.SampleType.SampleTypeName;
                                    }
                                }
                            }
                            if (objQCBatchSequence != null)
                            {
                                if (objQCBatchSequence.StrSampleL != null)
                                    strSampleLayerID = objQCBatchSequence.StrSampleL;
                            }
                            if (objQCBatchSequence != null)
                            {
                                strSystemID = objQCBatchSequence.SystemID;
                            }
                            if (objQCBatchSequence != null)
                            {
                                strSYSSamplecode = objQCBatchSequence.SYSSamplecode;
                            }
                            if (objSP.Testparameter != null && objSP.Testparameter.Parameter != null)
                            {
                                strParameter = objSP.Testparameter.Parameter.ParameterName;
                            }
                            if (objSP.Testparameter != null)
                            {
                                strUqTestparameterID = objSP.Testparameter.Oid;
                            }
                            if (objSP.Testparameter != null)
                            {
                                strRptLimit = objSP.Testparameter.RptLimit;
                            }
                            if (objSP.Testparameter != null)
                            {
                                strDefaultResult = objSP.Testparameter.DefaultResult;
                            }
                            if (objSP.Testparameter != null)
                            {
                                strMDL = objSP.Testparameter.MDL;
                            }
                            if (objSP.Testparameter != null)
                            {
                                strSort = objSP.Testparameter.Sort;
                            }
                            if (objSP.Testparameter != null && objSP.Testparameter.DefaultUnits != null)
                            {
                                strUnits = objSP.Testparameter.DefaultUnits.UnitName;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.TtimeUnit != null)
                            {
                                strSLUnits = objSP.Samplelogin.TtimeUnit.UnitName;
                            }
                            if (objSP.Testparameter != null && objSP.Testparameter.TestMethod != null)
                            {
                                strTestName = objSP.Testparameter.TestMethod.TestName;
                            }
                            if (objSP.Testparameter != null && objSP.Testparameter.TestMethod != null && objSP.Testparameter.TestMethod.MethodName != null)
                            {
                                strMethodNumber = objSP.Testparameter.TestMethod.MethodName.MethodNumber;
                            }
                            if (objSP.Testparameter != null && objSP.Testparameter.TestMethod != null && objSP.Testparameter.TestMethod.MethodName != null)
                            {
                                strMethodName = objSP.Testparameter.TestMethod.MethodName.MethodName;
                            }
                            if (objSP.Testparameter != null && objSP.Testparameter.TestMethod != null && objSP.Testparameter.TestMethod.MatrixName != null)
                            {
                                strMatrixName = objSP.Testparameter.TestMethod.MatrixName.MatrixName;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                strClientSampleID = objSP.Samplelogin.ClientSampleID;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.TtimeUnit != null)
                            {
                                strSLUnits = objSP.Samplelogin.TtimeUnit.UnitName;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                objCollected = objSP.Samplelogin.CollectDate;
                                strStartFlow = objSP.Samplelogin.StartFlow;
                                strEndFlow = objSP.Samplelogin.EndFlow;
                                dStartTime = objSP.Samplelogin.TimeStart;
                                dEndTime = objSP.Samplelogin.TimeEnd;
                                if (objSP.Samplelogin.Collector != null)
                                {
                                    strSampledBy = objSP.Samplelogin.Collector.FirstName;
                                }
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            {
                                strJobID = objSP.Samplelogin.JobID.JobID;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                strSampleNo = objSP.Samplelogin.SampleNo;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                objUQSAMPLEID = objSP.Samplelogin.Oid;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                intFlowRate = Convert.ToDecimal(objSP.Samplelogin.FlowRate);
                            }
                            if (objSP.Samplelogin != null)
                            {
                                totaltime = Convert.ToInt32(objSP.Samplelogin.Time);
                            }
                            if (objSP.Samplelogin != null)
                            {
                                strlength = objSP.Samplelogin.Length;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                strwidth = objSP.Samplelogin.Width;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                strarea = objSP.Samplelogin.ServiceArea;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                strVolume = objSP.Samplelogin.Volume;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                starttime = objSP.Samplelogin.TimeStart;
                            }
                            if (objSP.Samplelogin != null)
                            {
                                endtime = objSP.Samplelogin.TimeEnd;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            {
                                objSCJobID = objSP.Samplelogin.Oid;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            {
                                strSampleName = objSP.Samplelogin.JobID.SampleName;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            {
                                RecievedDate = objSP.Samplelogin.JobID.RecievedDate;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            {
                                strReportID = objSP.Samplelogin.JobID.ReportID;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null)
                            {
                                strModelNumber = objSP.Samplelogin.JobID.ModelNumber;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.VisualMatrix != null)
                            {
                                strVisualMatrixName = objSP.Samplelogin.VisualMatrix.VisualMatrixName;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null && objSP.Samplelogin.JobID.ProjectID != null)
                            {
                                strProjectId = objSP.Samplelogin.JobID.ProjectID.ProjectId;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null && objSP.Samplelogin.JobID.ProjectID != null)
                            {
                                strProjectName = objSP.Samplelogin.JobID.ProjectID.ProjectName;
                                strProjectLocation = objSP.Samplelogin.JobID.ProjectID.ProjectLocation;
                            }
                            if (objSP.Samplelogin != null && objSP.Samplelogin.JobID != null && objSP.Samplelogin.JobID.ClientName != null)
                            {
                                strClientName = objSP.Samplelogin.JobID.ClientName.CustomerName;
                            }
                            if (objQCBatchSequence != null && objQCBatchSequence.QCType != null)
                            {
                                strQCTypeName = objQCBatchSequence.QCType.QCTypeName;
                            }
                            if (objQCBatchSequence != null && objQCBatchSequence.QCType != null)
                            {
                                strUqQcTypeID = objQCBatchSequence.QCType.Oid;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                strAnalyedby = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.CreatedBy.DisplayName;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                objAnalyedDate = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.CreatedDate;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                objuqQCBatchID = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Oid;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                strHumidity = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Humidity;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                            {
                                strRoomtemp = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Roomtemp;
                            }
                            //if (objQCBatchSequence != null && objQCBatchSequence.qcseqdetail != null)
                            //{
                            //    strQCBatchID = objQCBatchSequence.qcseqdetail.QCBatchID;
                            //}
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                            {
                                //strQCBatchID = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument;
                                string[] paramsplit = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument.Split(';');
                                foreach (string arrInstrument in paramsplit)
                                {
                                    Guid objInstrument = new Guid(arrInstrument);
                                    Labware objlabware = ObjectSpace.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", objInstrument));
                                    if (paramsplit[0] == arrInstrument)
                                    {
                                        strLabwareName1 = objlabware.LabwareName;
                                        strAssignedName1 = objlabware.AssignedName;
                                        strFileNumber1 = objlabware.FileNumber;
                                        strSpecification1 = objlabware.Specification;
                                        objExpirationDate1 = objlabware.ExpirationDate;
                                        strSerialNumber1 = objlabware.SerialNumber;
                                    }
                                    else if (paramsplit[1] == arrInstrument)
                                    {
                                        strLabwareName2 = objlabware.LabwareName;
                                        strAssignedName2 = objlabware.AssignedName;
                                        strFileNumber2 = objlabware.FileNumber;
                                        strSpecification2 = objlabware.Specification;
                                        objExpirationDate2 = objlabware.ExpirationDate;
                                        strSerialNumber2 = objlabware.SerialNumber;
                                    }
                                    else if (paramsplit[2] == arrInstrument)
                                    {
                                        strLabwareName1 = objlabware.LabwareName;
                                        strAssignedName3 = objlabware.AssignedName;
                                        strFileNumber3 = objlabware.FileNumber;
                                        strSpecification3 = objlabware.Specification;
                                        objExpirationDate3 = objlabware.ExpirationDate;
                                        strSerialNumber3 = objlabware.SerialNumber;
                                    }
                                    else if (paramsplit[3] == arrInstrument)
                                    {
                                        strLabwareName4 = objlabware.LabwareName;
                                        strAssignedName4 = objlabware.AssignedName;
                                        strFileNumber4 = objlabware.FileNumber;
                                        strSpecification4 = objlabware.Specification;
                                        objExpirationDate4 = objlabware.ExpirationDate;
                                        strSerialNumber4 = objlabware.SerialNumber;
                                    }
                                    else if (paramsplit[4] == arrInstrument)
                                    {
                                        strLabwareName5 = objlabware.LabwareName;
                                        strAssignedName5 = objlabware.AssignedName;
                                        strFileNumber5 = objlabware.FileNumber;
                                        strSpecification5 = objlabware.Specification;
                                        objExpirationDate5 = objlabware.ExpirationDate;
                                        strSerialNumber5 = objlabware.SerialNumber;
                                    }
                                }
                            }
                            if (objQCBatchSequence != null)
                            {
                                intRunno = objQCBatchSequence.Runno;
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                            {
                                strLabwareName = strLabwareName1 + (string.IsNullOrEmpty(strLabwareName2) ? "" : ";" + strLabwareName2 + (string.IsNullOrEmpty(strLabwareName3) ? "" : ";" + strLabwareName3 + (string.IsNullOrEmpty(strLabwareName4) ? "" : ";" + strLabwareName4 + (string.IsNullOrEmpty(strLabwareName5) ? "" : ";" + strLabwareName5))));
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                            {
                                strAssignedName = strAssignedName1 + (string.IsNullOrEmpty(strAssignedName2) ? "" : ";" + strAssignedName2 + (string.IsNullOrEmpty(strAssignedName3) ? "" : ";" + strAssignedName3 + (string.IsNullOrEmpty(strLabwareName4) ? "" : ";" + strAssignedName4 + (string.IsNullOrEmpty(strAssignedName5) ? "" : ";" + strAssignedName5))));
                            }
                            if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                            {
                                strSerialNumber = strSerialNumber1 + (string.IsNullOrEmpty(strSerialNumber2) ? "" : ";" + strSerialNumber2 + (string.IsNullOrEmpty(strSerialNumber3) ? "" : ";" + strSerialNumber3 + (string.IsNullOrEmpty(strSerialNumber4) ? "" : ";" + strSerialNumber4 + (string.IsNullOrEmpty(strSerialNumber5) ? "" : ";" + strSerialNumber5))));
                            }
                            if (objSP != null)
                            {
                                if (objSP.SpikeAmount == 0)
                                {
                                    strSpikeAmount = null;
                                }
                                else
                                {
                                    strSpikeAmount = objSP.SpikeAmount;
                                }
                            }
                            if (objSP != null)
                            {
                                strRecLCLimit = objSP.RecLCLimit;
                            }
                            if (objSP != null)
                            {
                                strRPDLCLimit = objSP.RPDLCLimit;
                            }
                            if (objSP != null && objSP.Testparameter != null && objSP.Testparameter.RecLCLimit != null)
                            {
                                strRECCTRLLIMIT = objSP.Testparameter.RecLCLimit + "-" + objSP.Testparameter.RecHCLimit;
                            }
                            if (objSP != null && objSP.Testparameter != null && objSP.Testparameter.RPDLCLimit != null)
                            {
                                strRDCTRLLIMIT = objSP.Testparameter.RPDLCLimit + "-" + objSP.Testparameter.RPDHCLimit;
                            }
                            if (objSP != null && objSP.Testparameter != null)
                            {
                                strRECULIMIT = objSP.Testparameter.RecHCLimit;
                            }
                            if (objSP != null && objSP.Testparameter != null)
                            {
                                strRPDULIMIT = objSP.Testparameter.RPDHCLimit;
                            }
                            if (objSP != null && objSP.Testparameter != null)
                            {
                                strRECLLIMIT = objSP.Testparameter.RecLCLimit;
                            }
                            if (objSP != null && objSP.Testparameter != null)
                            {
                                strRPDLLIMIT = objSP.Testparameter.RPDLCLimit;
                            }
                            #endregion
                            dtsht1.Rows.Add(strQcSort, objUqSampleparameterID, strSampleID, strSystemID, strSYSSamplecode, strParameter, strUqTestparameterID, strRptLimit, strDefaultResult,
                                strMDL, strSort, strUnits, strSLUnits, strTestName, strMethodNumber, strMethodName, strMatrixName, strClientSampleID, objCollected, strJobID, strSampleNo, objUQSAMPLEID,
                                intFlowRate, strVolume, strVolume, objSCJobID, strSampleName, RecievedDate, strReportID, strModelNumber, strVisualMatrixName, strProjectId, strProjectName, strClientName,
                                strQCTypeName, strQCTypeName, strUqQcTypeID, strAnalyedby, objAnalyedDate, strAnalyedby, objAnalyedDate, objuqQCBatchID, strHumidity, strRoomtemp, strQCBatchID, strLabwareName1,
                                strLabwareName2, strLabwareName3, strLabwareName4, strLabwareName5, strAssignedName1, strAssignedName2, strAssignedName3, strAssignedName4, strAssignedName5, strFileNumber1,
                                strFileNumber2, strFileNumber3, strFileNumber4, strFileNumber5, strSpecification1, strSpecification2, strSpecification3, strSpecification4, strSpecification5, objExpirationDate1,
                                objExpirationDate2, objExpirationDate3, objExpirationDate4, objExpirationDate5, intuqTestSurrogateId, intSurrogateSort, intRunno, strAssignedName, strLabwareName, strSerialNumber,
                                strSpikeAmount, strRecLCLimit, strRPDLCLimit, strRECCTRLLIMIT, strRDCTRLLIMIT, strRECULIMIT, strRPDULIMIT, strRECLLIMIT, strRPDLLIMIT, intdilutioncnt, strdilution, DilIsReport, strSampleType, strDescription, strSampleLocation, strSampleLayerID, strSampledBy, strProjectLocation,
                                                strStartFlow, strEndFlow, starttime, endtime, strTime, totaltime, strlength, strwidth, strarea);
                            strRECCTRLLIMIT = "";
                            strRDCTRLLIMIT = "";
                            strSLUnits = string.Empty;
                        }
                    }
                    //Surrogates
                    IList<Testparameter> lstSurrogates = qcbatchSeqInnerView.ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[QCType.Oid] IS NULL AND [Surroagate] > 0 AND [TestMethod.Oid]=?", objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Test.Oid));
                    if (lstSurrogates != null && lstSurrogates.Count > 0)
                    {
                        foreach (Testparameter objTestSurrogate in lstSurrogates.ToList())
                        {
                            if (objTestSurrogate != null)
                            {
                                #region Surrogate Fields
                                intuqTestSurrogateId = 1;
                                strQcSort = qcsort;
                                qcsort++;
                                DilIsReport = true;

                                if (objQCBatchSequence != null)
                                {
                                    intdilutioncnt = 1;
                                    strdilution = objQCBatchSequence.Dilution;
                                    strSampleID = objQCBatchSequence.StrSampleID;

                                    if (objQCBatchSequence.SampleID != null)
                                    {
                                        strTime = objQCBatchSequence.SampleID.Time;
                                        strDescription = objQCBatchSequence.SampleID.Description;
                                        if (objQCBatchSequence.SampleID.SampleType != null)
                                        {
                                            strSampleType = objQCBatchSequence.SampleID.SampleType.SampleTypeName;
                                        }
                                        if (objQCBatchSequence.SampleID.SamplingLocation != null)
                                        {
                                            strSampleLocation = objQCBatchSequence.SampleID.SamplingLocation;
                                        }
                                    }
                                    if (objQCBatchSequence.StrSampleL != null)
                                    {
                                        strSampleLayerID = objQCBatchSequence.StrSampleL;
                                    }
                                    if (objQCBatchSequence.SampleID != null)
                                    {
                                        strSampleLocation = objQCBatchSequence.SampleID.SamplingLocation;
                                    }
                                    strSystemID = objQCBatchSequence.SystemID;
                                    strSYSSamplecode = objQCBatchSequence.SYSSamplecode;

                                    if (objQCBatchSequence.QCType != null)
                                    {
                                        strQCTypeName = objQCBatchSequence.QCType.QCTypeName;
                                        strUqQcTypeID = objQCBatchSequence.QCType.Oid;
                                    }


                                    strAnalyedby = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.CreatedBy.DisplayName;
                                    objAnalyedDate = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.CreatedDate;
                                }
                                if (objTestSurrogate != null)
                                {
                                    strParameter = objTestSurrogate.Parameter.ParameterName;
                                    strUqTestparameterID = objTestSurrogate.Oid;
                                    strRptLimit = objTestSurrogate.RptLimit;
                                    strDefaultResult = objTestSurrogate.DefaultResult;
                                    strMDL = objTestSurrogate.MDL;
                                    strSort = objTestSurrogate.Sort;
                                    if (objTestSurrogate.SpikeAmount == 0)
                                    {
                                        strSpikeAmount = null;
                                    }
                                    else
                                    {
                                        strSpikeAmount = objTestSurrogate.SpikeAmount;
                                    }
                                    strRecLCLimit = objTestSurrogate.RecLCLimit;
                                    strRPDLCLimit = objTestSurrogate.RPDLCLimit;
                                    strRECULIMIT = objTestSurrogate.RecHCLimit;
                                    strRPDULIMIT = objTestSurrogate.RPDHCLimit;
                                    strRECLLIMIT = objTestSurrogate.RecLCLimit;
                                    strRPDLLIMIT = objTestSurrogate.RPDLCLimit;
                                    if (objTestSurrogate != null && objTestSurrogate.RecLCLimit != null)
                                    {
                                        strRECCTRLLIMIT = objTestSurrogate.RecLCLimit + "-" + objTestSurrogate.RecHCLimit;
                                    }
                                    if (objTestSurrogate != null && objTestSurrogate.RPDLCLimit != null)
                                    {
                                        strRDCTRLLIMIT = objTestSurrogate.RPDLCLimit + "-" + objTestSurrogate.RPDHCLimit;
                                    }
                                    if (objTestSurrogate.DefaultUnits != null)
                                    {
                                        strUnits = objTestSurrogate.DefaultUnits.UnitName;
                                    }
                                    if (objTestSurrogate.TestMethod != null)
                                    {
                                        strTestName = objTestSurrogate.TestMethod.TestName;
                                        strMethodNumber = objTestSurrogate.TestMethod.MethodName.MethodNumber;
                                        strMethodName = objTestSurrogate.TestMethod.MethodName.MethodName;
                                        strMatrixName = objTestSurrogate.TestMethod.MatrixName.MatrixName;
                                    }
                                }

                                if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null)
                                {
                                    objuqQCBatchID = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Oid;
                                    strRoomtemp = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Roomtemp;
                                    strHumidity = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Humidity;
                                }

                                if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                                {

                                    string[] paramsplit = objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument.Split(';');
                                    foreach (string arrInstrument in paramsplit)
                                    {
                                        Guid objInstrument = new Guid(arrInstrument);
                                        Labware objlabware = ObjectSpace.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", objInstrument));
                                        if (paramsplit[0] == arrInstrument)
                                        {
                                            strLabwareName1 = objlabware.LabwareName;
                                            strAssignedName1 = objlabware.AssignedName;
                                            strFileNumber1 = objlabware.FileNumber;
                                            strSpecification1 = objlabware.Specification;
                                            objExpirationDate1 = objlabware.ExpirationDate;
                                            strSerialNumber1 = objlabware.SerialNumber;
                                        }
                                        else if (paramsplit[1] == arrInstrument)
                                        {
                                            strLabwareName2 = objlabware.LabwareName;
                                            strAssignedName2 = objlabware.AssignedName;
                                            strFileNumber2 = objlabware.FileNumber;
                                            strSpecification2 = objlabware.Specification;
                                            objExpirationDate2 = objlabware.ExpirationDate;
                                            strSerialNumber2 = objlabware.SerialNumber;
                                        }
                                        else if (paramsplit[2] == arrInstrument)
                                        {
                                            strLabwareName1 = objlabware.LabwareName;
                                            strAssignedName3 = objlabware.AssignedName;
                                            strFileNumber3 = objlabware.FileNumber;
                                            strSpecification3 = objlabware.Specification;
                                            objExpirationDate3 = objlabware.ExpirationDate;
                                            strSerialNumber3 = objlabware.SerialNumber;
                                        }
                                        else if (paramsplit[3] == arrInstrument)
                                        {
                                            strLabwareName4 = objlabware.LabwareName;
                                            strAssignedName4 = objlabware.AssignedName;
                                            strFileNumber4 = objlabware.FileNumber;
                                            strSpecification4 = objlabware.Specification;
                                            objExpirationDate4 = objlabware.ExpirationDate;
                                            strSerialNumber4 = objlabware.SerialNumber;
                                        }
                                        else if (paramsplit[4] == arrInstrument)
                                        {
                                            strLabwareName5 = objlabware.LabwareName;
                                            strAssignedName5 = objlabware.AssignedName;
                                            strFileNumber5 = objlabware.FileNumber;
                                            strSpecification5 = objlabware.Specification;
                                            objExpirationDate5 = objlabware.ExpirationDate;
                                            strSerialNumber5 = objlabware.SerialNumber;
                                        }
                                    }
                                }
                                if (objQCBatchSequence != null)
                                {
                                    intRunno = objQCBatchSequence.Runno;
                                }
                                if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                                {
                                    strLabwareName = strLabwareName1 + (string.IsNullOrEmpty(strLabwareName2) ? "" : ";" + strLabwareName2 + (string.IsNullOrEmpty(strLabwareName3) ? "" : ";" + strLabwareName3 + (string.IsNullOrEmpty(strLabwareName4) ? "" : ";" + strLabwareName4 + (string.IsNullOrEmpty(strLabwareName5) ? "" : ";" + strLabwareName5))));
                                }
                                if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                                {
                                    strAssignedName = strAssignedName1 + (string.IsNullOrEmpty(strAssignedName2) ? "" : ";" + strAssignedName2 + (string.IsNullOrEmpty(strAssignedName3) ? "" : ";" + strAssignedName3 + (string.IsNullOrEmpty(strLabwareName4) ? "" : ";" + strAssignedName4 + (string.IsNullOrEmpty(strAssignedName5) ? "" : ";" + strAssignedName5))));
                                }
                                if (objABinfo != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch != null && objABinfo.lstSpreadSheetEntry_AnalyticalBatch.Instrument != null)
                                {
                                    strSerialNumber = strSerialNumber1 + (string.IsNullOrEmpty(strSerialNumber2) ? "" : ";" + strSerialNumber2 + (string.IsNullOrEmpty(strSerialNumber3) ? "" : ";" + strSerialNumber3 + (string.IsNullOrEmpty(strSerialNumber4) ? "" : ";" + strSerialNumber4 + (string.IsNullOrEmpty(strSerialNumber5) ? "" : ";" + strSerialNumber5))));
                                }
                                #endregion

                                dtsht1.Rows.Add(strQcSort, objUqSampleparameterID, strSampleID, strSystemID, strSYSSamplecode, strParameter, strUqTestparameterID, strRptLimit, strDefaultResult,
                                                strMDL, strSort, strUnits, strSLUnits, strTestName, strMethodNumber, strMethodName, strMatrixName, strClientSampleID, objCollected, strJobID, strSampleNo, objUQSAMPLEID,
                                                intFlowRate, strVolume, strVolume, objSCJobID, strSampleName, RecievedDate, strReportID, strModelNumber, strVisualMatrixName, strProjectId, strProjectName, strClientName,
                                                strQCTypeName, strQCTypeName, strUqQcTypeID, strAnalyedby, objAnalyedDate, strAnalyedby, objAnalyedDate, objuqQCBatchID, strHumidity, strRoomtemp, strQCBatchID, strLabwareName1,
                                                strLabwareName2, strLabwareName3, strLabwareName4, strLabwareName5, strAssignedName1, strAssignedName2, strAssignedName3, strAssignedName4, strAssignedName5, strFileNumber1,
                                                strFileNumber2, strFileNumber3, strFileNumber4, strFileNumber5, strSpecification1, strSpecification2, strSpecification3, strSpecification4, strSpecification5, objExpirationDate1,
                                                objExpirationDate2, objExpirationDate3, objExpirationDate4, objExpirationDate5, intSurrogateSort, intuqTestSurrogateId, intRunno, strAssignedName, strLabwareName, strSerialNumber,
                                                strSpikeAmount, strRecLCLimit, strRPDLCLimit, strRECCTRLLIMIT, strRDCTRLLIMIT, strRECULIMIT, strRPDULIMIT, strRECLLIMIT, strRPDLLIMIT, intdilutioncnt, strdilution, DilIsReport, strSampleType, strDescription, strSampleLocation, strSampleLayerID, strSampledBy, strProjectLocation, strStartFlow, strEndFlow, starttime, endtime, strTime, totaltime, strlength, strwidth, strarea);
                                strRECCTRLLIMIT = "";
                                strRDCTRLLIMIT = "";
                                intuqTestSurrogateId = -1;

                            }
                        }
                    }
                }

                #region oldcode
                //string selectedPath = string.Empty;
                //Thread t = new Thread((ThreadStart)(() =>
                //{
                //    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                //    saveFileDialog1.FileName = "ResultEntry";
                //    //saveFileDialog1.Filter = "Execl files (*.xls)|*.xls";
                //    saveFileDialog1.Filter = "Excel Files (*.xlsx)|*.xlsx";
                //    saveFileDialog1.FilterIndex = 2;
                //    saveFileDialog1.RestoreDirectory = true;

                //    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //    {
                //        selectedPath = saveFileDialog1.FileName;
                //    }
                //}));
                //t.SetApartmentState(ApartmentState.STA);
                //t.Start();
                //t.Join();
                //Workbook wb = new Workbook();
                //Worksheet worksheet0 = wb.Worksheets[0];
                //worksheet0.Name = "data";
                //wb.Worksheets[0].Import(dtsht1, true, 0, 0);
                //wb.SaveDocument(selectedPath);
                //FileInfo fileInfo = new FileInfo(selectedPath);

                //if (fileInfo.Exists)
                //{
                //    Application.ShowViewStrategy.ShowMessage("File download completed", InformationType.Success, 3000, InformationPosition.Top);
                //}
                //DevExpress.Spreadsheet spreadsheetControl1 = new DevExpress.Spreadsheet();
                //Worksheet worksheet = spreadsheetControl1.Document.Worksheets[0];
                #endregion
                objABinfo.dtQCdatatable = dtsht1;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            DashboardViewItem qcdetail = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
            if (qcdetail != null && qcdetail.InnerView != null)
            {
                qcdetail.InnerView.ObjectSpace.Committed -= ObjectSpace_Committed;
                SpreadSheetEntry_AnalyticalBatch curqC = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                IList<SpreadSheetEntry_AnalyticalBatch> QCBatchID = ObjectSpace.GetObjects<SpreadSheetEntry_AnalyticalBatch>(CriteriaOperator.Parse("[QCBatchID]=?", curqC.AnalyticalBatchID));
                if (QCBatchID != null && QCBatchID.Count > 1)
                {
                    string tempqc = string.Empty;
                    if (objnavigationRefresh.ClickedNavigationItem == "QCbatches")
                    {
                        CriteriaOperator qcexpression = CriteriaOperator.Parse("Max(SUBSTRING(QCBatchID, 2))");
                        CriteriaOperator qcct = CriteriaOperator.Parse("StartsWith([QCBatchID], 'QB')");
                        tempqc = (Convert.ToInt32(((XPObjectSpace)qcdetail.InnerView.ObjectSpace).Session.Evaluate(typeof(SpreadSheetEntry_AnalyticalBatch), qcexpression, qcct)) + 1).ToString();
                        var curdate = DateTime.Now.ToString("yyMMdd");
                        if (tempqc != "1")
                        {
                            var predate = tempqc.Substring(0, 6);
                            if (predate == curdate)
                            {
                                tempqc = "QB" + tempqc;
                            }
                            else
                            {
                                tempqc = "QB" + curdate + "01";
                            }
                        }
                        else
                        {
                            tempqc = "QB" + curdate + "01";
                        }
                    }
                    else
                    {
                        CriteriaOperator qcexpression = CriteriaOperator.Parse("Max(SUBSTRING(QCBatchID, 2))");
                        CriteriaOperator qcct = CriteriaOperator.Parse("StartsWith([QCBatchID], 'AB')");
                        tempqc = (Convert.ToInt32(((XPObjectSpace)qcdetail.InnerView.ObjectSpace).Session.Evaluate(typeof(SpreadSheetEntry_AnalyticalBatch), qcexpression, qcct)) + 1).ToString();
                        var curdate = DateTime.Now.ToString("yyMMdd");
                        if (tempqc != "1")
                        {
                            var predate = tempqc.Substring(0, 6);
                            if (predate == curdate)
                            {
                                tempqc = "AB" + tempqc;
                            }
                            else
                            {
                                tempqc = "AB" + curdate + "01";
                            }
                        }
                        else
                        {
                            tempqc = "AB" + curdate + "01";
                        }
                    }
                    curqC.AnalyticalBatchID = qcbatchinfo.strqcid = tempqc;
                    qcdetail.InnerView.ObjectSpace.CommitChanges();
                }
            }
        }
        private void QCadd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem qclist = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qclist") as DashboardViewItem;
                DashboardViewItem qctype = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qctype") as DashboardViewItem;
                DashboardViewItem qcdetail = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qcdetail") as DashboardViewItem;
                if (qclist != null && qctype != null && qclist.InnerView != null && qctype.InnerView != null)
                {
                    ASPxGridListEditor qclistgrid = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                    ASPxGridListEditor qctypegrid = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                    if (qclistgrid != null && qctypegrid != null && qclistgrid.Grid != null && qctypegrid.Grid != null)
                    {
                        if (qclistgrid.Grid.Selection.Count == 1)
                        {
                            QCBatchSequence qbslist = (QCBatchSequence)qclistgrid.GetSelectedObjects()[0];
                            QCType selqc = (QCType)e.CurrentObject;
                            if (qbslist != null && selqc != null && selqc.QCSource != null || qbslist.QCType.QCTypeName == "Sample" || selqc.QCSource == null && qbslist.QCType.QCTypeName != "Sample" && qbslist.QCType.QCSource == null)
                            {
                                //if (selqc.QCRootRole == QCRoleCN.加标 && selqc.QCSource != null && qbslist.QCType.QCTypeName != selqc.QCSource.QCTypeName)
                                //{
                                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "qccannotadd"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                //    return;
                                //}
                                if (selqc.QCSource != null && qbslist.QCType.QCTypeName != selqc.QCSource.QC_Source)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "qccannotadd"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                QCBatchSequence qCBatch = qclist.InnerView.ObjectSpace.CreateObject<QCBatchSequence>();
                                qCBatch.QCType = qclist.InnerView.ObjectSpace.GetObjectByKey<QCType>(selqc.Oid);
                                qCBatch.batchno = qbslist.batchno;
                                qCBatch.Runno = qbslist.Runno;
                                qCBatch.SampleID = qbslist.SampleID;
                                qCBatch.Dilution = "1";
                                if (qbslist.FinalVolume != null && qbslist.SampleAmount != null)
                                {
                                    qCBatch.FinalVolume = qbslist.FinalVolume;
                                    qCBatch.SampleAmount = qbslist.SampleAmount;
                                }

                                int canaddtop = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.Sort == 0).ToList().Count;
                                if (qbslist.Sort == 1 && canaddtop == 0 && (selqc.QCSource == null || selqc.QCSource.QC_Source != "Sample"))
                                {
                                    qCBatch.Sort = qbslist.Sort - 1;
                                }
                                else
                                {
                                    qCBatch.Sort = qbslist.Sort + 1;
                                    //if (selqc.QCSource.QC_Source != "Sample")
                                    //{
                                    //    qCBatch.Sort = qbslist.Sort + 1; 
                                    //}
                                    //else
                                    //{
                                    //    QCBatchSequence objPrevAdded = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.QCType.Oid == selqc.Oid && a.SYSSamplecode.StartsWith(qbslist.StrSampleID+selqc.QCTypeName)).OrderByDescending(a => a.Sort).FirstOrDefault();
                                    //    if (objPrevAdded != null)
                                    //    {
                                    //        qCBatch.Sort = objPrevAdded.Sort + 1;
                                    //    }
                                    //}
                                }
                                if (selqc.QCSource != null && selqc.QCSource.QC_Source == qbslist.QCType.QCTypeName)
                                {
                                    qCBatch.StrSampleID = qbslist.SYSSamplecode;
                                }
                                else if (selqc.QCSource != null && selqc.QCSource.QC_Source == "Sample" /*|| selqc.QCRole == QCRoleCN.平行*/)
                                {
                                    qCBatch.StrSampleID = qbslist.StrSampleID;
                                }
                                int tempindex = qCBatch.Sort;
                                foreach (QCBatchSequence sequences in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.Sort >= tempindex).OrderBy(a => a.Sort).ToList())
                                {
                                    if (tempindex == sequences.Sort)
                                    {
                                        sequences.Sort += 1;
                                        tempindex = sequences.Sort;
                                    }
                                }
                                ((ListView)qclist.InnerView).CollectionSource.Add(qCBatch);
                                int tempsort = 1;
                                foreach (QCBatchSequence curqbs in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.QCType.Oid == selqc.Oid).OrderBy(a => a.Sort).ToList())
                                {
                                    if (selqc.QCSource != null)
                                    {
                                        if (selqc.QCSource.QC_Source == "Sample")
                                        {
                                            var objqc = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.batchno == curqbs.batchno && i.QCType.Oid == selqc.Oid).OrderBy(a => a.Sort).ToList();
                                            if (objqc != null)
                                            {
                                                int objindex = objqc.IndexOf(curqbs);
                                                if (objindex == 0)
                                                {
                                                    curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                    curqbs.SYSSamplecode = curqbs.StrSampleID + curqbs.QCType.QCTypeName + tempsort;
                                                }
                                                else
                                                {
                                                    curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                    curqbs.SYSSamplecode = curqbs.StrSampleID + curqbs.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                }
                                                SpreadSheetEntry_AnalyticalBatch objQcbatch = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                                                if (objQcbatch != null && objQcbatch.Test != null && objQcbatch.Test.IsPLMTest)
                                                {
                                                    const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                                    string valueeql = "";
                                                    int i = 0;
                                                    foreach (QCBatchSequence objsampll in objqc)
                                                    {
                                                        valueeql = null;
                                                        if (i >= letterseql.Length)
                                                            valueeql += letterseql[i / letterseql.Length - 1];

                                                        valueeql += letterseql[i % letterseql.Length];
                                                        objsampll.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        objsampll.StrSampleL = curqbs.StrSampleID + curqbs.QCType.QCTypeName + "." + valueeql;
                                                        ((ListView)qclist.InnerView).CollectionSource.Add(objsampll);
                                                        i++;
                                                    }
                                                }
                                                //if (objqc.Count == (objindex + 1))
                                                {
                                                    tempsort++;
                                                }
                                            }
                                        }
                                        else if (selqc.QCSource.QC_Source == "MS")
                                        {
                                            var objqc = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.batchno == curqbs.batchno && i.QCType.Oid == selqc.Oid).OrderBy(a => a.Sort).ToList();
                                            if (objqc != null)
                                            {
                                                string MSsampleid = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == curqbs.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.StrSampleID).FirstOrDefault();
                                                string MSSYSSamplecode = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == curqbs.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample" /*&& a.Oid == qbslist.Oid*/).Select(i => i.SYSSamplecode).FirstOrDefault();
                                                if (!string.IsNullOrEmpty(MSsampleid) && !string.IsNullOrEmpty(MSSYSSamplecode))
                                                {
                                                    int objindex = objqc.IndexOf(curqbs);
                                                    if (objindex == 0)
                                                    {
                                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        curqbs.SYSSamplecode = MSsampleid + curqbs.QCType.QCTypeName + tempsort;
                                                    }
                                                    else
                                                    {
                                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        curqbs.SYSSamplecode = MSsampleid + curqbs.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                    }
                                                    curqbs.StrSampleID = MSSYSSamplecode.Replace(MSsampleid, "");
                                                    //if (objqc.Count == (objindex + 1))
                                                    {
                                                        tempsort++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (curqbs.QCType.QcRole != null && !curqbs.QCType.QcRole.IsBlank)
                                            {
                                                var objqc = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == curqbs.batchno && a.StrSampleID == curqbs.StrSampleID).OrderBy(a => a.Sort).ToList();
                                                if (objqc != null)
                                                {
                                                    int objindex = objqc.IndexOf(curqbs);
                                                    if (objindex == 0)
                                                    {
                                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        curqbs.SYSSamplecode = curqbs.QCType.QCTypeName + tempsort;
                                                    }
                                                    else
                                                    {
                                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        curqbs.SYSSamplecode = curqbs.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                    }
                                                    //if (objqc.Count == (objindex + 1))
                                                    {
                                                        tempsort++;
                                                    }
                                                }
                                                //curqbs.SYSSamplecode = curqbs.StrSampleID.Replace(curqbs.QCType.QCSource.QCTypeName, curqbs.QCType.QCTypeName) + tempsort;
                                            }
                                            else
                                            {
                                                curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                curqbs.SYSSamplecode = curqbs.QCType.QCTypeName + tempsort;
                                                tempsort++;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                        curqbs.SYSSamplecode = curqbs.QCType.QCTypeName + tempsort;
                                        tempsort++;
                                    }
                                }
                            }
                            //qclistgrid.Grid.Selection.UnselectAll();
                            //qctypegrid.Grid.Selection.UnselectAll();
                            ((ListView)qclist.InnerView).Refresh();
                        }
                        else if (qclistgrid.Grid.Selection.Count > 1)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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

        private void QCremove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem qclist = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qclist") as DashboardViewItem;
                DashboardViewItem qctype = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qctype") as DashboardViewItem;
                DashboardViewItem qcdetail = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qcdetail") as DashboardViewItem;
                if (qclist != null && qctype != null && qclist.InnerView != null && qctype.InnerView != null)
                {
                    ASPxGridListEditor qclistgrid = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                    ASPxGridListEditor qctypegrid = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                    if (qclistgrid != null && qctypegrid != null && qclistgrid.Grid != null && qctypegrid.Grid != null)
                    {
                        QCBatchSequence qbslist = (QCBatchSequence)e.CurrentObject;
                        if (qbslist != null)
                        {
                            if (qbslist.QCType.QCTypeName != "Sample")
                            {
                                int canremove = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.batchno == qbslist.batchno && i.QCType.QCTypeName != "Sample" && i.QCType.QCSource != null && i.QCType.QCSource.Oid == qbslist.QCType.Oid).ToList().Count;
                                int intSourceQCExist = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.StrSampleID != null && qbslist.SystemID != null && i.StrSampleID == qbslist.SystemID).ToList().Count;
                                if (canremove == 0 && intSourceQCExist == 0)
                                {
                                    int tempindex = qbslist.Sort;
                                    foreach (QCBatchSequence sequences in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.Sort >= tempindex).OrderBy(a => a.Sort).ToList())
                                    {
                                        if (tempindex == sequences.Sort)
                                        {
                                            sequences.Sort -= 1;
                                            tempindex = sequences.Sort;
                                        }
                                    }
                                    string strqctypename = qbslist.QCType.QCTypeName;
                                    Guid seqqcid = qbslist.QCType.Oid;
                                    string strSystemID = qbslist.SystemID;
                                    qclist.InnerView.ObjectSpace.RemoveFromModifiedObjects(qbslist);
                                    ((ListView)qclist.InnerView).CollectionSource.Remove(qbslist);
                                    int tempsort = 1;
                                    foreach (QCBatchSequence curqbs in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.QCType.Oid == seqqcid).OrderBy(a => a.Sort).ToList())
                                    {
                                        if (curqbs.QCType.QCSource != null)
                                        {
                                            if (curqbs.QCType.QCSource.QC_Source == "Sample")
                                            {
                                                var objqc = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.batchno == curqbs.batchno && i.QCType.Oid == seqqcid).OrderBy(a => a.Sort).ToList();
                                                if (objqc != null)
                                                {
                                                    int objindex = objqc.IndexOf(curqbs);
                                                    if (objindex == 0)
                                                    {
                                                        curqbs.Runno = tempsort;
                                                        foreach (QCBatchSequence srcqbs in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.StrSampleID == curqbs.SystemID).ToList())
                                                        {
                                                            srcqbs.StrSampleID = curqbs.QCType.QCTypeName + tempsort;
                                                        }
                                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        curqbs.SYSSamplecode = curqbs.StrSampleID + curqbs.QCType.QCTypeName + tempsort;
                                                    }
                                                    else
                                                    {
                                                        curqbs.Runno = tempsort;
                                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        curqbs.SYSSamplecode = curqbs.StrSampleID + curqbs.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                    }
                                                    SpreadSheetEntry_AnalyticalBatch objQcbatch = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                                                    if (objQcbatch != null && objQcbatch.Test != null && objQcbatch.Test.IsPLMTest)
                                                    {
                                                        const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                                        string valueeql = "";
                                                        int i = 0;
                                                        foreach (QCBatchSequence objsampll in objqc)
                                                        {
                                                            valueeql = null;
                                                            if (i >= letterseql.Length)
                                                                valueeql += letterseql[i / letterseql.Length - 1];

                                                            valueeql += letterseql[i % letterseql.Length];
                                                            objsampll.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                            objsampll.StrSampleL = curqbs.StrSampleID + curqbs.QCType.QCTypeName + "." + valueeql;
                                                            ((ListView)qclist.InnerView).CollectionSource.Add(objsampll);
                                                            i++;
                                                        }
                                                    }
                                                    if (objqc.Count == (objindex + 1))
                                                    {
                                                        tempsort++;
                                                    }
                                                }
                                            }
                                            else if (curqbs.QCType.QCSource.QC_Source == "MS")
                                            {
                                                var objqc = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.batchno == curqbs.batchno && i.QCType.Oid == seqqcid).OrderBy(a => a.Sort).ToList();
                                                if (objqc != null)
                                                {
                                                    string MSsampleid = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == curqbs.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.StrSampleID).FirstOrDefault();
                                                    string MSSYSSamplecode = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == curqbs.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.SYSSamplecode).FirstOrDefault();
                                                    if (!string.IsNullOrEmpty(MSsampleid) && !string.IsNullOrEmpty(MSSYSSamplecode))
                                                    {
                                                        int objindex = objqc.IndexOf(curqbs);
                                                        if (objindex == 0)
                                                        {
                                                            curqbs.Runno = tempsort;
                                                            curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                            curqbs.SYSSamplecode = MSsampleid + curqbs.QCType.QCTypeName + tempsort;
                                                        }
                                                        else
                                                        {
                                                            curqbs.Runno = tempsort;
                                                            curqbs.SYSSamplecode = MSsampleid + curqbs.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                        }
                                                        curqbs.StrSampleID = MSSYSSamplecode.Replace(MSsampleid, "");
                                                        if (objqc.Count == (objindex + 1))
                                                        {
                                                            tempsort++;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            if (curqbs.QCType.QcRole != null && !curqbs.QCType.QcRole.IsBlank)
                                            {
                                                var objqc = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == curqbs.batchno && a.StrSampleID == curqbs.StrSampleID).OrderBy(a => a.Sort).ToList();
                                                if (objqc != null)
                                                {
                                                    int objindex = objqc.IndexOf(curqbs);
                                                    if (objindex == 0)
                                                    {
                                                        curqbs.Runno = tempsort;
                                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        curqbs.SYSSamplecode = curqbs.QCType.QCTypeName + tempsort;
                                                    }
                                                    else
                                                    {
                                                        curqbs.Runno = tempsort;
                                                        curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                        curqbs.SYSSamplecode = curqbs.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                    }
                                                    if (objqc.Count == (objindex + 1))
                                                    {
                                                        tempsort++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                curqbs.Runno = tempsort;
                                                curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                                curqbs.SYSSamplecode = curqbs.QCType.QCTypeName + tempsort;
                                                tempsort++;
                                            }
                                        }
                                        else
                                        {
                                            curqbs.Runno = tempsort;
                                            foreach (QCBatchSequence srcqbs in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(i => i.StrSampleID == curqbs.SystemID).ToList())
                                            {
                                                srcqbs.StrSampleID = curqbs.QCType.QCTypeName + tempsort;
                                            }
                                            curqbs.SystemID = curqbs.QCType.QCTypeName + tempsort;
                                            curqbs.SYSSamplecode = curqbs.QCType.QCTypeName + tempsort;
                                            tempsort++;
                                        }
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "qccannotremove"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "samplecannotremove"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            }
                            //qclistgrid.Grid.Selection.UnselectAll();
                            //qctypegrid.Grid.Selection.UnselectAll();
                            ((ListView)qclist.InnerView).Refresh();
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

        private void QCprevious_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem qclist = ((DashboardView)View).FindItem("qclist") as DashboardViewItem;
                DashboardViewItem qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                DashboardViewItem qcdetail = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                ActionContainerViewItem btnReset = ((DashboardView)View).FindItem("qcaction0") as ActionContainerViewItem;
                QCreset.Active.SetItemValue("btnreset", true);
                if (qclist != null && qclist.InnerView != null && qcdetail != null && qcdetail.InnerView != null && qctype != null && qctype.InnerView != null)
                {
                    ASPxGridListEditor gridlisteditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                    if (gridlisteditor != null && gridlisteditor.Grid != null)
                    {
                        gridlisteditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = false;
                    }
                    foreach (QCBatchSequence qCBatchseq in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(a => a.Sort).ToList())
                    {
                        if (qCBatchseq.IsDilution == true)
                        {
                            ((ListView)qclist.InnerView).CollectionSource.Remove(qCBatchseq);
                            ((ListView)qclist.InnerView).Refresh();
                        }
                        else
                        {
                            ((ListView)qclist.InnerView).AllowEdit["CanSampleBatchEdit"] = true;
                            if (qCBatchseq.QCType.QCTypeName != "Sample")
                            {
                                qclist.InnerView.ObjectSpace.RemoveFromModifiedObjects(qCBatchseq);
                                ((ListView)qclist.InnerView).CollectionSource.Remove(qCBatchseq);
                                ((ListView)qclist.InnerView).Refresh();
                            }
                        }
                    }

                    int tempindex = 1;
                    foreach (QCBatchSequence sequences in ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().OrderBy(a => a.Sort).ToList())
                    {
                        sequences.Sort = tempindex;
                        if (sequences != null)
                        {
                            if (qcbatchinfo != null && qcbatchinfo.lststrseqstringSampleAmount != null)
                            {
                                foreach (string objstrSA in qcbatchinfo.lststrseqstringSampleAmount.ToList())
                                {
                                    string[] strSA = objstrSA.Split('|');
                                    if (strSA[0].Contains(sequences.Oid.ToString()))
                                    {
                                        sequences.SampleAmount = strSA[1];
                                    }
                                }
                            }
                            if (qcbatchinfo != null && qcbatchinfo.lststrseqstringFinalVolume != null)
                            {
                                foreach (string objstrFV in qcbatchinfo.lststrseqstringFinalVolume.ToList())
                                {
                                    string[] strFV = objstrFV.Split('|');
                                    if (strFV[0].Contains(sequences.Oid.ToString()))
                                    {
                                        sequences.FinalVolume = strFV[1];
                                    }
                                }
                            }
                        }
                        tempindex++;
                        ASPxGridListEditor gridlistEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                        gridlistEditor.Grid.Selection.SelectRowByKey(sequences.Oid);
                    }
                    SpreadSheetEntry_AnalyticalBatch batch = (SpreadSheetEntry_AnalyticalBatch)((DetailView)qcdetail.InnerView).CurrentObject;
                    batch.ISShown = true;
                    if (qcbatchinfo.strqcid != null)
                    {
                        disenbcontrols(true, false, qcdetail.InnerView);
                    }
                    else
                    {
                        disenbcontrols(true, true, qcdetail.InnerView);
                    }
                    if (batch != null && !string.IsNullOrEmpty(batch.Jobid))
                    {
                        QCType sampleqCType = qclist.InnerView.ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                        string[] ids = batch.Jobid.Split(';');
                        foreach (string id in ids)
                        {
                            Samplecheckin objsamplecheckin = qclist.InnerView.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", id));
                            if (objsamplecheckin != null)
                            {
                                IList<SampleParameter> objsp = qclist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID]=? and [Testparameter.TestMethod.Oid] = ? And [SignOff] = True And [IsTransferred] = true And [QCBatchID] Is Null", objsamplecheckin.Oid, batch.Test.Oid));
                                IList<SampleLogIn> objdistsl = new List<SampleLogIn>();
                                foreach (SampleParameter sample in objsp)
                                {
                                    if (!objdistsl.Contains(sample.Samplelogin))
                                    {
                                        objdistsl.Add(sample.Samplelogin);
                                    }
                                }
                                objdistsl = objdistsl.Where(s => !((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Any(q => q.SampleID.Oid == s.Oid)).ToList();
                                foreach (SampleLogIn sampleLog in objdistsl.OrderBy(a => int.Parse(a.SampleID.Split('-')[1])).ToList())
                                {
                                    for (int i = 1; i <= batch.Noruns; i++)
                                    {
                                        QCBatchSequence qCBatch = qclist.InnerView.ObjectSpace.CreateObject<QCBatchSequence>();
                                        qCBatch.QCType = sampleqCType;
                                        qCBatch.SampleID = sampleLog;
                                        qCBatch.StrSampleID = sampleLog.SampleID;
                                        qCBatch.SYSSamplecode = sampleLog.SampleID;
                                        qCBatch.Runno = i;
                                        ((ListView)qclist.InnerView).CollectionSource.Add(qCBatch);
                                    }
                                }
                            }
                        }
                        ASPxGridListEditor gridListEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.ClearSort();
                            if (qcbatchinfo.strqcid == null)
                            {
                                gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["StrSampleID"], ColumnSortOrder.Ascending);
                            }
                            else
                            {
                                gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Sort"], ColumnSortOrder.Ascending);
                            }
                            foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                            {
                                if (column.Name == "SelectionCommandColumn")
                                {
                                    column.Visible = true;
                                }
                                else if (column.Name == "QCremove")
                                {
                                    column.Visible = false;
                                }
                            }
                            gridListEditor.Grid.Columns["Sort"].Visible = true;
                            gridListEditor.Grid.Columns["Sort"].Width = 50;
                            gridListEditor.Grid.Columns["SampleName"].Width = 150;
                            gridListEditor.Grid.Columns["SYSSamplecode"].Width = 200;
                            if (!qcbatchinfo.IsPLMTest)
                            {
                                gridListEditor.Grid.Columns["DilutionCount"].Visible = true;
                                gridListEditor.Grid.Columns["DilutionCount"].Width = 100;
                                gridListEditor.Grid.Columns["DilutionCount"].SetColVisibleIndex(4);
                            }
                            else
                            {
                                gridListEditor.Grid.Columns["DilutionCount"].Visible = false;
                            }
                            gridListEditor.Grid.Columns["Dilution"].Visible = false;
                            gridListEditor.Grid.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                            for (int i = 0; i <= gridListEditor.Grid.VisibleRowCount; i++)
                            {
                                int cursort = Convert.ToInt32(gridListEditor.Grid.GetRowValues(i, "Sort"));
                                if (cursort != 0)
                                {
                                    gridListEditor.Grid.Selection.SelectRow(i);
                                }
                            }
                        }
                        ASPxGridListEditor qctypegridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                        if (qctypegridListEditor != null && qctypegridListEditor.Grid != null)
                        {
                            GridViewColumn QCadd = qctypegridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "QCadd").ToList()[0];
                            if (QCadd != null)
                            {
                                QCadd.Visible = false;
                            }
                        }
                        ActionContainerViewItem qcaction2 = ((DashboardView)View).FindItem("qcaction2") as ActionContainerViewItem;
                        qcaction2.Actions[0].Enabled.SetItemValue("key", false);
                        if (CurrentLanguage == "En")
                        {
                            qcaction2.Actions[1].Caption = "Sort";
                        }
                        else
                        {
                            qcaction2.Actions[1].Caption = "序号";
                        }
                        ((ListView)qclist.InnerView).Refresh();
                    }
                }

                ASPxGridListEditor qclistEditor = ((ListView)qclist.InnerView).Editor as ASPxGridListEditor;
                if (qclistEditor != null && qclistEditor.Grid != null)
                {
                    foreach (GridViewColumn column in qclistEditor.Grid.Columns)
                    {
                        if (column.Name == "QCremove")
                        {
                            column.Visible = false;
                        }
                    }
                }
                ASPxGridListEditor qctypeEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                if (qctypeEditor != null && qctypeEditor.Grid != null)
                {
                    foreach (GridViewColumn column in qctypeEditor.Grid.Columns)
                    {
                        if (column.Name == "QCadd")
                        {
                            column.Visible = false;
                        }
                    }
                }
                if (qclistEditor != null && qclistEditor.Grid != null)
                {
                    qclistEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (View.Id == "QCBatch_ListView_Copy" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_History")
                {
                    SpreadSheetEntry_AnalyticalBatch batch = (SpreadSheetEntry_AnalyticalBatch)e.InnerArgs.CurrentObject;
                    if (batch != null)
                    {
                        qcbatchinfo.strqcid = batch.AnalyticalBatchID;
                    }
                    IsQCview.IsViewMode = true;
                    Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "QCBatchsequence", true));
                    e.Handled = true;
                }
                else
                {
                    qcbatchinfo.strqcbatchid = null;
                    qcbatchinfo.strqcid = null;
                    if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject.GetType() == typeof(TestMethod))
                    {
                        TestMethod objTM = (TestMethod)e.InnerArgs.CurrentObject;
                        //SpreadSheetBuilder_TestParameter templatetest = ObjectSpace.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID] =?", ((TestMethod)e.InnerArgs.CurrentObject).Oid));
                        //////if (objTM != null && objTM.IsSDMSTest || objTM.IsPLMTest)
                        {
                            qcbatchinfo.OidTestMethod = ((TestMethod)e.InnerArgs.CurrentObject).Oid;
                            e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "QCBatchsequence", true);
                        }
                        //////else
                        //////{
                        //////    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "sdmstemplatenotfound"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        //////}
                        //qcbatchinfo.OidTestMethod = ((TestMethod)e.InnerArgs.CurrentObject).Oid;
                        //e.InnerArgs.ShowViewParameters.CreatedView = Application.CreateDashboardView(Application.CreateObjectSpace(), "QCBatchsequence", true);
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

        private void disenbcontrols(bool enbstat, bool isnew, DevExpress.ExpressApp.View view)
        {
            try
            {
                SpreadSheetEntry_AnalyticalBatch curabobj = (SpreadSheetEntry_AnalyticalBatch)view.CurrentObject;
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
                    else if (item.GetType() == typeof(AspxGridLookupCustomEditor))
                    {
                        AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                    {
                        ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                        if (curabobj != null && !string.IsNullOrEmpty(curabobj.AnalyticalBatchID))
                        {
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                            }
                        }
                        else if (curabobj != null && string.IsNullOrEmpty(curabobj.AnalyticalBatchID))
                        {
                            if (propertyEditor != null && propertyEditor.Editor != null && item.Id != "Instrument")
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                            }
                        }

                    }
                    else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                    {
                        ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                    {
                        ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(FileDataPropertyEditor))
                    {
                        FileDataPropertyEditor propertyEditor = item as FileDataPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                    {
                        ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
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
                            if (isnew)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                            }
                            else
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", false);
                            }
                        }
                    }
                    else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                    {
                        ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
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
        private void OpenResultEntry_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem QCDetailView = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                if (QCDetailView != null && QCDetailView.InnerView != null && ((DetailView)QCDetailView.InnerView).CurrentObject != null)
                {
                    SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)((DetailView)QCDetailView.InnerView).CurrentObject;
                    objQPInfo.lstQCBatchID = new List<string>();
                    //objQPInfo.SelectMode = QueryMode.QC;
                    objQPInfo.lstQCBatchID.Add(qC.AnalyticalBatchID);
                }
                objQPInfo.ResultEntryQueryFilter = "QCBatchID.qcseqdetail.GCRecord is null and [QCBatchID.qcseqdetail.QCBatchID] IN (" + "'" + string.Join("','", objQPInfo.lstQCBatchID) + "'" + ")";
                objQPInfo.QCResultEntryQueryFilter = objQPInfo.ResultEntryQueryFilter;
                Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "ResultEntry_Enter", true));

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ReagentLink_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(os, typeof(Reagent));
                ListView createdView = Application.CreateListView("Reagent_LookupListView", cs, false);
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += ReagentLink_Accept;
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

        private void ReagentLink_Accept(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    DashboardViewItem QCDetailView = null;
                    if (Application.MainWindow.View is DashboardView)
                    {
                        QCDetailView = ((DashboardView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    else
                    if (Application.MainWindow.View is DetailView)
                    {
                        QCDetailView = ((DetailView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    if (QCDetailView != null && QCDetailView.InnerView != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch objQc = ((SpreadSheetEntry_AnalyticalBatch)QCDetailView.InnerView.CurrentObject);
                        foreach (Reagent obj in e.AcceptActionArgs.SelectedObjects)
                        {
                            if (objQc != null && !objQc.Reagents.Contains(obj))
                            {
                                objQc.Reagents.Add(QCDetailView.InnerView.ObjectSpace.GetObject(obj));
                                ((ListView)View).CollectionSource.Add(View.ObjectSpace.GetObject(obj));
                            }
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ReagentUnLink_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (((ListView)View).SelectedObjects.Count > 0)
                {
                    DashboardViewItem QCReagent = null;
                    DashboardViewItem QCDetailView = null;
                    if (Application.MainWindow.View is DashboardView)
                    {
                        QCReagent = ((DashboardView)Application.MainWindow.View).FindItem("Reagent") as DashboardViewItem;
                        QCDetailView = ((DashboardView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    else
                    if (Application.MainWindow.View is DetailView)
                    {
                        QCReagent = ((DetailView)Application.MainWindow.View).FindItem("Reagent") as DashboardViewItem;
                        QCDetailView = ((DetailView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    if (QCDetailView != null && QCDetailView.InnerView != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch objQc = ((SpreadSheetEntry_AnalyticalBatch)QCDetailView.InnerView.CurrentObject);
                        foreach (Reagent obj in ((ListView)View).SelectedObjects)
                        {
                            objQc.Reagents.Remove(QCDetailView.InnerView.ObjectSpace.GetObject(obj));
                            ((ListView)QCReagent.InnerView).CollectionSource.Remove(View.ObjectSpace.GetObject(obj));
                            QCReagent.InnerView.Refresh();
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
        private void InstrumentUnLink_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (((ListView)View).SelectedObjects.Count > 0)
                {
                    DashboardViewItem QCInstrument = null;
                    DashboardViewItem QCDetailView = null;
                    if (Application.MainWindow.View is DashboardView)
                    {
                        QCInstrument = ((DashboardView)Application.MainWindow.View).FindItem("Instrument") as DashboardViewItem;
                        QCDetailView = ((DashboardView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    else
                    if (Application.MainWindow.View is DetailView)
                    {
                        QCInstrument = ((DetailView)Application.MainWindow.View).FindItem("Instrument") as DashboardViewItem;
                        QCDetailView = ((DetailView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    if (QCDetailView != null && QCDetailView.InnerView != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch objQc = ((SpreadSheetEntry_AnalyticalBatch)QCDetailView.InnerView.CurrentObject);
                        foreach (Labware obj in ((ListView)View).SelectedObjects)
                        {
                            objQc.Instruments.Remove(QCDetailView.InnerView.ObjectSpace.GetObject(obj));
                            ((ListView)QCInstrument.InnerView).CollectionSource.Remove(View.ObjectSpace.GetObject(obj));
                            QCInstrument.InnerView.Refresh();
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

        private void InstrumentLink_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(os, typeof(Labware));
                ListView createdView = Application.CreateListView("Labware_LookupListView_QcBatch", cs, false);
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += InstrumentLink_Accept;
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

        private void InstrumentLink_Accept(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    DashboardViewItem QCDetailView = null;
                    if (Application.MainWindow.View is DashboardView)
                    {
                        QCDetailView = ((DashboardView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    else
                    if (Application.MainWindow.View is DetailView)
                    {
                        QCDetailView = ((DetailView)Application.MainWindow.View).FindItem("qcdetail") as DashboardViewItem;
                    }
                    if (QCDetailView != null && QCDetailView.InnerView != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch objQc = ((SpreadSheetEntry_AnalyticalBatch)QCDetailView.InnerView.CurrentObject);
                        foreach (Labware obj in e.AcceptActionArgs.SelectedObjects)
                        {
                            if (objQc != null && !objQc.Instruments.Contains(obj))
                            {
                                objQc.Instruments.Add(QCDetailView.InnerView.ObjectSpace.GetObject(obj));
                                ((ListView)View).CollectionSource.Add(View.ObjectSpace.GetObject(obj));
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
        private void SDMS_Open(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View is DashboardView)
                {
                    DashboardViewItem QCDetailView = ((DashboardView)View).FindItem("qcdetail") as DashboardViewItem;
                    if (QCDetailView != null && QCDetailView.InnerView != null)
                    {
                        SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)((DetailView)QCDetailView.InnerView).CurrentObject;
                        if (qC != null && qC.Test != null && (qC.TemplateID > 0 || qC.Test.IsPLMTest))
                        {
                            qcbatchinfo.canfilter = true;
                            qcbatchinfo.QCBatchOid = qC.Oid;
                            qcbatchinfo.strqcid = qcbatchinfo.strqcbatchid = qC.AnalyticalBatchID;
                            qcbatchinfo.strTest = qC.Test.TestName;
                            qcbatchinfo.OidTestMethod = qC.Test.Oid;
                            qcbatchinfo.strAB = qC.AnalyticalBatchID;
                            qcbatchinfo.qcstatus = qC.Status;
                            if (qC.Test.IsPLMTest)
                            {
                                Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "PLM", true));
                            }
                            else
                            {
                                Frame.SetView(Application.CreateDashboardView((NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet)), "SDMS", true));
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "sdmstemplatenotfound"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else
                {
                    if (View.SelectedObjects.Count == 1)
                    {
                        SpreadSheetEntry_AnalyticalBatch qC = (SpreadSheetEntry_AnalyticalBatch)View.CurrentObject;
                        if (qC != null && qC.Test != null && (qC.TemplateID > 0 || qC.Test.IsPLMTest))
                        {
                            qcbatchinfo.canfilter = true;
                            qcbatchinfo.QCBatchOid = qC.Oid;
                            qcbatchinfo.strqcid = qcbatchinfo.strqcbatchid = qC.AnalyticalBatchID;
                            qcbatchinfo.strTest = qC.Test.TestName;
                            qcbatchinfo.OidTestMethod = qC.Test.Oid;
                            qcbatchinfo.strAB = qC.AnalyticalBatchID;
                            qcbatchinfo.qcstatus = qC.Status;
                            if (qC.Test.IsPLMTest)
                            {
                                Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "PLM", true));
                            }
                            else
                            {
                                Frame.SetView(Application.CreateDashboardView((NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(SDMSDCSpreadsheet)), "SDMS", true));
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "sdmstemplatenotfound"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        if (View.SelectedObjects.Count == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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
        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "Labware_DetailView_Copy")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1100);
                    e.Height = new System.Web.UI.WebControls.Unit(750);
                    e.Handled = true;
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
