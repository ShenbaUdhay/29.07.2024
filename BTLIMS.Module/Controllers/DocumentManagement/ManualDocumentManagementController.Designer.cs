namespace LDM.Module.Controllers.DocumentManagement
{
    partial class ManualDocumentManagementController
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

            this.btnUnRetire = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);

            this.btnUnRetire.Caption = "Reactivate";
            this.btnUnRetire.ConfirmationMessage = "Do you want reactivate the document?";
            this.btnUnRetire.ImageName = "Action_Workflow_Activate";
            this.btnUnRetire.Id = "btnUnRetireTheFile";
            this.btnUnRetire.ToolTip = null;
            this.btnUnRetire.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnUnRetire_simpleAction1_Execute);

            this.Actions.Add(this.btnUnRetire);
        }

        private DevExpress.ExpressApp.Actions.SimpleAction btnUnRetire;
        #endregion
    }
}
