using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Layout;

namespace LDM.Module.Controllers.SamplePreparation
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SamplePreparationTabViewController : ViewController<DetailView>
    {
        public SamplePreparationTabViewController()
        {
            InitializeComponent();
            TargetViewId = "SamplePrepBatch_DetailView_Copy;" + "SamplePrepBatch_DetailView_Copy_History;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ((WebLayoutManager)View.LayoutManager).PageControlCreated += TabViewController_PageControlCreated;
        }

        private void TabViewController_PageControlCreated(object sender, PageControlCreatedEventArgs e)
        {
            e.PageControl.ClientSideEvents.Init = "function(s,e){s.SetActiveTabIndex(0);}";
            ((WebLayoutManager)View.LayoutManager).PageControlCreated -= TabViewController_PageControlCreated;
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
        }
    }
}
