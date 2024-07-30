
namespace LDM.Module.Web.Controllers.ResultEntry
{
    partial class DataReviewQueryPanelController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.dataReviewDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.SDMS = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.dataReviewNextAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // dataReviewDateFilterAction
            // 
            this.dataReviewDateFilterAction.Caption = "Date Filter";
            this.dataReviewDateFilterAction.Category = "RecordEdit";
            this.dataReviewDateFilterAction.ConfirmationMessage = null;
            this.dataReviewDateFilterAction.Id = "dataReviewDateFilterAction";
            this.dataReviewDateFilterAction.ImageName = "icons8-filter-16";
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
            choiceActionItem5.Caption = "2Y";
            choiceActionItem5.Id = "2Y";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "5Y";
            choiceActionItem6.Id = "5Y";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "All";
            choiceActionItem7.Id = "All";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            this.dataReviewDateFilterAction.Items.Add(choiceActionItem1);
            this.dataReviewDateFilterAction.Items.Add(choiceActionItem2);
            this.dataReviewDateFilterAction.Items.Add(choiceActionItem3);
            this.dataReviewDateFilterAction.Items.Add(choiceActionItem4);
            this.dataReviewDateFilterAction.Items.Add(choiceActionItem5);
            this.dataReviewDateFilterAction.Items.Add(choiceActionItem6);
            this.dataReviewDateFilterAction.Items.Add(choiceActionItem7);
            this.dataReviewDateFilterAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            this.dataReviewDateFilterAction.ToolTip = "Date Filter";
            this.dataReviewDateFilterAction.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.dataReviewDateFilterAction_Execute);
            // 
            // SDMS
            // 
            this.SDMS.Caption = "SDMS";
            this.SDMS.ConfirmationMessage = null;
            this.SDMS.Id = "SDMS";
            this.SDMS.ToolTip = null;
            this.SDMS.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SDMS_Execute);
            // 
            // dataReviewNextAction
            // 
            this.dataReviewNextAction.Caption = "Retrieve";
            this.dataReviewNextAction.ConfirmationMessage = null;
            this.dataReviewNextAction.Id = "dataReviewNextAction";
            this.dataReviewNextAction.ToolTip = null;
            this.dataReviewNextAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.dataReviewNextAction_Execute);
            // 
            // DataReviewQueryPanelController
            // 
            this.Actions.Add(this.dataReviewDateFilterAction);
            this.Actions.Add(this.SDMS);
            this.Actions.Add(this.dataReviewNextAction);

        }

        #endregion

        //private DevExpress.ExpressApp.Actions.SimpleAction ResultValidation;
        //private DevExpress.ExpressApp.Actions.SimpleAction ResultApproval;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction dataReviewDateFilterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SDMS;
        private DevExpress.ExpressApp.Actions.SimpleAction dataReviewNextAction;
    }
}
