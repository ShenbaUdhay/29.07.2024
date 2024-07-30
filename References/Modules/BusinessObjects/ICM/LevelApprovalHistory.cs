using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class LevelApprovalHistory : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        #region Constructor
        public LevelApprovalHistory(Session session)
            : base(session)
        {
        }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #endregion

        #region BatchID
        private string _BatchID;

        public string BatchID
        {
            get { return _BatchID; }
            set { SetPropertyValue("BatchID", ref _BatchID, value); }
        }
        #endregion

        #region ApprovedDate
        private DateTime? _ApprovedDate;
        public DateTime? ApprovedDate
        {
            get { return _ApprovedDate; }
            set { SetPropertyValue("ApprovedDate", ref _ApprovedDate, value); }
        }
        #endregion

        #region ApprovedBy
        private CustomSystemUser _ApprovedBy;
        public CustomSystemUser ApprovedBy
        {
            get { return _ApprovedBy; }
            set { SetPropertyValue("ApprovedBy", ref _ApprovedBy, value); }
        }
        #endregion

        #region Level
        private int _Level;
        public int Level
        {
            get { return _Level; }
            set { SetPropertyValue("Level", ref _Level, value); }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get
            {
                return fModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref fModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime fModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
        public DateTime ModifiedDate
        {
            get
            {
                return fModifiedDate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref fModifiedDate, value);
            }
        }
        #endregion

        #region CanceledBy
        private CustomSystemUser fCanceledBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public CustomSystemUser CanceledBy
        {
            get
            {
                return fCanceledBy;
            }
            set
            {
                SetPropertyValue("CanceledBy", ref fCanceledBy, value);
            }
        }
        #endregion

        #region CanceledDate
        private DateTime fCanceledDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
        public DateTime CanceledDate
        {
            get
            {
                return fCanceledDate;
            }
            set
            {
                SetPropertyValue("CanceledDate", ref fCanceledDate, value);
            }
        }
        #endregion
    }
}