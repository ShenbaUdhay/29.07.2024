namespace LDM.Module.Controllers.ICM
{
    partial class POViewController
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
            this.POApprove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.POrollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.POTracking = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.POFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.POReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // POApprove
            // 
            this.POApprove.Caption = "Approve";
            this.POApprove.ConfirmationMessage = null;
            this.POApprove.Id = "PO Approve";
            this.POApprove.TargetViewId = "Purchaseorder_ListView_Approve;Purchaseorder_DetailView_Approve;";
            this.POApprove.ToolTip = null;
            this.POApprove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.POApprove_Execute);
            // 
            // POrollback
            // 
            this.POrollback.Caption = "RollBack";
            this.POrollback.ConfirmationMessage = null;
            this.POrollback.Id = "POrollback";
            this.POrollback.ToolTip = "Delete";
            this.POrollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.POrollback_Execute);
            // 
            // POTracking
            // 
            this.POTracking.Caption = "History";
            this.POTracking.ImageName = "Action_Search";
            this.POTracking.ConfirmationMessage = null;
            this.POTracking.Id = "POTracking";
            this.POTracking.ToolTip = null;
            this.POTracking.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.POTracking_Execute);
            // 
            // POFilter
            // 
            this.POFilter.Caption = "Date Filter";
            this.POFilter.ConfirmationMessage = null;
            this.POFilter.Id = "POFilter";
            this.POFilter.ToolTip = null;
            this.POFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.POFilter_Execute);
            // 
            // POReport
            // 
            this.POReport.Caption = "POReport";
            this.POReport.Category = "RecordEdit";
            this.POReport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.POReport.ConfirmationMessage = null;
            this.POReport.Id = "POReport";
            this.POReport.ToolTip = null;
            this.POReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.POReport_Execute);
            // 
            // POViewController
            // 
            this.Actions.Add(this.POApprove);
            this.Actions.Add(this.POrollback);
            this.Actions.Add(this.POTracking);
            this.Actions.Add(this.POFilter);
            this.Actions.Add(this.POReport);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction POApprove;
        private DevExpress.ExpressApp.Actions.SimpleAction POrollback;
        private DevExpress.ExpressApp.Actions.SimpleAction POTracking;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction POFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction POReport;
    }
}
