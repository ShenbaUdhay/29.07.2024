
using System;
using DevExpress.ExpressApp.Actions;

namespace Labmaster.Module.Controllers.SamplingManagement
{
    partial class FieldDataReviewController
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
            this.FieldDataValidated = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataApproved = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataRollBack = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataApprovedRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataReviewRecord = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);

            // 
            // FieldDataValidated
            // 
            this.FieldDataValidated.Caption = "Validate";
            this.FieldDataValidated.ConfirmationMessage = null;
            this.FieldDataValidated.Id = "FieldDataValidated";
            this.FieldDataValidated.ToolTip = null;
            this.FieldDataValidated.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataValidated_Execute);
            // 
            // FieldDataApproved
            // 
            this.FieldDataApproved.Caption = "Approve";
            this.FieldDataApproved.ConfirmationMessage = null;
            this.FieldDataApproved.Id = "FieldDataApproved";
            this.FieldDataApproved.ToolTip = null;
            this.FieldDataApproved.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataApproved_Execute);
            // 
            // FieldDataRollBack
            // 
            this.FieldDataRollBack.Caption = "Rollback";
            this.FieldDataRollBack.ConfirmationMessage = null;
            this.FieldDataRollBack.Id = "FieldDataRollBack";
            this.FieldDataRollBack.ToolTip = null;
            this.FieldDataRollBack.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataRollBack_Execute);
            // 
            // FieldDataApprovedRollback
            // 
            this.FieldDataApprovedRollback.Caption = "Rollback";
            this.FieldDataApprovedRollback.ConfirmationMessage = null;
            this.FieldDataApprovedRollback.Id = "FieldDataApprovedRollback";
            this.FieldDataApprovedRollback.ToolTip = null;
            this.FieldDataApprovedRollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataApprovedRollback_Execute);
            // 
            // FieldDataReviewView
            // 
            this.FieldDataReviewRecord.Caption = "Status Filter";
            this.FieldDataReviewRecord.ConfirmationMessage = null;
            this.FieldDataReviewRecord.Id = "FieldDataReviewRecord";
            this.FieldDataReviewRecord.ToolTip = null;
            this.FieldDataReviewRecord.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.FieldDataReviewFilter_Excecute);
            // 
            // FieldDataReviewController
            // 
            this.Actions.Add(this.FieldDataValidated);
            this.Actions.Add(this.FieldDataApproved);
            this.Actions.Add(this.FieldDataRollBack);
            this.Actions.Add(this.FieldDataApprovedRollback);
            this.Actions.Add(this.FieldDataReviewRecord);

        }



        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataValidated;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataApproved;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataRollBack;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataApprovedRollback;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction FieldDataReviewRecord;
    }
}
