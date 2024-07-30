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
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LDM.Module.Controllers.Settings
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class QuotesDefaultController : ViewController
    {
        MessageTimer timer = new MessageTimer();

        public QuotesDefaultController()
        {
            InitializeComponent();
            TargetViewId = "QuotesDefaultSettings_DetailView;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ModificationsController modificationcontroller = Frame.GetController<ModificationsController>();
                if (modificationcontroller != null)
                {
                    modificationcontroller.SaveAction.Executing += SaveAction_Executing;
                    modificationcontroller.SaveAndCloseAction.Executing += SaveAction_Executing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                QuotesDefaultSettings objResult = (QuotesDefaultSettings)View.CurrentObject;
                IList<DefaultSetting> FDdefsetting = View.ObjectSpace.GetObjects<DefaultSetting>(CriteriaOperator.Parse("[ModuleName] = 'Quotes' And Not IsNullOrEmpty([NavigationItemName])"));
                if (FDdefsetting != null && FDdefsetting.Count > 0)
                {
                    DefaultSetting FD2defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "QuotesReview").FirstOrDefault();
                    if (FD2defsetting != null)
                    {
                        if (objResult.QuotesReview == QuotesDefaultSettings.YesNoFilter.No)
                        {
                            FD2defsetting.Select = false;
                        }
                        else if (objResult.QuotesReview == QuotesDefaultSettings.YesNoFilter.Yes)
                        {
                            FD2defsetting.Select = true;
                        }
                    }

                    DefaultSetting FD3defsetting = FDdefsetting.Where(a => a.NavigationItemNameID == "QuotesReview").FirstOrDefault();
                    if (FD3defsetting != null)
                    {
                        if (objResult.QuotesReview == QuotesDefaultSettings.YesNoFilter.No)
                        {
                            FD3defsetting.Select = false;
                        }
                        else if (objResult.QuotesReview == QuotesDefaultSettings.YesNoFilter.Yes)
                        {
                            FD3defsetting.Select = true;
                        }
                    }
                    View.ObjectSpace.CommitChanges();
                    Application.ShowViewStrategy.ShowMessage("Saved successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);

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
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            base.OnDeactivated();
            try
            {
                ModificationsController modificationcontroller = Frame.GetController<ModificationsController>();
                if (modificationcontroller != null)
                {
                    modificationcontroller.SaveAction.Executing -= SaveAction_Executing;
                    modificationcontroller.SaveAndCloseAction.Executing -= SaveAction_Executing;
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
