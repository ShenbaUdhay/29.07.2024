using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.ICM
{
    public class ConsumptionHistory : BaseObject
    {
        #region Consturctor
        public ConsumptionHistory(Session session) : base(session) { }
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
        public Distribution Distribution
        {
            get { return fDistribution; }
            set { SetPropertyValue("Distribution", ref fDistribution, value); }
        }
        #endregion

        #region ConsumptionDate
        private DateTime? fConsumptionDate;
        public DateTime? ConsumptionDate
        {
            get { return fConsumptionDate; }
            set { SetPropertyValue<DateTime?>("ConsumptionDate", ref fConsumptionDate, value); }
        }
        #endregion

        #region ConsumptionBy
        private Hr.Employee fConsumptionBy;
        public Hr.Employee ConsumptionBy
        {
            get { return fConsumptionBy; }
            set { SetPropertyValue("ConsumptionBy", ref fConsumptionBy, value); }
        }
        #endregion

        #region EnteredDate
        private DateTime? fEnteredDate;
        public DateTime? EnteredDate
        {
            get { return fEnteredDate; }
            set { SetPropertyValue<DateTime?>("EnteredDate", ref fEnteredDate, value); }
        }
        #endregion

        #region EnteredBy
        private Hr.Employee fEnteredBy;
        public Hr.Employee EnteredBy
        {
            get { return fEnteredBy; }
            set { SetPropertyValue("EnteredBy", ref fEnteredBy, value); }
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

        #region ReturnBy
        private CustomSystemUser fReturnBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ReturnBy
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

        #region Consumed
        private Boolean fConsumed;
        public Boolean Consumed
        {
            get { return fConsumed; }
            set { SetPropertyValue<Boolean>("Consumed", ref fConsumed, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public Employee ModifiedBy
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