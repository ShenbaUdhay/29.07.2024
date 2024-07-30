
namespace LDM.Module.Controllers.Settings
{
    partial class TabFieldConfigurationViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

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
            this.btnTestConfigAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.btnTestConfigRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // btnTestConfigAdd
            // 
            this.btnTestConfigAdd.Caption = "Add";
            this.btnTestConfigAdd.Category = "TestfieldsAddRemove";
            this.btnTestConfigAdd.ConfirmationMessage = null;
            this.btnTestConfigAdd.Id = "btnTestConfigAdd";
            this.btnTestConfigAdd.ImageName = "Add";
            this.btnTestConfigAdd.ToolTip = null;
            this.btnTestConfigAdd.Execute += BtnTestConfigAdd_Execute;
            // 
            // btnTestConfigRemove
            // 
            this.btnTestConfigRemove.Caption = "Remove";
            this.btnTestConfigRemove.Category = "TestfieldsAddRemove";
            this.btnTestConfigRemove.ConfirmationMessage = null;
            this.btnTestConfigRemove.Id = "btnTestConfigRemove";
            this.btnTestConfigRemove.ImageName = "Remove";
            this.btnTestConfigRemove.ToolTip = null;
            this.btnTestConfigRemove.Execute += BtnTestConfigRemove_Execute;
            // 
            // TestFieldConfigurationViewController
            // 
            this.Actions.Add(this.btnTestConfigAdd);
            this.Actions.Add(this.btnTestConfigRemove);

        }


        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction btnTestConfigAdd;
        private DevExpress.ExpressApp.Actions.SimpleAction btnTestConfigRemove;
    }
}
