using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Barcode_Sample_Custody
{
    partial class SampleInOutController
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
            this.btn_Disposal_History = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SAHistory
            // 
            this.btn_Disposal_History.Caption = "History";
            this.btn_Disposal_History.ConfirmationMessage = null;
            this.btn_Disposal_History.Id = "btn_Disposal_History";
            this.btn_Disposal_History.ImageName = "Action_Search";
            this.btn_Disposal_History.ToolTip = null;
            this.btn_Disposal_History.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_Disposal_History_Execute);
            // 
            // SampleInOutHistoryController
            // 
            this.Actions.Add(this.btn_Disposal_History);
        }
        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction btn_Disposal_History;
    }
}
