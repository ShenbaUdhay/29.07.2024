using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Web.Controllers.SamplingProposal
{
    partial class SamplingAllocationViewController
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
            //this.AttachJobID = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.DeAttachJobID = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.COC_BarReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SampleLabel = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RTCTReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Vertical = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //// 
            //// 
            //// AttachJobID
            //// 
            //this.AttachJobID.Caption = "Attach Job ID";
            //this.AttachJobID.ConfirmationMessage = null;
            //this.AttachJobID.Id = "AttachJobID";
            //this.AttachJobID.ToolTip = null;
            //this.TargetViewId = "TaskSchedulerEventList_ListView_Copy";
            //this.AttachJobID.Category = "ObjectsCreation";
            //// 
            //// DeattachJobID
            //// 
            //this.DeAttachJobID.Caption = "Detach Job ID";
            //this.DeAttachJobID.ConfirmationMessage = null;
            //this.DeAttachJobID.Id = "Detach Job ID";
            //this.DeAttachJobID.ToolTip = null;
            //this.DeAttachJobID.Category = "ObjectsCreation";
             // 
            // COC_BarReport
            // 
            this.COC_BarReport.Caption = "COC Report";
            this.COC_BarReport.ConfirmationMessage = null;
            this.COC_BarReport.ImageName = "BO_Report";
            this.COC_BarReport.Id = "COC_Report";
            this.COC_BarReport.ToolTip = null;
            this.COC_BarReport.Category = "ObjectsCreation";
            this.COC_BarReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COC_BarReport_Execute);
            // 
            // SampleLabel
            // 
            this.SampleLabel.Caption = "Sample Label";
            this.SampleLabel.ConfirmationMessage = null;            
            this.SampleLabel.ImageName = "Barcode_16x16";
            this.SampleLabel.Id = "Samplelabel";
            this.SampleLabel.ToolTip = null;
            this.SampleLabel.Category = "ObjectsCreation";
            this.SampleLabel.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SampleLabel_Execute);
            // 
            // SamplingAllocationViewController
            // 


            this.RTCTReport.Caption = "RTCT Report";
            this.RTCTReport.ConfirmationMessage = null;
            this.RTCTReport.ImageName = "BO_Report";
            this.RTCTReport.Id = "RTCTReport";
            this.RTCTReport.ToolTip = null;
            this.RTCTReport.Category = "ObjectsCreation";
            this.RTCTReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RTCTReport_Execute);

            this.Vertical.Caption = "Barcode";
            this.Vertical.ConfirmationMessage = null;
            //this.BarcodeReport.ImageName = "BO_Report";
            this.Vertical.Id = "Vertical";
            this.Vertical.ToolTip = null;
            this.Vertical.ImageName = "barcode_Image";
            this.Vertical.Category = "ObjectsCreation";
            this.Vertical.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Vertical_Execute);

            //this.Actions.Add(this.AttachJobID);
            //this.Actions.Add(this.DeAttachJobID);
            this.Actions.Add(this.COC_BarReport);
            this.Actions.Add(this.SampleLabel);
            this.Actions.Add(this.RTCTReport);
            this.Actions.Add(this.Vertical);
            this.TargetViewId = "TaskSchedulerEventList_ListView_Copy";
            //this.Actions.Add(this.DeAttachJobID);
            //this.Actions.Add(this.AttachJobID);
        }

        #endregion
        //private DevExpress.ExpressApp.Actions.SimpleAction AttachJobID;
        //private DevExpress.ExpressApp.Actions.SimpleAction DeAttachJobID;
        private DevExpress.ExpressApp.Actions.SimpleAction COC_BarReport;
        private DevExpress.ExpressApp.Actions.SimpleAction SampleLabel;
        private DevExpress.ExpressApp.Actions.SimpleAction RTCTReport;
        private DevExpress.ExpressApp.Actions.SimpleAction Vertical;
    }
}
