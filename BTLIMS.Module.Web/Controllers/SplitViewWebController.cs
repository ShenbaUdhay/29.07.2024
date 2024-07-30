using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.SystemModule.CallbackHandlers;
using System;

namespace BTLIMS.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SplitViewWebController : ViewController<DashboardView>
    {
        internal const string DashboardViewId = "Employee_Dashboard_ListViewAndDetailView";
        private DashboardViewItem listViewDashboardViewItem;
        private DashboardViewItem detailViewdashboardViewItem;
        private ListView listView;
        private DetailView detailView;
        public SplitViewWebController()
        {
            this.TargetViewId = DashboardViewId;//This DashboardView is defined in the Model.DesignedDiffs.xafml file.
            //ListViewFastCallbackHandlerController.UseFastCallbackHandler = false;
        }
        private void listViewProcessCurrentObjectController_CustomProcessSelectedItem(Object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            if (e.InnerArgs.CurrentObject != null)
            {
                e.Handled = true;
                detailView.CurrentObject = detailView.ObjectSpace.GetObject(e.InnerArgs.CurrentObject);
            }
        }
        private void detailViewObjectSpace_Committed(Object sender, EventArgs e)
        {
            if (((IObjectSpace)sender).IsDeletedObject(detailView.CurrentObject))
            {
                detailView.CurrentObject = null;
            }
            listView.ObjectSpace.Refresh();
        }
        private void listViewObjectSpace_Committed(Object sender, EventArgs e)
        {
            IObjectSpace listViewObjectSpace = ((IObjectSpace)sender);
            if (listViewObjectSpace.IsDeletedObject(listViewObjectSpace.GetObject(detailView.CurrentObject)))
            {
                detailView.CurrentObject = null;
            }
            detailView.ObjectSpace.Refresh();
        }
        private void listViewDashboardViewItem_ControlCreated(Object sender, EventArgs e)
        {
            ListViewProcessCurrentObjectController listViewProcessCurrentObjectController = listViewDashboardViewItem.Frame.GetController<ListViewProcessCurrentObjectController>();
            listViewProcessCurrentObjectController.CustomProcessSelectedItem += listViewProcessCurrentObjectController_CustomProcessSelectedItem;
            ListViewFastCallbackHandlerController listViewFastCallbackHandlerController = listViewDashboardViewItem.Frame.GetController<ListViewFastCallbackHandlerController>();
            //listViewFastCallbackHandlerController.Active["CustomLock"] = false;
            listView = (ListView)listViewDashboardViewItem.InnerView;
            listView.ObjectSpace.Committed -= listViewObjectSpace_Committed;
            listView.ObjectSpace.Committed += listViewObjectSpace_Committed;
        }
        private void detailViewdashboardViewItem_ControlCreated(Object sender, EventArgs e)
        {
            detailView = (DetailView)detailViewdashboardViewItem.InnerView;
            detailView.ObjectSpace.Committed -= detailViewObjectSpace_Committed;
            detailView.ObjectSpace.Committed += detailViewObjectSpace_Committed;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            DashboardView dashboardView = (DashboardView)View;
            //These items are defined in the Model.DesignedDiffs.xafml file.
            listViewDashboardViewItem = (DashboardViewItem)dashboardView.FindItem("Employee_ListView_MeasureQualification");
            detailViewdashboardViewItem = (DashboardViewItem)dashboardView.FindItem("Employee_DetailView_MeasureQualification");
            listViewDashboardViewItem.ControlCreated += listViewDashboardViewItem_ControlCreated;
            detailViewdashboardViewItem.ControlCreated += detailViewdashboardViewItem_ControlCreated;
        }
        protected override void OnDeactivated()
        {
            if (listViewDashboardViewItem != null)
            {
                listViewDashboardViewItem.ControlCreated -= listViewDashboardViewItem_ControlCreated;
            }
            if (detailViewdashboardViewItem != null)
            {
                detailViewdashboardViewItem.ControlCreated -= detailViewdashboardViewItem_ControlCreated;
            }
            if (detailView != null)
            {
                detailView.ObjectSpace.Committed -= detailViewObjectSpace_Committed;
            }
            if (listView != null)
            {
                listView.ObjectSpace.Committed -= listViewObjectSpace_Committed;
            }
            base.OnDeactivated();
        }
    }
    //public class SaveActionInSplitViewController : ViewController<DetailView> {
    //    public SimpleAction SaveActionInSplitView { get; private set; }
    //    public SaveActionInSplitViewController() {
    //        TargetViewId = "Employee_DetailView_MeasureQualification";
    //        SaveActionInSplitView = new SimpleAction(this, "SaveActionInSplitView", PredefinedCategory.Edit.ToString(), (s, e) => {
    //            ObjectSpace.CommitChanges();
    //        });
    //        SaveActionInSplitView.Caption = "Save";
    //        SaveActionInSplitView.ImageName = "MenuBar_Save";
    //    }
    //    protected override void OnViewControlsCreated() {
    //        base.OnViewControlsCreated();
    //        SaveActionInSplitView.Active["Available in DashboardView only"] = Frame is NestedFrame ? ((NestedFrame)(Frame)).ViewItem.View.Id == WebSplitViewController.DashboardViewId : false;
    //        SaveActionInSplitView.Active["Current object is set and DetailView is in Edit mode"] = View.CurrentObject != null && View.ViewEditMode == ViewEditMode.Edit;
    //    }
    //}

}
