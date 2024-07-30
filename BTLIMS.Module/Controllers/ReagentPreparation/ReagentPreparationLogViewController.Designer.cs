
using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.ReagentPreparation
{
    partial class ReagentPreparationLogViewController
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
            this.ResetReagentPrepLog = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PreviousReagentPrepLog = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.NextReagentPrepLog = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LevelOfOk = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OkReagentPrepLog = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PrepNotepad = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Reset
            // 
            this.ResetReagentPrepLog.Caption = "Reset";
            this.ResetReagentPrepLog.Category = "catReset";
            this.ResetReagentPrepLog.ConfirmationMessage = null;
            this.ResetReagentPrepLog.Id = "ResetReagentPrepLog";
            this.ResetReagentPrepLog.ToolTip = null;
            this.ResetReagentPrepLog.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Reset_Execute);
            //ReagentPreparationLogViewController
            this.Actions.Add(this.ResetReagentPrepLog);
            // 
            // Previous
            // 
            this.PreviousReagentPrepLog.Caption = "Previous";
            this.PreviousReagentPrepLog.Category = "catPrevious";
            this.PreviousReagentPrepLog.ConfirmationMessage = null;
            this.PreviousReagentPrepLog.Id = "PreviousReagentPrepLog";
            this.PreviousReagentPrepLog.ToolTip = null;
            this.PreviousReagentPrepLog.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Previous_Execute);

            // 
            // Next
            // 
            this.NextReagentPrepLog.Caption = "Next";
            this.NextReagentPrepLog.Category = "catNext";
            this.NextReagentPrepLog.ConfirmationMessage = null;
            this.NextReagentPrepLog.Id = "NextReagentPrepLog";
            this.NextReagentPrepLog.ToolTip = null;
            this.NextReagentPrepLog.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Next_Execute);

            // 
            // Ok
            // 
            this.LevelOfOk.Caption = "Ok";
            this.LevelOfOk.Category = "catOk";
            this.LevelOfOk.ConfirmationMessage = null;
            this.LevelOfOk.Id = "LevelOfOk";
            this.LevelOfOk.ToolTip = null;
            this.LevelOfOk.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Ok_Level_Execute);

            // 
            // Ok
            // 
            this.OkReagentPrepLog.Caption = "Ok";
            this.OkReagentPrepLog.Category = "catprepOk";
            this.OkReagentPrepLog.ConfirmationMessage = null;
            this.OkReagentPrepLog.Id = "OkReagentPrepLog";
            this.OkReagentPrepLog.ToolTip = null;
            this.OkReagentPrepLog.Executing += Ok_Executing;
            this.OkReagentPrepLog.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Ok_Execute);


            // 
            // PrepNotepad
            // 
            this.PrepNotepad.Caption = "Prep Notepad";
            this.PrepNotepad.Category = "View";
            this.PrepNotepad.ConfirmationMessage = null;
            this.PrepNotepad.Id = "PrepNotepad";
            this.PrepNotepad.ImageName = "Action_Inline_Edit;";
            this.PrepNotepad.ToolTip = null;
            this.PrepNotepad.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PrepNotePad_Excecute);



            //ReagentPreparationLogViewController
            this.Actions.Add(this.ResetReagentPrepLog);
            this.Actions.Add(this.PreviousReagentPrepLog);
            this.Actions.Add(this.NextReagentPrepLog);
            this.Actions.Add(this.LevelOfOk);
            this.Actions.Add(this.OkReagentPrepLog);
            this.Actions.Add(this.PrepNotepad);
        }

      

        private SimpleAction ResetReagentPrepLog;
        private SimpleAction PreviousReagentPrepLog;
        private SimpleAction NextReagentPrepLog;
        private SimpleAction LevelOfOk;
        private SimpleAction OkReagentPrepLog;
        private SimpleAction PrepNotepad;
        #endregion
    }
}
