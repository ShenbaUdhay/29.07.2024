
using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.DailyQC
{
    partial class DailyQCViewController
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
            this.QCChart = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ChartRetrieve = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ChartReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);

            // 
            // QCChart
            // 
            this.QCChart.Caption = "Chart";
            this.QCChart.Category = "ObjectsCreation";
            this.QCChart.ConfirmationMessage = null;
            this.QCChart.Id = "QCChart";
            this.QCChart.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.QCChart.ToolTip = null;
            this.QCChart.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.QCChart_Execute);
            //            
            // ChartRetrieve
            // 
            this.ChartRetrieve.Caption = "Retrieve";
            this.ChartRetrieve.Category = "ChartRetrieve";
            this.ChartRetrieve.ConfirmationMessage = null;
            this.ChartRetrieve.Id = "ChartRetrieve";
            this.ChartRetrieve.ToolTip = null;
            this.ChartRetrieve.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ChartRetrieve_Execute);
            // 
            //
            //ChartReport
            this.ChartReport.Caption = "Daily QC Report";
            this.ChartReport.Category = "ChartReport";
            this.ChartReport.ConfirmationMessage = null;
            this.ChartReport.Id = "ChartReport";
            this.ChartReport.ToolTip = null;
            this.ChartReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ChartReport_Execute);
          
            //
            // DailyQCViewController
            // 
            this.Actions.Add(this.QCChart);
            this.Actions.Add(this.ChartRetrieve);
            this.Actions.Add(this.ChartReport);
         
        }
     
       
        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction QCChart;
        private DevExpress.ExpressApp.Actions.SimpleAction ChartRetrieve;
        private DevExpress.ExpressApp.Actions.SimpleAction ChartReport;
      
    }
}
