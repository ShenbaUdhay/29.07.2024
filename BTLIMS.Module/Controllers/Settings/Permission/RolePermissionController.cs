using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Web;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NavigationItem = Modules.BusinessObjects.Setting.NavigationItem;

namespace LDM.Module.Controllers.Settings.Permission
{
    public partial class RolePermissionController : ViewController
    {
        MessageTimer timer = new MessageTimer();
        PermissionInfo permissionInfo = new PermissionInfo();
        bool boolCaption = false;
        curlanguage objLanguage = new curlanguage();

        public RolePermissionController()
        {
            InitializeComponent();
            TargetViewId = "RoleNavigationPermission_ListView;" + "RoleNavigationPermission_DetailView;" +
                "RoleNavigationPermission_RoleNavigationPermissionDetails_ListView;" + "RoleNavigationPermission_DetailView_New;"
                + "NavigationItem_ListView_Copy;" + "NavigationItem_ListView_Defaultsettings;";
            addNavigationItemAction.TargetViewId = "RoleNavigationPermission_RoleNavigationPermissionDetails_ListView;";
            removeNavigationItemAction.TargetViewId = "RoleNavigationPermission_RoleNavigationPermissionDetails_ListView;";
            GridSaveAction.TargetViewId = "NavigationItem_ListView_Defaultsettings;";
        }

