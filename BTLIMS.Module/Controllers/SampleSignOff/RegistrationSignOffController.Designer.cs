using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SampleSignOff
{
    partial class RegistrationSignOffController
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
            this.SignOffAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SignedOffHistoryAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SignedOffSamplesAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RegistrationSignOffDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.RegistrationSignOffRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SubmittedJobIDRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SignOffAction
            // 
            this.SignOffAction.Caption = "Sign Off";
            this.SignOffAction.Category = "Edit";
            this.SignOffAction.ConfirmationMessage = null;
            this.SignOffAction.Id = "SignOffAction";
            this.SignOffAction.ImageName = "BO_Validation";
            this.SignOffAction.ToolTip = "Sign Off";
            this.SignOffAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SignOffAction_Execute);
            // 
            // SignedOffHistoryAction
            // 
            this.SignedOffHistoryAction.Caption = "History";
            this.SignedOffHistoryAction.ConfirmationMessage = null;
            this.SignedOffHistoryAction.Id = "SignedOffHistoryAction";
            this.SignedOffHistoryAction.ImageName = "Action_Search";
            this.SignedOffHistoryAction.ToolTip = null;
            this.SignedOffHistoryAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SignedOffHistoryAction_Execute);
            // 
            // SignedOffSamplesAction
            // 
            this.SignedOffSamplesAction.Caption = "View Bottles";
            this.SignedOffSamplesAction.Category = "View";
            this.SignedOffSamplesAction.ConfirmationMessage = null;
            this.SignedOffSamplesAction.Id = "SignedOffSamplesAction";
            this.SignedOffSamplesAction.ToolTip = null;
            this.SignedOffSamplesAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SignedOffSamplesAction_Execute);
            // 
            // RegistrationSignOffDateFilterAction
            // 
            this.RegistrationSignOffDateFilterAction.Caption = "Date Filter";
            this.RegistrationSignOffDateFilterAction.Category = "View";
            this.RegistrationSignOffDateFilterAction.ConfirmationMessage = null;
            this.RegistrationSignOffDateFilterAction.Id = "RegistrationSignOffDateFilterAction";
            choiceActionItem1.Caption = "3M";
            choiceActionItem1.Id = "3M";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "6M";
            choiceActionItem2.Id = "6M";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "1Y";
            choiceActionItem3.Id = "1Y";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "2Y";
            choiceActionItem4.Id = "2Y";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "All";
            choiceActionItem5.Id = "All";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.RegistrationSignOffDateFilterAction.Items.Add(choiceActionItem1);
            this.RegistrationSignOffDateFilterAction.Items.Add(choiceActionItem2);
            this.RegistrationSignOffDateFilterAction.Items.Add(choiceActionItem3);
            this.RegistrationSignOffDateFilterAction.Items.Add(choiceActionItem4);
            this.RegistrationSignOffDateFilterAction.Items.Add(choiceActionItem5);
            this.RegistrationSignOffDateFilterAction.SelectedItemChanged += RegistrationSignOffDateFilterAction_SelectedItemChanged;
            // 
            // RegistrationSignOffRollBack
            // 
            this.RegistrationSignOffRollback.Caption = "RollBack";
            this.RegistrationSignOffRollback.Category = "PopupActions";
            this.RegistrationSignOffRollback.ConfirmationMessage = null;
            this.RegistrationSignOffRollback.ImageName = "Backward_16x16";
            this.RegistrationSignOffRollback.Id = "RegistrationSignOffRollback";
            this.RegistrationSignOffRollback.ToolTip = null;
            this.RegistrationSignOffRollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Rollback_Execute);
            // 
            // RegistrationSignOffRollBack
            // 
            this.SubmittedJobIDRollback.Caption = "RollBack";
            this.SubmittedJobIDRollback.Category = "View";
            this.SubmittedJobIDRollback.ConfirmationMessage = null;
            this.SubmittedJobIDRollback.ImageName = "Backward_16x16";
            this.SubmittedJobIDRollback.Id = "SubmittedJobIDRollback";
            this.SubmittedJobIDRollback.ToolTip = null;
            this.SubmittedJobIDRollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RollbackJobID_Execute);
            // 
            // RegistrationSignOffController
            // 
            this.Actions.Add(this.SignOffAction);
            this.Actions.Add(this.SignedOffHistoryAction);
            this.Actions.Add(this.SignedOffSamplesAction);
            this.Actions.Add(this.RegistrationSignOffDateFilterAction);
            this.Actions.Add(this.RegistrationSignOffRollback);
            this.Actions.Add(this.SubmittedJobIDRollback);

        }

       



        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SignOffAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SignedOffHistoryAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SignedOffSamplesAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction RegistrationSignOffDateFilterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction RegistrationSignOffRollback;
        private DevExpress.ExpressApp.Actions.SimpleAction SubmittedJobIDRollback;
    }
}
