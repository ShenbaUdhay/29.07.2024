
namespace LDM.Module.Web.Controllers.Template_Builder
{
    partial class TBViewController
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
            this.FullScreen = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AddTB = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RemoveTB = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestSelectionSave = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.EditTemplate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // FullScreen
            // 
            this.FullScreen.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept;
            this.FullScreen.Caption = "FullScreen";
            this.FullScreen.ConfirmationMessage = null;
            this.FullScreen.Id = "FullScreen";
            this.FullScreen.ToolTip = null;
            // 
            // AddTB
            // 
            this.AddTB.Caption = "Add";
            this.AddTB.ImageName= "Add_16x16";
            this.AddTB.ConfirmationMessage = null;
            this.AddTB.Id = "AddTB";
            this.AddTB.ToolTip = null;
            this.AddTB.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AddTB_Execute);
            // 
            // RemoveTB
            // 
            this.RemoveTB.Caption = "Remove";
            this.RemoveTB.ImageName = "Remove";
            this.RemoveTB.ConfirmationMessage = null;
            this.RemoveTB.Id = "RemoveTB";
            this.RemoveTB.ToolTip = null;
            this.RemoveTB.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RemoveTB_Execute);
            // 
            // TestSelectionAdd
            // 
            this.TestSelectionAdd.Caption = "Add";
            this.TestSelectionAdd.Category = "PopupActions";
            this.TestSelectionAdd.ConfirmationMessage = null;
            this.TestSelectionAdd.Id = "TBTestSelectionAdd";
            this.TestSelectionAdd.ToolTip = null;
            this.TestSelectionAdd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionAdd_Execute);
            // 
            // TestSelectionRemove
            // 
            this.TestSelectionRemove.Caption = "Remove";
            this.TestSelectionRemove.Category = "PopupActions";
            this.TestSelectionRemove.ConfirmationMessage = null;
            this.TestSelectionRemove.Id = "TBTestSelectionRemove";
            this.TestSelectionRemove.TargetViewId = "";
            this.TestSelectionRemove.ToolTip = null;
            this.TestSelectionRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionRemove_Execute);
            // 
            // TestSelectionSave
            // 
            this.TestSelectionSave.Caption = "OK";
            this.TestSelectionSave.Category = "PopupActions";
            this.TestSelectionSave.ConfirmationMessage = null;
            this.TestSelectionSave.Id = "TBTestSelectionSave";
            this.TestSelectionSave.ToolTip = null;
            this.TestSelectionSave.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestSelectionSave_Execute);
            // 
            // EditTemplate
            // 
            this.EditTemplate.Caption = "Edit";
            this.EditTemplate.Category = "RecordEdit";
            this.EditTemplate.ConfirmationMessage = null;
            this.EditTemplate.Id = "EditTemplate";
            this.EditTemplate.ImageName = "";
            this.EditTemplate.ToolTip = null;
            this.EditTemplate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.EditTemplate_Execute);
            // 
            // TBViewController
            // 
            this.Actions.Add(this.FullScreen);
            this.Actions.Add(this.AddTB);
            this.Actions.Add(this.RemoveTB);
            this.Actions.Add(this.TestSelectionAdd);
            this.Actions.Add(this.TestSelectionRemove);
            this.Actions.Add(this.TestSelectionSave);
            this.Actions.Add(this.EditTemplate);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction FullScreen;
        private DevExpress.ExpressApp.Actions.SimpleAction AddTB;
        private DevExpress.ExpressApp.Actions.SimpleAction RemoveTB;
        private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionAdd;
        private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionRemove;
        private DevExpress.ExpressApp.Actions.SimpleAction TestSelectionSave;
        private DevExpress.ExpressApp.Actions.SimpleAction EditTemplate;
    }
}
