﻿using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Settings.Labware
{
    partial class LabwareMaintenanceViewController
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
            //this.TaskChecklistLink = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ChecklistShow = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ChecklistHide = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LabwareBarcode = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LabwareBarcodeBig = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TaskChecklistLink
            // 
            //this.TaskChecklistLink.AcceptButtonCaption = null;
            //this.TaskChecklistLink.CancelButtonCaption = null;
            //this.TaskChecklistLink.Caption = "Link";
            //this.TaskChecklistLink.Category = "Edit";
            //this.TaskChecklistLink.ConfirmationMessage = null;
            //this.TaskChecklistLink.Id = "TaskChecklistLink";
            //this.TaskChecklistLink.ImageName = "Action_LinkUnlink_Link";
            //this.TaskChecklistLink.ToolTip = null;
            //this.TaskChecklistLink.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowAction1_CustomizePopupWindowParams);
            //this.TaskChecklistLink.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowAction1_Execute);
            // 
            // ChecklistShow
            // 
            this.ChecklistShow.Caption = "Link";
            this.ChecklistShow.Category = "Edit";
            this.ChecklistShow.ConfirmationMessage = null;
            this.ChecklistShow.ImageName = "Action_LinkUnlink_Link";
            this.ChecklistShow.Id = "ChecklistShow";
            this.ChecklistShow.ToolTip = null;
            this.ChecklistShow.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ChecklistShow_Execute);
            // 
            // ChecklistShow
            // 
            this.ChecklistHide.Caption = "Unlink";
            this.ChecklistHide.Category = "Edit";
            this.ChecklistHide.ConfirmationMessage = null;
            this.ChecklistHide.ImageName = "Action_LinkUnlink_Unlink";
            this.ChecklistHide.Id = "ChecklistHide";
            this.ChecklistHide.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            this.ChecklistHide.ToolTip = null;
            this.ChecklistHide.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ChecklistHide_Execute);
            // 
            // LabwareMaintenanceViewController
            // 
            //
            // Labwarebarcode
            //
            //
            this.LabwareBarcode.Caption = "PrintLabwareBarcode";
            this.LabwareBarcode.ConfirmationMessage = null;
            this.LabwareBarcode.Id = "LabwareId";
            this.LabwareBarcode.ToolTip = "";
            this.LabwareBarcode.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LabwareBarcode_Execute);
            //
            //
            // LabwareBarcodeBig
            //
            //
            this.LabwareBarcodeBig.Caption = "PrintLabwareBarcodeBig";
            this.LabwareBarcodeBig.ConfirmationMessage = null;
            this.LabwareBarcodeBig.Id = "BarcodeBig";
            this.LabwareBarcodeBig.ToolTip = null;
            this.LabwareBarcodeBig.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LabwareBarcodeBig_Execute);
            this.Actions.Add(this.LabwareBarcode);
          
            //this.Actions.Add(this.TaskChecklistLink);
            this.Actions.Add(this.ChecklistShow);
            this.Actions.Add(this.ChecklistHide);
            this.Actions.Add(this.LabwareBarcodeBig);
        }



        #endregion

        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction TaskChecklistLink;
        private DevExpress.ExpressApp.Actions.SimpleAction ChecklistShow;
        private DevExpress.ExpressApp.Actions.SimpleAction ChecklistHide;
        private DevExpress.ExpressApp.Actions.SimpleAction LabwareBarcode;
        private DevExpress.ExpressApp.Actions.SimpleAction LabwareBarcodeBig;
    }
}
