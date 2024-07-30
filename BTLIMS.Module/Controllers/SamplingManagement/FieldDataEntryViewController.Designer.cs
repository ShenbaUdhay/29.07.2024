
namespace Labmaster.Module.Controllers.SamplingManagement
{
    partial class FieldDataEntryViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem7 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.STFilter = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.FieldDataEntrySave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataEntryComplete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataEntryEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataEntryCopyToAll = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataEntryCopyPrevious = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FieldDataEntryCopyTo = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FieldDataEntrySave
            // 
            this.FieldDataEntrySave.Caption = "Save";
            this.FieldDataEntrySave.ConfirmationMessage = null;
            this.FieldDataEntrySave.Id = "FieldDataEntrySave";
            this.FieldDataEntrySave.ToolTip = null;
            this.FieldDataEntrySave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataEntrySave_Execute);
            // 
            // FieldDataEntryComplete
            // 
            this.FieldDataEntryComplete.Caption = "Complete";
            this.FieldDataEntryComplete.ConfirmationMessage = null;
            this.FieldDataEntryComplete.Id = "FieldDataEntryComplete";
            this.FieldDataEntryComplete.ToolTip = null;
            this.FieldDataEntryComplete.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataEntryComplete_Execute);
            // 
            // FieldDataEntryEdit
            // 
            this.FieldDataEntryEdit.Caption = "Edit";
            this.FieldDataEntryEdit.ConfirmationMessage = null;
            this.FieldDataEntryEdit.Id = "FieldDataEntryEdit";
            this.FieldDataEntryEdit.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.FieldDataEntryEdit.ToolTip = null;
            this.FieldDataEntryEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataEntryEdit_Execute);
            // 
            // FieldDataEntryCopyToAll
            // 
            this.FieldDataEntryCopyToAll.Caption = "Copy To All";
            this.FieldDataEntryCopyToAll.Category = "PopupActions";
            this.FieldDataEntryCopyToAll.ConfirmationMessage = null;
            this.FieldDataEntryCopyToAll.Id = "FieldDataEntryCopyToAll";
            this.FieldDataEntryCopyToAll.ToolTip = null;
            this.FieldDataEntryCopyToAll.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataEntryCopyToAll_Execute);
            // 
            // FieldDataEntryCopyPrevious
            // 
            this.FieldDataEntryCopyPrevious.Caption = "Copy Previous";
            this.FieldDataEntryCopyPrevious.Category = "PopupActions";
            this.FieldDataEntryCopyPrevious.ConfirmationMessage = null;
            this.FieldDataEntryCopyPrevious.Id = "FieldDataEntryCopyPrevious";
            this.FieldDataEntryCopyPrevious.ToolTip = null;
            this.FieldDataEntryCopyPrevious.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataEntryCopyPrevious_Execute);
            // 
            // FieldDataEntryCopyTo
            // 
            this.FieldDataEntryCopyTo.Caption = "Copy To";
            this.FieldDataEntryCopyTo.Category = "PopupActions";
            this.FieldDataEntryCopyTo.ConfirmationMessage = null;
            this.FieldDataEntryCopyTo.Id = "FieldDataEntryCopyTo";
            this.FieldDataEntryCopyTo.ToolTip = null;
            this.FieldDataEntryCopyTo.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FieldDataEntryCopyTo_Execute);
            // 
            // sampleRegistrationDateFilterAction
            // 
            this.STFilter.Caption = "Date Filter";
            this.STFilter.Category = "View";
            this.STFilter.ConfirmationMessage = null;
            this.STFilter.Id = "STFilter1";
            choiceActionItem1.Caption = "1M";
            choiceActionItem1.Id = "1M";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "3M";
            choiceActionItem2.Id = "3M";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "6M";
            choiceActionItem3.Id = "6M";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "1Y";
            choiceActionItem4.Id = "1Y";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "2Y";
            choiceActionItem5.Id = "2Y";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "5Y";
            choiceActionItem6.Id = "5Y";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            choiceActionItem7.Caption = "All";
            choiceActionItem7.Id = "All";
            choiceActionItem7.ImageName = null;
            choiceActionItem7.Shortcut = null;
            choiceActionItem7.ToolTip = null;
            this.STFilter.Items.Add(choiceActionItem1);
            this.STFilter.Items.Add(choiceActionItem2);
            this.STFilter.Items.Add(choiceActionItem3);
            this.STFilter.Items.Add(choiceActionItem4);
            this.STFilter.Items.Add(choiceActionItem5);
            this.STFilter.Items.Add(choiceActionItem6);
            this.STFilter.Items.Add(choiceActionItem7);
            this.STFilter.ToolTip = "Date Filter";
            // 
            // FieldDataEntryViewController
            // 
            this.Actions.Add(this.FieldDataEntrySave);
            this.Actions.Add(this.FieldDataEntryComplete);
            this.Actions.Add(this.FieldDataEntryEdit);
            this.Actions.Add(this.FieldDataEntryCopyToAll);
            this.Actions.Add(this.FieldDataEntryCopyPrevious);
            this.Actions.Add(this.FieldDataEntryCopyTo);
            this.Actions.Add(this.STFilter);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataEntrySave;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataEntryComplete;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataEntryEdit;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataEntryCopyToAll;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataEntryCopyPrevious;
        private DevExpress.ExpressApp.Actions.SimpleAction FieldDataEntryCopyTo;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction STFilter;
    }
}
