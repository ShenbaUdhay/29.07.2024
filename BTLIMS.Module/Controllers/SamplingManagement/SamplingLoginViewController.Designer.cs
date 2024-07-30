using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SamplingManagement
{
    partial class SamplingLoginViewController
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
            this.SamplingCopySamples = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
           // this.SamplingReanalysis = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Btn_Add_SamplingCollector = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingSL_CopyTest = new SimpleAction(this.components);
            this.SamplingFolderLabel = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingBarcodeReport = new SimpleAction(this.components);
            this.SaveSamplingSamples = new SimpleAction(this.components);

            ///
            //CopySamples
            //
            this.SamplingCopySamples.Caption = "Copy Samples";
            this.SamplingCopySamples.Category = "View";
            this.SamplingCopySamples.ConfirmationMessage = null;
            this.SamplingCopySamples.Id = "SamplingCopySamples";
            this.SamplingCopySamples.ImageName = "Action_Copy_CellValue";
            this.SamplingCopySamples.TargetViewId = "";
            this.SamplingCopySamples.ToolTip = null;
            this.SamplingCopySamples.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CopySamples_Execute);
            ///
            //Reanalysis
            //
            //this.SamplingReanalysis.Caption = "Reanalysis";
            //this.SamplingReanalysis.Category = "View";
            //this.SamplingReanalysis.ConfirmationMessage = null;
            //this.SamplingReanalysis.Id = "SamplingReanalysis";
            //this.SamplingReanalysis.ImageName = "Action_ResetViewSettings";
            //this.SamplingReanalysis.TargetViewId = "";
            //this.SamplingReanalysis.ToolTip = null;
            //this.SamplingReanalysis.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //this.SamplingReanalysis.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Reanalysis_Execute);
            ///
            //Add Collector
            //
            this.Btn_Add_SamplingCollector.Caption = "Add New Collector";
            this.Btn_Add_SamplingCollector.Category = "View";
            this.Btn_Add_SamplingCollector.ConfirmationMessage = null;
            this.Btn_Add_SamplingCollector.ImageName = "Add_16x16";
            this.Btn_Add_SamplingCollector.Id = "Btn_Add_SamplingCollector";
            this.Btn_Add_SamplingCollector.TargetViewId = "";
            this.Btn_Add_SamplingCollector.ToolTip = null;
            this.Btn_Add_SamplingCollector.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Btn_Add_Collector_Execute);

            //
            //Copy test
            //
            this.SamplingSL_CopyTest.Caption = "Copy Test";
            this.SamplingSL_CopyTest.Category = "View";
            this.SamplingSL_CopyTest.ConfirmationMessage = null;
            this.SamplingSL_CopyTest.Id = "SamplingSL_CopyTest";
            this.SamplingSL_CopyTest.ToolTip = null;
            this.SamplingSL_CopyTest.ImageName = "Copy_16x16";
            this.SamplingSL_CopyTest.Execute += SL_CopyTest_Execute;
            // 
            // BarcodeReport
            // 
            this.SamplingBarcodeReport.Caption = "Barcode Report";
            this.SamplingBarcodeReport.ConfirmationMessage = null;
            this.SamplingBarcodeReport.Id = "SamplingBarcodeReport";
            this.SamplingBarcodeReport.ToolTip = null;
            this.SamplingBarcodeReport.ImageName = "Barcode_16x16";
            this.SamplingBarcodeReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.BarcodeReport_Execute);
            // 
            // FolderLabel
            // 
            this.SamplingFolderLabel.Caption = "Folder Label";
            this.SamplingFolderLabel.ConfirmationMessage = null;
            this.SamplingFolderLabel.Id = "SamplingFolderLabel";
            this.SamplingFolderLabel.ToolTip = null;
            this.SamplingFolderLabel.ImageName = "Barcode_16x16";
            this.SamplingFolderLabel.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FolderLabel_Execute);
            //
            // Save
            // 
            this.SaveSamplingSamples.Caption = "Save";
            this.SaveSamplingSamples.ConfirmationMessage = null;
            this.SaveSamplingSamples.Category = "Edit";
            this.SaveSamplingSamples.Id = "SaveSamplingSamples";
            this.SaveSamplingSamples.ToolTip = null;
            this.SaveSamplingSamples.ImageName = "Save_16x16";
            this.SaveSamplingSamples.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveAction_Execute);
            //
            //Controller
            //
            this.Actions.Add(this.SamplingCopySamples);
            //this.Actions.Add(this.SamplingReanalysis);
            this.Actions.Add(this.Btn_Add_SamplingCollector);
            this.Actions.Add(this.SamplingSL_CopyTest);
            this.Actions.Add(this.SamplingBarcodeReport);
            this.Actions.Add(this.SamplingFolderLabel);
            this.Actions.Add(this.SaveSamplingSamples);
        }

        private SimpleAction SamplingCopySamples;
        //private SimpleAction SamplingReanalysis;
        private SimpleAction Btn_Add_SamplingCollector;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplingSL_CopyTest;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplingBarcodeReport;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplingFolderLabel;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveSamplingSamples;
        #endregion
    }
}
