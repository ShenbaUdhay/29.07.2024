namespace Modules.BusinessObjects.Report
{
    public partial class FolderLabel : DevExpress.XtraReports.UI.XtraReport
    {
        public FolderLabel()
        {
            InitializeComponent();
        }


        public void DataBindingsToReport()
        {

            JobID.DataBindings.Add("Text", DataSource, "JobID");
            ProjectID.DataBindings.Add("Text", DataSource, "ProjectID");
            Client.DataBindings.Add("Text", DataSource, "Client");
            DateReceived.DataBindings.Add("Text", DataSource, "DateReceived");
        }

    }
}
