namespace LDM.Module.Controllers.Settings.Permission
{
    partial class CopyUserPermissionViewController
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
            this.SelectMultipleUser = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // SelectMultipleUser
            // 
            this.SelectMultipleUser.AcceptButtonCaption = null;
            this.SelectMultipleUser.CancelButtonCaption = null;
            this.SelectMultipleUser.Caption = "Select Users";
            this.SelectMultipleUser.Category = "SelectMultipleUser";
            this.SelectMultipleUser.ConfirmationMessage = null;
            this.SelectMultipleUser.Id = "SelectMultipleUser";
            this.SelectMultipleUser.ToolTip = null;
            this.SelectMultipleUser.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SelectMultipleUser_CustomizePopupWindowParams);
            this.SelectMultipleUser.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SelectMultipleUser_Execute);
            // 
            // CopyUserPermissionViewController
            // 
            this.Actions.Add(this.SelectMultipleUser);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SelectMultipleUser;
    }
}
