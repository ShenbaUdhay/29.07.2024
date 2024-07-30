using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Container = Modules.BusinessObjects.Setting.Container;

namespace Modules.BusinessObjects.SampleManagement
{
    public enum SamplingTransferStatus
    {
        [XafDisplayName("Pending Transfer")]
        PendingTransfer,
        [XafDisplayName("Pending Submission")]
        PendingSubmission,
        Submitted
    }

    [DefaultClassOptions]
    public class SampleBottleAllocation : BaseObject
    {
        public SampleBottleAllocation(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
        protected override void OnSaving()
        {
            base.OnSaving();
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region Qty
        private uint _Qty;
        [NonPersistent]
        public uint Qty
        {
            get { return _Qty; }
            set { SetPropertyValue("Qty", ref _Qty, value); }
        }
        #endregion

        #region BottleID
        private string _BottleID;
        [Size(SizeAttribute.Unlimited)]
        public string BottleID
        {
            get { return _BottleID; }
            set { SetPropertyValue("BottleID", ref _BottleID, value); }
        }
        #endregion

        #region SampleRegistration
        private SampleLogIn _SampleRegistration;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public SampleLogIn SampleRegistration
        {
            get
            {
                return _SampleRegistration;
            }
            set
            {
                SetPropertyValue("SampleRegistration", ref _SampleRegistration, value);
            }
        }
        #endregion

        #region SampleBottleID
        private string _SampleBottleID;
        [Size(SizeAttribute.Unlimited)]
        [NonPersistent]
        public string SampleBottleID
        {
            get
            {
                if (SampleRegistration != null && SampleRegistration.SampleID != null)
                {
                    return SampleRegistration.SampleID + "-" + BottleID;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region TestMethod
        private TestMethod _TestMethod;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public TestMethod TestMethod
        {
            get { return _TestMethod; }
            set { SetPropertyValue(nameof(TestMethod), ref _TestMethod, value); }
        }
        #endregion

        #region Preservative
        private Preservative fPreservative;
        [DataSourceProperty("PreservativeSettings")]
        public Preservative Preservative
        {
            get { return fPreservative; }
            set { SetPropertyValue("Preservative", ref fPreservative, value); }
        }

        public XPCollection<Preservative> PreservativeSettings
        {
            get
            {
                if (TestMethod != null)
                {
                    List<string> lstSM = new List<string>();
                    IList<TestGuide> objVM = Session.GetObjects(Session.GetClassInfo(typeof(TestGuide)), CriteriaOperator.Parse("[TestMethod.Oid] = ?", TestMethod.Oid), null, int.MaxValue, false, true).Cast<TestGuide>().ToList();
                    foreach (TestGuide test in objVM)
                    {
                        if (test != null && test.Preservative != null && !lstSM.Contains(test.Preservative.PreservativeName))
                        {
                            lstSM.Add(test.Preservative.PreservativeName);
                        }
                    }
                    return new XPCollection<Preservative>(Session, new InOperator("PreservativeName", lstSM));
                }
                else
                {
                    return new XPCollection<Preservative>(Session, CriteriaOperator.Parse("[Oid] is null"));
                }
            }
        }
        #endregion

        #region Containers
        private Container _Containers;
        [DataSourceProperty("ContainerSettings")]
        public Container Containers
        {
            get { return _Containers; }
            set { SetPropertyValue("Containers", ref _Containers, value); }
        }

        public XPCollection<Container> ContainerSettings
        {
            get
            {
                if (TestMethod != null)
                {
                    List<string> lstSM = new List<string>();
                    IList<TestGuide> objVM = Session.GetObjects(Session.GetClassInfo(typeof(TestGuide)), CriteriaOperator.Parse("[TestMethod.Oid] = ?", TestMethod.Oid), null, int.MaxValue, false, true).Cast<TestGuide>().ToList();
                    foreach (TestGuide test in objVM)
                    {
                        if (test != null && test.Container != null && !lstSM.Contains(test.Container.ContainerName))
                        {
                            lstSM.Add(test.Container.ContainerName);
                        }
                    }
                    return new XPCollection<Container>(Session, new InOperator("ContainerName", lstSM));
                }
                else
                {
                    return new XPCollection<Container>(Session, CriteriaOperator.Parse("[Oid] is null"));
                }
            }
        }
        #endregion

        #region StorageID
        private Storage _StorageID;
        public Storage StorageID
        {
            get { return _StorageID; }
            set { SetPropertyValue("StorageID", ref _StorageID, value); }
        }
        #endregion

        #region StorageCondition
        private PreserveCondition _StorageCondition;
        public PreserveCondition StorageCondition
        {
            get { return _StorageCondition; }
            set { SetPropertyValue("StorageCondition", ref _StorageCondition, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
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

        #region SignOffBy
        private Employee _SignOffBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee SignOffBy
        {
            get
            {
                return _SignOffBy;
            }
            set
            {
                SetPropertyValue(nameof(SignOffBy), ref _SignOffBy, value);
            }
        }
        #endregion

        #region SignOffDate
        private Nullable<DateTime> _SignOffDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Nullable<DateTime> SignOffDate
        {
            get
            {
                return _SignOffDate;
            }
            set
            {
                SetPropertyValue(nameof(SignOffDate), ref _SignOffDate, value);
            }
        }
        #endregion

        #region RollbackedDate
        private Nullable<DateTime> _RollbackedDate;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Nullable<DateTime> RollbackedDate
        {
            get { return _RollbackedDate; }
            set { SetPropertyValue(nameof(RollbackedDate), ref _RollbackedDate, value); }
        }
        #endregion

        #region RollbackedBy
        private Employee _RollbackedBy;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Employee RollbackedBy
        {
            get { return _RollbackedBy; }
            set { SetPropertyValue(nameof(RollbackedBy), ref _RollbackedBy, value); }
        }
        #endregion

        #region RollBackReason
        private string _RollbackReason;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string RollbackReason
        {
            get { return _RollbackReason; }
            set { SetPropertyValue(nameof(RollbackReason), ref _RollbackReason, value); }
        }
        #endregion

        #region NPBottleID
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string NPBottleID
        {
            get
            {
                if (SampleRegistration != null && TestMethod != null)
                {
                    IList<SampleBottleAllocation> sample = Session.GetObjects(Session.GetClassInfo(typeof(SampleBottleAllocation)), CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [TestMethod.Oid]=?", SampleRegistration.Oid, TestMethod.Oid), null, int.MaxValue, false, false).Cast<SampleBottleAllocation>().ToList();
                    if (sample != null)
                    {
                        return string.Join(", ", sample.Select(a => a.BottleID).OrderBy(a => a));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region NPTest
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string NPTest
        {
            get
            {
                if (SampleRegistration != null)
                {
                    IList<SampleBottleAllocation> sample = Session.GetObjects(Session.GetClassInfo(typeof(SampleBottleAllocation)), CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [BottleID]=?", SampleRegistration.Oid, BottleID), null, int.MaxValue, false, false).Cast<SampleBottleAllocation>().ToList();
                    if (sample != null)
                    {
                        return string.Join(", ", sample.Select(a => a.TestMethod.TestName).OrderBy(a => a));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region ReceivedBy
        private Employee fReceivedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ReceivedBy
        {
            get
            {
                return fReceivedBy;
            }
            set
            {
                SetPropertyValue("ReceivedBy", ref fReceivedBy, value);
            }
        }
        #endregion

        #region ReceivedDate
        private DateTime? fReceivedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime? ReceivedDate
        {
            get
            {
                return fReceivedDate;
            }
            set
            {
                SetPropertyValue("ReceivedDate", ref fReceivedDate, value);
            }
        }
        #endregion

        #region IsRemark
        [NonPersistent]
        public bool IsRemark
        {
            get { return !string.IsNullOrEmpty(Remark); }
        }
        #endregion

        #region Remark
        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue("Remark", ref _Remark, value); }
        }
        #endregion

        #region SampleStatus
        private SampleStatus _SampleStatus;
        public SampleStatus SampleStatus
        {
            get { return _SampleStatus; }
            set { SetPropertyValue(nameof(SampleStatus), ref _SampleStatus, value); }
        }
        #endregion

        #region SampleTransferStatus
        private SamplingTransferStatus _SampleTransferStatus;
        [DefaultValue(0)]
        public SamplingTransferStatus SampleTransferStatus
        {
            get { return _SampleTransferStatus; }
            set { SetPropertyValue(nameof(SamplingTransferStatus), ref _SampleTransferStatus, value); }
        }
        #endregion

        #region BroughtBy
        private BroughtBy _BroughtBy;
        public BroughtBy BroughtBy
        {
            get { return _BroughtBy; }
            set { SetPropertyValue<BroughtBy>("BroughtBy", ref _BroughtBy, value); }
        }
        #endregion

        #region ScanDateTime
        private DateTime? fScanDateTime;
        public DateTime? ScanDateTime
        {
            get
            {
                return fScanDateTime;
            }
            set
            {
                SetPropertyValue("ScanDateTime", ref fScanDateTime, value);
            }
        }
        #endregion

        #region STPassword
        private string _STPassword;
        [NonPersistent]
        public string STPassword
        {
            get { return _STPassword; }
            set { SetPropertyValue<string>("STPassword", ref _STPassword, value); }
        }
        #endregion

        #region STNoSamples
        private int _STNoSamples;
        public int STNoSamples
        {
            get { return _STNoSamples; }
            set { SetPropertyValue<int>("STNoSamples", ref _STNoSamples, value); }
        }
        #endregion

        #region STNoContainers
        private int _STNoContainers;
        public int STNoContainers
        {
            get { return _STNoContainers; }
            set { SetPropertyValue<int>("STNoContainers", ref _STNoContainers, value); }
        }
        #endregion

        #region STNoCoolers
        private uint _STNoCoolers;
        public uint STNoCoolers
        {
            get { return _STNoCoolers; }
            set { SetPropertyValue<uint>("STNoCoolers", ref _STNoCoolers, value); }
        }
        #endregion

        #region SampleAppearance
        private string _SampleAppearance;
        public string SampleAppearance
        {
            get { return _SampleAppearance; }
            set { SetPropertyValue("SampleAppearance", ref _SampleAppearance, value); }
        }
        #endregion

        #region TransportCondition
        private string _TransportCondition;
        public string TransportCondition
        {
            get { return _TransportCondition; }
            set { SetPropertyValue("TransportCondition", ref _TransportCondition, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }
        }
        #endregion

        #region NPTestSummary
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string NPTestSummary
        {
            get
            {
                if (SampleRegistration != null)
                {
                    IList<SampleBottleAllocation> sample = Session.GetObjects(Session.GetClassInfo(typeof(SampleBottleAllocation)), CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [SampleTransferStatus]=?", SampleRegistration.Oid, SampleTransferStatus.ToString()), null, int.MaxValue, false, false).Cast<SampleBottleAllocation>().ToList();
                    if (sample != null)
                    {
                        return string.Join(", ", sample.Select(a => a.TestMethod.TestName).Distinct().OrderBy(a => a));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region NPScanBottle
        private string _NPScanBottle;
        [NonPersistent]
        public string NPScanBottle
        {
            get { return _NPScanBottle; }
            set { SetPropertyValue("NPScanBottle", ref _NPScanBottle, value); }
        }
        #endregion

        #region CoolerId
        private string _CoolerId;
        public string CoolerId
        {
            get { return _CoolerId; }
            set { SetPropertyValue<string>("CoolerId", ref _CoolerId, value); }
        }
        #endregion

        #region CoolerTemp
        private string _CoolerTemp;
        public string CoolerTemp
        {
            get { return _CoolerTemp; }
            set { SetPropertyValue<string>("CoolerTemp", ref _CoolerTemp, value); }
        }
        #endregion
    }
}