using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    public class NPSamplingFieldConfiguration : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public NPSamplingFieldConfiguration(Session session)
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
    }
}