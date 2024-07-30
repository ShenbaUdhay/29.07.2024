namespace LDM.Module.Controllers.Settings.TestParameter
{
    partial class TestParameterEditViewController
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
            this.prevQCParameterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.nextQCParameterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.addSampleParameterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.removeSampleParameterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.saveQCParameterAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.copyFromQCTypeAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.saveParameterDefaultsAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.gotoTestparameterEditAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // prevQCParameterAction
            // 
            this.prevQCParameterAction.Caption = "< Prev";
            this.prevQCParameterAction.Category = "catPrevQCParam";
            this.prevQCParameterAction.ConfirmationMessage = null;
            this.prevQCParameterAction.Id = "prevQCParameterAction";
            this.prevQCParameterAction.ToolTip = "Prev";
            this.prevQCParameterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.prevQCParameterAction_Execute);
            // 
            // nextQCParameterAction
            // 
            this.nextQCParameterAction.Caption = "Next >";
            this.nextQCParameterAction.Category = "catNextQCParam";
            this.nextQCParameterAction.ConfirmationMessage = null;
            this.nextQCParameterAction.Id = "nextQCParameterAction";
            this.nextQCParameterAction.ToolTip = "Next";
            this.nextQCParameterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.nextQCParameterAction_Execute);
            // 
            // addSampleParameterAction
            // 
            this.addSampleParameterAction.Caption = "Add";
            this.addSampleParameterAction.Category = "catAddSampleParam";
            this.addSampleParameterAction.ConfirmationMessage = null;
            this.addSampleParameterAction.Id = "addSampleParameterAction";
            this.addSampleParameterAction.ToolTip = "Add";
            this.addSampleParameterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.addSampleParameterAction_Execute);
            // 
            // removeSampleParameterAction
            // 
            this.removeSampleParameterAction.Caption = "Remove";
            this.removeSampleParameterAction.Category = "catRemoveSampleParam";
            this.removeSampleParameterAction.ConfirmationMessage = null;
            this.removeSampleParameterAction.Id = "removeSampleParameterAction";
            this.removeSampleParameterAction.ToolTip = "Remove";
            this.removeSampleParameterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.removeSampleParameterAction_Execute);
            // 
            // saveQCParameterAction
            // 
            this.saveQCParameterAction.Caption = "Save";
            this.saveQCParameterAction.Category = "catSaveQCParam";
            this.saveQCParameterAction.ConfirmationMessage = null;
            this.saveQCParameterAction.Id = "saveQCParameterAction";
            this.saveQCParameterAction.ToolTip = "Save";
            this.saveQCParameterAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.saveQCParameterAction_Execute);
            // 
            // copyFromQCTypeAction
            // 
            this.copyFromQCTypeAction.Caption = "Copy";
            this.copyFromQCTypeAction.Category = "catCopyFromQC";
            this.copyFromQCTypeAction.ConfirmationMessage = null;
            this.copyFromQCTypeAction.Id = "copyFromQCTypeAction";
            this.copyFromQCTypeAction.ToolTip = "Copy";
            this.copyFromQCTypeAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.copyFromQCTypeAction_Execute);
            // 
            // saveParameterDefaultsAction
            // 
            this.saveParameterDefaultsAction.Caption = "Save";
            this.saveParameterDefaultsAction.ConfirmationMessage = null;
            this.saveParameterDefaultsAction.Id = "saveParameterDefaultsAction";
            this.saveParameterDefaultsAction.ToolTip = "Save";
            this.saveParameterDefaultsAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveParameterDefaultsAction_Execute);
            // 
            // gotoTestparameterEditAction
            // 
            this.gotoTestparameterEditAction.Caption = "Testparameter Edit";
            this.gotoTestparameterEditAction.ConfirmationMessage = null;
            this.gotoTestparameterEditAction.Id = "gotoTestparameterEditAction";
            this.gotoTestparameterEditAction.ToolTip = "Testparameter Edit";
            this.gotoTestparameterEditAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.gotoTestparameterEditAction_Execute);
            // 
            // TestParameterEditViewController
            // 
            this.Actions.Add(this.prevQCParameterAction);
            this.Actions.Add(this.nextQCParameterAction);
            this.Actions.Add(this.addSampleParameterAction);
            this.Actions.Add(this.removeSampleParameterAction);
            this.Actions.Add(this.saveQCParameterAction);
            this.Actions.Add(this.copyFromQCTypeAction);
            this.Actions.Add(this.saveParameterDefaultsAction);
            this.Actions.Add(this.gotoTestparameterEditAction);

        }        

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction prevQCParameterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction nextQCParameterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction addSampleParameterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction removeSampleParameterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction saveQCParameterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction copyFromQCTypeAction;
        //private DevExpress.ExpressApp.Actions.SimpleAction copyToAllTestParamAction;
        //private DevExpress.ExpressApp.Actions.SimpleAction copyToAllQCParamAction;
        private DevExpress.ExpressApp.Actions.SimpleAction saveParameterDefaultsAction;
        private DevExpress.ExpressApp.Actions.SimpleAction gotoTestparameterEditAction;
    }
}
