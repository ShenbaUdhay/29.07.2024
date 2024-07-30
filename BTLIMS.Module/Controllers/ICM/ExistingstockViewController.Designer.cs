namespace LDM.Module.Controllers.ICM
{
    partial class ExistingstockViewController
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
            this.StockSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.LTBarcodeReportES = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.LTPreviewReportES = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ExistingstockRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddItem = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.Existstockdelete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Existingstockview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // StockSave
            // 
            this.StockSave.Caption = "Save";
            this.StockSave.ConfirmationMessage = null;
            this.StockSave.Id = "StockSave";
            this.StockSave.ToolTip = null;
            // 
            // LTBarcodeReportES
            // 
            this.LTBarcodeReportES.Caption = "Barcode Report";
            this.LTBarcodeReportES.Category = "View";
            this.LTBarcodeReportES.ConfirmationMessage = null;
            this.LTBarcodeReportES.Id = "LTBarcodeReportES";
            this.LTBarcodeReportES.ToolTip = null;
            // 
            // LTPreviewReportES
            // 
            this.LTPreviewReportES.Caption = "Preview Report";
            this.LTPreviewReportES.Category = "View";
            this.LTPreviewReportES.ConfirmationMessage = null;
            this.LTPreviewReportES.Id = "LTPreviewReportES";
            this.LTPreviewReportES.ToolTip = null;
            this.LTPreviewReportES.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LTPreviewReportES_Execute);
            // 
            // ExistingstockRollback
            // 
            this.ExistingstockRollback.Caption = "Rollback";
            this.ExistingstockRollback.ConfirmationMessage = null;
            this.ExistingstockRollback.Id = "ExistingstockRollback";
            this.ExistingstockRollback.ToolTip = null;
            this.ExistingstockRollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExistingstockRollback_Execute);
            // 
            // AddItem
            // 
            this.AddItem.AcceptButtonCaption = null;
            this.AddItem.CancelButtonCaption = null;
            this.AddItem.Caption = "Add Exist Item";
            this.AddItem.ConfirmationMessage = null;
            this.AddItem.Id = "AddExistItem";
            this.AddItem.ToolTip = null;
            this.AddItem.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.AddItem_CustomizePopupWindowParams);
            this.AddItem.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.AddItem_Execute);
            // 
            // Existstockdelete
            // 
            this.Existstockdelete.Caption = "Delete";
            this.Existstockdelete.Category = "RecordEdit";
            this.Existstockdelete.ConfirmationMessage = null;
            this.Existstockdelete.Id = "Existstockdelete";
            this.Existstockdelete.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Existstockdelete.ToolTip = null;
            this.Existstockdelete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Existstockdelete_Execute);
            // 
            // Existingstockview
            // 
            this.Existingstockview.Caption = "View";
            this.Existingstockview.Category = "View";
            this.Existingstockview.ConfirmationMessage = null;
            this.Existingstockview.Id = "Existingstockview";
            this.Existingstockview.ToolTip = null;
            this.Existingstockview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Existingstockview_Execute);
            // 
            // ExistingstockViewController
            // 
            this.Actions.Add(this.StockSave);
            this.Actions.Add(this.LTBarcodeReportES);
            this.Actions.Add(this.LTPreviewReportES);
            this.Actions.Add(this.ExistingstockRollback);
            this.Actions.Add(this.AddItem);
            this.Actions.Add(this.Existstockdelete);
            this.Actions.Add(this.Existingstockview);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction StockSave;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction LTBarcodeReportES;
        private DevExpress.ExpressApp.Actions.SimpleAction LTPreviewReportES;
        private DevExpress.ExpressApp.Actions.SimpleAction ExistingstockRollback;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction AddItem;
        private DevExpress.ExpressApp.Actions.SimpleAction Existstockdelete;
        private DevExpress.ExpressApp.Actions.SimpleAction Existingstockview;
    }
}
