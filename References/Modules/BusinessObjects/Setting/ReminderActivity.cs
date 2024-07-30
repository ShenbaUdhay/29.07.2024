using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class ReminderActivity : Event
    {
        public ReminderActivity(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = Library.GetServerTime(Session);
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }

        DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        Employee _CreatedBy;
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue<Employee>(nameof(CreatedBy), ref _CreatedBy, value); }
        }

        private Samplecheckin _JobID;
        public Samplecheckin JobID
        {
            get { return _JobID; }
            set { SetPropertyValue<Samplecheckin>(nameof(JobID), ref _JobID, value); }
        }

    }
}