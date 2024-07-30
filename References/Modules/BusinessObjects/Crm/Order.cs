using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modules.BusinessObjects.Accounts
{
    public enum OrderStatus
    {
        None = 0,
        [XafDisplayName("No Money")]
        NoMoney = 1,
        [XafDisplayName("Cusomer Canceled")]
        CustomerCanceled = 2
    }

    public interface IOrderCustomer
    {
        IList<Order> Orders { get; set; }
    }

    public interface IOrderOpportunity
    {
        IList<Order> Orders { get; set; }
    }

    public interface IOrderTarget
    {
        Order SourceOrder { get; set; }
    }

    [VisibleInReports]
    [ImageName("BO_Order")]
    [Appearance("Disable IOrder by StatusReason", TargetItems = "*", Criteria = "[Status] <> ##Enum#XCRM.Module.BusinessObjects.OrderStatus,None#", Enabled = false)]
    //[ListViewFilter("Closed Orders", "[Status] != ##XCRM.Module.BusinessObjects.OrderStatus,None#", "Closed Orders", Index = 0)]
    //[ListViewFilter("My Open Orders", "[Owner.ID] = custom('CurrentUserId') AND [Status] = ##XCRM.Module.BusinessObjects.OrderStatus,None#", "My Open Orders", true, Index = 1)]
    //[ListViewFilter("Open Orders", "[Status] == ##XCRM.Module.BusinessObjects.OrderStatus,None#", "Open Orders", Index = 2)]
    [Table("Orders")]
    [DefaultClassOptions]
    [ListViewAutoFilterRowAttribute(true)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Order : SaleBase, IProspectsTarget, IObjectSpaceLink
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Order(Session session)
            : base(session)
        {
            //Invoices = new List<CRMInvoice>();
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

        private Prospects BackRefProspect;
        private Customer backRefCustomer;

        private OrderStatus _Status;
        public OrderStatus Status { get { return _Status; } set { SetPropertyValue<OrderStatus>("Status", ref _Status, value); } }

        private Nullable<DateTime> _CancelDate;
        [VisibleInListView(false)]
        public Nullable<DateTime> CancelDate { get { return _CancelDate; } set { SetPropertyValue<Nullable<DateTime>>("CancelDate", ref _CancelDate, value); } }

        //private Quote _SourceQuote;
        //[Association("Quote-Order")]
        //[VisibleInListView(false)]
        //[ImmediatePostData]
        //public Quote SourceQuote { get { return _SourceQuote; } set { SetPropertyValue<Quote>("SourceQuote", ref _SourceQuote, value); } }

        [System.ComponentModel.Browsable(false)]
        [VisibleInListView(false)]
        public Customer BackRefCustomer
        {
            get { return backRefCustomer; }
            set { SetPropertyValue<Customer>("BackRefCustomer", ref backRefCustomer, value); PotentialCustomer = BackRefCustomer; }
        }

        [System.ComponentModel.Browsable(false)]
        [VisibleInListView(false)]
        public virtual Prospects Prospect
        {
            get { return BackRefProspect; }
            set { SetPropertyValue<Prospects>("Opportunity", ref BackRefProspect, value); SourceProspects = Prospect; }
        }

        //[InverseProperty("SourceOrder")]
        //public virtual IList<CRMInvoice> Invoices { get; set; }


        #region IOpportunityTarget
        private Prospects sourceProspects;
        [VisibleInListView(false)]
        //[NotMapped]
        public virtual Prospects SourceProspects
        {
            get { return sourceProspects; }
            set
            {
                SetPropertyValue<Prospects>("SourceOpportunity", ref sourceProspects, value);
                SourceOpportunityUpdated();
            }
        }
        protected virtual void SourceOpportunityUpdated()
        {
            if (Prospect != SourceProspects)
            {
                Prospect = SourceProspects;
            }
        }
        #endregion

        //[Action(PredefinedCategory.View, Caption = "Cancel Order...", AutoCommit = true, TargetObjectsCriteria = "Status = ##XCRM.Module.BusinessObjects.OrderStatus,None#",
        //    SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject)]
        public void Cancel(CancelOrderParameters parameters)
        {

            Status = parameters.Status;
            CancelDate = parameters.CancelDate;
            ObjectSpace.SetModified(this);
        }


        //[Action(PredefinedCategory.View, Caption = "Create Invoice...", AutoCommit = true,
        //  SelectionDependencyType = MethodActionSelectionDependencyType.RequireSingleObject)]
        //public void CreateInvoiceFromOrder()
        //{
        //    //TODO: Move to container

        //    CRMInvoice invoice = ObjectSpace.CreateObject<CRMInvoice>();
        //    Copy(invoice);
        //    Invoices.Add(invoice);
        //}
        protected override void PotentialCustomerUpdated()
        {
            if (BackRefCustomer != PotentialCustomer)
            {
                BackRefCustomer = PotentialCustomer;
            }
        }
        private DateTime _OrderedDate;
        public DateTime OrderedDate
        {
            get { return _OrderedDate; }
            set { SetPropertyValue("OrderedDate", ref _OrderedDate, value); }
        }

        private FileData _Attachment;
        public virtual FileData Attachment { get { return _Attachment; } set { SetPropertyValue<FileData>("Attachment", ref _Attachment, value); } }

        private Customer _Client;



    }

    [DomainComponent]
    public class CancelOrderParameters : BaseObject
    {
        public CancelOrderParameters(Order order)
        {
            this.CancelDate = DateTime.Now;
        }

        private OrderStatus _Status;
        public OrderStatus Status { get { return _Status; } set { SetPropertyValue<OrderStatus>("Status", ref _Status, value); } }

        private Nullable<DateTime> _CancelDate;
        public Nullable<DateTime> CancelDate
        {
            get { return _CancelDate; }
            set { SetPropertyValue<Nullable<DateTime>>("CancelDate", ref _CancelDate, value); }
        }


    }
}