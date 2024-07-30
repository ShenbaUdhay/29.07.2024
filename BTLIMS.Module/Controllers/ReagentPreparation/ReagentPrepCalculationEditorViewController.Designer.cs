
using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.ReagentPreparation
{
    partial class ReagentPrepCalculationEditorViewController
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
            this.ReagentPrepFormula = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);

            //ReagentPrepFormula


            this.ReagentPrepFormula.Caption = "Formula";
            this.ReagentPrepFormula.Category = "catReagentFormula";
            this.ReagentPrepFormula.ConfirmationMessage = null;
            this.ReagentPrepFormula.Id = "ReagentPrepFormula";
            //this.ReagentPrepFormula.ImageName = "import_data_16x16";
            this.ReagentPrepFormula.ToolTip = null;
            this.ReagentPrepFormula.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Formula_Execute);

            // 
            // ReagentPrepCalculationEditorViewController
            // 
            this.Actions.Add(this.ReagentPrepFormula);
        }

       

        private DevExpress.ExpressApp.Actions.SimpleAction ReagentPrepFormula;
        #endregion
    }
}
