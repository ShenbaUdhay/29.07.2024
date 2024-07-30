using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SourceOpportunity : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SourceOpportunity(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _Opportunity;
        [RuleRequiredField]
        [RuleUniqueValue]
        public string Opportunity
        {
            get { return _Opportunity; }
            set { SetPropertyValue(nameof(Opportunity), ref _Opportunity, value); }
        }

        private string _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        private string _Createdby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Createdby
        {
            get { return _Createdby; }
            set { SetPropertyValue(nameof(Createdby), ref _Createdby, value); }
        }

        private string _ModifiedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }

        private string _Modifiedby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Modifiedby
        {
            get { return _Modifiedby; }
            set { SetPropertyValue(nameof(Modifiedby), ref _Modifiedby, value); }
        }
    }
}