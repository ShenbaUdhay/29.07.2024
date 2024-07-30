namespace LDM.Module.Controllers.SampleLogIn
{
    partial class SampleLoginBarCodeReportViewController
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
            this.BarcodeReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FolderLabel = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // BarcodeReport
            // 
            this.BarcodeReport.Caption = "Barcode Report";
            //this.BarcodeReport.Category = "PopupActions";
            this.BarcodeReport.ConfirmationMessage = null;
            this.BarcodeReport.Id = "BarcodeReport";
            this.BarcodeReport.ToolTip = null;
            this.BarcodeReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BarcodeReport_Execute);
            // 
            // FolderLabel
            // 
            this.FolderLabel.Caption = "Folder Label";
            //this.FolderLabel.Category = "PopupActions";
            this.FolderLabel.ConfirmationMessage = null;
            this.FolderLabel.Id = "FolderLabel";
            this.FolderLabel.ToolTip = null;
            this.FolderLabel.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FolderLabel_Execute);
            // 
            // SampleLoginBarCodeReportViewController
            // 
            this.Actions.Add(this.BarcodeReport);
            this.Actions.Add(this.FolderLabel);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction BarcodeReport;
        private DevExpress.ExpressApp.Actions.SimpleAction FolderLabel;
    }
}
