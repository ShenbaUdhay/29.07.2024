using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//using Modules.BusinessObjects.ContractManagement;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.BusinessObjects.SuboutTracking
{
    public enum SuboutTrackingStatus
    {
        PendingSuboutSubmission,
        SuboutSubmitted,
        SuboutResultsEntered,
        SuboutResultsValidated,
        SuboutResultsApproved,
        Reported,
        ReportDelivered,
        Invoiced,
        InvoiceDelivered
    }
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SubOutSampleRegistrations : BaseObject
    {
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SubOutSampleRegistrations(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            DateRegistered = DateTime.Now;
            DateReceived = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SuboutOrderID == null)
            {
                CriteriaOperator qcct = CriteriaOperator.Parse("Max(SUBSTRING(SuboutOrderID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(SubOutSampleRegistrations), qcct, null)) + 1).ToString();
                var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "1")
                {
                    var predate = tempID.Substring(0, 6);
                    if (predate == curdate)
                    {
                        tempID = "SO" + tempID;
                    }
                    else
                    {
                        tempID = "SO" + curdate + "01";
                    }
                }
                else
                {
                    tempID = "SO" + curdate + "01";
                }
                SuboutOrderID = tempID;
            }
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
        private string _SuboutOrderID;
        [Appearance("SuboutOrderID", Enabled = false, Context = "DetailView")]
        public string SuboutOrderID
        {
            get { return _SuboutOrderID; }
            set { SetPropertyValue("SuboutOrderID", ref _SuboutOrderID, value); }
        }

        private DateTime _DateRegistered;
        [Appearance("DateRegistered", Enabled = false, Context = "DetailView")]
        public DateTime DateRegistered
        {
            get { return _DateRegistered; }
            set { SetPropertyValue("DateRegistered", ref _DateRegistered, value); }
        }

        private Employee _RegisteredBy;
        [Appearance("RegisteredBy", Enabled = false, Context = "DetailView")]
        public Employee RegisteredBy
        {
            get { return _RegisteredBy; }
            set { SetPropertyValue("RegisteredBy", ref _RegisteredBy, value); }
        }

        private SubOutContractLab _ContractLabName;
        [RuleRequiredField("ContractName", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public SubOutContractLab ContractLabName
        {
            get
            {
                return _ContractLabName;
            }
            set { SetPropertyValue("ContractLabName", ref _ContractLabName, value); }
        }


        private string _AccreditationID;
        [Appearance("CA", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        public string AccreditationID
        {
            get
            {
                if (ContractLabName != null)
                {
                    _AccreditationID = ContractLabName.AccreditationID;
                }

                return _AccreditationID;
            }
            set { SetPropertyValue("AccreditationID", ref _AccreditationID, value); }
        }


        private string _LabAddress;
        [Appearance("Add", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        public string LabAddress
        {
            get
            {
                if (ContractLabName != null)
                {
                    _LabAddress = ContractLabName.Address1;
                }
                return _LabAddress;
            }
            set { SetPropertyValue("LabAddress", ref _LabAddress, value); }
        }

        private string _Contact;
        [Appearance("Contact", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        public string Contact
        {
            get
            {
                if (ContractLabName != null)
                {
                    _Contact = ContractLabName.Contact;
                }
                return _Contact;
            }
            set { SetPropertyValue("Contact", ref _Contact, value); }
        }

        private string _ContactPhone;
        [Appearance("Phone", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        public string ContactPhone
        {
            get
            {
                if (ContractLabName != null)
                {
                    _ContactPhone = ContractLabName.Phone;
                }
                return _ContactPhone;
            }
            set { SetPropertyValue("ContactPhone", ref _ContactPhone, value); }
        }

        private string _ContactEmail;
        [Appearance("Email", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        public string ContactEmail
        {
            get
            {
                if (ContractLabName != null)
                {
                    _ContactEmail = ContractLabName.EmailID;
                }
                return _ContactEmail;
            }
            set { SetPropertyValue("ContactEmail", ref _ContactEmail, value); }
        }

        private string _TestDescription;
        [Appearance("TestDescription", Enabled = false, Context = "DetailView")]
        public string TestDescription
        {
            get
            {
                return _TestDescription;
            }
            set { SetPropertyValue("TestDescription", ref _TestDescription, value); }
        }

        private int _NumberofSamples;
        [Appearance("NumberofSamples", Enabled = false, Context = "DetailView")]
        public int NumberofSamples
        {
            get
            {
                if (SampleParameter != null)
                {
                    if (SampleParameter.Count > 0)
                    {
                        _NumberofSamples = SampleParameter.Select(i => i.Samplelogin).Distinct().Count();
                    }
                }
                return _NumberofSamples;

            }
        }

        private int _NumberofBottles;
        [Appearance("NumberofBottles", Enabled = false, Context = "DetailView")]
        public int NumberofBottles
        {
            get
            {
                if (SampleParameter != null)
                {
                    if (SampleParameter.Count > 0)
                    {
                        _NumberofBottles = SampleParameter.Count;
                    }
                }
                return _NumberofBottles;
            }
            set { SetPropertyValue("NumberofBottles", ref _NumberofBottles, value); }
        }


        private Preservative _Preservative;
        public Preservative Preservative
        {
            get
            {
                return _Preservative;
            }
            set { SetPropertyValue("Preservative", ref _Preservative, value); }
        }


        private TurnAroundTime _TurnAroundTime;
        public TurnAroundTime TurnAroundTime
        {
            get
            {
                return _TurnAroundTime;
            }
            set { SetPropertyValue("TurnAroundTime", ref _TurnAroundTime, value); }
        }

        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get
            {
                return _Remark;
            }
            set { SetPropertyValue("Remark", ref _Remark, value); }
        }

        [Association("SubOutSampleTest")]
        public XPCollection<SampleManagement.SampleParameter> SampleParameter
        {
            get { return GetCollection<SampleManagement.SampleParameter>("SampleParameter"); }
        }

        [Association("SubOutQcSample")]
        public XPCollection<SuboutQcSample> SubOutQcSample
        {
            get { return GetCollection<SuboutQcSample>("SubOutQcSample"); }
        }

        private SuboutStatus _Status;
        public SuboutStatus Status
        {
            get
            {
                return _Status;
            }
            set { SetPropertyValue("Status", ref _Status, value); }
        }
        #region 
        private SuboutStatus _fStatus;
        [NonPersistent]
        [VisibleInDetailView(false),VisibleInDashboards(false),VisibleInListView(false),VisibleInLookupListView(false)]
        [XafDisplayName("Status")]
        public SuboutStatus ftatus
        {
            get
            {
                if(objnavigationRefresh.ClickedNavigationItem== "Level2SuboutDataReview")
                {
                    _fStatus = SuboutTracking.SuboutStatus.SuboutPendingValidation;
                }
                else if(objnavigationRefresh.ClickedNavigationItem == "Level3SuboutDataReview")
                {
                    _fStatus= SuboutTracking.SuboutStatus.SuboutPendingApproval;
                }
                return _fStatus;
            }

        }
        #endregion
        private string _TrackingNo;
        [RuleRequiredField]
        [XafDisplayName("Tracking#")]
        public string TrackingNo
        {
            get
            {
                return _TrackingNo;
            }
            set { SetPropertyValue("TrackingNo", ref _TrackingNo, value); }
        }

        private DateTime _DateDelivered;
        public DateTime DateDelivered
        {
            get
            {
                return _DateDelivered;
            }
            set { SetPropertyValue("DateDelivered", ref _DateDelivered, value); }
        }

        private SuboutDeliveryService _DeliveryService;
        public SuboutDeliveryService DeliveryService
        {
            get
            {
                return _DeliveryService;
            }
            set
            {
                SetPropertyValue("DeliveryService", ref _DeliveryService, value);
            }
        }

        private string _Driver;
        public string Driver
        {
            get
            {
                return _Driver;
            }
            set
            {
                SetPropertyValue("Driver", ref _Driver, value);
            }
        }


        private DateTime _DateTimeArrive;
        public DateTime DateTimeArrive
        {
            get
            {
                return _DateTimeArrive;
            }
            set
            {
                SetPropertyValue("DateTimeArrive", ref _DateTimeArrive, value);
            }
        }


        private int _DaysElapsed;
        public int DaysElapsed
        {
            get
            {
                return _DaysElapsed;
            }
            set
            {
                SetPropertyValue("DaysElapsed", ref _DaysElapsed, value);
            }
        }


        private DateTime _DateReceiptExpected;
        public DateTime DateReceiptExpected
        {
            get
            {
                return _DateReceiptExpected;
            }
            set { SetPropertyValue("DateReceiptExpected", ref _DateReceiptExpected, value); }
        }

        private DateTime _DaysOverdue;
        public DateTime DaysOverdue
        {
            get
            {
                return _DaysOverdue;
            }
            set { SetPropertyValue("DaysOverdue", ref _DaysOverdue, value); }
        }


        private DateTime _DateReceived;
        public DateTime DateReceived
        {
            get { return _DateReceived; }
            set { SetPropertyValue("DateReceived", ref _DateReceived, value); }
        }

        private CustomSystemUser _ReceivedBy;
        public CustomSystemUser ReceivedBy
        {
            get { return _ReceivedBy; }
            set { SetPropertyValue("ReceivedBy", ref _ReceivedBy, value); }
        }

        [VisibleInDetailView(false)]
        public string Client
        {
            get
            {
                if (SampleParameter != null && SampleParameter.Count > 0)
                {
                    SampleManagement.SampleParameter sample = SampleParameter.FirstOrDefault();
                    if (sample != null && sample.Samplelogin != null && sample.Samplelogin.JobID != null && sample.Samplelogin.JobID.ClientName != null)
                    {
                        return sample.Samplelogin.JobID.ClientName.CustomerName;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #region deliveryContact
        private string _DeliveryContact;
        [VisibleInDetailView(false), VisibleInListView(false)]
        public string DeliveryContact
        {
            get { return _DeliveryContact; }
            set { SetPropertyValue("DeliveryContact", ref _DeliveryContact, value); }
        }
        #endregion
        #region deliveryEmail
        private string _DeliveryEmail;
        [VisibleInDetailView(false), VisibleInListView(false)]
        public string DeliveryEmail
        {
            get { return _DeliveryEmail; }
            set { SetPropertyValue("DeliveryEmail", ref _DeliveryEmail, value); }
        }
        #endregion
        #region deliveryContactPhone
        private string _DeliveryContactPhone;
        [VisibleInDetailView(false), VisibleInListView(false)]
        public string DeliveryContactPhone
        {
            get { return _DeliveryContactPhone; }
            set { SetPropertyValue("DeliveryContactPhone", ref _DeliveryContactPhone, value); }
        }
        #endregion
        #region DateShipped
        private DateTime _DateShipped;
        [VisibleInDetailView(false), VisibleInListView(false)]
        public DateTime DateShipped
        {
            get { return _DateShipped; }
            set { SetPropertyValue("DateShipped", ref _DateShipped, value); }
        }
        #endregion
        #region DueDate
        private Nullable<DateTime> _DueDate;
        [VisibleInDetailView(false), VisibleInListView(false)]
        [ImmediatePostData]
        [RuleRequiredField("Enter the Subout Duedate", DefaultContexts.Save)]
        public Nullable<DateTime> DueDate
        {
            get { return _DueDate; }
            set { SetPropertyValue("DueDate", ref _DueDate, value); }
        }
        #endregion

        [Association("SuboutSampleRegistration-Attachments")]
        public XPCollection<Attachment> Attachments
        {
            get { return GetCollection<Attachment>("Attachments"); }
        }
        #region Notes
        [DevExpress.ExpressApp.DC.Aggregated, Association("Notes_SuboutSampleReg")]
        public XPCollection<Notes> Notes
        {
            get { return GetCollection<Notes>(nameof(Notes)); }
        }
        #endregion
        #region TestForSubout
        private string _TestforSubout;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string TestforSubout
        {
            get
            {

                if (SampleParameter != null && SampleParameter.Count > 0)
                {
                    IList<string> objParam = SampleParameter.Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && i.Testparameter.TestMethod.TestName != null).Select(i => i.Testparameter.TestMethod.TestName).Distinct().ToList();
                    if (objParam != null && objParam.Count > 0)
                    {
                        string TestName = string.Empty;
                        foreach (string obj in objParam)
                        {
                            if (string.IsNullOrEmpty(TestName))
                            {
                                TestName = obj;
                            }
                            else
                            {
                                TestName = TestName + ", " + obj;
                            }
                        }
                        _TestforSubout = TestName;
                    }

                }
                return _TestforSubout;
            }
            set { SetPropertyValue("TestforSubout", ref _TestforSubout, value); }
        }
        #endregion
        #region Reason
        private string _Reason;
        [VisibleInDetailView(false), VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string Reason
        {
            get
            {
                return _Reason;
            }
            set { SetPropertyValue("Reason", ref _Reason, value); }
        }
        #endregion
        #region ResultImportEDD
        private FileData _ResultImportEDD;
        [VisibleInDetailView(false), VisibleInListView(false)]
        // [RuleRequiredField("Enter the ResultImportEDD",DefaultContexts.Save)]
        public FileData ResultImportEDD
        {
            get { return _ResultImportEDD; }
            set { SetPropertyValue("ResultImportEDD", ref _ResultImportEDD, value); }
        }

        #endregion
        #region SuboutNotificationStatus
        private SuboutNotificationQueueStatus _SuboutNotificationStatus;
        // [XafDisplayName("Status")]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public SuboutNotificationQueueStatus SuboutNotificationStatus
        {
            get { return _SuboutNotificationStatus; }
            set { SetPropertyValue("SuboutNotificationStatus", ref _SuboutNotificationStatus, value); }
        }
        #endregion
        #region COCAttached
        private bool _COCAttached;
        [VisibleInDetailView(false), VisibleInListView(false)]
        [NonPersistent]
        public bool COCAttached
        {
            get
            {
                if (Attachments.Count > 0)
                {
                    _COCAttached = true;
                }
                return _COCAttached;
            }
            set { SetPropertyValue("COCAttached", ref _COCAttached, value); }
        }
        #endregion
        #region EDDTemplateAttached
        private bool _EDDTemplateAttached;
        [VisibleInDetailView(false), VisibleInListView(false)]
        [NonPersistent]
        public bool EDDTemplateAttached
        {
            get
            {
                if (ResultImportEDD != null)
                {
                    _EDDTemplateAttached = true;
                }
                return _EDDTemplateAttached;
            }
            set { SetPropertyValue("EDDTemplateAttached", ref _EDDTemplateAttached, value); }
        }
        #endregion

        #region RollBackDate
        private DateTime? _RollBackDate;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime? RollBackDate
        {
            get { return _RollBackDate; }
            set { SetPropertyValue(nameof(RollBackDate), ref _RollBackDate, value); }
        }
        #endregion

        #region RollBackBy
        private Employee _RollBackBy;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Employee RollBackBy
        {
            get { return _RollBackBy; }
            set { SetPropertyValue(nameof(RollBackBy), ref _RollBackBy, value); }
        }
        #endregion

        #region RollBackReason
        private string _RollBackReason;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string RollBackReason
        {
            get { return _RollBackReason; }
            set { SetPropertyValue(nameof(RollBackReason), ref _RollBackReason, value); }
        }
        #endregion
        #region Status
        private SuboutTrackingStatus _SuboutStatus;
        //[XafDisplayName("Status")]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public SuboutTrackingStatus SuboutStatus
        {
            get { return _SuboutStatus; }
            set { SetPropertyValue(nameof(SuboutStatus), ref _SuboutStatus, value); }
        }
        #endregion
    }
}