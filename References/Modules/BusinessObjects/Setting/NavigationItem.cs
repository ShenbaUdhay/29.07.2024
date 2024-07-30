using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(NavigationCNCaption))]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class NavigationItem : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public NavigationItem(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _NavigationId;

        public string NavigationId
        {
            get { return _NavigationId; }
            set { SetPropertyValue("NavigationId", ref _NavigationId, value); }
        }

        private string _NavigationCaption;
        public string NavigationCaption
        {
            get { return _NavigationCaption; }
            set { SetPropertyValue("NavigationCaption", ref _NavigationCaption, value); }
        }

        private string _NavigationCNCaption;
        public string NavigationCNCaption
        {
            get { return _NavigationCNCaption; }
            set { SetPropertyValue("NavigationCNCaption", ref _NavigationCNCaption, value); }
        }


        private string _NavigationView;

        public string NavigationView
        {
            get { return _NavigationView; }
            set { SetPropertyValue("NavigationView", ref _NavigationView, value); }
        }

        private string _NavigationModelClass;
        [Size(500)]
        public string NavigationModelClass
        {
            get { return _NavigationModelClass; }
            set { SetPropertyValue("_NavigationModelClass", ref _NavigationModelClass, value); }
        }

        private string _Itempath;
        [Size(1000)]
        public string Itempath
        {
            get { return _Itempath; }
            set { SetPropertyValue("Itempath", ref _Itempath, value); }
        }

        [Association, Browsable(false)]
        public IList<UserNavigationPermissionDetails> UserNavigationPermissionDetails
        {
            get
            {
                return GetList<UserNavigationPermissionDetails>("UserNavigationPermissionDetails");
            }
        }
        [Association, Browsable(false)]
        public IList<RoleNavigationPermissionDetails> RoleNavigationPermissionDetails
        {
            get
            {
                return GetList<RoleNavigationPermissionDetails>("RoleNavigationPermissionDetails");
            }
        }

        [ManyToManyAlias("UserNavigationPermissionDetails", "UserNavigationPermission")]
        public IList<UserNavigationPermission> UserNavigationPermissions
        {
            get
            {
                return GetList<UserNavigationPermission>("UserNavigationPermissions");
            }
        }
        [ManyToManyAlias("RoleNavigationPermissionDetails", "CustomSystemRole")]
        public IList<CustomSystemRole> CustomSystemRoles
        {
            get
            {
                return GetList<CustomSystemRole>("CustomSystemRoles");
            }
        }
        private bool _Read;
        [NonPersistent]
        [ImmediatePostData]
        public bool Read
        {
            get { return _Read; }
            set { SetPropertyValue("Read", ref _Read, value); }
        }

        private bool _Write;
        [NonPersistent]
        [ImmediatePostData]
        public bool Write
        {
            get { return _Write; }
            set { SetPropertyValue("Write", ref _Write, value); }
        }

        private bool _Create;
        [NonPersistent]
        [ImmediatePostData]
        public bool Create
        {
            get { return _Create; }
            set { SetPropertyValue("Create", ref _Create, value); }
        }

        private bool _Delete;
        [NonPersistent]
        [ImmediatePostData]
        public bool Delete
        {
            get { return _Delete; }
            set { SetPropertyValue("Delete", ref _Delete, value); }
        }

        private bool _Navigate;
        [NonPersistent]
        [ImmediatePostData]
        public bool Navigate
        {
            get { return _Navigate; }
            set { SetPropertyValue("Navigate", ref _Navigate, value); }
        }

        private string _Parent;
        public string Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                SetPropertyValue<string>("Parent", ref _Parent, value);
            }
        }
        private bool _Exclude;
        public bool Exclude
        {
            get { return _Exclude; }
            set { SetPropertyValue("Exclude", ref _Exclude, value); }
        }

        private bool _Exist;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public bool Exist
        {
            get { return _Exist; }
            set { SetPropertyValue("Exist", ref _Exist, value); }
        }

        #region Linkedclasses
        [Association("NavigationItemLinkedClasses")]
        public XPCollection<LinkedClasses> LinkedClasses
        {
            get
            {
                return GetCollection<LinkedClasses>(nameof(LinkedClasses));
            }
        }
        #endregion

        private bool _Defaultbool;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public bool Defaultbool
        {
            get { return _Defaultbool; }
            set { SetPropertyValue("Defaultbool", ref _Defaultbool, value); }
        }
        private bool _Select;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public bool Select
        {
            get { return _Select; }
            set { SetPropertyValue("Select", ref _Select, value); }
        }

    }
}