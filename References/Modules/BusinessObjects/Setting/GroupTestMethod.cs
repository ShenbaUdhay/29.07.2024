using DevExpress.ExpressApp.DC;
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
    public class GroupTestMethod : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public GroupTestMethod(Session session)
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
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}

        //#region GroupTest      
        //private GroupTest _GroupTests;
        //[Association]
        //public GroupTest GroupTests
        //{
        //    get { return _GroupTests; }
        //    set { SetPropertyValue("GroupTests", ref _GroupTests, value); }
        //}
        //#endregion

        #region TestMethod
        private TestMethod _TestMethods;
        [Association]
        public TestMethod TestMethods
        {
            get { return _TestMethods; }
            set { SetPropertyValue("TestMethods", ref _TestMethods, value); }
        }
        #endregion

        #region TestMethod
        private TestMethod _TestMethod;
        public TestMethod TestMethod
        {
            get { return _TestMethod; }
            set { SetPropertyValue("TestMethod", ref _TestMethod, value); }
        }
        #endregion

        #region Tests
        private TestMethod _Tests;
        public TestMethod Tests
        {
            get { return _Tests; }
            set { SetPropertyValue("Tests", ref _Tests, value); }
        }
        #endregion

        #region TestParameter
        private Testparameter _TestParameter;
        public Testparameter TestParameter
        {
            get { return _TestParameter; }
            set { SetPropertyValue("TestParameter", ref _TestParameter, value); }
        }
        #endregion

        #region GroupTestParameter
        private string _GroupTestParameter;
        [XafDisplayName("Parameter")]
        [ImmediatePostData]
        [NonPersistent]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string GroupTestParameter
        {
            get
            {
                if (!string.IsNullOrEmpty(GroupTestParameters))
                {
                    _GroupTestParameter = "Customized";
                }
                else
                {
                    _GroupTestParameter = "Default";
                }
                return _GroupTestParameter;
            }
            set { SetPropertyValue("GroupTestParameter", ref _GroupTestParameter, value); }
        }
        #endregion

        #region GroupTestParameters
        private string _GroupTestParameters;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        [ImmediatePostData]
        public string GroupTestParameters
        {
            get
            {
                return _GroupTestParameters;
            }
            set { SetPropertyValue("GroupTestParameters", ref _GroupTestParameters, value); }
        }
        #endregion

        [Association("TestParameter-GroupTestMethod")]
        public XPCollection<Testparameter> Testparameters
        {
            get { return GetCollection<Testparameter>(nameof(Testparameters)); }
        }
    }
}