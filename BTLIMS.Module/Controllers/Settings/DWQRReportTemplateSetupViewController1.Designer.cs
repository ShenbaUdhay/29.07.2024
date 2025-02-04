﻿using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Settings.DWQRReportTemplateSetup
{
    partial class DWQRReportTemplateSetupViewController1
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
            //Assign this compenent to the buttons that created
            //
            this.components = new System.ComponentModel.Container();
            this.btn_DWQRTestData = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_DWQRPreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_DWQRSubmit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_DWQRApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_DWQRHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_DWQRPreviewToApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_DWQRRollBack = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_DWQRDeliverReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_DWQRDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();

            //Add the buttons that created
            //
            this.Actions.Add(this.btn_DWQRTestData);
            this.Actions.Add(this.btn_DWQRPreview);
            this.Actions.Add(this.btn_DWQRSubmit);
            this.Actions.Add(this.btn_DWQRApprove);
            this.Actions.Add(this.btn_DWQRHistory);
            this.Actions.Add(this.btn_DWQRPreviewToApprove);
            this.Actions.Add(this.btn_DWQRRollBack);
            this.Actions.Add(this.btn_DWQRDateFilter);
            this.Actions.Add(this.btn_DWQRDeliverReport);

            //Initialize the button

            //
            this.btn_DWQRTestData.Caption = "Test";
            //this.btn_DWQRTestData.Category = "DWQRTest";
            this.btn_DWQRTestData.ConfirmationMessage = null;
            this.btn_DWQRTestData.ImageName = "Action_Validation_Validate";
            this.btn_DWQRTestData.Id = "btn_DWQRTestData";
            this.btn_DWQRTestData.ToolTip = "Test";
            this.btn_DWQRTestData.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_DWQRTestData_Execute);
            
            //
            this.btn_DWQRPreview.Caption = "Preview";
            //this.btn_DWQRPreview.Category = "DWQRTest";
            this.btn_DWQRPreview.ConfirmationMessage = null;
            this.btn_DWQRPreview.ImageName = "Action_Report_Object_Inplace_Preview";
            this.btn_DWQRPreview.Id = "btn_DWQRPreview";
            this.btn_DWQRPreview.ToolTip = "Preview the report";
            this.btn_DWQRPreview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_DWQRPreview_Execute);

            //
            this.btn_DWQRSubmit.Caption = "Submit";
            //this.btn_DWQRSubmit.Category = "DWQRTest";
            this.btn_DWQRSubmit.ConfirmationMessage = null;
            this.btn_DWQRSubmit.ImageName = "Submit_image";
            this.btn_DWQRSubmit.Id = "btn_DWQRSubmit";
            this.btn_DWQRSubmit.ToolTip = "Submit the report";
            this.btn_DWQRSubmit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_DWQRSubmit_Execute);

            //
            this.btn_DWQRApprove.Caption = "Approve";
            //this.btn_DWQRApprove.Category = "DWQRTest";
            this.btn_DWQRApprove.ConfirmationMessage = null;
            this.btn_DWQRApprove.ImageName = "State_Task_Completed";
            this.btn_DWQRApprove.Id = "btn_DWQRApprove";
            this.btn_DWQRApprove.ToolTip = "Approve the report";
            this.btn_DWQRApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_DWQRApprove_Execute);


            this.btn_DWQRPreviewToApprove.Caption = "Preview";
            //this.btn_DWQRApprove.Category = "DWQRTest";
            this.btn_DWQRPreviewToApprove.ConfirmationMessage = null;
            this.btn_DWQRPreviewToApprove.ImageName = "Action_Report_Object_Inplace_Preview";
            this.btn_DWQRPreviewToApprove.Id = "btn_DWQRPreviewToApprove";
            this.btn_DWQRPreviewToApprove.ToolTip = "Preview the report";
            this.btn_DWQRPreviewToApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_DWQRPreviewToApprove_Execute);

            //
            this.btn_DWQRHistory.Caption = "History";
            //this.btn_DWQRHistory.Category = "DWQRTest";
            this.btn_DWQRHistory.ConfirmationMessage = null;
            this.btn_DWQRHistory.ImageName = "Action_Search";
            this.btn_DWQRHistory.Id = "btn_DWQRHistory";
            this.btn_DWQRHistory.ToolTip = "History of reports delivered";
            this.btn_DWQRHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_DWQRHistory_Execute);

            //
            this.btn_DWQRRollBack.Caption = "Rollback";
            //this.btn_DWQRRollBack.Category = "DWQRTest";
            this.btn_DWQRRollBack.ConfirmationMessage = null;
            this.btn_DWQRRollBack.ImageName = "Action_Redo";
            this.btn_DWQRRollBack.Id = "btn_DWQRRollBack";
            this.btn_DWQRRollBack.ToolTip = "Rollback the report";
            this.btn_DWQRRollBack.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_DWQRRollBack_Execute);

            // 
            this.btn_DWQRDeliverReport.Caption = "Deliver Via Email";
            //this.btn_DWQRDeliverReport.Category = "Edit";
            this.btn_DWQRDeliverReport.ConfirmationMessage = null;
            this.btn_DWQRDeliverReport.Id = "btn_DWQRDeliverReport";
            this.btn_DWQRDeliverReport.ToolTip = null;
            this.btn_DWQRDeliverReport.ImageName = "Send16x";
            this.btn_DWQRDeliverReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btn_DWQRDeliverReport_Execute);

            // 
            this.btn_DWQRDateFilter.Caption = "Date Filter";
            //this.btn_DWQRDateFilter.Category = "View";
            this.btn_DWQRDateFilter.ConfirmationMessage = null;
            this.btn_DWQRDateFilter.Id = "btn_DWQRDateFilter";
            choiceActionItem1.Caption = "1M";
            choiceActionItem1.Id = "1M";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "3M";
            choiceActionItem2.Id = "3M";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "6M";
            choiceActionItem3.Id = "6M";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "1Y";
            choiceActionItem4.Id = "1Y";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "2Y";
            choiceActionItem5.Id = "2Y";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "5Y";
            choiceActionItem6.Id = "5Y";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "All";
            choiceActionItem7.Id = "All";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            this.btn_DWQRDateFilter.Items.Add(choiceActionItem1);
            this.btn_DWQRDateFilter.Items.Add(choiceActionItem2);
            this.btn_DWQRDateFilter.Items.Add(choiceActionItem3);
            this.btn_DWQRDateFilter.Items.Add(choiceActionItem4);
            this.btn_DWQRDateFilter.Items.Add(choiceActionItem5);
            this.btn_DWQRDateFilter.Items.Add(choiceActionItem6);
            this.btn_DWQRDateFilter.Items.Add(choiceActionItem7);
        }

        #endregion

        //Create button here
        //
        private DevExpress.ExpressApp.Actions.SimpleAction btn_DWQRTestData;
        private DevExpress.ExpressApp.Actions.SimpleAction btn_DWQRPreview;
        private DevExpress.ExpressApp.Actions.SimpleAction btn_DWQRSubmit;
        private DevExpress.ExpressApp.Actions.SimpleAction btn_DWQRApprove;
        private DevExpress.ExpressApp.Actions.SimpleAction btn_DWQRPreviewToApprove;
        private DevExpress.ExpressApp.Actions.SimpleAction btn_DWQRHistory;
        private DevExpress.ExpressApp.Actions.SimpleAction btn_DWQRRollBack;
        private DevExpress.ExpressApp.Actions.SimpleAction btn_DWQRDeliverReport;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction btn_DWQRDateFilter;
    }
}
