
using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.CRM
{
    partial class CRMContactLogViewController
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
            this.Notify = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ADD = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);

            // 
            // ReActivate
            // 
            this.Notify.Caption = "Notify";
            this.Notify.ConfirmationMessage = null;
            this.Notify.Id = "Notify";
            //this.Notify.ImageName = "Action_Workflow_Activate";
            this.Notify.ToolTip = null;
            this.Notify.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Notify_Execute);

            // 
            // ReActivate
            // 
            this.ADD.Caption = "ADD";
            this.ADD.ConfirmationMessage = null;
            this.ADD.Id = "CRMADD";
            this.ADD.Category = "Edit";
            this.ADD.ImageName = "Add_16x16";
            this.ADD.ToolTip = null;
            this.ADD.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ADD_CustomizePopupWindowParams);


            //CRMContactLogViewController
            this.Actions.Add(this.Notify);
            this.Actions.Add(this.ADD);
        }
        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Notify;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ADD;        
    }
}
