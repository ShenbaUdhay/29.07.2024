
namespace LDM.Module.Controllers.Settings
{
    partial class CheckPointsSetupViewController
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
            this.CheckPointSetupSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CheckPointSetupSave
            // 
            this.CheckPointSetupSave.Caption = "Save";
            this.CheckPointSetupSave.Category = "Save";
            this.CheckPointSetupSave.ConfirmationMessage = null;
            this.CheckPointSetupSave.Id = "CheckPointSetupSave";
            this.CheckPointSetupSave.ImageName = "Action_Save";
            this.CheckPointSetupSave.ToolTip = null;
            this.CheckPointSetupSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CheckPointSetupSave_Execute);
            // 
            // CheckPointsSetupViewController
            // 
            this.Actions.Add(this.CheckPointSetupSave);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CheckPointSetupSave;
    }
}
