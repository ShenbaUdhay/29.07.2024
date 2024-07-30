using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using DevExpress.Data.Filtering;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraReports.UI;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Assets;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Report;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.TaskManagement;
using Project = Modules.BusinessObjects.Setting.Project;

namespace LDM.Module.Controllers.SamplingManagement
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SamplingLoginViewController : ViewController, IXafCallbackHandler
    {
        #region Declaration
        MessageTimer timer = new MessageTimer();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        SamplingManagementInfo objSMInfo = new SamplingManagementInfo();
        SamplingInfo objSampleInfo = new SamplingInfo();
        CopyNoOfSamplesPopUp objCopySampleInfo = new CopyNoOfSamplesPopUp();
        Guid SampleOid = Guid.Empty;
        List<Guid> TPOid = new List<Guid>();
        string SLOid = string.Empty;
        string FocusedJobID = string.Empty;
        Sampling objParentSL;
        string strRegistrationID = string.Empty;
        string strSampleID = string.Empty;
        bool SampleTest = true;
        #endregion
        public SamplingLoginViewController()
        {
            InitializeComponent();
            TargetViewId = "Sampling_ListView_SamplingProposal;"+ "Sampling_ListView_SourceSample;"+ "Sampling_LookupListView_CopyTest_SampleID;"
                + "SampleSites_LookupListView_Sampling;"+ "SampleSites_LookupListView_Sampling_StationLocation;"+ "SamplingProposal_SampleLogin;";
            SamplingCopySamples.TargetViewId = "Sampling_ListView_SamplingProposal;";
            Btn_Add_SamplingCollector.TargetViewId = "Sampling_ListView_SamplingProposal";
            //SamplingReanalysis.TargetViewId = "Sampling_ListView_SamplingProposal";
            //SamplingReanalysis.ImageName = "Action_ResetViewSettings";
            SamplingBarcodeReport.TargetViewId =SamplingFolderLabel.TargetViewId= "Sampling_ListView_SamplingProposal;";
            SaveSamplingSamples.TargetViewId = "Sampling_ListView_SamplingProposal;";
            SamplingSL_CopyTest.TargetViewId = "Sampling_ListView_SamplingProposal;";

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                if (View.Id == "Sampling_ListView_SamplingProposal")
                {
                    View.SelectionChanged += View_SelectionChanged;
                    if (objPermissionInfo.SamplingProposalIsWrite == false)
                    {
                        SamplingCopySamples.Active["showCopySamples"] = false;
                        Btn_Add_SamplingCollector.Active["showAddCollector"] = false;
                        //SamplingReanalysis.Active["showReanalysis"] = false;
                        SamplingSL_CopyTest.Active["ShowCopyTest"] = false;
                        SaveSamplingSamples.Active["showSave"] = false;
                    }
                    else
                    {
                        SamplingCopySamples.Active["showCopySamples"] = true;
                        Btn_Add_SamplingCollector.Active["showAddCollector"] = true;
                        //SamplingReanalysis.Active["showReanalysis"] = true;
                        SamplingSL_CopyTest.Active["ShowCopyTest"] = true;
                        SaveSamplingSamples.Active["showSave"] = true;
                    }
                    SamplingCopySamples.Executing += CopySamples_Executing;
                    SamplingSL_CopyTest.Executing += SL_CopyTest_Executing;

                }
                else if(View.Id== "SampleSites_LookupListView_Sampling" ||View.Id== "SampleSites_LookupListView_Sampling_StationLocation")
                {
                    View.ControlsCreated += View_ControlsCreated;
                }
                else if(View.Id== "SamplingProposal_SampleLogin")
                {
                    View.Closing += View_Closing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
           
        }

        private void View_Closing(object sender, EventArgs e)
        {
            try
            {
                DashboardViewItem lvSamples = ((DashboardView)View).FindItem("SampleLogin") as DashboardViewItem;
                if(lvSamples!=null && lvSamples.InnerView!=null && lvSamples.InnerView.ObjectSpace.ModifiedObjects.Count>0)
                {
                    lvSamples.InnerView.ObjectSpace.CommitChanges();
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void View_ControlsCreated(object sender, EventArgs e)
        {
           try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    if (View.Id== "SampleSites_LookupListView_Sampling")
                    {
                        if (HttpContext.Current.Session["AlternativeStation"] != null)
                        {
                            string[] assignedto = HttpContext.Current.Session["AlternativeStation"].ToString().Split(new string[] { "; " }, StringSplitOptions.None);
                            foreach (string val in assignedto.Where(i=>!string.IsNullOrEmpty(i)))
                            {
                                SampleSites employee = ObjectSpace.FindObject<SampleSites>(CriteriaOperator.Parse("[Oid]=?",new Guid(val)));
                                if (employee != null)
                                {
                                    gridListEditor.Grid.Selection.SelectRowByKey(employee.Oid);
                                }
                            }
                        } 
                    }
                    else if(View.Id== "SampleSites_LookupListView_Sampling_StationLocation")
                    {
                        if (HttpContext.Current.Session["StationLocation"] != null)
                        {
                            string str = HttpContext.Current.Session["StationLocation"].ToString();
                            SampleSites site = ObjectSpace.FindObject<SampleSites>(CriteriaOperator.Parse("[Oid]=?",new Guid( HttpContext.Current.Session["StationLocation"].ToString())));
                            if (site != null)
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(site.Oid);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void FolderLabel_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    strRegistrationID = string.Empty;
                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (Sampling obj in View.SelectedObjects)
                        {
                            if (strRegistrationID == string.Empty)
                            {
                                strRegistrationID = "'" + obj.SamplingProposal.RegistrationID.ToString() + "'";
                            }
                            else
                            {
                                if (!strRegistrationID.Contains(obj.SamplingProposal.RegistrationID.ToString()))
                                    strRegistrationID = strRegistrationID + ",'" + obj.SamplingProposal.RegistrationID.ToString() + "'";
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
                        string sqlSelect = "Select * from SL_FolderLabel_Sampling_View where [JobID] in(" + strRegistrationID + ")";
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
        private void BarcodeReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    strSampleID = string.Empty;
                    if (View.SelectedObjects.Count > 0)
                    {
                        foreach (Sampling obj in View.SelectedObjects)
                        {
                            SampleTest = true;
                            if (ObjectSpace.GetObjectsCount(typeof(SamplingParameter), CriteriaOperator.Parse("Sampling.Oid =?", obj.Oid)) > 0)
                            {
                                if (strSampleID == string.Empty)
                                {
                                    strSampleID = "'" + obj.SampleID.ToString() + "'";
                                }
                                else
                                {
                                    if (!strSampleID.Contains(obj.SampleID.ToString()))
                                        strSampleID = strSampleID + ",'" + obj.SampleID.ToString() + "'";
                                }
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Add Test for Samples and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                SampleTest = false;
                                break;
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
                            string sqlSelect = "Select * from SampleVeriticalLabel_Sampling_view where [SampleID] in(" + strSampleID + ")";
                            SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                            SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                            sqlDa.Fill(dataTable);
                            dataTable = new DataView(dataTable, "", "SampleBottleID", DataViewRowState.CurrentRows).ToTable(false, "SampleBottleID", "SampleName", "SampleMatrix", "CollectDateTime", "RecievedDate", "DueDate", "Tests");
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

                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Add Test for Samples and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);
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
        private void Btn_Add_Collector_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Sampling_ListView_SamplingProposal")
                {
                    IObjectSpace objectSpace = Application.CreateObjectSpace();
                    Collector objCollector = objectSpace.CreateObject<Collector>();
                    CriteriaOperator cs = CriteriaOperator.Parse("RegistrationID=?", objSMInfo.strJobID);
                    SamplingProposal objSamplecheckin = objectSpace.FindObject<SamplingProposal>(cs);
                    if (objSamplecheckin != null)
                    {
                        objCollector.CustomerName = objSamplecheckin.ClientName;
                    }
                    DetailView dv = Application.CreateDetailView(objectSpace, "Collector_DetailView_Sampling", true, objCollector);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    ShowViewParameters showViewParameters = new ShowViewParameters();
                    showViewParameters.CreatedView = dv;
                    showViewParameters.Context = TemplateContext.PopupWindow;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    showViewParameters.Controllers.Add(dc);
                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Reanalysis_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                CriteriaOperator cs = CriteriaOperator.Parse("RegistrationID=?", objSMInfo.strJobID);
                Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                Sampling objCurrent = (Sampling)View.CurrentObject;
                Sampling objSLOld = uow.GetObjectByKey<Sampling>(objCurrent.Oid);
                SamplingProposal objsamplecheckin = uow.FindObject<SamplingProposal>(cs);
                Sampling objSLNew = new Sampling(uow);
                objSLNew.SamplingProposal = objsamplecheckin;
                objSLNew.IsReanalysis = true;
                List<SamplingBottleAllocation> smplold = uow.Query<SamplingBottleAllocation>().Where(i => i.Sampling != null && i.Sampling.Oid == objSLOld.Oid).ToList();
                int sampleno = 0;
                SelectedData sproc = currentSession.ExecuteSproc("GetSamplingSampleID", new OperandValue(objsamplecheckin.RegistrationID));
                if (sproc.ResultSet[1].Rows[0].Values[0].ToString() != null)
                {
                    sampleno = (int)sproc.ResultSet[1].Rows[0].Values[0];
                }
                objSLNew.SampleNo = sampleno;
                int sysSampleno = 0;
                if (objCurrent != null && !objCurrent.IsReanalysis)
                {
                    sysSampleno = uow.Query<Sampling>().Where(i => i.SampleSource != null && i.SampleSource == objCurrent.SampleID).ToList().Count();
                    objSLNew.SampleSource = objSLOld.SampleID;
                }
                else
                {
                    sysSampleno = uow.Query<Sampling>().Where(i => i.SampleSource != null && i.SampleSource == objCurrent.SampleSource).ToList().Count();
                    objSLNew.SampleSource = objSLOld.SampleSource;
                }
                sysSampleno++;
                objSLNew.SysSampleCode = objSLOld.SampleID + "LR" + sysSampleno;
                objSLNew.ExcludeInvoice = true;
                if (objSLOld.VisualMatrix != null)
                {
                    objSLNew.VisualMatrix = uow.GetObjectByKey<VisualMatrix>(objSLOld.VisualMatrix.Oid); ;
                }
                if (objSLOld.SampleType != null)
                {
                    objSLNew.SampleType = uow.GetObjectByKey<SampleType>(objSLOld.SampleType.Oid);
                }
                objSLNew.Qty = objSLOld.Qty;
                if (objSLOld.Storage != null)
                {
                    objSLNew.Storage = uow.GetObjectByKey<Storage>(objSLOld.Storage.Oid);
                }
                objSLNew.Preservetives = objSLOld.Preservetives;
                objSLNew.SamplingLocation = objSLOld.SamplingLocation;
                if (objSLOld.QCType != null)
                {
                    objSLNew.QCType = uow.GetObjectByKey<QCType>(objSLOld.QCType.Oid);
                }
                //objSLNew.QCSource = objSLOld.QCSource;
                if (objSLOld.QCSource != null)
                {
                    objSLNew.QCSource = uow.GetObjectByKey<Sampling>(objSLOld.QCSource.Oid);
                }
                if (objSLOld.Collector != null)
                {
                    objSLNew.Collector = uow.GetObjectByKey<Collector>(objSLOld.Collector.Oid);
                }
                if (objSLOld.Client != null)
                {
                    objSLNew.Client = uow.GetObjectByKey<Customer>(objSLOld.Client.Oid);
                }
                if (objSLOld.Department != null)
                {

                    objSLNew.Department = uow.GetObjectByKey<Department>(objSLOld.Department.Oid);
                }
                if (objSLOld.ProjectID != null)
                {

                    objSLNew.ProjectID = uow.GetObjectByKey<Modules.BusinessObjects.Setting.Project>(objSLOld.ProjectID.Oid);
                }
                if (objSLOld.PreserveCondition != null)
                {

                    objSLNew.PreserveCondition = uow.GetObjectByKey<PreserveCondition>(objSLOld.PreserveCondition.Oid);
                }
                if (objSLOld.StorageID != null)
                {

                    objSLNew.StorageID = uow.GetObjectByKey<Storage>(objSLOld.StorageID.Oid);
                }
                objSLNew.CollectDate = objSLOld.CollectDate;
                objSLNew.CollectTime = objSLOld.CollectTime;
                objSLNew.FlowRate = objSLOld.FlowRate;
                objSLNew.TimeStart = objSLOld.TimeStart;
                objSLNew.TimeEnd = objSLOld.TimeEnd;
                objSLNew.Time = objSLOld.Time;
                objSLNew.Volume = objSLOld.Volume;
                objSLNew.Address = objSLOld.Address;
                objSLNew.AreaOrPerson = objSLOld.AreaOrPerson;
                if (objSLOld.BalanceID != null)
                {
                    objSLNew.BalanceID = uow.GetObjectByKey<Labware>(objSLOld.BalanceID.Oid);
                }
                objSLNew.AssignTo = objSLOld.AssignTo;
                objSLNew.Barp = objSLOld.Barp;
                objSLNew.BatchID = objSLOld.BatchID;
                objSLNew.BatchSize = objSLOld.BatchSize;
                objSLNew.BatchSize_pc = objSLOld.BatchSize_pc;
                objSLNew.BatchSize_Units = objSLOld.BatchSize_Units;
                objSLNew.Blended = objSLOld.Blended;
                objSLNew.BottleQty = objSLOld.BottleQty;
                objSLNew.BuriedDepthOfGroundWater = objSLOld.BuriedDepthOfGroundWater;
                objSLNew.ChlorineFree = objSLOld.ChlorineFree;
                objSLNew.ChlorineTotal = objSLOld.ChlorineTotal;
                objSLNew.City = objSLOld.City;
                objSLNew.CollectorPhone = objSLOld.CollectorPhone;
                objSLNew.CompositeQty = objSLOld.CompositeQty;
                objSLNew.DateEndExpected = objSLOld.DateEndExpected;
                objSLNew.DateStartExpected = objSLOld.DateStartExpected;
                objSLNew.Depth = objSLOld.Depth;
                objSLNew.Description = objSLOld.Description;
                objSLNew.DischargeFlow = objSLOld.DischargeFlow;
                objSLNew.DischargePipeHeight = objSLOld.DischargePipeHeight;
                objSLNew.DO = objSLOld.DO;
                objSLNew.DueDate = objSLOld.DueDate;
                objSLNew.Emission = objSLOld.Emission;
                objSLNew.EndOfRoad = objSLOld.EndOfRoad;
                objSLNew.EquipmentModel = objSLOld.EquipmentModel;
                objSLNew.EquipmentName = objSLOld.EquipmentName;
                objSLNew.FacilityID = objSLOld.FacilityID;
                objSLNew.FacilityName = objSLOld.FacilityName;
                objSLNew.FacilityType = objSLOld.FacilityType;
                objSLNew.FinalForm = objSLOld.FinalForm;
                objSLNew.FinalPackaging = objSLOld.FinalPackaging;
                objSLNew.FlowRate = objSLOld.FlowRate;
                objSLNew.FlowRateCubicMeterPerHour = objSLOld.FlowRateCubicMeterPerHour;
                objSLNew.FlowRateLiterPerMin = objSLOld.FlowRateLiterPerMin;
                objSLNew.FlowVelocity = objSLOld.FlowVelocity;
                objSLNew.ForeignMaterial = objSLOld.ForeignMaterial;
                objSLNew.Frequency = objSLOld.Frequency;
                objSLNew.GISStatus = objSLOld.GISStatus;
                objSLNew.GravelContent = objSLOld.GravelContent;
                objSLNew.GrossWeight = objSLOld.GrossWeight;
                objSLNew.GroupSample = objSLOld.GroupSample;
                objSLNew.Hold = objSLOld.Hold;
                objSLNew.Humidity = objSLOld.Humidity;
                objSLNew.IceCycle = objSLOld.IceCycle;
                objSLNew.Increments = objSLOld.Increments;
                objSLNew.Interval = objSLOld.Interval;
                objSLNew.IsActive = objSLOld.IsActive;
                //objSLNew.IsNotTransferred = objSLOld.IsNotTransferred;
                objSLNew.ItemName = objSLOld.ItemName;
                objSLNew.KeyMap = objSLOld.KeyMap;
                objSLNew.LicenseNumber = objSLOld.LicenseNumber;
                objSLNew.ManifestNo = objSLOld.ManifestNo;
                objSLNew.MonitoryingRequirement = objSLOld.MonitoryingRequirement;
                objSLNew.NoOfCollectionsEachTime = objSLOld.NoOfCollectionsEachTime;
                objSLNew.NoOfPoints = objSLOld.NoOfPoints;
                objSLNew.Notes = objSLOld.Notes;
                objSLNew.OriginatingEntiry = objSLOld.OriginatingEntiry;
                objSLNew.OriginatingLicenseNumber = objSLOld.OriginatingLicenseNumber;
                objSLNew.PackageNumber = objSLOld.PackageNumber;
                objSLNew.ParentSampleDate = objSLOld.ParentSampleDate;
                objSLNew.ParentSampleID = objSLOld.ParentSampleID;
                objSLNew.PiecesPerUnit = objSLOld.PiecesPerUnit;
                objSLNew.Preservetives = objSLOld.Preservetives;
                objSLNew.ProjectName = objSLOld.ProjectName;
                objSLNew.PurifierSampleID = objSLOld.PurifierSampleID;
                objSLNew.PWSID = objSLOld.PWSID;
                objSLNew.PWSSystemName = objSLOld.PWSSystemName;
                objSLNew.RegionNameOfSection = objSLOld.RegionNameOfSection;
                objSLNew.RejectionCriteria = objSLOld.RejectionCriteria;
                objSLNew.RepeatLocation = objSLOld.RepeatLocation;
                objSLNew.RetainedWeight = objSLOld.RetainedWeight;
                objSLNew.RiverWidth = objSLOld.RiverWidth;
                objSLNew.RushSample = objSLOld.RushSample;
                objSLNew.SampleAmount = objSLOld.SampleAmount;
                objSLNew.SampleCondition = objSLOld.SampleCondition;
                objSLNew.SampleDescription = objSLOld.SampleDescription;
                objSLNew.SampleImage = objSLOld.SampleImage;
                objSLNew.SampleName = objSLOld.SampleName;
                objSLNew.SamplePointID = objSLOld.SamplePointID;
                objSLNew.SamplePointType = objSLOld.SamplePointType;
                objSLNew.SampleTag = objSLOld.SampleTag;
                objSLNew.SampleWeight = objSLOld.SampleWeight;
                objSLNew.SamplingAddress = objSLOld.SamplingAddress;
                objSLNew.SamplingEquipment = objSLOld.SamplingEquipment;
                objSLNew.SamplingLocation = objSLOld.SamplingLocation;
                objSLNew.SamplingProcedure = objSLOld.SamplingProcedure;
                objSLNew.SequenceTestSampleID = objSLOld.SequenceTestSampleID;
                objSLNew.SequenceTestSortNo = objSLOld.SequenceTestSortNo;
                objSLNew.ServiceArea = objSLOld.ServiceArea;
                objSLNew.SiteCode = objSLOld.SiteCode;
                objSLNew.SiteDescription = objSLOld.SiteDescription;
                objSLNew.SiteID = objSLOld.SiteID;
                objSLNew.SiteNameArchived = objSLOld.SiteNameArchived;
                objSLNew.SiteUserDefinedColumn1 = objSLOld.SiteUserDefinedColumn1;
                objSLNew.SiteUserDefinedColumn2 = objSLOld.SiteUserDefinedColumn2;
                objSLNew.SiteUserDefinedColumn3 = objSLOld.SiteUserDefinedColumn3;
                objSLNew.SubOut = objSLOld.SubOut;
                objSLNew.SystemType = objSLOld.SystemType;
                objSLNew.TargetMGTHC_CBD_mg_pc = objSLOld.TargetMGTHC_CBD_mg_pc;
                objSLNew.TargetMGTHC_CBD_mg_unit = objSLOld.TargetMGTHC_CBD_mg_unit;
                objSLNew.TargetPotency = objSLOld.TargetPotency;
                objSLNew.TargetUnitWeight_g_pc = objSLOld.TargetUnitWeight_g_pc;
                objSLNew.TargetUnitWeight_g_unit = objSLOld.TargetUnitWeight_g_unit;
                objSLNew.TargetWeight = objSLOld.TargetWeight;
                objSLNew.Time = objSLOld.Time;
                objSLNew.TimeEnd = objSLOld.TimeEnd;
                objSLNew.TimeStart = objSLOld.TimeStart;
                objSLNew.TotalSamples = objSLOld.TotalSamples;
                objSLNew.TotalTimes = objSLOld.TotalTimes;
                if (objSLOld.TtimeUnit != null)
                {
                    objSLNew.TtimeUnit = uow.GetObjectByKey<Unit>(objSLOld.TtimeUnit.Oid);
                }
                objSLNew.WaterType = objSLOld.WaterType;
                objSLNew.ZipCode = objSLOld.ZipCode;
                //objSLNew.ModifiedBy = objSLOld.ModifiedBy;
                if (objSLOld.ModifiedBy != null)
                {
                    objSLNew.ModifiedBy = uow.GetObjectByKey<Modules.BusinessObjects.Hr.CustomSystemUser>(objSLOld.ModifiedBy.Oid);
                }
                objSLNew.ModifiedDate = objSLOld.ModifiedDate;
                objSLNew.Comment = objSLOld.Comment;
                objSLNew.Latitude = objSLOld.Latitude;
                objSLNew.Longitude = objSLOld.Longitude;
                List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType.QCTypeName == "Sample" && j.Sampling.Where(a => a.Oid == objSLOld.Oid).Count() > 0).ToList();
                foreach (var objLineA in lsttp)
                {
                    objSLNew.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                }
                foreach (var objSampleparameter in objSLOld.SamplingParameter.Where(a => a.IsGroup == true && a.GroupTest != null).ToList())
                {
                    SamplingParameter sample = objSLNew.SamplingParameter.FirstOrDefault<SamplingParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                    if (objSampleparameter.GroupTest != null && sample != null)
                    {
                        sample.IsGroup = true;
                        sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objSampleparameter.GroupTest.Oid);
                    }
                }
                foreach (var objSampleparameter in objSLOld.SamplingParameter.Where(a => a.SubOut == true).ToList())
                {
                    SamplingParameter sample = objSLNew.SamplingParameter.FirstOrDefault<SamplingParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                    if (sample != null)
                    {
                        sample.SubOut = true;
                    }
                }
                if (smplold != null && smplold.Count > 0)
                {
                    foreach (SamplingBottleAllocation smpl in smplold.ToList())
                    {
                        SamplingBottleAllocation smplnew = new SamplingBottleAllocation(uow);
                        smplnew.Sampling = objSLNew;
                        smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(smpl.TestMethod.Oid);
                        smplnew.BottleID = smpl.BottleID;
                        if (smpl.Containers != null)
                        {
                            smplnew.Containers = uow.GetObjectByKey<Container>(smpl.Containers.Oid);
                        }
                        if (smpl.Preservative != null)
                        {
                            smplnew.Preservative = uow.GetObjectByKey<Preservative>(smpl.Preservative.Oid);
                        }
                        if (smpl.StorageID != null)
                        {
                            smplnew.StorageID = uow.GetObjectByKey<Storage>(smpl.StorageID.Oid);
                        }
                        if (smpl.StorageCondition != null)
                        {
                            smplnew.StorageCondition = uow.GetObjectByKey<PreserveCondition>(smpl.StorageCondition.Oid);
                        }
                        smplnew.Save();
                    }
                }
                if (objSLNew.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                {
                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSLNew.SamplingProposal.Oid, OperationType.Created, "Sample Login", objSLNew.SampleSource, "Reanalysis", objSLNew.SampleID, "", "");
                }
                objSLNew.Save();
                uow.CommitChanges();
                ((ListView)View).CollectionSource.Add(((ListView)View).ObjectSpace.GetObject(objSLNew));
                View.Refresh();
            }

            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CopySamples_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                if (gridListEditor != null && gridListEditor.Grid != null)
                {
                    gridListEditor.Grid.UpdateEdit();
                }
                IObjectSpace objspace = Application.CreateObjectSpace();
                SL_CopyNoOfSamples copyNoOfSamples = objspace.CreateObject<SL_CopyNoOfSamples>();
                DetailView dvcopysample = Application.CreateDetailView(objspace, "SL_CopyNoOfSamples_DetailView", false, copyNoOfSamples);
                dvcopysample.ViewEditMode = ViewEditMode.Edit;
                ShowViewParameters showViewParameters = new ShowViewParameters(dvcopysample);
                showViewParameters.CreatedView = dvcopysample;
                showViewParameters.Context = TemplateContext.PopupWindow;
                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.SaveOnAccept = false;
                dc.Accepting += CopySamples_Accepting;
                dc.CloseOnCurrentObjectProcessing = false;
                showViewParameters.Controllers.Add(dc);
                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CopySamples_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (objSampleInfo.RegistrationID != string.Empty)
                {
                    if (objCopySampleInfo.NoOfSamples > 0)
                    {
                        objCopySampleInfo.Msgflag = false;
                        bool DBAccess = false;
                        string JobID = string.Empty;
                        int SampleNo = 0;
                        IObjectSpace objectSpace = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)(objectSpace)).Session;
                        UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                        List<Guid> lstnewSamples = new List<Guid>();
                        if (View != null && View.CurrentObject != null && View.ObjectTypeInfo.Type == typeof(Sampling))
                        {
                            Sampling objSLOld = (Sampling)View.CurrentObject;
                            objSLOld = uow.GetObjectByKey<Sampling>(objSLOld.Oid);
                            List<SamplingBottleAllocation> smplold = uow.Query<SamplingBottleAllocation>().Where(i => i.Sampling != null && i.Sampling.Oid == objSLOld.Oid).ToList();
                            VisualMatrix visualMatrix = null;
                            Collector objcollector = null;
                            SamplingProposal objJobId = uow.GetObjectByKey<SamplingProposal>(objSLOld.SamplingProposal.Oid);
                            if (objSLOld.VisualMatrix != null)
                            {
                                visualMatrix = uow.GetObjectByKey<VisualMatrix>(objSLOld.VisualMatrix.Oid);
                            }
                            if (objSLOld.Collector != null)
                            {
                                objcollector = uow.GetObjectByKey<Collector>(objSLOld.Collector.Oid);
                            }
                            for (int i = 1; i <= objCopySampleInfo.NoOfSamples; i++)
                            {
                                Sampling objSLNew = new Sampling(uow);
                                lstnewSamples.Add(objSLNew.Oid);
                                objSLNew.SamplingProposal = objJobId;
                                objSLNew.ExcludeInvoice = false;
                                if (DBAccess == false)
                                {
                                    SelectedData sproc = currentSession.ExecuteSproc("GetSamplingSampleID", new OperandValue(objSLNew.SamplingProposal.RegistrationID.ToString()));
                                    if (sproc.ResultSet[1].Rows[0].Values[0] != null)
                                    {
                                        objSampleInfo.SampleID = sproc.ResultSet[1].Rows[0].Values[0].ToString();
                                        SampleNo = Convert.ToInt32(objSampleInfo.SampleID);
                                        DBAccess = true;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                objSampleInfo.boolCopySamples = true;
                                objSLNew.SampleNo = SampleNo;
                                objSLNew.Test = true;
                                if (visualMatrix != null)
                                {
                                    objSLNew.VisualMatrix = visualMatrix;
                                }
                                if(objSLOld.StationLocation!=null)
                                {
                                    objSLNew.StationLocation = uow.GetObjectByKey<SampleSites>(objSLOld.StationLocation.Oid);
                                }
                                if (objSLOld.SampleType != null)
                                {
                                    objSLNew.SampleType = uow.GetObjectByKey<SampleType>(objSLOld.SampleType.Oid);
                                }
                                objSLNew.Qty = objSLOld.Qty;
                                objSLNew.AlternativeStation = objSLOld.AlternativeStation;
                                objSLNew.AlternativeStationOid = objSLOld.AlternativeStationOid;
                                objSLNew.Test = true;
                                if (objSLOld.Storage != null)
                                {
                                    objSLNew.Storage = uow.GetObjectByKey<Storage>(objSLOld.Storage.Oid);
                                }
                                objSLNew.Preservetives = objSLOld.Preservetives;
                                objSLNew.SamplingLocation = objSLOld.SamplingLocation;
                                if (objSLOld.QCType != null)
                                {
                                    objSLNew.QCType = uow.GetObjectByKey<QCType>(objSLOld.QCType.Oid);
                                }
                                if (objSLOld.QCSource != null)
                                {
                                    objSLNew.QCSource = uow.GetObjectByKey<Sampling>(objSLOld.QCSource.Oid);
                                }
                                if (objcollector != null)
                                {
                                    objSLNew.Collector = objcollector;
                                }
                                if (objSLOld.Client != null)
                                {
                                    objSLNew.Client = uow.GetObjectByKey<Customer>(objSLOld.Client.Oid);
                                }
                                if (objSLOld.Department != null)
                                {
                                    objSLNew.Department = uow.GetObjectByKey<Department>(objSLOld.Department.Oid);
                                }
                                if (objSLOld.ProjectID != null)
                                {

                                    objSLNew.ProjectID = uow.GetObjectByKey<Project>(objSLOld.ProjectID.Oid);
                                }
                                if (objSLOld.PreserveCondition != null)
                                {
                                    objSLNew.PreserveCondition = uow.GetObjectByKey<PreserveCondition>(objSLOld.PreserveCondition.Oid);
                                }
                                if (objSLOld.StorageID != null)
                                {
                                    objSLNew.StorageID = uow.GetObjectByKey<Storage>(objSLOld.StorageID.Oid);
                                }
                                objSLNew.CollectDate = objSLOld.CollectDate;
                                objSLNew.CollectTime = objSLOld.CollectTime;
                                objSLNew.FlowRate = objSLOld.FlowRate;
                                objSLNew.TimeStart = objSLOld.TimeStart;
                                objSLNew.TimeEnd = objSLOld.TimeEnd;
                                objSLNew.Time = objSLOld.Time;
                                objSLNew.Volume = objSLOld.Volume;
                                objSLNew.Address = objSLOld.Address;
                                objSLNew.AreaOrPerson = objSLOld.AreaOrPerson;
                                if (objSLOld.BalanceID != null)
                                {
                                    objSLNew.BalanceID = uow.GetObjectByKey<Labware>(objSLOld.BalanceID.Oid);
                                }
                                objSLNew.AssignTo = objSLOld.AssignTo;
                                objSLNew.Barp = objSLOld.Barp;
                                objSLNew.BatchID = objSLOld.BatchID;
                                objSLNew.BatchSize = objSLOld.BatchSize;
                                objSLNew.BatchSize_pc = objSLOld.BatchSize_pc;
                                objSLNew.BatchSize_Units = objSLOld.BatchSize_Units;
                                objSLNew.Blended = objSLOld.Blended;
                                objSLNew.BottleQty = objSLOld.BottleQty;
                                objSLNew.BuriedDepthOfGroundWater = objSLOld.BuriedDepthOfGroundWater;
                                objSLNew.ChlorineFree = objSLOld.ChlorineFree;
                                objSLNew.ChlorineTotal = objSLOld.ChlorineTotal;
                                objSLNew.City = objSLOld.City;
                                objSLNew.CollectorPhone = objSLOld.CollectorPhone;
                                //objSLNew.CollectTimeDisplay = objSLOld.CollectTimeDisplay;
                                objSLNew.CompositeQty = objSLOld.CompositeQty;
                                objSLNew.DateEndExpected = objSLOld.DateEndExpected;
                                objSLNew.DateStartExpected = objSLOld.DateStartExpected;
                                objSLNew.Depth = objSLOld.Depth;
                                objSLNew.Description = objSLOld.Description;
                                objSLNew.DischargeFlow = objSLOld.DischargeFlow;
                                objSLNew.DischargePipeHeight = objSLOld.DischargePipeHeight;
                                objSLNew.DO = objSLOld.DO;
                                objSLNew.DueDate = objSLOld.DueDate;
                                objSLNew.Emission = objSLOld.Emission;
                                objSLNew.EndOfRoad = objSLOld.EndOfRoad;
                                objSLNew.EquipmentModel = objSLOld.EquipmentModel;
                                objSLNew.EquipmentName = objSLOld.EquipmentName;
                                objSLNew.FacilityID = objSLOld.FacilityID;
                                objSLNew.FacilityName = objSLOld.FacilityName;
                                objSLNew.FacilityType = objSLOld.FacilityType;
                                objSLNew.FinalForm = objSLOld.FinalForm;
                                objSLNew.FinalPackaging = objSLOld.FinalPackaging;
                                objSLNew.FlowRate = objSLOld.FlowRate;
                                objSLNew.FlowRateCubicMeterPerHour = objSLOld.FlowRateCubicMeterPerHour;
                                objSLNew.FlowRateLiterPerMin = objSLOld.FlowRateLiterPerMin;
                                objSLNew.FlowVelocity = objSLOld.FlowVelocity;
                                objSLNew.ForeignMaterial = objSLOld.ForeignMaterial;
                                objSLNew.Frequency = objSLOld.Frequency;
                                objSLNew.GISStatus = objSLOld.GISStatus;
                                objSLNew.GravelContent = objSLOld.GravelContent;
                                objSLNew.GrossWeight = objSLOld.GrossWeight;
                                objSLNew.GroupSample = objSLOld.GroupSample;
                                objSLNew.Hold = objSLOld.Hold;
                                objSLNew.Humidity = objSLOld.Humidity;
                                objSLNew.IceCycle = objSLOld.IceCycle;
                                objSLNew.Increments = objSLOld.Increments;
                                objSLNew.Interval = objSLOld.Interval;
                                objSLNew.IsActive = objSLOld.IsActive;
                                //objSLNew.IsNotTransferred = objSLOld.IsNotTransferred;
                                objSLNew.ItemName = objSLOld.ItemName;
                                objSLNew.KeyMap = objSLOld.KeyMap;
                                objSLNew.LicenseNumber = objSLOld.LicenseNumber;
                                objSLNew.ManifestNo = objSLOld.ManifestNo;
                                objSLNew.MonitoryingRequirement = objSLOld.MonitoryingRequirement;
                                objSLNew.NoOfCollectionsEachTime = objSLOld.NoOfCollectionsEachTime;
                                objSLNew.NoOfPoints = objSLOld.NoOfPoints;
                                objSLNew.Notes = objSLOld.Notes;
                                objSLNew.OriginatingEntiry = objSLOld.OriginatingEntiry;
                                objSLNew.OriginatingLicenseNumber = objSLOld.OriginatingLicenseNumber;
                                objSLNew.PackageNumber = objSLOld.PackageNumber;
                                objSLNew.ParentSampleDate = objSLOld.ParentSampleDate;
                                objSLNew.ParentSampleID = objSLOld.ParentSampleID;
                                objSLNew.PiecesPerUnit = objSLOld.PiecesPerUnit;
                                objSLNew.Preservetives = objSLOld.Preservetives;
                                objSLNew.ProjectName = objSLOld.ProjectName;
                                objSLNew.PurifierSampleID = objSLOld.PurifierSampleID;
                                objSLNew.PWSID = objSLOld.PWSID;
                                objSLNew.PWSSystemName = objSLOld.PWSSystemName;
                                objSLNew.RegionNameOfSection = objSLOld.RegionNameOfSection;
                                objSLNew.RejectionCriteria = objSLOld.RejectionCriteria;
                                objSLNew.RepeatLocation = objSLOld.RepeatLocation;
                                objSLNew.RetainedWeight = objSLOld.RetainedWeight;
                                objSLNew.RiverWidth = objSLOld.RiverWidth;
                                objSLNew.RushSample = objSLOld.RushSample;
                                objSLNew.SampleAmount = objSLOld.SampleAmount;
                                objSLNew.SampleCondition = objSLOld.SampleCondition;
                                objSLNew.SampleDescription = objSLOld.SampleDescription;
                                objSLNew.SampleImage = objSLOld.SampleImage;
                                objSLNew.SampleName = objSLOld.SampleName;
                                objSLNew.SamplePointID = objSLOld.SamplePointID;
                                objSLNew.SamplePointType = objSLOld.SamplePointType;
                                if (!objSLOld.IsReanalysis)
                                {
                                    objSLNew.SampleSource = objSLOld.SampleSource;
                                }
                                objSLNew.SampleTag = objSLOld.SampleTag;
                                objSLNew.SampleWeight = objSLOld.SampleWeight;
                                objSLNew.SamplingAddress = objSLOld.SamplingAddress;
                                objSLNew.SamplingEquipment = objSLOld.SamplingEquipment;
                                objSLNew.SamplingLocation = objSLOld.SamplingLocation;
                                objSLNew.SamplingProcedure = objSLOld.SamplingProcedure;
                                objSLNew.SequenceTestSampleID = objSLOld.SequenceTestSampleID;
                                objSLNew.SequenceTestSortNo = objSLOld.SequenceTestSortNo;
                                objSLNew.ServiceArea = objSLOld.ServiceArea;
                                objSLNew.SiteCode = objSLOld.SiteCode;
                                objSLNew.SiteDescription = objSLOld.SiteDescription;
                                objSLNew.SiteID = objSLOld.SiteID;
                                objSLNew.SiteNameArchived = objSLOld.SiteNameArchived;
                                objSLNew.SiteUserDefinedColumn1 = objSLOld.SiteUserDefinedColumn1;
                                objSLNew.SiteUserDefinedColumn2 = objSLOld.SiteUserDefinedColumn2;
                                objSLNew.SiteUserDefinedColumn3 = objSLOld.SiteUserDefinedColumn3;
                                objSLNew.SubOut = objSLOld.SubOut;
                                objSLNew.SystemType = objSLOld.SystemType;
                                objSLNew.TargetMGTHC_CBD_mg_pc = objSLOld.TargetMGTHC_CBD_mg_pc;
                                objSLNew.TargetMGTHC_CBD_mg_unit = objSLOld.TargetMGTHC_CBD_mg_unit;
                                objSLNew.TargetPotency = objSLOld.TargetPotency;
                                objSLNew.TargetUnitWeight_g_pc = objSLOld.TargetUnitWeight_g_pc;
                                objSLNew.TargetUnitWeight_g_unit = objSLOld.TargetUnitWeight_g_unit;
                                objSLNew.TargetWeight = objSLOld.TargetWeight;
                                objSLNew.Time = objSLOld.Time;
                                objSLNew.TimeEnd = objSLOld.TimeEnd;
                                objSLNew.TimeStart = objSLOld.TimeStart;
                                objSLNew.TotalSamples = objSLOld.TotalSamples;
                                objSLNew.TotalTimes = objSLOld.TotalTimes;
                                objSLNew.WindDirection = objSLOld.WindDirection;
                                objSLNew.Temp = objSLOld.Temp;
                                objSLNew.WeatherCondition = objSLOld.WeatherCondition;
                                objSLNew.Transparencyk = objSLOld.Transparencyk;
                                objSLNew.Transparencyk1 = objSLOld.Transparencyk1;
                                objSLNew.Transparencyk2 = objSLOld.Transparencyk2;
                                if (objSLOld.TtimeUnit != null)
                                {
                                    objSLNew.TtimeUnit = uow.GetObjectByKey<Unit>(objSLOld.TtimeUnit.Oid);
                                }
                                objSLNew.WaterType = objSLOld.WaterType;
                                objSLNew.ZipCode = objSLOld.ZipCode;
                                //objSLNew.ModifiedBy = objSLOld.ModifiedBy;
                                if (objSLOld.ModifiedBy != null)
                                {
                                    objSLNew.ModifiedBy = uow.GetObjectByKey<Modules.BusinessObjects.Hr.CustomSystemUser>(objSLOld.ModifiedBy.Oid);
                                }
                                objSLNew.ModifiedDate = objSLOld.ModifiedDate;
                                objSLNew.Comment = objSLOld.Comment;
                                objSLNew.Latitude = objSLOld.Latitude;
                                objSLNew.Longitude = objSLOld.Longitude;
                                List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType.QCTypeName == "Sample" && j.Sampling.Where(a => a.Oid == objSLOld.Oid).Count() > 0).ToList();
                                foreach (var objLineA in lsttp)
                                {
                                    objSLNew.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                                }
                                foreach (var objSampleparameter in objSLOld.SamplingParameter.Where(a => a.IsGroup == true && a.GroupTest != null).ToList())
                                {
                                    SamplingParameter sample = objSLNew.SamplingParameter.FirstOrDefault<SamplingParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                    if (objSampleparameter.GroupTest != null && sample != null)
                                    {
                                        sample.IsGroup = true;
                                        sample.GroupTest = uow.GetObjectByKey<GroupTestMethod>(objSampleparameter.GroupTest.Oid);
                                    }
                                }
                                foreach (var objSampleparameter in objSLOld.SamplingParameter.Where(a => a.SubOut == true).ToList())
                                {
                                    SamplingParameter sample = objSLNew.SamplingParameter.FirstOrDefault<SamplingParameter>(obj => obj.Testparameter.Oid == objSampleparameter.Testparameter.Oid);
                                    if (sample != null)
                                    {
                                        sample.SubOut = true;
                                    }
                                }
                                objSLNew.Save();
                                SampleNo++;
                                if (smplold != null && smplold.Count > 0)
                                {
                                    foreach (SamplingBottleAllocation smpl in smplold.ToList())
                                    {
                                        SamplingBottleAllocation smplnew = new SamplingBottleAllocation(uow);
                                        smplnew.Sampling = objSLNew;
                                        smplnew.TestMethod = uow.GetObjectByKey<TestMethod>(smpl.TestMethod.Oid);
                                        smplnew.BottleID = smpl.BottleID;
                                        if (smpl.Containers != null)
                                        {
                                            smplnew.Containers = uow.GetObjectByKey<Container>(smpl.Containers.Oid);
                                        }
                                        if (smpl.Preservative != null)
                                        {
                                            smplnew.Preservative = uow.GetObjectByKey<Preservative>(smpl.Preservative.Oid);
                                        }
                                        if (smpl.StorageID != null)
                                        {
                                            smplnew.StorageID = uow.GetObjectByKey<Storage>(smpl.StorageID.Oid);
                                        }
                                        if (smpl.StorageCondition != null)
                                        {
                                            smplnew.StorageCondition = uow.GetObjectByKey<PreserveCondition>(smpl.StorageCondition.Oid);
                                        }
                                    }
                                }
                                if (objSLNew.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                {
                                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSLNew.SamplingProposal.Oid, OperationType.Created, "Sampling Proposal", objSLNew.SamplingProposal.RegistrationID, "Samples", "", objSLNew.SampleID, "");
                                }
                             
                            }
                            uow.CommitChanges();
                        }           
                        objCopySampleInfo.NoOfSamples = 0;
                        objSampleInfo.boolCopySamples = false;
                        if (Frame is NestedFrame)
                        {
                            NestedFrame nestedFrame = (NestedFrame)Frame;
                            CompositeView view = nestedFrame.ViewItem.View;
                            foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                            {
                                if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                                {
                                    if (frameContainer.Frame.View is DetailView)
                                    {
                                        frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                                    }
                                    else
                                    {
                                        (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                                    }
                                    frameContainer.Frame.View.Refresh();
                                }
                            }
                        }
                        View.Refresh();
                        View.RefreshDataSource();
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void CopySamples_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count == 0)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                else if (View != null && View.SelectedObjects.Count > 1)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void View_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.ObjectTypeInfo.Type == typeof(Sampling))
                {
                    if (View.CurrentObject != null)
                    {
                        Sampling sl = (Sampling)View.CurrentObject;
                        if (sl.VisualMatrix != null)
                        {
                            objSampleInfo.SLVisualMatrixName = sl.VisualMatrix.MatrixName.MatrixName;
                        }
                        if (sl.SamplingProposal != null)
                        {
                            objSampleInfo.RegistrationID = View.ObjectSpace.GetKeyValue(View.CurrentObject).ToString();
                            objSampleInfo.focusedJobID = sl.SamplingProposal.RegistrationID;
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
            try
            {
                if (View.Id == "Sampling_ListView_SamplingProposal")
                {
                    Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active.RemoveItem("DisableUnsavedChangesController");
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) {
                                        if( e.focusedColumn.fieldName == 'SampleSource')
                                             {
                                              e.cancel = true;
                                              }
                                          else
                                               {
                                                    e.cancel = false;
                                               }
                                           }";
                        ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        selparameter.CallbackManager.RegisterHandler("SampleSourcePopup", this);
                    }
                }
                else if (View.Id == "Sampling_ListView_SourceSample")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ICallbackManagerHolder selparameter = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    selparameter.CallbackManager.RegisterHandler("SampleSourceSelected", this);
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load += Grid_Load;
                        gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                          if (e.visibleIndex != '-1')
                          {
                            s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                             if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                                RaiseXafCallback(globalCallbackControl, 'SampleSourceSelected', 'Selected|' + Oidvalue , '', false);    
                             }
                            }); 
                          }             
                        }";
                    }
                }
                else if(View.Id== "Sampling_LookupListView_CopyTest_SampleID")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 320;
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                }
                else if (View.Id == "SampleSites_LookupListView_Sampling" ||View.Id== "SampleSites_LookupListView_Sampling_StationLocation")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                        gridListEditor.Grid.Settings.VerticalScrollableHeight = 410;
                        gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
           
        }
        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                if (View.Id== "Sampling_ListView_SourceSample")
                {
                    ASPxGridView gridview = (ASPxGridView)sender;
                    if (gridview != null)
                    {
                        var selectionBoxColumn = gridview.Columns.OfType<GridViewCommandColumn>().Where(i => i.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                        }
                        if (SampleOid != Guid.Empty)
                        {
                            gridview.Selection.UnselectRowByKey(SampleOid);
                            SampleOid = Guid.Empty;
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

        private void Grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "Sampling_ListView_SamplingProposal" && objPermissionInfo.SamplingProposalIsWrite)
                {
                    if (e.DataColumn.FieldName == "SampleSource" || e.DataColumn.FieldName == "AlternativeStation" || e.DataColumn.FieldName== "StationLocation.Oid")
                    {
                    e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'SampleSourcePopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
                    }
                    else
                    {
                        return;
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
            try
            {
                if (View.Id == "Sampling_ListView_SamplingProposal")
                {
                    View.SelectionChanged -= View_SelectionChanged;
                    SamplingCopySamples.Executing -= CopySamples_Executing;
                    SamplingSL_CopyTest.Executing -= SL_CopyTest_Executing;
                }
                else if (View.Id == "Sampling_ListView_SourceSample")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        gridListEditor.Grid.Load -= Grid_Load;
                    }
                }
                else if (View.Id == "SampleSites_LookupListView_Sampling" || View.Id == "SampleSites_LookupListView_Sampling_StationLocation")
                {
                    View.ControlsCreated -= View_ControlsCreated;
                }
                else if (View.Id == "SamplingProposal_SampleLogin")
                {
                    View.Closing -= View_Closing;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        public void ProcessAction(string parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    string[] param = parameter.Split('|');
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor!=null)
                    {
                    if (param[0] == "SampleSource")
                    {
                        if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                        {
                            object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                            IObjectSpace os = Application.CreateObjectSpace();
                            Sampling objSample = os.GetObjectByKey<Sampling>(currentOid);
                            if (objSample != null && objSample.IsReanalysis)
                            {
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(labwareObjectSpace, typeof(Sampling));
                                List<Sampling> lstSamples = os.GetObjects<Sampling>(CriteriaOperator.Parse("SamplingProposal.Oid=?", objSample.SamplingProposal.Oid)).ToList();
                                cs.Criteria["Filter"] = CriteriaOperator.Parse("SamplingProposal.RegistrationID='" + objSample.SamplingProposal.RegistrationID + "'");
                                cs.Criteria["Filter2"] = new InOperator("Oid", lstSamples.Where(i => Convert.ToInt32(i.SampleNo) < Convert.ToInt32(objSample.SampleNo)).Select(i => i.Oid));
                                if (!string.IsNullOrEmpty(objSample.SampleSource))
                                {
                                    cs.Criteria["Filter1"] = CriteriaOperator.Parse("[SampleID] <> ?", objSample.SampleSource);
                                }
                                showViewParameters.CreatedView = Application.CreateListView("Sampling_ListView_SourceSample", cs, false);
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.Accepting += Dc_Accepting;
                                dc.SaveOnAccept = false;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            }
                        }
                    }
                    else if (param[0] == "Selected")
                    {
                        if (HttpContext.Current.Session["rowid"] != null && editor != null)
                        {
                            Guid SampleOid = new Guid(HttpContext.Current.Session["rowid"].ToString());
                            Sampling objSample = View.ObjectSpace.GetObjectByKey<Sampling>(SampleOid);
                            if (objSample != null)
                            {
                                if (objSample.Testparameters.Count > 0)
                                {
                                    Sampling objCurrentSample = (Sampling)View.CurrentObject;
                                    if (objCurrentSample != null)
                                    {
                                        editor.Grid.Selection.UnselectRowByKey(objCurrentSample.Oid);
                                        Application.ShowViewStrategy.ShowMessage("Please removed the test in sample.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                }

                            }
                        }
                    }
                        else if (param[0] == "AlternativeStation")
                        {
                            if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                            {
                                object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                                HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                                HttpContext.Current.Session["AlternativeStation"] = editor.Grid.GetRowValues(int.Parse(param[1]), "AlternativeStationOid");
                                IObjectSpace os = Application.CreateObjectSpace();
                                Sampling objSample = os.GetObjectByKey<Sampling>(currentOid);
                                if (objSample != null)
                                {
                                    ShowViewParameters showViewParameters = new ShowViewParameters();
                                    IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                    CollectionSource cs = new CollectionSource(labwareObjectSpace, typeof(SampleSites));
                                    cs.Criteria["Filter1"] = CriteriaOperator.Parse("[Client.Oid] = ?", objSample.SamplingProposal.ClientName.Oid);
                                    showViewParameters.CreatedView = Application.CreateListView("SampleSites_LookupListView_Sampling", cs, false);
                                    showViewParameters.Context = TemplateContext.PopupWindow;
                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    DialogController dc = Application.CreateController<DialogController>();
                                    dc.AcceptAction.Execute += AcceptAction_Execute_AlternativeStation;
                                    dc.SaveOnAccept = false;
                                    showViewParameters.Controllers.Add(dc);
                                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                }
                            }
                        } 
                        else if (param[0] == "StationLocation.Oid")
                        {
                            if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                            {
                                object currentOid = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                                HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                                HttpContext.Current.Session["StationLocation"] = editor.Grid.GetRowValues(int.Parse(param[1]), "StationLocation.Oid");
                                IObjectSpace os = Application.CreateObjectSpace();
                                Sampling objSample = os.GetObjectByKey<Sampling>(currentOid);
                                if (objSample != null)
                                {
                                    ShowViewParameters showViewParameters = new ShowViewParameters();
                                    IObjectSpace labwareObjectSpace = Application.CreateObjectSpace();
                                    CollectionSource cs = new CollectionSource(labwareObjectSpace, typeof(SampleSites));
                                    cs.Criteria["Filter1"] = CriteriaOperator.Parse("[Client.Oid] = ?", objSample.SamplingProposal.ClientName.Oid);
                                    showViewParameters.CreatedView = Application.CreateListView("SampleSites_LookupListView_Sampling_StationLocation", cs, false);
                                    showViewParameters.Context = TemplateContext.PopupWindow;
                                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                    DialogController dc = Application.CreateController<DialogController>();
                                    dc.AcceptAction.Execute += AcceptAction_Execute_StationLocation;
                                    dc.Accepting += Dc_Accepting_StationLocation;
                                    dc.SaveOnAccept = false;
                                    showViewParameters.Controllers.Add(dc);
                                    Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                                }
                            }
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

        private void Dc_Accepting_StationLocation(object sender, DialogControllerAcceptingEventArgs e)
        {
           try
            {
                if(e.AcceptActionArgs.SelectedObjects.Count>1)
                {
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void AcceptAction_Execute_StationLocation(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["rowid"] != null)
                {
                    Sampling objsampling = ((ListView)View).CollectionSource.List.Cast<Sampling>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                    SampleSites objSite=e.SelectedObjects.Cast<SampleSites>().FirstOrDefault();
                    if (objSite!=null)
                    {
                        objSite = View.ObjectSpace.GetObjectByKey<SampleSites>(objSite.Oid);
                        if (objsampling != null)
                        {
                            objsampling.StationLocation = objSite;
                            objsampling.PWSID = objSite.PWSID;
                            objsampling.KeyMap = objSite.KeyMap;
                            objsampling.Address = objSite.Address;
                            objsampling.SamplePointID = objSite.SamplePointID;
                            objsampling.SamplePointType = objSite.SamplePointType;
                            objsampling.SystemType = objSite.SystemType;
                            objsampling.PWSSystemName = objSite.PWSSystemName;
                            objsampling.RejectionCriteria = objSite.RejectionCriteria;
                            objsampling.WaterType = objSite.WaterType;
                            objsampling.ParentSampleID = objSite.ParentSampleID;
                            objsampling.ParentSampleDate = objSite.ParentSampleDate;
                        }
                    }
                    else
                    {
                        objsampling.StationLocation = null;
                        objsampling.PWSID = null;
                        objsampling.KeyMap = null;
                        objsampling.Address = null;
                        objsampling.SamplePointID = null;
                        objsampling.SamplePointType = null;
                        objsampling.SystemType = null;
                        objsampling.PWSSystemName = null;
                        objsampling.RejectionCriteria = null;
                        objsampling.WaterType = null;
                        objsampling.ParentSampleID = null;
                        objsampling.ParentSampleDate = null;
                    }
                   ((ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_Execute_AlternativeStation(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                string assigned = string.Empty;
                string assignedOid = string.Empty;
                assigned=string.Join("; ", e.SelectedObjects.Cast<SampleSites>().Where(i => i.SiteName != null && !string.IsNullOrEmpty(i.SiteName)).Select(i => i.SiteName).Distinct().ToList());
                assignedOid= string.Join("; ", e.SelectedObjects.Cast<SampleSites>().Where(i => i.SiteName != null && !string.IsNullOrEmpty(i.SiteName)).Select(i => i.Oid).Distinct().ToList());
                if (HttpContext.Current.Session["rowid"] != null)
                {
                    Sampling objsampling = ((ListView)View).CollectionSource.List.Cast<Sampling>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                    if (objsampling != null)
                    {
                        objsampling.AlternativeStation = assigned;
                        objsampling.AlternativeStationOid = assignedOid;
                    }
                    ((ListView)View).Refresh();

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SL_CopyTest_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (View != null && View.SelectedObjects.Count == 0)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
                else if (View != null && View.SelectedObjects.Count > 1)
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlyonechkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count == 1)
                {
                    Sampling objSample = (Sampling)e.AcceptActionArgs.CurrentObject;
                    Session currentSession = ((XPObjectSpace)(View.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    if (objSample != null && HttpContext.Current.Session["rowid"] != null)
                    {
                        Sampling objSampleLogin = uow.FindObject<Sampling>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                        if (objSampleLogin != null && objSampleLogin.SampleSource != objSample.SampleID)
                        {
                            int sysSampleno = uow.Query<Sampling>().Where(i => i.SampleSource != null && i.SampleSource == objSample.SampleID).ToList().Count();
                            sysSampleno++;
                            objSampleLogin.SampleSource = objSample.SampleID;
                            objSampleLogin.SysSampleCode = objSample.SampleID + "LR" + sysSampleno;
                            List<Testparameter> lsttp = uow.Query<Testparameter>().Where(j => j.QCType != null && j.QCType.QCTypeName == "Sample" && j.Sampling.Where(a => a.Oid == objSample.Oid).Count() > 0).ToList();
                            foreach (var objLineA in lsttp)
                            {
                                objSampleLogin.Testparameters.Add(uow.GetObjectByKey<Testparameter>(objLineA.Oid));
                            }
                            uow.CommitChanges();
                            View.ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                        }
                    }
                }
                else
                {
                    if (e.AcceptActionArgs.SelectedObjects.Count == 0)
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectonlychk"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SL_CopyTest_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                List<string> FocusedMatrix = new List<string>();
               
                if (View != null && View.Id == "Sampling_ListView_SamplingProposal" && View.SelectedObjects.Count == 1)
                {
                    IObjectSpace objspace = Application.CreateObjectSpace();
                    foreach (Sampling objtp in View.SelectedObjects)
                    {
                        Sampling objSample = objspace.GetObject(objtp);
                        if (objSample != null)
                        {
                            foreach (Testparameter testparameter in objSample.Testparameters)
                            {
                                TPOid.Add(testparameter.Oid);
                                FocusedMatrix.Add(testparameter.TestMethod.MatrixName.MatrixName.ToString());
                            }
                            objSampleInfo.SLOid = objtp.Oid.ToString();
                        }
                    }
                    object objToShow = objspace.CreateObject(typeof(Sampling));
                    if (objToShow != null)
                    {
                        CollectionSource cs = new CollectionSource(objspace, typeof(Sampling));
                        cs.Criteria.Clear();
                        if (View.Id == "Sampling_ListView_SamplingProposal" && !string.IsNullOrEmpty(objSampleInfo.SLOid))
                        {
                            cs.Criteria["filter1"] = CriteriaOperator.Parse("[SamplingProposal.RegistrationID]='" + objSampleInfo.focusedJobID + "' and Oid <> ?", new Guid(objSampleInfo.SLOid));
                        }
                        else
                        {
                            cs.Criteria["filter1"] = CriteriaOperator.Parse("[SamplingProposal.RegistrationID]='" + objSampleInfo.focusedJobID + "'");
                        }
                        cs.Criteria["filter2"] = new InOperator("VisualMatrix.MatrixName.MatrixName", FocusedMatrix);
                        ListView dvbottleAllocation = Application.CreateListView("Sampling_LookupListView_CopyTest_SampleID", cs, false);
                        ShowViewParameters showViewParameters = new ShowViewParameters(dvbottleAllocation);
                        showViewParameters.CreatedView = dvbottleAllocation;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.Accepting += CopyTest_Accepting;
                        dc.CloseOnCurrentObjectProcessing = false;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void CopyTest_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count>0)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                    IList<Testparameter> objtp = null;
                    SamplingProposal objJobId = null;
                    if (TPOid.Count > 0)
                    {
                        XPClassInfo TestParameterinfo;
                        TestParameterinfo = uow.GetClassInfo(typeof(Testparameter));
                        objtp = uow.GetObjects(TestParameterinfo, new InOperator("Oid", TPOid), null, int.MaxValue, false, true).Cast<Testparameter>().ToList();
                        CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + objSampleInfo.SLOid + "'");
                        objParentSL = uow.FindObject<Sampling>(criteria);
                    }
                    List<Guid> lstSampleOid = new List<Guid>();
                    foreach (Sampling obj in e.AcceptActionArgs.SelectedObjects)
                    {
                        if (objtp != null && obj.Testparameters != null)
                        {
                            lstSampleOid.Add(obj.Oid);
                            if (objJobId == null)
                            {
                                objJobId = obj.SamplingProposal;
                            }
                            CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + obj.Oid + "'");
                            Sampling objSL = uow.FindObject<Sampling>(criteria);
                            IList<SamplingParameter> objsp = (IList<SamplingParameter>)objParentSL.SamplingParameter;
                            if (objsp != null)
                            {
                                IList<SamplingParameter> lsts = ObjectSpace.GetObjects<SamplingParameter>(CriteriaOperator.Parse("[Sampling.Oid]=? ", objSL.Oid));
                                foreach (SamplingParameter item in lsts)
                                {
                                    var parametersToRemove = objSL.Testparameters.Where(objTestSL => !objtp.Contains(objTestSL) && string.IsNullOrEmpty(item.uqQCBatchID) && string.IsNullOrEmpty(item.PrepBatchID) && string.IsNullOrEmpty(item.Result)).ToList();
                                    foreach (var parameterToRemove in parametersToRemove)
                                    {
                                        objSL.Testparameters.Remove(parameterToRemove);
                                    }
                                }
                                foreach (Testparameter objtestperam in objtp)
                                {
                                    if (!objSL.Testparameters.Contains(objtestperam))
                                    {
                                        foreach (SamplingParameter sp in objsp)
                                        {
                                            if (sp != null)
                                            {
                                                if (sp.Testparameter != null && objtestperam.Oid == sp.Testparameter.Oid)
                                                {
                                                    objSL.Testparameters.Add(objtestperam);
                                                }
                                            }
                                        }
                                    }
                                    if (objSL.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                    {
                                        Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSL.SamplingProposal.Oid, OperationType.Created, "Sampling Proposal", objSL.SampleID, "Test", "", objtestperam.TestMethod.TestName + " | " + objtestperam.Parameter.ParameterName, "");
                                    }
                                }
                                objSL.Save();
                                uow.CommitChanges();
                                Application.ShowViewStrategy.ShowMessage("Copied tests applied successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                            }
                            Frame.GetController<TaskManagementViewController>().AssignBottleAllocationToSamples(uow, objSL.Oid);
                        }
                    }
                    if (TPOid.Count > 0)
                    {
                        TPOid.Clear();
                    }

                    //foreach (Sampling obj in e.AcceptActionArgs.SelectedObjects)
                    //{
                    //    if (objtp != null && obj.Testparameters != null)
                    //    {

                    //        lstSampleOid.Add(obj.Oid);
                    //        if (objJobId == null)
                    //        {
                    //            objJobId = obj.SamplingProposal;
                    //        }
                    //        CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + obj.Oid + "'");
                    //        Sampling objSL = uow.FindObject<Sampling>(criteria);
                    //        IList<SamplingParameter> objsp = (IList<SamplingParameter>)objParentSL.SamplingParameter;
                    //        if (objsp != null)
                    //        {
                    //            foreach (Testparameter objtestperam in objtp)
                    //            {
                    //                foreach (Testparameter objTestSL in objSL.Testparameters.ToList())
                    //                {
                    //                    if (objTestSL != objtestperam)
                    //                    {
                    //                        objSL.Testparameters.Remove(objTestSL);
                    //                    }
                    //                }
                    //            }
                    //            foreach (Testparameter objtestperam in objtp)
                    //            {
                    //                if (!objSL.Testparameters.Contains(objtestperam))
                    //                {
                    //                    foreach (SamplingParameter sp in objsp)
                    //                    {
                    //                        if (sp != null)
                    //                        {
                    //                            if (objtestperam.Oid == sp.Testparameter.Oid)
                    //                            {
                    //                                objSL.Testparameters.Add(objtestperam);
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                if (objSL.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                    //                {
                    //                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSL.SamplingProposal.Oid, OperationType.Created, "Sampling Proposal", objSL.SampleID, "Test", "", objtestperam.TestMethod.TestName + " | " + objtestperam.Parameter.ParameterName, "");
                    //                }
                    //            }
                    //            objSL.Save();
                    //            uow.CommitChanges();
                    //        }
                    //        Frame.GetController<TaskManagementViewController>().AssignBottleAllocationToSamples(uow, objSL.Oid);
                    //        Sampling objSample = uow.GetObjectByKey<Sampling>(objSL.Oid);
                    //        IList<SamplingParameter> childsampleparemter = uow.GetObjects(uow.GetClassInfo(typeof(SamplingParameter)), CriteriaOperator.Parse("[Sampling]=?", objSample.Oid), null, int.MaxValue, false, true).Cast<SamplingParameter>().ToList();

                    //        foreach (SamplingParameter Parentsp in objsp)
                    //        {
                    //            if (Parentsp != null)
                    //            {
                    //                foreach (SamplingParameter CopiedSpPara in childsampleparemter)
                    //                {
                    //                    if (CopiedSpPara != null && Parentsp.Testparameter.Oid == CopiedSpPara.Testparameter.Oid)
                    //                    {
                    //                        if (Parentsp.TAT != null)
                    //                        {
                    //                            CopiedSpPara.TAT = uow.GetObjectByKey<TurnAroundTime>(Parentsp.TAT.Oid);
                    //                        }
                    //                        CopiedSpPara.SubOut = Parentsp.SubOut;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //        uow.CommitChanges();
                    //    }
                    //}
                    //if (TPOid.Count > 0)
                    //{
                    //    TPOid.Clear();
                    //}

                    if (Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        CompositeView view = nestedFrame.ViewItem.View;
                        foreach (IFrameContainer frameContainer in view.GetItems<IFrameContainer>())
                        {
                            if ((frameContainer.Frame != null) && (frameContainer.Frame.View != null) && (frameContainer.Frame.View.ObjectSpace != null))
                            {
                                if (frameContainer.Frame.View is DetailView)
                                {
                                    frameContainer.Frame.View.ObjectSpace.ReloadObject(frameContainer.Frame.View.CurrentObject);
                                }
                                else
                                {
                                    (frameContainer.Frame.View as DevExpress.ExpressApp.ListView).CollectionSource.Reload();
                                }
                                frameContainer.Frame.View.Refresh();
                            }
                        }
                    }
                    View.Refresh();
                    Application.ShowViewStrategy.ShowMessage("Copied Tests applied successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
                else
                {
                    e.Cancel = true;
                    Application.ShowViewStrategy.ShowMessage("Please select a checkbox.", InformationType.Error, 3000, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (((ListView)View).CollectionSource.List.Cast<Sampling>().FirstOrDefault(i=>i.StationLocation==null)!=null)
                {
                    Application.ShowViewStrategy.ShowMessage("Enter the stationlocation.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    return;
                }
                else
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        gridListEditor.Grid.UpdateEdit();
                        if (View.ObjectSpace.ModifiedObjects.Count > 0)
                        {
                            View.ObjectSpace.CommitChanges();
                            View.ObjectSpace.Refresh();
                        }
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch(Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

    }
}