        protected override void OnActivated()
        {
            try
            {
                base.OnActivated();
                Modules.BusinessObjects.Hr.Employee currentUser = SecuritySystem.CurrentUser as Modules.BusinessObjects.Hr.Employee;
                if (currentUser != null && View != null && View.Id != null)
                {

                    if (currentUser.Roles != null && currentUser.Roles.Count > 0)
                    {
                        permissionInfo.AnalysisDepartmentChainIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            permissionInfo.RolesIsWrite = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "Role" && i.Write == true) != null)
                                {
                                    permissionInfo.RolesIsWrite = true;
                                    //return;
                                }
                            }
                            addNavigationItemAction.Active["ShowLinkNavigationItem"] = permissionInfo.RolesIsWrite;
                            removeNavigationItemAction.Active["ShowUnLinkNavigationItem"] = permissionInfo.RolesIsWrite;
                        }
                    }
                }

                if (View.Id == "RoleNavigationPermission_DetailView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    DeleteObjectsViewController DeleteController = Frame.GetController<DeleteObjectsViewController>();
                    Frame.GetController<RefreshController>().RefreshAction.Executing += RefreshAction_Executing;
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Executing += SaveAction_Executing;
                        modificationController.SaveAction.Executed += SaveAction_Executed;
                        modificationController.SaveAndCloseAction.Active.SetItemValue("rolepermissionsaveclose.visible", false);
                        modificationController.SaveAndNewAction.Active.SetItemValue("rolepermissionsavenew.visible", false);
                    }
                    if (DeleteController != null)
                    {
                        DeleteController.DeleteAction.Executing += DeleteAction_Executing;
                    }

                    permissionInfo.LinkedNavigationItems = new List<Guid>();
                    permissionInfo.LinkedTypePermissionsDetails = new List<Guid>();
                    permissionInfo.UnLinkedNavigationItems = new List<Guid>();
                    permissionInfo.UnLinkedTypePermissionsDetails = new List<Guid>();
                }
                else if (View.Id == "RoleNavigationPermission_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing += NewObjectAction_Executing;
                    DeleteObjectsViewController DeleteController = Frame.GetController<DeleteObjectsViewController>();
                    if (DeleteController != null)
                    {
                        DeleteController.DeleteAction.Executing += DeleteAction_Executing;
                    }
                }
                else if (View.Id == "RoleNavigationPermission_DetailView_New")
                {
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Executed += SaveAction_Executed;
                        modificationController.SaveAndCloseAction.Active.SetItemValue("rolepermissionsaveclose.visible", false);
                        modificationController.SaveAndNewAction.Active.SetItemValue("rolepermissionsavenew.visible", false);
                    }
                    permissionInfo.LinkedNavigationItems = new List<Guid>();
                    permissionInfo.LinkedTypePermissionsDetails = new List<Guid>();
                    permissionInfo.UnLinkedNavigationItems = new List<Guid>();
                    permissionInfo.UnLinkedTypePermissionsDetails = new List<Guid>();
                }

            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void RefreshAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                ClearUnSavedData();
                permissionInfo.LinkedNavigationItems.Clear();
                permissionInfo.LinkedTypePermissionsDetails.Clear();
                permissionInfo.UnLinkedNavigationItems.Clear();
                permissionInfo.UnLinkedTypePermissionsDetails.Clear();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void ClearUnSavedData()
        {
            try
            {
                if (View.Id == "RoleNavigationPermission_DetailView" && View.CurrentObject != null)
                {
                    Guid oid = ((RoleNavigationPermission)View.CurrentObject).Oid;
                    IObjectSpace os = Application.CreateObjectSpace();
                    bool CanCommit = false;
                    RoleNavigationPermission role = os.GetObjectByKey<RoleNavigationPermission>(oid);
                    if (role != null)
                    {
                        if (permissionInfo.UnLinkedTypePermissionsDetails != null && permissionInfo.UnLinkedTypePermissionsDetails.Count > 0)
                        {
                            CanCommit = true;
                            foreach (Guid id in permissionInfo.UnLinkedTypePermissionsDetails)
                            {
                                role.RoleNavigationPermissionDetails.Add(os.GetObjectByKey<RoleNavigationPermissionDetails>(id));
                            }
                        }
                        if (permissionInfo.LinkedTypePermissionsDetails != null && permissionInfo.LinkedTypePermissionsDetails.Count > 0)
                        {
                            CanCommit = true;
                            foreach (Guid id in permissionInfo.LinkedTypePermissionsDetails)
                            {
                                role.RoleNavigationPermissionDetails.Remove(os.GetObjectByKey<RoleNavigationPermissionDetails>(id));
                            }
                        }
                    }
                    if (CanCommit)
                    {
                        os.CommitChanges();
                        os.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void NewObjectAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                IObjectSpace os = Application.CreateObjectSpace();
                RoleNavigationPermission permission = os.CreateObject<RoleNavigationPermission>();
                DetailView dv = Application.CreateDetailView(os, "RoleNavigationPermission_DetailView_New", true, permission);
                dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                Frame.SetView(dv);
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executed(object sender, ActionBaseEventArgs e)
        {
            try
            {
                if (View.Id == "RoleNavigationPermission_DetailView")
                {
                    permissionInfo.LinkedNavigationItems.Clear();
                    permissionInfo.LinkedTypePermissionsDetails.Clear();
                    permissionInfo.UnLinkedNavigationItems.Clear();
                    permissionInfo.UnLinkedTypePermissionsDetails.Clear();
                    RoleNavigationPermission objRolePermission = (RoleNavigationPermission)View.CurrentObject;
                    if (objRolePermission != null && objRolePermission.Employees.Count > 0)
                    {
                        foreach (Modules.BusinessObjects.Hr.Employee objEmployee in objRolePermission.Employees)
                        {
                            Frame.GetController<EmployeeController>().AssignPermissionToUser(objEmployee, objRolePermission);
                        }
                    }
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);

                }
                else if (View.Id == "RoleNavigationPermission_DetailView_New")
                {
                    IObjectSpace objSpace = Application.CreateObjectSpace();
                    DetailView dv = Application.CreateDetailView(objSpace, "RoleNavigationPermission_DetailView", true, objSpace.GetObject<RoleNavigationPermission>((RoleNavigationPermission)View.CurrentObject));
                    dv.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                    Frame.SetView(dv);
                    Application.ShowViewStrategy.ShowMessage(CaptionHelper.GetLocalizedText(@"Messages\SuccessfulMessage", "Savesuccess"), InformationType.Success, timer.Seconds, InformationPosition.Top);
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeleteAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "RoleNavigationPermission_DetailView" && View.CurrentObject != null && View.CurrentObject is RoleNavigationPermission)
                {
                    //e.Cancel = true;
                    RoleNavigationPermission objRolePermission = (RoleNavigationPermission)View.CurrentObject;
                    if (!View.ObjectSpace.IsDeletedObject(objRolePermission) && objRolePermission != null && objRolePermission.Employees.Count > 0)
                    {
                        DeletePermissions(objRolePermission);
                    }
                }
                else if (View.Id == "RoleNavigationPermission_ListView" && View.SelectedObjects.Count > 0)
                {
                    List<RoleNavigationPermission> lstPermissions = View.SelectedObjects.Cast<RoleNavigationPermission>().ToList();
                    foreach (RoleNavigationPermission objRolePermission in lstPermissions)
                    {
                        DeletePermissions(objRolePermission);
                    }
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void DeletePermissions(RoleNavigationPermission objRolePermission)
        {
            try
            {
                IObjectSpace os = Application.CreateObjectSpace();
                objRolePermission = os.GetObjectByKey<RoleNavigationPermission>(objRolePermission.Oid);
                if (objRolePermission != null)
                {
                    if (View.Id != "RoleNavigationPermission_DetailView")
                    {
                        foreach (RoleNavigationPermissionDetails permissionDetails in objRolePermission.RoleNavigationPermissionDetails.ToList())
                        {
                            objRolePermission.RoleNavigationPermissionDetails.Remove(permissionDetails);
                        }
                        foreach (Employee objEmployee in objRolePermission.Employees.ToList())
                        {
                            objRolePermission.Employees.Remove(objEmployee);
                        }
                    }
                    IList<CustomSystemRole> roles = os.GetObjects<CustomSystemRole>(CriteriaOperator.Parse("[RoleNavigationPermission.Oid] = ?", objRolePermission.Oid));
                    foreach (CustomSystemRole role in roles.ToList())
                    {
                        os.Delete(role);
                    }
                }
                os.CommitChanges();
                os.Dispose();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            try
            {
                if (View.Id == "RoleNavigationPermission_DetailView")
                {
                    ListPropertyEditor lstNavigation = (ListPropertyEditor)((DetailView)View).FindItem("RoleNavigationPermissionDetails");
                    if (lstNavigation != null && lstNavigation.ListView != null)
                    {
                        if (((ASPxGridListEditor)((ListView)lstNavigation.ListView).Editor).Grid != null)
                        {
                            ((ASPxGridListEditor)((ListView)lstNavigation.ListView).Editor).Grid.UpdateEdit();
                        }
                    }
                    if (View.CurrentObject != null)
                    {
                        RoleNavigationPermission currentRole = (RoleNavigationPermission)View.CurrentObject;
                        if (currentRole.AdministrativePrivilege == AdministrativePrivilege.ClientAdministrator && currentRole.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationView != null && i.NavigationItem.NavigationView.EndsWith("ListView") && i.NavigationItem.NavigationModelClass != null && i.NavigationItem.NavigationModelClass == typeof(Employee).ToString()) == null)
                        {
                            RoleNavigationPermissionDetails roleDetails = View.ObjectSpace.CreateObject<RoleNavigationPermissionDetails>();
                            roleDetails.NavigationItem = View.ObjectSpace.FindObject<NavigationItem>(CriteriaOperator.Parse("[NavigationModelClass] = ? AND EndsWith([NavigationView], 'ListView')", typeof(Employee).ToString()));
                            roleDetails.RoleNavigationPermission = currentRole;
                            roleDetails.Navigate = true;
                            roleDetails.Read = true;
                            roleDetails.Create = true;
                            roleDetails.Write = true;
                            roleDetails.Delete = true;
                        }
                        if (currentRole.AdministrativePrivilege != AdministrativePrivilege.SystemSupplierAdministrator)
                        {

                            IList<Employee> lstEmployees = View.ObjectSpace.GetObjects<Employee>(CriteriaOperator.Parse("[RolePermissions][[Oid] = ?]", currentRole.Oid));
                            foreach (Employee objEmployee in lstEmployees.ToList())
                            {
                                bool isAdministrative = false;
                                IList<RoleNavigationPermission> lstRole = View.ObjectSpace.GetObjects<RoleNavigationPermission>(CriteriaOperator.Parse("[Employees][[Oid] = ?]", objEmployee.Oid));
                                foreach (RoleNavigationPermission objRole in lstRole)
                                {
                                    if (objRole.AdministrativePrivilege == AdministrativePrivilege.SystemSupplierAdministrator)
                                    {
                                        isAdministrative = true;
                                    }
                                }
                                if (!isAdministrative)
                                {
                                    PermissionPolicyRole role = View.ObjectSpace.FindObject<PermissionPolicyRole>(CriteriaOperator.Parse("[Name] = 'Administrator'"));
                                    if (objEmployee != null && role != null)
                                    {
                                        objEmployee.Roles.Remove(role);
                                    }
                                }

                                isAdministrative = false;
                                //foreach (PermissionPolicyRole objRole in objEmployee.Roles.ToList())
                                //{
                                //    CustomSystemRole role = (CustomSystemRole)objRole;
                                //    if (role != null && role.RoleNavigationPermission != null && role.RoleNavigationPermission.Oid == currentRole.Oid)
                                //    {
                                //        role.IsAdministrative = false;
                                //    }
                                //}
                                //foreach (CustomSystemRole objRole in objEmployee.Roles.Cast<CustomSystemRole>().Where(i => i.RoleNavigationPermission != null && i.RoleNavigationPermission.Oid == currentRole.Oid).ToList())
                                //{
                                //    objRole.IsAdministrative = false;
                                //}
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

        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View.Id == "RoleNavigationPermission_RoleNavigationPermissionDetails_ListView" || View.Id == "NavigationItem_ListView_Defaultsettings")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gridListEditor.Grid.CustomJSProperties += Grid_CustomJSProperties;
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                            gv.FillContextMenuItems += Gv_FillContextMenuItems;
                            gv.SettingsContextMenu.Enabled = true;
                            gv.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                            gv.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {   
                                sessionStorage.setItem('RoleNavigationFocusedColumn', null); 
                                if((e.cellInfo.column.name.indexOf('Command') !== -1) || (e.cellInfo.column.name == 'Edit'))
                                {                              
                                    e.cancel = true;
                                }    
                                else
                                {
                                    var fieldName = e.cellInfo.column.fieldName;                       
                                    sessionStorage.setItem('RoleNavigationFocusedColumn', fieldName);
                                }                                         
                            }";
                            if (View.Id == "RoleNavigationPermission_RoleNavigationPermissionDetails_ListView")
                            {
                                ((ListView)View).CollectionSource.Criteria["Filter"] = CriteriaOperator.Parse("NavigationItem.GCRecord is null And GCRecord is null");
                                gv.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                            var FocusedColumn = sessionStorage.getItem('RoleNavigationFocusedColumn');       
                            if(FocusedColumn != 'NonNavigationItem')
                            {
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                {                                                            
                                    var oid;
                                    var text;
                                    if(FocusedColumn.includes('.'))
                                    {       
                                        oid=s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn,false);
                                        text = s.batchEditApi.GetCellTextContainer(e.elementIndex,FocusedColumn).innerText;                                                     
                                        if (e.item.name =='CopyToAllCell')
                                        {  
                                            for(var i = 0; i < s.cpVisibleRowCount; i++)
                                            { 
                                                if (s.IsRowSelectedOnPage(i)) 
                                                {                                               
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                                }
                                            }
                                        }        
                                    }
                                else{
                                var CopyValue = s.batchEditApi.GetCellValue(e.elementIndex,FocusedColumn);                            
                                if (e.item.name =='CopyToAllCell')
                                {
                                    //console.log(s.cpVisibleRowCount);
                                    for(var i = 0; i < s.cpVisibleRowCount; i++)
                                    {  
                                        //console.log(i);
                                        if (s.IsRowSelectedOnPage(i)) 
                                        {
                                            console.log(CopyValue);
                                            s.batchEditApi.SetCellValue(i,FocusedColumn,CopyValue);                    
                                        }
                                    }
                                }    
                             }
                                }
                                e.processOnServer = false;
                            }
                            }";
                            }

                            else if (View.Id == "NavigationItem_ListView_Defaultsettings")
                            {

                                gv.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                         {                        
                        sessionStorage.setItem('NavigationItemFocusedColumn', null); 
                        if(e.cellInfo.column.name.indexOf('Command') !== -1)
                        {                              
                            e.cancel = true;
                        }
                        else if (e.cellInfo.column.fieldName == 'Defaultbool')
                        {
                            var fieldName = e.cellInfo.column.fieldName;                       
                            sessionStorage.setItem('NavigationItemFocusedColumn', fieldName);  
                        } 
                        else if (e.cellInfo.column.fieldName == 'Select')
                        {
                            var fieldName = e.cellInfo.column.fieldName;                       
                            sessionStorage.setItem('NavigationItemFocusedColumn', fieldName);  
                        } 
                                          
                    }";
                                gv.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                { 
                                    var FocusedColumn = sessionStorage.getItem('NavigationItemFocusedColumn');      //alert(FocusedColumn);                          
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
                                                    s.batchEditApi.SetCellValue(i,FocusedColumn,oid,text,false);
                                                }
                                            }
                                        }        
                                    }
                                else{
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
                            }";
                            }
                        }
                    }
                }
                else if (View.Id == "RoleNavigationPermission_ListView")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null && gridListEditor.Grid != null)
                    {
                        ASPxGridView gv = gridListEditor.Grid;
                        if (gv != null)
                        {
                            gv.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        }
                    }
                }
                else if (View.Id == "NavigationItem_ListView_Copy" && boolCaption == false)
                {
                    //CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse(""));
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (objLanguage.strcurlanguage != "En")
                    {
                        foreach (WebColumnBase column in gridListEditor.Grid.Columns)
                        {
                            IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                            if (columnInfo != null)
                            {
                                if (columnInfo.Model.Id == "NavigationCNCaption")
                                {
                                    column.Visible = true;
                                }
                                else if (columnInfo.Model.Id == "NavigationCaption")
                                {
                                    column.Visible = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (WebColumnBase column in gridListEditor.Grid.Columns)
                        {
                            IColumnInfo columnInfo = ((IDataItemTemplateInfoProvider)gridListEditor).GetColumnInfo(column);
                            if (columnInfo != null)
                            {
                                if (columnInfo.Model.Id == "NavigationCNCaption")
                                {
                                    column.Visible = false;
                                }
                                else if (columnInfo.Model.Id == "NavigationCaption")
                                {
                                    column.Visible = true;
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
        private void Grid_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            try
            {
                ASPxGridView gridView = sender as ASPxGridView;
                e.Properties["cpVisibleRowCount"] = gridView.VisibleRowCount;
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void Gv_FillContextMenuItems(object sender, ASPxGridViewContextMenuEventArgs e)
        {
            try
            {
                if (e.MenuType == GridViewContextMenuType.Rows)
                {
                    //CurrentLanguage currentLanguage = ObjectSpace.FindObject<CurrentLanguage>(CriteriaOperator.Parse(""));
                    if (objLanguage.strcurlanguage != "En")
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
                        item.Image.IconID = "edit_copy_16x16office2013";
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
            try
            {
                base.OnDeactivated();
                if (View.Id == "RoleNavigationPermission_DetailView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    DeleteObjectsViewController DeleteController = Frame.GetController<DeleteObjectsViewController>();
                    Frame.GetController<RefreshController>().RefreshAction.Executing -= RefreshAction_Executing;
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Executing -= SaveAction_Executing;
                        modificationController.SaveAction.Executed -= SaveAction_Executed;
                        modificationController.SaveAndCloseAction.Active.RemoveItem("rolepermissionsaveclose.visible");
                        modificationController.SaveAndNewAction.Active.RemoveItem("rolepermissionsavenew.visible");
                    }
                    if (DeleteController != null)
                    {
                        DeleteController.DeleteAction.Executing -= DeleteAction_Executing;
                    }

                    if (View != null && View.CurrentObject != null)
                    {
                        RoleNavigationPermission role = (RoleNavigationPermission)View.CurrentObject;
                        bool IsDeleted = View.ObjectSpace.IsDeletedObject(role);
                        if (!View.ObjectSpace.IsDeletedObject(role))
                        {
                            ClearUnSavedData();
                        }
                    }

                    permissionInfo.LinkedNavigationItems.Clear();
                    permissionInfo.LinkedTypePermissionsDetails.Clear();
                    permissionInfo.UnLinkedNavigationItems.Clear();
                    permissionInfo.UnLinkedTypePermissionsDetails.Clear();
                }
                else if (View.Id == "RoleNavigationPermission_ListView")
                {
                    Frame.GetController<NewObjectViewController>().NewObjectAction.Executing -= NewObjectAction_Executing;
                    DeleteObjectsViewController DeleteController = Frame.GetController<DeleteObjectsViewController>();
                    if (DeleteController != null)
                    {
                        DeleteController.DeleteAction.Executing -= DeleteAction_Executing;
                    }
                }
                else if (View.Id == "RoleNavigationPermission_DetailView_New")
                {
                    ModificationsController modificationController = Frame.GetController<ModificationsController>();
                    if (modificationController != null)
                    {
                        modificationController.SaveAction.Executed -= SaveAction_Executed;
                        modificationController.SaveAndCloseAction.Active.RemoveItem("rolepermissionsaveclose.visible");
                        modificationController.SaveAndNewAction.Active.RemoveItem("rolepermissionsavenew.visible");
                    }
                }
                else if (View.Id == "NavigationItem_ListView_Copy")
                {
                    boolCaption = false;
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void addNavigationItemAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            try
            {
                if (e.PopupWindowViewSelectedObjects.Count > 0)
                {
                    if (Frame != null && Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                        {
                            CompositeView cv = nestedFrame.ViewItem.View;
                            if (cv != null && cv.CurrentObject != null)
                            {
                                RoleNavigationPermission role = (RoleNavigationPermission)cv.CurrentObject;
                                if (role != null)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    bool CanCommit = false;
                                    foreach (Modules.BusinessObjects.Setting.NavigationItem item in e.PopupWindowViewSelectedObjects)
                                    {
                                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.Oid == item.Oid) == null)
                                        {
                                            RoleNavigationPermissionDetails details = cv.ObjectSpace.FindObject<RoleNavigationPermissionDetails>(CriteriaOperator.Parse("[NavigationItem.Oid] = ? And [RoleNavigationPermission.Oid] = ?", item.Oid, role.Oid));

                                            if (details == null)
                                            {
                                                if (permissionInfo.UnLinkedNavigationItems.Count == 0 || permissionInfo.UnLinkedNavigationItems.FirstOrDefault(i => i == item.Oid) == null)
                                                {
                                                    details = cv.ObjectSpace.FindObject<RoleNavigationPermissionDetails>(CriteriaOperator.Parse("[NavigationItem.Oid] = ? And [RoleNavigationPermission] Is Null", item.Oid));
                                                    if (details == null)
                                                    {
                                                        details = os.CreateObject<RoleNavigationPermissionDetails>();
                                                    }
                                                    else
                                                    {
                                                        details = os.GetObject<RoleNavigationPermissionDetails>(details);
                                                    }
                                                }
                                                else
                                                {
                                                    details = os.FindObject<RoleNavigationPermissionDetails>(CriteriaOperator.Parse("[NavigationItem.Oid] = ? And [RoleNavigationPermission] Is Null", item.Oid));
                                                    if (details == null)
                                                    {
                                                        details = os.CreateObject<RoleNavigationPermissionDetails>();
                                                    }
                                                }

                                                if (details != null)
                                                {
                                                    details.Read = true;
                                                    details.Create = false;
                                                    details.Write = false;
                                                    details.Delete = false;
                                                    details.Navigate = true;

                                                    details.NavigationItem = os.GetObject<NavigationItem>(item);
                                                    details.RoleNavigationPermission = os.GetObject<RoleNavigationPermission>(role);
                                                    if (!permissionInfo.LinkedNavigationItems.Contains(item.Oid))
                                                    {
                                                        permissionInfo.LinkedNavigationItems.Add(item.Oid);
                                                    }
                                                    if (permissionInfo.UnLinkedNavigationItems.Contains(item.Oid))
                                                    {
                                                        permissionInfo.UnLinkedNavigationItems.Remove(item.Oid);
                                                    }
                                                    if (permissionInfo.UnLinkedTypePermissionsDetails.Contains(details.Oid))
                                                    {
                                                        permissionInfo.UnLinkedTypePermissionsDetails.Remove(details.Oid);
                                                    }
                                                    if (!permissionInfo.LinkedTypePermissionsDetails.Contains(details.Oid))
                                                    {
                                                        permissionInfo.LinkedTypePermissionsDetails.Add(details.Oid);
                                                    }
                                                    CanCommit = true;
                                                }
                                            }
                                        }
                                    }
                                    if (CanCommit)
                                    {
                                        os.CommitChanges();
                                        os.Dispose();
                                    }
                                }
                                cv.ObjectSpace.Refresh();
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

        private void removeNavigationItemAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "RoleNavigationPermission_RoleNavigationPermissionDetails_ListView" && View.SelectedObjects.Count > 0)
                {
                    if (Frame != null && Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                        {
                            CompositeView cv = nestedFrame.ViewItem.View;
                            if (cv != null && cv.CurrentObject != null)
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                bool CanCommit = false;
                                //bool CanUnlinkEmployeeForm = true;
                                Guid oid = ((RoleNavigationPermission)cv.CurrentObject).Oid;
                                RoleNavigationPermission role = os.GetObjectByKey<RoleNavigationPermission>(oid);
                                if (role != null)
                                {
                                    List<RoleNavigationPermissionDetails> lstUnlinkedNavigation = View.SelectedObjects.Cast<RoleNavigationPermissionDetails>().Where(i => i.NavigationItem != null).ToList();

                                    if (role.AdministrativePrivilege == AdministrativePrivilege.ClientAdministrator && lstUnlinkedNavigation.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.NavigationModelClass == typeof(Employee).ToString() && i.NavigationItem.NavigationView.EndsWith("ListView")) != null)
                                    {
                                        //CanUnlinkEmployeeForm = false;
                                        Application.ShowViewStrategy.ShowMessage("Cannot Unlink Employee form from Client Administrator", InformationType.Error, timer.Seconds, InformationPosition.Top);
                                        return;
                                    }
                                    foreach (RoleNavigationPermissionDetails details in lstUnlinkedNavigation)
                                    {
                                        CanCommit = true;
                                        role.RoleNavigationPermissionDetails.Remove(os.GetObject<RoleNavigationPermissionDetails>(details));
                                        //((ListView)View).CollectionSource.Remove(details);
                                        if (permissionInfo.LinkedNavigationItems.Contains(details.NavigationItem.Oid))
                                        {
                                            permissionInfo.LinkedNavigationItems.Remove(details.NavigationItem.Oid);
                                        }
                                        if (permissionInfo.LinkedTypePermissionsDetails.Contains(details.Oid))
                                        {
                                            permissionInfo.LinkedTypePermissionsDetails.Remove(details.Oid);
                                        }
                                        if (!permissionInfo.UnLinkedNavigationItems.Contains(details.NavigationItem.Oid))
                                        {
                                            permissionInfo.UnLinkedNavigationItems.Add(details.NavigationItem.Oid);
                                        }
                                        if (!permissionInfo.UnLinkedTypePermissionsDetails.Contains(details.Oid))
                                        {
                                            permissionInfo.UnLinkedTypePermissionsDetails.Add(details.Oid);
                                        }
                                    }
                                }
                                if (CanCommit)
                                {
                                    os.CommitChanges();
                                    os.Dispose();
                                }
                                cv.ObjectSpace.Refresh();
                            }
                        }
                    }
                    //((ListView)View).Refresh();
                }
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }

        private void addNavigationItemAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                IObjectSpace objspace = Application.CreateObjectSpace();
                RoleNavigationPermission objRole = null;
                if (View.Id == "RoleNavigationPermission_DetailView" && View.CurrentObject != null && View.CurrentObject is RoleNavigationPermission)
                {
                    objRole = (RoleNavigationPermission)View.CurrentObject;
                }
                else if (View.Id == "RoleNavigationPermission_RoleNavigationPermissionDetails_ListView" && Frame is NestedFrame)
                {
                    NestedFrame nestedFrame = (NestedFrame)Frame;
                    if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                    {
                        CompositeView cv = (CompositeView)nestedFrame.ViewItem.View;
                        if (cv != null && cv.CurrentObject != null && cv.CurrentObject is RoleNavigationPermission)
                        {
                            objRole = (RoleNavigationPermission)cv.CurrentObject;
                        }
                    }
                }
                ListView createdView = null;
                if (objRole != null)
                {
                    CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.Setting.NavigationItem));
                    cs.Criteria.Clear();
                    if (objRole != null && objRole.RoleNavigationPermissionDetails != null && objRole.RoleNavigationPermissionDetails.Count > 0)
                    {
                        string strCriteria = "Not [Oid] In(" + string.Format("'{0}'", string.Join("','", objRole.RoleNavigationPermissionDetails.Where(i => i.NavigationItem != null).Select(i => i.NavigationItem.Oid.ToString().Replace("'", "''")))) + ")";
                        cs.Criteria["Filter"] = CriteriaOperator.Parse(strCriteria);
                    }
                    createdView = Application.CreateListView("NavigationItem_ListView_Copy", cs, false);
                    //e.Size = new System.Drawing.Size(750, 500);
                    //e.View = CreateListView;
                    //e.DialogController.SaveOnAccept = false;
                }
                else
                {
                    CollectionSource cs = new CollectionSource(objspace, typeof(Modules.BusinessObjects.Setting.NavigationItem));
                    cs.Criteria.Clear();
                    createdView = Application.CreateListView("NavigationItem_ListView_Copy", cs, false);
                    //e.Size = new System.Drawing.Size(750, 500);
                    //e.View = CreateListView;
                    //e.DialogController.SaveOnAccept = false;
                }
                if (createdView != null)
                {
                    ShowViewParameters showViewParameters = new ShowViewParameters(createdView);
                    showViewParameters.Context = TemplateContext.NestedFrame;
                    showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.SaveOnAccept = false;
                    dc.Accepting += AddNavigationItem_Accepting;
                    dc.CloseOnCurrentObjectProcessing = false;
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

        private void AddNavigationItem_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {
                if (e.AcceptActionArgs.SelectedObjects.Count > 0)
                {
                    if (Frame != null && Frame is NestedFrame)
                    {
                        NestedFrame nestedFrame = (NestedFrame)Frame;
                        if (nestedFrame != null && nestedFrame.ViewItem != null && nestedFrame.ViewItem.View != null)
                        {
                            CompositeView cv = nestedFrame.ViewItem.View;
                            if (cv != null && cv.CurrentObject != null)
                            {
                                RoleNavigationPermission role = (RoleNavigationPermission)cv.CurrentObject;
                                if (role != null)
                                {
                                    IObjectSpace os = Application.CreateObjectSpace();
                                    bool CanCommit = false;
                                    foreach (Modules.BusinessObjects.Setting.NavigationItem item in e.AcceptActionArgs.SelectedObjects)
                                    {
                                        if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem != null && i.NavigationItem.Oid == item.Oid) == null)
                                        {
                                            RoleNavigationPermissionDetails details = cv.ObjectSpace.FindObject<RoleNavigationPermissionDetails>(CriteriaOperator.Parse("[NavigationItem.Oid] = ? And [RoleNavigationPermission.Oid] = ?", item.Oid, role.Oid));

                                            if (details == null)
                                            {
                                                if (permissionInfo.UnLinkedNavigationItems.Count == 0 || permissionInfo.UnLinkedNavigationItems.FirstOrDefault(i => i == item.Oid) == null)
                                                {
                                                    details = cv.ObjectSpace.FindObject<RoleNavigationPermissionDetails>(CriteriaOperator.Parse("[NavigationItem.Oid] = ? And [RoleNavigationPermission] Is Null", item.Oid));
                                                    if (details == null)
                                                    {
                                                        details = os.CreateObject<RoleNavigationPermissionDetails>();
                                                    }
                                                    else
                                                    {
                                                        details = os.GetObject<RoleNavigationPermissionDetails>(details);
                                                    }
                                                }
                                                else
                                                {
                                                    details = os.FindObject<RoleNavigationPermissionDetails>(CriteriaOperator.Parse("[NavigationItem.Oid] = ? And [RoleNavigationPermission] Is Null", item.Oid));
                                                    if (details == null)
                                                    {
                                                        details = os.CreateObject<RoleNavigationPermissionDetails>();
                                                    }
                                                }

                                                if (details != null)
                                                {
                                                    details.Read = true;
                                                    details.Navigate = true;
                                                    if (role.PermissionPolicy == DevExpress.Persistent.Base.SecurityPermissionPolicy.AllowAllByDefault)
                                                    {
                                                        details.Create = true;
                                                        details.Write = true;
                                                        details.Delete = true;
                                                    }
                                                    else
                                                    {
                                                        details.Create = false;
                                                        details.Write = false;
                                                        details.Delete = false;
                                                    }

                                                    details.NavigationItem = os.GetObject<NavigationItem>(item);
                                                    details.RoleNavigationPermission = os.GetObject<RoleNavigationPermission>(role);
                                                    if (!permissionInfo.LinkedNavigationItems.Contains(item.Oid))
                                                    {
                                                        permissionInfo.LinkedNavigationItems.Add(item.Oid);
                                                    }
                                                    if (permissionInfo.UnLinkedNavigationItems.Contains(item.Oid))
                                                    {
                                                        permissionInfo.UnLinkedNavigationItems.Remove(item.Oid);
                                                    }
                                                    if (permissionInfo.UnLinkedTypePermissionsDetails.Contains(details.Oid))
                                                    {
                                                        permissionInfo.UnLinkedTypePermissionsDetails.Remove(details.Oid);
                                                    }
                                                    if (!permissionInfo.LinkedTypePermissionsDetails.Contains(details.Oid))
                                                    {
                                                        permissionInfo.LinkedTypePermissionsDetails.Add(details.Oid);
                                                    }
                                                    CanCommit = true;
                                                }
                                            }
                                        }
                                    }
                                    if (CanCommit)
                                    {
                                        os.CommitChanges();
                                        os.Dispose();
                                    }
                                }
                                cv.ObjectSpace.Refresh();
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

        private void GridSaveAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                if (View.Id == "NavigationItem_ListView_Defaultsettings")
                {
                    ((ASPxGridListEditor)((ListView)View).Editor).Grid.UpdateEdit();
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
