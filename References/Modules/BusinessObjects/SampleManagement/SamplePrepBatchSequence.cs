using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Setting;

namespace Modules.BusinessObjects.SampleManagement.SamplePreparation
{
    [DefaultClassOptions]
    public class SamplePrepBatchSequence : BaseObject
    {
        public SamplePrepBatchSequence(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            DilutionCount = 1;
        }


        #region QCType
        private QCType _QCType;
        public QCType QCType
        {
            get { return _QCType; }
            set { SetPropertyValue("QCType", ref _QCType, value); }
        }
        #endregion

        #region Sort
        private int _Sort;
        public int Sort
        {
            get { return _Sort; }
            set { SetPropertyValue("Sort", ref _Sort, value); }
        }
        #endregion

        private string _StrSampleID;
        public string StrSampleID
        {
            get { return _StrSampleID; }
            set { SetPropertyValue("StrSampleID", ref _StrSampleID, value); }
        }

        #region SampleID
        private SampleLogIn _SampleID;
        public SampleLogIn SampleID
        {
            get { return _SampleID; }
            set { SetPropertyValue("SampleID", ref _SampleID, value); }
        }
        #endregion

        private string _SystemID;
        public string SystemID
        {
            get { return _SystemID; }
            set { SetPropertyValue("SystemID", ref _SystemID, value); }
        }

        #region SYSSamplecode
        private string _SYSSamplecode;
        public string SYSSamplecode
        {
            get { return _SYSSamplecode; }
            set { SetPropertyValue("SYSSamplecode", ref _SYSSamplecode, value); }
        }
        #endregion

        #region SampleName
        [NonPersistent]
        public string SampleName
        {
            get
            {
                if (SampleID != null && SampleID.JobID != null && QCType != null && QCType.QCTypeName == "Sample")
                {
                    return SampleID.ClientSampleID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region SampleAmount
        private string _SampleAmount;
        public string SampleAmount
        {
            get
            {
                if (_SampleAmount == null)
                {
                    if (SampleID != null && SampleID.JobID != null && QCType != null && QCType.QCTypeName == "Sample")
                    {
                        return _SampleAmount = SampleID.SampleAmount;
                    }
                    else
                    {
                        return _SampleAmount;
                    }
                }
                else
                {
                    return _SampleAmount;
                }
            }
            set { SetPropertyValue(nameof(SampleAmount), ref _SampleAmount, value); }
        }
        #endregion

        #region FinalVolume
        private string _FinalVolume;
        public string FinalVolume
        {
            get { return _FinalVolume; }
            set { SetPropertyValue(nameof(FinalVolume), ref _FinalVolume, value); }
        }
        #endregion

        #region Multiplier
        private string _Multiplier;
        public string Multiplier
        {
            get { return _Multiplier; }
            set { SetPropertyValue(nameof(Multiplier), ref _Multiplier, value); }
        }
        #endregion

        #region HoldTime
        private string _HoldTime;
        public string HoldTime
        {
            get { return _HoldTime; }
            set { SetPropertyValue(nameof(HoldTime), ref _HoldTime, value); }
        }
        #endregion

        #region TakenSampleUnit
        private string _TakenSampleUnit;
        public string TakenSampleUnit
        {
            get { return _TakenSampleUnit; }
            set { SetPropertyValue(nameof(TakenSampleUnit), ref _TakenSampleUnit, value); }
        }
        #endregion

        #region SamplePrepBatchlink       
        private SamplePrepBatch _SamplePrepBatchDetail;
        [Association("SamplePrepBatchlink")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public SamplePrepBatch SamplePrepBatchDetail
        {
            get { return _SamplePrepBatchDetail; }
            set { SetPropertyValue(nameof(SamplePrepBatchDetail), ref _SamplePrepBatchDetail, value); }
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
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public uint DilutionCount
        {
            get { return _DilutionCount; }
            set { SetPropertyValue("DilutionCount", ref _DilutionCount, value); }
        }

        private string _DF;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string DF
        {
            get { return _DF; }
            set { SetPropertyValue("DF", ref _DF, value); }
        }
        #region IsDilution
        private bool _IsDilution;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public bool IsDilution
        {
            get { return _IsDilution; }
            set { SetPropertyValue("IsDilution", ref _IsDilution, value); }
        }
        #endregion
        #region PrepMethod
        private PrepMethod _PrepMethod;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public PrepMethod PrepMethod
        {
            get { return _PrepMethod; }
            set { SetPropertyValue("PrepMethod", ref _PrepMethod, value); }
        }
        #endregion
        #region Units
        private Unit _Units;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Unit Units
        {
            get { return _Units; }
            set { SetPropertyValue("Units", ref _Units, value); }
        }
        #endregion
        #region JobID
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string JobID
        {
            get
            {
                if (SampleID != null && SampleID.JobID != null && QCType != null && QCType.QCTypeName == "Sample")
                {
                    return SampleID.JobID.JobID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }
}