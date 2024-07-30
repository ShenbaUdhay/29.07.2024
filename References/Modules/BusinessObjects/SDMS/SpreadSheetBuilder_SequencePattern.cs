using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting.SDMS
{
    [DefaultProperty("uqTestMethodID")]
    public partial class SpreadSheetBuilder_SequencePattern : XPLiteObject
    {
        public SpreadSheetBuilder_SequencePattern(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

        int fuqSequencePatternID;
        [Key(true)]
        public int uqSequencePatternID
        {
            get { return fuqSequencePatternID; }
            set { SetPropertyValue<int>(nameof(uqSequencePatternID), ref fuqSequencePatternID, value); }
        }

        Matrix fuqMatrixID;
        [ImmediatePostData(true)]
        [DevExpress.Persistent.Validation.RuleRequiredField("SpreadSheetBuilder_SequencePattern.uqMatrixID", DefaultContexts.Save, "Matrix Should not be empty.")]
        public Matrix uqMatrixID
        {
            get { return fuqMatrixID; }
            set
            {
                SetPropertyValue<Matrix>(nameof(uqMatrixID), ref fuqMatrixID, value);
                uqTestMethodID = null;
                Test = null;
            }
        }

        private TestMethod _Test;
        [ImmediatePostData(true)]
        [NonPersistent]
        [DataSourceProperty("TestDataSource")]
        [RuleRequiredField("NPTest", DefaultContexts.Save)]

        public TestMethod Test
        {
            get
            {
                if (_Test == null && uqTestMethodID != null)
                {
                    _Test = uqTestMethodID;
                }
                return _Test;
            }
            set
            {
                SetPropertyValue("Test", ref _Test, value);
            }
        }

        TestMethod fuqTestMethodID;
        [ImmediatePostData(true)]
        [DataSourceProperty("MethodDataSource")]
        [DevExpress.Persistent.Validation.RuleRequiredField("SpreadSheetBuilder_SequencePattern.uqTestMethodID", DefaultContexts.Save, "Test Should not be empty.")]
        public TestMethod uqTestMethodID
        {
            get { return fuqTestMethodID; }
            set { SetPropertyValue<TestMethod>(nameof(uqTestMethodID), ref fuqTestMethodID, value); }
        }
        int fNumberOfSamplesBetweenQCTest;
        [ImmediatePostData]
        public int NumberOfSamplesBetweenQCTest
        {
            get
            {
                if (fNumberOfSamplesBetweenQCTest <= 0)
                {
                    fNumberOfSamplesBetweenQCTest = 1;
                }
                return fNumberOfSamplesBetweenQCTest;
            }
            set { SetPropertyValue<int>(nameof(NumberOfSamplesBetweenQCTest), ref fNumberOfSamplesBetweenQCTest, value); }
        }
        int fDupSamplesAfterNoOfSamples;
        [ImmediatePostData]
        public int DupSamplesAfterNoOfSamples
        {
            get
            {
                if (fDupSamplesAfterNoOfSamples <= 0)
                {
                    fDupSamplesAfterNoOfSamples = 1;
                }
                return fDupSamplesAfterNoOfSamples;
            }
            set { SetPropertyValue<int>(nameof(DupSamplesAfterNoOfSamples), ref fDupSamplesAfterNoOfSamples, value); }
        }
        int fSpikeSamplesAfterNoOfSamples;
        [ImmediatePostData]
        public int SpikeSamplesAfterNoOfSamples
        {
            get
            {
                if (fSpikeSamplesAfterNoOfSamples <= 0)
                {
                    fSpikeSamplesAfterNoOfSamples = 1;
                }
                return fSpikeSamplesAfterNoOfSamples;
            }
            set { SetPropertyValue<int>(nameof(SpikeSamplesAfterNoOfSamples), ref fSpikeSamplesAfterNoOfSamples, value); }
        }
        bool fIsClosingQC;
        public bool IsClosingQC
        {
            get { return fIsClosingQC; }
            set { SetPropertyValue<bool>(nameof(IsClosingQC), ref fIsClosingQC, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (uqMatrixID != null && Test == null)
                {
                    XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName]=? and [MethodName.GCRecord] is null", uqMatrixID.Oid));
                    List<TestMethod> lstDistinct = tests.GroupBy(i => i.TestName).Select(grp => grp.FirstOrDefault()).ToList();
                    tests = new XPCollection<TestMethod>(Session, new InOperator("Oid", lstDistinct.Select(i => i.Oid)));
                    return tests;
                }
                else
                {
                    return null;
                }
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> MethodDataSource
        {
            get
            {
                if (uqMatrixID != null && Test != null && uqTestMethodID == null)
                {
                    return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=?", Test.TestName, uqMatrixID.MatrixName));
                    //List<TestMethod> lstDistinct = tests.GroupBy(i => i.TestName).Select(grp => grp.FirstOrDefault()).ToList();
                    //tests = new XPCollection<TestMethod>(Session, new InOperator("Oid", lstDistinct.Select(i => i.Oid)));
                }
                else
                {
                    return null;
                }
            }
        }
    }
}