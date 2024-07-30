using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Utils;
using DevExpress.Web;

namespace LDM.Module.Web.Controllers.GlobalController
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BatchEditModeNavigationWebViewController : ViewController<ListView>
    {
        #region Declaration
        private const string EventHandlerKey = "BatchEditModeNavigationController";
        #endregion

        #region Constructor
        public BatchEditModeNavigationWebViewController()
        {
            InitializeComponent();
        }
        #endregion

        #region DeafaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            IModelListViewWeb model = (IModelListViewWeb)View.Model;
            if (model.InlineEditMode != InlineEditMode.Batch) return;
            ASPxGridListEditor editor = View.Editor as ASPxGridListEditor;
            if (editor == null) return;
            editor.Grid.SettingsEditing.BatchEditSettings.StartEditAction = GridViewBatchStartEditAction.FocusedCellClick;
            ClientSideEventsHelper.AssignClientHandlerSafe(editor.Grid, "Init", "onInit", EventHandlerKey);
            ClientSideEventsHelper.AssignClientHandlerSafe(editor.Grid, "BatchEditStartEditing", "onStartEditing", EventHandlerKey);
            editor.Grid.ClientInstanceName = "Grid"; // required as per https://github.com/DevExpress-Examples/obsolete-aspxgridview-how-to-implement-navigation-by-up-left-down-right-buttons-when-the-b-t283418/blob/db525099926458681f961bce87ce14c9296bb978/CS/Default.aspx#L63

        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        #endregion
    }
}
