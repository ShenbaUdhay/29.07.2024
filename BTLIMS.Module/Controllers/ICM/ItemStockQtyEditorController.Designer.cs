
namespace LDM.Module.Controllers.ICM
{
    partial class ItemStockQtyEditorController
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
            this.AddItems = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.StockQtyEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AddItems
            // 
            this.AddItems.Caption = "ADD";
            this.AddItems.Category = "catStockStockQtyAdd";
            this.AddItems.ConfirmationMessage = null;
            this.AddItems.Id = "AddItems";
            this.AddItems.ToolTip = null;
            this.AddItems.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddItems_Execute);
            // 
            // StockQtyEdit
            // 
            this.StockQtyEdit.Caption = "Stock Qty Edit";
            this.StockQtyEdit.Category = "Edit";
            this.StockQtyEdit.ConfirmationMessage = null;
            this.StockQtyEdit.Id = "StockQtyEdit";
            this.StockQtyEdit.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.StockQtyEdit.ImageName = "Action_Edit";
            this.StockQtyEdit.ToolTip = null;
            this.StockQtyEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.StockQtyEdit_Execute);
            // 
            // ItemStockQtyEditorController
            // 
            this.Actions.Add(this.AddItems);
            this.Actions.Add(this.StockQtyEdit);
        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction AddItems;
        private DevExpress.ExpressApp.Actions.SimpleAction StockQtyEdit;
    }
}
