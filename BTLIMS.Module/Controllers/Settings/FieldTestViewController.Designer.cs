
namespace Labmaster.Module.Controllers.Settings
{
    partial class FieldTestViewController
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
            this.FieldTestEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldTestSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FieldTestEdit
            // 
            this.FieldTestEdit.Caption = "Edit";
            this.FieldTestEdit.Category = "Edit";
            this.FieldTestEdit.ConfirmationMessage = null;
            this.FieldTestEdit.Id = "FieldTestEdit";
            this.FieldTestEdit.ToolTip = null;
            this.FieldTestEdit.ImageName = "Action_Edit";
            this.FieldTestEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldTestEdit_Execute);
            // 
            // FieldTestSave
            // 
            this.FieldTestSave.Caption = "Save";
            this.FieldTestSave.Category = "Save";
            this.FieldTestSave.ConfirmationMessage = null;
            this.FieldTestSave.Id = "FieldTestSave";
            this.FieldTestSave.ToolTip = null;
            this.FieldTestSave.ImageName = "Action_Save";
            this.FieldTestSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldTestSave_Execute);
            // 
            // FieldTestViewController
            // 
            this.Actions.Add(this.FieldTestEdit);
            this.Actions.Add(this.FieldTestSave);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FieldTestEdit;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldTestSave;
    }
}
