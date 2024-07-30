using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.ICM
{
    partial class DistributionViewController
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
            this.DistributionQueryPanel = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.Distribute = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LTPreviewReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.txtBarcodeActionDistribution = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            this.DistributeRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.DistributeRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DistributionDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.Distributionview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.DistributionSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.NewAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FractionalHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DistributionQueryPanel
            // 
            this.DistributionQueryPanel.AcceptButtonCaption = null;
            this.DistributionQueryPanel.CancelButtonCaption = null;
            this.DistributionQueryPanel.Caption = "Query";
            this.DistributionQueryPanel.ConfirmationMessage = null;
            this.DistributionQueryPanel.Id = "Distributionquerypanel";
            this.DistributionQueryPanel.ToolTip = null;
            this.DistributionQueryPanel.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.DistributionQueryPanel_CustomizePopupWindowParams);
            this.DistributionQueryPanel.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.DistributionQueryPanel_Execute);
            // 
            // Distribute
            // 
            this.Distribute.Caption = "Distribute";
            this.Distribute.ConfirmationMessage = "Do you want to save the distribution?";
            this.Distribute.Id = "Distribute";
            this.Distribute.ToolTip = null;
            this.Distribute.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Distribute_Execute);
            // 
            // LTPreviewReport
            // 
            this.LTPreviewReport.Caption = "PreviewReport";
            this.LTPreviewReport.Category = "View";
            this.LTPreviewReport.ConfirmationMessage = null;
            this.LTPreviewReport.Id = "LTPreviewReport";
            this.LTPreviewReport.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.LTPreviewReport.ToolTip = null;
            this.LTPreviewReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LTPreviewReport_Execute);
            // 
            // txtBarcodeActionDistribution
            // 
            this.txtBarcodeActionDistribution.Caption = "Scan";
            this.txtBarcodeActionDistribution.Category = "View";
            this.txtBarcodeActionDistribution.ConfirmationMessage = null;
            this.txtBarcodeActionDistribution.Id = "txtBarcodeActionDistribution";
            this.txtBarcodeActionDistribution.NullValuePrompt = null;
            this.txtBarcodeActionDistribution.ShortCaption = null;
            this.txtBarcodeActionDistribution.ToolTip = null;
            // 
            // DistributeRollback
            // 
            this.DistributeRollback.Caption = "Rollback";
            this.DistributeRollback.Category = "View";
            this.DistributeRollback.ConfirmationMessage = null;
            this.DistributeRollback.Id = "DistributeRollback";
            this.DistributeRollback.ToolTip = null;
            this.DistributeRollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DistributeRollback_Execute);
            //this.DistributeRollback.Executed += DistributeRollback_Executed;
            // 
            // DistributionDateFilter
            // 
            this.DistributionDateFilter.Caption = "Date Filter";
            this.DistributionDateFilter.Category = "View";
            this.DistributionDateFilter.ConfirmationMessage = null;
            this.DistributionDateFilter.Id = "DistributionDateFilter";
            this.DistributionDateFilter.ToolTip = null;
            this.DistributionDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.DistributionDateFilter_Execute);
            // 
            // Distributionview
            // 
            this.Distributionview.Caption = "History";
            this.Distributionview.ImageName = "Action_Search";
            this.Distributionview.Category = "View";
            this.Distributionview.ConfirmationMessage = null;
            this.Distributionview.Id = "Distributionview";
            this.Distributionview.ToolTip = null;
            this.Distributionview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Distributionview_Execute);

            //// 
            //this.DistributionSave.Caption = "Save";
            //this.DistributionSave.ConfirmationMessage = null;
            //this.DistributionSave.Id = "DistributionSave";
            //this.DistributionSave.ToolTip = null;
            //this.DistributionSave.Category = "RecordEdit";
            //this.DistributionSave.ImageName = "Action_Save";
            ////this.DistributionSave.Model.Index = 4;
            //this.DistributionSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Distributionsave_Execute);
            // 
            // 
            // DistributionViewController
            // 
            // 
            // NewAction
            // 
            this.NewAction.Caption = "New";
            this.NewAction.Category = "Edit";
            this.NewAction.Id = "NewAction";
            this.NewAction.ImageName = "Action_New";
            this.NewAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.NewAction_Executing);
            // 
            // FractionalHistory
            // 
            this.FractionalHistory.Caption = "History";
            this.FractionalHistory.ConfirmationMessage = null;
            this.FractionalHistory.Id = "FractionalHistory";
            this.FractionalHistory.ImageName= "Action_Search";
            this.FractionalHistory.ToolTip = null;
            this.FractionalHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FractionalHistory_Execute);


            this.Actions.Add(this.DistributionQueryPanel);
            this.Actions.Add(this.NewAction);
            this.Actions.Add(this.Distribute);
            this.Actions.Add(this.LTPreviewReport);
            this.Actions.Add(this.txtBarcodeActionDistribution);
            this.Actions.Add(this.DistributeRollback);
            this.Actions.Add(this.DistributionDateFilter);
            this.Actions.Add(this.Distributionview);
            //this.Actions.Add(this.DistributionSave);
            this.Actions.Add(this.FractionalHistory);
        }

       






        #endregion

        private SimpleAction NewAction;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction DistributionQueryPanel;
        private DevExpress.ExpressApp.Actions.SimpleAction Distribute;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction LTBarcodeReport;
        private DevExpress.ExpressApp.Actions.SimpleAction LTPreviewReport;
        private DevExpress.ExpressApp.Actions.ParametrizedAction txtBarcodeActionDistribution;
        private DevExpress.ExpressApp.Actions.SimpleAction DistributeRollback;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction DistributionDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction Distributionview;
        //private DevExpress.ExpressApp.Actions.SimpleAction DistributionSave;
        private DevExpress.ExpressApp.Actions.SimpleAction FractionalHistory;
    }
}
