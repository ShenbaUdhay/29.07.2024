using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    public class SampleConditionCheckPoint : BaseObject
    {
        public SampleConditionCheckPoint(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            User = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            DateTime = Library.GetServerTime(Session);
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region CheckPoint
        private SampleConditionCheckData _CheckPoint;
        public SampleConditionCheckData CheckPoint
        {
            get
            {
                return _CheckPoint;
            }
            set
            {
                SetPropertyValue<SampleConditionCheckData>("CheckPoint", ref _CheckPoint, value);
            }
        }
        #endregion

        #region Yes
        private bool _Yes;
        public bool Yes
        {
            get { return _Yes; }
            set { SetPropertyValue(nameof(_Yes), ref _Yes, value); }
        }
        #endregion

        #region No
        private bool _No;
        public bool No
        {
            get { return _No; }
            set { SetPropertyValue(nameof(_No), ref _No, value); }
        }
        #endregion

        #region NA
        private bool _NA;
        public bool NA
        {
            get { return _NA; }
            set { SetPropertyValue(nameof(_NA), ref _NA, value); }
        }
        #endregion

        #region User
        private Employee _User;
        public Employee User
        {
            get { return _User; }
            set { SetPropertyValue("User", ref _User, value); }
        }
        #endregion

        #region Initial
        [NonPersistent]
        public string Initial
        {
            get
            {
                if (User != null)
                {
                    if (!string.IsNullOrEmpty(User.Initial))
                    {
                        return User.Initial;
                    }
                    else
                    {
                        return User.FirstName.Substring(0, 1) + User.LastName.Substring(0, 1);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region DateTime
        private DateTime _DateTime;
        public DateTime DateTime
        {
            get { return _DateTime; }
            set { SetPropertyValue("DateTime", ref _DateTime, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }
        }

        #endregion        

        [Association("SampleConditionCheck-SampleConditionCheckPoint")]

        public XPCollection<SampleConditionCheck> SampleConditionCheck
        {
            get { return GetCollection<SampleConditionCheck>("SampleConditionCheck"); }
        }
    }
}