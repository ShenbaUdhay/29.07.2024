using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Settings
{
    partial class EmployeeController
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
            this.linkRoleToEmployeeAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SetPassword = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // linkRoleToEmployeeAction
            // 
            this.linkRoleToEmployeeAction.Caption = "Link";
            this.linkRoleToEmployeeAction.Category = "Edit";
            this.linkRoleToEmployeeAction.ConfirmationMessage = null;
            this.linkRoleToEmployeeAction.Id = "linkRoleToEmployeeAction";
            this.linkRoleToEmployeeAction.ToolTip = null;
            this.linkRoleToEmployeeAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.linkRoleToEmployeeAction_Execute);

            // 
            // SetPassword
            // 
            this.SetPassword.Caption = "Set Password";
            this.SetPassword.Category = "canSetPassword";
            this.SetPassword.ConfirmationMessage = null;
            this.SetPassword.Id = "SetPassword";
            this.SetPassword.ToolTip = null;
            this.SetPassword.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SetPassword_Excecute);
            // 
            // EmployeeController
            // 
            this.Actions.Add(this.linkRoleToEmployeeAction);
            this.Actions.Add(this.SetPassword);

        }

      

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction linkRoleToEmployeeAction;
        private SimpleAction SetPassword;
    }
}
