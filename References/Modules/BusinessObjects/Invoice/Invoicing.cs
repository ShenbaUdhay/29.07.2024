﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace Modules.BusinessObjects.Setting.Invoicing
{
    public enum InviceStatus
    {
        [XafDisplayName("Pending Invoicing")]
        PendingInvoicing,
        [XafDisplayName("Pending Submission")]
        PendingSubmit,
        [XafDisplayName("Pending Review")]
        PendingReview,
        [XafDisplayName("Pending Delivery")]
        PendingDelivery,
        [XafDisplayName("Delivered")]
        Delivered

    }
    [DefaultClassOptions]

    public class Invoicing : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Invoicing(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            InvoicedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            DateInvoiced = Library.GetServerTime(Session);
            DateReceived = Library.GetServerTime(Session);
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            DateReceived = Library.GetServerTime(Session);
            if (string.IsNullOrEmpty(InvoiceID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(InvoiceID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(Invoicing), criteria, null)) + 1).ToString("000");
                var curdate = DateTime.Now.ToString("yyMMdd");
                string ProjectName = string.Empty;
                string sqlSelect = string.Empty;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProjectName"].ToString()))
                {
                    ProjectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                }
                
                string strPrefix = "IV";
                if (ProjectName != null && ProjectName.ToUpper() == "OIL")
                {
                    strPrefix = "11";
                }
                if (tempID != "001")
                {
                    var predate = tempID.Substring(0, 6);
                    if (predate == curdate)
                    {
                        tempID = strPrefix + tempID;
                    }
                    else
                    {
                        tempID = strPrefix + curdate + "001";
                    }
                }
                else
                {
                    tempID = strPrefix + curdate + "001";
                }
                InvoiceID = tempID;
            }
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        #region InvoiceID
        private string _InvoiceID;
        public string InvoiceID
        {
            get { return _InvoiceID; }
            set { SetPropertyValue(nameof(InvoiceID), ref _InvoiceID, value); }
        }
        #endregion
        #region DateInvoiced
        private DateTime _DateInvoiced;
        public DateTime DateInvoiced
        {
            get { return _DateInvoiced; }
            set { SetPropertyValue(nameof(DateInvoiced), ref _DateInvoiced, value); }
        }
        #endregion

        #region InvoicedBy
        private Employee _InvoicedBy;
        public Employee InvoicedBy
        {
            get { return _InvoicedBy; }
            set { SetPropertyValue(nameof(InvoicedBy), ref _InvoicedBy, value); }
        }
        #endregion
        #region JobID
        private string _JobID;
        //[ImmediatePostData]
        //[EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField("InvoiceJobID", DefaultContexts.Save)]
        public string JobID
        {
            get { return _JobID; }
            set { SetPropertyValue(nameof(JobID), ref _JobID, value); }
        }

        #endregion
        //#region JobID
        //private string _NpJobID;
        //[ImmediatePostData]
        ////[EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[Size(SizeAttribute.Unlimited)]
        //[RuleRequiredField("_NpJobID", DefaultContexts.Save)]
        //public string NpJobID
        //{
        //    get { return _NpJobID; }
        //    set { SetPropertyValue(nameof(NpJobID), ref _NpJobID, value); }
        //}

        //#endregion
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<SampleParameter> JobIDs
        {
            get
            {
                return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Status] = 'Reported' And ([InvoiceIsDone] = False Or [InvoiceIsDone] Is Null)"));
            }
        }
        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "JobID" && JobIDs != null && JobIDs.Count > 0)
            {
                foreach (SampleParameter objJobId in JobIDs.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).OrderByDescending(i => i.Samplelogin.JobID.JobID).Distinct().ToList())
                {
                    if (!Properties.ContainsKey(objJobId.Samplelogin.JobID.Oid))
                    {
                        Properties.Add(objJobId.Samplelogin.JobID.Oid, objJobId.Samplelogin.JobID.JobID);
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
        #region Client
        private Customer _Client;
        [RuleRequiredField("InvoiceClient", DefaultContexts.Save)]
        [ImmediatePostData]

        public Customer Client
        {
            get { return _Client; }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        }
        #endregion
        #region ProjectID
        private Project _ProjectID;
        public Project ProjectID
        {
            get
            {
                if (Client != null)
                {

                }
                return _ProjectID;
            }
            set { SetPropertyValue(nameof(ProjectID), ref _ProjectID, value); }
        }
        #endregion
        #region ProjectName
        private string _ProjectName;
        [ReadOnly(true)]
        [NonPersistent]
        public string ProjectName
        {
            get
            {
                if (ProjectID != null && ProjectID.ProjectName != null)
                {
                    _ProjectName = ProjectID.ProjectName;
                }
                return _ProjectName;
            }
            set { SetPropertyValue<string>("ProjectName", ref _ProjectName, value); }
        }
        #endregion
        #region QuoteID
        private CRMQuotes _QuoteID;
        [ImmediatePostData]
        [DataSourceCriteria("[Client.CustomerName] =Client.CustomerName")]

        public CRMQuotes QuoteID
        {
            get { return _QuoteID; }
            set { SetPropertyValue(nameof(QuoteID), ref _QuoteID, value); }
        }
        #endregion
        #region QuotedDate
        private DateTime _QuotedDate;
        public DateTime QuotedDate
        {
            get
            {
                if (QuoteID != null && QuoteID.QuotedDate != DateTime.MinValue)
                {
                    _QuotedDate = QuoteID.QuotedDate;
                }
                return _QuotedDate;
            }
            set { SetPropertyValue(nameof(QuotedDate), ref _QuotedDate, value); }
        }
        #endregion
        #region QuotedBy
        private Employee _QuotedBy;
        public Employee QuotedBy
        {
            get
            {
                if (QuoteID != null)
                {
                    _QuotedBy = QuoteID.QuotedBy;
                }
                return _QuotedBy;
            }
            set { SetPropertyValue(nameof(QuotedBy), ref _QuotedBy, value); }
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
        #region Priority
        private Priority _Priority;
        public Priority Priority
        {
            get { return _Priority; }
            set { SetPropertyValue(nameof(Priority), ref _Priority, value); }
        }
        #endregion
        #region TAT
        private TurnAroundTime _TAT;
        [ImmediatePostData]
        public TurnAroundTime TAT
        {
            get { return _TAT; }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }
        #endregion
        #region DueDate
        private DateTime _DueDate;
        public DateTime DueDate
        {
            get
            {

                return _DueDate;
            }
            set { SetPropertyValue<DateTime>("DueDate", ref _DueDate, value); }
        }
        #endregion
        #region DaysDelayed
        private int _DaysDelayed;
        public int DaysDelayed
        {
            get
            {
                if (DueDate != DateTime.MinValue && DueDate < DateTime.Today)
                {
                    _DaysDelayed = Math.Abs(((DateTime)DueDate).Subtract(DateTime.Today).Days);
                }
                return _DaysDelayed;
            }
            set { SetPropertyValue<int>("DaysDelayed", ref _DaysDelayed, value); }
        }
        #endregion
        #region ActualPriorityApplied
        private Priority _ActualPriorityApplied;
        public Priority ActualPriorityApplied
        {
            get { return _ActualPriorityApplied; }
            set { SetPropertyValue(nameof(ActualPriorityApplied), ref _ActualPriorityApplied, value); }
        }
        #endregion
        #region Remark
        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        #endregion
        #region Status
        private InviceStatus _Status;

        public InviceStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        #endregion
        #region DetailedAmount
        private decimal _DetailedAmount;

        public decimal DetailedAmount
        {
            get { return _DetailedAmount; }
            set { SetPropertyValue(nameof(DetailedAmount), ref _DetailedAmount, value); }
        }
        #endregion
        #region DiscountAmount
        private decimal _DiscountAmount;

        public decimal DiscountAmount
        {
            get { return _DiscountAmount; }
            set { SetPropertyValue(nameof(DiscountAmount), ref _DiscountAmount, value); }
        }
        #endregion
        #region Discount%
        private Decimal _Discount;

        public Decimal Discount
        {
            get { return _Discount; }
            set { SetPropertyValue(nameof(Discount), ref _Discount, value); }
        }
        #endregion
        #region Amount
        private decimal _Amount;

        public decimal Amount
        {
            get { return _Amount; }
            set { SetPropertyValue(nameof(Amount), ref _Amount, value); }
        }
        #endregion
        #region PrimaryContact
        private Contact _PrimaryContact;

        public Contact PrimaryContact
        {
            get { return _PrimaryContact; }
            set { SetPropertyValue(nameof(PrimaryContact), ref _PrimaryContact, value); }
        }
        #endregion
        #region ContactPhone
        private string _ContactPhone;

        public string ContactPhone
        {
            get { return _ContactPhone; }
            set { SetPropertyValue(nameof(ContactPhone), ref _ContactPhone, value); }
        }
        #endregion
        #region ContactEmail
        private string _ContactEmail;
        public string ContactEmail
        {
            get { return _ContactEmail; }
            set { SetPropertyValue(nameof(ContactEmail), ref _ContactEmail, value); }
        }
        #endregion
        #region BillToStreet1
        private string _BillStreet1;
        [ImmediatePostData]
        public string BillStreet1
        {
            get { return _BillStreet1; }
            set { SetPropertyValue(nameof(BillStreet1), ref _BillStreet1, value); }
        }
        #endregion

        #region BillStreet2
        private string _BillStreet2;
        [ImmediatePostData]
        public string BillStreet2
        {
            get { return _BillStreet2; }
            set { SetPropertyValue(nameof(BillStreet2), ref _BillStreet2, value); }
        }
        #endregion

        #region BillCity
        private string _BillCity;
        [ImmediatePostData]
        public string BillCity
        {
            get { return _BillCity; }
            set { SetPropertyValue(nameof(BillCity), ref _BillCity, value); }
        }
        #endregion

        #region BillState
        private string _BillState;
        [ImmediatePostData]
        public string BillState
        {
            get { return _BillState; }
            set { SetPropertyValue(nameof(BillState), ref _BillState, value); }
        }
        #endregion

        #region BillZipCode
        private string _BillZipCode;
        [ImmediatePostData]
        public string BillZipCode
        {
            get { return _BillZipCode; }
            set { SetPropertyValue(nameof(BillZipCode), ref _BillZipCode, value); }
        }
        #endregion

        #region BillCountry
        private string _BillCountry;
        [ImmediatePostData]
        public string BillCountry
        {
            get { return _BillCountry; }
            set { SetPropertyValue(nameof(BillCountry), ref _BillCountry, value); }
        }
        #endregion

        #region SameAddressForBothShippingAndBilling
        private bool _SameAddressForBothShippingAndBilling;
        [ImmediatePostData]
        public bool SameAddressForBothShippingAndBilling
        {
            get { return _SameAddressForBothShippingAndBilling; }
            set { SetPropertyValue(nameof(SameAddressForBothShippingAndBilling), ref _SameAddressForBothShippingAndBilling, value); }
        }
        #endregion

        #region ReportStreet1
        private string _ReportStreet1;
        public string ReportStreet1
        {
            get
            {
                if (BillStreet1 != null && SameAddressForBothShippingAndBilling == true)
                {
                    _ReportStreet1 = BillStreet1;
                }
                return _ReportStreet1;
            }
            set { SetPropertyValue(nameof(ReportStreet1), ref _ReportStreet1, value); }
        }
        #endregion

        #region ReportStreet2
        private string _ReportStreet2;
        public string ReportStreet2
        {
            get
            {
                if (BillStreet2 != null && SameAddressForBothShippingAndBilling == true)
                {
                    _ReportStreet2 = BillStreet2;
                }
                return _ReportStreet2;
            }
            set { SetPropertyValue(nameof(ReportStreet2), ref _ReportStreet2, value); }
        }
        #endregion

        #region ReportCity
        private string _ReportCity;
        public string ReportCity
        {
            get
            {
                if (BillCity != null && SameAddressForBothShippingAndBilling == true)
                {
                    _ReportCity = BillCity;
                }
                return _ReportCity;
            }
            set { SetPropertyValue(nameof(ReportCity), ref _ReportCity, value); }
        }
        #endregion

        #region ReportState
        private string _ReportState;
        public string ReportState
        {
            get
            {
                if (BillState != null && SameAddressForBothShippingAndBilling == true)
                {
                    _ReportState = BillState;
                }
                return _ReportState;
            }
            set { SetPropertyValue(nameof(ReportState), ref _ReportState, value); }
        }
        #endregion

        #region ReportZipCode
        private string _ReportZipCode;
        public string ReportZipCode
        {
            get
            {
                if (BillZipCode != null && SameAddressForBothShippingAndBilling == true)
                {
                    _ReportZipCode = BillZipCode;
                }
                return _ReportZipCode;
            }
            set { SetPropertyValue(nameof(ReportZipCode), ref _ReportZipCode, value); }
        }
        #endregion

        #region ReportCountry
        private string _ReportCountry;
        public string ReportCountry
        {
            get
            {
                if (BillCountry != null && SameAddressForBothShippingAndBilling == true)
                {
                    _ReportCountry = BillCountry;
                }
                return _ReportCountry;
            }
            set { SetPropertyValue(nameof(ReportCountry), ref _ReportCountry, value); }
        }
        #endregion
        #region DateReceived
        private DateTime _DateReceived;
        public DateTime DateReceived
        {
            get { return _DateReceived; }
            set { SetPropertyValue(nameof(DateReceived), ref _DateReceived, value); }
        }
        #endregion
        #region Notes Collection
        [DevExpress.ExpressApp.DC.Aggregated, Association("Notes_Invoice")]
        public XPCollection<Notes> Notes
        {
            get { return GetCollection<Notes>("Notes"); }
        }
        #endregion
        //#region ItemCharge Collection

        //[Association("Invoice-ItemCharge")]
        //public XPCollection<ItemChargePricing> ItemCharges
        //{
        //    get { return GetCollection<ItemChargePricing>("ItemCharges"); }
        //}
        //#endregion
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        private Employee _Submittedby;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee Submittedby
        {
            get { return _Submittedby; }
            set { SetPropertyValue(nameof(Submittedby), ref _Submittedby, value); }
        }
        private DateTime _SubmittedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime SubmittedDate
        {
            get { return _SubmittedDate; }
            set { SetPropertyValue(nameof(SubmittedDate), ref _SubmittedDate, value); }
        }
        private Employee _ReviewedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ReviewedBy
        {
            get { return _ReviewedBy; }
            set { SetPropertyValue(nameof(ReviewedBy), ref _ReviewedBy, value); }
        }
        private Nullable<DateTime> _DateReviewed;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Nullable<DateTime> DateReviewed
        {
            get { return _DateReviewed; }
            set { SetPropertyValue(nameof(DateReviewed), ref _DateReviewed, value); }
        }
        #region AccountNumber
        private string _AccountNumber;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { SetPropertyValue(nameof(AccountNumber), ref _AccountNumber, value); }
        }
        #endregion
        #region Report
        private byte[] _Report;
        [Size(SizeAttribute.Unlimited)]
        [Browsable(false)]
        public byte[] Report
        {
            get { return _Report; }
            set { SetPropertyValue(nameof(Report), ref _Report, value); }
        }
        #endregion

        #region Email
        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { SetPropertyValue(nameof(Email), ref _Email, value); }
        }
        #endregion
        #region RollbackedBy
        private Employee _RollbackedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Employee RollbackedBy
        {
            get { return _RollbackedBy; }
            set { SetPropertyValue(nameof(RollbackedBy), ref _RollbackedBy, value); }
        }
        #endregion
        #region RollbackedDate
        private DateTime _RollbackedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public DateTime RollbackedDate
        {
            get { return _RollbackedDate; }
            set { SetPropertyValue(nameof(RollbackedDate), ref _RollbackedDate, value); }
        }
        #endregion
        #region RollbackReason
        private String _RollbackReason;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public String RollbackReason
        {
            get { return _RollbackReason; }
            set { SetPropertyValue(nameof(_RollbackReason), ref _RollbackReason, value); }
        }
        #endregion
        #region ItemChargePricingCollection
        [DevExpress.ExpressApp.DC.Aggregated, Association("Invoicing-InvoicingItemCharge")]
        public XPCollection<InvoicingItemCharge> ItemCharges
        {
            get { return GetCollection<InvoicingItemCharge>(nameof(ItemCharges)); }
        }
        #endregion
        #region SentBy
        private Employee _SentBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Employee SentBy
        {
            get { return _SentBy; }
            set { SetPropertyValue(nameof(SentBy), ref _SentBy, value); }
        }
        #endregion
        #region DateSend
        private DateTime _DateSend;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public DateTime DateSend
        {
            get { return _DateSend; }
            set { SetPropertyValue(nameof(DateSend), ref _DateSend, value); }
        }
        #endregion
        private string _PO;
        [XafDisplayName("PO #")]
        public string PO
        {
            get { return _PO; }
            set { SetPropertyValue("PO", ref _PO, value); }
        }

        #region DueDateCompleted
        private DateTime _DueDateCompleted;
        public DateTime DueDateCompleted
        {
            get
            {
                if (!string.IsNullOrEmpty(JobID) && _DueDateCompleted == DateTime.MinValue && !IsSaving && !IsLoading)
                {
                    string[] strJob = JobID.Replace(" ", "").Split(';');
                    string strJobID = string.Join("','", strJob);
                    XPCollection<Reporting> litreport = new XPCollection<Reporting>(Session, CriteriaOperator.Parse(string.Format("[JobID.JobID] in ( '{0}')", strJobID/*JobID*/)));
                    if (litreport != null && litreport.Count > 0)
                    {
                        _DueDateCompleted = litreport.Select(i => i.ReportedDate).Max();
                    }
                }
                return _DueDateCompleted;
            }
            set { SetPropertyValue<DateTime>("DueDateCompleted", ref _DueDateCompleted, value); }
        }
        #endregion
    }
}
