using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//using Modules.BusinessObjects.ContractManagement;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.Setting.PLM;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Method = Modules.BusinessObjects.Setting.Method;
//using NavigationItem = Modules.BusinessObjects.Hr.NavigationItem;

namespace Modules.BusinessObjects.PLM
{
    public class PTStudyLog : BaseObject, ICheckedListBoxItemsProvider
    {
        MessageTimer timer = new MessageTimer();
        InfoClass.NavigationRefresh objnavigationRefresh = new InfoClass.NavigationRefresh();
        public PTStudyLog(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ReleaseBool = false;
        }
        protected override void OnSaving()
        {
            base.OnSaving();

            {
                if (objnavigationRefresh.ClickedNavigationItem == "PTStudyLog")
                {

                    if (string.IsNullOrEmpty(PTID))
                    {
                        CriteriaOperator idct = CriteriaOperator.Parse("Max(SUBSTRING(PTID, 4))");
                        string tempid = (Convert.ToInt32(Session.Evaluate(typeof(PTStudyLog), idct, null)) + 1).ToString();
                        var curdate = DateTime.Now.ToString("yy");
                        if (tempid != "1")
                        {
                            //var predate = tempid.Substring(0, 7);
                            if (Convert.ToInt32(tempid) < 10)
                            {
                                tempid = "PT" + curdate + "100" + tempid;
                            }
                            else if (Convert.ToInt32(tempid) < 100)
                            {
                                tempid = "PT" + curdate + "10" + tempid;
                            }
                            else if (Convert.ToInt32(tempid) >= 100)
                            {
                                tempid = "PT" + curdate + tempid;
                            }
                        }
                        else
                        {
                            tempid = "PT" + curdate + "1001";
                        }
                        PTID = tempid;
                    }
                }

            }
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            SubmittedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
        }

