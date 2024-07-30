namespace Modules.BusinessObjects.Report
{
    public partial class SampleReport : DevExpress.XtraReports.UI.XtraReport
    {
        public SampleReport()
        {
            InitializeComponent();
        }

        public void DataBindingsToReport()
        {
            lblSampleID.DataBindings.Add("Text", DataSource, "SampleBottleID");
            lblSampleName.DataBindings.Add("Text", DataSource, "SampleName");
            lblSampleMatrix.DataBindings.Add("Text", DataSource, "SampleMatrix");
            lblCollectedDate.DataBindings.Add("Text", DataSource, "CollectDateTime");
            lblRecievedDate.DataBindings.Add("Text", DataSource, "RecievedDate");
            lblDueDate.DataBindings.Add("Text", DataSource, "DueDate");
            lblTest.DataBindings.Add("Text", DataSource, "Tests");
            xrBarCode1.DataBindings.Add("Text", DataSource, "SampleBottleID");
            //xrTableCell4.DataBindings.Add("Text",DataSource, "SampleID");
        }
    }
}
