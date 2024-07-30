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
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).

    public class Preservative : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Preservative(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);

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

        #region PreservativeName
        private string _PreservativeName;
        //[RuleRequiredField((string id, string targetContextIDs, string messageTemplate))]

        [RuleRequiredField("Preservative.PreservativeName", DefaultContexts.Save, "Preservative must not be empty")]
        [RuleUniqueValue]
        public string PreservativeName
        {
            get { return _PreservativeName; }
            set { SetPropertyValue("PreservativeName", ref _PreservativeName, value); }
        }
        #endregion
        #region PreservativeCode
        private string _PreservativeCode;
        public string PreservativeCode
        {
            get { return _PreservativeCode; }
            set { SetPropertyValue("PreservativeCode", ref _PreservativeCode, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        [Size(1000)]
        public String Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }

        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        // [Appearance("Preservative.ModifiedDate", Enabled = false, Context = "DetailView")]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }

        }
        #endregion

        #region ModifiedBy
        private CustomSystemUser _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Appearance("Preservative.ModifiedBy", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue("ModifiedBy", ref _ModifiedBy, value); }

        }
        #endregion

        #region collection
        //private TestMethod _TestMethods;
        //[Association(@"TestMethodReferencesPreservative")]
        //public TestMethod TestMethods
        //{
        //    get
        //    {
        //       return _TestMethods;
        //    }
        //    set
        //    {
        //        SetPropertyValue<TestMethod>("TestMethods", ref _TestMethods, value);
        //    }
        //}
        #endregion
        #region Container
        private Container _Container;
        [VisibleInListView(false)]
        public Container Container
        {
            get { return _Container; }
            set { SetPropertyValue("Container", ref _Container, value); }
        }
        #endregion
        #region Temperature
        private String _Temperature;
        [VisibleInListView(false)]
        public String Temperature
        {
            get { return _Temperature; }
            set { SetPropertyValue("Temperature", ref _Temperature, value); }
        }
        #endregion
        private TestMethod _TestMethod;
        [Association("TestMethod-Preservative")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public TestMethod TestMethod
        {
            get { return _TestMethod; }
            set { SetPropertyValue(nameof(TestMethod), ref _TestMethod, value); }
        }

    }
}