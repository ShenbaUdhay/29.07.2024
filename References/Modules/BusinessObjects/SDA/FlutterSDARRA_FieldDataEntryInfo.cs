using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
namespace Modules.BusinessObjects.AlpacaLims
{
    [DefaultClassOptions]
    public partial class FlutterSDARRA_FieldDataEntryInfo : XPLiteObject
    {
        public FlutterSDARRA_FieldDataEntryInfo(Session session) : base(session) { }

        long fuqFieldDataEntryInfoID;
        [Key(true)]
        public long uqFieldDataEntryInfoID
        {
            get { return fuqFieldDataEntryInfoID; }
            set { SetPropertyValue<long>(nameof(uqFieldDataEntryInfoID), ref fuqFieldDataEntryInfoID, value); }
        }
        Guid fuqStationID;
        public Guid uqStationID
        {
            get { return fuqStationID; }
            set { SetPropertyValue<Guid>(nameof(uqStationID), ref fuqStationID, value); }
        }
        long fuqSamplingElementID;
        public long uqSamplingElementID
        {
            get { return fuqSamplingElementID; }
            set { SetPropertyValue<long>(nameof(uqSamplingElementID), ref fuqSamplingElementID, value); }
        }
        Guid fuqSampleID;
        public Guid uqSampleID
        {
            get { return fuqSampleID; }
            set { SetPropertyValue<Guid>(nameof(uqSampleID), ref fuqSampleID, value); }
        }
        string fAirTemperature_Fahrenheit_;
        [Size(50)]
        [Persistent(@"AirTemperature(Fahrenheit)")]
        public string AirTemperature_Fahrenheit_
        {
            get { return fAirTemperature_Fahrenheit_; }
            set { SetPropertyValue<string>(nameof(AirTemperature_Fahrenheit_), ref fAirTemperature_Fahrenheit_, value); }
        }
        int fBasin;
        public int Basin
        {
            get { return fBasin; }
            set { SetPropertyValue<int>(nameof(Basin), ref fBasin, value); }
        }
        string fChainofCustody;
        [Size(50)]
        public string ChainofCustody
        {
            get { return fChainofCustody; }
            set { SetPropertyValue<string>(nameof(ChainofCustody), ref fChainofCustody, value); }
        }
        string fClientName;
        [Size(150)]
        public string ClientName
        {
            get { return fClientName; }
            set { SetPropertyValue<string>(nameof(ClientName), ref fClientName, value); }
        }
        string fClientSampleID;
        public string ClientSampleID
        {
            get { return fClientSampleID; }
            set { SetPropertyValue<string>(nameof(ClientSampleID), ref fClientSampleID, value); }
        }
        string fCOCLaboratoryID;
        [Size(50)]
        public string COCLaboratoryID
        {
            get { return fCOCLaboratoryID; }
            set { SetPropertyValue<string>(nameof(COCLaboratoryID), ref fCOCLaboratoryID, value); }
        }
        Guid? fCollectedBy;
        public Guid? CollectedBy
        {
            get { return fCollectedBy; }
            set { SetPropertyValue<Guid?>(nameof(CollectedBy), ref fCollectedBy, value); }
        }
        DateTime fCollectedDate;
        public DateTime CollectedDate
        {
            get { return fCollectedDate; }
            set { SetPropertyValue<DateTime>(nameof(CollectedDate), ref fCollectedDate, value); }
        }
        bool fCollectedMultiDepthSamples;
        public bool CollectedMultiDepthSamples
        {
            get { return fCollectedMultiDepthSamples; }
            set { SetPropertyValue<bool>(nameof(CollectedMultiDepthSamples), ref fCollectedMultiDepthSamples, value); }
        }
        string fConductivity_uS_cm_;
        [Persistent(@"Conductivity(uS/cm)")]
        public string Conductivity_uS_cm_
        {
            get { return fConductivity_uS_cm_; }
            set { SetPropertyValue<string>(nameof(Conductivity_uS_cm_), ref fConductivity_uS_cm_, value); }
        }
        string fCounty;
        public string County
        {
            get { return fCounty; }
            set { SetPropertyValue<string>(nameof(County), ref fCounty, value); }
        }
        string fDE;
        [Size(50)]
        public string DE
        {
            get { return fDE; }
            set { SetPropertyValue<string>(nameof(DE), ref fDE, value); }
        }
        string fDepthBottomofWaterBody_m_;
        [Persistent(@"DepthBottomofWaterBody(m)")]
        public string DepthBottomofWaterBody_m_
        {
            get { return fDepthBottomofWaterBody_m_; }
            set { SetPropertyValue<string>(nameof(DepthBottomofWaterBody_m_), ref fDepthBottomofWaterBody_m_, value); }
        }
        string fDissolvedOxygen_mg_L_;
        [Persistent(@"DissolvedOxygen(mg/L)")]
        public string DissolvedOxygen_mg_L_
        {
            get { return fDissolvedOxygen_mg_L_; }
            set { SetPropertyValue<string>(nameof(DissolvedOxygen_mg_L_), ref fDissolvedOxygen_mg_L_, value); }
        }
        string fElementName;
        public string ElementName
        {
            get { return fElementName; }
            set { SetPropertyValue<string>(nameof(ElementName), ref fElementName, value); }
        }
        DateTime fEventDate;
        public DateTime EventDate
        {
            get { return fEventDate; }
            set { SetPropertyValue<DateTime>(nameof(EventDate), ref fEventDate, value); }
        }
        string fFieldTestSummary;
        [Size(SizeAttribute.Unlimited)]
        public string FieldTestSummary
        {
            get { return fFieldTestSummary; }
            set { SetPropertyValue<string>(nameof(FieldTestSummary), ref fFieldTestSummary, value); }
        }
        string fFlow_CFS_;
        [Persistent(@"Flow(CFS)")]
        public string Flow_CFS_
        {
            get { return fFlow_CFS_; }
            set { SetPropertyValue<string>(nameof(Flow_CFS_), ref fFlow_CFS_, value); }
        }
        string fFlowEstimate_CFS_;
        [Size(50)]
        [Persistent(@"FlowEstimate(CFS)")]
        public string FlowEstimate_CFS_
        {
            get { return fFlowEstimate_CFS_; }
            set { SetPropertyValue<string>(nameof(FlowEstimate_CFS_), ref fFlowEstimate_CFS_, value); }
        }
        string fFlowMeasurementMethods;
        public string FlowMeasurementMethods
        {
            get { return fFlowMeasurementMethods; }
            set { SetPropertyValue<string>(nameof(FlowMeasurementMethods), ref fFlowMeasurementMethods, value); }
        }
        string fFlowSeverity;
        [Size(50)]
        public string FlowSeverity
        {
            get { return fFlowSeverity; }
            set { SetPropertyValue<string>(nameof(FlowSeverity), ref fFlowSeverity, value); }
        }
        string fHUANo;
        public string HUANo
        {
            get { return fHUANo; }
            set { SetPropertyValue<string>(nameof(HUANo), ref fHUANo, value); }
        }
        string fJobID;
        [Size(50)]
        public string JobID
        {
            get { return fJobID; }
            set { SetPropertyValue<string>(nameof(JobID), ref fJobID, value); }
        }
        string fMonitoringType;
        [Size(50)]
        public string MonitoringType
        {
            get { return fMonitoringType; }
            set { SetPropertyValue<string>(nameof(MonitoringType), ref fMonitoringType, value); }
        }
        string fNonFieldTestSummary;
        [Size(SizeAttribute.Unlimited)]
        public string NonFieldTestSummary
        {
            get { return fNonFieldTestSummary; }
            set { SetPropertyValue<string>(nameof(NonFieldTestSummary), ref fNonFieldTestSummary, value); }
        }
        string fpH_StandardUnits_;
        [Persistent(@"pH(StandardUnits)")]
        public string pH_StandardUnits_
        {
            get { return fpH_StandardUnits_; }
            set { SetPropertyValue<string>(nameof(pH_StandardUnits_), ref fpH_StandardUnits_, value); }
        }
        string fProjectID;
        [Size(150)]
        public string ProjectID
        {
            get { return fProjectID; }
            set { SetPropertyValue<string>(nameof(ProjectID), ref fProjectID, value); }
        }
        string fProjectName;
        [Size(500)]
        public string ProjectName
        {
            get { return fProjectName; }
            set { SetPropertyValue<string>(nameof(ProjectName), ref fProjectName, value); }
        }
        string fQAO;
        [Size(50)]
        public string QAO
        {
            get { return fQAO; }
            set { SetPropertyValue<string>(nameof(QAO), ref fQAO, value); }
        }
        string fReach;
        public string Reach
        {
            get { return fReach; }
            set { SetPropertyValue<string>(nameof(Reach), ref fReach, value); }
        }
        string fRegistrationID;
        [Size(50)]
        public string RegistrationID
        {
            get { return fRegistrationID; }
            set { SetPropertyValue<string>(nameof(RegistrationID), ref fRegistrationID, value); }
        }
        string fReservoirAccessNotPossible;
        public string ReservoirAccessNotPossible
        {
            get { return fReservoirAccessNotPossible; }
            set { SetPropertyValue<string>(nameof(ReservoirAccessNotPossible), ref fReservoirAccessNotPossible, value); }
        }
        string fReservoirPercentFull;
        public string ReservoirPercentFull
        {
            get { return fReservoirPercentFull; }
            set { SetPropertyValue<string>(nameof(ReservoirPercentFull), ref fReservoirPercentFull, value); }
        }
        string fReservoirStage;
        public string ReservoirStage
        {
            get { return fReservoirStage; }
            set { SetPropertyValue<string>(nameof(ReservoirStage), ref fReservoirStage, value); }
        }
        string fReservoirStorage;
        public string ReservoirStorage
        {
            get { return fReservoirStorage; }
            set { SetPropertyValue<string>(nameof(ReservoirStorage), ref fReservoirStorage, value); }
        }
        string fRRALaboratoryID;
        [Size(50)]
        public string RRALaboratoryID
        {
            get { return fRRALaboratoryID; }
            set { SetPropertyValue<string>(nameof(RRALaboratoryID), ref fRRALaboratoryID, value); }
        }
        string fRRATagNo;
        [Size(50)]
        public string RRATagNo
        {
            get { return fRRATagNo; }
            set { SetPropertyValue<string>(nameof(RRATagNo), ref fRRATagNo, value); }
        }
        string fSampleCollectionDepth;
        public string SampleCollectionDepth
        {
            get { return fSampleCollectionDepth; }
            set { SetPropertyValue<string>(nameof(SampleCollectionDepth), ref fSampleCollectionDepth, value); }
        }
        string fSampleComment;
        [Size(1000)]
        public string SampleComment
        {
            get { return fSampleComment; }
            set { SetPropertyValue<string>(nameof(SampleComment), ref fSampleComment, value); }
        }
        string fSampleID;
        [Size(SizeAttribute.Unlimited)]
        public string SampleID
        {
            get { return fSampleID; }
            set { SetPropertyValue<string>(nameof(SampleID), ref fSampleID, value); }
        }
        string fSecchiDisc_m_Appear;
        [Persistent(@"SecchiDisc(m)Appear")]
        public string SecchiDisc_m_Appear
        {
            get { return fSecchiDisc_m_Appear; }
            set { SetPropertyValue<string>(nameof(SecchiDisc_m_Appear), ref fSecchiDisc_m_Appear, value); }
        }
        string fSecchiDisc_m_Disappear;
        [Persistent(@"SecchiDisc(m)Disappear")]
        public string SecchiDisc_m_Disappear
        {
            get { return fSecchiDisc_m_Disappear; }
            set { SetPropertyValue<string>(nameof(SecchiDisc_m_Disappear), ref fSecchiDisc_m_Disappear, value); }
        }
        string fSegment;
        public string Segment
        {
            get { return fSegment; }
            set { SetPropertyValue<string>(nameof(Segment), ref fSegment, value); }
        }
        string fSignificantPrecip;
        public string SignificantPrecip
        {
            get { return fSignificantPrecip; }
            set { SetPropertyValue<string>(nameof(SignificantPrecip), ref fSignificantPrecip, value); }
        }
        int fSortOrder;
        public int SortOrder
        {
            get { return fSortOrder; }
            set { SetPropertyValue<int>(nameof(SortOrder), ref fSortOrder, value); }
        }
        string fStation;
        [Size(200)]
        public string Station
        {
            get { return fStation; }
            set { SetPropertyValue<string>(nameof(Station), ref fStation, value); }
        }
        string fStationComment;
        [Size(500)]
        public string StationComment
        {
            get { return fStationComment; }
            set { SetPropertyValue<string>(nameof(StationComment), ref fStationComment, value); }
        }
        string fStationID;
        [Size(50)]
        public string StationID
        {
            get { return fStationID; }
            set { SetPropertyValue<string>(nameof(StationID), ref fStationID, value); }
        }
        string fTCEQSiteID;
        public string TCEQSiteID
        {
            get { return fTCEQSiteID; }
            set { SetPropertyValue<string>(nameof(TCEQSiteID), ref fTCEQSiteID, value); }
        }
        string fTestSummary;
        [Size(SizeAttribute.Unlimited)]
        public string TestSummary
        {
            get { return fTestSummary; }
            set { SetPropertyValue<string>(nameof(TestSummary), ref fTestSummary, value); }
        }
        DateTime fTimeEnd;
        public DateTime TimeEnd
        {
            get { return fTimeEnd; }
            set { SetPropertyValue<DateTime>(nameof(TimeEnd), ref fTimeEnd, value); }
        }
        DateTime fTimeStart;
        public DateTime TimeStart
        {
            get { return fTimeStart; }
            set { SetPropertyValue<DateTime>(nameof(TimeStart), ref fTimeStart, value); }
        }
        string fTotalDepth_m_;
        [Persistent(@"TotalDepth(m)")]
        public string TotalDepth_m_
        {
            get { return fTotalDepth_m_; }
            set { SetPropertyValue<string>(nameof(TotalDepth_m_), ref fTotalDepth_m_, value); }
        }
        string fTotalMeasurement;
        public string TotalMeasurement
        {
            get { return fTotalMeasurement; }
            set { SetPropertyValue<string>(nameof(TotalMeasurement), ref fTotalMeasurement, value); }
        }
        bool fUncollected;
        public bool Uncollected
        {
            get { return fUncollected; }
            set { SetPropertyValue<bool>(nameof(Uncollected), ref fUncollected, value); }
        }
        string fUSGSGaugeID;
        public string USGSGaugeID
        {
            get { return fUSGSGaugeID; }
            set { SetPropertyValue<string>(nameof(USGSGaugeID), ref fUSGSGaugeID, value); }
        }
        string fWaterClarity;
        public string WaterClarity
        {
            get { return fWaterClarity; }
            set { SetPropertyValue<string>(nameof(WaterClarity), ref fWaterClarity, value); }
        }
        string fWaterColor;
        public string WaterColor
        {
            get { return fWaterColor; }
            set { SetPropertyValue<string>(nameof(WaterColor), ref fWaterColor, value); }
        }
        string fWaterOdour;
        public string WaterOdour
        {
            get { return fWaterOdour; }
            set { SetPropertyValue<string>(nameof(WaterOdour), ref fWaterOdour, value); }
        }
        string fWaterSurface;
        public string WaterSurface
        {
            get { return fWaterSurface; }
            set { SetPropertyValue<string>(nameof(WaterSurface), ref fWaterSurface, value); }
        }
        string fWaterTemp_C_;
        [Persistent(@"WaterTemp(C)")]
        public string WaterTemp_C_
        {
            get { return fWaterTemp_C_; }
            set { SetPropertyValue<string>(nameof(WaterTemp_C_), ref fWaterTemp_C_, value); }
        }
        string fWeather;
        [Size(50)]
        public string Weather
        {
            get { return fWeather; }
            set { SetPropertyValue<string>(nameof(Weather), ref fWeather, value); }
        }
        string fWindCondition;
        [Size(50)]
        public string WindCondition
        {
            get { return fWindCondition; }
            set { SetPropertyValue<string>(nameof(WindCondition), ref fWindCondition, value); }
        }
        bool fIssubmitted;
        public bool Issubmitted
        {
            get { return fIssubmitted; }
            set { SetPropertyValue<bool>(nameof(Issubmitted), ref fIssubmitted, value); }
        }
        string fSampleDepth;
        public string SampleDepth
        {
            get { return fSampleDepth; }
            set { SetPropertyValue<string>(nameof(SampleDepth), ref fSampleDepth, value); }
        }
        int fNoOfDepth;
        public int NoOfDepth
        {
            get { return fNoOfDepth; }
            set { SetPropertyValue<int>(nameof(NoOfDepth), ref fNoOfDepth, value); }
        }
        decimal fDepth;
        public decimal Depth
        {
            get { return fDepth; }
            set { SetPropertyValue<decimal>(nameof(Depth), ref fDepth, value); }
        }
        DateTime fModifiedDate;
        public DateTime ModifiedDate
        {
            get { return fModifiedDate; }
            set { SetPropertyValue<DateTime>(nameof(ModifiedDate), ref fModifiedDate, value); }
        }
        bool fDry;
        public bool Dry
        {
            get { return fDry; }
            set { SetPropertyValue<bool>(nameof(Dry), ref fDry, value); }
        }
        string fSummary;
        [Size(50)]
        public string Summary
        {
            get { return fSummary; }
            set { SetPropertyValue<string>(nameof(Summary), ref fSummary, value); }
        }
        bool fBlended;
        public bool Blended
        {
            get { return fBlended; }
            set { SetPropertyValue<bool>(nameof(Blended), ref fBlended, value); }
        }
        string fLatitude;
        [Size(50)]
        public string Latitude
        {
            get { return fLatitude; }
            set { SetPropertyValue<string>(nameof(Latitude), ref fLatitude, value); }
        }
        string fLongitude;
        [Size(50)]
        public string Longitude
        {
            get { return fLongitude; }
            set { SetPropertyValue<string>(nameof(Longitude), ref fLongitude, value); }
        }
        string fPlannedLatitude;
        [Size(50)]
        public string PlannedLatitude
        {
            get { return fPlannedLatitude; }
            set { SetPropertyValue<string>(nameof(PlannedLatitude), ref fPlannedLatitude, value); }
        }
        string fPlannedLongitude;
        [Size(50)]
        public string PlannedLongitude
        {
            get { return fPlannedLongitude; }
            set { SetPropertyValue<string>(nameof(PlannedLongitude), ref fPlannedLongitude, value); }
        }
        string fTemprature_C_;
        [Size(50)]
        [Persistent(@"Temprature(C)")]
        public string Temprature_C_
        {
            get { return fTemprature_C_; }
            set { SetPropertyValue<string>(nameof(Temprature_C_), ref fTemprature_C_, value); }
        }
        string fHumidity___;
        [Size(50)]
        [Persistent(@"Humidity(%)")]
        public string Humidity___
        {
            get { return fHumidity___; }
            set { SetPropertyValue<string>(nameof(Humidity___), ref fHumidity___, value); }
        }
        string fBottleID;
        [Size(50)]
        public string BottleID
        {
            get { return fBottleID; }
            set { SetPropertyValue<string>(nameof(BottleID), ref fBottleID, value); }
        }
        string fSampleName;
        [Size(50)]
        public string SampleName
        {
            get { return fSampleName; }
            set { SetPropertyValue<string>(nameof(SampleName), ref fSampleName, value); }
        }
        string fSampleBottleID;
        [Size(50)]
        public string SampleBottleID
        {
            get { return fSampleBottleID; }
            set { SetPropertyValue<string>(nameof(SampleBottleID), ref fSampleBottleID, value); }
        }
        string fFreeResidualChlorine;
        [Size(50)]
        public string FreeResidualChlorine
        {
            get { return fFreeResidualChlorine; }
            set { SetPropertyValue<string>(nameof(FreeResidualChlorine), ref fFreeResidualChlorine, value); }
        }
        string fTotalResidualChlorine;
        [Size(50)]
        public string TotalResidualChlorine
        {
            get { return fTotalResidualChlorine; }
            set { SetPropertyValue<string>(nameof(TotalResidualChlorine), ref fTotalResidualChlorine, value); }
        }
        string fMonoChloramine;
        [Size(50)]
        public string MonoChloramine
        {
            get { return fMonoChloramine; }
            set { SetPropertyValue<string>(nameof(MonoChloramine), ref fMonoChloramine, value); }
        }
        string fFreeAmmonia;
        [Size(50)]
        public string FreeAmmonia
        {
            get { return fFreeAmmonia; }
            set { SetPropertyValue<string>(nameof(FreeAmmonia), ref fFreeAmmonia, value); }
        }
        string fDO;
        [Size(50)]
        public string DO
        {
            get { return fDO; }
            set { SetPropertyValue<string>(nameof(DO), ref fDO, value); }
        }
        string fNitrateasNitrogen;
        [Size(50)]
        public string NitrateasNitrogen
        {
            get { return fNitrateasNitrogen; }
            set { SetPropertyValue<string>(nameof(NitrateasNitrogen), ref fNitrateasNitrogen, value); }
        }
        string fSecchiDiskDepth;
        [Size(50)]
        public string SecchiDiskDepth
        {
            get { return fSecchiDiskDepth; }
            set { SetPropertyValue<string>(nameof(SecchiDiskDepth), ref fSecchiDiskDepth, value); }
        }
        bool fSubmitProcessed;
        public bool SubmitProcessed
        {
            get { return fSubmitProcessed; }
            set { SetPropertyValue<bool>(nameof(SubmitProcessed), ref fSubmitProcessed, value); }
        }

        string fStationTemperature;
        public string StationTemperature
        {
            get { return fStationTemperature; }
            set { SetPropertyValue<string>(nameof(StationTemperature), ref fStationTemperature, value); }
        }
    }
}
