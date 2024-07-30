using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using Container = Modules.BusinessObjects.Setting.Container;

namespace Modules.BusinessObjects.SamplingManagement
{
    [DefaultClassOptions]
    public class SamplingBottleAllocation : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SamplingBottleAllocation(Session session)
            : base(session)
        {
        }
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
        private Sampling _Sampling;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Sampling Sampling
        {
            get
            {
                return _Sampling;
            }
            set
            {
                SetPropertyValue("Sampling", ref _Sampling, value);
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
        [NonPersistent]
        [Browsable(false)]
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

        [NonPersistent]
        [Browsable(false)]
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
        private uint _DefaultContainerQty;
        [NonPersistent]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false),VisibleInDashboards(false)]
        public uint DefaultContainerQty
        {
            get { return _DefaultContainerQty; }
            set { SetPropertyValue("DefaultContainerQty", ref _DefaultContainerQty, value); }

        }
    }
}