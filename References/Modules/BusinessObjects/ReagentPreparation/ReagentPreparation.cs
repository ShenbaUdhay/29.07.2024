using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Modules.BusinessObjects.ReagentPreparation
{
    public enum Solvent
    {
        [XafDisplayName("N/A")]
        NA,
        [XafDisplayName("Sample As Solvent")]
        SampleAsSolvent,
        [XafDisplayName("Water As Solvent")]
        WaterAsSolvent
    }
    public enum StockSolution
    {
        [XafDisplayName("Vendor Stock")]
        VendorStock,
        [XafDisplayName("Lab Stock")]
        LabStock
    }
    [DefaultClassOptions]
    [Appearance("ShowVendorstock", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "VendorStock;LT;", Criteria = "[SelectStockSolution] = 'VendorStock'", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideVendorstock", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "VendorStock;LT;", Criteria = "[SelectStockSolution] = 'LabStock'", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    [Appearance("ShowLabstock", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "LabStock;LLTLabstock;", Criteria = "[SelectStockSolution] = 'LabStock'", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideLabstock", AppearanceItemType = "ViewItem", Context = "DetailView", TargetItems = "LabStock;LLTLabstock;", Criteria = "[SelectStockSolution] = 'VendorStock'", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ReagentPreparation : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ReagentPreparation(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            DatePrepared = DateTime.Now;
            PreparedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
        }
        protected override void OnSaving()
        {
            if (string.IsNullOrEmpty(LLT))
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Max(SUBSTRING(LLT, 3))");
                string tempID = (Convert.ToInt32(Session.Evaluate(typeof(ReagentPreparation), criteria, null)) + 1).ToString("000");
                var curdate = DateTime.Now.ToString("yyMMdd");
                if (tempID != "001")
                {
                    var predate = tempID.Substring(0, 6);
                    if (predate == curdate)
                    {
                        tempID = "LLT" + tempID;
                    }
                    else
                    {
                        tempID = "LLT" + curdate + "001";
                    }
                }
                else
                {
                    tempID = "LLT" + curdate + "001";
                }
                LLT = tempID;
            }
            //if(StockConc==null)
            //{
            //    StockConc = null;
            //}
        }
        #region LLT#
        private string _LLT;
        //[XafDisplayName("LLT#")]
        public string LLT
        {
            get { return _LLT; }
            set { SetPropertyValue(nameof(LLT), ref _LLT, value); }
        }
        #endregion
        #region PrepType
        private WSPrepType _PrepType;
        [RuleRequiredField]
        public WSPrepType PrepType
        {
            get { return _PrepType; }
            set { SetPropertyValue(nameof(PrepType), ref _PrepType, value); }
        }
        #endregion
        #region PrepName
        private StandardName _PrepName;
        [RuleRequiredField]
        public StandardName PrepName
        {
            get { return _PrepName; }
            set { SetPropertyValue(nameof(PrepName), ref _PrepName, value); }
        }
        #endregion
        #region Department
        private Department _Department;
        public Department Department
        {
            get { return _Department; }
            set { SetPropertyValue(nameof(Department), ref _Department, value); }
        }
        #endregion
        #region ExpirationDate
        private DateTime _ExpirationDate;
        [RuleRequiredField]
        [ImmediatePostData(true)]
        public DateTime ExpirationDate
        {
            get { return _ExpirationDate; }
            set { SetPropertyValue(nameof(ExpirationDate), ref _ExpirationDate, value); }
        }
        #endregion
        #region PreparedBy
        private Employee _PreparedBy;
        public Employee PreparedBy
        {
            get { return _PreparedBy; }
            set { SetPropertyValue(nameof(PreparedBy), ref _PreparedBy, value); }
        }
        #endregion
        #region Dateprepared
        private DateTime _DatePrepared;
        public DateTime DatePrepared
        {
            get { return _DatePrepared; }
            set { SetPropertyValue(nameof(DatePrepared), ref _DatePrepared, value); }
        }
        #endregion
        #region Solvent
        private Solvent _Solvent;
        [ImmediatePostData(true)]
        public Solvent Solvent
        {
            get { 
                return _Solvent; }
            set { SetPropertyValue(nameof(Solvent), ref _Solvent, value); }
        }
        #endregion
        #region Solvent ID
        private string _SolventID;
        public string SolventID
        {
            get { return _SolventID; }
            set { SetPropertyValue(nameof(SolventID), ref _SolventID, value); }
        }
        #endregion
        #region Storage
        private WSStorageName _Storage;
        [RuleRequiredField]
        public WSStorageName Storage
        {
            get { return _Storage; }
            set { SetPropertyValue(nameof(Storage), ref _Storage, value); }
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
        #region Test
        private TestMethod _Test;
        public TestMethod Test
        {
            get { return _Test; }
            set { SetPropertyValue(nameof(Test), ref _Test, value); }
        }
        #endregion
        //#region PrepLot
        //private string _PrepLot;
        //public string PrepLot
        //{
        //    get { return _PrepLot; }
        //    set { SetPropertyValue(nameof(PrepLot), ref _PrepLot, value); }
        //}
        //#endregion
        #region boolSolvent
        private bool _BoolSolvent;
        public bool BoolSolvent
        {
            get { return _BoolSolvent; }
            set { SetPropertyValue(nameof(BoolSolvent), ref _BoolSolvent, value); }
        }
        #endregion
        #region PrepSelectType
        private PrepSelectTypes _PrepSelectType;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        [Browsable(false)]
        public PrepSelectTypes PrepSelectType
        {
            get { return _PrepSelectType; }
            set { SetPropertyValue(nameof(PrepSelectType), ref _PrepSelectType, value); }
        }
        #endregion
        
        #region  ReagentPrepLog Collection
        [DevExpress.ExpressApp.DC.Aggregated, Association("ReagentPrparation-PreLog")]
        public XPCollection<ReagentPrepLog> ReagentPrepLogs
        {
            get { return GetCollection<ReagentPrepLog>(nameof(ReagentPrepLogs)); }
        }
        #endregion
        #region Calibration Prep Type
        #region SelectStockSolution
        private StockSolution _SelectStockSolution;
        [VisibleInDetailView(false),VisibleInListView(false),VisibleInDashboards(false)]
        [ImmediatePostData(true)]
        public StockSolution SelectStockSolution
        {
            get { return _SelectStockSolution; }
            set { SetPropertyValue(nameof(SelectStockSolution), ref _SelectStockSolution, value); }
        }
        #endregion
        #region StockConc
        private string _StockConc;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        [ImmediatePostData(true)]
        public string StockConc
        {
            get { return _StockConc; }
            set { SetPropertyValue(nameof(StockConc), ref _StockConc, value); }
        }
        #endregion
        #region VendorStock
        private Items _VendorStock;
        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
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
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
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
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public StandardName LabStock
        {
            get { return _LabStock; }
            set { SetPropertyValue(nameof(LabStock), ref _LabStock, value); }
        }
        #endregion
        #region LLT#
        private ReagentPreparation _LLTLabstock;
        [XafDisplayName("LLT#")]
        [DataSourceProperty("LLTDataSource", DataSourcePropertyIsNullMode.SelectNothing)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public ReagentPreparation LLTLabstock
        {
            get { return _LLTLabstock; }
            set { SetPropertyValue(nameof(LLTLabstock), ref _LLTLabstock, value); }
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
        #region StockConcUnit
        private ReagentUnits _StockConcUnit;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public ReagentUnits StockConcUnit
        {
            get { return _StockConcUnit; }
            set { SetPropertyValue(nameof(StockConcUnit), ref _StockConcUnit, value); }
        }
        #endregion

        #region Calibration Solvent
        private Items _CalibrationSolvent;
        [XafDisplayName("Solvent")]
        [ImmediatePostData(true)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public Items CalibrationSolvent
        {
            get { return _CalibrationSolvent; }
            set { SetPropertyValue(nameof(CalibrationSolvent), ref _CalibrationSolvent, value); }
        }
        #endregion
        #region Calibration SolventLT#
        private Distribution _CalibrationSolventLT;
        [XafDisplayName("Solvent LT#")]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        [DataSourceProperty("CalibrationSolventLTDataSource", DataSourcePropertyIsNullMode.SelectNothing)]
        public Distribution CalibrationSolventLT
        {
            get { return _CalibrationSolventLT; }
            set { SetPropertyValue(nameof(CalibrationSolventLT), ref _CalibrationSolventLT, value); }
        }
        #region LTDataSource
        [Browsable(false)]
        [NonPersistent]
        public XPCollection<Distribution> CalibrationSolventLTDataSource
        {
            get
            {
                if (CalibrationSolvent != null)
                {
                    return new XPCollection<Distribution>(Session, CriteriaOperator.Parse("[Item.Oid]=?", CalibrationSolvent.Oid));
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
        #endregion
        #region NoOfLevels
        private uint _NoOfLevels;
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInDashboards(false)]
        public uint NoOfLevels
        {
            get { return _NoOfLevels; }
            set { SetPropertyValue(nameof(NoOfLevels), ref _NoOfLevels, value); }
        }
        #endregion
        #endregion
    }
}