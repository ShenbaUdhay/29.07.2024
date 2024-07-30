using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Linq;

namespace Labmaster.Module.Controllers.Settings
{
    public partial class FieldTestViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        NavigationRefresh objnavigationRefresh = new NavigationRefresh();
        public FieldTestViewController()
        {
            InitializeComponent();
            TargetViewId = "TestMethod_ListView_FieldTest";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Modules.BusinessObjects.Hr.Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                FieldTestEdit.Active["valFieldTest"] = false;
                if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                {
                    FieldTestEdit.Active["valFieldTest"] = true;
                }
                else
                {
                    if (objnavigationRefresh.ClickedNavigationItem == "FieldTests")
                    {
                        foreach (RoleNavigationPermission role in currentUser.RolePermissions)
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "FieldTests" && i.Write == true) != null)
                            {
                                FieldTestEdit.Active["valFieldTest"] = true;
                                break;
                            }
                        }
                    }
                }
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = false;
                FieldTestEdit.Active.SetItemValue("Stat", true);
                FieldTestSave.Active.SetItemValue("Stat", false);
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[IsFieldTest] = True");
                View.ControlsCreated += View_ControlsCreated;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_ControlsCreated(object sender, System.EventArgs e)
        {
            try
            {
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.Columns["IsFieldTest"].Visible = false;
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
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                gridListEditor.Grid.ClientInstanceName = "FieldTest";
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesNotificationController"] = true;
                View.ControlsCreated -= View_ControlsCreated;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FieldTestEdit_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                FieldTestEdit.Active.SetItemValue("Stat", false);
                FieldTestSave.Active.SetItemValue("Stat", true);
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.Columns["IsFieldTest"].Visible = true;
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("");
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FieldTestSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                FieldTestEdit.Active.SetItemValue("Stat", true);
                FieldTestSave.Active.SetItemValue("Stat", false);
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                ((ASPxGridListEditor)((ListView)View).Editor).Grid.Columns["IsFieldTest"].Visible = false;
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[IsFieldTest] = True");
                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
