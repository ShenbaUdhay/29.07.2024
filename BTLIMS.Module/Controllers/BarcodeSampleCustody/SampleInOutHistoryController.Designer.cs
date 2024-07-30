namespace LDM.Module.Controllers.Barcode_Sample_Custody
{
    partial class SampleInOutHistoryController
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
            this.SAHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SAHistory
            // 
            this.SAHistory.Caption = "History";
            this.SAHistory.ConfirmationMessage = null;
            this.SAHistory.Id = "History";
            this.SAHistory.ImageName = "Action_Search";
            this.SAHistory.ToolTip = null;
            this.SAHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SAHistory_Execute);
            // 
            // SampleInOutHistoryController
            // 
            this.Actions.Add(this.SAHistory);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SAHistory;
    }
}
