namespace LDM.Module.Web.Controllers.SamplingProposal
{
    partial class TaskRecurranceSetupController
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
            this.TaskScheduler = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Save = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            
            this.TaskAgenda = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);

            // 
            // TaskScheduler
            // 
            this.TaskScheduler.Caption = "Task Scheduler ";
            this.TaskScheduler.ConfirmationMessage = null;
            this.TaskScheduler.Id = "Task Scheduler ";
            this.TaskScheduler.ToolTip = null;
            this.TaskScheduler.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TaskScheduler_Execute);
            // 
            // Save
            // 
            this.Save.Caption = "Save";
            this.Save.Category = "TaskSave";
            this.Save.ConfirmationMessage = null;
            this.Save.Id = "TaskSave";
            this.Save.ToolTip = null;
            this.Save.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Save_Execute);
            // 
            // TaskRecurranceSetupController
            // 
            this.Actions.Add(this.TaskScheduler);
            this.Actions.Add(this.Save);
            this.Actions.Add(this.TaskAgenda);
            this.TargetViewId = "SamplingProposal_DetailView";

            //
            // Task Agenda
            // 
            // 
            this.TaskAgenda.Caption = "Task Agenda";
            this.TaskAgenda.ConfirmationMessage = null;
            this.TaskAgenda.ImageName = "Task_Agenda_16x16";
            this.TaskAgenda.Id = "TaskAgenda";
            this.TaskAgenda.ToolTip = "TaskAgenda";
            this.TaskAgenda.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TaskAgenda_Execute);
        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction TaskScheduler;
        private DevExpress.ExpressApp.Actions.SimpleAction Save;
        private DevExpress.ExpressApp.Actions.SimpleAction TaskAgenda;

    }
}
