namespace LDM.Module.Web.Controllers.Reporting
{
    partial class ReportEmailViewController
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
            this.ReportSentEmail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ReportSentEmail
            // 

            //this.ReportSentEmail.Caption = "Sent Email";
            this.ReportSentEmail.Caption = "Send";
            this.ReportSentEmail.Category = "View";
            this.ReportSentEmail.ConfirmationMessage = null;
            this.ReportSentEmail.Id = "ReportSentEmail";
            this.ReportSentEmail.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ReportSentEmail.ToolTip = null;
            this.ReportSentEmail.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ReportSentEmail.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportSentEmail_Execute);
            // 
            // ReportEmailViewController
            // 
            this.Actions.Add(this.ReportSentEmail);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ReportSentEmail;
    }
}
