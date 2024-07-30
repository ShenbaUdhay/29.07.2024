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
    public class FiberTypesValues : BaseObject
    {
        public FiberTypesValues(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Createdby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            Modifiedby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region FiberType
        private string _FiberType;
        [RuleRequiredField]
        public string FiberType
        {
            get { return _FiberType; }
            set { SetPropertyValue(nameof(FiberType), ref _FiberType, value); }
        }
        #endregion
        #region RIAlpha
        private string _RIAlpha;
        public string RIAlpha
        {
            get { return _RIAlpha; }
            set { SetPropertyValue(nameof(RIAlpha), ref _RIAlpha, value); }
        }
        #endregion
        #region GammaPrep
        private string _GammaPrep;
        public string GammaPrep
        {
            get { return _GammaPrep; }
            set { SetPropertyValue(nameof(GammaPrep), ref _GammaPrep, value); }
        }
        #endregion
        #region RIGamma
        private string _RIGamma;
        public string RIGamma
        {
            get { return _RIGamma; }
            set { SetPropertyValue(nameof(RIGamma), ref _RIGamma, value); }
        }
        #endregion
        #region GammaPara
        private string _GammaPara;
        public string GammaPara
        {
            get { return _GammaPara; }
            set { SetPropertyValue(nameof(GammaPara), ref _GammaPara, value); }
        }
        #endregion
        #region Pleochroism
        private string _Pleochroism;
        public string Pleochroism
        {
            get { return _Pleochroism; }
            set { SetPropertyValue(nameof(Pleochroism), ref _Pleochroism, value); }
        }
        #endregion
        #region Birefringence
        private string _Birefringence;
        public string Birefringence
        {
            get { return _Birefringence; }
            set { SetPropertyValue(nameof(Birefringence), ref _Birefringence, value); }
        }
        #endregion
        #region Extinction
        private string _Extinction;
        public string Extinction
        {
            get { return _Extinction; }
            set { SetPropertyValue(nameof(Extinction), ref _Extinction, value); }
        }
        #endregion
        #region Elognation
        private string _Elognation;
        public string Elognation
        {
            get { return _Elognation; }
            set { SetPropertyValue(nameof(Elognation), ref _Elognation, value); }
        }
        #endregion
        #region Parallel
        private string _Parallel;
        public string Parallel
        {
            get { return _Parallel; }
            set { SetPropertyValue(nameof(Parallel), ref _Parallel, value); }
        }
        #endregion
        #region Perpenducular
        private string _Perpenducular;
        public string Perpenducular
        {
            get { return _Perpenducular; }
            set { SetPropertyValue(nameof(Perpenducular), ref _Perpenducular, value); }
        }
        #endregion
        #region Morphology
        private string _Morphology;
        public string Morphology
        {
            get { return _Morphology; }
            set { SetPropertyValue(nameof(Morphology), ref _Morphology, value); }
        }
        #endregion
        #region Isotropic
        private string _Isotropic;
        public string Isotropic
        {
            get { return _Isotropic; }
            set { SetPropertyValue(nameof(Isotropic), ref _Isotropic, value); }
        }
        #endregion
        #region RI
        private string _RI;
        public string RI
        {
            get { return _RI; }
            set { SetPropertyValue(nameof(RI), ref _RI, value); }
        }
        #endregion
        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }
        #endregion
        #region Modifiedby
        private CustomSystemUser _Modifiedby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser Modifiedby
        {
            get { return _Modifiedby; }
            set { SetPropertyValue("Modifiedby", ref _Modifiedby, value); }
        }
        #endregion
        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }
        #endregion
        #region Createdby
        private CustomSystemUser _Createdby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser Createdby
        {
            get { return _Createdby; }
            set { SetPropertyValue("Createdby", ref _Createdby, value); }
        }
        #endregion
    }
}