using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class RefreshPublicVariableViewController : ViewController
    {
        #region Constructor
        MessageTimer timer = new MessageTimer();
        public RefreshPublicVariableViewController()
        {
            InitializeComponent();
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Frame.GetController<ShowNavigationItemController>().CustomShowNavigationItem += new EventHandler<CustomShowNavigationItemEventArgs>(ViewController1_CustomShowNavigationItem);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                Frame.GetController<ShowNavigationItemController>().CustomShowNavigationItem -= new EventHandler<CustomShowNavigationItemEventArgs>(ViewController1_CustomShowNavigationItem);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        void ViewController1_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            try
            {
                ClearPublicValues();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        #endregion

        #region Methods
        private void ClearPublicValues()
        {
            try
            {
                IValueManager<string> QCResultValidationCurrentTabViewID = ValueManager.GetValueManager<string>("QCResultValidationCurrentTabViewID");
                if (QCResultValidationCurrentTabViewID.CanManageValue)
                    QCResultValidationCurrentTabViewID.Value = string.Empty;
                IValueManager<string> QCResultValidationCurrentViewID = ValueManager.GetValueManager<string>("QCResultValidationCurrentTabViewID");
                if (QCResultValidationCurrentViewID.CanManageValue)
                    QCResultValidationCurrentViewID.Value = string.Empty;
                IValueManager<string> QCResultValidationQPanel = ValueManager.GetValueManager<string>("QCResultValidationQueryFilter");
                if (QCResultValidationQPanel.CanManageValue)
                    QCResultValidationQPanel.Value = string.Empty;
                IValueManager<string> ResultEntryCurrentViewID = ValueManager.GetValueManager<string>("ResultEntryCurrentViewID");
                if (ResultEntryCurrentViewID.CanManageValue)
                    ResultEntryCurrentViewID.Value = string.Empty;
                IValueManager<string> REQPanel = ValueManager.GetValueManager<string>("ResultEntryQueryPanel");
                if (REQPanel.CanManageValue)
                    REQPanel.Value = string.Empty;
                IValueManager<string> ReportingCurrentViewID = ValueManager.GetValueManager<string>("ReportingCurrentViewID");
                if (ReportingCurrentViewID.CanManageValue)
                    ReportingCurrentViewID.Value = string.Empty;
                IValueManager<string> ReportingQPanel = ValueManager.GetValueManager<string>("ReportingQueryPanel");
                if (ReportingQPanel.CanManageValue)
                    ReportingQPanel.Value = string.Empty;
                IValueManager<string> SLJobID = ValueManager.GetValueManager<string>("SLJobID");
                if (SLJobID.CanManageValue)
                    SLJobID.Value = string.Empty;
                IValueManager<string> SLSampleID = ValueManager.GetValueManager<string>("SLSampleID");
                if (SLSampleID.CanManageValue)
                    SLSampleID.Value = string.Empty;
                IValueManager<string> SLfocusedJobID = ValueManager.GetValueManager<string>("SLfocusedJobID");
                if (SLfocusedJobID.CanManageValue)
                    SLfocusedJobID.Value = string.Empty;
                IValueManager<bool> boolCopySamples = ValueManager.GetValueManager<bool>("boolCopySamples");
                if (boolCopySamples.CanManageValue)
                    boolCopySamples.Value = false;
                IValueManager<string> SLVisualMatrixName = ValueManager.GetValueManager<string>("SLVisualMatrixName");
                if (SLVisualMatrixName.CanManageValue)
                    SLVisualMatrixName.Value = string.Empty;
                IValueManager<string> SLOid = ValueManager.GetValueManager<string>("SLOid");
                if (SLOid.CanManageValue)
                    SLOid.Value = string.Empty;
                IValueManager<string> SLQPanel = ValueManager.GetValueManager<string>("SLQueryPanel");
                if (SLQPanel.CanManageValue)
                    SLQPanel.Value = string.Empty;
                IValueManager<int> NoOfSamples = ValueManager.GetValueManager<int>("NoOfSamples");
                if (NoOfSamples.CanManageValue)
                    NoOfSamples.Value = 0;
                IValueManager<bool> Msgflag = ValueManager.GetValueManager<bool>("Msgflag");
                if (Msgflag.CanManageValue)
                    Msgflag.Value = false;
                IValueManager<string> SCJobID = ValueManager.GetValueManager<string>("SCJobID");
                if (SCJobID.CanManageValue)
                    SCJobID.Value = string.Empty;
                IValueManager<string> SCVisualMatrixName = ValueManager.GetValueManager<string>("SCVisualMatrixName");
                if (SCVisualMatrixName.CanManageValue)
                    SCVisualMatrixName.Value = string.Empty;
                IValueManager<bool> bolGoToSampleLogin = ValueManager.GetValueManager<bool>("bolGoToSampleLogin");
                if (bolGoToSampleLogin.CanManageValue)
                    bolGoToSampleLogin.Value = false;
                IValueManager<string> SCQPanel = ValueManager.GetValueManager<string>("SCQueryPanel");
                if (SCQPanel.CanManageValue)
                    SCQPanel.Value = string.Empty;
                IValueManager<bool> SCSLQueryPanelExecuted = ValueManager.GetValueManager<bool>("SCSLQueryPanelExecuted");
                if (SCSLQueryPanelExecuted.CanManageValue)
                    SCSLQueryPanelExecuted.Value = false;
                IValueManager<string> ClientName = ValueManager.GetValueManager<string>("ClientName");
                if (ClientName.CanManageValue)
                    ClientName.Value = string.Empty;
                IValueManager<string> TestMethodOid = ValueManager.GetValueManager<string>("TestMethodOid");
                if (TestMethodOid.CanManageValue)
                    TestMethodOid.Value = string.Empty;
                IValueManager<string> RequisitionFilter = ValueManager.GetValueManager<string>("RequisitionFilter");
                if (RequisitionFilter.CanManageValue)
                    RequisitionFilter.Value = string.Empty;
                IValueManager<string> valueManager = ValueManager.GetValueManager<string>("ICMinfo");
                if (valueManager.CanManageValue)
                    valueManager.Value = string.Empty;
                IValueManager<string> RollBackReason = ValueManager.GetValueManager<string>("RollBackReason");
                if (RollBackReason.CanManageValue)
                    RollBackReason.Value = string.Empty;
                IValueManager<string> Vendor = ValueManager.GetValueManager<string>("Vendor");
                if (Vendor.CanManageValue)
                    Vendor.Value = string.Empty;
                IValueManager<string> receivequery = ValueManager.GetValueManager<string>("receivequery");
                if (receivequery.CanManageValue)
                    receivequery.Value = string.Empty;
                IValueManager<string> DistributionFilter = ValueManager.GetValueManager<string>("DistributionFilter");
                if (DistributionFilter.CanManageValue)
                    DistributionFilter.Value = string.Empty;
                IValueManager<string> QueryFilter = ValueManager.GetValueManager<string>("QueryFilter");
                if (QueryFilter.CanManageValue)
                    QueryFilter.Value = string.Empty;
                IValueManager<string> sesitem = ValueManager.GetValueManager<string>("sesitem");
                if (sesitem.CanManageValue)
                    sesitem.Value = string.Empty;
                IValueManager<string> ApproveFilter = ValueManager.GetValueManager<string>("ApproveFilter");
                if (ApproveFilter.CanManageValue)
                    ApproveFilter.Value = string.Empty;
                IValueManager<string> rgMode = ValueManager.GetValueManager<string>("rgMode");
                if (rgMode.CanManageValue)
                    rgMode.Value = string.Empty;
                IValueManager<string> ConsumptionFilter = ValueManager.GetValueManager<string>("ConsumptionFilter");
                if (ConsumptionFilter.CanManageValue)
                    ConsumptionFilter.Value = string.Empty;
                IValueManager<string> DisposalFilter = ValueManager.GetValueManager<string>("DisposalFilter");
                if (DisposalFilter.CanManageValue)
                    DisposalFilter.Value = string.Empty;
                IValueManager<string> disposalrgMode = ValueManager.GetValueManager<string>("disposalrgMode");
                if (disposalrgMode.CanManageValue)
                    disposalrgMode.Value = string.Empty;
                IValueManager<string> consumptionrgMode = ValueManager.GetValueManager<string>("consumptionrgMode");
                if (consumptionrgMode.CanManageValue)
                    consumptionrgMode.Value = string.Empty;
                IValueManager<List<string>> ObjectsToShow = ValueManager.GetValueManager<List<string>>("ObjectsToShow");
                if (ObjectsToShow.CanManageValue)
                    ObjectsToShow.Value = new List<string>();
                IValueManager<bool> ResultEntryFromDashboard = ValueManager.GetValueManager<bool>("ResultEntryFromDashboard");
                if (ResultEntryFromDashboard.CanManageValue)
                    ResultEntryFromDashboard.Value = false;
                IValueManager<string> poquery = ValueManager.GetValueManager<string>("poquery");
                if (poquery.CanManageValue)
                    poquery.Value = string.Empty;
                IValueManager<string> ExistingstockFilter = ValueManager.GetValueManager<string>("ExistingstockFilter");
                if (ExistingstockFilter.CanManageValue)
                    ExistingstockFilter.Value = string.Empty;
                IValueManager<List<string>> Items = ValueManager.GetValueManager<List<string>>("Items");
                if (Items.CanManageValue)
                    Items.Value = new List<string>();
                IValueManager<List<string>> Esidlist = ValueManager.GetValueManager<List<string>>("Esidlist");
                if (Esidlist.CanManageValue)
                    Esidlist.Value = new List<string>();
                IValueManager<string> strSampleID = ValueManager.GetValueManager<string>("strSampleID");
                if (strSampleID.CanManageValue)
                    strSampleID.Value = string.Empty;
                IValueManager<string> strJobID = ValueManager.GetValueManager<string>("strJobID");
                if (strJobID.CanManageValue)
                    strJobID.Value = string.Empty;
                IValueManager<string> lstSampleID = ValueManager.GetValueManager<string>("lstSampleID");
                if (strJobID.CanManageValue)
                    strJobID.Value = string.Empty;
                IValueManager<Guid> lstSampleOid = ValueManager.GetValueManager<Guid>("lstSampleOid");
                if (strJobID.CanManageValue)
                    strJobID.Value = string.Empty;
                IValueManager<string> SLCOCID = ValueManager.GetValueManager<string>("SLCOCID");
                if (SLJobID.CanManageValue)
                    SLJobID.Value = string.Empty;
                IValueManager<string> COCSampleID = ValueManager.GetValueManager<string>("COCSampleID");
                if (COCSampleID.CanManageValue)
                    COCSampleID.Value = string.Empty;
                IValueManager<string> COCfocusedCOCID = ValueManager.GetValueManager<string>("COCfocusedCOCID");
                if (COCfocusedCOCID.CanManageValue)
                    COCfocusedCOCID.Value = string.Empty;
                IValueManager<string> COCVisualMatrixName = ValueManager.GetValueManager<string>("COCVisualMatrixName");
                if (COCVisualMatrixName.CanManageValue)
                    COCVisualMatrixName.Value = string.Empty;
                IValueManager<string> COCOid = ValueManager.GetValueManager<string>("COCOid");
                if (COCOid.CanManageValue)
                    COCOid.Value = string.Empty;
                IValueManager<string> SCCOCID = ValueManager.GetValueManager<string>("SCCOCID");
                if (SCCOCID.CanManageValue)
                    SCCOCID.Value = string.Empty;
                IValueManager<string> strCOCID = ValueManager.GetValueManager<string>("strCOCID");
                if (strCOCID.CanManageValue)
                    strCOCID.Value = string.Empty;

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion
    }
}
