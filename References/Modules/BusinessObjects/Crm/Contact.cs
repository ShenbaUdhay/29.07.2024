// ================================================================================
// Table Name: [Contact]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：客户联系人
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.E_Mail;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Crm
{
    /// <summary>
    /// 表[Contact]的实体类
    /// </summary>
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("Contact", DefaultContexts.Save, "FullName,Customer", SkipNullOrEmptyValues = false)]

    [Appearance("showProspect", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "CRMProspect;", Criteria = "[Prospect] = True", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("hideProspect", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "CRMProspect;", Criteria = "[Prospect] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("showCustomer", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "Customer;", Criteria = "[Prospect] = True", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("hideCustomer", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "Customer;", Criteria = "[Prospect] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    public class Contact : BaseObject
    {
        //private string CurrentLanguage;
        curlanguage strCurrentLanguage = new curlanguage();
        /// <summary>
        /// 初始化类 Contact 的新实例。
        /// </summary>
        public Contact(Session session) : base(session)
        {
            //SelectedData sproc = Session.ExecuteSproc("getCurrentLanguage");
            //CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here.
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;

            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            // Place you update code here.
        }


        private string Name;
        //[RuleUniqueValue]
        public string FullName
        {

            get
            {
                if (strCurrentLanguage.strcurlanguage == "zh-CN")
                {
                    Name = string.Format("{0}{1}{2}", LastName, MiddleName, FirstName);
                }
                else if (strCurrentLanguage.strcurlanguage == "En")
                {
                    Name = string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
                }
                return Name;
            }
        }

        //public string FullName
        //{
        //    get { return string.Format("{0} {1} {2}", FirstName,MiddleName, LastName); }
        //}
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

        #region 名
        private string _firstName;
        /// <summary>
        /// 名
        /// </summary>
        [Size(100)]
        [RuleRequiredField("Contact.FirstName", DefaultContexts.Save)]
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

        private string _MiddleName;
        /// <summary>
        /// 名
        /// </summary>
        [Size(100)]
        public string MiddleName
        {
            get
            {
                return _MiddleName;
            }
            set
            {
                SetPropertyValue("MiddleName", ref _MiddleName, value);
            }
        }

        #region 姓
        private string _lastName;
        /// <summary>
        /// 姓
        /// </summary>
        [Size(100)]
        //[RuleRequiredField("Contact.LastName", DefaultContexts.Save)]
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

        #region 出生年月
        private DateTime _birthday;
        /// <summary>
        /// 出生年月
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

        #region 客户
        private Customer _customer;
        /// <summary>
        /// 客户
        /// </summary>
        [Association("Customer-Contacts")]
        [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Customer Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                SetPropertyValue("Customer", ref _customer, value);
            }
        }
        #endregion

        #region 职务
        private string _position;
        /// <summary>
        /// 职务
        /// </summary>
        public string Position
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

        #region 账户
        private string _account;
        /// <summary>
        /// 账户
        /// </summary>
        [Size(128)]
        public string Account
        {
            get
            {
                return _account;
            }
            set
            {
                SetPropertyValue("Account", ref _account, value);
            }
        }
        #endregion

        #region 邮箱
        private string _email;
        /// <summary>
        /// 邮箱
        /// </summary>
        [Size(128)]
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
        //[RuleRequiredField("Contact.MobilePhone", DefaultContexts.Save)]
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

        #region 地址1
        private string _street1;
        /// <summary>
        /// 地址1
        /// </summary>
        [Size(128)]
        public string Street1
        {
            get
            {
                return _street1;
            }
            set
            {
                SetPropertyValue("Street1", ref _street1, value);
            }
        }
        #endregion

        #region 地址2
        private string _street2;
        /// <summary>
        /// 地址2
        /// </summary>
        [Size(128)]
        public string Street2
        {
            get
            {
                return _street2;
            }
            set
            {
                SetPropertyValue("Street2", ref _street2, value);
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
                if (Country == null)
                {
                    State = null;
                }
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
                if (Country == null || State == null)
                {
                    City = null;
                }
                return _city;
            }
            set
            {
                SetPropertyValue("City", ref _city, value);
            }
        }
        #endregion

        #region 邮编
        private string _zip;
        /// <summary>
        /// 邮编
        /// </summary>
        [Size(8)]
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

        #region 婚姻状况
        private MaritalStatus _maritalStatus;
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public MaritalStatus MaritalStatus
        {
            get
            {
                return _maritalStatus;
            }
            set
            {
                SetPropertyValue("MaritalStatus", ref _maritalStatus, value);
            }
        }
        #endregion

        #region 配偶姓名
        private string _spouseName;
        /// <summary>
        /// 配偶姓名
        /// </summary>
        [Size(128)]
        public string SpouseName
        {
            get
            {
                return _spouseName;
            }
            set
            {
                SetPropertyValue("SpouseName", ref _spouseName, value);
            }
        }
        #endregion

        #region 结婚纪念日
        private DateTime _anniversary;
        /// <summary>
        /// 结婚纪念日
        /// </summary>
        public DateTime Anniversary
        {
            get
            {
                return _anniversary;
            }
            set
            {
                SetPropertyValue("Anniversary", ref _anniversary, value);
            }
        }
        #endregion

        #region ReportDelivery

        [Association("To-Contact", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Email> MailTo
        {
            get { return GetCollection<Email>("MailTo"); }
        }
        [Association("CC-Contact", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Email> MailCC
        {
            get { return GetCollection<Email>("MailCC"); }
        }
        [Association("Bcc-Contact", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Email> MailBcc
        {
            get { return GetCollection<Email>("MailBcc"); }
        }

        #endregion


        #region Prospects
        private Prospects _Prospects;
        //[VisibleInListView(false)]
        [VisibleInDetailView(false)]
        public virtual Prospects Prospects
        {
            get { return _Prospects; }
            set { SetPropertyValue<Prospects>("Prospects", ref _Prospects, value); }
        }
        #endregion



        #region Prospect
        private bool _Prospect;
        [ImmediatePostData(true)]
        public bool Prospect
        {
            get
            {
                return _Prospect;
            }
            set
            {
                SetPropertyValue<bool>("Prospect", ref _Prospect, value);
            }
        }
        #endregion

        #region 客户
        private ProspectClient _ProspectClient;
        /// <summary>
        /// 客户
        /// </summary>
        [Association("ProspectClient-Contacts")]
        [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public ProspectClient ProspectClient
        {
            get
            {
                return _ProspectClient;
            }
            set
            {
                SetPropertyValue("ProspectClient", ref _ProspectClient, value);
            }
        }
        #endregion


        private string _JobTitle;
        public string JobTitle
        {
            //get { return Person.JobTitle; }
            //set { Person.JobTitle = value; }

            get
            {
                return _JobTitle;
            }
            set
            {
                SetPropertyValue<string>("JobTitle", ref _JobTitle, value);
            }
        }

        #region CRMProspect
        private CRMProspects _CRMProspect;
        [XafDisplayName("Client")]
        [DataSourceProperty(nameof(ProsClientDS))]

        public CRMProspects CRMProspect
        {
            get { return _CRMProspect; }
            set { SetPropertyValue(nameof(CRMProspect), ref _CRMProspect, value); }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<CRMProspects> ProsClientDS
        {
            get
            {
                if (Prospect == true)
                {
                    XPCollection<CRMProspects> lstTests = new XPCollection<CRMProspects>(Session, CriteriaOperator.Parse("[Status] = 'None'"));

                    return lstTests;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region IsReport
        private bool _IsReport;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsReport
        {
            get { return _IsReport; }
            set { SetPropertyValue(nameof(IsReport), ref _IsReport, value); }
        }
        #endregion

        #region IsInvoice
        private bool _IsInvoice;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsInvoice
        {
            get { return _IsInvoice; }
            set { SetPropertyValue(nameof(IsInvoice), ref _IsInvoice, value); }
        }
        #endregion

        #region EmailCC
        private string _EmailCC;
        [Size(128)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string EmailCC
        {
            get { return _EmailCC; }
            set { SetPropertyValue(nameof(EmailCC), ref _EmailCC, value); }
        }
        #endregion
        #region EmailBCC
        private string _EmailBCC;
        [Size(128)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string EmailBCC
        {
            get { return _EmailBCC; }
            set { SetPropertyValue(nameof(EmailBCC), ref _EmailBCC, value); }
        }
        #endregion
        #region Comment
        private string _Comment;
        [Size(1000)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion
        #region Terms
        private string _Terms;
        public string Terms
        {
            get { return _Terms; }
            set { SetPropertyValue(nameof(Terms), ref _Terms, value); }
        }
        #endregion
        #region SampleReceiptNotification
        private bool _SampleReceiptNotification;
        public bool SampleReceiptNotification
        {
            get { return _SampleReceiptNotification; }
            set { SetPropertyValue(nameof(SampleReceiptNotification), ref _SampleReceiptNotification, value); }
        }
        #endregion
        #region ReportDelivery
        private bool _ReportDelivery;
        public bool ReportDelivery
        {
            get { return _ReportDelivery; }
            set { SetPropertyValue(nameof(ReportDelivery), ref _ReportDelivery, value); }
        }
        #endregion

        #region InvoiceDelivery
        private bool _InvoiceDelivery;
        public bool InvoiceDelivery
        {
            get { return _InvoiceDelivery; }
            set { SetPropertyValue(nameof(InvoiceDelivery), ref _InvoiceDelivery, value); }
        }
        #endregion
    }
}
