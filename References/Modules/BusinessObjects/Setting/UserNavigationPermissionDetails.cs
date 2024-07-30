using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class UserNavigationPermissionDetails : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public UserNavigationPermissionDetails(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region NavigationItem      
        private NavigationItem _NavigationItem;
        [Association]
        public NavigationItem NavigationItem
        {
            get { return _NavigationItem; }
            set { SetPropertyValue("NavigationItem", ref _NavigationItem, value); }
        }
        #endregion NavigationItem

        #region UserNavigationPermission
        private UserNavigationPermission _UserNavigationPermission;
        [Association]
        public UserNavigationPermission UserNavigationPermission
        {
            get { return _UserNavigationPermission; }
            set { SetPropertyValue("UserNavigationPermission", ref _UserNavigationPermission, value); }
        }
        #endregion UserNavigationPermission


        private bool _Read;

        public bool Read
        {
            get { return _Read; }
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

        private bool _Navigate;
        public bool Navigate
        {
            get { return _Navigate; }
            set { SetPropertyValue("Navigate", ref _Navigate, value); }
        }
    }
}