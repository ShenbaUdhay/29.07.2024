using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [NonPersistent]
    public class NPSampleFields : BaseObject
    {
        public NPSampleFields(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
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