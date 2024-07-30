using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SuboutTracking
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SubOutContractLab : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SubOutContractLab(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        #region ContractLabName
        private string _ContractLabName;
        [RuleUniqueValue]
        [RuleRequiredField("ContractLab", DefaultContexts.Save,"'Contract Lab'must not be empty")]
        public string ContractLabName
        {
            get
            {
                return _ContractLabName;
            }
            set { SetPropertyValue("ContractLabName", ref _ContractLabName, value.Trim()); }
        }
        #endregion 


        //[System.ComponentModel.Browsable(false)]
        //[InverseProperty("SubOutSampleRegistration")]
        //public Customer Customer
        //{
        //    get { return customer; }
        //    set { SetPropertyValue<Customer>("Customer", ref customer, value); PotentialCustomer = Customer; }
        //}



        //private SuboutSampleRegistration _ContractLabName;
        //[DevExpress.Xpo.Association("SubOutSampleRegistration-SubOutContract")]
        //public SuboutSampleRegistration ContractLabName
        //{
        //    get { return _ContractLabName; }
        //    set { SetPropertyValue("ContractLabName", ref _ContractLabName, value); }
        //}
        #region AccreditationID
        private string _AccreditationID;
        public string AccreditationID
        {
            get
            {
                return _AccreditationID;
            }
            set { SetPropertyValue("AccreditationID", ref _AccreditationID, value); }
        }
        #endregion 

        #region Zip
        private string _Zip;
        public string Zip
        {
            get
            {
                return _Zip;
            }
            set { SetPropertyValue("Zip", ref _Zip, value); }
        }
        #endregion 

        #region Address1
        private string _Address1;
        public string Address1
        {
            get
            {
                return _Address1;
            }
            set { SetPropertyValue("Address1", ref _Address1, value); }
        }
        #endregion 

        #region Address2
        private string _Address2;
        public string Address2
        {
            get
            {
                return _Address2;
            }
            set { SetPropertyValue("Address2", ref _Address2, value); }
        }
        #endregion 

        #region City
        private string _City;
        public string City
        {
            get
            {
                return _City;
            }
            set { SetPropertyValue("City", ref _City, value); }
        }
        #endregion 

        #region State
        private string _State;
        public string State
        {
            get
            {
                return _State;
            }
            set { SetPropertyValue("State", ref _State, value); }
        }
        #endregion 

        #region Country
        private string _Country;
        public string Country
        {
            get
            {
                return _Country;
            }
            set { SetPropertyValue("Country", ref _Country, value); }
        }
        #endregion 

        #region WebSite
        private string _WebSite;
        public string WebSite
        {
            get
            {
                return _WebSite;
            }
            set { SetPropertyValue("WebSite", ref _WebSite, value); }
        }
        #endregion 

        #region Certificate
        private FileData _Certificate;
        public FileData Certificate
        {
            get { return _Certificate; }
            set { SetPropertyValue(nameof(Certificate), ref _Certificate, value); }
        }
        #endregion

        #region Deactivate
        private bool _Deactivate;
        public bool Deactivate
        {
            get { return _Deactivate; }
            set { SetPropertyValue(nameof(Deactivate), ref _Deactivate, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        #region Contact
        private string _Contact;
        public string Contact
        {
            get
            {
                return _Contact;
            }
            set { SetPropertyValue("Contact", ref _Contact, value); }
        }
        #endregion

        #region Phone
        private string _Phone;
        public string Phone
        {
            get
            { return _Phone; }
            set { SetPropertyValue("Phone", ref _Phone, value); }
        }
        #endregion

        #region EmailID
        private string _EmailID;
        public string EmailID
        {
            get
            {
                return _EmailID;
            }
            set { SetPropertyValue("EmailID", ref _EmailID, value); }
        }
        #endregion

        #region CertifiedTests
        [Association("SubOutContractLab-CertifiedTests")]
        [ImmediatePostData]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public XPCollection<CertifiedTests> CertifiedTests
        {
            get { return GetCollection<CertifiedTests>("CertifiedTests"); }
        }
        #endregion
    }
}