using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Reporting
{
    partial class ReportManagementViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem16 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem17 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem18 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem19 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem20 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem21 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem22 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.ReportManagementView = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportManagementDelete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportManagementPreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportManagementPreviewDC = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ReportPrint = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportDeliverySave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReportDeliveryPreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ReportManagementView
            // 
            this.ReportManagementView.Caption = "History";
            this.ReportManagementView.ImageName = "Action_Search";
            this.ReportManagementView.Category = "View";
            this.ReportManagementView.ConfirmationMessage = null;
            this.ReportManagementView.Id = "ReportManagementView";
            this.ReportManagementView.ToolTip = null;
            this.ReportManagementView.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportManagementView_Execute);
            // 
            // ReportManagementDelete
            // 
            this.ReportManagementDelete.Caption = "Delete";
            this.ReportManagementDelete.Category = "Edit";
            this.ReportManagementDelete.ConfirmationMessage = null;
            this.ReportManagementDelete.Id = "ReportManagementDelete";
            this.ReportManagementDelete.ToolTip = null;
            this.ReportManagementDelete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportManagementDelete_Execute);
            // 
            // ReportManagementPreview
            // 
            this.ReportManagementPreview.Caption = "Preview";
            this.ReportManagementPreview.Category = "ObjectsCreation";
            this.ReportManagementPreview.ConfirmationMessage = null;
            this.ReportManagementPreview.Id = "ReportManagementPreview";
            this.ReportManagementPreview.ToolTip = null;
            this.ReportManagementPreview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportManagementPreview_Execute);
            // 
            // ReportDateFilterAction
            // 
            this.ReportManagementPreviewDC.Caption = "...";
            this.ReportManagementPreviewDC.Category = "RecordEdit";
            this.ReportManagementPreviewDC.ConfirmationMessage = null;
            this.ReportManagementPreviewDC.Id = "ReportManagementPreviewDC";
            this.ReportManagementPreviewDC.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.ReportManagementPreviewDC.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportManagementPreview_Execute);
            // 
            // ReportDateFilterAction
            // 
            this.ReportDateFilterAction.Caption = "Date Filter";
            this.ReportDateFilterAction.Category = "View";
            this.ReportDateFilterAction.ConfirmationMessage = null;
            this.ReportDateFilterAction.Id = "ReportDateFilterAction";
            choiceActionItem16.Caption = "1M";
            choiceActionItem16.Id = "1M";
            choiceActionItem16.ImageName = null;
            choiceActionItem16.Shortcut = null;
            choiceActionItem16.ToolTip = null;
            choiceActionItem17.Caption = "3M";
            choiceActionItem17.Id = "3M";
            choiceActionItem17.ImageName = null;
            choiceActionItem17.Shortcut = null;
            choiceActionItem17.ToolTip = null;
            choiceActionItem18.Caption = "6M";
            choiceActionItem18.Id = "6M";
            choiceActionItem18.ImageName = null;
            choiceActionItem18.Shortcut = null;
            choiceActionItem18.ToolTip = null;
            choiceActionItem19.Caption = "1Y";
            choiceActionItem19.Id = "1Y";
            choiceActionItem19.ImageName = null;
            choiceActionItem19.Shortcut = null;
            choiceActionItem19.ToolTip = null;
            choiceActionItem20.Caption = "2Y";
            choiceActionItem20.Id = "2Y";
            choiceActionItem20.ImageName = null;
            choiceActionItem20.Shortcut = null;
            choiceActionItem20.ToolTip = null;
            choiceActionItem21.Caption = "5Y";
            choiceActionItem21.Id = "5Y";
            choiceActionItem21.ImageName = null;
            choiceActionItem21.Shortcut = null;
            choiceActionItem21.ToolTip = null;
            choiceActionItem22.Caption = "All";
            choiceActionItem22.Id = "All";
            choiceActionItem22.ImageName = null;
            choiceActionItem22.Shortcut = null;
            choiceActionItem22.ToolTip = null;
            this.ReportDateFilterAction.Items.Add(choiceActionItem16);
            this.ReportDateFilterAction.Items.Add(choiceActionItem17);
            this.ReportDateFilterAction.Items.Add(choiceActionItem18);
            this.ReportDateFilterAction.Items.Add(choiceActionItem19);
            this.ReportDateFilterAction.Items.Add(choiceActionItem20);
            this.ReportDateFilterAction.Items.Add(choiceActionItem21);
            this.ReportDateFilterAction.Items.Add(choiceActionItem22);
            this.ReportDateFilterAction.ToolTip = "Date Filter";
            // 
            // ReportPrint
            // 
            this.ReportPrint.Caption = "Print";
            this.ReportPrint.Category = "PopupActions";
            this.ReportPrint.ConfirmationMessage = null;
            this.ReportPrint.Id = "ReportPrint";
            this.ReportPrint.ToolTip = null;
            this.ReportPrint.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportPrint_Execute);
            // 
            // ReportDeliverySave
            // 
            //this.ReportDeliverySave.Caption = "Delivery";
            this.ReportDeliverySave.Caption = "Send";
            this.ReportDeliverySave.Category = "Edit";
            this.ReportDeliverySave.ConfirmationMessage = null;
            this.ReportDeliverySave.Id = "ReportDeliverySave";
            this.ReportDeliverySave.ToolTip = null;
            this.ReportDeliverySave.ImageName = "Send16x";
            this.ReportDeliverySave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReportDeliverySave_Execute);
            // 
            // ReportDeliveryReport
            // 
            this.ReportDeliveryPreview.Caption = "Preview";
            this.ReportDeliveryPreview.Category = "RecordEdit";
            this.ReportDeliveryPreview.ConfirmationMessage = null;
            this.ReportDeliveryPreview.Id = "ReportDeliveryPreview";
            this.ReportDeliveryPreview.ToolTip = null;
            this.ReportDeliveryPreview.ImageName = "BO_Report";
            this.ReportDeliveryPreview.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.ReportDeliveryPreview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeliveryReportPreview_Execute);
            // 
            // ReportManagementViewController
            // 
            this.Actions.Add(this.ReportManagementView);
            this.Actions.Add(this.ReportManagementDelete);
            this.Actions.Add(this.ReportManagementPreview);
            this.Actions.Add(this.ReportManagementPreviewDC);
            this.Actions.Add(this.ReportDateFilterAction);
            this.Actions.Add(this.ReportPrint);
            this.Actions.Add(this.ReportDeliverySave);
            this.Actions.Add(this.ReportDeliveryPreview);

        }

      


        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ReportManagementView;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportManagementDelete;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportManagementPreview;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportManagementPreviewDC;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ReportDateFilterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportPrint;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportDeliverySave;
        private DevExpress.ExpressApp.Actions.SimpleAction ReportDeliveryPreview;
    }
}
