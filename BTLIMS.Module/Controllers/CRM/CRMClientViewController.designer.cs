
namespace LDM.Module.Controllers.CRM
{
    partial class CRMClientViewController
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
            this.ReActivate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DeActivate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ReActivate
            // 
            this.ReActivate.Caption = "ReActivate";
            this.ReActivate.ConfirmationMessage = null;
            this.ReActivate.Id = "ReActivate";
            this.ReActivate.ImageName = "Action_Workflow_Activate";
            this.ReActivate.ToolTip = null;
            this.ReActivate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReActivate_Execute);
            // 
            // DeActivate
            // 
            this.DeActivate.Caption = "DeActivate";
            this.DeActivate.ConfirmationMessage = null;
            this.DeActivate.Id = "DeActivate";
            this.DeActivate.ImageName = "Action_Workflow_Deactivate";
            this.DeActivate.ToolTip = null;
            this.DeActivate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeActivate_Execute);
            // 
            // CRMClientViewController
            // 
            this.Actions.Add(this.ReActivate);
            this.Actions.Add(this.DeActivate);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ReActivate;
        private DevExpress.ExpressApp.Actions.SimpleAction DeActivate;
    }
}
