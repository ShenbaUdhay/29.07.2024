
using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.SampleRegistration
{
    partial class SampleRegistrationBottleAllocationViewController
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
            this.btnAssignbottles_BottleAllocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnCopybottles_BottleAllocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnResetbottles_BottleAllocation = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QtyOk = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QtyReset = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // btnCopybottles_BottleAllocation
            // 
            this.btnAssignbottles_BottleAllocation.Caption = "Assign Bottles";
            this.btnAssignbottles_BottleAllocation.ConfirmationMessage = null;
            this.btnAssignbottles_BottleAllocation.Id = "btnAssignbottles_BottleAllocation";
            //this.btnCopybottles_BottleAllocation.ImageName = "Action_Copy_CellValue";
            this.btnAssignbottles_BottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnAssignbottles_BottleAllocation.ToolTip = null;
            this.btnAssignbottles_BottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnAssignbottles_BottleAllocation_Execute);
            // 
            // btnCopybottles_BottleAllocation
            // 
            this.btnCopybottles_BottleAllocation.Caption = "Copy Bottles";
            this.btnCopybottles_BottleAllocation.ConfirmationMessage = null;
            this.btnCopybottles_BottleAllocation.Id = "btnCopybottles_BottleAllocation";
            //this.btnCopybottles_BottleAllocation.ImageName = "Action_Copy_CellValue";
            this.btnCopybottles_BottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnCopybottles_BottleAllocation.ToolTip = null;
            this.btnCopybottles_BottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnCopybottles_BottleAllocation_Execute);
            // 
            // btnResetbottles_BottleAllocation
            // 
            this.btnResetbottles_BottleAllocation.Caption = "Reset Bottles";
            this.btnResetbottles_BottleAllocation.ConfirmationMessage = null;
            this.btnResetbottles_BottleAllocation.Id = "btnResetbottles_BottleAllocation";
            this.btnResetbottles_BottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnResetbottles_BottleAllocation.ToolTip = null;
            this.btnResetbottles_BottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnResetbottles_BottleAllocation_Execute);
            // 
            // SampleRegistrationBottleAllocationViewController
            // 
            //

            //
            //QtyOk
            //
            this.QtyOk.Caption = "Ok";
            this.QtyOk.ConfirmationMessage = null;
            this.QtyOk.Id = "QtyOkayId";
            this.QtyOk.Category = "catqtyok";
            this.QtyOk.ToolTip = null;
            this.QtyOk.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QtyOk_Execute);
            //
            //QtyReset
            //
            this.QtyReset.Caption = "Reset";
            this.QtyReset.ConfirmationMessage = null;
            this.QtyReset.Id = "QtyResetId";
            this.QtyReset.Category = "CatQtyReset";
            this.QtyReset.ToolTip = null;
            this.QtyReset.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QtyReset_Execute);

            this.Actions.Add(this.btnAssignbottles_BottleAllocation);
            this.Actions.Add(this.btnCopybottles_BottleAllocation);
            this.Actions.Add(this.btnResetbottles_BottleAllocation);
            this.Actions.Add(this.QtyOk);
            this.Actions.Add(this.QtyReset);
        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction QtyReset;
        private DevExpress.ExpressApp.Actions.SimpleAction QtyOk;
        private DevExpress.ExpressApp.Actions.SimpleAction btnAssignbottles_BottleAllocation;
        private DevExpress.ExpressApp.Actions.SimpleAction btnCopybottles_BottleAllocation;
        private DevExpress.ExpressApp.Actions.SimpleAction btnResetbottles_BottleAllocation;        
    }
}
