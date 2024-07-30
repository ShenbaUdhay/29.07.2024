using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.ICM
{
    [DomainComponent]
    [NonPersistent]
    [FileAttachment("InputFile")]
    public class ItemsFileUpload : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ItemsFileUpload(Session session)
            : base(session)
        {


        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.InputFile = new NPFileData();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        //private FileData _file;

        //[NonPersistent]
        //public FileData File
        //{
        //    get { return _file; }
        //    set { SetPropertyValue<FileData>("File", ref _file, value); }
        //}

        private NPFileData _npfiledata;
        public NPFileData InputFile
        {
            get { return _npfiledata; }
            set { SetPropertyValue<NPFileData>(nameof(InputFile), ref _npfiledata, value); }
        }
    }
}