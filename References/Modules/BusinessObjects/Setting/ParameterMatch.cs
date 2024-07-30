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
    public class ParameterMatch : BaseObject
    {
        public ParameterMatch(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }

        #region Parameter
        private Parameter _Parameter;
        [RuleRequiredField]
        public Parameter Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue("Parameter", ref _Parameter, value); }
        }
        #endregion

        #region MatchingParameter
        private string _MatchingParameter;

        public string MatchingParameter
        {
            get { return _MatchingParameter; }
            set { SetPropertyValue("MatchingParameter", ref _MatchingParameter, value); }
        }
        #endregion

        #region Software
        private InstrumentSoftware _Software;
        [RuleRequiredField]
        public InstrumentSoftware Software
        {
            get { return _Software; }
            set { SetPropertyValue("Software", ref _Software, value); }
        }
        #endregion

        #region Surrogate
        private bool _Surrogate;

        public bool Surrogate
        {
            get { return _Surrogate; }
            set { SetPropertyValue("Surrogate", ref _Surrogate, value); }
        }
        #endregion

        #region CreatedBy
        private CustomSystemUser _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser CreatedBy
        {
            get
            {
                return _CreatedBy;
            }
            set
            {
                SetPropertyValue("CreatedBy", ref _CreatedBy, value);
            }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                SetPropertyValue("CreatedDate", ref _CreatedDate, value);
            }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser ModifiedBy
        {
            get
            {
                return _ModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref _ModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get
            {
                return _ModifiedDate;
            }
            set
            {
                SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value);
            }
        }
        #endregion
    }
}