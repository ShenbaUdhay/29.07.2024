// ================================================================================
// Table Name: [Customer]
// Author: Sunny
// Date: 2016年12月15日
// ================================================================================
// Change History
// ================================================================================
// 		Date:		Author:				Description:
// 		--------	--------			-------------------
//    
// ================================================================================
// Desciption：客户管理
// ================================================================================
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Seting;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.BusinessObjects.Crm
{
    /// <summary>
    /// 表[Customer]的实体类
    /// </summary>   
    //public enum CustomerStatus
    //{
    //    Inactive = 0,
    //    Active = 1
    //}

    public enum PreferredContactMethod
    {
        Any = 0,
        Email = 1,
        Phone = 2,
        Fax = 3,
        Mail = 4
    }

    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("customer", DefaultContexts.Save, "CustomerName,Address", SkipNullOrEmptyValues = false)]
    //[Appearance("showclosereason", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "IsClose;CloseReason;", Criteria = "[IsClose] = True", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    //[Appearance("hideclosereason", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "IsClose;CloseReason;", Criteria = "[IsClose] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    //[Appearance("showIsclose", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "Clauses;MethodNumber;", Criteria = "[IndoorInspection] = True Or [OutdoorInspection] = True", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    //[Appearance("hideIsclose", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "Clauses;MethodNumber;", Criteria = "[IndoorInspection] = False And [OutdoorInspection] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    public class Customer : BaseObject /*,IProspectsCustomer, INotifyPropertyChanged*/
    {

        /// <summary>
        /// 初始化类 Customer 的新实例。
        /// </summary>
        public Customer(Session session) : base(session)
        {
            Prospect = new List<Prospects>();
            //Invoices = new List<Invoice>();
            //Quotes = new List<Quote>();
            //Orders = new List<Order>();
            ////Payables = new List<Payables>();
            //Status = CustomerStatus.Active;
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here.
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreateTime = Library.GetServerTime(Session);
            UpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);
            Company = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId).Company;



        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            UpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            UpdateTime = Library.GetServerTime(Session);

            if (string.IsNullOrEmpty(ClientCode))
            {
                ClientCode += Convert.ToInt32(Session.Evaluate<Customer>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(ClientCode)"), null)) + 1;
                if (ClientCode.Length == 2)
                {
                    ClientCode = "0" + ClientCode;
                }
                else if (ClientCode.Length == 3)
                {
                    ClientCode = "0" + ClientCode;
                }
                else
                {
                    ClientCode = "00" + ClientCode;
                }
            }
            if (Terms == null)
            {
                Terms = Session.FindObject<PaymentTerms>(CriteriaOperator.Parse("Terms = 30 "));
            }
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
            //System.Collections.ICollection lstReferenceObjects = Session.CollectReferencingObjects(this);
            //if (lstReferenceObjects.Count > 0)
            //{
            //    foreach (var obj in Session.CollectReferencingObjects(this))
            //    {
            //        if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject))
            //        {
            //            Exception ex = new Exception("Already used can't allow to delete");
            //            throw ex;
            //            break;
            //        }
            //    }
            //}
        }

        #region ClientCode
        private string _clientCode;
        /// <summary>
        /// 传真
        /// </summary>
        [Size(50)]
        public string ClientCode
        {
            get
            {
                return _clientCode;
            }
            set
            {
                SetPropertyValue("ClientCode", ref _clientCode, value);
            }
        }
        #endregion
        //[InverseProperty("Customer")]
        //public virtual IList<Prospects> Opportunities { get; set; }

        #region Company
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

        #region CustomerName
        private string _customerName;
        /// <summary>
        /// 名称
        /// </summary>
        [Size(500)]
        [RuleRequiredField("Customer.CustomerName", DefaultContexts.Save,"'Client' must not be empty")]
        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                SetPropertyValue("CustomerName", ref _customerName, value);
            }
        }
        #endregion

        #region ClientNumber
        private string _clientNumber;
        /// <summary>
        /// 传真
        /// </summary>
        [Size(50)]
        public string ClientNumber
        {
            get
            {
                return _clientNumber;
            }
            set
            {
                SetPropertyValue("ClientNumber", ref _clientNumber, value);
            }
        }
        #endregion

        #region Account
        private string _account;
        /// <summary>
        /// 传真
        /// </summary>
        [Size(50)]
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

        #region Type
        private KeyValue _type;
        /// <summary>
        /// 单位类型
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='16'")]
        public KeyValue Type
        {
            get
            {
                return _type;
            }
            set
            {
                SetPropertyValue("Type", ref _type, value);
            }
        }
        #endregion

        #region 年收入
        private decimal _annualRevenue;
        /// <summary>
        /// 年收入
        /// </summary>
        public decimal AnnualRevenue
        {
            get
            {
                return _annualRevenue;
            }
            set
            {
                SetPropertyValue("AnnualRevenue", ref _annualRevenue, value);
            }
        }
        #endregion

        #region 雇员人数
        private int _numberOfEmployees;
        /// <summary>
        /// 雇员人数
        /// </summary>
        public int NumberOfEmployees
        {
            get
            {
                return _numberOfEmployees;
            }
            set
            {
                SetPropertyValue("NumberOfEmployees", ref _numberOfEmployees, value);
            }
        }
        #endregion

        #region Industry
        private KeyValue _industry;
        /// <summary>
        /// 行业
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='17'")]
        public KeyValue Industry
        {
            get
            {
                return _industry;
            }
            set
            {
                SetPropertyValue("Industry", ref _industry, value);
            }
        }
        #endregion

        #region Zone
        private KeyValue _zone;
        /// <summary>
        /// 分区（州）
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='18'")]
        public KeyValue Zone
        {
            get
            {
                return _zone;
            }
            set
            {
                SetPropertyValue("Zone", ref _zone, value);
            }
        }
        #endregion

        //#region Category
        //private KeyValue _category;
        ///// <summary>
        ///// 分类（诚信）
        ///// </summary>
        //[DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='19'")]
        //public KeyValue Category
        //{
        //    get
        //    {
        //        return _category;
        //    }
        //    set
        //    {
        //        SetPropertyValue("Category", ref _category, value);
        //    }
        //}
        //#endregion

        #region PrimaryContact
        private Contact _primaryContact;
        /// <summary>
        /// 主要联系人
        /// </summary>
        [DataSourceProperty("ContactsDataSource")]
        [ImmediatePostData]
        public Contact PrimaryContact
        {
            get
            {
                return _primaryContact;
            }
            set
            {
                SetPropertyValue("PrimaryContact", ref _primaryContact, value);
            }
        }

        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public XPCollection<Contact> ContactsDataSource
        {
            get
            {
                if (Oid != null)
                {
                    if (Contacts != null && Contacts.Count > 0)
                    {
                        return new XPCollection<Contact>(Session, new InOperator("Oid", Contacts));
                    }
                    else
                    {
                        return null;
                    }
                    //return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region 办公电话
        private string _officePhone;
        /// <summary>
        /// 办公电话
        /// </summary>
        [Size(50)]
        //[RuleRequiredField("Customer.OfficePhone", DefaultContexts.Save)]
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

        private string _MobilePhone;
        public string MobilePhone { get { return _MobilePhone; } set { SetPropertyValue<string>("MobilePhone", ref _MobilePhone, value); } }
        private string _HomePhone;
        public string HomePhone { get { return _HomePhone; } set { SetPropertyValue<string>("HomePhone", ref _HomePhone, value); } }



        #region WebSite
        private string _webSite;
        /// <summary>
        /// 网站
        /// </summary>
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

        #region Fax
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

        #region Email
        private string _email;
        /// <summary>
        /// 邮箱
        /// </summary>
        [Size(50)]
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

        #region Country
        private CustomCountry _country;
        /// <summary>
        /// 国家
        /// </summary>
        //[RuleRequiredField("Customer.Country", DefaultContexts.Save)]
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

        #region State
        private CustomState _state;
        /// <summary>
        /// 省
        /// </summary>
        //[RuleRequiredField("Customer.State", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        [DataSourceProperty("Country.States", DataSourcePropertyIsNullMode.SelectNothing)]
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

        #region City
        private City _city;
        /// <summary>
        /// 市
        /// </summary>
        //[RuleRequiredField("Customer.City", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        [DataSourceProperty("State.Cities", DataSourcePropertyIsNullMode.SelectNothing)]
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

        #region Area
        private Area _area;
        /// <summary>
        /// 区
        /// </summary>
        [ImmediatePostData(true)]
        [DataSourceProperty("City.Areas", DataSourcePropertyIsNullMode.SelectNothing)]
        public Area Area
        {
            get
            {
                return _area;
            }
            set
            {
                SetPropertyValue("Area", ref _area, value);
            }
        }
        #endregion

        #region 地址
        private string _address;
        /// <summary>
        /// 地址
        /// </summary>
        [Size(1024)]
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


        #region Address1
        private string _address1;
        /// <summary>
        /// 地址
        /// </summary>
        [Size(1024)]
        public string Address1
        {
            get
            {
                return _address1;
            }
            set
            {
                SetPropertyValue("Address1", ref _address1, value);
            }
        }
        #endregion

        #region Zip
        private string _zip;
        /// <summary>
        /// 邮编
        /// </summary>
        [Size(50)]
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

        #region PostCode
        private string _postCode;
        /// <summary>
        /// 邮编
        /// </summary>

        public string PostCode
        {
            get
            {
                return _postCode;
            }
            set
            {
                SetPropertyValue("PostCode", ref _postCode, value);
            }
        }
        #endregion

        #region SICCode
        private string _sicCode;
        /// <summary>
        /// 邮编
        /// </summary>

        public string SICCode
        {
            get
            {
                return _sicCode;
            }
            set
            {
                SetPropertyValue("SICCode", ref _sicCode, value);
            }
        }
        #endregion

        #region SiteMap
        private string _siteMap;
        /// <summary>
        /// 邮编
        /// </summary>

        public string SiteMap
        {
            get
            {
                return _siteMap;
            }
            set
            {
                SetPropertyValue("SiteMap", ref _siteMap, value);
            }
        }


        #endregion

        #region UserDefine01
        private string _userDefine01;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine01
        {
            get
            {
                return _userDefine01;
            }
            set
            {
                SetPropertyValue("UserDefine01", ref _userDefine01, value);
            }
        }


        #endregion

        #region UserDefine02
        private string _userDefine02;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine02
        {
            get
            {
                return _userDefine02;
            }
            set
            {
                SetPropertyValue("UserDefine02", ref _userDefine02, value);
            }
        }


        #endregion

        #region UserDefine03
        private string _userDefine03;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine03
        {
            get
            {
                return _userDefine03;
            }
            set
            {
                SetPropertyValue("UserDefine03", ref _userDefine03, value);
            }
        }


        #endregion

        #region UserDefine04
        private string _userDefine04;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine04
        {
            get
            {
                return _userDefine04;
            }
            set
            {
                SetPropertyValue("UserDefine04", ref _userDefine04, value);
            }
        }


        #endregion

        #region UserDefine05
        private string _userDefine05;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine05
        {
            get
            {
                return _userDefine05;
            }
            set
            {
                SetPropertyValue("UserDefine05", ref _userDefine05, value);
            }
        }


        #endregion

        #region UserDefine06
        private string _userDefine06;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine06
        {
            get
            {
                return _userDefine06;
            }
            set
            {
                SetPropertyValue("UserDefine06", ref _userDefine06, value);
            }
        }


        #endregion

        #region UserDefine07
        private string _userDefine07;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine07
        {
            get
            {
                return _userDefine07;
            }
            set
            {
                SetPropertyValue("UserDefine07", ref _userDefine07, value);
            }
        }


        #endregion

        #region UserDefine08
        private string _userDefine08;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine08
        {
            get
            {
                return _userDefine08;
            }
            set
            {
                SetPropertyValue("UserDefine08", ref _userDefine08, value);
            }
        }


        #endregion

        #region UserDefine09
        private string _userDefine09;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine09
        {
            get
            {
                return _userDefine09;
            }
            set
            {
                SetPropertyValue("UserDefine01", ref _userDefine09, value);
            }
        }


        #endregion

        #region UserDefine10
        private string _userDefine10;
        /// <summary>
        /// 邮编
        /// </summary>

        public string UserDefine10
        {
            get
            {
                return _userDefine10;
            }
            set
            {
                SetPropertyValue("UserDefine10", ref _userDefine10, value);
            }
        }


        #endregion

        #region CreatedBy
        private Hr.Employee _createdBy;
        /// <summary>
        /// 创建人
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Hr.Employee CreatedBy
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

        #region CreateTime
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

        #region UpdatedBy
        private Employee _updatedBy;
        /// <summary>
        /// 修改人
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee UpdatedBy
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

        #region UpdateTime
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

        #region ProducerCode
        private string _ProducerCode;
        public string ProducerCode
        {
            get
            {
                return _ProducerCode;
            }
            set
            {
                SetPropertyValue("ProducerCode", ref _ProducerCode, value);
            }
        }
        #endregion

        #region LicenseNumber
        private string _LicenseNumber;
        public string LicenseNumber
        {
            get
            {
                return _LicenseNumber;
            }
            set
            {
                SetPropertyValue("LicenseNumber", ref _LicenseNumber, value);
            }
        }
        #endregion

        #region METRCCode
        private string _METRCCode;
        public string METRCCode
        {
            get
            {
                return _METRCCode;
            }
            set
            {
                SetPropertyValue("METRCCode", ref _METRCCode, value);
            }
        }
        #endregion

        //#region TypeofUse
        //private TypeOfUse _TypeofUse;
        //public TypeOfUse TypeofUse
        //{
        //    get
        //    {
        //        return _TypeofUse;
        //    }
        //    set
        //    {
        //        SetPropertyValue("TypeofUse", ref _TypeofUse, value);
        //    }
        //}
        //#endregion

        //#region ClientClass
        //private Class _ClientClass;
        //public Class ClientClass
        //{
        //    get
        //    {
        //        return _ClientClass;
        //    }
        //    set
        //    {
        //        SetPropertyValue("ClientClass", ref _ClientClass, value);
        //    }
        //}
        //#endregion

        #region Logo
        private byte[] _logo;
        [ImageEditor(ListViewImageEditorCustomHeight = 5, DetailViewImageEditorFixedHeight = 10)]
        public byte[] Logo
        {
            get { return _logo; }
            set
            {
                SetPropertyValue("Logo", ref _logo, value);
            }
        }
        #endregion
        #region Category
        private CustomerCategory _Category;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public CustomerCategory fCategory
        {
            get { return _Category; }
            set { SetPropertyValue("fCategory", ref _Category, value); }
        }
        #endregion
        #region Classification
        private Classification _Classification;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public Classification Classification
        {
            get { return _Classification; }
            set { SetPropertyValue("Classification", ref _Classification, value); }
        }
        #endregion
        #region CreditRating
        private CreditRating _CreditRating;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public CreditRating CreditRating
        {
            get { return _CreditRating; }
            set { SetPropertyValue("CreditRating", ref _CreditRating, value); }
        }
        #endregion

        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [Association("Customer-Contacts")]
        [ImmediatePostData]
        public XPCollection<Contact> Contacts
        {
            get { return GetCollection<Contact>("Contacts"); }
        }
        // [Association("Customer-CRMQuote")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public XPCollection<CRMQuote> CRMQuote
        //{
        //    get { return GetCollection<CRMQuote>("CRMQuote"); }
        //}
        //[Association("Customer-ProductVersion")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public XPCollection<ProductVersion> ProductVersion
        //{
        //    get { return GetCollection<ProductVersion>("ProductVersion"); }
        //}

        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[Association("Customer-Payables")]
        //public XPCollection<Payables> Payables
        //{
        //    get { return GetCollection<Payables>("Payables"); }
        //}
        [Association("Customer-CallLog")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public XPCollection<CalLog> CallLog
        {
            get { return GetCollection<CalLog>("CallLog"); }
        }
        //[Association("Customer-CRMOrder")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public XPCollection<CRMOrder> CRMOrder
        //{
        //    get { return GetCollection<CRMOrder>("CRMOrder"); }
        //}
        //[Association("Customer-Invoice")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public XPCollection<Invoice> CRMInvoiceReportingContact
        //{
        //    get { return GetCollection<Invoice>("CRMInvoice"); }
        //}

        //[Association("Customer-CRMContact")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public XPCollection<CRMContact> CRMContact
        //{
        //    get { return GetCollection<CRMContact>("CRMContact"); }
        //}
        //[Association("Customer-Contract")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public XPCollection<CRMContracts> CRMContracts
        //{
        //    get { return GetCollection<CRMContracts>("CRMContracts"); }
        //}

        //#region ManyNotes
        //private Notes _ManyNotes;
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[DevExpress.Xpo.Association("Customers-Notes")]
        //public Notes ManyNotes
        //{
        //    get { return _ManyNotes; }
        //    set { SetPropertyValue("ManyNotes", ref _ManyNotes, value); }
        //}
        //#endregion


        //[Association("Customer-Payment")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public XPCollection<Payment> Payment
        //{
        //    get { return GetCollection<Payment>("Payment"); }
        //}
        //[Association("Customer-RemoteConnections")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public XPCollection<RemoteConnection> RemoteConnection
        //{
        //    get { return GetCollection<RemoteConnection>("RemoteConnection"); }
        //}

        [Association("Customer-Projects")]
        // [DataSourceCriteria("[Company] = CurrentCompanyOid()")]

        public XPCollection<Project> Projects
        {
            get
            {
                return GetCollection<Project>("Projects");
            }
        }

        [Association("Customer-Collectors")]
        public XPCollection<Collector> Collectors
        {
            get
            {
                return GetCollection<Collector>("Collectors");
            }
        }

        [VisibleInDetailView(false), VisibleInListView(false)]
        [Association("Customer-Notes")]
        public XPCollection<Notes> Notes
        {
            get { return GetCollection<Notes>("Notes"); }
        }

        [Association("Customer-InvoicingContact")]
        [ImmediatePostData]
        public XPCollection<InvoicingContact> InvoicingContact
        {
            get { return GetCollection<InvoicingContact>("InvoicingContact"); }
        }

        [Association("Customer-SampleSites")]
        public XPCollection<SampleSites> SampleSites
        {
            get { return GetCollection<SampleSites>("SampleSites"); }
        }

        [Association("Customer-ReportingContact")]
        [ImmediatePostData]
        public XPCollection<ReportingContact> ReportingContact
        {
            get { return GetCollection<ReportingContact>("ReportingContact"); }
        }

        //[Association("Customer-InvoicingAddress")]
        //public XPCollection<InvoicingAddress> InvoicingAddress
        //{
        //    get { return GetCollection<InvoicingAddress>("InvoicingAddress"); }
        //}
        ////[Association("Customer-SampleSites")]
        //public XPCollection<SampleSites> SampleSites
        //{
        //    get { return GetCollection<SampleSites>("SampleSites"); }
        //}
        private Project _Project;
        /// <summary>
        /// 客户
        /// </summary>
        [Association("Customer-Project")]
        // [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
        public Project Project
        {
            get
            {
                return _Project;
            }
            set
            {
                SetPropertyValue("Project", ref _Project, value);
            }
        }
        private int _CustomerId;

        //[Key]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int CustomerId { get { return _CustomerId; } set { SetPropertyValue<int>("CustomerId", ref _CustomerId, value); } }

        //[NotMapped]
        [SearchMemberOptions(SearchMemberMode.Exclude)]
        public string DisplayName
        {
            get { return ReflectionHelper.GetObjectDisplayText(this); }
        }
        private Employee _Owner;
        public Employee Owner { get { return _Owner; } set { SetPropertyValue<Employee>("Owner", ref _Owner, value); } }
        [InverseProperty("Customer")]
        public virtual IList<Prospects> Prospect { get; set; }

        //[InverseProperty("Customer")]
        //public virtual IList<Payables> Payables { get; set; }


        //[Association("Customer-CRMContact")]
        //public XPCollection<CRMContact> SampleSites
        //{
        //    get { return GetCollection<CRMContact>("SampleSites"); }
        //}

        //[Association("Customer-CRMContact")]
        //public XPCollection<CRMContact> SampleSites
        //{
        //    get { return GetCollection<CRMContact>("SampleSites"); }
        //}
        //private String _Name;
        //[VisibleInListView(false), VisibleInDetailView(false)]
        //[RuleRequiredField]
        //public String Name { get { return _Name; } set { SetPropertyValue<String>("Name", ref _Name, value); } }


        //private CustomerStatus status;
        //[ImmediatePostData]
        //public CustomerStatus Status
        //{
        //    get { return status; }
        //    set
        //    {
        //        if (status != value)
        //        {
        //            SetPropertyValue<CustomerStatus>("Status", ref status, value);
        //            OnPropertyChanged("Status");
        //        }
        //    }
        //}
        public Nullable<DateTime> LastContactDate
        {
            get
            {
                if (Notes != null && Notes.Count > 0)
                {
                    //UnFollowSettings settings = Session.FindObject<UnFollowSettings>(CriteriaOperator.Parse(""));
                    //if (settings != null)
                    //{
                    //    if (settings.MonitoringField == UnFollowedAccountMonitoringField.DateNoted)
                    //    {
                    //        return Note.Max(i => i.Date);
                    //    }
                    //    else
                    //    {
                    //        return Note.Max(i => i.FollowUpDate);
                    //    }
                    //}
                    //else
                    {
                        //return CreatedDate;
                        return UpdateTime;
                    }
                }
                else
                {
                    //return CreatedDate;
                    return UpdateTime;
                }
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public int NumberofUncontactedDays
        {
            get
            {
                if (LastContactDate != null)
                {
                    return DateTime.Today.Subtract((DateTime)LastContactDate).Days;
                }
                else
                {
                    return 0;
                }
            }
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public bool IsUnFollowed
        {
            get
            {
                UnFollowSettings settings = Session.FindObject<UnFollowSettings>(CriteriaOperator.Parse(""));
                if (settings != null && NumberofUncontactedDays >= settings.UnfollowedClient)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        private DateTime _ContractDate;
        public DateTime ContractDate
        {
            get { return _ContractDate; }
            set { SetPropertyValue("ContractDate", ref _ContractDate, value); }
        }
        private PreferredContactMethod _PreferredContactMethod;
        public PreferredContactMethod PreferredContactMethod { get { return _PreferredContactMethod; } set { SetPropertyValue<PreferredContactMethod>("PreferredContactMethod", ref _PreferredContactMethod, value); } }

        private decimal _CreditLimit;
        [VisibleInListView(false)]
        public decimal CreditLimit { get { return _CreditLimit; } set { SetPropertyValue<decimal>("CreditLimit", ref _CreditLimit, value); } }

        private bool _CreditHold;
        [VisibleInListView(false)]
        public bool CreditHold { get { return _CreditHold; } set { SetPropertyValue<bool>("CreditHold", ref _CreditHold, value); } }

        private bool _IsClose;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsClose { get { return _IsClose; } set { SetPropertyValue<bool>("IsClose", ref _IsClose, value); } }

        private string _CloseReason;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(int.MaxValue)]
        public string CloseReason { get { return _CloseReason; } set { SetPropertyValue<string>("CloseReason", ref _CloseReason, value); } }

        #region SourceLead
        private CRMProspects _SourceLead;
        [VisibleInListView(false)]
        public CRMProspects SourceLead { get { return _SourceLead; } set { SetPropertyValue("SourceLead", ref _SourceLead, value); } }
        #endregion

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { SetPropertyValue("StartDate", ref _StartDate, value); }
        }


        private DateTime _AcceptanceDate;

        public DateTime AcceptanceDate
        {
            get { return _AcceptanceDate; }
            set { SetPropertyValue("AcceptanceDate", ref _AcceptanceDate, value); }

        }

        private DateTime _ManintenanceStartDate;
        public DateTime ManintenanceStartDate
        {
            get { return _ManintenanceStartDate; }
            set { SetPropertyValue("ManintenanceStartDate", ref _ManintenanceStartDate, value); }
        }

        private DateTime _ManintenanceEndDate;
        public DateTime ManintenanceEndDate
        {
            get { return _ManintenanceEndDate; }
            set { SetPropertyValue("ManintenanceEndDate", ref _ManintenanceEndDate, value); }
        }

        private DateTime _ReminderDate;
        public DateTime ReminderDate
        {
            get { return _ReminderDate; }
            set { SetPropertyValue("ReminderDate", ref _ReminderDate, value); }

        }
        private PaymentTerms  _Terms;
        public PaymentTerms Terms
        {
            get { return _Terms; }
            set { SetPropertyValue("Terms", ref _Terms, value); }

        }

        #region Mail
        private bool _Mail;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false),VisibleInDashboards(false)]
        public bool Mail
        {
            get { return _Mail; }
            set { SetPropertyValue("Mail", ref _Mail, value); }

        }
        #endregion
        #region Email
        private bool _Email;
        [XafDisplayName("Email")]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public bool fEmail
        {
            get { return _Email; }
            set { SetPropertyValue("fEmail", ref _Email, value); }

        }
        #endregion
        ////private string _BillToStreet1;
        ////private string _BillToStreet2;
        ////private string _BillToCity;
        ////private string _BillToState;
        ////private string _BillToCountry;
        ////private string _BillToZip;

        ////public string BillToStreet1 { get { return _BillToStreet1; } set { SetPropertyValue<string>("BillToStreet1", ref _BillToStreet1, value); } }
        ////public string BillToStreet2 { get { return _BillToStreet2; } set { SetPropertyValue<string>("BillToStreet2", ref _BillToStreet2, value); } }
        ////public string BillToCity { get { return _BillToCity; } set { SetPropertyValue<string>("BillToCity", ref _BillToCity, value); } }
        ////public string BillToState { get { return _BillToState; } set { SetPropertyValue<string>("BillToState", ref _BillToState, value); } }
        ////public string BillToCountry { get { return _BillToCountry; } set { SetPropertyValue<string>("BillToCountry", ref _BillToCountry, value); } }
        ////public string BillToZip { get { return _BillToZip; } set { SetPropertyValue<string>("BillToZip", ref _BillToZip, value); } }

        ////private string _ShipToStreet1;
        ////private string _ShipToStreet2;
        ////private string _ShipToCity;
        ////private string _ShipToState;
        ////private string _ShipToCountry;
        ////private string _ShipToZip;

        ////public string ShipToStreet1 { get { return _ShipToStreet1; } set { SetPropertyValue<string>("ShipToStreet1", ref _ShipToStreet1, value); } }
        ////public string ShipToStreet2 { get { return _ShipToStreet2; } set { SetPropertyValue<string>("ShipToStreet2", ref _ShipToStreet2, value); } }
        ////public string ShipToCity { get { return _ShipToCity; } set { SetPropertyValue<string>("ShipToCity", ref _ShipToCity, value); } }
        ////public string ShipToState { get { return _ShipToState; } set { SetPropertyValue<string>("ShipToState", ref _ShipToState, value); } }
        ////public string ShipToCountry { get { return _ShipToCountry; } set { SetPropertyValue<string>("ShipToCountry", ref _ShipToCountry, value); } }
        ////public string ShipToZip { get { return _ShipToZip; } set { SetPropertyValue<string>("ShipToZip", ref _ShipToZip, value); } }



        //[NotMapped]
        //public decimal SaleAmount
        //{
        //    get
        //    {
        //        decimal amount = 0;
        //        foreach (Invoice invoice in Invoices)
        //        {
        //            if (invoice.Status == InvoiceStatus.Completed)
        //            {
        //                amount += invoice.Amount;
        //            }
        //        }
        //        return amount;
        //    }
        //}

        //public Nullable<DateTime> LastContactDate
        //{
        //    get
        //    {
        //        if (Notes != null && Notes.Count > 0)
        //        {
        //            //UnFollowSettings settings = Session.FindObject<UnFollowSettings>(CriteriaOperator.Parse(""));
        //            //if (settings != null)
        //            //{
        //            //    if (settings.MonitoringField == UnFollowedAccountMonitoringField.DateNoted)
        //            //    {
        //            //        return Note.Max(i => i.Date);
        //            //    }
        //            //    else
        //            //    {
        //            //        return Note.Max(i => i.FollowUpDate);
        //            //    }
        //            //}
        //            //else
        //            {
        //                //return CreatedDate;
        //                return UpdateTime;
        //            }
        //        }
        //        else
        //        {
        //            //return CreatedDate;
        //            return UpdateTime;
        //        }
        //    }
        //}

        //[VisibleInListView(false)]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //public int NumberofUncontactedDays
        //{
        //    get
        //    {
        //        if (LastContactDate != null)
        //        {
        //            return DateTime.Today.Subtract((DateTime)LastContactDate).Days;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    }
        //}

        //[VisibleInListView(false)]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //public bool IsUnFollowed
        //{
        //    get
        //    {
        //        UnFollowSettings settings = Session.FindObject<UnFollowSettings>(CriteriaOperator.Parse(""));
        //        if (settings != null && NumberofUncontactedDays >= settings.UnfollowedClient)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        //[InverseProperty("Customer")]
        //public virtual IList<Invoice> Invoices { get; set; }

        //[InverseProperty("BackRefCustomer")]
        //public virtual IList<Quotes> Quotes { get; set; }

        //[InverseProperty("BackRefCustomer")]
        //public virtual IList<Order> Orders { get; set; }

        //TODO: Move to controller
        //[Action(PredefinedCategory.View, Caption = "Activate...", AutoCommit = true,
        //    ConfirmationMessage = "This operation will set the selected object as Active.", 
        //    SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]//TargetObjectsCriteria = "Status!=##XCRM.Module.BusinessObjects.CustomerStatus,Active#",
        //public void Activate()
        //{
        //    Status = CustomerStatus.Active;
        //}

        //[Action(PredefinedCategory.View, Caption = "Deactivate...", AutoCommit = true,
        //    ConfirmationMessage = DeactivateConfirmationMessage, 
        //    SelectionDependencyType = MethodActionSelectionDependencyType.RequireMultipleObjects)]//TargetObjectsCriteria = "Status = ##XCRM.Module.BusinessObjects.CustomerStatus,Active#",
        //public void Deactivate()
        //{
        //    Status = CustomerStatus.Inactive;
        //}
        //        public const string DeactivateConfirmationMessage =
        //@"This action will set the object as inactive. There may be records in the system that continue to reference these inactive records.

        //Do you want to proceed?";

        //        public event PropertyChangedEventHandler PropertyChanged;
        //        protected void OnPropertyChanged(string propertyName)
        //        {
        //            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
        //            if (PropertyChanged != null)
        //            {
        //                PropertyChanged(this, args);
        //            }
        //       }
    }
}
