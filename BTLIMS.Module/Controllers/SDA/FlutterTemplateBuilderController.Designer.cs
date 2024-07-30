using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SDA
{
    partial class FlutterTemplateBuilderController
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
            this.HeaderAddAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.HeaderRemoveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DetailAddAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DetailRemoveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CalibrationAddAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CalibrationRemoveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // HeaderAddAction
            // 
            this.HeaderAddAction.Caption = "Add";
            this.HeaderAddAction.Category = "catHeaderAdd";
            this.HeaderAddAction.ConfirmationMessage = null;
            this.HeaderAddAction.Id = "HeaderAddAction";
            this.HeaderAddAction.ToolTip = "Add";
            this.HeaderAddAction.ImageName = "Add";
            this.HeaderAddAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.HeaderAddAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.HeaderAddAction_Execute);
            // 
            // HeaderRemoveAction
            // 
            this.HeaderRemoveAction.Caption = "Remove";
            this.HeaderRemoveAction.Category = "catHeaderRemove";
            this.HeaderRemoveAction.Id = "HeaderRemoveAction";
            this.HeaderRemoveAction.ToolTip = "Remove";
            this.HeaderRemoveAction.ImageName = "Remove";
            this.HeaderRemoveAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.HeaderRemoveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.HeaderRemoveAction_Execute);
            // 
            // DetailAddAction
            // 
            this.DetailAddAction.Caption = "Add";
            this.DetailAddAction.Category = "catDetailAdd";
            this.DetailAddAction.ConfirmationMessage = null;
            this.DetailAddAction.Id = "DetailAddAction";
            this.DetailAddAction.ToolTip = "Add";
            this.DetailAddAction.ImageName = "Add";
            this.DetailAddAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.DetailAddAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DetailAddAction_Execute);
            // 
            // DetailRemoveAction
            // 
            this.DetailRemoveAction.Caption = "Remove";
            this.DetailRemoveAction.Category = "catDetailRemove";
            this.DetailRemoveAction.ConfirmationMessage = null;
            this.DetailRemoveAction.Id = "DetailRemoveAction";
            this.DetailRemoveAction.ToolTip = "Remove";
            this.DetailRemoveAction.ImageName = "Remove";
            this.DetailRemoveAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.DetailRemoveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DetailRemoveAction_Execute);
            // 
            // CalibrationAddAction
            // 
            this.CalibrationAddAction.Caption = "Add";
            this.CalibrationAddAction.Category = "catCalibrationAdd";
            this.CalibrationAddAction.ConfirmationMessage = null;
            this.CalibrationAddAction.Id = "CalibrationAddAction";
            this.CalibrationAddAction.ToolTip = "Add";
            this.CalibrationAddAction.ImageName = "Add";
            this.CalibrationAddAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.CalibrationAddAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CalibrationAddAction_Execute);
            // 
            // CalibrationRemoveAction
            // 
            this.CalibrationRemoveAction.Caption = "Remove";
            this.CalibrationRemoveAction.Category = "catCalibrationRemove";
            this.CalibrationRemoveAction.ConfirmationMessage = null;
            this.CalibrationRemoveAction.Id = "CalibrationRemoveAction";
            this.CalibrationRemoveAction.ToolTip = "Remove";
            this.CalibrationRemoveAction.ImageName = "Remove";
            this.CalibrationRemoveAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.CalibrationRemoveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CalibrationRemoveAction_Execute);
            // 
            // FlutterTemplateBuilderController
            // 
            this.Actions.Add(this.HeaderAddAction);
            this.Actions.Add(this.HeaderRemoveAction);
            this.Actions.Add(this.DetailAddAction);
            this.Actions.Add(this.DetailRemoveAction);
            this.Actions.Add(this.CalibrationAddAction);
            this.Actions.Add(this.CalibrationRemoveAction);
        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction HeaderAddAction;
        private DevExpress.ExpressApp.Actions.SimpleAction HeaderRemoveAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DetailAddAction;
        private DevExpress.ExpressApp.Actions.SimpleAction DetailRemoveAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CalibrationAddAction;
        private DevExpress.ExpressApp.Actions.SimpleAction CalibrationRemoveAction;
    }
}
