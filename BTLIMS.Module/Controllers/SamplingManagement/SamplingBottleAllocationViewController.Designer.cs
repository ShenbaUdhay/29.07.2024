using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SamplingManagement
{
    partial class SamplingBottleAllocationViewController
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
            this.btnAssignbottles_SamplingBottleAllocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnCopybottles_SamplingBottleAllocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnResetbottles_SamplingBottleAllocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QtyOkSampling = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QtyResetSampling = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // btnCopybottles_BottleAllocation
            // 
            this.btnAssignbottles_SamplingBottleAllocation.Caption = "Assign Bottles";
            this.btnAssignbottles_SamplingBottleAllocation.ConfirmationMessage = null;
            this.btnAssignbottles_SamplingBottleAllocation.Id = "btnAssignbottles_SamplingBottleAllocation";
            this.btnAssignbottles_SamplingBottleAllocation.ImageName = "AssignBottle16";
            this.btnAssignbottles_SamplingBottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnAssignbottles_SamplingBottleAllocation.ToolTip = null;
            this.btnAssignbottles_SamplingBottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnAssignbottles_BottleAllocation_Execute);
            // 
            // btnCopybottles_BottleAllocation
            // 
            this.btnCopybottles_SamplingBottleAllocation.Caption = "Copy Bottles";
            this.btnCopybottles_SamplingBottleAllocation.ConfirmationMessage = null;
            this.btnCopybottles_SamplingBottleAllocation.Id = "btnCopybottles_SamplingBottleAllocation";
            this.btnCopybottles_SamplingBottleAllocation.ImageName = "CopyBottles16";
            this.btnCopybottles_SamplingBottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnCopybottles_SamplingBottleAllocation.ToolTip = null;
            this.btnCopybottles_SamplingBottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnCopybottles_BottleAllocation_Execute);
            // 
            // btnResetbottles_BottleAllocation
            // 
            this.btnResetbottles_SamplingBottleAllocation.Caption = "Reset Bottles";
            this.btnResetbottles_SamplingBottleAllocation.ConfirmationMessage = null;
            this.btnResetbottles_SamplingBottleAllocation.Id = "btnResetbottles_SamplingBottleAllocation";
            this.btnResetbottles_SamplingBottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnResetbottles_SamplingBottleAllocation.ToolTip = null;
            this.btnResetbottles_SamplingBottleAllocation.ImageName = "Action_ResetViewSettings";
            this.btnResetbottles_SamplingBottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnResetbottles_BottleAllocation_Execute);

            //
            //QtyOk
            //
            this.QtyOkSampling.Caption = "Ok";
            this.QtyOkSampling.ConfirmationMessage = null;
            this.QtyOkSampling.Id = "QtyOkSampling";
            this.QtyOkSampling.Category = "catqtyok";
            this.QtyOkSampling.ToolTip = null;
            this.QtyOkSampling.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QtyOk_Execute);
            //
            //QtyReset
            //
            this.QtyResetSampling.Caption = "Reset";
            this.QtyResetSampling.ConfirmationMessage = null;
            this.QtyResetSampling.Id = "QtyResetSampling";
            this.QtyResetSampling.Category = "CatQtyReset";
            this.QtyResetSampling.ToolTip = null;
            this.QtyResetSampling.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QtyReset_Execute);
            // 
            // SampleRegistrationBottleAllocationViewController
            // 
            this.Actions.Add(this.btnAssignbottles_SamplingBottleAllocation);
            this.Actions.Add(this.btnCopybottles_SamplingBottleAllocation);
            this.Actions.Add(this.btnResetbottles_SamplingBottleAllocation);
            this.Actions.Add(this.QtyResetSampling);
            this.Actions.Add(this.QtyOkSampling);
        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction btnAssignbottles_SamplingBottleAllocation;
        private DevExpress.ExpressApp.Actions.SimpleAction btnCopybottles_SamplingBottleAllocation;
        private DevExpress.ExpressApp.Actions.SimpleAction btnResetbottles_SamplingBottleAllocation;
        private DevExpress.ExpressApp.Actions.SimpleAction QtyResetSampling;
        private DevExpress.ExpressApp.Actions.SimpleAction QtyOkSampling;
    }
}
