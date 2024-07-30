using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.Invoicing;
using System;

namespace Modules.BusinessObjects.Accounting.Receivables
{
    public enum DepositStatus
    {
        Unpaid,
        [XafDisplayName("Partially Paid")]
        PartiallyPaid,
        Paid
    }
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Deposits : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Deposits(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }
        protected override void OnSaving()
        {
            if (string.IsNullOrEmpty(DepositID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(DepositID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(Deposits), criteria, null)) + 1).ToString("000");
                var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "001")
                {
                    var predate = tempID.Substring(0, 6);
                    if (predate == curdate)
                    {
                        tempID = "DP" + tempID;
                    }
                    else
                    {
                        tempID = "DP" + curdate + "001";
                    }
                }
                else
                {
                    tempID = "DP" + curdate + "001";
                }
                DepositID = tempID;
            }
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        #region DepositID
        private string _DepositID;
        public string DepositID
        {
            get { return _DepositID; }
            set { SetPropertyValue(nameof(DepositID), ref _DepositID, value); }
        }
        #endregion
        #region InvoiceID
        private Invoicing _InvoiceID;
        public Invoicing InvoiceID
        {
            get { return _InvoiceID; }
            set { SetPropertyValue(nameof(InvoiceID), ref _InvoiceID, value); }
        }
        #endregion
        #region Tittle
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue(nameof(Title), ref _Title, value); }
        }
        #endregion
        #region Description
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
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
        #region AmountPaid
        private decimal _AmountPaid;
        public decimal AmountPaid
        {
            get { return _AmountPaid; }
            set { SetPropertyValue(nameof(AmountPaid), ref _AmountPaid, value); }
        }
        #endregion
        #region Balance
        private decimal _Balance;
        public decimal Balance
        {
            get { return _Balance; }
            set { SetPropertyValue(nameof(Balance), ref _Balance, value); }
        }
        #endregion
        #region Term
        private string _Term;
        public string Term
        {
            get { return _Term; }
            set { SetPropertyValue(nameof(Term), ref _Term, value); }
        }
        #endregion
        #region DueDate
        private DateTime _DueDate;
        public DateTime DueDate
        {
            get { return _DueDate; }
            set { SetPropertyValue(nameof(DueDate), ref _DueDate, value); }
        }
        #endregion
        #region DaysPastDue
        private int _DaysPastDue;
        public int DaysPastDue
        {
            get
            {
                if (DueDate != DateTime.MinValue && DueDate < DateTime.Today)
                {
                    _DaysPastDue = Math.Abs(((DateTime)DueDate).Subtract(DateTime.Today).Days);
                }
                return _DaysPastDue;
            }
            set { SetPropertyValue<int>("DaysPastDue", ref _DaysPastDue, value); }
        }
        #endregion
        #region Client
        private Customer _Client;
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
                return _ProjectID;
            }
            set { SetPropertyValue(nameof(ProjectID), ref _ProjectID, value); }
        }
        #endregion
        #region Status
        private DepositStatus _Status;
        public DepositStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        #endregion
        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion
        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion
        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion
        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion
        #region DepositPayment
        [Association("Deposits-DepositPayment")]
        public XPCollection<DepositPayment> DepositPayments
        {
            get { return GetCollection<DepositPayment>("DepositPayments"); }
        }
        #endregion
        #region Note
        [Association("Deposits-Note")]
        public XPCollection<Notes> Note
        {
            get { return GetCollection<Notes>("Note"); }
        }
        #endregion
        #region DateInvoiced
        private DateTime _DateInvoiced;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime DateInvoiced
        {
            get { return _DateInvoiced; }
            set { SetPropertyValue(nameof(DateInvoiced), ref _DateInvoiced, value); }
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
        #region CreditRating
        private CreditRating _CreditRating;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        //[ImmediatePostData]
        [NonPersistent]
        public CreditRating CreditRating
        {
            get
            {
                if (_CreditRating == null && _Client != null && _Client.CreditRating != null)
                {
                    _CreditRating = _Client.CreditRating;
                }
                return _CreditRating;
            }
            set { SetPropertyValue("CreditRating", ref _CreditRating, value); }
        }
        #endregion
    }
}