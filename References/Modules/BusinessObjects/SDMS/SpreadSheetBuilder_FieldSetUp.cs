using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using Modules.BusinessObjects.Setting.SDMS;
using System;

namespace Modules.BusinessObjects.SDMS
{
    [DefaultClassOptions]
    public class SpreadSheetBuilder_FieldSetUp : XPLiteObject//BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SpreadSheetBuilder_FieldSetUp(Session session)
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
        SpreadSheetBuilder_ScientificData fFieldID;
        public SpreadSheetBuilder_ScientificData FieldID
        {
            get { return fFieldID; }
            set { SetPropertyValue<SpreadSheetBuilder_ScientificData>(nameof(FieldID), ref fFieldID, value); }
        }
        int fDataSource;
        public int DataSource
        {
            get { return fDataSource; }
            set { SetPropertyValue<int>(nameof(DataSource), ref fDataSource, value); }
        }
        string fCaption_EN;
        [Size(50)]
        public string Caption_EN
        {
            get { return fCaption_EN; }
            set { SetPropertyValue<string>(nameof(Caption_EN), ref fCaption_EN, value); }
        }
        string fCaption_CN;
        [Size(50)]
        public string Caption_CN
        {
            get { return fCaption_CN; }
            set { SetPropertyValue<string>(nameof(Caption_CN), ref fCaption_CN, value); }
        }
        int fWidth;
        public int Width
        {
            get { return fWidth; }
            set { SetPropertyValue<int>(nameof(Width), ref fWidth, value); }
        }
        bool fVisible;
        public bool Visible
        {
            get { return fVisible; }
            set { SetPropertyValue<bool>(nameof(Visible), ref fVisible, value); }
        }
        int fSort;
        public int Sort
        {
            get { return fSort; }
            set { SetPropertyValue<int>(nameof(Sort), ref fSort, value); }
        }
        string fExportSample;
        [Size(50)]
        public string ExportSample
        {
            get { return fExportSample; }
            set { SetPropertyValue<string>(nameof(ExportSample), ref fExportSample, value); }
        }
        string fExportBlank;
        [Size(50)]
        public string ExportBlank
        {
            get { return fExportBlank; }
            set { SetPropertyValue<string>(nameof(ExportBlank), ref fExportBlank, value); }
        }
        string fExportSpike;
        [Size(50)]
        public string ExportSpike
        {
            get { return fExportSpike; }
            set { SetPropertyValue<string>(nameof(ExportSpike), ref fExportSpike, value); }
        }
        string fExportStandard;
        [Size(50)]
        public string ExportStandard
        {
            get { return fExportStandard; }
            set { SetPropertyValue<string>(nameof(ExportStandard), ref fExportStandard, value); }
        }
        bool fMatchByHeader;
        [ColumnDbDefaultValue("((0))")]
        public bool MatchByHeader
        {
            get { return fMatchByHeader; }
            set { SetPropertyValue<bool>(nameof(MatchByHeader), ref fMatchByHeader, value); }
        }
        string fImportField;
        [Size(50)]
        public string ImportField
        {
            get { return fImportField; }
            set { SetPropertyValue<string>(nameof(ImportField), ref fImportField, value); }
        }
        Guid fuqTestParameterID;
        public Guid uqTestParameterID
        {
            get { return fuqTestParameterID; }
            set { SetPropertyValue<Guid>(nameof(uqTestParameterID), ref fuqTestParameterID, value); }
        }
    }
}