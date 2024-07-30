using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.BusinessObjects.Setting.Invoicing
{
    [DefaultClassOptions]

    public class InvoicingAnalysisCharge : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public InvoicingAnalysisCharge(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region JobID
        private string _JobID;
        public string JobID
        {
            get { return _JobID; }
            set { SetPropertyValue(nameof(JobID), ref _JobID, value); }
        }
        #endregion
        #region Qty
        private uint _Qty;

        public uint Qty
        {
            get
            {
                if (JobID != null && !Session.IsObjectsSaving && Invoice == null)
                {
                    List<string> lstJobId = JobID.Split(',').ToList();
                    if (lstJobId.Count > 0)
                    {
                        //if (Matrix != null && Test != null && Method != null && Component != null)
                        //{
                        //    _Qty = Convert.ToUInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), new GroupOperator(GroupOperatorType.And, new InOperator("Samplelogin.JobID.JobID", lstJobId.Select(i => i.Replace(" ", ""))), CriteriaOperator.Parse("[Testparameter.TestMethod.MatrixName.MatrixName] = ? And [Testparameter.TestMethod.TestName] = ? And [Testparameter.TestMethod.MethodName.MethodNumber] = ? And [Testparameter.Component.Components] = ?",
                        //                      Matrix.MatrixName, Test.TestName, Method.MethodNumber, Component.Components))));

                        //}
                        if (IsGroup == false)
                        {
                            XPCollection<SampleParameter> lstSamples = new XPCollection<SampleParameter>(Session);
                            lstSamples.Criteria = new InOperator("Samplelogin.JobID.JobID", (lstJobId.Select(i => i.Replace(" ", ""))));
                            if (Matrix != null && Test != null && Method != null && Component != null)
                            {
                                _Qty = Convert.ToUInt32(lstSamples.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName
                                == Matrix.MatrixName && i.Testparameter.TestMethod.TestName == Test.TestName &&
                                i.Testparameter.TestMethod.MethodName.MethodNumber == Method.MethodNumber &&
                                i.Testparameter.Component.Components == Component.Components && i.InvoiceIsDone == false &&i.Samplelogin.ExcludeInvoice==false  && i.IsGroup == false)
                                    .Select(m => new { m.Samplelogin.Oid, m.Testparameter.TestMethod.MatrixName.MatrixName, m.Testparameter.TestMethod.TestName, m.Testparameter.TestMethod.MethodName.MethodNumber }).Distinct().Count());
                                //int a = lstSamples.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null && i.Samplelogin.JobID.ClientName != null).Select(i => i.Samplelogin.JobID.ClientName.Oid).Distinct().Count();
                            }
                        }
                        else if (IsGroup == true)
                        {
                            XPCollection<SampleParameter> lstSamples = new XPCollection<SampleParameter>(Session);
                            lstSamples.Criteria = new InOperator("Samplelogin.JobID.JobID", (lstJobId.Select(i => i.Replace(" ", ""))));
                            if (Matrix != null && Test != null)
                            {
                                _Qty = Convert.ToUInt32(lstSamples.Where(i => i.GroupTest != null && i.IsGroup == true && i.GroupTest.TestMethod.MatrixName.MatrixName
                                == Matrix.MatrixName && i.GroupTest.TestMethod.TestName == Test.TestName &&
                                i.InvoiceIsDone == false &&i.Samplelogin.ExcludeInvoice==false && i.IsGroup == true)
                                    .Select(m => new { m.Samplelogin.Oid, m.GroupTest.TestMethod.MatrixName.MatrixName, m.GroupTest.TestMethod.TestName }).Distinct().Count());

                            }
                        }
                        //List<Guid> lstTestParamOids = lstSampleParams.GroupBy(p => new { p.Testparameter.TestMethod.MatrixName.MatrixName, p.Testparameter.TestMethod.MethodName.MethodName, p.Testparameter.TestMethod.TestName, p.Testparameter.Component.Components }).Select(g => g.First().Testparameter.Oid).ToList();
                    }
                }
                return _Qty;
            }
            set { SetPropertyValue(nameof(Qty), ref _Qty, value); }
        }
        #endregion
        #region Qty
        private uint _fQty;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        [NonPersistent]
        public uint fQty
        {
            get
            {
                if (JobID != null && !Session.IsObjectsSaving && Invoice == null)
                {
                    if (IsGroup == false)
                    {
                        XPCollection<SampleParameter> lstSamples = new XPCollection<SampleParameter>(Session);
                        //lstSamples.Criteria = new InOperator("Samplelogin.JobID.JobID", (lstJobId.Select(i => i.Replace(" ", ""))));
                        lstSamples.Criteria = CriteriaOperator.Parse("[Samplelogin.JobID.JobID]=?", JobID);
                        if (Matrix != null && Test != null && Method != null && Component != null)
                        {
                            _fQty = Convert.ToUInt32(lstSamples.Where(i => i.Testparameter.TestMethod.MatrixName.MatrixName
                            == Matrix.MatrixName && i.Testparameter.TestMethod.TestName == Test.TestName &&
                            i.Testparameter.TestMethod.MethodName.MethodNumber == Method.MethodNumber &&
                            i.Testparameter.Component.Components == Component.Components && i.IsGroup == false && i.Samplelogin.ExcludeInvoice == false)
                                .Select(m => new { m.Samplelogin.Oid, m.Testparameter.TestMethod.MatrixName.MatrixName, m.Testparameter.TestMethod.TestName, m.Testparameter.TestMethod.MethodName.MethodNumber }).Distinct().Count());
                        }
                    }
                    else if (IsGroup == true)
                    {
                        XPCollection<SampleParameter> lstSamples = new XPCollection<SampleParameter>(Session);
                        lstSamples.Criteria = CriteriaOperator.Parse("[Samplelogin.JobID.JobID]=?", JobID);
                        if (Matrix != null && Test != null)
                        {
                            _fQty = Convert.ToUInt32(lstSamples.Where(i => i.GroupTest != null && i.IsGroup == true && i.Samplelogin.ExcludeInvoice == false && i.GroupTest.TestMethod.MatrixName.MatrixName
                            == Matrix.MatrixName && i.GroupTest.TestMethod.TestName == Test.TestName &&
                             i.IsGroup == true)
                                .Select(m => new { m.Samplelogin.Oid, m.GroupTest.TestMethod.MatrixName.MatrixName, m.GroupTest.TestMethod.TestName }).Distinct().Count());
                        }
                    }
                }
                return _fQty;
            }
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
        #region Matrix
        private Matrix _Matrix;
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
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
        #region Component
        private Component _Component;
        public Component Component
        {
            get { return _Component; }
            set { SetPropertyValue(nameof(Component), ref _Component, value); }
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
        #region ChargeType
        private ChargeType _ChargeType;
        public ChargeType ChargeType
        {
            get { return _ChargeType; }
            set { SetPropertyValue(nameof(ChargeType), ref _ChargeType, value); }
        }
        #endregion
        #region Priority
        private Priority _Priority;
        public Priority Priority
        {
            get { return _Priority; }
            set { SetPropertyValue(nameof(Priority), ref _Priority, value); }
        }
        #endregion
        #region UnitPrice
        private decimal _UnitPrice;
        //[ImmediatePostData]
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref _UnitPrice, value); }
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
        public decimal TierPrice
        {
            get { return _TierPrice; }
            set { SetPropertyValue(nameof(TierPrice), ref _TierPrice, value); }
        }
        #endregion

        #region Prep1Price
        private decimal _Prep1Price;
        public decimal Prep1Price
        {
            get { return _Prep1Price; }
            set { SetPropertyValue(nameof(Prep1Price), ref _Prep1Price, value); }
        }
        #endregion

        #region Prep2Price
        private decimal _Prep2Price;
        public decimal Prep2Price
        {
            get { return _Prep2Price; }
            set { SetPropertyValue(nameof(Prep2Price), ref _Prep2Price, value); }
        }
        #endregion
        #region TotalUnitPrice
        private decimal _TotalUnitPrice;
        public decimal TotalUnitPrice
        {
            get
            {
                return _TotalUnitPrice;
            }
            set { SetPropertyValue(nameof(TotalUnitPrice), ref _TotalUnitPrice, value); }
        }
        #endregion
        #region Discount 
        private decimal _Discount;
        [ImmediatePostData(true)]
        public decimal Discount
        {
            get { return _Discount; }
            set { SetPropertyValue(nameof(Discount), ref _Discount, value); }
        }
        #endregion
        #region Amount
        private decimal _Amount;
        [ImmediatePostData(true)]
        public decimal Amount
        {
            get
            {

                return _Amount;
            }
            set { SetPropertyValue(nameof(Amount), ref _Amount, value); }
        }
        #endregion

        #region Invoice
        private Invoicing _Invoice;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public Invoicing Invoice
        {
            get
            {

                return _Invoice;
            }
            set { SetPropertyValue(nameof(Invoice), ref _Invoice, value); }
        }
        #endregion
        #region Testparameter
        private Testparameter _Testparameter;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public Testparameter Testparameter
        {
            get
            {

                return _Testparameter;
            }
            set { SetPropertyValue(nameof(Testparameter), ref _Testparameter, value); }
        }
        #endregion
        //#region Count
        //private int _Count;
        //[VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        //public int Count
        //{
        //    get
        //    {
        //        if(Invoice!=null&& Invoice.JobID!=null)
        //        {
        //            List<string> lstJobId = Invoice.JobID.Split(',').ToList();
        //            if(lstJobId.Count>0)
        //            {
        //                XPCollection<SampleParameter> lstSamples = new XPCollection<SampleParameter>(Session);
        //                lstSamples.Criteria = new InOperator("Samplelogin.JobID.Oid", lstJobId.Select(i => i.Replace(" ", "")));
        //                if (Matrix!=null && Test!=null && Method!=null && Component!=null)
        //                {
        //                    _Count= lstSamples.Where(i => i.Testparameter.TestMethod.Matrix == Matrix && i.Testparameter.TestMethod.TestName == Test.TestName && i.Testparameter.TestMethod.MethodName == Method && i.Testparameter.Component == Component).Select(i => i.Samplelogin.SampleID).Distinct().Count();
        //                }
        //                //List<Guid> lstTestParamOids = lstSampleParams.GroupBy(p => new { p.Testparameter.TestMethod.MatrixName.MatrixName, p.Testparameter.TestMethod.MethodName.MethodName, p.Testparameter.TestMethod.TestName, p.Testparameter.Component.Components }).Select(g => g.First().Testparameter.Oid).ToList();
        //            }
        //        }
        //        return _Count;
        //    }
        //    set { SetPropertyValue(nameof(Count), ref _Count, value); }
        //}
        //#endregion
        #region IsGroup
        private bool _IsGroup;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public bool IsGroup
        {
            get
            { return _IsGroup; }
            set { SetPropertyValue(nameof(IsGroup), ref _IsGroup, value); }
        }
        #endregion
        #region Surcharge
        private int _Surcharge;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public int Surcharge
        {
            get
            { return _Surcharge; }
            set { SetPropertyValue(nameof(Surcharge), ref _Surcharge, value); }
        }
        #endregion
        #region Surcharge Price
        private decimal _SurchargePrice;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public decimal SurchargePrice
        {
            get { return _SurchargePrice; }
            set { SetPropertyValue("SurchargePrice", ref _SurchargePrice, value); }
        }
        #endregion
        #region TAT
        private TurnAroundTime _TAT;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public TurnAroundTime TAT
        {
            get
            { return _TAT; }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }
        #endregion
        #region Description
        private string _Description;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        #endregion
    }
}