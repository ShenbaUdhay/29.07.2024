
namespace LDM.Module.Controllers.SalesOrder
{
    partial class SalesOrderWindowController
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
            this.btnsalesorder = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // btnsalesorder
            // 
            this.btnsalesorder.Caption = "Sales Dashboard";
            this.btnsalesorder.Category = "Notifications";
            this.btnsalesorder.ConfirmationMessage = null;
            this.btnsalesorder.Id = "btnsalesorder";
            this.btnsalesorder.ToolTip = null;
            this.btnsalesorder.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnsalesorder_Execute);
            // 
            // SalesOrderWindowController
            // 
            this.Actions.Add(this.btnsalesorder);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction btnsalesorder;
    }
}
