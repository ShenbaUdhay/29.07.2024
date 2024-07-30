using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Component = Modules.BusinessObjects.Setting.Component;
using Matrix = Modules.BusinessObjects.Setting.Matrix;
using Method = Modules.BusinessObjects.Setting.Method;

namespace Modules.BusinessObjects.TestPricing
{
    [DefaultClassOptions]
    //GroupTestListView
    [Appearance("ShowTestmethodCP", AppearanceItemType = "LayoutItem",
 TargetItems = "Item2", Criteria = "[IsGroup] = True",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestmethodCP", AppearanceItemType = "LayoutItem",
 TargetItems = "Item2", Criteria = "[IsGroup] = False",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    //ParameterListView
    [Appearance("ShowParameterCP", AppearanceItemType = "LayoutItem",
 TargetItems = "Item4", Criteria = "ChargeType = 'Parameter'",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideParameterCP", AppearanceItemType = "LayoutItem",
 TargetItems = "Item4", Criteria = "ChargeType <> 'Parameter'",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    //   [Appearance("ShowParameterCP", AppearanceItemType = "LayoutItem",
    //TargetItems = "Item4", Criteria = "[IsGroup] = False And [Component] Is Not Null",
    //Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    //   [Appearance("HideParameterCP", AppearanceItemType = "LayoutItem",
    //TargetItems = "Item4", Criteria = "[IsGroup] = True Or [Component] Is Null",
    //Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCombinationOfPropertiesIsUnique("CPUnique", DefaultContexts.Save, targetProperties: "Matrix,Test,Method,Component", CustomMessageTemplate = "A note with the same data already exists")]
    public class ConstituentPricing : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        PricingInfo priceinfo = new PricingInfo();
        public ConstituentPricing(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(PriceCode))
            {
                string strpricecode = string.Empty;
                string strprcode = string.Empty;
                int prcode = 0;
                string pricecode = string.Empty;
                if (Matrix != null && Test != null && Method != null && Component != null && IsGroup != true)
                {
                    CriteriaOperator estexpression = CriteriaOperator.Parse("Max(PriceCode)");
                    CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodNumber] = ? ", Matrix.MatrixName, Test.TestName, Method.MethodNumber); // And[Component.Components] = ? , Component.Components
                    var objtpsurcharge = Session.Evaluate<ConstituentPricing>(estexpression, estfilter);
                    var objdefprice = Session.Evaluate<DefaultPricing>(estexpression, estfilter);
                    if (objtpsurcharge != null)
                    {
                        if (objtpsurcharge != null)
                        {
                            strpricecode = objtpsurcharge.ToString();
                        }
                        else if (objdefprice != null)
                        {
                            strpricecode = objdefprice.ToString();
                        }
                        string[] strarr = strpricecode.Split('-');
                        if (strarr != null)
                        {
                            prcode = Convert.ToInt32(strarr[1]);
                            strprcode = (prcode + 1).ToString();

                            if (!string.IsNullOrEmpty(strprcode) && strprcode.Length == 1)
                            {
                                strprcode = "0" + strprcode;
                            }

                            PriceCode = strarr[0] + "-" + strprcode;
                        }
                    }
                    else
                    {
                        CriteriaOperator expression = CriteriaOperator.Parse("Max(PriceCode)");
                        CriteriaOperator filter = CriteriaOperator.Parse("Not IsNullOrEmpty([PriceCode])");
                        //var max = Session.Evaluate<ConstituentPricing>(expression, filter);
                        var max = Session.Evaluate<TestPriceCode>(expression, filter);
                        if (max != null)
                        {
                            strprcode = max.ToString();
                            if (!string.IsNullOrEmpty(strprcode))
                            {
                                string[] strarr = strprcode.Split('-');
                                //int strarr = Convert.ToInt32(strprcode.Substring(2, 3));
                                if (strarr != null)
                                {
                                    ////strprcode = (Convert.ToInt32(strarr[0] + 1)).ToString();
                                    prcode = Convert.ToInt32(strarr[0]);
                                    strprcode = (prcode + 1).ToString();
                                    PriceCode =/* "CP" +*/ strprcode + "-01";
                                }
                            }
                        }
                        else
                        {
                            PriceCode = "100-01";
                        }
                    }
                    priceinfo.strCPpricecode = PriceCode;
                }
                else if (Matrix != null && Test != null && IsGroup == true)
                {
                    CriteriaOperator estexpression = CriteriaOperator.Parse("Max(PriceCode)");
                    CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ?", Matrix.MatrixName, Test.TestName);
                    var objtpsurcharge = Session.Evaluate<ConstituentPricing>(estexpression, estfilter);
                    if (objtpsurcharge != null)
                    {
                        strpricecode = objtpsurcharge.ToString();
                        string[] strarr = strpricecode.Split('-');
                        if (strarr != null)
                        {
                            prcode = Convert.ToInt32(strarr[1]);
                            strprcode = (prcode + 1).ToString();

                            if (!string.IsNullOrEmpty(strprcode) && strprcode.Length == 1)
                            {
                                strprcode = "0" + strprcode;
                            }

                            PriceCode = strarr[0] + "-" + strprcode;
                        }
                    }
                    else
                    {
                        CriteriaOperator expression = CriteriaOperator.Parse("Max(PriceCode)");
                        CriteriaOperator filter = CriteriaOperator.Parse("Not IsNullOrEmpty([PriceCode])");
                        //var max = Session.Evaluate<ConstituentPricing>(expression, filter);
                        var max = Session.Evaluate<TestPriceCode>(expression, filter);
                        if (max != null)
                        {
                            strprcode = max.ToString();
                            if (!string.IsNullOrEmpty(strprcode))
                            {
                                string[] strarr = strprcode.Split('-');
                                //int strarr = Convert.ToInt32(strprcode.Substring(2, 3));
                                if (strarr != null)
                                {
                                    ////strprcode = (Convert.ToInt32(strarr[0] + 1)).ToString();
                                    prcode = Convert.ToInt32(strarr[0]);
                                    strprcode = (prcode + 1).ToString();
                                    PriceCode =/* "CP" +*/ strprcode + "-01";
                                }
                            }
                        }
                        else
                        {
                            PriceCode = "100-01";
                        }
                    }
                    priceinfo.strCPpricecode = PriceCode;
                }
            }

            Createdby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            Modifiedby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            ModifiedDate = Library.GetServerTime(Session);
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
        #region PriceCode
        private string _PriceCode;
        //[RuleRequiredField]
        public string PriceCode
        {
            get { return _PriceCode; }
            set { SetPropertyValue(nameof(PriceCode), ref _PriceCode, value); }
        }
        #endregion

        #region Matrix
        private Matrix _Matrix;
        [ImmediatePostData]
        [RuleRequiredField]
        //[RuleUniqueValue]
        // [DataSourceProperty(nameof(SampleMatrixes))]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
        }
        #endregion

        #region Test
        private TestMethod _Test;
        [ImmediatePostData]
        [RuleRequiredField]
        //[RuleUniqueValue]
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
                    XPCollection<Testparameter> lsttestpara = new XPCollection<Testparameter>(Session, CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName]=?", Matrix.MatrixName));
                    List<Guid> lstTestparaOid = new List<Guid>();
                    List<string> lsttempname = new List<string>();
                    List<string> lststrtestpara = new List<string>();
                    foreach (Testparameter objOid in lsttestpara)
                    {
                        if (objOid.TestMethod != null && objOid.TestMethod.MatrixName != null && objOid.TestMethod.MethodName != null && objOid.Component != null)
                        {
                            string strtemp = objOid.TestMethod.TestName.ToString() + "|" + objOid.TestMethod.MethodName.MethodNumber.ToString() + "|" + objOid.Component.Components.ToString();
                            if (!lstTestparaOid.Contains(objOid.Oid) && !lststrtestpara.Contains(strtemp))
                            {
                                lststrtestpara.Add(strtemp);
                                ConstituentPricing objconpri = Session.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName]=? And [Test.Oid] = ? And [Method.Oid] = ? And [Component.Oid] = ? And [IsGroup] = False", objOid.TestMethod.MatrixName.MatrixName, objOid.TestMethod.Oid, objOid.TestMethod.MethodName.Oid, objOid.Component.Oid));
                                DefaultPricing objdefpri = Session.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName]=? And [Test.Oid] = ? And [Method.Oid] = ? And [Component.Oid] = ? And [IsGroup] = 'No'", objOid.TestMethod.MatrixName.MatrixName, objOid.TestMethod.Oid, objOid.TestMethod.MethodName.Oid, objOid.Component.Oid));
                                if (objconpri == null && objdefpri == null)
                                {
                                    if (!lstTestparaOid.Contains(objOid.TestMethod.Oid) && !lsttempname.Contains(objOid.TestMethod.TestName))
                                    {
                                        lsttempname.Add(objOid.TestMethod.TestName);
                                        lstTestparaOid.Add(objOid.TestMethod.Oid);
                                    }
                                }
                            }
                        }
                        //if (objOid.TestMethod != null && objOid.IsGroup == true)
                        //{
                        //    ConstituentPricing objconpri = Session.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = True", objOid.TestMethod.Oid));
                        //    DefaultPricing objdefpri = Session.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Test.Oid] = ? And [IsGroup] = 'Yes'", objOid.TestMethod.Oid));
                        //    if (objconpri == null && objdefpri == null)
                        //    {
                        //        if (!lstTestparaOid.Contains(objOid.TestMethod.Oid) && !lsttempname.Contains(objOid.TestMethod.TestName))
                        //        {
                        //            lsttempname.Add(objOid.TestMethod.TestName);
                        //            lstTestparaOid.Add(objOid.TestMethod.Oid);
                        //        }
                        //    }
                        //}
                    }
                    //return new XPCollection<TestMethod>(Session, CriteriaOperator.Parse("[MatrixName.MatrixName]=?",Matrix.MatrixName.MatrixName));
                    ////XPCollection<TestMethod> lstTest = new XPCollection<TestMethod>(Session);
                    ////lstTest.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName]=?", Matrix.MatrixName);
                    ////lstTest.OrderBy(i => i.TestName);
                    ////List<Guid> lstTestOid = new List<Guid>();
                    ////List<string> lstSM = new List<string>();
                    ////foreach (TestMethod objOid in lstTest)
                    ////{
                    ////    if (!lstSM.Contains(objOid.TestName))
                    ////    {
                    ////        lstSM.Add(objOid.TestName);
                    ////        lstTestOid.Add(objOid.Oid);
                    ////    }
                    ////}
                    //xpCollection.Sorting.Add(new SortProperty(nameof(User.Oid), DevExpress.Xpo.DB.SortingDirection.Ascending));
                    XPCollection<TestMethod> lstTests = new XPCollection<TestMethod>(Session, new InOperator("Oid", lstTestparaOid));
                    lstTests.OrderBy(i => i.TestName);
                    return lstTests;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region IsGroup
        private bool _IsGroup;
        //[ImmediatePostData]
        [Appearance("IsGroupCPShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = True", Context = "DetailView")]
        [Appearance("IsGroupCPHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = False", Context = "DetailView")]
        public bool IsGroup
        {
            get { return _IsGroup; }
            set { SetPropertyValue(nameof(IsGroup), ref _IsGroup, value); }
        }
        #endregion

