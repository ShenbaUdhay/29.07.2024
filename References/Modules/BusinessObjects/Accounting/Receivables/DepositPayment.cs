using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using System;

namespace Modules.BusinessObjects.Accounting.Receivables
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DepositPayment : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public DepositPayment(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Date = DateTime.Now;
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }
        #region Name
        private string _Name;
        [RuleRequiredField(DefaultContexts.Save)]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(nameof(Name), ref _Name, value); }
        }
        #endregion
        #region InvoiceAmuont
        private decimal _InvoiceAmuont;
        public decimal InvoiceAmuont
        {
            get { return _InvoiceAmuont; }
            set { SetPropertyValue(nameof(InvoiceAmuont), ref _InvoiceAmuont, value); }
        }
        #endregion
        #region AmountReceived
        private decimal _AmountReceived;
        [ImmediatePostData(true)]
        public decimal AmountReceived
        {
            get { return _AmountReceived; }
            set { SetPropertyValue(nameof(AmountReceived), ref _AmountReceived, value); }
        }
        #endregion
        #region SumAmountReceived
        private decimal _SumAmountReceived;
        public decimal SumAmountReceived
        {
            get { return _SumAmountReceived; }
            set { SetPropertyValue(nameof(SumAmountReceived), ref _SumAmountReceived, value); }
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
        #region Remark
        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        #endregion
        #region Date
        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue(nameof(Date), ref _Date, value); }
        }
        #endregion
        #region Attachment
        [Association("DepositPayment-Attachment")]
        public XPCollection<Attachment> Attachment
        {
            get { return GetCollection<Attachment>("Attachment"); }
        }
        #endregion
        #region Region
        private Region _Region;
        public Region Region
        {
            get { return _Region; }
            set { SetPropertyValue(nameof(Region), ref _Region, value); }
        }
        #endregion
        #region Deposits
        private Deposits _Deposits;
        [Association("Deposits-DepositPayment")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Deposits Deposits
        {
            get { return _Deposits; }
            set { SetPropertyValue(nameof(Deposits), ref _Deposits, value); }
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
    }
}