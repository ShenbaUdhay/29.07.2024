using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using System;

namespace Modules.BusinessObjects.Crm
{
    [DefaultClassOptions]
    [ImageName("BO_Scheduler")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Activity : Event
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        TimeZoneinfo objTimeZone = new TimeZoneinfo();
        bool boolStart = true;
        bool boolEnd = true;
        DateTime dtUpdatedDate;
        public Activity(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            Owner = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (StartDateOn != null & StartDateOn != DateTime.MinValue)
            {
                StartDateOn = TimeZoneInfo.ConvertTimeToUtc(StartDateOn, objTimeZone.TimeZone);
                StartOn = StartDateOn;
            }
            if (EndDateOn != null & EndDateOn != DateTime.MinValue)
            {
                EndDateOn = TimeZoneInfo.ConvertTimeToUtc(EndDateOn, objTimeZone.TimeZone);
                EndOn = EndDateOn;
            }

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }

        protected override void OnSaved()
        {
            if (StartDateOn != null & StartDateOn != DateTime.MinValue)
            {
                StartDateOn = TimeZoneInfo.ConvertTimeFromUtc(StartDateOn, TimeZoneInfo.Local);
                StartOn = StartDateOn;
            }
            if (EndDateOn != null & EndDateOn != DateTime.MinValue)
            {
                EndDateOn = TimeZoneInfo.ConvertTimeFromUtc(EndDateOn, TimeZoneInfo.Local);

                EndOn = EndDateOn;
            }
        }



        private Employee _Owner;
        public Employee Owner { get { return _Owner; } set { SetPropertyValue<Employee>("Owner", ref _Owner, value); } }

        [VisibleInDetailView(false), VisibleInListView(false)]
        [DevExpress.Xpo.Association("Activity-Notes")]
        public XPCollection<Notes> Notes
        {
            get { return GetCollection<Notes>("Notes"); }
        }

        private Notes _NoteID;

        public Notes NoteID
        {
            get { return _NoteID; }
            set { SetPropertyValue("NoteID", ref _NoteID, value); }
        }

        //private CRMLead _LeadID;

        //public CRMLead LeadID
        //{
        //    get { return _LeadID; }
        //    set { SetPropertyValue("LeadID", ref _LeadID, value); }
        //}

        //private CRMOpportunity _OpportunityID;

        //public CRMOpportunity OpportunityID
        //{
        //    get { return _OpportunityID; }
        //    set { SetPropertyValue("OpportunityID", ref _OpportunityID, value); }
        //}

        //private CRMAccount _AccountID;
        //public CRMAccount AccountID
        //{
        //    get { return _AccountID; }
        //    set { SetPropertyValue("AccountID", ref _AccountID, value); }
        //}

        [Association("Activity-Employee")]
        public XPCollection<Employee> Employee
        {
            get { return GetCollection<Employee>("Employee"); }
        }

        private DateTime _StartDateOn;
        // [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime StartDateOn
        {
            get
            {

                return _StartDateOn;
            }
            set
            {

                SetPropertyValue("StartDateOn", ref _StartDateOn, value);

            }
        }

        private DateTime _EndDateOn;
        // [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime EndDateOn
        {
            get
            {

                return _EndDateOn;
            }
            set
            {

                SetPropertyValue("EndDateOn", ref _EndDateOn, value);

            }
        }

        //public DateTime EndOn { get; private set; }

        //private DateTime _Date;
        //[NonPersistent]
        //// [ValueConverter(typeof(UtcDateTimeConverter))]
        //public DateTime Date
        //{
        //    get
        //    {
        //        if (StartDateOn != DateTime.MinValue && objTimeZone != null && objTimeZone.TimeZone != null)
        //        {
        //            return TimeZoneInfo.ConvertTimeFromUtc(StartDateOn, objTimeZone.TimeZone);
        //        }
        //        else
        //        {
        //            return StartOn;
        //        }
        //    }

        private DateTime _Date;
        [NonPersistent]
        // [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime Date
        {
            get
            {
                if (StartDateOn != DateTime.MinValue && objTimeZone != null && objTimeZone.TimeZone != null)
                {
                    return TimeZoneInfo.ConvertTimeFromUtc(StartDateOn, objTimeZone.TimeZone);
                }
                else
                {
                    return StartOn;
                }
            }

        }

        //[DevExpress.ExpressApp.DC.Aggregated]
        //public virtual IList<Note> Notes { get; set; }
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


        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        //  [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }


        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }


        private DateTime _ModifiedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        // [ValueConverter(typeof(UtcDateTimeConverter))]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }


        private Employee _ModifiedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }
        }

    }
}