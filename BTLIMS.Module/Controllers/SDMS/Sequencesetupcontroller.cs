using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.SDMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDM.Module.Controllers.Settings
{
    public partial class Sequencesetupcontroller : ViewController
    {
        SetupRemoveInfo setupremoveinfo = new SetupRemoveInfo();
        MessageTimer timer = new MessageTimer();
        SimpleAction ADD;
        SimpleAction REMOVE;
        ModificationsController mdcSave;
        DeleteObjectsViewController dcDelete;
        public Sequencesetupcontroller()
        {
            InitializeComponent();
            TargetViewId = "SpreadSheetBuilder_SequencePattern_ListView;" + "SpreadSheetBuilder_SequencePattern_DetailView;" + "SpreadSheetBuilder_InitialQCTestRun_ListView;" + "SpreadSheetBuilder_SampleQCTestRun_ListView;" + "SpreadSheetBuilder_ClosingQCTestRun_ListView;";
            ADD = new SimpleAction(this, "ADDQC", PredefinedCategory.ObjectsCreation);
            ADD.Caption = "ADD";
            ADD.ImageName = "Add_16x16";
            ADD.TargetViewId = "SpreadSheetBuilder_InitialQCTestRun_ListView;" + "SpreadSheetBuilder_SampleQCTestRun_ListView;" + "SpreadSheetBuilder_ClosingQCTestRun_ListView;";
            ADD.Execute += ADD_Execute;
            REMOVE = new SimpleAction(this, "REMOVEQC", PredefinedCategory.ObjectsCreation);
            REMOVE.Caption = "REMOVE";
            REMOVE.ImageName = "Remove.png" /*"Remove_16x16"*/;
            REMOVE.TargetViewId = "SpreadSheetBuilder_InitialQCTestRun_ListView;" + "SpreadSheetBuilder_SampleQCTestRun_ListView;" + "SpreadSheetBuilder_ClosingQCTestRun_ListView;";
            REMOVE.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            REMOVE.Execute += REMOVE_Execute;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "SpreadSheetBuilder_SequencePattern_DetailView")
                {
                    setupremoveinfo.RemoveInitialQCTestRun = new List<SpreadSheetBuilder_InitialQCTestRun>();
                    setupremoveinfo.RemoveSampleQCTestRun = new List<SpreadSheetBuilder_SampleQCTestRun>();
                    setupremoveinfo.RemoveClosingQCTestRun = new List<SpreadSheetBuilder_ClosingQCTestRun>();
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("SaveAndCloseAction", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("ShowSaveAndNewAction", false);
                    mdcSave = Frame.GetController<ModificationsController>();
                    mdcSave.SaveAction.Executing += SaveAction_Executing;
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;

                }
                if (View.Id == "SpreadSheetBuilder_SequencePattern_DetailView" || View.Id == "SpreadSheetBuilder_SequencePattern_ListView")
                {
                    dcDelete = Frame.GetController<DeleteObjectsViewController>();
                    dcDelete.DeleteAction.Executing += DeleteAction_Executing;
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
                if (View is ListView)
                {
                    foreach (SpreadSheetBuilder_SequencePattern sequencePattern in ((ListView)View).SelectedObjects)
                    {
                        deletesequence(sequencePattern);
                    }
                }
                else if (View is DetailView)
                {
                    deletesequence((SpreadSheetBuilder_SequencePattern)View.CurrentObject);
                }
                View.ObjectSpace.CommitChanges();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void deletesequence(SpreadSheetBuilder_SequencePattern sequencePattern)
        {
            try
            {
                IList<SpreadSheetBuilder_InitialQCTestRun> initialQCTestRuns = View.ObjectSpace.GetObjects<SpreadSheetBuilder_InitialQCTestRun>(CriteriaOperator.Parse("[uqSequencePatternID]=?", sequencePattern.uqSequencePatternID));
                IList<SpreadSheetBuilder_SampleQCTestRun> sampleQCTestRuns = View.ObjectSpace.GetObjects<SpreadSheetBuilder_SampleQCTestRun>(CriteriaOperator.Parse("[uqSequencePatternID]=?", sequencePattern.uqSequencePatternID));
                IList<SpreadSheetBuilder_ClosingQCTestRun> closingQCTestRuns = View.ObjectSpace.GetObjects<SpreadSheetBuilder_ClosingQCTestRun>(CriteriaOperator.Parse("[uqSequencePatternID]=?", sequencePattern.uqSequencePatternID));
                View.ObjectSpace.Delete(initialQCTestRuns);
                View.ObjectSpace.Delete(sampleQCTestRuns);
                View.ObjectSpace.Delete(closingQCTestRuns);
                View.ObjectSpace.Delete(sequencePattern);
                View.ObjectSpace.CommitChanges();
                IObjectSpace os = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(os, typeof(SpreadSheetBuilder_SequencePattern));
                if (cs != null)
                {
                    ListView listview = Application.CreateListView("SpreadSheetBuilder_SequencePattern_ListView", cs, true);

                    Frame.SetView(listview);
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
                string[] NoNeed = { "NPTest" };
                if (NoNeed.Contains(e.Rule.Id))
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

        private void ADD_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SpreadSheetBuilder_SequencePattern objCurrentSeq = (SpreadSheetBuilder_SequencePattern)Application.MainWindow.View.CurrentObject;
                if (objCurrentSeq != null && objCurrentSeq.uqMatrixID != null && objCurrentSeq.Test != null && objCurrentSeq.uqTestMethodID != null)
                {
                    List<string> lstqctype = new List<string>();
                    if (View.Id == "SpreadSheetBuilder_InitialQCTestRun_ListView")
                    {
                        //foreach (SpreadSheetBuilder_InitialQCTestRun QC in ((ListView)View).CollectionSource.List)
                        //{
                        //    if (!lstqctype.Contains(QC.uqQCTypeID.QCTypeName))
                        //    {
                        //        lstqctype.Add(QC.uqQCTypeID.QCTypeName);
                        //    }
                        //}
                        lstqctype = ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_InitialQCTestRun>().Where(i => i.uqQCTypeID != null).Select(i => i.uqQCTypeID.QCTypeName).ToList();
                    }
                    else if (View.Id == "SpreadSheetBuilder_SampleQCTestRun_ListView")
                    {
                        //foreach (SpreadSheetBuilder_SampleQCTestRun QC in ((ListView)View).CollectionSource.List)
                        //{
                        //    if (!lstqctype.Contains(QC.uqQCTypeID.QCTypeName))
                        //    {
                        //        lstqctype.Add(QC.uqQCTypeID.QCTypeName);
                        //    }
                        //}'
                        lstqctype = ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_SampleQCTestRun>().Where(i => i.uqQCTypeID != null).Select(i => i.uqQCTypeID.QCTypeName).ToList();
                    }
                    else if (View.Id == "SpreadSheetBuilder_ClosingQCTestRun_ListView")
                    {
                        //foreach (SpreadSheetBuilder_ClosingQCTestRun QC in ((ListView)View).CollectionSource.List)
                        //{
                        //    if (!lstqctype.Contains(QC.uqQCTypeID.QCTypeName))
                        //    {
                        //        lstqctype.Add(QC.uqQCTypeID.QCTypeName);
                        //    }
                        //}
                        lstqctype = ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_ClosingQCTestRun>().Where(i => i.uqQCTypeID != null).Select(i => i.uqQCTypeID.QCTypeName).ToList();
                    }
                    CollectionSource cs = new CollectionSource(View.ObjectSpace, typeof(QCType));
                    cs.Criteria["filter"] = CriteriaOperator.Parse("Not [QCTypeName] In (" + string.Format("'{0}'", string.Join("','", lstqctype)) + ")");
                    IList<Testparameter> objTestparam = View.ObjectSpace.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ?", objCurrentSeq.uqMatrixID.MatrixName, objCurrentSeq.Test.TestName, objCurrentSeq.uqTestMethodID.MethodName.MethodNumber));
                    IList<string> qcName = objTestparam.Where(i => i.QCType != null && i.QCType.QCTypeName != "Sample" && i.QCType.QCTypeName != null).Select(i => i.QCType.QCTypeName).Distinct().ToList();
                    cs.Criteria["filter1"] = CriteriaOperator.Parse("[QCTypeName] In (" + string.Format("'{0}'", string.Join("','", qcName)) + ")");
                    ListView createdView;
                    if (View.Id == "SpreadSheetBuilder_SampleQCTestRun_ListView")
                    {
                        createdView = Application.CreateListView("QCType_LookupListView_SequenceSetup_Sampleqctype", cs, false);
                    }
                    else
                    {
                        createdView = Application.CreateListView("QCType_LookupListView_SequenceSetup", cs, false);
                    }
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += Dc_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else
                {
                    if (objCurrentSeq.uqMatrixID == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "matrixnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (objCurrentSeq.Test == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectTest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (objCurrentSeq.uqTestMethodID == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Methodnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void REMOVE_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (setupremoveinfo.RemoveInitialQCTestRun == null)
                {
                    setupremoveinfo.RemoveInitialQCTestRun = new List<SpreadSheetBuilder_InitialQCTestRun>();
                }
                if (setupremoveinfo.RemoveSampleQCTestRun == null)
                {
                    setupremoveinfo.RemoveSampleQCTestRun = new List<SpreadSheetBuilder_SampleQCTestRun>();
                }
                if (setupremoveinfo.RemoveClosingQCTestRun == null)
                {
                    setupremoveinfo.RemoveClosingQCTestRun = new List<SpreadSheetBuilder_ClosingQCTestRun>();
                }
                if (View.Id == "SpreadSheetBuilder_InitialQCTestRun_ListView")
                {
                    foreach (SpreadSheetBuilder_InitialQCTestRun QC in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(QC);
                        //View.ObjectSpace.Delete(QC);
                        setupremoveinfo.RemoveInitialQCTestRun.Add(QC);
                    }
                    int temporder = 0;
                    foreach (SpreadSheetBuilder_InitialQCTestRun IQC in ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_InitialQCTestRun>().OrderBy(a => a.Order).ToList())
                    {
                        temporder = temporder + 1;
                        IQC.Order = temporder;
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_SampleQCTestRun_ListView")
                {
                    foreach (SpreadSheetBuilder_SampleQCTestRun QC in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(QC);
                        //View.ObjectSpace.Delete(QC);
                        setupremoveinfo.RemoveSampleQCTestRun.Add(QC);
                    }
                    int temporder = 0;
                    foreach (SpreadSheetBuilder_SampleQCTestRun IQC in ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_SampleQCTestRun>().OrderBy(a => a.Order).ToList())
                    {
                        temporder = temporder + 1;
                        IQC.Order = temporder;
                    }
                }
                else if (View.Id == "SpreadSheetBuilder_ClosingQCTestRun_ListView")
                {
                    foreach (SpreadSheetBuilder_ClosingQCTestRun QC in e.SelectedObjects)
                    {
                        ((ListView)View).CollectionSource.Remove(QC);
                        //View.ObjectSpace.Delete(QC);
                        setupremoveinfo.RemoveClosingQCTestRun.Add(QC);
                    }
                    int temporder = 0;
                    foreach (SpreadSheetBuilder_ClosingQCTestRun IQC in ((ListView)View).CollectionSource.List.Cast<SpreadSheetBuilder_ClosingQCTestRun>().OrderBy(a => a.Order).ToList())
                    {
                        temporder = temporder + 1;
                        IQC.Order = temporder;
                    }
                }
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "removedsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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
                SpreadSheetBuilder_SequencePattern objCurrentSeq = (SpreadSheetBuilder_SequencePattern)Application.MainWindow.View.CurrentObject;
                if (objCurrentSeq != null && objCurrentSeq.uqMatrixID != null && objCurrentSeq.Test != null && objCurrentSeq.uqTestMethodID != null)
                {
                    IList<SpreadSheetBuilder_SequencePattern> objseqp = View.ObjectSpace.GetObjects<SpreadSheetBuilder_SequencePattern>(CriteriaOperator.Parse("[uqMatrixID] = ? And [uqTestMethodID] = ? ", objCurrentSeq.uqMatrixID.Oid, objCurrentSeq.uqTestMethodID.Oid));
                    if (objseqp != null && objseqp.Count > 0)
                    {
                        if (objCurrentSeq.uqSequencePatternID == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage("Template already exists.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                View.ObjectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
                foreach (ViewItem item in ((DetailView)View).Items)
                {
                    if (item.GetType() == typeof(DashboardViewItem))
                    {
                        DashboardViewItem dashboardView = (DashboardViewItem)item;
                        if (dashboardView != null && dashboardView.InnerView != null)
                        {
                            if (dashboardView.InnerView.Id == "SpreadSheetBuilder_InitialQCTestRun_ListView")
                            {
                                foreach (SpreadSheetBuilder_InitialQCTestRun QC in ((ListView)dashboardView.InnerView).CollectionSource.List)
                                {
                                    if (QC.uqSequencePatternID == 0)
                                    {
                                        QC.uqSequencePatternID = ((SpreadSheetBuilder_SequencePattern)View.CurrentObject).uqSequencePatternID;
                                    }
                                }
                                if (setupremoveinfo != null && setupremoveinfo.RemoveInitialQCTestRun.Count > 0)
                                {
                                    foreach (SpreadSheetBuilder_InitialQCTestRun Qc in setupremoveinfo.RemoveInitialQCTestRun.ToList())
                                    {
                                        dashboardView.InnerView.ObjectSpace.Delete(Qc);
                                    }
                                }
                            }
                            else if (dashboardView.InnerView.Id == "SpreadSheetBuilder_SampleQCTestRun_ListView")
                            {
                                foreach (SpreadSheetBuilder_SampleQCTestRun QC in ((ListView)dashboardView.InnerView).CollectionSource.List)
                                {
                                    if (QC.uqSequencePatternID == 0)
                                    {
                                        QC.uqSequencePatternID = ((SpreadSheetBuilder_SequencePattern)View.CurrentObject).uqSequencePatternID;
                                    }
                                }
                                if (setupremoveinfo != null && setupremoveinfo.RemoveSampleQCTestRun.Count > 0)
                                {
                                    foreach (SpreadSheetBuilder_SampleQCTestRun Qc in setupremoveinfo.RemoveSampleQCTestRun.ToList())
                                    {
                                        dashboardView.InnerView.ObjectSpace.Delete(Qc);
                                    }
                                }
                            }
                            else if (dashboardView.InnerView.Id == "SpreadSheetBuilder_ClosingQCTestRun_ListView")
                            {
                                foreach (SpreadSheetBuilder_ClosingQCTestRun QC in ((ListView)dashboardView.InnerView).CollectionSource.List)
                                {
                                    if (QC.uqSequencePatternID == 0)
                                    {
                                        QC.uqSequencePatternID = ((SpreadSheetBuilder_SequencePattern)View.CurrentObject).uqSequencePatternID;
                                    }
                                }
                                if (setupremoveinfo != null && setupremoveinfo.RemoveClosingQCTestRun.Count > 0)
                                {
                                    foreach (SpreadSheetBuilder_ClosingQCTestRun Qc in setupremoveinfo.RemoveClosingQCTestRun.ToList())
                                    {
                                        dashboardView.InnerView.ObjectSpace.Delete(Qc);
                                    }
                                }
                            }

                            dashboardView.InnerView.ObjectSpace.CommitChanges();
                        }
                    }
                }
                setupremoveinfo.RemoveInitialQCTestRun = new List<SpreadSheetBuilder_InitialQCTestRun>();
                setupremoveinfo.RemoveSampleQCTestRun = new List<SpreadSheetBuilder_SampleQCTestRun>();
                setupremoveinfo.RemoveClosingQCTestRun = new List<SpreadSheetBuilder_ClosingQCTestRun>();
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
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
                NestedFrame nestedFrame = (NestedFrame)Frame;
                CompositeView cview = nestedFrame.ViewItem.View;
                if (cview != null && cview.CurrentObject != null)
                {
                    //SpreadSheetBuilder_SequencePattern objCurrentSeq = (SpreadSheetBuilder_SequencePattern)Application.MainWindow.View.CurrentObject;
                    //Application.MainWindow.View.ObjectSpace.CommitChanges();

                    foreach (QCType selqc in e.AcceptActionArgs.SelectedObjects)
                    {
                        if (View.Id == "SpreadSheetBuilder_InitialQCTestRun_ListView")
                        {
                            SpreadSheetBuilder_InitialQCTestRun newInitialQC = View.ObjectSpace.CreateObject<SpreadSheetBuilder_InitialQCTestRun>();
                            newInitialQC.uqQCTypeID = View.ObjectSpace.GetObjectByKey<QCType>(selqc.Oid);
                            //if (((SpreadSheetBuilder_SequencePattern)cview.CurrentObject).uqSequencePatternID == 0)
                            newInitialQC.uqInitialQCTestRunID = Guid.NewGuid();
                            newInitialQC.uqSequencePatternID = ((SpreadSheetBuilder_SequencePattern)cview.CurrentObject).uqSequencePatternID;
                            newInitialQC.Order = ((ListView)View).CollectionSource.List.Count + 1;
                            ((ListView)View).CollectionSource.Add(newInitialQC);
                        }
                        else if (View.Id == "SpreadSheetBuilder_SampleQCTestRun_ListView")
                        {
                            SpreadSheetBuilder_SampleQCTestRun newSampleQC = View.ObjectSpace.CreateObject<SpreadSheetBuilder_SampleQCTestRun>();
                            newSampleQC.uqQCTypeID = View.ObjectSpace.GetObjectByKey<QCType>(selqc.Oid);
                            newSampleQC.uqSampleQCTestRunID = Guid.NewGuid();
                            newSampleQC.uqSequencePatternID = ((SpreadSheetBuilder_SequencePattern)cview.CurrentObject).uqSequencePatternID;
                            newSampleQC.Order = ((ListView)View).CollectionSource.List.Count + 1;
                            ((ListView)View).CollectionSource.Add(newSampleQC);

                        }
                        else if (View.Id == "SpreadSheetBuilder_ClosingQCTestRun_ListView")
                        {
                            SpreadSheetBuilder_ClosingQCTestRun newClosingQC = View.ObjectSpace.CreateObject<SpreadSheetBuilder_ClosingQCTestRun>();
                            newClosingQC.uqClosingQCTestRunID = Guid.NewGuid();
                            newClosingQC.uqQCTypeID = View.ObjectSpace.GetObjectByKey<QCType>(selqc.Oid);
                            newClosingQC.uqSequencePatternID = ((SpreadSheetBuilder_SequencePattern)cview.CurrentObject).uqSequencePatternID;
                            newClosingQC.Order = ((ListView)View).CollectionSource.List.Count + 1;
                            ((ListView)View).CollectionSource.Add(newClosingQC);
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
                if (View.Id == "SpreadSheetBuilder_InitialQCTestRun_ListView" || View.Id == "SpreadSheetBuilder_SampleQCTestRun_ListView" || View.Id == "SpreadSheetBuilder_ClosingQCTestRun_ListView")
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView cview = nestedFrame.ViewItem.View;
                    if (cview != null && cview.CurrentObject != null && !((ListView)View).CollectionSource.Criteria.ContainsKey("filter"))
                    {
                        ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[uqSequencePatternID] = ?", ((SpreadSheetBuilder_SequencePattern)cview.CurrentObject).uqSequencePatternID);
                    }
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;

                    //                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                    //{
                    ////if (e.focusedColumn.fieldName == 'Order')
                    ////{
                    //alert('S');
                    ////}
                    //}
                    //";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                        {
                             if (e.focusedColumn.fieldName !='Order')
                             {
                                e.cancel =true;
                             }
                        }";
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
                if (View.Id == "SpreadSheetBuilder_SequencePattern_DetailView")
                {
                    setupremoveinfo.RemoveInitialQCTestRun = new List<SpreadSheetBuilder_InitialQCTestRun>();
                    setupremoveinfo.RemoveSampleQCTestRun = new List<SpreadSheetBuilder_SampleQCTestRun>();
                    setupremoveinfo.RemoveClosingQCTestRun = new List<SpreadSheetBuilder_ClosingQCTestRun>();
                    mdcSave.SaveAction.Executing -= SaveAction_Executing;
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
                }
                if (View.Id == "SpreadSheetBuilder_SequencePattern_DetailView" || View.Id == "SpreadSheetBuilder_SequencePattern_ListView")
                {
                    dcDelete.DeleteAction.Executing -= DeleteAction_Executing;
                }
                if (View.Id == "SpreadSheetBuilder_InitialQCTestRun_ListView" || View.Id == "SpreadSheetBuilder_SampleQCTestRun_ListView" || View.Id == "SpreadSheetBuilder_ClosingQCTestRun_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = true;
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
