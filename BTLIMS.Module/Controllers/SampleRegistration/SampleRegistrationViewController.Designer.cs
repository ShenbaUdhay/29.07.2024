using System;
using DevExpress.ExpressApp.Actions;

namespace LDM.Module.Controllers.SampleRegistration
{
    partial class SampleRegistrationViewController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem0 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem1 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem2 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem3 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem4 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem5 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem6 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.SampleRegistrationSC_Save = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SampleRegistrationSL_Save = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SR_SLListViewEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SR_SLDetailViewNew = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Sample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddSample = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Test = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveSampleRegistration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestGroup = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestDescription = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnCOC_BarReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnImportSamples = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.btnQuoteImportSamples = new DevExpress.ExpressApp.Actions.SimpleAction (this.components);
            //this.btnCOCImportSamples = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.sampleRegistrationDateFilterAction = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.btnTask_RegistrationReport = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnImportBasicInformationAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImageEditPreviewAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SRHistory = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SentMail = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.COCAttach = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PreInvoice = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.MailContent = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SampleReceipt = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Containers = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PreInvoiceDetails = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_TestEdit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_TestEditAddTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_TestEditRemoveTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btn_TestEditCopyTest = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.SaveAsSampleRegistration = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SampleRegistrationSC_Save
            // 
            this.SampleRegistrationSC_Save.Caption = "Save";
            this.SampleRegistrationSC_Save.Category = "View";
            this.SampleRegistrationSC_Save.ConfirmationMessage = null;
            this.SampleRegistrationSC_Save.Id = "SampleRegistrationSC_Save";
            this.SampleRegistrationSC_Save.ToolTip = null;
            this.SampleRegistrationSC_Save.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SampleRegistrationSC_Save_Execute);
            // 
            // SampleRegistrationSL_Save
            // 
            this.SampleRegistrationSL_Save.Caption = "Save";
            this.SampleRegistrationSL_Save.Category = "View";
            this.SampleRegistrationSL_Save.ConfirmationMessage = null;
            this.SampleRegistrationSL_Save.Id = "SampleRegistrationSL_Save";
            this.SampleRegistrationSL_Save.ToolTip = null;
            this.SampleRegistrationSL_Save.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SampleRegistrationSL_Save_Execute);
            // 
            // SR_SLListViewEdit
            // 
            this.SR_SLListViewEdit.Caption = "Edit";
            this.SR_SLListViewEdit.Category = "Edit";
            this.SR_SLListViewEdit.ConfirmationMessage = null;
            this.SR_SLListViewEdit.Id = "SR_SLListViewEdit";
            this.SR_SLListViewEdit.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SR_SLListViewEdit.ToolTip = null;
            this.SR_SLListViewEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SLListViewEdit_Execute);
            // 
            // SR_SLDetailViewNew
            // 
            this.SR_SLDetailViewNew.Caption = "New";
            this.SR_SLDetailViewNew.Category = "ObjectsCreation";
            this.SR_SLDetailViewNew.ConfirmationMessage = null;
            this.SR_SLDetailViewNew.Id = "SR_SLDetailViewNew";
            this.SR_SLDetailViewNew.ToolTip = null;
            this.SR_SLDetailViewNew.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SLDetailViewNew_Execute);
            // 
            // Sample
            // 
            this.Sample.Caption = "Sample";
            this.Sample.Category = "Sample";
            this.Sample.ConfirmationMessage = null;
            this.Sample.Id = "Sample";
            this.Sample.ToolTip = null;
            this.Sample.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Sample_Execute);
            // 
            // AddSample
            // 
            this.AddSample.Caption = "Add Sample";
            //this.AddSample.Category = "PopupActions";
            this.AddSample.ConfirmationMessage = null;
            this.AddSample.Id = "Add Sample";
            this.AddSample.ToolTip = null;
            this.AddSample.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddSample_Execute);
            // 
            // Test
            // 
            this.Test.Caption = "Test";
            this.Test.Category = "RecordEdit";
            this.Test.ConfirmationMessage = null;
            this.Test.Id = "Test";
            this.Test.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Test.ToolTip = null;
            this.Test.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Test_Execute);
            // 
            // SaveSampleRegistration
            // 
            this.SaveSampleRegistration.Caption = "Save";
            this.SaveSampleRegistration.Category = "View";
            this.SaveSampleRegistration.ConfirmationMessage = null;
            this.SaveSampleRegistration.Id = "SaveSampleRegistration";
            this.SaveSampleRegistration.ToolTip = null;
            this.SaveSampleRegistration.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveSampleRegistration_Execute);
            // 
            // TestGroup
            // 
            this.TestGroup.Caption = "TestGroup";
            this.TestGroup.Category = "RecordEdit";
            this.TestGroup.ConfirmationMessage = null;
            this.TestGroup.Id = "TestGroup";
            this.TestGroup.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.TestGroup.ToolTip = null;
            this.TestGroup.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestGroup_Execute);
            // 
            // TestDescription
            // 
            this.TestDescription.Caption = "TestDescription";
            this.TestDescription.Category = "ListView";
            this.TestDescription.ConfirmationMessage = null;
            this.TestDescription.Id = "TestDescription";
            this.TestDescription.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.TestDescription.ToolTip = null;
            this.TestDescription.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestDescription_Execute);
            // 
            // TestSelectionAdd
            // 
            this.TestSelectionAdd.Caption = "Add";
            this.TestSelectionAdd.Category = "PopupActions";
            this.TestSelectionAdd.ConfirmationMessage = null;
            this.TestSelectionAdd.Id = "TestSelectionAdd";
            this.TestSelectionAdd.ToolTip = null;
            this.TestSelectionAdd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionAdd_Execute);
            // 
            // TestSelectionRemove
            // 
            this.TestSelectionRemove.Caption = "Remove";
            this.TestSelectionRemove.Category = "PopupActions";
            this.TestSelectionRemove.ConfirmationMessage = null;
            this.TestSelectionRemove.Id = "TestSelectionRemove";
            this.TestSelectionRemove.TargetViewId = "";
            this.TestSelectionRemove.ToolTip = null;
            this.TestSelectionRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionRemove_Execute);
            // 
            // TestSelectionSave
            // 
            this.TestSelectionSave.Caption = "Save";
            this.TestSelectionSave.Category = "PopupActions";
            this.TestSelectionSave.ConfirmationMessage = null;
            this.TestSelectionSave.Id = "TestSelectionSave";
            this.TestSelectionSave.ToolTip = null;
            this.TestSelectionSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionSave_Execute);
            // 
            // btnCOC_BarReport
            // 
            this.btnCOC_BarReport.Caption = "COC Report";
            this.btnCOC_BarReport.Category = "Reports";
            this.btnCOC_BarReport.ConfirmationMessage = null;
            this.btnCOC_BarReport.Id = "btnCOC_BarReport";
            this.btnCOC_BarReport.ImageName = "BO_Report";
            this.btnCOC_BarReport.TargetObjectsCriteria = "";
            this.btnCOC_BarReport.TargetObjectsCriteriaMode = DevExpress.ExpressApp.Actions.TargetObjectsCriteriaMode.TrueForAll;
          //  this.btnCOC_BarReport.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.btnCOC_BarReport.ToolTip = null;
            this.btnCOC_BarReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnCOC_BarReport_Execute);
            // 
            // btnImportSamples
            // 
            this.btnImportSamples.AcceptButtonCaption = null;
            this.btnImportSamples.CancelButtonCaption = null;
            this.btnImportSamples.Caption = "Import";
            //this.btnImportSamples.Category = "View";
            this.btnImportSamples.ConfirmationMessage = null;
            this.btnImportSamples.Id = "btnImportSamples";
            this.btnImportSamples.ToolTip = null;
            this.btnImportSamples.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.btnImportSamples_CustomizePopupWindowParams);
            this.btnImportSamples.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.btnImportSamples_Execute);
            // 
            // btnQuoteImportSamples
            // 
            //this.btnQuoteImportSamples.AcceptButtonCaption = null;
            //this.btnQuoteImportSamples.CancelButtonCaption = null;
            this.btnQuoteImportSamples.Caption = "Import Quote";
            this.btnQuoteImportSamples.ImageName = "Down_16x16";
            //this.btnImportSamples.Category = "View";
            this.btnQuoteImportSamples.ConfirmationMessage = null;
            this.btnQuoteImportSamples.Id = "btnQuoteImportSamples";
            this.btnQuoteImportSamples.ToolTip = "Import Quotes";
            //this.btnQuoteImportSamples.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.btnQuoteImportSamples_CustomizePopupWindowParams);
            this.btnQuoteImportSamples.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnQuoteImportSamples_Execute);
            // 
            // btnCOCImportSamples
            // 
            //this.btnCOCImportSamples.AcceptButtonCaption = null;
            //this.btnCOCImportSamples.CancelButtonCaption = null;
            //this.btnCOCImportSamples.Caption = "Import COC";
            //this.btnCOCImportSamples.ImageName = "Down_16x16";
            //this.btnCOCImportSamples.Category = "Unspecified";
            //this.btnCOCImportSamples.ConfirmationMessage = null;
            //this.btnCOCImportSamples.Id = "btnCOCImportSamples";
            //this.btnCOCImportSamples.ToolTip = null;
            //this.btnCOCImportSamples.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.btnCOCImportSamples_CustomizePopupWindowParams);
            //this.btnCOCImportSamples.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.btnCOCImportSamples_Execute);
            // 
            // sampleRegistrationDateFilterAction
            // 
            this.sampleRegistrationDateFilterAction.Caption = "Date Filter";
            this.sampleRegistrationDateFilterAction.Category = "View";
            this.sampleRegistrationDateFilterAction.ConfirmationMessage = null;
            this.sampleRegistrationDateFilterAction.Id = "sampleRegistrationDateFilterAction";
            choiceActionItem0.Caption = "1M";
            choiceActionItem0.Id = "1M";
            choiceActionItem0.ImageName = null;
            choiceActionItem0.Shortcut = null;
            choiceActionItem0.ToolTip = null;
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
            choiceActionItem5.Caption = "5Y";
            choiceActionItem5.Id = "5Y";
            choiceActionItem5.ImageName = null;
            choiceActionItem5.Shortcut = null;
            choiceActionItem5.ToolTip = null;
            choiceActionItem6.Caption = "All";
            choiceActionItem6.Id = "All";
            choiceActionItem6.ImageName = null;
            choiceActionItem6.Shortcut = null;
            choiceActionItem6.ToolTip = null;
            this.sampleRegistrationDateFilterAction.Items.Add(choiceActionItem0);
            this.sampleRegistrationDateFilterAction.Items.Add(choiceActionItem1);
            this.sampleRegistrationDateFilterAction.Items.Add(choiceActionItem2);
            this.sampleRegistrationDateFilterAction.Items.Add(choiceActionItem3);
            this.sampleRegistrationDateFilterAction.Items.Add(choiceActionItem4);
            this.sampleRegistrationDateFilterAction.Items.Add(choiceActionItem5);
            this.sampleRegistrationDateFilterAction.Items.Add(choiceActionItem6);
            this.sampleRegistrationDateFilterAction.ToolTip = "Date Filter";
            // 
            // btnTask_RegistrationReport
            // 
            this.btnTask_RegistrationReport.Caption = "Task Registration Report";
            this.btnTask_RegistrationReport.Category = "Reports";
            this.btnTask_RegistrationReport.ConfirmationMessage = null;
            this.btnTask_RegistrationReport.Id = "btnTask_RegistrationReport";
            this.btnTask_RegistrationReport.ImageName = "Action_Export_ToDOCX";
            this.btnTask_RegistrationReport.ToolTip = null;
            this.btnTask_RegistrationReport.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnTask_RegistrationReport_Execute);
            // 
            // btnImportBasicInformationAction
            // 
            this.btnImportBasicInformationAction.Caption = "Import Basic Information";
            this.btnImportBasicInformationAction.Category = "Reports";
            this.btnImportBasicInformationAction.ConfirmationMessage = null;
            this.btnImportBasicInformationAction.Id = "btnImportBasicInformationAction";
            this.btnImportBasicInformationAction.ImageName = "";
            this.btnImportBasicInformationAction.ToolTip = null;
            this.btnImportBasicInformationAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.btnImportBasicInformationAction_Execute);
            // 
            // ImageEditPreviewAction
            // 
            this.ImageEditPreviewAction.Caption = "Preview";
            this.ImageEditPreviewAction.Category = "RecordEdit";
            this.ImageEditPreviewAction.ConfirmationMessage = null;
            this.ImageEditPreviewAction.ImageName = "Action_Export_ToImage";
            this.ImageEditPreviewAction.Id = "ImageEditPreviewAction";
            this.ImageEditPreviewAction.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ImageEditPreviewAction.ToolTip = "Preview";
            this.ImageEditPreviewAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImageEditPreviewAction_Execute);
            // 
            // SRHistory
            // 
            this.SRHistory.Caption = "History";
            this.SRHistory.ConfirmationMessage = null;
            this.SRHistory.Id = "SRHistory";
            this.SRHistory.ImageName = "Action_Search";
            this.SRHistory.ToolTip = null;
            this.SRHistory.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SRHistory_Execute);
            // 
            // SentMail
            // 
            this.SentMail.Caption = "Send";
            this.SentMail.Category = "View";
            this.SentMail.ConfirmationMessage = null;
            this.SentMail.Id = "SentMail1";
            this.SentMail.ImageName = "SendMail_16x16";
            this.SentMail.ToolTip = null;
            this.SentMail.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SentMail_Execute);
            // 
            // COCAttach
            // 
            this.COCAttach.Caption = "COC...";
            this.COCAttach.Category = "RecordEdit";
            this.COCAttach.ConfirmationMessage = null;
            this.COCAttach.Id = "COC";
            this.COCAttach.ImageName = "BO_Folder";
            this.COCAttach.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.COCAttach.ToolTip = null;
            this.COCAttach.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.COC_Execute);
            // 
            // PreInvoice
            // 
            this.PreInvoice.Caption = "Pre Invoice";
            this.PreInvoice.Category = "View";
            this.PreInvoice.ConfirmationMessage = null;
            this.PreInvoice.Id = "PreInvoice";
            this.PreInvoice.ImageName = "BO_Invoice";
            this.PreInvoice.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.PreInvoice.ToolTip = null;
            this.PreInvoice.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PreInvoiceReport_Execute);
            // 
            // MailContent
            // 
            this.MailContent.Caption = "Mail Content";
            this.MailContent.Category = "ListView";
            this.MailContent.ConfirmationMessage = null;
            this.MailContent.Id = "MailContent";
            this.MailContent.ImageName = "Mail_16x16";
            this.MailContent.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.MailContent.ToolTip = null;
            this.MailContent.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.MailContent_Execute);
            // 
            // SampleReceipt
            // 
            this.SampleReceipt.Caption = "Sample Receipt";
            this.SampleReceipt.Category = "RecordEdit";
            this.SampleReceipt.ConfirmationMessage = null;
            this.SampleReceipt.Id = "SampleReceipt";
            this.SampleReceipt.ImageName = "Action_Report_Object_Inplace_Preview";
            this.SampleReceipt.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.SampleReceipt.ToolTip = null;
            this.SampleReceipt.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SampleReceipt_Execute);
            // 
            // Containers
            // 
            this.Containers.Caption = "Cont";
            this.Containers.Category = "ListView";
            this.Containers.ConfirmationMessage = null;
            this.Containers.Id = "Containers";
            this.Containers.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.Containers.ToolTip = null;
            this.Containers.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Containers_Execute);
            // 
            // Invoice
            // 
            this.PreInvoiceDetails.Caption = "Invoice";
            this.PreInvoiceDetails.Category = "Sample";
            this.PreInvoiceDetails.ConfirmationMessage = null;
            this.PreInvoiceDetails.Id = "PreInvoiceDetails";
            this.PreInvoiceDetails.ToolTip = null;
            this.PreInvoiceDetails.Execute += Btn_PreInvoiceDetails_Execute;
            // 
            // SampleRegistrationViewController
            // 
            // 
            // btn_TestEdit
            // 
            this.btn_TestEdit.Caption = "Test Edit";
            this.btn_TestEdit.ImageName = "Action_Edit";
            this.btn_TestEdit.ConfirmationMessage = null;
            this.btn_TestEdit.Id = "Test_Edit";
            this.btn_TestEdit.ToolTip = null;
            this.btn_TestEdit.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestEdit_Execute);
            //
            // 
            // btn_TestEditAddTest
            // 
            this.btn_TestEditAddTest.Caption = "Add Test";
            this.btn_TestEditAddTest.ImageName = "Add";
            //this.btn_TestEditAddTest.Category = "TestEditAddTest";
            this.btn_TestEditAddTest.ConfirmationMessage = null;
            this.btn_TestEditAddTest.Id = "TestEditAddTest";
            this.btn_TestEditAddTest.ToolTip = null;
            this.btn_TestEditAddTest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestEditAddTest_Execute);
            // 
            // btn_TestEditRemoveTest
            // 
            this.btn_TestEditRemoveTest.Caption = "Remove Test";
            this.btn_TestEditRemoveTest.ImageName = "Remove";
            //this.btn_TestEditRemoveTest.Category = "TestEditRemoveTest";
            this.btn_TestEditRemoveTest.ConfirmationMessage = null;
            this.btn_TestEditRemoveTest.Id = "TestEditRemoveTest";
            this.btn_TestEditRemoveTest.ToolTip = null;
            this.btn_TestEditRemoveTest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestEditRemoveTest_Execute);
            //this.btn_TestEditRemoveTest.Executing += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestEditRemoveTest_Executing);
            // 
            // btn_TestEditCopyTest
            // 
            this.btn_TestEditCopyTest.Caption = "Copy Tests";
            this.btn_TestEditCopyTest.ImageName = "Action_Copy";
            //this.btn_TestEditCopyTest.Category = "TestEditCopyTest";
            this.btn_TestEditCopyTest.ConfirmationMessage = null;
            this.btn_TestEditCopyTest.Id = "TestEditCopyTest";
            this.btn_TestEditCopyTest.ToolTip = null;
            this.btn_TestEditCopyTest.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestEditCopyTest_Execute);
            // 
            // SaveAs
            // 
            this.SaveAsSampleRegistration.Caption = "Save As";
            this.SaveAsSampleRegistration.Category = "View";
            this.SaveAsSampleRegistration.Id = "SaveAsSampleRegistration";
            this.SaveAsSampleRegistration.ToolTip = "Save As";
            this.SaveAsSampleRegistration.ImageName = "SaveAs_16x16_1";
            this.SaveAsSampleRegistration.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            this.SaveAsSampleRegistration.ConfirmationMessage = "Do you want to save as a new Job ID?";
            this.SaveAsSampleRegistration.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.SaveAs_Execute);
            //
            //Add the buttons created here
            //
            this.Actions.Add(this.SampleRegistrationSC_Save);
            this.Actions.Add(this.SampleRegistrationSL_Save);
            this.Actions.Add(this.SR_SLListViewEdit);
            this.Actions.Add(this.SR_SLDetailViewNew);
            this.Actions.Add(this.Sample);
            this.Actions.Add(this.AddSample);
            this.Actions.Add(this.Test);
            this.Actions.Add(this.SaveSampleRegistration);
            this.Actions.Add(this.TestGroup);
            this.Actions.Add(this.TestDescription);
            this.Actions.Add(this.TestSelectionAdd);
            this.Actions.Add(this.TestSelectionRemove);
            this.Actions.Add(this.TestSelectionSave);
            this.Actions.Add(this.btnCOC_BarReport);
            this.Actions.Add(this.btnImportSamples);
            this.Actions.Add(this.btnQuoteImportSamples);
            //this.Actions.Add(this.btnCOCImportSamples);
            this.Actions.Add(this.sampleRegistrationDateFilterAction);
            this.Actions.Add(this.btnTask_RegistrationReport);
            this.Actions.Add(this.btnImportBasicInformationAction);
            this.Actions.Add(this.ImageEditPreviewAction);
            this.Actions.Add(this.SRHistory);
            this.Actions.Add(this.SentMail);
            this.Actions.Add(this.COCAttach);
            this.Actions.Add(this.PreInvoice);
            this.Actions.Add(this.MailContent);
            this.Actions.Add(this.SampleReceipt);
            this.Actions.Add(this.Containers);
            this.Actions.Add(this.PreInvoiceDetails);
            this.Actions.Add(this.btn_TestEdit);
            this.Actions.Add(this.btn_TestEditAddTest);
            this.Actions.Add(this.btn_TestEditRemoveTest);
            this.Actions.Add(this.btn_TestEditCopyTest);
            this.Actions.Add(this.SaveAsSampleRegistration);
            this.ViewControlsCreated += new System.EventHandler(this.SampleRegistrationViewController_ViewControlsCreated);

        }
        #endregion


        // Create buttons here
        private DevExpress.ExpressApp.Actions.SimpleAction SampleRegistrationSC_Save;
        private DevExpress.ExpressApp.Actions.SimpleAction SampleRegistrationSL_Save;
        private DevExpress.ExpressApp.Actions.SimpleAction SR_SLListViewEdit;
        private DevExpress.ExpressApp.Actions.SimpleAction SR_SLDetailViewNew;
        private DevExpress.ExpressApp.Actions.SimpleAction Sample;
        private DevExpress.ExpressApp.Actions.SimpleAction AddSample;
        private DevExpress.ExpressApp.Actions.SimpleAction Test;
        private DevExpress.ExpressApp.Actions.SimpleAction SaveSampleRegistration;
        private DevExpress.ExpressApp.Actions.SimpleAction TestGroup;
        private DevExpress.ExpressApp.Actions.SimpleAction TestDescription;
		private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionAdd;
        private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionRemove;
        private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionSave;
        private DevExpress.ExpressApp.Actions.SimpleAction btnCOC_BarReport;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction btnImportSamples;
        private DevExpress.ExpressApp.Actions.SimpleAction btnQuoteImportSamples;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction btnCOCImportSamples;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction sampleRegistrationDateFilterAction;
        private DevExpress.ExpressApp.Actions.SimpleAction btnTask_RegistrationReport;
        private DevExpress.ExpressApp.Actions.SimpleAction btnImportBasicInformationAction;
        private DevExpress.ExpressApp.Actions.SimpleAction ImageEditPreviewAction;
        private SimpleAction SRHistory;
        private SimpleAction SentMail;
        private SimpleAction COCAttach;
        private SimpleAction PreInvoice;
        private SimpleAction MailContent;
        private SimpleAction SampleReceipt;
        private SimpleAction Containers;
        private SimpleAction PreInvoiceDetails;
        private SimpleAction btn_TestEdit;
        private SimpleAction btn_TestEditAddTest;
        private SimpleAction btn_TestEditRemoveTest;
        private SimpleAction btn_TestEditCopyTest;
        private SimpleAction SaveAsSampleRegistration;
    }
}
