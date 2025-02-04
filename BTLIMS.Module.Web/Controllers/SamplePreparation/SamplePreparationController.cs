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
using DevExpress.Web;
using DevExpress.Web.ASPxRichEdit;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.SDMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;

namespace LDM.Module.Controllers.SamplePreparation
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SamplePreparationController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        SamplePrepBatchInfo sampleprepbatchinfo = new SamplePrepBatchInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        ICallbackManagerHolder callbackManager;
        //string CurrentLanguage = string.Empty;
        curlanguage objLanguage = new curlanguage();
        bool IsSaveaction = false;
        ShowNavigationItemController ShowNavigationController;
        ResourceManager rm;
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        CaseNarativeInfo CNInfo = new CaseNarativeInfo();
        AuditInfo objAuditInfo = new AuditInfo();
        public SamplePreparationController()
        {
            InitializeComponent();
            TargetViewId = "QCType_ListView_SamplePrepBatchSequence;" +
                           "SamplePrepBatchSequence_ListView;" + "SamplePrepBatch_DetailView;" +
                           "SamplePrepBatch_ListView;" + "SamplePrepBatch_DetailView_Copy;" +
                           "SamplePrepBatch_ListView_CopyFrom;" + "SamplePrepBatch_Reagents_ListView;" +
                           "SamplePrepBatch_Instruments_ListView;" + "TestMethod_ListView_PrepQueue;" + "SamplePrepBatch_ListView_History;"
                           + "TestMethod_PrepMethods_ListView_PrepQueue;" + "SamplePrepBatch_DetailView_Copy_History;" + "SamplePrepBatchSequence_ListView_History;"
                           + "QCType_ListView_SamplePrepBatchSequence_History;" + "Reagent_DetailView;" + "Labware_LookupListView;" + "Reagent_LookupListView;"
                           + "SamplePrepBatch_DetailView_History;" + "Notes_ListView_CaseNarrative_Sampleprep;";
            SamplePrepLoad.TargetViewId = "SamplePrepBatchsequence;" + "SamplePrepBatch_DetailView_Copy;";
            SamplePrepPrevious.TargetViewId = "SamplePrepBatchsequence;" + "SamplePrepBatch_DetailView_Copy;";
            SamplePrepReset.TargetViewId = "SamplePrepBatchsequence;" + "SamplePrepBatch_DetailView_Copy;";
            SamplePrepSort.TargetViewId = "SamplePrepBatchsequence;" + "SamplePrepBatch_DetailView_Copy;";
            SamplePrepAdd.TargetViewId = "QCType_ListView_SamplePrepBatchSequence;" + "QCType_ListView_SamplePrepBatchSequence_History;";
            SamplePrepRemove.TargetViewId = "SamplePrepBatchSequence_ListView;" + "SamplePrepBatchSequence_ListView_History;";
            btnCopyFromSamplePrepAction.TargetViewId = "SamplePrepBatch_DetailView_Copy;" + "SamplePrepBatch_DetailView_Copy_History;";
            SavePrepBatch.TargetViewId = "SamplePrepBatch_DetailView_Copy_History;" + "SamplePrepBatch_DetailView_Copy;";
            SamplePrepHistoryAction.TargetViewId = "TestMethod_ListView_PrepQueue;" + "SamplePrepBatch_ListView;";
            SamplePrepBatchDateFilter.TargetViewId = "SamplePrepBatch_ListView;";
            SamplePrepAddSamples.TargetViewId = "SamplePrepBatch_DetailView_Copy_History;";
            Comment.TargetViewId = "SamplePrepBatch_DetailView_Copy;" + "SamplePrepBatch_ListView;"+ "SamplePrepBatch_ListView_History;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (View.Id == "SamplePrepBatch_DetailView_Copy")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
                    //if (IsSaveaction == false)
                    //{
                    //    SavePrepBatch.Active.SetItemValue("View", false);
                    //}
                    ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Remarks") as ASPxRichTextPropertyEditor;
                    if (RichText != null)
                    {
                        RichText.ControlCreated += RichText_ControlCreated;
                    }
                    Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("ShowSave", false);
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("ShowSaveAndClose", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("ShowSaveAndNew", false);
                    Frame.GetController<ModificationsController>().CancelAction.Active.SetItemValue("ShowCancel", false);
                    Frame.GetController<NewObjectViewController>().Active.SetItemValue("ShowNew", false);

                    //WebWindow.CurrentRequestWindow.RegisterClientScript("alrt", "document.getElementById('separatorButton').setAttribute('onclick', 'qcuirefresh();')");
                    Frame.GetController<RefreshController>().RefreshAction.Executing += RefreshAction_Executing;
                    SamplePrepPrevious.Enabled.SetItemValue("key", false);
                    Comment.Enabled.SetItemValue("HideComment", false);

                    List<string> lstqctype = new List<string>();
                    SamplePrepBatch batch = View.CurrentObject as SamplePrepBatch;
                    if (batch != null)
                    {
                        //btnCopyFromSamplePrepAction.Active["ShowCopyFrom"] = false;
                        SamplePrepPrevious.Enabled.SetItemValue("key", false);
                        if (View.ObjectSpace.IsNewObject(batch))
                        {
                            sampleprepbatchinfo.strSamplePrepID = string.Empty;
                            sampleprepbatchinfo.OidTestMethod = null;
                            sampleprepbatchinfo.IsNewPrepBatch = true;
                            sampleprepbatchinfo.IsEnbStat = true;

                            if (objLanguage.strcurlanguage == "En")
                            {
                                SamplePrepSort.Caption = "Sort";
                            }
                            else
                            {
                                SamplePrepSort.Caption = "序号";
                            }
                            if (SamplePrepSort.Enabled.Contains("key"))
                            {
                                SamplePrepSort.Enabled.RemoveItem("key");
                            }
                            if (SamplePrepLoad.Enabled.Contains("key"))
                            {
                                SamplePrepLoad.Enabled.RemoveItem("key");
                            }
                            //if (SamplePrepPrevious.Enabled.Contains("key"))
                            //{
                            //    SamplePrepPrevious.Enabled.RemoveItem("key"); 
                            //}
                            if (SamplePrepReset.Enabled.Contains("key"))
                            {
                                SamplePrepReset.Enabled.RemoveItem("key");
                            }
                            if (btnCopyFromSamplePrepAction.Active.Contains("ShowCopyFrom"))
                            {
                                btnCopyFromSamplePrepAction.Active.RemoveItem("ShowCopyFrom");
                            }
                            WebModificationsController webModificationsController = Frame.GetController<WebModificationsController>();
                            if (webModificationsController != null)
                            {
                                if (webModificationsController.EditAction.Active.Contains("ShowEdit"))
                                {
                                    webModificationsController.EditAction.Active.RemoveItem("ShowEdit");
                                }
                            }
                            SamplePrepPrevious.Enabled.SetItemValue("key", false);
                            if (!sampleprepbatchinfo.SamplepreparationWrite)
                            {
                                btnCopyFromSamplePrepAction.Enabled.SetItemValue("key", false);
                            }
                        }
                        else
                        {
                            WebModificationsController webModificationsController = Frame.GetController<WebModificationsController>();
                            if (webModificationsController != null)
                            {
                                webModificationsController.EditAction.Active.SetItemValue("ShowEdit", false);
                            }
                            sampleprepbatchinfo.IsNewPrepBatch = false;
                            sampleprepbatchinfo.IsEnbStat = false;
                            sampleprepbatchinfo.strSamplePrepID = batch.PrepBatchID;
                            btnCopyFromSamplePrepAction.Active["ShowCopyFrom"] = false;
                            if (objLanguage.strcurlanguage == "En")
                            {
                                SamplePrepSort.Caption = "Ok";
                            }
                            else
                            {
                                SamplePrepSort.Caption = "确定";
                            }
                            SamplePrepSort.Enabled.SetItemValue("key", false);
                            SamplePrepLoad.Enabled.SetItemValue("key", false);
                            SamplePrepPrevious.Enabled.SetItemValue("key", false);
                            SamplePrepReset.Enabled.SetItemValue("key", false);
                            //((DetailView)View).ViewEditMode = ViewEditMode.View;
                        }
                        if (batch != null)
                        {
                            string[] guidStringstest = batch.Test.Split(';');
                            string[] guidStringsmatrix = batch.Matrix.Split(';');
                            string[] guidStringsmethod = batch.Method.Split(';');

                            List<Guid> lstTestOid = guidStringstest.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                            List<Guid> lstMatrixOid = guidStringsmatrix.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                            List<Guid> lstMethodOid = guidStringsmethod.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                            IList<TestMethod> lst = View.ObjectSpace.GetObjects<TestMethod>(new GroupOperator(GroupOperatorType.And,new InOperator("MatrixName.Oid", lstMatrixOid),new InOperator("Oid", lstTestOid),new InOperator("MethodName.Oid", lstMethodOid)));
                            foreach (TestMethod objTest in lst)
                            {
                                if (objTest != null && objTest.Labwares != null && objTest.Labwares.Count == 1)
                                {
                                    Labware singleLabware = objTest.Labwares.First();
                                    if (singleLabware != null)
                                    {
                                        batch.Instrument = singleLabware.Oid.ToString();
                                        batch.strInstrument = singleLabware.AssignedName.ToString();
                                        break; 
                                    }
                                }
                            }
                        }




                        //if (batch != null && batch.TestDataSource != null && batch.TestDataSource.Any(i => i.Labwares != null && i.Labwares.Any(l => !string.IsNullOrEmpty(l.LabwareName))))
                        //    {

                        //        if (batch.TestDataSource.Any(i => i.Labwares.Count == 1))
                        //        {

                        //        batch.Instrument = batch.TestDataSource.First(i => i.Labwares.Count == 1).Labwares.First().Oid.ToString();
                        //        batch.strInstrument = batch.TestDataSource.First(i => i.Labwares.Count == 1).Labwares.First().AssignedName.ToString();
                        //        }
                        //    }
            

                        //if (batch.Test != null)
                        //{
                        //    if (!string.IsNullOrEmpty(batch.Test))
                        //    {
                        //        List<string> lstTMOid = batch.Test.Split(';').ToList();
                        //        if (lstTMOid != null)
                        //        {
                        //            foreach (string obj in lstTMOid)
                        //            {
                        //                TestMethod objTM = ObjectSpace.GetObjectByKey<TestMethod>(new Guid(obj.Trim()));
                        //                if (objTM != null)
                        //                {
                        //                    foreach (Testparameter TP in objTM.TestParameter)
                        //                    {
                        //                        if (TP != null && TP.QCType != null && !lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample" && objTM.QCTypes.Contains(TP.QCType))
                        //                        {
                        //                          //  lstqctype.Add(TP.QCType.QCTypeName);
                        //                        }
                        //                    }
                        //                }
                        //            }

                        //        }
                        //    }

                        //}
                    }
                    sampleprepbatchinfo.qctypeCriteria = "[QCTypeName] In(" + string.Format("'{0}'", string.Join("','", lstqctype)) + ")";
                    sampleprepbatchinfo.canfilter = true;
                }
                
                else if (View.Id == "SamplePrepBatch_DetailView_Copy_History" ||View.Id== "SamplePrepBatch_DetailView_History")
                {
                    Comment.Enabled.SetItemValue("HideComment", true);
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
                    ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Remarks") as ASPxRichTextPropertyEditor;
                    if (RichText != null)
                    {
                        RichText.ControlCreated += RichText_ControlCreated;
                    }
                    Frame.GetController<ModificationsController>().SaveAction.Active.SetItemValue("ShowSave", false);
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.SetItemValue("ShowSaveAndClose", false);
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.SetItemValue("ShowSaveAndNew", false);
                    Frame.GetController<ModificationsController>().CancelAction.Active.SetItemValue("ShowCancel", false);
                    Frame.GetController<NewObjectViewController>().Active.SetItemValue("ShowNew", false);
                    WebModificationsController webModificationsController = Frame.GetController<WebModificationsController>();
                    if (webModificationsController != null)
                    {
                        webModificationsController.EditAction.Active.SetItemValue("ShowEdit", false);
                    }
                    SamplePrepBatch batch = View.CurrentObject as SamplePrepBatch;
                    List<string> lstqctype = new List<string>();
                    if (batch != null)
                    {
                        if (batch.Test != null)
                        {
                            if (!string.IsNullOrEmpty(batch.Test))
                            {
                                List<string> lstTMOid = batch.Test.Split(';').ToList();
                                if (lstTMOid != null)
                                {
                                    foreach (string obj in lstTMOid)
                                    {
                                        TestMethod objTM = ObjectSpace.GetObjectByKey<TestMethod>(new Guid(obj.Trim()));
                                        if (objTM != null)
                                        {
                                            foreach (Testparameter TP in objTM.TestParameter)
                                            {
                                                if (TP.QCType != null)
                                                {
                                                if (!lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample" && objTM.QCTypes.Contains(TP.QCType))
                                                {
                                                    lstqctype.Add(TP.QCType.QCTypeName);
                                                }
                                            }
                                        }
                                    }
                                    }

                                }
                            }

                        }
                        sampleprepbatchinfo.qctypeCriteria = "[QCTypeName] In(" + string.Format("'{0}'", string.Join("','", lstqctype)) + ")";
                    }
                    if (sampleprepbatchinfo.lstRemoveSampleSequence == null)
                    {
                        sampleprepbatchinfo.lstRemoveSampleSequence = new List<SamplePrepBatchSequence>();
                    }
                    if (sampleprepbatchinfo.lstRemoveSampleSequence.Count > 0)
                    {
                        sampleprepbatchinfo.lstRemoveSampleSequence.Clear();
                    }
                    if (sampleprepbatchinfo.lstAddSampleSequence == null)
                    {
                        sampleprepbatchinfo.lstAddSampleSequence = new List<SamplePrepBatchSequence>();
                    }
                    if (sampleprepbatchinfo.lstAddSampleSequence.Count > 0)
                    {
                        sampleprepbatchinfo.lstAddSampleSequence.Clear();
                    }
                    objAuditInfo.currentViewOid = null;
                    SamplePrepBatch objPrepID = (SamplePrepBatch)View.CurrentObject;
                    SRInfo.currentPrepbatchID = objPrepID;
                    if (objPrepID != null)
                    {
                        objAuditInfo.currentViewOid = objPrepID.Oid;
                    }
                }
                else if (View.Id == "SamplePrepBatchSequence_ListView")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[SamplePrepBatchDetail.PrepBatchID] = ?", sampleprepbatchinfo.strSamplePrepID);
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                else if (View.Id == "SamplePrepBatchSequence_ListView_History")
                {
                    SamplePrepBatch batch = (SamplePrepBatch)Application.MainWindow.View.CurrentObject;
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[SamplePrepBatchDetail.PrepBatchID] = ?", batch.PrepBatchID);
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                else if (View.Id == "QCType_ListView_SamplePrepBatchSequence")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(sampleprepbatchinfo.qctypeCriteria);
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                else if (View.Id == "QCType_ListView_SamplePrepBatchSequence_History")
                {
                    ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(sampleprepbatchinfo.qctypeCriteria);
                }
                else if (View.Id == "TestMethod_ListView_PrepQueue")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                else if (View.Id == "TestMethod_PrepMethods_ListView_PrepQueue")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    sampleprepbatchinfo.SamplepreparationWrite = false;
                    if (user.Roles != null && user.Roles.Count > 0)
                    {
                        if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            sampleprepbatchinfo.SamplepreparationWrite = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in user.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplePreparation" && i.Write == true) != null)
                                {
                                    sampleprepbatchinfo.SamplepreparationWrite = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "SamplePrepBatch_ListView")
                {
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    sampleprepbatchinfo.SamplepreparationWrite = false;
                    if (user.Roles != null && user.Roles.Count > 0)
                    {
                        if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            sampleprepbatchinfo.SamplepreparationWrite = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in user.RolePermissions)
                            {
                                if (objnavigationRefresh.ClickedNavigationItem == "SamplePreparation")
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplePreparation" && i.NavigationItem.IsDeleted == false && i.Write == true) != null)
                                    {
                                        sampleprepbatchinfo.SamplepreparationWrite = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplePrepBatches" && i.Write == true) != null)
                                    {
                                        sampleprepbatchinfo.SamplepreparationWrite = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    if (SamplePrepBatchDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        SamplePrepBatchDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        SamplePrepBatchDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        SamplePrepBatchDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        SamplePrepBatchDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        SamplePrepBatchDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        SamplePrepBatchDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        SamplePrepBatchDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    SamplePrepBatchDateFilter.SelectedIndex = 1;
                    ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffMonth(Datecreated , Now()) <= 3 And [Datecreated] Is Not Null");
                }
                else if (View.Id == "Reagent_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                else if (View.Id == "Notes_ListView_CaseNarrative_Sampleprep")
                {
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                }
                //else if (View.Id == "Labware_LookupListView")
                //{
                //    View.ControlsCreated += View_ControlsCreated;
                //}
                //else if(View.Id== "Reagent_LookupListView")
                //{
                //    View.ControlsCreated += View_ControlsCreated;
                //}
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
                if (Application.MainWindow.View != null && Application.MainWindow.View.ObjectTypeInfo.Type == typeof(SamplePrepBatch) && View.Id == "Labware_LookupListView" && Application.MainWindow.View is DetailView)
                {
                    ListPropertyEditor lvInstrumet = ((DetailView)Application.MainWindow.View).FindItem("Instruments") as ListPropertyEditor;
                    if (lvInstrumet != null && lvInstrumet.ListView != null)
                    {
                        if (((ListView)lvInstrumet.ListView).CollectionSource.GetCount() > 0)
                        {
                            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                            foreach (Labware objLabware in lvInstrumet.ListView.CollectionSource.List.Cast<Labware>().ToList())
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(objLabware.Oid);
                            }
                        }
                    }
                }
                if (Application.MainWindow.View != null && Application.MainWindow.View.ObjectTypeInfo.Type == typeof(SamplePrepBatch) && View.Id == "Reagent_LookupListView" && Application.MainWindow.View is DetailView)
                {
                    ListPropertyEditor lvReagent = ((DetailView)Application.MainWindow.View).FindItem("Reagents") as ListPropertyEditor;
                    if (lvReagent != null && lvReagent.ListView != null)
                    {
                        if (((ListView)lvReagent.ListView).CollectionSource.GetCount() > 0)
                        {
                            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                            foreach (Reagent objReagent in lvReagent.ListView.CollectionSource.List.Cast<Reagent>().ToList())
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(objReagent.Oid);
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

        private void RefreshAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SamplePrepBatchsequence" || View.Id == "SamplePrepBatch_DetailView_Copy")
                {
                    e.Cancel = true;
                    if (SamplePrepReset.Enabled)
                    {
                        SamplePrepReset.DoExecute();
                    }
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
                if (e.ViewID == "SamplePrepBatch_DetailView")
                {
                    Application.DetailViewCreating -= Application_DetailViewCreating;
                    IObjectSpace os = Application.CreateObjectSpace();
                    sampleprepbatchinfo.canfilter = true;
                    if (string.IsNullOrEmpty(sampleprepbatchinfo.strSamplePrepID))
                    {
                        SamplePrepBatch batch = os.CreateObject<SamplePrepBatch>();

                        //if (test != null)
                        //{
                        //    batch.Matrix = test.MatrixName;
                        //    batch.Test = test;
                        //    if (Frame.Context != TemplateContext.PopupWindow)
                        //    {
                        //        sampleprepbatchinfo.OidTestMethod = null;
                        //    }
                        //}
                        //if (sampleprepbatchinfo.OidTestMethod != null)
                        //{
                        //    //TestMethod test = os.GetObjectByKey<TestMethod>(sampleprepbatchinfo.OidTestMethod);
                        //    if (test != null)
                        //    {
                        //        batch.Matrix = test.MatrixName;
                        //        batch.Test = test;
                        //        if (Frame.Context != TemplateContext.PopupWindow)
                        //        {
                        //            sampleprepbatchinfo.OidTestMethod = null;
                        //        }
                        //    }
                        //}
                        e.View = Application.CreateDetailView(os, batch);
                        if (sampleprepbatchinfo.SamplepreparationWrite)
                        {
                            e.View.ViewEditMode = ViewEditMode.Edit;
                        }
                    }
                    else
                    {
                        SamplePrepBatch batch = os.FindObject<SamplePrepBatch>(CriteriaOperator.Parse("[PrepBatchID]=?", sampleprepbatchinfo.strSamplePrepID));
                        if (batch != null)
                        {
                            e.View = Application.CreateDetailView(os, batch);
                            if (sampleprepbatchinfo.SamplepreparationWrite)
                            {
                                e.View.ViewEditMode = ViewEditMode.Edit;
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
        private void EditAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SamplePrepBatch_ListView")
                {
                    //    e.Cancel = true;
                    IsSaveaction = true;
                    SavePrepBatch.Active.SetItemValue("View", true);
                    SamplePrepRemove.Enabled.SetItemValue("key", true);
                    SamplePrepAdd.Enabled.SetItemValue("key", true);
                    //SamplePrepBatch batch = (SamplePrepBatch)View.CurrentObject;
                    //if (batch != null)
                    //{
                    //    sampleprepbatchinfo.strSamplePrepID = batch.PrepBatchID;
                    //    sampleprepbatchinfo.CanAutopopulateSampleGrid = false;
                    //}
                    //Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "SamplePrepBatchsequence", true));
                }
                else if (View.Id == "Notes_ListView_CaseNarrative_Sampleprep")
                {
                    Notes note = (Notes)View.CurrentObject;
                    if (CNInfo.SPJobId != null)
                    {
                        note.SourceID = CNInfo.SPJobId;
                        ObjectSpace.CommitChanges();
                    }
                                      
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SamplePrepBatch_ListView")
                {
                    e.Cancel = true;
                    sampleprepbatchinfo.strSamplePrepID = string.Empty;
                    sampleprepbatchinfo.OidTestMethod = null;
                    sampleprepbatchinfo.CanAutopopulateSampleGrid = false;
                    Frame.SetView(Application.CreateDashboardView(Application.CreateObjectSpace(), "SamplePrepBatchsequence", true));
                }
                else if (View.Id == "TestMethod_PrepMethods_ListView_PrepQueue")
                {
                    e.Cancel = true;
                    if (((ListView)View).CollectionSource.GetCount() > 0)
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            List<PrepMethod> lstTest = View.SelectedObjects.Cast<PrepMethod>().ToList();
                            if (lstTest.Count > 1)
                            {
                                if (lstTest.FirstOrDefault(i => i.Tier == 2) != null)
                                {
                                    Application.ShowViewStrategy.ShowMessage("Only tier 1 will allow multiple tests.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                            }
                            Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                            sampleprepbatchinfo.SamplepreparationWrite = false;
                            //sampleprepbatchinfo.CanAutopopulateSampleGrid = true;
                            if (user.Roles != null && user.Roles.Count > 0)
                            {
                                if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                                {
                                    //TestMethod objTest = (TestMethod)e.InnerArgs.CurrentObject;
                                    PrepMethod objTest = (PrepMethod)View.CurrentObject;
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch objPrepBatch = os.CreateObject<SamplePrepBatch>();
                                    objPrepBatch.Matrix = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.MatrixName != null).Select(i => i.TestMethod.MatrixName.Oid).Distinct().ToList()));
                                    objPrepBatch.Test = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.TestName != null).Select(i => i.TestMethod.Oid).Distinct().ToList()));
                                    objPrepBatch.Method = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.MethodName != null).Select(i => i.TestMethod.MethodName.Oid).Distinct().ToList()));
                                    if (objTest != null)
                                    {
                                        objPrepBatch.PrepType = os.GetObjectByKey<PrepTypes>(objTest.PrepType.Oid);
                                        objPrepBatch.Tier = objTest.Tier;
                                        objPrepBatch.Sort = objTest.Sort;
                                    }
                                    else
                                    {
                                        objPrepBatch.PrepType = os.GetObjectByKey<PrepTypes>(lstTest.Select(i => i.PrepType.Oid).FirstOrDefault());
                                        objPrepBatch.Tier = lstTest.Select(i => i.Tier).FirstOrDefault();
                                        objPrepBatch.Sort = lstTest.Select(i => i.Sort).FirstOrDefault();
                                    }
                                    DetailView dv = Application.CreateDetailView(os, "SamplePrepBatch_DetailView_Copy", true, objPrepBatch);
                                    dv.ViewEditMode = ViewEditMode.Edit;
                                    sampleprepbatchinfo.SamplepreparationWrite = true;
                                    Frame.SetView(dv);
                                }
                                else
                                {
                                    foreach (RoleNavigationPermission role in user.RolePermissions)
                                    {
                                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplePreparation" && i.Write == true) != null)
                                        {
                                            PrepMethod objTest = (PrepMethod)View.CurrentObject;
                                            IObjectSpace os = Application.CreateObjectSpace();
                                            Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch objPrepBatch = os.CreateObject<SamplePrepBatch>();
                                            //objPrepBatch.Matrix = os.GetObject<Matrix>(objTest.TestMethod.MatrixName);
                                            //objPrepBatch.Test = os.GetObjectByKey<TestMethod>(objTest.TestMethod.Oid);
                                            //objPrepBatch.Method = os.GetObjectByKey<TestMethod>(objTest.TestMethod.MethodName.Oid);
                                            objPrepBatch.Matrix = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.MatrixName != null).Select(i => i.TestMethod.MatrixName.Oid).Distinct().ToList()));
                                            objPrepBatch.Test = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.TestName != null).Select(i => i.TestMethod.Oid).Distinct().ToList()));
                                            objPrepBatch.Method = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.MethodName != null).Select(i => i.TestMethod.MethodName.Oid).Distinct().ToList()));
                                            if (objTest != null)
                                            {
                                                objPrepBatch.PrepType = os.GetObjectByKey<PrepTypes>(objTest.PrepType.Oid);
                                                objPrepBatch.Tier = objTest.Tier;
                                                objPrepBatch.Sort = objTest.Sort;
                                            }
                                            else
                                            {
                                                objPrepBatch.PrepType = os.GetObjectByKey<PrepTypes>(lstTest.Select(i => i.PrepType.Oid).FirstOrDefault());
                                                objPrepBatch.Tier = lstTest.Select(i => i.Tier).FirstOrDefault();
                                                objPrepBatch.Sort = lstTest.Select(i => i.Sort).FirstOrDefault();
                                            }
                                            DetailView dv = Application.CreateDetailView(os, "SamplePrepBatch_DetailView_Copy", true, objPrepBatch);
                                            dv.ViewEditMode = ViewEditMode.Edit;
                                            sampleprepbatchinfo.SamplepreparationWrite = true;
                                            Frame.SetView(dv);
                                            break;
                                        }
                                    }
                                    if (!sampleprepbatchinfo.SamplepreparationWrite)
                                    {
                                        PrepMethod objTest = (PrepMethod)View.CurrentObject;
                                        IObjectSpace os = Application.CreateObjectSpace();
                                        Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch objPrepBatch = os.CreateObject<SamplePrepBatch>();
                                        //objPrepBatch.Matrix = os.GetObject<Matrix>(objTest.TestMethod.MatrixName);
                                        //objPrepBatch.Test = os.GetObjectByKey<TestMethod>(objTest.TestMethod.Oid);
                                        //objPrepBatch.Method = os.GetObjectByKey<TestMethod>(objTest.TestMethod.MethodName.Oid);
                                        objPrepBatch.Matrix = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.MatrixName != null).Select(i => i.TestMethod.MatrixName.Oid).Distinct().ToList()));
                                        objPrepBatch.Test = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.TestName != null).Select(i => i.TestMethod.Oid).Distinct().ToList()));
                                        objPrepBatch.Method = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestMethod.MethodName != null).Select(i => i.TestMethod.MethodName.Oid).Distinct().ToList()));
                                        if (objTest != null)
                                        {
                                            objPrepBatch.PrepType = os.GetObjectByKey<PrepTypes>(objTest.PrepType.Oid);
                                            objPrepBatch.Tier = objTest.Tier;
                                            objPrepBatch.Sort = objTest.Sort;
                                        }
                                        else
                                        {
                                            objPrepBatch.PrepType = os.GetObjectByKey<PrepTypes>(lstTest.Select(i => i.PrepType.Oid).FirstOrDefault());
                                            objPrepBatch.Tier = lstTest.Select(i => i.Tier).FirstOrDefault();
                                            objPrepBatch.Sort = lstTest.Select(i => i.Sort).FirstOrDefault();
                                        }
                                        DetailView dv = Application.CreateDetailView(os, "SamplePrepBatch_DetailView_Copy", true, objPrepBatch);
                                        dv.ViewEditMode = ViewEditMode.View;
                                        Frame.SetView(dv);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "notpendingsamples"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (View.Id == "SamplePrepBatch_DetailView_Copy")
                {
                    bool enbstat;
                    ActionContainerViewItem sampleprepaction2 = ((DetailView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                    if (!string.IsNullOrEmpty(sampleprepbatchinfo.strSamplePrepID) || SamplePrepSort.Caption == "Ok" || SamplePrepSort.Caption == "确定")
                    {
                        enbstat = false;
                    }
                    else
                    {
                        if (sampleprepbatchinfo.SamplepreparationWrite)
                        {
                            enbstat = true;
                        }
                        else
                        {
                            enbstat = false;
                        }
                    }
                    if (((SamplePrepBatch)View.CurrentObject != null))
                    {
                        ((SamplePrepBatch)View.CurrentObject).ISShown = enbstat;
                    }
                    string js = @"function(s,e) 
                    {
                        var nav = document.getElementById('LPcell');
                        var sep = document.getElementById('separatorCell');                      
                        if(nav != null && sep != null) 
                        {
                           // var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                          //  s.SetWidth(totusablescr /3);
                        }
                        else 
                        {
                          //  s.SetWidth(145); 
                        }                      
                    }";
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item is ASPxStringPropertyEditor)
                        {
                            ASPxStringPropertyEditor editor = (ASPxStringPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                                ASPxTextBox textBox = (ASPxTextBox)editor.Editor;
                                if (textBox != null)
                                {
                                    textBox.ClientSideEvents.Init = js;
                                    textBox.ClientInstanceName = editor.Id;
                                }
                            }
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }

                        else if (item is ASPxIntPropertyEditor)
                        {
                            ASPxIntPropertyEditor editor = (ASPxIntPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                                editor.Editor.ClientSideEvents.Init = js;
                                editor.Editor.ClientInstanceName = editor.Id;
                            }
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }

                        else if (item is ASPxDoublePropertyEditor)
                        {
                            ASPxDoublePropertyEditor editor = (ASPxDoublePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                                editor.Editor.ClientSideEvents.Init = js;
                                editor.Editor.ClientInstanceName = editor.Id;
                            }
                        }

                        else if (item is ASPxDecimalPropertyEditor)
                        {
                            ASPxDecimalPropertyEditor editor = (ASPxDecimalPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                                editor.Editor.ClientSideEvents.Init = js;
                                editor.Editor.ClientInstanceName = editor.Id;
                            }

                        }

                        else if (item is ASPxDateTimePropertyEditor)
                        {
                            ASPxDateTimePropertyEditor editor = (ASPxDateTimePropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                                editor.Editor.ClientSideEvents.Init = js;
                                editor.Editor.ClientInstanceName = editor.Id;
                            }
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item is ASPxLookupPropertyEditor)
                        {
                            ASPxLookupPropertyEditor editor = (ASPxLookupPropertyEditor)item;
                            if (editor != null && editor.DropDownEdit != null && editor.DropDownEdit.DropDown != null)
                            {
                                editor.DropDownEdit.DropDown.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                                if (editor.FindEdit != null && editor.FindEdit.Visible)
                                {
                                    editor.FindEdit.Editor.ClientSideEvents.Init = js;
                                    editor.FindEdit.Editor.ClientInstanceName = editor.Id;
                                }
                                else if (editor.DropDownEdit != null)
                                {
                                    editor.DropDownEdit.DropDown.ClientSideEvents.Init = js;
                                    editor.DropDownEdit.DropDown.ClientInstanceName = editor.Id;
                                }
                            }
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
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
                        else if (item is ASPxBooleanPropertyEditor)
                        {
                            ASPxBooleanPropertyEditor editor = (ASPxBooleanPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                            }
                        }
                        else if (item is ASPxEnumPropertyEditor)
                        {
                            ASPxEnumPropertyEditor editor = (ASPxEnumPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                            }
                        }
                        else if (item is ASPxGridLookupPropertyEditor)
                        {
                            ASPxGridLookupPropertyEditor editor = (ASPxGridLookupPropertyEditor)item;
                            if (editor != null && editor.Editor != null)
                            {
                                editor.Editor.ForeColor = Color.Black;
                                editor.AllowEdit.SetItemValue("stat", enbstat);
                            }
                        }
                        else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                        {
                            ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                                if (propertyEditor.Id != "Instrument")
                                {
                                    propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                }
                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                if (gridLookup != null)
                                {
                                    gridLookup.ClientSideEvents.Init = js;
                                    gridLookup.ClientInstanceName = propertyEditor.Id;
                                }
                            }
                            else
                            {
                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                if (gridLookup != null)
                                {
                                    gridLookup.ForeColor = Color.Black;
                                    gridLookup.ClientSideEvents.Init = js;
                                    gridLookup.ClientInstanceName = propertyEditor.Id;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(ASPxRichTextPropertyEditor))
                        {
                            ASPxRichTextPropertyEditor propertyEditor = item as ASPxRichTextPropertyEditor;
                            if (propertyEditor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                            }
                        }
                        else if (item.GetType() == typeof(ListPropertyEditor))
                        {
                            ListPropertyEditor propertyEditor = item as ListPropertyEditor;
                            if (propertyEditor != null)
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                            }
                        }
                        else if (item.GetType() == typeof(AspxGridLookupCustomEditor))
                        {
                            AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null && item.Id == "NPJobid")
                            {
                                propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                                ASPxGridLookup gridLookup = (ASPxGridLookup)propertyEditor.Editor;
                                SamplePrepBatch batch = (SamplePrepBatch)View.CurrentObject;
                                if (batch != null)
                                {
                                    if (gridLookup != null)
                                    {
                                        gridLookup.JSProperties["cpJobID"] = batch.Jobid;
                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                            {
                                            s.SetText(s.cpJobID);
                                            var nav = document.getElementById('LPcell');
                                            var sep = document.getElementById('separatorCell');                      
                                            if(nav != null && sep != null) 
                                            {
                                              //  var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                             //   s.SetWidth(totusablescr / 3);
                                            }
                                            else 
                                            {
                                              //  s.SetWidth(145); 
                                            }                      
                                            }";
                                        gridLookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                                        gridLookup.GridView.SettingsPager.AlwaysShowPager = true;
                                        gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                                        gridLookup.ValueChanged += GridLookup_ValueChanged;
                                        gridLookup.GridView.SettingsBehavior.AllowFocusedRow = false;
                                        gridLookup.GridView.Columns.Add(new GridViewCommandColumn { ShowSelectCheckbox = true });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "JobID" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "Sx" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "TAT" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DueDate" });
                                        gridLookup.GridView.Columns.Add(new GridViewDataColumn { FieldName = "DateReceived" });
                                        gridLookup.GridView.Columns["JobID"].Width = 100;
                                        gridLookup.GridView.Columns["Sx"].Width = 70;
                                        gridLookup.GridView.Columns["TAT"].Width = 70;
                                        gridLookup.GridView.Columns["DueDate"].Width = 100;
                                        gridLookup.GridView.Columns["DateReceived"].Width = 100;
                                        gridLookup.GridView.KeyFieldName = "JobID";
                                        gridLookup.TextFormatString = "{0}";
                                        gridLookup.GridView.HtmlRowPrepared += Grid_HtmlRowPrepared;
                                        DataTable table = new DataTable();
                                        table.Columns.Add("JobID");
                                        table.Columns.Add("Sx");
                                        table.Columns.Add("TAT");
                                        table.Columns.Add("DueDate");
                                        table.Columns.Add("DateReceived");
                                        table.Columns.Add("SortableDueDate", typeof(DateTime));
                                        if (batch.Test != null && batch.Method != null)
                                        {
                                            IList<SampleParameter> samples = new List<SampleParameter>();
                                            List<string> lstMatrixOid = batch.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                            List<string> lstTestOid = batch.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                            List<string> lstMethdOid = batch.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                            samples = ObjectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[SignOff] = True  And [Status] = 'PendingEntry' And [IsPrepMethodComplete]  = False And ([IsTransferred] = true Or [IsTransferred] is null)  And [TestHold]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=?", batch.Sort - 1), new InOperator("Testparameter.TestMethod.MatrixName.Oid", lstMatrixOid.Select(i => new Guid(i))),
                                                       new InOperator("Testparameter.TestMethod.Oid", lstTestOid.Select(i => new Guid(i))), new InOperator("Testparameter.TestMethod.MethodName.Oid", lstMethdOid.Select(i => new Guid(i)))));
                                            //samples = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [SignOff] = True  And [Status] = 'PendingEntry' And [IsPrepMethodComplete]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=?", batch.Method.Oid, batch.Sort - 1));
                                            if (samples != null && samples.Count > 0)
                                            {
                                                foreach (SampleParameter objsample in samples.Where(a => a.Status == Samplestatus.PendingEntry && a.Samplelogin != null && a.Samplelogin.JobID != null /*&& a.SamplePrepBatchID != null */&& !string.IsNullOrEmpty(a.Samplelogin.JobID.JobID) && a.TestHold == false).GroupBy(p => p.Samplelogin.JobID.Oid).Select(grp => grp.FirstOrDefault()).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
                                                {
                                                    if (objsample.Samplelogin.JobID.TAT != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                    {
                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy"), objsample.Samplelogin.JobID.DueDate });
                                                    }
                                                    else if (objsample.Samplelogin.JobID.TAT != null)
                                                    {
                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), objsample.Samplelogin.JobID.TAT.TAT, Convert.ToDateTime(objsample.Samplelogin.JobID.DueDate).ToString("MM/dd/yy"), null, objsample.Samplelogin.JobID.DueDate });
                                                    }
                                                    else if (objsample.Samplelogin.JobID.RecievedDate != null && objsample.Samplelogin.JobID.RecievedDate != DateTime.MinValue)
                                                    {
                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, Convert.ToDateTime(objsample.Samplelogin.JobID.RecievedDate).ToString("MM/dd/yy"), null });
                                                    }
                                                    else
                                                    {
                                                        table.Rows.Add(new object[] { objsample.Samplelogin.JobID.JobID, samples.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null && a.TestHold == false && a.Samplelogin.JobID.Oid == objsample.Samplelogin.JobID.Oid).GroupBy(p => p.Samplelogin.Oid).Select(grp => grp.FirstOrDefault()).Count(), null, null, null, null });
                                                    }

                                                    //  table.Columns.Add("SortableDueDate", typeof(DateTime));
                                                }
                                                //foreach (DataRow row in table.Rows)
                                                //{
                                                //    DateTime dueDate = DateTime.ParseExact(row["DueDate"].ToString(), "MM/dd/yy", CultureInfo.InvariantCulture);
                                                //    row["SortableDueDate"] = dueDate;
                                                //}
                                                DataView dv = new DataView(table);

                                                dv.Sort = "SortableDueDate ASC";

                                                table.Columns.Remove("SortableDueDate");

                                                // DataView dv = new DataView(table);
                                                //  dv.Sort = "DueDate Asc";
                                                table = dv.ToTable();
                                            }
                                            else if (samples == null && batch.Jobid != null)
                                            {
                                                string[] ids = batch.Jobid.Split(';');
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
                                SamplePrepBatch batch = (SamplePrepBatch)View.CurrentObject;
                                if (batch != null)
                                {
                                    if (gridLookup != null)
                                    {
                                        gridLookup.JSProperties["cpNPInstrument"] = batch.strInstrument;
                                        gridLookup.ClientInstanceName = propertyEditor.Id;
                                        gridLookup.ClientSideEvents.Init = @"function(s,e) 
                                        {
                                        s.SetText(s.cpNPInstrument);
                                        //var nav = document.getElementById('LPcell');
                                        //var sep = document.getElementById('separatorCell');                      
                                        //if(nav != null && sep != null) 
                                        //{
                                        //    var totusablescr = (((screen.width - (sep.offsetWidth + nav.offsetWidth)) / 100) * 84) - (286); 
                                        //    s.SetWidth(totusablescr / 3);
                                        //}
                                        //else 
                                        //{
                                        //    s.SetWidth(220); 
                                        //}                      
                                        }";
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
                                        if (batch.Test != null && batch.Method != null)
                                        {
                                            string[] guidStringstest = batch.Test.Split(';');
                                            string[] guidStringsmatrix = batch.Matrix.Split(';');
                                            string[] guidStringsmethod = batch.Method.Split(';');
                                            List<Guid> lstTestOid = guidStringstest.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                                            List<Guid> lstMatrixOid = guidStringsmatrix.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                                            List<Guid> lstMethodOid = guidStringsmethod.Where(guidString => Guid.TryParse(guidString, out _)).Select(guidString => Guid.Parse(guidString)).Distinct().ToList();
                                            IList<TestMethod> lst = View.ObjectSpace.GetObjects<TestMethod>(new GroupOperator(GroupOperatorType.And, new InOperator("MatrixName.Oid", lstMatrixOid),new InOperator("Oid", lstTestOid), new InOperator("MethodName.Oid", lstMethodOid)));
                                            foreach (TestMethod objTest in lst.ToList())
                                            {
                                                foreach (Labware objlab in objTest.Labwares.OrderBy(a => a.AssignedName).ToList())
                                                {
                                                    table.Rows.Add(new object[] { objlab.Oid, objlab.LabwareName, objlab.AssignedName });
                                                }
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
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (View.Id == "QCType_ListView_SamplePrepBatchSequence")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                    gridListEditor.Grid.Settings.ShowHeaderFilterButton = false;
                    gridListEditor.Grid.ClientInstanceName = "QCType";
                    gridListEditor.Grid.Load += Grid_Load;

                }
                else if (View.Id == "QCType_ListView_SamplePrepBatchSequence_History")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                    gridListEditor.Grid.Settings.ShowHeaderFilterButton = false;
                    gridListEditor.Grid.Load += Grid_Load1;

                }
                else if (View.Id == "SamplePrepBatchSequence_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("SamplePrep", this);
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.Settings.ShowHeaderFilterButton = false;
                    gridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                    gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    //gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.ClientInstanceName = "SamplePrepBatchSequence";
                    ActionContainerViewItem qcaction2 = ((DetailView)Application.MainWindow.View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                    if (qcaction2 != null && (qcaction2.Actions[1].Caption == "Sort" || qcaction2.Actions[1].Caption == "序号"))
                    {
                        //if (gridListEditor.Grid != null && gridListEditor != null)
                        //{
                        //    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = false;
                        //}
                        gridListEditor.Grid.JSProperties["cpIsSeleted"] = true;
                    }
                    else if (qcaction2 != null && (qcaction2.Actions[1].Caption == "Ok" || qcaction2.Actions[1].Caption == "确定"))
                    {
                        //if (gridListEditor.Grid != null && gridListEditor != null)
                        //{
                        //    gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        //}
                        gridListEditor.Grid.JSProperties["cpIsSeleted"] = false;
                    }
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                           {
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
                            if (s.IsRowSelectedOnPage(e.elementIndex))  
                            { 
                                var FocusedColumn = sessionStorage.getItem('PrevFocusedColumn');                                
                                var oid;
                                var text;
                                if(FocusedColumn.includes('.'))
                                {                                       
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText; 
                                    if (e.item.name =='CopyToAllCell' )
                                    {
                                       if (FocusedColumn=='FinalVolume' || FocusedColumn=='TakenSampleUnit' || FocusedColumn=='SampleAmount')
	                                   {
		                                 for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                               s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);   
                                            }
                                         } 
	                                   }
                                     }        
                                 }
                                 else
                                 { 
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn); 
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        if (FocusedColumn=='FinalVolume' || FocusedColumn=='TakenSampleUnit' || FocusedColumn=='SampleAmount')
                                        {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                                s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                            }
                                        }
                                      }
                                    }                            
                                 }
                             }
                             e.processOnServer = false;
                        }";
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                     if(s.cpIsSeleted == true)
                    {
                      if (e.visibleIndex != '-1')
                      {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'SamplePrep', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'SamplePrep', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'SamplePrep', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'SamplePrep', 'UNSelectall', '', false);                        
                      }
                 }
                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                    window.setTimeout(function() {  
                    var Dilutioncount = s.batchEditApi.GetCellValue(e.visibleIndex, 'DilutionCount');
                    var Dilution = s.batchEditApi.GetCellValue(e.visibleIndex, 'DF');
                    var SysSampleCode = s.batchEditApi.GetCellValue(e.visibleIndex, 'SYSSamplecode');
                    var fieldName = sessionStorage.getItem('PrevFocusedColumn');
                    if(fieldName=='DilutionCount')
                    {
                        if (e.visibleIndex != '-1')
                       {
                        s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) 
                        {
                            if(Dilutioncount >= 0)
                                {
                                      var dilutioncount = sessionStorage.getItem('Dilutioncount');
                                      if(s.batchEditApi.HasChanges(e.visibleIndex,dilutioncount))
                                      {
                                         console.log(dilutioncount);
                                         RaiseXafCallback(globalCallbackControl,'SamplePrep' , 'DilutionCnt|'+ Oidvalue+'|'+Dilutioncount, '', false);
                                         s.batchEditApi.SetCellValue(e.visibleIndex, 'Dilutioncount', dilutioncount);
                                      }
                                      else if(Dilutioncount == 0 || Dilutioncount == 1)
                                      {
                                            console.log('A');
                                            RaiseXafCallback(globalCallbackControl,'SamplePrep' , 'DilutionCnt|'+ Oidvalue+'|'+Dilutioncount, '', false);
                                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Dilutioncount', dilutioncount);
                                      }
                                }
                                if(Dilution != null)
                                {
                                    RaiseXafCallback(globalCallbackControl,'SamplePrep' , 'StringDilution|'+ Oidvalue+'|'+Dilution, '', false);
                                }
                        });
                    }
                    }
                    //if (s.batchEditApi.HasChanges()) { 
                    //               s.UpdateEdit();  
                    //             } 
                    }, 20);}";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e)
                            {
                                var string1 =sessionStorage.getItem('PrevFocusedColumn')
                                string1 = string1.trim();
                                if(sessionStorage.getItem('PrevFocusedColumn') == null || sessionStorage.getItem('PrevFocusedColumn')=='' || string1.length == 0)
                                {
                                   sessionStorage.setItem('PrevFocusedColumn',e.focusedColumn.fieldName);
                                }
                            }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditConfirmShowing = @"function(s,e) 
                    { 
                        e.cancel = true;
                    }";
                    //gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                    //        s.timerHandle = setTimeout(function() {  
                    //             if (s.batchEditApi.HasChanges()) {  
                    //               s.UpdateEdit();  
                    //             } 
                    //           }, 20);}";



                }
                else if (View.Id == "SamplePrepBatchSequence_ListView_History")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.KeyboardSupport = true;
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Load += Grid_Load1;
                        //gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        gridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                        gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                           {
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
                            if (s.IsRowSelectedOnPage(e.elementIndex))  
                            { 
                                var FocusedColumn = sessionStorage.getItem('PrevFocusedColumn');                                
                                var oid;
                                var text;
                                if(FocusedColumn.includes('.'))
                                {                                       
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText; 
                                    if (e.item.name =='CopyToAllCell' )
                                    {
                                       if (FocusedColumn=='FinalVolume' || FocusedColumn=='TakenSampleUnit' || FocusedColumn=='SampleAmount')
	                                   {
		                                 for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                         { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                               s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);   
                                            }
                                         } 
	                                  }
                                     }        
                                 }
                                 else
                                 { 
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn); 
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        if (FocusedColumn=='FinalVolume' || FocusedColumn=='TakenSampleUnit' || FocusedColumn=='SampleAmount')
                                        {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                                s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                            }
                                        }
                                      }
                                    }                            
                                 }
                             }
                             e.processOnServer = false;
                        }";
                        if (Application.MainWindow.View is DetailView)
                        {
                            if (((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit && sampleprepbatchinfo.SamplepreparationWrite)
                            {
                                gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                              {
                                              e.cancel = false;
                                              }
                                      }";
                            }
                            else
                            {
                                gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                              {
                                              e.cancel = true;
                                              }
                                      }";
                            }
                        }
                    }
                }
                else if (View.Id == "SamplePrepBatch_DetailView_Copy")
                {
                    callbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    callbackManager.CallbackManager.RegisterHandler("CanCloseView", this);

                    //DashboardViewItem sampleprepdetail = ((DetailView)View).FindItem("sampleprepdetail") as DashboardViewItem;
                    //DashboardViewItem qctype = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                    //DashboardViewItem samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                    //SamplePrepBatch batch = (SamplePrepBatch)((DetailView)View).CurrentObject;

                    SamplePrepLoad.SetClientScript("sessionStorage.setItem('AllowSelectionByDataCell', false);");
                    SamplePrepSort.SetClientScript("sessionStorage.setItem('AllowSelectionByDataCell', true);");
                }
                else if (View.Id == "SamplePrepBatch_ListView_CopyFrom")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = true;
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    }
                }
                //else if (View.Id == "SamplePrepBatch_Reagents_ListView" || View.Id == "SamplePrepBatch_Instruments_ListView")
                //{
                //    Frame.GetController<NewObjectViewController>().NewObjectAction.Enabled.SetItemValue("Key", sampleprepbatchinfo.IsEnbStat);
                //}
                else if (View.Id == "TestMethod_ListView_PrepQueue")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        if (gridListEditor.Grid.Columns["NoSamples"] != null)
                        {
                            gridListEditor.Grid.Columns["NoSamples"].CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                        }
                    }
                }
                else if (View.Id == "SamplePrepBatch_DetailView_Copy_History" ||View.Id== "SamplePrepBatch_DetailView_History")
                {
                    callbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    callbackManager.CallbackManager.RegisterHandler("CanDeletePrpID", this);

                    if (((DetailView)View).ViewEditMode == ViewEditMode.Edit && sampleprepbatchinfo.SamplepreparationWrite)
                    {
                        SavePrepBatch.Active.SetItemValue("View", true);
                        SamplePrepAddSamples.Active.SetItemValue("ShowAddSample", true);
                        btnCopyFromSamplePrepAction.Active["ShowCopyFrom"] = true;
                    }
                    else
                    {
                        SavePrepBatch.Active.SetItemValue("View", false);
                        SamplePrepAddSamples.Active.SetItemValue("ShowAddSample", false);
                        btnCopyFromSamplePrepAction.Active["ShowCopyFrom"] = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                NestedFrame nestedFrame = (NestedFrame)Frame;
                CompositeView view = nestedFrame.ViewItem.View;
                ActionContainerViewItem sampleprepaction0 = null;
                ActionContainerViewItem sampleprepaction2 = null;
                sampleprepaction0 = ((DetailView)view).FindItem("sampleprepaction0") as ActionContainerViewItem;
                sampleprepaction2 = ((DetailView)view).FindItem("sampleprepaction2") as ActionContainerViewItem;
                if (sampleprepaction2.Actions[1].Caption == "Sort" || sampleprepaction2.Actions[1].Caption == "序号")
                {
                    ASPxGridView gridView = sender as ASPxGridView;
                    e.Properties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Grid_Load1(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "QCType_ListView_SamplePrepBatchSequence_History")
                {
                    GridViewColumn AddQC = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "SamplePrepAdd").ToList()[0];
                    if (AddQC != null)
                    {
                        AddQC.Width = 40;
                        if (Application.MainWindow.View is DetailView && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit && sampleprepbatchinfo.SamplepreparationWrite)
                        {
                            AddQC.Visible = true;
                        }
                        else
                        {
                            AddQC.Visible = false;
                        }
                    }
                    gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridView.Settings.VerticalScrollableHeight = 320;
                }
                else
                {

                    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    gridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridView.Settings.VerticalScrollableHeight = 320;
                    if (gridView.Columns["InlineEditCommandColumn"] != null)
                    {
                        gridView.Columns["InlineEditCommandColumn"].Visible = false;
                    }
                    if (gridView.Columns["SelectionCommandColumn"] != null)
                    {
                        gridView.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    GridViewColumn QCRemoves = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "SamplePrepRemove").ToList()[0];
                    if (QCRemoves != null)
                    {
                        QCRemoves.Width = 40;
                        QCRemoves.FixedStyle = GridViewColumnFixedStyle.Left;
                        if (Application.MainWindow.View is DetailView && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit && sampleprepbatchinfo.SamplepreparationWrite)
                        {
                            QCRemoves.Visible = true;
                        }
                        else
                        {
                            QCRemoves.Visible = false;
                        }
                    }
                    if (gridView.Columns["QCType"] != null)
                    {
                        gridView.Columns["QCType"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.Columns["JobID"] != null)
                    {
                        gridView.Columns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.Columns["StrSampleID"] != null)
                    {
                        gridView.Columns["StrSampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    }
                    if (gridView.Columns["SYSSamplecode"] != null)
                    {
                        gridView.Columns["SYSSamplecode"].Width = 150;
                    }
                    if (gridView.Columns["FunctionType"] != null)
                    {
                        gridView.Columns["FunctionType"].Width = 150;
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
                ASPxGridView gridView = sender as ASPxGridView;
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");

                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";
                    e.Items.Remove(e.Items.FindByText("Edit"));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
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
        private void GridLookup_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridLookup grid = sender as ASPxGridLookup;
                if (grid != null && grid.GridView != null)
                {
                    if (grid.GridView.KeyFieldName == "JobID")
                    {
                        ((SamplePrepBatch)View.CurrentObject).Jobid = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("JobID"));
                    }
                    else
                    {
                        ((SamplePrepBatch)View.CurrentObject).Instrument = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("Oid"));
                        ((SamplePrepBatch)View.CurrentObject).strInstrument = string.Join(";", ((ASPxGridLookup)sender).GridView.GetSelectedFieldValues("InstrumentID"));
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
                if (View != null && View.Id == "SamplePrepBatch_DetailView_Copy")
                {
                    if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Test")
                    {
                        SamplePrepBatch objsampleprepbat = (SamplePrepBatch)View.CurrentObject;
                        if (objsampleprepbat != null)
                        {
                            objsampleprepbat.Method = null;
                        }
                    }
                    else if (View != null && View.CurrentObject == e.Object && e.PropertyName == "Matrix")
                    {
                        SamplePrepBatch objsampleprepbat = (SamplePrepBatch)View.CurrentObject;
                        if (objsampleprepbat != null)
                        {
                            objsampleprepbat.Method = null;
                            objsampleprepbat.Test = null;
                        }
                    }
                }
                else if (View.Id == "Reagent_DetailView" && e.PropertyName == "ExpiredDate" && e.OldValue != e.NewValue)
                {
                    Reagent objReagent = (Reagent)e.Object;
                    if (objReagent.ExpiredDate != DateTime.MinValue && objReagent.ExpiredDate < DateTime.Today)
                    {
                        Application.ShowViewStrategy.ShowMessage("Expiry date must be greater than the current date.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        objReagent.ExpiredDate = null;
                        return;
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
                if (View.Id == "SamplePrepBatch_ListView_CopyFrom")
                {
                    sampleprepbatchinfo.CopyFromPrepBatchSource = ((ListView)View).SelectedObjects.Cast<SamplePrepBatch>().FirstOrDefault();
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
                if (View.Id == "QCType_ListView_SamplePrepBatchSequence")
                {
                    GridViewColumn AddQC = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "SamplePrepAdd").ToList()[0];
                    if (AddQC != null)
                    {
                        AddQC.Width = 40;
                        if (sampleprepbatchinfo.canfilter)
                        {
                          //  gridView.Columns["QCType"].Visible = false;
                            AddQC.Visible = false;
                        }
                    }
                    gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridView.Settings.VerticalScrollableHeight = 320;
                }
                else
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    CompositeView view = nestedFrame.ViewItem.View;
                    ActionContainerViewItem sampleprepaction0 = null;
                    ActionContainerViewItem sampleprepaction2 = null;
                    if (view is DashboardView)
                    {
                        sampleprepaction0 = ((DashboardView)view).FindItem("sampleprepaction0") as ActionContainerViewItem;
                        sampleprepaction2 = ((DashboardView)view).FindItem("sampleprepaction2") as ActionContainerViewItem;
                    }
                    else
                    {
                        sampleprepaction0 = ((DetailView)view).FindItem("sampleprepaction0") as ActionContainerViewItem;
                        sampleprepaction2 = ((DetailView)view).FindItem("sampleprepaction2") as ActionContainerViewItem;
                    }
                    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    gridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridView.Settings.VerticalScrollableHeight = 320;
                    if (gridView.Columns["InlineEditCommandColumn"] != null)
                    {
                        gridView.Columns["InlineEditCommandColumn"].Visible = false;
                    }
                    if (sampleprepaction2.Actions[1].Caption == "Sort" || sampleprepaction2.Actions[1].Caption == "序号")
                    {
                        if (gridView.Columns["DilutionCount"] != null)
                        {
                            gridView.Columns["DilutionCount"].Visible = true;
                            gridView.Columns["DilutionCount"].Width = 100;
                            gridView.Columns["DilutionCount"].SetColVisibleIndex(4);
                        }
                    }
                    else
                    {
                        if (gridView.Columns["DilutionCount"] != null)
                        {
                            gridView.Columns["DilutionCount"].Visible = false;
                        }
                    }
                    if (gridView.Columns["SelectionCommandColumn"] != null)
                    {
                        if (sampleprepbatchinfo.canfilter)
                        {
                            gridView.Columns["SelectionCommandColumn"].Width = 40;
                            gridView.Columns["DilutionCount"].Visible = false;
                            gridView.Columns["DF"].Visible = true;
                            gridView.Columns["DF"].Width = 70;
                            gridView.Columns["DF"].SetColVisibleIndex(5);
                            if (!string.IsNullOrEmpty(sampleprepbatchinfo.strSamplePrepID))
                            {
                                gridView.Columns["SelectionCommandColumn"].Visible = false;
                            }
                            else
                            {
                                gridView.Columns["SelectionCommandColumn"].Visible = true;
                                gridView.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.Columns["DilutionCount"].Visible = true;
                                gridView.Columns["DilutionCount"].Width = 100;
                                gridView.Columns["DilutionCount"].SetColVisibleIndex(4);
                                gridView.Columns["DF"].Visible = false;
                              
                            }
                        }
                        else
                        {
                            gridView.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                    }
                    GridViewColumn QCRemoves = gridView.Columns.Cast<GridViewColumn>().Where(i => i.Name == "SamplePrepRemove").ToList()[0];
                    if (QCRemoves != null)
                    {
                        QCRemoves.Width = 40;
                        if (sampleprepbatchinfo.canfilter)
                        {
                            QCRemoves.Visible = false;
                            QCRemoves.FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        else
                        {
                            QCRemoves.FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                    }

                    if (gridView.Columns["QCType"] != null)
                    {
                        gridView.Columns["QCType"].FixedStyle = GridViewColumnFixedStyle.Left;
                        //gridView.Columns["QCType"].VisibleIndex = 1;
                    }
                    if (gridView.Columns["JobID"] != null)
                    {
                        gridView.Columns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                        //gridView.Columns["JobID"].VisibleIndex = 2;
                    }
                    if (gridView.Columns["StrSampleID"] != null)
                    {
                        gridView.Columns["StrSampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                        //gridView.Columns["StrSampleID"].VisibleIndex = 3;
                    }
                    if (gridView.Columns["FunctionType"] != null)
                    {
                        gridView.Columns["FunctionType"].Width = 150;
                    }
                    if (sampleprepbatchinfo.canfilter)
                    {
                        sampleprepbatchinfo.canfilter = false;
                        if (!string.IsNullOrEmpty(sampleprepbatchinfo.strSamplePrepID))
                        {
                            gridView.ClearSort();
                            if (gridView.Columns["Sort"] != null)
                            {
                                gridView.SortBy(gridView.Columns["Sort"], ColumnSortOrder.Ascending);
                            }
                            sampleprepaction0.Actions[0].Enabled.SetItemValue("key", false);
                            sampleprepaction0.Actions[1].Enabled.SetItemValue("key", false);
                            sampleprepaction2.Actions[0].Enabled.SetItemValue("key", false);
                            sampleprepaction2.Actions[1].Enabled.SetItemValue("key", false);
                            gridView.Columns["Sort"].Visible = false;
                        }
                        else
                        {
                            gridView.ClearSort();
                            if (sampleprepbatchinfo.SamplepreparationWrite)
                            {
                                sampleprepaction0.Actions[0].Enabled.SetItemValue("key", true);
                                sampleprepaction0.Actions[1].Enabled.SetItemValue("key", true);
                                sampleprepaction2.Actions[0].Enabled.SetItemValue("key", false);
                                sampleprepaction2.Actions[1].Enabled.SetItemValue("key", true);
                            }
                            else
                            {
                                sampleprepaction0.Actions[0].Enabled.SetItemValue("key", false);
                                sampleprepaction0.Actions[1].Enabled.SetItemValue("key", false);
                                sampleprepaction2.Actions[0].Enabled.SetItemValue("key", false);
                                sampleprepaction2.Actions[1].Enabled.SetItemValue("key", false);
                            }
                            if (gridView.Columns["Sort"] != null)
                            {
                                gridView.Columns["Sort"].Visible = true;
                                gridView.Columns["Sort"].VisibleIndex = 5;
                                gridView.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                    }
                    else
                    {
                        if (gridView.Columns["Sort"] != null)
                        {
                            if (SamplePrepSort.Caption == "Sort" || SamplePrepSort.Caption == "序号")
                            {
                                gridView.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                                //gridView.Columns["Sort"].VisibleIndex = 4; 
                            }
                            else
                            {
                                //gridView.Columns["Sort"].VisibleIndex = -1;
                                gridView.Columns["Sort"].Visible = false;
                            }
                        }
                    }
                    if (gridView.Columns["SYSSamplecode"] != null)
                    {
                        gridView.Columns["SYSSamplecode"].Width = 150;
                    }
                    if (sampleprepaction2.Actions[1].Caption == "Sort" || sampleprepaction2.Actions[1].Caption == "序号")
                    {
                        gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
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
                NestedFrame nestedFrame = (NestedFrame)Frame;
                CompositeView view = nestedFrame.ViewItem.View;
                ActionContainerViewItem sampleprepaction2 = null;
                if (view is DashboardView)
                {
                    sampleprepaction2 = ((DashboardView)view).FindItem("sampleprepaction2") as ActionContainerViewItem;
                }
                else
                {
                    sampleprepaction2 = ((DetailView)view).FindItem("sampleprepaction2") as ActionContainerViewItem;
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
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                if (View.Id == "SamplePrepBatch_DetailView_Copy")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
                    Frame.GetController<ModificationsController>().SaveAction.Active.RemoveItem("ShowSave");
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.RemoveItem("ShowSaveAndClose");
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.RemoveItem("ShowSaveAndNew");
                    Frame.GetController<ModificationsController>().CancelAction.Active.RemoveItem("ShowCancel");
                    Frame.GetController<NewObjectViewController>().Active.RemoveItem("ShowNew");
                    Frame.GetController<RefreshController>().RefreshAction.Executing -= RefreshAction_Executing;
                    WebModificationsController webModificationsController = Frame.GetController<WebModificationsController>();
                    if (webModificationsController != null)
                    {
                        if (webModificationsController.EditAction.Active.Contains("ShowEdit"))
                        {
                            webModificationsController.EditAction.Active.RemoveItem("ShowEdit");
                        }
                    }
                    //SavePrepBatch.Active.SetItemValue("View", false);
                    IsSaveaction = false;
                    SamplePrepPrevious.Enabled.RemoveItem("key");
                    ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Remarks") as ASPxRichTextPropertyEditor;
                    if (RichText != null)
                    {
                        RichText.ControlCreated -= RichText_ControlCreated;
                    }
                    CNInfo.SCoidValue = Guid.Empty;
                    CNInfo.SPJobId = null;
                    CNInfo.SPSampleMatries = null;
                }
                else if (View.Id == "SamplePrepBatch_DetailView_Copy_History" ||View.Id== "SamplePrepBatch_DetailView_History")
                {
                    DevExpress.Persistent.Validation.RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
                    ASPxRichTextPropertyEditor RichText = ((DetailView)View).FindItem("Remarks") as ASPxRichTextPropertyEditor;
                    if (RichText != null)
                    {
                        RichText.ControlCreated -= RichText_ControlCreated;
                    }
                    Frame.GetController<ModificationsController>().SaveAction.Active.RemoveItem("ShowSave");
                    Frame.GetController<ModificationsController>().SaveAndCloseAction.Active.RemoveItem("ShowSaveAndClose");
                    Frame.GetController<ModificationsController>().SaveAndNewAction.Active.RemoveItem("ShowSaveAndNew");
                    Frame.GetController<ModificationsController>().CancelAction.Active.RemoveItem("ShowCancel");
                    Frame.GetController<NewObjectViewController>().Active.RemoveItem("ShowNew");
                }
                else if (View.Id == "SamplePrepBatch_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;

                    //Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
                else if (View.Id == "SamplePrepBatchSequence_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load -= Grid_Load;
                        gridListEditor.Grid.FillContextMenuItems -= Grid_FillContextMenuItems;
                    }
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                else if (View.Id == "QCType_ListView_SamplePrepBatchSequence")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load -= Grid_Load;
                    }
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                else if (View.Id == "SamplePrepBatch_ListView_CopyFrom")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.SelectionChanged -= Grid_SelectionChanged;
                    }
                }
                else if (View.Id == "TestMethod_ListView_PrepQueue" || View.Id == "TestMethod_PrepMethods_ListView_PrepQueue")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                    if (View.Id == "TestMethod_PrepMethods_ListView_PrepQueue")
                    {
                        Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    }
                }
                else if (View.Id == "QCType_ListView_SamplePrepBatchSequence_History")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load -= Grid_Load1;
                    }
                }
                else if (View.Id == "SamplePrepBatchSequence_ListView_History")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load -= Grid_Load1;
                    }
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesNotificationController");
                }
                //else if (View.Id == "Labware_LookupListView")
                //{
                //    View.ControlsCreated -= View_ControlsCreated;
                //}
                //else if (View.Id == "Reagent_LookupListView")
                //{
                //    View.ControlsCreated -= View_ControlsCreated;
                //}
                if (sampleprepbatchinfo.lststrseqdilutioncount != null && sampleprepbatchinfo.lststrseqdilutioncount.Count > 0)
                {
                    sampleprepbatchinfo.lststrseqdilutioncount.Clear();
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
                if (View.Id == "TestMethod_PrepMethods_ListView_PrepQueue")
                {
                    ListViewProcessCurrentObjectController targetController = Frame.GetController<ListViewProcessCurrentObjectController>();
                    targetController.ProcessCurrentObjectAction.Enabled["HideDetailview"] = false;
                }
                else
                {
                    List<TestMethod> lstTest = View.SelectedObjects.Cast<TestMethod>().ToList();
                    Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                    sampleprepbatchinfo.SamplepreparationWrite = false;
                    sampleprepbatchinfo.CanAutopopulateSampleGrid = false;
                    if (user.Roles != null && user.Roles.Count > 0)
                    {
                        if (user.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            //TestMethod objTest = (TestMethod)e.InnerArgs.CurrentObject;
                            PrepMethod objTest = (PrepMethod)e.InnerArgs.CurrentObject;
                            IObjectSpace os = Application.CreateObjectSpace();
                            Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch objPrepBatch = os.CreateObject<SamplePrepBatch>();
                            //objPrepBatch.Matrix = os.GetObject<Matrix>(objTest.TestMethod.MatrixName);
                            //objPrepBatch.Test = os.GetObjectByKey<TestMethod>(objTest.TestMethod.Oid);
                            //objPrepBatch.Method = os.GetObjectByKey<TestMethod>(objTest.TestMethod.MethodName.Oid);
                            objPrepBatch.Matrix = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.MatrixName != null).Select(i => i.MatrixName.Oid).Distinct().ToList()));
                            objPrepBatch.Test = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestName != null).Select(i => i.Oid).Distinct().ToList())); ;
                            objPrepBatch.Method = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.MethodName != null).Select(i => i.MethodName.Oid).Distinct().ToList()));
                            objPrepBatch.Tier = objTest.Tier;
                            DetailView dv = Application.CreateDetailView(os, "SamplePrepBatch_DetailView_Copy", true, objPrepBatch);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            e.InnerArgs.ShowViewParameters.CreatedView = dv;
                            e.Handled = true;
                            sampleprepbatchinfo.SamplepreparationWrite = true;
                        }
                        else
                        {
                            foreach (RoleNavigationPermission role in user.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplePreparation" && i.Write == true) != null)
                                {
                                    TestMethod objTest = (TestMethod)e.InnerArgs.CurrentObject;
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch objPrepBatch = os.CreateObject<SamplePrepBatch>();
                                    //objPrepBatch.Matrix = os.GetObject<Matrix>(objTest.MatrixName);
                                    //objPrepBatch.Test = os.GetObjectByKey<TestMethod>(objTest.Oid);
                                    //objPrepBatch.Method = os.GetObjectByKey<TestMethod>(objTest.Oid);
                                    objPrepBatch.Matrix = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.MatrixName != null).Select(i => i.MatrixName.Oid).Distinct().ToList()));
                                    objPrepBatch.Test = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestName != null).Select(i => i.Oid).Distinct().ToList())); ;
                                    objPrepBatch.Method = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.MethodName != null).Select(i => i.MethodName.Oid).Distinct().ToList()));
                                    DetailView dv = Application.CreateDetailView(os, "SamplePrepBatch_DetailView_Copy", true, objPrepBatch);
                                    dv.ViewEditMode = ViewEditMode.Edit;
                                    e.InnerArgs.ShowViewParameters.CreatedView = dv;
                                    e.Handled = true;
                                    sampleprepbatchinfo.SamplepreparationWrite = true;
                                    break;
                                }
                            }
                            if (!sampleprepbatchinfo.SamplepreparationWrite)
                            {
                                TestMethod objTest = (TestMethod)e.InnerArgs.CurrentObject;
                                IObjectSpace os = Application.CreateObjectSpace();
                                Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch objPrepBatch = os.CreateObject<SamplePrepBatch>();
                                //objPrepBatch.Matrix = os.GetObject<Matrix>(objTest.MatrixName);
                                //objPrepBatch.Test = os.GetObjectByKey<TestMethod>(objTest.Oid);
                                //objPrepBatch.Method = os.GetObjectByKey<TestMethod>(objTest.Oid);
                                objPrepBatch.Matrix = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.MatrixName != null).Select(i => i.MatrixName.Oid).Distinct().ToList()));
                                objPrepBatch.Test = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.TestName != null).Select(i => i.Oid).Distinct().ToList())); ;
                                objPrepBatch.Method = string.Format("{0}", string.Join("; ", lstTest.Where(i => i.MethodName != null).Select(i => i.MethodName.Oid).Distinct().ToList()));
                                DetailView dv = Application.CreateDetailView(os, "SamplePrepBatch_DetailView_Copy", true, objPrepBatch);
                                e.InnerArgs.ShowViewParameters.CreatedView = dv;
                                dv.ViewEditMode = ViewEditMode.View;
                                e.Handled = true;
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
                if (!string.IsNullOrEmpty(parameter))
                {
                    if (bool.TryParse(parameter, out bool CanCloseView))
                    {
                        if (sampleprepbatchinfo.IsDeletePreID && CanCloseView)
                        {
                            SamplePrepBatch batch = (SamplePrepBatch)Application.MainWindow.View.CurrentObject;
                            if (batch != null)
                            {
                                SamplePrepBatch objBatch = ObjectSpace.GetObject(batch);
                                ObjectSpace.Delete(objBatch);
                                ObjectSpace.CommitChanges();
                                Frame.SetView(Application.CreateListView(typeof(SamplePrepBatch), true));
                                sampleprepbatchinfo.IsDeletePreID = false;
                            }
                        }
                        else
                        {
                            if (CanCloseView)
                            {
                                DetailView sampleprepdetail = null;
                                DashboardViewItem samplepreplist = null;
                                if (View is DashboardView)
                                {
                                    DashboardViewItem dvSamplePrep = ((DashboardView)View).FindItem("sampleprepdetail") as DashboardViewItem;
                                    if (dvSamplePrep != null && dvSamplePrep.InnerView != null)
                                    {
                                        sampleprepdetail = dvSamplePrep.InnerView as DetailView;
                                        samplepreplist = ((DashboardView)View).FindItem("samplepreplist") as DashboardViewItem;
                                    }
                                }
                                else if (View is DetailView)
                                {
                                    sampleprepdetail = View as DetailView;
                                    samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                                }

                                if (samplepreplist != null && sampleprepdetail != null && samplepreplist.InnerView != null)
                                {
                                    if (View is DashboardView)
                                    {
                                        sampleprepdetail.ObjectSpace.CommitChanges();
                                    }
                                    else
                                    {
                                        ((ASPxGridListEditor)((ListView)samplepreplist.InnerView).Editor).Grid.UpdateEdit();
                                        ObjectSpace.CommitChanges();
                                    }
                                    SamplePrepBatch batch = samplepreplist.InnerView.ObjectSpace.GetObject<SamplePrepBatch>((SamplePrepBatch)sampleprepdetail.CurrentObject);
                                    if (batch != null)
                                    {
                                        foreach (SamplePrepBatchSequence sequence in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().OrderBy(a => a.Sort).ToList())
                                        {
                                            sequence.SamplePrepBatchDetail = batch;
                                        }
                                        if (!sampleprepdetail.ObjectSpace.IsNewObject(batch))
                                        {
                                            IList<SamplePrepBatchSequence> seq = samplepreplist.InnerView.ObjectSpace.GetObjects<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.PrepBatchID]=?", batch.PrepBatchID));
                                            foreach (SamplePrepBatchSequence prepBatch in seq.ToList())
                                            {
                                                if (((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(x => x.Oid == prepBatch.Oid).ToList().Count == 0)
                                                {
                                                    samplepreplist.InnerView.ObjectSpace.Delete(prepBatch);
                                                }
                                            }

                                        }
                                    }
                                    samplepreplist.InnerView.ObjectSpace.CommitChanges();

                                    bool CanCommit = false;
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    List<string> lstMatrixOid = batch.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                    List<string> lstTestOid = batch.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                    List<string> lstMethdOid = batch.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                    List<string> lstTest = new List<string>();
                                    if (lstTestOid != null)
                                    {
                                        foreach (string objOid in lstTestOid)
                                        {
                                            if (!string.IsNullOrEmpty(objOid))
                                            {
                                                TestMethod objTest = ObjectSpace.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                                if (objTest != null && !lstTest.Contains(objTest.TestName))
                                                {
                                                    lstTest.Add(objTest.TestName);
                                                }
                                            }

                                        }
                                    }
                                    List<string> lstMatrix = new List<string>();
                                    if (lstMatrixOid != null)
                                    {
                                        foreach (string objOid in lstMatrixOid)
                                        {
                                            if (!string.IsNullOrEmpty(objOid))
                                            {
                                                Matrix objTest = ObjectSpace.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                                if (objTest != null && !lstMatrix.Contains(objTest.MatrixName))
                                                {
                                                    lstMatrix.Add(objTest.MatrixName);
                                                }
                                            }

                                        }
                                    }
                                    List<string> lstMethod = new List<string>();
                                    if (lstMethdOid != null)
                                    {
                                        foreach (string objOid in lstMethdOid)
                                        {
                                            if (!string.IsNullOrEmpty(objOid))
                                            {
                                                Method objTest = ObjectSpace.GetObjectByKey<Method>(new Guid(objOid.Trim()));
                                                if (lstMethod != null && !lstMethod.Contains(objTest.MethodNumber))
                                                {
                                                    lstMethod.Add(objTest.MethodNumber);
                                                }
                                            }

                                        }
                                    }
                                    List<TestMethod> lst = ObjectSpace.GetObjects<TestMethod>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[PrepMethods][].Count() > 0"), new InOperator("MatrixName.MatrixName", lstMatrix),
                            new InOperator("TestName", lstTest), new InOperator("MethodName.MethodNumber", lstMethod))).ToList();
                                    foreach (SamplePrepBatchSequence sequence in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null && i.QCType.QCTypeName == "Sample" && i.IsDilution == false).OrderBy(a => a.Sort).ToList())
                                    {
                                        IList<SampleParameter> lstSampleParameter = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Samplelogin.Oid] = ? And [SignOff] = True And [IsPrepMethodComplete]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=? ", sequence.SampleID.Oid, batch.Sort - 1), new InOperator("Testparameter.TestMethod.Oid", lst.Select(i => i.Oid))));
                                        foreach (SampleParameter sampleParameter in lstSampleParameter)
                                        {
                                            if (sampleParameter != null)
                                            {
                                                if (string.IsNullOrEmpty(sampleParameter.PrepBatchID))
                                                {
                                                    sampleParameter.PrepBatchID = batch.Oid.ToString();
                                                    sampleParameter.OSSync = true;
                                                }
                                                else
                                                {
                                                    sampleParameter.PrepBatchID = sampleParameter.PrepBatchID + "; " + batch.Oid.ToString();
                                                    sampleParameter.OSSync = true;
                                                }
                                                sampleParameter.PrepMethodCount = sampleParameter.PrepMethodCount + 1;
                                                if (sampleParameter.Testparameter.TestMethod.PrepMethods.Count == sampleParameter.PrepMethodCount)
                                                {
                                                    sampleParameter.IsPrepMethodComplete = true;
                                                }
                                                CanCommit = true;
                                            }
                                        }
                                    }
                                    if (CanCommit)
                                    {
                                        os.CommitChanges();
                                        foreach (Samplecheckin objJobID in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.SampleID != null && i.SampleID.JobID != null).Select(i => i.SampleID.JobID).Distinct().ToList())
                                        {
                                            List<SampleParameter> lstSampleParam = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.TestMethod.PrepMethods][].Count() > 0", objJobID.Oid)).ToList();
                                            Samplecheckin obj = os.GetObjectByKey<Samplecheckin>(objJobID.Oid);
                                            if (lstSampleParam.FirstOrDefault(i => i.IsPrepMethodComplete == false) == null)
                                            {
                                                StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Index]='116'"));
                                                if (status != null)
                                                {
                                                    obj.Index = status;
                                                }
                                            }
                                            else if (lstSampleParam.FirstOrDefault(i => i.PrepMethodCount == 0) == null)
                                            {
                                                StatusDefinition status = os.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID]=15"));
                                                if (status != null)
                                                {
                                                    obj.Index = status;
                                                }
                                            }
                                            //else if(lstSampleParam.FirstOrDefault(i=>i.Testparameter!=null && i.Testparameter.TestMethod!=null && i.Testparameter.TestMethod.PrepMethods!=null && i.Testparameter.TestMethod.PrepMethods.Count!=i.PrepMethodCount && i.PrepMethodCount >0)!=null)
                                            //{

                                            //}
                                        }
                                        os.CommitChanges();
                                        os.Dispose();
                                    }
                                }
                                sampleprepbatchinfo.canfilter = true;
                                //Frame.SetView(Application.CreateListView(typeof(SamplePrepBatch), true));
                                IObjectSpace objectSpace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objectSpace, typeof(PrepMethod));
                                ListView listview = Application.CreateListView("TestMethod_PrepMethods_ListView_PrepQueue", cs, true);
                                Frame.SetView(listview);
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else
                            {
                                SamplePrepBatch batch = (SamplePrepBatch)View.CurrentObject;
                                if (batch != null)
                                {
                                    batch.PrepBatchID = string.Empty;
                                }
                            }
                        }

                    }
                    else
                    {
                        string[] param = parameter.Split('|');
                        if (param[0] == "DilutionCnt")
                        {
                            if (sampleprepbatchinfo.lststrseqdilutioncount == null)
                            {
                                sampleprepbatchinfo.lststrseqdilutioncount = new List<string>();
                            }
                            if (sampleprepbatchinfo.lststrseqdilutioncount.Count == 0)
                            {
                                sampleprepbatchinfo.lststrseqdilutioncount.Add(param[1].ToString() + "|" + param[2].ToString());
                            }
                            else
                            {
                                bool IsNew = true;
                                foreach (string objstrdcnt in sampleprepbatchinfo.lststrseqdilutioncount)
                                {
                                    string[] strcnt = objstrdcnt.Split('|');
                                    if (strcnt[0].Contains(param[1]))
                                    {
                                        sampleprepbatchinfo.lststrseqdilutioncount.Remove(objstrdcnt);
                                        sampleprepbatchinfo.lststrseqdilutioncount.Add(param[1].ToString() + "|" + param[2].ToString());
                                        IsNew = false;
                                        break;
                                    }
                                }
                                if (IsNew)
                                {
                                    sampleprepbatchinfo.lststrseqdilutioncount.Add(param[1].ToString() + "|" + param[2].ToString());
                                }
                            }

                        }
                        else
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            DashboardViewItem samplepreplist = null;
                            if (view is DashboardView)
                            {
                                samplepreplist = ((DashboardView)view).FindItem("samplepreplist") as DashboardViewItem;
                            }
                            else if (view is DetailView)
                            {
                                samplepreplist = ((DetailView)view).FindItem("samplepreplist") as DashboardViewItem;
                            }
                            if (samplepreplist != null && samplepreplist.InnerView != null)
                            {
                                ActionContainerViewItem sampleprepaction2 = null;
                                if (View is DashboardView)
                                {
                                    sampleprepaction2 = ((DashboardView)view).FindItem("sampleprepaction2") as ActionContainerViewItem;
                                }
                                else
                                {
                                    sampleprepaction2 = ((DetailView)view).FindItem("sampleprepaction2") as ActionContainerViewItem;
                                }
                                if (sampleprepaction2.Actions[1].Caption == "Sort" || sampleprepaction2.Actions[1].Caption == "序号")
                                {
                                    if (parameter == "Selectall")
                                    {
                                        int maxsort = 1;
                                        foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().OrderByDescending(i => i.SampleID.JobID.JobID).ThenBy(i => i.SampleID.SampleNo).ToList())
                                        {
                                            sequences.Sort = maxsort;
                                            sequences.batchno = maxsort;
                                            maxsort++;
                                            if (sampleprepbatchinfo.lststrseqdilutioncount != null && sampleprepbatchinfo.lststrseqdilutioncount.Count > 0)
                                            {
                                                foreach (string lststrqcseq in sampleprepbatchinfo.lststrseqdilutioncount.ToList())
                                                {
                                                    string[] strqcseq = lststrqcseq.Split('|');
                                                    if (strqcseq[0].Contains(sequences.Oid.ToString()))
                                                    {
                                                        sequences.DilutionCount = Convert.ToUInt32(strqcseq[1].ToString());
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (parameter == "UNSelectall")
                                    {
                                        foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().ToList())
                                        {
                                            sequences.Sort = 0;
                                            sequences.batchno = 0;
                                            if (sampleprepbatchinfo.lststrseqdilutioncount != null && sampleprepbatchinfo.lststrseqdilutioncount.Count > 0)
                                            {
                                                foreach (string lststrqcseq in sampleprepbatchinfo.lststrseqdilutioncount.ToList())
                                                {
                                                    string[] strqcseq = lststrqcseq.Split('|');
                                                    if (strqcseq[0].Contains(sequences.Oid.ToString()))
                                                    {
                                                        sequences.DilutionCount = Convert.ToUInt32(strqcseq[1].ToString());
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string[] splparm = parameter.Split('|');
                                        if (splparm[0] == "Selected" && !string.IsNullOrEmpty(splparm[1]))
                                        {
                                            int maxsort = 0;
                                            ASPxGridListEditor gridListEditor = ((ListView)samplepreplist.InnerView).Editor as ASPxGridListEditor;
                                            if (gridListEditor != null && gridListEditor.Grid != null)
                                            {
                                                for (int i = 0; i <= gridListEditor.Grid.VisibleRowCount; i++)
                                                {
                                                    int cursort = Convert.ToInt32(gridListEditor.Grid.GetRowValues(i, "Sort"));
                                                    if (maxsort <= cursort)
                                                    {
                                                        maxsort = cursort + 1;
                                                    }
                                                }
                                            }
                                            SamplePrepBatchSequence seq = samplepreplist.InnerView.ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                            if (seq != null && seq.Sort == 0)
                                            {
                                                seq.Sort = maxsort;
                                                seq.batchno = maxsort;
                                                if (sampleprepbatchinfo.lststrseqdilutioncount != null && sampleprepbatchinfo.lststrseqdilutioncount.Count > 0)
                                                {
                                                    foreach (string lststrqcseq in sampleprepbatchinfo.lststrseqdilutioncount.ToList())
                                                    {
                                                        string[] strqcseq = lststrqcseq.Split('|');
                                                        if (strqcseq[0].Contains(splparm[1]))
                                                        {
                                                            seq.DilutionCount = Convert.ToUInt32(strqcseq[1].ToString());
                                                            break;
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        else if (splparm[0] == "UNSelected" && !string.IsNullOrEmpty(splparm[1]))
                                        {
                                            SamplePrepBatchSequence seq = samplepreplist.InnerView.ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[Oid]=?", new Guid(splparm[1])), true);
                                            if (seq != null)
                                            {
                                                foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.Sort > seq.Sort).OrderBy(a => a.SampleID.SampleID).ToList())
                                                {
                                                    sequences.Sort -= 1;
                                                    sequences.batchno -= 1;
                                                }
                                                seq.Sort = 0;
                                                seq.batchno = 0;
                                                if (sampleprepbatchinfo.lststrseqdilutioncount != null && sampleprepbatchinfo.lststrseqdilutioncount.Count > 0)
                                                {
                                                    foreach (string lststrqcseq in sampleprepbatchinfo.lststrseqdilutioncount.ToList())
                                                    {
                                                        string[] strqcseq = lststrqcseq.Split('|');
                                                        if (strqcseq[0].Contains(splparm[1]))
                                                        {
                                                            seq.DilutionCount = Convert.ToUInt32(strqcseq[1].ToString());
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    ((ListView)samplepreplist.InnerView).Refresh();
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

        private void SamplePrepReset_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objectSpace, typeof(PrepMethod));
                ListView listview = Application.CreateListView("TestMethod_PrepMethods_ListView_PrepQueue", cs, true);
                Frame.SetView(listview);
                //DetailView sampleprepdetail = null;
                //if (View is DashboardView)
                //{
                //    DashboardViewItem dvSamplePrepDetail = ((DashboardView)View).FindItem("sampleprepdetail") as DashboardViewItem;
                //    if (dvSamplePrepDetail != null && dvSamplePrepDetail.InnerView != null)
                //    {
                //        sampleprepdetail = dvSamplePrepDetail.InnerView as DetailView;
                //    }
                //}
                //else if (View is DetailView)
                //{
                //    sampleprepdetail = View as DetailView;
                //}
                //SamplePrepBatch batch = (SamplePrepBatch)sampleprepdetail.CurrentObject;
                //if (sampleprepdetail.ObjectSpace.IsNewObject(batch))
                //{
                //    batch.PrepBatchID = string.Empty;
                //    batch.Humidity = null;
                //    //batch.Instrument = null;
                //    ASPxCheckedLookupStringPropertyEditor InstrumentpropertyEditor = sampleprepdetail.FindItem("Instrument") as ASPxCheckedLookupStringPropertyEditor;
                //    if (InstrumentpropertyEditor != null && InstrumentpropertyEditor.Editor != null)
                //    {
                //        ASPxGridLookup spinEdit = (ASPxGridLookup)InstrumentpropertyEditor.Editor;
                //        spinEdit.GridView.Selection.UnselectAll();
                //        InstrumentpropertyEditor.Refresh();
                //    }
                //    ASPxCheckedLookupStringPropertyEditor matrixproperty = sampleprepdetail.FindItem("Matrix") as ASPxCheckedLookupStringPropertyEditor;
                //    if (matrixproperty != null && matrixproperty.Editor != null)
                //    {
                //        ASPxGridLookup spinEdit = (ASPxGridLookup)matrixproperty.Editor;
                //        spinEdit.GridView.Selection.UnselectAll();
                //        matrixproperty.PropertyValue = null;
                //        matrixproperty.Refresh();
                //        matrixproperty.RefreshDataSource();
                //    }
                //    ASPxCheckedLookupStringPropertyEditor methodproperty = sampleprepdetail.FindItem("Method") as ASPxCheckedLookupStringPropertyEditor;
                //    if (methodproperty != null && methodproperty.Editor != null)
                //    {
                //        ASPxGridLookup spinEdit = (ASPxGridLookup)methodproperty.Editor;
                //        spinEdit.GridView.Selection.UnselectAll();
                //        methodproperty.Refresh();
                //    }
                //    batch.Jobid = null;
                //    batch.Matrix = null;
                //    batch.Method = null;
                //    //batch.Noruns = 1;
                //    batch.Temperature = null;
                //    batch.Test = null;
                //    batch.ISShown = true;
                //    disablecontrols(true, sampleprepdetail);
                //    ASPxCheckedLookupStringPropertyEditor TestpropertyEditor = sampleprepdetail.FindItem("Test") as ASPxCheckedLookupStringPropertyEditor;
                //    if (TestpropertyEditor != null && TestpropertyEditor.Editor != null)
                //    {
                //        ASPxGridLookup spinEdit = (ASPxGridLookup)TestpropertyEditor.Editor;
                //        spinEdit.GridView.Selection.UnselectAll();
                //        TestpropertyEditor.Refresh();
                //    }
                //    ActionContainerViewItem sampleprepaction2 = null;
                //    if (View is DashboardView)
                //    {
                //        sampleprepaction2 = ((DashboardView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                //    }
                //    else
                //    {
                //        sampleprepaction2 = ((DetailView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                //    }
                //    sampleprepaction2.Actions[0].Enabled.SetItemValue("key", false);
                //    if (objLanguage.strcurlanguage == "En")
                //    {
                //        sampleprepaction2.Actions[1].Caption = "Sort";
                //    }
                //    else
                //    {
                //        sampleprepaction2.Actions[1].Caption = "序号";
                //    }

                //    DashboardViewItem samplepreplist = null;
                //    if (View is DashboardView)
                //    {
                //        samplepreplist = ((DashboardView)View).FindItem("samplepreplist") as DashboardViewItem;
                //    }
                //    else
                //    {
                //        samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                //    }
                //    if (samplepreplist != null && samplepreplist.InnerView != null)
                //    {
                //        ASPxGridListEditor gridListEditor = ((ListView)samplepreplist.InnerView).Editor as ASPxGridListEditor;
                //        if (gridListEditor != null && gridListEditor.Grid != null)
                //        {
                //            gridListEditor.Grid.ClearSort();
                //            foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                //            {
                //                if (column.Name == "SelectionCommandColumn")
                //                {
                //                    column.Visible = true;
                //                }
                //                else if (column.Name == "SamplePrepRemove")
                //                {
                //                    column.Visible = false;
                //                }
                //            }
                //            if (gridListEditor.Grid.Columns["Sort"] != null)
                //            {
                //                gridListEditor.Grid.Columns["Sort"].Visible = true;
                //                gridListEditor.Grid.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                //            }
                //        }
                //        foreach (SamplePrepBatchSequence sequence in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().ToList())
                //        {
                //            samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequence);
                //            ((ListView)samplepreplist.InnerView).CollectionSource.Remove(sequence);
                //        }
                //        ((ListView)samplepreplist.InnerView).Refresh();
                //    }

                //    DashboardViewItem qctype = null;
                //    if (View is DashboardView)
                //    {
                //        qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                //    }
                //    else
                //    {
                //        qctype = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                //        ((DetailView)View).RefreshDataSource();
                //    }
                //    if (qctype != null && qctype.InnerView != null)
                //    {
                //        ASPxGridListEditor gridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                //        if (gridListEditor != null && gridListEditor.Grid != null)
                //        {
                //            gridListEditor.Grid.ClearSort();
                //            foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                //            {
                //                if (column.Name == "SamplePrepAdd")
                //                {
                //                    column.Visible = false;
                //                }
                //            }
                //        }
                //        ((ListView)qctype.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] is null");
                //        ((ListView)qctype.InnerView).Refresh();
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void disablecontrols(bool enbstat, DevExpress.ExpressApp.View view)
        {
            try
            {
                sampleprepbatchinfo.IsEnbStat = enbstat;
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
                    else if (item.GetType() == typeof(ASPxRichTextPropertyEditor))
                    {
                        ASPxRichTextPropertyEditor propertyEditor = item as ASPxRichTextPropertyEditor;
                        if (propertyEditor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                    {
                        ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null && propertyEditor.Id != "Instrument")
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
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
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
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
                    else if (item.GetType() == typeof(AspxGridLookupCustomEditor))
                    {
                        AspxGridLookupCustomEditor propertyEditor = item as AspxGridLookupCustomEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ASPxRichTextPropertyEditor))
                    {
                        ASPxRichTextPropertyEditor propertyEditor = item as ASPxRichTextPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        }
                    }
                    else if (item.GetType() == typeof(ListPropertyEditor))
                    {
                        ListPropertyEditor propertyEditor = item as ListPropertyEditor;
                        if (propertyEditor != null)
                        //if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                            //if (propertyEditor.Frame.GetController<NewObjectViewController>() != null && propertyEditor.Frame.GetController<NewObjectViewController>().NewObjectAction != null)
                            //{
                            //    propertyEditor.Frame.GetController<NewObjectViewController>().NewObjectAction.Enabled.SetItemValue("Key", enbstat);
                            //}
                            //propertyEditor.Frame.GetController<ListViewController>().EditAction.Enabled.SetItemValue("Key", enbstat);
                            //propertyEditor.Frame.GetController<DeleteObjectsViewController>().DeleteAction.Enabled.SetItemValue("Key", enbstat);
                            //propertyEditor.Frame.GetController<LinkUnlinkController>().LinkAction.Enabled.SetItemValue("Key", enbstat);
                            //propertyEditor.Frame.GetController<LinkUnlinkController>().UnlinkAction.Enabled.SetItemValue("Key", enbstat);
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

        private void SamplePrepLoad_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem qctype = null;
                DashboardViewItem samplepreplist = null;
                DetailView sampleprepdetail = null;
                ActionContainerViewItem sampleprepaction2 = null;
                if (View is DashboardView)
                {
                    qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                    samplepreplist = ((DashboardView)View).FindItem("samplepreplist") as DashboardViewItem;
                    DashboardViewItem dvSampleprepdetail = ((DashboardView)View).FindItem("sampleprepdetail") as DashboardViewItem;
                    if (dvSampleprepdetail != null && dvSampleprepdetail.InnerView != null)
                    {
                        sampleprepdetail = dvSampleprepdetail.InnerView as DetailView;
                    }
                    sampleprepaction2 = ((DashboardView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                }
                else if (View is DetailView)
                {
                    qctype = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                    samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                    sampleprepdetail = View as DetailView;
                    sampleprepaction2 = ((DetailView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                }
                SamplePrepBatch batch = (SamplePrepBatch)sampleprepdetail.CurrentObject;
                if (batch.Jobid != null && !string.IsNullOrEmpty(batch.Jobid) && batch.Matrix != null && batch.Test != null && batch.Method != null)
                {
                    if (samplepreplist != null && qctype != null && sampleprepdetail != null && sampleprepaction2 != null)
                    {
                        if (samplepreplist.InnerView == null)
                        {
                            samplepreplist.CreateControl();
                        }
                        if (qctype.InnerView == null)
                        {
                            qctype.CreateControl();
                        }
                        if (samplepreplist.InnerView != null && qctype.InnerView != null)
                        {
                            Comment.Enabled.SetItemValue("HideComment", true);
                            if (((ListView)samplepreplist.InnerView).CollectionSource.GetCount() > 0 && (sampleprepaction2.Actions[1].Caption == "Sort" || sampleprepaction2.Actions[1].Caption == "序号"))
                            {
                                foreach (SamplePrepBatchSequence sequence in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().ToList())
                                {
                                    samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequence);
                                    ((ListView)samplepreplist.InnerView).CollectionSource.Remove(sequence);
                                }
                                ((ListView)samplepreplist.InnerView).Refresh();
                            }
                            else if (((ListView)samplepreplist.InnerView).CollectionSource.GetCount() > 0 && (sampleprepaction2.Actions[1].Caption == "Ok" || sampleprepaction2.Actions[1].Caption == "确定"))
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "samplesorterror"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                return;
                            }

                            //if (batch != null && !string.IsNullOrEmpty(batch.Jobid))
                            if (batch != null && batch.Test != null)
                            {
                                List<string> lstqctype = new List<string>();
                                if (!string.IsNullOrEmpty(batch.Test))
                                {
                                    List<string> lstTMOid = batch.Test.Split(';').ToList();
                                    if (lstTMOid != null)
                                    {
                                        foreach (string obj in lstTMOid)
                                        {
                                            TestMethod objTM = ObjectSpace.GetObjectByKey<TestMethod>(new Guid(obj.Trim()));
                                            if (objTM != null)
                                            {
                                                foreach (Testparameter TP in objTM.TestParameter)
                                                {
                                                    if (TP != null && TP.QCType != null && !lstqctype.Contains(TP.QCType.QCTypeName) && TP.QCType.QCTypeName != "Sample" && objTM.QCTypes.Contains(TP.QCType))
                                                    {
                                                        lstqctype.Add(TP.QCType.QCTypeName);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                                sampleprepbatchinfo.qctypeCriteria = "[QCTypeName] In(" + string.Format("'{0}'", string.Join("','", lstqctype)) + ")";
                                ((ListView)qctype.InnerView).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse(sampleprepbatchinfo.qctypeCriteria);
                                QCType sampleqCType = samplepreplist.InnerView.ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                                string[] ids = batch.Jobid.Split(';');
                                foreach (string id in ids)
                                {
                                    Samplecheckin objsamplecheckin = samplepreplist.InnerView.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", id));
                                    if (objsamplecheckin != null)
                                    {
                                        CNInfo.SPJobId = objsamplecheckin.JobID;
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
                                            CNInfo.SPSampleMatries = sb.ToString();
                                        }
                                        List<string> lstMatrixOid = batch.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                        List<string> lstTestOid = batch.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                        List<string> lstMethdOid = batch.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                        //IList<SampleParameter> objsp = samplepreplist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse(" [Samplelogin.JobID.JobID] = ? and [Testparameter.TestMethod.Oid] = ? And [SignOff] = True  And [Status] = 'PendingEntry' And [IsPrepMethodComplete]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=?", objsamplecheckin.JobID, batch.Method.Oid, batch.Sort - 1));
                                        IList<SampleParameter> objsp = samplepreplist.InnerView.ObjectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse(" [Samplelogin.JobID.JobID] = ?  And [SignOff] = True  And [IsTransferred] = true  And [Status] = 'PendingEntry' And [TestHold]  = False And [IsPrepMethodComplete]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=?", objsamplecheckin.JobID, batch.Sort - 1), new InOperator("Testparameter.TestMethod.MatrixName.Oid", lstMatrixOid.Select(i => new Guid(i))),
                                            new InOperator("Testparameter.TestMethod.Oid", lstTestOid.Select(i => new Guid(i))), new InOperator("Testparameter.TestMethod.MethodName.Oid", lstMethdOid.Select(i => new Guid(i)))));
                                        IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objdistsl = new List<Modules.BusinessObjects.SampleManagement.SampleLogIn>();
                                        foreach (SampleParameter sample in objsp)
                                        {
                                            if (!objdistsl.Contains(sample.Samplelogin))
                                            {
                                                objdistsl.Add(sample.Samplelogin);
                                            }
                                        }
                                        foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn sampleLog in objdistsl.Where(i => i != null).OrderBy(a => int.Parse(a.SampleID.Split('-')[1])).ToList())
                                        {
                                            SamplePrepBatchSequence qCBatch = samplepreplist.InnerView.ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                            List<SampleParameter> objSampleparam = samplepreplist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=?", sampleLog.Oid)).ToList();
                                            if (objSampleparam.Count > 0)
                                            {
                                                PrepMethod objPrep = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.TestHold == false && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).SelectMany(i => i.Testparameter.TestMethod.PrepMethods).FirstOrDefault(j => j.Tier == batch.Tier);
                                                //PrepMethod objPrep = objSampleparam.Testparameter.TestMethod.PrepMethods.FirstOrDefault(i => i.Tier == batch.Tier);
                                                if (objPrep != null)
                                                {
                                                    qCBatch.PrepMethod = samplepreplist.InnerView.ObjectSpace.GetObjectByKey<PrepMethod>(objPrep.Oid);
                                                }
                                                TestGuide objGuide = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.TestHold == false && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).SelectMany(i => i.Testparameter.TestMethod.TestGuides).FirstOrDefault();
                                                if (objGuide != null && objGuide.HoldingTimeBeforePrep != null)
                                                {
                                                    qCBatch.HoldTime = objGuide.HoldingTimeBeforePrep.HoldingTime;
                                                }
                                                Unit objTestParam = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.TestHold == false && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).Select(i => i.Testparameter.DefaultUnits).FirstOrDefault();
                                                if (objTestParam != null)
                                                {
                                                    qCBatch.Units = objTestParam;
                                                }
                                            }
                                            qCBatch.QCType = sampleqCType;
                                            qCBatch.SampleID = sampleLog;
                                            qCBatch.StrSampleID = sampleLog.SampleID;
                                            qCBatch.SYSSamplecode = sampleLog.SampleID;
                                            ((ListView)samplepreplist.InnerView).CollectionSource.Add(qCBatch);
                                        }
                                    }
                                }


                                ((ListView)samplepreplist.InnerView).Refresh();
                                ASPxGridListEditor gridListEditor = ((ListView)samplepreplist.InnerView).Editor as ASPxGridListEditor;
                                if (gridListEditor != null)
                                {
                                    if (gridListEditor.Grid == null)
                                    {
                                        gridListEditor.CreateControls();
                                    }
                                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                                    gridListEditor.Grid.ClearSort();
                                    ////gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["QCType"], ColumnSortOrder.Ascending);
                                    //gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["QCType.QCTypeName"], ColumnSortOrder.Ascending);
                                    ////gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["JobID"], ColumnSortOrder.Descending);
                                    //  gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["SampleID.JobID.DueDate"], ColumnSortOrder.Ascending);
                                    if (gridListEditor.Grid.Columns["JobID"]!=null)
                                    {
                                        gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["JobID"], ColumnSortOrder.Descending); 
                                    }
                                    gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["StrSampleID"], ColumnSortOrder.Ascending);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (batch.Matrix == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "matrixnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (batch.Test == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SelectTest"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (batch.Method == null)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Methodnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                    else if (string.IsNullOrEmpty(batch.Jobid))
                    {
                        //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "jobidnotempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                        Application.ShowViewStrategy.ShowMessage("Job ID must not be empty.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SamplePrepSort_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (e.Action.Caption == "Sort" || e.Action.Caption == "序号")
                {
                    DashboardViewItem qctype = null;
                    DashboardViewItem samplepreplist = null;
                    DetailView sampleprepdetail = null;
                    ActionContainerViewItem sampleprepaction2 = null;
                    if (View is DashboardView)
                    {
                        qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                        samplepreplist = ((DashboardView)View).FindItem("samplepreplist") as DashboardViewItem;
                        DashboardViewItem dvSampleprepdetail = ((DashboardView)View).FindItem("sampleprepdetail") as DashboardViewItem;
                        if (dvSampleprepdetail != null && dvSampleprepdetail.InnerView != null)
                        {
                            sampleprepdetail = dvSampleprepdetail.InnerView as DetailView;
                        }
                        sampleprepaction2 = ((DashboardView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                    }
                    else if (View is DetailView)
                    {
                        qctype = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                        samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                        sampleprepdetail = View as DetailView;
                        sampleprepaction2 = ((DetailView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                    }

                    if (samplepreplist != null && sampleprepdetail != null && qctype != null && samplepreplist.InnerView != null && ((ListView)samplepreplist.InnerView).CollectionSource.GetCount() > 0)
                    {
                        if (((ListView)samplepreplist.InnerView).SelectedObjects.Count > 0)
                        {
                            SamplePrepBatch batch = (SamplePrepBatch)sampleprepdetail.CurrentObject;
                            batch.ISShown = false;
                            disablecontrols(false, sampleprepdetail);
                            foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.Sort >= 0).ToList())
                            {
                                if (sequences.Sort == 0)
                                {
                                    samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequences);
                                    ((ListView)samplepreplist.InnerView).CollectionSource.Remove(sequences);
                                    //((ListView)samplepreplist.InnerView).AllowEdit["CanSamplePrepEdit"] = false;
                                    ((ListView)samplepreplist.InnerView).Refresh();
                                }
                                else if (string.IsNullOrEmpty(sequences.DF) || string.IsNullOrWhiteSpace(sequences.DF))
                                {
                                    sequences.DF = "1";
                                }
                            }
                            // Add Sequence from Squence Setup fprm
                            AddSequence(samplepreplist, batch);
                            if (sampleprepbatchinfo.lststrseqdilutioncount != null && sampleprepbatchinfo.lststrseqdilutioncount.Count > 0)
                            {
                                foreach (string strseq in sampleprepbatchinfo.lststrseqdilutioncount.ToList())
                                {
                                    string[] strarr = strseq.Split('|');
                                    int dcount = Convert.ToInt32(strarr[1]);
                                    SamplePrepBatchSequence objqcseq = samplepreplist.InnerView.ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strarr[0].ToString())));
                                    if (objqcseq != null && objqcseq.Sort > 0)
                                    {
                                        int sortcnt = objqcseq.Sort;
                                        int rcnt = 1;
                                        for (int x = 0; x < dcount - 1; x++)
                                        {
                                            ASPxGridListEditor qclistgrid = ((ListView)samplepreplist.InnerView).Editor as ASPxGridListEditor;
                                            if (qclistgrid != null && qclistgrid.Grid != null)
                                            {
                                                SamplePrepBatchSequence qCBatch = samplepreplist.InnerView.ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                                qCBatch.QCType = samplepreplist.InnerView.ObjectSpace.GetObjectByKey<QCType>(objqcseq.QCType.Oid);
                                                qCBatch.batchno = objqcseq.batchno;
                                                qCBatch.SampleID = objqcseq.SampleID;
                                                qCBatch.Units = objqcseq.Units;
                                                qCBatch.HoldTime = objqcseq.HoldTime;
                                                qCBatch.PrepMethod = objqcseq.PrepMethod;
                                                qCBatch.Sort = sortcnt;
                                                qCBatch.StrSampleID = objqcseq.StrSampleID;
                                                qCBatch.SYSSamplecode = objqcseq.SYSSamplecode + "R" + rcnt.ToString();
                                                qCBatch.IsDilution = true;
                                                ((ListView)samplepreplist.InnerView).CollectionSource.Add(qCBatch);
                                                rcnt++;
                                            }
                                        }
                                    }
                                }
                               ((ListView)samplepreplist.InnerView).Refresh();
                            }
                            ASPxGridListEditor gridListEditor = ((ListView)samplepreplist.InnerView).Editor as ASPxGridListEditor;
                            if (gridListEditor != null && gridListEditor.Grid != null)
                            {
                                gridListEditor.Grid.ClearSort();
                                gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["Sort"], ColumnSortOrder.Ascending);
                                foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                                {
                                    //if (column.Name == "SelectionCommandColumn")
                                    //{
                                    //    column.Visible = false;
                                    //}
                                    if (column.Name == "SamplePrepRemove")
                                    {
                                        column.Visible = true;
                                    }
                                }
                                //((ListView)samplepreplist.InnerView).AllowEdit["CanSamplePrepEdit"] = false;
                                //((ASPxGridListEditor)((ListView)samplepreplist.InnerView).Editor).Grid.UpdateEdit();
                                //gridListEditor.Grid.Columns["Sort"].Visible = false;
                                gridListEditor.Grid.Columns["DilutionCount"].Visible = false;
                                gridListEditor.Grid.Columns["DF"].Visible = true;
                                gridListEditor.Grid.Columns["DF"].Width = 70;
                                gridListEditor.Grid.Columns["DF"].SetColVisibleIndex(5);
                                gridListEditor.Grid.Selection.UnselectAll();
                            }
                            ASPxGridListEditor qctypegridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                            if (qctypegridListEditor != null && qctypegridListEditor.Grid != null)
                            {
                                GridViewColumn QCadd = qctypegridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "SamplePrepAdd").ToList()[0];
                                if (QCadd != null)
                                {
                                    QCadd.Visible = true;
                                    QCadd.VisibleIndex = 2;
                                }
                            }
                            ((ListView)samplepreplist.InnerView).Refresh();
                            ((ListView)qctype.InnerView).Refresh();
                            //ActionContainerViewItem sampleprepaction2 = ((DashboardView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                            sampleprepaction2.Actions[0].Enabled.SetItemValue("key", true);
                            if (objLanguage.strcurlanguage == "En")
                            {
                                sampleprepaction2.Actions[1].Caption = "Ok";
                                sampleprepaction2.Actions[1].ToolTip = "Ok";
                            }
                            else
                            {
                                sampleprepaction2.Actions[1].Caption = "确定";
                                sampleprepaction2.Actions[1].ToolTip = "确定";
                            }
                            //if (gridListEditor.Grid.Columns["SelectionCommandColumn"] != null)
                            //{
                            //    gridListEditor.Grid.Columns["SelectionCommandColumn"].Visible = false;
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
                    DashboardViewItem qctype = null;
                    DashboardViewItem samplepreplist = null;
                    DetailView sampleprepdetail = null;
                    ActionContainerViewItem sampleprepaction2 = null;
                    if (View is DashboardView)
                    {
                        qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                        samplepreplist = ((DashboardView)View).FindItem("samplepreplist") as DashboardViewItem;
                        DashboardViewItem dvSampleprepdetail = ((DashboardView)View).FindItem("sampleprepdetail") as DashboardViewItem;
                        if (dvSampleprepdetail != null && dvSampleprepdetail.InnerView != null)
                        {
                            sampleprepdetail = dvSampleprepdetail.InnerView as DetailView;
                        }
                        sampleprepaction2 = ((DashboardView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                    }
                    else if (View is DetailView)
                    {
                        qctype = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                        samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                        sampleprepdetail = View as DetailView;
                        sampleprepaction2 = ((DetailView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                    }

                    if (samplepreplist != null && sampleprepdetail != null && samplepreplist.InnerView != null)
                    {
                        SamplePrepBatch batch = (SamplePrepBatch)sampleprepdetail.CurrentObject;
                        batch.NPJobid = batch.Jobid;
                        batch.NPInstrument = batch.strInstrument;
                        //if (batch != null && !string.IsNullOrEmpty(batch.Instrument))
                        // if (batch != null && batch.Instruments.Count > 0)
                        if (batch != null && !string.IsNullOrEmpty(batch.Instrument) && batch.Instrument.Length > 0)
                        {
                            /* Below code has commented due to Prepbactch id format was changed */
                            /* CriteriaOperator qcct = CriteriaOperator.Parse("Max(SUBSTRING(PrepBatchID, 2))");
                            var val = ((XPObjectSpace)sampleprepdetail.ObjectSpace).Session.Evaluate(typeof(SamplePrepBatch), qcct, null);
                            string tempqc = (Convert.ToInt32(((XPObjectSpace)sampleprepdetail.ObjectSpace).Session.Evaluate(typeof(SamplePrepBatch), qcct, null)) + 1).ToString();
                            var curdate = DateTime.Now.ToString("yyMMdd");
                            string userid = ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                             if (tempqc != "1")
                             {
                                 var predate = tempqc.Substring(0, 6);
                                 if (predate == curdate)
                                 {
                                     //tempqc = "PB" + tempqc;
                                     tempqc = "PB" + tempqc;
                                 }
                                 else
                                 {
                                     //tempqc = "PB" + curdate + "01";
                                     tempqc = "PB" + curdate + userid + "01";
                                 }
                             }
                             else
                             {
                                 tempqc = "PB" + curdate + userid + "01";
                             }*/


                            // Prepbatch id format-"PB +YYMMDD + UserID + Seq
                            //if (sampleprepdetail.ObjectSpace.IsNewObject(batch))
                            //{
                            //    var curdate = DateTime.Now.ToString("yyMMdd");
                            //    string userid = ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                            //    string tempqc = string.Empty;
                            //    IList<SamplePrepBatch> SamplePrepatch = ((XPObjectSpace)sampleprepdetail.ObjectSpace).GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("SUBSTRING([PrepBatchID], 2, 9)=?", curdate + userid));
                            //    if (SamplePrepatch.Count > 0)
                            //    {
                            //        SamplePrepatch = SamplePrepatch.OrderBy(a => a.PrepBatchID).ToList();
                            //        tempqc = "PB" + curdate + userid + (Convert.ToInt32(SamplePrepatch[SamplePrepatch.Count - 1].PrepBatchID.Substring(11, 2)) + 1).ToString("00");
                            //    }
                            //    else
                            //    {
                            //        tempqc = "PB" + curdate + userid + "01";
                            //    }

                            //    batch.PrepBatchID = sampleprepbatchinfo.strSamplePrepID = tempqc;
                            //}
                            if (sampleprepdetail.ObjectSpace.IsNewObject(batch))
                            {
                                string tempqc = string.Empty;
                                string userid = "_" + ((Employee)SecuritySystem.CurrentUser).UserID.ToString("000");
                                var curdate = DateTime.Now.ToString("yyMMdd");
                                IList<SamplePrepBatch> SamplePrepatch = sampleprepdetail.ObjectSpace.GetObjects<SamplePrepBatch>(CriteriaOperator.Parse("SUBSTRING([PrepBatchID], 2, 6)=?", curdate));
                                if (SamplePrepatch.Count > 0)
                                {
                                    SamplePrepatch = SamplePrepatch.OrderBy(a => a.PrepBatchID).ToList();
                                    tempqc = "PB" + curdate + (Convert.ToInt32(SamplePrepatch[SamplePrepatch.Count - 1].PrepBatchID.Substring(8, 2)) + 1).ToString("00") + userid;

                                }
                                else
                                {
                                    tempqc = "PB" + curdate + "01" + userid;
                                }
                                batch.PrepBatchID = sampleprepbatchinfo.strSamplePrepID = tempqc;
                            }
                            //sampleprepbatchinfo.strTest = batch.Test.TestName;

                            //sampleprepbatchinfo.OidTestMethod = batch.Test.Oid;
                            //sampleprepbatchinfo.qcstatus = 0;
                            //string[] ids = batch.Instrument.Split(';');

                            ((ASPxGridListEditor)((ListView)samplepreplist.InnerView).Editor).Grid.UpdateEdit();

                            string msg;
                            if (sampleprepdetail.ObjectSpace.IsNewObject(batch))
                            {
                                if (objLanguage.strcurlanguage == "En")
                                {
                                    msg = "A Prep Batch ID " + sampleprepbatchinfo.strSamplePrepID + " has been created. Do you want to save it?";
                                }
                                else
                                {
                                    msg = "填写其余信息，然后单击“确定”。 弹出消息框“已创建质控批编号" + sampleprepbatchinfo.strSamplePrepID + "。您要保存表单吗？";
                                }
                            }
                            else
                            {
                                msg = "Do you want to update the changes ?";
                            }
                            WebWindow.CurrentRequestWindow.RegisterClientScript("CloseWeighingBatch", string.Format(CultureInfo.InvariantCulture, @"var openconfirm = confirm('" + msg + "'); {0}", callbackManager.CallbackManager.GetScript("CanCloseView", "openconfirm")));
                            //ResetNavigationCount();
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectinstrument"), InformationType.Error, timer.Seconds, InformationPosition.Top);
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

        private void SamplePrepAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem samplepreplist = null;
                DashboardViewItem qctype = null;
                if (View is DashboardView)
                {
                    samplepreplist = ((DashboardView)View).FindItem("samplepreplist") as DashboardViewItem;
                    qctype = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qctype") as DashboardViewItem;
                }
                if (View is DetailView)
                {
                    samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                    qctype = ((DetailView)((NestedFrame)Frame).ViewItem.View).FindItem("qctype") as DashboardViewItem;
                }
                else if (View is ListView && Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                    {
                        CompositeView cv = (CompositeView)nestedFrame.ViewItem.View;
                        if (cv != null && cv is DashboardView)
                        {
                            samplepreplist = ((DashboardView)cv).FindItem("samplepreplist") as DashboardViewItem;
                            qctype = ((DashboardView)cv).FindItem("qctype") as DashboardViewItem;
                        }
                        else
                        if (cv != null && cv is DetailView)
                        {
                            samplepreplist = ((DetailView)cv).FindItem("samplepreplist") as DashboardViewItem;
                            qctype = ((DetailView)cv).FindItem("qctype") as DashboardViewItem;
                        }
                    }
                }

                if (samplepreplist != null && qctype != null && samplepreplist.InnerView != null && qctype.InnerView != null)
                {
                    ASPxGridListEditor samplepreplistgrid = ((ListView)samplepreplist.InnerView).Editor as ASPxGridListEditor;
                    ASPxGridListEditor qctypegrid = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                    if (samplepreplistgrid != null && qctypegrid != null && samplepreplistgrid.Grid != null && qctypegrid.Grid != null)
                    {
                        if (samplepreplistgrid.Grid.Selection.Count == 1)
                        {
                            SamplePrepBatchSequence seqlist = (SamplePrepBatchSequence)samplepreplistgrid.GetSelectedObjects()[0];
                            QCType selqc = (QCType)e.CurrentObject;
                            if (seqlist != null && selqc != null && selqc.QCSource != null || seqlist.QCType.QCTypeName == "Sample" || selqc.QCSource == null && seqlist.QCType.QCTypeName != "Sample" && seqlist.QCType.QCSource == null)
                            {
                                //if (selqc.QCRootRole == QCRoleCN.加标 && selqc.QCSource != null && seqlist.QCType.QCTypeName != selqc.QCSource.QCTypeName)
                                //{
                                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "qccannotadd"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                //    return;
                                //}
                                if (selqc.QCSource != null && seqlist.QCType.QCTypeName != selqc.QCSource.QC_Source)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "qccannotadd"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    return;
                                }
                                SamplePrepBatchSequence sequence = samplepreplist.InnerView.ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                sequence.QCType = samplepreplist.InnerView.ObjectSpace.GetObjectByKey<QCType>(selqc.Oid);
                                sequence.batchno = seqlist.batchno;
                                sequence.SampleAmount = seqlist.SampleAmount;
                                sequence.FinalVolume = seqlist.FinalVolume;
                                //sequence.Runno = seqlist.Runno;
                                sequence.SampleID = seqlist.SampleID;
                                sequence.DF = "1";
                                int canaddtop = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.Sort == 0).ToList().Count;
                                if (seqlist.Sort == 1 && canaddtop == 0 && (selqc.QCSource == null || selqc.QCSource.QC_Source != "Sample"))
                                {
                                    sequence.Sort = seqlist.Sort - 1;
                                }
                                else
                                {
                                    sequence.Sort = seqlist.Sort + 1;
                                }
                                if (selqc.QCSource != null && selqc.QCSource.QC_Source == seqlist.QCType.QCTypeName)
                                {
                                    sequence.StrSampleID = seqlist.SYSSamplecode;
                                }
                                else if (selqc.QCSource != null && selqc.QCSource.QC_Source == "Sample")
                                {
                                    sequence.StrSampleID = seqlist.StrSampleID;
                                }
                                sequence.Units = seqlist.Units;
                                sequence.HoldTime = seqlist.HoldTime;
                                sequence.PrepMethod = seqlist.PrepMethod;
                                //else if (selqc.QCSource != null && selqc.QCSource.QC_Source == "Sample" || selqc.QCRole == QCRoleCN.平行)
                                //{
                                //    sequence.StrSampleID = seqlist.StrSampleID;
                                //}
                                int tempindex = sequence.Sort;
                                foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.Sort >= tempindex).OrderBy(a => a.Sort).ToList())
                                {
                                    if (tempindex == sequences.Sort)
                                    {
                                        sequences.Sort += 1;
                                        tempindex = sequences.Sort;
                                    }
                                }
                                ((ListView)samplepreplist.InnerView).CollectionSource.Add(sequence);
                                int tempsort = 1;
                                foreach (SamplePrepBatchSequence curseq in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.QCType.Oid == selqc.Oid).OrderBy(a => a.Sort).ToList())
                                {
                                    if (selqc.QCSource != null)
                                    {
                                        if (selqc.QCSource.QC_Source == "Sample")
                                        {
                                            var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.batchno == curseq.batchno && i.QCType.Oid == selqc.Oid).OrderBy(a => a.Sort).ToList();
                                            //var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType.Oid == selqc.Oid).OrderBy(a => a.Sort).ToList();
                                            if (objqc != null)
                                            {
                                                int objindex = objqc.IndexOf(curseq);
                                                if (objindex == 0)
                                                {
                                                    curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                    curseq.SYSSamplecode = curseq.StrSampleID + curseq.QCType.QCTypeName + tempsort;
                                                }
                                                else
                                                {
                                                    curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                    curseq.SYSSamplecode = curseq.StrSampleID + curseq.QCType.QCTypeName + tempsort;
                                                }
                                                //if (objqc.Count == (objindex + 1))
                                                {
                                                    tempsort++;
                                                }
                                            }
                                        }
                                        else if (selqc.QCSource.QC_Source == "MS")
                                        {
                                            var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.batchno == curseq.batchno && i.QCType.Oid == selqc.Oid).OrderBy(a => a.Sort).ToList();
                                            //var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType.Oid == selqc.Oid).OrderBy(a => a.Sort).ToList();
                                            if (objqc != null)
                                            {
                                                string MSsampleid = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.batchno == curseq.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.StrSampleID).FirstOrDefault();
                                                //string MSsampleid = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.QCType.QCSource != null && a.QCType.QCSource.QCTypeName == "Sample").Select(i => i.StrSampleID).FirstOrDefault();
                                                string MSSYSSamplecode = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.batchno == curseq.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.SYSSamplecode).FirstOrDefault();
                                                //string MSSYSSamplecode = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.QCType.QCSource != null && a.QCType.QCSource.QCTypeName == "Sample").Select(i => i.SYSSamplecode).FirstOrDefault();
                                                if (!string.IsNullOrEmpty(MSsampleid) && !string.IsNullOrEmpty(MSSYSSamplecode))
                                                {
                                                    int objindex = objqc.IndexOf(curseq);
                                                    if (objindex == 0)
                                                    {
                                                        curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                        curseq.SYSSamplecode = MSsampleid + curseq.QCType.QCTypeName + tempsort;
                                                    }
                                                    else
                                                    {
                                                        curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                        curseq.SYSSamplecode = MSsampleid + curseq.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                    }
                                                    curseq.StrSampleID = MSSYSSamplecode.Replace(MSsampleid, "");
                                                    //if (objqc.Count == (objindex + 1))
                                                    {
                                                        tempsort++;
                                                    }
                                                }
                                            }
                                        }
                                        //    else
                                        //    {
                                        //        if (curseq.QCType.QCRole != QCRoleCN.空白)
                                        //        {
                                        //            var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.batchno == curseq.batchno && a.StrSampleID == curseq.StrSampleID).OrderBy(a => a.Sort).ToList();
                                        //            //var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.StrSampleID == curseq.StrSampleID).OrderBy(a => a.Sort).ToList();
                                        //            if (objqc != null)
                                        //            {
                                        //                int objindex = objqc.IndexOf(curseq);
                                        //                if (objindex == 0)
                                        //                {
                                        //                    curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                        //                    curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                        //                }
                                        //                else
                                        //                {
                                        //                    curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                        //                    curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                        //                }
                                        //                if (objqc.Count == (objindex + 1))
                                        //                {
                                        //                    tempsort++;
                                        //                }
                                        //            }
                                        //        }
                                        //        else
                                        //        {
                                        //            curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                        //            curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                        //            tempsort++;
                                        //        }

                                        //    }
                                        //}
                                        else
                                        {
                                            //curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                            //curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                            //tempsort++;
                                            if (curseq.QCType.QcRole != null && !curseq.QCType.QcRole.IsBlank)
                                            {
                                                var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.batchno == curseq.batchno && a.StrSampleID == curseq.StrSampleID).OrderBy(a => a.Sort).ToList();
                                                if (objqc != null)
                                                {
                                                    int objindex = objqc.IndexOf(curseq);
                                                    if (objindex == 0)
                                                    {
                                                        curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                        curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                                    }
                                                    else
                                                    {
                                                        curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                        curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                    }
                                                    {
                                                        tempsort++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                                tempsort++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                        curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                        tempsort++;
                                    }
                                }
                            ((ListView)samplepreplist.InnerView).Refresh();
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
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

        private void SamplePrepRemove_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem samplepreplist = null;
                DashboardViewItem qctype = null;
                if (View is DashboardView)
                {
                    samplepreplist = ((DashboardView)View).FindItem("samplepreplist") as DashboardViewItem;
                    qctype = ((DashboardView)((NestedFrame)Frame).ViewItem.View).FindItem("qctype") as DashboardViewItem;
                }
                if (View is DetailView)
                {
                    samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                    qctype = ((DetailView)((NestedFrame)Frame).ViewItem.View).FindItem("qctype") as DashboardViewItem;
                }
                else if (View is ListView && Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                    {
                        CompositeView cv = (CompositeView)nestedFrame.ViewItem.View;
                        if (cv != null && cv is DashboardView)
                        {
                            samplepreplist = ((DashboardView)cv).FindItem("samplepreplist") as DashboardViewItem;
                            qctype = ((DashboardView)cv).FindItem("qctype") as DashboardViewItem;
                        }
                        else
                        if (cv != null && cv is DetailView)
                        {
                            samplepreplist = ((DetailView)cv).FindItem("samplepreplist") as DashboardViewItem;
                            qctype = ((DetailView)cv).FindItem("qctype") as DashboardViewItem;
                        }
                    }
                }

                if (samplepreplist != null && qctype != null && samplepreplist.InnerView != null && qctype.InnerView != null)
                {
                    ASPxGridListEditor samplepreplistgrid = ((ListView)samplepreplist.InnerView).Editor as ASPxGridListEditor;
                    ASPxGridListEditor qctypegrid = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                    if (samplepreplistgrid != null && qctypegrid != null && samplepreplistgrid.Grid != null && qctypegrid.Grid != null)
                    {
                        SamplePrepBatchSequence seqlist = (SamplePrepBatchSequence)e.CurrentObject;
                        if (seqlist != null)
                        {
                            if (seqlist.QCType.QCTypeName != "Sample")
                            {
                                int canremove = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.batchno == seqlist.batchno && i.QCType.QCTypeName != "Sample" && i.QCType.QCSource != null && i.QCType.QCSource.Oid == seqlist.QCType.Oid).ToList().Count;
                                //int canremove = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType.QCTypeName != "Sample" && i.QCType.QCSource != null && i.QCType.QCSource.Oid == seqlist.QCType.Oid).ToList().Count;
                                if (canremove == 0)
                                {
                                    int tempindex = seqlist.Sort;
                                    foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.Sort >= tempindex).OrderBy(a => a.Sort).ToList())
                                    {
                                        if (tempindex == sequences.Sort)
                                        {
                                            sequences.Sort -= 1;
                                            tempindex = sequences.Sort;
                                        }
                                    }
                                    string strqctypename = seqlist.QCType.QCTypeName;
                                    Guid seqqcid = seqlist.QCType.Oid;
                                    samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(seqlist);
                                    ((ListView)samplepreplist.InnerView).CollectionSource.Remove(seqlist);
                                    int tempsort = 1;
                                    foreach (SamplePrepBatchSequence curseq in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.QCType.Oid == seqqcid).OrderBy(a => a.Sort).ToList())
                                    {
                                        if (curseq.QCType.QCSource != null)
                                        {
                                            if (curseq.QCType.QCSource.QC_Source == "Sample")
                                            {
                                                var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.batchno == curseq.batchno && i.QCType.Oid == seqqcid).OrderBy(a => a.Sort).ToList();
                                                //var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType.Oid == seqqcid).OrderBy(a => a.Sort).ToList();
                                                if (objqc != null)
                                                {
                                                    int objindex = objqc.IndexOf(curseq);
                                                    if (objindex == 0)
                                                    {
                                                        curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                        curseq.SYSSamplecode = curseq.StrSampleID + curseq.QCType.QCTypeName + tempsort;
                                                    }
                                                    else
                                                    {
                                                        curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                        curseq.SYSSamplecode = curseq.StrSampleID + curseq.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                    }
                                                    if (objqc.Count == (objindex + 1))
                                                    {
                                                        tempsort++;
                                                    }
                                                }
                                            }
                                            else if (curseq.QCType.QCSource.QC_Source == "MS")
                                            {
                                                var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.batchno == curseq.batchno && i.QCType.Oid == seqqcid).OrderBy(a => a.Sort).ToList();
                                                //var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType.Oid == seqqcid).OrderBy(a => a.Sort).ToList();
                                                if (objqc != null)
                                                {
                                                    string MSsampleid = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.batchno == curseq.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.StrSampleID).FirstOrDefault();
                                                    string MSSYSSamplecode = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.batchno == curseq.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.SYSSamplecode).FirstOrDefault();
                                                    //string MSsampleid = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.QCType.QCSource != null && a.QCType.QCSource.QCTypeName == "Sample").Select(i => i.StrSampleID).FirstOrDefault();
                                                    //string MSSYSSamplecode = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.QCType.QCSource != null && a.QCType.QCSource.QCTypeName == "Sample").Select(i => i.SYSSamplecode).FirstOrDefault();
                                                    if (!string.IsNullOrEmpty(MSsampleid) && !string.IsNullOrEmpty(MSSYSSamplecode))
                                                    {
                                                        int objindex = objqc.IndexOf(curseq);
                                                        if (objindex == 0)
                                                        {
                                                            curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                            curseq.SYSSamplecode = MSsampleid + curseq.QCType.QCTypeName + tempsort;
                                                        }
                                                        else
                                                        {
                                                            curseq.SYSSamplecode = MSsampleid + curseq.QCType.QCTypeName + tempsort;// + "R" + objindex;
                                                        }
                                                        curseq.StrSampleID = MSSYSSamplecode.Replace(MSsampleid, "");
                                                        if (objqc.Count == (objindex + 1))
                                                        {
                                                            tempsort++;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //if (curseq.QCType.QCRole != QCRoleCN.空白)
                                                //{
                                                //    var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.batchno == curseq.batchno && a.StrSampleID == curseq.StrSampleID).OrderBy(a => a.Sort).ToList();
                                                //    //var objqc = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.StrSampleID == curseq.StrSampleID).OrderBy(a => a.Sort).ToList();
                                                //    if (objqc != null)
                                                //    {
                                                //        int objindex = objqc.IndexOf(curseq);
                                                //        if (objindex == 0)
                                                //        {
                                                //            curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                //            curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                                //        }
                                                //        else
                                                //        {
                                                //            curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                //            curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                                //        }
                                                //        if (objqc.Count == (objindex + 1))
                                                //        {
                                                //            tempsort++;
                                                //        }
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                //    curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                                //    tempsort++;
                                                //}
                                                curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                                curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
                                                tempsort++;
                                            }
                                        }
                                        else
                                        {
                                            curseq.SystemID = curseq.QCType.QCTypeName + tempsort;
                                            curseq.SYSSamplecode = curseq.QCType.QCTypeName + tempsort;
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
                                if (View.Id == "SamplePrepBatchSequence_ListView_History")
                                {
                                    int SampleSourceCount = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => seqlist.QCType != null && seqlist.QCType.QCTypeName == "Sample" && seqlist.IsDilution == false && i.StrSampleID == seqlist.StrSampleID && i.Oid != seqlist.Oid).ToList().Count();
                                    if (SampleSourceCount > 0)
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Please remove the sample source to proceed further.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                    //int SamplesCount = ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i=>i.QCType!=null && i.QCType.QCTypeName== "Sample").ToList().Count();
                                    //if(SamplesCount>1)
                                    //{
                                    //    string msg = "Removed all samples will delete the PrepID. Do you want to continue?";
                                    //    WebWindow.CurrentRequestWindow.RegisterClientScript("RemovedPrepID", string.Format(CultureInfo.InvariantCulture, @"var deleteconfirm = confirm('" + msg + "'); {0}", callbackManager.CallbackManager.GetScript("RemovedPrepID", "deleteconfirm")));
                                    //}
                                    if (samplepreplist.InnerView.SelectedObjects.Count == 1)
                                    {
                                        Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch obj = (Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch)Application.MainWindow.View.CurrentObject;
                                        if (obj != null)
                                        {
                                            if (samplepreplist.InnerView.ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID != null && i.PrepBatchID.Contains(obj.Oid.ToString()) && i.Samplelogin != null && i.Samplelogin.Oid == seqlist.SampleID.Oid && (i.QCBatchID != null || i.UQABID != null)) == null)
                                            {
                                                List<string> lstMatrixOid = obj.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                                List<string> lstTestOid = obj.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                                List<string> lstMethdOid = obj.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                                List<string> lstTest = new List<string>();
                                                if (lstTestOid != null)
                                                {
                                                    foreach (string objOid in lstTestOid)
                                                    {
                                                        if (!string.IsNullOrEmpty(objOid))
                                                        {
                                                            TestMethod objTest = samplepreplist.InnerView.ObjectSpace.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                                            if (objTest != null && !lstTest.Contains(objTest.TestName))
                                                            {
                                                                lstTest.Add(objTest.TestName);
                                                            }
                                                        }

                                                    }
                                                }
                                                List<string> lstMatrix = new List<string>();
                                                if (lstMatrixOid != null)
                                                {
                                                    foreach (string objOid in lstMatrixOid)
                                                    {
                                                        if (!string.IsNullOrEmpty(objOid))
                                                        {
                                                            Matrix objTest = samplepreplist.InnerView.ObjectSpace.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                                            if (objTest != null && !lstMatrix.Contains(objTest.MatrixName))
                                                            {
                                                                lstMatrix.Add(objTest.MatrixName);
                                                            }
                                                        }

                                                    }
                                                }
                                                List<string> lstMethod = new List<string>();
                                                if (lstMethdOid != null)
                                                {
                                                    foreach (string objOid in lstMethdOid)
                                                    {
                                                        if (!string.IsNullOrEmpty(objOid))
                                                        {
                                                            Method objTest = samplepreplist.InnerView.ObjectSpace.GetObjectByKey<Method>(new Guid(objOid.Trim()));
                                                            if (lstMethod != null && !lstMethod.Contains(objTest.MethodNumber))
                                                            {
                                                                lstMethod.Add(objTest.MethodNumber);
                                                            }
                                                        }

                                                    }
                                                }
                                                List<TestMethod> lsts = samplepreplist.InnerView.ObjectSpace.GetObjects<TestMethod>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[PrepMethods][].Count() > 0"), new InOperator("MatrixName.MatrixName", lstMatrix),
                                               new InOperator("TestName", lstTest), new InOperator("MethodName.MethodNumber", lstMethod))).ToList();
                                                if ((lsts.FirstOrDefault(i => i.PrepMethods.Count == 2) != null) && obj.Sort == 1)
                                                {
                                                    bool IsDelete = true;
                                                    foreach (TestMethod objtestmethod in lsts)
                                                    {
                                                        if (samplepreplist.InnerView.ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID.Contains(obj.Oid.ToString()) && i.Samplelogin != null && i.Samplelogin.Oid == seqlist.SampleID.Oid && objtestmethod.PrepMethods.Count == 2 && i.IsPrepMethodComplete == true && i.Testparameter != null
                                                        && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.Oid == objtestmethod.Oid) != null)
                                                        {
                                                            if (samplepreplist.InnerView.ObjectSpace.FirstOrDefault<SampleParameter>(i => i.PrepBatchID.Contains(obj.Oid.ToString()) && i.Samplelogin != null && i.Samplelogin.Oid == seqlist.Oid && i.IsPrepMethodComplete == true) != null)
                                                            {
                                                                IsDelete = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (IsDelete)
                                                    {

                                                        string strqctypename = seqlist.QCType.QCTypeName;
                                                        Guid seqqcid = seqlist.QCType.Oid;
                                                        foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.SampleID != null && a.SampleID == seqlist.SampleID).ToList())
                                                        {
                                                            if (seqlist.IsDilution)
                                                            {
                                                                if (sequences.Oid == seqlist.Oid)
                                                                {
                                                                    samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequences);
                                                                    ((ListView)samplepreplist.InnerView).CollectionSource.Remove(sequences);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequences);
                                                                ((ListView)samplepreplist.InnerView).CollectionSource.Remove(sequences);
                                                            }
                                                        }
                                                        samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(seqlist);
                                                        ((ListView)samplepreplist.InnerView).CollectionSource.Remove(seqlist);
                                                        int tempindex = seqlist.Sort;
                                                        foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.Sort >= tempindex).OrderBy(a => a.Sort).ToList())
                                                        {
                                                            if (tempindex == sequences.Sort)
                                                            {
                                                                sequences.Sort -= 1;
                                                                tempindex = sequences.Sort;
                                                            }
                                                        }
                                                        if (sampleprepbatchinfo.lstRemoveSampleSequence == null)
                                                        {
                                                            sampleprepbatchinfo.lstRemoveSampleSequence = new List<SamplePrepBatchSequence>();
                                                        }
                                                        if (!sampleprepbatchinfo.lstRemoveSampleSequence.Contains(seqlist))
                                                        {
                                                            sampleprepbatchinfo.lstRemoveSampleSequence.Add(seqlist);
                                                        }
                                                        if (sampleprepbatchinfo.lstAddSampleSequence.Contains(seqlist))
                                                        {
                                                            sampleprepbatchinfo.lstAddSampleSequence.Remove(seqlist);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletePrepSamples"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                                    }

                                                }
                                                else
                                                {
                                                    int tempindex = seqlist.Sort;
                                                    foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.Sort >= tempindex).OrderBy(a => a.Sort).ToList())
                                                    {
                                                        if (tempindex == sequences.Sort)
                                                        {
                                                            sequences.Sort -= 1;
                                                            tempindex = sequences.Sort;
                                                        }
                                                    }
                                                    string strqctypename = seqlist.QCType.QCTypeName;
                                                    Guid seqqcid = seqlist.QCType.Oid;
                                                    foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(a => a.SampleID != null && a.SampleID == seqlist.SampleID).ToList())
                                                    {
                                                        samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequences);
                                                        ((ListView)samplepreplist.InnerView).CollectionSource.Remove(sequences);
                                                    }
                                                    samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(seqlist);
                                                    ((ListView)samplepreplist.InnerView).CollectionSource.Remove(seqlist);
                                                    if (sampleprepbatchinfo.lstRemoveSampleSequence == null)
                                                    {
                                                        sampleprepbatchinfo.lstRemoveSampleSequence = new List<SamplePrepBatchSequence>();
                                                    }
                                                    if (!sampleprepbatchinfo.lstRemoveSampleSequence.Contains(seqlist))
                                                    {
                                                        sampleprepbatchinfo.lstRemoveSampleSequence.Add(seqlist);
                                                    }
                                                }
                                            }

                                            else
                                            {
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "cannotdeletePrepSamples"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "samplecannotremove"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }

                            }
                            //samplepreplistgrid.Grid.Selection.UnselectAll();
                            //qctypegrid.Grid.Selection.UnselectAll();
                            ((ListView)samplepreplist.InnerView).Refresh();
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

        private void SamplePrepPrevious_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                DashboardViewItem qctype = null;
                DashboardViewItem samplepreplist = null;
                DetailView sampleprepdetail = null;
                if (View is DashboardView)
                {
                    qctype = ((DashboardView)View).FindItem("qctype") as DashboardViewItem;
                    samplepreplist = ((DashboardView)View).FindItem("samplepreplist") as DashboardViewItem;
                    DashboardViewItem dvSampleprepdetail = ((DashboardView)View).FindItem("sampleprepdetail") as DashboardViewItem;
                    if (dvSampleprepdetail != null && dvSampleprepdetail.InnerView != null)
                    {
                        sampleprepdetail = dvSampleprepdetail.InnerView as DetailView;
                    }
                }
                else
                if (View is DetailView)
                {
                    qctype = ((DetailView)View).FindItem("qctype") as DashboardViewItem;
                    samplepreplist = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                    sampleprepdetail = View as DetailView;
                }

                if (samplepreplist != null && samplepreplist.InnerView != null && sampleprepdetail != null && qctype != null && qctype.InnerView != null)
                {
                    foreach (SamplePrepBatchSequence sequence in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null).OrderBy(a => a.Sort).ToList())
                    {
                        if (sequence.IsDilution == true)
                        {
                            ((ListView)samplepreplist.InnerView).CollectionSource.Remove(sequence);
                            ((ListView)samplepreplist.InnerView).Refresh();
                        }
                        else
                        {
                            if ((sequence.QCType.QCTypeName != "Sample"))
                            {
                                samplepreplist.InnerView.ObjectSpace.RemoveFromModifiedObjects(sequence);
                                ((ListView)samplepreplist.InnerView).CollectionSource.Remove(sequence);
                                ((ListView)samplepreplist.InnerView).Refresh();
                            }
                        }
                    }
                    int tempindex = 1;
                    foreach (SamplePrepBatchSequence sequences in ((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null).OrderBy(a => a.Sort).ToList())
                    {
                        sequences.Sort = tempindex;
                        tempindex++;
                    }
                    SamplePrepBatch batch = (SamplePrepBatch)sampleprepdetail.CurrentObject;
                    batch.ISShown = true;
                    disablecontrols(true, sampleprepdetail);
                    //if (batch != null && !string.IsNullOrEmpty(batch.Jobid))
                    if (batch != null)
                    {
                        QCType sampleqCType = samplepreplist.InnerView.ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                        //string[] ids = batch.Jobid.Split(';');
                        //foreach (string id in ids)
                        //{
                        //    Samplecheckin objsamplecheckin = samplepreplist.InnerView.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", new Guid(id)));
                        //    if (objsamplecheckin != null)
                        //    {
                        //        IList<SampleParameter> objsp = samplepreplist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID]=? and [Testparameter.TestMethod.Oid] = ? And [SamplePrepBatchID] Is Null And [Status] = 'PendingEntry'", objsamplecheckin.Oid, batch.Test.Oid));
                        //        IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objdistsl = new List<Modules.BusinessObjects.SampleManagement.SampleLogIn>();
                        //        foreach (SampleParameter sample in objsp)
                        //        {
                        //            if (!objdistsl.Contains(sample.Samplelogin))
                        //            {
                        //                objdistsl.Add(sample.Samplelogin);
                        //            }
                        //        }
                        //        objdistsl = objdistsl.Where(s => !((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Any(q => q.SampleID.Oid == s.Oid)).ToList();
                        //        foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn sampleLog in objdistsl.OrderBy(a => int.Parse(a.SampleID.Split('-')[1])).ToList())
                        //        {
                        //            SamplePrepBatchSequence qCBatch = samplepreplist.InnerView.ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                        //            qCBatch.QCType = sampleqCType;
                        //            qCBatch.SampleID = sampleLog;
                        //            qCBatch.StrSampleID = sampleLog.SampleID;
                        //            qCBatch.SYSSamplecode = sampleLog.SampleID;
                        //            ((ListView)samplepreplist.InnerView).CollectionSource.Add(qCBatch);
                        //        }
                        //    }
                        //}
                        string[] ids = batch.Jobid.Split(';');
                        foreach (string id in ids)
                        {
                            Samplecheckin objsamplecheckin = samplepreplist.InnerView.ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", id));
                            if (objsamplecheckin != null)
                            {
                                List<string> lstMatrixOid = batch.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                List<string> lstTestOid = batch.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                List<string> lstMethdOid = batch.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                                IList<SampleParameter> objsp = samplepreplist.InnerView.ObjectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse(" [Samplelogin.JobID.JobID] = ?  And [SignOff] = True  And [Status] = 'PendingEntry' And [IsPrepMethodComplete]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=?", objsamplecheckin.JobID, batch.Sort - 1), new InOperator("Testparameter.TestMethod.MatrixName.Oid", lstMatrixOid.Select(i => new Guid(i))),
                                           new InOperator("Testparameter.TestMethod.Oid", lstTestOid.Select(i => new Guid(i))), new InOperator("Testparameter.TestMethod.MethodName.Oid", lstMethdOid.Select(i => new Guid(i)))));
                                //IList<SampleParameter> objsp = samplepreplist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.JobID] = ? and [Testparameter.TestMethod.Oid] = ?  And [SignOff] = True And [Status] = 'PendingEntry' And [IsPrepMethodComplete]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=?", objsamplecheckin.JobID, batch.Test.Oid, batch.Sort - 1));
                                IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objdistsl = new List<Modules.BusinessObjects.SampleManagement.SampleLogIn>();
                                foreach (SampleParameter sample in objsp)
                                {
                                    if (sample.Samplelogin != null && !objdistsl.Contains(sample.Samplelogin))
                                    {
                                        objdistsl.Add(sample.Samplelogin);
                                    }
                                }
                                objdistsl = objdistsl.Where(s => !((ListView)samplepreplist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Any(q => q != null && q.SampleID != null && q.SampleID.Oid == s.Oid)).ToList();
                                foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn sampleLog in objdistsl.OrderBy(a => int.Parse(a.SampleID.Split('-')[1])).ToList())
                                {
                                    SamplePrepBatchSequence qCBatch = samplepreplist.InnerView.ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                    List<SampleParameter> objSampleparam = samplepreplist.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=?", sampleLog.Oid)).ToList();
                                    if (objSampleparam.Count > 0)
                                    {
                                        PrepMethod objPrep = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).SelectMany(i => i.Testparameter.TestMethod.PrepMethods).FirstOrDefault(j => j.Tier == batch.Tier);
                                        if (objPrep != null)
                                        {
                                            qCBatch.PrepMethod = samplepreplist.InnerView.ObjectSpace.GetObjectByKey<PrepMethod>(objPrep.Oid);
                                        }
                                        TestGuide objGuide = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).SelectMany(i => i.Testparameter.TestMethod.TestGuides).FirstOrDefault();
                                        if (objGuide != null && objGuide.HoldingTimeBeforePrep != null)
                                        {
                                            qCBatch.HoldTime = objGuide.HoldingTimeBeforePrep.HoldingTime;
                                        }
                                        Unit objTestParam = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).Select(i => i.Testparameter.DefaultUnits).FirstOrDefault();
                                        if (objTestParam != null)
                                        {
                                            qCBatch.Units = objTestParam;
                                        }
                                    }
                                    qCBatch.QCType = sampleqCType;
                                    qCBatch.SampleID = sampleLog;
                                    qCBatch.StrSampleID = sampleLog.SampleID;
                                    qCBatch.SYSSamplecode = sampleLog.SampleID;
                                    ((ListView)samplepreplist.InnerView).CollectionSource.Add(qCBatch);
                                }
                            }
                        }

                        ASPxGridListEditor gridListEditor = ((ListView)samplepreplist.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.ClearSort();
                            //gridListEditor.Grid.SettingsBehavior.AllowSelectSingleRowOnly = false;
                            if (gridListEditor.Grid.Columns["JobID"] != null)
                            {
                                gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["JobID"], ColumnSortOrder.Descending); 
                            }
                            gridListEditor.Grid.SortBy(gridListEditor.Grid.Columns["StrSampleID"], ColumnSortOrder.Ascending);
                            //foreach (GridViewColumn column in gridListEditor.Grid.Columns)
                            //{
                            //    if (column.Name == "SelectionCommandColumn")
                            //    {
                            //        column.Visible = true;
                            //    }
                            //    else if (column.Name == "SamplePrepRemove")
                            //    {
                            //        column.Visible = false;
                            //    }
                            //}
                            GridViewCommandColumn selectionBoxColumn = gridListEditor.Grid.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                            if (selectionBoxColumn != null)
                            {
                                selectionBoxColumn.Visible = true;
                            }
                            gridListEditor.Grid.Columns["Sort"].Visible = true;
                            gridListEditor.Grid.Columns["Sort"].FixedStyle = GridViewColumnFixedStyle.Left;
                            gridListEditor.Grid.Columns["DilutionCount"].Visible = true;
                            gridListEditor.Grid.Columns["DilutionCount"].Width = 100;
                            gridListEditor.Grid.Columns["DilutionCount"].SetColVisibleIndex(4);
                            gridListEditor.Grid.Columns["DF"].Visible = false;
                            if (gridListEditor.Grid.Columns["InlineEditCommandColumn"] != null)
                            {
                                gridListEditor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                            }
                            if (gridListEditor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridListEditor.Grid.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
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
                        ASPxGridListEditor qctypegridListEditor = ((ListView)qctype.InnerView).Editor as ASPxGridListEditor;
                        if (qctypegridListEditor != null && qctypegridListEditor.Grid != null)
                        {
                            GridViewColumn QCadd = qctypegridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "SamplePrepAdd").ToList()[0];
                            if (QCadd != null)
                            {
                                QCadd.Visible = false;
                            }
                        }

                        ActionContainerViewItem sampleprepaction2 = null;
                        if (View is DashboardView)
                        {
                            sampleprepaction2 = ((DashboardView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                        }
                        else
                        {
                            sampleprepaction2 = ((DetailView)View).FindItem("sampleprepaction2") as ActionContainerViewItem;
                        }
                        sampleprepaction2.Actions[0].Enabled.SetItemValue("key", false);
                        if (objLanguage.strcurlanguage == "En")
                        {
                            sampleprepaction2.Actions[1].Caption = "Sort";
                            sampleprepaction2.Actions[1].ToolTip = "Sort";
                        }
                        else
                        {
                            sampleprepaction2.Actions[1].Caption = "序号";
                            sampleprepaction2.Actions[1].ToolTip = "序号";
                        }
                        //((ListView)samplepreplist.InnerView).AllowEdit["CanSamplePrepEdit"] = true;
                        ((ListView)samplepreplist.InnerView).Refresh();
                        GridViewColumn QCRemoves = gridListEditor.Grid.Columns.Cast<GridViewColumn>().Where(i => i.Name == "SamplePrepRemove").ToList()[0];
                        if (QCRemoves != null)
                        {
                            QCRemoves.Visible = false;
                        }
                        gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void btnCopyFromSamplePrepAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SamplePrepBatch_DetailView_Copy" || View.Id == "SamplePrepBatch_DetailView_Copy_History")
                {
                    sampleprepbatchinfo.CopyFromPrepBatchSource = null;
                    IObjectSpace os = Application.CreateObjectSpace(typeof(CopyFromSamplePrepBatch));
                    CopyFromSamplePrepBatch source = os.CreateObject<CopyFromSamplePrepBatch>();
                    DetailView createdView = Application.CreateDetailView(os, source);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += CopyingFromPreviousBatch;
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

        private void CopyingFromPreviousBatch(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                SamplePrepBatch curBatch = (SamplePrepBatch)View.CurrentObject;
                CopyFromSamplePrepBatch source = (CopyFromSamplePrepBatch)e.AcceptActionArgs.CurrentObject;
                if (source != null && curBatch != null)
                {
                    if (sampleprepbatchinfo.CopyFromPrepBatchSource != null)
                    {
                        if (source.AllAbove || source.Reagent || source.Instrument || source.Others)
                        {
                            bool copyReagent = false;
                            bool copyInstrument = false;
                            bool copyOthers = false;
                            if (source.AllAbove)
                            {
                                copyReagent = true;
                                copyInstrument = true;
                                copyOthers = true;
                                //CopyFromPrevBatch(copyReagent: true, copyInstrument: true, copyOthers: true);
                            }
                            else
                            {
                                if (source.Reagent)
                                {
                                    copyReagent = true;
                                }
                                if (source.Instrument)
                                {
                                    copyInstrument = true;
                                }
                                if (source.Others)
                                {
                                    copyOthers = true;
                                }
                            }

                            if (copyOthers)
                            {
                                curBatch.Remarks = sampleprepbatchinfo.CopyFromPrepBatchSource.Remarks;
                            }
                            if (copyReagent)
                            {
                                foreach (Reagent obj in sampleprepbatchinfo.CopyFromPrepBatchSource.Reagents)
                                {
                                    Reagent objReagent = View.ObjectSpace.GetObject<Reagent>(obj);
                                    if (objReagent != null && !curBatch.Reagents.Contains(objReagent))
                                    {
                                        curBatch.Reagents.Add(objReagent);
                                    }
                                }
                            }
                            if (copyInstrument)
                            {
                                foreach (Labware obj in sampleprepbatchinfo.CopyFromPrepBatchSource.Instruments)
                                {
                                    Labware objInstrument = View.ObjectSpace.GetObject<Labware>(obj);
                                    if (objInstrument != null && !curBatch.Instruments.Contains(objInstrument))
                                    {
                                        curBatch.Instruments.Add(objInstrument);
                                    }
                                }
                            }
                            //((DetailView)View).ObjectSpace.ReloadObject(curBatch);
                            ((DetailView)View).Refresh();
                            Application.ShowViewStrategy.ShowMessage("Sample prep batch copied sucessfully.", InformationType.Info, timer.Seconds, InformationPosition.Top);

                        }
                        else
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Select a prep batch to copy from.", InformationType.Info, timer.Seconds, InformationPosition.Top);
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
                    e.Width = new System.Web.UI.WebControls.Unit(1200);
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

        private void SamplePrepHistoryAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //Frame.SetView(Application.CreateListView(typeof(SamplePrepBatch), true));
                IObjectSpace objectSpace = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(objectSpace, typeof(SamplePrepBatch));
                //cs.Criteria["HistoryStatusFilter"] = CriteriaOperator.Parse("[PrepBatchStatus] = 'ResultCompleted'");
                ListView listview = Application.CreateListView("SamplePrepBatch_ListView_History", cs, true);
                Frame.SetView(listview);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SavePrepBatch_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SamplePrepBatch_DetailView_Copy_History" && View.CurrentObject != null)
                {
                    SamplePrepBatch curtsmplpb = (SamplePrepBatch)View.CurrentObject;
                    DashboardViewItem Dvqcbatchseq = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                    if (Dvqcbatchseq != null && Dvqcbatchseq.InnerView != null)
                    {
                        if (((ListView)Dvqcbatchseq.InnerView).CollectionSource.GetCount() == 0)
                        {
                            string msg = "Removing all samples will delete the PrepID. Do you want to continue?";
                            WebWindow.CurrentRequestWindow.RegisterClientScript("CanDeletePrpID", string.Format(CultureInfo.InvariantCulture, @"var deleteconfirm = confirm('" + msg + "'); {0}", callbackManager.CallbackManager.GetScript("CanDeletePrpID", "deleteconfirm")));
                            sampleprepbatchinfo.IsDeletePreID = true;
                        }
                        ASPxGridListEditor gridlisteditor = ((ListView)Dvqcbatchseq.InnerView).Editor as ASPxGridListEditor;
                        List<SamplePrepBatchSequence> lstSequence = Dvqcbatchseq.InnerView.ObjectSpace.GetObjects<SamplePrepBatchSequence>(CriteriaOperator.Parse("[SamplePrepBatchDetail.Oid] = ?", curtsmplpb.Oid)).ToList();
                        foreach (SamplePrepBatchSequence objSequnce in lstSequence.ToList())
                        {
                            if (!((ListView)Dvqcbatchseq.InnerView).CollectionSource.List.Contains(objSequnce))
                            {
                                Dvqcbatchseq.InnerView.ObjectSpace.Delete(objSequnce);
                            }
                        }
                        Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch obj = (Modules.BusinessObjects.SampleManagement.SamplePreparation.SamplePrepBatch)View.CurrentObject;
                        List<string> lstMatrixOid = obj.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                        List<string> lstTestOid = obj.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                        List<string> lstMethdOid = obj.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                        List<string> lstTest = new List<string>();
                        if (lstTestOid != null)
                        {
                            foreach (string objOid in lstTestOid)
                            {
                                if (!string.IsNullOrEmpty(objOid))
                                {
                                    TestMethod objTest = Dvqcbatchseq.InnerView.ObjectSpace.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                    if (objTest != null && !lstTest.Contains(objTest.TestName))
                                    {
                                        lstTest.Add(objTest.TestName);
                                    }
                                }

                            }
                        }
                        List<string> lstMatrix = new List<string>();
                        if (lstMatrixOid != null)
                        {
                            foreach (string objOid in lstMatrixOid)
                            {
                                if (!string.IsNullOrEmpty(objOid))
                                {
                                    Matrix objTest = Dvqcbatchseq.InnerView.ObjectSpace.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                    if (objTest != null && !lstMatrix.Contains(objTest.MatrixName))
                                    {
                                        lstMatrix.Add(objTest.MatrixName);
                                    }
                                }

                            }
                        }
                        List<string> lstMethod = new List<string>();
                        if (lstMethdOid != null)
                        {
                            foreach (string objOid in lstMethdOid)
                            {
                                if (!string.IsNullOrEmpty(objOid))
                                {
                                    Method objTest = Dvqcbatchseq.InnerView.ObjectSpace.GetObjectByKey<Method>(new Guid(objOid.Trim()));
                                    if (lstMethod != null && !lstMethod.Contains(objTest.MethodNumber))
                                    {
                                        lstMethod.Add(objTest.MethodNumber);
                                    }
                                }

                            }
                        }
                        if (sampleprepbatchinfo.lstAddSampleSequence != null && sampleprepbatchinfo.lstAddSampleSequence.Count > 0)
                        {
                            bool CanCommit = false;
                            IObjectSpace os = Application.CreateObjectSpace();
                            List<TestMethod> lst = ObjectSpace.GetObjects<TestMethod>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[PrepMethods][].Count() > 0"), new InOperator("MatrixName.MatrixName", lstMatrix),
                            new InOperator("TestName", lstTest), new InOperator("MethodName.MethodNumber", lstMethod))).ToList();
                            foreach (SamplePrepBatchSequence sequence in ((ListView)Dvqcbatchseq.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.SamplePrepBatchDetail == null).OrderBy(a => a.Sort))
                            {
                                sequence.SamplePrepBatchDetail = Dvqcbatchseq.InnerView.ObjectSpace.GetObjectByKey<SamplePrepBatch>(obj.Oid);
                            }
                            foreach (SamplePrepBatchSequence sequence in ((ListView)Dvqcbatchseq.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null && i.QCType.QCTypeName == "Sample").OrderBy(a => a.Sort).ToList())
                            {
                                IList<SampleParameter> lstSampleParameter = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[Samplelogin.Oid] = ? And [SignOff] = True And [IsPrepMethodComplete]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=? ", sequence.SampleID.Oid, obj.Sort - 1), new InOperator("Testparameter.TestMethod.Oid", lst.Select(i => i.Oid))));
                                foreach (SampleParameter sampleParameter in lstSampleParameter)
                                {
                                    if (sampleParameter != null)
                                    {
                                        if (string.IsNullOrEmpty(sampleParameter.PrepBatchID))
                                        {
                                            sampleParameter.PrepBatchID = obj.Oid.ToString();
                                        }
                                        else
                                        {
                                            sampleParameter.PrepBatchID = sampleParameter.PrepBatchID + "; " + obj.Oid.ToString();
                                        }
                                        sampleParameter.PrepMethodCount = sampleParameter.PrepMethodCount + 1;
                                        if (sampleParameter.Testparameter.TestMethod.PrepMethods.Count == sampleParameter.PrepMethodCount)
                                        {
                                            sampleParameter.IsPrepMethodComplete = true;
                                        }
                                        CanCommit = true;
                                    }
                                }
                            }
                            if (CanCommit)
                            {
                                os.CommitChanges();
                                os.Dispose();
                                List<string> lstJobID = ((ListView)Dvqcbatchseq.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().ToList().Where(i => i.SampleID != null && i.SampleID.JobID != null).Select(i => i.SampleID.JobID.JobID).Distinct().ToList();
                                foreach (string jobID in lstJobID)
                                {
                                    if (!obj.Jobid.Contains(jobID))
                                    {
                                        obj.Jobid = obj.Jobid + ";" + jobID;
                                    }
                                }
                            }
                            sampleprepbatchinfo.lstAddSampleSequence.Clear();
                        }
                        if (sampleprepbatchinfo.lstRemoveSampleSequence != null && sampleprepbatchinfo.lstRemoveSampleSequence.Count > 0)
                        {
                            if (obj != null)
                            {
                                foreach (SamplePrepBatchSequence sequence in sampleprepbatchinfo.lstRemoveSampleSequence.ToList())
                                {
                                    List<TestMethod> lsts = Dvqcbatchseq.InnerView.ObjectSpace.GetObjects<TestMethod>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[PrepMethods][].Count() > 0"), new InOperator("MatrixName.MatrixName", lstMatrix),
                                       new InOperator("TestName", lstTest), new InOperator("MethodName.MethodNumber", lstMethod))).ToList();
                                    if ((lsts.FirstOrDefault(i => i.PrepMethods.Count == 2) != null) && obj.Sort == 1)
                                    {
                                        foreach (TestMethod objtestmethod in lsts)
                                        {
                                            SamplePrepBatchSequence seq = Dvqcbatchseq.InnerView.ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[Oid]=?", sequence.Oid));
                                            Dvqcbatchseq.InnerView.ObjectSpace.Delete(seq);
                                            IList<SampleParameter> objsampleParameters = Dvqcbatchseq.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Contains([PrepBatchID], ?) And [Samplelogin.Oid]=?", obj.Oid.ToString(), sequence.SampleID));
                                            if (obj.Sort == 1)
                                            {
                                                foreach (SampleParameter sample in objsampleParameters.ToList())
                                                {
                                                    sample.PrepBatchID = null;
                                                    sample.PrepMethodCount = 0;
                                                    sample.IsPrepMethodComplete = false;
                                                }
                                            }
                                            else if (obj.Sort == 2)
                                            {
                                                foreach (SampleParameter sample in objsampleParameters.ToList())
                                                {
                                                    List<string> lst = sample.PrepBatchID.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                                    if (lst.Contains(obj.Oid.ToString()))
                                                    {
                                                        lst.Remove(obj.Oid.ToString());
                                                    }
                                                    sample.PrepBatchID = lst.FirstOrDefault();
                                                    sample.PrepMethodCount = 1;
                                                    sample.IsPrepMethodComplete = false;
                                                }
                                            }
                                        }


                                    }
                                    else
                                    {
                                        SamplePrepBatchSequence seq = Dvqcbatchseq.InnerView.ObjectSpace.FindObject<SamplePrepBatchSequence>(CriteriaOperator.Parse("[Oid]=?", sequence.Oid));
                                        Dvqcbatchseq.InnerView.ObjectSpace.Delete(seq);
                                        IList<SampleParameter> objsampleParameters = Dvqcbatchseq.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("Contains([PrepBatchID], ?) And [Samplelogin.Oid]=?", obj.Oid.ToString(), sequence.SampleID));
                                        if (obj.Sort == 1)
                                        {
                                            foreach (SampleParameter sample in objsampleParameters.ToList())
                                            {
                                                sample.PrepBatchID = null;
                                                sample.PrepMethodCount = 0;
                                                sample.IsPrepMethodComplete = false;
                                            }
                                        }
                                        else if (obj.Sort == 2)
                                        {
                                            foreach (SampleParameter sample in objsampleParameters.ToList())
                                            {
                                                List<string> lst = sample.PrepBatchID.Split(';').Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                                if (lst.Contains(obj.Oid.ToString()))
                                                {
                                                    lst.Remove(obj.Oid.ToString());
                                                }
                                                sample.PrepBatchID = lst.FirstOrDefault();
                                                sample.PrepMethodCount = 1;
                                                sample.IsPrepMethodComplete = false;
                                            }
                                        }
                                    }
                                }
                                List<string> lstJobID = sampleprepbatchinfo.lstAddSampleSequence.Where(i => i.SampleID != null && i.SampleID.JobID != null).Select(i => i.SampleID.JobID.JobID).Distinct().ToList();
                                foreach (string jobID in lstJobID)
                                {
                                    if (!obj.Jobid.Contains(jobID))
                                    {
                                        obj.Jobid = obj.Jobid + ";" + jobID;
                                    }
                                }
                            }
                            sampleprepbatchinfo.lstRemoveSampleSequence.Clear();
                        }
                        if (gridlisteditor != null && gridlisteditor.Grid != null)
                        {
                            gridlisteditor.Grid.UpdateEdit();
                            Dvqcbatchseq.InnerView.ObjectSpace.CommitChanges();
                        }

                    }
                    if (curtsmplpb != null)
                    {
                        objAuditInfo.currentViewOid = curtsmplpb.Oid;
                        curtsmplpb.NPJobid = curtsmplpb.Jobid;
                        curtsmplpb.NPInstrument = curtsmplpb.strInstrument;
                        curtsmplpb.Save();
                        View.ObjectSpace.CommitChanges();
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        public void ResetNavigationCount()
        {
            ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
            ChoiceActionItem dataentryNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "DataEntry");
            if (dataentryNode != null)
            {

                ChoiceActionItem child = dataentryNode.Items.FirstOrDefault(i => i.Id == "AnalysisQueue" || i.Id == "AnalysisQueue ");
                if (child != null)
                {
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        int count = 0;
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        IList<SampleParameter> lstTests = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SignOff] = True And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False) And [UQABID] Is Null And [QCBatchID] Is Null And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [SamplePrepBatchID] Is Not Null) Or [Testparameter.TestMethod.PrepMethods][].Count() = 0)"));
                        if (lstTests != null && lstTests.Count > 0)
                        {
                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                            count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                        }
                        //var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        if (count > 0)
                        {
                            child.Caption = cap[0] + " (" + count + ")";
                        }
                        else
                        {
                            child.Caption = cap[0];
                        }
                    }
                    else
                    {
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        IList<AnalysisDepartmentChain> lstAnalysisDepartChain = objSpace.GetObjects<AnalysisDepartmentChain>(CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultValidation] =True", currentUser.Oid));

                        List<Guid> lstTestMethodOid = new List<Guid>();
                        IList<SampleParameter> lstTests = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SignOff] = True And [IsTransferred] = true And ([SubOut] Is Null Or [SubOut] = False) And [UQABID] Is Null And [QCBatchID] Is Null And (([Testparameter.TestMethod.PrepMethods][].Count() > 0 And [SamplePrepBatchID] Is Not Null) Or [Testparameter.TestMethod.PrepMethods][].Count() = 0)"));
                        if (lstTests != null && lstTests.Count > 0)
                        {
                            //count = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.IsSDMSTest).Select(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                            IList<Guid> lstselTests = lstTests.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).Select(i => i.Testparameter.TestMethod.Oid).ToList();
                            if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                            {
                                foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                                {
                                    foreach (TestMethod testMethod in departmentChain.TestMethods)
                                    {
                                        if (!lstTestMethodOid.Contains(testMethod.Oid) && lstselTests.Contains(testMethod.Oid))
                                        {
                                            lstTestMethodOid.Add(testMethod.Oid);
                                        }
                                    }
                                }
                            }

                        }

                        if (lstTestMethodOid.Count > 0)
                        {
                            int count = lstTestMethodOid.Count();

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
            ChoiceActionItem parentSamplePreparationRootNode = ShowNavigationController.ShowNavigationItemAction.Items.FirstOrDefault(i => i.Id == "SamplePreparationRootNode");
            if (parentSamplePreparationRootNode != null)
            {
                ChoiceActionItem child = parentSamplePreparationRootNode.Items.FirstOrDefault(i => i.Id == "SamplePreparation");
                if (child != null)
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    int objperpCount = objectSpace.GetObjects<TestMethod>().ToList().Where(i => i.NoOfPrepSamples > 0).Count();
                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    if (objperpCount > 0)
                    {
                        child.Caption = cap[0] + " (" + objperpCount + ")";
                    }
                    else
                    {
                        child.Caption = cap[0];
                    }

                }
            }

        }
        private void DateFilterSelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (View != null && SamplePrepBatchDateFilter != null && SamplePrepBatchDateFilter.SelectedItem != null && View.Id == "SamplePrepBatch_ListView")
                    {
                        string strSelectedItem = ((DevExpress.ExpressApp.Actions.SingleChoiceAction)sender).SelectedItem.Id.ToString();
                        if (strSelectedItem == "1M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffMonth(Datecreated, Now()) <= 1 And [Datecreated] Is Not Null");
                        }
                        else if (strSelectedItem == "3M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffMonth(Datecreated, Now()) <= 3 And [Datecreated] Is Not Null");
                        }
                        else if (strSelectedItem == "6M")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffMonth(Datecreated, Now()) <= 6 And [Datecreated] Is Not Null");
                        }
                        else if (strSelectedItem == "1Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(Datecreated, Now()) <= 1 And [Datecreated] Is Not Null");
                        }
                        else if (strSelectedItem == "2Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(Datecreated, Now()) <= 2 And [Datecreated] Is Not Null");
                        }
                        else if (strSelectedItem == "5Y")
                        {
                            ((ListView)View).CollectionSource.Criteria["DateFilter"] = CriteriaOperator.Parse("DateDiffYear(Datecreated, Now()) <= 5 And [Datecreated] Is Not Null");
                        }
                        else if (strSelectedItem == "ALL")
                        {
                            ((ListView)View).CollectionSource.Criteria.Remove("DateFilter");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void AddSample_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {

                SamplePrepBatch batch = (SamplePrepBatch)View.CurrentObject;
                IObjectSpace os = Application.CreateObjectSpace();
                if (batch != null && batch.Matrix != null && batch.Test != null && batch.Method != null)
                {
                    List<string> lstMatrixOid = batch.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                    List<string> lstTestOid = batch.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                    List<string> lstMethdOid = batch.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                    IList<SampleParameter> objsp = os.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And, CriteriaOperator.Parse("[SignOff] = True  And [Status] = 'PendingEntry' And [IsPrepMethodComplete]  = False And [Testparameter.TestMethod.PrepMethods][].Count() > 0 And [PrepMethodCount]=?", batch.Sort - 1), new InOperator("Testparameter.TestMethod.MatrixName.Oid", lstMatrixOid.Select(i => new Guid(i))),
                                          new InOperator("Testparameter.TestMethod.Oid", lstTestOid.Select(i => new Guid(i))), new InOperator("Testparameter.TestMethod.MethodName.Oid", lstMethdOid.Select(i => new Guid(i))), new NotOperator(new InOperator("Samplelogin.Oid", sampleprepbatchinfo.lstAddSampleSequence.Where(i => i.SampleID != null).Select(i => i.SampleID.Oid)))));

                    CollectionSource cs = new CollectionSource(os, typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn));
                    cs.Criteria["Filter"] = new InOperator("Oid", objsp.Where(i => i.Samplelogin != null).Select(i => i.Samplelogin.Oid).Distinct().ToList());
                    ListView lvparameter = Application.CreateListView("SampleLogIn_ListView_Preparation", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                    showViewParameters.CreatedView = lvparameter;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.Accepting += Dc_Accepting;
                    dc.SaveOnAccept = false;
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    SamplePrepBatch batch = (SamplePrepBatch)View.CurrentObject;
                    DashboardViewItem dvSampleprepdetail = ((DetailView)View).FindItem("samplepreplist") as DashboardViewItem;
                    if (dvSampleprepdetail != null && dvSampleprepdetail.InnerView != null)
                    {
                        QCType sampleqCType = dvSampleprepdetail.InnerView.ObjectSpace.FindObject<QCType>(CriteriaOperator.Parse("[QCTypeName] = 'Sample'"));
                        foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn objSample in e.AcceptActionArgs.SelectedObjects)
                        {
                            Modules.BusinessObjects.SampleManagement.SampleLogIn objSamplelogin = dvSampleprepdetail.InnerView.ObjectSpace.GetObject(objSample);
                            SamplePrepBatchSequence qCBatch = dvSampleprepdetail.InnerView.ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                            List<SampleParameter> objSampleparam = dvSampleprepdetail.InnerView.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=?", objSamplelogin.Oid)).ToList();
                            if (objSampleparam.Count > 0)
                            {
                                PrepMethod objPrep = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).SelectMany(i => i.Testparameter.TestMethod.PrepMethods).FirstOrDefault(j => j.Tier == batch.Tier);
                                if (objPrep != null)
                                {
                                    qCBatch.PrepMethod = dvSampleprepdetail.InnerView.ObjectSpace.GetObjectByKey<PrepMethod>(objPrep.Oid);
                                }
                                TestGuide objGuide = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).SelectMany(i => i.Testparameter.TestMethod.TestGuides).FirstOrDefault();
                                if (objGuide != null && objGuide.HoldingTimeBeforePrep != null)
                                {
                                    qCBatch.HoldTime = objGuide.HoldingTimeBeforePrep.HoldingTime;
                                }
                                Unit objTestParam = objSampleparam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.PrepMethods.Count > 0 && batch.Test.Contains(i.Testparameter.TestMethod.Oid.ToString()) && batch.Matrix.Contains(i.Testparameter.TestMethod.MatrixName.Oid.ToString()) && batch.Method.Contains(i.Testparameter.TestMethod.MethodName.Oid.ToString())).Select(i => i.Testparameter.DefaultUnits).FirstOrDefault();
                                if (objTestParam != null)
                                {
                                    qCBatch.Units = objTestParam;
                                }
                            }
                            qCBatch.QCType = sampleqCType;
                            qCBatch.SampleID = objSamplelogin;
                            qCBatch.StrSampleID = objSamplelogin.SampleID;
                            qCBatch.DF = "1";
                            qCBatch.SYSSamplecode = objSamplelogin.SampleID;
                            qCBatch.Sort = ((ListView)dvSampleprepdetail.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Max(i => i.Sort) + 1;
                            ((ListView)dvSampleprepdetail.InnerView).CollectionSource.Add(qCBatch);
                            if (!sampleprepbatchinfo.lstAddSampleSequence.Contains(qCBatch))
                            {
                                sampleprepbatchinfo.lstAddSampleSequence.Add(qCBatch);
                            }
                            objAuditInfo.currentViewOid = batch.Oid;
                            //processobjectchange(sender, e, objSamplePrepBatch.Oid, typeof(SamplePrepBatch), objNavInfo.SelectedNavigationCaption, objSamplePrepBatch.PrepBatchID, e.PropertyName);
                            Frame.GetController<AuditlogViewController>().insertauditdata(ObjectSpace, batch.Oid, OperationType.ValueChanged, "Sample Prep Batches", batch.PrepBatchID, "QCType", "", qCBatch.QCType.QCTypeName, "");
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

        private void AddSequence(DashboardViewItem qclist, SamplePrepBatch batch)
        {
            try
            {
                IList<SpreadSheetBuilder_InitialQCTestRun> initialQCTestRuns = new List<SpreadSheetBuilder_InitialQCTestRun>();
                IList<SpreadSheetBuilder_SampleQCTestRun> sampleQCTestRuns = new List<SpreadSheetBuilder_SampleQCTestRun>();
                IList<SpreadSheetBuilder_ClosingQCTestRun> closingQCTestRuns = new List<SpreadSheetBuilder_ClosingQCTestRun>();
                List<string> lstMatrixOid = batch.Matrix.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                List<string> lstTestOid = batch.Test.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                List<string> lstMethdOid = batch.Method.Split(';').ToList().Select(i => i = i.Trim()).ToList();
                if (lstTestOid != null && lstTestOid.Count == 1)
                {
                    //SpreadSheetBuilder_SequencePattern sequencePattern = ((ListView)qclist.InnerView).ObjectSpace.FindObject<SpreadSheetBuilder_SequencePattern>(CriteriaOperator.Parse("[uqTestMethodID] =? AND [uqMatrixID]=?", batch.Test, batch.Matrix));
                    SpreadSheetBuilder_SequencePattern sequencePattern = ((ListView)qclist.InnerView).ObjectSpace.FindObject<SpreadSheetBuilder_SequencePattern>(new GroupOperator(GroupOperatorType.And, new InOperator("uqTestMethodID", lstTestOid.Select(i => new Guid(i))), new InOperator("uqMatrixID", lstMatrixOid.Select(i => new Guid(i)))));
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
                                            List<SamplePrepBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                            if (lstTestRuns != null && lstTestRuns.Count > 0)
                                            {
                                                intRunNo = lstTestRuns.Count + 1;
                                            }
                                        }
                                        SamplePrepBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                        sample.QCType = qcType;
                                        sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                        sample.SystemID = qcType.QCTypeName + intRunNo;
                                        sample.Sort = index;

                                        sample.DF = "1";
                                        int tempindex = index;
                                        if (qcType.QCSource != null && qcType.QCSource.QC_Source == "LCS")
                                        {
                                            Int32 iLCSCount = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(x => x.QCType.QCTypeName == "LCS").OrderBy(a => a.Sort).ToList().Count;
                                            if (iLCSCount > 0)
                                            {
                                                SamplePrepBatchSequence qblist = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(x => x.QCType.QCTypeName == "LCS").OrderBy(a => a.Sort).ToList()[iLCSCount - 1];
                                                sample.StrSampleID = qblist.SYSSamplecode;
                                            }
                                        }
                                        foreach (SamplePrepBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().OrderBy(a => a.Sort).ToList())
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

                        List<SamplePrepBatchSequence> lstSamples = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(x => x.SampleID != null).OrderBy(a => a.Sort).ToList();
                        SamplePrepBatchSequence objLastSample = lstSamples.OrderByDescending(i => i.Sort).FirstOrDefault();
                        SamplePrepBatchSequence objFirstSample = lstSamples.OrderBy(i => i.Sort).FirstOrDefault();

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
                                            List<SamplePrepBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                            if (lstTestRuns != null && lstTestRuns.Count > 0)
                                            {
                                                intRunNo = lstTestRuns.Count + 1;
                                            }
                                        }
                                        SamplePrepBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                        sample.QCType = qcType;
                                        sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                        sample.SystemID = qcType.QCTypeName + intRunNo;
                                        sample.Sort = index;
                                        //sample.Runno = intRunNo;
                                        //sample.Runno = batchno;
                                        sample.DF = "1";
                                        int tempindex = index;
                                        foreach (SamplePrepBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().OrderBy(a => a.Sort).ToList())
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

                        foreach (SamplePrepBatchSequence qCBatch in lstSamples)
                        {
                            intIterations++;
                            intEntryCount++;
                            //if (sequencePattern.NumberOfSamplesBetweenQCTest == intIterations)
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
                                                List<SamplePrepBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                                if (lstTestRuns != null && lstTestRuns.Count > 0)
                                                {
                                                    intRunNo = lstTestRuns.Count + 1;
                                                }
                                            }
                                            SamplePrepBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                            sample.QCType = qcType;
                                            sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                            sample.Sort = index;
                                            //sample.Runno = intRunNo;
                                            //sample.Runno = batchno;
                                            sample.DF = "1";
                                            int tempindex = index;
                                            foreach (SamplePrepBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().OrderBy(a => a.Sort).ToList())
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
                                                SamplePrepBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                                sample.QCType = qcType;
                                                sample.batchno = qCBatch.batchno;
                                                sample.SampleID = qCBatch.SampleID;
                                                sample.DF = "1";
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
                                                sample.Sort = index;
                                                //sample.Runno = batchno;
                                                int tempindex = index;
                                                foreach (SamplePrepBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().OrderBy(a => a.Sort).ToList())
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
                                                SamplePrepBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                                sample.QCType = qcType;
                                                sample.batchno = qCBatch.batchno;
                                                sample.SampleID = qCBatch.SampleID;
                                                sample.DF = "1";
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
                                                sample.Sort = index;
                                                //sample.Runno = batchno;
                                                int tempindex = index;
                                                foreach (SamplePrepBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().OrderBy(a => a.Sort).ToList())
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
                                                    List<SamplePrepBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                                    if (lstTestRuns != null && lstTestRuns.Count > 0)
                                                    {
                                                        intRunNo = lstTestRuns.Count + 1;
                                                    }
                                                }
                                                SamplePrepBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                                sample.QCType = qcType;
                                                sample.SampleID = qCBatch.SampleID;
                                                sample.DF = "1";
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
                                                    sample.SampleID = qclist.InnerView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(qCBatch.SampleID.Oid);
                                                    sample.SYSSamplecode = qCBatch.SampleID.SampleID + qcType.QCTypeName + batchno;
                                                    //string MSsampleid = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == sample.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample").Select(i => i.StrSampleID).FirstOrDefault();
                                                    //string MSSYSSamplecode = ((ListView)qclist.InnerView).CollectionSource.List.Cast<QCBatchSequence>().Where(a => a.batchno == sample.batchno && a.QCType.QCSource != null && a.QCType.QCSource.QC_Source == "Sample" && a.Oid == qbslist.Oid).Select(i => i.SYSSamplecode).FirstOrDefault();
                                                    sample.StrSampleID = sample.SampleID.SampleID;//MSSYSSamplecode.Replace(MSsampleid, "");
                                                }
                                                else
                                                {
                                                    sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                                }


                                                sample.Sort = index;
                                                //sample.Runno = batchno;
                                                int tempindex = index;
                                                foreach (SamplePrepBatchSequence sequence in ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().OrderBy(a => a.Sort).ToList())
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
                            if (sequencePattern.NumberOfSamplesBetweenQCTest == intIterations)
                            {
                                batchno += 1;
                                intIterations = 0;
                            }
                        }

                        index = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Max(i => i.Sort) + 1;
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
                                            List<SamplePrepBatchSequence> lstTestRuns = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(i => i.QCType != null && i.QCType.Oid == testRun.uqQCTypeID.Oid).ToList();
                                            if (lstTestRuns != null && lstTestRuns.Count > 0)
                                            {
                                                intRunNo = lstTestRuns.Count + 1;
                                            }
                                        }
                                        SamplePrepBatchSequence sample = ((ListView)qclist.InnerView).ObjectSpace.CreateObject<SamplePrepBatchSequence>();
                                        sample.QCType = qcType;
                                        sample.SYSSamplecode = qcType.QCTypeName + intRunNo;
                                        sample.SystemID = qcType.QCTypeName + intRunNo;
                                        sample.Sort = index;
                                        //sample.Runno = intRunNo;
                                        //sample.Runno = batchno;
                                        sample.DF = "1";
                                        ((ListView)qclist.InnerView).CollectionSource.Add(sample);
                                        if (qcType.QCSource != null && qcType.QCSource.QC_Source == "LCS")
                                        {
                                            Int32 iLCSCount = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(x => x.QCType.QCTypeName == "LCS").OrderBy(a => a.Sort).ToList().Count;
                                            if (iLCSCount > 0)
                                            {
                                                SamplePrepBatchSequence qblist = ((ListView)qclist.InnerView).CollectionSource.List.Cast<SamplePrepBatchSequence>().Where(x => x.QCType.QCTypeName == "LCS").OrderBy(a => a.Sort).ToList()[iLCSCount - 1];
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
                if (View.Id == "SamplePrepBatch_DetailView_Copy")
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
                if (View.Id == "SamplePrepBatch_ListView")
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        if (View.SelectedObjects.Count == 1)
                        {                            
                            foreach (SamplePrepBatch spb in View.SelectedObjects)
                            {
                                Samplecheckin checkin = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID] = ?", spb.Jobid));
                                CNInfo.SCoidValue = checkin.Oid;
                                CNInfo.SPJobId = spb.PrepBatchID;
                            }
                            IObjectSpace os = Application.CreateObjectSpace(typeof(Notes));
                            Notes objcrtdummy = os.CreateObject<Notes>();
                            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(Notes));
                            cs.Criteria["filter"] = CriteriaOperator.Parse("[Samplecheckin.Oid] = ? AND [NoteSource] = 'Sample Prepration'", CNInfo.SCoidValue);
                            ListView lvparameter = Application.CreateListView("Notes_ListView_CaseNarrative_Sampleprep", cs, false);
                            lvparameter.Caption = "Notes History";
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
