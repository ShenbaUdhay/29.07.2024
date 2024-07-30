namespace LDM.Module.Controllers.SubOutTracking
{
    partial class SubOutTrackingViewController
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
            this.SubOut = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SubOutview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SubOut
            // 
            this.SubOut.Caption = "Save";
            this.SubOut.Category = "View";
            this.SubOut.ConfirmationMessage = null;
            this.SubOut.Id = "SubOut";
            this.SubOut.ToolTip = null;
            this.SubOut.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SubOut_Execute);
            // 
            // SubOutview
            // 
            this.SubOutview.Caption = "Subout View";
            this.SubOutview.ConfirmationMessage = null;
            this.SubOutview.Id = "Suboutview";
            this.SubOutview.ToolTip = null;
            this.SubOutview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SubOutview_Execute);
            // 
            // SubOutTrackingViewController
            // 
            this.Actions.Add(this.SubOut);
            this.Actions.Add(this.SubOutview);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SubOut;
        private DevExpress.ExpressApp.Actions.SimpleAction SubOutview;
    }
}
