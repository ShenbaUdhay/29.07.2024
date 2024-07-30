using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.TestPricing;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[NonPersistent]
    //[DomainComponent]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ConstituentPricingTier : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ConstituentPricingTier(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            //TierNo = 1;
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
        #region ConstituentPricing
        private ConstituentPricing _ConstituentPricing;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public ConstituentPricing ConstituentPricing
        {
            get { return _ConstituentPricing; }
            set { SetPropertyValue(nameof(ConstituentPricing), ref _ConstituentPricing, value); }
        }
        #endregion

        #region TierNo
        private uint _TierNo;
        //[NonPersistent]
        [ImmediatePostData(true)]
        [RuleRequiredField]
        //[RuleUniqueValue]
        //[VisibleInDetailView(false)]
        public uint TierNo
        {
            get { return _TierNo; }
            set { SetPropertyValue(nameof(TierNo), ref _TierNo, value); }
        }
        #endregion

        #region From
        private uint _From;
        //[NonPersistent]
        [RuleRequiredField]
        [ImmediatePostData]
        //[RuleUniqueValue]
        //[VisibleInDetailView(false)]
        public uint From
        {
            get { return _From; }
            set { SetPropertyValue(nameof(From), ref _From, value); }
        }
        #endregion

        #region To
        private uint _To;
        //[NonPersistent]
        [RuleRequiredField]
        [ImmediatePostData]
        //[RuleUniqueValue]
        //[VisibleInDetailView(false)]
        public uint To
        {
            get { return _To; }
            set { SetPropertyValue(nameof(To), ref _To, value); }
        }
        #endregion

        #region TierPrice
        private decimal _TierPrice;
        [RuleRequiredField]
        [ImmediatePostData]
        public decimal TierPrice
        {
            get { return _TierPrice; }
            set { SetPropertyValue(nameof(TierPrice), ref _TierPrice, Math.Round(value, 2)); }
        }
        #endregion

        #region Prep1Charge
        private decimal _Prep1Charge;
        [ImmediatePostData]
        public decimal Prep1Charge
        {
            get { return _Prep1Charge; }
            set { SetPropertyValue(nameof(Prep1Charge), ref _Prep1Charge, Math.Round(value, 2)); }
        }
        #endregion

        #region Prep2Charge
        private decimal _Prep2Charge;
        [ImmediatePostData]
        public decimal Prep2Charge
        {
            get { return _Prep2Charge; }
            set { SetPropertyValue(nameof(Prep2Charge), ref _Prep2Charge, Math.Round(value, 2)); }
        }
        #endregion

        #region TotalTierPrice
        private decimal _TotalTierPrice;
        //[ImmediatePostData]
        public decimal TotalTierPrice
        {
            get
            {
                //if (TierPrice > 0)
                //{
                //    decimal totalprice = TierPrice + Prep1Charge + Prep2Charge;
                //    _TotalTierPrice = Math.Round(totalprice,2);
                //}
                //else
                //{
                //    TotalTierPrice = 0;
                //    Prep1Charge = 0;
                //    Prep2Charge = 0;
                //}
                return _TotalTierPrice;
            }
            set { SetPropertyValue(nameof(TotalTierPrice), ref _TotalTierPrice, Math.Round(value, 2)); }
        }
        #endregion

        #region TestMethod
        private string _TestMethod;
        [ImmediatePostData]
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[VisibleInDetailView(false)]
        public string TestMethod
        {
            get
            {
                string strtest = string.Empty;
                if (ConstituentPricing != null && ConstituentPricing.Matrix != null && ConstituentPricing.Test != null)
                {
                    strtest = ConstituentPricing.Matrix.MatrixName + "| " + ConstituentPricing.Test.TestName;
                }
                if (ConstituentPricing != null && ConstituentPricing.Method != null && !string.IsNullOrEmpty(strtest))
                {
                    strtest = strtest + "| " + ConstituentPricing.Method.MethodNumber;
                }
                if (ConstituentPricing != null && ConstituentPricing.Component != null && !string.IsNullOrEmpty(strtest))
                {
                    strtest = strtest + "| " + ConstituentPricing.Component.Components;
                }
                if (!string.IsNullOrEmpty(strtest))
                {
                    _TestMethod = strtest;
                }
                return _TestMethod;
            }
            set { SetPropertyValue(nameof(TestMethod), ref _TestMethod, value); }
        }
        #endregion

        #region ConstituentPricings
        private ConstituentPricing _ConstituentPricings;
        [Association("ConstituentPricing-ConstituentPricingTier")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public ConstituentPricing ConstituentPricings
        {
            get { return _ConstituentPricings; }
            set { SetPropertyValue(nameof(ConstituentPricings), ref _ConstituentPricings, value); }
        }
        #endregion

    }
}