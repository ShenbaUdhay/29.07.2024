using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    public class DOC : BaseObject/*, ICheckedListBoxItemsProvider*/
    {
        InfoClass.NavigationRefresh objnavigationRefresh = new InfoClass.NavigationRefresh();

        public DOC(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Analyst = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            DateAnalyzed = DateTime.Now;
            DateSubmitted = DateTime.Now;
            DateValidated = DateTime.Now;
            DatePrepared = DateTime.Now;
            ValidatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            DOCReport = "Available";
            Certificate = "Available";
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            //ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            //ModifiedDate = DateTime.Now;
            if (string.IsNullOrEmpty(DOCID))
            {
                string MaxID = string.Empty;
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(DOCID,5))");
                int tempID = Convert.ToInt32(Session.Evaluate(typeof(DOC), criteria, null));
                //string tempid1 = (Convert.ToInt32(Session.Evaluate(typeof(DOC), CriteriaOperator.Parse("Max(DOCID)"), null)) + 1).ToString();
                var curdate = DateTime.Now.ToString("yy");
                //var predate = tempid1.Substring(0, 6);
                if (tempID > 0)
                {
                    MaxID = (tempID + 1).ToString();

                    if (MaxID.Length == 1)
                    {
                        MaxID = "DOC" + curdate.ToString() + "000" + MaxID;
                    }
                    else if (MaxID.Length == 2)
                    {
                        MaxID = "DOC" + curdate.ToString() + "00" + MaxID;
                    }
                    else if (MaxID.Length == 3)
                    {
                        MaxID = "DOC" + curdate.ToString() + "0" + MaxID;
                    }
                    else
                    {
                        MaxID = "DOC" + curdate.ToString() + MaxID;
                    }
                }
                else
                {
                    MaxID = "DOC" + curdate.ToString() + "0001";
                }

                DOCID = MaxID;


            }
            //if (objnavigationRefresh.ClickedNavigationItem == "DOC")
            //{

            //}

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        public enum DOCstatus
        {
            PendingSubmission,
            PendingValidation,
            Pass,
            Fail
        }

        #region 'DOCID'
        private string _DOCID;
        public string DOCID
        {
            get { return _DOCID; }
            set { SetPropertyValue(nameof(DOCID), ref _DOCID, value); }
        }
        #endregion     

        #region 'Test'
        private TestMethod _Test;
        [ImmediatePostData]
        [VisibleInListView(false)]
        [DataSourceProperty("TestDataSource")]
        [RuleRequiredField]
        public TestMethod Test
        {
            get
            {
                if (_Test == null)
                {
                    Method = null;

                }
                return _Test;
            }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (Matrix != null && Test == null)
                {
                    List<object> groups = new List<object>();
                    using (XPView lstview = new XPView(Session, typeof(TestMethod)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And([IsGroup] <> True Or [IsGroup] Is Null)", Matrix.MatrixName);
                        lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                    }
                    if (groups.Count == 0)
                    {
                        XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=?", Matrix.MatrixName));
                        return tests;
                    }
                    else
                    {
                        XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
                        return tests;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region 'Method'
        private Method _Method;
        //[ReadOnly(true)]
        //[Appearance("Method", Enabled = false, Context = "DetailView")]
        [ImmediatePostData]
        [VisibleInListView(false)]
        [DataSourceProperty(nameof(MethodDataSource))]
        [RuleRequiredField]
        public Method Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Method> MethodDataSource
        {
            get
            {
                if (Matrix != null && Test != null)
                {
                    //XPCollection<Method> methods = new XPCollection<Method>(Session, CriteriaOperator.Parse("Not IsNullOrEmpty([MethodName]) AND GCRecord Is NULL"));
                    XPCollection<TestMethod> methods = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] = ? And [MatrixName.Oid] = ?",Test.TestName,Matrix.Oid));
                    List<Guid> lstmethod = new List<Guid>();
                    foreach (TestMethod objtm in methods)
                    {
                        if (objtm.MethodName != null)
                        {
                            lstmethod.Add(objtm.MethodName.Oid);
                        }
                    }
                    if (lstmethod.Count > 0)
                    {
                        return new XPCollection<Method>(Session, new InOperator("Oid", lstmethod));
                    }
                }
                return null;
            }
        }
        #endregion

        #region Matrix
        private Matrix _Matrix;
        [ImmediatePostData]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [DataSourceProperty(nameof(MatrixDataSource))]
        [RuleRequiredField]
        public Matrix Matrix
        {
            get
            {
                if (_Matrix == null)
                {
                    Test = null;
                    Method = null;
                    QCBatches = null;
                }
                return _Matrix;
            }
            set { SetPropertyValue("Matrix", ref _Matrix, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Matrix> MatrixDataSource
        {
            get
            {
                if (Matrix == null /*&& Matrix != null && Matrix != null*/)
                {
                    XPCollection<Matrix> matrixs = new XPCollection<Matrix>(Session, CriteriaOperator.Parse("Not IsNullOrEmpty([MatrixName])"));
                    List<Guid> lstmethod = new List<Guid>();
                    List<string> ids = matrixs.Select(i => i.MatrixName.ToString()).Distinct().ToList();
                    foreach (string objids in ids.ToList())//tests.Where(a => a.TestName !=null).Distinct())
                    {
                        Matrix objtm = Session.FindObject<Matrix>(CriteriaOperator.Parse("[MatrixName] = ?", objids));
                        lstmethod.Add(objtm.Oid);
                    }
                    List<Guid> ids1 = lstmethod.Select(i => new Guid(i.ToString())).ToList();
                    return new XPCollection<Matrix>(Session, new InOperator("Oid", ids1));
                    //return tests;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion


        #region JobID
        private Samplecheckin _JobID;
        [ReadOnly(true)]
        //[Appearance("JobID", Enabled = false, Context = "DetailView")]
        public Samplecheckin JobID
        {
            get
            {
                return _JobID;
            }
            set
            {
                SetPropertyValue<Samplecheckin>("JobID", ref _JobID, value);
            }
        }
        #endregion
        #region strJobID
        private string _strJobID;
        [ReadOnly(true)]
        [XafDisplayName("JobID")]
        //[Appearance("JobID", Enabled = false, Context = "DetailView")]
        public string strJobID
        {
            get
            {
                return _strJobID;
            }
            set
            {
                SetPropertyValue<string>("strJobID", ref _strJobID, value);
            }
        }
        #endregion

        #region 'Analyst'
        private Employee _Analyst;
        //[ReadOnly(true)]
        //[Appearance("Analyst", Enabled = false, Context = "DetailView")]
        public Employee Analyst
        {
            get { return _Analyst; }
            set { SetPropertyValue(nameof(Analyst), ref _Analyst, value); }
        }
        #endregion

        #region 'DateAnalyzed'

        private DateTime? _DateAnalyzed;
        //[ReadOnly(true)]
        //[Appearance("DateAnalyzed", Enabled = false, Context = "DetailView")]
        public DateTime? DateAnalyzed
        {
            get { return _DateAnalyzed; }
            set { SetPropertyValue(nameof(DateAnalyzed), ref _DateAnalyzed, value); }
        }
        #endregion

        #region 'DateSubmitted'
        private DateTime? _DateSubmitted;
        //[ReadOnly(true)]
        //[Appearance("DateSubmitted", Enabled = false, Context = "DetailView")]
        public DateTime? DateSubmitted
        {
            get { return _DateSubmitted; }
            set { SetPropertyValue(nameof(DateSubmitted), ref _DateSubmitted, value); }
        }
        #endregion

        #region 'DateValidated'
        private DateTime? _DateValidated;
        //[ReadOnly(true)]
        //[Appearance("DateValidated", Enabled = false, Context = "DetailView")]
        public DateTime? DateValidated
        {
            get { return _DateValidated; }
            set { SetPropertyValue(nameof(DateValidated), ref _DateValidated, value); }
        }
        #endregion

        #region 'ValidatedBy'
        private Employee _ValidatedBy;
        //[ReadOnly(true)]
        //[Appearance("ValidatedBy", Enabled = false, Context = "DetailView")]
        public Employee ValidatedBy
        {
            get { return _ValidatedBy; }
            set { SetPropertyValue(nameof(ValidatedBy), ref _ValidatedBy, value); }
        }
        #endregion

        #region 'DOCReport'
        private string _DOCReport;
        [EditorAlias("HyperLinkStringPropertyEditor")]
        //[ReadOnly(true)]
        //[Appearance("DOCReport", Enabled = false, Context = "DetailView")]
        public string DOCReport
        {
            get { return _DOCReport; }
            set { SetPropertyValue<string>("DOCReport", ref _DOCReport, value); }
        }
        #endregion

        #region 'Certificate'
        private string _Certificate;
        [EditorAlias("HyperLinkStringPropertyEditor")]
        //[ReadOnly(true)]
        //[Appearance("Certificate", Enabled = false, Context = "DetailView")]
        public string Certificate
        {
            get { return _Certificate; }
            set { SetPropertyValue<string>("Certificate", ref _Certificate, value); }
        }
        #endregion

        #region 'Status'
        private DOCstatus _Status;
        //[ReadOnly(true)]
        //[Appearance("Status", Enabled = false, Context = "DetailView")]
        public DOCstatus Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }
        #endregion

        #region'DateTCLPPreped'
        private DateTime _DateTCLPPreped;
        public DateTime DateTCLPPreped
        {
            get { return _DateTCLPPreped; }
            set { SetPropertyValue(nameof(DateTCLPPreped), ref _DateTCLPPreped, value); }
        }
        #endregion

        #region 'TCLPPrepedBy'
        private Employee _TCLPPrepedBy;
        public Employee TCLPPrepedBy
        {
            get { return _TCLPPrepedBy; }
            set { SetPropertyValue(nameof(TCLPPrepedBy), ref _TCLPPrepedBy, value); }
        }
        #endregion

        #region 'TCLPPrepedMethod'
        private string _TCLPPrepedMethod;
        public string TCLPPrepedMethod
        {
            get { return _TCLPPrepedMethod; }
            set { SetPropertyValue<string>("TCLPPrepedMethod", ref _TCLPPrepedMethod, value); }
        }
        #endregion

        #region 'PrepBatchID'
        private string _PrepBatchID;
        public string PrepBatchID
        {
            get { return _PrepBatchID; }
            set { SetPropertyValue<string>("PrepBatchID", ref _PrepBatchID, value); }
        }
        #endregion

        #region'DatePrepared'
        private DateTime _DatePrepared;
        public DateTime DatePrepared
        {
            get { return _DatePrepared; }
            set { SetPropertyValue(nameof(DatePrepared), ref _DatePrepared, value); }
        }
        #endregion

        #region 'PreparedBy'
        private Employee _PreparedBy;
        public Employee PreparedBy
        {
            get { return _PreparedBy; }
            set { SetPropertyValue(nameof(PreparedBy), ref _PreparedBy, value); }
        }
        #endregion

        #region 'PrepMethod'
        private string _PrepMethod;
        public string PrepMethod
        {
            get { return _PrepMethod; }
            set { SetPropertyValue<string>("PrepMethod", ref _PrepMethod, value); }
        }
        #endregion

        #region 'PrepInstrument'
        private string _PrepInstrument;
        public string PrepInstrument
        {
            get { return _PrepInstrument; }
            set { SetPropertyValue<string>("PrepInstrument", ref _PrepInstrument, value); }
        }
        #endregion

        #region 'AnalyticalInstrument'
        private string _AnalyticalInstrument;
        public string AnalyticalInstrument
        {
            get { return _AnalyticalInstrument; }
            set { SetPropertyValue("AnalyticalInstrument", ref _AnalyticalInstrument, value); }
        }
        #endregion

        #region 'StandardName'
        private string _StandardName;
        public string StandardName
        {
            get { return _StandardName; }
            set { SetPropertyValue<string>("StandardName", ref _StandardName, value); }
        }
        #endregion

        #region 'StandardID'
        private string _StandardID;
        public string StandardID
        {
            get { return _StandardID; }
            set { SetPropertyValue<string>("StandardID", ref _StandardID, value); }
        }
        #endregion

        #region 'StandardConcentration'
        private string _StandardConcentration;
        public string StandardConcentration
        {
            get { return _StandardConcentration; }
            set { SetPropertyValue<string>("StandardConcentration", ref _StandardConcentration, value); }
        }
        #endregion

        #region 'SpikeAmount'
        private string _SpikeAmount;
        public string SpikeAmount
        {
            get { return _SpikeAmount; }
            set { SetPropertyValue<string>("SpikeAmount", ref _SpikeAmount, value); }
        }
        #endregion

        #region 'SpikeUnits'
        private string _SpikeUnits;
        public string SpikeUnits
        {
            get { return _SpikeUnits; }
            set { SetPropertyValue<string>("SpikeUnits", ref _SpikeUnits, value); }
        }
        #endregion

        #region 'TrainerName'
        private string _TrainerName;
        public string TrainerName
        {
            get { return _TrainerName; }
            set { SetPropertyValue<string>("TrainerName", ref _TrainerName, value); }
        }
        #endregion

        #region'TrainingStartDate'
        private DateTime? _TrainingStartDate;
        [ImmediatePostData]
        public DateTime? TrainingStartDate
        {
            get { return _TrainingStartDate; }
            set { SetPropertyValue(nameof(TrainingStartDate), ref _TrainingStartDate, value); }
        }
        #endregion

        #region'TrainingEndDate'
        private DateTime? _TrainingEndDate;
        [ImmediatePostData]
        public DateTime? TrainingEndDate
        {
            get { return _TrainingEndDate; }
            set { SetPropertyValue(nameof(TrainingEndDate), ref _TrainingEndDate, value); }
        }
        #endregion

        #region 'Comment'
        private bool _Commentbool;
        [ImmediatePostData(true)]
        public bool Commentbool
        {
            get { return _Commentbool; }
            set { SetPropertyValue(nameof(Commentbool), ref _Commentbool, value); }
        }

        private string _Comment;

        public event EventHandler ItemsChanged;

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        #region SampleParameter
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        //[Association("DOC-SampleParameter")]
        public XPCollection<SampleParameter> SampleParameter
        {
            get { return GetCollection<SampleParameter>("SampleParameter"); }
        }
        private string _QCBatches;
        //[RuleRequiredField]
        [XafDisplayName("QC Batches")]
        //[ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField]
        public string QCBatches
        {
            get { return _QCBatches; }
            set { SetPropertyValue(nameof(QCBatches), ref _QCBatches, value); }
        }
        //#region ICheckedListBoxItemsProvider Members
        //public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        //{
        //    Dictionary<object, string> Properties = new Dictionary<object, string>();
        //    if (targetMemberName == "QCBatches" && SampleParameter != null && SampleParameter.Count > 0)
        //    {
        //        Properties = SampleParameter.OrderBy(i => i.UQABID).ToDictionary(x => (Object)x.Oid, x => x.SampleParameter);
        //        foreach (VisualMatrix objSampleMatrix in SampleParameter.Where(i => i.Testparameter.Test != null).OrderBy(i => i.VisualMatrixName).ToList())
        //        {
        //            if (!Properties.ContainsKey(objSampleMatrix.Oid))
        //            {
        //                Properties.Add(objSampleMatrix.Oid, objSampleMatrix.VisualMatrixName);
        //            }
        //        }
        //    }
        //        return Properties;
        //}
        ////public event EventHandler ItemsChanged;
        ////protected void OnItemsChanged()
        ////{
        ////    if (ItemsChanged != null)
        ////    {
        ////        ItemsChanged(this, new EventArgs());
        ////    }
        ////}
        //#endregion
        //private SampleParameter _SampleParameter;
        //[ImmediatePostData(true)]
        //public SampleParameter SampleParameter
        //{
        //    get {
        //        if (Method == null)
        //        {
        //            SampleParameter = null;
        //        } 

        //        return _SampleParameter; }
        //    set { SetPropertyValue(nameof(SampleParameter), ref _SampleParameter, value); }
        //}


        #endregion

        #region DeletedBy
        private Employee _DeletedBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Employee DeletedBy
        {
            get { return _DeletedBy; }
            set { SetPropertyValue(nameof(DeletedBy), ref _DeletedBy, value); }
        }
        #endregion

        #region DeletedDate
        private DateTime _DeletedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public DateTime DeletedDate
        {
            get { return _DeletedDate; }
            set { SetPropertyValue(nameof(DeletedDate), ref _DeletedDate, value); }
        }
        #endregion

        #region DeletedReason
        private String _DeletedReason;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public String DeletedReason
        {
            get { return _DeletedReason; }
            set { SetPropertyValue(nameof(DeletedReason), ref _DeletedReason, value); }
        }
        #endregion
        #region RollBackBy
        private Employee _RollBackBy;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public Employee RollBackBy
        {
            get { return _RollBackBy; }
            set { SetPropertyValue(nameof(RollBackBy), ref _RollBackBy, value); }
        }
        #endregion

        #region RollBackDate
        private DateTime _RollBackDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public DateTime RollBackDate
        {
            get { return _RollBackDate; }
            set { SetPropertyValue(nameof(RollBackDate), ref _RollBackDate, value); }
        }
        #endregion

        #region RollBackReason
        private String _RollBackReason;
        [Size(SizeAttribute.Unlimited)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public String RollBackReason
        {
            get { return _RollBackReason; }
            set { SetPropertyValue(nameof(RollBackReason), ref _RollBackReason, value); }
        }
        #endregion

        #region IsQCBatchID
        private bool _IsQCBatchID;
        [ImmediatePostData(true)]
        public bool IsQCBatchID
        {
            get { return _IsQCBatchID; }
            set { SetPropertyValue(nameof(IsQCBatchID), ref _IsQCBatchID, value); }
        }
        #endregion
    }
}
