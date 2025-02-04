using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;

namespace BTLIMS.Module.Controllers
{
    public class GlobalViewController : ViewController
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        private System.ComponentModel.IContainer components;
        ModificationsController modificationController;
        DeleteObjectsViewController DeleteController;
        CopyNoOfSamplesPopUp objCopySampleInfo = new CopyNoOfSamplesPopUp();
        CopyPermissioninfo objCopyUserPermissionInfo = new CopyPermissioninfo();
        #endregion

        #region Constructor
        public GlobalViewController()
        {
            InitializeComponent();
            this.TargetViewId = "DBConnection_ListView;" 
                + "DBConnection_DetailView;" 
                + "EDDQueryBuilder_DetailView_SheetEDD;" 
                + "Topic_DetailView;" 
                + "Invoicing_DetailView_Review;" 
                + "JobIDFormat_DetailView;" 
                + "TurnAroundTime_DetailView;" 
                + "Glossary_DetailView;" 
                + "SampleLogIn_DetailView;" 
                + "Samplecheckin_DetailView;" 
                + "SampleLogIn_DetailView_Copy;" 
                + "Labware_DetailView;" 
                +"LabwareMaintenance_DetailView;" 
                 + "LabwareCertificate_DetailView;" 
                 + "Customer_DetailView;" 
                 + "Contact_DetailView;" 
                 + "Project_DetailView;" 
                 + "Employee_DetailView;" 
                 + "BroughtBy_DetailView;" 
                 + "Matrix_DetailView;" 
                 +"Preservative_DetailView;"
                 + "QCType_DetailView;" 
                 + "SampleCategory_DetailView;" 
                 + "SampleType_DetailView;"
                 + "Storage_DetailView;" 
                 + "Unit_DetailView;" 
                 + "VisualMatrix_DetailView;" 
                 + "GroupTest_DetailView;" 
                 + "TestMethod_DetailView;" 
                 +"Parameter_DetailView;" 
                 + "Method_DetailView;" 
                 + "City_DetailView;" 
                 + "Company_DetailView;" 
                 + "CustomCountry_DetailView;" 
                 + "Department_DetailView;"
                 + "Area_DetailView;" 
                 + "CurrentLanguage_DetailView;" 
                 +"Position_DetailView;" 
                 + "CustomState_DetailView;" 
                 + "MeasureMajorCategory_DetailView;"
                 + "KeyValue_DetailView;" 
                 + "KeyType_DetailView;" 
                 + "MeasureMethodLibrary_DetailView;" 
                 + "MeasureStandardCertificate_DetailView;" 
                 +"MeasureTestItem_DetailView;" 
                 + "QcParameter_DetailView;" 
                 + "CustomSystemRole_DetailView;"
                 + "UserNavigationPermission_DetailView;"
                 + "SubContractLab_DetailView;" 
                 + "SampleCustody_DetailView_Copy_SampleDisposal;" 
                 +"SampleCustody_DetailView;" 
                 + "DefaultSetting_DetailView;" 
                 + "Region_DetailView;" 
                 + "ReportCategory_DetailView;"
                 + "ReportType_DetailView;" 
                 + "ProjectCategory_DetailView;"
                 + "Collector_DetailView;" 
                 + "TabControls_DetailView;"
                 +"SampleConditionCheckData_DetailView;" 
                 + "VisualMatrix_DetailView_FieldSetup;" 
                 + "VisualMatrix_DetailView_FieldSetup;" 
                 + "eNotificationContentTemplate_DetailView;" 
                 + "BroughtBy_DetailView;" 
                 + "Container_DetailView;" 
                 +"ContainerSettings_DetailView;" 
                 + "HoldingTimes_DetailView;"
                 + "Matrix_DetailView;"
                 + "MethodCategory_DetailView;" 
                 + "PrepTypes_DetailView;" 
                 + "Preservative_DetailView;" 
                 + "QcRole_DetailView;" 
                 + "Holidays_DetailView;" 
                 +"QCRootRole_DetailView;" 
                 + "QCSource_DetailView;" 
                 + "QCType_DetailView;" 
                 + "SampleCategory_DetailView;" 
                 + "SamplePrepTemplates_DetailView;"
                 + "SampleType_DetailView;"
                 + "PretreatmentPricing_DetailView;" 
                 + "Unit_DetailView;"
                 + "COCSettings_DetailView;"
                 + "PreserveCondition_DetailView;"
                 + "ItemChargePricing_DetailView;"
                 + "Priority_DetailView;" 
                 + "ConstituentPricing_DetailView;" 
                 + "CRMQuotes_ListView_pendingsubmission;" 
                 + "CRMQuotes_DetailView;"
                 + "CRMQuotes_ListView_PendingReview;" 
                 + "CRMQuotes_ListView_Expired;"
                 + "CRMQuotes_ListView_Cancel;" 
                 + "Invoicing_DetailView_Queue;"
                 + "Manual_DetailView;"
                 + "DocumentCategory_ListView;" 
                 + "DocumentCategory_DetailView;" 
                 + "Manual_ListView;" 
                 + "CRMProspects_DetailView;"
                 + "CRMProspects_ListView;"
                 + "CRMProspects_ListView_MyOpenLeads_Copy;"
                 + "CRMProspects_ListView_Copy_Closed;" 
                 + "CRMProspects_DetailView_Closed;" 
                 + "CRMProspects_ListView_Copy_Open;"
                 + "HelpCenter_DetailView_FAQ_Articles;" 
                 + "Notes_DetailView_Client_CallLog;"
                 + "Notes_DetailView_Prospect;"
                 + "EDDBuilder_ListView;" 
                 + "EDDBuilder_DetailView;" 
                 + "EDDCategory_ListView;" 
                 + "EDDCategory_DetailView;"
                 + "UserManualEditorCategory_DetailView;" 
                 + "UnFollowSettings_DetailView;" 
                 + "Customer_DetailView_ClosedCRM;"
                 + "TestMethod_DetailView_SamplePreparationChain;" 
                 + "Distribution_DetailView;"
                 + "SampleCustodyTest_DetailView_SampleLocation;" 
                 //+ "SampleCustodyTest_DetailView_SampleDisposal;" 
                 //+ "SampleCustodyTest_DetailView_SampleDisposal_History;" 
                 + "SampleCustodyTest_DetailView_SampleLocation_History;"
                 + "SampleCustodyTest_DetailView_SampleIn;"
                 + "SampleCustodyTest_DetailView_SampleOut;" 
                 + "Email_DetailView;"
                 + "COCSettings_DetailView_Copy_SampleRegistration;" 
                 + "SubOutContractLab_DetailView;"
                 + "Reagent_DetailView;"
                 /*+ "SubOutSampleRegistrations_DetailView_Copy;"*/ 
                 + "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry;" 
                 + "DailyQC_DetailView;" 
                 + "DailyQCSettings_DetailView;" 
                 + "PTStudyLog_DetailView;"
                 + "StudyID_DetailView;"
                 + "StudyName_DetailView;"
                 + "Source_DetailView;" 
                 + "CompliantInitiation_DetailView;" 
                 + "CompliantInitiation_DetailView_Verification;" 
                 + "NonConformityInitiation_DetailView;"
                 + "NonConformityInitiation_DetailView_PendingVerification;"
                 + "ActionCategory_DetailView;"
                 + "NCAReason_DetailView;"
                 + "ProblemCategory_DetailView;"
                 + "StatusDefinition_DetailView;" 
                 + "SampleSites_DetailView;"
                 +"SamplingProposalIDFormat_DetailView;"
                 + "SamplingProposal_DetailView;"
                 + "SubOutSampleRegistrations_DetailView_TestOrder;"
                 + "PaymentStatus_DetailView;"
                 + "Accrediation_DetailView;"
                 + "MaintenanceSetup_DetailView;"
                 + "MaintenanceCategory_DetailView;"
                 + "SkipReason_DetailView;"
                 + ";TaskCheckList_DetailView;"
                 + "Activity_DetailView;"
                 + "SampleStatus_DetailView;"
                 + "VisualMatrix_DetailView_SamplingFieldSetup;"
                 + "VisualMatrix_DetailView_FieldConfiguration;"
                 + "FlutterDefaultSettings_DetailView;" 
                 + "DOC_ListView;" 
                 + "DOC_DetailView_Copy_DV;" 
                 + "SampleSourceSetup_DetailView;" 
                 + "SystemTypes_DetailView;"
                 + "WaterTypes_DetailView;"
                 + "InstrumentSoftware_DetailView;"
                 + "SDATemplate_DetailView;" 
                 + "SpreadSheetBuilder_TemplateType_DetailView;"
                 + "InstrumentSoftware_DetailView;" 
                 + "BottleSharing_DetailView;"
                 + "ReportIDFormat_DetailView;"
                 + "QCTypeMatch_DetailView;" 
                 + "ParameterMatch_DetailView;" 
                 + "ItemChargePricingCategory_ListView;" 
                 + "DefaultSetting_DetailView_Copy;" 
                 + "ItemChargePricingCategory_DetailView;" 
                 + "MCLAndSCLLimits_DetailView;"
                 + "Qualifiers_DetailView;"
                 + "PWSSystem_DetailView;"
                 + "PermitTypes_DetailView;"
                 + "PermitLibrary_DetailView;"
                 + "VendorReagentCertificate_DetailView;"
                  + "CalculationApproach_DetailView;"
                  + "ReagentOperator_DetailView;"
                  + "ReagentUnits_DetailView;"
                  + "RegentPrepCalculationEditor_DetailView;" 
                  + "StandardName_DetailView;"
                  + "WSPrepType_DetailView;" 
                  + "WSStorageName_DetailView;"
                  + "ReagentPreparation_DetailView_Calibration;" 
                  + "ReagentPreparation_DetailView_Chemistry;" 
                  + "ReagentPreparation_DetailView_MicroMedia;"
                  + "SanitarianTemperature_DetailView;"
                  + "Sizes_DetailView;"
                  + "SLKind_DetailView;"
                  + "SLCode_DetailView;"
                  + "Bare_DetailView;"
                  + "DataEntry_DetailView;"
                  + "DataEntry_ListlView;"
                  + "EmailContentTemplate_DetailView;"
                 ;
            //"LabwareMaintenance_ListView;" + "SampleType_ListView;" + "QCSource_ListView;" + "QCType_ListView;" + 
            //"Unit_ListView;" + "SampleCategory_ListView;" + "SampleLogIn_ListView;" + "Samplecheckin_ListView;" + 
            //"Project_ListView;" + "Labware_ListView;" + "SamplePrepTemplates_ListView;" + "Preservative_ListView;" + 
            //"PreserveCondition_ListView;" + "QcRole_ListView;" + "PrepTypes_ListView;" + "Container_ListView;" + 
            //"Matrix_ListView;" + "QCRootRole_ListView;" + "MethodCategory_ListView;" + "BroughtBy_ListView;" + 
            //"Collector_ListView;" + "eNotificationContentTemplate_ListView;" + "HoldingTimes_ListView;" + 
            //"VisualMatrix_ListView_FieldSetup;" + "SampleConditionCheckData_ListView;" + "ReportType_ListView;" 
            //+ "ContainerSettings_ListView_testmethod;" + "Region_ListView;" + "ReportCategory_ListView;" 
            //+ "QcParameter_ListView;" + "MeasureTestItem_ListView;" + "DefaultSetting_ListView;" + "ProjectCategory_ListView;" 
            //+ "UserNavigationPermission_ListView;" + "KeyValue_ListView;" + "MeasureMajorCategory_ListView;" + "KeyType_ListView;" 
            //+ "CustomState_ListView;" + "Position_ListView;" + "Company_ListView;" + "MeasureStandardCertificate_ListView;" 
            //+ "MeasureMethodLibrary_ListView;" + "CustomCountry_ListView;" + "City_ListView;" + "Parameter_ListView;" 
            //+ "Method_ListView;" + "TestMethod_ListView;" + "CurrentLanguage_ListView;" + "Area_ListView;" + "Department_ListView;" 
            //+ "LabwareCertificate_ListView;" + "Unit_ListView;" + "Storage_ListView;" + "SampleType_ListView;" + "Preservative_ListView;" 
            //+ "SampleCategory_ListView;" + "QCType_ListView;" + "GroupTest_ListView;" + "VisualMatrix_ListView;" + "Customer_ListView;" 
            //+ "Contact_ListView;" + "Employee_ListView;" + "BroughtBy_ListView;" + "Matrix_ListView;";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                modificationController = Frame.GetController<ModificationsController>();
                DeleteController = Frame.GetController<DeleteObjectsViewController>();
                if (modificationController != null)
                {
                    modificationController.SaveAction.Execute += SaveAction_Execute;
                    modificationController.SaveAndCloseAction.Execute += SaveAndCloseAction_Execute;
                    modificationController.SaveAndNewAction.Execute += SaveAndNewAction_Execute;
                }
                //if (DeleteController != null)
                //{
                //    DeleteController.DeleteAction.Execute += DeleteAction_Execute;
                //}
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
                if (modificationController != null)
                {
                    modificationController.SaveAction.Execute -= SaveAction_Execute;
                    modificationController.SaveAndCloseAction.Execute -= SaveAndCloseAction_Execute;
                    modificationController.SaveAndNewAction.Execute -= SaveAndNewAction_Execute;
                }
                //if (DeleteController != null)
                //{
                //    DeleteController.DeleteAction.Execute -= DeleteAction_Execute;
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
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }
        #endregion
        private void ShowMessage(string message)
        {
            try
            {
                MessageOptions options = new MessageOptions();
                options.Duration = timer.Seconds;
                options.Message = message;
                options.Type = InformationType.Success;
                options.Web.Position = InformationPosition.Top;
                Application.ShowViewStrategy.ShowMessage(options);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #region Events
        //private void DeleteAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (View != null && View.ObjectTypeInfo.Type == typeof(SampleLogIn) && objCopySampleInfo.Msgflag == false)
        //        {
        //            MessageOptions options = new MessageOptions();
        //            options.Duration = 1000;
        //            //options.Message = "Deleted SuccessFully";
        //            options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess");
        //            options.Type = InformationType.Success;
        //            options.Web.Position = InformationPosition.Top;
        //            Application.ShowViewStrategy.ShowMessage(options);
        //            objCopySampleInfo.Msgflag = true;
        //        }
        //        else if (View != null && View.ObjectTypeInfo.Type != typeof(SampleLogIn))
        //        {
        //            MessageOptions options = new MessageOptions();
        //            options.Duration = 1000;
        //            options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Deletesuccess");
        //            //options.Message = "Deleted SuccessFully";
        //            options.Type = InformationType.Success;
        //            options.Web.Position = InformationPosition.Top;
        //            Application.ShowViewStrategy.ShowMessage(options);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }
        //}
        private void SaveAndNewAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(SampleLogIn) && objCopySampleInfo.Msgflag == false)
                {
                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);
                    objCopySampleInfo.Msgflag = true;
                }
                else if (View != null && View.ObjectTypeInfo.Type != typeof(SampleLogIn))
                {
                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    //options.Message = "Saved SuccessFully";
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void SaveAndCloseAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(SampleLogIn) && objCopySampleInfo.Msgflag == false)
                {
                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    //options.Message = "Saved SuccessFully";
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);

                    objCopySampleInfo.Msgflag = true;
                }
                else if (View != null && View.ObjectTypeInfo.Type != typeof(SampleLogIn))
                {
                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    //options.Message = "Saved SuccessFully";
                    options.Type = InformationType.Success;
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);
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
                if (View != null && View.ObjectTypeInfo.Type == typeof(SampleLogIn) && objCopySampleInfo.Msgflag == false)
                {
                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    //options.Message = "Saved SuccessFully";
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);

                    objCopySampleInfo.Msgflag = true;
                }
                else if (View != null && View.ObjectTypeInfo.Type != typeof(SampleLogIn))
                {
                    objCopyUserPermissionInfo.DUpdated = false;
                    objCopyUserPermissionInfo.BUpdated = false;
                    MessageOptions options = new MessageOptions();
                    options.Duration = 1000;
                    //options.Message = "Saved SuccessFully";
                    options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                    options.Type = InformationType.Success;
                    options.Web.Position = InformationPosition.Top;
                    Application.ShowViewStrategy.ShowMessage(options);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
    #endregion

}
