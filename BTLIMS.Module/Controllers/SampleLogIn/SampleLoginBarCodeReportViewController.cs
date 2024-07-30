using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.XtraReports.UI;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Report;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.SampleLogIn
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SampleLoginBarCodeReportViewController : ViewController
    {
        #region Declaration
        IObjectSpace os;
        bool SampleTest = true;
        string strJobID = string.Empty;
        string strSampleID = string.Empty;
        MessageTimer timer = new MessageTimer();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        SampleReport sampleLabelReport = new SampleReport();

        #endregion

        #region Constructor
        public SampleLoginBarCodeReportViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleLogIn_ListView;" + "SampleLogIn_DetailView;" + "Samplecheckin_ListView_Copy;" + "SampleLogIn_ListView_Copy_SampleRegistration;";
            BarcodeReport.TargetViewId = "SampleLogIn_ListView;" + "Samplecheckin_ListView_Copy;" + "SampleLogIn_ListView_Copy_SampleRegistration;";
            FolderLabel.TargetViewId = "SampleLogIn_ListView;" + "Samplecheckin_ListView_Copy;" + "SampleLogIn_ListView_Copy_SampleRegistration;";
        }
        #endregion

        #region DefaultMethods
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                //BarcodeReport.Active["ShowbarcodeRpt"] = objPermissionInfo.SampleRegIsWrite;
                //FolderLabel.Active["ShowfolderRpt"] = objPermissionInfo.SampleRegIsWrite;
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
        }
        protected override void OnDeactivated()
        {
            try
            {
                //DynamicDesigner.GlobalReportSourceCode.struqSampleParameterID = string.Empty;
                //DynamicDesigner.GlobalReportSourceCode.strJobID = string.Empty;
                ObjReportingInfo.struqSampleParameterID = string.Empty;
                ObjReportingInfo.strJobID = string.Empty;
                base.OnDeactivated();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        #endregion

        #region Events
        private void BarcodeReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    strSampleID = string.Empty;
                    if (View.SelectedObjects.Count > 0)
                    {
                        if (View.Id == "SampleLogIn_ListView")
                        {
                            foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn obj in View.SelectedObjects)
                            {
                                if (ObjectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.SampleParameter), CriteriaOperator.Parse("Samplelogin.Oid =?", obj.Oid)) > 0)
                                {

                                    if (strJobID == string.Empty)
                                    {
                                        strJobID = "'" + obj.JobID.JobID.ToString() + "'";
                                    }
                                    else
                                    {
                                        if (!strJobID.Contains(obj.JobID.JobID.ToString()))
                                            strJobID = strJobID + ",'" + obj.JobID.JobID.ToString() + "'";
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add test for samples and try again.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    SampleTest = false;
                                    break;
                                }
                            }
                        }
                        else if (View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                        {
                            foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn obj in View.SelectedObjects)
                            {
                                SampleTest = true;
                                if (ObjectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.SampleParameter), CriteriaOperator.Parse("Samplelogin.Oid =?", obj.Oid)) > 0)
                                {
                                    //SampleTest = true;
                                    //IList<SampleParameter> parameters = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid]=?", obj.SampleID));

                                    if (strSampleID == string.Empty)
                                    {
                                        strSampleID = "'" + obj.SampleID.ToString() + "'";
                                    }

                                    else
                                    {
                                        //strJobID = strJobID + ",'" + obj.JobID.ToString() + "'";
                                        if (!strSampleID.Contains(obj.SampleID.ToString()))
                                            strSampleID = strSampleID + ",'" + obj.SampleID.ToString() + "'";
                                    }

                                }

                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add test for samples and try again.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    SampleTest = false;
                                    break;
                                }

                            }
                        }
                        else if (View.Id == "Samplecheckin_ListView_Copy")
                        {
                            foreach (Modules.BusinessObjects.SampleManagement.Samplecheckin obj in View.SelectedObjects)
                            {
                                if (ObjectSpace.GetObjectsCount(typeof(Modules.BusinessObjects.SampleManagement.SampleParameter), CriteriaOperator.Parse("Samplelogin.Oid =?", obj.Oid)) > 0)
                                {
                                    if (strJobID == string.Empty)
                                    {
                                        strJobID = "'" + obj.JobID.ToString() + "'";
                                    }
                                    else
                                    {
                                        strJobID = strJobID + ",'" + obj.JobID.ToString() + "'";
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add test for samples and try again.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    SampleTest = false;
                                    break;

                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(strSampleID) && SampleTest == true)
                        {
                            string strTempPath = Path.GetTempPath();
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                            }
                            string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                            XtraReport xtraReport = new XtraReport();
                            //XtraTabControl tabControl = new XtraTabControl();
                            DataView dv = new DataView();
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("SampleBottleID");
                            dataTable.Columns.Add("SampleName");
                            dataTable.Columns.Add("SampleMatrix");
                            dataTable.Columns.Add("CollectDateTime");
                            dataTable.Columns.Add("RecievedDate");
                            dataTable.Columns.Add("DueDate");
                            dataTable.Columns.Add("Tests");
                            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                            string serverName = connectionStringBuilder.DataSource.Trim();
                            string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                            string userID = connectionStringBuilder.UserID.Trim();
                            string password = connectionStringBuilder.Password.Trim();
                            string sqlSelect = "Select * from SampleVeriticalLabel_view where [SampleID] in(" + strSampleID + ")";
                            SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                            SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);

                            sqlDa.Fill(dataTable);
                            //dataTable = new DataView(dataTable, "", "SampleBottleID", DataViewRowState.CurrentRows).ToTable(false, "SampleBottleID");
                            dataTable = new DataView(dataTable, "", "SampleBottleID", DataViewRowState.CurrentRows).ToTable(false, "SampleBottleID", "SampleName", "SampleMatrix", "CollectDateTime", "RecievedDate", "DueDate", "Tests");

                            //if (dataTable.Rows.Count > 0)
                            //{
                            //    foreach (DataRow drv in dataTable.Rows)
                            //    {
                            //        DataRow dataRow = dataTable.NewRow();
                            //        dataRow["SampleBottleID"] = drv["SampleBottleID"] != null ? drv["SampleBottleID"] : "";
                            //        dataRow["SampleName"] = drv["SampleName"] != null ? drv["SampleName"] : "";
                            //        dataRow["SampleMatrix"] = drv["SampleMatrix"] != null ? drv["SampleMatrix"] : "";
                            //        dataRow["DateCollected"] = drv["CollectDateTime"] != null ? drv["CollectDateTime"] : "";
                            //        dataRow["DateRecieved"] = drv["RecievedDate"] != null ? drv["RecievedDate"] : "";
                            //        dataRow["DueDate"] = drv["DueDate"] != null ? drv["DueDate"] : "";
                            //        dataRow["Tests"] = drv["Tests"] != null ? drv["Tests"] : "";
                            //        dataTable.Rows.Add(dataRow);
                            //    }

                            //}
                            SampleReport sampleLabelReport = new SampleReport();
                            sampleLabelReport.DataSource = dataTable;
                            sampleLabelReport.DataBindingsToReport();
                            xtraReport = sampleLabelReport;
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 2;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));



                            //    XtraReport xtraReport = new XtraReport();
                            //    DataTable dataTable = new DataTable();
                            //    dataTable.Columns.Add("SampleBottleID");
                            //    dataTable.Columns.Add("SampleName");
                            //    dataTable.Columns.Add("SampleMatrix");
                            //    dataTable.Columns.Add("DateCollected");
                            //    dataTable.Columns.Add("DateRecieved");
                            //    dataTable.Columns.Add("DueDate");
                            //    dataTable.Columns.Add("Tests");
                            //    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            //    var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                            //    string serverName = connectionStringBuilder.DataSource.Trim();
                            //    string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                            //    string userID = connectionStringBuilder.UserID.Trim();
                            //    string password = connectionStringBuilder.Password.Trim();
                            //    string sqlSelect = "Select * from SampleVeriticalLabel_view where [SampleID] in(" + strSampleID + ")";
                            //    SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                            //    SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                            //    SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                            //    sqlDa.Fill(dataTable);
                            //    dataTable = new DataView(dataTable, "", "SampleBottleID", DataViewRowState.CurrentRows).ToTable(false, "SampleBottleID");//SortOrder asending SampleBottleID 
                            //    SampleReport sampleLabelReport = new SampleReport();
                            //    sampleLabelReport.DataSource = dataTable;
                            //    sampleLabelReport.DataBindingsToReport();
                            //    xtraReport = sampleLabelReport;
                            //    xtraReport.ExportToPdf(strExportedPath);
                            //    string[] path = strExportedPath.Split('\\');
                            //    int arrcount = path.Count();
                            //    int sc = arrcount - 2;
                            //    string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                            //    WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Add test for samples and try again.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
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
        #endregion

        #region Function
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
        #endregion


        private void FolderLabel_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    strJobID = string.Empty;
                    if (View.SelectedObjects.Count > 0)
                    {
                        if (View.Id == "SampleLogIn_ListView" || View.Id == "SampleLogIn_ListView_Copy_SampleRegistration")
                        {
                            foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn obj in View.SelectedObjects)
                            {
                                if (strJobID == string.Empty)
                                {
                                    strJobID = "'" + obj.JobID.JobID.ToString() + "'";
                                }
                                else
                                {
                                    if (!strJobID.Contains(obj.JobID.JobID.ToString()))
                                        strJobID = strJobID + ",'" + obj.JobID.JobID.ToString() + "'";
                                }
                            }
                        }
                        else if (View.Id == "Samplecheckin_ListView_Copy")
                        {
                            foreach (Modules.BusinessObjects.SampleManagement.Samplecheckin obj in View.SelectedObjects)
                            {
                                if (strJobID == string.Empty)
                                {
                                    strJobID = "'" + obj.JobID.ToString() + "'";
                                }
                                else
                                {
                                    strJobID = strJobID + ",'" + obj.JobID.ToString() + "'";
                                }
                            }
                        }
                        string strTempPath = Path.GetTempPath();
                        String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                        }

                        string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                        XtraReport xtraReport = new XtraReport();
                        DataView dv = new DataView();
                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("SalesorderNo");
                        dataTable.Columns.Add("Client");
                        dataTable.Columns.Add("DateReceived");
                        dataTable.Columns.Add("NumberofSample");
                        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                        string serverName = connectionStringBuilder.DataSource.Trim();
                        string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                        string userID = connectionStringBuilder.UserID.Trim();
                        string password = connectionStringBuilder.Password.Trim();
                        string sqlSelect = "Select * from SL_FolderLabel_View where [JobID] in(" + strJobID + ")";
                        SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                        SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                        SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                        sqlDa.Fill(dataTable);
                        String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                        if (strProjectName == "Consci")
                        {


                            FolderLabel sampleLabelReport1 = new FolderLabel();
                            sampleLabelReport1.DataSource = dataTable;
                            xtraReport = sampleLabelReport1;
                            sampleLabelReport1.DataBindingsToReport();

                        }
                        else
                        {
                            FolderLabelReport sampleLabelReport = new FolderLabelReport();
                            sampleLabelReport.DataSource = dataTable;
                            xtraReport = sampleLabelReport;
                            sampleLabelReport.DataBindingsToReport();
                        }

                        //FolderLabelReport sampleLabelReport = new FolderLabelReport();
                        //sampleLabelReport.DataSource = dataTable;
                        //xtraReport = sampleLabelReport;
                        //sampleLabelReport.DataBindingsToReport();
                        xtraReport.ExportToPdf(strExportedPath);
                        string[] path = strExportedPath.Split('\\');
                        int arrcount = path.Count();
                        int sc = arrcount - 2;
                        string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                        WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
