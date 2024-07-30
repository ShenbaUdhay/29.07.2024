using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TurnAroundTime : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TurnAroundTime(Session session)
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


        //private int _NPTAT;
        //[RuleUniqueValue]
        //[RuleRequiredField("Enter the TAT", DefaultContexts.Save)]
        //[NonPersistent]
        //[ImmediatePostData]
        //[VisibleInDetailView(false),VisibleInListView(false),VisibleInLookupListView(false)]
        //public int NPTAT
        //{
        //    get { return _NPTAT; }
        //    set { SetPropertyValue("NPTAT", ref _NPTAT, value); }
        //}

        private String _TAT;
        [RuleUniqueValue]
        [ImmediatePostData]
        [RuleRequiredField("Enter the TAT", DefaultContexts.Save)]
        public String TAT
        {
            get { return _TAT; }
            set { SetPropertyValue("TAT", ref _TAT, value); }
        }



        private String _LabTAT;

        public String LabTAT
        {
            get { return _LabTAT; }
            set { SetPropertyValue("LabTAT", ref _LabTAT, value); }
        }


        private int _Count;

        public int Count
        {
            get { return _Count; }
            set { SetPropertyValue("Count", ref _Count, value); }
        }

        private int _Sort;
        [RuleValueComparison("valSort", DefaultContexts.Save, ValueComparisonType.GreaterThan, -1)]
        public int Sort
        {
            get { return _Sort; }
            set { SetPropertyValue(nameof(Sort), ref _Sort, value); }
        }

        private bool _Default;

        public bool Default
        {
            get { return _Default; }
            set { SetPropertyValue("Default", ref _Default, value); }
        }

        private string _Comment;

        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }
        }



    }
}