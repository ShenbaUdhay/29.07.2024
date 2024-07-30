using BTLIMS.Module.Controllers;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Pdf;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraReports.UI;
using DynamicDesigner;
using iTextSharp.text.pdf;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Crm;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
//using Rebex.Net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using static System.Net.WebRequestMethods;
//using Ftp = Rebex.Net.Ftp;

namespace LDM.Module.Controllers.Reporting
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BatchReportingViewController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        private string uqOid;
        private string JobID;
        private string SampleID;
        private string QcBatchID;
        private string SampleParameterID;
        private string TestMethodID;
        private string ParameterID;
        public string strFTPServerName = string.Empty;
        public string strFTPPath = string.Empty;
        public string strFTPUserName = string.Empty;
        public string strFTPPassword = string.Empty;
        public int FTPPort = 0;
        public bool strFTPStatus;
        bool boolReportSave = false;
        string strReportIDT = string.Empty;
        DefaultSetting objDefaultReportValidation;
        DefaultSetting objDefaultReportApprove;
        DefaultSetting objDefaultReportDelivery;
        DefaultSetting objDefaultReportArchive;
        DefaultSetting objDefaultReportprintanddownload;
        Samplecheckin objSCI;
        DynamicReportDesignerConnection ObjReportDesignerInfo = new DynamicReportDesignerConnection();
        DefaultSettingInfo objDefaultInfo = new DefaultSettingInfo();
        LDMReportingVariables ObjReportingInfo = new LDMReportingVariables();
        curlanguage objLanguage = new curlanguage();
        SampleRegistrationInfo Sampleinfo = new SampleRegistrationInfo();
        public BatchReportingViewController()
        {
            InitializeComponent();
            TargetViewId = "Samplecheckin_ListView_BatchReporting;"+ "BatchReporting_DetailView;";
            JobIdsRetrieve.TargetViewId = "BatchReporting_DetailView;";
            JobIdsRefresh.TargetViewId = "BatchReporting_DetailView;";
            BatchSave.TargetViewId = "BatchReporting_DetailView;";
            BatchPreview.TargetViewId = "Samplecheckin_ListView_BatchReporting;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                if(View.Id== "Samplecheckin_ListView_BatchReporting")
                {
                    Session currentSession = ((XPObjectSpace)this.ObjectSpace).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    objDefaultReportprintanddownload = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportPrintDownload'"));
                    objDefaultReportValidation = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportValidation'"));
                    objDefaultReportApprove = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportApproval'"));
                    objDefaultReportDelivery = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportDelivery'"));
                    objDefaultReportArchive = uow.FindObject<DefaultSetting>(CriteriaOperator.Parse("NavigationItemNameID='ReportArchive'"));
                    if (objDefaultReportprintanddownload != null && objDefaultReportprintanddownload.Select == true)
                    {
                        objDefaultInfo.boolReportPrintDownload = true;
                    }
                    if (objDefaultReportValidation != null && objDefaultReportValidation.Select == true)
                    {
                        objDefaultInfo.boolReportValidation = true;
                    }
                    if (objDefaultReportApprove != null && objDefaultReportApprove.Select == true)
                    {
                        objDefaultInfo.boolReportApprove = true;
                    }
                    if (objDefaultReportDelivery != null && objDefaultReportDelivery.Select == true)
                    {
                        objDefaultInfo.boolReportdelivery = true;
                    }
                    if (objDefaultReportArchive != null && objDefaultReportArchive.Select == true)
                    {
                        objDefaultInfo.boolReportArchive = true;
                    }
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                    Sampleinfo.lstSampleparameter = new List<Guid>();
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser != null && currentUser.UserName != "Administrator" && currentUser.UserName != "Service")
                    {
                        BatchPreview.Active["hidePreview"] = false;
                        List<string> lstRoles = currentUser.RoleNames.Split(',').ToList();
                        var objRNPs = lstRoles.SelectMany(strrole => ObjectSpace.GetObjects<RoleNavigationPermission>(CriteriaOperator.Parse("[RoleName] = ?", strrole)));
                        NavigationItem objNI = ObjectSpace.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationId] = 'BatchReporting'"));
                        if (objNI != null)
                        {
                            var objRNPDs = objRNPs.SelectMany(objRNP => objRNP.RoleNavigationPermissionDetails).Where(objRNPD => objRNPD.NavigationItem.NavigationId == objNI.NavigationId && objRNPD.Write);
                            if (objRNPDs.Any())
                            {
                                BatchPreview.Active.RemoveItem("hidePreview");
                            }
                        }
                    }
                }
                if(View.Id== "BatchReporting_DetailView")
                {
                    ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                    //BatchSave.Active["hideClose"] = false;
                    //Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    //if (currentUser != null && currentUser.UserName != "Administrator" && currentUser.UserName != "Service")
                    //{
                    //    List<string> lstRoles = currentUser.RoleNames.Split(',').ToList();
                    //    foreach (string strrole in lstRoles)
                    //    {
                    //        RoleNavigationPermission objRNP = ObjectSpace.FindObject<RoleNavigationPermission>(CriteriaOperator.Parse("[RoleName] = ?", strrole));
                    //        NavigationItem objNI = ObjectSpace.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationId] = 'CorrectiveActionVerificationNonconformity'"));
                    //        if (objNI != null)
                    //        {
                    //            foreach (RoleNavigationPermissionDetails objRNPD in objRNP.RoleNavigationPermissionDetails.Where(i => i.NavigationItem.NavigationId == objNI.NavigationId))
                    //            {
                    //                if (objRNPD.Create == true && objRNPD.Write == true && objRNPD.Delete == true)
                    //                {
                    //                    BatchSave.Active.RemoveItem("hideClose");
                    //                }
                    //            }
                    //        }
                    //    }

                    //}
                   
                    Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                    if (currentUser != null && currentUser.UserName != "Administrator" && currentUser.UserName != "Service")
                    {
                        BatchSave.Active["hideClose"] = false;
                        BatchPreview.Active["hidePreview"] = false;
                        List<string> lstRoles = currentUser.RoleNames.Split(',').ToList();
                        var objRNPs = lstRoles.SelectMany(strrole =>ObjectSpace.GetObjects<RoleNavigationPermission>(CriteriaOperator.Parse("[RoleName] = ?", strrole)));
                        NavigationItem objNI = ObjectSpace.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationId] = 'BatchReporting'"));
                        if (objNI != null)
                        {
                            var objRNPDs = objRNPs.SelectMany(objRNP => objRNP.RoleNavigationPermissionDetails).Where(objRNPD => objRNPD.NavigationItem.NavigationId == objNI.NavigationId &&  objRNPD.Write);
                            if (objRNPDs.Any())
                            {
                                BatchSave.Active.RemoveItem("hideClose");
                                BatchPreview.Active.RemoveItem("hidePreview");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            try
            {

                if (View.Id== "BatchReporting_DetailView")
                {
                    BatchReporting objBR = View.CurrentObject as BatchReporting;
                    if (e.PropertyName == "Client")
                    {
                        ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("ProjectID") as ASPxGridLookupPropertyEditor;
                        if (objBR.Client != null)
                        {
                            propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("[customername.Oid] = ? ", objBR.Client.Oid);
                        }
                        else
                        {
                            propertyEditor.CollectionSource.Criteria["ProjectID"] = CriteriaOperator.Parse("");
                        }
                        propertyEditor.Refresh();
                        propertyEditor.RefreshDataSource();
                    } 
                    else if (e.PropertyName == "SampleMatrix")
                    {
                        ASPxGridLookupPropertyEditor propertyEditor = ((DetailView)View).FindItem("Test") as ASPxGridLookupPropertyEditor;
                        if (objBR.SampleMatrix != null)
                        {
                            propertyEditor.CollectionSource.Criteria["Test"] = CriteriaOperator.Parse("[MatrixName]=? ", objBR.SampleMatrix.MatrixName);
                        }
                        else
                        {
                            propertyEditor.CollectionSource.Criteria["Test"] = CriteriaOperator.Parse("");
                        }
                        propertyEditor.Refresh();
                        propertyEditor.RefreshDataSource();
                    }

                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if(View.Id== "Samplecheckin_ListView_BatchReporting")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if(gridListEditor!=null && gridListEditor.Grid!=null)
                    {
                        gridListEditor.Grid.Settings.ShowStatusBar = DevExpress.Web.GridViewStatusBarMode.Hidden;
                        gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                        gridListEditor.Grid.SettingsBehavior.ProcessSelectionChangedOnServer = true;
                        gridListEditor.Grid.FillContextMenuItems += Grid_FillContextMenuItems1;
                        gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        gridListEditor.Grid.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                        {                        
                            sessionStorage.setItem('templateFocusedColumn', null); 
                            if(e.cellInfo.column.fieldName == 'ReportTemplates.Oid')
                            {
                                var fieldName = e.cellInfo.column.fieldName;                       
                                sessionStorage.setItem('templateFocusedColumn', fieldName); 
                            }
                            else 
                            {
                                e.cancel = true;
                            }                                          
                        }";
                        gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                        { 
                            if (s.IsRowSelectedOnPage(e.elementIndex))  
                            { 
                                var FocusedColumn = sessionStorage.getItem('templateFocusedColumn');  
                                var oid;
                                var text;
                                if(FocusedColumn.includes('.'))
                                {     
                                    oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                    text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                               //if(FocusedColumn=='ReportTemplates.Oid')
                                               //{
                                                    //s.batchEditApi.SetCellValue(i,'ReportTemplates.Oid',null,null,false);
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                               //}
                                               //else
                                               //{
                                               //     s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                               //}
                                           
                                            }
                                         }
                                     }        
                                 }
                                 else
                                 {                                                             
                                    var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                    if (e.item.name =='CopyToAllCell')
                                    {
                                        for(var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                        { 
                                            if (s.IsRowSelectedOnPage(i)) 
                                            {
                                                s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);
                                            }
                                        }
                                    }                            
                                 }
                             }
                             e.processOnServer = false;
                             s.UpdateEdit();
                        }";
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }

        }

        private void Grid_FillContextMenuItems1(object sender, DevExpress.Web.ASPxGridViewContextMenuEventArgs e)
        {
            try
            {

                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    e.Items.Add("Copy To All Cell", "CopyToAllCell");
                    e.Items.Remove(e.Items.FindByText("Edit"));
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

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                var selected = gridListEditor.GetSelectedObjects();
                if (View.Id == "Samplecheckin_ListView_BatchReporting")
                {
                    //IObjectSpace objSpace = Application.CreateObjectSpace();
                    string strReportID = null;
                    ////if (((ListView)View).SelectedObjects.Cast<Samplecheckin>().Max(i=>i.BatchReportID)==null)
                    ////private void Grid_SelectionChanged(object sender, EventArgs e)
                    //Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                    //    SelectedData sproc = currentSession.ExecuteSproc("ReportingV5_CreateNewID_SP");
                    //    strReportID = sproc.ResultSet[0].Rows[0].Values[0].ToString(); 
        //    {
                    //    strReportID = ((ListView)View).SelectedObjects.Cast<Samplecheckin>().Max(i => i.BatchReportID);
        //        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    //string prefix = strReportID.Substring(0, 8); 
                    //string numericPart = strReportID.Substring(8); 
                    //int currentNumber = int.Parse(numericPart);
                    foreach (Samplecheckin objCheck in ((ListView)View).CollectionSource.List.Cast<Samplecheckin>().OrderByDescending(i=>i.JobID))
                    {
                        if (selected.Contains(objCheck))
                        {
                            //if(((ListView)View).CollectionSource.List.Cast<Samplecheckin>().OrderByDescending(i => i.JobID).First()== objCheck)
        //                {
                            //    objCheck.BatchReportID = strReportID;
        //                }
        //                else
        //                {
                            //    //if (string.IsNullOrEmpty(objCheck.BatchReportID))
                            //    //{
                            //    currentNumber++;
                            //    objCheck.BatchReportID = prefix + currentNumber.ToString("000");
                            //        var selected = gridListEditor.GetSelectedObjects();
        //        }
                            ReportIDformat(strReportID, objCheck);
                            strReportID = objCheck.BatchReportID;
                        }
                        else
                        {
                            objCheck.BatchReportID = null;
                        }
                    }
                    //View.Refresh();
                    //View.ObjectSpace.Refresh();
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected override void OnDeactivated()
        {
            try
            {
                base.OnDeactivated();
                if(View.Id== "BatchReporting_DetailView")
                {
                    strReportIDT = null;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void JobIdsRetrieve_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "BatchReporting_DetailView")
                {
                    BatchReporting objBR = (BatchReporting)View.CurrentObject;
                    if (objBR != null)
                    {
                        DashboardViewItem viRawData = ((DetailView)View).FindItem("BatchReporting") as DashboardViewItem;
                        string strCriteria = string.Empty;
                        if (viRawData != null && viRawData.InnerView != null)
                        {
                            if (objBR.DateReceivedFrom != DateTime.MinValue || objBR.DateReceivedTo != DateTime.MinValue)
                            {
                                if (objBR.DateReceivedFrom != DateTime.MinValue && objBR.DateReceivedTo != DateTime.MinValue && objBR.DateReceivedFrom <= objBR.DateReceivedTo)
                            {
                                strCriteria = "[RecievedDate] BETWEEN('" + objBR.DateReceivedFrom.Date + "', '" + objBR.DateReceivedTo.Date + "')";
                            }
                                else if(objBR.DateReceivedFrom == DateTime.MinValue && objBR.DateReceivedTo != DateTime.MinValue)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "FillReceivedDateFrom"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                }
                                else if(objBR.DateReceivedFrom != DateTime.MinValue && objBR.DateReceivedTo == DateTime.MinValue)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "FillReceivedDateTo"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                }
                                else if(objBR.DateReceivedFrom > objBR.DateReceivedTo)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "lessFromdate"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            if (objBR.Client != null)
                            {
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    strCriteria = strCriteria + "And" + string.Format("[ClientName.Oid] = '{0}'", objBR.Client.Oid);
                                }
                                else
                                {
                                    strCriteria = string.Format("[ClientName.Oid] = '{0}'", objBR.Client.Oid);
                                }
                            }
                            if (objBR.ProjectID != null)
                            {
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    strCriteria = strCriteria + "And" + string.Format("[ProjectID.Oid] = '{0}'", objBR.ProjectID.Oid);
                                }
                                else
                                {
                                    strCriteria = string.Format("[ProjectID.Oid] = '{0}'", objBR.ProjectID.Oid);
                                }
                            }
                            if (objBR.SampleCategory != null)
                            {
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    strCriteria = strCriteria + "And" + string.Format("[SampleCategory] Like '%" + objBR.SampleCategory.Oid + "%' ");
                                }
                                else
                                {
                                    strCriteria = string.Format("[SampleCategory] Like '%" + objBR.SampleCategory.Oid + "%'");
                                }
                            }
                            if (objBR.SampleMatrix != null )
                            {
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    strCriteria = strCriteria + "And" + string.Format("[SampleMatries] Like '%" + objBR.SampleMatrix.Oid+ "%' ");
                                }
                                else
                                {
                                    strCriteria = string.Format("[SampleMatries] Like '%" + objBR.SampleMatrix.Oid + "%'");
                                }
                            }
                            if (objBR.Test != null )
                            {
                                if (!string.IsNullOrEmpty(strCriteria))
                                {
                                    IList<SampleParameter> lstTestSample = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ?", objBR.Test.Oid));
                                    strCriteria = strCriteria + "And" + string.Format("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestSample.Where(i => i.Samplelogin != null && i.Samplelogin.JobID != null).Select(i => i.Samplelogin.JobID.Oid))) + ")");
                                }
                                else
                                {
                                    IList<SampleParameter> lstTestSample = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Testparameter.TestMethod.Oid] = ?", objBR.Test.Oid));
                                    strCriteria = string.Format("[Oid] In(" + string.Format("'{0}'", string.Join("','", lstTestSample.Where(i=>i.Samplelogin!=null && i.Samplelogin.JobID != null).Select(i=>i.Samplelogin.JobID.Oid))) + ")");
                                }
                            }
                            if (!string.IsNullOrEmpty(strCriteria))
                            {
                                IList<Guid> lstCheckin = ObjectSpace.GetObjects<Samplecheckin>(CriteriaOperator.Parse(strCriteria)).Select(i => i.Oid).ToList();
                                IList<Guid> lstOid = ObjectSpace.GetObjects<SampleParameter>(new GroupOperator(GroupOperatorType.And,new InOperator("Samplelogin.JobID.Oid", lstCheckin),(CriteriaOperator.Parse("[Samplelogin] Is Not Null And [Samplelogin.JobID] Is Not Null And [Status] = 'PendingReporting'")))).Select(i=>i.Samplelogin.JobID.Oid).Distinct().ToList();
                                ((ListView)viRawData.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstOid);
                                if(lstOid.Count==0)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "noavailabledata"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else if(objBR.All)
                            {
                                IList<Guid> lstOid = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin] Is Not Null And [Samplelogin.JobID] Is Not Null And [Status] = 'PendingReporting'")).Select(i => i.Samplelogin.JobID.Oid).Distinct().ToList();
                                ((ListView)viRawData.InnerView).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstOid);
                                if (lstOid.Count == 0)
                                {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "noavailabledata"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                                }
                            }
                            else
                            {
                                ((ListView)viRawData.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "ChooseQuery"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                            ((ListView)viRawData.InnerView).CollectionSource.ObjectSpace.Refresh();
                        }
                        //info.lstSelectedObject = new List<SampleParameter>();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void JobIdsRefresh_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                //if (Frame is NestedFrame)
                //{
                //    NestedFrame nestedFrame = (NestedFrame)Frame;
                //    if (nestedFrame!=null&&nestedFrame.ViewItem!=null)
                //    {
                //        CompositeView compositeView = nestedFrame.ViewItem.View;
                //        if (compositeView!=null)
                                //{
                //            BatchReporting curBR = compositeView.CurrentObject as BatchReporting;
                //            if (curBR != null)
                //            {
                //                curBR.All = false;
                //                curBR.Client = null;
                //                curBR.ProjectID = null;
                //                curBR.DateReceivedFrom = DateTime.MinValue;
                //                curBR.DateReceivedTo = DateTime.MinValue;
                //                curBR.SampleCategory = null;
                //                curBR.SampleMatrix = null;
                //                curBR.Test = null;
                //                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                //                //DashboardViewItem viBatch = ((DetailView)compositeView).FindItem("BatchReporting") as DashboardViewItem;
                //                //if (viBatch != null && viBatch.InnerView != null)
                //                //{
                //                //    ((ListView)viBatch.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                //                //}
                //            }   
                //        }
                //    }
                                //}
                //else
                //{
                BatchReporting curBR = View.CurrentObject as BatchReporting;
                if (curBR!=null)
                {
                    curBR.All = false;
                    curBR.Client = null;
                    curBR.ProjectID = null;
                    curBR.DateReceivedFrom = DateTime.MinValue;
                    curBR.DateReceivedTo = DateTime.MinValue;
                    curBR.SampleCategory = null;
                    curBR.SampleMatrix = null;
                    curBR.Test = null;
                    DashboardViewItem viBatch = ((DetailView)View).FindItem("BatchReporting") as DashboardViewItem;
                    if(viBatch!=null&& viBatch.InnerView!=null)
                    {
                        ((ListView)viBatch.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("1=2");
                    }
                }
            //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void BatchPreview_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Samplecheckin objtemp = null;
                IObjectSpace os = Application.CreateObjectSpace();
                if (View.Id == "Samplecheckin_ListView_BatchReporting")
                {
                    objtemp = View.CurrentObject as Samplecheckin;
                }
                else
                {
                    DashboardViewItem viBatch = ((DetailView)View).FindItem("BatchReporting") as DashboardViewItem;
                    if (viBatch != null && viBatch.InnerView != null&& viBatch.InnerView.Id == "Samplecheckin_ListView_BatchReporting")
                    {
                        objtemp = objSCI;
                        //os1 = viBatch.InnerView.ObjectSpace;
                    }
                }
                if (objtemp!=null)
                {
                    if (objtemp != null && objtemp.ReportTemplates != null|| Sampleinfo.lstSampleparameter.Count>0)
                    {
                        if(string.IsNullOrEmpty(Sampleinfo.curPackageName))
                        {
                            Sampleinfo.curPackageName = objtemp.ReportTemplates.PackageName;
                        }
                        List<SampleParameter> listSP = new List<SampleParameter>();
                        
                         if (Sampleinfo.lstSampleparameter.Count>0)
                        {
                            listSP = os.GetObjects<SampleParameter>(new InOperator("Oid", Sampleinfo.lstSampleparameter)).ToList();
                        }
                         else
                        {
                            listSP = os.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Status] = 'PendingReporting'", objtemp.Oid)).ToList();
                        }
                        List<string> lstreport = new List<string>();
                        IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?",Sampleinfo.curPackageName));
                        foreach (ReportPackage objrp in objrep.ToList())
                        {
                            lstreport.Add(objrp.ReportName);
                        }
                        uqOid = null;
                        foreach (SampleParameter obj in listSP)
                        {
                            if (obj.NotReport == false)
                            {
                                if (uqOid == null)
                                {
                                    uqOid = "'" + obj.Oid.ToString() + "'";
                                    JobID = "'" + obj.Samplelogin.JobID.JobID + "'";
                                    SampleID = "'" + obj.Samplelogin.SampleID + "'";
                                    //if (obj.QCBatchID != null && obj.QCBatchID.qcseqdetail != null)
                                    //{
                                    //    QcBatchID = "'" + obj.QCBatchID.qcseqdetail.AnalyticalBatchID + "'";
                                    //}
                                    SampleParameterID = "'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                    TestMethodID = "'" + obj.Testparameter.TestMethod.Oid.ToString() + "'";
                                    ParameterID = "'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                }
                                else
                                {
                                    uqOid = uqOid + ",'" + obj.Oid.ToString() + "'";
                                    if (!JobID.Contains(obj.Samplelogin.JobID.JobID))
                                    {
                                        JobID = JobID + ",'" + obj.Samplelogin.JobID.JobID + "'";
                                    }
                                    if (!SampleID.Contains(obj.Samplelogin.SampleID))
                                    {
                                        SampleID = SampleID + ",'" + obj.Samplelogin.SampleID + "'";
                                    }
                                    if (!SampleParameterID.Contains(obj.Testparameter.Parameter.Oid.ToString()))
                                    {
                                        SampleParameterID = SampleParameterID + ",'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                    }
                                    if (!TestMethodID.Contains(obj.Testparameter.TestMethod.Oid.ToString()))
                                    {
                                        TestMethodID = TestMethodID + ",'" + obj.Testparameter.TestMethod.Oid.ToString() + "'";
                                    }
                                    if (!ParameterID.Contains(obj.Testparameter.Parameter.Oid.ToString()))
                                    {
                                        ParameterID = ParameterID + ",'" + obj.Testparameter.Parameter.Oid.ToString() + "'";
                                    }
                                }
                                if (!SampleID.Contains(obj.Samplelogin.SampleID))
                                {
                                    SampleID = SampleID + ",'" + obj.Samplelogin.SampleID + "'";
                                }
                            }
                        }
                        XtraReport xtraReport = new XtraReport();
                        ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                        ObjReportDesignerInfo.WebConfigConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        SetConnectionString();
                        DynamicReportBusinessLayer.BLCommon.SetDBConnection(ObjReportDesignerInfo.LDMSQLServerName, ObjReportDesignerInfo.LDMSQLDatabaseName, ObjReportDesignerInfo.LDMSQLUserID, ObjReportDesignerInfo.LDMSQLPassword);
                        IObjectSpace objSpace = Application.CreateObjectSpace();
                        Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                        if (boolReportSave == true && string.IsNullOrEmpty(strReportIDT))
                        {
                            SelectedData sproc = currentSession.ExecuteSproc("ReportingV5_CreateNewID_SP");
                            strReportIDT = sproc.ResultSet[0].Rows[0].Values[0].ToString();
                        }
                        ObjReportingInfo.struqSampleParameterID = uqOid;
                        ObjReportingInfo.strJobID = JobID;
                        ObjReportingInfo.strSampleID = SampleID;
                        ObjReportingInfo.strLimsReportedDate = DateTime.Now.ToString("MM/dd/yyyy");
                        ObjReportingInfo.strQcBatchID = QcBatchID;
                        ObjReportingInfo.strTestMethodID = TestMethodID;
                        ObjReportingInfo.strParameterID = ParameterID;
                        ObjReportingInfo.strviewid = View.Id.ToString();
                        GlobalReportSourceCode.strTestMethodID = TestMethodID;
                        GlobalReportSourceCode.strParameterID = ParameterID;
                        DynamicDesigner.GlobalReportSourceCode.struqQCBatchID = QcBatchID;
                        DynamicDesigner.GlobalReportSourceCode.strJobID = JobID;
                        List<string> listPage = new List<string>();
                        int pagenumber = 0;
                        using (MemoryStream newms = new MemoryStream())
                        {
                            if (objrep != null && objrep.Count > 0)
                            {
                                var sortobj = objrep.OrderBy(x => x.sort);
                                foreach (ReportPackage report in sortobj.Where(i => i.ReportName != null))
                                {
                                    XtraReport tempxtraReport = new XtraReport();
                                    bool IsReportExist = false;
                                    SelectedData sprocCheckReport = currentSession.ExecuteSproc("CheckReportExists", report.ReportName);
                                    if (sprocCheckReport.ResultSet != null && sprocCheckReport.ResultSet[1] != null && sprocCheckReport.ResultSet[1].Rows[0] != null && sprocCheckReport.ResultSet[1].Rows[0].Values[0] != null)
                                    {
                                        IsReportExist = Convert.ToBoolean(sprocCheckReport.ResultSet[1].Rows[0].Values[0]);
                                    }
                                    if (IsReportExist)
                                    {
                                            ObjReportingInfo.struqSampleParameterID = uqOid;
                                            ObjReportingInfo.strSampleID = SampleID;
                                        if (report.ReportName == "xrEnvTRRPQCComborptnew")
                                        {
                                            GlobalReportSourceCode.dsQCDataSource = DynamicReportBusinessLayer.BLCommon.GetQcComboReportTRRp_DataSet("Env_QCPotraitRegular_RPT_SP", ObjReportingInfo.strSampleID, ObjReportingInfo.struqSampleParameterID, ObjReportingInfo.strTestMethodID, ObjReportingInfo.strParameterID);
                                        }
                                        tempxtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(report.ReportName.ToString(), ObjReportingInfo, false);
                                        DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(tempxtraReport, ObjReportingInfo);
                                        tempxtraReport.CreateDocument();
                                        for (int i = 1; i <= tempxtraReport.Pages.Count; i++)
                                        {
                                            if (report.PageDisplay == true && report.PageCount == true)
                                            {
                                                pagenumber += 1;
                                                listPage.Add(pagenumber.ToString());
                                            }
                                            else if (report.PageCount == true)
                                            {
                                                pagenumber += 1;
                                                listPage.Add("");
                                            }
                                            else
                                            {
                                                listPage.Add("");
                                            }
                                        }
                                        xtraReport.Pages.AddRange(tempxtraReport.Pages);
                                    }
                                }
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    xtraReport.ExportToPdf(ms);
                                    using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                                    {
                                        source.LoadDocument(ms);
                                        foreach (DevExpress.Pdf.PdfPage page in source.Document.Pages)
                                        {
                                            var curpageval = listPage[source.Document.Pages.IndexOf(page)];
                                            if (curpageval.Length > 0)
                                            {
                                                using (DevExpress.Pdf.PdfGraphics graphics = source.CreateGraphics())
                                                {
                                                    DevExpress.Pdf.PdfRectangle rectangle = page.MediaBox;
                                                    RectangleF r = new RectangleF((float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height);
                                                    SolidBrush black = (SolidBrush)Brushes.Black;
                                                    using (Font font = new Font("Microsoft Yahei", 11, FontStyle.Regular))
                                                    {
                                                        string text;
                                                        if (objLanguage.strcurlanguage == "En")
                                                        {
                                                            text = "Total " + pagenumber + " of " + curpageval + " page";
                                                        }
                                                        else
                                                        {
                                                            text = "共 " + pagenumber + " 页 第 " + curpageval + " 页";
                                                        }
                                                        graphics.DrawString(text, font, black, r.Width + 48, 170);
                                                    }
                                                    graphics.AddToPageForeground(page);
                                                }
                                            }
                                        }
                                        source.SaveDocument(newms);
                                    }
                                }
                            }
                            else
                            {
                                string strReport = Sampleinfo.curPackageName;
                                xtraReport = DynamicDesigner.GlobalReportSourceCode.GetReportFromLayOut(strReport, ObjReportingInfo, false);
                                DynamicDesigner.GlobalReportSourceCode.AssignLimsDatasource(xtraReport, ObjReportingInfo);
                                xtraReport.ExportToPdf(newms);
                            }
                            newms.Position = 0;
                            if (boolReportSave == true)
                            {
                                Sampleinfo.bytevalues = newms.ToArray();
                                boolReportSave = false;
                            }
                            else
                            {
                            MemoryStream tempms = new MemoryStream();
                            NonPersistentObjectSpace Popupos = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(PDFPreview));
                            PDFPreview objToShow = (PDFPreview)Popupos.CreateObject(typeof(PDFPreview));
                            string WatermarkText;
                            if (objLanguage.strcurlanguage == "En")
                            {
                                WatermarkText = "UnApproved";
                            }
                            else
                            {
                                WatermarkText = ConfigurationManager.AppSettings["ReportWaterMarkText"];
                            }
                            using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                            {
                                string fontName = "Microsoft Yahei";
                                int fontSize = 25;
                                PdfStringFormat stringFormat = PdfStringFormat.GenericTypographic;
                                stringFormat.Alignment = PdfStringAlignment.Center;
                                stringFormat.LineAlignment = PdfStringAlignment.Center;
                                documentProcessor.LoadDocument(newms);
                                using (SolidBrush brush = new SolidBrush(Color.FromArgb(63, Color.Black)))
                                {
                                    using (Font font = new Font(fontName, fontSize))
                                    {
                                        foreach (var page in documentProcessor.Document.Pages)
                                        {
                                            var watermarkSize = page.CropBox.Width * 0.75;
                                            using (DevExpress.Pdf.PdfGraphics graphics = documentProcessor.CreateGraphics())
                                            {
                                                SizeF stringSize = graphics.MeasureString(WatermarkText, font);
                                                Single scale = Convert.ToSingle(watermarkSize / stringSize.Width);
                                                graphics.TranslateTransform(Convert.ToSingle(page.CropBox.Width * 0.5), Convert.ToSingle(page.CropBox.Height * 0.5));
                                                graphics.RotateTransform(-45);
                                                graphics.TranslateTransform(Convert.ToSingle(-stringSize.Width * scale * 0.5), Convert.ToSingle(-stringSize.Height * scale * 0.5));
                                                using (Font actualFont = new Font(fontName, fontSize * scale))
                                                {
                                                    RectangleF rect = new RectangleF(0, 0, stringSize.Width * scale, stringSize.Height * scale);
                                                    graphics.DrawString(WatermarkText, actualFont, brush, rect, stringFormat);
                                                }
                                                graphics.AddToPageForeground(page, 72, 72);
                                            }
                                        }
                                    }
                                }
                                documentProcessor.SaveDocument(tempms);
                            }
                            objToShow.PDFData = tempms.ToArray();
                            DetailView CreatedDetailView = Application.CreateDetailView(Popupos, objToShow);
                            CreatedDetailView.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(CreatedDetailView);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "PDFViewer";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.AcceptAction.Active.SetItemValue("disable", false);
                            dc.CancelAction.Active.SetItemValue("disable", false);
                            dc.CloseOnCurrentObjectProcessing = false;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                            Sampleinfo.curPackageName = string.Empty;
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "selecttemplate"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void SetConnectionString()
        {
            try
            {
                AppSettingsReader config = new AppSettingsReader();
                //string serverType, server, database, user, password;
                string[] connectionstring = ObjReportDesignerInfo.WebConfigConn.Split(';');
                ObjReportDesignerInfo.LDMSQLServerName = connectionstring[0].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLDatabaseName = connectionstring[1].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLUserID = connectionstring[2].Split('=').GetValue(1).ToString();
                ObjReportDesignerInfo.LDMSQLPassword = connectionstring[3].Split('=').GetValue(1).ToString();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void BatchSave_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id== "BatchReporting_DetailView")
                {

                    DashboardViewItem viBatch = ((DetailView)View).FindItem("BatchReporting") as DashboardViewItem;
                    if (viBatch != null && viBatch.InnerView != null)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)viBatch.InnerView).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null)
                        {
                            gridListEditor.Grid.UpdateEdit();
                        }
                        if (((ListView)viBatch.InnerView).CollectionSource.List.Count > 0)
                {
                            if (((ListView)viBatch.InnerView).SelectedObjects.Count>0)
                            {
                            if (((ListView)viBatch.InnerView).SelectedObjects.Cast<Samplecheckin>().Where(i => i.ReportTemplates == null).Count() > 0)
                    {
                                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "templatemust"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    }
                    else
                    {
                        string firstReportID = string.Empty;
                        string LastReportID = string.Empty;
                        bool onlytwo = false;
                                    foreach (Samplecheckin objSamplecheck in ((ListView)viBatch.InnerView).SelectedObjects)
                        {
                            if (objSamplecheck.ReportTemplates != null && objSamplecheck.ReportTemplates.PackageName != null)// null)
                            {
                                Sampleinfo.curPackageName = objSamplecheck.ReportTemplates.PackageName;

                                            bool IsAllowMultipleJobID = true;
                                            List<string> lstreport = new List<string>();
                                            IList<ReportPackage> objrep = ObjectSpace.GetObjects<ReportPackage>(CriteriaOperator.Parse("PackageName=?", objSamplecheck.ReportTemplates.PackageName));
                                            foreach (ReportPackage objrp in objrep.ToList())
                                            {
                                                lstreport.Add(objrp.ReportName);
                                            }
                                IObjectSpace objSpace = Application.CreateObjectSpace();
                                List<SampleParameter> lstsamparameter = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Status] = 'PendingReporting'", objSamplecheck.Oid)).ToList();
                                            var jobid = lstsamparameter.Select(i => i.Samplelogin.JobID).Distinct().ToList();
                                            if (jobid.Count() > 1)
                                            {
                                                if (lstreport.Count > 0)
                                                {
                                                    IList<tbl_Public_CustomReportDesignerDetails> objreport = ObjectSpace.GetObjects<tbl_Public_CustomReportDesignerDetails>(new InOperator("colCustomReportDesignerName", lstreport));
                                                    List<tbl_Public_CustomReportDesignerDetails> lstAllowjobidcnt = objreport.Where(i => i.AllowMultipleJOBID == false).ToList();
                                                    if (lstAllowjobidcnt.Count > 0)
                                                    {
                                                        IsAllowMultipleJobID = false;
                                                    }
                                                }
                                            }

                                            if (/*jobid.Count() == 1*/IsAllowMultipleJobID == true)
                                {
                                    boolReportSave = true;
                                    bool stat;
                                    Session currentSession = ((XPObjectSpace)(objSpace)).Session;
                                                //if (boolReportSave == true)
                                                //{
                                                //    SelectedData sproc = currentSession.ExecuteSproc("ReportingV5_CreateNewID_SP");
                                                //    strReportIDT = sproc.ResultSet[0].Rows[0].Values[0].ToString();
                                                //}
                                    Modules.BusinessObjects.SampleManagement.Reporting objReporting = new Modules.BusinessObjects.SampleManagement.Reporting(currentSession);
                                                objReporting.ReportID = objSamplecheck.BatchReportID;
                                    objReporting.ReportedDate = DateTime.Now;
                                    objReporting.ReportedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                    objReporting.ReportName = objSamplecheck.ReportTemplates.PackageName;
                                    objReporting.ReportStatus = ReportStatus.Pending1stReview;
                                                if (objSamplecheck.ClientName!=null)
                                    {
                                                    objReporting.Email = objSamplecheck.ClientName.Contacts.Where(i => i.Email != null && i.IsReport == true).Select(i => i.Email).FirstOrDefault();
                                    }
                                    if (objDefaultInfo.boolReportValidation == true)
                                    {
                                                    //objReporting.ReportApprovedDate = DateTime.Now;
                                                    //objReporting.ReportApprovedBy = View.ObjectSpace.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportStatus = ReportStatus.Pending1stReview;
                                        AddReportSign(objReporting, IsReported: true, IsValidated: false, IsApproved: false);
                                    }
                                    if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == true)
                                    {
                                        objReporting.ReportValidatedDate = DateTime.Now;
                                        objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.LastUpdatedDate = DateTime.Now;
                                        objReporting.LastUpdatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportStatus = ReportStatus.Pending2ndReview;
                                        AddReportSign(objReporting, IsReported: true, IsValidated: true, IsApproved: false);
                                    }
                                    if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == true)
                                    {
                                        objReporting.ReportValidatedDate = DateTime.Now;
                                        objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportApprovedDate = DateTime.Now;
                                        objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.LastUpdatedDate = DateTime.Now;
                                        objReporting.LastUpdatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportStatus = ReportStatus.PendingPrint;
                                    }
                                    else if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == true)
                                    {
                                        objReporting.ReportValidatedDate = DateTime.Now;
                                        objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportApprovedDate = DateTime.Now;
                                        objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportStatus = ReportStatus.PendingDelivery;

                                        stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportPrintDownloadShow"]);
                                        if (!stat)
                                        {
                                            objReporting.DatePrinted = DateTime.Now;
                                            objReporting.PrintedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            objReporting.ReportStatus = ReportStatus.PendingDelivery;
                                                        ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                                        ////if (!stat)
                                                        ////{
                                                        ////    objReporting.DateDelivered = DateTime.Now;
                                                        ////    objReporting.DeliveredBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                        ////    objReporting.ReportStatus = ReportStatus.PendingArchive;
                                                        ////    stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                                        ////    if (!stat)
                                                        ////    {
                                                        ////        objReporting.DateArchived = DateTime.Now;
                                                        ////        objReporting.ArchivedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                        ////        objReporting.ReportStatus = ReportStatus.Archived;
                                                        ////    }
                                                        ////}
                                        }
                                        AddReportSign(objReporting, IsReported: true, IsValidated: true, IsApproved: true);
                                    }
                                    else if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == true)
                                    {
                                        objReporting.ReportValidatedDate = DateTime.Now;
                                        objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportApprovedDate = DateTime.Now;
                                        objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportStatus = ReportStatus.PendingArchive;
                                        // stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportDeliveryShow"]);
                                        Modules.BusinessObjects.Setting.DefaultSetting objReportDelivery = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'ReportDelivery'"));
                                        if (objReportDelivery.Select == false)
                                        {
                                            objReporting.DateDelivered = DateTime.Now;
                                            objReporting.DeliveredBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                            objReporting.ReportStatus = ReportStatus.PendingArchive;
                                            // stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                            Modules.BusinessObjects.Setting.DefaultSetting objReportArchive = ObjectSpace.FindObject<DefaultSetting>(CriteriaOperator.Parse("[NavigationItemNameID] = 'ReportArchive'"));
                                            if (objReportArchive.Select == false)
                                            {
                                                objReporting.DateArchived = DateTime.Now;
                                                objReporting.ArchivedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                                objReporting.ReportStatus = ReportStatus.Archived;
                                            }
                                        }
                                        AddReportSign(objReporting, IsReported: true, IsValidated: true, IsApproved: true);
                                    }
                                    else if (objDefaultInfo.boolReportValidation == false && objDefaultInfo.boolReportApprove == false && objDefaultInfo.boolReportPrintDownload == false && objDefaultInfo.boolReportdelivery == false && objDefaultInfo.boolReportArchive == false)
                                    {
                                        objReporting.ReportValidatedDate = DateTime.Now;
                                        objReporting.ReportValidatedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportApprovedDate = DateTime.Now;
                                        objReporting.ReportApprovedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.DeliveredBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        objReporting.ReportStatus = ReportStatus.ReportDelivered;
                                        ////stat = Convert.ToBoolean(ConfigurationManager.AppSettings["ReportArchiveShow"]);
                                        ////if (!stat)
                                        ////{
                                        ////    objReporting.DateArchived = DateTime.Now;
                                        ////    objReporting.ArchivedBy = currentSession.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
                                        ////    objReporting.ReportStatus = ReportStatus.Archived;
                                        ////}
                                        AddReportSign(objReporting, IsReported: true, IsValidated: true, IsApproved: true);
                                    }


                                    List<string> lstvisualmatrix = new List<string>();
                                                foreach (SampleParameter objLineA in lstsamparameter)
                                    {
                                                    CriteriaOperator criteria = CriteriaOperator.Parse("[Oid]='" + objLineA.Oid + "'");
                                                    SampleParameter objSample = objSpace.FindObject<SampleParameter>(criteria);
                                        objReporting.SampleParameter.Add(objSample);
                                        objSample.Status = Samplestatus.Reported;
                                        objSample.OSSync = true;
                                                    if (objLineA.Samplelogin != null && objLineA.Samplelogin.VisualMatrix != null)
                                        {
                                                        if (!lstvisualmatrix.Contains(objLineA.Samplelogin.VisualMatrix.VisualMatrixName))
                                            {
                                                            lstvisualmatrix.Add(objLineA.Samplelogin.VisualMatrix.VisualMatrixName);
                                            }
                                        }
                                                    objReporting.JobID = objSample.Samplelogin.JobID;
                                    }
                                    objReporting.SampleType = string.Join(",", lstvisualmatrix);
                                                objSCI = objSamplecheck;
                                                strReportIDT = objSamplecheck.BatchReportID;
                                                BatchPreview_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));
                                    objSpace.CommitChanges();
                                    bool isSave = false;
                                    IList<SampleParameter> lstsmpl1 = objSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.JobID.Oid] = ? And [Testparameter.QCType.QCTypeName] = 'Sample'", objReporting.JobID.Oid));
                                    if (lstsmpl1.Count() == lstsmpl1.Where(i => i.Status == Samplestatus.Reported).Count())
                                    {
                                        StatusDefinition objStatus = objSpace.FindObject<StatusDefinition>(CriteriaOperator.Parse("[UqIndexID] = 23"));
                                        if (objStatus != null)
                                        {
                                            Samplecheckin objJobID = objSpace.GetObjectByKey<Samplecheckin>(objReporting.JobID.Oid);
                                            objJobID.Index = objStatus;
                                            isSave = true;
                                        }
                                    }
                                    if (isSave)
                                    {
                                        objSpace.CommitChanges();
                                    }
                                                FileLinkSepDBController obj = Frame.GetController<FileLinkSepDBController>();
                                                if (obj != null)
                                                {
                                                    obj.FileLink(strReportIDT, Sampleinfo.bytevalues);
                                                }
                                                int count = ((ListView)viBatch.InnerView).SelectedObjects.Count - 1;
                                                if (((ListView)viBatch.InnerView).SelectedObjects[0] == objSamplecheck)
                                    {
                                                    firstReportID = objSamplecheck.BatchReportID;
                                    }
                                                else if (((ListView)viBatch.InnerView).SelectedObjects[count] == objSamplecheck)
                                    {
                                                    LastReportID = objSamplecheck.BatchReportID; ;
                                                    if (((ListView)viBatch.InnerView).SelectedObjects.Count == 2)
                                        {
                                            onlytwo = true;
                                        }
                                    }
                                }
                                else
                                {
                                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "FTPNotConnected"), InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(LastReportID))
                        {
                                        strReportIDT = null;
                                    Application.ShowViewStrategy.ShowMessage(firstReportID + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "Batchreportsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        else if (onlytwo)
                        {
                                        strReportIDT = null;
                                    Application.ShowViewStrategy.ShowMessage(firstReportID + " , " + LastReportID + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "Batchreportsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {
                                        strReportIDT = null;
                                        Application.ShowViewStrategy.ShowMessage(firstReportID + "........" + LastReportID + CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "Batchreportsuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        JobIdsRefresh_Execute(new object(), new SimpleActionExecuteEventArgs(null, null));
                                    //List<Guid> lstGuid = View.SelectedObjects.Cast<SampleParameter>().ToList().Select(i => i.Oid).ToList();
                                } 
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages", "selectchkbox"), InformationType.Info, timer.Seconds, InformationPosition.Top);
                    } 
                }
                else
                {
                            Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\LDMMessages\BatchReporting", "collectionempty"), InformationType.Info, timer.Seconds, InformationPosition.Top);
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
        private void AddReportSign(Modules.BusinessObjects.SampleManagement.Reporting objReporting, bool IsReported, bool IsValidated, bool IsApproved)
        {
            try
            {
                List<string> listPage = new List<string>();
                XtraReport xtraReport = new XtraReport();
                ObjReportDesignerInfo.WebConfigFTPConn = ((NameValueCollection)System.Configuration.ConfigurationManager.GetSection("FTPConnectionStrings"))["FTPConnectionString"];
                //ReadXmlFile_FTPConc();
                //Rebex.Net.Ftp _FTP = GetFTPConnection();
                //string strRemotePath = /*"//CONSCI//LDMReports//";*/  ConfigurationManager.AppSettings["FinalReportPath"];
                //string strExportedPath = strRemotePath.Replace(@"\", "//") + objReporting.ReportID + ".pdf";
                //if (_FTP.FileExists(strExportedPath))
                FileLinkSepDBController objfilelink = Frame.GetController<FileLinkSepDBController>();
                if (objfilelink != null)
                {
                    DataTable dt = objfilelink.GetFileLink(objReporting.ReportID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        byte[] objbyte = (byte[])dt.Rows[0]["FileContent"];
                        MemoryStream ms = new MemoryStream(objbyte);
                        //_FTP.TransferType = FtpTransferType.Binary;
                        //_FTP.GetFile(strExportedPath, ms);

                    #region AddSignatureusingSpiredll
                    //Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                    //doc.LoadFromStream(ms);
                    //PdfTextFind[] results = null;
                    //foreach (PdfPageBase page in doc.Pages)
                    //{
                    //    if (IsReported)
                    //    {
                    //        string strSearchText = "报告人";
                    //        byte[] img;
                    //        if (objReporting.ReportedBy != null)
                    //        {
                    //            img = objReporting.ReportedBy.Signature;
                    //        }
                    //        else
                    //        {
                    //            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                    //        }

                    //        if (page.FindText(strSearchText, TextFindParameter.None) != null)
                    //        {
                    //            results = page.FindText(strSearchText, TextFindParameter.None).Finds;
                    //            foreach (PdfTextFind text in results)
                    //            {
                    //                PointF p = new PointF(text.Position.X + 50, text.Position.Y - 8);
                    //                SizeF imgSize = new SizeF(100.5F, 20F);
                    //                MemoryStream imgms = new MemoryStream(img);
                    //                System.Drawing.Image imgSignature = System.Drawing.Image.FromStream(imgms);
                    //                Spire.Pdf.Graphics.PdfImage pdfimage = Spire.Pdf.Graphics.PdfImage.FromImage(imgSignature);
                    //                page.Canvas.DrawImage(pdfimage, p, imgSize);
                    //            }
                    //        }
                    //    }
                    //    if (IsValidated)
                    //    {
                    //        string strSearchText = "签发人";
                    //        byte[] img;
                    //        if (objReporting.ReportValidatedBy != null)
                    //        {
                    //            img = objReporting.ReportValidatedBy.Signature;
                    //        }
                    //        else
                    //        {
                    //            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                    //        }

                    //        if (page.FindText(strSearchText, TextFindParameter.None) != null)
                    //        {
                    //            results = page.FindText(strSearchText, TextFindParameter.None).Finds;
                    //            foreach (PdfTextFind text in results)
                    //            {
                    //                PointF p = new PointF(text.Position.X + 50, text.Position.Y - 8);
                    //                SizeF imgSize = new SizeF(100.5F, 20F);
                    //                MemoryStream imgms = new MemoryStream(img);
                    //                System.Drawing.Image imgSignature = System.Drawing.Image.FromStream(imgms);
                    //                Spire.Pdf.Graphics.PdfImage pdfimage = Spire.Pdf.Graphics.PdfImage.FromImage(imgSignature);
                    //                page.Canvas.DrawImage(pdfimage, p, imgSize);
                    //            }
                    //        }
                    //    }
                    //    if (IsApproved)
                    //    {
                    //        string strSearchText = "批准人";
                    //        byte[] img;
                    //        if (objReporting.ReportApprovedBy != null)
                    //        {
                    //            img = objReporting.ReportApprovedBy.Signature; 
                    //        }
                    //        else
                    //        {
                    //            img = ((Employee)SecuritySystem.CurrentUser).Signature;
                    //        }

                    //        if (page.FindText(strSearchText, TextFindParameter.None) != null)
                    //        {
                    //            results = page.FindText(strSearchText, TextFindParameter.None).Finds;
                    //            foreach (PdfTextFind text in results)
                    //            {
                    //                PointF p = new PointF(text.Position.X + 50, text.Position.Y - 8);
                    //                SizeF imgSize = new SizeF(100.5F, 20F);
                    //                MemoryStream imgms = new MemoryStream(img);
                    //                System.Drawing.Image imgSignature = System.Drawing.Image.FromStream(imgms);
                    //                Spire.Pdf.Graphics.PdfImage pdfimage = Spire.Pdf.Graphics.PdfImage.FromImage(imgSignature);
                    //                page.Canvas.DrawImage(pdfimage, p, imgSize);
                    //            }
                    //        }
                    //    }
                    //}
                    //doc.SaveToStream(tempms);
                    #endregion

                    #region AddSignatureusingiTextSharpdll
                    //Create an instance of our strategy
                    LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy reportedbyExtractor = new LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy();
                    LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy validatedbyExtractor = new LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy();
                    LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy approvedbyExtractor = new LDM.Module.BusinessObjects.MyLocationTextExtractionStrategy();

                    //reportedbyExtractor.TextToSearchFor = "报告人";
                    //validatedbyExtractor.TextToSearchFor = "签发人";
                    //approvedbyExtractor.TextToSearchFor = "批准人";

                    reportedbyExtractor.TextToSearchFor = "主检：";
                    validatedbyExtractor.TextToSearchFor = "审核：";
                    approvedbyExtractor.TextToSearchFor = "批准：";

                    MemoryStream tempms = new MemoryStream();
                    iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(ms.ToArray());

                    for (int pageno = 1; pageno <= reader.NumberOfPages; pageno++)
                    {
                        reportedbyExtractor.pageno = validatedbyExtractor.pageno = approvedbyExtractor.pageno = pageno;
                        string strReportedResult = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pageno, reportedbyExtractor);
                        string strValidatedResult = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pageno, validatedbyExtractor);
                        string strApprovedResult = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, pageno, approvedbyExtractor);
                    }

                    var stamper = new PdfStamper(reader, tempms);
                    for (int pageno = 1; pageno <= reader.NumberOfPages; pageno++)
                    {
                        var pdfContentByte = stamper.GetOverContent(pageno);
                        if (IsReported)
                        {
                            if (reportedbyExtractor != null && reportedbyExtractor.myPoints != null && reportedbyExtractor.myPoints.Count > 0)
                            {
                                LDM.Module.BusinessObjects.GetTextAndRectangle extractor = reportedbyExtractor.myPoints.FirstOrDefault(i => i.pageno == pageno);
                                if (extractor != null && extractor.Rect != null)
                                {
                                    byte[] img;
                                    if (objReporting.ReportedBy != null)
                                    {
                                        img = objReporting.ReportedBy.Signature;
                                    }
                                    else
                                    {
                                        img = ((Employee)SecuritySystem.CurrentUser).Signature;
                                    }
                                    iTextSharp.text.Image sigimage = iTextSharp.text.Image.GetInstance(img);
                                    sigimage.SetAbsolutePosition(extractor.Rect.Left + extractor.Rect.Width + 20F, extractor.Rect.Top + extractor.Rect.Height - 25F);
                                    sigimage.ScaleToFit(105f, 25f);
                                    pdfContentByte.AddImage(sigimage);
                                }
                            }
                        }
                        if (IsValidated)
                        {
                            if (validatedbyExtractor != null && validatedbyExtractor.myPoints != null && validatedbyExtractor.myPoints.Count > 0)
                            {
                                LDM.Module.BusinessObjects.GetTextAndRectangle extractor = validatedbyExtractor.myPoints.FirstOrDefault(i => i.pageno == pageno);
                                if (extractor != null && extractor.Rect != null)
                                {
                                    byte[] img;
                                    if (objReporting.ReportValidatedBy != null)
                                    {
                                        img = objReporting.ReportValidatedBy.Signature;
                                    }
                                    else
                                    {
                                        img = ((Employee)SecuritySystem.CurrentUser).Signature;
                                    }
                                    iTextSharp.text.Image sigimage = iTextSharp.text.Image.GetInstance(img);
                                    sigimage.SetAbsolutePosition(extractor.Rect.Left + extractor.Rect.Width + 20F, extractor.Rect.Top + extractor.Rect.Height - 25F);
                                    sigimage.ScaleToFit(105f, 25f);
                                    pdfContentByte.AddImage(sigimage);
                                }
                            }
                        }
                        if (IsApproved)
                        {
                            if (approvedbyExtractor != null && approvedbyExtractor.myPoints != null && approvedbyExtractor.myPoints.Count > 0)
                            {
                                LDM.Module.BusinessObjects.GetTextAndRectangle extractor = approvedbyExtractor.myPoints.FirstOrDefault(i => i.pageno == pageno);
                                if (extractor != null && extractor.Rect != null)
                                {
                                    byte[] img;
                                    if (objReporting.ReportApprovedBy != null)
                                    {
                                        img = objReporting.ReportApprovedBy.Signature;
                                    }
                                    else
                                    {
                                        img = ((Employee)SecuritySystem.CurrentUser).Signature;
                                    }
                                    iTextSharp.text.Image sigimage = iTextSharp.text.Image.GetInstance(img);
                                    sigimage.SetAbsolutePosition(extractor.Rect.Left + extractor.Rect.Width + 20F, extractor.Rect.Top + extractor.Rect.Height - 25F);
                                    sigimage.ScaleToFit(105f, 25f);
                                    pdfContentByte.AddImage(sigimage);
                                }
                            }
                        }
                    }
                    stamper.Close();
                    reader.Close();
                    #endregion

                        objfilelink.FileLinkUpdate(objReporting.ReportID, tempms.ToArray());
                        //String timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        //string strFilePath = HttpContext.Current.Server.MapPath(@"~\ReportPreview\" + timeStamp + ".pdf");
                        //if (Directory.Exists(HttpContext.Current.Server.MapPath(@"~\ReportPreview")) == false)
                        //{
                        //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(@"~\ReportPreview"));
                        //}
                        //System.IO.File.WriteAllBytes(strFilePath, tempms.ToArray());
                        ////_FTP.DeleteFile(strExportedPath);
                        //////_FTP.PutFile(tempms, strExportedPath);
                        ////_FTP.PutFile(strFilePath, strExportedPath);
                        //FileInfo file = new FileInfo(strFilePath);
                        //if (file.Exists)
                        //{
                        //    file.Delete();
                        //} 
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        //public void ReadXmlFile_FTPConc()
        //{
        //    try
        //    {
        //        string[] FTPconnectionstring = ObjReportDesignerInfo.WebConfigFTPConn.Split(';');
        //        strFTPServerName = FTPconnectionstring[0].Split('=').GetValue(1).ToString();
        //        strFTPUserName = FTPconnectionstring[1].Split('=').GetValue(1).ToString();
        //        strFTPPassword = FTPconnectionstring[2].Split('=').GetValue(1).ToString();
        //        strFTPPath = FTPconnectionstring[3].Split('=').GetValue(1).ToString();
        //        FTPPort = Convert.ToInt32(FTPconnectionstring[4].ToString().Split('=').GetValue(1).ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //    }

        //}
        //public Rebex.Net.Ftp GetFTPConnection()
        //{
        //    try
        //    {
        //        Rebex.Net.Ftp FTP = new Rebex.Net.Ftp();
        //        FTP.TransferType = FtpTransferType.Binary;
        //        if ((!(strFTPServerName == null)
        //                    && ((strFTPServerName.Length > 0)
        //                    && (!(strFTPUserName == null)
        //                    && (strFTPUserName.Length > 0)))))
        //        {
        //            try
        //            {
        //                FTP.Timeout = 6000;
        //                FTP.Connect(strFTPServerName, FTPPort);
        //                FTP.Login(strFTPUserName, strFTPPassword);
        //                strFTPStatus = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                strFTPStatus = false;
        //                return new Rebex.Net.Ftp();
        //            }
        //        }
        //        else
        //        {
        //            strFTPStatus = false;
        //            return new Rebex.Net.Ftp();
        //        }

        //        return FTP;
        //    }
        //    catch (Exception ex)
        //    {
        //        Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
        //        Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
        //        return null;
        //    }
        //}
        public void ReportIDformat(string strReportID, Samplecheckin objSCI)
        {
            ReportIDFormat idFormat = ObjectSpace.FindObject<ReportIDFormat>(null);
            if (idFormat != null)
            {
                string jd = objSCI.JobID;
                string newReportID = null;
                var curdate = DateTime.Now;
                if (idFormat.Prefixs == YesNoFilters.Yes)
                {
                    newReportID = idFormat.PrefixsValue;
                }
                if (idFormat.ReportIDFormatOption == ReportIDFormatOption.No)
                {
                    //foreach (string jobId in View.SelectedObjects.OfType<SampleParameter>().Select(sp => sp.Samplelogin.JobID.JobID).Distinct())
                    //{
                    //    jd = jobId;
                        newReportID += jd;
                    //}
                    //jd += jobid;
                    //newReportID += jobid;
                    if (idFormat.SequentialNumber > 0)
                    {
                        var latestReport = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(r => r.JobID.JobID == jd && r.RevisionNo == 0).OrderByDescending(r => r.ReportedDate).FirstOrDefault();
                        if (latestReport != null && latestReport.JobID != null)
                        {
                            string latestJobID = latestReport.JobID.JobID;
                            bool isJobIDMatch = latestJobID.Contains(jd);
                            if (isJobIDMatch)
                            {
                                string baseValue = latestReport.ReportID.Substring(0, latestReport.ReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                                int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                newReportID += nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                                //for (int i = 0; i < idFormat.SequentialNumber; i++)
                                //{
                                //    string str = "0";
                                //    newReportID += str;
                                //}
                                //string lastDigits = latestReport.ReportID.Substring(latestReport.ReportID.Length - 3);
                                //int nextSequentialNumber = int.Parse(lastDigits) + 1;
                                //newReportID = newReportID.Substring(0, newReportID.Length - 3) + nextSequentialNumber.ToString().PadLeft(3, '0');
                            }
                            else
                            {
                                 newReportID = newReportID + new string('0',Convert.ToInt32(idFormat.SequentialNumber-1)) + "1";
                            }
                        }
                        else
                        {
                             newReportID = newReportID + new string('0', Convert.ToInt32(idFormat.SequentialNumber-1)) + "1";
                        }
                    }
                }
                if (idFormat.ReportIDFormatOption == ReportIDFormatOption.Yes)
                {
                    string currentDateSubstring = "";

                    if (idFormat.Year == YesNoFilters.Yes)
                    {
                        newReportID += curdate.ToString(idFormat.YearFormat.ToString());
                        currentDateSubstring += curdate.ToString(idFormat.YearFormat.ToString());
                    }
                    if (idFormat.Month == YesNoFilters.Yes)
                    {
                        newReportID += curdate.ToString(idFormat.MonthFormat.ToUpper());
                        currentDateSubstring += curdate.ToString(idFormat.MonthFormat.ToUpper());
                    }
                    if (idFormat.Day == YesNoFilters.Yes)
                    {
                        newReportID += curdate.ToString(idFormat.DayFormat);
                        currentDateSubstring += curdate.ToString(idFormat.DayFormat);
                    }
                    if (idFormat.SequentialNumber > 0)
                    {

                        if (string.IsNullOrEmpty(strReportID))
                        {
                            var latestReport = ObjectSpace.GetObjects<Modules.BusinessObjects.SampleManagement.Reporting>().Where(r => r.RevisionNo == 0).OrderByDescending(r => r.ReportedDate).FirstOrDefault(r => r.ReportID.Contains(currentDateSubstring));
                            if (latestReport!=null)
                            {
                                strReportID = latestReport.ReportID; 
                            }
                        }
                        if (!string.IsNullOrEmpty(strReportID))
                        {
                            string baseValue = strReportID.Substring(0, strReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                            string lastDigits = strReportID.Substring(strReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                            int nextSequentialNumber = int.Parse(lastDigits) + 1;
                            newReportID +=  nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                            // newReportID = newReportID + new string('0', Convert.ToInt32(idFormat.SequentialNumber-1));
                            ////for (int i = 0; i < idFormat.SequentialNumber; i++)
                            ////{
                            ////    string str = "0";
                            ////    newReportID += str;
                            ////}
                            //string lastDigits = strReportID.Substring(strReportID.Length - Convert.ToInt32(idFormat.SequentialNumber));
                            //int nextSequentialNumber = int.Parse(lastDigits) + 1;
                            //newReportID = newReportID.Substring(0, newReportID.Length - Convert.ToInt32(idFormat.SequentialNumber)) + nextSequentialNumber.ToString().PadLeft(Convert.ToInt32(idFormat.SequentialNumber), '0');
                        }
                        else
                        {
                             newReportID = newReportID + new string('0', Convert.ToInt32(idFormat.SequentialNumber-1)) + "1";
                        }
                    }
                }
                objSCI.BatchReportID = newReportID;
            }
        }
    }
}
