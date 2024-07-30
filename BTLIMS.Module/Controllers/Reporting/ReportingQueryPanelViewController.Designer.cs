namespace BTLIMS.Module.Controllers.Reporting
{
    partial class ReportingQueryPanelViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem8 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem9 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.reportingDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.reportingJobIDFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // reportingDateFilterAction
            // 
            this.reportingDateFilterAction.Caption = "Date Filter";
            this.reportingDateFilterAction.Category = "View";
            this.reportingDateFilterAction.ConfirmationMessage = null;
            this.reportingDateFilterAction.Id = "reportingDateFilterAction";
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
            choiceActionItem8.Caption = "5Y";
            choiceActionItem8.Id = "5Y";
            choiceActionItem8.ImageName = null;
            choiceActionItem8.Shortcut = null;
            choiceActionItem8.ToolTip = null;
            choiceActionItem9.Caption = "All";
            choiceActionItem9.Id = "All";
            choiceActionItem9.ImageName = null;
            choiceActionItem9.Shortcut = null;
            choiceActionItem9.ToolTip = null;
            this.reportingDateFilterAction.Items.Add(choiceActionItem1);
            this.reportingDateFilterAction.Items.Add(choiceActionItem2);
            this.reportingDateFilterAction.Items.Add(choiceActionItem3);
            this.reportingDateFilterAction.Items.Add(choiceActionItem4);
            this.reportingDateFilterAction.Items.Add(choiceActionItem5);
            this.reportingDateFilterAction.Items.Add(choiceActionItem8);
            this.reportingDateFilterAction.Items.Add(choiceActionItem9);
            this.reportingDateFilterAction.ToolTip = "Date Filter";
            // 
            // reportingJobIDFilterAction
            // 
            this.reportingJobIDFilterAction.Caption = "JobID Filter";
            this.reportingJobIDFilterAction.Category = "View";
            this.reportingJobIDFilterAction.ConfirmationMessage = null;
            this.reportingJobIDFilterAction.Id = "reportingJobIDFilterAction";
            choiceActionItem6.Caption = "All JobID";
            choiceActionItem6.Id = "AllJobID";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "Pending JobID";
            choiceActionItem7.Id = "PendingJobID";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            this.reportingJobIDFilterAction.Items.Add(choiceActionItem6);
            this.reportingJobIDFilterAction.Items.Add(choiceActionItem7);
            this.reportingJobIDFilterAction.ToolTip = "JobID Filter";
            // 
            // ReportingQueryPanelViewController
            // 
            this.Actions.Add(this.reportingDateFilterAction);
            this.Actions.Add(this.reportingJobIDFilterAction);
            this.ViewControlsCreated += new System.EventHandler(this.ReportingQueryPanelViewController_ViewControlsCreated);

        }

        #endregion

        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction ReportingQueryPanelPopupWindow;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction RepJobIDPopUp;
        //private DevExpress.ExpressApp.Actions.SimpleAction OpenReportJobId;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction reportingDateFilterAction;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction reportingJobIDFilterAction;
    }
}
