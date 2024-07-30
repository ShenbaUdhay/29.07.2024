using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SDMS
{
    [DefaultClassOptions]
    public class SpreadSheetBuilder_TemplateInfo : XPLiteObject
    {
        public SpreadSheetBuilder_TemplateInfo(Session session) : base(session) { }

        int fTemplateID;
        [Key(true)]
        public int TemplateID
        {
            get { return fTemplateID; }
            set { SetPropertyValue<int>(nameof(TemplateID), ref fTemplateID, value); }
        }
        string fTemplateName;
        [RuleRequiredField("TBTemplateName", DefaultContexts.Save)]
        public string TemplateName
        {
            get { return fTemplateName; }
            set { SetPropertyValue<string>(nameof(TemplateName), ref fTemplateName, value); }
        }
        byte fDTSheetID;
        public byte DTSheetID
        {
            get { return fDTSheetID; }
            set { SetPropertyValue<byte>(nameof(DTSheetID), ref fDTSheetID, value); }
        }
        bool fIsActive;
        public bool IsActive
        {
            get { return fIsActive; }
            set { SetPropertyValue<bool>(nameof(IsActive), ref fIsActive, value); }
        }
        bool fIsRetire;
        public bool IsRetire
        {
            get { return fIsRetire; }
            set { SetPropertyValue<bool>(nameof(IsRetire), ref fIsRetire, value); }
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
        byte[] fSpreadSheet_PD;
        [Size(SizeAttribute.Unlimited)]
        [MemberDesignTimeVisibility(true)]
        [EditorAlias(EditorAliases.SpreadsheetPropertyEditor)]
        [NonPersistent]
        public byte[] SpreadSheet_PD
        {
            get { return fSpreadSheet_PD; }
            set { SetPropertyValue<byte[]>(nameof(SpreadSheet_PD), ref fSpreadSheet_PD, value); }
        }
        int fCalibrationLevelNo;
        public int CalibrationLevelNo

        {
            get { return fCalibrationLevelNo; }
            set { SetPropertyValue<int>(nameof(CalibrationLevelNo), ref fCalibrationLevelNo, value); }
        }
        Employee fEnteredBy;
        public Employee EnteredBy
        {
            get { return fEnteredBy; }
            set { SetPropertyValue<Employee>(nameof(EnteredBy), ref fEnteredBy, value); }
        }
        DateTime fEnteredDate;
        public DateTime EnteredDate
        {
            get { return fEnteredDate; }
            set { SetPropertyValue<DateTime>(nameof(EnteredDate), ref fEnteredDate, value); }
        }
        Employee fModifiedBy;
        public Employee ModifiedBy
        {
            get { return fModifiedBy; }
            set { SetPropertyValue<Employee>(nameof(ModifiedBy), ref fModifiedBy, value); }
        }
        DateTime fModifiedDate;
        public DateTime ModifiedDate
        {
            get { return fModifiedDate; }
            set { SetPropertyValue<DateTime>(nameof(ModifiedDate), ref fModifiedDate, value); }
        }
        SpreadSheetBuilder_TemplateType fTemplateType;
        public SpreadSheetBuilder_TemplateType TemplateType
        {
            get { return fTemplateType; }
            set { SetPropertyValue<SpreadSheetBuilder_TemplateType>(nameof(TemplateType), ref fTemplateType, value); }
        }
        bool fIsSampling;
        public bool IsSampling
        {
            get { return fIsSampling; }
            set { SetPropertyValue<bool>(nameof(IsSampling), ref fIsSampling, value); }
        }
        bool fIsSampleBased;
        public bool IsSampleBased
        {
            get { return fIsSampleBased; }
            set { SetPropertyValue<bool>(nameof(IsSampleBased), ref fIsSampleBased, value); }
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

        bool fOrientation;
        [ColumnDbDefaultValue("((0))")]
        public bool Orientation
        {
            get { return fOrientation; }
            set { SetPropertyValue<bool>(nameof(Orientation), ref fOrientation, value); }
        }

        private EnumOrientation _SDMS = EnumOrientation.Landscape;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [NonPersistent]
        [ImmediatePostData]
        public EnumOrientation SDMS
        {
            get
            {
                return _SDMS;
            }
            set
            {
                SetPropertyValue("SDMS", ref _SDMS, value);
                if (SDMS == EnumOrientation.Vertical)
                {
                    Orientation = true;
                }
                else
                {
                    Orientation = false;
                }
            }
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

        //private Testparameter _TestParameter;
        //public Testparameter TestParameter
        //{
        //    get { return _TestParameter; }
        //    set { SetPropertyValue<Testparameter>(nameof(TestParameter), ref _TestParameter, value); }
        //}

        private string _NonPersistantTest;
        [NonPersistent]
        public string NonPersistantTest
        {

            get
            {
                if (TemplateID != 0)
                {

                    SpreadSheetBuilder_TestParameter obj = Session.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TemplateID]=?", TemplateID));
                    if (obj != null)
                    {
                        TestMethod objTestMethod = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid]=?", obj.TestMethodID));
                        if (objTestMethod != null)
                        {
                            _NonPersistantTest = objTestMethod.TestName;
                            NonPersistantMethod = objTestMethod.MethodName.MethodNumber;
                            NonPersistantMatrix = objTestMethod.MatrixName.MatrixName;
                            Category = objTestMethod.Category;
                        }
                    }
                }
                return _NonPersistantTest;
            }
            set
            {
                SetPropertyValue<string>(nameof(NonPersistantTest), ref _NonPersistantTest, value);
            }

        }

        private string _NonPersistantMethod;
        [NonPersistent]
        public string NonPersistantMethod
        {

            get
            {
                return _NonPersistantMethod;
            }
            set { SetPropertyValue<string>(nameof(NonPersistantMethod), ref _NonPersistantMethod, value); }
        }

        private string _NonPersistantMatrix;
        [NonPersistent]
        public string NonPersistantMatrix
        {

            get
            {
                return _NonPersistantMatrix;
            }
            set { SetPropertyValue<string>(nameof(NonPersistantMatrix), ref _NonPersistantMatrix, value); }

        }

        private string _NonTemplateType;
        [NonPersistent]
        public string NonTemplateType
        {

            get
            {
                if (TemplateType != null)
                {
                    SpreadSheetBuilder_TemplateType obj = Session.FindObject<SpreadSheetBuilder_TemplateType>(CriteriaOperator.Parse("[uqID]=?", TemplateType));
                    if (obj != null)
                    {
                        _NonTemplateType = obj.TemplateType;
                    }
                }
                return _NonTemplateType;
            }
            set
            {
                SetPropertyValue<string>(nameof(TemplateType), ref _NonTemplateType, value);
            }


        }

        private string _Category;
        [NonPersistent]
        public string Category
        {
            get
            {
                return _Category;
            }
            set
            {
                SetPropertyValue<string>("Category", ref _Category, value);
            }
        }

        private string _Test;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDashboards(false)]
        [RuleRequiredField("TemplateTest", DefaultContexts.Save)]
        public string Test
        {
            get
            {
                //if (TemplateID != 0)
                //{
                //    string Testname = string.Empty;
                //    XPCollection<SpreadSheetBuilder_TestParameter> stp = new XPCollection<SpreadSheetBuilder_TestParameter>(Session, CriteriaOperator.Parse("[TemplateID]=?", TemplateID));
                //    if (stp != null)
                //    {
                //        foreach (string testmethod in stp.Select(a => a.TestMethodID.ToString()).Distinct())
                //        {
                //            TestMethod TM = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] =?", new Guid(testmethod)));
                //            if (TM != null)
                //            {
                //                if (Testname == string.Empty)
                //                {
                //                    Testname = TM.TestName;
                //                }
                //                else
                //                {
                //                    Testname = Testname + "," + TM.TestName;
                //                }
                //            }
                //        }
                //        _NPTestTemplate = Testname;
                //    }
                //}
                return _Test;
            }
            set { SetPropertyValue("Test", ref _Test, value); }
        }



        private string _Source;
        public string Source
        {
            get
            {
                return _Source;
            }
            set
            {
                SetPropertyValue<string>("Source", ref _Source, value);
            }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }

    public enum EnumOrientation
    {
        Landscape = 0,
        Vertical = 1
    };

    public enum EnumMailMergeMode
    {
        SingleSheet = 0,
        MultipleSheets = 1,
        MultipleDocuments = 2
    };
}