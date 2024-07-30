namespace XCRM.Module.Controllers
{
    partial class ProspectsController
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
            this.opportunityFilterChoiceAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.popupImportFile = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ReactivateOpportunity = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Rollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // opportunityFilterChoiceAction
            // 
            this.opportunityFilterChoiceAction.Caption = "opportunity Filter Choice Action";
            this.opportunityFilterChoiceAction.ConfirmationMessage = null;
            this.opportunityFilterChoiceAction.Id = "ProspectsFilterChoiceAction";
            this.opportunityFilterChoiceAction.ToolTip = null;
            // 
            // popupImportFile
            // 
            this.popupImportFile.AcceptButtonCaption = null;
            this.popupImportFile.CancelButtonCaption = null;
            this.popupImportFile.Caption = "Import";
            this.popupImportFile.ConfirmationMessage = null;
            this.popupImportFile.Id = "popupImportFile";
            this.popupImportFile.ToolTip = null;
            this.popupImportFile.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupImportFile_CustomizePopupWindowParams);
            this.popupImportFile.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupImportFile_Execute);
            // 
            // ReactivateOpportunity
            // 
            this.ReactivateOpportunity.Caption = "Reactivate Prospects";
            this.ReactivateOpportunity.ConfirmationMessage = null;
            this.ReactivateOpportunity.Id = "ReactivateProspects";
            this.ReactivateOpportunity.ToolTip = null;
            this.ReactivateOpportunity.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ReactivateProspects_Execute);
            // 
            // Rollback
            // 
            this.Rollback.Caption = "Rollback";
            this.Rollback.ConfirmationMessage = null;
            this.Rollback.Id = "RollbackId";
            this.Rollback.ImageName = "Action_Cancel";
            this.Rollback.ToolTip = null;
            this.Rollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Rollback_Execute);
            // 
            // ProspectsController
            // 
            this.Actions.Add(this.opportunityFilterChoiceAction);
            this.Actions.Add(this.popupImportFile);
            this.Actions.Add(this.ReactivateOpportunity);
            this.Actions.Add(this.Rollback);

        }

        #endregion

        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction closeProspects;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction opportunityFilterChoiceAction;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupImportFile;
        private DevExpress.ExpressApp.Actions.SimpleAction ReactivateOpportunity;
        private DevExpress.ExpressApp.Actions.SimpleAction Rollback;
    }
}
