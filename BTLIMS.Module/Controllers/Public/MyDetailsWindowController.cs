using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.Public
{
    public partial class MyDetailsWindowController : WindowController
    {
        MessageTimer timer = new MessageTimer();
        public MyDetailsWindowController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                MyDetailsController myDetailsController = Frame.GetController<MyDetailsController>();
                Employee employee = (Employee)SecuritySystem.CurrentUser;
                if (myDetailsController != null && employee != null)
                {
                    myDetailsController.Actions["MyDetails"].ToolTip = employee.DisplayName;
                    myDetailsController.Actions["MyDetails"].Caption = employee.DisplayName;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
