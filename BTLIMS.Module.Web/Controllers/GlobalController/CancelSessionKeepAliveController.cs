using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Controls;
using System;
using System.Web.UI;

namespace Labmaster.Module.Web.Controllers.GlobalController
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class CancelSessionKeepAliveController : WindowController
    {
        public CancelSessionKeepAliveController()
        {
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            Application.CustomizeTemplate += new EventHandler<CustomizeTemplateEventArgs>(Application_CustomizeTemplate);
        }
        void Application_CustomizeTemplate(object sender, CustomizeTemplateEventArgs e)
        {
            if (e.Template is Page)
            {
                SessionKeepAliveControl keepAliveControl = ((Page)e.Template).FindControl("SKA") as SessionKeepAliveControl;
                if (keepAliveControl != null)
                {
                    keepAliveControl.Parent.Controls.Remove(keepAliveControl);
                }
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            Application.CustomizeTemplate -= new EventHandler<CustomizeTemplateEventArgs>(Application_CustomizeTemplate);
        }
    }
}
