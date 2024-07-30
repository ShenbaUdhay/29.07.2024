namespace LDM.Module.Controllers.ICM
{
    partial class ItemViewController
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
            this.Reorder = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RequisitionDate = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.Itemsave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportFromFileAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.removeItemsAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.exportitemsstock = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Reorder
            // 
            this.Reorder.Caption = "Re-Order";
            this.Reorder.Category = "View";
            this.Reorder.ConfirmationMessage = null;
            this.Reorder.Id = "Reorder";
            this.Reorder.ImageName = "";
            this.Reorder.ToolTip = null;
            this.Reorder.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Reorder_Execute);
            // 
            // RequisitionDate
            // 
            this.RequisitionDate.Caption = "Date Filter";
            this.RequisitionDate.ConfirmationMessage = null;
            this.RequisitionDate.Id = "RequisitionDate";
            this.RequisitionDate.ToolTip = null;
            this.RequisitionDate.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.RequisitionDate_Execute);
            // 
            // Itemsave
            // 
            this.Itemsave.Caption = "Save";
            this.Itemsave.ConfirmationMessage = null;
            this.Itemsave.Id = "Itemsave";
            this.Itemsave.ToolTip = null;
            this.Itemsave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Itemsave_Execute);
            // 
            // ImportFromFileAction
            // 
            this.ImportFromFileAction.AcceptButtonCaption = null;
            this.ImportFromFileAction.CancelButtonCaption = null;
            this.ImportFromFileAction.Caption = "Import File";
            this.ImportFromFileAction.ConfirmationMessage = null;
            this.ImportFromFileAction.Id = "ImportFromFileAction";
            this.ImportFromFileAction.Category = "RecordEdit";
            this.ImportFromFileAction.ToolTip = null;
            this.ImportFromFileAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportFromFileAction_CustomizePopupWindowParams);
            this.ImportFromFileAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportFileAction_Execute);
            // 
            // removeItemsAction
            // 
            this.removeItemsAction.Caption = "Delete";
            this.removeItemsAction.Category = "RecordEdit";
            this.removeItemsAction.ConfirmationMessage = null;
            this.removeItemsAction.Id = "removeItemsAction";
            this.removeItemsAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.removeItemsAction.ToolTip = null;
            this.removeItemsAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.removeItemsAction_Execute);
            // 
            // exportitemsstock
            // 
            this.exportitemsstock.Caption = "Export items stock";
            this.exportitemsstock.Category = "RecordEdit";
            this.exportitemsstock.ConfirmationMessage = null;
            this.exportitemsstock.Id = "exportitemsstock";
            this.exportitemsstock.ToolTip = null;
            this.exportitemsstock.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.exportitemsstock_Execute);
            // 
            // ItemViewController
            // 
            this.Actions.Add(this.Reorder);
            this.Actions.Add(this.RequisitionDate);
            this.Actions.Add(this.Itemsave);
            this.Actions.Add(this.ImportFromFileAction);
            this.Actions.Add(this.removeItemsAction);
            this.Actions.Add(this.exportitemsstock);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Reorder;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction RequisitionDate;
        private DevExpress.ExpressApp.Actions.SimpleAction Itemsave;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportFromFileAction;
        private DevExpress.ExpressApp.Actions.SimpleAction removeItemsAction;
        private DevExpress.ExpressApp.Actions.SimpleAction exportitemsstock;
    }
}
