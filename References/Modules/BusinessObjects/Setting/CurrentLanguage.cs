using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CurrentLanguage : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        string strLanguage = string.Empty;
        public CurrentLanguage(Session session)
            : base(session)
        {

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();

        }
        protected override void OnSaved()
        {
            base.OnSaved();

        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

        }

        //protected override void BeforeSave()
        //{
        //    base.BeforeSave();
        //}
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        #region Chinese
        private bool _Chinese;
        [ImmediatePostData]
        public bool Chinese
        {
            get
            {
                return _Chinese;
            }
            set
            {
                SetPropertyValue("Chinese", ref _Chinese, value);
            }
        }
        #endregion Chinese

        #region English
        private bool _English;
        [ImmediatePostData]
        public bool English
        {
            get
            {
                return _English;
            }
            set
            {
                if (Chinese == true)
                {

                }
                SetPropertyValue("English", ref _English, value);
            }
        }
        #endregion English
    }


}