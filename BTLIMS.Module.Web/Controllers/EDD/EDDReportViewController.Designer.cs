namespace LDM.Module.Web.Controllers.Settings
{
    partial class EDDReportViewController
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
        /// 
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ClearEDD = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ExportToEDD
            // 
            this.ClearEDD.Caption = "Clear";
            this.ClearEDD.Category = "PopupActions";
            this.ClearEDD.ConfirmationMessage = null;
            this.ClearEDD.Id = "ClearEDD";
            this.ClearEDD.ToolTip = null;
            this.ClearEDD.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClearEDD_Execute);
            this.Actions.Add(this.ClearEDD);
        }
        private DevExpress.ExpressApp.Actions.SimpleAction ClearEDD;
        #endregion
    }
}
