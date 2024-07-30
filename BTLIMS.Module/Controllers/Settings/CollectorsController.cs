using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Drawing;

namespace LDM.Module.Controllers.Collectors
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CollectorsController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public CollectorsController()
        {
            InitializeComponent();
            TargetViewId = "Collector_ListView;" + "Collector_DetailView_SampleLogin;" + "Collector_DetailView_ClientCollector;" + "Collector_ListView_ClientCollectors;"
                            + "Collector_DetailView;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
            // Perform various tasks depending on the target View.
        }
        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame != null)
                {
                    if (e.PopupFrame.View.Id == "Collector_DetailView")
                    {
                        e.Width = new System.Web.UI.WebControls.Unit(800);
                        e.Height = new System.Web.UI.WebControls.Unit(350);
                        e.Handled = true;
                    }
                }
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
            try
            {
                if (View.Id == "Collector_DetailView_SampleLogin")
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxStringPropertyEditor))
                        {
                            ASPxStringPropertyEditor propertyEditor = (ASPxStringPropertyEditor)item;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        //else if (item.GetType() == typeof(EnumRadioButtonListPropertyEditor))
                        //{
                        //    EnumRadioButtonListPropertyEditor propertyEditor = item as EnumRadioButtonListPropertyEditor;
                        //    if (propertyEditor != null && propertyEditor.Editor != null)
                        //    {
                        //        propertyEditor.AllowEdit.SetItemValue("stat", enbstat);
                        //    }
                        //}
                        else if (item.GetType() == typeof(ASPxLookupPropertyEditor))
                        {
                            ASPxLookupPropertyEditor propertyEditor = item as ASPxLookupPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxBooleanPropertyEditor))
                        {
                            ASPxBooleanPropertyEditor propertyEditor = item as ASPxBooleanPropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
                            }
                        }
                        else if (item.GetType() == typeof(ASPxDateTimePropertyEditor))
                        {
                            ASPxDateTimePropertyEditor propertyEditor = item as ASPxDateTimePropertyEditor;
                            if (propertyEditor != null && propertyEditor.Editor != null)
                            {
                                propertyEditor.Editor.ForeColor = Color.Black;
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
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
