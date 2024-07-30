using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Validation;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CountryViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public CountryViewController()
        {
            InitializeComponent();
            TargetViewId = "CustomState_DetailView_Copy";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {

            base.OnActivated();
            try
            {
                RuleSet.CustomNeedToValidateRule += RuleSet_CustomNeedToValidateRule;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            try
            {
                RuleSet.CustomNeedToValidateRule -= RuleSet_CustomNeedToValidateRule;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RuleSet_CustomNeedToValidateRule(object sender, CustomNeedToValidateRuleEventArgs e)
        {
            try
            {
                if (View != null && View is DetailView && (View.Id == "CustomState_DetailView_Copy"))
                {
                    DetailView dv = View as DetailView;
                    RuleBase ruleBase = e.Rule as RuleBase;
                    if (dv != null && ruleBase != null && dv.ObjectTypeInfo.Type == ruleBase.Properties.TargetType)
                    {
                        List<string> visibleProperties = new List<string>();
                        foreach (PropertyEditor editor in dv.GetItems<PropertyEditor>())
                        {
                            if (editor.Control != null && ((System.Web.UI.Control)editor.Control).Visible)
                            {
                                visibleProperties.Add(editor.PropertyName);
                            }
                        }
                        foreach (string propertyToBeValidated in e.Rule.UsedProperties)
                        {
                            if (!visibleProperties.Contains(propertyToBeValidated))
                            {
                                e.NeedToValidateRule = false;
                                e.Handled = true;
                                break;
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
    }
}
