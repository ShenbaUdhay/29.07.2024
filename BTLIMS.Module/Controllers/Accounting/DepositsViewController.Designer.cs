
using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.Accounting.Receivables
{
    partial class DepositsViewController
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
            this.DepositHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DepositRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DepositEDDDetail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DepositHistory
            // 
            this.DepositHistory.Caption = "History";
            this.DepositHistory.Category = "View";
            this.DepositHistory.ConfirmationMessage = null;
            this.DepositHistory.Id = "DepositHistory";
            this.DepositHistory.ImageName = "History2_16x16";
            this.DepositHistory.Execute += HistoryDeposit_Execute;

            // 
            // Rollback
            // 
            this.DepositRollback.Caption = "Rollback";
            this.DepositRollback.Category = "View";
            this.DepositRollback.ConfirmationMessage = null;
            this.DepositRollback.Id = "DepositRollback";
            this.DepositRollback.ImageName = "icons8-undo-16";
            this.DepositRollback.Execute += Rollback_Execute;
            // 
            // Rollback
            // 
            this.DepositEDDDetail.Caption = "EDDDetail";
            this.DepositEDDDetail.Category = "ListView";
            this.DepositEDDDetail.ConfirmationMessage = null;
            this.DepositEDDDetail.Id = "DepositEDDDetail";
            this.DepositEDDDetail.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //this.DepositEDDDetail.ImageName = "icons8-undo-16";
            this.DepositEDDDetail.Execute += EDDDetail_Excecute;
            // 
            // DepositViewController
            // 
            this.Actions.Add(this.DepositHistory);
            this.Actions.Add(this.DepositRollback);
            this.Actions.Add(this.DepositEDDDetail);
        }

        private DevExpress.ExpressApp.Actions.SimpleAction DepositHistory;
        private DevExpress.ExpressApp.Actions.SimpleAction DepositRollback;
        private DevExpress.ExpressApp.Actions.SimpleAction DepositEDDDetail;
        #endregion
    }
}
