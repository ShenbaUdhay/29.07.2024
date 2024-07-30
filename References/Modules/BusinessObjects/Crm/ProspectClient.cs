using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Seting;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.Crm
{
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("Prospectclient", DefaultContexts.Save, "CustomerName", SkipNullOrEmptyValues = false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProspectClient : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ProspectClient(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region ClientCode
        private string _clientCode;
        /// <summary>
        /// ´«Õæ
        /// </summary>
        [Size(50)]
        [ImmediatePostData]
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

        #region Company
        private Company _company;
        /// <summary>
        /// ¹«Ë¾
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
        /// Ãû³Æ
        /// </summary>
        [Size(500)]
        [RuleRequiredField("ProspectClient.CustomerName", DefaultContexts.Save)]
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
        /// ´«Õæ
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
        /// ´«Õæ
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
        /// µ¥Î»ÀàÐÍ
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

        #region ÄêÊÕÈë
        private decimal _annualRevenue;
        /// <summary>
        /// ÄêÊÕÈë
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

        #region ¹ÍÔ±ÈËÊý
        private int _numberOfEmployees;
        /// <summary>
        /// ¹ÍÔ±ÈËÊý
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
        /// ÐÐÒµ
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
        /// ·ÖÇø£¨ÖÝ£©
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

        #region Category
        private KeyValue _category;
        /// <summary>
        /// ·ÖÀà£¨³ÏÐÅ£©
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid() and KeyType.TypeNumber='19'")]
        public KeyValue Category
        {
            get
            {
                return _category;
            }
            set
            {
                SetPropertyValue("Category", ref _category, value);
            }
        }
        #endregion

        #region PrimaryContact
        private Contact _primaryContact;
        /// <summary>
        /// Ö÷ÒªÁªÏµÈË
        /// </summary>
        [DataSourceCriteria("[Company] = CurrentCompanyOid()")]
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
        #endregion

        #region °ì¹«µç»°
        private string _officePhone;
        /// <summary>
        /// °ì¹«µç»°
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

        #region ÆäËüµç»°
        private string _otherPhone;
        /// <summary>
        /// ÆäËüµç»°
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

        #region WebSite
        private string _webSite;
        /// <summary>
        /// ÍøÕ¾
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
        /// ´«Õæ
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
        /// ÓÊÏä
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
        /// ¹ú¼Ò
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
        /// Ê¡
        /// </summary>
        //[RuleRequiredField("Customer.State", DefaultContexts.Save)]
        [ImmediatePostData(true)]
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

        #region City
        private City _city;
        /// <summary>
        /// ÊÐ
        /// </summary>
        //[RuleRequiredField("Customer.City", DefaultContexts.Save)]
        [ImmediatePostData(true)]
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

        #region Area
        private Area _area;
        /// <summary>
        /// Çø
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

        #region µØÖ·
        private string _address;
        /// <summary>
        /// µØÖ·
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
        /// µØÖ·
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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

        #region County
        private string _county;
        /// <summary>
        /// ÓÊ±à
        /// </summary>

        public string County
        {
            get
            {
                return _county;
            }
            set
            {
                SetPropertyValue("County", ref _county, value);
            }
        }
        #endregion

        #region SiteMap
        private string _siteMap;
        /// <summary>
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        /// ÓÊ±à
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
        private CustomSystemUser _createdBy;
        /// <summary>
        /// ´´½¨ÈË
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

        #region CreateTime
        private DateTime _createTime;
        /// <summary>
        /// ´´½¨Ê±¼ä
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
        private CustomSystemUser _updatedBy;
        /// <summary>
        /// ÐÞ¸ÄÈË
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

        #region UpdateTime
        private DateTime _updateTime;
        /// <summary>
        /// ÐÞ¸ÄÊ±¼ä
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

        #region TypeofUse
        private TypeOfUse _TypeofUse;
        public TypeOfUse TypeofUse
        {
            get
            {
                return _TypeofUse;
            }
            set
            {
                SetPropertyValue("TypeofUse", ref _TypeofUse, value);
            }
        }
        #endregion

        ////#region ClientClass
        ////private Class _ClientClass;
        ////public Class ClientClass
        ////{
        ////    get
        ////    {
        ////        return _ClientClass;
        ////    }
        ////    set
        ////    {
        ////        SetPropertyValue("ClientClass", ref _ClientClass, value);
        ////    }
        ////}
        ////#endregion

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

        [Association("ProspectClient-Contacts")]
        public XPCollection<Contact> ProspectContacts
        {
            get { return GetCollection<Contact>("ProspectContacts"); }
        }

        [Association("ProspectClient-Projects")]
        // [DataSourceCriteria("[Company] = CurrentCompanyOid()")]

        public XPCollection<Project> ProspectProjects
        {
            get
            {
                return GetCollection<Project>("ProspectProjects");
            }
        }
        [Association("ProspectClient-Note")]
        public XPCollection<Notes> ProspectNote
        {
            get { return GetCollection<Notes>("ProspectNote"); }
        }

    }
}