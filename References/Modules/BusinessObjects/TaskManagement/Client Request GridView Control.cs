using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.TaskManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Client_Request_GridView_Control : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Client_Request_GridView_Control(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _Package;
        public string Package
        {
            get { return _Package; }
            set { SetPropertyValue<string>(nameof(Package), ref _Package, value); }
        }
        private string _BatchId;
        public string BatchId
        {
            get { return _BatchId; }
            set { SetPropertyValue<string>(nameof(BatchId), ref _BatchId, value); }
        }
        private string _BatchSize;
        public string BatchSize
        {
            get { return _BatchSize; }
            set { SetPropertyValue<string>(nameof(BatchSize), ref _BatchSize, value); }
        }
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue<string>(nameof(Remark), ref _Remark, value); }
        }
        private Client_Request_General_Information _Client_Request_General_Information;
        [Association("Client_Request_General_Information-Client_Request_GridView_Control")]
        public Client_Request_General_Information Client_Request_General_Information
        {
            get { return _Client_Request_General_Information; }
            set { SetPropertyValue<Client_Request_General_Information>(nameof(Client_Request_General_Information), ref _Client_Request_General_Information, value); }
        }
    }
}
