namespace LDM.Module.Controllers.Settings.Permission
{
    partial class RolePermissionController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.addNavigationItemAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.removeNavigationItemAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.GridSaveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // addNavigationItemAction
            // 
            this.addNavigationItemAction.Caption = "Link";
            this.addNavigationItemAction.ConfirmationMessage = null;
            this.addNavigationItemAction.Id = "addNavigationItemAction";
            this.addNavigationItemAction.ToolTip = null;
            this.addNavigationItemAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.addNavigationItemAction_Execute);
            // 
            // removeNavigationItemAction
            // 
            this.removeNavigationItemAction.Caption = "Unlink";
            this.removeNavigationItemAction.ConfirmationMessage = null;
            this.removeNavigationItemAction.Id = "removeNavigationItemAction";
            this.removeNavigationItemAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.removeNavigationItemAction.ToolTip = null;
            this.removeNavigationItemAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.removeNavigationItemAction_Execute);
            // 
            // GridSaveAction
            // 
            this.GridSaveAction.Caption = "Save";
            this.GridSaveAction.Category = "RecordEdit";
            this.GridSaveAction.ConfirmationMessage = null;
            this.GridSaveAction.Id = "GridSaveAction";
            this.GridSaveAction.ImageName = "Action_Save";
            this.GridSaveAction.ToolTip = null;
            this.GridSaveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GridSaveAction_Execute);
            // 
            // RolePermissionController
            // 
            this.Actions.Add(this.addNavigationItemAction);
            this.Actions.Add(this.removeNavigationItemAction);
            this.Actions.Add(this.GridSaveAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction addNavigationItemAction;
        private DevExpress.ExpressApp.Actions.SimpleAction removeNavigationItemAction;
        private DevExpress.ExpressApp.Actions.SimpleAction GridSaveAction;
    }
}
