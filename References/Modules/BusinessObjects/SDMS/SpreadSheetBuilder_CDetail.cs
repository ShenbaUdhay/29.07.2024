using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SDMS
{
    [DefaultClassOptions]
    public class SpreadSheetBuilder_CDetail : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SpreadSheetBuilder_CDetail(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        int fuqID;
        [Key(true)]
        public int uqID
        {
            get { return fuqID; }
            set { SetPropertyValue<int>(nameof(uqID), ref fuqID, value); }
        }
        int fTemplateID;
        public int TemplateID
        {
            get { return fTemplateID; }
            set { SetPropertyValue<int>(nameof(TemplateID), ref fTemplateID, value); }
        }
        int fSheetID;
        public int SheetID
        {
            get { return fSheetID; }
            set { SetPropertyValue<int>(nameof(SheetID), ref fSheetID, value); }
        }
        string fColumnName;
        public string ColumnName
        {
            get { return fColumnName; }
            set { SetPropertyValue<string>(nameof(ColumnName), ref fColumnName, value); }
        }
        int fColumnIndex;
        public int ColumnIndex
        {
            get { return fColumnIndex; }
            set { SetPropertyValue<int>(nameof(ColumnIndex), ref fColumnIndex, value); }
        }
    }
}