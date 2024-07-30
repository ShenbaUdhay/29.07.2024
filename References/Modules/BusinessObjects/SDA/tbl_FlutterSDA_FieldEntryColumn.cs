using DevExpress.Persistent.Base;
using DevExpress.Xpo;
namespace Modules.BusinessObjects.SDA
{
    [DefaultClassOptions]
    public class tbl_FlutterSDA_FieldEntryColumn : XPLiteObject
    {
        public tbl_FlutterSDA_FieldEntryColumn(Session session) : base(session) { }

        int fuqFieldEntryColumnID;
        [Key(true)]
        public int uqFieldEntryColumnID
        {
            get { return fuqFieldEntryColumnID; }
            set { SetPropertyValue<int>(nameof(uqFieldEntryColumnID), ref fuqFieldEntryColumnID, value); }
        }
        string fFieldType;
        public string FieldType
        {
            get { return fFieldType; }
            set { SetPropertyValue<string>(nameof(FieldType), ref fFieldType, value); }
        }
        string fFieldName;
        public string FieldName
        {
            get { return fFieldName; }
            set { SetPropertyValue<string>(nameof(FieldName), ref fFieldName, value); }
        }
        string fCaption_EN;
        [Size(200)]
        public string Caption_EN
        {
            get { return fCaption_EN; }
            set { SetPropertyValue<string>(nameof(Caption_EN), ref fCaption_EN, value); }
        }
        string fCaption_CN;
        [Size(200)]
        public string Caption_CN
        {
            get { return fCaption_CN; }
            set { SetPropertyValue<string>(nameof(Caption_CN), ref fCaption_CN, value); }
        }
        string fDataType;
        public string DataType
        {
            get { return fDataType; }
            set { SetPropertyValue<string>(nameof(DataType), ref fDataType, value); }
        }
        int fElement;
        public int Element
        {
            get { return fElement; }
            set { SetPropertyValue<int>(nameof(Element), ref fElement, value); }
        }
    }
}
