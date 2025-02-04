﻿using ALPACpre.Module.BusinessObjects.ICM;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    public class Distribution : BaseObject
    {
        #region StatusEnum
        public enum LTStatus
        {
            PendingDistribute, PendingConsume, Consumed, PendingDispose, Disposed, Deleted, PartialConsumed, Emptygrid
        }
        #endregion

        #region Constructor
        public Distribution(Session session) : base(session) { }
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
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }

        #endregion

        #region status
        private LTStatus fStatus;
        public LTStatus Status
        {
            get { return fStatus; }
            set { SetPropertyValue<LTStatus>("Status", ref fStatus, value); }
        }

        private string _Category;
        public string Category
        {
            get { return _Category; }

            set { SetPropertyValue<string>(nameof(Category), ref _Category, value); }
        }
        private bool _Select;
        public bool Select
        {
            get { return _Select; }
            set { SetPropertyValue<bool>(nameof(Select), ref _Select, value); }
        }
        //private ItemDepletion _AmountTaken;
        //public ItemDepletion AmountTaken
        //{
        //    get { return _AmountTaken; }
        //    set { SetPropertyValue<ItemDepletion>(nameof(AmountTaken), ref _AmountTaken, value); }
        //}
        public int AmountTaken
        {
            get
            {
                if (itemDepletionsCollection.Count > 0)
                {
                    return itemDepletionsCollection.Sum(i => i.AmountTaken);
                }
                else
                {
                    return 0;
                }
            }
        }
        public int AmountLeft
        {
            get
            {
                if (itemDepletionsCollection.Count > 0)
                {
                    return itemDepletionsCollection.Min(i => i.AmountRemain);
                }
                else if (Item != null && !string.IsNullOrEmpty(Item.Amount))
                {
                    //return Convert.ToDecimal(Item.Amount);
                    double amountAsDouble;
                    if (double.TryParse(Item.Amount, out amountAsDouble))
                    {
                        // If successful, return the integer part of the double
                        return (int)amountAsDouble;
                    }
                    else
                    {
                        // If conversion fails, handle the error (e.g., log it, return a default value, etc.)
                        // Here, I'm returning -1 as an example of a default value when conversion fails.
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        private DateTime _DateOpened;
        public DateTime DateOpened
        {
            get { return _DateOpened; }
            set { SetPropertyValue<DateTime>(nameof(DateOpened), ref _DateOpened, value); }
        }
        #endregion

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

        #region itemreceiving
        private int fitemreceiving;
        public int itemreceiving
        {
            get { return fitemreceiving; }
            set { SetPropertyValue<int>("itemreceiving", ref fitemreceiving, value); }
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
        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue<string>(nameof(Description), ref _Description, value); }
        }
        private string _Size;
        public string Size
        {
            get { return _Size; }
            set { SetPropertyValue<string>(nameof(Size), ref _Size, value); }
        }
        private string _ItemCode;
        public string ItemCode
        {
            get { return _ItemCode; }
            set { SetPropertyValue<string>(nameof(ItemCode), ref _ItemCode, value); }
        }
        private Items _StockAmount1;
        public Items StockAmount1
        {
            get { return _StockAmount1; }
            set { SetPropertyValue<Items>(nameof(StockAmount1), ref _StockAmount1, value); }
        }


        private string _OriginalAmount;
        public string OriginalAmount
        {
            get { return _OriginalAmount; }
            set { SetPropertyValue<string>(nameof(OriginalAmount), ref _OriginalAmount, value); }
        }

        private string _AmountUnits;
        public string AmountUnits
        {
            get { return _AmountUnits; }
            set { SetPropertyValue<string>(nameof(AmountUnits), ref _AmountUnits, value); }
        }

        private bool _IsDeplete;
        public bool IsDeplete
        {
            get { return _IsDeplete; }
            set { SetPropertyValue<bool>(nameof(IsDeplete), ref _IsDeplete, value); }
        }
        private DateTime _DateDepleted;
        public DateTime DateDepleted
        {
            get { return _DateDepleted; }
            set { SetPropertyValue<DateTime>(nameof(DateDepleted), ref _DateDepleted, value); }
        }

        private Employee _DepletedBy;
        public Employee DepletedBy
        {
            get { return _DepletedBy; }
            set { SetPropertyValue(nameof(DepletedBy), ref _DepletedBy, value); }
        }

        private string _RollbackReason;
        [Size(SizeAttribute.Unlimited)]
        public string RollbackReason
        {
            get { return _RollbackReason; }
            set { SetPropertyValue("RollbackReason", ref _RollbackReason, value); }
        }


        #region itemremaining
        private int fitemremaining;
        public int itemremaining
        {
            get { return fitemremaining; }
            set { SetPropertyValue<int>("itemremaining", ref fitemremaining, value); }
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

        #region DistributedBy
        private Employee fDistributedBy;
        public Employee DistributedBy
        {
            get { return fDistributedBy; }
            set { SetPropertyValue("DistributedBy", ref fDistributedBy, value); }
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

        #region comment
        private string fComment;
        [Size(1000)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue<string>("Comment", ref fComment, value); }
        }
        #endregion       

        #region rqid
        private Requisition fRQID;
        public Requisition RQID
        {
            get { return fRQID; }
            set { SetPropertyValue("RQID", ref fRQID, value); }
        }
        #endregion

        #region poid
        private Purchaseorder fPOID;
        public Purchaseorder POID
        {
            get { return fPOID; }
            set { SetPropertyValue<Purchaseorder>("POID", ref fPOID, value); }
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

        #region ReceivedBy
        private Hr.Employee fReceivedBy;
        public Hr.Employee ReceivedBy
        {
            get { return fReceivedBy; }
            set { SetPropertyValue("ReceivedBy", ref fReceivedBy, value); }
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

        #region esid
        private string fESID;
        public string ESID
        {
            get { return fESID; }
            set { SetPropertyValue<string>("ESID", ref fESID, value); }
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

        #region NumItemCode
        private int fNumItemCode;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDashboards(false)]
        public int NumItemCode
        {
            get { return fNumItemCode; }
            set { SetPropertyValue("NumItemCode", ref fNumItemCode, value); }
        }
        #endregion


        #region NumItems
        private int fNumItems;
        [NonPersistent]
        public int NumItems
        {
            get
            {
                fNumItems = Convert.ToInt32(Session.Evaluate(typeof(Distribution), DevExpress.Data.Filtering.CriteriaOperator.Parse("Count()"), DevExpress.Data.Filtering.CriteriaOperator.Parse("Status = 'PendingDistribute' And ReceiveID = ?", ReceiveID)));
                return fNumItems;
            }
        }
        #endregion

        //[NonPersistent]
        //public int PONumberItems
        //{
        //    get
        //    {
        //        if (POID != null)
        //        {
        //            return POID.Item.Count;
        //        }
        //        return 0;
        //    }
        //}

        [NonPersistent]
        public int itemreceivedsort
        {
            get
            {
                if (itemreceived != null)
                {
                    return Convert.ToInt32(itemreceived.Substring(0, itemreceived.IndexOf("of") - 1));
                }
                return 0;
            }
        }

        [NonPersistent]
        public string Receiveviewgroup
        {
            get
            {
                // if (POID != null && RQID != null && Vendor != null && ReceiveDate != null)
                if (Vendor != null && ReceiveDate != null)
                {
                    //if (POID.ExpectDate == DateTime.MinValue)
                    //{
                    //    return string.Format("RC#:{0}  PO#:{1}  Vendor:{2}  NumItemCode:{3}  ExpectDate:{4}  TrackingNumber:{5}  ReceivedDate:{6}", ReceiveID, POID, Vendor.Vendor, NumItemCode, null, POID.TrackingNumber, ReceiveDate.ToShortDateString());
                    //}

                    return string.Format("RC#:{0}   Vendor:{1}  NumItemCode:{2}    ReceivedDate:{3}", ReceiveID, Vendor.Vendor, NumItemCode, ReceiveDate);

                }
                else
                {
                    return null;
                }
            }
        }
        #region VendorReagentCertificate
        private VendorReagentCertificate _VendorReagentCertificate;
        [Association("VendorReagentCertificate_Distribution")]
        public VendorReagentCertificate VendorReagentCertificate
        {
            get
            {
                return _VendorReagentCertificate;
            }
            set
            {
                SetPropertyValue<VendorReagentCertificate>("VendorReagentCertificate", ref _VendorReagentCertificate, value);
            }
        }

        #endregion

        [NonPersistent]
        //private int _DaysRemaining;
        public int DaysReamining
        {
            get
            {
                if (ExpiryDate != null)
                {
                    return (ExpiryDate.Value.Date - DateTime.Now.Date).Days;
                }
                return 0;

            }

        }
        #region RequisitionID

        private Requisition _RequisitionID;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public Requisition RequisitionID
        {
            get { return _RequisitionID; }
            set { SetPropertyValue("RequisitionID", ref _RequisitionID, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion

        #region Parent
        public string Parent
        {
            get
            {
                if (Item != null)
                {
                    return string.Format("Vendor:{0} Item(#):{1} Description:{2} StockQty:{3}:", Item.Vendor.Vendor, Item.items, Item.Specification, Item.StockQty);
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        [Association("Distribution-ItemDepletion")]
        public XPCollection<ItemDepletion> itemDepletionsCollection
        {
            get { return GetCollection<ItemDepletion>("itemDepletionsCollection"); }
        }
        #endregion

        //[Browsable(false)]
        [NonPersistent]
        [VisibleInDetailView(false),VisibleInListView(false),VisibleInDashboards(false),VisibleInLookupListView(false)]
        public string ffLT
        {
            get
            {
                if (LT != null)
                {
                    string StrLT="LT" + LT;
                    return StrLT;
                }
                return null;

            }

        }

    }
}