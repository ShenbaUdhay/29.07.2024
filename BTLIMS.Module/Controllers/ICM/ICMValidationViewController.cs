using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using ICM.Module.BusinessObjects;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.ICM
{
    public partial class ICMValidationViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        #region Declaration
        ModificationsController mdcSave;
        ModificationsController mdcSaveClose;
        ModificationsController mdcSaveNew;
        #endregion

        #region Consturctor
        public ICMValidationViewController()
        {
            InitializeComponent();
            TargetViewId = "Items_DetailView;" + "Requisition_DetailView;" + "Vendors_DetailView;" + "Category_DetailView;" + "Grades_DetailView;" + "ICMStorage_DetailView;" + "Packageunits_DetailView;" + "Shippingoptions_DetailView;" + "Brand_DetailView;" + "Purchaseorder_DetailView;" + "ShipVia_DetailView;";
        }
        #endregion

        #region Default Methods
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "Packageunits_DetailView" || View.Id == "Shippingoptions_DetailView" || View.Id == "ShipVia_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                }
                mdcSave = Frame.GetController<ModificationsController>();
                mdcSave.SaveAction.Executed += SaveAction_Executed;
                mdcSaveClose = Frame.GetController<ModificationsController>();
                mdcSaveClose.SaveAndCloseAction.Executed += SaveAndCloseAction_Executed;
                mdcSaveNew = Frame.GetController<ModificationsController>();
                mdcSaveNew.SaveAndNewAction.Executed += SaveAndNewAction_Executed;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {
                if (View.Id == "Packageunits_DetailView" && View.CurrentObject == e.Object && e.NewValue != e.OldValue)
                {
                    Packageunits obj = (Packageunits)e.Object;

                    if (e.PropertyName == "Sort")
                    {
                        if (obj.Sort < 0)
                        {
                            obj.Sort = Convert.ToInt32(e.OldValue);
                            Application.ShowViewStrategy.ShowMessage("Sort must not be negative", InformationType.Info, 3000, InformationPosition.Top);
                        }
                    }
                }
                if (View.Id == "Shippingoptions_DetailView" && View.CurrentObject == e.Object && e.NewValue != e.OldValue)
                {
                    Shippingoptions objShip = (Shippingoptions)e.Object;
                    if (e.PropertyName == "Sort")
                    {
                        if (objShip.Sort < 0)
                        {
                            objShip.Sort = Convert.ToInt32(e.OldValue);
                            Application.ShowViewStrategy.ShowMessage("Sort must not be negative", InformationType.Info, 3000, InformationPosition.Top);
                        }
                    }
                }
                if (View.Id == "ShipVia_DetailView" && View.CurrentObject == e.Object && e.NewValue != e.OldValue)
                {
                    ShipVia objShip = (ShipVia)e.Object;
                    if (e.PropertyName == "Sort")
                    {
                        if (objShip.Sort < 0)
                        {
                            objShip.Sort = Convert.ToInt32(e.OldValue);
                            Application.ShowViewStrategy.ShowMessage("Sort must not be negative", InformationType.Info, 3000, InformationPosition.Top);
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
                mdcSave = Frame.GetController<ModificationsController>();
                mdcSave.SaveAction.Executed += SaveAction_Executed;
                mdcSaveClose = Frame.GetController<ModificationsController>();
                mdcSaveClose.SaveAndCloseAction.Executed += SaveAndCloseAction_Executed;
                mdcSaveNew = Frame.GetController<ModificationsController>();
                mdcSaveNew.SaveAndNewAction.Executed += SaveAndNewAction_Executed;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void SaveAndNewAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAndCloseAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion
    }
}
