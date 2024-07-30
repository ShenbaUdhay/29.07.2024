using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.QC
{
    [DefaultClassOptions]
    public class QCBatch : BaseObject, ICheckedListBoxItemsProvider
    {
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        InfoClass.NavigationRefresh objnavigationRefresh = new InfoClass.NavigationRefresh();
        public QCBatch(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string _QCBatchID;
        [Appearance("QCBatchHide", Visibility = ViewItemVisibility.Hide, Criteria = "[ShowQCBatchID] = False", Context = "DetailView")]
        [Appearance("QCBatchShow", Visibility = ViewItemVisibility.Show, Criteria = "[ShowQCBatchID] = True", Context = "DetailView")]
        public string QCBatchID
        {
            get { return _QCBatchID; }
            set { SetPropertyValue("QCBatchID", ref _QCBatchID, value); }
        }

        private Matrix _Matrix;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [DataSourceProperty("MatrixDataSource")]
        [ImmediatePostData]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set
            {
                SetPropertyValue("Matrix", ref _Matrix, value);
                Method = null;
                Test = null;
            }
        }

        private string _Roomtemp;
        public string Roomtemp
        {
            get { return _Roomtemp; }
            set { SetPropertyValue("Roomtemp", ref _Roomtemp, value); }
        }

        private string _NPJobid;
        [NonPersistent]
        [Appearance("NPJobid", Visibility = ViewItemVisibility.Hide, Criteria = "!ISShown", Context = "DetailView")]
        [RuleRequiredField("NPJobid", DefaultContexts.Save)]
        public string NPJobid
        {
            get { return _NPJobid; }
            set { SetPropertyValue("NPJobid", ref _NPJobid, value); }
        }

        private string _Jobid;
        [Appearance("Jobid", Visibility = ViewItemVisibility.Hide, Criteria = "ISShown", Context = "DetailView")]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public string Jobid
        {
            get { return _Jobid; }
            set { SetPropertyValue("Jobid", ref _Jobid, value); }
        }

        private bool _ISShown;
        [NonPersistent]
        [ImmediatePostData]
        public bool ISShown
        {
            get { return _ISShown; }
            set { SetPropertyValue("ISShown", ref _ISShown, value); }
        }

        private DateTime _Datecreated;
        public DateTime Datecreated
        {
            get { return _Datecreated; }
            set { SetPropertyValue("Datecreated", ref _Datecreated, value); }
        }

        //private DateTime _ReceivedDate;
        //public DateTime ReceivedDate
        //{
        //    get { return _ReceivedDate; }
        //    set { SetPropertyValue("ReceivedDate", ref _ReceivedDate, value); }
        //}

        private TestMethod _Test;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [DataSourceProperty("TestDataSource")]
        [ImmediatePostData]
        public TestMethod Test
        {
            get { return _Test; }
            set
            {
                SetPropertyValue("Test", ref _Test, value);
                if (Test != null && qcbatchinfo.IsResetActionEnable == false)
                {
                    Method = Test;
                }
                else
                {
                    Method = null;
                }
            }
        }

        private string _Humidity;
        public string Humidity
        {
            get { return _Humidity; }
            set { SetPropertyValue("Humidity", ref _Humidity, value); }
        }

        private int _Noruns;
        public int Noruns
        {
            get { return _Noruns; }
            set { SetPropertyValue("Noruns", ref _Noruns, value); }
        }

        private Employee _CreatedBy;
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }

        private TestMethod _Method;
        [DataSourceProperty("MethodDataSource")]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [ImmediatePostData]
        public TestMethod Method
        {
            get { return _Method; }
            set { SetPropertyValue("Method", ref _Method, value); }
        }

        private string _Instrument;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Instrument
        {
            get { return _Instrument; }
            set { SetPropertyValue("Instrument", ref _Instrument, value); }
        }

        private SpreadSheetEntry_AnalyticalBatch _ABID;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public SpreadSheetEntry_AnalyticalBatch ABID
        {
            get { return _ABID; }
            set { SetPropertyValue("ABID", ref _ABID, value); }
        }


        //#region QClink
        //[Association("QClink")]
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //public XPCollection<QCBatchSequence> qcdetail
        //{
        //    get { return GetCollection<QCBatchSequence>("qcdetail"); }
        //}
        //#endregion

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (Matrix != null && Test == null)
                {
                    if (objnavigationRefresh.ClickedNavigationItem == "Asbestos_PLM")
                    {
                        XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName]=? and [MethodName.GCRecord] is null And [IsPLMTest] = True", Matrix));
                        foreach (TestMethod test in tests.ToList())
                        {
                            XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [QCBatchID] Is Null", test.Oid));
                            //SpreadSheetBuilder_TestParameter templatetest = Session.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID] =?", test.Oid));
                            //XPCollection<SpreadSheetBuilder_TestParameter> templatetest = new XPCollection<SpreadSheetBuilder_TestParameter>(Session, CriteriaOperator.Parse("[TestMethodID] =?", test.Oid));
                            //if (templatetest != null || (templatetest == null && samples.Count == 0))
                            if (samples.Count == 0)
                            {
                                tests.Remove(test);
                            }
                        }
                        XPView testsView = new XPView(Session, typeof(TestMethod));
                        testsView.Criteria = new InOperator("Oid", tests.Select(i => i.Oid));
                        testsView.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "TestName", true, true));
                        testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in testsView)
                            groups.Add(rec["Toid"]);
                        return new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
                    }
                    else
                    {
                        XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName]=? and [MethodName.GCRecord] is null", Matrix));
                        foreach (TestMethod test in tests.ToList())
                        {
                            XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [QCBatchID] Is Null And [SignOff] = True And [IsTransferred] = true", test.Oid));
                            //SpreadSheetBuilder_TestParameter templatetest = Session.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID] =?", test.Oid));
                            //XPCollection<SpreadSheetBuilder_TestParameter> templatetest = new XPCollection<SpreadSheetBuilder_TestParameter>(Session, CriteriaOperator.Parse("[TestMethodID] =?", test.Oid));
                            //if (templatetest != null || (templatetest == null && samples.Count == 0))
                            if (samples.Count == 0)
                            {
                                tests.Remove(test);
                            }
                            ////else if (test.PrepMethods.Count > 0 && samples.FirstOrDefault(i => i.SamplePrepBatchID != null) == null)
                            ////{
                            ////    tests.Remove(test);
                            ////}
                        }
                        XPView testsView = new XPView(Session, typeof(TestMethod));
                        testsView.Criteria = new InOperator("Oid", tests.Select(i => i.Oid));
                        testsView.Properties.Add(new ViewProperty("TJobID", SortDirection.Ascending, "TestName", true, true));
                        testsView.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                        List<object> groups = new List<object>();
                        foreach (ViewRecord rec in testsView)
                            groups.Add(rec["Toid"]);
                        return new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
                    }

                }
                else
                {
                    return null;
                }
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> MethodDataSource
        {
            get
            {
                if (Matrix != null && Test != null && Method == null)
                {
                    //XPCollection<SpreadSheetBuilder_TestParameter> sdmstest = new XPCollection<SpreadSheetBuilder_TestParameter>(Session, CriteriaOperator.Parse("[TestMethodID] =?", Oid));
                    //if (sdmstest == null || sdmstest.Count == 0)
                    //{
                    //    return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=?", Test.TestName, Matrix.MatrixName));
                    //}
                    //else
                    //{
                    //    List<Guid> lstTests = sdmstest.Select(i => i.TestMethodID).Distinct().ToList();
                    //    return new XPCollection<TestMethod>(Session, new GroupOperator(GroupOperatorType.And ,CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=?", Test.TestName, Matrix.MatrixName), new NotOperator(new InOperator("Oid", lstTests))));
                    //}
                    XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName] =?", Test.TestName, Matrix.MatrixName));
                    foreach (TestMethod test in tests.ToList())
                    {
                        XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [QCBatchID] Is Null And [SignOff] = True And [IsTransferred] = true", test.Oid));
                        //SpreadSheetBuilder_TestParameter templatetest = Session.FindObject<SpreadSheetBuilder_TestParameter>(CriteriaOperator.Parse("[TestMethodID] =?", test.Oid));
                        //XPCollection<SpreadSheetBuilder_TestParameter> templatetest = new XPCollection<SpreadSheetBuilder_TestParameter>(Session, CriteriaOperator.Parse("[TestMethodID] =?", test.Oid));
                        //if (templatetest != null || (templatetest == null && samples.Count == 0))
                        if (samples.Count == 0)
                        {
                            tests.Remove(test);
                        }
                        ////else if (test.PrepMethods.Count > 0 && samples.FirstOrDefault(i => i.SamplePrepBatchID != null) == null)
                        ////{
                        ////    tests.Remove(test);
                        ////}
                    }
                    return tests;
                }
                else
                {
                    return null;
                }
            }
        }

        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<SampleParameter> JobidDataSource
        //{
        //    get
        //    {
        //        if (Test != null && Method != null && (QCBatchID == "Auto Generate" || QCBatchID == "自动生成")) //&& Jobid == null
        //        {
        //            return new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ? And [QCBatchID] Is Null", Test.Oid));
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Matrix> MatrixDataSource
        {
            get
            {
                if (objnavigationRefresh.ClickedNavigationItem == "Asbestos_PLM")
                {
                    XPCollection<TestMethod> lstMatrix = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[IsPLMTest] = True"));
                    IList<Guid> lstM = lstMatrix.Where(i => i.MatrixName != null).Select(i => i.MatrixName.Oid).Distinct().ToList();
                    /*XPCollection<Matrix> lstTests*/
                    return new XPCollection<Matrix>(Session, new InOperator("Oid", lstM));
                }
                else
                {
                    return new XPCollection<Matrix>(Session, CriteriaOperator.Parse(""));
                }
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Labware> InstrumentDataSource
        {
            get
            {
                return new XPCollection<Labware>(Session, CriteriaOperator.Parse(""));
            }
        }

        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> properties = new Dictionary<object, string>();
            //if (targetMemberName == "Jobid" && JobidDataSource != null && JobidDataSource.Count > 0)
            //{
            //    foreach (SampleParameter objsample in JobidDataSource.Where(a => a.Samplelogin != null && a.Samplelogin.JobID != null).OrderByDescending(a => a.Samplelogin.JobID.JobID).ToList())
            //    {
            //        if (!properties.ContainsKey(objsample.Samplelogin.JobID.Oid))
            //        {
            //            properties.Add(objsample.Samplelogin.JobID.Oid, objsample.Samplelogin.JobID.JobID);
            //        }
            //    }
            //}
            //else 
            //if (targetMemberName == "Jobid" && JobidDataSource == null && Jobid != null)
            //{
            //    string[] ids = Jobid.Split(';');
            //    foreach (string id in ids)
            //    {
            //        Samplecheckin sample = Session.FindObject<Samplecheckin>(CriteriaOperator.Parse("[Oid]=?", new Guid(id.Replace(" ", ""))));
            //        if (sample != null)
            //        {
            //            properties.Add(sample.Oid, sample.JobID);
            //        }
            //    }
            //}
            if (targetMemberName == "Instrument" && Test != null && Test.Labwares.Count > 0)
            {
                foreach (Labware objlab in Test.Labwares.OrderBy(a => a.AssignedName).ToList())
                {
                    if (!properties.ContainsKey(objlab.Oid) && !string.IsNullOrEmpty(objlab.AssignedName))
                    {
                        properties.Add(objlab.Oid, objlab.AssignedName);
                    }
                }
            }
            return properties;
        }

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        #endregion

        private int _PendingEntrySamplesCount;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [XafDisplayName("#Sample")]
        [ImmediatePostData]
        public int PendingEntrySamplesCount
        {
            get
            {
                if (!string.IsNullOrEmpty(QCBatchID))
                {
                    _PendingEntrySamplesCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"),
                     CriteriaOperator.Parse("[Status] = 'PendingEntry' And [SignOff] = True And [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample' And [QCBatchID.qcseqdetail.QCBatchID] = ?", QCBatchID)));

                    //XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Status] = 'PendingEntry' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample'", QCBatchID));
                    //_PendingEntrySamplesCount = samples.Count;
                }
                return _PendingEntrySamplesCount;
            }
        }

        private int _PendingEntryQCCount;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [XafDisplayName("#QC")]
        [ImmediatePostData]
        public int PendingEntryQCCount
        {
            get
            {
                if (!string.IsNullOrEmpty(QCBatchID))
                {
                    ////_PendingEntryQCCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Status] = 'PendingEntry' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID)));
                    _PendingEntryQCCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Status] = 'PendingEntry' And [SignOff] = True And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID)));

                    //XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Status] = 'PendingEntry' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID));
                    //_PendingEntryQCCount = samples.Count;
                }
                return _PendingEntryQCCount;
            }
        }

        private int _PendingValidationSamplesCount;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [XafDisplayName("#Sample")]
        [ImmediatePostData]
        public int PendingValidationSamplesCount
        {
            get
            {
                if (!string.IsNullOrEmpty(QCBatchID))
                {
                    _PendingValidationSamplesCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Status] = 'PendingValidation' And [SignOff] = True And [QCBatchID] Is Not Null And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample'  And [QCBatchID.qcseqdetail.QCBatchID] = ?", QCBatchID)));

                    //XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Status] = 'PendingValidation' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample'", QCBatchID));
                    //_PendingValidationSamplesCount = samples.Count;
                }
                return _PendingValidationSamplesCount;
            }
        }

        private int _PendingValidationQCCount;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [XafDisplayName("#QC")]
        [ImmediatePostData]
        public int PendingValidationQCCount
        {
            get
            {
                if (!string.IsNullOrEmpty(QCBatchID))
                {
                    _PendingValidationQCCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Status] = 'PendingValidation' And [SignOff] = True And [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID)));

                    //XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Status] = 'PendingValidation' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID));
                    //_PendingValidationQCCount = samples.Count;
                }
                return _PendingValidationQCCount;
            }
        }

        private int _PendingApprovalSamplesCount;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [XafDisplayName("#Sample")]
        [ImmediatePostData]
        public int PendingApprovalSamplesCount
        {
            get
            {
                if (!string.IsNullOrEmpty(QCBatchID))
                {
                    _PendingApprovalSamplesCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Status] = 'PendingApproval' And [SignOff] = True And [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample'", QCBatchID)));

                    //XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Status] = 'PendingApproval' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] = 'Sample'", QCBatchID));
                    //_PendingApprovalSamplesCount = samples.Count;
                }
                return _PendingApprovalSamplesCount;
            }
        }

        private int _PendingApprovalQCCount;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [NonPersistent]
        [XafDisplayName("#QC")]
        [ImmediatePostData]
        public int PendingApprovalQCCount
        {
            get
            {
                if (!string.IsNullOrEmpty(QCBatchID))
                {
                    _PendingApprovalQCCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Status] = 'PendingApproval' And [SignOff] = True And [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID)));

                    //XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Status] = 'PendingApproval' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID));
                    //_PendingApprovalQCCount = samples.Count;
                }
                return _PendingApprovalQCCount;
            }
        }


        private int _PendingViewQCCount;
        [VisibleInDashboards(false)]
        [VisibleInLookupListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [NonPersistent]
        [XafDisplayName("#ViewQC")]
        [ImmediatePostData]
        public int PendingViewQCCount
        {
            get
            {
                if (!string.IsNullOrEmpty(QCBatchID))
                {
                    ////_PendingEntryQCCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Status] = 'PendingEntry' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID)));
                    _PendingViewQCCount = Convert.ToInt32(Session.Evaluate(typeof(SampleParameter), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("[Status] <> 'PendingEntry' And [SignOff] = True And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType.QCTypeName] <> 'Sample' AND ([ABID] is Null or ([ABID] is not null and [Status] <>'PendingEntry' and [Status] <>'PendingReview' and [Status] <>'PendingVerify' ))", QCBatchID)));

                    //XPCollection<SampleParameter> samples = new XPCollection<SampleParameter>(Session, CriteriaOperator.Parse("[Status] = 'PendingEntry' And  [QCBatchID.qcseqdetail.QCBatchID] = ? And [QCBatchID.QCType] Is Not Null And [QCBatchID.QCType.QCTypeName] <> 'Sample'", QCBatchID));
                    //_PendingEntryQCCount = samples.Count;
                }
                return _PendingViewQCCount;
            }
        }

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public bool ShowQCBatchID
        {
            get
            {
                if (objnavigationRefresh.ClickedNavigationItem == "Analyticalbatch" || objnavigationRefresh.ClickedNavigationItem == "AnalysisQueue" || objnavigationRefresh.ClickedNavigationItem == "AnalysisQueue ")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public bool ShowABID
        {
            get
            {
                if (objnavigationRefresh.ClickedNavigationItem == "Analyticalbatch")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        [Appearance("ABIDHide", Visibility = ViewItemVisibility.Hide, Criteria = "[ShowABID] = False", Context = "DetailView")]
        [Appearance("ABIDShow", Visibility = ViewItemVisibility.Show, Criteria = "[ShowABID] = True", Context = "DetailView")]
        [VisibleInListView(false), VisibleInLookupListView(false)]
        public string strABID
        {
            get
            {
                if (ABID != null)
                {
                    return ABID.AnalyticalBatchID;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private string _Comments;
        [Size(SizeAttribute.Unlimited)]
        public string Comments
        {
            get
            {
                return _Comments;
            }
            set
            {
                SetPropertyValue<string>(nameof(Comments), ref _Comments, value);
            }
        }
        #region Reagents
        [Association("QcBatchReagents", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Reagent> Reagents
        {
            get
            {
                return GetCollection<Reagent>(nameof(Reagents));
            }
        }
        #endregion

        ////#region Instruments
        ////[Association("QcBatchInstruments", UseAssociationNameAsIntermediateTableName = true)]
        ////public XPCollection<Labware> Instruments
        ////{
        ////    get
        ////    {
        ////        return GetCollection<Labware>(nameof(Instruments));
        ////    }
        ////}
        ////#endregion


    }
}