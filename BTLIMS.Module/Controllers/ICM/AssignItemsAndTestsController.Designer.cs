
namespace LDM.Module.Controllers.ICM
{
    partial class AssignItemsAndTestsController
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
            this.AssignItemAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.AssignItemRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AssignMethodAdd = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.AssignMethodRemove = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);

            // AssignItemAdd
            // 
            this.AssignItemAdd.Caption = "Add";
            this.AssignItemAdd.Category = "Edit";
            this.AssignItemAdd.ConfirmationMessage = null;
            this.AssignItemAdd.Id = "AssignItemAdd";
            this.AssignItemAdd.ImageName = "Add_16x16";
            this.AssignItemAdd.TargetViewId = "Items_Linkparameters_ListView;";
            this.AssignItemAdd.ToolTip = null;
            this.AssignItemAdd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AssignItemAdd_Execute);
            //// 

            //// AssignItemRemove
            //// 
            //this.AssignItemRemove.Caption = "Remove";
            //this.AssignItemRemove.Category = "Edit";
            //this.AssignItemRemove.ConfirmationMessage = null;
            //this.AssignItemRemove.Id = "AssignItemRemove";
            //this.AssignItemRemove.TargetViewId = "Items_Linkparameters_ListView;";
            //this.AssignItemRemove.ToolTip = null;
            //this.AssignItemRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AssignItemRemove_Execute);
            // 

            // AssignMethodAdd
            // 
            this.AssignMethodAdd.Caption = "Add";
            this.AssignMethodAdd.Category = "Edit";
            this.AssignMethodAdd.ConfirmationMessage = null;
            this.AssignMethodAdd.Id = "AssignMethodAdd";
            this.AssignMethodAdd.ImageName = "Add_16x16";
            this.AssignMethodAdd.TargetViewId = "TestMethod_Linkparameters_ListView;";
            this.AssignMethodAdd.ToolTip = null;
            this.AssignMethodAdd.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AssignMethodAdd_Execute);
            //// 

            //// AssignMethodRemove
            //// 
            //this.AssignMethodRemove.Caption = "Remove";
            //this.AssignMethodRemove.Category = "Edit";
            //this.AssignMethodRemove.ConfirmationMessage = null;
            //this.AssignMethodRemove.Id = "AssignMethodRemove";
            //this.AssignMethodRemove.TargetViewId = "TestMethod_Linkparameters_ListView;";
            //this.AssignMethodRemove.ToolTip = null;
            //this.AssignMethodRemove.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AssignMethodRemove_Execute);
            // 

            // AssignItemsAndTestsController
            // 
            this.Actions.Add(this.AssignItemAdd);
            //this.Actions.Add(this.AssignItemRemove);
            this.Actions.Add(this.AssignMethodAdd);
            //this.Actions.Add(this.AssignMethodRemove); 

        }

        #endregion
        //private DevExpress.ExpressApp.Actions.SimpleAction AssignItemRemove;
        private DevExpress.ExpressApp.Actions.SimpleAction AssignItemAdd;
        //private DevExpress.ExpressApp.Actions.SimpleAction AssignMethodRemove;
        private DevExpress.ExpressApp.Actions.SimpleAction AssignMethodAdd;
    }
}
