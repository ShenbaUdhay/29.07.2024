using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.SuboutTracking
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class CertifiedTests : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        Contractlabinfo contlabinfo = new Contractlabinfo();
        public CertifiedTests(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        #region Matrix
        private Matrix _Matrix;
        [ImmediatePostData]
        [RuleRequiredField]
        [DataSourceProperty(nameof(MatrixDataSource))]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
        }
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Matrix> MatrixDataSource
        {
            get
            {
                if (contlabinfo.listMatrix != null && contlabinfo.listMatrix.Count > 0)
                {
                    List<Guid> lsttmatrixoid = new List<Guid>();
                    List<string> lstmatrixname = new List<string>();
                    foreach (Matrix objmat in contlabinfo.listMatrix.ToList())
                    {
                        if (!lstmatrixname.Contains(objmat.MatrixName))
                        {
                            lstmatrixname.Add(objmat.MatrixName);
                            lsttmatrixoid.Add(objmat.Oid);
                        }
                    }
                    XPCollection<Matrix> lstmatr = new XPCollection<Matrix>(Session, new InOperator("Oid", lsttmatrixoid));
                    return lstmatr;
                }
                else
                {
                    XPCollection<Matrix> lstmatr = new XPCollection<Matrix>(Session, CriteriaOperator.Parse(""));
                    return lstmatr;
                }
            }
        }
        #endregion

        #region Test
        private TestMethod _Test;
        [ImmediatePostData]
        [RuleRequiredField]
        [DataSourceProperty(nameof(TestDataSource))]
        public TestMethod Test
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
                if (Matrix != null)
                {
                    List<string> lsttestname = new List<string>();
                    List<Guid> lsttmoid = new List<Guid>();
                    if (contlabinfo.listTestMethod != null && contlabinfo.listTestMethod.Count > 0)
                    {
                        foreach (TestMethod objtm in contlabinfo.listTestMethod.ToList())
                        {
                            if (!lsttestname.Contains(objtm.TestName))
                            {
                                lsttestname.Add(objtm.TestName);
                                lsttmoid.Add(objtm.Oid);
                            }
                        }
                        XPCollection<TestMethod> lsttestpara = new XPCollection<TestMethod>(Session, new InOperator("Oid", lsttmoid));//CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = False", Matrix.MatrixName));
                        return lsttestpara;
                    }
                    else
                    {
                        XPCollection<TestMethod> lsttestpara = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = False", Matrix.MatrixName));
                        foreach (TestMethod objtm in lsttestpara.ToList())
                        {
                            if (!lsttestname.Contains(objtm.TestName))
                            {
                                lsttestname.Add(objtm.TestName);
                                lsttmoid.Add(objtm.Oid);
                            }
                        }
                        XPCollection<TestMethod> lsttestmed = new XPCollection<TestMethod>(Session, new InOperator("Oid", lsttmoid));//CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = False", Matrix.MatrixName));
                        return lsttestpara;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Method
        private TestMethod _Method;
        [ImmediatePostData]
        [RuleRequiredField]
        [DataSourceProperty(nameof(MethodDataSource))]
        public TestMethod Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<TestMethod> MethodDataSource
        {
            get
            {
                if (Test != null && Matrix != null)
                {
                    List<TestMethod> lstmed = new List<TestMethod>();
                    List<Guid> lstmedoid = new List<Guid>();
                    if (contlabinfo.listMethod != null && contlabinfo.listMethod.Count > 0)
                    {
                        XPCollection<TestMethod> lsttestmed = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]= ? And [TestName] = ?", Matrix.MatrixName, Test.TestName));
                        foreach (TestMethod objtm in lsttestmed.ToList())
                        {
                            if (!lstmed.Contains(objtm))
                            {
                                lstmed.Add(objtm);
                            }
                        }
                        foreach (TestMethod objtm in contlabinfo.listMethod.ToList())
                        {
                            if (lstmed.Contains(objtm))
                            {
                                if (objtm != null)
                                {
                                    lstmedoid.Add(objtm.Oid);
                                }
                            }
                        }
                        XPCollection<TestMethod> lsttestpara = new XPCollection<TestMethod>(Session, new InOperator("Oid", lstmedoid));//CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = False", Matrix.MatrixName));
                        return lsttestpara;
                    }
                    else
                    {
                        XPCollection<TestMethod> lsttestmed = new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]= ? And [TestName] = ?", Matrix.MatrixName, Test.TestName));
                        foreach (TestMethod objtm in lsttestmed.ToList())
                        {
                            if (!lstmed.Contains(objtm))
                            {
                                lstmed.Add(objtm);
                            }
                        }
                        foreach (TestMethod objtm in lstmed.ToList())
                        {
                            if (lstmed.Contains(objtm))
                            {
                                if (objtm != null)
                                {
                                    lstmedoid.Add(objtm.Oid);
                                }
                            }
                        }
                        XPCollection<TestMethod> lsttestpara = new XPCollection<TestMethod>(Session, new InOperator("Oid", lstmedoid));//CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [IsGroup] = False", Matrix.MatrixName));
                        return lsttestpara;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region TAT
        private TurnAroundTime _TAT;
        [ImmediatePostData]
        public TurnAroundTime TAT
        {
            get { return _TAT; }
            set { SetPropertyValue(nameof(TAT), ref _TAT, value); }
        }
        #endregion

        #region UnitPrice
        private decimal _UnitPrice;
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref _UnitPrice, value); }
        }
        #endregion

        #region Comment
        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion

        #region SubOutContractLab
        private SubOutContractLab _SubOutContractLab;
        [Association("SubOutContractLab-CertifiedTests")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public SubOutContractLab SubOutContractLab
        {
            get { return _SubOutContractLab; }
            set { SetPropertyValue(nameof(SubOutContractLab), ref _SubOutContractLab, value); }
        }
        #endregion

    }
}