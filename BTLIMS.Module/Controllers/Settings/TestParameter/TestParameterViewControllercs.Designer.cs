using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Web.Controllers.TestParameter
{
    partial class TestParameterViewControllercs
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.parameterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Copyparameter = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CopyTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CopyParameters = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ADDAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RemoveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestDefaultResult = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddNewTestDefaultResult = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DeleteTestDefaultResult = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // parameterAction
            // 
            this.parameterAction.Caption = "Parameter";
            this.parameterAction.Category = "catTestParameter";
            this.parameterAction.ConfirmationMessage = null;
            this.parameterAction.Id = "parameterAction";
            this.parameterAction.ToolTip = "Parameter";
            this.parameterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.parameterAction_Execute);
            // 
            // Copyparameter
            // 
            this.Copyparameter.Caption = "Copy Parameter";
            this.Copyparameter.Category = "RecordEdit";
            this.Copyparameter.ConfirmationMessage = null;
            this.Copyparameter.Id = "Copyparameter";
            this.Copyparameter.ImageName = "Action_Copy";
            this.Copyparameter.ToolTip = null;
            this.Copyparameter.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Copyparameter_Execute);
            // 
            // CopyTest
            // 
            this.CopyTest.Caption = "Copy Test";
            this.CopyTest.Category = "RecordEdit";
            this.CopyTest.ConfirmationMessage = null;
            this.CopyTest.Id = "CopyTest";
            this.CopyTest.ImageName = "Action_Copy";
            this.CopyTest.ToolTip = null;
            this.CopyTest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CopyTest_Execute);
            // 
            // CopyParameters
            // 
            this.CopyParameters.Caption = "Copy Parameters";
            this.CopyParameters.ConfirmationMessage = null;
            this.CopyParameters.Id = "CopyParameters";
            this.CopyParameters.ImageName = "Action_Copy";
            choiceActionItem6.Caption = "";
            choiceActionItem6.Id = "None";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem1.Caption = "Copy parameters";
            choiceActionItem1.Id = "Copyparameters";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Copy surrogates";
            choiceActionItem2.Id = "Copysurrogates";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Copy internal standards";
            choiceActionItem3.Id = "Copyinternalstandards";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "Copy qc parameters";
            choiceActionItem4.Id = "Copyqcparameters";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "Copy qc parameters from same test";
            choiceActionItem5.Id = "Copyqcparameterssametest";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.CopyParameters.Items.Add(choiceActionItem6);
            this.CopyParameters.Items.Add(choiceActionItem1);
            this.CopyParameters.Items.Add(choiceActionItem2);
            this.CopyParameters.Items.Add(choiceActionItem3);
            this.CopyParameters.Items.Add(choiceActionItem4);
            this.CopyParameters.Items.Add(choiceActionItem5);
            this.CopyParameters.ToolTip = null;
            this.CopyParameters.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.CopyParameters_Execute);
            // 
            // ADDAction
            // 
            this.ADDAction.Caption = "ADD";
            this.ADDAction.Category = "RecordEdit";
            this.ADDAction.ConfirmationMessage = null;
            this.ADDAction.Id = "ADDAction";
            this.ADDAction.ImageName = "Add_16x16.png";
            this.ADDAction.ToolTip = null;
            this.ADDAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ADDAction_Execute);
            // 
            // RemoveAction
            // 
            this.RemoveAction.Caption = "Remove";
            this.RemoveAction.Category = "RecordEdit";
            this.RemoveAction.ConfirmationMessage = null;
            this.RemoveAction.Id = "RemoveAction";
            this.RemoveAction.ImageName = "Remove.png";
            this.RemoveAction.ToolTip = null;
            this.RemoveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RemoveAction_Execute);
            // 
            // SaveAction
            // 
            this.SaveAction.Caption = "Save";
            this.SaveAction.Category = "RecordEdit";
            this.SaveAction.ConfirmationMessage = null;
            this.SaveAction.Id = "SaveAction";
            this.SaveAction.ImageName = "Save.png";
            this.SaveAction.ToolTip = null;
            this.SaveAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveAction_Execute);
            // 
            // TestDefaultResult
            // 
            this.TestDefaultResult.Caption = "...";
            this.TestDefaultResult.Category = "ListView";
            this.TestDefaultResult.ConfirmationMessage = null;
            this.TestDefaultResult.Id = "TestDefaultResult";
            this.TestDefaultResult.ToolTip = null;
            this.TestDefaultResult.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.TestDefaultResult.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DefaultResultSelect); // 
            // TestDefaultResult
            // 
            this.AddNewTestDefaultResult.Caption = "New";
            this.AddNewTestDefaultResult.Category = "ObjectsCreation";
            this.AddNewTestDefaultResult.ConfirmationMessage = null;
            this.AddNewTestDefaultResult.Id = "AddNewTestDefaultResult";
            this.AddNewTestDefaultResult.ToolTip = null;
            this.AddNewTestDefaultResult.ImageName = "Action_New";
            this.AddNewTestDefaultResult.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DefaultResultAddNew);
            // TestDefaultResultDelete
            // 
            this.DeleteTestDefaultResult.Caption = "Delete";
            this.DeleteTestDefaultResult.Category = "ObjectsCreation";
            this.DeleteTestDefaultResult.ConfirmationMessage = null;
            this.DeleteTestDefaultResult.Id = "DeleteTestDefaultResult";
            this.DeleteTestDefaultResult.ToolTip = null;
            this.DeleteTestDefaultResult.ImageName = "Action_Delete";
            this.DeleteTestDefaultResult.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DeleteDefaultResult);
            // 
            // TestParameterViewControllercs
            // 
            this.Actions.Add(this.parameterAction);
            this.Actions.Add(this.Copyparameter);
            this.Actions.Add(this.CopyTest);
            this.Actions.Add(this.CopyParameters);
            this.Actions.Add(this.ADDAction);
            this.Actions.Add(this.RemoveAction);
            this.Actions.Add(this.SaveAction);
            this.Actions.Add(this.TestDefaultResult);
            this.Actions.Add(this.AddNewTestDefaultResult);
            this.Actions.Add(this.DeleteTestDefaultResult);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction parameterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction Copyparameter;
        private DevExpress.ExpressApp.Actions.SimpleAction CopyTest;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction CopyParameters;
        private DevExpress.ExpressApp.Actions.SimpleAction ADDAction;
        private DevExpress.ExpressApp.Actions.SimpleAction RemoveAction;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveAction;
        private DevExpress.ExpressApp.Actions.SimpleAction TestDefaultResult;
        private DevExpress.ExpressApp.Actions.SimpleAction AddNewTestDefaultResult;
        private DevExpress.ExpressApp.Actions.SimpleAction DeleteTestDefaultResult;
    }
}
