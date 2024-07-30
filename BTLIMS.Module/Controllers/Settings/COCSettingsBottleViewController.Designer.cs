
namespace Labmaster.Module.Controllers.Settings
{
    partial class COCSettingsBottleViewController
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
            //this.COCBottleSetup = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.COCBottleDeleteSetup = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.COCSamplesTestRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.COCCopyBottleSet = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //
            // 
            // btnCopybottles_BottleAllocation
            // 
            this.btnAssignbottles_BottleAllocation.Caption = "Assign Bottles";
            this.btnAssignbottles_BottleAllocation.ConfirmationMessage = null;
            this.btnAssignbottles_BottleAllocation.Id = "btnAssignbottles_Bottle";
            //this.btnCopybottles_BottleAllocation.ImageName = "Action_Copy_CellValue";
            this.btnAssignbottles_BottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnAssignbottles_BottleAllocation.ToolTip = null;
            this.btnAssignbottles_BottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnAssignbottles_BottleAllocation_Execute);
            // 
            // btnCopybottles_BottleAllocation
            // 
            this.btnCopybottles_BottleAllocation.Caption = "Copy Bottles";
            this.btnCopybottles_BottleAllocation.ConfirmationMessage = null;
            this.btnCopybottles_BottleAllocation.Id = "btnCopybottles_Bottle";
            //this.btnCopybottles_BottleAllocation.ImageName = "Action_Copy_CellValue";
            this.btnCopybottles_BottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnCopybottles_BottleAllocation.ToolTip = null;
            this.btnCopybottles_BottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnCopybottles_BottleAllocation_Execute);
            // 
            // btnResetbottles_BottleAllocation
            // 
            this.btnResetbottles_BottleAllocation.Caption = "Reset Bottles";
            this.btnResetbottles_BottleAllocation.ConfirmationMessage = null;
            this.btnResetbottles_BottleAllocation.Id = "btnResetbottles_Bottle";
            this.btnResetbottles_BottleAllocation.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.btnResetbottles_BottleAllocation.ToolTip = null;
            this.btnResetbottles_BottleAllocation.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnResetbottles_BottleAllocation_Execute);

            // COCBottleSetup
            // 
            //this.COCBottleSetup.Caption = "Bottle";
            //this.COCBottleSetup.Category = "RecordEdit";
            //this.COCBottleSetup.ConfirmationMessage = null;
            //this.COCBottleSetup.Id = "COCBottleSetup";
            //this.COCBottleSetup.ToolTip = null;
            //this.COCBottleSetup.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COCBottleSetup_Execute);
            // 
            // COCBottleDeleteSetup
            // 
            this.COCBottleDeleteSetup.Caption = "Delete";
            this.COCBottleDeleteSetup.Category = "ListView";
            this.COCBottleDeleteSetup.ConfirmationMessage = null;
            this.COCBottleDeleteSetup.Id = "COCBottleDeleteSetup";
            this.COCBottleDeleteSetup.ToolTip = null;
            this.COCBottleDeleteSetup.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COCBottleDeleteSetup_Execute);
            // 
            // COCSamplesTestRemove
            // 
            //this.COCSamplesTestRemove.Caption = "Link";
            //this.COCSamplesTestRemove.Category = "RecordEdit";
            //this.COCSamplesTestRemove.ConfirmationMessage = null;
            //this.COCSamplesTestRemove.Id = "COCSamplesTestRemove";
            //this.COCSamplesTestRemove.ToolTip = null;
            //this.COCSamplesTestRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COCSamplesTestRemove_Execute);
            // 
            // COCCopyBottleSet
            // 
            this.COCCopyBottleSet.Caption = "Copy Bottle set";
            this.COCCopyBottleSet.Category = "RecordEdit";
            this.COCCopyBottleSet.ConfirmationMessage = null;
            this.COCCopyBottleSet.Id = "COCCopyBottleSet";
            this.COCCopyBottleSet.ToolTip = null;
            this.COCCopyBottleSet.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COCCopyBottleSet_Execute);
            // 
            // COCSettingsBottleViewController
            // 
            //this.Actions.Add(this.COCBottleSetup);
            this.Actions.Add(this.COCBottleDeleteSetup);
            //this.Actions.Add(this.COCSamplesTestRemove);
            this.Actions.Add(this.COCCopyBottleSet);
            this.Actions.Add(this.btnAssignbottles_BottleAllocation);
            this.Actions.Add(this.btnCopybottles_BottleAllocation);
            this.Actions.Add(this.btnResetbottles_BottleAllocation);

        }

        #endregion

        //private DevExpress.ExpressApp.Actions.SimpleAction COCBottleSetup;
        private DevExpress.ExpressApp.Actions.SimpleAction COCBottleDeleteSetup;
        //private DevExpress.ExpressApp.Actions.SimpleAction COCSamplesTestRemove;
        private DevExpress.ExpressApp.Actions.SimpleAction COCCopyBottleSet;
        private DevExpress.ExpressApp.Actions.SimpleAction btnAssignbottles_BottleAllocation;
        private DevExpress.ExpressApp.Actions.SimpleAction btnCopybottles_BottleAllocation;
        private DevExpress.ExpressApp.Actions.SimpleAction btnResetbottles_BottleAllocation;
    }
}
