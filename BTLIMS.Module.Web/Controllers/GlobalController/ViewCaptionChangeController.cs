using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Labmaster.Module.Web.Controllers.GlobalController
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ViewCaptionChangeController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        NavigationInfo navigationInfo = new NavigationInfo();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        View view;
        IModelDetailView viewdetial;
        List<string> lstViewID = new List<string>();
        public ViewCaptionChangeController()
        {
            InitializeComponent();
            //this.TargetViewId = "ReminderActivity_ListView;" + "ReminderActivity_DetailView;" + "ReminderActivity_ListView_Copy;" + "ReminderActivity_DetailView_Copy;" + "Samplecheckin_ListView_Incompletejobs;" + "Samplecheckin_DetailView;" 
            //    + "Samplecheckin_ListView_ProjectTracking;" + "Contract_ListView;" + "Contract_DetailView;" + "Contract_ListView_ContractValidation_Copy;" + "Contract_DetailView_ContractValidation_Copy;" + "Contract_ListView_ContractTrcking_Copy;"
            //    + "Contract_ListView_CancelledContract_Copy;" + "Contract_DetailView_CancelContract_Copy;" + "Tasks_ListView;" + "Tasks_DetailView;" + "Tasks_ListView_Copy_ProposalValidation;" + "Tasks_DetailView_Copy_TasksRelease;" + "Tasks_ListView_PropsalCancellation;"
            //    + "Tasks_ListView_Copy_TaskRelease;" + "Tasks_DetailView_Copy_TasksRelease;" + "Tasks_ListView_Copy_TaskAcceptance;" + "Tasks_DetailView_Copy_TaskAcceptance;" + "Tasks_ListView_DeliveryTasks;" + "Tasks_DetailView_DeliveryTasks;" + "Tasks_ListView_CanceledDelivery;"
            //    + "Tasks_DetailView_CanceledDeliveryTasks;" + "Tasks_ListView_SamplingAssignment;" + "Tasks_DetailView_SamplingAssignment;" + "Tasks_ListView_PrintingSampleLabels;" + "Tasks_ListView_FieldDataEntry;" + "Tasks_DetailView_FieldDataEntry;" + "Tasks_ListView_FieldDataReview1;"
            //    + "Tasks_DetailView_FieldDataReview1;" + "Tasks_ListView_FieldDataReview2;" + "Tasks_DetailView_FieldDataReview2;" + "Samplecheckin_ListView_Copy_Registration;" + "Samplecheckin_DetailView_Copy_SampleRegistration;" + "Samplecheckin_ListView_Copy_RegistrationSigningOff;"
            //    + "Samplecheckin_DetailView_Copy_RegistrationSigningOff;" + "SampleLogIn_ListView_SampleDisposition;" + "SampleLogIn_DetailView;" + "Tasks_ListView_SampleTransfer;" + "Tasks_DetailView_SampleTransfer;" + "SampleParameter_ListView_Copy_SubOutPendingSamples;"
            //    + "SubOutSampleRegistrations_ListView_Copy_SuboutDelivered;" + "SubOutSampleRegistrations_DetailView;" + "SubOutSampleRegistrations_ListView_Copy_ResultEntry;" + "SubOutSampleRegistrations_DetailView_Copy_SuboutResultEntry;" + "SubOutSampleRegistrations_ListView_Level2DataReview;"
            //    + "SubOutSampleRegistrations_DetailView_Level2DataReview;" + "SubOutSampleRegistrations_ListView_Level3DataReview;" + "SubOutSampleRegistrations_DetailView_Level3DataReview;" + "SampleParameter_ListView_Copy_SubOutPendingSamples;" + "SubOutContractLab_ListView;" + "SubOutContractLab_DetailView;"
            //    + "SubOutSampleRegistrations_ListView_NotificationQueue;" + "SubOutSampleRegistrations_DetailView_SuboutOrderTracking;" + "SubOutSampleRegistrations_ListView_NotificationQueue;" + "IndoorInspection_ListView;" + "SubOutSampleRegistrations_ListView_Copy_SuboutDelivery;"
            //    + "SubOutSampleRegistrations_ListView;" + "IndoorInspection_DetailView;" + "OutdoorInspection_ListView;" + "OutdoorInspection_DetailView;" + "ProductSampleMapping_ListView;" + "ProductSampleMapping_DetailView;" + "SamplePretreatmentBatch_ListView;" + "SamplePretreatmentBatch_DetailView;" 
            //    + "TestMethod_ListView_PrepQueue;" + "TestMethod_DetailView;" + "SampleWeighingBatch_ListView;" + "SampleWeighingBatch_DetailView;" + "SampleWeighingBatchSequence_ListView_Tracking;" + "SampleWeighingBatchSequence_DetailView;" + "TestMethod_ListView_AnalysisQueue;" + "SpreadSheetEntry_AnalyticalBatch_ListView;"
            //    + "SpreadSheetEntry_AnalyticalBatch_DetailView_Copy;" + "QCBatch_ListView;" + "QCBatch_DetailView;" + "SDMS;" + "ResultEntryQueryPanel_DetailView_Copy;" + "DummyClass_ListView;" + "DummyClass_DetailView;" + "RawDataResultView_ListView;" + "RawDataResultView_DetailView;" + "SampleParameter_ListView_Copy_ResultEntry;"
            //    + "Result_View;" + "ResultViewQueryPanel_DetailView;" + "Calibration_ListView;" + "Calibration_DetailView;" + "CalibrationInfo_ListView_Curve;" + "CalibrationInfo_DetailView;" + "Requisition_ListViewEntermode;" + "Requisition_DetailView;" + "Requisition_ListView_Review;" + "Requisition_ListView_Approve;" 
            //    + "Requisition_ListView_Purchaseorder_Mainview;" + "Requisition_ListView_Receive_MainReceive;" + "Distribution_ListView_MainDistribute;" + "Distribution_ListView_Consumption;" + "Distribution_DetailView;"+ "Distribution_ListView_Disposal;" + "VendorReagentCertificate_ListView;" + "VendorReagentCertificate_DetailView;"
            //    + "Items_ListView_Copy_StockWatch;" + "Items_DetailView;" + "Items_ListView_Copy_StockAlert;" + "Distribution_ListView_Copy_ExpirationAlert;" + "Items_ListView;" + "Vendors_ListView;" + "Vendors_DetailView;" + "Category_ListView;" + "Category_DetailView;"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";"
            //    + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";" + ";";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                // Perform various tasks depending on the target View.
                if (lstViewID != null)
                {
                    lstViewID.Clear();
                }
                else
                {
                    lstViewID = new List<string>();
                }
                NavigationItem objNavigation = ObjectSpace.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationId] = ?", objnavigationRefresh.ClickedNavigationItem));
                if (objNavigation != null)
                {
                    lstViewID.Add(objNavigation.NavigationView);
                    DevExpress.ExpressApp.Model.IModelViews lstViews = DevExpress.ExpressApp.Web.WebApplication.Instance.Model.Views;
                    if (View.GetType() == typeof(DetailView) && objNavigation.NavigationView.Contains("_ListView") && lstViews != null)
                    {
                        DevExpress.ExpressApp.Model.IModelListView lv = (DevExpress.ExpressApp.Model.IModelListView)lstViews.FirstOrDefault(i => i.Id == objNavigation.NavigationView);
                        if (lv != null && lv.DetailView != null)
                        {
                            lstViewID.Add(lv.DetailView.Id);
                        }
                    }

                }

                if (View.Id == "ResultEntry_Validation" || View.Id == "ResultEntry_Approval")
                {
                    string strCaption = string.Empty;
                    string[] arrCaptions = navigationInfo.SelectedNavigationCaption.Split('(');
                    if (arrCaptions.Length == 1)
                    {
                        strCaption = navigationInfo.SelectedNavigationCaption;
                    }
                    else if (arrCaptions.Length == 2)
                    {
                        strCaption = arrCaptions[0];
                    }
                    else if (arrCaptions.Length > 2)
                    {
                        strCaption = string.Join(" ", arrCaptions.ToList().Where(i => i != arrCaptions[arrCaptions.Length - 1]));
                    }
                    View.Caption = strCaption;
                }
                else if (View.Id == "ResultValidationQueryPanel_DetailView_ResultValidation_View" || View.Id == "ResultEntry_Validation_View" ||
                    View.Id == "ResultValidationQueryPanel_DetailView_ResultApproval_View" || View.Id == "ResultEntry_Approval_View" ||
                    View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview_History" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel3BatchReview_History")
                {
                    string strCaption = string.Empty;
                    string[] arrCaptions = navigationInfo.SelectedNavigationCaption.Split('(');
                    if (arrCaptions.Length == 1)
                    {
                        strCaption = navigationInfo.SelectedNavigationCaption;
                    }
                    else if (arrCaptions.Length == 2)
                    {
                        strCaption = arrCaptions[0];
                    }
                    else if (arrCaptions.Length > 2)
                    {
                        strCaption = string.Join(" ", arrCaptions.ToList().Where(i => i != arrCaptions[arrCaptions.Length - 1]));
                    }
                    View.Caption = strCaption.Trim() + " History";
                }
                if (View.Id == "Manual_DetailView")
                {
                    View.Caption = navigationInfo.SelectedNavigationCaption;

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
                //ShowNavigationItemController navigationController = Frame.GetController<ShowNavigationItemController>();
                //if (View is ListView)
                //{
                //    //navigationController.ShowNavigationItemAction.i
                //    //((ListView)View).DetailViewId;
                //    if (navigationController.Window != null)
                //    {
                //        view = navigationController.Window.View;
                //        IModelListView model = ((ListView)View).Model;
                //        viewdetial = model.DetailView;
                //    }
                //}

                if (View is ListView)
                {
                    if (View.Id != "Reporting_ListView_Datacenter" && View.Id != "TestMethod_DetailView" && View.Id != "COCSettings_DetailView" && View.Id != "SamplePrepBatch_DetailView_Copy_History" && lstViewID != null && lstViewID.Contains(View.Id))
                    {
                        if (!navigationInfo.SelectedNavigationCaption.Trim().EndsWith(")"))
                        {
                            View.Caption = navigationInfo.SelectedNavigationCaption;
                        }
                        else
                        {
                            string strCaption = string.Empty;
                            string[] arrCaptions = navigationInfo.SelectedNavigationCaption.Split('(');
                            if (arrCaptions.Length == 2)
                            {
                                strCaption = arrCaptions[0];
                            }
                            else if (arrCaptions.Length > 2)
                            {
                                strCaption = string.Join(" ", arrCaptions.ToList().Where(i => i != arrCaptions[arrCaptions.Length - 1]));
                            }
                            View.Caption = strCaption;
                        }
                    }

                }
                // Access and customize the target View control.
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
