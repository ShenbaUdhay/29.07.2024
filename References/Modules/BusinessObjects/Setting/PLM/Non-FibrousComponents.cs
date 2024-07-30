using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class Non_FibrousComponents : BaseObject
    {
        public Non_FibrousComponents(Session session) : base(session) { }
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

        #region NonFibrousComponenetName
        private string _NonFibrousComponenetName;
        [RuleRequiredField]

        public string NonFibrousComponenetName
        {
            get { return _NonFibrousComponenetName; }
            set { SetPropertyValue(nameof(NonFibrousComponenetName), ref _NonFibrousComponenetName, value); }
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