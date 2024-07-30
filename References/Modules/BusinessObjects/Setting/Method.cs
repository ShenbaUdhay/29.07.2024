using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
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

    [RuleCombinationOfPropertiesIsUnique("Method", DefaultContexts.Save, "MethodName,MethodNumber", SkipNullOrEmptyValues = false)]
    public class Method : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Method(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            var obj = Session.Evaluate(typeof(Method), null, CriteriaOperator.Parse("[MethodCode] = ? And [Oid] <> ?", MethodCode, Oid));

            if (string.IsNullOrEmpty(MethodCode) || obj != null)
            {
                int ID = Convert.ToInt32(Session.Evaluate(typeof(Method), CriteriaOperator.Parse("MAX(MethodCode)"), null)) + 1;

                if (ID.ToString().Length == 1)
                {
                    MethodCode = "000" + ID;
                }
                else if (ID.ToString().Length == 2)
                {
                    MethodCode = "00" + ID;
                }
                else if (ID.ToString().Length == 3)
                {
                    MethodCode = "0" + ID;
                }
                else
                {
                    MethodCode = ID.ToString();
                }
            }
        }

        #region MethodCategory
        private MethodCategory fMethodCategory;
        [DevExpress.Xpo.DisplayName("Category")]
        public MethodCategory MethodCategory
        {
            get { return fMethodCategory; }
            set { SetPropertyValue(nameof(MethodCategory), ref fMethodCategory, value); }
        }
        #endregion

        #region MethodCode
        private string fMethodCode;
        public string MethodCode
        {
            get
            {
                return fMethodCode;
            }
            set
            {
                SetPropertyValue(nameof(MethodCode), ref fMethodCode, value);
            }
        }
        #endregion

        #region MethodName
        private string fMethodName;
        //[Browsable(false)]
        [RuleRequiredField("MethodName", DefaultContexts.Save, "'Method' must not be empty.")]
        [Size(SizeAttribute.Unlimited)]
        public string MethodName
        {
            get
            {
                return fMethodName;
            }
            set
            {
                SetPropertyValue(nameof(MethodName), ref fMethodName, value);
            }
        }
        #endregion

        #region MethodNumber
        private string fMethodNumber;
        //[Browsable(false)]
        [RuleRequiredField("MethodNumber", DefaultContexts.Save)]
        [RuleUniqueValue]
        public string MethodNumber
        {
            get
            {
                return fMethodNumber;
            }
            set
            {
                    if (value != null)
                SetPropertyValue(nameof(MethodNumber), ref fMethodNumber, value.Trim());
            }
        }
        #endregion

        #region StandardClause
        private string fStandardClause;
        public string StandardClause
        {
            get
            {
                return fStandardClause;
            }
            set
            {
                SetPropertyValue(nameof(StandardClause), ref fStandardClause, value);
            }
        }
        #endregion

        #region ClauseSubject
        private string fClauseSubject;
        public string ClauseSubject
        {
            get
            {
                return fClauseSubject;
            }
            set
            {
                SetPropertyValue(nameof(ClauseSubject), ref fClauseSubject, value);
            }
        }
        #endregion

        #region Active
        private bool fActive;
        public bool Active
        {
            get
            {
                return fActive;
            }
            set
            {
                SetPropertyValue(nameof(Active), ref fActive, value);
            }
        }
        #endregion

        #region Comment
        private string fComment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get
            {
                return fComment;
            }
            set
            {
                SetPropertyValue(nameof(Comment), ref fComment, value);
            }
        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("MB2", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get
            {
                return fModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref fModifiedBy, value);
            }
        }
        #endregion

        #region ModifiedDate
        private DateTime fModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Appearance("MD2", Enabled = false, Context = "DetailView")]
        public DateTime ModifiedDate
        {
            get
            {
                return fModifiedDate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref fModifiedDate, value);
            }
        }
        #endregion

        #region RetireDate
        private DateTime _RetireDate;
        public DateTime RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue("RetireDate", ref _RetireDate, value); }
        }
        #endregion
        #region ActiveDate
        private DateTime _ActiveDate;
        public DateTime ActiveDate
        {
            get { return _ActiveDate; }
            set { SetPropertyValue("ActiveDate", ref _ActiveDate, value); }
        }
        #endregion
        #region DateIssued
        private DateTime _DateIssued;
        public DateTime DateIssued
        {
            get { return _DateIssued; }
            set { SetPropertyValue(nameof(DateIssued), ref _DateIssued, value); }
        }
        #endregion

        private string _Requirement;
        [Size(SizeAttribute.Unlimited)]
        //[EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public string Requirement
        {
            get { return _Requirement; }
            set { SetPropertyValue(nameof(Requirement), ref _Requirement, value); }
        }


        #region SamplingMethodCollection
        [Association("SamplingMethod-Method")]
        public XPCollection<TestMethod> SamplingMethod
        {
            get { return GetCollection<TestMethod>(nameof(SamplingMethod)); }
        }
        #endregion


    }
}