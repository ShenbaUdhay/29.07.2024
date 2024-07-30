using DevExpress.ExpressApp.DC;

namespace Modules.BusinessObjects.SampleManagement
{
    [DomainComponent]
    public class PDFPreview
    {
        public byte[] PDFData { get; set; }
        public string ReportID { get; set; }
        public string ViewID { get; set; }
    }
}