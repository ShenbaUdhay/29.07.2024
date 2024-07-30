using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Reporting : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Reporting(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            NewReportFormat = true;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            //DateReceived = Library.GetServerTime(Session);
            if (string.IsNullOrEmpty(ReportID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(ReportID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(Reporting), criteria, null)) + 1).ToString("000");
                var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "001")
                {
                    var predate = tempID.Substring(0, 6);
                    if (predate == curdate)
                    {
                        tempID = "RP" + tempID;
                    }
                    else
                    {
                        tempID = "RP" + curdate + "001";
                    }
                }
                else
                {
                    tempID = "RP" + curdate + "001";
                }
                ReportID = tempID;
            }
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
        #region Reporting-SampleParameter Relation 

        private XPCollection<SampleParameter> _SampleParameter;
        [Association("ReportingSampleparameter", UseAssociationNameAsIntermediateTableName = true)]
        //[RuleRequiredField("VisualMatrixName2", DefaultContexts.Save)]
        public XPCollection<SampleParameter> SampleParameter
        {

            get
            {
                return GetCollection<SampleParameter>("SampleParameter");

            }


        }
        #endregion

        private string _ReportID;
        [EditorAlias("HyperLinkStringPropertyEditor")]
        public string ReportID
        {
            get { return _ReportID; }
            set { SetPropertyValue<string>("ReportID", ref _ReportID, value); }
        }

        private DateTime _ReportedDate;

        public DateTime ReportedDate
        {
            get { return _ReportedDate; }
            set { SetPropertyValue("ReportedDate", ref _ReportedDate, value); }
        }

        #region ReportedBy
        private Employee _ReportedBy;
        public Employee ReportedBy
        {
            get { return _ReportedBy; }
            set { SetPropertyValue<Employee>("ReportedBy", ref _ReportedBy, value); }
        }
        #endregion

        private Samplecheckin _JobID;

        public Samplecheckin JobID
        {
            get { return _JobID; }
            set { SetPropertyValue("JobID", ref _JobID, value); }
        }

        private string _ReportName;
        public string ReportName
        {
            get { return _ReportName; }
            set { SetPropertyValue("ReportName", ref _ReportName, value); }
        }

        private Project _Project;

        //public Project Project
        //{
        //    get { return _Project; }
        //    set { SetPropertyValue("Project", ref _Project, value); }
        //}

        //private Customer _CustomerName;

        //public Customer CustomerName
        //{
        //    get { return _CustomerName; }
        //    set { SetPropertyValue("CustomerName", ref _CustomerName, value); }

        //}

        private Nullable<DateTime> _ReportValidatedDate;

        public Nullable<DateTime> ReportValidatedDate
        {
            get { return _ReportValidatedDate; }
            set { SetPropertyValue("ReportValidatedDate", ref _ReportValidatedDate, value); }
        }

        #region ReportedBy
        private Employee _ReportValidatedBy;
        public Employee ReportValidatedBy
        {
            get { return _ReportValidatedBy; }
            set { SetPropertyValue<Employee>("ReportValidatedBy", ref _ReportValidatedBy, value); }
        }
        #endregion
        #region ReportApprovedDate
        private DateTime _ReportApprovedDate;

        public DateTime ReportApprovedDate
        {
            get { return _ReportApprovedDate; }
            set { SetPropertyValue("ReportApprovedDate", ref _ReportApprovedDate, value); }
        }
        #endregion
        #region ApprovedBy
        private Employee _ReportApprovedBy;
        public Employee ReportApprovedBy
        {
            get { return _ReportApprovedBy; }
            set { SetPropertyValue<Employee>("ReportApprovedBy", ref _ReportApprovedBy, value); }
        }
        #endregion
        #region Status
        private Samplestatus _Status;

        [NonPersistent]
        public Samplestatus Status
        {
            get
            {
                //if (ReportedDate != DateTime.MinValue && ReportValidatedDate == DateTime.MinValue)
                //{
                //    //_Status = "Pending Report Validation";
                //    _Status = Samplestatus.Pending2ndReview;
                //}
                //else if (ReportValidatedDate != DateTime.MinValue && ReportApprovedDate == DateTime.MinValue)
                //{
                //    //_Status = "Pending Report Approval";
                //    _Status = Samplestatus.Pending3rdReview;
                //}
                //else if (ReportApprovedDate != DateTime.MinValue && DatePrinted == null)
                //{
                //    //_Status = "Report Approved";
                //    _Status = Samplestatus.PendingPrint;
                //}
                //else if (DatePrinted != null && DateDelivered == null)
                //{
                //    _Status = Samplestatus.PendingDelivery;
                //}
                //else if (DateDelivered != null && DateArchived == null)
                //{
                //    _Status = Samplestatus.PendingArchive;
                //}
                //else if (DateArchived != null)
                //{
                //    _Status = Samplestatus.Archived;
                //}
                return _Status;
            }

        }
        #endregion

        private int _PrintCopies;
        public int PrintCopies
        {
            get { return _PrintCopies; }
            set { SetPropertyValue("PrintCopies", ref _PrintCopies, value); }
        }

        private string _StoreLocation;
        [RuleRequiredField("StoreLocation", DefaultContexts.Save)]
        public string StoreLocation
        {
            get { return _StoreLocation; }
            set { SetPropertyValue("StoreLocation", ref _StoreLocation, value); }
        }

        private DateTime? _DatePrinted;
        public DateTime? DatePrinted
        {
            get { return _DatePrinted; }
            set { SetPropertyValue("DatePrinted", ref _DatePrinted, value); }
        }

        private Employee _PrintedBy;
        public Employee PrintedBy
        {
            get { return _PrintedBy; }
            set { SetPropertyValue<Employee>("PrintedBy", ref _PrintedBy, value); }
        }

        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { SetPropertyValue("LastUpdatedDate", ref _LastUpdatedDate, value); }
        }

        private Employee _LastUpdatedBy;
        public Employee LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { SetPropertyValue<Employee>("LastUpdatedBy", ref _LastUpdatedBy, value); }
        }

        private string _PrintComments;
        [Size(500)]
        public string PrintComments
        {
            get { return _PrintComments; }
            set { SetPropertyValue<string>("PrintComments", ref _PrintComments, value); }
        }

        private DeliveryMethod _DeliveryMethod;
        public DeliveryMethod DeliveryMethod
        {
            get { return _DeliveryMethod; }
            set { SetPropertyValue("DeliveryMethod", ref _DeliveryMethod, value); }
        }

        private string _Tracking;
        public string Tracking
        {
            get { return _Tracking; }
            set { SetPropertyValue<string>("Tracking", ref _Tracking, value); }
        }

        private string _DeliveryAddress;
        [RuleRequiredField("DeliveryAddress", DefaultContexts.Save)]
        public string DeliveryAddress
        {
            get { return _DeliveryAddress; }
            set { SetPropertyValue<string>("DeliveryAddress", ref _DeliveryAddress, value); }
        }

        private string _ReceivedBy;
        [RuleRequiredField("ReceivedBy", DefaultContexts.Save)]
        public string ReceivedBy
        {
            get { return _ReceivedBy; }
            set { SetPropertyValue<string>("ReceivedBy", ref _ReceivedBy, value); }
        }

        private int _DeliveryCopies;
        public int DeliveryCopies
        {
            get { return _DeliveryCopies; }
            set { SetPropertyValue<int>("DeliveryCopies", ref _DeliveryCopies, value); }
        }

        private bool _ISEmail;
        [ImmediatePostData(true)]
        public bool ISEmail
        {
            get { return _ISEmail; }
            set { SetPropertyValue<bool>("ISEmail", ref _ISEmail, value); }
        }
        private bool _Mail;
        //[ImmediatePostData(true)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public bool Mail
        {
            get { return _Mail; }
            set { SetPropertyValue<bool>("Mail", ref _Mail, value); }
        }
        private bool _DoNotDeliver;
       // [ImmediatePostData(true)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public bool DoNotDeliver
        {
            get { return _DoNotDeliver; }
            set { SetPropertyValue<bool>("DoNotDeliver", ref _DoNotDeliver, value); }
        }
        private string _EmailAddress;
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { SetPropertyValue<string>("EmailAddress", ref _EmailAddress, value); }
        }

        private string _DeliveryComments;
        [Size(500)]
        public string DeliveryComments
        {
            get { return _DeliveryComments; }
            set { SetPropertyValue<string>("DeliveryComments", ref _DeliveryComments, value); }
        }

        private Employee _HandledBy;
        public Employee HandledBy
        {
            get { return _HandledBy; }
            set { SetPropertyValue<Employee>("HandledBy", ref _HandledBy, value); }
        }

        private DateTime? _DateDelivered;
        public DateTime? DateDelivered
        {
            get { return _DateDelivered; }
            set { SetPropertyValue("DateDelivered", ref _DateDelivered, value); }
        }

        private Employee _DeliveredBy;
        public Employee DeliveredBy
        {
            get { return _DeliveredBy; }
            set { SetPropertyValue<Employee>("DeliveredBy", ref _DeliveredBy, value); }
        }

        private int _ArchiveCopies;
        public int ArchiveCopies
        {
            get { return _ArchiveCopies; }
            set { SetPropertyValue<int>("ArchiveCopies", ref _ArchiveCopies, value); }
        }

        private string _Box;
        [RuleRequiredField("#Box", DefaultContexts.Save)]
        public string Box
        {
            get { return _Box; }
            set { SetPropertyValue<string>("Box", ref _Box, value); }
        }

        private string _CodeName;
        [RuleRequiredField("CodeName", DefaultContexts.Save)]
        public string CodeName
        {
            get { return _CodeName; }
            set { SetPropertyValue<string>("CodeName", ref _CodeName, value); }
        }

        private string _ArchiveLocation;
        [RuleRequiredField("ArchiveLocation", DefaultContexts.Save)]
        public string ArchiveLocation
        {
            get { return _ArchiveLocation; }
            set { SetPropertyValue<string>("ArchiveLocation", ref _ArchiveLocation, value); }
        }

        private string _ArchiveComment;
        [Size(500)]
        public string ArchiveComment
        {
            get { return _ArchiveComment; }
            set { SetPropertyValue<string>("ArchiveComment", ref _ArchiveComment, value); }
        }

        private Employee _ArchivedBy;
        public Employee ArchivedBy
        {
            get { return _ArchivedBy; }
            set { SetPropertyValue<Employee>("ArchivedBy", ref _ArchivedBy, value); }
        }

        private DateTime? _DateArchived;
        public DateTime? DateArchived
        {
            get { return _DateArchived; }
            set { SetPropertyValue("DateArchived", ref _DateArchived, value); }
        }

        private Employee _ArchivedReceivedBy;
        public Employee ArchivedReceivedBy
        {
            get { return _ArchivedReceivedBy; }
            set { SetPropertyValue<Employee>("ArchivedReceivedBy", ref _ArchivedReceivedBy, value); }
        }

        private string _DeliveryDeleteReason;
        [Size(1000)]
        public string DeliveryDeleteReason
        {
            get { return _DeliveryDeleteReason; }
            set { SetPropertyValue("DeliveryDeleteReason", ref _DeliveryDeleteReason, value); }
        }

        private string _ArchiveDeleteReason;
        [Size(1000)]
        public string ArchiveDeleteReason
        {
            get { return _ArchiveDeleteReason; }
            set { SetPropertyValue("ArchiveDeleteReason", ref _ArchiveDeleteReason, value); }
        }

        private string _Reason;
        [Size(1000)]
        [NonPersistent]
        public string Reason
        {
            get { return _Reason; }
            set { SetPropertyValue("Reason", ref _Reason, value); }
        }

        private int _RecalledCopies;
        public int RecalledCopies
        {
            get { return _RecalledCopies; }
            set { SetPropertyValue<int>("RecalledCopies", ref _RecalledCopies, value); }
        }

        private DateTime? _DateRecalled;
        public DateTime? DateRecalled
        {
            get { return _DateRecalled; }
            set { SetPropertyValue("DateRecalled", ref _DateRecalled, value); }
        }

        private Employee _RecalledBy;
        public Employee RecalledBy
        {
            get { return _RecalledBy; }
            set { SetPropertyValue<Employee>("RecalledBy", ref _RecalledBy, value); }
        }

        private string _RecalledFrom;
        [RuleRequiredField("RecalledFrom", DefaultContexts.Save)]
        public string RecalledFrom
        {
            get { return _RecalledFrom; }
            set { SetPropertyValue("RecalledFrom", ref _RecalledFrom, value); }
        }

        private RecalledMethod _RecalledMethod;
        public RecalledMethod RecalledMethod
        {
            get { return _RecalledMethod; }
            set { SetPropertyValue("RecalledMethod", ref _RecalledMethod, value); }
        }

        private string _RecallComment;
        [Size(500)]
        public string RecallComment
        {
            get { return _RecallComment; }
            set { SetPropertyValue<string>("RecallComment", ref _RecallComment, value); }
        }

        private ReportStatus _ReportStatus;
        public ReportStatus ReportStatus
        {
            get { return _ReportStatus; }
            set { SetPropertyValue<ReportStatus>("ReportStatus", ref _ReportStatus, value); }
        }

        private string _SampleType;
        //[Size(500)]
        public string SampleType
        {
            get { return _SampleType; }
            set { SetPropertyValue<string>("SampleType", ref _SampleType, value); }
        }

        private FileData _ImageUpload;
        public FileData ImageUpload
        {
            get { return _ImageUpload; }
            set { SetPropertyValue("ImageUpload", ref _ImageUpload, value); }
        }

        private DateTime? _DateRollback;
        public DateTime? DateRollback
        {
            get { return _DateRollback; }
            set { SetPropertyValue("DateRollback", ref _DateRollback, value); }
        }

        private Employee _RollbackedBy;
        public Employee RollbackedBy
        {
            get { return _RollbackedBy; }
            set { SetPropertyValue<Employee>("RollbackedBy", ref _RollbackedBy, value); }
        }

        private string _RollbackReason;
        [Size(SizeAttribute.Unlimited)]
        public string RollbackReason
        {
            get { return _RollbackReason; }
            set { SetPropertyValue("RollbackReason", ref _RollbackReason, value); }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { SetPropertyValue(nameof(Email), ref _Email, value); }
        }
        private int _RevisionNo;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public int RevisionNo
        {
            get { return _RevisionNo; }
            set { SetPropertyValue(nameof(RevisionNo), ref _RevisionNo, value); }
        }
        private string _RevisionReason;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public string RevisionReason
        {
            get { return _RevisionReason; }
            set { SetPropertyValue("RevisionReason", ref _RevisionReason, value); }
        }
        private DateTime _RevisionDate;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public DateTime RevisionDate
        {
            get { return _RevisionDate; }
            set { SetPropertyValue("RevisionDate", ref _RevisionDate, value); }
        }
        private Employee _RevisionBy;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public Employee RevisionBy
        {
            get { return _RevisionBy; }
            set { SetPropertyValue("RevisionBy", ref _RevisionBy, value); }
        }
        private string _PreviousReportID;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public string PreviousReportID
        {
            get { return _PreviousReportID; }
            set { SetPropertyValue("PreviousReportID", ref _PreviousReportID, value); }
        }
        public object Attachments { get; set; }

        private bool _NewReportFormat;
        [ImmediatePostData(true)]
        public bool NewReportFormat
        {
            get { return _NewReportFormat; }
            set { SetPropertyValue<bool>("NewReportFormat", ref _NewReportFormat, value); }
        }
        private int _LastSequentialNumber;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        public int LastSequentialNumber
        {
            get { return _LastSequentialNumber; }
            set { SetPropertyValue<int>("LastSequentialNumber", ref _LastSequentialNumber, value); }
        }
    }
}