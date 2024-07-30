using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    public class ReportRollbackLog : BaseObject
    {
        public ReportRollbackLog(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            IsNotificationMailSent = false;
        }

        private string _ReportID;
        public string ReportID
        {
            get { return _ReportID; }
            set { SetPropertyValue<string>("ReportID", ref _ReportID, value); }
        }

        private DateTime _ReportedDate;

        public DateTime ReportedDate
        {
            get { return _ReportedDate; }
            set { SetPropertyValue("ReportedDate", ref _ReportedDate, value); }
        }

        #region ReportedBy
        private Employee _ReportedBy;
        public Employee ReportedBy
        {
            get { return _ReportedBy; }
            set { SetPropertyValue<Employee>("ReportedBy", ref _ReportedBy, value); }
        }
        #endregion

        private Samplecheckin _JobID;

        public Samplecheckin JobID
        {
            get { return _JobID; }
            set { SetPropertyValue("JobID", ref _JobID, value); }
        }

        private string _ReportName;
        public string ReportName
        {
            get { return _ReportName; }
            set { SetPropertyValue("ReportName", ref _ReportName, value); }
        }

        private DateTime? _DateRollback;
        public DateTime? DateRollback
        {
            get { return _DateRollback; }
            set { SetPropertyValue("DateRollback", ref _DateRollback, value); }
        }

        private Employee _RollbackedBy;
        public Employee RollbackedBy
        {
            get { return _RollbackedBy; }
            set { SetPropertyValue<Employee>("RollbackedBy", ref _RollbackedBy, value); }
        }

        private string _RollbackReason;
        [Size(SizeAttribute.Unlimited)]
        public string RollbackReason
        {
            get { return _RollbackReason; }
            set { SetPropertyValue("RollbackReason", ref _RollbackReason, value); }
        }

        private byte[] _Report;
        public byte[] Report
        {
            get
            {
                return _Report;
            }
            set
            {
                SetPropertyValue<byte[]>(nameof(Report), ref _Report, value);
            }
        }

        private bool _IsNotificationMailSent;
        [VisibleInDashboards(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public bool IsNotificationMailSent
        {
            get
            {
                return _IsNotificationMailSent;
            }
            set
            {
                SetPropertyValue<bool>(nameof(IsNotificationMailSent), ref _IsNotificationMailSent, value);
            }
        }

    }
}