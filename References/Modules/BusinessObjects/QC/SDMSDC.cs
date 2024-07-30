using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using Modules.BusinessObjects.ICM;

namespace Modules.BusinessObjects.QC
{
    [DomainComponent]
    public class SDMSDCSpreadsheet
    {
        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        public byte[] Data { get; set; }
    }

    [DomainComponent]
    public class SDMSDCAB { }


    [DomainComponent]
    [FileAttachment("Import")]
    public class SDMSDCImport
    {
        public SDMSDCImport()
        {
            this.Import = new NPFileData();
        }
        public string InstFileType { get; set; }

        [ImmediatePostData]
        public NPFileData Import { get; set; }
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }

    [NonPersistent]
    public class SDMSReportPopupDC
    {
        [Key]
        public int ID { get; set; }
        public string Report { get; set; }
    }
}