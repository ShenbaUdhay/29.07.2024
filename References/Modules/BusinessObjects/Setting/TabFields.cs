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
    public class TabFields : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TabFields(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region FieldID
        private string _FieldID;
        public string FieldID
        {
            get
            {
                return _FieldID;
            }
            set
            {
                SetPropertyValue<string>(nameof(FieldID), ref _FieldID, value);
            }
        }
        #endregion

        #region FieldCaption
        private string _FieldCaption;
        public string FieldCaption
        {
            get
            {
                return _FieldCaption;
            }
            set
            {
                SetPropertyValue<string>(nameof(FieldCaption), ref _FieldCaption, value);
            }
        }
        #endregion

        #region TabName
        private string _TabName;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string TabName
        {
            get { return _TabName; }
            set { SetPropertyValue(nameof(TabName), ref _TabName, value); }
        }
        #endregion
    }
}