using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Pdf;
using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraReports.UI;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.AlpacaLims;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Report;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.SDA.AlpacaLims;
using Modules.BusinessObjects.Setting;
using Modules.BusinessObjects.TaskManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace LDM.Module.Web.Controllers.SamplingProposal
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SamplingAllocationViewController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        string strSampleID = string.Empty;
        string strJobID = string.Empty;
        bool SampleTest = true;
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        DynamicReportDesignerConnection objDRDCInfo = new DynamicReportDesignerConnection();
        bool Attachjobid = false;
        bool DeattachJobid = false;
        //private System.ComponentModel.IContainer components;
        string strConnectionString;
        public SamplingAllocationViewController()
        {
            InitializeComponent();
            TargetViewId = "TaskSchedulerEventList_ListView_Copy;" + "SampleLogIn_ListView_SamplingAllocation;" + "TaskJobIDAutomation_ListView;" + "SampleLogIn_ListView_SamplingAllocation;"
                + "Samplecheckin_ListView_SamplingAllocation;"+ "COCSample_DetailView;";
            //AttachJobID.TargetViewId = "TaskSchedulerEventList_ListView_Copy;";
            //DeAttachJobID.TargetViewId = "TaskSchedulerEventList_ListView_Copy;";
            COC_BarReport.TargetViewId = "Samplecheckin_ListView_SamplingAllocation;";
            SampleLabel.TargetViewId = "Samplecheckin_ListView_SamplingAllocation;";
            RTCTReport.TargetViewId = "Samplecheckin_ListView_SamplingAllocation;";
            Vertical.TargetViewId = "Samplecheckin_ListView_SamplingAllocation;";

           // AttachJobID.Execute += AttachJobID_Execute;
           // DeAttachJobID.Execute += DeAttachJobID_Execute;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.Id== "Samplecheckin_ListView_SamplingAllocation")
            {
                //DefaultSetting setting = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='SamplingAllocation'"));
                //tbl_Public_CustomReportDesignerDetails objCustomReport = ObjectSpace.FindObject<tbl_Public_CustomReportDesignerDetails>(CriteriaOperator.Parse("colCustomReportDesignerName = 'SamplingAllocation'"));
                LDM.Module.Web.Controllers.SamplingProposal.SamplingAllocationViewController objsample = Frame.GetController<LDM.Module.Web.Controllers.SamplingProposal.SamplingAllocationViewController>();
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("GetReportDetails_sp", connection);
                connection.Open();
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable sqlDt = new DataTable();
                sda.Fill(sqlDt);
                connection.Close();
                if(sqlDt!=null && sqlDt.Rows.Count > 0)
                {
                    foreach(DataRow dr in sqlDt.Rows)
                    {
                        if (dr["colCustomReportDesignerName"] != DBNull.Value && Convert.ToString(dr["colCustomReportDesignerName"]) == "Sample Report")
                        {
                            if (Convert.ToBoolean(dr["Active"]) == true && Convert.ToBoolean(dr["FinalReport"]) == true)
                            {
                                objsample.SampleLabel.Active.SetItemValue("FG", true);

                            }
                            else
                            {
                                objsample.SampleLabel.Active.SetItemValue("FG", false);
                            }
                        }

                        if (dr["colCustomReportDesignerName"] != DBNull.Value && Convert.ToString(dr["colCustomReportDesignerName"]) == "BarcodeSampleAllocationReport")
                        {
                            if (Convert.ToBoolean(dr["Active"]) == true && Convert.ToBoolean(dr["FinalReport"]) == true)
                            {
                                objsample.Vertical.Active.SetItemValue("FG", true);

                            }
                            else
                            {
                                objsample.Vertical.Active.SetItemValue("FG", false);
                            }
                        }



                        if (dr["colCustomReportDesignerName"]!=DBNull.Value && Convert.ToString(dr["colCustomReportDesignerName"]) == "COC_Report")
                        {
                            if (Convert.ToBoolean(dr["Active"]) == true && Convert.ToBoolean(dr["FinalReport"]) == true)
                            {
                                objsample.COC_BarReport.Active.SetItemValue("FG", true);

                            }
                            else
                            {
                                objsample.COC_BarReport.Active.SetItemValue("FG", false);
                            }
                        }
                        if (dr["colCustomReportDesignerName"] != DBNull.Value && Convert.ToString(dr["colCustomReportDesignerName"]) == "RTCRMicrobialReport")
                        {
                            if (Convert.ToBoolean(dr["Active"]) == true && Convert.ToBoolean(dr["FinalReport"]) == true)
                            {
                                objsample.RTCTReport.Active.SetItemValue("FG", true);

                            }
                            else
                            {
                                objsample.RTCTReport.Active.SetItemValue("FG", false);
                            }
                        }
                        
                        
                    }
                }

              
            }
            strConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            if (View.Id == "SampleLogIn_ListView_SamplingAllocation")
                {

                
                ObjectSpace.Committed += ObjectSpace_Committed;

                DialogController dialogController = Frame.GetController<DialogController>();
                if(dialogController!= null)
                    {
                    dialogController.AcceptAction.Active.SetItemValue("disable", false);
                    dialogController.CancelAction.Active.SetItemValue("disable", false);

                    }
                }
            else if(View.Id== "Samplecheckin_ListView_SamplingAllocation")
            {
                ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                tar.CustomProcessSelectedItem += Tar_CustomProcessSelectedItem;
            }

            ////if(View.Id == "SampleLogIn_ListView_SamplingAllocation")
            ////{
            ////    DashboardViewItem lsampllogin = ((DetailView)View).FindItem("Assign") as DashboardViewItem;
            ////                    DashboardViewItem Note = ((DetailView)View).FindItem("Note") as DashboardViewItem;

            ////    if (HttpContext.Current.Session["rowid"] != null)
            ////    {
            ////        if (lsampllogin!=null && lsampllogin.InnerView==null)
            ////        {
            ////            lsampllogin.CreateControl(); 
            ////        }
            ////        if (lsampllogin != null && lsampllogin.InnerView != null)
            ////        {
            ////            ((ListView)lsampllogin.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString()));

            ////        }
            ////    }
            ////    else
            ////    {
            ////        ((ListView)lsampllogin.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] is null");
            ////    }

            ////    if (HttpContext.Current.Session["rowid"] != null)
            ////    {

            ////        if (Note != null && Note.InnerView == null)
            ////        {
            ////            Note.CreateControl();
            ////        }
            ////        if (Note != null && Note.InnerView != null)
            ////        {

            ////            if (lsampllogin != null && lsampllogin.InnerView != null)
            ////            {
            ////                ((ListView)Note.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString()));

            ////            }
            ////        }
            ////    }
            ////    else
            ////    {
            ////        ((ListView)lsampllogin.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] is null");
            ////    }

            if (View.Id == "SampleLogIn_ListView_SamplingAllocation")
            {
                Employee employee = View.ObjectSpace.GetObjectByKey<Modules.BusinessObjects.Hr.Employee>(SecuritySystem.CurrentUserId);
                foreach (SampleLogIn objSampleLogin in ((ListView)View).CollectionSource.List)
                {

                    objSampleLogin.DateAssigned = DateTime.Now;
                    objSampleLogin.AssignedBy = employee;
                    IList<SampleParameter> parameters = View.ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid]=?", objSampleLogin.Oid)).ToList();
                    foreach (SampleParameter sp in parameters)
                    {
                        if (sp != null)
                        {
                            if (sp.Testparameter != null && objSampleLogin.Oid == sp.Samplelogin.Oid)
                            {

                                //var labwareNames = parameters.Where(i => i.Testparameter != null && i.Samplelogin.Oid == objSampleLogin.Oid).Select(l => l.Testparameter).Where(tp => tp.TestMethod != null && tp.TestMethod.Labwares.Any()).SelectMany(tp => tp.TestMethod.Labwares).Where(l => !string.IsNullOrEmpty(l.LabwareName)).Select(l => l.LabwareName).Distinct().ToList();
                                var labwareNames = parameters.Where(i => i.Testparameter != null && i.Samplelogin.Oid == objSampleLogin.Oid).Select(l => l.Testparameter).Where(tp => tp.TestMethod != null && tp.TestMethod.Labwares != null && tp.TestMethod.Labwares.Any()).SelectMany(tp => tp.TestMethod.Labwares).Where(l => !string.IsNullOrEmpty(l.LabwareName)).Select(l => l.LabwareName).Distinct().ToList();

                                objSampleLogin.SamplingEquipment = string.Join(",", labwareNames); 
                            }
                        }
                    }

                    if (ObjectSpace is XPObjectSpace objectSpace)
                    {
                        objectSpace.CommitChanges();
                    }

                    

                }
            }


                ////if(View.Id == "SampleLogIn_ListView_SamplingAllocation")
                ////{
                ////    DashboardViewItem lsampllogin = ((DetailView)View).FindItem("Assign") as DashboardViewItem;
                ////                    DashboardViewItem Note = ((DetailView)View).FindItem("Note") as DashboardViewItem;

                ////    if (HttpContext.Current.Session["rowid"] != null)
                ////    {
                ////        if (lsampllogin!=null && lsampllogin.InnerView==null)
                ////        {
                ////            lsampllogin.CreateControl(); 
                ////        }
                ////        if (lsampllogin != null && lsampllogin.InnerView != null)
                ////        {
                ////            ((ListView)lsampllogin.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString()));

                ////        }
                ////    }
                ////    else
                ////    {
                ////        ((ListView)lsampllogin.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] is null");
                ////    }

                ////    if (HttpContext.Current.Session["rowid"] != null)
                ////    {

                ////        if (Note != null && Note.InnerView == null)
                ////        {
                ////            Note.CreateControl();
                ////        }
                ////        if (Note != null && Note.InnerView != null)
                ////        {

                ////            if (lsampllogin != null && lsampllogin.InnerView != null)
                ////            {
                ////                ((ListView)Note.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SamplingProposal.Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString()));

                ////            }
                ////        }
                ////    }
                ////    else
                ////    {
                ////        ((ListView)lsampllogin.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] is null");
                ////    }


                ////}
                if (View.Id == "TaskSchedulerEventList_ListView_Copy")
                    {
                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[JobID] IS NOT NULL");
            }
            ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                    }

        private void ObjectSpace_Committed(object sender, EventArgs e)
                    {
            try
                        {
                //if(View != null && View.Id == "SampleLogIn_ListView_SamplingAllocation" && ObjectSpace.IsModified)
                //{
                //    Application.ShowViewStrategy.ShowMessage("Updated Successfully", InformationType.Success, 3000, InformationPosition.Top);
                //}


            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Tar_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            try
            {
               if(View.Id== "Samplecheckin_ListView_SamplingAllocation")
                {
                    Samplecheckin SampleCheckIn = (Samplecheckin)e.InnerArgs.CurrentObject;
                    if(SampleCheckIn!=null)
                    {
                        IObjectSpace objspace = Application.CreateObjectSpace();
                        CollectionSource cs = new CollectionSource(objspace, typeof(SampleLogIn));
                        cs.Criteria["sampleallocation"] = CriteriaOperator.Parse("[JobID] =?", SampleCheckIn.Oid);
                        ListView creadetView = Application.CreateListView("SampleLogIn_ListView_SamplingAllocation", cs, true);
                        ShowViewParameters showViewParameters = new ShowViewParameters();
                        showViewParameters.CreatedView = creadetView;
                        showViewParameters.Context = TemplateContext.PopupWindow;
                        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                        DialogController dc = Application.CreateController<DialogController>();
                        dc.SaveOnAccept = false;
                        dc.CloseOnCurrentObjectProcessing = false;
                        //    dc.Accepting += Dc_Accepting;
                        showViewParameters.Controllers.Add(dc);
                        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        e.Handled = true;
                    }
                    //if (currentOid != null)
                    //{
                    //    TaskSchedulerEventList samplingAllocation = objspace.GetObjectByKey<TaskSchedulerEventList>(currentOid);
                    //    if (samplingAllocation != null && samplingAllocation.JobID != null)
                    //    {
                    //        HttpContext.Current.Session["rowid"] = samplingAllocation.TaskSchedulerID.COCSettings.Oid;
                    //        CollectionSource cs = new CollectionSource(objspace, typeof(SampleLogIn));
                    //        cs.Criteria["sampleallocation"] = CriteriaOperator.Parse("[JobID] =?", samplingAllocation.JobID.Oid);
                    //        ListView creadetView = Application.CreateListView("SampleLogIn_ListView_SamplingAllocation", cs, true);
                    //        //ListView creadetView = Application.CreateListView(objspace, "SampleLogIn_ListView_SamplingAllocation", true);
                    //        ShowViewParameters showViewParameters = new ShowViewParameters();
                    //        showViewParameters.CreatedView = creadetView;
                    //        showViewParameters.Context = TemplateContext.PopupWindow;
                    //        showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    //        DialogController dc = Application.CreateController<DialogController>();
                    //        dc.SaveOnAccept = false;
                    //        dc.CloseOnCurrentObjectProcessing = false;
                    //        //    dc.Accepting += Dc_Accepting;
                    //        showViewParameters.Controllers.Add(dc);
                    //        Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                    //        IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objsl = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.Oid] =?", samplingAllocation.JobID.JobID));
                    //    }
                    //}
                    //else
                    //{
                    //    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    //}
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {

            try
            {
                e.PopupControl.CustomizePopupWindowSize += PopupControl_CustomizePopupWindowSize;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void PopupControl_CustomizePopupWindowSize(object sender, CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "SampleLogIn_ListView_SamplingAllocation")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(1200);
                    e.Height = new System.Web.UI.WebControls.Unit(600);
                    e.Handled = true;
                }
                if(e.PopupFrame.View.Id== "COCSample_DetailView")
                {
                    e.Width = new System.Web.UI.WebControls.Unit(300);
                    e.Height = new System.Web.UI.WebControls.Unit(250);
                    e.Handled = true;
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
            if (View.Id == "SampleLogIn_ListView_SamplingAllocation")
            {
                ASPxGridListEditor aSPxGridList = ((ListView)View).Editor as ASPxGridListEditor;
                aSPxGridList.Grid.BatchUpdate += Grid_BatchUpdate;

                if (aSPxGridList != null)
                {
                    ASPxGridView aSPxGrid = aSPxGridList.Grid;
                    if(aSPxGrid!= null)
                    {
                        Modules.BusinessObjects.Hr.Employee user = (Modules.BusinessObjects.Hr.Employee)SecuritySystem.CurrentUser;
                        aSPxGrid.JSProperties["cpuserid"] = SecuritySystem.CurrentUserId;
                        aSPxGrid.JSProperties["cpusername"] = user.DisplayName;
                        aSPxGrid.ClientSideEvents.FocusedCellChanging = @"function(s, e) 
                        {
                                if(sessionStorage.getItem('CurrFocusedColumn') == null)
                                {
                                    sessionStorage.setItem('PrevFocusedColumn', e.cellInfo.column.fieldName);
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }
                                else
                                {
                                    var precolumn = sessionStorage.getItem('CurrFocusedColumn');
                                    sessionStorage.setItem('PrevFocusedColumn', precolumn);                           
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }                                 
                        }";
                        aSPxGrid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            window.setTimeout(function() { 
                            var fieldName = sessionStorage.getItem('PrevFocusedColumn');
                            if( s.batchEditApi.HasChanges(e.visibleIndex) && fieldName == 'AssignToSampleAllocation.Oid')
                            {
                               var today = new Date();  
                              var AssingToobj = s.batchEditApi.GetCellValue(e.visibleIndex, 'AssignToSampleAllocation.Oid');
                              if(AssingToobj!=null)
                                {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DateAssigned', today);          
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'AssignedBy', s.cpuserid, s.cpusername, false);  
                                }
                                else
                                {
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'DateAssigned', null);
                                    s.batchEditApi.SetCellValue(e.visibleIndex, 'AssignedBy', null);
                                }
                            }

                            }, 20); }";

                    }
                }
            }

            //if (View.Id == "TaskSchedulerEventList_ListView_Copy")
            //{
            //    if (View.CurrentObject!=null)
            //    {
            //        TaskSchedulerEventList samplingAllocation = View.CurrentObject as TaskSchedulerEventList;
            //        if (samplingAllocation != null && samplingAllocation.JobID != null)
            //        {
            //            IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objsl = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.JobID] =?", samplingAllocation.JobID.JobID));

            //            //CollectionSource cs = new CollectionSource(View.ObjectSpace,typeof (Sampling));
            //            if (objsl != null && objsl != null && View.Id== "Sampling_ListView_AssignTo")
            //            {
            //                ((ListView)View).CollectionSource.Criteria["Sampling"] = CriteriaOperator.Parse("[SamplingProposal] = ?", samplingAllocation.TaskSchedulerID.RegistrationID);

            //                //cs.Criteria["Filter"] = CriteriaOperator.Parse("[SamplingProposal] = ?", samplingAllocation.TaskSchedulerID.RegistrationID);
            //            }
            //        }
            //    }
            //}
            else if (View.Id == "TaskJobIDAutomation_ListView")
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ASPxGridView gridView = gridListEditor.Grid;
                if (gridListEditor.Grid != null)
                {
                    gridListEditor.Grid.BatchUpdate += Grid_BatchUpdate;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                           {
                                if(sessionStorage.getItem('CurrFocusedColumn') == null)
                                {
                                    sessionStorage.setItem('PrevFocusedColumn', e.cellInfo.column.fieldName);
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }
                                else
                                {
                                    var precolumn = sessionStorage.getItem('CurrFocusedColumn');
                                    sessionStorage.setItem('PrevFocusedColumn', precolumn);                           
                                    sessionStorage.setItem('CurrFocusedColumn', e.cellInfo.column.fieldName);
                                }                                 
                           }";
                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s, e) { 
                    if (s.IsRowSelectedOnPage(e.elementIndex)) 
                    { 
                         var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');  
                         if (FocusedColumn=='DaysinAdvance') 
                         {                                                             
                            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);
                            if (e.item.name == 'CopyToAllCell')
                             {
                                  for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                   { 
                                     if (s.IsRowSelectedOnPage(i)) 
                                     {
                                        s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                                     }
                                   }
                              }                            
                }

            }
                     e.processOnServer = false;
                     }";
                }

            }
            else if (View.Id == "TaskSchedulerEventList_ListView_Copy")
            {
                XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                callbackManager.RegisterHandler("SampleAllocationHandler", this);

                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                ASPxGridView gridView = gridListEditor.Grid;
                if (gridView != null)
                {
                    //gridView.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared;

                    // Access and customize the target View control.
                }
            }
        }

        //private void Grid_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        //{
        //    TaskJobIDAutomation taskJobIDAutomation = e.Object as TaskJobIDAutomation;
        //    Session currentSessions = ((XPObjectSpace)(this.ObjectSpace)).Session;
        //    XPMemberInfo optimisticLock = currentSessions.GetClassInfo(taskJobIDAutomation).OptimisticLockField;
        //    object cValue = optimisticLock.GetValue(taskJobIDAutomation);
        //    if (taskJobIDAutomation.COCID != null)
        //    {
        //        if (!string.IsNullOrEmpty(e.PropertyName) && taskJobIDAutomation != null && cValue != null && (int)cValue > 0)
        //        {
        //            objAuditInfo.SaveData = true;
        //            Frame.GetController<AuditlogViewController>().insertauditdata(ObjectSpace, taskJobIDAutomation.Oid, OperationType.ValueChanged, "Report Tracking", taskJobIDAutomation.COCID.ToString(), "ReportName", objReporting1.ReportName, objReporting.ReportName, objReporting.RevisionReason);
        //            AuditData Objects = objSpace.FindObject<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
        //            if (Objects != null)
        //            {
        //                Objects.CommentProcessed = true;
        //            }
        //        }
        //    }

        //}
        private void GridView_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse("Oid is null"));
                    if (currentLanguage != null && currentLanguage.Chinese)
                    {
                        e.Items.Add("复制到所有单元格", "CopyToAllCell");
                    }
                    else
                    {
                        e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    }
                    GridViewContextMenuItem Edititem = e.Items.FindByName("EditRow");
                    if (Edititem != null)
                        Edititem.Visible = false;
                    GridViewContextMenuItem item = e.Items.FindByName("CopyToAllCell");
                    if (item != null)
                        item.Image.IconID = "edit_copy_16x16office2013";//"navigation_home_16x16";
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
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
            if (View.Id == "SampleLogIn_ListView_SamplingAllocation")
            {
                ObjectSpace.Committed -= ObjectSpace_Committed;
                DialogController dialogController = Frame.GetController<DialogController>();
                if (dialogController != null)
                {
                    dialogController.AcceptAction.Active.SetItemValue("disable", true);
                    dialogController.CancelAction.Active.SetItemValue("disable", true);

                }
            }
            else if (View.Id == "Samplecheckin_ListView_SamplingAllocation")
            {
                ListViewProcessCurrentObjectController tar = Frame.GetController<ListViewProcessCurrentObjectController>();
                tar.CustomProcessSelectedItem -= Tar_CustomProcessSelectedItem;
            }
            if (View.Id == "Sampling_DetailView_Copy")
            {
                HttpContext.Current.Session["rowid"] =null;
            }
        }
        private void Grid_BatchUpdate(object sender, ASPxDataBatchUpdateEventArgs e)
        {

            if (View != null && View.Id == "SampleLogIn_ListView_SamplingAllocation")
            {
                Application.ShowViewStrategy.ShowMessage("Updated Successfully", InformationType.Success, 3000, InformationPosition.Top);
            }
            if (View.Id== "TaskJobIDAutomation_ListView")
            {
                IList<AuditData> objAudit = View.ObjectSpace.GetObjects<AuditData>(CriteriaOperator.Parse("[CommentProcessed] = False And [CreatedBy.Oid] = ?", SecuritySystem.CurrentUserId), true);
                if (objAudit.Count > 0)
                {
                    Frame.GetController<AuditlogViewController>().getcomments(View.ObjectSpace, objAudit.First());

                } 
            }
        }


        private void GridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "TaskSchedulerEventList_ListView_Copy")
                {
                    if (e.DataColumn.ToString() == "JobID")
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'SampleAllocationHandler', 'JobID|'+{0}, '', false)", e.VisibleIndex));
                    }

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
                string[] values = parameter.Split('|');

                if (View.Id == "TaskSchedulerEventList_ListView_Copy" && values[0] == "JobID")
                    {
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    object currentOid = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid");
                    HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(values[1]), "Oid"); IObjectSpace objspace = Application.CreateObjectSpace();
                    SampleLogIn sampleLog = View.CurrentObject as SampleLogIn;

                    if (currentOid!=null)
                    {
                        TaskSchedulerEventList samplingAllocation = objspace.GetObjectByKey<TaskSchedulerEventList>(currentOid);
                        if (samplingAllocation != null && samplingAllocation.JobID != null)
                        {
                            HttpContext.Current.Session["rowid"] = samplingAllocation.TaskSchedulerID.COCSettings.Oid;
                            CollectionSource cs = new CollectionSource(objspace, typeof(SampleLogIn));
                            cs.Criteria["sampleallocation"] = CriteriaOperator.Parse("[JobID] =?", samplingAllocation.JobID.Oid);
                            ListView creadetView = Application.CreateListView("SampleLogIn_ListView_SamplingAllocation", cs,true);
                            //ListView creadetView = Application.CreateListView(objspace, "SampleLogIn_ListView_SamplingAllocation", true);
                            ShowViewParameters showViewParameters = new ShowViewParameters();
                            showViewParameters.CreatedView = creadetView;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;                            
                        //    dc.Accepting += Dc_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> objsl = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID.Oid] =?", samplingAllocation.JobID.JobID));
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        //private void insertdatatoflutter(Modules.BusinessObjects.SamplingManagement.SamplingProposal objSamplingProposal, SampleLogIn objSampleLogin, Sampling objSampling)
        //{
        //    FlutterSDARRA_FieldDataEntryInfo objflu = ObjectSpace.CreateObject<FlutterSDARRA_FieldDataEntryInfo>();
        //    objflu.uqRegistrationID = objSamplingProposal.Oid;
        //    objflu.RegistrationID = objSamplingProposal.RegistrationID;
        //    if (objSampling.StationLocation != null)
        //    {
        //        objflu.uqSamplingStationID = objSampling.StationLocation.Oid;
        //        objflu.Station = objSampling.StationLocation.SiteName;
        //        objflu.StationID = objSampling.StationLocation.SiteID;
        //        insertalternatestation(objSampling.StationLocation, objSampleLogin);
        //    }
        //    if (objSampleLogin.JobID.ProjectID != null)
        //    {
        //        objflu.ProjectID = objSampleLogin.JobID.ProjectID.ProjectId;
        //        objflu.ProjectName = objSampleLogin.JobID.ProjectID.ProjectName;
        //    }
        //    objflu.uqSamplingSampleID = objSampling.Oid;
        //    objflu.uqSampleID = objSampleLogin.Oid;
        //    objflu.JobID = objSampleLogin.JobID.JobID;
        //    objflu.SampleID = objSampleLogin.SampleID;
        //    objflu.ElementName = "DW";
        //    objflu.Blended = objSampleLogin.Blended;
        //    objflu.Latitude = objSampleLogin.Latitude;
        //    objflu.Longitude = objSampleLogin.Longitude;
        //    objflu.PlannedLatitude = objSampleLogin.PlannedLatitude;
        //    objflu.PlannedLongitude = objSampleLogin.PlannedLongitude;
        //    objflu.SampleName = objSampleLogin.SampleName;
        //    objflu.CollectedBy = null;
        //    if (objSampleLogin.JobID.ClientName != null)
        //    {
        //        objflu.ClientName = objSampleLogin.JobID.ClientName.DisplayName;
        //    }
        //    objflu.Temprature_C_ = objSampleLogin.Temp;
        //    objflu.Humidity___ = objSampleLogin.Humidity;
        //    objflu.ClientSampleID = objSampleLogin.ClientSampleID;
        //    //objflu.SampleBottleID = objSampleLogin.
        //    objflu.TestSummary = objSampleLogin.TestSummary;
        //    objflu.ModifiedDate = DateTime.Now;

        //    foreach (string guid in objSampling.AlternativeStationOid.Split(new[] { "; " }, StringSplitOptions.None))
        //    {
        //        SampleSites sites = ObjectSpace.FindObject<SampleSites>(CriteriaOperator.Parse("[Oid]=?", new Guid(guid)));
        //        if (sites != null)
        //        {
        //            insertalternatestation(sites, objSampleLogin);
        //        }
        //    }
        //}

        private void insertalternatestation(SampleSites sites, SampleLogIn objSampleLogin)
        {
            FlutterSDARRA_SamplingAlternateStation site = ObjectSpace.FindObject<FlutterSDARRA_SamplingAlternateStation>(CriteriaOperator.Parse("[uqSamplingStationID]=? and [JobID]=?", sites.Oid, objSampleLogin.JobID.JobID));
            if (site == null)
            {
                FlutterSDARRA_SamplingAlternateStation objsta = ObjectSpace.CreateObject<FlutterSDARRA_SamplingAlternateStation>();
                //objsta.uqSamplingStationID = sites.Oid;
                objsta.StationName = sites.SiteName;
                objsta.StationID = sites.SiteID;
                objsta.JobID = objSampleLogin.JobID.JobID;
            }
        }

        //private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        //{
        //    if (View != null)
        //    {
        //        IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(SampleLogIn));
        //        objectSpace.CommitChanges();
        //    }
        //}

        private void AttachJobID_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.SelectedObjects.Count > 0)
                {
                    int intJobIDAttached = View.SelectedObjects.Cast<TaskSchedulerEventList>().Count(obj => obj.JobID != null);
                    foreach (TaskSchedulerEventList item in View.SelectedObjects)
                    {
                        if (item.JobID == null)
                    {
                            Attachjobid = true;
                            TaskSchedulerEventList objtasks = View.CurrentObject as TaskSchedulerEventList;
                        ICallbackManagerHolder handlerid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                        handlerid.CallbackManager.RegisterHandler("SampleAllocationHandler", this);
                            string msg = "Do you want to attach the RegistrationId to a new JobId?";
                        WebWindow.CurrentRequestWindow.RegisterClientScript("Samplereset", string.Format(CultureInfo.InvariantCulture, @"var SampleReset = confirm('" + msg + "'); {0}", handlerid.CallbackManager.GetScript("SampleAllocationHandler", "SampleReset")));
                    }
                else
                {
                            ASPxGridListEditor aspxlist = ((ListView)View).Editor as ASPxGridListEditor;
                            if (aspxlist != null)
                            {
                                ASPxGridView gridview = (ASPxGridView)aspxlist.Grid;
                                if (gridview != null)
                                {
                                    //aspxlist.Grid.SettingsBehavior.AllowSelectByRowClick = unchecked;
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "RegistrationIDAttached"), InformationType.Error, timer.Seconds, InformationPosition.Top);

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

        //private void DeAttachJobID_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    try
        //    {
        //        if (View.SelectedObjects.Count > 0)
        //        {
        //            foreach (TaskSchedulerEventList item in View.SelectedObjects)
        //            {
        //                if (item.Status != null)
        //                {
        //            int pendingSamplingCount = View.SelectedObjects.Cast<TaskSchedulerEventList>().Count(obj => obj.Status == RegistrationStatus.PendingSampling);
        //            if (pendingSamplingCount == View.SelectedObjects.Count)
        //            {
        //                DeattachJobid = true;

        //                ICallbackManagerHolder handlerid = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
        //                handlerid.CallbackManager.RegisterHandler("SampleAllocationHandler", this);
        //                string msg = "The sample tests and parameters are cleared..! Do you want to continue?";
        //                WebWindow.CurrentRequestWindow.RegisterClientScript("Samplereset", string.Format(CultureInfo.InvariantCulture, @"var SampleReset = confirm('" + msg + "'); {0}", handlerid.CallbackManager.GetScript("SampleAllocationHandler", "SampleReset")));
        //                    }
                            
        //                }
        //                    else
        //                    {

        //                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
        //                    if (gridListEditor != null)
        //                            {
        //                        gridListEditor.UnselectAll();
        //                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "DetatchingJOBID"), InformationType.Error, timer.Seconds, InformationPosition.Top);

        //                            }
        //                        }
                      

        //            }
        //        }
        //        else
        //        {
        //            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Error, timer.Seconds, InformationPosition.Top);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }

        //}

        private DateTime AddWorkingDays(DateTime date, int daysToAdd)
        {
            try
            {
                while (daysToAdd > 0)
                {
                    date = date.AddDays(1);
                    IList<Holidays> lstHoliday = ObjectSpace.GetObjects<Holidays>(CriteriaOperator.Parse("Oid is Not Null"));
                    Holidays objHoliday = lstHoliday.FirstOrDefault(i => i.HolidayDate != DateTime.MinValue && i.HolidayDate.Day.Equals(date.Day));
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && objHoliday == null)
                    {
                        daysToAdd -= 1;
                    }
                }
                return date;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return DateTime.Now;
            }
        }
        private DateTime AddWorkingHours(DateTime date, int daysToAdd)
        {
            try
            {
                while (daysToAdd > 0)
                {
                    date = date.AddHours(1);
                    IList<Holidays> lstHoliday = ObjectSpace.GetObjects<Holidays>(CriteriaOperator.Parse("Oid is Not Null"));
                    Holidays objHoliday = lstHoliday.FirstOrDefault(i => i.HolidayDate != DateTime.MinValue && i.HolidayDate.Day.Equals(date.Day));
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && objHoliday == null)
                    {
                        daysToAdd -= 1;
                    }
                }
                return date;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
                return DateTime.Now;
            }
        }

        private void COC_BarReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {



            try
            {
                if (View != null && (View.Id == "Samplecheckin_ListView_SamplingAllocation"))
                {
                    ProjectDetailsInfo projectDetails = new ProjectDetailsInfo();
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProjectName"].ToString()))
                    {
                        projectDetails.ProjectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                    }
                    if (sender != null && !string.IsNullOrEmpty(projectDetails.ProjectName))
                    {
                        if (projectDetails.ProjectName.ToUpper().Trim() == "DWO")
                        {
                            if (View.SelectedObjects.Count > 0)
                            {
                                //if (View.Id == "CRMQuotes_DetailView")
                                //{
                                //    ObjectSpace.CommitChanges();
                                //}
                                string strCOCID = string.Empty;
                                List<string> strCOCIDlist = new List<string>();
                                foreach (Samplecheckin obj in View.SelectedObjects)
                                {
                                    if (obj.JobID != null)
                                    {
                                        strCOCIDlist.Add(obj.JobID);
                                        if (string.IsNullOrEmpty(strCOCID))
                                        {
                                            strCOCID = "'" + obj.JobID + "'";
                                        }
                                        else
                                        {
                                            strCOCID = strCOCID + ",'" + obj.JobID + "'";
                                        }
                                    }
                                    else
                                    {
                                        Application.ShowViewStrategy.ShowMessage("Add jobId for registrationid and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);

                                    }
                                }
                                string strTempPath = Path.GetTempPath();
                                String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                                if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\")) == false)
                                {
                                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\"));
                                }
                                string strExportedPath = HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\" + timeStamp + ".pdf");
                                XtraReport xtraReport = new XtraReport();

                                objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                SetConnectionString();

                                DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                                //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                                ObjReportingInfo.strJobID = strCOCID;
                                xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("TCEQMicrobialReport", ObjReportingInfo, false);
                                //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                                xtraReport.ExportToPdf(strExportedPath);
                                string[] path = strExportedPath.Split('\\');
                                int arrcount = path.Count();
                                int sc = arrcount - 3;
                                string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                                //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                                WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                            }
                        }

                    else
                    {

                    string strCOCID = string.Empty;
                    List<string> strCOCIDlist = new List<string>();
                    if (View.Id == "Samplecheckin_ListView_SamplingAllocation")
                    {
                        if (View.SelectedObjects != null)
                        {
                            foreach (Samplecheckin obj in View.SelectedObjects)
                            {
                                if (obj.JobID != null)
                                {
                                    strCOCIDlist.Add(obj.JobID);
                                    if (string.IsNullOrEmpty(strCOCID))
                                    {
                                        strCOCID = "'" + obj.JobID + "'";
                                    }
                                    else
                                    {
                                        strCOCID = strCOCID + ",'" + obj.JobID + "'";
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add jobId for registrationid and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);

                                }
                            }
                        }
                    }
                    else
                    {
                        if (View.CurrentObject != null)
                        {
                            Samplecheckin objSampleCheckin = (Samplecheckin)View.CurrentObject;
                            strCOCID = "'" + objSampleCheckin.JobID + "'";
                            strCOCIDlist.Add(objSampleCheckin.JobID);
                        }
                    }
                    if (View.SelectedObjects.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(strCOCID))
                        {
                            string strTempPath = Path.GetTempPath();
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                            }
                            string strExportedPath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                            XtraReport xtraReport = new XtraReport();
                            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                            string serverName = connectionStringBuilder.DataSource.Trim();
                            string databaseName = connectionStringBuilder.InitialCatalog.Trim();
                            string userID = connectionStringBuilder.UserID.Trim();
                            string password = connectionStringBuilder.Password.Trim();
                            if (strCOCIDlist.Count > 0)
                            {
                                foreach (string strCOC_ID in strCOCIDlist)
                                {
                                    XtraReport tempxtraReport = new XtraReport();

                                    SqlConnection connection = new SqlConnection(strConnectionString);
                                    SqlCommand command = new SqlCommand("COCReportGetData_Sp", connection);
                                    connection.Open();
                                    command.CommandType = CommandType.StoredProcedure;
                                    SqlParameter[] param = new SqlParameter[1];
                                    param[0] = new SqlParameter("@JobID", strCOC_ID);
                                    command.Parameters.AddRange(param);
                                    SqlDataAdapter sda = new SqlDataAdapter(command);
                                    DataTable sqlDt = new DataTable();
                                    sda.Fill(sqlDt);
                                    connection.Close();
                                    //string sqlSelect = "Select * from Report_COC_LegalDocument where [COCID] in(" + "'" + strCOC_ID + "'" + ")";
                                    //SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ToString());
                                    //SqlCommand sqlCommand = new SqlCommand(sqlSelect, sqlConnection);
                                    //SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCommand);
                                    //DataTable sqlDt = new DataTable();
                                    //sqlDa.Fill(sqlDt);
                                    String strProjectName = ConfigurationManager.AppSettings["ProjectName"];
                                    COC_Report COCReport = new COC_Report();
                                    sqlDt = new DataView(sqlDt, "", "COCSampleID", DataViewRowState.CurrentRows).ToTable();
                                    COCReport.DataSource = sqlDt;
                                    COCReport.DataBindingsToReport();
                                    tempxtraReport = COCReport;
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
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 2;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Add jobid for registrationid and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select at least one checkbox.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }

                }
                    }

                }

            }






            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>()
                    .InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Vertical_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null && (View.Id == "Samplecheckin_ListView_SamplingAllocation"))
                {
                    if (View.SelectedObjects.Count > 0)
                    {
                        SampleSourceSetup objSampleSource = ObjectSpace.FindObject<SampleSourceSetup>(CriteriaOperator.Parse(""));
                        if (objSampleSource != null && objSampleSource.NeedToActivateSampleSourceMode == SampleSourceMode.Yes)
                        {
                            //e.Cancel = true;
                            NonPersistentObjectSpace nos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(COCSample));
                            COCSample objToShow = nos.CreateObject<COCSample>();
                            objToShow.COCSamples = objSampleSource.COCSamples;
                            DetailView createDetailView = Application.CreateDetailView(nos, "COCSample_DetailView", false, objToShow);
                            createDetailView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(createDetailView);
                            showViewParameters.CreatedView = createDetailView;
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CancelAction.Active.SetItemValue("Cancel", false);
                            dc.Accepting += Dc_Accepting_SampleSource; ;
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Dc_Accepting_SampleSource(object sender, DialogControllerAcceptingEventArgs e)
        {
            //COCSample objCOCSamples = Application.View.CurrentObject as COCSample;
            COCSample objCOCSamples = (COCSample)e.AcceptActionArgs.CurrentObject;

            if (objCOCSamples.COCSamples == COCSamples.VerticalBarcode)
            {

                ProjectDetailsInfo projectDetails = new ProjectDetailsInfo();
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProjectName"].ToString()))
                {
                    projectDetails.ProjectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                }
                if (sender != null && !string.IsNullOrEmpty(projectDetails.ProjectName))
                {
                    if (projectDetails.ProjectName.ToUpper().Trim() == "DWO")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            //if (View.Id == "CRMQuotes_DetailView")
                            //{
                            //    ObjectSpace.CommitChanges();
                            //}
                            string strCOCID = string.Empty;
                            List<string> strCOCIDlist = new List<string>();
                            foreach (Samplecheckin obj in View.SelectedObjects)
                            {
                                if (obj.JobID != null)
                                {
                                    strCOCIDlist.Add(obj.JobID);
                                    if (string.IsNullOrEmpty(strCOCID))
                                    {
                                        strCOCID = "'" + obj.JobID + "'";
                                    }
                                    else
                                    {
                                        strCOCID = strCOCID + ",'" + obj.JobID + "'";
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add jobId for registrationid and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);

                                }
                            }
                            string strTempPath = Path.GetTempPath();
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\"));
                            }
                            string strExportedPath = HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\" + timeStamp + ".pdf");
                            XtraReport xtraReport = new XtraReport();

                            objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            SetConnectionString();

                            DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                            //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                            ObjReportingInfo.strJobID = strCOCID;
                            xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("SampleReceiving_VerticalBarcode", ObjReportingInfo, false);
                            //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 3;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }

                    }




                }
            }
            else if (objCOCSamples.COCSamples == COCSamples.MRFVerticalBarcode)
            {
                ProjectDetailsInfo projectDetails = new ProjectDetailsInfo();
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProjectName"].ToString()))
                {
                    projectDetails.ProjectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                }
                if (sender != null && !string.IsNullOrEmpty(projectDetails.ProjectName))
                {
                    if (projectDetails.ProjectName.ToUpper().Trim() == "DWO")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            //if (View.Id == "CRMQuotes_DetailView")
                            //{
                            //    ObjectSpace.CommitChanges();
                            //}
                            string strCOCID = string.Empty;
                            List<string> strCOCIDlist = new List<string>();
                            foreach (Samplecheckin obj in View.SelectedObjects)
                            {
                                if (obj.JobID != null)
                                {
                                    strCOCIDlist.Add(obj.JobID);
                                    if (string.IsNullOrEmpty(strCOCID))
                                    {
                                        strCOCID = "'" + obj.JobID + "'";
                                    }
                                    else
                                    {
                                        strCOCID = strCOCID + ",'" + obj.JobID + "'";
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add jobId for registrationid and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);

                                }
                            }
                            string strTempPath = Path.GetTempPath();
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\"));
                            }
                            string strExportedPath = HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\" + timeStamp + ".pdf");
                            XtraReport xtraReport = new XtraReport();

                            objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            SetConnectionString();

                            DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                            //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                            ObjReportingInfo.strJobID = strCOCID;
                            xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("MRF_VerticalBarcode", ObjReportingInfo, false);
                            //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 3;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }

                    }
                }




            }
            else if (objCOCSamples.COCSamples == COCSamples.LCRVerticalBarcode)
            {

                ProjectDetailsInfo projectDetails = new ProjectDetailsInfo();
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProjectName"].ToString()))
                {
                    projectDetails.ProjectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                }
                if (sender != null && !string.IsNullOrEmpty(projectDetails.ProjectName))
                {
                    if (projectDetails.ProjectName.ToUpper().Trim() == "DWO")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            //if (View.Id == "CRMQuotes_DetailView")
                            //{
                            //    ObjectSpace.CommitChanges();
                            //}
                            string strCOCID = string.Empty;
                            List<string> strCOCIDlist = new List<string>();
                            foreach (Samplecheckin obj in View.SelectedObjects)
                            {
                                if (obj.JobID != null)
                                {
                                    strCOCIDlist.Add(obj.JobID);
                                    if (string.IsNullOrEmpty(strCOCID))
                                    {
                                        strCOCID = "'" + obj.JobID + "'";
                                    }
                                    else
                                    {
                                        strCOCID = strCOCID + ",'" + obj.JobID + "'";
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add jobId for registrationid and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);

                                }
                            }
                            string strTempPath = Path.GetTempPath();
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\"));
                            }
                            string strExportedPath = HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\" + timeStamp + ".pdf");
                            XtraReport xtraReport = new XtraReport();

                            objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            SetConnectionString();

                            DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                            //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                            ObjReportingInfo.strJobID = strCOCID;
                            xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("LCR_VerticalBarcode", ObjReportingInfo, false);
                            //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 3;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }

                    }
                }




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

        private void RTCTReport_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View != null && (View.Id == "Samplecheckin_ListView_SamplingAllocation"))
            {
                ProjectDetailsInfo projectDetails = new ProjectDetailsInfo();
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProjectName"].ToString()))
                {
                    projectDetails.ProjectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                }
                if (sender != null && !string.IsNullOrEmpty(projectDetails.ProjectName))
                {
                    if (projectDetails.ProjectName.ToUpper().Trim() == "DWO")
                    {
                        if (View.SelectedObjects.Count > 0)
                        {
                            //if (View.Id == "CRMQuotes_DetailView")
                            //{
                            //    ObjectSpace.CommitChanges();
                            //}
                            string strCOCID = string.Empty;
                            List<string> strCOCIDlist = new List<string>();
                            foreach (Samplecheckin obj in View.SelectedObjects)
                            {
                                if (obj.JobID != null)
                                {
                                    strCOCIDlist.Add(obj.JobID);
                                    if (string.IsNullOrEmpty(strCOCID))
                                    {
                                        strCOCID = "'" + obj.JobID + "'";
                                    }
                                    else
                                    {
                                        strCOCID = strCOCID + ",'" + obj.JobID + "'";
                                    }
                                }
                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add jobId for registrationid and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);

                                }
                            }
                            string strTempPath = Path.GetTempPath();
                            String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\")) == false)
                            {
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\"));
                            }
                            string strExportedPath = HttpContext.Current.Server.MapPath(@"~\Preview\Quotes\" + timeStamp + ".pdf");
                            XtraReport xtraReport = new XtraReport();

                            objDRDCInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            SetConnectionString();

                            DynamicReportBusinessLayer.BLCommon.SetDBConnection(objDRDCInfo.LDMSQLServerName, objDRDCInfo.LDMSQLDatabaseName, objDRDCInfo.LDMSQLUserID, objDRDCInfo.LDMSQLPassword);
                            //DynamicDesigner.GlobalReportSourceCode.strLT = strLT;
                            ObjReportingInfo.strJobID = strCOCID;
                            xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut("RTCRMicrobialReport", ObjReportingInfo, false);
                            //DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport,ObjReportingInfo);
                            xtraReport.ExportToPdf(strExportedPath);
                            string[] path = strExportedPath.Split('\\');
                            int arrcount = path.Count();
                            int sc = arrcount - 3;
                            string OriginalPath = string.Join("/", path.GetValue(sc), path.GetValue(sc + 1), path.GetValue(sc + 2));
                            //WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open(window.location.href.split('{1}')[0]+'{0}');", OriginalPath, View.Id + "/"));
                            WebWindow.CurrentRequestWindow.RegisterClientScript("show", string.Format("window.open('{0}','_blank');", OriginalPath));
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Warning, timer.Seconds, InformationPosition.Top);
                        }
                    }
                }
            }
        }


        private void SampleLabel_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            try
            {
                if (View != null)
                {
                    StringBuilder sb = new StringBuilder();
                    strSampleID = string.Empty;
                    if (View.SelectedObjects.Count > 0)
                    {
                        
                        if (View.Id == "Samplecheckin_ListView_SamplingAllocation")
                        {
                            foreach (Samplecheckin obj in View.SelectedObjects)
                            {
                                SampleTest = true;
                                if (obj.JobID != null)
                                {
                                    IList<Modules.BusinessObjects.SampleManagement.SampleLogIn> lstSamples=ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[JobID] = ?", obj.Oid));
                                    strSampleID = string.Join(",", lstSamples.Where(i => i.Testparameters.Count > 0).Select(i => i.SampleID).ToList());
                                    if (sb.Length==0)
                                    {
                                        sb.Append(strSampleID); 
                                    }
                                    else
                                    {
                                        sb.Append(",");
                                        sb.Append(strSampleID);
                                    }
                                }

                                else
                                {
                                    Application.ShowViewStrategy.ShowMessage("Add jobId for registrationid and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);
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

                            
                            using (SqlConnection conn = new SqlConnection(strConnectionString))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("SampleLabelReportGetData_Sp", conn) { CommandType = CommandType.StoredProcedure })
                                {
                                    cmd.Parameters.AddWithValue("@SampleID", sb.ToString());
                                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                                    {
                                        adapter.Fill(dataTable);
                                    }
                                }
                                conn.Close();
                            }

                            //SqlConnection connection = new SqlConnection(strConnectionString);
                            //SqlCommand command = new SqlCommand("SampleLabelReportGetData_Sp", connection);
                            //connection.Open();
                            //command.CommandType = CommandType.StoredProcedure;
                            //SqlParameter[] param = new SqlParameter[1];
                            //param[0] = new SqlParameter("@SampleID", sb.ToString());
                            //command.Parameters.AddRange(param);
                            //SqlDataAdapter sda = new SqlDataAdapter(command);
                            //sda.Fill(dataTable);
                            //connection.Close();

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
                            Application.ShowViewStrategy.ShowMessage("Add jobid for registrationId and try again", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Select at least one checkbox.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    }
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
