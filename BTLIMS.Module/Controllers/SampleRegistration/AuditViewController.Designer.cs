
namespace LDM.Module.Controllers.SampleRegistration
{
    partial class AuditViewController
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
            this.Audit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Audit
            // 
            this.Audit.Caption = "AuditTrail";
            this.Audit.Category = "Export";
            this.Audit.ConfirmationMessage = null;
            this.Audit.Id = "AuditTrail";
            this.Audit.ToolTip = null;
            this.Audit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Audit_Execute);
            // 
            // AuditViewController
            // 
            this.Actions.Add(this.Audit);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Audit;
    }
}
