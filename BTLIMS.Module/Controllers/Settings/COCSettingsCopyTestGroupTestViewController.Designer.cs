
namespace LDM.Module.Controllers.Settings
{
    partial class COCSettingsCopyTestGroupTestViewController
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
            this.COC_CopyTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SL_CopyTest
            //            
            this.COC_CopyTest.Caption = "Copy Test";
            this.COC_CopyTest.ConfirmationMessage = null;
            this.COC_CopyTest.Id = "COC_CopyTest";
            this.COC_CopyTest.ToolTip = null;
            this.COC_CopyTest.Execute += COC_CopyTest_Execute;
            //            
            // SL_CopyTestViewController
            // 
            this.Actions.Add(this.COC_CopyTest);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction COC_CopyTest;
    }
}
