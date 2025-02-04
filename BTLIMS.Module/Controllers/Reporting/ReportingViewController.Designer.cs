﻿using DevExpress.ExpressApp.Actions;
using System;

namespace BTLIMS.Module.Controllers
{
    partial class ReportingWebController
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
            this.ReportPreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.ReportRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DocumentPreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ExcelPreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveReportView = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportValidate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportApproval = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportDelete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Reportview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.EditReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Comment = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PreviewRollbackReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Level1ReviewView = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Level2ReviewView = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Level2ReviewViewDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.Retrive = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CaseNarrative = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            // 
            // ReportPreview
            // 
            this.ReportPreview.Caption = "Preview Report";
            this.ReportPreview.Category = "View";
            this.ReportPreview.ConfirmationMessage = null;
            this.ReportPreview.Id = "PreviewReport";
            this.ReportPreview.TargetViewId = "";
            this.ReportPreview.TargetObjectsCriteria = "NotReport == false";
            this.ReportPreview.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ReportPreview.ToolTip = null;
            this.ReportPreview.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ReportPreview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportPreview_Execute);
            // 
            // ReportRollback
            // 
            //this.ReportRollback.Caption = "Rollback";
            //this.ReportRollback.Category = "View";
            //this.ReportRollback.ConfirmationMessage = null;
            //this.ReportRollback.Id = "ReportRollback";
            //this.ReportRollback.TargetViewId = "";
            //this.ReportRollback.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            //this.ReportRollback.ToolTip = null;
            //this.ReportRollback.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            //this.ReportRollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportRollback_Execute);
            // 
            // DocumentPreview
            // 
            this.DocumentPreview.Caption = "Preview Document";
            this.DocumentPreview.Category = "View";
            this.DocumentPreview.ConfirmationMessage = null;
            this.DocumentPreview.Id = "DocumentPreview";
            this.DocumentPreview.ImageName = "Action_Export_ToDOCX";
            this.DocumentPreview.TargetViewId = "";
            this.DocumentPreview.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.DocumentPreview.ToolTip = null;
            this.DocumentPreview.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.DocumentPreview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DocumentPreview_Execute);
            // 
            // ExcelPreview
            // 
            this.ExcelPreview.Caption = "Preview Excel";
            this.ExcelPreview.Category = "View";
            this.ExcelPreview.ConfirmationMessage = null;
            this.ExcelPreview.Id = "ExcelPreview";
            this.ExcelPreview.ImageName = "Action_Export_ToXLSX";
            this.ExcelPreview.TargetViewId = "";
            this.ExcelPreview.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ExcelPreview.ToolTip = null;
            this.ExcelPreview.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ExcelPreview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExcelPreview_Execute);
            // 
            // SaveReport
            // 
            this.SaveReport.Caption = "Save";
            this.SaveReport.Category = "Edit";
            this.SaveReport.ConfirmationMessage = null;
            this.SaveReport.Id = "SaveReport";
            this.SaveReport.TargetViewId = "";
            this.SaveReport.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SaveReport.ToolTip = null;
            this.SaveReport.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SaveReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveReport_Execute);
            // 
            // SaveReportView
            // 
            this.SaveReportView.Caption = "Save";
            this.SaveReportView.Category = "Edit";
            this.SaveReportView.ConfirmationMessage = null;
            this.SaveReportView.Id = "SaveReportView";
            this.SaveReportView.ImageName = "Save_16x16";
            this.SaveReportView.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.SaveReportView.ToolTip = null;
            this.SaveReportView.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.SaveReportView.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveReportView_Execute);
            // 
            // ReportValidate
            // 
            this.ReportValidate.Caption = "ReportValidate";
            this.ReportValidate.Category = "Edit";
            this.ReportValidate.ConfirmationMessage = null;
            this.ReportValidate.Id = "ReportValidate";
            this.ReportValidate.TargetViewId = "Reporting_ListView";
            this.ReportValidate.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ReportValidate.ToolTip = null;
            this.ReportValidate.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ReportValidate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportValidate_Execute);
            // 
            // ReportApproval
            // 
            this.ReportApproval.Caption = "Report Approval";
            this.ReportApproval.Category = "Edit";
            this.ReportApproval.ConfirmationMessage = null;
            this.ReportApproval.Id = "ReportApproval";
            this.ReportApproval.TargetViewId = "Reporting_ListView_Copy_ReportApproval";
            this.ReportApproval.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ReportApproval.ToolTip = null;
            this.ReportApproval.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ReportApproval.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportApproval_Execute);
            // 
            // ReportDelete
            // 
            this.ReportDelete.Caption = "Delete";
            this.ReportDelete.Category = "View";
            this.ReportDelete.ConfirmationMessage = null;
            this.ReportDelete.Id = "ReportDelete";
            this.ReportDelete.ToolTip = null;
            this.ReportDelete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportDelete_Execute);
            // 
            // Reportview
            // 
            this.Reportview.Caption = "History";
            this.Reportview.ImageName = "Action_Search";
            this.Reportview.Category = "View";
            this.Reportview.ConfirmationMessage = null;
            this.Reportview.Id = "Reportview";
            this.Reportview.ToolTip = null;
            this.Reportview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Reportview_Execute);
            // 

            this.Retrive.Caption = "Retrive";
            this.Retrive.ImageName = "Action_Redo";
            this.Retrive.Category = "View";
            this.Retrive.Id = "Retrive";
            this.Retrive.ToolTip = null;
            this.Retrive.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Retrive_Execute);




            // Level1ReviewView
            // 
            this.Level1ReviewView.Caption = "History";
            this.Level1ReviewView.ImageName = "Action_Search";
            this.Level1ReviewView.Category = "View";
            this.Level1ReviewView.ConfirmationMessage = null;
            this.Level1ReviewView.Id = "Level1ReviewView";
            this.Level1ReviewView.ToolTip = null;
            this.Level1ReviewView.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Level1ReviewView_Execute);
            // 

            // Level2ReviewView
            // 
            this.Level2ReviewView.Caption = "History";
            this.Level2ReviewView.ImageName = "Action_Search";
            this.Level2ReviewView.Category = "View";
            this.Level2ReviewView.ConfirmationMessage = null;
            this.Level2ReviewView.Id = "Level2ReviewView";
            this.Level2ReviewView.ToolTip = null;
            this.Level2ReviewView.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Level2ReviewView_Execute);
            // 

            // EditReport
            // 
            this.EditReport.Caption = "Edit Report";
            this.EditReport.Category = "View";
            this.EditReport.ConfirmationMessage = null;
            this.EditReport.Id = "EditReport";
            this.EditReport.ToolTip = "EditReport";
            this.EditReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.EditReport_Execute);
            // 
            // Comment
            // 
            this.Comment.AcceptButtonCaption = null;
            this.Comment.CancelButtonCaption = null;
            this.Comment.Caption = "Comment";
            this.Comment.Category = "Edit";
            this.Comment.ConfirmationMessage = null;
            this.Comment.Id = "Comment";
            this.Comment.ToolTip = null;
            this.Comment.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.Comment_CustomizePopupWindowParams);
            //this.Comment.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.Comment_Execute);
            // 
            // CaseNarrative
            // 
            //this.CaseNarrative.AcceptButtonCaption = null;
            //this.CaseNarrative.CancelButtonCaption = null;
            this.CaseNarrative.Caption = "Case Narrative";
            this.CaseNarrative.Category = "Edit";
            this.CaseNarrative.ConfirmationMessage = null;
            this.CaseNarrative.Id = "CaseNarrative";
            this.CaseNarrative.ImageName = "Case_Narrative_Image";
            this.CaseNarrative.ToolTip = null;
            this.CaseNarrative.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CaseNarrative_Execute);
            // PreviewRollbackReport
            // 
            this.PreviewRollbackReport.Caption = "Preview";
            this.PreviewRollbackReport.Category = "View";
            this.PreviewRollbackReport.ConfirmationMessage = null;
            this.PreviewRollbackReport.Id = "PreviewRollbackReport";
            this.PreviewRollbackReport.TargetViewId = "";
            this.PreviewRollbackReport.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.PreviewRollbackReport.ToolTip = null;
            this.PreviewRollbackReport.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.PreviewRollbackReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreviewRollbackReport_Execute);
            // 
            // Level2ReviewViewDateFilter
            // 
            this.Level2ReviewViewDateFilter.Caption = "Date Filter";
            this.Level2ReviewViewDateFilter.Category = "View";
            this.Level2ReviewViewDateFilter.ConfirmationMessage = null;
            this.Level2ReviewViewDateFilter.Id = "Level2ReviewViewDateFilter";
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
            this.Level2ReviewViewDateFilter.Items.Add(choiceActionItem1);
            this.Level2ReviewViewDateFilter.Items.Add(choiceActionItem2);
            this.Level2ReviewViewDateFilter.Items.Add(choiceActionItem3);
            this.Level2ReviewViewDateFilter.Items.Add(choiceActionItem4);
            this.Level2ReviewViewDateFilter.Items.Add(choiceActionItem5);
            this.Level2ReviewViewDateFilter.Items.Add(choiceActionItem6);
            this.Level2ReviewViewDateFilter.Items.Add(choiceActionItem7);
            this.Level2ReviewViewDateFilter.SelectedItemChanged += Level2ReviewViewDateFilter_SelectedItemChanged;
            // 
            // ReportingWebController
            // 
            this.Actions.Add(this.ReportPreview);
            //this.Actions.Add(this.ReportRollback);
            this.Actions.Add(this.DocumentPreview);
            this.Actions.Add(this.ExcelPreview);
            this.Actions.Add(this.SaveReport);
            this.Actions.Add(this.SaveReportView);
            this.Actions.Add(this.ReportValidate);
            this.Actions.Add(this.ReportApproval);
            this.Actions.Add(this.ReportDelete);
            this.Actions.Add(this.Reportview);
            this.Actions.Add(this.EditReport);
            this.Actions.Add(this.Comment);
            this.Actions.Add(this.PreviewRollbackReport);
            this.Actions.Add(this.Level1ReviewView);
            this.Actions.Add(this.Level2ReviewView);
            this.Actions.Add(this.Level2ReviewViewDateFilter);
            this.Actions.Add(this.Retrive);
            this.Actions.Add(this.CaseNarrative);

        }
        #endregion

        //private DevExpress.ExpressApp.Actions.SimpleAction ReportRollback;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportPreview;
        private DevExpress.ExpressApp.Actions.SimpleAction DocumentPreview;
        private DevExpress.ExpressApp.Actions.SimpleAction ExcelPreview;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveReport;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveReportView;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportValidate;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportApproval;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportDelete;
        private DevExpress.ExpressApp.Actions.SimpleAction Reportview;
        private DevExpress.ExpressApp.Actions.SimpleAction EditReport;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction Comment;
        private DevExpress.ExpressApp.Actions.SimpleAction PreviewRollbackReport;
        private DevExpress.ExpressApp.Actions.SimpleAction Level1ReviewView;
        private DevExpress.ExpressApp.Actions.SimpleAction Level2ReviewView;
        private DevExpress.ExpressApp.Actions.SimpleAction Retrive;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction Level2ReviewViewDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction CaseNarrative;

    }
}
