using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.EDD;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Setting.EDD;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EDDBuilder : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EDDBuilder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            //Sheets = 1;
            DateCreated = DateTime.Now;
            Active = true;
            Author = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(EDDID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(EDDID, 3))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(EDDBuilder), criteria, null)) + 1).ToString();
                //var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "1")
                {
                    tempID = "EDD" + tempID;
                }
                else
                {
                    tempID = "EDD" + "1001";
                }

                EDDID = tempID;

            }
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        #region EDDID
        private string _EDDID;
        public string EDDID
        {
            get { return _EDDID; }
            set { SetPropertyValue(nameof(EDDID), ref _EDDID, value); }
        }
        #endregion

        #region EDDName
        private string _EDDName;
        //[RuleRequiredField]
        [RuleRequiredField("EDDName", DefaultContexts.Save, "'EDD Name' must not to be empty.")]

        [RuleUniqueValue]
        public string EDDName
        {
            get { return _EDDName; }
            set
            {
                if (value != null && string.IsNullOrEmpty(value.Trim()))
                {
                    value = null;
                }
                SetPropertyValue(nameof(EDDName), ref _EDDName, value);
            }
        }
        #endregion

        #region Description
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        #endregion

        #region Category
        private EDDCategory _Category;
        public EDDCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue(nameof(Category), ref _Category, value); }
        }
        #endregion

        #region Sheets
        private EDDQueryBuilder _Sheets;
        [NonPersistent]
        public EDDQueryBuilder Sheets
        {
            get { return _Sheets; }
            set { SetPropertyValue(nameof(Sheets), ref _Sheets, value); }
        }
        #endregion

        #region Client
        private Customer _Client;
        public Customer Client
        {
            get { return _Client; }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        }
        #endregion

        #region DateCreated
        private DateTime _DateCreated;
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { SetPropertyValue(nameof(DateCreated), ref _DateCreated, value); }
        }
        #endregion

        #region Author
        private Employee _Author;
        public Employee Author
        {
            get { return _Author; }
            set { SetPropertyValue(nameof(Author), ref _Author, value); }
        }
        #endregion

        #region Active
        private bool _Active;
        [ImmediatePostData]
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue(nameof(Active), ref _Active, value); }
        }
        #endregion

        #region Retire
        private bool _Retire;
        [ImmediatePostData]
        public bool Retire
        {
            get { return _Retire; }
            set { SetPropertyValue(nameof(Retire), ref _Retire, value); }
        }
        #endregion

        #region Remark
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        #endregion

        //#region QueryBuilder
        //private string _QueryBuilder;
        //[Size(SizeAttribute.Unlimited)]
        //public string QueryBuilder
        //{
        //    get { return _QueryBuilder; }
        //    set { SetPropertyValue(nameof(QueryBuilder), ref _QueryBuilder, value); }
        //}
        //#endregion


        [Association("EDDQueryBuilders")]
        public XPCollection<EDDQueryBuilder> EDDQueryBuilders
        {
            get
            {
                return GetCollection<EDDQueryBuilder>(nameof(EDDQueryBuilders));
            }
        }
    }
}