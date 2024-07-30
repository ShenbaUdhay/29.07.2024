using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Utils;

namespace LDM.Module.Web.Controllers
{
    partial class HelpCenterController
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
            this.FullText = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            this.ArticalRelease = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.HelpCenterArtical = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.downloadAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ArticalCategory = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // FullText
            // 
            this.FullText.Caption = "Search";
            this.FullText.Category = "View";
            this.FullText.ConfirmationMessage = null;
            this.FullText.Id = "Search";
            this.FullText.NullValuePrompt = "Search for articles…";
            this.FullText.ShortCaption = null;
            this.FullText.ToolTip = null;
            this.FullText.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.FullText.Execute += new DevExpress.ExpressApp.Actions.ParametrizedActionExecuteEventHandler(this.FullText_Execute_1);
            // 
            // ArticalRelease
            // 
            this.ArticalRelease.Caption = "Release";
            this.ArticalRelease.Category = "View";
            this.ArticalRelease.ConfirmationMessage = null;
            this.ArticalRelease.Id = "ArticalRelease";
            this.ArticalRelease.ImageName = "State_Validation_Valid_48x48";
            this.ArticalRelease.ToolTip = null;
            this.ArticalRelease.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReleaseArtical_Excecute);
            // 
            // HelpCenterArtical
            // 
            this.HelpCenterArtical.Caption = "Artical";
            this.HelpCenterArtical.Category = "RecordEdit";
            this.HelpCenterArtical.ConfirmationMessage = null;
            this.HelpCenterArtical.Id = "HelpCenterArtical";
            this.HelpCenterArtical.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.HelpCenterArtical.ToolTip = null;
            this.HelpCenterArtical.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Artical_Excecute);
            // 
            // downloadAction
            // 
            this.downloadAction.Caption = "Download";
            this.downloadAction.Category = "RecordEdit";
            this.downloadAction.ConfirmationMessage = null;
            this.downloadAction.Id = "downloadAction";
            this.downloadAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.downloadAction.TargetObjectsCriteria = " [HelpCenterAttachments][].Count() > 0";
            this.downloadAction.ToolTip = null;
            this.downloadAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.downloadAction_Execute);
            //ArticalCategory
            this.ArticalCategory.Caption = "";
            this.ArticalCategory.Category = "View";
            this.ArticalCategory.ConfirmationMessage = null;
            this.ArticalCategory.Id = "ArticalCategory";
            choiceActionItem1.Caption = "FAQ";
            choiceActionItem1.Id = "FAQ";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Manual";
            choiceActionItem2.Id = "Manual";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            this.ArticalCategory.Items.Add(choiceActionItem1);
            this.ArticalCategory.Items.Add(choiceActionItem2);
            this.ArticalCategory.ToolTip = "Choose category";
            this.ArticalCategory.TargetViewType = ViewType.Any;
            //this.ArticalCategory.SelectedItem = ArticalCategory.Items[0];
            //this.ArticalCategory.Execute += ArticalCategory_Execute;
            this.ArticalCategory.SelectedItemChanged += ArticalCategory_SelectedItemChanged;
            // 
            // HelpCenterController
            // 
            this.Actions.Add(this.FullText);
            this.Actions.Add(this.ArticalRelease);
            this.Actions.Add(this.HelpCenterArtical);
            this.Actions.Add(this.downloadAction);
            this.Actions.Add(this.ArticalCategory);

        }

        #endregion
        DevExpress.ExpressApp.Actions.ParametrizedAction FullText;
        DevExpress.ExpressApp.Actions.SimpleAction ArticalRelease;
        DevExpress.ExpressApp.Actions.SimpleAction HelpCenterArtical;
        DevExpress.ExpressApp.Actions.SimpleAction downloadAction;
        DevExpress.ExpressApp.Actions.SingleChoiceAction ArticalCategory;
    }
}
