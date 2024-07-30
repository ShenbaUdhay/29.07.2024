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
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class QCResults : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public QCResults(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        private Samplecheckin _JobID;

        public Samplecheckin JobID
        {
            get { return _JobID; }
            set { SetPropertyValue("JobID", ref _JobID, value); }
        }

        private QCType _QCType;

        public QCType QCType
        {
            get { return _QCType; }
            set { SetPropertyValue("QCType", ref _QCType, value); }
        }

        #region TestParameter
        private Testparameter _Testparameter;
        //[Association]
        public Testparameter Testparameter
        {
            get { return _Testparameter; }
            set { SetPropertyValue("Testparameter", ref _Testparameter, value); }
        }
        #endregion TestParameter

        private string _SampleID;
        public string SampleID
        {
            get { return _SampleID; }
            set { SetPropertyValue("SampleID", ref _SampleID, value); }
        }

        private string _uqSampleID;
        public string uqSampleID
        {
            get { return _uqSampleID; }
            set { SetPropertyValue("uqSampleID", ref _uqSampleID, value); }
        }


        private int _QCRunNo;

        public int QCRuno
        {
            get { return _QCRunNo; }
            set { SetPropertyValue("QCRunoNo", ref _QCRunNo, value); }
        }

        private string _SystemID;

        public string SystemID
        {
            get { return _SystemID; }
            set { SetPropertyValue("SystemID", ref _SystemID, value); }
        }

        private int _UQABID;

        public int UQABID
        {
            get { return _UQABID; }
            set { SetPropertyValue("UQABID", ref _UQABID, value); }
        }

        private string _FrontRes;

        public string FrontRes
        {
            get { return _FrontRes; }
            set { SetPropertyValue("FrontRes", ref _FrontRes, value); }
        }


        private string _BackRes;

        public string BackRes
        {
            get { return _BackRes; }
            set { SetPropertyValue("BackRes", ref _BackRes, value); }
        }

        private string _PreW;
        public string PreW
        {
            get { return _PreW; }
            set { SetPropertyValue("PreW", ref _PreW, value); }
        }

        private string _PostW;
        public string PostW
        {
            get { return _PostW; }
            set { SetPropertyValue("PostW", ref _PostW, value); }
        }

        private string _Result;
        public string Result
        {
            get { return _Result; }
            set { SetPropertyValue("Result", ref _Result, value); }
        }

        private Unit _Units;
        public Unit Units
        {
            get { return _Units; }
            set { SetPropertyValue("Units", ref _Units, value); }
        }

        private string _FlowRate;
        public string FlowRate
        {
            get { return _FlowRate; }
            set { SetPropertyValue("FlowRate", ref _FlowRate, value); }
        }

        private string _Time;
        public string Time
        {
            get { return _Time; }
            set { SetPropertyValue("Time", ref _Time, value); }
        }

        private string _Volume1;
        public string Volume1
        {
            get { return _Volume1; }
            set { SetPropertyValue("Volume1", ref _Volume1, value); }
        }

        private string _Volume2;
        public string Volume2
        {
            get { return _Volume2; }
            set { SetPropertyValue("Volume2", ref _Volume2, value); }
        }

        private string _Volume3;
        public string Volume3
        {
            get { return _Volume3; }
            set { SetPropertyValue("Volume3", ref _Volume3, value); }
        }

        private string _DF;
        public string DF
        {
            get { return _DF; }
            set { SetPropertyValue("DF", ref _DF, value); }
        }

        private string _Moisture;
        public string Moisture
        {
            get { return _Moisture; }
            set { SetPropertyValue("DF", ref _Moisture, value); }
        }

        private string _SpikeAmount;
        public string SpikeAmount
        {
            get { return _SpikeAmount; }
            set { SetPropertyValue("SpikeAmount", ref _SpikeAmount, value); }
        }

        private string _Rec;
        public string Rec
        {
            get { return _Rec; }
            set { SetPropertyValue("Rec", ref _Rec, value); }
        }

        private string _PercentRecLimit;
        public string PercentRecLimit
        {
            get { return _PercentRecLimit; }
            set { SetPropertyValue("PercentRecLimit", ref _PercentRecLimit, value); }
        }

        private string _RepLimit;
        public string RepLimit
        {
            get { return _RepLimit; }
            set { SetPropertyValue("RepLimit", ref _RepLimit, value); }
        }

        private string _ClientRepLimit;
        public string ClientRepLimit
        {
            get { return _ClientRepLimit; }
            set { SetPropertyValue("ClientRepLimit", ref _ClientRepLimit, value); }
        }

        private string _Qualifier;
        public string Qualifier
        {
            get { return _Qualifier; }
            set { SetPropertyValue("Qualifier", ref _Qualifier, value); }
        }

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
            {
                _EnteredBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
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
            {
                _ValidatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
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
            {
                _ApprovedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
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

        #region AnalyzedBy
        private Employee _AnalyzedBy;
        public Employee AnalyzedBy
        {
            get
            {
                _AnalyzedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _AnalyzedBy;
            }
            set
            {
                SetPropertyValue<Employee>("AnalyzedBy", ref _AnalyzedBy, value);
            }
        }

        #endregion

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue("Comment ", ref _Comment, value); }
        }

        private int _Duplicate;
        public int Duplicate
        {
            get { return _Duplicate; }
            set { SetPropertyValue("Duplicate ", ref _Duplicate, value); }
        }

        private string _RPD;
        public string RPD
        {
            get { return _RPD; }
            set { SetPropertyValue("RPD ", ref _RPD, value); }
        }

        private string _QCSampleResult;
        public string QCSampleResult
        {
            get { return _QCSampleResult; }
            set { SetPropertyValue("QCSampleResult ", ref _QCSampleResult, value); }
        }

        private string _ResultNumeric;
        public string ResultNumeric
        {
            get { return _ResultNumeric; }
            set { SetPropertyValue("ResultNumeric", ref _ResultNumeric, value); }
        }

        #region AnanyzedDate
        private DateTime? _AuditedDate;
        public DateTime? AuditedDate
        {
            get
            {
                return _AuditedDate;
            }
            set { SetPropertyValue<DateTime?>("AuditedDate ", ref _AuditedDate, value); }
        }
        #endregion

        #region AuditedBy 
        private Employee _AuditedBy;
        public Employee AuditedBy
        {
            get
            {
                _AuditedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _AuditedBy;
            }
            set
            {
                SetPropertyValue<Employee>("AuditedBy ", ref _AuditedBy, value);
            }
        }

        #endregion


        private string _colLRecLimit;
        public string colLRecLimit
        {
            get { return _colLRecLimit; }
            set { SetPropertyValue("colLRecLimit ", ref _colLRecLimit, value); }
        }

        private string _colHRecLimit;
        public string colHRecLimit
        {
            get { return _colHRecLimit; }
            set { SetPropertyValue("colHRecLimit ", ref _colHRecLimit, value); }
        }

        private string _colLRPDLimit;
        public string colLRPDLimit
        {
            get { return _colLRPDLimit; }
            set { SetPropertyValue("colHRecLimit ", ref _colLRPDLimit, value); }
        }

        private string _colHRPDLimit;
        public string colHRPDLimit
        {
            get { return _colHRPDLimit; }
            set { SetPropertyValue("colHRPDLimit ", ref _colHRPDLimit, value); }
        }


        #region LastUpdatedDate
        private DateTime? _LastUpdatedDate;
        public DateTime? LastUpdatedDate
        {
            get
            {
                return _LastUpdatedDate;
            }
            set { SetPropertyValue<DateTime?>("LastUpdatedDate ", ref _LastUpdatedDate, value); }
        }
        #endregion



        #region LastUpdatedBy  
        private Employee _LastUpdatedBy;
        public Employee LastUpdatedBy
        {
            get
            {
                _LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _LastUpdatedBy;
            }
            set
            {
                SetPropertyValue<Employee>("AuditedBy ", ref _AuditedBy, value);
            }
        }
        #endregion

        private string _ScientificResult;
        public string ScientificResult
        {
            get { return _ScientificResult; }
            set { SetPropertyValue("ScientificResult  ", ref _ScientificResult, value); }
        }

        private float _AssignedValue;
        public float AssignedValue
        {
            get { return _AssignedValue; }
            set { SetPropertyValue("colAssignedValue  ", ref _AssignedValue, value); }
        }

        private float _LowTolerance;
        public float LowTolerance
        {
            get { return _LowTolerance; }
            set { SetPropertyValue("LowTolerance  ", ref _LowTolerance, value); }
        }


        private float _HighTolerance;
        public float HighTolerance
        {
            get { return _HighTolerance; }
            set { SetPropertyValue("HighTolerance  ", ref _HighTolerance, value); }
        }

        #region EBFEnteredDate 
        private DateTime? _EBFEnteredDate;
        public DateTime? EBFEnteredDate
        {
            get
            {
                return _EBFEnteredDate;
            }
            set { SetPropertyValue<DateTime?>("EBFEnteredDate  ", ref _EBFEnteredDate, value); }
        }
        #endregion


        #region EBFValidatedDate  
        private DateTime? _EBFValidatedDate;
        public DateTime? EBFValidatedDate
        {
            get
            {
                return _EBFValidatedDate;
            }
            set { SetPropertyValue<DateTime?>("EBFValidatedDate", ref _EBFValidatedDate, value); }
        }
        #endregion

        #region EBFApprovedDate   
        private DateTime? _EBFApprovedDate;
        public DateTime? EBFApprovedDate
        {
            get
            {
                return _EBFApprovedDate;
            }
            set { SetPropertyValue<DateTime?>("EBFApprovedDate", ref _EBFApprovedDate, value); }
        }
        #endregion

        #region IsComplete    
        private bool _IsComplete;
        public bool IsComplete
        {
            get
            {
                return _IsComplete;
            }
            set { SetPropertyValue<bool>("IsComplete ", ref _IsComplete, value); }
        }
        #endregion


        #region EBFValidatedBy  
        private Employee _EBFValidatedBy;
        public Employee EBFValidatedBy
        {
            get
            {
                _EBFValidatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _EBFValidatedBy;
            }
            set
            {
                SetPropertyValue<Employee>("EBFValidatedBy  ", ref _EBFValidatedBy, value);
            }
        }
        #endregion

        #region EBFApprovedBy   
        private Employee _EBFApprovedBy;
        public Employee EBFApprovedBy
        {
            get
            {
                _EBFApprovedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _EBFApprovedBy;
            }
            set
            {
                SetPropertyValue<Employee>("EBFApprovedBy   ", ref _EBFApprovedBy, value);
            }
        }
        #endregion


        #region IsExported     
        private bool _IsExported;
        public bool IsExported
        {
            get
            {
                return _IsExported;
            }
            set { SetPropertyValue<bool>("IsExported", ref _IsExported, value); }
        }
        #endregion

        #region EBFAnalyzedDate   
        private DateTime? _EBFAnalyzedDate;
        public DateTime? EBFAnalyzedDate
        {
            get
            {
                return _EBFAnalyzedDate;
            }
            set { SetPropertyValue<DateTime?>("EBFAnalyzedDate", ref _EBFAnalyzedDate, value); }
        }
        #endregion

        #region EBFAnalyzedBy    
        private Employee _EBFAnalyzedBy;
        public Employee EBFAnalyzedBy
        {
            get
            {
                _EBFAnalyzedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                return _EBFAnalyzedBy;
            }
            set
            {
                SetPropertyValue<Employee>("EBFAnalyzedBy    ", ref _EBFAnalyzedBy, value);
            }
        }
        #endregion


        private string _SpikeVol;
        public string SpikeVol
        {
            get { return _SpikeVol; }
            set { SetPropertyValue("SpikeVol  ", ref _SpikeVol, value); }
        }

        private string _FinalVol;
        public string FinalVol
        {
            get { return _FinalVol; }
            set { SetPropertyValue("FinalVol  ", ref _FinalVol, value); }
        }


        private string _StandardVo;
        public string StandardVo
        {
            get { return _StandardVo; }
            set { SetPropertyValue("StandardVo  ", ref _StandardVo, value); }
        }

        private string _StandardConc;
        public string StandardConc
        {
            get { return _StandardConc; }
            set { SetPropertyValue("StandardConc  ", ref _StandardConc, value); }
        }

        private string _CCVID;
        public string CCVID
        {
            get { return _CCVID; }
            set { SetPropertyValue("CCVID  ", ref _CCVID, value); }
        }

        private string _UnCertainity;
        public string UnCertainity
        {
            get { return _UnCertainity; }
            set { SetPropertyValue("UnCertainity  ", ref _UnCertainity, value); }
        }


        private string _RE;
        public string RE
        {
            get { return _RE; }
            set { SetPropertyValue("RE  ", ref _RE, value); }
        }

        private string _ABID;

        public string ABID
        {
            get { return _ABID; }
            set { SetPropertyValue("ABID", ref _ABID, value); }
        }
    }
}