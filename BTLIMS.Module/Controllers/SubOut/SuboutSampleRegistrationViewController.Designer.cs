﻿using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SubOutTracking
{
    partial class SuboutSampleRegistrationViewController
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
            this.SuboutSubmit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.importSuboutResult = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.SuboutViewHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutAllowEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutEDDTempalte = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ResultEntryHistoryDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.SubOutSampleTestOrderDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.SubOutSampleHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SubOutSampleHistoryDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.SuboutNotificationhistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ResultEntryHistorRollbak = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutResultReview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutResultApproval = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutSigningOff = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.NotificationQueueDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.SuboutSigningOffHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutCOCReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.NotificationQueueMailReSend = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutOrderSubmit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutAddSample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SuboutRemoveSample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SuboutSubmit
            // 
            this.SuboutSubmit.Caption = "Submit";
            this.SuboutSubmit.ConfirmationMessage = null;
            this.SuboutSubmit.Id = "SuboutSubmit";
            this.SuboutSubmit.ImageName = "Submit_image";
            this.SuboutSubmit.ToolTip = null;
            this.SuboutSubmit.Category = "Edit";
            this.SuboutSubmit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionPendingForSubout_Execute);
            // 
            // importSuboutResult
            // 
            this.importSuboutResult.AcceptButtonCaption = null;
            this.importSuboutResult.CancelButtonCaption = null;
            this.importSuboutResult.Caption = "EDD Import";
            this.importSuboutResult.Category = "View";
            this.importSuboutResult.ConfirmationMessage = null;
            this.importSuboutResult.Id = "importSuboutResult";
            this.importSuboutResult.ToolTip = null;
            this.importSuboutResult.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.importSuboutResult_CustomizePopupWindowParams);
            this.importSuboutResult.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.importSuboutResult_Execute);
            // 
            // SuboutHistory
            // 
            this.SuboutViewHistory.Caption = "History";
            this.SuboutViewHistory.ConfirmationMessage = null;
            this.SuboutViewHistory.Category = "View";
            this.SuboutViewHistory.Id = "SuboutViewHistory";
            this.SuboutViewHistory.ImageName = "Action_Search";
            this.SuboutViewHistory.ToolTip = null;
            this.SuboutViewHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewHistory_Execute);
            // 
            // SuboutHistory
            // 
            this.SuboutAllowEdit.Caption = "Allow Edit";
            this.SuboutAllowEdit.ConfirmationMessage = null;
            this.SuboutAllowEdit.Id = "SuboutAllowEdit";
            this.SuboutAllowEdit.ToolTip = null;
            this.SuboutAllowEdit.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.SuboutAllowEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SuboutAllowEdit_Execute);
            //
            //
            //
            this.SuboutEDDTempalte.Caption = "EDDTemplate";
            this.SuboutEDDTempalte.ConfirmationMessage = null;
            this.SuboutEDDTempalte.Category = "ListView";
            this.SuboutEDDTempalte.Id = "EDDTemplate ";
            this.SuboutEDDTempalte.ToolTip = null;
            this.SuboutEDDTempalte.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SuboutEDDTempalte.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SuboutEDDTempalte_Execute);
            // 
            // ResultEntryHistoryDateFilter
            // 
            this.ResultEntryHistoryDateFilter.Caption = "Date Filter";
            this.ResultEntryHistoryDateFilter.ConfirmationMessage = null;
            this.ResultEntryHistoryDateFilter.Id = "ResultEntryHistoryDateFilter";
            this.ResultEntryHistoryDateFilter.ToolTip = null;
            this.ResultEntryHistoryDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SuboutResultHistoryDateFilter_Execute);
            // 
            // 
            // 
            this.SubOutSampleHistory.Caption = "History";
            this.SubOutSampleHistory.ImageName = "Action_Search";
            this.SubOutSampleHistory.ConfirmationMessage = null;
            this.SubOutSampleHistory.Id = "SubOutSampleHistory";
            this.SubOutSampleHistory.ToolTip = null;
            this.SubOutSampleHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SuboutSampleHistory_Execute);
            // 
            // SubOutSampleHistoryDateFilter
            // 
            this.SubOutSampleHistoryDateFilter.Caption = "Date Filter";
            this.SubOutSampleHistoryDateFilter.ConfirmationMessage = null;
            this.SubOutSampleHistoryDateFilter.Id = "SubOutSampleHistoryDateFilter";
            this.SubOutSampleHistoryDateFilter.ToolTip = null;
            this.SubOutSampleHistoryDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SubOutSampleHistoryDateFilter_Execute);
            // 
            // SubOutSampleTestOrder
            // 
            this.SubOutSampleTestOrderDateFilter.Caption = "Date Filter";
            this.SubOutSampleTestOrderDateFilter.ConfirmationMessage = null;
            this.SubOutSampleTestOrderDateFilter.Id = "SubOutSampleTestOrderDateFilter";
            this.SubOutSampleTestOrderDateFilter.ToolTip = null;
            this.SubOutSampleTestOrderDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.SubOutSampleTestOrderDateFilter_Execute);
            // 
            // SuboutNotificationhistory
            // 
            this.SuboutNotificationhistory.Caption = "History";
            this.SuboutNotificationhistory.ImageName = "Action_Search";
            this.SuboutNotificationhistory.ConfirmationMessage = null;
            this.SuboutNotificationhistory.Id = "SuboutNotificationhistory";
            this.SuboutNotificationhistory.ToolTip = null;
            this.SuboutNotificationhistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.historyofSuboutNotification_Execute);
            // 
            // ResultEntryHistorRollbak
            // 
            this.ResultEntryHistorRollbak.Caption = "Rollback";
            this.ResultEntryHistorRollbak.ImageName = "Action_Cancel";
            this.ResultEntryHistorRollbak.ConfirmationMessage = null;
            this.ResultEntryHistorRollbak.Id = "ResultEntryHistorRollbak";
            this.ResultEntryHistorRollbak.ToolTip = null;
            this.ResultEntryHistorRollbak.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RollbackResultEntryHistory_Execute);
            // 
            // SuboutResultReview
            // 
            this.SuboutResultReview.Caption = "Review";
            this.SuboutResultReview.ImageName = "";
            this.SuboutResultReview.ConfirmationMessage = null;
            this.SuboutResultReview.Id = "SuboutResultReview";
            this.SuboutResultReview.ToolTip = null;
            this.SuboutResultReview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SuboutResultReview_Execute);
            // 
            // SuboutResultApproval
            // 
            this.SuboutResultApproval.Caption = "Approval";
            this.SuboutResultApproval.ImageName = "";
            this.SuboutResultApproval.ConfirmationMessage = null;
            this.SuboutResultApproval.Id = "SuboutResultApproval";
            this.SuboutResultApproval.ToolTip = null;
            this.SuboutResultApproval.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SuboutResultApproval_Execute);
            // 
            // SuboutSigningOff
            // 
            this.SuboutSigningOff.Caption = "Sign Off";
            this.SuboutSigningOff.ImageName = "";
            this.SuboutSigningOff.ConfirmationMessage = null;
            this.SuboutSigningOff.Id = "SuboutSigningOff";
            this.SuboutSigningOff.ToolTip = null;
            this.SuboutSigningOff.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SigningOff_Execute);
            // 
            // NotificationQueueDateFilter
            // 
            this.NotificationQueueDateFilter.Caption = "Date Filter";
            this.NotificationQueueDateFilter.ImageName = "";
            this.NotificationQueueDateFilter.ConfirmationMessage = null;
            this.NotificationQueueDateFilter.Id = "NotificationQueueDateFilter";
            this.NotificationQueueDateFilter.ToolTip = null;
            this.NotificationQueueDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.NotificationQueueDateFilter_Execute);
            // 
            // SuboutSigningOffHistory
            // 
            this.SuboutSigningOffHistory.Caption = "History";
            this.SuboutSigningOffHistory.ImageName = "Action_Search";
            this.SuboutSigningOffHistory.ConfirmationMessage = null;
            this.SuboutSigningOffHistory.Id = "SuboutSigningOffHistory";
            this.SuboutSigningOffHistory.ToolTip = null;
            this.SuboutSigningOffHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.historyofSuboutSigningOff_Execute);
            // 
            // btnCOC_BarReport
            // 
            this.SuboutCOCReport.Caption = "COC Report";
            this.SuboutCOCReport.Category = "Reports";
            this.SuboutCOCReport.ConfirmationMessage = null;
            this.SuboutCOCReport.Id = "SuboutCOCReport";
            this.SuboutCOCReport.ImageName = "BO_Report";
            this.SuboutCOCReport.TargetObjectsCriteria = "";
            this.SuboutCOCReport.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
            this.SuboutCOCReport.ToolTip = null;
            this.SuboutCOCReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnCOC_BarReport_Execute);
            // 
            // SuboutSigningOffHistory
            // 
            this.NotificationQueueMailReSend.Caption = "Resend";
            this.NotificationQueueMailReSend.ImageName = "";
            this.NotificationQueueMailReSend.ConfirmationMessage = null;
            this.NotificationQueueMailReSend.Id = "NotificationQueueMailReSend";
            this.NotificationQueueMailReSend.ToolTip = null;
            //this.NotificationQueueMailReSend.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.NotificationQueueMailReSend.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ResendMail_Execute);

            this.SuboutEDDTempalte.Caption = "EDDTemplate";
            this.SuboutEDDTempalte.ConfirmationMessage = null;
            this.SuboutEDDTempalte.Id = "EDDTemplate";
            this.SuboutEDDTempalte.ToolTip = null;
            this.SuboutEDDTempalte.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.SuboutEDDTempalte.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SuboutEDDTempalte_Execute);
            // 
            //SuboutOrderSubmit
            //
            // 
            // SuboutSigningOffHistory
            // 
            this.SuboutOrderSubmit.Caption = "Submit";
            this.SuboutOrderSubmit.ImageName = "upload_32x32";
            this.SuboutOrderSubmit.ConfirmationMessage = null;
            this.SuboutOrderSubmit.Id = "SuboutOrderSubmit";
            this.SuboutOrderSubmit.ToolTip = null;
            //this.SuboutOrderSubmit.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.SuboutOrderSubmit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Submit_Execute);
            // 
            // AddSample
            // 
            this.SuboutAddSample.Caption = "Add";
            this.SuboutAddSample.ImageName = "Add_16x16";
            this.SuboutAddSample.ConfirmationMessage = null;
            this.SuboutAddSample.Id = "SuboutAddSample";
            this.SuboutAddSample.ToolTip = "Add";
            this.SuboutAddSample.Execute += AddSample_Execute;
            // 
            // RemoveSample
            // 
            this.SuboutRemoveSample.Caption = "Remove";
            this.SuboutRemoveSample.ImageName = "Remove.png";
            this.SuboutRemoveSample.ConfirmationMessage = null;
            this.SuboutRemoveSample.Id = "SuboutRemoveSample";
            this.SuboutRemoveSample.ToolTip = "Remove";
            this.SuboutRemoveSample.Execute += RemoveSample_Execute; ;
            // 
            // 
            // SuboutSampleRegistrationViewController
            // 
            this.Actions.Add(this.SuboutSubmit);
            this.Actions.Add(this.importSuboutResult);
            this.Actions.Add(this.SuboutViewHistory);
            this.Actions.Add(this.SuboutAllowEdit);
            this.Actions.Add(this.SuboutEDDTempalte);
            this.Actions.Add(this.ResultEntryHistoryDateFilter);
            this.Actions.Add(this.SubOutSampleTestOrderDateFilter);
            this.Actions.Add(this.SubOutSampleHistory);
            this.Actions.Add(this.SubOutSampleHistoryDateFilter);
            this.Actions.Add(this.SuboutNotificationhistory);
            this.Actions.Add(this.ResultEntryHistorRollbak);
            this.Actions.Add(this.SuboutResultReview);
            this.Actions.Add(this.SuboutResultApproval);
            this.Actions.Add(this.SuboutSigningOff);
            this.Actions.Add(this.NotificationQueueDateFilter);
            this.Actions.Add(this.SuboutSigningOffHistory);
            this.Actions.Add(this.SuboutCOCReport);
            this.Actions.Add(this.NotificationQueueMailReSend);
            this.Actions.Add(this.SuboutOrderSubmit);
            this.Actions.Add(this.SuboutAddSample);
            this.Actions.Add(this.SuboutRemoveSample);
        }

     
        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SuboutSubmit;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction importSuboutResult;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutViewHistory;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutAllowEdit;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutEDDTempalte;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ResultEntryHistoryDateFilter;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction SubOutSampleTestOrderDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction SubOutSampleHistory;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction SubOutSampleHistoryDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutNotificationhistory;
        private DevExpress.ExpressApp.Actions.SimpleAction ResultEntryHistorRollbak;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutResultReview;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutResultApproval;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutSigningOff;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction NotificationQueueDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutSigningOffHistory;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutCOCReport;
        private DevExpress.ExpressApp.Actions.SimpleAction NotificationQueueMailReSend;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutOrderSubmit;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutAddSample;
        private DevExpress.ExpressApp.Actions.SimpleAction SuboutRemoveSample;
    }
}
