using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DynamicDesigner;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Configuration;

namespace BTLIMS.Module.Controllers.DynamicReportDesigner
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DynamicReportDesigner : WindowController
    {
        MessageTimer timer = new MessageTimer();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        ShowNavigationItemController ShowNavigationController;
        #region Constructor
        public DynamicReportDesigner()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                ShowNavigationController.CustomShowNavigationItem += ShowNavigationController_CustomShowNavigationItem;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                ShowNavigationController.CustomShowNavigationItem -= ShowNavigationController_CustomShowNavigationItem;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void ShowNavigationController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            try
            {
                if (e.ActionArguments.SelectedChoiceActionItem.Id == "Report Designer")
                {
                    objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                    string[] connectionstring = objDRDCInfo.WebConfigConn.Split(';');

                    objDRDCInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                    objDRDCInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                    objDRDCInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                    objDRDCInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
                    frmCustomReportDesignerMDI objDesign = new frmCustomReportDesignerMDI(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                    objDesign.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion
    }
}
