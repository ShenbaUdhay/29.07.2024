using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.BusinessObjects.PLM_Quality_Control
{
    [DefaultClassOptions]
    public class PLMQualityControl : BaseObject
    {
        public PLMQualityControl(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region QCBatchID
        private SpreadSheetEntry_AnalyticalBatch _QCBatchID;

        public SpreadSheetEntry_AnalyticalBatch QCBatchID
        {
            get { return _QCBatchID; }
            set { SetPropertyValue(nameof(QCBatchID), ref _QCBatchID, value); }
        }
        #endregion
        #region QCType
        private QCType _QCType;

        public QCType QCType
        {
            get { return _QCType; }
            set { SetPropertyValue(nameof(QCType), ref _QCType, value); }
        }
        #endregion
        #region Analyst
        private string _Analyst;
        [RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Analyst
        {
            get { return _Analyst; }
            set { SetPropertyValue(nameof(Analyst), ref _Analyst, value); }
        }
        #endregion

        public XPCollection<Employee> AnalystDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Analyst))
                {
                    List<string> lstSM = new List<string>();
                    List<string> lstSMOid = Analyst.Split(';').ToList();
                    if (lstSMOid != null)
                    {
                        foreach (string objOid in lstSMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Employee objVM = Session.GetObjectByKey<Employee>(new Guid(objOid.Trim()));
                                if (objVM != null && !lstSM.Contains(objVM.FirstName))
                                {
                                    lstSM.Add(objVM.FirstName);
                                }
                            }

                        }
                    }
                    return new XPCollection<Employee>(Session, CriteriaOperator.Parse("", lstSM));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }

        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "Analyst" && AnalystDataSource != null && AnalystDataSource.Count > 0)
            {
                foreach (Employee objTest in AnalystDataSource.Where(i => i.FirstName != null).OrderBy(i => i.FirstName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objTest.FirstName))
                    {
                        Properties.Add(objTest.Oid, objTest.FirstName);
                    }
                }
            }
            return Properties;
        }

        #region AnalystFromDate
        private DateTime _AnalystFromDate;
        public DateTime AnalystFromDate
        {
            get { return _AnalystFromDate; }
            set { SetPropertyValue(nameof(AnalystFromDate), ref _AnalystFromDate, value); }
        }
        #endregion
        #region AnalystToDate
        private DateTime _AnalystToDate;
        public DateTime AnalystToDate
        {
            get { return _AnalystToDate; }
            set { SetPropertyValue(nameof(AnalystToDate), ref _AnalystToDate, value); }
        }
        #endregion
        #region ControlType
        private string _ControlType;

        public string ControlType
        {
            get { return _ControlType; }
            set { SetPropertyValue(nameof(ControlType), ref _ControlType, value); }
        }
        #endregion
        #region Category
        private string _Category;
        public string Category
        {
            get { return _Category; }
            set { SetPropertyValue(nameof(Category), ref _Category, value); }
        }
        #endregion
        #region Range
        private string _Range;
        public string Range
        {
            get { return _Range; }
            set { SetPropertyValue(nameof(Range), ref _Range, value); }
        }
        #endregion
        #region LCL
        private string _LCL;
        public string LCL
        {
            get { return _LCL; }
            set { SetPropertyValue(nameof(LCL), ref _LCL, value); }
        }
        #endregion
        #region UCL
        private string _UCL;
        public string UCL
        {
            get { return _UCL; }
            set { SetPropertyValue(nameof(UCL), ref _UCL, value); }
        }
        #endregion
        #region EnteredBy
        private string _EnteredBy;
        public string EnteredBy
        {
            get { return _EnteredBy; }
            set { SetPropertyValue(nameof(EnteredBy), ref _EnteredBy, value); }
        }
        #endregion
        #region EnteredDate
        private DateTime _EnteredDate;
        public DateTime EnteredDate
        {
            get { return _EnteredDate; }
            set { SetPropertyValue(nameof(EnteredDate), ref _EnteredDate, value); }
        }
        #endregion
        #region UpdateLog
        private bool _UpdateLog;
        public bool UpdateLog
        {
            get { return _UpdateLog; }
            set { SetPropertyValue(nameof(UpdateLog), ref _UpdateLog, value); }
        }
        #endregion


        #region QCAnalysis(IntraDup)tbl_ENV_ResultPLMQC


        //private Guid _PLMResultQCOid;

        //public Guid PLMResultQCOid
        //{
        //    get { return _PLMResultQCOid; }
        //    set { SetPropertyValue(nameof(PLMResultQCOid), ref _PLMResultQCOid, value); }
        //}

        //private QCType _QCTypeID;

        //public QCType QCTypeID
        //{
        //    get { return _QCTypeID; }
        //    set { SetPropertyValue(nameof(QCTypeID), ref _QCTypeID, value); }
        //}

        private string _UserSampleLNO;
        public string UserSampleLNO
        {
            get { return _UserSampleLNO; }
            set { SetPropertyValue(nameof(UserSampleLNO), ref _UserSampleLNO, value); }
        }

        //private Employee _AnalysedBy;
        //public Employee AnalysedBy
        //{
        //    get { return _AnalysedBy; }
        //    set { SetPropertyValue(nameof(AnalysedBy), ref _AnalysedBy, value); }
        //}


        private DateTime _AnalyzedDateQC;

        public DateTime AnalyzedDateQC
        {
            get { return _AnalyzedDateQC; }
            set { SetPropertyValue<DateTime>("AnalyzedDate", ref _AnalyzedDateQC, value); }

        }

        //private string _ResultStatus;
        //public string ResultStatus
        //{
        //    get { return _ResultStatus; }
        //    set { SetPropertyValue(nameof(ResultStatus), ref _ResultStatus, value); }
        //}

        //private string _ResultType;
        //public string ResultType
        //{
        //    get { return _ResultType; }
        //    set { SetPropertyValue(nameof(ResultType), ref _ResultType, value); }
        //}
        //private Employee _ValidatedBy;
        //public Employee ValidatedBy
        //{
        //    get { return _ValidatedBy; }
        //    set { SetPropertyValue(nameof(ValidatedBy), ref _ValidatedBy, value); }
        //}

        //private DateTime _ValidatedDate;

        //public DateTime ValidatedDate
        //{
        //    get { return _ValidatedDate; }
        //    set { SetPropertyValue<DateTime>("ValidatedDate", ref _ValidatedDate, value); }

        //}

        //private CustomSystemUser _ApprovedBy;
        //public CustomSystemUser ApprovedBy
        //{
        //    get { return _ApprovedBy; }
        //    set { SetPropertyValue("ApprovedBy", ref _ApprovedBy, value); }
        //}

        //private DateTime _ApprovedDate;

        //public DateTime ApprovedDate
        //{
        //    get { return _ApprovedDate; }
        //    set { SetPropertyValue<DateTime>("ApprovedDate", ref _ApprovedDate, value); }

        //}

        //private string _MaterialID;
        //public string MaterialID
        //{
        //    get { return _MaterialID; }
        //    set { SetPropertyValue(nameof(MaterialID), ref _MaterialID, value); }
        //}

        //private string _TextureGranular;
        //public string TextureGranular
        //{
        //    get { return _MaterialID; }
        //    set { SetPropertyValue(nameof(TextureGranular), ref _TextureGranular, value); }
        //}

        //private string _TextureTar;
        //public string TextureTar
        //{
        //    get { return _TextureTar; }
        //    set { SetPropertyValue(nameof(TextureTar), ref _TextureTar, value); }
        //}

        //private string _TextureFoam;
        //public string TextureFoam
        //{
        //    get { return _TextureTar; }
        //    set { SetPropertyValue(nameof(TextureFoam), ref _TextureFoam, value); }
        //}

        //private string _TextureFibrous;
        //public string TextureFibrous
        //{
        //    get { return _TextureTar; }
        //    set { SetPropertyValue(nameof(TextureFibrous), ref _TextureFibrous, value); }
        //}

        //private string _TextureVinyl;
        //public string TextureVinyl
        //{
        //    get { return _TextureVinyl; }
        //    set { SetPropertyValue(nameof(TextureVinyl), ref _TextureVinyl, value); }
        //}

        //private string _TextureOther;
        //public string TextureOther
        //{
        //    get { return _TextureOther; }
        //    set { SetPropertyValue(nameof(TextureOther), ref _TextureOther, value); }
        //}

        //private string _Homogenous;
        //public string Homogenous
        //{
        //    get { return _Homogenous; }
        //    set { SetPropertyValue(nameof(Homogenous), ref _Homogenous, value); }
        //}

        //private string _NonHomogenous;
        //public string NonHomogenous
        //{
        //    get { return _NonHomogenous; }
        //    set { SetPropertyValue(nameof(NonHomogenous), ref _NonHomogenous, value); }
        //}


        //private string _Layered;
        //public string Layered
        //{
        //    get { return _Layered; }
        //    set { SetPropertyValue(nameof(Layered), ref _Layered, value); }
        //}

        //private string _HomogenetyOther;
        //public string HomogenetyOther
        //{
        //    get { return _HomogenetyOther; }
        //    set { SetPropertyValue(nameof(HomogenetyOther), ref _HomogenetyOther, value); }
        //}

        //private string _ColorBlack;
        //public string ColorBlack
        //{
        //    get { return _ColorBlack; }
        //    set { SetPropertyValue(nameof(ColorBlack), ref _ColorBlack, value); }
        //}

        //private string _ColorTan;
        //public string ColorTan
        //{
        //    get { return _ColorBlack; }
        //    set { SetPropertyValue(nameof(ColorTan), ref _ColorTan, value); }
        //}


        //private string _ColorWhite;
        //public string ColorWhite
        //{
        //    get { return _ColorWhite; }
        //    set { SetPropertyValue(nameof(ColorWhite), ref _ColorWhite, value); }
        //}

        //private string _ColorGreen;
        //public string ColorGreen
        //{
        //    get { return _ColorGreen; }
        //    set { SetPropertyValue(nameof(ColorGreen), ref _ColorGreen, value); }
        //}

        //private string _ColorPink;
        //public string ColorPink
        //{
        //    get { return _ColorPink; }
        //    set { SetPropertyValue(nameof(ColorPink), ref _ColorPink, value); }
        //}

        //private string _ColorBlue;
        //public string ColorBlue
        //{
        //    get { return _ColorBlue; }
        //    set { SetPropertyValue(nameof(ColorBlue), ref _ColorBlue, value); }
        //}

        //private string _ColorBrown;
        //public string ColorBrown
        //{
        //    get { return _ColorBlue; }
        //    set { SetPropertyValue(nameof(ColorBrown), ref _ColorBrown, value); }
        //}

        //private string _ColorYellow;
        //public string ColorYellow
        //{
        //    get { return _ColorBlue; }
        //    set { SetPropertyValue(nameof(ColorYellow), ref _ColorYellow, value); }
        //}

        //private string _ColorRed;
        //public string ColorRed
        //{
        //    get { return _ColorBlue; }
        //    set { SetPropertyValue(nameof(ColorRed), ref _ColorRed, value); }
        //}


        //private string _ColorSilver;
        //public string ColorSilver
        //{
        //    get { return _ColorSilver; }
        //    set { SetPropertyValue(nameof(ColorSilver), ref _ColorSilver, value); }
        //}

        //private string _ColorGray;
        //public string ColorGray
        //{
        //    get { return _ColorGray; }
        //    set { SetPropertyValue(nameof(ColorGray), ref _ColorGray, value); }
        //}

        //private string _ColorOther;
        //public string ColorOther
        //{
        //    get { return _ColorOther; }
        //    set { SetPropertyValue(nameof(ColorOther), ref _ColorOther, value); }
        //}

        //private string _StereoScopicExamination;
        //public string StereoScopicExamination
        //{
        //    get { return _StereoScopicExamination; }
        //    set { SetPropertyValue(nameof(StereoScopicExamination), ref _StereoScopicExamination, value); }
        //}

        //private string _MatrixThermal;
        //public string MatrixThermal
        //{
        //    get { return _MatrixThermal; }
        //    set { SetPropertyValue(nameof(MatrixThermal), ref _MatrixThermal, value); }
        //}

        //private string _MatrixAcid;
        //public string MatrixAcid
        //{
        //    get { return _MatrixAcid; }
        //    set { SetPropertyValue(nameof(MatrixAcid), ref _MatrixAcid, value); }
        //}

        //private string _MatrixSolvent;
        //public string MatrixSolvent
        //{
        //    get { return _MatrixSolvent; }
        //    set { SetPropertyValue(nameof(MatrixSolvent), ref _MatrixSolvent, value); }
        //}

        //private string _MatrixFriable;
        //public string MatrixFriable
        //{
        //    get { return _MatrixFriable; }
        //    set { SetPropertyValue(nameof(MatrixFriable), ref _MatrixFriable, value); }
        //}

        //private string _MatrixNonFriable;
        //public string MatrixNonFriable
        //{
        //    get { return _MatrixNonFriable; }
        //    set { SetPropertyValue(nameof(MatrixNonFriable), ref _MatrixNonFriable, value); }
        //}

        //private string _MatrixReduction;
        //public string MatrixReduction
        //{
        //    get { return _MatrixReduction; }
        //    set { SetPropertyValue(nameof(MatrixReduction), ref _MatrixReduction, value); }
        //}

        //private string _HCLReaction;
        //public string HCLReaction
        //{
        //    get { return _HCLReaction; }
        //    set { SetPropertyValue(nameof(HCLReaction), ref _HCLReaction, value); }
        //}

        //private string _PLMExamination;
        //public string PLMExamination
        //{
        //    get { return _PLMExamination; }
        //    set { SetPropertyValue(nameof(PLMExamination), ref _PLMExamination, value); }
        //}

        //private string _LabReportComment;
        //public string LabReportComment
        //{
        //    get { return _LabReportComment; }
        //    set { SetPropertyValue(nameof(LabReportComment), ref _LabReportComment, value); }
        //}

        //private string _ResultDescription;
        //public string ResultDescription
        //{
        //    get { return _ResultDescription; }
        //    set { SetPropertyValue(nameof(LabReportComment), ref _ResultDescription, value); }
        //}

        //private string _AsbestosDetected;
        //public string AsbestosDetected
        //{
        //    get { return _AsbestosDetected; }
        //    set { SetPropertyValue(nameof(AsbestosDetected), ref _AsbestosDetected, value); }
        //}

        private string _FiberChrysotileQC;
        public string FiberChrysotileQC
        {
            get { return _FiberChrysotileQC; }
            set { SetPropertyValue(nameof(FiberChrysotileQC), ref _FiberChrysotileQC, value); }
        }

        private string _FiberAmositeQC;
        public string FiberAmositeQC
        {
            get { return _FiberAmositeQC; }
            set { SetPropertyValue(nameof(FiberAmosite), ref _FiberAmositeQC, value); }
        }

        private string _FiberCrocidioliteQC;
        public string FiberCrocidioliteQC
        {
            get { return _FiberCrocidioliteQC; }
            set { SetPropertyValue(nameof(FiberCrocidioliteQC), ref _FiberCrocidioliteQC, value); }
        }

        private string _FiberTremoliteQC;
        public string FiberTremoliteQC
        {
            get { return _FiberTremoliteQC; }
            set { SetPropertyValue(nameof(FiberTremoliteQC), ref _FiberTremoliteQC, value); }
        }

        private string _FiberAnthrophytileQC;
        public string FiberAnthrophytileQC
        {
            get { return _FiberAnthrophytileQC; }
            set { SetPropertyValue(nameof(FiberAnthrophytileQC), ref _FiberAnthrophytileQC, value); }
        }

        private string _FiberActrinoliteQC;
        public string FiberActrinoliteQC
        {
            get { return _FiberActrinoliteQC; }
            set { SetPropertyValue(nameof(FiberActrinoliteQC), ref _FiberActrinoliteQC, value); }
        }

        //private string _ReportStatus;
        //public string ReportStatus
        //{
        //    get { return _ReportStatus; }
        //    set { SetPropertyValue(nameof(ReportStatus), ref _ReportStatus, value); }
        //}

        #endregion
        #region QCAnalysis(IntraDup)tbl_ENV_ResultPLM

        private DateTime _AnalyzedDate;


        private Guid _PLMResultID;
        public Guid PLMResultID
        {
            get { return _PLMResultID; }
            set { SetPropertyValue(nameof(PLMResultID), ref _PLMResultID, value); }
        }
        public DateTime AnalyzedDate
        {
            get { return _AnalyzedDate; }
            set { SetPropertyValue<DateTime>("AnalyzedDate", ref _AnalyzedDate, value); }

        }
        private string _Actinolite;
        public string Actinolite
        {
            get { return _Actinolite; }
            set { SetPropertyValue(nameof(Actinolite), ref _Actinolite, value); }
        }

        private string _FiberChrysotile;
        public string FiberChrysotile
        {
            get { return _FiberChrysotile; }
            set { SetPropertyValue(nameof(FiberChrysotile), ref _FiberChrysotile, value); }
        }

        private string _FiberAmosite;
        public string FiberAmosite
        {
            get { return _FiberAmosite; }
            set { SetPropertyValue(nameof(FiberAmosite), ref _FiberAmosite, value); }
        }

        private string _FiberCrocidiolite;
        public string FiberCrocidiolite
        {
            get { return _FiberCrocidiolite; }
            set { SetPropertyValue(nameof(FiberCrocidiolite), ref _FiberCrocidiolite, value); }
        }

        private string _FiberTremolite;
        public string FiberTremolite
        {
            get { return _FiberTremolite; }
            set { SetPropertyValue(nameof(FiberTremolite), ref _FiberTremolite, value); }
        }

        private string _FiberAnthrophytile;
        public string FiberAnthrophytile
        {
            get { return _FiberAnthrophytile; }
            set { SetPropertyValue(nameof(FiberAnthrophytile), ref _FiberAnthrophytile, value); }
        }

        private string _FiberActrinolite;
        public string FiberActrinolite
        {
            get { return _FiberActrinolite; }
            set { SetPropertyValue(nameof(FiberActrinolite), ref _FiberActrinolite, value); }
        }

        private string _ReportStatus;
        public string ReportStatus
        {
            get { return _ReportStatus; }
            set { SetPropertyValue(nameof(ReportStatus), ref _ReportStatus, value); }
        }

        #endregion
    }
}