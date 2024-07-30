using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;

namespace Modules.BusinessObjects.EDD
{
    [DefaultClassOptions]
    [RuleCombinationOfPropertiesIsUnique("EDDQuery", DefaultContexts.Save, "QueryName,EDDBuilder", SkipNullOrEmptyValues = false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EDDQueryBuilder : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        EDDInfo objEDDInfo = new EDDInfo();
        public EDDQueryBuilder(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            SheetName = "Sheet" + objEDDInfo.EDDBuilderQuerycount.ToString();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region QueryName
        private string _QueryName;
        [RuleRequiredField]
        public string QueryName
        {
            get { return _QueryName; }
            set { SetPropertyValue(nameof(QueryName), ref _QueryName, value); }
        }
        #endregion
        #region SheetName
        private string _SheetName;
        [RuleRequiredField]
        public string SheetName
        {
            get { return _SheetName; }
            set { SetPropertyValue(nameof(SheetName), ref _SheetName, value); }
        }
        #endregion

        //#region 
        //private EDDBuilder _EDDBuilder;
        //public EDDBuilder EDDBuilder
        //{
        //    get { return _EDDBuilder; }
        //    set { SetPropertyValue(nameof(EDDBuilder), ref _EDDBuilder, value); }
        //}
        //#endregion

        #region QueryBuilder
        private string _QueryBuilder;
        [Size(SizeAttribute.Unlimited)]
        public string QueryBuilder
        {
            get { return _QueryBuilder; }
            set { SetPropertyValue(nameof(QueryBuilder), ref _QueryBuilder, value); }
        }
        #endregion

        #region StrQueryBuilder
        private string _StrQueryBuilder;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        public string StrQueryBuilder
        {
            get
            {
                if (!string.IsNullOrEmpty(QueryBuilder))
                {
                    if (QueryBuilder.Length > 50)
                    {
                        _StrQueryBuilder = QueryBuilder.Substring(0, 50);
                    }
                    else
                    {
                        _StrQueryBuilder = QueryBuilder;
                    }
                }
                return _StrQueryBuilder;
            }
            set { SetPropertyValue(nameof(StrQueryBuilder), ref _StrQueryBuilder, value); }
        }
        #endregion

        #region EDDBuilder
        private EDDBuilder _EDDBuilder;
        [Association("EDDQueryBuilders")]
        public EDDBuilder EDDBuilder
        {
            get { return _EDDBuilder; }
            set { SetPropertyValue(nameof(EDDBuilder), ref _EDDBuilder, value); }
        }
        #endregion

        #region EDDFieldEditor
        //private EDDFieldEditor _EDDFieldEditor;
        //[Association("EDDQueryBuilderFields")]
        //public EDDFieldEditor EDDFieldEditor
        //{
        //    get { return _EDDFieldEditor; }
        //    set { SetPropertyValue(nameof(EDDFieldEditor), ref _EDDFieldEditor, value); }
        //}

        [Association("EDDQueryBuilderFields")]
        public XPCollection<EDDFieldEditor> EDDFieldEditors
        {
            get
            {
                return GetCollection<EDDFieldEditor>(nameof(EDDFieldEditors));
            }
        }
        #endregion
    }
}