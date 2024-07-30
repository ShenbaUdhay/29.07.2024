namespace LDM.Module.Controllers.ICM
{
    partial class DisposalViewController
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
            this.Disposal = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DisposalQueryPanel = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.DisposalReturn = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.txtBarcodeActionDisposal = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            this.DisposalView = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DisposalDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // Disposal
            // 
            this.Disposal.Caption = "Dispose";
            this.Disposal.ConfirmationMessage = null;
            this.Disposal.Id = "Dispose";
            this.Disposal.ToolTip = null;
            this.Disposal.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Disposal_Execute);
            // 
            // DisposalQueryPanel
            // 
            this.DisposalQueryPanel.AcceptButtonCaption = null;
            this.DisposalQueryPanel.CancelButtonCaption = null;
            this.DisposalQueryPanel.Caption = "Dispose Filter";
            this.DisposalQueryPanel.ConfirmationMessage = null;
            this.DisposalQueryPanel.Id = "DisposeQuerypanel";
            this.DisposalQueryPanel.ToolTip = null;
            this.DisposalQueryPanel.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.DisposalQueryPanel_CustomizePopupWindowParams);
            this.DisposalQueryPanel.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.DisposalQueryPanel_Execute);
            // 
            // DisposalReturn
            // 
            this.DisposalReturn.AcceptButtonCaption = null;
            this.DisposalReturn.CancelButtonCaption = null;
            this.DisposalReturn.Caption = "Dispose Return";
            this.DisposalReturn.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.DisposalReturn.ConfirmationMessage = null;
            this.DisposalReturn.Id = "DisposeReturn";
            this.DisposalReturn.ToolTip = null;
            this.DisposalReturn.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.DisposalReturn_CustomizePopupWindowParams);
            this.DisposalReturn.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.DisposalReturn_Execute);
            // 
            // txtBarcodeActionDisposal
            // 
            this.txtBarcodeActionDisposal.Caption = "Scan";
            this.txtBarcodeActionDisposal.Category = "View";
            this.txtBarcodeActionDisposal.ConfirmationMessage = null;
            this.txtBarcodeActionDisposal.Id = "txtBarcodeActionDisposal";
            this.txtBarcodeActionDisposal.NullValuePrompt = null;
            this.txtBarcodeActionDisposal.ShortCaption = null;
            this.txtBarcodeActionDisposal.ToolTip = null;
            // 
            // DisposalView
            // 
            this.DisposalView.Caption = "History";
            this.DisposalView.ImageName = "Action_Search";
            this.DisposalView.Category = "RecordEdit";
            this.DisposalView.ConfirmationMessage = null;
            this.DisposalView.Id = "DisposalView";
            this.DisposalView.ToolTip = null;
            this.DisposalView.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DisposalView_Execute);
            // 
            // DisposalDateFilter
            // 
            this.DisposalDateFilter.Caption = "Date Filter";
            this.DisposalDateFilter.Category = "View";
            this.DisposalDateFilter.ConfirmationMessage = null;
            this.DisposalDateFilter.Id = "DisposalDateFilter";
            this.DisposalDateFilter.ToolTip = null;
            this.DisposalDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.DisposalDateFilter_Execute);
            // 
            // DisposalViewController
            // 
            this.Actions.Add(this.Disposal);
            this.Actions.Add(this.DisposalQueryPanel);
            this.Actions.Add(this.DisposalReturn);
            this.Actions.Add(this.txtBarcodeActionDisposal);
            this.Actions.Add(this.DisposalView);
            this.Actions.Add(this.DisposalDateFilter);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Disposal;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction DisposalQueryPanel;
        private DevExpress.ExpressApp.Actions.ParametrizedAction txtBarcodeActionDisposal;
		private DevExpress.ExpressApp.Actions.PopupWindowShowAction DisposalReturn;
        private DevExpress.ExpressApp.Actions.SimpleAction DisposalView;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction DisposalDateFilter;
    }
}
