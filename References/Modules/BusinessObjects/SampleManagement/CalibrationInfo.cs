using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SDMS;
using Modules.BusinessObjects.Setting;
using System;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    //[NonPersistent]
    [Persistent("SpreadSheetEntry_CalibrationInfo")]
    public class CalibrationInfo : BaseObject
    {
        public CalibrationInfo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (uqID == 0)
            {
                uqID = Convert.ToInt32(Session.Evaluate(typeof(CalibrationInfo), CriteriaOperator.Parse("Max(uqID)"), null)) + 1;
            }
        }

        #region uqID
        int fuqID;
        public int uqID
        {
            get { return fuqID; }
            set { SetPropertyValue<int>(nameof(uqID), ref fuqID, value); }
        }
        #endregion

        #region uqCalibrationID
        int fuqCalibrationID;
        public int uqCalibrationID
        {
            get { return fuqCalibrationID; }
            set { SetPropertyValue<int>(nameof(uqCalibrationID), ref fuqCalibrationID, value); }
        }
        #endregion

        #region Calibration
        Calibration fCalibration;
        [Association("Calibration-CalibrationInfo")]
        public Calibration Calibration
        {
            get
            {
                return fCalibration;
            }
            set
            {
                SetPropertyValue<Calibration>(nameof(Calibration), ref fCalibration, value);
                if (value != null)
                {
                    uqCalibrationID = value.uqID;
                }
            }
        }
        #endregion

        #region uqTemplateID
        SpreadSheetBuilder_TemplateInfo fuqTemplateID;
        public SpreadSheetBuilder_TemplateInfo uqTemplateID
        {
            get { return fuqTemplateID; }
            set { SetPropertyValue<SpreadSheetBuilder_TemplateInfo>(nameof(uqTemplateID), ref fuqTemplateID, value); }
        }
        #endregion

        #region uqTestParameterID
        Testparameter fuqTestParameterID;
        public Testparameter uqTestParameterID
        {
            get { return fuqTestParameterID; }
            set { SetPropertyValue<Testparameter>(nameof(uqTestParameterID), ref fuqTestParameterID, value); }
        }
        #endregion

        #region LevelNo
        int fLevelNo;
        public int LevelNo
        {
            get { return fLevelNo; }
            set { SetPropertyValue<int>(nameof(LevelNo), ref fLevelNo, value); }
        }
        #endregion

        #region CalibratedDate
        DateTime fCalibratedDate;
        public DateTime CalibratedDate
        {
            get { return fCalibratedDate; }
            set { SetPropertyValue<DateTime>(nameof(CalibratedDate), ref fCalibratedDate, value); }
        }
        #endregion

        #region CalibratedBy
        CustomSystemUser fCalibratedBy;
        public CustomSystemUser CalibratedBy
        {
            get { return fCalibratedBy; }
            set { SetPropertyValue<CustomSystemUser>(nameof(CalibratedBy), ref fCalibratedBy, value); }
        }
        #endregion

        #region Conc
        string fConc;
        [Size(50)]
        public string Conc
        {
            get { return fConc; }
            set { SetPropertyValue<string>(nameof(Conc), ref fConc, value); }
        }
        #endregion

        #region Concentration
        [NonPersistent]
        [Size(50)]
        public double Concentration
        {
            get
            {
                if (string.IsNullOrEmpty(Conc))
                {
                    return double.MinValue;
                }
                else
                {
                    return Convert.ToDouble(Conc);
                }
            }
        }
        #endregion

        #region Absorb
        [NonPersistent]
        [Size(50)]
        public double Absorb
        {
            get
            {
                if (string.IsNullOrEmpty(ABSORBANCE))
                {
                    return double.MinValue;
                }
                else
                {
                    return Convert.ToDouble(ABSORBANCE);
                }
            }
        }
        #endregion

        #region FinalVolume
        string fFinalVolume;
        [Size(50)]
        public string FinalVolume
        {
            get { return fFinalVolume; }
            set { SetPropertyValue<string>(nameof(FinalVolume), ref fFinalVolume, value); }
        }
        #endregion

        #region Ct_mol_L_
        string fCt_mol_L_;
        [Size(50)]
        [Persistent(@"Ct (mol/L)")]
        public string Ct_mol_L_
        {
            get { return fCt_mol_L_; }
            set { SetPropertyValue<string>(nameof(Ct_mol_L_), ref fCt_mol_L_, value); }
        }
        #endregion

        #region F
        string fF;
        [Size(50)]
        public string F
        {
            get { return fF; }
            set { SetPropertyValue<string>(nameof(F), ref fF, value); }
        }
        #endregion

        #region Intercept
        string fIntercept;
        [Size(50)]
        public string Intercept
        {
            get { return fIntercept; }
            set { SetPropertyValue<string>(nameof(Intercept), ref fIntercept, value); }
        }
        #endregion

        #region Titrant1
        string fTitrant1;
        [Size(50)]
        public string Titrant1
        {
            get { return fTitrant1; }
            set { SetPropertyValue<string>(nameof(Titrant1), ref fTitrant1, value); }
        }
        #endregion

        #region Titrant2
        string fTitrant2;
        [Size(50)]
        public string Titrant2
        {
            get { return fTitrant2; }
            set { SetPropertyValue<string>(nameof(Titrant2), ref fTitrant2, value); }
        }
        #endregion

        #region Parameter
        string fParameter;
        [Size(50)]
        public string Parameter
        {
            get { return fParameter; }
            set { SetPropertyValue<string>(nameof(Parameter), ref fParameter, value); }
        }
        #endregion

        #region RCAP2
        string fRCAP2;
        [Size(50)]
        public string RCAP2
        {
            get { return fRCAP2; }
            set { SetPropertyValue<string>(nameof(RCAP2), ref fRCAP2, value); }
        }
        #endregion

        #region Reading
        string fReading;
        [Size(50)]
        public string Reading
        {
            get { return fReading; }
            set { SetPropertyValue<string>(nameof(Reading), ref fReading, value); }
        }
        #endregion

        string fSlope;
        [Size(50)]
        public string Slope
        {
            get { return fSlope; }
            set { SetPropertyValue<string>(nameof(Slope), ref fSlope, value); }
        }
        string fVS;
        [Size(50)]
        public string VS
        {
            get { return fVS; }
            set { SetPropertyValue<string>(nameof(VS), ref fVS, value); }
        }
        string fVC1;
        [Size(50)]
        public string VC1
        {
            get { return fVC1; }
            set { SetPropertyValue<string>(nameof(VC1), ref fVC1, value); }
        }
        string fVC2;
        [Size(50)]
        public string VC2
        {
            get { return fVC2; }
            set { SetPropertyValue<string>(nameof(VC2), ref fVC2, value); }
        }
        string fVC;
        [Size(50)]
        public string VC
        {
            get { return fVC; }
            set { SetPropertyValue<string>(nameof(VC), ref fVC, value); }
        }
        string fABSORBANCE;
        [Size(50)]
        public string ABSORBANCE
        {
            get { return fABSORBANCE; }
            set { SetPropertyValue<string>(nameof(ABSORBANCE), ref fABSORBANCE, value); }
        }
        string fUnit;
        [Size(50)]
        public string Unit
        {
            get { return fUnit; }
            set { SetPropertyValue<string>(nameof(Unit), ref fUnit, value); }
        }
        string fAssignedpH;
        [Size(50)]
        public string AssignedpH
        {
            get { return fAssignedpH; }
            set { SetPropertyValue<string>(nameof(AssignedpH), ref fAssignedpH, value); }
        }
        string fReadingAfter;
        [Size(50)]
        public string ReadingAfter
        {
            get { return fReadingAfter; }
            set { SetPropertyValue<string>(nameof(ReadingAfter), ref fReadingAfter, value); }
        }
        string fReadingBefore;
        [Size(50)]
        public string ReadingBefore
        {
            get { return fReadingBefore; }
            set { SetPropertyValue<string>(nameof(ReadingBefore), ref fReadingBefore, value); }
        }
        string fReagentID;
        [Size(50)]
        public string ReagentID
        {
            get { return fReagentID; }
            set { SetPropertyValue<string>(nameof(ReagentID), ref fReagentID, value); }
        }
        string fBuffer;
        [Size(50)]
        public string Buffer
        {
            get { return fBuffer; }
            set { SetPropertyValue<string>(nameof(Buffer), ref fBuffer, value); }
        }
        string fCurveLimit;
        [Size(50)]
        public string CurveLimit
        {
            get { return fCurveLimit; }
            set { SetPropertyValue<string>(nameof(CurveLimit), ref fCurveLimit, value); }
        }
        string fMDL;
        [Size(50)]
        public string MDL
        {
            get { return fMDL; }
            set { SetPropertyValue<string>(nameof(MDL), ref fMDL, value); }
        }
        string fStdConc;
        [Size(50)]
        public string StdConc
        {
            get { return fStdConc; }
            set { SetPropertyValue<string>(nameof(StdConc), ref fStdConc, value); }
        }
        string fStdConcVolUsed;
        [Size(50)]
        public string StdConcVolUsed
        {
            get { return fStdConcVolUsed; }
            set { SetPropertyValue<string>(nameof(StdConcVolUsed), ref fStdConcVolUsed, value); }
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
        Guid fParameterID;
        public Guid ParameterID
        {
            get { return fParameterID; }
            set { SetPropertyValue<Guid>(nameof(ParameterID), ref fParameterID, value); }
        }
        string fUserDefined21;
        [Size(50)]
        public string UserDefined21
        {
            get { return fUserDefined21; }
            set { SetPropertyValue<string>(nameof(UserDefined21), ref fUserDefined21, value); }
        }
        string fUserDefined22;
        [Size(50)]
        public string UserDefined22
        {
            get { return fUserDefined22; }
            set { SetPropertyValue<string>(nameof(UserDefined22), ref fUserDefined22, value); }
        }
        string fUserDefined23;
        [Size(50)]
        public string UserDefined23
        {
            get { return fUserDefined23; }
            set { SetPropertyValue<string>(nameof(UserDefined23), ref fUserDefined23, value); }
        }
        string fUserDefined24;
        [Size(50)]
        public string UserDefined24
        {
            get { return fUserDefined24; }
            set { SetPropertyValue<string>(nameof(UserDefined24), ref fUserDefined24, value); }
        }
        string fUserDefined25;
        [Size(50)]
        public string UserDefined25
        {
            get { return fUserDefined25; }
            set { SetPropertyValue<string>(nameof(UserDefined25), ref fUserDefined25, value); }
        }
        string fUserDefined26;
        [Size(50)]
        public string UserDefined26
        {
            get { return fUserDefined26; }
            set { SetPropertyValue<string>(nameof(UserDefined26), ref fUserDefined26, value); }
        }
        string fUserDefined27;
        [Size(50)]
        public string UserDefined27
        {
            get { return fUserDefined27; }
            set { SetPropertyValue<string>(nameof(UserDefined27), ref fUserDefined27, value); }
        }
        string fUserDefined28;
        [Size(50)]
        public string UserDefined28
        {
            get { return fUserDefined28; }
            set { SetPropertyValue<string>(nameof(UserDefined28), ref fUserDefined28, value); }
        }
        string fUserDefined29;
        [Size(50)]
        public string UserDefined29
        {
            get { return fUserDefined29; }
            set { SetPropertyValue<string>(nameof(UserDefined29), ref fUserDefined29, value); }
        }
        string fUserDefined30;
        [Size(50)]
        public string UserDefined30
        {
            get { return fUserDefined30; }
            set { SetPropertyValue<string>(nameof(UserDefined30), ref fUserDefined30, value); }
        }
        string fUserDefined31;
        [Size(50)]
        public string UserDefined31
        {
            get { return fUserDefined31; }
            set { SetPropertyValue<string>(nameof(UserDefined31), ref fUserDefined31, value); }
        }
        string fUserDefined32;
        [Size(50)]
        public string UserDefined32
        {
            get { return fUserDefined32; }
            set { SetPropertyValue<string>(nameof(UserDefined32), ref fUserDefined32, value); }
        }
        string fUserDefined33;
        [Size(50)]
        public string UserDefined33
        {
            get { return fUserDefined33; }
            set { SetPropertyValue<string>(nameof(UserDefined33), ref fUserDefined33, value); }
        }
        string fUserDefined34;
        [Size(50)]
        public string UserDefined34
        {
            get { return fUserDefined34; }
            set { SetPropertyValue<string>(nameof(UserDefined34), ref fUserDefined34, value); }
        }
        string fUserDefined35;
        [Size(50)]
        public string UserDefined35
        {
            get { return fUserDefined35; }
            set { SetPropertyValue<string>(nameof(UserDefined35), ref fUserDefined35, value); }
        }

    }
}