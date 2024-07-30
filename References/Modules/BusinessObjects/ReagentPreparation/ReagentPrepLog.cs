using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using ICM.Module.BusinessObjects;
using Modules.BusinessObjects.ICM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Modules.BusinessObjects.ReagentPreparation
{
    [DefaultClassOptions]
    [Appearance("ShowMW", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "MW;", Criteria = "[isShowMW] = True", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideMW", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "MW;", Criteria = "[isShowMW] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("ShowEqWt", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "EqWt;", Criteria = "[isShowEqWt] = True", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideEqWt", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "EqWt;", Criteria = "[isShowEqWt] = False", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class ReagentPrepLog : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReagentPrepLog(Session session)
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
            if(LTExpDate!=null && LTExpDate==DateTime.MinValue)
            {
                LTExpDate = null;
            }
            if (LLTExpDate != null && LLTExpDate == DateTime.MinValue)
            {
                LLTExpDate = null;
            }
        }
        #region Chemistry Type
        #region VendorStock
        private Items _VendorStock;
        [ImmediatePostData]
        public Items VendorStock
        {
            get { return _VendorStock; }
            set { SetPropertyValue(nameof(VendorStock), ref _VendorStock, value); }
        }
        #endregion
        #region LT#
        private Distribution _LT;
        [XafDisplayName("LT#")]
        [ImmediatePostData]
        [DataSourceProperty("LTDataSource", DataSourcePropertyIsNullMode.SelectNothing)]
        public Distribution LT
        {
            get { return _LT; }
            set { SetPropertyValue(nameof(LT), ref _LT, value); }
        }
        #region LTDataSource
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Distribution> LTDataSource
        {
            get
            {
                if (VendorStock != null)
                {
                    return new XPCollection<Distribution>(Session, CriteriaOperator.Parse("[Item.Oid]=?", VendorStock.Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #endregion

        #region LabStock
        private StandardName _LabStock;
        [ImmediatePostData(true)]
        public StandardName LabStock
        {
            get { return _LabStock; }
            set { SetPropertyValue(nameof(LabStock), ref _LabStock, value); }
        }
        #endregion
        #region LLT#
        private ReagentPreparation _LLT;
        [XafDisplayName("LLT#")]
        [ImmediatePostData(true)]
        [DataSourceProperty("LLTDataSource", DataSourcePropertyIsNullMode.SelectNothing)]
        public ReagentPreparation LLT
        {
            get { return _LLT; }
            set { SetPropertyValue(nameof(LLT), ref _LLT, value); }
        }
        #region LLTDataSource
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<ReagentPreparation> LLTDataSource
        {
            get
            {
                if (LabStock != null)
                {
                    return new XPCollection<ReagentPreparation>(Session, CriteriaOperator.Parse("[PrepName.Oid]=?", LabStock.Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #endregion
        #region CalculationApproach
        private CalculationApproach _CalculationApproach;
        [ImmediatePostData(true)]
        public CalculationApproach CalculationApproach
        {
            get { return _CalculationApproach; }
            set { SetPropertyValue(nameof(CalculationApproach), ref _CalculationApproach, value); }
        }
        #endregion
        #region InitialVolTaken(V1)Units
        private ReagentUnits _InitialVolTaken_V1_Units;
        [ImmediatePostData(true)]
        public ReagentUnits InitialVolTaken_V1_Units
        {
            get { return _InitialVolTaken_V1_Units; }
            set { SetPropertyValue(nameof(InitialVolTaken_V1_Units), ref _InitialVolTaken_V1_Units, value); }
        }
        #endregion
        #region StockConc(c1)Units
        private ReagentUnits _StockConc_C1_Units;
        [ImmediatePostData(true)]
        public ReagentUnits StockConc_C1_Units
        {
            get { return _StockConc_C1_Units; }
            set { SetPropertyValue(nameof(StockConc_C1_Units), ref _StockConc_C1_Units, value); }
        }
        #endregion
        #region FinalVol(V2)Units
        private ReagentUnits _FinalVol_V2_Units;
        [ImmediatePostData(true)]
        public ReagentUnits FinalVol_V2_Units
        {
            get { return _FinalVol_V2_Units; }
            set { SetPropertyValue(nameof(FinalVol_V2_Units), ref _FinalVol_V2_Units, value); }
        }
        #endregion
        #region FinalConc(C2)Units
        private ReagentUnits _FinalConc_C2_Units;
        [ImmediatePostData(true)]
        public ReagentUnits FinalConc_C2_Units
        {
            get { return _FinalConc_C2_Units; }
            set { SetPropertyValue(nameof(FinalConc_C2_Units), ref _FinalConc_C2_Units, value); }
        }
        #endregion
 
        #region Purity(%)
        private string _Purity;
        public string Purity
        {
            get { return _Purity; }
            set { SetPropertyValue(nameof(Purity), ref _Purity, value); }
        }
        #endregion
        #region Density
        private string _Density;
        public string Density
        {
            get { return _Density; }
            set { SetPropertyValue(nameof(Density), ref _Density, value); }
        }
        #endregion
        #region Constant
        private string _Constant;
        public string Constant
        {
            get { return _Constant; }
            set { SetPropertyValue(nameof(Constant), ref _Constant, value); }
        }
        #endregion
        #region Weight (g) (w1)
        private string _Weight_g_w1;
        [ImmediatePostData(true)]
        public string Weight_g_w1
        {
            get { return _Weight_g_w1; }
            set { SetPropertyValue(nameof(Weight_g_w1), ref _Weight_g_w1, value); }
        }
        #endregion
        #region VolumeTaken(v1)
        private string _VolumeTaken_V1;
        [ImmediatePostData(true)]
        public string VolumeTaken_V1
        {
            get { return _VolumeTaken_V1; }
            set { SetPropertyValue(nameof(VolumeTaken_V1), ref _VolumeTaken_V1, value); }
        }
        #endregion
        #region StockConc(C1)
        private string _StockConc_C1;
        [ImmediatePostData(true)]
        public string StockConc_C1
        {
            get { return _StockConc_C1; }
            set { SetPropertyValue(nameof(StockConc_C1), ref _StockConc_C1, value); }
        }
        #endregion
        #region FinalVol(V2) 
        private string _FinalVol_V2;
        [ImmediatePostData(true)]
        public string FinalVol_V2
        {
            get { return _FinalVol_V2; }
            set { SetPropertyValue(nameof(FinalVol_V2), ref _FinalVol_V2, value); }
        }
        #endregion
        #region FinalConc(c2) 
        private string _FinalConc_C2;
        [ImmediatePostData(true)]
        public string FinalConc_C2
        {
            get { return _FinalConc_C2; }
            set { SetPropertyValue(nameof(FinalConc_C2), ref _FinalConc_C2, value); }
        }
        #endregion
        # region Solvent
        private bool _Solvent;
        [ImmediatePostData(true)]
        public bool Solvent
        {
            get { return _Solvent; }
            set { SetPropertyValue(nameof(Solvent), ref _Solvent, value); }
        }
        #endregion
        # region LTExpDate
        private Nullable<DateTime>_LTExpDate;
        public Nullable<DateTime> LTExpDate
        {
            get {
                if(LT!=null)
                {
                    _LTExpDate = LT.ExpiryDate;
                }
                return _LTExpDate; 
                }
            set { SetPropertyValue(nameof(LTExpDate), ref _LTExpDate, value); }
        }
        #endregion
        # region LLTExpDate
        private Nullable<DateTime> _LLTExpDate;
        public Nullable<DateTime> LLTExpDate
        {
            get {  
                 if(LLT!=null)
                   {
                    _LLTExpDate = LLT.ExpirationDate;
                   }
                    return _LLTExpDate; 
                  }
            set { SetPropertyValue(nameof(LLTExpDate), ref _LLTExpDate, value); }
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
        #region Comment
        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        #endregion
        #region ComponentID
        private uint _ComponentID;
        public uint ComponentID
        {
            get { return _ComponentID; }
            set { SetPropertyValue(nameof(ComponentID), ref _ComponentID, value); }
        }
        #endregion
        #region ReagentPreparation
        private ReagentPreparation _ReagentPreparation;
        [Association("ReagentPrparation-PreLog")]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        [VisibleInListView(false)]
        public ReagentPreparation ReagentPreparation

        {
            get { return _ReagentPreparation; }
            set { SetPropertyValue(nameof(ReagentPreparation), ref _ReagentPreparation, value); }
        }
        #endregion
        #region MW
        private string _MW;
        [VisibleInDetailView(false),VisibleInListView(false),VisibleInDashboards(false)]
        public string MW
        {
            get { return _MW; }
            set { SetPropertyValue(nameof(MW), ref _MW, value); }
        }
        #endregion
        #region isShowMW
        private bool _isShowMW;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public bool isShowMW
        {
            get { 
                if(!string.IsNullOrEmpty(Formula))
                {
                    if(Formula.Contains("MW"))
                    {
                        _isShowMW = true;
                    }
                    else
                    {
                        _isShowMW = false;
                    }
                }
                else
                {
                    _isShowMW = false;
                }
                return _isShowMW;
                }
         
        }
        #endregion
        #region EqWt
        private string _EqWt;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string EqWt
        {
            get { return _EqWt; }
            set { SetPropertyValue(nameof(EqWt), ref _EqWt, value); }
        }
        #endregion
        #region isShowEqWt
        private bool _isShowEqWt;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        [Browsable(false)]
        public bool isShowEqWt
        {
            get
            {
                if (!string.IsNullOrEmpty(Formula))
                {
                    if (Formula.Contains("EqWt"))
                    {
                        _isShowEqWt = true;
                    }
                    else
                    {
                        _isShowEqWt = false;
                    }
                }
                else
                {
                    _isShowEqWt = false;
                }
                return _isShowEqWt;
            }

        }
        #endregion
        #endregion

        #region CalibrationType
        #region WorkingStdName
        private string _WorkingStdName;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string WorkingStdName
        {
            get { return _WorkingStdName; }
            set { SetPropertyValue(nameof(WorkingStdName), ref _WorkingStdName, value); }
        }
        #endregion
        #region StockStdName
        private string _StockStdName;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string StockStdName
        {
            get { return _StockStdName; }
            set { SetPropertyValue(nameof(StockStdName), ref _StockStdName, value); }
        }
        #endregion
        #region WSCons_Units
        private ReagentUnits _WSCons_Units;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public ReagentUnits WSCons_Units
        {
            get { return _WSCons_Units; }
            set { SetPropertyValue(nameof(WSCons_Units), ref _WSCons_Units, value); }
        }
        #endregion
        #region SubLLT
        private string _SubLLT;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        [NonPersistent]
        public string SubLLT
        {
            get { 
                if(ReagentPreparation!=null && ReagentPreparation.LLT!=null)
                {
                    _SubLLT = ReagentPreparation.LLT +"_"+ ComponentID;
                }
                else
                {
                    _SubLLT = null;
                }
                return _SubLLT;
                }
        }
        #endregion
        #region Cal_VolTaken(V1)Units
        private ReagentUnits _Cal_VolTaken_V1_Units;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public ReagentUnits Cal_VolTaken_V1_Units
        {
            get { return _Cal_VolTaken_V1_Units; }
            set { SetPropertyValue(nameof(Cal_VolTaken_V1_Units), ref _Cal_VolTaken_V1_Units, value); }
        }
        #endregion
        #region Cal_FinalVol(V2)Units
        private ReagentUnits _Cal_FinalVol_V2_Units;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public ReagentUnits Cal_FinalVol_V2_Units
        {
            get { return _Cal_FinalVol_V2_Units; }
            set { SetPropertyValue(nameof(Cal_FinalVol_V2_Units), ref _Cal_FinalVol_V2_Units, value); }
        }
        #endregion
        #region Cal_FinalConc(C2)Units
        private ReagentUnits _Cal_FinalConc_C2_Units;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public ReagentUnits Cal_FinalConc_C2_Units
        {
            get { return _Cal_FinalConc_C2_Units; }
            set { SetPropertyValue(nameof(Cal_FinalConc_C2_Units), ref _Cal_FinalConc_C2_Units, value); }
        }
        #endregion

        #region Cal_Weight (g) (w1)
        private string _Cal_Weight_g_w1;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string Cal_Weight_g_w1
        {
            get { return _Cal_Weight_g_w1; }
            set { SetPropertyValue(nameof(Cal_Weight_g_w1), ref _Cal_Weight_g_w1, value); }
        }
        #endregion
        #region Cal_VolumeTaken(v1)
        private string _Cal_VolumeTaken_V1;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string Cal_VolumeTaken_V1
        {
            get { return _Cal_VolumeTaken_V1; }
            set { SetPropertyValue(nameof(Cal_VolumeTaken_V1), ref _Cal_VolumeTaken_V1, value); }
        }
        #endregion
        #region Cal_FinalVol(V2) 
        private string _Cal_FinalVol_V2;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string Cal_FinalVol_V2
        {
            get { return _Cal_FinalVol_V2; }
            set { SetPropertyValue(nameof(Cal_FinalVol_V2), ref _Cal_FinalVol_V2, value); }
        }
        #endregion
        #region Cal_FinalConc(c2) 
        private string _Cal_FinalConc_C2;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string Cal_FinalConc_C2
        {
            get { return _Cal_FinalConc_C2; }
            set { SetPropertyValue(nameof(Cal_FinalConc_C2), ref _Cal_FinalConc_C2, value); }
        }
        #endregion
        #endregion

        #region MicroMedia
        #region PHCriteria
        private string _PHCriteria;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string PHCriteria
        {
            get { return _PHCriteria; }
            set { SetPropertyValue(nameof(PHCriteria), ref _PHCriteria, value); }
        }
        #endregion
        #region PH
        private string _PH;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string PH
        {
            get { return _PH; }
            set { SetPropertyValue(nameof(PH), ref _PH, value); }
        }
        #endregion
        #region Autoclave(Y/N)
        private string _Autoclave;
        [XafDisplayName("Autoclave(Y/N)")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string Autoclave
        {
            get { return _Autoclave; }
            set { SetPropertyValue(nameof(Autoclave), ref _Autoclave, value); }
        }
        #endregion
        #region FilterSterilization(Y/N)
        private string _FilterSterilization;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string FilterSterilization
        {
            get { return _FilterSterilization; }
            set { SetPropertyValue(nameof(FilterSterilization), ref _FilterSterilization, value); }
        }
        #endregion
        #region SporeGrowth(Y/N)
        private string _SporeGrowth;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string SporeGrowth
        {
            get { return _SporeGrowth; }
            set { SetPropertyValue(nameof(SporeGrowth), ref _SporeGrowth, value); }
        }
        #endregion
        #region +Control
        private string _PositiveControl;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string PositiveControl
        {
            get { return _PositiveControl; }
            set { SetPropertyValue(nameof(PositiveControl), ref _PositiveControl, value); }
        }
        #endregion
        #region -Control
        private string _NegativeControl;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public string NegativeControl
        {
            get { return _NegativeControl; }
            set { SetPropertyValue(nameof(NegativeControl), ref _NegativeControl, value); }
        }
        #endregion
        #endregion
    }
}