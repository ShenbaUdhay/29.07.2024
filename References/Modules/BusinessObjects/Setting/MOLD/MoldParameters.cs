using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting.Mold
{
    [DefaultClassOptions]

    public class MoldParameters : BaseObject
    {
        public MoldParameters(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region MoldParameter
        private string _MoldParameter;
        [RuleRequiredField]
        public string MoldParameter
        {
            get { return _MoldParameter; }
            set { SetPropertyValue(nameof(MoldParameter), ref _MoldParameter, value); }
        }
        #endregion

        #region IsPopulationRange
        private bool _IsPopulationRange;
        public bool IsPopulationRange
        {
            get { return _IsPopulationRange; }
            set { SetPropertyValue(nameof(IsPopulationRange), ref _IsPopulationRange, value); }
        }
        #endregion

        #region Sort
        private int _Sort;
        public int Sort
        {
            get { return _Sort; }
            set { SetPropertyValue(nameof(Sort), ref _Sort, value); }
        }
        #endregion

        #region Set1
        private bool _Set1;
        public bool Set1
        {
            get { return _Set1; }
            set { SetPropertyValue(nameof(Set1), ref _Set1, value); }
        }
        #endregion

        #region Set2
        private bool _Set2;
        public bool Set2
        {
            get { return _Set2; }
            set { SetPropertyValue(nameof(Set2), ref _Set2, value); }
        }
        #endregion

        #region Character
        private string _Character;
        [Size(SizeAttribute.Unlimited)]
        public string Character
        {
            get { return _Character; }
            set { SetPropertyValue(nameof(Character), ref _Character, value); }
        }
        #endregion

        #region Description
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [Browsable(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private CustomSystemUser _CreatedBy;
        [Browsable(false)]
        public CustomSystemUser CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [Browsable(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [Browsable(false)]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }

        #endregion


    }
}