        #region Method
        private Method _Method;
        [ImmediatePostData]
        [DataSourceProperty(nameof(MethodDataSource))]
        [Appearance("MethodCPHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = True", Context = "DetailView")]
        [Appearance("MethodCPShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = False", Context = "DetailView")]
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
                if (Test != null && Matrix != null)
                {

                    XPCollection<Testparameter> lsttestpara = new XPCollection<Testparameter>(Session, CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName]=? And [TestMethod.TestName] = ?", Matrix.MatrixName, Test.TestName));
                    List<Guid> lstTestparaOid = new List<Guid>();
                    List<string> lststrtestpara = new List<string>();
                    foreach (Testparameter objOid in lsttestpara)
                    {
                        if (objOid.TestMethod != null && objOid.TestMethod.MatrixName != null && objOid.TestMethod.MethodName != null && objOid.Component != null)
                        {
                            string strtemp = objOid.TestMethod.MatrixName.MatrixName + objOid.TestMethod.TestName.ToString() + objOid.TestMethod.MethodName.Oid.ToString() + objOid.Component.Components.ToString();
                            if (!lstTestparaOid.Contains(objOid.Oid) && !lststrtestpara.Contains(strtemp))
                            {
                                lststrtestpara.Add(strtemp);
                                ConstituentPricing objconpri = Session.FindObject<ConstituentPricing>(CriteriaOperator.Parse("[Matrix.MatrixName]=? And [Test.TestName] = ? And [Method.MethodNumber] = ? And [Component.Components] = ?", objOid.TestMethod.MatrixName.MatrixName, objOid.TestMethod.TestName, objOid.TestMethod.MethodName.MethodNumber, objOid.Component.Components));
                                DefaultPricing objdefpri = Session.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix.MatrixName]=? And [Test.Oid] = ? And [Method.Oid] = ? And [Component.Oid] = ? And [IsGroup] = 'No' ", objOid.TestMethod.MatrixName.MatrixName, objOid.TestMethod.Oid, objOid.TestMethod.MethodName.Oid, objOid.Component.Oid));
                                if (objconpri == null && objdefpri == null)
                                {
                                    lstTestparaOid.Add(objOid.TestMethod.Oid);
                                }
                            }
                        }
                    }



