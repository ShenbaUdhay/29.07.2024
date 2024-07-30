using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SDA
{
    [DefaultClassOptions]
    public class SDATemplateDetail : BaseObject
    {
        public SDATemplateDetail(Session session) : base(session) { }

        SDATemplate fTemplateID;
        [Association("SDATemplateFields")]
        public SDATemplate TemplateID
        {
            get { return fTemplateID; }
            set { SetPropertyValue<SDATemplate>(nameof(TemplateID), ref fTemplateID, value); }
        }

        int fuqFieldEntryColumnID;
        public int uqFieldEntryColumnID
        {
            get { return fuqFieldEntryColumnID; }
            set { SetPropertyValue<int>(nameof(uqFieldEntryColumnID), ref fuqFieldEntryColumnID, value); }
        }

        string fFieldName;
        public string FieldName
        {
            get { return fFieldName; }
            set { SetPropertyValue<string>(nameof(FieldName), ref fFieldName, value); }
        }

        string fFieldType;
        public string FieldType
        {
            get { return fFieldType; }
            set { SetPropertyValue<string>(nameof(FieldType), ref fFieldType, value); }
        }

        int fSort;
        public int Sort
        {
            get { return fSort; }
            set { SetPropertyValue<int>(nameof(Sort), ref fSort, value); }
        }

        string fFieldDataType;
        public string FieldDataType
        {
            get { return fFieldDataType; }
            set { SetPropertyValue<string>(nameof(FieldDataType), ref fFieldDataType, value); }
        }

        bool fISReadonly;
        public bool ISReadonly
        {
            get { return fISReadonly; }
            set { SetPropertyValue<bool>(nameof(ISReadonly), ref fISReadonly, value); }
        }

        string fCaption;
        public string Caption
        {
            get { return fCaption; }
            set { SetPropertyValue<string>(nameof(Caption), ref fCaption, value); }
        }

        bool fISDepth;
        public bool ISDepth
        {
            get { return fISDepth; }
            set { SetPropertyValue<bool>(nameof(ISDepth), ref fISDepth, value); }
        }
    }
}