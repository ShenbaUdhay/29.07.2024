
using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.Invoices
{
    partial class InvoicingViewController
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
            this.InvoiceJobIDSelect = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InvoiceSubmit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Invoicehistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InvoiceViewDateFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.InvoiceReview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InvoicePreview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InvoicePreviewDC = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.BatchInvoicing = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PreInvoicingReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InvoiceRollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.InvoiceReviewHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Export = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Add = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Remove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ExportToQuickbook = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);

            //this.InvoiceEDDDetails = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // InvoiceJobIDSelect
            // 





            // 
            // Export
            // 
            this.Export.Caption = "Export to CSV";
            this.Export.ConfirmationMessage = null;
            this.Export.Category ="View";
            this.Export.Id = "ExportToCSV";
            this.Export.ToolTip = null;
            this.Export.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Export_Execute);
            // 
            // InvoicingViewController
            // 
            this.InvoiceJobIDSelect.Caption = "Select";
            this.InvoiceJobIDSelect.Category = "InvoiceJobIDSelect";
            this.InvoiceJobIDSelect.ConfirmationMessage = null;
            this.InvoiceJobIDSelect.Id = "InvoiceJobIDSelect";
            //this.InvoiceJobIDSelect.ImageName = "import_data_16x16";
            this.InvoiceJobIDSelect.ToolTip = null;
            this.InvoiceJobIDSelect.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SelectJobID_Execute);
            // 
            // Submit
            // 
            this.InvoiceSubmit.Caption = "Submit";
            this.InvoiceSubmit.Category = "View";
            this.InvoiceSubmit.ConfirmationMessage = null;
            this.InvoiceSubmit.Id = "InvoiceSubmit";
            this.InvoiceSubmit.ImageName = "Submit_image";
            this.InvoiceSubmit.ToolTip = null;
            this.InvoiceSubmit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Submit_Execute);
            // 
            // History
            // 
            this.Invoicehistory.Caption = "History";
            this.Invoicehistory.Category = "View";
            this.Invoicehistory.ConfirmationMessage = null;
            this.Invoicehistory.Id = "Invoicehistory";
            this.Invoicehistory.ImageName = "History2_16x16";
            this.Invoicehistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.History_InvoiceView_Execute);
            // 
            // DateFilter
            // 
            this.InvoiceViewDateFilter.Caption = "Date Filter";
            this.InvoiceViewDateFilter.Category = "View";
            this.InvoiceViewDateFilter.ConfirmationMessage = null;
            this.InvoiceViewDateFilter.Id = "InvoiceViewDateFilter";
            //this.Invoicehistory.ImageName = "Action_Search";
            this.InvoiceViewDateFilter.SelectedItemChanged += InvoiceViewDateFilter_SelectedItemChanged;
            // 
            // Review
            // 
            this.InvoiceReview.Caption = "Review";
            this.InvoiceReview.Category = "View";
            this.InvoiceReview.ConfirmationMessage = null;
            this.InvoiceReview.Id = "InvoiceReview";
            this.InvoiceReview.ImageName = "BO_Validation";
            this.InvoiceReview.Execute += Review_Execute;
            // 
            // Review
            // 
            this.InvoicePreview.Caption = "Preview";
            this.InvoicePreview.Category = "RecordEdit";
            this.InvoicePreview.ConfirmationMessage = null;
            this.InvoicePreview.Id = "InvoicePreview";
            this.InvoicePreview.ImageName = "Action_Report_Object_Inplace_Preview";
            this.InvoicePreview.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.InvoicePreview.Execute += PreviewReport_Execute;
            // 
            // InvoicingViewController
            // 
            this.InvoicePreviewDC.Caption = "...";
            this.InvoicePreviewDC.Category = "RecordEdit";
            this.InvoicePreviewDC.ConfirmationMessage = null;
            this.InvoicePreviewDC.Id = "InvoicePreviewDC";
            this.InvoicePreviewDC.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.InvoicePreviewDC.Execute += PreviewReport_Execute;
            // 
            // BatchInvoicing
            // 
            this.BatchInvoicing.Caption = "Batch Invoicing";
            this.BatchInvoicing.Category = "View";
            this.BatchInvoicing.ConfirmationMessage = null;
            this.BatchInvoicing.Id = "BatchInvoicing";
            //this.BatchInvoicing.ImageName = "Action_Report_Object_Inplace_Preview";
            //this.BatchInvoicing.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.BatchInvoicing.Execute += BatchInvoicing_Execute;
            // 
            // PreInvoicing
            // 
            this.PreInvoicingReport.Caption = "Pre Invoice";
            this.PreInvoicingReport.Category = "Reports";
            this.PreInvoicingReport.ConfirmationMessage = null;
            this.PreInvoicingReport.Id = "PreInvoicingReport";
            this.PreInvoicingReport.ImageName = "BO_Report";
            this.PreInvoicingReport.Execute += PreInvoiceReport_Execute;
            // 
            // InvoicingViewController
            // 
            this.InvoiceRollback.Caption = "Rollback";
            this.InvoiceRollback.Category = "View";
            this.InvoiceRollback.ConfirmationMessage = null;
            this.InvoiceRollback.Id = "InvoiceRollback";
            this.InvoiceRollback.ImageName = "icons8-undo-16";
            this.InvoiceRollback.Execute += Rollback_Execute;
            // 
            // InvoiceReviewHistory
            // 
            this.InvoiceReviewHistory.Caption = "History";
            this.InvoiceReviewHistory.Category = "View";
            this.InvoiceReviewHistory.ConfirmationMessage = null;
            this.InvoiceReviewHistory.Id = "InvoiceReviewHistory";
            this.Invoicehistory.ImageName = "History2_16x16";
            this.InvoiceReviewHistory.Execute += HistoryReview_Execute;
            // 
            // AnalysisChargeAdd&ItemCharge
            // 
            this.Add.Caption = "Add";
            this.Add.Category = "ObjectsCreation";
            this.Add.ConfirmationMessage = null;
            this.Add.Id = "AddAC&IC";
            this.Add.ImageName = "Add.png";
            this.Add.Execute += Add_Execute;
            // 
            // RemoveAnalysisCharge&ItemCharge
            // 
            this.Remove.Caption = "Remove";
            this.Remove.Category = "ObjectsCreation";
            this.Remove.ConfirmationMessage = null;
            this.Remove.Id = "RemoveAC&IC";
            this.Remove.ImageName = "Remove.png";
            this.Remove.Execute += Remove_Execute;


            this.ExportToQuickbook.Caption = "Export To Quick Book";
            this.ExportToQuickbook.ConfirmationMessage = null;
            this.ExportToQuickbook.Id = "ExportToQuickBook";
            this.ExportToQuickbook.ImageName = "Action_Export_ToCSV";
            this.ExportToQuickbook.ToolTip = null;
            this.ExportToQuickbook.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportToQuickbook_Execute);
            // 
            // InvoicingViewController
            // 


            // 
            // EDDDetails
            // 
            //this.InvoiceEDDDetails.Caption = "EDDDetail";
            //this.InvoiceEDDDetails.Category = "ListView";
            //this.InvoiceEDDDetails.ConfirmationMessage = null;
            //this.InvoiceEDDDetails.Id = "InvoiceEDDDetails";
            //this.InvoiceEDDDetails.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //this.InvoiceEDDDetails.Execute += EDDDetails_Execute;
            // 
            // InvoicingViewController
            // 
            this.Actions.Add(this.InvoiceJobIDSelect);
            this.Actions.Add(this.InvoiceSubmit);
            this.Actions.Add(this.Invoicehistory);
            this.Actions.Add(this.InvoiceViewDateFilter);
            this.Actions.Add(this.InvoiceReview);
            this.Actions.Add(this.InvoicePreview);
            this.Actions.Add(this.InvoicePreviewDC);
            this.Actions.Add(this.BatchInvoicing);
            this.Actions.Add(this.PreInvoicingReport);
            this.Actions.Add(this.InvoiceRollback);
            this.Actions.Add(this.InvoiceReviewHistory);
            this.Actions.Add(this.Export);
            this.Actions.Add(this.Add);
            this.Actions.Add(this.Remove);
            this.Actions.Add(this.ExportToQuickbook);

            //this.Actions.Add(this.InvoiceEDDDetails);
        }

       
        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction InvoiceJobIDSelect;
        private DevExpress.ExpressApp.Actions.SimpleAction InvoiceSubmit;
        private DevExpress.ExpressApp.Actions.SimpleAction Invoicehistory;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction InvoiceViewDateFilter;
        private DevExpress.ExpressApp.Actions.SimpleAction InvoiceReview;
        private DevExpress.ExpressApp.Actions.SimpleAction InvoicePreview;
        private DevExpress.ExpressApp.Actions.SimpleAction InvoicePreviewDC;
        private DevExpress.ExpressApp.Actions.SimpleAction BatchInvoicing;
        private DevExpress.ExpressApp.Actions.SimpleAction PreInvoicingReport;
        private DevExpress.ExpressApp.Actions.SimpleAction InvoiceRollback;
        private DevExpress.ExpressApp.Actions.SimpleAction InvoiceReviewHistory;
        private DevExpress.ExpressApp.Actions.SimpleAction Add;
        private DevExpress.ExpressApp.Actions.SimpleAction Remove;
        //private DevExpress.ExpressApp.Actions.SimpleAction InvoiceEDDDetails;
    }
}
