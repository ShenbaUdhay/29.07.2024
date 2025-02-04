﻿using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Settings.NCA
{
    partial class NCAViewController
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
            this.NonconformitySubmitAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.NonconformityCloseAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.History = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // NonconformitySubmitAction
            // 
            this.NonconformitySubmitAction.Caption = "Submit";
            this.NonconformitySubmitAction.ConfirmationMessage = null;
            this.NonconformitySubmitAction.Id = "NonconformitySubmitAction";
            this.NonconformitySubmitAction.ImageName = "Submit_image";
            this.NonconformitySubmitAction.ToolTip = null;
            this.NonconformitySubmitAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.NonconformitySubmitAction_Execute);
            // 
            // NonconformityCloseAction
            // 
            this.NonconformityCloseAction.Caption = "Close";
            this.NonconformityCloseAction.ConfirmationMessage = null;
            this.NonconformityCloseAction.Id = "NonconformityCloseAction";
            this.NonconformityCloseAction.ImageName = "Close16x16";
            this.NonconformityCloseAction.ToolTip = null;
            this.NonconformityCloseAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.NonconformityCloseAction_Execute);
            // 
            // History
            // 
            this.History.Caption = "History";
            this.History.ConfirmationMessage = null;
            this.History.Id = "ComplaintHistory";
            this.History.ImageName = "Action_Search";
            this.History.ToolTip = null;
            this.History.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.History_Execute);
            // 
            // NCAViewController
            // 
            this.Actions.Add(this.NonconformitySubmitAction);
            this.Actions.Add(this.NonconformityCloseAction);
            this.Actions.Add(this.History);

        }

       
        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction NonconformitySubmitAction;
        private DevExpress.ExpressApp.Actions.SimpleAction NonconformityCloseAction;
        private SimpleAction History;
    }
}
