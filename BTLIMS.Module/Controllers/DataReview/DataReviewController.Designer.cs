namespace LDM.Module.Controllers.DataReview
{
    partial class DataReviewController
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

            DevExpress.ExpressApp.Actions.ChoiceActionItem CAitem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem CAitem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem CAitem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem CAitem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem CAitem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem CAitem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem CAitem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();

            this.DataReviewBatchDetailsAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DataReviewBatchReviewAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DataReviewBatchRollbackAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DataReviewBatchHistoryAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DataReviewBatchHistoryDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            //this.AnalyticalBatchDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // DataReviewBatchDetailsAction
            // 
            this.DataReviewBatchDetailsAction.Caption = "Preview";
            this.DataReviewBatchDetailsAction.Category = "RecordEdit";
            this.DataReviewBatchDetailsAction.ConfirmationMessage = null;
            this.DataReviewBatchDetailsAction.Id = "DataReviewBatchDetailsAction";
            this.DataReviewBatchDetailsAction.ImageName = "Action_Report_Object_Inplace_Preview";
            this.DataReviewBatchDetailsAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.DataReviewBatchDetailsAction.ToolTip = null;
            this.DataReviewBatchDetailsAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DataReviewBatchDetailsAction_Execute);
            // 
            // DataReviewBatchReviewAction
            // 
            this.DataReviewBatchReviewAction.Caption = "Review";
            this.DataReviewBatchReviewAction.Category = "Edit";
            this.DataReviewBatchReviewAction.ConfirmationMessage = null;
            this.DataReviewBatchReviewAction.Id = "DataReviewBatchReviewAction";
            this.DataReviewBatchReviewAction.ImageName = "Action_Save";
            this.DataReviewBatchReviewAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.DataReviewBatchReviewAction.ToolTip = null;
            this.DataReviewBatchReviewAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DataReviewBatchReviewAction_Execute);
            // 
            // DataReviewBatchRollbackAction
            // 
            this.DataReviewBatchRollbackAction.Caption = "Rollback";
            this.DataReviewBatchRollbackAction.Category = "Edit";
            this.DataReviewBatchRollbackAction.ConfirmationMessage = null;
            this.DataReviewBatchRollbackAction.Id = "DataReviewBatchRollbackAction";
            this.DataReviewBatchRollbackAction.ImageName = "icons8-undo-16";
            this.DataReviewBatchRollbackAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.DataReviewBatchRollbackAction.ToolTip = null;
            this.DataReviewBatchRollbackAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DataReviewBatchRollbackAction_Execute);
            // 
            // DataReviewBatchHistoryAction
            // 
            this.DataReviewBatchHistoryAction.Caption = "History";
            this.DataReviewBatchHistoryAction.Category = "View";
            this.DataReviewBatchHistoryAction.ConfirmationMessage = null;
            this.DataReviewBatchHistoryAction.Id = "DataReviewBatchHistoryAction";
            this.DataReviewBatchHistoryAction.ImageName = "Action_Search";
            this.DataReviewBatchHistoryAction.ToolTip = null;
            this.DataReviewBatchHistoryAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DataReviewBatchHistoryAction_Execute);
            // 
            // DataReviewBatchHistoryDateFilter
            // 
            this.DataReviewBatchHistoryDateFilter.Caption = "Date Filter";
            this.DataReviewBatchHistoryDateFilter.ConfirmationMessage = null;
            this.DataReviewBatchHistoryDateFilter.Id = "DataReviewBatchHistoryDateFilter";
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
            choiceActionItem7.Id = "ALL";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            this.DataReviewBatchHistoryDateFilter.Items.Add(choiceActionItem1);
            this.DataReviewBatchHistoryDateFilter.Items.Add(choiceActionItem2);
            this.DataReviewBatchHistoryDateFilter.Items.Add(choiceActionItem3);
            this.DataReviewBatchHistoryDateFilter.Items.Add(choiceActionItem4);
            this.DataReviewBatchHistoryDateFilter.Items.Add(choiceActionItem5);
            this.DataReviewBatchHistoryDateFilter.Items.Add(choiceActionItem6);
            this.DataReviewBatchHistoryDateFilter.Items.Add(choiceActionItem7);
            this.DataReviewBatchHistoryDateFilter.ToolTip = null;
            this.DataReviewBatchHistoryDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.DataReviewBatchHistoryDateFilter_Execute);
            //
            //AnalyticalBatchDateFilter
            //
            //this.AnalyticalBatchDateFilter.Caption = "Date Filter";
            //this.AnalyticalBatchDateFilter.ConfirmationMessage = null;
            //this.AnalyticalBatchDateFilter.Id = "AnalyticalBatchDateFilter";
            //CAitem1.Caption = "1M";
            //CAitem1.Id = "1M";
            //CAitem1.ImageName = null;
            //CAitem1.Shortcut = null;
            //CAitem1.ToolTip = null;
            //CAitem2.Caption = "3M";
            //CAitem2.Id = "3M";
            //CAitem2.ImageName = null;
            //CAitem2.Shortcut = null;
            //CAitem2.ToolTip = null;
            //CAitem3.Caption = "6M";
            //CAitem3.Id = "6M";
            //CAitem3.ImageName = null;
            //CAitem3.Shortcut = null;
            //CAitem3.ToolTip = null;
            //CAitem4.Caption = "1Y";
            //CAitem4.Id = "1Y";
            //CAitem4.ImageName = null;
            //CAitem4.Shortcut = null;
            //CAitem4.ToolTip = null;
            //CAitem5.Caption = "2Y";
            //CAitem5.Id = "2Y";
            //CAitem5.ImageName = null;
            //CAitem5.Shortcut = null;
            //CAitem5.ToolTip = null;
            //CAitem6.Caption = "All";
            //CAitem6.Id = "ALL";
            //CAitem6.ImageName = null;
            //CAitem6.Shortcut = null;
            //CAitem6.ToolTip = null;
            //this.AnalyticalBatchDateFilter.Items.Add(CAitem1);
            //this.AnalyticalBatchDateFilter.Items.Add(CAitem2);
            //this.AnalyticalBatchDateFilter.Items.Add(CAitem3);
            //this.AnalyticalBatchDateFilter.Items.Add(CAitem4);
            //this.AnalyticalBatchDateFilter.Items.Add(CAitem5);
            //this.AnalyticalBatchDateFilter.Items.Add(CAitem6);
            //this.AnalyticalBatchDateFilter.ToolTip = null;
            //this.AnalyticalBatchDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.AnalyticalBatchDateFilter_Execute); 
            // 
            // DataReviewController
            // 
            this.Actions.Add(this.DataReviewBatchDetailsAction);
            this.Actions.Add(this.DataReviewBatchReviewAction);
            this.Actions.Add(this.DataReviewBatchRollbackAction);
            this.Actions.Add(this.DataReviewBatchHistoryAction);
            this.Actions.Add(this.DataReviewBatchHistoryDateFilter);
            //this.Actions.Add(this.AnalyticalBatchDateFilter);

        }

        
        private DevExpress.ExpressApp.Actions.SimpleAction DataReviewBatchDetailsAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DataReviewBatchReviewAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DataReviewBatchRollbackAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DataReviewBatchHistoryAction;
        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction DataReviewBatchHistoryDateFilter;
        //private DevExpress.ExpressApp.Actions.SingleChoiceAction AnalyticalBatchDateFilter;
    }
}
