namespace LDM.Module.Controllers.ICM
{
    partial class ReceiveViewController
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
            this.ReceiveQuerPanel = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ReceiveModify = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Receiveview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ReceiveDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.RollbackReceive = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.POReportPreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ReceiveQuerPanel
            // 
            this.ReceiveQuerPanel.AcceptButtonCaption = null;
            this.ReceiveQuerPanel.CancelButtonCaption = null;
            this.ReceiveQuerPanel.Caption = "Receivequerypanel";
            this.ReceiveQuerPanel.ConfirmationMessage = null;
            this.ReceiveQuerPanel.Id = "Receivequerypanel";
            this.ReceiveQuerPanel.ToolTip = null;
            this.ReceiveQuerPanel.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ReceiveQuerPanel_CustomizePopupWindowParams);
            this.ReceiveQuerPanel.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ReceiveQuerPanel_Execute);
            // 
            // ReceiveModify
            // 
            this.ReceiveModify.Caption = "Save";
            this.ReceiveModify.ConfirmationMessage = null;
            this.ReceiveModify.Id = "ReceiveModify";
            this.ReceiveModify.ToolTip = null;
            this.ReceiveModify.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReceiveModify_Execute);
            // 
            // Receiveview
            // 
            this.Receiveview.Caption = "History";
            this.Receiveview.ImageName = "Action_Search";
            this.Receiveview.ConfirmationMessage = null;
            this.Receiveview.Id = "Receiveview";
            this.Receiveview.ToolTip = null;
            this.Receiveview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Receiveview_Execute);
            // 
            // ReceiveDateFilter
            // 
            this.ReceiveDateFilter.Caption = "Date Filter";
            this.ReceiveDateFilter.ConfirmationMessage = null;
            this.ReceiveDateFilter.Id = "ReceiveDateFilter";
            this.ReceiveDateFilter.ToolTip = null;
            this.ReceiveDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ReceiveDateFilter_Execute);
            // 
            // RollbackReceive
            // 
            this.RollbackReceive.AcceptButtonCaption = null;
            this.RollbackReceive.CancelButtonCaption = null;
            this.RollbackReceive.Caption = "Rollback";
            this.RollbackReceive.ConfirmationMessage = null;
            this.RollbackReceive.Id = "RollbackReceive";
            this.RollbackReceive.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.RollbackReceive.ToolTip = null;
            this.RollbackReceive.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.RollbackReceive_CustomizePopupWindowParams);
            this.RollbackReceive.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.RollbackReceive_Execute);
            // 
            // POReportPreview
            // 
            this.POReportPreview.Caption = "Click here";
            this.POReportPreview.Category = "RecordEdit";
            this.POReportPreview.ConfirmationMessage = null;
            this.POReportPreview.Id = "POReportPreview";
            this.POReportPreview.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.POReportPreview.ToolTip = null;
            this.POReportPreview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.POReportPreview_Execute);
            // 
            // ReceiveViewController
            // 
            this.Actions.Add(this.ReceiveQuerPanel);
            this.Actions.Add(this.ReceiveModify);
            this.Actions.Add(this.Receiveview);
            this.Actions.Add(this.ReceiveDateFilter);
            this.Actions.Add(this.RollbackReceive);
            this.Actions.Add(this.POReportPreview);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ReceiveQuerPanel;
        private DevExpress.ExpressApp.Actions.SimpleAction ReceiveModify;
        private DevExpress.ExpressApp.Actions.SimpleAction Receiveview;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ReceiveDateFilter;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction RollbackReceive;
        private DevExpress.ExpressApp.Actions.SimpleAction POReportPreview;
    }
}
