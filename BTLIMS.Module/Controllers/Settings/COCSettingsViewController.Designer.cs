
using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.Settings
{
    partial class COCSettingsViewController
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
            this.Sample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddSample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Test = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.COCSR_SLListViewEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestGroup = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Containers = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Sample
            // 
            this.Sample.Caption = "Sample";
            this.Sample.Category = "catCOCSample";
            this.Sample.ConfirmationMessage = null;
            this.Sample.Id = "COCSamplesbtn";
            this.Sample.ToolTip = "Samples";
            this.Sample.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Sample_Execute);
            // 
            // AddSample
            // 
            this.AddSample.Caption = "Add Sample";
            //this.AddSample.Category = "PopupActions";
            this.AddSample.ConfirmationMessage = null;
            this.AddSample.Id = "Add COCSample";
            this.AddSample.ToolTip = null;
            this.AddSample.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SimpleAction1_Execute);
            // 
            // Test
            // 
            this.Test.Caption = "Test";
            this.Test.Category = "RecordEdit";
            this.Test.ConfirmationMessage = null;
            this.Test.Id = "COCTestbtn";
            this.Test.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Test.ToolTip = "Test";
            this.Test.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Test_Execute);
            // 
            // SR_SLListViewEdit
            // 
            this.COCSR_SLListViewEdit.Caption = "Edit";
            this.COCSR_SLListViewEdit.Category = "Edit";
            this.COCSR_SLListViewEdit.ConfirmationMessage = null;
            this.COCSR_SLListViewEdit.Id = "COCSR_SLListViewEdit";
            this.COCSR_SLListViewEdit.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.COCSR_SLListViewEdit.ToolTip = null;
            this.COCSR_SLListViewEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COCSLListViewEdit_Execute);
            //
            //// TestGroup
            // 
            this.TestGroup.Caption = "TestGroup";
            this.TestGroup.Category = "RecordEdit";
            this.TestGroup.ConfirmationMessage = null;
            this.TestGroup.Id = "COCTestGroup1";
            this.TestGroup.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.TestGroup.ToolTip = null;
            this.TestGroup.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestGroup_Execute);
            //            
            // TestSelectionAdd
            // 
            this.TestSelectionAdd.Caption = "Add";
            this.TestSelectionAdd.Category = "PopupActions";
            this.TestSelectionAdd.ConfirmationMessage = null;
            this.TestSelectionAdd.Id = "TestSelectionAdd1";
            this.TestSelectionAdd.ToolTip = null;
            this.TestSelectionAdd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionAdd_Execute);
            // 
            // TestSelectionRemove
            // 
            this.TestSelectionRemove.Caption = "Remove";
            this.TestSelectionRemove.Category = "PopupActions";
            this.TestSelectionRemove.ConfirmationMessage = null;
            this.TestSelectionRemove.Id = "TestSelectionRemove1";
            this.TestSelectionRemove.TargetViewId = "";
            this.TestSelectionRemove.ToolTip = null;
            this.TestSelectionRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionRemove_Execute);
            // 
            // TestSelectionSave
            // 
            this.TestSelectionSave.Caption = "Save";
            this.TestSelectionSave.Category = "PopupActions";
            this.TestSelectionSave.ConfirmationMessage = null;
            this.TestSelectionSave.Id = "TestSelectionSave1";
            this.TestSelectionSave.ToolTip = null;
            this.TestSelectionSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionSave_Execute);
            //
            // Containers
            // 
            this.Containers.Caption = "Cont";
            this.Containers.Category = "ListView";
            this.Containers.ConfirmationMessage = null;
            this.Containers.Id = "Containers1";
            this.Containers.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Containers.ToolTip = null;
            this.Containers.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Containers_Execute);
            // 
            // COCSettingsViewController
            //
            this.Actions.Add(this.Sample);
            this.Actions.Add(this.AddSample);
            this.Actions.Add(this.Test);
            this.Actions.Add(this.TestGroup);
            this.Actions.Add(this.COCSR_SLListViewEdit);
            this.Actions.Add(this.TestSelectionAdd);
            this.Actions.Add(this.TestSelectionRemove);
            this.Actions.Add(this.TestSelectionSave);
            this.Actions.Add(this.Containers);
        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Sample;
        private DevExpress.ExpressApp.Actions.SimpleAction Test;
        private DevExpress.ExpressApp.Actions.SimpleAction COCSR_SLListViewEdit;
        private DevExpress.ExpressApp.Actions.SimpleAction AddSample;
        private DevExpress.ExpressApp.Actions.SimpleAction TestGroup;
        private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionAdd;
        private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionRemove;
        private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionSave;
        private SimpleAction Containers;
    }
}
