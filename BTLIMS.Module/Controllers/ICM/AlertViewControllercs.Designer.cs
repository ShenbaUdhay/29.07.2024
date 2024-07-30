namespace LDM.Module.Controllers.ICM
{
    partial class AlertViewControllercs
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
            this.OrderAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // OrderAction
            // 
            this.OrderAction.Caption = "Requisition";
            this.OrderAction.Category = "RecordEdit";
            this.OrderAction.ConfirmationMessage = null;
            this.OrderAction.Id = "OrderActionId";
            this.OrderAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.OrderAction.ToolTip = null;
            this.OrderAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OrderAction_Execute);
            //this.OrderAction.Executed += new DevExpress.ExpressApp.Actions.ActionBaseEventArgs(this.OrderAction_Executed);
            // 
            // AlertViewControllercs
            // 
            this.Actions.Add(this.OrderAction);
        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction OrderAction;
    }
}
