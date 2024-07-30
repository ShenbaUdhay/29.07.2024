
using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Settings.TestPrice
{
    partial class TestPriceViewController
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
            this.PriceSaveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Add_Btn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Remove_Btn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PriceCancelAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Reactivate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PriceSaveAction
            // 
            this.PriceSaveAction.Caption = "Save";
            this.PriceSaveAction.ConfirmationMessage = null;
            this.PriceSaveAction.Id = "PriceSaveAction";
            this.PriceSaveAction.ImageName = "Save_16x16";
            this.PriceSaveAction.Category = "Save";
            this.PriceSaveAction.ToolTip = null;
            this.PriceSaveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PriceSaveAction_Execute);
            // 
            // Add_Btn
            // 
            this.Add_Btn.Caption = "Add";
            this.Add_Btn.Category = "RecordEdit";
            this.Add_Btn.ConfirmationMessage = null;
            this.Add_Btn.Id = "Add_Btn";
            this.Add_Btn.ImageName = "Add";
            this.Add_Btn.ToolTip = null;
            this.Add_Btn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Add_Btn_Execute);
            // 
            // Remove_Btn
            // 
            this.Remove_Btn.Caption = "Remove";
            this.Remove_Btn.Category = "RecordEdit";
            this.Remove_Btn.ConfirmationMessage = null;
            this.Remove_Btn.Id = "Remove_Btn";
            this.Remove_Btn.ImageName = "Remove";
            this.Remove_Btn.ToolTip = null;
            this.Remove_Btn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Remove_Btn_Execute);
            // 
            // Cancel
            // 
            this.PriceCancelAction.Caption = "Cancel";
            this.PriceCancelAction.Category = "Save";
            this.PriceCancelAction.ConfirmationMessage = null;
            this.PriceCancelAction.Id = "PriceCancelAction";
            this.PriceCancelAction.ImageName = "State_Validation_Invalid";
            this.PriceCancelAction.ToolTip = null;
            this.PriceCancelAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Cancel_Btn_Execute);
            // 
            // Reactivate
            // 
            this.Reactivate.Caption = "Reactivate";
            //this.Reactivate.Category = "View";
            this.Reactivate.ConfirmationMessage = "Do you want to reactivate this Item Charge ?";
            this.Reactivate.Id = "ItemchargeReactivated";
            this.Reactivate.ImageName = "Action_Workflow_Activate";
            this.Reactivate.ToolTip = null;
            this.Reactivate.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.Reactivate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReactivateRegistration_Execute);
            // 
            // TestPriceViewController
            // 
            this.Actions.Add(this.PriceSaveAction);
            this.Actions.Add(this.Add_Btn);
            this.Actions.Add(this.Remove_Btn);
            this.Actions.Add(this.PriceCancelAction);
            this.Actions.Add(this.Reactivate);
            //this.Activated += new System.EventHandler(this.TestSurchargePriceViewController_Activated);

        }


        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PriceSaveAction;
        private DevExpress.ExpressApp.Actions.SimpleAction Add_Btn;
        private DevExpress.ExpressApp.Actions.SimpleAction Remove_Btn;
        private DevExpress.ExpressApp.Actions.SimpleAction PriceCancelAction;
        private DevExpress.ExpressApp.Actions.SimpleAction Reactivate;
    }
}
