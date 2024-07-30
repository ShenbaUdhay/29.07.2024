using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SamplingManagement
{
    partial class TaskManagementViewController
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
            this.SamplingSample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingAddSample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnSamplingQuoteImportSamples = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingContainers = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingTestSelectionAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingTestSelectionRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingTestSelectionSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SPSubmit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SPSaveAs = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SPCancel = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingProposalDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.HistoryOfSamplingProposal = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CopyRecurrence = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CopyReccurenceSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SamplingProposalHistoryDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);

            // Sample
            this.SamplingSample.Caption = "Sample";
            this.SamplingSample.Category = "Sample";
            this.SamplingSample.ConfirmationMessage = null;
            this.SamplingSample.Id = "SamplingSample";
            this.SamplingSample.ToolTip = null;
            this.SamplingSample.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Sample_Execute);
            // 
            // AddSample
            // 
            this.SamplingAddSample.Caption = "Add Sample";
            this.SamplingAddSample.Category = "ObjectsCreation";
            this.SamplingAddSample.ImageName = "Add_16x16";
            this.SamplingAddSample.ConfirmationMessage = null;
            this.SamplingAddSample.Id = "SamplingAddSample";
            this.SamplingAddSample.ToolTip = null;
            this.SamplingAddSample.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddSample_Execute);
            // 
            // Test
            // 
            this.SamplingTest.Caption = "Test";
            this.SamplingTest.Category = "ListView";
            this.SamplingTest.ConfirmationMessage = null;
            this.SamplingTest.Id = "SamplingTest";
            this.SamplingTest.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SamplingTest.ToolTip = null;
            this.SamplingTest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Test_Execute);
            //


            // 
            // btnQuoteImportSamples
            // 
            this.btnSamplingQuoteImportSamples.Caption = "Import Quote";
            this.btnSamplingQuoteImportSamples.ImageName = "Down_16x16";
            this.btnSamplingQuoteImportSamples.ConfirmationMessage = null;
            this.btnSamplingQuoteImportSamples.Id = "btnSamplingQuoteImportSamples";
            this.btnSamplingQuoteImportSamples.ToolTip = "Import Quotes";
            this.btnSamplingQuoteImportSamples.Category = "ImportQuoteSamples";
            this.btnSamplingQuoteImportSamples.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnQuoteImportSamples_Execute);
            // 
            // Containers
            // 
            this.SamplingContainers.Caption = "Cont";
            this.SamplingContainers.Category = "ListView";
            this.SamplingContainers.ConfirmationMessage = null;
            this.SamplingContainers.Id = "SamplingContainers";
            this.SamplingContainers.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SamplingContainers.ToolTip = null;
            this.SamplingContainers.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Containers_Execute);

            // 
            // TestSelectionAdd
            // 
            this.SamplingTestSelectionAdd.Caption = "Add";
            this.SamplingTestSelectionAdd.Category = "PopupActions";
            this.SamplingTestSelectionAdd.ConfirmationMessage = null;
            this.SamplingTestSelectionAdd.Id = "SamplingTestSelectionAdd";
            this.SamplingTestSelectionAdd.ToolTip = null;
            this.SamplingTestSelectionAdd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionAdd_Execute);
            // 
            // TestSelectionRemove
            // 
            this.SamplingTestSelectionRemove.Caption = "Remove";
            this.SamplingTestSelectionRemove.Category = "PopupActions";
            this.SamplingTestSelectionRemove.ConfirmationMessage = null;
            this.SamplingTestSelectionRemove.Id = "SamplingTestSelectionRemove";
            this.SamplingTestSelectionRemove.TargetViewId = "";
            this.SamplingTestSelectionRemove.ToolTip = null;
            this.SamplingTestSelectionRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionRemove_Execute);
            // 
            // TestSelectionSave
            // 
            this.SamplingTestSelectionSave.Caption = "Save";
            this.SamplingTestSelectionSave.Category = "PopupActions";
            this.SamplingTestSelectionSave.ConfirmationMessage = null;
            this.SamplingTestSelectionSave.Id = "SamplingTestSelectionSave";
            this.SamplingTestSelectionSave.ToolTip = null;
            this.SamplingTestSelectionSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionSave_Execute);
            // 
            // Submit
            // 
            this.SPSubmit.Caption = "Submit";
            this.SPSubmit.Category = "View";
            this.SPSubmit.ConfirmationMessage = null;
            this.SPSubmit.Id = "SPSubmit";
            this.SPSubmit.ToolTip = "Submit";
            this.SPSubmit.ImageName = "upload_32x32";
            this.SPSubmit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Submit_Execute);
            // 
            // SaveAs
            // 
            this.SPSaveAs.Caption = "SaveAs";
            this.SPSaveAs.Category = "View";
            this.SPSaveAs.Id = "SPSaveAs";
            this.SPSaveAs.ToolTip = "SaveAs";
            this.SPSaveAs.ImageName = "SaveAs_16x16_1";
            this.SPSaveAs.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.SPSaveAs.ConfirmationMessage = "Do you want to save as a new Registration ID?";
            this.SPSaveAs.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveAs_Execute);
            // 
            // Cancel
            // 
            this.SPCancel.Caption = "Cancel";
            this.SPCancel.Category = "View";
            this.SPCancel.Id = "SPCancel";
            this.SPCancel.ToolTip = "Cancel";
            this.SPCancel.ImageName = "State_Validation_Invalid";
            this.SPCancel.ConfirmationMessage = "Do you want to cancel the Registration ID?";
            this.SPCancel.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Cancel_RegistrationID_Execute);
	
 //
            // copy reccurence 
            //
            this.CopyRecurrence.Caption = "Copy Recurrence ";
            this.CopyRecurrence.Id = "CopyRecurrenceId";
            this.CopyRecurrence.ConfirmationMessage = null;
            this.CopyRecurrence.ImageName = "Action_Copy";
            this.CopyRecurrence.ToolTip = null;
            this.CopyRecurrence.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CopyRecurrence_Execute);
            //
            //
            //
            //
            // copyreccurencesave 
            //
            this.CopyReccurenceSave.Caption = "Save";
            this.CopyReccurenceSave.Id = "CopyRecurrenceSave";
            this.CopyReccurenceSave.Category = "CopySave";
            this.CopyReccurenceSave.ConfirmationMessage = null;
            this.CopyReccurenceSave.ToolTip = null;
            this.CopyReccurenceSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CopyReccurenceSave_Execute);
            //
            // 
            // History
            // 
            this.HistoryOfSamplingProposal.Caption = "History";
            this.HistoryOfSamplingProposal.Category = "View";
            this.HistoryOfSamplingProposal.Id = "HistoryOfSamplingProposal";
            this.HistoryOfSamplingProposal.ToolTip =null;
            this.HistoryOfSamplingProposal.ImageName = "Action_Search";
            this.HistoryOfSamplingProposal.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.History_Excecute);

	
            //
            //dateFilter
            //
            this.SamplingProposalDateFilterAction.Caption = "Date Filter";
            this.SamplingProposalDateFilterAction.Category = "View";
            this.SamplingProposalDateFilterAction.ConfirmationMessage = null;
            this.SamplingProposalDateFilterAction.Id = "SamplingProposalDateFilterAction";
            choiceActionItem1.Caption = "3M";
            choiceActionItem1.Id = "3M";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "6M";
            choiceActionItem2.Id = "6M";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "1Y";
            choiceActionItem3.Id = "1Y";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "2Y";
            choiceActionItem4.Id = "2Y";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "All";
            choiceActionItem5.Id = "All";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.SamplingProposalDateFilterAction.Items.Add(choiceActionItem1);
            this.SamplingProposalDateFilterAction.Items.Add(choiceActionItem2);
            this.SamplingProposalDateFilterAction.Items.Add(choiceActionItem3);
            this.SamplingProposalDateFilterAction.Items.Add(choiceActionItem4);
            this.SamplingProposalDateFilterAction.Items.Add(choiceActionItem5);
            this.SamplingProposalDateFilterAction.ToolTip = "Date Filter";

            //
            //HistorydateFilter
            //
            this.SamplingProposalHistoryDateFilterAction.Caption = "Date Filter";
            this.SamplingProposalHistoryDateFilterAction.Category = "View";
            this.SamplingProposalHistoryDateFilterAction.ConfirmationMessage = null;
            this.SamplingProposalHistoryDateFilterAction.Id = "SamplingProposalHistoryDateFilterAction";
            choiceActionItem1.Caption = "3M";
            choiceActionItem1.Id = "3M";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "6M";
            choiceActionItem2.Id = "6M";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "1Y";
            choiceActionItem3.Id = "1Y";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            choiceActionItem4.Caption = "2Y";
            choiceActionItem4.Id = "2Y";
            choiceActionItem4.ImageName = null;
            choiceActionItem4.Shortcut = null;
            choiceActionItem4.ToolTip = null;
            choiceActionItem5.Caption = "All";
            choiceActionItem5.Id = "All";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            this.SamplingProposalHistoryDateFilterAction.Items.Add(choiceActionItem1);
            this.SamplingProposalHistoryDateFilterAction.Items.Add(choiceActionItem2);
            this.SamplingProposalHistoryDateFilterAction.Items.Add(choiceActionItem3);
            this.SamplingProposalHistoryDateFilterAction.Items.Add(choiceActionItem4);
            this.SamplingProposalHistoryDateFilterAction.Items.Add(choiceActionItem5);
            this.SamplingProposalHistoryDateFilterAction.ToolTip = "Date Filter";

            //
            //
            //

            this.Actions.Add(this.SamplingSample);
            this.Actions.Add(this.SamplingAddSample);
            this.Actions.Add(this.SamplingTest);
            this.Actions.Add(this.btnSamplingQuoteImportSamples);
            this.Actions.Add(this.SamplingContainers);
            this.Actions.Add(this.SamplingTestSelectionAdd);
            this.Actions.Add(this.SamplingTestSelectionRemove);
            this.Actions.Add(this.SamplingTestSelectionSave);
            this.Actions.Add(this.SPSubmit);
            this.Actions.Add(this.SPSaveAs);
            this.Actions.Add(this.SPCancel);
            this.Actions.Add(this.SamplingProposalDateFilterAction);
            this.Actions.Add(this.HistoryOfSamplingProposal);
	        this.Actions.Add(this.CopyRecurrence);
            this.Actions.Add(this.CopyReccurenceSave);
            this.Actions.Add(this.SamplingProposalHistoryDateFilterAction);
        }

      

        private DevExpress.ExpressApp.Actions.SimpleAction SamplingSample;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplingAddSample;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplingTest;
        private DevExpress.ExpressApp.Actions.SimpleAction btnSamplingQuoteImportSamples;
        private SimpleAction SamplingContainers;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplingTestSelectionAdd;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplingTestSelectionRemove;
        private DevExpress.ExpressApp.Actions.SimpleAction SamplingTestSelectionSave;
        private DevExpress.ExpressApp.Actions.SimpleAction SPSubmit;
        private DevExpress.ExpressApp.Actions.SimpleAction SPSaveAs;
        private DevExpress.ExpressApp.Actions.SimpleAction SPCancel;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction SamplingProposalDateFilterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction HistoryOfSamplingProposal;
        private DevExpress.ExpressApp.Actions.SimpleAction CopyRecurrence;
        private DevExpress.ExpressApp.Actions.SimpleAction CopyReccurenceSave;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction SamplingProposalHistoryDateFilterAction;

        #endregion
    }
}
