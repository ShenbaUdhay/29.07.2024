using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Linq;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AssignTestToItemController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public AssignTestToItemController()
        {
            InitializeComponent();
            TargetViewId = "Items_DetailView_NN;" + "Items_Linkparameters_ListView;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {

            base.OnActivated();
            ModificationsController modificationsController = Frame.GetController<ModificationsController>();
            //modificationsController.SaveAction.Execute += SaveAction_Execute;
            //modificationsController.SaveAndCloseAction.Execute += SaveAndCloseAction_Execute;
            modificationsController.SaveAction.Executing += SaveAction_Executing;
            modificationsController.SaveAndCloseAction.Executing += SaveAndCloseAction_Executing;
            // Perform various tasks depending on the target View.
        }

        private void SaveAndCloseAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id != null && View.Id == "Items_DetailView_NN")
                {
                    Items obj = View.CurrentObject as Items;
                    if (obj.Linkparameters.Count > 0)
                    {
                        if (obj.Linkparameters.FirstOrDefault(i => i.LinkTestMethod == null) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage("Please enter the Matrix ", InformationType.Warning, 3000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        MessageOptions options = new MessageOptions();
                        options.Duration = 1000;
                        //options.Message = "Saved SuccessFully";
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Add Test ", InformationType.Warning, 3000, InformationPosition.Top);
                    }

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View.Id != null && View.Id == "Items_DetailView_NN")
                {
                    Items obj = View.CurrentObject as Items;
                    if (obj.Linkparameters.Count > 0)
                    {
                        if (obj.Linkparameters.FirstOrDefault(i => i.LinkTestMethod == null) != null)
                        {
                            Application.ShowViewStrategy.ShowMessage("Please enter the matrix. ", InformationType.Warning, 3000, InformationPosition.Top);
                            e.Cancel = true;
                            return;
                        }
                        MessageOptions options = new MessageOptions();
                        options.Duration = 1000;
                        //options.Message = "Saved SuccessFully";
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                    else
                    {
                        e.Cancel = true;
                        Application.ShowViewStrategy.ShowMessage("Add Test ", InformationType.Warning, 3000, InformationPosition.Top);
                    }

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAndCloseAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id != null && View.Id == "Items_DetailView_NN")
                {
                    Items obj = View.CurrentObject as Items;
                    if (obj.Linkparameters.Count > 0)
                    {
                        MessageOptions options = new MessageOptions();
                        options.Duration = 1000;
                        //options.Message = "Saved SuccessFully";
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Add Test ", InformationType.Warning, 3000, InformationPosition.Top);
                    }

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id != null && View.Id == "Items_DetailView_NN")
                {
                    Items obj = View.CurrentObject as Items;
                    if (obj.Linkparameters.Count > 0)
                    {

                        MessageOptions options = new MessageOptions();
                        options.Duration = 1000;
                        //options.Message = "Saved SuccessFully";
                        options.Message = CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess");
                        options.Type = InformationType.Success;
                        options.Web.Position = InformationPosition.Top;
                        Application.ShowViewStrategy.ShowMessage(options);
                    }
                    else
                    {

                        Application.ShowViewStrategy.ShowMessage("Add Test", InformationType.Warning, 3000, InformationPosition.Top);
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
            try
            {
                base.OnViewControlsCreated();
                if (View.Id == "Items_Linkparameters_ListView")
                {
                    ASPxGridListEditor grid = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = grid.Grid;
                    gridView.CancelRowEditing += GridView_CancelRowEditing;
                }
                // Access and customize the target View control.
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void GridView_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            try
            {
                e.Cancel = true;
                // ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit
                ((ListView)View).CollectionSource.Reload();
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
            ModificationsController modificationsController = Frame.GetController<ModificationsController>();
            //modificationsController.SaveAction.Execute -= SaveAction_Execute;
            //modificationsController.SaveAndCloseAction.Execute -= SaveAndCloseAction_Execute;
            modificationsController.SaveAction.Executing -= SaveAction_Executing;
            modificationsController.SaveAndCloseAction.Executing -= SaveAndCloseAction_Executing;
        }
    }
}
