using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.QC
{
    [DefaultClassOptions]
    public class SDMSRollback : BaseObject
    {
        public SDMSRollback(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string _QCBatchID;
        public string QCBatchID
        {
            get { return _QCBatchID; }
            set { SetPropertyValue("QCBatchID", ref _QCBatchID, value); }
        }

        private SampleLogIn _SampleLoginID;
        public SampleLogIn SampleLoginID
        {
            get { return _SampleLoginID; }
            set { SetPropertyValue("SampleLoginID", ref _SampleLoginID, value); }
        }

        private string _QCType;
        public string QCType
        {
            get { return _QCType; }
            set { SetPropertyValue("QCType", ref _QCType, value); }
        }

        private TestMethod _TestMethodID;
        public TestMethod TestMethodID
        {
            get { return _TestMethodID; }
            set { SetPropertyValue("TestMethodID", ref _TestMethodID, value); }
        }

        private string _CurrentStatus;
        public string CurrentStatus
        {
            get { return _CurrentStatus; }
            set { SetPropertyValue("CurrentStatus", ref _CurrentStatus, value); }
        }

        private string _PreviousStatus;
        public string PreviousStatus
        {
            get { return _PreviousStatus; }
            set { SetPropertyValue("PreviousStatus", ref _PreviousStatus, value); }
        }

        private Employee _RollbackBy;
        public Employee RollbackBy
        {
            get { return _RollbackBy; }
            set { SetPropertyValue("RollbackBy", ref _RollbackBy, value); }
        }

        private DateTime _RollbackDate;
        public DateTime RollbackDate
        {
            get { return _RollbackDate; }
            set { SetPropertyValue("RollbackDate", ref _RollbackDate, value); }
        }

        private Employee _ResultEnteredBy;
        public Employee ResultEnteredBy
        {
            get { return _ResultEnteredBy; }
            set { SetPropertyValue("ResultEnteredBy", ref _ResultEnteredBy, value); }
        }

        private DateTime _ResultEnteredDate;
        public DateTime ResultEnteredDate
        {
            get { return _ResultEnteredDate; }
            set { SetPropertyValue("ResultEnteredDate", ref _ResultEnteredDate, value); }
        }

        private SampleParameter _SampleParameterID;
        public SampleParameter SampleParameterID
        {
            get { return _SampleParameterID; }
            set { SetPropertyValue("SampleParameterID", ref _SampleParameterID, value); }
        }

        private string _RollBackReason;
        [Size(1000)]
        public string RollBackReason
        {
            get { return _RollBackReason; }
            set { SetPropertyValue("RollBackReason", ref _RollBackReason, value); }
        }

        private string _PopupRollBackReason;
        [Size(1000)]
        [NonPersistent]
        public string PopupRollBackReason
        {
            get { return _PopupRollBackReason; }
            set { SetPropertyValue("PopupRollBackReason", ref _PopupRollBackReason, value); }
        }

        private string _AnalyticalBatchID;
        public string AnalyticalBatchID
        {
            get { return _AnalyticalBatchID; }
            set { SetPropertyValue("AnalyticalBatchID", ref _AnalyticalBatchID, value); }
        }

    }
}