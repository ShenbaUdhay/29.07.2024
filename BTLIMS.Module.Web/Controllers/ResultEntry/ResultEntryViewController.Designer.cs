﻿using DevExpress.ExpressApp.Actions;
using System;

namespace BTLIMS.Module.Controllers.ResultEntry
{
    partial class ResultEntryViewController
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
            this.ResultValidation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ResultApproval = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Rollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.ResultEnter = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ResultDelete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ResultSubmit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SampleRegistration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ExportResulyEntry = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportResultentry = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ResultValidation
            // 
            this.ResultValidation.Caption = "Result Validate";
            this.ResultValidation.ConfirmationMessage = null;
            this.ResultValidation.Id = "ResultValidation";
            this.ResultValidation.ToolTip = null;
            this.ResultValidation.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ResultValidation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ResultValidation_Execute);
            // 
            // ResultApproval
            // 
            this.ResultApproval.Caption = "Result Approve";
            this.ResultApproval.ConfirmationMessage = null;
            this.ResultApproval.Id = "ResultApproval";
            this.ResultApproval.ToolTip = null;
            this.ResultApproval.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ResultApproval.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ResultApproval_Execute);
            // 
            // Rollback
            // 
            this.Rollback.Caption = "Rollback";
            this.Rollback.Category = "View";
            this.Rollback.ConfirmationMessage = null;
            this.Rollback.Id = "Rollback";
            this.Rollback.ToolTip = null;
            this.Rollback.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.Rollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Rollback_Execute);
            // 
            // ResultEnter
            // 
            //this.ResultEnter.Caption = "Save";
            //this.ResultEnter.Category = "Save";
            //this.ResultEnter.ConfirmationMessage = null;
            //this.ResultEnter.Id = "ResultEnter";
            ////this.ResultEnter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            //this.ResultEnter.ToolTip = null;
            //this.ResultEnter.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            //this.ResultEnter.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveResult_Execute);
            // 
            // ResultDelete
            // 
            this.ResultDelete.Caption = "Delete";
            this.ResultDelete.Category = "View";
            this.ResultDelete.ConfirmationMessage = null;
            this.ResultDelete.Id = "ResultDelete";
            //this.ResultDelete.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ResultDelete.ToolTip = null;
            //this.ResultDelete.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ResultDelete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ResultDelete_Execute);
            // 
            // ResultSubmit
            // 
            this.ResultSubmit.Caption = "Submit";
            this.ResultSubmit.Category = "View";
            this.ResultSubmit.ConfirmationMessage = null;
            this.ResultSubmit.Id = "ResultSubmit";
            this.ResultSubmit.ImageName = "Submit_image";
            this.ResultSubmit.ToolTip = null;
            //this.ResultSubmit.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            // 
            // SampleRegistration
            // 
            this.SampleRegistration.Caption = "Sample Registration";
            this.SampleRegistration.Category = "View";
            this.SampleRegistration.ConfirmationMessage = null;
            this.SampleRegistration.Id = "SampleRegistration";
            this.SampleRegistration.ToolTip = null;
            this.SampleRegistration.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SampleRegistration_Execute);
            // 
            // ExportResulyEntry
            // 
            this.ExportResulyEntry.Caption = "Export File";
            this.ExportResulyEntry.Category = "RecordEdit";
            this.ExportResulyEntry.ConfirmationMessage = null;
            this.ExportResulyEntry.Id = "ExportResulyEntry";
            this.ExportResulyEntry.ToolTip = null;
            this.ExportResulyEntry.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportResulyEntry_Execute);
            // 
            // ImportResultentry
            // 
            this.ImportResultentry.AcceptButtonCaption = null;
            this.ImportResultentry.CancelButtonCaption = null;
            this.ImportResultentry.Caption = "Import File";
            this.ImportResultentry.Category = "RecordEdit";
            this.ImportResultentry.ConfirmationMessage = null;
            this.ImportResultentry.Id = "ImportResultEntry";
            this.ImportResultentry.ToolTip = null;
            this.ImportResultentry.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportFromFileAction_CustomizePopupWindowParams);
            this.ImportResultentry.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportResultentry_Execute);
            // 
            // ResultEntryViewController
            // 
            this.Actions.Add(this.ResultValidation);
            this.Actions.Add(this.ResultApproval);
            this.Actions.Add(this.Rollback);
            //this.Actions.Add(this.ResultEnter);
            this.Actions.Add(this.ResultDelete);
            this.Actions.Add(this.ResultSubmit);
            this.Actions.Add(this.SampleRegistration);
            this.Actions.Add(this.ExportResulyEntry);
            this.Actions.Add(this.ImportResultentry);

        }
        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ResultValidation;
        private DevExpress.ExpressApp.Actions.SimpleAction ResultApproval;
        private DevExpress.ExpressApp.Actions.SimpleAction Rollback;
        //private DevExpress.ExpressApp.Actions.SimpleAction ResultEnter;
        private DevExpress.ExpressApp.Actions.SimpleAction ResultDelete;
        private DevExpress.ExpressApp.Actions.SimpleAction ResultSubmit;
        private DevExpress.ExpressApp.Actions.SimpleAction SampleRegistration;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportResulyEntry;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportResultentry;
    }
}
