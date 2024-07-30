namespace Modules.Controllers.Biz
{
    partial class SymbolController
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
            this.SymbolAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // SymbolAction
            // 
            this.SymbolAction.AcceptButtonCaption = null;
            this.SymbolAction.CancelButtonCaption = null;
            this.SymbolAction.Caption = "Symbol";
            this.SymbolAction.Category = "View";
            this.SymbolAction.ConfirmationMessage = null;
            this.SymbolAction.Id = "SymbolAction";
            this.SymbolAction.ToolTip = null;
            this.SymbolAction.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.SymbolAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SymbolAction_CustomizePopupWindowParams);
            this.SymbolAction.CustomizeControl += new System.EventHandler<DevExpress.ExpressApp.Actions.CustomizeControlEventArgs>(this.SymbolAction_CustomizeControl);
            // 
            // SymbolController
            // 
            this.Actions.Add(this.SymbolAction);
            this.TypeOfView = typeof(DevExpress.ExpressApp.View);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SymbolAction;
    }
}
