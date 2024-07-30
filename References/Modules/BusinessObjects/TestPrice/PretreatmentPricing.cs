using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PretreatmentPricing : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        PricingInfo priceinfo = new PricingInfo();
        public PretreatmentPricing(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Modifiedby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            string strpricecode = string.Empty;
            string strprcode = string.Empty;
            int prcode = 0;
            string pricecode = string.Empty;
            if (Matrix != null && Test != null && PrepMethod != null)
            {
                CriteriaOperator estexpression = CriteriaOperator.Parse("Max(PriceCode)");
                CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And [Test] = ? And[PrepMethod.Method.MethodNumber] = ?", Matrix.MatrixName, Test, PrepMethod.Method.MethodNumber);
                var objtpsurcharge = Session.Evaluate<PretreatmentPricing>(estexpression, estfilter);
                if (objtpsurcharge != null)
                {
                    strpricecode = objtpsurcharge.ToString();
                    //strpricecode = strpricecode.Replace("PP", "");
                    string[] strarr = strpricecode.Trim().Split('-');
                    if (strarr != null)
                    {
                        ////strprcode = (Convert.ToInt32(strarr[1]) + 1).ToString();
                        prcode = Convert.ToInt32(strarr[1]);
                        strprcode = (prcode + 1).ToString();
                        if (!string.IsNullOrEmpty(strprcode) && strprcode.Length == 1)
                        {
                            strprcode = "0" + strprcode;
                        }
                        PriceCode = /*"PP" +*/ strarr[0] + "-" + strprcode;
                    }
                }
                else
                {
                    CriteriaOperator expression = CriteriaOperator.Parse("Max(PriceCode)");
                    CriteriaOperator filter = CriteriaOperator.Parse("Not IsNullOrEmpty([PriceCode])");
                    ////var max = Session.Evaluate<PretreatmentPricing>(expression, filter);
                    var max = Session.Evaluate<TestPriceCode>(expression, filter);
                    if (max != null)
                    {
                        strprcode = max.ToString();
                        //strprcode = strprcode.Replace("PP", "");
                        if (!string.IsNullOrEmpty(strprcode))
                        {
                            string[] strarr = strprcode.Split('-');
                            if (strarr != null)
                            {
                                prcode = Convert.ToInt32(strarr[0]);
                                strprcode = (prcode + 1).ToString();
                                ////strprcode = (Convert.ToInt32(strarr[0].Trim()) + 1).ToString();
                                PriceCode = /*"PP" + */strprcode + "-01";
                            }
                        }
                    }
                    else
                    {
                        PriceCode = "100-01";
                    }
                }
                priceinfo.strPPpricecode = PriceCode;
            }
            Createdby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            Modifiedby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            ModifiedDate = Library.GetServerTime(Session);
        }

        #region PriceCode
        private string _PriceCode;
        public string PriceCode
        {
            get { return _PriceCode; }
            set { SetPropertyValue("PriceCode", ref _PriceCode, value); }
        }
        #endregion

        #region Matrix
        private Matrix _Matrix;
        [ImmediatePostData]
        [RuleRequiredField("MatrixPP", DefaultContexts.Save)]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue("Matrix", ref _Matrix, value); }
        }
        #endregion

        #region PrepMethod
        private PrepMethod _PrepMethod;
        [ImmediatePostData]
        [XafDisplayName("Method")]
        [RuleRequiredField("PrepMethodPP", DefaultContexts.Save)]
        [DataSourceProperty(nameof(PrepMethodDataSource))]
        public PrepMethod PrepMethod
        {
            get { return _PrepMethod; }
            set { SetPropertyValue("PrepMethod", ref _PrepMethod, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<PrepMethod> PrepMethodDataSource
        {
            get
            {
                List<Guid> lst = new List<Guid>();
                List<object> groups = new List<object>();
                if (PrepMethod == null && Matrix != null)
                {
                    XPCollection<PrepMethod> lstpm = new XPCollection<PrepMethod>(Session);
                    lstpm.Criteria = CriteriaOperator.Parse("[TestMethod.MatrixName.Oid] = ? And [TestMethod.GCRecord] is Null", Matrix.Oid);
                    if (lstpm != null && lstpm.Count > 0)
                    {
                        foreach (PrepMethod objpp in lstpm.ToList())
                        {
                            if (!lst.Contains(objpp.Method.Oid))
                            {
                                lst.Add(objpp.Method.Oid);
                                groups.Add(objpp.Oid);
                            }
                        }
                    }
                    if (groups.Count > 0)
                    {
                        XPCollection<PrepMethod> testpp = new XPCollection<PrepMethod>(Session, new InOperator("Oid", groups));
                        return testpp;
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
        #endregion

        #region Test
        private string _Test;
        [ImmediatePostData]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        [RuleRequiredField("TestPretretPri", DefaultContexts.Save)]
        public string Test
        {
            get { return _Test; }
            set { SetPropertyValue("Test", ref _Test, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<TestMethod> TestDataSource
        {
            get
            {
                if (Matrix != null && Matrix.MatrixName != null && PrepMethod != null && PrepMethod.Method != null && PrepMethod.Method.MethodNumber != null)
                {
                    ////List<string> strmatrix = new List<string>();
                    ////strmatrix.Add(Matrix.MatrixName);
                    ////return new XPCollection<TestMethod>(Session, new InOperator("MatrixName.MatrixName", strmatrix));
                    //////return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", lstSM));
                    ////// return objTests;
                    List<Guid> testoid = new List<Guid>();
                    XPCollection<PrepMethod> lstpp = new XPCollection<PrepMethod>(Session);
                    lstpp.Criteria = CriteriaOperator.Parse("[Method.Oid] = ? And [TestMethod.MatrixName.Oid] = ?", PrepMethod.Method.Oid, Matrix.Oid);
                    foreach (PrepMethod objpp in lstpp.ToList())
                    {
                        if (!testoid.Contains(objpp.TestMethod.Oid))
                        {
                            testoid.Add(objpp.TestMethod.Oid);
                        }
                    }
                    return new XPCollection<TestMethod>(Session, new InOperator("Oid", testoid));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "Test" && TestDataSource != null && TestDataSource.Count > 0)
            {
                foreach (TestMethod objTest in TestDataSource.Where(i => i.TestName != null).OrderBy(i => i.TestName).ToList())
                {
                    //if (!Properties.ContainsKey(objTest.Oid))
                    if (!Properties.ContainsValue(objTest.TestName))
                    {
                        Properties.Add(objTest.Oid, objTest.TestName);
                    }
                }
            }
            return Properties;
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

        //#region Method
        //private TestMethod _Method;
        //[ImmediatePostData]
        //[RuleRequiredField("MethodPP", DefaultContexts.Save)]
        //[DataSourceProperty(nameof(MethodDataSource))]
        //public TestMethod Method
        //{
        //    get { return _Method; }
        //    set { SetPropertyValue("Method", ref _Method, value); }
        //}

        //[Browsable(false)]
        //[ImmediatePostData]
        //[NonPersistent]
        //public XPCollection<TestMethod> MethodDataSource
        //{
        //    get
        //    {
        //        string[] strtestarr = null;
        //        List<string> listtempmed = new List<string>();
        //        List<string> lsttest = new List<string>();
        //        List<object> groups = new List<object>();
        //        if (Test != null && Method == null && Matrix != null)
        //        {
        //            strtestarr = Test.Split(';');
        //            foreach (string objtest in strtestarr.ToList())
        //            {
        //                if(!string.IsNullOrEmpty(objtest))
        //                {
        //                    TestMethod objtm = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[Oid] =? And [MatrixName.MatrixName]=?", new Guid(objtest.Trim()), Matrix.MatrixName));
        //                    if (objtm != null && objtm.TestName != null)
        //                    {
        //                        lsttest.Add(objtm.TestName);
        //                    }
        //                }                        
        //            }
        //            foreach (string objtestname in lsttest.ToList())
        //            {
        //                TestMethod objtm = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[TestName] =? And [MatrixName.MatrixName]=?", objtestname.Trim(), Matrix.MatrixName));
        //                if (objtm != null && objtm.MethodName != null && objtm.MethodName.MethodNumber != null && !listtempmed.Contains(objtm.MethodName.MethodNumber))
        //                {
        //                    groups.Add(objtm.Oid);
        //                    listtempmed.Add(objtm.MethodName.MethodNumber);
        //                }
        //            }
        //            if (groups.Count > 0)
        //            {
        //                XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
        //                return tests;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //            //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=?", Test, Matrix.MatrixName));
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        //#endregion

        #region UnitPrice
        private decimal _UnitPrice;
        [ImmediatePostData]
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { SetPropertyValue("UnitPrice", ref _UnitPrice, value); }
        }
        #endregion

        #region Createdby
        private CustomSystemUser _Createdby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser Createdby
        {
            get { return _Createdby; }
            set { SetPropertyValue("Createdby", ref _Createdby, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }
        #endregion

        #region Modifiedby
        private CustomSystemUser _Modifiedby;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public CustomSystemUser Modifiedby
        {
            get { return _Modifiedby; }
            set { SetPropertyValue("Modifiedby", ref _Modifiedby, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue("ModifiedDate", ref _ModifiedDate, value); }
        }
        #endregion
    }
}