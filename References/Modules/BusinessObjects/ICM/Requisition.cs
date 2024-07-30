using ALPACpre.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.Libraries;
using System;
using static Modules.BusinessObjects.ICM.Distribution;

namespace ICM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [Appearance("StatusColumnColor", AppearanceItemType = "ViewItem", TargetItems = "Status,NonPersistentStatus", Context = "Requisition_ListView", FontColor = "Blue")]
    public class Requisition : BaseObject
    {
        public const string ProgressPropertyAlias = "ProgressStatus";
        #region NonPersistentStatusEnum
        public enum NonPersistentRequitionStatus
        {
            [XafDisplayName("Pending Review")]
            PendingReview,
            [XafDisplayName("Pending Purchasing")]
            PendingPurchasing,
            [XafDisplayName("Pending Receipt")]
            PendingReceipt,
            [XafDisplayName("Pending Distribution")]
            PendingDistribution,
            Distributed,
            Null
        }
        #endregion
        #region StatusEnum
        public enum TaskStatus { PendingReview, PendingApproval, PendingOrdering, PendingOrderingApproval, PendingReceived, PartiallyReceived, Received, Cancelled, Level1Pending, Level2Pending, Level3Pending, Level4Pending, Level5Pending, Level6Pending, Level7Pending, Level8Pending, Level9Pending, Level10Pending }
        #endregion

        #region Constructor
        public Requisition(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        protected override void OnSaving()
        {
            base.OnSaving();
            if (!Session.IsNewObject(this))
            {
                ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                ModifiedDate = Library.GetServerTime(Session);
            }

            if (OrderQty > -1)
            {
                double tempprice = OrderQty * UnitPrice;
                ExpPrice = Math.Round(tempprice * 100) / 100;
            }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            OrderQty = 1;
            //RQID += Convert.ToInt32(Session.Evaluate<Requisition>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Max(SUBSTRING(RQID, 2))"), null)) + 1;
            //var curdate = Library.GetServerTime(Session).ToString("yyMMdd");
            //if (RQID != "1")
            //{
            //    var predate = RQID.Substring(0, 6);
            //    if (predate == curdate)
            //    {
            //        RQID = "RI" + RQID;
            //    }
            //    else
            //    {
            //        RQID = "RI" + curdate + "01";
            //    }
            //}
            //else
            //{
            //    RQID = "RI" + curdate + "01";
            //}
            RequestedDate = Library.GetServerTime(Session);
            RequestedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            EnteredDate = Library.GetServerTime(Session);
            EnteredBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            Status = TaskStatus.PendingReview;
        }
        #endregion

        #region item
        private Items fItem;
        [ImmediatePostData]
        [Appearance("Item", Enabled = false, Criteria = "Status <> 'PendingReview'", Context = "DetailView")]
        [RuleRequiredField("Item", DefaultContexts.Save)]
        public Items Item
        {
            get { return fItem; }
            set { SetPropertyValue("Item", ref fItem, value); }
        }
        #endregion

        #region vendor
        private Vendors fVendor;
        [Appearance("Vendor", Enabled = false, Criteria = "Status <> 'PendingReview'", Context = "DetailView")]
        //[RuleRequiredField("fVendor", DefaultContexts.Save)]
        public Vendors Vendor
        {
            get { return fVendor; }
            set { SetPropertyValue("Vendor", ref fVendor, value); }
        }
        #endregion

        #region Manufacturer
        private Manufacturer fManufacturer;
        //[RuleRequiredField("fBrand", DefaultContexts.Save)]
        public Manufacturer Manufacturer
        {
            get { return fManufacturer; }
            set { SetPropertyValue("Manufacturer", ref fManufacturer, value); }
        }
        #endregion

        #region catalog
        private string fCatalog;
        [Appearance("Catalog", Enabled = false, Criteria = "Status <> 'PendingReview'", Context = "DetailView")]
        public string Catalog
        {
            get { return fCatalog; }
            set { SetPropertyValue<string>("Catalog", ref fCatalog, value); }
        }
        #endregion

        #region orderqty
        private int fOrderQty;
        //[ImmediatePostData]
        [Appearance("OrderQty", Enabled = false, Criteria = "Status <> 'PendingReview'", Context = "DetailView")]
        [RuleRequiredField("fOrderQty", DefaultContexts.Save)]
        public int OrderQty
        {
            get { return fOrderQty; }
            set { SetPropertyValue<int>("OrderQty", ref fOrderQty, value); }
        }
        #endregion

        #region totalitems
        private int fTotalItems;
        //[RuleRequiredField("fOrderQty", DefaultContexts.Save)]
        public int TotalItems
        {
            get { return fTotalItems; }
            set { SetPropertyValue<int>("TotalItems", ref fTotalItems, value); }
        }
        #endregion

        #region unitprice
        private double fUnitPrice;
        [Appearance("UnitPrice", Enabled = false, Criteria = "Status <> 'PendingReview'", Context = "DetailView")]
        //[RuleRequiredField("fUnitPrice", DefaultContexts.Save)]
        [ImmediatePostData]
        public double UnitPrice
        {
            get { return fUnitPrice; }
            set { SetPropertyValue<double>("UnitPrice", ref fUnitPrice, value); }
        }
        #endregion

        #region expprice
        private double fExpPrice;
        [Appearance("ExpPrice", Enabled = false, Criteria = "Status <> 'PendingReview'", Context = "DetailView")]
        public double ExpPrice
        {
            get { return fExpPrice; }
            set { SetPropertyValue<double>("ExpPrice", ref fExpPrice, value); }
        }
        #endregion

        #region shippingoption
        private Shippingoptions fShippingOption;
        [Appearance("ShippingOption", Enabled = false, Criteria = "Status <> 'PendingReview'", Context = "DetailView")]
        //[RuleRequiredField("fShippingOption", DefaultContexts.Save)]
        public Shippingoptions ShippingOption
        {
            get { return fShippingOption; }
            set { SetPropertyValue("ShippingOption", ref fShippingOption, value); }
        }
        #endregion

        #region department
        private Department fdepartment;
        public Department department
        {
            get { return fdepartment; }
            set { SetPropertyValue<Department>("department", ref fdepartment, value); }
        }
        #endregion

        #region requesteddate
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
        private string fRQID;
        public string RQID
        {
            get { return fRQID; }
            set { SetPropertyValue<string>("RQID", ref fRQID, value); }
        }
        #endregion

        #region status
        private TaskStatus fStatus;
        public TaskStatus Status
        {
            get { return fStatus; }
            set { SetPropertyValue<TaskStatus>("Status", ref fStatus, value); }
        }
        #endregion

        #region reviewdate
        private DateTime? fReviewedDate;
        public DateTime? ReviewedDate
        {
            get { return fReviewedDate; }
            set { SetPropertyValue<DateTime?>("ReviewedDate", ref fReviewedDate, value); }
        }
        #endregion

        #region reviewby
        private Employee fReviewedBy;
        public Employee ReviewedBy
        {
            get { return fReviewedBy; }
            set { SetPropertyValue("ReviewedBy", ref fReviewedBy, value); }
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
        private Employee fApprovedBy;
        public Employee ApprovedBy
        {
            get { return fApprovedBy; }
            set { SetPropertyValue("ApprovedBy", ref fApprovedBy, value); }
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

        #region itemremaining
        private int fitemremaining;
        public int itemremaining
        {
            get { return fitemremaining; }
            set { SetPropertyValue<int>("itemremaining", ref fitemremaining, value); }
        }
        #endregion

        #region itemreceiving
        private int fitemreceiving;
        //[ImmediatePostData]
        [NonPersistent]
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

        #region itemcollection
        [Association("Requisitionlink", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Purchaseorder> items
        {
            get { return GetCollection<Purchaseorder>("items"); }
        }
        #endregion

        #region Cancel
        private bool _Cancel;
        public bool Cancel
        {
            get { return _Cancel; }
            set { SetPropertyValue("Cancel", ref _Cancel, value); }
        }

        #endregion

        #region CanceledBy
        private Employee fCanceledBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public Employee CanceledBy
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
        private Nullable<DateTime> fCanceledDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
        public Nullable<DateTime> CanceledDate
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

        #region rollbackreason
        private string _RollbackReason;
        public string RollbackReason
        {
            get { return _RollbackReason; }
            set { SetPropertyValue("RollbackReason", ref _RollbackReason, value); }
        }
        #endregion

        #region "BatchID"
        private string _BatchID;
        public string BatchID
        {
            get { return _BatchID; }
            set { SetPropertyValue("BatchID", ref _BatchID, value); }
        }
        #endregion

        #region "Level"
        private int _Level;
        public int Level
        {
            get { return _Level; }
            set { SetPropertyValue("Level", ref _Level, value); }
        }
        #endregion

        #region Errorlog
        [NonPersistent]
        private string fErrorlog;
        public string Errorlog
        {
            get { return fErrorlog; }
            set { SetPropertyValue<string>("Errorlog", ref fErrorlog, value); }
        }
        #endregion


        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public string grade
        {
            get
            {
                if (Item != null && Item.Grade != null && Item.Grade.Grade != null)
                {
                    return Item.Grade.Grade;
                }
                else
                {
                    return null;
                }
            }
        }

        [NonPersistent]
        public string Parent
        {
            get
            {
                var totamount = Convert.ToDouble(Session.Evaluate<Requisition>(DevExpress.Data.Filtering.CriteriaOperator.Parse("Sum(ExpPrice)"), DevExpress.Data.Filtering.CriteriaOperator.Parse("BatchID=?", BatchID)));
                return string.Format("RR#:{0}  Status:{1} TotalAmount:{2}", BatchID, Status, string.Format("{0:0.00}", totamount));

            }
            //set { SetPropertyValue("MatrixTestMethod", ref _TestGroup, value); }
        }

        //[NonPersistent]
        //      public string POIDvendor
        //      {
        //          get
        //          {
        //              if (Vendor != null && POID != null)
        //              {
        //                  return string.Format("POID:{0}  Vendor:{1}", POID.POID, Vendor.Vendor);
        //              }
        //              else {
        //                  return null;
        //              }
        //          }
        //      }

        //      [NonPersistent]

        //      public int PONumberItems
        //      {
        //          get
        //          {
        //              if (POID != null)
        //              {
        //                  return POID.Item.Count;
        //              }
        //              return 0;
        //          }
        //      }

        #region NumItems
        private int fNumItems;
        [NonPersistent]
        public int NumItems
        {
            get
            {
                if (Vendor != null)
                {
                    fNumItems = Convert.ToInt32(Session.Evaluate(typeof(Requisition), DevExpress.Data.Filtering.CriteriaOperator.Parse("Count()"), DevExpress.Data.Filtering.CriteriaOperator.Parse("Status = 'PendingOrdering' And Vendor.Vendor = ?", Vendor.Vendor)));
                }
                return fNumItems;
            }
        }
        #endregion

        [NonPersistent]

        public int NumItemCode
        {
            get
            {
                if (Item != null && POID != null)
                {
                    return Convert.ToInt32(Session.Evaluate(typeof(Requisition), DevExpress.Data.Filtering.CriteriaOperator.Parse("Count()"), DevExpress.Data.Filtering.CriteriaOperator.Parse("([Status] = 'PendingReceived' Or [Status] = 'PartiallyReceived') And POID.POID = ?", POID.POID)));
                }
                return 0;
            }
        }

        #region ReceivedBy
        private Employee fReceivedBy;
        //[NonPersistent]
        public Employee ReceivedBy
        {
            get { return fReceivedBy; }
            set { SetPropertyValue("ReceivedBy", ref fReceivedBy, value); }
        }
        #endregion

        #region receivedate
        private DateTime? fReceiveDate;
        //[NonPersistent]
        public DateTime? ReceiveDate
        {
            get { return fReceiveDate; }
            set { SetPropertyValue<DateTime?>("ReceiveDate", ref fReceiveDate, value); }
        }
        #endregion


        #region TotalPrice
        private double fTotalPrice;
        [NonPersistent]
        public double TotalPrice
        {
            get
            {
                if (Vendor != null)
                {
                    fTotalPrice = Convert.ToDouble(Session.Evaluate(typeof(Requisition), DevExpress.Data.Filtering.CriteriaOperator.Parse("Sum(ExpPrice)"), DevExpress.Data.Filtering.CriteriaOperator.Parse("Status = 'PendingOrdering' And Vendor.Vendor = ?", Vendor.Vendor)));
                }
                return fTotalPrice;
            }
        }
        #endregion

        #region DeliveryPriority
        private DeliveryPriority fDeliveryPriority;
        public DeliveryPriority DeliveryPriority
        {
            get { return fDeliveryPriority; }
            set { SetPropertyValue("DeliveryPriority", ref fDeliveryPriority, value); }
        }
        #endregion

        private Distribution _Distribution;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public Distribution Distribution
        {
            get
            {
                if (Status != TaskStatus.PendingReceived && Status != TaskStatus.PendingReview && Status != TaskStatus.PendingOrdering)
                {
                    Distribution objDistribution = Session.FindObject<Distribution>(CriteriaOperator.Parse("[RequisitionID] = ?", Oid));
                    if (objDistribution != null)
                    {
                        if (objDistribution != null)
                        {
                            _Distribution = objDistribution;
                        }
                        else
                        {
                            _Distribution = null;
                        }
                    }
                }
                return _Distribution;
            }
            set { SetPropertyValue("Distribution", ref _Distribution, value); }
        }

        #region NonPersistentStatus
        private NonPersistentRequitionStatus _NonPersistentStatus;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public NonPersistentRequitionStatus NonPersistentStatus
        {
            get
            {

                if (Status == TaskStatus.PendingReview)
                {
                    _NonPersistentStatus = NonPersistentRequitionStatus.PendingReview;
                }
                else if (Status == TaskStatus.PendingOrdering)
                {
                    _NonPersistentStatus = NonPersistentRequitionStatus.PendingPurchasing;
                }
                else if (Status == TaskStatus.PendingReceived)
                {
                    _NonPersistentStatus = NonPersistentRequitionStatus.PendingReceipt;
                }
                else
                {
                    Distribution objDistribution = Session.FindObject<Distribution>(CriteriaOperator.Parse("[RequisitionID] = ?", Oid));
                    if (objDistribution != null)
                    {
                        if (objDistribution.Status == LTStatus.PendingDistribute)
                        {
                            _NonPersistentStatus = NonPersistentRequitionStatus.PendingDistribution;
                        }
                        else
                        {
                            _NonPersistentStatus = NonPersistentRequitionStatus.Distributed;
                        }
                    }
                }
                return _NonPersistentStatus;
            }
            set { SetPropertyValue("NonPersistentStatus", ref _NonPersistentStatus, value); }
        }

        #endregion
        [EditorAlias(ProgressPropertyAlias)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        public double ProgressStatus
        {
            get
            {
                if (NonPersistentStatus == NonPersistentRequitionStatus.PendingReview)
                {
                    return 20;
                }
                else if (NonPersistentStatus == NonPersistentRequitionStatus.PendingPurchasing)
                {
                    return 40;
                }
                else if (NonPersistentStatus == NonPersistentRequitionStatus.PendingReceipt)
                {
                    return 60;
                }
                else if (NonPersistentStatus == NonPersistentRequitionStatus.PendingDistribution)
                {
                    return 80;
                }
                else if (NonPersistentStatus == NonPersistentRequitionStatus.Distributed)
                {
                    return 100;
                }
                else
                {
                    return 0;
                }
            }
        }

        #region Project
        private ICMProject _Project;
        public ICMProject Project
        {
            get { return _Project; }
            set { SetPropertyValue("Project", ref _Project, value); }
        }
        #endregion

    }
}