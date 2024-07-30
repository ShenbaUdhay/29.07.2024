using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace Modules.BusinessObjects.SDMS
{
    [DefaultClassOptions]
    public partial class SpreadSheetBuilder_DataTransfer : XPLiteObject
    {
        public SpreadSheetBuilder_DataTransfer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
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
        string fName;
        [Size(50)]
        [RuleRequiredField("TBName", DefaultContexts.Save)]
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref fName, value); }
        }
        byte[] fSpreadSheet;
        [Size(SizeAttribute.Unlimited)]
        [MemberDesignTimeVisibility(true)]
        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]

        public byte[] SpreadSheet
        {
            get { return fSpreadSheet; }
            set { SetPropertyValue<byte[]>(nameof(SpreadSheet), ref fSpreadSheet, value); }
        }
        string fEnteredBy;
        [Size(SizeAttribute.Unlimited)]
        public string EnteredBy
        {
            get { return fEnteredBy; }
            set { SetPropertyValue<string>(nameof(EnteredBy), ref fEnteredBy, value); }
        }
        DateTime fEnteredDate;
        public DateTime EnteredDate
        {
            get { return fEnteredDate; }
            set { SetPropertyValue<DateTime>(nameof(EnteredDate), ref fEnteredDate, value); }
        }
        string fModifiedBy;
        [Size(SizeAttribute.Unlimited)]
        public string ModifiedBy
        {
            get { return fModifiedBy; }
            set { SetPropertyValue<string>(nameof(ModifiedBy), ref fModifiedBy, value); }
        }
        DateTime fModifiedDate;
        public DateTime ModifiedDate
        {
            get { return fModifiedDate; }
            set { SetPropertyValue<DateTime>(nameof(ModifiedDate), ref fModifiedDate, value); }
        }

        EnumMailMergeMode fMailMergeMode;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        public EnumMailMergeMode MailMergeMode
        {
            get { return fMailMergeMode; }
            set { SetPropertyValue<EnumMailMergeMode>(nameof(MailMergeMode), ref fMailMergeMode, value); }
        }
        EnumOrientation fDocumentOrientation;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        public EnumOrientation DocumentOrientation
        {
            get { return fDocumentOrientation; }
            set { SetPropertyValue<EnumOrientation>(nameof(DocumentOrientation), ref fDocumentOrientation, value); }
        }

        string fHeaderRange;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDashboards(false)]
        public string HeaderRange
        {
            get { return fHeaderRange; }
            set { SetPropertyValue<string>(nameof(HeaderRange), ref fHeaderRange, value); }
        }

        string fDetailRange;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDashboards(false)]
        public string DetailRange
        {
            get { return fDetailRange; }
            set { SetPropertyValue<string>(nameof(DetailRange), ref fDetailRange, value); }
        }

        string fCHeaderRange;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDashboards(false)]
        public string CHeaderRange
        {
            get { return fCHeaderRange; }
            set { SetPropertyValue<string>(nameof(CHeaderRange), ref fCHeaderRange, value); }
        }

        string fCDetailRange;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDashboards(false)]
        public string CDetailRange
        {
            get { return fCDetailRange; }
            set { SetPropertyValue<string>(nameof(CDetailRange), ref fCDetailRange, value); }
        }
    }
}