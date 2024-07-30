using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace Modules.BusinessObjects.Accounts
{
    public enum ProspectsStatus
    {
        None = 0,
        Won = 1,
        Cancelled = 2
        //[XafDisplayName("Out-Sold")]
        //OutSold = 3
    }
    public enum ProspectsRating
    {
        Hot = 0,
        Warm = 1,
        Cold = 2
    }

    public class ProspectsValidationRules
    {
        public const string ProbabilityIsBeetwen0And100 = "ProbabilityyIsBeetwen0And100";
    }

    public interface IProspectsCustomer
    {
        IList<Prospects> Opportunities { get; set; }
    }

    public interface IProspectsTarget
    {
        Prospects SourceProspects { get; set; }
    }

    public interface IProspectsInvoice : IProspectsTarget
    {
        Prospects BackRefProspects { get; set; }
    }

    [VisibleInReports]
    [ImageName("BO_Prospects")]
    [Appearance("Disable IProspects by StatusReason", TargetItems = "*", Criteria = "[Status] <> ##Enum#Modules.BusinessObjects.Accounts.ProspectsStatus,None#", Enabled = false)]
    [Appearance("Hide Amount, Discount, DiscountPercent by Empty Criteria", TargetItems = "Amount, Discount, DiscountPercent", Criteria = "", Visibility = ViewItemVisibility.Hide)]
    [Appearance("Enable EstimatedRevenue", TargetItems = "EstimatedRevenue", Criteria = "RevenueIsUserProvided", Enabled = true)]
    [Appearance("Disable EstimatedRevenue", TargetItems = "EstimatedRevenue", Criteria = "!RevenueIsUserProvided", Enabled = false)]
    [ListViewAutoFilterRowAttribute(true)]
    [Table("Opportunities")]
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Prospects : SaleBase, INotifyPropertyChanged
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Prospects(Session session)
            : base(session)
        {
            //Invoices = new List<CRMInvoice>();
            //Quotes = new List<Quote>();
            Orders = new List<Order>();
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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

        private bool revenueIsUserProvided;
        private Customer customer;
        private decimal _EstimatedRevenue;
        private decimal _ActualRevenue;
        private int _Probability;

        private ProspectsRating _Rating;
        public ProspectsRating Rating { get { return _Rating; } set { SetPropertyValue<ProspectsRating>("Rating", ref _Rating, value); } }

        public decimal EstimatedRevenue { get { return _EstimatedRevenue; } set { SetPropertyValue<decimal>("EstimatedRevenue", ref _EstimatedRevenue, value); } }
        [VisibleInListView(false)]
        public decimal ActualRevenue { get { return _ActualRevenue; } set { SetPropertyValue<decimal>("ActualRevenue", ref _ActualRevenue, value); } }
        [RuleRange(ProspectsValidationRules.ProbabilityIsBeetwen0And100, DefaultContexts.Save, 0, 100)]
        public int Probability { get { return _Probability; } set { SetPropertyValue<int>("Probability", ref _Probability, value); } }

        private ProspectsStatus _Status;
        public ProspectsStatus Status { get { return _Status; } set { SetPropertyValue<ProspectsStatus>("Status", ref _Status, value); } }

        //private bool _IsClose;
        //[VisibleInListView(false), VisibleInDetailView(false),VisibleInLookupListView(false)]
        //public bool IsClose { get { return _IsClose; } set { SetPropertyValue<bool>("IsClose", ref _IsClose, value); } }

        private Nullable<DateTime> _CloseDate;
        [VisibleInListView(false)]
        public Nullable<DateTime> CloseDate { get { return _CloseDate; } set { SetPropertyValue<Nullable<DateTime>>("CloseDate", ref _CloseDate, value); } }

        private Nullable<DateTime> _EstimatedCloseDate;
        [VisibleInListView(false)]
        public Nullable<DateTime> EstimatedCloseDate { get { return _EstimatedCloseDate; } set { SetPropertyValue<Nullable<DateTime>>("EstimatedCloseDate", ref _EstimatedCloseDate, value); } }

        //[InverseProperty("BackRefProspects")]
        //public virtual IList<CRMInvoice> Invoices { get; set; }

        [ImmediatePostData]
        [VisibleInListView(false)]
        public bool RevenueIsUserProvided
        {
            get { return revenueIsUserProvided; }
            set
            {
                SetPropertyValue<bool>("RevenueIsUserProvided", ref revenueIsUserProvided, value);
                //UpdateAmount();
                //Fix for EF & Conditional Appearance
            }
        }
        [System.ComponentModel.Browsable(false)]
        [InverseProperty("Prospects")]
        [ImmediatePostData]
        public Customer Customer
        {
            get { return customer; }
            set
            {
                SetPropertyValue<Customer>(nameof(Customer), ref customer, value);
                PotentialCustomer = Customer;
            }
        }

        //[InverseProperty("BackRefProspects")]
        //public virtual IList<Quote> Quotes { get; set; }

        [InverseProperty("Prospects")]
        public virtual IList<Order> Orders { get; set; }

        //public override void UpdateAmount()
        //{
        //    if (!IsLoaded && !IsCreated) { return; }

        //    Amount = DetailAmount;

        //    if (!RevenueIsUserProvided)
        //    {
        //        EstimatedRevenue = Amount;
        //    }
        //}
        protected override void PotentialCustomerUpdated()
        {
            if (Customer != PotentialCustomer)
            {
                Customer = PotentialCustomer;
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, args);
            }
        }

        //TODO: Move to controller
        //[Action(PredefinedCategory.View, Caption = "Close Opportunity...", AutoCommit = true,
        //    //TargetObjectsCriteria = "Status = ##Modules.BusinessObjects.Accounts.OpportunityStatus,None#",
        //    SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject)]
        //public void Close(CloseOpportunityParameters parameters)
        //{
        //    Status = parameters.Status;
        //    CloseDate = parameters.CloseDate;
        //    ActualRevenue = parameters.ActualRevenue;
        //}

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [DomainComponent]
    public class CloseProspectsParameters : BaseObject
    {
        //public CloseOpportunityParameters(Session session)
        //   : base(session)
        //{

        //}
        public CloseProspectsParameters(Prospects prospects)
        {
            this.CloseDate = DateTime.Now;
            this.ActualRevenue = prospects.Amount;
        }
        //public ClosePerspectsParameters()
        //{
        //    this.CloseDate = DateTime.Now;Perspects
        //    //this.ActualRevenue = Perspects.Amount;
        //}
        private ProspectsStatus _Status;
        public ProspectsStatus Status { get { return _Status; } set { SetPropertyValue<ProspectsStatus>("Status", ref _Status, value); } }

        private decimal _ActualRevenue;
        public decimal ActualRevenue { get { return _ActualRevenue; } set { SetPropertyValue<decimal>("ActualRevenue", ref _ActualRevenue, value); } }

        private Nullable<DateTime> _CloseDate;
        public Nullable<DateTime> CloseDate { get { return _CloseDate; } set { SetPropertyValue<Nullable<DateTime>>("CloseDate", ref _CloseDate, value); } }


    }
}