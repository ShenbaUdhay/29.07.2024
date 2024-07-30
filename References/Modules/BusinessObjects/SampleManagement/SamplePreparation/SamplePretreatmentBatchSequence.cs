using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SampleManagement.SamplePreparation
{
    [DefaultClassOptions]
    public class SamplePretreatmentBatchSequence : BaseObject
    {
        public SamplePretreatmentBatchSequence(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
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

        #region SampleID
        private SampleLogIn _SampleID;
        public SampleLogIn SampleID
        {
            get { return _SampleID; }
            set { SetPropertyValue("SampleID", ref _SampleID, value); }
        }
        #endregion

        #region NPSampleID
        public string NPSampleID
        {
            get
            {
                if (SampleID != null)
                {
                    return SampleID.SampleID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region SystemID
        private string _SystemID;
        public string SystemID
        {
            get { return _SystemID; }
            set { SetPropertyValue("SystemID", ref _SystemID, value); }
        }
        #endregion

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
                if (SampleID != null && SampleID.JobID != null)
                {
                    return SampleID.JobID.SampleName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region WetWeight
        private string _WetWeight;
        public string WetWeight
        {
            get
            {
                return _WetWeight;
            }
            set
            {
                SetPropertyValue<string>(nameof(WetWeight), ref _WetWeight, value);
            }
        }
        #endregion

        #region DryWeight
        private string _DryWeight;
        public string DryWeight
        {
            get
            {
                return _DryWeight;
            }
            set
            {
                SetPropertyValue<string>(nameof(DryWeight), ref _DryWeight, value);
            }
        }
        #endregion

        #region ParticleSize
        private string _ParticleSize;
        public string ParticleSize
        {
            get
            {
                return _ParticleSize;
            }
            set
            {
                SetPropertyValue<string>(nameof(ParticleSize), ref _ParticleSize, value);
            }
        }
        #endregion

        #region DryingTemp
        private string _DryingTemp;
        public string DryingTemp
        {
            get
            {
                return _DryingTemp;
            }
            set
            {
                SetPropertyValue<string>(nameof(DryingTemp), ref _DryingTemp, value);
            }
        }
        #endregion

        #region TimeIn
        private DateTime _TimeIn;
        public DateTime TimeIn
        {
            get
            {
                return _TimeIn;
            }
            set
            {
                SetPropertyValue<DateTime>(nameof(TimeIn), ref _TimeIn, value);
            }
        }
        #endregion

        #region TimeOut
        private DateTime _TimeOut;
        public DateTime TimeOut
        {
            get
            {
                return _TimeOut;
            }
            set
            {
                SetPropertyValue<DateTime>(nameof(TimeOut), ref _TimeOut, value);
            }
        }
        #endregion

        #region DryingTimeDisplay
        [NonPersistent]
        public string DryingTimeDisplay
        {
            get { return ConvertStoredValueToText(DryingTime); ; }
            set { DryingTime = ConverTextToStoredValue(value); }
        }
        #endregion

        private TimeSpan? ConverTextToStoredValue(string value)
        {
            if (value != null)
            {
                TimeSpan? time = TimeSpan.Parse(value);
                return time;
            }
            return TimeSpan.Zero;
        }

        private string ConvertStoredValueToText(TimeSpan? dryingTime)
        {
            if (dryingTime != null)
            {

                string strTime = dryingTime.Value.ToString(@"hh\:mm");
                return strTime;
            }
            return null;
        }

        #region DryingTime
        private TimeSpan? _DryingTime;
        public TimeSpan? DryingTime
        {
            get { return _DryingTime; }
            set { SetPropertyValue(nameof(DryingTime), ref _DryingTime, value); }
        }
        #endregion

        #region ContainerID
        public string ContainerID
        {
            get
            {
                return string.Empty;
            }
        }
        #endregion

        #region StorageID
        private Storage _StorageID;
        public Storage StorageID
        {
            get { return _StorageID; }
            set { SetPropertyValue<Storage>(nameof(StorageID), ref _StorageID, value); }
        }
        #endregion

        #region StorageCondition
        private string _StorageCondition;
        public string StorageCondition
        {
            get { return _StorageCondition; }
            set { SetPropertyValue<string>(nameof(StorageCondition), ref _StorageCondition, value); }
        }
        #endregion

        #region Test
        private string _Test;
        public string Test
        {
            get
            {
                return _Test;
            }
            set
            {
                SetPropertyValue<string>(nameof(Test), ref _Test, value);
            }
        }
        #endregion

        #region SampleAmount
        private string _SampleAmount;
        public string SampleAmount
        {
            get { return _SampleAmount; }
            set { SetPropertyValue(nameof(SampleAmount), ref _SampleAmount, value); }
        }
        #endregion

        #region SampleUnits
        private string _SampleUnits;
        public string SampleUnits
        {
            get { return _SampleUnits; }
            set { SetPropertyValue(nameof(SampleUnits), ref _SampleUnits, value); }
        }
        #endregion

        #region ZHE_ID
        private string _ZHEID;
        public string ZHE_ID
        {
            get { return _ZHEID; }
            set { SetPropertyValue(nameof(ZHE_ID), ref _ZHEID, value); }
        }
        #endregion

        #region SolidPercentage
        private string _SolidPercentage;
        public string SolidPercentage
        {
            get { return _SolidPercentage; }
            set { SetPropertyValue(nameof(SolidPercentage), ref _SolidPercentage, value); }
        }
        #endregion

        #region Initial_pH
        private string _InitialpH;
        public string Initial_pH
        {
            get { return _InitialpH; }
            set { SetPropertyValue(nameof(Initial_pH), ref _InitialpH, value); }
        }
        #endregion

        #region Final_pH
        private string _FinalpH;
        public string Final_pH
        {
            get { return _FinalpH; }
            set { SetPropertyValue(nameof(Final_pH), ref _FinalpH, value); }
        }
        #endregion

        #region Fluid#1_ID
        private string _Fluid1ID;
        public string Fluid1_ID
        {
            get { return _Fluid1ID; }
            set { SetPropertyValue(nameof(Fluid1_ID), ref _Fluid1ID, value); }
        }
        #endregion

        #region Fluid#2_ID
        private string _Fluid2ID;
        public string Fluid2_ID
        {
            get { return _Fluid2ID; }
            set { SetPropertyValue(nameof(Fluid2_ID), ref _Fluid2ID, value); }
        }
        #endregion

        #region FluidVol
        private string _FluidVol;
        public string FluidVol
        {
            get { return _FluidVol; }
            set { SetPropertyValue<string>(nameof(FluidVol), ref _FluidVol, value); }
        }
        #endregion

        #region DateTimeStarted
        private DateTime _DateTimeStarted;
        public DateTime DateTimeStarted
        {
            get
            {
                return _DateTimeStarted;
            }
            set
            {
                SetPropertyValue<DateTime>(nameof(DateTimeStarted), ref _DateTimeStarted, value);
            }
        }
        #endregion

        #region DateTimeEnded
        private DateTime _DateTimeEnded;
        public DateTime DateTimeEnded
        {
            get
            {
                return _DateTimeEnded;
            }
            set
            {
                SetPropertyValue<DateTime>(nameof(DateTimeEnded), ref _DateTimeEnded, value);
            }
        }
        #endregion

        #region TempMin
        private string _TempMin;
        public string TempMin
        {
            get { return _TempMin; }
            set { SetPropertyValue<string>(nameof(TempMin), ref _TempMin, value); }
        }
        #endregion

        #region TempMax
        private string _TempMax;
        public string TempMax
        {
            get { return _TempMax; }
            set { SetPropertyValue<string>(nameof(TempMax), ref _TempMax, value); }
        }
        #endregion

        #region RPM
        private string _RPM;
        public string RPM
        {
            get
            {
                return _RPM;
            }
            set
            {
                SetPropertyValue<string>(nameof(RPM), ref _RPM, value);
            }
        }
        #endregion

        #region TumblerID
        private string _TumblerID;
        public string TumblerID
        {
            get
            {
                return _TumblerID;
            }
            set
            {
                SetPropertyValue<string>(nameof(TumblerID), ref _TumblerID, value);
            }
        }
        #endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        #region SamplePretreatmentBatchlink       
        private SamplePretreatmentBatch _SamplePretreatmentBatchDetail;
        [Association("SamplePretreatmentBatchlink")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public SamplePretreatmentBatch SamplePretreatmentBatchDetail
        {
            get { return _SamplePretreatmentBatchDetail; }
            set { SetPropertyValue(nameof(SamplePretreatmentBatchDetail), ref _SamplePretreatmentBatchDetail, value); }
        }
        #endregion

    }
}