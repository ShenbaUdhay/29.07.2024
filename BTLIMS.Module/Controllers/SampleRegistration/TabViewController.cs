using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Layout;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.SampleRegistration
{
    public partial class TabViewController : ViewController<DashboardView>
    {
        MessageTimer timer = new MessageTimer();
        public TabViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleRegistration;" + "PLM;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                ((WebLayoutManager)View.LayoutManager).PageControlCreated += TabViewController_PageControlCreated;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void TabViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            try
            {
                if (e.Model.Id == "Item2")
                {
                    e.PageControl.ClientSideEvents.Init = "function(s,e){s.SetActiveTabIndex(1);}";
                    ((WebLayoutManager)View.LayoutManager).PageControlCreated -= TabViewController_PageControlCreated;
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
                ((WebLayoutManager)View.LayoutManager).PageControlCreated -= TabViewController_PageControlCreated;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
    }
}
