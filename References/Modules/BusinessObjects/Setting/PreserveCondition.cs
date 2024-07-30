using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class PreserveCondition : BaseObject
    {
        public PreserveCondition(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            EnteredBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            EnteredDate = Library.GetServerTime(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (ID == 0)
            {
                ID = Convert.ToUInt32(Session.Evaluate(typeof(PreserveCondition), CriteriaOperator.Parse("MAX(ID)"), null)) + 1;
            }
        }
        #region ID
        private uint _ID;
        public uint ID
        {
            get
            {
                return _ID;
            }
            set
            {
                SetPropertyValue<uint>(nameof(ID), ref _ID, value);
            }
        }
        #endregion

        #region PreserveConditionName
        private string _PreservativeConditionName;
        [RuleRequiredField("PreserveCondition.PreserveConditionName", DefaultContexts.Save)]
        public string PreserveConditionName
        {
            get { return _PreservativeConditionName; }
            set { SetPropertyValue(nameof(PreserveConditionName), ref _PreservativeConditionName, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        [Size(1000)]
        public String Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }

        }
        #endregion

        #region EnteredDate
        private DateTime _EnteredDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime EnteredDate
        {
            get { return _EnteredDate; }
            set { SetPropertyValue("EnteredDate", ref _EnteredDate, value); }

        }
        #endregion

        #region EnteredBy
        private CustomSystemUser _EnteredBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser EnteredBy
        {
            get { return _EnteredBy; }
            set { SetPropertyValue("EnteredBy", ref _EnteredBy, value); }

        }
        #endregion

    }
}