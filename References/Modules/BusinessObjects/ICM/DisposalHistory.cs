using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.ICM
{
    public class DisposalHistory : BaseObject
    {
        #region Constructor
        public DisposalHistory(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #endregion

        #region Distribution
        private Distribution fDistribution;
        //[RuleRequiredField("fVendor", DefaultContexts.Save)]
        public Distribution Distribution
        {
            get { return fDistribution; }
            set { SetPropertyValue("Distribution", ref fDistribution, value); }
        }
        #endregion

        #region DisposedDate
        private DateTime? fDisposedDate;
        public DateTime? DisposedDate
        {
            get { return fDisposedDate; }
            set { SetPropertyValue<DateTime?>("DisposedDate", ref fDisposedDate, value); }
        }
        #endregion

        #region DisposedBy
        private Hr.Employee fDisposedBy;
        public Hr.Employee DisposedBy
        {
            get { return fDisposedBy; }
            set { SetPropertyValue("DisposedBy", ref fDisposedBy, value); }
        }
        #endregion

        #region ReturnBy
        private Hr.Employee fReturnBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public Hr.Employee ReturnBy
        {
            get
            {
                return fReturnBy;
            }
            set
            {
                SetPropertyValue("ReturnBy", ref fReturnBy, value);
            }
        }
        #endregion

        #region ReturnDate
        private DateTime fReturnDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
        public DateTime ReturnDate
        {
            get
            {
                return fReturnDate;
            }
            set
            {
                SetPropertyValue("ReturnDate", ref fReturnDate, value);
            }
        }
        #endregion

        #region Returnreason
        private string _ReturnReason;
        public string ReturnReason
        {
            get { return _ReturnReason; }
            set { SetPropertyValue("ReturnReason", ref _ReturnReason, value); }
        }
        #endregion

        #region ModifiedBy
        private Hr.Employee fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public Hr.Employee ModifiedBy
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
    }
}