using DevExpress.ExpressApp.Actions;
using System;

namespace BTLIMS.Module.Controllers.Reporting
{
    partial class QueryPanelViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem16 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem17 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.Loaddefault = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.resultEntryDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ResultViewHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SampleSelectionMode = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.RetriveResultEntry = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Comment = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Loaddefault
            // 
            this.Loaddefault.Caption = "Loaddefault";
            this.Loaddefault.Category = "View";
            this.Loaddefault.ConfirmationMessage = null;
            this.Loaddefault.Id = "Loaddefault";
            this.Loaddefault.ToolTip = null;
            this.Loaddefault.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Loaddefault_Execute);
            // 
            // resultEntryDateFilterAction
            // 
            this.resultEntryDateFilterAction.Caption = "Date Filter";
            this.resultEntryDateFilterAction.Category = "View";
            this.resultEntryDateFilterAction.ConfirmationMessage = null;
            this.resultEntryDateFilterAction.Id = "resultEntryDateFilterAction";
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
            choiceActionItem16.Caption = "5Y";
            choiceActionItem16.Id = "5Y";
            choiceActionItem16.ImageName = null;
            choiceActionItem16.Shortcut = null;
            choiceActionItem16.ToolTip = null;
            choiceActionItem17.Caption = "All";
            choiceActionItem17.Id = "All";
            choiceActionItem17.ImageName = null;
            choiceActionItem17.Shortcut = null;
            choiceActionItem17.ToolTip = null;
            this.resultEntryDateFilterAction.Items.Add(choiceActionItem1);
            this.resultEntryDateFilterAction.Items.Add(choiceActionItem2);
            this.resultEntryDateFilterAction.Items.Add(choiceActionItem3);
            this.resultEntryDateFilterAction.Items.Add(choiceActionItem4);
            this.resultEntryDateFilterAction.Items.Add(choiceActionItem5);
            this.resultEntryDateFilterAction.Items.Add(choiceActionItem16);
            this.resultEntryDateFilterAction.Items.Add(choiceActionItem17);
            this.resultEntryDateFilterAction.ToolTip = "Date Filter";
            // 
            // ResultViewHistory
            // 
            this.ResultViewHistory.Caption = "History";
            this.ResultViewHistory.Category = "RecordEdit";
            this.ResultViewHistory.ConfirmationMessage = null;
            this.ResultViewHistory.Id = "ResultViewHistory";
            this.ResultViewHistory.ImageName = "Action_Search";
            this.ResultViewHistory.ToolTip = null;
            this.ResultViewHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ResultViewHistory_Execute);
            // 
            // Retrive
            // 
            this.RetriveResultEntry.Caption = "Retrive";
            this.RetriveResultEntry.Category = "View";
            this.RetriveResultEntry.ConfirmationMessage = null;
            this.RetriveResultEntry.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.RetriveResultEntry.Id = "RetriveResultEntry";
            this.RetriveResultEntry.ImageName = "Action_Redo";
            this.RetriveResultEntry.ToolTip = null;
            this.RetriveResultEntry.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RetriveResultEntry_Execute);
            // 
            // SampleSelectionMode
            // 
            this.SampleSelectionMode.Caption = "Selection Mode";
            this.SampleSelectionMode.ConfirmationMessage = null;
            this.SampleSelectionMode.Id = "SampleSelectionMode";
            choiceActionItem6.Caption = "Sample";
            choiceActionItem6.Id = "Sample";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "QC Sample";
            choiceActionItem7.Id = "QCSample";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            this.SampleSelectionMode.Items.Add(choiceActionItem6);
            this.SampleSelectionMode.Items.Add(choiceActionItem7);
            this.SampleSelectionMode.ToolTip = null;
            // 
            // Comment
            // 
            this.Comment.Caption = "Comment";
            this.Comment.Category = "View";
            this.Comment.ImageName = "comment_16X16";
            this.Comment.ConfirmationMessage = null;
            this.Comment.Id = "CommentRE";
            this.Comment.ToolTip = null;
            this.Comment.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Comment_Execute);
            // 
            // QueryPanelViewController
            // 
            this.Actions.Add(this.Loaddefault);
            this.Actions.Add(this.resultEntryDateFilterAction);
            this.Actions.Add(this.ResultViewHistory);
            this.Actions.Add(this.SampleSelectionMode);
            this.Actions.Add(this.RetriveResultEntry);
            this.Actions.Add(this.Comment);

        }

        #endregion
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowQueryPanel;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction RE_JobIDPopUp;
        //private DevExpress.ExpressApp.Actions.SimpleAction OpenJobId;
        private DevExpress.ExpressApp.Actions.SimpleAction Loaddefault;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction resultEntryDateFilterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ResultViewHistory;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction SampleSelectionMode;
        private DevExpress.ExpressApp.Actions.SimpleAction RetriveResultEntry;
        private DevExpress.ExpressApp.Actions.SimpleAction Comment;
    }
}
