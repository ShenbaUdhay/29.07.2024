using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace ICM.Module.BusinessObjects
{
    [DefaultProperty("Evaluation")]

    public class VendorEvaluation : BaseObject
    {
        
        #region Constructor
        public VendorEvaluation(Session session) : base(session) { }
        #endregion

        #region DefaultMethods
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }
        #endregion

        #region evaluation
        string fEvaluation;
        [RuleRequiredField("EvaluationS", DefaultContexts.Save,"'Name must not be empty'")]
        public string Evaluation
        {
            get { return fEvaluation; }
            set { SetPropertyValue<string>("Evaluation", ref fEvaluation, value.Trim()); }
        }
        #endregion

        #region vendorevluationlink
        [Association("VendorEvaluationlink", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<Vendors> vendors
        {
            get { return GetCollection<Vendors>("vendors"); }
        }
        #endregion
        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion
        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>("Comment", ref _Comment, value); }
        }
    }
}