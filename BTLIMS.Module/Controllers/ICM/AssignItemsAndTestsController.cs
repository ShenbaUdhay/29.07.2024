using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web.SystemModule;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AssignItemsAndTestsController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public AssignItemsAndTestsController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            AssignItemAdd.TargetViewId = "Items_Linkparameters_ListView;";
            AssignMethodAdd.TargetViewId = "TestMethod_Linkparameters_ListView;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            try
            {
                if (View.Id == "TestMethod_Linkparameters_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["MethodItemController"] = false;
                }
                if (View.Id == "Items_Linkparameters_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["ItemMethodController"] = false;
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
            try
            {
                if (View.Id == "TestMethod_Linkparameters_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["MethodItemController"] = false;
                }
                if (View.Id == "Items_Linkparameters_ListView")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["ItemMethodController"] = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void AssignItemAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Items objItem = Application.MainWindow.View.CurrentObject as Items;
                ItemTestMethodLink objParameter = Application.MainWindow.View.ObjectSpace.CreateObject<ItemTestMethodLink>();
                objItem.Linkparameters.Add(objParameter);
                //IObjectSpace os = Application.CreateObjectSpace();
                //if (View.Id == "Items_Linkparameters_ListView")
                //{
                //    Items obj = os.CreateObject<Items>();
                //    //os.CommitChanges();
                //    ((ListView)View).CollectionSource.Add(ObjectSpace.GetObject(obj));
                //}
                //View.Refresh();
                Application.MainWindow.View.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AssignMethodAdd_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                TestMethod objTestMethod = Application.MainWindow.View.CurrentObject as TestMethod;
                ItemTestMethodLink objParameter = Application.MainWindow.View.ObjectSpace.CreateObject<ItemTestMethodLink>();
                objTestMethod.Linkparameters.Add(objParameter);
                Application.MainWindow.View.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

    }
}
