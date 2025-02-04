﻿using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.DOC
{
    partial class DOCViewController
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
            this.Input = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.Save = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Submit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Validate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DOCRollBack = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Comment = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Calculate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DOCReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DOCCertificate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //
            // Input
            //
            this.Input.Caption = "...";
            this.Input.Category = "ListView";
            this.Input.ConfirmationMessage = null;
            this.Input.Id = "Input";
            this.Input.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Input.ToolTip = null;
            this.Input.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(Input_Execute);
            //
            // Save
            // 
            //this.Save.Caption = "Save";
            //this.Save.Category = "View";
            //this.Save.ConfirmationMessage = null;
            //this.Save.Id = "Save1";
            //this.Save.ToolTip = null;
            //this.Save.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Save_Execute);
            // 
            // Submit
            // 
            this.Submit.Caption = "Submit";
            this.Submit.Category = "View";
            this.Submit.ConfirmationMessage = null;
            this.Submit.Id = "Submit1";
            this.Submit.ImageName = "Submit_image";
            this.Submit.ToolTip = null;
            this.Submit.TargetObjectsCriteria = "[Status] = 'PendingSubmission'";
            this.Submit.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Submit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Submit_Execute);
            // 
            // Validate
            // 
            this.Validate.Caption = "Validate";
            this.Validate.Category = "View";
            this.Validate.ConfirmationMessage = null;
            this.Validate.Id = "Validate1";
            this.Validate.ToolTip = null;
            this.Validate.TargetObjectsCriteria = "[Status] = 'PendingValidation'";
            this.Validate.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Validate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Validate_Execute);
            // 
            // DOCRollBack
            // 
            this.DOCRollBack.Caption = "RollBack";
            this.DOCRollBack.Category = "View";
            this.DOCRollBack.ConfirmationMessage = null;
            this.DOCRollBack.Id = "DOCRollBack";
            this.DOCRollBack.ImageName = "RollBack16x";
            this.DOCRollBack.ToolTip = null;
            this.DOCRollBack.TargetObjectsCriteria = "[Status] = 'PendingValidation'";
            this.DOCRollBack.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.DOCRollBack.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DOCRollBack_Execute);
            // 
            // Comment
            // 
            this.DOCReport.Caption = "DOCReport";
            this.DOCReport.Category = "ListView";
            this.DOCReport.ConfirmationMessage = null;
            this.DOCReport.Id = "DOCReport";
            this.DOCReport.ToolTip = null;
            this.DOCReport.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.DOCReport.TargetObjectsCriteria = "[Status] <> 'PendingSubmission' And [Status] <> 'Fail'";
            this.DOCReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DOCReport_Execute);
            // 
            // Certificate
            // 
            this.DOCCertificate.Caption = "Certificate";
            this.DOCCertificate.Category = "ListView";
            this.DOCCertificate.ConfirmationMessage = null;
            this.DOCCertificate.Id = "DOCCertificate";
            this.DOCCertificate.ToolTip = null;
            this.DOCCertificate.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.DOCCertificate.TargetObjectsCriteria = "[Status] <> 'PendingSubmission'And [Status] <> 'Fail'";
            this.DOCCertificate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DOCCertificate_Execute);
            // 
            // Comment
            // 
            this.Comment.Caption = "Comment";
            this.Comment.Category = "View";
            this.Comment.ImageName = "comment_16X16";
            this.Comment.ConfirmationMessage = null;
            this.Comment.Id = "CommentDOC";
            this.Comment.ToolTip = null;
            this.Comment.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Comment_Execute);
            // 
            // Calculate
            //
            this.Calculate.Caption = "Calculate";
            this.Calculate.Category = "ObjectsCreation";
            //this.Calculate.ImageName = null;
            this.Calculate.ConfirmationMessage = null;
            this.Calculate.Id = "CalculateDOC";
            this.Calculate.ToolTip = null;
            //this.Calculate.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Calculate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Calculate_Execute);
            // 
            //
            //
            this.Actions.Add(this.Input);
            //this.Actions.Add(this.Save);
            this.Actions.Add(this.Submit);
            this.Actions.Add(this.Validate);
            this.Actions.Add(this.DOCRollBack);
            this.Actions.Add(this.Comment);
            this.Actions.Add(this.Calculate);
            this.Actions.Add(this.DOCReport);
            this.Actions.Add(this.DOCCertificate);
        }


        private DevExpress.ExpressApp.Actions.SimpleAction Input;
        //private DevExpress.ExpressApp.Actions.SimpleAction Save;
        private DevExpress.ExpressApp.Actions.SimpleAction Submit;
        private DevExpress.ExpressApp.Actions.SimpleAction Validate;
        private DevExpress.ExpressApp.Actions.SimpleAction DOCRollBack;
        private DevExpress.ExpressApp.Actions.SimpleAction Comment;
        private DevExpress.ExpressApp.Actions.SimpleAction Calculate;
        private DevExpress.ExpressApp.Actions.SimpleAction DOCReport;
        private DevExpress.ExpressApp.Actions.SimpleAction DOCCertificate;


        #endregion
    }
}
