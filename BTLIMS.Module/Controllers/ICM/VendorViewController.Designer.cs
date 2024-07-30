
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.ICM
{
    partial class VendorViewController
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
            this.AddEvaluationItem = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.ADDROW = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportFile = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ADDPrice = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);            
			// 
            // AddEvaluationItem
            // 
            this.AddEvaluationItem.Caption = "Add";
            this.AddEvaluationItem.Category = "View";
            this.AddEvaluationItem.ConfirmationMessage = null;
            this.AddEvaluationItem.Id = "AddEvaluationItem";
            this.AddEvaluationItem.ToolTip = null;
            this.AddEvaluationItem.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddEvaluationItem_Execute_1);
            // 
            // ADDROW
            // 
            //this.ADDROW.Caption = "ADD";
            //this.ADDROW.ConfirmationMessage = null;
            //this.ADDROW.Id = "ADDROW";
            //this.ADDROW.ToolTip = null;
            //this.ADDROW.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ADDROW_Execute);
            // 
            // ImportFile
            // 
            this.ImportFile.Caption = "Import File";
            this.ImportFile.ConfirmationMessage = null;
            this.ImportFile.Id = "ImportFile";
            this.ImportFile.ToolTip = null;
            this.ImportFile.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImportFile_Execute);
            // 
            // ADDPrice
            // 
            this.ADDPrice.Caption = "Import File";
            this.ADDPrice.ConfirmationMessage = null;
            this.ADDPrice.Id = "ADDPrice";
            this.ADDPrice.ToolTip = null;
            this.ADDPrice.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ADDPrice_Execute);
            // 
            // VendorViewController
            // 
            this.Actions.Add(this.AddEvaluationItem);
            //this.Actions.Add(this.ADDROW);
            this.Actions.Add(this.ImportFile);
            this.Actions.Add(this.ADDPrice);

        }

        #endregion

        private SimpleAction AddEvaluationItem;
        //private DevExpress.ExpressApp.Actions.SimpleAction ADDROW;
        private DevExpress.ExpressApp.Actions.SimpleAction ImportFile;
        private DevExpress.ExpressApp.Actions.SimpleAction ADDPrice;
    }

}
