using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Layout;

namespace LDM.Module.Web.Controllers.QC
{
    public partial class SDMSTabViewController : ViewController<DashboardView>
    {
        public SDMSTabViewController()
        {
            InitializeComponent();
            TargetViewId = "SDMS";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ((WebLayoutManager)View.LayoutManager).PageControlCreated += TabViewController_PageControlCreated;
        }

        private void TabViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            if (e.Model.Id == "Item1")
            {
                e.PageControl.ClientSideEvents.Init = "function(s,e){s.SetActiveTabIndex(0);}";
            }
            ((WebLayoutManager)View.LayoutManager).PageControlCreated -= TabViewController_PageControlCreated;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            ((WebLayoutManager)View.LayoutManager).PageControlCreated -= TabViewController_PageControlCreated;
            base.OnDeactivated();
        }
    }
}
