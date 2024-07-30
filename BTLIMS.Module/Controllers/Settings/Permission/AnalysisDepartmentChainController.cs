using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using DevExpress.Xpo;
using LDM.Module.Controllers.Public;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LDM.Module.Controllers.Settings.Permission
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AnalysisDepartmentChainController : ViewController, IXafCallbackHandler
    {
        MessageTimer timer = new MessageTimer();
        AnalysisDepartmentChainInfo departmentChainInfo = new AnalysisDepartmentChainInfo();
        PermissionInfo objPermissionInfo = new PermissionInfo();
        public AnalysisDepartmentChainController()
        {
            InitializeComponent();
            TargetViewId = "AnalysisDepartmentChain_ListView;" + "TestMethod_ListView_AnalysisDepartmentChain;" + "Employee_LookupListView_AnalysisDepartmentChain;";
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
                        objPermissionInfo.AnalysisDepartmentChainIsWrite = false;
                        if (currentUser.Roles.FirstOrDefault(i => i.IsAdministrative == true) != null)
                        {
                            objPermissionInfo.AnalysisDepartmentChainIsWrite = true;
                        }
                        else
                        {
                            foreach (Modules.BusinessObjects.Setting.RoleNavigationPermission role in currentUser.RolePermissions)
                            {
                                if (role.RoleNavigationPermissionDetails.FirstOrDefault(i => i.NavigationItem.NavigationId == "AnalysisDepartmentChain" && i.Write == true) != null)
                                {
                                    objPermissionInfo.AnalysisDepartmentChainIsWrite = true;
                                    //return;
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
        protected override void OnViewControlsCreated()
        {
            try
            {
                base.OnViewControlsCreated();
                if (View.Id == "TestMethod_ListView_AnalysisDepartmentChain")
                {
                    XafCallbackManager callbackManager = ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;
                    callbackManager.RegisterHandler("AnalysisDepartmentChainHandler", this);

                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    if (gridView != null)
                    {
                        gridView.Settings.ShowStatusBar = GridViewStatusBarMode.Hidden;
                        gridView.HtmlDataCellPrepared += GridView_HtmlDataCellPrepared;
                        gridView.FillContextMenuItems += GridView_FillContextMenuItems;
                        gridView.SettingsContextMenu.Enabled = true;
                        gridView.SettingsContextMenu.EnableRowMenu = DevExpress.Utils.DefaultBoolean.True;
                        gridView.ClientSideEvents.FocusedCellChanging = @"function(s,e)
                            {                  
                                sessionStorage.setItem('TPFocusedColumn', null);  
                                var fieldName = e.cellInfo.column.fieldName;                       
                                sessionStorage.setItem('TPFocusedColumn', fieldName);   
                                //alert(fieldName);
                            }";
                        gridView.ClientSideEvents.ContextMenuItemClick = @"function(s,e) 
                            { 
                                if (s.IsRowSelectedOnPage(e.elementIndex))  
                                {  
                                    var FocusedColumn = sessionStorage.getItem('TPFocusedColumn'); 
                                    RaiseXafCallback(globalCallbackControl, 'AnalysisDepartmentChainHandler', FocusedColumn+'|'+e.elementIndex+'|CopyToAllCell', '', false)
                                }
                                e.processOnServer = false;
                            }";
                    }
                }
                else if (View.Id == "Employee_LookupListView_AnalysisDepartmentChain")
                {
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    ASPxGridView gridView = gridListEditor.Grid;
                    if (gridView != null)
                    {
                        gridView.PreRender += GridView_PreRender;
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

        private void GridView_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (View.Id == "Employee_LookupListView_AnalysisDepartmentChain")
                {
                    ASPxGridView grid = (ASPxGridView)sender;
                    if (grid != null && departmentChainInfo.EmployeesOid != null && departmentChainInfo.EmployeesOid.Count > 0)
                    {
                        foreach (Employee obj in ((ListView)View).CollectionSource.List)
                        {
                            if (departmentChainInfo.EmployeesOid.Contains(obj.Oid))
                            {
                                grid.Selection.SelectRowByKey(obj.Oid);
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

        private void GridView_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            try
            {
                if (View.Id == "TestMethod_ListView_AnalysisDepartmentChain" && objPermissionInfo.AnalysisDepartmentChainIsWrite)
                {
                    if (e.DataColumn.FieldName == "ResultEntryUsers")
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'AnalysisDepartmentChainHandler', 'ResultEntry|'+{0}, '', false)", e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "ResultValidationUsers")
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'AnalysisDepartmentChainHandler', 'ResultValidation|'+{0}, '', false)", e.VisibleIndex));
                    }
                    else if (e.DataColumn.FieldName == "ResultApprovalUsers")
                    {
                        e.Cell.Attributes.Add("ondblclick", string.Format(@"RaiseXafCallback(globalCallbackControl, 'AnalysisDepartmentChainHandler', 'ResultApproval|'+{0}, '', false)", e.VisibleIndex));
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
            try
            {
                base.OnDeactivated();
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
                if (!string.IsNullOrEmpty(parameter) && View.Id == "TestMethod_ListView_AnalysisDepartmentChain")
                {
                    string[] param = parameter.Split('|');
                    ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (editor != null && editor.Grid != null && param != null && param.Count() > 1)
                    {
                        HttpContext.Current.Session["rowid"] = editor.Grid.GetRowValues(int.Parse(param[1]), "Oid");
                        //Guid currentOid = new Guid(editor.Grid.GetRowValues(int.Parse(param[1]), "Oid"));
                        Guid currentOid = new Guid(HttpContext.Current.Session["rowid"].ToString());
                        TestMethod curTestMethod = View.ObjectSpace.GetObjectByKey<TestMethod>(currentOid);
                        if (curTestMethod != null)
                        {
                            if (param.Contains("CopyToAllCell"))
                            {
                                IObjectSpace os = Application.CreateObjectSpace();
                                IList<TestMethod> lstTestMethods = os.GetObjects<TestMethod>(CriteriaOperator.Parse("[Oid] <> ?", curTestMethod.Oid));
                                foreach (TestMethod testMethod in lstTestMethods.ToList())
                                {
                                    if (param[0] == "ResultEntryUsers")
                                    {
                                        IEnumerable<AnalysisDepartmentChain> resultEntryUsers = testMethod.DepartmentChains.Where(i => i.ResultEntry == true);
                                        foreach (AnalysisDepartmentChain departmentChain in resultEntryUsers.ToList())
                                        {
                                            os.Delete(departmentChain);
                                        }

                                        IEnumerable<AnalysisDepartmentChain> curTestResultEntryUsers = curTestMethod.DepartmentChains.Where(i => i.ResultEntry == true);
                                        foreach (AnalysisDepartmentChain departmentChain in curTestResultEntryUsers.ToList())
                                        {
                                            AnalysisDepartmentChain obj = os.CreateObject<AnalysisDepartmentChain>();
                                            obj.Employee = os.GetObjectByKey<Employee>(departmentChain.Employee.Oid);
                                            obj.ResultEntry = true;
                                            //os.CommitChanges();
                                            testMethod.DepartmentChains.Add(obj);
                                        }
                                    }
                                    else if (param[0] == "ResultValidationUsers")
                                    {
                                        IEnumerable<AnalysisDepartmentChain> samplePrep1Users = testMethod.DepartmentChains.Where(i => i.ResultValidation == true);
                                        foreach (AnalysisDepartmentChain departmentChain in samplePrep1Users.ToList())
                                        {
                                            os.Delete(departmentChain);
                                        }

                                        IEnumerable<AnalysisDepartmentChain> curTestSamplePrep1Users = curTestMethod.DepartmentChains.Where(i => i.ResultValidation == true);
                                        foreach (AnalysisDepartmentChain departmentChain in curTestSamplePrep1Users.ToList())
                                        {
                                            AnalysisDepartmentChain obj = os.CreateObject<AnalysisDepartmentChain>();
                                            obj.Employee = os.GetObjectByKey<Employee>(departmentChain.Employee.Oid);
                                            obj.ResultValidation = true;
                                            //os.CommitChanges();
                                            testMethod.DepartmentChains.Add(obj);
                                        }
                                    }
                                    else if (param[0] == "ResultApprovalUsers")
                                    {
                                        IEnumerable<AnalysisDepartmentChain> samplePrep2Users = testMethod.DepartmentChains.Where(i => i.ResultApproval == true);
                                        foreach (AnalysisDepartmentChain departmentChain in samplePrep2Users.ToList())
                                        {
                                            os.Delete(departmentChain);
                                        }

                                        IEnumerable<AnalysisDepartmentChain> curTestSamplePrep2Users = curTestMethod.DepartmentChains.Where(i => i.ResultApproval == true);
                                        foreach (AnalysisDepartmentChain departmentChain in curTestSamplePrep2Users.ToList())
                                        {
                                            AnalysisDepartmentChain obj = os.CreateObject<AnalysisDepartmentChain>();
                                            obj.Employee = os.GetObjectByKey<Employee>(departmentChain.Employee.Oid);
                                            obj.ResultApproval = true;
                                            //os.CommitChanges();
                                            testMethod.DepartmentChains.Add(obj);
                                        }
                                    }
                                }
                                os.CommitChanges();
                                ((ListView)View).ObjectSpace.Refresh();
                            }
                            else
                            {
                                departmentChainInfo.CurrentTest = curTestMethod.Oid;
                                if (param[0] == "ResultEntry")
                                {
                                    departmentChainInfo.ResultEntry = true;
                                    departmentChainInfo.ResultValidation = false;
                                    departmentChainInfo.ResultApproval = false;
                                    departmentChainInfo.EmployeesOid = curTestMethod.DepartmentChains.Where(i => i.ResultEntry == true).Select(i => i.Employee.Oid).ToList();
                                }
                                else if (param[0] == "ResultValidation")
                                {
                                    departmentChainInfo.ResultEntry = false;
                                    departmentChainInfo.ResultValidation = true;
                                    departmentChainInfo.ResultApproval = false;
                                    departmentChainInfo.EmployeesOid = curTestMethod.DepartmentChains.Where(i => i.ResultValidation == true).Select(i => i.Employee.Oid).ToList();
                                }
                                else if (param[0] == "ResultApproval")
                                {
                                    departmentChainInfo.ResultEntry = false;
                                    departmentChainInfo.ResultValidation = false;
                                    departmentChainInfo.ResultApproval = true;
                                    departmentChainInfo.EmployeesOid = curTestMethod.DepartmentChains.Where(i => i.ResultApproval == true).Select(i => i.Employee.Oid).ToList();
                                }
                                IObjectSpace objspace = Application.CreateObjectSpace();
                                CollectionSource cs = new CollectionSource(objspace, typeof(Employee));
                                ListView createListView = Application.CreateListView("Employee_LookupListView_AnalysisDepartmentChain", cs, false);
                                ShowViewParameters showViewParameters = new ShowViewParameters();
                                showViewParameters.CreatedView = createListView;
                                showViewParameters.Context = TemplateContext.PopupWindow;
                                showViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                                DialogController dc = Application.CreateController<DialogController>();
                                dc.SaveOnAccept = false;
                                dc.CloseOnCurrentObjectProcessing = false;
                                dc.Accepting += Dc_Accepting;
                                showViewParameters.Controllers.Add(dc);
                                Application.ShowViewStrategy.ShowView(showViewParameters, new ShowViewSource(null, null));
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

        private void Dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            try
            {

                IObjectSpace os = Application.CreateObjectSpace();
                Session currentSession = ((XPObjectSpace)(this.ObjectSpace)).Session;
                UnitOfWork uow = new UnitOfWork(((XPObjectSpace)os).Session.DataLayer);
                {
                    TestMethod testMethod = uow.GetObjectByKey<TestMethod>(departmentChainInfo.CurrentTest);
                    if (testMethod != null)
                    {
                        List<Guid> lstSelEmpOid = e.AcceptActionArgs.SelectedObjects.Cast<Employee>().Select(i => i.Oid).ToList();
                        List<Guid> lstRemovedEmpOid = departmentChainInfo.EmployeesOid.Except(lstSelEmpOid).ToList();
                        foreach (Guid oid in lstRemovedEmpOid)
                        {
                            if (departmentChainInfo.ResultEntry == true)
                            {
                                AnalysisDepartmentChain obj = testMethod.DepartmentChains.FirstOrDefault(i => i.Employee.Oid == oid && i.ResultEntry == true);
                                if (obj != null)
                                {
                                    testMethod.DepartmentChains.Remove(obj);
                                }
                            }
                            if (departmentChainInfo.ResultValidation == true)
                            {
                                AnalysisDepartmentChain obj = testMethod.DepartmentChains.FirstOrDefault(i => i.Employee.Oid == oid && i.ResultValidation == true);
                                if (obj != null)
                                {
                                    testMethod.DepartmentChains.Remove(obj);
                                }
                            }
                            if (departmentChainInfo.ResultApproval == true)
                            {
                                AnalysisDepartmentChain obj = testMethod.DepartmentChains.FirstOrDefault(i => i.Employee.Oid == oid && i.ResultApproval == true);
                                if (obj != null)
                                {
                                    testMethod.DepartmentChains.Remove(obj);
                                }
                            }
                        }
                        foreach (Guid oid in lstSelEmpOid)
                        {
                            if (departmentChainInfo.ResultEntry == true)
                            {
                                AnalysisDepartmentChain obj = testMethod.DepartmentChains.FirstOrDefault(i => i.Employee.Oid == oid && i.ResultEntry == true);
                                if (obj == null)
                                {
                                    obj = new AnalysisDepartmentChain(uow);
                                    obj.Employee = uow.GetObjectByKey<Employee>(oid);
                                    obj.ResultEntry = true;
                                    testMethod.DepartmentChains.Add(obj);
                                }
                            }
                            if (departmentChainInfo.ResultValidation == true)
                            {
                                AnalysisDepartmentChain obj = testMethod.DepartmentChains.FirstOrDefault(i => i.Employee.Oid == oid && i.ResultValidation == true);
                                if (obj == null)
                                {
                                    obj = new AnalysisDepartmentChain(uow);
                                    obj.Employee = uow.GetObjectByKey<Employee>(oid);
                                    obj.ResultValidation = true;
                                    testMethod.DepartmentChains.Add(obj);
                                }
                            }
                            if (departmentChainInfo.ResultApproval == true)
                            {
                                AnalysisDepartmentChain obj = testMethod.DepartmentChains.FirstOrDefault(i => i.Employee.Oid == oid && i.ResultApproval == true);
                                if (obj == null)
                                {
                                    obj = new AnalysisDepartmentChain(uow);
                                    obj.Employee = uow.GetObjectByKey<Employee>(oid);
                                    obj.ResultApproval = true;
                                    testMethod.DepartmentChains.Add(obj);
                                }
                            }
                        }
                        uow.CommitChanges();

                    }

                }
                View.ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                Frame.GetController<ExceptionTrackingViewController>().InsertException(ex.Message, ex.StackTrace, this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, View.Id);
                Application.ShowViewStrategy.ShowMessage(ex.Message, InformationType.Error, timer.Seconds, InformationPosition.Top);
            }
        }
    }
}
