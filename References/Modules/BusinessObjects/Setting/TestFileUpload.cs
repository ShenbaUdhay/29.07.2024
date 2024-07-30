using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.ICM;

namespace Modules.BusinessObjects.Setting
{
    [DomainComponent]
    [NonPersistent]
    [FileAttachment("InputFile")]

    public class TestFileUpload : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TestFileUpload(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.InputFile = new NPFileData();

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        private NPFileData _npfiledata;
        public NPFileData InputFile
        {
            get { return _npfiledata; }
            set { SetPropertyValue<NPFileData>(nameof(InputFile), ref _npfiledata, value); }
        }
    }
}

