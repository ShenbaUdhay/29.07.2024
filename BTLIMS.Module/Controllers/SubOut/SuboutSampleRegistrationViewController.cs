using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Pdf;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.XtraReports.UI;
using E_RoundOff;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.SuboutTracking;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Web;
using Attachment = System.Net.Mail.Attachment;
using Method = Modules.BusinessObjects.Setting.Method;
using Parameter = Modules.BusinessObjects.Setting.Parameter;

namespace LDM.Module.Controllers.SubOutTracking
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SuboutSampleRegistrationViewController : ViewController, IXafCallbackHandler
    {
        Contractlabinfo contractinfo = new Contractlabinfo();
        MessageTimer timer = new MessageTimer();
        DataTable dt = new DataTable();
        string strQctype = string.Empty;
        string strNumericResult = string.Empty;
        string strResult = string.Empty;
        string strUnits = string.Empty;
        string strDF = string.Empty;
        string strLOQ = string.Empty;
        string strUQl = string.Empty;
        string strRptLimit = string.Empty;
        string strMDL = string.Empty;
        string strAnalyzedBy = string.Empty;
        DateTime? strAnalyzedDate;
        string strApprovedBy = string.Empty;
        DateTime strApprovedDate;
        string strValidatedBy = string.Empty;
        DateTime strValidatedDate;
        double strSpikeAmount = 0;
        string strRecovery = string.Empty;
        string strRPD = string.Empty;
        string strRecLCLimit = string.Empty;
        string strRecUCLimit = string.Empty;
        string strRPDLCLimit = string.Empty;
        string strRPDUCLimit = string.Empty;
        string strJobID = string.Empty;
        string strQCType = string.Empty;
        string strSampleID = string.Empty;
        string strSampleName = string.Empty;
        string strMatrix = string.Empty;
        string strTest = string.Empty;
        string strMethod = string.Empty;
        string strParameter = string.Empty;
        string strSurrgate = string.Empty;
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        bool SuboutEdit;
        bool boolMailSend;
        SampleRegistrationInfo Sampleinfo = new SampleRegistrationInfo();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        ShowNavigationItemController ShowNavigationController;
        SuboutSampleInfo SSInfo = new SuboutSampleInfo();
        Roundoff Roundoff = new Roundoff();
        bool boolReportPreview;
                NavigationInfo objNavInfo = new NavigationInfo();

        public SuboutSampleRegistrationViewController()
        {
            InitializeComponent();
            TargetViewId = "SubOutSampleRegistrations_ListView;" + "SampleParameter_ListView_Copy_SubOutPendingSamples;" + "SubOutSampleRegistrations_DetailView_Copy;" + "SubOutContractLab_DetailView;" +
                "SubOutSampleRegistrations_ListView_Copy_SuboutDelivery;" + "SubOutSampleRegistrations_ListView_Copy_SuboutDelivered;" + "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry;" +
                "SubOutSampleRegistrations_ListView_Copy_ResultEntry;" + "SubOutSampleRegistrations_SampleParameter_ListView;" + "SubOutSampleRegistrations_SampleParameter_ListView_Copy_SuboutSampleRegistration;"
                + "SubOutSampleRegistrations_SampleParameter_ListView_Tracking;" + "SubOutSampleRegistrations_SampleParameter_ListView_ViewMode;" + "SubOutSampleRegistrations_ListView_ViewMode;"
                + "SampleParameter_ListView_SuboutSampleHistory;" + "SubOutSampleRegistrations_SampleParameter_ListView_QCResults;" + "SubOutSampleRegistrations_SampleParameter_ListView_QCResultsView;"
                + "SubOutSampleRegistrations_ListView_NotificationQueue;" + "SubOutSampleRegistrations_ListView_NotificationQueueView;" + "Customer_LookupListView_SuboutClient;"
                + "SubOutSampleRegistrations_DetailView_Level2DataReview;" + "SubOutSampleRegistrations_DetailView_Level3DataReview;"
                + "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data;" + "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data;"
                + "SubOutSampleRegistrations_DetailView_PendingSignOff;" + "SubOutSampleRegistrations_SampleParameter_ListView_SignOff;"
                + "SubOutSampleRegistrations_SampleParameter_ListView_Level3QcResults;" + "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data;"
                + "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data;" + "SubOutSampleRegistrations_SampleParameter_ListView_Level2QcResults;"
                + "SubOutSampleRegistrations_ListView_PendingSignOff;" + "SubOutSampleRegistrations_DetailView_SuboutOrderTracking;"
                + "SubOutSampleRegistrations_SampleParameter_ListView_SignOffHistory;" + "SubOutSampleRegistrations_ListView_Level2DataReview;" + "SubOutSampleRegistrations_ListView_Level3DataReview;"
                + "SubOutSampleRegistrations_ListView_Level2DataReview_History;" + "SubOutSampleRegistrations_ListView_Level3DataReview_History;" + "SubOutSampleRegistrations_ListView_SigningOffHistory;"
                + "SubOutContractLab_CertifiedTests_ListView;" + "Parameter_ListView_ContractLab;" + "CertifiedTests_DetailView;" + "SubOutSampleRegistrations_SubOutQcSample_ListView;"
                + "SubOutSampleRegistrations_DetailView_TestOrder;" + "SubOutSampleRegistrations_ListView_TestOrder;" + "SubOutSampleRegistrations_SubOutQcSample_ListView_Level2;" + "SubOutSampleRegistrations_SubOutQcSample_ListView_Level3;"
                + "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data_History;" + "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data_History;" + "SubOutSampleRegistrations_SubOutQcSample_ListView_Level2_History;"
                + "SubOutSampleRegistrations_SubOutQcSample_ListView_Level3_History;" + "SubOutSampleRegistrations_SubOutQcSample_ListView_ResltEntryView;" + "SubOutSampleRegistrations_ListView_SuboutRegHistory;"
                + "SubOutSampleRegistrations_DetailView_SuboutResultEntry_History;" + "SubOutSampleRegistrations_DetailView_Level2DataReview_History;" + "SubOutSampleRegistrations_DetailView_Level3DataReview_History;"
                + "SubOutSampleRegistrations_SampleParameter_ListView_TestOrder;" + "SampleParameter_ListView_Copy_SubOutAddSamples;";
            //SuboutSubmit.TargetViewId = "SubOutSampleRegistrations_ListView;" + "SubOutSampleRegistrations_ListView_Copy_SuboutDelivery;"+ "SubOutSampleRegistrations_ListView_Copy_SuboutDelivered;";
            importSuboutResult.TargetViewId = "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry;";
            SuboutSubmit.TargetViewId = "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry;";
            SuboutViewHistory.TargetViewId = "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry;" + "SubOutSampleRegistrations_ListView_Copy_ResultEntry;" /*+ "SubOutSampleRegistrations_ListView_TestOrder"*/;
            SuboutAllowEdit.TargetViewId = "SubOutSampleRegistrations_ListView_ViewMode;";
            ResultEntryHistoryDateFilter.TargetViewId = "SubOutSampleRegistrations_ListView_ViewMode;";
            SubOutSampleTestOrderDateFilter.TargetViewId = "SubOutSampleRegistrations_ListView_TestOrder;";
            SubOutSampleHistory.TargetViewId = "SampleParameter_ListView_Copy_SubOutPendingSamples;";
            SubOutSampleHistoryDateFilter.TargetViewId = "SampleParameter_ListView_SuboutSampleHistory;";
            SuboutNotificationhistory.TargetViewId = "SubOutSampleRegistrations_ListView_NotificationQueue;";
            ResultEntryHistorRollbak.TargetViewId = "SubOutSampleRegistrations_DetailView_SuboutResultEntry_History;" + "SubOutSampleRegistrations_DetailView_Level2DataReview_History;" + "SubOutSampleRegistrations_DetailView_Level2DataReview;" + "SubOutSampleRegistrations_DetailView_Level3DataReview;" + "SubOutSampleRegistrations_DetailView_Level3DataReview_History;";
            SuboutResultReview.TargetViewId = "SubOutSampleRegistrations_DetailView_Level2DataReview;";
            SuboutResultApproval.TargetViewId = "SubOutSampleRegistrations_DetailView_Level3DataReview";
            SuboutSigningOff.TargetViewId = "SubOutSampleRegistrations_DetailView_PendingSignOff";
            NotificationQueueDateFilter.TargetViewId = "SubOutSampleRegistrations_ListView_NotificationQueueView;";
            SuboutSigningOffHistory.TargetViewId = "SubOutSampleRegistrations_ListView_PendingSignOff;";
            SuboutCOCReport.TargetViewId = "SubOutSampleRegistrations_ListView_NotificationQueue;" + "SubOutSampleRegistrations_ListView;" + "SubOutSampleRegistrations_DetailView_Copy;"
                + "SubOutSampleRegistrations_DetailView_SuboutOrderTracking;" + "SubOutSampleRegistrations_ListView_NotificationQueueView;" + "SubOutSampleRegistrations_ListView_SuboutRegHistory;" + "SubOutSampleRegistrations_DetailView_TestOrder;"
                + "SubOutSampleRegistrations_ListView_TestOrder;";
            NotificationQueueMailReSend.TargetViewId = "SubOutSampleRegistrations_ListView_NotificationQueue;";
            SuboutOrderSubmit.TargetViewId = "SubOutSampleRegistrations_DetailView_TestOrder;" + "SubOutSampleRegistrations_ListView_TestOrder;";
            SuboutOrderSubmit.TargetObjectsCriteria = "[SuboutStatus] = 'PendingSuboutSubmission'";
            SuboutAddSample.TargetViewId = SuboutRemoveSample.TargetViewId = "SubOutSampleRegistrations_SampleParameter_ListView_TestOrder";
            SuboutEDDTempalte.TargetViewId = "SubOutSampleRegistrations_ListView_NotificationQueue;";
            // Target required Views (via the TargetXXX properties) and create their Actions.

            SimpleAction btnparameter = new SimpleAction(this, "btnparameter", PredefinedCategory.RecordEdit);
            {
                btnparameter.Caption = "Parameter";
            }
            btnparameter.TargetViewId = "SubOutContractLab_CertifiedTests_ListView;";
            btnparameter.Execute += btnparameter_Execute;
            btnparameter.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //btnparameter.ImageName = "Action_Search";

            SimpleAction SuboutDataReviewHistory = new SimpleAction(this, "SuboutDataReviewHistory", PredefinedCategory.RecordEdit);
            {
                SuboutDataReviewHistory.Caption = "History";
            }
            SuboutDataReviewHistory.TargetViewId = "SubOutSampleRegistrations_DetailView_Level3DataReview;" + "SubOutSampleRegistrations_DetailView_Level2DataReview;" + "SubOutSampleRegistrations_ListView_Level2DataReview;" + "SubOutSampleRegistrations_ListView_Level3DataReview;";
            SuboutDataReviewHistory.Execute += SuboutDataReviewHistory_Execute;
            SuboutDataReviewHistory.ImageName = "History16";

            //SimpleAction SuboutDataReviewRollBack = new SimpleAction(this, "SuboutDataReviewRollBack", PredefinedCategory.RecordEdit);
            //{
            //    SuboutDataReviewRollBack.Caption = "RollBack";
            //}
            //SuboutDataReviewRollBack.TargetViewId = "SubOutSampleRegistrations_ListView_Level2DataReview_History;" + "SubOutSampleRegistrations_ListView_Level3DataReview_History;" + "SubOutSampleRegistrations_ListView_SigningOffHistory;";
            //SuboutDataReviewRollBack.Execute += SuboutDataReviewRollBack_Execute;
            //SuboutDataReviewRollBack.ImageName = "Backward_16x16";
        }

        private void btnparameter_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SubOutContractLab_CertifiedTests_ListView")
                {
                    foreach (CertifiedTests objtest in View.SelectedObjects)
                    {
                        if (objtest.Matrix != null && objtest.Test != null && objtest.Method != null && objtest.Method.MethodName != null)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            List<Guid> lstParamOid = new List<Guid>();
                            List<Testparameter> lsttestpara = os.GetObjects<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ? And [QCType.QCTypeName] = 'Sample'", objtest.Matrix.MatrixName, objtest.Test.TestName, objtest.Method.MethodName.MethodNumber)).ToList();
                            foreach (Testparameter objtestpara in lsttestpara.ToList())
                            {
                                if (!lstParamOid.Contains(objtestpara.Parameter.Oid) && objtestpara.Parameter != null)
                                {
                                    lstParamOid.Add(objtestpara.Parameter.Oid);
                                }
                            }
                            CollectionSource cs = new CollectionSource(os, typeof(Parameter));
                            cs.Criteria["Filter"] = new InOperator("Oid", lstParamOid);
                            ListView lstview = Application.CreateListView("Parameter_ListView_ContractLab", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lstview);
                            showViewParameters.CreatedView = lstview;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.AcceptAction.Active.SetItemValue("okay", false);
                            dc.CancelAction.Active.SetItemValue("cancel", false);
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        private void SuboutDataReviewRollBack_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count > 0)
                {
                    foreach (SubOutSampleRegistrations objsuboutobjs in View.SelectedObjects)
                    {
                        IObjectSpace os = Application.CreateObjectSpace(typeof(SubOutSampleRegistrations));
                        //Tasks obj = os.CreateObject<Tasks>();
                        SubOutSampleRegistrations suboutObj = os.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid] = ?", objsuboutobjs.Oid));
                        DetailView createdView = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_RollBack", true, suboutObj);
                        createdView.ViewEditMode = ViewEditMode.Edit;
                        ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        showViewParameters.CreatedView.Caption = "RollBack";
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += SubOutSampleRegistrations_RollBack_Accepting;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
                else if (View != null && View.SelectedObjects.Count > 1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
                else if (View != null && View.SelectedObjects.Count == 0)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SubOutSampleRegistrations_RollBack_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                Guid suboutguid = Guid.Empty;
                string strrollbackreason = string.Empty;
                if (sender != null)
                {
                    DialogController dc = (DialogController)sender;
                    if (dc.Window.View != null)
                    {
                        SubOutSampleRegistrations objsubout = (SubOutSampleRegistrations)dc.Window.View.CurrentObject;
                        if (objsubout != null)
                        {
                            suboutguid = objsubout.Oid;
                            strrollbackreason = objsubout.RollBackReason;
                        }
                        if (objsubout != null && string.IsNullOrEmpty(objsubout.RollBackReason))
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbackempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(strrollbackreason))
                    {
                        if (View.Id == "SubOutSampleRegistrations_ListView_Level2DataReview_History")
                        {
                            SubOutSampleRegistrations objsubout = ObjectSpace.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid] = ?", suboutguid));
                            if (objsubout != null && objsubout.SampleParameter != null && objsubout.SampleParameter.Count > 0)
                            {
                                foreach (SampleParameter objsmplpara in objsubout.SampleParameter.ToList())
                                {
                                    objsmplpara.Status = Samplestatus.PendingEntry;
                                    objsmplpara.OSSync = true;
                                    objsmplpara.IsExportedSuboutResult = false;
                                    objsubout.Status = SuboutStatus.SuboutPendingValidation;
                                    objsubout.RollBackBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objsubout.RollBackDate = DateTime.Now;
                                    objsubout.RollBackReason = strrollbackreason;
                                    objsmplpara.SuboutValidatedDate = null;
                                    objsmplpara.SuboutValidatedBy = null;

                                    objsmplpara.AnalyzedDate = null;
                                    objsmplpara.AnalyzedBy = null;
                                    objsmplpara.ValidatedDate = null;
                                    objsmplpara.ValidatedBy = null;
                                    objsmplpara.ApprovedDate = null;
                                    objsmplpara.ApprovedBy = null;
                                    objsmplpara.MDL = null;
                                    objsmplpara.UQL = null;
                                    objsmplpara.LOQ = null;
                                    //objsmplpara.ResultNumeric = null;
                                    //objsmplpara.Result = null;

                                }
                                View.ObjectSpace.CommitChanges();
                                View.ObjectSpace.Refresh();
                            }
                        }
                        if (View.Id == "SubOutSampleRegistrations_ListView_Level3DataReview_History")
                        {
                            SubOutSampleRegistrations objsubout = ObjectSpace.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid] = ?", suboutguid));
                            if (objsubout != null && objsubout.SampleParameter != null && objsubout.SampleParameter.Count > 0)
                            {
                                foreach (SampleParameter objsmplpara in objsubout.SampleParameter.ToList())
                                {
                                    if (objsmplpara.Status != Samplestatus.Reported)
                                    {
                                        objsmplpara.Status = Samplestatus.SuboutPendingValidation;
                                        objsmplpara.OSSync = true;
                                        objsmplpara.IsExportedSuboutResult = false;
                                        objsubout.Status = SuboutStatus.SuboutPendingValidation;
                                        objsubout.RollBackBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objsubout.RollBackDate = DateTime.Now;
                                        objsubout.RollBackReason = strrollbackreason;
                                        //objsmplpara.SuboutApprovedDate = null;
                                        //objsmplpara.SuboutApprovedBy = null;

                                        //objsmplpara.AnalyzedDate = null;
                                        //objsmplpara.AnalyzedBy = null;
                                        //objsmplpara.ValidatedDate = null;
                                        //objsmplpara.ValidatedBy = null;
                                        //objsmplpara.ApprovedDate = null;
                                        //objsmplpara.ApprovedBy = null;
                                        //objsmplpara.MDL = null;
                                        //objsmplpara.UQL = null;
                                        //objsmplpara.LOQ = null;
                                        //objsmplpara.ResultNumeric = null;
                                        //objsmplpara.Result = null;
                                    }
                                }
                                View.ObjectSpace.CommitChanges();
                                View.ObjectSpace.Refresh();
                            }
                        }
                        if (View.Id == "SubOutSampleRegistrations_ListView_SigningOffHistory")
                        {
                            SubOutSampleRegistrations objsubout = ObjectSpace.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid] = ?", suboutguid));
                            if (objsubout != null && objsubout.SampleParameter != null && objsubout.SampleParameter.Count > 0)
                            {
                                foreach (SampleParameter objsmplpara in objsubout.SampleParameter.ToList())
                                {
                                    objsmplpara.SuboutSignOff = false;
                                    objsubout.Status = SuboutStatus.PendingSigningOff;
                                    objsubout.RollBackBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objsubout.RollBackDate = DateTime.Now;
                                    objsubout.RollBackReason = strrollbackreason;
                                    objsmplpara.SuboutSignOffBy = null;
                                    objsmplpara.SuboutSignOffDate = null;
                                }
                                View.ObjectSpace.CommitChanges();
                                View.ObjectSpace.Refresh();
                            }
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SuboutDataReviewHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(SubOutSampleRegistrations));
                if (View.Id == "SubOutSampleRegistrations_ListView_Level2DataReview" || View.Id == "SubOutSampleRegistrations_DetailView_Level2DataReview")
                {
                    CollectionSource cs = new CollectionSource(os, typeof(SubOutSampleRegistrations));
                    ListView crtlistview = Application.CreateListView("SubOutSampleRegistrations_ListView_Level2DataReview_History", cs, true);
                    Frame.SetView(crtlistview);
                }
                else if (View.Id == "SubOutSampleRegistrations_ListView_Level3DataReview" || View.Id == "SubOutSampleRegistrations_DetailView_Level3DataReview")
                {
                    CollectionSource cs = new CollectionSource(os, typeof(SubOutSampleRegistrations));
                    ListView crtlistview = Application.CreateListView("SubOutSampleRegistrations_ListView_Level3DataReview_History", cs, true);
                    Frame.SetView(crtlistview);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View is ListView)
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active["DisableUnsavedChangesNotificationController"] = false;
                ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                importSuboutResult.Active.SetItemValue("valSuboutImport", false);
                SuboutSubmit.Active.SetItemValue("valSuboutSubmit", false);
                SuboutAllowEdit.Active.SetItemValue("valSuboutAllowEdit", false);
                SuboutSigningOff.Active.SetItemValue("valSuboutSigningOff", false);
                ResultEntryHistorRollbak.Active.SetItemValue("valResultEntryHistorRollbak", false);
                SuboutResultReview.Active.SetItemValue("valSuboutResultReview", false);
                SuboutResultApproval.Active.SetItemValue("valSuboutResultApproval", false);
                SuboutOrderSubmit.Active["valSuboutOrderSubmit"] = false;
                SuboutAddSample.Enabled["valSuboutAddSample"] = false;
                SuboutRemoveSample.Enabled.SetItemValue("valSuboutRemoveSample", false);
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                {
                    importSuboutResult.Active.SetItemValue("valSuboutImport", true);
                    SuboutSubmit.Active.SetItemValue("valSuboutSubmit", true);
                    //SuboutAllowEdit.Active.SetItemValue("valSuboutAllowEdit", true);
                    SuboutEdit = true;
                    SuboutSigningOff.Active.SetItemValue("valSuboutSigningOff", true);
                    ResultEntryHistorRollbak.Active.SetItemValue("valResultEntryHistorRollbak", true);
                    SuboutResultReview.Active.SetItemValue("valSuboutResultReview", true);
                    SuboutResultApproval.Active.SetItemValue("valSuboutResultApproval", true);
                    SuboutOrderSubmit.Active.SetItemValue("valSuboutOrderSubmit", true);
                    SuboutAddSample.Enabled["valSuboutAddSample"] = true;
                    SuboutRemoveSample.Enabled["valSuboutRemoveSample"] = true;
                }
                else
                {
                    if (objnavigationRefresh.ClickedNavigationItem == "SuboutSampleResultEntry")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SuboutSampleResultEntry" && i.Write == true) != null)
                            {
                                importSuboutResult.Active.SetItemValue("valSuboutImport", true);
                                SuboutSubmit.Active.SetItemValue("valSuboutSubmit", true);
                                //SuboutAllowEdit.Active.SetItemValue("valSuboutAllowEdit", true);
                                ResultEntryHistorRollbak.Active.SetItemValue("valResultEntryHistorRollbak", true);
                            }
                        }
                    }
                    if (objnavigationRefresh.ClickedNavigationItem == "SuboutOrderTracking ")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SuboutOrderTracking " && i.Write == true) != null)
                            {
                                SuboutEdit = true;
                            }
                        }
                    }
                    else if (objnavigationRefresh.ClickedNavigationItem == "Level2SuboutDataReview")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Level2SuboutDataReview" && i.Write == true) != null)
                            {
                                SuboutResultReview.Active.SetItemValue("valSuboutResultReview", true);
                                ResultEntryHistorRollbak.Active.SetItemValue("valResultEntryHistorRollbak", true);
                            }
                        }
                    }
                    else if (objnavigationRefresh.ClickedNavigationItem == "Level3SuboutDataReview")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Level3SuboutDataReview" && i.Write == true) != null)
                            {
                                SuboutResultApproval.Active.SetItemValue("valSuboutResultApproval", true);
                                ResultEntryHistorRollbak.Active.SetItemValue("valResultEntryHistorRollbak", true);
                            }
                        }
                    }
                    else if (objnavigationRefresh.ClickedNavigationItem == "SuboutRegistrationSigningOff")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SuboutRegistrationSigningOff" && i.Write == true) != null)
                            {
                                SuboutSigningOff.Active.SetItemValue("valSuboutSigningOff", true);
                            }
                        }
                    }
                    else if (objnavigationRefresh.ClickedNavigationItem == "SuboutTestOrder")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SuboutTestOrder" && i.Write == true) != null)
                            {
                                SuboutOrderSubmit.Active.SetItemValue("valSuboutOrderSubmit", true);
                                SuboutAddSample.Active["valSuboutAddSample"] = true;
                                SuboutRemoveSample.Active["valSuboutRemoveSample"] = true;
                            }
                        }
                    }
                }
                if (View.Id == "SampleParameter_ListView_Copy_SubOutPendingSamples" || View.Id == "SubOutSampleRegistrations_ListView_Copy_ResultEntry")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    Frame.GetController<NewObjectViewController>().NewObjectAction.CustomGetTotalTooltip += NewObjectAction_CustomGetTotalTooltip;
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    if (View.Id == "SampleParameter_ListView_Copy_SubOutPendingSamples")
                    {
                        DevExpress.ExpressApp.SystemModule.FilterController filterController = Frame.GetController<DevExpress.ExpressApp.SystemModule.FilterController>();
                        if (filterController != null)
                        {
                            filterController.Active.SetItemValue("Search", false);

                        }
                        //IList<SampleParameter> lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [SuboutSample] Is Null And [Samplelogin] Is Not Null And [Testparameter] Is Not Null And [Testparameter.Parameter] Is Not Null And Not IsNullOrEmpty([Testparameter.Parameter.ParameterName]) And [Samplelogin.JobID.Status] <> 'PendingSubmit'"));
                        //List<SampleParameter> distinctSample = lstss.GroupBy(p => new {p.Samplelogin.JobID.JobID,p.Samplelogin.SampleID, p.Testparameter.TestMethod.MatrixName.MatrixName, p.Testparameter.TestMethod.TestName, p.Testparameter.TestMethod.MethodName.MethodNumber }).Select(g => g.First()).ToList();
                        //List<Guid> objOid = distinctSample.Select(i => i.Oid).ToList();
                        //if (objOid.Count > 0)
                        //{
                        //    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", objOid);
                        //}
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = CriteriaOperator.Parse("[SubOut] = True And [SuboutSample] Is Null And [Samplelogin] Is Not Null And [Testparameter] Is Not Null And [Testparameter.Parameter] Is Not Null And Not IsNullOrEmpty([Testparameter.Parameter.ParameterName]) And [Samplelogin.JobID.Status] <> 'PendingSubmit'");
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Executing += SaveAction_Executing;
                        modificationController.SaveAction.Executed += SaveAction_Executed; ;
                        modificationController.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                        modificationController.SaveAndCloseAction.Executed += SaveAndCloseAction_Executed;
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_ListView")
                {
                    Frame.GetController<ListViewController>().EditAction.Executing += EditAction_Executing;
                    if (!SuboutEdit)
                    {
                        ListViewController listViewController = Frame.GetController<ListViewController>();
                        if (listViewController != null)
                        {
                            listViewController.EditAction.Active["SubotEditAction"] = false;
                        }
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_ListView_ViewMode")
                {
                    ListViewController listViewController = Frame.GetController<ListViewController>();
                    if (listViewController != null)
                    {
                        listViewController.EditAction.Active["SubotViewEditAction"] = false;
                    }
                    if (ResultEntryHistoryDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        ResultEntryHistoryDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        ResultEntryHistoryDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        ResultEntryHistoryDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        ResultEntryHistoryDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        ResultEntryHistoryDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        ResultEntryHistoryDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        ResultEntryHistoryDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (ResultEntryHistoryDateFilter.SelectedItem == null)
                    {
                        if (setting.SuboutTracking == EnumDateFilter.OneMonth)
                        {
                            ResultEntryHistoryDateFilter.SelectedItem = ResultEntryHistoryDateFilter.Items[0];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.ThreeMonth)
                        {
                            ResultEntryHistoryDateFilter.SelectedItem = ResultEntryHistoryDateFilter.Items[1];
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 3 And [DateRegistered] Is Not Null");
                }
                        else if (setting.SuboutTracking == EnumDateFilter.SixMonth)
                        {
                            ResultEntryHistoryDateFilter.SelectedItem = ResultEntryHistoryDateFilter.Items[2];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 6 And [DateRegistered] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.OneYear)
                        {
                            ResultEntryHistoryDateFilter.SelectedItem = ResultEntryHistoryDateFilter.Items[3];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.TwoYear)
                        {
                            ResultEntryHistoryDateFilter.SelectedItem = ResultEntryHistoryDateFilter.Items[4];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 2 And [DateRegistered] Is Not Null");

                        }
                        else if (setting.SuboutTracking == EnumDateFilter.FiveYear)
                        {
                            ResultEntryHistoryDateFilter.SelectedItem = ResultEntryHistoryDateFilter.Items[5];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 5 And [DateRegistered] Is Not Null");

                        }
                        else
                        {
                            ResultEntryHistoryDateFilter.SelectedItem = ResultEntryHistoryDateFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria.Clear();
                            ((ListView)View).CollectionSource.Criteria["HistoricalRecordsFilter"] = CriteriaOperator.Parse("[Status] = 'SuboutPendingValidation' or [Status] = 'SuboutPendingApproval' or [Status] = 'IsExported'");
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    //ResultEntryHistoryDateFilter.SelectedIndex = 1;
                    //((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 3 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                }
                else if (View.Id == "SampleParameter_ListView_SuboutSampleHistory")
                {
                    if (SubOutSampleHistoryDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        SubOutSampleHistoryDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        SubOutSampleHistoryDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        SubOutSampleHistoryDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        SubOutSampleHistoryDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        SubOutSampleHistoryDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        SubOutSampleHistoryDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        SubOutSampleHistoryDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //SubOutSampleHistoryDateFilter.SelectedIndex = 1;
                    //((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 3 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (SubOutSampleHistoryDateFilter.SelectedItem == null)
                    {
                        if (setting.SuboutTracking == EnumDateFilter.OneMonth)
                        {
                            SubOutSampleHistoryDateFilter.SelectedItem = SubOutSampleHistoryDateFilter.Items[0];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 1 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.ThreeMonth)
                        {
                            SubOutSampleHistoryDateFilter.SelectedItem = SubOutSampleHistoryDateFilter.Items[1];
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 3 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.SixMonth)
                        {
                            SubOutSampleHistoryDateFilter.SelectedItem = SubOutSampleHistoryDateFilter.Items[2];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 6 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.OneYear)
                        {
                            SubOutSampleHistoryDateFilter.SelectedItem = SubOutSampleHistoryDateFilter.Items[3];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 1 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.TwoYear)
                        {
                            SubOutSampleHistoryDateFilter.SelectedItem = SubOutSampleHistoryDateFilter.Items[4];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 2 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.FiveYear)
                        {
                            SubOutSampleHistoryDateFilter.SelectedItem = SubOutSampleHistoryDateFilter.Items[5];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 5 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                        }
                        else
                        {
                            SubOutSampleHistoryDateFilter.SelectedItem = SubOutSampleHistoryDateFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria.Clear();
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }

                    IList<SampleParameter> lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [SuboutSample] Is Not Null And [Samplelogin] Is Not Null And [Testparameter] Is Not Null And [Testparameter.Parameter] Is Not Null And Not IsNullOrEmpty([Testparameter.Parameter.ParameterName])"));
                    List<SampleParameter> distinctSample = lstss.GroupBy(p => new { p.Testparameter.TestMethod.MatrixName.MatrixName, p.Testparameter.TestMethod.TestName, p.Testparameter.TestMethod.MethodName.MethodNumber }).Select(g => g.First()).ToList();
                    List<Guid> objOid = distinctSample.Select(i => i.Oid).ToList();
                    if (objOid.Count > 0)
                    {
                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", objOid);
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_ListView_TestOrder")
                {
                    if (SubOutSampleTestOrderDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        SubOutSampleTestOrderDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        SubOutSampleTestOrderDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        SubOutSampleTestOrderDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        SubOutSampleTestOrderDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        SubOutSampleTestOrderDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        SubOutSampleTestOrderDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        SubOutSampleTestOrderDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //SubOutSampleHistoryDateFilter.SelectedIndex = 1;
                    //((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 3 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (SubOutSampleTestOrderDateFilter.SelectedItem == null)
                    {
                        if (setting.SuboutTracking == EnumDateFilter.OneMonth)
                        {
                            SubOutSampleTestOrderDateFilter.SelectedItem = SubOutSampleTestOrderDateFilter.Items[0];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.ThreeMonth)
                        {
                            SubOutSampleTestOrderDateFilter.SelectedItem = SubOutSampleTestOrderDateFilter.Items[1];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 3 And [DateRegistered] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.SixMonth)
                        {
                            SubOutSampleTestOrderDateFilter.SelectedItem = SubOutSampleTestOrderDateFilter.Items[2];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 6 And [DateRegistered] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.OneYear)
                        {
                            SubOutSampleTestOrderDateFilter.SelectedItem = SubOutSampleTestOrderDateFilter.Items[3];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.TwoYear)
                        {
                            SubOutSampleTestOrderDateFilter.SelectedItem = SubOutSampleTestOrderDateFilter.Items[4];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 2 And [DateRegistered] Is Not Null");
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.FiveYear)
                        {
                            SubOutSampleTestOrderDateFilter.SelectedItem = SubOutSampleTestOrderDateFilter.Items[5];
                            ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 5 And [DateRegistered] Is Not Null");
                        }
                        else
                        {
                            SubOutSampleTestOrderDateFilter.SelectedItem = SubOutSampleTestOrderDateFilter.Items[6];
                            ((ListView)View).CollectionSource.Criteria.Clear();
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }

                //    IList<SampleParameter> lstss = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [SuboutSample] Is Not Null And [Samplelogin] Is Not Null And [Testparameter] Is Not Null And [Testparameter.Parameter] Is Not Null And Not IsNullOrEmpty([Testparameter.Parameter.ParameterName])"));
                //    List<SampleParameter> distinctSample = lstss.GroupBy(p => new { p.Testparameter.TestMethod.MatrixName.MatrixName, p.Testparameter.TestMethod.TestName, p.Testparameter.TestMethod.MethodName.MethodNumber }).Select(g => g.First()).ToList();
                //    List<Guid> objOid = distinctSample.Select(i => i.Oid).ToList();
                //    if (objOid.Count > 0)
                //    {
                //        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", objOid);
                //    }
                }
                else if (View.Id == "Customer_LookupListView_SuboutClient")
                {
                    if (HttpContext.Current.Session["rowid"] != null)
                    {
                        string TestOid = HttpContext.Current.Session["rowid"].ToString();
                        if (TestOid != null)
                        {
                            SampleParameter objParam = ObjectSpace.FindObject<SampleParameter>(CriteriaOperator.Parse("Oid=?", new Guid(TestOid)));
                            if (objParam != null)
                            {
                                IList<SampleParameter> objParameters = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SuboutSample] Is Null And [SubOut] = True And [Testparameter.TestMethod.MatrixName.MatrixName] = ?" +
                                    " And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ?", objParam.Testparameter.TestMethod.MatrixName.MatrixName,
                                    objParam.Testparameter.TestMethod.TestName, objParam.Testparameter.TestMethod.MethodName.MethodNumber));
                                if (objParameters != null && objParameters.Count > 0)
                                {
                                    List<Guid> objOid = objParameters.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.ClientName != null).Select(i => i.Samplelogin.JobID.ClientName.Oid).Distinct().ToList();
                                    if (objOid.Count > 0)
                                    {
                                        ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", objOid);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Copy_SuboutSampleRegistration" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_SignOff"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_SignOffHistory" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Tracking")
                {
                    //List<SampleParameter> lists = ((ListView)View).CollectionSource.List.Cast<SampleParameter>().ToList();
                    //List<SampleParameter> distinctSample = lists.GroupBy(p => new { p.Samplelogin.SampleID, p.Testparameter.TestMethod.MatrixName.MatrixName, p.Testparameter.TestMethod.TestName, p.Testparameter.TestMethod.MethodName.MethodNumber }).Select(g => g.First()).ToList();
                    //List<Guid> objOid = distinctSample.Select(i => i.Oid).ToList();
                    //if (objOid.Count > 0)
                    //{
                    //    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", objOid);
                    //}

                    //using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleParameter)))
                    //{
                    //    lstview.Properties.Add(new ViewProperty("Matrix", SortDirection.Ascending, "Testparameter.TestMethod.MatrixName.MatrixName", true, true));
                    //    lstview.Properties.Add(new ViewProperty("Test",  SortDirection.Ascending, "Testparameter.TestMethod.TestName", true, true));
                    //    lstview.Properties.Add(new ViewProperty("Method", SortDirection.Ascending, "Testparameter.TestMethod.MethodName.MethodNumber", true, true));
                    //    lstview.Properties.Add(new ViewProperty("TopOid", SortDirection.Ascending, "Max(Oid)", false, true));
                    //    List<object> groups = new List<object>();
                    //    foreach (ViewRecord rec in lstview)
                    //        groups.Add(rec["TopOid"]);
                    //    ((ListView)View).CollectionSource.Criteria["Distinct"] = new InOperator("Oid", groups);
                    //}
                }
                else if (View.Id == "SubOutSampleRegistrations_ListView_NotificationQueueView")
                {
                    if (NotificationQueueDateFilter.Items.Count == 0)
                    {
                        var item1 = new ChoiceActionItem();
                        var item2 = new ChoiceActionItem();
                        var item3 = new ChoiceActionItem();
                        var item4 = new ChoiceActionItem();
                        var item5 = new ChoiceActionItem();
                        var item6 = new ChoiceActionItem();
                        var item7 = new ChoiceActionItem();
                        NotificationQueueDateFilter.Items.Add(new ChoiceActionItem("1M", item1));
                        NotificationQueueDateFilter.Items.Add(new ChoiceActionItem("3M", item2));
                        NotificationQueueDateFilter.Items.Add(new ChoiceActionItem("6M", item3));
                        NotificationQueueDateFilter.Items.Add(new ChoiceActionItem("1Y", item4));
                        NotificationQueueDateFilter.Items.Add(new ChoiceActionItem("2Y", item5));
                        NotificationQueueDateFilter.Items.Add(new ChoiceActionItem("5Y", item6));
                        NotificationQueueDateFilter.Items.Add(new ChoiceActionItem("ALL", item7));
                    }
                    //NotificationQueueDateFilter.SelectedIndex = 1;
                    DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("Oid is not null"));
                    if (NotificationQueueDateFilter.SelectedItem == null)
                    {
                        if (setting.SuboutTracking == EnumDateFilter.OneMonth)
                        {
                            NotificationQueueDateFilter.SelectedItem = NotificationQueueDateFilter.Items[0];
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.ThreeMonth)
                        { 
                            NotificationQueueDateFilter.SelectedItem = NotificationQueueDateFilter.Items[1];
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.SixMonth)
                        {
                            NotificationQueueDateFilter.SelectedItem = NotificationQueueDateFilter.Items[2];
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.OneYear)
                        {
                            NotificationQueueDateFilter.SelectedItem = NotificationQueueDateFilter.Items[3];
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.TwoYear)
                        {
                            NotificationQueueDateFilter.SelectedItem = NotificationQueueDateFilter.Items[4];
                        }
                        else if (setting.SuboutTracking == EnumDateFilter.FiveYear)
                        {
                            NotificationQueueDateFilter.SelectedItem = NotificationQueueDateFilter.Items[5];
                        }
                        else
                        {
                            NotificationQueueDateFilter.SelectedItem = NotificationQueueDateFilter.Items[6];
                        }
                        //reportingDateFilterAction.SelectedItem = reportingDateFilterAction.Items[1];
                    }
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 3 And [DateRegistered] Is Not Null");
                }
                else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                else if (View.Id == "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry")
                {
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Executing += SaveAction_Executing;
                        modificationController.SaveAction.Executed += SaveAction_Executed;
                        modificationController.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
                        modificationController.SaveAndCloseAction.Executed += SaveAndCloseAction_Executed;
                    }
                }
                if(View.Id == "SubOutSampleRegistrations_DetailView_TestOrder" || View.Id == "SubOutSampleRegistrations_ListView_TestOrder" ||View.Id== "SubOutSampleRegistrations_DetailView_Copy")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing += DeleteAction_Executing;
                }
                if (View.Id == "SubOutContractLab_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing += SubOutContractLabSaveAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }

        private void SubOutContractLabSaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                SubOutContractLab getEmailID = View.CurrentObject as SubOutContractLab;
                if (getEmailID.EmailID == null)
                {
                    Application.ShowViewStrategy.ShowMessage("Please enter the 'Email ID' of the contract lab.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
                else if (getEmailID.EmailID != null)
                {
                    //bool isValid = true;
                    string emails = getEmailID.EmailID;
                    string[] emailArray = emails.Split(',');
                    List<string> lstOfIncorrectEmails = new List<string>();
                    foreach (string email in emailArray)
                    {
                        if (!IsValidEmail(email.Trim()))
                        {
                            lstOfIncorrectEmails.Add(email);
                        }
                    }
                    if (lstOfIncorrectEmails.Count > 0)
                    {
                        string invalidEmails = string.Join(", ", lstOfIncorrectEmails);
                        Application.ShowViewStrategy.ShowMessage("' " + invalidEmails + " ' " + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "ValidateTheFormatOfEmails"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\DWQRMessageGroup", "SetupIDCreated"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                // Regular expression to validate email format
                string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return false;
            }
        }

        private void NewObjectAction_CustomGetTotalTooltip(object sender, CustomGetTotalTooltipEventArgs e)
        {
            try
            {
                e.Tooltip = ((ActionBase)sender).Enabled ? "New Subout Registration" : null;
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
                if (e.PopupFrame != null)
                {
                    if (e.PopupFrame.View.Id == "SampleParameter_ListView_Copy_SubOutAddSamples")
                    {
                        e.Width = new System.Web.UI.WebControls.Unit(1210);
                        e.Height = new System.Web.UI.WebControls.Unit(700);
                        e.Handled = true;
                    }
                    if (e.PopupFrame.View.Id == "SubOutContractLab_DetailView")
                    {
                        e.Width = new System.Web.UI.WebControls.Unit(1065);
                        e.Height = new System.Web.UI.WebControls.Unit(400);
                        e.Handled = true;
                    }
                    if (e.PopupFrame.View.Id == "SuboutDeliveryService_DetailView")
                    {
                        e.Width = new System.Web.UI.WebControls.Unit(500);
                        e.Height = new System.Web.UI.WebControls.Unit(195);
                        e.Handled = true;
                    }
                    if (e.PopupFrame.View.Id == "Preservative_DetailView")
                    {
                        e.Width = new System.Web.UI.WebControls.Unit(500);
                        e.Height = new System.Web.UI.WebControls.Unit(260);
                        e.Handled = true;
                    }
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
                SubOutSampleRegistrations objSSR = (SubOutSampleRegistrations)e.InnerArgs.CurrentObject;
                if (objSSR != null)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    if (View.Id == "SubOutSampleRegistrations_ListView_PendingSignOff")
                    {
                        SubOutSampleRegistrations newobjSSR = os.GetObjectByKey<SubOutSampleRegistrations>(objSSR.Oid);
                        DetailView detailview = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_PendingSignOff", true, newobjSSR);
                        detailview.ViewEditMode = ViewEditMode.Edit;
                        Frame.SetView(detailview);
                        e.Handled = true;
                    }
                    else if (View.Id == "SubOutSampleRegistrations_ListView_Copy_ResultEntry")
                    {
                        SubOutSampleRegistrations newobjSSR = os.GetObjectByKey<SubOutSampleRegistrations>(objSSR.Oid);
                        DetailView detailview = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry", true, newobjSSR);
                        detailview.ViewEditMode = ViewEditMode.Edit;
                        Frame.SetView(detailview);
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null && View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {

                    //SubOutSampleRegistrations objSubotSample = (SubOutSampleRegistrations)View.CurrentObject;
                    //SubOutSampleRegistrations objSubout = ObjectSpace.GetObject<SubOutSampleRegistrations>(objSubotSample);
                    //if (SSInfo.IsNewObject)
                    //{
                    //    SendNotificationEmail(objSubotSample);
                    //    objSubout.SuboutNotificationStatus = SuboutNotificationQueueStatus.Send;
                    //    ObjectSpace.CommitChanges();
                    //}
                    //ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                    //if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                    //{
                    //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    //    {
                    //        if (parent.Id == "SampleSubOutTracking")
                    //        {
                    //            foreach (ChoiceActionItem child in parent.Items)
                    //            {
                    //                if (child.Id == "SuboutSampleResultEntry")
                    //                {
                    //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                    IList<SampleParameter> objParam = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [SuboutSample] Is Null And [Samplelogin] Is Not Null And [Testparameter] Is Not Null And [Testparameter.Parameter] Is Not Null And Not IsNullOrEmpty([Testparameter.Parameter.ParameterName])"));
                    //                    if (objParam != null && objParam.Count > 0)
                    //                    {
                    //                        var count = objParam.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID != null).Select(i => i.Samplelogin.JobID.JobID).Distinct().Count();
                    //                        var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        if (count > 0)
                    //                        {
                    //                            child.Caption = cap[0] + " (" + count + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            child.Caption = cap[0];
                    //                        }
                    //                    }
                    //                    break;
                    //                }

                    //                break;
                    //            }
                    //        }

                    //    }
                    //}
                }
                if (View.Id == "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry")
                {
                    SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                    ListPropertyEditor lstviewSuboutSampleQC = ((DetailView)View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    if (lstviewSuboutSampleQC != null && lstviewSuboutSampleQC.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstviewSuboutSampleQC.ListView).Editor).Grid.UpdateEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null && View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {
                    //SSInfo.IsNewObject = false;
                    //SubOutSampleRegistrations objSubotSample = (SubOutSampleRegistrations)View.CurrentObject;
                    //if (View.ObjectSpace.IsNewObject(objSubotSample))
                    //{
                    //    SSInfo.IsNewObject = true;
                    //}
                }
                if (View.Id == "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry")
                {
                    bool IsSeleted = false;
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                    ListPropertyEditor lstviewSuboutSampleQC = ((DetailView)View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null && lstviewSuboutSample.ListView.SelectedObjects.Count > 0)
                    {
                        IsSeleted = true;
                        foreach (SampleParameter setManualEntryDetails in lstviewSuboutSample.ListView.SelectedObjects)
                        {
                            if (setManualEntryDetails.AnalyzedDate != null && setManualEntryDetails.AnalyzedDate != DateTime.MinValue)
                            {
                                if (setManualEntryDetails.AnalyzedDate.Value.Date < setManualEntryDetails.Samplelogin.JobID.RecievedDate.Date)
                                {
                                    Application.ShowViewStrategy.ShowMessage("'AnalyzedDate' should be greater than the received date.", InformationType.Warning, timer.Seconds, InformationPosition.Top);

                                    if (lstviewSuboutSample.ListView.SelectedObjects.Cast<SampleParameter>().FirstOrDefault(i => i.AnalyzedDate.Value.Date < i.Samplelogin.JobID.RecievedDate.Date) != null)
                                    {
                                        setManualEntryDetails.AnalyzedDate = null;
                                    }
                                    e.Cancel = true;
                                }
                                else if (setManualEntryDetails.AnalyzedDate != null && setManualEntryDetails.AnalyzedDate != DateTime.MinValue && setManualEntryDetails.AnalyzedDate.Value.Date > DateTime.Today.Date)
                                {
                                    Application.ShowViewStrategy.ShowMessage("'AnalyzedDate' should not be greater than the current date.", InformationType.Warning, timer.Seconds, InformationPosition.Top);

                                    if (lstviewSuboutSample.ListView.SelectedObjects.Cast<SampleParameter>().FirstOrDefault(i => i.AnalyzedDate != null && i.AnalyzedDate.Value.Date > DateTime.Today.Date) != null)
                                    {
                                        setManualEntryDetails.AnalyzedDate = null;
                                    }
                                    e.Cancel = true;
                                }
                            }
                            if (setManualEntryDetails.AnalyzedDate != null && setManualEntryDetails.AnalyzedDate != DateTime.MinValue && setManualEntryDetails.SuboutAnalyzedBy != null && setManualEntryDetails.Result != null)
                            {
                                setManualEntryDetails.SuboutEnteredBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                setManualEntryDetails.SuboutEnteredDate = DateTime.Now;
                            }
                        }
                    }
                    if (lstviewSuboutSampleQC != null && lstviewSuboutSampleQC.ListView != null && lstviewSuboutSampleQC.ListView.SelectedObjects.Count > 0)
                    {
                        IsSeleted = true;
                        foreach (SuboutQcSample setManualEntryDetails in lstviewSuboutSampleQC.ListView.SelectedObjects)
                        {
                            if (setManualEntryDetails.AnalyzedDate != null && setManualEntryDetails.AnalyzedDate != DateTime.MinValue)
                            {
                                if (setManualEntryDetails.AnalyzedDate.Value.Date < setManualEntryDetails.JobID.RecievedDate.Date)
                                {
                                    Application.ShowViewStrategy.ShowMessage("'AnalyzedDate' should be greater than the received date.", InformationType.Warning, timer.Seconds, InformationPosition.Top);

                                    if (lstviewSuboutSample.ListView.SelectedObjects.Cast<SuboutQcSample>().FirstOrDefault(i => i.AnalyzedDate.Value.Date < i.JobID.RecievedDate.Date) != null)
                                    {
                                        setManualEntryDetails.AnalyzedDate = null;
                                    }
                                    e.Cancel = true;
                                }
                                else if (setManualEntryDetails.AnalyzedDate != null && setManualEntryDetails.AnalyzedDate != DateTime.MinValue && setManualEntryDetails.AnalyzedDate.Value.Date > DateTime.Today.Date)
                                {
                                    Application.ShowViewStrategy.ShowMessage("'AnalyzedDate' should not be greater than the current date.", InformationType.Warning, timer.Seconds, InformationPosition.Top);

                                    if (lstviewSuboutSample.ListView.SelectedObjects.Cast<SuboutQcSample>().FirstOrDefault(i => i.AnalyzedDate.Value.Date > DateTime.Today.Date) != null)
                                    {
                                        setManualEntryDetails.AnalyzedDate = null;
                                    }
                                    e.Cancel = true;
                                }
                            }
                            if (setManualEntryDetails.AnalyzedDate != null && setManualEntryDetails.AnalyzedDate != DateTime.MinValue && setManualEntryDetails.AnalyzedBy != null && setManualEntryDetails.Result != null)
                            {
                                setManualEntryDetails.SuboutEnteredBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                setManualEntryDetails.SuboutEnteredDate = DateTime.Now;
                            }
                        }
                    }
                    if (!IsSeleted)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                    else
                    {
                        //if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                        //{
                        //    if(lstviewSuboutSample.ListView.SelectedObjects.Cast<SampleParameter>().FirstOrDefault(i=>string.IsNullOrEmpty(i.SuboutAnalyzedBy))!=null)
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        e.Cancel = true;
                        //    }
                        //}
                        //if (lstviewSuboutSampleQC != null && lstviewSuboutSampleQC.ListView != null)
                        //{
                        //    if (lstviewSuboutSampleQC.ListView.SelectedObjects.Cast<SuboutQcSample>().FirstOrDefault(i => string.IsNullOrEmpty(i.AnalyzedBy)) != null)
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        e.Cancel = true;
                        //    }
                        //}


                        //if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                        //{
                        //    foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                        //    {
                        //        if (string.IsNullOrEmpty(objparam.SuboutAnalyzedBy))
                        //        {
                        //            Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //            e.Cancel = true;
                        //            break;
                        //        }
                        //    }
                        //}
                        //if (lstviewSuboutSampleQC != null && lstviewSuboutSampleQC.ListView != null)
                        //{
                        //    foreach (SuboutQcSample objparam in lstviewSuboutSampleQC.ListView.SelectedObjects)
                        //    {
                        //        if (string.IsNullOrEmpty(objparam.AnalyzedBy))
                        //        {
                        //            Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //            e.Cancel = true;
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null && View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {
                    SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                    Application.ShowViewStrategy.ShowMessage("The subout ID " + "'" + objSubout.SuboutOrderID + "'" + " was saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                    if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstviewSuboutSample.ListView).Editor).Grid.UpdateEdit();
                    }
                    string strJobID = string.Empty;
                    string strQcType = string.Empty;
                    string strSampleId = string.Empty;
                    string Method = string.Empty;
                    string strMatrix = string.Empty;
                    string strSamplename = string.Empty;
                    string strTest = string.Empty;
                    string strParameter = string.Empty;
                    //SubOutSampleRegistrations objSubout = ObjectSpace.GetObject<SubOutSampleRegistrations>(objSubotSample);
                    //if (SSInfo.IsNewObject)
                    //{
                    //    SendNotificationEmail(objSubotSample);
                    //    if(boolMailSend)
                    //    {
                    //        objSubout.SuboutNotificationStatus = SuboutNotificationQueueStatus.Send;
                    //        View.ObjectSpace.CommitChanges();
                    //        boolMailSend = false;
                    //    }

                    //}
                    string strLocalFile = HttpContext.Current.Server.MapPath(@"~\SuboutResultEntryEDD.xls");
                    if (File.Exists(strLocalFile) == true)
                    {
                        DataTable dtsht1 = new DataTable();
                        DevExpress.Spreadsheet.Workbook workbookOld = new DevExpress.Spreadsheet.Workbook();
                        DevExpress.Spreadsheet.Workbook workbookNew = new DevExpress.Spreadsheet.Workbook();
                        Worksheet worksheet0 = workbookNew.Worksheets[0];
                        workbookOld.LoadDocument(strLocalFile);
                        DevExpress.Spreadsheet.Worksheet worksheet = workbookOld.Worksheets[0];
                        //CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                        CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex,0);
                        DataTable dt = worksheet.CreateDataTable(range, true);
                        for (int col = 0; col < range.ColumnCount; col++)
                        {
                            CellValueType cellType = range[0, col].Value.Type;
                            for (int r = 1; r < range.RowCount; r++)
                            {
                                if (cellType != range[r, col].Value.Type)
                                {
                                    dt.Columns[col].DataType = typeof(string);
                                    break;
                                }
                            }
                        }

                        DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                        exporter.Export();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row1 = dt.Rows[0];
                            if (row1[0].ToString() == dt.Columns[0].Caption)
                            {
                                row1.Delete();
                                dt.AcceptChanges();
                            }
                            foreach (DataColumn c in dt.Columns)
                                c.ColumnName = c.ColumnName.ToString().Trim();
                            IList<SampleParameter> listSample = lstviewSuboutSample.ListView.CollectionSource.List.Cast<SampleParameter>().ToList();
                            if (listSample != null && listSample.Count > 0)
                            {
                                foreach (SampleParameter objParam in listSample)
                                {
                                    //    IList<SampleParameter> objParam = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] =? And" +
                                    //" [Testparameter.TestMethod.TestName]=? And [Testparameter.TestMethod.MethodName.MethodNumber]=? And [SubOut] = True And [SuboutSample] Is Not Null And [Samplelogin.Oid] = ?",
                                    //obj.Testparameter.TestMethod.MatrixName.MatrixName, obj.Testparameter.TestMethod.TestName, obj.Testparameter.TestMethod.MethodName.MethodNumber, obj.Samplelogin.Oid));
                                    if (objParam.Samplelogin != null && objParam.Samplelogin.SampleID != null)
                                    {
                                        strSampleId = objParam.Samplelogin.SampleID;
                                    }
                                    if (objParam.Samplelogin != null && objParam.Samplelogin.JobID != null)
                                    {
                                        strJobID = objParam.Samplelogin.JobID.JobID;
                                    }
                                    if (objParam.Samplelogin != null && objParam.Samplelogin.ClientSampleID != null)
                                    {
                                        strSamplename = objParam.Samplelogin.ClientSampleID;
                                    }
                                    if (objParam.Samplelogin != null && objParam.Samplelogin.VisualMatrix != null)
                                    {
                                        strMatrix = objParam.Samplelogin.VisualMatrix.MatrixName.MatrixName;
                                    }
                                    if (objParam.Samplelogin != null && objParam.Samplelogin.SampleID != null)
                                    {
                                        strTest = objParam.Testparameter.TestMethod.TestName;
                                    }
                                    if (objParam.Testparameter != null && objParam.Testparameter.TestMethod != null && objParam.Testparameter.TestMethod.MethodName != null)
                                    {
                                        Method = objParam.Testparameter.TestMethod.MethodName.MethodNumber;
                                    }
                                    if (objParam.Testparameter != null && objParam.Testparameter.Parameter != null)
                                    {
                                        strParameter = objParam.Testparameter.Parameter.ParameterName;
                                    }
                                    if (objParam.Testparameter != null && objParam.Samplelogin.SampleID != null)
                                    {
                                        strParameter = objParam.Testparameter.Parameter.ParameterName;
                                    }
                                    dt.Rows.Add(strJobID, strQctype, strSampleId, strSamplename, strMatrix, strTest, Method, strParameter);

                                }
                            }
                            worksheet0.Import(dt, true, 0, 0);
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\EDDTemplate")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\EDDTemplate"));
                            }
                            string strFilePath = HttpContext.Current.Server.MapPath(@"~\EDDTemplate");
                            Stream excelStream = File.Create(Path.GetFullPath(strFilePath + "\\SuboutResultEntryEDD.xlsx"));
                            workbookNew.SaveDocument(excelStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                            excelStream.Dispose();
                            using (FileStream fs = File.OpenRead(strFilePath + "\\SuboutResultEntryEDD.xlsx"))
                            {
                                if (objSubout.ResultImportEDD == null)
                                {
                                    objSubout.ResultImportEDD = View.ObjectSpace.CreateObject<FileData>();
                                }
                                objSubout.ResultImportEDD.LoadFromStream(strFilePath + "\\SuboutResultEntryEDD.xlsx", fs);
                                objSubout.ResultImportEDD.FileName = objSubout.SuboutOrderID + "_EDD.xlsx";
                                View.ObjectSpace.CommitChanges();
                            }

                        }
                        //ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                        //if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                        //{
                        //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                        //    {
                        //        if (parent.Id == "SampleSubOutTracking")
                        //        {
                        //            foreach (ChoiceActionItem child in parent.Items)
                        //            {
                        //                if (child.Id == "SuboutRegistrationSigningOff")
                        //                {
                        //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //                    var count = objectSpace.GetObjectsCount(typeof(SubOutSampleRegistrations), CriteriaOperator.Parse("[Status] = 'PendingSigningOff'"));
                        //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        //                    if (count > 0)
                        //                    {
                        //                        child.Caption = cap[0] + " (" + count + ")";
                        //                    }
                        //                    else
                        //                    {
                        //                        child.Caption = cap[0];
                        //                    }
                        //                }
                        //                else if (child.Id == "SuboutSampleRegistration")
                        //                {
                        //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //                    IList<SampleParameter> objParam = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [SuboutSample] Is Null And [Samplelogin] Is Not Null And [Testparameter] Is Not Null And [Testparameter.Parameter] Is Not Null And Not IsNullOrEmpty([Testparameter.Parameter.ParameterName])"));
                        //                    if (objParam != null && objParam.Count > 0)
                        //                    {
                        //                        //var count = objParam.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID != null).Select(i => i.Samplelogin.JobID.JobID).Distinct().Count();
                        //                        var count = objParam.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null).GroupBy(i => i.Testparameter.TestMethod.Oid).Distinct().Count();
                        //                        var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        //                        if (count > 0)
                        //                        {
                        //                            child.Caption = cap[0] + " (" + count + ")";
                        //                        }
                        //                        else
                        //                        {
                        //                            child.Caption = cap[0];
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            break;
                        //        }
                        //    }
                        //}

                    }
                    //Application.ShowViewStrategy.ShowMessage("A SuboutOrderID " + objSubout.SuboutOrderID + " has been created sucessfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    //Application.ShowViewStrategy.ShowMessage("'"+ objSubout.SuboutOrderID +"'"+"Save successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);

                }
                if (View.Id == "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry")
                {
                    SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                    ListPropertyEditor lstviewSuboutSampleQC = ((DetailView)View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    if (lstviewSuboutSampleQC != null && lstviewSuboutSampleQC.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstviewSuboutSampleQC.ListView).Editor).Grid.UpdateEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null && View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {
                    SubOutSampleRegistrations objSubotSample = (SubOutSampleRegistrations)View.CurrentObject;
                    string strJobID = string.Empty;
                    string strQcType = string.Empty;
                    string strSampleId = string.Empty;
                    string Method = string.Empty;
                    string strMatrix = string.Empty;
                    string strSamplename = string.Empty;
                    string strTest = string.Empty;
                    string strParameter = string.Empty;
                    string strLocalFile = HttpContext.Current.Server.MapPath(@"~\SuboutResultEntryEDD.xls");
                    if (File.Exists(strLocalFile) == true)
                    {
                        DataTable dtsht1 = new DataTable();
                        DevExpress.Spreadsheet.Workbook workbookOld = new DevExpress.Spreadsheet.Workbook();
                        DevExpress.Spreadsheet.Workbook workbookNew = new DevExpress.Spreadsheet.Workbook();
                        Worksheet worksheet0 = workbookNew.Worksheets[0];
                        workbookOld.LoadDocument(strLocalFile);
                        DevExpress.Spreadsheet.Worksheet worksheet = workbookOld.Worksheets[0];
                        CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                        DataTable dt = worksheet.CreateDataTable(range, true);
                        for (int col = 0; col < range.ColumnCount; col++)
                        {
                            CellValueType cellType = range[0, col].Value.Type;
                            for (int r = 1; r < range.RowCount; r++)
                            {
                                if (cellType != range[r, col].Value.Type)
                                {
                                    dt.Columns[col].DataType = typeof(string);
                                    break;
                                }
                            }
                        }

                        DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                        exporter.Export();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row1 = dt.Rows[0];
                            if (row1[0].ToString() == dt.Columns[0].Caption)
                            {
                                row1.Delete();
                                dt.AcceptChanges();
                            }
                            foreach (DataColumn c in dt.Columns)
                                c.ColumnName = c.ColumnName.ToString().Trim();
                        }
                        ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;

                        IList<SampleParameter> listSample = lstviewSuboutSample.ListView.CollectionSource.List.Cast<SampleParameter>().ToList();
                        if (listSample != null && listSample.Count > 0)
                        {
                            foreach (SampleParameter obj in listSample)
                            {
                                if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                                {
                                    strSampleId = obj.Samplelogin.SampleID;
                                }
                                if (obj.Samplelogin != null && obj.Samplelogin.JobID != null)
                                {
                                    strJobID = obj.Samplelogin.JobID.JobID;
                                }
                                if (obj.Samplelogin != null && obj.Samplelogin.ClientSampleID != null)
                                {
                                    strSamplename = obj.Samplelogin.ClientSampleID;
                                }
                                if (obj.Samplelogin != null && obj.Samplelogin.VisualMatrix != null)
                                {
                                    strMatrix = obj.Samplelogin.VisualMatrix.MatrixName.MatrixName;
                                }
                                if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                                {
                                    strTest = obj.Testparameter.TestMethod.TestName;
                                }
                                if (obj.Testparameter != null && obj.Testparameter.TestMethod != null && obj.Testparameter.TestMethod.MethodName != null)
                                {
                                    Method = obj.Testparameter.TestMethod.MethodName.MethodNumber;
                                }
                                if (obj.Testparameter != null && obj.Testparameter.Parameter != null)
                                {
                                    strParameter = obj.Testparameter.Parameter.ParameterName;
                                }
                                if (obj.Testparameter != null && obj.Samplelogin.SampleID != null)
                                {
                                    strParameter = obj.Testparameter.Parameter.ParameterName;
                                }

                                dt.Rows.Add(strJobID, strQctype, strSampleId, strSamplename, strMatrix, Method, strTest, strParameter);

                            }
                        }
                        worksheet0.Import(dt, true, 0, 0);
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\EDDTemplate")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\EDDTemplate"));
                        }
                        string strFilePath = HttpContext.Current.Server.MapPath(@"~\EDDTemplate");
                        Stream excelStream = File.Create(Path.GetFullPath(strFilePath + "\\SuboutResultEntryEDD.xlsx"));
                        workbookNew.SaveDocument(excelStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                        excelStream.Dispose();
                        using (FileStream fs = File.OpenRead(strFilePath + "\\SuboutResultEntryEDD.xlsx"))
                        {
                            if (objSubotSample.ResultImportEDD == null)
                            {
                                objSubotSample.ResultImportEDD = View.ObjectSpace.CreateObject<FileData>();
                            }
                            objSubotSample.ResultImportEDD.LoadFromStream(strFilePath + "\\SuboutResultEntryEDD.xlsx", fs);
                            objSubotSample.ResultImportEDD.FileName = objSubotSample.SuboutOrderID + "_EDD.xlsx";
                            View.ObjectSpace.CommitChanges();
                        }
                        SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                        Application.ShowViewStrategy.ShowMessage("The subout ID " + "'" + objSubout.SuboutOrderID + "'" + " was saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);

                    }
                    //ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                    //if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                    //{
                    //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    //    {
                    //        if (parent.Id == "SampleSubOutTracking")
                    //        {
                    //            foreach (ChoiceActionItem child in parent.Items)
                    //            {
                    //                if (child.Id == "SuboutRegistrationSigningOff")
                    //                {
                    //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                    var count = objectSpace.GetObjectsCount(typeof(SubOutSampleRegistrations), CriteriaOperator.Parse("[Status] = 'PendingSigningOff'"));
                    //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                    if (count > 0)
                    //                    {
                    //                        child.Caption = cap[0] + " (" + count + ")";
                    //                    }
                    //                    else
                    //                    {
                    //                        child.Caption = cap[0];
                    //                    }
                    //                }
                    //                else if (child.Id == "SuboutSampleRegistration")
                    //                {
                    //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    //                    IList<SampleParameter> objParam = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [SuboutSample] Is Null And [Samplelogin] Is Not Null And [Testparameter] Is Not Null And [Testparameter.Parameter] Is Not Null And Not IsNullOrEmpty([Testparameter.Parameter.ParameterName])"));
                    //                    if (objParam != null && objParam.Count > 0)
                    //                    {
                    //                        var count = objParam.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID != null).Select(i => i.Samplelogin.JobID.JobID).Distinct().Count();
                    //                        var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                        if (count > 0)
                    //                        {
                    //                            child.Caption = cap[0] + " (" + count + ")";
                    //                        }
                    //                        else
                    //                        {
                    //                            child.Caption = cap[0];
                    //                        }
                    //                    }

                    //                }
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}
                    //SSInfo.IsNewObject = false;
                    //SubOutSampleRegistrations objSubotSample = (SubOutSampleRegistrations)View.CurrentObject;
                    //if (View.ObjectSpace.IsNewObject(objSubotSample))
                    //{
                    //    //View.ObjectSpace.CommitChanges();
                    //    //SubOutSampleRegistrations objSubout = ObjectSpace.GetObject<SubOutSampleRegistrations>(objSubotSample);
                    //    //SendNotificationEmail(objSubotSample);
                    //    //if (boolMailSend)
                    //    //{
                    //    //    objSubout.SuboutNotificationStatus = SuboutNotificationQueueStatus.Send;
                    //    //    View.ObjectSpace.CommitChanges(); 
                    //    //}

                    //}

                }
                if (View.Id == "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry")
                {
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                    if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                    {
                        foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                        {
                            if (string.IsNullOrEmpty(objparam.SuboutAnalyzedBy))
                            {
                                Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
                                break;
                            }
                        }
                    }
                    ListPropertyEditor lstviewSuboutSampleQC = ((DetailView)View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    if (lstviewSuboutSampleQC != null && lstviewSuboutSampleQC.ListView != null)
                    {
                        foreach (SuboutQcSample objparam in lstviewSuboutSampleQC.ListView.SelectedObjects)
                        {
                            if (string.IsNullOrEmpty(objparam.AnalyzedBy))
                            {
                                Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                e.Cancel = true;
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


        private void SendNotificationEmail(SubOutSampleRegistrations SuboutOrderSamples)
        {
            try
            {
                #region 
                IObjectSpace objSpace = Application.CreateObjectSpace();

                foreach (SubOutSampleRegistrations objSubOut in View.SelectedObjects)
                {
                    string strPath = ReportGeneratePdf();
                    SmtpClient sc = new SmtpClient();
                    Employee currentUser = SecuritySystem.CurrentUser as Employee;
                    string strSmtpHost = "Smtp.gmail.com";
                    string strMailFromUserName = currentUser.Email;
                    string strMailFromPassword = currentUser.Password;
                    //string strWebSiteAddress = ((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["WebSiteAddress"];
                    string strMailto = string.Empty;

                    string strTestForSubout = SuboutOrderSamples.TestforSubout;
                    string strContractLab = string.Empty;
                    if (SuboutOrderSamples != null && SuboutOrderSamples.ContractLabName != null && SuboutOrderSamples.ContractLabName.ContractLabName != null)
                    {
                        strContractLab = SuboutOrderSamples.ContractLabName.ContractLabName;
                    }
                    string strSuboutOrderId = SuboutOrderSamples.SuboutOrderID;
                    int strSuboutNoOfSamples = SuboutOrderSamples.NumberofSamples;


                    MailMessage m = new MailMessage();
                    m.IsBodyHtml = true;
                    m.From = new MailAddress(strMailFromUserName);
                    CriteriaOperator cs = CriteriaOperator.Parse("[Contents] = '1'");
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    EmailContentTemplate objent = objectSpace.FindObject<EmailContentTemplate>(cs);
                    if (objent != null && objent.Contents != null)
                    {
                        if (objent != null)
                        {
                            eNotificationContentTemplate objNotification1 = objectSpace.CreateObject<eNotificationContentTemplate>();
                            string objbody = objent.Body.Replace("[ReceivedDate]", objSubOut.DateReceived.ToString()).Replace("[SuboutID]", objSubOut.SuboutOrderID.ToString());
                            objNotification1.Body = objbody;
                            objNotification1.Subject = objent.Subject;
                            m.Body = objNotification1.Body;
                            m.Subject = objNotification1.Subject;
                        }
                    }
                       
                
                    //EmailContentTemplate objEnct = objSpace.FindObject<EmailContentTemplate>(CriteriaOperator.Parse("[SubOutSample] =? ", objSubOut.Oid));
                    //if (objEnct == null)
                    //{

                    //    CriteriaOperator cs = CriteriaOperator.Parse("[Contents] = '1'");
                    //    EmailContentTemplate objent = ObjectSpace.FindObject<EmailContentTemplate>(cs);
                    //    if (objent != null)
                    //    {

                    //        objent.Subject = "SboutOrderID: " + strSuboutOrderId + " - SuboutOrder Notification";
                    //        //objent.Body  = "The Attached PDF Report is for Samples received on " + objSubOut.DateReceived.ToShortDateString() + " submitted with Chain of Custody and EDD " + objSubOut.SuboutOrderID + " processed by the Red River Authority of Texas Environmental Services Laboratory in Wichita Falls, Texas.";
                    //        objent.Body = "The Attached PDF Report is for Samples received on " + objSubOut.DateReceived.ToShortDateString() + " submitted with Chain of Custody and EDD " + objSubOut.SuboutOrderID +".";

                    //        //objent.Body = @"SuboutSample : " + strSuboutOrderId + " has been Assigned.<br><br>"
                    //        //                + "<table border=1 cellpadding=4 cellspacing=0>" +
                    //        //                "<tr><td>ContractLabName:</td>" +
                    //        //                "<td>" + strContractLab + "</td></tr>" +
                    //        //                "<tr><td>Test For Subout:</td>" +
                    //        //                "<td>" + strTestForSubout + "</td></tr>" +  
                    //        //                "<tr><td>Subout ID:</td>" +
                    //        //                "<td>" + objSubOut.SuboutOrderID + "</td></tr>" +
                    //        //                "<tr><td>NoOfSamples:</td>" +
                    //        //                "<td>" + strSuboutNoOfSamples + "</td></tr>" +
                    //        //               "<h2><br/><br/>**THIS SINGLE RESULT DOES NOT TRIGGER A VIOLATION FOR THE SYSTEM**<h2>";
                    //        //objent.Body += "<br/><br/>" + "[AnalyteNotes]";



                    //        //strBody = objent.Body;
                    //        //strBody = "There was an MRL exceedance in the [WaterSystem] at [Facility] for [Analyte].";

                    //        //if (strBody.ToUpper().Contains("LabID Number:") && objSubOut.SuboutOrderID != null)
                    //        //{
                    //        //    strBody = strBody.Replace("@SubOutOrderID", objSubOut.SuboutOrderID);
                    //        //}
                    //        //if (strBody.ToUpper().Contains("Method:") /*&& samplecheckin.Job != null*/)
                    //        //{
                    //        //    strBody = strBody.Replace("Method:", objSubOut.TurnAroundTime.TAT);
                    //        //}
                    //        //if (strBody.ToUpper().Contains("Method:") /*&& samplecheckin.Job != null*/)
                    //        //{
                    //        //    strBody = strBody.Replace("Method:", objSubOut.TurnAroundTime.TAT);
                    //        //}


                    //        //if (strBody.ToUpper().Contains("DWW link:") && objSubOut.DateReceived != null)
                    //        //{
                    //        //    strBody = strBody.Replace("DWW link:", objSubOut.DateReceived.ToString());
                    //        //}
                    //    }

                    //    //strBody += "<br/><br/>**THIS SINGLE RESULT DOES NOT TRIGGER A VIOLATION FOR THE SYSTEM**";

                    //    //strBody += "<br/><br/>" + "[AnalyteNotes]";
                    //    //}
                    //    m.Body = objent.Body;
                    //    m.Subject = objent.Subject;

                    //}






                    if (!string.IsNullOrEmpty(strPath))
                    {
                        var filename = strPath;
                        m.Attachments.Add(new Attachment(filename));
                    }
                    if (SuboutOrderSamples.ContractLabName != null && !string.IsNullOrEmpty(SuboutOrderSamples.ContractLabName.EmailID))
                    {
                        m.To.Add(SuboutOrderSamples.ContractLabName.EmailID);
                    }
                    else if (SuboutOrderSamples.ContractLabName != null && SuboutOrderSamples.ContractLabName.EmailID == null)
                    {
                        Application.ShowViewStrategy.ShowMessage("This sub-lab has no email ID to receive the report. Please verify the respective form.", InformationType.Info, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    
                        NetworkCredential credential = new NetworkCredential();
                        credential.UserName = strMailFromUserName;
                        credential.Password = strMailFromPassword;
                        sc.UseDefaultCredentials = true;
                        sc.Host = strSmtpHost;
                        //sc.Port = 25;
                        sc.Port = Convert.ToInt32(((NameValueCollection)ConfigurationManager.GetSection("emailSettings"))["MailPort"]);
                        sc.EnableSsl = true;
                        sc.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        sc.DeliveryFormat = SmtpDeliveryFormat.SevenBit;
                        sc.Credentials = new NetworkCredential(strMailFromUserName, strMailFromPassword);
                        string sFileName = SuboutOrderSamples.SuboutOrderID + ".xlsx";
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\EDDTemplate")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\EDDTemplate"));
                        }
                        
                        string strFilePath = HttpContext.Current.Server.MapPath(@"~\EDDTemplate");
                        string OriginalPath = strFilePath + sFileName;

                    if (SuboutOrderSamples.ResultImportEDD != null)
                    {
                        using (FileStream fileStream = new FileStream(OriginalPath, FileMode.OpenOrCreate))
                        {

                            SuboutOrderSamples.ResultImportEDD.SaveToStream(fileStream);
                            SuboutOrderSamples.ResultImportEDD.FileName = SuboutOrderSamples.SuboutOrderID + "_EDD.xlsx";
                            fileStream.Position = 0;
                            Attachment data = new Attachment(fileStream, System.Net.Mime.MediaTypeNames.Application.Octet);
                            System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;

                            disposition.FileName = SuboutOrderSamples.SuboutOrderID + "_EDD.xlsx";

                            disposition.Size = fileStream.Length;

                            disposition.CreationDate = System.IO.File.GetCreationTime(OriginalPath);

                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(OriginalPath);

                            disposition.ReadDate = System.IO.File.GetLastAccessTime(OriginalPath);
                            m.Attachments.Add(data);
                            try
                            {
                                if (m.To != null && m.To.Count > 0)
                                {
                                    sc.Send(m);
                                    boolMailSend = true;
                                }
                            }
                            catch (SmtpFailedRecipientsException ex)
                            {
                                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                                {
                                    SmtpStatusCode exstatus = ex.InnerExceptions[i].StatusCode;
                                    if (exstatus == SmtpStatusCode.GeneralFailure || exstatus == SmtpStatusCode.ServiceNotAvailable || exstatus == SmtpStatusCode.SyntaxError || exstatus == SmtpStatusCode.SystemStatus || exstatus == SmtpStatusCode.TransactionFailed)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.Message);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.InnerExceptions[i].FailedRecipient);
                                    }
                                }
                            }
                            data.Dispose();

                            // Delete temp file.

                            if (System.IO.File.Exists(OriginalPath))
                            {

                                System.IO.File.Delete(OriginalPath);

                            }
                        }                   

                    }
               
                    else
                        {
                            try
                            {
                                if (m.To != null && m.To.Count > 0)
                                {
                                    sc.Send(m);
                                    boolMailSend = true;
                                }
                            }
                            catch (SmtpFailedRecipientsException ex)
                            {
                                for (int i = 0; i < ex.InnerExceptions.Length; i++)
                                {
                                    SmtpStatusCode exstatus = ex.InnerExceptions[i].StatusCode;
                                    if (exstatus == SmtpStatusCode.GeneralFailure || exstatus == SmtpStatusCode.ServiceNotAvailable || exstatus == SmtpStatusCode.SyntaxError || exstatus == SmtpStatusCode.SystemStatus || exstatus == SmtpStatusCode.TransactionFailed)
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.Message);
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage(ex.InnerExceptions[i].FailedRecipient);
                                    }
                                }
                            }
                        }
                        #endregion



                    
                } 
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                    .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View is DetailView)
                {
                    SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                    if (objSubout != null)
                    {
                        //if (objSubout.SampleParameter.ToList().FirstOrDefault(i => i.Status != Samplestatus.SuboutPendingApproval || i.Status != Samplestatus.SuboutPendingValidation) != null)
                        //{
                        //    Application.ShowViewStrategy.ShowMessage("Unable to delete since it has been referenced already.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        //    e.Cancel = true;
                        //    return;
                        //}
                        //else 
                        if (objSubout.SuboutStatus != SuboutTrackingStatus.PendingSuboutSubmission && objSubout.SuboutStatus != SuboutTrackingStatus.SuboutSubmitted)
                        {
                            Application.ShowViewStrategy.ShowMessage("Unable to delete since it has been referenced already.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        SubOutSampleRegistrations objSuboutSample = (SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject;
                        foreach (SampleParameter objParam in objSuboutSample.SampleParameter.ToList())
                        {
                            objParam.SuboutSignOff = false;
                            objParam.SuboutSignOffDate = null;
                            objParam.SuboutSignOffBy = null;
                            if (objSuboutSample.SampleParameter.Contains(objParam))
                            {
                                objSuboutSample.SampleParameter.Remove(objParam);
                            }
                        }
                    }
                }
                else
                {
                    foreach (SubOutSampleRegistrations objSubout in View.SelectedObjects)
                    {
                        //if (objSubout.SampleParameter.ToList().FirstOrDefault(i => i.Status != Samplestatus.SuboutPendingApproval || i.Status != Samplestatus.SuboutPendingValidation) != null)
                        //{
                        //    Application.ShowViewStrategy.ShowMessage("Unable to delete since it has been referenced already.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        //    e.Cancel = true;
                        //    return;
                        //}
                        //else 
                        if (objSubout.SuboutStatus != SuboutTrackingStatus.PendingSuboutSubmission && objSubout.SuboutStatus != SuboutTrackingStatus.SuboutSubmitted)
                        {
                            Application.ShowViewStrategy.ShowMessage("Unable to delete since it has been referenced already.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                    }
                    foreach (SubOutSampleRegistrations objSubout in View.SelectedObjects)
                    {
                        foreach (SampleParameter objParam in objSubout.SampleParameter.ToList())
                        {
                            objParam.SuboutSignOff = false;
                            objParam.SuboutSignOffDate = null;
                            objParam.SuboutSignOffBy = null;
                            if (objSubout.SampleParameter.Contains(objParam))
                            {
                                objSubout.SampleParameter.Remove(objParam);
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
                if (base.View != null && e.NewValue != e.OldValue && base.View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {
                    if (View != null && View.CurrentObject == e.Object)
                    {
                        if (e.PropertyName == "DueDate")
                        {
                            SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)e.Object;
                            if (objSubout.DueDate != DateTime.MinValue && objSubout.DueDate != null && objSubout.DueDate < DateTime.Today)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "validduedate"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                objSubout.DueDate = null;
                                return;
                            }
                            else if (objSubout.DueDate != null && objSubout.DueDate >= DateTime.Today)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                IList<Holidays> objHoliday = os.GetObjects<Holidays>(CriteriaOperator.Parse("Oid is Not Null"));
                                var TAT = ((DateTime)objSubout.DueDate).Subtract(DateTime.Today).Days;
                                var dic = new Dictionary<DateTime, DayOfWeek>();
                                for (int i = 0; i < TAT + 1; i++)
                                {
                                    if (objHoliday != null && !objHoliday.Any(x => x.HolidayDate == DateTime.Today.AddDays(i)))
                                        dic.Add(DateTime.Today.AddDays(i), DateTime.Now.AddDays(i).DayOfWeek);
                                }
                                int CountExceptHolidays = dic.Where(x => x.Value != DayOfWeek.Saturday && x.Value != DayOfWeek.Sunday).Count();
                                if (CountExceptHolidays > 1)
                                {
                                    int TATdays = Convert.ToInt32(CountExceptHolidays.ToString());
                                    var days = 0;
                                    var years = 0;
                                    var weeks = 0;
                                    var months = 0;
                                    string temptat = string.Empty;
                                    string stryears = string.Empty;
                                    string strmonths = string.Empty;
                                    string strweeks = string.Empty;
                                    string strdays = string.Empty;
                                    years = (TATdays / 365);
                                    months = (TATdays % 365) / 30;
                                    weeks = (TATdays % 365) / 7;
                                    days = TATdays - ((years * 365) + (weeks * 7));
                                    //years
                                    if (years == 1)
                                    {
                                        stryears = years + " " + "Year";
                                    }
                                    else if (years > 1)
                                    {
                                        stryears = years + " " + "Years";
                                    }
                                    //months
                                    if (months == 1)
                                    {
                                        strmonths = months + " " + "Month";
                                    }
                                    else if (months > 1)
                                    {
                                        strmonths = months + " " + "Months";
                                    }
                                    //week
                                    if (weeks == 1)
                                    {
                                        strweeks = weeks + " " + "Week";
                                    }
                                    else if (weeks > 1)
                                    {
                                        strweeks = weeks + " " + "Weeks";
                                    }
                                    //Days
                                    if (TATdays == 1)
                                    {
                                        strdays = TATdays + " " + "Day";
                                    }
                                    else if (TATdays > 1)
                                    {
                                        strdays = TATdays + " " + "Days";
                                    }

                                    if (years > 0 && months <= 12 && weeks <= 4 && days == 0)
                                    {
                                        temptat = stryears;
                                    }
                                    else
                                    if (months > 0 && weeks <= 4 && years == 0 && days <= 3)
                                    {
                                        temptat = strmonths;
                                    }
                                    else
                                    if (weeks > 0 && months == 0 && years == 0 && days == 0)
                                    {
                                        temptat = strweeks;
                                    }
                                    else
                                    {
                                        temptat = strdays;
                                    }

                                    TurnAroundTime objTAT = os.FindObject<TurnAroundTime>(CriteriaOperator.Parse("TAT=?", temptat));
                                    if (objTAT == null)
                                    {
                                        objTAT = os.CreateObject<TurnAroundTime>();
                                        //objTAT.TAT = CountExceptHolidays.ToString();
                                        objTAT.Count = TATdays * 24;
                                        objTAT.TAT = temptat;

                                        os.CommitChanges();
                                    }
                                    objSubout.TurnAroundTime = ObjectSpace.GetObject<TurnAroundTime>(objTAT);
                                }
                                else
                                {
                                    string sameDay;
                                    CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse(""));
                                    if (currentLanguage != null && currentLanguage.Chinese)
                                    {
                                        sameDay = "同一天";
                                    }
                                    else
                                    {
                                        sameDay = "Same Day";
                                    }

                                    TurnAroundTime objTAT = os.FindObject<TurnAroundTime>(CriteriaOperator.Parse("TAT=?", sameDay));
                                    if (objTAT == null)
                                    {
                                        objTAT = os.CreateObject<TurnAroundTime>();
                                        objTAT.TAT = sameDay;
                                        os.CommitChanges();
                                    }
                                    objSubout.TurnAroundTime = ObjectSpace.GetObject<TurnAroundTime>(objTAT);
                                }
                            }
                        }
                    }
                }
                if (View != null && View.Id == "SubOutContractLab_CertifiedTests_ListView")
                {
                    if (Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "SubOutContractLab_DetailView" && Application.MainWindow.View.CurrentObject != null)
                    {
                        SubOutContractLab curtcontlab = (SubOutContractLab)Application.MainWindow.View.CurrentObject;
                        if (e.Object != null && e.PropertyName == "Matrix")
                        {
                            contractinfo.listTestMethod = new List<TestMethod>();
                            CertifiedTests curttst = (CertifiedTests)e.Object;
                            if (curttst != null && curttst.Matrix != null)
                            {
                                if (curtcontlab != null && curtcontlab.CertifiedTests.Count > 0)
                                {
                                    List<TestMethod> lsttest = new List<TestMethod>();
                                    foreach (CertifiedTests objtest in curtcontlab.CertifiedTests.ToList())
                                    {
                                        TestMethod objtstmed = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = 'False'", objtest.Matrix.MatrixName));
                                        if (objtstmed != null)
                                        {
                                            if (!contractinfo.listTestMethod.Contains(objtstmed))
                                            {
                                                contractinfo.listTestMethod.Add(objtstmed);
                                            }
                                        }
                                    }
                                    curttst.Test = null;
                                    curttst.Method = null;
                                }
                            }
                        }
                        else if (e.Object != null && e.PropertyName == "Test")
                        {
                            contractinfo.listMethod = new List<TestMethod>();
                            CertifiedTests curttst = (CertifiedTests)e.Object;
                            if (curttst != null && curttst.Matrix != null && curttst.Test != null)
                            {
                                if (curtcontlab != null && curtcontlab.CertifiedTests.Count > 0)
                                {
                                    List<TestMethod> lstmed = new List<TestMethod>();
                                    foreach (CertifiedTests objtest in curtcontlab.CertifiedTests.ToList())
                                    {
                                        lstmed.Add(objtest.Method);
                                    }
                                    foreach (CertifiedTests objtest in curtcontlab.CertifiedTests.ToList())
                                    {
                                        List<TestMethod> lsttstmed = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [IsGroup] = 'False'", objtest.Matrix.MatrixName, objtest.Test.TestName)).ToList();
                                        if (lsttstmed != null && lsttstmed.Count > 0)
                                        {
                                            foreach (TestMethod objtstmed in lsttstmed.ToList())
                                            {
                                                if (!contractinfo.listMethod.Contains(objtstmed) && !lstmed.Contains(objtstmed))
                                                {
                                                    contractinfo.listMethod.Add(objtstmed);
                                                }
                                            }
                                        }
                                    }
                                    curttst.Method = null;
                                }
                            }
                        }
                        else if (e.Object != null && e.PropertyName == "Method")
                        {
                            contractinfo.listMethod = new List<TestMethod>();
                            CertifiedTests curttst = (CertifiedTests)e.Object;
                            if (curttst != null && curttst.Matrix != null && curttst.Test != null)
                            {
                                if (curtcontlab != null && curtcontlab.CertifiedTests.Count > 0)
                                {
                                    List<TestMethod> lstmed = new List<TestMethod>();
                                    foreach (CertifiedTests objtest in curtcontlab.CertifiedTests.ToList())
                                    {
                                        lstmed.Add(objtest.Method);
                                    }
                                    foreach (CertifiedTests objtest in curtcontlab.CertifiedTests.ToList())
                                    {
                                        List<TestMethod> lsttstmed = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [IsGroup] = 'False'", objtest.Matrix.MatrixName, objtest.Test.TestName)).ToList();
                                        if (lsttstmed != null && lsttstmed.Count > 0)
                                        {
                                            foreach (TestMethod objtstmed in lsttstmed.ToList())
                                            {
                                                if (!contractinfo.listMethod.Contains(objtstmed) && !lstmed.Contains(objtstmed))
                                                {
                                                    contractinfo.listMethod.Add(objtstmed);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                if (View != null && View.Id == "CertifiedTests_DetailView")
                {
                    if (Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "SubOutContractLab_DetailView" && Application.MainWindow.View.CurrentObject != null)
                    {
                        SubOutContractLab curtcontlab = (SubOutContractLab)Application.MainWindow.View.CurrentObject;
                        if (e.Object != null && e.PropertyName == "Matrix")
                        {
                            CertifiedTests curttst = (CertifiedTests)e.Object;
                            if (curttst != null && curttst.Matrix != null)
                            {
                                if (curtcontlab != null && curtcontlab.CertifiedTests.Count > 0)
                                {
                                    List<TestMethod> lsttest = new List<TestMethod>();
                                    foreach (CertifiedTests objtest in curtcontlab.CertifiedTests.ToList())
                                    {
                                        TestMethod objtstmed = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [MethodName.MethodNumber] = ?", objtest.Matrix.MatrixName, objtest.Test.TestName, objtest.Method.MethodName.MethodNumber));
                                        if (objtstmed != null)
                                        {
                                            lsttest.Add(objtstmed);
                                        }
                                    }
                                    if (lsttest != null && lsttest.Count > 0)
                                    {
                                        if (contractinfo.listTestMethod == null)
                                        {
                                            contractinfo.listTestMethod = new List<TestMethod>();
                                        }
                                        else
                                        {
                                            contractinfo.listTestMethod = new List<TestMethod>();
                                        }
                                        List<TestMethod> lsttm = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = 'False'", curttst.Matrix.MatrixName)).ToList();
                                        foreach (TestMethod objtm in lsttm.ToList())
                                        {
                                            if (!lsttest.Contains(objtm))
                                            {
                                                if (!contractinfo.listTestMethod.Contains(objtm))
                                                {
                                                    contractinfo.listTestMethod.Add(objtm);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (curttst.Matrix != null)
                                {
                                    List<string> lsttestname = new List<string>();
                                    List<TestMethod> lsttestpara = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = False", curttst.Matrix.MatrixName)).ToList();
                                    foreach (TestMethod objtm in lsttestpara.Cast<TestMethod>().Where(i => i.MatrixName.MatrixName == curttst.Matrix.MatrixName).ToList())
                                    {
                                        if (!lsttestname.Contains(objtm.TestName))
                                        {
                                            lsttestname.Add(objtm.TestName);
                                            contractinfo.listTestMethod.Add(objtm);
                                        }
                                    }
                                }
                                curttst.Test = null;
                                curttst.Method = null;
                            }
                        }
                        else if (e.Object != null && e.PropertyName == "Test")
                        {
                            CertifiedTests curttst = (CertifiedTests)e.Object;
                            if (curttst != null && curttst.Matrix != null && curttst.Test != null)
                            {
                                if (curtcontlab != null && curtcontlab.CertifiedTests.Count > 0)
                                {
                                    List<TestMethod> lsttest = new List<TestMethod>();
                                    foreach (CertifiedTests objtest in curtcontlab.CertifiedTests.ToList())
                                    {
                                        TestMethod objtstmed = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [MethodName.MethodNumber] = ?", objtest.Matrix.MatrixName, objtest.Test.TestName, objtest.Method.MethodName.MethodNumber));
                                        if (objtstmed != null)
                                        {
                                            lsttest.Add(objtstmed);
                                        }
                                    }
                                    if (lsttest != null && lsttest.Count > 0)
                                    {
                                        if (contractinfo.listMethod == null)
                                        {
                                            contractinfo.listMethod = new List<TestMethod>();
                                        }
                                        else
                                        {
                                            contractinfo.listMethod = new List<TestMethod>();
                                        }
                                        List<TestMethod> lsttm = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [IsGroup] = 'False'", curttst.Matrix.MatrixName, curttst.Test.TestName)).ToList();
                                        foreach (TestMethod objtm in lsttm.ToList())
                                        {
                                            if (!lsttest.Contains(objtm))
                                            {
                                                if (!contractinfo.listMethod.Contains(objtm))
                                                {
                                                    contractinfo.listMethod.Add(objtm);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (curttst.Test != null)
                                {
                                    List<string> lsttestname = new List<string>();
                                    List<TestMethod> lsttestpara = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [IsGroup] = False", curttst.Matrix.MatrixName, curttst.Test.TestName)).ToList();
                                    foreach (TestMethod objtm in lsttestpara.Cast<TestMethod>().Where(i => i.MatrixName.MatrixName == curttst.Matrix.MatrixName && i.TestName == curttst.Test.TestName).ToList())
                                    {
                                        if (!lsttestname.Contains(objtm.MethodName.MethodNumber))
                                        {
                                            lsttestname.Add(objtm.MethodName.MethodNumber);
                                            contractinfo.listMethod.Add(objtm);
                                        }
                                    }
                                }
                                curttst.Method = null;
                            }
                        }
                    }
                }
                //if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView")
                //{
                //    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    SampleParameter objSP = (SampleParameter)e.Object;
                //    //SampleParameter objSP1 = ((ListView)View).ObjectSpace.GetObject<SampleParameter>(objSP);
                //    foreach (SampleParameter objSP2 in ((ListView)View).CollectionSource.List.Cast<SampleParameter>().ToList().Where(i=>i.Oid== objSP.Oid))
                //    {
                //        if (objSP != null)
                //        {
                //            gridListEditor.Grid.Selection.SelectRowByKey(objSP2.Oid);
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

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View.Id == "CertifiedTests_DetailView")
                {
                    CertifiedTests crttest = (CertifiedTests)View.CurrentObject;
                    if (crttest != null && Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "SubOutContractLab_DetailView")
                    {
                        SubOutContractLab suboutlabobj = (SubOutContractLab)Application.MainWindow.View.CurrentObject;
                        if (suboutlabobj != null && suboutlabobj.CertifiedTests.Count > 0)
                        {
                            List<TestMethod> lsttest = new List<TestMethod>();
                            foreach (CertifiedTests objtest in suboutlabobj.CertifiedTests.ToList())
                            {
                                TestMethod objtstmed = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [MethodName.MethodNumber] = ?", objtest.Matrix.MatrixName, objtest.Test.TestName, objtest.Method.MethodName.MethodNumber));
                                if (objtstmed != null)
                                {
                                    lsttest.Add(objtstmed);
                                }
                            }
                            if (lsttest != null && lsttest.Count > 0)
                            {
                                contractinfo.listMatrix = new List<Matrix>();
                                //Matrixfilter
                                List<TestMethod> lsttm = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("")).ToList();
                                foreach (TestMethod objtm in lsttm.ToList())
                                {
                                    if (!lsttest.Contains(objtm))
                                    {
                                        if (!contractinfo.listMatrix.Contains(objtm.MatrixName))
                                        {
                                            contractinfo.listMatrix.Add(objtm.MatrixName);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            contractinfo.listMatrix = new List<Matrix>();
                            contractinfo.listTestMethod = new List<TestMethod>();
                            contractinfo.listMethod = new List<TestMethod>();
                        }
                    }

                }
                if (View.Id == "SubOutContractLab_CertifiedTests_ListView")
                {
                    if (Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "SubOutContractLab_DetailView")
                    {
                        SubOutContractLab curtcontlab = (SubOutContractLab)Application.MainWindow.View.CurrentObject;
                        if (curtcontlab != null && curtcontlab.CertifiedTests.Count > 0)
                        {
                            List<TestMethod> lsttest = new List<TestMethod>();
                            foreach (CertifiedTests objtest in curtcontlab.CertifiedTests.ToList())
                            {
                                if (objtest.Matrix != null && objtest.Test != null && objtest.Method != null)
                                {
                                    TestMethod objtstmed = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName] = ? And [MethodName.MethodNumber] = ?", objtest.Matrix.MatrixName, objtest.Test.TestName, objtest.Method.MethodName.MethodNumber));
                                    if (objtstmed != null)
                                    {
                                        lsttest.Add(objtstmed);
                                    }
                                }

                            }
                            if (lsttest != null && lsttest.Count > 0)
                            {
                                List<TestMethod> lsttm = ObjectSpace.GetObjects<TestMethod>(CriteriaOperator.Parse("[IsGroup] = 'False'")).ToList();
                                contractinfo.listMethod = new List<TestMethod>();
                                contractinfo.listMatrix = new List<Matrix>();
                                contractinfo.listTestMethod = new List<TestMethod>();
                                foreach (TestMethod objtm in lsttm.ToList())
                                {
                                    if (!lsttest.Contains(objtm))
                                    {
                                        if (!contractinfo.listTestMethod.Contains(objtm))
                                        {
                                            contractinfo.listTestMethod.Add(objtm);
                                        }
                                        if (!contractinfo.listMethod.Contains(objtm))
                                        {
                                            contractinfo.listMethod.Add(objtm);
                                        }
                                        if (!contractinfo.listMatrix.Contains(objtm.MatrixName))
                                        {
                                            contractinfo.listMatrix.Add(objtm.MatrixName);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            contractinfo.listMatrix = new List<Matrix>();
                            contractinfo.listTestMethod = new List<TestMethod>();
                            contractinfo.listMethod = new List<TestMethod>();
                        }
                    }
                }
                if (View.Id == "Parameter_ListView_ContractLab")
                {
                    ASPxGridListEditor gridlist = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridlist != null && gridlist.Grid != null)
                    {
                        gridlist.Grid.Settings.VerticalScrollableHeight = 300;
                    }
                }
                if (View.Id == "SubOutSampleRegistrations_ListView_Copy_SuboutDelivery" || View.Id == "SubOutSampleRegistrations_ListView_Copy_SuboutDelivered")
                {

                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor.Grid != null)
                    {
                        ASPxGridView gridView = gridListEditor.Grid;
                        gridView.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        if (View.Id == "SubOutSampleRegistrations_ListView_Copy_SuboutDelivery" || View.Id == "SubOutSampleRegistrations_ListView_Copy_SuboutDelivered")
                        {
                            //gridView.SelectionChanged += Grid_SelectionChanged;
                            gridView.ClientSideEvents.SelectionChanged = @"function(s,e){
                                var i = s.cpPagesize * s.GetPageIndex();
                                var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                                if (e.visibleIndex == -1 && !s.IsRowSelectedOnPage(0)) {
                                for (i; i < totrow; i++) {
                                 if(s.batchEditApi.GetCellValue(i, 'DateDelivered') != null && s.batchEditApi.HasChanges(i,'DateDelivered')){
                                 s.batchEditApi.SetCellValue(i, 'DateDelivered', null);}
                                }
                                }
                                else if(e.visibleIndex == -1 && s.IsRowSelectedOnPage(0)){
                                var today = new Date();
                                for (i; i < totrow; i++) { 
                                if(s.batchEditApi.GetCellValue(i, 'DateDelivered') == null && !s.batchEditApi.HasChanges(i,'DateDelivered')){
                                s.batchEditApi.SetCellValue(i, 'DateDelivered', today);}
                                }
                                }
                                else{
                                if (s.IsRowSelectedOnPage(e.visibleIndex)) {            
                                 var today = new Date();
                                 if(s.batchEditApi.GetCellValue(e.visibleIndex, 'DateDelivered') == null && !s.batchEditApi.HasChanges(e.visibleIndex,'DateDelivered')){
                                 s.batchEditApi.SetCellValue(e.visibleIndex, 'DateDelivered', today);}  
                                }
                                else{
                                if(s.batchEditApi.GetCellValue(e.visibleIndex, 'DateDelivered') != null && s.batchEditApi.HasChanges(e.visibleIndex,'DateDelivered')){
                                 s.batchEditApi.SetCellValue(e.visibleIndex, 'DateDelivered', null);} 
                                }
                               }  
                            }";
                        }
                    }
                }
                else if(View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue" || View.Id == "SubOutSampleRegistrations_ListView_NotificationQueueView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                    gridListEditor.Grid.JSProperties["cpusername"] = SecuritySystem.CurrentUserName;
                    if (View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue" || View.Id == "SubOutSampleRegistrations_ListView_NotificationQueueView")
                    {
                        if (gridListEditor.Grid.Columns["SelectionCommandColumn"] != null)
                        {
                            gridListEditor.Grid.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        if (gridListEditor.Grid.Columns["NotificationQueueMailReSend"] != null)
                        {
                            gridListEditor.Grid.Columns["NotificationQueueMailReSend"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        if (gridListEditor.Grid.Columns["SuboutOrderID"] != null)
                        {
                            gridListEditor.Grid.Columns["SuboutOrderID"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        if (gridListEditor.Grid.Columns["SuboutEDDTempalte"] != null)
                        {
                            gridListEditor.Grid.Columns["SuboutEDDTempalte"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        //if (gridListEditor.Grid.Columns["TestforSubout"] != null)
                        //{
                        //    gridListEditor.Grid.Columns["TestforSubout"].FixedStyle = GridViewColumnFixedStyle.Left;
                        //    gridListEditor.Grid.Columns["TestforSubout"].Width = 180;
                        //}
                        if (gridListEditor.Grid.Columns["EDDTemplateAttached"] != null)
                        {
                            gridListEditor.Grid.Columns["EDDTemplateAttached"].Width = 140;
                        }
                        if (gridListEditor.Grid.Columns["LabAddress"] != null)
                        {
                            gridListEditor.Grid.Columns["LabAddress"].Width = 140;
                        }
                    }


                }
                else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView" /*|| View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Copy_SuboutSampleRegistration"*/
                                                                                         /*|| View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Tracking"*/ || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_ViewMode"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_QCResults" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_QCResultsView"
                    /*|| View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue" */|| View.Id == "SubOutSampleRegistrations_ListView_NotificationQueueView"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data"
                    /*|| View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_SignOff"*/ || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3QcResults"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2QcResults" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_SignOffHistory"
                    || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                    gridListEditor.Grid.JSProperties["cpusername"] = SecuritySystem.CurrentUserName;
                    //if (View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue" || View.Id == "SubOutSampleRegistrations_ListView_NotificationQueueView")
                    //{
                    //    if (gridListEditor.Grid.Columns["SelectionCommandColumn"] != null)
                    //    {
                    //        gridListEditor.Grid.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                    //    }
                    //    //if (gridListEditor.Grid.Columns["NotificationQueueMailReSend"] != null)
                    //    //{
                    //    //    gridListEditor.Grid.Columns["NotificationQueueMailReSend"].FixedStyle = GridViewColumnFixedStyle.Left;
                    //    //}
                    //    if (gridListEditor.Grid.Columns["SuboutOrderID"] != null)
                    //    {
                    //        gridListEditor.Grid.Columns["SuboutOrderID"].FixedStyle = GridViewColumnFixedStyle.Left;
                    //    }
                    //    if (gridListEditor.Grid.Columns["SuboutEDDTempalte"] != null)
                    //    {
                    //        gridListEditor.Grid.Columns["SuboutEDDTempalte"].FixedStyle = GridViewColumnFixedStyle.Left;
                    //    }
                    //    //if (gridListEditor.Grid.Columns["TestforSubout"] != null)
                    //    //{
                    //    //    gridListEditor.Grid.Columns["TestforSubout"].FixedStyle = GridViewColumnFixedStyle.Left;
                    //    //    gridListEditor.Grid.Columns["TestforSubout"].Width = 180;
                    //    //}
                    //    if (gridListEditor.Grid.Columns["EDDTemplateAttached"] != null)
                    //    {
                    //        gridListEditor.Grid.Columns["EDDTemplateAttached"].Width = 140;
                    //    }
                    //    if (gridListEditor.Grid.Columns["LabAddress"] != null)
                    //    {
                    //        gridListEditor.Grid.Columns["LabAddress"].Width = 140;
                    //    }
                    //}
                    if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView")
                    {
                        if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView")
                        {
                            if (SSInfo.EditColumnNameSample == null)
                            {
                                SSInfo.EditColumnNameSample = new List<string>();
                                foreach (ColumnWrapper wrapper in gridListEditor.Columns)
                                {
                                    IModelColumn columnModel = ((ListView)View).Model.Columns[wrapper.PropertyName];
                                    if (columnModel != null && columnModel.AllowEdit == true && !SSInfo.EditColumnNameSample.Contains(columnModel.Id + ".Oid") && columnModel.PropertyEditorType == typeof(ASPxLookupPropertyEditor))
                                    {
                                        SSInfo.EditColumnNameSample.Add(columnModel.Id + ".Oid");
                                    }
                                    else if (columnModel != null && columnModel.AllowEdit == true && !SSInfo.EditColumnNameSample.Contains(columnModel.Id) && columnModel.PropertyEditorType != typeof(ASPxLookupPropertyEditor))
                                    {
                                        SSInfo.EditColumnNameSample.Add(columnModel.Id);
                                    }
                                }
                            }
                            if (SSInfo.EditColumnNameSample.Count > 0)
                            {
                                gridListEditor.Grid.JSProperties["cpeditcolumnname"] = SSInfo.EditColumnNameSample;
                            }
                        }
                        else if (View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView")
                        {
                            if (SSInfo.EditColumnNameQCSample == null)
                            {
                                SSInfo.EditColumnNameQCSample = new List<string>();
                                foreach (ColumnWrapper wrapper in gridListEditor.Columns)
                                {
                                    IModelColumn columnModel = ((ListView)View).Model.Columns[wrapper.PropertyName];
                                    if (columnModel != null && columnModel.AllowEdit == true && !SSInfo.EditColumnNameQCSample.Contains(columnModel.Id + ".Oid") && columnModel.PropertyEditorType == typeof(ASPxLookupPropertyEditor))
                                    {
                                        SSInfo.EditColumnNameQCSample.Add(columnModel.Id + ".Oid");
                                    }
                                    else if (columnModel != null && columnModel.AllowEdit == true && !SSInfo.EditColumnNameQCSample.Contains(columnModel.Id) && columnModel.PropertyEditorType != typeof(ASPxLookupPropertyEditor))
                                    {
                                        SSInfo.EditColumnNameQCSample.Add(columnModel.Id);
                                    }
                                }
                            }
                            if (SSInfo.EditColumnNameQCSample.Count > 0)
                            {
                                gridListEditor.Grid.JSProperties["cpeditcolumnname"] = SSInfo.EditColumnNameQCSample;
                            }
                        }
                        gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e){
                         if( s.GetEditor('ResultNumeric')!=null)
                            {
                              s.GetEditor('ResultNumeric').KeyPress.AddHandler(OnResultNumericChanged);
                            }
                          if( s.GetEditor('Result')!=null)
                           {
                            s.GetEditor('Result').KeyPress.AddHandler(OnResultNumericChanged);
                           }
                           if( s.GetEditor('NumericResult')!=null)
                           {
                            s.GetEditor('NumericResult').KeyPress.AddHandler(OnResultNumericChanged);
                           }
                         //for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                         //{                                                                                 
                         //   if (s.batchEditApi.GetCellValue(i, 'DF') == null) {

                         //       s.batchEditApi.SetCellValue(i, 'DF', '1');
                         //   }                                                                                
                         //}
                        }";
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        {
                            var fieldName = e.cellInfo.column.fieldName;
                            if (s.cpeditcolumnname.includes(e.cellInfo.column.fieldName))
                               {
                              var fieldName = e.cellInfo.column.fieldName; 
                             sessionStorage.setItem('ResultEntryFocusedColumn', fieldName); 
                               }
                             else
                               {
                               e.cancel=true;
                               }
                        }";
                        gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                                {   
                           
                                var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');                                
                                var oid;
                                var text;
                                if(FocusedColumn.includes('.'))
                                {                        
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
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
                                 else
                                 {  
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                    if (e.item.name =='CopyToAllCell')
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
                             e.processOnServer = false;
                        }";
                        gridListEditor.Grid.ClientSideEvents.BatchEditConfirmShowing = @"function(s,e) 
                          { 
                         e.cancel = true;
                          }";
                    }
                    #region Subout Calculation
                    //                    else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_QCResults"
                    //                                    /*|| View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView"*/)
                    //                    {
                    //                        gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = false;
                    //                        gridListEditor.Grid.JSProperties["cpPagesize"] = gridListEditor.Grid.SettingsPager.PageSize;
                    //                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    //                        gridListEditor.Grid.CustomDataCallback += Grid_CustomDataCallback;
                    //                        gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    //                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    //                        gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    //                        gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    //                        ICallbackManagerHolder parameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    //                        parameter.CallbackManager.RegisterHandler("SuboutResultEntry", this);
                    //                        gridListEditor.Grid.ClientSideEvents.Init = @"function (s, e) {
                    //    s.GetEditor('ResultNumeric').KeyPress.AddHandler(OnResultNumericChanged);
                    //    var i = s.cpPagesize * s.GetPageIndex();
                    //    var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                    //    for (i; i < totrow; i++) {
                    //        if (s.batchEditApi.GetCellValue(i, 'DF') == null) {

                    //            s.batchEditApi.SetCellValue(i, 'DF', '1');
                    //        }
                    //    }
                    //    window.onclick = function () {
                    //        var name = s.GetEditor('ResultNumeric').name;
                    //if(name != null)
                    //{
                    //        var grid = name.split('_');
                    //        grid.pop();
                    //        grid.push('DXDataRow');
                    //        name = grid.join('_');
                    //        window.setTimeout(function () {
                    //            var i = s.cpPagesize * s.GetPageIndex();
                    //            var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                    //            for (i; i < totrow; i++) {
                    //                if (s.batchEditApi.HasChanges(i, 'ResultNumeric') && s.batchEditApi.HasChanges(i, 'Result')) {
                    //                    var LOQ = parseFloat(s.batchEditApi.GetCellValue(i, 'LOQ'));
                    //                    var UQL = parseFloat(s.batchEditApi.GetCellValue(i, 'UQL'));
                    //                    var ResultNumeric = parseFloat(s.batchEditApi.GetCellValue(i, 'ResultNumeric'));
                    //                    var RptLimit = parseFloat(s.batchEditApi.GetCellValue(i, 'RptLimit'));
                    //                    if (ResultNumeric != null && LOQ != null && UQL != null && RptLimit != null && ResultNumeric > RptLimit) {
                    //                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', ResultNumeric < LOQ || ResultNumeric > UQL);
                    //                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', false);
                    //                    } else if (ResultNumeric == RptLimit) {
                    //                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', true);
                    //                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', false);
                    //                    }
                    //                }
                    //            }
                    //        }, 10);
                    //}
                    //    }
                    //    s.OnScroll = function () {
                    //        if (s.GetHorizontalScrollPosition() < 400) {
                    //            var name = s.GetEditor('ResultNumeric').name;
                    //if(name != null)
                    //{
                    //            var grid = name.split('_');
                    //            grid.pop();
                    //            grid.push('DXDataRow');
                    //            name = grid.join('_');
                    //            var i = s.cpPagesize * s.GetPageIndex();
                    //            var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                    //            for (i; i < totrow; i++) {
                    //                if (s.batchEditApi.HasChanges(i, 'ResultNumeric') && s.batchEditApi.HasChanges(i, 'Result')) {
                    //                    var LOQ = parseFloat(s.batchEditApi.GetCellValue(i, 'LOQ'));
                    //                    var UQL = parseFloat(s.batchEditApi.GetCellValue(i, 'UQL'));
                    //                    var ResultNumeric = parseFloat(s.batchEditApi.GetCellValue(i, 'ResultNumeric'));
                    //                    var RptLimit = parseFloat(s.batchEditApi.GetCellValue(i, 'RptLimit'));
                    //                    if (ResultNumeric != null && LOQ != null && UQL != null && RptLimit != null && ResultNumeric > RptLimit) {
                    //                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', ResultNumeric < LOQ || ResultNumeric > UQL);
                    //                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', false);
                    //                    } else if (ResultNumeric == RptLimit) {
                    //                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', true);
                    //                        $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', false);
                    //                    }
                    //                }
                    //            }
                    //}
                    //        }
                    //    };
                    //}";
                    //                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function (s, e) {
                    //    sessionStorage.setItem('ResultEntryFocusedColumn', null);
                    //    if ((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit')) {
                    //        e.cancel = true;
                    //    } else if (e.cellInfo.column.fieldName == 'ResultNumeric' || e.cellInfo.column.fieldName == 'Result' || e.cellInfo.column.fieldName == 'SurrogateUnits.Oid'
                    //         || e.cellInfo.column.fieldName == 'Units.Oid' || e.cellInfo.column.fieldName == 'Comment' || e.cellInfo.column.fieldName == 'FinalResult' || e.cellInfo.column.fieldName == 'FinalResultUnits.Oid' || e.cellInfo.column.fieldName == 'NumResult'
                    //         || e.cellInfo.column.fieldName == 'AnalyzedBy.Oid' || e.cellInfo.column.fieldName == 'AnalyzedDate') {
                    //        var fieldName = e.cellInfo.column.fieldName;
                    //        sessionStorage.setItem('ResultEntryFocusedColumn', fieldName);
                    //    } else {
                    //        sessionStorage.setItem('ResultEntryFocusedColumn', fieldName);
                    //        //e.cancel = true;
                    //    }
                    //    var name = s.GetEditor('ResultNumeric').name;
                    //if(name != null)
                    //{
                    //    var grid = name.split('_');
                    //    grid.pop();
                    //    grid.push('DXDataRow');
                    //    name = grid.join('_');
                    //    window.setTimeout(function () {
                    //        var i = s.cpPagesize * s.GetPageIndex();
                    //        var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                    //        for (i; i < totrow; i++) {
                    //            if (s.batchEditApi.HasChanges(i, 'ResultNumeric') && s.batchEditApi.HasChanges(i, 'Result')) {
                    //                var LOQ = parseFloat(s.batchEditApi.GetCellValue(i, 'LOQ'));
                    //                var UQL = parseFloat(s.batchEditApi.GetCellValue(i, 'UQL'));
                    //                var ResultNumeric = parseFloat(s.batchEditApi.GetCellValue(i, 'ResultNumeric'));
                    //                var RptLimit = parseFloat(s.batchEditApi.GetCellValue(i, 'RptLimit'));
                    //                if (ResultNumeric != null && LOQ != null && UQL != null && RptLimit != null && ResultNumeric > RptLimit) {
                    //                    $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', ResultNumeric < LOQ || ResultNumeric > UQL);
                    //                    $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', false);
                    //                } else if (ResultNumeric == RptLimit) {
                    //                    $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', true);
                    //                    $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', false);
                    //                }
                    //            }
                    //        }
                    //    }, 10);
                    //}
                    //}";
                    //                        gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function (s, e) {
                    //    if (s.IsRowSelectedOnPage(e.elementIndex)) {
                    //        var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');
                    //        var oid;
                    //        var text;
                    //        if (FocusedColumn.includes('.')) {
                    //            oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
                    //            text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;
                    //            if (e.item.name == 'CopyToAllCell') {
                    //            var i = s.cpPagesize * s.GetPageIndex();
                    //            var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                    //            for (i; i < totrow; i++) {
                    //                    if (s.IsRowSelectedOnPage(i)) {
                    //                        s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                    //                    }
                    //                }
                    //            }
                    //        } else {
                    //            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);
                    //            var parameter;
                    //            if (e.item.name == 'CopyToAllCell') {
                    //                 var i = s.cpPagesize * s.GetPageIndex();
                    //                 var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                    //                 for (i; i < totrow; i++) {
                    //                    if (s.IsRowSelectedOnPage(i)) {
                    //                        console.log(FocusedColumn);
                    //                        if (FocusedColumn == 'ResultNumeric') {
                    //                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                    //                            var name = s.GetEditor('ResultNumeric').name;
                    //if(name != null)
                    //{
                    //                            var grid = name.split('_');
                    //                            grid.pop();
                    //                            grid.push('DXDataRow');
                    //                            name = grid.join('_');
                    //                            if (s.batchEditApi.GetCellValue(i, 'RptLimit') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString().length > 0) {
                    //                                var ResultNumeric = s.batchEditApi.GetCellValue(i, 'ResultNumeric');
                    //                                var LOQ = s.batchEditApi.GetCellValue(i, 'LOQ');
                    //				                var UQL = s.batchEditApi.GetCellValue(i, 'UQL');
                    //                                if (parameter == null || parameter.length == 0) {
                    //                                    parameter = ResultNumeric + '|' + i  + '|' + LOQ + '|' + UQL;
                    //                                } else {
                    //                                    parameter = parameter + ';' + ResultNumeric + '|' + i +  '|' + LOQ + '|' + UQL;
                    //                                }
                    //                            } else {
                    //                                if (s.batchEditApi.GetCellValue(i, 'RptLimit') == null && s.batchEditApi.GetCellValue(i, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString().length > 0) {
                    //                                    var ResultNumeric = s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString();
                    //                                    s.batchEditApi.SetCellValue(i, 'Result', ResultNumeric);
                    //                                } else {
                    //                                    s.batchEditApi.SetCellValue(i, 'Result', null);
                    //                                }
                    //                                $('#' + name + i + ' td[fieldName=Result]').removeClass('redCell');
                    //                                $('#' + name + i + ' td[fieldName=Result]').removeClass('yellowCell');
                    //                            }
                    //                        } else
                    //                            {
                    //                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                    //                            }
                    // }                      
                    //                    }
                    //                    else if(FocusedColumn == 'AnalyzedBy' || FocusedColumn == 'AnalyzedDate')
                    //                      {
                    //                         console.log(CopyValue);
                    //                         s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                    //                      }
                    //                }               
                    //                if (parameter != null) {
                    //                    window.startProgress();
                    //                    s.GetValuesOnCustomCallback(parameter, OnGetValuesOnCustomCallbackComplete);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    e.processOnServer = false;
                    //}";
                    //                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e) {
                    //    window.setTimeout(function () {
                    //            var name = s.GetEditor('ResultNumeric').name;
                    //if(name != null)
                    //{
                    //            s.SelectRowOnPage(e.visibleIndex, true);
                    //            var grid = name.split('_');
                    //            grid.pop();
                    //            grid.push('DXDataRow');
                    //            name = grid.join('_');
                    //        var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');
                    //        if (FocusedColumn == 'ResultNumeric') {           
                    //            if (s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric').toString().length > 0) {
                    //                var ResultNumeric = s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric');
                    //                var LOQ = s.batchEditApi.GetCellValue(e.visibleIndex, 'LOQ');
                    //				var UQL = s.batchEditApi.GetCellValue(e.visibleIndex, 'UQL');
                    //                var parameter = ResultNumeric + '|' + e.visibleIndex + '|' + LOQ + '|' + UQL+ '|ResultNumeric';
                    //                window.startProgress();
                    //                RaiseXafCallback(globalCallbackControl, 'SuboutResultEntry',parameter,'', false);
                    //                //s.GetValuesOnCustomCallback(parameter, OnGetValuesOnCustomCallbackComplete);
                    //            } else {
                    //                if (s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') == null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric').toString().length > 0) {
                    //                    var ResultNumeric = s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric');
                    //                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', ResultNumeric);
                    //                } else {
                    //                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', null);
                    //                }
                    //                $('#' + name + e.visibleIndex + ' td[fieldName=Result]').removeClass('redCell');
                    //                $('#' + name + e.visibleIndex + ' td[fieldName=Result]').removeClass('yellowCell');
                    //            }
                    //        }
                    //}
                    //    }, 10);
                    //}";
                    //                    } 
                    #endregion

                    else if (View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView")
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function (s, e) {
    window.setTimeout(function () {
                                    s.SelectRowOnPage(e.visibleIndex, true);
    }, 10);
}";
                    }
                    else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2QcResults")
                    {
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;

                    }
                    else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data"
                        || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3QcResults")
                    {
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
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
                        else if (item.GetType() == typeof(ASPxGridLookupPropertyEditor))
                        {
                            ASPxGridLookupPropertyEditor propertyEditor = item as ASPxGridLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxEnumPropertyEditor))
                        {
                            ASPxEnumPropertyEditor propertyEditor = item as ASPxEnumPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
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
                        else if (item.GetType() == typeof(ASPxIntPropertyEditor))
                        {
                            ASPxIntPropertyEditor propertyEditor = item as ASPxIntPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                        }
                    }
                }
                else if (View.Id == "SampleParameter_ListView_Copy_SubOutPendingSamples")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ICallbackManagerHolder parameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    parameter.CallbackManager.RegisterHandler("ClientName", this);
                    gridListEditor.Grid.ClientInstanceName = "ClientNames";
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                }
                if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_ViewMode"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_QCResults" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_QCResultsView"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Copy_SuboutSampleRegistration" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_SignOff"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_SignOffHistory" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Copy_SuboutSampleRegistration")
                    {
                        gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                        gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                           {
                              sessionStorage.setItem('ResultEntryFocusedColumn', null);
                              if ((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit')) 
                                {
                                    e.cancel = true;
                                } 
                                else if (e.cellInfo.column.fieldName == 'ContainerType.Oid' || e.cellInfo.column.fieldName == 'ContainerType') 
                                {
                                    var fieldName = e.cellInfo.column.fieldName;
                                    sessionStorage.setItem('ResultEntryFocusedColumn', fieldName);
                                } 
                                else 
                                {
                                    sessionStorage.setItem('ResultEntryFocusedColumn', fieldName);
                                    e.cancel = true;
                                }
                           }";
                        gridView.KeyboardSupport = true;
                        //gridListEditor.Grid. += gridControl_ProcessGridKey;
                        //gridView.Load += gridControl_ProcessGridKey;
                        if (gridView == null)
                            gridListEditor.ControlsCreated += editor_ControlsCreated;
                        else SetClientInstanceName(gridView);
                        gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');
                                    var text;
alert(FocusedColumn);
                                    if (FocusedColumn.includes('.'))
                                    { 
alert('includes .');
                                        oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
                                        text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;
                                        if (e.item.name =='CopyToAllCell')
                                        {
                                           alert(e.item.name);     
                                            for(var i = 0; i < s.cpVisibleRowCount; i++)
                                            { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                {                                               
                                                    s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);
alert('else');
                                    
                                        if (e.item.name =='CopyToAllCell')
                                        {
                                            for(var i = 0; i < s.cpVisibleRowCount; i++)
                                            { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                {
                                                    s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
alert(FocusedColumn);
alert('sets');
                                                }
                                            }
                                        }                                         
                                    }
                                }
                                e.processOnServer = false;
                            }";
                    }
                    if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView")
                    {
                        gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                        gridView.ClientSideEvents.FocusedCellChanging = @"function (s, e) 
{
    sessionStorage.setItem('ResultEntryFocusedColumn', null);
    if ((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit')) 
    {
        e.cancel = true;
    } 
    else if (e.cellInfo.column.fieldName == 'AnalyzedDate' || e.cellInfo.column.fieldName == 'AnalyzedBy.Oid' || e.cellInfo.column.fieldName == 'SuboutAnalyzedBy' ||e.cellInfo.column.fieldName == 'ResultNumeric' || e.cellInfo.column.fieldName == 'Result' || e.cellInfo.column.fieldName == 'SurrogateUnits.Oid'
         || e.cellInfo.column.fieldName == 'Units.Oid' || e.cellInfo.column.fieldName == 'Comment' || e.cellInfo.column.fieldName == 'FinalResult' || e.cellInfo.column.fieldName == 'FinalResultUnits.Oid' || e.cellInfo.column.fieldName == 'NumResult'
|| e.cellInfo.column.fieldName == 'DF' || e.cellInfo.column.fieldName == 'LOQ' || e.cellInfo.column.fieldName == 'UQL' || e.cellInfo.column.fieldName == 'MDL' || e.cellInfo.column.fieldName == 'RptLimit') 
    {
        var fieldName = e.cellInfo.column.fieldName;
        sessionStorage.setItem('ResultEntryFocusedColumn', fieldName);
    } 
    else 
    {
        sessionStorage.setItem('ResultEntryFocusedColumn', fieldName);
        e.cancel = true;
    }
    //var name = s.GetEditor('ResultNumeric').name;
    //if(name != null)
    //{
    //var grid = name.split('_');
    //grid.pop();
    //grid.push('DXDataRow');
    //name = grid.join('_');
    //window.setTimeout(function () 
    //{
    //    var i = s.cpPagesize * s.GetPageIndex();
    //    var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
    //    for (i; i < totrow; i++) 
    //        {
    //        if (s.batchEditApi.HasChanges(i, 'ResultNumeric') && s.batchEditApi.HasChanges(i, 'Result')) 
    //            {
    //            var LOQ = parseFloat(s.batchEditApi.GetCellValue(i, 'LOQ'));
    //            var UQL = parseFloat(s.batchEditApi.GetCellValue(i, 'UQL'));
    //            var ResultNumeric = parseFloat(s.batchEditApi.GetCellValue(i, 'ResultNumeric'));
    //            var RptLimit = parseFloat(s.batchEditApi.GetCellValue(i, 'RptLimit'));

    //                if (ResultNumeric != null && LOQ != null && UQL != null && RptLimit != null && ResultNumeric > RptLimit)
    //                {
    //                $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', ResultNumeric < LOQ || ResultNumeric > UQL);
    //                $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', false);
    //                } 
    //                else if (ResultNumeric == RptLimit) 
    //                {
    //                $('#' + name + i + ' td[fieldName=Result]').toggleClass('yellowCell', true);
    //                $('#' + name + i + ' td[fieldName=Result]').toggleClass('redCell', false);
    //                }
    //            }
    //        }
    //}, 10);
    //}
}";
//                        gridView.ClientSideEvents.BatchEditEndEditing = @"function (s, e) {
//            window.setTimeout(function () {
//            var name = s.GetEditor('ResultNumeric').name;
//            if(name != null)
//            {
//                var grid = name.split('_');
//                grid.pop();
//                grid.push('DXDataRow');
//                name = grid.join('_');
//                var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');
//                if (FocusedColumn == 'ResultNumeric')
//                {
//                    if (s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') != '' && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric').toString().length > 0)
//                    {
//                        var ResultNumeric = s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric');
//                        if (s.batchEditApi.GetCellValue(e.visibleIndex, 'DefaultResult') != null && e.visibleIndex < 0 )
//                        {
//                            var defaultResult = s.batchEditApi.GetCellValue(e.visibleIndex, 'DefaultResult');
//                            var rptLimit = s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit');
//                            if (ResultNumeric < rptLimit && defaultResult != null)
//	                            {
//                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', defaultResult);
//	                            }
//                                else
//	                            {
//                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', ResultNumeric);
//	                            }
//                        }
//                        else
//	                    {
//                            s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', ResultNumeric);
//	                    }
//                        //var LOQ = s.batchEditApi.GetCellValue(e.visibleIndex, 'LOQ');
//				        //var UQL = s.batchEditApi.GetCellValue(e.visibleIndex, 'UQL');
//                        //var parameter = ResultNumeric + '|' + e.visibleIndex + '|' + LOQ + '|' + UQL;
//                        //window.startProgress();
//                        //s.GetValuesOnCustomCallback(parameter, OnGetValuesOnCustomCallbackComplete);
//                    } 
//                    else 
//                    {
//                    if ((s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') == null || s.batchEditApi.GetCellValue(e.visibleIndex, 'RptLimit') == '') && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric').toString().length > 0) 
//                    {
//                        var ResultNumeric = s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric');
//                        s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', ResultNumeric);
//                    } 
//                    else 
//                    {
//                    var ResultNumeric = s.batchEditApi.GetCellValue(e.visibleIndex, 'ResultNumeric');
//                        s.batchEditApi.SetCellValue(e.visibleIndex, 'Result', null);
//                    }
//                    $('#' + name + e.visibleIndex + ' td[fieldName=Result]').removeClass('redCell');
//                    $('#' + name + e.visibleIndex + ' td[fieldName=Result]').removeClass('yellowCell');
//                    }
//                } 
//            }
//    }, 10);
//}";
                        gridView.KeyboardSupport = true;
                        //gridListEditor.Grid. += gridControl_ProcessGridKey;
                        //gridView.Load += gridControl_ProcessGridKey;
                        if (gridView == null)
                            gridListEditor.ControlsCreated += editor_ControlsCreated;
                        else SetClientInstanceName(gridView);
                        gridView.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
        var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');
        var text;
                                if(FocusedColumn.includes('.'))
                                {  
            oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
            text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                       alert(e.item.name);     
                                        for(var i = 0; i < s.cpVisibleRowCount; i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {                                               
                        s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                    }
                }
            }
                                }
                                else
                                {
            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);
                                    //var ResultNumeric = s.batchEditApi.GetCellValue(e.elementIndex, 'ResultNumeric');
                                    //var rptLimit = s.batchEditApi.GetCellValue(e.elementIndex, 'RptLimit');
                                    //var defaultResult = s.batchEditApi.GetCellValue(e.elementIndex, 'DefaultResult');
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.cpVisibleRowCount; i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                                    //s.batchEditApi.SetCellValue(i, 'Result', ResultNumeric);
                                }
                }
                }                                         
            }
        }
    e.processOnServer = false;
}";
                        gridListEditor.Grid.ClientSideEvents.BatchEditConfirmShowing = @"function(s,e) 
                                { 
                                  e.cancel = true;
                                }";
                    }
                    else
                    {
                        gridView.ClientSideEvents.ContextMenuItemClick = @"function (s, e) {
    if (s.IsRowSelectedOnPage(e.elementIndex)) {
        var FocusedColumn = sessionStorage.getItem('ResultEntryFocusedColumn');
        var oid;
        var text;
        if (FocusedColumn.includes('.')) {
            oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
            text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;
            if (e.item.name == 'CopyToAllCell') {
            var i = s.cpPagesize * s.GetPageIndex();
            var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
            for (i; i < totrow; i++) {
                    if (s.IsRowSelectedOnPage(i)) {
                        s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                    }
                }
            }
        } else {
            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);
            var parameter;
            if (e.item.name == 'CopyToAllCell') {
                 var i = s.cpPagesize * s.GetPageIndex();
                 var totrow = i + s.GetVisibleRowsOnPage(s.GetPageIndex());
                 for (i; i < totrow; i++) {
                    if (s.IsRowSelectedOnPage(i)) {
                        if (FocusedColumn == 'ResultNumeric') {
                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                            var name = s.GetEditor('ResultNumeric').name;
                            if(name != null)
    {
                            //var grid = name.split('_');
                            //grid.pop();
                            //grid.push('DXDataRow');
                            //name = grid.join('_');
                            if (s.batchEditApi.GetCellValue(i, 'RptLimit') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString().length > 0) {
                                var ResultNumeric = s.batchEditApi.GetCellValue(i, 'ResultNumeric');
                                var LOQ = s.batchEditApi.GetCellValue(i, 'LOQ');
				                var UQL = s.batchEditApi.GetCellValue(i, 'UQL');
                                if (parameter == null || parameter.length == 0) {
                                    parameter = ResultNumeric + '|' + i  + '|' + LOQ + '|' + UQL;
                                } else {
                                    parameter = parameter + ';' + ResultNumeric + '|' + i +  '|' + LOQ + '|' + UQL;
                                }
                            } else {
                                if (s.batchEditApi.GetCellValue(i, 'RptLimit') == null && s.batchEditApi.GetCellValue(i, 'ResultNumeric') != null && s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString().length > 0) {
                                    var ResultNumeric = s.batchEditApi.GetCellValue(i, 'ResultNumeric').toString();
                                    s.batchEditApi.SetCellValue(i, 'Result', ResultNumeric);
                                } else {
                                    s.batchEditApi.SetCellValue(i, 'Result', null);
                                }
                                $('#' + name + i + ' td[fieldName=Result]').removeClass('redCell');
                                $('#' + name + i + ' td[fieldName=Result]').removeClass('yellowCell');
                            }
}
                        } else {
                            s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                        }
                    }
                }               
                if (parameter != null) {
                    window.startProgress();
                    //s.GetValuesOnCustomCallback(parameter, OnGetValuesOnCustomCallbackComplete);
                }
            }
        }
    }
    e.processOnServer = false;
}";
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level3" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level2"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data_History" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data_History"
                    || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level2_History" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level3_History" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_ResltEntryView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    if (View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level3" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level2")
                    {
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                    }
                }
                else if (View.Id == "SampleParameter_ListView_Copy_SubOutAddSamples")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 400;
                    }
                }
                if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_TestOrder")
                {
                    if (Application.MainWindow.View is DetailView && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.View)
                    {
                        SuboutAddSample.Enabled["valSuboutAddSample"] = false;
                        SuboutRemoveSample.Enabled["valSuboutRemoveSample"] = false;
                    }
                    else
                    {
                        SuboutAddSample.Enabled["valSuboutAddSample"] = true;
                        SuboutRemoveSample.Enabled["valSuboutRemoveSample"] = true;
                    }
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                           {
                              sessionStorage.setItem('SuboutFocusedColumn', null); 
                              var fieldName = e.cellInfo.column.fieldName;                       
                              sessionStorage.setItem('SuboutFocusedColumn', fieldName);                            
                           }";
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                                {   
                           
                                var FocusedColumn = sessionStorage.getItem('SuboutFocusedColumn');                                
                                var oid;
                                var text;
                                if(FocusedColumn=='ContainerType.Oid'|| FocusedColumn=='Preservative.Oid' || FocusedColumn=='HoldTime')
                                  {
                                if(FocusedColumn.includes('.'))
                                {                        
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
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
                                 else
                                 {                                                             
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                    if (e.item.name =='CopyToAllCell')
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
                             e.processOnServer = false;
                        }";

                }
                //else if (View.Id == "SubOutSampleRegistrations_ListView_Copy_ResultEntry")
                //{
                //    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                //    gridListEditor.Grid.SettingsBehavior.a = false;
                //    // ((ListView)View).CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("Status=?", SuboutStatus.PendingResultEntry);
                //}
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
                ASPxGridView gridView = sender as ASPxGridView;
                e.Properties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                //e.Properties["cpFilterRowCount"] = gridView.Selection.FilteredCount;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SetClientInstanceName(ASPxGridView gridView)
        {
            try
            {
                gridView.ClientInstanceName = "Grid";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void editor_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                ASPxGridListEditor editor = (ASPxGridListEditor)sender;
                SetClientInstanceName(editor.Grid);
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

                if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2QcResults")
                {
                    foreach (SampleParameter objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            objSampleResult.SuboutValidatedDate = DateTime.Now;
                            objSampleResult.SuboutValidatedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objSampleResult.SuboutValidatedDate = null;
                            objSampleResult.SuboutValidatedBy = null;
                        }
                    }
                    View.Refresh();
                }
                else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3QcResults")
                {
                    foreach (SampleParameter objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            objSampleResult.SuboutApprovedDate = DateTime.Now;
                            objSampleResult.SuboutApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objSampleResult.SuboutApprovedDate = null;
                            objSampleResult.SuboutApprovedBy = null;
                        }
                    }
                    View.Refresh();
                }
                else if (View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level3")
                {
                    foreach (SuboutQcSample objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            objSampleResult.SuboutApprovedDate = DateTime.Now;
                            objSampleResult.SuboutApprovedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objSampleResult.SuboutApprovedDate = null;
                            objSampleResult.SuboutApprovedBy = null;
                        }
                    }
                    View.Refresh();
                }
                else if (View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level2")
                {
                    foreach (SuboutQcSample objSampleResult in ((ListView)View).CollectionSource.List)
                    {
                        if (selected.Contains(objSampleResult))
                        {
                            objSampleResult.SuboutValidatedDate = DateTime.Now;
                            objSampleResult.SuboutValidatedBy = CS.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                        }
                        else
                        {
                            objSampleResult.SuboutValidatedBy = null;
                            objSampleResult.SuboutValidatedDate = null;
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
        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {

            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse("Oid is null"));
                    if (currentLanguage != null && currentLanguage.Chinese)
                    {
                        e.Items.Add("复制到所有单元格", "CopyToAllCell");
                    }
                    else
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    }
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
        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "SampleParameter_ListView_Copy_SubOutPendingSamples")
                {
                    if (e.DataColumn.FieldName != "SuboutSamplesClients") return;
                    e.Cell.Attributes.Add("onclick", "RaiseXafCallback(globalCallbackControl, 'ClientName'," + e.VisibleIndex + " , '', false);");
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
                if (View is ListView)
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
                }
                SuboutSubmit.Active.SetItemValue("valSuboutSubmit", true);
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                Frame.GetController<ListViewController>().EditAction.Executing -= EditAction_Executing;
                if (View.Id == "SubOutSampleRegistrations_ListView")
                {
                    SuboutEdit = false;
                    ListViewController listViewController = Frame.GetController<ListViewController>();
                    if (listViewController != null)
                    {
                        listViewController.EditAction.Active.RemoveItem("SubotEditAction");
                        listViewController = null;
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_ListView_ViewMode")
                {
                    ListViewController listViewController = Frame.GetController<ListViewController>();
                    if (listViewController != null)
                    {
                        listViewController.EditAction.Active.RemoveItem("SubotViewEditAction");
                        listViewController = null;
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Execute -= SaveAction_Executed;
                        modificationController.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                    }
                    bool mailSend = false;
                }
                else if (View.Id == "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry")
                {
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Executing -= SaveAction_Executing;
                        modificationController.SaveAction.Executed -= SaveAction_Executed;
                        modificationController.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                        modificationController.SaveAndCloseAction.Executed -= SaveAndCloseAction_Executed;
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_DetailView_Copy")
                {
                    ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Executing -= SaveAction_Executing;
                        modificationController.SaveAction.Executed -= SaveAction_Executed; ;
                        modificationController.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
                        modificationController.SaveAndCloseAction.Executed -= SaveAndCloseAction_Executed;
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_DetailView_TestOrder" || View.Id == "SubOutSampleRegistrations_ListView_TestOrder" ||View.Id== "SubOutSampleRegistrations_DetailView_Copy")
                {
                    Frame.GetController<DeleteObjectsViewController>().DeleteAction.Executing -= DeleteAction_Executing;
                }
                if (View.Id == "SubOutContractLab_DetailView")
                {
                    Frame.GetController<ModificationsController>().SaveAction.Executing -= SubOutContractLabSaveAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            base.OnDeactivated();
        }
        private void EditAction_Executing(object seneder, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "SubOutSampleRegistrations_ListView")
                {
                    e.Cancel = true;
                    IObjectSpace os = Application.CreateObjectSpace();
                    SubOutSampleRegistrations objView = (SubOutSampleRegistrations)os.GetObject<SubOutSampleRegistrations>((SubOutSampleRegistrations)View.CurrentObject);
                    if (objView != null)
                    {
                        DetailView dv = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_SuboutOrderTracking", true, objView);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        //e.ShowViewParameters.CreatedView = dv;
                        Frame.SetView(dv);
                    }
                }
                else if (View.Id == "SubOutSampleRegistrations_ListView_Copy_ResultEntry")
                {
                    e.Cancel = true;
                    IObjectSpace os = Application.CreateObjectSpace();
                    SubOutSampleRegistrations objView = (SubOutSampleRegistrations)os.GetObject<SubOutSampleRegistrations>((SubOutSampleRegistrations)View.CurrentObject);
                    if (objView != null)
                    {
                        DetailView dv = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry", true, objView);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Frame.SetView(dv);
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
                if (View.Id == "SubOutSampleRegistrations_ListView")
                {
                    e.Cancel = true;
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(objectSpace, typeof(SampleParameter));
                    ListView createListview = Application.CreateListView("SampleParameter_ListView_Copy_SubOutPendingSamples", cs, false);
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = createListview;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.CloseOnCurrentObjectProcessing = false;
                    dc.Accepting += Dc_Accepting;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                else if (View.Id == "SampleParameter_ListView_Copy_SubOutPendingSamples")
                {
                    e.Cancel = true;
                    IObjectSpace os = Application.CreateObjectSpace();
                    if (View.SelectedObjects.Count > 0)
                    {
                        SubOutSampleRegistrations objSuboutSample = os.CreateObject<SubOutSampleRegistrations>();
                        //DefaultSetting objSettingSubout = os.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'SuboutRegistrationSigningOff'"));
                        //if (objSettingSubout!=null && objSettingSubout.Select)
                        //{
                        //    objSuboutSample.Status = SuboutStatus.PendingSigningOff;
                        //}
                        //else
                        //{
                        //    objSuboutSample.Status = SuboutStatus.PendingResultEntry;

                        //}
                        objSuboutSample.SuboutStatus = SuboutTrackingStatus.PendingSuboutSubmission;
                        objSuboutSample.Status = SuboutStatus.PendingResultEntry;
                        foreach (SampleParameter obj in View.SelectedObjects)
                        {
                            //    IList<SampleParameter> objParam = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] =? And" +
                            //" [Testparameter.TestMethod.TestName]=? And" +
                            //"[Testparameter.TestMethod.MethodName.MethodNumber]=? And [SubOut] = True And [SuboutSample] Is Null", obj.Testparameter.TestMethod.MatrixName.MatrixName, obj.Testparameter.TestMethod.TestName,
                            // obj.Testparameter.TestMethod.MethodName.MethodNumber));
                            //if (objParam != null && objParam.Count > 0)
                            //{
                            //    foreach (SampleParameter objsampleParam in objParam)
                            //    {
                            //        SampleParameter objSample = os.GetObject<SampleParameter>(objsampleParam);
                            //        if (objSample != null && !objSuboutSample.SampleParameter.Contains(objSample))
                            //        {
                            //            objSuboutSample.SampleParameter.Add(objSample);
                            //            objSample.Container = 1;
                            //            if (!objSettingSubout.Select)
                            //            {
                            //                objSample.SuboutSignOff = true;
                            //                objSample.SuboutSignOffDate = DateTime.Now;
                            //                objSample.SuboutSignOffBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                            //            }
                            //        }
                            //    }

                            //}
                            SampleParameter objSample = os.GetObject<SampleParameter>(obj);
                            if (objSample != null && !objSuboutSample.SampleParameter.Contains(objSample))
                            {
                                objSuboutSample.SampleParameter.Add(objSample);
                                objSample.Container = 1;
                                objSample.SuboutSignOff = true;
                                objSample.SuboutSignOffDate = DateTime.Now;
                                objSample.SuboutSignOffBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                if (objSample.Testparameter != null && objSample.Samplelogin != null)
                                {
                                    SampleBottleAllocation objAllocation = os.GetObjects<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.TestMethod != null).FirstOrDefault(i => i.SampleRegistration.Oid == objSample.Samplelogin.Oid && i.TestMethod.Oid == objSample.Testparameter.TestMethod.Oid);
                                    if (objAllocation != null)
                                    {
                                        if (objAllocation.Preservative != null)
                                        {
                                            objSample.Preservative = objAllocation.Preservative;
                                        }
                                        if (objAllocation.Containers != null)
                                        {
                                            objSample.ContainerType = objAllocation.Containers;
                                        }
                                    }
                                }
                            }
                        }
                        objSuboutSample.DateRegistered = DateTime.Now;
                        objSuboutSample.RegisteredBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        DetailView dv = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_Copy", true, objSuboutSample);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        //e.ShowViewParameters.CreatedView = dv;
                        Frame.SetView(dv);
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
        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    SubOutSampleRegistrations objSuboutSample = os.CreateObject<SubOutSampleRegistrations>();
                    objSuboutSample.Status = SuboutStatus.PendingForSubout;
                    //objSuboutSample.JobID = objSample.SuboutSample.JobID;
                    //objSuboutSample.SampleID = objSample.SuboutSample.SampleID;
                    //objSuboutSample.Test = objSample.SuboutSample.Test;
                    //objSuboutSample.VisualMatrix = objSample.SuboutSample.VisualMatrix;
                    //objSuboutSample.Method = objSample.SuboutSample.Method;
                    ////objSuboutSample.Client = obj.SuboutSample.ClientSampleID;
                    //objSuboutSample.ProjectName = objSample.SuboutSample.ProjectName;
                    foreach (SampleParameter obj in e.AcceptActionArgs.SelectedObjects)
                    {
                        SampleParameter objSample = os.GetObject<SampleParameter>(obj);
                        if (objSample != null && !objSuboutSample.SampleParameter.Contains(objSample))
                        {
                            objSuboutSample.SampleParameter.Add(objSample);
                        }
                    }
                    //CriteriaOperator qcct = CriteriaOperator.Parse("Max(SUBSTRING(SuboutOrderID, 2))");
                    //string tempID = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(SubOutSampleRegistrations), qcct, null)) + 1).ToString();
                    //var curdate = DateTime.Now.ToString("yyMMdd");
                    //if (tempID != "1")
                    //{
                    //    var predate = tempID.Substring(0, 6);
                    //    if (predate == curdate)
                    //    {
                    //        tempID = "SO" + tempID;
                    //    }
                    //    else
                    //    {
                    //        tempID = "SO" + curdate + "01";
                    //    }
                    //}
                    //else
                    //{
                    //    tempID = "SO" + curdate + "01";
                    //}
                    //objSuboutSample.SuboutOrderID = tempID;
                    objSuboutSample.DateRegistered = DateTime.Now;
                    objSuboutSample.RegisteredBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                    DetailView dv = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_Copy", true, objSuboutSample);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    //e.ShowViewParameters.CreatedView = dv;
                    Frame.SetView(dv);
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }


        }

        private void simpleActionPendingForSubout_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry")
                {
                    bool resultSubmit = true;
                    int selectedcount = 0;
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                    ListPropertyEditor lstviewSuboutQCSample = ((DetailView)View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                    {
                        if (lstviewSuboutSample.ListView.SelectedObjects.Count > 0)
                        {
                            if (lstviewSuboutSample.ListView.SelectedObjects.Cast<SampleParameter>().FirstOrDefault(i => string.IsNullOrEmpty(i.Result)) != null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                resultSubmit = false;
                            }
                            if (lstviewSuboutSample.ListView.SelectedObjects.Cast<SampleParameter>().FirstOrDefault(i => string.IsNullOrEmpty(i.SuboutAnalyzedBy)) != null)
                            {
                                Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                resultSubmit = false;
                            }
                            if (lstviewSuboutSample.ListView.SelectedObjects.Cast<SampleParameter>().FirstOrDefault(i => i.AnalyzedDate == DateTime.MinValue || i.AnalyzedDate == null) != null)
                            {
                                Application.ShowViewStrategy.ShowMessage("AnalyzedDate should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                resultSubmit = false;
                            }
                        }
                        //foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                        //{
                        //    if (string.IsNullOrEmpty(objparam.Result))
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        resultSubmit = false;
                        //        break;
                        //    }
                        //    if (string.IsNullOrEmpty(objparam.SuboutAnalyzedBy))
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        resultSubmit = false;
                        //        break;
                        //    }
                        //}
                    }
                    if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView != null)
                    {
                        if (lstviewSuboutQCSample.ListView.SelectedObjects.Count > 0)
                        {
                            if (lstviewSuboutQCSample.ListView.SelectedObjects.Cast<SuboutQcSample>().FirstOrDefault(i => string.IsNullOrEmpty(i.Result)) != null)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                resultSubmit = false;
                            }
                            if (lstviewSuboutQCSample.ListView.SelectedObjects.Cast<SuboutQcSample>().FirstOrDefault(i => string.IsNullOrEmpty(i.AnalyzedBy)) != null)
                            {
                                Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                resultSubmit = false;
                            }
                            if (lstviewSuboutQCSample.ListView.SelectedObjects.Cast<SuboutQcSample>().FirstOrDefault(i => i.AnalyzedDate == DateTime.MinValue || i.AnalyzedDate == null) != null)
                            {
                                Application.ShowViewStrategy.ShowMessage("AnalyzedDate should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                                resultSubmit = false;
                            }
                        }
                        //foreach (SuboutQcSample objparam in lstviewSuboutQCSample.ListView.SelectedObjects)
                        //{
                        //    if (string.IsNullOrEmpty(objparam.Result))
                        //    {
                        //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultNull"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //        resultSubmit = false;
                        //        break;
                        //    }
                        //    if (string.IsNullOrEmpty(objparam.AnalyzedBy))
                        //{
                        //        Application.ShowViewStrategy.ShowMessage("AnalyzedBy should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //    resultSubmit = false;
                        //    break;
                        //}
                        //    //if (string.IsNullOrEmpty(objparam.AnalyzedDate))
                        //    //{
                        //    //    Application.ShowViewStrategy.ShowMessage("AnalyzedDate should not be empty.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        //    //    resultSubmit = false;
                        //    //    break;
                        //    //}
                        //}
                    }
                    if (resultSubmit == true)
                    {
                        DefaultSetting objDefaultLevel2 = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='Level2SuboutDataReview'"));
                        DefaultSetting objDefaultLevel3 = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='Level3SuboutDataReview'"));
                        if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                        {
                            if (lstviewSuboutSample.ListView.SelectedObjects.Count > 0)
                            {
                                selectedcount = lstviewSuboutSample.ListView.SelectedObjects.Count;
                                SubOutSampleRegistrations objsubout = (SubOutSampleRegistrations)View.CurrentObject;
                                if (objDefaultLevel2 != null && objDefaultLevel2.Select == true)
                                {
                                    foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                                    {
                                        foreach (SampleParameter objparam1 in lstviewSuboutSample.ListView.SelectedObjects)
                                        {
                                            objparam1.Status = Samplestatus.SuboutPendingValidation;
                                            objparam1.OSSync = true;
                                            //objparam1.AnalyzedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            //objparam1.AnalyzedDate = DateTime.Now;
                                            //objparam1.IsExportedSuboutResult = true;
                                        }
                                    }
                                }
                                else if (objDefaultLevel3 != null && objDefaultLevel3.Select == true)
                                {
                                    foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                                    {
                                        foreach (SampleParameter objparam1 in lstviewSuboutSample.ListView.SelectedObjects)
                                        {
                                            objparam1.Status = Samplestatus.SuboutPendingApproval;
                                            objparam1.OSSync = true;
                                            objparam1.SuboutValidatedDate = DateTime.Now;
                                            objparam1.SuboutValidatedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            objparam1.ValidatedDate = DateTime.Now;
                                            objparam1.ValidatedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                                    {
                                        foreach (SampleParameter objparam1 in lstviewSuboutSample.ListView.SelectedObjects)
                                        {
                                            objparam1.Status = Samplestatus.PendingReporting;
                                             objparam1.OSSync = true;
                                            //objparam1.AnalyzedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            //objparam1.AnalyzedDate = DateTime.Now;
                                            objparam1.SuboutApprovedDate = DateTime.Now;
                                            objparam1.SuboutApprovedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            objparam1.SuboutValidatedDate = DateTime.Now;
                                            objparam1.SuboutValidatedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            objparam1.ValidatedDate = DateTime.Now;
                                            objparam1.ValidatedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                            objparam1.ApprovedDate = DateTime.Now;
                                            objparam1.ApprovedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                        }
                                    }
                                }
                                lstviewSuboutSample.ListView.ObjectSpace.CommitChanges();
                                if (objsubout != null)
                                {
                                    objsubout.RollBackBy = null;
                                    objsubout.RollBackReason = null;
                                    objsubout.RollBackDate = null;
                                }
                            }
                        }
                        if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView != null)
                        {
                            if (lstviewSuboutQCSample.ListView.SelectedObjects.Count > 0)
                            {
                                selectedcount = lstviewSuboutQCSample.ListView.SelectedObjects.Count;
                                if (objDefaultLevel2 != null && objDefaultLevel2.Select == true)
                                {
                                    foreach (SuboutQcSample objparam in lstviewSuboutQCSample.ListView.SelectedObjects)
                                    {
                                        objparam.Status = Samplestatus.SuboutPendingValidation;

                                    }
                                }
                                else if (objDefaultLevel3 != null && objDefaultLevel3.Select == true)
                                {
                                    foreach (SuboutQcSample objparam in lstviewSuboutQCSample.ListView.SelectedObjects)
                                    {
                                        objparam.Status = Samplestatus.SuboutPendingApproval;
                                        objparam.SuboutValidatedDate = DateTime.Now;
                                        objparam.SuboutValidatedBy = lstviewSuboutQCSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    }
                                }
                                else
                                {
                                    foreach (SuboutQcSample objparam in lstviewSuboutQCSample.ListView.SelectedObjects)
                                    {
                                        objparam.Status = Samplestatus.PendingReporting;
                                        objparam.SuboutApprovedDate = DateTime.Now;
                                        objparam.SuboutApprovedBy = lstviewSuboutQCSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                        objparam.SuboutValidatedDate = DateTime.Now;
                                        objparam.SuboutValidatedBy = lstviewSuboutQCSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    }
                                }
                                lstviewSuboutQCSample.ListView.ObjectSpace.CommitChanges();
                                lstviewSuboutQCSample.ListView.ObjectSpace.Refresh();
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                if (View.CanClose())
                                {
                                    View.Close();
                                }

                            }
                        }
                        if (selectedcount == 0)
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            SubOutSampleRegistrations obj = (SubOutSampleRegistrations)View.CurrentObject;
                            if (obj != null && obj.SampleParameter != null)
                            {
                                int objCount = obj.SampleParameter.Where(i => i.SubOut == true && i.Status == Samplestatus.PendingEntry).Select(i => i.Oid).Count();
                                int objQCCount = obj.SubOutQcSample.Where(i => i.Status == Samplestatus.PendingEntry).Select(i => i.Oid).Count();
                                if (objCount == 0 && objQCCount == 0)
                                {
                                    obj.Status = SuboutStatus.SuboutPendingValidation;
                                    obj.SuboutStatus = SuboutTrackingStatus.SuboutResultsEntered;
                                    View.ObjectSpace.CommitChanges();
                                    if (View.CanClose())
                                    {
                                        View.Close();
                                    }

                                }
                                else
                                {
                                    View.ObjectSpace.Refresh();
                                }
                            }
                            else
                            {
                                View.ObjectSpace.Refresh();
                            }                            
                        }
                    }
                }
                //if (View.Id == "SubOutSampleRegistrations_ListView")
                //{
                //    if (View != null && e.SelectedObjects.Count > 0)
                //    {
                //        foreach (SubOutSampleRegistrations objSubouSample in View.SelectedObjects)
                //        {
                //            if (objSubouSample.Status == SuboutStatus.PendingForSubout)
                //            {
                //                objSubouSample.Status = SuboutStatus.PendingForSuboutDelivery;
                //            }
                //            else
                //            {
                //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Allreadysubmitted"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //                return;
                //            }
                //        }
                //        ObjectSpace.CommitChanges();
                //        ObjectSpace.Refresh();
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //    }
                //}
                //else if (View.Id == "SubOutSampleRegistrations_ListView_Copy_SuboutDelivery")
                //{
                //    bool dateDelivered = true;
                //    if (View != null && View.SelectedObjects.Count > 0)
                //    {
                //        foreach (SubOutSampleRegistrations objSubouSample in View.SelectedObjects)
                //        {
                //            if (objSubouSample.DateDelivered == null)
                //            {
                //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "suboutdatedelivered"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                //                dateDelivered = false;
                //                break;
                //            }
                //        }
                //        if (dateDelivered)
                //        {
                //            ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                //            foreach (SubOutSampleRegistrations objSubouSample in View.SelectedObjects)
                //            {
                //                objSubouSample.Status = SuboutStatus.SuboutDelivered;
                //            }
                //            ObjectSpace.CommitChanges();
                //            ObjectSpace.Refresh();
                //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //        }
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //    }
                //}
                //else if (View.Id == "SubOutSampleRegistrations_ListView_Copy_SuboutDelivered")
                //{
                //    if (View != null && View.SelectedObjects.Count > 0)
                //    {
                //        ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                //        foreach (SubOutSampleRegistrations objSuboutSample in View.SelectedObjects)
                //        {
                //            objSuboutSample.Status = SuboutStatus.PendingResultEntry;
                //        }
                //        ObjectSpace.CommitChanges();
                //        ObjectSpace.Refresh();
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //    }
                //    else
                //    {
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void importSuboutResult_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            try
            {
                IObjectSpace objectSpaceSheet = Application.CreateObjectSpace(typeof(ItemsFileUpload));
                ItemsFileUpload objSheet = objectSpaceSheet.CreateObject<ItemsFileUpload>();
                DetailView dv = Application.CreateDetailView(objectSpaceSheet, objSheet);
                dv.Caption = "EDD Import File";
                dv.ViewEditMode = ViewEditMode.Edit;
                e.View = dv;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void importSuboutResult_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                ResourceManager rmEnglish = new ResourceManager("Resources.LocalizeSuboutResultEntryEnglish", Assembly.Load("App_GlobalResources"));
                ResourceManager rmChinese = new ResourceManager("Resources.LocalizeSuboutResultEntryChinese", Assembly.Load("App_GlobalResources"));
                ItemsFileUpload itemsFile = (ItemsFileUpload)e.PopupWindowViewCurrentObject;
                //if (itemsFile.InputFile != null)
                //{
                //    string strFileName = itemsFile.InputFile.FileName;
                //    string strFilePath = HttpContext.Current.Server.MapPath(@"~\TestSpreadSheet\");
                //    string strLocalPath = HttpContext.Current.Server.MapPath(@"~\TestSpreadSheet\" + strFileName);
                //    if (Directory.Exists(strFilePath) == false)
                //    {
                //        Directory.CreateDirectory(strFilePath);
                //    }
                //    byte[] file = itemsFile.InputFile.Content;
                //    File.WriteAllBytes(strLocalPath, file);
                //    string fileExtension = Path.GetExtension(itemsFile.InputFile.FileName);
                //    string connectionString = string.Empty;
                //    if (fileExtension == ".xlsx")
                //    {
                //        connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0 Xml;", strLocalPath);
                //    }
                //    else if (fileExtension == ".xls")
                //    {
                //        connectionString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", strLocalPath);
                //    }
                //    if (connectionString != string.Empty)
                //    {
                //        using (var conn = new OleDbConnection(connectionString))
                //        {
                //            conn.Open();
                //            List<string> sheets = new List<string>();
                //            OleDbDataAdapter oleda = new OleDbDataAdapter();
                //            DataTable sheetNameTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //            foreach (DataRow drSheet in sheetNameTable.Rows)
                //            {
                //                if (drSheet["TABLE_NAME"].ToString().Contains("$"))
                //                {
                //                    string s = drSheet["TABLE_NAME"].ToString();
                //                    sheets.Add(s.StartsWith("'") ? s.Substring(1, s.Length - 3) : s.Substring(0, s.Length - 1));
                //                }
                //            }

                //            var cmd = conn.CreateCommand();
                //            cmd.CommandText = String.Format(
                //                @"SELECT * FROM [{0}]", sheets[0] + "$"
                //                );

                //            //variable name change

                //            oleda = new OleDbDataAdapter(cmd);
                //            using (dt = new DataTable())
                //            {
                //                oleda.Fill(dt);
                //                foreach (DataRow row in dt.Rows)
                //                {
                //                    var isEmpty = row.ItemArray.All(c => c is DBNull);
                //                    if (!isEmpty)
                //                    {
                //                        foreach (SampleParameter objSuboutSample in ((ListView)View).CollectionSource.List)
                //                        {
                //                            if (objSuboutSample != null && objSuboutSample.SuboutSample.SuboutOrderID != null && objSuboutSample.Samplelogin.JobID.JobID != null
                //                                && objSuboutSample.Samplelogin.SampleID != null && objSuboutSample.Testparameter.TestMethod.TestName != null
                //                                && objSuboutSample.Testparameter.TestMethod.TestName != null && objSuboutSample.Testparameter.TestMethod.MethodName.MethodName != null
                //                                && objSuboutSample.Testparameter.Parameter.ParameterName != null)
                //                            {
                //                                string strPattern = @"(?:\r|\n|\a|\t|)";
                //                                if (!string.IsNullOrEmpty(row[0].ToString()))
                //                                {
                //                                    row[0] = System.Text.RegularExpressions.Regex.Replace(row[0].ToString(), strPattern, string.Empty);
                //                                }
                //                                if (!string.IsNullOrEmpty(row[2].ToString()))
                //                                {
                //                                    row[2] = System.Text.RegularExpressions.Regex.Replace(row[2].ToString(), strPattern, string.Empty);
                //                                }
                //                                if (!string.IsNullOrEmpty(row[3].ToString()))
                //                                {
                //                                    row[3] = System.Text.RegularExpressions.Regex.Replace(row[3].ToString(), strPattern, string.Empty);
                //                                }
                //                                if (!string.IsNullOrEmpty(row[4].ToString()))
                //                                {
                //                                    row[4] = System.Text.RegularExpressions.Regex.Replace(row[4].ToString(), strPattern, string.Empty);
                //                                }
                //                                if (!string.IsNullOrEmpty(row[6].ToString()))
                //                                {
                //                                    row[6] = System.Text.RegularExpressions.Regex.Replace(row[6].ToString(), strPattern, string.Empty);
                //                                }
                //                                if (!string.IsNullOrEmpty(row[7].ToString()))
                //                                {
                //                                    row[7] = System.Text.RegularExpressions.Regex.Replace(row[7].ToString(), strPattern, string.Empty);
                //                                }
                //                                if (!string.IsNullOrEmpty(row[8].ToString()))
                //                                {
                //                                    row[8] = System.Text.RegularExpressions.Regex.Replace(row[8].ToString(), strPattern, string.Empty);
                //                                }
                //                                string str = row[6].ToString().Trim();
                //                                //if (objSuboutSample.SuboutSample.SuboutOrderID == row[0].ToString().Trim()
                //                                //    && objSuboutSample.Samplelogin.JobID.JobID == row[2].ToString().Trim()
                //                                //    && objSuboutSample.Samplelogin.SampleID == row[3].ToString().Trim()
                //                                //    && objSuboutSample.Testparameter.TestMethod.TestName == row[6].ToString().Trim()
                //                                //    && objSuboutSample.Testparameter.TestMethod.MethodName.MethodName == row[7].ToString().Trim()
                //                                //    && objSuboutSample.Testparameter.Parameter.ParameterName == row[8].ToString().Trim())
                //                                if ((!string.IsNullOrEmpty(row[4].ToString())
                //                                    && objSuboutSample.Samplelogin.ClientSampleID == row[4].ToString().Trim()
                //                                    && objSuboutSample.Testparameter.TestMethod.TestName == row[6].ToString().Trim()
                //                                    && objSuboutSample.Testparameter.TestMethod.MethodName.MethodName == row[7].ToString().Trim()
                //                                    && objSuboutSample.Testparameter.Parameter.ParameterName == row[8].ToString().Trim())
                //                                    || (objSuboutSample.Samplelogin.SampleID == row[3].ToString().Trim()
                //                                    && objSuboutSample.Testparameter.TestMethod.TestName == row[6].ToString().Trim()
                //                                    && objSuboutSample.Testparameter.TestMethod.MethodName.MethodName == row[7].ToString().Trim()
                //                                    && objSuboutSample.Testparameter.Parameter.ParameterName == row[8].ToString().Trim()))
                //                                {
                //                                    if (dt.Columns.Contains(rmChinese.GetString("QcType")) && !row.IsNull(rmChinese.GetString("QcType")))
                //                                    {
                //                                        strQctype = row[rmChinese.GetString("QcType")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("QcType")) && !row.IsNull(rmEnglish.GetString("QcType")))
                //                                    {
                //                                        strQctype = row[rmEnglish.GetString("QcType")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strQctype = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("NumericResult")) && !row.IsNull(rmChinese.GetString("NumericResult")))
                //                                    {
                //                                        strNumericResult = row[rmChinese.GetString("NumericResult")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("NumericResult")) && !row.IsNull(rmEnglish.GetString("NumericResult")))
                //                                    {
                //                                        strNumericResult = row[rmEnglish.GetString("NumericResult")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strNumericResult = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("Results")) && !row.IsNull(rmChinese.GetString("Results")))
                //                                    {
                //                                        strResult = row[rmChinese.GetString("Results")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("Results")) && !row.IsNull(rmEnglish.GetString("Results")))
                //                                    {
                //                                        strResult = row[rmEnglish.GetString("Results")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strResult = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("Units")) && !row.IsNull(rmChinese.GetString("Units")))
                //                                    {
                //                                        strUnits = row[rmChinese.GetString("Units")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("Units")) && !row.IsNull(rmEnglish.GetString("Units")))
                //                                    {
                //                                        strUnits = row[rmEnglish.GetString("Units")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strUnits = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("DF")) && !row.IsNull(rmChinese.GetString("DF")))
                //                                    {
                //                                        strDF = row[rmChinese.GetString("DF")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("DF")) && !row.IsNull(rmEnglish.GetString("DF")))
                //                                    {
                //                                        strDF = row[rmEnglish.GetString("DF")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strDF = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("LOQ")) && !row.IsNull(rmChinese.GetString("LOQ")))
                //                                    {
                //                                        strLOQ = row[rmChinese.GetString("LOQ")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("LOQ")) && !row.IsNull(rmEnglish.GetString("LOQ")))
                //                                    {
                //                                        strLOQ = row[rmEnglish.GetString("LOQ")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strLOQ = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("UQL")) && !row.IsNull(rmChinese.GetString("UQL")))
                //                                    {
                //                                        strUQl = row[rmChinese.GetString("UQL")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("UQL")) && !row.IsNull(rmEnglish.GetString("UQL")))
                //                                    {
                //                                        strUQl = row[rmEnglish.GetString("UQL")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strUQl = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                //                                    {
                //                                        strRptLimit = row[rmChinese.GetString("RptLimit")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                //                                    {
                //                                        strRptLimit = row[rmEnglish.GetString("RptLimit")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strRptLimit = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                //                                    {
                //                                        strMDL = row[rmChinese.GetString("MDL")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                //                                    {
                //                                        strMDL = row[rmEnglish.GetString("MDL")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strMDL = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("SpikeAmount")) && !row.IsNull(rmChinese.GetString("SpikeAmount")))
                //                                    {
                //                                        strSpikeAmount = Convert.ToDouble(row[rmChinese.GetString("SpikeAmount")]);
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("SpikeAmount")) && !row.IsNull(rmEnglish.GetString("SpikeAmount")))
                //                                    {
                //                                        strSpikeAmount = Convert.ToDouble(row[rmEnglish.GetString("SpikeAmount")].ToString());
                //                                    }
                //                                    else
                //                                    {
                //                                        strSpikeAmount = 0;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("%Recovery")) && !row.IsNull(rmChinese.GetString("%Recovery")))
                //                                    {
                //                                        strRecovery = row[rmChinese.GetString("%Recovery")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("%Recovery")) && !row.IsNull(rmEnglish.GetString("%Recovery")))
                //                                    {
                //                                        strRecovery = row[rmEnglish.GetString("%Recovery")].ToString();
                //                                    }
                //                                    else
                //                                    {
                //                                        strRecovery = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("%RPD")) && !row.IsNull(rmChinese.GetString("%RPD")))
                //                                    {
                //                                        strRPD = row[rmChinese.GetString("%RPD")].ToString();
                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("%RPD")) && !row.IsNull(rmEnglish.GetString("%RPD")))
                //                                    {
                //                                        strRPD = row[rmEnglish.GetString("%RPD")].ToString();

                //                                    }
                //                                    else
                //                                    {
                //                                        strRPD = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("RecCLLimit")) && !row.IsNull(rmChinese.GetString("RecCLLimit")))
                //                                    {
                //                                        strRecCLLimite = row[rmChinese.GetString("RecCLLimit")].ToString();

                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("RecCLLimit")) && !row.IsNull(rmEnglish.GetString("RecCLLimit")))
                //                                    {
                //                                        strRecCLLimite = row[rmEnglish.GetString("RecCLLimit")].ToString();

                //                                    }
                //                                    else
                //                                    {
                //                                        strRecCLLimite = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("RecCHLimit")) && !row.IsNull(rmChinese.GetString("RecCHLimit")))
                //                                    {
                //                                        strRecCHLImite = row[rmChinese.GetString("RecCHLimit")].ToString();

                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("RecCHLimit")) && !row.IsNull(rmEnglish.GetString("RecCHLimit")))
                //                                    {
                //                                        strRecCHLImite = row[rmEnglish.GetString("RecCHLimit")].ToString();

                //                                    }
                //                                    else
                //                                    {
                //                                        strRecCHLImite = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("RPDCLLimit")) && !row.IsNull(rmChinese.GetString("RPDCLLimit")))
                //                                    {
                //                                        strRPDCLLimite = row[rmChinese.GetString("RPDCLLimit")].ToString();

                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("RPDCLLimit")) && !row.IsNull(rmEnglish.GetString("RPDCLLimit")))
                //                                    {
                //                                        strRPDCLLimite = row[rmEnglish.GetString("RPDCLLimit")].ToString();

                //                                    }
                //                                    else
                //                                    {
                //                                        strRPDCLLimite = string.Empty;
                //                                    }
                //                                    if (dt.Columns.Contains(rmChinese.GetString("RPDCHLimit")) && !row.IsNull(rmChinese.GetString("RPDCHLimit")))
                //                                    {
                //                                        strRPDCHLimite = row[rmChinese.GetString("RPDCHLimit")].ToString();

                //                                    }
                //                                    else if (dt.Columns.Contains(rmEnglish.GetString("RPDCHLimit")) && !row.IsNull(rmEnglish.GetString("RPDCHLimit")))
                //                                    {
                //                                        strRPDCHLimite = row[rmEnglish.GetString("RPDCHLimit")].ToString();

                //                                    }
                //                                    else
                //                                    {
                //                                        strRPDCHLimite = string.Empty;
                //                                    }
                //                                    objSuboutSample.ResultNumeric = strNumericResult.Trim();
                //                                    objSuboutSample.Result = strResult.Trim();
                //                                    if (strUnits != null)
                //                                    {
                //                                        Unit obj = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strUnits + "'"));
                //                                        if (obj != null)
                //                                        {
                //                                            objSuboutSample.Units = obj;
                //                                        }
                //                                    }
                //                                    objSuboutSample.DF = strDF.Trim();
                //                                    objSuboutSample.LOQ = strLOQ.Trim();
                //                                    objSuboutSample.UQL = strUQl.Trim();
                //                                    objSuboutSample.RptLimit = strRptLimit.Trim();
                //                                    objSuboutSample.MDL = strMDL.Trim();
                //                                    objSuboutSample.SpikeAmount = strSpikeAmount;
                //                                    objSuboutSample.Rec = strRecovery.Trim();
                //                                    objSuboutSample.RPD = strRPD.Trim();
                //                                    objSuboutSample.RecHCLimit = strRecCHLImite.Trim();
                //                                    objSuboutSample.RecLCLimit = strRecCLLimite.Trim();
                //                                    objSuboutSample.RPDHCLimit = strRPDCHLimite.Trim();
                //                                    objSuboutSample.RPDLCLimit = strRPDCLLimite.Trim();
                //                                    //objSuboutSample.AnalyzedDate = DateTime.Now;
                //                                    //objSuboutSample.AnalyzedBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                //                                    ObjectSpace.CommitChanges();
                //                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "importdata"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //                                }
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //            conn.Close();
                //        }
                //    }
                //}
                if (itemsFile.InputFile != null && itemsFile.InputFile.Content != null && !string.IsNullOrEmpty(itemsFile.InputFile.FileName) && itemsFile.InputFile.Size > 0)
                {
                    byte[] file = itemsFile.InputFile.Content;
                    string fileExtension = Path.GetExtension(itemsFile.InputFile.FileName);
                    DevExpress.Spreadsheet.Workbook workbook = new DevExpress.Spreadsheet.Workbook();
                    if (fileExtension == ".xlsx")
                    {
                        workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    }
                    else if (fileExtension == ".xls")
                    {
                        workbook.LoadDocument(file, DevExpress.Spreadsheet.DocumentFormat.Xls);
                    }
                    DevExpress.Spreadsheet.WorksheetCollection worksheets = workbook.Worksheets;
                    DevExpress.Spreadsheet.Worksheet worksheet = workbook.Worksheets[0];
                    CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                    DataTable dt = worksheet.CreateDataTable(range, true);
                    for (int col = 0; col < range.ColumnCount; col++)
                    {
                        CellValueType cellType = range[0, col].Value.Type;
                        for (int r = 1; r < range.RowCount; r++)
                        {
                            if (cellType != range[r, col].Value.Type)
                            {
                                dt.Columns[col].DataType = typeof(string);
                                break;
                            }
                        }
                    }
                    DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                    exporter.Export();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow row1 = dt.Rows[0];
                        if (row1[0].ToString() == dt.Columns[0].Caption)
                        {
                            row1.Delete();
                            dt.AcceptChanges();
                        }
                        foreach (DataColumn c in dt.Columns)
                            c.ColumnName = c.ColumnName.ToString().Trim();
                    }
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                    ListPropertyEditor lstviewSuboutQCSample = ((DetailView)View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView == null)
                    {
                        lstviewSuboutQCSample.CreateControl();
                    }
                    foreach (DataRow row in dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(c => c is DBNull)))
                    {
                        var isEmpty = row.ItemArray.All(c => c is DBNull);
                        if (!isEmpty)
                        {
                            #region SampleParameter
                            //ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                            //if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                            //{
                            //    foreach (SampleParameter objSuboutSample in ((ListView)lstviewSuboutSample.ListView).CollectionSource.List.Cast<SampleParameter>().ToList())
                            //    {
                            //        if (objSuboutSample != null && objSuboutSample.SuboutSample.SuboutOrderID != null && objSuboutSample.Samplelogin.JobID.JobID != null
                            //            && objSuboutSample.Samplelogin.SampleID != null && objSuboutSample.Testparameter.TestMethod.TestName != null
                            //            && objSuboutSample.Testparameter.TestMethod.TestName != null && objSuboutSample.Testparameter.TestMethod.MethodName.MethodNumber != null
                            //            && objSuboutSample.Testparameter.Parameter.ParameterName != null)
                            //        {
                            //            string strPattern = @"(?:\r|\n|\a|\t|)";
                            //            if (!string.IsNullOrEmpty(row[0].ToString()))
                            //            {
                            //                row[0] = System.Text.RegularExpressions.Regex.Replace(row[0].ToString(), strPattern, string.Empty);
                            //            }
                            //            if (!string.IsNullOrEmpty(row[2].ToString()))
                            //            {
                            //                row[2] = System.Text.RegularExpressions.Regex.Replace(row[2].ToString(), strPattern, string.Empty);
                            //            }
                            //            if (!string.IsNullOrEmpty(row[3].ToString()))
                            //            {
                            //                row[3] = System.Text.RegularExpressions.Regex.Replace(row[3].ToString(), strPattern, string.Empty);
                            //            }
                            //            if (!string.IsNullOrEmpty(row[4].ToString()))
                            //            {
                            //                row[4] = System.Text.RegularExpressions.Regex.Replace(row[4].ToString(), strPattern, string.Empty);
                            //            }
                            //            if (!string.IsNullOrEmpty(row[6].ToString()))
                            //            {
                            //                row[6] = System.Text.RegularExpressions.Regex.Replace(row[6].ToString(), strPattern, string.Empty);
                            //            }
                            //            if (!string.IsNullOrEmpty(row[7].ToString()))
                            //            {
                            //                row[7] = System.Text.RegularExpressions.Regex.Replace(row[7].ToString(), strPattern, string.Empty);
                            //            }
                            //            if (!string.IsNullOrEmpty(row[8].ToString()))
                            //            {
                            //                row[8] = System.Text.RegularExpressions.Regex.Replace(row[8].ToString(), strPattern, string.Empty);
                            //            }
                            //            if (!string.IsNullOrEmpty(row[1].ToString()))
                            //            {
                            //                row[1] = System.Text.RegularExpressions.Regex.Replace(row[1].ToString(), strPattern, string.Empty);
                            //            }
                            //            if (!string.IsNullOrEmpty(row[23].ToString()))
                            //            {
                            //                row[23] = System.Text.RegularExpressions.Regex.Replace(row[23].ToString(), strPattern, string.Empty);
                            //            }
                            //            string str = row[6].ToString().Trim();
                            //            string str1 = row[7].ToString().Trim();
                            //            if ((!string.IsNullOrEmpty(row[0].ToString())

                            //                && string.IsNullOrEmpty(row[1].ToString())
                            //            && !string.IsNullOrEmpty(row[2].ToString())
                            //            ////&& objSuboutSample.Samplelogin.ClientSampleID == row[4].ToString().Trim()
                            //            && objSuboutSample.Testparameter.TestMethod.TestName.Trim() == row[6].ToString().Trim()
                            //            && objSuboutSample.Testparameter.TestMethod.MethodName.MethodNumber.Trim() == row[5].ToString().Trim()
                            //           && objSuboutSample.Testparameter.Parameter.ParameterName.Trim() == row[7].ToString().Trim())
                            //            || string.IsNullOrEmpty(row[0].ToString())
                            //            && (objSuboutSample.Samplelogin.SampleID == row[2].ToString().Trim()
                            //            && objSuboutSample.Testparameter.TestMethod.TestName.Trim() == row[6].ToString().Trim()
                            //            && objSuboutSample.Testparameter.TestMethod.MethodName.MethodNumber.Trim() == row[5].ToString().Trim()
                            //            && objSuboutSample.Testparameter.Parameter.ParameterName.Trim() == row[7].ToString().Trim()))

                            //            {

                            //                if (dt.Columns.Contains(rmChinese.GetString("NumericResult")) && !row.IsNull(rmChinese.GetString("NumericResult")))
                            //                {
                            //                    strNumericResult = row[rmChinese.GetString("NumericResult")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("NumericResult")) && !row.IsNull(rmEnglish.GetString("NumericResult")))
                            //                {
                            //                    strNumericResult = row[rmEnglish.GetString("NumericResult")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strNumericResult = string.Empty;
                            //                }
                            //                if (dt.Columns.Contains(rmChinese.GetString("Results")) && !row.IsNull(rmChinese.GetString("Results")))
                            //                {
                            //                    strResult = row[rmChinese.GetString("Results")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("Results")) && !row.IsNull(rmEnglish.GetString("Results")))
                            //                {
                            //                    strResult = row[rmEnglish.GetString("Results")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strResult = string.Empty;
                            //                }
                            //                if (dt.Columns.Contains(rmChinese.GetString("Units")) && !row.IsNull(rmChinese.GetString("Units")))
                            //                {
                            //                    strUnits = row[rmChinese.GetString("Units")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("Units")) && !row.IsNull(rmEnglish.GetString("Units")))
                            //                {
                            //                    strUnits = row[rmEnglish.GetString("Units")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strUnits = string.Empty;
                            //                }
                            //                if (dt.Columns.Contains(rmChinese.GetString("DF")) && !row.IsNull(rmChinese.GetString("DF")))
                            //                {
                            //                    strDF = row[rmChinese.GetString("DF")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("DF")) && !row.IsNull(rmEnglish.GetString("DF")))
                            //                {
                            //                    strDF = row[rmEnglish.GetString("DF")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strDF = string.Empty;
                            //                }
                            //                if (dt.Columns.Contains(rmChinese.GetString("LOQ")) && !row.IsNull(rmChinese.GetString("LOQ")))
                            //                {
                            //                    strLOQ = row[rmChinese.GetString("LOQ")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("LOQ")) && !row.IsNull(rmEnglish.GetString("LOQ")))
                            //                {
                            //                    strLOQ = row[rmEnglish.GetString("LOQ")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strLOQ = string.Empty;
                            //                }
                            //                if (dt.Columns.Contains(rmChinese.GetString("UQL")) && !row.IsNull(rmChinese.GetString("UQL")))
                            //                {
                            //                    strUQl = row[rmChinese.GetString("UQL")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("UQL")) && !row.IsNull(rmEnglish.GetString("UQL")))
                            //                {
                            //                    strUQl = row[rmEnglish.GetString("UQL")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strUQl = string.Empty;
                            //                }
                            //                if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                            //                {
                            //                    strRptLimit = row[rmChinese.GetString("RptLimit")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                            //                {
                            //                    strRptLimit = row[rmEnglish.GetString("RptLimit")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strRptLimit = string.Empty;
                            //                }
                            //                if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                            //                {
                            //                    strMDL = row[rmChinese.GetString("MDL")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                            //                {
                            //                    strMDL = row[rmEnglish.GetString("MDL")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strMDL = string.Empty;
                            //                }
                            //                if (dt.Columns.Contains(rmChinese.GetString("AnalyzedBy")) && !row.IsNull(rmChinese.GetString("AnalyzedBy")))
                            //                {
                            //                    strAnalyzedBy = row[rmChinese.GetString("AnalyzedBy")].ToString();
                            //                }
                            //                else if (dt.Columns.Contains(rmEnglish.GetString("AnalyzedBy")) && !row.IsNull(rmEnglish.GetString("AnalyzedBy")))
                            //                {
                            //                    strAnalyzedBy = row[rmEnglish.GetString("AnalyzedBy")].ToString();
                            //                }
                            //                else
                            //                {
                            //                    strAnalyzedBy = string.Empty;
                            //                }
                            //                SuboutQcSample objSuboutQCSample = ObjectSpace.CreateObject<SuboutQcSample>();
                            //                //objSuboutQCSample.SuboutID = 
                            //                objSuboutQCSample.NumericResult = strNumericResult.Trim();
                            //                objSuboutQCSample.Result = strResult.Trim();
                            //                objSuboutQCSample.Units = strUnits.Trim();
                            //                //if (strUnits != null)
                            //                //{
                            //                //    Unit obj = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strUnits + "'"));
                            //                //    if (obj != null)
                            //                //    {
                            //                //        objSuboutSample.Units = obj;
                            //                //    }
                            //                //}
                            //                objSuboutQCSample.DF = strDF.Trim();
                            //                objSuboutQCSample.LOQ = strLOQ.Trim();
                            //                objSuboutQCSample.UQL = strUQl.Trim();
                            //                objSuboutQCSample.RptLimit = strRptLimit.Trim();
                            //                objSuboutQCSample.MDL = strMDL.Trim();
                            //                objSuboutQCSample.Rec = strRecovery.Trim();
                            //                objSuboutQCSample.RPD = strRPD.Trim();
                            //                //objSuboutSample.AnalyzedBy = strAnalyzedBy.Trim();
                            //                ObjectSpace.CommitChanges();
                            //                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "importdata"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                            //            }
                            //        }                                    
                            //    }

                            //} 
                            #endregion                            
                            #region QcSamples
                            //if (dt.Columns.Contains(rmChinese.GetString("JobID")) && !row.IsNull(rmChinese.GetString("JobID")))
                            //{
                            //    strJobID = row[rmChinese.GetString("JobID")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("JobID")) && !row.IsNull(rmEnglish.GetString("JobID")))
                            //{
                            //    strJobID = row[rmEnglish.GetString("JobID")].ToString();
                            //}
                            //else
                            //{
                            //    strJobID = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("QcType")) && !row.IsNull(rmChinese.GetString("QcType")))
                            //{
                            //    strQCType = row[rmChinese.GetString("QcType")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("QcType")) && !row.IsNull(rmEnglish.GetString("QcType")))
                            //{
                            //    strQCType = row[rmEnglish.GetString("QcType")].ToString();
                            //}
                            //else
                            //{
                            //    strQCType = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("SampleID")) && !row.IsNull(rmChinese.GetString("SampleID")))
                            //{
                            //    strSampleID = row[rmChinese.GetString("SampleID")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("SampleID")) && !row.IsNull(rmEnglish.GetString("SampleID")))
                            //{
                            //    strSampleID = row[rmEnglish.GetString("SampleID")].ToString();
                            //}
                            //else
                            //{
                            //    strSampleID = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("SampleName")) && !row.IsNull(rmChinese.GetString("SampleName")))
                            //{
                            //    strSampleName = row[rmChinese.GetString("SampleName")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("SampleName")) && !row.IsNull(rmEnglish.GetString("SampleName")))
                            //{
                            //    strSampleName = row[rmEnglish.GetString("SampleName")].ToString();
                            //}
                            //else
                            //{
                            //    strSampleName = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("Matrix")) && !row.IsNull(rmChinese.GetString("Matrix")))
                            //{
                            //    strMatrix = row[rmChinese.GetString("Matrix")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("Matrix")) && !row.IsNull(rmEnglish.GetString("Matrix")))
                            //{
                            //    strMatrix = row[rmEnglish.GetString("Matrix")].ToString();
                            //}
                            //else
                            //{
                            //    strMatrix = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("Test")) && !row.IsNull(rmChinese.GetString("Test")))
                            //{
                            //    strTest = row[rmChinese.GetString("Test")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("Test")) && !row.IsNull(rmEnglish.GetString("Test")))
                            //{
                            //    strTest = row[rmEnglish.GetString("Test")].ToString();
                            //}
                            //else
                            //{
                            //    strTest = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("Method")) && !row.IsNull(rmChinese.GetString("Method")))
                            //{
                            //    strMethod = row[rmChinese.GetString("Method")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("Method")) && !row.IsNull(rmEnglish.GetString("Method")))
                            //{
                            //    strMethod = row[rmEnglish.GetString("Method")].ToString();
                            //}
                            //else
                            //{
                            //    strMethod = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("Parameter")) && !row.IsNull(rmChinese.GetString("Parameter")))
                            //{
                            //    strParameter = row[rmChinese.GetString("Parameter")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("Parameter")) && !row.IsNull(rmEnglish.GetString("Parameter")))
                            //{
                            //    strParameter = row[rmEnglish.GetString("Parameter")].ToString();
                            //}
                            //else
                            //{
                            //    strParameter = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("NumericResult")) && !row.IsNull(rmChinese.GetString("NumericResult")))
                            //{
                            //    strNumericResult = row[rmChinese.GetString("NumericResult")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("NumericResult")) && !row.IsNull(rmEnglish.GetString("NumericResult")))
                            //{
                            //    strNumericResult = row[rmEnglish.GetString("NumericResult")].ToString();
                            //}
                            //else
                            //{
                            //    strNumericResult = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("Results")) && !row.IsNull(rmChinese.GetString("Results")))
                            //{
                            //    strResult = row[rmChinese.GetString("Results")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("Results")) && !row.IsNull(rmEnglish.GetString("Results")))
                            //{
                            //    strResult = row[rmEnglish.GetString("Results")].ToString();
                            //}
                            //else
                            //{
                            //    strResult = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("Units")) && !row.IsNull(rmChinese.GetString("Units")))
                            //{
                            //    strUnits = row[rmChinese.GetString("Units")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("Units")) && !row.IsNull(rmEnglish.GetString("Units")))
                            //{
                            //    strUnits = row[rmEnglish.GetString("Units")].ToString();
                            //}
                            //else
                            //{
                            //    strUnits = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("DF")) && !row.IsNull(rmChinese.GetString("DF")))
                            //{
                            //    strDF = row[rmChinese.GetString("DF")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("DF")) && !row.IsNull(rmEnglish.GetString("DF")))
                            //{
                            //    strDF = row[rmEnglish.GetString("DF")].ToString();
                            //}
                            //else
                            //{
                            //    strDF = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("LOQ")) && !row.IsNull(rmChinese.GetString("LOQ")))
                            //{
                            //    strLOQ = row[rmChinese.GetString("LOQ")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("LOQ")) && !row.IsNull(rmEnglish.GetString("LOQ")))
                            //{
                            //    strLOQ = row[rmEnglish.GetString("LOQ")].ToString();
                            //}
                            //else
                            //{
                            //    strLOQ = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("UQL")) && !row.IsNull(rmChinese.GetString("UQL")))
                            //{
                            //    strUQl = row[rmChinese.GetString("UQL")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("UQL")) && !row.IsNull(rmEnglish.GetString("UQL")))
                            //{
                            //    strUQl = row[rmEnglish.GetString("UQL")].ToString();
                            //}
                            //else
                            //{
                            //    strUQl = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                            //{
                            //    strRptLimit = row[rmChinese.GetString("RptLimit")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                            //{
                            //    strRptLimit = row[rmEnglish.GetString("RptLimit")].ToString();
                            //}
                            //else
                            //{
                            //    strRptLimit = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                            //{
                            //    strMDL = row[rmChinese.GetString("MDL")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                            //{
                            //    strMDL = row[rmEnglish.GetString("MDL")].ToString();
                            //}
                            //else
                            //{
                            //    strMDL = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("%Recovery")) && !row.IsNull(rmChinese.GetString("%Recovery")))
                            //{
                            //    strRecovery = row[rmChinese.GetString("%Recovery")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("%Recovery")) && !row.IsNull(rmEnglish.GetString("%Recovery")))
                            //{
                            //    strRecovery = row[rmEnglish.GetString("%Recovery")].ToString();
                            //}
                            //else
                            //{
                            //    strRecovery = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("%RPD")) && !row.IsNull(rmChinese.GetString("%RPD")))
                            //{
                            //    strRPD = row[rmChinese.GetString("%RPD")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("%RPD")) && !row.IsNull(rmEnglish.GetString("%RPD")))
                            //{
                            //    strRPD = row[rmEnglish.GetString("%RPD")].ToString();
                            //}
                            //else
                            //{
                            //    strRPD = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("RecLCLimit")) && !row.IsNull(rmChinese.GetString("RecLCLimit")))
                            //{
                            //    strRecLCLimit = row[rmChinese.GetString("RecLCLimit")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("RecLCLimit")) && !row.IsNull(rmEnglish.GetString("RecLCLimit")))
                            //{
                            //    strRecLCLimit = row[rmEnglish.GetString("RecLCLimit")].ToString();
                            //}
                            //else
                            //{
                            //    strRecLCLimit = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("RecUCLimit")) && !row.IsNull(rmChinese.GetString("RecUCLimit")))
                            //{
                            //    strRecUCLimit = row[rmChinese.GetString("RecUCLimit")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("RecUCLimit")) && !row.IsNull(rmEnglish.GetString("RecUCLimit")))
                            //{
                            //    strRecUCLimit = row[rmEnglish.GetString("RecUCLimit")].ToString();
                            //}
                            //else
                            //{
                            //    strRecUCLimit = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("RPDLCLimit")) && !row.IsNull(rmChinese.GetString("RPDLCLimit")))
                            //{
                            //    strRPDLCLimit = row[rmChinese.GetString("RPDLCLimit")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("RPDLCLimit")) && !row.IsNull(rmEnglish.GetString("RPDLCLimit")))
                            //{
                            //    strRPDLCLimit = row[rmEnglish.GetString("RPDLCLimit")].ToString();
                            //}
                            //else
                            //{
                            //    strRPDLCLimit = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("RPDUCLimit")) && !row.IsNull(rmChinese.GetString("RPDUCLimit")))
                            //{
                            //    strRPDUCLimit = row[rmChinese.GetString("RPDUCLimit")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("RPDUCLimit")) && !row.IsNull(rmEnglish.GetString("RPDUCLimit")))
                            //{
                            //    strRPDUCLimit = row[rmEnglish.GetString("RPDUCLimit")].ToString();
                            //}
                            //else
                            //{
                            //    strRPDUCLimit = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("AnalyzedBy")) && !row.IsNull(rmChinese.GetString("AnalyzedBy")))
                            //{
                            //    strAnalyzedBy = row[rmChinese.GetString("AnalyzedBy")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("AnalyzedBy")) && !row.IsNull(rmEnglish.GetString("AnalyzedBy")))
                            //{
                            //    strAnalyzedBy = row[rmEnglish.GetString("AnalyzedBy")].ToString();
                            //}
                            //else
                            //{
                            //    strAnalyzedBy = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("ApprovedBy")) && !row.IsNull(rmChinese.GetString("ApprovedBy")))
                            //{
                            //    strApprovedBy = row[rmChinese.GetString("ApprovedBy")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("ApprovedBy")) && !row.IsNull(rmEnglish.GetString("ApprovedBy")))
                            //{
                            //    strApprovedBy = row[rmEnglish.GetString("ApprovedBy")].ToString();
                            //}
                            //else
                            //{
                            //    strApprovedBy = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("ValidatedBy")) && !row.IsNull(rmChinese.GetString("ValidatedBy")))
                            //{
                            //    strValidatedBy = row[rmChinese.GetString("ValidatedBy")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("ValidatedBy")) && !row.IsNull(rmEnglish.GetString("ValidatedBy")))
                            //{
                            //    strValidatedBy = row[rmEnglish.GetString("ValidatedBy")].ToString();
                            //}
                            //else
                            //{
                            //    strValidatedBy = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("AnalyzedDate")) && !row.IsNull(rmChinese.GetString("AnalyzedDate")))
                            //{
                            //    strAnalyzedDate = row[rmChinese.GetString("AnalyzedDate")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("AnalyzedDate")) && !row.IsNull(rmEnglish.GetString("AnalyzedDate")))
                            //{
                            //    strAnalyzedDate = row[rmEnglish.GetString("AnalyzedDate")].ToString();
                            //}
                            //else
                            //{
                            //    strAnalyzedDate = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("ApprovedDate")) && !row.IsNull(rmChinese.GetString("ApprovedDate")))
                            //{
                            //    strApprovedDate = row[rmChinese.GetString("ApprovedDate")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("ApprovedDate")) && !row.IsNull(rmEnglish.GetString("ApprovedDate")))
                            //{
                            //    strApprovedDate = row[rmEnglish.GetString("ApprovedDate")].ToString();
                            //}
                            //else
                            //{
                            //    strApprovedDate = string.Empty;
                            //}
                            //if (dt.Columns.Contains(rmChinese.GetString("ValidatedDate")) && !row.IsNull(rmChinese.GetString("ValidatedDate")))
                            //{
                            //    strValidatedDate = row[rmChinese.GetString("ValidatedDate")].ToString();
                            //}
                            //else if (dt.Columns.Contains(rmEnglish.GetString("ValidatedDate")) && !row.IsNull(rmEnglish.GetString("ValidatedDate")))
                            //{
                            //    strValidatedDate = row[rmEnglish.GetString("ValidatedDate")].ToString();
                            //}
                            //else
                            //{
                            //    strValidatedDate = string.Empty;
                            //}
                            //IObjectSpace os = Application.CreateObjectSpace();
                            //SuboutQcSample objSuboutQCSample = os.CreateObject<SuboutQcSample>();
                            //SubOutSampleRegistrations objSSR = os.GetObject((SubOutSampleRegistrations)View.CurrentObject);
                            //if (objSSR != null)
                            //{
                            //    objSuboutQCSample.SuboutID = objSSR;
                            //}
                            //if (strJobID != null)
                            //{
                            //    Samplecheckin objSc = os.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", strJobID));
                            //    if (objSc != null)
                            //    {
                            //        objSuboutQCSample.JobID = objSc;
                            //    }
                            //}
                            //objSuboutQCSample.QCType = strQCType.Trim();
                            //objSuboutQCSample.SampleID = strSampleID.Trim();
                            //objSuboutQCSample.SampleName = strSampleName.Trim();
                            //if (strMatrix != null)
                            //{
                            //    Matrix objMx = os.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", strMatrix));
                            //    if (objMx != null)
                            //    {
                            //        objSuboutQCSample.Matrix = objMx;
                            //    }
                            //}
                            //if (strTest != null)
                            //{
                            //    TestMethod objTm = os.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=?", strTest));
                            //    if (objTm != null)
                            //    {
                            //        objSuboutQCSample.Test = objTm;
                            //    }
                            //    //else
                            //    //{
                            //    //    objTm = ObjectSpace.CreateObject<TestMethod>();
                            //    //    //objTm.TestName=
                            //    //}
                            //}
                            //if (strMethod != null)
                            //{
                            //    Method objM = os.FindObject<Method>(CriteriaOperator.Parse("[MethodNumber]=?", strMethod));
                            //    if (objM != null)
                            //    {
                            //        objSuboutQCSample.Method = objM;
                            //    }
                            //}
                            //objSuboutQCSample.Parameter = strParameter.Trim();
                            //objSuboutQCSample.NumericResult = strNumericResult.Trim();
                            //objSuboutQCSample.Result = strResult.Trim();
                            //objSuboutQCSample.Units = strUnits.Trim();
                            //objSuboutQCSample.DF = strDF.Trim();
                            //objSuboutQCSample.LOQ = strLOQ.Trim();
                            //objSuboutQCSample.UQL = strUQl.Trim();
                            //objSuboutQCSample.RptLimit = strRptLimit.Trim();
                            //objSuboutQCSample.MDL = strMDL.Trim();
                            //objSuboutQCSample.Rec = strRecovery.Trim();
                            //objSuboutQCSample.RPD = strRPD.Trim();
                            //objSuboutQCSample.RecLCLimit = strRecLCLimit.Trim();
                            //objSuboutQCSample.RPDLCLimit = strRPDLCLimit.Trim();
                            //objSuboutQCSample.RecUCLimit = strRecUCLimit.Trim();
                            //objSuboutQCSample.RPDUCLimit = strRPDUCLimit.Trim();
                            //objSuboutQCSample.Recovery = strRecovery.Trim();
                            //objSuboutQCSample.AnalyzedBy = strAnalyzedBy.Trim();
                            //objSuboutQCSample.AnalyzedDate = strAnalyzedDate.Trim();
                            //objSuboutQCSample.ApprovedBy = strApprovedBy.Trim();
                            //objSuboutQCSample.ApprovedDate = strApprovedDate.Trim();
                            //objSuboutQCSample.ValidatedBy = strValidatedBy.Trim();
                            //objSuboutQCSample.ValidatedDate = strValidatedDate.Trim();
                            //os.CommitChanges();
                            //Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "importdata"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            #endregion
                            string strPattern = @"(?:\r|\n|\a|\t|)";
                            if (!string.IsNullOrEmpty(row[0].ToString()))
                            {
                                row[0] = System.Text.RegularExpressions.Regex.Replace(row[0].ToString(), strPattern, string.Empty);
                            }
                            if (!string.IsNullOrEmpty(row[2].ToString()))
                            {
                                row[2] = System.Text.RegularExpressions.Regex.Replace(row[2].ToString(), strPattern, string.Empty);
                            }
                            if (!string.IsNullOrEmpty(row[3].ToString()))
                            {
                                row[3] = System.Text.RegularExpressions.Regex.Replace(row[3].ToString(), strPattern, string.Empty);
                            }
                            if (!string.IsNullOrEmpty(row[4].ToString()))
                            {
                                row[4] = System.Text.RegularExpressions.Regex.Replace(row[4].ToString(), strPattern, string.Empty);
                            }
                            if (!string.IsNullOrEmpty(row[6].ToString()))
                            {
                                row[6] = System.Text.RegularExpressions.Regex.Replace(row[6].ToString(), strPattern, string.Empty);
                            }
                            if (!string.IsNullOrEmpty(row[7].ToString()))
                            {
                                row[7] = System.Text.RegularExpressions.Regex.Replace(row[7].ToString(), strPattern, string.Empty);
                            }
                            if (!string.IsNullOrEmpty(row[8].ToString()))
                            {
                                row[8] = System.Text.RegularExpressions.Regex.Replace(row[8].ToString(), strPattern, string.Empty);
                            }
                            if (!string.IsNullOrEmpty(row[1].ToString()))
                            {
                                row[1] = System.Text.RegularExpressions.Regex.Replace(row[1].ToString(), strPattern, string.Empty);
                            }
                            if (string.IsNullOrEmpty(row[1].ToString()))
                            {
                                if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                                {
                                    foreach (SampleParameter objSuboutSample in ((ListView)lstviewSuboutSample.ListView).CollectionSource.List.Cast<SampleParameter>().ToList())
                                    {
                                        if (objSuboutSample != null && objSuboutSample.SuboutSample.SuboutOrderID != null && objSuboutSample.Samplelogin.JobID.JobID != null
                                            && objSuboutSample.Samplelogin.SampleID != null && objSuboutSample.Testparameter.TestMethod.TestName != null
                                            && objSuboutSample.Testparameter.TestMethod.TestName != null && objSuboutSample.Testparameter.TestMethod.MethodName.MethodNumber != null
                                            && objSuboutSample.Testparameter.Parameter.ParameterName != null)
                                        {
                                            string str = row[6].ToString().Trim();
                                            if (string.IsNullOrEmpty(row[1].ToString())
                                                && objSuboutSample.Samplelogin.ClientSampleID == row[3].ToString().Trim()
                                                && objSuboutSample.Testparameter.TestMethod.MatrixName.MatrixName.Trim() == row[4].ToString().Trim()
                                                && objSuboutSample.Testparameter.TestMethod.TestName.Trim() == row[5].ToString().Trim()
                                                && objSuboutSample.Testparameter.TestMethod.MethodName.MethodNumber.Trim() == row[6].ToString().Trim()
                                                && objSuboutSample.Testparameter.Parameter.ParameterName.Trim() == row[7].ToString().Trim())
                                            {
                                                //if (dt.Columns.Contains(rmChinese.GetString("NumericResult")) && !row.IsNull(rmChinese.GetString("NumericResult")))
                                                //{
                                                //    strNumericResult = row[rmChinese.GetString("NumericResult")].ToString();
                                                //}
                                                //else if (dt.Columns.Contains(rmEnglish.GetString("NumericResult")) && !row.IsNull(rmEnglish.GetString("NumericResult")))
                                                //{
                                                //    strNumericResult = row[rmEnglish.GetString("NumericResult")].ToString();
                                                //}
                                                //else
                                                //{
                                                //    strNumericResult = string.Empty;
                                                //}
                                                if (dt.Columns.Contains(rmChinese.GetString("Results")) && !row.IsNull(rmChinese.GetString("Results")))
                                                {
                                                    strResult = row[rmChinese.GetString("Results")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("Results")) && !row.IsNull(rmEnglish.GetString("Results")))
                                                {
                                                    strResult = row[rmEnglish.GetString("Results")].ToString();
                                                }
                                                else
                                                {
                                                    strResult = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("Units")) && !row.IsNull(rmChinese.GetString("Units")))
                                                {
                                                    strUnits = row[rmChinese.GetString("Units")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("Units")) && !row.IsNull(rmEnglish.GetString("Units")))
                                                {
                                                    strUnits = row[rmEnglish.GetString("Units")].ToString();
                                                }
                                                else
                                                {
                                                    strUnits = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("DF")) && !row.IsNull(rmChinese.GetString("DF")))
                                                {
                                                    strDF = row[rmChinese.GetString("DF")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("DF")) && !row.IsNull(rmEnglish.GetString("DF")))
                                                {
                                                    strDF = row[rmEnglish.GetString("DF")].ToString();
                                                }
                                                else
                                                {
                                                    strDF = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("LOQ")) && !row.IsNull(rmChinese.GetString("LOQ")))
                                                {
                                                    strLOQ = row[rmChinese.GetString("LOQ")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("LOQ")) && !row.IsNull(rmEnglish.GetString("LOQ")))
                                                {
                                                    strLOQ = row[rmEnglish.GetString("LOQ")].ToString();
                                                }
                                                else
                                                {
                                                    strLOQ = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("UQL")) && !row.IsNull(rmChinese.GetString("UQL")))
                                                {
                                                    strUQl = row[rmChinese.GetString("UQL")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("UQL")) && !row.IsNull(rmEnglish.GetString("UQL")))
                                                {
                                                    strUQl = row[rmEnglish.GetString("UQL")].ToString();
                                                }
                                                else
                                                {
                                                    strUQl = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                                                {
                                                    strRptLimit = row[rmChinese.GetString("RptLimit")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                                                {
                                                    strRptLimit = row[rmEnglish.GetString("RptLimit")].ToString();
                                                }
                                                else
                                                {
                                                    strRptLimit = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                                                {
                                                    strMDL = row[rmChinese.GetString("MDL")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                                                {
                                                    strMDL = row[rmEnglish.GetString("MDL")].ToString();
                                                }
                                                else
                                                {
                                                    strMDL = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("QcType")) && !row.IsNull(rmChinese.GetString("QcType")))
                                                {
                                                    strQctype = row[rmChinese.GetString("QcType")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("QcType")) && !row.IsNull(rmEnglish.GetString("QcType")))
                                                {
                                                    strQctype = row[rmEnglish.GetString("QcType")].ToString();
                                                }
                                                else
                                                {
                                                    strQctype = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                                                {
                                                    strRptLimit = row[rmChinese.GetString("RptLimit")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                                                {
                                                    strRptLimit = row[rmEnglish.GetString("RptLimit")].ToString();
                                                }
                                                else
                                                {
                                                    strRptLimit = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                                                {
                                                    strMDL = row[rmChinese.GetString("MDL")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                                                {
                                                    strMDL = row[rmEnglish.GetString("MDL")].ToString();
                                                }
                                                else
                                                {
                                                    strMDL = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("SpikeAmount")) && !row.IsNull(rmChinese.GetString("SpikeAmount")))
                                                {
                                                    strSpikeAmount = Convert.ToDouble(row[rmChinese.GetString("SpikeAmount")]);
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("SpikeAmount")) && !row.IsNull(rmEnglish.GetString("SpikeAmount")))
                                                {
                                                    strSpikeAmount = Convert.ToDouble(row[rmEnglish.GetString("SpikeAmount")].ToString());
                                                }
                                                else
                                                {
                                                    strSpikeAmount = 0;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("%Recovery")) && !row.IsNull(rmChinese.GetString("%Recovery")))
                                                {
                                                    strRecovery = row[rmChinese.GetString("%Recovery")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("%Recovery")) && !row.IsNull(rmEnglish.GetString("%Recovery")))
                                                {
                                                    strRecovery = row[rmEnglish.GetString("%Recovery")].ToString();
                                                }
                                                else
                                                {
                                                    strRecovery = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("%RPD")) && !row.IsNull(rmChinese.GetString("%RPD")))
                                                {
                                                    strRPD = row[rmChinese.GetString("%RPD")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("%RPD")) && !row.IsNull(rmEnglish.GetString("%RPD")))
                                                {
                                                    strRPD = row[rmEnglish.GetString("%RPD")].ToString();

                                                }
                                                else
                                                {
                                                    strRPD = string.Empty;
                                                }
                                                if (dt.Columns.Contains(rmChinese.GetString("AnalyzedBy")) && !row.IsNull(rmChinese.GetString("AnalyzedBy")))
                                                {
                                                    strAnalyzedBy = row[rmChinese.GetString("AnalyzedBy")].ToString();
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("AnalyzedBy")) && !row.IsNull(rmEnglish.GetString("AnalyzedBy")))
                                                {
                                                    strAnalyzedBy = row[rmEnglish.GetString("AnalyzedBy")].ToString();
                                                }
                                                else
                                                {
                                                    strAnalyzedBy = string.Empty;
                                                }
                                                strAnalyzedDate = null;
                                                if (dt.Columns.Contains(rmChinese.GetString("AnalyzedDate")) && !row.IsNull(rmChinese.GetString("AnalyzedDate")))
                                                {
                                                    if (row[rmChinese.GetString("AnalyzedDate")].GetType() == typeof(DateTime))
                                                    {
                                                        strAnalyzedDate = Convert.ToDateTime(row[rmChinese.GetString("AnalyzedDate")]);
                                                    }
                                                    else if (row[rmChinese.GetString("AnalyzedDate")].GetType() == typeof(string))
                                                    {
                                                        string strFollowUpDate = row[rmChinese.GetString("AnalyzedDate")].ToString();
                                                        if (strFollowUpDate.Contains("/"))
                                                        {
                                                            string strdate = row[rmChinese.GetString("AnalyzedDate")].ToString();
                                                            if (strdate != string.Empty && !string.IsNullOrEmpty(strdate))
                                                            {
                                                                strAnalyzedDate = DateTime.ParseExact(strdate, "M/dd/yyyy", null);
                                                            }
                                                        }
                                                        else if (strFollowUpDate.Contains("-"))
                                                        {
                                                            string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                            if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                            {
                                                                if (arrFollowUpDate[0].Length <= 2)
                                                                {
                                                                    strAnalyzedDate = Convert.ToDateTime(strFollowUpDate);
                                                                }
                                                                else
                                                                {
                                                                    DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                    strAnalyzedDate = date;
                                                                }
                                                            }
                                                        }
                                                        else if (strFollowUpDate.Contains("."))
                                                        {
                                                            string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                            if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                            {
                                                                if (arrFollowUpDate[0].Length <= 2)
                                                                {
                                                                    strAnalyzedDate = Convert.ToDateTime(strFollowUpDate);
                                                                }
                                                                else
                                                                {
                                                                    DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                    strAnalyzedDate = date;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (dt.Columns.Contains(rmEnglish.GetString("AnalyzedDate")) && !row.IsNull(rmEnglish.GetString("AnalyzedDate")))
                                                {
                                                    if (row[rmEnglish.GetString("AnalyzedDate")].GetType() == typeof(DateTime))
                                                    {
                                                        strAnalyzedDate = Convert.ToDateTime(row[rmEnglish.GetString("AnalyzedDate")]);
                                                    }
                                                    else if (row[rmEnglish.GetString("AnalyzedDate")].GetType() == typeof(string))
                                                    {
                                                        string strFollowUpDate = row[rmEnglish.GetString("AnalyzedDate")].ToString();
                                                        if (strFollowUpDate.Contains("/"))
                                                        {
                                                            string strdate = row[rmEnglish.GetString("AnalyzedDate")].ToString();
                                                            if (strdate != string.Empty && !string.IsNullOrEmpty(strdate))
                                                            {
                                                                strAnalyzedDate = DateTime.ParseExact(strdate, "M/dd/yyyy", null);
                                                            }
                                                        }
                                                        else if (strFollowUpDate.Contains("-"))
                                                        {
                                                            string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                            if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                            {
                                                                if (arrFollowUpDate[0].Length <= 2)
                                                                {
                                                                    strAnalyzedDate = Convert.ToDateTime(strFollowUpDate);
                                                                }
                                                                else
                                                                {
                                                                    DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                    strAnalyzedDate = date;
                                                                }
                                                            }
                                                        }
                                                        else if (strFollowUpDate.Contains("."))
                                                        {
                                                            string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                            if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                            {
                                                                if (arrFollowUpDate[0].Length <= 2)
                                                                {
                                                                    strAnalyzedDate = Convert.ToDateTime(strFollowUpDate);
                                                                }
                                                                else
                                                                {
                                                                    DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                                    strAnalyzedDate = date;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                objSuboutSample.ResultNumeric = strNumericResult.Trim();
                                                objSuboutSample.Result = strResult.Trim();
                                                if (strUnits != null && !string.IsNullOrEmpty(strUnits))
                                                {
                                                    Unit obj = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strUnits + "'"));
                                                    if (obj != null)
                                                    {
                                                        objSuboutSample.Units = obj;
                                                    }
                                                    else
                                                    {
                                                        obj = ObjectSpace.CreateObject<Unit>();
                                                        obj.UnitName = strUnits;
                                                        objSuboutSample.Units = obj;
                                                    }
                                                }
                                                objSuboutSample.DF = strDF.Trim();
                                                objSuboutSample.LOQ = strLOQ.Trim();
                                                objSuboutSample.UQL = strUQl.Trim();
                                                objSuboutSample.RptLimit = strRptLimit.Trim();
                                                objSuboutSample.MDL = strMDL.Trim();
                                                objSuboutSample.Rec = strRecovery.Trim();
                                                objSuboutSample.RPD = strRPD.Trim();
                                                objSuboutSample.SuboutAnalyzedBy = strAnalyzedBy;
                                                if (strAnalyzedDate != DateTime.MinValue && strAnalyzedDate != null)
                                                {
                                                    objSuboutSample.AnalyzedDate = strAnalyzedDate;
                                                }
                                                objSuboutSample.IsEDDImported = true;
                                                ObjectSpace.CommitChanges();
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "importdata"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                            }
                                        }


                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(row[1].ToString()) && !string.IsNullOrEmpty(row[3].ToString()) && !string.IsNullOrEmpty(row[5].ToString()) && !string.IsNullOrEmpty(row[6].ToString()) && !string.IsNullOrEmpty(row[7].ToString()))
                            {
                                if (lstviewSuboutQCSample != null)
                                {
                                    SubOutSampleRegistrations objSSR = ObjectSpace.GetObject((SubOutSampleRegistrations)View.CurrentObject);
                                    SuboutQcSample objSuboutQc = (((ListView)lstviewSuboutQCSample.ListView).CollectionSource.List.Cast<SuboutQcSample>().FirstOrDefault(i =>
                                      i.QCType == row[1].ToString().Trim()
                                      && i.Matrix.MatrixName == row[4].ToString().Trim()
                                      && i.Test.TestName == row[5].ToString().Trim()
                                      && i.Method.MethodNumber == row[6].ToString().Trim()
                                      && i.Parameter == row[7].ToString().Trim()));
                                    //if (dt.Columns.Contains(rmChinese.GetString("NumericResult")) && !row.IsNull(rmChinese.GetString("NumericResult")))
                                    //{
                                    //    strNumericResult = row[rmChinese.GetString("NumericResult")].ToString();
                                    //}
                                    //else if (dt.Columns.Contains(rmEnglish.GetString("NumericResult")) && !row.IsNull(rmEnglish.GetString("NumericResult")))
                                    //{
                                    //    strNumericResult = row[rmEnglish.GetString("NumericResult")].ToString();
                                    //}
                                    //else
                                    //{
                                    //    strNumericResult = string.Empty;
                                    //}
                                    if (dt.Columns.Contains(rmChinese.GetString("Results")) && !row.IsNull(rmChinese.GetString("Results")))
                                    {
                                        strResult = row[rmChinese.GetString("Results")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Results")) && !row.IsNull(rmEnglish.GetString("Results")))
                                    {
                                        strResult = row[rmEnglish.GetString("Results")].ToString();
                                    }
                                    else
                                    {
                                        strResult = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Units")) && !row.IsNull(rmChinese.GetString("Units")))
                                    {
                                        strUnits = row[rmChinese.GetString("Units")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Units")) && !row.IsNull(rmEnglish.GetString("Units")))
                                    {
                                        strUnits = row[rmEnglish.GetString("Units")].ToString();
                                    }
                                    else
                                    {
                                        strUnits = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("DF")) && !row.IsNull(rmChinese.GetString("DF")))
                                    {
                                        strDF = row[rmChinese.GetString("DF")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("DF")) && !row.IsNull(rmEnglish.GetString("DF")))
                                    {
                                        strDF = row[rmEnglish.GetString("DF")].ToString();
                                    }
                                    else
                                    {
                                        strDF = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("LOQ")) && !row.IsNull(rmChinese.GetString("LOQ")))
                                    {
                                        strLOQ = row[rmChinese.GetString("LOQ")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("LOQ")) && !row.IsNull(rmEnglish.GetString("LOQ")))
                                    {
                                        strLOQ = row[rmEnglish.GetString("LOQ")].ToString();
                                    }
                                    else
                                    {
                                        strLOQ = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("UQL")) && !row.IsNull(rmChinese.GetString("UQL")))
                                    {
                                        strUQl = row[rmChinese.GetString("UQL")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("UQL")) && !row.IsNull(rmEnglish.GetString("UQL")))
                                    {
                                        strUQl = row[rmEnglish.GetString("UQL")].ToString();
                                    }
                                    else
                                    {
                                        strUQl = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                                    {
                                        strRptLimit = row[rmChinese.GetString("RptLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                                    {
                                        strRptLimit = row[rmEnglish.GetString("RptLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRptLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                                    {
                                        strMDL = row[rmChinese.GetString("MDL")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                                    {
                                        strMDL = row[rmEnglish.GetString("MDL")].ToString();
                                    }
                                    else
                                    {
                                        strMDL = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("QcType")) && !row.IsNull(rmChinese.GetString("QcType")))
                                    {
                                        strQctype = row[rmChinese.GetString("QcType")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("QcType")) && !row.IsNull(rmEnglish.GetString("QcType")))
                                    {
                                        strQctype = row[rmEnglish.GetString("QcType")].ToString();
                                    }
                                    else
                                    {
                                        strQctype = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RptLimit")) && !row.IsNull(rmChinese.GetString("RptLimit")))
                                    {
                                        strRptLimit = row[rmChinese.GetString("RptLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RptLimit")) && !row.IsNull(rmEnglish.GetString("RptLimit")))
                                    {
                                        strRptLimit = row[rmEnglish.GetString("RptLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRptLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("MDL")) && !row.IsNull(rmChinese.GetString("MDL")))
                                    {
                                        strMDL = row[rmChinese.GetString("MDL")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("MDL")) && !row.IsNull(rmEnglish.GetString("MDL")))
                                    {
                                        strMDL = row[rmEnglish.GetString("MDL")].ToString();
                                    }
                                    else
                                    {
                                        strMDL = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("SpikeAmount")) && !row.IsNull(rmChinese.GetString("SpikeAmount")))
                                    {
                                        strSpikeAmount = Convert.ToDouble(row[rmChinese.GetString("SpikeAmount")]);
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("SpikeAmount")) && !row.IsNull(rmEnglish.GetString("SpikeAmount")))
                                    {
                                        strSpikeAmount = Convert.ToDouble(row[rmEnglish.GetString("SpikeAmount")].ToString());
                                    }
                                    else
                                    {
                                        strSpikeAmount = 0;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("%Recovery")) && !row.IsNull(rmChinese.GetString("%Recovery")))
                                    {
                                        strRecovery = row[rmChinese.GetString("%Recovery")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("%Recovery")) && !row.IsNull(rmEnglish.GetString("%Recovery")))
                                    {
                                        strRecovery = row[rmEnglish.GetString("%Recovery")].ToString();
                                    }
                                    else
                                    {
                                        strRecovery = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("%RPD")) && !row.IsNull(rmChinese.GetString("%RPD")))
                                    {
                                        strRPD = row[rmChinese.GetString("%RPD")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("%RPD")) && !row.IsNull(rmEnglish.GetString("%RPD")))
                                    {
                                        strRPD = row[rmEnglish.GetString("%RPD")].ToString();

                                    }
                                    else
                                    {
                                        strRPD = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("JobID")) && !row.IsNull(rmChinese.GetString("JobID")))
                                    {
                                        strJobID = row[rmChinese.GetString("JobID")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("JobID")) && !row.IsNull(rmEnglish.GetString("JobID")))
                                    {
                                        strJobID = row[rmEnglish.GetString("JobID")].ToString();

                                    }
                                    else
                                    {
                                        strJobID = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Matrix")) && !row.IsNull(rmChinese.GetString("Matrix")))
                                    {
                                        strMatrix = row[rmChinese.GetString("Matrix")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Matrix")) && !row.IsNull(rmEnglish.GetString("Matrix")))
                                    {
                                        strMatrix = row[rmEnglish.GetString("Matrix")].ToString();

                                    }
                                    else
                                    {
                                        strMatrix = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Test")) && !row.IsNull(rmChinese.GetString("Test")))
                                    {
                                        strTest = row[rmChinese.GetString("Test")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Test")) && !row.IsNull(rmEnglish.GetString("Test")))
                                    {
                                        strTest = row[rmEnglish.GetString("Test")].ToString();
                                    }
                                    else
                                    {
                                        strTest = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Method")) && !row.IsNull(rmChinese.GetString("Method")))
                                    {
                                        strMethod = row[rmChinese.GetString("Method")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Method")) && !row.IsNull(rmEnglish.GetString("Method")))
                                    {
                                        strMethod = row[rmEnglish.GetString("Method")].ToString();
                                    }
                                    else
                                    {
                                        strMethod = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Parameter")) && !row.IsNull(rmChinese.GetString("Parameter")))
                                    {
                                        strParameter = row[rmChinese.GetString("Parameter")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Parameter")) && !row.IsNull(rmEnglish.GetString("Parameter")))
                                    {
                                        strParameter = row[rmEnglish.GetString("Parameter")].ToString();
                                    }
                                    else
                                    {
                                        strParameter = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("SampleID")) && !row.IsNull(rmChinese.GetString("SampleID")))
                                    {
                                        strSampleID = row[rmChinese.GetString("SampleID")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("SampleID")) && !row.IsNull(rmEnglish.GetString("SampleID")))
                                    {
                                        strSampleID = row[rmEnglish.GetString("SampleID")].ToString();
                                    }
                                    else
                                    {
                                        strSampleID = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RecLCLimit")) && !row.IsNull(rmChinese.GetString("RecLCLimit")))
                                    {
                                        strRecLCLimit = row[rmChinese.GetString("RecLCLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RecLCLimit")) && !row.IsNull(rmEnglish.GetString("RecLCLimit")))
                                    {
                                        strRecLCLimit = row[rmEnglish.GetString("RecLCLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRecLCLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RecUCLimit")) && !row.IsNull(rmChinese.GetString("RecUCLimit")))
                                    {
                                        strRecUCLimit = row[rmChinese.GetString("RecUCLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RecUCLimit")) && !row.IsNull(rmEnglish.GetString("RecUCLimit")))
                                    {
                                        strRecUCLimit = row[rmEnglish.GetString("RecUCLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRecUCLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RPDLCLimit")) && !row.IsNull(rmChinese.GetString("RPDLCLimit")))
                                    {
                                        strRPDLCLimit = row[rmChinese.GetString("RPDLCLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RPDLCLimit")) && !row.IsNull(rmEnglish.GetString("RPDLCLimit")))
                                    {
                                        strRPDLCLimit = row[rmEnglish.GetString("RPDLCLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRPDLCLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RPDUCLimit")) && !row.IsNull(rmChinese.GetString("RPDUCLimit")))
                                    {
                                        strRPDUCLimit = row[rmChinese.GetString("RPDUCLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RPDUCLimit")) && !row.IsNull(rmEnglish.GetString("RPDUCLimit")))
                                    {
                                        strRPDUCLimit = row[rmEnglish.GetString("RPDUCLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRPDUCLimit = string.Empty;
                                    }
                                    strAnalyzedDate = null;
                                    if (dt.Columns.Contains(rmChinese.GetString("AnalyzedDate")) && !row.IsNull(rmChinese.GetString("AnalyzedDate")))
                                    {
                                        if (row[rmChinese.GetString("AnalyzedDate")].GetType() == typeof(DateTime))
                                        {
                                            strAnalyzedDate = Convert.ToDateTime(row[rmChinese.GetString("AnalyzedDate")]);
                                        }
                                        else if (row[rmChinese.GetString("AnalyzedDate")].GetType() == typeof(string))
                                        {
                                            string strFollowUpDate = row[rmChinese.GetString("AnalyzedDate")].ToString();
                                            if (strFollowUpDate.Contains("/"))
                                            {
                                                string strdate = row[rmChinese.GetString("AnalyzedDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    strAnalyzedDate = DateTime.ParseExact(strdate, "M/dd/yyyy", null);
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("-"))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        strAnalyzedDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        strAnalyzedDate = date;
                                                    }
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("."))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        strAnalyzedDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        strAnalyzedDate = date;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("AnalyzedDate")) && !row.IsNull(rmEnglish.GetString("AnalyzedDate")))
                                    {
                                        if (row[rmEnglish.GetString("AnalyzedDate")].GetType() == typeof(DateTime))
                                        {
                                            strAnalyzedDate = Convert.ToDateTime(row[rmEnglish.GetString("AnalyzedDate")]);
                                        }
                                        else if (row[rmEnglish.GetString("AnalyzedDate")].GetType() == typeof(string))
                                        {
                                            string strFollowUpDate = row[rmEnglish.GetString("AnalyzedDate")].ToString();
                                            if (strFollowUpDate.Contains("/"))
                                            {
                                                string strdate = row[rmEnglish.GetString("AnalyzedDate")].ToString();
                                                if (strdate != string.Empty)
                                                {
                                                    strAnalyzedDate = DateTime.ParseExact(strdate, "M/dd/yyyy", null);
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("-"))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('-');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        strAnalyzedDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        strAnalyzedDate = date;
                                                    }
                                                }
                                            }
                                            else if (strFollowUpDate.Contains("."))
                                            {
                                                string[] arrFollowUpDate = strFollowUpDate.Split('.');
                                                if (arrFollowUpDate != null && arrFollowUpDate.Count() >= 3)
                                                {
                                                    if (arrFollowUpDate[0].Length <= 2)
                                                    {
                                                        strAnalyzedDate = Convert.ToDateTime(strFollowUpDate);
                                                    }
                                                    else
                                                    {
                                                        DateTime date = new DateTime(Convert.ToInt32(arrFollowUpDate[0]), Convert.ToInt32(arrFollowUpDate[1]), Convert.ToInt32(arrFollowUpDate[2]));
                                                        strAnalyzedDate = date;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RecLCLimit")) && !row.IsNull(rmChinese.GetString("RecLCLimit")))
                                    {
                                        strRecLCLimit = row[rmChinese.GetString("RecLCLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RecLCLimit")) && !row.IsNull(rmEnglish.GetString("RecLCLimit")))
                                    {
                                        strRecLCLimit = row[rmEnglish.GetString("RecLCLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRecLCLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RecUCLimit")) && !row.IsNull(rmChinese.GetString("RecUCLimit")))
                                    {
                                        strRecUCLimit = row[rmChinese.GetString("RecUCLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RecUCLimit")) && !row.IsNull(rmEnglish.GetString("RecUCLimit")))
                                    {
                                        strRecUCLimit = row[rmEnglish.GetString("RecUCLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRecUCLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RPDLCLimit")) && !row.IsNull(rmChinese.GetString("RPDLCLimit")))
                                    {
                                        strRPDLCLimit = row[rmChinese.GetString("RPDLCLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RPDLCLimit")) && !row.IsNull(rmEnglish.GetString("RPDLCLimit")))
                                    {
                                        strRPDLCLimit = row[rmEnglish.GetString("RPDLCLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRPDLCLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("RPDUCLimit")) && !row.IsNull(rmChinese.GetString("RPDUCLimit")))
                                    {
                                        strRPDUCLimit = row[rmChinese.GetString("RPDUCLimit")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("RPDUCLimit")) && !row.IsNull(rmEnglish.GetString("RPDUCLimit")))
                                    {
                                        strRPDUCLimit = row[rmEnglish.GetString("RPDUCLimit")].ToString();
                                    }
                                    else
                                    {
                                        strRPDUCLimit = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("AnalyzedBy")) && !row.IsNull(rmChinese.GetString("AnalyzedBy")))
                                    {
                                        strAnalyzedBy = row[rmChinese.GetString("AnalyzedBy")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("AnalyzedBy")) && !row.IsNull(rmEnglish.GetString("AnalyzedBy")))
                                    {
                                        strAnalyzedBy = row[rmEnglish.GetString("AnalyzedBy")].ToString();
                                    }
                                    else
                                    {
                                        strAnalyzedBy = string.Empty;
                                    }
                                    if (dt.Columns.Contains(rmChinese.GetString("Surrogate")) && !row.IsNull(rmChinese.GetString("Surrogate")))
                                    {
                                        strSurrgate = row[rmChinese.GetString("Surrogate")].ToString();
                                    }
                                    else if (dt.Columns.Contains(rmEnglish.GetString("Surrogate")) && !row.IsNull(rmEnglish.GetString("Surrogate")))
                                    {
                                        strSurrgate = row[rmEnglish.GetString("Surrogate")].ToString();
                                    }
                                    else
                                    {
                                        strSurrgate = string.Empty;
                                    }
                                    if (objSuboutQc == null)
                                    {
                                        SuboutQcSample objSuboutQCSample = ObjectSpace.CreateObject<SuboutQcSample>();
                                        if (objSSR != null)
                                        {
                                            objSuboutQCSample.SuboutID = objSSR;
                                        }
                                        if (strJobID != null)
                                        {
                                            Samplecheckin objSc = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", strJobID));
                                            if (objSc != null)
                                            {
                                                objSuboutQCSample.JobID = objSc;
                                            }
                                        }
                                        objSuboutQCSample.QCType = strQctype.Trim();
                                        objSuboutQCSample.SampleID = strSampleID.Trim();
                                        if (strMatrix != null)
                                        {
                                            Matrix objMx = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", strMatrix));
                                            if (objMx != null)
                                            {
                                                objSuboutQCSample.Matrix = objMx;
                                            }
                                        }
                                        if (strTest != null)
                                        {
                                            TestMethod objTm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=?", strTest));
                                            if (objTm != null)
                                            {
                                                objSuboutQCSample.Test = objTm;
                                            }
                                        }
                                        if (strMethod != null)
                                        {
                                            Method objM = ObjectSpace.FindObject<Method>(CriteriaOperator.Parse("[MethodNumber]=?", strMethod));
                                            if (objM != null)
                                            {
                                                objSuboutQCSample.Method = objM;
                                            }
                                        }
                                        objSuboutQCSample.Parameter = strParameter.Trim();
                                        objSuboutQCSample.NumericResult = strNumericResult.Trim();
                                        objSuboutQCSample.Result = strResult.Trim();
                                        if (!string.IsNullOrEmpty(strUnits))
                                        {
                                            Unit obj = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strUnits + "'"));
                                            if (obj != null)
                                            {
                                                objSuboutQCSample.Units = obj;
                                            }
                                            else
                                            {
                                                obj = ObjectSpace.CreateObject<Unit>();
                                                obj.UnitName = strUnits;
                                                objSuboutQCSample.Units = obj;
                                            }
                                        }
                                        objSuboutQCSample.DF = strDF.Trim();
                                        objSuboutQCSample.LOQ = strLOQ.Trim();
                                        objSuboutQCSample.UQL = strUQl.Trim();
                                        objSuboutQCSample.RptLimit = strRptLimit.Trim();
                                        objSuboutQCSample.MDL = strMDL.Trim();
                                        objSuboutQCSample.Rec = strRecovery.Trim();
                                        objSuboutQCSample.RPD = strRPD.Trim();
                                        objSuboutQCSample.RecLCLimit = strRecLCLimit.Trim();
                                        objSuboutQCSample.RPDLCLimit = strRPDLCLimit.Trim();
                                        objSuboutQCSample.RecUCLimit = strRecUCLimit.Trim();
                                        objSuboutQCSample.RPDUCLimit = strRPDUCLimit.Trim();
                                        objSuboutQCSample.Recovery = strRecovery.Trim();
                                        objSuboutQCSample.AnalyzedBy = strAnalyzedBy.Trim();
                                        objSuboutQCSample.ApprovedBy = strApprovedBy.Trim();
                                        objSuboutQCSample.ValidatedBy = strValidatedBy.Trim();
                                        objSuboutQCSample.Surrogate = strSurrgate.Trim();
                                        if (strAnalyzedDate != DateTime.MinValue && strAnalyzedDate != null)
                                        {
                                            objSuboutQCSample.AnalyzedDate = strAnalyzedDate;
                                        }
                                        objSSR.SubOutQcSample.Add(objSuboutQCSample);
                                        objSuboutQc.IsEDDImported = true;
                                        ObjectSpace.CommitChanges();
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "importdata"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    }
                                    else
                                    {
                                        if (objSSR != null)
                                        {
                                            objSuboutQc.SuboutID = objSSR;
                                        }
                                        if (strJobID != null)
                                        {
                                            Samplecheckin objSc = ObjectSpace.FindObject<Samplecheckin>(CriteriaOperator.Parse("[JobID]=?", strJobID));
                                            if (objSc != null)
                                            {
                                                objSuboutQc.JobID = objSc;
                                            }
                                        }
                                        objSuboutQc.QCType = strQctype.Trim();
                                        objSuboutQc.SampleID = strSampleID.Trim();
                                        objSuboutQc.SampleName = strSampleName.Trim();
                                        if (strMatrix != null)
                                        {
                                            Matrix objMx = ObjectSpace.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName]=?", strMatrix));
                                            if (objMx != null)
                                            {
                                                objSuboutQc.Matrix = objMx;
                                            }
                                        }
                                        if (strTest != null)
                                        {
                                            TestMethod objTm = ObjectSpace.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName]=?", strTest));
                                            if (objTm != null)
                                            {
                                                objSuboutQc.Test = objTm;
                                            }
                                        }
                                        if (strMethod != null)
                                        {
                                            Method objM = ObjectSpace.FindObject<Method>(CriteriaOperator.Parse("[MethodNumber]=?", strMethod));
                                            if (objM != null)
                                            {
                                                objSuboutQc.Method = objM;
                                            }
                                        }
                                        objSuboutQc.Parameter = strParameter.Trim();
                                        objSuboutQc.NumericResult = strNumericResult.Trim();
                                        objSuboutQc.Result = strResult.Trim();
                                        if (!string.IsNullOrEmpty(strUnits))
                                        {
                                            Unit obj = ObjectSpace.FindObject<Unit>(CriteriaOperator.Parse("[UnitName]='" + strUnits + "'"));
                                            if (obj != null)
                                            {
                                                objSuboutQc.Units = obj;
                                            }
                                            else
                                            {
                                                obj = ObjectSpace.CreateObject<Unit>();
                                                obj.UnitName = strUnits;
                                                objSuboutQc.Units = obj;
                                            }
                                        }
                                        objSuboutQc.DF = strDF.Trim();
                                        objSuboutQc.LOQ = strLOQ.Trim();
                                        objSuboutQc.UQL = strUQl.Trim();
                                        objSuboutQc.RptLimit = strRptLimit.Trim();
                                        objSuboutQc.MDL = strMDL.Trim();
                                        objSuboutQc.Rec = strRecovery.Trim();
                                        objSuboutQc.RPD = strRPD.Trim();
                                        objSuboutQc.RecLCLimit = strRecLCLimit.Trim();
                                        objSuboutQc.RPDLCLimit = strRPDLCLimit.Trim();
                                        objSuboutQc.RecUCLimit = strRecUCLimit.Trim();
                                        objSuboutQc.RPDUCLimit = strRPDUCLimit.Trim();
                                        objSuboutQc.Recovery = strRecovery.Trim();
                                        objSuboutQc.AnalyzedBy = strAnalyzedBy.Trim();
                                        if (strAnalyzedDate != DateTime.MinValue && strAnalyzedDate != null)
                                        {
                                            objSuboutQc.AnalyzedDate = strAnalyzedDate;
                                        }
                                        objSuboutQc.ApprovedBy = strApprovedBy.Trim();
                                        if (strApprovedDate != DateTime.MinValue && strApprovedDate != null)
                                        {
                                            objSuboutQc.ApprovedDate = strApprovedDate;
                                        }
                                        objSuboutQc.ValidatedBy = strValidatedBy.Trim();
                                        if (strValidatedDate != DateTime.MinValue && strValidatedDate != null)
                                        {
                                            objSuboutQc.ValidatedDate = strValidatedDate;
                                        }
                                        objSuboutQc.Surrogate = strSurrgate.Trim();
                                        objSuboutQc.IsEDDImported = true;
                                        ObjectSpace.CommitChanges();
                                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "importdata"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                    }
                                    
                                }
                            }

                        }

                    }
                    if(lstviewSuboutSample!=null && lstviewSuboutSample.ListView!=null)
                    {
                        lstviewSuboutSample.ListView.ObjectSpace.Refresh();
                    }
                    if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView != null)
                    {
                        lstviewSuboutQCSample.ListView.ObjectSpace.Refresh();
                    } 
                    ObjectSpace.Refresh();
                    
                    //Frame.GetController<RefreshController>().RefreshAction.DoExecute();
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "uploadfile"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    e.CanCloseWindow = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void ViewHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SubOutSampleRegistrations_ListView_TestOrder")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(os, typeof(SubOutSampleRegistrations));
                    ListView listview = Application.CreateListView("SubOutSampleRegistrations_ListView_TestOrder_History", cs, true);
                    Frame.SetView(listview);
                }
                else
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    CollectionSource cs = new CollectionSource(os, typeof(SubOutSampleRegistrations));
                    cs.Criteria["HistoricalRecordsFilter"] = CriteriaOperator.Parse("[Status] = 'SuboutPendingValidation' or [Status] = 'SuboutPendingApproval' or [Status] = 'IsExported'");
                    ListView listview = Application.CreateListView("SubOutSampleRegistrations_ListView_ViewMode", cs, true);
                    Frame.SetView(listview);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SuboutAllowEdit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace(typeof(SubOutSampleRegistrations));
                SubOutSampleRegistrations obj = os.CreateObject<SubOutSampleRegistrations>();
                DetailView createdView = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_Reason", true, obj);
                createdView.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                showViewParameters.Context = TemplateContext.NestedFrame;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += SuboutAllowEdit_Accepting;
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

        private void SuboutAllowEdit_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (View.CurrentObject != null)
                {
                    SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)e.AcceptActionArgs.CurrentObject;
                    if (objSubout != null && !string.IsNullOrEmpty(objSubout.Reason))
                    {
                        SubOutSampleRegistrations obj = (SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject;
                        if (obj != null)
                        {
                            obj.Reason = objSubout.Reason;
                            ObjectSpace.CommitChanges();
                            IObjectSpace os = Application.CreateObjectSpace();
                            SubOutSampleRegistrations objSubotMain = os.GetObject((SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject);
                            if (objSubotMain != null)
                            {
                                DetailView dv = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_SuboutResultEntry_History", true, objSubotMain);
                                dv.ViewEditMode = ViewEditMode.Edit;
                                Frame.SetView(dv);
                            }
                        }
                    }
                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "returnreason"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SuboutResultHistoryDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedChoiceActionItem.Id == "1M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "3M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 3 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "6M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 6 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "1Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "2Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 2 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "5Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 5 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "ALL")
                {
                    ((ListView)View).CollectionSource.Criteria.Remove("DateFilter1");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SuboutSampleHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(os, typeof(SubOutSampleRegistrations));
                ListView listview = Application.CreateListView("SubOutSampleRegistrations_ListView_SuboutRegHistory", cs, true);
                Frame.SetView(listview);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SubOutSampleHistoryDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {

                if (e.SelectedChoiceActionItem.Id == "1M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 1 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "3M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 3 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "6M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(Samplelogin.JobID.RecievedDate, Now()) <= 6 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "1Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 1 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "2Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 2 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "5Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(Samplelogin.JobID.RecievedDate, Now()) <= 5 And [Samplelogin.JobID.RecievedDate] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "ALL")
                {
                    ((ListView)View).CollectionSource.Criteria.Remove("DateFilter1");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void historyofSuboutNotification_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(os, typeof(SubOutSampleRegistrations));
                ListView listview = Application.CreateListView("SubOutSampleRegistrations_ListView_NotificationQueueView", cs, true);
                Frame.SetView(listview);
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
                    if (View.Id == "SampleParameter_ListView_Copy_SubOutPendingSamples")
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null)
                        {
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(parameter), "Oid");
                            //HttpContext.Current.Session["Client"] = gridListEditor.Grid.GetRowValues(int.Parse(parameter), "ClientName");
                            IObjectSpace os = Application.CreateObjectSpace();
                            CollectionSource cs = new CollectionSource(os, typeof(Customer));
                            ListView lv = Application.CreateListView("Customer_LookupListView_SuboutClient", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.AcceptAction.Active.SetItemValue("Ok", false);
                            dc.CancelAction.Active.SetItemValue("Cancel", false);
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                    else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView")
                    {
                        string[] values = parameter.Split('|');
                        if (values != null && values.Length >= 5 && values[4] == "ResultNumeric")
                        {
                            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                            if (editor != null && editor.Grid != null)
                            {
                                ASPxGridView grid = editor.Grid as ASPxGridView;
                                List<string> mainparam = new List<string>();
                                List<string> response = new List<string>();
                                if (parameter.Contains(";"))
                                {
                                    mainparam = parameter.Split(';').ToList();
                                }
                                else
                                {
                                    mainparam.Add(parameter);
                                }
                                foreach (string mainparamsplit in mainparam)
                                {
                                    Testparameter testparameterobj = null;

                                    string SpResult = string.Empty;
                                    var param = mainparamsplit.Split('|');
                                    if (param.Count() > 1)
                                    {
                                        var sampleoid = grid.GetRowValues(Convert.ToInt32(param[1]), "Oid");
                                        SampleParameter sampleParameter = ObjectSpace.GetObjectByKey<SampleParameter>(sampleoid);
                                        if (sampleParameter != null && sampleParameter.Testparameter != null)
                                        {
                                            testparameterobj = ObjectSpace.GetObjectByKey<Testparameter>(sampleParameter.Testparameter.Oid);
                                        }
                                    }
                                    if (testparameterobj != null)
                                    {
                                        string result;
                                        string resultoutput = string.Empty;
                                        if (testparameterobj.RptLimit != null && Convert.ToDouble(param[0]) >= Convert.ToDouble(testparameterobj.RptLimit))
                                        {
                                            if (testparameterobj.CutOff != null && testparameterobj.CutOff.Length > 0)
                                            {
                                                if (testparameterobj.SigFig != null && testparameterobj.SigFig.Length > 0 && Convert.ToDouble(param[0]) >= Convert.ToDouble(testparameterobj.CutOff))
                                                {
                                                    var Cal = Roundoff.RoundoffInput(param[0], testparameterobj.SigFig).Split('|');
                                                    result = Cal[0];
                                                }
                                                else if (testparameterobj.Decimal != null && testparameterobj.Decimal.Length > 0 && Convert.ToDouble(param[0]) < Convert.ToDouble(testparameterobj.CutOff))
                                                {
                                                    result = Roundoff.FormatDecimalValue(param[0], Convert.ToInt32(testparameterobj.Decimal));
                                                }
                                                else
                                                {
                                                    result = param[0];
                                                }
                                            }
                                            else
                                            {
                                                if (testparameterobj.SigFig != null && testparameterobj.SigFig.Length > 0)
                                                {
                                                    var Cal = Roundoff.RoundoffInput(param[0], testparameterobj.SigFig).Split('|');
                                                    result = Cal[0];
                                                }
                                                else if (testparameterobj.Decimal != null && testparameterobj.Decimal.Length > 0)
                                                {
                                                    result = Roundoff.FormatDecimalValue(param[0], Convert.ToInt32(testparameterobj.Decimal));
                                                }
                                                else
                                                {
                                                    result = param[0];
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result = param[0];
                                            SpResult = result;
                                        }
                                        if (testparameterobj.RptLimit != null && Convert.ToDouble(result) < Convert.ToDouble(testparameterobj.RptLimit))
                                        {
                                            resultoutput = testparameterobj.DefaultResult + "|" + param[1] + "|";
                                            SpResult = testparameterobj.DefaultResult;
                                        }
                                        else if (testparameterobj.RptLimit != null && Convert.ToDouble(result) >= Convert.ToDouble(testparameterobj.RptLimit))
                                        {
                                            if (((param[2] != "" && param[2] != "null") && (param[3] != "" && param[3] != "null")) && (Convert.ToDouble(result) < Convert.ToDouble(param[2]) || Convert.ToDouble(result) > Convert.ToDouble(param[3])))
                                            {
                                                resultoutput = result + "|" + param[1] + "|redcell";
                                                SpResult = result;
                                            }
                                            else if (Convert.ToDouble(result) == Convert.ToDouble(testparameterobj.RptLimit))
                                            {
                                                resultoutput = result + "|" + param[1] + "|yellowcell";
                                                SpResult = result;
                                            }
                                            else
                                            {
                                                resultoutput = result + "|" + param[1] + "|";
                                                SpResult = result;
                                            }
                                        }
                                        else
                                        {
                                            resultoutput = result + "|" + param[1] + "|";
                                            SpResult = result;
                                        }
                                        response.Add(resultoutput);
                                    }
                                    if (param.Count() > 1)
                                    {
                                        var sampleoid = grid.GetRowValues(Convert.ToInt32(param[1]), "Oid");
                                        SampleParameter sampleParameter = ObjectSpace.GetObjectByKey<SampleParameter>(sampleoid);
                                        sampleParameter.Result = SpResult;
                                    }
                                }
                                //grid.UpdateEdit();
                                //if (response.Count > 0)
                                //{
                                //    values[5] = string.Join(";", response.ToArray());
                                //    e.Result = string.Join(";", response.ToArray());

                                //} 
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
        private void SetConnectionString()
        {
            try
            {
                string[] connectionstring = objDRDCInfo.WebConfigConn.Split(';');
                objDRDCInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private string ReportGeneratePdf()
        {
            try
            {
                SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                if (objSubout != null)
                {
                    string strSuboutID = "'" + objSubout.SuboutOrderID + "'";
                    string strTempPath = Path.GetTempPath();
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SuboutCOCReport\")) == false)
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SuboutCOCReport\"));
                    }
                    string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\SuboutCOCReport\" + timeStamp + ".pdf");
                    XtraReport xtraReport = new XtraReport();

                    objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    SetConnectionString();

                    DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                    ObjReportingInfo.strSuboutOrderID = "'" + objSubout.SuboutOrderID + "'";
                    xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Subout_COC_Report", ObjReportingInfo, false);
                    xtraReport.ExportToPdf(strExportedPath);
                    if (boolReportPreview)
                    {
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 3;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        boolReportPreview = false;
                    }

                    return strExportedPath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        private void Grid_CustomDataCallback(object sender, ASPxGridViewCustomDataCallbackEventArgs e)
        {
            try
            {
                if (e.Parameters != "CustomCallback")
                {
                    ASPxGridView grid = sender as ASPxGridView;
                    List<string> mainparam = new List<string>();
                    List<string> response = new List<string>();
                    if (e.Parameters.Contains(";"))
                    {
                        mainparam = e.Parameters.Split(';').ToList();
                    }
                    else
                    {
                        mainparam.Add(e.Parameters);
                    }
                    foreach (string mainparamsplit in mainparam)
                    {
                        Testparameter testparameterobj = null;
                        var param = mainparamsplit.Split('|');
                        if (param.Count() > 1)
                        {
                            var sampleoid = grid.GetRowValues(Convert.ToInt32(param[1]), "Oid");
                            SampleParameter sampleParameter = ObjectSpace.GetObjectByKey<SampleParameter>(sampleoid);
                            if (sampleParameter != null && sampleParameter.Testparameter != null)
                            {
                                testparameterobj = ObjectSpace.GetObjectByKey<Testparameter>(sampleParameter.Testparameter.Oid);
                            }
                        }
                        if (testparameterobj != null)
                        {
                            string result;
                            string resultoutput = string.Empty;
                            if (testparameterobj.RptLimit != null && Convert.ToDouble(param[0]) >= Convert.ToDouble(testparameterobj.RptLimit))
                            {
                                if (testparameterobj.CutOff != null && testparameterobj.CutOff.Length > 0)
                                {
                                    if (testparameterobj.SigFig != null && testparameterobj.SigFig.Length > 0 && Convert.ToDouble(param[0]) >= Convert.ToDouble(testparameterobj.CutOff))
                                    {
                                        var Cal = Roundoff.RoundoffInput(param[0], testparameterobj.SigFig).Split('|');
                                        result = Cal[0];
                                    }
                                    else if (testparameterobj.Decimal != null && testparameterobj.Decimal.Length > 0 && Convert.ToDouble(param[0]) < Convert.ToDouble(testparameterobj.CutOff))
                                    {
                                        result = Roundoff.FormatDecimalValue(param[0], Convert.ToInt32(testparameterobj.Decimal));
                                    }
                                    else
                                    {
                                        result = param[0];
                                    }
                                }
                                else
                                {
                                    if (testparameterobj.SigFig != null && testparameterobj.SigFig.Length > 0)
                                    {
                                        var Cal = Roundoff.RoundoffInput(param[0], testparameterobj.SigFig).Split('|');
                                        result = Cal[0];
                                    }
                                    else if (testparameterobj.Decimal != null && testparameterobj.Decimal.Length > 0)
                                    {
                                        result = Roundoff.FormatDecimalValue(param[0], Convert.ToInt32(testparameterobj.Decimal));
                                    }
                                    else
                                    {
                                        result = param[0];
                                    }
                                }
                            }
                            else
                            {
                                result = param[0];
                            }
                            if (testparameterobj.RptLimit != null && Convert.ToDouble(result) < Convert.ToDouble(testparameterobj.RptLimit))
                            {
                                resultoutput = testparameterobj.DefaultResult + "|" + param[1] + "|";
                            }
                            else if (testparameterobj.RptLimit != null && Convert.ToDouble(result) >= Convert.ToDouble(testparameterobj.RptLimit))
                            {
                                if (((param[2] != "" && param[2] != "null") && (param[3] != "" && param[3] != "null")) && (Convert.ToDouble(result) < Convert.ToDouble(param[2]) || Convert.ToDouble(result) > Convert.ToDouble(param[3])))
                                {
                                    resultoutput = result + "|" + param[1] + "|redcell";
                                }
                                else if (Convert.ToDouble(result) == Convert.ToDouble(testparameterobj.RptLimit))
                                {
                                    resultoutput = result + "|" + param[1] + "|yellowcell";
                                }
                                else
                                {
                                    resultoutput = result + "|" + param[1] + "|";
                                }
                            }
                            else
                            {
                                resultoutput = result + "|" + param[1] + "|";
                            }
                            response.Add(resultoutput);
                        }
                    }
                    if (response.Count > 0)
                    {
                        e.Result = string.Join(";", response.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = "error";
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void RollbackResultEntryHistory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ListPropertyEditor lstviewSuboutSample = ((DetailView)Application.MainWindow.View).FindItem("SampleParameter") as ListPropertyEditor;
                ListPropertyEditor lstviewSuboutQCSample = ((DetailView)Application.MainWindow.View).FindItem("SubOutQcSample") as ListPropertyEditor;
                int selectedCount = 0;
                SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject;
                if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                {
                    if (lstviewSuboutSample.ListView.SelectedObjects.Count > 0)
                    {
                        selectedCount = lstviewSuboutSample.ListView.SelectedObjects.Count;
                    }
                    if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView != null)
                    {
                        if (lstviewSuboutQCSample.ListView.SelectedObjects.Count > 0)
                        {
                            selectedCount = lstviewSuboutQCSample.ListView.SelectedObjects.Count;
                        }
                    }

                }
                if (selectedCount == 0)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                else
                {

                    IObjectSpace os = Application.CreateObjectSpace(typeof(SubOutSampleRegistrations));
                    SubOutSampleRegistrations suboutObj = os.FindObject<SubOutSampleRegistrations>(CriteriaOperator.Parse("[Oid] = ?", objSubout.Oid));
                    DetailView createdView = Application.CreateDetailView(os, "SubOutSampleRegistrations_DetailView_RollBack", true, suboutObj);
                    createdView.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //showViewParameters.CreatedView.Caption = "RollBack";
                    showViewParameters.CreatedView.Caption = Application.MainWindow.View.Caption + " Rollback";
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += Rollback_Dc_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
                //if (View.SelectedObjects.Count > 0)
                //{
                //    foreach (SubOutSampleRegistrations objSubout in View.SelectedObjects)
                //    {
                //        foreach (SampleParameter objParam in objSubout.SampleParameter)
                //        {
                //            if (objParam.Status == Samplestatus.SuboutPendingApproval || objParam.Status == Samplestatus.SuboutPendingValidation || objParam.Status == Samplestatus.PendingEntry)
                //            {
                //                //objParam.IsExportedSuboutResult = false;
                //                objParam.Status = Samplestatus.PendingEntry;
                //                objSubout.Status = SuboutStatus.PendingResultEntry;
                //                objParam.AnalyzedDate = null;
                //                objParam.AnalyzedBy = null;
                //                objParam.ValidatedDate = null;
                //                objParam.ValidatedBy = null;
                //                objParam.ApprovedDate = null;
                //                objParam.ApprovedBy = null;
                //                objParam.MDL = null;
                //                objParam.UQL = null;
                //                objParam.LOQ = null;
                //                objParam.ResultNumeric = null;
                //                objParam.Result = null;
                //                objParam.SuboutAnalyzedBy = null;

                //            }
                //        }
                //        foreach (SuboutQcSample objQcParam in objSubout.SubOutQcSample)
                //        {
                //            if (objQcParam.Status == Samplestatus.SuboutPendingApproval || objQcParam.Status == Samplestatus.SuboutPendingValidation || objQcParam.Status == Samplestatus.PendingEntry)
                //            {
                //                //objParam.IsExportedSuboutResult = false;
                //                objQcParam.Status = Samplestatus.PendingEntry;
                //                objSubout.Status = SuboutStatus.PendingResultEntry;
                //                objQcParam.AnalyzedDate = null;
                //                objQcParam.AnalyzedBy = null;
                //                objQcParam.ValidatedDate = null;
                //                objQcParam.ValidatedBy = null;
                //                objQcParam.ApprovedDate = null;
                //                objQcParam.ApprovedBy = null;
                //                objQcParam.MDL = null;
                //                objQcParam.UQL = null;
                //                objQcParam.LOQ = null;
                //                objQcParam.NumericResult = null;
                //                objQcParam.Result = null;

                //            }
                //        }
                //        ObjectSpace.CommitChanges();
                //        View.ObjectSpace.Refresh();
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //        //ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                //        //if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                //        //{
                //        //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                //        //    {
                //        //        if (parent.Id == "SampleSubOutTracking")
                //        //        {
                //        //            foreach (ChoiceActionItem child in parent.Items)
                //        //            {
                //        //                if (child.Id == "SuboutSampleResultEntry")
                //        //                {
                //        //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                //        //                    IList<SampleParameter> objParam = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [IsExportedSuboutResult] = False  And [SuboutSignOff] = True And [Status] = 'PendingEntry'"));
                //        //                    if (objParam != null && objParam.Count > 0)
                //        //                    {
                //        //                        var count = objParam.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID != null).Select(i => i.Samplelogin.JobID.JobID).Distinct().Count();
                //        //                        var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                //        //                        if (count > 0)
                //        //                        {
                //        //                            child.Caption = cap[0] + " (" + count + ")";
                //        //                        }
                //        //                        else
                //        //                        {
                //        //                            child.Caption = cap[0];
                //        //                        }
                //        //                    }
                //        //                    break;
                //        //                }
                //        //            }
                //        //            break;
                //        //        }
                //        //    }
                //        //}
                //        //int objCount = objSubout.SampleParameter.Where(i => i.SubOut == true && i.IsExportedSuboutResult == false && i.Status==Samplestatus.PendingEntry).Select(i => i.Oid).Count();
                //        //if(objCount== objSubout.SampleParameter.Count)
                //        //{
                //        //    objSubout.Status = SuboutStatus.PendingResultEntry;
                //        //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //        //}

                //    }
                //}
                //else
                //{
                //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Rollback_Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                string strReason = ((SubOutSampleRegistrations)e.AcceptActionArgs.CurrentObject).RollBackReason;
                if (!string.IsNullOrEmpty(strReason) && !string.IsNullOrWhiteSpace(strReason))
                {
                    SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject;
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)Application.MainWindow.View).FindItem("SampleParameter") as ListPropertyEditor;
                    ListPropertyEditor lstviewSuboutQCSample = ((DetailView)Application.MainWindow.View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                    {
                        if (lstviewSuboutSample.ListView.SelectedObjects.Count > 0)
                        {
                            objSubout.Status = SuboutStatus.PendingResultEntry;
                            foreach (SampleParameter objParam in lstviewSuboutSample.ListView.SelectedObjects)
                            {
                                objParam.Status = Samplestatus.PendingEntry;
                                objParam.OSSync = true;
                                objParam.AnalyzedDate = null;
                                objParam.AnalyzedBy = null;
                                objParam.ValidatedDate = null;
                                objParam.ValidatedBy = null;
                                objParam.ApprovedDate = null;
                                objParam.ApprovedBy = null;
                                objParam.MDL = null;
                                objParam.UQL = null;
                                objParam.LOQ = null;
                                objParam.ResultNumeric = null;
                                objParam.Result = null;
                                objParam.SuboutAnalyzedBy = null;
                                objParam.Rollback = strReason;
                                objParam.IsExportedSuboutResult = false;
                                objParam.SuboutApprovedDate = null;
                                objParam.SuboutApprovedBy = null;
                                objParam.ApprovedDate = null;
                                objParam.ApprovedBy = null;
                                objParam.SuboutValidatedDate = null;
                                objParam.SuboutValidatedBy = null;
                            }
                            lstviewSuboutSample.ListView.ObjectSpace.CommitChanges();
                        }
                    }
                    if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView != null)
                    {
                        if (lstviewSuboutQCSample.ListView.SelectedObjects.Count > 0)
                        {
                            objSubout.Status = SuboutStatus.PendingResultEntry;
                            foreach (SuboutQcSample objQcParam in lstviewSuboutQCSample.ListView.SelectedObjects)
                            {
                                objQcParam.Status = Samplestatus.PendingEntry;
                                objQcParam.AnalyzedDate = null;
                                objQcParam.AnalyzedBy = null;
                                objQcParam.ValidatedDate = null;
                                objQcParam.ValidatedBy = null;
                                objQcParam.ApprovedDate = null;
                                objQcParam.ApprovedBy = null;
                                objQcParam.MDL = null;
                                objQcParam.UQL = null;
                                objQcParam.LOQ = null;
                                objQcParam.Surrogate = null;
                                objQcParam.Rec = null;
                                objQcParam.RecLCLimit = null;
                                objQcParam.RecUCLimit = null;
                                objQcParam.NumericResult = null;
                                objQcParam.Result = null;
                                objQcParam.Rollback = strReason;
                                objQcParam.SuboutValidatedDate = null;
                                objQcParam.SuboutValidatedBy = null;
                                objQcParam.SuboutApprovedDate = null;
                                objQcParam.SuboutApprovedBy = null;
                            }
                            lstviewSuboutQCSample.ListView.ObjectSpace.CommitChanges();
                        }
                    }
                    Application.MainWindow.View.ObjectSpace.CommitChanges();
                    ObjectSpace.Refresh();
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbacksuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "Rollbackempty"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SuboutResultApproval_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SubOutSampleRegistrations_DetailView_Level3DataReview")
                {
                    int selectedcount = 0;
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                    ListPropertyEditor lstviewSuboutQCSample = ((DetailView)View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstviewSuboutSample.ListView).Editor).Grid.UpdateEdit();
                        if (lstviewSuboutSample.ListView.SelectedObjects.Count > 0)
                        {
                            selectedcount = lstviewSuboutSample.ListView.SelectedObjects.Count;
                            foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                            {
                                objparam.Status = Samplestatus.PendingReporting;
                                objparam.OSSync = true;
                                objparam.IsExportedSuboutResult = true;
                                objparam.SuboutApprovedDate = DateTime.Now;
                                objparam.SuboutApprovedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                objparam.ApprovedDate = DateTime.Now;
                                objparam.ApprovedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                            }
                            lstviewSuboutSample.ListView.ObjectSpace.CommitChanges();
                        }
                    }
                    if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstviewSuboutQCSample.ListView).Editor).Grid.UpdateEdit();
                        if (lstviewSuboutQCSample.ListView.SelectedObjects.Count > 0)
                        {
                            selectedcount = lstviewSuboutQCSample.ListView.SelectedObjects.Count;
                            foreach (SuboutQcSample objparam in lstviewSuboutQCSample.ListView.SelectedObjects)
                            {
                                objparam.Status = Samplestatus.PendingReporting;
                                objparam.SuboutApprovedDate = DateTime.Now;
                                objparam.SuboutApprovedBy = lstviewSuboutQCSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                            }
                            lstviewSuboutQCSample.ListView.ObjectSpace.CommitChanges();

                        }
                    }
                    if (selectedcount == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "resultapprove"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        SubOutSampleRegistrations obj = (SubOutSampleRegistrations)View.CurrentObject;
                        if (obj != null && obj.SampleParameter != null)
                        {
                            int objCount = obj.SampleParameter.Where(i => i.SubOut == true && i.IsExportedSuboutResult == false && (i.Status == Samplestatus.PendingValidation || i.Status == Samplestatus.SuboutPendingValidation && i.Status == Samplestatus.PendingApproval)).Select(i => i.Oid).Count();
                            int objQCCount = obj.SubOutQcSample.Where(i => (i.Status == Samplestatus.PendingValidation || i.Status == Samplestatus.SuboutPendingValidation && i.Status == Samplestatus.PendingApproval)).Select(i => i.Oid).Count();
                            if (objCount == 0 && objQCCount == 0)
                            {
                                obj.Status = SuboutStatus.IsExported;
                                obj.SuboutStatus = SuboutTrackingStatus.SuboutResultsApproved;
                                View.ObjectSpace.CommitChanges();
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

        private void SuboutResultReview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SubOutSampleRegistrations_DetailView_Level2DataReview")
                {
                    int selectedcount = 0;
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                    ListPropertyEditor lstviewSuboutQCSample = ((DetailView)View).FindItem("SubOutQcSample") as ListPropertyEditor;
                    DefaultSetting objDefaultLevel3 = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='Level3SuboutDataReview'"));
                    if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstviewSuboutSample.ListView).Editor).Grid.UpdateEdit();
                        if (lstviewSuboutSample.ListView.SelectedObjects.Count > 0)
                        {
                            selectedcount = lstviewSuboutSample.ListView.SelectedObjects.Count;
                            if (objDefaultLevel3 != null && objDefaultLevel3.Select)
                            {
                                foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                                {
                                    objparam.Status = Samplestatus.SuboutPendingApproval;
                                    objparam.OSSync = true;
                                    objparam.ValidatedDate = DateTime.Now;
                                    objparam.ValidatedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    objparam.SuboutValidatedDate = DateTime.Now;
                                    objparam.SuboutValidatedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                }
                            }
                            else
                            {
                                foreach (SampleParameter objparam in lstviewSuboutSample.ListView.SelectedObjects)
                                {
                                    objparam.Status = Samplestatus.PendingReporting;
                                    objparam.OSSync = true;
                                    objparam.IsExportedSuboutResult = true;
                                    objparam.SuboutValidatedDate = DateTime.Now;
                                    objparam.SuboutValidatedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    objparam.SuboutApprovedDate = DateTime.Now;
                                    objparam.SuboutApprovedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    objparam.ValidatedDate = DateTime.Now;
                                    objparam.ValidatedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    objparam.ApprovedDate = DateTime.Now;
                                    objparam.ApprovedBy = lstviewSuboutSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                }
                            }
                            lstviewSuboutSample.ListView.ObjectSpace.CommitChanges();
                            SubOutSampleRegistrations objsubout = (SubOutSampleRegistrations)View.CurrentObject;
                            if (objsubout != null)
                            {
                                objsubout.RollBackBy = null;
                                objsubout.RollBackReason = null;
                                objsubout.RollBackDate = null;
                            }

                        }
                    }
                    if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstviewSuboutQCSample.ListView).Editor).Grid.UpdateEdit();
                        if (lstviewSuboutQCSample.ListView.SelectedObjects.Count > 0)
                        {
                            selectedcount = lstviewSuboutQCSample.ListView.SelectedObjects.Count;
                            if (objDefaultLevel3 != null && objDefaultLevel3.Select)
                            {
                                foreach (SuboutQcSample objparam in lstviewSuboutQCSample.ListView.SelectedObjects)
                                {
                                    objparam.Status = Samplestatus.SuboutPendingApproval;
                                    objparam.SuboutValidatedDate = DateTime.Now;
                                    objparam.SuboutValidatedBy = lstviewSuboutQCSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                }
                            }
                            else
                            {
                                foreach (SuboutQcSample objparam in lstviewSuboutQCSample.ListView.SelectedObjects)
                                {
                                    objparam.Status = Samplestatus.PendingReporting;
                                    objparam.SuboutValidatedDate = DateTime.Now;
                                    objparam.SuboutValidatedBy = lstviewSuboutQCSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                    objparam.SuboutApprovedDate = DateTime.Now;
                                    objparam.SuboutApprovedBy = lstviewSuboutQCSample.ListView.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                                }
                            }
                            lstviewSuboutQCSample.ListView.ObjectSpace.CommitChanges();

                        }
                    }
                    if (selectedcount == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "ResultsReviewed"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        SubOutSampleRegistrations obj = (SubOutSampleRegistrations)View.CurrentObject;
                        if (obj != null && obj.SampleParameter != null)
                        {
                            int objCount = obj.SampleParameter.Where(i => i.SubOut == true && i.IsExportedSuboutResult == false && (i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation)).Select(i => i.Oid).Count();
                            int objQcCount = obj.SubOutQcSample.Where(i => (i.Status == Samplestatus.PendingEntry || i.Status == Samplestatus.SuboutPendingValidation)).Select(i => i.Oid).Count();
                            if (objCount == 0 && objQcCount == 0)
                            {
                                if (objDefaultLevel3 != null && objDefaultLevel3.Select)
                                {
                                obj.Status = SuboutStatus.SuboutPendingApproval;
                                }
                                else
                                {
                                    obj.Status = SuboutStatus.IsExported;
                                obj.SuboutStatus = SuboutTrackingStatus.SuboutResultsValidated;
                                }
                                View.ObjectSpace.CommitChanges();
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
        private void SigningOff_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SubOutSampleRegistrations_DetailView_PendingSignOff")
                {
                    ObjectSpace.CommitChanges();
                    ListPropertyEditor lstviewSuboutSample = ((DetailView)View).FindItem("SampleParameter") as ListPropertyEditor;
                    if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
                    {
                        ((ASPxGridListEditor)((ListView)lstviewSuboutSample.ListView).Editor).Grid.UpdateEdit();
                    }
                    string strJobID = string.Empty;
                    string strQcType = string.Empty;
                    string strSampleId = string.Empty;
                    string Method = string.Empty;
                    string strMatrix = string.Empty;
                    string strSamplename = string.Empty;
                    string strTest = string.Empty;
                    string strParameter = string.Empty;
                    SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                    if (objSubout != null)
                    {
                        //string strLocalFile = HttpContext.Current.Server.MapPath(@"~\SuboutResultEntryEDD.xls");
                        //if (File.Exists(strLocalFile) == true)
                        //{
                        //    DataTable dtsht1 = new DataTable();
                        //    DevExpress.Spreadsheet.Workbook workbookOld = new DevExpress.Spreadsheet.Workbook();
                        //    DevExpress.Spreadsheet.Workbook workbookNew = new DevExpress.Spreadsheet.Workbook();
                        //    Worksheet worksheet0 = workbookNew.Worksheets[0];
                        //    workbookOld.LoadDocument(strLocalFile);
                        //    DevExpress.Spreadsheet.Worksheet worksheet = workbookOld.Worksheets[0];
                        //    CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                        //    DataTable dt = worksheet.CreateDataTable(range, true);
                        //    for (int col = 0; col < range.ColumnCount; col++)
                        //    {
                        //        CellValueType cellType = range[0, col].Value.Type;
                        //        for (int r = 1; r < range.RowCount; r++)
                        //        {
                        //            if (cellType != range[r, col].Value.Type)
                        //            {
                        //                dt.Columns[col].DataType = typeof(string);
                        //                break;
                        //            }
                        //        }
                        //    }

                        //    DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                        //    exporter.Export();
                        //    if (dt != null && dt.Rows.Count > 0)
                        //    {
                        //        DataRow row1 = dt.Rows[0];
                        //        if (row1[0].ToString() == dt.Columns[0].Caption)
                        //        {
                        //            row1.Delete();
                        //            dt.AcceptChanges();
                        //        }
                        //        foreach (DataColumn c in dt.Columns)
                        //            c.ColumnName = c.ColumnName.ToString().Trim();
                        //    }

                        //    IList<SampleParameter> listSample = lstviewSuboutSample.ListView.CollectionSource.List.Cast<SampleParameter>().ToList();
                        //    if (listSample!=null && listSample.Count>0)
                        //    {
                        //        foreach (SampleParameter obj in listSample)
                        //        {
                        //            if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                        //            {
                        //                strSampleId = obj.Samplelogin.SampleID;
                        //            }
                        //            if (obj.Samplelogin != null && obj.Samplelogin.JobID != null)
                        //            {
                        //                strJobID = obj.Samplelogin.JobID.JobID;
                        //            }
                        //            if (obj.Samplelogin != null && obj.Samplelogin.ClientSampleID != null)
                        //            {
                        //                strSamplename = obj.Samplelogin.ClientSampleID;
                        //            }
                        //            if (obj.Samplelogin != null && obj.Samplelogin.VisualMatrix != null)
                        //            {
                        //                strMatrix = obj.Samplelogin.VisualMatrix.MatrixName.MatrixName;
                        //            }
                        //            if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                        //            {
                        //                strTest = obj.Testparameter.TestMethod.TestName;
                        //            }
                        //            if (obj.Testparameter != null && obj.Testparameter.TestMethod!=null && obj.Testparameter.TestMethod.MethodName != null)
                        //            {
                        //                Method = obj.Testparameter.TestMethod.MethodName.MethodNumber;
                        //            }
                        //            if (obj.Testparameter != null && obj.Testparameter.Parameter != null)
                        //            {
                        //                strParameter = obj.Testparameter.Parameter.ParameterName;
                        //            }
                        //            if (obj.Testparameter != null && obj.Samplelogin.SampleID != null)
                        //            {
                        //                strParameter = obj.Testparameter.Parameter.ParameterName;
                        //            }

                        //            dt.Rows.Add(strJobID, strQctype, strSampleId, strSamplename, strMatrix, Method, strTest, strParameter);

                        //        } 
                        //    }
                        //    worksheet0.Import(dt, true, 0, 0);
                        //    //System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        //    //System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        //    //formatter.Serialize(stream, dt); // dtUsers is a DataTable
                        //    //byte[] bytes = stream.GetBuffer();
                        //    //workbookNew.LoadDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);//stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                        //    //Stream excelStream = File.Create(Path.GetFullPath("D:\\Output.xlsx"));
                        //    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\EDDTemplate")) == false)
                        //    {
                        //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\EDDTemplate"));
                        //    }
                        //    string strFilePath = HttpContext.Current.Server.MapPath(@"~\EDDTemplate");
                        //    Stream excelStream = File.Create(Path.GetFullPath(strFilePath + "\\SuboutResultEntryEDD.xlsx"));
                        //    workbookNew.SaveDocument(excelStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                        //    excelStream.Dispose();
                        //    //workbookNew.SaveDocument(excelStream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                        //    //int str = stream.Read(bytes, 0, bytes.Length);
                        //    //using (MemoryStream newms = new MemoryStream())
                        //    //{
                        //    //    newms.Read(bytes, 0, bytes.Length);
                        //    //}

                        //    //string line = stream.Read();
                        //    using (FileStream fs = File.OpenRead(strFilePath+"\\SuboutResultEntryEDD.xlsx"))
                        //    {
                        //        if (objSubout.ResultImportEDD == null)
                        //        {
                        //            objSubout.ResultImportEDD = View.ObjectSpace.CreateObject<FileData>();
                        //        }
                        //        objSubout.ResultImportEDD.LoadFromStream(strFilePath+"\\SuboutResultEntryEDD.xlsx", fs);
                        //        View.ObjectSpace.CommitChanges();
                        //    }
                        //        //}
                        //        //using (FileStream fs = File.OpenWrite(strLocalFile))
                        //        //{
                        //        //    objSubout.ResultImportEDD.SaveToStream(fs);
                        //        //}

                        //        //stream.Read(bytes, 0, bytes.Length);

                        //        //IObjectSpace os = Application.CreateObjectSpace();
                        //        //FileData objFile = os.CreateObject<FileData>();
                        //        //objFile.LoadFromStream(objSubout.SuboutOrderID + "_EDD.xls", stream);
                        //        //objFile.FileName = objSubout.SuboutOrderID + "_EDD.xls";
                        //        //os.CommitChanges();
                        //        //objSubout.ResultImportEDD = View.ObjectSpace.GetObject<FileData>(objFile);
                        //        //ObjectSpace.CommitChanges();
                        //        // Flush the workbook to the Response.OutputStream
                        //    }
                        objSubout.Status = SuboutStatus.PendingResultEntry;
                        objSubout.RollBackBy = null;
                        objSubout.RollBackReason = null;
                        objSubout.RollBackDate = null;
                        foreach (SampleParameter objParam in objSubout.SampleParameter)
                        {
                            objParam.SuboutSignOff = true;
                            objParam.SuboutSignOffDate = DateTime.Now;
                            objParam.SuboutSignOffBy = ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                        }
                        ObjectSpace.CommitChanges();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "SignOff"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        if (objSubout.SuboutNotificationStatus == SuboutNotificationQueueStatus.Waiting)
                        {
                            SendNotificationEmail(objSubout);
                            if (boolMailSend)
                            {
                                objSubout.SuboutNotificationStatus = SuboutNotificationQueueStatus.Send;
                                boolMailSend = false;
                            }
                        }
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(SubOutSampleRegistrations));
                        e.ShowViewParameters.CreatedView = Application.CreateListView("SubOutSampleRegistrations_ListView_PendingSignOff", cs, true);
                        //ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                        //if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                        //{
                        //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                        //    {
                        //        if (parent.Id == "SampleSubOutTracking")
                        //        {
                        //            foreach (ChoiceActionItem child in parent.Items)
                        //            {
                        //                if (child.Id == "SuboutSampleResultEntry")
                        //                {
                        //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        //                    IList<SampleParameter> objParam = objectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[SubOut] = True And [IsExportedSuboutResult] = False  And [SuboutSignOff] = True And [Status] = 'PendingEntry'"));
                        //                    if (objParam != null && objParam.Count > 0)
                        //                    {
                        //                        //var count = objParam.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.JobID != null).Select(i => i.Samplelogin.JobID.JobID).Distinct().Count();
                        //                        var count = objParam.Where(i => i.SuboutSample != null && i.SuboutSample.SuboutOrderID != null).Select(i => i.SuboutSample.SuboutOrderID).Distinct().Count();

                        //                        if (count > 0)
                        //                        {
                        //                            child.Caption = cap[0] + " (" + count + ")";
                        //                        }
                        //                        else
                        //                        {
                        //                            child.Caption = cap[0];
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        child.Caption = cap[0];
                        //                    }

                        //                }
                        //                else if (child.Id == "SuboutRegistrationSigningOff")
                        //                {
                        //                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                        //                    var count = objectSpace.GetObjectsCount(typeof(SubOutSampleRegistrations), CriteriaOperator.Parse("[Status] = 'PendingSigningOff'"));
                        //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                        //                    if (count > 0)
                        //                    {
                        //                        child.Caption = cap[0] + " (" + count + ")";
                        //                    }
                        //                    else
                        //                    {
                        //                        child.Caption = cap[0];
                        //                    }
                        //                }
                        //            }
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void NotificationQueueDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedChoiceActionItem.Id == "1M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "3M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 3 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "6M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 6 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "1Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "2Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 2 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "5Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 5 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "ALL")
                {
                    ((ListView)View).CollectionSource.Criteria.Remove("DateFilter1");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void historyofSuboutSigningOff_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                CollectionSource cs = new CollectionSource(os, typeof(SubOutSampleRegistrations));
                ListView listview = Application.CreateListView("SubOutSampleRegistrations_ListView_SigningOffHistory", cs, true);
                Frame.SetView(listview);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void btnCOC_BarReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "SubOutSampleRegistrations_DetailView_Copy" || View.Id == "SubOutSampleRegistrations_ListView"
                     || View.Id == "SubOutSampleRegistrations_DetailView_SuboutOrderTracking" || View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue"
                     || View.Id == "SubOutSampleRegistrations_DetailView_TestOrder" || View.Id == "SubOutSampleRegistrations_ListView_TestOrder" || View.Id == "SubOutSampleRegistrations_ListView_SuboutRegHistory")
                {
                    if (View is DetailView)
                    {
                        boolReportPreview = true;
                        ReportGeneratePdf();
                    }
                    else
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            string strSuboutID = string.Empty;
                            List<string> listSuboutID = new List<string>();
                            foreach (SubOutSampleRegistrations obj in View.SelectedObjects)
                            {
                                listSuboutID.Add(obj.SuboutOrderID);
                                if (string.IsNullOrEmpty(strSuboutID))
                                {
                                    strSuboutID = "'" + obj.SuboutOrderID + "'";
                                }
                                else
                                {
                                    strSuboutID = strSuboutID + ",'" + obj.SuboutOrderID + "'";
                                }
                            }
                            if (listSuboutID != null && listSuboutID.Count > 0)
                            {
                                string strTempPath = Path.GetTempPath();
                                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SuboutCOCReport\")) == false)
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SuboutCOCReport\"));
                                }
                                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\SuboutCOCReport\" + timeStamp + ".pdf");
                                XtraReport xtraReport = new XtraReport();

                                objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                SetConnectionString();

                                DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);

                                if (listSuboutID.Count > 1)
                                {
                                    foreach (string strCOC_ID in listSuboutID)
                                    {
                                        XtraReport tempxtraReport = new XtraReport();
                                        ObjReportingInfo.strSuboutOrderID = "'" + strCOC_ID + "'"; ;
                                        tempxtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Subout_COC_Report", ObjReportingInfo, false);
                                        tempxtraReport.CreateDocument();
                                        xtraReport.Pages.AddRange(tempxtraReport.Pages);
                                    }

                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        xtraReport.ExportToPdf(ms);
                                        using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                                        {
                                            source.LoadDocument(ms);
                                            source.SaveDocument(strExportedPath);
                                        }
                                    }
                                }
                                else
                                {
                                    ObjReportingInfo.strSuboutOrderID = strSuboutID;
                                    xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Subout_COC_Report", ObjReportingInfo, false);
                                }

                                xtraReport.ExportToPdf(strExportedPath);
                                string[] path = strExportedPath.Split('\\');
                                int arrcount = path.Count();
                                int sc = arrcount - 3;
                                string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                                WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
        private void SuboutEDDTempalte_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string strJobID = string.Empty;
                string strQcType = string.Empty;
                string strSampleId = string.Empty;
                string Method = string.Empty;
                string strMatrix = string.Empty;
                string strSamplename = string.Empty;
                string strTest = string.Empty;
                string strParameter = string.Empty;

                SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                if (objSubout != null)
                {
                    string strTemplateDirectory = HttpContext.Current.Server.MapPath(@"~\EDDTemplate");
                    if (!Directory.Exists(strTemplateDirectory))
                    {
                        Directory.CreateDirectory(strTemplateDirectory);
                    }

                    string strFilePath = Path.Combine(strTemplateDirectory, "SuboutResultEntryEDD.xlsx");
                    if (File.Exists(strFilePath))
                    {
                        DevExpress.Spreadsheet.Workbook workbookOld = new DevExpress.Spreadsheet.Workbook();
                        workbookOld.LoadDocument(strFilePath);
                        DevExpress.Spreadsheet.Worksheet worksheet = workbookOld.Worksheets[0];
                        CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);

                        DataTable dt = worksheet.CreateDataTable(range, true);
                        for (int col = 0; col < range.ColumnCount; col++)
                        {
                            CellValueType cellType = range[0, col].Value.Type;
                            for (int r = 1; r < range.RowCount; r++)
                            {
                                if (cellType != range[r, col].Value.Type)
                                {
                                    dt.Columns[col].DataType = typeof(string);
                                    break;
                                }
                            }
                        }

                        DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                        exporter.Export();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row1 = dt.Rows[0];
                            if (row1[0].ToString() == dt.Columns[0].Caption)
                            {
                                row1.Delete();
                                dt.AcceptChanges();
                            }
                            foreach (DataColumn c in dt.Columns)
                            {
                                c.ColumnName = c.ColumnName.ToString().Trim();
                            }
                        }

                        // Clear the DataTable to avoid duplicates
                        dt.Rows.Clear();
                        dt.AcceptChanges();

                        foreach (SampleParameter obj in objSubout.SampleParameter)
                        {
                            if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                            {
                                strSampleId = obj.Samplelogin.SampleID;
                            }
                            if (obj.Samplelogin != null && obj.Samplelogin.JobID != null)
                            {
                                strJobID = obj.Samplelogin.JobID.JobID;
                            }
                            if (obj.Samplelogin != null && obj.Samplelogin.ClientSampleID != null)
                            {
                                strSamplename = obj.Samplelogin.ClientSampleID;
                            }
                            if (obj.Samplelogin != null && obj.Samplelogin.VisualMatrix != null)
                            {
                                strMatrix = obj.Samplelogin.VisualMatrix.MatrixName.MatrixName;
                            }
                            if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                            {
                                strTest = obj.Testparameter.TestMethod.TestName;
                            }
                            if (obj.Testparameter != null && obj.Testparameter.TestMethod != null && obj.Testparameter.TestMethod.MethodName != null)
                            {
                                Method = obj.Testparameter.TestMethod.MethodName.MethodNumber;
                            }
                            if (obj.Testparameter != null && obj.Testparameter.Parameter != null)
                            {
                                strParameter = obj.Testparameter.Parameter.ParameterName;
                            }
                            if (obj.Testparameter != null && obj.Samplelogin.SampleID != null)
                            {
                                strParameter = obj.Testparameter.Parameter.ParameterName;
                            }

                            dt.Rows.Add(strJobID, strQcType, strSampleId, strSamplename, strMatrix, Method, strTest, strParameter);
                        }

                        DevExpress.Spreadsheet.Workbook workbookNew = new DevExpress.Spreadsheet.Workbook();
                        DevExpress.Spreadsheet.Worksheet worksheet0 = workbookNew.Worksheets[0];
                        worksheet0.Import(dt, true, 0, 0);

                        using (Stream excelStream = File.Create(strFilePath))
                        {
                            workbookNew.SaveDocument(excelStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                        }

                        string relativeFilePath = "~/EDDTemplate/SuboutResultEntryEDD.xlsx";
                        string fileUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + VirtualPathUtility.ToAbsolute(relativeFilePath);

                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", $"window.open('{fileUrl}', '_blank');");
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        //private DataTable CreateDataTableFromExcel(string filePath)
        //{
        //    DevExpress.Spreadsheet.Workbook workbookOld = new DevExpress.Spreadsheet.Workbook();
        //    workbookOld.LoadDocument(filePath);
        //    DevExpress.Spreadsheet.Worksheet worksheet = workbookOld.Worksheets[0];
        //    DevExpress.Spreadsheet.CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);

        //    DataTable dt = worksheet.CreateDataTable(range, true);
        //    for (int col = 0; col < range.ColumnCount; col++)
        //    {
        //        DevExpress.Spreadsheet.CellValueType cellType = range[0, col].Value.Type;
        //        for (int r = 1; r < range.RowCount; r++)
        //        {
        //            if (cellType != range[r, col].Value.Type)
        //            {
        //                dt.Columns[col].DataType = typeof(string);
        //                break;
        //            }
        //        }
        //    }

        //    DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
        //    exporter.Export();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        DataRow row1 = dt.Rows[0];
        //        if (row1[0].ToString() == dt.Columns[0].Caption)
        //        {
        //            row1.Delete();
        //            dt.AcceptChanges();
        //        }
        //        foreach (DataColumn c in dt.Columns)
        //        {
        //            c.ColumnName = c.ColumnName.ToString().Trim();
        //        }
        //    }

        //    return dt;
        //}

        //private void PopulateDataTable(DataTable dt, SubOutSampleRegistrations objSubout)
        //{
        //    foreach (SampleParameter obj in objSubout.SampleParameter)
        //    {
        //        string strSampleId = obj.Samplelogin?.SampleID;
        //        string strJobID = obj.Samplelogin?.JobID?.JobID;
        //        string strSamplename = obj.Samplelogin?.ClientSampleID;
        //        string strMatrix = obj.Samplelogin?.VisualMatrix?.MatrixName?.MatrixName;
        //        string Method = obj.Testparameter?.TestMethod?.MethodName?.MethodNumber;
        //        string strTest = obj.Testparameter?.TestMethod?.TestName;
        //        string strParameter = obj.Testparameter?.Parameter?.ParameterName;

        //        dt.Rows.Add(strJobID, string.Empty, strSampleId, strSamplename, strMatrix, Method, strTest, strParameter);
        //    }
        //}

        private void ResendMail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //SubOutSampleRegistrations objSubotSample = (SubOutSampleRegistrations)View.CurrentObject;
                //if(objSubotSample!=null)
                //{
                //    SendNotificationEmail(objSubotSample);
                //    if(boolMailSend)
                //    {
                //        objSubotSample.SuboutNotificationStatus = SuboutNotificationQueueStatus.Send;
                //        View.ObjectSpace.CommitChanges();
                //        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                //        boolMailSend = false;
                //        View.ObjectSpace.Refresh();
                //    }

                //}
                string strJobID = string.Empty;
                string strQcType = string.Empty;
                string strSampleId = string.Empty;
                string Method = string.Empty;
                string strMatrix = string.Empty;
                string strSamplename = string.Empty;
                string strTest = string.Empty;
                string strParameter = string.Empty;
                SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                if (objSubout != null /*&& objSubout.COCAttached*/)
                {
                    string strLocalFile = HttpContext.Current.Server.MapPath(@"~\SuboutResultEntryEDD.xls");
                    if (File.Exists(strLocalFile) == true)
                    {
                        DataTable dtsht1 = new DataTable();
                        DevExpress.Spreadsheet.Workbook workbookOld = new DevExpress.Spreadsheet.Workbook();
                        DevExpress.Spreadsheet.Workbook workbookNew = new DevExpress.Spreadsheet.Workbook();
                        Worksheet worksheet0 = workbookNew.Worksheets[0];
                        workbookOld.LoadDocument(strLocalFile);
                        DevExpress.Spreadsheet.Worksheet worksheet = workbookOld.Worksheets[0];
                        CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                        DataTable dt = worksheet.CreateDataTable(range, true);
                        for (int col = 0; col < range.ColumnCount; col++)
                        {
                            CellValueType cellType = range[0, col].Value.Type;
                            for (int r = 1; r < range.RowCount; r++)
                            {
                                if (cellType != range[r, col].Value.Type)
                                {
                                    dt.Columns[col].DataType = typeof(string);
                                    break;
                                }
                            }
                        }

                        DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                        exporter.Export();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row1 = dt.Rows[0];
                            if (row1[0].ToString() == dt.Columns[0].Caption)
                            {
                                row1.Delete();
                                dt.AcceptChanges();
                            }
                            foreach (DataColumn c in dt.Columns)
                                c.ColumnName = c.ColumnName.ToString().Trim();
                        }
                        foreach (SampleParameter obj in objSubout.SampleParameter)
                        {
                            if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                            {
                                strSampleId = obj.Samplelogin.SampleID;
                            }
                            if (obj.Samplelogin != null && obj.Samplelogin.JobID != null)
                            {
                                strJobID = obj.Samplelogin.JobID.JobID;
                            }
                            if (obj.Samplelogin != null && obj.Samplelogin.ClientSampleID != null)
                            {
                                strSamplename = obj.Samplelogin.ClientSampleID;
                            }
                            if (obj.Samplelogin != null && obj.Samplelogin.VisualMatrix != null)
                            {
                                strMatrix = obj.Samplelogin.VisualMatrix.MatrixName.MatrixName;
                            }
                            if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                            {
                                strTest = obj.Testparameter.TestMethod.TestName;
                            }
                            if (obj.Testparameter != null && obj.Testparameter.TestMethod != null && obj.Testparameter.TestMethod.MethodName != null)
                            {
                                Method = obj.Testparameter.TestMethod.MethodName.MethodNumber;
                            }
                            if (obj.Testparameter != null && obj.Testparameter.Parameter != null)
                            {
                                strParameter = obj.Testparameter.Parameter.ParameterName;
                            }
                            if (obj.Testparameter != null && obj.Samplelogin.SampleID != null)
                            {
                                strParameter = obj.Testparameter.Parameter.ParameterName;
                            }

                            dt.Rows.Add(strJobID, strQctype, strSampleId, strSamplename, strMatrix, Method, strTest, strParameter);

                        }
                        worksheet0.Import(dt, true, 0, 0);
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\EDDTemplate")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\EDDTemplate"));
                        }
                        string strFilePath = HttpContext.Current.Server.MapPath(@"~\EDDTemplate");
                        Stream excelStream = File.Create(Path.GetFullPath(strFilePath + "\\SuboutResultEntryEDD.xlsx"));
                        workbookNew.SaveDocument(excelStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                        excelStream.Dispose();

                        using (FileStream fs = File.OpenRead(strFilePath + "\\SuboutResultEntryEDD.xlsx"))
                        {
                            if (objSubout.ResultImportEDD == null)
                            {
                                objSubout.ResultImportEDD = View.ObjectSpace.CreateObject<FileData>();
                            }
                            objSubout.ResultImportEDD.LoadFromStream(strFilePath + "\\SuboutResultEntryEDD.xlsx", fs);
                            View.ObjectSpace.CommitChanges();
                        }

                    }
                    SendNotificationEmail(objSubout);
                    if (boolMailSend)
                    {
                        objSubout.SuboutNotificationStatus = SuboutNotificationQueueStatus.Send;
                        boolMailSend = false;
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                    }
                    ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                    //ShowNavigationItemController ShowNavigationController = Application.MainWindow.GetController<ShowNavigationItemController>();
                    //if (ShowNavigationController != null && ShowNavigationController.ShowNavigationItemAction != null)
                    //{
                    //    foreach (ChoiceActionItem parent in ShowNavigationController.ShowNavigationItemAction.Items)
                    //    {
                    //        if (parent.Id == "SampleSubOutTracking")
                    //        {
                    //            foreach (ChoiceActionItem child in parent.Items)
                    //            {
                    //                if (child.Id == "SuboutNotificationQueue")
                    //                {
                    //                    IObjectSpace objectSpace = Application.CreateObjectSpace();

                    //                    var count = objectSpace.GetObjectsCount(typeof(SubOutSampleRegistrations), CriteriaOperator.Parse("[SuboutNotificationStatus] = 'Waiting'"));
                    //                    var cap = child.Caption.Split(new string[] { " (" }, StringSplitOptions.None);
                    //                    if (count > 0)
                    //                    {
                    //                        child.Caption = cap[0] + " (" + count + ")";
                    //                    }
                    //                    else
                    //                    {
                    //                        child.Caption = cap[0];
                    //                    }
                    //                    break;
                    //                }
                    //            }
                    //            break;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Please select COC Attached.", InformationType.Error, timer.Seconds, InformationPosition.Top);

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Submit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View is ListView)
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        string strJobID = string.Empty;
                        string strQcType = string.Empty;
                        string strSampleId = string.Empty;
                        string Method = string.Empty;
                        string strMatrix = string.Empty;
                        string strSamplename = string.Empty;
                        string strTest = string.Empty;
                        string strParameter = string.Empty;
                        SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                        if (objSubout != null)
                        {
                            string strLocalFile = HttpContext.Current.Server.MapPath(@"~\SuboutResultEntryEDD.xls");
                            if (File.Exists(strLocalFile) == true)
                            {
                                DataTable dtsht1 = new DataTable();
                                DevExpress.Spreadsheet.Workbook workbookOld = new DevExpress.Spreadsheet.Workbook();
                                DevExpress.Spreadsheet.Workbook workbookNew = new DevExpress.Spreadsheet.Workbook();
                                Worksheet worksheet0 = workbookNew.Worksheets[0];
                                workbookOld.LoadDocument(strLocalFile);
                                DevExpress.Spreadsheet.Worksheet worksheet = workbookOld.Worksheets[0];
                                CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                                DataTable dt = worksheet.CreateDataTable(range, true);
                                for (int col = 0; col < range.ColumnCount; col++)
                                {
                                    CellValueType cellType = range[0, col].Value.Type;
                                    for (int r = 1; r < range.RowCount; r++)
                                    {
                                        if (cellType != range[r, col].Value.Type)
                                        {
                                            dt.Columns[col].DataType = typeof(string);
                                            break;
                                        }
                                    }
                                }

                                DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                                exporter.Export();
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    DataRow row1 = dt.Rows[0];
                                    if (row1[0].ToString() == dt.Columns[0].Caption)
                                    {
                                        row1.Delete();
                                        dt.AcceptChanges();
                                    }
                                    foreach (DataColumn c in dt.Columns)
                                        c.ColumnName = c.ColumnName.ToString().Trim();
                                }
                                foreach (SampleParameter obj in objSubout.SampleParameter)
                                {
                                    if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                                    {
                                        strSampleId = obj.Samplelogin.SampleID;
                                    }
                                    if (obj.Samplelogin != null && obj.Samplelogin.JobID != null)
                                    {
                                        strJobID = obj.Samplelogin.JobID.JobID;
                                    }
                                    if (obj.Samplelogin != null && obj.Samplelogin.ClientSampleID != null)
                                    {
                                        strSamplename = obj.Samplelogin.ClientSampleID;
                                    }
                                    if (obj.Samplelogin != null && obj.Samplelogin.VisualMatrix != null)
                                    {
                                        strMatrix = obj.Samplelogin.VisualMatrix.MatrixName.MatrixName;
                                    }
                                    if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                                    {
                                        strTest = obj.Testparameter.TestMethod.TestName;
                                    }
                                    if (obj.Testparameter != null && obj.Testparameter.TestMethod != null && obj.Testparameter.TestMethod.MethodName != null)
                                    {
                                        Method = obj.Testparameter.TestMethod.MethodName.MethodNumber;
                                    }
                                    if (obj.Testparameter != null && obj.Testparameter.Parameter != null)
                                    {
                                        strParameter = obj.Testparameter.Parameter.ParameterName;
                                    }
                                    if (obj.Testparameter != null && obj.Samplelogin.SampleID != null)
                                    {
                                        strParameter = obj.Testparameter.Parameter.ParameterName;
                                    }

                                    dt.Rows.Add(strJobID, strQctype, strSampleId, strSamplename, strMatrix, Method, strTest, strParameter);

                                }
                                worksheet0.Import(dt, true, 0, 0);
                                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\EDDTemplate")) == false)
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\EDDTemplate"));
                                }
                                string strFilePath = HttpContext.Current.Server.MapPath(@"~\EDDTemplate");
                                Stream excelStream = File.Create(Path.GetFullPath(strFilePath + "\\SuboutResultEntryEDD.xlsx"));
                                workbookNew.SaveDocument(excelStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                                excelStream.Dispose();

                                using (FileStream fs = File.OpenRead(strFilePath + "\\SuboutResultEntryEDD.xlsx"))
                                {
                                    if (objSubout.ResultImportEDD == null)
                                    {
                                        objSubout.ResultImportEDD = View.ObjectSpace.CreateObject<FileData>();
                                    }
                                    objSubout.ResultImportEDD.LoadFromStream(strFilePath + "\\SuboutResultEntryEDD.xlsx", fs);
                                    View.ObjectSpace.CommitChanges();
                                }

                            }
                            SendNotificationEmail(objSubout);
                            if (boolMailSend)
                            {
                                objSubout.SuboutNotificationStatus = SuboutNotificationQueueStatus.Send;
                                boolMailSend = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                            }

                            foreach (SubOutSampleRegistrations objSubout1 in View.SelectedObjects)
                            {
                                objSubout1.SuboutStatus = SuboutTrackingStatus.SuboutSubmitted;
                            }
                            ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();



                            //View.ObjectSpace.CommitChanges();
                            //View.ObjectSpace.Refresh();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
                else
                {
                    if (View.CurrentObject != null)
                    {

                        string strJobID = string.Empty;
                        string strQcType = string.Empty;
                        string strSampleId = string.Empty;
                        string Method = string.Empty;
                        string strMatrix = string.Empty;
                        string strSamplename = string.Empty;
                        string strTest = string.Empty;
                        string strParameter = string.Empty;
                        SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)View.CurrentObject;
                        if (objSubout != null)
                        {
                            string strLocalFile = HttpContext.Current.Server.MapPath(@"~\SuboutResultEntryEDD.xls");
                            if (File.Exists(strLocalFile) == true)
                            {
                                DataTable dtsht1 = new DataTable();
                                DevExpress.Spreadsheet.Workbook workbookOld = new DevExpress.Spreadsheet.Workbook();
                                DevExpress.Spreadsheet.Workbook workbookNew = new DevExpress.Spreadsheet.Workbook();
                                Worksheet worksheet0 = workbookNew.Worksheets[0];
                                workbookOld.LoadDocument(strLocalFile);
                                DevExpress.Spreadsheet.Worksheet worksheet = workbookOld.Worksheets[0];
                                CellRange range = worksheet.Range.FromLTRB(0, 0, worksheet.Columns.LastUsedIndex, worksheet.GetUsedRange().BottomRowIndex);
                                DataTable dt = worksheet.CreateDataTable(range, true);
                                for (int col = 0; col < range.ColumnCount; col++)
                                {
                                    CellValueType cellType = range[0, col].Value.Type;
                                    for (int r = 1; r < range.RowCount; r++)
                                    {
                                        if (cellType != range[r, col].Value.Type)
                                        {
                                            dt.Columns[col].DataType = typeof(string);
                                            break;
                                        }
                                    }
                                }

                                DevExpress.Spreadsheet.Export.DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dt, false);
                                exporter.Export();
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    DataRow row1 = dt.Rows[0];
                                    if (row1[0].ToString() == dt.Columns[0].Caption)
                                    {
                                        row1.Delete();
                                        dt.AcceptChanges();
                                    }
                                    foreach (DataColumn c in dt.Columns)
                                        c.ColumnName = c.ColumnName.ToString().Trim();
                                }
                                foreach (SampleParameter obj in objSubout.SampleParameter)
                                {
                                    if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                                    {
                                        strSampleId = obj.Samplelogin.SampleID;
                                    }
                                    if (obj.Samplelogin != null && obj.Samplelogin.JobID != null)
                                    {
                                        strJobID = obj.Samplelogin.JobID.JobID;
                                    }
                                    if (obj.Samplelogin != null && obj.Samplelogin.ClientSampleID != null)
                                    {
                                        strSamplename = obj.Samplelogin.ClientSampleID;
                                    }
                                    if (obj.Samplelogin != null && obj.Samplelogin.VisualMatrix != null)
                                    {
                                        strMatrix = obj.Samplelogin.VisualMatrix.MatrixName.MatrixName;
                                    }
                                    if (obj.Samplelogin != null && obj.Samplelogin.SampleID != null)
                                    {
                                        strTest = obj.Testparameter.TestMethod.TestName;
                                    }
                                    if (obj.Testparameter != null && obj.Testparameter.TestMethod != null && obj.Testparameter.TestMethod.MethodName != null)
                                    {
                                        Method = obj.Testparameter.TestMethod.MethodName.MethodNumber;
                                    }
                                    if (obj.Testparameter != null && obj.Testparameter.Parameter != null)
                                    {
                                        strParameter = obj.Testparameter.Parameter.ParameterName;
                                    }
                                    if (obj.Testparameter != null && obj.Samplelogin.SampleID != null)
                                    {
                                        strParameter = obj.Testparameter.Parameter.ParameterName;
                                    }

                                    dt.Rows.Add(strJobID, strQctype, strSampleId, strSamplename, strMatrix, Method, strTest, strParameter);

                                }
                                worksheet0.Import(dt, true, 0, 0);
                                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\EDDTemplate")) == false)
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\EDDTemplate"));
                                }
                                string strFilePath = HttpContext.Current.Server.MapPath(@"~\EDDTemplate");
                                Stream excelStream = File.Create(Path.GetFullPath(strFilePath + "\\SuboutResultEntryEDD.xlsx"));
                                workbookNew.SaveDocument(excelStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                                excelStream.Dispose();

                                using (FileStream fs = File.OpenRead(strFilePath + "\\SuboutResultEntryEDD.xlsx"))
                                {
                                    if (objSubout.ResultImportEDD == null)
                                    {
                                        objSubout.ResultImportEDD = View.ObjectSpace.CreateObject<FileData>();
                                    }
                                    objSubout.ResultImportEDD.LoadFromStream(strFilePath + "\\SuboutResultEntryEDD.xlsx", fs);
                                    View.ObjectSpace.CommitChanges();
                                }

                            }
                            SendNotificationEmail(objSubout);
                            if (boolMailSend)
                            {
                                objSubout.SuboutNotificationStatus = SuboutNotificationQueueStatus.Send;
                                boolMailSend = false;
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "mailsendsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                            }

                            foreach (SubOutSampleRegistrations objSubout1 in View.SelectedObjects)
                            {
                                objSubout1.SuboutStatus = SuboutTrackingStatus.SuboutSubmitted;
                            }
                            ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                            View.Close();



                            //View.ObjectSpace.CommitChanges();
                            //View.ObjectSpace.Refresh();
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        //if (objSubout != null)
                        //{
                        //    objSubout.SuboutStatus = SuboutTrackingStatus.SuboutSubmitted;
                        //    View.ObjectSpace.CommitChanges();
                        //    View.ObjectSpace.Refresh();
                        //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "submitsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        //    View.Close();
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void RemoveSample_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    int selectItemCount = View.SelectedObjects.Count;
                    using (var uow = new UnitOfWork())
                    {
                        List<SampleParameter> selectedParameters = View.SelectedObjects.Cast<SampleParameter>().ToList();

                        // Check if any selected sample parameter has a status other than PendingEntry
                        if (selectedParameters.Any(i => i.Status != Samplestatus.PendingEntry))
                        {
                            Application.ShowViewStrategy.ShowMessage("Unable to remove the item since it has been referenced already.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }

                        if (Application.MainWindow.View != null && Application.MainWindow.View.CurrentObject != null)
                        {
                            //SubOutSampleRegistrations objSuboutSample = uow.GetObjectByKey<SubOutSampleRegistrations>(((SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject).Oid);
                            SubOutSampleRegistrations objSuboutSample = (SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject;

                            if (objSuboutSample != null)
                            {
                                IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                                foreach (SampleParameter objParam in selectedParameters)
                                {
                                    // Ensure objParam is managed within the same UnitOfWork context
                                    //SampleParameter managedParam = uow.GetObjectByKey<SampleParameter>(objParam.Oid);
                                    SampleParameter managedParam = os.GetObjectByKey<SampleParameter>(objParam.Oid);

                                    if (managedParam != null && objSuboutSample.SampleParameter.FirstOrDefault(i => i.Oid == managedParam.Oid) != null)
                                    {
                                        managedParam.SuboutSignOff = false;
                                        managedParam.SuboutSignOffDate = null;
                                        managedParam.SuboutSignOffBy = null;
                                        managedParam.SuboutSample = null;
                                        //objSuboutSample.SampleParameter.Remove(managedParam);
                                    }
                                }
                                //uow.CommitChanges();
                                //ObjectSpace.Refresh();
                                ViewItem vi = ((DetailView)Application.MainWindow.View).FindItem("SampleParameter");
                                if (vi != null)
                                {
                                    vi.Refresh();
                                    Application.MainWindow.View.Refresh();
                                }
                                if (selectItemCount == 1)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\SuboutMessageGroup", "ItemRemoved"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                                else if (selectItemCount > 1)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\SuboutMessageGroup", "ItemsRemoved"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                                }
                            } 
                        }
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

        private void AddSample_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                CollectionSource cs = new CollectionSource(ObjectSpace, typeof(SampleParameter));
                cs.Criteria["Filter"] = new NotOperator(new InOperator("Oid", ((ListView)View).CollectionSource.List.Cast<SampleParameter>().ToList().Select(i => i.Oid).ToList()));
                ListView lvparameter = Application.CreateListView("SampleParameter_ListView_Copy_SubOutAddSamples", cs, false);
                ShowViewParameters showViewParameters = new ShowViewParameters(lvparameter);
                showViewParameters.CreatedView = lvparameter;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.CloseOnCurrentObjectProcessing = false;
                dc.Accepting += Dc_AcceptingAddSample;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_AcceptingAddSample(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    int selectItemCount = e.AcceptActionArgs.SelectedObjects.Count;
                    //using (var uow = new UnitOfWork())
                    {
                        //SubOutSampleRegistrations objSuboutSample = uow.GetObjectByKey<SubOutSampleRegistrations>(((SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject).Oid);
                        SubOutSampleRegistrations objSuboutSample = (SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject;
                        IObjectSpace os = Application.MainWindow.View.ObjectSpace;
                        if (objSuboutSample != null)
                        {
                            objSuboutSample.SuboutStatus = SuboutTrackingStatus.SuboutSubmitted;
                            objSuboutSample.Status = SuboutStatus.PendingResultEntry;
                            foreach (SampleParameter obj in e.AcceptActionArgs.SelectedObjects)
                            {
                                //SampleParameter objSample = uow.GetObjectByKey<SampleParameter>(obj.Oid);
                                SampleParameter objSample = os.GetObjectByKey<SampleParameter>(obj.Oid);
                                if (objSample != null && objSuboutSample.SampleParameter != null && !objSuboutSample.SampleParameter.Contains(objSample))
                                {
                                    objSuboutSample.SampleParameter.Add(objSample);
                                    objSample.Container = 1;
                                    objSample.SuboutSignOff = true;
                                    objSample.SuboutSignOffDate = DateTime.Now;
                                    //objSample.SuboutSignOffBy = uow.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objSample.SuboutSignOffBy = os.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    if (objSample.Testparameter != null && objSample.Samplelogin != null)
                                    {
                                        //SampleBottleAllocation objAllocation = uow.Query<SampleBottleAllocation>().FirstOrDefault(i =>
                                        //        i.SampleRegistration.Oid == objSample.Samplelogin.Oid &&
                                        //        i.TestMethod.Oid == objSample.Testparameter.TestMethod.Oid);
                                        SampleBottleAllocation objAllocation = os.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid] = ? and [TestMethod.Oid] = ?", objSample.Samplelogin.Oid, objSample.Testparameter.TestMethod.Oid));
                                        if (objAllocation != null)
                                        {
                                            if (objAllocation.Preservative != null)
                                            {
                                                objSample.Preservative = objAllocation.Preservative;
                                            }
                                            if (objAllocation.Containers != null)
                                            {
                                                objSample.ContainerType = objAllocation.Containers;
                                            }
                                        }
                                    }
                                }
                            }
                            //uow.CommitChanges();
                            //ObjectSpace.Refresh();
                            ViewItem vi = ((DetailView)Application.MainWindow.View).FindItem("SampleParameter");
                            if (vi != null)
                            {
                                vi.Refresh();
                                Application.MainWindow.View.Refresh();
                            }
                            if (selectItemCount == 1)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\SuboutMessageGroup", "ItemAdded"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            else if (selectItemCount > 1)
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\SuboutMessageGroup", "ItemsAdded"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                        }
                    }
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SubOutSampleTestOrderDateFilter_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (e.SelectedChoiceActionItem.Id == "1M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "3M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 3 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "6M")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffMonth(DateRegistered, Now()) <= 6 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "1Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 1 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "2Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 2 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "5Y")
                {
                    ((ListView)View).CollectionSource.Criteria["DateFilter1"] = CriteriaOperator.Parse("DateDiffYear(DateRegistered, Now()) <= 5 And [DateRegistered] Is Not Null");
                }
                else if (e.SelectedChoiceActionItem.Id == "ALL")
                {
                    ((ListView)View).CollectionSource.Criteria.Remove("DateFilter1");
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        //private void SuboutRollback_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        ListPropertyEditor lstviewSuboutSample = ((DetailView)Application.MainWindow.View).FindItem("SampleParameter") as ListPropertyEditor;
        //        ListPropertyEditor lstviewSuboutQCSample = ((DetailView)Application.MainWindow.View).FindItem("SubOutQcSample") as ListPropertyEditor;
        //        int selectedCount = 0;
        //        SubOutSampleRegistrations objSubout = (SubOutSampleRegistrations)Application.MainWindow.View.CurrentObject;
        //        if (lstviewSuboutSample != null && lstviewSuboutSample.ListView != null)
        //        {
        //            if (lstviewSuboutSample.ListView.SelectedObjects.Count > 0)
        //            {
        //                selectedCount = lstviewSuboutSample.ListView.SelectedObjects.Count;
        //            }
        //            if (lstviewSuboutQCSample != null && lstviewSuboutQCSample.ListView != null)
        //            {
        //                if (lstviewSuboutQCSample.ListView.SelectedObjects.Count > 0)
        //                {
        //                    selectedCount = lstviewSuboutQCSample.ListView.SelectedObjects.Count;
        //                }
        //            }

        //        }
        //        if (selectedCount == 0)
        //        {
        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
        //        }

        //        if (View.Id == "SubOutSampleRegistrations_DetailView_Level2DataReview")
        //        {

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
    }
}
