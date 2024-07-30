using DevExpress.XtraReports.UI;

namespace Modules.BusinessObjects.Report
{
    public partial class XrDOCReport : DevExpress.XtraReports.UI.XtraReport
    {
        private TopMarginBand topMarginBand1;
        private DetailBand detailBand1;
        private BottomMarginBand bottomMarginBand1;

        public XrDOCReport()
        {
            InitializeComponent();
        }
        public void DataBindingsToReport()
        {
            //lblDOCID.DataBindings.Add("Text", DataSource, "DocID");
            lblDOCID.DataBindings.Add("Text", DataSource, "DOCID");
            lblTestname.DataBindings.Add("Text", DataSource, "TestName");
            lblmethod.DataBindings.Add("Text", DataSource, "MethodNumber");
            lblMatrix.DataBindings.Add("Text", DataSource, "MatrixName");
            lblPrepMethod.DataBindings.Add("Text", DataSource, "PrepMethod");
            lblAnalysedby.DataBindings.Add("Text", DataSource, "AnalysisName");
            lblPreparedby.DataBindings.Add("Text", DataSource, "PreparedBy");
            lblDatePrepared.DataBindings.Add("Text", DataSource, "DatePrepared");
            lblPrepBatchID.DataBindings.Add("Text", DataSource, "PrepBatchID");
            lblQCBatchID.DataBindings.Add("Text", DataSource, "QCBatchID");
            lblJobID.DataBindings.Add("Text", DataSource, "JobID");
            lblSpikeAmount.DataBindings.Add("Text", DataSource, "SpikeAmount");
            lblPreparationInfo.DataBindings.Add("Text", DataSource, "SpikePrepInfo");
            lblConcentration.DataBindings.Add("Text", DataSource, "SpikeConcentration");
            lblStandardID.DataBindings.Add("Text", DataSource, "SpikeStandardID");
            lblStandardName.DataBindings.Add("Text", DataSource, "SpikeStandardName");
            lblSpikeUnit.DataBindings.Add("Text", DataSource, "SpikeUnits");
            lblPrepInstrument.DataBindings.Add("Text", DataSource, "PrepInstrument");
            lblDateAnalyzed.DataBindings.Add("Text", DataSource, "DateAnalyzed");
            lblParameter.DataBindings.Add("Text", DataSource, "Parameter");
            lblSpikeAmount.DataBindings.Add("Text", DataSource, "SPKAmt");
            lblRun1.DataBindings.Add("Text", DataSource, "Result1");
            lblRun2.DataBindings.Add("Text", DataSource, "Result2");
            lblRun3.DataBindings.Add("Text", DataSource, "Result3");
            lblRun4.DataBindings.Add("Text", DataSource, "Result4");
            //dataTable.DataBindings.Add("Text", DataSource, "Result5");
            //dataTable.DataBindings.Add("Text", DataSource, "Result6");
            //dataTable.DataBindings.Add("Text", DataSource, "Result7");
            lblAVE.DataBindings.Add("Text", DataSource, "Average");
            lblRec.DataBindings.Add("Text", DataSource, "%RecAve");
            lblSD.DataBindings.Add("Text", DataSource, "SD");
            lblRSD.DataBindings.Add("Text", DataSource, "RSD");
            //dataTable.DataBindings.Add("Text", DataSource, "MDL");
            //dataTable.DataBindings.Add("Text", DataSource, "MDL1");
            //dataTable.DataBindings.Add("Text", DataSource, "RptLimit");
            //dataTable.DataBindings.Add("Text", DataSource, "Ratio");
            lblInstrument.DataBindings.Add("Text", DataSource, "QCInstruments");
            lblDateApproved.DataBindings.Add("Text", DataSource, "DateApproved");
            lblApprovedby.DataBindings.Add("Text", DataSource, "ApprovedBy");
            //dataTable.DataBindings.Add("Text", DataSource, "Status");
            lblLowLimit.DataBindings.Add("Text", DataSource, "LLimit");
            lblHighLimit.DataBindings.Add("Text", DataSource, "HLimit");
            //dataTabl.DataBindings.Add("Text", DataSource, "MDLCheck");
            lblComment.DataBindings.Add("Text", DataSource, "Comments");
            lblDateTCLPPrepared.DataBindings.Add("Text", DataSource, "DateTCLPPrepared");
            lblTCLPPreparedby.DataBindings.Add("Text", DataSource, "TCLPPreparedby");
            lblTCLPPrepMethod.DataBindings.Add("Text", DataSource, "TCLPPrepMethod");
            //ana.DataBindings.Add("Text", DataSource, "DateTCLPPrepared");
        }


    }
}
