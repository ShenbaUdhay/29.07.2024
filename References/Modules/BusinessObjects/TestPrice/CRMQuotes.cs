using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Quotes;
using System;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Crm
{
    [DefaultClassOptions]

    [Appearance("ShowClient", AppearanceItemType = "ViewItem", TargetItems = "Client;", Criteria = "IsProspect = False", Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideClient", AppearanceItemType = "ViewItem", TargetItems = "Client;", Criteria = "IsProspect = True", Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("ShowProspectClient", AppearanceItemType = "ViewItem", TargetItems = "ProspectClient;", Criteria = "IsProspect =True", Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideprespectClient", AppearanceItemType = "ViewItem", TargetItems = "ProspectClient;", Criteria = "IsProspect = False", Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    //[RuleCriteria("ProspectClient", DefaultContexts.Save, "IsProspect=false", SkipNullOrEmptyValues = true,UsedProperties = "ProspectClient")]
    //[RuleCriteria("Client", DefaultContexts.Save, "IsProspect=True", SkipNullOrEmptyValues = true,UsedProperties = "ProspectClient")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CRMQuotes : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        QuotesInfo quotesinfo = new QuotesInfo();
        public CRMQuotes(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (string.IsNullOrEmpty(QuoteID))
            {
                Status = QuoteStatus.PendingSubmission;
            }
            QuotedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            QuotedDate = Library.GetServerTime(Session);
            Owner = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            if (string.IsNullOrEmpty(QuoteID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(QuoteID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(CRMQuotes), criteria, null)) + 1).ToString();
                var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "1")
                {
                    var predate = tempID.Substring(0, 6);
                    if (predate == curdate)
                    {
                        tempID = "QT" + tempID;
                    }
                    else
                    {
                        tempID = "QT" + curdate + "001";
                    }
                }
                else
                {
                    tempID = "QT" + curdate + "001";
                }
                QuoteID = tempID;
            }
        }
        public enum QuoteStatus
        {
            [XafDisplayName("Pending Submission")]
            PendingSubmission,
            [XafDisplayName("Pending Review")]
            PendingReview,
            Reviewed,
            Active,
            Expired,
            Canceled,
            QuoteSubmited

        }
        #region QuoteID
        private string _QuoteID;


        public string QuoteID
        {
            get { return _QuoteID; }
            set { SetPropertyValue(nameof(QuoteID), ref _QuoteID, value); }
        }
        #endregion

        #region Title
        private string _Title;
        [RuleRequiredField]
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue(nameof(Title), ref _Title, value); }
        }
        #endregion

        #region Client
        private Customer _Client;
        [ImmediatePostData(true)]
        //[RuleRequiredField]
        public Customer Client
        {
            get { return _Client; }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        }
        #endregion
        #region NonClient
        private string _NonClient;
        //[RuleRequiredField]
        [XafDisplayName("Client")]
        [NonPersistent]
        public string NonClient
        {
            get
            {
                if (IsProspect == true)
                {
                    if (ProspectClient != null)
                    {
                        return ProspectClient.Prospects;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (Client != null)
                    {
                        return Client.CustomerName;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            set { SetPropertyValue(nameof(NonClient), ref _NonClient, value); }
        }
        #endregion
        #region ProspectClient
        private CRMProspects _ProspectClient;
        //[RuleRequiredField]
        [XafDisplayName("Client")]
        [DataSourceProperty(nameof(ProsClientDS))]
        [ImmediatePostData]

        public CRMProspects ProspectClient
        {
            get { return _ProspectClient; }
            set { SetPropertyValue(nameof(ProspectClient), ref _ProspectClient, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<CRMProspects> ProsClientDS
        {
            get
            {
                if (IsProspect == true)
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

        #region IsProspect
        private bool _IsProspect;
        [ImmediatePostData(true)]
        public bool IsProspect
        {
            get { return _IsProspect; }
            set { SetPropertyValue(nameof(IsProspect), ref _IsProspect, value); }
        }
        #endregion

        #region ProjectID
        private Project _ProjectID;
        [DataSourceProperty("Project")]
        [ImmediatePostData(true)]

        public Project ProjectID
        {
            get { return _ProjectID; }
            set { SetPropertyValue(nameof(ProjectID), ref _ProjectID, value); }
        }
        #endregion

        #region Conotact DataSource Criteria
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData(true)]
        public XPCollection<Project> Project
        {
            get
            {
                if (IsProspect == false && Client != null && Client.Oid != null)
                {
                    XPCollection<Project> project = new XPCollection<Project>(Session, CriteriaOperator.Parse("[customername.Oid] = ? ", Client.Oid));
                    project.Where(proj => proj.customername.Oid == Client.Oid).ToList();
                    return project;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region ProjectName
        private string _ProjectName;
        [ImmediatePostData(true)]

        public string ProjectName
        {
            get
            {
                if (ProjectID != null && ProjectID.ProjectName != null)
                {
                    _ProjectName = ProjectID.ProjectName;
                }
                else
                {
                    _ProjectName = string.Empty;
                }
                return _ProjectName;
            }
            set { SetPropertyValue(nameof(ProjectName), ref _ProjectName, value); }
        }
        #endregion

        #region TAT
        private TurnAroundTime _TAT;
        public TurnAroundTime TAT
        {
            get { return _TAT; }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }
        #endregion

        #region QuotedDate
        private DateTime _QuotedDate;
        public DateTime QuotedDate
        {
            get { return _QuotedDate; }
            set { SetPropertyValue(nameof(QuotedDate), ref _QuotedDate, value); }
        }
        #endregion

        #region QuotedBy
        private Employee _QuotedBy;
        public Employee QuotedBy
        {
            get { return _QuotedBy; }
            set { SetPropertyValue(nameof(QuotedBy), ref _QuotedBy, value); }
        }
        #endregion

        #region QuotedAmount
        private decimal _QuotedAmount;
        public decimal QuotedAmount
        {
            get { return _QuotedAmount; }
            set { SetPropertyValue(nameof(QuotedAmount), ref _QuotedAmount, Math.Round(value, 2)); }
        }
        #endregion

        #region FinalAmount
        private decimal _FinalAmount;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal FinalAmount
        {
            get { return _FinalAmount; }
            set { SetPropertyValue(nameof(FinalAmount), ref _FinalAmount, value); }
        }
        #endregion

        #region PaymentMethod
        private PaymentMethod _PaymentMethod;
        public PaymentMethod PaymentMethod
        {
            get { return _PaymentMethod; }
            set { SetPropertyValue(nameof(PaymentMethod), ref _PaymentMethod, value); }
        }
        #endregion

        #region ExpirationDate
        private DateTime _ExpirationDate;
        [ImmediatePostData]
        [RuleRequiredField]
        public DateTime ExpirationDate
        {
            get { return _ExpirationDate; }
            set { SetPropertyValue(nameof(ExpirationDate), ref _ExpirationDate, value); }
        }
        #endregion

        #region Cancel 
        private bool _Cancel;
        [ImmediatePostData]
        [Appearance("QIsCancelHide", Visibility = ViewItemVisibility.Hide, Criteria = "IsNullOrEmpty([QuoteID])", Context = "DetailView")]
        [Appearance("QIsCancelShow", Visibility = ViewItemVisibility.Show, Criteria = "Not IsNullOrEmpty([QuoteID])", Context = "DetailView")]
        public bool Cancel
        {
            get
            {
                if (_Cancel == true)
                {
                    Status = QuoteStatus.Canceled;
                }
                return _Cancel;
            }
            set { SetPropertyValue(nameof(Cancel), ref _Cancel, value); }
        }
        #endregion

        #region CancelReason
        private string _CancelReason;
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [Size(3000)]
        public string CancelReason
        {
            get { return _CancelReason; }
            set { SetPropertyValue(nameof(CancelReason), ref _CancelReason, value); }
        }
        #endregion

        #region DateCanceled
        private DateTime _DateCanceled;
        [ImmediatePostData]
        [Appearance("QDateCanceledHide", Visibility = ViewItemVisibility.Hide, Criteria = "[Cancel] = False", Context = "DetailView")]
        [Appearance("QDateCanceledShow", Visibility = ViewItemVisibility.Show, Criteria = "[Cancel] = True", Context = "DetailView")]
        public DateTime DateCanceled
        {
            get
            {
                if (Cancel == true)
                {
                    _DateCanceled = Library.GetServerTime(Session);
                }
                else
                {
                    _DateCanceled = DateTime.MinValue;
                }
                return _DateCanceled;
            }
            set { SetPropertyValue(nameof(DateCanceled), ref _DateCanceled, value); }
        }
        #endregion

        ////#region CanceledBy
        ////private Employee _CanceledBy;
        ////[ImmediatePostData]
        ////[VisibleInDetailView(false),VisibleInListView(false),VisibleInLookupListView(false)]
        ////[Appearance("QCanceledByHide", Visibility = ViewItemVisibility.Hide, Criteria = "[Cancel] = False", Context = "DetailView")]
        ////[Appearance("QCanceledByShow", Visibility = ViewItemVisibility.Show, Criteria = "[Cancel] = True", Context = "DetailView")] 

        ////public Employee CanceledBy
        ////{
        ////    get {
        ////        if (Cancel == true)
        ////        {
        ////            _CanceledBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        ////        }
        ////        return _CanceledBy; }
        ////    set { SetPropertyValue(nameof(CanceledBy), ref _CanceledBy, value); }
        ////}
        ////#endregion


        #region ReactiveReason
        private string _ReactiveReason;
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [Size(3000)]
        public string ReactiveReason
        {
            get { return _ReactiveReason; }
            set { SetPropertyValue(nameof(ReactiveReason), ref _ReactiveReason, value); }
        }
        #endregion

        #region Remark
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        #endregion

        #region DetailedAmount
        private decimal _DetailedAmount;
        [ImmediatePostData]
        public decimal DetailedAmount
        {
            get
            {
                if (quotesinfo.lvDetailedPrice > 0)
                {
                    _DetailedAmount = quotesinfo.lvDetailedPrice;
                }
                return _DetailedAmount;
            }
            set { SetPropertyValue(nameof(DetailedAmount), ref _DetailedAmount, Math.Round(value, 2)); }
        }
        #endregion

        #region DiscountAmount
        private decimal _DiscountAmount;
        [ImmediatePostData]
        public decimal DiscountAmount
        {
            get { return _DiscountAmount; }
            set { SetPropertyValue(nameof(DiscountAmount), ref _DiscountAmount, Math.Round(value, 2)); }
        }
        #endregion

        #region 
        private bool _IsGobalDiscount;
        [ImmediatePostData]
        [NonPersistent]
        public bool IsGobalDiscount
        {
            get { return _IsGobalDiscount; }
            set { SetPropertyValue(nameof(IsGobalDiscount), ref _IsGobalDiscount, value); }
        }
        #endregion 

        #region Discount
        private decimal _Discount;
        [ImmediatePostData]
        public decimal Discount
        {
            get { return _Discount; }
            set { SetPropertyValue(nameof(Discount), ref _Discount, Math.Round(value, 2)); }
        }
        #endregion

        #region TotalAmount
        private decimal _TotalAmount;
        public decimal TotalAmount
        {
            get { return _TotalAmount; }
            set { SetPropertyValue(nameof(TotalAmount), ref _TotalAmount, Math.Round(value, 2)); }
        }
        #endregion

        #region PrimaryContact
        private Contact _PrimaryContact;
        [DataSourceProperty("ContactsDataSource")]
        [ImmediatePostData]
        public Contact PrimaryContact
        {
            get { return _PrimaryContact; }
            set { SetPropertyValue(nameof(PrimaryContact), ref _PrimaryContact, value); }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Contact> ContactsDataSource
        {
            get
            {
                if (IsProspect == false && Client != null && Client.CustomerName != null)
                {
                    return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.CustomerName] = ?", Client.CustomerName));
                }
                else if (IsProspect == true && ProspectClient != null && ProspectClient.PrimaryContact != null && ProspectClient.PrimaryContact.Oid != null)
                {
                    return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Oid] = ?", ProspectClient.PrimaryContact.Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region CellPhone
        private string _CellPhone;
        [ImmediatePostData(true)]
        public string CellPhone
        {
            get
            {
                if (string.IsNullOrEmpty(_CellPhone))
                {
                    if (PrimaryContact != null && PrimaryContact.MobilePhone != null)
                    {
                        _CellPhone = PrimaryContact.MobilePhone;
                    }
                    else
                    {
                        _CellPhone = string.Empty;
                    }
                }
                else
                {
                    return _CellPhone;
                }
                return _CellPhone;
            }
            set { SetPropertyValue(nameof(CellPhone), ref _CellPhone, value); }
        }
        #endregion

        #region OtherPhone
        private string _OtherPhone;
        [ImmediatePostData(true)]
        public string OtherPhone
        {
            get
            {
                if (string.IsNullOrEmpty(_OtherPhone))
                {
                    if (PrimaryContact != null && PrimaryContact.OtherPhone != null)
                    {
                        _OtherPhone = PrimaryContact.OtherPhone;
                    }
                    else
                    {
                        _OtherPhone = string.Empty;
                    }
                }
                else
                {
                    return _OtherPhone;
                }
                return _OtherPhone;
            }
            set { SetPropertyValue(nameof(OtherPhone), ref _OtherPhone, value); }
        }
        #endregion

        #region OfficePhone
        private string _OfficePhone;
        [ImmediatePostData(true)]
        public string OfficePhone
        {
            get
            {
                if (string.IsNullOrEmpty(_OfficePhone))
                {
                    if (PrimaryContact != null && PrimaryContact.OfficePhone != null)
                    {
                        _OfficePhone = PrimaryContact.OfficePhone;
                    }
                    else
                    {
                        _OfficePhone = string.Empty;
                    }
                }
                else
                {
                    return _OfficePhone;
                }
                return _OfficePhone;
            }
            set { SetPropertyValue(nameof(OfficePhone), ref _OfficePhone, value); }
        }
        #endregion


        #region EmailID
        private string _EmailID;
        [ImmediatePostData(true)]
        public string EmailID
        {
            get
            {
                //if (string.IsNullOrEmpty(_EmailID))
                //{
                if (PrimaryContact != null && PrimaryContact.Email != null)
                {
                    _EmailID = PrimaryContact.Email;
                }
                else
                {
                    _EmailID = string.Empty;
                }
                //}
                //else
                //{
                //    return _EmailID;
                //}
                return _EmailID;
            }
            set { SetPropertyValue(nameof(EmailID), ref _EmailID, value); }
        }
        #endregion

        #region BillStreet1
        private string _BillStreet1;
        [ImmediatePostData]
        public string BillStreet1
        {
            get
            {
                if (string.IsNullOrEmpty(_BillStreet1))
                {
                    if (IsProspect == false && Client != null && Client.Address != null)
                    {
                        _BillStreet1 = Client.Address;
                    }
                    else if (IsProspect == true && ProspectClient != null && ProspectClient.Street1 != null)
                    {
                        _BillStreet1 = ProspectClient.Street1;
                    }
                    else
                    {
                        _BillStreet1 = string.Empty;
                    }
                }
                else
                {
                    return _BillStreet1;
                }
                return _BillStreet1;
            }
            set { SetPropertyValue(nameof(BillStreet1), ref _BillStreet1, value); }
        }
        #endregion

        #region BillStreet2
        private string _BillStreet2;
        [ImmediatePostData]
        public string BillStreet2
        {
            get
            {
                if (string.IsNullOrEmpty(_BillStreet2))
                {
                    if (IsProspect == false && Client != null && Client.Address1 != null)
                    {
                        _BillStreet2 = Client.Address1;
                    }
                    else if (IsProspect == true && ProspectClient != null && ProspectClient.Street1 != null)
                    {
                        _BillStreet2 = ProspectClient.Street2;
                    }
                    else
                    {
                        _BillStreet2 = string.Empty;
                    }
                }
                else
                {
                    return _BillStreet2;
                }
                return _BillStreet2;
            }
            set { SetPropertyValue(nameof(BillStreet2), ref _BillStreet2, value); }
        }
        #endregion

        #region BillCity
        private string _BillCity;
        [ImmediatePostData]
        public string BillCity
        {
            get
            {
                if (string.IsNullOrEmpty(_BillCity))
                {
                    if (IsProspect == false && Client != null && Client.City != null)
                    {
                        _BillCity = Client.City.CityName;
                    }
                    else if (IsProspect == true && ProspectClient != null && ProspectClient.City != null)
                    {
                        _BillCity = ProspectClient.City.CityName;
                    }
                    else
                    {
                        _BillCity = string.Empty;
                    }
                }
                else
                {
                    return _BillCity;
                }
                return _BillCity;
            }
            set { SetPropertyValue(nameof(BillCity), ref _BillCity, value); }
        }
        #endregion

        #region BillState
        private string _BillState;
        [ImmediatePostData]
        public string BillState
        {
            get
            {
                if (string.IsNullOrEmpty(_BillState))
                {
                    if (IsProspect == false && Client != null && Client.State != null)
                    {
                        _BillState = Client.State.LongName;
                    }
                    else if (IsProspect == true && ProspectClient != null && ProspectClient.State != null)
                    {
                        _BillState = ProspectClient.State.LongName;
                    }
                    else
                    {
                        _BillState = string.Empty;
                    }
                }
                else
                {
                    return _BillState;
                }
                return _BillState;
            }
            set { SetPropertyValue(nameof(BillState), ref _BillState, value); }
        }
        #endregion

        #region BillZipCode
        private string _BillZipCode;
        [ImmediatePostData]
        public string BillZipCode
        {
            get
            {
                if (string.IsNullOrEmpty(_BillZipCode))
                {
                    if (IsProspect == false && Client != null && Client.Zip != null)
                    {
                        _BillZipCode = Client.Zip;
                    }
                    else if (IsProspect == true && ProspectClient != null && ProspectClient.Zip != null)
                    {
                        _BillZipCode = ProspectClient.Zip;
                    }
                    else
                    {
                        _BillZipCode = string.Empty;
                    }
                }
                else
                {
                    return _BillZipCode;
                }
                return _BillZipCode;
            }
            set { SetPropertyValue(nameof(BillZipCode), ref _BillZipCode, value); }
        }
        #endregion

        #region BillCountry
        private string _BillCountry;
        [ImmediatePostData]
        public string BillCountry
        {
            get
            {
                if (string.IsNullOrEmpty(_BillCountry))
                {
                    if (IsProspect == false && Client != null && Client.Country != null)
                    {
                        _BillCountry = Client.Country.EnglishLongName;
                    }
                    else if (IsProspect == true && ProspectClient != null && ProspectClient.Country != null)
                    {
                        _BillCountry = ProspectClient.Country.EnglishLongName;
                    }
                    else
                    {
                        _BillCountry = string.Empty;
                    }
                }
                else
                {
                    return _BillCountry;
                }
                return _BillCountry;
            }
            set { SetPropertyValue(nameof(BillCountry), ref _BillCountry, value); }
        }
        #endregion

        #region SameAddress
        private bool _SameAddress;
        [ImmediatePostData]
        public bool SameAddress
        {
            get { return _SameAddress; }
            set { SetPropertyValue(nameof(SameAddress), ref _SameAddress, value); }
        }
        #endregion

        #region ShipStreet1
        private string _ShipStreet1;
        [ImmediatePostData]
        public string ShipStreet1
        {
            get
            {
                if (BillStreet1 != null && SameAddress == true)
                {
                    _ShipStreet1 = BillStreet1;
                }
                return _ShipStreet1;
            }
            set { SetPropertyValue(nameof(ShipStreet1), ref _ShipStreet1, value); }
        }
        #endregion

        #region ShipStreet2
        private string _ShipStreet2;
        [ImmediatePostData]
        public string ShipStreet2
        {
            get
            {
                if (BillStreet2 != null && SameAddress == true)
                {
                    _ShipStreet2 = BillStreet2;
                }
                return _ShipStreet2;
            }
            set { SetPropertyValue(nameof(ShipStreet2), ref _ShipStreet2, value); }
        }
        #endregion

        #region ShipCity
        private string _ShipCity;
        [ImmediatePostData]
        public string ShipCity
        {
            get
            {
                if (BillCity != null && SameAddress == true)
                {
                    _ShipCity = BillCity;
                }
                return _ShipCity;
            }
            set { SetPropertyValue(nameof(ShipCity), ref _ShipCity, value); }
        }
        #endregion

        #region ShipState
        private string _ShipState;
        [ImmediatePostData]
        public string ShipState
        {
            get
            {
                if (BillState != null && SameAddress == true)
                {
                    _ShipState = BillState;
                }
                return _ShipState;
            }
            set { SetPropertyValue(nameof(ShipState), ref _ShipState, value); }
        }
        #endregion

        #region ShipZipCode
        private string _ShipZipCode;
        [ImmediatePostData]
        public string ShipZipCode
        {
            get
            {
                if (BillZipCode != null && SameAddress == true)
                {
                    _ShipZipCode = BillZipCode;
                }
                return _ShipZipCode;
            }
            set { SetPropertyValue(nameof(ShipZipCode), ref _ShipZipCode, value); }
        }
        #endregion

        #region ShipCountry
        private string _ShipCountry;
        [ImmediatePostData]
        public string ShipCountry
        {
            get
            {
                if (BillCountry != null && SameAddress == true)
                {
                    _ShipCountry = BillCountry;
                }
                return _ShipCountry;
            }
            set { SetPropertyValue(nameof(ShipCountry), ref _ShipCountry, value); }
        }
        #endregion

        #region Owner
        private Employee _Owner;
        public Employee Owner
        {
            get { return _Owner; }
            set { SetPropertyValue(nameof(Owner), ref _Owner, value); }
        }
        #endregion

        #region Credit
        private Credit _Credit;

        public Credit Credit
        {
            get { return _Credit; }
            set { SetPropertyValue(nameof(Credit), ref _Credit, value); }
        }
        #endregion

        #region SourceOpportunity
        private SourceOpportunity _SourceOpportunity;
        public SourceOpportunity SourceOpportunity
        {
            get { return _SourceOpportunity; }
            set { SetPropertyValue(nameof(SourceOpportunity), ref _SourceOpportunity, value); }
        }
        #endregion

        #region Status
        private QuoteStatus _Status;
        [ImmediatePostData]
        public QuoteStatus Status
        {
            get
            {
                if (ExpirationDate.Date != DateTime.MinValue.Date && ExpirationDate.Date < DateTime.Today)
                {
                    _Status = QuoteStatus.Expired;
                }
                return _Status;
            }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        #endregion

        #region RollBackReason
        private string _RollBackReason;
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [Size(3000)]
        public string RollBackReason
        {
            get { return _RollBackReason; }
            set { SetPropertyValue(nameof(RollBackReason), ref _RollBackReason, value); }
        }
        #endregion

        #region Note
        [Association("CRMQuotes-Note")]
        public XPCollection<Notes> Note
        {
            get { return GetCollection<Notes>("Note"); }
        }
        #endregion

        #region CRMProspect
        private CRMProspects _CRMProspect;
        [Association("CRMProspects_CRMQuotes")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public CRMProspects CRMProspect

        {
            get { return _CRMProspect; }
            set { SetPropertyValue(nameof(CRMProspect), ref _CRMProspect, value); }
        }
        #endregion

        #region Attachments
        [Association("CRMQuotes-Attachments")]
        public XPCollection<Attachment> Attachments
        {
            get { return GetCollection<Attachment>("Attachments"); }
        }
        #endregion

        #region ItemCharging
        [Association("CRMQuotes-ItemChargePricing")]
        public XPCollection<ItemChargePricing> ItemChargePricing
        {
            get { return GetCollection<ItemChargePricing>("ItemChargePricing"); }
        }
        #endregion

        #region QuotesItemChargePrice
        [Association("CRMQuotes-QuotesItemChargePrice")]
        public XPCollection<QuotesItemChargePrice> QuotesItemChargePrice
        {
            get { return GetCollection<QuotesItemChargePrice>("QuotesItemChargePrice"); }
        }
        #endregion

        #region AnalysisPricing
        [Association("CRMQuotes-AnalysisPricing")]
        [ImmediatePostData]
        public XPCollection<AnalysisPricing> AnalysisPricing
        {
            get { return GetCollection<AnalysisPricing>("AnalysisPricing"); }
        }
        #endregion
    }
}