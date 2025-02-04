﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Web.UI.WebControls;

namespace LDM.Module.Web.Controllers.GlobalController
{
    public partial class ColumnWidthWebViewController : ViewController<ListView>
    {
        MessageTimer timer = new MessageTimer();
        #region Constructor
        public ColumnWidthWebViewController()
        {
            InitializeComponent();
            TargetViewId = "Samplecheckin_ListView;" + "SampleLogIn_ListView;" + "Reporting_ListView_Level1Review_View;" + "Reporting_ListView_Level2Review_View;" +
                "UserNavigationPermission_ListView;" + "TestMethod_ListView;" + "Customer_ListView;" + "Employee_ListView;" + "QCTestParameter_ListView_Copy;" +
                /*"SampleLogIn_ListView_Copy_SampleRegistration;"+*//* + "Samplecheckin_ListView_Copy_Registration;"*/ "Method_ListView;" +
                "SampleParameter_ListView_Copy_ResultEntry;" + "SampleParameter_ListView_Copy_QCResultEntry;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;" + "SampleParameter_ListView_Copy_ResultView_SingleChoice;" +
                "SampleParameter_ListView_Copy_ResultView;" + "SampleParameter_ListView_Copy_QCResultView;" +
                "SampleParameter_ListView_Copy_ResultApproval;" + "SampleParameter_ListView_Copy_QCResultApproval;" +
                "SampleParameter_ListView_Copy_ResultValidation;" + "SampleParameter_ListView_Copy_QCResultValidation;" +
                 "Reporting_ListView_Copy_ReportApproval;" + "Reporting_ListView_Copy_ReportView;" +
                "SampleParameter_ListView_Copy_ResultView_Main;" + "SampleParameter_ListView_Copy_ResultEntry_Main;" +
                "ClauseInspectionSettings_ListView;" + "IndoorInspection_ClauseInspections_ListView;" + "IndoorInspection_Notepads_ListView;" +
                "SampleLogIn_ListView_SampleDisposition;" + "Reporting_ListView_PrintDownload;" /*+ "Reporting_ListView_Delivery;" */+ "Invoicing_ListView_Delivery;" + "Reporting_ListView_Deliveired;" + "Reporting_ListView_Archieve;"
                + "Reporting_ListView_Account;" + "Reporting_ListView_Recall;" + "QCBatch_ListView_ResultEntry;" /*+ "Requisition_ListView;" + "Requisition_ListView_Review;"
                + "Requisition_ListView_Approve;" + "Items_ListView;" + "Items_ListView_Copy_StockWatch;" + "Vendors_ListView;"*/ + "Contact_ListView;" + "Parameter_ListView;"
                + "SDMSRollback_ListView;" /*+ "ExistingStock_ListView;"*/ + "Labware_ListView;" + "COCSettings_ListView;"
                + "TestMethod_ListView_AnalysisDepartmentChain;" /*+ "Distribution_ListView_Consumption;"*/ + "Testparameter_ListView;" + "TestParameter_ListView_DefaultSettings;"
                + "TestParameter_ListView_DefaultSettings_QC;" + "QCBatch_ListView_ResultValidation;" + "CustomReportBuilder_ListView"
                + "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview;"
                + "SampleParameter_ListView_Copy_ResultApproval_Level2Review;"
                + "SampleParameter_ListView_Copy_QCResultValidation_Level1Review;"
                + "SampleParameter_ListView_Copy_QCResultApproval_Level2Review;"
                + "Samplecheckin_ListView_Copy_RegistrationSigningOff;" + "Samplecheckin_ListView_Copy_RegistrationSigningOff_History;"
                + "SampleParameter_ListView_Copy_RegistrationSignOffSamples;" + "SampleLogIn_ListView_SampleDisposition_CurrentSamples;" + "SampleLogIn_ListView_FieldDataEntry_Sampling;" + "SampleParameter_ListView_FieldDataEntry;"
                + "Sampling_ListView_SamplingAssignment;" + "Tasks_ListView_FieldDataEntry;" + "Tasks_ListView_FieldDataReview2;" + "Tasks_ListView_FieldDataReview1;" + "SampleLogIn_ListView_FieldDataEntry_Station;" + "Tasks_ListView_History;"
                + "Email_To_ListView;" + "Email_Bcc_ListView;" + "Email_CC_ListView;"
                + "SampleLogIn_ListView_FieldDataReview1_Sampling;"
                + "SampleLogIn_ListView_FieldDataReview1_Station;"
                + "SampleParameter_ListView_FieldDataReview1;"
                + "SampleLogIn_ListView_FieldDataReview2_Sampling;"
                + "SampleLogIn_ListView_FieldDataReview2_Station;"
                + "SampleParameter_ListView_FieldDataReview2;"
                + "SampleBottleAllocation_ListView_SampleTransfer;";
            //Samplecheckin_ListView_Copy_Registration
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                ASPxGridListEditor gridListEditorscroll = View.Editor as ASPxGridListEditor;
                ASPxGridView gridView = gridListEditorscroll.Grid;
                gridView.Settings.UseFixedTableLayout = false;
                string strwidth = System.Web.HttpContext.Current.Request.Cookies.Get("width").Value;
                string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                if (View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView" || View.Id == "Purchaseorder_Item_ListView" || View.Id == "Email_Bcc_ListView"
                    || View.Id == "Email_CC_ListView" || View.Id == "Email_To_ListView" || View.Id == "Tasks_ListView_FieldDataReview2" || View.Id == "Tasks_ListView_FieldDataReview1"
                    || View.Id == "Sampling_ListView_SamplingAssignment" || View.Id == "Tasks_ListView_FieldDataEntry" || View.Id == "Reporting_ListView_Copy_ReportApproval"
                    || View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Archieve" || View.Id == "Reporting_ListView_Account"
                    || View.Id == "Reporting_ListView_Recall" || View.Id == "Contact_ListView" || View.Id == "SampleLogIn_ListView_SampleDisposition" || View.Id == "SampleLogIn_ListView_FieldDataEntry_Station"
                    || View.Id == "SampleLogIn_ListView_FieldDataEntry_Sampling" || View.Id == "SampleParameter_ListView_FieldDataEntry" || (View.Id == "Reporting_ListView_Copy_ReportView" && Convert.ToInt32(strscreenwidth) < 1800)
                    || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Reporting_ListView_Delivery" || View.Id == "Items_ListView" || View.Id == "Vendors_ListView" || View.Id == "Customer_ListView" || View.Id == "TestParameter_ListView_DefaultSettings_QC"
                    || View.Id == "TestParameter_ListView_DefaultSettings" || View.Id == "AuditDataItemPersistent_ListView" || View.Id == "Tasks_ListView_SamplingAssignment" ||/* View.Id == "Samplecheckin_ListView_Copy_Registration" || View.Id == "Samplecheckin_ListView_Copy_Registration_History" ||*/ View.Id == "Tasks_ListView_History"
                     || View.Id == "SampleLogIn_ListView_FieldDataReview1_Sampling"
                      || View.Id == "SampleLogIn_ListView_FieldDataReview1_Station"
                      || View.Id == "SampleParameter_ListView_FieldDataReview1"
                      || View.Id == "SampleLogIn_ListView_FieldDataReview2_Sampling"
                      || View.Id == "SampleLogIn_ListView_FieldDataReview2_Station"
                      || View.Id == "SampleParameter_ListView_FieldDataReview2"
                      || View.Id == "SampleBottleAllocation_ListView_SampleTransfer")
                {

                    if (gridListEditorscroll != null)
                    {
                        //gridView.Settings.UseFixedTableLayout = true;
                        gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                        gridView.Width = System.Web.UI.WebControls.Unit.Percentage(100);

                    }

                }
                gridView.Width = Unit.Percentage(100);
                gridView.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;
                ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
                if (gridListEditor != null)
                {
                    //ASPxGridView gridView = gridListEditor.Grid;
                    //gridView.Settings.UseFixedTableLayout = true;
                    //gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    //gridView.Width = Unit.Percentage(100);
                    foreach (WebColumnBase column in gridView.Columns)
                    {
                        IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                        if (columnInfo != null)
                        {
                            //IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                            if (columnInfo.Model.Id == "SampleID")
                            {
                                column.Width = 125;
                            }
                            else
                            if ((columnInfo.Model.Id == "SampleCount" || columnInfo.Model.Id == "TestsCount" || columnInfo.Model.Id == "ContainerCount") && (View.Id == "Samplecheckin_ListView_Copy_Registration" || View.Id == "Samplecheckin_ListView_Copy_Registration_History"))
                            {
                                column.Width = 80;
                            }
                            else
                            if ((columnInfo.Model.Id == "SampleCount" || columnInfo.Model.Id == "TestsCount") && (/*View.Id == "Samplecheckin_ListView_Copy_Registration" ||*/ View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"))
                            {
                                column.Width = 80;
                            }
                            else
                            if (columnInfo.Model.Id == "ContainerCount" && (/*View.Id == "Samplecheckin_ListView_Copy_Registration" ||*/ View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"))
                            {
                                column.Width = 90;
                            }
                            else
                            if (columnInfo.Model.Id == "Status" && (/*View.Id == "Samplecheckin_ListView_Copy_Registration" ||*/ View.Id == "Samplecheckin_ListView_Copy_Registration_History" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"))
                            {
                                column.Width = 130;
                            }
                            else if (View.Id == "CustomReportBuilder_ListView" && (columnInfo.Model.Id == "DisplayName" || columnInfo.Model.Id == "CustomReportName" || columnInfo.Model.Id == "ViewDataSource" || columnInfo.Model.Id == "BusinessObjectName" || columnInfo.Model.Id == "ShowReportID"))
                            {
                                if (columnInfo.Model.Id == "DisplayName")
                                {
                                    column.Width = 200;
                                }
                                else
                                {
                                    column.Width = 150;
                                }

                            }
                            //else
                            //if (columnInfo.Model.Id == "PackageNo" && View.Id == "Samplecheckin_ListView_Copy_Registration")
                            //{
                            //    column.Width = 200;
                            //}
                            //else
                            //if ((columnInfo.Model.Id == "CollectionDate" || columnInfo.Model.Id == "DueDate" || columnInfo.Model.Id == "JobID") && View.Id == "Samplecheckin_ListView_Copy_Registration")
                            //{
                            //    column.Width = 100;
                            //}
                            else
                            if (columnInfo.Model.Id == "JobID" && (/*View.Id == "Samplecheckin_ListView_Copy_Registration" ||*/ View.Id == "Samplecheckin_ListView_Copy_Registration_History"))
                            {
                                column.Width = 120;
                            }
                            else
                            if ((columnInfo.Model.Id == "ReceivedDate" || columnInfo.Model.Id == "RecievedDate" || columnInfo.Model.Id == "ReceiveDate" || columnInfo.Model.Id == "SCDateReceived") && (/*View.Id == "Samplecheckin_ListView_Copy_Registration" ||*/ View.Id == "Samplecheckin_ListView_Copy_Registration_History" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main"
                                || View.Id == "SampleParameter_ListView_Copy_ResultView_Main" || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ClientName" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry_Main" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultView_Main" || View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel" ||
                                View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel" || View.Id == "SampleParameter_ListView_Copy_ResultView"))
                            {
                                column.Width = 250;
                            }
                            else
                            if (columnInfo.Model.Id == "Client" && View.Id == "SampleLogIn_ListView_SampleDisposition")
                            {
                                column.Width = 250;
                            }
                            else if (columnInfo.Model.Id == "Client" && (View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Account" || View.Id == "Reporting_ListView_Recall"))
                            {
                                column.Width = 200;
                            }
                            else if (columnInfo.Model.Id == "Email" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Invoicing_ListView_Delivery"))
                            {
                                column.Width = 200;
                            }
                            else if (columnInfo.Model.Id == "PrintedBy" && View.Id == "Reporting_ListView_PrintDownload")
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "StoreLocation" && View.Id == "Reporting_ListView_PrintDownload")
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "LastUpdatedBy" && (View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Recall" || View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Reporting_ListView_Account" || View.Id == "Reporting_ListView_Archieve"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "LastUpdatedDate" && (View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Recall" || View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Reporting_ListView_Account" || View.Id == "Reporting_ListView_Archieve"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "ProjectReportID" && (View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Recall" || View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Reporting_ListView_Account" || View.Id == "Reporting_ListView_Archieve"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "ReceivedBy" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Reporting_ListView_Archieve" || View.Id == "Reporting_ListView_Recall"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "DeliveredBy" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Reporting_ListView_Recall"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "HandledBy" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "HandledBy" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "DeliveryMethod" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired"))
                            {
                                column.Width = 120;
                            }
                            else if (columnInfo.Model.Id == "Delivery Copies" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired"))
                            {
                                column.Width = 120;
                            }
                            else if (columnInfo.Model.Id == "Date Delivered" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired"))
                            {
                                column.Width = 120;
                            }
                            else if (columnInfo.Model.Id == "DeliveryAddress" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired"))
                            {
                                column.Width = 250;
                            }
                            else if (columnInfo.Model.Id == "EmailAddress" && (View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Deliveired" || View.Id == "Reporting_ListView_Recall"))
                            {
                                column.Width = 250;
                            }
                            else if (columnInfo.Model.Id == "ArchiveLocation" && (View.Id == "Reporting_ListView_Archieve" || View.Id == "Reporting_ListView_Account"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "ArchivedBy" && (View.Id == "Reporting_ListView_Archieve" || View.Id == "Reporting_ListView_Account"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "DeliveryDeleteReason" && View.Id == "Reporting_ListView_Account")
                            {
                                column.Width = 250;
                            }
                            else if (columnInfo.Model.Id == "ArchiveDeleteReason" && View.Id == "Reporting_ListView_Account")
                            {
                                column.Width = 250;
                            }
                            else if (columnInfo.Model.Id == "RecalledBy" && View.Id == "Reporting_ListView_Recall")
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "Upload COC" && (/*View.Id == "Samplecheckin_ListView_Copy_Registration" ||*/ View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"))
                            {
                                column.Width = 120;
                            }
                            else
                            if (columnInfo.Model.Id == "InspectionCategory" && (/*View.Id == "Samplecheckin_ListView_Copy_Registration" ||*/ View.Id == "Samplecheckin_ListView_Copy_Registration_History" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"))
                            {
                                column.Width = 140;
                            }
                            else
                            if (columnInfo.Model.Id == "TestDescription" && (/*View.Id == "Samplecheckin_ListView_Copy_Registration" || */View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"))
                            {
                                column.Width = 200;
                            }
                            else
                            if (column.Caption == "Parameter" && View.Id == "QCTestParameter_ListView_Copy")
                            {
                                column.Width = 150;
                            }
                            else
                            if (column.Caption == "User" && View.Id == "UserNavigationPermission_ListView")
                            {
                                column.Width = 870;
                            }
                            else
                           if (columnInfo.Model.Id == "TestName" && View.Id == "TestMethod_ListView")
                            {
                                //column.Width = 250;
                                column.Width = 250;
                            }
                            else
                           if (columnInfo.Model.Id == "MethodName" && View.Id == "TestMethod_ListView")
                            {
                                //column.Width = 250;
                                column.Width = Unit.Percentage(15);
                            }
                            else
                           if (columnInfo.Model.Id == "TestCode" && View.Id == "TestMethod_ListView")
                            {
                                //column.Width =100;
                                column.Width = Unit.Percentage(8);
                            }
                            else
                           if (columnInfo.Model.Id == "MatrixName" && View.Id == "TestMethod_ListView")
                            {
                                //column.Width =250;
                                column.Width = Unit.Percentage(15);
                            }
                            else
                           if (columnInfo.Model.Id == "IsGroup" && View.Id == "TestMethod_ListView")
                            {
                                //column.Width =250;
                                column.Width = Unit.Percentage(14);
                            }
                            else
                           if (columnInfo.Model.Id == "Components" && View.Id == "TestMethod_ListView")
                            {
                                //column.Width = 300;
                                column.Width = Unit.Percentage(15);
                            }
                            else
                           if (columnInfo.Model.Id == "Category" && View.Id == "TestMethod_ListView")
                            {
                                //column.Width = 300;
                                column.Width = Unit.Percentage(15);
                            }
                            else
                            if (columnInfo.Model.Id == "ClientName" && (View.Id == "Customer_ListView" || /*View.Id == "Samplecheckin_ListView_Copy_Registration" ||*/ View.Id == "Samplecheckin_ListView_Copy_Registration_History" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"))
                            {
                                column.Width = 250;
                            }
                            else
                            if (column.Caption == "Phone" && View.Id == "Customer_ListView")
                            {
                                column.Width = 120;
                            }
                            else
                            if (column.Caption == "Fax" && View.Id == "Customer_ListView")
                            {
                                column.Width = 120;
                            }
                            else
                            if (column.Caption == "Address1" && View.Id == "Customer_ListView")
                            {
                                column.Width = 200;
                            }
                            else
                            if (column.Caption == "ClientCode" && View.Id == "Customer_ListView")
                            {
                                column.Width = 70;
                            }
                            else
                            if (column.Caption == "FullName" && View.Id == "Employee_ListView")
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "Active" && View.Id == "Method_ListView")
                            {
                                column.Width = 80;
                            }
                            else
                            if (columnInfo.Model.Id == "MethodName" && View.Id == "Method_ListView")
                            {
                                column.Width = 170;
                            }
                            else
                            if (columnInfo.Model.Id == "MethodNumber" && View.Id == "Method_ListView")
                            {
                                column.Width = 150;
                            }
                            if ((columnInfo.Model.Id == "StandardClause" || columnInfo.Model.Id == "ClauseSubject") && View.Id == "Method_ListView")
                            {
                                column.Width = 120;
                            }
                            else
                            if (columnInfo.Model.Id == "MethodName" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultView"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"
                                || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                            {
                                column.Width = 250;
                            }
                            else
                            if (columnInfo.Model.Id == "ClientSampleID" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" ||
                                //if (columnInfo.Model.Id == "ClientSampleID" && (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration" || View.Id == "SampleParameter_ListView_Copy_ResultEntry" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleLogIn_ListView_SampleDisposition"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                            {
                                column.Width = 200;
                            }
                            else
                            if (columnInfo.Model.Id == "ReportName" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView" || View.Id == "Reporting_ListView_PrintDownload"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ContactName" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView" || View.Id == "Reporting_ListView_PrintDownload"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "CompanyID" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"))
                            {
                                column.Width = 200;
                            }
                            else
                            if (columnInfo.Model.Id == "ReportValidatedBy" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ReportApprovedBy" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ReportedBy" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "Status" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"
                                || View.Id == "SampleParameter_ListView_Copy_ResultView_Main" || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"))
                            {
                                column.Width = 200;
                            }
                            else
                            if (columnInfo.Model.Id == "ReportStatus" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ReportedDate" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"))
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "ReportValidatedDate" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ReportApprovedDate" && (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Level1Review_View" || View.Id == "Reporting_ListView_Level2Review_View" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_Copy_ReportView"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "AnalyzedDate" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                                || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultView"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"
                                || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "AnalyzedBy" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                                || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultView"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                  || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"
                                || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "Test" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                                || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultView"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                  || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"
                                || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                            {
                                column.Width = 150;
                            }
                            else if (columnInfo.Model.Id == "SysSampleCode" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "Parameter" && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                                || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_QCResultView"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation" ||
                                View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                  || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"
                                || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ValidatedBy" && (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                  || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ApprovedBy" && (View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ValidatedDate" && (View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                 || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                  || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"))
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "ApprovedDate" && (View.Id == "SampleParameter_ListView_Copy_ResultApproval" || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"))
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "QCBatchID" && (View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultView"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"))
                            {
                                column.Width = 110;
                            }
                            else
                            if (columnInfo.Model.Id == "SYSSamplecode" && (View.Id == "SampleParameter_ListView_Copy_QCResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultValidation"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultView"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                                 || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"))
                            {
                                column.Width = 200;
                            }
                            else
                            if (columnInfo.Model.Id == "ID" && (View.Id == "ClauseInspectionSettings_ListView" || View.Id == "IndoorInspection_ClauseInspections_ListView"))
                            {
                                column.Width = 70;
                            }
                            else
                            if (columnInfo.Model.Id == "Clause" && (View.Id == "ClauseInspectionSettings_ListView" || View.Id == "IndoorInspection_ClauseInspections_ListView"))
                            {
                                column.Width = 175;
                            }
                            else
                            if (columnInfo.Model.Id == "Category" && View.Id == "ClauseInspectionSettings_ListView")
                            {
                                column.Width = 175;
                            }
                            else
                            if (columnInfo.Model.Id == "Description" && View.Id == "ClauseInspectionSettings_ListView")
                            {
                                column.Width = 450;
                            }
                            else
                            if (columnInfo.Model.Id == "Requirement" && View.Id == "IndoorInspection_ClauseInspections_ListView")
                            {
                                column.Width = 450;
                            }
                            else
                            if (columnInfo.Model.Id == "Result" && View.Id == "IndoorInspection_ClauseInspections_ListView")
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "Conclusion" && View.Id == "IndoorInspection_ClauseInspections_ListView")
                            {
                                column.Width = 140;
                            }
                            else
                            if (columnInfo.Model.Id == "DateInspected" && View.Id == "IndoorInspection_Notepads_ListView")
                            {
                                column.Width = 175;
                            }
                            else
                            if (columnInfo.Model.Id == "Inspector" && View.Id == "IndoorInspection_Notepads_ListView")
                            {
                                column.Width = 175;
                            }
                            else
                            if (columnInfo.Model.Id == "Note" && View.Id == "IndoorInspection_Notepads_ListView")
                            {
                                column.Width = 600;
                            }
                            //else
                            //if (columnInfo.Model.Id == "SamplingLocation" && View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                            //{
                            //    column.Width = 150;
                            //}
                            else
                            if (columnInfo.Model.Id == "PendingEntrySamplesCount" && View.Id == "QCBatch_ListView_ResultEntry")
                            {
                                column.Width = 90;
                            }
                            else
                            if (columnInfo.Model.Id == "QCBatchID" && View.Id == "QCBatch_ListView_ResultEntry")
                            {
                                column.Width = 150;
                            }
                            else
                            if (columnInfo.Model.Id == "CreatedBy" && View.Id == "QCBatch_ListView_ResultEntry")
                            {
                                column.Width = 120;
                            }
                            else
                            if (columnInfo.Model.Id == "Datecreated" && View.Id == "QCBatch_ListView_ResultEntry")
                            {
                                column.Width = 130;
                            }
                            if (columnInfo.Model.Id == "Method" && View.Id == "QCBatch_ListView_ResultEntry")
                            {
                                column.Width = 260;
                            }
                            else
                            if (columnInfo.Model.Id == "RequestedDate" && View.Id == "Requisition_ListView")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "Vendor" && View.Id == "Requisition_ListView_Review")
                            {
                                column.Width = 70;
                            }

                            else
                            if (columnInfo.Model.Id == "StockQty" && View.Id == "Items_ListView_Copy_StockWatch")
                            {
                                column.Width = 90;
                            }
                            //else
                            //if (columnInfo.Model.Id == "Vendorcode" && View.Id == "Vendors_ListView")
                            //{
                            //    column.Width = 120;
                            //}
                            //else
                            //if (columnInfo.Model.Id == "MethodName" && View.Id == "TestMethod_ListView_AnalysisDepartmentChain")
                            //{
                            //    column.Width = 100;
                            //}
                            else
                            if (columnInfo.Model.Id == "Parameter" && View.Id == "Testparameter_ListView")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "SurrogateLowLimit" && View.Id == "TestParameter_ListView_DefaultSettings")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "FinalDefaultResult" && View.Id == "TestParameter_ListView_DefaultSettings_QC")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "SurrogateAmount" && View.Id == "TestParameter_ListView_DefaultSettings")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "SurrogateHighLimit" && View.Id == "TestParameter_ListView_DefaultSettings")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "FinalDefaultUnits" && View.Id == "TestParameter_ListView_DefaultSettings_QC")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "SpikeAmountUnit" && View.Id == "TestParameter_ListView_DefaultSettings_QC")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "RecLCLimit" && View.Id == "TestParameter_ListView_DefaultSettings_QC")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "RecHCLimit" && View.Id == "TestParameter_ListView_DefaultSettings_QC")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "RPDLCLimit" && View.Id == "TestParameter_ListView_DefaultSettings_QC")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "RPDHCLimit" && View.Id == "TestParameter_ListView_DefaultSettings_QC")
                            {
                                column.Width = 130;
                            }
                            else
                            if (columnInfo.Model.Id == "CreatedBy" && View.Id == "QCBatch_ListView_ResultValidation")
                            {
                                column.Width = 120;
                            }
                            else
                            if (columnInfo.Model.Id == "Datecreated" && View.Id == "QCBatch_ListView_ResultValidation")
                            {
                                column.Width = 120;
                            }
                            if (columnInfo.Model.Id == "Method" && View.Id == "QCBatch_ListView_ResultValidation")
                            {
                                column.Width = 330;
                            }




                            // if (columnInfo.Model.Id == "JobID" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(8);
                            // }
                            // if (columnInfo.Model.Id == "ClientSampleID" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(9);
                            // }
                            // if (columnInfo.Model.Id == "VisualMatrix" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(12);
                            // }
                            // if (columnInfo.Model.Id == "StorageID" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(10);
                            // }
                            // if (columnInfo.Model.Id == "PreserveCondition" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(12);
                            // }
                            // if (columnInfo.Model.Id == "DaysSampleKeeping" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(12);
                            // }
                            // if (columnInfo.Model.Id == "DisposedDate" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(10);
                            // }
                            // if (columnInfo.Model.Id == "DisposedBy" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(10);
                            // }

                            //if (columnInfo.Model.Id == "Client" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(17);
                            // }

                            //if (columnInfo.Model.Id == "ProjectID" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(10);
                            // }

                            //if (columnInfo.Model.Id == "ProjectName" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(12);
                            // }
                            // if (columnInfo.Model.Id == "DaysRemaining" && View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                            // {
                            //     column.Width = Unit.Percentage(10);
                            // }

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
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        #endregion
    }
}
