using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using System;
using System.ComponentModel.DataAnnotations;

namespace Modules.BusinessObjects.Accounts
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CalLog : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CalLog(Session session)
            : base(session)
        {
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


        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue("Date", ref _Date, value); }
        }

        private string _Title;
        [MaxLength(255)]
        public string Title { get { return _Title; } set { SetPropertyValue<string>("Title", ref _Title, value); } }

        private string _Description;

        [FieldSize(4000)]
        [MaxLength(4000)]
        public string Description { get { return _Description; } set { SetPropertyValue<string>("Description", ref _Description, value); } }

        private User _EnteredBy;
        public User EnteredBy
        {
            get { return _EnteredBy; }
            set { SetPropertyValue("EnteredBy", ref _EnteredBy, value); }
        }

        private Customer _Client;

        public Customer Client
        {
            get { return _Client; }
            set { SetPropertyValue("Client", ref _Client, value); }
        }

        private CRMProspects _Company;

        public CRMProspects Company
        {
            get { return _Company; }
            set { SetPropertyValue("Company", ref _Company, value); }
        }

        //private CRMLead _PotentialCustomer;

        //public CRMLead PotentialCustomer
        //{
        //    get { return _PotentialCustomer; }
        //    set { SetPropertyValue("Company", ref _PotentialCustomer, value); }
        //}


        private Customer _Clients;

        [DevExpress.Xpo.Association("Customer-CallLog")]

        public Customer Clients
        {
            get { return _Clients; }
            set { SetPropertyValue("Clients", ref _Clients, value); }
        }

        //private CRMLead _CRMLead;
        //[DevExpress.Xpo.Association("CRMLead-CallLog")]
        //public CRMLead CRMLead
        //{
        //    get { return _CRMLead; }
        //    set { SetPropertyValue("CRMLead", ref _CRMLead, value); }
        //}

        private CRMProspects _CRMProspects;

        [DevExpress.Xpo.Association("CRMProspects-CallLog")]

        public CRMProspects CRMProspects
        {
            get { return _CRMProspects; }
            set { SetPropertyValue("CRMProspects", ref _CRMProspects, value); }
        }

    }
}