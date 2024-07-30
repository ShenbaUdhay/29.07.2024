using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.XtraReports.UI;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace ALPACpre.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ICMLTReportController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        DateTime fromdate, todate;
        DateTime TodayDate = DateTime.Now;
        public ICMLTReportController()
        {
            InitializeComponent();
            TargetViewId = "Distribution_ListView_LTsearch;";
            LTReportAction.TargetViewId = "Distribution_ListView_LTsearch;";
            //LTReportAction.Category = "RecordEdit";
            //LTReportAction.Model.Index = 2;
            //LTBarcodeReport = new SingleChoiceAction(this, "LTBarcodeReport", "View");
            //LTBarcodeRpt.TargetViewId = "Distribution_ListView_LTsearch";
            //LTBarcodeRpt.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Caption;
            //FromDate.TargetViewId = "Distribution_ListView_LTsearch;";
            //ToDate.TargetViewId = "Distribution_ListView_LTsearch;";
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if (View.Id == "Distribution_ListView_LTsearch")
                {
                    IModelColumn columnInfo = ((IModelList<IModelColumn>)(View as ListView).Model.Columns)["LT"];
                    if (columnInfo != null)
                    {
                        columnInfo.SortIndex = 0;
                        columnInfo.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }

                }
                //if (LTBarcodeRpt != null && LTBarcodeRpt.Items.Count > 0)
                //{
                //    LTBarcodeRpt.SelectedIndex = 0;
                //}
                // Perform various tasks depending on the target View.
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        void FromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ParametrizedAction calendar = (ParametrizedAction)sender;

                if (calendar.Value != null)
                {
                    DateTime values = (DateTime)calendar.Value;
                    fromdate = values.Date;
                    if (values.Date != DateTime.MinValue)
                    {
                        if (todate < TodayDate || fromdate < TodayDate)
                        {
                            if (todate < fromdate)
                            {
                                Application.ShowViewStrategy.ShowMessage("ToDate greater than FromDate", InformationType.Warning, 3000, InformationPosition.Top);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse(" [EnteredDate] Between(?, ?) And [EnteredBy] = ?", fromdate, todate, SecuritySystem.CurrentUserId);
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Please check the Date", InformationType.Warning, 3000, InformationPosition.Top);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        void ToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ParametrizedAction calendar = (ParametrizedAction)sender;
                if (calendar.Value != null)
                {
                    DateTime values = (DateTime)calendar.Value;
                    todate = values.Date;
                    if (values.Date != DateTime.MinValue)
                    {
                        if (todate < TodayDate || fromdate < TodayDate)
                        {
                            if (todate < fromdate)
                            {
                                Application.ShowViewStrategy.ShowMessage("ToDate greater than FromDate", InformationType.Warning, 3000, InformationPosition.Top);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[EnteredDate] >= ? And [EnteredDate] <= ? AND [EnteredBy] = ?", fromdate, todate, SecuritySystem.CurrentUserId);
                            }
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Please check the Date", InformationType.Warning, 3000, InformationPosition.Top);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void LTReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string strInvoiceID = string.Empty;
                string reporttype = string.Empty;
                if (View.Id == "Distribution_ListView_LTsearch")
                {
                    string strLT = string.Empty;
                    if (View.SelectedObjects.Count > 0)
                    {

                        foreach (Distribution objdistribution in View.SelectedObjects)
                        {
                            if (strLT == string.Empty)
                            {
                                strLT = "'" + objdistribution.LT.ToString() + "'";
                            }
                            else
                            {
                                strLT = strLT + ",'" + objdistribution.LT + "'";
                            }
                        }

                        //    if (View.CurrentObject != null && View.SelectedObjects.Count == 1)
                        //{
                        //    //if (LTBarcodeRpt.SelectedItem != null && LTBarcodeRpt.SelectedItem.ToString() != string.Empty)
                        //    {
                        //Distribution objdistribution = (Distribution)View.CurrentObject;
                        //if (objdistribution != null && !string.IsNullOrEmpty(objdistribution.LT) == true)
                        //{
                        //    // strInvoiceNumber = "'" +"SI"+objInvoice. InvoiceNumber + "'";
                        //    strLT = "'" + objdistribution.LT.ToString() + "'";
                        //}
                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\"));
                        }
                        XtraReport xtraReport = new XtraReport();

                        objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();
                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                        //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                        ObjReportingInfo.strLT = strLT;
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("LTBarcode", ObjReportingInfo, false);
                        //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\LT\" + timeStamp + ".pdf");
                        xtraReport.ExportToPdf(strExportedPath);
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 3;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                        //strLT = string.Empty;
                    }
                    else
                    {
                        //Application.ShowViewStrategy.ShowMessage("Select any atleast one row.", InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);

                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }
        private void SetConnectionString()
        {
            try
            {
                string[] connectionstring = objDRDCInfo.WebConfigConn.Split(';');
                objDRDCInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                objDRDCInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
