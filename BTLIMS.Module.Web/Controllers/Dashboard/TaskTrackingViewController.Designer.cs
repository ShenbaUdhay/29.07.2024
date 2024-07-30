namespace LDM.Module.Controllers.Public
{
    partial class TaskTrackingViewController
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
            this.ProjectTrackingDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem0 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();

            // ProjectTrackingDateFilterAction
            // 
            this.ProjectTrackingDateFilterAction.Caption = "Date Filter";
            this.ProjectTrackingDateFilterAction.Category = "View";
            this.ProjectTrackingDateFilterAction.ConfirmationMessage = null;
            this.ProjectTrackingDateFilterAction.Id = "ProjectTrackingDateFilterAction";
            choiceActionItem0.Caption = "1M";
            choiceActionItem0.Id = "1M";
            choiceActionItem0.ImageName = null;
            choiceActionItem0.Shortcut = null;
            choiceActionItem0.ToolTip = null;
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
            choiceActionItem5.Caption = "5Y";
            choiceActionItem5.Id = "5Y";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "All";
            choiceActionItem6.Id = "All";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            this.ProjectTrackingDateFilterAction.Items.Add(choiceActionItem0);
            this.ProjectTrackingDateFilterAction.Items.Add(choiceActionItem1);
            this.ProjectTrackingDateFilterAction.Items.Add(choiceActionItem2);
            this.ProjectTrackingDateFilterAction.Items.Add(choiceActionItem3);
            this.ProjectTrackingDateFilterAction.Items.Add(choiceActionItem4);
            this.ProjectTrackingDateFilterAction.Items.Add(choiceActionItem5);
            this.ProjectTrackingDateFilterAction.Items.Add(choiceActionItem6);


            this.Actions.Add(this.ProjectTrackingDateFilterAction);
        }
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ProjectTrackingDateFilterAction;
        #endregion
    }
}
