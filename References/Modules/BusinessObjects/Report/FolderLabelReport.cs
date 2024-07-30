namespace Modules.BusinessObjects.Report
{
    public partial class FolderLabelReport : DevExpress.XtraReports.UI.XtraReport
    {
        public FolderLabelReport()
        {
            InitializeComponent();
        }

        public void DataBindingsToReport()
        {
            lblJobID.DataBindings.Add("Text", DataSource, "JobID");
            lblProjectID.DataBindings.Add("Text", DataSource, "ProjectID");
            lblClient.DataBindings.Add("Text", DataSource, "Client");
            lblDateReceived.DataBindings.Add("Text", DataSource, "DateReceived");
        }
    }
}
