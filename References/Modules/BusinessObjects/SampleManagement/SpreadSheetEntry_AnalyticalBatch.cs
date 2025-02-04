﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
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
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.QC;
using Modules.BusinessObjects.SampleManagement.SamplePreparation;
using Modules.BusinessObjects.SDMS;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Method = Modules.BusinessObjects.Setting.Method;

namespace Modules.BusinessObjects.SampleManagement
{
    public enum AnalyticalBatchStatus
    {
        [XafDisplayName("Pending Result Entry")]
        PendingResultEntry,
        [XafDisplayName("Pending Completion")]
        PendingCompletion,
        [XafDisplayName("Pending Validation")]
        PendingValidation,
        Validated
    }
    public enum DataPackageStatus
    {
        [XafDisplayName("N/A")]
        NA,
        PendingSubmission,
        Submitted,
        PendingReview,
        Reviewed
    }
    [Appearance("RawDataBatchDetailsText", AppearanceItemType = "Action", TargetItems = "DataReviewBatchDetailsAction", Context = "Any", FontColor = "Blue", FontStyle = System.Drawing.FontStyle.Bold, BackColor = "Gray")]
    public class SpreadSheetEntry_AnalyticalBatch : BaseObject, ICheckedListBoxItemsProvider
    {
        ResultEntryQueryPanelInfo objQPInfo = new ResultEntryQueryPanelInfo();
        List<Guid> assignedTestMethods = new List<Guid>();
        List<Guid> assignedValidationTestMethods = new List<Guid>();
        List<Guid> assignedApprovalTestMethods = new List<Guid>();
        QCResultValidationQueryPanelInfo objDRQPInfo = new QCResultValidationQueryPanelInfo();
        ReportingQueryPanelInfo objRQPInfo = new ReportingQueryPanelInfo();
        Qcbatchinfo qcbatchinfo = new Qcbatchinfo();
        viewInfo strviewid = new viewInfo();
        InfoClass.NavigationRefresh objnavigationRefresh = new InfoClass.NavigationRefresh();
        public SpreadSheetEntry_AnalyticalBatch(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Status = 1;
            Noruns = 1;
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }

        string fAnalyticalBatchID;
        [Size(50)]
        public string AnalyticalBatchID
        {
            get { return fAnalyticalBatchID; }
            set { SetPropertyValue<string>(nameof(AnalyticalBatchID), ref fAnalyticalBatchID, value); }
        }
        byte[] fSpreadSheet;
        [Size(SizeAttribute.Unlimited)]
        [MemberDesignTimeVisibility(true)]
        public byte[] SpreadSheet
        {
            get { return fSpreadSheet; }
            set { SetPropertyValue<byte[]>(nameof(SpreadSheet), ref fSpreadSheet, value); }
        }
        DateTime fCreatedDate;
        public DateTime CreatedDate
        {
            get { return fCreatedDate; }
            set { SetPropertyValue<DateTime>(nameof(CreatedDate), ref fCreatedDate, value); }
        }
        Employee fCreatedBy;
        public Employee CreatedBy
        {
            get { return fCreatedBy; }
            set { SetPropertyValue<Employee>(nameof(CreatedBy), ref fCreatedBy, value); }
        }

        int fuqCalibrationID;
        public int uqCalibrationID
        {
            get { return fuqCalibrationID; }
            set { SetPropertyValue<int>(nameof(uqCalibrationID), ref fuqCalibrationID, value); }
        }

        int fTemplateID;
        public int TemplateID
        {
            get { return fTemplateID; }
            set { SetPropertyValue<int>(nameof(TemplateID), ref fTemplateID, value); }
        }
        int fStatus;
        public int Status
        {
            get { return fStatus; }
            set { SetPropertyValue<int>(nameof(Status), ref fStatus, value); }
        }
        AnalyticalBatchStatus _NonStatus;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        [XafDisplayName("Status")]
        [NonPersistent]
        public AnalyticalBatchStatus NonStatus
        {
            get
            {
                if (Test != null && Test.IsPLMTest)
                {
                    if (Status == 0)
                    {
                        _NonStatus = AnalyticalBatchStatus.PendingResultEntry;
                    }
                    else if (Status == 1)
                    {
                        _NonStatus = AnalyticalBatchStatus.PendingCompletion;
                    }
                    else if (Status == 2 || Status == 3)
                    {
                        _NonStatus = AnalyticalBatchStatus.PendingValidation;
                    }
                    else if (Status == 4)
                    {
                        _NonStatus = AnalyticalBatchStatus.Validated;
                    }
                }
                else
                {
                    if (Status == 1 && TemplateID == 0)
                    {
                        _NonStatus = AnalyticalBatchStatus.PendingResultEntry;
                    }
                    else if (Status == 1)
                    {
                        _NonStatus = AnalyticalBatchStatus.PendingCompletion;
                    }
                    else if (Status == 2 || Status == 3)
                    {
                        _NonStatus = AnalyticalBatchStatus.PendingValidation;
                    }
                    else if (Status == 4)
                    {
                        _NonStatus = AnalyticalBatchStatus.Validated;
                    }
                }
                return _NonStatus;
            }

        }

