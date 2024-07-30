using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.ICM;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCombinationOfPropertiesIsUnique("ValItemTestMethodLink", DefaultContexts.Save, "LinkItems.Oid,LinkTestMethod.Oid", SkipNullOrEmptyValues = false)]
    public class ItemTestMethodLink : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ItemTestMethodLink(Session session)
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

        //[Association("Items-TestMethodLink")]
        private Items fLinkItems;
        [Association("Items-TestMethodLinks")]
        public Items LinkItems
        {
            get { return fLinkItems; }
            set { SetPropertyValue(nameof(LinkItems), ref (fLinkItems), value); }
        }


        //[Association("TestMethod-ItemsLink")]

        private TestMethod fTestMethod;
        [Association("TestMethod-ItemsLinks")]
        public TestMethod LinkTestMethod
        {
            get { return fTestMethod; }
            set { SetPropertyValue(nameof(LinkTestMethod), ref (fTestMethod), value); }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { SetPropertyValue(nameof(Amount), ref (_Amount), value); }
        }

        private Unit _Unit;
        public Unit Unit
        {
            get { return _Unit; }
            set { SetPropertyValue(nameof(Unit), ref (_Unit), value); }
        }

    }
}