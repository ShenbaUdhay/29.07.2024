using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;

namespace LDM.Module.Web.Controllers.GlobalController
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class RightClickWebController : ViewController<ListView>
    {
        public RightClickWebController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();


            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
            if (gridListEditor != null && gridListEditor.Grid != null)
            {
                gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                //gridListEditor.Grid.SettingsContextMenu.RowMenuItemVisibility.DeleteRow = false;
                //gridListEditor.Grid.SettingsContextMenu.RowMenuItemVisibility.Refresh = false;
                //gridListEditor.Grid.SettingsContextMenu.RowMenuItemVisibility.NewRow = false;
                //gridListEditor.Grid.SettingsContextMenu.RowMenuItemVisibility.EditRow = false;
                gridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems;
                gridListEditor.Grid.ContextMenuItemClick += Grid_ContextMenuItemClick;

            }

            // Access and customize the target View control.
        }

        private void Grid_ContextMenuItemClick(object sender, DevExpress.Web.ASPxGridViewContextMenuItemClickEventArgs e)
        {
            if (e.Item.Name == "")
            {

            }
        }

        private void Grid_FillContextMenuItems(object sender, DevExpress.Web.ASPxGridViewContextMenuEventArgs e)
        {
            if (e.MenuType == DevExpress.Web.GridViewContextMenuType.Rows)
            {
                //e.Items.Add("CopySamples", "copytoall");
                e.Items.Remove(e.Items.FindByText("Edit"));
            }

        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();

        }


    }
}
