
namespace LDM.Module.Web.Controllers.Settings
{
    partial class DBConnectionViewController
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
            this.btnpasswordshown = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ConnectionTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // btnpasswordshown
            // 
            this.btnpasswordshown.Caption = "show";
            this.btnpasswordshown.Category = "Passwordshown";
            this.btnpasswordshown.ConfirmationMessage = null;
            this.btnpasswordshown.Id = "btnpasswordshown";
            this.btnpasswordshown.ImageName = "State_ItemVisibility_Hide";
            this.btnpasswordshown.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.btnpasswordshown.ToolTip = null;
            this.btnpasswordshown.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnpasswordshown_Execute);
            // 
            // ConnectionTest
            // 
            this.ConnectionTest.Caption = "Test Connection";
            this.ConnectionTest.Category = "RecordEdit";
            this.ConnectionTest.ConfirmationMessage = null;
            this.ConnectionTest.Id = "ConnectionTest";
            this.ConnectionTest.ToolTip = null;
            this.ConnectionTest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ConnectionTest_Execute);
            // 
            // DBConnectionViewController
            // 
            this.Actions.Add(this.btnpasswordshown);
            this.Actions.Add(this.ConnectionTest);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction btnpasswordshown;
        private DevExpress.ExpressApp.Actions.SimpleAction ConnectionTest;
    }
}
