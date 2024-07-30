namespace LDM.Module.Controllers.ICM
{
    partial class ICMRollbackViewController
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
            this.ICMRollBackAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ICMRollBackAction
            // 
            this.ICMRollBackAction.AcceptButtonCaption = null;
            this.ICMRollBackAction.CancelButtonCaption = null;
            this.ICMRollBackAction.Caption = "RollBack";
            this.ICMRollBackAction.Category = "View";
            this.ICMRollBackAction.ConfirmationMessage = null;
            this.ICMRollBackAction.Id = "ICMRollBackAction";
            this.ICMRollBackAction.ToolTip = null;
            this.ICMRollBackAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.ICMRollBackAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ICMRollBack_CustomizePopupWindowParams);
            this.ICMRollBackAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ICMRollBack_Execute);
            // 
            // ICMRollbackViewController
            // 
            this.Actions.Add(this.ICMRollBackAction);

        }



        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ICMRollBackAction;
    }
}
