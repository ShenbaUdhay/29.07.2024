namespace LDM.Module.Controllers.SamplePreparation
{
    partial class SamplePreTreatmentController
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
            this.SamplePreTreatmentReset = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePreTreatmentPrevious = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePreTreatmentSort = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePreTreatmentLoad = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SamplePreTreatmentReset
            // 
            this.SamplePreTreatmentReset.Caption = "Reset";
            this.SamplePreTreatmentReset.Category = "samplepretreatmentaction0";
            this.SamplePreTreatmentReset.ConfirmationMessage = null;
            this.SamplePreTreatmentReset.Id = "SamplePreTreatmentReset";
            this.SamplePreTreatmentReset.ToolTip = null;
            this.SamplePreTreatmentReset.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePreTreatmentReset_Execute);
            // 
            // SamplePreTreatmentPrevious
            // 
            this.SamplePreTreatmentPrevious.Caption = "Previous";
            this.SamplePreTreatmentPrevious.Category = "samplepretreatmentaction2";
            this.SamplePreTreatmentPrevious.ConfirmationMessage = null;
            this.SamplePreTreatmentPrevious.Id = "SamplePreTreatmentPrevious";
            this.SamplePreTreatmentPrevious.ToolTip = null;
            this.SamplePreTreatmentPrevious.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePreTreatmentPrevious_Execute);
            // 
            // SamplePreTreatmentSort
            // 
            this.SamplePreTreatmentSort.Caption = "Sort";
            this.SamplePreTreatmentSort.Category = "samplepretreatmentaction2";
            this.SamplePreTreatmentSort.ConfirmationMessage = null;
            this.SamplePreTreatmentSort.Id = "SamplePreTreatmentSort";
            this.SamplePreTreatmentSort.ToolTip = null;
            this.SamplePreTreatmentSort.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePreTreatmentSort_Execute);
            // 
            // SamplePreTreatmentLoad
            // 
            this.SamplePreTreatmentLoad.Caption = "Load";
            this.SamplePreTreatmentLoad.Category = "samplepretreatmentaction0";
            this.SamplePreTreatmentLoad.ConfirmationMessage = null;
            this.SamplePreTreatmentLoad.Id = "SamplePreTreatmentLoad";
            this.SamplePreTreatmentLoad.ToolTip = null;
            this.SamplePreTreatmentLoad.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePreTreatmentLoad_Execute);
            // 
            // SamplePreTreatmentController
            // 
            this.Actions.Add(this.SamplePreTreatmentReset);
            this.Actions.Add(this.SamplePreTreatmentPrevious);
            this.Actions.Add(this.SamplePreTreatmentSort);
            this.Actions.Add(this.SamplePreTreatmentLoad);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SamplePreTreatmentReset;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePreTreatmentPrevious;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePreTreatmentSort;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePreTreatmentLoad;
    }
}
