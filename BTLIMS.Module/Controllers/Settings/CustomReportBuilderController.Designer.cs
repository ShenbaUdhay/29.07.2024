namespace LDM.Module.Controllers.CustomReportBuilder
{
    partial class CustomReportBuilderController
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
            this.btnAddReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnNewPackage = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnReportDesigner = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.btnCustomReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // btnAddReport
            // 
            this.btnAddReport.Caption = "Add";
            this.btnAddReport.ConfirmationMessage = null;
            this.btnAddReport.ImageName= "Add.png";
            this.btnAddReport.Category= "Edit";
            this.btnAddReport.Id = "btnAddReport";
            this.btnAddReport.ToolTip = null;
            // 
            // btnNewPackage
            // 
            this.btnNewPackage.Caption = "New";
            this.btnNewPackage.ConfirmationMessage = null;
            this.btnNewPackage.Id = "btnNewPackage";
            this.btnNewPackage.ToolTip = null;
            this.btnNewPackage.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnNewPackage_Execute);
            // 
            // btnReportDesigner
            // 
            this.btnReportDesigner.Caption = "...";
            this.btnReportDesigner.Category = "RecordEdit";
            this.btnReportDesigner.ConfirmationMessage = null;
            this.btnReportDesigner.Id = "btnReportDesigner";
            this.btnReportDesigner.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.btnReportDesigner.ToolTip = null;
            this.btnReportDesigner.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnReportDesigner_Execute);
            // 
            // btnCustomReport
            // 
            //this.btnCustomReport.Caption = "ReportDesign";
            //this.btnCustomReport.Category = "RecordEdit";
            //this.btnCustomReport.ConfirmationMessage = null;
            //this.btnCustomReport.Id = "btnCustomReport";
            //this.btnCustomReport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            //this.btnCustomReport.ToolTip = null;
            //this.btnCustomReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnCustomReport_Execute);
            // 
            // CustomReportBuilderController
            // 
            this.Actions.Add(this.btnAddReport);
            this.Actions.Add(this.btnNewPackage);
            this.Actions.Add(this.btnReportDesigner);
           // this.Actions.Add(this.btnCustomReport);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction btnAddReport;
        private DevExpress.ExpressApp.Actions.SimpleAction btnNewPackage;
        private DevExpress.ExpressApp.Actions.SimpleAction btnReportDesigner;
        private DevExpress.ExpressApp.Actions.SimpleAction btnCustomReport;
    }
}
