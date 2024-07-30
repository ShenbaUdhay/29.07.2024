
using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SampleRegistration
{
    partial class SampleConditionCheckViewController
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
            this.SCC_Report = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PopupSCC_Report = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SampleRegistrationSCC_Report
            // 
            this.SCC_Report.Caption = "SCC Report";
            this.SCC_Report.Category = "Reports";
            this.SCC_Report.ImageName = "BO_Report";
            this.SCC_Report.ConfirmationMessage = null;
            this.SCC_Report.Id = "SCC_Report";
            this.SCC_Report.ToolTip = null;
           // this.SCC_Report.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.SCC_Report.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SCC_Report_Execute);
            // 
            // SampleRegistrationSCC_Report
            // 
            this.PopupSCC_Report.Caption = "SCC Report";
            this.PopupSCC_Report.Category = "ObjectsCreation";
            this.PopupSCC_Report.ImageName = "BO_Report";
            this.PopupSCC_Report.ConfirmationMessage = null;
            this.PopupSCC_Report.Id = "PopupSCC_Report";
            this.PopupSCC_Report.ToolTip = null;
            this.PopupSCC_Report.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SCC_PopupReport_Execute);
            // 
            // 
            // SampleConditionCheckViewController
            // 
            this.Actions.Add(this.SCC_Report);
            this.Actions.Add(this.PopupSCC_Report);
        }

        


        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction SCC_Report;
        private DevExpress.ExpressApp.Actions.SimpleAction PopupSCC_Report;

    }
}
