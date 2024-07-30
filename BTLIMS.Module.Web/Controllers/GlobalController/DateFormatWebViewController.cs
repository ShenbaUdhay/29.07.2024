using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Web.Configuration;

namespace LDM.Module.Controllers.Public
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DateFormat : ViewController
    {
        MessageTimer timer = new MessageTimer();
        #region Constructor
        public DateFormat()
        {
            InitializeComponent();
            TargetViewId = "Samplecheckin_ListView;" + "SampleLogIn_ListView;" + "SampleParameter_ListView_Copy_ResultEntry;" + "SampleParameter_ListView_Copy_ResultValidation;" +
                "SampleParameter_ListView_Copy_ResultView;" + "SampleParameter_ListView_Copy_ResultApproval;" + "Samplecheckin_DetailView;" + "Reporting_ListView_Copy_ReportView;" +
                "Reporting_ListView_Copy_ReportApproval;" + "Reporting_ListView;" + "Requisition_ListView;" + "Requisition_DetailView;" + "Requisition_DetailView_Receive;" +
                "Requisition_ListView_Approve;" + "Requisition_ListView_Receive;" + "Requisition_ListView_Review;" + "Purchaseorder_Item_ListView;" + "Items_DetailView;" +
                "Items_ListView;" + "Purchaseorder_DetailView;" + "Purchaseorder_DetailView_Approve;" + "Purchaseorder_ListView;" + "Purchaseorder_ListView_Approve;" +
                "Requisition_ListView_Purchaseorder_ViewMode;" + "Requisition_ListView_Purchaseorder_Mainview;" + "Samplecheckin_ListView_Copy_Registration;" + "Samplecheckin_DetailView_Copy_SampleRegistration;" + "Samplecheckin_DetailView_Copy_RegistrationSigningOff;"
                + "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview;" + "SampleParameter_ListView_Copy_ResultApproval_Level2Review;" + "Samplecheckin_ListView_Copy_RegistrationSigningOff;" + "Samplecheckin_ListView_Copy_RegistrationSigningOff_History;" + "SampleParameter_ListView_Copy_ResultEntry_SingleChoice;";
        }
        #endregion

        #region DeaultMethods
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View != null && View is ListView && View.Id == "Requisition_ListView" || View.Id == "Requisition_ListView_Approve" || View.Id == "Requisition_ListView_Receive" || View.Id == "Requisition_ListView_Review" || View.Id == "Purchaseorder_Item_ListView")
                {
                    //ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //ASPxGridView gv = gridListEditor.Grid;
                    //object objRequestedDate;
                    //objRequestedDate = null;
                    //if (gv != null)
                    //{
                    //    objRequestedDate = gv.DataColumns["RequestedDate"];
                    //}
                    //if (objRequestedDate != null && gv.DataColumns["RequestedDate"].Index != -1)
                    //{
                    //    gv.DataColumns["RequestedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    //}
                }
                if (View != null && View is ListView && View.Id == "Requisition_ListView" || View.Id == "Requisition_ListView_Approve" || View.Id == "Requisition_ListView_Receive" || View.Id == "Purchaseorder_Item_ListView" || View.Id == "Requisition_ListView_Review")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objReviewedDate;
                    objReviewedDate = null;
                    if (gv != null)
                    {
                        objReviewedDate = gv.DataColumns["ReviewedDate"];
                    }
                    if (objReviewedDate != null && gv.DataColumns["ReviewedDate"].Index != -1)
                    {
                        gv.DataColumns["ReviewedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }

                if (View != null && View is ListView && View.Id == "Requisition_ListView" || View.Id == "Requisition_ListView_Receive" || View.Id == "Purchaseorder_Item_ListView" || View.Id == "Requisition_ListView_Approve")
                {

                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objApprovedDate;
                    objApprovedDate = null;
                    if (gv != null)
                    {
                        objApprovedDate = gv.DataColumns["ApprovedDate"];
                    }
                    if (objApprovedDate != null && gv.DataColumns["ApprovedDate"].Index != -1)
                    {
                        gv.DataColumns["ApprovedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View != null && View is ListView && View.Id == "Items_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objRetireDate;
                    objRetireDate = null;
                    if (gv != null)
                    {
                        objRetireDate = gv.DataColumns["RetireDate"];
                    }
                    if (objRetireDate != null && gv.DataColumns["RetireDate"].Index != -1)
                    {
                        gv.DataColumns["RetireDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View is DetailView && View != null && View.Id == "Items_DetailView")
                {
                    var obj = ((DetailView)View).FindItem("RetireDate");
                    if (obj != null)
                    {
                        PropertyEditor editor = ((PropertyEditor)obj);
                        editor.DisplayFormat = WebConfigurationManager.AppSettings["DateFormat"];
                        editor.EditMask = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View != null && View is ListView && View.Id == "Purchaseorder_ListView" || View.Id == "Purchaseorder_ListView_Approve")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objPurchaseDate;
                    objPurchaseDate = null;
                    if (gv != null)
                    {
                        objPurchaseDate = gv.DataColumns["PurchaseDate"];
                    }
                    if (objPurchaseDate != null && gv.DataColumns["PurchaseDate"].Index != -1)
                    {
                        gv.DataColumns["PurchaseDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View != null && View is ListView && View.Id == "Requisition_ListView_Purchaseorder_ViewMode")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objPurchaseDate;
                    objPurchaseDate = null;
                    if (gv != null)
                    {
                        objPurchaseDate = gv.DataColumns["OrderDate"];
                    }
                    if (objPurchaseDate != null && gv.DataColumns["OrderDate"].Index != -1)
                    {
                        gv.DataColumns["OrderDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View != null && View is ListView && View.Id == "Purchaseorder_ListView" || View.Id == "Purchaseorder_ListView_Approve")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objApprovedDate;
                    objApprovedDate = null;
                    if (gv != null)
                    {
                        objApprovedDate = gv.DataColumns["ApprovedDate"];
                    }
                    if (objApprovedDate != null && gv.DataColumns["ApprovedDate"].Index != -1)
                    {
                        gv.DataColumns["ApprovedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View is DetailView && View != null && View.Id == "Purchaseorder_DetailView" || View.Id == "Purchaseorder_DetailView_Approve")
                {
                    var obj = ((DetailView)View).FindItem("PurchaseDate");
                    if (obj != null)
                    {
                        PropertyEditor editor = ((PropertyEditor)obj);
                        editor.DisplayFormat = WebConfigurationManager.AppSettings["DateFormat"];
                        editor.EditMask = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }

                if (View is DetailView && View != null && View.Id == "Requisition_DetailView" || View.Id == "Requisition_DetailView_Receive")
                {
                    var obj = ((DetailView)View).FindItem("RequestedDate");
                    if (obj != null)
                    {
                        PropertyEditor editor = ((PropertyEditor)obj);
                        editor.DisplayFormat = WebConfigurationManager.AppSettings["DateFormat"];
                        editor.EditMask = WebConfigurationManager.AppSettings["DateFormat"];
                    }

                }
                if (View != null && View is ListView && (View.Id == "Samplecheckin_ListView" || View.Id == "SampleLogIn_ListView" || View.Id == "SampleParameter_ListView_Copy_ResultEntry" ||
                    View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_ResultApproval" ||
                    View.Id == "Samplecheckin_ListView_Copy_Registration" ||
                    View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" ||
                    View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review" ||
                    View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff" ||
                    View.Id == "Samplecheckin_ListView_Copy_RegistrationSigningOff_History"))
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objReceivedDate;
                    objReceivedDate = null;
                    if (gv != null)
                    {
                        objReceivedDate = gv.DataColumns["ReceivedDate"];
                    }
                    if (objReceivedDate != null && gv.DataColumns["ReceivedDate"].Index != -1)
                    {
                        gv.DataColumns["ReceivedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View.Id == "SampleParameter_ListView_Copy_ResultEntry_SingleChoice")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objReceivedDate;
                    objReceivedDate = null;
                    if (gv != null)
                    {
                        objReceivedDate = gv.DataColumns["SCDateReceived"];
                    }
                    if (objReceivedDate != null && gv.DataColumns["SCDateReceived"].Index != -1)
                    {
                        gv.DataColumns["SCDateReceived"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View is DetailView && View != null && (View.Id == "Samplecheckin_DetailView" || View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration" || View.Id == "Samplecheckin_DetailView_Copy_RegistrationSigningOff"))
                {
                    var obj = ((DetailView)View).FindItem("RecievedDate");
                    if (obj != null)
                    {
                        PropertyEditor editor = ((PropertyEditor)obj);
                        editor.DisplayFormat = WebConfigurationManager.AppSettings["DateFormat"];
                        editor.EditMask = WebConfigurationManager.AppSettings["DateFormat"];
                    }

                }
                if (View != null && View is ListView && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" ||
                  View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                  || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" ||
                    View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"))
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objAnalyzedDate;
                    objAnalyzedDate = null;
                    if (gv != null)
                    {
                        objAnalyzedDate = gv.DataColumns["AnalyzedDate"];
                    }

                    if (objAnalyzedDate != null && gv.DataColumns["AnalyzedDate"].Index != -1)
                    {
                        gv.DataColumns["AnalyzedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View != null && View is ListView && (View.Id == "SampleParameter_ListView_Copy_ResultEntry" ||
                  View.Id == "SampleParameter_ListView_Copy_ResultValidation" || View.Id == "SampleParameter_ListView_Copy_ResultView"
                  || View.Id == "SampleParameter_ListView_Copy_ResultApproval" ||
                   View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" ||
                    View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"))
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objAnalyzedDate;
                    objAnalyzedDate = null;
                    if (gv != null)
                    {
                        objAnalyzedDate = gv.DataColumns["EnteredDate"];
                    }

                    if (objAnalyzedDate != null && gv.DataColumns["EnteredDate"].Index != -1)
                    {
                        gv.DataColumns["EnteredDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View != null && View is ListView && (View.Id == "SampleParameter_ListView_Copy_ResultValidation"
                    || View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_ResultApproval"
                    || View.Id == "SampleParameter_ListView_Copy_ResultValidation_Leve1lReview" ||
                    View.Id == "SampleParameter_ListView_Copy_ResultApproval_Level2Review"))
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objValidatedDate;
                    objValidatedDate = null;
                    if (gv != null)
                    {
                        objValidatedDate = gv.DataColumns["ValidatedDate"];
                    }
                    if (objValidatedDate != null && gv.DataColumns["ValidatedDate"].Index != -1)
                    {
                        gv.DataColumns["ValidatedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View != null && View is ListView && (View.Id == "SampleParameter_ListView_Copy_ResultView" || View.Id == "SampleParameter_ListView_Copy_ResultApproval"))
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objValidatedDate;
                    objValidatedDate = null;
                    if (gv != null)
                    {
                        objValidatedDate = gv.DataColumns["ApprovedDate"];
                    }

                    if (objValidatedDate != null && gv.DataColumns["ApprovedDate"].Index != -1)
                    {
                        gv.DataColumns["ApprovedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
                if (View != null && View is ListView && (View.Id == "Reporting_ListView_Copy_ReportView" ||
                 View.Id == "Reporting_ListView_Copy_ReportApproval" || View.Id == "Reporting_ListView"))
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gv = gridListEditor.Grid;
                    object objReportedDate;
                    objReportedDate = null;
                    if (gv != null)
                    {
                        objReportedDate = gv.DataColumns["ReportedDate"];
                    }

                    if (objReportedDate != null && gv.DataColumns["ReportedDate"].Index != -1)
                    {
                        gv.DataColumns["ReportedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }

                    object objValidatedDate;
                    objValidatedDate = null;
                    if (gv != null)
                    {
                        objValidatedDate = gv.DataColumns["ReportValidatedDate"];
                    }

                    if (objValidatedDate != null && gv.DataColumns["ReportValidatedDate"].Index != -1)
                    {
                        gv.DataColumns["ReportValidatedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }

                    object objApprovedDate;
                    objApprovedDate = null;
                    if (gv != null)
                    {
                        objApprovedDate = gv.DataColumns["ReportApprovedDate"];
                    }
                    if (objApprovedDate != null && gv.DataColumns["ReportApprovedDate"].Index != -1)
                    {
                        gv.DataColumns["ReportApprovedDate"].PropertiesEdit.DisplayFormatString = WebConfigurationManager.AppSettings["DateFormat"];
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        #endregion
    }
}
