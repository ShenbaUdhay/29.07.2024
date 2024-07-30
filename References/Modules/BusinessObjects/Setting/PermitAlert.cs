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
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using DevExpress.Persistent.Base.General;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PermitAlert : BaseObject, ISupportNotifications
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        private TaskImpl task = new TaskImpl();
        private DateTime? alarmTime;
        protected override void OnLoading()
        {
            task.IsLoading = true;
            base.OnLoading();
        }
        protected override void OnLoaded()
        {
            base.OnLoaded();
            task.IsLoading = false;
        }
        private void SetAlarmTime(DateTime? startDate, TimeSpan remindTime)
        {
            alarmTime = DateTime.Now;
        }
        public PermitAlert(Session session)
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

        public string Subject
        {
            get { return task.Subject; }
            set
            {
                string oldValue = task.Subject;
                task.Subject = value;
                OnChanged("Subject", oldValue, task.Subject);
            }
        }
        [Size(SizeAttribute.Unlimited), ObjectValidatorIgnoreIssue(typeof(ObjectValidatorLargeNonDelayedMember))]
        public string Description
        {
            get { return task.Description; }
            set
            {
                string oldValue = task.Description;
                task.Description = value;
                OnChanged("Description", oldValue, task.Description);
            }
        }
        public DateTime? AlarmTime
        {
            get { return alarmTime; }
            set
            {
                SetPropertyValue("AlarmTime", ref alarmTime, value);
                if (value == null)
                {
                    IsPostponed = false;
                }
            }
        }
        [Browsable(false)]
        [NonPersistent]
        public string NotificationMessage
        {
            get { return Subject; }
        }
        [NonPersistent]
        [Browsable(false)]
        public object UniqueId
        {
            get { return Oid; }
        }

        [Browsable(false)]
        public bool IsPostponed
        {
            get;
            set;
        }
    }
}