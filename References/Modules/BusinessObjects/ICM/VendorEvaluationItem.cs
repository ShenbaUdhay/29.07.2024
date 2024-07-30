using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.ComponentModel;

namespace Modules.BusinessObjects.ICM
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    [DefaultProperty("EvaluationItem")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[RuleCombinationOfPropertiesIsUnique("VendorEvaluationItem", DefaultContexts.Save, "EvaluationItem", SkipNullOrEmptyValues = false)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class VendorEvaluationItem : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public VendorEvaluationItem(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            UpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            UpdatedDate = Library.GetServerTime(Session);
        }
        private string _EvaluationItem;
        [RuleRequiredField("EvaluationItem", DefaultContexts.Save)]
        // [RuleStringComparison("RuleStringComparison_EvalutionItem_NotNullOrEmpty", DefaultContexts.Save, StringComparisonType.NotEquals, null)]
        [XafDisplayName("Name")]
        public string EvaluationItem
        {
            get { return _EvaluationItem; }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetPropertyValue<string>("EvaluationItem", ref _EvaluationItem, value.Trim());
            }
        }
        private string _Comment;
        //[NonPersistent]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>("Comment", ref _Comment, value); }
        }
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

        #region UpdatedDate
        private DateTime _UpdatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime UpdatedDate
        {
            get { return _UpdatedDate; }
            set { SetPropertyValue("UpdatedDate", ref _UpdatedDate, value); }
        }
        #endregion

        #region UpdatedBy
        private Employee _UpdatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee UpdatedBy
        {
            get { return _UpdatedBy; }
            set { SetPropertyValue("UpdatedBy", ref _UpdatedBy, value); }
        }

        #endregion
    }
}