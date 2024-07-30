using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.SampleManagement;

namespace Modules.BusinessObjects.QC
{
    [DefaultClassOptions]
    [FileAttachment("Image")]
    public class SDMSUploadImage : BaseObject
    {
        public SDMSUploadImage(Session session) : base(session)
        {

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.Image = new NPFileData();
        }

        private NPFileData fImage;
        [ImmediatePostData]
        [NonPersistent]
        public NPFileData Image
        {
            get
            {
                return fImage;
            }
            set
            {
                SetPropertyValue("Image", ref fImage, value);
            }
        }

        private string fFileName;
        public string FileName
        {
            get
            {
                return fFileName;
            }
            set
            {
                SetPropertyValue("FileName", ref fFileName, value);
            }
        }

        private byte[] fData;
        public byte[] Data
        {
            get
            {
                return fData;
            }
            set
            {
                SetPropertyValue("Data", ref fData, value);
            }
        }

        private SpreadSheetEntry_AnalyticalBatch fABID;
        public SpreadSheetEntry_AnalyticalBatch ABID
        {
            get
            {
                return fABID;
            }
            set
            {
                SetPropertyValue("ABID", ref fABID, value);
            }
        }
    }
}