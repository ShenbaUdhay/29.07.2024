using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting.Sample_Custody;
using System;

namespace Modules.BusinessObjects.BarCodeSampleCustody
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SampleIn : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleIn(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region SampleBottleID
        private string _SampleBottleID;
        public string SampleBottleID
        {
            get { return _SampleBottleID; }
            set { SetPropertyValue(nameof(SampleBottleID), ref _SampleBottleID, value); }
        }
        #endregion

        #region From
        private string _From;
        [EditorAlias("SampleToPropertyEditor")]
        public string From
        {
            get { return _From; }
            set { SetPropertyValue(nameof(From), ref _From, value); }
        }
        #endregion

        #region To
        private string _To;
        [EditorAlias("SampleToPropertyEditor")]
        public string To
        {
            get { return _To; }
            set { SetPropertyValue(nameof(To), ref _To, value); }
        }
        #endregion

        #region DateTime
        private DateTime _DateHandled;
        public DateTime DateHandled
        {
            get { return _DateHandled; }
            set { SetPropertyValue(nameof(DateHandled), ref _DateHandled, value); }
        }
        #endregion

        #region Mode
        private string _Mode;
        public string Mode
        {
            get { return _Mode; }
            set { SetPropertyValue(nameof(Mode), ref _Mode, value); }
        }
        #endregion

        #region Hazardous
        private bool _Hazardous;
        public bool Hazardous
        {
            get { return _Hazardous; }
            set { SetPropertyValue(nameof(Hazardous), ref _Hazardous, value); }
        }
        #endregion

        #region HandledBy
        private Employee _HandledBy;
        public Employee HandledBy
        {
            get { return _HandledBy; }
            set { SetPropertyValue("HandledBy", ref _HandledBy, value); }

        }
        #endregion

        #region Storage
        private string _Storage;
        public string Storage
        {
            get { return _Storage; }
            set { SetPropertyValue(nameof(Storage), ref _Storage, value); }
        }
        #endregion

        #region User
        private string _User;
        public string User
        {
            get { return _User; }
            set { SetPropertyValue(nameof(User), ref _User, value); }
        }
        #endregion

        #region DateDisposed
        private DateTime _DateDisposed;
        public DateTime DateDisposed
        {
            get { return _DateDisposed; }
            set { SetPropertyValue(nameof(DateDisposed), ref _DateDisposed, value); }
        }
        #endregion

        #region DisposedBy
        private Employee _DisposedBy;
        public Employee DisposedBy
        {
            get { return _DisposedBy; }
            set { SetPropertyValue(nameof(DisposedBy), ref _DisposedBy, value); }
        }
        #endregion

        #region Deplete
        private bool _Deplete;
        [ImmediatePostData]
        public bool Deplete
        {
            get { return _Deplete; }
            set { SetPropertyValue(nameof(Deplete), ref _Deplete, value); }
        }
        #endregion

        #region WasteDrumID
        private WasteDrums _WasteDrumID;
        public WasteDrums WasteDrumID
        {
            get { return _WasteDrumID; }
            set { SetPropertyValue(nameof(WasteDrumID), ref _WasteDrumID, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("MD11", Enabled = false, Context = "DetailView")]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }

        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("MB11", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }

        }
        #endregion

        #region JobID
        private Modules.BusinessObjects.SampleManagement.Samplecheckin _JobID;
        public Modules.BusinessObjects.SampleManagement.Samplecheckin JobID
        {
            get { return _JobID; }
            set { SetPropertyValue(nameof(JobID), ref _JobID, value); }
        }
        #endregion

        #region Client
        private Customer _Client;
        public Customer Client
        {
            get { return _Client; }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        }
        #endregion

        #region SampleCustodyTest
        private SampleCustodyTest _SampleCustodyTest;
        [Association("SampleCustodyTest-SampleIn")]

        public SampleCustodyTest SampleCustodyTest
        {
            get { return _SampleCustodyTest; }
            set { SetPropertyValue("SampleCustodyTest", ref _SampleCustodyTest, value); }
        }
        #endregion

    }
}