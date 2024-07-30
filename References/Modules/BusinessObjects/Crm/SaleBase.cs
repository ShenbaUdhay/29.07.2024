using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Accounts
{
    public class SaleBaseValidationRules
    {
        public const string NameIsRequired = "SaleBaseNameIsRequired";
        public const string DetailAmountGreaterthanZero = "DetailamountmustbegreaterthanZero";
        // public const string PotentialCustomerIsRequired = "PotentialCustomerIsRequired";
        public const string DiscountIsGreaterThanOrEqualZero = "SaleBaseDiscountIsGreaterThanOrEqualZero";
        public const string DiscountPercentIn0_100Range = "SaleBaseDiscountPercentIn0_100Range";
    }

    public enum ShippingMethod
    {
        None = 0,
        Airborne = 1,
        DHL = 2,
        FedEx = 3,
        UPS = 4,
        PostalMail = 5,
        FullLoad = 6,
        WillCall = 7
    }
    [DefaultClassOptions]
    //[Appearance("Disable Amount", TargetItems = "Amount", Criteria = "", Enabled = false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //[RuleCriteria("valDetailAmount", DefaultContexts.Save, "[DetailAmount] > 0", "Detail amount should be greater than 0")]
    //[RuleCombinationOfPropertiesIsUnique("SaleBase", DefaultContexts.Save, "Name", SkipNullOrEmptyValues = false)]
    public abstract class SaleBase : BaseObject, IObjectSpaceLink, IXafEntityObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SaleBase(Session session)
            : base(session)
        {
            //SaleItems = new List<SaleItem>();
        }

        private bool isLoaded = false;
        private bool isCreated = false;
        private decimal discount;
        private decimal discountPercent;
        private Customer potentialCustomer;
        SaleBaseInfo info = new SaleBaseInfo();
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private int _SaleBaseId;
        private string _Name;
        private decimal _Amount;
        private string _ID;

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [ImmediatePostData]
        public int SaleBaseId { get { return _SaleBaseId; } set { SetPropertyValue<int>("SaleBaseId", ref _SaleBaseId, value); } }

        [SearchMemberOptions(SearchMemberMode.Exclude)]
        public string DisplayName
        {
            get { return ReflectionHelper.GetObjectDisplayText(this); }
        }

        protected bool IsLoaded { get { return isLoaded; } }

        protected bool IsCreated { get { return isCreated; } }

        private bool _bolNotRuleRequired;
        [NonPersistent]
        public bool NotRuleRequired
        {
            get
            {
                if (info.ViewID == "Payables_DetailView")
                {
                    _bolNotRuleRequired = true;
                }
                else
                {
                    _bolNotRuleRequired = false;
                }
                return _bolNotRuleRequired;
            }
        }
        public bool IsRuleRequired
        {
            get
            {
                return info.IsCRMQuotes;
            }
        }

        // [RuleRequiredField(SaleBaseValidationRules.NameIsRequired, DefaultContexts.Save, TargetCriteria = "NotRuleRequired=false", CustomMessageTemplate = "\"Topic\" must not be empty")]
        public string Name { get { return _Name; } set { SetPropertyValue<string>("Name", ref _Name, value); } }

        // [RuleRequiredField(SaleBaseValidationRules.PotentialCustomerIsRequired, DefaultContexts.Save)]

        ReportingToInfo objInfo = new ReportingToInfo();
        [DataSourceProperty(nameof(OpportunityCustomer), DataSourcePropertyIsNullMode.SelectNothing)]
        [ImmediatePostData]
        public Customer PotentialCustomer
        {
            get { return potentialCustomer; }
            set { SetPropertyValue<Customer>("PotentialCustomer", ref potentialCustomer, value); PotentialCustomerUpdated(); }
        }

        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false)]
        public XPCollection<Customer> OpportunityCustomer
        {
            get
            {
                if (SecuritySystem.CurrentUserName == "Administrator")
                {
                    return new XPCollection<Customer>(Session);
                }
                else
                {
                    return new XPCollection<Customer>(Session, CriteriaOperator.Parse("[Owner.Oid] In(" + string.Format("'{0}'", string.Join("','", objInfo.ReportingTo.Select(i => i.ToString().Replace("'", "''")))) + ") or [CreatedBy.Oid] In(" + string.Format("'{0}'", string.Join("','", objInfo.ReportingTo.Select(i => i.ToString().Replace("'", "''")))) + ") "));
                }
            }
        }
        [ImmediatePostData]
        public decimal Amount { get { return _Amount; } set { SetPropertyValue<decimal>("Amount", ref _Amount, value); } }

        //[Association("SaleBase-SaleBaseItem")]
        //public XPCollection<SaleItem> SaleItem
        //{
        //    get { return GetCollection<SaleItem>("SaleItem"); }
        //}

        //[DevExpress.ExpressApp.DC.Aggregated]
        //[InverseProperty("SaleBase")]
        //public virtual IList<SaleItem> SaleItems { get; set; }

        private Employee _Owner;
        [VisibleInListView(false)]
        public Employee Owner { get { return _Owner; } set { SetPropertyValue<Employee>("Owner", ref _Owner, value); } }

        [RuleValueComparison(SaleBaseValidationRules.DiscountIsGreaterThanOrEqualZero, DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        [ImmediatePostData]
        [VisibleInListView(false)]
        public decimal Discount
        {
            get { return discount; }
            set { SetPropertyValue<decimal>("Discount", ref discount, value); }// UpdateAmount(); }
        }


        [RuleRange(SaleBaseValidationRules.DiscountPercentIn0_100Range, DefaultContexts.Save, 0, 100)]
        [ImmediatePostData]
        //[ModelDefault("DisplayFormat", "{0:N2}")]
        //[ModelDefault("EditMask", "N2")]
        [VisibleInListView(false)]
        public decimal DiscountPercent
        {
            get { return discountPercent; }
            set { SetPropertyValue<decimal>("DiscountPercent", ref discountPercent, value); }// UpdateAmount(); }
        }

        private decimal _DetailAmount;

        //[Appearance("DetailAmount", Enabled = false, Context = "DetailView")]
        [ImmediatePostData]
        [VisibleInListView(false)]
        [RuleRequiredField(SaleBaseValidationRules.DetailAmountGreaterthanZero, DefaultContexts.Save, TargetCriteria = "IsRuleRequired=true")]
        public decimal DetailAmount
        {
            get
            {
                return _DetailAmount;

                //decimal amount = 0;
                //if (!Session.IsObjectsLoading && !Session.IsObjectsSaving)
                //{
                //    foreach (SaleItem saleItem in SaleItem)
                //    {
                //        amount += saleItem.Amount;
                //    }
                //}
                //return amount;
            }
            set { SetPropertyValue<decimal>("DetailAmount", ref _DetailAmount, value); }
        }

        [VisibleInListView(false)]
        [Appearance("ID", Enabled = false, Context = "DetailView")]
        public string ID { get { return _ID; } set { SetPropertyValue<string>("ID", ref _ID, value); } }

        private Nullable<DateTime> _CreatedOn;
        [VisibleInDetailView(false), VisibleInListView(false)]
        public Nullable<DateTime> CreatedOn { get { return _CreatedOn; } set { SetPropertyValue<Nullable<DateTime>>("CreatedOn", ref _CreatedOn, value); } }

        //public void Copy(SaleBase target)
        //{
        //    //rewrite with Cloner - S35833
        //    target.Discount = Discount;
        //    target.DiscountPercent = DiscountPercent;
        //    target.Name = Name;
        //    target.Owner = Owner;
        //    target.PotentialCustomer = PotentialCustomer;

        //    foreach (SaleItem saleItem in SaleItem)
        //    {
        //        SaleItem saleItemNew = ObjectSpace.CreateObject<SaleItem>();
        //        saleItemNew.Discount = saleItem.Discount;
        //        saleItemNew.Product = saleItem.Product;
        //        saleItemNew.Quantity = saleItem.Quantity;
        //        //saleItemNew.UpdateAmount();

        //        target.SaleItem.Add(saleItemNew);
        //    }
        //    //target.UpdateAmount();

        //    if (target is IOpportunityTarget)
        //    {
        //        if (this is Opportunity)
        //        {
        //            ((IOpportunityTarget)target).SourceOpportunity = (Opportunity)this;
        //        }
        //        else if (this is IOpportunityTarget)
        //        {
        //            ((IOpportunityTarget)target).SourceOpportunity = ((IOpportunityTarget)this).SourceOpportunity;
        //        }
        //    }
        //    //if (this is IGenericAddressableSale)
        //    //{
        //    //    ((IGenericAddressableSale)this).Copy(target as IGenericAddressableSale);
        //    //}
        //}

        //public virtual void UpdateAmount()
        //{
        //    if (!IsLoaded && !IsCreated)
        //    {
        //        return;
        //    }
        //    Amount = DetailAmount - Discount - (DetailAmount - Discount) * DiscountPercent / 100;
        //}

        protected abstract void PotentialCustomerUpdated();

        #region IObjectSpaceLink
        [Browsable(false)]
        public IObjectSpace ObjectSpace { get; set; }
        #endregion

        #region IXafEntityObject
        public virtual void OnCreated()
        {
            CreatedOn = DateTime.Now;
            isCreated = true;
        }
        public virtual void OnLoaded()
        {
            isLoaded = true;
        }
        public virtual void OnSaving() { }
        #endregion
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
    }

    //public interface IGenericAddressableSale
    //{
    //    XCRMAddress BillToAddress { get; set; }
    //    XCRMAddress ShipToAddress { get; set; }
    //    Nullable<DateTime> DeliveryDate { get; set; }
    //    ShippingMethod? ShippingMethod { get; set; }
    //    void Copy(IGenericAddressableSale target);
    //}

    //public static class IGenericAddressableSaleLogic
    //{
    //    public static void Copy(IGenericAddressableSale source, IGenericAddressableSale target)
    //    {
    //        if (source != null && target != null)
    //        {
    //            target.BillToAddress = source.BillToAddress;
    //            target.ShipToAddress = source.ShipToAddress;
    //            target.ShippingMethod = source.ShippingMethod;
    //            target.DeliveryDate = source.DeliveryDate;
    //        }
    //    }
    //}
}