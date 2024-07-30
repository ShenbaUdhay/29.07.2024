using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.SampleRegistration
{
    public partial class SampleRegistrationBottleAllocationViewController : ViewController, IXafCallbackHandler
    {
        SampleRegistrationInfo SRInfo = new SampleRegistrationInfo();
        MessageTimer timer = new MessageTimer();
        ICallbackManagerHolder BottleDelcallbackManager;
        PermissionInfo objPermissionInfo = new PermissionInfo();

        public SampleRegistrationBottleAllocationViewController()
        {
            InitializeComponent();
            TargetViewId = "SampleRegistration;" + "SampleBottleAllocation_ListView_Sampleregistration;"
                + "SampleLogIn_ListView_SampleRegistration_SeletedSampleID;" + "DummyClass_ListView_SampleRegistration_TEST_POPUP;"
                + "DummyClass_ListView_SampleRegistration;" + "TestMethod_ListView_SamplesBA_Popup;"
                + "SampleBottleAllocation_DetailView_SampleRegistration;" + "SampleLogIn_ListView_SampleRegistration_BottleAllocation;" + "SampleLogIn_ListView_Bottle;" + "NPSampleLogIn_Bottle_DetailView;";
            btnAssignbottles_BottleAllocation.TargetViewId = btnCopybottles_BottleAllocation.TargetViewId = btnResetbottles_BottleAllocation.TargetViewId = "SampleBottleAllocation_ListView_Sampleregistration;";
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                btnAssignbottles_BottleAllocation.Active["showAssignbtlalloc"] = objPermissionInfo.SampleRegIsWrite;
                btnCopybottles_BottleAllocation.Active["showCopybtlalloc"] = objPermissionInfo.SampleRegIsWrite;
                btnResetbottles_BottleAllocation.Active["showresetbtlalloc"] = objPermissionInfo.SampleRegIsWrite;
                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                {
                    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                    if (popupWindow != null && popupWindow.View != null && popupWindow.View.Id == "SampleBottleAllocation_DetailView_SampleRegistration")
                    {
                        popupWindow.RefreshParentWindowOnCloseButtonClick = true;
                    }
                }
                ObjectSpace.Committed += ObjectSpace_Committed;
                if (View.Id == "TestMethod_ListView_Samples_BA" || View.Id == "SampleBottleAllocation_ListView_Sampleregistration")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                }
                if (View.Id == "DummyClass_ListView_SampleRegistration")
                {
                    SRInfo.selectionhideGuid = new List<string>();
                    SRInfo.lstviewselected = new List<DummyClass>();
                }
                if (View.Id == "SampleBottleAllocation_ListView_Sampleregistration" && Application.MainWindow.View.Id != "SampleBottleAllocation_DetailView_SampleTransfer")
                {
                    if (SRInfo.lstsmplbtlallo == null)
                    {
                        SRInfo.lstsmplbtlallo = new List<SampleBottleAllocation>();
                    }
                    foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions.Where(i => i.RoleNavigationPermissionDetails.FirstOrDefault(x => x.NavigationItem.NavigationId == "SampleRegistration") != null))
                    {
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SampleRegistration" && i.Create == true) != null)
                        {
                            objPermissionInfo.SampleBottleIsCreate = true;
                            btnAssignbottles_BottleAllocation.Active["DisableAssign"] = true;
                            btnCopybottles_BottleAllocation.Active["DisableCopySet"] = true;
                            btnResetbottles_BottleAllocation.Active["DisableReset"] = true;
                            return;
                        }
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SampleRegistration" && i.Write == true) != null)
                        {
                            objPermissionInfo.SampleBottleIsWrite = true;
                            btnAssignbottles_BottleAllocation.Active["DisableAssign"] = true;
                            btnCopybottles_BottleAllocation.Active["DisableCopySet"] = true;
                            btnResetbottles_BottleAllocation.Active["DisableReset"] = true;
                            return;
                        }
                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SampleRegistration" && i.Read == true) != null)
                        {
                            objPermissionInfo.SampleBottleIsRead = true;
                            btnAssignbottles_BottleAllocation.Active["DisableAssign"] = false;
                            btnCopybottles_BottleAllocation.Active["DisableCopySet"] = false;
                            btnResetbottles_BottleAllocation.Active["DisableReset"] = false;
                            return;
                        }
                    }
                }
                else if (Application.MainWindow.View.Id == "SampleBottleAllocation_DetailView_SampleTransfer")
                {
                    QtyOk.Active.SetItemValue("enb", false);
                    QtyReset.Active.SetItemValue("enb", false);
                    if (SRInfo.lstsmplbtlallo == null)
                    {
                        SRInfo.lstsmplbtlallo = new List<SampleBottleAllocation>();
                    }
                }
                else if (View.Id == "TestMethod_ListView_SamplesBA_Popup")
                {
                    //SRInfo.lstdummytests = new List<Guid>();
                    //SRInfo.lstdummycreationtests = new List<Guid>();
                    //IList<SampleParameter> lstsmpltest = ObjectSpace.GetObjects<SampleParameter>(CriteriaOperator.Parse("[Samplelogin.Oid] = ?", SRInfo.SamplingGuid)).Where(i => i.Testparameter != null && i.Testparameter.TestMethod != null && !string.IsNullOrEmpty(i.Testparameter.TestMethod.TestName)).GroupBy(i => i.Testparameter.TestMethod.TestName).Select(i => i.FirstOrDefault()).ToList();
                    //((ListView)View).CollectionSource.Criteria["Filter"] = new InOperator("Oid", lstsmpltest.Select(i => i.Testparameter.TestMethod.Oid));
                    //IList<SampleBottleAllocation> lstsmplaloca = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid] = ?", SRInfo.SamplingGuid)).Where(i => i.Oid != new Guid(HttpContext.Current.Session["rowid"].ToString())).ToList();
                    //if (SRInfo.lstavailtest == null)
                    //{
                    //    SRInfo.lstavailtest = new List<string>();
                    //}
                    //if (SRInfo.lstavailtest.Count > 0)
                    //{
                    //    SRInfo.lstavailtest.Clear();
                    //}
                    //if (lstsmplaloca.Count > 0)
                    //{
                    //    foreach (SampleBottleAllocation objBottle in lstsmplaloca.ToList())
                    //    {
                    //        if (!string.IsNullOrEmpty(objBottle.SharedTests))
                    //        {
                    //            List<string> lstTest = objBottle.SharedTests.Split(';').ToList().Select(i => i.Trim()).ToList();
                    //            foreach (string str in lstTest.ToList())
                    //            {
                    //                if (!SRInfo.lstavailtest.Contains(str))
                    //                {
                    //                    SRInfo.lstavailtest.Add(str);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                else if (View.Id == "SampleRegistration")
                {
                    DashboardViewItem lvSampleID = ((DashboardView)Application.MainWindow.View).FindItem("SampleID") as DashboardViewItem;
                    {
                        if (lvSampleID != null && lvSampleID.InnerView != null)
                        {
                            ((ListView)lvSampleID.InnerView).CollectionSource.Criteria.Clear();
                            ((ListView)lvSampleID.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[JobID.JobID] = ? And[VisualMatrix.Oid] = ?", SRInfo.strJobID, SRInfo.visualmatrixGuid);
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
        private void PopupControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            try
            {
                if (e.PopupFrame.View.Id == "DummyClass_ListView_SampleRegistration")
                {
                    SRInfo.Ispopup = true;
                }
                if (e.PopupFrame.View.Id == "TestMethod_ListView_SamplesBA_Popup")
                {
                    SRInfo.Ispopup = true;
                }
                //if (e.PopupFrame.View.Id == "NPSampleLogIn_Bottle_DetailView")
                //{

                //    e.Width = new System.Web.UI.WebControls.Unit(600);
                //    e.Height = new System.Web.UI.WebControls.Unit(500);
                //    e.Handled = true;
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ObjectSpace_Committed(object sender, EventArgs e)
        {
            try
            {
                if (View != null && View.Id == "SampleBottleAllocation_ListView_Sampleregistration")
                {
                    WebWindow.CurrentRequestWindow.RegisterClientScript("SampleBottleAllocationRefresh", "RaiseXafCallback(globalCallbackControl, 'SamplingBottleIDPopup', 'BottleAllocationRefresh', false);");
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
                Frame.GetController<ASPxGridListEditorConfirmUnsavedChangesController>().Active["DisableUnsavedChangesController"] = false;
                if (View.Id == "SampleBottleAllocation_ListView_Sampleregistration")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (Application.MainWindow.View.Id == "SampleBottleAllocation_DetailView_SampleTransfer")
                    {
                        gridListEditor.AllowEdit = false;
                    }
                    else
                    {
                        gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                        gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                        gridListEditor.Grid.BatchUpdate += Grid_BatchUpdate;
                        gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                        gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        gridListEditor.Grid.Load += Grid_Load;
                    }

                    BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    //BottleDelcallbackManager.CallbackManager.RegisterHandler("CanDeleteTaskBottleAllocation", this);
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Visible;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 270;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.ClientInstanceName = "sampleid";
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("SamplingBottleIDPopup", this);
                    //BottleDelcallbackManager.CallbackManager.RegisterHandler("SamplingBottleSelection", this);
                    if (objPermissionInfo.SampleBottleIsRead == true)
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                        {
                           e.cancel = true;
                        }";


                    }
                    gridListEditor.Grid.JSProperties["cpVisibleRowCount"] = gridListEditor.Grid.VisibleRowCount;
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
                         var oid;
                         var text;

                         console.log('FocusedColumn:', FocusedColumn);
                      

                         if (FocusedColumn && FocusedColumn.includes('.')) 
                         {                                       
                              oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
                              text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;                                                     
                              console.log('oid:', oid);
                              console.log('text:', text);
console.log(FocusedColumn);
                               if (e.item.name == 'CopyToAllCell')
                               { if (FocusedColumn=='Containers.Oid' || FocusedColumn=='Preservative.Oid' || FocusedColumn=='StorageID.Oid'|| FocusedColumn=='StorageCondition.Oid')
	                              {
                                 
                                    for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                    { 
                                       if (s.IsRowSelectedOnPage(i))
                                       {                                               
                                         s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                       }



                                            
                                    }
                                   //console.log('CopyValue:', FocusedColumn);

                                 }
                                 
                               }        
                         } 
                         else if (FocusedColumn) 
                         {    
console.log('3');
                            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);                            
                            console.log('CopyValue:', CopyValue);

                            if (e.item.name == 'CopyToAllCell')
                             {
                                 if (FocusedColumn=='Containers.Oid' || FocusedColumn=='Preservative.Oid' || FocusedColumn=='StorageID.Oid')
	                             {
                                   for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                   { 
                                     if (s.IsRowSelectedOnPage(i)) 
                                     {
                                        s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);
                                     }
                                   }
                                 }
                                //console.log('CopyValue:', FocusedColumn);
                                
                              }                            
                          }
                    
                    }
                     e.processOnServer = false;
                     }";

                }
                if (View.Id == "DummyClass_ListView_SampleRegistration")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    BottleDelcallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    BottleDelcallbackManager.CallbackManager.RegisterHandler("SampleDummyClass", this);
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = @"function(s,e) { 
                      if (e.visibleIndex != '-1')
                      {
                         s.GetRowValues(e.visibleIndex, 'Oid', function(Oidvalue) {      
                         if (s.IsRowSelectedOnPage(e.visibleIndex)) {                             
                            RaiseXafCallback(globalCallbackControl, 'SampleDummyClass', 'Selected|' + Oidvalue , '', false);    
                         }else{
                            RaiseXafCallback(globalCallbackControl, 'SampleDummyClass', 'UNSelected|' + Oidvalue, '', false);    
                         }
                        }); 
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == s.cpVisibleRowCount)
                      {        
                        RaiseXafCallback(globalCallbackControl, 'SampleDummyClass', 'Selectall', '', false);     
                      }
                      else if(e.visibleIndex == '-1' && s.GetSelectedRowCount() == 0)
                      {
                        RaiseXafCallback(globalCallbackControl, 'SampleDummyClass', 'UNSelectall', '', false);                        
                      }                      
                    }";
                    Modules.BusinessObjects.SampleManagement.SampleLogIn objsampling = ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[Oid] = ?", SRInfo.SamplingGuid));
                    if (SRInfo.Ispopup == true && objsampling != null)
                    {
                        int strbottleid = 0;
                        List<string> lstbtlid = new List<string>();
                        SRInfo.lstbottleid = new List<DummyClass>();
                        int strbottleqty = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Modules.BusinessObjects.SampleManagement.SampleLogIn), CriteriaOperator.Parse("SUM(Qty)"), CriteriaOperator.Parse("[Oid]=?", objsampling.Oid))));
                        if (strbottleqty > 0)
                        {
                            IList<SampleBottleAllocation> lstsmplbtl = ObjectSpace.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=?", objsampling.Oid));
                            foreach (SampleBottleAllocation objsmplbtl in lstsmplbtl.ToList())
                            {
                                if (objsmplbtl != null && objsmplbtl.BottleID != null)
                                {
                                    string[] arystrbottleid = objsmplbtl.BottleID.Split(',');
                                    foreach (string strboltid in arystrbottleid)
                                    {
                                        if (!string.IsNullOrEmpty(strboltid) && !lstbtlid.Contains(strboltid.Trim()))
                                        {
                                            lstbtlid.Add(strboltid.Trim());
                                        }
                                    }
                                }
                            }
                            strbottleid = lstbtlid.Count();
                            if (strbottleid == strbottleqty)
                            {
                                const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                string valueeql = "";
                                for (int i = 0; i <= strbottleqty - 1; i++)
                                {
                                    valueeql = "";
                                    if (i >= letterseql.Length)
                                    {
                                        valueeql += letterseql[i / letterseql.Length - 1];
                                    }
                                    valueeql += letterseql[i % letterseql.Length];
                                    if (lstbtlid.Contains(valueeql))
                                    {
                                        DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                                        SRInfo.lstbottleid.Add(objdc);
                                        objdc.Name = valueeql.ToString();
                                        ((ListView)View).CollectionSource.Add(objdc);
                                    }
                                    else
                                    {
                                        strbottleqty = strbottleqty + 1;
                                        continue;
                                    }

                                }
                            }
                            else if (strbottleqty > strbottleid)
                            {
                                if (lstbtlid.Count > 0)
                                {
                                    foreach (string objsmplbtlid in lstbtlid)
                                    {
                                        DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                                        SRInfo.lstbottleid.Add(objdc);
                                        objdc.Name = objsmplbtlid.ToString();
                                        ((ListView)View).CollectionSource.Add(objdc);
                                    }
                                }
                                const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                string value = "";
                                for (int i = 0; i <= strbottleqty - lstbtlid.Count - 1; i++)
                                {
                                    value = "";
                                    if (i >= letters.Length)
                                    {
                                        value += letters[i / letters.Length - 1];
                                    }

                                    value += letters[i % letters.Length];
                                    if (!lstbtlid.Contains(value))
                                    {
                                        DummyClass objdc = ObjectSpace.CreateObject<DummyClass>();
                                        SRInfo.lstbottleid.Add(objdc);
                                        objdc.Name = value.ToString();
                                        ((ListView)View).CollectionSource.Add(objdc);
                                    }
                                    else
                                    {
                                        strbottleqty = strbottleqty + 1;
                                        continue;
                                    }
                                }
                            }
                        }
                    }

                }
                if (View.Id == "TestMethod_ListView_SamplesBA_Popup")
                {
                    if (SRInfo.Ispopup == true)
                    {
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        gridListEditor.Grid.CommandButtonInitialize += Grid_CommandButtonInitialize;
                        gridListEditor.Grid.Load += Grid_Load;
                    }
                }
                else if (View.Id == "SampleLogIn_ListView_Bottle")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 310;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    gridListEditor.Grid.Load += Grid_Load;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    ICallbackManagerHolder seltest = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    seltest.CallbackManager.RegisterHandler("Bottle", this);
                    string script = seltest.CallbackManager.GetScript();
                    script = string.Format(CultureInfo.InvariantCulture, @"
                        function(s, e) {{ 
                            var xafCallback = function() {{
                            s.EndCallback.RemoveHandler(xafCallback);
                            {0}
                            }};
                            s.EndCallback.AddHandler(xafCallback);
                        }}
                    ", script);
                    gridListEditor.Grid.ClientSideEvents.SelectionChanged = script;
                    gridListEditor.Grid.ClientSideEvents.Init = @"function(s,e)
                    { 
                    s.RowClick.ClearHandlers();
                    }";
                    gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                        { 

                        if(e.focusedColumn.fieldName != 'Qty') 
                        { 
                          e.cancel = true;  
                        }
                        else
                          {
                              var Qty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                              sessionStorage.setItem('valQty', Qty);
                              s.Qty=Qty;
                          }
                        }";
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                    gridListEditor.Grid.ClientSideEvents.BatchEditEndEditing = @"function(s, e) {
                            s.timerHandle = setTimeout(function() {  
                                 if (s.batchEditApi.HasChanges()) {  
                                   s.UpdateEdit();  
                                 } 
                                   var newQty = s.batchEditApi.GetCellValue(e.visibleIndex, 'Qty');
                                   var oldQty = sessionStorage.getItem('valQty');
                                   if(newQty!=null && oldQty!=null && oldQty > newQty)
                                      {
                                           RaiseXafCallback(globalCallbackControl,'Bottle', 'Qty|'+e.visibleIndex+'|'+oldQty+'|'+newQty, '', false);
                                      }
                               }, 20);}";

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

                    gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                                { 
var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');  
var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);     
                                if (e.item.name == 'CopyToAllCell')
                                    {
                                     if (FocusedColumn=='Qty' )
	                                   {
  
                                        for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                         { 

                                                  var oldQty = s.batchEditApi.GetCellValue(i,FocusedColumn); 
                                                  var newQty = CopyValue;
                                                  //s.batchEditApi.SetCellValue(i, FocusedColumn, CopyValue);

                                             if(newQty!=null && oldQty!=null && oldQty > newQty)
                                              {
                                                RaiseXafCallback(globalCallbackControl,'Bottle', 'QtyCopy|'+i+'|'+oldQty+'|'+CopyValue+'|'+'qtylow', '', false);
                                              }
                                              else
                                               {
                                                 RaiseXafCallback(globalCallbackControl,'Bottle', 'QtyCopy|'+i+'|'+oldQty+'|'+CopyValue+'|'+'qtyhigh', '', false);
                                                }
                                         }
							         }
                                     } 
                                 }";
                    if (!((ListView)View).CollectionSource.Criteria.ContainsKey("matrix"))
                    {
                        ((ListView)View).CollectionSource.Criteria["matrix"] = CriteriaOperator.Parse("[JobID.JobID] = ?", SRInfo.strJobID);
                    }
                }
                if (View.Id == "NPSampleLogIn_Bottle_DetailView")
                {
                    foreach (ViewItem item in ((DetailView)View).Items)
                    {
                        if (item.GetType() == typeof(ASPxCheckedLookupStringPropertyEditor))
                        {
                            ASPxCheckedLookupStringPropertyEditor propertyEditor = item as ASPxCheckedLookupStringPropertyEditor;
                            if (propertyEditor != null && propertyEditor.AllowEdit && propertyEditor.Editor != null)
                            {
                                //  propertyEditor.Editor.BackColor = Color.LightYellow;
                            }
                            ASPxGridLookup lookup = (ASPxGridLookup)propertyEditor.Editor;
                            if (lookup != null)
                            {
                                lookup.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                                lookup.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                                lookup.GridView.Settings.VerticalScrollableHeight = 200;
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

        private void Grid_BatchUpdate(object sender, DevExpress.Web.Data.ASPxDataBatchUpdateEventArgs e)
        {
            try
            {
                foreach (var args in e.UpdateValues)
                {
                    UpdateItem(args.Keys, args.NewValues);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        protected void UpdateItem(OrderedDictionary keys, OrderedDictionary newValues)
        {
            try
            {
                var id = Convert.ToString(keys["Oid"]);
                var val = Convert.ToString(newValues["NPBottleID"]);
                if (val.Contains(", "))
                {
                    SampleBottleAllocation sample = ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().Where(a => a.Oid == new Guid(id)).First();
                    if (sample != null)
                    {
                        using (IObjectSpace os = Application.CreateObjectSpace())
                        {
                            IList<SampleBottleAllocation> samples = os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [TestMethod.Oid]=? And [Oid]<>?", sample.SampleRegistration.Oid, sample.TestMethod.Oid, new Guid(id))).ToList();
                            foreach (SampleBottleAllocation allocation in samples)
                            {
                                allocation.StorageID = newValues["StorageID.Oid"] != null ? os.GetObjectByKey<Storage>(new Guid(newValues["StorageID.Oid"].ToString())) : null;
                                allocation.Containers = newValues["Containers.Oid"] != null ? os.GetObjectByKey<Container>(new Guid(newValues["Containers.Oid"].ToString())) : null;
                                allocation.Preservative = newValues["Preservative.Oid"] != null ? os.GetObjectByKey<Preservative>(new Guid(newValues["Preservative.Oid"].ToString())) : null;
                                allocation.StorageCondition = newValues["StorageCondition.Oid"] != null ? os.GetObjectByKey<PreserveCondition>(new Guid(newValues["StorageCondition.Oid"].ToString())) : null;
                            }
                            os.CommitChanges();
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

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "SampleLogIn_ListView_Bottle")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                        if (cv != null)
                        {
                            DashboardViewItem dvBottleAllocation = ((DetailView)cv).FindItem("SampleBottleAllocation_SampleRegistration") as DashboardViewItem;
                            if (dvBottleAllocation != null && dvBottleAllocation.InnerView != null)
                            {
                                if ((Modules.BusinessObjects.SampleManagement.SampleLogIn)View.CurrentObject != null)
                                {
                                    Modules.BusinessObjects.SampleManagement.SampleLogIn logIn = (Modules.BusinessObjects.SampleManagement.SampleLogIn)View.CurrentObject;
                                    if (logIn != null)
                                    {
                                        ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria.Clear();
                                        List<object> OidTask = new List<object>();
                                        using (XPView lstview = new XPView(((XPObjectSpace)this.ObjectSpace).Session, typeof(SampleBottleAllocation)))
                                        {
                                            lstview.Criteria = CriteriaOperator.Parse("[SampleRegistration.Oid] = ?", logIn.Oid);
                                            lstview.Properties.Add(new ViewProperty("group", SortDirection.Ascending, "TestMethod.Oid", true, true));
                                            lstview.Properties.Add(new ViewProperty("Toid", SortDirection.Ascending, "MAX(Oid)", false, true));
                                            foreach (ViewRecord Vrec in lstview)
                                                OidTask.Add(Vrec["Toid"]);
                                        }
                                        ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria["filter"] = new InOperator("Oid", OidTask);
                                        //((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[SampleRegistration.Oid] = ?", logIn.Oid);
                                        ((ListView)dvBottleAllocation.InnerView).Refresh();
                                        if (SRInfo.counter == 0)
                                        {
                                            SRInfo.SamplingGuid = logIn.Oid;
                                            SRInfo.strselSample = logIn.Oid.ToString();
                                        }
                                    }
                                }
                                else if (View.SelectedObjects.Count == 0 || (View.SelectedObjects.Count == gridListEditor.Grid.VisibleRowCount && gridListEditor.Grid.VisibleRowCount != 2))
                                {
                                    ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria.Clear();
                                    ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] is null");
                                    ((ListView)dvBottleAllocation.InnerView).Refresh();
                                    SRInfo.SamplingGuid = new Guid();
                                    SRInfo.strselSample = null;
                                    SRInfo.counter = 0;
                                    SRInfo.CanProcess = null;
                                }
                                else if (SRInfo.CanProcess == true)
                                {
                                    if (SRInfo.counter == 0)
                                    {
                                        SRInfo.counter = 1;
                                    }
                                    else
                                    {
                                        SRInfo.counter = 0;
                                        SRInfo.CanProcess = false;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(SRInfo.strselSample);
                                    }
                                }
                                else if (SRInfo.CanProcess == false)
                                {
                                    Modules.BusinessObjects.SampleManagement.SampleLogIn logIn = View.SelectedObjects.Cast<Modules.BusinessObjects.SampleManagement.SampleLogIn>().Where(a => a.Oid.ToString() != SRInfo.strselSample).FirstOrDefault();
                                    if (logIn != null)
                                    {
                                        SRInfo.CanProcess = true;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(logIn.Oid);
                                    }
                                }
                                else if (SRInfo.CanProcess == null && SRInfo.strselSample != null && (Modules.BusinessObjects.SampleManagement.SampleLogIn)View.CurrentObject == null && View.SelectedObjects.Count > 1)
                                {
                                    if (SRInfo.counter == 0)
                                    {
                                        SRInfo.counter = 1;
                                    }
                                    else
                                    {
                                        SRInfo.counter = 0;
                                        SRInfo.CanProcess = true;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(SRInfo.strselSample);
                                    }
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

        private void Grid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (e.ButtonType == ColumnCommandButtonType.SelectCheckbox)
                {
                    var curOid = gridView.GetRowValues(e.VisibleIndex, "TestName");
                    if (curOid != null)
                    {
                        if (SRInfo.lstavailtest.Contains(curOid.ToString().Trim()))
                        {
                            e.Visible = false;
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

        private void Grid_Load(object sender, EventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                if (View.Id == "DummyClass_ListView_SampleRegistration")
                {
                    if (SRInfo.lstcrtbottleid != null && SRInfo.lstcrtbottleid.Count > 0)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "Name").ToString()))
                            {
                                string strbottleid = gridView.GetRowValues(i, "Name").ToString();
                                if (SRInfo.lstcrtbottleid.Contains(strbottleid))
                                {
                                    gridView.Selection.SelectRow(i);
                                }
                            }
                        }
                    }
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                    SRInfo.lstcrtbottleid = new List<string>();
                    SRInfo.Ispopup = false;
                }
                if (View.Id == "TestMethod_ListView_SamplesBA_Popup" && SRInfo.Ispopup == true)
                {
                    SRInfo.Ispopup = false;
                    if (SRInfo.lstcrttests != null && SRInfo.lstcrttests.Count > 0)
                    {
                        List<Guid> lstTest = ObjectSpace.GetObjects<TestMethod>(new InOperator("TestName", SRInfo.lstcrttests)).Select(i => i.Oid).ToList();
                        ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                        if (gridListEditor != null && gridListEditor.Grid != null && lstTest.Count > 0)
                        {
                            foreach (Guid obj in lstTest.ToList())
                            {
                                gridListEditor.Grid.Selection.SelectRowByKey(obj);
                            }
                        }
                    }
                    if (SRInfo.lstavailtest.Count > 0)
                    {
                        var selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(i => i.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
                        }
                    }
                }
                if (View.Id == "TestMethod_ListView_Samples_BA")
                {
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }
                if (View.Id == "SampleBottleAllocation_ListView_Sampleregistration")
                {
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }
                if (View.Id == "SampleLogIn_ListView_SampleRegistration_SeletedSampleID")
                {
                    for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                    {
                        gridView.Selection.SelectRow(i);
                    }
                }
                else if (View.Id == "SampleLogIn_ListView_Bottle")
                {
                    if (gridView != null)
                    {
                        GridViewCommandColumn selectionBoxColumn = gridView.Columns.OfType<GridViewCommandColumn>().Where(x => x.ShowSelectCheckbox).FirstOrDefault();
                        if (selectionBoxColumn != null)
                        {
                            selectionBoxColumn.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.None;
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
                if (objPermissionInfo.SampleBottleIsRead == false)
                {
                    if (e.DataColumn.FieldName == "NPBottleID")
                    {
                        e.Cell.Attributes.Add("onclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'SamplingBottleIDPopup', '{0}|{1}' , '', false)", e.DataColumn.FieldName, e.VisibleIndex));
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
                //if (View.Id == "SampleBottleAllocation_DetailView_SampleRegistration")
                //{
                //    SRInfo.strJobID = null;
                //}
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                ObjectSpace.Committed -= ObjectSpace_Committed;
                if (View.Id == "DummyClass_ListView_SampleRegistration")
                {
                    SRInfo.selectionhideGuid.Clear();
                    SRInfo.lstviewselected.Clear();
                }
                if (View.Id == "SampleBottleAllocation_ListView_Sampleregistration")
                {
                    SRInfo.lstsmplbtlallo.Clear();
                }
                if (View.Id == "TestMethod_ListView_SamplesBA_Popup")
                {
                    SRInfo.lstdummytests.Clear();
                    SRInfo.lstdummycreationtests.Clear();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void samplegridrefresh()
        {
            CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
            if (cv != null)
            {
                SampleBottleAllocation objsampleallocation1 = cv.CurrentObject as SampleBottleAllocation;

                DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleIDs") as DashboardViewItem;
                if (dvsampleids != null && dvsampleids.InnerView != null)
                {
                    dvsampleids.InnerView.ObjectSpace.Refresh();
                }
                objsampleallocation1.Qty = 1;
            }
        }

        private void Refresh()
        {
            DashboardViewItem dvsampleidss = ((DetailView)View).FindItem("SampleBottleAllocation_SampleRegistration") as DashboardViewItem;

            DashboardViewItem dvsampleids = ((DetailView)View).FindItem("SampleIDs") as DashboardViewItem;
            if (dvsampleids != null && dvsampleids.InnerView != null)
            {
                dvsampleids.InnerView.ObjectSpace.Refresh();

            }
            if (dvsampleidss != null && dvsampleidss.InnerView != null)
            {
                dvsampleidss.InnerView.ObjectSpace.Refresh();

            }
        }
        private void btnAssignbottles_BottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (SRInfo.SamplingGuid != null)
                {
                    int i = 65;
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<SampleBottleAllocation> lstcurrentBottle = uow.Query<SampleBottleAllocation>().Where(a => a.SampleRegistration != null && a.SampleRegistration.Oid == SRInfo.SamplingGuid).ToList();
                    if (lstcurrentBottle.Where(a => a.SignOffBy != null).ToList().Count > 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Bottle cannot be assigned", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }

                    lstcurrentBottle.ForEach(a => a.BottleID = string.Empty);
                    if (lstcurrentBottle.Count > 0)
                    {
                        Guid matrix = lstcurrentBottle.Select(a => a.SampleRegistration.VisualMatrix.Oid).FirstOrDefault();
                        IList<string> Testguids = lstcurrentBottle.Select(a => a.TestMethod.Oid.ToString()).ToList();
                        List<BottleSharing> lstBottlesharing = uow.Query<BottleSharing>().Where(a => a.SampleMatrix.Oid == matrix).ToList();
                        foreach (BottleSharing bottleSharing in lstBottlesharing)
                        {
                            string[] sharingtest = bottleSharing.Tests.Split(new string[] { "; " }, StringSplitOptions.None);
                            if (sharingtest.Count(a => Testguids.Contains(a)) > 1)
                            {
                                foreach (SampleBottleAllocation sample in lstcurrentBottle.Where(a => sharingtest.Contains(a.TestMethod.Oid.ToString())).ToList())
                                {
                                    if (sample.BottleID == string.Empty)
                                    {
                                        sample.BottleID = ((char)i).ToString();
                                    }
                                    else
                                    {
                                        //sample.BottleID += ", " + ((char)i).ToString();
                                        SampleBottleAllocation objbottle = uow.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid] = ? and [TestMethod.Oid] = ? And [BottleID]=?", sample.SampleRegistration.Oid, sample.TestMethod.Oid, ((char)i).ToString(), true));
                                        if (objbottle == null)
                                        {
                                            SampleBottleAllocation objNewBottle = new SampleBottleAllocation(uow);
                                            objNewBottle.BottleID = ((char)i).ToString();
                                            objNewBottle.SampleRegistration = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(sample.SampleRegistration.Oid);
                                            objNewBottle.TestMethod = uow.GetObjectByKey<TestMethod>(sample.TestMethod.Oid);
                                            if (sample.Containers != null)
                                            {
                                                objNewBottle.Containers = uow.GetObjectByKey<Container>(sample.Containers.Oid);
                                            }
                                            if (sample.Preservative != null)
                                            {
                                                objNewBottle.Preservative = uow.GetObjectByKey<Preservative>(sample.Preservative.Oid);
                                            }
                                            if (sample.StorageID != null)
                                            {
                                                objNewBottle.StorageID = uow.GetObjectByKey<Storage>(sample.StorageID.Oid);
                                            }
                                            if (sample.StorageCondition != null)
                                            {
                                                objNewBottle.StorageCondition = uow.GetObjectByKey<PreserveCondition>(sample.StorageCondition.Oid);
                                            }
                                        }
                                    }
                                    if (sample.SampleRegistration.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                                    {
                                        Frame.GetController<AuditlogViewController>().insertauditdata(uow, sample.SampleRegistration.JobID.Oid, OperationType.ValueChanged, "SampleBottleAllocation", sample.SampleRegistration.SampleID, "Assignbottles", "", sample.BottleID, "");
                                    }
                                }
                            }
                            i++;
                        }
                        IList<SampleBottleAllocation> EBottle = lstcurrentBottle.Where(a => string.IsNullOrEmpty(a.BottleID)).ToList();
                        List<string> ETestGuid =  EBottle.Select(a => a.TestMethod.Oid.ToString()).Distinct().ToList();
                        int intmax = 0;
                        if (EBottle.Count > 0)
                        {
                            foreach (string Etest in ETestGuid)
                            {
                                int j = 65;
                                foreach (SampleBottleAllocation sample in EBottle.Where(a => a.TestMethod.Oid.ToString() == Etest).ToList())
                                {
                                    sample.BottleID = ((char)j).ToString();
                                    j++;
                                }
                                int currentqty = j - 65; ;
                                if (intmax < currentqty)
                                {
                                    intmax =currentqty;
                                }
                                
                            }
                                //foreach (SampleBottleAllocation bottle in EBottle)
                            //{
                            //    i = 65;
                            //    bottle.BottleID = ((char)i).ToString();
                            //    if (bottle.SampleRegistration.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                            //    {
                            //        Frame.GetController<AuditlogViewController>().insertauditdata(uow, bottle.SampleRegistration.JobID.Oid, OperationType.ValueChanged, "SampleBottleAllocation", bottle.SampleRegistration.SampleID, "Assignbottles", "", bottle.BottleID, "");
                            //    }
                                
                            //}
                        }
                        else
                        {
                            i--;
                        }
                        if (intmax < (i - 64))
                            lstcurrentBottle.FirstOrDefault().SampleRegistration.Qty = (uint)(i - 64);
else
                            lstcurrentBottle.FirstOrDefault().SampleRegistration.Qty = (uint)(intmax - 64);
                        uow.CommitChanges();
                        View.ObjectSpace.Refresh();
                        samplegridrefresh();
                        Application.ShowViewStrategy.ShowMessage("Bottle assigned successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select the sample id", InformationType.Error, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btnResetbottles_BottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                List<SampleBottleAllocation> lstBottle = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.JobID.JobID == SRInfo.strJobID && i.SignOffBy == null).ToList();
                foreach (var sample in lstBottle.GroupBy(a => new { a.SampleRegistration, a.TestMethod.Oid }))
                {
                    IList<SampleBottleAllocation> bottles = lstBottle.Where(a => a.TestMethod.Oid == sample.Key.Oid && a.SampleRegistration.Oid == sample.Key.SampleRegistration.Oid).OrderBy(a => a.BottleID).ToList();
                    if (bottles.Count == 1)
                    {
                        bottles[0].BottleID = "A";
                        bottles[0].SampleRegistration.Qty = 1;
                        bottles[0].Containers = null;
                        bottles[0].Preservative = null;
                        bottles[0].StorageID = null;
                        bottles[0].StorageCondition = null;
                    }
                    else if (bottles.Count > 1)
                    {
                        foreach (SampleBottleAllocation allocation in bottles.ToList())
                        {
                            if (allocation == bottles.First())
                            {
                                allocation.BottleID = "A";
                                allocation.SampleRegistration.Qty = 1;
                                allocation.Containers = null;
                                allocation.Preservative = null;
                                allocation.StorageID = null;
                                allocation.StorageCondition = null;
                            }
                            else
                {
                                uow.Delete(allocation);
                            }
                        }
                    }
                    if (bottles.Count > 0 && bottles[0].SampleRegistration.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                    {
                        Frame.GetController<AuditlogViewController>().insertauditdata(uow, bottles[0].SampleRegistration.JobID.Oid, OperationType.ValueChanged, "SampleBottleAllocation", bottles[0].SampleRegistration.SampleID, "Resetbottles", "", "A", "");
                    }
                }
                uow.CommitChanges();
                View.ObjectSpace.Refresh();
                samplegridrefresh();
                Application.ShowViewStrategy.ShowMessage("Bottles reset successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void QtyReset_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)

                {

                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<SampleBottleAllocation> lstBottle = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.JobID.JobID == SRInfo.strJobID && i.SignOffBy == null).ToList();
                    SampleBottleAllocation objsampleallocation = View.CurrentObject as SampleBottleAllocation;
                    foreach (var sample in lstBottle.GroupBy(a => new { a.SampleRegistration, a.TestMethod.Oid }))
                    {
                        IList<SampleBottleAllocation> bottles = lstBottle.Where(a => a.TestMethod.Oid == sample.Key.Oid && a.SampleRegistration.Oid == sample.Key.SampleRegistration.Oid).OrderBy(a => a.BottleID).ToList();
                        if (bottles.Count == 1)
                        {
                            bottles[0].BottleID = "A";
                            bottles[0].SampleRegistration.Qty = 1;
                            bottles[0].Containers = null;
                            bottles[0].Preservative = null;
                            bottles[0].StorageID = null;
                            bottles[0].StorageCondition = null;
                        }
                        else if (bottles.Count > 1)
                        {
                            foreach (SampleBottleAllocation allocation in bottles.ToList())
                            {
                                if (allocation == bottles.First())
                                {
                                    allocation.BottleID = "A";
                                    allocation.SampleRegistration.Qty = 1;
                                    allocation.Containers = null;
                                    allocation.Preservative = null;
                                    allocation.StorageID = null;
                                    allocation.StorageCondition = null;
                                }
                                else
                    {
                                    uow.Delete(allocation);
                                }
                            }
                        }
                    }
                    objsampleallocation.Qty = 1;
                    uow.CommitChanges();
                    Refresh();
                    View.Refresh();

                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void QtyOk_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)

                {

                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<SampleBottleAllocation> lstBottle = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.JobID.JobID == SRInfo.strJobID && i.SignOffBy == null).ToList();
                    SampleBottleAllocation objsampleallocation = View.CurrentObject as SampleBottleAllocation;
                    if (objsampleallocation.Qty > 0)
                    {
                        foreach (var sample in lstBottle.GroupBy(a => new { a.SampleRegistration, a.TestMethod.Oid }))
                    {
                            IList<SampleBottleAllocation> bottles = lstBottle.Where(a => a.TestMethod.Oid == sample.Key.Oid && a.SampleRegistration.Oid == sample.Key.SampleRegistration.Oid).OrderBy(a => a.BottleID).ToList();
                            if (bottles.Count == 1)
                            {
                                bottles[0].BottleID = "A";
                                bottles[0].SampleRegistration.Qty = objsampleallocation.Qty;
                                bottles[0].Containers = null;
                                bottles[0].Preservative = null;
                                bottles[0].StorageID = null;
                                bottles[0].StorageCondition = null;
                            }
                            else if (bottles.Count > 1)
                            {
                                foreach (SampleBottleAllocation allocation in bottles.ToList())
                                {
                                    if (allocation == bottles.First())
                                    {
                                        allocation.BottleID = "A";
                                        allocation.SampleRegistration.Qty = objsampleallocation.Qty;
                                        allocation.Containers = null;
                                        allocation.Preservative = null;
                                        allocation.StorageID = null;
                                        allocation.StorageCondition = null;
                                    }
                                    else
                        {
                                        uow.Delete(allocation);
                                    }
                                }
                            }
                        }
                    }
                    else if(objsampleallocation.Qty < 1)
                    {
                        Application.ShowViewStrategy.ShowMessage("This default container qty must greater than 0", InformationType.Error, timer.Seconds, InformationPosition.Top);

                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("This default container qty must not be empty", InformationType.Error, timer.Seconds, InformationPosition.Top);

                    }

                    uow.CommitChanges();
                    Refresh();
                    View.Refresh();

                }

            }
            catch (Exception ex)
            {

                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void btnCopybottles_BottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                if (cv != null)
                {
                    DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleIDs") as DashboardViewItem;
                    if (dvsampleids != null && dvsampleids.InnerView != null)
                    {
                        if (((ListView)dvsampleids.InnerView).CollectionSource.GetCount() > 1)
                        {
                            IObjectSpace os = Application.CreateObjectSpace();
                            NPSampleLogIn_Bottle btle = os.CreateObject<NPSampleLogIn_Bottle>();
                            DetailView dv = Application.CreateDetailView(os, btle);
                            dv.ViewEditMode = ViewEditMode.Edit;
                            ShowViewParameters showViewParameters = new ShowViewParameters(dv);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "Copy Bottle Allocation";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += CopyToSampleID_AcceptAction_Execute;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                            SRInfo.Ispopup = false;
                        }
                        else
                        {
                            Application.ShowViewStrategy.ShowMessage("Add atleast two sampleID's", InformationType.Warning, timer.Seconds, InformationPosition.Top);
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

        private void CopyToSampleID_AcceptAction_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    DialogController dialog = (DialogController)sender;
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    NPSampleLogIn_Bottle logIn_Bottle = (NPSampleLogIn_Bottle)dialog.Window.View.CurrentObject;
                    if(logIn_Bottle.From==null)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select the from.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                    if (logIn_Bottle.From == null)
                    {
                        Application.ShowViewStrategy.ShowMessage("Select the To.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                        return;
                    }
                    if (logIn_Bottle != null && !string.IsNullOrEmpty(logIn_Bottle.To))
                    {
                        List<SampleBottleAllocation> lstFromBottle = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.Oid == logIn_Bottle.From.Oid).ToList();
                        foreach (string oid in logIn_Bottle.To.Split(new string[] { "; " }, StringSplitOptions.None))
                        {
                            foreach (SampleBottleAllocation CurFromBottle in lstFromBottle.GroupBy(a => a.TestMethod.Oid).Select(a => a.First()).ToList())
                            {
                                IList<SampleBottleAllocation> objSamplebottle = uow.Query<SampleBottleAllocation>().Where(a => a.SampleRegistration.Oid == new Guid(oid) && a.TestMethod.Oid == CurFromBottle.TestMethod.Oid).OrderBy(a => a.BottleID).ToList();
                                if (objSamplebottle.Count > 0)
                            {
                                    if (objSamplebottle.Count > 1)
                                    {
                                        foreach (SampleBottleAllocation sampleBottle in objSamplebottle)
                                {
                                            if (sampleBottle != objSamplebottle.First())
                                            {
                                                uow.Delete(sampleBottle);
                                            }
                                        }
                                    }
                                    IList<SampleBottleAllocation> bottles = lstFromBottle.Where(a => a.TestMethod.Oid == CurFromBottle.TestMethod.Oid).OrderBy(a => a.BottleID).ToList();
                                    if (bottles.Count == 1)
                                    {
                                        objSamplebottle[0].BottleID = CurFromBottle.BottleID;
                                        if (CurFromBottle.Containers != null)
                                        {
                                            objSamplebottle[0].Containers = uow.GetObjectByKey<Container>(CurFromBottle.Containers.Oid);
                                        }
                                        if (CurFromBottle.Preservative != null)
                                        {
                                            objSamplebottle[0].Preservative = uow.GetObjectByKey<Preservative>(CurFromBottle.Preservative.Oid);
                                        }
                                        if (CurFromBottle.StorageID != null)
                                        {
                                            objSamplebottle[0].StorageID = uow.GetObjectByKey<Storage>(CurFromBottle.StorageID.Oid);
                                        }
                                        if (CurFromBottle.StorageCondition != null)
                                        {
                                            objSamplebottle[0].StorageCondition = uow.GetObjectByKey<PreserveCondition>(CurFromBottle.StorageCondition.Oid);
                                        }
                                        objSamplebottle[0].SampleRegistration.Qty = CurFromBottle.SampleRegistration.Qty;

                                    }
                                    else if (bottles.Count > 1)
                                    {
                                        foreach (SampleBottleAllocation bottle in bottles)
                                        {
                                            if (bottles.First() == bottle)
                                            {
                                                objSamplebottle[0].BottleID = bottle.BottleID;
                                                if (CurFromBottle.Containers != null)
                                                {
                                                    objSamplebottle[0].Containers = uow.GetObjectByKey<Container>(CurFromBottle.Containers.Oid);
                                                }
                                                if (CurFromBottle.Preservative != null)
                                    {
                                                    objSamplebottle[0].Preservative = uow.GetObjectByKey<Preservative>(CurFromBottle.Preservative.Oid);
                                                }
                                                if (CurFromBottle.StorageID != null)
                                                {
                                                    objSamplebottle[0].StorageID = uow.GetObjectByKey<Storage>(CurFromBottle.StorageID.Oid);
                                                }
                                                if (CurFromBottle.StorageCondition != null)
                                                {
                                                    objSamplebottle[0].StorageCondition = uow.GetObjectByKey<PreserveCondition>(CurFromBottle.StorageCondition.Oid);
                                                }
                                    }
                                            else
                                            {
                                                SampleBottleAllocation objbottle = uow.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid] = ? and [TestMethod.Oid] = ? And [BottleID]=?", objSamplebottle[0].SampleRegistration.Oid, objSamplebottle[0].TestMethod.Oid, bottle.BottleID, true));
                                                if (objbottle == null)
                                    {
                                                    SampleBottleAllocation objNewBottle = new SampleBottleAllocation(uow);
                                                    objNewBottle.BottleID = bottle.BottleID;
                                                    objNewBottle.SampleRegistration = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objSamplebottle[0].SampleRegistration.Oid);
                                                    objNewBottle.TestMethod = uow.GetObjectByKey<TestMethod>(objSamplebottle[0].TestMethod.Oid);
                                                    if (CurFromBottle.Containers != null)
                                                    {
                                                        objNewBottle.Containers = uow.GetObjectByKey<Container>(CurFromBottle.Containers.Oid);
                                    }
                                                    if (CurFromBottle.Preservative != null)
                                    {
                                                        objNewBottle.Preservative = uow.GetObjectByKey<Preservative>(CurFromBottle.Preservative.Oid);
                                    }
                                                    if (CurFromBottle.StorageID != null)
                                    {
                                                        objNewBottle.StorageID = uow.GetObjectByKey<Storage>(CurFromBottle.StorageID.Oid);
                                    }
                                                    if (CurFromBottle.StorageCondition != null)
                                    {
                                                        objNewBottle.StorageCondition = uow.GetObjectByKey<PreserveCondition>(CurFromBottle.StorageCondition.Oid);
                                                    }
                                                }
                                            }
                                        }
                                        objSamplebottle[0].SampleRegistration.Qty = CurFromBottle.SampleRegistration.Qty;
                                    }
                                }
                            }
                        }
                        uow.CommitChanges();
                        samplegridrefresh();
                        Application.ShowViewStrategy.ShowMessage("Bottle set copied successfully.", InformationType.Success, timer.Seconds, InformationPosition.Top);
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
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                string[] samplesplit = parameter.Split('|');
                if (SRInfo.SampleIDGuid == null)
                {
                    SRInfo.SampleIDGuid = Guid.Empty;
                }
                if (gridListEditor != null && parameter != null && View.Id == "SampleBottleAllocation_ListView_Sampleregistration")
                {
                    if (objPermissionInfo.SampleBottleIsRead == false)
                    {
                        if (samplesplit[0] == "NPBottleID")
                        {
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                            if (HttpContext.Current.Session["rowid"] != null)
                            {
                                SRInfo.lstcrtbottleid = new List<string>();
                                SampleBottleAllocation objsampling = ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                                if (objsampling != null && objsampling.BottleID != null)
                                {
                                    string[] strbottleid = objsampling.NPBottleID.Split(',');
                                    foreach (var strbotid in strbottleid)
                                    {
                                        SRInfo.lstcrtbottleid.Add(strbotid.Trim());
                                    }
                                }
                            }
                            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(DummyClass));
                            ListView lv = Application.CreateListView("DummyClass_ListView_SampleRegistration", cs, false);
                            ShowViewParameters showViewParameters = new ShowViewParameters(lv);
                            showViewParameters.Context = TemplateContext.PopupWindow;
                            showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                            showViewParameters.CreatedView.Caption = "BottleID";
                            DialogController dc = Application.CreateController<DialogController>();
                            dc.SaveOnAccept = false;
                            dc.CloseOnCurrentObjectProcessing = false;
                            dc.Accepting += AcceptAction_Accepting;
                            showViewParameters.Controllers.Add(dc);
                            Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
                        }
                    }
                }
                else if (gridListEditor != null && parameter != null && View.Id == "DummyClass_ListView_SampleRegistration")
                {
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        if (parameter == "Selectall")
                        {
                            if (gridListEditor.Grid.VisibleRowCount == gridListEditor.Grid.Selection.Count)
                            {
                                foreach (DummyClass objdc in ((ListView)View).CollectionSource.List.Cast<DummyClass>().ToList())
                                {
                                    if (!SRInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                    {
                                        if (!SRInfo.lstviewselected.Contains(objdc))
                                        {
                                            SRInfo.lstviewselected.Add(objdc);
                                        }
                                    }
                                }
                            }
                        }
                        else if (parameter == "UNSelectall")
                        {
                            SRInfo.lstviewselected.Clear();
                        }
                        else
                        {
                            string[] splparm = parameter.Split('|');
                            if (!string.IsNullOrEmpty(splparm[1]))
                            {
                                Guid objguid = new Guid(splparm[1]);
                                if (splparm[0] == "Selected")
                                {
                                    foreach (DummyClass objdc in View.SelectedObjects)
                                    {
                                        if (!SRInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                        {
                                            if (!SRInfo.lstviewselected.Contains(objdc))
                                            {
                                                SRInfo.lstviewselected.Add(objdc);
                                            }
                                        }
                                    }
                                }
                                else if (splparm[0] == "UNSelected")
                                {
                                    SRInfo.lstviewselected.Clear();
                                    foreach (DummyClass objdc in View.SelectedObjects)
                                    {
                                        if (!SRInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                        {
                                            if (!SRInfo.lstviewselected.Contains(objdc))
                                            {
                                                SRInfo.lstviewselected.Add(objdc);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                       ((ListView)View).Refresh();
                }
                else if (gridListEditor != null && parameter != null && View.Id == "TestMethod_ListView_SamplesBA_Popup")
                {
                    if (gridListEditor != null && gridListEditor.Grid != null && parameter != "true" && parameter != "false")
                    {
                    }
                    else if (bool.TryParse(parameter, out bool CanDeleteTaskBottleAllocation))
                    {
                        if (CanDeleteTaskBottleAllocation)
                        {
                            List<object> lstsharedtest = new List<object>();
                            string sharedtest = string.Empty;
                            foreach (TestMethod objdeltest in View.SelectedObjects)
                            {
                                if (string.IsNullOrEmpty(sharedtest))
                                {
                                    sharedtest = objdeltest.Oid.ToString();
                                }
                                else
                                {
                                    sharedtest = sharedtest + "| " + objdeltest.Oid.ToString();
                                }
                                lstsharedtest.Add(objdeltest.Oid);
                            }
                            if (!string.IsNullOrEmpty(sharedtest))
                            {
                                SampleBottleAllocation objsmp = ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (objsmp != null)
                                {
                                    ObjectSpace.CommitChanges();
                                    ObjectSpace.Refresh();
                                }
                            }
                            else
                            {
                                SampleBottleAllocation objsmp = ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid] = ?", new Guid(HttpContext.Current.Session["rowid"].ToString())));
                                if (objsmp != null)
                                {
                                    ObjectSpace.Delete(objsmp);
                                    ObjectSpace.CommitChanges();
                                    ObjectSpace.Refresh();
                                }
                            }
                            if (lstsharedtest != null && lstsharedtest.Count > 0)
                            {
                                ((ListView)View).CollectionSource.Criteria.Clear();
                                ((ListView)View).CollectionSource.Criteria["filter"] = new InOperator("Oid", lstsharedtest);
                            }
                            else
                            {
                                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                            }
                            Application.ShowViewStrategy.ShowMessage("Remove successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
                        }
                        else
                        {

                        }
                    }
                }
                else if (gridListEditor != null && parameter != null && View.Id == "SampleLogIn_ListView_Bottle")
                {
                    if (samplesplit[0] == "Qty")
                    {
                        HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            Modules.BusinessObjects.SampleManagement.SampleLogIn objsample = ((ListView)View).CollectionSource.List.Cast<Modules.BusinessObjects.SampleManagement.SampleLogIn>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                            if (objsample != null)
                            {
                                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                                Session currentSession = ((XPObjectSpace)(((ListView)View).ObjectSpace)).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                if (objsample.Qty <= 0)
                                {
                                    //objsample.Qty = Convert.ToUInt16(samplesplit[2]);
                                    objsample.Qty = Convert.ToUInt16(samplesplit[2]) == 0 ? objsample.Qty : Convert.ToUInt16(samplesplit[2]);

                                    //Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objsample.Oid);
                                    //objSampleLogin.Qty = Convert.ToUInt16(samplesplit[2]);                                    
                                    //((ListView)View).ObjectSpace.CommitChanges();
                                    View.ObjectSpace.CommitChanges();
                                    View.ObjectSpace.Refresh();
                                    //((ListView)View).Refresh();
                                    Application.ShowViewStrategy.ShowMessage("Qty must be greater than 0", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                                else
                                {
                                    List<string> lstqtyBottle = new List<string>();
                                    const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                    string value = "";
                                    for (int i = 0; i <= objsample.Qty - 1; i++)
                                    {
                                        value = "";
                                        if (i >= letterseql.Length)
                                        {
                                            value += letterseql[i / letterseql.Length - 1];
                                        }

                                        value += letterseql[i % letterseql.Length];
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            lstqtyBottle.Add(value);
                                        }
                                    }
                                    List<string> lstAssignedBttle = new List<string>();
                                    List<SampleBottleAllocation> lstBottle = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.Oid == objsample.Oid && i.SignOffBy == null).ToList();
                                    foreach (SampleBottleAllocation sample in lstBottle)
                                    {
                                        List<string> lstBottles = sample.BottleID.Split(',').ToList().Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                        lstAssignedBttle.AddRange(lstBottles);
                                    }
                                    int qty = 0;
                                    foreach (string objLetter in lstqtyBottle.ToList())
                                    {
                                        qty = qty + lstAssignedBttle.Where(i => i == objLetter).Count();
                                    }
                                    if (qty != lstAssignedBttle.Count)
                                    {
                                        Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objsample.Oid);
                                        objSampleLogin.Qty = Convert.ToUInt16(samplesplit[2]);
                                        uow.CommitChanges();
                                        View.ObjectSpace.Refresh();
                                        Application.ShowViewStrategy.ShowMessage("The test has already been assigned to bottles; please remove the bottle ID.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                    }
                                }
                                //uow.CommitChanges();
                                //CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                                //if (cv != null)
                                //{
                                //    DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleBottleAllocation_SampleRegistration") as DashboardViewItem;
                                //    if (dvsampleids != null && dvsampleids.InnerView != null)
                                //    {
                                //        dvsampleids.InnerView.ObjectSpace.Refresh();
                                //    }
                                //}
                            }
                        }
                    }


                    if (samplesplit[0] == "QtyCopy")
                    {
                        HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            Modules.BusinessObjects.SampleManagement.SampleLogIn objsample = ((ListView)View).CollectionSource.List.Cast<Modules.BusinessObjects.SampleManagement.SampleLogIn>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                            if (objsample != null)
                            {
                                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                                Session currentSession = ((XPObjectSpace)(((ListView)View).ObjectSpace)).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                if (objsample.Qty <= 0)
                                {
                                    //objsample.Qty = Convert.ToUInt16(samplesplit[2]);
                                    objsample.Qty = Convert.ToUInt16(samplesplit[2]) == 0 ? objsample.Qty : Convert.ToUInt16(samplesplit[2]);

                                    //Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objsample.Oid);
                                    //objSampleLogin.Qty = Convert.ToUInt16(samplesplit[2]);                                    
                                    //((ListView)View).ObjectSpace.CommitChanges();
                                    View.ObjectSpace.CommitChanges();
                                    View.ObjectSpace.Refresh();
                                    //((ListView)View).Refresh();
                                    Application.ShowViewStrategy.ShowMessage("Qty must be greater than 0", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                }
                                else
                                {
                                    List<string> lstqtyBottle = new List<string>();
                                    const string letterseql = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                    string value = "";
                                    objsample.Qty = Convert.ToUInt16(samplesplit[3]);
                                    if (samplesplit[4] == "qtyhigh")
                                    {
                                        View.ObjectSpace.CommitChanges();
                                        View.ObjectSpace.Refresh();
                                    }
                                    else
                                    {
                                        for (int i = 0; i <= objsample.Qty - 1; i++)
                                        {
                                            value = "";
                                            if (i >= letterseql.Length)
                                            {
                                                value += letterseql[i / letterseql.Length - 1];
                                            }

                                            value += letterseql[i % letterseql.Length];
                                            if (!string.IsNullOrEmpty(value))
                                            {
                                                lstqtyBottle.Add(value);
                                            }
                                        }
                                        List<string> lstAssignedBttle = new List<string>();
                                        List<SampleBottleAllocation> lstBottle = uow.Query<SampleBottleAllocation>().Where(i => i.SampleRegistration != null && i.SampleRegistration.Oid == objsample.Oid && i.SignOffBy == null).ToList();
                                        foreach (SampleBottleAllocation sample in lstBottle)
                                        {
                                            List<string> lstBottles = sample.BottleID.Split(',').ToList().Where(i => !string.IsNullOrEmpty(i)).Select(i => i.Trim()).ToList();
                                            lstAssignedBttle.AddRange(lstBottles);
                                        }
                                        int qty = 0;
                                        foreach (string objLetter in lstqtyBottle.ToList())
                                        {
                                            qty = qty + lstAssignedBttle.Where(i => i == objLetter).Count();
                                        }
                                        if (qty != lstAssignedBttle.Count)
                                        {
                                            Modules.BusinessObjects.SampleManagement.SampleLogIn objSampleLogin = uow.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objsample.Oid);
                                            objSampleLogin.Qty = Convert.ToUInt16(samplesplit[2]);
                                            uow.CommitChanges();
                                            View.ObjectSpace.Refresh();
                                            Application.ShowViewStrategy.ShowMessage("The test has already been assigned to bottles; please remove the bottle ID.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        }
                                    }
                                }
                                //uow.CommitChanges();
                                //CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                                //if (cv != null)
                                //{
                                //    DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleBottleAllocation_SampleRegistration") as DashboardViewItem;
                                //    if (dvsampleids != null && dvsampleids.InnerView != null)
                                //    {
                                //        dvsampleids.InnerView.ObjectSpace.Refresh();
                                //    }
                                //}
                            }
                        }
                    }


                }
                //else if (gridListEditor != null && parameter != null && View.Id == "SampleLogIn_ListView_SampleRegistration_CopyTo_SampleID")
                //{
                //    if (gridListEditor != null && gridListEditor.Grid != null)
                //    {
                //        if (SRInfo.lstcopytosampleID == null)
                //        {
                //            SRInfo.lstcopytosampleID = new List<Guid>();
                //        }
                //        if (samplesplit[0] == "Selectall")
                //        {
                //            SRInfo.lstcopytosampleID.Clear();
                //foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn objsampling in ((ListView)View).CollectionSource.List.Cast<Modules.BusinessObjects.SampleManagement.SampleLogIn>().ToList())
                //{
                //                if (!SRInfo.lstcopytosampleID.Contains(objsampling.Oid))
                //    {
                //                    SRInfo.lstcopytosampleID.Add(objsampling.Oid);
                //                }
                //            }
                //        }
                //        else if (samplesplit[0] == "UNSelectall")
                //        {
                //            //foreach (Modules.BusinessObjects.SampleManagement.SampleLogIn objsampling in ((ListView)View).CollectionSource.List.Cast<Modules.BusinessObjects.SampleManagement.SampleLogIn>().ToList())
                //            //{
                //            //    if (SInfo.lstcopytosampleID.Contains(objsampling.Oid))
                //            //    {
                //            //        SInfo.lstcopytosampleID.Remove(objsampling.Oid);
                //            //    }
                //            //}
                //            //qctypeinfo.chkselectall = null;
                //        }
                //        else if (samplesplit[0] == "Selected" || samplesplit[0] == "UNSelected")
                //        {
                //            if (samplesplit[0] == "Selected")
                //            {
                //                Modules.BusinessObjects.SampleManagement.SampleLogIn objsampling = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[Oid]=?", new Guid(samplesplit[1])), true);
                //                if (objsampling != null)
                //                {
                //                    if (!SRInfo.lstcopytosampleID.Contains(objsampling.Oid))
                //                    {
                //                        SRInfo.lstcopytosampleID.Add(objsampling.Oid);
                //                    }
                //                }
                //            }
                //            else if (samplesplit[0] == "UNSelected")
                //            {
                //                Modules.BusinessObjects.SampleManagement.SampleLogIn objsampling = View.ObjectSpace.FindObject<Modules.BusinessObjects.SampleManagement.SampleLogIn>(CriteriaOperator.Parse("[Oid]=?", new Guid(samplesplit[1])), true);
                //                if (objsampling != null)
                //                {
                //                    if (SRInfo.lstcopytosampleID.Contains(objsampling.Oid))
                //                    {
                //                        SRInfo.lstcopytosampleID.Remove(objsampling.Oid);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void AcceptAction_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                SampleBottleAllocation objsampling = ((ListView)View).CollectionSource.List.Cast<SampleBottleAllocation>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                string assigned = string.Empty;
                DialogController objDialog = (DialogController)sender;
                ListView view = null;
                if (objDialog != null && objDialog.Frame.View is ListView)
                {
                    view = (ListView)objDialog.Frame.View;
                }
                if (view.SelectedObjects.Count > 0)
                {
                    if (objsampling.SampleRegistration.Qty >= SRInfo.lstviewselected.Count)
                    {
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            if (objsampling != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                SampleBottleAllocation objBottle = os.GetObjectByKey<SampleBottleAllocation>(objsampling.Oid);
                                if (objBottle != null)
                                {
                                    if (view.SelectedObjects.Count == 1)
                                    {
                                        objBottle.BottleID = ((DummyClass)view.SelectedObjects[0]).Name;
                                        IList<SampleBottleAllocation> samples = os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [TestMethod.Oid]=?", objBottle.SampleRegistration.Oid, objBottle.TestMethod.Oid)).ToList();
                                        if (samples != null && samples.Count > 1)
                                        {
                                            foreach (SampleBottleAllocation sampleBottle in samples)
                                            {
                                                if (sampleBottle.Oid != objBottle.Oid)
                                                {
                                                    os.Delete(sampleBottle);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                foreach (DummyClass objdc in view.SelectedObjects.Cast<DummyClass>().OrderBy(a => a.Name).ToList())
                {
                    if (!assigned.Contains(objdc.Name))
                    {
                        if (assigned == string.Empty)
                        {
                                                    objBottle.BottleID = assigned = objdc.Name;
                        }
                        else
                        {
                            assigned = assigned + ", " + objdc.Name;
                                                    IList<SampleBottleAllocation> samples = os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [TestMethod.Oid]=? And [BottleID]=?", objBottle.SampleRegistration.Oid, objBottle.TestMethod.Oid, objdc.Name), true).ToList();
                                                    if (samples.Count == 0)
                                                    {
                                                        SampleBottleAllocation objNewBottle = os.CreateObject<SampleBottleAllocation>();
                                                        objNewBottle.BottleID = objdc.Name;
                                                        objNewBottle.SampleRegistration = os.GetObjectByKey<Modules.BusinessObjects.SampleManagement.SampleLogIn>(objBottle.SampleRegistration.Oid);
                                                        objNewBottle.TestMethod = os.GetObjectByKey<TestMethod>(objBottle.TestMethod.Oid);
                                                        if (objBottle.Containers != null)
                                                        {
                                                            objNewBottle.Containers = os.GetObjectByKey<Container>(objBottle.Containers.Oid);
                        }
                                                        if (objBottle.Preservative != null)
                                                        {
                                                            objNewBottle.Preservative = os.GetObjectByKey<Preservative>(objBottle.Preservative.Oid);
                    }
                                                        if (objBottle.StorageID != null)
                                                        {
                                                            objNewBottle.StorageID = os.GetObjectByKey<Storage>(objBottle.StorageID.Oid);
                }
                                                        if (objBottle.StorageCondition != null)
                {
                                                            objNewBottle.StorageCondition = os.GetObjectByKey<PreserveCondition>(objBottle.StorageCondition.Oid);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        IList<SampleBottleAllocation> delsamples = os.GetObjects<SampleBottleAllocation>(CriteriaOperator.Parse("[SampleRegistration.Oid]=? And [TestMethod.Oid]=? And Not [BottleID] In (" + string.Format("'{0}'", assigned.Replace(", ", "','")) + ")", objBottle.SampleRegistration.Oid, objBottle.TestMethod.Oid)).ToList();
                                        foreach (SampleBottleAllocation sample in delsamples)
                                {
                                            os.Delete(sample);
                                        }
                                    }
                                    if (objBottle.SampleRegistration.JobID.Status != SampleRegistrationSignoffStatus.PendingSubmit)
                                    {
                                        Frame.GetController<AuditlogViewController>().insertauditdata(os, objBottle.SampleRegistration.JobID.Oid, OperationType.ValueChanged, "SampleBottleAllocation", objBottle.SampleRegistration.SampleID, "BottleID", objBottle.BottleID, assigned, "");
                                    }
                                    HttpContext.Current.Session["Assign"] = assigned;
                                    os.CommitChanges();
                                    os.Refresh();
                                    View.ObjectSpace.Refresh();
                                }
                            }
                           ((ListView)View).Refresh();
                        }
                    }
                    else
                    {
                        Application.ShowViewStrategy.ShowMessage("Selected bottleid count greather than Qty", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        e.Cancel = true;
                    }
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("Select BottleID check box", InformationType.Error, timer.Seconds, InformationPosition.Top);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Tests_AcceptAction_Execute(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                IObjectSpace ossmpl = Application.CreateObjectSpace(typeof(SampleBottleAllocation));
                string strtest = string.Empty;
                string strtestname = string.Empty;
                List<Guid> lstContainer = new List<Guid>();
                List<Guid> lstPreservative = new List<Guid>();

                if (sender != null)
                {
                    DialogController objDialog = (DialogController)sender as DialogController;
                    if (objDialog.Window.View.Id == "TestMethod_ListView_SamplesBA_Popup")
                    {
                        if (objDialog.Window.View.SelectedObjects.Count > 0)
                        {
                            foreach (TestMethod objtm in objDialog.Window.View.SelectedObjects)
                            {
                                if (objtm != null && string.IsNullOrEmpty(strtest))
                                {
                                    strtest = objtm.Oid.ToString();
                                    strtestname = objtm.TestName;
                                }
                                else
                                {
                                    strtestname = strtestname + "; " + objtm.TestName;
                                    strtest = strtest + "; " + objtm.Oid.ToString();
                                }

                                IList<Guid> containerNames = objtm.TestGuides.Where(i => i.Container != null).Select(i => i.Container.Oid).ToList();
                                if (containerNames != null && containerNames.Count > 0)
                                {
                                    foreach (Guid objContainer in containerNames)
                                    {
                                        if (!lstContainer.Contains(objContainer))
                                        {
                                            lstContainer.Add(objContainer);
                                        }
                                    }
                                }
                                IList<Guid> Preservatives = objtm.TestGuides.Where(i => i.Preservative != null).Select(i => i.Preservative.Oid).ToList();
                                if (Preservatives != null && Preservatives.Count > 0)
                                {
                                    foreach (Guid objPreservative in Preservatives)
                                    {
                                        if (!lstPreservative.Contains(objPreservative))
                                        {
                                            lstPreservative.Add(objPreservative);
                                        }
                                    }
                                }

                            }

                            SampleBottleAllocation objsmplbtl = ObjectSpace.FindObject<SampleBottleAllocation>(CriteriaOperator.Parse("[Oid] = ?", SRInfo.lstsmplbtlalloGuid));
                            if (objsmplbtl != null)
                            {
                                if (lstContainer.Count == 1)
                                {
                                    Modules.BusinessObjects.Setting.Container objContainer = ObjectSpace.FindObject<Modules.BusinessObjects.Setting.Container>(CriteriaOperator.Parse("Oid=?", lstContainer[0]));
                                    if (objContainer != null)
                                    {
                                        objsmplbtl.Containers = objContainer;
                                    }
                                }
                                else if (lstContainer.Count == 0)
                                {
                                    objsmplbtl.Containers = null;
                                }
                                if (lstPreservative.Count == 1)
                                {
                                    Preservative objpreservative = ObjectSpace.FindObject<Preservative>(CriteriaOperator.Parse("Oid=?", lstPreservative[0]));
                                    if (objpreservative != null)
                                    {
                                        objsmplbtl.Preservative = objpreservative;
                                    }
                                }
                                else if (lstPreservative.Count == 0)
                                {
                                    objsmplbtl.Preservative = null;
                                }
                                ObjectSpace.CommitChanges();
                                ObjectSpace.Refresh();
                            }
                            else
                            {
                                Application.ShowViewStrategy.ShowMessage("Select checkbox", InformationType.Info, timer.Seconds, InformationPosition.Top);
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Select checkbox", InformationType.Info, timer.Seconds, InformationPosition.Top);
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
    }
}
