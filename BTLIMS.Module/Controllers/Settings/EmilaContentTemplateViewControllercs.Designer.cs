
using DevExpress.ExpressApp.Actions;
using System;

namespace LDM.Module.Controllers.Settings
{
    partial class EmilaContentTemplateViewControllercs
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
            this.Addvalues = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);


            this.Addvalues.Caption = "Add";
            this.Addvalues.Category = "colAddDatasourceValue";
            this.Addvalues.ConfirmationMessage = null;
            this.Addvalues.Id = "AddValues";
            this.Addvalues.ToolTip = "Add";
            this.Addvalues.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Addvalues_Execute);

            this.Actions.Add(this.Addvalues);

        }

       

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction Addvalues;

    }
}
