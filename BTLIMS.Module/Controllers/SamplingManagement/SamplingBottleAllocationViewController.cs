using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using DevExpress.Data.Filtering;
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
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.SampleManagement;
using Modules.BusinessObjects.SamplingManagement;
using Modules.BusinessObjects.Setting;

namespace LDM.Module.Controllers.SamplingManagement
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SamplingBottleAllocationViewController : ViewController, IXafCallbackHandler
    {
        SamplingManagementInfo ObjSMInfo = new SamplingManagementInfo();
        MessageTimer timer = new MessageTimer();
        ICallbackManagerHolder BottlecallbackManager;
        PermissionInfo objPermissionInfo = new PermissionInfo();

        public SamplingBottleAllocationViewController()
        {
            InitializeComponent();
            TargetViewId = "SamplingBottleAllocation_DetailView_Sampling;" + "Sampling_ListView_Bottle;" + "SamplingBottleAllocation_ListView;"
              + "NPSamplingSample_Bottle_DetailView;" + "DummyClass_ListView_Sampling;";
            btnAssignbottles_SamplingBottleAllocation.TargetViewId = btnCopybottles_SamplingBottleAllocation.TargetViewId = btnResetbottles_SamplingBottleAllocation.TargetViewId = "SamplingBottleAllocation_ListView;";
            QtyResetSampling.TargetViewId = QtyOkSampling.TargetViewId = "SamplingBottleAllocation_DetailView_Sampling";
           
        }
        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                //btnAssignbottles_SamplingBottleAllocation.Active["showAssignbtlalloc"] = objPermissionInfo.SamplingProposalIsWrite;
                //btnCopybottles_SamplingBottleAllocation.Active["showCopybtlalloc"] = objPermissionInfo.SamplingProposalIsWrite;
                //btnResetbottles_SamplingBottleAllocation.Active["showresetbtlalloc"] = objPermissionInfo.SamplingProposalIsWrite;
                Employee currentUser = (Employee)SecuritySystem.CurrentUser;
                ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
                if (Frame is DevExpress.ExpressApp.Web.PopupWindow)
                {
                    DevExpress.ExpressApp.Web.PopupWindow popupWindow = Frame as DevExpress.ExpressApp.Web.PopupWindow;
                    if (popupWindow != null && popupWindow.View != null && popupWindow.View.Id == "SamplingBottleAllocation_DetailView_Sampling")
                    {
                        popupWindow.RefreshParentWindowOnCloseButtonClick = true;
                    }
                }
                ObjectSpace.Committed += ObjectSpace_Committed;
                if (View.Id == "TestMethod_ListView_Samples_BA" || View.Id == "SamplingBottleAllocation_ListView")
                {
                    ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] Is Null");
                }
                else if (View.Id == "DummyClass_ListView_Sampling")
                {
                    ObjSMInfo.selectionhideGuid = new List<string>();
                    ObjSMInfo.lstviewselected = new List<DummyClass>();
                }
                else if(View.Id== "SamplingBottleAllocation_DetailView_Sampling")
                {
                    Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active["DisableUnsavedChangesNotificationController"] = false;
                }
                if (View.Id == "SamplingBottleAllocation_ListView")
                {
                    if (ObjSMInfo.lstsmplbtlallo == null)
                    {
                        ObjSMInfo.lstsmplbtlallo = new List<SampleBottleAllocation>();
                    }
                    if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null && Application.MainWindow.View.ObjectTypeInfo.Type == typeof(SamplingProposal) && ((DetailView)Application.MainWindow.View).ViewEditMode == ViewEditMode.Edit)
                    {
                        objPermissionInfo.SamplingProposalIsWrite = true;
                    }
                    else
                    {
                        foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions.Where(i => i.RoleNavigationPermissionDetails.FirstOrDefault(x => x.NavigationItem.NavigationId == "SamplingProposal") != null))
                        {
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplingProposal" && i.Create == true) != null)
                            {
                                objPermissionInfo.SampleBottleIsCreate = true;
                                btnAssignbottles_SamplingBottleAllocation.Active["DisableAssign"] = true;
                                btnCopybottles_SamplingBottleAllocation.Active["DisableCopySet"] = true;
                                btnResetbottles_SamplingBottleAllocation.Active["DisableReset"] = true;
                                return;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplingProposal" && i.Write == true) != null)
                            {
                                objPermissionInfo.SampleBottleIsWrite = true;
                                btnAssignbottles_SamplingBottleAllocation.Active["DisableAssign"] = true;
                                btnCopybottles_SamplingBottleAllocation.Active["DisableCopySet"] = true;
                                btnResetbottles_SamplingBottleAllocation.Active["DisableReset"] = true;
                                return;
                            }
                            if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "SamplingProposal" && i.Read == true) != null)
                            {
                                objPermissionInfo.SampleBottleIsRead = true;
                                btnAssignbottles_SamplingBottleAllocation.Active["DisableAssign"] = false;
                                btnCopybottles_SamplingBottleAllocation.Active["DisableCopySet"] = false;
                                btnResetbottles_SamplingBottleAllocation.Active["DisableReset"] = false;
                                return;
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
                if (e.PopupFrame.View.Id == "DummyClass_ListView_Sampling")
                {
                    ObjSMInfo.Ispopup = true;
                }
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
                if (View != null && View.Id == "SamplingBottleAllocation_ListView")
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
                if (View.Id == "SamplingBottleAllocation_ListView")
                {
                    BottlecallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    BottlecallbackManager.CallbackManager.RegisterHandler("CanDeleteTaskBottleAllocation", this);

                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    gridListEditor.Grid.Settings.ShowStatusBar = GridViewStatusBarMode.Visible;
                    gridListEditor.Grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 270;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.ClientInstanceName = "sampleid";
                    BottlecallbackManager.CallbackManager.RegisterHandler("SamplingBottleIDPopup", this);
                    BottlecallbackManager.CallbackManager.RegisterHandler("SamplingBottleSelection", this);
                    gridListEditor.Grid.HtmlDataCellPrepared += Grid_HtmlDataCellPrepared;
                    gridListEditor.Grid.FillContextMenuItems += GridView_FillContextMenuItems;
                    gridListEditor.Grid.SettingsContextMenu.Enabled = true;
                    gridListEditor.Grid.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                    gridListEditor.Grid.Load += Grid_Load;
                    
                    if (objPermissionInfo.SamplingProposalIsWrite)
                    {
                        gridListEditor.Grid.ClientSideEvents.ContextMenuItemClick = @"function(s, e) { 
                    if (s.IsRowSelectedOnPage(e.elementIndex)) 
                    { 
                         var FocusedColumn = sessionStorage.getItem('CurrFocusedColumn');                                
                         var oid;
                         var text;
                         if (FocusedColumn && FocusedColumn.includes('.')) 
                         {                                       
                              oid = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn, false);
                              text = s.batchEditApi.GetCellTextContainer(e.elementIndex, FocusedColumn).innerText;                                                     
                              console.log('oid:', oid);
                              console.log('text:', text);

                               if (e.item.name == 'CopyToAllCell')
                               { if (FocusedColumn=='BottleID' || FocusedColumn=='Containers.Oid' || FocusedColumn=='Preservative.Oid' || FocusedColumn=='StorageID.Oid')
                               {

                                    for (var i = 0; i < s.GetVisibleRowsOnPage(); i++)
                                    { 
                                       if (s.IsRowSelectedOnPage(i))
                                       {                                               
                                         s.batchEditApi.SetCellValue(i, FocusedColumn, oid, text, false);
                                       }
                                    }

                                 }

                               }        
                         } 
                         else if (FocusedColumn) 
                         {                                                             
                            var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex, FocusedColumn);                            
                            console.log('CopyValue:', CopyValue);

                            if (e.item.name == 'CopyToAllCell')
                             {
                                 if (FocusedColumn=='BottleID' || FocusedColumn=='Containers.Oid' || FocusedColumn=='Preservative.Oid' || FocusedColumn=='StorageID.Oid')
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

                    }
                     e.processOnServer = false;
                     }";
                    }
                    else
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                        {
                           e.cancel = true;
                        }";
                    }
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
                    gridListEditor.Grid.JSProperties["cpVisibleRowCount"] = gridListEditor.Grid.VisibleRowCount;


                }
                if (View.Id == "DummyClass_ListView_Sampling")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    BottlecallbackManager = (ICallbackManagerHolder)WebWindow.CurrentRequestPage;
                    BottlecallbackManager.CallbackManager.RegisterHandler("SampleDummyClass", this);
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
                    Sampling objsampling = ObjectSpace.FindObject<Sampling>(CriteriaOperator.Parse("[Oid] = ?", ObjSMInfo.SamplingGuid));
                    if (ObjSMInfo.Ispopup == true && objsampling != null)
                    {
                        int strbottleid = 0;
                        List<string> lstbtlid = new List<string>();
                        ObjSMInfo.lstbottleid = new List<DummyClass>();
                        int strbottleqty = (Convert.ToInt32(((XPObjectSpace)ObjectSpace).Session.Evaluate(typeof(Sampling), CriteriaOperator.Parse("SUM(Qty)"), CriteriaOperator.Parse("[Oid]=?", objsampling.Oid))));
                        if (strbottleqty > 0)
                        {
                            IList<SamplingBottleAllocation> lstsmplbtl = ObjectSpace.GetObjects<SamplingBottleAllocation>(CriteriaOperator.Parse("[Sampling.Oid]=?", objsampling.Oid));
                            foreach (SamplingBottleAllocation objsmplbtl in lstsmplbtl.ToList())
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
                                        ObjSMInfo.lstbottleid.Add(objdc);
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
                                        ObjSMInfo.lstbottleid.Add(objdc);
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
                                        ObjSMInfo.lstbottleid.Add(objdc);
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

                else if (View.Id == "Sampling_ListView_Bottle")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    gridListEditor.Grid.Settings.VerticalScrollBarMode = ScrollBarMode.Visible;
                    gridListEditor.Grid.Settings.VerticalScrollableHeight = 310;
                    gridListEditor.Grid.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                    gridListEditor.Grid.SelectionChanged += Grid_SelectionChanged;
                    gridListEditor.Grid.Load += Grid_Load;
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
                    if(objPermissionInfo.SamplingProposalIsWrite)
                    {
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
                    }
                    else
                    {
                        gridListEditor.Grid.ClientSideEvents.BatchEditStartEditing = @"function(s,e) 
                        {
                           e.cancel = true;
                        }";
                    }
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
                    if (!((ListView)View).CollectionSource.Criteria.ContainsKey("matrix"))
                    {
                        ((ListView)View).CollectionSource.Criteria["matrix"] = CriteriaOperator.Parse("[SamplingProposal.RegistrationID] = ?", ObjSMInfo.strJobID);
                    }
                }
                else if (View.Id == "NPSamplingSample_Bottle_DetailView")
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
                else if(View.Id== "SamplingBottleAllocation_DetailView_Sampling")
                {
                    ASPxIntPropertyEditor qtyProerty = ((DetailView)View).FindItem("DefaultContainerQty") as ASPxIntPropertyEditor;
                    if(qtyProerty!=null)
                    {
                        if ((qtyProerty as ASPxIntPropertyEditor).Editor != null)
                            (qtyProerty as ASPxIntPropertyEditor).Editor.Load += Editor_Load;
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }


        private void Editor_Load(object sender, EventArgs e)
        {
            try
            {
                DevExpress.Web.ASPxSpinEdit editor = sender as DevExpress.Web.ASPxSpinEdit;
                editor.MinValue = 1;
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
                if (View.Id == "Sampling_ListView_Bottle")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                        if (cv != null)
                        {
                            DashboardViewItem dvBottleAllocation = ((DetailView)cv).FindItem("SamplingBottle") as DashboardViewItem;
                            if (dvBottleAllocation != null && dvBottleAllocation.InnerView != null)
                            {
                                if ((Sampling)View.CurrentObject != null)
                                {
                                    Sampling logIn = (Sampling)View.CurrentObject;
                                    if (logIn != null)
                                    {
                                        ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria.Clear();
                                        ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Sampling.Oid] = ?", logIn.Oid);
                                        ((ListView)dvBottleAllocation.InnerView).Refresh();
                                        if (ObjSMInfo.counter == 0)
                                        {
                                            ObjSMInfo.SamplingGuid = logIn.Oid;
                                            ObjSMInfo.strselSample = logIn.Oid.ToString();
                                        }
                                    }
                                }
                                else if (View.SelectedObjects.Count == 0 || (View.SelectedObjects.Count == gridListEditor.Grid.VisibleRowCount && gridListEditor.Grid.VisibleRowCount != 2))
                                {
                                    ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria.Clear();
                                    ((ListView)dvBottleAllocation.InnerView).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("[Oid] is null");
                                    ((ListView)dvBottleAllocation.InnerView).Refresh();
                                    ObjSMInfo.SamplingGuid = new Guid();
                                    ObjSMInfo.strselSample = null;
                                    ObjSMInfo.counter = 0;
                                    ObjSMInfo.CanProcess = null;
                                }
                                else if (ObjSMInfo.CanProcess == true)
                                {
                                    if (ObjSMInfo.counter == 0)
                                    {
                                        ObjSMInfo.counter = 1;
                                    }
                                    else
                                    {
                                        ObjSMInfo.counter = 0;
                                        ObjSMInfo.CanProcess = false;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(ObjSMInfo.strselSample);
                                    }
                                }
                                else if (ObjSMInfo.CanProcess == false)
                                {
                                    Sampling logIn = View.SelectedObjects.Cast<Sampling>().Where(a => a.Oid.ToString() != ObjSMInfo.strselSample).FirstOrDefault();
                                    if (logIn != null)
                                    {
                                        ObjSMInfo.CanProcess = true;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(logIn.Oid);
                                    }
                                }
                                else if (ObjSMInfo.CanProcess == null && ObjSMInfo.strselSample != null && (Sampling)View.CurrentObject == null && View.SelectedObjects.Count > 1)
                                {
                                    if (ObjSMInfo.counter == 0)
                                    {
                                        ObjSMInfo.counter = 1;
                                    }
                                    else
                                    {
                                        ObjSMInfo.counter = 0;
                                        ObjSMInfo.CanProcess = true;
                                        gridListEditor.Grid.Selection.UnselectRowByKey(ObjSMInfo.strselSample);
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
                        if (ObjSMInfo.lstavailtest.Contains(curOid.ToString().Trim()))
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
                if (View.Id == "DummyClass_ListView_Sampling")
                {
                    if (ObjSMInfo.lstcrtbottleid != null && ObjSMInfo.lstcrtbottleid.Count > 0)
                    {
                        for (int i = 0; i <= gridView.VisibleRowCount - 1; i++)
                        {
                            if (!string.IsNullOrEmpty(gridView.GetRowValues(i, "Name").ToString()))
                            {
                                string strbottleid = gridView.GetRowValues(i, "Name").ToString();
                                if (ObjSMInfo.lstcrtbottleid.Contains(strbottleid))
                                {
                                    gridView.Selection.SelectRow(i);
                                }
                            }
                        }
                    }
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                    ObjSMInfo.lstcrtbottleid = new List<string>();
                    ObjSMInfo.Ispopup = false;
                }
                else if (View.Id == "SamplingBottleAllocation_ListView")
                {
                    gridView.JSProperties["cpVisibleRowCount"] = gridView.VisibleRowCount;
                }
                else if (View.Id == "Sampling_ListView_Bottle")
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
                if (objPermissionInfo.SamplingProposalIsWrite)
                {
                    if (e.DataColumn.FieldName == "BottleID")
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
                ((WebApplication)Application).PopupWindowManager.PopupShowing -= PopupWindowManager_PopupShowing;
                ObjectSpace.Committed -= ObjectSpace_Committed;
                if (View.Id == "DummyClass_ListView_Sampling")
                {
                    if (ObjSMInfo.selectionhideGuid!=null && ObjSMInfo.selectionhideGuid.Count>0)
                    {
                        ObjSMInfo.selectionhideGuid.Clear(); 
                    }
                    if (ObjSMInfo.lstviewselected!=null && ObjSMInfo.lstviewselected.Count>0)
                    {
                        ObjSMInfo.lstviewselected.Clear(); 
                    }
                }
                else if (View.Id == "SamplingBottleAllocation_ListView")
                {
                    if (ObjSMInfo.lstsmplbtlallo!=null && ObjSMInfo.lstsmplbtlallo.Count>0)
                    {
                        ObjSMInfo.lstsmplbtlallo.Clear(); 
                    }
                }
                else if (View.Id == "SamplingBottleAllocation_DetailView_Sampling")
                {
                    if (Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active.Contains("DisableUnsavedChangesNotificationController"))
                    {
                        Frame.GetController<WebConfirmUnsavedChangesDetailViewController>().Active.RemoveItem("DisableUnsavedChangesNotificationController"); 
                    }
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
                DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleIDs") as DashboardViewItem;
                if (dvsampleids != null && dvsampleids.InnerView != null)
                {
                    dvsampleids.InnerView.ObjectSpace.Refresh();
                }
            }
        }

        private void btnAssignbottles_BottleAllocation_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (ObjSMInfo.SamplingGuid != null)
                {
                    int i = 65;
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<SamplingBottleAllocation> lstcurrentBottle = uow.Query<SamplingBottleAllocation>().Where(a => a.Sampling != null && a.Sampling.Oid == ObjSMInfo.SamplingGuid).ToList();
                    if (lstcurrentBottle.Where(a => a.SignOffBy != null).ToList().Count > 0)
                    {
                        Application.ShowViewStrategy.ShowMessage("Bottle cannot be assigned", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }

                    lstcurrentBottle.ForEach(a => a.BottleID = string.Empty);
                    if (lstcurrentBottle.Count > 0)
                    {
                        Guid matrix = lstcurrentBottle.Select(a => a.Sampling.VisualMatrix.Oid).FirstOrDefault();
                        IList<string> Testguids = lstcurrentBottle.Select(a => a.TestMethod.Oid.ToString()).ToList();
                        List<BottleSharing> lstBottlesharing = uow.Query<BottleSharing>().Where(a => a.SampleMatrix.Oid == matrix).ToList();
                        foreach (BottleSharing bottleSharing in lstBottlesharing)
                        {
                            string[] sharingtest = bottleSharing.Tests.Split(new string[] { "; " }, StringSplitOptions.None);
                            if (sharingtest.Count(a => Testguids.Contains(a)) > 1)
                            {
                                foreach (SamplingBottleAllocation sample in lstcurrentBottle.Where(a => sharingtest.Contains(a.TestMethod.Oid.ToString())).ToList())
                                {
                                    if (sample.BottleID == string.Empty)
                                    {
                                        sample.BottleID = ((char)i).ToString();
                                    }
                                    else
                                    {
                                        sample.BottleID += ", " + ((char)i).ToString();
                                    }
                                    if (sample.Sampling.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                    {
                                        Frame.GetController<AuditlogViewController>().insertauditdata(uow, sample.Sampling.SamplingProposal.Oid, OperationType.ValueChanged, "SampleBottleAllocation", sample.Sampling.SampleID, "Assignbottles", "", sample.BottleID, "");
                                    }
                                }
                            }
                            i++;
                        }
                        IList<SamplingBottleAllocation> EBottle = lstcurrentBottle.Where(a => string.IsNullOrEmpty(a.BottleID)).ToList();
                        if (EBottle.Count > 0)
                        {
                            foreach (SamplingBottleAllocation bottle in EBottle)
                            {
                                bottle.BottleID = ((char)i).ToString();
                                if (bottle.Sampling.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                {
                                    Frame.GetController<AuditlogViewController>().insertauditdata(uow, bottle.Sampling.SamplingProposal.Oid, OperationType.ValueChanged, "SampleBottleAllocation", bottle.Sampling.SampleID, "Assignbottles", "", bottle.BottleID, "");
                                }
                            }
                        }
                        else
                        {
                            i--;
                        }
                        lstcurrentBottle.FirstOrDefault().Sampling.Qty = (uint)(i - 64);
                        uow.CommitChanges();
                        View.ObjectSpace.Refresh();
                        samplegridrefresh();
                        Application.ShowViewStrategy.ShowMessage("Bottle assigned successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
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
                List<SamplingBottleAllocation> lstBottle = uow.Query<SamplingBottleAllocation>().Where(i => i.Sampling != null && i.Sampling.SamplingProposal.RegistrationID == ObjSMInfo.strJobID && i.SignOffBy == null).ToList();
                foreach (SamplingBottleAllocation sample in lstBottle)
                {
                    sample.BottleID = "A";
                    sample.Sampling.Qty = 1;
                    sample.Containers = null;
                    sample.Preservative = null;
                    sample.StorageID = null;
                    sample.StorageCondition = null;
                    if (sample.Sampling.SamplingProposal.Status != RegistrationStatus.PendingSampling)
                    {
                        Frame.GetController<AuditlogViewController>().insertauditdata(uow, sample.Sampling.SamplingProposal.Oid, OperationType.ValueChanged, "SampleBottleAllocation", sample.Sampling.SampleID, "Resetbottles", "", sample.BottleID, "");
                    }
                }
                CompositeView cv = ((NestedFrame)Frame).ViewItem.View;
                if (cv!=null)
                {
                    DashboardViewItem dvsampleids = ((DetailView)cv).FindItem("SampleIDs") as DashboardViewItem;
                    if (dvsampleids != null && dvsampleids.InnerView != null)
                    {
                        ((ListView)dvsampleids.InnerView).CollectionSource.List.Cast<Sampling>().ToList().ForEach(i => i.Qty = 1);
                        dvsampleids.InnerView.ObjectSpace.CommitChanges();
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
                            NPSamplingSample_Bottle btle = os.CreateObject<NPSamplingSample_Bottle>();
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
                            ObjSMInfo.Ispopup = false;
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
                    NPSamplingSample_Bottle logIn_Bottle = (NPSamplingSample_Bottle)dialog.Window.View.CurrentObject;
                    if(logIn_Bottle!=null)
                    {
                        if(logIn_Bottle.From==null)
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Select the From.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        if (logIn_Bottle.To == null)
                        {
                            e.Cancel = true;
                            Application.ShowViewStrategy.ShowMessage("Select the To.", InformationType.Error, timer.Seconds, InformationPosition.Top);
                            return;
                        }
                        List<SamplingBottleAllocation> lstcurrentBottle = uow.Query<SamplingBottleAllocation>().Where(i => i.Sampling != null && i.Sampling.Oid == logIn_Bottle.From.Oid).ToList();
                        if (logIn_Bottle != null && !string.IsNullOrEmpty(logIn_Bottle.To))
                        {
                            foreach (string oid in logIn_Bottle.To.Split(new string[] { "; " }, StringSplitOptions.None))
                            {
                                foreach (SamplingBottleAllocation objextsmplbtl in lstcurrentBottle.ToList())
                                {
                                    SamplingBottleAllocation objSamplebottle = uow.FindObject<SamplingBottleAllocation>(CriteriaOperator.Parse("[Sampling.Oid] = ? and [TestMethod.Oid] = ?", new Guid(oid), objextsmplbtl.TestMethod.Oid));
                                    if (objSamplebottle != null)
                                    {
                                        objSamplebottle.BottleID = objextsmplbtl.BottleID;
                                        if (objextsmplbtl.Containers != null)
                                        {
                                            objSamplebottle.Containers = uow.GetObjectByKey<Container>(objextsmplbtl.Containers.Oid);
                                        }
                                        if (objextsmplbtl.Preservative != null)
                                        {
                                            objSamplebottle.Preservative = uow.GetObjectByKey<Preservative>(objextsmplbtl.Preservative.Oid);
                                        }
                                        if (objextsmplbtl.StorageID != null)
                                        {
                                            objSamplebottle.StorageID = uow.GetObjectByKey<Storage>(objextsmplbtl.StorageID.Oid);
                                        }
                                        if (objextsmplbtl.StorageCondition != null)
                                        {
                                            objSamplebottle.StorageCondition = uow.GetObjectByKey<PreserveCondition>(objextsmplbtl.StorageCondition.Oid);
                                        }
                                        objSamplebottle.Sampling.Qty = objextsmplbtl.Sampling.Qty;
                                        if (objSamplebottle.Sampling.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                        {
                                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, objSamplebottle.Sampling.SamplingProposal.Oid, OperationType.ValueChanged, "SampleBottleAllocation", objSamplebottle.Sampling.SampleID, "Copybottles", "", objSamplebottle.BottleID, "");
                                        }
                                    }
                                }
                            }
                            uow.CommitChanges();
                            samplegridrefresh();
                            Application.ShowViewStrategy.ShowMessage("Bottle set copied successfully", InformationType.Success, timer.Seconds, InformationPosition.Top);
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

        public void ProcessAction(string parameter)
        {
            try
            {
                ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                string[] samplesplit = parameter.Split('|');
                if (gridListEditor != null && parameter != null && View.Id == "SamplingBottleAllocation_ListView")
                {
                    if (objPermissionInfo.SampleBottleIsRead == false)
                    {
                        if (samplesplit[0] == "BottleID")
                        {
                            HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                            if (HttpContext.Current.Session["rowid"] != null)
                            {
                                ObjSMInfo.lstcrtbottleid = new List<string>();
                                SamplingBottleAllocation objsampling = ((ListView)View).CollectionSource.List.Cast<SamplingBottleAllocation>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                                if (objsampling != null && objsampling.BottleID != null)
                                {
                                    string[] strbottleid = objsampling.BottleID.Split(',');
                                    foreach (var strbotid in strbottleid)
                                    {
                                        ObjSMInfo.lstcrtbottleid.Add(strbotid.Trim());
                                    }
                                }
                            }
                            CollectionSource cs = new CollectionSource(ObjectSpace, typeof(DummyClass));
                            ListView lv = Application.CreateListView("DummyClass_ListView_Sampling", cs, false);
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
                else if (gridListEditor != null && parameter != null && View.Id == "DummyClass_ListView_Sampling")
                {
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        if (parameter == "Selectall")
                        {
                            if (gridListEditor.Grid.VisibleRowCount == gridListEditor.Grid.Selection.Count)
                            {
                                foreach (DummyClass objdc in ((ListView)View).CollectionSource.List.Cast<DummyClass>().ToList())
                                {
                                    if (!ObjSMInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                    {
                                        if (!ObjSMInfo.lstviewselected.Contains(objdc))
                                        {
                                            ObjSMInfo.lstviewselected.Add(objdc);
                                        }
                                    }
                                }
                            }
                        }
                        else if (parameter == "UNSelectall")
                        {
                            ObjSMInfo.lstviewselected.Clear();
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
                                        if (!ObjSMInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                        {
                                            if (!ObjSMInfo.lstviewselected.Contains(objdc))
                                            {
                                                ObjSMInfo.lstviewselected.Add(objdc);
                                            }
                                        }
                                    }
                                }
                                else if (splparm[0] == "UNSelected")
                                {
                                    ObjSMInfo.lstviewselected.Clear();
                                    foreach (DummyClass objdc in View.SelectedObjects)
                                    {
                                        if (!ObjSMInfo.selectionhideGuid.Contains(objdc.Oid.ToString()))
                                        {
                                            if (!ObjSMInfo.lstviewselected.Contains(objdc))
                                            {
                                                ObjSMInfo.lstviewselected.Add(objdc);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                       ((ListView)View).Refresh();
                }
                else if (gridListEditor != null && parameter != null && View.Id == "Sampling_ListView_Bottle")
                {
                    if (samplesplit[0] == "Qty")
                    {
                        HttpContext.Current.Session["rowid"] = gridListEditor.Grid.GetRowValues(int.Parse(samplesplit[1]), "Oid");
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            Sampling objsample = ((ListView)View).CollectionSource.List.Cast<Sampling>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                            if (objsample != null)
                            {
                                ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
                                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                                UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                                if (objsample.Qty <= 0)
                                {
                                    Sampling objSampleLogin = uow.GetObjectByKey<Sampling>(objsample.Oid);
                                    objSampleLogin.Qty = Convert.ToUInt16(samplesplit[2]);
                                    uow.CommitChanges();
                                    View.ObjectSpace.Refresh();
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
                                    List<SamplingBottleAllocation> lstBottle = uow.Query<SamplingBottleAllocation>().Where(i => i.Sampling != null && i.Sampling.Oid == objsample.Oid && i.SignOffBy == null).ToList();
                                    foreach (SamplingBottleAllocation sample in lstBottle)
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
                                        Sampling objSampleLogin = uow.GetObjectByKey<Sampling>(objsample.Oid);
                                        objSampleLogin.Qty = Convert.ToUInt16(samplesplit[2]);
                                        uow.CommitChanges();
                                        View.ObjectSpace.Refresh();
                                        Application.ShowViewStrategy.ShowMessage("The test has already been assigned to bottles; please remove the bottle ID.", InformationType.Error, timer.Seconds, InformationPosition.Top);
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
        private void AcceptAction_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                SamplingBottleAllocation objsampling = ((ListView)View).CollectionSource.List.Cast<SamplingBottleAllocation>().Where(a => a.Oid == new Guid(HttpContext.Current.Session["rowid"].ToString())).First();
                string assigned = string.Empty;
                DialogController objDialog = (DialogController)sender;
                ListView view = null;
                if (objDialog != null && objDialog.Frame.View is ListView)
                {
                    view = (ListView)objDialog.Frame.View;
                }
                foreach (DummyClass objdc in view.SelectedObjects.Cast<DummyClass>().OrderBy(a => a.Name).ToList())
                {
                    if (!assigned.Contains(objdc.Name))
                    {
                        if (assigned == string.Empty)
                        {
                            assigned = objdc.Name;
                        }
                        else
                        {
                            assigned = assigned + ", " + objdc.Name;
                        }
                    }
                }
                if (view.SelectedObjects.Count > 0)
                {
                    if (objsampling.Sampling.Qty >= ObjSMInfo.lstviewselected.Count)
                    {
                        if (HttpContext.Current.Session["rowid"] != null)
                        {
                            if (objsampling != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                SamplingBottleAllocation objBottle = os.GetObjectByKey<SamplingBottleAllocation>(objsampling.Oid);
                                if (objBottle != null)
                                {
                                    if (objBottle.Sampling.SamplingProposal.Status != RegistrationStatus.PendingSubmission)
                                    {
                                        Frame.GetController<AuditlogViewController>().insertauditdata(os, objBottle.Sampling.SamplingProposal.Oid, OperationType.ValueChanged, "SampleBottleAllocation", objBottle.Sampling.SampleID, "BottleID", objBottle.BottleID, assigned, "");
                                    }
                                    objBottle.BottleID = assigned;
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
                IObjectSpace ossmpl = Application.CreateObjectSpace(typeof(SamplingBottleAllocation));
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

                            SamplingBottleAllocation objsmplbtl = ObjectSpace.FindObject<SamplingBottleAllocation>(CriteriaOperator.Parse("[Oid] = ?", ObjSMInfo.lstsmplbtlalloGuid));
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
        private void QtyReset_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    List<SamplingBottleAllocation> lstBottle = uow.Query<SamplingBottleAllocation>().Where(i => i.Sampling != null && i.Sampling.SamplingProposal.RegistrationID == ObjSMInfo.strJobID && i.SignOffBy == null).ToList();
                    SamplingBottleAllocation objsampleallocation = View.CurrentObject as SamplingBottleAllocation;
                    foreach (SamplingBottleAllocation sample in lstBottle)
                    {
                        sample.BottleID = "A";
                        sample.Sampling.Qty = 1;
                        if (sample.Sampling.SamplingProposal.Status != RegistrationStatus.PendingSampling)
                        {
                            Frame.GetController<AuditlogViewController>().insertauditdata(uow, sample.Sampling.SamplingProposal.Oid, OperationType.ValueChanged, "SampleBottleAllocation", sample.Sampling.SampleID, "Resetbottles", "", sample.BottleID, "");
                        }
                    }
                    DashboardViewItem dvsampleids = ((DetailView)View).FindItem("SampleIDs") as DashboardViewItem;
                    if (dvsampleids != null && dvsampleids.InnerView != null)
                    {
                        ((ListView)dvsampleids.InnerView).CollectionSource.List.Cast<Sampling>().ToList().ForEach(i => i.Qty =1);
                        dvsampleids.InnerView.ObjectSpace.CommitChanges();
                    }
                    objsampleallocation.DefaultContainerQty = 1;
                    uow.CommitChanges();
                    Refresh();
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
        private void Refresh()
        {
            DashboardViewItem dvsampleidss = ((DetailView)View).FindItem("SamplingBottle") as DashboardViewItem;
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

        private void QtyOk_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View != null)
                {
                    SamplingBottleAllocation objsampleallocation = View.CurrentObject as SamplingBottleAllocation;
                    Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                    UnitOfWork uow = new UnitOfWork(currentSession.DataLayer);
                    if (objsampleallocation.DefaultContainerQty < 1)
                    {
                        Application.ShowViewStrategy.ShowMessage("This default container qty must greater than 0", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    else if(objsampleallocation.DefaultContainerQty==null)
                    {
                        Application.ShowViewStrategy.ShowMessage("This default container qty must not be empty", InformationType.Error, timer.Seconds, InformationPosition.Top);
                        return;
                    }
                    List<SamplingBottleAllocation> lstBottle = uow.Query<SamplingBottleAllocation>().Where(i => i.Sampling != null && i.Sampling.SamplingProposal.RegistrationID == ObjSMInfo.strJobID && i.SignOffBy == null).ToList();
                    if (objsampleallocation.DefaultContainerQty > 0)
                    {
                        foreach (SamplingBottleAllocation sample in lstBottle)
                        {
                            sample.Sampling.Qty =objsampleallocation.DefaultContainerQty;
                            sample.BottleID = "A";
                        }
                    }
                    DashboardViewItem dvsampleids = ((DetailView)View).FindItem("SampleIDs") as DashboardViewItem;
                    if(dvsampleids!=null && dvsampleids.InnerView!=null)
                    {
                        ((ListView)dvsampleids.InnerView).CollectionSource.List.Cast<Sampling>().ToList().ForEach(i => i.Qty = objsampleallocation.DefaultContainerQty);
                        dvsampleids.InnerView.ObjectSpace.CommitChanges();
                    }
                    uow.CommitChanges();
                    Refresh();

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
