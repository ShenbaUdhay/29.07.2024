using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.Setting
{

    public enum EnumRELevelSetup
    {
        Yes = 1,  //true
        No = 0  //false
    };

    public enum EnumDateFilter
    {
        [XafDisplayName("1M")]
        OneMonth,
        [XafDisplayName("3M")]
        ThreeMonth,
        [XafDisplayName("6M")]
        SixMonth,
        [XafDisplayName("1Y")]
        OneYear,
        [XafDisplayName("2Y")]
        TwoYear,
        [XafDisplayName("5Y")]
        FiveYear,
        All
    };
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).

    ////   [Appearance("ShowInventoryManagementlayoutGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_InventoryControlManagement", Criteria = "ModuleName = 'Inventory Management'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideInventoryManagementLayoutGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_InventoryControlManagement", Criteria = "ModuleName <> 'Inventory Management'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowTaskManagementGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_TaskManagement", Criteria = "ModuleName = 'Task Management'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideTaskManagementGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_TaskManagement", Criteria = "ModuleName <> 'Task Management'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowMyDesktopGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_MyDeskTop", Criteria = "ModuleName = 'My Desktop'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideMyDesktopGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_MyDeskTop", Criteria = "ModuleName <> 'My Desktop'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowSampleTrackingGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_SampleTracking", Criteria = "ModuleName = 'Sample Tracking'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideSampleTrackingGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_SampleTracking", Criteria = "ModuleName <> 'Sample Tracking'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowInspectionGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_Inspection", Criteria = "ModuleName = 'Inspection'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideInspectionGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_Inspection", Criteria = "ModuleName <> 'Inspection'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowSamplePreparationGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_SamplePreparation", Criteria = "ModuleName = 'Sample Preparation'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideSamplePreparationGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_SamplePreparation", Criteria = "ModuleName <> 'Sample Preparation'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowSampleWeighingGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_SampleWeighing", Criteria = "ModuleName = 'Sample Weighing'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideSampleWeighingGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_SampleWeighing", Criteria = "ModuleName <> 'Sample Weighing'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowAnalysisGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_Analysis", Criteria = "ModuleName = 'Analysis'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideAnalysisGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_Analysis", Criteria = "ModuleName <> 'Analysis'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowDataReviewGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_DataReview", Criteria = "ModuleName = 'Data Review'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideDataReviewGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_DataReview", Criteria = "ModuleName <> 'Data Review'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowReportingroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_Reporting", Criteria = "ModuleName = 'Reporting'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideReportingGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_Reporting", Criteria = "ModuleName <> 'Reporting'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowMaintenanceroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_Maintenance", Criteria = "ModuleName = 'Maintenance'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideMaintenanceGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_Maintenance", Criteria = "ModuleName <> 'Maintenance'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    ////   [Appearance("ShowAuditTrailGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_AuditTrail", Criteria = "ModuleName = 'Audit Trail'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideAuditTrailGroup", AppearanceItemType = "LayoutItem",
    ////TargetItems = "Grp_AuditTrail", Criteria = "ModuleName <> 'Audit Trail'",
    ////Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]




    ////   //[Appearance("Can Edit Value", Context = "DefaultSetting_ListView_2", TargetItems = "Requisition", Criteria = "not (Operation.CanEdit and CanEdit)", Enabled = false)]

    ////   [Appearance("Mandatory Enable", Context = "ListView", TargetItems = "Select", Criteria = ("Mandatory = 'True'"), Enabled = false)]

    ////   [Appearance("Operation1 Enable", Context = "DetailView", TargetItems = "Requisition", Criteria = " FlowControl = 'WithPurchasing'", Enabled = false)]
    ////   [Appearance("Operation2 Enable", Context = "DetailView", TargetItems = "Review", Criteria = " FlowControl = 'WithPurchasing'", Enabled = false)]
    ////   [Appearance("Operation3 Enable", Context = "DetailView", TargetItems = "OrderingItem", Criteria = " FlowControl = 'WithPurchasing'", Enabled = false)]
    ////   [Appearance("Operation4 Enable", Context = "DetailView", TargetItems = "Receiving", Criteria = " FlowControl = 'WithPurchasing'", Enabled = false)]
    ////   [Appearance("Operation5 Enable", Context = "DetailView", TargetItems = "Distribution", Criteria = " FlowControl = 'WithPurchasing'", Enabled = false)]

    ////   [Appearance("ShowFlowControl", AppearanceItemType = "ViewItem",
    ////   TargetItems = "FlowControl", Criteria = "Operations = True",
    ////   Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    ////   [Appearance("HideFlowControl", AppearanceItemType = "ViewItem",
    ////   TargetItems = "FlowControl", Criteria = "Operations = False",
    ////   Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]

    [Appearance("ShowNoofLevels", AppearanceItemType = "ViewItem", TargetItems = "NoofLevels", Criteria = "RequisitionApproval = True", Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
    [Appearance("HideNoofLevels", AppearanceItemType = "ViewItem", TargetItems = "NoofLevels", Criteria = "RequisitionApproval = False", Context = "DetailView", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
    public class DefaultSetting : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public DefaultSetting(Session session)
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
            if (NavigationItemNameID != null && NavigationItemNameID.Trim() == "RawDataLevel2BatchReview")
            {
                DefaultSetting objSDMSSetting = Session.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='Spreadsheet'"));
                if (objSDMSSetting != null)
                {
                    if (Select)
                    {
                        objSDMSSetting.Review = EnumRELevelSetup.Yes;
                        Review = EnumRELevelSetup.Yes;
                    }
                    else
                    {
                        objSDMSSetting.Review = EnumRELevelSetup.No;
                        Review = EnumRELevelSetup.No;
                    }
                }
            }
            else
            if (NavigationItemNameID != null && NavigationItemNameID.Trim() == "RawDataLevel3BatchReview")
            {
                DefaultSetting objSDMSSetting = Session.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='Spreadsheet'"));
                if (objSDMSSetting != null)
                {
                    if (Select)
                    {
                        objSDMSSetting.Verify = EnumRELevelSetup.Yes;
                        Verify = EnumRELevelSetup.Yes;
                    }
                    else
                    {
                        objSDMSSetting.Verify = EnumRELevelSetup.No;
                        Verify = EnumRELevelSetup.No;
                    }
                }
            }
            else
            if (NavigationItemNameID != null && NavigationItemNameID.Trim() == "Result Validation")
            {
                DefaultSetting objSDMSSetting = Session.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='Spreadsheet'"));
                if (objSDMSSetting != null)
                {
                    if (Select)
                    {
                        objSDMSSetting.REValidate = EnumRELevelSetup.Yes;
                        REValidate = EnumRELevelSetup.Yes;
                    }
                    else
                    {
                        objSDMSSetting.REValidate = EnumRELevelSetup.No;
                        REValidate = EnumRELevelSetup.No;
                    }
                }
            }
            else
            if (NavigationItemNameID != null && NavigationItemNameID.Trim() == "Result Approval")
            {
                DefaultSetting objSDMSSetting = Session.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='Spreadsheet'"));
                if (objSDMSSetting != null)
                {
                    if (Select)
                    {
                        objSDMSSetting.REApprove = EnumRELevelSetup.Yes;
                        REApprove = EnumRELevelSetup.Yes;
                    }
                    else
                    {
                        objSDMSSetting.REApprove = EnumRELevelSetup.No;
                        REApprove = EnumRELevelSetup.No;
                    }
                }
            }
        }

        #region Review
        private EnumRELevelSetup _Review = EnumRELevelSetup.Yes;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumRELevelSetup Review
        {
            get { return _Review; }
            //set { SetPropertyValue("Review", ref _Review, value); }
            set
            {
                SetPropertyValue("Review", ref _Review, value);
            }
        }
        #endregion

        #region Verify
        private EnumRELevelSetup _Verify = EnumRELevelSetup.Yes;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumRELevelSetup Verify
        {
            get { return _Verify; }
            //set { SetPropertyValue("Verify", ref _Verify, value); }
            set { SetPropertyValue("Verify", ref _Verify, value); }
        }
        #endregion
        #region Validate
        private EnumRELevelSetup _REValidate = EnumRELevelSetup.Yes;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumRELevelSetup REValidate
        {
            get { return _REValidate; }
            set
            {
                SetPropertyValue("REValidate", ref _REValidate, value);
                if (value == EnumRELevelSetup.No)
                {
                    REReviewValidate = EnumRELevelSetup.No;
                }

            }
        }
        #endregion
        #region Approve
        private EnumRELevelSetup _REApprove = EnumRELevelSetup.Yes;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumRELevelSetup REApprove
        {
            get { return _REApprove; }
            set
            {
                SetPropertyValue("REApprove", ref _REApprove, value);
                if (value == EnumRELevelSetup.No)
                {
                    REVerifyApprove = EnumRELevelSetup.No;
                }
            }
        }
        #endregion


        #region ReviewValidate
        private EnumRELevelSetup _REReviewValidate = EnumRELevelSetup.No;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        [Appearance("ReviewValidate", AppearanceItemType = "ViewItem", Criteria = "REValidate=1" /*AND SDMS=1*/, Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
        [Appearance("ReviewValidate1", AppearanceItemType = "ViewItem", Criteria = "REValidate=0"  /*OR SDMS = 0*/, Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public EnumRELevelSetup REReviewValidate
        {
            get { return _REReviewValidate; }
            set
            {
                SetPropertyValue("REReviewValidate", ref _REReviewValidate, value);
                if (value == EnumRELevelSetup.No && REVerifyApprove == EnumRELevelSetup.Yes)
                {
                    SetPropertyValue("REVerifyApprove", ref _REVerifyApprove, value);
                }
                // else
                //{
                //    SetPropertyValue("ReviewValidate", ref _ReviewValidate, value);
                //}

            }
        }
        #endregion 

        #region VerifyApprove
        private EnumRELevelSetup _REVerifyApprove = EnumRELevelSetup.No;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        [Appearance("VerifyApprove", AppearanceItemType = "ViewItem", Criteria = "REApprove=1", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Show)]
        [Appearance("VerifyApprove1", AppearanceItemType = "ViewItem", Criteria = "REApprove=0", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        public EnumRELevelSetup REVerifyApprove
        {
            get { return _REVerifyApprove; }
            set
            {
                if (REReviewValidate == EnumRELevelSetup.No)
                {
                    SetPropertyValue("REVerifyApprove", ref _REVerifyApprove, REReviewValidate);
                }
                else
                {
                    SetPropertyValue("REVerifyApprove", ref _REVerifyApprove, value);
                }
            }

        }
        #endregion

        #region  ReportApproval
        //private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        private bool _ReportApproval = true;
        [ImmediatePostData]
        public bool ReportApproval
        {
            get { return _ReportApproval; }

            set
            {
                SetPropertyValue("ReportApproval", ref _ReportApproval, value);
            }
        }
        #endregion

        //////#region Navigation Settings

        //////#region MY DESKTOP
        //////#region  MyDesktop
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _MyDesktop = true;
        //////[ImmediatePostData]
        //////public bool MyDesktop
        //////{
        //////    get { return _MyDesktop; }

        //////    set
        //////    {
        //////        SetPropertyValue("MyDesktop", ref _MyDesktop, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Calendar
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Calendar = true;
        //////[ImmediatePostData]
        //////public bool Calendar
        //////{
        //////    get { return _Calendar; }

        //////    set
        //////    {
        //////        SetPropertyValue("Calendar", ref _Calendar, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Pending Agenda
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Agenda = true;
        //////[ImmediatePostData]
        //////public bool Agenda
        //////{
        //////    get { return _Agenda; }

        //////    set
        //////    {
        //////        SetPropertyValue("Agenda", ref _Agenda, value);
        //////    }
        //////}
        //////#endregion

        //////#region  TaskTracking
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _TaskTracking = true;
        //////[ImmediatePostData]
        //////public bool TaskTracking
        //////{
        //////    get { return _TaskTracking; }

        //////    set
        //////    {
        //////        SetPropertyValue("TaskTracking", ref _TaskTracking, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Project Tracking
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ProjectTracking = true;
        //////[ImmediatePostData]
        //////public bool ProjectTracking
        //////{
        //////    get { return _ProjectTracking; }

        //////    set
        //////    {
        //////        SetPropertyValue("ProjectTracking", ref _ProjectTracking, value);
        //////    }
        //////}
        //////#endregion

        //////#endregion

        //////#region Task Management
        //////#region  TaskManagement
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _TaskManagement = true;
        //////[ImmediatePostData]
        //////public bool TaskManagement
        //////{
        //////    get { return _TaskManagement; }

        //////    set
        //////    {
        //////        SetPropertyValue("TaskManagement", ref _TaskManagement, value);
        //////    }
        //////}
        //////#endregion

        //////#region  ClientRequest
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ClientRequest = true;
        //////[ImmediatePostData]
        //////public bool ClientRequest
        //////{
        //////    get { return _ClientRequest; }

        //////    set
        //////    {
        //////        SetPropertyValue("ClientRequest", ref _ClientRequest, value);
        //////    }
        //////}
        //////#endregion

        //////#region  COC Tracking
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _COCTracking = true;
        //////[ImmediatePostData]
        //////public bool COCTracking
        //////{
        //////    get { return _COCTracking; }

        //////    set
        //////    {
        //////        SetPropertyValue("COCTracking", ref _COCTracking, value);
        //////    }
        //////}
        //////#endregion


        //////#endregion

        //////#region Sample Tracking
        //////#region  SampleManagement
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SampleManagement = true;
        //////[ImmediatePostData]
        //////public bool SampleManagement
        //////{
        //////    get { return _SampleManagement; }

        //////    set
        //////    {
        //////        SetPropertyValue("SampleManagement", ref _SampleManagement, value);
        //////    }
        //////}
        //////#endregion

        //////#region  SampleRegistration
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SampleRegistration = true;
        //////[ImmediatePostData]
        //////public bool SampleRegistration
        //////{
        //////    get { return _SampleRegistration; }

        //////    set
        //////    {
        //////        SetPropertyValue("SampleRegistration", ref _SampleRegistration, value);
        //////    }
        //////}
        //////#endregion

        //////#region Sample_Reconciliation
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Sample_Reconciliation = true;
        //////[ImmediatePostData]
        //////public bool Sample_Reconciliation
        //////{
        //////    get { return _Sample_Reconciliation; }

        //////    set
        //////    {
        //////        SetPropertyValue("Sample_Reconciliation", ref _Sample_Reconciliation, value);
        //////    }
        //////}
        //////#endregion

        //////#region  SampleDisposition
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SampleDisposition = true;
        //////[ImmediatePostData]
        //////public bool SampleDisposition
        //////{
        //////    get { return _SampleDisposition; }

        //////    set
        //////    {
        //////        SetPropertyValue("SampleDisposition", ref _SampleDisposition, value);
        //////    }
        //////}
        //////#endregion


        //////#endregion

        //////#region Inspection
        //////#region  Inspection
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Inspection = true;
        //////[ImmediatePostData]
        //////public bool Inspection
        //////{
        //////    get { return _Inspection; }

        //////    set
        //////    {
        //////        SetPropertyValue("Inspection", ref _Inspection, value);
        //////    }
        //////}
        //////#endregion

        //////#region  IndoorInspection
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _IndoorInspection = true;
        //////[ImmediatePostData]
        //////public bool IndoorInspection
        //////{
        //////    get { return _IndoorInspection; }

        //////    set
        //////    {
        //////        SetPropertyValue("IndoorInspection", ref _IndoorInspection, value);
        //////    }
        //////}
        //////#endregion

        //////#region OutdoorInspection
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _OutdoorInspection = true;
        //////[ImmediatePostData]
        //////public bool OutdoorInspection
        //////{
        //////    get { return _OutdoorInspection; }

        //////    set
        //////    {
        //////        SetPropertyValue("OutdoorInspection", ref _OutdoorInspection, value);
        //////    }
        //////}
        //////#endregion

        //////#region  ProductAndSampleMapping
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ProductAndSampleMapping = true;
        //////[ImmediatePostData]
        //////public bool ProductAndSampleMapping
        //////{
        //////    get { return _ProductAndSampleMapping; }

        //////    set
        //////    {
        //////        SetPropertyValue("ProductAndSampleMapping", ref _ProductAndSampleMapping, value);
        //////    }
        //////}
        //////#endregion


        //////#endregion

        //////#region Sample Preparation
        //////#region  SamplePreparationRootNode
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SamplePreparationRootNode = true;
        //////[ImmediatePostData]
        //////public bool SamplePreparationRootNode
        //////{
        //////    get { return _SamplePreparationRootNode; }

        //////    set
        //////    {
        //////        SetPropertyValue("SamplePreparationRootNode", ref _SamplePreparationRootNode, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Samplepretreatment
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Samplepretreatment = true;
        //////[ImmediatePostData]
        //////public bool Samplepretreatment
        //////{
        //////    get { return _Samplepretreatment; }

        //////    set
        //////    {
        //////        SetPropertyValue("Samplepretreatment", ref _Samplepretreatment, value);
        //////    }
        //////}
        //////#endregion

        //////#region SamplePreparation
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SamplePreparation = true;
        //////[ImmediatePostData]
        //////public bool SamplePreparation
        //////{
        //////    get { return _SamplePreparation; }

        //////    set
        //////    {
        //////        SetPropertyValue("SamplePreparation", ref _SamplePreparation, value);
        //////    }
        //////}
        //////#endregion

        //////#endregion

        //////#region Sample Weighing Module
        //////#region  SampleWeighingRootNode  
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SampleWeighingRootNode = true;
        //////[ImmediatePostData]
        //////public bool SampleWeighingRootNode
        //////{
        //////    get { return _SampleWeighingRootNode; }

        //////    set
        //////    {
        //////        SetPropertyValue("SampleWeighingRootNode ", ref _SampleWeighingRootNode, value);
        //////    }
        //////}
        //////#endregion

        //////#region  SampleWeighing
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SampleWeighing = true;
        //////[ImmediatePostData]
        //////public bool SampleWeighing
        //////{
        //////    get { return _SampleWeighing; }

        //////    set
        //////    {
        //////        SetPropertyValue("SampleWeighing", ref _SampleWeighing, value);
        //////    }
        //////}
        //////#endregion

        //////#region Sample Weighing Tracking
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SampleWeighingTracking = true;
        //////[ImmediatePostData]
        //////public bool SampleWeighingTracking
        //////{
        //////    get { return _SampleWeighingTracking; }

        //////    set
        //////    {
        //////        SetPropertyValue("SampleWeighingTracking", ref _SampleWeighingTracking, value);
        //////    }
        //////}
        //////#endregion

        //////#endregion

        //////#region Analysis
        //////#region  DataEntry  
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _DataEntry = true;
        //////[ImmediatePostData]
        //////public bool DataEntry
        //////{
        //////    get { return _DataEntry; }

        //////    set
        //////    {
        //////        SetPropertyValue("DataEntry ", ref _DataEntry, value);
        //////    }
        //////}
        //////#endregion

        //////#region  AnalysisQueue 
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _AnalysisQueue = true;
        //////[ImmediatePostData]
        //////public bool AnalysisQueue
        //////{
        //////    get { return _AnalysisQueue; }

        //////    set
        //////    {
        //////        SetPropertyValue("AnalysisQueue ", ref _AnalysisQueue, value);
        //////    }
        //////}
        //////#endregion

        //////#region Analyticalbatch
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Analyticalbatch = true;
        //////[ImmediatePostData]
        //////public bool Analyticalbatch
        //////{
        //////    get { return _Analyticalbatch; }

        //////    set
        //////    {
        //////        SetPropertyValue("Analyticalbatch", ref _Analyticalbatch, value);
        //////    }
        //////}
        //////#endregion

        //////#region QCbatches
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _QCbatches = true;
        //////[ImmediatePostData]
        //////public bool QCbatches
        //////{
        //////    get { return _QCbatches; }

        //////    set
        //////    {
        //////        SetPropertyValue("QCbatches", ref _QCbatches, value);
        //////    }
        //////}
        //////#endregion

        //////#region SDMS
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Spreadsheet = true;
        //////[ImmediatePostData]
        //////public bool Spreadsheet
        //////{
        //////    get { return _Spreadsheet; }

        //////    set
        //////    {
        //////        SetPropertyValue("Spreadsheet", ref _Spreadsheet, value);
        //////    }
        //////}
        //////#endregion

        //////#region Result Entry Express
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ResultEntry = true;
        //////[ImmediatePostData]
        //////public bool ResultEntry
        //////{
        //////    get { return _ResultEntry; }

        //////    set
        //////    {
        //////        SetPropertyValue("ResultEntry", ref _ResultEntry, value);
        //////    }
        //////}
        //////#endregion

        //////#endregion

        //////#region Data Review
        //////#region  DataReview  
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _DataReview = true;
        //////[ImmediatePostData]
        //////public bool DataReview
        //////{
        //////    get { return _DataReview; }

        //////    set
        //////    {
        //////        SetPropertyValue("DataReview ", ref _DataReview, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Level 1 Data Review
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ResultValidation = true;
        //////[ImmediatePostData]
        //////public bool ResultValidation
        //////{
        //////    get { return _ResultValidation; }

        //////    set
        //////    {
        //////        SetPropertyValue("ResultValidation", ref _ResultValidation, value);
        //////    }
        //////}
        //////#endregion

        //////#region Level 2 Data Review
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ResultApproval = true;
        //////[ImmediatePostData]
        //////public bool ResultApproval
        //////{
        //////    get { return _ResultApproval; }

        //////    set
        //////    {
        //////        SetPropertyValue("ResultApproval", ref _ResultApproval, value);
        //////    }
        //////}
        //////#endregion

        //////#endregion

        //////#region Reporting
        //////#region  Reporting
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Reporting = true;
        //////[ImmediatePostData]
        //////public bool Reporting
        //////{
        //////    get { return _Reporting; }

        //////    set
        //////    {
        //////        SetPropertyValue("Reporting", ref _Reporting, value);
        //////    }
        //////}
        //////#endregion

        //////#region Custom Reporting
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _CustomReporting = true;
        //////[ImmediatePostData]
        //////public bool CustomReporting
        //////{
        //////    get { return _CustomReporting; }

        //////    set
        //////    {
        //////        SetPropertyValue("CustomReporting", ref _ClientRequest, value);
        //////    }
        //////}
        //////#endregion

        //////#region  ReportValidation
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ReportValidation = true;
        //////[ImmediatePostData]
        //////public bool ReportValidation
        //////{
        //////    get { return _ReportValidation; }

        //////    set
        //////    {
        //////        SetPropertyValue("ReportValidation", ref _ReportValidation, value);
        //////    }
        //////}
        //////#endregion




        //////#region  ReportPrintDownload
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ReportPrintDownload = true;
        //////[ImmediatePostData]
        //////public bool ReportPrintDownload
        //////{
        //////    get { return _ReportPrintDownload; }

        //////    set
        //////    {
        //////        SetPropertyValue("ReportPrintDownload", ref _ReportPrintDownload, value);
        //////    }
        //////}
        //////#endregion


        //////#region  ReportDelivery
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ReportDelivery = true;
        //////[ImmediatePostData]
        //////public bool ReportDelivery
        //////{
        //////    get { return _ReportDelivery; }

        //////    set
        //////    {
        //////        SetPropertyValue("ReportDelivery", ref _ReportDelivery, value);
        //////    }
        //////}
        //////#endregion
        //////#endregion

        //////#region Maintenance
        //////#region  Maintenance
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Maintenance = true;
        //////[ImmediatePostData]
        //////public bool Maintenance
        //////{
        //////    get { return _Maintenance; }

        //////    set
        //////    {
        //////        SetPropertyValue("Maintenance", ref _Maintenance, value);
        //////    }
        //////}
        //////#endregion

        //////#region Clients
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Customer = true;
        //////[ImmediatePostData]
        //////public bool Customer
        //////{
        //////    get { return _Customer; }

        //////    set
        //////    {
        //////        SetPropertyValue("Customer", ref _Customer, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Client
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Customer_ListView = true;
        //////[ImmediatePostData]
        //////public bool Customer_ListView
        //////{
        //////    get { return _Customer_ListView; }

        //////    set
        //////    {
        //////        SetPropertyValue("Customer_ListView", ref _Customer_ListView, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Contacts
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Contact_ListView = true;
        //////[ImmediatePostData]
        //////public bool Contact_ListView
        //////{
        //////    get { return _Contact_ListView; }

        //////    set
        //////    {
        //////        SetPropertyValue("Contact_ListView", ref _Contact_ListView, value);
        //////    }
        //////}
        //////#endregion


        //////#region  Project
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Project = true;
        //////[ImmediatePostData]
        //////public bool Project
        //////{
        //////    get { return _Project; }

        //////    set
        //////    {
        //////        SetPropertyValue("Project", ref _Project, value);
        //////    }
        //////}
        //////#endregion


        //////#region  HumanResources
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _HumanResources = true;
        //////[ImmediatePostData]
        //////public bool HumanResources
        //////{
        //////    get { return _HumanResources; }

        //////    set
        //////    {
        //////        SetPropertyValue("HumanResources", ref _HumanResources, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Employee
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Employee_ListView = true;
        //////[ImmediatePostData]
        //////public bool Employee_ListView
        //////{
        //////    get { return _Employee_ListView; }

        //////    set
        //////    {
        //////        SetPropertyValue("Employee_ListView", ref _Employee_ListView, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Labware
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Assets = true;
        //////[ImmediatePostData]
        //////public bool Assets
        //////{
        //////    get { return _Assets; }

        //////    set
        //////    {
        //////        SetPropertyValue("Assets", ref _Assets, value);
        //////    }
        //////}
        //////#endregion
        //////#region  LABWARE
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Labware_ListView = true;
        //////[ImmediatePostData]
        //////public bool Labware_ListView
        //////{
        //////    get { return _Labware_ListView; }

        //////    set
        //////    {
        //////        SetPropertyValue("Labware_ListView", ref _Labware_ListView, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Labware Certificate
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _LabwareCertificate_ListView = true;
        //////[ImmediatePostData]
        //////public bool LabwareCertificate_ListView
        //////{
        //////    get { return _LabwareCertificate_ListView; }

        //////    set
        //////    {
        //////        SetPropertyValue("LabwareCertificate_ListView", ref _LabwareCertificate_ListView, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Labware Maintenance
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _LabwareMaintenance_ListView = true;
        //////[ImmediatePostData]
        //////public bool LabwareMaintenance_ListView
        //////{
        //////    get { return _LabwareMaintenance_ListView; }

        //////    set
        //////    {
        //////        SetPropertyValue("LabwareMaintenance_ListView", ref _LabwareMaintenance_ListView, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Security
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _System = true;
        //////[ImmediatePostData]
        //////public bool System
        //////{
        //////    get { return _System; }

        //////    set
        //////    {
        //////        SetPropertyValue("System", ref _System, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Role
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Role = true;
        //////[ImmediatePostData]
        //////public bool Role
        //////{
        //////    get { return _Role; }

        //////    set
        //////    {
        //////        SetPropertyValue("Role", ref _Role, value);
        //////    }
        //////}
        //////#endregion
        //////#region  My Details
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _MyDetails = true;
        //////[ImmediatePostData]
        //////public bool MyDetails
        //////{
        //////    get { return _MyDetails; }

        //////    set
        //////    {
        //////        SetPropertyValue("MyDetails", ref _MyDetails, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Sample Preparation Chain
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SamplePreparationChain = true;
        //////[ImmediatePostData]
        //////public bool SamplePreparationChain
        //////{
        //////    get { return _SamplePreparationChain; }

        //////    set
        //////    {
        //////        SetPropertyValue("SamplePreparationChain", ref _SamplePreparationChain, value);
        //////    }
        //////}
        //////#endregion
        //////#endregion

        //////#region Audit Trail
        //////#region  AuditTrail  
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _AuditTrail = true;
        //////[ImmediatePostData]
        //////public bool AuditTrail
        //////{
        //////    get { return _AuditTrail; }

        //////    set
        //////    {
        //////        SetPropertyValue("AuditTrail ", ref _AuditTrail, value);
        //////    }
        //////}
        //////#endregion

        //////#region  GlobalAuditTrail 
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _GlobalAuditTrail = true;
        //////[ImmediatePostData]
        //////public bool GlobalAuditTrail
        //////{
        //////    get { return _GlobalAuditTrail; }

        //////    set
        //////    {
        //////        SetPropertyValue("GlobalAuditTrail ", ref _GlobalAuditTrail, value);
        //////    }
        //////}
        //////#endregion

        //////#region Sample LogIn Audit Trail
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SampleLogInAuditTrail = true;
        //////[ImmediatePostData]
        //////public bool SampleLogInAuditTrail
        //////{
        //////    get { return _SampleLogInAuditTrail; }

        //////    set
        //////    {
        //////        SetPropertyValue("SampleLogInAuditTrail", ref _SampleLogInAuditTrail, value);
        //////    }
        //////}
        //////#endregion

        //////#region Sample CheckIn Audit Trail
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SampleCheckInAuditTrail = true;
        //////[ImmediatePostData]
        //////public bool SampleCheckInAuditTrail
        //////{
        //////    get { return _SampleCheckInAuditTrail; }

        //////    set
        //////    {
        //////        SetPropertyValue("SampleCheckInAuditTrail", ref _SampleCheckInAuditTrail, value);
        //////    }
        //////}
        //////#endregion

        //////#region SDMSRollback
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SDMSRollback = true;
        //////[ImmediatePostData]
        //////public bool SDMSRollback
        //////{
        //////    get { return _SDMSRollback; }

        //////    set
        //////    {
        //////        SetPropertyValue("SDMSRollback", ref _SDMSRollback, value);
        //////    }
        //////}
        //////#endregion

        //////#region Test Parameter 
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _TestParameter = true;
        //////[ImmediatePostData]
        //////public bool TestParameter
        //////{
        //////    get { return _TestParameter; }

        //////    set
        //////    {
        //////        SetPropertyValue("TestParameter", ref _TestParameter, value);
        //////    }
        //////}
        //////#endregion
        //////#endregion

        //////#region Settings
        //////#region  Settings
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Settings = true;
        //////[ImmediatePostData]
        //////public bool Settings
        //////{
        //////    get { return _Settings; }

        //////    set
        //////    {
        //////        SetPropertyValue("Settings", ref _Settings, value);
        //////    }
        //////}
        //////#endregion

        //////#region Dashboard
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Dashboard = true;
        //////[ImmediatePostData]
        //////public bool Dashboard
        //////{
        //////    get { return _Dashboard; }

        //////    set
        //////    {
        //////        SetPropertyValue("Dashboard", ref _Dashboard, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Dashboard Designer
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _DashboardDesigner = true;
        //////[ImmediatePostData]
        //////public bool DashboardDesigner
        //////{
        //////    get { return _DashboardDesigner; }

        //////    set
        //////    {
        //////        SetPropertyValue("DashboardDesigner", ref _DashboardDesigner, value);
        //////    }
        //////}
        //////#endregion

        //////#region  AssignDashboardToUser
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _AssignDashboardToUser = true;
        //////[ImmediatePostData]
        //////public bool AssignDashboardToUser
        //////{
        //////    get { return _AssignDashboardToUser; }

        //////    set
        //////    {
        //////        SetPropertyValue("AssignDashboardToUser", ref _AssignDashboardToUser, value);
        //////    }
        //////}
        //////#endregion


        //////#region  SystemManagement
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _SystemManagement = true;
        //////[ImmediatePostData]
        //////public bool SystemManagement
        //////{
        //////    get { return _SystemManagement; }

        //////    set
        //////    {
        //////        SetPropertyValue("SystemManagement", ref _SystemManagement, value);
        //////    }
        //////}
        //////#endregion


        //////#region  Company
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Company = true;
        //////[ImmediatePostData]
        //////public bool Company
        //////{
        //////    get { return _Company; }

        //////    set
        //////    {
        //////        SetPropertyValue("Company", ref _Company, value);
        //////    }
        //////}
        //////#endregion
        //////#region  City
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _City = true;
        //////[ImmediatePostData]
        //////public bool City
        //////{
        //////    get { return _City; }

        //////    set
        //////    {
        //////        SetPropertyValue("City", ref _City, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Country
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Country = true;
        //////[ImmediatePostData]
        //////public bool Country
        //////{
        //////    get { return _Country; }

        //////    set
        //////    {
        //////        SetPropertyValue("Country", ref _Country, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Department
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Department = true;
        //////[ImmediatePostData]
        //////public bool Department
        //////{
        //////    get { return _Department; }

        //////    set
        //////    {
        //////        SetPropertyValue("Department", ref _Department, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Language
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Language = true;
        //////[ImmediatePostData]
        //////public bool Language
        //////{
        //////    get { return _Language; }

        //////    set
        //////    {
        //////        SetPropertyValue("Language", ref _Language, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Position
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Position = true;
        //////[ImmediatePostData]
        //////public bool Position
        //////{
        //////    get { return _Position; }

        //////    set
        //////    {
        //////        SetPropertyValue("Position", ref _Position, value);
        //////    }
        //////}
        //////#endregion
        //////#region  State
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _State = true;
        //////[ImmediatePostData]
        //////public bool State
        //////{
        //////    get { return _State; }

        //////    set
        //////    {
        //////        SetPropertyValue("State", ref _State, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Glossary
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Glossary = true;
        //////[ImmediatePostData]
        //////public bool Glossary
        //////{
        //////    get { return _Glossary; }

        //////    set
        //////    {
        //////        SetPropertyValue("Glossary", ref _Glossary, value);
        //////    }
        //////}
        //////#endregion

        //////#region  E-mail
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Email = true;
        //////[ImmediatePostData]
        //////public bool Email
        //////{
        //////    get { return _Email; }

        //////    set
        //////    {
        //////        SetPropertyValue("Email", ref _Email, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Email
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _E_mail = true;
        //////[ImmediatePostData]
        //////public bool E_mail
        //////{
        //////    get { return _E_mail; }

        //////    set
        //////    {
        //////        SetPropertyValue("E_mail", ref _E_mail, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Method
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Method = true;
        //////[ImmediatePostData]
        //////public bool Method
        //////{
        //////    get { return _Method; }

        //////    set
        //////    {
        //////        SetPropertyValue("Method", ref _Method, value);
        //////    }
        //////}
        //////#endregion

        //////#region  MethodCategory
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _MethodCategory = true;
        //////[ImmediatePostData]
        //////public bool MethodCategory
        //////{
        //////    get { return _MethodCategory; }

        //////    set
        //////    {
        //////        SetPropertyValue("MethodCategory", ref _MethodCategory, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Parameter Library
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Parameter = true;
        //////[ImmediatePostData]
        //////public bool Parameter
        //////{
        //////    get { return _Parameter; }

        //////    set
        //////    {
        //////        SetPropertyValue("Parameter", ref _Parameter, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Tests
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Tests = true;
        //////[ImmediatePostData]
        //////public bool Tests
        //////{
        //////    get { return _Tests; }

        //////    set
        //////    {
        //////        SetPropertyValue("Tests", ref _Tests, value);
        //////    }
        //////}
        //////#endregion
        //////#region  Parameter Default
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _TestParameterDefaultSetup = true;
        //////[ImmediatePostData]
        //////public bool TestParameterDefaultSetup
        //////{
        //////    get { return _TestParameterDefaultSetup; }

        //////    set
        //////    {
        //////        SetPropertyValue("TestParameterDefaultSetup", ref _TestParameterDefaultSetup, value);
        //////    }
        //////}
        //////#endregion

        //////#region  ReportPackage
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ReportPackage = true;
        //////[ImmediatePostData]
        //////public bool ReportPackage
        //////{
        //////    get { return _ReportPackage; }

        //////    set
        //////    {
        //////        SetPropertyValue("ReportPackage", ref _ReportPackage, value);
        //////    }
        //////}
        //////#endregion

        //////#region  COCSettings
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _COCSettings = true;
        //////[ImmediatePostData]
        //////public bool COCSettings
        //////{
        //////    get { return _COCSettings; }

        //////    set
        //////    {
        //////        SetPropertyValue("COCSettings", ref _COCSettings, value);
        //////    }
        //////}
        //////#endregion

        //////#region  ClauseSettings
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ClauseSettings = true;
        //////[ImmediatePostData]
        //////public bool ClauseSettings
        //////{
        //////    get { return _ClauseSettings; }

        //////    set
        //////    {
        //////        SetPropertyValue("ClauseSettings", ref _ClauseSettings, value);
        //////    }
        //////}
        //////#endregion

        //////#region  AnalysisDepartmentChain
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _AnalysisDepartmentChain = true;
        //////[ImmediatePostData]
        //////public bool AnalysisDepartmentChain
        //////{
        //////    get { return _AnalysisDepartmentChain; }

        //////    set
        //////    {
        //////        SetPropertyValue("AnalysisDepartmentChain", ref _AnalysisDepartmentChain, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Roundoff
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Roundoff = true;
        //////[ImmediatePostData]
        //////public bool Roundoff
        //////{
        //////    get { return _Roundoff; }

        //////    set
        //////    {
        //////        SetPropertyValue("Roundoff", ref _Roundoff, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Reagent
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Reagent = true;
        //////[ImmediatePostData]
        //////public bool Reagent
        //////{
        //////    get { return _Reagent; }

        //////    set
        //////    {
        //////        SetPropertyValue("Reagent", ref _Reagent, value);
        //////    }
        //////}
        //////#endregion

        //////#region  Holidays
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Holidays = true;
        //////[ImmediatePostData]
        //////public bool Holidays
        //////{
        //////    get { return _Holidays; }

        //////    set
        //////    {
        //////        SetPropertyValue("Holidays", ref _Holidays, value);
        //////    }
        //////}
        //////#endregion

        //////#region  ProjectCategory
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _ProjectCategoryt = true;
        //////[ImmediatePostData]
        //////public bool ProjectCategory
        //////{
        //////    get { return _ProjectCategoryt; }

        //////    set
        //////    {
        //////        SetPropertyValue("ProjectCategory", ref _ProjectCategoryt, value);
        //////    }
        //////}
        //////#endregion

        //////#region  TurnAroundTime
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _TurnAroundTime = true;
        //////[ImmediatePostData]
        //////public bool TurnAroundTime
        //////{
        //////    get { return _TurnAroundTime; }

        //////    set
        //////    {
        //////        SetPropertyValue("TurnAroundTime", ref _TurnAroundTime, value);
        //////    }
        //////}
        //////#endregion

        ////////#region  Reagent
        //////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////////private bool _Reagent = true;
        ////////[ImmediatePostData]
        ////////public bool Reagent
        ////////{
        ////////    get { return _Reagent; }

        ////////    set
        ////////    {
        ////////        SetPropertyValue("Reagent", ref _Reagent, value);
        ////////    }
        ////////}

        ////////#endregion

        //////#endregion



        //////#endregion




        ////#region SDMS
        ////private EnumRELevelSetup _SDMS = EnumRELevelSetup.No;
        ////[ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        ////[ImmediatePostData]
        ////public EnumRELevelSetup SDMS
        ////{
        ////    get { return _SDMS; }

        ////    set
        ////    {
        ////        SetPropertyValue("SDMS", ref _SDMS, value);
        ////        //if (value == EnumRELevelSetup.No)
        ////        //{
        ////        //    _Review = EnumRELevelSetup.No;
        ////        //    _Verify = EnumRELevelSetup.No;
        ////        //    _REReviewValidate = EnumRELevelSetup.No;
        ////        //    _REVerifyApprove = EnumRELevelSetup.No;
        ////        //}
        ////    }
        ////}
        ////#endregion


        ////#region ContractEvaluation
        ////private EnumRELevelSetup _ContractEvaluation = EnumRELevelSetup.Yes;
        ////[ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        ////[ImmediatePostData]
        ////public EnumRELevelSetup ContractEvaluation
        ////{
        ////    get { return _ContractEvaluation; }
        ////    //set { SetPropertyValue("Review", ref _Review, value); }
        ////    set
        ////    {
        ////        SetPropertyValue("ContractEvaluation", ref _ContractEvaluation, value);
        ////    }
        ////}
        ////#endregion





        ////#endregion

        #region ReportValidate
        private EnumRELevelSetup _ReportValidate = EnumRELevelSetup.Yes;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumRELevelSetup ReportValidate
        {
            get { return _ReportValidate; }
            set { SetPropertyValue("ReportValidate", ref _ReportValidate, value); }
        }
        #endregion
        #region ReportApprove
        private EnumRELevelSetup _ReportApprove = EnumRELevelSetup.Yes;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumRELevelSetup ReportApprove
        {
            get { return _ReportApprove; }
            set { SetPropertyValue("ReportApprove", ref _ReportApprove, value); }
        }
        #endregion

        ////#region Inventory Control Manager 

        ////#region InventoryManagement
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _InventoryManagement = true;
        ////[ImmediatePostData]
        ////public bool InventoryManagement
        ////{
        ////    get { return _InventoryManagement; }

        ////    set
        ////    {
        ////        SetPropertyValue("InventoryManagement", ref _InventoryManagement, value);
        ////    }
        ////}
        ////#endregion

        ////#region Operations
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Operations = true;
        ////[ImmediatePostData]
        ////public bool Operations
        ////{
        ////    get { return _Operations; }

        ////    set
        ////    {
        ////        SetPropertyValue("Operations", ref _Operations, value);
        ////    }
        ////}
        ////#endregion

        ////#region FlowControl
        ////private ICMOperationsFlow _FlowControl;
        ////[ImmediatePostData]
        ////public ICMOperationsFlow FlowControl
        ////{
        ////    get
        ////    {
        ////        return _FlowControl;
        ////    }
        ////    set
        ////    {
        ////        SetPropertyValue<ICMOperationsFlow>(nameof(FlowControl), ref _FlowControl, value);
        ////        if (value == ICMOperationsFlow.WithPurchasing)
        ////        {
        ////            Requisition = true;
        ////            //Review = true;
        ////            OrderingItem = true;
        ////            Receiving = true;
        ////            Distribution = true;
        ////        }
        ////    }
        ////}
        ////#endregion

        ////#region Item Requisition
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Requisition = true;
        ////[ImmediatePostData]
        ////public bool Requisition
        ////{
        ////    get { return _Requisition; }

        ////    set
        ////    {
        ////        SetPropertyValue("Requisition", ref _Requisition, value);
        ////    }
        ////}
        ////#endregion
        //////#region Requisition Review
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _Review = true;
        //////[ImmediatePostData]
        //////public bool Review
        //////{
        //////    get { return _Review; }

        //////    set
        //////    {
        //////        SetPropertyValue("Review", ref _Review, value);
        //////    }
        //////}
        //////#endregion
        #region RequisitionApproval
        //private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        private bool _RequisitionApproval = true;
        [ImmediatePostData]
        public bool RequisitionApproval
        {
            get { return _RequisitionApproval; }

            set
            {
                SetPropertyValue("RequisitionApproval", ref _RequisitionApproval, value);
            }
        }
        #endregion

        #region  NoofLevels
        private int _NoofLevels;
        [RuleRange(0, 10)]
        public int NoofLevels
        {
            get { return _NoofLevels; }

            set
            {
                SetPropertyValue("NoofLevels", ref _NoofLevels, value);
            }
        }
        #endregion

        ////#region OrderingItem
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _OrderingItem = true;
        ////[ImmediatePostData]
        ////public bool OrderingItem
        ////{
        ////    get { return _OrderingItem; }

        ////    set
        ////    {
        ////        SetPropertyValue("OrderingItem", ref _OrderingItem, value);
        ////    }
        ////}
        ////#endregion

        ////#region Receiving Items
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Receiving = true;
        ////[ImmediatePostData]
        ////public bool Receiving
        ////{
        ////    get { return _Receiving; }

        ////    set
        ////    {
        ////        SetPropertyValue("Receiving", ref _Receiving, value);
        ////    }
        ////}
        ////#endregion

        ////#region Item Distribution
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Distribution = true;
        ////[ImmediatePostData]
        ////public bool Distribution
        ////{
        ////    get { return _Distribution; }

        ////    set
        ////    {
        ////        SetPropertyValue("Distribution", ref _Distribution, value);
        ////    }
        ////}
        ////#endregion
        ////#region Direct Receiving Items
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _ReceiveOrderDirect = true;
        ////[ImmediatePostData]
        ////public bool ReceiveOrderDirect
        ////{
        ////    get { return _ReceiveOrderDirect; }

        ////    set
        ////    {
        ////        SetPropertyValue("ReceiveOrderDirect", ref _ReceiveOrderDirect, value);
        ////    }
        ////}
        ////#endregion
        ////#region Testing
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Testing = true;
        ////[ImmediatePostData]
        ////public bool Testing
        ////{
        ////    get { return _Testing; }

        ////    set
        ////    {
        ////        SetPropertyValue("Testing", ref _Testing, value);
        ////    }
        ////}
        ////#endregion
        ////#region Item Consumption
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Consumption = true;
        ////[ImmediatePostData]
        ////public bool Consumption
        ////{
        ////    get { return _Consumption; }

        ////    set
        ////    {
        ////        SetPropertyValue("Consumption", ref _Consumption, value);
        ////    }
        ////}
        ////#endregion
        ////#region Vendor Reagent Certificate
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _VendorReagentCertificate = true;
        ////[ImmediatePostData]
        ////public bool VendorReagentCertificate
        ////{
        ////    get { return _VendorReagentCertificate; }

        ////    set
        ////    {
        ////        SetPropertyValue("VendorReagentCertificate", ref _VendorReagentCertificate, value);
        ////    }
        ////}
        ////#endregion
        ////#region Item Disposal
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Disposal = true;
        ////[ImmediatePostData]
        ////public bool Disposal
        ////{
        ////    get { return _Disposal; }

        ////    set
        ////    {
        ////        SetPropertyValue("Disposal", ref _Disposal, value);
        ////    }
        ////}
        ////#endregion
        ////#region Certificate of Analysis 
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _COA = true;
        ////[ImmediatePostData]
        ////public bool COA
        ////{
        ////    get { return _COA; }

        ////    set
        ////    {
        ////        SetPropertyValue("COA", ref _COA, value);
        ////    }
        ////}
        ////#endregion

        ////#region Cancelled Requisition
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _CancelledRequisition = true;
        ////[ImmediatePostData]
        ////public bool CancelledRequisition
        ////{
        ////    get { return _CancelledRequisition; }

        ////    set
        ////    {
        ////        SetPropertyValue("CancelledRequisition", ref _CancelledRequisition, value);
        ////    }
        ////}
        ////#endregion

        ////#region Inventory
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Inventory = true;
        ////[ImmediatePostData]
        ////public bool Inventory
        ////{
        ////    get { return _Inventory; }

        ////    set
        ////    {
        ////        SetPropertyValue("Inventory", ref _Inventory, value);
        ////    }
        ////}
        ////#endregion

        ////#region ASIProductStockInventory
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _ASIProductStockInventory = true;
        ////[ImmediatePostData]
        ////public bool ASIProductStockInventory
        ////{
        ////    get { return _ASIProductStockInventory; }

        ////    set
        ////    {
        ////        SetPropertyValue("ASIProductStockInventory", ref _ASIProductStockInventory, value);
        ////    }
        ////}
        ////#endregion

        ////#region ItemStockInventory 
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _ItemStockInventory = true;
        ////[ImmediatePostData]
        ////public bool ItemStockInventory
        ////{
        ////    get { return _ItemStockInventory; }

        ////    set
        ////    {
        ////        SetPropertyValue("ItemStockInventory ", ref _ItemStockInventory, value);
        ////    }
        ////}
        ////#endregion
        ////#region ResaleProductStockInventory
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _ResaleProductStockInventory = true;
        ////[ImmediatePostData]
        ////public bool ResaleProductStockInventory
        ////{
        ////    get { return _ResaleProductStockInventory; }

        ////    set
        ////    {
        ////        SetPropertyValue("ResaleProductStockInventory", ref _ResaleProductStockInventory, value);
        ////    }
        ////}
        ////#endregion
        ////#region Alert
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Alert = true;
        ////[ImmediatePostData]
        ////public bool Alert
        ////{
        ////    get { return _Alert; }

        ////    set
        ////    {
        ////        SetPropertyValue("Alert", ref _Alert, value);
        ////    }
        ////}
        ////#endregion
        ////#region Expiration
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Expiration = true;
        ////[ImmediatePostData]
        ////public bool Expiration
        ////{
        ////    get { return _Expiration; }

        ////    set
        ////    {
        ////        SetPropertyValue("Expiration", ref _Expiration, value);
        ////    }
        ////}
        ////#endregion

        ////#region StockAlert
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _StockAlert = true;
        ////[ImmediatePostData]
        ////public bool StockAlert
        ////{
        ////    get { return _StockAlert; }

        ////    set
        ////    {
        ////        SetPropertyValue("StockAlert", ref _StockAlert, value);
        ////    }
        ////}
        ////#endregion

        //////#region StockInventory
        ////////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        //////private bool _StockInventory = true;
        //////[ImmediatePostData]
        //////public bool StockInventory
        //////{
        //////    get { return _Distribution; }

        //////    set
        //////    {
        //////        SetPropertyValue("StockInventory", ref _Distribution, value);
        //////    }
        //////}
        //////#endregion
        ////#region Stock Watch
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _StockWatch = true;
        ////[ImmediatePostData]
        ////public bool StockWatch
        ////{
        ////    get { return _StockWatch; }

        ////    set
        ////    {
        ////        SetPropertyValue("StockWatch", ref _StockWatch, value);
        ////    }
        ////}
        ////#endregion

        ////#region BasicSettings
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _BasicSettings = true;
        ////[ImmediatePostData]
        ////public bool BasicSettings
        ////{
        ////    get { return _BasicSettings; }

        ////    set
        ////    {
        ////        SetPropertyValue("BasicSettings", ref _BasicSettings, value);
        ////    }
        ////}
        ////#endregion
        ////#region Items
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Items = true;
        ////[ImmediatePostData]
        ////public bool Items
        ////{
        ////    get { return _Items; }

        ////    set
        ////    {
        ////        SetPropertyValue("Items", ref _Items, value);
        ////    }
        ////}
        ////#endregion
        ////#region Vendors
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Vendors = true;
        ////[ImmediatePostData]
        ////public bool Vendors
        ////{
        ////    get { return _Vendors; }

        ////    set
        ////    {
        ////        SetPropertyValue("Vendors", ref _Vendors, value);
        ////    }
        ////}
        ////#endregion
        ////#region VendorEvaluationItems
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _VendorEvaluationItems = true;
        ////[ImmediatePostData]
        ////public bool VendorEvaluationItems
        ////{
        ////    get { return _VendorEvaluationItems; }

        ////    set
        ////    {
        ////        SetPropertyValue("VendorEvaluationItems", ref _VendorEvaluationItems, value);
        ////    }
        ////}
        ////#endregion
        ////#region DeliveryPriority
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _DeliveryPriority = true;
        ////[ImmediatePostData]
        ////public bool DeliveryPriority
        ////{
        ////    get { return _DeliveryPriority; }

        ////    set
        ////    {
        ////        SetPropertyValue("DeliveryPriority", ref _DeliveryPriority, value);
        ////    }
        ////}
        ////#endregion
        ////#region Grade
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Grade = true;
        ////[ImmediatePostData]
        ////public bool Grade
        ////{
        ////    get { return _Grade; }

        ////    set
        ////    {
        ////        SetPropertyValue("Grade", ref _Grade, value);
        ////    }
        ////}
        ////#endregion
        ////#region Item Categories
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Category = true;
        ////[ImmediatePostData]
        ////public bool Category
        ////{
        ////    get { return _Category; }

        ////    set
        ////    {
        ////        SetPropertyValue("Category", ref _Category, value);
        ////    }
        ////}
        ////#endregion

        ////#region Manufacturers
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Manufacturers = true;
        ////[ImmediatePostData]
        ////public bool Manufacturers
        ////{
        ////    get { return _Manufacturers; }

        ////    set
        ////    {
        ////        SetPropertyValue("Manufacturers", ref _Manufacturers, value);
        ////    }
        ////}
        ////#endregion

        ////#region Packageunits
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Packageunits = true;
        ////[ImmediatePostData]
        ////public bool Packageunits
        ////{
        ////    get { return _Packageunits; }

        ////    set
        ////    {
        ////        SetPropertyValue("Packageunits", ref _Packageunits, value);
        ////    }
        ////}
        ////#endregion

        ////#region ShippingOptions
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _ShippingOptions = true;
        ////[ImmediatePostData]
        ////public bool ShippingOptions
        ////{
        ////    get { return _ShippingOptions; }

        ////    set
        ////    {
        ////        SetPropertyValue("ShippingOptions", ref _ShippingOptions, value);
        ////    }
        ////}
        ////#endregion

        ////#region ShipVia
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _ShipVia = true;
        ////[ImmediatePostData]
        ////public bool ShipVia
        ////{
        ////    get { return _ShipVia; }

        ////    set
        ////    {
        ////        SetPropertyValue("ShipVia", ref _ShipVia, value);
        ////    }
        ////}
        ////#endregion

        ////#region Storage
        //////private EnumRELevelSetup _SDMS = EnumRELevelSetup.Off;
        ////private bool _Storage = true;
        ////[ImmediatePostData]
        ////public bool Storage
        ////{
        ////    get { return _Storage; }

        ////    set
        ////    {
        ////        SetPropertyValue("Storage", ref _Storage, value);
        ////    }
        ////}
        ////#endregion

        ////#endregion

        #region  NavigationItemName
        private string _NavigationItemName;
        public string NavigationItemName
        {
            get { return _NavigationItemName; }

            set
            {
                SetPropertyValue("NavigationItemName", ref _NavigationItemName, value);
            }
        }
        #endregion

        #region  NavigationItemNameID
        private string _NavigationItemNameID;
        public string NavigationItemNameID
        {
            get { return _NavigationItemNameID; }

            set
            {
                SetPropertyValue("NavigationItemNameID", ref _NavigationItemNameID, value);
            }
        }
        #endregion

        #region  ModuleNameID
        private string _ModuleNameID;
        public string ModuleNameID
        {
            get { return _ModuleNameID; }

            set
            {
                SetPropertyValue("ModuleNameID", ref _ModuleNameID, value);
            }
        }
        #endregion

        #region  ModuleName
        private string _ModuleName;
        public string ModuleName
        {
            get { return _ModuleName; }

            set
            {
                SetPropertyValue("ModuleName", ref _ModuleName, value);
            }
        }
        #endregion

        #region  IsModule
        private bool _IsModule;
        public bool IsModule
        {
            get { return _IsModule; }

            set
            {
                SetPropertyValue("IsModule", ref _IsModule, value);
            }
        }
        #endregion

        #region  Select
        private bool _Select;
        [ImmediatePostData]
        public bool Select
        {
            get { return _Select; }

            set
            {
                SetPropertyValue("Select", ref _Select, value);
            }
        }
        #endregion

        #region  Mandatory
        private bool _Mandatory;
        [ImmediatePostData]
        public bool Mandatory
        {
            get { return _Mandatory; }

            set
            {
                SetPropertyValue("Mandatory", ref _Mandatory, value);
                if (Mandatory)
                {
                    Select = true;
                }
            }
        }
        #endregion

        #region  Sort
        private string _Sort;
        public string Sort
        {
            get { return _Sort; }

            set
            {
                SetPropertyValue("Sort", ref _Sort, value);
            }
        }
        #endregion

        #region  SortIndex
        private int _SortIndex;
        public int SortIndex
        {
            get { return _SortIndex; }

            set
            {
                SetPropertyValue("SortIndex", ref _SortIndex, value);
            }
        }
        #endregion

        ////#region STValidate
        ////private EnumRELevelSetup _STValidate = EnumRELevelSetup.Yes;
        ////[ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        ////[ImmediatePostData]
        ////public EnumRELevelSetup STValidate
        ////{
        ////    get { return _STValidate; }
        ////    //set { SetPropertyValue("Review", ref _Review, value); }
        ////    set
        ////    {
        ////        SetPropertyValue("STValidate", ref _STValidate, value);
        ////    }
        ////}
        ////#endregion

        ////#region STApprove
        ////private EnumRELevelSetup _STApprove = EnumRELevelSetup.Yes;
        ////[ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        ////[ImmediatePostData]
        ////public EnumRELevelSetup STApprove
        ////{
        ////    get { return _STApprove; }
        ////    //set { SetPropertyValue("Review", ref _Review, value); }
        ////    set
        ////    {
        ////        SetPropertyValue("STApprove", ref _STApprove, value);
        ////    }
        ////}
        ////#endregion

        ////#region TaskRelease
        //////private EnumRELevelSetup _TaskRelease = EnumRELevelSetup.On;
        ////private bool _TaskRelease = true;
        ////[ImmediatePostData]
        ////public bool TaskRelease
        ////{
        ////    get { return _TaskRelease; }
        ////    set { SetPropertyValue("TaskRelease", ref _TaskRelease, value); }
        ////}
        ////#endregion
        ////#region TaskAcceptance
        //////private EnumRELevelSetup _TaskAcceptance = EnumRELevelSetup.On;
        ////private bool _TaskAcceptance = true;
        ////[ImmediatePostData]
        ////public bool TaskAcceptance
        ////{
        ////    get { return _TaskAcceptance; }
        ////    set { SetPropertyValue("TaskAcceptance", ref _TaskAcceptance, value); }
        ////}
        ////#endregion

        #region DateFilter
        #region TaskWorkflow
        private EnumDateFilter _TaskWorkflow;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter TaskWorkflow
        {
            get { return _TaskWorkflow; }
            set { SetPropertyValue("TaskWorkflow", ref _TaskWorkflow, value); }
        }
        #endregion
        #region AnalysisEntryModel
        private EnumDateFilter _AnalysisEntryModel;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter AnalysisEntryModel
        {
            get { return _AnalysisEntryModel; }
            set { SetPropertyValue("AnalysisEntryModel", ref _AnalysisEntryModel, value); }
        }
        #endregion
        #region AnalysisReviewLevel
        private EnumDateFilter _AnalysisReviewLevel;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter AnalysisReviewLevel
        {
            get { return _AnalysisReviewLevel; }
            set { SetPropertyValue("AnalysisReviewLevel", ref _AnalysisReviewLevel, value); }
        }
        #endregion
        #region ReportingWorkFlow
        private EnumDateFilter _ReportingWorkFlow;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter ReportingWorkFlow
        {
            get { return _ReportingWorkFlow; }
            set { SetPropertyValue("ReportingWorkFlow", ref _ReportingWorkFlow, value); }
        }
        #endregion
        #region InventoryWorkFlow
        private EnumDateFilter _InventoryWorkFlow;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter InventoryWorkFlow
        {
            get { return _InventoryWorkFlow; }
            set { SetPropertyValue("InventoryWorkFlow", ref _InventoryWorkFlow, value); }
        }
        #endregion
        #region SampleTracking
        private EnumDateFilter _SampleTracking;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter SampleTracking
        {
            get { return _SampleTracking; }
            set { SetPropertyValue("SampleTracking", ref _SampleTracking, value); }
        }
        #endregion

        #region SampleTransfer
        private EnumDateFilter _SampleTransfer;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter SampleTransfer
        {
            get { return _SampleTransfer; }
            set { SetPropertyValue("SampleTransfer", ref _SampleTransfer, value); }
        }
        #endregion

        #region Inspection
        private EnumDateFilter _InspectionModel;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter InspectionModel
        {
            get { return _InspectionModel; }
            set { SetPropertyValue("InspectionModel", ref _InspectionModel, value); }
        }
        #endregion
        #region SuboutTracking
        private EnumDateFilter _SuboutTracking;
        [ModelDefault("PropertyEditorType", "LDM.Module.Web.Editors.EnumRadioButtonPropertyEditor")]
        [ImmediatePostData]
        public EnumDateFilter SuboutTracking
        {
            get { return _SuboutTracking; }
            set { SetPropertyValue("SuboutTracking", ref _SuboutTracking, value); }
        }
        #endregion

        #endregion


        #region ItemPath
        private string _ItemPath;
        [Size(1000)]
        public string ItemPath
        {
            get { return _ItemPath; }
            set { SetPropertyValue("ItemPath", ref _ItemPath, value); }
        }
        #endregion

        private string _NavigationCaption;
        public string NavigationCaption
        {
            get { return _NavigationCaption; }
            set { SetPropertyValue("NavigationCaption", ref _NavigationCaption, value); }
        }
        private string _Parent;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string Parent
        {
            get { return _Parent; }
            set { SetPropertyValue("Parent", ref _Parent, value); }
        }
    }
    public enum ICMOperationsFlow
    {
        WithPurchasing = 0,
        WithoutPurchasing = 1
    }
}