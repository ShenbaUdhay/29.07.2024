namespace LDM.Module.Controllers.Settings
{
    partial class ParserSetupViewController
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
            this.ShowRetired = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ShowRetired
            // 
            this.ShowRetired.Caption = "ShowRetired";
            this.ShowRetired.ConfirmationMessage = null;
            this.ShowRetired.Id = "ShowRetired";
            this.ShowRetired.TargetObjectType = typeof(Modules.BusinessObjects.Setting.InstrumentSoftware);
            this.ShowRetired.TargetViewId = "InstrumentSoftware_ListView";
            this.ShowRetired.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ShowRetired.ToolTip = null;
            this.ShowRetired.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ShowRetired.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ShowRetired_Execute);
            // 
            // ParserSetupViewController
            // 
            this.Actions.Add(this.ShowRetired);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ShowRetired;
    }
}
