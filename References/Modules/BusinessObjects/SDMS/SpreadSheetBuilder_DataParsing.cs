using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using Modules.BusinessObjects.Setting.SDMS;
using System;

namespace Modules.BusinessObjects.SDMS
{
    [DefaultClassOptions]
    public class SpreadSheetBuilder_DataParsing : XPLiteObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SpreadSheetBuilder_DataParsing(Session session)
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
        SpreadSheetBuilder_TemplateInfo fTemplateID;
        public SpreadSheetBuilder_TemplateInfo TemplateID
        {
            get { return fTemplateID; }
            set { SetPropertyValue<SpreadSheetBuilder_TemplateInfo>(nameof(TemplateID), ref fTemplateID, value); }
        }
        int fSheetID;
        public int SheetID
        {
            get { return fSheetID; }
            set { SetPropertyValue<int>(nameof(SheetID), ref fSheetID, value); }
        }
        int fFieldID;
        public int FieldID
        {
            get { return fFieldID; }
            set { SetPropertyValue<int>(nameof(FieldID), ref fFieldID, value); }
        }
        string fPosition;
        [Size(50)]
        public string Position
        {
            get { return fPosition; }
            set { SetPropertyValue<string>(nameof(Position), ref fPosition, value); }
        }
        string fFormula;
        [Size(500)]
        public string Formula
        {
            get { return fFormula; }
            set { SetPropertyValue<string>(nameof(Formula), ref fFormula, value); }
        }
        string fFormat;
        [Size(50)]
        public string Format
        {
            get { return fFormat; }
            set { SetPropertyValue<string>(nameof(Format), ref fFormat, value); }
        }
        bool fContinuous;
        public bool Continuous
        {
            get { return fContinuous; }
            set { SetPropertyValue<bool>(nameof(Continuous), ref fContinuous, value); }
        }
        bool fRead;
        public bool Read
        {
            get { return fRead; }
            set { SetPropertyValue<bool>(nameof(Read), ref fRead, value); }
        }
        bool fWrite;
        public bool Write
        {
            get { return fWrite; }
            set { SetPropertyValue<bool>(nameof(Write), ref fWrite, value); }
        }
        Guid fParameterID;
        public Guid ParameterID
        {
            get { return fParameterID; }
            set { SetPropertyValue<Guid>(nameof(ParameterID), ref fParameterID, value); }
        }
        string fCaption;
        [Size(50)]
        public string Caption
        {
            get { return fCaption; }
            set { SetPropertyValue<string>(nameof(Caption), ref fCaption, value); }
        }

        private string _fFieldName;
        [NonPersistent]
        public string FieldName
        {
            get
            {
                if (FieldID != 0)
                {
                    SpreadSheetBuilder_ScientificData obj = Session.FindObject<SpreadSheetBuilder_ScientificData>(CriteriaOperator.Parse("[uqID]=?", FieldID));
                    if (obj != null)
                    {
                        _fFieldName = obj.FieldName;
                    }
                }
                return _fFieldName;
            }
            set
            {
                SetPropertyValue<string>(nameof(FieldName), ref _fFieldName, value);
            }
        }

        private ResultType _RunType;
        [NonPersistent]
        public ResultType RunType
        {
            get
            {
                return _RunType;
            }
            set
            {
                SetPropertyValue("RunType", ref _RunType, value);
            }
        }
    }

    public enum ResultType
    {
        RawDataTable = 0,
        CalibrationTable = 1
    }
}