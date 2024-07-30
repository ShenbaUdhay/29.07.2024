using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [Appearance("ShowTestmethodLV", AppearanceItemType = "LayoutItem",
 TargetItems = "Item2", Criteria = "[IsGroup] = True",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestmethodLV", AppearanceItemType = "LayoutItem",
 TargetItems = "Item2", Criteria = "[IsGroup] = False",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TestPriceSurcharge : BaseObject, ICheckedListBoxItemsProvider
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        //PricingInfo priceinfo = new PricingInfo();
        public TestPriceSurcharge(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Modifiedby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            //Surcharge = null;

            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        protected override void OnSaving()
        {
            if (!string.IsNullOrEmpty(PriceCode))
            {
                string[] stringapricecode = PriceCode.Split('-');
                if (stringapricecode.Length == 2)
                {
                    PriceCode = PriceCode + "-" + PrioritySort.ToString();
                }
            }
            if (Surcharge == null)
            {
                Surcharge = 0;
            }
            ////string strpricecode = string.Empty;
            ////string strprcode = string.Empty;
            ////int prcode = 0;
            ////string pricecode = string.Empty;
            ////if (Matrix != null && Matrix.MatrixName != null && Test != null && Test.TestName != null && Method != null && Method.MethodName != null && Method.MethodName.MethodNumber != null && Component != null)
            ////{
            ////    CriteriaOperator estexpression = CriteriaOperator.Parse("Max(PriceCode)");
            ////    CriteriaOperator estfilter = CriteriaOperator.Parse("[Matrix.MatrixName] = ? And[Test.TestName] = ? And[Method.MethodName.MethodNumber] = ? And[Component.Components] = ? ", Matrix.MatrixName, Test.TestName, Method.MethodName.MethodNumber, Component.Components);
            ////    //var objtpsurcharge = Session.Evaluate<TestPriceSurcharge>(estexpression, estfilter);
            ////    //if (objtpsurcharge != null)
            ////    //{
            ////    //    strpricecode = objtpsurcharge.ToString();
            ////    //    //strpricecode = strpricecode.Replace("TS", "");
            ////    //    string[] strarr = strpricecode.Trim().Split('-');
            ////    //    if (strarr != null)
            ////    //    {
            ////    //        ////strprcode = (Convert.ToInt32(strarr[1]) + 1).ToString();
            ////    //        prcode = Convert.ToInt32(strarr[1]);
            ////    //        strprcode = (prcode + 1).ToString();
            ////    //        if (!string.IsNullOrEmpty(strprcode) && strprcode.Length == 1)
            ////    //        {
            ////    //            strprcode = "0" + strprcode;
            ////    //        }
            ////    //        PriceCode = /*"TS" + */strarr[0] + "-" + strprcode;
            ////    //    }
            ////    //}
            ////    //else
            ////    //{
            ////    //    CriteriaOperator expression = CriteriaOperator.Parse("Max(PriceCode)");
            ////    //    CriteriaOperator filter = CriteriaOperator.Parse("Not IsNullOrEmpty([PriceCode])");
            ////    //    var max = Session.Evaluate<TestPriceCode>(expression, filter);
            ////    //    if (max != null)
            ////    //    {
            ////    //        strprcode = max.ToString();
            ////    //        //strprcode = strprcode.Replace("TS", "");
            ////    //        if (!string.IsNullOrEmpty(strprcode))
            ////    //        {
            ////    //            string[] strarr = strprcode.Split('-');
            ////    //            if (strarr != null)
            ////    //            {
            ////    //                prcode = Convert.ToInt32(strarr[0]);
            ////    //                strprcode = (prcode + 1).ToString();
            ////    //                //strprcode = (Convert.ToInt32(strarr[0].Trim()) + 1).ToString();
            ////    //                PriceCode = /*"TS" +*/ strprcode + "-01";
            ////    //            }
            ////    //        }
            ////    //    }
            ////    //    else
            ////    //    {
            ////    //        PriceCode = "100-01";
            ////    //    }
            ////    //}
            ////    //priceinfo.strTPSpricecode = PriceCode;
            ////}
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
        [RuleRequiredField("MatrixTPS", DefaultContexts.Save)]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue("Matrix", ref _Matrix, value); }
        }
        #endregion

        #region Test
        private TestMethod _Test;
        [DataSourceProperty(nameof(TestDataSource))]
        [ImmediatePostData]
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
                if (Matrix != null && Test == null)
                {
                    List<object> groups = new List<object>();
                    using (XPView lstview = new XPView(Session, typeof(TestMethod)))
                    {
                        lstview.Criteria = CriteriaOperator.Parse("[MatrixName.MatrixName] = ?", Matrix.MatrixName);
                        lstview.Properties.Add(new ViewProperty("TTestName", DevExpress.Xpo.SortDirection.Ascending, "TestName", true, true));
                        lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
                        foreach (ViewRecord rec in lstview)
                            groups.Add(rec["Toid"]);
                    }
                    if (groups.Count > 0)
                    {
                        XPCollection<TestMethod> tests = new XPCollection<TestMethod>(Session, new InOperator("Oid", groups));
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

        #region Method
        private TestMethod _Method;
        [ImmediatePostData]
        //[RuleRequiredField("MethodTPS", DefaultContexts.Save)]
        [Appearance("MethodPSHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = True", Context = "DetailView")]
        [Appearance("MethodPSShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = False", Context = "DetailView")]
        [DataSourceProperty(nameof(MethodDataSource))]
        public TestMethod Method
        {

            get { return _Method; }
            set { SetPropertyValue("Method", ref _Method, value); }
        }

        [Browsable(false)]
        [ImmediatePostData]
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

        #region IsGroup
        private bool _IsGroup;
        //[RuleRequiredField("IsGroup", DefaultContexts.Save)]
        [ImmediatePostData]
        [Appearance("IsGroupPSHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = False", Context = "DetailView")]
        [Appearance("IsGroupPSShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = True", Context = "DetailView")]
        //[NonPersistent]
        public bool IsGroup
        {

            get
            {
                return _IsGroup;
            }
            set { SetPropertyValue("IsGroup", ref _IsGroup, value); }
        }
        #endregion

        #region Component
        private Component _Component;
        //[RuleRequiredField("ComponentTPS", DefaultContexts.Save)]
        ////[DataSourceProperty(nameof(ComponentDataSource))]
        [ImmediatePostData]
        [Appearance("ComponentPSHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = True", Context = "DetailView")]
        [Appearance("ComponentPSShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = False", Context = "DetailView")]
        public Component Component
        {
            get { return _Component; }
            set { SetPropertyValue("Component", ref _Component, value); }
        }

        ////[Browsable(false)]
        ////[NonPersistent]
        ////public XPCollection<Testparameter> ComponentDataSource
        ////{
        ////    get
        ////    {
        ////        if (Matrix != null && Test != null && Method != null)
        ////        {
        ////            List<object> groups = new List<object>();
        ////            using (XPView lstview = new XPView(Session, typeof(Testparameter)))
        ////            {
        ////                TestMethod objtm = Session.FindObject<TestMethod>(CriteriaOperator.Parse("[MatrixName.MatrixName] = ? And [TestName] = ? And [MethodName.MethodName] = ?",
        ////                    Matrix.MatrixName, Test.TestName, Method.MethodName.MethodName));
        ////                if (objtm != null)
        ////                {
        ////                    lstview.Criteria = CriteriaOperator.Parse("[TestMethod.Oid] = ? And [Component] Is Not Null", objtm.Oid);
        ////                    lstview.Properties.Add(new ViewProperty("TPComponent", DevExpress.Xpo.SortDirection.Ascending, "Component", true, true));
        ////                    lstview.Properties.Add(new ViewProperty("Toid", DevExpress.Xpo.SortDirection.Ascending, "MAX(Oid)", false, true));
        ////                    foreach (ViewRecord rec in lstview)
        ////                        groups.Add(rec["Toid"]);
        ////                }
        ////            }
        ////            if (groups.Count > 0)
        ////            {
        ////                XPCollection<Testparameter> tests = new XPCollection<Testparameter>(Session, new InOperator("Oid", groups));
        ////                return tests;
        ////            }
        ////            else
        ////            {
        ////                return null;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            return null;
        ////        }
        ////    }
        ////}
        #endregion

        #region TAT
        private string _TAT;
        [DevExpress.Persistent.Validation.RuleRequiredField]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        [Size(SizeAttribute.Unlimited)]
        ////[ImmediatePostData]
        ////[EditorAlias(EditorAliases.CheckedListBoxEditor)]
        ////[Size(SizeAttribute.Unlimited)]
        ////[RuleRequiredField("TATTPS", DefaultContexts.Save)]
        //////[RuleUniqueValue]
        public string TAT
        {
            get { return _TAT; }
            set { SetPropertyValue("TAT", ref _TAT, value); }
        }
        #endregion

        #region ChargeType
        private ChargeType _ChargeType;
        [ImmediatePostData]
        [RuleRequiredField]
        public ChargeType ChargeType
        {
            get { return _ChargeType; }
            set { SetPropertyValue(nameof(ChargeType), ref _ChargeType, value); }
        }
        #endregion

        [Browsable(false)]
        [ImmediatePostData]
        [NonPersistent]
        public XPCollection<TurnAroundTime> TATDatasource
        {
            get
            {
                return new XPCollection<TurnAroundTime>(Session, CriteriaOperator.Parse(""));
            }
        }

        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            Dictionary<object, string> Properties = new Dictionary<object, string>();
            if (targetMemberName == "TAT" && TATDatasource != null && TATDatasource.Count > 0)
            {
                foreach (TurnAroundTime objtat in TATDatasource.Where(i => i.TAT != null).OrderBy(i => i.Count).ToList())
                {
                    if (!Properties.ContainsKey(objtat.TAT) && !string.IsNullOrEmpty(objtat.TAT))
                    {
                        Properties.Add(objtat.TAT, objtat.TAT);
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

        #region Priority
        private Priority _Priority;
        [RuleRequiredField("PriorityTPS", DefaultContexts.Save)]
        public Priority Priority
        {
            get { return _Priority; }
            set { SetPropertyValue("Priority", ref _Priority, value); }
        }
        #endregion

        #region PrioritySort
        private int _PrioritySort;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [ImmediatePostData]
        [NonPersistent]
        public int PrioritySort
        {
            get
            {
                if (Priority != null)
                {
                    _PrioritySort = Priority.Sort;
                }
                return _PrioritySort;
            }
            set { SetPropertyValue("PrioritySort", ref _PrioritySort, value); }
        }
        #endregion

        #region Surcharge 
        private int _Surcharge;
        [ImmediatePostData]
        [XafDisplayName("Surcharge(%)")]
        //[Nullable(true)]
        public int Surcharge
        {
            get { return _Surcharge; }
            set { SetPropertyValue("Surcharge", ref _Surcharge, value); }
        }
        #endregion

        #region Surcharge Price
        private Nullable<decimal> _SurchargePrice;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        [ImmediatePostData]
        public Nullable<decimal> SurchargePrice
        {
            get { return _SurchargePrice; }
            set { SetPropertyValue("SurchargePrice", ref _SurchargePrice, value); }
        }
        #endregion
        #region Remark
        private string _Remark;
        [Size(1000)]
        [ImmediatePostData]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue("Remark", ref _Remark, value); }
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