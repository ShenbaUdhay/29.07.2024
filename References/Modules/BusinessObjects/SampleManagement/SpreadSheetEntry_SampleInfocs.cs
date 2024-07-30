using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    [Persistent("SpreadSheetEntry_SampleInfo")]
    public class SpreadSheetEntry : BaseObject
    {
        public SpreadSheetEntry(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            //ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            //ModifiedDate = Library.GetServerTime(Session);
        }


        SampleParameter fuqSampleParameterID;
        public SampleParameter uqSampleParameterID
        {
            get { return fuqSampleParameterID; }
            set { SetPropertyValue<SampleParameter>(nameof(uqSampleParameterID), ref fuqSampleParameterID, value); }
        }
        //QCBatch fuqQCBatchID;
        //public QCBatch uqQCBatchID
        //{
        //    get { return fuqQCBatchID; }
        //    set { SetPropertyValue<QCBatch>(nameof(uqQCBatchID), ref fuqQCBatchID, value); }
        //}
        QCType fuqQCTypeID;
        public QCType uqQCTypeID
        {
            get { return fuqQCTypeID; }
            set { SetPropertyValue<QCType>(nameof(uqQCTypeID), ref fuqQCTypeID, value); }
        }
        string fSystemID;
        [Size(50)]
        public string SystemID
        {
            get { return fSystemID; }
            set { SetPropertyValue<string>(nameof(SystemID), ref fSystemID, value); }
        }
        int fRunNo;
        public int RunNo
        {
            get { return fRunNo; }
            set { SetPropertyValue<int>(nameof(RunNo), ref fRunNo, value); }
        }
        SpreadSheetEntry_AnalyticalBatch fuqAnalyticalBatchID;
        public SpreadSheetEntry_AnalyticalBatch uqAnalyticalBatchID
        {
            get { return fuqAnalyticalBatchID; }
            set { SetPropertyValue(nameof(uqAnalyticalBatchID), ref fuqAnalyticalBatchID, value); }
        }
        int fuqCalibrationID;
        public int uqCalibrationID
        {
            get { return fuqCalibrationID; }
            set { SetPropertyValue<int>(nameof(uqCalibrationID), ref fuqCalibrationID, value); }
        }
        Employee fEnteredBy;
        public Employee EnteredBy
        {
            get { return fEnteredBy; }
            set { SetPropertyValue<Employee>(nameof(EnteredBy), ref fEnteredBy, value); }
        }
        DateTime? fEnteredDate;
        public DateTime? EnteredDate
        {
            get { return fEnteredDate; }
            set { SetPropertyValue<DateTime?>(nameof(EnteredDate), ref fEnteredDate, value); }
        }
        CustomSystemUser fModifiedBy;
        public CustomSystemUser ModifiedBy
        {
            get { return fModifiedBy; }
            set { SetPropertyValue<CustomSystemUser>(nameof(ModifiedBy), ref fModifiedBy, value); }
        }
        DateTime fModifiedDate;
        public DateTime ModifiedDate
        {
            get { return fModifiedDate; }
            set { SetPropertyValue<DateTime>(nameof(ModifiedDate), ref fModifiedDate, value); }
        }
        Employee fAnalyzedBy;
        public Employee AnalyzedBy
        {
            get { return fAnalyzedBy; }
            set { SetPropertyValue<Employee>(nameof(AnalyzedBy), ref fAnalyzedBy, value); }
        }
        DateTime? fAnalyzedDate;
        public DateTime? AnalyzedDate
        {
            get { return fAnalyzedDate; }
            set { SetPropertyValue<DateTime?>(nameof(AnalyzedDate), ref fAnalyzedDate, value); }
        }
        Employee fReviewedBy;
        public Employee ReviewedBy
        {
            get { return fReviewedBy; }
            set { SetPropertyValue<Employee>(nameof(ReviewedBy), ref fReviewedBy, value); }
        }
        DateTime? fReviewedDate;
        public DateTime? ReviewedDate
        {
            get { return fReviewedDate; }
            set { SetPropertyValue<DateTime?>(nameof(ReviewedDate), ref fReviewedDate, value); }
        }
        Employee fVerifiedBy;
        public Employee VerifiedBy
        {
            get { return fVerifiedBy; }
            set { SetPropertyValue<Employee>(nameof(VerifiedBy), ref fVerifiedBy, value); }
        }
        DateTime? fVerifiedDate;
        public DateTime? VerifiedDate
        {
            get { return fVerifiedDate; }
            set { SetPropertyValue<DateTime?>(nameof(VerifiedDate), ref fVerifiedDate, value); }
        }
        bool fIsComplete;
        public bool IsComplete
        {
            get { return fIsComplete; }
            set { SetPropertyValue<bool>(nameof(IsComplete), ref fIsComplete, value); }
        }
        bool fIsExported;
        public bool IsExported
        {
            get { return fIsExported; }
            set { SetPropertyValue<bool>(nameof(IsExported), ref fIsExported, value); }
        }
        string fP_RD;
        [Size(50)]
        [Persistent(@"%RD")]
        public string P_RD
        {
            get { return fP_RD; }
            set { SetPropertyValue<string>(nameof(P_RD), ref fP_RD, value); }
        }
        string fP_RDCtrlLimit;
        [Size(50)]
        [Persistent(@"%RDCtrlLimit")]
        public string P_RDCtrlLimit
        {
            get { return fP_RDCtrlLimit; }
            set { SetPropertyValue<string>(nameof(P_RDCtrlLimit), ref fP_RDCtrlLimit, value); }
        }
        string fP_RecCtrlLimit;
        [Size(50)]
        [Persistent(@"%RecCtrlLimit")]
        public string P_RecCtrlLimit
        {
            get { return fP_RecCtrlLimit; }
            set { SetPropertyValue<string>(nameof(P_RecCtrlLimit), ref fP_RecCtrlLimit, value); }
        }
        string fP_Recovery;
        [Size(50)]
        [Persistent(@"%Recovery")]
        public string P_Recovery
        {
            get { return fP_Recovery; }
            set { SetPropertyValue<string>(nameof(P_Recovery), ref fP_Recovery, value); }
        }
        string fP_TsFinalWt;
        [Size(50)]
        [Persistent(@"%TsFinalWt")]
        public string P_TsFinalWt
        {
            get { return fP_TsFinalWt; }
            set { SetPropertyValue<string>(nameof(P_TsFinalWt), ref fP_TsFinalWt, value); }
        }
        string fA_NO2_;
        [Size(50)]
        [Persistent(@"A(NO2)")]
        public string A_NO2_
        {
            get { return fA_NO2_; }
            set { SetPropertyValue<string>(nameof(A_NO2_), ref fA_NO2_, value); }
        }
        string fA_NO2_X;
        [Size(50)]
        [Persistent(@"A(NO2)X")]
        public string A_NO2_X
        {
            get { return fA_NO2_X; }
            set { SetPropertyValue<string>(nameof(A_NO2_X), ref fA_NO2_X, value); }
        }
        string fA0;
        [Size(50)]
        public string A0
        {
            get { return fA0; }
            set { SetPropertyValue<string>(nameof(A0), ref fA0, value); }
        }
        string fAb;
        [Size(50)]
        public string Ab
        {
            get { return fAb; }
            set { SetPropertyValue<string>(nameof(Ab), ref fAb, value); }
        }
        string fAb1;
        [Size(50)]
        public string Ab1
        {
            get { return fAb1; }
            set { SetPropertyValue<string>(nameof(Ab1), ref fAb1, value); }
        }
        string fAb11;
        [Size(50)]
        public string Ab11
        {
            get { return fAb11; }
            set { SetPropertyValue<string>(nameof(Ab11), ref fAb11, value); }
        }
        string fAb12;
        [Size(50)]
        public string Ab12
        {
            get { return fAb12; }
            set { SetPropertyValue<string>(nameof(Ab12), ref fAb12, value); }
        }
        string fAb2;
        [Size(50)]
        public string Ab2
        {
            get { return fAb2; }
            set { SetPropertyValue<string>(nameof(Ab2), ref fAb2, value); }
        }
        string fAb21;
        [Size(50)]
        public string Ab21
        {
            get { return fAb21; }
            set { SetPropertyValue<string>(nameof(Ab21), ref fAb21, value); }
        }
        string fAb22;
        [Size(50)]
        public string Ab22
        {
            get { return fAb22; }
            set { SetPropertyValue<string>(nameof(Ab22), ref fAb22, value); }
        }
        string fAn;
        [Size(50)]
        public string An
        {
            get { return fAn; }
            set { SetPropertyValue<string>(nameof(An), ref fAn, value); }
        }
        string fAw;
        [Size(50)]
        public string Aw
        {
            get { return fAw; }
            set { SetPropertyValue<string>(nameof(Aw), ref fAw, value); }
        }
        string fAz;
        [Size(50)]
        public string Az
        {
            get { return fAz; }
            set { SetPropertyValue<string>(nameof(Az), ref fAz, value); }
        }
        string fCNO2;
        [Size(50)]
        public string CNO2
        {
            get { return fCNO2; }
            set { SetPropertyValue<string>(nameof(CNO2), ref fCNO2, value); }
        }
        string fCalibrationConc;
        [Size(50)]
        public string CalibrationConc
        {
            get { return fCalibrationConc; }
            set { SetPropertyValue<string>(nameof(CalibrationConc), ref fCalibrationConc, value); }
        }
        string fCCVConc;
        [Size(50)]
        public string CCVConc
        {
            get { return fCCVConc; }
            set { SetPropertyValue<string>(nameof(CCVConc), ref fCCVConc, value); }
        }
        string fCCVID;
        [Size(50)]
        public string CCVID
        {
            get { return fCCVID; }
            set { SetPropertyValue<string>(nameof(CCVID), ref fCCVID, value); }
        }
        string fCellLength;
        [Size(50)]
        public string CellLength
        {
            get { return fCellLength; }
            set { SetPropertyValue<string>(nameof(CellLength), ref fCellLength, value); }
        }
        string fClientName;
        [Size(50)]
        public string ClientName
        {
            get { return fClientName; }
            set { SetPropertyValue<string>(nameof(ClientName), ref fClientName, value); }
        }
        string fConcentration;
        [Size(50)]
        public string Concentration
        {
            get { return fConcentration; }
            set { SetPropertyValue<string>(nameof(Concentration), ref fConcentration, value); }
        }
        string fDilution;
        [Size(50)]
        public string Dilution
        {
            get { return fDilution; }
            set { SetPropertyValue<string>(nameof(Dilution), ref fDilution, value); }
        }
        string fEqwt;
        [Size(50)]
        public string Eqwt
        {
            get { return fEqwt; }
            set { SetPropertyValue<string>(nameof(Eqwt), ref fEqwt, value); }
        }
        string fFilterID;
        [Size(50)]
        public string FilterID
        {
            get { return fFilterID; }
            set { SetPropertyValue<string>(nameof(FilterID), ref fFilterID, value); }
        }
        string fFinalResult;
        [Size(50)]
        public string FinalResult
        {
            get { return fFinalResult; }
            set { SetPropertyValue<string>(nameof(FinalResult), ref fFinalResult, value); }
        }
        string fFlag;
        [Size(50)]
        public string Flag
        {
            get { return fFlag; }
            set { SetPropertyValue<string>(nameof(Flag), ref fFlag, value); }
        }
        string fGrossWt1;
        [Size(50)]
        public string GrossWt1
        {
            get { return fGrossWt1; }
            set { SetPropertyValue<string>(nameof(GrossWt1), ref fGrossWt1, value); }
        }
        string fGrossWt2;
        [Size(50)]
        public string GrossWt2
        {
            get { return fGrossWt2; }
            set { SetPropertyValue<string>(nameof(GrossWt2), ref fGrossWt2, value); }
        }
        string fInitialWt;
        [Size(50)]
        public string InitialWt
        {
            get { return fInitialWt; }
            set { SetPropertyValue<string>(nameof(InitialWt), ref fInitialWt, value); }
        }
        string fInstrument;
        [Size(int.MaxValue)]
        public string Instrument
        {
            get { return fInstrument; }
            set { SetPropertyValue<string>(nameof(Instrument), ref fInstrument, value); }
        }
        string fLengthRatio;
        [Size(50)]
        public string LengthRatio
        {
            get { return fLengthRatio; }
            set { SetPropertyValue<string>(nameof(LengthRatio), ref fLengthRatio, value); }
        }
        string fNetWt;
        [Size(50)]
        public string NetWt
        {
            get { return fNetWt; }
            set { SetPropertyValue<string>(nameof(NetWt), ref fNetWt, value); }
        }
        string fNew;
        [Size(50)]
        public string New
        {
            get { return fNew; }
            set { SetPropertyValue<string>(nameof(New), ref fNew, value); }
        }
        string fNumericResult;
        [Size(50)]
        public string NumericResult
        {
            get { return fNumericResult; }
            set { SetPropertyValue<string>(nameof(NumericResult), ref fNumericResult, value); }
        }
        string fOriginalReportingLimit;
        [Size(50)]
        public string OriginalReportingLimit
        {
            get { return fOriginalReportingLimit; }
            set { SetPropertyValue<string>(nameof(OriginalReportingLimit), ref fOriginalReportingLimit, value); }
        }
        string fR;
        [Size(50)]
        public string R
        {
            get { return fR; }
            set { SetPropertyValue<string>(nameof(R), ref fR, value); }
        }
        string fReading;
        [Size(50)]
        public string Reading
        {
            get { return fReading; }
            set { SetPropertyValue<string>(nameof(Reading), ref fReading, value); }
        }
        string fReading1;
        [Size(50)]
        public string Reading1
        {
            get { return fReading1; }
            set { SetPropertyValue<string>(nameof(Reading1), ref fReading1, value); }
        }
        string fReading2;
        [Size(50)]
        public string Reading2
        {
            get { return fReading2; }
            set { SetPropertyValue<string>(nameof(Reading2), ref fReading2, value); }
        }
        string fRegressionAmount;
        [Size(50)]
        public string RegressionAmount
        {
            get { return fRegressionAmount; }
            set { SetPropertyValue<string>(nameof(RegressionAmount), ref fRegressionAmount, value); }
        }
        string fResponse;
        [Size(50)]
        public string Response
        {
            get { return fResponse; }
            set { SetPropertyValue<string>(nameof(Response), ref fResponse, value); }
        }
        string fResult;
        [Size(50)]
        public string Result
        {
            get { return fResult; }
            set { SetPropertyValue<string>(nameof(Result), ref fResult, value); }
        }
        string fSalinity;
        [Size(50)]
        public string Salinity
        {
            get { return fSalinity; }
            set { SetPropertyValue<string>(nameof(Salinity), ref fSalinity, value); }
        }
        string fSaturation_;
        [Size(50)]
        [Persistent(@"Saturation%")]
        public string Saturation_
        {
            get { return fSaturation_; }
            set { SetPropertyValue<string>(nameof(Saturation_), ref fSaturation_, value); }
        }
        string fSaturationConc;
        [Size(50)]
        public string SaturationConc
        {
            get { return fSaturationConc; }
            set { SetPropertyValue<string>(nameof(SaturationConc), ref fSaturationConc, value); }
        }
        double fSpikeAmount;
        [Size(50)]
        public double SpikeAmount
        {
            get { return fSpikeAmount; }
            set { SetPropertyValue<double>(nameof(SpikeAmount), ref fSpikeAmount, value); }
        }
        string fSpikeTV;
        [Size(50)]
        public string SpikeTV
        {
            get { return fSpikeTV; }
            set { SetPropertyValue<string>(nameof(SpikeTV), ref fSpikeTV, value); }
        }
        string fStdConc;
        [Size(50)]
        public string StdConc
        {
            get { return fStdConc; }
            set { SetPropertyValue<string>(nameof(StdConc), ref fStdConc, value); }
        }
        string fT1;
        [Size(50)]
        public string T1
        {
            get { return fT1; }
            set { SetPropertyValue<string>(nameof(T1), ref fT1, value); }
        }
        string fUncertainity;
        [Size(50)]
        public string Uncertainity
        {
            get { return fUncertainity; }
            set { SetPropertyValue<string>(nameof(Uncertainity), ref fUncertainity, value); }
        }
        string fUnits;
        [Size(50)]
        public string Units
        {
            get { return fUnits; }
            set { SetPropertyValue<string>(nameof(Units), ref fUnits, value); }
        }
        string fVb;
        [Size(50)]
        public string Vb
        {
            get { return fVb; }
            set { SetPropertyValue<string>(nameof(Vb), ref fVb, value); }
        }
        string fVb1;
        [Size(50)]
        public string Vb1
        {
            get { return fVb1; }
            set { SetPropertyValue<string>(nameof(Vb1), ref fVb1, value); }
        }
        string fVb2;
        [Size(50)]
        public string Vb2
        {
            get { return fVb2; }
            set { SetPropertyValue<string>(nameof(Vb2), ref fVb2, value); }
        }
        string fVc;
        [Size(50)]
        public string Vc
        {
            get { return fVc; }
            set { SetPropertyValue<string>(nameof(Vc), ref fVc, value); }
        }
        string fVc1;
        [Size(50)]
        public string Vc1
        {
            get { return fVc1; }
            set { SetPropertyValue<string>(nameof(Vc1), ref fVc1, value); }
        }
        string fVc2;
        [Size(50)]
        public string Vc2
        {
            get { return fVc2; }
            set { SetPropertyValue<string>(nameof(Vc2), ref fVc2, value); }
        }
        string fVolumeUsed;
        [Size(50)]
        public string VolumeUsed
        {
            get { return fVolumeUsed; }
            set { SetPropertyValue<string>(nameof(VolumeUsed), ref fVolumeUsed, value); }
        }
        string fVt;
        [Size(50)]
        public string Vt
        {
            get { return fVt; }
            set { SetPropertyValue<string>(nameof(Vt), ref fVt, value); }
        }
        string fVt1;
        [Size(50)]
        public string Vt1
        {
            get { return fVt1; }
            set { SetPropertyValue<string>(nameof(Vt1), ref fVt1, value); }
        }
        string fVt2;
        [Size(50)]
        public string Vt2
        {
            get { return fVt2; }
            set { SetPropertyValue<string>(nameof(Vt2), ref fVt2, value); }
        }
        string fWaveLength;
        [Size(50)]
        public string WaveLength
        {
            get { return fWaveLength; }
            set { SetPropertyValue<string>(nameof(WaveLength), ref fWaveLength, value); }
        }
        string fWT;
        [Size(50)]
        public string WT
        {
            get { return fWT; }
            set { SetPropertyValue<string>(nameof(WT), ref fWT, value); }
        }
        string fAvgResult;
        [Size(50)]
        public string AvgResult
        {
            get { return fAvgResult; }
            set { SetPropertyValue<string>(nameof(AvgResult), ref fAvgResult, value); }
        }
        string fSatAmount;
        [Size(50)]
        public string SatAmount
        {
            get { return fSatAmount; }
            set { SetPropertyValue<string>(nameof(SatAmount), ref fSatAmount, value); }
        }
        string fSatDegree;
        [Size(50)]
        public string SatDegree
        {
            get { return fSatDegree; }
            set { SetPropertyValue<string>(nameof(SatDegree), ref fSatDegree, value); }
        }
        string fSuppParam1;
        [Size(50)]
        public string SuppParam1
        {
            get { return fSuppParam1; }
            set { SetPropertyValue<string>(nameof(SuppParam1), ref fSuppParam1, value); }
        }
        string fSuppParam2;
        [Size(50)]
        public string SuppParam2
        {
            get { return fSuppParam2; }
            set { SetPropertyValue<string>(nameof(SuppParam2), ref fSuppParam2, value); }
        }
        string fSuppParam3;
        [Size(50)]
        public string SuppParam3
        {
            get { return fSuppParam3; }
            set { SetPropertyValue<string>(nameof(SuppParam3), ref fSuppParam3, value); }
        }
        string fBottleID;
        [Size(50)]
        public string BottleID
        {
            get { return fBottleID; }
            set { SetPropertyValue<string>(nameof(BottleID), ref fBottleID, value); }
        }
        string fUserDefined1;
        [Size(50)]
        public string UserDefined1
        {
            get { return fUserDefined1; }
            set { SetPropertyValue<string>(nameof(UserDefined1), ref fUserDefined1, value); }
        }
        string fUserDefined2;
        [Size(50)]
        public string UserDefined2
        {
            get { return fUserDefined2; }
            set { SetPropertyValue<string>(nameof(UserDefined2), ref fUserDefined2, value); }
        }
        string fUserDefined3;
        [Size(50)]
        public string UserDefined3
        {
            get { return fUserDefined3; }
            set { SetPropertyValue<string>(nameof(UserDefined3), ref fUserDefined3, value); }
        }
        string fUserDefined4;
        [Size(50)]
        public string UserDefined4
        {
            get { return fUserDefined4; }
            set { SetPropertyValue<string>(nameof(UserDefined4), ref fUserDefined4, value); }
        }
        string fUserDefined5;
        [Size(50)]
        public string UserDefined5
        {
            get { return fUserDefined5; }
            set { SetPropertyValue<string>(nameof(UserDefined5), ref fUserDefined5, value); }
        }
        string fUserDefined6;
        [Size(50)]
        public string UserDefined6
        {
            get { return fUserDefined6; }
            set { SetPropertyValue<string>(nameof(UserDefined6), ref fUserDefined6, value); }
        }
        string fUserDefined7;
        [Size(50)]
        public string UserDefined7
        {
            get { return fUserDefined7; }
            set { SetPropertyValue<string>(nameof(UserDefined7), ref fUserDefined7, value); }
        }
        string fUserDefined8;
        [Size(50)]
        public string UserDefined8
        {
            get { return fUserDefined8; }
            set { SetPropertyValue<string>(nameof(UserDefined8), ref fUserDefined8, value); }
        }
        string fUserDefined9;
        [Size(50)]
        public string UserDefined9
        {
            get { return fUserDefined9; }
            set { SetPropertyValue<string>(nameof(UserDefined9), ref fUserDefined9, value); }
        }
        string fUserDefined10;
        [Size(50)]
        public string UserDefined10
        {
            get { return fUserDefined10; }
            set { SetPropertyValue<string>(nameof(UserDefined10), ref fUserDefined10, value); }
        }
        string fDF;
        [Size(50)]
        public string DF
        {
            get { return fDF; }
            set { SetPropertyValue<string>(nameof(DF), ref fDF, value); }
        }
        string fTwX;
        [Size(50)]
        public string TwX
        {
            get { return fTwX; }
            set { SetPropertyValue<string>(nameof(TwX), ref fTwX, value); }
        }
        string fTmX;
        [Size(50)]
        public string TmX
        {
            get { return fTmX; }
            set { SetPropertyValue<string>(nameof(TmX), ref fTmX, value); }
        }
        string fBeta;
        [Size(50)]
        public string Beta
        {
            get { return fBeta; }
            set { SetPropertyValue<string>(nameof(Beta), ref fBeta, value); }
        }
        string fAlpha;
        [Size(50)]
        public string Alpha
        {
            get { return fAlpha; }
            set { SetPropertyValue<string>(nameof(Alpha), ref fAlpha, value); }
        }
        string fRemark;
        [Size(2000)]
        public string Remark
        {
            get { return fRemark; }
            set { SetPropertyValue<string>(nameof(Remark), ref fRemark, value); }
        }
        int fUQTESTQCSPIKE;
        public int UQTESTQCSPIKE
        {
            get { return fUQTESTQCSPIKE; }
            set { SetPropertyValue<int>(nameof(UQTESTQCSPIKE), ref fUQTESTQCSPIKE, value); }
        }
        int fUQTESTQCSTANDARDSPIKEID;
        public int UQTESTQCSTANDARDSPIKEID
        {
            get { return fUQTESTQCSTANDARDSPIKEID; }
            set { SetPropertyValue<int>(nameof(UQTESTQCSTANDARDSPIKEID), ref fUQTESTQCSTANDARDSPIKEID, value); }
        }
        string fRPD;
        [Size(50)]
        public string RPD
        {
            get { return fRPD; }
            set { SetPropertyValue<string>(nameof(RPD), ref fRPD, value); }
        }
        string fMDL;
        [Size(50)]
        public string MDL
        {
            get { return fMDL; }
            set { SetPropertyValue<string>(nameof(MDL), ref fMDL, value); }
        }
        Testparameter fUQTESTPARAMETERID;
        public Testparameter UQTESTPARAMETERID
        {
            get { return fUQTESTPARAMETERID; }
            set { SetPropertyValue<Testparameter>(nameof(UQTESTPARAMETERID), ref fUQTESTPARAMETERID, value); }
        }
        string fRPTLIMIT;
        [Size(50)]
        public string RPTLIMIT
        {
            get { return fRPTLIMIT; }
            set { SetPropertyValue<string>(nameof(RPTLIMIT), ref fRPTLIMIT, value); }
        }
        string fFLOWRATE;
        [Size(50)]
        public string FLOWRATE
        {
            get { return fFLOWRATE; }
            set { SetPropertyValue<string>(nameof(FLOWRATE), ref fFLOWRATE, value); }
        }
        string fABSORPTIONVOL;
        [Size(50)]
        public string ABSORPTIONVOL
        {
            get { return fABSORPTIONVOL; }
            set { SetPropertyValue<string>(nameof(ABSORPTIONVOL), ref fABSORPTIONVOL, value); }
        }
        string fSTANDARDVOLUME;
        [Size(50)]
        public string STANDARDVOLUME
        {
            get { return fSTANDARDVOLUME; }
            set { SetPropertyValue<string>(nameof(STANDARDVOLUME), ref fSTANDARDVOLUME, value); }
        }
        string fVOLUME;
        [Size(50)]
        public string VOLUME
        {
            get { return fVOLUME; }
            set { SetPropertyValue<string>(nameof(VOLUME), ref fVOLUME, value); }
        }
        string fFinalVolume;
        [Size(50)]
        public string FinalVolume
        {
            get { return fFinalVolume; }
            set { SetPropertyValue<string>(nameof(FinalVolume), ref fFinalVolume, value); }
        }
        string fFileResult;
        [Size(50)]
        public string FileResult
        {
            get { return fFileResult; }
            set { SetPropertyValue<string>(nameof(FileResult), ref fFileResult, value); }
        }
        string fFileResponse;
        [Size(50)]
        public string FileResponse
        {
            get { return fFileResponse; }
            set { SetPropertyValue<string>(nameof(FileResponse), ref fFileResponse, value); }
        }
        string fLT;
        [Size(50)]
        public string LT
        {
            get { return fLT; }
            set { SetPropertyValue<string>(nameof(LT), ref fLT, value); }
        }
        string fRE;
        [Size(50)]
        public string RE
        {
            get { return fRE; }
            set { SetPropertyValue<string>(nameof(RE), ref fRE, value); }
        }
        string fITEM1;
        [Size(50)]
        public string ITEM1
        {
            get { return fITEM1; }
            set { SetPropertyValue<string>(nameof(ITEM1), ref fITEM1, value); }
        }
        string fITEM2;
        [Size(50)]
        public string ITEM2
        {
            get { return fITEM2; }
            set { SetPropertyValue<string>(nameof(ITEM2), ref fITEM2, value); }
        }
        string fITEM3;
        [Size(50)]
        public string ITEM3
        {
            get { return fITEM3; }
            set { SetPropertyValue<string>(nameof(ITEM3), ref fITEM3, value); }
        }
        string fITEM4;
        [Size(50)]
        public string ITEM4
        {
            get { return fITEM4; }
            set { SetPropertyValue<string>(nameof(ITEM4), ref fITEM4, value); }
        }
        string fITEM5;
        [Size(50)]
        public string ITEM5
        {
            get { return fITEM5; }
            set { SetPropertyValue<string>(nameof(ITEM5), ref fITEM5, value); }
        }
        string fITEM6;
        [Size(50)]
        public string ITEM6
        {
            get { return fITEM6; }
            set { SetPropertyValue<string>(nameof(ITEM6), ref fITEM6, value); }
        }
        string fITEM7;
        [Size(50)]
        public string ITEM7
        {
            get { return fITEM7; }
            set { SetPropertyValue<string>(nameof(ITEM7), ref fITEM7, value); }
        }
        string fITEM8;
        [Size(50)]
        public string ITEM8
        {
            get { return fITEM8; }
            set { SetPropertyValue<string>(nameof(ITEM8), ref fITEM8, value); }
        }
        string fITEM9;
        [Size(50)]
        public string ITEM9
        {
            get { return fITEM9; }
            set { SetPropertyValue<string>(nameof(ITEM9), ref fITEM9, value); }
        }
        string fITEM10;
        [Size(50)]
        public string ITEM10
        {
            get { return fITEM10; }
            set { SetPropertyValue<string>(nameof(ITEM10), ref fITEM10, value); }
        }
        string fITEM11;
        [Size(50)]
        public string ITEM11
        {
            get { return fITEM11; }
            set { SetPropertyValue<string>(nameof(ITEM11), ref fITEM11, value); }
        }
        string fITEM12;
        [Size(50)]
        public string ITEM12
        {
            get { return fITEM12; }
            set { SetPropertyValue<string>(nameof(ITEM12), ref fITEM12, value); }
        }
        string fITEM13;
        [Size(50)]
        public string ITEM13
        {
            get { return fITEM13; }
            set { SetPropertyValue<string>(nameof(ITEM13), ref fITEM13, value); }
        }
        string fITEM14;
        [Size(50)]
        public string ITEM14
        {
            get { return fITEM14; }
            set { SetPropertyValue<string>(nameof(ITEM14), ref fITEM14, value); }
        }
        string fITEM15;
        [Size(50)]
        public string ITEM15
        {
            get { return fITEM15; }
            set { SetPropertyValue<string>(nameof(ITEM15), ref fITEM15, value); }
        }
        string fITEM16;
        [Size(50)]
        public string ITEM16
        {
            get { return fITEM16; }
            set { SetPropertyValue<string>(nameof(ITEM16), ref fITEM16, value); }
        }
        string fITEM17;
        [Size(50)]
        public string ITEM17
        {
            get { return fITEM17; }
            set { SetPropertyValue<string>(nameof(ITEM17), ref fITEM17, value); }
        }
        string fITEM18;
        [Size(50)]
        public string ITEM18
        {
            get { return fITEM18; }
            set { SetPropertyValue<string>(nameof(ITEM18), ref fITEM18, value); }
        }
        string fITEM19;
        [Size(50)]
        public string ITEM19
        {
            get { return fITEM19; }
            set { SetPropertyValue<string>(nameof(ITEM19), ref fITEM19, value); }
        }
        string fITEM20;
        [Size(50)]
        public string ITEM20
        {
            get { return fITEM20; }
            set { SetPropertyValue<string>(nameof(ITEM20), ref fITEM20, value); }
        }
        string fGroupAvgResult;
        [Size(50)]
        public string GroupAvgResult
        {
            get { return fGroupAvgResult; }
            set { SetPropertyValue<string>(nameof(GroupAvgResult), ref fGroupAvgResult, value); }
        }
        string fEmissionRate;
        [Size(50)]
        public string EmissionRate
        {
            get { return fEmissionRate; }
            set { SetPropertyValue<string>(nameof(EmissionRate), ref fEmissionRate, value); }
        }
        string fAvgEmissionRate;
        [Size(50)]
        public string AvgEmissionRate
        {
            get { return fAvgEmissionRate; }
            set { SetPropertyValue<string>(nameof(AvgEmissionRate), ref fAvgEmissionRate, value); }
        }
        string fUserDefined11;
        [Size(50)]
        public string UserDefined11
        {
            get { return fUserDefined11; }
            set { SetPropertyValue<string>(nameof(UserDefined11), ref fUserDefined11, value); }
        }
        string fUserDefined12;
        [Size(50)]
        public string UserDefined12
        {
            get { return fUserDefined12; }
            set { SetPropertyValue<string>(nameof(UserDefined12), ref fUserDefined12, value); }
        }
        string fUserDefined13;
        [Size(50)]
        public string UserDefined13
        {
            get { return fUserDefined13; }
            set { SetPropertyValue<string>(nameof(UserDefined13), ref fUserDefined13, value); }
        }
        string fUserDefined14;
        [Size(50)]
        public string UserDefined14
        {
            get { return fUserDefined14; }
            set { SetPropertyValue<string>(nameof(UserDefined14), ref fUserDefined14, value); }
        }
        string fUserDefined15;
        [Size(50)]
        public string UserDefined15
        {
            get { return fUserDefined15; }
            set { SetPropertyValue<string>(nameof(UserDefined15), ref fUserDefined15, value); }
        }
        string fUserDefined16;
        [Size(50)]
        public string UserDefined16
        {
            get { return fUserDefined16; }
            set { SetPropertyValue<string>(nameof(UserDefined16), ref fUserDefined16, value); }
        }
        string fUserDefined17;
        [Size(50)]
        public string UserDefined17
        {
            get { return fUserDefined17; }
            set { SetPropertyValue<string>(nameof(UserDefined17), ref fUserDefined17, value); }
        }
        string fUserDefined18;
        [Size(50)]
        public string UserDefined18
        {
            get { return fUserDefined18; }
            set { SetPropertyValue<string>(nameof(UserDefined18), ref fUserDefined18, value); }
        }
        string fUserDefined19;
        [Size(50)]
        public string UserDefined19
        {
            get { return fUserDefined19; }
            set { SetPropertyValue<string>(nameof(UserDefined19), ref fUserDefined19, value); }
        }
        string fUserDefined20;
        [Size(50)]
        public string UserDefined20
        {
            get { return fUserDefined20; }
            set { SetPropertyValue<string>(nameof(UserDefined20), ref fUserDefined20, value); }
        }
        string fAssignedValue;
        [Size(50)]
        public string AssignedValue
        {
            get { return fAssignedValue; }
            set { SetPropertyValue<string>(nameof(AssignedValue), ref fAssignedValue, value); }
        }
        string fInstrumentTrackType;
        [Size(50)]
        public string InstrumentTrackType
        {
            get { return fInstrumentTrackType; }
            set { SetPropertyValue<string>(nameof(InstrumentTrackType), ref fInstrumentTrackType, value); }
        }
        string fCalibrationType;
        [Size(50)]
        public string CalibrationType
        {
            get { return fCalibrationType; }
            set { SetPropertyValue<string>(nameof(CalibrationType), ref fCalibrationType, value); }
        }
        string fCalibrationEquation;
        [Size(50)]
        public string CalibrationEquation
        {
            get { return fCalibrationEquation; }
            set { SetPropertyValue<string>(nameof(CalibrationEquation), ref fCalibrationEquation, value); }
        }
        DateTime fInstrumentExpDate;
        public DateTime InstrumentExpDate
        {
            get { return fInstrumentExpDate; }
            set { SetPropertyValue<DateTime>(nameof(InstrumentExpDate), ref fInstrumentExpDate, value); }
        }
        string fReferSolution;
        [Size(50)]
        public string ReferSolution
        {
            get { return fReferSolution; }
            set { SetPropertyValue<string>(nameof(ReferSolution), ref fReferSolution, value); }
        }
        string fCellThickness;
        [Size(50)]
        public string CellThickness
        {
            get { return fCellThickness; }
            set { SetPropertyValue<string>(nameof(CellThickness), ref fCellThickness, value); }
        }
        string fInstrumentModel;
        [Size(50)]
        public string InstrumentModel
        {
            get { return fInstrumentModel; }
            set { SetPropertyValue<string>(nameof(InstrumentModel), ref fInstrumentModel, value); }
        }
        string fColorTime;
        [Size(50)]
        public string ColorTime
        {
            get { return fColorTime; }
            set { SetPropertyValue<string>(nameof(ColorTime), ref fColorTime, value); }
        }
        string fColorTemp;
        [Size(50)]
        public string ColorTemp
        {
            get { return fColorTemp; }
            set { SetPropertyValue<string>(nameof(ColorTemp), ref fColorTemp, value); }
        }
        string fP_RecLLimit;
        [Size(50)]
        [Persistent(@"%RecLLimit")]
        public string P_RecLLimit
        {
            get { return fP_RecLLimit; }
            set { SetPropertyValue<string>(nameof(P_RecLLimit), ref fP_RecLLimit, value); }
        }
        string fP_RecULimit;
        [Size(50)]
        [Persistent(@"%RecULimit")]
        public string P_RecULimit
        {
            get { return fP_RecULimit; }
            set { SetPropertyValue<string>(nameof(P_RecULimit), ref fP_RecULimit, value); }
        }
        string fP_RPDLLimit;
        [Size(50)]
        [Persistent(@"%RPDLLimit")]
        public string P_RPDLLimit
        {
            get { return fP_RPDLLimit; }
            set { SetPropertyValue<string>(nameof(P_RPDLLimit), ref fP_RPDLLimit, value); }
        }
        string fP_RPDULimit;
        [Size(50)]
        [Persistent(@"%RPDULimit")]
        public string P_RPDULimit
        {
            get { return fP_RPDULimit; }
            set { SetPropertyValue<string>(nameof(P_RPDULimit), ref fP_RPDULimit, value); }
        }
        string fConversionConc;
        [Size(50)]
        public string ConversionConc
        {
            get { return fConversionConc; }
            set { SetPropertyValue<string>(nameof(ConversionConc), ref fConversionConc, value); }
        }
        string fAvgConversionConc;
        [Size(50)]
        public string AvgConversionConc
        {
            get { return fAvgConversionConc; }
            set { SetPropertyValue<string>(nameof(AvgConversionConc), ref fAvgConversionConc, value); }
        }
        string fUncertainityLLimit;
        [Size(50)]
        public string UncertainityLLimit
        {
            get { return fUncertainityLLimit; }
            set { SetPropertyValue<string>(nameof(UncertainityLLimit), ref fUncertainityLLimit, value); }
        }
        string fUncertainityULimit;
        [Size(50)]
        public string UncertainityULimit
        {
            get { return fUncertainityULimit; }
            set { SetPropertyValue<string>(nameof(UncertainityULimit), ref fUncertainityULimit, value); }
        }
        string fSpikeUnit;
        [Size(50)]
        public string SpikeUnit
        {
            get { return fSpikeUnit; }
            set { SetPropertyValue<string>(nameof(SpikeUnit), ref fSpikeUnit, value); }
        }
        string fSpikeVol;
        [Size(50)]
        public string SpikeVol
        {
            get { return fSpikeVol; }
            set { SetPropertyValue<string>(nameof(SpikeVol), ref fSpikeVol, value); }
        }
        string fDestinationDataTransferFilePath;
        [Size(500)]
        public string DestinationDataTransferFilePath
        {
            get { return fDestinationDataTransferFilePath; }
            set { SetPropertyValue<string>(nameof(DestinationDataTransferFilePath), ref fDestinationDataTransferFilePath, value); }
        }
        int fuqSampleTestID;
        public int uqSampleTestID
        {
            get { return fuqSampleTestID; }
            set { SetPropertyValue<int>(nameof(uqSampleTestID), ref fuqSampleTestID, value); }
        }
        int fuqTestSurrogateID;
        [ColumnDbDefaultValue("((-1))")]
        public int uqTestSurrogateID
        {
            get { return fuqTestSurrogateID; }
            set { SetPropertyValue<int>(nameof(uqTestSurrogateID), ref fuqTestSurrogateID, value); }
        }
        string fSampleID;
        [Size(50)]
        public string SampleID
        {
            get { return fSampleID; }
            set { SetPropertyValue<string>(nameof(SampleID), ref fSampleID, value); }
        }
        string fHumidity;
        [Size(50)]
        public string Humidity
        {
            get { return fHumidity; }
            set { SetPropertyValue<string>(nameof(Humidity), ref fHumidity, value); }
        }
        string fTemperature;
        [Size(50)]
        public string Temperature
        {
            get { return fTemperature; }
            set { SetPropertyValue<string>(nameof(Temperature), ref fTemperature, value); }
        }
        string fQCSampleResult;
        [Size(50)]
        public string QCSampleResult
        {
            get { return fQCSampleResult; }
            set { SetPropertyValue<string>(nameof(QCSampleResult), ref fQCSampleResult, value); }
        }

        DateTime? fValidatedDate;
        public DateTime? ValidatedDate
        {
            get { return fValidatedDate; }
            set { SetPropertyValue<DateTime?>("ValidatedDate", ref fValidatedDate, value); }
        }

        DateTime? fApprovedDate;
        public DateTime? ApprovedDate
        {
            get { return fApprovedDate; }
            set { SetPropertyValue<DateTime?>("ApprovedDate", ref fApprovedDate, value); }
        }

        Employee fValidatedBy;
        public Employee ValidatedBy
        {
            get { return fValidatedBy; }
            set
            {
                SetPropertyValue("ValidatedBy", ref fValidatedBy, value);
            }
        }

        Employee fApprovedBy;
        public Employee ApprovedBy
        {
            get { return fApprovedBy; }
            set
            {
                SetPropertyValue("ApprovedBy", ref fApprovedBy, value);
            }
        }

        Samplestatus _Status;
        public Samplestatus Status
        {
            get { return _Status; }
            set { SetPropertyValue("Status", ref _Status, value); }
        }

        Samplecheckin _uqSampleJobID;
        public Samplecheckin uqSampleJobID
        {
            get { return _uqSampleJobID; }
            set { SetPropertyValue("uqSampleJobID", ref _uqSampleJobID, value); }
        }

        string fLabwareName1;
        [Size(int.MaxValue)]
        [Persistent(@"LabwareName1")]
        public string LabwareName1
        {
            get { return fLabwareName1; }
            set { SetPropertyValue<string>(nameof(LabwareName1), ref fLabwareName1, value); }
        }

        string fLabwareName2;
        [Size(int.MaxValue)]
        [Persistent(@"LabwareName2")]
        public string LabwareName2
        {
            get { return fLabwareName2; }
            set { SetPropertyValue<string>(nameof(LabwareName2), ref fLabwareName2, value); }
        }

        string fLabwareName3;
        [Size(int.MaxValue)]
        [Persistent(@"LabwareName3")]
        public string LabwareName3
        {
            get { return fLabwareName3; }
            set { SetPropertyValue<string>(nameof(LabwareName3), ref fLabwareName3, value); }
        }

        string fLabwareName4;
        [Size(100)]
        [Persistent(@"LabwareName4")]
        public string LabwareName4
        {
            get { return fLabwareName4; }
            set { SetPropertyValue<string>(nameof(LabwareName4), ref fLabwareName4, value); }
        }

        string fLabwareName5;
        [Size(100)]
        [Persistent(@"LabwareName5")]
        public string LabwareName5
        {
            get { return fLabwareName5; }
            set { SetPropertyValue<string>(nameof(LabwareName5), ref fLabwareName5, value); }
        }

        string fAssignedName1;
        [Size(100)]
        [Persistent(@"AssignedName1")]
        public string AssignedName1
        {
            get { return fAssignedName1; }
            set { SetPropertyValue<string>(nameof(AssignedName1), ref fAssignedName1, value); }
        }

        string fAssignedName2;
        [Size(100)]
        [Persistent(@"AssignedName2")]
        public string AssignedName2
        {
            get { return fAssignedName2; }
            set { SetPropertyValue<string>(nameof(AssignedName2), ref fAssignedName2, value); }
        }

        string fAssignedName3;
        [Size(100)]
        [Persistent(@"AssignedName3")]
        public string AssignedName3
        {
            get { return fAssignedName3; }
            set { SetPropertyValue<string>(nameof(AssignedName3), ref fAssignedName3, value); }
        }
        string fAssignedName4;
        [Size(100)]
        [Persistent(@"AssignedName4")]
        public string AssignedName4
        {
            get { return fAssignedName4; }
            set { SetPropertyValue<string>(nameof(AssignedName4), ref fAssignedName4, value); }
        }
        string fAssignedName5;
        [Size(100)]
        [Persistent(@"AssignedName5")]
        public string AssignedName5
        {
            get { return fAssignedName5; }
            set { SetPropertyValue<string>(nameof(AssignedName5), ref fAssignedName5, value); }
        }

        string fFileNumber1;
        [Size(100)]
        [Persistent(@"FileNumber1")]
        public string FileNumber1
        {
            get { return fFileNumber1; }
            set { SetPropertyValue<string>(nameof(FileNumber1), ref fFileNumber1, value); }
        }

        string fFileNumber2;
        [Size(100)]
        [Persistent(@"FileNumber2")]
        public string FileNumber2
        {
            get { return fFileNumber2; }
            set { SetPropertyValue<string>(nameof(FileNumber2), ref fFileNumber2, value); }
        }

        string fFileNumber3;
        [Size(100)]
        [Persistent(@"FileNumber3")]
        public string FileNumber3
        {
            get { return fFileNumber3; }
            set { SetPropertyValue<string>(nameof(FileNumber3), ref fFileNumber3, value); }
        }

        string fFileNumber4;
        [Size(100)]
        [Persistent(@"FileNumber4")]
        public string FileNumber4
        {
            get { return fFileNumber4; }
            set { SetPropertyValue<string>(nameof(FileNumber4), ref fFileNumber4, value); }
        }
        string fFileNumber5;
        [Size(100)]
        [Persistent(@"FileNumber5")]
        public string FileNumber5
        {
            get { return fFileNumber5; }
            set { SetPropertyValue<string>(nameof(FileNumber5), ref fFileNumber5, value); }
        }

        string fSpecification1;
        [Size(100)]
        [Persistent(@"Specification1")]
        public string Specification1
        {
            get { return fSpecification1; }
            set { SetPropertyValue<string>(nameof(Specification1), ref fSpecification1, value); }
        }

        string fSpecification2;
        [Size(100)]
        [Persistent(@"Specification2")]
        public string Specification2
        {
            get { return fSpecification2; }
            set { SetPropertyValue<string>(nameof(Specification2), ref fSpecification2, value); }
        }

        string fSpecification3;
        [Size(100)]
        [Persistent(@"Specification3")]
        public string Specification3
        {
            get { return fSpecification3; }
            set { SetPropertyValue<string>(nameof(Specification3), ref fSpecification3, value); }
        }

        string fSpecification4;
        [Size(100)]
        [Persistent(@"Specification4")]
        public string Specification4
        {
            get { return fSpecification4; }
            set { SetPropertyValue<string>(nameof(Specification4), ref fSpecification4, value); }
        }

        string fSpecification5;
        [Size(100)]
        [Persistent(@"Specification5")]
        public string Specification5
        {
            get { return fSpecification5; }
            set { SetPropertyValue<string>(nameof(Specification5), ref fSpecification5, value); }
        }

        DateTime fExpirationDate1;
        [Persistent(@"ExpirationDate1")]
        public DateTime ExpirationDate1
        {
            get { return fExpirationDate1; }
            set { SetPropertyValue<DateTime>(nameof(ExpirationDate1), ref fExpirationDate1, value); }
        }

        DateTime fExpirationDate2;
        [Persistent(@"ExpirationDate2")]
        public DateTime ExpirationDate2
        {
            get { return fExpirationDate2; }
            set { SetPropertyValue<DateTime>(nameof(ExpirationDate2), ref fExpirationDate2, value); }
        }

        DateTime fExpirationDate3;
        [Persistent(@"ExpirationDate3")]
        public DateTime ExpirationDate3
        {
            get { return fExpirationDate3; }
            set { SetPropertyValue<DateTime>(nameof(ExpirationDate3), ref fExpirationDate3, value); }
        }

        DateTime fExpirationDate4;
        [Persistent(@"ExpirationDate4")]
        public DateTime ExpirationDate4
        {
            get { return fExpirationDate4; }
            set { SetPropertyValue<DateTime>(nameof(ExpirationDate4), ref fExpirationDate4, value); }
        }

        DateTime fExpirationDate5;
        [Persistent(@"ExpirationDate5")]
        public DateTime ExpirationDate5
        {
            get { return fExpirationDate5; }
            set { SetPropertyValue<DateTime>(nameof(ExpirationDate5), ref fExpirationDate5, value); }
        }

        string fClientSampleID;
        [Size(100)]
        [Persistent(@"ClientSampleID")]
        public string ClientSampleID
        {
            get { return fClientSampleID; }
            set { SetPropertyValue<string>(nameof(ClientSampleID), ref fClientSampleID, value); }
        }

        string fSysSampleCode;
        [Size(100)]
        [Persistent(@"SysSampleCode")]
        public string SysSampleCode
        {
            get { return fSysSampleCode; }
            set { SetPropertyValue<string>(nameof(SysSampleCode), ref fSysSampleCode, value); }
        }

        string fMaterial;
        [Size(1000)]
        [Persistent(@"Material")]
        public string Material
        {
            get { return fMaterial; }
            set { SetPropertyValue<string>(nameof(Material), ref fMaterial, value); }
        }

        string fNAPositiveStop;
        [Persistent(@"NAPositiveStop")]
        [Size(100)]
        public string NAPositiveStop
        {
            get { return fNAPositiveStop; }
            set { SetPropertyValue<string>(nameof(NAPositiveStop), ref fNAPositiveStop, value); }
        }

        string fFriable;
        [Size(200)]
        [Persistent(@"Friable")]
        public string Friable
        {
            get { return fFriable; }
            set { SetPropertyValue<string>(nameof(Friable), ref fFriable, value); }
        }

        string fTexture;
        [Size(200)]
        [Persistent(@"Texture")]
        public string Texture
        {
            get { return fTexture; }
            set { SetPropertyValue<string>(nameof(Texture), ref fTexture, value); }
        }

        string fVisualGross;
        [Size(200)]
        [Persistent(@"VisualGross")]
        public string VisualGross
        {
            get { return fVisualGross; }
            set { SetPropertyValue<string>(nameof(VisualGross), ref fVisualGross, value); }
        }

        string fColor;
        [Size(2000)]
        [Persistent(@"Color")]
        public string Color
        {
            get { return fColor; }
            set { SetPropertyValue<string>(nameof(Color), ref fColor, value); }
        }

        string fNonAsbestosValue1;
        [Size(2000)]
        [Persistent(@"NonAsbestosValue1")]
        public string NonAsbestosValue1
        {
            get { return fNonAsbestosValue1; }
            set { SetPropertyValue<string>(nameof(NonAsbestosValue1), ref fNonAsbestosValue1, value); }
        }


        string fNonAsbestosType1;
        [Size(1000)]
        [Persistent(@"NonAsbestosType1")]
        public string NonAsbestosType1
        {
            get { return fNonAsbestosType1; }
            set { SetPropertyValue<string>(nameof(NonAsbestosType1), ref fNonAsbestosType1, value); }
        }

        string fNonAsbestosValue2;
        [Size(100)]
        [Persistent(@"NonAsbestosValue2")]
        public string NonAsbestosValue2
        {
            get { return fNonAsbestosValue2; }
            set { SetPropertyValue<string>(nameof(NonAsbestosValue2), ref fNonAsbestosValue2, value); }
        }


        string fNonAsbestosType2;
        [Size(1000)]
        [Persistent(@"NonAsbestosType2")]
        public string NonAsbestosType2
        {
            get { return fNonAsbestosType2; }
            set { SetPropertyValue<string>(nameof(NonAsbestosType2), ref fNonAsbestosType2, value); }
        }

        string fNonAsbestosValue3;
        [Size(100)]
        [Persistent(@"NonAsbestosValue3")]
        public string NonAsbestosValue3
        {
            get { return fNonAsbestosValue3; }
            set { SetPropertyValue<string>(nameof(NonAsbestosValue3), ref fNonAsbestosValue3, value); }
        }

        string fNonAsbestosType3;
        [Size(1000)]
        [Persistent(@"NonAsbestosType3")]
        public string NonAsbestosType3
        {
            get { return fNonAsbestosType3; }
            set { SetPropertyValue<string>(nameof(NonAsbestosType3), ref fNonAsbestosType3, value); }
        }

        string fNonAsbestosType4;
        [Size(1000)]
        [Persistent(@"NonAsbestosType4")]
        public string NonAsbestosType4
        {
            get { return fNonAsbestosType4; }
            set { SetPropertyValue<string>(nameof(NonAsbestosType4), ref fNonAsbestosType4, value); }
        }

        string fPointCount;
        [Size(50)]
        [Persistent(@"PointCount")]
        public string PointCount
        {
            get { return fPointCount; }
            set { SetPropertyValue<string>(nameof(PointCount), ref fPointCount, value); }
        }
        //Asbestos Type
        string fAsbestosValue1;
        [Size(100)]
        [Persistent(@"AsbestosValue1")]
        public string AsbestosValue1
        {
            get { return fAsbestosValue1; }
            set { SetPropertyValue<string>(nameof(AsbestosValue1), ref fAsbestosValue1, value); }
        }


        string fAsbestosType1;
        [Size(1000)]
        [Persistent(@"AsbestosType1")]
        public string AsbestosType1
        {
            get { return fAsbestosType1; }
            set { SetPropertyValue<string>(nameof(AsbestosType1), ref fAsbestosType1, value); }
        }

        string fAsbestosValue2;
        [Size(100)]
        [Persistent(@"AsbestosValue2")]
        public string AsbestosValue2
        {
            get { return fAsbestosValue2; }
            set { SetPropertyValue<string>(nameof(AsbestosValue2), ref fAsbestosValue2, value); }
        }

        string fAsbestosType2;
        [Size(1000)]
        [Persistent(@"AsbestosType2")]
        public string AsbestosType2
        {
            get { return fAsbestosType2; }
            set { SetPropertyValue<string>(nameof(AsbestosType2), ref fAsbestosType2, value); }
        }

        string fAsbestosValue3;
        [Size(100)]
        [Persistent(@"AsbestosValue3")]
        public string AsbestosValue3
        {
            get { return fAsbestosValue3; }
            set { SetPropertyValue<string>(nameof(AsbestosValue3), ref fAsbestosValue3, value); }
        }

        string fAsbestosType3;
        [Size(1000)]
        [Persistent(@"AsbestosType3")]
        public string AsbestosType3
        {
            get { return fAsbestosType3; }
            set { SetPropertyValue<string>(nameof(AsbestosType3), ref fAsbestosType3, value); }
        }

        string fNonFibrousValue1;
        [Size(100)]
        [Persistent(@"NonFibrousValue1")]
        public string NonFibrousValue1
        {
            get { return fNonFibrousValue1; }
            set { SetPropertyValue<string>(nameof(NonFibrousValue1), ref fNonFibrousValue1, value); }
        }

        string fNonFibrousType1;
        [Size(1000)]
        [Persistent(@"NonFibrousType1")]
        public string NonFibrousType1
        {
            get { return fNonFibrousType1; }
            set { SetPropertyValue<string>(nameof(NonFibrousType1), ref fNonFibrousType1, value); }
        }

        string fSampleLayerID;
        [Size(200)]
        [Persistent(@"SampleLayerID")]
        public string SampleLayerID
        {
            get { return fSampleLayerID; }
            set { SetPropertyValue<string>(nameof(SampleLayerID), ref fSampleLayerID, value); }
        }

        string fGratitudeArea;
        [Size(100)]
        [Persistent(@"GratitudeArea")]
        public string GratitudeArea
        {
            get { return fGratitudeArea; }
            set { SetPropertyValue<string>(nameof(GratitudeArea), ref fGratitudeArea, value); }
        }

        string fEffectiveFilterArea;
        [Size(100)]
        [Persistent(@"EffectiveFilterArea")]
        public string EffectiveFilterArea
        {
            get { return fEffectiveFilterArea; }
            set { SetPropertyValue<string>(nameof(EffectiveFilterArea), ref fEffectiveFilterArea, value); }
        }

        string fFibersCount;
        [Size(100)]
        [Persistent(@"FibersCount")]
        public string FibersCount
        {
            get { return fFibersCount; }
            set { SetPropertyValue<string>(nameof(FibersCount), ref fFibersCount, value); }
        }

        string fFieldsCount;
        [Size(100)]
        [Persistent(@"FieldsCount")]
        public string FieldsCount
        {
            get { return fFieldsCount; }
            set { SetPropertyValue<string>(nameof(FieldsCount), ref fFieldsCount, value); }
        }



        [NonPersistent]
        public string JobID
        {
            get
            {
                if (SampleID != null)
                {
                    string[] sample = SampleID.Split('-');
                    if (sample.Length > 0)
                    {
                        return sample[0];
                    }
                    else
                    {
                        return null;
                    }

                }
                return null;
            }

        }
    }

}
