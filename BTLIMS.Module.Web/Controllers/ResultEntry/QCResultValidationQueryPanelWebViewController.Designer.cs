namespace LDM.Module.Web.Controllers.ResultEntry
{
    partial class QCResultValidationQueryPanelWebViewController
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
            this.QCResultValidation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QCResultApproval = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.OpenDataReviewJobId = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Level1ResultViewHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Level2ResultViewHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // QCResultValidation
            // 
            this.QCResultValidation.Caption = "Result Validation";
            this.QCResultValidation.Category = "View";
            this.QCResultValidation.ConfirmationMessage = null;
            this.QCResultValidation.Id = "QCResultValidation";
            this.QCResultValidation.ToolTip = null;
            this.QCResultValidation.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.QCResultValidation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QCResultValidation_Execute);
            // 
            // QCResultApproval
            // 
            this.QCResultApproval.Caption = "Result Approval";
            this.QCResultApproval.Category = "View";
            this.QCResultApproval.ConfirmationMessage = null;
            this.QCResultApproval.Id = "QCResultApproval";
            this.QCResultApproval.TargetViewType = DevExpress.ExpressApp.ViewType.DashboardView;
            this.QCResultApproval.ToolTip = null;
            this.QCResultApproval.TypeOfView = typeof(DevExpress.ExpressApp.DashboardView);
            this.QCResultApproval.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ResultApproval_Execute);
            // 
            // OpenDataReviewJobId
            // 
            this.OpenDataReviewJobId.Caption = "Open";
            this.OpenDataReviewJobId.ConfirmationMessage = null;
            this.OpenDataReviewJobId.Id = "OpenDataReviewJobId";
            this.OpenDataReviewJobId.ToolTip = "Open";
            this.OpenDataReviewJobId.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.OpenDataReviewJobId_Execute);
            // 
            // Level1ResultViewHistory
            // 
            this.Level1ResultViewHistory.Caption = "History";
            this.Level1ResultViewHistory.Category = "RecordEdit";
            this.Level1ResultViewHistory.ConfirmationMessage = null;
            this.Level1ResultViewHistory.Id = "Level1ResultViewHistory";
            this.Level1ResultViewHistory.ImageName = "Action_Search";
            this.Level1ResultViewHistory.ToolTip = null;
            this.Level1ResultViewHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Level1ResultViewHistory_Execute);
            // 
            // Level2ResultViewHistory
            // 
            this.Level2ResultViewHistory.Caption = "History ";
            this.Level2ResultViewHistory.Category = "RecordEdit";
            this.Level2ResultViewHistory.ConfirmationMessage = null;
            this.Level2ResultViewHistory.Id = "Level2ResultViewHistory";
            this.Level2ResultViewHistory.ImageName = "Action_Search";
            this.Level2ResultViewHistory.ToolTip = null;
            this.Level2ResultViewHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Level2ResultViewHistory_Execute);
            // 
            // QCResultValidationQueryPanelWebViewController
            // 
            this.Actions.Add(this.QCResultValidation);
            this.Actions.Add(this.QCResultApproval);
            this.Actions.Add(this.OpenDataReviewJobId);
            this.Actions.Add(this.Level1ResultViewHistory);
            this.Actions.Add(this.Level2ResultViewHistory);
            this.ViewControlsCreated += new System.EventHandler(this.QCResultValidationQueryPanelWebViewController_ViewControlsCreated);

        }

        #endregion

        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction QCResultValidationQueryPanel;
        private DevExpress.ExpressApp.Actions.SimpleAction QCResultValidation;
        private DevExpress.ExpressApp.Actions.SimpleAction QCResultApproval;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction RE_QCJobIDPopUp;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction RE_QCSampleIDPopUp;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction RE_QCBatchIDPopUp;
        private DevExpress.ExpressApp.Actions.SimpleAction OpenDataReviewJobId;
        private DevExpress.ExpressApp.Actions.SimpleAction Level1ResultViewHistory;
        private DevExpress.ExpressApp.Actions.SimpleAction Level2ResultViewHistory;
    }
}