                    XPCollection<TestMethod> lstTest = new XPCollection<TestMethod>(Session, new InOperator("Oid", lstTestparaOid));
                    //lstTest.Criteria = CriteriaOperator.Parse("[TestName] =? and [MatrixName.MatrixName]=?", Test.TestName, Matrix.MatrixName);
                    lstTest.OrderBy(i => i.MethodName.MethodNumber);
                    List<Guid> lstTestOid = new List<Guid>();
                    List<string> lstSM = new List<string>();
                    foreach (TestMethod objOid in lstTest)
                    {
                        if (objOid.IsGroup == false)
                        {
                            if (!lstSM.Contains(objOid.MethodName.MethodNumber))
                            {
                                lstSM.Add(objOid.MethodName.MethodNumber);
                                lstTestOid.Add(objOid.MethodName.Oid);
                            }
                        }
                    }
                    if (lstTestOid.Count > 0)
                    {
                        return new XPCollection<Method>(Session, new InOperator("Oid", lstTestOid));
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

        #region Component
        private Component _Component;
        //[RuleUniqueValue]
        [Appearance("ComponentCPShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = False", Context = "DetailView")]
        [Appearance("ComponentCPHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = True", Context = "DetailView")]
        [DataSourceProperty(nameof(ComponentDataSource))]
        [ImmediatePostData]
        public Component Component
        {
            get { return _Component; }
            set { SetPropertyValue(nameof(Component), ref _Component, value); }
        }
        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<Component> ComponentDataSource
        {
            get
            {
                if (Matrix != null && Test != null && Method != null)
                {
                    List<object> groups = new List<object>();
                    using (XPView lstview = new XPView(Session, typeof(Component)))
                    {
                        TestMethod objtm = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName]=? And [MethodName.MethodNumber]=?",
                        Matrix.MatrixName, Test.TestName, Method.MethodNumber));
                        if (objtm != null)
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[TestMethod.Oid]=? And [Components] <> 'null'", objtm.Oid);
                            lstview.Properties.Add(new ViewProperty("TPComponent", DevExpress.Xpo.SortDirection.Ascending, "Components", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                            foreach (ViewRecord rec in lstview)
                            {
                                groups.Add(rec["Toid"]);
                            }
                            Component objcomp = Session.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                            if (objcomp != null)
                            {
                                groups.Add(objcomp.Oid);
                            }
                        }
                    }
                    //if (groups.Count == 0)
                    //{
                    //    Component objcomp = Session.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                    //    if(objcomp != null)
                    //    {
                    //        groups.Add(objcomp.Oid);
                    //    }
                    //}
                    if (groups.Count > 0)
                    {
                        foreach (var objoid in groups.ToList())
                        {
                            if (Test != null && Matrix != null && Method != null)
                            {
                                string strgrpoid = objoid.ToString();
                                Testparameter objtstpara = Session.FindObject<Testparameter>(CriteriaOperator.Parse("[TestMethod.MatrixName.MatrixName]=? And [TestMethod.TestName]=? And [TestMethod.MethodName.MethodNumber]=?  And [Component.Oid] = ?", Matrix.MatrixName, Test.TestName, Method.MethodNumber, new Guid(strgrpoid)));
                                if (objtstpara == null)
                                {
                                    groups.Remove(objoid);
                                    continue;
                                }
                                Component objtp = Session.FindObject<Component>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strgrpoid)));
                                if (objtp != null)
                                {
                                    DefaultPricing objdp = Session.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix] = ? And [Test] =? and [Method]=? And [Component] = ?", Matrix, Test, Method, objtp.Oid));
                                    if (objdp != null)
                                    {
                                        groups.Remove(objtp.Oid);
                                    }
                                }
                                XPCollection<ConstituentPricing> lstconstprice = new XPCollection<ConstituentPricing>(Session, CriteriaOperator.Parse(""));
                                foreach (ConstituentPricing objconprice in lstconstprice.ToList())
                                {
                                    if (objconprice.Component != null && objconprice.Component.Oid == new Guid(strgrpoid) && objconprice.Test != null && objconprice.Test.TestName == Test.TestName && objconprice.Matrix != null && objconprice.Matrix.MatrixName == Matrix.MatrixName && objconprice.Method != null && objconprice.Method.MethodNumber == Method.MethodNumber)
                                    {
                                        groups.Remove(objtp.Oid);
                                    }
                                }
                            }
                        }
                    }
                    if (groups.Count > 0)
                    {
                        XPCollection<Component> tests = new XPCollection<Component>(Session, new InOperator("Oid", groups));
                        return tests;
                    }
                    else
                    {
                        return null;
                    }
                }
                if (Matrix != null && Test != null && Method == null)
                {
                    List<object> groups = new List<object>();
                    using (XPView lstview = new XPView(Session, typeof(Component)))
                    {
                        TestMethod objtm = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName]=? And [TestName]=? And [IsGroup]= True",
                        Matrix.MatrixName, Test.TestName));
                        if (objtm != null)
                        {
                            lstview.Criteria = CriteriaOperator.Parse("[TestMethod.Oid]=? And [Components] <> 'null'", objtm.Oid);
                            lstview.Properties.Add(new ViewProperty("TPComponent", DevExpress.Xpo.SortDirection.Ascending, "Components", true, true));
                            lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                            foreach (ViewRecord rec in lstview)
                            {
                                groups.Add(rec["Toid"]);
                            }
                            Component objcomp = Session.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                            if (objcomp != null)
                            {
                                groups.Add(objcomp.Oid);
                            }
                        }
                    }
                    //if (groups.Count == 0)
                    //{
                    //    Component objcomp = Session.FindObject<Component>(CriteriaOperator.Parse("[Components] = 'Default'"));
                    //    if (objcomp != null)
                    //    {
                    //        groups.Add(objcomp.Oid);
                    //    }
                    //}
                    if (groups.Count > 0)
                    {
                        foreach (var objoid in groups.ToList())
                        {
                            string strgrpoid = objoid.ToString();
                            Component objtp = Session.FindObject<Component>(CriteriaOperator.Parse("[Oid] = ?", new Guid(strgrpoid)));
                            if (objtp != null)
                            {
                                DefaultPricing objdp = Session.FindObject<DefaultPricing>(CriteriaOperator.Parse("[Matrix] = ? And [Test] =? and [Component.Oid] = ? and [IsGroup] = True", Matrix, Test, objtp.Oid));
                                if (objdp != null)
                                {
                                    groups.Remove(objtp.Oid);
                                }
                                XPCollection<ConstituentPricing> lstconstprice = new XPCollection<ConstituentPricing>(Session, CriteriaOperator.Parse(""));
                                foreach (ConstituentPricing objconprice in lstconstprice.ToList())
                                {
                                    if (objconprice.Component != null && objconprice.Component.Oid == new Guid(strgrpoid) && objconprice.Test != null && objconprice.Test == Test && objconprice.Matrix != null && objconprice.Matrix == Matrix)
                                    {
                                        groups.Remove(objtp.Oid);
                                    }
                                }
                            }
                        }
                    }
                    if (groups.Count > 0)
                    {
                        XPCollection<Component> tests = new XPCollection<Component>(Session, new InOperator("Oid", groups));
                        return tests;
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

        #region ChargeType
        private ChargeType _ChargeType;
        [ImmediatePostData]
        public ChargeType ChargeType
        {
            get { return _ChargeType; }
            set { SetPropertyValue(nameof(ChargeType), ref _ChargeType, value); }
        }
        #endregion

        //// #region TierNo
        //// private uint _TierNo;
        ////// [RuleRequiredField]
        ////// [RuleUniqueValue]
        //// public uint TierNo
        //// {
        ////     get { return _TierNo; }
        ////     set { SetPropertyValue(nameof(TierNo), ref _TierNo, value); }
        //// }
        //// #endregion

        //// #region From
        //// private uint _From;
        ////// [RuleRequiredField]
        //// //[RuleUniqueValue]
        //// public uint From
        //// {
        ////     get { return _From; }
        ////     set { SetPropertyValue(nameof(From), ref _From, value); }
        //// }
        //// #endregion

        //// #region To
        //// private uint _To;
        //// //[RuleRequiredField]
        //// //[RuleUniqueValue]
        //// public uint To
        //// {
        ////     get { return _To; }
        ////     set { SetPropertyValue(nameof(To), ref _To, value); }
        //// }
        //// #endregion

        //// #region TierPrice
        //// private double _TierPrice;
        ////// [RuleRequiredField]
        //// [ImmediatePostData]
        //// public double TierPrice
        //// {
        ////     get { return _TierPrice; }
        ////     set { SetPropertyValue(nameof(TierPrice), ref _TierPrice, value); }
        //// }
        //// #endregion

        //// #region Prep1Charge
        //// private double _Prep1Charge;
        //// [ImmediatePostData]
        //// public double Prep1Charge
        //// {
        ////     get { return _Prep1Charge; }
        ////     set { SetPropertyValue(nameof(Prep1Charge), ref _Prep1Charge, value); }
        //// }
        //// #endregion

        //// #region Prep2Charge
        //// private double _Prep2Charge;
        //// [ImmediatePostData]
        //// public double Prep2Charge
        //// {
        ////     get { return _Prep2Charge; }
        ////     set { SetPropertyValue(nameof(Prep2Charge), ref _Prep2Charge, value); }
        //// }
        //// #endregion

        //// #region TotalTierPrice
        //// private double _TotalTierPrice;
        //// [ImmediatePostData]
        //// public double TotalTierPrice
        //// {
        ////     get
        ////     {
        ////         _TotalTierPrice = TierPrice + Prep1Charge + Prep2Charge;
        ////         return _TotalTierPrice;
        ////     }
        ////     set { SetPropertyValue(nameof(TotalTierPrice), ref _TotalTierPrice, value); }
        //// }
        //// #endregion

        #region Remark
        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        #endregion

        #region Createdby
        private CustomSystemUser _Createdby;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser Createdby
        {
            get { return _Createdby; }
            set { SetPropertyValue(nameof(Createdby), ref _Createdby, value); }
        }
        #endregion

        #region Modifiedby
        private CustomSystemUser _Modifiedby;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public CustomSystemUser Modifiedby
        {
            get { return _Modifiedby; }
            set { SetPropertyValue(nameof(Modifiedby), ref _Modifiedby, value); }
        }
        #endregion

        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion

        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion

        #region ConstituentPricingTiers
        [Association("ConstituentPricing-ConstituentPricingTier")]
        [ImmediatePostData]
        public XPCollection<ConstituentPricingTier> ConstituentPricingTiers
        {
            get { return GetCollection<ConstituentPricingTier>("ConstituentPricingTiers"); }
        }
        #endregion
    }

}