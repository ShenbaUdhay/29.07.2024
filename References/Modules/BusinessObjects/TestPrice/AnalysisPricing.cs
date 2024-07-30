using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting.Quotes
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class AnalysisPricing : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        QuotesInfo quoteinfo = new QuotesInfo();
        int parametercnt = 0;
        public AnalysisPricing(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        public enum Chargetypeprop
        {
            Test,
            Parameter,
        }
        #region Sort
        private uint _Sort;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public uint Sort
        {
            get { return _Sort; }
            set { SetPropertyValue(nameof(Sort), ref _Sort, value); }
        }
        #endregion

        #region TestDescription
        private string _TestDescription;
        [ImmediatePostData]
        public string TestDescription
        {
            get
            {
                //if (Test != null)
                //{
                //    _TestDescription = Test.Comment;
                //}
                return _TestDescription;
            }
            set { SetPropertyValue(nameof(TestDescription), ref _TestDescription, value); }
        }
        #endregion

        #region Qty
        private uint _Qty;
        [ImmediatePostData]
        public uint Qty
        {
            get { return _Qty; }
            set { SetPropertyValue(nameof(Qty), ref _Qty, value); }
        }
        #endregion

        #region PriceCode
        private string _PriceCode;
        public string PriceCode
        {
            get { return _PriceCode; }
            set { SetPropertyValue(nameof(PriceCode), ref _PriceCode, value); }
        }
        #endregion
        public XPCollection<VisualMatrix> visualMatrices
        {
            get
            {
                if (Matrix != null)
                {
                    return new XPCollection<VisualMatrix>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=?", Matrix.MatrixName));
                }
                else
                {
                    return null;
                }
            }
        }
        #region Matrix
        private Matrix _Matrix;
        [ImmediatePostData]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
        }
        #endregion

        #region SampleMatrix
        public XPCollection<VisualMatrix> SampleMatrixes
        {
            get
            {
                return new XPCollection<VisualMatrix>(Session, CriteriaOperator.Parse("[IsRetired] <> True Or [IsRetired] Is Null"));
            }
        }
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "SampleMatries" && SampleMatrixes != null && SampleMatrixes.Count > 0)
            {
                Properties = SampleMatrixes.OrderBy(i => i.VisualMatrixName).ToDictionary(x => (Object)x.Oid, x => x.VisualMatrixName);
                //foreach (VisualMatrix objSampleMatrix in SampleMatrixes.Where(i => i.VisualMatrixName != null).OrderBy(i => i.VisualMatrixName).ToList())
                //{
                //    if (!Properties.ContainsKey(objSampleMatrix.Oid))
                //    {
                //        Properties.Add(objSampleMatrix.Oid, objSampleMatrix.VisualMatrixName);
                //    }
                //}
            }
            return Properties;

        }
        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        private string _SampleMatries;
        [XafDisplayName("Sample Matrice")]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string SampleMatries
        {
            get { return _SampleMatries; }
            set { SetPropertyValue(nameof(SampleMatries), ref _SampleMatries, value); }
        }
        private VisualMatrix _SampleMatrix;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[DataSourceProperty(nameof(visualMatrices))]
        [ImmediatePostData]
        public VisualMatrix SampleMatrix
        {
            get { return _SampleMatrix; }
            set { SetPropertyValue(nameof(SampleMatrix), ref _SampleMatrix, value); }
        }
        #endregion
        #region Test
        private TestMethod _Test;
        [ImmediatePostData]
        //[DataSourceProperty("TestDataSource")]
        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        ////[Browsable(false)]
        ////[NonPersistent]
        ////public XPCollection<TestMethod> TestDataSource
        ////{
        ////    get
        ////    {
        ////        if (Matrix != null && Test == null)
        ////        {
        ////            XPCollection<TestMethod> lstTest = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=?  and [MatrixName.GCRecord] is null", Matrix.MatrixName));
        ////            List<Guid> lstTestOid = new List<Guid>();
        ////            List<string> lstSM = new List<string>();
        ////            foreach (TestMethod objOid in lstTest)
        ////            {
        ////                if (!lstSM.Contains(objOid.TestName))
        ////                {
        ////                    lstSM.Add(objOid.TestName);
        ////                    lstTestOid.Add(objOid.Oid);
        ////                }
        ////            }
        ////            XPView testsView = new XPView(Session, typeof(TestMethod));
        ////            testsView.Criteria = new InOperator("Oid", lstTest.Select(i => i.Oid));
        ////            testsView.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "TestName", true, true));
        ////            testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
        ////            List<object> groups = new List<object>();
        ////            foreach (ViewRecord rec in testsView)
        ////                groups.Add(rec["Toid"]);
        ////            return new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
        ////        }
        ////        else
        ////        {
        ////            return null;
        ////        }
        ////    }
        ////}
        #endregion

        #region IsGroup
        private bool _IsGroup;
        [ImmediatePostData]
        public bool IsGroup
        {
            get { return _IsGroup; }
            set { SetPropertyValue(nameof(IsGroup), ref _IsGroup, value); }
        }
        #endregion

        #region Method
        private Method _Method;
        [ImmediatePostData]
        //[DataSourceProperty("MethodDataSource")]
        public Method Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }


        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<Method> MethodDataSource
        //{
        //    get
        //    {
        //        if (Test != null && Matrix != null && Method == null && IsGroup == false)
        //        {
        //            XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName] =?", Test.TestName, Matrix.MatrixName));
        //            List<Guid> lstmethodoid = new List<Guid>();
        //            foreach (TestMethod objtestmethod in tests.ToList())
        //            {
        //                if (objtestmethod != null && objtestmethod.MethodName != null)
        //                {
        //                    lstmethodoid.Add(objtestmethod.MethodName.Oid);
        //                }
        //            }
        //            if (lstmethodoid.Count > 0)
        //            {
        //                XPCollection<Method> lstTests = new XPCollection<Method>(Session, new InOperator("Oid", lstmethodoid));
        //                lstTests.OrderBy(i => i.MethodNumber);
        //                return lstTests;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        #endregion

        private string _NPMethod;
        [NonPersistent]
        public string NPMethod
        {
            get
            {
                if (Method != null)
                {
                    _NPMethod = Method.MethodNumber;
                }
                return _NPMethod;
            }
            set { SetPropertyValue(nameof(NPMethod), ref _NPMethod, value); }
        }

        #region Component
        private Component _Component;
        [ImmediatePostData]
        //[DataSourceProperty("ComponentDataSource")]
        public Component Component
        {
            get { return _Component; }
            set { SetPropertyValue(nameof(Component), ref _Component, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Component> ComponentDataSource
        {
            get
            {
                if (Test != null && Matrix != null && Method != null)
                {
                    List<string> lstcompvalue = new List<string>();
                    XPCollection<Component> lstcomp = new XPCollection<Component>(Session, CriteriaOperator.Parse("[TestMethod.Oid]=?", Test.Oid));

                    foreach (Component objcomp in lstcomp.ToList())
                    {
                        if (!lstcompvalue.Contains(objcomp.Components))
                        {
                            lstcompvalue.Add(objcomp.Components);
                        }
                    }
                    Component objcompdef = Session.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                    if (objcompdef != null && !lstcomp.Contains(objcompdef) && !lstcompvalue.Contains(objcompdef.Components))
                    {
                        lstcompvalue.Add(objcompdef.Components);
                        lstcomp.Add(objcompdef);
                    }
                    return lstcomp;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Parameter
        private string _Parameter;
        public string Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue(nameof(Parameter), ref _Parameter, value); }
        }
        #endregion

        #region ParameterGuid
        private string _ParameterGuid;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [Size(int.MaxValue)]
        public string ParameterGuid
        {
            get { return _ParameterGuid; }
            set { SetPropertyValue(nameof(ParameterGuid), ref _ParameterGuid, value); }
        }
        #endregion

        #region ChargeType
        private ChargeType _ChargeType;
        public ChargeType ChargeType
        {
            get { return _ChargeType; }
            set { SetPropertyValue(nameof(ChargeType), ref _ChargeType, value); }
        }
        #endregion

        #region UnitPrice
        private decimal _UnitPrice;
        //[ImmediatePostData]
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref _UnitPrice, Math.Round(value, 2)); }
        }
        #endregion

        #region NPUnitPrice
        private decimal _NPUnitPrice;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[NonPersistent]
        public decimal NPUnitPrice
        {
            get { return _NPUnitPrice; }
            set { SetPropertyValue(nameof(NPUnitPrice), ref _NPUnitPrice, Math.Round(value, 2)); }
        }
        #endregion

        #region TierNo
        private uint _TierNo;
        public uint TierNo
        {
            get { return _TierNo; }
            set { SetPropertyValue(nameof(TierNo), ref _TierNo, value); }
        }
        #endregion

        #region From
        private uint _From;
        public uint From
        {
            get { return _From; }
            set { SetPropertyValue(nameof(From), ref _From, value); }
        }
        #endregion

        #region To
        private uint _To;
        public uint To
        {
            get { return _To; }
            set { SetPropertyValue(nameof(To), ref _To, value); }
        }
        #endregion

        #region TierPrice
        private decimal _TierPrice;
        //[ImmediatePostData]
        public decimal TierPrice
        {
            get { return _TierPrice; }
            set { SetPropertyValue(nameof(TierPrice), ref _TierPrice, Math.Round(value, 2)); }
        }
        #endregion

        #region Prep1Charge
        private decimal _Prep1Charge;
        //[ImmediatePostData]
        public decimal Prep1Charge
        {
            get { return _Prep1Charge; }
            set { SetPropertyValue(nameof(Prep1Charge), ref _Prep1Charge, Math.Round(value, 2)); }
        }
        #endregion

        #region Prep2Charge
        private decimal _Prep2Charge;
        //[ImmediatePostData]
        public decimal Prep2Charge
        {
            get { return _Prep2Charge; }
            set { SetPropertyValue(nameof(Prep2Charge), ref _Prep2Charge, Math.Round(value, 2)); }
        }
        #endregion

        #region TotalTierPrice
        private decimal _TotalTierPrice;
        //[ImmediatePostData]
        public decimal TotalTierPrice
        {
            get
            { return _TotalTierPrice; }
            set { SetPropertyValue(nameof(TotalTierPrice), ref _TotalTierPrice, Math.Round(value, 2)); }
        }
        #endregion


        #region NPTotalPrice
        private decimal _NPTotalPrice;
        //[NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal NPTotalPrice
        {
            get { return _NPTotalPrice; }
            set { SetPropertyValue(nameof(NPTotalPrice), ref _NPTotalPrice, Math.Round(value, 2)); }
        }
        #endregion

        #region NPSurcharge
        private decimal _NPSurcharge;
        //[NonPersistent]
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public decimal NPSurcharge
        {
            get
            {
                return _NPSurcharge;
            }
            set { SetPropertyValue(nameof(NPSurcharge), ref _NPSurcharge, Math.Round(value, 2)); }
        }
        #endregion

        #region Remark
        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        #endregion

        #region Amount
        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { SetPropertyValue(nameof(Amount), ref _Amount, value); }
        }
        #endregion

        #region TAT
        private TurnAroundTime _TAT;
        [ImmediatePostData(true)]
        [DataSourceProperty("TATDatasource")]
        public TurnAroundTime TAT
        {
            get
            {
                return _TAT;
            }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public XPCollection<TurnAroundTime> TATDatasource
        {
            get
            {
                if (Test != null && Matrix != null && Method != null && Component != null)
                {
                    XPCollection<TurnAroundTime> lstTest = new XPCollection<TurnAroundTime>(Session, CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [Method.MethodName.MethodNumber] = ? And [Component.Components] = ?", Matrix.MatrixName, Test.TestName, Method.MethodNumber, Component.Components));
                    XPView testsView = new XPView(Session, typeof(TurnAroundTime));
                    testsView.Criteria = new InOperator("Oid", lstTest.Select(i => i.Oid));
                    testsView.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "TAT", true, true));
                    testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    List<object> groups = new List<object>();
                    foreach (ViewRecord rec in testsView)
                        groups.Add(rec["Toid"]);
                    return new XPCollection<TurnAroundTime>(Session, new InOperator("Oid", groups));
                }
                else
                if (Test != null && Matrix != null && IsGroup == true)
                {
                    XPCollection<TurnAroundTime> lstTest = new XPCollection<TurnAroundTime>(Session, CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test.TestName] = ? And [IsGroup] = 'True'", Matrix.MatrixName, Test.TestName));
                    XPView testsView = new XPView(Session, typeof(TurnAroundTime));
                    testsView.Criteria = new InOperator("Oid", lstTest.Select(i => i.Oid));
                    testsView.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "TAT", true, true));
                    testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                    List<object> groups = new List<object>();
                    foreach (ViewRecord rec in testsView)
                        groups.Add(rec["Toid"]);
                    return new XPCollection<TurnAroundTime>(Session, new InOperator("Oid", groups));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Priority
        private Priority _Priority;
        public Priority Priority
        {
            get { return _Priority; }
            set { SetPropertyValue("Priority", ref _Priority, value); }
        }
        #endregion

        #region NPPriority
        private string _NPPriority;
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string NPPriority
        {
            get { return _NPPriority; }
            set { SetPropertyValue("NPPriority", ref _NPPriority, value); }
        }
        #endregion

        #region Discount 
        private decimal _Discount;
        [ImmediatePostData]
        public decimal Discount
        {
            get { return _Discount; }
            set { SetPropertyValue(nameof(Discount), ref _Discount, Math.Round(value, 2)); }
        }
        #endregion

        #region DiscountAmount 
        private decimal _DiscountAmount;
        [ImmediatePostData]
        public decimal DiscountAmount
        {
            get
            {
                if (Discount != 0)
                {
                    _DiscountAmount = TotalTierPrice * (Discount / 100);
                }
                return _DiscountAmount;
            }
            set { SetPropertyValue(nameof(DiscountAmount), ref _DiscountAmount, Math.Round(value, 2)); }
        }
        #endregion

        #region FinalAmount
        private decimal _FinalAmount;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        //[ImmediatePostData]
        public decimal FinalAmount
        {
            get { return _FinalAmount; }
            set { SetPropertyValue(nameof(FinalAmount), ref _FinalAmount, Math.Round(value, 2)); }
        }
        #endregion

        #region Status
        private string _Status;
        public string Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        #endregion

        #region Createdby
        private CustomSystemUser _Createdby;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser Createdby
        {
            get { return _Createdby; }
            set { SetPropertyValue(nameof(Createdby), ref _Createdby, value); }
        }
        #endregion

        #region Modifiedby
        private CustomSystemUser _Modifiedby;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser Modifiedby
        {
            get { return _Modifiedby; }
            set { SetPropertyValue(nameof(Modifiedby), ref _Modifiedby, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region CRMQuotes
        private CRMQuotes _CRMQuotes;
        [Association("CRMQuotes-AnalysisPricing")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        [ImmediatePostData]
        public CRMQuotes CRMQuotes
        {
            get { return _CRMQuotes; }
            set { SetPropertyValue(nameof(CRMQuotes), ref _CRMQuotes, value); }
        }
        #endregion

        #region TestPriceSurcharge
        private TestPriceSurcharge _TestPriceSurcharge;
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public TestPriceSurcharge TestPriceSurcharge
        {
            get { return _TestPriceSurcharge; }
            set { SetPropertyValue(nameof(TestPriceSurcharge), ref _TestPriceSurcharge, value); }
        }
        #endregion 
    }
}