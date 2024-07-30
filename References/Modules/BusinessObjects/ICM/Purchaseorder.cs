using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    public class Purchaseorder : BaseObject
    {
        #region Constructor
        public Purchaseorder(Session session) : base(session) { }
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
            //POID += Convert.ToInt32(Session.Evaluate<Purchaseorder>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(SUBSTRING(POID, 2))"), null)) + 1;
            //var curdate = Library.GetServerTime(Session).ToString("yyMMdd");
            //if (POID != "1")
            //{
            //    var predate = POID.Substring(0, 6);
            //    if (predate == curdate)
            //    {
            //        POID = "PO" + POID;
            //    }
            //    else
            //    {
            //        POID = "PO" + curdate + "01";
            //    }
            //}
            //else
            //{
            //    POID = "PO" + curdate + "01";
            //}
            PurchaseDate = Library.GetServerTime(Session);
            EnteredDate = Library.GetServerTime(Session);
            EnteredBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
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

        #region purchasedate
        private DateTime fPurchaseDate;
        public DateTime PurchaseDate
        {
            get { return fPurchaseDate; }
            set { SetPropertyValue<DateTime>("PurchaseDate", ref fPurchaseDate, value); }
        }
        #endregion

        #region purchaseby
        private Employee fPurchaseBy;
        public Employee PurchaseBy
        {
            get { return fPurchaseBy; }
            set { SetPropertyValue<Employee>("PurchaseBy", ref fPurchaseBy, value); }
        }
        #endregion

        #region vendor
        private Requisition fVendor;
        //[NonPersistent]
        [RuleRequiredField("POVendor", DefaultContexts.Save)]
        public Requisition Vendor
        {
            get
            {

                return fVendor;
            }
            set
            {
                SetPropertyValue("Vendor", ref fVendor, value);
            }
        }
        #endregion

        #region vendorname
        private string fVendorName;
        [NonPersistent]
        public string VendorName
        {
            get { return fVendorName; }
            set { SetPropertyValue("VendorName", ref fVendorName, value); }
        }
        #endregion

        #region account
        private string fAccount;
        [NonPersistent]
        public string Account
        {
            get { return fAccount; }
            set { SetPropertyValue("Account", ref fAccount, value); }
        }
        #endregion

        #region Address
        private string fAddress;
        [NonPersistent]
        public string Address
        {
            get { return fAddress; }
            set { SetPropertyValue("Address", ref fAddress, value); }
        }
        #endregion

        #region phone
        private string fPhone;
        [NonPersistent]
        public string Phone
        {
            get { return fPhone; }
            set { SetPropertyValue("Phone", ref fPhone, value); }
        }
        #endregion

        #region Email

        private string _Email;
        [NonPersistent]
        public string Email
        {
            get { return _Email; }
            set { SetPropertyValue("Email", ref _Email, value); }
        }
        #endregion

        private string _Contact;
        [NonPersistent]
        public string Contact
        {
            get { return _Contact; }
            set { SetPropertyValue("Contact", ref _Contact, value); }
        }

        #region itemassociation

        [Association("Requisitionlink", UseAssociationNameAsIntermediateTableName = true)]
        [Appearance("Purchaseorder.Item", Context = "DetailView", Criteria = "Vendor == NULL", Enabled = false)]
        public XPCollection<Requisition> Item
        {
            get { return GetCollection<Requisition>("Item"); }
        }
        #endregion

        #region approvedate
        private DateTime? fApprovedDate;
        public DateTime? ApprovedDate
        {
            get { return fApprovedDate; }
            set { SetPropertyValue<DateTime?>("ApprovedDate", ref fApprovedDate, value); }
        }
        #endregion

        #region approveby
        private CustomSystemUser fApprovedBy;
        public CustomSystemUser ApprovedBy
        {
            get { return fApprovedBy; }
            set { SetPropertyValue("ApprovedBy", ref fApprovedBy, value); }
        }
        #endregion

        #region comment
        string fComment;
        [Size(1000)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue<string>("Comment", ref fComment, value); }
        }
        #endregion

        #region EnteredDate

        private DateTime fEnteredDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime EnteredDate
        {
            get { return fEnteredDate; }
            set { SetPropertyValue<DateTime>("EnteredDate", ref fEnteredDate, value); }
        }
        #endregion

        #region EnteredBy
        private CustomSystemUser fEnteredBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser EnteredBy
        {
            get { return fEnteredBy; }
            set { SetPropertyValue("EnteredBy", ref fEnteredBy, value); }
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

        private Shippingoptions _Shipping;
        [RuleRequiredField("POShipping", DefaultContexts.Save)]
        public Shippingoptions Shipping
        {
            get { return _Shipping; }
            set { SetPropertyValue("Shipping", ref _Shipping, value); }
        }

        private ShipVia _ShipVia;
        [RuleRequiredField("POShipVia", DefaultContexts.Save)]
        public ShipVia ShipVia
        {
            get { return _ShipVia; }
            set { SetPropertyValue("ShipVia", ref _ShipVia, value); }
        }

        private string _ShippingCharge;

        public string ShippingCharge
        {
            get { return _ShippingCharge; }
            set { SetPropertyValue("ShippingCharge", ref _ShippingCharge, value); }
        }


        private string _ReferenceNumber;

        public string ReferenceNumber
        {
            get { return _ReferenceNumber; }
            set { SetPropertyValue("ReferenceNumber", ref _ReferenceNumber, value); }
        }


        private string _TrackingNumber;

        public string TrackingNumber
        {
            get { return _TrackingNumber; }
            set { SetPropertyValue("TrackingNumber", ref _TrackingNumber, value); }
        }

        private DateTime _ExpectDate;
        public DateTime ExpectDate
        {
            get
            {
                return _ExpectDate;
            }
            set
            {
                SetPropertyValue("ExpectDate", ref _ExpectDate, value);
            }
        }

        private int _NumItems;

        public int NumItems
        {
            get { return Item.Count; }
            set { SetPropertyValue("NumItems", ref _NumItems, value); }

        }

        private string _TotalPrice;
        public string TotalPrice
        {
            get { return _TotalPrice; }
            set { SetPropertyValue("TotalPrice", ref _TotalPrice, value); }
        }


        private double _NonPersisitantTotalPrice;
        [NonPersistent]
        public double NonPersisitantTotalPrice
        {
            get { return _NonPersisitantTotalPrice; }
            set { SetPropertyValue("NonPersisitantTotalPrice", ref _NonPersisitantTotalPrice, value); }
        }


        private string _Tax;
        public string Tax
        {
            get { return _Tax; }
            set { SetPropertyValue("Tax", ref _Tax, value); }
        }

        private double _TotalPriceWithTax;
        public double TotalPriceWithTax
        {
            get { return _TotalPriceWithTax; }
            set { SetPropertyValue("TotalPriceWithTax", ref _TotalPriceWithTax, value); }
        }


    }
}