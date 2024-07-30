using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SDMS
{
    [DefaultClassOptions]
    public class SpreadSheetBuilder_CHeader : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SpreadSheetBuilder_CHeader(Session session)
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
        string fCaption;
        public string Caption
        {
            get { return fCaption; }
            set { SetPropertyValue<string>(nameof(Caption), ref fCaption, value); }
        }
        string fPosition;
        [Size(50)]
        public string Position
        {
            get { return fPosition; }
            set { SetPropertyValue<string>(nameof(Position), ref fPosition, value); }
        }

    }
}