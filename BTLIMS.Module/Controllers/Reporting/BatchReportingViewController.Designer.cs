
using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.Reporting
{
    partial class BatchReportingViewController
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
            this.JobIdsRetrieve = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.JobIdsRefresh = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.BatchPreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.BatchSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // JobIdsRetrieve
            // 
            this.JobIdsRetrieve.Caption = "Retrieve";
            //this.JobIdsRetrieve.Category = "BatchReporting";
            this.JobIdsRetrieve.ConfirmationMessage = null;
            this.JobIdsRetrieve.Id = "JobIdsRetrieve";
            this.JobIdsRetrieve.ImageName = "Action_Redo";
            this.JobIdsRetrieve.ToolTip = null;
            this.JobIdsRetrieve.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.JobIdsRetrieve_Execute);
            // 
            // JobIdsRefresh
            // 
            this.JobIdsRefresh.Caption = "Refresh";
            //this.JobIdsRefresh.Category = "BatchReportingRefresh";
            this.JobIdsRefresh.ConfirmationMessage = null;
            this.JobIdsRefresh.ImageName = "Action_Refresh";
            this.JobIdsRefresh.Id = "JobIdsRefresh";
            this.JobIdsRefresh.ToolTip = null;
            this.JobIdsRefresh.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.JobIdsRefresh_Execute);
            // 
            // BatchPreview
            // 
            this.BatchPreview.Caption = "Preview";
            this.BatchPreview.Category = "ListView";
            this.BatchPreview.ConfirmationMessage = null;
            this.BatchPreview.Id = "BatchPreview";
            this.BatchPreview.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.BatchPreview.ToolTip = null;
            this.BatchPreview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BatchPreview_Execute);
            // 
            // BatchSave
            // 
            this.BatchSave.Caption = "Save";
            this.BatchSave.ConfirmationMessage = "Do you want to generate the reports for the selected Job IDs?";
            this.BatchSave.Id = "BatchSave";
            this.BatchSave.ImageName = "Save_16x16";
            this.BatchSave.ToolTip = null;
            this.BatchSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BatchSave_Execute);
            // 
            // BatchReportingViewController
            // 
            this.Actions.Add(this.JobIdsRetrieve);
            this.Actions.Add(this.JobIdsRefresh);
            this.Actions.Add(this.BatchPreview);
            this.Actions.Add(this.BatchSave);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction JobIdsRetrieve;
        private DevExpress.ExpressApp.Actions.SimpleAction JobIdsRefresh;
        private DevExpress.ExpressApp.Actions.SimpleAction BatchPreview;
        private DevExpress.ExpressApp.Actions.SimpleAction BatchSave;
    }
}
