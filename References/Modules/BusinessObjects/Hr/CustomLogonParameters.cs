using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Modules.BusinessObjects.Hr
{
    //在此类中设置登陆面页可展示的字段
    [DomainComponent, Serializable]
    [System.ComponentModel.DisplayName("Log On")]
    public class CustomLogonParameters : ISerializable, ICustomObjectSerialize
    {
        #region Constructor
        public CustomLogonParameters()
        {
            //UserName = "Administrator";//默认写上登陆人
        }
        // ISerializable 
        public CustomLogonParameters(SerializationInfo info, StreamingContext context)
        {
            if (info.MemberCount > 0)
            {
                UserName = info.GetString("UserName");
                _password = info.GetString("Password");
            }
        }
        #endregion

        #region 公司
        private Company _company;
        [Browsable(false)]
        [DataSourceProperty("AvailableCompanies"), ImmediatePostData]
        public Company Company
        {
            get { return _company; }
            set
            {
                if (_company == value) return;
                _company = value;
                RefreshAvailableUsers();
            }
        }
        #endregion

        #region 员工
        private Employee _employee;
        [Browsable(false)]
        [DataSourceProperty("AvailableUsers"), ImmediatePostData]
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                if (_employee == value || value == null) return;
                _employee = value;
                Company = _employee.Company;
                UserName = _employee.UserName;
            }
        }
        #endregion

        #region 用户名
        [Browsable(true)]
        public String UserName { get; set; }
        #endregion

        #region 密码
        private string _password;
        [PasswordPropertyText(true)]
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
            }
        }
        #endregion

        #region 语言
        private Enum _language;
        // [DataSourceProperty("AvailableUsers"), ImmediatePostData]
        [Browsable(false)]
        public Enum Language
        {
            get { return _language; }
            set
            {
                if (_language == value) return;
                _language = value;
            }
        }
        #endregion

        #region 根据公司筛选员工
        private void RefreshAvailableUsers()
        {
            if (_availableUsers == null) return;
            if (Company == null)
            {
                _availableUsers.Criteria = null;
            }
            else
            {
                _availableUsers.Criteria = new BinaryOperator("Company", Company);
            }
            if (_employee != null)
            {
                if ((_availableUsers.IndexOf(_employee) == -1) || (_employee.Company != Company))
                {
                    Employee = null;
                }
            }
        }

        private IObjectSpace _objectSpace;
        private XPCollection<Company> _availableCompanies;
        private XPCollection<Employee> _availableUsers;
        [Browsable(false)]
        public IObjectSpace ObjectSpace
        {
            get { return _objectSpace; }
            set { _objectSpace = value; }
        }
        [Browsable(false)]
        [CollectionOperationSet(AllowAdd = false)]
        public XPCollection<Company> AvailableCompanies
        {
            get
            {
                if (_availableCompanies == null)
                {
                    _availableCompanies = ObjectSpace.GetObjects<Company>() as XPCollection<Company>;
                }
                return _availableCompanies;
            }
        }
        [Browsable(false)]
        [CollectionOperationSet(AllowAdd = false)]
        public XPCollection<Employee> AvailableUsers
        {
            get
            {
                if (_availableUsers == null)
                {
                    _availableUsers = ObjectSpace.GetObjects<Employee>() as XPCollection<Employee>;
                    RefreshAvailableUsers();
                }
                return _availableUsers;
            }
        }
        #endregion

        #region RememberPassword

        private bool _RememberPassword;
        public bool RememberPassword
        {
            get
            {
                return _RememberPassword;
            }
            set
            {
                _RememberPassword = value;
            }
        }
        #endregion

        #region Function
        [System.Security.SecurityCritical]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UserName", UserName);
            info.AddValue("Password", _password);
        }
        public void ReadPropertyValues(SettingsStorage storage)
        {
            UserName = storage.LoadOption("", "UserName");
            Password = storage.LoadOption("", "Password");
            RememberPassword = storage.LoadBoolOption("", "RememberPassword", false);
        }
        public void WritePropertyValues(SettingsStorage storage)
        {
            storage.SaveOption("", "UserName", UserName);
            storage.SaveOption("", "Password", RememberPassword ? Password : "");
            storage.SaveOption("", "RememberPassword", RememberPassword.ToString());
        }
        #endregion

    }
}