        #region SampleParameterStatus
        private SampleParameter _SampleParameterStatus;
        [XafDisplayName("Status")]
        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public SampleParameter SampleParameterStatus
        {
            get
            {
                if (strviewid.strtempresultentryviewid == "SpreadSheetEntry_AnalyticalBatch_ListView")
                {
                    SampleParameter strstatus = Session.FindObject<SampleParameter>(CriteriaOperator.Parse("[UQABID.Oid] = ? And [QCBatchID.QCType.QCTypeName] = 'Sample'", Oid));
                    if (strstatus != null)
                    {
                        _SampleParameterStatus = strstatus;
                    }
                    else
                    {
                        SampleParameter strstatusentry = Session.FindObject<SampleParameter>(CriteriaOperator.Parse("[Status] = 'PendingEntry'"));
                        _SampleParameterStatus = strstatusentry;
                    }

                }
                return _SampleParameterStatus;
            }
            set { SetPropertyValue(nameof(Samplestatus), ref _SampleParameterStatus, value); }
        }
        #endregion

        private DataPackageStatus _DPStatus;
        public DataPackageStatus DPStatus
        {
            get { return _DPStatus; }
            set { SetPropertyValue(nameof(DPStatus), ref _DPStatus, value); }
        }

        int fuqJobID;
        public int uqJobID
        {
            get { return fuqJobID; }
            set { SetPropertyValue<int>(nameof(uqJobID), ref fuqJobID, value); }
        }

        //private QCBatch fqcbatchID;
        //[VisibleInDetailView(false)]
        //[VisibleInListView(false)]
        //[VisibleInLookupListView(false)]
        //public QCBatch qcbatchID
        //{
        //    get { return fqcbatchID; }
        //    set { SetPropertyValue("qcbatchID", ref fqcbatchID, value); }
        //}

        private Matrix _Matrix;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [DataSourceProperty("MatrixDataSource")]
        [ImmediatePostData]
        public Matrix Matrix
        {
            get
            {
                if (_Matrix == null)
                {
                    Test = null;
                    Method = null;
                    Jobid = null;
                    NPJobid = null;
                }
                return _Matrix;
            }
            set
            {
                SetPropertyValue("Matrix", ref _Matrix, value);

            }
        }
        #region Method
        private TestMethod _Method;
        [DataSourceProperty("MethodDataSource")]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [ImmediatePostData]
        public TestMethod Method
        {
            get
            {
                if (_Method == null)
                {
                    Jobid = null;
                    NPJobid = null;
                }
                return _Method;
            }
            set { SetPropertyValue("Method", ref _Method, value); }
        }
        #endregion

        private Calibration fCalibration;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Calibration Calibration
        {
            get { return fCalibration; }
            set
            {
                SetPropertyValue("Calibration", ref fCalibration, value);
                if (value != null)
                {
                    uqCalibrationID = value.uqID;
                }
            }
        }
        private TestMethod _Test;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [DataSourceProperty("TestDataSource")]
        [ImmediatePostData]
        public TestMethod Test
        {
            get
            {
                if (_Test == null)
                {
                    Method = null;
                    Jobid = null;
                    NPJobid = null;
                }
                return _Test;
            }
            set
            {
                SetPropertyValue("Test", ref _Test, value);
                if (Test != null && qcbatchinfo.IsResetActionEnable == false && objnavigationRefresh.ClickedNavigationItem == "AnalysisQueue ")
                {
                    Method = Test;
                }

            }
        }
        private string _NPJobid;
        [VisibleInListView(false)]
        [NonPersistent]
        [Appearance("ABNPJobid", Visibility = ViewItemVisibility.Hide, Criteria = "!ISShown", Context = "DetailView")]
        [RuleRequiredField("ABNPJobid", DefaultContexts.Save)]
        public string NPJobid
        {
            get
            {
                if (!Session.IsNewObject(this) && !string.IsNullOrEmpty(Jobid))
                {
                    _NPJobid = Jobid;
                }
                return _NPJobid;
            }
            set { SetPropertyValue("NPJobid", ref _NPJobid, value); }
        }

