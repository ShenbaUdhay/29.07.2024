using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.SampleManagement.SampleCustody
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("SampleBottleID")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SampleCustody : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleCustody(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            LastUpdatedDate = Library.GetServerTime(Session);
            LastUpdatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
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
        private string _SampleBottleID;
        // [ModelDefault("AllowEdit", "false")]
        [ImmediatePostData]
        [RuleRequiredField("SampleCustody.SampleBottleID", DefaultContexts.Save)]
        public string SampleBottleID
        {
            get { return _SampleBottleID; }
            set { SetPropertyValue<string>("SampleBottleID", ref _SampleBottleID, value); }
        }
        //private Employee _From;
        //[ModelDefault("AllowEdit","false")]
        //public Employee From
        //{
        //    get { return _From; }
        //    set { SetPropertyValue<Employee>("From", ref _From, value); }
        //}
        private Employee _FromEmployee;
        //[ModelDefault("AllowEdit", "false")]
        [ImmediatePostData]
        [Appearance("SampleCustody.FromEmployee", Enabled = false, Context = "DetailView", Criteria = "FromStorage !=null")]
        public Employee FromEmployee
        {
            get { return _FromEmployee; }
            set { SetPropertyValue<Employee>("FromEmployee", ref _FromEmployee, value); }
        }
        private Storage _FromStorage;
        //[ModelDefault("AllowEdit", "false")]
        [ImmediatePostData]
        [Appearance("SampleCustody.FromStorage", Enabled = false, Context = "DetailView", Criteria = "FromEmployee !=null")]
        public Storage FromStorage
        {
            get { return _FromStorage; }
            set { SetPropertyValue<Storage>("FromStorage", ref _FromStorage, value); }
        }
        //private Employee _To;
        //public Employee To
        //{
        //    get { return _To; }
        //    set { SetPropertyValue<Employee>("To", ref _To, value); }
        //}
        private Employee _GivenTo;
        [ImmediatePostData]
        [Appearance("SampleCustody.GivenTo", Enabled = false, Context = "DetailView", Criteria = "ToStorage !=null")]
        //[RuleRequiredField("SampleCustody.GivenTo", DefaultContexts.Save)]
        public Employee GivenTo
        {
            get { return _GivenTo; }
            set { SetPropertyValue<Employee>("GivenTo", ref _GivenTo, value); }
        }
        private Storage _ToStorage;
        [ImmediatePostData]
        [Appearance("SampleCustody.ToStorage", Enabled = false, Context = "DetailView", Criteria = "GivenTo !=null")]
        public Storage ToStorage
        {
            get { return _ToStorage; }
            set { SetPropertyValue<Storage>("ToStorage", ref _ToStorage, value); }
        }
        private DateTime _DateHandled;
        public DateTime DateHandled
        {
            get { return _DateHandled; }
            set { SetPropertyValue<DateTime>("DateHandled", ref _DateHandled, value); }
        }
        private SampleCustodyMode _Mode;
        [ModelDefault("AllowEdit", "false")]
        public SampleCustodyMode Mode
        {
            get { return _Mode; }
            set { SetPropertyValue<SampleCustodyMode>("Mode", ref _Mode, value); }
        }
        private bool _Depleted;
        public bool Depleted
        {
            get { return _Depleted; }
            set { SetPropertyValue<bool>("_Depleted", ref _Depleted, value); }
        }

        [NonPersistent]
        private string _ErrorDescription;
        [ModelDefault("AllowEdit", "false")]
        [Appearance("SampleCustody.ErrorDescription", FontColor = "Red", Context = "DetailView", Criteria = "ErrorDescription !=null")]
        public string ErrorDescription
        {
            get { return _ErrorDescription; }
            set { SetPropertyValue<string>("ErrorDescription", ref _ErrorDescription, value); }
        }
        #region SampleDisposal
        private DateTime _DisposedDate;
        [RuleRequiredField("SampleCustody.DisposedDate", DefaultContexts.Save)]
        public DateTime DisposedDate
        {
            get { return _DisposedDate; }
            set { SetPropertyValue<DateTime>("DisposedDate", ref _DisposedDate, value); }
        }
        private Employee _DisposedBy;
        [RuleRequiredField("SampleCustody.DisposedBy", DefaultContexts.Save)]
        public Employee DisposedBy
        {
            get { return _DisposedBy; }
            set { SetPropertyValue<Employee>("DisposedBy", ref _DisposedBy, value); }
        }
        #endregion
        #region SampleLocation
        private string _Where;
        [ModelDefault("AllowEdit", "false")]
        public string Where
        {
            get { return _Where; }
            set { SetPropertyValue<string>("Where", ref _Where, value); }
        }
        private DateTime _LastUpdatedDate;
        public DateTime LastUpdatedDate
        {
            get { return _LastUpdatedDate; }
            set { SetPropertyValue<DateTime>("LastUpdatedDate", ref _LastUpdatedDate, value); }
        }
        private CustomSystemUser _LastUpdatedBy;
        [Browsable(false)]
        public CustomSystemUser LastUpdatedBy
        {
            get { return _LastUpdatedBy; }
            set { SetPropertyValue<CustomSystemUser>("LastUpdatedBy", ref _LastUpdatedBy, value); }
        }
        #endregion

    }
    public enum SampleCustodyMode
    {
        None = 0,
        In = 1,
        Out = 2
    }

}