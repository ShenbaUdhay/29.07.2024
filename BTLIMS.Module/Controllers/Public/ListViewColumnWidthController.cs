using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Web;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Drawing;
using System.Web.UI.WebControls;

namespace ALPACpre.Module.Controllers.Public
{
    public partial class ListViewColumnWidthController : ViewController<ListView>
    {
        MessageTimer timer = new MessageTimer();
        viewInfo tempviewinfo = new viewInfo();
        public ListViewColumnWidthController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View != null && !string.IsNullOrEmpty(View.Id))
                {
                    tempviewinfo.strtempviewid = View.Id.ToString();
                    tempviewinfo.strsampleviewtype = View.ObjectTypeInfo.Name.ToString();
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
                ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                if (editor != null && editor.Grid != null && View != null)
                {
                    ASPxGridView gridView = editor.Grid;
                    gridView.Settings.UseFixedTableLayout = false;
                    string strwidth = System.Web.HttpContext.Current.Request.Cookies.Get("width").Value;
                    string strscreenwidth = System.Web.HttpContext.Current.Request.Cookies.Get("screenwidth").Value;
                    string strheight = System.Web.HttpContext.Current.Request.Cookies.Get("height").Value;
                    string strscreenheight = System.Web.HttpContext.Current.Request.Cookies.Get("screenheight").Value;

                    #region HorizontalScrollBarMode
                    if (View.Id == "Method_ListView" 
                        || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History" 
                        || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView" 
                        || View.Id == "TestMethod_ListView_AnalysisQueue" 
                        || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice" 
                        || View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice"
                        || View.Id == "Notes_Employee_ListView"
                        || View.Id == "Customer_InvoicingContact_ListView" 
                        || View.Id == "Customer_InvoicingAddress_ListView" 
                        || View.Id == "SampleSites_ListView" 
                        || View.Id == "TestPriceSurcharge_ListView" 
                        || View.Id == "Testparameter_ListView" 
                        || View.Id == "TestMethod_ListView"
                        || View.Id == "AnalysisPricing_ListView_Quotes" 
                        || View.Id == "CRMQuotes_AnalysisPricing_ListView" 
                        || View.Id == "CRMQuotes_AnalysisPricing_ListView_ViewMode" 
                        || View.Id == "Reporting_ListView_Deliveired" 
                        || View.Id == "Tasks_ListView_Copy_ProposalValidation" 
                        || View.Id == "Tasks_ListView" 
                        || View.Id == "AnalysisPricing_ListView_Quotes_SampleRegistration"
                        || View.Id == "Contract_ListView_ContractTrcking_Copy" 
                        || View.Id == "AnalysisPricing_LookupListView" 
                        || View.Id == "CRMQuotes_AnalysisPricing_ListView" 
                        || View.Id == "TestPriceSurcharge_ListView_Edit"
                        || View.Id == "ContainerSettings_ListView_testmethod"
                        || View.Id == "ContainerSettings_ListView_testmethod_Edit" 
                        || View.Id == "TestPrice_ListView_Copy_perparameter" 
                        || View.Id == "Reporting_ListView" 
                        || View.Id == "Reporting_ListView_Copy_ReportApproval"
                        || View.Id == "Reporting_ListView_PrintDownload" 
                        || View.Id == "Labware_TestItems_ListView" 
                        || View.Id == "Labware_TestMethods_ListView" 
                        || View.Id == "TestMethod_surrogates_ListView" 
                        || View.Id == "Distribution_ListView_Consumption_Tracking" 
                        || View.Id == "Distribution_ListView_LTsearch" 
                        || View.Id == "Requisition_ListView_History"
                        || (View.Id == "Requisition_ListView" && Convert.ToInt32(strscreenwidth) < 1700) 
                        || (View.Id == "Requisition_ListViewEntermode" && Convert.ToInt32(strscreenwidth) < 1700)
                        || (View.Id == "VendorReagentCertificate_ListView" && Convert.ToInt32(strscreenwidth) < 1700) 
                        || View.Id == "Requisition_ListView_Receive"
                         || (View.Id == "DOC_ListView" && Convert.ToInt32(strscreenwidth) < 1500)
                         || (View.Id == "SampleParameter_ListView_Copy_DOC" && Convert.ToInt32(strscreenwidth) < 1480)
                        || View.Id == "Requisition_ListView_ViewMode"
                        || View.Id == "Sample_Reconciliation_General_Information_ListView" 
                        || ((View.Id == "SampleLogIn_ListView_SampleDisposition" 
                        || View.Id == "SampleLogIn_ListView_SampleDisposition_DisposedSamples") && Convert.ToInt32(strscreenwidth) < 1700) 
                        || (View.Id == "Reporting_ListView_Copy_ReportView" && Convert.ToInt32(strscreenwidth) < 1800)
                        //|| View.Id == "Reporting_ListView_Delivery" 
                        || (View.Id == "Distribution_ListView_ReceiveView" && Convert.ToInt32(strscreenwidth) < 1800)
                         || (View.Id == "Distribution_ListView_Copy_ExpirationAlert" && Convert.ToInt32(strscreenwidth) < 1700)
                         || (View.Id == "Distribution_ListView_Fractional_Consumption" && Convert.ToInt32(strscreenwidth) < 1500)
                        || View.Id == "SubOutContractLab_ListView"
                        || View.Id == "CRMProspects_ListView_Copy_Closed" 
                        || View.Id == "CRMProspects_ListView_MyOpenLeads_Copy" 
                        || View.Id == "CRMProspects_ListView" 
                        || View.Id == "Customer_ListView_InvoiceAddress" 
                        || View.Id == "Customer_SampleSites_ListView" 
                        || View.Id == "Customer_ReportingContact_ListView"
                         || (View.Id == "Distribution_ListView_Fractional_Consumption_History" && Convert.ToInt32(strscreenwidth) < 1500)
                         || (View.Id == "Requisition_ListView_Tracking" && Convert.ToInt32(strscreenwidth) < 1700)
                         || (View.Id == "Distribution_ListView_ConsumptionViewmode" && Convert.ToInt32(strscreenwidth) < 1700)
                         || (View.Id == "Purchaseorder_Item_ListView" && Convert.ToInt32(strscreenwidth) < 1500)
                         || (View.Id == "Requisition_ListView_Review" && Convert.ToInt32(strscreenwidth) < 1700)
                         || (View.Id == "Requisition_ListViewEntermode" && Convert.ToInt32(strscreenwidth) < 1700)
                         || (View.Id == "Items_ListView_Copy_StockWatch" && Convert.ToInt32(strscreenwidth) < 1500)
                         || (View.Id == "Distribution_ListView_Disposal" && Convert.ToInt32(strscreenwidth) < 1500)
                         || (View.Id == "Distribution_ListView" && Convert.ToInt32(strscreenwidth) < 1700)
                        || View.Id == "Distribution_ListView_Viewmode"
                         || (View.Id == "Items_ListView_Copy_StockAlert" && Convert.ToInt32(strscreenwidth) < 1700)
                         || (View.Id == "Requisition_ListView_Cancelled" && Convert.ToInt32(strscreenwidth) < 1700)
                         || (View.Id == "Distribution_itemDepletionsCollection_ListView_History" && Convert.ToInt32(strscreenwidth) < 1500)
                        || (View.Id == "Customer_ListView" && Convert.ToInt32(strscreenwidth) < 1700) 
                        || View.Id == "Contact_ListView"
                        || View.Id == "SampleParameter_ListView_Copy_ResultEntry" 
                        || View.Id == "SampleParameter_ListView_Copy_ResultView" 
                        || View.Id == "SampleParameter_ListView_Copy_QCResultEntry"
                        || View.Id == "SampleParameter_ListView_Copy_QCResultView" 
                        || View.Id == "SampleParameter_ListView_Copy_ResultValidation" 
                        || View.Id == "SampleParameter_ListView_Copy_QCResultValidation" 
                        || View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                                || View.Id == "Sampling_ListView"
                                || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultValidation_Level1Review"
                                || View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval_Level2Review"
                                || View.Id == "SampleParameter_ListView_Copy_QCResultApproval"
                        || (View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" && Convert.ToInt32(strscreenwidth) < 1535)
                                || View.Id == "Tasks_BottlesOrders_ListView"
                        || View.Id == "Tasks_ListView_PrintingSampleLabels" 
                        || View.Id == "Tasks_ListView_PropsalCancellation" 
                        || View.Id == "Tasks_ListView_SamplingAssignment" 
                        || ((View.Id == "Samplecheckin_ListView_Copy_Registration_History" || View.Id == "Samplecheckin_ListView_Copy_Registration") && Convert.ToInt32(strscreenwidth) < 1600)
                        || View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples" 
                        || View.Id == "Tasks_ListView_SampleTransfer" 
                        || (View.Id == "SubOutSampleRegistrations_ListView_PendingSignOff" && Convert.ToInt32(strscreenwidth) < 1500) 
                        //|| (View.Id == "SubOutSampleRegistrations_ListView_Level2DataReview" && Convert.ToInt32(strscreenwidth) < 3500) 
                        || (View.Id == "SubOutSampleRegistrations_ListView_Level3DataReview" && Convert.ToInt32(strscreenwidth) < 1500) 
                        || (View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue" && Convert.ToInt32(strscreenwidth) < 1500)
                        || (View.Id == "SubOutSampleRegistrations_ListView" && Convert.ToInt32(strscreenwidth) < 1450) 
                        || View.Id == "SampleWeighingBatch_ListView" 
                        || View.Id == "SampleWeighingBatchSequence_ListView_Tracking" 
                        || View.Id == "Distribution_ListView_Consumption" 
                        || View.Id == "DefaultPricing_ListView_Copy" 
                        || View.Id == "DefaultPricing_ListView" 
                        || View.Id == "ConstituentPricing_ListView" 
                        || View.Id == "Samplecheckin_ListView_SampleReceiptNotification" 
                        || View.Id == "Samplecheckin_ListView_SampleReceiptNotification_History"
                                || View.Id == "tbl_Public_CustomReportDesignerDetails_ListView"
                                || (View.Id == "QCBatchSequence_ListView_Copy" && Convert.ToInt32(strscreenwidth) < 100)
                        || View.Id == "Contract_ListView_ContractTrcking_Copy_DC" 
                        || View.Id == "Contact_ListView_Copy_DC" 
                        || (View.Id == "Customer_Note_Listview_Copy_DC" && Convert.ToInt32(strscreenwidth) < 100) 
                        || (View.Id == "Customer_ListView_Copy_DC" && Convert.ToInt32(strscreenwidth) < 150)
                        || (View.Id == "Items_ListView_Copy_DC" && Convert.ToInt32(strscreenwidth) < 100) 
                        || View.Id == "Parameter_ListView_Copy_QC"
                        || View.Id == "InvoicingAnalysisCharge_ListView_Invoice"
                        || View.Id == "Testparameter_ListView_Copy_DataCenter"
                        || View.Id == "QCparameter_ListView_Copy_DataCenter"
                        || (View.Id == "DefaultPricing_ListView_DataCenter" && Convert.ToInt32(strscreenwidth) < 1000)
                        || View.Id == "ConstituentPricing_ListView_DataCenter" 
                        || View.Id == "Vendors_ListView_DataCenter"
                                || View.Id == "Container_ListView_DataCenter"
                                || View.Id == "Employee_ListView"
                        || View.Id == "Testparameter_ListView_Test_Surrogates_DataCenter"
                        || View.Id == "RoleNavigationPermission_ListView_DataCenter"
                                 || View.Id == "SampleLogIn_ListView_DataCenter"
                        || (View.Id == "Samplecheckin_ListView_DataCenter" && Convert.ToInt32(strscreenwidth) < 1000)
                                 || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_Review"
                                 || View.Id == "InvoicingAnalysisCharge_ListView_Invoice_View"
                                 || View.Id == "InvoicingAnalysisCharge_ListView_Queue"
                                   || View.Id == "Testparameter_ListView_Test_SampleParameter_DataCenter"
                                 || View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault_DataCenter"
                        || (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults" && Convert.ToInt32(strscreenwidth) < 100)
                        || View.Id == "SampleParameter_ListView_Copy_DC_ResultSample" 
                        || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC"
                        || (View.Id == "MaintenanceTaskCheckList_ListView_MaintenanceQueue" && Convert.ToInt32(strscreenwidth) < 2000)
                                 || (View.Id == "FiberTypes_ListView" && Convert.ToInt32(strscreenwidth) < 1600)
                        || View.Id == "VendorReagentCertificate_LT_ListView"
                        || View.Id == "SDMSDCAB_ListView_EDDReportGenerator" 
                        || View.Id == "AnalysisPricing_ListView_Quotes_SamplingProposal"
|| View.Id == "SampleParameter_ListView_Copy_DOC"
                                 )
                    {
                        gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                    }
                    #endregion

                    gridView.Width = Unit.Percentage(100);
                    gridView.SettingsResizing.ColumnResizeMode = ColumnResizeMode.Control;

                    if (View is ListView)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor.Grid.Columns["JobID"] != null)
                        {
                            gridListEditor.Grid.Columns["JobID"].Width = 110;
                        }
                        if (gridListEditor.Grid.Columns["SampleID"] != null)
                        {
                            gridListEditor.Grid.Columns["SampleID"].Width = 125;
                        }
                        if (gridListEditor.Grid.Columns["QCBatchID"] != null)
                        {
                            gridListEditor.Grid.Columns["QCBatchID"].Width = 120;
                        }
                        if (gridListEditor.Grid.Columns["QcBatchID"] != null)
                        {
                            gridListEditor.Grid.Columns["QcBatchID"].Width = 120;
                        }
                    }
                    if (Frame != null && Frame.GetType() != typeof(NestedFrame) && Frame.GetType() != typeof(DevExpress.Web.PopupWindow))
                    {
                        if (View.Id != "SampleParameter_ListView_Copy_Reporting_MainView" && View.CollectionSource != null && View.CollectionSource.List != null && View.CollectionSource.List.Count > 20)
                        {
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                            editor.Grid.Settings.VerticalScrollableHeight = 600;
                        }
                    }
                    if (View.Id == "DataSource_ListView" && View.CollectionSource.List != null && View.CollectionSource.List.Count > 0)
                    {
                        editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                        editor.Grid.Settings.VerticalScrollableHeight = 20;
                    }
                        if (View.Id == "SampleParameter_ListView_Copy_DOC_Lookup")
                        {
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                            editor.Grid.Settings.VerticalScrollableHeight = 100;
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_DC_ResultSample" || View.Id == "SampleParameter_ListView_Copy_DC_ResultQC" || View.Id == "Labware_ListView"
                            /*|| View.Id == "SampleParameter_ListView_Copy_TrendAnalysis" */|| View.Id == "LabwareCertificate_ListView")
                        {
                            gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                            foreach (WebColumnBase column in gridView.Columns)
                            {
                                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)editor).GetColumnInfo(column);
                                if (columnInfo != null)
                                {
                                    IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                                    column.Width = Unit.Pixel(modelColumn.Width);
                                }
                            }
                            if (editor.Grid.Columns["JobID"] != null && View.Id == "SampleParameter_ListView_Copy_DC_ResultSample")
                            {
                                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["SampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["SampleName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["UQABID"] != null && View.Id == "SampleParameter_ListView_Copy_DC_ResultQC")
                            {
                                gridView.VisibleColumns["QCBatchID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["SampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["SampleName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && View.Id == "Labware_ListView")
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["InlineEditCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                if (editor.Grid.Columns["Edit"] != null)
                                {
                                    gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                gridView.VisibleColumns["LabwareID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["InstrumentName"].FixedStyle = GridViewColumnFixedStyle.Left;
                                editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            }
                        }
                    else if (View.Id == "SampleBottleAllocation_ListView_SampleTransfer")
                    {
                        editor.Grid.Settings.VerticalScrollableHeight = 330;
                        if (editor.Grid.Columns["SampleID"] != null)
                        {
                            editor.Grid.Columns["SampleID"].Width = Unit.Percentage(10);
                        }
                        if (editor.Grid.Columns["SampleName"] != null)
                        {
                            editor.Grid.Columns["SampleName"].Width = Unit.Percentage(10);
                        }
                        if (editor.Grid.Columns["Test"] != null)
                        {
                            editor.Grid.Columns["Test"].Width = Unit.Percentage(23);
                        }
                        if (editor.Grid.Columns["BottleID"] != null)
                        {
                            editor.Grid.Columns["BottleID"].Width = Unit.Percentage(8);
                        }
                        if (editor.Grid.Columns["SampleStatus"] != null)
                        {
                            editor.Grid.Columns["SampleStatus"].Width = Unit.Percentage(11);
                        }
                        if (editor.Grid.Columns["IsRemark"] != null)
                        {
                            editor.Grid.Columns["IsRemark"].Width = Unit.Percentage(8);
                        }
                        if (editor.Grid.Columns["ReceivedBy"] != null)
                        {
                            editor.Grid.Columns["ReceivedBy"].Width = Unit.Percentage(11);
                        }
                        if (editor.Grid.Columns["ReceivedDate"] != null)
                        {
                            editor.Grid.Columns["ReceivedDate"].Width = Unit.Percentage(11);
                        }
                        if (editor.Grid.Columns["ScanDateTime"] != null)
                        {
                            editor.Grid.Columns["ScanDateTime"].Width = Unit.Percentage(11);
                        }
                    }
                        else if (View.Id == "DailyQC_ListView_Data")
                        {
                            editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            gridView.SettingsPager.AlwaysShowPager = false;
                            gridView.SettingsPager.PageSizeItemSettings.Visible = false;
                            editor.Grid.Settings.VerticalScrollableHeight = 330;
                            if (editor.Grid.Columns["Date"] != null)
                            {
                                editor.Grid.Columns["Date"].Width = Unit.Percentage(21);
                            }
                            if (editor.Grid.Columns["RefValue"] != null)
                            {
                                editor.Grid.Columns["RefValue"].Width = Unit.Percentage(21);
                            }
                            if (editor.Grid.Columns["Reading"] != null)
                            {
                                editor.Grid.Columns["Reading"].Width = Unit.Percentage(17);
                            }
                            if (editor.Grid.Columns["Mean"] != null)
                            {
                                editor.Grid.Columns["Mean"].Width = Unit.Percentage(15);
                            }
                            if (editor.Grid.Columns["UCL"] != null)
                            {
                                editor.Grid.Columns["UCL"].Width = Unit.Percentage(13);
                            }
                            if (editor.Grid.Columns["LCL"] != null)
                            {
                                editor.Grid.Columns["LCL"].Width = Unit.Percentage(13);
                            }
                        }
                        else if (View.Id == "DailyQC_ListView")
                        {
                            if (editor.Grid.Columns["InstrumentID"] != null)
                            {
                                editor.Grid.Columns["InstrumentID"].Width = Unit.Percentage(10);
                            }
                            if (editor.Grid.Columns["InstrumentName"] != null)
                            {
                                editor.Grid.Columns["InstrumentName"].Width = Unit.Percentage(10);
                            }
                        }        
                        else if (View.Id == "SampleLogIn_ListView_SamplingAllocation")
                        {
                            if (editor.Grid.Columns["SamplingEquipment"] != null)
                            {
                                editor.Grid.Columns["SamplingEquipment"].Width = Unit.Percentage(15);
                            }
                            
                        }
                        else if (View.Id == "CalibrationSettings_ListView_Trending_Data")
                        {
                            editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            editor.Grid.Settings.VerticalScrollableHeight = 200;
                        }
                        else if (View.Id == "SamplePrepBatch_ListView_CopyFrom")
                        {
                            //  editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            // editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            //  editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                            // editor.Grid.Settings.VerticalScrollableHeight = 200;
                            if (editor.Grid.Columns["PrepBatchID"] != null)
                            {
                                editor.Grid.Columns["PrepBatchID"].Width = 120;
                            }

                            //if (editor.Grid.Columns["Matrix"] != null)
                            //{
                            //    editor.Grid.Columns["Matrix"].Width = 65;
                            //}

                            //if (editor.Grid.Columns["Test"] != null)
                            //{
                            //    editor.Grid.Columns["Test"].Width = 70;
                            //}

                            //if (editor.Grid.Columns["Method"] != null)
                            //{
                            //    editor.Grid.Columns["Method"].Width = 70;
                            //}

                            //if (editor.Grid.Columns["Datecreated"] != null)
                            //{
                            //    editor.Grid.Columns["Datecreated"].Width = 70;
                            //}

                            //if (editor.Grid.Columns["CreatedBy"] != null)
                            //{
                            //    editor.Grid.Columns["CreatedBy"].Width = 70;
                            //}
                        }
                        else if (View.Id == "Purchaseorder_DetailView")
                        {
                            if (editor.Grid.Columns["DeliveryPriority"] != null)
                            {
                                editor.Grid.Columns["DeliveryPriority"].Width = 140;
                            }
                            //    if (editor.Grid.Columns["Category"] != null)
                            //    {
                            //        editor.Grid.Columns["Category"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["Vendor"] != null)
                            //    {
                            //        editor.Grid.Columns["Vendor"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["Item"] != null)
                            //    {
                            //        editor.Grid.Columns["Item"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["Select"] != null)
                            //    {
                            //        editor.Grid.Columns["Select"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["AmountTaken"] != null)
                            //    {
                            //        editor.Grid.Columns["AmountTaken"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["RQID"] != null)
                            //    {
                            //        editor.Grid.Columns["RQID"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["AmountLeft"] != null)
                            //    {
                            //        editor.Grid.Columns["AmountLeft"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["POID"] != null)
                            //    {
                            //        editor.Grid.Columns["POID"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["DateOpened"] != null)
                            //    {
                            //        editor.Grid.Columns["DateOpened"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["ReceiveID"] != null)
                            //    {
                            //        editor.Grid.Columns["ReceiveID"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["givento"] != null)
                            //    {
                            //        editor.Grid.Columns["givento"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["Storage"] != null)
                            //    {
                            //        editor.Grid.Columns["Storage"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["Description"] != null)
                            //    {
                            //        editor.Grid.Columns["Description"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["Size"] != null)
                            //    {
                            //        editor.Grid.Columns["Size"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["ItemCode"] != null)
                            //    {
                            //        editor.Grid.Columns["ItemCode"].Width = 100;
                            //    }
                            if (editor.Grid.Columns["StockAmount1"] != null)
                            {
                                editor.Grid.Columns["StockAmount1"].Width = 150;
                            }
                            if (editor.Grid.Columns["OriginalAmount"] != null)
                            {
                                editor.Grid.Columns["OriginalAmount"].Width = 130;
                            }
                            //    if (editor.Grid.Columns["AmountUnits"] != null)
                            //    {
                            //        editor.Grid.Columns["AmountUnits"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["IsDeplete"] != null)
                            //    {
                            //        editor.Grid.Columns["IsDeplete"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["DateDepleted"] != null)
                            //    {
                            //        editor.Grid.Columns["DateDepleted"].Width = 100;
                            //    }
                            //    if (editor.Grid.Columns["DepletedBy"] != null)
                            //    {
                            //        editor.Grid.Columns["DepletedBy"].Width = 100;
                            //    }
                            if (editor.Grid.Columns["ItemReceivedSort"] != null)
                            {
                                editor.Grid.Columns["ItemReceivedSort"].Width = 140;
                            }
                            if (editor.Grid.Columns["Days Reamining"] != null)
                            {
                                editor.Grid.Columns["Days Reamining"].Width = 140;
                            }
                            //    if (editor.Grid.Columns["Parent"] != null)
                            //    {
                            //        editor.Grid.Columns["Parent"].Width = 100;
                        }

                        //}
                        else if (View.Id == "SampleParameter_ListView_Copy_TrendAnalysis")
                        {
                            foreach (WebColumnBase column in gridView.Columns)
                            {
                                IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)editor).GetColumnInfo(column);
                                if (columnInfo != null)
                                {
                                    IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                                    column.Width = Unit.Pixel(modelColumn.Width);
                                }
                            }
                            editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            editor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strscreenheight) - (Convert.ToInt32(strscreenheight) * 82 / 100);
                        }
                        else if (View.Id == "TrendAnalysis_ListView_DateVisualization")
                        {
                            editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowPager;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            //editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            editor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strscreenheight) - (Convert.ToInt32(strscreenheight) * 82 / 100);
                        }
                        else if (View.Id == "Requisition_ListView_PurchaseOrder_Lookup")
                        {
                            gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;

                        }
                        else if (View.Id == "SamplePrepBatch_Instruments_ListView")
                        {
                            editor.Grid.Columns["Maintenance Frequency"].Width = 170;
                        }
                        else if (View.Id == "QCType_ListView_SamplePrepBatchSequence_History")
                        {
                            editor.Grid.Columns["QCTypeName"].FixedStyle = GridViewColumnFixedStyle.Left;
                        }
                        else if (View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Final" || View.Id == "SpreadSheetBuilder_FieldSetUp_ListView_Final_Calibration")
                        {
                            editor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            //editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            editor.Grid.Settings.VerticalScrollableHeight = 400;

                            //int RowCount = ((ListView)View).CollectionSource.GetCount();
                            //if (RowCount > 0)
                            //{
                            //    editor.Grid.Settings.VerticalScrollableHeight = 300;
                            //    editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            //}
                        }

                        //else if(View.Id== "Distribution_itemDepletionsCollection_ListView" && Convert.ToInt32(strscreenwidth) < 1500)
                        //{
                        //    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;

                        //}
                        else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_Copy_SDMSBatchResults")
                        {
                            if (editor.Grid.Columns["DateCreated"] != null && Convert.ToInt32(strscreenwidth) < 1500)
                            {
                                editor.Grid.Columns["DateCreated"].Width = 120;
                            }
                        }
                        else if (View.Id == "Parameter_ListView")
                        {

                            int ColumnCount = ((ListView)View).CollectionSource.GetCount();
                            if (ColumnCount > 5)
                            {
                                editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            }
                            if (editor.Grid.Columns["ParameterName"] != null && Convert.ToInt32(strscreenwidth) < 1500)
                            {
                                editor.Grid.Columns["ParameterName"].Width = 250;
                            }
                            else if (editor.Grid.Columns["ParameterName"] != null && Convert.ToInt32(strscreenwidth) >= 1500)
                            {
                                editor.Grid.Columns["ParameterName"].Width = 300;
                            }

                            if (editor.Grid.Columns["Synonym"] != null && Convert.ToInt32(strscreenwidth) < 1500)
                            {
                                editor.Grid.Columns["Synonym"].Width = 250;
                            }
                            else if (editor.Grid.Columns["Synonym"] != null && Convert.ToInt32(strscreenwidth) >= 1500)
                            {
                                editor.Grid.Columns["Synonym"].Width = 300;
                            }
                            if (editor.Grid.Columns["Formula"] != null)
                            {
                                editor.Grid.Columns["Formula"].Width = 125;
                            }
                            if (editor.Grid.Columns["CAS"] != null)
                            {
                                editor.Grid.Columns["CAS"].Width = 125;
                            }
                            if (editor.Grid.Columns["MW"] != null)
                            {
                                editor.Grid.Columns["MW"].Width = 125;
                            }
                            if (editor.Grid.Columns["ParameterCode"] != null)
                            {
                                editor.Grid.Columns["ParameterCode"].Width = 125;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["InlineEditCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["InlineEditCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["ParameterName"] != null)
                            {
                                gridView.VisibleColumns["ParameterName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }

                        }
                        else if (View.Id == "ConstituentPricing_ListView")
                        {

                            int ColumnCount = ((ListView)View).CollectionSource.GetCount();
                            if (ColumnCount > 5)
                            {
                                editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            }

                        }
                        else if (View.Id == "TestPriceDetail_ListView_Copy_pertest")
                        {

                            int RowCount = ((ListView)View).CollectionSource.GetCount();
                            if (RowCount > 8)
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = 300;
                                editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            }

                        }
                        else if (View.Id == "PTStudyLog_Results_ListView")
                        {

                            int RowCount = ((ListView)View).CollectionSource.GetCount();
                            if (RowCount > 10)
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = 300;
                                editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            }

                        }
                        else if (View.Id == "Samplecheckin_ListView_SampleReceiptNotification" || View.Id == "Samplecheckin_ListView_SampleReceiptNotification_History")
                        {

                            //int RowCount = ((ListView)View).CollectionSource.GetCount();
                            //if (RowCount > 8)
                            //{
                            //    editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            //}
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["COC"] != null)
                            {
                                gridView.VisibleColumns["COC"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SampleReceipt"] != null)
                            {
                                gridView.VisibleColumns["SampleReceipt"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["MailContent"] != null)
                            {
                                gridView.VisibleColumns["MailContent"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }

                            if (editor.Grid.Columns["JobID"] != null)
                            {
                                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.VisibleColumns["Email"] != null)
                            {
                                gridView.VisibleColumns["Email"].Width = 200;
                            }
                        }
                        if (View.Id == "Distribution_ListView_Copy_ExpirationAlert")
                        {

                            if (editor.Grid.Columns["Items"] != null)
                            {
                                editor.Grid.Columns["Items"].Width = 200;
                            }
                            if (editor.Grid.Columns["Decription"] != null)
                            {
                                editor.Grid.Columns["Decription"].Width = 200;
                            }
                            if (editor.Grid.Columns["DaysRemaining"] != null)
                            {
                                editor.Grid.Columns["DaysRemaining"].Width = 130;
                            }

                        }
                        if(View.Id == "DOC_ListView")
                    {
                        if (editor.Grid.Columns["DOCID"] != null)
                        {
                            editor.Grid.Columns["DOCID"].Width = 130;
                        }
                        if (editor.Grid.Columns["Test"] != null)
                        {
                            editor.Grid.Columns["Test"].Width = 130;
                        }
                        if (editor.Grid.Columns["Method"] != null)
                        {
                            editor.Grid.Columns["Method"].Width = 130;
                        }
                        if (editor.Grid.Columns["Matrix"] != null)
                        {
                            editor.Grid.Columns["Matrix"].Width = 130;
                        }
                        if (editor.Grid.Columns["strJobID"] != null)
                        {
                            editor.Grid.Columns["strJobID"].Width = 130;
                        }
                        if (editor.Grid.Columns["Analyst"] != null)
                        {
                            editor.Grid.Columns["Analyst"].Width = 130;
                        }
                        if (editor.Grid.Columns["DateSubmitted"] != null)
                        {
                            editor.Grid.Columns["DateSubmitted"].Width = 130;
                        }
                        if (editor.Grid.Columns["DateAnalyzed"] != null)
                        {
                            editor.Grid.Columns["DateAnalyzed"].Width = 130;
                        }
                        if (editor.Grid.Columns["Status"] != null)
                        {
                            editor.Grid.Columns["Status"].Width = 130;
                        }
                        if (editor.Grid.Columns["DateValidated"] != null)
                        {
                            editor.Grid.Columns["DateValidated"].Width = 130;
                        }
                    }
                        if (View.Id == "Distribution_ListView" || View.Id == "Distribution_ListView_Viewmode")
                        {

                            if (editor.Grid.Columns["#ItemReceived"] != null)
                            {
                                editor.Grid.Columns["#ItemReceived"].Width = 150;
                            }
                            if (editor.Grid.Columns["DistributionDate"] != null)
                            {
                                editor.Grid.Columns["DistributionDate"].Width = 140;

                            }


                        }

                        else if (View.Id == "TestPriceDetail_ListView_Copy_perparameter")
                        {
                            int RowCount = ((ListView)View).CollectionSource.GetCount();
                            if (RowCount > 8)
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = 300;
                                editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            }
                            if (editor.Grid.Columns["BasicParamPrice"] != null)
                            {
                                editor.Grid.Columns["BasicParamPrice"].Width = 160;
                            }
                            if (editor.Grid.Columns["BasicParamPriceItemPerUnit"] != null)
                            {
                                editor.Grid.Columns["BasicParamPriceItemPerUnit"].Width = 180;
                            }
                            if (editor.Grid.Columns["AdditionalParamPriceItemPerUnit"] != null)
                            {
                                editor.Grid.Columns["AdditionalParamPriceItemPerUnit"].Width = 210;
                            }
                            if (editor.Grid.Columns["TAT"] != null)
                            {
                                editor.Grid.Columns["TAT"].Width = 160;
                            }
                            if (editor.Grid.Columns["Surcharge"] != null)
                            {
                                editor.Grid.Columns["Surcharge"].Width = 160;
                            }
                            if (editor.Grid.Columns["Priority"] != null)
                            {
                                editor.Grid.Columns["Priority"].Width = 160;
                            }
                        }
                        else if (View.Id == "Notes_Employee_ListView")
                        {
                            if (editor.Grid.Columns["JobTitle"] != null)
                            {
                                editor.Grid.Columns["JobTitle"].Width = 150;
                            }
                            if (editor.Grid.Columns["UserName"] != null)
                            {
                                editor.Grid.Columns["UserName"].Width = 150;
                            }
                            if (editor.Grid.Columns["Country"] != null)
                            {
                                editor.Grid.Columns["Country"].Width = 160;
                            }
                            if (editor.Grid.Columns["RoleNames"] != null)
                            {
                                editor.Grid.Columns["RoleNames"].Width = 160;
                            }
                            if (editor.Grid.Columns["Department"] != null)
                            {
                                editor.Grid.Columns["Department"].Width = 160;
                            }
                            if (editor.Grid.Columns["Position"] != null)
                            {
                                editor.Grid.Columns["Position"].Width = 160;
                            }
                            if (editor.Grid.Columns["RetireDate"] != null)
                            {
                                editor.Grid.Columns["RetireDate"].Width = 160;
                            }
                        }
                        else if (View.Id == "Qualifiers_ListView")
                        {
                            if (editor.Grid.Columns["QualifierID"] != null)
                            {
                                editor.Grid.Columns["QualifierID"].Width = 150;
                            }
                            if (editor.Grid.Columns["Symbol"] != null)
                            {
                                editor.Grid.Columns["Symbol"].Width = 80;
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = 150;
                            }
                            if (editor.Grid.Columns["Definition"] != null)
                            {
                                editor.Grid.Columns["Definition"].Width = Unit.Percentage(60);
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = Unit.Percentage(40);
                            }
                        }
                        else if (View.Id == "CrossChecklistSetup_ListView")
                        {
                            if (editor.Grid.Columns["Department"] != null)
                            {
                                editor.Grid.Columns["Department"].Width = 150;
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = 150;
                            }
                            if (editor.Grid.Columns["DPTest"] != null)
                            {
                                editor.Grid.Columns["DPTest"].Width = 250;
                            }
                            if (editor.Grid.Columns["Sort"] != null)
                            {
                                editor.Grid.Columns["Sort"].Width = 60;
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = 150;
                            }
                            if (editor.Grid.Columns["CheckPoint"] != null)
                            {
                                editor.Grid.Columns["CheckPoint"].Width = Unit.Percentage(50);
                            }
                            if (editor.Grid.Columns["DPTest"] != null)
                            {
                                editor.Grid.Columns["DPTest"].Width = Unit.Percentage(50);
                            }
                        }
                        else if (View.Id == "COCSettingsSamples_ListView")
                        {
                            if (editor.Grid.Columns["SamplingLocation"] != null)
                            {
                                editor.Grid.Columns["SamplingLocation"].Width = 180;
                            }
                        }
                        else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_DataPackageCrossChecks_ListView")
                        {
                            gridView.SettingsPager.AlwaysShowPager = false;
                            gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridView.SettingsPager.PageSizeItemSettings.Visible = false;

                            int RowCount = ((ListView)View).CollectionSource.GetCount();
                            if (RowCount >= 0 && !string.IsNullOrEmpty(strheight) && int.TryParse(strheight, out int a))
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strheight) - (Convert.ToInt32(strheight) * 65 / 100);
                                editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            }

                            if (editor.Grid.Columns["SortNo"] != null)
                            {
                                editor.Grid.Columns["SortNo"].Width = 70;
                            }
                            if (editor.Grid.Columns["CheckPoint"] != null)
                            {
                                editor.Grid.Columns["CheckPoint"].Width = Unit.Percentage(100);
                            }
                            if (editor.Grid.Columns["Yes"] != null)
                            {
                                editor.Grid.Columns["Yes"].Width = 60;
                            }
                            if (editor.Grid.Columns["No"] != null)
                            {
                                editor.Grid.Columns["No"].Width = 60;
                            }
                            if (editor.Grid.Columns["NA"] != null)
                            {
                                editor.Grid.Columns["NA"].Width = 60;
                            }
                            if (editor.Grid.Columns["Qualifier"] != null)
                            {
                                editor.Grid.Columns["Qualifier"].Width = 200;
                            }
                            if (editor.Grid.Columns["Remark"] != null)
                            {
                                editor.Grid.Columns["Remark"].Width = 250;
                            }
                        }

                        else if (View.Id == "Distribution_itemDepletionsCollection_ListView_History")
                        {

                            if (editor.Grid.Columns["Last Updated By"] != null)
                            {
                                editor.Grid.Columns["Last Updated By"].Width = 150;
                            }
                            if (editor.Grid.Columns["LastUpdatedDate"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedDate"].Width = 150;
                            }

                        }

                        else if (View.Id == "Contact_ListView_Invoice")
                        {
                            if (editor.Grid.Columns["InvoiceContact"] != null)
                            {
                                editor.Grid.Columns["InvoiceContact"].Width = Unit.Percentage(60);
                            }
                            if (editor.Grid.Columns["InvoiceMail"] != null)
                            {
                                editor.Grid.Columns["InvoiceMail"].Width = Unit.Percentage(70);
                            }
                            if (editor.Grid.Columns["InvoiceMailCC"] != null)
                            {
                                editor.Grid.Columns["InvoiceMailCC"].Width = Unit.Percentage(70);
                            }
                            if (editor.Grid.Columns["InvoiceMailBCC"] != null)
                            {
                                editor.Grid.Columns["InvoiceMailBCC"].Width = Unit.Percentage(70);
                            }
                            if (editor.Grid.Columns["InvoicePhone"] != null)
                            {
                                editor.Grid.Columns["InvoicePhone"].Width = Unit.Percentage(50);
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = Unit.Percentage(40);
                            }
                        }
                        else if (View.Id == "Contact_ListView_Report")
                        {
                            if (editor.Grid.Columns["ReportContact"] != null)
                            {
                                editor.Grid.Columns["ReportContact"].Width = Unit.Percentage(60);
                            }
                            if (editor.Grid.Columns["ReportMail"] != null)
                            {
                                editor.Grid.Columns["ReportMail"].Width = Unit.Percentage(70);
                            }
                            if (editor.Grid.Columns["ReportMailCC"] != null)
                            {
                                editor.Grid.Columns["ReportMailCC"].Width = Unit.Percentage(70);
                            }
                            if (editor.Grid.Columns["ReportMailBCC"] != null)
                            {
                                editor.Grid.Columns["ReportMailBCC"].Width = Unit.Percentage(70);
                            }
                            if (editor.Grid.Columns["ReportPhone"] != null)
                            {
                                editor.Grid.Columns["ReportPhone"].Width = Unit.Percentage(50);
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = Unit.Percentage(40);
                            }
                        }

                        else if (View.Id == "Qualifiers_ListView_DataPackage")
                        {
                            gridView.SettingsPager.AlwaysShowPager = false;
                            gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridView.SettingsPager.PageSizeItemSettings.Visible = false;
                            if (editor.Grid.Columns["QualifierID"] != null)
                            {
                                editor.Grid.Columns["QualifierID"].Width = 100;
                            }
                            if (editor.Grid.Columns["Symbol"] != null)
                            {
                                editor.Grid.Columns["Symbol"].Width = 65;
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = 120;
                            }
                            if (editor.Grid.Columns["Definition"] != null)
                            {
                                editor.Grid.Columns["Definition"].Width = Unit.Percentage(60);
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = Unit.Percentage(40);
                            }
                        }
                        else if (View.Id == "Testparameter_ListView_Test_InternalStandards" || View.Id == "Testparameter_ListView_Test_QCSampleParameter")
                        {
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 250;
                            }
                            if (editor.Grid.Columns["SpikeAmountUnits"] != null)
                            {
                                editor.Grid.Columns["SpikeAmountUnits"].Width = 150;
                            }
                            if (editor.Grid.Columns["RegulatoryLimit"] != null)
                            {
                                editor.Grid.Columns["RegulatoryLimit"].Width = 150;
                            }
                        }
                        else if (View.Id == "COCSettings_ListView")
                        {
                            if (editor.Grid.Columns["ProjectLocation"] != null)
                            {
                                editor.Grid.Columns["ProjectLocation"].Width = 115;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_RegistrationSignOffSamples")
                        {
                            if (editor.Grid.Columns["SampleID"] != null)
                            {
                                editor.Grid.Columns["SampleID"].Width = 115;
                            }
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 115;
                            }
                            if (editor.Grid.Columns["ReceivedBy"] != null)
                            {
                                editor.Grid.Columns["ReceivedBy"].Width = 115;
                            }
                            if (editor.Grid.Columns["DateReceived"] != null)
                            {
                                editor.Grid.Columns["DateReceived"].Width = 115;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_DOC")
                        {
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 250;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultEntry")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 250;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultApproval")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 250;
                            }
                        }
                        else if (View.Id == "SysSampleCodeval")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 200;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultView")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 250;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 250;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultEntry")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 250;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultValidation")
                        {
                            if (editor.Grid.Columns["SYSSamplecode"] != null)
                            {
                                editor.Grid.Columns["SYSSamplecode"].Width = 220;
                            }
                            if (editor.Grid.Columns["QCBatchID"] != null)
                            {
                                editor.Grid.Columns["QCBatchID"].Width = 150;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_QCResultValidation")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 250;
                            }
                            if (editor.Grid.Columns["QcBatchID"] != null)
                            {
                                editor.Grid.Columns["QcBatchID"].Width = 150;
                            }
                            if (editor.Grid.Columns["Analytical Batch ID"] != null)
                            {
                                editor.Grid.Columns["Analytical Batch ID"].Width = 150;
                            }
                        }
                        if (View.Id == "MaintenanceTaskCheckList_ListView_MaintenanceQueue")
                        {
                            if (editor.Grid.Columns["InstrumentID"] != null)
                            {
                                editor.Grid.Columns["InstrumentID"].Width = 150;
                            }
                            if (editor.Grid.Columns["InstrumentName"] != null)
                            {
                            editor.Grid.Columns["Instrument Name"].Width = 250;
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = 150;
                            }
                            if (editor.Grid.Columns["TaskToBeDone"] != null)
                            {
                                editor.Grid.Columns["TaskToBeDone"].Width = 150;
                            }
                            if (editor.Grid.Columns["ActionDescription"] != null)
                            {
                                editor.Grid.Columns["ActionDescription"].Width = 150;
                            }
                            if (editor.Grid.Columns["Attachment"] != null)
                            {
                                editor.Grid.Columns["Attachment"].Width = 150;
                            }
                            if (editor.Grid.Columns["MaintainedBy"] != null)
                            {
                                editor.Grid.Columns["MaintainedBy"].Width = 150;
                            }
                            if (editor.Grid.Columns["MaintainedDate"] != null)
                            {
                                editor.Grid.Columns["MaintainedDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["NextMaintainDate"] != null)
                            {
                            editor.Grid.Columns["NextMaintainDate"].Width = 250;
                            }
                            if (editor.Grid.Columns["Skip"] != null)
                            {
                                editor.Grid.Columns["Skip"].Width = 80;
                            }
                            if (editor.Grid.Columns["DateToMaintain"] != null)
                            {
                            editor.Grid.Columns["DateToMaintain"].Width = 250;
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = Unit.Percentage(100);
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_QCResultApproval")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 250;
                            }
                            if (editor.Grid.Columns["QCBatchID"] != null)
                            {
                                editor.Grid.Columns["QCBatchID"].Width = 150;
                            }
                            if (editor.Grid.Columns["Analytical Batch ID"] != null)
                            {
                                editor.Grid.Columns["Analytical Batch ID"].Width = 150;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview")
                        {
                            if (editor.Grid.Columns["SysSampleCode"] != null)
                            {
                                editor.Grid.Columns["SysSampleCode"].Width = 250;
                            }
                        }
                        else if (View.Id == "ReportPackage_ListView_New")
                        {
                            if (editor.Grid.Columns["TemplateName"] != null)
                            {
                                editor.Grid.Columns["TemplateName"].Width = 170;
                            }
                            if (editor.Grid.Columns["TemplateID"] != null)
                            {
                                editor.Grid.Columns["TemplateID"].CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                            }
                            if (editor.Grid.Columns["Sort"] != null)
                            {
                                editor.Grid.Columns["Sort"].CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                            }
                        }
                        else if (View.Id == "Testparameter_ListView_Test_SampleParameter")
                        {
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 250;
                            }
                            if (editor.Grid.Columns["FinalDefaultResult"] != null)
                            {
                                editor.Grid.Columns["FinalDefaultResult"].Width = 150;
                            }
                            if (editor.Grid.Columns["FinalDefaultUnits"] != null)
                            {
                                editor.Grid.Columns["FinalDefaultUnits"].Width = 150;
                            }
                            if (editor.Grid.Columns["RegulatoryLimit"] != null)
                            {
                                editor.Grid.Columns["RegulatoryLimit"].Width = 150;
                            }
                        }
                        else if (View.Id == "Testparameter_ListView_Test_Surrogates")
                        {
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 250;
                            }
                            if (editor.Grid.Columns["SpikeAmount"] != null)
                            {
                                editor.Grid.Columns["SpikeAmount"].Width = 100;
                            }
                            if (editor.Grid.Columns["SpikeAmountUnit"] != null)
                            {
                                editor.Grid.Columns["SpikeAmountUnit"].Width = 150;
                            }
                            if (editor.Grid.Columns["InternalStandard"] != null)
                            {
                                editor.Grid.Columns["InternalStandard"].Width = 150;
                            }
                            if (editor.Grid.Columns["SurrogateAmount"] != null)
                            {
                                editor.Grid.Columns["SurrogateAmount"].Width = 150;
                            }
                            if (editor.Grid.Columns["SurrogateLowLimit"] != null)
                            {
                                editor.Grid.Columns["SurrogateLowLimit"].Width = 150;
                            }
                            if (editor.Grid.Columns["SurrogateHighLimit"] != null)
                            {
                                editor.Grid.Columns["SurrogateHighLimit"].Width = 150;
                            }
                            if (editor.Grid.Columns["RecLCLimit"] != null)
                            {
                                editor.Grid.Columns["RecLCLimit"].Width = 100;
                            }
                            if (editor.Grid.Columns["LowCLimit"] != null)
                            {
                                editor.Grid.Columns["LowCLimit"].Width = 100;
                            }
                            if (editor.Grid.Columns["HighCLimit"] != null)
                            {
                                editor.Grid.Columns["HighCLimit"].Width = 100;
                            }
                            if (editor.Grid.Columns["RELCLimit"] != null)
                            {
                                editor.Grid.Columns["RELCLimit"].Width = 100;
                            }
                            if (editor.Grid.Columns["REHCLimit"] != null)
                            {
                                editor.Grid.Columns["REHCLimit"].Width = 100;
                            }
                            if (editor.Grid.Columns["SigFig"] != null)
                            {
                                editor.Grid.Columns["SigFig"].Width = 100;
                            }
                            if (editor.Grid.Columns["CutOff"] != null)
                            {
                                editor.Grid.Columns["CutOff"].Width = 100;
                            }
                            if (editor.Grid.Columns["Decimal"] != null)
                            {
                                editor.Grid.Columns["Decimal"].Width = 100;
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = 100;
                            }
                            if (editor.Grid.Columns["RegulatoryLimit"] != null)
                            {
                                editor.Grid.Columns["RegulatoryLimit"].Width = 150;
                            }
                        }
                        else if (View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault")
                        {
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 250;
                            }
                            if (editor.Grid.Columns["SpikeAmountUnits"] != null)
                            {
                                editor.Grid.Columns["SpikeAmountUnits"].Width = 150;
                            }
                            if (editor.Grid.Columns["RegulatoryLimit"] != null)
                            {
                                editor.Grid.Columns["RegulatoryLimit"].Width = 150;
                            }
                        }
                        else if (View.Id == "SamplePrepBatchSequence_ListView")
                        {

                            if (editor.Grid.Columns["SampleID.SampleAmount"] != null)
                            {
                                editor.Grid.Columns["SampleID.SampleAmount"].Width = 120;
                            }
                            if (editor.Grid.Columns["TakenSampleUnit"] != null)
                            {
                                editor.Grid.Columns["TakenSampleUnit"].Width = 150;
                            }
                        }
                        else if (View.Id == "Labware_TestMethods_ListView")
                        {
                            if (editor.Grid.Columns["MatrixName.MatrixName"] != null)
                            {
                                editor.Grid.Columns["MatrixName.MatrixName"].Width = 150;
                            }
                            if (editor.Grid.Columns["ResultValidationUsers"] != null)
                            {
                                editor.Grid.Columns["ResultValidationUsers"].Width = 180;
                            }
                            if (editor.Grid.Columns["ResultApprovalUsers"] != null)
                            {
                                editor.Grid.Columns["ResultApprovalUsers"].Width = 180;
                            }
                            if (editor.Grid.Columns["ResultEntryUsers"] != null)
                            {
                                editor.Grid.Columns["ResultEntryUsers"].Width = 150;
                            }

                        }
                        else if (View.Id == "CustomReportBuilder_ListView")
                        {
                            if (editor.Grid.Columns["btnReportDesigner"] != null)
                            {
                                editor.Grid.Columns["btnReportDesigner"].Width = 100;
                                editor.Grid.Columns["btnReportDesigner"].Index = 8;
                                editor.Grid.Columns["btnReportDesigner"].Caption = "ReportDesigner";
                            }
                            if (editor.Grid.Columns["TemplateID"] != null)
                            {
                                editor.Grid.Columns["TemplateID"].Width = 100;
                            }
                            if (editor.Grid.Columns["ReportDesignerName"] != null)
                            {
                                editor.Grid.Columns["ReportDesignerName"].Width = 130;
                            }
                            if (editor.Grid.Columns["CustomCaption"] != null)
                            {
                                editor.Grid.Columns["CustomCaption"].Width = 130;
                            }
                            if (editor.Grid.Columns["Active"] != null)
                            {
                                editor.Grid.Columns["Active"].Width = 80;
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = 100;
                            }
                            if (editor.Grid.Columns["ReportType"] != null)
                            {
                                editor.Grid.Columns["ReportType"].Width = 100;
                            }
                            if (editor.Grid.Columns["UserAccess"] != null)
                            {
                                editor.Grid.Columns["UserAccess"].Width = 100;
                            }
                            if (editor.Grid.Columns["Module"] != null)
                            {
                                editor.Grid.Columns["Module"].Width = 130;
                            }
                            if (editor.Grid.Columns["BusinessObject"] != null)
                            {
                                editor.Grid.Columns["BusinessObject"].Width = 150;
                            }
                            if (editor.Grid.Columns["ShowReportID"] != null)
                            {
                                editor.Grid.Columns["ShowReportID"].Width = 110;
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = 150;
                            }
                            editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                        }
                        else if (View.Id == "Labware_TestItems_ListView")
                        {
                            if (editor.Grid.Columns["TestItemName"] != null)
                            {
                                editor.Grid.Columns["TestItemName"].Width = 150;
                            }
                            if (editor.Grid.Columns["MajorCategory"] != null)
                            {
                                editor.Grid.Columns["MajorCategory"].Width = 150;
                            }
                            if (editor.Grid.Columns["SampleCategory"] != null)
                            {
                                editor.Grid.Columns["SampleCategory"].Width = 150;
                            }
                            if (editor.Grid.Columns["MeasuringRange"] != null)
                            {
                                editor.Grid.Columns["MeasuringRange"].Width = 150;
                            }
                            if (editor.Grid.Columns["StandardCertificate"] != null)
                            {
                                editor.Grid.Columns["StandardCertificate"].Width = 150;
                            }

                        }
                        else if (View.Id == "Parameter_ListView")
                        {
                            if (editor.Grid.Columns["ParameterName"] != null && Convert.ToInt32(strscreenwidth) < 1500)
                            {
                                editor.Grid.Columns["ParameterName"].Width = 250;
                            }
                            else if (editor.Grid.Columns["ParameterName"] != null && Convert.ToInt32(strscreenwidth) >= 1500)
                            {
                                editor.Grid.Columns["ParameterName"].Width = 300;
                            }

                            if (editor.Grid.Columns["Synonym"] != null && Convert.ToInt32(strscreenwidth) < 1500)
                            {
                                editor.Grid.Columns["Synonym"].Width = 250;
                            }
                            else if (editor.Grid.Columns["Synonym"] != null && Convert.ToInt32(strscreenwidth) >= 1500)
                            {
                                editor.Grid.Columns["Synonym"].Width = 300;
                            }
                            if (editor.Grid.Columns["Formula"] != null)
                            {
                                editor.Grid.Columns["Formula"].Width = 125;
                            }
                            if (editor.Grid.Columns["CAS"] != null)
                            {
                                editor.Grid.Columns["CAS"].Width = 125;
                            }
                            if (editor.Grid.Columns["MW"] != null)
                            {
                                editor.Grid.Columns["MW"].Width = 125;
                            }
                            if (editor.Grid.Columns["ParameterCode"] != null)
                            {
                                editor.Grid.Columns["ParameterCode"].Width = 125;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["InlineEditCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["InlineEditCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["ParameterName"] != null)
                            {
                                gridView.VisibleColumns["ParameterName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (View.Id == "Items_ListView") /*|| (View.Id == "Vendors_ListView"*//*/* && Convert.ToInt32(strscreenwidth) < 200)*///* || (View.Id == "Items_ListView_Copy_StockWatch" && Convert.ToInt32(strscreenwidth) < 1700)* /*||*//* (View.Id == "Items_ListView_Copy_StockAlert" && Convert.ToInt32(strscreenwidth) < 1700))*/
                        {
                            //gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                            //foreach (WebColumnBase column in gridView.Columns)
                            //{
                            //    IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)editor).GetColumnInfo(column);
                            //    if (columnInfo != null)
                            //    {
                            //        IModelColumn modelColumn = (IModelColumn)columnInfo.Model;
                            //        column.Width = Unit.Pixel(modelColumn.Width);
                            //    }


                            //}
                            if (View.Id == "Items_ListView")
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                //gridView.VisibleColumns["InlineEditCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                if (editor.Grid.Columns["Edit"] != null)
                                {
                                    gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                //if (Convert.ToInt32(strscreenwidth) < 1380)
                                {
                                    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                                }
                                if (gridView.VisibleColumns["ItemName"] != null)
                                {
                                    gridView.VisibleColumns["ItemName"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                if (editor.Grid.Columns["ItemName"] != null)
                                {
                                    editor.Grid.Columns["ItemName"].Width = 250;
                                }
                                if (editor.Grid.Columns["ItemCode"] != null)
                                {
                                    editor.Grid.Columns["ItemCode"].Width = 120;
                                }
                                if (editor.Grid.Columns["Description"] != null)
                                {
                                    editor.Grid.Columns["Description"].Width = 250;
                                }
                                editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";

                            }

                            if (View.Id == "Distribution_ListView_Disposal")
                            {

                                if (editor.Grid.Columns["DisposedDate"] != null)
                                {
                                    editor.Grid.Columns["DisposedDate"].Width = 170;
                                }
                                if (editor.Grid.Columns["ReceiveDate"] != null)
                                {
                                    editor.Grid.Columns["ReceiveDate"].Width = 170;
                                }
                                if (editor.Grid.Columns["DisposedBy"] != null)
                                {
                                    editor.Grid.Columns["DisposedBy"].Width = 170;
                                }
                            }
                            if (View.Id == "Vendors_ListView")
                            {

                                if (Convert.ToInt32(strscreenwidth) < 1372)
                                {
                                    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                                }

                                //if (editor.Grid.Columns["Vendor"] != null)
                                //{
                                //    editor.Grid.Columns["Vendor"].Width = 130;
                                //}
                                //if (editor.Grid.Columns["Address1"] != null)
                                //{
                                //    editor.Grid.Columns["Address1"].Width = 130;
                                //}
                                //if (editor.Grid.Columns["City"] != null)
                                //{
                                //    editor.Grid.Columns["City"].Width = 130;
                                //}
                                //if (editor.Grid.Columns["State"] != null)
                                //{
                                //    editor.Grid.Columns["State"].Width = 130;
                                //}
                                //if (editor.Grid.Columns["ZipCode"] != null)
                                //{
                                //    editor.Grid.Columns["ZipCode"].Width = 130;
                                //}
                                //if (editor.Grid.Columns["Country"] != null)
                                //{
                                //    editor.Grid.Columns["Country"].Width = 130;
                                //}
                                //if (editor.Grid.Columns["Phone"] != null)
                                //{
                                //    editor.Grid.Columns["Phone"].Width = 130;
                                //}
                                //if (editor.Grid.Columns["Account"] != null)
                                //{
                                //    editor.Grid.Columns["Account"].Width = 130;
                                //}

                            }
                        }
                        else if (View.Id == "SamplingProposal_ListView")
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = Unit.Percentage(20);
                            }
                        }
                        else if (View.Id == "NonConformityInitiation_ListView" || View.Id == "NonConformityInitiation_ListView_PendingVerification"
                            || View.Id == "CompliantInitiation_ListView" || View.Id == "CompliantInitiation_ListView_Verification" || View.Id == "CompliantInitiation_ListView_DataCenter" || View.Id == "NonConformityInitiation_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["NCAID"] != null)
                            {
                                editor.Grid.Columns["NCAID"].Width = Unit.Percentage(8);
                            }
                            if (editor.Grid.Columns["CCID"] != null)
                            {
                                editor.Grid.Columns["CCID"].Width = Unit.Percentage(8);
                            }
                            if (editor.Grid.Columns["DateInitiated"] != null)
                            {
                                editor.Grid.Columns["DateInitiated"].Width = Unit.Percentage(11);
                            }
                            if (editor.Grid.Columns["Specifics"] != null)
                            {
                                editor.Grid.Columns["Specifics"].Width = Unit.Percentage(24);
                            }
                            if (editor.Grid.Columns["Subject"] != null)
                            {
                                editor.Grid.Columns["Subject"].Width = Unit.Percentage(12);
                            }
                            if (editor.Grid.Columns["Department"] != null)
                            {
                                editor.Grid.Columns["Department"].Width = Unit.Percentage(11);
                            }
                            if (editor.Grid.Columns["AssignedTo"] != null)
                            {
                                editor.Grid.Columns["AssignedTo"].Width = Unit.Percentage(10);
                            }
                            if (editor.Grid.Columns["InitiatedBy"] != null)
                            {
                                editor.Grid.Columns["InitiatedBy"].Width = Unit.Percentage(10);
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = Unit.Percentage(15);
                            }
                        }
                        if (View.Id == "NonConformityInitiation_CorrectiveActionLogs_ListView" || View.Id == "NonConformityInitiation_CorrectiveActionLogs_ListView_PendingVerification" ||
                            View.Id == "CompliantInitiation_CorrectiveActionLogs_ListView" || View.Id == "CompliantInitiation_CorrectiveActionLogs_ListView_PendingVerification")
                        {
                            if (editor.Grid.Columns["Date"] != null)
                            {
                                editor.Grid.Columns["Date"].Width = Unit.Percentage(15);
                            }
                            if (editor.Grid.Columns["Employee"] != null)
                            {
                                editor.Grid.Columns["Employee"].Width = Unit.Percentage(13);
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = Unit.Percentage(22);
                            }
                            if (editor.Grid.Columns["CorrectiveAction"] != null)
                            {
                                editor.Grid.Columns["CorrectiveAction"].Width = Unit.Percentage(35);
                            }
                            if (editor.Grid.Columns["Attachment"] != null)
                            {
                                editor.Grid.Columns["Attachment"].Width = Unit.Percentage(15);
                            }
                        }
                        else if (View.ObjectTypeInfo.Type == typeof(Requisition) || View.ObjectTypeInfo.Type == typeof(Distribution) || View.ObjectTypeInfo.Type == typeof(Purchaseorder) || View.Id == "VendorReagentCertificate_ListView")
                        {
                            if (editor.Grid.Columns["Item.items"] != null)
                            {
                                editor.Grid.Columns["Item.items"].Width = 230;
                            }
                            if (editor.Grid.Columns["Item.Specification"] != null)
                            {
                                editor.Grid.Columns["Item.Specification"].Width = 230;
                            }
                            if (editor.Grid.Columns["Delivery Priority"] != null)
                            {
                                editor.Grid.Columns["Delivery Priority"].Width = 150;
                            }
                            if (View.ObjectTypeInfo.Type == typeof(Requisition) && editor.Grid.Columns["Vendor"] != null && View.Id != "Requisition_ListView_Purchaseorder_Mainview")
                            {
                                editor.Grid.Columns["Vendor"].Width = 150;

                            }
                            if (View.ObjectTypeInfo.Type == typeof(Requisition) && editor.Grid.Columns["Errorlog"] != null)
                            {
                                editor.Grid.Columns["Errorlog"].Width = 250;
                            }
                            if (View.ObjectTypeInfo.Type == typeof(Requisition) && editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = 250;
                            }
                        }
                        else if (View.Id == "SampleLogIn_ListView_SampleDisposition" || View.Id == "SampleLogIn_ListView_SampleDisposition_DisposedSamples") /*|| View.Id == "SampleLogIn_ListView_Copy_SampleRegistration"*/
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 250;
                            }
                        }
                        else if (View.Id == "Reporting_ListView_Level1Review_View")
                        {
                            gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                            gridView.Settings.VerticalScrollableHeight = 600;
                            if (editor.Grid.Columns["Project"] != null)
                            {
                                editor.Grid.Columns["Project"].Width = 200;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 250;
                            }
                        }



                        else if (View.Id == "SampleParameter_ListView_Copy_QCResultApproval")
                        {
                            editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;

                        }
                        else if (View.Id == "Reporting_ListView_Level2Review_View")
                        {
                            gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                            gridView.Settings.VerticalScrollableHeight = 600;
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 250;
                            }

                        }
                        else if (View.Id == "Customer_ListView")
                        {
                            if (editor.Grid.Columns["CustomerName"] != null)
                            {
                                editor.Grid.Columns["CustomerName"].Width = 200;
                            }
                            if (editor.Grid.Columns["Address"] != null)
                            {
                                editor.Grid.Columns["Address"].Width = 200;
                            }
                            if (editor.Grid.Columns["Address1"] != null)
                            {
                                editor.Grid.Columns["Address1"].Width = 180;
                            }
                            if (editor.Grid.Columns["City"] != null)
                            {
                                editor.Grid.Columns["City"].Width = 100;
                            }
                            if (editor.Grid.Columns["State"] != null)
                            {
                                editor.Grid.Columns["State"].Width = 100;
                            }
                            if (editor.Grid.Columns["Zip"] != null)
                            {
                                editor.Grid.Columns["Zip"].Width = 100;
                            }
                            if (editor.Grid.Columns["Account"] != null)
                            {
                                editor.Grid.Columns["Account"].Width = 100;
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = 120;
                            }
                            if (editor.Grid.Columns["OfficePhone"] != null)
                            {
                                editor.Grid.Columns["OfficePhone"].Width = 100;
                            }
                            if (editor.Grid.Columns["Fax"] != null)
                            {
                                editor.Grid.Columns["Fax"].Width = 150;
                            }
                            if (editor.Grid.Columns["WebSite"] != null)
                            {
                                editor.Grid.Columns["WebSite"].Width = 150;
                            }
                            if (editor.Grid.Columns["LastContactDate"] != null)
                            {
                                editor.Grid.Columns["LastContactDate"].Width = 180;
                            }
                            if (editor.Grid.Columns["PreferredContactMethod"] != null)
                            {
                                editor.Grid.Columns["PreferredContactMethod"].Width = 180;
                            }
                            if (editor.Grid.Columns["DisplayName"] != null)
                            {
                                editor.Grid.Columns["DisplayName"].Width = 200;
                            }

                        }
                        else if (View.Id == "Customer_Contacts_ListView")
                        {
                            editor.Grid.Settings.VerticalScrollableHeight = 150;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else if (View.Id == "Customer_Projects_ListView")
                        {
                            editor.Grid.Settings.VerticalScrollableHeight = 150;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else if (View.Id == "Customer_Collectors_ListView")
                        {
                            editor.Grid.Settings.VerticalScrollableHeight = 150;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else if (View.Id == "Customer_Note_ListView")
                        {
                            editor.Grid.Settings.VerticalScrollableHeight = 150;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else if (View.Id == "Customer_InvoicingAddress_ListView")
                        {
                            editor.Grid.Settings.VerticalScrollableHeight = 150;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else if (View.Id == "Customer_InvoicingContact_ListView")
                        {
                            editor.Grid.Settings.VerticalScrollableHeight = 150;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else if (View.Id == "Customer_ReportingContact_ListView")
                        {
                            editor.Grid.Settings.VerticalScrollableHeight = 150;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else if (View.Id == "Customer_SampleSites_ListView")
                        {
                            editor.Grid.Settings.VerticalScrollableHeight = 150;
                            editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        }
                        else if (View.Id == "TestMethod_ListView" || View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Delivery")
                        {
                            if (View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_PrintDownload" || View.Id == "Reporting_ListView_Delivery" || View.Id == "Reporting_ListView_Copy_ReportApproval")
                            {
                                if (editor.Grid.Columns["ReportID"] != null)
                                {
                                    editor.Grid.Columns["ReportID"].Width = 100;

                                }
                                if (editor.Grid.Columns["ReportName"] != null)
                                {
                                    editor.Grid.Columns["ReportName"].Width = 200;
                                }
                                if (editor.Grid.Columns["JobID"] != null)
                                {
                                    editor.Grid.Columns["JobID"].Width = 100;
                                }
                                if (editor.Grid.Columns["Client"] != null)
                                {
                                    editor.Grid.Columns["Client"].Width = 250;
                                }
                                if (editor.Grid.Columns["ProjectID"] != null)
                                {
                                    editor.Grid.Columns["ProjectID"].Width = 100;
                                }
                                if (editor.Grid.Columns["Project"] != null)
                                {
                                    editor.Grid.Columns["Project"].Width = 200;
                                }
                                if (editor.Grid.Columns["CompanyID"] != null)
                                {
                                    editor.Grid.Columns["CompanyID"].Width = 200;
                                }
                                if (editor.Grid.Columns["ReportedDate"] != null)
                                {
                                    editor.Grid.Columns["ReportedDate"].Width = 150;
                                }
                                if (editor.Grid.Columns["ReportedBy"] != null)
                                {
                                    editor.Grid.Columns["ReportedBy"].Width = 100;
                                }
                                if (editor.Grid.Columns["ReportedBy"] != null)
                                {
                                    editor.Grid.Columns["ReportedBy"].Width = 150;
                                }
                                if (editor.Grid.Columns["ReportValidatedDate"] != null)
                                {
                                    editor.Grid.Columns["ReportValidatedDate"].Width = 150;
                                }
                                if (editor.Grid.Columns["ReportValidatedBy"] != null)
                                {
                                    editor.Grid.Columns["ReportValidatedBy"].Width = 200;

                                }
                                if (editor.Grid.Columns["ReportStatus"] != null)
                                {
                                    editor.Grid.Columns["ReportStatus"].Width = 200;
                                }
                                if (editor.Grid.Columns["Email"] != null)
                                {
                                    editor.Grid.Columns["Email"].Width = 200;
                                }
                                if (gridView.VisibleColumns["SelectionCommandColumn"] != null)
                                {
                                    gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                if (gridView.VisibleColumns["ReportID"] != null)
                                {
                                    gridView.VisibleColumns["ReportID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                if (gridView.VisibleColumns["MailContent"] != null)
                                {
                                    gridView.VisibleColumns["MailContent"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                if (gridView.VisibleColumns["Edit"] != null)
                                {
                                    gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                                if (gridView.VisibleColumns["ReportDeliveryPreview"] != null)
                                {
                                    gridView.VisibleColumns["ReportDeliveryPreview"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                            }

                        }
                        else if (View.Id == "SampleConditionCheckPoint_ListView")
                        {
                            if (editor.Grid.Columns["CheckPoint"] != null)
                            {
                                editor.Grid.Columns["CheckPoint"].Width = 500;
                            }
                            if (editor.Grid.Columns["Yes"] != null)
                            {
                                editor.Grid.Columns["Yes"].Width = 100;
                            }
                            if (editor.Grid.Columns["No"] != null)
                            {
                                editor.Grid.Columns["No"].Width = 100;
                            }
                            if (editor.Grid.Columns["NA"] != null)
                            {
                                editor.Grid.Columns["NA"].Width = 100;
                            }
                            if (editor.Grid.Columns["Initial"] != null)
                            {
                                editor.Grid.Columns["Initial"].Width = 180;
                            }
                            if (editor.Grid.Columns["DateTime"] != null)
                            {
                                editor.Grid.Columns["DateTime"].Width = 120;
                            }
                        }
                        if (View.Id == "Requisition_ListViewEntermode" || View.Id == "Requisition_ListView_Review")
                        {

                            //if (Convert.ToInt32(strscreenwidth) < 1372)
                            //{
                            //    gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                            //}
                            if (editor.Grid.Columns["ItemName"] != null)
                            {
                                editor.Grid.Columns["ItemName"].Width = 250;
                            }
                            if (editor.Grid.Columns["ItemCode"] != null)
                            {
                                editor.Grid.Columns["ItemCode"].Width = 120;
                            }
                            if (editor.Grid.Columns["Item.ItemDescription"] != null)
                            {
                                editor.Grid.Columns["Item.ItemDescription"].Width = 250;
                            }
                            //if (editor.Grid.Columns["ContainerSize"] != null)
                            //{
                            //    editor.Grid.Columns["ContainerSize"].Width =100;
                            //}
                            if (editor.Grid.Columns["Catalog#"] != null)
                            {
                                editor.Grid.Columns["Catalog#"].Width = 100;
                            }
                            if (editor.Grid.Columns["Vendor"] != null)
                            {
                                editor.Grid.Columns["Vendor"].Width = 120;
                            }
                            //if (editor.Grid.Columns["PackUnits"] != null)
                            //{
                            //    editor.Grid.Columns["PackUnits"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["Grade"] != null)
                            //{
                            //    editor.Grid.Columns["Grade"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["StockQty"] != null)
                            //{
                            //    editor.Grid.Columns["StockQty"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["OrderQty"] != null)
                            //{
                            //    editor.Grid.Columns["OrderQty"].Width = 100;
                            //}
                            if (editor.Grid.Columns["DateReviewed"] != null)
                            {
                                editor.Grid.Columns["DateReviewed"].Width = 120;
                            }
                            if (editor.Grid.Columns["DeliveryPriority"] != null)
                            {
                                editor.Grid.Columns["DeliveryPriority"].Width = 120;
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = 120;
                            }
                            if (editor.Grid.Columns["Errorlog"] != null)
                            {
                                editor.Grid.Columns["Errorlog"].Width = 120;
                            }

                        }


                        else if (View.Id == "Requisition_ListView")
                        {
                            if (editor.Grid.Columns["Catlog#"] != null)
                            {
                                editor.Grid.Columns["Catlog#"].Width = 120;
                            }
                            if (editor.Grid.Columns["OrderQty"] != null)
                            {
                                editor.Grid.Columns["OrderQty"].Width = 120;
                            }
                            if (editor.Grid.Columns["Department"] != null)
                            {
                                editor.Grid.Columns["Department"].Width = 120;
                            }
                            if (editor.Grid.Columns["PackUnits"] != null)
                            {
                                editor.Grid.Columns["PackUnits"].Width = 120;
                            }
                        }


                        else if (View.Id == "SampleConditionCheckPoint_ListView_Task")
                        {
                            if (editor.Grid.Columns["CheckPoint"] != null)
                            {
                                editor.Grid.Columns["CheckPoint"].Width = 500;
                            }
                            if (editor.Grid.Columns["Yes"] != null)
                            {
                                editor.Grid.Columns["Yes"].Width = 100;
                            }
                            if (editor.Grid.Columns["No"] != null)
                            {
                                editor.Grid.Columns["No"].Width = 100;
                            }
                            if (editor.Grid.Columns["NA"] != null)
                            {
                                editor.Grid.Columns["NA"].Width = 100;
                            }
                            if (editor.Grid.Columns["Initial"] != null)
                            {
                                editor.Grid.Columns["Initial"].Width = 180;
                            }
                            if (editor.Grid.Columns["DateTime"] != null)
                            {
                                editor.Grid.Columns["DateTime"].Width = 120;
                            }
                        }
                        else if (View.Id == "SampleConditionCheckData_ListView")
                        {
                            if (editor.Grid.Columns["CheckPoint"] != null)
                            {
                                editor.Grid.Columns["CheckPoint"].Width = 500;
                            }
                            if (editor.Grid.Columns["SampleMatrices"] != null)
                            {
                                editor.Grid.Columns["SampleMatrices"].Width = 350;
                            }
                            if (editor.Grid.Columns["PickAnswer"] != null)
                            {
                                editor.Grid.Columns["PickAnswer"].Width = 250;
                            }
                            if (editor.Grid.Columns["Sort"] != null)
                            {
                                editor.Grid.Columns["Sort"].Width = 250;
                            }
                        }
                        else if (View.Id == "VisualMatrix_SetupFields_ListView")
                        {
                            if (editor.Grid.Columns["Freeze"] != null)
                            {
                                editor.Grid.Columns["Freeze"].Width = 100;
                            }
                            if (editor.Grid.Columns["Width"] != null)
                            {
                                editor.Grid.Columns["Width"].Width = 100;
                            }
                            if (editor.Grid.Columns["SortOrder"] != null)
                            {
                                editor.Grid.Columns["SortOrder"].Width = 100;
                            }
                        }
                        else if (View.Id == "Contact_ListView")
                        {
                            if (editor.Grid.Columns["FullName"] != null)
                            {
                                editor.Grid.Columns["FullName"].Width = 200;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 200;
                            }
                            if (editor.Grid.Columns["Prospect Client"] != null)
                            {
                                editor.Grid.Columns["Prospect Client"].Width = 180;
                            }
                            if (editor.Grid.Columns["OfficePhone"] != null)
                            {
                                editor.Grid.Columns["OfficePhone"].Width = 150;
                            }
                            if (editor.Grid.Columns["MobilePhone"] != null)
                            {
                                editor.Grid.Columns["MobilePhone"].Width = 150;
                            }
                            if (editor.Grid.Columns["OtherPhone"] != null)
                            {
                                editor.Grid.Columns["OtherPhone"].Width = 150;
                            }
                            if (editor.Grid.Columns["Email"] != null)
                            {
                                editor.Grid.Columns["Email"].Width = 100;
                            }
                            if (editor.Grid.Columns["Country"] != null)
                            {
                                editor.Grid.Columns["Country"].Width = 100;
                            }
                            if (editor.Grid.Columns["City"] != null)
                            {
                                editor.Grid.Columns["City"].Width = 100;
                            }
                            if (editor.Grid.Columns["State"] != null)
                            {
                                editor.Grid.Columns["State"].Width = 80;
                            }
                            if (editor.Grid.Columns["Zip"] != null)
                            {
                                editor.Grid.Columns["Zip"].Width = 80;
                            }
                        }
                        //else if (View.Id == "TestMethod_ListView_AnalysisDepartmentChain")
                        //{
                        //    if (editor.Grid.Columns["ResultEntryUsers"] != null)
                        //    {
                        //        editor.Grid.Columns["ResultEntryUsers"].Width = 180;
                        //    }
                        //    if (editor.Grid.Columns["ResultValidationUsers"] != null)
                        //    {
                        //        editor.Grid.Columns["ResultValidationUsers"].Width = 180;
                        //    }
                        //    if (editor.Grid.Columns["ResultApprovalUsers"] != null)
                        //    {
                        //        editor.Grid.Columns["ResultApprovalUsers"].Width = 180;
                        //    }
                        //}
                        else if (View.ObjectTypeInfo.Type == ((typeof(SampleParameter))))
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 200;
                            }
                            if (editor.Grid.Columns["ClientName"] != null)
                            {
                                editor.Grid.Columns["ClientName"].Width = 200;
                            }
                        }
                        if (View.Id == "TestMethod_Sampleqctype_ListView" || View.Id == "TestMethod_Initialqctype_ListView" || View.Id == "TestMethod_Closingqctype_ListView" || View.Id == "Reporting_ListView_Deliveired")
                        {
                            if (editor.Grid.Columns["InlineEditCommandColumn"] != null)
                            {
                                editor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                            }
                            if (View.Id == "Reporting_ListView_Deliveired")
                            {
                                if (editor.Grid.Columns["Email"] != null)
                                {
                                    editor.Grid.Columns["Email"].Width = 200;
                                }
                            }
                        }
                        else if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin_Copy")
                        {
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].Width = 75;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 115;
                            }
                            if (editor.Grid.Columns["IsGroup"] != null)
                            {
                                editor.Grid.Columns["IsGroup"].Width = 70;
                            }
                            if (editor.Grid.Columns["SubOut"] != null)
                            {
                                editor.Grid.Columns["SubOut"].Width = 70;
                            }
                            if (editor.Grid.Columns["Component"] != null)
                            {
                                editor.Grid.Columns["Component"].Width = 106;
                            }
                            if (editor.Grid.Columns["Method"].Width != null)
                            {
                                editor.Grid.Columns["Method"].Width = 79;
                            }
                            if (editor.Grid.Columns["IsSubutAttached"] != null)
                            {
                                editor.Grid.Columns["IsSubutAttached"].Width = 0;
                            }
                        }
                        else if (View.Id == "Testparameter_LookupListView_Sampling_SeletectedTest")
                        {
                            if (editor.Grid.Columns["IsSubutAttached"].Width != null)
                            {
                                editor.Grid.Columns["IsSubutAttached"].Width = 0;
                            }
                        }
                        else if (View.Id == "SampleBottleAllocation_ListView_Sampleregistration")
                        {
                            if (editor.Grid.Columns["StorageCondition"] != null)
                            {
                                editor.Grid.Columns["StorageCondition"].Width = 120;
                            }
                        }
                        else if (View.Id == "Messages_ListView")
                        {
                            if (editor.Grid.Columns["MessageKey"] != null)
                            {
                                editor.Grid.Columns["MessageKey"].Width = 100;
                            }
                        }
                        else if (View.Id == "TableFields_ListView")
                        {
                            if (editor.Grid.Columns["FieldName"] != null)
                            {
                                editor.Grid.Columns["FieldName"].Width = 170;
                            }
                            if (editor.Grid.Columns["CaptionEN"] != null)
                            {
                                editor.Grid.Columns["CaptionEN"].Width = 120;
                            }
                            if (editor.Grid.Columns["CaptionCN"] != null)
                            {
                                editor.Grid.Columns["CaptionCN"].Width = 120;
                            }
                            if (editor.Grid.Columns["Definition"] != null)
                            {
                                editor.Grid.Columns["Definition"].Width = 370;
                            }
                            if (editor.Grid.Columns["Type"] != null)
                            {
                                editor.Grid.Columns["Type"].Width = 95;
                            }
                            if (editor.Grid.Columns["Width"] != null)
                            {
                                editor.Grid.Columns["Width"].Width = 95;
                            }
                        }
                        if (View.Id == "Tasks_ListView_Copy_TaskAcceptance" || View.Id == "Tasks_ListView_Copy_TaskAcceptance_History")
                        {
                            if (editor.Grid.Columns["ProjectName"] != null)
                            {
                                editor.Grid.Columns["ProjectName"].Width = 140;
                            }
                        }
                        else if (View.Id == "Tasks_ListView" || View.Id == "Tasks_ListView_History" || View.Id == "Tasks_ListView_Copy_TaskAcceptance" || View.Id == "Tasks_ListView_Copy_TaskAcceptance_History")
                        {
                            if (editor.Grid.Columns["JobID"] != null)
                            {
                                editor.Grid.Columns["JobID"].Width = 120;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 250;
                            }
                            if (editor.Grid.Columns["ProjectID"] != null)
                            {
                                editor.Grid.Columns["ProjectID"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectName"] != null)
                            {
                                editor.Grid.Columns["ProjectName"].Width = 120;
                            }
                            if (editor.Grid.Columns["WhenAct"] != null)
                            {
                                editor.Grid.Columns["WhenAct"].Width = 120;
                            }
                            if (editor.Grid.Columns["TaskRegistrationID"] != null)
                            {
                                editor.Grid.Columns["TaskRegistrationID"].Width = 130;
                            }
                            if (editor.Grid.Columns["RegisteredDate"] != null)
                            {
                                editor.Grid.Columns["RegisteredDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["TaskCategory"] != null)
                            {
                                editor.Grid.Columns["TaskCategory"].Width = 180;
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = 150;
                            }
                            editor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                        }
                        else if (View.Id == "Tasks_ListView_Copy_ProposalValidation")
                        {
                            if (editor.Grid.Columns["TaskRegistrationID"] != null)
                            {
                                editor.Grid.Columns["TaskRegistrationID"].Width = 150;
                            }
                            if (editor.Grid.Columns["RegisteredDate"] != null)
                            {
                                editor.Grid.Columns["RegisteredDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["SampleMatrix"] != null)
                            {
                                editor.Grid.Columns["SampleMatrix"].Width = 120;
                            }
                            if (editor.Grid.Columns["TaskCategory"] != null)
                            {
                                editor.Grid.Columns["TaskCategory"].Width = 120;
                            }
                            if (editor.Grid.Columns["DateCancelled"] != null)
                            {
                                editor.Grid.Columns["DateCancelled"].Width = 120;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 200;
                            }
                            if (editor.Grid.Columns["ProjectID"] != null)
                            {
                                editor.Grid.Columns["ProjectID"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectName"] != null)
                            {
                                editor.Grid.Columns["ProjectName"].Width = 120;
                            }
                            if (editor.Grid.Columns["WhenAct"] != null)
                            {
                                editor.Grid.Columns["WhenAct"].Width = 120;
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = 150;
                            }
                        }
                        else if (View.Id == "Tasks_ListView_PropsalCancellation")
                        {
                            if (editor.Grid.Columns["TaskRegistrationID"] != null)
                            {
                                editor.Grid.Columns["TaskRegistrationID"].Width = 150;
                            }
                            if (editor.Grid.Columns["RegisteredDate"] != null)
                            {
                                editor.Grid.Columns["RegisteredDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["SampleMatrix"] != null)
                            {
                                editor.Grid.Columns["SampleMatrix"].Width = 120;
                            }
                            if (editor.Grid.Columns["TaskCategory"] != null)
                            {
                                editor.Grid.Columns["TaskCategory"].Width = 120;
                            }
                            if (editor.Grid.Columns["DateCancelled"] != null)
                            {
                                editor.Grid.Columns["DateCancelled"].Width = 120;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 200;
                            }
                            if (editor.Grid.Columns["ProjectID"] != null)
                            {
                                editor.Grid.Columns["ProjectID"].Width = 100;
                            }
                            if (editor.Grid.Columns["ProjectName"] != null)
                            {
                                editor.Grid.Columns["ProjectName"].Width = 120;
                            }
                            if (editor.Grid.Columns["ContractTitle"] != null)
                            {
                                editor.Grid.Columns["ContractTitle"].Width = 120;
                            }
                        }
                        else if (View.Id == "Tasks_ListView_SamplingAssignment")
                        {
                            if (editor.Grid.Columns["TaskRegistrationID"] != null)
                            {
                                editor.Grid.Columns["TaskRegistrationID"].Width = 140;
                            }
                            if (editor.Grid.Columns["ProjectCategory"] != null)
                            {
                                editor.Grid.Columns["ProjectCategory"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectLocation"] != null)
                            {
                                editor.Grid.Columns["ProjectLocation"].Width = 120;
                            }
                            if (editor.Grid.Columns["NumberOfSamples"] != null)
                            {
                                editor.Grid.Columns["NumberOfSamples"].Width = 140;
                            }
                        }
                        else if (View.Id == "Tasks_ListView_PrintingSampleLabels")
                        {
                            if (editor.Grid.Columns["ProjectCategory"] != null)
                            {
                                editor.Grid.Columns["ProjectCategory"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectLocation"] != null)
                            {
                                editor.Grid.Columns["ProjectLocation"].Width = 120;
                            }
                            if (editor.Grid.Columns["NumberOfSamples"] != null)
                            {
                                editor.Grid.Columns["NumberOfSamples"].Width = 140;
                            }
                        }
                        else if (View.Id == "Tasks_ListView_FieldDataEntry" || View.Id == "Tasks_ListView_FieldDataReview1" || View.Id == "Tasks_ListView_FieldDataReview2")
                        {
                            if (editor.Grid.Columns["TaskRegistrationID"] != null)
                            {
                                editor.Grid.Columns["TaskRegistrationID"].Width = 140;
                            }
                            if (editor.Grid.Columns["RegisteredDate"] != null)
                            {
                                editor.Grid.Columns["RegisteredDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectCategory"] != null)
                            {
                                editor.Grid.Columns["ProjectCategory"].Width = 120;
                            }
                            if (editor.Grid.Columns["NumberOfSamples"] != null)
                            {
                                editor.Grid.Columns["NumberOfSamples"].Width = 140;
                            }
                            if (editor.Grid.Columns["SampleCategory"] != null)
                            {
                                editor.Grid.Columns["SampleCategory"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectLocation"] != null)
                            {
                                editor.Grid.Columns["ProjectLocation"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectOverview"] != null)
                            {
                                editor.Grid.Columns["ProjectOverview"].Width = 120;
                            }


                            if (gridView.VisibleColumns["TaskRegistrationID"] != null)
                            {
                                gridView.VisibleColumns["TaskRegistrationID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }


                        }
                        else if (View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                        {
                            if (editor.Grid.Columns["JobID"] != null)
                            {
                                editor.Grid.Columns["JobID"].Width = 120;
                            }
                            if (editor.Grid.Columns["SampleID"] != null)
                            {
                                editor.Grid.Columns["SampleID"].Width = 120;
                            }
                            if (editor.Grid.Columns["SampleName"] != null)
                            {
                                editor.Grid.Columns["SampleName"].Width = 120;
                            }
                            if (editor.Grid.Columns["SampleMatrix"] != null)
                            {
                                editor.Grid.Columns["SampleMatrix"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectID"] != null)
                            {
                                editor.Grid.Columns["ProjectID"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectName"] != null)
                            {
                                editor.Grid.Columns["ProjectName"].Width = 120;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 250;
                            }
                            if (editor.Grid.Columns["StorageID"] != null)
                            {
                                editor.Grid.Columns["StorageID"].Width = 120;
                            }
                            if (editor.Grid.Columns["PreserveCondition"] != null)
                            {
                                editor.Grid.Columns["PreserveCondition"].Width = 150;
                            }
                            if (editor.Grid.Columns["DaysSampleKeeping"] != null)
                            {
                                editor.Grid.Columns["DaysSampleKeeping"].Width = 150;
                            }
                            if (editor.Grid.Columns["DaysRemain"] != null)
                            {
                                editor.Grid.Columns["DaysRemain"].Width = 120;
                            }
                        }
                        else if (View.Id == "Samplecheckin_ListView_Copy_Registration" || View.Id == "Samplecheckin_ListView_Copy_Registration_History" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History" || View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff")
                        {
                            if (editor.Grid.Columns["JobID"] != null)
                            {
                                editor.Grid.Columns["JobID"].Width = Unit.Percentage(12);
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = Unit.Percentage(18);
                            }
                            if (editor.Grid.Columns["ProjectID"] != null)
                            {
                                editor.Grid.Columns["ProjectID"].Width = Unit.Percentage(15);
                            }
                            if (editor.Grid.Columns["ProjectName"] != null)
                            {
                                editor.Grid.Columns["ProjectName"].Width = Unit.Percentage(15);
                            }
                           if (editor.Grid.Columns["RegistrationType"] != null)
                            {
                            editor.Grid.Columns["RegistrationType"].Width = Unit.Percentage(15);
                            }
                        if (editor.Grid.Columns["ReceivedDate"] != null)
                            {
                                editor.Grid.Columns["ReceivedDate"].Width = Unit.Percentage(13);
                            }
                            if (editor.Grid.Columns["DueDate"] != null)
                            {
                                editor.Grid.Columns["DueDate"].Width = Unit.Percentage(13);
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = Unit.Percentage(14);
                            }

                        }
                        else if (View.Id == "Tasks_ListView_SampleTransfer")
                        {
                            if (editor.Grid.Columns["ProjectCategory"] != null)
                            {
                                editor.Grid.Columns["ProjectCategory"].Width = 120;
                            }
                            if (editor.Grid.Columns["ProjectLocation"] != null)
                            {
                                editor.Grid.Columns["ProjectLocation"].Width = 120;
                            }
                            if (editor.Grid.Columns["TextSummary"] != null)
                            {
                                editor.Grid.Columns["TextSummary"].Width = 120;
                            }
                        }
                        else if (View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue")
                        {
                            if (editor.Grid.Columns["SuboutOrderID"] != null)
                            {
                              editor.Grid.Columns["SuboutOrderID"].Width = Unit.Percentage(10);
                        }
                        if (editor.Grid.Columns["TestforSubout"] != null)
                        {
                            editor.Grid.Columns["TestforSubout"].Width = Unit.Percentage(12);
                        }
                        if (editor.Grid.Columns["ContactEmail"] != null)
                        {
                            editor.Grid.Columns["ContactEmail"].Width = Unit.Percentage(15);
                        }
                        if (editor.Grid.Columns["ContractLabName"] != null)
                        {
                            editor.Grid.Columns["ContractLabName"].Width = Unit.Percentage(10);
                        }
                    }

                    else if (View.Id == "SubOutSampleRegistrations_ListView_PendingSignOff" || View.Id == "SubOutSampleRegistrations_ListView_Level2DataReview"
                            || View.Id == "SubOutSampleRegistrations_ListView_Level3DataReview" || View.Id == "SubOutSampleRegistrations_ListView" /*|| View.Id == "SubOutSampleRegistrations_ListView_NotificationQueue"*/)
                        {
                            if (editor.Grid.Columns["SuboutOrderID"] != null)
                            {
                                editor.Grid.Columns["SuboutOrderID"].Width = 130;
                            }
                            if (editor.Grid.Columns["TurnAroundTime"] != null)
                            {
                                editor.Grid.Columns["TurnAroundTime"].Width = 120;
                            }
                            if (editor.Grid.Columns["EDDTemplateAttached"] != null)
                            {
                                editor.Grid.Columns["EDDTemplateAttached"].Width = 140;
                            }
                            if (editor.Grid.Columns["ContactNumber"] != null)
                            {
                                editor.Grid.Columns["ContactNumber"].Width = 120;
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = 200;
                            }
                        }
                        else if (View.Id == "SampleWeighingBatch_ListView")
                        {
                            if (editor.Grid.Columns["WeighingBatchID"] != null)
                            {
                                editor.Grid.Columns["WeighingBatchID"].Width = 120;
                            }
                            if (editor.Grid.Columns["LastDateUpdated"] != null)
                            {
                                editor.Grid.Columns["LastDateUpdated"].Width = 120;
                            }
                            if (editor.Grid.Columns["LastUpdatedBy"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedBy"].Width = 120;
                            }
                        }
                        else if (View.Id == "SampleWeighingBatchSequence_ListView_Tracking")
                        {
                            if (editor.Grid.Columns["RetianedWeight(g)"] != null)
                            {
                                editor.Grid.Columns["RetianedWeight(g)"].Width = 130;
                            }
                            if (editor.Grid.Columns["SampleWeight(g)"] != null)
                            {
                                editor.Grid.Columns["SampleWeight(g)"].Width = 120;
                            }
                            if (editor.Grid.Columns["RemainWeight(g)"] != null)
                            {
                                editor.Grid.Columns["RemainWeight(g)"].Width = 120;
                            }
                            if (editor.Grid.Columns["WeighingBatchID"] != null)
                            {
                                editor.Grid.Columns["WeighingBatchID"].Width = 120;
                            }
                            if (editor.Grid.Columns["SampleName"] != null)
                            {
                                editor.Grid.Columns["SampleName"].Width = 120;
                            }
                            if (editor.Grid.Columns["MetrcSampleID"] != null)
                            {
                                editor.Grid.Columns["MetrcSampleID"].Width = 120;
                            }
                        }
                        else if (View.Id == "PTStudyLog_ListView")
                        {
                            if (Convert.ToInt32(strscreenwidth) < 1372)
                            {
                                gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                            }

                            if (editor.Grid.Columns["DateResultReceived"] != null)
                            {
                                editor.Grid.Columns["DateResultReceived"].Width = 150;
                            }
                            if (editor.Grid.Columns["DateResultSubmitted"] != null)
                            {
                                editor.Grid.Columns["DateResultSubmitted"].Width = 150;
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = 150;
                            }
                        }
                        else if (View.Id == "Matrix_ListView")
                        {
                            gridView.SettingsPager.AlwaysShowPager = false;
                            gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridView.SettingsPager.PageSizeItemSettings.Visible = false;
                        }

                        #region RemovePageSizeFooter
                        if (View.Id == "TestMethod_ListView_PrepQueue" || View.Id == "TestMethod_ListView_AnalysisQueue" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView" || View.Id == "SampleParameter_ListView_Copy_Reporting_MainView" ||
                                        View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_RawDataLevel2BatchReview" || /*View.Id == "Reporting_ListView" || View.Id == "Reporting_ListView_Delivery" ||*/ View.Id == "SampleParameter_ListView_ResultValidation_ABID" ||
                                        View.Id == "SampleParameter_ListView_ResultValidation_QueryPanel" || View.Id == "SampleParameter_ListView_ResultApproval_QueryPanel" || View.Id == "SampleParameter_ListView_ResultApproval_ABID" || View.Id == "SampleParameter_ListView_ResultView_ABID" ||
                                        View.Id == "SampleParameter_ListView_Copy_ResultView_Main" || View.Id == "HelpCenter_ListView_Articles" || View.Id == "HelpCenter_ListView_Articles_Manual" ||
                                        View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_DataPackage" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_DataPackage_History" || View.Id == "CrossChecklistSetup_ListView" ||
                                        View.Id == "CrossCheckListCategory_ListView" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_DataPackage_History_Queue" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_DataPackage_History_Review" ||
                                        View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_DataPackage_Queue" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_DataPackage_Review")
                        {
                            gridView.SettingsPager.AlwaysShowPager = false;
                            gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridView.SettingsPager.PageSizeItemSettings.Visible = false;
                        }
                        #endregion

                        if (View.Id == "ContainerSettings_ListView_testmethod" || View.Id == "ContainerSettings_ListView_testmethod_Edit")
                        {
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                editor.Grid.Columns["Edit"].Visible = false;
                            }
                            if (editor.Grid.Columns["Matrix.MatrixName"] != null)
                            {
                                editor.Grid.Columns["Matrix.MatrixName"].Width = 130;
                            }
                            if (editor.Grid.Columns["Test.TestName"] != null)
                            {
                                editor.Grid.Columns["Test.TestName"].Width = 130;
                            }
                            if (editor.Grid.Columns["Method.MethodName.MethodNumber"] != null)
                            {
                                editor.Grid.Columns["Method.MethodName.MethodNumber"].Width = 130;
                            }
                            if (editor.Grid.Columns["Container"] != null)
                            {
                                editor.Grid.Columns["Container"].Width = 120;
                            }
                            if (editor.Grid.Columns["DefaultContainer"] != null)
                            {
                                editor.Grid.Columns["DefaultContainer"].Width = 120;
                            }
                            if (editor.Grid.Columns["Preservative"] != null)
                            {
                                editor.Grid.Columns["Preservative"].Width = 120;
                            }
                            if (editor.Grid.Columns["HTBeforePrep"] != null)
                            {
                                editor.Grid.Columns["HTBeforePrep"].Width = 120;
                            }
                            if (editor.Grid.Columns["HTBeforeAnalysis"] != null)
                            {
                                editor.Grid.Columns["HTBeforeAnalysis"].Width = 120;
                            }
                            if (editor.Grid.Columns["SetPreTimeAsAnalysisTime"] != null)
                            {
                                editor.Grid.Columns["SetPreTimeAsAnalysisTime"].Width = 150;
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = 200;
                            }
                        }
                        else if (View.Id == "Samplecheckin_ListView_SampleReceiptNotification" || View.Id == "Samplecheckin_ListView_SampleReceiptNotification_History")
                        {
                            if (editor.Grid.Columns["JobID"] != null)
                            {
                                editor.Grid.Columns["JobID"].Width = 100;
                            }
                            if (editor.Grid.Columns["MailContent"] != null)
                            {
                                editor.Grid.Columns["MailContent"].Width = 200;
                                editor.Grid.Columns["MailContent"].Caption = "MailContent";

                            }
                            if (editor.Grid.Columns["COC"] != null)
                            {
                                editor.Grid.Columns["COC"].Width = 200;
                                editor.Grid.Columns["COC"].Caption = "COC...";
                            }
                            if (editor.Grid.Columns["PreInvoice"] != null)
                            {
                                editor.Grid.Columns["PreInvoice"].Width = 250;
                                editor.Grid.Columns["PreInvoice"].Caption = "PreInvoice...";
                            }

                            if (editor.Grid.Columns["ProjectID"] != null)
                            {
                                editor.Grid.Columns["ProjectID"].Width = 150;
                            }
                            if (editor.Grid.Columns["ProjectName"] != null)
                            {
                                editor.Grid.Columns["ProjectName"].Width = 150;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 200;
                            }
                            if (editor.Grid.Columns["DateReceived"] != null)
                            {
                                editor.Grid.Columns["DateReceived"].Width = 150;
                            }
                            if (editor.Grid.Columns["TAT"] != null)
                            {
                                editor.Grid.Columns["TAT"].Width = 100;
                            }
                            if (editor.Grid.Columns["DueDate"] != null)
                            {
                                editor.Grid.Columns["DueDate"].Width = 100;
                            }
                            if (editor.Grid.Columns["Comments"] != null)
                            {
                                editor.Grid.Columns["Comments"].Width = 70;
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = 100;
                            }
                            if (editor.Grid.Columns["DateTimeReceived"] != null)
                            {
                                editor.Grid.Columns["DateTimeReceived"].Width = 150;
                            }
                            if (editor.Grid.Columns["DateTimeSignedOff"] != null)
                            {
                                editor.Grid.Columns["DateTimeSignedOff"].Width = 150;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["COCAttachedPreview"] != null)
                            {
                                gridView.VisibleColumns["COCAttachedPreview"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "AssignDashboardToUserDepartment_ListView")
                        {
                            if (editor.Grid.Columns["Template_Preview"] != null)
                            {
                                editor.Grid.Columns["Template_Preview"].Width = 200;
                                editor.Grid.Columns["Template_Preview"].Caption = "Template";
                                editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            }
                        }
                        else if (View.Id == "Reporting_ListView_Datacenter")
                        {
                            if (editor.Grid.Columns["ReportID"] != null)
                            {
                                gridView.VisibleColumns["ReportID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                gridView.VisibleColumns["Client"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["ReportManagementPreviewDC"] != null)
                            {
                                editor.Grid.Columns["ReportManagementPreviewDC"].Caption = "Preview";
                                editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            }
                        }
                        else if (View.Id == "Invoicing_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["InvoiceID"] != null)
                            {
                                gridView.VisibleColumns["InvoiceID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            //if (editor.Grid.Columns["InvoiceID"] != null)
                            //{                           
                            //    gridView.VisibleColumns["InvoiceID"].Width = 150;
                            //}
                            //if (editor.Grid.Columns["Client"] != null)
                            //{
                            //    gridView.VisibleColumns["Client"].Width = 150;
                            //}
                            //if (editor.Grid.Columns["Date"] != null)
                            //{
                            //    gridView.VisibleColumns["Date"].Width = 150;
                            //}
                            //if (editor.Grid.Columns["Status"] != null)
                            //{
                            //    gridView.VisibleColumns["Status"].Width = 150;
                            //}
                            if (editor.Grid.Columns["InvoicePreviewDC"] != null)
                            {
                                editor.Grid.Columns["InvoicePreviewDC"].Caption = "Preview";
                                editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            }

                            foreach (IModelColumn column in View.Model.Columns)
                            {
                                column.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                                if (column.PropertyName == "InvoiceID")
                                {
                                    column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                                }
                            }
                            ((IModelListViewWeb)View.Model).PageSize = 200;
                        }
                        else if (View.Id == "CRMQuotes_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["QuoteID"] != null)
                            {
                                gridView.VisibleColumns["QuoteID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                gridView.VisibleColumns["Client"].Width = Unit.Percentage(40);
                            }
                            if (editor.Grid.Columns["QuotePreviewDC"] != null)
                            {
                                //editor.Grid.Columns["ReportManagementPreview"].Width = 200;
                                editor.Grid.Columns["QuotePreviewDC"].Caption = "Preview";
                                editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            }
                            //
                        }
                        else if (View.Id == "CRMQuotes_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["QuoteID"] != null)
                            {
                                gridView.VisibleColumns["QuoteID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                gridView.VisibleColumns["Client"].Width = Unit.Percentage(40);
                            }
                            if (editor.Grid.Columns["QuotePreviewDC"] != null)
                            {
                                editor.Grid.Columns["QuotePreviewDC"].Caption = "Preview";
                                editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                            }
                        }
                        else if (View.Id == "CRMQuotes_ListView_Reviewed" || View.Id == "CRMQuotes_ListView_PendingReview")
                        {
                            if (editor.Grid.Columns["InlineEditCommandColumn"] != null)
                            {
                                editor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                            }
                        }
                        else if (View.Id == "TabFieldConfiguration_ListView_SampleParameters" || View.Id == "TabFieldConfiguration_ListView_Surrogates" || View.Id == "TabFieldConfiguration_ListView_InternalStandards" || View.Id == "TabFieldConfiguration_ListView_QCParameters" || View.Id == "TabFieldConfiguration_ListView_Components" || View.Id == "TabFieldConfiguration_ListView_QCParameterDefaults")
                        {
                            if (editor.Grid.Columns["FieldCustomCaption"] != null)
                            {
                                editor.Grid.Columns["FieldCustomCaption"].Width = 140;
                            }
                        }
                        else if (View.Id == "SampleSites_ListView" || View.Id == "Customer_SampleSites_ListView")
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 200;
                            }
                            if (editor.Grid.Columns["Address"] != null)
                            {
                                editor.Grid.Columns["Address"].Width = 200;
                            }
                            if (editor.Grid.Columns["ProjectName"] != null)
                            {
                                editor.Grid.Columns["ProjectName"].Width = 150;
                            }
                            if (editor.Grid.Columns["SamplingAddress"] != null)
                            {
                                editor.Grid.Columns["SamplingAddress"].Width = 150;
                            }
                            if (editor.Grid.Columns["StationLocation"] != null)
                            {
                                editor.Grid.Columns["StationLocation"].Width = 150;
                            }
                            if (editor.Grid.Columns["CollectorPhone"] != null)
                            {
                                editor.Grid.Columns["CollectorPhone"].Width = 120;
                            }
                            if (editor.Grid.Columns["ParentSampleID"] != null)
                            {
                                editor.Grid.Columns["ParentSampleID"].Width = 150;
                            }
                            if (editor.Grid.Columns["ParentSampleDate"] != null)
                            {
                                editor.Grid.Columns["ParentSampleDate"].Width = 150;
                            }
                            if (editor.Grid.Columns["RejectionCriteria"] != null)
                            {
                                editor.Grid.Columns["RejectionCriteria"].Width = 150;
                            }
                            if (editor.Grid.Columns["RepeatLocation"] != null)
                            {
                                editor.Grid.Columns["RepeatLocation"].Width = 150;
                            }
                            if (editor.Grid.Columns["EnteredDate"] != null)
                            {
                                editor.Grid.Columns["EnteredDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["EnteredBy"] != null)
                            {
                                editor.Grid.Columns["EnteredBy"].Width = 120;
                            }
                            if (editor.Grid.Columns["LastUpdatedDate"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedDate"].Width = 130;
                            }
                            if (editor.Grid.Columns["LastUpdatedBy"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedBy"].Width = 130;
                            }

                            if (editor.Grid.Columns["SiteNameArchived"] != null)
                            {
                                editor.Grid.Columns["SiteNameArchived"].Width = 140;
                            }
                            if (editor.Grid.Columns["SamplingPointID"] != null)
                            {
                                editor.Grid.Columns["SamplingPointID"].Width = 140;
                            }
                            if (editor.Grid.Columns["SamplePointType"] != null)
                            {
                                editor.Grid.Columns["SamplePointType"].Width = 140;
                            }
                            if (editor.Grid.Columns["MonitoryingRequirement"] != null)
                            {
                                editor.Grid.Columns["MonitoryingRequirement"].Width = 180;
                            }
                            if (editor.Grid.Columns["SiteUserDefinedColumn1"] != null)
                            {
                                editor.Grid.Columns["SiteUserDefinedColumn1"].Width = 180;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SiteID"] != null)
                            {
                                gridView.VisibleColumns["SiteID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        //else if (View.Id == "Requisition_ListView_Cancelled"||View.Id== "Requisition_ListView_Tracking")
                        //{

                        //    if (Convert.ToInt32(strscreenwidth) < 1380)
                        //    {
                        //        gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                        //    }                      

                        //}






                        else if (View.Id == "SubOutContractLab_ListView")
                        {
                            if (editor.Grid.VisibleColumns["SelectionCommandColumn"] != null)
                            {
                                editor.Grid.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.VisibleColumns["Edit"] != null)
                            {
                                editor.Grid.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.VisibleColumns["ContractLabName"] != null)
                            {
                                editor.Grid.VisibleColumns["ContractLabName"].Width = 180;
                                editor.Grid.VisibleColumns["ContractLabName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Address"] != null)
                            {
                                editor.Grid.Columns["Address"].Width = 130;
                            }
                            if (editor.Grid.Columns["City"] != null)
                            {
                                editor.Grid.Columns["City"].Width = 130;
                            }
                            if (editor.Grid.Columns["State"] != null)
                            {
                                editor.Grid.Columns["State"].Width = 130;
                            }
                            if (editor.Grid.Columns["Country"] != null)
                            {
                                editor.Grid.Columns["Country"].Width = 130;
                            }
                            if (editor.Grid.Columns["Zip"] != null)
                            {
                                editor.Grid.Columns["Zip"].Width = 130;
                            }
                            if (editor.Grid.Columns["AccreditationID"] != null)
                            {
                                editor.Grid.Columns["AccreditationID"].Width = 130;
                            }
                            if (editor.Grid.Columns["Contact"] != null)
                            {
                                editor.Grid.Columns["Contact"].Width = 130;
                            }
                            if (editor.Grid.Columns["Phone"] != null)
                            {
                                editor.Grid.Columns["Phone"].Width = 130;
                            }
                            if (editor.Grid.Columns["EmailID"] != null)
                            {
                                editor.Grid.Columns["EmailID"].Width = 170;
                            }
                            if (editor.Grid.Columns["WebSite"] != null)
                            {
                                editor.Grid.Columns["WebSite"].Width = 100;
                            }
                            if (editor.Grid.Columns["Certificate"] != null)
                            {
                                editor.Grid.Columns["Certificate"].Width = 150;
                            }


                        }
                        else if (View.Id == "Customer_SampleSites_ListView")
                        {
                            if (editor.Grid.Columns["SiteID"] != null)
                            {
                                editor.Grid.VisibleColumns["SiteID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SiteName"] != null)
                            {
                                editor.Grid.VisibleColumns["SiteName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "CRMProspects_ListView_Copy_Closed")
                        {
                            if (editor.Grid.Columns["Topic"] != null)
                            {
                                editor.Grid.Columns["Topic"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Prospects"] != null)
                            {
                                editor.Grid.Columns["Prospects"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Address"] != null)
                            {
                                editor.Grid.Columns["Address"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Contact"] != null)
                            {
                                editor.Grid.Columns["Contact"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Phone"] != null)
                            {
                                editor.Grid.Columns["Phone"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Email"] != null)
                            {
                                editor.Grid.Columns["Email"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Probability"] != null)
                            {
                                editor.Grid.Columns["Probability"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = Unit.Percentage(25);
                            }

                        }
                        else if (View.Id == "CRMProspects_ListView_MyOpenLeads_Copy")
                        {
                            if (editor.Grid.Columns["Topic"] != null)
                            {
                                editor.Grid.Columns["Topic"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Prospects"] != null)
                            {
                                editor.Grid.Columns["Prospects"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Address"] != null)
                            {
                                editor.Grid.Columns["Address"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Contact"] != null)
                            {
                                editor.Grid.Columns["Contact"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Phone"] != null)
                            {
                                editor.Grid.Columns["Phone"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Email"] != null)
                            {
                                editor.Grid.Columns["Email"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Probability"] != null)
                            {
                                editor.Grid.Columns["Probability"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = Unit.Percentage(25);
                            }

                        }
                        if (View.Id == "CRMProspects_ListView")
                        {
                            if (editor.Grid.Columns["Topic"] != null)
                            {
                                editor.Grid.Columns["Topic"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Prospects"] != null)
                            {
                                editor.Grid.Columns["Prospects"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Address"] != null)
                            {
                                editor.Grid.Columns["Address"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Contact"] != null)
                            {
                                editor.Grid.Columns["Contact"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Phone"] != null)
                            {
                                editor.Grid.Columns["Phone"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Email"] != null)
                            {
                                editor.Grid.Columns["Email"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Probability"] != null)
                            {
                                editor.Grid.Columns["Probability"].Width = Unit.Percentage(25);
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = Unit.Percentage(25);
                            }

                        }
                        else if (View.Id == "Testparameter_LookupListView_Copy_SampleLogin")
                        {
                            if (editor.Grid.Columns["Component"] != null)
                            {
                                editor.Grid.Columns["Component"].Width = 103;
                            }
                        }
                        else if (View.Id == "Customer_ReportingContact_ListView")
                        {
                            if (editor.Grid.Columns["ReportContactID"] != null)
                            {
                                editor.Grid.Columns["ReportContactID"].Width = 150;
                                editor.Grid.VisibleColumns["ReportContactID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["ReportContact"] != null)
                            {
                                editor.Grid.Columns["ReportContact"].Width = 150;
                                editor.Grid.VisibleColumns["ReportContact"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["ReportEmailCC"] != null)
                            {
                                editor.Grid.Columns["ReportEmailCC"].Width = 120;
                            }
                            if (editor.Grid.Columns["ReportEmailBCC"] != null)
                            {
                                editor.Grid.Columns["ReportEmailBCC"].Width = 120;
                            }
                            if (editor.Grid.Columns["DefaultContact"] != null)
                            {
                                editor.Grid.Columns["DefaultContact"].Width = 120;
                            }
                            if (editor.Grid.Columns["LastUpdatedDate"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedDate"].Width = 150;
                            }
                            if (editor.Grid.Columns["Last Updated By"] != null)
                            {
                                editor.Grid.Columns["Last Updated By"].Width = 150;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "Customer_InvoicingContact_ListView")
                        {
                            if (editor.Grid.Columns["InvoiceContactID"] != null)
                            {
                                editor.Grid.Columns["InvoiceContactID"].Width = 120;
                                editor.Grid.VisibleColumns["InvoiceContactID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["InvoiceContact"] != null)
                            {
                                editor.Grid.Columns["InvoiceContact"].Width = 120;
                                editor.Grid.VisibleColumns["InvoiceContact"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["InvoiceEmailCC"] != null)
                            {
                                editor.Grid.Columns["InvoiceEmailCC"].Width = 120;
                            }
                            if (editor.Grid.Columns["InvoiceEmailBCC"] != null)
                            {
                                editor.Grid.Columns["InvoiceEmailBCC"].Width = 120;
                            }
                            if (editor.Grid.Columns["DefaultContact"] != null)
                            {
                                editor.Grid.Columns["DefaultContact"].Width = 120;
                            }
                            if (editor.Grid.Columns["LastUpdatedDate"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedDate"].Width = 150;
                            }
                            if (editor.Grid.Columns["Last Updated By"] != null)
                            {
                                editor.Grid.Columns["Last Updated By"].Width = 150;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "AnalysisPricing_ListView_Quotes_SampleRegistration" || View.Id == "AnalysisPricing_ListView_Quotes_SamplingProposal")
                        {
                            if (editor.Grid.Columns["Qty"] != null)
                            {
                                editor.Grid.Columns["Qty"].CellStyle.BackColor = Color.LightYellow;
                            }
                        }
                        else if (View.Id == "Customer_InvoicingAddress_ListView")
                        {
                            if (editor.Grid.Columns["InvoiceAddressID"] != null)
                            {
                                editor.Grid.VisibleColumns["InvoiceAddressID"].Width = 120;
                                editor.Grid.VisibleColumns["InvoiceAddressID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Invoice Client"] != null)
                            {
                                editor.Grid.VisibleColumns["Invoice Client"].Width = 150;
                                editor.Grid.VisibleColumns["Invoice Client"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["DefaultAddress"] != null)
                            {
                                editor.Grid.Columns["DefaultAddress"].Width = 120;
                            }
                            if (editor.Grid.Columns["LastUpdatedDate"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedDate"].Width = 150;
                            }
                            if (editor.Grid.Columns["Last Updated By"] != null)
                            {
                                editor.Grid.Columns["Last Updated By"].Width = 150;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "Customer_ListView_InvoiceAddress")
                        {
                            if (editor.Grid.Columns["CustomerName"] != null)
                            {
                                editor.Grid.Columns["CustomerName"].Width = 200;
                            }
                            if (editor.Grid.Columns["Address"] != null)
                            {
                                editor.Grid.Columns["Address"].Width = 200;
                            }
                            if (editor.Grid.Columns["City"] != null)
                            {
                                editor.Grid.Columns["City"].Width = 100;
                            }
                            if (editor.Grid.Columns["State"] != null)
                            {
                                editor.Grid.Columns["State"].Width = 100;
                            }
                            if (editor.Grid.Columns["Zip"] != null)
                            {
                                editor.Grid.Columns["Zip"].Width = 100;
                            }
                            if (editor.Grid.Columns["OfficePhone"] != null)
                            {
                                editor.Grid.Columns["OfficePhone"].Width = 120;
                            }
                        }
                        else if (/*View.Id == "Samplecheckin_ListView_Copy_Registration_History" ||*/ View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff"
                            || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Copy_SuboutSampleRegistration" && Convert.ToInt32(strscreenwidth) < 1600
                            || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_SignOff" && Convert.ToInt32(strscreenwidth) < 1600
                            || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_Tracking" && Convert.ToInt32(strscreenwidth) < 1600)
                        {
                            if (gridView.VisibleColumns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.VisibleColumns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.VisibleColumns["JobID"] != null)
                            {
                                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.VisibleColumns["Client"] != null)
                            {
                                gridView.VisibleColumns["Client"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "Reporting_ListView_Copy_ReportView")
                        {
                          if (gridView.VisibleColumns["SelectionCommandColumn"] != null)
                          {
                              gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                          }

                       
                        if (editor.Grid.Columns["Client"] != null)
                        {
                            editor.Grid.Columns["Client"].Width = Unit.Percentage(25);
                        }
                    
                      }
                     
                        else if (View.Id == "DefaultSetting_ListView_2")
                        {
                            if (editor.Grid.Columns["ItemPath"] != null)
                            {
                                editor.Grid.Columns["ItemPath"].Width = 300;
                            }
                        }
                        else if (View.Id == "Distribution_ListView_Fractional_Consumption_History")
                        {
                            if (editor.Grid.Columns["ConsumedDate"] != null)
                            {
                                editor.Grid.Columns["ConsumedDate"].Width = 150;
                            }
                        }

                        else if (View.Id == "Customer_Notes_ListView_ContactLog" || View.Id == "CRMProspects_Notes_ListView_ContactLog")
                        {
                            if (editor.Grid.Columns["Date"] != null)
                            {
                                editor.Grid.Columns["Date"].Width = 120;
                            }
                            if (editor.Grid.Columns["Title"] != null)
                            {
                                editor.Grid.Columns["Title"].Width = 200;
                            }
                            if (editor.Grid.Columns["Customer"] != null)
                            {
                                editor.Grid.Columns["Customer"].Width = 200;
                            }
                            if (editor.Grid.Columns["Author"] != null)
                            {
                                editor.Grid.Columns["Author"].Width = 200;
                            }
                        }
                        else if (View.Id == "SubOutSampleRegistrations_SampleParameter_ListView" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_QCResults" || View.Id == "SubOutSampleRegistrations_SubOutQcSample_ListView"
                            || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_QCResultsView" || View.Id == "SubOutSampleRegistrations_SampleParameter_ListView_ViewMode")
                        {
                            if (gridView.VisibleColumns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.VisibleColumns["JobID"] != null)
                            {
                                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (gridView.VisibleColumns["SampleID"] != null)
                            {
                                gridView.VisibleColumns["SampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        //else if (View.Id == "QCBatchSequence_ListView")
                        //{
                        //    if (editor.Grid.Columns["QCType"] != null)
                        //    {
                        //        editor.Grid.Columns["QCType"].Width = 100;
                        //    }
                        //    if (editor.Grid.Columns["Sort"] != null)
                        //    {
                        //        editor.Grid.Columns["Sort"].Width = 70;
                        //    }
                        //    if (editor.Grid.Columns["Client"] != null)
                        //    {
                        //        editor.Grid.Columns["Client"].Width = 150;
                        //    }
                        //    if (editor.Grid.Columns["SYSSamplecode"] != null)
                        //    {
                        //        editor.Grid.Columns["SYSSamplecode"].Width = 225;
                        //    }
                        //    if (editor.Grid.Columns["SampleName"] != null)
                        //    {
                        //        editor.Grid.Columns["SampleName"].Width = 150;
                        //    }
                        //    if (editor.Grid.Columns["DateCollected"] != null)
                        //    {
                        //        editor.Grid.Columns["DateCollected"].Width = 150;
                        //    }
                        //    if (editor.Grid.Columns["ProjectID"] != null)
                        //    {
                        //        editor.Grid.Columns["ProjectID"].Width = 150;
                        //    }
                        //    if (editor.Grid.Columns["DilutionCount"] != null)
                        //    {
                        //        editor.Grid.Columns["DilutionCount"].Width = 100;
                        //    }
                        //    if (editor.Grid.Columns["Dilution"] != null)
                        //    {
                        //        editor.Grid.Columns["Dilution"].Width = 100;
                        //    }
                        //}
                        else if (View.Id == "QCBatchSequence_ListView_Copy")
                        {
                            if (editor.Grid.Columns["QCType"] != null)
                            {
                                editor.Grid.Columns["QCType"].Width = 90;
                            }
                            if (editor.Grid.Columns["Sort"] != null)
                            {
                                editor.Grid.Columns["Sort"].Width = 70;
                            }
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 150;
                            }
                            if (editor.Grid.Columns["SYSSamplecode"] != null)
                            {
                                editor.Grid.Columns["SYSSamplecode"].Width = 225;
                            }
                            if (editor.Grid.Columns["SampleName"] != null)
                            {
                                editor.Grid.Columns["SampleName"].Width = 150;
                            }
                            if (editor.Grid.Columns["DateCollected"] != null)
                            {
                                editor.Grid.Columns["DateCollected"].Width = 150;
                            }
                            if (editor.Grid.Columns["ProjectID"] != null)
                            {
                                editor.Grid.Columns["ProjectID"].Width = 150;
                            }
                            if (editor.Grid.Columns["DilutionCount"] != null)
                            {
                                editor.Grid.Columns["DilutionCount"].Width = 100;
                            }
                            if (editor.Grid.Columns["Dilution"] != null)
                            {
                                editor.Grid.Columns["Dilution"].Width = 100;
                            }
                        }
                        else if (View.Id == "SDMSDCAB_ListView")
                        {
                            if (editor.Grid.Columns["SYSSamplecode"] != null)
                            {
                                editor.Grid.Columns["SYSSamplecode"].Width = 225;
                            }
                        }
                        else if (View.Id == "SampleLogIn_ListView_SampleDisposition" || View.Id == "SampleLogIn_ListView_SampleDisposition_DisposedSamples" || View.Id == "SampleLogIn_ListView_SampleDisposition_CurrentSamples")
                        {
                            if (editor.Grid.Columns["ClientSampleID"] != null)
                            {
                                editor.Grid.Columns["ClientSampleID"].Width = 100;
                            }
                            if (editor.Grid.Columns["SampleMatrix"] != null)
                            {
                                editor.Grid.Columns["SampleMatrix"].Width = 150;
                            }
                            if (editor.Grid.Columns["DaysSampleKeeping"] != null)
                            {
                                editor.Grid.Columns["DaysSampleKeeping"].Width = 150;
                            }
                            if (editor.Grid.Columns["DisposedDate"] != null)
                            {
                                editor.Grid.Columns["DisposedDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["DisposedBy"] != null)
                            {
                                editor.Grid.Columns["DisposedBy"].Width = 120;
                            }
                            if (editor.Grid.Columns["DaysRemain"] != null)
                            {
                                editor.Grid.Columns["DaysRemain"].Width = 120;
                            }
                            if (editor.Grid.Columns["DateCollected"] != null)
                            {
                                editor.Grid.Columns["DateCollected"].Width = 120;
                            }
                            if (editor.Grid.Columns["DateReceived"] != null)
                            {
                                editor.Grid.Columns["DateReceived"].Width = 120;
                            }
                            if (editor.Grid.Columns["PreserveCondition"] != null)
                            {
                                editor.Grid.Columns["PreserveCondition"].Width = 160;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["JobID"] != null && editor.Grid.Columns["SampleID"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["SampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "SubOutContractLab_CertifiedTests_ListView")
                        {
                            if (editor.Grid.VisibleColumns["SelectionCommandColumn"] != null)
                            {
                                editor.Grid.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.VisibleColumns["Edit"] != null)
                            {
                                editor.Grid.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.VisibleColumns["InlineEditCommandColumn"] != null)
                            {
                                editor.Grid.VisibleColumns["InlineEditCommandColumn"].Visible = true;
                                editor.Grid.VisibleColumns["InlineEditCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.VisibleColumns["Matrix"] != null)
                            {
                                editor.Grid.VisibleColumns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.VisibleColumns["Test"] != null)
                            {
                                editor.Grid.VisibleColumns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.VisibleColumns["Method"] != null)
                            {
                                editor.Grid.VisibleColumns["Method"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.VisibleColumns["btnparameter"] != null)
                            {
                                editor.Grid.VisibleColumns["btnparameter"].Caption = "Parameter";
                            }
                        }
                        else if (View.Id == "AnalysisPricing_ListView_Quotes" || View.Id == "CRMQuotes_AnalysisPricing_ListView")
                        {
                            if (editor.Grid.Columns["TestDescription"] != null)
                            {
                                editor.Grid.Columns["TestDescription"].Width = 150;
                            }
                            if (editor.Grid.Columns["Qty"] != null)
                            {
                                editor.Grid.Columns["Qty"].Width = 80;
                            }
                            if (editor.Grid.Columns["Sort"] != null)
                            {
                                editor.Grid.Columns["Sort"].Width = 50;
                            }
                            if (editor.Grid.Columns["UnitPrice"] != null)
                            {
                                editor.Grid.Columns["UnitPrice"].Width = 100;
                            }
                            if (editor.Grid.Columns["IsGroup"] != null)
                            {
                                editor.Grid.Columns["IsGroup"].Width = 80;
                            }
                            if (editor.Grid.Columns["TAT"] != null)
                            {
                                editor.Grid.Columns["TAT"].Width = 70;
                            }
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 100;
                                editor.Grid.Columns["Parameter"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["ChargeType"] != null)
                            {
                                editor.Grid.Columns["ChargeType"].Width = 100;
                                editor.Grid.Columns["ChargeType"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["FinalAmount"] != null)
                            {
                                editor.Grid.Columns["FinalAmount"].Width = 100;
                                editor.Grid.Columns["FinalAmount"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["TestDescription"] != null)
                            {
                                editor.Grid.Columns["TestDescription"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Sample Matrix"] != null)
                            {
                                editor.Grid.Columns["Sample Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                editor.Grid.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["PriceCode"] != null)
                            {
                                editor.Grid.Columns["PriceCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["InlineEditCommandColumn"] != null)
                            {
                                editor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "Invoicing_ListView_Review" || View.Id == "Invoicing_ListView_PendingReview_History")
                        {
                            //if (editor.Grid.Columns["InlineEditCommandColumn"] != null)
                            //{
                            //    editor.Grid.Columns["InlineEditCommandColumn"].Visible = false;
                            //}
                            //editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "CRMQuotes_ItemChargePricing_ListView")
                        {
                            if (editor.Grid.Columns["Amount"] != null)
                            {
                                editor.Grid.Columns["Amount"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["ChargeType"] != null)
                            {
                                editor.Grid.Columns["Discount"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["FinalAmount"] != null)
                            {
                                editor.Grid.Columns["FinalAmount"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["ChargeType"] != null)
                            {
                                editor.Grid.Columns["Discount"].CellStyle.ForeColor = Color.Black;
                            }
                        }
                        else if (View.Id == "Container_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["ContainerName"] != null)
                            {
                                editor.Grid.Columns["ContainerName"].Width = Unit.Percentage(35);
                            }
                            if (editor.Grid.Columns["ContainerCode"] != null)
                            {
                                editor.Grid.Columns["ContainerCode"].Width = Unit.Percentage(35);
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = Unit.Percentage(30);
                            }

                        }
                        else if (View.Id == "Component_ListView_Datacenter")
                        {
                            if (editor.Grid.Columns["Components"] != null)
                            {
                                editor.Grid.Columns["Components"].Width = Unit.Percentage(20);
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = Unit.Percentage(30);
                            }
                            if (editor.Grid.Columns["RetireDate"] != null)
                            {
                                editor.Grid.Columns["RetireDate"].Width = Unit.Percentage(15);
                            }
                            if (editor.Grid.Columns["LastUpdatedDate"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedDate"].Width = Unit.Percentage(15);
                            }
                            if (editor.Grid.Columns["LastUpdatedBy"] != null)
                            {
                                editor.Grid.Columns["LastUpdatedBy"].Width = Unit.Percentage(20);
                            }
                            ((IModelListViewWeb)View.Model).PageSize = 200;
                        }
                        if (View.Id == "TestPriceSurcharge_ListView_DataCenter")
                        {
                            ((IModelListViewWeb)View.Model).PageSize = 200;
                            gridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;

                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].Width = 150;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 150;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].Width = 150;
                            }
                            if (editor.Grid.Columns["SurchargePrice"] != null)
                            {
                                editor.Grid.Columns["SurchargePrice"].Width = 150;
                            }
                            if (editor.Grid.Columns["Surcharge(%)"] != null)
                            {
                                editor.Grid.Columns["Surcharge(%)"].Width = 150;
                            }
                        }
                        else if (View.Id == "DefaultPricing_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["PriceCode"] != null)
                            {
                                gridView.VisibleColumns["PriceCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                gridView.VisibleColumns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                gridView.VisibleColumns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (View.Id == "Customer_ListView_Copy_DC")
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = Unit.Percentage(12);
                            }
                            if (editor.Grid.Columns["Account"] != null)
                            {
                                editor.Grid.Columns["Account"].Width = Unit.Percentage(7);
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = Unit.Percentage(9);
                            }
                            if (editor.Grid.Columns["Phone"] != null)
                            {
                                editor.Grid.Columns["Phone"].Width = Unit.Percentage(9);
                            }
                            if (editor.Grid.Columns["Fax"] != null)
                            {
                                editor.Grid.Columns["Fax"].Width = Unit.Percentage(9);
                            }
                            if (editor.Grid.Columns["WebSite"] != null)
                            {
                                editor.Grid.Columns["WebSite"].Width = Unit.Percentage(12);
                            }
                            if (editor.Grid.Columns["Address1"] != null)
                            {
                                editor.Grid.Columns["Address1"].Width = Unit.Percentage(10);
                            }
                            if (editor.Grid.Columns["Address2"] != null)
                            {
                                editor.Grid.Columns["Address2"].Width = Unit.Percentage(10);
                            }
                            if (editor.Grid.Columns["City"] != null)
                            {
                                editor.Grid.Columns["City"].Width = Unit.Percentage(8);
                            }
                            if (editor.Grid.Columns["State"] != null)
                            {
                                editor.Grid.Columns["State"].Width = Unit.Percentage(8);
                            }
                            if (editor.Grid.Columns["Zip"] != null)
                            {
                                editor.Grid.Columns["Zip"].Width = Unit.Percentage(5);
                            }
                        }
                        else if (View.Id == "Items_ListView_Copy_DC")
                        {
                            if (editor.Grid.Columns["ItemName"] != null)
                            {
                                editor.Grid.Columns["ItemName"].Width = Unit.Percentage(18);
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = Unit.Percentage(30);
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = Unit.Percentage(10);
                            }
                            if (editor.Grid.Columns["Vendor"] != null)
                            {
                                editor.Grid.Columns["Vendor"].Width = Unit.Percentage(10);
                            }
                            if (editor.Grid.Columns["Catalog#"] != null)
                            {
                                editor.Grid.Columns["Catalog#"].Width = Unit.Percentage(10);
                            }
                            if (editor.Grid.Columns["Size"] != null)
                            {
                                editor.Grid.Columns["Size"].Width = Unit.Percentage(5);
                            }
                            if (editor.Grid.Columns["UnitPrice"] != null)
                            {
                                editor.Grid.Columns["UnitPrice"].Width = Unit.Percentage(7);
                            }
                            if (editor.Grid.Columns["StockQty"] != null)
                            {
                                editor.Grid.Columns["StockQty"].Width = Unit.Percentage(5);
                            }
                            if (editor.Grid.Columns["ItemCode"] != null)
                            {
                                editor.Grid.Columns["ItemCode"].Width = Unit.Percentage(5);
                            }
                        }
                        else if (View.Id == "Method_ListView_Copy_DC")
                        {
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                gridView.VisibleColumns["Method"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = Unit.Percentage(30);
                            }
                        }
                        else if (View.Id == "Manual_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = Unit.Percentage(30);
                            }
                            foreach (IModelColumn column in View.Model.Columns)
                            {
                                column.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                                if (column.PropertyName == "Name")
                                {
                                    column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                                }
                            }
                            ((IModelListViewWeb)View.Model).PageSize = 200;
                        }
                        else if (View.Id == "Contract_ListView_ContractTrcking_Copy_DC")
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 170;
                                gridView.VisibleColumns["Client"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["ContractID"] != null)
                            {
                                gridView.VisibleColumns["ContractID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["ContractNumber"] != null)
                            {
                                gridView.VisibleColumns["ContractNumber"].FixedStyle = GridViewColumnFixedStyle.Left;
                                editor.Grid.Columns["ContractNumber"].Width = 170;
                            }
                            if (editor.Grid.Columns["ContractCategory"] != null)
                            {
                                editor.Grid.Columns["ContractCategory"].Width = 170;
                            }
                            if (editor.Grid.Columns["ContractEndDate"] != null)
                            {
                                editor.Grid.Columns["ContractEndDate"].Width = 170;
                            }
                            if (editor.Grid.Columns["ContractAmount"] != null)
                            {
                                editor.Grid.Columns["ContractAmount"].Width = 170;
                            }
                            if (editor.Grid.Columns["ContractOverview"] != null)
                            {
                                editor.Grid.Columns["ContractOverview"].Width = 170;
                            }
                            if (editor.Grid.Columns["ContractStartDate"] != null)
                            {
                                editor.Grid.Columns["ContractStartDate"].Width = 170;
                            }
                        }
                        else if (View.Id == "Contact_ListView_Copy_DC")
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 170;
                                gridView.VisibleColumns["Client"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Email"] != null)
                            {
                                editor.Grid.Columns["Email"].Width = 170;
                            }
                            if (editor.Grid.Columns["FullName"] != null)
                            {
                                gridView.VisibleColumns["FullName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (View.Id == "Samplecheckin_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 190;
                            }
                            if (editor.Grid.Columns["ProjectID"] != null)
                            {
                                editor.Grid.Columns["ProjectID"].Width = 100;
                            }
                            if (editor.Grid.Columns["JobID"] != null)
                            {
                                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["TAT"] != null)
                            {
                                editor.Grid.Columns["TAT"].Width = 100;
                            }
                            if (editor.Grid.Columns["DueDate"] != null)
                            {
                                editor.Grid.Columns["DueDate"].Width = 100;
                            }
                        }
                        else if (View.Id == "SampleLogIn_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["Client"] != null)
                            {
                                editor.Grid.Columns["Client"].Width = 190;
                            }
                            if (editor.Grid.Columns["JobID"] != null)
                            {
                                gridView.VisibleColumns["JobID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SampleID"] != null)
                            {
                                gridView.VisibleColumns["SampleID"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["DateRegistered"] != null)
                            {
                                editor.Grid.Columns["DateRegistered"].Width = 190;
                            }
                            if (editor.Grid.Columns["SampleLocation"] != null)
                            {
                                editor.Grid.Columns["SampleLocation"].Width = 190;
                            }
                        }
                        else if (View.Id == "HelpCenter_HelpCenterAttachments_ListView_Download")
                        {
                            if (editor.Grid.Columns["Title"] != null)
                            {
                                editor.Grid.Columns["Title"].Width = 120;
                            }
                            if (editor.Grid.Columns["File"] != null)
                            {
                                editor.Grid.Columns["File"].Width = Unit.Percentage(100);
                            }
                            if (editor.Grid.Columns["Size"] != null)
                            {
                                editor.Grid.Columns["Size"].Width = 120;
                            }
                            if (editor.Grid.Columns["Sort"] != null)
                            {
                                editor.Grid.Columns["Sort"].Width = 120;
                            }
                        }
                        else if (View.Id == "HelpCenter_ListView_Articles")
                        {
                            if (editor.Grid.Columns["Module"] != null)
                            {
                                editor.Grid.Columns["Module"].Width = Unit.Percentage(100);
                            }
                            if (editor.Grid.Columns["downloadAction"] != null)
                            {
                                editor.Grid.Columns["downloadAction"].Width = 150;
                            }
                            if (editor.Grid.Columns["Topic"] != null)
                            {
                                editor.Grid.Columns["Topic"].Width = 300;
                            }
                            if (editor.Grid.Columns["Question"] != null)
                            {
                                editor.Grid.Columns["Question"].Width = 300;
                            }
                            if (editor.Grid.Columns["ReferenceAnswer"] != null)
                            {
                                editor.Grid.Columns["ReferenceAnswer"].Width = Unit.Percentage(100);
                            }
                        }
                        //else if (View.Id == "SamplePrepBatchSequence_ListView")
                        //{
                        //    if (editor.Grid.Columns["SelectionCommandColumn"] != null || editor.Grid.Columns["JobID"] != null)
                        //    {
                        //        gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                        //    }
                        //    //editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        //}
                        else if (View.Id == "DefaultPricing_ListView" || View.Id == "TestPriceSurcharge_ListView" || View.Id == "Customer_ListView" || View.Id == "Contact_ListView")
                        {
                            if (gridView.VisibleColumns["InlineEditCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["InlineEditCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["InlineEditCommandColumn"].Visible = true;
                            }
                        }
                        else if (View.Id == "Contact_ListView")
                        {
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["FullName"] != null)
                            {
                                gridView.VisibleColumns["FullName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }

                        }
                        else if (View.Id == "Customer_ListView")
                        {
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["CustomerName"] != null)
                            {
                                gridView.VisibleColumns["CustomerName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (View.Id == "DefaultPricing_ListView_Copy")
                        {
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].Width = 150;
                                editor.Grid.Columns["Matrix"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 200;
                                editor.Grid.Columns["Test"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].Width = 150;
                                editor.Grid.Columns["Method"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Component"] != null)
                            {
                                editor.Grid.Columns["Component"].Width = 150;
                                editor.Grid.Columns["Component"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["ChargeType"] != null)
                            {
                                editor.Grid.Columns["ChargeType"].Width = 125;
                                editor.Grid.Columns["ChargeType"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["UnitPrice"] != null)
                            {
                                editor.Grid.Columns["UnitPrice"].Width = 125;
                                editor.Grid.Columns["UnitPrice"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Prep1Charge"] != null)
                            {
                                editor.Grid.Columns["Prep1Charge"].Width = 100;
                                editor.Grid.Columns["Prep1Charge"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Prep2Charge"] != null)
                            {
                                editor.Grid.Columns["Prep2Charge"].Width = 100;
                                editor.Grid.Columns["Prep2Charge"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["TotalUnitPrice"] != null)
                            {
                                editor.Grid.Columns["TotalUnitPrice"].Width = 150;
                                editor.Grid.Columns["TotalUnitPrice"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Remark"] != null)
                            {
                                editor.Grid.Columns["Remark"].Width = 200;
                                editor.Grid.Columns["Remark"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                                if (View.Id == "DefaultPricing_ListView")
                                {
                                    gridView.VisibleColumns["PriceCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "DefaultPricing_ListView")
                        {
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].Width = 120;
                                editor.Grid.Columns["Matrix"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 200;
                                editor.Grid.Columns["Test"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].Width = 120;
                                editor.Grid.Columns["Method"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Component"] != null)
                            {
                                editor.Grid.Columns["Component"].Width = 120;
                                editor.Grid.Columns["Component"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["IsGroup"] != null)
                            {
                                editor.Grid.Columns["IsGroup"].Width = 80;
                                editor.Grid.Columns["IsGroup"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["ChargeType"] != null)
                            {
                                editor.Grid.Columns["ChargeType"].Width = 100;
                                editor.Grid.Columns["ChargeType"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["UnitPrice"] != null)
                            {
                                editor.Grid.Columns["UnitPrice"].Width = 80;
                                editor.Grid.Columns["UnitPrice"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Prep1Charge"] != null)
                            {
                                editor.Grid.Columns["Prep1Charge"].Width = 100;
                                editor.Grid.Columns["Prep1Charge"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Prep2Charge"] != null)
                            {
                                editor.Grid.Columns["Prep2Charge"].Width = 100;
                                editor.Grid.Columns["Prep2Charge"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["TotalUnitPrice"] != null)
                            {
                                editor.Grid.Columns["TotalUnitPrice"].Width = 150;
                                editor.Grid.Columns["TotalUnitPrice"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Remark"] != null)
                            {
                                editor.Grid.Columns["Remark"].Width = 200;
                                editor.Grid.Columns["Remark"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;

                                gridView.VisibleColumns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                                if (View.Id == "DefaultPricing_ListView")
                                {
                                    gridView.VisibleColumns["PriceCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                                }
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "tbl_Public_CustomReportDesignerDetails_ListView")
                        {
                            int RowCount = ((ListView)View).CollectionSource.GetCount();
                            if (RowCount > 8)
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = 300;
                                editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            }
                            if (editor.Grid.Columns["TemplateID"] != null)
                            {
                                editor.Grid.Columns["TemplateID"].Width = 90;
                                editor.Grid.Columns["TemplateID"].CellStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                            }
                            if (editor.Grid.Columns["TemplateName"] != null)
                            {
                                editor.Grid.Columns["TemplateName"].Width = 170;
                            }
                            if (editor.Grid.Columns["CustomCaption"] != null)
                            {
                                editor.Grid.Columns["CustomCaption"].Width = 150;
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = 120;
                            }
                            if (editor.Grid.Columns["ReportType"] != null)
                            {
                                editor.Grid.Columns["ReportType"].Width = 120;
                            }
                            if (editor.Grid.Columns["Navigation"] != null)
                            {
                                editor.Grid.Columns["Navigation"].Width = 120;
                            }
                            if (editor.Grid.Columns["BusinessObject"] != null)
                            {
                                editor.Grid.Columns["BusinessObject"].Width = 120;
                            }
                            if (editor.Grid.Columns["UserAccess"] != null)
                            {
                                editor.Grid.Columns["UserAccess"].Width = 120;
                            }
                            if (editor.Grid.Columns["Comment"] != null)
                            {
                                editor.Grid.Columns["Comment"].Width = 160;
                            }
                            if (editor.Grid.Columns["AllowMultipleJOBID"] != null)
                            {
                                editor.Grid.Columns["AllowMultipleJOBID"].Width = 160;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["InlineEditCommandColumn"] != null && editor.Grid.Columns["Edit"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["InlineEditCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["TemplateID"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["TemplateName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            foreach (WebColumnBase column in gridView.Columns)
                            {
                                if (column.Name == "Edit")
                                {
                                    column.Width = 65;
                                }
                            }
                        }
                        else if (View.Id == "HelpCenter_ListView_Articles" || View.Id == "HelpCenter_ListView_Articles_Manual" || View.Id == "CrossChecklistSetup_ListView" || View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView_DataPackage")
                        {
                            //int RowCount = ((ListView)View).CollectionSource.GetCount();
                            //if (RowCount >= 0 && !string.IsNullOrEmpty(strheight) && int.TryParse(strheight, out int a))
                            //{
                            //    editor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strheight) - (Convert.ToInt32(strheight) * 33 / 100);
                            //    editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            //}
                        }
                        else if (View.Id == "SpreadSheetEntry_AnalyticalBatch_DataPackageAttachments_ListView")
                        {
                            gridView.SettingsPager.AlwaysShowPager = false;
                            gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridView.SettingsPager.PageSizeItemSettings.Visible = false;
                            int RowCount = ((ListView)View).CollectionSource.GetCount();
                            if (RowCount >= 5 && !string.IsNullOrEmpty(strheight) && int.TryParse(strheight, out int a))
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strheight) - (Convert.ToInt32(strheight) * 75 / 100);
                                editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            }
                            else
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = 20;
                            }
                        }
                        else if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_ResultView_SingleChoice" || View.Id == "SampleParameter_ListView_Copy_QCResultView" || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_ResultEntry" || View.Id == "SampleParameter_ListView_Copy_QCResultEntry")
                        {
                            gridView.SettingsPager.AlwaysShowPager = false;
                            gridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                            gridView.SettingsPager.PageSizeItemSettings.Visible = false;
                            int RowCount = ((ListView)View).CollectionSource.GetCount();
                            if (RowCount >= 5 && !string.IsNullOrEmpty(strheight) && int.TryParse(strheight, out int a))
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strheight) - (Convert.ToInt32(strheight) * 45 / 100);
                                editor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                            }
                            else
                            {
                                editor.Grid.Settings.VerticalScrollableHeight = 20;
                            }
                        }
                        else if (View.Id == "TestPriceSurcharge_ListView")
                        {
                            if (editor.Grid.Columns["PriceCode"] != null)
                            {
                                editor.Grid.Columns["PriceCode"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["IsGroup"] != null)
                            {
                                editor.Grid.Columns["IsGroup"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Component"] != null)
                            {
                                editor.Grid.Columns["Component"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["ChargeType"] != null)
                            {
                                editor.Grid.Columns["ChargeType"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["Priority"] != null)
                            {
                                editor.Grid.Columns["Priority"].CellStyle.ForeColor = Color.Black;
                            }
                            if (editor.Grid.Columns["PrioritySort"] != null)
                            {
                                editor.Grid.Columns["PrioritySort"].CellStyle.ForeColor = Color.Black;
                            }
                        }
                        else if (View.Id == "GroupTest_TestMethods_ListView_grouptestsurcharge")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null)
                            {
                                editor.Grid.Columns["SelectionCommandColumn"].Visible = false;
                            }
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                editor.Grid.Columns["Edit"].Visible = false;
                            }
                        }




                        else if (View.Id == "ConstituentPricing_ListView")
                        {
                            if (editor.Grid.Columns["PriceCode"] != null)
                            {
                                editor.Grid.Columns["PriceCode"].Width = 150;
                            }
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].Width = 150;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 150;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].Width = 150;
                            }
                            if (editor.Grid.Columns["Component"] != null)
                            {
                                editor.Grid.Columns["Component"].Width = 150;
                            }
                            if (editor.Grid.Columns["ChargeType"] != null)
                            {
                                editor.Grid.Columns["ChargeType"].Width = 150;
                            }
                            if (editor.Grid.Columns["TierNo"] != null)
                            {
                                editor.Grid.Columns["TierNo"].Width = 150;
                            }
                            if (editor.Grid.Columns["From"] != null)
                            {
                                editor.Grid.Columns["From"].Width = 150;
                            }
                            if (editor.Grid.Columns["To"] != null)
                            {
                                editor.Grid.Columns["To"].Width = 150;
                            }
                            if (editor.Grid.Columns["TierPrice"] != null)
                            {
                                editor.Grid.Columns["TierPrice"].Width = 150;
                            }
                            if (editor.Grid.Columns["Prep1Charge"] != null)
                            {
                                editor.Grid.Columns["Prep1Charge"].Width = 150;
                            }
                            if (editor.Grid.Columns["Prep2Charge"] != null)
                            {
                                editor.Grid.Columns["Prep2Charge"].Width = 150;
                            }
                            if (editor.Grid.Columns["TotalTierPrice"] != null)
                            {
                                editor.Grid.Columns["TotalTierPrice"].Width = 150;
                            }
                            if (editor.Grid.Columns["Remark"] != null)
                            {
                                editor.Grid.Columns["Remark"].Width = 200;
                            }
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["Edit"] != null && editor.Grid.Columns["PriceCode"] != null && editor.Grid.Columns["Matrix"] != null && editor.Grid.Columns["Test"] != null)
                            {
                                gridView.VisibleColumns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["PriceCode"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";
                        }
                        else if (View.Id == "Requisition_ListView")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["InlineEditCommandColumn"] != null && editor.Grid.Columns["RequestedDate"] != null && editor.Grid.Columns["ItemName"] != null)
                            {
                                gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[3].FixedStyle = GridViewColumnFixedStyle.Left;

                            }
                        }
                        else if (View.Id == "Requisition_ListView_Review")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["InlineEditCommandColumn"] != null && editor.Grid.Columns["RequestedDate"] != null && editor.Grid.Columns["ItemName"] != null)
                            {
                                gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[3].FixedStyle = GridViewColumnFixedStyle.Left;

                            }
                            editor.Grid.ClientSideEvents.Init = @"function (s, e){s.Refresh();}";

                        }
                        else if (View.Id == "Requisition_ListView_ViewMode")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["RequestedDate"] != null && editor.Grid.Columns["ItemName"] != null)
                            {
                                gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["DeliveryPriority"] != null)
                            {
                                editor.Grid.Columns["DeliveryPriority"].Width = 150;
                            }

                        }
                        else if (View.Id == "Distribution_ListView_Consumption" || View.Id == "Distribution_ListView_ConsumptionViewmode" || View.Id == "Distribution_ListView"
                            || View.Id == "Distribution_ListView_Viewmode" || View.Id == "Distribution_ListView_ReceiveView" || View.Id == "Requisition_ListView_Receive")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["ItemName"] != null)
                            {
                                gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (View.Id == "Distribution_ListView_ConsumptionViewmode")
                            {
                                if (editor.Grid.Columns["ConsumptionDate"] != null)
                                {
                                    editor.Grid.Columns["ConsumptionDate"].Width = 150;
                                }
                                if (editor.Grid.Columns["ConsumptionBy"] != null)
                                {
                                    editor.Grid.Columns["ConsumptionBy"].Width = 150;
                                }
                            }
                        }
                        else if (View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["ItemName"] != null && editor.Grid.Columns["POReport"] != null)
                            {
                                gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;

                            }
                        }




                        else if (View.Id == "VendorReagentCertificate_DetailView")
                        {
                            if (editor.Grid.Columns["StockAmount"] != null)
                            {
                                editor.Grid.Columns["StockAmount"].Width = 150;
                            }
                            if (editor.Grid.Columns["ItemReceivedSort"] != null)
                            {
                                editor.Grid.Columns["ItemReceivedSort"].Width = 150;
                            }
                            if (editor.Grid.Columns["DaysReamining"] != null)
                            {
                                editor.Grid.Columns["DaysReamining"].Width = 150;
                            }
                        }
                        else if (View.Id == "VendorReagentCertificate_LT_ListView")
                        {
                            if (editor.Grid.Columns["OriginalAmount"] != null)
                            {
                                editor.Grid.Columns["OriginalAmount"].Width = 150;
                            }

                            if (editor.Grid.Columns["Stock Amount"] != null)
                            {
                                editor.Grid.Columns["Stock Amount"].Width = 150;
                            }
                            if (editor.Grid.Columns["ItemReceivedSort"] != null)
                            {
                                editor.Grid.Columns["ItemReceivedSort"].Width = 140;
                            }
                            if (editor.Grid.Columns["DaysReamining"] != null)
                            {
                                editor.Grid.Columns["DaysReamining"].Width = 150;
                            }
                            if (editor.Grid.Columns["ItemName"] != null)
                            {
                                editor.Grid.Columns["ItemName"].Width = 150;
                            }
                            if (editor.Grid.Columns["DisposedDate"] != null)
                            {
                                editor.Grid.Columns["DisposedDate"].Width = 130;
                            }
                            if (editor.Grid.Columns["AmountTaken"] != null)
                            {
                                editor.Grid.Columns["AmountTaken"].Width = 130;
                            }
                            if (editor.Grid.Columns["AmountUnits"] != null)
                            {
                                editor.Grid.Columns["AmountUnits"].Width = 130;
                            }
                        }





                        else if (View.Id == "SampleBottleAllocation_ListView_Taskregistration")
                        {
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                editor.Grid.Columns["Edit"].Visible = false;
                            }
                            if (editor.Grid.Columns["BottleSet"] != null)
                            {
                                editor.Grid.Columns["BottleSet"].Width = 100;
                            }
                            if (editor.Grid.Columns["SharedTests"] != null)
                            {
                                editor.Grid.Columns["SharedTests"].Width = 260;
                            }
                            if (editor.Grid.Columns["Qty"] != null)
                            {
                                editor.Grid.Columns["Qty"].Width = 80;
                            }
                            if (editor.Grid.Columns["BottleID"] != null)
                            {
                                editor.Grid.Columns["BottleID"].Width = 100;
                            }
                            if (editor.Grid.Columns["Containers"] != null)
                            {
                                editor.Grid.Columns["Containers"].Width = 150;
                            }
                            if (editor.Grid.Columns["Preservative"] != null)
                            {
                                editor.Grid.Columns["Preservative"].Width = 150;
                            }
                        }
                        else if (View.Id == "SampleBottleAllocation_ListView_Sampleregistration")
                        {
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                editor.Grid.Columns["Edit"].Visible = false;
                            }
                            if (editor.Grid.Columns["BottleSet"] != null)
                            {
                                editor.Grid.Columns["BottleSet"].Width = 100;
                            }
                            if (editor.Grid.Columns["SharedTests"] != null)
                            {
                                editor.Grid.Columns["SharedTests"].Width = 260;
                            }
                            if (editor.Grid.Columns["Qty"] != null)
                            {
                                editor.Grid.Columns["Qty"].Width = 80;
                            }
                            if (editor.Grid.Columns["BottleID"] != null)
                            {
                                editor.Grid.Columns["BottleID"].Width = 100;
                            }
                            if (editor.Grid.Columns["Containers"] != null)
                            {
                                editor.Grid.Columns["Containers"].Width = 150;
                            }
                            if (editor.Grid.Columns["Preservative"] != null)
                            {
                                editor.Grid.Columns["Preservative"].Width = 150;
                            }
                        }
                        else if (View.Id == "SampleBottleAllocation_ListView_COCSettings")
                        {
                            if (editor.Grid.Columns["Edit"] != null)
                            {
                                editor.Grid.Columns["Edit"].Visible = false;
                            }
                            if (editor.Grid.Columns["BottleSet"] != null)
                            {
                                editor.Grid.Columns["BottleSet"].Width = 100;
                            }
                            if (editor.Grid.Columns["SharedTests"] != null)
                            {
                                editor.Grid.Columns["SharedTests"].Width = 260;
                            }
                            if (editor.Grid.Columns["Qty"] != null)
                            {
                                editor.Grid.Columns["Qty"].Width = 80;
                            }
                            if (editor.Grid.Columns["BottleID"] != null)
                            {
                                editor.Grid.Columns["BottleID"].Width = 100;
                            }
                            if (editor.Grid.Columns["Containers"] != null)
                            {
                                editor.Grid.Columns["Containers"].Width = 150;
                            }
                            if (editor.Grid.Columns["Preservative"] != null)
                            {
                                editor.Grid.Columns["Preservative"].Width = 150;
                            }
                        }
                        else if (View.Id == "Requisition_ListView_Tracking")
                        {


                            if (editor.Grid.Columns["ItemName"] != null)
                            {
                                editor.Grid.Columns["ItemName"].Width = 200;
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = 200;
                            }
                            if (editor.Grid.Columns["DeliveryPriority"] != null)
                            {
                                editor.Grid.Columns["DeliveryPriority"].Width = 150;
                            }
                            //if (editor.Grid.Columns["Catalog#"] != null)
                            //{
                            //    editor.Grid.Columns["Catalog#"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["Vendor"] != null)
                            //{
                            //    editor.Grid.Columns["Vendor"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["OrderQty"] != null)
                            //{
                            //    editor.Grid.Columns["OrderQty"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["StockQty"] != null)
                            //{
                            //    editor.Grid.Columns["StockQty"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["PackUnits"] != null)
                            //{
                            //    editor.Grid.Columns["PackUnits"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["Delivery Priority"] != null)
                            //{
                            //    editor.Grid.Columns["Delivery Priority"].Width = 150;
                            //}
                            //if (editor.Grid.Columns["Department"] != null)
                            //{
                            //    editor.Grid.Columns["Department"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["RequestedBy"] != null)
                            //{
                            //    editor.Grid.Columns["RequestedBy"].Width = 130;
                            //}
                            //if (editor.Grid.Columns["Status"] != null)
                            //{
                            //    editor.Grid.Columns["Status"].Width = 100;
                            //}
                        }
                        else if (View.Id == "Requisition_ListView_History")
                        {

                            if (editor.Grid.Columns["ItemName"] != null)
                            {
                                editor.Grid.Columns["ItemName"].Width = 200;
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = 200;
                            }
                            if (editor.Grid.Columns["Catalog#"] != null)
                            {
                                editor.Grid.Columns["Catalog#"].Width = 100;
                            }
                            if (editor.Grid.Columns["Vendor"] != null)
                            {
                                editor.Grid.Columns["Vendor"].Width = 100;
                            }
                            if (editor.Grid.Columns["RQ#"] != null)
                            {
                                editor.Grid.Columns["RQ#"].Width = 100;
                            }
                            if (editor.Grid.Columns["RC#"] != null)
                            {
                                editor.Grid.Columns["RC#"].Width = 100;
                            }
                            if (editor.Grid.Columns["PO#"] != null)
                            {
                                editor.Grid.Columns["PO#"].Width = 100;
                            }
                            if (editor.Grid.Columns["LT#"] != null)
                            {
                                editor.Grid.Columns["LT#"].Width = 100;
                            }
                            if (editor.Grid.Columns["VendorLT"] != null)
                            {
                                editor.Grid.Columns["VendorLT"].Width = 100;
                            }
                            if (editor.Grid.Columns["ExpiryDate"] != null)
                            {
                                editor.Grid.Columns["ExpiryDate"].Width = 150;
                            }
                            if (editor.Grid.Columns["Units"] != null)
                            {
                                editor.Grid.Columns["Units"].Width = 100;
                            }
                            if (editor.Grid.Columns["Storage"] != null)
                            {
                                editor.Grid.Columns["Storage"].Width = 100;
                            }
                            if (editor.Grid.Columns["Department"] != null)
                            {
                                editor.Grid.Columns["Department"].Width = 100;
                            }
                            if (editor.Grid.Columns["RequestedBy"] != null)
                            {
                                editor.Grid.Columns["RequestedBy"].Width = 100;
                            }
                            if (editor.Grid.Columns["RequestedDate"] != null)
                            {
                                editor.Grid.Columns["RequestedDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["ReviewedDate"] != null)
                            {
                                editor.Grid.Columns["ReviewedDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["ReviewedBy"] != null)
                            {
                                editor.Grid.Columns["ReviewedBy"].Width = 100;
                            }
                            if (editor.Grid.Columns["PurchaseDate"] != null)
                            {
                                editor.Grid.Columns["PurchaseDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["PurchaseBy"] != null)
                            {
                                editor.Grid.Columns["PurchaseBy"].Width = 100;
                            }
                            if (editor.Grid.Columns["Receive Date"] != null)
                            {
                                editor.Grid.Columns["Receive Date"].Width = 120;
                            }
                            if (editor.Grid.Columns["Received By"] != null)
                            {
                                editor.Grid.Columns["Received By"].Width = 100;
                            }
                            if (editor.Grid.Columns["DistributionDate"] != null)
                            {
                                editor.Grid.Columns["DistributionDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["GivenBy"] != null)
                            {
                                editor.Grid.Columns["GivenBy"].Width = 100;
                            }
                            if (editor.Grid.Columns["Status"] != null)
                            {
                                editor.Grid.Columns["Status"].Width = 100;
                            }
                        }
                        else if (View.Id == "Distribution_ListView_LTsearch")
                        {
                            if (editor.Grid.Columns["LT#"] != null)
                            {
                                editor.Grid.Columns["LT#"].Width = 100;
                            }
                            if (editor.Grid.Columns["ItemName"] != null)
                            {
                                editor.Grid.Columns["ItemName"].Width = 200;
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = 200;
                            }
                            if (editor.Grid.Columns["Catalog#"] != null)
                            {
                                editor.Grid.Columns["Catalog#"].Width = 100;
                            }
                            if (editor.Grid.Columns["Vendor"] != null)
                            {
                                editor.Grid.Columns["Vendor"].Width = 100;
                            }
                            if (editor.Grid.Columns["Category"] != null)
                            {
                                editor.Grid.Columns["Category"].Width = 100;
                            }
                            if (editor.Grid.Columns["Grade"] != null)
                            {
                                editor.Grid.Columns["Grade"].Width = 100;
                            }
                            if (editor.Grid.Columns["Department"] != null)
                            {
                                editor.Grid.Columns["Department"].Width = 100;
                            }
                            if (editor.Grid.Columns["RequestedDate"] != null)
                            {
                                editor.Grid.Columns["RequestedDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["RequestedBy"] != null)
                            {
                                editor.Grid.Columns["RequestedBy"].Width = 100;
                            }
                            if (editor.Grid.Columns["ReviewedDate"] != null)
                            {
                                editor.Grid.Columns["ReviewedDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["ReviewedBy"] != null)
                            {
                                editor.Grid.Columns["ReviewedBy"].Width = 100;
                            }
                            if (editor.Grid.Columns["PuchaseDate"] != null)
                            {
                                editor.Grid.Columns["PuchaseDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["PurchaseBy"] != null)
                            {
                                editor.Grid.Columns["PurchaseBy"].Width = 100;
                            }
                            if (editor.Grid.Columns["Receive Date"] != null)
                            {
                                editor.Grid.Columns["Receive Date"].Width = 120;
                            }
                            if (editor.Grid.Columns["Received By"] != null)
                            {
                                editor.Grid.Columns["Received By"].Width = 100;
                            }
                            if (editor.Grid.Columns["DistributionDate"] != null)
                            {
                                editor.Grid.Columns["DistributionDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["DistributedBy"] != null)
                            {
                                editor.Grid.Columns["DistributedBy"].Width = 100;
                            }
                            if (editor.Grid.Columns["Consumption Date"] != null)
                            {
                                editor.Grid.Columns["Consumption Date"].Width = 130;
                            }
                            if (editor.Grid.Columns["Consumption By"] != null)
                            {
                                editor.Grid.Columns["Consumption By"].Width = 120;
                            }
                            if (editor.Grid.Columns["Disposed Date"] != null)
                            {
                                editor.Grid.Columns["Disposed Date"].Width = 120;
                            }
                            if (editor.Grid.Columns["Disposed By"] != null)
                            {
                                editor.Grid.Columns["Disposed By"].Width = 100;
                            }
                            if (editor.Grid.Columns["RQ#"] != null)
                            {
                                editor.Grid.Columns["RQ#"].Width = 100;
                            }
                            if (editor.Grid.Columns["PO#"] != null)
                            {
                                editor.Grid.Columns["PO#"].Width = 100;
                            }
                            if (editor.Grid.Columns["RC#"] != null)
                            {
                                editor.Grid.Columns["RC#"].Width = 100;
                            }
                            if (editor.Grid.Columns["VedorLT"] != null)
                            {
                                editor.Grid.Columns["VedorLT"].Width = 100;
                            }
                            if (editor.Grid.Columns["ExpirationDate"] != null)
                            {
                                editor.Grid.Columns["ExpirationDate"].Width = 120;
                            }
                            if (editor.Grid.Columns["PackUnits"] != null)
                            {
                                editor.Grid.Columns["PackUnits"].Width = 100;
                            }
                            if (editor.Grid.Columns["UnitPrice"] != null)
                            {
                                editor.Grid.Columns["UnitPrice"].Width = 100;
                            }
                        }
                        else if (View.Id == "Requisition_ListView_Cancelled")
                        {


                            if (editor.Grid.Columns["ItemName"] != null)
                            {
                                editor.Grid.Columns["ItemName"].Width = 200;
                            }
                            if (editor.Grid.Columns["Description"] != null)
                            {
                                editor.Grid.Columns["Description"].Width = 200;
                            }
                            if (editor.Grid.Columns["Delivery Priority"] != null)
                            {
                                editor.Grid.Columns["Delivery Priority"].Width = 150;
                            }
                            //if (editor.Grid.Columns["Catalog#"] != null)
                            //{
                            //    editor.Grid.Columns["Catalog#"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["Vendor"] != null)
                            //{
                            //    editor.Grid.Columns["Vendor"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["OrderQty"] != null)
                            //{
                            //    editor.Grid.Columns["OrderQty"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["PackUnits"] != null)
                            //{
                            //    editor.Grid.Columns["PackUnits"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["Delivery Priority"] != null)
                            //{
                            //    editor.Grid.Columns["Delivery Priority"].Width =150;
                            //}
                            //if (editor.Grid.Columns["Department"] != null)
                            //{
                            //    editor.Grid.Columns["Department"].Width = 100;
                            //}
                            //if (editor.Grid.Columns["RequestedBy"] != null)
                            //{
                            //    editor.Grid.Columns["RequestedBy"].Width = 130;
                            //}
                            //if (editor.Grid.Columns["CanceledDate"] != null)
                            //{
                            //    editor.Grid.Columns["CanceledDate"].Width = 130;
                            //}
                            //if (editor.Grid.Columns["CanceledBy"] != null)
                            //{
                            //    editor.Grid.Columns["CanceledBy"].Width = 130;
                            //}
                        }
                        else if (View.Id == "Requisition_ListView_History")
                        {
                            if (editor.Grid.Columns["RequestedDate"] != null && editor.Grid.Columns["ItemName"] != null)
                            {
                                gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;

                            }
                        }
                        else if (View.Id == "Distribution_ListView_LTsearch")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["ItemName"] != null && editor.Grid.Columns["LT"] != null)
                            {
                                gridView.VisibleColumns[0].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[1].FixedStyle = GridViewColumnFixedStyle.Left;
                                gridView.VisibleColumns[2].FixedStyle = GridViewColumnFixedStyle.Left;

                            }
                        }
                        else if (View.Id == "VendorReagentCertificate_ListView")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["Edit"] != null && editor.Grid.Columns["ItemName"] != null)
                            {
                                int intwidth = Convert.ToInt32(gridView.Width.Value);
                                editor.Grid.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                editor.Grid.Columns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                                editor.Grid.Columns["ItemName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (View.Id == "Testparameter_ListView_Test_SampleParameter" || View.Id == "Testparameter_ListView_Test_InternalStandards" || View.Id == "Testparameter_ListView_Test_QCSampleParameter" || View.Id == "Testparameter_ListView_Test_Surrogates")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                editor.Grid.Columns["Parameter"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault")
                        {
                            if (editor.Grid.Columns["SelectionCommandColumn"] != null && editor.Grid.Columns["Edit"] != null && editor.Grid.Columns["QCType.QCTypeName"] != null && editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["SelectionCommandColumn"].FixedStyle = GridViewColumnFixedStyle.Left;
                                editor.Grid.Columns["Edit"].FixedStyle = GridViewColumnFixedStyle.Left;
                                editor.Grid.Columns["QCType.QCTypeName"].FixedStyle = GridViewColumnFixedStyle.Left;
                                editor.Grid.Columns["Parameter"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (View.Id == "Testparameter_ListView_Test_Surrogates_DataCenter")
                        {
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 180;
                                editor.Grid.Columns["Parameter"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 140;
                                editor.Grid.Columns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].Width = 140;
                                editor.Grid.Columns["Method"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SpikeAmountUnits"] != null)
                            {
                                editor.Grid.Columns["SpikeAmountUnits"].Width = 180;
                            }
                            if (editor.Grid.Columns["SurrogateAmount"] != null)
                            {
                                editor.Grid.Columns["SurrogateSpikeAmount"].Width = 180;
                            }
                            if (editor.Grid.Columns["SurrogateSpikeAmount"] != null)
                            {
                                editor.Grid.Columns["SurrogateLCLimit"].Width = 140;
                            }
                            if (editor.Grid.Columns["SurrogateUCLimit"] != null)
                            {
                                editor.Grid.Columns["SurrogateUCLimit"].Width = 140;
                            }

                        }
                        else if (View.Id == "Testparameter_ListView_Test_SampleParameter_DataCenter")
                        {
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 180;
                                editor.Grid.Columns["Parameter"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 140;
                                editor.Grid.Columns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].Width = 140;
                                editor.Grid.Columns["Method"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["FinalDefaultResult"] != null)
                            {
                                editor.Grid.Columns["FinalDefaultResult"].Width = 180;
                            }
                            if (editor.Grid.Columns["FinalDefaultUnits"] != null)
                            {
                                editor.Grid.Columns["FinalDefaultUnits"].Width = 180;
                            }
                            if (editor.Grid.Columns["RegulatoryLimit"] != null)
                            {
                                editor.Grid.Columns["RegulatoryLimit"].Width = 180;
                            }

                        }
                        else if (View.Id == "Testparameter_ListView_Test_QCSampleParameter_QCParameterDefault_DataCenter")
                        {
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 140;
                                editor.Grid.Columns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].Width = 140;
                                editor.Grid.Columns["Method"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["QCType"] != null)
                            {
                                editor.Grid.Columns["QCType"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 180;
                                editor.Grid.Columns["Parameter"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SpikeAmountUnits"] != null)
                            {
                                editor.Grid.Columns["SpikeAmountUnits"].Width = 180;
                            }
                        }
                        else if (View.Id == "Testparameter_ListView_Test_InternalStandards_TestISTD_DataCenter")
                        {
                            if (editor.Grid.Columns["Parameter"] != null)
                            {
                                editor.Grid.Columns["Parameter"].Width = 180;
                                editor.Grid.Columns["Parameter"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Matrix"] != null)
                            {
                                editor.Grid.Columns["Matrix"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["SpikeAmountUnits"] != null)
                            {
                                editor.Grid.Columns["SpikeAmountUnits"].Width = 160;
                            }
                            if (editor.Grid.Columns["Test"] != null)
                            {
                                editor.Grid.Columns["Test"].Width = 130;
                            }
                            if (editor.Grid.Columns["Method"] != null)
                            {
                                editor.Grid.Columns["Method"].Width = 130;
                            }

                        }
                        else if (View.Id == "Employee_ListView")
                        {
                            if (editor.Grid.Columns["FullName"] != null)
                            {
                                editor.Grid.Columns["FullName"].Width = Unit.Percentage(17);
                            }
                            if (editor.Grid.Columns["UserName"] != null)
                            {
                                editor.Grid.Columns["UserName"].Width = Unit.Percentage(17);
                            }
                            if (editor.Grid.Columns["Initial"] != null)
                            {
                                editor.Grid.Columns["Initial"].Width = Unit.Percentage(16);
                            }
                            if (editor.Grid.Columns["JobTitle"] != null)
                            {
                                editor.Grid.Columns["JobTitle"].Width = Unit.Percentage(16);
                            }
                            if (editor.Grid.Columns["Department"] != null)
                            {
                                editor.Grid.Columns["Department"].Width = Unit.Percentage(16);
                            }
                            if (editor.Grid.Columns["RoleNames"] != null)
                            {
                                editor.Grid.Columns["RoleNames"].Width = Unit.Percentage(18);
                            }
                        }
                        else if (View.Id == "Employee_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["FullName"] != null)
                            {
                                editor.Grid.Columns["FullName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                            if (editor.Grid.Columns["UserName"] != null)
                            {
                                editor.Grid.Columns["UserName"].FixedStyle = GridViewColumnFixedStyle.Left;
                            }
                        }
                        else if (View.Id == "TestMethod_PrepMethods_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["PrepMethod"] != null)
                            {
                                editor.Grid.Columns["PrepMethod"].Width = 250;
                            }

                        }
                        else if (View.Id == "Vendors_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["Vendor"] != null)
                            {

                                editor.Grid.Columns["Vendor"].Width = 150;
                                editor.Grid.Columns["Vendor"].FixedStyle = GridViewColumnFixedStyle.Left;

                            }
                            if (editor.Grid.Columns["Account"] != null)
                            {
                                editor.Grid.Columns["Account"].Width = 200;
                            }
                            if (editor.Grid.Columns["Address2"] != null)
                            {
                                editor.Grid.Columns["Address2"].Width = 250;
                            }
                            if (editor.Grid.Columns["Country"] != null)
                            {
                                editor.Grid.Columns["Country"].Width = 170;
                            }
                            if (editor.Grid.Columns["Website"] != null)
                            {
                                editor.Grid.Columns["Website"].Width = 250;
                            }
                            if (editor.Grid.Columns["Email"] != null)
                            {
                                editor.Grid.Columns["Email"].Width = 250;
                            }
                            if (editor.Grid.Columns["Phone"] != null)
                            {
                                editor.Grid.Columns["Phone"].Width = 180;
                            }
                            if (editor.Grid.Columns["Address1"] != null)
                            {
                                editor.Grid.Columns["Address1"].Width = 250;
                            }
                        }
                        //else if (View.Id == "MaintenanceTaskCheckList_ListView_MaintenanceQueue")
                        //{
                        //    if (editor.Grid.Columns["Skip"] != null)
                        //    {
                        //        editor.Grid.Columns["Skip"].Width = Unit.Percentage(5);
                        //    }
                        //    if (editor.Grid.Columns["NextMaintainDate"] != null)
                        //    {
                        //        editor.Grid.Columns["NextMaintainDate"].Width = Unit.Percentage(15);
                        //    }
                        //    if (editor.Grid.Columns["MaintainedDate"] != null)
                        //    {
                        //        editor.Grid.Columns["MaintainedDate"].Width = Unit.Percentage(15);
                        //    }
                        //    if (editor.Grid.Columns["TaskToBeDone"] != null)
                        //    {
                        //        editor.Grid.Columns["TaskToBeDone"].Width = Unit.Percentage(15);
                        //    }
                        //    if (editor.Grid.Columns["InstrumentName"] != null)
                        //    {
                        //        editor.Grid.Columns["InstrumentName"].Width = Unit.Percentage(15);
                        //    }
                        //    if (editor.Grid.Columns["ActionDescription"] != null)
                        //    {
                        //        editor.Grid.Columns["ActionDescription"].Width = Unit.Percentage(15);
                        //    }
                        //}
                        else if (View.Id == "HoldingTimes_ListView_DataCenter")
                        {
                            if (editor.Grid.Columns["HourValueConversion"] != null)
                            {

                                editor.Grid.Columns["HourValueConversion"].CellStyle.HorizontalAlign = HorizontalAlign.Left;

                        }
                    }
                    if (View.Id == "SpreadSheetEntry_AnalyticalBatch_ListView")
                    {
                        if (editor.Grid.Columns["AnalyticalBatchID"] != null)
                        {
                            editor.Grid.Columns["AnalyticalBatchID"].Width = Unit.Percentage(20);
                        }
                        if (editor.Grid.Columns["Test"] != null)
                        {
                            editor.Grid.Columns["Test"].Width = Unit.Percentage(20);
                        }
                        if (editor.Grid.Columns["Method"] != null)
                        {
                            editor.Grid.Columns["Method"].Width = Unit.Percentage(20);
                        }
                        if (editor.Grid.Columns["Matrix"] != null)
                        {
                            editor.Grid.Columns["Matrix"].Width = Unit.Percentage(20);
                        }
                        if (editor.Grid.Columns["NonStatus"] != null)
                        {
                            editor.Grid.Columns["NonStatus"].Width = Unit.Percentage(20);
                        }
                    }
                    if (View.Id == "TestMethod_ListView_AnalysisQueue")
                    {
                        if (editor.Grid.Columns["TestName"] != null)
                        {
                            editor.Grid.Columns["TestName"].Width = Unit.Percentage(25);
                        }
                        if (editor.Grid.Columns["NoSamples"] != null)
                        {
                            editor.Grid.Columns["NoSamples"].Width = Unit.Percentage(15);
                        }
                        if (editor.Grid.Columns["Method"] != null)
                        {
                            editor.Grid.Columns["Method"].Width = Unit.Percentage(25);
                        }
                        if (editor.Grid.Columns["Matrix"] != null)
                        {
                            editor.Grid.Columns["Matrix"].Width = Unit.Percentage(15);
                        }
                        if (editor.Grid.Columns["Template"] != null)
                        {
                            editor.Grid.Columns["Template"].Width = Unit.Percentage(25);
                        }
                    }
                    if (View.Id == "PTStudyLog_ListView" || View.Id == "PTStudyLog_ListView_Performance")
                    {
                        if (editor.Grid.Columns["JobID"] != null)
                        {
                            editor.Grid.Columns["JobID"].Width = Unit.Percentage(20);
                        }
                        if (editor.Grid.Columns["Study Name"] != null)
                        {
                            editor.Grid.Columns["Study Name"].Width = Unit.Percentage(25);
                        }
                        if (editor.Grid.Columns["Study ID"] != null)
                        {
                            editor.Grid.Columns["Study ID"].Width = Unit.Percentage(20);
                        }
                        if (editor.Grid.Columns["Category"] != null)
                        {
                            editor.Grid.Columns["Category"].Width = Unit.Percentage(15);
                        }
                        if (editor.Grid.Columns["Description"] != null)
                        {
                            editor.Grid.Columns["Description"].Width = Unit.Percentage(25);
                        }
                        if (editor.Grid.Columns["DatePTSampleReceived"] != null)
                        {
                            editor.Grid.Columns["DatePTSampleReceived"].Width = Unit.Percentage(28);
                        }
                        if (editor.Grid.Columns["DateLabResultSubmitted"] != null)
                        {
                            editor.Grid.Columns["DateLabResultSubmitted"].Width = Unit.Percentage(30);
                        }
                        if (editor.Grid.Columns["DatePTResultReceived"] != null)
                        {
                            editor.Grid.Columns["DatePTResultReceived"].Width = Unit.Percentage(25);
                        }
                        if (editor.Grid.Columns["Status"] != null)
                        {
                            editor.Grid.Columns["Status"].Width = Unit.Percentage(20);
                        }
                    }
                    else if (View.Id == "SampleSites_ListView")
                    {
                        if (editor.Grid.Columns["Site ID"] != null)
                        {
                            editor.Grid.Columns["Site ID"].Width = 200;
                        }
                        if (editor.Grid.Columns["Site Name"] != null)
                        {
                            editor.Grid.Columns["Site Name"].Width = 200;
                        }
                        if (editor.Grid.Columns["Site Code"] != null)
                        {
                            editor.Grid.Columns["Site Code"].Width = 200;
                        }
                        if (editor.Grid.Columns["Client"] != null)
                        {
                            editor.Grid.Columns["Client"].Width = 200;
                        }
                        if (editor.Grid.Columns["Address"] != null)
                        {
                            editor.Grid.Columns["Address"].Width = 200;
                        }
                        if (editor.Grid.Columns["ProjectID"] != null)
                        {
                            editor.Grid.Columns["ProjectID"].Width = 200;
                        }
                        if (editor.Grid.Columns["Sample Point ID"] != null)
                        {
                            editor.Grid.Columns["Sample Point ID"].Width = 200;
                        }
                        if (editor.Grid.Columns["Blended"] != null)
                        {
                            editor.Grid.Columns["Blended"].Width = 200;
                        }
                        if (editor.Grid.Columns["Project Name"] != null)
                        {
                            editor.Grid.Columns["Project Name"].Width = 200;
                        }
                        if (editor.Grid.Columns["PWSSystem Name"] != null)
                        {
                            editor.Grid.Columns["PWSSystem Name"].Width = 200;
                        }
                        if (editor.Grid.Columns["Sampling Address"] != null)
                        {
                            editor.Grid.Columns["Sampling Address"].Width = 200;
                        }
                        if (editor.Grid.Columns["Station Location"] != null)
                        {
                            editor.Grid.Columns["Station Location"].Width = 200;
                        }
                        if (editor.Grid.Columns["System Type"] != null)
                        {
                            editor.Grid.Columns["System Type"].Width = 200;
                        }
                        if (editor.Grid.Columns["Depth"] != null)
                        {
                            editor.Grid.Columns["Depth"].Width = 200;
                        }
                        if (editor.Grid.Columns["SampleID"] != null)
                        {
                            editor.Grid.Columns["SampleID"].Width = 200;
                        }
                        if (editor.Grid.Columns["Sample No"] != null)
                        {
                            editor.Grid.Columns["Sample No"].Width = 200;
                        }
                        if (editor.Grid.Columns["Latitude"] != null)
                        {
                            editor.Grid.Columns["Latitude"].Width = 200;
                        }
                        if (editor.Grid.Columns["Langitude"] != null)
                        {
                            editor.Grid.Columns["Langitude"].Width = 200;
                        }
                        if (editor.Grid.Columns["Sample Matrix"] != null)
                        {
                            editor.Grid.Columns["Sample Matrix"].Width = 200;
                        }
                        if (editor.Grid.Columns["Matrix"] != null)
                        {
                            editor.Grid.Columns["Matrix"].Width = 200;
                        }
                        if (editor.Grid.Columns["Key Map"] != null)
                        {
                            editor.Grid.Columns["Key Map"].Width = 200;
                        }
                        if (editor.Grid.Columns["Qty"] != null)
                        {
                            editor.Grid.Columns["Qty"].Width = 200;
                        }
                        if (editor.Grid.Columns["PWSID"] != null)
                        {
                            editor.Grid.Columns["PWSID"].Width = 200;
                        }
                        if (editor.Grid.Columns["Service Area"] != null)
                        {
                            editor.Grid.Columns["Service Area"].Width = 200;
                        }
                        if (editor.Grid.Columns["Description"] != null)
                        {
                            editor.Grid.Columns["Description"].Width = 200;
                        }
                        if (editor.Grid.Columns["Collector"] != null)
                        {
                            editor.Grid.Columns["Collector"].Width = 200;
                        }
                        if (editor.Grid.Columns["Collector Phone"] != null)
                        {
                            editor.Grid.Columns["Collector Phone"].Width = 200;
                        }
                        if (editor.Grid.Columns["IsActive"] != null)
                        {
                            editor.Grid.Columns["IsActive"].Width = 200;
                        }
                        if (editor.Grid.Columns["Site Name Archived"] != null)
                        {
                            editor.Grid.Columns["Site Name Archived"].Width = 200;
                        }
                        if (editor.Grid.Columns["Monitorying Requirement"] != null)
                        {
                            editor.Grid.Columns["Monitorying Requirement"].Width = 200;
                        }
                        if (editor.Grid.Columns["Facility ID"] != null)
                        {
                            editor.Grid.Columns["Facility ID"].Width = 200;
                        }
                        if (editor.Grid.Columns["Facility Name"] != null)
                        {
                            editor.Grid.Columns["Facility Name"].Width = 200;
                        }
                        if (editor.Grid.Columns["Facility Type"] != null)
                        {
                            editor.Grid.Columns["Facility Type"].Width = 200;
                        }
                        if (editor.Grid.Columns["Sample Point Type"] != null)
                        {
                            editor.Grid.Columns["Sample Point Type"].Width = 200;
                        }
                        if (editor.Grid.Columns["Water Type"] != null)
                        {
                            editor.Grid.Columns["Water Type"].Width = 200;
                        }
                        if (editor.Grid.Columns["ParentSampleID"] != null)
                        {
                            editor.Grid.Columns["ParentSampleID"].Width = 200;
                        }
                        if (editor.Grid.Columns["Parent Sample Date"] != null)
                        {
                            editor.Grid.Columns["Parent Sample Date"].Width = 200;
                        }
                        if (editor.Grid.Columns["RejectionCriteria#"] != null)
                        {
                            editor.Grid.Columns["RejectionCriteria#"].Width = 200;
                        }
                        if (editor.Grid.Columns["Repeate Location"] != null)
                        {
                            editor.Grid.Columns["Repeate Location"].Width = 200;
                        }
                        if (editor.Grid.Columns["Entered Date"] != null)
                        {
                            editor.Grid.Columns["Entered Date"].Width = 200;
                        }
                        if (editor.Grid.Columns["Entered By"] != null)
                        {
                            editor.Grid.Columns["Entered By"].Width = 200;
                        }
                        if (editor.Grid.Columns["Last Updated Date"] != null)
                        {
                            editor.Grid.Columns["Last Updated Date"].Width = 200;
                        }
                        if (editor.Grid.Columns["Last Updated By"] != null)
                        {
                            editor.Grid.Columns["Last Updated By"].Width = 200;
                        }
                        if (editor.Grid.Columns["ModifiedDate"] != null)
                        {
                            editor.Grid.Columns["ModifiedDate"].Width = 200;
                        }
                        if (editor.Grid.Columns["ModifiedBy"] != null)
                        {
                            editor.Grid.Columns["ModifiedBy"].Width = 200;
                        }
                        if (editor.Grid.Columns["City"] != null)
                        {
                            editor.Grid.Columns["City"].Width = 200;
                        }
                        if (editor.Grid.Columns["State"] != null)
                        {
                            editor.Grid.Columns["State"].Width = 200;
                        }
                        if (editor.Grid.Columns["ZipCode"] != null)
                        {
                            editor.Grid.Columns["ZipCode"].Width = 200;
                        }
                        if (editor.Grid.Columns["SiteUserDefinedColumn1"] != null)
                        {
                            editor.Grid.Columns["SiteUserDefinedColumn1"].Width = 300;
                        }
                        if (editor.Grid.Columns["SiteUserDefinedColumn2"] != null)
                        {
                            editor.Grid.Columns["SiteUserDefinedColumn2"].Width = 300;
                        }
                        if (editor.Grid.Columns["SiteUserDefinedColumn3"] != null)
                        {
                            editor.Grid.Columns["SiteUserDefinedColumn3"].Width = 300;
                        }
                    }
                    else if (View.Id == "Samplecheckin_ListView_BatchReporting")
                    {
                        if (editor.Grid.Columns["BatchPreview"] != null)
                        {
                            editor.Grid.Columns["BatchPreview"].Caption = "Preview";
                            editor.Grid.Columns["BatchPreview"].VisibleIndex = 10;
                        }
                    }
                    else if (View.Id == "ItemChargePricing_ListView" || View.Id == "ItemChargePricing_ListView_Invoice_Popup" || View.Id == "ItemChargePricing_ListView_Quotes_Popup")
                    {
                        if (editor.Grid.Columns["ItemCode"] != null)
                        {
                            editor.Grid.Columns["ItemCode"].Width = 120;
                        }
                        if (editor.Grid.Columns["ItemName"] != null)
                        {
                            editor.Grid.Columns["ItemName"].Width = Unit.Percentage(50);
                        }
                        if (editor.Grid.Columns["Category"] != null)
                        {
                            editor.Grid.Columns["Category"].Width = Unit.Percentage(50);
                        }
                        if (editor.Grid.Columns["Units"] != null)
                        {
                            editor.Grid.Columns["Units"].Width = 120;
                        }
                        if (editor.Grid.Columns["UnitPrice"] != null)
                        {
                            editor.Grid.Columns["UnitPrice"].Width = 120;
                        }
                    }
                    if (View.Id == "QualifierAutomation_ListView")
                    {
                        if (editor.Grid.Columns["QualifierID"] != null)
                        {
                            editor.Grid.Columns["QualifierID"].Width = 100;
                        }
                        if (editor.Grid.Columns["Symbol"] != null)
                        {
                            editor.Grid.Columns["Symbol"].Width = 80;
                        }
                        if (editor.Grid.Columns["Type"] != null)
                        {
                            editor.Grid.Columns["Type"].Width = 80;
                        }
                        if (editor.Grid.Columns["Formula"] != null)
                        {
                            editor.Grid.Columns["Formula"].Width = Unit.Percentage(30);
                        }
                        if (editor.Grid.Columns["Matrix"] != null)
                        {
                            editor.Grid.Columns["Matrix"].Width = 100;
                        }
                        if (editor.Grid.Columns["Test"] != null)
                        {
                            editor.Grid.Columns["Test"].Width = 150;
                        }
                        if (editor.Grid.Columns["Method"] != null)
                        {
                            editor.Grid.Columns["Method"].Width = 150;
                        }
                        if (editor.Grid.Columns["Parameter"] != null)
                        {
                            editor.Grid.Columns["Parameter"].Width = Unit.Percentage(70);
                        }
                    }


                    #region comment section
                    //if (View.Id == "Purchaseorder_Item_ListView")
                    //{
                    //    if (editor.Grid.Columns["Item.items"] != null)
                    //    {
                    //        //editor.Grid.Columns["Item.items"].Width = System.Web.UI.WebControls.Unit.Percentage(25);
                    //        editor.Grid.Columns["Item.items"].Width = 250;
                    //    }
                    //    if (editor.Grid.Columns["Item.Specification"] != null)
                    //    {
                    //        //editor.Grid.Columns["Item.Specification"].Width = System.Web.UI.WebControls.Unit.Percentage(25);
                    //        editor.Grid.Columns["Item.Specification"].Width = 250;
                    //    }
                    //    if (editor.Grid.Columns["Catalog"] != null)
                    //    {
                    //        //editor.Grid.Columns["Catalog"].Width = System.Web.UI.WebControls.Unit.Percentage(10);
                    //        editor.Grid.Columns["Catalog"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["PackUnits"] != null)
                    //    {
                    //        //editor.Grid.Columns["PackUnits"].Width = System.Web.UI.WebControls.Unit.Percentage(10);
                    //        editor.Grid.Columns["PackUnits"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["OrderQty"] != null)
                    //    {
                    //        //editor.Grid.Columns["OrderQty"].Width = System.Web.UI.WebControls.Unit.Percentage(10);
                    //        editor.Grid.Columns["OrderQty"].Width = 80;
                    //    }
                    //    if (editor.Grid.Columns["UnitPrice"] != null)
                    //    {
                    //        //editor.Grid.Columns["UnitPrice"].Width = System.Web.UI.WebControls.Unit.Percentage(5);
                    //        editor.Grid.Columns["UnitPrice"].Width = 80;
                    //    }
                    //    if (editor.Grid.Columns["ExtPrice"] != null)
                    //    {
                    //        //editor.Grid.Columns["ExtPrice"].Width = System.Web.UI.WebControls.Unit.Percentage(5);
                    //        editor.Grid.Columns["ExtPrice"].Width = 80;
                    //    }
                    //    if (editor.Grid.Columns["department"] != null)
                    //    {
                    //        //editor.Grid.Columns["department"].Width = System.Web.UI.WebControls.Unit.Percentage(5);
                    //        editor.Grid.Columns["department"].Width = 80;
                    //    }
                    //    if (editor.Grid.Columns["Delivery Priority"] != null)
                    //    {
                    //        //editor.Grid.Columns["Delivery Priority"].Width = System.Web.UI.WebControls.Unit.Percentage(5);
                    //        editor.Grid.Columns["Delivery Priority"].Width = 80;
                    //    }
                    //}
                    //if (View.Id == "Requisition_ListView_Review" || View.Id == "Requisition_ListViewEntermode")
                    //{
                    //    if (editor.Grid.Columns["Item.items"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.items"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Item.Description"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.Description"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Item.Specification"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.Specification"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["NonPersistentStatus"] != null)
                    //    {
                    //        editor.Grid.Columns["NonPersistentStatus"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["DeliveryPriority"] != null)
                    //    {
                    //        editor.Grid.Columns["DeliveryPriority"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Catalog"] != null)
                    //    {
                    //        editor.Grid.Columns["Catalog"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Vendor"] != null)
                    //    {
                    //        editor.Grid.Columns["Vendor"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["ModifiedDate"] != null)
                    //    {
                    //        editor.Grid.Columns["ModifiedDate"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["ModifiedBy"] != null)
                    //    {
                    //        editor.Grid.Columns["ModifiedBy"].Width = 100;
                    //    }
                    //}
                    ////if (View.ObjectTypeInfo.Type == typeof(Items))
                    ////{
                    ////    if (editor.Grid.Columns["items"] != null)
                    ////    {
                    ////        editor.Grid.Columns["items"].Width = 200;
                    ////    }
                    ////    if (editor.Grid.Columns["Description"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Description"].Width = 200;
                    ////    }
                    ////    if (editor.Grid.Columns["Specification"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Specification"].Width = 200;
                    ////    }
                    ////}
                    ////if (View.Id == "Items_ListView_Copy_StockAlert")
                    ////{
                    ////    if (editor.Grid.Columns["Specification"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Specification"].Width = 200;
                    ////    }
                    ////}

                    if (View.Id == "Purchaseorder_Item_ListView")
                    {
                        if (editor.Grid.Columns["Item.items"] != null)
                        {
                            editor.Grid.Columns["Item.items"].Width = 200;
                        }
                        if (editor.Grid.Columns["Item.Specification"] != null)
                        {
                            editor.Grid.Columns["Item.Specification"].Width = 200;
                        }
                        if (editor.Grid.Columns["DeliveryPriority"] != null)
                        {
                            editor.Grid.Columns["DeliveryPriority"].Width = 160;
                        }
                    }

                    //if (View.Id == "Distribution_ListView_ReceiveView" || View.Id == "Distribution_ListView"
                    //    || View.Id == "Distribution_ListView_Disposal" || View.Id == "Distribution_ListView_LTsearch" || View.Id == "Requisition_ListView_Cancelled"
                    //    || View.Id == "Requisition_ListView_History" || View.Id == "Requisition_ListView_Tracking"
                    //    || View.Id == "Requisition_ListViewEntermode")

                    //{
                    //    if (editor.Grid.Columns["StockQty"] != null)
                    //    {
                    //        editor.Grid.Columns["StockQty"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["OrderQty"] != null)
                    //    {
                    //        editor.Grid.Columns["OrderQty"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["DeliveryPriority"] != null)
                    //    {
                    //        editor.Grid.Columns["DeliveryPriority"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["PackUnits"] != null)
                    //    {
                    //        editor.Grid.Columns["PackUnits"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Item.items"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.items"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Item.Specification"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.Specification"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Status"] != null)
                    //    {
                    //        editor.Grid.Columns["Status"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["NonPersistentStatus"] != null)
                    //    {
                    //        editor.Grid.Columns["NonPersistentStatus"].Width = 100;
                    //    }

                    //    if (editor.Grid.Columns["Catalog"] != null)
                    //    {
                    //        editor.Grid.Columns["Catalog"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Vendor"] != null)
                    //    {
                    //        editor.Grid.Columns["Vendor"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["ModifiedDate"] != null)
                    //    {
                    //        editor.Grid.Columns["ModifiedDate"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["ModifiedBy.UserName"] != null)
                    //    {
                    //        editor.Grid.Columns["ModifiedBy.UserName"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["DisposedDate"] != null)
                    //    {
                    //        editor.Grid.Columns["DisposedDate"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["ReceiveDate"] != null)
                    //    {
                    //        editor.Grid.Columns["ReceiveDate"].Width = 100;
                    //    }
                    //}

                    //View.Id == "Distribution_ListView_Disposal"{

                    //}





                    if (View.Id == "Distribution_ListView_Consumption")
                    {
                        if (editor.Grid.Columns["ExpiryDate"] != null)
                        {
                            editor.Grid.Columns["ExpiryDate"].Width = 100;
                        }
                        if (editor.Grid.Columns["DateOpened"] != null)
                        {
                            editor.Grid.Columns["DateOpened"].Width = 100;
                        }
                        if (editor.Grid.Columns["AlertQty"] != null)
                        {
                            editor.Grid.Columns["AlertQty"].Width = 100;
                        }
                        if (editor.Grid.Columns["StockQty"] != null)
                        {
                            editor.Grid.Columns["StockQty"].Width = 100;
                        }
                        if (editor.Grid.Columns["Amount"] != null)
                        {
                            editor.Grid.Columns["Amount"].Width = 100;
                        }
                        if (editor.Grid.Columns["Amount"] != null)
                        {
                            editor.Grid.Columns["Amount"].Width = 100;
                        }
                        if (editor.Grid.Columns["Parent"] != null)
                        {
                            editor.Grid.Columns["Parent"].Width = 150;
                        }
                        if (editor.Grid.Columns["Catalog#"] != null)
                        {
                            editor.Grid.Columns["Catalog#"].Width = 150;
                        }
                        if (editor.Grid.Columns["ItemCode"] != null)
                        {
                            editor.Grid.Columns["ItemCode"].Width = 100;
                        }
                        if (editor.Grid.Columns["ReceivedBy"] != null)
                        {
                            editor.Grid.Columns["ReceivedBy"].Width = 100;
                        }
                        if (editor.Grid.Columns["ReceiveDate"] != null)
                        {
                            editor.Grid.Columns["ReceiveDate"].Width = 100;
                        }
                        if (editor.Grid.Columns["DistributedBy"] != null)
                        {
                            editor.Grid.Columns["DistributedBy"].Width = 100;
                        }
                        if (editor.Grid.Columns["DisposedDate"] != null)
                        {
                            editor.Grid.Columns["DisposedDate"].Width = 100;
                        }
                        if (editor.Grid.Columns["Comment"] != null)
                        {
                            editor.Grid.Columns["Comment"].Width = 100;
                        }
                        if (editor.Grid.Columns["DepletedBy"] != null)
                        {
                            editor.Grid.Columns["DepletedBy"].Width = 100;
                        }
                        if (editor.Grid.Columns["IsDeplete"] != null)
                        {
                            editor.Grid.Columns["IsDeplete"].Width = 100;
                        }
                        if (editor.Grid.Columns["DateDepleted"] != null)
                        {
                            editor.Grid.Columns["DateDepleted"].Width = 100;
                        }
                        if (editor.Grid.Columns["AmountUnits"] != null)
                        {
                            editor.Grid.Columns["AmountUnits"].Width = 150;
                        }
                        if (editor.Grid.Columns["OriginalAmount"] != null)
                        {
                            editor.Grid.Columns["OriginalAmount"].Width = 100;
                        }
                        if (editor.Grid.Columns["Size"] != null)
                        {
                            editor.Grid.Columns["Size"].Width = 100;
                        }
                        if (editor.Grid.Columns["Description"] != null)
                        {
                            editor.Grid.Columns["Description"].Width = 100;
                        }
                        if (editor.Grid.Columns["LT"] != null)
                        {
                            editor.Grid.Columns["LT"].Width = 100;
                        }
                        if (editor.Grid.Columns["StockAmount"] != null)
                        {
                            editor.Grid.Columns["StockAmount"].Width = 150;
                        }
                        if (editor.Grid.Columns["ConsumptionDate"] != null)
                        {
                            editor.Grid.Columns["ConsumptionDate"].Width = 150;
                        }
                        if (editor.Grid.Columns["ConsumptionBy"] != null)
                        {
                            editor.Grid.Columns["ConsumptionBy"].Width = 150;
                        }
                        if (editor.Grid.Columns["StockQty"] != null)
                        {
                            editor.Grid.Columns["StockQty"].Width = 100;
                        }
                        if (editor.Grid.Columns["OrderQty"] != null)
                        {
                            editor.Grid.Columns["OrderQty"].Width = 100;
                        }
                        if (editor.Grid.Columns["DeliveryPriority"] != null)
                        {
                            editor.Grid.Columns["DeliveryPriority"].Width = 120;
                        }
                        if (editor.Grid.Columns["PackUnits"] != null)
                        {
                            editor.Grid.Columns["PackUnits"].Width = 100;
                        }
                        if (editor.Grid.Columns["Item.items"] != null)
                        {
                            editor.Grid.Columns["Item.items"].Width = 200;
                        }
                        if (editor.Grid.Columns["Item.Specification"] != null)
                        {
                            editor.Grid.Columns["Item.Specification"].Width = 200;
                        }
                        if (editor.Grid.Columns["Status"] != null)
                        {
                            editor.Grid.Columns["Status"].Width = 150;
                        }
                        if (editor.Grid.Columns["NonPersistentStatus"] != null)
                        {
                            editor.Grid.Columns["NonPersistentStatus"].Width = 150;
                        }
                        if (editor.Grid.Columns["Catalog"] != null)
                        {
                            editor.Grid.Columns["Catalog"].Width = 150;
                        }
                        if (editor.Grid.Columns["Vendor"] != null)
                        {
                            editor.Grid.Columns["Vendor"].Width = 150;
                        }
                        if (editor.Grid.Columns["Storage"] != null)
                        {
                            editor.Grid.Columns["Storage"].Width = 150;
                        }
                    }

                    if (View.Id == "Distribution_ListView_Fractional_Consumption")
                    {

                        if (editor.Grid.Columns["ConsumptionDate"] != null)
                        {
                            editor.Grid.Columns["ConsumptionDate"].Width = 140;
                        }
                        if (editor.Grid.Columns["Status"] != null)
                        {
                            editor.Grid.Columns["Status"].Width = 140;
                        }
                    }
                    //}
                    //    if (editor.Grid.Columns["StockQty"] != null)
                    //    {
                    //        editor.Grid.Columns["StockQty"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["OrderQty"] != null)
                    //    {
                    //        editor.Grid.Columns["OrderQty"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["DeliveryPriority"] != null)
                    //    {
                    //        editor.Grid.Columns["DeliveryPriority"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["PackUnits"] != null)
                    //    {
                    //        editor.Grid.Columns["PackUnits"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Item.items"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.items"].Width = 150;
                    //    }
                    //    if (editor.Grid.Columns["Item.Specification"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.Specification"].Width = 150;
                    //    }
                    //    if (editor.Grid.Columns["Status"] != null)
                    //    {
                    //        editor.Grid.Columns["Status"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["NonPersistentStatus"] != null)
                    //    {
                    //        editor.Grid.Columns["NonPersistentStatus"].Width = 100;
                    //    }

                    //    if (editor.Grid.Columns["Catalog"] != null)
                    //    {
                    //        editor.Grid.Columns["Catalog"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Vendor"] != null)
                    //    {
                    //        editor.Grid.Columns["Vendor"].Width = 100;
                    //    }
                    //    if (editor.Grid.Columns["Storage"] != null)
                    //    {
                    //        editor.Grid.Columns["Storage"].Width = 100;
                    //    }

                    //}

                    //if (View.Id == "Purchaseorder_Item_ListView")
                    //{
                    //    if (editor.Grid.Columns["DeliveryPriority"] != null)
                    //    {
                    //        editor.Grid.Columns["DeliveryPriority"].Width = 150;
                    //    }
                    //    if (editor.Grid.Columns["Catalog"] != null)
                    //    {
                    //        editor.Grid.Columns["Catalog"].Width = 150;
                    //    }
                    //}

                    //if (View.Id == "Distribution_ListView_LTsearch")
                    //{
                    //    if (editor.Grid.Columns["ExpiryDate"] != null)
                    //    {
                    //        editor.Grid.Columns["ExpiryDate"].Width = 150;
                    //    }
                    //    if (editor.Grid.Columns["RequestedDate"] != null)
                    //    {
                    //        editor.Grid.Columns["RequestedDate"].Width = 150;
                    //    }
                    //    if (editor.Grid.Columns["DistributionDate"] != null)
                    //    {
                    //        editor.Grid.Columns["DistributionDate"].Width = 150;
                    //    }
                    //}

                    if (View.Id == "Items_ListView_Copy_StockAlert" || View.Id == "Items_ListView_Copy_StockWatch")
                    {
                        if (editor.Grid.Columns["ItemName"] != null)
                        {
                            editor.Grid.Columns["ItemName"].Width = 200;
                        }
                        if (editor.Grid.Columns["Description"] != null)
                        {
                            editor.Grid.Columns["Description"].Width = 200;
                        }
                        if (editor.Grid.Columns["Vendor"] != null)
                        {
                            editor.Grid.Columns["Vendor"].Width = 150;
                        }
                    }

                    ////if (View.Id == "Items_ListView_Copy_StockWatch")
                    ////{
                    ////    if (editor.Grid.Columns["Item.items"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Item.items"].Width = 100;
                    ////    }
                    ////    if (editor.Grid.Columns["Item.Specification"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Item.Specification"].Width = 100;
                    ////    }
                    ////    //if (editor.Grid.Columns["VendorCatName"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["VendorCatName"].Width = 100;
                    ////    //}
                    ////    //if (editor.Grid.Columns["Vendor"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["Vendor"].Width = 100;
                    ////    //}
                    ////    //if (editor.Grid.Columns["Unit.Option"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["Unit.Option"].Width = 100;
                    ////    //}
                    ////    if (editor.Grid.Columns["Storage"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Storage"].Width = Unit.Pixel(150);
                    ////    }
                    ////    //if (editor.Grid.Columns["StockQty"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["StockQty"].Width = 100;
                    ////    //}
                    ////    //if (editor.Grid.Columns["ItemCode"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["ItemCode"].Width = 100;
                    ////    //}
                    ////}
                    //if (View.Id == "Requisition_ListView_Purchaseorder_Mainview")
                    //{
                    //    if (editor.Grid.Columns["Vendor"] != null)
                    //    {
                    //        editor.Grid.Columns["Vendor"].Width = 360;
                    //    }
                    //    if (editor.Grid.Columns["NumItems"] != null)
                    //    {
                    //        editor.Grid.Columns["NumItems"].Width = 360;
                    //    }
                    //    if (editor.Grid.Columns["TotalPrice"] != null)
                    //    {
                    //        editor.Grid.Columns["TotalPrice"].Width = 360;
                    //    }
                    //}
                    ////if (View.Id == "Purchaseorder_Item_ListView")
                    ////{

                    ////    if (editor.Grid.Columns["Order_Qty"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Order_Qty"].Width = 120;
                    ////    }
                    ////    if (editor.Grid.Columns["UnitPrice"] != null)
                    ////    {
                    ////        editor.Grid.Columns["UnitPrice"].Width = 120;
                    ////    }
                    ////    if (editor.Grid.Columns["ExpPrice"] != null)
                    ////    {
                    ////        editor.Grid.Columns["ExpPrice"].Width = 120;
                    ////    }
                    ////    if (editor.Grid.Columns["DeliveryPriority"] != null)
                    ////    {
                    ////        editor.Grid.Columns["DeliveryPriority"].Width = 120;
                    ////    }
                    ////    if (editor.Grid.Columns["department"] != null)
                    ////    {
                    ////        editor.Grid.Columns["department"].Width = 120;
                    ////    }
                    ////}
                    ////if (View.Id == "Requisition_ListView_Receive_MainReceive")
                    ////{
                    ////    if (editor.Grid.Columns["Vendor"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Vendor"].Width = 100;
                    ////    }
                    ////    if (editor.Grid.Columns["PO#"] != null)
                    ////    {
                    ////        editor.Grid.Columns["PO#"].Width = 100;
                    ////    }
                    ////    if (editor.Grid.Columns["NumItemCode"] != null)
                    ////    {
                    ////        editor.Grid.Columns["NumItemCode"].Width = 100;
                    ////    }
                    ////    if (editor.Grid.Columns["ExpectDate"] != null)
                    ////    {
                    ////        editor.Grid.Columns["ExpectDate"].Width = 100;
                    ////    }
                    ////    if (editor.Grid.Columns["TrackingNumber"] != null)
                    ////    {
                    ////        editor.Grid.Columns["TrackingNumber"].Width = 100;
                    ////    }
                    ////}
                    ////if (View.Id == "Distribution_ListView_MainDistribute")
                    ////{
                    ////    if (editor.Grid.Columns["ReceiveID"] != null)
                    ////    {
                    ////        editor.Grid.Columns["ReceiveID"].Width = 480;
                    ////    }
                    ////    if (editor.Grid.Columns["Vendor"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Vendor"].Width = 480;
                    ////    }
                    ////    if (editor.Grid.Columns["NumItems"] != null)
                    ////    {
                    ////        editor.Grid.Columns["NumItems"].Width = 480;
                    ////    }

                    ////}
                    ////if (View.Id == "Items_LookupListView")
                    ////{
                    ////    if (editor.Grid.Columns["Item.items"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Item.items"].Width = 200;
                    ////    }
                    ////    if (editor.Grid.Columns["Item.Specification"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Item.Specification"].Width = 200;
                    ////    }
                    ////    if (editor.Grid.Columns["Vendor"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Vendor"].Width = 200;
                    ////    }
                    ////    if (editor.Grid.Columns["Catalog#"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Catalog#"].Width = 200;
                    ////    }

                    ////}
                    ///

                    ////#region Requisition_ListView
                    ////if (View.Id == "Requisition_ListView")
                    ////{
                    ////    if (editor.Grid.Columns["Item.items"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Item.items"].Width = 200;
                    ////    }
                    ////    if (editor.Grid.Columns["Item.Description"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Item.Description"].Width = 200;
                    ////    }
                    ////    if (editor.Grid.Columns["Item.Specification"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Item.Specification"].Width = 200;
                    ////    }
                    ////    if (editor.Grid.Columns["NonPersistentStatus"] != null)
                    ////    {
                    ////        editor.Grid.Columns["NonPersistentStatus"].Width = 150;
                    ////    }
                    ////    if (editor.Grid.Columns["DeliveryPriority"] != null)
                    ////    {
                    ////        editor.Grid.Columns["DeliveryPriority"].Width = 150;
                    ////    }
                    ////    if (editor.Grid.Columns["Item.VendorCatName"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Item.VendorCatName"].Width = 150;
                    ////    }
                    ////    if (editor.Grid.Columns["Vendor"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Vendor"].Width = 150;
                    ////    }
                    ////    if (editor.Grid.Columns["ModifiedDate"] != null)
                    ////    {
                    ////        editor.Grid.Columns["ModifiedDate"].Width = 150;
                    ////    }
                    ////    if (editor.Grid.Columns["StockQty"] != null)
                    ////    {
                    ////        editor.Grid.Columns["StockQty"].Width = 120;
                    ////    }
                    ////    if (editor.Grid.Columns["OrderQty"] != null)
                    ////    {
                    ////        editor.Grid.Columns["OrderQty"].Width = 120;
                    ////    }
                    ////    if (editor.Grid.Columns["PackUnits"] != null)
                    ////    {
                    ////        editor.Grid.Columns["PackUnits"].Width = 100;
                    ////    }
                    ////    if (editor.Grid.Columns["Status"] != null)
                    ////    {
                    ////        editor.Grid.Columns["Status"].Width = 150;
                    ////    }
                    ////    if (editor.Grid.Columns["ModifiedBy.UserName"] != null)
                    ////    {
                    ////        editor.Grid.Columns["ModifiedBy.UserName"].Width = 150;
                    ////    }
                    ////    if (editor.Grid.Columns["RequestedBy"] != null)
                    ////    {
                    ////        editor.Grid.Columns["RequestedBy"].Width = 150;
                    ////    }
                    ////}
                    ////#endregion

                    ////#region Requisition_ListView_Receive
                    if (View.Id == "Requisition_ListView_Receive")
                    {
                        if (editor.Grid.Columns["#ItemRecieved"] != null)
                        {
                            editor.Grid.Columns["#ItemRecieved"].Width = 150;
                        }
                        if (editor.Grid.Columns["#ItemReceiving"] != null)
                        {
                            editor.Grid.Columns["#ItemReceiving"].Width = 150;
                        }
                        if (editor.Grid.Columns["Status"] != null)
                        {
                            editor.Grid.Columns["Status"].Width = 180;
                        }

                    }


                    ////#endregion

                    ////#region Requisition_ListView_ViewMode
                    ////if (View.Id == "Requisition_ListView_ViewMode")
                    ////{
                    ////    gridView.Width = Unit.Percentage(100);
                    ////    //if (editor.Grid.Columns["Item.items"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["Item.items"].Width = 100;
                    ////    //}
                    ////    //if (editor.Grid.Columns["Item.Description"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["Item.Description"].Width = 100;
                    ////    //}
                    ////    //if (editor.Grid.Columns["Item.Specification"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["Item.Specification"].Width = 100;
                    ////    //}
                    ////    //if (editor.Grid.Columns["NonPersistentStatus"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["NonPersistentStatus"].Width = 100;
                    ////    //}
                    ////    //if (editor.Grid.Columns["Status"] != null)
                    ////    //{
                    ////    //    editor.Grid.Columns["Status"].Width = 100;
                    ////    //}
                    ////}

                    ////#endregion

                    ////#region Distribution_ListView_Viewmode
                    //if (View.Id == "Distribution_ListView_Viewmode")
                    //{
                    //    if (editor.Grid.Columns["Item.items"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.items"].Width = 200;
                    //    }
                    //    if (editor.Grid.Columns["Item.Description"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.Description"].Width = 200;
                    //    }
                    //    if (editor.Grid.Columns["Item.Specification"] != null)
                    //    {
                    //        editor.Grid.Columns["Item.Specification"].Width = 200;
                    //    }
                    //}
                    ////#endregion                  
                    #endregion

                    if (Application.MainWindow.View.Id != "RoleNavigationPermission_DetailView" && Application.MainWindow.View.Id != "TestMethod_DetailView" && Application.MainWindow.View.Id != "TestPriceSurcharge_ListView_newlist" && Application.MainWindow.View.Id != "Tasks_DetailView" && Application.MainWindow.View.Id != "Samplecheckin_DetailView_Copy_SampleRegistration"
                        && Application.MainWindow.View.Id != "QCBatch_DetailView" && Application.MainWindow.View.Id != "TestPriceSurcharge_DetailView" && Application.MainWindow.View.Id != "PrepMethod_DetailView" && Application.MainWindow.View.Id != "SamplePretreatmentBatch_DetailView"
                        && Application.MainWindow.View.Id != "SamplePrepBatch_DetailView_Copy" && Application.MainWindow.View.Id != "SampleWeighingBatch_DetailView" && View.Id != "Samplecheckin_ListView_SampleReceiptNotification" && Application.MainWindow.View.Id != "Invoicing_DetailView"
                        && Application.MainWindow.View.Id != "Invoicing_DetailView_Delivery" &&/* Application.MainWindow.View.Id != "Invoicing_DetailView_Review" && Application.MainWindow.View.Id != "Invoicing_DetailView_View_History" &&*/ Application.MainWindow.View.Id != "SampleRegistration"
                        && Application.MainWindow.View.Id != "COCSettings_DetailView_Copy_SampleRegistration" && Application.MainWindow.View.Id != "MaintenanceSetup_DetailView" && Application.MainWindow.View.Id != "SampleParameter_ListView_Copy_ResultEntry_SingleChoice" && Application.MainWindow.View.Id != "Customer_DetailView" && Application.MainWindow.View.Id != "CompliantInitiation_DetailView"
                        && Application.MainWindow.View.Id != "NonConformityInitiation_DetailView" && Application.MainWindow.View.Id != "TrendAnalysis_DetailView" && Application.MainWindow.View.Id != "SamplingProposal_DetailView")
                    {
                        Application.MainWindow.View.Refresh();
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
            try
            {
                base.OnDeactivated();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
