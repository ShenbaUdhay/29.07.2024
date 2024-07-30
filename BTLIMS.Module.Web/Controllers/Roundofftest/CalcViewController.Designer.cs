namespace LDM.Module.Web.Controllers.Roundofftest
{
    partial class CalcViewController
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
            this.Roundoffcalc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Roundoffclear = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Roundoffcalc
            // 
            this.Roundoffcalc.Caption = "Show Results!";
            this.Roundoffcalc.Category = "Roundoffcalc";
            this.Roundoffcalc.ConfirmationMessage = null;
            this.Roundoffcalc.Id = "Roundoffcalc";
            this.Roundoffcalc.ToolTip = null;
            this.Roundoffcalc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Roundoffcalc_Execute);
            // 
            // Roundoffclear
            // 
            this.Roundoffclear.Caption = "Clear";
            this.Roundoffclear.Category = "Roundoffclear";
            this.Roundoffclear.ConfirmationMessage = null;
            this.Roundoffclear.Id = "Roundoffclear";
            this.Roundoffclear.ToolTip = null;
            this.Roundoffclear.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Roundoffclear_Execute);
            // 
            // CalcViewController
            // 
            this.Actions.Add(this.Roundoffcalc);
            this.Actions.Add(this.Roundoffclear);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Roundoffcalc;
        private DevExpress.ExpressApp.Actions.SimpleAction Roundoffclear;
    }
}
