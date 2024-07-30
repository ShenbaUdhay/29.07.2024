namespace LDM.Module.Controllers.ICM
{
    partial class ConsumptionViewController
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
            this.Consume = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ConsumeQueryPanel = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.txtBarcodeActionConsumption = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            this.ConsumeReturn = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.PendingConsume = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.Consumeview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ConsumeDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // Consume
            // 
            this.Consume.Caption = "Consume";
            this.Consume.ConfirmationMessage = null;
            this.Consume.Id = "Consume";
            this.Consume.ToolTip = null;
            this.Consume.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Consume_Execute);
            // 
            // ConsumeQueryPanel
            // 
            this.ConsumeQueryPanel.AcceptButtonCaption = null;
            this.ConsumeQueryPanel.CancelButtonCaption = null;
            this.ConsumeQueryPanel.Caption = "Consume Query Panel";
            this.ConsumeQueryPanel.Category = "View";
            this.ConsumeQueryPanel.ConfirmationMessage = null;
            this.ConsumeQueryPanel.Id = "ConsumeQueryPanel";
            this.ConsumeQueryPanel.ToolTip = null;
            this.ConsumeQueryPanel.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ConsumeQueryPanel_CustomizePopupWindowParams);
            this.ConsumeQueryPanel.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ConsumeQueryPanel_Execute);
            // 
            // txtBarcodeActionConsumption
            // 
            this.txtBarcodeActionConsumption.Caption = "Barcode";
            this.txtBarcodeActionConsumption.Category = "View";
            this.txtBarcodeActionConsumption.ConfirmationMessage = null;
            this.txtBarcodeActionConsumption.Id = "txtBarcodeActionConsumption";
            this.txtBarcodeActionConsumption.NullValuePrompt = null;
            this.txtBarcodeActionConsumption.ShortCaption = null;
            this.txtBarcodeActionConsumption.ToolTip = null;
            // 
            // ConsumeReturn
            // 
            this.ConsumeReturn.AcceptButtonCaption = null;
            this.ConsumeReturn.CancelButtonCaption = null;
            this.ConsumeReturn.Caption = "Consume Return";
            this.ConsumeReturn.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.ConsumeReturn.ConfirmationMessage = null;
            this.ConsumeReturn.Id = "ConsumeReturn";
            this.ConsumeReturn.ToolTip = null;
            this.ConsumeReturn.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ConsumeReturn_CustomizePopupWindowParams);
            this.ConsumeReturn.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ConsumeReturn_Execute);
            // 
            // PendingConsume
            // 
            this.PendingConsume.AcceptButtonCaption = null;
            this.PendingConsume.CancelButtonCaption = null;
            this.PendingConsume.Caption = "Search Items";
            this.PendingConsume.ConfirmationMessage = null;
            this.PendingConsume.Id = "PendingConsume";
            this.PendingConsume.ToolTip = null;
            this.PendingConsume.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.PendingConsume_CustomizePopupWindowParams);
            this.PendingConsume.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.PendingConsume_Execute);
            // 
            // Consumeview
            // 
            this.Consumeview.Caption = "History";
            this.Consumeview.ImageName = "Action_Search";
            this.Consumeview.ConfirmationMessage = null;
            this.Consumeview.Id = "Consumeview";
            this.Consumeview.ToolTip = null;
            this.Consumeview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Consumeview_Execute);
            // 
            // ConsumeDateFilter
            // 
            this.ConsumeDateFilter.Caption = "Date Filter";
            this.ConsumeDateFilter.ConfirmationMessage = null;
            this.ConsumeDateFilter.Id = "ConsumeDateFilter";
            this.ConsumeDateFilter.ToolTip = null;
            this.ConsumeDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.ConsumeDateFilter_Execute);
            // 
            // ConsumptionViewController
            // 
            this.Actions.Add(this.Consume);
            this.Actions.Add(this.ConsumeQueryPanel);
            this.Actions.Add(this.txtBarcodeActionConsumption);
            this.Actions.Add(this.ConsumeReturn);
            this.Actions.Add(this.PendingConsume);
            this.Actions.Add(this.Consumeview);
            this.Actions.Add(this.ConsumeDateFilter);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Consume;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ConsumeQueryPanel;
        private DevExpress.ExpressApp.Actions.ParametrizedAction txtBarcodeActionConsumption;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ConsumeReturn;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction PendingConsume;
        private DevExpress.ExpressApp.Actions.SimpleAction Consumeview;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction ConsumeDateFilter;
    }
}
