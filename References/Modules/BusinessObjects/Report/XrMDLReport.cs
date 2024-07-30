namespace Modules.BusinessObjects.Report
{
    public partial class XrMDLReport : DevExpress.XtraReports.UI.XtraReport
    {
        public XrMDLReport()
        {
            InitializeComponent();
        }
        public void MDLDataBindingsToReport()
        {
            //lblDOCID.DataBindings.Add("Text", DataSource, "DocID");
            lblDOCID.DataBindings.Add("Text", DataSource, "DOCID");
            lblTestname.DataBindings.Add("Text", DataSource, "TestName");
            lblmethod.DataBindings.Add("Text", DataSource, "MethodNumber");
            lblMatrix.DataBindings.Add("Text", DataSource, "MatrixName");
            lblPrepMethod.DataBindings.Add("Text", DataSource, "PrepMethod");
            lblAnalysedby.DataBindings.Add("Text", DataSource, "AnalyzedBy");
            lblPreparedby.DataBindings.Add("Text", DataSource, "PreparedBy");
            lblDatePrepared.DataBindings.Add("Text", DataSource, "DatePrepared");
            lblPrepBatchID.DataBindings.Add("Text", DataSource, "PrepBatchID");
            lblDateTCLPPrepared.DataBindings.Add("Text", DataSource, "DateTCLPPrepared");
            lblTCLPPrepMethod.DataBindings.Add("Text", DataSource, "TCLPPrepMethod");
            lblTCLPPreparedby.DataBindings.Add("Text", DataSource, "PrepBatchID");
            lblQCBatchID.DataBindings.Add("Text", DataSource, "QCBatchID");
            lblJobID.DataBindings.Add("Text", DataSource, "JobID");
            //lblSpikeAmount.DataBindings.Add("Text", DataSource, "SpikeAmount");
            lblSPreparationInfo.DataBindings.Add("Text", DataSource, "SpikePrepInfo");
            lblSConcentration.DataBindings.Add("Text", DataSource, "SpikeConcentration");
            lblSID.DataBindings.Add("Text", DataSource, "SpikeStandardID");
            //lblStandardName.DataBindings.Add("Text", DataSource, "SpikeStandardName");
            lblSUnit.DataBindings.Add("Text", DataSource, "SpikeUnits");
            lblPrepInstrument.DataBindings.Add("Text", DataSource, "PrepInstrument");
            lblDateAnalyzed.DataBindings.Add("Text", DataSource, "DateAnalyzed");
            lblParameter.DataBindings.Add("Text", DataSource, "Parameter");
            lblSPKAmt.DataBindings.Add("Text", DataSource, "SPKAmt");
            lblRun1.DataBindings.Add("Text", DataSource, "Result1");
            lblRun2.DataBindings.Add("Text", DataSource, "Result2");
            lblRun3.DataBindings.Add("Text", DataSource, "Result3");
            lblRun4.DataBindings.Add("Text", DataSource, "Result4");
            lblRun5.DataBindings.Add("Text", DataSource, "Result5");
            lblRun6.DataBindings.Add("Text", DataSource, "Result6");
            lblRun7.DataBindings.Add("Text", DataSource, "Result7");
            lblAVE.DataBindings.Add("Text", DataSource, "Average");
            lblRec.DataBindings.Add("Text", DataSource, "%RecAve");
            lblSD.DataBindings.Add("Text", DataSource, "SD");
            lblRSD.DataBindings.Add("Text", DataSource, "RSD");
            lblMDL.DataBindings.Add("Text", DataSource, "MDL");
            lblMDL1.DataBindings.Add("Text", DataSource, "MDL1");
            lblRptLimit.DataBindings.Add("Text", DataSource, "RptLimit");
            lblRatio.DataBindings.Add("Text", DataSource, "Ratio");
            //lblQCInstruments.DataBindings.Add("Text", DataSource, "QCInstruments");
            lblDateApproved.DataBindings.Add("Text", DataSource, "DateApproved");
            lblApprovedby.DataBindings.Add("Text", DataSource, "ApprovedBy");
            //lblStatus.DataBindings.Add("Text", DataSource, "Status");
            lblLLimit.DataBindings.Add("Text", DataSource, "LLimit");
            lblHLimit.DataBindings.Add("Text", DataSource, "HLimit");
            lblMDLCheck.DataBindings.Add("Text", DataSource, "MDLCheck");
            lblComment.DataBindings.Add("Text", DataSource, "Comments");
            lblTCLPPreparedby.DataBindings.Add("Text", DataSource, "TCLPPreparedby");
        }
    }
}
