
namespace LDM.Module.Controllers.Settings.Quotes
{
    partial class QuoteViewController
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
            this.QuoteSubmit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QuoteReactive = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QuoteReview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QuoteRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QuotesHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QuotesDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.QuotePreviewDC = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.QuoteSaveAs = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // QuotePreviewDC
            // 
            this.QuotePreviewDC.Caption = "...";
            this.QuotePreviewDC.Category = "RecordEdit";
            this.QuotePreviewDC.ConfirmationMessage = null;
            this.QuotePreviewDC.Id = "QuotePreviewDC";
            this.QuotePreviewDC.TargetViewId = "CRMQuotes_ListView_DataCenter";
            this.QuotePreviewDC.ToolTip = "Preview";
            this.QuotePreviewDC.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.QuotePreviewDC.Execute+= new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Btnqrpreview_Execute);
            // 
            // QuoteSaveAs
            // 
            this.QuoteSaveAs.Caption = "Save As";
            this.QuoteSaveAs.ConfirmationMessage = null;
            this.QuoteSaveAs.Id = "QuoteSaveAs";
            this.QuoteSaveAs.ImageName = "SaveAs_16x16";
            this.QuoteSaveAs.ToolTip = "Save As new Quote";
            this.QuoteSaveAs.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.QuoteSaveAs.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QuoteSaveAs_Execute);            
            //  
            // QuoteSubmit
            // 
            this.QuoteSubmit.Caption = "Submit";
            this.QuoteSubmit.ConfirmationMessage = null;
            this.QuoteSubmit.Id = "QuoteSubmit";
            this.QuoteSubmit.ImageName = "Submit_image";
            this.QuoteSubmit.ToolTip = null;
            this.QuoteSubmit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QuoteSubmit_Execute);
            // 
            // QuoteReactive
            // 
            this.QuoteReactive.Caption = "Reactive";
            this.QuoteReactive.ConfirmationMessage = null;
            this.QuoteReactive.Id = "QuoteReactive";
            this.QuoteReactive.ToolTip = null;
            this.QuoteReactive.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QuoteReactive_Execute);
            // 
            // QuoteReview
            // 
            this.QuoteReview.Caption = "Review";
            this.QuoteReview.ConfirmationMessage = null;
            this.QuoteReview.Id = "QuoteReview";
            this.QuoteReview.ToolTip = null;
            this.QuoteReview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QuoteReview_Execute);
            // 
            // QuoteRollback
            // 
            this.QuoteRollback.Caption = "Rollback";
            this.QuoteRollback.ConfirmationMessage = null;
            this.QuoteRollback.Id = "QuoteRollback";
            this.QuoteRollback.ToolTip = null;
            this.QuoteRollback.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QuoteRollback_Execute);
            // 
            // QuotesHistory
            // 
            this.QuotesHistory.Caption = "History";
            this.QuotesHistory.ConfirmationMessage = null;
            this.QuotesHistory.Id = "QuotesHistory";
            this.QuotesHistory.ToolTip = null;
            this.QuotesHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QuotesHistory_Execute);
            // 
            // QuotesDateFilter
            // 
            this.QuotesDateFilter.Caption = "Date Filter";
            this.QuotesDateFilter.ConfirmationMessage = null;
            this.QuotesDateFilter.Id = "QuotesDateFilter";
            choiceActionItem1.Caption = "All";
            choiceActionItem1.Id = "All";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "1M";
            choiceActionItem2.Id = "1M";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "3M";
            choiceActionItem3.Id = "3M";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "6M";
            choiceActionItem4.Id = "6M";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "1Y";
            choiceActionItem5.Id = "1Y";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.QuotesDateFilter.Items.Add(choiceActionItem1);
            this.QuotesDateFilter.Items.Add(choiceActionItem2);
            this.QuotesDateFilter.Items.Add(choiceActionItem3);
            this.QuotesDateFilter.Items.Add(choiceActionItem4);
            this.QuotesDateFilter.Items.Add(choiceActionItem5);
            this.QuotesDateFilter.ToolTip = null;
            this.QuotesDateFilter.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.QuotesDateFilter_Execute);
            // 
            // QuoteViewController
            // 
            this.Actions.Add(this.QuoteSubmit);
            this.Actions.Add(this.QuoteReactive);
            this.Actions.Add(this.QuoteReview);
            this.Actions.Add(this.QuoteRollback);
            this.Actions.Add(this.QuotesHistory);
            this.Actions.Add(this.QuotesDateFilter);
            this.Actions.Add(this.QuotePreviewDC);
            this.Actions.Add(this.QuoteSaveAs);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction QuotePreviewDC;
        private DevExpress.ExpressApp.Actions.SimpleAction QuoteSubmit;
        private DevExpress.ExpressApp.Actions.SimpleAction QuoteReactive;
        private DevExpress.ExpressApp.Actions.SimpleAction QuoteReview;
        private DevExpress.ExpressApp.Actions.SimpleAction QuoteRollback;
        private DevExpress.ExpressApp.Actions.SimpleAction QuotesHistory;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction QuotesDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction QuoteSaveAs;
    }
}
