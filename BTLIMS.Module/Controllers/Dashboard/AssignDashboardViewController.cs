using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Dashboard;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;

namespace BTLIMS.Module.Controllers.Dashboard
{
    public partial class AssignDashboardViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        #region Constructor
        public AssignDashboardViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(AssignDashboardToUserDepartment);
        }
        #endregion

        #region DefaultMethds
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        #endregion

        #region Events
        private void AssignDashboardViewController_ViewControlsCreated(object sender, EventArgs e)
        {
            try
            {
                if (View != null)
                {
                    if (View.Id == "AssignDashboardToUserDepartment_DetailView")
                    {
                        var lst = ((IModelApplicationNavigationItems)Application.Model).NavigationItems.AllItems;
                        if (lst != null)
                        {
                            List<string> lstNAvItems = new List<string>();
                            IModelApplicationNavigationItems navitems = (IModelApplicationNavigationItems)Application.Model;
                            IModelRootNavigationItems navRoots = (navitems).NavigationItems;

                            foreach (IModelNavigationItem rootnav in navRoots.Items)
                            {
                                lstNAvItems.Add(rootnav.Caption);
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
        #endregion
    }
}
