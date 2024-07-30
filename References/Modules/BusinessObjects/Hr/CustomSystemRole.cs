using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using Modules.BusinessObjects.Setting;
using System.Collections.Generic;

namespace Modules.BusinessObjects.Hr
{
    [DefaultClassOptions]
    [ImageName("BO_Role")]
    public class CustomSystemRole : PermissionPolicyRole
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public bool IsBatchExport { get; set; }
        #region Constructor
        public CustomSystemRole(Session session)
            : base(session)
        {
        }
        #endregion

        #region Events
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;
            ISNavigationPermission = false;
        }
        #endregion

        #region 公司
        private Company _company;
        /// <summary>
        /// 公司
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Company Company
        {
            get
            {
                return _company;
            }
            set
            {
                SetPropertyValue("Company", ref _company, value);
            }
        }
        #endregion

        #region 导航
        /// <summary>
        /// 导航
        /// </summary>
        [Association("Roles-Navigations")]
        public XPCollection<Navigation> Navigations
        {
            get { return GetCollection<Navigation>("Navigations"); }
        }

        #endregion

        private bool _ISNavigationPermission;

        public bool ISNavigationPermission
        {
            get { return _ISNavigationPermission; }
            set { SetPropertyValue("ISNavigationPermission", ref _ISNavigationPermission, value); }
        }

        [ManyToManyAlias("RoleNavigationPermissionDetails", "NavigationItem")]
        public IList<Modules.BusinessObjects.Setting.NavigationItem> NavigationItems
        {
            get
            {
                return GetList<Modules.BusinessObjects.Setting.NavigationItem>("NavigationItems");
            }
        }

        //[Association, Browsable(false)]
        [Association]
        public IList<RoleNavigationPermissionDetails> RoleNavigationPermissionDetails
        {
            get
            {
                return GetList<RoleNavigationPermissionDetails>("RoleNavigationPermissionDetails");
            }
        }

        private int _Sort;
        public int Sort
        {
            get
            {
                return _Sort;
            }
            set
            {
                SetPropertyValue<int>(nameof(Sort), ref _Sort, value);
            }
        }

        private RoleNavigationPermission _RoleNavigationPermission;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public RoleNavigationPermission RoleNavigationPermission
        {
            get
            {
                return _RoleNavigationPermission;
            }
            set
            {
                SetPropertyValue<RoleNavigationPermission>(nameof(RoleNavigationPermission), ref _RoleNavigationPermission, value);
            }
        }

    }
}