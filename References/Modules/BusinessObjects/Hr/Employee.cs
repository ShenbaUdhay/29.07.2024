// ================================================================================
// Table Name: [Employee]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：员工管理
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Module.BusinessObjects.Accounts;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Dashboard;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Report;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Modules.BusinessObjects.Hr
{
    /// <summary>
    /// 员工类，继承自用户
    /// </summary>
    [DefaultClassOptions]
    //[DefaultProperty("FullName")]
    [XafDefaultProperty(nameof(DisplayName))]
    public class Employee : CustomSystemUser
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private string CurrentLanguage;
        curlanguage strCurrentLanguage = new curlanguage();
        AnalysisDeptUser objUserInfo = new AnalysisDeptUser();
        #region Constructor
        public Employee(Session session)
            : base(session)
        {
            //SelectedData sproc = Session.ExecuteSproc("getCurrentLanguage");
            //CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
        }
        #endregion

        #region Event
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreateTime = Library.GetServerTime(Session);
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;
            IsNewUser = true;
            Language = Language.English;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
            if (UserID == 0)
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(UserID)");
                int tempID = (Convert.ToInt32(Session.Evaluate(typeof(Employee), criteria, null)) + 1);
                UserID = tempID;
            }
        }
        #endregion
        protected override void OnSaved()
        {
            if (IsNewUser)
            {
                IsNewUser = false;
            }
        }

        #region 公司
        private Company _company;
        /// <summary>
        /// 公司
        /// </summary>
        [Association("Company-Employees")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        public Company Company
        {
            get { return _company; }
            set { SetPropertyValue("Company", ref _company, value); }
        }
        #endregion

        #region 员工号
        private string _employeeNumber;
        /// <summary>
        /// 员工号
        /// </summary>
        [Size(8)]
        //[RuleRequiredField("Employee.EmployeeNumber", DefaultContexts.Save)]
        //[Indexed(Unique = true)]
        [Browsable(false)]
        public string EmployeeNumber
        {
            get
            {
                return _employeeNumber;
            }
            set
            {
                SetPropertyValue("EmployeeNumber", ref _employeeNumber, value);
                if (IsSaving || IsLoading) return;
                if (value == null) return;
                UserName = value;   //同步基类员工号
            }
        }
        #endregion

        #region 中文名格式
        /// <summary>
        /// 中文名格式 
        /// </summary>
        ///
        ///
        private string Name;
        public string FullName
        {

            get
            {
                if (strCurrentLanguage.strcurlanguage == "zh-CN")
                {
                    Name = string.Format("{0}{1}", LastName, FirstName);
                }
                else if (strCurrentLanguage.strcurlanguage == "En")
                {
                    Name = string.Format("{0} {1}", FirstName, LastName);
                }
                return Name;
            }
        }
        #endregion



        #region 英文名格式
        /// <summary>
        /// 英文名格式
        /// </summary>
        //public string FullNameEN
        //{
        //    get { return string.Format("{0} {1}", FirstName, LastName); }
        //}
        #endregion

        #region 名
        private string _firstName;
        /// <summary>
        /// 名
        /// </summary>
        [Size(128)]
        [RuleRequiredField("Employee.FirstName", DefaultContexts.Save)]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                SetPropertyValue("FirstName", ref _firstName, value);
            }
        }
        #endregion

        #region 姓
        private string _lastName;
        /// <summary>
        /// 姓
        /// </summary>
        [Size(128)]
        [RuleRequiredField("Employee.LastName", DefaultContexts.Save)]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                SetPropertyValue("LastName", ref _lastName, value);
            }
        }
        #endregion

        #region Initial
        private string _initial;
        /// <summary>
        /// 姓
        /// </summary>
        [Size(128)]

        public string Initial
        {
            get
            {
                return _initial;
            }
            set
            {
                SetPropertyValue("Initial", ref _initial, value);
            }
        }
        #endregion


        #region JobTitle
        private string _jobTitle;
        /// <summary>
        /// 姓
        /// </summary>
        [Size(128)]

        public string JobTitle
        {
            get
            {
                return _jobTitle;
            }
            set
            {
                SetPropertyValue("JobTitle", ref _jobTitle, value);
            }
        }
        #endregion


        #region 性别
        private Gender _gender;
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                SetPropertyValue("Gender", ref _gender, value);
            }
        }
        #endregion

        #region 国家
        private CustomCountry _country;
        /// <summary>
        /// 国家
        /// </summary>
        [ImmediatePostData(true)]
        //[RuleRequiredField("Employee.Country", DefaultContexts.Save)]
        public CustomCountry Country
        {
            get
            {
                return _country;
            }
            set
            {
                SetPropertyValue("Country", ref _country, value);
            }
        }
        #endregion

        #region 省
        private CustomState _state;
        /// <summary>
        /// 省
        /// </summary>
        [ImmediatePostData(true)]
        [DataSourceProperty("Country.States", DataSourcePropertyIsNullMode.SelectNothing)]
        //[RuleRequiredField("Employee.State", DefaultContexts.Save)]
        public CustomState State
        {
            get
            {
                return _state;
            }
            set
            {
                SetPropertyValue("State", ref _state, value);
            }
        }
        #endregion

        #region 市
        private City _city;
        /// <summary>
        /// 市
        /// </summary>
        [DataSourceProperty("State.Cities", DataSourcePropertyIsNullMode.SelectNothing)]
        //[RuleRequiredField("Employee.City", DefaultContexts.Save)]
        public City City
        {
            get
            {
                return _city;
            }
            set
            {
                SetPropertyValue("City", ref _city, value);
            }
        }
        #endregion

        #region 住址
        private string _address;
        /// <summary>
        /// 住址
        /// </summary>
        [Size(256)]
        [ModelDefault("RowCount", "1")]
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                SetPropertyValue("Address", ref _address, value);
            }
        }
        #endregion

        private string password;
        //[RuleRequiredField]
        //[DisplayFormat("Password")]
        [Size(255)]
        public string Password
        {
            get { return password; }
            set { SetPropertyValue(nameof(Password), ref password, value); }
        }
        //public void UpdateMaskedPassword()
        //{
        //    Password = Password?.Replace('*', '*');
        //}



        #region 邮编
        private string _zip;
        /// <summary>
        /// 邮编
        /// </summary>
        [Size(16)]
        public string Zip
        {
            get
            {
                return _zip;
            }
            set
            {
                SetPropertyValue("Zip", ref _zip, value);
            }
        }
        #endregion

        #region 出生日期
        private DateTime _birthday;
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday
        {
            get
            {
                return _birthday;
            }
            set
            {
                SetPropertyValue("Birthday", ref _birthday, value);
            }
        }
        #endregion

        #region 部门
        private Department _department;
        /// <summary>
        /// 部门
        /// </summary>
        [Association("Department-Employees")]
        //[RuleRequiredField("Employee.Department", DefaultContexts.Save)] 
        [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Department Department
        {
            get
            {
                return _department;
            }
            set
            {
                SetPropertyValue("Department", ref _department, value);
            }
        }
        #endregion

        #region 职务
        private Position _position;
        /// <summary>
        /// 职务
        /// </summary>
        //[RuleRequiredField("Employee.Position", DefaultContexts.Save)] 
        [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Position Position
        {
            get
            {
                return _position;
            }
            set
            {
                SetPropertyValue("Position", ref _position, value);
            }
        }
        #endregion

        #region 办公电话
        private string _officePhone;
        /// <summary>
        /// 办公电话
        /// </summary>
        [Size(50)]
        public string OfficePhone
        {
            get
            {
                return _officePhone;
            }
            set
            {
                SetPropertyValue("OfficePhone", ref _officePhone, value);
            }
        }
        #endregion

        #region 家庭电话
        private string _homePhone;
        /// <summary>
        /// 家庭电话
        /// </summary>
        [Size(50)]
        public string HomePhone
        {
            get
            {
                return _homePhone;
            }
            set
            {
                SetPropertyValue("HomePhone", ref _homePhone, value);
            }
        }
        #endregion

        #region 手机
        private string _mobilePhone;
        /// <summary>
        /// 手机
        /// </summary>
        [Size(50)]
        public string MobilePhone
        {
            get
            {
                return _mobilePhone;
            }
            set
            {
                SetPropertyValue("MobilePhone", ref _mobilePhone, value);
            }
        }
        #endregion

        #region 传真
        private string _fax;
        /// <summary>
        /// 传真
        /// </summary>
        [Size(50)]
        public string Fax
        {
            get
            {
                return _fax;
            }
            set
            {
                SetPropertyValue("Fax", ref _fax, value);
            }
        }
        #endregion

        #region 其它电话
        private string _otherPhone;
        /// <summary>
        /// 其它电话
        /// </summary>
        [Size(50)]
        public string OtherPhone
        {
            get
            {
                return _otherPhone;
            }
            set
            {
                SetPropertyValue("OtherPhone", ref _otherPhone, value);
            }
        }
        #endregion

        #region 邮箱
        private string _email;
        //[RuleRequiredField]
        /// <summary>
        /// 邮箱
        /// </summary>
        [Size(256)]
        [ModelDefault("RowCount", "1")]
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                SetPropertyValue("Email", ref _email, value);
            }
        }
        #endregion

        #region 照片
        private byte[] _photo;
        /// <summary>
        /// 照片
        /// </summary>
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] Photo
        {
            get
            {
                return _photo;
            }
            set
            {
                SetPropertyValue("Photo", ref _photo, value);
            }
        }
        #endregion

        #region 签名
        private byte[] _signature;
        /// <summary>
        /// 签名
        /// </summary>
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] Signature
        {
            get
            {
                return _signature;
            }
            set
            {
                SetPropertyValue("Signature", ref _signature, value);
            }
        }
        #endregion

        #region 简历
        private string _resume;
        /// <summary>
        /// 简历
        /// </summary>
        [Size(int.MaxValue)]
        public string Resume
        {
            get
            {
                return _resume;
            }
            set
            {
                SetPropertyValue("Resume", ref _resume, value);
            }
        }
        #endregion

        #region 聘用日期
        private DateTime? _employmentDate;
        /// <summary>
        /// 聘用日期
        /// </summary>
        //[RuleRequiredField("Employee.EmploymentDate", DefaultContexts.Save)]
        public DateTime? EmploymentDate
        {
            get
            {
                return _employmentDate;
            }
            set
            {
                SetPropertyValue("EmploymentDate", ref _employmentDate, value);
            }
        }
        #endregion

        #region StartDate
        [NonPersistent]
        public DateTime? StartDate
        {
            get { return EmploymentDate; }
        }
        #endregion

        #region 退役日期
        private DateTime _retireDate;
        /// <summary>
        /// 退役日期
        /// </summary>
        public DateTime RetireDate
        {
            get
            {
                return _retireDate;
            }
            set
            {
                SetPropertyValue("RetireDate", ref _retireDate, value);
            }
        }
        #endregion

        #region 语言
        private Language _language;
        /// <summary>
        /// 语言
        /// </summary>
        public Language Language
        {
            get
            {
                return _language;
            }
            set
            {
                SetPropertyValue("Language", ref _language, value);
            }
        }
        #endregion

        #region 是否启用语言
        private bool _isEnabledLanguage;
        public bool IsEnabledLanguage
        {
            get { return _isEnabledLanguage; }
            set { SetPropertyValue("IsEnabledLanguage", ref _isEnabledLanguage, value); }
        }
        #endregion

        #region 创建人
        private CustomSystemUser _createdBy;
        /// <summary>
        /// 创建人
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser CreatedBy
        {
            get
            {
                return _createdBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _createdBy, value);
            }
        }
        #endregion

        #region 创建时间
        private DateTime _createTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreateTime
        {
            get
            {
                return _createTime;
            }
            set
            {
                SetPropertyValue("CreateTime", ref _createTime, value);
            }
        }
        #endregion

        #region 修改人
        private CustomSystemUser _updatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser UpdatedBy
        {
            get
            {
                return _updatedBy;
            }
            set
            {
                SetPropertyValue("UpdatedBy", ref _updatedBy, value);
            }
        }
        #endregion

        #region 修改时间
        private DateTime _updateTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime UpdateTime
        {
            get
            {
                return _updateTime;
            }
            set
            {
                SetPropertyValue("UpdateTime", ref _updateTime, value);
            }
        }
        #endregion

        [Association, Browsable(false)]
        public IList<EmailUser> EmailUser
        {
            get
            {
                return GetList<EmailUser>("EmailUser");
            }
        }


        [Association("ActivityUser", UseAssociationNameAsIntermediateTableName = true), Browsable(false)]
        public XPCollection<CRMActivity> Activity
        {
            get { return GetCollection<CRMActivity>("Activity"); }
        }

        private int _UserID;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[FetchOnly]
        public int UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                SetPropertyValue("UserID", ref _UserID, value);
            }
        }

        private string _EmpDepartment;
        [NonPersistent]
        public string EmpDepartment
        {
            get
            {
                if (Department != null && Department.Name != null)
                {
                    return Department.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string _EmpPosition;
        [NonPersistent]
        public string EmpPosition
        {
            get
            {
                if (Position != null && Position.PositionName != null)
                {
                    return Position.PositionName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #region RoleNames
        public string RoleNames
        {
            get
            {
                if (RolePermissions.Count > 0)
                {
                    return string.Format("{0}", string.Join(",", RolePermissions.ToList().Where(i => !string.IsNullOrEmpty(i.RoleName)).Select(i => i.RoleName).Distinct().ToList()));
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region DashboardEmployee
        [ManyToManyAlias("AssignDashboardEmployees", "AssignDashboard")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public IList<AssignDashboardToUserDepartment> AssignDashboard
        {
            get { return GetList<AssignDashboardToUserDepartment>("AssignDashboard"); }
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

        #region RolePermissions
        [Association("EmployeeRoleNavigationPermission", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<RoleNavigationPermission> RolePermissions
        {
            get
            {
                return GetCollection<RoleNavigationPermission>(nameof(RolePermissions));
            }
        }
        #endregion

        #region SamplePrepChains
        [VisibleInDetailView(false)]
        [Association("SamplePreparationChainUsers", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<SamplePreparationChain> SamplePrepChains
        {
            get
            {
                return GetCollection<SamplePreparationChain>(nameof(SamplePrepChains));
            }
        }
        #endregion


        #region Notes
        [VisibleInDetailView(false)]
        //[Association("Notes", UseAssociationNameAsIntermediateTableName = true)]
        [Association("Employee-Notes")]
        public XPCollection<Notes> Notes
        {
            get
            {
                return GetCollection<Notes>(nameof(Notes));
            }
        }
        #endregion 
        private Activity _Activity;
        [Association("Activity-Employee")]
        public Activity Activitys
        {
            get { return _Activity; }
            set { SetPropertyValue("Activitys", ref _Activity, value); }
        }

        //[VisibleInDetailView(false), VisibleInListView(false)]
        //[Association("NoteUser", UseAssociationNameAsIntermediateTableName = true), Browsable(false)]
        //public XPCollection<Notes> Note
        //{
        //    get { return GetCollection<Notes>("Note"); }
        //}

        private bool _IsManager;
        public bool IsManager
        {
            get { return _IsManager; }
            set { SetPropertyValue("IsManager", ref _IsManager, value); }
        }


        //[ManyToManyAlias("EmailUser", "EmailSetting")]
        //public IList<EmailSetting> EmailSettings
        //{
        //    get
        //    {
        //        return GetList<EmailSetting>("EmailSettings");
        //    }

        //}
        #region ReportingTo
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        private string _ReportingTo;
        // [DataSourceProperty(nameof(ReportingToUser), DataSourcePropertyIsNullMode.SelectNothing)]
        public string ReportingTo
        {
            get
            {
                return _ReportingTo;
            }
            set { SetPropertyValue("ReportingTo", ref _ReportingTo, value); }
        }

        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> properties = new Dictionary<object, string>();
            if (targetMemberName == "ReportingTo" && ReportingToUser != null && ReportingToUser.Count > 0)
            {
                foreach (Employee objUser in ReportingToUser.Where(a => a.UserName != null).OrderByDescending(a => a.UserName).ToList())
                {
                    if (!properties.ContainsKey(objUser.UserName))
                    {
                        properties.Add(objUser.Oid, objUser.UserName);
                    }
                }
            }
            return properties;
        }

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }


        [NonPersistent]
        public XPCollection<Employee> ReportingToUser
        {
            get
            {
                return new XPCollection<Employee>(Session, CriteriaOperator.Parse("[Oid] <> ? and [FirstName]<>?", Oid, "Admin"));
            }

        }
        #endregion

        #region DisplayName
        private string _DisplayName;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string DisplayName
        {
            get { return _DisplayName; }
            set { SetPropertyValue("DisplayName", ref _DisplayName, value); }
        }
        #endregion

        #region IsEmailValid
        [RuleFromBoolProperty(nameof(IsEmailValid), DefaultContexts.Save, CustomMessageTemplate = "Invalid email format!", UsedProperties = nameof(UserName))]
        [Browsable(false)]
        public bool IsEmailValid
        {
            get
            {
                System.ComponentModel.DataAnnotations.EmailAddressAttribute emailAddressValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
                return emailAddressValidator.IsValid(UserName);
            }
        }
        #endregion
        #region IsNewUser
        private bool _IsNewUser;
        [Browsable(false)]
        public bool IsNewUser
        {
            get { return _IsNewUser; }
            set { SetPropertyValue("IsNewUser", ref _IsNewUser, value); }
        }
        #endregion
        #region IsPasswordNotSet
        private bool _IsPasswordNotSet;
        [Browsable(false)]
        [NonPersistent]
        public bool IsPasswordNotSet
        {
            get
            {
                if (string.IsNullOrEmpty(StoredPassword))
                {
                    _IsPasswordNotSet = true;
                }
                else
                {
                    _IsPasswordNotSet = false;
                }
                return _IsPasswordNotSet;
            }

        }
        #endregion
        #region SetPassword
        private bool _SetPassword;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public bool SetPassword
        {
            get {
                if (string.IsNullOrEmpty(StoredPassword))
                {
                    _SetPassword = false;
                }
                else
                {
                    _SetPassword = true;
                }
                return _SetPassword; }
            set { SetPropertyValue("SetPassword", ref _SetPassword, value); }
        }
        #endregion


        #region MailServerName
        private string _MailServerName;
        public string MailServerName
        {
            get { return _MailServerName; }
            set { SetPropertyValue("MailServerName", ref _MailServerName, value); }
        }
        #endregion
        #region Port
        private int _Port;
        public int Port
        {
            get { return _Port; }
            set { SetPropertyValue("Port", ref _Port, value); }
        }
        #endregion

        #region EnableSSL
        private bool _EnableSSL;
        public bool EnableSSL
        {
            get { return _EnableSSL; }
            set { SetPropertyValue("EnableSSL", ref _EnableSSL, value); }
        }
        #endregion

        #region Default
        private bool _SampleRegistrationDefault ;
        public bool SampleRegistrationDefault
        {
            get { return _SampleRegistrationDefault; }
            set { SetPropertyValue("SampleRegistrationDefault", ref _SampleRegistrationDefault, value); }
        }

        private bool _ReportDeliveryDefault;
        public bool ReportDeliveryDefault
        {
            get { return _ReportDeliveryDefault; }
            set { SetPropertyValue("ReportDeliveryDefault", ref _ReportDeliveryDefault, value); }
        }

        private bool _InvoiceDeliveryDefault;
        public bool InvoiceDeliveryDefault
        {
            get { return _InvoiceDeliveryDefault; }
            set { SetPropertyValue("InvoiceDeliveryDefault", ref _InvoiceDeliveryDefault, value); }
        }
        #endregion



    }
}