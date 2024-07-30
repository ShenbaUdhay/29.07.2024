using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SamplingManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [Appearance("ShowCopyTestItem3", AppearanceItemType = "LayoutItem",
 TargetItems = "Item3", Criteria = "IsGrouptm = True",
 Context = "Testparameter_DetailView_CopyTest", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideCopyTestItem3", AppearanceItemType = "LayoutItem",
 TargetItems = "Item3", Criteria = "IsGrouptm = False",
 Context = "Testparameter_DetailView_CopyTest", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowCopyTestItem2", AppearanceItemType = "LayoutItem",
 TargetItems = "Item2", Criteria = "IsGrouptm = False",
 Context = "Testparameter_DetailView_CopyTest", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideCopyTestItem2", AppearanceItemType = "LayoutItem",
 TargetItems = "Item2", Criteria = "IsGrouptm = True",
 Context = "Testparameter_DetailView_CopyTest", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowCopyTestItem4", AppearanceItemType = "LayoutItem",
 TargetItems = "Item4", Criteria = "IsGrouptm = False",
 Context = "Testparameter_DetailView_CopyTest", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideCopyTestItem4", AppearanceItemType = "LayoutItem",
 TargetItems = "Item4", Criteria = "IsGrouptm = True",
 Context = "Testparameter_DetailView_CopyTest", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    public class Testparameter : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        //string CurrentLanguage;
        curlanguage curlanguage = new curlanguage();
        TestInfo testinfo = new TestInfo();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        public Testparameter(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            //SelectedData sproc = Session.ExecuteSproc("getCurrentLanguage", "");
            //CurrentLanguage = sproc.ResultSet[1].Rows[0].Values[0].ToString();     
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SpikeAmount == 0)
            {
                SpikeAmount = null;
            }
        }

       

        #region TAT
        private TurnAroundTime _TAT;
        [NonPersistent]
        [ImmediatePostData]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public TurnAroundTime TAT
        {
            get { return _TAT; }
            set { SetPropertyValue("TAT", ref _TAT, value); }
        }
        #endregion

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
        private TestMethod _TestMethod;
        [Association]
        public TestMethod TestMethod
        {
            get { return _TestMethod; }
            set { SetPropertyValue("TestMethod", ref _TestMethod, value); }
        }
        #endregion

        #region Test
        private TestMethod _Test;
        [NonPersistent]
        [ImmediatePostData]
        //[RuleRequiredField("Testid1", DefaultContexts.Save, CustomMessageTemplate = "'Test' must not to be empty.")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [DataSourceProperty(nameof(TestDataSource))]


        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue("Test", ref _Test, value); }
        }


        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (Matrix != null && Test == null && Matrix != null)
                {
                    if (testinfo.isgroup == true)
                    {
                        List<Guid> groups = new List<Guid>();
                        using (XPView lstview = new XPView(Session, typeof(TestMethod)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = 'True'", Matrix.MatrixName);
                            lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                            foreach (ViewRecord rec in lstview)
                                groups.Add(new Guid(rec["Toid"].ToString()));
                        }
                        if (groups.Count == 0)
                        {
                            XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=?  And [IsGroup] = 'True'", Matrix.MatrixName));
                            return tests;
                        }
                        else
                        {
                            if (groups.Contains(testinfo.CurrentTest.Oid))
                            {
                                groups.Remove(testinfo.CurrentTest.Oid);
                            }
                            XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
                            return tests;
                        }
                    }
                    else
                    {
                        List<object> groups = new List<object>();
                        using (XPView lstview = new XPView(Session, typeof(TestMethod)))
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName]=?", Matrix.MatrixName);
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
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region IsGroup
        private bool _IsGrouptm;
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public bool IsGrouptm
        {
            get
            {
                if (testinfo.isgroup)
                {
                    _IsGrouptm = true;
                }
                else
                {
                    _IsGrouptm = false;
                }
                return _IsGrouptm;
            }
            set { SetPropertyValue("IsGrouptm", ref _IsGrouptm, value); }
        }
        #endregion

        #region Method
        private TestMethod _Method;
        [NonPersistent]
        //[RuleRequiredField("Methodid1", DefaultContexts.Save, CustomMessageTemplate = "'Method' must not to be empty.")]
        [ImmediatePostData]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [DataSourceProperty(nameof(MethodDataSource))]

        public TestMethod Method
        {
            get { return _Method; }
            set { SetPropertyValue("Method", ref _Method, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> MethodDataSource
        {
            get
            {
                if (Test != null && Method == null && Matrix != null)
                {
                    return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=?", Test.TestName, Matrix.MatrixName));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Matrix
        private Matrix _Matrix;
        [NonPersistent]
        //[RuleRequiredField("Matrixid1", DefaultContexts.Save, CustomMessageTemplate = "'Matrix' must not to be empty.")]
        [ImmediatePostData]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [DataSourceProperty(nameof(MatrixDataSource))]
        public Matrix Matrix
        {
            get { return _Matrix; }
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


        #region IListforSampleLogin
        //[ManyToManyAlias("SampleParameter", "SampleLogIn")]
        [ManyToManyAlias("SampleParameter", "Samplelogin")]
        public IList<SampleLogIn> SampleLogIns
        {
            get
            {
                return GetList<SampleLogIn>("SampleLogIns");
            }
        }
        #endregion

        #region IListforSampleParameter
        [Association, Browsable(false)]
        public IList<SampleParameter> SampleParameter
        {
            get
            {
                return GetList<SampleParameter>("SampleParameter");
            }
        }
        #endregion
        //#region IListforSampling
        //[ManyToManyAlias("SamplingParameter", "Sampling")]
        //public IList<Sampling> Sampling
        //{
        //    get
        //    {
        //        return GetList<Sampling>("Sampling");
        //    }
        //}
        //#endregion

        //#region IListforSamplingTest
        //[Association, Browsable(false)]
        //public IList<SamplingParameter> SamplingParameter
        //{
        //    get
        //    {
        //        return GetList<SamplingParameter>("SamplingParameter");
        //    }
        //}
        //#endregion IListforSamplingTest

        #region IListforCOCSettingsSamples
        //[ManyToManyAlias("SampleParameter", "SampleLogIn")]
        [ManyToManyAlias("COCSettingsTests", "COCSettingsSamples")]
        public IList<COCSettingsSamples> COCSettingsSample
        {
            get
            {
                return GetList<COCSettingsSamples>("COCSettingsSample");
            }
        }
        #endregion 

        #region IListforCOCSample
        [Association, Browsable(false)]
        public IList<COCSettingsTest> COCSettingsTests
        {
            get
            {
                return GetList<COCSettingsTest>("COCSettingsTests");
            }
        }
        //[Association("COCSettingsSamples", UseAssociationNameAsIntermediateTableName = true), Browsable(false)]
        //public XPCollection<COCSettingsTest> COCSettingsTest
        //{
        //    get { return GetCollection<COCSettingsTest>("COCSettingsTest"); }
        //}
        //public IList<COCSettingsTest> COCSettingsTest
        //{
        //    get
        //    {
        //        return GetList<COCSettingsTest>("COCSettingsTest");
        //    }
        //}
        #endregion IListforCOCSample

        //#region AuditTrail
        //private XPCollection<AuditDataItemPersistent> auditTrail;
        //public XPCollection<AuditDataItemPersistent> AuditTrail
        //{
        //    get
        //    {
        //        if (auditTrail == null)
        //        {
        //            auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this);
        //        }
        //        return auditTrail;
        //    }
        //}

        //#endregion

        [NonPersistent]
        [Association("SampleCheckinTest", UseAssociationNameAsIntermediateTableName = true)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]

        public XPCollection<Samplecheckin> Samplecheckins
        {
            get { return GetCollection<Samplecheckin>("Samplecheckins"); }
        }
        //#region SamplingProposal
        //[NonPersistent]
        //[Association("SamplingProposalTest", UseAssociationNameAsIntermediateTableName = true)]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]

        //public XPCollection<SamplingProposal> SamplingProposals
        //{
        //    get { return GetCollection<SamplingProposal>("SamplingProposals"); }
        //}
        //#endregion
        [Association("TestParameter-GroupTestMethod")]
        public XPCollection<GroupTestMethod> GroupTestMethod
        {
            get { return GetCollection<GroupTestMethod>(nameof(GroupTestMethod)); }
        }

        private QCType _QCtype;
        [Association]
        public QCType QCType
        {
            get { return _QCtype; }
            set { SetPropertyValue("QCType", ref _QCtype, value); }
        }

        private int _Sort;

        public int Sort
        {
            get { return _Sort; }
            set { SetPropertyValue("Sort", ref _Sort, value); }
        }

        private string _DefaultResult;
        public string DefaultResult
        {
            get { return _DefaultResult; }
            set { SetPropertyValue("DefaultResult", ref _DefaultResult, value); }
        }
        //private Accrediation _Accrediation;
        //public Accrediation Accrediation
        //{
        //    get { return _Accrediation; }
        //    set { SetPropertyValue("Accrediation", ref _Accrediation, value); }
        //}
        private string _lAccrediation;
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string lAccrediation
        {
            get { return _lAccrediation; }
            set { SetPropertyValue("lAccrediation", ref _lAccrediation, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Accrediation> ACCDatasource
        {
            get
            {
                return new XPCollection<Accrediation>(Session, CriteriaOperator.Parse(""));
            }
        }
        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "lAccrediation" && ACCDatasource != null && ACCDatasource.Count > 0)
            {
                foreach (Accrediation accrediation in ACCDatasource.Where(i => i.lAccrediation != null).OrderBy(i => i.lAccrediation).ToList())
                {
                    if (!Properties.ContainsKey(accrediation.lAccrediation) && !string.IsNullOrEmpty(accrediation.lAccrediation))
                    {
                        Properties.Add(accrediation.lAccrediation, accrediation.lAccrediation);
                    }
                }
            }
            return Properties;
        }
        private string _ParameterDefaultResults;
        [VisibleInDetailView(false), VisibleInDashboards(false), VisibleInLookupListView(false), VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string ParameterDefaultResults
        {
            get { return _ParameterDefaultResults; }
            set { SetPropertyValue("ParameterDefaultResults", ref _ParameterDefaultResults, value); }
        }
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
                if (ParameterDefaultResults != null && !string.IsNullOrEmpty(ParameterDefaultResults))
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
        private Unit _DefaultUnits;

        public Unit DefaultUnits
        {
            get { return _DefaultUnits; }
            set { SetPropertyValue("DefaultUnits", ref _DefaultUnits, value); }
        }

        private string _FinalDefaultResult;
        public string FinalDefaultResult
        {
            get { return _FinalDefaultResult; }
            set { SetPropertyValue("FinalDefaultResult", ref _FinalDefaultResult, value); }
        }

        private Unit _FinalDefaultUnits;
        public Unit FinalDefaultUnits
        {
            get { return _FinalDefaultUnits; }
            set { SetPropertyValue("FinalDefaultUnits", ref _FinalDefaultUnits, value); }
        }



        private Unit _SurrogateUnits;
        public Unit SurrogateUnits
        {
            get
            {
                return _SurrogateUnits;
            }
            set { SetPropertyValue("SurrogateUnits", ref _SurrogateUnits, value); }
        }

        private double _SurrogateAmount;
        public double SurrogateAmount
        {
            get { return _SurrogateAmount; }
            set { SetPropertyValue("SurrogateAmount", ref _SurrogateAmount, value); }
        }

        private string _SurrogateLowLimit;
        public string SurrogateLowLimit
        {
            get { return _SurrogateLowLimit; }
            set { SetPropertyValue("SurrogateLowLimit", ref _SurrogateLowLimit, value); }
        }

        private string _SurrogateHighLimit;
        public string SurrogateHighLimit
        {
            get { return _SurrogateHighLimit; }
            set { SetPropertyValue("SurrogateHighLimit", ref _SurrogateHighLimit, value); }
        }

        private string _LOQ;
        public string LOQ
        {
            get { return _LOQ; }
            set { SetPropertyValue("LOQ", ref _LOQ, value); }
        }

        private string _UQL;
        public string UQL
        {
            get { return _UQL; }
            set { SetPropertyValue("UQL", ref _UQL, value); }
        }

        private string _RptLimit;
        public string RptLimit
        {
            get { return _RptLimit; }
            set { SetPropertyValue("UQL", ref _RptLimit, value); }
        }

        private string _RegulatoryLimit;
        public string RegulatoryLimit
        {
            get { return _RegulatoryLimit; }
            set { SetPropertyValue("RegulatoryLimit", ref _RegulatoryLimit, value); }
        }

        private string _MDL;
        public string MDL
        {
            get { return _MDL; }
            set { SetPropertyValue("MDL", ref _MDL, value); }
        }

        private string _MCL;
        public string MCL
        {
            get { return _MCL; }
            set { SetPropertyValue("MCL", ref _MCL, value); }
        }

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set
            {
                SetPropertyValue("Comment", ref _Comment, value);
            }
        }

        private DateTime _RetireDate;

        public DateTime RetireDate
        {
            get { return _RetireDate; }
            set { SetPropertyValue("RetireDate", ref _RetireDate, value); }
        }

        private string _SigFig;
        public string SigFig
        {
            get { return _SigFig; }
            set { SetPropertyValue("SigFig", ref _SigFig, value); }
        }

        private string _CutOff;
        public string CutOff
        {
            get { return _CutOff; }
            set { SetPropertyValue("CutOff", ref _CutOff, value); }
        }

        private string _Decimal;
        public string Decimal
        {
            get { return _Decimal; }
            set { SetPropertyValue("Decimal", ref _Decimal, value); }
        }

        private bool _Surrogate;

        public bool Surroagate
        {
            get { return _Surrogate; }
            set { SetPropertyValue("Surroagate", ref _Surrogate, value); }
        }

        private bool _InternalStandard;

        public bool InternalStandard
        {
            get { return _InternalStandard; }
            set { SetPropertyValue("InternalStandar", ref _InternalStandard, value); }
        }

        private string _TestGroup;
        [NonPersistent]
        public string TestGroup
        {
            get { return _TestGroup; }
            set { SetPropertyValue("TestGroup", ref _TestGroup, value); }
        }

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
        #region Hold

        private bool _Hold;
        [NonPersistent]
        public bool Hold
        {
            get
            {
                return _Hold;
            }
            set
            {
                SetPropertyValue("Hold", ref _Hold, value);
            }
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
        private Nullable<double> _SpikeAmount;
        public Nullable<double> SpikeAmount
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
        #region CustomLimit
        private string _CustomLimit;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string CustomLimit
        {
            get { return _CustomLimit; }
            set { SetPropertyValue("CustomLimit ", ref _CustomLimit, value); }
        }
        #endregion

        #region Subout
        private bool _SubOut;
        [NonPersistent]
        public bool SubOut
        {
            get { return _SubOut; }
            set { SetPropertyValue("SubOut", ref _SubOut, value); }
        }
        #endregion

        [NonPersistent]
        public string Parent
        {
            get
            {
                if (TestGroup != null && TestGroup.Length > 0)
                {
                    if (curlanguage.strcurlanguage == "En")
                    {
                        return string.Format("TestGroup:{0}", TestGroup);
                    }
                    else
                    {
                        return string.Format("监测组合:{0}", TestGroup);
                    }
                }
                else
                {
                    if (curlanguage.strcurlanguage == "En")
                    {
                        return "TestGroup:None";
                    }
                    else
                    {
                        return "监测组合:None";
                    }

                }

            }

        }
        [NonPersistent]
        public string Child
        {
            get
            {
                if (TestMethod != null && Component != null)
                {
                    if (curlanguage.strcurlanguage == "En")
                    {
                        return string.Format("Matrix:{0} Test:{1} Method:{2} Component:{3} Parameter:", TestMethod.MatrixName.MatrixName, TestMethod.TestName, TestMethod.MethodName.MethodNumber, Component.Components);
                        //return string.Format("Matrix:{0} Test:{1} Method:{2} ", TestMethod.MatrixName.MatrixName, TestMethod.TestName, TestMethod.MethodName.MethodNumber);
                    }
                    else
                    {
                        return string.Format("基质:{0} 检测项目:{1} 方法依据:{2} 零件:{3} 检测参数:", TestMethod.MatrixName.MatrixName, TestMethod.TestName, TestMethod.MethodName.MethodNumber, Component.Components);
                        //return string.Format("基质:{0} 检测项目:{1} 方法依据:{2} ", TestMethod.MatrixName.MatrixName, TestMethod.TestName, TestMethod.MethodName.MethodNumber);
                    }
                }

                else
                {
                    return null;
                }
            }

            //set { SetPropertyValue("MatrixTestMethod", ref _TestGroup, value); }
        }

        [NonPersistent]
        public string QCChild
        {
            get
            {
                if (TestMethod != null)
                {

                    if (curlanguage.strcurlanguage == "En")
                    {
                        if (QCType != null)
                        {
                            return string.Format("Test:{0} Method:{1} QCType:{2} Parameter:", TestMethod.TestName, TestMethod.MethodName.MethodName, QCType.QCTypeName);
                        }
                        else if (TestMethod != null && TestMethod.MethodName != null)
                        {
                            return string.Format("Test:{0} Method:{1} Parameter:", TestMethod.TestName, TestMethod.MethodName.MethodName);
                        }
                        else
                        {
                            return string.Format("Test:{0} Parameter:", TestMethod.TestName);
                        }
                    }
                    else
                    {
                        if (QCType != null)
                        {
                            return string.Format("检测项目:{0} 方法依据:{1} 质控类型:{2} 检测参数:", TestMethod.TestName, TestMethod.MethodName.MethodName, QCType.QCTypeName);
                        }
                        else if (TestMethod != null && TestMethod.MethodName != null)
                        {
                            return string.Format("检测项目:{0} 方法依据:{1} 检测参数:", TestMethod.TestName, TestMethod.MethodName.MethodName);
                        }
                        else
                        {
                            return string.Format("检测项目:{0} 检测参数:", TestMethod.TestName);
                        }
                    }
                }

                else
                {
                    return null;
                }
            }

            //set { SetPropertyValue("MatrixTestMethod", ref _TestGroup, value); }
        }

        #region IsTestMethodExists
        [NonPersistent]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsTestMethodExists
        {
            get
            {
                if (TestMethod != null)
                {
                    SelectedData sprocCheckTest = this.Session.ExecuteSproc("CheckTesMethod", TestMethod.Oid);
                    return Convert.ToBoolean(sprocCheckTest.ResultSet[1].Rows[0].Values[0]);
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
        //#region IListforMonitoringSample
        //[ManyToManyAlias("SamplingTest", "Sampling")]
        //public IList<Sampling> Sampling
        //{
        //    get
        //    {
        //        return GetList<Sampling>("Sampling");
        //    }
        //}
        //#endregion 
        //#region IListforMonitoringTest
        //[Association, Browsable(false)]
        //public IList<SamplingTest> SamplingTest
        //{
        //    get
        //    {
        //        return GetList<SamplingTest>("SamplingTest");
        //    }
        //}
        //#endregion
        //#region IListforProposalSettingSample
        //[ManyToManyAlias("SamplingProposalSettingSampleTest", "SamplingProposalSettingSamples")]
        //public IList<SamplingProposalSettingSamples> SamplingProposalSettingSamples
        //{
        //    get
        //    {
        //        return GetList<SamplingProposalSettingSamples>("SamplingProposalSettingSamples");
        //    }
        //}
        //#endregion 
        //#region IListforProposalSettingSampleTest
        //[Association, Browsable(false)]
        //public IList<SamplingProposalSettingSampleTest> SamplingProposalSettingSampleTest
        //{
        //    get
        //    {
        //        return GetList<SamplingProposalSettingSampleTest>("SamplingProposalSettingSampleTest");
        //    }
        //}
        //#endregion

        #region Component
        private Component _Component;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Component Component
        {
            get
            {
                return _Component;
            }
            set { SetPropertyValue("Component", ref _Component, value); }
        }
        #endregion

        #region IsGroup
        private bool _IsGroup;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool IsGroup
        {
            get
            {
                return _IsGroup;
            }
            set { SetPropertyValue("IsGroup", ref _IsGroup, value); }
        }
        #endregion

        #region GroupTestParameter
        private string _GroupTestParameter;
        [XafDisplayName("Parameter")]
        [NonPersistent]
        [ImmediatePostData]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string GroupTestParameter
        {
            get
            {
                if (!string.IsNullOrEmpty(GroupTestParameters))
                {
                    _GroupTestParameter = "Customized";
                }
                else
                {
                    _GroupTestParameter = "Default";
                }
                return _GroupTestParameter;
            }
            set { SetPropertyValue("GroupTestParameter", ref _GroupTestParameter, value); }
        }
        #endregion

        #region GroupTestParameters
        private string _GroupTestParameters;
        [NonPersistent]
        [ImmediatePostData]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string GroupTestParameters
        {
            get
            {
                return _GroupTestParameters;
            }
            set { SetPropertyValue("GroupTestParameters", ref _GroupTestParameters, value); }
        }
        #endregion

        #region RSD
        private string _RSD;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string RSD
        {
            get { return _RSD; }
            set { SetPropertyValue("RSD", ref _RSD, value); }
        }
        #endregion
        #region IsSubutAttached
        private bool _IsSubutAttached;

        

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false),VisibleInDashboards(false)]
        [NonPersistent]
        public bool IsSubutAttached
        {
            get
            {
                if (SRInfo.IsAttchedSubout)
                {
                    SampleParameter objsmpltest = Session.FindObject<SampleParameter>(CriteriaOperator.Parse("[Testparameter.Oid] = ? And [Samplelogin.Oid]= ?", Oid, SRInfo.SampleOid));
                    if (objsmpltest != null && objsmpltest.SuboutSample != null)
                    {
                        _IsSubutAttached = true;
                    }
                    else
                    {
                        _IsSubutAttached = false;
                    } 
                }
                return _IsSubutAttached;
            }

        }
        #endregion

        #region DWQRTemplateSetupParameter
        [Association("DWQRReportTemplateSetup-Testparameter")]
        public XPCollection<Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup> DWQRTemplateSetupParameters
        {
            get { return GetCollection<Modules.BusinessObjects.Setting.DWQRReportTemplateSetup.DWQRReportTemplateSetup>(nameof(DWQRTemplateSetupParameters)); }
        }
        #endregion
    }
}