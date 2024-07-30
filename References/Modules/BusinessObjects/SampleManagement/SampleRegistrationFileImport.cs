using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SampleManagement
{
    [DomainComponent]
    [NonPersistent]
    //[FileAttachment("InputFile")]
    public class SampleRegistrationFileImport : BaseObject
    {
        public SampleRegistrationFileImport(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private FileData _InputFile;
        public FileData InputFile
        {
            get { return _InputFile; }
            set { SetPropertyValue<FileData>(nameof(InputFile), ref _InputFile, value); }
        }

        //private NPFileData _npfiledata;
        //public NPFileData InputFile
        //{
        //    get { return _npfiledata; }
        //    set { SetPropertyValue<NPFileData>(nameof(InputFile), ref _npfiledata, value); }
        //}
    }
}