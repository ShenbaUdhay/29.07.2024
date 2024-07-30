using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Accounting.Receivables;
using Modules.BusinessObjects.Accounts;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Invoicing;
using Modules.BusinessObjects.SuboutTracking;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    [DefaultProperty("Title")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //[RuleCombinationOfPropertiesIsUnique(id: "ContractNoteIsUnique", targetContexts: DefaultContexts.Save, targetProperties: "Title,Contract.Oid", CustomMessageTemplate = "A note with the same title already exists", TargetCriteria = "Contract is not null")]
    //[RuleCombinationOfPropertiesIsUnique(id: "TaskRegistrationNoteIsUnique", targetContexts: DefaultContexts.Save, targetProperties: "Title,TaskRegistration.Oid", CustomMessageTemplate = "A note with the same title already exists", TargetCriteria = "TaskRegistration is not null")]
    [RuleCombinationOfPropertiesIsUnique(id: "SampleRegistrationNoteIsUnique", targetContexts: DefaultContexts.Save, targetProperties: "Title,Samplecheckin.Oid", CustomMessageTemplate = "A note with the same title already exists", TargetCriteria = "Samplecheckin is not null")]
    [RuleCombinationOfPropertiesIsUnique(id: "CRMProspectsNoteIsUnique", targetContexts: DefaultContexts.Save, targetProperties: "Title,CRMProspects.Oid", CustomMessageTemplate = "A note with the same title already exists", TargetCriteria = "CRMProspects is not null")]
    [RuleCombinationOfPropertiesIsUnique(id: "CustomerNoteIsUnique", targetContexts: DefaultContexts.Save, targetProperties: "Title,Customer.Oid", CustomMessageTemplate = "A note with the same title already exists", TargetCriteria = "CRMProspects is not null")]
    public class Notes : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        CaseNarativeInfo CNInfo = new CaseNarativeInfo();
        public Notes(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Author = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            Date = DateTime.Now;
            if (objnavigationRefresh.ClickedNavigationItem == "SampleRegistration")
            {
                NoteSource = "Sample Registration";
                if (CNInfo.SCJobId != null)
                {
                    SourceID = CNInfo.SCJobId;
                }
                if (CNInfo.SCSampleMatries != null)
                {
                    NameSource = CNInfo.SCSampleMatries;
                }
            }
            else if (objnavigationRefresh.ClickedNavigationItem == "SamplePreparation")
            {
                NoteSource = "Sample Prepration";
                if (CNInfo.SPJobId != null)
                {
                    SourceID = CNInfo.SPJobId;
                }
                if (CNInfo.SPSampleMatries != null)
                {
                    NameSource = CNInfo.SPSampleMatries;
                }
            }
            else if (objnavigationRefresh.ClickedNavigationItem == "Result Entry")
            {
                NoteSource = "Result Entry";
                if (CNInfo.REQCBatchId != null)
                {
                    SourceID = CNInfo.REQCBatchId;
                }
                if (CNInfo.RESampleMatries != null)
                {
                    NameSource = CNInfo.RESampleMatries;
                }
            }
            else if (objnavigationRefresh.ClickedNavigationItem == "AnalysisQueue ")
            {
                NoteSource = "QC Batch";
                if (CNInfo.QCJobId != null)
                {
                    SourceID = CNInfo.QCJobId;
                }
                if (CNInfo.QCSampleMatries != null)
                {
                    NameSource = CNInfo.QCSampleMatries;
                }
            }
            else if (objnavigationRefresh.ClickedNavigationItem == "Custom Reporting")
            {
                NoteSource = "Custom Reporting";
                //if (CNInfo.QCJobId != null)
                //{
                //    SourceID = CNInfo.RpJobId;
                //}
                //if (CNInfo.QCSampleMatries != null)
                //{
                //    NameSource = CNInfo.RpSampleMatries;
                //}
            }
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            if (FollowUpDate < DateTime.Now && FollowUpDate != null && FollowUpDate != DateTime.MinValue)
            {
                Exception ex = new Exception("Please note that the 'Follow Up Date' must be greater than current date.");
                throw ex;
                return;
            }
        }

        private string _Title;
        [RuleRequiredField]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "A note with the same title already exists")]
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue(nameof(Title), ref _Title, value); }
        }
        private FileData _Attachment;
        public FileData Attachment
        {
            get { return _Attachment; }
            set { SetPropertyValue(nameof(Attachment), ref _Attachment, value); }
        }
        private string _Text;
        [RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string Text
        {
            get { return _Text; }
            set { SetPropertyValue(nameof(Text), ref _Text, value); }
        }

        private string _NoteSource;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public string NoteSource
        {
            get { return _NoteSource; }
            set { SetPropertyValue(nameof(NoteSource), ref _NoteSource, value); }
        }

        private string _NameSource;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public string NameSource
        {
            get { return _NameSource; }
            set { SetPropertyValue(nameof(NameSource), ref _NameSource, value); }
        }

        private string _SourceID;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public string SourceID
        {
            get { return _SourceID; }
            set { SetPropertyValue(nameof(SourceID), ref _SourceID, value); }
        }

        private Employee _Author;
        public Employee Author
        {
            get { return _Author; }
            set { SetPropertyValue(nameof(Author), ref _Author, value); }
        }
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue(nameof(Date), ref _Date, value); }
        }
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        //private Contract _Contract;
        //[DevExpress.Xpo.Association("Contract-Notes")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public Contract Contract
        //{
        //    get { return _Contract; }
        //    set { SetPropertyValue(nameof(_Contract), ref _Contract, value); }
        //}
        //private Tasks _TaskRegistration;
        //[Association("Tasks-Notes")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public Tasks TaskRegistration
        //{
        //    get { return _TaskRegistration; }
        //    set { SetPropertyValue(nameof(TaskRegistration), ref _TaskRegistration, value); }
        //}
        private Samplecheckin _Samplecheckin;
        [DevExpress.ExpressApp.DC.Aggregated, Association("Notes_SampleCheckIn")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Samplecheckin Samplecheckin
        {
            get { return _Samplecheckin; }
            set { SetPropertyValue(nameof(Samplecheckin), ref _Samplecheckin, value); }
        }

        private Invoicing _Invoicing;
        [DevExpress.ExpressApp.DC.Aggregated, Association("Notes_Invoice")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Invoicing Invoicing
        {
            get { return _Invoicing; }
            set { SetPropertyValue(nameof(Invoicing), ref _Invoicing, value); }
        }

        private PTStudyLog _PTStudyLog;
        [Association("Notes_PTStudyLog")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public PTStudyLog PTStudyLog
        {
            get { return _PTStudyLog; }
            set { SetPropertyValue(nameof(PTStudyLog), ref _PTStudyLog, value); }
        }
        //private ClientRequest _ClientRequest;
        //[Association("ClientRequest-Notes")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public ClientRequest ClientRequest
        //{
        //    get { return _ClientRequest; }
        //    set { SetPropertyValue(nameof(ClientRequest), ref _ClientRequest, value); }
        //}

        #region COCSettings
        private COCSettings _COCSettings;
        [Association("COCSettings-Note")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public COCSettings COCSettings
        {
            get { return _COCSettings; }
            set { SetPropertyValue(nameof(COCSettings), ref _COCSettings, value); }
        }
        #endregion
        #region Deposits
        private Deposits _Deposits;
        [Association("Deposits-Note")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Deposits Deposits
        {
            get { return _Deposits; }
            set { SetPropertyValue(nameof(Deposits), ref _Deposits, value); }
        }
        #endregion
        #region FollowUpDate
        private DateTime _FollowUpDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime FollowUpDate
        {
            get { return _FollowUpDate; }
            set { SetPropertyValue("FollowUpDate", ref _FollowUpDate, value); }
        }
        #endregion
        #region Invoice Collection
        [VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInListView(false)]
        [Association("Notes-InvoicingContact")]
        public XPCollection<InvoicingContact> InvoiceContacts
        {
            get { return GetCollection<InvoicingContact>("InvoiceContacts"); }
        }
        #endregion

        #region CRMProspects
        private CRMProspects _CRMProspects;
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.Association("CRMProspects-Notes")]
        public CRMProspects CRMProspects
        {
            get { return _CRMProspects; }
            set { SetPropertyValue("CRMProspects", ref _CRMProspects, value); }
        }
        #endregion

        #region Customer
        private Customer _Customer;
        [ImmediatePostData(true)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.Association("Customer-Notes")]
        public Customer Customer
        {
            get { return _Customer; }
            set { SetPropertyValue("Customer", ref _Customer, value); }
        }
        #endregion

        #region Contact
        private Contact _Contact;
        [DataSourceProperty("Contacts")]
        [ImmediatePostData(true)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Contact Contact
        {
            get
            {
                return _Contact;
            }
            set { SetPropertyValue("Contact", ref _Contact, value); }
        }
        #endregion
        #region Conotact DataSource Criteria
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Contact> Contacts
        {
            get
            {
                if (Customer != null && Customer.Oid != null)
                {
                    return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", Customer.Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region Phone
        private string _Phone;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Phone
        {
            get
            {
                if (Contact != null)
                {
                    _Phone = Contact.MobilePhone;
                }
                else if (CRMProspects != null)
                {
                    _Phone = CRMProspects.MobilePhone;
                }
                return _Phone;
            }
        }
        #endregion
        #region Email
        private string _Email;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Email
        {
            get
            {
                if (Contact != null)
                {
                    _Email = Contact.Email;
                }
                else if (CRMProspects != null)
                {
                    _Email = CRMProspects.Email;
                }
                return _Email;
            }

        }
        #endregion
        #region Employee
        [VisibleInDetailView(false), VisibleInListView(false)]
        [Association("Employee-Notes")]
        public XPCollection<Employee> Employee
        {
            get { return GetCollection<Employee>("Employee"); }
        }
        #endregion 


        #region ProspectClient
        private ProspectClient _ProspectClient;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.Association("ProspectClient-Note")]
        public ProspectClient ProspectClient
        {
            get { return _ProspectClient; }
            set { SetPropertyValue("ProspectClient", ref _ProspectClient, value); }
        }
        #endregion

        #region CRMQuotes
        private CRMQuotes _CRMQuotes;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.Association("CRMQuotes-Note")]
        public CRMQuotes CRMQuotes
        {
            get { return _CRMQuotes; }
            set { SetPropertyValue("CRMQuotes", ref _CRMQuotes, value); }
        }
        #endregion

        ////#region Vendors
        ////private Vendors _Vendors;
        ////[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        ////[DevExpress.Xpo.Association("Vendors-Notes")]
        ////public Vendors Vendors
        ////{
        ////    get { return _Vendors; }
        ////    set { SetPropertyValue("Vendors", ref _Vendors, value); }
        ////}
        ////#endregion

        #region CRMActivity
        private CRMActivity _CRMActivity;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.Association("CRMActivity-Notes")]

        public CRMActivity CRMActivity
        {
            get { return _CRMActivity; }
            set { SetPropertyValue("CRMActivity", ref _CRMActivity, value); }
        }
        #endregion



        //#region CRMProduct
        //private CRMProduct _CRMProduct;

        //[DevExpress.Xpo.Association("CRMProduct-Notes")]
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //public CRMProduct CRMProduct
        //{
        //    get { return _CRMProduct; }
        //    set { SetPropertyValue("CRMProduct", ref _CRMProduct, value); }
        //}
        //#endregion

        #region Activity
        private Activity _Activity;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DevExpress.Xpo.Association("Activity-Notes")]
        public Activity Activitys
        {
            get { return _Activity; }
            set { SetPropertyValue(nameof(Activitys), ref _Activity, value); }
        }
        #endregion

        //#region ProductVersion
        //private ProductVersion _ProductVersion;
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[DevExpress.Xpo.Association("ProductVersion-Notes")]

        //public ProductVersion ProductVersion
        //{
        //    get { return _ProductVersion; }
        //    set { SetPropertyValue("ProductVersion", ref _ProductVersion, value); }
        //}
        //#endregion

        private SubOutSampleRegistrations _SubOutSampleRegistration;
        [DevExpress.ExpressApp.DC.Aggregated, Association("Notes_SuboutSampleReg")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public SubOutSampleRegistrations SubOutSampleRegistration
        {
            get { return _SubOutSampleRegistration; }
            set { SetPropertyValue(nameof(SubOutSampleRegistration), ref _SubOutSampleRegistration, value); }
        }


        //private SamplingProposal _SamplingProposal;
        //[DevExpress.ExpressApp.DC.Aggregated, Association("Notes_SamplingProposal")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public SamplingProposal SamplingProposal
        //{
        //    get { return _SamplingProposal; }
        //    set { SetPropertyValue(nameof(SamplingProposal), ref _SamplingProposal, value); }
        //}

        private bool _IsCaseNarrative;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        public bool IsCaseNarrative
        {
            get { return _IsCaseNarrative; }
            set
            {
                SetPropertyValue("IsCaseNarrative", ref _IsCaseNarrative, value);
            }
        }
    }
}