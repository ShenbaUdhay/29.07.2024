using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("SaveRule", DefaultContexts.Save, "Test, Method, InstrumentID, Active")]
    public class DailyQCSettings : BaseObject
    {
        public DailyQCSettings(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void OnSaving()
        {
            if (string.IsNullOrEmpty(DailyQCBookID))
            {
                DailyQCBookID = (Convert.ToInt32(Session.Evaluate(typeof(DailyQCSettings), CriteriaOperator.Parse("Max(DailyQCBookID)"), null)) + 1).ToString("000");
                CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                CreatedDate = DateTime.Now;
            }
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }

        private string _DailyQCBookID;
        public string DailyQCBookID
        {
            get { return _DailyQCBookID; }
            set { SetPropertyValue("DailyQCBookID", ref _DailyQCBookID, value); }
        }

        private TestMethod _Test;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [DataSourceProperty("TestDataSource")]
        [ImmediatePostData(true)]
        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue("Test", ref _Test, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (Test == null)
                {
                    XPView testsView = new XPView(Session, typeof(TestMethod));
                    testsView.Properties.Add(new ViewProperty("TTestName", SortDirection.Ascending, "TestName", true, true));
                    testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    List<object> groups = new List<object>();
                    foreach (ViewRecord rec in testsView)
                        groups.Add(rec["Toid"]);
                    return new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
                }
                else
                {
                    return null;
                }
            }
        }

        private TestMethod _Method;
        [DataSourceProperty("MethodDataSource")]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [ImmediatePostData(true)]
        public TestMethod Method
        {
            get { return _Method; }
            set { SetPropertyValue("Method", ref _Method, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> MethodDataSource
        {
            get
            {
                if (Test != null && Method == null)
                {
                    return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =?", Test.TestName));
                }
                else
                {
                    return null;
                }
            }
        }

        private Modules.BusinessObjects.Assets.Labware _InstrumentID;
        [ImmediatePostData(true)]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public Modules.BusinessObjects.Assets.Labware InstrumentID
        {
            get { return _InstrumentID; }
            set { SetPropertyValue("InstrumentID", ref _InstrumentID, value); }
        }

        private string _InstrumentName;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string InstrumentName
        {
            get
            {
                if (_InstrumentID != null)
                {
                    _InstrumentName = _InstrumentID.AssignedName;
                }
                else
                {
                    _InstrumentName = string.Empty;
                }
                return _InstrumentName;
            }
            set { SetPropertyValue<string>("InstrumentName", ref _InstrumentName, value); }

        }

        private double _StandardValue;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public double StandardValue
        {
            get { return _StandardValue; }
            set { SetPropertyValue(nameof(StandardValue), ref _StandardValue, value); }
        }

        private Unit _Units;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public Unit Units
        {
            get { return _Units; }
            set { SetPropertyValue("Units", ref _Units, value); }
        }

        private double _LCL;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public double LCL
        {
            get { return _LCL; }
            set { SetPropertyValue(nameof(LCL), ref _LCL, value); }
        }

        private double _UCL;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public double UCL
        {
            get { return _UCL; }
            set { SetPropertyValue(nameof(UCL), ref _UCL, value); }
        }

        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue(nameof(Active), ref _Active, value); }
        }

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion
    }
}