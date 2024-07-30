namespace LDM.Module.Controllers.SampleLogIn
{
    partial class SampleLoginCopyTestGroupTestViewController
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
            this.SL_CopyTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SL_GroupTest = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // SL_CopyTest
            // 
            //this.SL_CopyTest.AcceptButtonCaption = null;
            //this.SL_CopyTest.CancelButtonCaption = null;
            this.SL_CopyTest.Caption = "Copy Test";
            this.SL_CopyTest.ConfirmationMessage = null;
            this.SL_CopyTest.Id = "SL_CopyTest";
            //this.SL_CopyTest.Category = "PopupActions";
            this.SL_CopyTest.ToolTip = null;
            //this.SL_CopyTest.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SL_CopyTest_CustomizePopupWindowParams);
            this.SL_CopyTest.Execute += SL_CopyTest_Execute;
            // 
            // SL_GroupTest
            // 
            this.SL_GroupTest.AcceptButtonCaption = null;
            this.SL_GroupTest.CancelButtonCaption = null;
            this.SL_GroupTest.Caption = "Group Test";
            this.SL_GroupTest.ConfirmationMessage = null;
            this.SL_GroupTest.Id = "SL_GroupTest";
            this.SL_GroupTest.ToolTip = null;
            this.SL_GroupTest.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.SL_GroupTest_CustomizePopupWindowParams);
            this.SL_GroupTest.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.SL_GroupTest_Execute);
            // 
            // SL_CopyTestViewController
            // 
            this.Actions.Add(this.SL_CopyTest);
            this.Actions.Add(this.SL_GroupTest);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction SL_CopyTest;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction SL_GroupTest;
    }
}
