using DevExpress.XtraReports.UI;

namespace Modules.BusinessObjects.Report
{
    public partial class DOCCertificate : DevExpress.XtraReports.UI.XtraReport
    {
        private TopMarginBand topMarginBand1;
        private DetailBand detailBand1;
        private TopMarginBand topMarginBand2;
        private DetailBand detailBand2;
        private BottomMarginBand bottomMarginBand2;
        private TopMarginBand topMarginBand3;
        private DetailBand detailBand3;
        private BottomMarginBand bottomMarginBand3;
        private TopMarginBand topMarginBand4;
        private DetailBand detailBand4;
        private BottomMarginBand bottomMarginBand4;
        private BottomMarginBand bottomMarginBand1;

        public DOCCertificate()
        {
            InitializeComponent();
            //lblDOCID.DataBindings.Add("Text", DataSource, "DocID");
        }

        public void DataBindingsToReport()
        {
            lblDOCID.DataBindings.Add("Text", DataSource, "DocID");
            lblTestName.DataBindings.Add("Text", DataSource, "TestName");
            lblMethodNumber.DataBindings.Add("Text", DataSource, "MethodNumber");
            lblMatrixName.DataBindings.Add("Text", DataSource, "MatrixName");
            lblPrepMethod.DataBindings.Add("Text", DataSource, "SamplePrepType");
            lblAnalysisName.DataBindings.Add("Text", DataSource, "AnalysisName");
            lblPrepBy.DataBindings.Add("Text", DataSource, "PreparedBy");
            //lblProjectID.DataBindings.Add("Text", DataSource, "ProjectID");
            //lblClient.DataBindings.Add("Text", DataSource, "Client");
            //lblDateReceived.DataBindings.Add("Text", DataSource, "DateReceived");
        }
    }
}
