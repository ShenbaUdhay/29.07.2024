using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Hr
{
    [DefaultClassOptions]
    [NonPersistent]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class UserRightDC : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public UserRightDC(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set { SetPropertyValue("FullName", ref _FullName, value); }
        }
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { SetPropertyValue("UserName", ref _UserName, value); }
        }
        private string _Role;
        public string Role
        {
            get { return _Role; }
            set { SetPropertyValue("Role", ref _Role, value); }
        }
        private string _NavigationItem;
        public string NavigationItem
        {
            get { return _NavigationItem; }
            set { SetPropertyValue("NavigationItem", ref _NavigationItem, value); }
        }
        private bool _Read;
        public bool Read
        {
            get { return _Read = true; }
            set { SetPropertyValue("Read", ref _Read, value); }
        }

        private bool _Write;
        public bool Write
        {
            get { return _Write; }
            set { SetPropertyValue("Write", ref _Write, value); }
        }

        private bool _Create;
        public bool Create
        {
            get { return _Create; }
            set { SetPropertyValue("Create", ref _Create, value); }
        }

        private bool _Delete;
        public bool Delete
        {
            get { return _Delete; }
            set { SetPropertyValue("Delete", ref _Delete, value); }
        }
        private string _User;
        public string User
        {
            get
            {
                if (FullName != null && UserName != null)
                {
                    _User = FullName + "| " + "UserName: " + UserName;
                }
                return _User;
            }
            set { SetPropertyValue("User", ref _User, value); }
        }
    }
}