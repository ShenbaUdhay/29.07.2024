using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Dashboard
{
    [DefaultClassOptions]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AssignDashboardToUserDepartment : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        #region Consturctor
        public AssignDashboardToUserDepartment(Session session)
            : base(session)
        {
        }
        #endregion

        #region Default Events
        public override void AfterConstruction()
        {

            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).                       
        }
        #endregion

        #region GetCheckedListBoxItems
        Dictionary<object, string> ICheckedListBoxItemsProvider.GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "Role" && RoleDataSource != null && RoleDataSource.Count > 0)
            {
                foreach (RoleNavigationPermission objds in RoleDataSource.Where(i => i.RoleName != null).OrderBy(i => i.RoleName).ToList())
                {
                    if (!Properties.ContainsKey(objds.Oid))
                    {
                        Properties.Add(objds.Oid, objds.RoleName);
                    }
                }
            }
            return Properties;
        }
        #endregion

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }

        #region Employee
        [ManyToManyAlias("AssignDashboardEmployees", "EmployeeName")]
        public IList<Employee> EmployeeName
        {
            get
            {
                return GetList<Employee>("EmployeeName");
            }
        }
        [Association, Browsable(false)]
        public IList<AssignDashboardEmployees> AssignDashboardEmployees
        {
            get
            {
                return GetList<AssignDashboardEmployees>("AssignDashboardEmployees");
            }
        }
        #endregion

        #region Department
        private Department _DepartmentName;
        public Department DepartmentName
        {
            get { return _DepartmentName; }
            set
            {
                SetPropertyValue<Department>("DepartmentName", ref _DepartmentName, value);
            }
        }
        #endregion

        #region Dashboard
        private DashboardData _DashboardViewName;
        [RuleRequiredField("AssignDashboard.DashboardViewName", DefaultContexts.Save)]
        public DashboardData DashboardViewName
        {
            get { return _DashboardViewName; }
            set
            {
                SetPropertyValue<DashboardData>("DashboardViewName", ref _DashboardViewName, value);
            }
        }
        #endregion

        #region NavigationItem
        private DefaultSetting _NavigationItem;
        [DataSourceProperty(nameof(NavigationItemDataSource))]
        public DefaultSetting NavigationItem
        {
            get
            {
                return _NavigationItem;
            }
            set
            {
                SetPropertyValue<DefaultSetting>("NavigationItem", ref _NavigationItem, value);
            }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<DefaultSetting> NavigationItemDataSource
        {
            get
            {
                return new XPCollection<DefaultSetting>(Session, CriteriaOperator.Parse("[IsModule] = True"));
            }
        }
        #endregion
        #region Role
        private string _Role;
        //[ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Role
        {
            get { return _Role; }
            set { SetPropertyValue("Role", ref _Role, value); }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<RoleNavigationPermission> RoleDataSource
        {
            get
            {

                return new XPCollection<RoleNavigationPermission>(Session, CriteriaOperator.Parse(""));
            }
        }

        #endregion

        #region IsActive
        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set { SetPropertyValue(nameof(IsActive), ref _IsActive, value); }
        }
        #endregion

        #region Template
        private byte[] _Template;
        public byte[] Template
        {
            get { return _Template; }
            set { SetPropertyValue<byte[]>(nameof(Template), ref _Template, value); }
        }
        #endregion

    }
}