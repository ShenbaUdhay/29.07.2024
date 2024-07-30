using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Web.Controllers.QC
{
    partial class QCBatchController
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
            this.QCadd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QCremove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QCreset = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QCprevious = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QCsort = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.qcload = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.qcbatchDateFilterActions = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.OpenResultEntry = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReagentLink = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReagentUnLink = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InstrumentUnLink = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InstrumentLink = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Comment = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenSDMSQcbatch = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // QCadd
            // 
            this.QCadd.Caption = "Add";
            this.QCadd.Category = "ListView";
            this.QCadd.ConfirmationMessage = null;
            this.QCadd.Id = "QCadd";
            this.QCadd.ImageName = "";
            this.QCadd.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.QCadd.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.QCadd.ToolTip = "Add";
            this.QCadd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QCadd_Execute);
            // 
            // QCremove
            // 
            this.QCremove.Caption = "Remove";
            this.QCremove.Category = "ListView";
            this.QCremove.ConfirmationMessage = null;
            this.QCremove.Id = "QCremove";
            this.QCremove.ImageName = "";
            this.QCremove.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.QCremove.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.QCremove.ToolTip = "Remove";
            this.QCremove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QCremove_Execute);
            // 
            // QCreset
            // 
            this.QCreset.Caption = "Reset";
            this.QCreset.Category = "qcaction0";
            this.QCreset.ConfirmationMessage = null;
            this.QCreset.Id = "QCreset";
            this.QCreset.ToolTip = null;
            this.QCreset.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QCreset_Execute);
            // 
            // QCprevious
            // 
            this.QCprevious.Caption = "Previous";
            this.QCprevious.Category = "qcaction2";
            this.QCprevious.ConfirmationMessage = null;
            this.QCprevious.Id = "QCprevious";
            this.QCprevious.ToolTip = null;
            this.QCprevious.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QCprevious_Execute);
            // 
            // QCsort
            // 
            this.QCsort.Caption = "Sort";
            this.QCsort.Category = "qcaction2";
            this.QCsort.ConfirmationMessage = null;
            this.QCsort.Id = "QCsort";
            this.QCsort.ToolTip = null;
            this.QCsort.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QCsort_Execute);
            // 
            // qcload
            // 
            this.qcload.Caption = "Load";
            this.qcload.Category = "qcaction0";
            this.qcload.ConfirmationMessage = null;
            this.qcload.Id = "QCload";
            this.qcload.ToolTip = null;
            this.qcload.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.qcload_Execute);
            // 
            // qcbatchDateFilterAction
            // 
            this.qcbatchDateFilterActions.Caption = "Date Filter";
            this.qcbatchDateFilterActions.ConfirmationMessage = null;
            this.qcbatchDateFilterActions.Id = "qcbatchDateFilterActions";
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
            this.qcbatchDateFilterActions.Items.Add(choiceActionItem1);
            this.qcbatchDateFilterActions.Items.Add(choiceActionItem2);
            this.qcbatchDateFilterActions.Items.Add(choiceActionItem3);
            this.qcbatchDateFilterActions.Items.Add(choiceActionItem4);
            this.qcbatchDateFilterActions.Items.Add(choiceActionItem5);
            this.qcbatchDateFilterActions.Items.Add(choiceActionItem6);
            this.qcbatchDateFilterActions.Items.Add(choiceActionItem7);
            this.qcbatchDateFilterActions.ToolTip = "Date Filter";
            // 
            // OpenResultEntry
            // 
            this.OpenResultEntry.Caption = "Result Entry";
            this.OpenResultEntry.ConfirmationMessage = null;
            this.OpenResultEntry.Id = "OpenResultEntry";
            this.OpenResultEntry.ToolTip = null;
            this.OpenResultEntry.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenResultEntry_Execute);
            // 
            // ReagentLink
            // 
            this.ReagentLink.Caption = "Link";
            this.ReagentLink.Category = "View";
            this.ReagentLink.ConfirmationMessage = null;
            this.ReagentLink.Id = "ReagentLink";
            this.ReagentLink.ToolTip = null;
            this.ReagentLink.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReagentLink_Execute);
            // 
            // ReagentUnLink
            // 
            this.ReagentUnLink.Caption = "Unlink";
            this.ReagentUnLink.Category = "View";
            this.ReagentUnLink.ConfirmationMessage = null;
            this.ReagentUnLink.Id = "ReagentUnLink";
            this.ReagentUnLink.ToolTip = null;
            this.ReagentUnLink.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReagentUnLink_Execute);
            // 
            // InstrumentUnLink
            // 
            this.InstrumentUnLink.Caption = "Unlink";
            this.InstrumentUnLink.Category = "View";
            this.InstrumentUnLink.ConfirmationMessage = null;
            this.InstrumentUnLink.Id = "InstrumentUnLink";
            this.InstrumentUnLink.ToolTip = null;
            this.InstrumentUnLink.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.InstrumentUnLink_Execute);
            // 
            // InstrumentLink
            // 
            this.InstrumentLink.Caption = "link";
            this.InstrumentLink.Category = "View";
            this.InstrumentLink.ConfirmationMessage = null;
            this.InstrumentLink.Id = "InstrumentLink";
            this.InstrumentLink.ToolTip = null;
            this.InstrumentLink.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.InstrumentLink_Execute);
            //
            //// Comment
            // 
            this.Comment.Caption = "Comment";
            this.Comment.Category = "View";
            this.Comment.ImageName = "comment_16X16";
            this.Comment.ConfirmationMessage = null;
            this.Comment.Id = "CommentQC";
            this.Comment.ToolTip = null;
            this.Comment.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Comment_Execute);

            //
            //// OpenSDMS
            // 
            this.OpenSDMSQcbatch.Caption = "SDMS";
            this.OpenSDMSQcbatch.Category = "View";
            this.OpenSDMSQcbatch.ConfirmationMessage = null;
            this.OpenSDMSQcbatch.Id = "OpenSDMSQcbatch";
            this.OpenSDMSQcbatch.ImageName = "icons8-data-sheet_16x16";
            this.OpenSDMSQcbatch.ToolTip = null;
            this.OpenSDMSQcbatch.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SDMS_Open);
            // 
            // QCBatchController
            // 
            this.Actions.Add(this.QCadd);
            this.Actions.Add(this.QCremove);
            this.Actions.Add(this.QCreset);
            this.Actions.Add(this.QCprevious);
            this.Actions.Add(this.QCsort);
            this.Actions.Add(this.qcload);
            this.Actions.Add(this.qcbatchDateFilterActions);
            this.Actions.Add(this.OpenResultEntry);
            this.Actions.Add(this.ReagentLink);
            this.Actions.Add(this.ReagentUnLink);
            this.Actions.Add(this.InstrumentUnLink);
            this.Actions.Add(this.InstrumentLink);
            this.Actions.Add(this.Comment);
            this.Actions.Add(this.OpenSDMSQcbatch);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction QCadd;
        private DevExpress.ExpressApp.Actions.SimpleAction QCremove;
        private DevExpress.ExpressApp.Actions.SimpleAction QCreset;
        private DevExpress.ExpressApp.Actions.SimpleAction QCprevious;
        private DevExpress.ExpressApp.Actions.SimpleAction QCsort;
        private DevExpress.ExpressApp.Actions.SimpleAction qcload;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction qcbatchDateFilterActions;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenResultEntry;
        private DevExpress.ExpressApp.Actions.SimpleAction ReagentLink;
        private DevExpress.ExpressApp.Actions.SimpleAction ReagentUnLink;
        private DevExpress.ExpressApp.Actions.SimpleAction InstrumentUnLink;
        private DevExpress.ExpressApp.Actions.SimpleAction InstrumentLink;
        private DevExpress.ExpressApp.Actions.SimpleAction Comment;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenSDMSQcbatch;
    }
}
