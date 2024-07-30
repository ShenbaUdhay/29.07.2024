using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.QC;
using Container = Modules.BusinessObjects.Setting.Container;

namespace Modules.BusinessObjects.SamplingManagement
{
    [DefaultClassOptions]
    public class SamplingParameter : BaseObject
    {
        curlanguage curlanguage = new curlanguage();
        public SamplingParameter(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #region TAT
        private TurnAroundTime _TAT;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [ImmediatePostData]
        public TurnAroundTime TAT
        {
            get { return _TAT; }
            set { SetPropertyValue<TurnAroundTime>("TAT", ref _TAT, value); }

        }
        #endregion

        #region CreatedDate

        private DateTime _CreatedDate;
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue<DateTime>("CreatedDate", ref _CreatedDate, value); }

        }
        #endregion

        #region CreatedBy
        private Employee _CreatedBy;
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue<Employee>("CreatedBy", ref _CreatedBy, value); }
        }
        #endregion

        #region SampleArea
        private string _SampleArea;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string SampleArea
        {

            get { return _SampleArea; }

            set { SetPropertyValue<string>("SampleArea", ref _SampleArea, value); }
        }
        #endregion

        #region FinalVolume
        private string _FinalVolume;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string FinalVolume
        {

            get { return _FinalVolume; }

            set { SetPropertyValue<string>("FinalVolume", ref _FinalVolume, value); }
        }
        #endregion

        #region ugsample
        private string _ugsample;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ugsample
        {

            get { return _ugsample; }

            set { SetPropertyValue<string>("ugsample", ref _ugsample, value); }
        }
        #endregion

        #region ugft
        private string _ugft;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ugft
        {

            get { return _ugft; }

            set { SetPropertyValue<string>("ugft", ref _ugft, value); }
        }
        #endregion

        #region ugmcube
        private string _ugmcube;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string ugmcube
        {

            get { return _ugmcube; }

            set { SetPropertyValue<string>("ugmcube", ref _ugmcube, value); }
        }
        #endregion

        #region TWA
        private string _TWA;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string TWA
        {

            get { return _TWA; }

            set { SetPropertyValue<string>("TWA", ref _TWA, value); }
        }
        #endregion

        #region FinalRptLimit
        private string _FinalRptLimit;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string FinalRptLimit
        {

            get { return _FinalRptLimit; }

            set { SetPropertyValue<string>("FinalRptLimit", ref _FinalRptLimit, value); }
        }
        #endregion

        #region FinalRptLimitpercentage
        private string _FinalRptLimitpercentage;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string FinalRptLimitpercentage
        {

            get { return _FinalRptLimitpercentage; }

            set { SetPropertyValue<string>("FinalRptLimitpercentage", ref _FinalRptLimitpercentage, value); }
        }
        #endregion



        //#region Sampling      
        //private Sampling _Sampling;
        //[Association]
        //public Sampling Sampling
        //{
        //    get { return _Sampling; }
        //    set { SetPropertyValue("Sampling", ref _Sampling, value); }
        //}
        //#endregion Samplelogin

        #region TestParameter
        private Testparameter _Testparameter;
        [Association]
        public Testparameter Testparameter
        {
            get { return _Testparameter; }
            set { SetPropertyValue("Testparameter", ref _Testparameter, value); }
        }
        #endregion TestParameter

        #region Result

        private string _Result;
        //[ImmediatePostData]
        public string Result
        {
            get
            {
                if (_Result == null && Testparameter != null && SignOff == true)
                {

                    return Testparameter.DefaultResult;
                }
                return _Result;
            }
            set
            {
                SetPropertyValue("Result", ref _Result, value);
                //OnChanged("ResultNumeric");
            }
        }
        #endregion Result

        #region EnteredDate
        private DateTime? _EnteredDate;
        public DateTime? EnteredDate
        {
            get
            {
                return _EnteredDate;
            }
            set
            {
                SetPropertyValue<DateTime?>("EnteredDate", ref _EnteredDate, value);
            }
        }
        #endregion

        #region EnteredBy
        private Employee _EnteredBy;
        public Employee EnteredBy
        {
            get
            { //_EnteredBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _EnteredBy;
            }
            set
            {
                SetPropertyValue<Employee>("EnteredBy", ref _EnteredBy, value);
            }

        }


        #endregion

        #region ValidatedDate
        private DateTime? _ValidatedDate;
        public DateTime? ValidatedDate
        {
            get { return _ValidatedDate; }
            set { SetPropertyValue<DateTime?>("ValidatedDate", ref _ValidatedDate, value); }
        }
        #endregion

        #region ValidatedBy
        private Employee _ValidatedBy;
        public Employee ValidatedBy
        {
            get
            {//_ValidatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _ValidatedBy;
            }
            set { SetPropertyValue<Employee>("ValidatedBy", ref _ValidatedBy, value); }
        }
        #endregion

        #region ApprovedDate
        private DateTime? _ApprovedDate;
        public DateTime? ApprovedDate
        {
            get { return _ApprovedDate; }
            set { SetPropertyValue<DateTime?>("ApprovedDate", ref _ApprovedDate, value); }
        }
        #endregion

        #region ApprovedBy
        private Employee _ApprovedBy;
        public Employee ApprovedBy
        {
            get
            {// _ApprovedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _ApprovedBy;
            }
            set { SetPropertyValue<Employee>("ApprovedBy", ref _ApprovedBy, value); }
        }
        #endregion

        #region AnanyzedDate
        private DateTime? _AnalyzedDate;
        public DateTime? AnalyzedDate
        {
            get
            {
                return _AnalyzedDate;
            }
            set { SetPropertyValue<DateTime?>("AnalyzedDate", ref _AnalyzedDate, value); }
        }
        #endregion

        #region SCDateReceived
        private DateTime? _SCDateReceived;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        public DateTime? SCDateReceived
        {
            get
            {
                if (Sampling != null)
                {
                    _SCDateReceived = Sampling.SamplingProposal.RecievedDate;
                }
                else if (QCBatchID != null && QCBatchID.SampleID != null)
                {
                    _SCDateReceived = QCBatchID.SampleID.JobID.RecievedDate;
                }
                return _SCDateReceived;
            }
            set { SetPropertyValue<DateTime?>("SCDateReceived", ref _SCDateReceived, value); }
        }
        #endregion

        #region AnalyzedBy
        private Employee _AnalyzedBy;
        [ImmediatePostData]
        public Employee AnalyzedBy
        {
            get
            {// _AnalyzedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);

                return _AnalyzedBy;
            }
            set
            {
                SetPropertyValue<Employee>("AnalyzedBy", ref _AnalyzedBy, value);
            }
        }
        #endregion

        #region Units
        private Unit _Units;
        public Unit Units
        {
            get
            {
                if ((_Units == null) && (_Testparameter != null))
                {
                    return Testparameter.DefaultUnits;
                }
                return _Units;
            }
            set { SetPropertyValue<Unit>("Units", ref _Units, value); }
        }
        #endregion

        #region Status
        private Samplestatus _Status;
        [DefaultValue(0)]
        public Samplestatus Status
        {
            get
            {
                return _Status;

            }
            set { SetPropertyValue<Samplestatus>("Status", ref _Status, value); }
        }
        #endregion

        #region DF
        private string _DF;
        public string DF
        {
            get { return _DF; }

            set { SetPropertyValue<string>("DF", ref _DF, value); }
        }

        #endregion

        #region ResultNumeric

       

        private string _ResultNumeric;
        //[ImmediatePostData(true)]
        public string ResultNumeric
        {

            get { return _ResultNumeric; }
            set { SetPropertyValue<string>("ResultNumeric", ref _ResultNumeric, value); }
        }

        #endregion

        #region SpikeAmount
        
        #endregion

        #region Rec
        private string _Rec;
        public string Rec
        {
            get { return _Rec; }

            set { SetPropertyValue<string>("Rec", ref _Rec, value); }
        }
        #endregion

        #region RPD
        private string _RPD;
        public string RPD
        {
            get { return _RPD; }

            set { SetPropertyValue<string>("RPD", ref _RPD, value); }
        }
        #endregion

        #region MDL
        private string _MDL;
        public string MDL
        {

            get
            {
                if (_MDL == null && Testparameter != null)
                {

                    return Testparameter.MDL;
                }
                return _MDL;
            }

            set { SetPropertyValue<string>("MDL", ref _MDL, value); }
        }
        #endregion

        #region RptLimit
        private string _RptLimit;
        public string RptLimit
        {
            get
            {
                //if (Testparameter != null)
                //{
                //    return Testparameter.RptLimit;
                //}
                if (_RptLimit == null && Testparameter != null)
                {
                    return Testparameter.RptLimit;
                }
                return _RptLimit;

            }

            set { SetPropertyValue<string>("RptLimit", ref _RptLimit, value); }
        }
        #endregion RptLimit

        #region AssignedValue
        private string _AssignedValue;
        public string AssignedValue
        {
            get { return _AssignedValue; }

            set { SetPropertyValue<string>("AssignedValue", ref _AssignedValue, value); }
        }
        #endregion AssignedValue

        #region  EBFAnalyzedBy 
        private Employee _EBFAnalyzedBy;
        public Employee EBFAnalyzedBy
        {
            get { return _EBFAnalyzedBy; }

            set { SetPropertyValue<Employee>("EBFAnalyzedBy", ref _EBFAnalyzedBy, value); }
        }
        #endregion

        #region EBFAnalyzedDate  
        private DateTime? _EBFAnalyzedDate;
        public DateTime? EBFAnalyzedDate
        {
            get { return _EBFAnalyzedDate; }
            set { SetPropertyValue<DateTime?>("EBFAnalyzedDate", ref _EBFAnalyzedDate, value); }
        }
        #endregion

        #region EBFValidatedBy 
        private Employee _EBFValidatedBy;
        public Employee EBFValidatedBy
        {
            get { return _EBFValidatedBy; }
            set { SetPropertyValue<Employee>("EBFValidatedBy", ref _EBFValidatedBy, value); }
        }
        #endregion

        #region EBFValidatedDate
        private DateTime? _EBFValidatedDate;
        public DateTime? EBFValidatedDate
        {
            get { return _EBFValidatedDate; }
            set { SetPropertyValue<DateTime?>("EBFValidatedDate", ref _EBFValidatedDate, value); }
        }
        #endregion

        #region EBFApprovedBy
        private Employee _EBFApprovedBy;
        public Employee EBFApprovedBy
        {
            get { return _EBFApprovedBy; }
            set { SetPropertyValue<Employee>("EBFApprovedBy", ref _EBFApprovedBy, value); }
        }
        #endregion

        #region EBFApprovedDate 
        private DateTime? _EBFApprovedDate;
        public DateTime? EBFApprovedDate
        {
            get { return _EBFApprovedDate; }
            set { SetPropertyValue<DateTime?>("EBFApprovedDate", ref _EBFApprovedDate, value); }
        }
        #endregion

        #region IsComplete
        private bool _IsComplete;
        public bool IsComplete
        {
            get { return _IsComplete; }
            set { SetPropertyValue<bool>("IsComplete", ref _IsComplete, value); }
        }
        #endregion

        #region EBFEnteredDate
        private DateTime _EBFEnteredDate;
        public DateTime EBFEnteredDate
        {
            get { return _EBFEnteredDate; }
            set { SetPropertyValue<DateTime>("EBFEnteredDate", ref _EBFEnteredDate, value); }
        }
        #endregion

        #region IsExported
        private bool _IsExported;
        public bool IsExported
        {
            get { return _IsExported; }
            set { SetPropertyValue<bool>("IsExported", ref _IsExported, value); }
        }
        #endregion

        public enum Priority
        {
            [ImageName("State_Priority_Low")]
            Low = 0,
            [ImageName("State_Priority_Normal")]
            Normal = 1,
            [ImageName("State_Priority_High")]
            High = 2
        }


       

        //#region Reporting-SampleParameter Relation
        //[Association("ReportingSampleparameter", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //public XPCollection<Reporting> Reportings
        //{
        //    get { return GetCollection<Reporting>("Reportings"); }
        //}
        //#endregion

        private string _ABID;

        public string ABID
        {
            get { return _ABID; }
            set { SetPropertyValue("ABID", ref _ABID, value); }
        }

        private SpreadSheetEntry_AnalyticalBatch _UQABID;

        public SpreadSheetEntry_AnalyticalBatch UQABID
        {
            get { return _UQABID; }
            set { SetPropertyValue("UQABID", ref _UQABID, value); }
        }

        private string _Qualifier;
        public string Qualifier
        {
            get { return _Qualifier; }
            set { SetPropertyValue("Qualifier", ref _Qualifier, value); }
        }

        private string _FinalResult;
        public string FinalResult
        {
            get { return _FinalResult; }
            set { SetPropertyValue("FinalResult", ref _FinalResult, value); }
        }

        private Unit _FinalResultUnits;
        public Unit FinalResultUnits
        {
            get { return _FinalResultUnits; }
            set { SetPropertyValue("FinalResultUnits", ref _FinalResultUnits, value); }
        }

        private string _SurrogateLowLimit;
        public string SurrogateLowLimit
        {
            get
            {
                if ((_SurrogateLowLimit == null) && (Testparameter != null))
                {
                    return _Testparameter.SurrogateLowLimit;
                }
                return _SurrogateLowLimit;
            }
            set { SetPropertyValue("SurrogateLowLimit", ref _SurrogateLowLimit, value); }
        }

        private string _SurrogateHighLimit;
        public string SurrogateHighLimit
        {
            get
            {
                if ((_SurrogateHighLimit == null) && (Testparameter != null))
                {
                    return _Testparameter.SurrogateHighLimit;
                }
                return _SurrogateHighLimit;
            }
            set { SetPropertyValue("SurrogateHighLimit", ref _SurrogateHighLimit, value); }
        }



        #region MCL
        private string _MCL;
        public string MCL
        {

            get
            {
                if (_MCL == null && Testparameter != null)
                {

                    return Testparameter.MCL;
                }
                return _MCL;
            }

            set { SetPropertyValue<string>("MCL", ref _MCL, value); }
        }
        #endregion

        #region LOQ
        private string _LOQ;
        public string LOQ
        {

            get
            {
                if (_LOQ == null && Testparameter != null)
                {

                    return Testparameter.LOQ;
                }
                return _LOQ;
            }

            set { SetPropertyValue<string>("LOQ", ref _LOQ, value); }
        }
        #endregion

        #region UQL
        private string _UQL;
        public string UQL
        {

            get
            {
                if (_UQL == null && Testparameter != null)
                {

                    return Testparameter.UQL;
                }
                return _UQL;
            }

            set { SetPropertyValue<string>("UQL", ref _UQL, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        [Size(1000)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment", ref _Comment, value); }
        }


        #endregion
        #region SurrogateSpikeAmount
        private double? _SurrogateSpikeAmount;
        public double? SurrogateSpikeAmount
        {
            get
            {
                if ((_SurrogateSpikeAmount == null) && (_Testparameter != null))
                {
                    return Testparameter.SurrogateAmount;
                }
                else
                    return _SurrogateSpikeAmount;
            }
            set { SetPropertyValue<double?>("SurrogateSpikeAmount", ref _SurrogateSpikeAmount, value); }
        }
        #endregion

        #region SurrogateUnits
        private Unit _SurrogateUnits;
        public Unit SurrogateUnits
        {
            get
            {
                if ((_SurrogateUnits == null) && (_Testparameter != null))
                {
                    return Testparameter.SurrogateUnits;
                }
                else
                    return _SurrogateUnits;
            }
            set { SetPropertyValue<Unit>("SurrogateUnits", ref _SurrogateUnits, value); }
        }
        #endregion

        #region RawData
        private FileData _RawData;
        public FileData RawData
        {
            get
            {
                return _RawData;
            }
            set { SetPropertyValue("RawData", ref _RawData, value); }
        }
        #endregion

        [NonPersistent]
        public string Parent
        {
            get
            {
                if (Sampling != null && Testparameter != null)
                {
                    if (Testparameter.TestMethod != null)
                    {
                        if (curlanguage.strcurlanguage == "En")
                        {
                            if (Testparameter.IsGroup == false)
                            {
                                return string.Format("SampleID:{0} Martix:{1} Test:{2} Method:{3}: Component:{4}:", Sampling.SampleID, Testparameter.TestMethod.MatrixName.MatrixName, Testparameter.TestMethod.TestName, Testparameter.TestMethod.MethodName.MethodNumber, Testparameter.Component.Components);
                            }
                            else
                            {
                                return string.Format("SampleID:{0} Martix:{1} Test:{2} Method:{3}: Component:{4}:", Sampling.SampleID, Testparameter.TestMethod.MatrixName.MatrixName, Testparameter.TestMethod.TestName, Testparameter.TestMethod.MethodName, Testparameter.Component);
                            }
                            //return string.Format("SampleID:{0} Martix:{1} Test:{2} Method:{3}:", Samplelogin.SampleID, Testparameter.TestMethod.MatrixName.MatrixName, Testparameter.TestMethod.TestName, Testparameter.TestMethod.MethodName.MethodName);
                        }
                        else
                        {
                            return string.Format("样品编号:{0} 基质:{1} 监测项目:{2} 方法:{3}:", Sampling.SampleID, Testparameter.TestMethod.MatrixName.MatrixName, Testparameter.TestMethod.TestName, Testparameter.TestMethod.MethodName.MethodName);
                        }

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }

            }
            //set { SetPropertyValue("MatrixTestMethod", ref _TestGroup, value); }
        }


        private string _DefaultResult;
        [NonPersistent]
        public string DefaultResult
        {
            get
            {
                if (Testparameter != null)
                {
                    return Testparameter.DefaultResult;
                }
                return _DefaultResult;
            }

            set { SetPropertyValue("DefaultResult", ref _DefaultResult, value); }
        }

        #region STDConc
        private string _STDConc;
        public string STDConc
        {
            get
            {
                if (_STDConc == null && Testparameter != null)
                {
                    return Testparameter.STDConc;
                }
                return _STDConc;
            }
            set { SetPropertyValue("STDConc", ref _STDConc, value); }
        }
        #endregion

        #region STDConcUnit
        private Unit _STDConcUnit;
        public Unit STDConcUnit
        {
            get
            {
                if (_STDConcUnit == null && Testparameter != null)
                {
                    return Testparameter.STDConcUnit;
                }
                return _STDConcUnit;
            }
            set { SetPropertyValue("STDConcUnit", ref _STDConcUnit, value); }
        }
        #endregion

        #region STDVolAdd
        private string _STDVolAdd;
        public string STDVolAdd
        {
            get
            {
                if (_STDVolAdd == null && Testparameter != null)
                {
                    return Testparameter.STDVolAdd;
                }
                return _STDVolAdd;
            }
            set { SetPropertyValue("STDVolAdd", ref _STDVolAdd, value); }
        }
        #endregion

        #region STDVolUnit
        private Unit _STDVolUnit;
        public Unit STDVolUnit
        {
            get
            {
                if (_STDVolUnit == null && Testparameter != null)
                {
                    return Testparameter.STDVolUnit;
                }
                return _STDVolUnit;
            }
            set { SetPropertyValue("STDVolUnit", ref _STDVolUnit, value); }
        }
        #endregion

        #region SpikeAmount
        private Nullable<double> _SpikeAmount;
        public Nullable<double> SpikeAmount
        {
            get
            {
                if (_SpikeAmount == null && Testparameter != null)
                {
                    return Testparameter.SpikeAmount;
                }
                return _SpikeAmount;
            }
            set { SetPropertyValue("SpikeAmount", ref _SpikeAmount, value); }
        }
        #endregion

        #region SpikeAmountUnit
        private Unit _SpikeAmountUnit;
        public Unit SpikeAmountUnit
        {
            get
            {
                if (_SpikeAmountUnit == null && Testparameter != null)
                {
                    return Testparameter.SpikeAmountUnit;
                }
                return _SpikeAmountUnit;
            }
            set { SetPropertyValue("SpikeAmountUnit", ref _SpikeAmountUnit, value); }
        }
        #endregion

        #region SampleparameterUnit
        private Unit _SampleparameterUnit;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [XafDisplayName("Units")]
        public Unit SampleparameterUnit
        {
            get
            {
                if ((_SampleparameterUnit == null) && (_Testparameter != null))
                {
                    return Testparameter.DefaultUnits;
                }
                return _SampleparameterUnit;
            }
            set { SetPropertyValue<Unit>("SampleparameterUnit", ref _SampleparameterUnit, value); }
        }
        #endregion

        #region RecLCLimit
        private string _RecLCLimit;
        public string RecLCLimit
        {
            get
            {
                if (_RecLCLimit == null && Testparameter != null)
                {
                    return Testparameter.RecLCLimit;
                }
                return _RecLCLimit;
            }
            set { SetPropertyValue("RecLCLimit", ref _RecLCLimit, value); }
        }
        #endregion

        #region RecHCLimit
        private string _RecHCLimit;
        public string RecHCLimit
        {
            get
            {
                if (_RecHCLimit == null && Testparameter != null)
                {
                    return Testparameter.RecHCLimit;
                }
                return _RecHCLimit;
            }
            set { SetPropertyValue("RecHCLimit", ref _RecHCLimit, value); }
        }
        #endregion

        #region RPDLCLimit
        private string _RPDLCLimit;
        public string RPDLCLimit
        {
            get
            {
                if (_RPDLCLimit == null && Testparameter != null)
                {
                    return Testparameter.RPDLCLimit;
                }
                return _RPDLCLimit;
            }
            set { SetPropertyValue("RPDLCLimit", ref _RPDLCLimit, value); }
        }
        #endregion

        #region RPDHCLimit
        private string _RPDHCLimit;
        public string RPDHCLimit
        {
            get
            {
                if (_RPDHCLimit == null && Testparameter != null)
                {
                    return Testparameter.RPDHCLimit;
                }
                return _RPDHCLimit;
            }
            set { SetPropertyValue("RPDHCLimit", ref _RPDHCLimit, value); }
        }
        #endregion

        #region LowCLimit
        private string _LowCLimit;
        public string LowCLimit
        {
            get
            {
                if (_LowCLimit == null && Testparameter != null)
                {
                    return Testparameter.LowCLimit;
                }
                return _LowCLimit;
            }
            set { SetPropertyValue("LowCLimit", ref _LowCLimit, value); }
        }
        #endregion

        #region HighCLimit
        private string _HighCLimit;
        public string HighCLimit
        {
            get
            {
                if (_HighCLimit == null && Testparameter != null)
                {
                    return Testparameter.HighCLimit;
                }
                return _HighCLimit;
            }
            set { SetPropertyValue("HighCLimit", ref _HighCLimit, value); }
        }
        #endregion

        #region RELCLimit
        private string _RELCLimit;
        public string RELCLimit
        {
            get
            {
                if (_RELCLimit == null && Testparameter != null)
                {
                    return Testparameter.RecLCLimit;
                }
                return _RELCLimit;
            }
            set { SetPropertyValue("RELCLimit", ref _RELCLimit, value); }
        }
        #endregion

        #region REHCLimit
        private string _REHCLimit;
        public string REHCLimit
        {
            get
            {
                if (_REHCLimit == null && Testparameter != null)
                {
                    return Testparameter.REHCLimit;
                }
                return _REHCLimit;
            }
            set { SetPropertyValue("REHCLimit", ref _REHCLimit, value); }
        }
        #endregion

        #region QCBatchID
        private QCBatchSequence _QCBatchID;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public QCBatchSequence QCBatchID
        {
            get
            {
                return _QCBatchID;
            }
            set
            {
                SetPropertyValue<QCBatchSequence>("QCBatchID", ref _QCBatchID, value);
            }
        }
        #endregion

        #region QCSort
        private int _QCSort;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int QCSort
        {
            get
            {
                return _QCSort;
            }
            set
            {
                SetPropertyValue<int>("QCSort", ref _QCSort, value);
            }
        }
        #endregion

        private int _DupBottle;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public int DupBottle
        {
            get { return _DupBottle; }
            set { SetPropertyValue("DupBottle", ref _DupBottle, value); }
        }

        [VisibleInDetailView(false)]
        [NonPersistent]
        public string SysSampleCode
        {
            get
            {
                if (QCBatchID != null)
                {
                    return QCBatchID.SYSSamplecode;
                }
                else if (Sampling != null)
                {
                    return Sampling.SampleID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #region SortNo
        private int _SortNo;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [NonPersistent]
        public int SortNo
        {
            get
            {
                if (Sampling != null)
                {
                    _SortNo = Sampling.SampleNo;
                }
                return _SortNo;
            }
            set
            {
                SetPropertyValue<int>(nameof(SortNo), ref _SortNo, value);
            }
        }
        #endregion
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestGuide> TestGuides
        {
            get
            {
                if (Testparameter != null && Testparameter.TestMethod != null && Testparameter.TestMethod.TestGuides != null)
                {
                    XPCollection<TestGuide> listGuides = new XPCollection<TestGuide>(Session, CriteriaOperator.Parse("[TestMethod] = ? ", Testparameter.TestMethod.Oid));
                    return listGuides;
                }
                else
                {
                    return null;
                }
            }
        }
        #region Preservative
        private Preservative _Preservative;
        [DataSourceProperty("PreservativeSettings")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Preservative Preservative
        {
            get
            {
                return _Preservative;
            }
            set { SetPropertyValue("Preservative", ref _Preservative, value); }
        }
        public XPCollection<Preservative> PreservativeSettings
        {
            get
            {
                if (Testparameter != null && Testparameter.TestMethod != null)
                {
                    List<string> lstSM = new List<string>();
                    IList<TestGuide> objVM = Session.GetObjects(Session.GetClassInfo(typeof(TestGuide)), CriteriaOperator.Parse("[TestMethod.Oid] = ?", Testparameter.TestMethod.Oid), null, int.MaxValue, false, true).Cast<TestGuide>().ToList();
                    foreach (TestGuide test in objVM)
                    {
                        if (test != null && test.Preservative != null && !lstSM.Contains(test.Preservative.PreservativeName))
                        {
                            lstSM.Add(test.Preservative.PreservativeName);
                        }
                    }
                    return new XPCollection<Preservative>(Session, new InOperator("PreservativeName", lstSM));
                }
                else
                {
                    return new XPCollection<Preservative>(Session, CriteriaOperator.Parse("[Oid] is null"));
                }
            }
        }
        #endregion
        #region ContainerType
        private Modules.BusinessObjects.Setting.Container _ContainerType;
        [DataSourceProperty("ContainerSettings")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Modules.BusinessObjects.Setting.Container ContainerType
        {
            get
            {

                return _ContainerType;
            }
            set { SetPropertyValue("ContainerType", ref _ContainerType, value); }
        }
        public XPCollection<Container> ContainerSettings
        {
            get
            {
                if (Testparameter != null && Testparameter.TestMethod != null)
                {
                    List<string> lstSM = new List<string>();
                    IList<TestGuide> objVM = Session.GetObjects(Session.GetClassInfo(typeof(TestGuide)), CriteriaOperator.Parse("[TestMethod.Oid] = ?", Testparameter.TestMethod.Oid), null, int.MaxValue, false, true).Cast<TestGuide>().ToList();
                    foreach (TestGuide test in objVM)
                    {
                        if (test != null && test.Container != null && !lstSM.Contains(test.Container.ContainerName))
                        {
                            lstSM.Add(test.Container.ContainerName);
                        }
                    }
                    return new XPCollection<Container>(Session, new InOperator("ContainerName", lstSM));
                }
                else
                {
                    return new XPCollection<Container>(Session, CriteriaOperator.Parse("[Oid] is null"));
                }
            }
        }
        #endregion
        private uint _Container;
        [XafDisplayName("#Container")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public uint Container
        {
            get { return _Container; }
            set { SetPropertyValue("Container", ref _Container, value); }
        }
        private string _HoldTime;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string HoldTime
        {
            get
            {

                if (TestGuides != null && TestGuides.Count > 1)
                {
                    string minValue = TestGuides.Where(i => i.HoldingTimeBeforePrep != null && i.HoldingTimeBeforePrep.HoldingTime != null).Select(i => i.HoldingTimeBeforePrep.HoldingTime).Min();
                    if (minValue != null)
                    {
                        _HoldTime = TestGuides.Where(i => i.HoldingTimeBeforePrep != null && i.HoldingTimeBeforePrep.HoldingTime != null && i.HoldingTimeBeforePrep.HoldingTime == minValue).Select(i => i.HoldingTimeBeforePrep.HoldingTime).FirstOrDefault();
                    }
                }
                {
                    if (TestGuides != null && TestGuides.Count == 1)
                    {
                        _HoldTime = TestGuides.Where(i => i.HoldingTimeBeforePrep != null && i.HoldingTimeBeforePrep.HoldingTime != null).Select(i => i.HoldingTimeBeforePrep.HoldingTime).FirstOrDefault();
                    }
                }
                return _HoldTime;
            }
            set { SetPropertyValue("HoldTime", ref _HoldTime, value); }
        }

        #region SignOff
        private bool _SignOff;
        public bool SignOff
        {
            get
            {
                return _SignOff;
            }
            set
            {
                SetPropertyValue<bool>(nameof(SignOff), ref _SignOff, value);
            }
        }
        #endregion

        #region IsGroup
        private bool _IsGroup;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public bool IsGroup
        {
            get { return _IsGroup; }
            set { SetPropertyValue("IsGroup", ref _IsGroup, value); }
        }
        #endregion

        #region GroupTest
        private GroupTestMethod _GroupTest;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public GroupTestMethod GroupTest
        {
            get { return _GroupTest; }
            set { SetPropertyValue("GroupTest", ref _GroupTest, value); }
        }
        #endregion

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
        #region InvoiceIsDone
        private bool _InvoiceIsDone;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool InvoiceIsDone
        {
            get { return _InvoiceIsDone; }
            set { SetPropertyValue("InvoiceIsDone", ref _InvoiceIsDone, value); }
        }
        #endregion
        private string _QcSampleResult;
        //[ImmediatePostData(true)]
        public string QcSampleResult
        {

            get { return _QcSampleResult; }
            set { SetPropertyValue<string>("QcSampleResult", ref _QcSampleResult, value); }
        }

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string uqQCBatchID
        {
            get
            {
                if (UQABID != null)
                {
                    return UQABID.AnalyticalBatchID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string JobID
        {
            get
            {
                if (UQABID != null)
                {
                    return UQABID.Jobid;
                }
                else
                if (QCBatchID != null && QCBatchID.SampleID != null && QCBatchID.SampleID.JobID != null)
                {
                    return QCBatchID.SampleID.JobID.JobID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #region ClientName
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string ClientName
        {
            get
            {
                if (Sampling != null && Sampling.SamplingProposal != null && Sampling.SamplingProposal.ClientName != null)
                {
                    return Sampling.SamplingProposal.ClientName.CustomerName;
                }
                else
                if (QCBatchID != null && QCBatchID.SampleID != null && QCBatchID.SampleID.JobID != null && QCBatchID.SampleID.JobID.ClientName != null)
                {
                    return QCBatchID.SampleID.JobID.ClientName.CustomerName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion 

        #region IsResultDefaultValue
        private bool _IsResultDefaultValue;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool IsResultDefaultValue
        {
            get
            {
                if (Testparameter != null && !string.IsNullOrEmpty(Testparameter.ParameterDefaultResults))
                {
                    _IsResultDefaultValue = true;
                }
                else
                {
                    _IsResultDefaultValue = false;
                }
                return _IsResultDefaultValue;
            }
            set { SetPropertyValue("IsResultDefaultValue", ref _IsResultDefaultValue, value); }
        }
        #endregion

        #region DVResult
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        //[Browsable(false)]
        public Nullable<double> DVResult
        {
            get
            {
                if (double.TryParse(Result, out _))
                {
                    return Convert.ToDouble(Result);
                }
                else
                {
                    return null;
                }

            }
        }
        #endregion
        #region _IsPrepMethodComplete
        private bool _IsPrepMethodComplete;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public bool IsPrepMethodComplete
        {

            get { return _IsPrepMethodComplete; }
            set { SetPropertyValue<bool>("_IsPrepMethodComplete", ref _IsPrepMethodComplete, value); }
        }
        #endregion
        #region PrepMethodCount
        private int _PrepMethodCount;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public int PrepMethodCount
        {

            get { return _PrepMethodCount; }
            set { SetPropertyValue<int>("PrepMethodCount", ref _PrepMethodCount, value); }
        }
        #endregion
        #region PrepBatchID
        private string _PrepBatchID;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        [Size(SizeAttribute.Unlimited)]
        public string PrepBatchID
        {

            get { return _PrepBatchID; }
            set { SetPropertyValue<string>("PrepBatchID", ref _PrepBatchID, value); }
        }
        #endregion
        #region SubOut
        private bool _subOut;

        public bool SubOut
        {
            get { return _subOut; }
            set { SetPropertyValue("SubOut", ref _subOut, value); }
        }
        #endregion

        #region SubLabName
        private SubContractLab _SubLabName;
        public SubContractLab SubLabName
        {
            get { return _SubLabName; }
            set { SetPropertyValue<SubContractLab>("SubLabName", ref _SubLabName, value); }
        }
        #endregion

        #region SubOutBy
        private Employee _SubOutBy;
        public Employee SubOutBy
        {
            get { return _SubOutBy; }
            set { SetPropertyValue<Employee>("SubOutBy", ref _SubOutBy, value); }
        }
        #endregion

        #region SubOutDate 
        private DateTime? _SubOutDate;
        public DateTime? SubOutDate
        {
            get { return _SubOutDate; }
            set { SetPropertyValue<DateTime?>("SubOutDate", ref _SubOutDate, value); }
        }
        #endregion

        private QCType _SuboutQCType;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public QCType SuboutQCType
        {
            get { return _SuboutQCType; }
            set { SetPropertyValue("SuboutQCType", ref _SuboutQCType, value); }
        }
        #region  Rollback
        private string _Rollback;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        [Size(SizeAttribute.Unlimited)]
        public string Rollback
        {
            get { return _Rollback; }
            set { SetPropertyValue("Rollback", ref _Rollback, value); }
        }
        #endregion

        #region SuboutAnalyzedBy
        private string _SuboutAnalyzedBy;
        public string SuboutAnalyzedBy
        {
            get { return _SuboutAnalyzedBy; }
            set { SetPropertyValue(nameof(SuboutAnalyzedBy), ref _SuboutAnalyzedBy, value); }
        }
        #endregion

        #region SuboutAnalyzedDate
        private DateTime _SuboutAnalyzedDate;
        public DateTime SuboutAnalyzedDate
        {
            get { return _SuboutAnalyzedDate; }
            set { SetPropertyValue(nameof(SuboutAnalyzedDate), ref _SuboutAnalyzedDate, value); }
        }
        #endregion
        private bool _TestHold;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public bool TestHold
        {
            get { return _TestHold; }
            set { SetPropertyValue(nameof(TestHold), ref _TestHold, value); }
        }
    }
}