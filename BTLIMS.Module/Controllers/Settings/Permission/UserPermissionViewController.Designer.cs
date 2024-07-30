namespace LDM.Module.Controllers.Settings
{
    partial class UserPermissionViewController
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
            this.copyPermission = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // copyPermission
            // 
            this.copyPermission.AcceptButtonCaption = null;
            this.copyPermission.CancelButtonCaption = null;
            this.copyPermission.Caption = "Copy Permission";
            this.copyPermission.Category = "View";
            this.copyPermission.ConfirmationMessage = null;
            this.copyPermission.Id = "CopyPermission";
            this.copyPermission.ToolTip = null;
            this.copyPermission.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.copyPermission_CustomizePopupWindowParams);
            this.copyPermission.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CopyPermission_Execute);
            // 
            // UserPermissionViewController
            // 
            this.Actions.Add(this.copyPermission);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction copyPermission;
    }
}
