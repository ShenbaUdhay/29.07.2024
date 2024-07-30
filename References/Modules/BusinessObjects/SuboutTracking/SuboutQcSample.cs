using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.ComponentModel;
using Method = Modules.BusinessObjects.Setting.Method;

namespace Modules.BusinessObjects.SuboutTracking
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SuboutQcSample : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SuboutQcSample(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Status = Samplestatus.PendingEntry;
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region SuboutID
        private SubOutSampleRegistrations _SuboutID;
        [Association("SubOutQcSample")]
        public SubOutSampleRegistrations SuboutID
        {
            get { return _SuboutID; }
            set { SetPropertyValue(nameof(SuboutID), ref _SuboutID, value); }
        }
        #endregion

        #region Test
        private TestMethod _Test;
        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        #endregion

        #region Method
        private Method _Method;
        public Method Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }
        #endregion

        #region Matrix
        private Matrix _Matrix;
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Method), ref _Matrix, value); }
        }
        #endregion

        #region JobID
        private Samplecheckin _JobID;

        public Samplecheckin JobID
        {
            get { return _JobID; }
            set { SetPropertyValue("JobID", ref _JobID, value); }
        }
        #endregion

        #region QCType
        private string _QCType;
        public string QCType
        {
            get { return _QCType; }
            set { SetPropertyValue(nameof(QCType), ref _QCType, value); }
        }
        #endregion

        #region SampleID
        private string _SampleID;
        public string SampleID
        {
            get { return _SampleID; }
            set { SetPropertyValue(nameof(SampleID), ref _SampleID, value); }
        }
        #endregion

        #region SampleName
        private string _SampleName;
        public string SampleName
        {
            get { return _SampleName; }
            set { SetPropertyValue(nameof(SampleName), ref _SampleName, value); }
        }
        #endregion

        #region Parameter
        private string _Parameter;
        public string Parameter
        {
            get
            {
                return _Parameter;
            }
            set { SetPropertyValue(nameof(Parameter), ref _Parameter, value); }
        }
        #endregion

        #region NumericResult
        private string _NumericResult;
        public string NumericResult
        {
            get
            {
                return _NumericResult;
            }
            set { SetPropertyValue(nameof(NumericResult), ref _NumericResult, value); }
        }
        #endregion

        #region Result
        private string _Result;
        public string Result
        {
            get
            {
                return _Result;
            }
            set { SetPropertyValue(nameof(Result), ref _Result, value); }
        }
        #endregion

        #region Units
        private Unit _Units;
        public Unit Units
        {
            get
            {
                return _Units;
            }
            set { SetPropertyValue(nameof(Units), ref _Units, value); }
        }
        #endregion

        #region DF
        private string _DF;
        public string DF
        {
            get
            {
                return _DF;
            }
            set { SetPropertyValue(nameof(DF), ref _DF, value); }
        }
        #endregion

        #region LOQ
        private string _LOQ;
        public string LOQ
        {
            get
            {
                return _LOQ;
            }
            set { SetPropertyValue(nameof(LOQ), ref _LOQ, value); }
        }
        #endregion

        #region Rec
        private string _Rec;
        public string Rec
        {
            get
            {
                return _Rec;
            }
            set { SetPropertyValue(nameof(Rec), ref _Rec, value); }
        }
        #endregion

        #region UQL
        private string _UQL;
        public string UQL
        {
            get
            {
                return _UQL;
            }
            set { SetPropertyValue(nameof(UQL), ref _UQL, value); }
        }
        #endregion

        #region RptLimit
        private string _RptLimit;
        public string RptLimit
        {
            get
            {
                return _RptLimit;
            }
            set { SetPropertyValue(nameof(RptLimit), ref _RptLimit, value); }
        }
        #endregion

        #region MDL
        private string _MDL;
        public string MDL
        {
            get
            {
                return _MDL;
            }
            set { SetPropertyValue(nameof(MDL), ref _MDL, value); }
        }
        #endregion

        #region SpikeAmount
        private string _SpikeAmount;
        public string SpikeAmount
        {
            get
            {
                return _SpikeAmount;
            }
            set { SetPropertyValue(nameof(SpikeAmount), ref _SpikeAmount, value); }
        }
        #endregion

        #region %Recovery
        private string _Recovery;
        public string Recovery
        {
            get
            {
                return _Recovery;
            }
            set { SetPropertyValue(nameof(Recovery), ref _Recovery, value); }
        }
        #endregion

        #region %RPD
        private string _RPD;
        public string RPD
        {
            get
            {
                return _RPD;
            }
            set { SetPropertyValue(nameof(RPD), ref _RPD, value); }
        }
        #endregion

        #region RecLCLimit
        private string _RecLCLimit;
        public string RecLCLimit
        {
            get
            {
                return _RecLCLimit;
            }
            set { SetPropertyValue(nameof(RecLCLimit), ref _RecLCLimit, value); }
        }
        #endregion

        #region RPDLCLimit
        private string _RPDLCLimit;
        public string RPDLCLimit
        {
            get
            {
                return _RPDLCLimit;
            }
            set { SetPropertyValue(nameof(RPDLCLimit), ref _RPDLCLimit, value); }
        }
        #endregion

        #region RPDUCLimit
        private string _RPDUCLimit;
        public string RPDUCLimit
        {
            get
            {
                return _RPDUCLimit;
            }
            set { SetPropertyValue(nameof(RPDUCLimit), ref _RPDUCLimit, value); }
        }
        #endregion

        #region RecUCLimit
        private string _RecUCLimit;
        public string RecUCLimit
        {
            get
            {
                return _RecUCLimit;
            }
            set { SetPropertyValue(nameof(RecUCLimit), ref _RecUCLimit, value); }
        }
        #endregion

        #region AnalyzedBy
        private string _AnalyzedBy;
        public string AnalyzedBy
        {
            get
            {
                return _AnalyzedBy;
            }
            set { SetPropertyValue(nameof(AnalyzedBy), ref _AnalyzedBy, value); }
        }
        #endregion

        #region AnalyzedDate
        private Nullable<DateTime> _AnalyzedDate;
        public Nullable<DateTime> AnalyzedDate
        {
            get
            {
                return _AnalyzedDate;
            }
            set { SetPropertyValue(nameof(AnalyzedDate), ref _AnalyzedDate, value); }
        }
        #endregion

        #region SuboutEnteredBy
        private Employee _SuboutEnteredBy;
        public Employee SuboutEnteredBy
        {
            get
            {
                return _SuboutEnteredBy;
            }
            set { SetPropertyValue(nameof(SuboutEnteredBy), ref _SuboutEnteredBy, value); }
        }
        #endregion

        #region SuboutEnteredDate
        private Nullable<DateTime> _SuboutEnteredDate;
        public Nullable<DateTime> SuboutEnteredDate
        {
            get
            {
                return _SuboutEnteredDate;
            }
            set { SetPropertyValue(nameof(SuboutEnteredDate), ref _SuboutEnteredDate, value); }
        }
        #endregion

        #region IsEDDImported
        private bool _IsEDDImported;
        [Browsable(false)]
        public bool IsEDDImported
        {
            get
            {
                return _IsEDDImported;
            }
            set { SetPropertyValue(nameof(IsEDDImported), ref _IsEDDImported, value); }
        }
        #endregion

        #region ValidatedDate
        private Nullable<DateTime> _ValidatedDate;
        public Nullable<DateTime> ValidatedDate
        {
            get
            {
                return _ValidatedDate;
            }
            set { SetPropertyValue(nameof(ValidatedDate), ref _ValidatedDate, value); }
        }
        #endregion

        #region ValidatedBy
        private string _ValidatedBy;
        public string ValidatedBy
        {
            get
            {
                return _ValidatedBy;
            }
            set { SetPropertyValue(nameof(ValidatedBy), ref _ValidatedBy, value); }
        }
        #endregion

        #region ApprovedBy
        private string _ApprovedBy;
        public string ApprovedBy
        {
            get
            {
                return _ApprovedBy;
            }
            set { SetPropertyValue(nameof(ApprovedBy), ref _ApprovedBy, value); }
        }
        #endregion

        #region ApprovedDate
        private Nullable<DateTime> _ApprovedDate;
        public Nullable<DateTime> ApprovedDate
        {
            get
            {
                return _ApprovedDate;
            }
            set { SetPropertyValue(nameof(ApprovedDate), ref _ApprovedDate, value); }
        }
        #endregion

        #region SuboutApprovedDate
        private DateTime? _SuboutApprovedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public DateTime? SuboutApprovedDate
        {
            get { return _SuboutApprovedDate; }
            set { SetPropertyValue<DateTime?>("SuboutApprovedDate", ref _SuboutApprovedDate, value); }
        }
        #endregion

        #region SuboutApprovedBy
        private Employee _SuboutApprovedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Employee SuboutApprovedBy
        {
            get
            {
                return _SuboutApprovedBy;
            }
            set { SetPropertyValue<Employee>("SuboutApprovedBy", ref _SuboutApprovedBy, value); }
        }
        #endregion

        #region SuboutValidatedDate
        private DateTime? _SuboutValidatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public DateTime? SuboutValidatedDate
        {
            get { return _SuboutValidatedDate; }
            set { SetPropertyValue<DateTime?>("SuboutValidatedDate", ref _SuboutValidatedDate, value); }
        }
        #endregion

        #region SuboutValidatedBy
        private Employee _SuboutValidatedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Employee SuboutValidatedBy
        {
            get
            {
                return _SuboutValidatedBy;
            }
            set { SetPropertyValue<Employee>("SuboutValidatedBy", ref _SuboutValidatedBy, value); }
        }
        #endregion

        #region Surrogate
        private string _Surrogate;
        public string Surrogate
        {
            get
            {
                return _Surrogate;
            }
            set { SetPropertyValue(nameof(Surrogate), ref _Surrogate, value); }
        }
        #endregion

        #region ContractLab
        private string _ContractLab;
        public string ContractLab
        {
            get
            {
                return _ContractLab;
            }
            set { SetPropertyValue(nameof(ContractLab), ref _ContractLab, value); }
        }
        #endregion
        #region Status
        private Samplestatus _Status;
        [VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public Samplestatus Status
        {
            get
            {
                return _Status;
            }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        #endregion
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

    }
}