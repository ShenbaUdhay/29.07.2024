using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.PLM;
using Modules.BusinessObjects.SampleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting.PLM
{
    [DefaultClassOptions]
    public class PTStudyLogResults : BaseObject
    {
        public PTStudyLogResults(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }

        public enum TypesOfAcceptable
        {
            [XafDisplayName("N/A")]
            NA,
            [XafDisplayName("Yes")]
            Yes,
            [XafDisplayName("No")]
            No,
        }

        private SampleParameter _SampleID;
        public SampleParameter SampleID
        {
            get { return _SampleID; }
            set { SetPropertyValue(nameof(SampleID), ref _SampleID, value); }
        }

        private Matrix _Matrix;
        [ImmediatePostData]
        [DataSourceProperty("MatrixDataSource")]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
        }

        private TestMethod _Test;
        [ImmediatePostData]
        [DataSourceProperty("TestDataSource")]
        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        [Browsable(false)]
        public XPCollection<Matrix> MatrixDataSource
        {
            get
            {
                if (PTStudyLog != null)
                {
                    List<string> lstOid = new List<string>();
                    List<string> lstMatrix = new List<string>();
                    lstOid = PTStudyLog.Matrix.Split(';').ToList();
                    if (lstOid != null)
                    {
                        foreach (string objOid in lstOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Matrix objMatrix = Session.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                if (objMatrix != null && !lstMatrix.Contains(objMatrix.MatrixName))
                                {
                                    lstMatrix.Add(objMatrix.MatrixName);
                                }
                            }

                        }
                    }
                    return new XPCollection<Matrix>(Session, new InOperator("MatrixName", lstMatrix));
                }
                else
                {
                    return null;
                }
            }
        }

        [Browsable(false)]
        //[ImmediatePostData]

        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (Matrix != null)
                {
                    if (PTStudyLog != null)
                    {
                        List<string> lstOid = new List<string>();
                        List<Guid> lstTest = new List<Guid>();
                        lstOid = PTStudyLog.Test.Split(';').ToList();
                        if (lstOid != null)
                        {
                            foreach (string objOid in lstOid)
                            {
                                if (!string.IsNullOrEmpty(objOid))
                                {
                                    TestMethod objTest = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                    if (objTest != null && !lstTest.Contains(objTest.Oid) && objTest.MatrixName == Matrix)
                                    {
                                        lstTest.Add(objTest.Oid);
                                    }
                                }
                            }
                        }
                        return new XPCollection<TestMethod>(Session, new InOperator("Oid", lstTest));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        private Method _Method;
        [ImmediatePostData]
        [DataSourceProperty("MethodDataSource")]
        public Method Method
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
                if (Test != null)
                {
                    if (PTStudyLog != null)
                    {
                        List<string> lstOid = new List<string>();
                        List<Guid> lstMethod = new List<Guid>();
                        lstOid = PTStudyLog.Method.Split(';').ToList();
                        if (lstOid != null)
                        {
                            foreach (string objOid in lstOid)
                            {
                                if (!string.IsNullOrEmpty(objOid))
                                {
                                    TestMethod testMethod = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                    if (testMethod != null && !lstMethod.Contains(testMethod.Oid) && testMethod == Test)
                                    {
                                        lstMethod.Add(testMethod.Oid);
                                    }
                                }
                            }
                        }
                        return new XPCollection<Method>(Session, new InOperator("Oid", lstMethod));
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private Parameter _Parameter;
        [ImmediatePostData]
        [DataSourceProperty("ParameterDataSource")]
        public Parameter Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue(nameof(Parameter), ref _Parameter, value); }
        }

        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Parameter> ParameterDataSource
        {
            get
            {
                if (Method != null && Matrix != null && Test != null && Parameter == null)
                {
                    List<string> objlist = new List<string>();
                    XPCollection<Testparameter> objparameter = new XPCollection<Testparameter>(Session, CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] = ? And [TestMethod.TestName] = ? And [TestMethod.MethodName.MethodNumber] = ?", Matrix.MatrixName, Test.TestName, Method.MethodName));
                    foreach (Testparameter objparameter1 in objparameter)
                    {
                        if (!objlist.Contains(objparameter1.Parameter.ParameterName))
                        {
                            objlist.Add(objparameter1.Parameter.ParameterName);
                        }
                    }
                    return new XPCollection<Parameter>(Session, new InOperator("ParameterName", objlist));
                }
                else
                {
                    return null;
                }
            }
        }

        private string _ReportedValue;
        public string ReportedValue
        {
            get
            {
                //if (SampleID != null && SampleID.Status != Samplestatus.PendingEntry && SampleID.Status != Samplestatus.PendingValidation && SampleID.Status != Samplestatus.PendingApproval && SampleID.Status != Samplestatus.PendingReview && SampleID.Status != Samplestatus.PendingVerify && _ReportedValue == null)
                //{
                //    _ReportedValue = SampleID.Result;
                //}
                //else
                //{
                //    _ReportedValue = null;
                //}
                if (SampleID != null && SampleID.Result != null && DateAnalyzed != null && AnalyzedBy != null && DateAnalyzed != DateTime.MinValue)
                {
                    _ReportedValue = SampleID.Result;
                }
                else
                {
                    _ReportedValue = null;
                }
                return _ReportedValue;
            }
            set { SetPropertyValue(nameof(ReportedValue), ref _ReportedValue, value); }
        }
        private DateTime? _DateAnalyzed;
        public DateTime? DateAnalyzed
        {
            get
            {
                //if (SampleID != null && SampleID.Status != Samplestatus.PendingEntry && SampleID.Status != Samplestatus.PendingValidation && SampleID.Status != Samplestatus.PendingApproval && SampleID.Status != Samplestatus.PendingReview && SampleID.Status != Samplestatus.PendingVerify && _DateAnalyzed == null)
                //{
                //    _DateAnalyzed = SampleID.AnalyzedDate;
                //}
                //else
                //{
                //    _DateAnalyzed = null;
                //}
                if (SampleID != null && SampleID.AnalyzedDate != null)
                {
                    _DateAnalyzed = SampleID.AnalyzedDate;
                }
                else
                {
                    _DateAnalyzed = null;
                }
                return _DateAnalyzed;
            }
            set { SetPropertyValue(nameof(DateAnalyzed), ref _DateAnalyzed, value); }
        }
        private Employee _AnalyzedBy;
        public Employee AnalyzedBy
        {
            get
            {
                //if (SampleID != null && SampleID.Status != Samplestatus.PendingEntry && SampleID.Status != Samplestatus.PendingValidation && SampleID.Status != Samplestatus.PendingApproval && SampleID.Status != Samplestatus.PendingReview && SampleID.Status != Samplestatus.PendingVerify && _AnalyzedBy == null)
                //{
                //    _AnalyzedBy = SampleID.AnalyzedBy;
                //}
                //else
                //{
                //    _AnalyzedBy = null;
                //}
                if (SampleID != null && SampleID.AnalyzedBy != null)
                {
                    _AnalyzedBy = SampleID.AnalyzedBy;
                }
                else
                {
                    _AnalyzedBy = null;
                }
                return _AnalyzedBy;
            }
            set { SetPropertyValue(nameof(AnalyzedBy), ref _AnalyzedBy, value); }
        }

        private string _Z_Score;
        public string Z_Score
        {
            get { return _Z_Score; }
            set { SetPropertyValue(nameof(Z_Score), ref _Z_Score, value); }
        }

        private string _PT_Value;
        public string PT_Value
        {
            get { return _PT_Value; }
            set { SetPropertyValue(nameof(PT_Value), ref _PT_Value, value); }
        }

        private TypesOfAcceptable _Acceptable;
        public TypesOfAcceptable Acceptable
        {
            get { return _Acceptable; }
            set { SetPropertyValue(nameof(Acceptable), ref _Acceptable, value); }
        }

        private string _R_Analyst;
        public string R_Analyst
        {
            get { return _R_Analyst; }
            set { SetPropertyValue(nameof(R_Analyst), ref _R_Analyst, value); }
        }

        private string _Recheck_1;
        public string Recheck_1
        {
            get { return _Recheck_1; }
            set { SetPropertyValue(nameof(Recheck_1), ref _Recheck_1, value); }
        }

        private string _Recheck_2;
        public string Recheck_2
        {
            get { return _Recheck_2; }
            set { SetPropertyValue(nameof(Recheck_2), ref _Recheck_2, value); }
        }

        private string _Recheck_3;
        public string Recheck_3
        {
            get { return _Recheck_3; }
            set { SetPropertyValue(nameof(Recheck_3), ref _Recheck_3, value); }
        }

        private string _Sub_1;
        public string Sub_1
        {
            get { return _Sub_1; }
            set { SetPropertyValue(nameof(Sub_1), ref _Sub_1, value); }
        }

        private string _Sub_2;
        public string Sub_2
        {
            get { return _Sub_2; }
            set { SetPropertyValue(nameof(Sub_2), ref _Sub_2, value); }
        }

        private string _Sub_3;
        public string Sub_3
        {
            get { return _Sub_3; }
            set { SetPropertyValue(nameof(Sub_3), ref _Sub_3, value); }
        }

        private string _Low_ACCT_Lt;
        public string Low_ACCT_Lt
        {
            get { return _Low_ACCT_Lt; }
            set { SetPropertyValue(nameof(Low_ACCT_Lt), ref _Low_ACCT_Lt, value); }
        }

        private string _Up_ACCT_Lt;
        public string Up_ACCT_Lt
        {
            get { return _Up_ACCT_Lt; }
            set { SetPropertyValue(nameof(Up_ACCT_Lt), ref _Up_ACCT_Lt, value); }
        }

        private PTStudyLog _PTStudyLog;
        [Association("PTStudyLog-PTStudyLogResults")]
        public PTStudyLog PTStudyLog
        {
            get { return _PTStudyLog; }
            set { SetPropertyValue(nameof(PTStudyLog), ref _PTStudyLog, value); }
        }

        private bool _Commentbool;
        [ImmediatePostData(true)]
        public bool Commentbool
        {
            get { return _Commentbool; }
            set { SetPropertyValue(nameof(Commentbool), ref _Commentbool, value); }
        }
        private string _Comment;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #region SampleLogin
        private SampleLogIn _SampleLogin;
        //[Browsable(false)]
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public SampleLogIn SampleLogin
        {
            get { return _SampleLogin; }
            set { SetPropertyValue(nameof(SampleLogin), ref _SampleLogin, value); }
        }
        #endregion
    }
}