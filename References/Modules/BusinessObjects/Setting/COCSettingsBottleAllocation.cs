using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class COCSettingsBottleAllocation : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public COCSettingsBottleAllocation(Session session)
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

        #region COCSettingsRegistration
        private COCSettingsSamples _COCSettingsRegistration;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public COCSettingsSamples COCSettingsRegistration
        {
            get
            {
                return _COCSettingsRegistration;
            }
            set
            {
                SetPropertyValue("COCSettingsRegistration", ref _COCSettingsRegistration, value);
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

        #region NPBottleID
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string NPBottleID
        {
            get
            {
                if (COCSettingsRegistration != null && TestMethod != null)
                {
                    IList<COCSettingsBottleAllocation> sample = Session.GetObjects(Session.GetClassInfo(typeof(COCSettingsBottleAllocation)), CriteriaOperator.Parse("[COCSettingsRegistration.Oid]=? And [TestMethod.Oid]=?", COCSettingsRegistration.Oid, TestMethod.Oid), null, int.MaxValue, false, false).Cast<COCSettingsBottleAllocation>().ToList();
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
    }
}