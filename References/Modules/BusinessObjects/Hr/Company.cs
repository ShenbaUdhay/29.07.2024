// ================================================================================
// Table Name: [Company]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：公司
// ================================================================================
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Hr
{
    [DefaultClassOptions]
    public class Company : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Consturctor
        public Company(Session session)
            : base(session)
        {
        }
        #endregion

        #region Events
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here.
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreateTime = Library.GetServerTime(Session);
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
            IsValid = true;
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            UpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
        }
        #region OnDelete
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (BaseObject obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.Oid != null)
                    {
                        Exception ex = new Exception("Already Used Can't allow to Delete");
                        throw ex;
                        break;

                    }
                }
            }
        }
        #endregion

        #endregion

        #region 公司编码
        private string _companyCode;
        /// <summary>
        /// 公司编码
        /// </summary>
        [Size(16)]
        //[RuleRequiredField("Company.CompanyCode", DefaultContexts.Save)]
        [Browsable(false)]
        public string CompanyCode
        {
            get
            {
                return _companyCode;
            }
            set
            {
                SetPropertyValue("CompanyCode", ref _companyCode, value);
            }
        }
        #endregion

        #region 公司名称
        private string _companyName;
        /// <summary>
        /// 公司名称
        /// </summary>
        [Size(128)]
        [RuleRequiredField("Company.CompanyName", DefaultContexts.Save)]
        public string CompanyName
        {
            get
            {
                return _companyName;
            }
            set
            {
                SetPropertyValue("CompanyName", ref _companyName, value);
            }
        }
        #endregion

        #region 国家
        private CustomCountry _country;
        /// <summary>
        /// 国家
        /// </summary>
        [ImmediatePostData(true)]
        [RuleRequiredField("Company.Country", DefaultContexts.Save)]
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
        [RuleRequiredField("Company.State", DefaultContexts.Save)]
        [DataSourceProperty("Country.States", DataSourcePropertyIsNullMode.SelectNothing)]
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

        #region ZipCode
        private string _ZipCode;
        [ImmediatePostData(true)]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public string ZipCode
        {
            get
            {
                return _ZipCode;
            }
            set
            {
                SetPropertyValue("ZipCode", ref _ZipCode, value);
            }
        }
        #endregion

        #region 市
        private City _city;
        /// <summary>
        /// 市
        /// </summary>
        [RuleRequiredField("Company.City", DefaultContexts.Save)]
        [DataSourceProperty("State.Cities", DataSourcePropertyIsNullMode.SelectNothing)]
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

        #region 地址
        private string _address;
        /// <summary>
        /// 地址
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

        #region 网站
        private string _webSite;
        /// <summary>
        /// 网站
        /// </summary>
        [Size(128)]
        public string WebSite
        {
            get
            {
                return _webSite;
            }
            set
            {
                SetPropertyValue("WebSite", ref _webSite, value);
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

        #region 是否有效

        private bool _isValid;
        /// <summary>
        /// 公司是否有效
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
            set { SetPropertyValue("IsValid", ref _isValid, value); }
        }
        #endregion

        #region 联系人
        private string _contactName;
        /// <summary>
        /// 联系人
        /// </summary>
        [Size(256)]
        [ModelDefault("RowCount", "1")]
        [RuleRequiredField("Company.ContactName", DefaultContexts.Save)]
        public string ContactName
        {
            get
            {
                return _contactName;
            }
            set
            {
                SetPropertyValue("ContactName", ref _contactName, value);
            }
        }
        #endregion

        #region 办公电话
        private string _officePhone;
        /// <summary>
        /// 办公电话
        /// </summary>
        [Size(16)]
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
        [Size(16)]
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
        [Size(16)]
        [RuleRequiredField("Company.MobilePhone", DefaultContexts.Save)]
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
        [Size(16)]
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
        [Size(16)]
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

        #region 备注
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        [Size(2048)]
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                SetPropertyValue("Remark", ref _remark, value);
            }
        }
        #endregion

        #region 公司Logo
        private byte[] _logo;
        /// <summary>
        /// 公司Logo
        /// </summary>
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] Logo
        {
            get
            {
                return _logo;
            }
            set
            {
                SetPropertyValue("Logo", ref _logo, value);
            }
        }
        #endregion

        #region 二维码
        private byte[] _qrCode;
        /// <summary>
        /// 二维码
        /// </summary>
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] QRCode
        {
            get
            {
                return _qrCode;
            }
            set
            {
                SetPropertyValue("QRCode", ref _qrCode, value);
            }
        }
        #endregion

        #region 强检标志
        private byte[] _measureCVFlag;
        /// <summary>
        /// 强检标志
        /// </summary>
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] MeasureCVFlag
        {
            get
            {
                return _measureCVFlag;
            }
            set
            {
                SetPropertyValue("MeasureCVFlag", ref _measureCVFlag, value);
            }
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

        #region CompanyEmployee
        /// <summary>
        /// 公司下的员工
        /// </summary>
        [DevExpress.Xpo.Association("Company-Employees")]
        public XPCollection<Employee> Employees
        {
            get { return GetCollection<Employee>("Employees"); }
        }
        #endregion

        #region CompanyDepartment
        /// <summary>
        /// 公司下的部门科室
        /// </summary>
        [DevExpress.Xpo.Association("Company-Departments")]
        public XPCollection<Department> Departments
        {
            get { return GetCollection<Department>("Departments"); }
        }
        #endregion

        #region CompanyPositions
        /// <summary>
        /// 公司下的职务
        /// </summary>
        [DevExpress.Xpo.Association("Company-Positions")]
        public XPCollection<Position> Positions
        {
            get { return GetCollection<Position>("Positions"); }
        }
        #endregion


        [Association]
        public XPCollection<Certificates> Certificate
        {
            get { return GetCollection<Certificates>(nameof(Certificate)); }
        }

    }
}