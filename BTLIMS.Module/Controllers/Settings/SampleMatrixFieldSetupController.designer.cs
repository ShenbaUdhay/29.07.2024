using System;
using DevExpress.ExpressApp.Actions;

namespace Labmaster.Module.Controllers.Settings
{
    partial class SampleMatrixFieldsetupController
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
            this.AddFields = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RemoveFields = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.AddField = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.RemoveField = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AddFields
            // 
            this.AddFields.Caption = "Add";
            this.AddFields.Category = "catAddSampleFields";
            this.AddFields.ConfirmationMessage = null;
            this.AddFields.Id = "AddFields";
            this.AddFields.ToolTip = "Add";
            this.AddFields.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddFields_Execute);
            // 
            // AddFields
            // 
            //this.AddField.Caption = "Add";
            //this.AddField.Category = "ListView";
            //this.AddField.ConfirmationMessage = null;
            //this.AddField.Id = "AddField";
            //this.AddField.ToolTip = "Add";
            //this.AddField.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //this.AddField.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddField_Execute);
            // 
            // RemoveFields
            // 
            this.RemoveFields.Caption = "Remove";
            this.RemoveFields.Category = "catRemoveSampleFields";
            this.RemoveFields.ConfirmationMessage = null;
            this.RemoveFields.Id = "RemoveFields";
            this.RemoveFields.ToolTip = "Remove";
            this.RemoveFields.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RemoveFields_Execute);
            // 
            // RemoveField
            // 
            //this.RemoveField.Caption = "Remove";
            //this.RemoveField.Category = "ListView";
            //this.RemoveField.ConfirmationMessage = null;
            //this.RemoveField.Id = "RemoveField";
            //this.RemoveField.ToolTip = "Remove";
            //this.RemoveField.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //this.RemoveField.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RemoveField_Execute);
            // 
            // SampleMatrixSetupFieldsetupController
            // 
            this.Actions.Add(this.AddFields);
            this.Actions.Add(this.RemoveFields);
            //this.Actions.Add(this.AddField);
            //this.Actions.Add(this.RemoveField);
        }

      
        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AddFields;
        private DevExpress.ExpressApp.Actions.SimpleAction RemoveFields;
        //private DevExpress.ExpressApp.Actions.SimpleAction AddField;
        //private DevExpress.ExpressApp.Actions.SimpleAction RemoveField;

    }
}
