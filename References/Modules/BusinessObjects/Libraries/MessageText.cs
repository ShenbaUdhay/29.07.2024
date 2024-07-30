using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Libraries
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class MessageText : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public MessageText(Session session)
            : base(session)
        {
        }

        [ModelDefault("Caption", " ")]
        [Size(SizeAttribute.Unlimited)]
        public string Message
        {
            get { return DataValue; }
        }

        #region 条件值
        private string _dataValue;
        /// <summary>
        /// 条件值：
        /// 所选的项
        /// </summary>
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string DataValue
        {
            get
            {
                return _dataValue;
            }
            set
            {
                SetPropertyValue("DataValue", ref _dataValue, value);
            }
        }
        #endregion
    }
}