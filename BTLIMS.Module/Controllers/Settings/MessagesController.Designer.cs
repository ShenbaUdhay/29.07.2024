using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers
{
    partial class MessagesController
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
            this.ImportMessagesFromFileAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ImportMessagesFromFileAction
            // 
            this.ImportMessagesFromFileAction.AcceptButtonCaption = null;
            this.ImportMessagesFromFileAction.CancelButtonCaption = null;
            this.ImportMessagesFromFileAction.Caption = "Import File";
            this.ImportMessagesFromFileAction.Category = "RecordEdit";
            this.ImportMessagesFromFileAction.ConfirmationMessage = null;
            this.ImportMessagesFromFileAction.Id = "ImportMessagesFromFileAction";
            this.ImportMessagesFromFileAction.ToolTip = null;
            this.ImportMessagesFromFileAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportMessagesFromFileAction_CustomizePopupWindowParams);
            this.ImportMessagesFromFileAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportFileAction_Execute);
            // 
            // MessagesController
            // 
            this.Actions.Add(this.ImportMessagesFromFileAction);

        }


        #endregion
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportMessagesFromFileAction;
    }
}
