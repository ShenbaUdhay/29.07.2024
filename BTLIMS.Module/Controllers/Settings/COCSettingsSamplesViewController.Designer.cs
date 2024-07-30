
using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Settings
{
    partial class COCSettingsSamplesViewController
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
            this.CopySamples = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveCOCSettingSamples = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //
            //// CopySamples
            // 
            this.CopySamples.Caption = "Copy Samples";
            this.CopySamples.Category = "Edit";
            this.CopySamples.ConfirmationMessage = null;
            this.CopySamples.Id = "COCCopySamples1";
            this.CopySamples.ToolTip = "Copy Samples";
            //this.CopySamples.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CopySamples_CustomizePopupWindowParams);
            this.CopySamples.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CopySamples_Execute);

            //
            //// SaveCocsettingSamples
            // 
            this.SaveCOCSettingSamples.Caption = "Save";
            this.SaveCOCSettingSamples.Category = "View";
            this.SaveCOCSettingSamples.ConfirmationMessage = null;
            this.SaveCOCSettingSamples.Id = "SaveCOCSettingSamples";
            this.SaveCOCSettingSamples.ToolTip = "Save";
            this.SaveCOCSettingSamples.ImageName = "Action_Save";
            this.SaveCOCSettingSamples.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Save_Samples);
            // 
            //
            this.Actions.Add(this.CopySamples);
            this.Actions.Add(this.SaveCOCSettingSamples);
            this.ViewControlsCreated += new System.EventHandler(this.COCSampleViewController_ViewControlsCreated);



        }

        private DevExpress.ExpressApp.Actions.SimpleAction CopySamples;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveCOCSettingSamples;

        #endregion
    }
}
