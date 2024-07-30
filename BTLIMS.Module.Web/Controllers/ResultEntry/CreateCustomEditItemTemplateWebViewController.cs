using ALPACpre.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.Web;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using LDM.Module.Web.Editors;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.ReagentPreparation;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using Priority = Modules.BusinessObjects.Setting.Priority;

namespace LDM.Module.Web.Controllers.ResultEntry
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CreateCustomEditItemTemplateWebViewController : ViewController<ListView>
    {
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        TaskManagementInfo TMInfo = new TaskManagementInfo();
        MessageTimer timer = new MessageTimer();
        BottlesOrderInfo BOInfo = new BottlesOrderInfo();
        #region Constructor
        public CreateCustomEditItemTemplateWebViewController()
        {
            InitializeComponent();
            TargetViewId = /*"AnalysisPricing_ListView_Quotes;" + "CRMQuotes_AnalysisPricing_ListView;" +*/"TestMethod_QCTypes_ListView;" + "TestPriceDetail_ListView_Copy_perparameter;" + "TestPriceDetail_ListView_Copy_pertest;" + "SampleParameter_ListView_Copy_ResultValidation;" + "Requisition_ListViewEntermode;" + "SampleParameter_ListView_Copy_QCResultApproval;" + "SampleParameter_ListView_Copy_ResultApproval;" + "SampleParameter_ListView_Copy_ResultEntry;" + "SampleParameter_ListView_Copy_ResultView;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;" + "SampleParameter_ListView_Copy_ResultView_SingleChoice;" +
                "Testparameter_ListView_Copy;" + "QCTestParameter_ListView_Copy;" + "Distribution_ListView;" + /*"Requisition_ListView_Review;" + */"Requisition_ListView_Approve;" + "Distribution_ListView_Consumption;" +
                "Distribution_ListView_Disposal;" + "Disposal_ListView;" + "SampleParameter_ListView_Copy_SubOut;" + "SampleParameter_ListView_Copy_SubOut_Viewmode;" + "ExistingStock_ListView;" + "Requisition_ListView_Receive;" +
                "COCSettingsSamples_ListView_Copy_SampleRegistration;" + "SampleLogIn_ListView_Copy_SampleRegistration;" + "Items_ListView_Copy;" + "TestParameter_ListView_DefaultSettings;" + "QcParameter_ListView_DefaultSettings;" + "SampleParameter_ListView_Copy_QCResultEntry;" + "IndoorInspection_ClauseInspections_ListView;"
                + "Requisition_ListView_DirectReceiveEntermode;" + "Distribution_ListView_Viewmode;" + "Samplecheckin_CustomDueDates_ListView;"
                + "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview;" + "SampleParameter_ListView_Copy_ResultApproval_Level2Review;" + "Tasks_Sampling_ListView_SampleTransfer;"
                + "Sampling_ListView_SamplingAssignment;" + "BottlesOrder_BottlesOrderContainer_ListView;" + "Tasks_BottlesOrders_ListView_DeliveryTasks;" + "ContainerSettings_ListView_testmethod;"
                + "ClientRequest_DeliveryBottleOrders_ListView_DeliveryTasks;" + "InvoicingAnalysisCharge_ListView_Queue;" + "ReagentPreparation_ReagentPrepLogs_ListView_Calibration;"
                + "SampleParameter_ListView_Copy_QCResultValidation;" + "SampleParameter_ListView_Copy_QCResultApproval;" + "SampleParameter_ListView_Copy_ResultValidation;" + "SampleParameter_ListView_Copy_ResultApproval;" + "Distribution_ListView_StockQtyEdit;" + "Testparameter_LookupListView_Copy_SampleLogin_Copy;"
                + "Testparameter_LookupListView_Sampling_SeletectedTest;" + "SamplingProposal_CustomDueDates_ListView;" + "COCSettings_ListView_SamplingProposal_ImportCOC;" + "SampleLogIn_ListView_SamplingAllocation;"
                + "SampleLogIn_ListView_FieldDataEntry_Sampling;" + "SampleBottleAllocation_ListView_SampleTransfer;" + "PLMExam_ListView;";
        }
        #endregion

        #region DefaultEvents
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View != null && View.Editor != null && View.CollectionSource != null && View.CollectionSource.List != null)
                {
                    ASPxGridListEditor listEditor = (ASPxGridListEditor)View.Editor;
                    listEditor.CreateCustomEditItemTemplate += listEditor_CreateCustomEditItemTemplate;
                    listEditor.CreateCustomGridViewDataColumn += listEditor_CreateCustomGridViewDataColumn;
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
            // Access and customize the target View control.
            if (View != null && View.Editor != null && View.CollectionSource != null && View.CollectionSource.List != null)
            {
                ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
                gridListEditor.BatchEditModeHelper.CustomUpdateBatchValue += BatchValueIsUpdated;
                if (View.Id == "Distribution_ListView_StockQtyEdit")
                {
                    string Grid_BatchEditStartEditingStorage = "BatchEditStartEditingStorage";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingstorage, Grid_BatchEditStartEditingStorage);

                    string Grid_BatchEditEndEditingStorage = "BatchEditEndEditingStorage";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingstorage, Grid_BatchEditEndEditingStorage);
                }
                if (View.Id == "PLMExam_ListView")
                {
                    string Grid_BatchEditStartEditingFiberType = "BatchEditStartEditingFiberType";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingFiberType, Grid_BatchEditStartEditingFiberType);

                    string Grid_BatchEditEndEditingFiberType = "BatchEditEndEditingFiberType";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingFiberType, Grid_BatchEditEndEditingFiberType);
                }
                else if (View != null && View.Id == "SampleParameter_ListView_Copy_ResultView")
                {
                    //string Grid_BatchEditStartEditingKey = "BatchEditStartEditingKey";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    //BatchEditStartEditing, Grid_BatchEditStartEditingKey);

                    //string Grid_BatchEditEndEditingKey = "BatchEditEndEditingKey";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    //BatchEditEndEditing, Grid_BatchEditEndEditingKey);
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(SampleParameter))
                {
                    if (View.Id != "SampleParameter_ListView_Copy_QCResultValidation" && View.Id != "SampleParameter_ListView_Copy_QCResultApproval" && View.Id != "SampleParameter_ListView_Copy_ResultValidation" && View.Id != "SampleParameter_ListView_Copy_ResultApproval")
                    {
                        string Grid_BatchEditStartEditingKeyUnitName = "BatchEditStartEditingKeyUnitName";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingUnitName, Grid_BatchEditStartEditingKeyUnitName);

                        string Grid_BatchEditEndEditingKeyUnitName = "BatchEditEndEditingKeyUnitName";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingUnitName, Grid_BatchEditEndEditingKeyUnitName);
                    }

                    //string Grid_BatchEditStartEditingKey = "BatchEditStartEditingKey";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    //BatchEditStartEditing, Grid_BatchEditStartEditingKey);

                    //string Grid_BatchEditEndEditingKey = "BatchEditEndEditingKey";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    //BatchEditEndEditing, Grid_BatchEditEndEditingKey);

                    string Grid_BatchEditStartEditingKeyFinalResultUnits = "BatchEditStartEditingKeyFinalResultUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingFinalResultUnits, Grid_BatchEditStartEditingKeyFinalResultUnits);

                    string Grid_BatchEditEndEditingKeyFinalResultUnits = "BatchEditEndEditingKeyFinalResultUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingFinalResultUnits, Grid_BatchEditEndEditingKeyFinalResultUnits);

                    string Grid_BatchEditStartEditingKeySurrogateUnits = "BatchEditStartEditingKeySurrogateUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingSurrogateUnits, Grid_BatchEditStartEditingKeySurrogateUnits);

                    string Grid_BatchEditEndEditingKeySurrogateUnits = "BatchEditEndEditingKeySurrogateUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingSurrogateUnits, Grid_BatchEditEndEditingKeySurrogateUnits);

                    if (View != null && View.Id == "SampleParameter_ListView_Copy_SubOut" || View.Id == "SampleParameter_ListView_Copy_SubOut_Viewmode")
                    {
                        string Grid_BatchEditStartEditingKeySubLabName = "BatchEditStartEditingKeySubLabName";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                        BatchEditStartEditingSubLabName, Grid_BatchEditStartEditingKeySubLabName);

                        string Grid_BatchEditEndEditingKeySubLabName = "BatchEditEndEditingKeySubLabName";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                        BatchEditEndEditingSubLabName, Grid_BatchEditEndEditingKeySubLabName);

                        string Grid_BatchEditStartEditingKeySubOutBy = "BatchEditStartEditingKeySubOutBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSubOutBy, Grid_BatchEditStartEditingKeySubOutBy);

                        string Grid_BatchEditEndEditingKeySubOutBy = "BatchEditEndEditingKeySubOutBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSubOutBy, Grid_BatchEditEndEditingKeySubOutBy);
                    }
                    if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry" || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation" || View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"))
                    {
                        string Grid_BatchEditStartEditingKeyAnalyzedBy = "BatchEditStartEditingKeyAnalyzedBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingAnalyzedBy, Grid_BatchEditStartEditingKeyAnalyzedBy);

                        string Grid_BatchEditEndEditingKeyAnalyzedBy = "BatchEditEndEditingKeyAnalyzedBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingAnalyzedBy, Grid_BatchEditEndEditingKeyAnalyzedBy);

                        string Grid_BatchEditStartEditingKeyEnteredBy = "BatchEditStartEditingKeyEnteredBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingEnteredBy, Grid_BatchEditStartEditingKeyEnteredBy);

                        string Grid_BatchEditEndEditingKeyEnteredBy = "BatchEditEndEditingKeyEnteredBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingEnteredBy, Grid_BatchEditEndEditingKeyEnteredBy);

                        string Grid_BatchEditStartEditingKeySTDConcUnit = "BatchEditStartEditingKeySTDConcUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSTDConcUnit, Grid_BatchEditStartEditingKeySTDConcUnit);

                        string Grid_BatchEditEndEditingKeySTDConcUnit = "BatchEditEndEditingKeySTDConcUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSTDConcUnit, Grid_BatchEditEndEditingKeySTDConcUnit);

                        string Grid_BatchEditStartEditingKeySTDVolUnit = "BatchEditStartEditingKeySTDVolUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSTDVolUnit, Grid_BatchEditStartEditingKeySTDVolUnit);

                        string Grid_BatchEditEndEditingKeySTDVolUnit = "BatchEditEndEditingKeySTDVolUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSTDVolUnit, Grid_BatchEditEndEditingKeySTDVolUnit);

                        string Grid_BatchEditStartEditingKeySpikeAmountUnit = "BatchEditStartEditingKeySpikeAmountUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSpikeAmountUnit, Grid_BatchEditStartEditingKeySpikeAmountUnit);

                        string Grid_BatchEditEndEditingKeySpikeAmountUnit = "BatchEditEndEditingKeySpikeAmountUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSpikeAmountUnit, Grid_BatchEditEndEditingKeySpikeAmountUnit);


                        string Grid_BatchEditStartEditingKeyItemUnitsSampleparameter = "Grid_BatchEditStartEditingKeyItemUnitsSampleparameter";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingItemUnitsSampleparameter, Grid_BatchEditStartEditingKeyItemUnitsSampleparameter);

                        string Grid_BatchEditEndEditingKeyItemUnitsSampleparameter = "Grid_BatchEditEndEditingKeyItemUnitsSampleparameter";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingItemUnitsSampleparameter, Grid_BatchEditEndEditingKeyItemUnitsSampleparameter);

                    }
                    if (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation")
                    {
                        string Grid_BatchEditStartEditingKeyResultValidatedBy = "BatchEditStartEditingKeyResultValidatedBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingResultValidatedBy, Grid_BatchEditStartEditingKeyResultValidatedBy);

                        string Grid_BatchEditEndEditingKeyResultValidatedBy = "BatchEditEndEditingKeyResultValidatedBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingResultValidatedBy, Grid_BatchEditEndEditingKeyResultValidatedBy);
                    }
                    if (View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval")
                    {
                        string Grid_BatchEditStartEditingKeyResultApprovedBy = "BatchEditStartEditingKeyResultApprovedBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingResultApprovedBy, Grid_BatchEditStartEditingKeyResultApprovedBy);

                        string Grid_BatchEditEndEditingKeyResultApprovedBy = "BatchEditEndEditingKeyResultApprovedBy";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingResultApprovedBy, Grid_BatchEditEndEditingKeyResultApprovedBy);

                    }
                }
                else if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin_Copy" || View.Id == "Testparameter_LookupListView_Sampling_SeletectedTest")
                {
                    string Grid_BatchEditStartEditingTAT = "Grid_BatchEditStartEditingTAT";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingKeyTAT, Grid_BatchEditStartEditingTAT);

                    string Grid_BatchEditEndEditingKeyTAT = "Grid_BatchEditEndEditingKeyTAT";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingKeyTAT, Grid_BatchEditEndEditingKeyTAT);
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Testparameter) && View.Id != "Testparameter_LookupListView_Copy_SampleLogin_Copy" || View.Id == "Testparameter_LookupListView_Sampling_SeletectedTest")
                {
                    string Grid_BatchEditStartEditingKeyFinalDefaultUnits = "BatchEditStartEditingKeyFinalDefaultUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingFinalDefaultUnits, Grid_BatchEditStartEditingKeyFinalDefaultUnits);

                    string Grid_BatchEditEndEditingKeyFinalDefaultUnits = "BatchEditEndEditingKeyFinalDefaultUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingFinalDefaultUnits, Grid_BatchEditEndEditingKeyFinalDefaultUnits);

                    string Grid_BatchEditStartEditingKeyDefaultUnits = "BatchEditStartEditingKeyDefaultUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingDefaultUnits, Grid_BatchEditStartEditingKeyDefaultUnits);

                    string Grid_BatchEditEndEditingKeyDefaultUnits = "BatchEditEndEditingKeyDefaultUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingDefaultUnits, Grid_BatchEditEndEditingKeyDefaultUnits);

                    string Grid_BatchEditStartEditingKeySurrogateUnits = "BatchEditStartEditingKeySurrogateUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingSurrogateUnits, Grid_BatchEditStartEditingKeySurrogateUnits);

                    string Grid_BatchEditEndEditingKeySurrogateUnits = "BatchEditEndEditingKeySurrogateUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingSurrogateUnits, Grid_BatchEditEndEditingKeySurrogateUnits);

                    if (View.Id == "QcParameter_ListView_DefaultSettings")
                    {
                        string Grid_BatchEditStartEditingKeySTDConcUnit = "BatchEditStartEditingKeySTDConcUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                        BatchEditStartEditingSTDConcUnit, Grid_BatchEditStartEditingKeySTDConcUnit);

                        string Grid_BatchEditEndEditingKeySTDConcUnit = "BatchEditEndEditingKeySTDConcUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                        BatchEditEndEditingSTDConcUnit, Grid_BatchEditEndEditingKeySTDConcUnit);

                        string Grid_BatchEditStartEditingKeySTDVolUnit = "BatchEditStartEditingKeySTDVolUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                        BatchEditStartEditingSTDVolUnit, Grid_BatchEditStartEditingKeySTDVolUnit);

                        string Grid_BatchEditEndEditingKeySTDVolUnit = "BatchEditEndEditingKeySTDVolUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                        BatchEditEndEditingSTDVolUnit, Grid_BatchEditEndEditingKeySTDVolUnit);

                        string Grid_BatchEditStartEditingKeySpikeAmountUnit = "BatchEditStartEditingKeySpikeAmountUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                        BatchEditStartEditingSpikeAmountUnit, Grid_BatchEditStartEditingKeySpikeAmountUnit);

                        string Grid_BatchEndEditStartEditingKeySpikeAmountUnit = "BatchEndEditStartEditingKeySpikeAmountUnit";
                        ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                        BatchEditEndEditingSpikeAmountUnit, Grid_BatchEndEditStartEditingKeySpikeAmountUnit);
                    }
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(QCTestParameter))
                {
                    string Grid_BatchEditStartEditingKeySTDConcUnit = "BatchEditStartEditingKeySTDConcUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingSTDConcUnit, Grid_BatchEditStartEditingKeySTDConcUnit);

                    string Grid_BatchEditEndEditingKeySTDConcUnit = "BatchEditEndEditingKeySTDConcUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingSTDConcUnit, Grid_BatchEditEndEditingKeySTDConcUnit);

                    string Grid_BatchEditStartEditingKeySTDVolUnit = "BatchEditStartEditingKeySTDVolUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingSTDVolUnit, Grid_BatchEditStartEditingKeySTDVolUnit);

                    string Grid_BatchEditEndEditingKeySTDVolUnit = "BatchEditEndEditingKeySTDVolUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingSTDVolUnit, Grid_BatchEditEndEditingKeySTDVolUnit);

                    string Grid_BatchEditStartEditingKeySpikeAmountUnit = "BatchEditStartEditingKeySpikeAmountUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing",
                    BatchEditStartEditingSpikeAmountUnit, Grid_BatchEditStartEditingKeySpikeAmountUnit);

                    string Grid_BatchEndEditStartEditingKeySpikeAmountUnit = "BatchEndEditStartEditingKeySpikeAmountUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing",
                    BatchEditEndEditingSpikeAmountUnit, Grid_BatchEndEditStartEditingKeySpikeAmountUnit);
                }

                if (View != null && View.Id == "SampleLogIn_ListView_SamplingAllocation")
                {
                    string Grid_BatchEditStartEditingKeyAssingBy = "BatchEditStartEditingKeyAssingdBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingAssingendBy, Grid_BatchEditStartEditingKeyAssingBy);

                    string Grid_BatchEditEndEditingKeyAssingBy = "BatchEditEndEditingKeyAssingBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingAssingnedBy, Grid_BatchEditEndEditingKeyAssingBy);
                }
                if (View != null && View.Id == "Requisition_ListView_Approve")
                {
                    string Grid_BatchEditStartEditingKeyApprovedBy = "BatchEditStartEditingKeyApprovedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingApprovedBy, Grid_BatchEditStartEditingKeyApprovedBy);

                    string Grid_BatchEditEndEditingKeyApprovedBy = "BatchEditEndEditingKeyApprovedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingApprovedBy, Grid_BatchEditEndEditingKeyApprovedBy);
                }
                else if (View != null && (View.Id == "Distribution_ListView" || View.Id == "Distribution_ListView_Viewmode"))
                {
                    string Grid_BatchEditStartEditingKeyDistributedBy = "BatchEditStartEditingKeyDistributedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingDistributedBy, Grid_BatchEditStartEditingKeyDistributedBy);

                    string Grid_BatchEditEndEditingKeyDistributedBy = "BatchEditEndEditingKeyDistributedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingDistributedBy, Grid_BatchEditEndEditingKeyDistributedBy);

                    string Grid_BatchEditStartEditingKeygivenby = "BatchEditStartEditingKeygivenby";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditinggivenby, Grid_BatchEditStartEditingKeygivenby);

                    string Grid_BatchEditEndEditingKeygivenby = "BatchEditEndEditingKeygivenby";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditinggivenby, Grid_BatchEditEndEditingKeygivenby);

                    string Grid_BatchEditStartEditingKeystorage = "BatchEditStartEditingKeystorage";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingstorage, Grid_BatchEditStartEditingKeystorage);

                    string Grid_BatchEditEndEditingKeystorage = "BatchEditEndEditingKeystorage";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingstorage, Grid_BatchEditEndEditingKeystorage);

                    string Grid_BatchEditStartEditingKeyemp = "BatchEditStartEditingKeyemp";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingemp, Grid_BatchEditStartEditingKeyemp);

                    string Grid_BatchEditEndEditingKeyemp = "BatchEditEndEditingKeyemp";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingemp, Grid_BatchEditEndEditingKeyemp);
                }
                else if (View != null && View.Id == "Distribution_ListView_Consumption")
                {
                    string Grid_BatchEditStartEditingKeyEnteredBy = "BatchEditStartEditingKeyEnteredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingEnteredBy, Grid_BatchEditStartEditingKeyEnteredBy);

                    string Grid_BatchEditEndEditingKeyEnteredBy = "BatchEditEndEditingKeyEnteredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingEnteredBy, Grid_BatchEditEndEditingKeyEnteredBy);

                    string Grid_BatchEditStartEditingKeyconsumption = "BatchEditStartEditingKeyconsumption";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingconsumption, Grid_BatchEditStartEditingKeyconsumption);

                    string Grid_BatchEditEndEditingKeyconsumption = "BatchEditEndEditingKeyconsumption";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingconsumption, Grid_BatchEditEndEditingKeyconsumption);
                }
                else if (View != null && View.Id == "Distribution_ListView_Disposal")
                {
                    string Grid_BatchEditStartEditingKeydisposal = "BatchEditStartEditingKeydisposal";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingdisposal, Grid_BatchEditStartEditingKeydisposal);

                    string Grid_BatchEditEndEditingKeydisposal = "BatchEditEndEditingKeydisposal";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingdisposal, Grid_BatchEditEndEditingKeydisposal);
                }
                else if (View != null && View.Id == "Requisition_ListView_Receive" || View.Id == "Requisition_ListView_DirectReceiveEntermode")
                {
                    string Grid_BatchEditStartEditingKeyReceivedBy = "BatchEditStartEditingKeyReceivedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingReceivedBy, Grid_BatchEditStartEditingKeyReceivedBy);

                    string Grid_BatchEditEndEditingKeyReceivedBy = "BatchEditEndEditingKeyReceivedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingReceivedBy, Grid_BatchEditEndEditingKeyReceivedBy);
                }
                else if (View != null && View.Id == "ExistingStock_ListView")
                {
                    //string Grid_BatchEditStartEditingKeyitems = "BatchEditStartEditingKeyitems";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingitems, Grid_BatchEditStartEditingKeyitems);

                    //string Grid_BatchEditEndEditingKeyitems = "BatchEditEndEditingKeyitems";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingitems, Grid_BatchEditEndEditingKeyitems);

                    //string Grid_BatchEditStartEditingKeyvendor = "BatchEditStartEditingKeyvendor";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingvendor, Grid_BatchEditStartEditingKeyvendor);

                    //string Grid_BatchEditEndEditingKeyvendor = "BatchEditEndEditingKeyvendor";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingvendor, Grid_BatchEditEndEditingKeyvendor);

                    string Grid_BatchEditStartEditingKeyemp = "BatchEditStartEditingKeyemp";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingemp, Grid_BatchEditStartEditingKeyemp);

                    string Grid_BatchEditEndEditingKeyemp = "BatchEditEndEditingKeyemp";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingemp, Grid_BatchEditEndEditingKeyemp);

                    string Grid_BatchEditStartEditingKeystorage = "BatchEditStartEditingKeystorage";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingstorage, Grid_BatchEditStartEditingKeystorage);

                    string Grid_BatchEditEndEditingKeystorage = "BatchEditEndEditingKeystorage";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingstorage, Grid_BatchEditEndEditingKeystorage);
                }
                else if (View != null && View.Id == "Requisition_ListViewEntermode")
                {
                    string Grid_BatchEditStartEditingKeyDeliveryPriority = "BatchEditStartEditingKeyDeliveryPriority";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingDeliveryPriority, Grid_BatchEditStartEditingKeyDeliveryPriority);

                    string Grid_BatchEditEndEditingKeyDeliveryPriority = "BatchEditEndEditingKeyDeliveryPriority";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingDeliveryPriority, Grid_BatchEditEndEditingKeyDeliveryPriority);

                    string Grid_BatchEditStartEditingKeyDepartment = "BatchEditStartEditingKeyDepartment";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingDepartment, Grid_BatchEditStartEditingKeyDepartment);

                    string Grid_BatchEditEndEditingKeyDepartment = "BatchEditEndEditingKeyDepartment";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingDepartment, Grid_BatchEditEndEditingKeyDepartment);
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Requisition))
                {
                    string Grid_BatchEditStartEditingKeyvendor = "BatchEditStartEditingKeyvendor";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingvendor, Grid_BatchEditStartEditingKeyvendor);

                    string Grid_BatchEditEndEditingKeyvendor = "BatchEditEndEditingKeyvendor";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingvendor, Grid_BatchEditEndEditingKeyvendor);

                    string Grid_BatchEditStartEditingKeybrand = "BatchEditStartEditingKeybrand";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingbrand, Grid_BatchEditStartEditingKeybrand);

                    string Grid_BatchEditEndEditingKeybrand = "BatchEditEndEditingKeybrand";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingbrand, Grid_BatchEditEndEditingKeybrand);

                    string Grid_BatchEditStartEditingKeyship = "BatchEditStartEditingKeyship";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingship, Grid_BatchEditStartEditingKeyship);

                    string Grid_BatchEditEndEditingKeyship = "BatchEditEndEditingKeyship";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingship, Grid_BatchEditEndEditingKeyship);

                    string Grid_BatchEditStartEditingKeyDeliveryPriority = "BatchEditStartEditingKeyDeliveryPriority";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingDeliveryPriority, Grid_BatchEditStartEditingKeyDeliveryPriority);

                    string Grid_BatchEditEndEditingKeyDeliveryPriority = "BatchEditEndEditingKeyDeliveryPriority";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingDeliveryPriority, Grid_BatchEditEndEditingKeyDeliveryPriority);

                    string Grid_BatchEditStartEditingKeyDepartment = "BatchEditStartEditingKeyDepartment";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingDepartment, Grid_BatchEditStartEditingKeyDepartment);

                    string Grid_BatchEditEndEditingKeyDepartment = "BatchEditEndEditingKeyDepartment";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingDepartment, Grid_BatchEditEndEditingKeyDepartment);

                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Distribution))
                {
                    string Grid_BatchEditStartEditingKeyEnteredBy = "BatchEditStartEditingKeyEnteredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingEnteredBy, Grid_BatchEditStartEditingKeyEnteredBy);

                    string Grid_BatchEditEndEditingKeyEnteredBy = "BatchEditEndEditingKeyEnteredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingEnteredBy, Grid_BatchEditEndEditingKeyEnteredBy);

                    string Grid_BatchEditStartEditingKeyconsumption = "BatchEditStartEditingKeyconsumption";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingconsumption, Grid_BatchEditStartEditingKeyconsumption);

                    string Grid_BatchEditEndEditingKeyconsumption = "BatchEditEndEditingKeyconsumption";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingconsumption, Grid_BatchEditEndEditingKeyconsumption);
                }
                else if (View != null && View.ObjectTypeInfo.Type == typeof(Distribution))
                {
                    string Grid_BatchEditStartEditingKeydisposal = "BatchEditStartEditingKeydisposal";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingdisposal, Grid_BatchEditStartEditingKeydisposal);

                    string Grid_BatchEditEndEditingKeydisposal = "BatchEditEndEditingKeydisposal";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingdisposal, Grid_BatchEditEndEditingKeydisposal);
                }
                else if (View != null && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                {
                    string Grid_BatchEditStartEditingKeyEnteredBy = "BatchEditStartEditingKeyEnteredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingEnteredBy, Grid_BatchEditStartEditingKeyEnteredBy);

                    string Grid_BatchEditEndEditingKeyEnteredBy = "BatchEditEndEditingKeyEnteredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingEnteredBy, Grid_BatchEditEndEditingKeyEnteredBy);

                    string Grid_BatchEditStartEditingKeyVM = "BatchEditStartEditingKeyVM";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingVM, Grid_BatchEditStartEditingKeyVM);

                    string Grid_BatchEditEndEditingKeyVM = "BatchEditEndEditingKeyVM";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingVM, Grid_BatchEditEndEditingKeyVM);

                    //string Grid_BatchEditStartEditingKeyCollector = "BatchEditStartEditingKeyCollector";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingCollector, Grid_BatchEditStartEditingKeyCollector);

                    //string Grid_BatchEditEndEditingKeyCollector = "BatchEditEndEditingKeyCollector";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingCollector, Grid_BatchEditEndEditingKeyCollector);

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSampleType, "Grid_BatchEditStartEditingKeySampleType");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSampleType, "Grid_BatchEditEndEditingKeySampleType");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingQCCategory, "Grid_BatchEditStartEditingKeyQCCategory");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingQCCategory, "Grid_BatchEditEndEditingKeyQCCategory");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSampleStatus, "Grid_BatchEditStartEditingKeySampleStatus");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSampleStatus, "Grid_BatchEditEndEditingKeySampleStatus");


                }
                else if (View != null && View.Id == "COCSettingsSamples_ListView_Copy_SampleRegistration")
                {
                    string Grid_BatchEditStartEditingKeyEnteredBy = "BatchEditStartEditingKeyEnteredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingEnteredBy, Grid_BatchEditStartEditingKeyEnteredBy);

                    string Grid_BatchEditEndEditingKeyEnteredBy = "BatchEditEndEditingKeyEnteredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingEnteredBy, Grid_BatchEditEndEditingKeyEnteredBy);

                    string Grid_BatchEditStartEditingKeyVM = "BatchEditStartEditingKeyVM";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingVM, Grid_BatchEditStartEditingKeyVM);

                    string Grid_BatchEditEndEditingKeyVM = "BatchEditEndEditingKeyVM";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingVM, Grid_BatchEditEndEditingKeyVM);

                    //string Grid_BatchEditStartEditingKeyCollector = "BatchEditStartEditingKeyCollector";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingCollector, Grid_BatchEditStartEditingKeyCollector);

                    //string Grid_BatchEditEndEditingKeyCollector = "BatchEditEndEditingKeyCollector";
                    //ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingCollector, Grid_BatchEditEndEditingKeyCollector);

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSampleType, "Grid_BatchEditStartEditingKeySampleType");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSampleType, "Grid_BatchEditEndEditingKeySampleType");
                }
                else if (View != null && View.Id == "Items_ListView_Copy")
                {
                    string Grid_BatchEditStartEditingKeyItemsCategory = "Grid_BatchEditStartEditingKeyItemsCategory";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingItemCategory, Grid_BatchEditStartEditingKeyItemsCategory);

                    string Grid_BatchEditEndEditingKeyitemsCategory = "Grid_BatchEditEndEditingKeyitemsCategory";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingItemCategory, Grid_BatchEditEndEditingKeyitemsCategory);

                    string Grid_BatchEditStartEditingKeyItemsGrade = "Grid_BatchEditStartEditingKeyItemsGrade";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingItemGrade, Grid_BatchEditStartEditingKeyItemsGrade);

                    string Grid_BatchEditEndEditingKeyitemsGrade = "Grid_BatchEditEndEditingKeyitemsGrade";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingItemGrade, Grid_BatchEditEndEditingKeyitemsGrade);

                    string Grid_BatchEditStartEditingKeyItemsUnit = "Grid_BatchEditStartEditingKeyItemsUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingItemUnit, Grid_BatchEditStartEditingKeyItemsUnit);

                    string Grid_BatchEditEndEditingKeyitemsUnit = "Grid_BatchEditEndEditingKeyitemsUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingItemUnit, Grid_BatchEditEndEditingKeyitemsUnit);

                    string Grid_BatchEditStartEditingKeyItemsManufacturer = "Grid_BatchEditStartEditingKeyItemsManufacturer";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingItemManufacturer, Grid_BatchEditStartEditingKeyItemsManufacturer);

                    string Grid_BatchEditEndEditingKeyitemsManufacturer = "Grid_BatchEditEndEditingKeyitemsManufacturer";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingItemManufacturer, Grid_BatchEditEndEditingKeyitemsManufacturer);

                    string Grid_BatchEditStartEditingKeyItemsAmountUnit = "Grid_BatchEditStartEditingKeyItemsAmountUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingItemAmountUnit, Grid_BatchEditStartEditingKeyItemsAmountUnit);

                    string Grid_BatchEditEndEditingKeyItemsAmountUnit = "Grid_BatchEditEndEditingKeyItemsAmountUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingItemAmountUnit, Grid_BatchEditEndEditingKeyItemsAmountUnit);

                    string Grid_BatchEditStartEditingKeyItemsVendor = "Grid_BatchEditStartEditingKeyItemsVendor";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingItemVendor, Grid_BatchEditStartEditingKeyItemsVendor);

                    string Grid_BatchEditEndEditingKeyitemsVendor = "Grid_BatchEditEndEditingKeyitemsVendor";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingItemVendor, Grid_BatchEditEndEditingKeyitemsVendor);

                }
                else if (View != null && View.Id == "SampleLogIn_DetailView_ProductSampleMapping")
                {
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSampleType, "Grid_BatchEditStartEditingKeySampleType");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSampleType, "Grid_BatchEditEndEditingKeySampleType");
                }
                    else if (View != null && View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling")
                {
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSampleStatus, "Grid_BatchEditStartEditingKeySampleStatus");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSampleStatus, "Grid_BatchEditEndEditingKeySampleStatus");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingCollector, "BatchEditStartEditingKeyCollector");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingCollector, "BatchEditEndEditingKeyCollector");
                }
                else if (View != null && View.Id == "SampleBottleAllocation_ListView_SampleTransfer")
                {
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingSampleStatus, "Grid_BatchEditStartEditingKeySampleStatus");

                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingSampleStatus, "Grid_BatchEditEndEditingKeySampleStatus");

                    string Grid_BatchEditStartEditingKeyReceivedBy = "BatchEditStartEditingKeyReceivedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingReceivedBy, Grid_BatchEditStartEditingKeyReceivedBy);

                    string Grid_BatchEditEndEditingKeyReceivedBy = "BatchEditEndEditingKeyReceivedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingReceivedBy, Grid_BatchEditEndEditingKeyReceivedBy);
                }
                //if (View != null && View.ObjectTypeInfo.Type == typeof(TestPriceDetail))
                //{
                //    string Grid_BatchEditStartEditingKeyTestPrice = "BatchEditStartEditingKeyTestPrice";
                //    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingTestPrice, Grid_BatchEditStartEditingKeyTestPrice);

                //    string Grid_BatchEditEndEditingKeyTestPrice = "BatchEditEndEditingKeyTestPrice";
                //    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingTestPrice, Grid_BatchEditEndEditingKeyTestPrice);
                //}
                else if (View.Id == "Tasks_Sampling_ListView_SampleTransfer")
                {
                    string Grid_BatchEditStartEditingKeyTransferredBy = "BatchEditStartEditingKeyTransferredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingTransferredBy, Grid_BatchEditStartEditingKeyTransferredBy);

                    string Grid_BatchEditEndEditingKeyTransferredBy = "BatchEditEndEditingKeyTransferredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingTransferredBy, Grid_BatchEditEndEditingKeyTransferredBy);
                }
                else if (View != null && View.Id == "Sampling_ListView_SamplingAssignment")
                {
                    string Grid_BatchEditStartEditingKeyAssignedBy = "BatchEditStartEditingKeyAssignedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingAssignedBy, Grid_BatchEditStartEditingKeyAssignedBy);

                    string Grid_BatchEditEndEditingKeyAssignedBy = "BatchEditEndEditingKeyAssignedBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingAssignedBy, Grid_BatchEditEndEditingKeyAssignedBy);
                }
                else if (View.Id == "BottlesOrder_BottlesOrderContainer_ListView")
                {
                    string Grid_BatchEditStartEditingKeyPackageUnits = "Grid_BatchEditStartEditingKeyPackageUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingItemPackageUnits, Grid_BatchEditStartEditingKeyPackageUnits);

                    string Grid_BatchEditEndEditingKeyitemsPackangeUnits = "Grid_BatchEditEndEditingKeyitemsPackageUnits";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingItemPackageUnits, Grid_BatchEditEndEditingKeyitemsPackangeUnits);

                   
                }
                else if (View.Id == "Tasks_BottlesOrders_ListView_DeliveryTasks" || View.Id == "ClientRequest_DeliveryBottleOrders_ListView_DeliveryTasks")
                {
                    string Grid_BatchEditStartEditingKeyDeliveredBy = "BatchEditStartEditingKeyDeliveredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingDeliveredBy, Grid_BatchEditStartEditingKeyDeliveredBy);

                    string Grid_BatchEditEndEditingKeyDeliveredBy = "BatchEditEndEditingKeyDeliveredBy";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingDeliveredBy, Grid_BatchEditEndEditingKeyDeliveredBy);
                }
                else if (View != null && View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                {
                    string Grid_BatchEditStartEditingKeyPriority = "BatchEditStartEditingKeyPriority";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingPriority, Grid_BatchEditStartEditingKeyPriority);

                    string Grid_BatchEditEndEditingKeyPriority = "BatchEditEndEditingKeyPriority";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingPriority, Grid_BatchEditEndEditingKeyPriority);

                  


                    string Grid_BatchEditStartEditingKeyCRMQuotesComponent = "BatchEditStartEditingKeyCRMQuotesComponent";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingKeyCRMQuotesComponent, Grid_BatchEditStartEditingKeyCRMQuotesComponent);

                    string Grid_BatchEditEndEditingKeyCRMQuotesComponent = "BatchEditEndEditingKeyCRMQuotesComponent";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingKeyCRMQuotesComponent, Grid_BatchEditEndEditingKeyCRMQuotesComponent);

                   

                }
                else if (View != null && View.Id == "ContainerSettings_ListView_testmethod")
                {
                    string Grid_BatchEditStartEditingKeyHTBeforeAnalysisObjects = "Grid_BatchEditStartEditingKeyHTBeforeAnalysisObjects";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingHTBeforeAnalysisObjects, Grid_BatchEditStartEditingKeyHTBeforeAnalysisObjects);

                    string Grid_BatchEditEndEditingKeyHTBeforeAnalysisObjects = "Grid_BatchEditEndEditingKeyHTBeforeAnalysisObjects";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingHTBeforeAnalysisObjects, Grid_BatchEditEndEditingKeyHTBeforeAnalysisObjects);

                    string Grid_BatchEditStartEditingKeyHTBeforePrepObjects = "Grid_BatchEditStartEditingKeyHTBeforePrepObjects";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingHTBeforePrepObjects, Grid_BatchEditStartEditingKeyHTBeforePrepObjects);

                    string Grid_BatchEditEndEditingKeyHTBeforePrepObjects = "Grid_BatchEditEndEditingKeyHTBeforePrepObjects";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingHTBeforePrepObjects, Grid_BatchEditEndEditingKeyHTBeforePrepObjects);
                }
                else if (View != null && View.Id == "TestMethod_QCTypes_ListView")
                {
                    string Grid_BatchEditStartEditingKeyQCrole = "Grid_BatchEditStartEditingKeyQCrole";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingQCrole, Grid_BatchEditStartEditingKeyQCrole);

                    string Grid_BatchEditEndEditingKeyQCrole = "Grid_BatchEditEndEditingKeyQCrole";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingQCrole, Grid_BatchEditEndEditingKeyQCrole);

                    string Grid_BatchEditStartEditingKeyQCRootRule = "Grid_BatchEditStartEditingKeyQCRootRule";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingQCRootRule, Grid_BatchEditStartEditingKeyQCRootRule);

                    string Grid_BatchEditEndEditingKeyQCRootRule = "Grid_BatchEditEndEditingKeyQCRootRule";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingQCRootRule, Grid_BatchEditEndEditingKeyQCRootRule);

                    string Grid_BatchEditStartEditingKeyQCSource = "Grid_BatchEditStartEditingKeyQCSource";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingQCSource, Grid_BatchEditStartEditingKeyQCSource);

                    string Grid_BatchEditEndEditingKeyQCSource = "Grid_BatchEditEndEditingKeyPrepObjects";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingQCSource, Grid_BatchEditEndEditingKeyQCSource);
                }
                else if (View != null && View.Id == "InvoicingAnalysisCharge_ListView_Queue" || View.Id == "Samplecheckin_CustomDueDates_ListView" || View.Id == "SamplingProposal_CustomDueDates_ListView")
                {
                    string Grid_BatchEditStartEditingTAT = "Grid_BatchEditStartEditingTAT";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingKeyTAT, Grid_BatchEditStartEditingTAT);

                    string Grid_BatchEditEndEditingKeyTAT = "Grid_BatchEditEndEditingKeyTAT";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingKeyTAT, Grid_BatchEditEndEditingKeyTAT);
                }
                else if (View != null && View.Id == "ReagentPreparation_ReagentPrepLogs_ListView_Calibration")
                {
                    string Grid_BatchEditStartEditingWUnit = "Grid_BatchEditStartEditingWUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingKeyWUnit, Grid_BatchEditStartEditingWUnit);
                    string Grid_BatchEditEndEditingKeyWUnit = "Grid_BatchEditEndEditingKeyWUnit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingKeyWUnit, Grid_BatchEditEndEditingKeyWUnit);

                    string Grid_BatchEditStartEditingV1Unit = "Grid_BatchEditStartEditingV1Unit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingKeyV1Unit, Grid_BatchEditStartEditingV1Unit);
                    string Grid_BatchEditEndEditingKeyV1Unit = "Grid_BatchEditEndEditingKeyV1Unit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingKeyV1Unit, Grid_BatchEditEndEditingKeyV1Unit);

                    string Grid_BatchEditStartEditingV2Unit = "Grid_BatchEditStartEditingV2Unit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingKeyV2Unit, Grid_BatchEditStartEditingV2Unit);
                    string Grid_BatchEditEndEditingKeyV2Unit = "Grid_BatchEditEndEditingKeyV2Unit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingKeyV2Unit, Grid_BatchEditEndEditingKeyV2Unit);

                    string Grid_BatchEditStartEditingC2Unit = "Grid_BatchEditStartEditingC2Unit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingKeyC2Unit, Grid_BatchEditStartEditingC2Unit);
                    string Grid_BatchEditEndEditingKeyC2Unit = "Grid_BatchEditEndEditingKeyC2Unit";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingKeyC2Unit, Grid_BatchEditEndEditingKeyC2Unit);
                }
                else if (View != null && View.Id == "COCSettings_ListView_SamplingProposal_ImportCOC")
                {
                    string Grid_BatchEditStartEditingKeyProjectID = "BatchEditStartEditingKeyProjectID";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditStartEditing", BatchEditStartEditingProjectID, Grid_BatchEditStartEditingKeyProjectID);

                    string Grid_BatchEditEndEditingKeyProjectID = "BatchEditEndEditingKeyProjectID";
                    ClientSideEventsHelper.AssignClientHandlerSafe(gridListEditor.Grid, "BatchEditEndEditing", BatchEditEndEditingProjectID, Grid_BatchEditEndEditingKeyProjectID);
                }
            }
        }

        protected override void OnDeactivated()
        {
            try
            {
                if (View != null && View.Editor != null && View.CollectionSource != null)
                {
                    ASPxGridListEditor listEditor = (ASPxGridListEditor)View.Editor;
                    listEditor.CreateCustomEditItemTemplate -= listEditor_CreateCustomEditItemTemplate;
                    listEditor.CreateCustomGridViewDataColumn -= listEditor_CreateCustomGridViewDataColumn;
                    // Unsubscribe from previously subscribed events and release other references and resources.
                    listEditor.BatchEditModeHelper.CustomUpdateBatchValue -= BatchValueIsUpdated;
                    base.OnDeactivated();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        #endregion

        #region Function
        private void listEditor_CreateCustomGridViewDataColumn(object sender, CreateCustomGridViewDataColumnEventArgs e)
        {
            try
            {
                if (e.ModelColumn.PropertyEditorType == typeof(ASPxLookupPropertyEditor))
                {
                    //if (e.ModelColumn.PropertyName == "TAT")
                    //{
                    //    var gridColumn = new GridViewDataComboBoxColumn();
                    //    gridColumn.Name = e.ModelColumn.PropertyName;
                    //    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    //    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    //    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    //    gridColumn.PropertiesComboBox.TextField = "TAT";
                    //    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.TestPriceDetail>();
                    //    e.Column = gridColumn;
                    //}
                    if (e.ModelColumn.PropertyName == "FiberType")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "FiberType";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<FiberTypes>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "HTBeforePrep")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "HoldingTime";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<HoldingTimes>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "HTBeforeAnalysis")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "HoldingTime";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<HoldingTimes>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Units")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "SampleparameterUnit")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Priority")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Prioritys";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Priority>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "department")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Name";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Department>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "QcRole")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "QC_Role";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<QcRole>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "QCRootRole")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "QCRoot_Role";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<QCRootRole>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "QCSource")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "QC_Source";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<QCSource>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "FinalDefaultUnits")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "DefaultUnits")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "SurrogateUnits")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "FinalResultUnits")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "STDConcUnit")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "STDVolUnit")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "SpikeAmountUnit")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Storage")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "storage";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ICMStorage>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "givento")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Itemname")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "items";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.ICM.Items>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "ConsumptionBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "DisposedBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "AnalyzedBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "ApprovedBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "AssignedBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "ValidatedBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "ReceivedBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "EnteredBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "DistributedBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "GivenBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    //if (e.ModelColumn.PropertyName == "Vendor")
                    //{
                    //    var gridColumn = new GridViewDataComboBoxColumn();
                    //    gridColumn.Name = e.ModelColumn.PropertyName;
                    //    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    //    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    //    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    //    gridColumn.PropertiesComboBox.TextField = "Vendor";
                    //    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ICM.Module.BusinessObjects.Vendors>();
                    //    e.Column = gridColumn;
                    //}
                    if (e.ModelColumn.PropertyName == "ShippingOption")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "option";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ICM.Module.BusinessObjects.Shippingoptions>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Manufacturer")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "ManufacturerName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.ICM.Manufacturer>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "SubOutBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "VisualMatrix")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "VisualMatrixName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.VisualMatrix>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "QCCategory")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "QCCategoryName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<QCCategory>();
                        e.Column = gridColumn;
                    }
                    //if (e.ModelColumn.PropertyName == "Matrix" && View.Id == "BottlesOrder_BottlesOrderContainer_ListView")
                    //{
                    //    var gridColumn = new GridViewDataComboBoxColumn();
                    //    gridColumn.Name = e.ModelColumn.PropertyName;
                    //    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    //    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    //    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    //    gridColumn.PropertiesComboBox.TextField = "VisualMatrixName";
                    //    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Setting.VisualMatrix>();
                    //    e.Column = gridColumn;
                    //}
                    if (e.ModelColumn.PropertyName == "Collector" && View.Id != "SampleLogIn_ListView_Copy_SampleRegistration" && View.Id != "SampleLogIn_ListView_FieldDataEntry_Sampling")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.Hr.Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "SampleStatus")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Samplestatus";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<SampleStatus>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Collector" && View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "FullName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Collector>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Category" && e.ModelColumn.GetType() == typeof(Modules.BusinessObjects.SampleManagement.Category))
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "category";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Category>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Category" && e.ModelColumn.GetType() == typeof(ICM.Module.BusinessObjects.Category))
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "category";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ICM.Module.BusinessObjects.Category>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Grade")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Grade";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Grades>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Unit" || e.ModelColumn.PropertyName == "Units")
                    {
                        if (View.ObjectTypeInfo.Type != typeof(SampleParameter))
                        {
                            var gridColumn = new GridViewDataComboBoxColumn();
                            gridColumn.Name = e.ModelColumn.PropertyName;
                            gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                            gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                            gridColumn.PropertiesComboBox.ValueField = "Oid";
                            gridColumn.PropertiesComboBox.TextField = "Option";
                            gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Packageunits>();
                            e.Column = gridColumn;
                        }
                        else
                        {
                            var gridColumn = new GridViewDataComboBoxColumn();
                            gridColumn.Name = e.ModelColumn.PropertyName;
                            gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                            gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                            gridColumn.PropertiesComboBox.ValueField = "Oid";
                            gridColumn.PropertiesComboBox.TextField = "UnitName";
                            gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Unit>();
                            e.Column = gridColumn;
                        }
                        //if (View.Id != "SampleParameter_ListView_Copy_QCResultEntry" && View.Id != "SampleParameter_ListView_Copy_ResultEntry" && View.Id == "SampleParameter_ListView_Copy_ResultValidation" && View.Id == "SampleParameter_ListView_Copy_QCResultValidation" && View.Id == "SampleParameter_ListView_Copy_QCResultApproval" && View.Id == "SampleParameter_ListView_Copy_ResultApproval")
                        //{
                        //    var gridColumn = new GridViewDataComboBoxColumn();
                        //    gridColumn.Name = e.ModelColumn.PropertyName;
                        //    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        //    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        //    gridColumn.PropertiesComboBox.ValueField = "Oid";
                        //    gridColumn.PropertiesComboBox.TextField = "Option";
                        //    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Packageunits>();
                        //    e.Column = gridColumn;
                        //}
                    }
                    if (e.ModelColumn.PropertyName == "Vendor")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Vendor";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Vendors>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "AmountUnit")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "UnitName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Unit>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "SampleType")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "SampleTypeName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<SampleType>();
                        e.Column = gridColumn;
                    }
                   
                    if (e.ModelColumn.PropertyName == "TAT")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "TAT";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<TurnAroundTime>();
                        e.Column = gridColumn;
                    }
                    //if (e.ModelColumn.PropertyName == "AssignedBy")
                    //{
                    //    var gridColumn = new GridViewDataComboBoxColumn();
                    //    gridColumn.Name = e.ModelColumn.PropertyName;
                    //    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    //    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    //    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    //    gridColumn.PropertiesComboBox.TextField = "DisplayName";
                    //    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Employee>();
                    //    e.Column = gridColumn;
                    //}
                    //if (e.ModelColumn.PropertyName == "AssignToSampleAllocation")
                    //{
                    //    var gridColumn = new GridViewDataComboBoxColumn();
                    //    gridColumn.Name = e.ModelColumn.PropertyName;
                    //    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    //    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    //    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    //    gridColumn.PropertiesComboBox.TextField = "DisplayName";
                    //    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Employee>();
                    //    e.Column = gridColumn;
                    //}
                    if (e.ModelColumn.PropertyName == "TransferredBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "DeliveredBy")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "DisplayName";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Employee>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "Component")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Components";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Component>();
                        e.Column = gridColumn;
                    }
                    if (e.ModelColumn.PropertyName == "ProjectID" && View.Id == "COCSettings_ListView_SamplingProposal_ImportCOC")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "ProjectId";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Project>();
                        e.Column = gridColumn;
                    }
                    if (View.Id == "ReagentPreparation_ReagentPrepLogs_ListView_Calibration" && e.ModelColumn.PropertyName == "WSCons_Units")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Units";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ReagentUnits>();
                        e.Column = gridColumn;
                    }
                    if (View.Id == "ReagentPreparation_ReagentPrepLogs_ListView_Calibration" && e.ModelColumn.PropertyName == "Cal_VolTaken_V1_Units")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Units";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ReagentUnits>();
                        e.Column = gridColumn;
                    }
                    if (View.Id == "ReagentPreparation_ReagentPrepLogs_ListView_Calibration" && e.ModelColumn.PropertyName == "Cal_FinalVol_V2_Units")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Units";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ReagentUnits>();
                        e.Column = gridColumn;
                    }
                    if (View.Id == "ReagentPreparation_ReagentPrepLogs_ListView_Calibration" && e.ModelColumn.PropertyName == "Cal_FinalConc_C2_Units")
                    {
                        var gridColumn = new GridViewDataComboBoxColumn();
                        gridColumn.Name = e.ModelColumn.PropertyName;
                        gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                        gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                        gridColumn.PropertiesComboBox.ValueField = "Oid";
                        gridColumn.PropertiesComboBox.TextField = "Units";
                        gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ReagentUnits>();
                        e.Column = gridColumn;
                    }

                    ////if (e.ModelColumn.PropertyName == "Test" && View.Id == "CRMQuotes_AnalysisPricing_ListView")
                    ////{
                    ////    var gridColumn = new GridViewDataComboBoxColumn();
                    ////    gridColumn.Name = e.ModelColumn.PropertyName;
                    ////    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    ////    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    ////    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    ////    gridColumn.PropertiesComboBox.TextField = "TestName";
                    ////    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<TestMethod>();
                    ////    e.Column = gridColumn;
                    ////}
                    ////if (e.ModelColumn.PropertyName == "Method" && View.Id == "CRMQuotes_AnalysisPricing_ListView")
                    ////{
                    ////    var gridColumn = new GridViewDataComboBoxColumn();
                    ////    gridColumn.Name = e.ModelColumn.PropertyName;
                    ////    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    ////    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    ////    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    ////    gridColumn.PropertiesComboBox.TextField = "MethodNumber";
                    ////    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<Method>();
                    ////    e.Column = gridColumn;
                    ////}
                    //if (e.ModelColumn.PropertyName == "Conclusion")
                    //{
                    //    var gridColumn = new GridViewDataComboBoxColumn();
                    //    gridColumn.Name = e.ModelColumn.PropertyName;
                    //    gridColumn.FieldName = e.ModelColumn.PropertyName + ".Oid";
                    //    gridColumn.PropertiesComboBox.ValueType = typeof(int?);
                    //    gridColumn.PropertiesComboBox.ValueField = "Oid";
                    //    gridColumn.PropertiesComboBox.TextField = "Option";
                    //    //ITypeInfo typeInfo = XafTypesInfo.Instance.FindTypeInfo(View.CurrentObject.GetType());
                    //    //IMemberInfo memberInfo = typeInfo.FindMember(this.Model.DataSourceProperty);
                    //    //IList list = (IList)memberInfo.GetValue(View.CurrentObject);
                    //    gridColumn.PropertiesComboBox.DataSource = ObjectSpace.GetObjects<ClauseOptions>();
                    //    e.Column = gridColumn;
                    //}
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void listEditor_CreateCustomEditItemTemplate(object sender, CreateCustomEditItemTemplateEventArgs e)
        {
            try
            {
                if (e.ModelColumn.PropertyName == "FiberType")
                {
                    IEnumerable<FiberTypes> referencedObjectsList = ObjectSpace.CreateCollection(typeof(FiberTypes), null, new SortProperty[] { new SortProperty("FiberType", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<FiberTypes>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "FiberType");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Units" && View.Id != "BottlesOrder_BottlesOrderContainer_ListView")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList);
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Units" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"))
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Units");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "SampleparameterUnit")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "SampleparameterUnit");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "HTBeforeAnalysis")
                {
                    IEnumerable<HoldingTimes> referencedObjectsList = ObjectSpace.CreateCollection(typeof(HoldingTimes), null, new SortProperty[] { new SortProperty("HoldingTime", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<HoldingTimes>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "HTBeforeAnalysis");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "HTBeforePrep")
                {
                    IEnumerable<HoldingTimes> referencedObjectsList = ObjectSpace.CreateCollection(typeof(HoldingTimes), null, new SortProperty[] { new SortProperty("HoldingTime", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<HoldingTimes>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "HTBeforePrep");
                    e.Handled = true;
                }


                if (e.ModelColumn.PropertyName == "FinalDefaultUnits")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "FinalDefaultUnits");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "DefaultUnits")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "DefaultUnits");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "SurrogateUnits")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "SurrogateUnits");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "FinalResultUnits")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "FinalResultUnits");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "STDConcUnit")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "STDConcUnit");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "STDVolUnit")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "STDVolUnit");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "SpikeAmountUnit")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "SpikeAmountUnit");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Storage")
                {
                    IEnumerable<Modules.BusinessObjects.ICM.ICMStorage> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ICMStorage), null, new SortProperty[] { new SortProperty("storage", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ICMStorage>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Storage");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "givento")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "givento");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Itemname")
                {
                    IEnumerable<Modules.BusinessObjects.ICM.Items> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.ICM.Items), null, new SortProperty[] { new SortProperty("items", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.ICM.Items>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Itemname");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "DistributedBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "DistributedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "AnalyzedBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "AnalyzedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "GivenBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "GivenBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "ReceivedBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "ReceivedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "AssignedBy" && View != null && View.ObjectTypeInfo != null && View.ObjectTypeInfo.Type != typeof(SampleLogIn))
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "AssignedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "ApprovedBy" && View.GetType() != typeof(SampleParameter))
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "ApprovedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "ApprovedBy" && View.GetType() == typeof(SampleParameter))
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "ApprovedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "ValidatedBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "ValidatedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "EnteredBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "EnteredBy");
                    e.Handled = true;
                }
                //if (e.ModelColumn.PropertyName == "Vendor")
                //{
                //    IEnumerable<ICM.Module.BusinessObjects.Vendors> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ICM.Module.BusinessObjects.Vendors), null, new SortProperty[] { new SortProperty("Vendor", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ICM.Module.BusinessObjects.Vendors>();
                //    e.Template = new ReferencedTemplate(referencedObjectsList, "Vendor");
                //    e.Handled = true;
                //}
                if (e.ModelColumn.PropertyName == "ShippingOption")
                {
                    IEnumerable<ICM.Module.BusinessObjects.Shippingoptions> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ICM.Module.BusinessObjects.Shippingoptions), null, new SortProperty[] { new SortProperty("option", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ICM.Module.BusinessObjects.Shippingoptions>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "ShippingOption");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Priority")
                {
                    IEnumerable<Priority> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Priority), null, new SortProperty[] { new SortProperty("Prioritys", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Priority>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Priority");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "DeliveryPriority")
                {
                    //IEnumerable<DeliveryPriority> referencedObjectsList = ObjectSpace.CreateCollection(typeof(DeliveryPriority), null, new SortProperty[] { new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<DeliveryPriority>();
                    IEnumerable<DeliveryPriority> referencedObjectsList = ObjectSpace.CreateCollection(typeof(DeliveryPriority), null, new SortProperty[] { new SortProperty("Sort", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<DeliveryPriority>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "DeliveryPriority");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "department")
                {
                    IEnumerable<Department> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Department), null, new SortProperty[] { new SortProperty("Name", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Department>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "department");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "QcRole")
                {
                    IEnumerable<QcRole> referencedObjectsList = ObjectSpace.CreateCollection(typeof(QcRole), null, new SortProperty[] { new SortProperty("QC_Role", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<QcRole>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "QcRole");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "QCRootRole")
                {
                    IEnumerable<QCRootRole> referencedObjectsList = ObjectSpace.CreateCollection(typeof(QCRootRole), null, new SortProperty[] { new SortProperty("QCRoot_Role", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<QCRootRole>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "QCRootRole");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "QCSource")
                {
                    IEnumerable<QCSource> referencedObjectsList = ObjectSpace.CreateCollection(typeof(QCSource), null, new SortProperty[] { new SortProperty("QC_Source", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<QCSource>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "QCSource");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Manufacturer")
                {
                    IEnumerable<Modules.BusinessObjects.ICM.Manufacturer> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.ICM.Manufacturer), null, new SortProperty[] { new SortProperty("ManufacturerName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.ICM.Manufacturer>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Manufacturer");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "ConsumptionBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "ConsumptionBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "DisposedBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "DisposedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "SubOutBy")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "SubOutBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "VisualMatrix")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.VisualMatrix> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.VisualMatrix), CriteriaOperator.Parse("[IsRetired] <> True Or [IsRetired] Is Null"), new SortProperty[] { new SortProperty("VisualMatrixName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.VisualMatrix>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "VisualMatrix");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "QCCategory")
                {
                    IEnumerable<QCCategory> referencedObjectsList = ObjectSpace.CreateCollection(typeof(QCCategory), null, new SortProperty[] { new SortProperty("QCCategoryName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<QCCategory>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "QCCategory");
                    e.Handled = true;
                }
                //if (e.ModelColumn.PropertyName == "Matrix")
                //{
                //    IEnumerable<Modules.BusinessObjects.Setting.VisualMatrix> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.VisualMatrix), null, new SortProperty[] { new SortProperty("VisualMatrixName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.VisualMatrix>();
                //    e.Template = new ReferencedTemplate(referencedObjectsList, "Matrix");
                //    e.Handled = true;
                //}
                if (e.ModelColumn.PropertyName == "Collector" && View.Id != "SampleLogIn_ListView_Copy_SampleRegistration" && View.Id != "SampleLogIn_ListView_FieldDataEntry_Sampling")
                {
                    IEnumerable<Modules.BusinessObjects.Hr.Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Hr.Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Hr.Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Collector");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Collector" && View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling")
                {
                    IList<Collector> referencedObjectsList = ObjectSpace.GetObjects<Collector>(CriteriaOperator.Parse("[DontShow] != 1 AND [CustomerName.CustomerName]=?", TMInfo.ClientName)).ToList();
                    foreach (Collector col in referencedObjectsList)
                    {
                        IEnumerable<Collector> coltempList = referencedObjectsList.Where(a => a.FirstName.ToLower().Replace(" ", "") == col.FullName.ToLower().Replace(" ", ""));
                        if (coltempList.Count() > 1)
                        {
                            foreach (Collector collist in coltempList.Where(a => a != coltempList.First()))
                            {
                                referencedObjectsList.Remove(collist);
                            }
                        }
                    }
                    e.Template = new ReferencedTemplate(referencedObjectsList.OrderBy(a => a.FullName), "Collector");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Category" && e.ModelColumn.GetType() == typeof(Modules.BusinessObjects.SampleManagement.Category))
                {
                    IEnumerable<Modules.BusinessObjects.SampleManagement.Category> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.SampleManagement.Category), null, new SortProperty[] { new SortProperty("category", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.SampleManagement.Category>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Category");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Grade")
                {
                    IEnumerable<Grades> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Grades), null, new SortProperty[] { new SortProperty("Grade", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Grades>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Grade");
                    e.Handled = true;
                }
                if ((e.ModelColumn.PropertyName == "Unit" || e.ModelColumn.PropertyName == "Units") && View.ObjectTypeInfo.Type != typeof(SampleParameter))
                {
                    //if(View.Id != "SampleParameter_ListView_Copy_QCResultEntry" && View.Id != "SampleParameter_ListView_Copy_ResultEntry" && View.Id == "SampleParameter_ListView_Copy_ResultValidation" && View.Id == "SampleParameter_ListView_Copy_QCResultValidation" && View.Id == "SampleParameter_ListView_Copy_QCResultApproval" && View.Id == "SampleParameter_ListView_Copy_ResultApproval")
                    //{
                    IEnumerable<Packageunits> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Packageunits), null, new SortProperty[] { new SortProperty("Option", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Packageunits>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Unit");
                    e.Handled = true;
                    //}

                }
                if (e.ModelColumn.PropertyName == "Vendor")
                {
                    IEnumerable<Vendors> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Vendors), null, new SortProperty[] { new SortProperty("Vendor", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Vendors>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Vendor");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "AmountUnit")
                {
                    IEnumerable<Unit> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Unit), null, new SortProperty[] { new SortProperty("UnitName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Unit>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "AmountUnit");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "SampleType")
                {
                    IEnumerable<SampleType> referencedObjectsList = ObjectSpace.CreateCollection(typeof(SampleType), null, new SortProperty[] { new SortProperty("SampleTypeName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<SampleType>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "SampleType");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "SampleStatus")
                {
                    IEnumerable<SampleStatus> referencedObjectsList = ObjectSpace.CreateCollection(typeof(SampleStatus), null, new SortProperty[] { new SortProperty("Samplestatus", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<SampleStatus>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "SampleStatus");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "TAT")
                {
                    IEnumerable<Modules.BusinessObjects.Setting.TurnAroundTime> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Modules.BusinessObjects.Setting.TurnAroundTime), null, new SortProperty[] { new SortProperty("TAT", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Modules.BusinessObjects.Setting.TurnAroundTime>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "TAT");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "AssignedBy" && View.Id == "SampleLogIn_ListView_SamplingAllocation")
                {
                    IEnumerable<Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "AssignedBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "TransferredBy")
                {
                    IEnumerable<Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "TransferredBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "DeliveredBy")
                {
                    IEnumerable<Employee> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Employee), null, new SortProperty[] { new SortProperty("DisplayName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Employee>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "DeliveredBy");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Component")
                {
                    IEnumerable<Component> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Component), null, new SortProperty[] { new SortProperty("Components", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Component>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Component");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "ProjectID" && View.Id == "COCSettings_ListView_SamplingProposal_ImportCOC")
                {
                    IEnumerable<Project> referencedObjectsList = ObjectSpace.CreateCollection(typeof(Project), null, new SortProperty[] { new SortProperty("ProjectId", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<Project>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "ProjectID");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "WSCons_Units")
                {
                    IEnumerable<ReagentUnits> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ReagentUnits), null, new SortProperty[] { new SortProperty("Units", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ReagentUnits>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "WSCons_Units");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Cal_VolTaken_V1_Units")
                {
                    IEnumerable<ReagentUnits> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ReagentUnits), null, new SortProperty[] { new SortProperty("Units", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ReagentUnits>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Cal_VolTaken_V1_Units");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Cal_FinalVol_V2_Units")
                {
                    IEnumerable<ReagentUnits> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ReagentUnits), null, new SortProperty[] { new SortProperty("Units", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ReagentUnits>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Cal_FinalVol_V2_Units");
                    e.Handled = true;
                }
                if (e.ModelColumn.PropertyName == "Cal_FinalConc_C2_Units")
                {
                    IEnumerable<ReagentUnits> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ReagentUnits), null, new SortProperty[] { new SortProperty("Units", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ReagentUnits>();
                    e.Template = new ReferencedTemplate(referencedObjectsList, "Cal_FinalConc_C2_Units");
                    e.Handled = true;
                }



                ////if (e.ModelColumn.PropertyName == "Test" && View.Id == "CRMQuotes_AnalysisPricing_ListView")
                ////{
                ////    IEnumerable<TestMethod> referencedObjectsList = ObjectSpace.CreateCollection(typeof(TestMethod), null, new SortProperty[] { new SortProperty("TestName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<TestMethod>();
                ////    e.Template = new ReferencedTemplate(referencedObjectsList, "Test");
                ////    e.Handled = true;
                ////}
                ////if (e.ModelColumn.PropertyName == "Method" && View.Id == "CRMQuotes_AnalysisPricing_ListView")
                ////{
                ////    IEnumerable<TestMethod> referencedObjectsList = ObjectSpace.CreateCollection(typeof(TestMethod), null, new SortProperty[] { new SortProperty("MethodName.MethodNumber", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<TestMethod>();
                ////    e.Template = new ReferencedTemplate(referencedObjectsList, "Method");
                ////    e.Handled = true;
                ////}
                //if (e.ModelColumn.PropertyName == "Test" && View.Id == "BottlesOrder_BottlesOrderContainer_ListView")
                //{
                //    IEnumerable<TestMethod> referencedObjectsList = ObjectSpace.CreateCollection(typeof(TestMethod), CriteriaOperator.Parse("[MatrixName.MatrixName]=?", BOInfo.Matrix), new SortProperty[] { new SortProperty("TestName", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<TestMethod>();
                //    e.Template = new ReferencedTemplate(referencedObjectsList, "Test");
                //    e.Handled = true;
                //}
                //if (e.ModelColumn.PropertyName == "Conclusion")
                //{
                //    IEnumerable<ClauseOptions> referencedObjectsList = ObjectSpace.CreateCollection(typeof(ClauseOptions), null, new SortProperty[] { new SortProperty("Option", DevExpress.Xpo.DB.SortingDirection.Ascending) }).Cast<ClauseOptions>();
                //    e.Template = new ReferencedTemplate(referencedObjectsList, e.ModelColumn.PropertyName);
                //    e.Handled = true;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected void BatchValueIsUpdated(object sender, CustomUpdateBatchValueEventArgs e)
        {
            try
            {
                if (e.PropertyName == "FiberType.Oid")
                {
                    var exampleObject = e.Object as PLMExam;
                    if (e.NewValue == null)
                    {
                        exampleObject.FiberType = null;
                    }
                    else
                    {
                        exampleObject.FiberType = exampleObject.Session.GetObjectByKey<FiberTypes>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Itemname.StockQty")
                {
                    //var exampleObject = e.Object as SampleParameter;
                    //if (e.NewValue == null)
                    //{
                    //    exampleObject.Units = null;
                    //}
                    //else
                    //{
                    //    exampleObject.Units = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                    //}
                    e.Handled = true;
                }
                if (e.PropertyName == "Units.Oid" && View.Id != "BottlesOrder_BottlesOrderContainer_ListView")
                {
                    var exampleObject = e.Object as SampleParameter;
                    if (e.NewValue == null)
                    {
                        exampleObject.Units = null;
                    }
                    else
                    {
                        exampleObject.Units = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                    }
                    e.Handled = true;
                }
                //if (e.PropertyName == "Units.Oid" && View.Id == "BottlesOrder_BottlesOrderContainer_ListView")
                //{
                //    var exampleObject = e.Object as BottleOrderContainer;
                //    if (e.NewValue == null)
                //    {
                //        exampleObject.Units = null;
                //    }
                //    else
                //    {
                //        exampleObject.Units = exampleObject.Session.GetObjectByKey<Packageunits>(e.NewValue);
                //    }
                //    e.Handled = true;
                //}
                if (e.PropertyName == "FinalDefaultUnits.Oid")
                {
                    var exampleObject = e.Object as Testparameter;
                    if (e.NewValue == null)
                    {
                        exampleObject.FinalDefaultUnits = null;
                    }
                    else
                    {
                        exampleObject.FinalDefaultUnits = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "DefaultUnits.Oid")
                {
                    var exampleObject = e.Object as Testparameter;
                    if (e.NewValue == null)
                    {
                        exampleObject.DefaultUnits = null;
                    }
                    else
                    {
                        exampleObject.DefaultUnits = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "SurrogateUnits.Oid")
                {
                    if (e.Object.GetType() == typeof(Testparameter))
                    {
                        var exampleObject = e.Object as Testparameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.SurrogateUnits = null;
                        }
                        else
                        {
                            exampleObject.SurrogateUnits = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                    else if (e.Object.GetType() == typeof(SampleParameter))
                    {
                        var exampleObject = e.Object as SampleParameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.SurrogateUnits = null;
                        }
                        else
                        {
                            exampleObject.SurrogateUnits = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }

                }
                if (e.PropertyName == "FinalResultUnits.Oid")
                {
                    var exampleObject = e.Object as SampleParameter;
                    if (e.NewValue == null)
                    {
                        exampleObject.FinalResultUnits = null;
                    }
                    else
                    {
                        exampleObject.FinalResultUnits = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "STDConcUnit.Oid")
                {
                    if (e.Object.GetType() == typeof(Testparameter))
                    {
                        var exampleObject = e.Object as Testparameter;

                        if (e.NewValue == null)
                        {
                            exampleObject.STDConcUnit = null;
                        }
                        else
                        {
                            exampleObject.STDConcUnit = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                    else
                    if (e.Object.GetType() == typeof(QCTestParameter))
                    {
                        var exampleObject = e.Object as QCTestParameter;

                        if (e.NewValue == null)
                        {
                            exampleObject.STDConcUnit = null;
                        }
                        else
                        {
                            exampleObject.STDConcUnit = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                }
                if (e.PropertyName == "STDVolUnit.Oid")
                {
                    if (e.Object.GetType() == typeof(QCTestParameter))
                    {
                        var exampleObject = e.Object as QCTestParameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.STDVolUnit = null;
                        }
                        else
                        {
                            exampleObject.STDVolUnit = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                    else
                    if (e.Object.GetType() == typeof(Testparameter))
                    {
                        var exampleObject = e.Object as Testparameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.STDVolUnit = null;
                        }
                        else
                        {
                            exampleObject.STDVolUnit = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                }
                if (e.PropertyName == "SpikeAmountUnit.Oid")
                {
                    if (e.Object.GetType() == typeof(QCTestParameter))
                    {
                        var exampleObject = e.Object as QCTestParameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.SpikeAmountUnit = null;
                        }
                        else
                        {
                            exampleObject.SpikeAmountUnit = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                    else if (e.Object.GetType() == typeof(Testparameter))
                    {
                        var exampleObject = e.Object as Testparameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.SpikeAmountUnit = null;
                        }
                        else
                        {
                            exampleObject.SpikeAmountUnit = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                    else if (e.Object.GetType() == typeof(SampleParameter))
                    {
                        var exampleObject = e.Object as SampleParameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.SpikeAmountUnit = null;
                        }
                        else
                        {
                            exampleObject.SpikeAmountUnit = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                }
                if (e.PropertyName == "Storage.Oid")
                {
                    if (View.Id != null && View.Id == "ExistingStock_ListView")
                    {
                        var exampleObject = e.Object as ExistingStock;
                        if (e.NewValue == null)
                        {
                            exampleObject.Storage = null;
                        }
                        else
                        {
                            exampleObject.Storage = exampleObject.Session.GetObjectByKey<ICMStorage>(e.NewValue);
                        }
                    }
                    else
                    {
                        var exampleObject = e.Object as Distribution;
                        if (e.NewValue == null)
                        {
                            exampleObject.Storage = null;
                        }
                        else
                        {
                            exampleObject.Storage = exampleObject.Session.GetObjectByKey<ICMStorage>(e.NewValue);
                        }
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "givento.Oid")
                {
                    if (View.Id != null && View.Id == "ExistingStock_ListView")
                    {
                        var exampleObject = e.Object as ExistingStock;
                        if (e.NewValue == null)
                        {
                            exampleObject.givento = null;
                        }
                        else
                        {
                            exampleObject.givento = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                    }
                    else
                    {
                        var exampleObject = e.Object as Distribution;
                        if (e.NewValue == null)
                        {
                            exampleObject.givento = null;
                        }
                        else
                        {
                            exampleObject.givento = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Itemname.Oid")
                {
                    var exampleObject = e.Object as ExistingStock;
                    if (e.NewValue == null)
                    {
                        exampleObject.Itemname = null;
                    }
                    else
                    {
                        exampleObject.Itemname = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.ICM.Items>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "DistributedBy.Oid")
                {
                    var exampleObject = e.Object as Distribution;
                    if (e.NewValue == null)
                    {
                        exampleObject.DistributedBy = null;
                    }
                    else
                    {
                        exampleObject.DistributedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "AnalyzedBy.Oid")
                {
                    var exampleObject = e.Object as SampleParameter;
                    if (e.NewValue == null)
                    {
                        exampleObject.AnalyzedBy = null;
                    }
                    else
                    {
                        exampleObject.AnalyzedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "ApprovedBy.Oid")
                {
                    if (View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval")
                    {
                        var exampleObject = e.Object as SampleParameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.ApprovedBy = null;
                        }
                        else
                        {
                            exampleObject.ApprovedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                    else
                    {
                        var exampleObject = e.Object as Requisition;
                        if (e.NewValue == null)
                        {
                            exampleObject.ApprovedBy = null;
                        }
                        else
                        {
                            exampleObject.ApprovedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                }
                if (e.PropertyName == "AssignedBy.Oid" && View != null && View.ObjectTypeInfo != null && View.ObjectTypeInfo.Type == typeof(SampleLogIn))
                {
                    var exampleObject = e.Object as SampleLogIn;
                    if (e.NewValue == null)
                    {
                        exampleObject.AssignedBy = null;
                    }
                    else
                    {
                        exampleObject.AssignedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "ValidatedBy.Oid")
                {
                    var exampleObject = e.Object as SampleParameter;
                    if (e.NewValue == null)
                    {
                        exampleObject.ValidatedBy = null;
                    }
                    else
                    {
                        exampleObject.ValidatedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "GivenBy.Oid")
                {
                    var exampleObject = e.Object as Distribution;
                    if (e.NewValue == null)
                    {
                        exampleObject.GivenBy = null;
                    }
                    else
                    {
                        exampleObject.GivenBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "ReceivedBy.Oid")
                {
                    if (View.Id == "SampleBottleAllocation_ListView_SampleTransfer")
                    {
                        var exampleObject = e.Object as SampleBottleAllocation;
                        if (e.NewValue == null)
                        {
                            exampleObject.ReceivedBy = null;
                        }
                        else
                        {
                            exampleObject.ReceivedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                    }
                    else
                    {
                        var exampleObject = e.Object as Requisition;
                        if (e.NewValue == null)
                        {
                            exampleObject.ReceivedBy = null;
                        }
                        else
                        {
                            exampleObject.ReceivedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "EnteredBy.Oid")
                {
                    if (View != null && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"))
                    {
                        var exampleObject = e.Object as SampleParameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.EnteredBy = null;
                        }
                        else
                        {
                            exampleObject.EnteredBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                    }
                    else if (View != null && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                    {
                        var exampleObject = e.Object as SampleLogIn;
                        if (e.NewValue == null)
                        {
                            exampleObject.EnteredBy = null;
                        }
                        else
                        {
                            exampleObject.EnteredBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                    }
                    else
                    {
                        var exampleObject = e.Object as Distribution;
                        if (e.NewValue == null)
                        {
                            exampleObject.EnteredBy = null;
                        }
                        else
                        {
                            exampleObject.EnteredBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        }
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Vendor.Oid")
                {
                    if (View.Id != null && View.Id == "ExistingStock_ListView")
                    {
                        var exampleObject = e.Object as ExistingStock;
                        if (e.NewValue == null)
                        {
                            exampleObject.Vendor = null;
                        }
                        else
                        {
                            exampleObject.Vendor = exampleObject.Session.GetObjectByKey<ICM.Module.BusinessObjects.Vendors>(e.NewValue);
                        }
                    }
                    else if (View.Id == "Items_ListView_Copy")
                    {
                        var exampleObject = e.Object as Items;
                        if (e.NewValue == null)
                        {
                            exampleObject.Vendor = null;
                        }
                        else
                        {
                            exampleObject.Vendor = exampleObject.Session.GetObjectByKey<Vendors>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                    else
                    {
                        var exampleObject = e.Object as ICM.Module.BusinessObjects.Requisition;
                        if (e.NewValue == null)
                        {
                            exampleObject.Vendor = null;
                        }
                        else
                        {
                            exampleObject.Vendor = exampleObject.Session.GetObjectByKey<ICM.Module.BusinessObjects.Vendors>(e.NewValue);
                        }
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "ShippingOption.Oid")
                {
                    var exampleObject = e.Object as ICM.Module.BusinessObjects.Requisition;
                    if (e.NewValue == null)
                    {
                        exampleObject.ShippingOption = null;
                    }
                    else
                    {
                        exampleObject.ShippingOption = exampleObject.Session.GetObjectByKey<ICM.Module.BusinessObjects.Shippingoptions>(e.NewValue);
                    }
                    e.Handled = true;
                }
                //if (e.PropertyName == "department.Oid")
                //{
                //    var exampleObject = e.Object as Requisition;
                //    if (e.NewValue == null)
                //    {
                //        exampleObject.department = null;
                //    }
                //    else
                //    {
                //        exampleObject.department = exampleObject.Session.GetObjectByKey<Department>(e.NewValue);
                //    }
                //    e.Handled = true;
                //}
                if (e.PropertyName == "QcRole.Oid")
                {
                    var exampleObject = e.Object as QCType;
                    if (e.NewValue == null)
                    {
                        exampleObject.QcRole = null;
                    }
                    else
                    {
                        exampleObject.QcRole = exampleObject.Session.GetObjectByKey<QcRole>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "QCRootRole.Oid")
                {
                    var exampleObject = e.Object as QCType;
                    if (e.NewValue == null)
                    {
                        exampleObject.QCRootRole = null;
                    }
                    else
                    {
                        exampleObject.QCRootRole = exampleObject.Session.GetObjectByKey<QCRootRole>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "QCSource.Oid")
                {
                    var exampleObject = e.Object as QCType;
                    if (e.NewValue == null)
                    {
                        exampleObject.QCSource = null;
                    }
                    else
                    {
                        exampleObject.QCSource = exampleObject.Session.GetObjectByKey<QCSource>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Manufacturer.Oid")
                {
                    if (View.GetType() == typeof(Requisition))
                    {
                        var exampleObject = e.Object as ICM.Module.BusinessObjects.Requisition;
                        if (e.NewValue == null)
                        {
                            exampleObject.Manufacturer = null;
                        }
                        else
                        {
                            exampleObject.Manufacturer = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.ICM.Manufacturer>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                    else if (View.Id == "Items_ListView_Copy")
                    {
                        var exampleObject = e.Object as Items;
                        if (e.NewValue == null)
                        {
                            exampleObject.Manufacturer = null;
                        }
                        else
                        {
                            exampleObject.Manufacturer = exampleObject.Session.GetObjectByKey<Manufacturer>(e.NewValue);
                        }
                        e.Handled = true;
                    }

                    //e.Handled = true;
                }
                if (e.PropertyName == "ConsumptionBy.Oid")
                {
                    var exampleObject = e.Object as Distribution;
                    if (e.NewValue == null)
                    {
                        exampleObject.ConsumptionBy = null;
                    }
                    else
                    {
                        exampleObject.ConsumptionBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "DisposedBy.Oid")
                {
                    var exampleObject = e.Object as Distribution;
                    if (e.NewValue == null)
                    {
                        exampleObject.DisposedBy = null;
                    }
                    else
                    {
                        exampleObject.DisposedBy = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "VisualMatrix.Oid")
                {
                    var exampleObject = e.Object as SampleLogIn;
                    var exampleCOCObject = e.Object as COCSettingsSamples;
                    if (e.NewValue == null && exampleObject != null)
                    {
                        exampleObject.VisualMatrix = null;
                    }
                    else if (e.NewValue == null && exampleCOCObject != null)
                    {
                        exampleCOCObject.VisualMatrix = null;
                    }
                    else if (exampleObject != null)
                    {
                        exampleObject.VisualMatrix = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.VisualMatrix>(e.NewValue);
                    }
                    else if (exampleCOCObject != null)
                    {
                        exampleCOCObject.VisualMatrix = exampleCOCObject.Session.GetObjectByKey<VisualMatrix>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "QCCategory.Oid")
                {
                    var exampleObject = e.Object as SampleLogIn;
                    if (e.NewValue == null)
                    {
                        exampleObject.QCCategory = null;
                        //exampleObject.ExcludeInvoice = false;
                    }
                    else
                    {
                        exampleObject.QCCategory = exampleObject.Session.GetObjectByKey<QCCategory>(e.NewValue);
                        //exampleObject.ExcludeInvoice = true;
                    }
                    e.Handled = true;
                }
                //if (e.PropertyName == "Matrix.Oid" && View.Id == "BottlesOrder_BottlesOrderContainer_ListView")
                //{
                //    var exampleObject = e.Object as BottleOrderContainer;
                //    if (e.NewValue == null)
                //    {
                //        exampleObject.Matrix = null;
                //    }
                //    else
                //    {
                //        exampleObject.Matrix = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Setting.VisualMatrix>(e.NewValue);
                //    }
                //    e.Handled = true;
                //}
                if (e.PropertyName == "Collector.Oid" && View.Id != "SampleLogIn_ListView_Copy_SampleRegistration")
                {
                    var exampleObject = e.Object as SampleLogIn;
                    if (e.NewValue == null)
                    {
                        exampleObject.Collector = null;
                    }
                    else
                    {
                        //exampleObject.Collector = exampleObject.Session.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(e.NewValue);
                        exampleObject.Collector = exampleObject.Session.GetObjectByKey<Collector>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Category.Oid")
                {
                    var exampleObject = e.Object as Items;
                    if (e.NewValue == null)
                    {
                        exampleObject.Category = null;
                    }
                    else
                    {
                        exampleObject.Category = exampleObject.Session.GetObjectByKey<ICM.Module.BusinessObjects.Category>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Grade.Oid")
                {
                    var exampleObject = e.Object as Items;
                    if (e.NewValue == null)
                    {
                        exampleObject.Grade = null;
                    }
                    else
                    {
                        exampleObject.Grade = exampleObject.Session.GetObjectByKey<Grades>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Unit.Oid")
                {
                    var exampleObject = e.Object as Items;
                    if (e.NewValue == null)
                    {
                        exampleObject.Unit = null;
                    }
                    else
                    {
                        exampleObject.Unit = exampleObject.Session.GetObjectByKey<Packageunits>(e.NewValue);
                    }
                    e.Handled = true;
                }
                //if (e.PropertyName == "Units.Oid" && View.Id == "BottlesOrder_BottlesOrderContainer_ListView")
                //{
                //    var exampleObject = e.Object as BottleOrderContainer;
                //    if (e.NewValue == null)
                //    {
                //        exampleObject.Units = null;
                //    }
                //    else
                //    {
                //        exampleObject.Units = exampleObject.Session.GetObjectByKey<Packageunits>(e.NewValue);
                //    }
                //    e.Handled = true;
                //}
                if (e.PropertyName == "AmountUnit.Oid")
                {
                    var exampleObject = e.Object as Items;
                    if (e.NewValue == null)
                    {
                        exampleObject.AmountUnit = null;
                    }
                    else
                    {
                        exampleObject.AmountUnit = exampleObject.Session.GetObjectByKey<Unit>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Units.Oid")
                {
                    //if (View.Id == "BottlesOrder_BottlesOrderContainer_ListView")
                    //{
                    //    var exampleObject = e.Object as BottleOrderContainer;
                    //    if (e.NewValue == null)
                    //    {
                    //        exampleObject.Units = null;
                    //    }
                    //    else
                    //    {
                    //        exampleObject.Units = exampleObject.Session.GetObjectByKey<Packageunits>(e.NewValue);
                    //    }
                    //    e.Handled = true;
                    //}
                    //else
                    {
                        var exampleObject = e.Object as SampleParameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.Units = null;
                        }
                        else
                        {
                            exampleObject.Units = exampleObject.Session.GetObjectByKey<Unit>(e.NewValue);
                        }
                        e.Handled = true;
                    }
                }
                if (e.PropertyName == "SampleStatus.Oid")
                {
                    var exampleObject = e.Object as SampleBottleAllocation;
                    if (e.NewValue == null)
                    {
                        exampleObject.SampleStatus = null;
                    }
                    else
                    {
                        exampleObject.SampleStatus = exampleObject.Session.GetObjectByKey<SampleStatus>(e.NewValue);
                    }
                }
                if (e.PropertyName == "SampleStatus.Oid")
                {
                   
                    if (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                    {
                        var exampleObject = e.Object as SampleLogIn;
                        if (e.NewValue == null)
                        {
                            exampleObject.SampleStatus = null;
                        }
                        else
                        {
                            exampleObject.SampleStatus = exampleObject.Session.GetObjectByKey<SampleStatus>(e.NewValue);
                        }
                    }
                }

                if (e.PropertyName == "SampleType.Oid")
                {
                    var exampleObject = e.Object as SampleLogIn;
                    var exampleCOCObject = e.Object as COCSettingsSamples;
                    if (e.NewValue == null)
                    {
                        exampleObject.SampleType = null;
                    }
                    else if (e.NewValue == null && exampleCOCObject != null)
                    {
                        exampleCOCObject.SampleType = null;
                    }
                    else if (exampleCOCObject != null)
                    {
                        exampleCOCObject.SampleType = exampleCOCObject.Session.GetObjectByKey<SampleType>(e.NewValue);
                    }
                    else if (exampleObject != null)
                    {
                        exampleObject.SampleType = exampleObject.Session.GetObjectByKey<SampleType>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "TAT.Oid")
                {
                    if (View.Id == "Samplecheckin_CustomDueDates_ListView" || View.Id == "SamplingProposal_CustomDueDates_ListView")
                    {
                        var exampleObject = e.Object as CustomDueDate;
                        if (e.NewValue == null)
                        {
                            exampleObject.TAT = null;
                        }
                        else
                        {
                            exampleObject.TAT = exampleObject.Session.GetObjectByKey<TurnAroundTime>(e.NewValue);
                        }
                    }

                    if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin_Copy" || View.Id == "Testparameter_LookupListView_Sampling_SeletectedTest")
                    {
                        var exampleObject = e.Object as Testparameter;
                        if (e.NewValue == null)
                        {
                            exampleObject.TAT = null;
                        }
                        else
                        {
                            exampleObject.TAT = exampleObject.Session.GetObjectByKey<TurnAroundTime>(e.NewValue);
                        }
                    }
                    e.Handled = true;
                }

                ////if (e.PropertyName == "Test.Oid" && View.Id == "CRMQuotes_AnalysisPricing_ListView")
                ////{
                ////    var exampleObject = e.Object as AnalysisPricing;
                ////    if (e.NewValue == null)
                ////    {
                ////        exampleObject.Test = null;
                ////    }
                ////    else
                ////    {
                ////        exampleObject.Test = exampleObject.Session.GetObjectByKey<TestMethod>(e.NewValue);
                ////    }
                ////    e.Handled = true;
                ////}
                ////if (e.PropertyName == "Method.Oid" && View.Id == "CRMQuotes_AnalysisPricing_ListView")
                ////{
                ////    var exampleObject = e.Object as AnalysisPricing;
                ////    if (e.NewValue == null)
                ////    {
                ////        exampleObject.Method = null;
                ////    }
                ////    else
                ////    {
                ////        exampleObject.Method = exampleObject.Session.GetObjectByKey<TestMethod>(e.NewValue);
                ////    }
                ////    e.Handled = true;
                ////}
                if (e.PropertyName == "SampleparameterUnit.Oid")
                {
                    var exampleObject = e.Object as SampleParameter;
                    if (e.NewValue == null)
                    {
                        exampleObject.SampleparameterUnit = null;
                    }
                    else
                    {
                        exampleObject.SampleparameterUnit = exampleObject.Session.GetObjectByKey<Unit>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "HTBeforeAnalysis.Oid")
                {
                    var exampleObject = e.Object as ContainerSettings;
                    if (e.NewValue == null)
                    {
                        exampleObject.HTBeforeAnalysis = null;
                    }
                    else
                    {
                        exampleObject.HTBeforeAnalysis = exampleObject.Session.GetObjectByKey<HoldingTimes>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "HTBeforePrep.Oid")
                {
                    var exampleObject = e.Object as ContainerSettings;
                    if (e.NewValue == null)
                    {
                        exampleObject.HTBeforePrep = null;
                    }
                    else
                    {
                        exampleObject.HTBeforePrep = exampleObject.Session.GetObjectByKey<HoldingTimes>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "ProjectID.Oid" && View.Id == "COCSettings_ListView_SamplingProposal_ImportCOC")
                {
                    var exampleObject = e.Object as COCSettings;
                    if (e.NewValue == null)
                    {
                        exampleObject.ProjectID = null;
                    }
                    else
                    {
                        exampleObject.ProjectID = exampleObject.Session.GetObjectByKey<Project>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "WSCons_Units")
                {
                    var exampleObject = e.Object as ReagentPrepLog;
                    if (e.NewValue == null)
                    {
                        exampleObject.WSCons_Units = null;
                    }
                    else
                    {
                        exampleObject.WSCons_Units = exampleObject.Session.GetObjectByKey<ReagentUnits>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Cal_VolTaken_V1_Units")
                {
                    var exampleObject = e.Object as ReagentPrepLog;
                    if (e.NewValue == null)
                    {
                        exampleObject.Cal_VolTaken_V1_Units = null;
                    }
                    else
                    {
                        exampleObject.Cal_VolTaken_V1_Units = exampleObject.Session.GetObjectByKey<ReagentUnits>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Cal_FinalVol_V2_Units")
                {
                    var exampleObject = e.Object as ReagentPrepLog;
                    if (e.NewValue == null)
                    {
                        exampleObject.Cal_FinalVol_V2_Units = null;
                    }
                    else
                    {
                        exampleObject.Cal_FinalVol_V2_Units = exampleObject.Session.GetObjectByKey<ReagentUnits>(e.NewValue);
                    }
                    e.Handled = true;
                }
                if (e.PropertyName == "Cal_FinalConc_C2_Units")
                {
                    var exampleObject = e.Object as ReagentPrepLog;
                    if (e.NewValue == null)
                    {
                        exampleObject.Cal_FinalConc_C2_Units = null;
                    }
                    else
                    {
                        exampleObject.Cal_FinalConc_C2_Units = exampleObject.Session.GetObjectByKey<ReagentUnits>(e.NewValue);
                    }
                    e.Handled = true;
                }


                //if (e.PropertyName == "Test.Oid")
                //{
                //    var exampleObject = e.Object as BottleOrderContainer;
                //    if (e.NewValue == null)
                //    {
                //        exampleObject.Test = null;
                //    }
                //    else
                //    {
                //        exampleObject.Test = exampleObject.Session.GetObjectByKey<TestMethod>(e.NewValue);
                //    }
                //    e.Handled = true;
                //}
                //if (e.PropertyName == "Conclusion.Oid")
                //{
                //    var exampleObject = e.Object as ClauseInspection;
                //    if (e.NewValue == null)
                //    {
                //        exampleObject.Conclusion = null;
                //    }
                //    else
                //    {
                //        exampleObject.Conclusion = exampleObject.Session.GetObjectByKey<ClauseOptions>(e.NewValue);
                //    }
                //    e.Handled = true;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        #endregion

        const string BatchEditStartEditingFiberType =
          @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('FiberType.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    ClientFiberType.SetValue(cellInfo.value);                   
                    ClientFiberType.SetText(cellInfo.text);
                }
                    ClientFiberType['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    ClientFiberType.SetFocus();
                }
            }";

        const string BatchEditEndEditingFiberType =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('FiberType.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = ClientFiberType.GetValue();
                cellInfo.text = ClientFiberType.GetText();       
                ClientFiberType.SetValue(null);
            }";

        const string BatchEditStartEditingHTBeforePrepObjects =
          @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('HTBeforePrep.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientHTBeforePrep.SetValue(cellInfo.value);                   
                    clientHTBeforePrep.SetText(cellInfo.text);
                }
                    clientHTBeforePrep['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientHTBeforePrep.SetFocus();
                }
            }";

        const string BatchEditEndEditingHTBeforePrepObjects =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('HTBeforePrep.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientHTBeforePrep.GetValue();
                cellInfo.text = clientHTBeforePrep.GetText();       
                clientHTBeforePrep.SetValue(null);
            }";


        const string BatchEditStartEditingHTBeforeAnalysisObjects =
           @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('HTBeforeAnalysis.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientHTBeforeAnalysis.SetValue(cellInfo.value);                   
                    clientHTBeforeAnalysis.SetText(cellInfo.text);
                }
                    clientHTBeforeAnalysis['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientHTBeforeAnalysis.SetFocus();
                }
            }";

        const string BatchEditEndEditingHTBeforeAnalysisObjects =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('HTBeforeAnalysis.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientHTBeforeAnalysis.GetValue();
                cellInfo.text = clientHTBeforeAnalysis.GetText();       
                clientHTBeforeAnalysis.SetValue(null);
            }";


        const string BatchEditStartEditingItemCategory =
            @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('Category.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientItemCategory.SetValue(cellInfo.value);                   
                    clientItemCategory.SetText(cellInfo.text);
                }
                    clientItemCategory['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientItemCategory.SetFocus();
                }
            }";

        const string BatchEditEndEditingItemCategory =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('Category.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientItemCategory.GetValue();
                cellInfo.text = clientItemCategory.GetText();       
                clientItemCategory.SetValue(null);
            }";

        const string BatchEditStartEditingItemGrade =
            @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('Grade.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientItemGrade.SetValue(cellInfo.value);                   
                    clientItemGrade.SetText(cellInfo.text);
                }
                    clientItemGrade['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientItemGrade.SetFocus();
                }
            }";

        const string BatchEditEndEditingItemGrade =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('Grade.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientItemGrade.GetValue();
                cellInfo.text = clientItemGrade.GetText();       
                clientItemGrade.SetValue(null);
            }";

        const string BatchEditStartEditingItemUnit =
            @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('Unit.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientItemUnit.SetValue(cellInfo.value);                   
                    clientItemUnit.SetText(cellInfo.text);
                }
                    clientItemUnit['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientItemUnit.SetFocus();
                }
            }";

        const string BatchEditEndEditingItemUnit =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('Unit.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientItemUnit.GetValue();
                cellInfo.text = clientItemUnit.GetText();       
                clientItemUnit.SetValue(null);
            }";

        const string BatchEditStartEditingItemManufacturer =
            @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('Manufacturer.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientManufacturer.SetValue(cellInfo.value);
                    clientManufacturer.SetText(cellInfo.text);
                }
                clientManufacturer['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientManufacturer.SetFocus();
                }
            }";

        const string BatchEditEndEditingItemManufacturer =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('Manufacturer.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientManufacturer.GetValue();
                cellInfo.text = clientManufacturer.GetText();       
                clientManufacturer.SetValue(null);
            }";

        const string BatchEditStartEditingItemAmountUnit =
            @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('AmountUnit.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientItemAmountUnit.SetValue(cellInfo.value);                   
                    clientItemAmountUnit.SetText(cellInfo.text);
                }
                    clientItemAmountUnit['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientItemAmountUnit.SetFocus();
                }
            }";

        const string BatchEditEndEditingItemAmountUnit =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('AmountUnit.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientItemAmountUnit.GetValue();
                cellInfo.text = clientItemAmountUnit.GetText();       
                clientItemAmountUnit.SetValue(null);
            }";
        const string BatchEditStartEditingItemUnitsSampleparameter =
          @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('SampleparameterUnit.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;       
                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientUnit.SetValue(cellInfo.value);                   
                    clientUnit.SetText(cellInfo.text);
                }
                    clientUnit['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientUnit.SetFocus();
                }
            }";

        const string BatchEditEndEditingItemUnitsSampleparameter =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('SampleparameterUnit.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientUnit.GetValue();
                cellInfo.text = clientUnit.GetText();       
                clientUnit.SetValue(null);
            }";

        const string BatchEditStartEditingItemVendor =
            @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('Vendor.Oid');        
                
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;

                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientVendor.SetValue(cellInfo.value);                   
                    clientVendor.SetText(cellInfo.text);
                }
                    clientVendor['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientVendor.SetFocus();
                }
            }";

        const string BatchEditEndEditingItemVendor =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('Vendor.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientVendor.GetValue();
                cellInfo.text = clientVendor.GetText();       
                clientVendor.SetValue(null);
            }";

        const string BatchEditStartEditingitems =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('Itemname.Oid');

            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
            
            var cellInfo = e.rowValues[productNameColumn.index];
            if(cellInfo.value != null && cellInfo.text != null){
            clientItemname.SetValue(cellInfo.value);                   
            clientItemname.SetText(cellInfo.text);
            }
            clientItemname['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientItemname.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell.
        const string BatchEditEndEditingitems =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('Itemname.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            var Itemdataspl = clientItemname.GetValue().split('#');
            if(Itemdataspl[0] != s.batchEditApi.GetCellValue(e.visibleIndex, 'Itemname.Oid')){
            cellInfo.value = Itemdataspl[0];            
            if(Itemdataspl[1] != null){
            s.batchEditApi.SetCellValue(e.visibleIndex, 'Itemname.StockQty', Itemdataspl[1]);
            }
            if(Itemdataspl[2] != null){
            s.batchEditApi.SetCellValue(e.visibleIndex, 'Vendor.Oid', Itemdataspl[3], Itemdataspl[2]);
            }
            else{
            s.batchEditApi.SetCellValue(e.visibleIndex, 'Vendor', null);
            }
            }
            cellInfo.text = clientItemname.GetText();   
            clientItemname.SetValue(null);
            s.SelectRowOnPage(e.visibleIndex);
        }";

        const string BatchEditStartEditingEnteredBy =
      @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('EnteredBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientEnteredBy.SetText(cellInfo.text);
            clientEnteredBy.SetValue(cellInfo.value);
            clientEnteredBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientEnteredBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingEnteredBy =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('EnteredBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientEnteredBy.GetValue();
            cellInfo.text = clientEnteredBy.GetText();       
            clientEnteredBy.SetValue(null);
        }";

        const string BatchEditStartEditingAnalyzedBy =
     @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('AnalyzedBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientAnalyzedBy.SetText(cellInfo.text);
            clientAnalyzedBy.SetValue(cellInfo.value);
            clientAnalyzedBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientAnalyzedBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingAnalyzedBy =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('AnalyzedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientAnalyzedBy.GetValue();
            cellInfo.text = clientAnalyzedBy.GetText();       
            clientAnalyzedBy.SetValue(null);
        }";

        const string BatchEditStartEditingResultValidatedBy =
     @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('ValidatedBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientResultValidatedBy.SetText(cellInfo.text);
            clientResultValidatedBy.SetValue(cellInfo.value);
            clientResultValidatedBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientResultValidatedBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingResultValidatedBy =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('ValidatedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientResultValidatedBy.GetValue();
            cellInfo.text = clientResultValidatedBy.GetText();       
            clientResultValidatedBy.SetValue(null);
        }";

        const string BatchEditStartEditingResultApprovedBy =
     @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('ApprovedBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientResultApprovedBy.SetText(cellInfo.text);
            clientResultApprovedBy.SetValue(cellInfo.value);
            clientResultApprovedBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientResultApprovedBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingResultApprovedBy =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('ApprovedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientResultApprovedBy.GetValue();
            cellInfo.text = clientResultApprovedBy.GetText();       
            clientResultApprovedBy.SetValue(null);
        }";


        const string BatchEditStartEditingApprovedBy =
      @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('ApprovedBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientApprovedBy.SetText(cellInfo.text);
            clientApprovedBy.SetValue(cellInfo.value);
            clientApprovedBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientApprovedBy.SetFocus();
            }       
        }";
        const string BatchEditEndEditingApprovedBy =
          @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('ApprovedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientApprovedBy.GetValue();
            cellInfo.text = clientApprovedBy.GetText();       
            clientApprovedBy.SetValue(null);
        }";
        const string BatchEditStartEditingAssingendBy =
     @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('AssignedBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientAssignedBy.SetText(cellInfo.text);
            clientAssignedBy.SetValue(cellInfo.value);
            clientAssignedBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientAssignedBy.SetFocus();
            }       
        }";

        const string BatchEditEndEditingAssingnedBy =
         @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('AssignedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientAssignedBy.GetValue();
            cellInfo.text = clientAssignedBy.GetText();       
            clientAssignedBy.SetValue(null);
        }";

        //Handle the event to pass the value from the editor to the grid cell. 


        const string BatchEditStartEditingVM =
      @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('VisualMatrix.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];               
            clientVisualMatrix.SetText(cellInfo.text);
            clientVisualMatrix.SetValue(cellInfo.value);
            clientVisualMatrix['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientVisualMatrix.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingVM =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('VisualMatrix.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientVisualMatrix.GetValue();
            cellInfo.text = clientVisualMatrix.GetText();       
            clientVisualMatrix.SetValue(null);
        }";

        const string BatchEditStartEditingQCCategory =
    @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('QCCategory.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];               
            clientQCCategory.SetText(cellInfo.text);
            clientQCCategory.SetValue(cellInfo.value);
            clientQCCategory['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientQCCategory.SetFocus();
            }       
        }";

        const string BatchEditEndEditingQCCategory =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('QCCategory.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientQCCategory.GetValue();
            cellInfo.text = clientQCCategory.GetText();       
            clientQCCategory.SetValue(null);
        }";

        const string BatchEditStartEditingCollector =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('Collector.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientCollector.SetText(cellInfo.text);
            clientCollector.SetValue(cellInfo.value);
            clientCollector['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientCollector.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingCollector =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('Collector.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientCollector.GetValue();
            cellInfo.text = clientCollector.GetText();       
            clientCollector.SetValue(null);
        }";


        const string BatchEditStartEditingReceivedBy =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('ReceivedBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientReceiveby.SetText(cellInfo.text);
            clientReceiveby.SetValue(cellInfo.value);
            clientReceiveby['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientReceiveby.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingReceivedBy =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('ReceivedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientReceiveby.GetValue();
            cellInfo.text = clientReceiveby.GetText();       
            clientReceiveby.SetValue(null);
        }";

        const string BatchEditStartEditingDistributedBy =
        @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('DistributedBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientDistributedBy.SetText(cellInfo.text);
            clientDistributedBy.SetValue(cellInfo.value);
            clientDistributedBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientDistributedBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingDistributedBy =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('DistributedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientDistributedBy.GetValue();
            cellInfo.text = clientDistributedBy.GetText();       
            clientDistributedBy.SetValue(null);
        }";

        const string BatchEditStartEditinggivenby =
        @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('GivenBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientGivenBy.SetText(cellInfo.text);
            clientGivenBy.SetValue(cellInfo.value);
            clientGivenBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientGivenBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditinggivenby =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('GivenBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientGivenBy.GetValue();
            cellInfo.text = clientGivenBy.GetText();       
            clientGivenBy.SetValue(null);
        }";

        const string BatchEditStartEditingdisposal =
        @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('DisposedBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientHandledBy.SetText(cellInfo.text);
            clientHandledBy.SetValue(cellInfo.value);
            clientHandledBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientHandledBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingdisposal =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('DisposedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientHandledBy.GetValue();
            cellInfo.text = clientHandledBy.GetText();       
            clientHandledBy.SetValue(null);
        }";



        const string BatchEditStartEditingconsumption =
        @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('ConsumptionBy.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientConsumptionBy.SetText(cellInfo.text);
            clientConsumptionBy.SetValue(cellInfo.value);
            clientConsumptionBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientConsumptionBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingconsumption =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('ConsumptionBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientConsumptionBy.GetValue();
            cellInfo.text = clientConsumptionBy.GetText();       
            clientConsumptionBy.SetValue(null);
        }";

        const string BatchEditStartEditingbrand =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('Manufacturer.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientManufacturer.SetText(cellInfo.text);
            clientManufacturer.SetValue(cellInfo.value);
            clientManufacturer['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientManufacturer.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingbrand =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('Manufacturer.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientManufacturer.GetValue();
            cellInfo.text = clientManufacturer.GetText();       
            clientManufacturer.SetValue(null);
        }";

        const string BatchEditStartEditingship =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('ShippingOption.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientship.SetText(cellInfo.text);
            clientship.SetValue(cellInfo.value);
            clientship['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientship.SetFocus();
            }       
        }";

        const string BatchEditStartEditingDeliveryPriority =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('DeliveryPriority.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientdeliverypriority.SetText(cellInfo.text);
            clientdeliverypriority.SetValue(cellInfo.value);
            clientdeliverypriority['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientdeliverypriority.SetFocus();
            }       
        }";

        const string BatchEditStartEditingDepartment =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('department.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientdepartment.SetText(cellInfo.text);
            clientdepartment.SetValue(cellInfo.value);
            clientdepartment['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientdepartment.SetFocus();
            }       
        }";
        const string BatchEditEndEditingDepartment =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('department.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientdepartment.GetValue();
            cellInfo.text = clientdepartment.GetText();       
            clientdepartment.SetValue(null);
        }";

        const string BatchEditStartEditingQCrole =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('QcRole.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientColQcRole.SetText(cellInfo.text);
            clientColQcRole.SetValue(cellInfo.value);
            clientColQcRole['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientColQcRole.SetFocus();
            }       
        }";
        const string BatchEditEndEditingQCrole =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('QcRole.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientColQcRole.GetValue();
            cellInfo.text = clientColQcRole.GetText();       
            clientColQcRole.SetValue(null);
        }";

        const string BatchEditStartEditingQCRootRule =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('QCRootRole.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientColQCRootRole.SetText(cellInfo.text);
            clientColQCRootRole.SetValue(cellInfo.value);
            clientColQCRootRole['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientColQCRootRole.SetFocus();
            }       
        }";
        const string BatchEditEndEditingQCRootRule =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('QCRootRole.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientColQCRootRole.GetValue();
            cellInfo.text = clientColQCRootRole.GetText();       
            clientColQCRootRole.SetValue(null);
        }";

        const string BatchEditStartEditingQCSource =
       @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('QCSource.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientColQCSource.SetText(cellInfo.text);
            clientColQCSource.SetValue(cellInfo.value);
            clientColQCSource['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientColQCSource.SetFocus();
            }       
        }";
        const string BatchEditEndEditingQCSource =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('QCSource.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientColQCSource.GetValue();
            cellInfo.text = clientColQCSource.GetText();       
            clientColQCSource.SetValue(null);
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingship =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('ShippingOption.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientship.GetValue();
            cellInfo.text = clientship.GetText();       
            clientship.SetValue(null);
        }";
        const string BatchEditEndEditingDeliveryPriority =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('DeliveryPriority.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientdeliverypriority.GetValue();
            cellInfo.text = clientdeliverypriority.GetText();       
            clientdeliverypriority.SetValue(null);
        }";


        const string BatchEditStartEditingPriority =
      @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('Priority.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientTestpriority.SetText(cellInfo.text);
            clientTestpriority.SetValue(cellInfo.value);
            clientTestpriority['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientTestpriority.SetFocus();
            }       
        }";

        const string BatchEditEndEditingPriority =
           @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('Priority.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientTestpriority.GetValue();
            cellInfo.text = clientTestpriority.GetText();       
            clientTestpriority.SetValue(null);
        }";

        const string BatchEditStartEditingvendor =
        @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('Vendor.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientVendor.SetText(cellInfo.text);
            clientVendor.SetValue(cellInfo.value);
            clientVendor['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientVendor.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingvendor =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('Vendor.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientVendor.GetValue();
            cellInfo.text = clientVendor.GetText();       
            clientVendor.SetValue(null);
        }";

        const string BatchEditStartEditingstorage =
        @"function(s,e) {   
            var productNameColumn = s.GetColumnByField('Storage.Oid');          
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                    
            clientstorage.SetText(cellInfo.text);
            clientstorage.SetValue(cellInfo.value);
            clientstorage['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientstorage.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingstorage =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('Storage.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientstorage.GetValue();
            cellInfo.text = clientstorage.GetText();           
            clientstorage.SetValue(null);
        }";

        const string BatchEditStartEditingemp =
        @"function(s,e) {    
            var productNameColumn = s.GetColumnByField('givento.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientgivento.SetText(cellInfo.text);
            clientgivento.SetValue(cellInfo.value);
            clientgivento['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientgivento.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingemp =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('givento.Oid');                       
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];                       
            cellInfo.value = clientgivento.GetValue();
            cellInfo.text = clientgivento.GetText();            
            clientgivento.SetValue(null);         
        }";

        //#region Const
        //const string BatchEditStartEditing =
        //    @"function(s,e) {     
        //    var productNameColumn = s.GetColumnByField('Units.Oid');
        //    if(productNameColumn!=null){
        //    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
        //        return;
        //    var cellInfo = e.rowValues[productNameColumn.index];
        //    ReferencedEdit.SetText(cellInfo.text);
        //    ReferencedEdit.SetValue(cellInfo.value);
        //    ReferencedEdit['grid'] = s;
        //    if (e.focusedColumn === productNameColumn) {
        //        ReferencedEdit.SetFocus();
        //        }
        //    }
        //}";
        ////Handle the event to pass the value from the editor to the grid cell. 
        //const string BatchEditEndEditing =
        //    @"function(s,e){ 
        //    var productNameColumn = s.GetColumnByField('Units.Oid');
        //    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
        //        return;
        //    var cellInfo = e.rowValues[productNameColumn.index];
        //    cellInfo.value = ReferencedEdit.GetValue();
        //    cellInfo.text = ReferencedEdit.GetText();
        //    ReferencedEdit.SetValue(null);
        //}";
        //#endregion

        #region FinalDefaultUnits
        const string BatchEditStartEditingFinalDefaultUnits =
           @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('FinalDefaultUnits.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            ClientFinalDefaultUnits.SetText(cellInfo.text);
            ClientFinalDefaultUnits.SetValue(cellInfo.value);
            ClientFinalDefaultUnits['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                ClientFinalDefaultUnits.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingFinalDefaultUnits =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('FinalDefaultUnits.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = ClientFinalDefaultUnits.GetValue();
            cellInfo.text = ClientFinalDefaultUnits.GetText();
            ClientFinalDefaultUnits.SetValue(null);
        }";
        #endregion

        #region DefaultUnits
        const string BatchEditStartEditingDefaultUnits =
           @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('DefaultUnits.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            ClientDefaultUnits.SetText(cellInfo.text);
            ClientDefaultUnits.SetValue(cellInfo.value);
            ClientDefaultUnits['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                ClientDefaultUnits.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingDefaultUnits =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('DefaultUnits.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = ClientDefaultUnits.GetValue();
            cellInfo.text = ClientDefaultUnits.GetText();
            ClientDefaultUnits.SetValue(null);
        }";
        #endregion

        #region SurrogateUnits
        const string BatchEditStartEditingSurrogateUnits =
           @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('SurrogateUnits.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            ClientSurrogateUnits.SetText(cellInfo.text);
            ClientSurrogateUnits.SetValue(cellInfo.value);
            ClientSurrogateUnits['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                ClientSurrogateUnits.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingSurrogateUnits =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('SurrogateUnits.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = ClientSurrogateUnits.GetValue();
            cellInfo.text = ClientSurrogateUnits.GetText();
            ClientSurrogateUnits.SetValue(null);
        }";
        #endregion

        #region FinalResultUnits
        const string BatchEditStartEditingFinalResultUnits =
           @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('FinalResultUnits.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            ClientFinalResultUnits.SetText(cellInfo.text);
            ClientFinalResultUnits.SetValue(cellInfo.value);
            ClientFinalResultUnits['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                ClientFinalResultUnits.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingFinalResultUnits =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('FinalResultUnits.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = ClientFinalResultUnits.GetValue();
            cellInfo.text = ClientFinalResultUnits.GetText();
            ClientFinalResultUnits.SetValue(null);
        }";
        #endregion

        #region STDConcUnit
        const string BatchEditStartEditingSTDConcUnit =
           @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('STDConcUnit.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            ClientSTDConcUnit.SetText(cellInfo.text);
            ClientSTDConcUnit.SetValue(cellInfo.value);
            ClientSTDConcUnit['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                ClientSTDConcUnit.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingSTDConcUnit =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('STDConcUnit.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = ClientSTDConcUnit.GetValue();
            cellInfo.text = ClientSTDConcUnit.GetText();
            ClientSTDConcUnit.SetValue(null);
        }";
        #endregion

        #region STDVolUnit
        const string BatchEditStartEditingSTDVolUnit =
           @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('STDVolUnit.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            ClientSTDVolUnit.SetText(cellInfo.text);
            ClientSTDVolUnit.SetValue(cellInfo.value);
            ClientSTDVolUnit['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                ClientSTDVolUnit.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingSTDVolUnit =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('STDVolUnit.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = ClientSTDVolUnit.GetValue();
            cellInfo.text = ClientSTDVolUnit.GetText();
            ClientSTDVolUnit.SetValue(null);
        }";
        #endregion

        #region UnitName
        const string BatchEditStartEditingUnitName =
         @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('Units.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientUnitName.SetText(cellInfo.text);
            clientUnitName.SetValue(cellInfo.value);
            clientUnitName['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientUnitName.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingUnitName =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('Units.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientUnitName.GetValue();
            cellInfo.text = clientUnitName.GetText();
            clientUnitName.SetValue(null);
        }";
        #endregion 

        #region STDVolUnit
        const string BatchEditStartEditingSpikeAmountUnit =
           @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('SpikeAmountUnit.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientSpikeAmountUnit.SetText(cellInfo.text);
            clientSpikeAmountUnit.SetValue(cellInfo.value);
            clientSpikeAmountUnit['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientSpikeAmountUnit.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingSpikeAmountUnit =
            @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('SpikeAmountUnit.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientSpikeAmountUnit.GetValue();
            cellInfo.text = clientSpikeAmountUnit.GetText();
            clientSpikeAmountUnit.SetValue(null);
        }";
        #endregion

        #region SubLabName
        const string BatchEditStartEditingSubLabName =
           @"function(s,e) {     
            var SubLabNameColumn = s.GetColumnByField('SubLabName.Oid');
            if (!e.rowValues.hasOwnProperty(SubLabNameColumn.index))
                return;
            var cellInfo = e.rowValues[SubLabNameColumn.index];
            clientSubLabName.SetText(cellInfo.text);
            clientSubLabName.SetValue(cellInfo.value);
            clientSubLabName['grid'] = s;
            if (e.focusedColumn === SubLabNameColumn) {
                clientSubLabName.SetFocus();
            }
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingSubLabName =
            @"function(s,e){ 
            var SubLabNameColumn = s.GetColumnByField('SubLabName.Oid');
            if (!e.rowValues.hasOwnProperty(SubLabNameColumn.index))
                return;
            var cellInfo = e.rowValues[SubLabNameColumn.index];
            cellInfo.value = clientSubLabName.GetValue();
            cellInfo.text = clientSubLabName.GetText();
            clientSubLabName.SetValue(null);
        }";
        #endregion

        const string BatchEditStartEditingSubOutBy =
        @"function(s,e) {   
            var subOutByNameColumn = s.GetColumnByField('SubOutBy.Oid');          
            if (!e.rowValues.hasOwnProperty(subOutByNameColumn.index))
                return;
            var cellInfo = e.rowValues[subOutByNameColumn.index];                    
            clientSubOutBy.SetText(cellInfo.text);
            clientSubOutBy.SetValue(cellInfo.value);
            clientSubOutBy['grid'] = s;
            if (e.focusedColumn === subOutByNameColumn) {
                clientSubOutBy.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingSubOutBy =
            @"function(s,e){ 
            var subOutByNameColumn = s.GetColumnByField('SubOutBy.Oid');
            if (!e.rowValues.hasOwnProperty(subOutByNameColumn.index))
                return;
            var cellInfo = e.rowValues[subOutByNameColumn.index];
            cellInfo.value = clientSubOutBy.GetValue();
            cellInfo.text = clientSubOutBy.GetText();       
            clientSubOutBy.SetValue(null);
        }";

        const string BatchEditStartEditingSampleType =
        @"function(s,e) {   
            var sampleTypeNameColumn = s.GetColumnByField('SampleType.Oid');          
            if (!e.rowValues.hasOwnProperty(sampleTypeNameColumn.index))
                return;
            var cellInfo = e.rowValues[sampleTypeNameColumn.index];                    
            clientSampleType.SetText(cellInfo.text);
            clientSampleType.SetValue(cellInfo.value);
            clientSampleType['grid'] = s;
            if (e.focusedColumn === sampleTypeNameColumn) {
                clientSampleType.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingSampleType =
            @"function(s,e){ 
            var sampleTypeNameColumn = s.GetColumnByField('SampleType.Oid');
            if (!e.rowValues.hasOwnProperty(sampleTypeNameColumn.index))
                return;
            var cellInfo = e.rowValues[sampleTypeNameColumn.index];
            cellInfo.value = clientSampleType.GetValue();
            cellInfo.text = clientSampleType.GetText();       
            clientSampleType.SetValue(null);
        }";

        const string BatchEditStartEditingSampleStatus =
        @"function(s,e) {   
            var sampleStatuseNameColumn = s.GetColumnByField('SampleStatus.Oid');          
            if (!e.rowValues.hasOwnProperty(sampleStatuseNameColumn.index))
                return;
            var cellInfo = e.rowValues[sampleStatuseNameColumn.index];                    
            clientSampleStatus.SetText(cellInfo.text);
            clientSampleStatus.SetValue(cellInfo.value);
            clientSampleStatus['grid'] = s;
            if (e.focusedColumn === sampleStatuseNameColumn) {
                clientSampleStatus.SetFocus();
            }       
        }";
        //Handle the event to pass the value from the editor to the grid cell. 
        const string BatchEditEndEditingSampleStatus =
            @"function(s,e){ 
            var sampleStatuseNameColumn = s.GetColumnByField('SampleStatus.Oid');
            if (!e.rowValues.hasOwnProperty(sampleStatuseNameColumn.index))
                return;
            var cellInfo = e.rowValues[sampleStatuseNameColumn.index];
            cellInfo.value = clientSampleStatus.GetValue();
            cellInfo.text = clientSampleStatus.GetText();       
            clientSampleStatus.SetValue(null);
        }";

        //const string BatchEditStartEditingTestPrice =
        //@"function(s,e) {   
        //    var TATNameColumn = s.GetColumnByField('TAT.Oid');          
        //    if (!e.rowValues.hasOwnProperty(TATNameColumn.index))
        //        return;
        //    var cellInfo = e.rowValues[TATNameColumn.index];                    
        //    ClientTestPriceTAT.SetText(cellInfo.text);
        //    ClientTestPriceTAT.SetValue(cellInfo.value);
        //    ClientTestPriceTAT['grid'] = s;
        //    if (e.focusedColumn === TATNameColumn) {
        //        ClientTestPriceTAT.SetFocus();
        //    }       
        //}";
        ////Handle the event to pass the value from the editor to the grid cell. 
        //const string BatchEditEndEditingTestPrice =
        //    @"function(s,e){ 
        //    var TATNameColumn = s.GetColumnByField('TAT.Oid');
        //    if (!e.rowValues.hasOwnProperty(TATNameColumn.index))
        //        return;
        //    var cellInfo = e.rowValues[TATNameColumn.index];
        //    cellInfo.value = ClientTestPriceTAT.GetValue();
        //    cellInfo.text = ClientTestPriceTAT.GetText();       
        //    ClientTestPriceTAT.SetValue(null);
        //}";
        const string BatchEditStartEditingAssignedBy =
   @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('AssignedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientAssignedBy.SetText(cellInfo.text);
            clientAssignedBy.SetValue(cellInfo.value);
            clientAssignedBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientAssignedBy.SetFocus();
            }
        }";
        const string BatchEditEndEditingAssignedBy =
       @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('AssignedBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientAssignedBy.GetValue();
            cellInfo.text = clientAssignedBy.GetText();
            clientAssignedBy.SetValue(null);
        }";
        const string BatchEditStartEditingTransferredBy =
  @"function(s,e) {     
            var productNameColumn = s.GetColumnByField('TransferredBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientTransferredBy.SetText(cellInfo.text);
            clientTransferredBy.SetValue(cellInfo.value);
            clientTransferredBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientTransferredBy.SetFocus();
            }
        }";
        const string BatchEditEndEditingTransferredBy =
       @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('TransferredBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientTransferredBy.GetValue();
            cellInfo.text = clientTransferredBy.GetText();
            clientTransferredBy.SetValue(null);
        }";
        const string BatchEditStartEditingItemPackageUnits =
          @"function(s,e)
            {
                var productNameColumn = s.GetColumnByField('Units.Oid');  
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                if(cellInfo.value != null && cellInfo.text != null)
                {
                    clientItemUnit.SetValue(cellInfo.value);                   
                    clientItemUnit.SetText(cellInfo.text);
                }
                    clientItemUnit['grid'] = s;
                if (e.focusedColumn === productNameColumn)
                {
                    clientItemUnit.SetFocus();
                }
            }";

        const string BatchEditEndEditingItemPackageUnits =
            @"function(s,e)
            { 
                var productNameColumn = s.GetColumnByField('Units.Oid');
                if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                    return;
                var cellInfo = e.rowValues[productNameColumn.index];
                cellInfo.value = clientItemUnit.GetValue();
                cellInfo.text = clientItemUnit.GetText();       
                clientItemUnit.SetValue(null);
            }";
        const string BatchEditStartEditingDeliveredBy =
@"function(s,e) {     
            var productNameColumn = s.GetColumnByField('DeliveredBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientDeliveredBy.SetText(cellInfo.text);
            clientDeliveredBy.SetValue(cellInfo.value);
            clientDeliveredBy['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientDeliveredBy.SetFocus();
            }
        }";
        const string BatchEditEndEditingDeliveredBy =
       @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('DeliveredBy.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientDeliveredBy.GetValue();
            cellInfo.text = clientDeliveredBy.GetText();
            clientDeliveredBy.SetValue(null);
        }";

        const string BatchEditStartEditingKeyCRMQuotesComponent =
@"function(s,e) {     
            var productNameColumn = s.GetColumnByField('Component.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientComponent.SetText(cellInfo.text);
            clientComponent.SetValue(cellInfo.value);
            clientComponent['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientComponent.SetFocus();
            }
        }";
        const string BatchEditEndEditingKeyCRMQuotesComponent =
       @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('Component.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientComponent.GetValue();
            cellInfo.text = clientComponent.GetText();
            clientComponent.SetValue(null);
        }";
        const string BatchEditStartEditingKeyTAT =
@"function(s,e) {     
            var productNameColumn = s.GetColumnByField('TAT.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            InvoiceTAT.SetText(cellInfo.text);
            InvoiceTAT.SetValue(cellInfo.value);
            InvoiceTAT['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                InvoiceTAT.SetFocus();
            }
        }";
        const string BatchEditEndEditingKeyTAT =
       @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('TAT.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = InvoiceTAT.GetValue();
            cellInfo.text = InvoiceTAT.GetText();
            InvoiceTAT.SetValue(null);
        }";


        const string BatchEditStartEditingKeyWUnit =
@"function(s,e) {     
            var productNameColumn = s.GetColumnByField('WSCons_Units.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientWSCons_Units.SetText(cellInfo.text);
            clientWSCons_Units.SetValue(cellInfo.value);
            clientWSCons_Units['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientWSCons_Units.SetFocus();
            }
        }";
        const string BatchEditEndEditingKeyWUnit =
       @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('WSCons_Units.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientWSCons_Units.GetValue();
            cellInfo.text = clientWSCons_Units.GetText();
            clientWSCons_Units.SetValue(null);
        }";
        const string BatchEditStartEditingKeyV1Unit =
@"function(s,e) {     
                    var productNameColumn = s.GetColumnByField('Cal_VolTaken_V1_Units.Oid');
                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                        return;
                    var cellInfo = e.rowValues[productNameColumn.index];
                    clientCal_VolTaken_V1_Units.SetText(cellInfo.text);
                    clientCal_VolTaken_V1_Units.SetValue(cellInfo.value);
                    clientCal_VolTaken_V1_Units['grid'] = s;
                    if (e.focusedColumn === productNameColumn) {
                        clientCal_VolTaken_V1_Units.SetFocus();
                    }
                }";
        const string BatchEditEndEditingKeyV1Unit =
       @"function(s,e){ 
                    var productNameColumn = s.GetColumnByField('Cal_VolTaken_V1_Units.Oid');
                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                        return;
                    var cellInfo = e.rowValues[productNameColumn.index];
                    cellInfo.value = clientCal_VolTaken_V1_Units.GetValue();
                    cellInfo.text = clientCal_VolTaken_V1_Units.GetText();
                    clientCal_VolTaken_V1_Units.SetValue(null);
                }";


        const string BatchEditStartEditingKeyV2Unit =
@"function(s,e) {     
                    var productNameColumn = s.GetColumnByField('Cal_FinalVol_V2_Units.Oid');
                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                        return;
                    var cellInfo = e.rowValues[productNameColumn.index];
                    clientCal_FinalVol_V2_Units.SetText(cellInfo.text);
                    clientCal_FinalVol_V2_Units.SetValue(cellInfo.value);
                    clientCal_FinalVol_V2_Units['grid'] = s;
                    if (e.focusedColumn === productNameColumn) {
                        clientCal_FinalVol_V2_Units.SetFocus();
                    }
                }";
        const string BatchEditEndEditingKeyV2Unit =
       @"function(s,e){ 
                    var productNameColumn = s.GetColumnByField('Cal_FinalVol_V2_Units.Oid');
                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                        return;
                    var cellInfo = e.rowValues[productNameColumn.index];
                    cellInfo.value = clientCal_FinalVol_V2_Units.GetValue();
                    cellInfo.text = clientCal_FinalVol_V2_Units.GetText();
                    clientCal_FinalVol_V2_Units.SetValue(null);
                }";

        const string BatchEditStartEditingKeyC2Unit =
@"function(s,e) {     
                    var productNameColumn = s.GetColumnByField('Cal_FinalConc_C2_Units.Oid');
                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                        return;
                    var cellInfo = e.rowValues[productNameColumn.index];
                    clientCal_FinalConc_C2_Units.SetText(cellInfo.text);
                    clientCal_FinalConc_C2_Units.SetValue(cellInfo.value);
                    clientCal_FinalConc_C2_Units['grid'] = s;
                    if (e.focusedColumn === productNameColumn) {
                        clientCal_FinalConc_C2_Units.SetFocus();
                    }
                }";
        const string BatchEditEndEditingKeyC2Unit =
       @"function(s,e){ 
                    var productNameColumn = s.GetColumnByField('Cal_FinalConc_C2_Units.Oid');
                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                        return;
                    var cellInfo = e.rowValues[productNameColumn.index];
                    cellInfo.value = clientCal_FinalConc_C2_Units.GetValue();
                    cellInfo.text = clientCal_FinalConc_C2_Units.GetText();
                    clientCal_FinalConc_C2_Units.SetValue(null);
                }";



        const string BatchEditStartEditingProjectID =
@"function(s,e) {     
            var productNameColumn = s.GetColumnByField('ProjectID.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            clientProjectID.SetText(cellInfo.text);
            clientProjectID.SetValue(cellInfo.value);
            clientProjectID['grid'] = s;
            if (e.focusedColumn === productNameColumn) {
                clientProjectID.SetFocus();
            }
        }";
        const string BatchEditEndEditingProjectID =
      @"function(s,e){ 
            var productNameColumn = s.GetColumnByField('ProjectID.Oid');
            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
                return;
            var cellInfo = e.rowValues[productNameColumn.index];
            cellInfo.value = clientProjectID.GetValue();
            cellInfo.text = clientProjectID.GetText();
            clientProjectID.SetValue(null);
        }";


        //        const string BatchEditStartEditingItemMatrix =
        //@"function(s,e) {     
        //            var productNameColumn = s.GetColumnByField('Matrix.Oid');
        //            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
        //                return;
        //            var cellInfo = e.rowValues[productNameColumn.index];
        //            clientMatrix.SetText(cellInfo.text);
        //            clientMatrix.SetValue(cellInfo.value);
        //            clientMatrix['grid'] = s;
        //            if (e.focusedColumn === productNameColumn) {
        //                clientMatrix.SetFocus();
        //            }
        //        }";
        //        const string BatchEditEndEditingItemMatrix =
        //       @"function(s,e){ 
        //            var productNameColumn = s.GetColumnByField('Matrix.Oid');
        //            if (!e.rowValues.hasOwnProperty(productNameColumn.index))
        //                return;
        //            var cellInfo = e.rowValues[productNameColumn.index];
        //            cellInfo.value = clientMatrix.GetValue();
        //            cellInfo.text = clientMatrix.GetText();
        //            clientMatrix.SetValue(null);
        //        }";
        //        const string BatchEditStartEditingItemTestName =
        //@"function(s,e) {     
        //                    var productNameColumn = s.GetColumnByField('Test.Oid');
        //                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
        //                        return;
        //                    var cellInfo = e.rowValues[productNameColumn.index];
        //                    clientTestName.SetText(cellInfo.text);
        //                    clientTestName.SetValue(cellInfo.value);
        //                    clientTestName['grid'] = s;
        //                    if (e.focusedColumn === productNameColumn) {
        //                        clientTestName.SetFocus();
        //                    }
        //                }";
        //        const string BatchEditEndEditingItemTestName =
        //       @"function(s,e){ 
        //                    var productNameColumn = s.GetColumnByField('Test.Oid');
        //                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
        //                        return;
        //                    var cellInfo = e.rowValues[productNameColumn.index];
        //                    cellInfo.value = clientTestName.GetValue();
        //                    cellInfo.text = clientTestName.GetText();
        //                    clientTestName.SetValue(null);
        //                }";

        //        const string BatchEditStartEditingItemTestMethodNumber =
        //@"function(s,e) {     
        //                    var productNameColumn = s.GetColumnByField('Method.Oid');
        //                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
        //                        return;
        //                    var cellInfo = e.rowValues[productNameColumn.index];
        //                    clientTestMethodNumber.SetText(cellInfo.text);
        //                    clientTestMethodNumber.SetValue(cellInfo.value);
        //                    clientTestMethodNumber['grid'] = s;
        //                    if (e.focusedColumn === productNameColumn) {
        //                        clientTestMethodNumber.SetFocus();
        //                    }
        //                }";
        //        const string BatchEditEndEditingItemTestMethodNumber =
        //       @"function(s,e){ 
        //                    var productNameColumn = s.GetColumnByField('Method.Oid');
        //                    if (!e.rowValues.hasOwnProperty(productNameColumn.index))
        //                        return;
        //                    var cellInfo = e.rowValues[productNameColumn.index];
        //                    cellInfo.value = clientTestMethodNumber.GetValue();
        //                    cellInfo.text = clientTestMethodNumber.GetText();
        //                    clientTestMethodNumber.SetValue(null);
        //                }";
        //const string BatchEditStartEditingOptions =
        //@"function(s,e) {   
        //    var clauseOptionsColumn = s.GetColumnByField('Conclusion.Oid');          
        //    if (!e.rowValues.hasOwnProperty(clauseOptionsColumn.index))
        //        return;
        //    var cellInfo = e.rowValues[clauseOptionsColumn.index];                    
        //    clientColOptions.SetText(cellInfo.text);
        //    clientColOptions.SetValue(cellInfo.value);
        //    clientColOptions['grid'] = s;
        //    if (e.focusedColumn === clauseOptionsColumn) {
        //        clientColOptions.SetFocus();
        //    }       
        //}";
        //const string BatchEditEndEditingOptions =
        //    @"function(s,e){ 
        //    var clauseOptionsColumn = s.GetColumnByField('Conclusion.Oid');
        //    if (!e.rowValues.hasOwnProperty(clauseOptionsColumn.index))
        //        return;
        //    var cellInfo = e.rowValues[clauseOptionsColumn.index];
        //    cellInfo.value = clientColOptions.GetValue();
        //    cellInfo.text = clientColOptions.GetText();       
        //    clientColOptions.SetValue(null);
        //}";
    }
}
