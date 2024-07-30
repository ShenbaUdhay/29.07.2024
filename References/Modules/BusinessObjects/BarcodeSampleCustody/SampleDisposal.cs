using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
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
    public class SampleDisposal : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SampleDisposal(Session session)
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
        private Boolean _Deplete;
        public Boolean Deplete
        {
            get { return _Deplete; }
            set { SetPropertyValue(nameof(Deplete), ref _Deplete, value); }
        }
        #endregion

        #region Hazardous
        private Boolean _Hazardous;
        public Boolean Hazardous
        {
            get { return _Hazardous; }
            set { SetPropertyValue(nameof(Hazardous), ref _Hazardous, value); }
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

        #region Comment
        private string _Comment;
        [Size(1000)]
        public String Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }

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

        #region SampleCustodyTest
        private SampleCustodyTest _SampleCustodyTest;
        [Association("SampleCustodyTest-SampleDisposal")]

        public SampleCustodyTest SampleCustodyTest
        {
            get { return _SampleCustodyTest; }
            set { SetPropertyValue("SampleCustodyTest", ref _SampleCustodyTest, value); }
        }
        #endregion
    }
}