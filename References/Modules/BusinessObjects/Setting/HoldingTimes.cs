using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty("HoldingTime")]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class HoldingTimes : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public HoldingTimes(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }

        private const string V = "Holdingtime is already exists";

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
        #region HoldingTime
        private String _HoldingTime;
        [RuleUniqueValue("Holdingtimeisunique", DefaultContexts.Save, "Holding time is already exists")]
        [RuleRequiredField("Holdingtimeisuniquesss", DefaultContexts.Save, "Holding time must not be empty")]
        public String HoldingTime
        {
            get { return _HoldingTime; }
            set { SetPropertyValue("HoldingTime", ref _HoldingTime, value.Trim()); }

        }

        #endregion

        #region HourValueConversion
        private uint _HourValueConversion;
        public uint HourValueConversion
        {
            get { return _HourValueConversion; }
            set { SetPropertyValue("HourValueConversion", ref _HourValueConversion, value); }
        }
        #endregion

        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
    }
}