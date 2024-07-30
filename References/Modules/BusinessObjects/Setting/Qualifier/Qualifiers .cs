using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Qualifier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty("Symbol")]

    public class Qualifiers : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Qualifiers(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);

            //SampleAutomation = new QualifierAutomation(Session);
            //SampleAutomation.Type = QualifierAutomation.TypeList.Sample;
            //SampleAutomation.SampleAuto = this;

            //QCAutomation = new QualifierAutomation(Session);
            //QCAutomation.Type = QualifierAutomation.TypeList.QC;
            //QCAutomation.SampleAuto = this;

            //SurAutomation = new QualifierAutomation(Session);
            //SurAutomation.Type = QualifierAutomation.TypeList.Surrogate;
            //SurAutomation.SampleAuto = this;
        }


        protected override void OnSaving()
        {
            base.OnSaving();

            if (string.IsNullOrEmpty(QualifierID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(QualifierID, 2))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(Qualifiers), criteria, null)) + 1).ToString();
                //var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "1")
                {
                    if (tempID.Length == 1)
                    {
                        tempID = "QF00" + tempID;
                    }
                    else
                    if (tempID.Length == 2)
                    {
                        tempID = "QF0" + tempID;
                    }
                    else
                    {
                        tempID = "QF" + tempID;
                    }
                }
                else
                {
                    tempID = "QF001";
                }

                QualifierID = tempID;
            }

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);

            if (SampleAutomation == null && !string.IsNullOrEmpty(SampleFormula))
            {
                SampleAutomation = new QualifierAutomation(Session);
                SampleAutomation.Type = QualifierAutomation.TypeList.Sample;
                SampleAutomation.SampleAuto = this;
            }

            if (QCAutomation == null && !string.IsNullOrEmpty(QCFormula))
            {
                QCAutomation = new QualifierAutomation(Session);
                QCAutomation.Type = QualifierAutomation.TypeList.QC;
                QCAutomation.SampleAuto = this;
            }

            if (SurAutomation == null && !string.IsNullOrEmpty(SurFormula))
            {
                SurAutomation = new QualifierAutomation(Session);
                SurAutomation.Type = QualifierAutomation.TypeList.Surrogate;
                SurAutomation.SampleAuto = this;
            }

            if (SampleAutomation != null)
            {
                SampleAutomation.Formula = SampleFormula;
                SampleAutomation.Matrix = SampleMatrices;
                SampleAutomation.Test = SampleTest;
                SampleAutomation.Method = SampleMethod;
                SampleAutomation.Parameter = SampleParameter;
            }

            if (QCAutomation != null)
            {
                QCAutomation.Formula = QCFormula;
                QCAutomation.Matrix = QCMatrix;
                QCAutomation.Test = QCTest;
                QCAutomation.Method = QCMethod;
                QCAutomation.Parameter = QCParameter;
            }

            if (SurAutomation != null)
            {
                SurAutomation.Formula = SurFormula;
                SurAutomation.Matrix = SurMatrices;
                SurAutomation.Test = SurTest;
                SurAutomation.Method = SurMethod;
                SurAutomation.Parameter = SurParameter;
            }

        }

        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                ModifiedDate = Library.GetServerTime(Session);

                Session.Delete(SampleAutomation);
                Session.Delete(QCAutomation);
                Session.Delete(SurAutomation);

                //if (SampleAutomation.Formula != null ||
                //SampleAutomation.Matrix != null ||
                //SampleAutomation.Test != null ||
                //SampleAutomation.Method != null ||
                //SampleAutomation.Parameter != null)
                //{
                //    SampleAutomation.Formula = null;
                //    SampleAutomation.Matrix = null;
                //    SampleAutomation.Test = null;
                //    SampleAutomation.Method = null;
                //    SampleAutomation.Parameter = null; 
                //}

                //if (QCAutomation.Formula != null ||
                //QCAutomation.Matrix != null ||
                //QCAutomation.Test != null ||
                //QCAutomation.Method != null ||
                //QCAutomation.Parameter != null)
                //{
                //    QCAutomation.Formula = null;
                //    QCAutomation.Matrix = null;
                //    QCAutomation.Test = null;
                //    QCAutomation.Method = null;
                //    QCAutomation.Parameter = null; 
                //}

                //if (SurAutomation.Formula != null ||
                //SurAutomation.Matrix != null ||
                //SurAutomation.Test != null ||
                //SurAutomation.Method != null ||
                //SurAutomation.Parameter != null)
                //{
                //    SurAutomation.Formula = null;
                //    SurAutomation.Matrix = null;
                //    SurAutomation.Test = null;
                //    SurAutomation.Method = null;
                //    SurAutomation.Parameter = null; 
                //}
                Session.CommitTransaction();
            }
        }

        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();

            #region Sample
            if (targetMemberName == "SampleMatrices" && SampleMatrixesXP != null && SampleMatrixesXP.Count > 0)
            {
                foreach (Matrix objMatrix in SampleMatrixesXP.Where(i => i.MatrixName != null).OrderBy(i => i.MatrixName).ToList())
                {
                    if (!Properties.ContainsKey(objMatrix.Oid))
                    {
                        Properties.Add(objMatrix.Oid, objMatrix.MatrixName);
                    }
                }
            }
            if (targetMemberName == "SampleTest" && SampleTestDataSource != null && SampleTestDataSource.Count > 0)
            {
                foreach (TestMethod objTest in SampleTestDataSource.Where(i => i.TestName != null && i.IsGroup==false).OrderBy(i => i.TestName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objTest.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestName);
                    }
                }
            }
            if (targetMemberName == "SampleMethod" && SampleMethodDataSource != null && SampleMethodDataSource.Count > 0)
            {
                foreach (TestMethod objMethod in SampleMethodDataSource.Where(i => i.MethodName!=null && i.MethodName.MethodNumber != null).OrderBy(i => i.MethodName.MethodNumber).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.MethodName.MethodNumber))
                    {
                        Properties.Add(objMethod.Oid, objMethod.MethodName.MethodNumber);
                    }
                }
            }
            if (targetMemberName == "SampleParameter" && SampleParameterDataSource != null && SampleParameterDataSource.Count > 0)
            {
                foreach (Testparameter objMethod in SampleParameterDataSource.Where(i => i.Parameter.ParameterName != null).OrderBy(i => i.Parameter.ParameterName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.Parameter.ParameterName))
                    {
                        Properties.Add(objMethod.Oid, objMethod.Parameter.ParameterName);
                    }
                }
            }
            #endregion Sample

            #region QC
            if (targetMemberName == "QCType" && QCTypes != null && QCTypes.Count > 0)
            {
                foreach (QCType objQCType in QCTypes.Where(i => i.QCTypeName != null).OrderBy(i => i.QCTypeName).ToList())
                {
                    if (!Properties.ContainsKey(objQCType.Oid))
                    {
                        Properties.Add(objQCType.Oid, objQCType.QCTypeName);
                    }
                }
            }

            if (targetMemberName == "QCMatrix" && QCMatrixesXP != null && QCMatrixesXP.Count > 0)
            {
                foreach (Matrix objMatrix in QCMatrixesXP.Where(i => i.MatrixName != null).OrderBy(i => i.MatrixName).ToList())
                {
                    if (!Properties.ContainsKey(objMatrix.Oid))
                    {
                        Properties.Add(objMatrix.Oid, objMatrix.MatrixName);
                    }
                }
            }
            if (targetMemberName == "QCTest" && QCTestDataSource != null && QCTestDataSource.Count > 0)
            {
                foreach (TestMethod objTest in QCTestDataSource.Where(i => i.TestName != null && i.IsGroup==false).OrderBy(i => i.TestName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objTest.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestName);
                    }
                }
            }
            if (targetMemberName == "QCMethod" && QCMethodDataSource != null && QCMethodDataSource.Count > 0)
            {
                foreach (TestMethod objMethod in QCMethodDataSource.Where(i => i.MethodName!= null && i.MethodName.MethodNumber != null).OrderBy(i => i.MethodName.MethodNumber).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.MethodName.MethodNumber))
                    {
                        Properties.Add(objMethod.Oid, objMethod.MethodName.MethodNumber);
                    }
                }
            }
            if (targetMemberName == "QCParameter" && QCParameterDataSource != null && QCParameterDataSource.Count > 0)
            {
                foreach (Testparameter objMethod in QCParameterDataSource.Where(i => i.Parameter.ParameterName != null).OrderBy(i => i.Parameter.ParameterName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.Parameter.ParameterName))
                    {
                        Properties.Add(objMethod.Oid, objMethod.Parameter.ParameterName);
                    }
                }
            }
            #endregion QC

            #region Surrogate
            if (targetMemberName == "SurMatrices" && SurMatrixesXP != null && SurMatrixesXP.Count > 0)
            {
                foreach (Matrix objMatrix in SurMatrixesXP.Where(i => i.MatrixName != null).OrderBy(i => i.MatrixName).ToList())
                {
                    if (!Properties.ContainsKey(objMatrix.Oid))
                    {
                        Properties.Add(objMatrix.Oid, objMatrix.MatrixName);
                    }
                }
            }
            if (targetMemberName == "SurTest" && SurTestDataSource != null && SurTestDataSource.Count > 0)
            {
                foreach (Testparameter objTest in SurTestDataSource.Where(i => i.TestMethod.TestName != null && i.IsGroup==false).OrderBy(i => i.TestMethod.TestName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objTest.TestMethod.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestMethod.TestName);
                    }
                }
            }
            if (targetMemberName == "SurMethod" && SurMethodDataSource != null && SurMethodDataSource.Count > 0)
            {
                foreach (Testparameter objMethod in SurMethodDataSource.Where(i => i.TestMethod !=null && i.TestMethod.MethodName!=null && i.TestMethod.MethodName.MethodNumber != null).OrderBy(i => i.TestMethod.MethodName.MethodNumber).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.TestMethod.MethodName.MethodNumber))
                    {
                        Properties.Add(objMethod.Oid, objMethod.TestMethod.MethodName.MethodNumber);
                    }
                }
            }
            if (targetMemberName == "SurParameter" && SurParameterDataSource != null && SurParameterDataSource.Count > 0)
            {
                foreach (Testparameter objMethod in SurParameterDataSource.Where(i => i.Parameter.ParameterName != null).OrderBy(i => i.Parameter.ParameterName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.Parameter.ParameterName))
                    {
                        Properties.Add(objMethod.Oid, objMethod.Parameter.ParameterName);
                    }
                }
            }
            #endregion Surrogate

            return Properties;
        }

        private QualifierAutomation _SampleAutomation;
        public QualifierAutomation SampleAutomation
        {
            get { return _SampleAutomation; }
            set { SetPropertyValue(nameof(SampleAutomation), ref _SampleAutomation, value); }
        }
        private QualifierAutomation _QCAutomation;
        public QualifierAutomation QCAutomation
        {
            get { return _QCAutomation; }
            set { SetPropertyValue(nameof(QCAutomation), ref _QCAutomation, value); }
        }
        private QualifierAutomation _SurAutomation;
        public QualifierAutomation SurAutomation
        {
            get { return _SurAutomation; }
            set { SetPropertyValue(nameof(SurAutomation), ref _SurAutomation, value); }
        }

        #region Qualifier

        private string _QualifierID;
        [ModelDefault("AllowEdit", "False")]
        public string QualifierID
        {
            get { return _QualifierID; }
            set { SetPropertyValue(nameof(QualifierID), ref _QualifierID, value); }
        }

        private string _Symbol;
        [RuleRequiredField("Symbol", DefaultContexts.Save,  "'Symbol must not be empty.'")]
        [RuleUniqueValue("Unique", DefaultContexts.Save)]
        public string Symbol
        {
            get { return _Symbol; }
            set { SetPropertyValue(nameof(Symbol), ref _Symbol, value); }
        }

        private string _Definition;
        [RuleRequiredField("Definition", DefaultContexts.Save, "'Definition must not be empty.'")]
        [Size(SizeAttribute.Unlimited)]
        public string Definition
        {
            get { return _Definition; }
            set { SetPropertyValue(nameof(Definition), ref _Definition, value); }
        }
        private DefinitionCategory _Category;
        //[RuleRequiredField("Category", DefaultContexts.Save)]
        public DefinitionCategory Category
        {
            get { return _Category; }
            set { SetPropertyValue(nameof(Category), ref _Category, value); }
        }
        private string _Comment;
        [Size(1000)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion Qualifier

        #region SampleAutomation
        private string _SampleFormula;
        //[RuleRequiredField]
        public string SampleFormula
        {
            get { return _SampleFormula; }
            set { SetPropertyValue(nameof(SampleFormula), ref _SampleFormula, value); }
        }
        private string _SampleMatrices;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string SampleMatrices
        {
            get { return _SampleMatrices; }
            set { SetPropertyValue(nameof(SampleMatrices), ref _SampleMatrices, value); }
        }
        private string _SampleTest;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string SampleTest
        {
            get
            {
                if (string.IsNullOrEmpty(SampleMatrices))
                {
                    _SampleTest = null;
                }

                return _SampleTest;
            }
            set { SetPropertyValue(nameof(SampleTest), ref _SampleTest, value); }
        }
        private string _SampleMethod;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string SampleMethod
        {
            get { return _SampleMethod; }
            set { SetPropertyValue(nameof(SampleMethod), ref _SampleMethod, value); }
        }
        private string _SampleParameter;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string SampleParameter
        {
            get { return _SampleParameter; }
            set { SetPropertyValue(nameof(SampleParameter), ref _SampleParameter, value); }
        }

        [Browsable(false)]
        public XPCollection<Matrix> SampleMatrixesXP
        {
            get
            {
                return new XPCollection<Matrix>(Session, CriteriaOperator.Parse(""));
            }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<TestMethod> SampleTestDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(SampleMatrices))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = SampleMatrices.Split(';').ToList();
                    if (lstMOid != null)
                    {
                        foreach (string objOid in lstMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Matrix objM = Session.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                if (objM != null && !lstM.Contains(objM.MatrixName))
                                {
                                    lstM.Add(objM.MatrixName);
                                }
                            }

                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("MatrixName.MatrixName", lstM));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> SampleMethodDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(SampleTest))
                {
                    List<string> lstT = new List<string>();
                    List<string> lstTOid = SampleTest.Split(';').ToList();
                    if (lstTOid != null)
                    {
                        foreach (string objOid in lstTOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                TestMethod objM = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                if (objM != null && !lstT.Contains(objM.TestName))
                                {
                                    lstT.Add(objM.TestName);
                                }
                            }

                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("TestName", lstT));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Testparameter> SampleParameterDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(SampleMethod))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = SampleMethod.Split(';').ToList();
                    if (lstMOid != null)
                    {
                        foreach (string objOid in lstMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                TestMethod objM = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                if (objM != null && !lstM.Contains(objM.MethodName.MethodNumber))
                                {
                                    lstM.Add(objM.MethodName.MethodNumber);
                                }
                            }

                        }
                    }
                    return new XPCollection<Testparameter>(Session, new InOperator("TestMethod.MethodName.MethodNumber", lstM));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion SampleAutomation

        #region QCAutomation
        private string _QCType;
        //[RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string QCType
        {
            get { return _QCType; }
            set { SetPropertyValue(nameof(QCType), ref _QCType, value); }
        }
        private string _QCFormula;
        //[RuleRequiredField]
        public string QCFormula
        {
            get { return _QCFormula; }
            set { SetPropertyValue(nameof(QCFormula), ref _QCFormula, value); }
        }

        private string _QCMatrix;
        //[RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string QCMatrix
        {
            get { return _QCMatrix; }
            set { SetPropertyValue(nameof(QCMatrix), ref _QCMatrix, value); }
        }
        private string _QCTest;
        //[RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string QCTest
        {
            get { return _QCTest; }
            set { SetPropertyValue(nameof(QCTest), ref _QCTest, value); }
        }
        private string _QCMethod;
        //[RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string QCMethod
        {
            get { return _QCMethod; }
            set { SetPropertyValue(nameof(QCMethod), ref _QCMethod, value); }
        }
        private string _QCParameter;
        //[RuleRequiredField]
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        public string QCParameter
        {
            get { return _QCParameter; }
            set { SetPropertyValue(nameof(QCParameter), ref _QCParameter, value); }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<QCType> QCTypes
        {
            get
            {
                return new XPCollection<QCType>(Session, CriteriaOperator.Parse(""));
            }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Matrix> QCMatrixesXP
        {
            get
            {
                return new XPCollection<Matrix>(Session, CriteriaOperator.Parse(""));
            }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<TestMethod> QCTestDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(QCMatrix))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = QCMatrix.Split(';').ToList();
                    if (lstMOid != null)
                    {
                        foreach (string objOid in lstMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Matrix objM = Session.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                if (objM != null && !lstM.Contains(objM.MatrixName))
                                {
                                    lstM.Add(objM.MatrixName);
                                }
                            }

                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("MatrixName.MatrixName", lstM));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<TestMethod> QCMethodDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(QCTest))
                {
                    List<string> lstT = new List<string>();
                    List<string> lstTOid = QCTest.Split(';').ToList();
                    if (lstTOid != null)
                    {
                        foreach (string objOid in lstTOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                TestMethod objM = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                if (objM != null && !lstT.Contains(objM.TestName))
                                {
                                    lstT.Add(objM.TestName);
                                }
                            }

                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("TestName", lstT));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Testparameter> QCParameterDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(QCMethod))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = QCMethod.Split(';').ToList();
                    if (lstMOid != null)
                    {
                        foreach (string objOid in lstMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                TestMethod objM = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                if (objM != null && !lstM.Contains(objM.MethodName.MethodNumber))
                                {
                                    lstM.Add(objM.MethodName.MethodNumber);
                                }
                            }

                        }
                    }
                    return new XPCollection<Testparameter>(Session, new InOperator("TestMethod.MethodName.MethodNumber", lstM));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion QCAutomation

        #region SurrogateAutomation

        private string _SurFormula;
        //[RuleRequiredField]
        public string SurFormula
        {
            get { return _SurFormula; }
            set { SetPropertyValue(nameof(SurFormula), ref _SurFormula, value); }
        }
        private string _SurMatrices;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string SurMatrices
        {
            get { return _SurMatrices; }
            set { SetPropertyValue(nameof(SurMatrices), ref _SurMatrices, value); }
        }
        private string _SurTest;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string SurTest
        {
            get
            {
                if (string.IsNullOrEmpty(SurMatrices))
                {
                    _SurTest = null;
                }

                return _SurTest;
            }
            set { SetPropertyValue(nameof(SurTest), ref _SurTest, value); }
        }
        private string _SurMethod;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string SurMethod
        {
            get { return _SurMethod; }
            set { SetPropertyValue(nameof(SurMethod), ref _SurMethod, value); }
        }
        private string _SurParameter;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        //[RuleRequiredField]
        [Size(SizeAttribute.Unlimited)]
        public string SurParameter
        {
            get { return _SurParameter; }
            set { SetPropertyValue(nameof(SurParameter), ref _SurParameter, value); }
        }
        [Browsable(false)]
        [NonPersistent]
        [ImmediatePostData]
        public XPCollection<Matrix> SurMatrixesXP
        {
            get
            {
                IList<Guid> lstTestParam = new XPCollection<Testparameter>(Session, CriteriaOperator.Parse("[Surroagate] = true")).Cast<Testparameter>().Where(i => i.TestMethod != null && i.TestMethod.MatrixName != null).Select(i => i.TestMethod.MatrixName.Oid).ToList();
                //if (objM != null && !lstT.Contains(objM.TestName))
                //{
                //    lstT.Add(objM.TestName);
                //}
                if (lstTestParam != null && lstTestParam.Count > 0)
                {
                    return new XPCollection<Matrix>(Session, CriteriaOperator.Parse("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestParam)) + ")"));
                }
                return null;
            }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Testparameter> SurTestDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(SurMatrices))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = SurMatrices.Split(';').ToList();
                    if (lstMOid != null)
                    {
                        foreach (string objOid in lstMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Matrix objM = Session.GetObjectByKey<Matrix>(new Guid(objOid.Trim()));
                                if (objM != null && !lstM.Contains(objM.MatrixName))
                                {
                                    lstM.Add(objM.MatrixName);
                                }
                            }

                        }
                    }
                    return new XPCollection<Testparameter>(Session, CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName] In(" + string.Format("'{0}'", string.Join("','", lstM)) + ") AND  [Surroagate] = true"));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Testparameter> SurMethodDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(SurTest))
                {
                    List<string> lstT = new List<string>();
                    List<string> lstTOid = SurTest.Split(';').ToList();
                    if (lstTOid != null)
                    {
                        foreach (string objOid in lstTOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Testparameter objM = Session.GetObjectByKey<Testparameter>(new Guid(objOid.Trim()));
                                if (objM != null && !lstT.Contains(objM.TestMethod.TestName))
                                {
                                    lstT.Add(objM.TestMethod.TestName);
                                }
                            }

                        }
                    }
                    return new XPCollection<Testparameter>(Session, CriteriaOperator.Parse("[TestMethod.TestName] In(" + string.Format("'{0}'", string.Join("','", lstT)) + ") AND  [Surroagate] = true"));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Testparameter> SurParameterDataSource
        {

            get
            {

                if (!string.IsNullOrEmpty(SurMethod))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = SurMethod.Split(';').ToList();
                    if (lstMOid != null)
                    {
                        foreach (string objOid in lstMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                Testparameter objM = Session.GetObjectByKey<Testparameter>(new Guid(objOid.Trim()));
                                if (objM != null && !lstM.Contains(objM.TestMethod.MethodName.MethodNumber))
                                {
                                    lstM.Add(objM.TestMethod.MethodName.MethodNumber);
                                }
                            }

                        }
                    }
                    //return new XPCollection<Testparameter>(Session, new InOperator("[TestMethod.MethodName.MethodNumber]", lstM));
                    return new XPCollection<Testparameter>(Session, CriteriaOperator.Parse("[TestMethod.MethodName.MethodNumber] In(" + string.Format("'{0}'", string.Join("','", lstM)) + ") AND  [Surroagate] = true"));
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    // return objTests;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion SurrogateAutomation

        #region OtherDetails
        private CustomSystemUser _CreatedBy;
        [Browsable(false)]
        public CustomSystemUser CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        private CustomSystemUser _ModifiedBy;
        [Browsable(false)]
        public CustomSystemUser ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }

        private DateTime _CreatedDate;

        [Browsable(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }

        private DateTime _ModifiedDate;

        [Browsable(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        #endregion OtherDetails
    }
}