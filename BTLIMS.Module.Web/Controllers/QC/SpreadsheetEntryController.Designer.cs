﻿namespace LDM.Module.Web.Controllers.QC
{
    partial class SpreadsheetEntryController
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
            this.Import = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Import
            // 
            this.Import.Caption = "Import";
            this.Import.Category = "PopupActions";
            this.Import.ConfirmationMessage = null;
            this.Import.Id = "Import";
            this.Import.ToolTip = null;
            this.Import.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Import_Execute);
            // 
            // SpreadsheetEntryController
            // 
            this.Actions.Add(this.Import);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Import;
    }
}
