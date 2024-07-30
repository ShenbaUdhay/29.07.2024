using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Qualifier
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class QualifierAutomation : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public QualifierAutomation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            CreatedDate = DateTime.Now;
            CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
        }

        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();

            if (targetMemberName == "Matrix" && MatrixesXP != null && MatrixesXP.Count > 0)
            {
                foreach (Matrix objMatrix in MatrixesXP.Where(i => i.MatrixName != null).OrderBy(i => i.MatrixName).ToList())
                {
                    if (!Properties.ContainsKey(objMatrix.Oid))
                    {
                        Properties.Add(objMatrix.Oid, objMatrix.MatrixName);
                    }
                }
            }
            if (targetMemberName == "Test" && TestDataSource != null && TestDataSource.Count > 0 && Type != TypeList.Surrogate)
            {
                foreach (TestMethod objTest in TestDataSource.Where(i => i.TestName != null).OrderBy(i => i.TestName).ToList())
                {
                    if (Type == TypeList.Surrogate)
                    {
                        bool test = true;
                    }
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objTest.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestName);
                    }
                }
            }
            if (targetMemberName == "Test" && SurTestDataSource != null && SurTestDataSource.Count > 0 && Type == TypeList.Surrogate)
            {
                foreach (Testparameter objTest in SurTestDataSource.Where(i => i.TestMethod.TestName != null && i.IsGroup == false).OrderBy(i => i.TestMethod.TestName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objTest.TestMethod.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestMethod.TestName);
                    }
                }
            }
            if (targetMemberName == "Method" && SurMethodDataSource != null && SurMethodDataSource.Count > 0 && Type == TypeList.Surrogate)
            {
                foreach (Testparameter objMethod in SurMethodDataSource.Where(i => i.TestMethod != null && i.TestMethod.MethodName != null && i.TestMethod.MethodName.MethodNumber != null).OrderBy(i => i.TestMethod.MethodName.MethodNumber).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.TestMethod.MethodName.MethodNumber))
                    {
                        Properties.Add(objMethod.Oid, objMethod.TestMethod.MethodName.MethodNumber);
                    }
                }
            }
            if (targetMemberName == "Method" && MethodDataSource != null && MethodDataSource.Count > 0 && Type != TypeList.Surrogate)
            {
                foreach (TestMethod objMethod in MethodDataSource.Where(i => i.MethodName!=null && i.MethodName.MethodNumber != null).OrderBy(i => i.MethodName.MethodNumber).ToList())
                {
                    if (Type == TypeList.Surrogate)
                    {
                        bool test = true;
                    }
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.MethodName.MethodNumber))
                    {
                        Properties.Add(objMethod.Oid, objMethod.MethodName.MethodNumber);
                    }
                }
            }
            if (targetMemberName == "Parameter" && ParameterDataSource != null && ParameterDataSource.Count > 0)
            {
                foreach (Testparameter objMethod in ParameterDataSource.Where(i => i.Parameter.ParameterName != null).OrderBy(i => i.Parameter.ParameterName).ToList())
                {
                    if (Type == TypeList.Surrogate)
                    {
                        bool test = true;
                    }
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objMethod.Parameter.ParameterName))
                    {
                        Properties.Add(objMethod.Oid, objMethod.Parameter.ParameterName);
                    }
                }
            }
            return Properties;
        }

        //private string _Symbol;
        //[RuleRequiredField("QualifierSymbol", DefaultContexts.Save), RuleUniqueValue("Unique", DefaultContexts.Save)]
        ////[ModelDefault("AllowEdit", "False")]
        //[NonPersistent]
        //public string Symbol
        //{
        //    get { return _Symbol; }
        //    set { SetPropertyValue(nameof(Symbol), ref _Symbol, value); }
        //}

        private TypeList _Type;
        //[NonPersistent]
        //[ModelDefault("AllowEdit", "False")]
        public TypeList Type
        {
            get { return _Type; }
            set { SetPropertyValue(nameof(Type), ref _Type, value); }
        }

        private string _Formula;
        //[NonPersistent]
        //[ModelDefault("AllowEdit", "False")]
        public string Formula
        {
            get { return _Formula; }
            set { SetPropertyValue(nameof(Formula), ref _Formula, value); }
        }

        private string _Matrix;
        //[NonPersistent]
        //[ModelDefault("AllowEdit", "False")]
        [ImmediatePostData]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
        }
        private string _Test;
        //[NonPersistent]
        //[ModelDefault("AllowEdit", "False")]
        [ImmediatePostData]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        private string _Method;
        //[ModelDefault("AllowEdit", "False")]
        //[NonPersistent]
        [ImmediatePostData]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }
        private string _Parameter;
        //[ModelDefault("AllowEdit", "False")]
        [ImmediatePostData]
        //[NonPersistent]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string Parameter
        {
            get { return _Parameter; }
            set { SetPropertyValue(nameof(Parameter), ref _Parameter, value); }
        }

        [Browsable(false)]
        public XPCollection<Matrix> MatrixesXP
        {
            get
            {
                return new XPCollection<Matrix>(Session, CriteriaOperator.Parse(""));
            }
        }

        private Qualifiers _SampleAuto;
        public Qualifiers SampleAuto
        {
            get { return _SampleAuto; }
            set { SetPropertyValue(nameof(SampleAuto), ref _SampleAuto, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Matrix))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = Matrix.Split(';').ToList();
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
        public XPCollection<TestMethod> MethodDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Test))
                {
                    List<string> lstT = new List<string>();
                    List<string> lstTOid = Test.Split(';').ToList();
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
        public XPCollection<Testparameter> ParameterDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Method))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = Method.Split(';').ToList();
                    if (lstMOid != null)
                    {
                        foreach (string objOid in lstMOid)
                        {
                            if (!string.IsNullOrEmpty(objOid))
                            {
                                if (Type == TypeList.Surrogate)
                                {
                                    Testparameter objM = Session.GetObjectByKey<Testparameter>(new Guid(objOid.Trim()));
                                    if (objM != null && !lstM.Contains(objM.TestMethod.MethodName.MethodNumber))
                                    {
                                        lstM.Add(objM.TestMethod.MethodName.MethodNumber);
                                    } 
                                }
                                else
                                {
                                    TestMethod objM = Session.GetObjectByKey<TestMethod>(new Guid(objOid.Trim()));
                                    if (objM != null && !lstM.Contains(objM.MethodName.MethodNumber))
                                    {
                                        lstM.Add(objM.MethodName.MethodNumber);
                                    } 
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
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Testparameter> SurTestDataSource
        {
            get
            {
                if (!string.IsNullOrEmpty(Matrix))
                {
                    List<string> lstM = new List<string>();
                    List<string> lstMOid = Matrix.Split(';').ToList();
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
                if (!string.IsNullOrEmpty(Test))
                {
                    List<string> lstT = new List<string>();
                    List<string> lstTOid = Test.Split(';').ToList();
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
        public enum TypeList
        {
            Sample,
            QC,
            Surrogate
        }

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
    }
}