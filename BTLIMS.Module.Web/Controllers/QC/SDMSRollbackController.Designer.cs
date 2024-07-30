namespace LDM.Module.Web.Controllers.QC
{
    partial class SDMSRollbackController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.SDMSRollbackDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // SDMSRollbackDateFilterAction
            // 
            this.SDMSRollbackDateFilterAction.Caption = "Date Filter";
            this.SDMSRollbackDateFilterAction.Category = "View";
            this.SDMSRollbackDateFilterAction.ConfirmationMessage = null;
            this.SDMSRollbackDateFilterAction.Id = "SDMSRollbackDateFilterAction";
            choiceActionItem1.Caption = "1M";
            choiceActionItem1.Id = "1M";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "3M";
            choiceActionItem2.Id = "3M";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "6M";
            choiceActionItem3.Id = "6M";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "1Y";
            choiceActionItem4.Id = "1Y";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "All";
            choiceActionItem5.Id = "All";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.SDMSRollbackDateFilterAction.Items.Add(choiceActionItem1);
            this.SDMSRollbackDateFilterAction.Items.Add(choiceActionItem2);
            this.SDMSRollbackDateFilterAction.Items.Add(choiceActionItem3);
            this.SDMSRollbackDateFilterAction.Items.Add(choiceActionItem4);
            this.SDMSRollbackDateFilterAction.Items.Add(choiceActionItem5);
            this.SDMSRollbackDateFilterAction.ToolTip = "Date Filter";
            // 
            // SDMSRollbackController
            // 
            this.Actions.Add(this.SDMSRollbackDateFilterAction);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SDMSRollbackDateFilterAction;
    }
}
