using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.ICM.ReagentCertificate
{
    partial class VendorReagentCertificateViewController
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
            this.ShowViewModeVRC = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.VRCDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ShowNew = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 

            // showNew
            this.ShowNew.Caption = "New";
            this.ShowNew.Category = "New";
            this.ShowNew.ConfirmationMessage = null;
            this.ShowNew.Id = "ShowNew";
            this.ShowNew.ToolTip = null;          
            this.ShowNew.Execute += ShowNew_Execute;
            // 
            // ShowViewModeVRC
            // 
            this.ShowViewModeVRC.Caption = "History";
            this.ShowViewModeVRC.ImageName = "Action_Search";
            this.ShowViewModeVRC.Category = "View";
            this.ShowViewModeVRC.ConfirmationMessage = null;
            this.ShowViewModeVRC.Id = "ShowViewModeVRC";
            this.ShowViewModeVRC.ToolTip = null;
            this.ShowViewModeVRC.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ShowViewModeVRC_Execute);
            // 
            // VRCDateFilter
            // 
            this.VRCDateFilter.Caption = "Date Filter";
            this.VRCDateFilter.Category = "View";
            this.VRCDateFilter.ConfirmationMessage = null;
            this.VRCDateFilter.Id = "VRCDateFilter";
            this.VRCDateFilter.ToolTip = null;
            this.VRCDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.VRCDateFilter_Execute);
            // 
            // VendorReagentCertificateViewController
            // 
            this.Actions.Add(this.ShowViewModeVRC);
            this.Actions.Add(this.VRCDateFilter);
            this.Actions.Add(this.ShowNew);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction ShowViewModeVRC;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction VRCDateFilter;
        private SimpleAction ShowNew;
    }
}
