using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class RuleRequiredFieldDisableViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public RuleRequiredFieldDisableViewController()
        {
            InitializeComponent();
            
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

        }
        private void RuleSet_CustomNeedToValidateRule(object sender, CustomNeedToValidateRuleEventArgs e)
        {
            try
            {
                if (e.Target!=null && e.Target.GetType() == typeof(Samplecheckin))
                {
                    Samplecheckin objSampleCheckin = (Samplecheckin)e.Target;
                    if (objSampleCheckin != null)
                    {
                        if (objSampleCheckin.IsSampling)
                        {
                            string[] NoNeed = { "DateCollected1", "RecievedBy1", "RecievedDate1", "TAT1" };
                            if (NoNeed.Contains(e.Rule.Id))
                            {
                                e.NeedToValidateRule = false;
                                e.Handled = !e.NeedToValidateRule;
                            }
                        }
                        else
                        {
                            string[] NoNeed = { "projectid1", "DateExpect1" };
                            if (NoNeed.Contains(e.Rule.Id))
                            {
                                e.NeedToValidateRule = false;
                                e.Handled = !e.NeedToValidateRule;
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
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
           
        }
        protected override void OnDeactivated()
        {
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
    }
}
