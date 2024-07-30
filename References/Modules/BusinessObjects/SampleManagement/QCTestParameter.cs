using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
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
    public class QCTestParameter : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public QCTestParameter(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region Parameter
        private Parameter _Parameter;
        [Association]
        public Parameter Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue("Parameter", ref _Parameter, value); }
        }
        #endregion

        #region TestMethod
        private QcParameter _QcParameter;
        [Association]
        public QcParameter QcParameter
        {
            get { return _QcParameter; }
            set { SetPropertyValue("QcParameter", ref _QcParameter, value); }
        }
        #endregion QcParameter

        #region SortOrder
        private int _SortOrder;
        public int SortOrder
        {
            get { return _SortOrder; }
            set { SetPropertyValue("SortOrder", ref _SortOrder, value); }
        }
        #endregion

        #region Surrogate
        private bool _Surrogate;
        public bool Surrogate
        {
            get { return _Surrogate; }
            set { SetPropertyValue("Surrogate", ref _Surrogate, value); }
        }
        #endregion

        #region InternalStandard
        private bool _InternalStandard;
        public bool InternalStandard
        {
            get { return _InternalStandard; }
            set { SetPropertyValue("InternalStandard", ref _InternalStandard, value); }
        }
        #endregion

        #region STDConc
        private string _STDConc;
        public string STDConc
        {
            get { return _STDConc; }
            set { SetPropertyValue("STDConc", ref _STDConc, value); }
        }
        #endregion

        #region STDConcUnit
        private Unit _STDConcUnit;
        public Unit STDConcUnit
        {
            get { return _STDConcUnit; }
            set { SetPropertyValue("STDConcUnit", ref _STDConcUnit, value); }
        }
        #endregion

        #region STDVolAdd
        private string _STDVolAdd;
        public string STDVolAdd
        {
            get { return _STDVolAdd; }
            set { SetPropertyValue("STDVolAdd", ref _STDVolAdd, value); }
        }
        #endregion

        #region STDVolUnit
        private Unit _STDVolUnit;
        public Unit STDVolUnit
        {
            get { return _STDVolUnit; }
            set { SetPropertyValue("STDVolUnit", ref _STDVolUnit, value); }
        }
        #endregion


        #region SpikeAmount
        private double _SpikeAmount;
        public double SpikeAmount
        {
            get { return _SpikeAmount; }
            set { SetPropertyValue("SpikeAmount", ref _SpikeAmount, value); }
        }
        #endregion


        #region SpikeAmountUnit
        private Unit _SpikeAmountUnit;
        public Unit SpikeAmountUnit
        {
            get { return _SpikeAmountUnit; }
            set { SetPropertyValue("SpikeAmountUnit", ref _SpikeAmountUnit, value); }
        }
        #endregion

        #region RecLCLimit
        private string _RecLCLimit;
        public string RecLCLimit
        {
            get { return _RecLCLimit; }
            set { SetPropertyValue("RecLCLimit", ref _RecLCLimit, value); }
        }
        #endregion

        #region RecHCLimit
        private string _RecHCLimit;
        public string RecHCLimit
        {
            get { return _RecHCLimit; }
            set { SetPropertyValue("RecHCLimit", ref _RecHCLimit, value); }
        }
        #endregion

        #region RPDLCLimit
        private string _RPDLCLimit;
        public string RPDLCLimit
        {
            get { return _RPDLCLimit; }
            set { SetPropertyValue("RPDLCLimit", ref _RPDLCLimit, value); }
        }
        #endregion


        #region RPDHCLimit
        private string _RPDHCLimit;
        public string RPDHCLimit
        {
            get { return _RPDHCLimit; }
            set { SetPropertyValue("RPDHCLimit", ref _RPDHCLimit, value); }
        }
        #endregion

        #region LowCLimit
        private string _LowCLimit;
        public string LowCLimit
        {
            get { return _LowCLimit; }
            set { SetPropertyValue("LowCLimit", ref _LowCLimit, value); }
        }
        #endregion

        #region HighCLimit
        private string _HighCLimit;
        public string HighCLimit
        {
            get { return _HighCLimit; }
            set { SetPropertyValue("HighCLimit", ref _HighCLimit, value); }
        }
        #endregion

        #region RELCLimit
        private string _RELCLimit;
        public string RELCLimit
        {
            get { return _RELCLimit; }
            set { SetPropertyValue("RELCLimit", ref _RELCLimit, value); }
        }
        #endregion

        #region REHCLimit
        private string _REHCLimit;
        public string REHCLimit
        {
            get { return _REHCLimit; }
            set { SetPropertyValue("REHCLimit", ref _REHCLimit, value); }
        }
        #endregion


        #region SigFig
        private string _SigFig;
        public string SigFig
        {
            get { return _SigFig; }
            set { SetPropertyValue("SigFig", ref _SigFig, value); }
        }
        #endregion


        #region CutOff
        private string _CutOff;
        public string CutOff
        {
            get { return _CutOff; }
            set { SetPropertyValue("CutOff", ref _CutOff, value); }
        }
        #endregion

        #region Decimal
        private string _Decimal;
        public string Decimal
        {
            get { return _Decimal; }
            set { SetPropertyValue("Decimal", ref _Decimal, value); }
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

        #region Comment
        private DateTime _RetireDate;
        public DateTime RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue("RetireDate", ref _RetireDate, value); }
        }
        #endregion

    }
}