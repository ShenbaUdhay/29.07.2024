
namespace LDM.Module.Web.Controllers.Settings
{
    partial class DBConnectionWindowController
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
            this.DBConnection = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.singleChoiceAction1 = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // DBConnection
            // 
            this.DBConnection.Caption = "DB Connection";
            this.DBConnection.Category = "Notifications";
            this.DBConnection.ConfirmationMessage = null;
            this.DBConnection.Id = "DBConnection";
            this.DBConnection.ToolTip = null;
            this.DBConnection.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DBConnection_Execute);
            // 
            // singleChoiceAction1
            // 
            this.singleChoiceAction1.Caption = null;
            this.singleChoiceAction1.Category = "Notifications";
            this.singleChoiceAction1.ConfirmationMessage = null;
            this.singleChoiceAction1.Id = "86b5f5d7-8ed4-4414-bd06-6819d4e7fe64";
            this.singleChoiceAction1.ToolTip = null;
            this.singleChoiceAction1.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.singleChoiceAction1_Execute);
            // 
            // DBConnectionWindowController
            // 
            this.Actions.Add(this.DBConnection);
            this.Actions.Add(this.singleChoiceAction1);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction DBConnection;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction singleChoiceAction1;
    }
}
