using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [DefaultProperty("Title")]
    public class SampleConditionCheckComment : BaseObject
    {
        public SampleConditionCheckComment(Session session) : base(session) { }
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
        #region Title
        private string _Title;
        [RuleRequiredField ("Titless",DefaultContexts.Save,"Title must not be empty")]
        public string Title
        {
            get { return _Title; }
            set { SetPropertyValue(nameof(_Title), ref _Title, value); }
        }
        #endregion

        #region Note
        private string _Note;
        public string Note
        {
            get { return _Note; }
            set { SetPropertyValue(nameof(_Note), ref _Note, value); }
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

        [Association("SampleConditionCheck-SampleConditionCheckComment")]

        public XPCollection<SampleConditionCheck> SampleConditionCheck
        {
            get { return GetCollection<SampleConditionCheck>("SampleConditionCheck"); }
        }
    }
}