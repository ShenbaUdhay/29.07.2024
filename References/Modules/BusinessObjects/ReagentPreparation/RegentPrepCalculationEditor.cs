using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.ReagentPreparation
{
    [DefaultClassOptions]
    [Appearance("ShowUnit", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "IsClose;CloseReason;", Criteria = "[IsClose] = True", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideUnit", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "IsClose;CloseReason;", Criteria = "[IsClose] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class RegentPrepCalculationEditor : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public RegentPrepCalculationEditor(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(CodeID))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(CodeID, 3))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(RegentPrepCalculationEditor), criteria, null)) + 1).ToString("000");
                CodeID = "FC" + tempID;
            }
            if (HideV1Unit == true)
            {
                V1Units = null;
            }
            if (HideW1Unit == true)
            {
                W1Units = null;
            }
        }
        #region CodeID
        private string _CodeID;
        public string CodeID
        {
            get { return _CodeID; }
            set { SetPropertyValue(nameof(CodeID), ref _CodeID, value); }
        }
        #endregion
        #region CalculationApproch
        private CalculationApproach _CalculationApproch;
        [RuleRequiredField]
        [ImmediatePostData(true)]
        public CalculationApproach CalculationApproch
        {
            get { return _CalculationApproch; }
            set { SetPropertyValue(nameof(CalculationApproch), ref _CalculationApproch, value); }
        }
        #endregion
        #region C2Units
        private ReagentUnits _C2Units;
        public ReagentUnits C2Units
        {
            get { return _C2Units; }
            set { SetPropertyValue(nameof(C2Units), ref _C2Units, value); }
        }
        #endregion
        #region V2Units
        private ReagentUnits _V2Units;
        public ReagentUnits V2Units
        {
            get { return _V2Units; }
            set { SetPropertyValue(nameof(V2Units), ref _V2Units, value); }
        }
        #endregion
        #region C1Units
        private ReagentUnits _C1Units;
        public ReagentUnits C1Units
        {
            get { return _C1Units; }
            set { SetPropertyValue(nameof(C1Units), ref _C1Units, value); }
        }
        #endregion
        #region V1Units
        private ReagentUnits _V1Units;
        [Appearance("V1UnitsHide", Visibility = ViewItemVisibility.Hide, Criteria = "HideV1Unit='True'", Context = "DetailView")]
        [Appearance("V1UnitsShow", Visibility = ViewItemVisibility.Show, Criteria = "HideV1Unit='False'", Context = "DetailView")]
        public ReagentUnits V1Units
        {
            get { return _V1Units; }
            set { SetPropertyValue(nameof(V1Units), ref _V1Units, value); }
        }
        #endregion
        #region W1Units
        private ReagentUnits _W1Units;
        [Appearance("W1UnitsHide", Visibility = ViewItemVisibility.Hide, Criteria = "HideW1Unit='True'", Context = "DetailView")]
        [Appearance("W1UnitsShow", Visibility = ViewItemVisibility.Show, Criteria = "HideW1Unit='False'", Context = "DetailView")]
        public ReagentUnits W1Units
        {
            get { return _W1Units; }
            set { SetPropertyValue(nameof(W1Units), ref _W1Units, value); }
        }
        #endregion
        #region Formula
        private string _Formula;
        [Size(SizeAttribute.Unlimited)]
        public string Formula
        {
            get { return _Formula; }
            set { SetPropertyValue(nameof(Formula), ref _Formula, value); }
        }
        #endregion
        #region Remark
        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue(nameof(Remark), ref _Remark, value); }
        }
        #endregion
        #region CreatedDate
        private DateTime _CreatedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue(nameof(CreatedDate), ref _CreatedDate, value); }
        }
        #endregion
        #region CreatedBy
        private Employee _CreatedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue(nameof(CreatedBy), ref _CreatedBy, value); }
        }
        #endregion
        #region ModifiedDate
        private DateTime _ModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { SetPropertyValue(nameof(ModifiedDate), ref _ModifiedDate, value); }
        }
        #endregion
        #region ModifiedBy
        private Employee _ModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public Employee ModifiedBy
        {
            get { return _ModifiedBy; }
            set { SetPropertyValue(nameof(ModifiedBy), ref _ModifiedBy, value); }
        }
        #endregion
        #region HideV1Unit
        private bool _HideV1Unit;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool HideV1Unit
        {
            get
            {
                if (CalculationApproch != null)
                {
                    if (CalculationApproch.Approach != null && !CalculationApproch.Approach.Contains("V1"))
                    {
                        _HideV1Unit = true;
                    }
                    else
                    {
                        _HideV1Unit = false;
                    }
                }
                else
                {
                    _HideV1Unit = false;
                }
                return _HideV1Unit;
            }
            set { SetPropertyValue(nameof(HideV1Unit), ref _HideV1Unit, value); }
        }
        #endregion
        #region HideV1Unit
        private bool _HideW1Unit;
        [NonPersistent]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public bool HideW1Unit
        {
            get
            {
                if (CalculationApproch != null)
                {
                    if (CalculationApproch.Approach != null && !CalculationApproch.Approach.Contains("W1"))
                    {
                        _HideW1Unit = true;
                    }
                    else
                    {
                        _HideW1Unit = false;
                    }
                }
                else
                {
                    _HideW1Unit = false;
                }
                return _HideW1Unit;
            }
            set { SetPropertyValue(nameof(HideW1Unit), ref _HideW1Unit, value); }
        }
        #endregion

    }
}