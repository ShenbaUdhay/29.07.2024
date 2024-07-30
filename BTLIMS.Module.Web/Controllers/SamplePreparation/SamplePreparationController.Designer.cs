using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.SamplePreparation
{
    partial class SamplePreparationController
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
            this.SamplePrepAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePrepRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePrepReset = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePrepPrevious = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePrepSort = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePrepLoad = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnCopyFromSamplePrepAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePrepHistoryAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SavePrepBatch = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplePrepBatchDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.SamplePrepAddSamples = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Comment = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SamplePrepAdd
            // 
            this.SamplePrepAdd.Caption = "Add";
            this.SamplePrepAdd.Category = "ListView";
            this.SamplePrepAdd.ConfirmationMessage = null;
            this.SamplePrepAdd.Id = "SamplePrepAdd";
            this.SamplePrepAdd.ImageName = "Add.png";
            this.SamplePrepAdd.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.SamplePrepAdd.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SamplePrepAdd.ToolTip = "Add";
            this.SamplePrepAdd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePrepAdd_Execute);
            // 
            // SamplePrepRemove
            // 
            this.SamplePrepRemove.Caption = "Remove";
            this.SamplePrepRemove.Category = "ListView";
            this.SamplePrepRemove.ConfirmationMessage = null;
            this.SamplePrepRemove.Id = "SamplePrepRemove";
            this.SamplePrepRemove.ImageName = "Remove.png";
            this.SamplePrepRemove.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.SamplePrepRemove.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SamplePrepRemove.ToolTip = "Remove";
            this.SamplePrepRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePrepRemove_Execute);
            // 
            // SamplePrepReset
            // 
            this.SamplePrepReset.Caption = "Reset";
            this.SamplePrepReset.Category = "sampleprepaction0";
            this.SamplePrepReset.ConfirmationMessage = null;
            this.SamplePrepReset.Id = "SamplePrepReset";
            this.SamplePrepReset.ToolTip = null;
            this.SamplePrepReset.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePrepReset_Execute);
            // 
            // SamplePrepPrevious
            // 
            this.SamplePrepPrevious.Caption = "Previous";
            this.SamplePrepPrevious.Category = "sampleprepaction2";
            this.SamplePrepPrevious.ConfirmationMessage = null;
            this.SamplePrepPrevious.Id = "SamplePrepPrevious";
            this.SamplePrepPrevious.ToolTip = null;
            this.SamplePrepPrevious.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePrepPrevious_Execute);
            // 
            // SamplePrepSort
            // 
            this.SamplePrepSort.Caption = "Sort";
            this.SamplePrepSort.Category = "sampleprepaction2";
            this.SamplePrepSort.ConfirmationMessage = null;
            this.SamplePrepSort.Id = "SamplePrepSort";
            this.SamplePrepSort.ToolTip = null;
            this.SamplePrepSort.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePrepSort_Execute);
            // 
            // SamplePrepLoad
            // 
            this.SamplePrepLoad.Caption = "Load";
            this.SamplePrepLoad.Category = "sampleprepaction0";
            this.SamplePrepLoad.ConfirmationMessage = null;
            this.SamplePrepLoad.Id = "SamplePrepLoad";
            this.SamplePrepLoad.ToolTip = null;
            this.SamplePrepLoad.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePrepLoad_Execute);
            // 
            // btnCopyFromSamplePrepAction
            // 
            this.btnCopyFromSamplePrepAction.Caption = "Copy From Previous Batch";
            this.btnCopyFromSamplePrepAction.ConfirmationMessage = null;
            this.btnCopyFromSamplePrepAction.Id = "btnCopyFromSamplePrepAction";
            this.btnCopyFromSamplePrepAction.ToolTip = null;
            this.btnCopyFromSamplePrepAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnCopyFromSamplePrepAction_Execute);
            // 
            // SamplePrepHistoryAction
            // 
            this.SamplePrepHistoryAction.Caption = "History";
            this.SamplePrepHistoryAction.Category = "View";
            this.SamplePrepHistoryAction.ConfirmationMessage = null;
            this.SamplePrepHistoryAction.Id = "SamplePrepHistoryAction";
            this.SamplePrepHistoryAction.ImageName = "icons8-filter-16";
            this.SamplePrepHistoryAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.SamplePrepHistoryAction.ToolTip = null;
            this.SamplePrepHistoryAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SamplePrepHistoryAction_Execute);
            // 
            // SavePrepBatch
            // 
            this.SavePrepBatch.Caption = "Save ";
            this.SavePrepBatch.ConfirmationMessage = null;
            this.SavePrepBatch.ImageName = "Action_Save";
            this.SavePrepBatch.Id = "SavePrepBatch";
            this.SavePrepBatch.ToolTip = null;
            this.SavePrepBatch.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SavePrepBatch_Execute);

            // 
            // SamplePrepBatchDateFilter
            // 
            this.SamplePrepBatchDateFilter.Caption = "Date Filter";
            this.SamplePrepBatchDateFilter.Category = "View";
            this.SamplePrepBatchDateFilter.ConfirmationMessage = null;
            this.SamplePrepBatchDateFilter.Id = "SamplePrepBatchDateFilter";
            this.SamplePrepBatchDateFilter.ImageName = "Action_Search";
            this.SamplePrepBatchDateFilter.ToolTip = null;
            this.SamplePrepBatchDateFilter.SelectedItemChanged+= DateFilterSelectedItemChanged;
            // 
            // AddSample
            // 
            this.SamplePrepAddSamples.Caption = "Add Sample";
            this.SamplePrepAddSamples.Category = "View";
            this.SamplePrepAddSamples.ConfirmationMessage = null;
            this.SamplePrepAddSamples.Id = "SamplePrepAddSamples";
            this.SamplePrepAddSamples.ImageName = "Add_16x16";
            this.SamplePrepAddSamples.ToolTip = null;
            this.SamplePrepAddSamples.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddSample_Execute);
            //
            ////// Comment
            // 
            this.Comment.Caption = "Comment";
            this.Comment.Category = "View";
            this.Comment.ImageName = "comment_16X16";
            this.Comment.ConfirmationMessage = null;
            this.Comment.Id = "CommentSP";
            this.Comment.ToolTip = null;
            this.Comment.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Comment_Execute);
            // 
            // SamplePreparationController
            // 
            this.Actions.Add(this.SamplePrepAdd);
            this.Actions.Add(this.SamplePrepRemove);
            this.Actions.Add(this.SamplePrepReset);
            this.Actions.Add(this.SamplePrepPrevious);
            this.Actions.Add(this.SamplePrepSort);
            this.Actions.Add(this.SamplePrepLoad);
            this.Actions.Add(this.btnCopyFromSamplePrepAction);
            this.Actions.Add(this.SamplePrepHistoryAction);
            this.Actions.Add(this.SavePrepBatch);
            this.Actions.Add(this.SamplePrepBatchDateFilter);
            this.Actions.Add(this.SamplePrepAddSamples);
            this.Actions.Add(this.Comment);

        }
        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SamplePrepAdd;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePrepRemove;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePrepReset;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePrepPrevious;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePrepSort;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePrepLoad;
        private DevExpress.ExpressApp.Actions.SimpleAction btnCopyFromSamplePrepAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePrepHistoryAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SavePrepBatch;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction SamplePrepBatchDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplePrepAddSamples;
        private DevExpress.ExpressApp.Actions.SimpleAction Comment;
    }
}
