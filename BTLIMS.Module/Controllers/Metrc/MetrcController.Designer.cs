
namespace LDM.Module.Controllers.Metrc
{
    partial class MetrcController
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
            this.getIncoming = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // getIncoming
            // 
            this.getIncoming.Caption = "Incoming";
            this.getIncoming.ConfirmationMessage = null;
            this.getIncoming.Id = "17ae4896-9f1f-4d5c-8ce8-78ff7eaba694";
            this.getIncoming.ToolTip = null;
            this.getIncoming.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.getIncoming_Execute);
            // 
            // MetrcController
            // 
            this.Actions.Add(this.getIncoming);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction getIncoming;
    }
}
