using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;

namespace Modules.BusinessObjects.QC
{
    public enum PLMDataEnterStatus
    {
        PendingResultEntry,
        PendingSamplePrep,
        Entered
    }

    [DefaultClassOptions]
    //[Appearance("showPLMItems", AppearanceItemType = "ViewItem", Context = "ListView", TargetItems = "LayerCount;StrSampleL;", Criteria = "[IsPLMTest] = True",Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    //[Appearance("hidePLMItems", AppearanceItemType = "ViewItem", Context = "ListView", TargetItems = "LayerCount;StrSampleL;", Criteria = "[IsPLMTest] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    public class QCBatchSequence : BaseObject
    {
        public QCBatchSequence(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DilutionCount = 1;
            LayerCount = 1;
        }

        private QCType _QCType;
        public QCType QCType
        {
            get { return _QCType; }
            set { SetPropertyValue("QCType", ref _QCType, value); }
        }

        private int _Sort;
        public int Sort
        {
            get { return _Sort; }
            set { SetPropertyValue("Sort", ref _Sort, value); }
        }

        private int _Runno;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Runno
        {
            get { return _Runno; }
            set { SetPropertyValue("Runno", ref _Runno, value); }
        }

        private string _StrSampleID;
        public string StrSampleID
        {
            get { return _StrSampleID; }
            set { SetPropertyValue("StrSampleID", ref _StrSampleID, value); }
        }

        private SampleLogIn _SampleID;
        public SampleLogIn SampleID
        {
            get { return _SampleID; }
            set { SetPropertyValue("SampleID", ref _SampleID, value); }
        }

        private string _SystemID;
        public string SystemID
        {
            get { return _SystemID; }
            set { SetPropertyValue("SystemID", ref _SystemID, value); }
        }

        private string _SYSSamplecode;
        public string SYSSamplecode
        {
            get { return _SYSSamplecode; }
            set { SetPropertyValue("SYSSamplecode", ref _SYSSamplecode, value); }
        }

        private string _SampleAmount;
        public string SampleAmount
        {
            get { return _SampleAmount; }
            set { SetPropertyValue("SampleAmount", ref _SampleAmount, value); }
        }
        private string _FinalVolume;
        public string FinalVolume
        {
            get { return _FinalVolume; }
            set { SetPropertyValue("FinalVolume", ref _FinalVolume, value); }
        }
        private string _Multiplier;
        public string Multiplier
        {
            get { return _Multiplier; }
            set { SetPropertyValue("Multiplier", ref _Multiplier, value); }
        }
        private string _HoldTime;
        public string HoldTime
        {
            get { return _HoldTime; }
            set { SetPropertyValue("HoldTime", ref _HoldTime, value); }
        }
        #region QClink       
        private SpreadSheetEntry_AnalyticalBatch fqcseqdetail;
        [Association("QClink")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public SpreadSheetEntry_AnalyticalBatch qcseqdetail
        {
            get { return fqcseqdetail; }
            set { SetPropertyValue("qcseqdetail", ref fqcseqdetail, value); }
        }
        #endregion

        private int _batchno;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int batchno
        {
            get { return _batchno; }
            set { SetPropertyValue("batchno", ref _batchno, value); }
        }

        private uint _DilutionCount;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[ImmediatePostData]
        public uint DilutionCount
        {
            get { return _DilutionCount; }
            set { SetPropertyValue("DilutionCount", ref _DilutionCount, value); }
        }

        private string _Dilution;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[ImmediatePostData]
        public string Dilution
        {
            get { return _Dilution; }
            set { SetPropertyValue("Dilution", ref _Dilution, value); }
        }
        private bool _IsDilution;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[ImmediatePostData]
        public bool IsDilution
        {
            get { return _IsDilution; }
            set { SetPropertyValue("IsDilution", ref _IsDilution, value); }
        }

        private bool _IsReport;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[ImmediatePostData]
        public bool IsReport
        {
            get { return _IsReport; }
            set { SetPropertyValue("IsReport", ref _IsReport, value); }
        }
        #region #SampleL#
        private string _StrSampleL;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [XafDisplayName("SampleL#")]
        public string StrSampleL
        {
            get { return _StrSampleL; }
            set { SetPropertyValue("StrSampleL", ref _StrSampleL, value); }
        }
        #endregion
        #region LayerCount
        private uint _LayerCount;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[RuleValueComparison("valLayerCount", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0)]
        public uint LayerCount
        {
            get { return _LayerCount; }
            set { SetPropertyValue("LayerCount", ref _LayerCount, value); }
        }
        #endregion
        #region IsPLMTest
        private bool _IsPLMTest;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsPLMTest
        {
            get
            {
                if (fqcseqdetail != null && fqcseqdetail.Test != null && fqcseqdetail.Test.TestName.StartsWith("PLM"))
                {
                    _IsPLMTest = true;
                }
                else
                {
                    _IsPLMTest = false;
                }
                return _IsPLMTest;
            }
        }
        #endregion
        #region ABID
        private SpreadSheetEntry_AnalyticalBatch _UQABID;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public SpreadSheetEntry_AnalyticalBatch UQABID
        {
            get { return _UQABID; }
            set { SetPropertyValue("UQABID", ref _UQABID, value); }
        }
        #endregion


        #region Status
        private PLMDataEnterStatus _Status;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public PLMDataEnterStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue("Status", ref _Status, value); }
        }
        #endregion


        #region
        private string _JOBID;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [Size(int.MaxValue)]
        public string JOBID
        {
            get { return _JOBID; }
            set { SetPropertyValue("JOBID", ref _JOBID, value); }
        }
        #endregion

        private string _SampleName;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public string SampleName
        {
            get
            {
                if (SampleID != null && QCType != null && QCType.QCTypeName == "Sample")
                {
                    _SampleName = SampleID.ClientSampleID;
                }
                return _SampleName;
            }

        }
    }
}