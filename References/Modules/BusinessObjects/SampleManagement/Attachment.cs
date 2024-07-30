using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Accounting.Receivables;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.SuboutTracking;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.SampleManagement
{
    public enum Category
    {
        COC,
        Image,
        [XafDisplayName("Pdf Report")]
        PdfReport,
        [XafDisplayName("Result EDD")]
        ResultEDD,
        Other
    }
    [DefaultClassOptions]
    [DefaultProperty("Name")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //[RuleCombinationOfPropertiesIsUnique(id: "ContractAttachmentIsUnique", targetContexts: DefaultContexts.Save, targetProperties: "Name,Contract.Oid", CustomMessageTemplate = "An attachment with the same name already exists", TargetCriteria = "Contract is not null")]
    //[RuleCombinationOfPropertiesIsUnique(id: "TaskRegistrationAttachmentIsUnique", targetContexts: DefaultContexts.Save, targetProperties: "Name,TaskRegistration.Oid", CustomMessageTemplate = "An attachment with the same name already exists", TargetCriteria = "TaskRegistration is not null")]
    [RuleCombinationOfPropertiesIsUnique(id: "SampleRegistrationAttachmentIsUnique", targetContexts: DefaultContexts.Save, targetProperties: "Name,Samplecheckin.Oid", CustomMessageTemplate = "An attachment with the same name already exists", TargetCriteria = "Samplecheckin is not null")]
    public class Attachment : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Attachment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Operator = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            Date = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        private string _Name;
        [RuleRequiredField("NameAttachment",DefaultContexts.Save, "'Name must not be empty'")]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(nameof(Name), ref _Name, value); }
        }

        private Category _Category;
        [RuleRequiredField]
        public Category Category
        {
            get { return _Category; }
            set { SetPropertyValue(nameof(Category), ref _Category, value); }
        }

        private FileData _Attachment;
        [XafDisplayName("Attachment")]
        [RuleRequiredField]
        public FileData Attachments
        {
            get { return _Attachment; }
            set { SetPropertyValue(nameof(Attachments), ref _Attachment, value); }
        }
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue(nameof(_Date), ref _Date, value); }
        }
        private Employee _Operator;
        public Employee Operator
        {
            get { return _Operator; }
            set { SetPropertyValue(nameof(Operator), ref _Operator, value); }
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
        private string _Comment;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }


        //private Payables _Payables;
        //[DevExpress.Xpo.Association("Payables.Attachment")]
        //public Payables Payables
        //{
        //    get { return _Payables; }
        //    set { SetPropertyValue("Payables", ref _Payables, value); }
        //}



        //private VendorContract _VendorContract;
        //[DevExpress.Xpo.Association("VendorContract-Attachment")]
        //public VendorContract VendorContract
        //{
        //    get { return _VendorContract; }
        //    set { SetPropertyValue("VendorContract", ref _VendorContract, value); }
        //}


        //private CRMVendorInvoice _CRMVendorInvoice;
        //[DevExpress.Xpo.Association("CRMVendorInvoice-Attachment")]
        //public CRMVendorInvoice CRMVendorInvoice
        //{
        //    get { return _CRMVendorInvoice; }
        //    set { SetPropertyValue("CRMVendorInvoice", ref _CRMVendorInvoice, value); }
        //}


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

        //private Payment _Payment;
        //[DevExpress.Xpo.Association("Payment-Attachment")]
        //public Payment Payment
        //{
        //    get { return _Payment; }
        //    set { SetPropertyValue("Payment", ref _Payment, value); }
        //}


        //private Contract _Contract;
        //[DevExpress.Xpo.Association("Contract-Attachment")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public Contract Contract
        //{
        //    get { return _Contract; }
        //    set { SetPropertyValue(nameof(Contract), ref _Contract, value); }
        //}






        //private Tasks _TaskRegistration;
        //[Association("Tasks-Attachments")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public Tasks TaskRegistration
        //{
        //    get { return _TaskRegistration; }
        //    set { SetPropertyValue(nameof(TaskRegistration), ref _TaskRegistration, value); }
        //}

        private Samplecheckin _Samplecheckin;
        [Association("SampleRegistration-Attachments")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Samplecheckin Samplecheckin
        {
            get { return _Samplecheckin; }
            set { SetPropertyValue(nameof(Samplecheckin), ref _Samplecheckin, value); }
        }

        //private PTStudyLog _PTStudyLog;
        //[Association("Attachment-PTStudyLog")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public PTStudyLog PTStudyLog
        //{
        //    get { return _PTStudyLog; }
        //    set { SetPropertyValue(nameof(PTStudyLog), ref _PTStudyLog, value); }
        //}


        //private InstrumentParserLibrary _InstrumentParserLibrary;
        //[Association("InstrumentParserLibrary-Attachments")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public InstrumentParserLibrary InstrumentParserLibrary
        //{
        //    get { return _InstrumentParserLibrary; }
        //    set { SetPropertyValue(nameof(InstrumentParserLibrary), ref _InstrumentParserLibrary, value); }
        //}



        #region CRMQuotes
        private CRMQuotes _CRMQuotes;
        [Association("CRMQuotes-Attachments")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public CRMQuotes CRMQuotes
        {
            get { return _CRMQuotes; }
            set { SetPropertyValue(nameof(CRMQuotes), ref _CRMQuotes, value); }
        }
        #endregion

        #region PTStudyLog
        private PTStudyLog _PTStudyLog;
        [Association("Attachment-PTStudyLog")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public PTStudyLog PTStudyLog
        {
            get { return _PTStudyLog; }
            set { SetPropertyValue(nameof(PTStudyLog), ref _PTStudyLog, value); }
        }
        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        #endregion

        #region Labware-Attachment
        private Labware _Labware;
        [Association("Labware-Attachments")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Labware Labware
        {
            get { return _Labware; }
            set { SetPropertyValue(nameof(Labware), ref _Labware, value); }
        }
        #endregion

        //#region ClientRequest
        //private ClientRequest _ClientRequest;
        //[Association("ClientRequest-Attachments")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public ClientRequest ClientRequest
        //{
        //    get { return _ClientRequest; }
        //    set { SetPropertyValue(nameof(ClientRequest), ref _ClientRequest, value); }
        //}
        //#endregion

        #region COCSettings
        private COCSettings _COCSettings;
        [Association("COCSettings-Attachment")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public COCSettings COCSettings
        {
            get { return _COCSettings; }
            set { SetPropertyValue(nameof(COCSettings), ref _COCSettings, value); }
        }
        #endregion
        #region DepositPayment
        private DepositPayment _DepositPayment;
        [Association("DepositPayment-Attachment")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DepositPayment DepositPayment
        {
            get { return _DepositPayment; }
            set { SetPropertyValue(nameof(DepositPayment), ref _DepositPayment, value); }
        }
        #endregion

        private SubOutSampleRegistrations _SuboutSampleRegistration;
        [Association("SuboutSampleRegistration-Attachments")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public SubOutSampleRegistrations SuboutSampleRegistration
        {
            get { return _SuboutSampleRegistration; }
            set { SetPropertyValue(nameof(SuboutSampleRegistration), ref _SuboutSampleRegistration, value); }
        }
        //#region SamplingProposal
        //private SamplingProposal _SamplingProposal;
        //[Association("SamplingProposal-Attachments")]
        //[VisibleInDetailView(false)]
        //[VisibleInLookupListView(false)]
        //[VisibleInListView(false)]
        //public SamplingProposal SamplingProposal
        //{
        //    get { return _SamplingProposal; }
        //    set { SetPropertyValue(nameof(SamplingProposal), ref _SamplingProposal, value); }
        //}
        //#endregion
    }
}