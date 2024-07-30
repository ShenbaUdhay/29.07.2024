
namespace LDM.Module.Controllers.SamplingManagement
{
    partial class SampleTransferViewController
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
            this.Submit = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Rollback = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.STDelete = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Submit
            // 
            this.STDelete.Caption = "Delete";
            this.STDelete.ConfirmationMessage = null;
            this.STDelete.Id = "STDelete";
            this.STDelete.ToolTip = null;
            // 
            // Submit
            // 
            this.Submit.Caption = "Submit";
            this.Submit.ConfirmationMessage = null;
            this.Submit.Id = "STSubmit";
            this.Submit.ImageName = "Submit_image";
            this.Submit.ToolTip = null;
            // 
            // Rollback
            // 
            this.Rollback.Caption = "Rollback";
            this.Rollback.ConfirmationMessage = null;
            this.Rollback.Id = "STRollback";
            this.Rollback.ToolTip = null;
            // 
            // sampleRegistrationDateFilterAction
            // 
            this.STFilter.Caption = "Date Filter";
            this.STFilter.Category = "View";
            this.STFilter.ConfirmationMessage = null;
            this.STFilter.Id = "STFilter";
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
            // SampleTransferViewController
            // 
            this.Actions.Add(this.Submit);
            this.Actions.Add(this.Rollback);
            this.Actions.Add(this.STFilter);
            this.Actions.Add(this.STDelete);
        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Submit;
        private DevExpress.ExpressApp.Actions.SimpleAction STDelete;
        private DevExpress.ExpressApp.Actions.SimpleAction Rollback;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction STFilter;
    }
}
