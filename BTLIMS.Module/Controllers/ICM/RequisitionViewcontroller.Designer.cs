using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.ICM
{
    partial class RequisitionViewcontroller
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();

            this.Approve = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Review = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Receive = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RequisitionQuerPanel = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.RequistionNew = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.RequisitionSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RequisitionAddItem = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.Requisitiondelete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Requisitionview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RequisitionDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
           // this.RequisitionRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
           // this.CancelledItemRollback = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 

            // RequisitionDateFilter
            // 
            this.RequisitionDateFilter.Caption = "Date Filter";
            this.RequisitionDateFilter.Category = "View";
            this.RequisitionDateFilter.ConfirmationMessage = null;
            this.RequisitionDateFilter.Id = "RequisitionDateFilter";
            choiceActionItem1.Caption = "1M";
            choiceActionItem1.Id = "1M";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "3M";
            choiceActionItem2.Id = "3M";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "6M";
            choiceActionItem3.Id = "6M";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "1Y";
            choiceActionItem4.Id = "1Y";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "2Y";
            choiceActionItem5.Id = "2Y";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "5Y";
            choiceActionItem6.Id = "5Y";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "ALL";
            choiceActionItem7.Id = "ALL";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            this.RequisitionDateFilter.Items.Add(choiceActionItem1);
            this.RequisitionDateFilter.Items.Add(choiceActionItem2);
            this.RequisitionDateFilter.Items.Add(choiceActionItem3);
            this.RequisitionDateFilter.Items.Add(choiceActionItem4);
            this.RequisitionDateFilter.Items.Add(choiceActionItem5);
            this.RequisitionDateFilter.Items.Add(choiceActionItem6);
            this.RequisitionDateFilter.Items.Add(choiceActionItem7);

            this.RequisitionDateFilter.ToolTip = null;
           // this.RequisitionDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.RequisitionDateFilter_Execute);
            // 

            // RequisitionRollback
            // 
            //this.RequisitionRollback.Caption = "Rollback";
            //this.RequisitionRollback.Category = "View";
            //this.RequisitionRollback.ConfirmationMessage = null;
            //this.RequisitionRollback.Id = "RequisitionRollback";
            //this.RequisitionRollback.ToolTip = null;
            //this.RequisitionRollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RequisitionRollback_Execute);
            // 

            // CancelledItemRollback
            // 
            //this.CancelledItemRollback.AcceptButtonCaption = null;
            //this.CancelledItemRollback.CancelButtonCaption = null;
            //this.CancelledItemRollback.Caption = "RollBack";
            //this.CancelledItemRollback.Category = "View";
            //this.CancelledItemRollback.ConfirmationMessage = null;
            //this.CancelledItemRollback.Id = "ICMRollBackAction";
            // 
            // Approve
            // 
            // 

            // Requisitionview
            // 
            this.Requisitionview.Caption = "History";
            this.Requisitionview.ImageName = "Action_Search";
            this.Requisitionview.Category = "View";
            this.Requisitionview.ConfirmationMessage = null;
            this.Requisitionview.Id = "Requisitionview";
            this.Requisitionview.ToolTip = null;
            this.Requisitionview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Requisitionview_Execute);
            // 

            // Approve
            // 
            this.Approve.Caption = "Approve";
            this.Approve.ConfirmationMessage = null;
            this.Approve.Id = "Approve";
            this.Approve.TargetViewId = "Requisition_ListView_Approve";
            this.Approve.ToolTip = null;
            this.Approve.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Approve_Execute);
            // 
            // Review
            // 
            this.Review.Caption = "Review";
            this.Review.ConfirmationMessage = null;
            this.Review.Id = "Review";
            this.Review.TargetViewId = "Requisition_ListView_Review";
            this.Review.ToolTip = null;
            this.Review.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Review_Execute);
            // 
            // Receive
            // 
            this.Receive.Caption = "Receive";
            this.Receive.ConfirmationMessage = null;
            this.Receive.Id = "Receive";
            this.Receive.ToolTip = null;
            // 
            // RequisitionQuerPanel
            // 
            this.RequisitionQuerPanel.AcceptButtonCaption = null;
            this.RequisitionQuerPanel.CancelButtonCaption = null;
            this.RequisitionQuerPanel.Caption = "Requisition Query Panel";
            this.RequisitionQuerPanel.ConfirmationMessage = null;
            this.RequisitionQuerPanel.Id = "RequisitionQueryPanel";
            this.RequisitionQuerPanel.ToolTip = null;
            this.RequisitionQuerPanel.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.RequisitionQuerPanel_CustomizePopupWindowParams);
            this.RequisitionQuerPanel.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.RequisitionQuerPanel_Execute);
            // 
            // RequistionNew
            // 
            this.RequistionNew.AcceptButtonCaption = null;
            this.RequistionNew.CancelButtonCaption = null;
            this.RequistionNew.Caption = "New";
            this.RequistionNew.Category = "ObjectsCreation";
            this.RequistionNew.ConfirmationMessage = null;
            this.RequistionNew.Id = "RequistionNew";
            this.RequistionNew.ToolTip = null;
            this.RequistionNew.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.RequistionNew_CustomizePopupWindowParams);
            this.RequistionNew.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.RequistionNew_Execute);
            // 
            // RequisitionSave
            // 
            this.RequisitionSave.Caption = "Save";
            this.RequistionNew.Category = "RecordEdit";
            this.RequisitionSave.ConfirmationMessage = null;
            this.RequisitionSave.Id = "RequisitionSave";
            this.RequisitionSave.ToolTip = null;
            // 
            // RequisitionAddItem
            // 
            this.RequisitionAddItem.AcceptButtonCaption = null;
            this.RequisitionAddItem.CancelButtonCaption = null;
            //this.RequisitionAddItem.Caption = "Add Item";
            this.RequisitionAddItem.Caption = "New";
            this.RequisitionAddItem.Category = "ObjectsCreation";
            this.RequisitionAddItem.ConfirmationMessage = null;
            this.RequisitionAddItem.Id = "RequisitionAddItem";
            this.RequisitionAddItem.ToolTip = null;
            this.RequisitionAddItem.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.RequisitionAddItem_CustomizePopupWindowParams);
            this.RequisitionAddItem.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.RequisitionAddItem_Execute);
            // 
            // Requisitiondelete
            // 
            this.Requisitiondelete.Caption = "Delete";
            this.Requisitiondelete.Category = "RecordEdit";
            this.Requisitiondelete.ConfirmationMessage = null;
            this.Requisitiondelete.Id = "Requisitiondelete";
            this.Requisitiondelete.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Requisitiondelete.ToolTip = null;
            this.Requisitiondelete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Requisitiondelete_Execute);
            // 
            // RequisitionViewcontroller
            // 
            this.Actions.Add(this.Approve);
            this.Actions.Add(this.Review);
            this.Actions.Add(this.Receive);
            this.Actions.Add(this.RequisitionQuerPanel);
            this.Actions.Add(this.RequistionNew);
            this.Actions.Add(this.RequisitionSave);
            this.Actions.Add(this.RequisitionAddItem);
            this.Actions.Add(this.Requisitiondelete);
            this.Actions.Add(this.Requisitionview);
            this.Actions.Add(this.RequisitionDateFilter);
           // this.Actions.Add(this.RequisitionRollback);
           // this.Actions.Add(this.CancelledItemRollback);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction Approve;
        private DevExpress.ExpressApp.Actions.SimpleAction Review;
        private DevExpress.ExpressApp.Actions.SimpleAction Receive;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction RequisitionQuerPanel;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction RequistionNew;
        private DevExpress.ExpressApp.Actions.SimpleAction RequisitionSave;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction RequisitionAddItem;
        private DevExpress.ExpressApp.Actions.SimpleAction Requisitiondelete;
        private DevExpress.ExpressApp.Actions.SimpleAction Requisitionview;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction RequisitionDateFilter;
       //private DevExpress.ExpressApp.Actions.SimpleAction RequisitionRollback;
       // private DevExpress.ExpressApp.Actions.PopupWindowShowAction CancelledItemRollback;
    }
}
