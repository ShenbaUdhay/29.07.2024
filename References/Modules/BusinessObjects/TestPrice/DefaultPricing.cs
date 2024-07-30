using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;
using Component = Modules.BusinessObjects.Setting.Component;
using Method = Modules.BusinessObjects.Setting.Method;

namespace Modules.BusinessObjects.TestPricing
{
    [DefaultClassOptions]
    [Appearance("ShowTestmethodDP", AppearanceItemType = "LayoutItem",
 TargetItems = "Item2", Criteria = "[IsGroup] = 'Yes'",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideTestmethodDP", AppearanceItemType = "LayoutItem",
 TargetItems = "Item2", Criteria = "[IsGroup] = 'No'",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    //ParameterListView
    [Appearance("ShowParameterDP", AppearanceItemType = "LayoutItem",
 TargetItems = "Item3", Criteria = "[IsGroup] = 'No'",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideParameterDP", AppearanceItemType = "LayoutItem",
 TargetItems = "Item3", Criteria = "[IsGroup] = 'Yes'",
 Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [RuleCombinationOfPropertiesIsUnique("DPUnique", DefaultContexts.Save, targetProperties: "Matrix,Test,Method,Component", CustomMessageTemplate = "A note with the same data already exists")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DefaultPricing : BaseObject
    {
        //[RuleCombinationOfPropertiesIsUnique]
        // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public DefaultPricing(Session session)
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
            Createdby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            Modifiedby = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            CreatedDate = Library.GetServerTime(Session);
            ModifiedDate = Library.GetServerTime(Session);
        }
        public enum ISGroupType
        {
            Yes,
            No
        }
        #region PriceCode
        private string _PriceCode;
        public string PriceCode
        {
            get { return _PriceCode; }
            set { SetPropertyValue(nameof(PriceCode), ref _PriceCode, value); }
        }
        #endregion
        #region Matrix
        private Matrix _Matrix;
        //[RuleRequiredField]
        public Matrix Matrix
        {
            get { return _Matrix; }
            set { SetPropertyValue(nameof(Matrix), ref _Matrix, value); }
        }
        #endregion
        #region Test
        private TestMethod _Test;
        //[RuleRequiredField]
        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        #endregion
        #region GroupTest
        //private GroupTest _GroupTest;
        //[ImmediatePostData]
        ////[RuleRequiredField]
        //public GroupTest GroupTest
        //{
        //    get { return _GroupTest; }
        //    set { SetPropertyValue(nameof(GroupTest), ref _GroupTest, value); }
        //}
        #endregion
        #region IsGroup
        private ISGroupType _IsGroup;
        //[Browsable(false)]
        [RuleRequiredField]
        [ImmediatePostData]
        [Appearance("IsGroupDPShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = 'Yes'", Context = "DetailView")]
        [Appearance("IsGroupDPHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = 'No'", Context = "DetailView")]
        public ISGroupType IsGroup
        {
            get { return _IsGroup; }
            set { SetPropertyValue(nameof(IsGroup), ref _IsGroup, value); }
        }
        #endregion
        #region Method
        private Method _Method;
        [ImmediatePostData]
        [Appearance("MethodDPShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = 'No'", Context = "DetailView")]
        [Appearance("MethodDPHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = 'Yes'", Context = "DetailView")]
        public Method Method
        {
            get { return _Method; }
            set { SetPropertyValue(nameof(Method), ref _Method, value); }
        }
        #endregion
        #region Component
        private Component _Component;
        //[RuleRequiredField]
        [ImmediatePostData]
        [Appearance("ComponentDPShow", Visibility = ViewItemVisibility.Show, Criteria = "[IsGroup] = 'No'", Context = "DetailView")]
        [Appearance("ComponentDPHide", Visibility = ViewItemVisibility.Hide, Criteria = "[IsGroup] = 'Yes'", Context = "DetailView")]
        public Component Component
        {
            get { return _Component; }
            set { SetPropertyValue("Component", ref _Component, value); }
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
        #region UnitPrice
        private decimal _UnitPrice;
        //[ImmediatePostData]
        [RuleRequiredField]
        public decimal UnitPrice
        {
            get { return _UnitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref _UnitPrice, Math.Round(value, 2)); }
        }
        #endregion
        #region Prep1Charge
        private decimal _Prep1Charge;
        [ImmediatePostData]
        public decimal Prep1Charge
        {
            get { return _Prep1Charge; }
            set { SetPropertyValue(nameof(Prep1Charge), ref _Prep1Charge, Math.Round(value, 2)); }
        }
        #endregion
        #region Prep2Charge
        private decimal _Prep2Charge;
        [ImmediatePostData]
        public decimal Prep2Charge
        {
            get { return _Prep2Charge; }
            set { SetPropertyValue(nameof(Prep2Charge), ref _Prep2Charge, Math.Round(value, 2)); }
        }
        #endregion
        #region TotalUnitPrice
        private decimal _TotalUnitPrice;
        [ImmediatePostData]
        public decimal TotalUnitPrice
        {
            get
            {
                //if(UnitPrice > 0)
                //{
                //    decimal totalunitprice = UnitPrice + Prep1Charge + Prep2Charge;
                //    _TotalUnitPrice = Math.Round(totalunitprice, 2);
                //}
                //else
                //{
                //    UnitPrice = 0;
                //    Prep1Charge = 0;
                //    Prep2Charge = 0;

                //}
                return _TotalUnitPrice;
            }
            set { SetPropertyValue(nameof(TotalUnitPrice), ref _TotalUnitPrice, Math.Round(value, 2)); }
        }
        #endregion
        #region Remark
        private string _Remark;
        [ImmediatePostData]
        [Size(int.MaxValue)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
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
        private DateTime _CreatedDate;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
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
    }
}