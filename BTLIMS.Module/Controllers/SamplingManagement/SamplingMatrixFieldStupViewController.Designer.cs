namespace LDM.Module.Controllers.SamplingManagement.Settings
{
    partial class SamplingMatrixFieldStupViewController
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
            this.AddFieldsSampling = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RemoveFieldsSampling = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AddFields
            // 
            this.AddFieldsSampling.Caption = "Add";
            this.AddFieldsSampling.Category = "catAddSampleFields";
            this.AddFieldsSampling.ConfirmationMessage = null;
            this.AddFieldsSampling.Id = "AddFieldsSampling";
            this.AddFieldsSampling.ToolTip = "Add";
            this.AddFieldsSampling.ImageName = "Add.png";
            this.AddFieldsSampling.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddFields_Execute);
           
            // 
            // RemoveFields
            // 
            this.RemoveFieldsSampling.Caption = "Remove";
            this.RemoveFieldsSampling.Category = "catRemoveSampleFields";
            this.RemoveFieldsSampling.ConfirmationMessage = null;
            this.RemoveFieldsSampling.Id = "RemoveFieldsSampling";
            this.RemoveFieldsSampling.ToolTip = "Remove";
            this.RemoveFieldsSampling.ImageName = "Remove.png";
            this.RemoveFieldsSampling.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RemoveFields_Execute);
           
            // 
            // SampleMatrixSetupFieldsetupController
            // 
            this.Actions.Add(this.AddFieldsSampling);
            this.Actions.Add(this.RemoveFieldsSampling);
        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction AddFieldsSampling;
        private DevExpress.ExpressApp.Actions.SimpleAction RemoveFieldsSampling;
    }
}
