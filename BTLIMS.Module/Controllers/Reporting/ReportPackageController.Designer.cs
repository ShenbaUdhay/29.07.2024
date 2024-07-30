using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Reporting
{
    partial class ReportPackageController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

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
            this.newReportPackageAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            //this.addReportToPackageAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            //this.editReportPackageAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.saveReportPackageAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.deleteReportPackageAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.newReportAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            //this.RemoveReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.components = new System.ComponentModel.Container();
            this.AddNewReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Removereports = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.EditReport = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // AddNewReport
            // 
            this.AddNewReport.Caption = "Add ";
            this.AddNewReport.ConfirmationMessage = null;
            this.AddNewReport.Id = "AddNewReport";
            this.AddNewReport.ImageName = "Add.png";
            this.AddNewReport.ToolTip = null;
            this.AddNewReport.Category = "Edit";
            this.AddNewReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddNewReport_Execute);

            
            // 
            // newReportPackageAction
            // 
            this.newReportPackageAction.AcceptButtonCaption = null;
            this.newReportPackageAction.CancelButtonCaption = null;
            this.newReportPackageAction.Caption = "New Package";
            this.newReportPackageAction.Category = "Edit";
            this.newReportPackageAction.ConfirmationMessage = null;
            this.newReportPackageAction.Id = "newReportPackageAction";
            this.newReportPackageAction.ToolTip = null;
            this.newReportPackageAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.newReportPackageAction_CustomizePopupWindowParams);
            this.newReportPackageAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.newReportPackageAction_Execute);
            this.newReportPackageAction.Cancel += new System.EventHandler(this.NewReportAction_Cancel);
            // 
            // addReportToPackageAction
            // 
            //this.addReportToPackageAction.AcceptButtonCaption = null;
            //this.addReportToPackageAction.CancelButtonCaption = null;
            //this.addReportToPackageAction.Caption = "Add Report";
            //this.addReportToPackageAction.Category = "Edit";
            //this.addReportToPackageAction.ConfirmationMessage = null;
            //this.addReportToPackageAction.Id = "addReportToPackageAction";
            //this.addReportToPackageAction.ToolTip = null;
            //this.addReportToPackageAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.addReportToPackageAction_CustomizePopupWindowParams);
            //this.addReportToPackageAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.addReportToPackageAction_Execute);
            //// 
            // editReportPackageAction
            // 
            //this.editReportPackageAction.Caption = "Edit";
            //this.editReportPackageAction.Category = "RecordEdit";
            //this.editReportPackageAction.ConfirmationMessage = null;
            //this.editReportPackageAction.Id = "editReportPackageAction";
            //this.editReportPackageAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            //this.editReportPackageAction.ToolTip = null;
            //this.editReportPackageAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.editReportPackageAction_Execute);
            // 
            // saveReportPackageAction
            // 
            this.saveReportPackageAction.Caption = "Save";
            this.saveReportPackageAction.Category = "Edit";
            this.saveReportPackageAction.ConfirmationMessage = null;
            this.saveReportPackageAction.Id = "saveReportPackageAction";
            this.saveReportPackageAction.ToolTip = null;
            this.saveReportPackageAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.saveReportPackageAction_Execute);
            // 
            // deleteReportPackageAction
            // 
            this.deleteReportPackageAction.Caption = "Delete";
            this.deleteReportPackageAction.Category = "Edit";
            this.deleteReportPackageAction.ConfirmationMessage = null;
            this.deleteReportPackageAction.Id = "deleteReportPackageAction";
            this.deleteReportPackageAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.deleteReportPackageAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.deleteReportPackageAction_Execute);

            // RemoveReportAction

            //this.RemoveReportAction.Caption = "Remove";
            //this.RemoveReportAction.Category = "View";
            //this.RemoveReportAction.ConfirmationMessage = null;
            //this.RemoveReportAction.Id = "RemoveReportAction";
            //this.RemoveReportAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            //this.RemoveReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.deleteReport_Execute);


            // newReportAction
            // 
            //this.newReportAction.AcceptButtonCaption = null;
            //this.newReportAction.CancelButtonCaption = null;
            //this.newReportAction.Caption = "Add";
            //this.newReportAction.Category = "Edit";
            //this.newReportAction.ImageName = "Add.png";
            //this.newReportAction.ConfirmationMessage = null;
            //this.newReportAction.Id = "newReportAction";
            //this.newReportAction.ToolTip = null;
            //this.newReportAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.newReportAction_CustomizePopupWindowParams);
            //this.newReportAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.newReportAction_Execute);
            // 
            // Removereports
            // 
            this.Removereports.Caption = "Remove";
            this.Removereports.ConfirmationMessage = null;
            this.Removereports.Category = "Edit";
            this.Removereports.Id = "Removereports";
            this.Removereports.ToolTip = null;
            this.Removereports.ImageName = "Remove.png";
            this.Removereports.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Removereports_Execute);
            // 
            // EditReport
            // 
            this.EditReport.AcceptButtonCaption = null;
            this.EditReport.CancelButtonCaption = null;
            this.EditReport.Caption = "Edit";
            this.EditReport.Category = "RecordEdit";
            this.EditReport.ImageName = "Action_Edit";
            this.EditReport.ConfirmationMessage = null;
            this.EditReport.Id = "EditReportPack";
            this.EditReport.TargetViewId= "ReportPackage_ListView_Copy";
            this.EditReport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.EditReport.ToolTip = null;
            this.EditReport.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.EditReport_CustomizePopupWindowParams);
            this.EditReport.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.EditReport_Execute);
            // 
            // ReportPackageController
            // 
            this.Actions.Add(this.newReportPackageAction);
            //this.Actions.Add(this.addReportToPackageAction);
            //this.Actions.Add(this.editReportPackageAction);
            this.Actions.Add(this.saveReportPackageAction);
            this.Actions.Add(this.deleteReportPackageAction);
            //this.Actions.Add(this.newReportAction);
            //this.Actions.Add(this.RemoveReportAction);
            this.Actions.Add(this.AddNewReport);
            this.Actions.Add(this.Removereports);
            this.Actions.Add(this.EditReport);

        }

     
        #endregion
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction newReportPackageAction;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction addReportToPackageAction;
        //private DevExpress.ExpressApp.Actions.SimpleAction editReportPackageAction;
        private DevExpress.ExpressApp.Actions.SimpleAction saveReportPackageAction;
        private DevExpress.ExpressApp.Actions.SimpleAction deleteReportPackageAction;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction newReportAction;
       //private DevExpress.ExpressApp.Actions.SimpleAction RemoveReportAction;

    }
}
