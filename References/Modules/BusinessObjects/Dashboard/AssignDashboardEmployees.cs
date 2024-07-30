using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;

namespace Modules.BusinessObjects.Dashboard
{
    [DefaultClassOptions]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AssignDashboardEmployees : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Constructor
        public AssignDashboardEmployees(Session session)
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

        #region AssignDashboardToUserDepartment      
        private AssignDashboardToUserDepartment _AssignDashboard;
        [Association]
        public AssignDashboardToUserDepartment AssignDashboard
        {
            get { return _AssignDashboard; }
            set { SetPropertyValue("AssignDashboard", ref _AssignDashboard, value); }
        }
        #endregion

        #region Employee
        private Employee _EmployeeName;
        [Association]
        public Employee EmployeeName
        {
            get { return _EmployeeName; }
            set { SetPropertyValue("EmployeeName", ref _EmployeeName, value); }
        }
        #endregion TestParameter

    }
}