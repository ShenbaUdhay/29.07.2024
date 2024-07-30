using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.ICM
{
    public class ReceiveDistributionHistory : BaseObject
    {
        public ReceiveDistributionHistory(Session session)
            : base(session)
        {
        }

        #region vendor
        private Vendors fVendor;
        //[RuleRequiredField("fVendor", DefaultContexts.Save)]
        public Vendors Vendor
        {
            get { return fVendor; }
            set { SetPropertyValue("Vendor", ref fVendor, value); }
        }
        #endregion

        #region Item
        private Items fItem;
        public Items Item
        {
            get { return fItem; }
            set { SetPropertyValue("Item", ref fItem, value); }
        }
        #endregion       

        #region Manufacturer
        private Manufacturer fManufacturer;
        public Manufacturer Manufacturer
        {
            get { return fManufacturer; }
            set { SetPropertyValue("Manufacturer", ref fManufacturer, value); }
        }
        #endregion

        #region orderqty
        //Items/unit
        private int fOrderQty;
        public int OrderQty
        {
            get { return fOrderQty; }
            set { SetPropertyValue<int>("OrderQty", ref fOrderQty, value); }
        }
        #endregion

        #region itemreceived
        private string fitemreceived;
        public string itemreceived
        {
            get { return fitemreceived; }
            set { SetPropertyValue<string>("itemreceived", ref fitemreceived, value); }
        }
        #endregion

        #region itemremaining
        private int fitemremaining;
        public int itemremaining
        {
            get { return fitemremaining; }
            set { SetPropertyValue<int>("itemremaining", ref fitemremaining, value); }
        }
        #endregion     

        #region requestedDate
        private DateTime fRequestedDate;
        public DateTime RequestedDate
        {
            get { return fRequestedDate; }
            set { SetPropertyValue<DateTime>("RequestedDate", ref fRequestedDate, value); }
        }
        #endregion

        #region requestedby
        private Employee fRequestedBy;
        public Employee RequestedBy
        {
            get { return fRequestedBy; }
            set { SetPropertyValue("RequestedBy", ref fRequestedBy, value); }
        }
        #endregion     

        #region rqid
        private string fRQID;
        public string RQID
        {
            get { return fRQID; }
            set { SetPropertyValue<string>("RQID", ref fRQID, value); }
        }
        #endregion

        #region poid
        private string fPOID;
        public string POID
        {
            get { return fPOID; }
            set { SetPropertyValue<string>("POID", ref fPOID, value); }
        }
        #endregion

        #region receiveid
        private string fReceiveID;
        public string ReceiveID
        {
            get { return fReceiveID; }
            set { SetPropertyValue<string>("ReceiveID", ref fReceiveID, value); }
        }
        #endregion

        #region receivecount
        private string fReceiveCount;
        public string ReceiveCount
        {
            get { return fReceiveCount; }
            set { SetPropertyValue<string>("ReceiveCount", ref fReceiveCount, value); }
        }
        #endregion

        #region receivedate
        private DateTime? fReceiveDate;
        public DateTime? ReceiveDate
        {
            get { return fReceiveDate; }
            set { SetPropertyValue<DateTime?>("ReceiveDate", ref fReceiveDate, value); }
        }
        #endregion

        #region totalitems
        private int fTotalItems;
        public int TotalItems
        {
            get { return fTotalItems; }
            set { SetPropertyValue<int>("TotalItems", ref fTotalItems, value); }
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

        #region EnteredDate
        private DateTime? fEnteredDate;
        public DateTime? EnteredDate
        {
            get { return fEnteredDate; }
            set { SetPropertyValue<DateTime?>("EnteredDate", ref fEnteredDate, value); }
        }
        #endregion

        #region EnteredBy
        private Employee fEnteredBy;
        public Employee EnteredBy
        {
            get { return fEnteredBy; }
            set { SetPropertyValue("EnteredBy", ref fEnteredBy, value); }
        }
        #endregion

        #region Distributiondate
        private DateTime? fDistributionDate;
        public DateTime? DistributionDate
        {
            get { return fDistributionDate; }
            set { SetPropertyValue<DateTime?>("DistributionDate", ref fDistributionDate, value); }
        }
        #endregion

        #region givenby
        private Employee fGivenBy;
        public Employee GivenBy
        {
            get { return fGivenBy; }
            set { SetPropertyValue("GivenBy", ref fGivenBy, value); }
        }
        #endregion

        #region givento
        private Hr.Employee fgivento;
        public Hr.Employee givento
        {
            get { return fgivento; }
            set { SetPropertyValue("givento", ref fgivento, value); }
        }
        #endregion

        #region storage
        private ICMStorage fStorage;
        public ICMStorage Storage
        {
            get { return fStorage; }
            set { SetPropertyValue("Storage", ref fStorage, value); }
        }
        #endregion

        #region vendorlt
        private string fVendorLT;
        public string VendorLT
        {
            get { return fVendorLT; }
            set { SetPropertyValue<string>("VendorLT", ref fVendorLT, value); }
        }
        #endregion

        #region expirydate
        private DateTime? fExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return fExpiryDate; }
            set { SetPropertyValue<DateTime?>("ExpiryDate", ref fExpiryDate, value); }
        }
        #endregion

        #region lt
        private string fLT;
        //[Indexed(Unique = true)]
        public string LT
        {
            get { return fLT; }
            set { SetPropertyValue<string>("LT", ref fLT, value); }
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

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }
    }
}