
namespace Labmaster.Module.Controllers.TaskManagement
{
    partial class SamplingFieldConfigurationViewController
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
            this.addFieldConfiguration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.removeFieldConfiguration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.addStationFieldConfiguration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.removeStationFieldConfiguration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.addTestConfiguration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.removeTestConfiguration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // addFieldConfiguration
            // 
            this.addFieldConfiguration.Caption = "Add";
            this.addFieldConfiguration.Category = "ListView";
            this.addFieldConfiguration.ConfirmationMessage = null;
            this.addFieldConfiguration.Id = "addFieldConfiguration";
            this.addFieldConfiguration.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.addFieldConfiguration.ToolTip = null;
            this.addFieldConfiguration.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.addFieldConfiguration_Execute);
            // 
            // removeFieldConfiguration
            // 
            this.removeFieldConfiguration.Caption = "Remove";
            this.removeFieldConfiguration.Category = "ListView";
            this.removeFieldConfiguration.ConfirmationMessage = null;
            this.removeFieldConfiguration.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.removeFieldConfiguration.Id = "removeFieldConfiguration";
            this.removeFieldConfiguration.ToolTip = null;
            this.removeFieldConfiguration.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.removeFieldConfiguration_Execute);
            // 
            // addStationFieldConfiguration
            // 
            this.addStationFieldConfiguration.Caption = "Add";
            this.addStationFieldConfiguration.Category = "ListView";
            this.addStationFieldConfiguration.ConfirmationMessage = null;
            this.addStationFieldConfiguration.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.addStationFieldConfiguration.Id = "addStationFieldConfiguration";
            this.addStationFieldConfiguration.ToolTip = null;
            this.addStationFieldConfiguration.Execute += AddStationFieldConfiguration_Execute;
            // 
            // removeStationFieldConfiguration
            // 
            this.removeStationFieldConfiguration.Caption = "Remove";
            this.removeStationFieldConfiguration.Category = "ListView";
            this.removeStationFieldConfiguration.ConfirmationMessage = null;
            this.removeStationFieldConfiguration.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.removeStationFieldConfiguration.Id = "removeStationFieldConfiguration";
            this.removeStationFieldConfiguration.ToolTip = null;
            this.removeStationFieldConfiguration.Execute += RemoveStationFieldConfiguration_Execute;
            // 
            // addTestConfiguration
            // 
            this.addTestConfiguration.Caption = "Add";
            this.addTestConfiguration.Category = "ListView";
            this.addTestConfiguration.ConfirmationMessage = null;
            this.addTestConfiguration.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.addTestConfiguration.Id = "addTestConfiguration";
            this.addTestConfiguration.ToolTip = null;
            this.addTestConfiguration.Execute += AddTestConfiguration_Execute;
            // 
            // removeTestConfiguration
            // 
            this.removeTestConfiguration.Caption = "Remove";
            this.removeTestConfiguration.Category = "ListView";
            this.removeTestConfiguration.ConfirmationMessage = null;
            this.removeTestConfiguration.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.removeTestConfiguration.Id = "removeTestConfiguration";
            this.removeTestConfiguration.ToolTip = null;
            this.removeTestConfiguration.Execute += RemoveTestConfiguration_Execute;
            // 
            // SamplingFieldConfigurationViewController
            // 
            this.Actions.Add(this.addFieldConfiguration);
            this.Actions.Add(this.removeFieldConfiguration);
            this.Actions.Add(this.addStationFieldConfiguration);
            this.Actions.Add(this.removeStationFieldConfiguration);
            this.Actions.Add(this.addTestConfiguration);
            this.Actions.Add(this.removeTestConfiguration);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction addFieldConfiguration;
        private DevExpress.ExpressApp.Actions.SimpleAction removeFieldConfiguration;
        private DevExpress.ExpressApp.Actions.SimpleAction addStationFieldConfiguration;
        private DevExpress.ExpressApp.Actions.SimpleAction removeStationFieldConfiguration;
        private DevExpress.ExpressApp.Actions.SimpleAction addTestConfiguration;
        private DevExpress.ExpressApp.Actions.SimpleAction removeTestConfiguration;
    }
}
