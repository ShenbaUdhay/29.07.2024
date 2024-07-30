using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Dashboards;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Dashboard;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BTLIMS.Module.Win.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class DashboardWindowController : WindowController
    {
        ShowNavigationItemController ShowNavigationController;
        MessageTimer timer = new MessageTimer();
        List<AssignDashboardToUserDepartment> lstDashboard = new List<AssignDashboardToUserDepartment>();
        #region Constructor
        public DashboardWindowController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            TargetWindowType = WindowType.Main;
        }
        #endregion

        #region DefaultEvents
        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            UnSubscribeEvent();
            SubscribeEvent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        #endregion

        #region Functions
        private void SubscribeEvent()
        {
            try
            {
                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                if (ShowNavigationController != null)
                {
                    ShowNavigationController.NavigationItemCreated += ShowNavigationController_NavigationItemCreated;
                    IObjectSpace os = Application.CreateObjectSpace();
                    Session currentSession = ((XPObjectSpace)(os)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    lstDashboard = uow.Query<AssignDashboardToUserDepartment>().Where(i => i.NavigationItem != null).ToList();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void UnSubscribeEvent()
        {
            try
            {
                ShowNavigationController = Frame.GetController<ShowNavigationItemController>();
                if (ShowNavigationController != null)
                {
                    ShowNavigationController.NavigationItemCreated -= ShowNavigationController_NavigationItemCreated;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion


        #region Events
        private void ShowNavigationController_NavigationItemCreated(object sender, NavigationItemCreatedEventArgs e)
        {
            try
            {
                #region nav
                #endregion
                if (lstDashboard.FirstOrDefault(i => i.NavigationItem.NavigationItemNameID == e.NavigationItem.Id) != null)
                {
                    bool IsAdministrator = false;
                    Employee currentUser = SecuritySystem.CurrentUser as Employee;
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    if (currentUser != null && currentUser.Roles.Count > 0 && currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                    {
                        IsAdministrator = true;
                    }
                    if (IsAdministrator)
                    {
                        IList<AssignDashboardToUserDepartment> objAdt = objspace.GetObjects<AssignDashboardToUserDepartment>(CriteriaOperator.Parse("[NavigationItem.NavigationItemNameID] = ? And [IsActive] = True", e.NavigationItem.Id));/*.OrderBy(a=>a.NavigationItemDataSource).ToList();*/

                        if (objAdt.Count > 0)
                        {
                            foreach (AssignDashboardToUserDepartment objAdtu in objAdt.OrderBy(i => i.DashboardViewName.Title))
                            {

                                using (IObjectSpace ObjectSpace = Application.CreateObjectSpace(typeof(DashboardData)))
                                {
                                    DashboardData dashboarddata = ObjectSpace.FindObject<DashboardData>(CriteriaOperator.Parse("[Oid]=?", objAdtu.DashboardViewName.Oid));
                                    //AssignDashboardToUserDepartment objAdtu = ObjectSpace.FindObject<AssignDashboardToUserDepartment>(CriteriaOperator.Parse("[DashboardViewName.Oid] = ?", dashboarddata.Oid));
                                    if (objAdtu != null && objAdtu.IsActive == true && dashboarddata != null)
                                    {
                                        e.NavigationItem.Items.Add(CreateNavigationItem(ObjectSpace, dashboarddata));
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (currentUser != null && currentUser.UserName != "Administrator" && currentUser.UserName != "Service")
                        {
                            List<string> lstRoles = currentUser.RoleNames.Split(',').ToList();
                            List<string> lstNavigationId = new List<string>();
                            foreach (string strrole in lstRoles)
                            {
                                if (!String.IsNullOrEmpty(strrole))
                                {
                                    RoleNavigationPermission objRNP = objspace.FindObject<RoleNavigationPermission>(CriteriaOperator.Parse("[RoleName] = ?", strrole));
                                    //IList<AssignDashboardToUserDepartment> objAdt = objspace.GetObjects<AssignDashboardToUserDepartment>(CriteriaOperator.Parse("[NavigationItem.NavigationItemNameID] = ? And [IsActive] = True And [Role] Like  '%+?+%'", e.NavigationItem.Id, objRNP.Oid));
                                    if (objRNP != null)
                                    {
                                        IList<AssignDashboardToUserDepartment> objAdt = objspace.GetObjects<AssignDashboardToUserDepartment>(CriteriaOperator.Parse("[NavigationItem.NavigationItemNameID] = ? And Contains([Role], ?) And [IsActive] =True", e.NavigationItem.Id, objRNP.Oid.ToString().Replace(" ", "")));
                                        if (objAdt.Count > 0)
                                        {
                                            foreach (AssignDashboardToUserDepartment objAdtu in objAdt.OrderBy(i => i.DashboardViewName.Title))
                                            {

                                                using (IObjectSpace ObjectSpace = Application.CreateObjectSpace(typeof(DashboardData)))
                                                {
                                                    DashboardData dashboarddata = ObjectSpace.FindObject<DashboardData>(CriteriaOperator.Parse("[Oid]=?", objAdtu.DashboardViewName.Oid));
                                                    //AssignDashboardToUserDepartment objAdtu = ObjectSpace.FindObject<AssignDashboardToUserDepartment>(CriteriaOperator.Parse("[DashboardViewName.Oid] = ?", dashboarddata.Oid));
                                                    if (objAdtu != null && objAdtu.IsActive == true && dashboarddata != null)
                                                    {
                                                        if (!lstNavigationId.Contains(dashboarddata.ToString()))
                                                        {
                                                            e.NavigationItem.Items.Add(CreateNavigationItem(ObjectSpace, dashboarddata));
                                                            lstNavigationId.Add(dashboarddata.ToString());
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private ChoiceActionItem CreateNavigationItem(IObjectSpace ObjectSpace, DashboardData dashboarddata)
        {
            try
            {
                ViewShortcut viewshortcut = new ViewShortcut(DashboardsModule.DashboardDetailViewName, ObjectSpace.GetKeyValueAsString(dashboarddata));
                ChoiceActionItem Item = new ChoiceActionItem(dashboarddata.Title, dashboarddata.Title, viewshortcut);
                Item.Model.ImageName = "BO_Dashboard";
                return Item;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return null;
            }
        }
        #endregion
    }
}
