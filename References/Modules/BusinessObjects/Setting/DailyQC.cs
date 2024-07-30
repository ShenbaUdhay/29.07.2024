using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class DailyQC : BaseObject
    {
        public DailyQC(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        protected override void OnSaving()
        {
            if (string.IsNullOrEmpty(DLQCID))
            {
                string tempid = (Convert.ToInt32(Session.Evaluate(typeof(DailyQC), CriteriaOperator.Parse("Max(DLQCID)"), null)) + 1).ToString();
                var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempid != "1")
                {
                    var predate = tempid.Substring(0, 6);
                    if (predate != curdate)
                    {
                        tempid = curdate + "01";
                    }
                }
                else
                {
                    tempid = curdate + "01";
                }
                DLQCID = tempid;
                CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                CreatedDate = DateTime.Now;
            }
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
        }

        private string _DLQCID;
        public string DLQCID
        {
            get { return _DLQCID; }
            set { SetPropertyValue("DLQCID", ref _DLQCID, value); }
        }

        private DateTime _Date;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue("Date", ref _Date, value); }
        }

        private Employee _Analyst;
        public Employee Analyst
        {
            get { return _Analyst; }
            set { SetPropertyValue("Analyst", ref _Analyst, value); }
        }

        private double _Reading;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [ImmediatePostData(true)]
        public double Reading
        {
            get { return _Reading; }
            set { SetPropertyValue("Reading", ref _Reading, value); }
        }

        private double _ReferenceValue;
        public double ReferenceValue
        {
            get { return _ReferenceValue; }
            set { SetPropertyValue(nameof(ReferenceValue), ref _ReferenceValue, value); }
        }

        private Unit _Units;
        public Unit Units
        {
            get { return _Units; }
            set { SetPropertyValue("Units", ref _Units, value); }
        }

        private double _LCL;
        public double LCL
        {
            get { return _LCL; }
            set { SetPropertyValue(nameof(LCL), ref _LCL, value); }
        }

        private double _UCL;
        public double UCL
        {
            get { return _UCL; }
            set { SetPropertyValue(nameof(UCL), ref _UCL, value); }
        }

        private string _Status;
        [ImmediatePostData(true)]
        public string Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }

        private Modules.BusinessObjects.Assets.Labware _InstrumentID;
        public Modules.BusinessObjects.Assets.Labware InstrumentID
        {
            get { return _InstrumentID; }
            set { SetPropertyValue("InstrumentID", ref _InstrumentID, value); }
        }

        private string _InstrumentName;
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

        private TestMethod _Test;
        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue("Test", ref _Test, value); }
        }

        private TestMethod _Method;
        public TestMethod Method
        {
            get { return _Method; }
            set { SetPropertyValue("Method", ref _Method, value); }
        }

        private string _StandardLot;
        public string StandardLot
        {
            get { return _StandardLot; }
            set { SetPropertyValue(nameof(StandardLot), ref _StandardLot, value); }
        }

        private FileData _Attach;
        public FileData Attach
        {
            get { return _Attach; }
            set { SetPropertyValue(nameof(Attach), ref _Attach, value); }
        }

        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }

        private double _Mean;
        [NonPersistent]
        public double Mean
        {
            get { return _Mean; }
            set { SetPropertyValue(nameof(Mean), ref _Mean, value); }
        }

        [NonPersistent]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public string CDate
        {
            get { return Date != null ? Date.ToString("MM/dd/yy") : string.Empty; }
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