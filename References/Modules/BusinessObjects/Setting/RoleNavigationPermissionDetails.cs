using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[RuleCombinationOfPropertiesIsUnique("RoleNavigationPermission", DefaultContexts.Save, "NavigationItem.Oid,CustomSystemRole.Oid", SkipNullOrEmptyValues = false)]
    public class RoleNavigationPermissionDetails : BaseObject
    {
        //private string CurrentLanguage;
        curlanguage curlanguage = new curlanguage();
        public RoleNavigationPermissionDetails(Session session)
            : base(session)
        {
            //SelectedData sproc = session.ExecuteSproc("getCurrentLanguage", "");
            //CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region NavigationItem      
        private NavigationItem _NavigationItem;
        [Association]
        public NavigationItem NavigationItem
        {
            get { return _NavigationItem; }
            set { SetPropertyValue("NavigationItem", ref _NavigationItem, value); }
        }
        #endregion

        #region CustomRole
        private CustomSystemRole _CustomRole;
        [Association]
        public CustomSystemRole CustomSystemRole
        {
            get { return _CustomRole; }
            set { SetPropertyValue("CustomSystemRole", ref _CustomRole, value); }
        }
        #endregion

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

        private bool _Navigate;
        public bool Navigate
        {
            get { return _Navigate = true; }
            set { SetPropertyValue("Navigate", ref _Navigate, value); }
        }

        #region RoleNavigationPermission
        private RoleNavigationPermission _RoleNavigationPermission;
        [Association]
        public RoleNavigationPermission RoleNavigationPermission
        {
            get { return _RoleNavigationPermission; }
            set { SetPropertyValue("RoleNavigationPermission", ref _RoleNavigationPermission, value); }
        }
        #endregion
        private string _NPNavigationItemCaption;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string NPNavigationItemCaption
        {
            get
            {
                if (curlanguage.strcurlanguage == "En")
                {
                    if (NavigationItem != null && NavigationItem.NavigationCaption != null)
                    {
                        _NPNavigationItemCaption = NavigationItem.NavigationCaption;
                    }
                }
                else
                {
                    if (NavigationItem != null && NavigationItem.NavigationCNCaption != null)
                    {
                        _NPNavigationItemCaption = NavigationItem.NavigationCNCaption;
                    }
                }
                return _NPNavigationItemCaption;
            }
            set { SetPropertyValue("NPNavigationItemCaption", ref _NPNavigationItemCaption, value); }
        }

    }
}