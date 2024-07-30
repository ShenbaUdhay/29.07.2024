
namespace Modules.BusinessObjects.PLM_Control
{
    partial class PLMQualityControlController
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
            this.SelectMode = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.Retrieve = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // SelectMode
            // 
            this.SelectMode.Caption = "Select Mode";
            this.SelectMode.Category = "View";
            this.SelectMode.ConfirmationMessage = null;
            this.SelectMode.Id = "SelectMode";
            choiceActionItem1.Caption = "Intra Dup";
            choiceActionItem1.Id = "Entry 1";
            choiceActionItem1.ImageName = null;
            choiceActionItem1.Shortcut = null;
            choiceActionItem1.ToolTip = null;
            choiceActionItem2.Caption = "Inter Dup";
            choiceActionItem2.Id = "Entry 2";
            choiceActionItem2.ImageName = null;
            choiceActionItem2.Shortcut = null;
            choiceActionItem2.ToolTip = null;
            choiceActionItem3.Caption = "Reference";
            choiceActionItem3.Id = "Entry 3";
            choiceActionItem3.ImageName = null;
            choiceActionItem3.Shortcut = null;
            choiceActionItem3.ToolTip = null;
            this.SelectMode.Items.Add(choiceActionItem1);
            this.SelectMode.Items.Add(choiceActionItem2);
            this.SelectMode.Items.Add(choiceActionItem3);
            this.SelectMode.ToolTip = "Select Mode";
            // 
            // Retrieve
            // 
            this.Retrieve.Caption = "Retrieve";
            this.Retrieve.ConfirmationMessage = null;
            this.Retrieve.Id = "Retrieve";
            this.Retrieve.ToolTip = null;
            // 
            // PLMQualityControlController
            // 
            this.Actions.Add(this.SelectMode);
            this.Actions.Add(this.Retrieve);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction SelectMode;
        private DevExpress.ExpressApp.Actions.SimpleAction Retrieve;
    }
}
