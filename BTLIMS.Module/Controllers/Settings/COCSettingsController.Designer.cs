namespace LDM.Module.Controllers.Settings
{
    partial class COCSettingsController
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
            this.Test = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.COCTestGroup = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddSample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.TestSelectionAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.TestSelectionRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.TestSelectionSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveCOCSettings = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CopySamples = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CopyTest = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.COCSaveAs = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Sample
            // 
            this.Sample.Caption = "Samples";
            this.Sample.Category = "catCOCSample";
            this.Sample.ConfirmationMessage = null;
            this.Sample.Id = "COCSamples";
            this.Sample.ToolTip = "Samples";
            this.Sample.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Sample_Execute);
            // 
            // Test
            // 
            this.Test.Caption = "Test";
            this.Test.Category = "RecordEdit";
            this.Test.ConfirmationMessage = null;
            this.Test.Id = "COCTest";
            this.Test.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Test.ToolTip = "Test";
            this.Test.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Test_Execute);
            // 
            // COCTestGroup
            // 
            this.COCTestGroup.Caption = "TestGroup";
            this.COCTestGroup.Category = "RecordEdit";
            this.COCTestGroup.ConfirmationMessage = null;
            this.COCTestGroup.Id = "COCTestGroup";
            this.COCTestGroup.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.COCTestGroup.ToolTip = "TestGroup";
            this.COCTestGroup.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COCTestGroup_Execute);
            // 
            // AddSample
            // 
            this.AddSample.Caption = "Add Sample";
            this.AddSample.Category = "Edit";
            this.AddSample.ConfirmationMessage = null;
            this.AddSample.Id = "COCAddSample";
            this.AddSample.ToolTip = "Add Sample";
            this.AddSample.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddSample_Execute);
            // 
            // TestSelectionAdd
            // 
            //this.TestSelectionAdd.Caption = "Add";
            //this.TestSelectionAdd.Category = "PopupActions";
            //this.TestSelectionAdd.ConfirmationMessage = null;
            //this.TestSelectionAdd.Id = "COCTestSelectionAdd";
            //this.TestSelectionAdd.ToolTip = "Add";
            //this.TestSelectionAdd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionAdd_Execute);
            //// 
            //// TestSelectionRemove
            //// 
            //this.TestSelectionRemove.Caption = "Remove";
            //this.TestSelectionRemove.Category = "PopupActions";
            //this.TestSelectionRemove.ConfirmationMessage = null;
            //this.TestSelectionRemove.Id = "COCTestSelectionRemove";
            //this.TestSelectionRemove.ToolTip = "Remove";
            //this.TestSelectionRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionRemove_Execute);
            //// 
            //// TestSelectionSave
            //// 
            //this.TestSelectionSave.Caption = "Save";
            //this.TestSelectionSave.Category = "PopupActions";
            //this.TestSelectionSave.ConfirmationMessage = null;
            //this.TestSelectionSave.Id = "COCTestSelectionSave";
            //this.TestSelectionSave.ToolTip = "Save";
            //this.TestSelectionSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionSave_Execute);
            // 
            // SaveCOCSettings
            // 
            this.SaveCOCSettings.Caption = "Save";
            this.SaveCOCSettings.Category = "View";
            this.SaveCOCSettings.ConfirmationMessage = null;
            this.SaveCOCSettings.Id = "SaveCOCSettings";
            this.SaveCOCSettings.ToolTip = "Save";
            this.SaveCOCSettings.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveCOCSettings_Execute);
            // 
            // CopySamples
            // 
            this.CopySamples.AcceptButtonCaption = null;
            this.CopySamples.CancelButtonCaption = null;
            this.CopySamples.Caption = "Copy Samples";
            this.CopySamples.Category = "Edit";
            this.CopySamples.ConfirmationMessage = null;
            this.CopySamples.Id = "COCCopySamples";
            this.CopySamples.ToolTip = "Copy Samples";
            this.CopySamples.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CopySamples_CustomizePopupWindowParams);
            this.CopySamples.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CopySamples_Execute);
            // 
            // CopyTest
            // 
            this.CopyTest.AcceptButtonCaption = null;
            this.CopyTest.CancelButtonCaption = null;
            this.CopyTest.Caption = "Copy Test";
            this.CopyTest.Category = "Edit";
            this.CopyTest.ConfirmationMessage = null;
            this.CopyTest.Id = "COCCopyTest";
            this.CopyTest.ToolTip = "Copy Test";
            this.CopyTest.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CopyTest_CustomizePopupWindowParams);
            this.CopyTest.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CopyTest_Execute);
            // 
            // COCSaveAs
            // 
            this.COCSaveAs.Caption = "Save As";
            this.COCSaveAs.ConfirmationMessage = null;
            this.COCSaveAs.Id = "COCSaveAs";
            this.COCSaveAs.ToolTip = null;
            this.COCSaveAs.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COCSaveAs_Execute);
            // 
            // COCSettingsController
            // 
            this.Actions.Add(this.Sample);
            this.Actions.Add(this.Test);
            this.Actions.Add(this.COCTestGroup);
            this.Actions.Add(this.AddSample);
            //this.Actions.Add(this.TestSelectionAdd);
            //this.Actions.Add(this.TestSelectionRemove);
            //this.Actions.Add(this.TestSelectionSave);
            this.Actions.Add(this.SaveCOCSettings);
            this.Actions.Add(this.CopySamples);
            this.Actions.Add(this.CopyTest);
            this.Actions.Add(this.COCSaveAs);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Sample;
        private DevExpress.ExpressApp.Actions.SimpleAction Test;
        private DevExpress.ExpressApp.Actions.SimpleAction AddSample;
        //private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionAdd;
        //private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionRemove;
        //private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionSave;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveCOCSettings;
        private DevExpress.ExpressApp.Actions.SimpleAction COCTestGroup;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CopySamples;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CopyTest;
        private DevExpress.ExpressApp.Actions.SimpleAction COCSaveAs;
    }
}