        private string _Jobid;
        [Appearance("Jobid", Visibility = ViewItemVisibility.Hide, Criteria = "ISShown", Context = "DetailView")]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [Size(int.MaxValue)]
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

        private string _RollBackReason;
        [Size(SizeAttribute.Unlimited)]
        public string RollBackReason
        {
            get
            {
                return _RollBackReason;
            }
            set
            {
                SetPropertyValue<string>(nameof(RollBackReason), ref _RollBackReason, value);
            }
        }

        #region Reagents
        [Association("AnalyticalBatchReagents", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Reagent> Reagents
        {
            get
            {
                return GetCollection<Reagent>(nameof(Reagents));
            }
        }
        #endregion

        #region Instruments
        [Association("AnalyticalBatchInstruments", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<Labware> Instruments
        {
            get
            {
                return GetCollection<Labware>(nameof(Instruments));
            }
        }
        #endregion

        private string _Instrument;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Instrument
        {
            get 
            {
                return _Instrument;
            }
            set { SetPropertyValue("Instrument", ref _Instrument, value); }
        }
        private string _NPInstrument;
        [VisibleInListView(false)]
        [NonPersistent]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [Appearance("ABNPInstrument", Visibility = ViewItemVisibility.Hide, Criteria = "!ISShown", Context = "DetailView")]
        public string NPInstrument
        {
            get
            {
                if (!Session.IsNewObject(this) && !string.IsNullOrEmpty(Instrument))
                {
                    _NPInstrument = Instrument;
                }
                return _NPInstrument;
            }
            set { SetPropertyValue("NPInstrument", ref _NPInstrument, value); }
        }
        private string _strInstrument;
        [Appearance("Instrument", Visibility = ViewItemVisibility.Hide, Criteria = "ISShown", Context = "DetailView")]
        [DevExpress.Persistent.Validation.RuleRequiredField]
        public string strInstrument
        {
            get
            {
                //if (!string.IsNullOrEmpty(Instrument))
                //{
                //    //XPCollection<Labware> lstInstruments = new XPCollection<Labware>(Session);
                //    //lstInstruments.Criteria = new InOperator("Oid", Instrument.Split(';').Select(i => new Guid(i.Trim())));
                //    //if (lstInstruments != null && lstInstruments.Count > 0)
                //    //{
                //    //    _strInstrument = string.Join(",", lstInstruments.Select(i => i.AssignedName).Distinct().OrderBy(i => i).ToList());
                //    //}
                //}
                return _strInstrument;
            }
            set { SetPropertyValue("strInstrument", ref _strInstrument, value); }
        }

        public SpreadSheetBuilder_TemplateInfo Template
        {
            get
            {
                return Session.FindObject<SpreadSheetBuilder_TemplateInfo>(CriteriaOperator.Parse("[TemplateID] = ?", TemplateID));
            }
        }
        [NonPersistent]
        [XafDisplayName("Template")]
        public string TemplateName
        {
            get
            {
                if (Template != null)
                {
                    return Template.TemplateName;
                }
                else
                {
                    return "N/A";
                }
            }
        }
        Employee _RollBackedBy;
        public Employee RollBackedBy
        {
            get { return _RollBackedBy; }
            set { SetPropertyValue<Employee>(nameof(RollBackedBy), ref _RollBackedBy, value); }
        }
        DateTime? _RollBackedDate;
        public DateTime? RollBackedDate
        {
            get { return _RollBackedDate; }
            set { SetPropertyValue<DateTime?>(nameof(RollBackedDate), ref _RollBackedDate, value); }
        }

        Employee fReviewedBy;
        public Employee ReviewedBy
        {
            get { return fReviewedBy; }
            set { SetPropertyValue<Employee>(nameof(ReviewedBy), ref fReviewedBy, value); }
        }
        DateTime? fReviewedDate;
        public DateTime? ReviewedDate
        {
            get { return fReviewedDate; }
            set { SetPropertyValue<DateTime?>(nameof(ReviewedDate), ref fReviewedDate, value); }
        }
        Employee fVerifiedBy;
        public Employee VerifiedBy
        {
            get { return fVerifiedBy; }
            set { SetPropertyValue<Employee>(nameof(VerifiedBy), ref fVerifiedBy, value); }
        }
        DateTime? fVerifiedDate;
        public DateTime? VerifiedDate
        {
            get { return fVerifiedDate; }
            set { SetPropertyValue<DateTime?>(nameof(VerifiedDate), ref fVerifiedDate, value); }
        }

        private string _Humidity;
        public string Humidity
        {
            get { return _Humidity; }
            set { SetPropertyValue("Humidity", ref _Humidity, value); }
        }

        private uint _Noruns;
        public uint Noruns
        {
            get { return _Noruns; }
            set { SetPropertyValue("Noruns", ref _Noruns, value); }
        }

        private string _Roomtemp;
        public string Roomtemp
        {
            get { return _Roomtemp; }
            set { SetPropertyValue("Roomtemp", ref _Roomtemp, value); }
        }

        #region QClink
        [Association("QClink")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public XPCollection<QCBatchSequence> qcdetail
        {
            get { return GetCollection<QCBatchSequence>("qcdetail"); }
        }
        #endregion

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Matrix> MatrixDataSource
        {
            get
            {
                bool Administrator = false;
                List<Guid> lstMatrixOid = new List<Guid>();
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                {
                    Administrator = true;
                }
                else
                {
                    //IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                    XPCollection<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                    {
                        lstAnalysisDepartChain = new XPCollection<AnalysisDepartmentChain>(Session, CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] =True", currentUser.Oid));
                    }
                    if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                    {
                        foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                        {
                            foreach (TestMethod testMethod in departmentChain.TestMethods)
                            {
                                if (!lstMatrixOid.Contains(testMethod.MatrixName.Oid))
                                {
                                    lstMatrixOid.Add(testMethod.MatrixName.Oid);
                                }
                            }
                        }
                    }
                }
                if (!Administrator)
                {
                    return new XPCollection<Matrix>(Session, new InOperator("Oid", lstMatrixOid));
                }
                else
                {
                    return new XPCollection<Matrix>(Session, CriteriaOperator.Parse(""));
                }
            }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (Matrix != null && Test == null)
                {
                    bool Administrator = false;
                    List<Guid> lstTestMethodOid = new List<Guid>();
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        Administrator = true;
                    }
                    else
                    {
                        //IList<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        XPCollection<AnalysisDepartmentChain> lstAnalysisDepartChain = null;
                        {
                            lstAnalysisDepartChain = new XPCollection<AnalysisDepartmentChain>(Session, CriteriaOperator.Parse("[Employee.Oid] = ? And [ResultEntry] =True", currentUser.Oid));
                        }
                        if (lstAnalysisDepartChain != null && lstAnalysisDepartChain.Count > 0)
                        {
                            foreach (AnalysisDepartmentChain departmentChain in lstAnalysisDepartChain)
                            {
                                foreach (TestMethod testMethod in departmentChain.TestMethods)
                                {
                                    if (testMethod.MatrixName.MatrixName == Matrix.MatrixName)
                                    {
                                        if (!lstTestMethodOid.Contains(testMethod.Oid))
                                        {
                                            lstTestMethodOid.Add(testMethod.Oid);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!Administrator)
                    {
                        return new XPCollection<TestMethod>(Session, new InOperator("Oid", lstTestMethodOid));
                    }
                    else
                    {
                        XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName]=? and [MethodName.GCRecord] is null And ([IsFieldTest] Is Null Or [IsFieldTest] = False)", Matrix));
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
                    XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName] =? And ([IsFieldTest] Is Null Or [IsFieldTest] = False)", Test.TestName, Matrix.MatrixName));
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
            if (targetMemberName == "Instrument" && Method != null)
            {
                if (Method.Labwares.Count > 0)
                {
                    foreach (Labware objlab in Method.Labwares.OrderBy(a => a.AssignedName).ToList())
                    {
                        if (!properties.ContainsKey(objlab.Oid) && !string.IsNullOrEmpty(objlab.AssignedName))
                        {
                            properties.Add(objlab.Oid, objlab.AssignedName);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Instrument))
                {
                    string[] strinstrument = Instrument.Split(';');
                    foreach (string strobj in strinstrument)
                    {
                        Labware objlab = Session.FindObject<Labware>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strobj)));
                        if (!properties.ContainsKey(objlab.Oid) && !string.IsNullOrEmpty(objlab.AssignedName))
                        {
                            properties.Add(objlab.Oid, objlab.AssignedName);
                        }
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
    }
}