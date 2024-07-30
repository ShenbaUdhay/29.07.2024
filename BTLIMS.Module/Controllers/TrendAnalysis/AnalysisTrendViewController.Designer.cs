
using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.AnalysisTrend
{
    partial class AnalysisTrendViewController
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
            this.TARetrieve = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TACalculate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TATrend = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ListTrend = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RefreshScale = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AutoScaleChart = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TARetrieve
            // 
            this.TARetrieve.Caption = "Retrieve";
            this.TARetrieve.Category = "TARetrieve";
            this.TARetrieve.ConfirmationMessage = null;
            this.TARetrieve.Id = "TARetrieve";
            this.TARetrieve.ToolTip = null;
            this.TARetrieve.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TARetrieve_Execute);
            // 
            // TACalculate
            // 
            this.TACalculate.Caption = "Calculate";
            this.TACalculate.Category = "TACalculate";
            this.TACalculate.ConfirmationMessage = null;
            this.TACalculate.Id = "TACalculate";
            this.TACalculate.ToolTip = null;
            this.TACalculate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TACalculate_Execute);
            // 
            // TATrend
            // 
            this.TATrend.Caption = "Trend";
            this.TATrend.Category = "TATrend";
            this.TATrend.ConfirmationMessage = null;
            this.TATrend.Id = "TATrend";
            this.TATrend.ToolTip = null;
            this.TATrend.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TATrend_Execute);
            // 
            // ListTrend
            // 
            this.ListTrend.Caption = "Trend";
            this.ListTrend.Category = "ListView";
            this.ListTrend.ConfirmationMessage = null;
            this.ListTrend.Id = "ListTrend";
            this.ListTrend.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ListTrend.ToolTip = null;
            this.ListTrend.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ListTrend_Execute);
            // 
            // AnalysisTrendViewController
            // 
            this.RefreshScale.Caption = "Refresh";
            this.RefreshScale.Category = "TAScale";
            this.RefreshScale.ConfirmationMessage = null;
            this.RefreshScale.Id = "RefreshScale";
            this.RefreshScale.ToolTip = null;
            this.RefreshScale.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RefreshScale_Execute);// 
            // ListTrend
            // 
            this.AutoScaleChart.Caption = "AutoScaleChart";
            this.AutoScaleChart.Category = "AutoScaleChart";
            this.AutoScaleChart.ConfirmationMessage = null;
            this.AutoScaleChart.Id = "AutoScaleChart";
            this.AutoScaleChart.ToolTip = null;
            this.AutoScaleChart.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AutoScaleChart_Execute);
            // 
            // AnalysisTrendViewController
            // 
            this.Actions.Add(this.TARetrieve);
            this.Actions.Add(this.TACalculate);
            this.Actions.Add(this.TATrend);
            this.Actions.Add(this.ListTrend);
            this.Actions.Add(this.RefreshScale);
            this.Actions.Add(this.AutoScaleChart);

        }
        #endregion

        //private DevExpress.ExpressApp.Actions.ParametrizedAction TAFullTextSearch;
        private DevExpress.ExpressApp.Actions.SimpleAction TARetrieve;
        private DevExpress.ExpressApp.Actions.SimpleAction TACalculate;
        private DevExpress.ExpressApp.Actions.SimpleAction TATrend;
        private DevExpress.ExpressApp.Actions.SimpleAction ListTrend;
        private DevExpress.ExpressApp.Actions.SimpleAction RefreshScale;
        private DevExpress.ExpressApp.Actions.SimpleAction AutoScaleChart;
    }
}
