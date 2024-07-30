using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.Setting.Invoicing
{
    [DefaultClassOptions]

    public class InvoicingItemCharge : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InvoicingItemCharge(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region UnitPrice
        private decimal _UnitPrice;
        [RuleRequiredField]
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref _UnitPrice, Math.Round(value, 2)); }
        }
        #endregion

        #region Qty
        private uint _Qty;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[ImmediatePostData]
        public uint Qty
        {
            get
            {
                if (_Qty <= 0)
                {
                    _Qty = 1;
                    //Amount = UnitPrice;
                }
                return _Qty;
            }
            set { SetPropertyValue(nameof(Qty), ref _Qty, value); }
        }
        #endregion

        #region Amount
        private decimal _Amount;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[ImmediatePostData]
        public decimal Amount
        {
            get { return _Amount; }
            set { SetPropertyValue(nameof(Amount), ref _Amount, Math.Round(value, 2)); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }
        #endregion
        #region Modifiedby
        private Employee _Modifiedby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Employee Modifiedby
        {
            get { return _Modifiedby; }
            set { SetPropertyValue("Modifiedby", ref _Modifiedby, value); }
        }
        #endregion
        #region ModifiedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }
        #endregion
        #region Createdby
        private Employee _Createdby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public Employee Createdby
        {
            get { return _Createdby; }
            set { SetPropertyValue("Createdby", ref _Createdby, value); }
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
        #region ItemPrice
        private ItemChargePricing _ItemPrice;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public ItemChargePricing ItemPrice

        {
            get { return _ItemPrice; }
            set { SetPropertyValue(nameof(ItemPrice), ref _ItemPrice, value); }
        }
        #endregion

        #region Invoicing
        private Invoicing _Invoicing;
        [Association("Invoicing-InvoicingItemCharge")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Invoicing Invoicing

        {
            get { return _Invoicing; }
            set { SetPropertyValue(nameof(Invoicing), ref _Invoicing, value); }
        }
        #endregion
        #region FinalAmount
        private decimal _FinalAmount;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal FinalAmount
        {
            get
            {

                return _FinalAmount;
            }
            set { SetPropertyValue(nameof(FinalAmount), ref _FinalAmount, Math.Round(value, 2)); }
        }
        #endregion
        #region NpUnitPrice
        private decimal _NpUnitPrice;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[ImmediatePostData]
        //[RuleRequiredField]
        public decimal NpUnitPrice
        {
            get { return _NpUnitPrice; }
            set { SetPropertyValue(nameof(NpUnitPrice), ref _NpUnitPrice, Math.Round(value, 2)); }
        }
        #endregion
        #region Discount
        private decimal _Discount;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[ImmediatePostData]
        public decimal Discount
        {
            get { return _Discount; }
            set { SetPropertyValue(nameof(Discount), ref _Discount, Math.Round(value, 2)); }
        }
        #endregion
        #region Description
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        #endregion

    }
}