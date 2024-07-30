using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Setting;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.SampleManagement;
using DevExpress.ExpressApp.Editors;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;

namespace Modules.BusinessObjects.SamplingManagement
{
    public enum RegistrationStatus
    {
        [XafDisplayName("Pending Submission")]
        PendingSubmission,
        Submitted,
        Scheduled,
        [XafDisplayName("Pending Attach Job ID")]
        PendingAttachJobID,
        [XafDisplayName("Pending Sampling")]
        PendingSampling,
        Sampled,
        Retired
    }
    [DefaultClassOptions]
    public class SamplingProposal : BaseObject, ICheckedListBoxItemsProvider
    {
        SamplingManagementInfo SMInfo = new SamplingManagementInfo();
        curlanguage curlanguage = new curlanguage();
        public const string ProgressPropertyAlias = "ProgressStatus";
        public SamplingProposal(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            RecievedDate = Library.GetServerTime(Session);
            RecievedBy = (Employee)Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            CreatedBy = (Employee)Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            NoOfSamples = 1;
            //objinfo.bolNewJobID = true;
            IsAlpacJobid = 1;
            StatusDefinition objStatus = Session.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] =9"));
            if (objStatus != null)
            {
                Index = objStatus;
            }
            PaymentStatus objPayment = Session.FindObject<PaymentStatus>(CriteriaOperator.Parse("[Default] =True "));
            if (objPayment != null)
            {
                PaymentStatus = objPayment;
            }
        }
        #region Copyfrom
        private TaskRecurranceSetup _Copyfrom;
        [ImmediatePostData(true)]
        public TaskRecurranceSetup Copyfrom
        {
            get
            {
                return _Copyfrom;
            }
            set
            {
                SetPropertyValue("Copyfrom", ref _Copyfrom, value);
            }
        }
        #endregion

        #region RegistrationID
        private string _RegistrationID;
        [RuleUniqueValue]
        [RuleRequiredField("RegistrationID", DefaultContexts.Save)]
        [Appearance("RegistrationID", Context = "DetailView")]
        public string RegistrationID
        {
            get
            {
                return _RegistrationID;
            }
            set
            {
                SetPropertyValue<string>("RegistrationID", ref _RegistrationID, value);
            }
        }
        #endregion
        #region RecievedDate

