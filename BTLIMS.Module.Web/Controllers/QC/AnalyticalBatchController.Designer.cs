
using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Web.Controllers.QC
{
    partial class AnalyticalBatchController
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
            this.ABreset = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ABprevious = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ABsort = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ABload = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenSDMS = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ABHistorys = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ABHistoryDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.Comment = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ABreset
            // 
            this.ABreset.Caption = "Reset";
            this.ABreset.Category = "ABaction0";
            this.ABreset.ConfirmationMessage = null;
            this.ABreset.Id = "ABreset";
            this.ABreset.ToolTip = null;
            // 
            // ABprevious
            // 
            this.ABprevious.Caption = "Previous";
            this.ABprevious.Category = "ABaction2";
            this.ABprevious.ConfirmationMessage = null;
            this.ABprevious.Id = "ABprevious";
            this.ABprevious.ToolTip = null;
            // 
            // ABsort
            // 
            this.ABsort.Caption = "Sort";
            this.ABsort.Category = "ABaction2";
            this.ABsort.ConfirmationMessage = null;
            this.ABsort.Id = "ABsort";
            this.ABsort.ToolTip = null;
            // 
            // ABload
            // 
            this.ABload.Caption = "Load";
            this.ABload.Category = "ABaction0";
            this.ABload.ConfirmationMessage = null;
            this.ABload.Id = "ABload";
            this.ABload.ToolTip = null;
            // 
            // OpenSDMS
            // 
            this.OpenSDMS.Caption = "SDMS";
            this.OpenSDMS.ConfirmationMessage = null;
            this.OpenSDMS.Id = "OpenSDMS";
            this.OpenSDMS.ToolTip = null;
            this.OpenSDMS.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenSDMS_Execute);
            // 
            // ABHistory
            // 
            this.ABHistorys.Caption = "History";
            this.ABHistorys.ConfirmationMessage = null;
            this.ABHistorys.Id = "ABHistorys";
            this.ABHistorys.ImageName = "Action_Search";
            this.ABHistorys.ToolTip = null;
            this.ABHistorys.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ABHistory_Execute);

            // 
            // ABHistoryDateFilter
            // 
            this.ABHistoryDateFilter.Caption = "Date Filter";
            this.ABHistoryDateFilter.ConfirmationMessage = null;
            this.ABHistoryDateFilter.Id = "ABHistoryDateFilter";
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
            this.ABHistoryDateFilter.Items.Add(choiceActionItem1);
            this.ABHistoryDateFilter.Items.Add(choiceActionItem2);
            this.ABHistoryDateFilter.Items.Add(choiceActionItem3);
            this.ABHistoryDateFilter.Items.Add(choiceActionItem4);
            this.ABHistoryDateFilter.Items.Add(choiceActionItem5);
            this.ABHistoryDateFilter.Items.Add(choiceActionItem6);
            this.ABHistoryDateFilter.Items.Add(choiceActionItem7);
            this.ABHistoryDateFilter.ToolTip = null;
            this.ABHistoryDateFilter.Category = "View";
            this.ABHistoryDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ABHistoryDateFilter_Execute);
            // 
            //// Comment
            // 
            this.Comment.Caption = "Comment";
            this.Comment.Category = "View";
            this.Comment.ImageName = "comment_16X16";
            this.Comment.ConfirmationMessage = null;
            this.Comment.Id = "CommentQC1";
            this.Comment.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.Comment.ToolTip = null;
            this.Comment.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Comment_Execute);
            // 
            // AnalyticalBatchController
            // 
            this.Actions.Add(this.ABreset);
            this.Actions.Add(this.ABprevious);
            this.Actions.Add(this.ABsort);
            this.Actions.Add(this.ABload);
            this.Actions.Add(this.OpenSDMS);
            this.Actions.Add(this.ABHistorys);
            this.Actions.Add(this.ABHistoryDateFilter);
            this.Actions.Add(this.Comment);

        }

      

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ABreset;
        private DevExpress.ExpressApp.Actions.SimpleAction ABprevious;
        private DevExpress.ExpressApp.Actions.SimpleAction ABsort;
        private DevExpress.ExpressApp.Actions.SimpleAction ABload;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenSDMS;
        private DevExpress.ExpressApp.Actions.SimpleAction ABHistorys;
        private SingleChoiceAction ABHistoryDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction Comment;
    }
}
