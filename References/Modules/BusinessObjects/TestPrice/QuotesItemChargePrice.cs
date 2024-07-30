using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.Setting.Quotes
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class QuotesItemChargePrice : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public QuotesItemChargePrice(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            Createdby = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            Modifiedby = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            Qty = 1;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        //#region ItemCode
        //private string _ItemCode;
        ////[ImmediatePostData]
        //public string ItemCode
        //{
        //    get { return _ItemCode; }
        //    set { SetPropertyValue(nameof(ItemCode), ref _ItemCode, value); }
        //}
        //#endregion
        //#region ItemName
        //private string _ItemName;
        //[RuleRequiredField]
        //public string ItemName
        //{
        //    get { return _ItemName; }
        //    set { SetPropertyValue(nameof(ItemName), ref _ItemName, value); }
        //}
        //#endregion
        #region Description
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        //[RuleRequiredField]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        #endregion
        //#region Category
        //private ItemChargePricingCategory _Category;
        ////[RuleRequiredField]
        //public ItemChargePricingCategory Category
        //{
        //    get { return _Category; }
        //    set { SetPropertyValue(nameof(Category), ref _Category, value); }
        //}
        //#endregion
        //#region Units
        //private Unit _Units;
        //[RuleRequiredField]
        //public Unit Units
        //{
        //    get { return _Units; }
        //    set { SetPropertyValue(nameof(Units), ref _Units, value); }
        //}
        //#endregion

        #region UnitPrice
        private decimal _UnitPrice;
        [ImmediatePostData]
        [RuleRequiredField]
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref _UnitPrice, Math.Round(value, 2)); }
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

        #region Qty
        private uint _Qty;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [ImmediatePostData]
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

        #region Discount
        private decimal _Discount;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [ImmediatePostData]
        public decimal Discount
        {
            get { return _Discount; }
            set { SetPropertyValue(nameof(Discount), ref _Discount, Math.Round(value, 2)); }
        }
        #endregion
        #region FinalAmount
        private decimal _FinalAmount;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [ImmediatePostData]
        public decimal FinalAmount
        {
            get
            {
                //if (Amount > 0 && Discount == 0 && Qty> 0)
                //{
                //    decimal discountamt = (Amount *(Discount / 100));
                //    _FinalAmount = Qty * (Amount - discountamt);
                //}
                //if (Discount == 0 && Qty > 0 && Amount > 0)
                //{
                //    _FinalAmount = Qty * Amount;
                //}
                //else if (Discount > 0 && Qty > 0 && Amount > 0)
                //{
                //    decimal disamount = Amount * (Discount / 100);
                //    _FinalAmount = Qty * (Amount - disamount);
                //}
                //else if (Discount < 0 && Qty > 0 && Amount > 0)
                //{
                //    decimal disamount = Amount * (Discount / 100);
                //    _FinalAmount = Qty * (Amount + disamount);
                //}
                return _FinalAmount;
            }
            set { SetPropertyValue(nameof(FinalAmount), ref _FinalAmount, Math.Round(value, 2)); }
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
        private CustomSystemUser _Modifiedby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser Modifiedby
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
        private CustomSystemUser _Createdby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser Createdby
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

        #region Quotes
        private CRMQuotes _CRMQuotes;
        //[Association("CRMQuotes-ItemChargePricing")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public CRMQuotes CRMQuotes

        {
            get { return _CRMQuotes; }
            set { SetPropertyValue(nameof(CRMQuotes), ref _CRMQuotes, value); }
        }
        #endregion

        #region ItemPrice
        private ItemChargePricing _ItemPrice;
        //[Association("CRMQuotes-ItemChargePricing")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public ItemChargePricing ItemPrice

        {
            get { return _ItemPrice; }
            set { SetPropertyValue(nameof(ItemPrice), ref _ItemPrice, value); }
        }
        #endregion

        #region Quotes
        private CRMQuotes _Quotes;
        [Association("CRMQuotes-QuotesItemChargePrice")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public CRMQuotes Quotes

        {
            get { return _Quotes; }
            set { SetPropertyValue(nameof(Quotes), ref _Quotes, value); }
        }
        #endregion

    }
}