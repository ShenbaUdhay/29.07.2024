namespace ALPACpre.Module.Controllers.ICM
{
    partial class ICMLTReportController
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
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem11 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            DevExpress.ExpressApp.Actions.ChoiceActionItem choiceActionItem12 = new DevExpress.ExpressApp.Actions.ChoiceActionItem();
            this.LTReportAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            //this.LTBarcodeRpt = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            //this.FromDate = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            //this.ToDate = new DevExpress.ExpressApp.Actions.ParametrizedAction(this.components);
            // 
            // LTReportAction
            // 
            this.LTReportAction.Caption = "Label Report";
            this.LTReportAction.ConfirmationMessage = null;
            this.LTReportAction.Id = "LTReportAction";
            this.LTReportAction.ImageName = "BOReport_16x16";
            this.LTReportAction.ToolTip = null;
            this.LTReportAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LTReportAction_Execute);
            // 
            // LTBarcodeRpt
            // 
            //this.LTBarcodeRpt.Caption = " LTBarcodeReport";
            //this.LTBarcodeRpt.ConfirmationMessage = null;
            //this.LTBarcodeRpt.Id = "LTBarcodeRpt";
            //choiceActionItem11.Caption = "LTBarCode";
            //choiceActionItem11.Id = "LTBarCode";
            //choiceActionItem11.ImageName = null;
            //choiceActionItem11.Shortcut = null;
            //choiceActionItem11.ToolTip = null;
            //choiceActionItem12.Caption = "LTBarCodeBig";
            //choiceActionItem12.Id = "LTBarCodeBig";
            //choiceActionItem12.ImageName = null;
            //choiceActionItem12.Shortcut = null;
            //choiceActionItem12.ToolTip = null;
            //this.LTBarcodeRpt.Items.Add(choiceActionItem11);
            //this.LTBarcodeRpt.Items.Add(choiceActionItem12);
            //this.LTBarcodeRpt.ToolTip = null;
            // 
            // FromDate
            // 
            //this.FromDate.Category = "View";
            //this.FromDate.ConfirmationMessage = null;
            //this.FromDate.Id = "FromDate";
            //this.FromDate.NullValuePrompt = null;
            //this.FromDate.ShortCaption = null;
            //this.FromDate.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            //this.FromDate.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            //this.FromDate.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            //this.FromDate.ValueType = typeof(System.DateTime);
            //// 
            //// ToDate
            //// 
            //this.ToDate.Caption = "To Date";
            //this.ToDate.ConfirmationMessage = null;
            //this.ToDate.Id = "ToDate";
            //this.ToDate.NullValuePrompt = null;
            //this.ToDate.ShortCaption = null;
            //this.ToDate.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            //this.ToDate.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            //this.ToDate.ValueType = typeof(System.DateTime);
            // 
            // ICMLTReportController
            // 
            this.Actions.Add(this.LTReportAction);
            //this.Actions.Add(this.LTBarcodeRpt);
            //this.Actions.Add(this.FromDate);
            //this.Actions.Add(this.ToDate);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction LTReportAction;
        //private DevExpress.ExpressApp.Actions.SingleChoiceAction LTBarcodeRpt;
        //private DevExpress.ExpressApp.Actions.ParametrizedAction FromDate;
        //private DevExpress.ExpressApp.Actions.ParametrizedAction ToDate;
        //private DevExpress.ExpressApp.Actions.SingleChoiceAction LTBarcodeReport;
    }
}
