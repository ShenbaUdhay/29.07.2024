﻿
using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.PLM
{
    partial class PLMViewController
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
            this.PLMAddLayer = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FullScreen = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PLMRemoveLayer = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PLMAddType = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PLMRemoveType = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PLMSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PLMComplete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CopyPreviousPLM = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CopyToAllPLM = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CopyPLM = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CarryOverResults = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ClearPLM = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PLMDelete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PLMAddLayer
            // 
            this.PLMAddLayer.Caption = "Add Layer";
            this.PLMAddLayer.Category = "ObjectsCreation";
            this.PLMAddLayer.ConfirmationMessage = null;
            this.PLMAddLayer.Id = "PLMAddLayer";
            this.PLMAddLayer.ImageName = "Add.png";
            this.PLMAddLayer.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.PLMAddLayer.ToolTip = null;
            this.PLMAddLayer.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PLMAddLayer_Execute);
            // 
            // FullScreen
            // 
            this.FullScreen.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.FullScreen.Caption = "FullScreen";
            this.FullScreen.ConfirmationMessage = null;
            this.FullScreen.Id = "PLMFullScreen";
            this.FullScreen.ToolTip = null;
            // 
            // PLMRemoveLayer
            // 
            this.PLMRemoveLayer.Caption = "Remove Layer";
            this.PLMRemoveLayer.Category = "Edit";
            this.PLMRemoveLayer.ConfirmationMessage = null;
            this.PLMRemoveLayer.Id = "PLMRemoveLayer";
            this.PLMRemoveLayer.ImageName = "Remove.png";
            this.PLMRemoveLayer.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.PLMRemoveLayer.ToolTip = null;
            this.PLMRemoveLayer.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PLMRemoveLayer_Execute);
            // 
            // PLMAddType
            // 
            this.PLMAddType.Caption = "Add";
            this.PLMAddType.Category = "ObjectsCreation";
            this.PLMAddType.ConfirmationMessage = null;
            this.PLMAddType.Id = "PLMAddType";
            this.PLMAddType.ImageName = "Add.png";
            this.PLMAddType.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.PLMAddType.ToolTip = null;
            this.PLMAddType.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PLMAddType_Execute);
            // 
            // PLMRemoveType
            // 
            this.PLMRemoveType.Caption = "Remove";
            this.PLMRemoveType.Category = "Edit";
            this.PLMRemoveType.ConfirmationMessage = null;
            this.PLMRemoveType.Id = "PLMRemoveType";
            this.PLMRemoveType.ImageName = "Remove.png";
            this.PLMRemoveType.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.PLMRemoveType.ToolTip = null;
            this.PLMRemoveType.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PLMRemoveType_Execute);
            // 
            // PLMSave
            // 
            this.PLMSave.Caption = "Save";
            this.PLMSave.Category = "View";
            this.PLMSave.ConfirmationMessage = null;
            this.PLMSave.Id = "PLMSave";
            this.PLMSave.ImageName = "Action_Save";
            this.PLMSave.ToolTip = null;
            this.PLMSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveAction_Execute);
            // 
            // PLMComplete
            // 
            this.PLMComplete.Caption = "Complete";
            this.PLMComplete.Category = "View";
            this.PLMComplete.ConfirmationMessage = null;
            this.PLMComplete.Id = "PLMComplete";
            this.PLMComplete.ImageName = "Action_Save";
            this.PLMComplete.ToolTip = null;
            this.PLMComplete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CompleteAction_Execute);
            // 
            // CopyPreviousPLM
            // 
            this.CopyPreviousPLM.Caption = "Copy Previous";
            this.CopyPreviousPLM.Category = "CopyPreviousPLM";
            this.CopyPreviousPLM.ConfirmationMessage = null;
            this.CopyPreviousPLM.Id = "CopyPreviousPLM";
            this.CopyPreviousPLM.ToolTip = null;
            this.CopyPreviousPLM.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CopyPreviuous_Execute);
            // 
            // CopyToAllPLM
            // 
            this.CopyToAllPLM.Caption = "Copy To All";
            this.CopyToAllPLM.Category = "CopyToAllPLM";
            this.CopyToAllPLM.ConfirmationMessage = null;
            this.CopyToAllPLM.Id = "CopyToAllPLM";
            this.CopyToAllPLM.ToolTip = null;
            this.CopyToAllPLM.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CopyToAll_Execute);
            // 
            // CopyPLM
            // 
            this.CopyPLM.Caption = "Copy";
            this.CopyPLM.Category = "CopyPLM";
            this.CopyPLM.ConfirmationMessage = null;
            this.CopyPLM.Id = "CopyPLM";
            this.CopyPLM.ToolTip = null;
            this.CopyPLM.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Copy_Execute);
            // 
            // CarryOverResults
            // 
            this.CarryOverResults.Caption = "Carry Over Results";
            this.CarryOverResults.Category = "View";
            this.CarryOverResults.ConfirmationMessage = null;
            this.CarryOverResults.Id = "CarryOverResults";
            this.CarryOverResults.ToolTip = null;
            this.CarryOverResults.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CarryOverResults_Execute);
            // 
            // ClearPLM
            // 
            this.ClearPLM.Caption = "Clear";
            this.ClearPLM.Category = "ClearPLM";
            this.ClearPLM.ConfirmationMessage = null;
            this.ClearPLM.Id = "ClearPLM";
            this.ClearPLM.ToolTip = null;
            this.ClearPLM.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ClearPLM_Execute);
            // 
            // PLMDelete
            // 
            this.PLMDelete.Caption = "Delete";
            this.PLMDelete.Category = "Edit";
            this.PLMDelete.ConfirmationMessage = null;
            this.PLMDelete.Id = "PLMDelete";
            this.PLMDelete.ImageName = "";
            this.PLMDelete.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.PLMDelete.ToolTip = null;
            this.PLMDelete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PLMDelete_Execute);
            // 
            // PLMViewController
            // 
            this.Actions.Add(this.PLMAddLayer);
            this.Actions.Add(this.FullScreen);
            this.Actions.Add(this.PLMRemoveLayer);
            this.Actions.Add(this.PLMAddType);
            this.Actions.Add(this.PLMRemoveType);
            this.Actions.Add(this.PLMSave);
            this.Actions.Add(this.PLMComplete);
            this.Actions.Add(this.CopyPreviousPLM);
            this.Actions.Add(this.CopyToAllPLM);
            this.Actions.Add(this.CopyPLM);
            this.Actions.Add(this.CarryOverResults);
            this.Actions.Add(this.ClearPLM);
            this.Actions.Add(this.PLMDelete);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FullScreen;
        private DevExpress.ExpressApp.Actions.SimpleAction PLMAddLayer;
        private DevExpress.ExpressApp.Actions.SimpleAction PLMRemoveLayer;
        private DevExpress.ExpressApp.Actions.SimpleAction PLMAddType;
        private DevExpress.ExpressApp.Actions.SimpleAction PLMRemoveType;
        private DevExpress.ExpressApp.Actions.SimpleAction PLMSave;
        private DevExpress.ExpressApp.Actions.SimpleAction PLMComplete;
        private DevExpress.ExpressApp.Actions.SimpleAction CopyPreviousPLM;
        private DevExpress.ExpressApp.Actions.SimpleAction CopyToAllPLM;
        private DevExpress.ExpressApp.Actions.SimpleAction CopyPLM;
        private DevExpress.ExpressApp.Actions.SimpleAction CarryOverResults;
        private SimpleAction ClearPLM;
        private SimpleAction PLMDelete;
    }
}
