using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.SalesOrder
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class SalesOrderWindowController : WindowController
    {
        MessageTimer timer = new MessageTimer();
        public SalesOrderWindowController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
            // Target required Windows (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            btnsalesorder.Active.SetItemValue("Import", false);
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void btnsalesorder_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string viewUrl = "https://btsoftproducts.app/Alpalims_analytics";
                WebWindow.CurrentRequestWindow.RegisterStartupScript(btnsalesorder.Id, string.Format("window.open('{0}','_blank');", viewUrl));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