        public enum PTStudyLogStatus
        {
            [XafDisplayName("Pending Lab Result Entry")]
            PendingLabResultEntry,
            [XafDisplayName("Pending PT Result Entry")]
            PendingPTResultEntry,
            [XafDisplayName("Pending Submission")]
            PendingSubmission,
            [XafDisplayName("Submitted")]
            Submitted

        }
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "Matrix" && Matrixes != null && Matrixes.Count > 0)
            {
                foreach (Matrix objSampleMatrix in Matrixes.Where(i => i.MatrixName != null).OrderBy(i => i.MatrixName).ToList())
                {
                    if (!Properties.ContainsKey(objSampleMatrix.Oid))
                    {
                        Properties.Add(objSampleMatrix.Oid, objSampleMatrix.MatrixName);
                    }
                }
            }
            if (targetMemberName == "Test" && TestDataSource != null && TestDataSource.Count > 0)
            {
                foreach (TestMethod objTest in TestDataSource.Where(i => i.TestName != null).OrderBy(i => i.TestName).ToList())
                {
                    if (!Properties.ContainsValue(objTest.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestName);
                    }
                }
            }
            if (targetMemberName == "Method" && MethodDataSource != null && MethodDataSource.Count > 0)
            {
                foreach (Method objMethod in MethodDataSource.Where(i => i.MethodNumber != null).OrderBy(i => i.MethodNumber).ToList())
                {
                    if (!Properties.ContainsValue(objMethod.MethodNumber))
                    {
                        Properties.Add(objMethod.Oid, objMethod.MethodNumber);
                    }
                }
            }
            return Properties;
        }


        private DateTime _DatePTSampleReceived;
        public DateTime DatePTSampleReceived
        {
            get { return _DatePTSampleReceived; }
            set { SetPropertyValue(nameof(DatePTSampleReceived), ref _DatePTSampleReceived, value); }
        }

        private Source _Source;
        // [RuleRequiredField]
        public Source Source
        {
            get { return _Source; }
            set { SetPropertyValue(nameof(Source), ref _Source, value); }
        }

        private StudyName _StudyName;
        [RuleRequiredField]
        public StudyName StudyName
        {
            get { return _StudyName; }
            set { SetPropertyValue(nameof(StudyName), ref _StudyName, value); }
        }

        private string _JobID;
        // [RuleRequiredField]
        public string JobID
        {
            get { return _JobID; }
            set { SetPropertyValue(nameof(JobID), ref _JobID, value); }
        }

        private StudyID _StudyID;
        [RuleRequiredField]
        public StudyID StudyID
        {
            get { return _StudyID; }
            set { SetPropertyValue(nameof(StudyID), ref _StudyID, value); }
        }

        private Client_Request_General_Information _Client;
        [ImmediatePostData]
        [Browsable(false)]
        public Client_Request_General_Information Client
        {
            get { return _Client; }
            set { SetPropertyValue(nameof(Client), ref _Client, value); }
        }
        private Employee _Analyst;
        public Employee Analyst
        {
            get { return _Analyst; }
            set { SetPropertyValue(nameof(Analyst), ref _Analyst, value); }
        }

        private string _Matrix;
        //[RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Matrix> Matrixes
        {
            get
            {
                return new XPCollection<Matrix>(Session, CriteriaOperator.Parse(""));
            }
        }

        private string _Test;
        // [RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Matrix))
                {
                    List<string> lstSM = new List<string>();
                    List<string> lstSMOid = Matrix.Split(';').ToList();
                    if (lstSMOid != null)
                    {
                        foreach (string objOid in lstSMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Matrix objMatrix = Session.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                if (objMatrix != null && !lstSM.Contains(objMatrix.MatrixName))
                                {
                                    lstSM.Add(objMatrix.MatrixName);
                                }
                            }

                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("MatrixName.MatrixName", lstSM));
                }
                else
                {
                    return null;
                }
            }
        }
        private string _Method;
        //[RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Method> MethodDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Test))
                {
                    List<string> lstSM = new List<string>();
                    List<string> lstSMOid = Test.Split(';').ToList();
                    if (lstSMOid != null)
                    {
                        foreach (string objOid in lstSMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                TestMethod objTest = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                if (objTest != null && !lstSM.Contains(objTest.MethodName.MethodNumber))
                                {
                                    lstSM.Add(objTest.MethodName.MethodNumber);
                                }
                            }

                        }
                    }
                    return new XPCollection<Method>(Session, new InOperator("MethodNumber", lstSM));
                }
                else
                {
                    return null;
                }
            }
        }
        //[Browsable(false)]
        //[ImmediatePostData]
        //[NonPersistent]
        //public XPCollection<TestMethod> TestDataSource
        //{
        //    get
        //    {
        //        if (Matrix != null)
        //        {
        //            List<object> ObjTest = new List<object>();
        //            using (XPView lstview = new XPView(Session, typeof(TestMethod)))
        //            {
        //                lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName]=?", Matrix.MatrixName);
        //                lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
        //                lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
        //                foreach (ViewRecord rec in lstview)
        //                    ObjTest.Add(rec["Toid"]);
        //            }
        //            if (ObjTest.Count == 0)
        //            {
        //                XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=?", Matrix.MatrixName));
        //                return tests;
        //            }
        //            else
        //            {
        //                XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, new InOperator("Oid", ObjTest));
        //                return tests;
        //            }
        //        }
        //        else
        //        {

        //            return null;
        //        }
        //    }
        //}        

        private Testparameter _Parameter;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Testparameter Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue(nameof(Parameter), ref _Parameter, value); }
        }

        //[Browsable(false)]
        //[NonPersistent]
        //public XPCollection<Parameter> ParameterDataSource
        //{
        //    get
        //    {
        //        if (Method != null && Matrix != null && Test != null && Parameter == null)
        //        {
        //            List<string> objlist = new List<string>();
        //            XPCollection<Testparameter> objparameter = new XPCollection<Testparameter>(Session, CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ?", Matrix, Test.TestName, Method.MethodName.MethodNumber));
        //            foreach (Testparameter objparameter1 in objparameter)
        //            {
        //                if (!objlist.Contains(objparameter1.Parameter.ParameterName))
        //                {
        //                    objlist.Add(objparameter1.Parameter.ParameterName);
        //                }
        //            }
        //            return new XPCollection<Parameter>(Session, new InOperator("ParameterName", objlist));
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}       

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }

        private string _PTID;
        public string PTID
        {
            get { return _PTID; }
            set { SetPropertyValue(nameof(PTID), ref _PTID, value); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(nameof(Name), ref _Name, value); }
        }

        #region Attachment Collection
        [Association("Attachment-PTStudyLog")]
        public XPCollection<Attachment> Attachments
        {
            get { return GetCollection<Attachment>(nameof(Attachments)); }
        }
        #endregion

        #region Notes Collection   
        [Association("Notes_PTStudyLog")]
        public XPCollection<Notes> Notes
        {
            get { return GetCollection<Notes>(nameof(Notes)); }
        }
        #endregion

        private DateTime _DateLabResultSubmitted;
        public DateTime DateLabResultSubmitted
        {
            get { return _DateLabResultSubmitted; }
            set { SetPropertyValue(nameof(DateLabResultSubmitted), ref _DateLabResultSubmitted, value); }
        }

        private DateTime _DatePTResultReceived;
        public DateTime DatePTResultReceived
        {
            get { return _DatePTResultReceived; }
            set { SetPropertyValue(nameof(DatePTResultReceived), ref _DatePTResultReceived, value); }
        }


        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }

        private DateTime _DateAnalyzed;
        public DateTime DateAnalyzed
        {
            get { return _DateAnalyzed; }
            set { SetPropertyValue(nameof(DateAnalyzed), ref _DateAnalyzed, value); }
        }
        public event EventHandler ItemsChanged;
        private Employee _SubmittedBy;
        public Employee SubmittedBy
        {
            get
            {
                return _SubmittedBy;
            }
            set
            {
                SetPropertyValue(nameof(SubmittedBy), ref _SubmittedBy, value);
            }
        }
        [Association("PTStudyLog-PTStudyLogResults")]
        public XPCollection<PTStudyLogResults> Results
        {
            get { return GetCollection<PTStudyLogResults>(nameof(Results)); }
        }

        private bool _ReleaseBool;
        //[Browsable(false)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public bool ReleaseBool
        {
            get { return _ReleaseBool; }
            set { SetPropertyValue(nameof(ReleaseBool), ref _ReleaseBool, value); }
        }

        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        private Employee _QAOfficer;
        public Employee QAOfficer
        {
            get { return _QAOfficer; }
            set { SetPropertyValue(nameof(QAOfficer), ref _QAOfficer, value); }
        }

        private PTStudyLogStatus _Status;
        public PTStudyLogStatus Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value); }
        }

        private Samplecheckin _SampleCheckinJobID;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Samplecheckin SampleCheckinJobID
        {
            get { return _SampleCheckinJobID; }
            set { SetPropertyValue(nameof(SampleCheckinJobID), ref _SampleCheckinJobID, value); }
        }
        private string _Matrixs;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        public string Matrixs
        {
            get
            {
                if (Oid != null)
                {
                    XPCollection<PTStudyLogResults> lstSamples = new XPCollection<PTStudyLogResults>(Session, CriteriaOperator.Parse("[PTStudyLog.Oid] =?", Oid));
                    _Matrixs = string.Format("{0}", string.Join(";", lstSamples.Where(i => i.PTStudyLog != null && i.SampleID != null && i.SampleID.Testparameter != null && i.PTStudyLog.Oid == Oid && i.SampleID.Testparameter.TestMethod.MatrixName.MatrixName != null).Select(i => i.SampleID.Testparameter.TestMethod.MatrixName.MatrixName).Distinct().ToList()));
                }
                else
                {
                    _Matrixs = null;
                }

                return _Matrixs;
            }
        }

        private string _Tests;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        public string Tests
        {
            get
            {
                if (Oid != null)
                {
                    XPCollection<PTStudyLogResults> lstSamples = new XPCollection<PTStudyLogResults>(Session, CriteriaOperator.Parse("[PTStudyLog.Oid] =?", Oid));
                    _Tests = string.Format("{0}", string.Join(";", lstSamples.Where(i => i.PTStudyLog != null && i.SampleID != null && i.SampleID.Testparameter != null && i.PTStudyLog.Oid == Oid && i.SampleID.Testparameter.TestMethod.TestName != null).Select(i => i.SampleID.Testparameter.TestMethod.TestName).Distinct().ToList()));
                }
                else
                {
                    _Tests = null;
                }
                return _Tests;
            }

        }

        private string _Methods;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [NonPersistent]
        public string Methods
        {
            get
            {
                if (Oid != null)
                {
                    XPCollection<PTStudyLogResults> lstSamples = new XPCollection<PTStudyLogResults>(Session, CriteriaOperator.Parse("[PTStudyLog.Oid] =?", Oid));
                    _Methods = string.Format("{0}", string.Join(";", lstSamples.Where(i => i.SampleID != null && i.SampleID.Testparameter != null && i.SampleID.Testparameter.TestMethod.MethodName.MethodNumber != null).Select(i => i.SampleID.Testparameter.TestMethod.MethodName.MethodNumber).Distinct().ToList()));
                }
                else
                {
                    _Methods = null;
                }
                return _Methods;
            }
        }
        private string _Category;
        public string Category
        {
            get { return _Category; }
            set { SetPropertyValue(nameof(Category), ref _Category, value); }
        }

        //&& i.SampleID.Testparameter.TestMethod.TestName !=null && i.SampleID.Testparameter.TestMethod.MethodName.MethodNumber !=null && i.SampleID.Testparameter.Parameter.ParameterName !=null
    }
}