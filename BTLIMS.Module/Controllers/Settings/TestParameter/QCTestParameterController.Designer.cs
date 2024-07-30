namespace LDM.Module.Controllers.TestParameter
{
    partial class QCTestParameterController
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
            this.DefaultQCTestParameter = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // DefaultQCTestParameter
            // 
            this.DefaultQCTestParameter.AcceptButtonCaption = null;
            this.DefaultQCTestParameter.CancelButtonCaption = null;
            this.DefaultQCTestParameter.Caption = "QC Default";
            this.DefaultQCTestParameter.ConfirmationMessage = null;
            this.DefaultQCTestParameter.Id = "a1320e1d-bc89-474f-99bd-f612f7c0d40f";
            this.DefaultQCTestParameter.ToolTip = null;
            this.DefaultQCTestParameter.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.DefaultQCTestParameter_CustomizePopupWindowParams);
            // 
            // QCTestParameterController
            // 
            this.Actions.Add(this.DefaultQCTestParameter);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction DefaultQCTestParameter;
    }
}
