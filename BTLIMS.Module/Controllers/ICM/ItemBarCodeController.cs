using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.InfoClass;
using System;

namespace LDM.Module.Controllers.ICM
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ItemBarCodeController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        public string strItem = string.Empty;
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        public ItemBarCodeController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Items);
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
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

        private void ItemBarCodeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //if (View.Id== "Items_ListView" && View.SelectedObjects!=null)
                //{
                //    //Items quoteObj = (Items)e.CurrentObject;
                //    //strItem = "'" + quoteObj.Oid.ToString() + "'";

                //    foreach (Items obj in View.SelectedObjects)
                //    {
                //        if (strItem == string.Empty)
                //        {
                //            strItem = "'" + obj.Oid.ToString() + "'";
                //        }
                //        else
                //        {
                //            strItem = strItem + ",'" + obj.Oid.ToString() + "'";
                //        }
                //    }

                //    string strTempPath = Path.GetTempPath();
                //    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                //    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\Items\")) == false)
                //    {
                //        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\Items\"));
                //    }
                //    string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\Items\" + timeStamp + ".pdf");
                //    XtraReport xtraReport = new XtraReport();

                //    objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //    SetConnectionString();

                //    DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                //    ObjReportingInfo.strItem = strItem;
                //    xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("Item Barcode", ObjReportingInfo, false);
                //    //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                //    xtraReport.ExportToPdf(strExportedPath);
                //    string[] path = strExportedPath.Split('\\');
                //    int arrcount = path.Count();
                //    int sc = arrcount - 3;
                //    string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                //    WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                //    strItem = string.Empty;
                //}
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
