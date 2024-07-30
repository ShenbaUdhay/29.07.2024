using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using Modules.BusinessObjects.Accounting.Receivables;
//using Modules.BusinessObjects.ContractManagement;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Seting;
using Modules.BusinessObjects.Setting;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Accounts
{
    [DefaultClassOptions]
    [NavigationItem(true, GroupName = "Sales")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCombinationOfPropertiesIsUnique("ValTopic", DefaultContexts.Save, "Topic,Prospects", SkipNullOrEmptyValues = false)]
    [ImageName("BO_Opportunity")]
    public class CRMProspects : Prospects, /*IQuoteOpportunity, *//* ISaleStageHistoryTarget,*/ IOrderOpportunity, IXafEntityObject, IObjectSpaceLink
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CRMProspects(Session session)
            : base(session)
        {
            // Notes = new List<Note>();
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            Owner = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);


            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
            //if (SourceLead != null)
            //{
            //    SourceLead.Status = LeadStatus.None;
            //}
        }

        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        //[DevExpress.ExpressApp.DC.Aggregated]
        //public virtual IList<Note> Notes { get; set; }

        #region ILeadTarget
        //private CRMLead _SourceLead;

        ////[DataSourceProperty("Status=Qualified", DataSourcePropertyIsNullMode.SelectAll)]
        ////[DataSourceCriteria("Position.Title = 'Qualified' AND Oid != '@This.Oid'")]
        ////[RuleCriteria("Lead.Status==LeadStatus.QUalified")]
        //[VisibleInListView(false)]
        //public virtual CRMLead SourceLead { get { return _SourceLead; } set { SetPropertyValue<CRMLead>("SourceLead", ref _SourceLead, value); } }

        private DateTime _FollowUpDate;
        public DateTime FollowUpDate
        {
            get { return _FollowUpDate; }
            set { SetPropertyValue("FollowUpDate", ref _FollowUpDate, value); }
        }

        #endregion

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { SetPropertyValue("Address", ref _Address, value); }
        }


        private string _Account;
        public string Account
        {
            get { return _Account; }
            set { SetPropertyValue("Account", ref _Account, value); }
        }



        private string _Prospects;
        [RuleRequiredField]
        public string Prospects
        {
            get { return _Prospects; }
            set
            {
                if (value == null)
                { value = string.Empty; }
                SetPropertyValue("Prospects", ref _Prospects, value.Trim());
            }
        }


        #region IGenericAddressableSale
        //private XCRMAddress _BillToAddress;
        //[DevExpress.ExpressApp.DC.Aggregated]
        //[VisibleInListView(false)]
        //public virtual XCRMAddress BillToAddress { get { return _BillToAddress; } set { SetPropertyValue<XCRMAddress>("BillToAddress",ref _BillToAddress,value); } }
        //private string _BillToStreet1;
        //private string _BillToStreet2;
        //private City _BillToCity;
        //private CustomState _BillToState;
        //private CustomCountry _BillToCountry;
        //private string _BillToZip;

        //public string BillToStreet1 { get { return _BillToStreet1; } set { SetPropertyValue<string>("BillToStreet1", ref _BillToStreet1, value); } }
        //public string BillToStreet2 { get { return _BillToStreet2; } set { SetPropertyValue<string>("BillToStreet2", ref _BillToStreet2, value); } }
        //public City BillToCity { get { return _BillToCity; } set { SetPropertyValue<City>("BillToCity", ref _BillToCity, value); } }
        //public CustomState BillToState { get { return _BillToState; } set { SetPropertyValue<CustomState>("BillToState", ref _BillToState, value); } }
        //public CustomCountry BillToCountry { get { return _BillToCountry; } set { SetPropertyValue<CustomCountry>("BillToCountry", ref _BillToCountry, value); } }
        //public string BillToZip { get { return _BillToZip; } set { SetPropertyValue<string>("BillToZip", ref _BillToZip, value); } }

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
        private KeyValue _Industry;
        public KeyValue Industry

        {
            get { return _Industry; }
            set { SetPropertyValue("Industry", ref _Industry, value); }
        }


        private CustomerCategory _Category;
        public CustomerCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue("Category", ref _Category, value); }
        }

        private string _ClientCode;
        public string ClientCode

        {
            get { return _ClientCode; }
            set { SetPropertyValue("ClientCode", ref _ClientCode, value); }
        }

        private string _ClientNumber;
        public string ClientNumber

        {
            get { return _ClientNumber; }
            set { SetPropertyValue("ClientNumber", ref _ClientNumber, value); }
        }

        //private XCRMAddress _ShipToAddress;
        //[DevExpress.ExpressApp.DC.Aggregated]
        //[VisibleInListView(false)]
        //public virtual XCRMAddress ShipToAddress { get { return _ShipToAddress; } set { SetPropertyValue<XCRMAddress>("ShipToAddress",ref _ShipToAddress,value); } }

        private Nullable<DateTime> _DeliveryDate;
        [VisibleInListView(false)]
        public Nullable<DateTime> DeliveryDate { get { return _DeliveryDate; } set { SetPropertyValue<Nullable<DateTime>>("DeliveryDate", ref _DeliveryDate, value); } }

        [VisibleInListView(false)]
        public Modules.BusinessObjects.Accounts.ShippingMethod? ShippingMethod { get; set; }
        #endregion

        #region ISaleStageHistoryTarget
        //[System.ComponentModel.Browsable(false)]
        //public SaleStage SaleStage
        //{
        //    get { return SaleStage.Opportunity; }
        //}

        //[System.ComponentModel.Browsable(false)]
        //public ISaleStageHistory History
        //{
        //    get { return ISaleStageHistoryTargetLogic.GetHistory(this); }
        //}

        [VisibleInDetailView(false), VisibleInListView(false)]
        [DevExpress.Xpo.Association("CRMProspects-Notes")]
        public XPCollection<Notes> Notes
        {
            get { return GetCollection<Notes>("Notes"); }
        }


        [DevExpress.Xpo.Association("CRMProspects-CallLog")]
        public XPCollection<CalLog> CallLogs
        {
            get { return GetCollection<CalLog>("CallLogs"); }
        }

        private DateTime _DemoDate;
        public DateTime DemoDate
        {
            get { return _DemoDate; }
            set { SetPropertyValue("DemoDate", ref _DemoDate, value); }
        }

        private DateTime _QuoteDate;
        public DateTime QuoteDate
        {
            get { return _QuoteDate; }
            set { SetPropertyValue("QuoteDate", ref _QuoteDate, value); }
        }

        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }

        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }

        private DateTime _ModifiedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }





        private Topic _Topic;
        [RuleRequiredField]
        public Topic Topic
        {
            get
            {
                return _Topic;
            }
            set
            {
                SetPropertyValue(nameof(Topic), ref _Topic, value);

            }
        }
        private Employee _ModifiedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }
        }
        public IObjectSpace ObjectSpace { get; set; }
        //XCRMAddress IGenericAddressableSale.BillToAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //XCRMAddress IGenericAddressableSale.ShipToAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #endregion

        #region IXafEntityObject
        public override void OnLoaded()
        {
            base.OnLoaded();
        }
        public override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            //#region ISaleStageHistoryTarget
            //ISaleStageHistoryTargetLogic.UpdateHistory(this, ObjectSpace);
            //#endregion
        }
        public override void OnCreated()
        {
            base.OnCreated();
            #region IGenericAddressableSale
            //if (BillToAddress == null)
            //{
            //    BillToAddress = ObjectSpace.CreateObject<XCRMAddress>();
            //}
            //if (ShipToAddress == null)
            //{
            //    ShipToAddress = ObjectSpace.CreateObject<XCRMAddress>();
            //}
            #endregion
        }
        #endregion


        #region IAddressable
        //private XCRMAddress _PrimaryAddress;
        //[DevExpress.ExpressApp.DC.Aggregated]
        //[VisibleInListView(false)]
        //public virtual XCRMAddress PrimaryAddress { get { return _PrimaryAddress; } set { SetPropertyValue<XCRMAddress>("PrimaryAddress",ref _PrimaryAddress,value); } }

        #region City
        private City _city;
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

        #region State
        private CustomState _state;
        [ImmediatePostData(true)]
        //[DataSourceProperty("StateDataSource")]
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

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<CustomState> StateDataSource
        {
            get
            {
                if (City != null)
                {
                    City customcity = Session.FindObject<City>(CriteriaOperator.Parse("[CityName] = ?", City.CityName));
                    if (customcity != null && customcity.CityName != null && customcity.State != null)
                    {
                        XPCollection<CustomState> customstate = new XPCollection<CustomState>(Session, CriteriaOperator.Parse("[Oid] = ?", customcity.State.Oid));
                        if (customstate != null && customstate.Count > 0)
                        {
                            return customstate;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
        }
        #region Country
        private CustomCountry _country;
        [ImmediatePostData]
        //[DataSourceProperty("CountryDataSource")]
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
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<CustomCountry> CountryDataSource
        {
            get
            {
                if (State != null)
                {
                    CustomState customstate = Session.FindObject<CustomState>(CriteriaOperator.Parse("[LongName] = ?", State.LongName));
                    if (customstate != null && customstate.LongName != null && customstate.Country != null)
                    {
                        XPCollection<CustomCountry> customcountry = new XPCollection<CustomCountry>(Session, CriteriaOperator.Parse("[Oid] = ?", customstate.Country.Oid));
                        if (customcountry != null && customcountry.Count > 0)
                        {
                            return customcountry;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        private string _Street1;
        private string _Street2;
        private string _Zip;

        public string Street1 { get { return _Street1; } set { SetPropertyValue<string>("Street1", ref _Street1, value); } }
        public string Street2 { get { return _Street2; } set { SetPropertyValue<string>("Street2", ref _Street2, value); } }
        public string Zip { get { return _Zip; } set { SetPropertyValue<string>("Zip", ref _Zip, value); } }

        //public void CopyTo(IAddressable target)
        //{
        //    IAddressableLogic.Copy(this, target);
        //}
        #endregion



        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue("Remark", ref _Remark, value); }
        }



        #region IPhones
        private string _OtherPhone;
        private string _MobilePhone;
        private string _OfficePhone;
        private string _HomePhone;
        private string _Fax;
        public string OtherPhone { get { return _OtherPhone; } set { SetPropertyValue<string>("OtherPhone", ref _OtherPhone, value); } }
        public string MobilePhone { get { return _MobilePhone; } set { SetPropertyValue<string>("MobilePhone", ref _MobilePhone, value); } }
        public string OfficePhone { get { return _OfficePhone; } set { SetPropertyValue<string>("OfficePhone", ref _OfficePhone, value); } }
        public string HomePhone { get { return _HomePhone; } set { SetPropertyValue<string>("HomePhone", ref _HomePhone, value); } }
        public string Fax { get { return _Fax; } set { SetPropertyValue<string>("Fax", ref _Fax, value); } }

        //public void CopyTo(IPhones targetPhones)
        //{
        //    IPhonesLogic.Copy(this, targetPhones);
        //}
        #endregion

        #region IGenericEmail
        [VisibleInListView(false)]

        private string _Email;
        public string Email { get { return _Email; } set { SetPropertyValue<string>("Email", ref _Email, value); } }

        #endregion

        private string _WebSite;

        [VisibleInListView(false)]
        public string WebSite { get { return _WebSite; } set { SetPropertyValue<string>("WebSite", ref _WebSite, value); } }

        private Region _Region;
        public Region Region
        {
            get { return _Region; }
            set { SetPropertyValue("Region", ref _Region, value); }
        }



        [Association("CRMProspects_CRMQuotes")]
        public XPCollection<CRMQuotes> Quote
        {
            get { return GetCollection<CRMQuotes>(nameof(Quote)); }
        }

        public Nullable<DateTime> LastContactDate
        {
            get
            {
                if (Notes != null && Notes.Count > 0)
                {
                    //DefaultSettings settings = Session.FindObject<DefaultSettings>(CriteriaOperator.Parse(""));
                    //if (settings != null)
                    //{
                    //    if (settings.MonitoringField == UnFollowedAccountMonitoringField.DateNoted)
                    //    {
                    //        return Notes.Max(i => i.Date);
                    //    }
                    //    else
                    //    {
                    //        return Notes.Max(i => i.FollowUpDate);
                    //    }
                    //}
                    //else
                    {
                        //return CreatedDate;
                        return ModifiedDate;
                    }
                }
                else
                {
                    //return CreatedDate;
                    return ModifiedDate;
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

        private Contact _PrimaryContact;
        //[Appearance("NotEnableOpportunityPrimaryContact", Enabled = false, Criteria = "Customer is null ", Context = "DetailView")]
        //[Appearance("EnableOpportunityPrimaryContact", Enabled = true, Criteria = "Customer is not null ", Context = "DetailView")]
        ////[DataSourceProperty(nameof(PrimaryContacts), DataSourcePropertyIsNullMode.SelectNothing)]
        //[DataSourceCriteria("[Status] = 'Active'")]
        [ImmediatePostData]
        public /*virtual*/ Contact PrimaryContact
        {
            get
            {
                return _PrimaryContact;
            }
            set
            {
                SetPropertyValue<Contact>("PrimaryContact", ref _PrimaryContact, value);
            }
        }

        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public XPCollection<Contact> PrimaryContacts
        {
            get
            {
                //return new XPCollection<CRMContact>(Session, CriteriaOperator.Parse("[Opportunity]=? Or [SourceLead]=?",Oid,SourceLead));
                //return new XPCollection<CRMContact>(Session, CriteriaOperator.Parse("[Opportunity]=? Or [SourceLead]=?", Oid, SourceLead));
                return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer]=?", Customer));
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
                if (settings != null && NumberofUncontactedDays >= settings.UnfollowedProspect)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}