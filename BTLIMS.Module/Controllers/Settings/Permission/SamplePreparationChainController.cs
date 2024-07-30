using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Linq;

namespace LDM.Module.Controllers.Settings.Permission
{
    public partial class SamplePreparationChainController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        public SamplePreparationChainController()
        {
            InitializeComponent();
            TargetViewId = "TestMethod_ListView_SamplePreparationChain;" + "SamplePreparationChain_ListView;" + "SamplePreparationChain_DetailView;";
        }

        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        objPermissionInfo.SamplePreparationChainIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.SamplePreparationChainIsWrite = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplePreparationChain" && i.Write == true) != null)
                                {
                                    objPermissionInfo.SamplePreparationChainIsWrite = true;
                                    //return;
                                }
                            }
                        }
                    }
                }
                if (View.Id == "TestMethod_ListView_SamplePreparationChain")
                {
                    //ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    //tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
                if (e.InnerArgs.CurrentObject != null && e.InnerArgs.CurrentObject.GetType() == typeof(TestMethod))
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    SamplePreparationChain objPrepChain = os.FindObject<SamplePreparationChain>(CriteriaOperator.Parse("[TestMethod] =?", ((TestMethod)e.InnerArgs.CurrentObject).Oid));
                    if (objPrepChain == null)
                    {
                        objPrepChain = os.CreateObject<SamplePreparationChain>();
                        objPrepChain.TestMethod = os.GetObject<TestMethod>((TestMethod)e.InnerArgs.CurrentObject);
                    }
                    DetailView dv = Application.CreateDetailView(os, "SamplePreparationChain_DetailView", true, objPrepChain);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    e.InnerArgs.ShowViewParameters.CreatedView = dv;
                }
                e.Handled = true;
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
                if (View.Id == "TestMethod_ListView_SamplePreparationChain")
                {
                    ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                    tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
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
