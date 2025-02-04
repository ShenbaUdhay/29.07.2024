﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class HideDetailViewController : ViewController<ListView>
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        DevExpress.ExpressApp.Web.SystemModule.CallbackHandlers.ListViewFastCallbackHandlerController listController;
        public const string EnabledKeyShowDetailView = "ShowDetailViewFromListViewController";
        PermissionInfo objpermission = new PermissionInfo();
        NavigationInfo objNavInfo = new NavigationInfo();
        #endregion

        #region Constructor
        public HideDetailViewController()
        {
            InitializeComponent();
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                listController = Frame.GetController<DevExpress.ExpressApp.Web.SystemModule.CallbackHandlers.ListViewFastCallbackHandlerController>();
                if (listController != null)
                {
                    listController.Active.SetItemValue("Optimize", false);
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
                base.OnViewControlsCreated();//View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" ||View.Id == "SampleParameter_ListView_Copy_ResultView_Main" || 
                if (View.Id == "DummyClass_ListView" && !string.IsNullOrEmpty(objNavInfo.SelectedNavigationCaption))
                {
                    View.Caption = objNavInfo.SelectedNavigationCaption;
                }
                ListViewProcessCurrentObjectController targetController = Frame.GetController<ListViewProcessCurrentObjectController>();
                if (targetController != null && (View.Id == "Samplecheckin_ListView_Copy_ResultEntry" || View.Id == "Samplecheckin_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_ResultView_Main" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" || View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView" || View.Id == "CRMQuotes_ItemChargePricing_ListView" || View.Id == "TurnAroundTime_ListView_TestPriceSurcharge"
                     || View.Id == "DataEntry_ListView"
                    || View.Id == "PTStudyLog_ListView"
                    || View.Id == "ConstituentPricing_ListView"
                    || View.Id == "ItemChargePricing_ListView_Cancaled"
                    || View.Id == "DefaultPricing_ListView"
                    || View.Id == "PretreatmentPricing_ListView"
                    || View.Id == "ItemChargePricing_ListView"
                    || View.Id == "Priority_ListView"
                    || View.Id == "TestPriceSurcharge_ListView"
                    || View.Id == "GroupTestMethod_ListView"
                    || View.Id == "Preservative_ListView_ContainerSetting"
                    || View.Id == "Container_ListView_ContainerSettings"
                    || View.Id == "GroupTest_TestMethods_ListView_grouptestsurcharge"
                    || View.Id == "DummyClass_ListView_TemplateInfo"
                    || View.Id == "TestMethod_ListView_TaskBA_Popup"
                    || View.Id == "TestMethod_ListView_COCBA_Popup"
                    || View.Id == "TestMethod_ListView_SamplesBA_Popup"
                    || View.Id == "DummyClass_ListView_COCSettings"
                    || View.Id == "DummyClass_ListView_SampleRegistration"
                    || View.Id == "DummyClass_ListView_TasksRegistration"
                    || View.Id == "VisualMatrix_ListView_Sample_SampleMatrix"
                    || View.Id == "SampleLogIn_ListView_SampleRegistration_Bottle"
                    || View.Id == "SampleParameter_ListView_Samplebottle"
                    || View.Id == "VisualMatrix_ListView_COCSettings_SampleMatrix"
                    || View.Id == "COCSettingsTest_ListView_Bottle"
                    || View.Id == "SampleBottleAllocation_ListView_Sampleregistration"
                    || View.Id == "SampleBottleAllocation_ListView_COCSettings"
                    || View.Id == "COCSettingsSamples_ListView_COCBottle"
                    || View.Id == "Sampling_ListView_TaskBottle"
                    || View.Id == "SampleBottleAllocation_ListView_Taskregistration"
                    || View.Id == "SamplingParameter_ListView_SamplingProposal_Copy_taskbottle"
                    || View.Id == "VisualMatrix_ListView_TaskSampleMatrix"
                    || View.Id == "TestPrice_ListView"
                    || View.Id == "TestPrice_ListView_Copy_perparameter"
                    || View.Id == "TestPrice_ListView_Copy_pertest"
                    || View.Id == "DefaultSetting_ListView_2_DefaultSetting2"
                    || View.Id == "QCType_ListView_SamplePrepBatchSequence"
                    || View.Id == "VisualMatrix_ListView_FieldSetup_Copy_From"
                    || View.Id == "Reporting_SampleParameter_ListView"
                    || View.Id == "Reporting_ListView"
                    || View.Id == "Reporting_ListView_Copy_ReportApproval"
                    || View.Id == "Reporting_ListView_Copy_ReportView"
                    || View.Id == "SampleParameter_ListView_Copy_ResultView"
                    || View.Id == "SampleLogIn_Testparameters_ListView"
                    || View.Id == "TestMethod_QCTypes_ListView"
                    || View.Id == "Requisition_ListView_Review"
                    || View.Id == "Requisition_ListView_Approve"
                    || View.Id == "Requisition_ListView_Receive"
                    || View.Id == "SpreadSheetEntry_ListView_Copy_QC"
                    || View.Id == "SpreadSheetEntry_ListView_Copy_QC_ResultApproval"
                    || View.Id == "SpreadSheetEntry_ListView_Copy_QC_ResultView"
                    || View.Id == "SpreadSheetEntry_ListView_Copy_Sample"
                    || View.Id == "SpreadSheetEntry_ListView_Copy_Sample_ResultApproval"
                    || View.Id == "SpreadSheetEntry_ListView_Copy_Sample_ResultView"
                    || View.Id == "ICMSetup_ListView"
                    || View.Id == "Distribution_ListView_ConsumptionViewmode"
                    || View.Id == "Distribution_ListView_Copy_ExpirationAlert"
                    || View.Id == "Items_ListView_Copy_StockAlert"
                    || View.Id == "Items_ListView_Copy_StockWatch"
                    || View.Id == "SampleParameter_ListView_Copy_CustomReporting"
                    || View.Id == "SequenceSetup_ListView"
                    || View.Id == "QCBatch_ListView"
                    || View.Id == "QCBatchSequence_ListView"
                    || View.Id == "QCType_ListView_qcbatchsequence"
                    || View.Id == "SampleParameter_ListView_Copy_SampleRegistration"
                    || View.Id == "COCSettingsTest_ListView_Copy_SampleRegistration"
                    || View.Id == "COCSettingsSamples_ListView"
                    || View.Id == "COCSettingsTest_ListView"
                    || View.Id == "COCSettings_ListView_SampleRegistration"
                    || View.Id == "Samplecheckin_LookupListView_Copy_QCResultValidationQueryPanel"
                    || View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultValidation"
                    || View.Id == "SDMSRollback_ListView"
                    || View.Id == "SpreadSheetEntry_ListView_QCBatchQueryPanel_ResultApproval"
                    || View.Id == "SampleLogIn_ListView_SampleDisposition"
                    || View.Id == "Reporting_ListView_Account"
                    || View.Id == "Reporting_ListView_Recall"
                    || View.Id == "Reporting_ListView_PrintDownload"
                    || View.Id == "Reporting_ListView_Delivery"
                    || View.Id == "Reporting_ListView_Archieve"
                    || View.Id == "SampleWeighingBatchSequence_ListView"
                    || View.Id == "SDMSDCImport_ListView"
                    || View.Id == "Requisition_ListView"
                    || View.Id == "SampleParameter_ListView_Copy_QCResultView"
                    || View.Id == "SDMSReportPopupDC_ListView"
                    || View.Id == "SampleWeighingBatchSequence_ListView_Copy"
                    //|| View.Id == "SamplePrepBatch_ListView"
                    || View.Id == "ReportRollbackLog_ListView"
                    || View.Id == "SampleWeighingBalance_ListView"
                    || View.Id == "SamplePrepBatchSequence_ListView"
                    || View.Id == "Samplecheckin_ListView_Incompletejobs"
                    || View.Id == "Samplecheckin_ListView_ProjectTracking"
                    || View.Id == "SamplePretreatmentBatch_SamplePretreatmentBatchSeqDetail_ListView"
                    || View.Id == "Distribution_ListView_ConsumptionViewmode"
                    || View.Id == "Distribution_ListView_ASIProductStockInventory"
                    || View.Id == "Distribution_ListView_ItemStockInventory"
                    || View.Id == "Items_ListView_ResaleProductStockInventory"
                    || View.Id == "Items_ListView_Copy_StockInventory"
                    || View.Id == "SampleWeighingBatchSequence_ListView_Tracking"
                    || View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault"
                    || View.Id == "QCBatch_ListView_Copy"
                    || View.Id == "SampleParameter_ListView_Copy_CustomReporting_Edit"
                    || View.Id == "QCBatchSequence_ListView_Copy"
                    || View.Id == "QCType_ListView_analyticalbatchsequence"
                    || View.Id == "SampleParameter_ListView_Copy_RegistrationSignedOffSamples"
                    /*|| View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview"|| View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview"*/
                    || (!View.AllowEdit && (View.Id == "Items_ListView_Copy_StockWatch"
                    || View.Id == "Requisition_ListView_Cancelled"
                    || View.Id == "Distribution_ListView_Consumption_Tracking"
                    || View.Id == "Distribution_ListView_LTsearch"
                    || View.Id == "Requisition_ListView_Cancelled"
                    || View.Id == "Requisition_ListView_Tracking"
                    || View.Id == "Distribution_ListView_Summary"
                    || View.Id == "Distribution_ListView_Statistics"
                    || View.Id == "Requisition_ListView_History"
                    || View.Id == "Requisition_ListView_PurchaseOrder_Lookup"
                    || View.Id == "Requisition_ListView_ViewMode"
                    || View.Id == "Company_Certificate_ListView"
                    || View.Id == "Reporting_ListView_Level1Review_View"
                    || View.Id == "Reporting_ListView_Level2Review_View"
                    || View.Id == "Testparameter_ListView_Test_QCSampleParameter"
                    || View.Id == "Testparameter_ListView_Test_SampleParameter"
                    || View.Id == "QCBatch_Reagents_ListView"
                    || View.Id == "QCBatch_Instruments_ListView"
                    || View.Id == "DataSourceEmailTemplate_ListView"
                    || View.Id == "CustomReportBuilder_ListView"
                    || View.Id == "SampleParameter_ListView_Copy_SubOutPendingSamples"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Copy_SuboutSampleRegistration"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Tracking"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_ViewMode"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView"
                    || View.Id == "SampleParameter_ListView_SuboutSampleHistory"
                    || View.Id == "SubOutSampleRegistrations_ListView_NotificationQueueView"
                    || View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data"
                    || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data"
                    || View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples"
                    || View.Id == "SampleLogIn_ListView_SampleDisposition_DisposedSamples"
                    || View.Id == "Component_ListView_Test"
                    || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History"
                    || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History"
                    || View.Id == "Tasks_ListView_PrintingSampleLabels"
                    || View.Id == "Sampling_ListView_FieldDataReview1"
                    || View.Id == "Sampling_ListView_FieldDataReview2"
                    || View.Id == "SamplingTest_ListView_FieldDataReview1"
                    || View.Id == "SamplingTest_ListView_FieldDataReview2"
                    || View.Id == "SamplingStation_ListView_FieldDataReview1"
                    || View.Id == "SamplingStation_ListView_FieldDataReview2"
                    || View.Id == "Tasks_ListView_Copy_ProposalValidation"
                    || View.Id == "SamplingParameter_ListView_SamplingProposal"
                    || View.Id == "GroupTestMethod_ListView_IsGroup"
                    || View.Id == "PLMStereoscopicObservation_ListView"
                    || View.Id == "PLMExam_ListView"
                    || View.Id == "Samplecheckin_ListView_SampleReceiptNotification"
                    //|| View.Id == "CRMQuotes_ListView_PendingReview"
                    //|| View.Id == "CRMQuotes_ListView_ReviewHistory"
                    || View.Id == "CRMQuotes_ListView_Reviewed"))
                    || View.Id == "ReportPackage_ListView_Copy"
                     || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_DC"
                     || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults"
                     || View.Id == "Contract_ListView_ContractTrcking_Copy_DC"
                     || View.Id == "Contact_ListView_Copy_DC"
                     || View.Id == "Customer_Note_Listview_Copy_DC"
                     || View.Id == "Project_ListView_Copy_DC"
                     || View.Id == "Customer_ListView_Copy_DC"
                    || View.Id == "Items_ListView_Copy_DC"
                    || View.Id == "Method_ListView_Copy_DC"
                    || View.Id == "Parameter_ListView_Copy_DC"
                    || View.Id == "HoldingTimes_ListView_DataCenter"
                    || View.Id == "Testparameter_ListView_Copy_DataCenter"
                    || View.Id == "TestMethod_PrepMethods_ListView_DataCenter")
                    || View.Id == "Employee_ListView_DataCenter"
                    || View.Id == "Testparameter_ListView_Test_SampleParameter_DataCenter"
                    || View.Id == "InvoicingAnalysisCharge_ListView_Invoice"
                     || View.Id == "Samplecheckin_ListView_Incompletejobs_DataCenter"
                    || View.Id == "Component_ListView_Datacenter"
                    || View.Id == "ConstituentPricing_ListView_DataCenter"
                    || View.Id == "Container_ListView_DataCenter"
                    || View.Id == "DefaultPricing_ListView_DataCenter"
                    || View.Id == "Vendors_ListView_DataCenter"
                     || View.Id == "Testparameter_ListView_Test_Surrogates_DataCenter"
                    || View.Id == "RoleNavigationPermission_ListView_DataCenter"
                     || View.Id == "SpreadSheetBuilder_SequencePattern_ListView_DataCenter"
                    || View.Id == "DummyClass_ListView_TemplateInfo_DataCenter"
                    || View.Id == "Testparameter_ListView_Test_InternalStandards_TestISTD_DataCenter"
                    || View.Id == "Samplecheckin_ListView_DataCenter"
                    || View.Id == "SampleLogIn_ListView_DataCenter"
                    || View.Id == "SampleParameter_ListView_DataCenter"
                    || View.Id == "Method_ListView_Copy_DC"
                    || View.Id == "SampleParameter_ListView_DataCenter"
                    || View.Id == "SampleParameter_ListView_DataCenter"
                    || View.Id == "TestMethod_TestGuides_ListView_DataCenter"
                    || View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault_DataCenter"
                    || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC" || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample"
                    || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Review" || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_View"
                    || View.Id == "PTStudyLog_Results_ListView" || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Delivery"
                     || View.Id == "ConstituentPricingTier_ListView_DataCenter"
                    || View.Id == "Manual_ListView_DataCenter"
                    || View.Id == "Employee_ListView_UserPermisstion_DataCenter"
                    || View.Id == "Samplecheckin_Notes_ListView_DataCenter"
                    || View.Id == "Testparameter_LookupListView_ConstituentPricing"
                    || View.Id == "Testparameter_LookupListView_Quotes"
                    || View.Id == "GroupTestMethod_ListView_IsGroup"
                    || View.Id == "ConstituentPricingTier_ListView"
                    || View.Id == "SampleBottleAllocation_ListView_SignOff" || View.Id == "SampleBottleAllocation_ListView_SignedOff" || View.Id == "Parameter_ListView_Invoice"
                    || View.Id == "SpreadSheetBuilder_ClosingQCTestRun_ListView"
                    || View.Id == "SpreadSheetBuilder_InitialQCTestRun_ListView"
                    || View.Id == "SpreadSheetBuilder_SampleQCTestRun_ListView"
                     || View.Id == "Testparameter_LookupListView_Quotes_PriceCode"
                     || View.Id == "UserRightDC_ListView"
                     || View.Id == "Labware_ListView_Copy_DC"
                     || View.Id == "TestMethod_ListView_Instrument_DC"
                     || View.Id == "NPSampleFields_ListView"
                     || View.Id == "Samplecheckin_ListView_SampleReceiptNotification_History"
                      || View.Id == "Collector_ListView_ClientCollectors"
                      || View.Id == "AnalysisPricing_ListView_Quotes"
                      || View.Id == "CRMQuotes_AnalysisPricing_ListView"
                      || View.Id == "CRMQuotes_Attachments_ListView"
                      || View.Id == "CRMQuotes_ItemChargePricing_ListView"
                      || View.Id == "CRMQuotes_Note_ListView"
                      || View.Id == "Contact_LookupListView_Samplereceipt_EmailList"
                      || View.Id == "TestPriceSurcharge_ListView_Quotes_Popup"
                      || View.Id == "Invoicing_ListView_Delivery"
                      || View.Id == "AssignDashboardToUserDepartment_ListView"
                      || View.Id == "CompliantInitiation_ListView_DataCenter"
                      || View.Id == "NonConformityInitiation_ListView_DataCenter"
                      || View.Id == "Reporting_ListView_Datacenter"
                      || View.Id == "Invoicing_ListView_DataCenter"
                      || View.Id == "CRMQuotes_ListView_DataCenter"
                      || View.Id == "Samplecheckin_ListView_Invoice"
                      || View.Id == "HelpCenter_ListView_Articles"
                      //|| View.Id == "Invoicing_ListView_Review_History"
                      || View.Id == "Samplecheckin_ListView_ResultValidation"
                      || View.Id == "Samplecheckin_ListView_ResultApproval"
                      //|| View.Id == "Invoicing_ItemCharges_ListView"
                      //|| View.Id == "CRMQuotes_ListView_SubmittedHistory"
                      || View.Id == "CRMQuotes_AnalysisPricing_ListView_ViewMode"
                      || View.Id == "CRMQuotes_QuotesItemChargePrice_ListView_ViewMode"
                      || View.Id == "SpreadSheetEntry_AnalyticalBatch_DataPackageCrossChecks_ListView"
                      || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_SDAHeader"
                      || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_SDADetail"
                      || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_SDACalibration"
                      //|| View.Id == "Invoicing_ListView_Delivery_History"
                      || View.Id == "MaintenanceTaskCheckList_ListView_MaintenanceQueue_History"
                      || View.Id == "ClientRequest_ListView_RequestTracking"
                      || View.Id == "ClientRequest_ListView_RequestValidation"
                      || View.Id == "ClientRequest_ListView_RequestValidation_History"
                      || View.Id == "ClientRequest_ListView_RequestRegistration"
                      || View.Id == "Deposits_Note_ListView_CollectionLog"
                      || View.Id == "Testparameter_ListView_Parameter"
                      || View.Id == "AuditData_ListView"
                      || View.Id == "RoleNavigationPermission_RoleNavigationPermissionDetails_ListView_DataCenter"
                      || View.Id == "Deposits_ListView_DepositTracking" || View.Id == "DepositEDDExport_ListView"
                      || View.Id == "InvoicingEDDExport_ListView" || View.Id == "NPFields_ListView" || View.Id == "NotebookBuilder_SetupFields_ListView"
                      || View.Id == "Activity_ListView_Agenda" || View.Id == "NonPersistentReagent_ListView" || View.Id == "NotebookFields_ListView"
                      || View.Id == "LoginLog_ListView_Tracking" || View.Id == "ResultDefaultValue_LookupListView_ResultEntry"
                      || View.Id == "NotebookFields_ListView_Available" || View.Id == "CalibrationSettings_ListView_History" || View.Id == "ReagentPrepLog_ListView_Chemistry"
                      || View.Id == "NPSampleFields_ListView_Reagent" || View.Id == "ReagentPrepLog_ListView_MicroMedia" || View.Id == "Samplecheckin_ListView_ResultValidation_View"
                      || View.Id == "Samplecheckin_ListView_ResultApproval_View" || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation"
                      || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"
                      || View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                      || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                      || View.Id == "QCBatch_ListView_ResultView" || View.Id == "QCBatch_ListView_ResultEntry" || View.Id == "Samplecheckin_ListView_ResultEntry"
                      || View.Id == "Samplecheckin_ListView_ResultView" /*|| View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_History"*/ || View.Id == "LoginLog_ListView_DataCenter" || View.Id == "Items_ListView_Copy_TestMethodLink"
                      || View.Id == "TestMethod_PrepMethods_ListView_PrepQueue"
                      || View.Id == "COCSettingsSamples_LookupListView_Copy_COCSamples_Copy_CopyTest"
                      || View.Id == "SampleLogIn_LookupListView_Copy_SampleLogin_Copy_CopyTest" || View.Id == "SampleLogIn_ListView_SampleRegistration_CopyTo_SampleID" || View.Id == "SampleLogIn_ListView_Bottle" || View.Id == "COCSettingsSamples_ListView_Copy_Bottle"
                      || View.Id == "SamplePrepBatchSequence_ListView_History" || View.Id == "TestPriceSurcharge_ListView_DataCenter"
                      || View.Id == "TestPriceSurcharge_ListView_DataCenter" || View.Id == "QCType_ListView_SamplePrepBatchSequence_History" || View.Id == "CRMQuotes_ListView_DataCenter"
                      /*|| View.Id == "SubOutSampleRegistrations_ListView_Level3DataReview_History"*/ || View.Id == "SubOutSampleRegistrations_DetailView_Level2DataReview" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView"
                      || View.Id == "RegistrationSignOff_ListView" || View.Id == "RegistrationSignOff_ListView_SignedOff"
                      || View.Id == "SampleParameter_ListView_Copy_TrendAnalysis" || View.Id == "TrendAnalysis_ListView_DateVisualization" || View.Id == "AnalysisPricing_ListView_Quotes_SampleRegistration"
                      || View.Id == "InvoicingAnalysisCharge_ListView_PreInvoiceDetails" || View.Id == "Invoicing_ItemCharges_ListView_PreinvoiceDetails" || View.Id == "Samplecheckin_ListView_Copy_Reporting"
                      || View.Id == "SampleLogIn_ListView_SourceSample" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level3Data_History" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Level2Data_History" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level2"
                      || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level2_History" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level3" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_Level3_History" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView_ResltEntryView"
                      || View.Id == "SampleParameter_ListView_TestEdit" || View.Id == "SampleLogIn_LookupListView_EditTest" || View.Id == "Testparameter_LookupListView_TestEdit"  || View.Id == "MCLAndSCLLimits_ListView" ||View.Id== "DummyClass_ListView_Sampling" 
                      ||View.Id== "Sampling_ListView_SourceSample"||View.Id== "SampleLogIn_LookupListView_Copy_SampleLogin_Copy_CopyTest" || View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView_Report" || View.Id == "DWQRReportTemplateSetup_SampleSites_ListView_DWQR_ReportID" 
                      || View.Id == "DWQRReportTemplateSetup_ParameterCollection_ListView" || View.Id == "DWQRReportTemplateSetup_SampleSites_ListView"
                      || View.Id == "DOC_ListView"
                      || View.Id == "SampleParameter_ListView_Copy_DOC"
                      || View.Id == "SampleParameter_ListView_Copy_DOC_Lookup"
                      || View.Id == "SampleLogIn_ListView_FieldDataReview1_Sampling"
                      || View.Id == "SampleLogIn_ListView_FieldDataReview1_Station"
                      || View.Id == "SampleParameter_ListView_FieldDataReview1"
                      || View.Id == "SampleLogIn_ListView_FieldDataReview2_Sampling"
                      || View.Id == "SampleLogIn_ListView_FieldDataReview2_Station"
                      || View.Id == "SampleParameter_ListView_FieldDataReview2" || View.Id== "SamplingProposal_ListView_History"
                      || View.Id == "QualifierAutomation_ListView"
                      || View.Id == "LabwareCertificate_ListView"
                      || View.Id == "MaintenanceTaskCheckList_ListView"
                      || View.Id == "Employee_ListView_EmailProfile"
                      || View.Id == "Items_Linkparameters_ListView"
                      || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Copy_SuboutSampleRegistration"
                      || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_TestOrder"
                      || View.Id == "SampleIn_ListView"
                      ||View.Id== "TestMethod_ListView_Copy_DC"
                       ||View.Id== "COCSettings_ListView_DataCenter"
                      || View.Id == "SampleCustodyTest_SampleIns_ListView_Copy_samplelocation"
                      || View.Id == "SampleBottleAllocation_ListView_CoolerId"
                      || View.Id == "SampleCustodyTest_SampleIns_ListView_SampleDisposal_History"
                      ||View.Id== "QCType_ListView_DC"
                      ||View.Id== "ReagentPreparation_ListView_CopyPrevious"
					  || View.Id == "AuditData_ListView_FieldDataEntry"
                      || View.Id == "AuditData_ListView_ReportTracking"
                      || View.Id == "AuditData_ListView_Sampleregistration"
                      || View.Id == "AuditData_ListView_SampleSites"
                      || View.Id == "AuditData_ListView_SampleTransfer"
                      || View.Id == "AuditData_ListView_Test"
                      )
                {
                    targetController.ProcessCurrentObjectAction.Enabled[EnabledKeyShowDetailView] = false;// ((IModelShowDetailView)View.Model).ShowDetailView;
                }
                else
                {
                    targetController.ProcessCurrentObjectAction.Enabled[EnabledKeyShowDetailView] = true;
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
                listController = Frame.GetController<DevExpress.ExpressApp.Web.SystemModule.CallbackHandlers.ListViewFastCallbackHandlerController>();
                if (listController != null)
                {
                    listController.Active.SetItemValue("Optimize", true);
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
