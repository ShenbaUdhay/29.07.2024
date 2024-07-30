using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Linq;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DateFormatDetailViewController : ViewController<DetailView>
    {
        MessageTimer timer = new MessageTimer();
        public DateFormatDetailViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View != null)
                {
                    foreach (ViewItem Item in ((DetailView)View).Items.Where(i => i is ASPxDateTimePropertyEditor))
                    {
                        if (Item is ASPxDateTimePropertyEditor)
                        {
                            PropertyEditor editor = ((PropertyEditor)Item);
                            if (editor.DisplayFormat == "{0:d}")
                            {
                                editor.DisplayFormat = "MM/dd/yy";
                                editor.EditMask = "MM/dd/yy";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
