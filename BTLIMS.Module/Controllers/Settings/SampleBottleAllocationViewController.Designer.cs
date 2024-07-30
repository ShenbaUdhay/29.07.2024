
namespace Labmaster.Module.Controllers.Settings
{
    partial class SampleBottleAllocationViewController
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
            this.ADDTests = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RemoveTests = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ADDTests
            // 
            this.ADDTests.Caption = "ADD Test";
            this.ADDTests.Category = "RecordEdit";
            this.ADDTests.ConfirmationMessage = null;
            this.ADDTests.Id = "ADDTests";
            this.ADDTests.ImageName = "Add_16x16";
            this.ADDTests.ToolTip = null;
            this.ADDTests.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ADDTests_Execute);
            // 
            // RemoveTests
            // 
            this.RemoveTests.Caption = "Remove Test";
            this.RemoveTests.Category = "RecordEdit";
            this.RemoveTests.ConfirmationMessage = null;
            this.RemoveTests.Id = "RemoveTests";
            this.RemoveTests.ImageName = "Remove";
            this.RemoveTests.ToolTip = null;
            this.RemoveTests.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RemoveTests_Execute);
            // 
            // SampleBottleAllocationViewController
            // 
            this.Actions.Add(this.ADDTests);
            this.Actions.Add(this.RemoveTests);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ADDTests;
        private DevExpress.ExpressApp.Actions.SimpleAction RemoveTests;
    }
}
