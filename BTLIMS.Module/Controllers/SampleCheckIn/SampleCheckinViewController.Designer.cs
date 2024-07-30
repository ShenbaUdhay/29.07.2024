using System;
using DevExpress.ExpressApp.Actions;

namespace BTLIMS.Module.Controllers.SampleCheckIn
{
    partial class SampleCheckinUploadImageController
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
            this.AddSCItemCharge = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RemoveSCItemCharge = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SampleCheckinUploadImageController
            // 
            this.ViewControlsCreated += new System.EventHandler(this.SampleCheckinUploadImageController_ViewControlsCreated);

            // 
            // Add
            // 
            this.AddSCItemCharge.Caption = "Add";
            this.AddSCItemCharge.Category = "Edit";
            this.AddSCItemCharge.ConfirmationMessage = null;
            this.AddSCItemCharge.Id = "AddSCItemCharge";
            this.AddSCItemCharge.ImageName = "Add_16x16.png";
            this.AddSCItemCharge.Execute += AddItemCharge_Exceute;
            // 
            // Add
            // 
            this.RemoveSCItemCharge.Caption = "Remove";
            this.RemoveSCItemCharge.Category = "View";
            this.RemoveSCItemCharge.ConfirmationMessage = null;
            this.RemoveSCItemCharge.Id = "RemoveSCItemCharge";
            this.RemoveSCItemCharge.ImageName = "Remove.png";
            //this.RemoveSCItemCharge.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.RemoveSCItemCharge.Execute += RemoveItemCharge_Exceute;
            // 
            // InvoicingViewController
            // 
            this.Actions.Add(this.AddSCItemCharge);
            this.Actions.Add(this.RemoveSCItemCharge);
        }

      

        private DevExpress.ExpressApp.Actions.SimpleAction AddSCItemCharge;
        private DevExpress.ExpressApp.Actions.SimpleAction RemoveSCItemCharge;
        #endregion
    }
}
