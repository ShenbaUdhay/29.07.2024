using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Web.Controllers.PLM
{
    partial class PTStudyLogViewController
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
            //this.Add = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PTStudyDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.PTStudyLogRelease = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportResultsFileAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);

            //this.PTStudySave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Add
            // 
            //this.Add.Caption = "Link";
            //this.Add.Category = "Edit";
            //this.Add.ConfirmationMessage = null;
            //this.Add.Id = "Add";
            //this.Add.ImageName = "Add.png";
            //this.Add.TargetViewId = "PTStudyLog_ListView";
            //this.Add.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            //this.Add.ToolTip = null;
            //this.Add.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            //this.Add.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Add_Execute);

            //Save


            //this.PTStudySave.Caption = "Save";
            //this.PTStudySave.ConfirmationMessage = null;
            //this.PTStudySave.Id = "PTStudySave";
            //this.PTStudySave.ToolTip = null;
            //this.PTStudySave.ImageName = "Save_16x16.png";
            //this.PTStudySave.TargetViewId = "PTStudyLog_ListView";
            //this.PTStudySave.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            //this.PTStudySave.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            //this.PTStudySave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Save_Execute);
            //            

            // Release
            // 
            this.PTStudyLogRelease.Caption = "Submit";
            this.PTStudyLogRelease.ImageName = "Submit_image";
            this.PTStudyLogRelease.Category = "Edit";
            this.PTStudyLogRelease.ConfirmationMessage = null;
            this.PTStudyLogRelease.Id = "PTStudyLogSubmit";
            this.PTStudyLogRelease.TargetViewId = "PTStudyLog_ListView;"+ "PTStudyLog_DetailView;";
           // this.PTStudyLogRelease.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PTStudyLogRelease.ToolTip = null;
            //this.PTStudyLogRelease.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            this.PTStudyLogRelease.TargetObjectsCriteria = "[Status] = 'PendingSubmission'";
            //this.PTStudyLogRelease.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PTStudyLogRelease.Execute += PTStudyLogSubmit_Execute;
            //PTStudyLogViewController 

            // DateFilter
            // 
            this.PTStudyDateFilter.Caption = "Date Filter";
           // this.PTStudyDateFilter.Category = "Edit";
            this.PTStudyDateFilter.ConfirmationMessage = null;
            this.PTStudyDateFilter.Id = "PTStudyDateFilter";
            this.PTStudyDateFilter.TargetViewId = "PTStudyLog_ListView";
            this.PTStudyDateFilter.ImageName = "Action_Search";
            this.PTStudyDateFilter.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PTStudyDateFilter.ToolTip = null;
            this.PTStudyDateFilter.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PTStudyDateFilter.SelectedItemChanged += PTStudyDateFilter_SelectedItemChanged;           
            
            // 
            // ImportMessagesFromFileAction
            // 
            this.ImportResultsFileAction.Caption = "Import File";
            this.ImportResultsFileAction.Category = "RecordEdit";
            this.ImportResultsFileAction.ConfirmationMessage = null;
            this.ImportResultsFileAction.Id = "ImportResultsFileAction";
            this.ImportResultsFileAction.ImageName = "import_data_16x16";
            this.ImportResultsFileAction.ToolTip = null;
           // this.ImportResultsFileAction.TargetObjectsCriteria = "[StudyID.studyID] <> 'Null' And [StudyName.studyName] <> 'Null'";
            this.ImportResultsFileAction.TargetViewId = "PTStudyLog_Results_ListView";
            this.ImportResultsFileAction.Execute += ImportResultsFileAction_Execute;
            // 
            // MessagesController
            // 

            // 
            //this.Actions.Add(this.Add);
            this.Actions.Add(this.ImportResultsFileAction);
            this.Actions.Add(this.PTStudyDateFilter);
            this.Actions.Add(this.PTStudyLogRelease);
            //this.Actions.Add(this.PTStudySave);

        }
       



        #endregion

        // private DevExpress.ExpressApp.Actions.SimpleAction Add;
        private SingleChoiceAction PTStudyDateFilter;
        private SimpleAction PTStudyLogRelease;
        private SimpleAction ImportResultsFileAction;
        //private DevExpress.ExpressApp.Actions.SimpleAction PTStudySave;
    }
}
