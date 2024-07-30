using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.EDD
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class EDDFieldEditor : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public EDDFieldEditor(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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

        #region FieldName
        private string _FieldName;
        public string FieldName
        {
            get { return _FieldName; }
            set { SetPropertyValue(nameof(FieldName), ref _FieldName, value); }
        }
        #endregion

        #region Caption
        private string _Caption;
        public string Caption
        {
            get { return _Caption; }
            set { SetPropertyValue(nameof(Caption), ref _Caption, value); }
        }
        #endregion

        #region Width
        private int _Width;
        public int Width
        {
            get { return _Width; }
            set { SetPropertyValue(nameof(Width), ref _Width, value); }
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

        #region Visible
        private bool _Visible;
        public bool Visible
        {
            get { return _Visible; }
            set { SetPropertyValue(nameof(Visible), ref _Visible, value); }
        }
        #endregion

        #region DateTimeNeed
        private bool _DateTimeNeed;
        public bool DateTimeNeed
        {
            get { return _DateTimeNeed; }
            set { SetPropertyValue(nameof(DateTimeNeed), ref _DateTimeNeed, value); }
        }
        #endregion

        #region Frozen
        private bool _Frozen;
        public bool Frozen
        {
            get { return _Frozen; }
            set { SetPropertyValue(nameof(Frozen), ref _Frozen, value); }
        }
        #endregion

        //#region Eddbuilder
        //private EDDBuilder _EddBuilder;
        //[Association("EDDBuilder")]
        //public EDDBuilder EddBuilder
        //{
        //    get { return _EddBuilder; }
        //    set { SetPropertyValue(nameof(EddBuilder), ref _EddBuilder, value); }
        //}
        //#endregion

        #region EDDQueryBuilder
        private EDDQueryBuilder _EDDQueryBuilder;
        [Association("EDDQueryBuilderFields")]
        public EDDQueryBuilder EDDQueryBuilder
        {
            get { return _EDDQueryBuilder; }
            set { SetPropertyValue(nameof(EDDQueryBuilder), ref _EDDQueryBuilder, value); }
        }
        #endregion
    }
}