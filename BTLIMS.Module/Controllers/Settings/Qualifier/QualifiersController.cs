using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Drawing;

namespace LDM.Module.Controllers.Qualifiers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class QualifiersController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public QualifiersController()
        {
            InitializeComponent();
            TargetViewId = "Qualifiers_ListView;" + "Qualifiers_DetailView;" + "QualifierAutomation_ListView;" + "QualifierAutomation_DetailView;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "Qualifiers_ListView" || View.Id == "Qualifiers_DetailView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.CustomGetTotalTooltip += NewObjectAction_CustomGetTotalTooltip;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_CustomGetTotalTooltip(object sender, DevExpress.ExpressApp.Actions.CustomGetTotalTooltipEventArgs e)
        {
            try
            {
                e.Tooltip = ((ActionBase)sender).Enabled ? "New Qualifier" : null;
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
            if (View.Id == "Qualifiers_DetailView" || View.Id == "QualifierAutomation_DetailView")
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
                    else if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                    {
                        ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            propertyEditor.Editor.ForeColor = Color.Black;
                        }
                    }
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
            try
            {
                if (View.Id == "Qualifiers_ListView" || View.Id == "Qualifiers_DetailView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.CustomGetTotalTooltip -= NewObjectAction_CustomGetTotalTooltip;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
