﻿
using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Web.Controllers.Settings
{
    partial class EDDBuilderViewController
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
            this.Execute = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ExportToEDD = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.Preview_EDD = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Export_EDD = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            //DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            //
            //EDDExport
            //
            this.Export_EDD.Caption = "Select For Format";
            this.Export_EDD.Category = "Export";
            this.Export_EDD.ConfirmationMessage = null;
            this.Export_EDD.Id = "Export_EDD";

            choiceActionItem1.Caption = "Export to Excel All Sheet";
            choiceActionItem1.Id = "ExporttoExcelAllSheet";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;

            choiceActionItem2.Caption = "Export to Excel Single Sheet";
            choiceActionItem2.Id = "ExportToExcelSingleSheet";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;

            choiceActionItem3.Caption = "Export to CSV";
            choiceActionItem3.Id = "ExporttoCSV";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;

            choiceActionItem4.Caption = "Export to TXT";
            choiceActionItem4.Id = "ExporttoTXT";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null; 
            
            //choiceActionItem5.Caption = "N/A";
            //choiceActionItem5.Id = "NonAvalable";
            //choiceActionItem5.ImageName = null;
            //choiceActionItem5.Shortcut = null;
            //choiceActionItem5.ToolTip = null;

            this.Export_EDD.Items.Add(choiceActionItem1);
            this.Export_EDD.Items.Add(choiceActionItem2);
            this.Export_EDD.Items.Add(choiceActionItem3);
            this.Export_EDD.Items.Add(choiceActionItem4);
            //this.Export_EDD.Items.Add(choiceActionItem5);
            this.Export_EDD.SelectedItemChanged += EDDExport_SelectedItemChanged;
            this.Actions.Add(this.Export_EDD);
            // 
            // Preview_EDD
            // 
            //this.Preview_EDD.Caption = "Preview";
            //this.Preview_EDD.Category = "RecordEdit";
            //this.Preview_EDD.ConfirmationMessage = null;
            //this.Preview_EDD.ImageName = "report_16x16";
            //this.Preview_EDD.Id = "Preview_EDD";
            //this.Preview_EDD.TargetViewId = "EDDReportGenerator_ListView";
            //this.Preview_EDD.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            //this.Preview_EDD.ToolTip = null;
            //this.Preview_EDD.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Preview_EDD_Execute);
            //// 
            //// EDDBuilderViewController
            //// 
            //this.Actions.Add(this.Preview_EDD);
            // 
            // Execute
            // 
            this.Execute.Caption = "Execute";
            this.Execute.Category = "EDD_Execute";
            this.Execute.ConfirmationMessage = null;
            this.Execute.Id = "Execute";
            this.Execute.ToolTip = null;
            this.Execute.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Execute_Execute);
            // 
            // ExportToEDD
            // 
            this.ExportToEDD.Caption = "Export";
            this.ExportToEDD.Category = "Export";
            this.ExportToEDD.ConfirmationMessage = null;
            this.ExportToEDD.Id = "ExportToEDD";
            this.ExportToEDD.ToolTip = null;
            this.ExportToEDD.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ExportToEDD_Execute);
            // 
            // EDDBuilderViewController
            // 
            this.Actions.Add(this.Execute);
            this.Actions.Add(this.ExportToEDD);

        }


        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Execute;
        private DevExpress.ExpressApp.Actions.SimpleAction ExportToEDD;
        //private DevExpress.ExpressApp.Actions.SimpleAction Preview_EDD;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction Export_EDD;
    }
}
