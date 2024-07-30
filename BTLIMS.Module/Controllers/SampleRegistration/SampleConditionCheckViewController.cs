using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Pdf;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.SampleRegistration
{

    public partial class SampleConditionCheckViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        public SampleConditionCheckViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleConditionCheckPoint_ListView;" + "Samplecheckin_SampleConditionCheck_ListView;" + "SampleConditionCheck_DetailView_Copy;"
                + "SampleConditionCheck_DetailView_Tasks;" + "Tasks_SampleConditionCheck_ListView;" + "SampleConditionCheckPoint_ListView_Task;";
            SCC_Report.TargetViewId = "Samplecheckin_SampleConditionCheck_ListView;" + "SampleConditionCheck_DetailView_Copy;"
                + "Tasks_SampleConditionCheck_ListView;";
            PopupSCC_Report.TargetViewId = "SampleConditionCheck_DetailView_Copy;" + "SampleConditionCheck_DetailView_Tasks;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            try
            {
                //if (View.Id == "SampleConditionCheck_DetailView_Copy")
                //{
                //    DialogController dialogController = Frame.GetController<DialogController>();
                //    //dialogController.AcceptAction.Executing += AcceptAction_Executing;
                //}
                View.ControlsCreated += View_ControlsCreated;
                if (View.Id == "SampleConditionCheck_DetailView_Copy" || View.Id == "SampleConditionCheck_DetailView_Tasks")
                {
                    SampleConditionCheck obj = (SampleConditionCheck)View.CurrentObject;
                    if (obj != null && View.ObjectSpace.IsNewObject(obj) == true)
                    {
                        PopupSCC_Report.Enabled["enable"] = false;
                    }
                    else
                    {
                        PopupSCC_Report.Enabled["enable"] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void AcceptAction_Executing(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    if (View.Id == "SampleConditionCheck_DetailView_Copy")
        //    {
        //        ListPropertyEditor list = ((DetailView)View).FindItem("SampleConditionCheckComment") as ListPropertyEditor;
        //        if (list.ListView.CollectionSource.List.Count == 0)
        //        {
        //            Application.ShowViewStrategy.ShowMessage("Comment sholud not be empty", InformationType.Error, 3000, InformationPosition.Top);
        //            e.Cancel = true;
        //        }
        //    }
        //}

        private void View_ControlsCreated(object sender, EventArgs e)
        {
            try
            {
                View.ControlsCreated -= View_ControlsCreated;
                if (View.Id == "SampleConditionCheckPoint_ListView")
                {
                    if (((ListView)View).CollectionSource.GetCount() == 0)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null)
                        {
                            CompositeView view = nestedFrame.ViewItem.View;
                            if (view != null)
                            {
                                SampleConditionCheck obj = (SampleConditionCheck)view.CurrentObject;
                                if (Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "Samplecheckin_DetailView_Copy_SampleRegistration")
                                {
                                    Samplecheckin objSamplecheckin = (Samplecheckin)Application.MainWindow.View.CurrentObject;
                                    if (objSamplecheckin != null && objSamplecheckin.SampleMatries != null)
                                    {
                                        List<Guid> lstSMOid = new List<Guid>();
                                        HttpContext.Current.Session["SMOid"] = objSamplecheckin.SampleMatries;
                                        if (HttpContext.Current.Session["SMOid"] != null)
                                        {
                                            string[] SampleMatrix = HttpContext.Current.Session["SMOid"].ToString().Split(new string[] { "; " }, StringSplitOptions.None);
                                            foreach (string val in SampleMatrix)
                                            {
                                                VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(val.Trim()));
                                                if (objVM != null)
                                                {
                                                    IList<SampleConditionCheckData> datas = ObjectSpace.GetObjects<SampleConditionCheckData>(CriteriaOperator.Parse("Contains([SampleMatrices], ?)", objVM.VisualMatrixName/*.Replace(" ", "")*/));
                                                    foreach (SampleConditionCheckData obja in datas)
                                                    {
                                                        if (!lstSMOid.Contains(obja.Oid))
                                                            lstSMOid.Add(obja.Oid);
                                                    }
                                                }
                                            }
                                        }
                                        if (obj != null)
                                        {
                                            IList<SampleConditionCheckData> datas = ObjectSpace.GetObjects<SampleConditionCheckData>(new InOperator("Oid", lstSMOid));
                                            foreach (SampleConditionCheckData data in datas.OrderBy(a => a.Sort).ToList())
                                            {
                                                SampleConditionCheckPoint point = ObjectSpace.CreateObject<SampleConditionCheckPoint>();
                                                point.CheckPoint = data;
                                                if (data.PickAnswer == PickAnswer.Yes)
                                                {
                                                    point.Yes = true;
                                                }
                                                else if (data.PickAnswer == PickAnswer.No)
                                                {
                                                    point.No = true;
                                                }
                                                else if (data.PickAnswer == PickAnswer.None)
                                                {
                                                    point.NA = true;
                                                }
                                                obj.SampleConditionCheckPoint.Add(point);
                                            }
                                        }
                                    }
                                }
                                else if (Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "SampleRegistration")
                                {
                                    DashboardViewItem dvsamplecheckin = ((DashboardView)Application.MainWindow.View).FindItem("SampleCheckin") as DashboardViewItem;
                                    if (dvsamplecheckin != null && dvsamplecheckin.InnerView != null)
                                    {
                                        Samplecheckin objSamplecheckin = (Samplecheckin)dvsamplecheckin.InnerView.CurrentObject;
                                        if (objSamplecheckin != null && objSamplecheckin.SampleMatries != null)
                                        {
                                            List<Guid> lstSMOid = new List<Guid>();
                                            HttpContext.Current.Session["SMOid"] = objSamplecheckin.SampleMatries;
                                            if (HttpContext.Current.Session["SMOid"] != null)
                                            {
                                                string[] SampleMatrix = HttpContext.Current.Session["SMOid"].ToString().Split(new string[] { "; " }, StringSplitOptions.None);
                                                foreach (string val in SampleMatrix)
                                                {
                                                    VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(val.Trim()));
                                                    if (objVM != null)
                                                    {
                                                        IList<SampleConditionCheckData> datas = ObjectSpace.GetObjects<SampleConditionCheckData>(CriteriaOperator.Parse("Contains([SampleMatrices], ?)", objVM.VisualMatrixName/*.Replace(" ", "")*/));
                                                        foreach (SampleConditionCheckData obja in datas)
                                                        {
                                                            if (!lstSMOid.Contains(obja.Oid))
                                                                lstSMOid.Add(obja.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                            if (obj != null)
                                            {
                                                IList<SampleConditionCheckData> datas = ObjectSpace.GetObjects<SampleConditionCheckData>(new InOperator("Oid", lstSMOid));
                                                foreach (SampleConditionCheckData data in datas.OrderBy(a => a.Sort).ToList())
                                                {
                                                    SampleConditionCheckPoint point = ObjectSpace.CreateObject<SampleConditionCheckPoint>();
                                                    point.CheckPoint = data;
                                                    if (data.PickAnswer == PickAnswer.Yes)
                                                    {
                                                        point.Yes = true;
                                                    }
                                                    else if (data.PickAnswer == PickAnswer.No)
                                                    {
                                                        point.No = true;
                                                    }
                                                    else if (data.PickAnswer == PickAnswer.None)
                                                    {
                                                        point.NA = true;
                                                    }
                                                    obj.SampleConditionCheckPoint.Add(point);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (Application.MainWindow != null && Application.MainWindow.View != null && Application.MainWindow.View.Id == "SampleBottleAllocation_DetailView_SampleTransfer")
                                {
                                    Samplecheckin objSamplecheckin = ((SampleBottleAllocation)Application.MainWindow.View.CurrentObject).SampleRegistration.JobID;
                                    if (objSamplecheckin != null && objSamplecheckin.SampleMatries != null)
                                    {
                                        List<Guid> lstSMOid = new List<Guid>();
                                        HttpContext.Current.Session["SMOid"] = objSamplecheckin.SampleMatries;
                                        if (HttpContext.Current.Session["SMOid"] != null)
                                        {
                                            string[] SampleMatrix = HttpContext.Current.Session["SMOid"].ToString().Split(new string[] { "; " }, StringSplitOptions.None);
                                            foreach (string val in SampleMatrix)
                                            {
                                                VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(val.Trim()));
                                                if (objVM != null)
                                                {
                                                    IList<SampleConditionCheckData> datas = ObjectSpace.GetObjects<SampleConditionCheckData>(CriteriaOperator.Parse("Contains([SampleMatrices], ?)", objVM.VisualMatrixName/*.Replace(" ", "")*/));
                                                    foreach (SampleConditionCheckData obja in datas)
                                                    {
                                                        if (!lstSMOid.Contains(obja.Oid))
                                                            lstSMOid.Add(obja.Oid);
                                                    }
                                                }
                                            }
                                        }
                                        if (obj != null)
                                        {
                                            IList<SampleConditionCheckData> datas = ObjectSpace.GetObjects<SampleConditionCheckData>(new InOperator("Oid", lstSMOid));
                                            foreach (SampleConditionCheckData data in datas.OrderBy(a => a.Sort).ToList())
                                            {
                                                SampleConditionCheckPoint point = ObjectSpace.CreateObject<SampleConditionCheckPoint>();
                                                point.CheckPoint = data;
                                                if (data.PickAnswer == PickAnswer.Yes)
                                                {
                                                    point.Yes = true;
                                                }
                                                else if (data.PickAnswer == PickAnswer.No)
                                                {
                                                    point.No = true;
                                                }
                                                else if (data.PickAnswer == PickAnswer.None)
                                                {
                                                    point.NA = true;
                                                }
                                                obj.SampleConditionCheckPoint.Add(point);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (View.Id == "SampleConditionCheckPoint_ListView_Task")
                {
                    //if (((ListView)View).CollectionSource.GetCount() == 0)
                    //{
                    //    NestedFrame nestedFrame = (NestedFrame)Frame;
                    //    if (nestedFrame != null)
                    //    {
                    //        CompositeView view = nestedFrame.ViewItem.View;
                    //        if (view != null)
                    //        {
                    //            SampleConditionCheck obj = (SampleConditionCheck)view.CurrentObject;
                    //            Tasks objTask = (Tasks)Application.MainWindow.View.CurrentObject;
                    //            if (objTask != null && objTask.SampleMatrix != null)
                    //            {
                    //                List<Guid> lstSMOid = new List<Guid>();
                    //                HttpContext.Current.Session["SMOid"] = objTask.SampleMatrix;
                    //                if (HttpContext.Current.Session["SMOid"] != null)
                    //                {
                    //                    string[] SampleMatrix = HttpContext.Current.Session["SMOid"].ToString().Split(new string[] { "; " }, StringSplitOptions.None);
                    //                    foreach (string val in SampleMatrix)
                    //                    {
                    //                        VisualMatrix objVM = ObjectSpace.GetObjectByKey<VisualMatrix>(new Guid(val.Trim()));
                    //                        if (objVM != null)
                    //                        {
                    //                            IList<SampleConditionCheckData> datas = ObjectSpace.GetObjects<SampleConditionCheckData>(CriteriaOperator.Parse("Contains([SampleMatrices], ?)", objVM.VisualMatrixName/*.Replace(" ", "")*/));
                    //                            foreach (SampleConditionCheckData obja in datas)
                    //                            {
                    //                                if (!lstSMOid.Contains(obja.Oid))
                    //                                    lstSMOid.Add(obja.Oid);
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                if (obj != null)
                    //                {
                    //                    IList<SampleConditionCheckData> datas = ObjectSpace.GetObjects<SampleConditionCheckData>(new InOperator("Oid", lstSMOid));
                    //                    foreach (SampleConditionCheckData data in datas.OrderBy(a => a.Sort).ToList())
                    //                    {
                    //                        SampleConditionCheckPoint point = ObjectSpace.CreateObject<SampleConditionCheckPoint>();
                    //                        point.CheckPoint = data;
                    //                        if (data.PickAnswer == PickAnswer.Yes)
                    //                        {
                    //                            point.Yes = true;
                    //                        }
                    //                        else if (data.PickAnswer == PickAnswer.No)
                    //                        {
                    //                            point.No = true;
                    //                        }
                    //                        else if (data.PickAnswer == PickAnswer.None)
                    //                        {
                    //                            point.NA = true;
                    //                        }
                    //                        obj.SampleConditionCheckPoint.Add(point);
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            try
            {
                if (View is ListView)
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s, e) {                    
                            var valYes = s.batchEditApi.GetCellValue(e.visibleIndex, 'Yes');
                            var valNo = s.batchEditApi.GetCellValue(e.visibleIndex, 'No');
                            var valNA = s.batchEditApi.GetCellValue(e.visibleIndex, 'NA');     
                            sessionStorage.setItem('Yes', valYes);                      
                            sessionStorage.setItem('No', valNo);
                            sessionStorage.setItem('NA', valNA);               
                            }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() {                     
                            var valYes = s.batchEditApi.GetCellValue(e.visibleIndex, 'Yes');
                            var valNo = s.batchEditApi.GetCellValue(e.visibleIndex, 'No');
                            var valNA = s.batchEditApi.GetCellValue(e.visibleIndex, 'NA');
                            var oldYes = sessionStorage.getItem('Yes');
                            var oldNo = sessionStorage.getItem('No');
                            var oldNA = sessionStorage.getItem('NA'); 
                            if(valYes.toString() != oldYes.toString() && valYes == true)
                            {
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'No', false);  
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'NA', false);  
                            }
                            else if(valNo.toString() != oldNo.toString() && valNo == true)
                            {
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'Yes', false);  
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'NA', false);  
                            }
                            else if(valNA.toString() != oldNA.toString() && valNA == true)
                            {
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'Yes', false);  
                               s.batchEditApi.SetCellValue(e.visibleIndex, 'No', false);  
                            }
                            sessionStorage.removeItem('Yes', valYes);                      
                            sessionStorage.removeItem('No', valNo);
                            sessionStorage.removeItem('NA', valNA);    
                            }, 10); }";
                    gridListEditor.Grid.Settings.ShowHeaderFilterButton = false;
                }
                else if (View.Id == "SampleConditionCheck_DetailView_Copy")
                {
                    foreach (ViewItem item in ((DetailView)View).Items.Where(a => a.Id == "Temperature"))
                    {
                        ASPxStringPropertyEditor propertyEditor = item as ASPxStringPropertyEditor;
                        if (propertyEditor != null && propertyEditor.Editor != null)
                        {
                            ASPxTextBox textBox = (ASPxTextBox)propertyEditor.Editor;
                            if (textBox != null)
                            {
                                textBox.ClientSideEvents.Init = @"function(s,e){ s.ForceRefocusEditor(); }";
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
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            View.ControlsCreated -= View_ControlsCreated;
            //try
            //{
            //    if (View.Id == "SampleConditionCheck_DetailView_Copy")
            //    {
            //        DialogController dialogController = Frame.GetController<DialogController>();
            //        //dialogController.AcceptAction.Executing -= AcceptAction_Executing;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
            //    Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            //}
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
        private void SCC_Report_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "Samplecheckin_SampleConditionCheck_ListView" || View.Id == "Tasks_SampleConditionCheck_ListView")
                {
                    View.ObjectSpace.CommitChanges();
                    if (View.SelectedObjects.Count > 0)
                    {
                        string strSCCOid = string.Empty;
                        List<string> listSCCOid = new List<string>();
                        foreach (SampleConditionCheck obj in View.SelectedObjects)
                        {
                            listSCCOid.Add(obj.Oid.ToString());
                            if (string.IsNullOrEmpty(strSCCOid))
                            {
                                strSCCOid = "'" + obj.Oid + "'";
                            }
                            else
                            {
                                strSCCOid = strSCCOid + ",'" + obj.Oid + "'";
                            }
                        }
                        if (listSCCOid != null && listSCCOid.Count > 0)
                        {
                            string strTempPath = Path.GetTempPath();
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SCC_Report\")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SCC_Report\"));
                            }
                            string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\SCC_Report\" + timeStamp + ".pdf");
                            XtraReport xtraReport = new XtraReport();

                            objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            SetConnectionString();

                            DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);

                            if (listSCCOid.Count > 1)
                            {
                                foreach (string strCOC_ID in listSCCOid)
                                {
                                    XtraReport tempxtraReport = new XtraReport();
                                    ObjReportingInfo.SCCOid = "'" + strCOC_ID + "'"; ;
                                    if (View.Id == "Samplecheckin_SampleConditionCheck_ListView")
                                    {
                                        tempxtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("SCC_CheckPoints_Report", ObjReportingInfo, false);
                                    }
                                    else
                                    {
                                        tempxtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("SCC_CheckPoints_Task_Report", ObjReportingInfo, false);
                                    }
                                    tempxtraReport.CreateDocument();
                                    xtraReport.Pages.AddRange(tempxtraReport.Pages);
                                }

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    xtraReport.ExportToPdf(ms);
                                    using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                                    {
                                        source.LoadDocument(ms);
                                        source.SaveDocument(strExportedPath);
                                    }
                                }
                            }
                            else
                            {
                                ObjReportingInfo.SCCOid = strSCCOid;
                                if (View.Id == "Samplecheckin_SampleConditionCheck_ListView")
                                {
                                    xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("SCC_CheckPoints_Report", ObjReportingInfo, false);
                                }
                                else
                                {
                                    xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("SCC_CheckPoints_Task_Report", ObjReportingInfo, false);
                                }
                            }

                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 3;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        //string strJobID = string.Empty;
                        //foreach (SampleConditionCheck obj in View.SelectedObjects)
                        //{
                        //    if (strJobID == string.Empty)
                        //    {
                        //        strJobID = "'" + obj.Oid + "'";
                        //    }
                        //    else
                        //    {
                        //        if (!strJobID.Contains(obj.Oid.ToString()))
                        //            strJobID = strJobID + ",'" + obj.Oid + "'";
                        //    }
                        //}
                        //string strTempPath = Path.GetTempPath();
                        //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SCC_Report")) == false)
                        //{
                        //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SCC_Report"));
                        //}
                        //string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\\SCC_Report" + timeStamp + ".pdf");
                        //XtraReport xtraReport = new XtraReport();

                        //objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        //SetConnectionString();

                        //DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                        ////DynamicDesigner.GlobalReportSourceCode.strJobID = strJobID;
                        //ObjReportingInfo.SCCOid = strJobID;
                        ////xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("FolderLabel_Report", ObjReportingInfo, false);
                        //xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("SCC_CheckPoints_Report", ObjReportingInfo, false);
                        //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport);
                        //xtraReport.ExportToPdf(strExportedPath);
                        //string[] path = strExportedPath.Split('\\');
                        //int arrcount = path.Count();
                        //int sc = arrcount - 2;
                        //string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                        //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath)); 
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
        private void SCC_PopupReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                SampleConditionCheck objSCC = (SampleConditionCheck)View.CurrentObject;
                if (objSCC != null)
                {
                    string strSCCOid = "'" + objSCC.Oid + "'";
                    string strTempPath = Path.GetTempPath();
                    String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SCC_Report\")) == false)
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview\SCC_Report\"));
                    }
                    string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\SCC_Report\" + timeStamp + ".pdf");
                    XtraReport xtraReport = new XtraReport();

                    objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    SetConnectionString();

                    DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                    ObjReportingInfo.SCCOid = "'" + objSCC.Oid + "'";
                    if (View.Id == "Samplecheckin_SampleConditionCheck_ListView" || View.Id == "SampleConditionCheck_DetailView_Copy")
                    {
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("SCC_CheckPoints_Report", ObjReportingInfo, false);
                    }
                    else
                    {
                        xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("SCC_CheckPoints_Task_Report", ObjReportingInfo, false);
                    }
                    xtraReport.ExportToPdf(strExportedPath);
                    string[] path = strExportedPath.Split('\\');
                    int arrcount = path.Count();
                    int sc = arrcount - 3;
                    string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                    WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));

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
