using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SDMS
{
    partial class ScientificDataTableController
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
            this.AddRawData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveRawData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddCalibration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveCalibration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DeleteRawData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DeleteCalibrationData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AddRawData
            // 
            this.AddRawData.Caption = "Add";
            this.AddRawData.Category = "Edit";
            this.AddRawData.ImageName = "Add_16x16";
            this.AddRawData.ConfirmationMessage = null;
            this.AddRawData.Id = "AddRawData";
            this.AddRawData.ToolTip = null;
            this.AddRawData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddRawData_Execute);
            // 
            // SaveRawData
            // 
            this.SaveRawData.Caption = "Save";
            this.SaveRawData.Category = "Edit";
            this.SaveRawData.ImageName = "Save_16x16";
            this.SaveRawData.ConfirmationMessage = null;
            this.SaveRawData.Id = "SaveRawData";
            this.SaveRawData.ToolTip = null;
            this.SaveRawData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveRawData_Execute);
            // 
            // AddCalibration
            // 
            this.AddCalibration.Caption = "Add";
            this.AddCalibration.ImageName = "Add_16x16";
            this.AddCalibration.Category = "Edit";
            this.AddCalibration.ConfirmationMessage = null;
            this.AddCalibration.Id = "AddCalibration";
            this.AddCalibration.ToolTip = null;
            this.AddCalibration.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddCalibration_Execute);
            // 
            // SaveCalibration
            // 
            this.SaveCalibration.Caption = "Save";
            this.SaveCalibration.ImageName = "Save_16x16";
            this.SaveCalibration.Category = "Edit";
            this.SaveCalibration.ConfirmationMessage = null;
            this.SaveCalibration.Id = "SaveCalibration";
            this.SaveCalibration.ToolTip = null;
            this.SaveCalibration.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveCalibration_Execute);
            // 
            // DeleteRawData
            // 
            this.DeleteRawData.Caption = "Delete ";
            this.DeleteRawData.ConfirmationMessage = null;
            this.DeleteRawData.ImageName = "Action_Delete";
            this.DeleteRawData.Id = "DeleteRawData";
            this.DeleteRawData.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.DeleteRawData.ToolTip = null;
            this.DeleteRawData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeleteRawData_Execute);
            // 
            // DeleteCalibrationData
            // 
            this.DeleteCalibrationData.Caption = "Delete ";
            this.DeleteCalibrationData.ConfirmationMessage = null;
            this.DeleteCalibrationData.ImageName = "Action_Delete";
            this.DeleteCalibrationData.Id = "DeleteCalibrationData";
            this.DeleteCalibrationData.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.DeleteCalibrationData.ToolTip = null;
            this.DeleteCalibrationData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeleteCalibrationData_Execute);
            // 
            // ScientificDataTableController
            // 
            this.Actions.Add(this.AddRawData);
            this.Actions.Add(this.SaveRawData);
            this.Actions.Add(this.AddCalibration);
            this.Actions.Add(this.SaveCalibration);
            this.Actions.Add(this.DeleteRawData);
            this.Actions.Add(this.DeleteCalibrationData);

        }

    

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AddRawData;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveRawData;
        private DevExpress.ExpressApp.Actions.SimpleAction AddCalibration;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveCalibration;
        private SimpleAction DeleteRawData;
        private SimpleAction DeleteCalibrationData;
    }
}
