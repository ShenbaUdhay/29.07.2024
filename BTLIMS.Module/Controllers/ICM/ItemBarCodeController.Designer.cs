namespace LDM.Module.Controllers.ICM
{
    partial class ItemBarCodeController
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
            this.ItemBarCodeAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ItemBarCodeAction
            // 
            this.ItemBarCodeAction.Caption = "Item Bar Code";
            this.ItemBarCodeAction.ConfirmationMessage = null;
            this.ItemBarCodeAction.Id = "ItemBarCodeAction";
            this.ItemBarCodeAction.ImageName = "BO_Report";
            this.ItemBarCodeAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.ItemBarCodeAction.ToolTip = null;
            this.ItemBarCodeAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ItemBarCodeAction_Execute);
            // 
            // ItemBarCodeController
            // 
            this.Actions.Add(this.ItemBarCodeAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ItemBarCodeAction;
    }
}