        private DateTime _RecievedDate;
        [RuleRequiredField("RecievedDate12", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public DateTime RecievedDate
        {
            get { return _RecievedDate; }
            set { SetPropertyValue<DateTime>("RecievedDate", ref _RecievedDate, value); }

        }
        #endregion

        #region RecievedBy
        private Employee _RecievedBy;
        [RuleRequiredField("RecievedBy12", DefaultContexts.Save)]
        public Employee RecievedBy
        {
            get { return _RecievedBy; }
            set { SetPropertyValue<Employee>("RecievedBy", ref _RecievedBy, value); }
        }
        #endregion

        #region ClientName
        private Customer _ClientName;
        [RuleRequiredField("ClientName21", DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public Customer ClientName
        {
            get
            {
                return _ClientName;
            }
            set
            {
                SetPropertyValue("ClientName", ref _ClientName, value);
            }
        }
        #endregion

        #region ClientAddress
        private string _ClientAddress;
        [ImmediatePostData(true)]
        [Appearance("CA11", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        public string ClientAddress
        {
            get
            {
                if (ClientName != null)
                {
                    _ClientAddress = ClientName.Address;
                }
                else
                {
                    _ClientAddress = string.Empty;
                }
                return _ClientAddress;
            }
            set { SetPropertyValue<string>("ClientAddress", ref _ClientAddress, value); }

        }
        #endregion

        #region ClientAddress2
        private string _ClientAddress2;
        [ReadOnly(true)]
        [Appearance("CA12", Enabled = false, Context = "DetailView")]
        [NonPersistent]
        //[DataSourceProperty("Customer.Address1", DataSourcePropertyIsNullMode.SelectNothing)]
        public string ClientAddress2
        {
            get { return _ClientAddress2; }
            set { SetPropertyValue<string>("ClientAddress2", ref _ClientAddress2, value); }

        }
        #endregion

        #region ClientPhone
        private string _ClientPhone;
        [ReadOnly(true)]
        [NonPersistent]
        [Appearance("CA14", Enabled = false, Context = "DetailView")]
        public string ClientPhone
        {
            get
            {
                if (ClientName != null)
                {
                    _ClientPhone = ClientName.OfficePhone;
                }
                else
                {
                    _ClientPhone = string.Empty;
                }
                return _ClientPhone;
            }
            set { SetPropertyValue(nameof(ClientPhone), ref _ClientPhone, value); }

        }
        #endregion
        #region Contact
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Contact> Contacts
        {
            get
            {
                if (ClientName != null && ClientName.Oid != null)
                {
                    return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", ClientName.Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        private Contact _ClientContact;
        [DataSourceProperty("Contacts")]
        public Contact ClientContact
        {
            get { return _ClientContact; }
            set { SetPropertyValue(nameof(ClientContact), ref _ClientContact, value); }
        }
        #endregion
        //#region Licence
        //private string _License;
        //[NonPersistent]
        //public string License
        //{
        //    get
        //    {
        //        if (ClientName != null)
        //        {
        //            _License = ClientName.LicenseNumber;
        //        }
        //        return _License;
        //    }
        //    //set { SetPropertyValue<string>("License ", ref _License, value); }

        //}
        //#endregion
        #region ProjectId
        private Project _ProjectID;
        [ImmediatePostData(true)]
        [RuleRequiredField]
        public Project ProjectID
        {
            get { return _ProjectID; }
            set
            {
                SetPropertyValue(nameof(ProjectID), ref _ProjectID, value);
            }
        }
        #endregion

        #region ProjectName
        private string _ProjectName;
        [ReadOnly(true)]
        [NonPersistent]
        [Appearance("CA13", Enabled = false, Context = "DetailView")]
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
            //set { SetPropertyValue<string>("ProjectName", ref _ProjectName, value); }
        }
        #endregion

        #region Project Location
        private string _projectLocation;
        [NonPersistent]
        [Size(300)]
        [ModelDefault("RowCount", "1")]
        public string ProjectLocation
        {
            get
            {
                if (ProjectID != null)
                {
                    _projectLocation = ProjectID.ProjectLocation;
                }
                else
                {
                    _projectLocation = string.Empty;
                }
                return _projectLocation;
            }
            //set => SetPropertyValue(nameof(ProjectLocation), ref _projectLocation, value);
        }
        #endregion

        #region ProjectCategory
        private ProjectCategory _ProjectCategory;
        public ProjectCategory ProjectCategory
        {
            get { return _ProjectCategory; }
            set { SetPropertyValue<ProjectCategory>("ProjectCategory", ref _ProjectCategory, value); }
        }
        #endregion
        #region ProjectSource
        private string _ProjectSource;
        public string ProjectSource
        {
            get { return _ProjectSource; }
            set { SetPropertyValue<string>("ProjectSource", ref _ProjectSource, value); }
        }
        #endregion
        #region ProjectOverview
        private string _ProjectOverview;
        public string ProjectOverview
        {
            get { return _ProjectOverview; }
            set { SetPropertyValue<string>("ProjectOverview", ref _ProjectOverview, value); }
        }
        #endregion
        #region ProjectCity
        private string _ProjectCity;
        public string ProjectCity
        {
            get { return _ProjectCity; }
            set { SetPropertyValue<string>("ProjectCity", ref _ProjectCity, value); }
        }
        #endregion

        #region BroughtBy
        private BroughtBy _BroughtBy;
        public BroughtBy BroughtBy
        {
            get { return _BroughtBy; }
            set { SetPropertyValue<BroughtBy>("BroughtBy", ref _BroughtBy, value); }

        }
        #endregion
       
        #region DueDate
        private Nullable<DateTime> _DueDate;
        public Nullable<DateTime> DueDate
        {
            get { return _DueDate; }
            set { SetPropertyValue<Nullable<DateTime>>("DueDate", ref _DueDate, value); }
        }
        #endregion
        private StatusDefinition _Index;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public StatusDefinition Index
        {
            get
            {
                return _Index;
            }
            set
            {
                SetPropertyValue<StatusDefinition>("Index", ref _Index, value);
            }
        }
        #region ModifiedBy
        private Employee fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        [Appearance("MB19", Enabled = false, Context = "DetailView")]
        public Employee ModifiedBy
        {
            get
            {
                return fModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref fModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime fModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        [Appearance("MD19", Enabled = false, Context = "DetailView")]
        public DateTime ModifiedDate
        {
            get
            {
                return fModifiedDate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref fModifiedDate, value);
            }
        }
        #endregion

        #region Comment
        private string fComment;
        [Size(1000)]
        [Appearance("commentVisible", Visibility = ViewItemVisibility.Show, Criteria = "[RetireDate] Is Not Null", Context = "DetailView")]
        [Appearance("commentHide", Visibility = ViewItemVisibility.Hide, Criteria = "[RetireDate] Is Null", Context = "DetailView")]
        public string Comment
        {
            get
            {
                return fComment;
            }
            set
            {
                SetPropertyValue("Comment", ref fComment, value);
            }
        }


        #endregion

        #region Comment

        private bool _Comments;
        public bool Comments
        {
            get
            {
                return _Comments;
            }
            set
            {
                SetPropertyValue("Comment", ref _Comments, value);
            }
        }


        #endregion

        #region ProjectManager

        private string _ProjectManager;
        public string ProjectManager
        {
            get
            {
                return _ProjectManager;
            }
            set
            {
                SetPropertyValue("ProjectManager", ref _ProjectManager, value);
            }
        }


        #endregion
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Contact> ContactsDataSource
        {
            get
            {
                if (ClientName != null && ClientName.Oid != null)
                {
                    XPCollection<Contact> lstconatcts = new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", ClientName.Oid));
                    if (lstconatcts.Count > 0)
                    {
                        if (lstconatcts != null && lstconatcts.Count == 1)
                        {
                            return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", ClientName.Oid));
                        }
                        else
                        {
                            return new XPCollection<Contact>(Session, CriteriaOperator.Parse("[Customer.Oid] = ?", ClientName.Oid));
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
        #region EmailList

        private Contact _EmailList;
        [DataSourceProperty("ContactsDataSource")]
        [NonPersistent]
        [ImmediatePostData]
        public Contact EmailList
        {
            get
            {
                if (ContactsDataSource != null && ContactsDataSource.Count == 1)
                {
                    _EmailList = ContactsDataSource.FirstOrDefault();
                }
                return _EmailList;
            }
            set
            {
                SetPropertyValue("EmailList", ref _EmailList, value);
            }
        }
        #endregion

        #region Email
        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                SetPropertyValue("Email", ref _Email, value);
            }
        }
        #endregion

        #region DateTimeSent

        private string _DateTimeSent;
        public string DateTimeSent
        {
            get
            {
                return _DateTimeSent;
            }
            set
            {
                SetPropertyValue("DateTimeSent", ref _DateTimeSent, value);
            }
        }
        #endregion

        #region SendBy

        private Employee _SendBy;
        public Employee SendBy
        {
            get
            {
                return _SendBy;
            }
            set
            {
                SetPropertyValue("SendBy", ref _SendBy, value);
            }
        }
        #endregion

        #region SendDate

        private Nullable<DateTime> _SendDate;
        public Nullable<DateTime> SendDate
        {
            get
            {
                return _SendDate;
            }
            set
            {
                SetPropertyValue("SendDate", ref _SendDate, value);
            }
        }
        #endregion
        #region CreatedDate

        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>("CreatedDate", ref _CreatedDate, value); }

        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue<Employee>("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion

        #region SendingMethod

        private Samplereceiptsendingmethod _SendingMethod;
        public Samplereceiptsendingmethod SendingMethod
        {
            get
            {
                return _SendingMethod;
            }
            set
            {
                SetPropertyValue("SendingMethod", ref _SendingMethod, value);
            }
        }
        #endregion
        #region COCAttached

        private string _COCAttached;
        public string COCAttached
        {
            get
            {
                return _COCAttached;
            }
            set
            {
                SetPropertyValue("COCAttached", ref _COCAttached, value);
            }
        }
        #endregion

        #region TestDescription
        private string _TestDescription;
        [Size(1000)]
        public string TestDescription
        {
            get { return _TestDescription; }
            set { SetPropertyValue("TestDescription", ref _TestDescription, value); }
        }


        #endregion
        #region SampleImage
        private byte[] _SampleImage;
        [ImageEditor(ListViewImageEditorCustomHeight = 50, DetailViewImageEditorFixedHeight = 100)]
        public byte[] SampleImage
        {
            get
            {
                return _SampleImage;
            }
            set
            {
                SetPropertyValue("SampleImage", ref _SampleImage, value);
            }
        }
        #endregion
        #region UploadCOC
        private FileData _UploadCoc;

        public FileData UploadCOC
        {
            get { return _UploadCoc; }
            set { SetPropertyValue("UploadCOC", ref _UploadCoc, value); }
        }
        #endregion

        #region NoOfDaysRemaining

        private int _NoOfDaysRemaining;
        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public int NoOfDaysRemaining
        {
            get
            {
                if (DueDate != null && DueDate.HasValue)
                {
                    _NoOfDaysRemaining = Convert.ToInt32((DueDate.Value - DateTime.Now.Date).TotalDays);
                }
                return _NoOfDaysRemaining;
            }
        }
        #endregion

        #region NoOfSamples

        private uint _NoOfSamples;
        //This Property for sample login.
        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public uint NoOfSamples
        {
            get
            {

                if (!IsLoading && RegistrationID != null && SMInfo.isNoOfSampleDisable)
                {
                    _NoOfSamples = Convert.ToUInt32(Session.Evaluate(typeof(Sampling), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", Oid)));
                   
                }
                return _NoOfSamples;
            }
            set
            {
                SetPropertyValue<uint>("NoOfSamples", ref _NoOfSamples, value);
            }
        }
        #endregion


        #region SampleCheckin-Image Relation
        [Association("SamplingProposalImage", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Image> ImageUpload
        {
            get { return GetCollection<Image>("ImageUpload"); }
        }
        #endregion

        #region GroupSampleLogin
        [Association("SamplingProposalTest", UseAssociationNameAsIntermediateTableName = true)]

        public XPCollection<Testparameter> Testparameters
        {
            get { return GetCollection<Testparameter>("Testparameters"); }
        }

        private VisualMatrix _GroupVisualMatrix;
        [ImmediatePostData]
        public VisualMatrix GroupVisualMatrix
        {
            get { return _GroupVisualMatrix; }
            set { SetPropertyValue("GroupVisualMatrix", ref _GroupVisualMatrix, value); }
        }

        #endregion
        #region ClientSampleID
        private string _ClientSampleID;
        [NonPersistent]
        public string ClientSampleID
        {
            get { return _ClientSampleID; }
            set { SetPropertyValue("ClientSampleID", ref _ClientSampleID, value); }
        }
        #endregion
        #region CollectionDate
        private DateTime _CollectionDate;
        [NonPersistent]
        public DateTime CollectionDate
        {
            get { return _CollectionDate; }
            set { SetPropertyValue("CollectionDate", ref _CollectionDate, value); }
        }
        #endregion
        private TimeSpan _CollectionTime;
        [NonPersistent]
        public TimeSpan CollectionTime
        {
            get { return _CollectionTime; }
            set { SetPropertyValue("CollectionTime", ref _CollectionTime, value); }
        }
        #region Collector
        private Collector _Collector;
        [DataSourceProperty(nameof(CollectorDataSource))]
        [ImmediatePostData(true)]
        public Collector Collector
        {
            get { return _Collector; }
            set { SetPropertyValue("Collector", ref _Collector, value); }
        }
        [NonPersistent]
        [Browsable(false)]
        public XPCollection<Collector> CollectorDataSource
        {
            get
            {
                if (ClientName != null)
                {
                    XPCollection<Collector> names = new XPCollection<Collector>(Session, CriteriaOperator.Parse("[CustomerName] Is Null or [CustomerName.Oid] = ?", ClientName.Oid));
                    XPView lstview = new XPView(Session, typeof(Collector));
                    lstview.Criteria = new InOperator("Oid", names.Select(i => i.Oid));
                    lstview.Properties.Add(new ViewProperty("TCustomerName", SortDirection.Ascending, "FirstName", true, true));
                    lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    List<object> groups = new List<object>();
                    foreach (ViewRecord rec in lstview)
                        groups.Add(rec["Toid"]);
                    return new XPCollection<Collector>(Session, new InOperator("Oid", groups), new SortProperty("FirstName", SortingDirection.Ascending));
                }
                else
                {
                    return new XPCollection<Collector>(Session, CriteriaOperator.Parse("[CustomerName] Is Null"));
                }
            }
        }
        #endregion
        private string _SampleLocation;
        [NonPersistent]
        public string SampleLocation
        {
            get { return _SampleLocation; }
            set { SetPropertyValue("SampleLocation", ref _SampleLocation, value); }
        }

        private Contact _ContactName;
        public Contact ContactName
        {
            get { return _ContactName; }
            set
            {
                SetPropertyValue("ContactName", ref _ContactName, value);
            }
        }

        private string _SalesOrder;

        public string SalesOrder
        {
            get { return _SalesOrder; }
            set { SetPropertyValue("SaleseOrder", ref _SalesOrder, value); }
        }

        #region Status
        RegistrationStatus _status;
        [ReadOnly(true)]
        public RegistrationStatus Status { get => _status; set => SetPropertyValue(nameof(Status), ref _status, value); }
        #endregion
        #region Status
        SampleReceiptNotificationStatus _MailStatus;
        [ReadOnly(true)]
        public SampleReceiptNotificationStatus MailStatus { get => _MailStatus; set => SetPropertyValue(nameof(MailStatus), ref _MailStatus, value); }
        #endregion    

        #region Status
        ReportStatus _ReportStatus;
        [ReadOnly(true)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public ReportStatus ReportStatus { get => _ReportStatus; set => SetPropertyValue(nameof(ReportStatus), ref _ReportStatus, value); }
        #endregion
        #region COCSource
        private Modules.BusinessObjects.Setting.COCSettings _COCSource;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Modules.BusinessObjects.Setting.COCSettings COCSource
        {
            get { return _COCSource; }
            set { SetPropertyValue(nameof(COCSource), ref _COCSource, value); }
        }
        #endregion
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Project> ProjectDataSource
        {
            get
            {
                if (ClientName != null)
                {
                    XPCollection<Project> listprojects = new XPCollection<Project>(Session, CriteriaOperator.Parse("[customername.Oid] = ? ", ClientName.Oid));
                    return listprojects;
                }
                else
                {
                    return null;
                }
            }
        }
        #region upload

        #endregion
        #region ClientDataSource
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Customer> ClientDataSource
        {
            get
            {
                if (ProjectID != null)
                {
                    var lstCustomers = new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Projects][[ProjectName] = ?]", ProjectID.ProjectName));
                    if (lstCustomers.Count > 0)
                    {
                        return new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Projects][[ProjectName] = ?]", ProjectID.ProjectName));
                    }
                    else
                    {
                        return new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Oid] is not null"));
                    }
                }
                else
                {
                    return new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Oid] is not null"));
                }
            }
        }
        #endregion
        #region ProgressStatus
        [EditorAlias(ProgressPropertyAlias)]
        [NonPersistent]
        public double ProgressStatus
        {
            get
            {
                if (RegistrationID != null)
                {
                    Samplecheckin samplecheckin = Session.FindObject<Samplecheckin>(CriteriaOperator.Parse("[RegistrationID]=?", RegistrationID));
                    var objProgress = Session.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Oid] = ?", samplecheckin.Index));
                    if (objProgress != null)
                    {
                        if (Index.Status == "Registration Submitted")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "TCLP Prepared")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Sample Prepared")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Entered")
                        {
                            return objProgress.Progress;
                        }

                        else if (Index.Status == "Level 2 Batch Reviewed")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Level 3 Batch Reviewed")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Approved")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Reported")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Report Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Report Delivered")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoiced")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoice Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoice Delivered")
                        {
                            return objProgress.Progress;
                        }

                    }

                }

                return 0;
            }
        }
        #endregion
        [EditorAlias(ProgressPropertyAlias)]
        [NonPersistent]
        public double ProjectTrackingStatus
        {
            get
            {

                if (RegistrationID != null)
                {
                    Samplecheckin samplecheckin = Session.FindObject<Samplecheckin>(CriteriaOperator.Parse("[RegistrationID]=?", RegistrationID));
                    var objProgress = Session.FindObject<StatusDefinition>(CriteriaOperator.Parse("[Oid] = ?", samplecheckin.Index));
                    if (objProgress != null)
                    {
                        if (Index.Status == "Registration Submitted")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "TCLP Prepared")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Sample Prepared")
                        {

                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Entered")
                        {
                            return objProgress.Progress;
                        }

                        else if (Index.Status == "Level 2 Batch Reviewed")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Level 3 Batch Reviewed")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Result Approved")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Reported")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Report Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Report Delivered")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoiced")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoice Validated")
                        {
                            return objProgress.Progress;
                        }
                        else if (Index.Status == "Invoice Delivered")
                        {
                            return objProgress.Progress;
                        }

                    }

                }
                return 0;

                //double objSampleParameter = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid)));
                //if (objSampleParameter > 0)
                //{
                //    double PendingEntry = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] != 'PendingEntry'", Oid)));
                //    return Math.Round((PendingEntry / objSampleParameter) * 100);
                //}
                //else
                //{
                //    return 0;
                //}
            }
        }

        [NonPersistent]
        public string Reported
        {
            get
            {
                int objSampleParameter = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid)));
                if (objSampleParameter > 0)
                {
                    int Reported = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and [Status] = 'Reported'", Oid)));
                    if (objSampleParameter == Reported)
                    {
                        if (curlanguage.strcurlanguage == "En")
                        {
                            return "Reported";
                        }
                        else
                        {
                            return "已报告";
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
        #region RelinquishedBy
        private BroughtBy _RelinquishedBy;
        public BroughtBy RelinquishedBy
        {
            get { return _RelinquishedBy; }
            set { SetPropertyValue<BroughtBy>(nameof(RelinquishedBy), ref _RelinquishedBy, value); }
        }
        #endregion
        #region ManifestNumbers
        private uint _ManifestNumbers;
        public uint ManifestNumbers
        {
            get { return _ManifestNumbers; }
            set { SetPropertyValue<uint>(nameof(ManifestNumbers), ref _ManifestNumbers, value); }
        }
        #endregion
        #region SampleTemperature(°C)
        private string _SampleTemperature;
        public string SampleTemperature
        {
            get { return _SampleTemperature; }
            set { SetPropertyValue<string>(nameof(SampleTemperature), ref _SampleTemperature, value); }
        }
        #endregion
        #region BatchID
        private string _BatchID;
        public string BatchID
        {
            get { return _BatchID; }
            set { SetPropertyValue<string>(nameof(BatchID), ref _BatchID, value); }
        }
        #endregion
        #region PackageNo
        private string _PackageNo;
        public string PackageNo
        {
            get { return _PackageNo; }
            set { SetPropertyValue<string>(nameof(PackageNo), ref _PackageNo, value); }
        }
        #endregion
        #region PreviousWeight(g)
        private string _PreviousWeight;
        public string PreviousWeight
        {
            get { return _PreviousWeight; }
            set { SetPropertyValue<string>(nameof(PreviousWeight), ref _PreviousWeight, value); }
        }
        #endregion
        #region CurrentWeight(g)
        private string _CurrentWeight;
        public string CurrentWeight
        {
            get { return _CurrentWeight; }
            set { SetPropertyValue<string>(nameof(CurrentWeight), ref _CurrentWeight, value); }
        }
        #endregion
        #region Remark
        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue<string>(nameof(Remark), ref _Remark, value); }
        }
        #endregion
        private TurnAroundTime _TAT;
        [ImmediatePostData(true)]

        [RuleRequiredField("EnterTAT", DefaultContexts.Save)]
        public TurnAroundTime TAT
        {
            get
            {
                if (_TAT == null)
                {
                    var tatobject = new XPCollection<TurnAroundTime>(Session, CriteriaOperator.Parse("[Default] = True")).ToList();
                    _TAT = tatobject.FirstOrDefault();
                }

                return _TAT;



            }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }
        private string _ReportDeliveryMethod;
        public string ReportDeliveryMethod
        {
            get { return _ReportDeliveryMethod; }
            set { SetPropertyValue(nameof(ReportDeliveryMethod), ref _ReportDeliveryMethod, value); }
        }
        private string _ReportDeliveryAddress;
        public string ReportDeliveryAddress
        {
            get { return _ReportDeliveryAddress; }
            set { SetPropertyValue(nameof(ReportDeliveryAddress), ref _ReportDeliveryAddress, value); }
        }
        private string _ReportSpecification;
        public string ReportSpecification
        {
            get { return _ReportSpecification; }
            set { SetPropertyValue(nameof(ReportSpecification), ref _ReportSpecification, value); }
        }

        private bool _PendingResult;
        [NonPersistent]
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        public bool PendingResult
        {
            get
            {
                if (RegistrationID != null)
                {

                }
                int Result = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=? and Not IsNullOrEmpty([ResultNumeric])", Oid)));
                int Total = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", Oid)));
                if (Result != Total)
                {
                    _PendingResult = true;
                }
                else
                {
                    _PendingResult = false;
                }
                return _PendingResult;
            }
        }
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(SampleMatries))
                {
                    List<string> lstSM = new List<string>();
                    List<string> lstSMOid = SampleMatries.Split(';').ToList();
                    if (lstSMOid != null)
                    {
                        foreach (string objOid in lstSMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                VisualMatrix objVM = Session.GetObjectByKey<VisualMatrix>(new Guid(objOid.Trim()));
                                if (objVM != null && !lstSM.Contains(objVM.MatrixName.MatrixName))
                                {
                                    lstSM.Add(objVM.MatrixName.MatrixName);
                                }
                            }

                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("MatrixName.MatrixName", lstSM));
                }
                else
                {
                    return null;
                }
            }
        }
        #region TestSummary
        private string _TestSummary;
        public string TestSummary
        {
            get { return _TestSummary; }
            set { SetPropertyValue<string>(nameof(TestSummary), ref _TestSummary, value); }
        }
        #endregion
       
        #region NoStations
        private int _NoStations;
        public int NoStations
        {
            get { return _NoStations; }
            set { SetPropertyValue<int>(nameof(NoStations), ref _NoStations, value); }
        }
        #endregion
        #region SamplingEquipment
        private string _SamplingEquipment;
        public string SamplingEquipment
        {
            get { return _SamplingEquipment; }
            set { SetPropertyValue<string>(nameof(SamplingEquipment), ref _SamplingEquipment, value); }
        }
        #endregion
        private Employee _RollbackedBy;
        public Employee RollbackedBy
        {
            get { return _RollbackedBy; }
            set { SetPropertyValue(nameof(RollbackedBy), ref _RollbackedBy, value); }
        }
        private DateTime _RollbackedDate;
        public DateTime RollbackedDate
        {
            get { return _RollbackedDate; }
            set { SetPropertyValue(nameof(RollbackedDate), ref _RollbackedDate, value); }
        }
        private String _RollbackReason;
        [Size(SizeAttribute.Unlimited)]
        public String RollbackReason
        {
            get { return _RollbackReason; }
            set { SetPropertyValue(nameof(_RollbackReason), ref _RollbackReason, value); }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<SampleCategory> SampleCategorys
        {
            get
            {
                return new XPCollection<SampleCategory>(Session, CriteriaOperator.Parse(""));
            }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<VisualMatrix> SampleMatrixes
        {
            get
            {
                return new XPCollection<VisualMatrix>(Session, CriteriaOperator.Parse("[IsRetired] <> True Or [IsRetired] Is Null"));
            }
        }
        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "SampleMatries" && SampleMatrixes != null && SampleMatrixes.Count > 0)
            {
                Properties = SampleMatrixes.OrderBy(i => i.VisualMatrixName).ToDictionary(x => (Object)x.Oid, x => x.VisualMatrixName);
                //foreach (VisualMatrix objSampleMatrix in SampleMatrixes.Where(i => i.VisualMatrixName != null).OrderBy(i => i.VisualMatrixName).ToList())
                //{
                //    if (!Properties.ContainsKey(objSampleMatrix.Oid))
                //    {
                //        Properties.Add(objSampleMatrix.Oid, objSampleMatrix.VisualMatrixName);
                //    }
                //}
            }
            if (targetMemberName == "SampleCategory" && SampleCategorys != null && SampleCategorys.Count > 0)
            {
                Properties = SampleCategorys.Where(i => i.SampleCategoryName != null).OrderBy(i => i.SampleCategoryName).ToDictionary(x => (object)x.Oid, x => x.SampleCategoryName);
                //foreach (SampleCategory objCategory in SampleCategorys.Where(i => i.SampleCategoryName != null).OrderBy(i => i.SampleCategoryName).ToList())
                //{
                //    if (!Properties.ContainsKey(objCategory.Oid))
                //    {
                //        Properties.Add(objCategory.Oid, objCategory.SampleCategoryName);
                //    }
                //}
            }
            if (targetMemberName == "TestName" && TestDataSource != null && TestDataSource.Count > 0)
            {
                foreach (TestMethod objTest in TestDataSource.Where(i => i.TestName != null).OrderBy(i => i.TestName).ToList())
                {
                    if (!Properties.ContainsValue(objTest.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestName);
                    }
                }
            }
            return Properties;
        }
        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        #endregion
        private string _SampleMatries;
        [RuleRequiredField]
        [XafDisplayName("Sample Matrix")]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string SampleMatries
        {
            get { return _SampleMatries; }
            set { SetPropertyValue(nameof(SampleMatries), ref _SampleMatries, value); }
        }
        private string _SampleCategory;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string SampleCategory
        {
            get { return _SampleCategory; }
            set { SetPropertyValue(nameof(SampleCategory), ref _SampleCategory, value); }
        }
        [Association("SamplingProposal-SampleConditionCheck")]
        public XPCollection<SampleConditionCheck> SampleConditionCheck
        {
            get { return GetCollection<SampleConditionCheck>("SampleConditionCheck"); }
        }
        #region Attachments
        [Association("SamplingProposal-Attachments")]
        public XPCollection<Attachment> Attachments
        {
            get { return GetCollection<Attachment>("Attachments"); }
        }
        #endregion

        [Association("SamplingProposal-CopyTo")]
        public XPCollection<TaskRecurranceSetup> CopyTo
        {
            get { return GetCollection<TaskRecurranceSetup>("CopyTo"); }
        }



        #region Notes
        [DevExpress.ExpressApp.DC.Aggregated, Association("Notes_SamplingProposal")]
        public XPCollection<Notes> Notes
        {
            get { return GetCollection<Notes>(nameof(Notes)); }
        }
        #endregion
        private string _TestName;
        //[RuleRequiredField]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [ImmediatePostData]
        public string TestName
        {
            get
            {
                if (string.IsNullOrEmpty(SampleMatries))
                {
                    _TestName = null;
                }
                return _TestName;
            }
            set { SetPropertyValue(nameof(TestName), ref _TestName, value); }
        }
        #region CustomDueDateCollection
        [DevExpress.ExpressApp.DC.Aggregated, Association("SamplingProposal_CustomDueDate")]
        public XPCollection<CustomDueDate> CustomDueDates
        {
            get { return GetCollection<CustomDueDate>(nameof(CustomDueDates)); }
        }
        #endregion

        private Labware _BalanceID;
        public Labware BalanceID
        {
            get { return _BalanceID; }
            set { SetPropertyValue(nameof(BalanceID), ref _BalanceID, value); }
        }
        private string _ReasonforWeighing;
        [Size(SizeAttribute.Unlimited)]
        public string ReasonforWeighing
        {
            get { return _ReasonforWeighing; }
            set { SetPropertyValue(nameof(ReasonforWeighing), ref _ReasonforWeighing, value); }
        }
        #region BalanceCalibrationID
        private string _BalanceCalibrationID;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string BalanceCalibrationID
        {
            get { return _BalanceCalibrationID; }
            set { SetPropertyValue(nameof(BalanceCalibrationID), ref _BalanceCalibrationID, value); }
        }
        #endregion
        #region ReportTemplate
        private string _ReportTemplate;
        [EditorAlias("ReportTemplatePropertyEditor")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string ReportTemplate
        {
            get
            {
                return _ReportTemplate;
            }
            set { SetPropertyValue(nameof(ReportTemplate), ref _ReportTemplate, value); }
        }
        #endregion

        #region QuoteID
        private CRMQuotes _QuoteID;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [DataSourceProperty("QuotesDataSource")]
        [ImmediatePostData]
        public CRMQuotes QuoteID
        {
            get { return _QuoteID; }
            set { SetPropertyValue(nameof(QuoteID), ref _QuoteID, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<CRMQuotes> QuotesDataSource
        {
            get
            {
                if (ClientName != null)
                {
                    XPCollection<CRMQuotes> lstquotes = new XPCollection<CRMQuotes>(Session, CriteriaOperator.Parse("[Client.Oid]=? And [ExpirationDate] >= ? And [Status] = 'Reviewed'", ClientName.Oid, DateTime.Now.Date));
                    List<Guid> lstquotesOid = new List<Guid>();
                    List<string> lstquoteID = new List<string>();
                    foreach (CRMQuotes objOid in lstquotes)
                    {
                        if (!lstquoteID.Contains(objOid.QuoteID))
                        {
                            lstquoteID.Add(objOid.QuoteID);
                            lstquotesOid.Add(objOid.Oid);
                        }
                    }
                    XPView testsView = new XPView(Session, typeof(CRMQuotes));
                    testsView.Criteria = new InOperator("Oid", lstquotes.Select(i => i.Oid));
                    testsView.Properties.Add(new ViewProperty("TQuoteID", SortDirection.Ascending, "QuoteID", true, true));
                    testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    List<object> groups = new List<object>();
                    foreach (ViewRecord rec in testsView)
                        groups.Add(rec["Toid"]);
                    return new XPCollection<CRMQuotes>(Session, new InOperator("Oid", groups));
                }
                else
                {
                    return null;
                }
            }

        }
        #endregion


        #region InvoiceStatus
        private Modules.BusinessObjects.Hr.InvoiceStatus _InvoiceStatus;
        [XafDisplayName("Status")]
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Modules.BusinessObjects.Hr.InvoiceStatus InvoiceStatus
        {
            get
            {
                return _InvoiceStatus;
            }
            set { SetPropertyValue(nameof(InvoiceStatus), ref _InvoiceStatus, value); }
        }
        #endregion
        #region BatchInvoice
        //#region DateReportedFrom
        //private DateTime _DateReportedFrom;
        //[NonPersistent]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //[Appearance("DateReportedFrom1", Context = "DetailView")]
        //public DateTime DateReportedFrom
        //{
        //    get { return _DateReportedFrom; }
        //    set
        //    {
        //        SetPropertyValue("DateReportedFrom", ref _DateReportedFrom, value);
        //    }
        //}
        //#endregion
        //#region DateReportedTo
        //private DateTime _DateReportedTo;
        //[NonPersistent]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //[Appearance("DateReportedTo1", Context = "DetailView")]
        //public DateTime DateReportedTo
        //{
        //    get { return _DateReportedTo; }
        //    set
        //    {
        //        SetPropertyValue("DateReportedTo", ref _DateReportedTo, value);
        //    }
        //}
        //#endregion
        #endregion
        #region ItemChargePricingCollection
        [DevExpress.ExpressApp.DC.Aggregated, Association("SamplingProposal-SPItemChargePrice")]
        public XPCollection<SamplingProposalItemChargePricing> SPItemCharges
        {
            get { return GetCollection<SamplingProposalItemChargePricing>(nameof(SPItemCharges)); }
        }
        #endregion
        private string _NPTest;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        [Size(SizeAttribute.Unlimited)]
        //[NonPersistent]
        public string NPTest
        {
            get
            {
                return _NPTest;
            }
            set { SetPropertyValue("NPTest", ref _NPTest, value); }
        }

        private string _Test;
        [ImmediatePostData(true)]
        [XafDisplayName("Test Name")]
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public string Test
        {
            get { return _Test; }
            set { SetPropertyValue("Test", ref _Test, value); }
        }
        private string _PO;
        [XafDisplayName("PO #")]
        public string PO
        {
            get { return _PO; }
            set { SetPropertyValue("PO", ref _PO, value); }
        }
        #region IsAlpacJobid
        private int _IsAlpacJobid;
        [Browsable(false)]
        public int IsAlpacJobid
        {
            get { return _IsAlpacJobid; }
            set { SetPropertyValue("IsAlpacJobid", ref _IsAlpacJobid, value); }
        }
        #endregion
        #region DateCollected
        private Nullable<DateTime> _DateCollected;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        [ImmediatePostData]
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public Nullable<DateTime> DateCollected
        {
            get { return _DateCollected; }
            set { SetPropertyValue("DateCollected", ref _DateCollected, value); }
        }
        #endregion
        #region PaymentStatus
        private PaymentStatus _PaymentStatus;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        public PaymentStatus PaymentStatus
        {
            get { return _PaymentStatus; }
            set { SetPropertyValue("PaymentStatus", ref _PaymentStatus, value); }
        }
        #endregion
        #region Date Expect
        private DateTime _DateExpect;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        public DateTime DateExpect
        {
            get { return _DateExpect; }
            set { SetPropertyValue("DateExpect", ref _DateExpect, value); }
        }
        #endregion
        #region Retire Date
        private DateTime _RetireDate;
        [ImmediatePostData(true)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        public DateTime RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue("RetireDate", ref _RetireDate, value); }
        }
        #endregion
        #region RetiredBy
        private Employee _RetiredBy;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false), VisibleInLookupListView(false)]
        [Appearance("retiredVisible", Visibility = ViewItemVisibility.Show, Criteria = "[RetireDate] Is Not Null", Context = "DetailView")]
        [Appearance("retiredHide", Visibility = ViewItemVisibility.Hide, Criteria = "[RetireDate] Is Null", Context = "DetailView")]
        public Employee RetiredBy
        {
            get { return _RetiredBy; }
            set { SetPropertyValue("RetiredBy", ref _RetiredBy, value); }
        }
        #endregion
        #region SampleCount
        private int _SampleCount;
        [NonPersistent]
        [VisibleInDashboards(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [XafDisplayName("#Samples")]
        public int SampleCount
        {
            get
            {
                if(RegistrationID!=null)
                {
                    _SampleCount = Convert.ToInt32(Session.Evaluate(typeof(Sampling), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", Oid)));
                }
                else
                {
                    _SampleCount = 0;
                }
                return _SampleCount;
            }
        }
        #endregion
    }
}