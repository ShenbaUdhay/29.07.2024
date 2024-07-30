
namespace LDM.Module.Controllers.Settings
{
    partial class HelpCenterWindowController
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
            this.HelpCenterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SalesAnalyticsAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // HelpCenterAction
            // 
            this.HelpCenterAction.Caption = "Help Center";
            this.HelpCenterAction.Category = "Notifications";
            this.HelpCenterAction.ConfirmationMessage = null;
            this.HelpCenterAction.Id = "HelpCenterAction";
            this.HelpCenterAction.ToolTip = null;
            this.HelpCenterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.HelpCenterAction_Execute);
            // 
            // HelpCenterWindowController
            // 
            this.Actions.Add(this.HelpCenterAction);


            //salesanalytics
            this.SalesAnalyticsAction.Caption = "Sales Analytics";
            this.SalesAnalyticsAction.Category = "Notifications";
            this.SalesAnalyticsAction.ConfirmationMessage = null;
            this.SalesAnalyticsAction.Id = "salesanalyticsaction";
            this.SalesAnalyticsAction.ToolTip = null;
            this.SalesAnalyticsAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SalesAnalyticsAction_Execute);
            // 
            // salesanalyticsWindowController
            // 
            this.Actions.Add(this.SalesAnalyticsAction);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction HelpCenterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SalesAnalyticsAction;
    }
}
